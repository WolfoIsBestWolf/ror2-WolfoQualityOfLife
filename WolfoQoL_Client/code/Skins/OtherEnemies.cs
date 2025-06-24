using MonoMod.Cil;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using WolfoFixes;
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


            if (WConfig.cfgOldSotsEliteIcons.Value)
            {
                UpdateSotsEliteIcon(null, null);
            }
            Addressables.LoadAssetAsync<EliteDef>(key: "RoR2/DLC1/edSecretSpeed.asset").WaitForCompletion().shaderEliteRampIndex = 0;

            if (WConfig.cfgTwistedFire.Value)
            {
                On.RoR2.AffixBeadBehavior.Update += AffixBeadBehavior_Update;
            }
            On.RoR2.AffixBeadBehavior.OnEnable += AffixBeadBehavior_OnEnable;
        }

        private static void AffixBeadBehavior_OnEnable(On.RoR2.AffixBeadBehavior.orig_OnEnable orig, AffixBeadBehavior self)
        {
            orig(self); 
            //Util.PlaySound("Play_boss_falseson_skill4_primeDevastator_impact", self.gameObject);
            Util.PlaySound("Play_boss_falseson_skill3plus_lunarGaze_end", self.gameObject);
            //Util.PlaySound("Play_boss_falseson_skill4_primeDevastator_cast", self.gameObject);
            //Util.PlaySound("Play_boss_falseson_skill4_primeDevastator_lightning_tether", self.gameObject);
             
           
        }
 
        private static void AffixBeadBehavior_Update(On.RoR2.AffixBeadBehavior.orig_Update orig, AffixBeadBehavior self)
        {
            if (!NetworkServer.active)
            {
                return;
            }
            bool flag = self.stack > 0;
            if (self.beadHolderVFX != flag)
            {
                if (flag)
                {
                    self.beadHolderVFX = UnityEngine.Object.Instantiate<GameObject>(self.beadHolderVFXReference);
                    self.beadHolderVFX.GetComponent<NetworkedBodyAttachment>().AttachToGameObjectAndSpawn(self.body.gameObject, "Head");
                    self.beadHolderVFX.transform.localScale *= self.body.modelLocator.modelScaleCompensation;
                }
            }
            orig(self);
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
                Debug.LogWarning("CharacterModel_UpdateOverlays FAILED");
            }

        }

        public static void UpdateSotsEliteIcon(object sender, System.EventArgs e)
        {
            if (WConfig.cfgOldSotsEliteIcons.Value)
            {
                DLC2Content.Buffs.EliteAurelionite.iconSprite = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC2/Elites/EliteAurelionite/texBuffAffixAureleonite.png").WaitForCompletion();
                DLC2Content.Buffs.EliteBead.iconSprite = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC2/Elites/EliteBead/texBuffAffixBead.png").WaitForCompletion();
            }
            else
            {
                DLC2Content.Buffs.EliteAurelionite.iconSprite = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC2/Elites/EliteAurelionite/texBuffEliteAurelioniteIcon.png").WaitForCompletion();
                DLC2Content.Buffs.EliteBead.iconSprite = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC2/Elites/EliteBead/texBuffEliteBeadIcon.png").WaitForCompletion();
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