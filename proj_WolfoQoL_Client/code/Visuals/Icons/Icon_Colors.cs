using MonoMod.Cil;
using UnityEngine;

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
 
        }


        private static void TeleporterDiscoveredRed(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            bool a = c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("RoR2.UI.ChargeIndicatorController", "isDiscovered"));


            if (a && c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("RoR2.UI.ChargeIndicatorController", "spriteChargedColor")))
            {
                c.Remove();
                c.EmitDelegate<System.Func<RoR2.UI.ChargeIndicatorController, Color>>((charg) =>
                {
                    return charg.spriteFlashColor;
                });
            }
            else
            {
                Log.LogWarning("IL Failed: TP RED DISCOVER CHANGE");
            }
        }

    }

}
