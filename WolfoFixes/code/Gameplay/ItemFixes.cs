using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    public class ItemFixes
    {
        public static void Start()
        {
         
            // IL.RoR2.HealthComponent.TakeDamageProcess += FixEchoOSP;
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWarpedEchoE8;
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWarpedEchoNotUsingArmor;
 
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += FixChargedPerferatorCrit;

            On.RoR2.CharacterBody.OnTakeDamageServer += WEchoFirstHitIntoDanger;
        }

        private static void WEchoFirstHitIntoDanger(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, CharacterBody self, DamageReport damageReport)
        {
            //Pre for better compat?
            if (damageReport.damageInfo.firstHitOfDelayedDamageSecondHalf)
            {
                self.outOfDangerStopwatch = 0;
            }
            orig(self, damageReport);
        }

        private static void FixWarpedEchoNotUsingArmor(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Items","DelayedDamage"));

            if (c.TryGotoNext(MoveType.After,
           x => x.MatchLdloc(8) //This will break like instantly on update but probably fine mods right idk
           ))
            {
                //c.Next.Operand = 7; //Why does this not work?
                //Testing
                c.Emit(OpCodes.Ldloc, 7); 
                c.EmitDelegate<Func<float, float, float>>((aaa, bbb) =>
                {
                    /*Debug.Log(aaa);
                    Debug.Log(bbb);
                    Debug.Log(bbb*0.8f);*/
                    return bbb;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : FixWarpedEchoNotUsingArmor");
            }

            //After 80%
            if (c.TryGotoNext(MoveType.Before,
           x => x.MatchStloc(50) //This will break like instantly on update but probably fine mods right idk
           ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<float, HealthComponent, DamageInfo, float>>((damage, self, damageInfo) =>
                {
                    //I do not like copying it like this
                    //But I do not trust myself to do it in a more "official way" with breaks or moving pointers
                    //Debug.Log("Pre" + damage);
                    if (self.body.hasOneShotProtection && (damageInfo.damageType & DamageType.BypassOneShotProtection) != DamageType.BypassOneShotProtection)
                    {
                        float maxDamageOSP = (self.fullCombinedHealth + self.barrier) * (1f - self.body.oneShotProtectionFraction);
                        float b = Mathf.Max(0f, maxDamageOSP - self.serverDamageTakenThisUpdate); //What is this?
                        float huh = damage;
                        damage = Mathf.Min(damage, b); //This is what OSP does
                        if (damage != huh) //If you already took exactly 90% damage, dont do OSP (??)
                        {
                            Debug.Log("Trigger Warped OSP");
                            //self.TriggerOneShotProtection(); 
                            //Doing this rejects the first WEcho damage
                            //I'll just do this instead of dealing with OspTimer 
                        }
                    }
                    //Debug.Log("Post" + damage);

                    return damage;
                });

            }
            else
            {
                Debug.LogWarning("IL Failed : WARPED OSP FIX");
            }

         
        }
 
        private static void FixEchoOSP(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            int beforeEcho = -1;
            int beforeOSP = -1;
            int afterOSP = -1;

            ILCursor cbeforeEcho;
            ILCursor cbeforeOSP;
            ILCursor cafterOSP;



            //Goto X
            //Y
            //Echo Code
            //Goto Z
            //X
            //OSP
            //Goto Y
            //Z

            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Buff", "DelayedDamageBuff"));
            c.TryGotoNext(MoveType.After,
            x => x.MatchNewobj("RoR2.Orbs.SimpleLightningStrikeOrb"));


            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.Orbs.GenericDamageOrb", "isCrit")
            ))
            {

                cbeforeEcho = c.Emit(OpCodes.Break);


            }
            else
            {
                Debug.LogWarning("IL Failed : FixChargedPerferatorCrit");
            }

        }
 
        public static void FixChargedPerferatorCrit(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.RoR2Content/Items", "LightningStrikeOnHit"));
            c.TryGotoNext(MoveType.After,
            x => x.MatchNewobj("RoR2.Orbs.SimpleLightningStrikeOrb"));


            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.Orbs.GenericDamageOrb", "isCrit")
            ))
            {

                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<bool, DamageInfo, bool>>((isCrit, damageInfo) =>
                {
                    return damageInfo.crit;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : FixChargedPerferatorCrit");
            }
        }
 
        private static void FixWarpedEchoE8(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchCall("RoR2.Run", "get_instance"),
            x => x.MatchCallvirt("RoR2.Run", "get_selectedDifficulty"),
            x => x.MatchLdcI4((int)DifficultyIndex.Eclipse8));

            if (c.TryGotoNext(MoveType.Before,
           x => x.MatchLdfld("RoR2.DamageInfo", "delayedDamageSecondHalf")
           ))
            {

                c.Remove();
                c.EmitDelegate<System.Func<DamageInfo, bool>>((damageInfo) =>
                {
                    return false;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : FixWarpedEchoE8");
            }
        }
 
 

    }
 
}
