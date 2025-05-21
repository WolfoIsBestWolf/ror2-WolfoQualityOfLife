using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using UnityEngine;
 
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
        public static ItemIndex moffeine = ItemIndex.None; //None2 


        public static void Start()
        {
            IL.RoR2.CharacterBody.RecalculateStats += AttemptToFixClients;

            On.RoR2.CharacterMaster.OnBeadReset += AddOverlay;
            On.RoR2.Run.OnDisable += RemoveOverlay;

            //OldBeadsLevel is serveronly?
            On.RoR2.CharacterBody.Awake += CharacterBody_Awake;
        }

        private static void CharacterBody_Awake(On.RoR2.CharacterBody.orig_Awake orig, CharacterBody self)
        {
            orig(self);
            self.gameObject.AddComponent<PrayerBeadsStorage>();
        }


        private static void RemoveOverlay(On.RoR2.Run.orig_OnDisable orig, Run self)
        {
            orig(self);
            if (BeadsOverlay_Pickup != null)
            {
                BeadsOverlay_Pickup.Remove();
                BeadsOverlay_Desc.Remove();
            }
        }
        private static void AttemptToFixClients(ILContext il)
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

        private static void AddOverlay(On.RoR2.CharacterMaster.orig_OnBeadReset orig, CharacterMaster self, bool gainedStats)
        {
            CharacterBody body = self.GetBody();
            if (body && self.hasAuthority && gainedStats)
            {
                PrayerBeads_Ovelay(self.inventory, self, body);
            }
            //Done before, so we can make sure the overlay is made
            orig(self, gainedStats);
            if (body != null)
            {
                body.GetComponent<PrayerBeadsStorage>().lastSeenBeadAmount = 0;
            }

            if (WolfoMain.ServerModInstalled == false)
            {
                if (BeadsOverlay_Pickup != null)
                {
                    BeadsOverlay_Pickup.Remove();
                    BeadsOverlay_Desc.Remove();
                }
            }

        }

        public static void PrayerBeads_Ovelay(Inventory inventory, CharacterMaster characterMaster, CharacterBody body)
        {
            Debug.Log("PrayerBeads_Ovelay");
            if (body == null)
            {
                Debug.LogWarning("No Body Prayer");
                return;
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
            Debug.Log("Bead Levels " + bonusLevels);
            if (BeadsOverlay_Pickup != null)
            {
                BeadsOverlay_Pickup.Remove();
                BeadsOverlay_Desc.Remove();
            }
            if (WolfoMain.ServerModInstalled == true)
            {
                BeadsOverlay_Pickup = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_CONSUMED_PICKUP", string.Format(Language.GetString("OVERLAY_EXTRASTATSONLEVELUP_CONSUMED_PICKUP"), bonusStat0));
                BeadsOverlay_Desc = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_CONSUMED_DESC", string.Format(Language.GetString("OVERLAY_EXTRASTATSONLEVELUP_CONSUMED_DESC"), bonusStat0, bonusStat1, bonusStat2, bonusStat3));
            }
            else if (WolfoMain.ServerModInstalled == false)
            {
                string tokenFull = string.Format(Language.GetString("OVERLAY_EXTRASTATSONLEVELUP_CONSUMED_DESC"), bonusStat0, bonusStat1, bonusStat2, bonusStat3);
                BeadsOverlay_Pickup = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_PICKUP", tokenFull);
                BeadsOverlay_Desc = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_DESC", tokenFull);
                //CharacterMasterNotificationQueue.PushItemTransformNotification(body.master, DLC2Content.Items.ExtraStatsOnLevelUp.itemIndex, DLC2Content.Items.ExtraStatsOnLevelUp.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);

                CharacterMasterNotificationQueue notificationQueueForMaster = CharacterMasterNotificationQueue.GetNotificationQueueForMaster(characterMaster);
                CharacterMasterNotificationQueue.TransformationInfo transformation = new CharacterMasterNotificationQueue.TransformationInfo(CharacterMasterNotificationQueue.TransformationType.Default, DLC2Content.Items.ExtraStatsOnLevelUp);
                CharacterMasterNotificationQueue.NotificationInfo info = new CharacterMasterNotificationQueue.NotificationInfo(ItemCatalog.GetItemDef(DLC2Content.Items.ExtraStatsOnLevelUp.itemIndex), transformation);
                notificationQueueForMaster.PushNotification(info, 11f);
            }




        }

    }





}

