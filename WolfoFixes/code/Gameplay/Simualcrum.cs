using RoR2;
using UnityEngine.AddressableAssets;

using UnityEngine;

//using System;
using UnityEngine.Networking;
using System.ComponentModel;
using MonoMod.Cil;

namespace WolfoFixes
{
 
    public class Simualcrum
    {
        public static void Start()
        {
            IL.RoR2.InfiniteTowerWaveController.FixedUpdate += FixRequestIndicatorsClient;
            On.EntityStates.InfiniteTowerSafeWard.AwaitingActivation.OnEnter += Waiting_SetRadius;
        }

        public static void Waiting_SetRadius(On.EntityStates.InfiniteTowerSafeWard.AwaitingActivation.orig_OnEnter orig, EntityStates.InfiniteTowerSafeWard.AwaitingActivation self)
        {
            orig(self);

            //Client fix??
            InfiniteTowerRun run = Run.instance.GetComponent<InfiniteTowerRun>();
            if (!run.safeWardController)
            {
                run.safeWardController = self.gameObject.GetComponent<InfiniteTowerSafeWardController>();
            }
        }


        public static void FixRequestIndicatorsClient(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
             x => x.MatchCallvirt("RoR2.CombatSquad", "get_readOnlyMembersList"));

            if (c.TryGotoPrev(MoveType.Before,
             x => x.MatchLdfld("RoR2.InfiniteTowerWaveController", "combatSquad")
            ))
            {
                c.EmitDelegate<System.Func<InfiniteTowerWaveController, InfiniteTowerWaveController>>((wave) =>
                {
                    if (wave.combatSquad.readOnlyMembersList.Count == 0)
                    {
                        Debug.Log("Couln't do indicators the normal way");
                        for (int i = 0; wave.combatSquad.membersList.Count > i; i++)
                        {
                            wave.RequestIndicatorForMaster(wave.combatSquad.membersList[i]);
                        }
                    }
                    return wave;
                });
                Debug.Log("IL Found : IL.RoR2.InfiniteTowerWaveController.FixedUpdate");
            }
            else
            {
                Debug.LogWarning("IL Failed : IL.RoR2.InfiniteTowerWaveController.FixedUpdate");
            }
        }

    }


}
