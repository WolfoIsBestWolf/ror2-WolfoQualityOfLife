using RoR2;
using UnityEngine.AddressableAssets;

using UnityEngine;

//using System;
using UnityEngine.Networking;
using System.ComponentModel;

namespace WolfoQoL_Client
{
 
    public class GameplayQualityOfLife
    {
        public static void Start()
        {
            if (!WConfig.cfgGameplay.Value)
            {
                return;
            }
            PrismaticTrial.AllowPrismaticTrials();
            
            On.EntityStates.LunarTeleporter.LunarTeleporterBaseState.FixedUpdate += LunarTeleporterBaseState_FixedUpdate;

 
        }

        public static void LunarTeleporterBaseState_FixedUpdate(On.EntityStates.LunarTeleporter.LunarTeleporterBaseState.orig_FixedUpdate orig, EntityStates.LunarTeleporter.LunarTeleporterBaseState self)
        {
            self.fixedAge += self.GetDeltaTime();
            if (NetworkServer.active)
            {
                if (TeleporterInteraction.instance)
                {
                    if (TeleporterInteraction.instance.isInFinalSequence)
                    {
                        self.genericInteraction.Networkinteractability = Interactability.Disabled;
                        return;
                    }
                    self.genericInteraction.Networkinteractability = self.preferredInteractability;
                }
            }
        }
 
    }


}
