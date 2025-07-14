using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace WolfoFixes
{
    public class PrayerBeadsStorage : MonoBehaviour
    {
        public int lastSeenBeadAmount;
    }
    internal class PrayerBeads
    {

        public static void Start()
        {
            //OldBeadsLevel is serverOnly
            //So we Have to track it some other way
            ////I guess we coulda reused that for clients anyways
            On.RoR2.CharacterBody.Awake += BuffTracker_ClientFix;
            IL.RoR2.CharacterBody.RecalculateStats += BuffTracker_SetBuffCount_FixAmount;

            On.RoR2.CharacterMaster.OnBeadReset += CharacterMaster_OnBeadReset;
        }

        private static void CharacterMaster_OnBeadReset(On.RoR2.CharacterMaster.orig_OnBeadReset orig, CharacterMaster self, bool gainedStats)
        {
            orig(self, gainedStats);
            GameObject body = self.GetBodyObject();
            if (body != null)
            {
                body.GetComponent<PrayerBeadsStorage>().lastSeenBeadAmount = 0;
            }
        }

        private static void BuffTracker_ClientFix(On.RoR2.CharacterBody.orig_Awake orig, CharacterBody self)
        {
            orig(self);
            self.gameObject.AddComponent<PrayerBeadsStorage>();
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



}

