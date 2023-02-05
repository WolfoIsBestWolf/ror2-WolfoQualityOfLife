using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using RoR2.ExpansionManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WolfoQualityOfLife
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfoQualityOfLife", "WolfoQualityOfLife", "2.0.5")]
    [R2APISubmoduleDependency(nameof(ContentAddition), nameof(LoadoutAPI), nameof(PrefabAPI), nameof(LanguageAPI), nameof(ItemAPI))]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]


    public class GameplayQoL : BaseUnityPlugin
    {
        static readonly System.Random random = new System.Random();

        public static uint PrismaticTrialSeed = 1;
        public static bool PrismaticTrialExtender = false;

        public static EquipmentIndex[] bossAffixes = Array.Empty<EquipmentIndex>();

        public static void Awake()
        {
            GameModeStuff();
            BetterRedWhipCheck();
            Faster();
        }


        public static void Faster()
        {


            if (WolfoMain.FasterPrinter.Value == true)
            {
                On.RoR2.PurchaseInteraction.CreateItemTakenOrb += delegate (On.RoR2.PurchaseInteraction.orig_CreateItemTakenOrb orig, Vector3 effectOrigin, GameObject targetObject, ItemIndex itemIndex)
                {
                    if (!NetworkServer.active)
                    {
                        Debug.LogWarning("[Server] function 'System.Void RoR2.ScrapperController::CreateItemTakenOrb(UnityEngine.Vector3,UnityEngine.GameObject,RoR2.ItemIndex)' called on client");
                        return;
                    }
                    GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/ItemTakenOrbEffect");
                    RoR2.EffectData effectData = new RoR2.EffectData
                    {
                        origin = effectOrigin,
                        genericFloat = 0.75f,
                        genericUInt = (uint)(itemIndex + 1)
                    };
                    effectData.SetNetworkedObjectReference(targetObject);
                    RoR2.EffectManager.SpawnEffect(effectPrefab, effectData, true);
                };

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Duplicator").GetComponent<RoR2.EntityLogic.DelayedEvent>().enabled = false;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").GetComponent<RoR2.EntityLogic.DelayedEvent>().enabled = false;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary").GetComponent<RoR2.EntityLogic.DelayedEvent>().enabled = false;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild").GetComponent<RoR2.EntityLogic.DelayedEvent>().enabled = false;

                On.EntityStates.Duplicator.Duplicating.DropDroplet += (orig, self) =>
                {
                    orig(self);

                    if (NetworkServer.active)
                    {
                        self.outer.GetComponent<PurchaseInteraction>().Networkavailable = true;
                    }
                };

            }


            if (WolfoMain.FasterScrapper.Value == true)
            {
                On.RoR2.ScrapperController.CreateItemTakenOrb += delegate (On.RoR2.ScrapperController.orig_CreateItemTakenOrb orig, Vector3 effectOrigin, GameObject targetObject, ItemIndex itemIndex)
                {
                    if (!NetworkServer.active)
                    {
                        Debug.LogWarning("[Server] function 'System.Void RoR2.ScrapperController::CreateItemTakenOrb(UnityEngine.Vector3,UnityEngine.GameObject,RoR2.ItemIndex)' called on client");
                        return;
                    }
                    GameObject effectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/ItemTakenOrbEffect");
                    RoR2.EffectData effectData = new RoR2.EffectData
                    {
                        origin = effectOrigin,
                        genericFloat = 0.75f,
                        genericUInt = (uint)(itemIndex + 1)
                    };
                    effectData.SetNetworkedObjectReference(targetObject);
                    RoR2.EffectManager.SpawnEffect(effectPrefab, effectData, true);

                };
            }



            //SleepToIdle.playbackRate

            if (WolfoMain.FasterShrines.Value == true)
            {
                On.RoR2.ShrineBloodBehavior.AddShrineStack += (orig, self, activator) =>
                {
                    orig(self, activator);
                    if (NetworkServer.active)
                    {
                        self.refreshTimer = 1;
                    }
                };
                On.RoR2.ShrineBossBehavior.AddShrineStack += (orig, self, activator) =>
                {
                    orig(self, activator);
                    if (NetworkServer.active)
                    {
                        self.refreshTimer = 1;
                    }
                };
                On.RoR2.ShrineChanceBehavior.AddShrineStack += (orig, self, activator) =>
                {
                    orig(self, activator);
                    if (NetworkServer.active)
                    {
                        self.refreshTimer = 1;
                    }
                };
                On.RoR2.ShrineCombatBehavior.AddShrineStack += (orig, self, activator) =>
                {
                    orig(self, activator);
                    if (NetworkServer.active)
                    {
                        self.refreshTimer = 1;
                    }
                };
                On.RoR2.ShrineHealingBehavior.AddShrineStack += (orig, self, activator) =>
                {
                    orig(self, activator);
                    if (NetworkServer.active)
                    {
                        self.refreshTimer = 1;
                    }
                };
                On.RoR2.ShrineRestackBehavior.AddShrineStack += (orig, self, activator) =>
                {
                    orig(self, activator);
                    if (NetworkServer.active)
                    {
                        self.refreshTimer = 1;
                    }
                };

            }

        }



        public static void GameModeStuff()
        {


            GameEndingDef Ending = RoR2.LegacyResourcesAPI.Load<GameEndingDef>("GameEndingDefs/PrismaticTrialEnding");
            Ending.backgroundColor = new Color(0.7f, 0.3f, 0.7f, 0.615f);
            Ending.foregroundColor = new Color(0.9f, 0.6f, 0.9f, 0.833f);
            Ending.endingTextToken = "ACHIEVEMENT_COMPLETEPRISMATICTRIAL_NAME";

            if (WolfoMain.EclipseDifficultyAlways.Value == true)
            {

                DifficultyCatalog.standardDifficultyCount += RoR2.EclipseRun.maxEclipseLevel;

                On.RoR2.EclipseRun.GetEclipseDifficultyIndex += (orig, eclipseLevel) =>
                {
                    return (DifficultyIndex)(DifficultyCatalog.standardDifficultyCount - 9 + eclipseLevel);
                };
                On.RoR2.EclipseRun.GetEclipseLevelFromRuleBook += (orig, ruleBook) =>
                {
                    return ruleBook.FindDifficulty() - (DifficultyIndex)DifficultyCatalog.standardDifficultyCount + 9;
                };

            }

            On.RoR2.Run.ForceChoice_RuleChoiceMask_RuleChoiceMask_RuleChoiceDef += (orig, self, mustInclude, mustExclude, choiceDef) =>
            {
                //Debug.LogWarning(choiceDef);
                //Debug.LogWarning(choiceDef.globalName);

                if (self.name.StartsWith("WeeklyRun"))
                {
                    if (WolfoMain.PrismaticTrialsDIY.Value == true)
                    {
                        if (choiceDef != null && !choiceDef.globalName.StartsWith("Misc."))
                        {

                        }
                        else
                        {
                            orig(self, mustInclude, mustExclude, choiceDef);
                        }
                    }
                    else
                    {
                        if (choiceDef != null && choiceDef.globalName.StartsWith("Difficulty."))
                        {

                        }
                        else
                        {
                            orig(self, mustInclude, mustExclude, choiceDef);
                        }
                    }
                }
                else
                {
                    orig(self, mustInclude, mustExclude, choiceDef);
                }
            };

            if (WolfoMain.PrismaticTrialsOffline.Value == true)
            {

                On.RoR2.WeeklyRun.AdvanceStage += (orig, self, nextScene) =>
                {

                    if (self.stageClearCount == WolfoMain.PrismaticTrialsStageLimit.Value - 1 && SceneInfo.instance.countsAsStage)
                    {
                        self.BeginGameOver(RoR2Content.GameEndings.PrismaticTrialEnding);
                        return;
                    }
                    else if (self.stageClearCount == 1 && SceneInfo.instance.countsAsStage)
                    {
                        self.stageClearCount = 0;
                        PrismaticTrialExtender = true;
                    }
                    orig(self, nextScene);

                };
                On.RoR2.Run.AdvanceStage += (orig, self, nextScene) =>
                {
                    if (self.name.StartsWith("WeeklyRun(Clone)"))
                    {
                        if (PrismaticTrialExtender == true)
                        {
                            self.stageClearCount = 1;
                            PrismaticTrialExtender = false;
                        }
                    }

                    orig(self, nextScene);
                };

                On.RoR2.WeeklyRun.OnServerBossAdded += (orig, self, bossGroup, characterMaster) =>
                {
                    orig(self, bossGroup, characterMaster);

                    if (self.stageClearCount >= WolfoMain.PrismaticTrialsEliteBossCount.Value - 1)
                    {
                        if (characterMaster.inventory.GetEquipmentIndex() == EquipmentIndex.None)
                        {
                            var temparray = self.GetFieldValue<EquipmentIndex[]>("bossAffixes");
                            if (temparray.Length != 0)
                            {
                                //characterMaster.inventory.SetEquipmentIndex(temparray[random.Next(temparray.Length)]);
                                characterMaster.inventory.SetEquipment(new EquipmentState(temparray[random.Next(temparray.Length)], Run.FixedTimeStamp.negativeInfinity, 0), 0);

                            }
                        }
                        characterMaster.inventory.GiveItem(RoR2Content.Items.BoostHp, 5);
                        characterMaster.inventory.GiveItem(RoR2Content.Items.BoostDamage, 1);
                    }
                    else
                    {
                        if (characterMaster.inventory.GetItemCount(RoR2Content.Items.BoostHp) < 15)
                        {
                            characterMaster.inventory.SetEquipment(new EquipmentState(EquipmentIndex.None, Run.FixedTimeStamp.negativeInfinity, 0), 0);
                        }
                        characterMaster.inventory.RemoveItem(RoR2Content.Items.BoostHp, 5);
                        characterMaster.inventory.RemoveItem(RoR2Content.Items.BoostDamage, 1);
                    }
                };


                On.RoR2.WeeklyRun.Start += (orig, self) =>
                {
                    orig(self);
                    if (WolfoMain.PrismaticTrialsAllElites.Value != "Disabled")
                    {
                        self.SetFieldValue<EquipmentIndex[]>("bossAffixes", bossAffixes);
                    }
                    if (WolfoMain.PrismaticTrialsDIY.Value == true)
                    {
                        PrismaticTrialSeed = (uint)random.Next(999999999);
                    }
                };


                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/gamemodes/WeeklyRun").GetComponent<WeeklyRun>().userPickable = true;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/gamemodes/WeeklyRun").GetComponent<WeeklyRun>().crystalCount = WolfoMain.PrismaticTrialsCrystalsTotal.Value;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/gamemodes/WeeklyRun").GetComponent<WeeklyRun>().crystalsRequiredToKill = WolfoMain.PrismaticTrialsCrystalsNeeded.Value;

                On.RoR2.UI.WeeklyRunScreenController.OnEnable += (orig, self) =>
                {
                    Debug.Log("Disabled WeeklyRunScreenController.OnEnable");
                    Destroy(self.leaderboard.gameObject.transform.parent.parent.gameObject);
                    self.enabled = false;
                };
                On.RoR2.WeeklyRun.GetCurrentSeedCycle += (orig) =>
                {
                    Debug.Log("WeeklyRun.GetCurrentSeedCycle");
                    return PrismaticTrialSeed;
                };
                On.RoR2.WeeklyRun.ClientSubmitLeaderboardScore += (orig, self, runReport) =>
                {
                    Debug.Log("Disabled WeeklyRun.ClientSubmitLeaderboardScore");
                };
                On.RoR2.DisableIfGameModded.OnEnable += (orig, self) =>
                {
                    orig(self);
                    self.enabled = false;
                    self.gameObject.SetActive(true);
                    Debug.Log("Disabled DisableIfGameModded.OnEnable");
                };




                LanguageAPI.Add("TITLE_WEEKLY", "Prismatic Trials (Offline)", "en");
                //LanguageAPI.Add("TITLE_WEEKLY_DESC", "Compete in a fixed seed challenge with other players online.", "en");
                LanguageAPI.Add("TITLE_WEEKLY_DESC", "Play a Prismatic Trial with random artifacts and random stage order.", "en");
                //LanguageAPI.Add("TITLE_WEEKLY_START", "Start Prismatic Trials (Offline)", "en");
                LanguageAPI.Add("WEEKLY_RUN_NO_ENTRY", "You are not supposed to see this.", "en");
                LanguageAPI.Add("WEEKLY_RUN_NEXT_CYCLE_COUNTDOWN_FORMAT", "Prismatic Trials", "en");
                LanguageAPI.Add("WEEKLY_RUN_DESCRIPTION", "<style=cWorldEvent>'Offline Prismatic Trials'</style> allow you to play Prismatic Trials while using a modded version of the game. Your runs are not supposed to be recorded or be sent to any leaderboard.\n\nTo beat the <style=cWorldEvent>Trials</style>, you must break <style=cWorldEvent>Time Crystals</style> on each stage before activating the Teleporter. The Teleporter charges instantly when the boss is defeated.\n\nSeed gets randomized every time you enter the main menu, so you can repeat a specific seed as much as you like, as long as you don't leave this screen.", "en");
                LanguageAPI.Add("GAMEMODE_WEEKLY_RUN_NAME", "Prismatic Trials", "en");
            }

        }




        public static void MercGameplay()
        {
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += (orig, self) =>
            {
                if (self.isComboFinisher == true)
                {
                    self.ignoreAttackSpeed = true;

                }
                orig(self);
                /*
                if (self.durationBeforeInterruptable < 0.0378f)
                {
                    self.durationBeforeInterruptable = 0.0378f;
                }
                */
            };
            /*
           On.EntityStates.Merc.WhirlwindBase.OnEnter += (orig, self) =>
           {
               orig(self);
               //Debug.LogWarning(self.baseDuration);
               //Debug.LogWarning(self.duration);
               //self.duration = self.baseDuration;
               /*
               if (self.duration < 0.15f)
               {
                   self.duration = 0.15f;
               }
               */
            //Debug.LogWarning(self.GetFieldValue<float>("duration"));
            //};

            //Make this a config
            On.EntityStates.Merc.Uppercut.OnEnter += (orig, self) =>
            {
                orig(self);
                if (self.duration < 0.15f)
                {
                    self.duration = 0.15f;
                }
                //Debug.LogWarning(self.duration);
            };
        }


        public static void BetterRedWhipCheck()
        {

            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Huntress/AimArrowSnipe.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Engi/EngiBodyPlaceBubbleShield.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Mage/MageBodyFlyUp.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Merc/MercBodyAssaulter.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Merc/MercBodyFocusedAssault.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Toolbot/ToolbotBodyToolbotDash.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Loader/FireHook.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Loader/FireYankHook.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Croco/CrocoLeap.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Croco/CrocoChainableLeap.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Captain/PrepSupplyDrop.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/DLC1/Railgunner/RailgunnerBodyScopeLight.asset").WaitForCompletion().isCombatSkill = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/DLC1/Railgunner/RailgunnerBodyScopeHeavy.asset").WaitForCompletion().isCombatSkill = false;

        }

    }
}