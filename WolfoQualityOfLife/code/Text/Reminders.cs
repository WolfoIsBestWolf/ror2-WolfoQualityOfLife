using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.UI;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.ComponentModel;
using System.Runtime.InteropServices;
using RoR2.UI;
using RoR2.UI.SkinControllers;
using System.Net.Http.Headers;
 
namespace WolfoQualityOfLife
{
    public class Reminders
    {
        public static void Start()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += KeyReminderUpdater;
            On.RoR2.MultiShopController.OnPurchase += FreeChestReminderUpdater;
            //IL.RoR2.PurchaseInteraction.OnInteractionBegin += SaleStarReminderRemover;

            UIBorders.UpdateHuds();

            //If you scrap it the reminder should vanish
            //But what if you see the reminder and get another key to unlock the box?

            //If you void it it's just gone so that's fine
            //How would I check this in multiplayer I guess?

            On.RoR2.ScrapperController.BeginScrapping += RemoveReminders_Scrapper;
            On.RoR2.CharacterMasterNotificationQueue.SendTransformNotification_CharacterMaster_ItemIndex_ItemIndex_TransformationType += RemoveReminders_ItemTransform;

            if (WConfig.cfgRemindersGeneral.Value)
            {
                On.RoR2.HoldoutZoneController.PreStartClient += CheckForFreeChestVoid;
                On.RoR2.HoldoutZoneController.OnEnable += CheckFor_NewtAndFreeChestVoidAcitvate;

                On.RoR2.TeleporterInteraction.Start += Clear_Newt_SpawnedWithBlue;
                On.RoR2.PortalStatueBehavior.GrantPortalEntry += Newt_ReminderClear;

                On.RoR2.CharacterMasterNotificationQueue.PushItemTransformNotification += Clear_RegenScrap;

                On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionEntityStates.OnEnter += GeodeSecretMissionEntityStates_OnEnter;
                On.RoR2.GeodeSecretMissionController.AdvanceGeodeSecretMission += GeodeSecretMissionController_AdvanceGeodeSecretMission;
                On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionRewardState.OnEnter += GeodeSecretMissionRewardState_OnEnter;

                On.EntityStates.Missions.BrotherEncounter.PreEncounter.OnEnter += ClearTreasure_OnBrother;
                On.EntityStates.VoidRaidCrab.EscapeDeath.OnExit += ClearTreasure_OnVoidlingDeath;
                EntityStates.MeridianEvent.MeridianEventStart.OnMeridianEventStart += ClearReminders_OnFalseSon;

               

                if (WConfig.cfgRemindersPortal.Value == true)
                {
                    LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArtifactworld").AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_ARTIFACT";
                    LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalGoldshores").AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_GOLD";
                    LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalShop").AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_LUNAR";
                    LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalMS").AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_MS";
                    Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_VOID";
                    Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "OBJECTIVE_VOID_DEEP_PORTAL";

                    Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_GREEN";

                    //Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsHealing>Green Portal</style>";
                    //Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PM DestinationPortal.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsHealing>Destination Portal</style>";

                    On.RoR2.SceneExitController.Begin += (orig, self) =>
                    {
                        orig(self);
                        RoR2.Chat.SendBroadcastChat(new DestroyPortalReminderClients
                        {
                            portalObject = self.gameObject
                        });
                    };

                }
            }
            
            

            HalcyoniteObjective.Start();
        }

        private static void Clear_RegenScrap(On.RoR2.CharacterMasterNotificationQueue.orig_PushItemTransformNotification orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            orig(characterMaster, oldIndex, newIndex, transformationType);
            if (transformationType == CharacterMasterNotificationQueue.TransformationType.Default)
            {
                if (newIndex == DLC1Content.Items.RegeneratingScrapConsumed.itemIndex)
                {
                    TreasureReminder treasure = Run.instance.gameObject.GetComponent<TreasureReminder>();
                    if (treasure)
                    {
                        Object.Destroy(treasure.Objective_RegenScrap);
                    }
                }
            }
        }

