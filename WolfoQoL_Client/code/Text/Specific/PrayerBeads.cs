using R2API;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace WolfoQoL_Client.Text
{

    public class PrayerBeads
    {
        public static LanguageAPI.LanguageOverlay BeadsOverlay_Pickup;
        public static LanguageAPI.LanguageOverlay BeadsOverlay_Desc;
        public static ItemIndex moffeine = (ItemIndex)(-2);
        public static ItemIndex usedBeads = (ItemIndex)(-2);
        public static bool justRemovedBeadsLocal = false;

        private static void Consumed_OverrideTooltip(On.RoR2.UI.ItemIcon.orig_SetItemIndex orig, RoR2.UI.ItemIcon self, ItemIndex newItemIndex, int newItemCount)
        {
            orig(self, newItemIndex, newItemCount);
            if (newItemIndex == usedBeads)
            {
                SetDescription(self);
            }
        }

        internal static void SetDescription(ItemIcon self)
        {
            ItemInventoryDisplay display = self.GetComponentInParent<ItemInventoryDisplay>();
            if (display && display.inventory)
            {
                self.tooltipProvider.overrideBodyText = GetPrayerBeadsToken(display.inventory, display.inventory.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>(), "OVERLAY_EXTRASTATSONLEVELUP_CONSUMED_DESC");
            }
        }

        private static void Consumed_OverrideInspect(On.RoR2.UI.ItemIcon.orig_ItemClicked orig, ItemIcon self)
        {
            orig(self);
            if (self.itemIndex == usedBeads)
            {
                if (self.userProfile.useInspectFeature)
                {
                    ItemInventoryDisplay display = self.GetComponentInParent<ItemInventoryDisplay>();
                    if (display && display.inventory)
                    {
                        self.inspectPanel.InspectDescription.token = GetPrayerBeadsToken(display.inventory, display.inventory.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>(), "OVERLAY_EXTRASTATSONLEVELUP_CONSUMED_DESC");
                    }
                }
            }

        }

        public static void Start()
        {
            //OldBeadsLevel is serveronly?

            if (WolfoMain.ServerModInstalled)
            {
                On.RoR2.UI.ItemIcon.SetItemIndex += Consumed_OverrideTooltip;
                On.RoR2.UI.ItemIcon.ItemClicked += Consumed_OverrideInspect;
            }
            else
            {
                On.RoR2.UI.GenericNotification.SetItem += GenericNotification_SetItem;
            }
            On.RoR2.CharacterMaster.OnBeadReset += OverlayForTransformationMessage;
        }

        private static void GenericNotification_SetItem(On.RoR2.UI.GenericNotification.orig_SetItem orig, GenericNotification self, ItemDef itemDef)
        {
            orig(self, itemDef);
            if (justRemovedBeadsLocal && itemDef == DLC2Content.Items.ExtraStatsOnLevelUp)
            {
                justRemovedBeadsLocal = false;
                self.titleText.token = itemDef.nameToken;
                self.descriptionText.token = Language.GetString("BEADS_TEMP_");
                if (itemDef.pickupIconTexture != null)
                {
                    self.iconImage.gameObject.SetActive(false);

                }
                self.titleTMP.color = ColorCatalog.GetColor(itemDef.colorIndex);
            }
        }




        private static void OverlayForTransformationMessage(On.RoR2.CharacterMaster.orig_OnBeadReset orig, CharacterMaster self, bool gainedStats)
        {
            CharacterBody body = self.GetBody();
            if (body && gainedStats)
            {
                PrayerBeads_Ovelay(self.inventory, self, body);
            }
            //Done before, so we can make sure the overlay is made
            orig(self, gainedStats);

        }

        public static string GetPrayerBeadsToken(Inventory inventory, CharacterBody body, string tokenIn)
        {
            //WolfoMain.log.LogMessage("PrayerBeads_Ovelay");
            if (body == null)
            {
                body = inventory.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>();
            }

            float beadDamage = inventory.beadAppliedDamage;
            float beadHealth = inventory.beadAppliedHealth;
            float beadRegen = inventory.beadAppliedRegen;
            float bonusLevels = beadDamage / body.levelDamage;
            if (moffeine != ItemIndex.None)
            {
                float itemCount = (float)inventory.GetItemCount(moffeine);
                bonusLevels = itemCount / 5f;
                itemCount = itemCount * 0.05f;
                beadDamage = body.levelDamage * itemCount;
                beadHealth = body.levelMaxHealth * itemCount;
                beadRegen = body.levelRegen * itemCount;
            }
            string bonusStat0 = bonusLevels.ToString("0.##");
            string bonusStat1 = beadHealth.ToString("0.##");
            string bonusStat2 = beadRegen.ToString("0.##");
            string bonusStat3 = beadDamage.ToString("0.##");
            //WolfoMain.log.LogMessage("Bead Levels " + bonusLevels);
            return string.Format(Language.GetString(tokenIn), bonusStat0, bonusStat1, bonusStat2, bonusStat3);
        }

        public static void PrayerBeads_Ovelay(Inventory inventory, CharacterMaster master, CharacterBody body)
        {
            if (body == null)
            {
                WolfoMain.log.LogMessage("PrayerBeads_Ovelay NullBody");
                return;
            }
            if (!master.hasAuthority)
            {
                return;
            }
            if (BeadsOverlay_Pickup != null)
            {
                BeadsOverlay_Pickup.Remove();
            }
            if (BeadsOverlay_Desc != null)
            {
                BeadsOverlay_Desc.Remove();
            }
            if (WolfoMain.ServerModInstalled == true)
            {
                string tokenFull = GetPrayerBeadsToken(inventory, body, "OVERLAY_EXTRASTATSONLEVELUP_CONSUMED_PICKUP");
                BeadsOverlay_Pickup = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_CONSUMED_PICKUP", tokenFull);
                BeadsOverlay_Desc = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_CONSUMED_DESC", tokenFull);
                CharacterMasterNotificationQueue.PushItemTransformNotification(master, DLC2Content.Items.ExtraStatsOnLevelUp.itemIndex, usedBeads, CharacterMasterNotificationQueue.TransformationType.Default);
            }
            else if (WolfoMain.ServerModInstalled == false)
            {
                justRemovedBeadsLocal = true;
                //latestBeader = inventory;
                string tokenFull = GetPrayerBeadsToken(inventory, body, "OVERLAY_EXTRASTATSONLEVELUP_DESC");
                BeadsOverlay_Pickup = LanguageAPI.AddOverlay("BEADS_TEMP_", tokenFull);
                CharacterMasterNotificationQueue notificationQueueForMaster = CharacterMasterNotificationQueue.GetNotificationQueueForMaster(master);
                CharacterMasterNotificationQueue.TransformationInfo transformation = new CharacterMasterNotificationQueue.TransformationInfo(CharacterMasterNotificationQueue.TransformationType.Default, DLC2Content.Items.ExtraStatsOnLevelUp);
                CharacterMasterNotificationQueue.NotificationInfo info = new CharacterMasterNotificationQueue.NotificationInfo(ItemCatalog.GetItemDef(DLC2Content.Items.ExtraStatsOnLevelUp.itemIndex), transformation);
                notificationQueueForMaster.PushNotification(info, 12f);
            }
        }


    }

}

