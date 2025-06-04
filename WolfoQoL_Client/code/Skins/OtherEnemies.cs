using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace WolfoQoL_Client
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
                SceneDef sulfurpools = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC1/sulfurpools/sulfurpools.asset").WaitForCompletion();
                //SceneIndex sulfur = SceneCatalog.FindSceneIndex("sulfurpools");

                BeetleBody.AddComponent<ChangeSkinOnStage>().sceneDef = sulfurpools;
                BeetleGuardBody.AddComponent<ChangeSkinOnStage>().sceneDef = sulfurpools;
                BeetleQueenBody.AddComponent<ChangeSkinOnStage>().sceneDef = sulfurpools;
 
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
                UpdateSotsEliteIcon(null,null);
            }
            Addressables.LoadAssetAsync<EliteDef>(key: "RoR2/DLC1/edSecretSpeed.asset").WaitForCompletion().shaderEliteRampIndex = 0;

            On.RoR2.CharacterModel.IsAurelioniteAffix += CharacterModel_IsAurelioniteAffix;
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


        private static bool CharacterModel_IsAurelioniteAffix(On.RoR2.CharacterModel.orig_IsAurelioniteAffix orig, CharacterModel self)
        {
            if (self.myEliteIndex == DLC2Content.Elites.Aurelionite.eliteIndex)
            {
                return true;
            }
            return orig(self);
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