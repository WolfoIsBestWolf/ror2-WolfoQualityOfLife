using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using UnityEngine;
using RoR2.UI;
using UnityEngine.Events;

namespace WolfoQoL_Client
{
    public class PrayerBeadsStorage : MonoBehaviour
    {
        public int lastSeenBeadAmount;
    }
    public class PrayerBeads
    {
        public static LanguageAPI.LanguageOverlay BeadsOverlay_Pickup;
        public static LanguageAPI.LanguageOverlay BeadsOverlay_Desc;
        public static ItemIndex moffeine = (ItemIndex)(-2); 
        public static ItemIndex usedBeads = (ItemIndex)(-2);
        public static bool justRemovedBeadsLocal = false;

        private static void Consumed_OverrideTooltip(On.RoR2.UI.ItemIcon.orig_SetItemIndex orig, RoR2.UI.ItemIcon self, ItemIndex newItemIndex, int newItemCount)
        {
            //If Mithrix takes co
            orig(self, newItemIndex, newItemCount);
            if (newItemIndex == usedBeads)
            {  
                if (self.tooltipProvider)
                {
                    ItemInventoryDisplay display = self.GetComponentInParent<ItemInventoryDisplay>();
                    if (display && display.inventory)
                    {
                        self.tooltipProvider.bodyToken = GetPrayerBeadsToken(display.inventory, display.inventory.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>(), "OVERLAY_EXTRASTATSONLEVELUP_CONSUMED_DESC");
                    }
                }
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
            On.RoR2.CharacterBody.Awake += BuffTracker_ClientFix;
            IL.RoR2.CharacterBody.RecalculateStats += BuffTracker_SetBuffCount_FixAmount;
 
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
                    //self.iconImage.texture = RoR2Content.Items.ScrapGreen.pickupIconTexture;
                }
                self.titleTMP.color = ColorCatalog.GetColor(itemDef.colorIndex);
            }
        }

        private static void BuffTracker_ClientFix(On.RoR2.CharacterBody.orig_Awake orig, CharacterBody self)
        {
            orig(self);
            self.gameObject.AddComponent<PrayerBeadsStorage>();
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

            if (body != null)
            {
                body.GetComponent<PrayerBeadsStorage>().lastSeenBeadAmount = 0;
            }
          


        }

        public static string GetPrayerBeadsToken(Inventory inventory, CharacterBody body, string tokenIn)
        {
            //Debug.Log("PrayerBeads_Ovelay");
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
            //Debug.Log("Bead Levels " + bonusLevels);
            return string.Format(Language.GetString(tokenIn), bonusStat0, bonusStat1, bonusStat2, bonusStat3);
        }

        public static void PrayerBeads_Ovelay(Inventory inventory, CharacterMaster master, CharacterBody body)
        {
            if (body == null)
            {
                Debug.Log("PrayerBeads_Ovelay NullBody");
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
                //BeadsOverlay_Desc = LanguageAPI.AddOverlay("BEADS_TEMP_2", tokenFull);
                CharacterMasterNotificationQueue notificationQueueForMaster = CharacterMasterNotificationQueue.GetNotificationQueueForMaster(master);
                CharacterMasterNotificationQueue.TransformationInfo transformation = new CharacterMasterNotificationQueue.TransformationInfo(CharacterMasterNotificationQueue.TransformationType.Default, DLC2Content.Items.ExtraStatsOnLevelUp);
                CharacterMasterNotificationQueue.NotificationInfo info = new CharacterMasterNotificationQueue.NotificationInfo(ItemCatalog.GetItemDef(DLC2Content.Items.ExtraStatsOnLevelUp.itemIndex), transformation);
                notificationQueueForMaster.PushNotification(info, 12f);
            }
        }
 
        private static void BuffTracker_SetBuffCount_FixAmount(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("RoR2.CharacterBody", "extraStatsOnLevelUpCount_CachedLastApplied")
            ))
            {
                c.EmitDelegate<Func<CharacterBody, CharacterBody>>((body) =>
                {
                    int buff = body.GetBuffCount(DLC2Content.Buffs.ExtraStatsOnLevelUpBuff);
                    if (buff > 0)
                    {
                        PrayerBeadsStorage prayerBeadsStorage = body.GetComponent<PrayerBeadsStorage>();
                        prayerBeadsStorage.lastSeenBeadAmount = buff;
                    }
                    //master.oldBeadLevel (ServerOnly)
                    return body;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : PrayerAttemptToFixClients");
            }

            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Buffs", "ExtraStatsOnLevelUpBuff"),
            x => x.MatchCall("RoR2.CharacterBody", "GetBuffCount")
            ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<int, CharacterBody, int>>((buffCount, body) =>
                {
                    //master.oldBeadLevel (ServerOnly)
                    PrayerBeadsStorage prayerBeadsStorage = body.GetComponent<PrayerBeadsStorage>();
                    if (prayerBeadsStorage.lastSeenBeadAmount > buffCount)
                    {
                        return prayerBeadsStorage.lastSeenBeadAmount;
                    }
                    return buffCount;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : PrayerAttemptToFixClients2");
            }
        }

    }



    public class TimedNotificationInfo
    {
        // Token: 0x04003412 RID: 13330
        public CharacterMasterNotificationQueue.NotificationInfo notification;

        // Token: 0x04003413 RID: 13331
        public float startTime;

        // Token: 0x04003414 RID: 13332
        public float duration;
    }

}

