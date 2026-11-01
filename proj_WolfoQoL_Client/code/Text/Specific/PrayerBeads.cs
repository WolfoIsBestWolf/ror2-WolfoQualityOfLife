using R2API;
using RoR2;
using RoR2.UI;

namespace WolfoQoL_Client.Text
{

    public static class PrayerBeads
    {
        public static ItemIndex moffeine = (ItemIndex)(-2);
        public static ItemIndex usedBeads = (ItemIndex)(-2);
       
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

            if (WQoLMain.ServerModInstalled)
            {
                On.RoR2.UI.ItemIcon.SetItemIndex_ItemIndex_int_float += Consumed_OverrideTooltip;
                On.RoR2.UI.ItemIcon.ItemClicked += Consumed_OverrideInspect;
            }
             On.RoR2.CharacterMaster.OnBeadReset += OverlayForTransformationMessage;
        }

        private static void Consumed_OverrideTooltip(On.RoR2.UI.ItemIcon.orig_SetItemIndex_ItemIndex_int_float orig, ItemIcon self, ItemIndex newItemIndex, int newItemCount, float newDurationPercent)
        {
            orig(self, newItemIndex, newItemCount, newDurationPercent);
            if (newItemIndex == usedBeads)
            {
                SetDescription(self);
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
                float itemCount = (float)inventory.GetItemCountEffective(moffeine);
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
                Log.LogMessage("PrayerBeads_Ovelay NullBody");
                return;
            }
            if (!master.hasAuthority)
            {
                return;
            }
            CharacterMasterNotificationQueue.CustomOverrideInfo overrideInfo = new CharacterMasterNotificationQueue.CustomOverrideInfo
            {
                titleText = "ITEM_EXTRASTATSONLEVELUP_CONSUMED_NAME",
                descriptionText = GetPrayerBeadsToken(inventory, body, "OVERLAY_EXTRASTATSONLEVELUP_DESC"),
            };
            CharacterMasterNotificationQueue notificationQueueForMaster = CharacterMasterNotificationQueue.GetNotificationQueueForMaster(master);
            if (notificationQueueForMaster)
            {
                if (WQoLMain.ServerModInstalled == true)
                {
                    notificationQueueForMaster.PushNotification(new CharacterMasterNotificationQueue.NotificationInfo(usedBeads, null, overrideInfo, false), 12f);
                }
                else if (WQoLMain.ServerModInstalled == false)
                {
                    overrideInfo.iconColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Unaffordable);
                    notificationQueueForMaster.PushNotification(new CharacterMasterNotificationQueue.NotificationInfo(DLC2Content.Items.ExtraStatsOnLevelUp, null, overrideInfo, false), 12f);
                }
            }
           
           
        }


    }

}

