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
            //if (WConfig.cfgRemindersTreasure.Value) ;

            On.RoR2.PurchaseInteraction.OnInteractionBegin += (orig, self, activator) =>
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
                            //reminder.UpdateLockboxCount();
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
                            //reminder.UpdateLockboxVoidCount();
                        }
                    }
                }
            };

            On.RoR2.MultiShopController.OnPurchase += (orig, self, interactor, purchaseInteraction) =>
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
            };

            IL.RoR2.PurchaseInteraction.OnInteractionBegin += SaleStarReminderRemover;

            if (WConfig.cfgRemindersTreasure.Value)
            {
                //Objectives spacing
                GameObject Hud = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ClassicRun/ClassicRunInfoHudPanel.prefab").WaitForCompletion();
                Hud.transform.GetChild(5).GetChild(1).GetChild(1).GetComponent<UnityEngine.UI.VerticalLayoutGroup>().spacing = -2;

            }

            if (WConfig.cfgRemindersPortal.Value == true)
            {
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArtifactworld").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cDeath>Artifact Portal</style>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalGoldshores").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#FFE880>Gold Portal</color>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalShop").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsLunar>Blue Portal</style>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalMS").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#A0FFD6>Celestial Portal</color>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsVoid>Void Portal</style>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "OBJECTIVE_VOID_DEEP_PORTAL";

                //Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#00D863>Green Portal</color>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsHealing>Green Portal</style>";
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

        private static void RemoveAll_ShrineHalcyoniteFinished_OnEnter(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished self)
        {
            orig(self);

            Object.Destroy(self.gameObject.gameObject.GetComponent<GenericObjectiveProvider>());
        }

        private static void ShrineHalcyoniteMaxQuality_OnEnter(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality self)
        {
            orig(self);

            Object.Destroy(self.gameObject.gameObject.GetComponent<GenericObjectiveProvider>());
            string text = Language.GetString("OBJECTIVE_KILL_HALCSHRINE");
            self.gameObject.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = text;
        }

        private static void AddObjective_ShrineHalcyoniteActivatedState_OnEnter(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState self)
        {
            orig(self);
            
            if (!self.gameObject.gameObject.GetComponent<GenericObjectiveProvider>())
            {
                string text = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), 0);
                self.gameObject.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = text;
            }
        }

        private static void TrackObjective_HalcyoniteShrineInteractable_StoreDrainValue(On.RoR2.HalcyoniteShrineInteractable.orig_StoreDrainValue orig, HalcyoniteShrineInteractable self, int value)
        {
            orig(self, value);

            int chargeFrac = self.goldDrained*100 / self.maxGoldCost;
            string text = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), chargeFrac);
            self.gameObject.gameObject.GetComponent<GenericObjectiveProvider>().objectiveToken = text;
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
            public int freeChestVoidCount = 0;
            public bool saleStar = false;

            public RoR2.GenericObjectiveProvider lockboxInfo;
            public RoR2.GenericObjectiveProvider lockboxVoidInfo;
            public RoR2.GenericObjectiveProvider freeChestInfo;
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
                treasureReminder.freeChestVoidCount = 0;
                treasureReminder.saleStar = false;



                if (SceneInfo.instance && SceneInfo.instance.sceneDef.stageOrder <= 5)
                {
                    using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.networkUser.isLocalPlayer)
                            {
                                if (enumerator.Current.master.inventory.GetItemCount(DLC2Content.Items.LowerPricedChests) > 0) { treasureReminder.saleStar = true; }
                                if (enumerator.Current.master.inventory.GetItemCount(DLC2Content.Items.LowerPricedChestsConsumed) > 0) { treasureReminder.saleStar = true; }
                            }
                        }
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
                if (treasureReminder.lockboxVoidCount > 0)
                {
                    Debug.Log("TreasureCacheVoidCount " + treasureReminder.lockboxVoidCount);
                    string token = "Unlock the <color=#FF9EEC>Encrusted Lockbox</color>";
                    if (treasureReminder.lockboxVoidCount > 1)
                    {
                        token += " (" + treasureReminder.lockboxVoidCount + "/" + treasureReminder.lockboxVoidCount + ")";
                    }
                    treasureReminder.lockboxVoidInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.lockboxVoidInfo.objectiveToken = token;
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
                if (treasureReminder.saleStar)
                {
                    Debug.Log("SaleStar " + treasureReminder.saleStar);
                    string token = "Make use of <style=cIsHealing>Sale Star</style>";
                    treasureReminder.saleStarInfo = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    treasureReminder.saleStarInfo.objectiveToken = token;
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
