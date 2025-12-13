using MonoMod.Cil;
using Mono.Cecil.Cil;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static WolfoQoL_Client.Assets;

namespace WolfoQoL_Client
{

    public static class Icon_Colors
    {
 
        public static void Start()
        {
            //Color not UI ig
            if (WConfig.cfgTpIconDiscoveredRed.Value)
            {
                IL.RoR2.UI.ChargeIndicatorController.Update += TeleporterDiscoveredRed;
            }

            /*if (WConfig.OperatorDroneIndicator.Value)
            {
                GameObject DroneTrackingIndicator = Addressables.LoadAssetAsync<GameObject>(key: "54d961ee6ebdb68419804536906b4ab7").WaitForCompletion();
                //0 0.2863 0.2902 0.5176
                int i = 0;
                foreach (SpriteRenderer sprite in DroneTrackingIndicator.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (i == 0)
                    {
                        i++;
                    }
                    else
                    {
                        sprite.color = new Color(0.122f, 0.937f, 0.678f, 1);
                    }
                }
            }*/
 
        }
 
      
        private static void TeleporterDiscoveredRed(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("RoR2.UI.ChargeIndicatorController", "isDiscovered"));


            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("RoR2.UI.ChargeIndicatorController", "spriteChargedColor")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<Color, RoR2.UI.ChargeIndicatorController, Color>>((value, charg) =>
                {
                    return charg.spriteFlashColor;
                });
            }
            else
            {
                WQoLMain.log.LogWarning("IL Failed: TP RED DISCOVER CHANGE");
            }
        }

    }

}
