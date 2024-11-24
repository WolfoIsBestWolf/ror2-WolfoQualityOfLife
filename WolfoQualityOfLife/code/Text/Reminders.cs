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

        public static void UpdateHuds()
        {          
            float spacing = WConfig.cfgObjectiveHeight.Value; //Default 32
            float fonzSize = WConfig.cfgObjectiveFontSize.Value; //Default 12
            Debug.Log("Updating Hud spacing"+WConfig.cfgObjectiveHeight.Value+" / font"+ WConfig.cfgObjectiveFontSize.Value);

            HGTextMeshProUGUI text;
            Transform ObjectivePannelRoot;

            UISkinData fontTwo = Addressables.LoadAssetAsync<UISkinData>(key: "RoR2/Base/UI/skinObjectivePanel.asset").WaitForCompletion();
            fontTwo.bodyTextStyle.fontSize = fonzSize;


            GameObject Hud = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ClassicRun/ClassicRunInfoHudPanel.prefab").WaitForCompletion();
            ObjectivePannelRoot = Hud.transform.GetChild(5).GetChild(1);
            ObjectivePannelRoot.GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;
            ObjectivePannelRoot.GetChild(1).GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;
            text = ObjectivePannelRoot.GetChild(1).GetChild(0).GetChild(0).GetComponent<HGTextMeshProUGUI>();
            text.fontSizeMin = fonzSize / 2;
            text.fontSizeMax = fonzSize;
            text.fontSize = fonzSize;

            Hud = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerUI.prefab").WaitForCompletion();
            ObjectivePannelRoot = Hud.transform.GetChild(5).GetChild(1);
            ObjectivePannelRoot.GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;
            ObjectivePannelRoot.GetChild(1).GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;
            text = ObjectivePannelRoot.GetChild(1).GetChild(0).GetChild(0).GetComponent<HGTextMeshProUGUI>();
            text.fontSizeMin = fonzSize / 2;
            text.fontSizeMax = fonzSize;
            text.fontSize = fonzSize;

            if (HUD.instancesList.Count > 0)
            {
                Hud = HUD.instancesList[0].gameModeUiInstance;


                ObjectivePannelRoot = Hud.transform.GetChild(5).GetChild(1);
                ObjectivePannelRoot.GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;

                ObjectivePannelRoot = ObjectivePannelRoot.GetChild(1);
                for(int i = 0; i < ObjectivePannelRoot.childCount; i++)
                {
                    ObjectivePannelRoot.GetChild(i).GetComponent<LayoutElement>().preferredHeight = spacing;
                    text = ObjectivePannelRoot.GetChild(i).GetChild(0).GetComponent<HGTextMeshProUGUI>();
                    text.fontSizeMin = fonzSize / 2;
                    text.fontSizeMax = fonzSize;
                    text.fontSize = fonzSize;
                }
            }

        }






        public static void Start()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += KeyReminderUpdater;
            On.RoR2.MultiShopController.OnPurchase += FreeChestReminderUpdater;
            //IL.RoR2.PurchaseInteraction.OnInteractionBegin += SaleStarReminderRemover;

            UpdateHuds();

            //If you scrap it the reminder should vanish
            //But what if you see the reminder and get another key to unlock the box?

            //If you void it it's just gone so that's fine
            //How would I check this in multiplayer I guess?

            On.RoR2.ScrapperController.BeginScrapping += RemoveReminders_Scrapper;
            On.RoR2.CharacterMasterNotificationQueue.SendTransformNotification_CharacterMaster_ItemIndex_ItemIndex_TransformationType += RemoveReminders_ItemTransform;

            if (WConfig.cfgRemindersTreasure.Value)
            {
                On.RoR2.HoldoutZoneController.PreStartClient += CheckForFreeChestVoid;
                On.RoR2.HoldoutZoneController.OnEnable += CheckForFreeChestVoidAcitvate;


                

                On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionEntityStates.OnEnter += GeodeSecretMissionEntityStates_OnEnter;
                On.RoR2.GeodeSecretMissionController.AdvanceGeodeSecretMission += GeodeSecretMissionController_AdvanceGeodeSecretMission;
                On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionRewardState.OnEnter += GeodeSecretMissionRewardState_OnEnter;


                On.EntityStates.Missions.BrotherEncounter.PreEncounter.OnEnter += ClearTreasure_OnBrother;
                On.EntityStates.VoidRaidCrab.EscapeDeath.OnExit += ClearTreasure_OnVoidlingDeath;
                EntityStates.MeridianEvent.MeridianEventStart.OnMeridianEventStart += ClearReminders_OnFalseSon;
            }
            
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

            HalcyoniteObjective.Start();
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
                    if (reminder && reminder.lockboxInfo)
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
                    if (reminder && reminder.lockboxVoidInfo)
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
            Debug.LogWarning("Secret Geode Start");
            if (self.geodeSecretMissionController.geodeInteractionsTracker == 0)
            {
                string text = string.Format(Language.GetString("REMINDER_SECRET_GEODE"), 0, self.geodeSecretMissionController.numberOfGeodesNecessary);
                self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = text;
            }
        }

        private static void GeodeSecretMissionController_AdvanceGeodeSecretMission(On.RoR2.GeodeSecretMissionController.orig_AdvanceGeodeSecretMission orig, GeodeSecretMissionController self)
        {
            orig(self);
            Debug.LogWarning("Secret Geode Advance");
            if (self.gameObject.GetComponent<GenericObjectiveProvider>())
            {
                string text = string.Format(Language.GetString("REMINDER_SECRET_GEODE"), self.geodeInteractionsTracker, self.numberOfGeodesNecessary);
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

            public RoR2.GenericObjectiveProvider newtAltarInfo;

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
                if (!Run.instance.GetComponent<InfiniteTowerRun>())
                {
                    if (SceneInfo.instance && SceneInfo.instance.sceneDef.stageOrder > 5)
                    {
                        treasureReminder.saleStarBool = false;
                    }
                }
               
                //
                if (treasureReminder.lockboxVoidCount > 0)
                {
                    Debug.Log("TreasureCacheVoidCount " + treasureReminder.lockboxVoidCount);
                    string token = Language.GetString("REMINDER_KEYVOID");
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
                    string token = Language.GetString("REMINDER_KEY");
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
                    string token = Language.GetString("REMINDER_FREECHEST");
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
                    string token = Language.GetString("REMINDER_SALESTAR");
                    treasureReminder.saleStarInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.saleStarInfo.objectiveToken = token;
                }
                /*if (treasureReminder.freeChestVoidBool)
                {
                    Debug.Log("FreeChestVoid " + treasureReminder.freeChestVoidBool);
                    string token = Language.GetString("REMINDER_FREECHESTVOID");
                    treasureReminder.freeChestVoidInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.freeChestVoidInfo.objectiveToken = token;
                }*/
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
                if (this.lockboxInfo)
                {
                    if (keys == 0)
                    {
                        FailGenericObjective(this.lockboxInfo);
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

            public void RemoveSaleStar(CharacterBody body, CharacterMaster master)
            {
                if (saleStarInfo != null)
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
                            Object.Destroy(this.saleStarInfo);
                        }
                    }
                }
            }

            public void RemoveFreeChestVoid()
            {
                if (freeChestVoidInfo != null)
                {
                    Object.Destroy(this.freeChestVoidInfo);
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
