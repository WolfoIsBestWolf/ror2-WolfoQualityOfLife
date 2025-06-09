using RoR2;
using UnityEngine;


namespace WolfoQoL_Client
{
    public class Reminders_Treasure
    {
        public static void Start()
        {
            //Key and VoidKey
            On.RoR2.PurchaseInteraction.OnInteractionBegin += KeyReminderUpdater_Host;

            //Free Chest
            On.RoR2.MultiShopController.OnPurchase += FreeChestReminderUpdater_Host;

            //RegenScrap and SaleStar
            On.RoR2.CharacterMasterNotificationQueue.PushItemTransformNotification += Clear_Star_Regen;

        }


        private static void Clear_Star_Regen(On.RoR2.CharacterMasterNotificationQueue.orig_PushItemTransformNotification orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            orig(characterMaster, oldIndex, newIndex, transformationType);
            /*Debug.Log("PUSH" +
                "\nC:" + characterMaster+
                "\no" + oldIndex+
                "\nn" + newIndex+
                "\nT" + transformationType
                );
            */
            if (!characterMaster.hasAuthority)
            {
                Debug.LogError("Not clearing reminders for " + Util.GetBestMasterName(characterMaster) + " because they aren't local.");
                return;
            }
            if (transformationType == CharacterMasterNotificationQueue.TransformationType.Default)
            {
                if (newIndex == DLC1Content.Items.RegeneratingScrapConsumed.itemIndex)
                {
                    if (TreasureReminder.instance)
                    {
                        Reminders.CompleteObjective(TreasureReminder.instance.Objective_RegenScrap);
                    }
                }
            }
            else if (transformationType == CharacterMasterNotificationQueue.TransformationType.SaleStarRegen)
            {
                if (newIndex == DLC2Content.Items.LowerPricedChestsConsumed.itemIndex)
                {
                    if (TreasureReminder.instance)
                    {
                        Reminders.CompleteObjective(TreasureReminder.instance.Objective_SaleStar);
                    }
                }
            }
            else if (transformationType == CharacterMasterNotificationQueue.TransformationType.ContagiousVoid && oldIndex == RoR2Content.Items.TreasureCache.itemIndex)
            {
                TreasureReminder.CheckKeysVoided();
            }
        }


        public static void FreeChestReminderUpdater_Host(On.RoR2.MultiShopController.orig_OnPurchase orig, MultiShopController self, Interactor interactor, PurchaseInteraction purchaseInteraction)
        {
            orig(self, interactor, purchaseInteraction);
            //Debug.LogWarning(self);
            if (!self.available && self.name.StartsWith("FreeChestMultiShop"))
            {
                if (TreasureReminder.instance)
                {
                    TreasureReminder.instance.DeductFreeChestCount();
                }
            }
        }

        public static void KeyReminderUpdater_Host(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            CharacterBody body = activator.GetComponent<CharacterBody>();
            int hadSale = body.inventory.GetItemCount(DLC2Content.Items.LowerPricedChests);
            orig(self, activator);
            if (hadSale > 0 && self.saleStarCompatible)
            {
                Chat.SendBroadcastChat(new SaleStarMessage
                {
                    interactableToken = self.displayNameToken,
                    subjectAsCharacterBody = body,
                });
            }
            if (self.costType == CostTypeIndex.TreasureCacheItem)
            {
                if (TreasureReminder.instance)
                {
                    TreasureReminder.instance.DeductLockboxCount();
                }
            }
            else if (self.costType == CostTypeIndex.TreasureCacheVoidItem)
            {
                if (TreasureReminder.instance)
                {
                    TreasureReminder.instance.DeductLockboxVoidCount();
                }
            }
        }




    }



}