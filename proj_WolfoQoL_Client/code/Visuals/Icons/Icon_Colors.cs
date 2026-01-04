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
                WQoLMain.log.LogWarning("IL Failed: TP RED DISCOVER CHANGE");
            }
        }

    }

}
