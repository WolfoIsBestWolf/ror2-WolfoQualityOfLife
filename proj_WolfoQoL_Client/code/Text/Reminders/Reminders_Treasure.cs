using RoR2;
using WolfoLibrary;
using WolfoQoL_Client.Text;
using static WolfoQoL_Client.ModSupport.QualitySupport;

namespace WolfoQoL_Client.Reminders
{
    public static class Reminders_Treasure
    {
        public static void Start()
        {
            //Key and VoidKey
            On.RoR2.PurchaseInteraction.OnInteractionBegin += KeyReminderUpdater_Host;

            //Free Chest
            On.RoR2.MultiShopController.OnPurchase += FreeChestReminderUpdater_Host;

            //RegenScrap and SaleStar
            On.RoR2.CharacterMasterNotificationQueue.PushItemTransformNotification += Clear_Star_Regen;

            //Quality Mod Quality Barrel
            On.EntityStates.Barrel.Opening.OnEnter += Quality_Barrel;
        }

        private static void Quality_Barrel(On.EntityStates.Barrel.Opening.orig_OnEnter orig, EntityStates.Barrel.Opening self)
        {
            orig(self);
            if (self.gameObject.name.StartsWith("SpeedOnPickup"))
            {
                if (TreasureReminder.instance)
                {
                    TreasureReminder.Deduct(TreasureReminder.instance.Objective_Quality_Collectors, ref TreasureReminder.instance.qualityCollectors);
                }
            }
        }

        private static void Clear_Star_Regen(On.RoR2.CharacterMasterNotificationQueue.orig_PushItemTransformNotification orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            orig(characterMaster, oldIndex, newIndex, transformationType);
            /*WolfoMain.log.LogMessage("PUSH" +
                "\nC:" + characterMaster+
                "\no" + oldIndex+
                "\nn" + newIndex+
                "\nT" + transformationType
                );
            */
            if (!characterMaster.hasAuthority)
            {
                //Debug.LogError("Not clearing reminders for " + Util.GetBestMasterName(characterMaster) + " because they aren't local.");
                return;
            }
            if (transformationType == CharacterMasterNotificationQueue.TransformationType.Default)
            {

                if (PreBaseItemIndex(newIndex, DLC1Content.Items.RegeneratingScrapConsumed))
                {
                    if (TreasureReminder.instance)
                    {
                        Reminders_Main.CompleteObjective(TreasureReminder.instance.Objective_RegenScrap);
                    }
                }
                else if (newIndex == MoreMessages.VanillaVoids_WatchBrokeItem)
                {
                    TreasureReminder.CheckKeysVoided();
                    TreasureReminder.CheckItemsDestroyed();
                }
            }
            else if (transformationType == CharacterMasterNotificationQueue.TransformationType.SaleStarRegen)
            {
                //if (newIndex == DLC2Content.Items.LowerPricedChestsConsumed.itemIndex)
                if (PreBaseItemIndex(newIndex, DLC2Content.Items.LowerPricedChestsConsumed))
                {
                    if (TreasureReminder.instance)
                    {
                        Reminders_Main.CompleteObjective(TreasureReminder.instance.Objective_SaleStar);
                    }
                }
            }
            else if (transformationType == CharacterMasterNotificationQueue.TransformationType.LunarSun)
            {
                TreasureReminder.CheckKeysVoided();
                TreasureReminder.CheckItemsDestroyed();
            }
            else if (transformationType == CharacterMasterNotificationQueue.TransformationType.ContagiousVoid && oldIndex == RoR2Content.Items.TreasureCache.itemIndex)
            {
                TreasureReminder.CheckKeysVoided();
            }

        }


        private static void FreeChestReminderUpdater_Host(On.RoR2.MultiShopController.orig_OnPurchase orig, MultiShopController self, CostTypeDef.PayCostContext payCostContext, CostTypeDef.PayCostResults payCostResult)
        {
            orig(self, payCostContext, payCostResult);
            //WolfoMain.log.LogWarning(self);
            if (!self.available && self.name.StartsWith("FreeChestMultiShop"))
            {
                if (TreasureReminder.instance)
                {
                    TreasureReminder.Deduct(TreasureReminder.instance.Objective_FreeChest, ref TreasureReminder.instance.freeChestCount);
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
                Networker.SendWQoLMessage(new Text.InteractableMessage
                {
                    interactableToken = self.displayNameToken,
                    subjectAsCharacterBody = body,
                });
            }
            if (self.costType == CostTypeIndex.TreasureCacheItem)
            {
                if (TreasureReminder.instance)
                {
                    TreasureReminder.Deduct(TreasureReminder.instance.Objective_Lockbox, ref TreasureReminder.instance.lockboxCount);
                }
            }
            else if (self.costType == CostTypeIndex.TreasureCacheVoidItem)
            {
                if (TreasureReminder.instance)
                {
                    TreasureReminder.Deduct(TreasureReminder.instance.Objective_LockboxVoid, ref TreasureReminder.instance.lockboxVoidCount);
                }
            }
        }




    }



}