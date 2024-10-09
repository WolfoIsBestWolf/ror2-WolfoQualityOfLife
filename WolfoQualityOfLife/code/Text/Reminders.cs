using RoR2;
//using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace WolfoQualityOfLife
{
    public class Reminders
    {
        public static void Start()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += KeyReminderUpdater;
            On.RoR2.MultiShopController.OnPurchase += FreeChestReminderUpdater;
            IL.RoR2.PurchaseInteraction.OnInteractionBegin += SaleStarReminderRemover;

            if (WConfig.cfgRemindersTreasure.Value)
            {
                On.RoR2.HoldoutZoneController.PreStartClient += CheckForFreeChestVoid;
                On.RoR2.HoldoutZoneController.OnEnable += CheckForFreeChestVoidAcitvate;


                //Objectives spacing
                GameObject Hud = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ClassicRun/ClassicRunInfoHudPanel.prefab").WaitForCompletion();
                Hud.transform.GetChild(5).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 28; //Objective label
                Hud.transform.GetChild(5).GetChild(1).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 30;

                Hud = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerUI.prefab").WaitForCompletion();
                Hud.transform.GetChild(5).GetChild(1).GetChild(1).GetChild(0).GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 30;


                On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionEntityStates.OnEnter += GeodeSecretMissionEntityStates_OnEnter;
                On.RoR2.GeodeSecretMissionController.AdvanceGeodeSecretMission += GeodeSecretMissionController_AdvanceGeodeSecretMission;
                On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionRewardState.OnEnter += GeodeSecretMissionRewardState_OnEnter;


                On.EntityStates.Missions.BrotherEncounter.PreEncounter.OnEnter += ClearTreasure_OnBrother;
                On.EntityStates.VoidRaidCrab.EscapeDeath.OnExit += ClearTreasure_OnVoidlingDeath;
                MeridianEventTriggerInteraction.MeridianEventStart.OnMeridianEventStart += ClearReminders_OnFalseSon;
            }

            if (WConfig.cfgRemindersPortal.Value == true)
            {
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArtifactworld").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cDeath>Artifact Portal</style>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalGoldshores").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#FFE880>Gold Portal</color>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalShop").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsLunar>Blue Portal</style>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalMS").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#7CFEF3>Celestial Portal</color>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsVoid>Void Portal</style>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "OBJECTIVE_VOID_DEEP_PORTAL";

                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#7CFE7C>Green Portal</color>";
               
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

            

            R2API.LanguageAPI.Add("OBJECTIVE_CHARGE_HALCSHRINE", "Charge the <color=#FFE880>Halcyon Shrine</color> ({0}%)", "en");
            R2API.LanguageAPI.Add("OBJECTIVE_KILL_HALCSHRINE", "Defeat the <color=#FFE880>Guardian</color>", "en");

            bool otherMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Gorakh.NoMoreMath");

            if (!otherMod && WConfig.cfgChargeHalcyShrine.Value)
            {
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState.OnEnter += AddObjective_ShrineHalcyoniteActivatedState_OnEnter;
                On.RoR2.HalcyoniteShrineInteractable.StoreDrainValue += TrackObjective_HalcyoniteShrineInteractable_StoreDrainValue;
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality.OnEnter += ShrineHalcyoniteMaxQuality_OnEnter;
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished.OnEnter += RemoveAll_ShrineHalcyoniteFinished_OnEnter;
            }






             

        }



        private static void CheckForFreeChestVoidAcitvate(On.RoR2.HoldoutZoneController.orig_OnEnable orig, HoldoutZoneController self)
        {
            orig(self);
            if (self.name.StartsWith("VoidShellBattery"))
            {
                TreasureReminder treasure = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (treasure.freeChestVoidInfo)
                {
                    Object.Destroy(treasure.freeChestVoidInfo);
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
                    treasureReminder.freeChestVoidInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.freeChestVoidInfo.objectiveToken = token;
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
            if (reminder.lockboxInfo)
            {
                FailGenericObjective(reminder.lockboxInfo);
            }
            if (reminder.lockboxVoidInfo)
            {
                FailGenericObjective(reminder.lockboxVoidInfo);
            }
            if (reminder.freeChestInfo)
            {
                FailGenericObjective(reminder.freeChestInfo);
            }
            if (reminder.freeChestVoidInfo)
            {
                FailGenericObjective(reminder.freeChestVoidInfo);
            }
            if (reminder.saleStarInfo)
            {
                FailGenericObjective(reminder.saleStarInfo);
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
                if (reminder && reminder.freeChestInfo)
                {
                    RoR2.Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                    {
                        baseToken = "UPDATE_FREECHESTCOUNT"
                    });
                    //reminder.UpdateFreeChestCount();
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
                    if (reminder && reminder.lockboxInfo)
                    {
                        RoR2.Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                        {
                            baseToken = "UPDATE_LOCKBOXCOUNT"
                        });
                    }
                }
                else if (self.costType == CostTypeIndex.TreasureCacheVoidItem)
                {
                    TreasureReminder reminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
                    if (reminder && reminder.lockboxVoidInfo)
                    {
                        RoR2.Chat.SendBroadcastChat(new UpdateTreasureReminderCounts
                        {
                            baseToken = "UPDATE_LOCKBOXVOIDCOUNT"
                        });
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
            Debug.LogWarning("Secret Geode Start");
            if (self.geodeSecretMissionController.geodeInteractionsTracker == 0)
            {
                string text = string.Format(Language.GetString("OBJECTIVE_SECRET_GEODE"), 0, self.geodeSecretMissionController.numberOfGeodesNecessary);
                self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = text;
            }
        }

        private static void GeodeSecretMissionController_AdvanceGeodeSecretMission(On.RoR2.GeodeSecretMissionController.orig_AdvanceGeodeSecretMission orig, GeodeSecretMissionController self)
        {
            orig(self);
            Debug.LogWarning("Secret Geode Advance");
            if (self.gameObject.GetComponent<GenericObjectiveProvider>())
            {
                string text = string.Format(Language.GetString("OBJECTIVE_SECRET_GEODE"), self.geodeInteractionsTracker, self.numberOfGeodesNecessary);
                self.gameObject.GetComponent<GenericObjectiveProvider>().objectiveToken = text;
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


        private static void RemoveAll_ShrineHalcyoniteFinished_OnEnter(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished self)
        {
            orig(self);

            Object.Destroy(self.gameObject.GetComponent<GenericObjectiveProvider>());
        }

        private static void ShrineHalcyoniteMaxQuality_OnEnter(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality self)
        {
            orig(self);

            Object.Destroy(self.gameObject.GetComponent<GenericObjectiveProvider>());
            string text = Language.GetString("OBJECTIVE_KILL_HALCSHRINE");
            self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = text;
        }

        private static void AddObjective_ShrineHalcyoniteActivatedState_OnEnter(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState self)
        {
            orig(self);
            
            if (!self.gameObject.GetComponent<GenericObjectiveProvider>())
            {
                string text = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), 0);
                self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = text;
            }
        }

        private static void TrackObjective_HalcyoniteShrineInteractable_StoreDrainValue(On.RoR2.HalcyoniteShrineInteractable.orig_StoreDrainValue orig, HalcyoniteShrineInteractable self, int value)
        {
            orig(self, value);

            int chargeFrac = self.goldDrained*100 / self.maxGoldCost;
            string text = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), chargeFrac);
            self.gameObject.GetComponent<GenericObjectiveProvider>().objectiveToken = text;
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
                    if (treasure && treasure.saleStarInfo)
                    {
                        Object.Destroy(treasure.saleStarInfo);
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
            public bool saleStarBool = false;

            public RoR2.GenericObjectiveProvider lockboxInfo;
            public RoR2.GenericObjectiveProvider lockboxVoidInfo;
            public RoR2.GenericObjectiveProvider freeChestInfo;
            public RoR2.GenericObjectiveProvider freeChestVoidInfo;
            public RoR2.GenericObjectiveProvider saleStarInfo;

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
                treasureReminder.saleStarBool = false;

                int maximumKeys = 0;
                int maximumKeysVoid = 0;



                using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {                
                        if (enumerator.Current.networkUser.isLocalPlayer)
                        {                  
                            if (enumerator.Current.master.inventory.GetItemCount(DLC2Content.Items.LowerPricedChests) > 0) { treasureReminder.saleStarBool = true; }
                            if (enumerator.Current.master.inventory.GetItemCount(DLC2Content.Items.LowerPricedChestsConsumed) > 0) { treasureReminder.saleStarBool = true; }
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
                if (SceneInfo.instance && SceneInfo.instance.sceneDef.stageOrder >= 5)
                {
                    treasureReminder.saleStarBool = false;
                }
                //
                if (treasureReminder.lockboxVoidCount > 0)
                {
                    Debug.Log("TreasureCacheVoidCount " + treasureReminder.lockboxVoidCount);
                    string token = "Unlock the <color=#FF9EEC>Encrusted Lockbox</color>";
                    //string token = "Unlock the <color=#E59562>Encrusted Lockbox</color>";
                    if (treasureReminder.lockboxVoidCount > 1)
                    {
                        token += " (" + treasureReminder.lockboxVoidCount + "/" + treasureReminder.lockboxVoidCount + ")";
                    }
                    treasureReminder.lockboxVoidInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.lockboxVoidInfo.objectiveToken = token;
                }
                if (treasureReminder.lockboxCount > 0)
                {
                    Debug.Log("TreasureCacheCount " + treasureReminder.lockboxCount);
                    string token = "Unlock the <style=cisDamage>Rusty Lockbox</style>";
                    if (treasureReminder.lockboxCount > 1)
                    {
                        token += " (" + treasureReminder.lockboxCount + "/" + treasureReminder.lockboxCount + ")";
                    }
                    treasureReminder.lockboxInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.lockboxInfo.objectiveToken = token;
                }
                if (treasureReminder.freeChestCount > 0)
                {
                    Debug.Log("FreeChestCount " + treasureReminder.freeChestCount);
                    string token = "Collect free <style=cIsHealing>delivery</style>";
                    if (treasureReminder.freeChestCount > 1)
                    {
                        token += " (" + treasureReminder.freeChestCount + "/" + treasureReminder.freeChestCount + ")";
                    }
                    treasureReminder.freeChestInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.freeChestInfo.objectiveToken = token;
                }
                if (treasureReminder.saleStarBool)
                {
                    Debug.Log("SaleStar " + treasureReminder.saleStarBool);
                    string token = "Make use of <style=cIsUtility>Sale Star</style>";
                    treasureReminder.saleStarInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.saleStarInfo.objectiveToken = token;
                }
                if (treasureReminder.freeChestVoidBool)
                {
                    Debug.Log("FreeChestVoid " + treasureReminder.freeChestVoidBool);
                    string token = Language.GetString("REMINDER_FREECHESTVOID");
                    treasureReminder.freeChestVoidInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.freeChestVoidInfo.objectiveToken = token;
                }
            }

            public void DeductLockboxCount()
            {
                if (lockboxInfo != null)
                {
                    Debug.Log("TreasureCacheCount " + lockboxCount);
                    if (lockboxCount > 0)
                    {
                        string old = "(" + lockboxCount + "/";
                        lockboxCount--;
                        string newstring = "(" + lockboxCount + "/";
                        lockboxInfo.objectiveToken = lockboxInfo.objectiveToken.Replace(old, newstring);
                    }
                    if (lockboxCount == 0)
                    {
                        Destroy(lockboxInfo);
                    }
                }
            }

            public void DeductLockboxVoidCount()
            {
                if (lockboxVoidInfo != null)
                {
                    Debug.Log("TreasureCacheVoidCount " + lockboxVoidCount);
                    if (lockboxVoidCount > 0)
                    {
                        string old = "(" + lockboxVoidCount + "/";
                        lockboxVoidCount--;
                        string newstring = "(" + lockboxVoidCount + "/";
                        lockboxVoidInfo.objectiveToken = lockboxVoidInfo.objectiveToken.Replace(old, newstring);
                    }
                    if (lockboxVoidCount == 0)
                    {
                        Destroy(lockboxVoidInfo);
                    }
                }
            }

            public void DeductFreeChestCount()
            {
                if (freeChestInfo != null)
                {
                    Debug.Log("FreeChestCount " + freeChestCount);
                    if (freeChestCount > 0)
                    {
                        string old = "(" + freeChestCount + "/";
                        freeChestCount--;
                        string newstring = "(" + freeChestCount + "/";
                        freeChestInfo.objectiveToken = freeChestInfo.objectiveToken.Replace(old, newstring);
                    }
                    if (freeChestCount == 0)
                    {
                        Destroy(freeChestInfo);
                    }
                }
            }
        
        
        }

        public class UpdateTreasureReminderCounts : RoR2.Chat.SimpleChatMessage
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
                    }
                }
                return null;
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
