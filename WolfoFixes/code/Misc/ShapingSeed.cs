using MonoMod.Cil;
using RoR2;
using System;

using UnityEngine;
using UnityEngine.Networking;

namespace WolfoFixes
{
    internal class ShrineShapingFixes
    {
        public static void Start()
        {

            IL.RoR2.EquipmentSlot.FireHealAndRevive += ShrineRezEffect; //Use Seed
            IL.RoR2.CharacterMaster.RespawnExtraLifeHealAndRevive += ShrineRezEffect; //Use Seed already dead
            IL.RoR2.CharacterMaster.RespawnExtraLifeShrine += ShrineRezEffect; //Rev from Shrine already dead


        }

        private static void RezEffectOnDead(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
             x => x.MatchLdsfld("RoR2.RoR2Content/Buffs", "Immune")
            ))
            {
                c.EmitDelegate<Func<CharacterBody, CharacterBody>>((body) =>
                {
                    if (body)
                    {
                        //Debug.Log(body + " : " + body.transform.position);
                        GameObject gameObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect");
                        EffectManager.SpawnEffect(gameObject, new EffectData
                        {
                            origin = body.transform.position,
                            rotation = body.transform.rotation
                        }, true);
                        Util.PlaySound("Play_item_proc_extraLife", body.gameObject);
                    }
                    return body;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : bro they forgot to make a rez effect2");
            }
        }

        private static void ShrineRezEffect(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
             x => x.MatchLdstr("Prefabs/Effects/fxHealAndReviveGold")
            ))
            {
                c.Next.Operand = "Prefabs/Effects/HippoRezEffect";
            }
            else
            {
                Debug.LogWarning("IL Failed : bro they forgot to make a rez effect");
            }
        }

        private static void CharacterMaster_RespawnExtraLifeShrine(On.RoR2.CharacterMaster.orig_RespawnExtraLifeShrine orig, CharacterMaster self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                if (self.bodyInstanceObject)
                {
                    EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect"), new EffectData
                    {
                        origin = self.deathFootPosition,
                        rotation = self.bodyInstanceObject.transform.rotation
                    }, true);
                }
            }

        }




    }

}
