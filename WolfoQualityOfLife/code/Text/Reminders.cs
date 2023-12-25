using RoR2;
//using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQualityOfLife
{
    public class Reminders
    {
        public static void Start()
        {
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

            if (WConfig.cfgRemindersPortal.Value == true)
            {
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArtifactworld").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cArtifact>Artifact Portal</style>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalGoldshores").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#FFE880>Gold Portal</color>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalShop").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsLunar>Blue Portal</style>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalMS").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#A0FFD6>Celestial Portal</color>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsVoid>Void Portal</style>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "OBJECTIVE_VOID_DEEP_PORTAL";

                On.RoR2.SceneExitController.Begin += (orig, self) =>
                {
                    orig(self);
                    if (self.exitState == SceneExitController.ExitState.ExtractExp || self.exitState == SceneExitController.ExitState.TeleportOut)
                    {
                        GenericObjectiveProvider objective = self.GetComponent<GenericObjectiveProvider>();
                        if (objective)
                        {
                            Object.Destroy(objective);
                        }
                    }
                };

                On.RoR2.SceneExitController.SetState += (orig, self, newState) =>
                {
                    orig(self, newState);
                    if (newState == SceneExitController.ExitState.ExtractExp || newState == SceneExitController.ExitState.TeleportOut)
                    {
                        GenericObjectiveProvider objective = self.GetComponent<GenericObjectiveProvider>();
                        if (objective)
                        {
                            Object.Destroy(objective);
                        }
                    }
                };
            }
        }



        public class TreasureReminder : MonoBehaviour
        {
            public int lockboxCount = 0;
            public int lockboxVoidCount = 0;
            public int freeChestCount = 0;

            public RoR2.GenericObjectiveProvider lockboxInfo;
            public RoR2.GenericObjectiveProvider lockboxVoidInfo;
            public RoR2.GenericObjectiveProvider freeChestInfo;

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

        //This is like really fucking stupid
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

    }

}
