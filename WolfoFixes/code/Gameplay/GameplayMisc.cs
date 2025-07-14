using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    internal class GameplayMisc
    {
        public static void Start()
        {
            //Set destination beforehand so the process can't be skipped accidentally
            On.RoR2.SceneExitController.OnEnable += PathOfColossusSkipFix;
            IL.RoR2.Artifacts.SwarmsArtifactManager.OnSpawnCardOnSpawnedServerGlobal += SwarmsVengenceGooboFix;


            if (WConfig.cfgSlayerScale.Value)
            {
                //Is this even a bug, probably not is it.
                On.RoR2.HealthComponent.TakeDamageProcess += SlayerApplyingToProc;
            }

            //Helminth Roost should realistically be blocked from Stage 1 in WeeklyRun specifically
            //But both of these should be allowed for RandomStage order
            //At least 2 of my mods used it so i'm putting here.
            SceneDef scene = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC2/habitat/habitat.asset").WaitForCompletion();
            scene.validForRandomSelection = true;
            scene = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC2/helminthroost/helminthroost.asset").WaitForCompletion();
            scene.validForRandomSelection = true;

            //Halcyon Shrine drop table no longer split between NoSotS/YesSots just Any/YesSots which seems wrong.
            Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "e291748f54c927a47ad44789d295c39f").WaitForCompletion().bannedItemTags = new ItemTag[] { ItemTag.HalcyoniteShrine };


            //Needed for SimuAdds
            IL.RoR2.HealthComponent.ServerFixedUpdate += AllowGhostsToSuicideProperly;
        }

        public static void AllowGhostsToSuicideProperly(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.Before,
                x => x.MatchCall("RoR2.HealthComponent", "Suicide"));

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdarg(0)))
            {
                c.EmitDelegate<System.Func<HealthComponent, HealthComponent>>((stock) =>
                {
                    stock.health = 1;
                    return stock;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : HealthComponent_ServerFixedUpdateHealthComponent_Suicide1");
            }
        }


        private static void SwarmsVengenceGooboFix(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("RoR2.SpawnCard/SpawnResult", "spawnRequest"),
                x => x.MatchCallvirt("RoR2.DirectorCore", "TrySpawnObject")))
            {
                c.EmitDelegate<Func<SpawnCard.SpawnResult, SpawnCard.SpawnResult>>((result) =>
                {
                    if (result.spawnedInstance)
                    {
                        if (result.spawnRequest.spawnCard is MasterCopySpawnCard)
                        {
                            result.spawnRequest.spawnCard = MasterCopySpawnCard.FromMaster(result.spawnedInstance.GetComponent<CharacterMaster>(), true, true, null);
                        }
                    }
                    return result;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: SwarmsArtifactManager_OnSpawnCardOnSpawnedServerGlobal");
            }
        }




        public static void SlayerApplyingToProc(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            if ((damageInfo.damageType & DamageType.BonusToLowHealth) > 0UL)
            {
                damageInfo.damage *= Mathf.Lerp(3f, 1f, self.combinedHealthFraction);
                damageInfo.damageType &= ~DamageType.BonusToLowHealth;
            }
            orig(self, damageInfo);
        }

        private static void PathOfColossusSkipFix(On.RoR2.SceneExitController.orig_OnEnable orig, SceneExitController self)
        {
            orig(self);
            if (self.isColossusPortal && self.isAlternatePath)
            {
                self.destinationScene = self.GetDestinationSceneToPreload();
            }
        }
    }



}
