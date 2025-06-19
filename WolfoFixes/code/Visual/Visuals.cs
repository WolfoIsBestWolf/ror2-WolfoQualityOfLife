using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Mono.Cecil.Cil;

namespace WolfoFixes
{
    public class Visuals
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

         
            //Default fallback
            On.RoR2.UI.ModelPanel.CameraFramingCalculator.GetCharacterThumbnailPosition += AddOrSetDefaultModelPanelParamsIfMissing;
            On.RoR2.ModelPanelParameters.OnDrawGizmos += SetValuesForMissingPanelParams;


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



        private static void AddOrSetDefaultModelPanelParamsIfMissing(On.RoR2.UI.ModelPanel.CameraFramingCalculator.orig_GetCharacterThumbnailPosition orig, RoR2.UI.ModelPanel.CameraFramingCalculator self, float fov)
        {
            ModelPanelParameters component = self.modelInstance.GetComponent<ModelPanelParameters>();
            if (component)
            {
                component.OnDrawGizmos();
            }
            orig(self, fov);

        }

        private static void SetValuesForMissingPanelParams(On.RoR2.ModelPanelParameters.orig_OnDrawGizmos orig, ModelPanelParameters self)
        {

            if (self.cameraPositionTransform == null && self.focusPointTransform == null)
            {
                GameObject cameraPos = new GameObject("cameraPos");
                cameraPos.transform.SetParent(self.transform, false);
                cameraPos.transform.localPosition = new Vector3(-1.5f, 1f, 3f);
                self.cameraPositionTransform = cameraPos.transform;

                GameObject focusPoint = new GameObject("focusPoint");
                focusPoint.transform.SetParent(self.transform, false);
                self.focusPointTransform = focusPoint.transform;
                self.minDistance = 2;
                self.maxDistance = 8;

                if (self.name.StartsWith("mdlNewt"))
                {
                    self.minDistance = 5;
                    self.maxDistance = 15;
                    cameraPos.transform.localPosition = new Vector3(-1.5f, 4f, 3f);
                    focusPoint.transform.localPosition = new Vector3(0f, 3f, 0f);
                }
                else if (self.GetComponent<CharacterModel>() != null)
                {
                    focusPoint.transform.localPosition = new Vector3(0f, 1f, 0f);
                }
            }
            orig(self);
        }

 






    }

}