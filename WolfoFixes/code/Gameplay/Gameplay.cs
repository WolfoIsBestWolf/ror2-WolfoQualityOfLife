using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace WolfoFixes
{
    public class Gameplay
    {
        public static void Start()
        {
            On.RoR2.SceneExitController.OnEnable += SceneExitController_OnEnable;
            On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
            IL.RoR2.Artifacts.SwarmsArtifactManager.OnSpawnCardOnSpawnedServerGlobal += VengenceAndEnemyGooboFix;
        }


        private static void VengenceAndEnemyGooboFix(MonoMod.Cil.ILContext il)
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




        public static void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo)
        {
            if ((damageInfo.damageType & DamageType.BonusToLowHealth) > 0UL)
            {
                damageInfo.damage *= Mathf.Lerp(3f, 1f, self.combinedHealthFraction);
                damageInfo.damageType &= ~DamageType.BonusToLowHealth;
            }
            orig(self, damageInfo);
        }

        private static void SceneExitController_OnEnable(On.RoR2.SceneExitController.orig_OnEnable orig, SceneExitController self)
        {
            orig(self);
            if (self.isColossusPortal && self.isAlternatePath)
            {
                self.destinationScene = self.GetDestinationSceneToPreload();
            }
        }
    }



}
