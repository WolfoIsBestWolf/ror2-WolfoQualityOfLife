using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    public class ReStartParticleOnStart : MonoBehaviour
    {
        private void OnEnable()
        {
            if (particleSystem)
            {
                particleSystem.enableEmission = true;
            }
        }

        public ParticleSystem particleSystem;
    }
    internal class Visuals
    {
        public static void Start()
        {
            //Scope Alpha fix
            GameObject RailgunnerScopeHeavyOverlay = Addressables.LoadAssetAsync<GameObject>(key: "db5a0c21c1f689c4292ae5e292fd4f0e").WaitForCompletion();
            UnityEngine.UI.RawImage scope = RailgunnerScopeHeavyOverlay.transform.GetChild(1).GetComponent<UnityEngine.UI.RawImage>();
            scope.m_Color = scope.color.AlphaMultiplied(0.7f);
            GameObject RailgunnerScopeLightOverlay = Addressables.LoadAssetAsync<GameObject>(key: "c305c2dadaa35d840bd91dd48987c55e").WaitForCompletion();
            scope = RailgunnerScopeLightOverlay.transform.GetChild(1).GetComponent<UnityEngine.UI.RawImage>();
            scope.m_Color = scope.color.AlphaMultiplied(0.7f);

            //Rachis Radius is slightly wrong, noticible on high stacks 
            GameObject RachisObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/DamageZoneWard");
            RachisObject.transform.GetChild(1).GetChild(2).GetChild(1).localScale = new Vector3(2f, 2f, 2f);

            //Too small plant normally
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Plant/InterstellarDeskPlant.prefab").WaitForCompletion().transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);

            //Unused like blue explosion so he doesn't use magma explosion ig, probably unused for a reason but it looks fine
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion().GetComponent<WormBodyPositions2>().blastAttackEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Junk/ElectricWorm/ElectricWormImpactExplosion.prefab").WaitForCompletion();



            //2D beam fix
            GameObject DeepVoidPortalBattery = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBattery.prefab").WaitForCompletion();
            Transform Beam = DeepVoidPortalBattery.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0);
            Beam.localScale = new Vector3(1, 3, 1); //LongerBeam

            //2D beam fix
            ParticleSystem ps = Beam.GetComponent<ParticleSystem>();
            var psM = ps.main;
            //psM.startRotationX = 6.66f;
            psM.startRotationXMultiplier = 6.66f;
            psM.startRotation3D = true;

            //Glass when isGlass
            IL.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlayStates;
            IL.RoR2.CharacterModel.UpdateOverlayStates += CharacterModel_UpdateOverlayStates;

            //REX vine dissappears too fast
            GameObject EntangleOrbEffect = Addressables.LoadAssetAsync<GameObject>(key: "6e330e0a639bc3d4a9c1c282d70705b1").WaitForCompletion();
            AnimateShaderAlpha[] alphas = EntangleOrbEffect.transform.GetChild(0).GetComponents<AnimateShaderAlpha>();
            alphas[0].continueExistingAfterTimeMaxIsReached = false;
            alphas[1].continueExistingAfterTimeMaxIsReached = true;
            //EntangleOrbEffect.GetComponent<DetachParticleOnDestroyAndEndEmission>().enabled = false;



           /* bool otherGrandparent = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Moffein.RiskyFixes");
            //Grandparent invisible rock
            GameObject miniRock = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/GrandparentMiniBoulderGhost.prefab").WaitForCompletion();
            //MeshFilter mesh = miniRock.transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>();
            //mesh.sharedMesh = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/skymeadow/SMRockAngular.fbx").WaitForCompletion().GetComponent<MeshFilter>().mesh;
            miniRock.transform.GetChild(0).localScale *= 30;*/

            On.RoR2.DetachParticleOnDestroyAndEndEmission.OnDisable += RenableParticlesOnEnable;


        }

        private static void RenableParticlesOnEnable(On.RoR2.DetachParticleOnDestroyAndEndEmission.orig_OnDisable orig, DetachParticleOnDestroyAndEndEmission self)
        {
            orig(self);
            //Pooled effects get reused
            //So permamently disabling a particle effect means they dont show up again
            //And look wrong upon re-using
            //Small list but for the REX fix
            if (self.GetComponent<EffectComponent>())
            {
                if (!self.GetComponent<ReStartParticleOnStart>())
                {
                    self.gameObject.AddComponent<ReStartParticleOnStart>().particleSystem = self.particleSystem;
                }
            }

        }

        private static void CharacterModel_UpdateOverlayStates(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.Before,
            x => x.MatchLdsfld("RoR2.RoR2Content/Items", "LunarDagger"));
            if (a && c.TryGotoNext(MoveType.Before,
            x => x.MatchBr(out _)))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<bool, CharacterModel, bool>>((yes, model) =>
                {
                    if (model.body.isGlass)
                    {
                        return true;
                    }
                    return yes;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: IL.CharacterModel_UpdateOverlays");
            }
        }










    }

}