        private static void Clear_Newt_SpawnedWithBlue(On.RoR2.TeleporterInteraction.orig_Start orig, TeleporterInteraction self)
        {
            orig(self);
            if (self.shouldAttemptToSpawnShopPortal)
            {
                TreasureReminder treasure = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (treasure)
                {
                    Object.Destroy(treasure.Objective_NewtShrine);
                }
            }
        }

        private static void Newt_ReminderClear(On.RoR2.PortalStatueBehavior.orig_GrantPortalEntry orig, PortalStatueBehavior self)
        {
            orig(self);
            if (self.portalType == PortalStatueBehavior.PortalType.Shop)
            {
                TreasureReminder treasure = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (treasure)
                {
                    Object.Destroy(treasure.Objective_NewtShrine);
                }
            }
        }

        private static void RemoveReminders_ItemTransform(On.RoR2.CharacterMasterNotificationQueue.orig_SendTransformNotification_CharacterMaster_ItemIndex_ItemIndex_TransformationType orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            orig(characterMaster, oldIndex, newIndex, transformationType);


            if (transformationType == CharacterMasterNotificationQueue.TransformationType.ContagiousVoid && oldIndex == RoR2Content.Items.TreasureCache.itemIndex)
            {
                TreasureReminder.CheckKeysVoided();               
            }
            else if (oldIndex == DLC2Content.Items.LowerPricedChests.itemIndex)
            {
                Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                {
                    baseToken = "UPDATE_SALESTAR",
                    subjectAsCharacterBody = characterMaster.GetBody(),
                });
            }
        }
 

        private static void RemoveReminders_Scrapper(On.RoR2.ScrapperController.orig_BeginScrapping orig, ScrapperController self, int intPickupIndex)
        {
            orig(self, intPickupIndex);

            PickupDef pickupDef = PickupCatalog.GetPickupDef(new PickupIndex(intPickupIndex));
            if (pickupDef != null)
            {
                if (pickupDef.itemIndex != ItemIndex.None)
                {
                    if (pickupDef.itemIndex == RoR2Content.Items.TreasureCache.itemIndex)
                    {
                        TreasureReminder.CheckKeysVoided();
                    }
                    else if (pickupDef.itemIndex == DLC2Content.Items.LowerPricedChests.itemIndex)
                    {
                        CharacterBody component = self.interactor.GetComponent<CharacterBody>();
                        Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                        {
                            baseToken = "UPDATE_SALESTAR",
                            subjectAsCharacterBody = component,
                        });
                        //TreasureReminder.ReCheckForReminders();
                    }
                }
            }
        }

        private static void CheckFor_NewtAndFreeChestVoidAcitvate(On.RoR2.HoldoutZoneController.orig_OnEnable orig, HoldoutZoneController self)
        {
            orig(self);
            TreasureReminder treasure = Run.instance.gameObject.GetComponent<TreasureReminder>();
            if (treasure)
            {
                if (self.name.StartsWith("VoidShellBattery"))
                {
                    if (treasure.Objective_FreeChestVVVoid)
                    {
                        Object.Destroy(treasure.Objective_FreeChestVVVoid);
                    }
                }
                if (self.GetComponent<TeleporterInteraction>())
                {
                    FailGenericObjective(treasure.Objective_NewtShrine);
                }
            }
            
        }

        private static void CheckForFreeChestVoid(On.RoR2.HoldoutZoneController.orig_PreStartClient orig, HoldoutZoneController self)
        {
            orig(self);
            if (self.name.StartsWith("VoidShellBattery"))
            {
                TreasureReminder treasureReminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (treasureReminder)
                {
                    Debug.Log("FreeChestVoid " + treasureReminder.freeChestVoidBool);
                    string token = Language.GetString("REMINDER_FREECHESTVOID");
                    treasureReminder.Objective_FreeChestVVVoid = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.Objective_FreeChestVVVoid.objectiveToken = token;
                }
            }
        }

        private static void ClearTreasure_OnVoidlingDeath(On.EntityStates.VoidRaidCrab.EscapeDeath.orig_OnExit orig, EntityStates.VoidRaidCrab.EscapeDeath self)
        {
            orig(self);
            TreasureReminder reminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
            FailAllReminders(reminder);
        }

        private static void ClearTreasure_OnBrother(On.EntityStates.Missions.BrotherEncounter.PreEncounter.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.PreEncounter self)
        {
            orig(self);
            TreasureReminder reminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
            FailAllReminders(reminder);
        }


        public static void FailAllReminders(TreasureReminder reminder)
        {
            if (!reminder)
            {
                return;
            }
            if (reminder.Objective_Lockbox)
            {
                FailGenericObjective(reminder.Objective_Lockbox);
            }
            if (reminder.Objective_LockboxVoid)
            {
                FailGenericObjective(reminder.Objective_LockboxVoid);
            }
            if (reminder.Objective_FreeChest)
            {
                FailGenericObjective(reminder.Objective_FreeChest);
            }
            if (reminder.Objective_FreeChestVVVoid)
            {
                FailGenericObjective(reminder.Objective_FreeChestVVVoid);
            }
            if (reminder.Objective_SaleStar)
            {
                FailGenericObjective(reminder.Objective_SaleStar);
            }
        }

        public static void FailGenericObjective(GenericObjectiveProvider objective)
        {
            if (!objective)
            {
                Debug.LogWarning("Null Objective");
                return;
            }
            objective.markCompletedOnRetired = false;
            objective.objectiveToken = "<color=#808080><s>" + objective.objectiveToken + "</color></s>";
            objective.enabled = false;
        }

        private static void ClearReminders_OnFalseSon()
        {
            TreasureReminder reminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
            FailAllReminders(reminder);

            Debug.LogWarning("Secret Geode : End Early Boss");
            GameObject SecretMission = GameObject.Find("/Meridian_Halcyonite_Encounter");
            if (SecretMission)
            {
                GenericObjectiveProvider objective = SecretMission.GetComponent<GenericObjectiveProvider>();
                if (objective)
                {
                    FailGenericObjective(objective);
                }
            }
        }

        private static void FreeChestReminderUpdater(On.RoR2.MultiShopController.orig_OnPurchase orig, MultiShopController self, Interactor interactor, PurchaseInteraction purchaseInteraction)
        {
            orig(self, interactor, purchaseInteraction);
            //Debug.LogWarning(self);
            if (!self.available && self.name.StartsWith("FreeChestMultiShop"))
            {
                TreasureReminder reminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (reminder && reminder.Objective_FreeChest)
                {
                    if (WConfig.NotRequireByAll.Value)
                    {
                        reminder.DeductFreeChestCount();
                    }
                    else
                    {
                        Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                        {
                            baseToken = "UPDATE_FREECHESTCOUNT"
                        });
                    }
                }
            }
        }

        private static void KeyReminderUpdater(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            orig(self, activator);

            if (!self.available)
            {
                if (self.costType == CostTypeIndex.TreasureCacheItem)
                {
                    TreasureReminder reminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
                    if (reminder && reminder.Objective_Lockbox)
                    {
                        if (WConfig.NotRequireByAll.Value)
                        {
                            reminder.DeductLockboxCount();
                        }
                        else
                        {
                            Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                            {
                                baseToken = "UPDATE_LOCKBOXCOUNT"
                            });
                        }
                       
                    }
                }
                else if (self.costType == CostTypeIndex.TreasureCacheVoidItem)
                {
                    TreasureReminder reminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
                    if (reminder && reminder.Objective_LockboxVoid)
                    {
                        if (WConfig.NotRequireByAll.Value)
                        {
                            reminder.DeductLockboxVoidCount();
                        }
                        else
                        {
                            Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                            {
                                baseToken = "UPDATE_LOCKBOXVOIDCOUNT"
                            });
                        }
                    }
                }
            }
        }


        private static void GeodeSecretMissionRewardState_OnEnter(On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionRewardState.orig_OnEnter orig, EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionRewardState self)
        {
            orig(self);
            Debug.LogWarning("Secret Geode Reward");
            Object.Destroy(self.gameObject.GetComponent<GenericObjectiveProvider>());        
        }

        private static void GeodeSecretMissionEntityStates_OnEnter(On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionEntityStates.orig_OnEnter orig, EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionEntityStates self)
        {
            orig(self);
            if (WConfig.cfgRemindersSecretGeode.Value)
            {
                Debug.LogWarning("Secret Geode Start");
                if (self.geodeSecretMissionController.geodeInteractionsTracker == 0)
                {
                    string text = string.Format(Language.GetString("REMINDER_SECRET_GEODE"), 0, self.geodeSecretMissionController.numberOfGeodesNecessary);
                    self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = text;
                }
            }
            
        }

        private static void GeodeSecretMissionController_AdvanceGeodeSecretMission(On.RoR2.GeodeSecretMissionController.orig_AdvanceGeodeSecretMission orig, GeodeSecretMissionController self)
        {
            orig(self);
            Debug.LogWarning("Secret Geode Advance");
            GenericObjectiveProvider Objective = self.gameObject.GetComponent<GenericObjectiveProvider>();
            if (Objective)
            {
                string text = string.Format(Language.GetString("REMINDER_SECRET_GEODE"), self.geodeInteractionsTracker, self.numberOfGeodesNecessary);
                Objective.objectiveToken = text;
            }
        }
        
        private static void GeodeSecretMissionController_CheckIfRewardShouldBeGranted(On.RoR2.GeodeSecretMissionController.orig_CheckIfRewardShouldBeGranted orig, GeodeSecretMissionController self)
        {
            orig(self);
            if (self.numberOfGeodesNecessary <= self.geodeInteractionsTracker)
            {
                Object.Destroy(self.gameObject.GetComponent<GenericObjectiveProvider>());
                Debug.Log("Secret Geode End");
            }
        }

        private static void SaleStarReminderRemover(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Items", "LowerPricedChestsConsumed")))
            {
                c.EmitDelegate<System.Func<ItemDef, ItemDef>>((itemDef) =>
                {
                    TreasureReminder treasure = Run.instance.gameObject.GetComponent<TreasureReminder>();
                    if (treasure && treasure.Objective_SaleStar)
                    {
                        Object.Destroy(treasure.Objective_SaleStar);
                    }
                    return itemDef;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Sale Star Reminder");
            }
        }

        public class TreasureReminder : MonoBehaviour
        {
            public int lockboxCount = 0;
            public int lockboxVoidCount = 0;
            public int freeChestCount = 0;
            public bool freeChestVoidBool = false;
            public bool localHasSaleStar = false;
            public bool localHasRegenScrap = false;

            public GenericObjectiveProvider Objective_Lockbox;
            public GenericObjectiveProvider Objective_LockboxVoid;
            public GenericObjectiveProvider Objective_FreeChest;
            public GenericObjectiveProvider Objective_FreeChestVVVoid;
            public GenericObjectiveProvider Objective_SaleStar;
            public GenericObjectiveProvider Objective_RegenScrap;
            public GenericObjectiveProvider Objective_NewtShrine;


            public static void SetupReminders()
            {
               
                TreasureReminder treasureReminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (treasureReminder == null)
                {
                    treasureReminder = Run.instance.gameObject.AddComponent<TreasureReminder>();
                }
                treasureReminder.lockboxCount = 0;
                treasureReminder.lockboxVoidCount = 0;
                treasureReminder.freeChestCount = 0;
                //treasureReminder.freeChestVoidBool = false;
                treasureReminder.localHasSaleStar = false;
               


                int maximumKeys = 0;
                int maximumKeysVoid = 0;

                using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {                
                        if (enumerator.Current.networkUser.isLocalPlayer)
                        {
                            if (enumerator.Current.master.inventory.GetItemCount(DLC2Content.Items.LowerPricedChests) > 0) { treasureReminder.localHasSaleStar = true; }
                            if (enumerator.Current.master.inventory.GetItemCount(DLC2Content.Items.LowerPricedChestsConsumed) > 0) { treasureReminder.localHasSaleStar = true; }
                        }
                        maximumKeys += enumerator.Current.master.inventory.GetItemCount(RoR2Content.Items.TreasureCache);
                        maximumKeysVoid += enumerator.Current.master.inventory.GetItemCount(DLC1Content.Items.TreasureCacheVoid);
                    }
                }

                using (IEnumerator<CharacterMaster> enumerator = CharacterMaster.readOnlyInstancesList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current.inventory.GetItemCount(RoR2Content.Items.TreasureCache) > 0) { treasureReminder.lockboxCount++; }
                        if (enumerator.Current.inventory.GetItemCount(DLC1Content.Items.TreasureCacheVoid) > 0) { treasureReminder.lockboxVoidCount++; }
                        if (enumerator.Current.inventory.GetItemCount(DLC1Content.Items.FreeChest) > 0) { treasureReminder.freeChestCount++; }
                    }
                }

                if (treasureReminder.lockboxCount > maximumKeys)
                {
                    treasureReminder.lockboxCount = maximumKeys;
                }
                if (treasureReminder.lockboxVoidCount > maximumKeysVoid)
                {
                    treasureReminder.lockboxVoidCount = maximumKeysVoid;
                }
                if (!Run.instance.GetComponent<InfiniteTowerRun>())
                {
                    if (SceneInfo.instance && SceneInfo.instance.sceneDef.stageOrder > 5)
                    {
                        treasureReminder.localHasSaleStar = false;
                    }
                }

                if (WConfig.cfgRemindersKeys.Value)
                {
                    if (treasureReminder.lockboxVoidCount > 0)
                    {
                        Debug.Log("TreasureCacheVoidCount " + treasureReminder.lockboxVoidCount);
                        string token = Language.GetString("REMINDER_KEYVOID");
                        if (treasureReminder.lockboxVoidCount > 1)
                        {
                            token += " (" + treasureReminder.lockboxVoidCount + "/" + treasureReminder.lockboxVoidCount + ")";
                        }
                        treasureReminder.Objective_LockboxVoid = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                        treasureReminder.Objective_LockboxVoid.objectiveToken = token;
                    }
                    if (treasureReminder.lockboxCount > 0)
                    {
                        Debug.Log("TreasureCacheCount " + treasureReminder.lockboxCount);
                        string token = Language.GetString("REMINDER_KEY");
                        if (treasureReminder.lockboxCount > 1)
                        {
                            token += " (" + treasureReminder.lockboxCount + "/" + treasureReminder.lockboxCount + ")";
                        }
                        treasureReminder.Objective_Lockbox = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                        treasureReminder.Objective_Lockbox.objectiveToken = token;
                    }
                }
                if (WConfig.cfgRemindersFreechest.Value)
                {
                    if (treasureReminder.freeChestCount > 0)
                    {
                        Debug.Log("FreeChestCount " + treasureReminder.freeChestCount);
                        string token = Language.GetString("REMINDER_FREECHEST");
                        if (treasureReminder.freeChestCount > 1)
                        {
                            token += " (" + treasureReminder.freeChestCount + "/" + treasureReminder.freeChestCount + ")";
                        }
                        treasureReminder.Objective_FreeChest = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                        treasureReminder.Objective_FreeChest.objectiveToken = token;
                    }
                }
                if (WConfig.cfgRemindersSaleStar.Value)
                {
                    if (treasureReminder.localHasSaleStar)
                    {
                        Debug.Log("SaleStar " + treasureReminder.localHasSaleStar);
                        string token = Language.GetString("REMINDER_SALESTAR");
                        treasureReminder.Objective_SaleStar = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                        treasureReminder.Objective_SaleStar.objectiveToken = token;
                    }
                }
                if (WConfig.cfgRemindersRegenScrap.Value == WConfig.ReminderChoice.Always)
                {
                    treasureReminder.SetupRegenScrap(); 
                }
                
 
            }


            public void SetupRegenScrap()
            {
                localHasRegenScrap = false;
                if (WConfig.cfgRemindersRegenScrap.Value == WConfig.ReminderChoice.Off)
                {
                    return;
                }
                if (Objective_RegenScrap == null)
                {
                    using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.networkUser.isLocalPlayer)
                            {
                                if (enumerator.Current.master.inventory.GetItemCount(DLC1Content.Items.RegeneratingScrap) > 0) { localHasRegenScrap = true; }
                                if (enumerator.Current.master.inventory.GetItemCount(DLC1Content.Items.RegeneratingScrapConsumed) > 0) { localHasRegenScrap = true; }
                            }
                        }
                    }
                    if (localHasRegenScrap)
                    {
                        Debug.Log("RegenScrap Reminder");
                        string token = Language.GetString("REMINDER_REGENSCRAP");
                        Objective_RegenScrap = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                        Objective_RegenScrap.objectiveToken = token;
                    }
                }
                
            }


            public static void CheckKeysVoided()
            {
                int maximumKeys = 0;
                //int maximumKeysVoid = 0;

                using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        maximumKeys += enumerator.Current.master.inventory.GetItemCount(RoR2Content.Items.TreasureCache);
                        //maximumKeysVoid += enumerator.Current.master.inventory.GetItemCount(DLC1Content.Items.TreasureCacheVoid);
                    }
                }
                Debug.Log("Check if voided Lockbox reminder : " + maximumKeys);
                Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                {
                    baseToken = "CHECK_LOCKBOX_VOIDED",
                    keysPostVoid = maximumKeys,
                });
            }


            public void ShouldRemoveKeyObjective(int keys)
            {
                if (this.Objective_Lockbox)
                {
                    if (keys == 0)
                    {
                        FailGenericObjective(this.Objective_Lockbox);
                    }
                    else if (this.lockboxCount > keys)
                    {
                        for (int i = lockboxCount; i > keys; i--)
                        {
                            this.DeductLockboxCount();
                        }
                    }
                }
            }

            public void DeductLockboxCount()
            {
                if (Objective_Lockbox != null)
                {
                    Debug.Log("TreasureCacheCount " + lockboxCount);
                    if (lockboxCount > 0)
                    {
                        string old = "(" + lockboxCount + "/";
                        lockboxCount--;
                        string newstring = "(" + lockboxCount + "/";
                        Objective_Lockbox.objectiveToken = Objective_Lockbox.objectiveToken.Replace(old, newstring);
                    }
                    if (lockboxCount == 0)
                    {
                        Destroy(Objective_Lockbox);
                    }
                }
            }

            public void DeductLockboxVoidCount()
            {
                if (Objective_LockboxVoid != null)
                {
                    Debug.Log("TreasureCacheVoidCount " + lockboxVoidCount);
                    if (lockboxVoidCount > 0)
                    {
                        string old = "(" + lockboxVoidCount + "/";
                        lockboxVoidCount--;
                        string newstring = "(" + lockboxVoidCount + "/";
                        Objective_LockboxVoid.objectiveToken = Objective_LockboxVoid.objectiveToken.Replace(old, newstring);
                    }
                    if (lockboxVoidCount == 0)
                    {
                        Destroy(Objective_LockboxVoid);
                    }
                }
            }

            public void DeductFreeChestCount()
            {
                if (Objective_FreeChest != null)
                {
                    Debug.Log("FreeChestCount " + freeChestCount);
                    if (freeChestCount > 0)
                    {
                        string old = "(" + freeChestCount + "/";
                        freeChestCount--;
                        string newstring = "(" + freeChestCount + "/";
                        Objective_FreeChest.objectiveToken = Objective_FreeChest.objectiveToken.Replace(old, newstring);
                    }
                    if (freeChestCount == 0)
                    {
                        Destroy(Objective_FreeChest);
                    }
                }
            }

            public void RemoveSaleStar(CharacterBody body, CharacterMaster master)
            {
                if (Objective_SaleStar != null)
                {
                    PlayerCharacterMasterController player;
                    if (body && body.master)
                    {
                        master = body.master;
                    }
                    if (!master)
                    {
                        Debug.LogWarning("Remove Sale Star Objective No master");
                    }
                    if (master && master.TryGetComponent<PlayerCharacterMasterController>(out player))
                    {
                        if (player.networkUser.isLocalPlayer)
                        {
                            Object.Destroy(this.Objective_SaleStar);
                        }
                    }
                }
            }

            public void RemoveFreeChestVoid()
            {
                if (Objective_FreeChestVVVoid != null)
                {
                    Object.Destroy(this.Objective_FreeChestVVVoid);
                }
            }

        }


        public class GreenPrinterReminder : MonoBehaviour
        {
            public void OnEnable()
            {
                TreasureReminder treasure = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (treasure)
                {
                    treasure.SetupRegenScrap();
                }
            }
        }


        public class UpdateTreasureReminderCounts : RoR2.SubjectChatMessage
        {
            public override string ConstructChatString()
            {
                TreasureReminder treasureReminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (treasureReminder != null)
                {
                    switch (baseToken)
                    {
                        case "UPDATE_LOCKBOXCOUNT":
                            treasureReminder.DeductLockboxCount();
                            break;
                        case "UPDATE_LOCKBOXVOIDCOUNT":
                            treasureReminder.DeductLockboxVoidCount();
                            break;
                        case "UPDATE_FREECHESTCOUNT":
                            treasureReminder.DeductFreeChestCount();
                            break;
                        case "UPDATE_SALESTAR":
                            treasureReminder.RemoveSaleStar(subjectAsCharacterBody, null);
                            break;
                        case "UPDATE_REGENSCRAP":
                            treasureReminder.RemoveSaleStar(subjectAsCharacterBody, null);
                            break;
                        case "UPDATE_NEWT":
                            treasureReminder.RemoveSaleStar(subjectAsCharacterBody, null);
                            break;
                        case "CHECK_LOCKBOX_VOIDED":
                            treasureReminder.ShouldRemoveKeyObjective(keysPostVoid);
                            break;
                    }
                }
                return null;
            }

            public int keysPostVoid;
            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(keysPostVoid);

            }

            public override void Deserialize(NetworkReader reader)
            {
                base.Deserialize(reader);
                keysPostVoid = reader.ReadInt32();
            }
        }

        public class DestroyPortalReminderClients : RoR2.ChatMessageBase
        {
            public override string ConstructChatString()
            {
                GenericObjectiveProvider objective = portalObject.GetComponent<GenericObjectiveProvider>();
                if (objective)
                {
                    Object.Destroy(objective);
                }
                else
                {
                    Debug.LogWarning("Sent DestroyPortalReminderClients without portalObject");
                }
                return null;
            }

            public GameObject portalObject;
            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(portalObject);

            }

            public override void Deserialize(NetworkReader reader)
            {
                base.Deserialize(reader);
                portalObject = reader.ReadGameObject();
            }

        }
    }
}
