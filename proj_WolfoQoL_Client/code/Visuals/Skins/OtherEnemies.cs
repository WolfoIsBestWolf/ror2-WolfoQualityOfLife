using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using WolfoLibrary;

namespace WolfoQoL_Client.Skins
{
   
    public class OtherEnemies
    {

        public static void Start()
        {
            if (WConfig.SulfurPoolsSkin.Value)
            {
                GameObject BeetleBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Beetle/BeetleBody.prefab").WaitForCompletion();
                GameObject BeetleGuardBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/BeetleGuard/BeetleGuardBody.prefab").WaitForCompletion();
                GameObject BeetleQueenBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/BeetleQueen/BeetleQueen2Body.prefab").WaitForCompletion();

                BeetleBody.AddComponent<ChangeSkinOnStage>().sceneDef = SceneList.SulfurPools;
                BeetleGuardBody.AddComponent<ChangeSkinOnStage>().sceneDef = SceneList.SulfurPools;
                BeetleQueenBody.AddComponent<ChangeSkinOnStage>().sceneDef = SceneList.SulfurPools;

            }
            ChangeSkinOnStage GolemBody = Addressables.LoadAssetAsync<GameObject>(key: "f21a0264f74389246acbbc5e281092b1").WaitForCompletion().AddComponent<ChangeSkinOnStage>();
            GolemBody.sceneDef = SceneList.RootJungle;

            if (WConfig.cfgVoidAllyCyanEyes.Value)
            {
                Material voidAlly = Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Nullifier/matNullifierAlly.mat").WaitForCompletion();
                voidAlly.SetColor("_EmColor", new Color(0, 3, 2.25f, 1)); //1,1,1
                voidAlly = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC1/VoidJailer/matVoidJailerEyesAlly.mat").WaitForCompletion();
                voidAlly.SetColor("_EmColor", new Color(0f, 1f, 0.75f, 1)); //0.8706 0.4764 1 1
                voidAlly = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC1/VoidMegaCrab/matVoidMegaCrabAlly.mat").WaitForCompletion();
                voidAlly.SetColor("_EmColor", new Color(0f, 2f, 1f, 1f)); //0.7306 0 0.8208 1
            }

            ScorchlingController Scorchling = Addressables.LoadAssetAsync<GameObject>(key: "4f70f94ff7396dd43b72e0bd263f42a9").WaitForCompletion().GetComponent<ScorchlingController>();
            Scorchling.timeInAirToShutOffDustTrail = 9999;
        }

        public static Material matEliteBead;
        public static void CallLate()
        {
            if (WConfig.cfgDarkTwisted.Value)
            {
                //EliteRamp.AddRamp(DLC2Content.Elites.Aurelionite, Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/RampEliteAur.png"));

                EliteRamp.AddRamp(DLC2Content.Elites.Bead, Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/RampEliteBead.png"));
                //IL.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            }
 
            Addressables.LoadAssetAsync<EliteDef>(key: "RoR2/DLC1/edSecretSpeed.asset").WaitForCompletion().shaderEliteRampIndex = 0;


         
            if (WConfig.cfgTwistedFire.Value)
            {
                IL.RoR2.AffixBeadBehavior.Update += TwistedFire;
            }

            //On.RoR2.AffixBeadBehavior.OnEnable += TwistedSound;
            //On.RoR2.WwiseUtils.SoundbankLoader.Start += LoadFalseSonSoundsPermamently;

        }
        private static void LoadFalseSonSoundsPermamently(On.RoR2.WwiseUtils.SoundbankLoader.orig_Start orig, RoR2.WwiseUtils.SoundbankLoader self)
        {
            //HG.ArrayUtils.ArrayAppend(ref self.soundbankStrings, "char_Toolbot");
            HG.ArrayUtils.ArrayAppend(ref self.soundbankStrings, "Boss_FalseSon");
            orig(self);
        }
       

        private static void TwistedFire(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.Before,
                x => x.MatchStfld("RoR2.AffixBeadBehavior", "affixBeadWard"));

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdarg(0)))
            {
                c.EmitDelegate<System.Func<AffixBeadBehavior, AffixBeadBehavior>>((self) =>
                {
                    if (self.beadHolderVFX == null)
                    {
                        if (NetworkServer.active)
                        {
                            self.beadHolderVFX = UnityEngine.Object.Instantiate<GameObject>(self.beadHolderVFXReference);
                            self.beadHolderVFX.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(self.body.gameObject, "Head");
                            self.beadHolderVFX.transform.localScale *= self.body.modelLocator.modelScaleCompensation;
                        }
                    }
                    return self;
                });
            }
            else
            {
                WQoLMain.log.LogWarning("TwistedFire FAILED");
            }
        }

        private static void TwistedSound(On.RoR2.AffixBeadBehavior.orig_OnEnable orig, AffixBeadBehavior self)
        {
            orig(self);
            //Util.PlaySound("Play_boss_falseson_skill3plus_lunarGaze_end", self.gameObject);
            Util.PlaySound("Play_boss_falseson_skill4_primeDevastator_lightning_tether", self.gameObject);
        }

        private static void CharacterModel_UpdateOverlays(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("RoR2.DLC2Content/Buffs", "EliteBead"));

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdsfld("RoR2.CharacterModel", "lunarGolemShieldMaterial")))
            {
                c.EmitDelegate<System.Func<Material, Material>>((no) =>
                {
                    return matEliteBead;
                });
            }
            else
            {
                WQoLMain.log.LogWarning("CharacterModel_UpdateOverlays FAILED");
            }

        }

         



    }
    public class ChangeSkinOnStage : MonoBehaviour
    {
        public SceneDef sceneDef;
        public SceneIndex SceneIndex;
        public uint localSkinIndex = 1;
        public void Start()
        {
            //if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.MixEnemy))
            if (SceneCatalog.mostRecentSceneDef == sceneDef)
            {
                GetComponent<CharacterBody>().skinIndex = localSkinIndex;
            }
        }
    }

}