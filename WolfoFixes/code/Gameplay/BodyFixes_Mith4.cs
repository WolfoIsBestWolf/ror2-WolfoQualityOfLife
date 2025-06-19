/*
using MonoMod.Cil;
using Mono.Cecil.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using EntityStates.Fauna;
using HarmonyLib;
using EntityStates.EngiTurret.EngiTurretWeapon;
using RoR2.Projectile;
using EntityStates;
using EntityStates.BrotherMonster;

namespace WolfoFixes
{

   

    public class BodyFixes_Mith4
    {
       

        public static void Start()
        {
            
 
        
            //On.EntityStates.BrotherMonster.SpellChannelEnterState.OnEnter += SpellChannelEnterState_OnEnter;
            //On.EntityStates.BrotherMonster.SpellChannelEnterState.OnExit += SpellChannelEnterState_OnExit;

    
            //Smthsmth maybe steal items after wards idk I never figured out smth cool
            //Just keep it in LGT for any actual change
        }

 
        private static void StaggerBaseState_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchCallvirt("EntityStates.BrotherMonster.StaggerBaseState", "get_nextState")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<EntityState,StaggerBaseState ,EntityState>>((newState, thisState) =>
                {
                    if (thisState is StaggerExit)
                    {
                        return new SpellChannelEnterState();
                    }
                    return newState;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : FixCaptainBeaconNoCrit");
            }
        }

        private static void StaggerExit_OnEnter(On.EntityStates.BrotherMonster.StaggerExit.orig_OnEnter orig, EntityStates.BrotherMonster.StaggerExit self)
        {
            self.outer.nextStateModifier = new EntityStateMachine.ModifyNextStateDelegate(ItemStealAfterStagger);
            orig(self);

        }

        public static void ItemStealAfterStagger(EntityStateMachine entityStateMachine, ref EntityState newNextState)
        {
           
            newNextState = new SpellChannelEnterState();
            //newNextState.fixedAge = 3;
            SpellChannelEnterState.duration = 3;
            entityStateMachine.GetComponent<ItemStealController>();

            entityStateMachine.nextStateModifier = null;
        }
 
  
    }


}
*/