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

            if (WConfig.OperatorDroneIndicator.Value)
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
            }

            Material matShrineBossSymbol = Addressables.LoadAssetAsync<Material>(key: "b53bfabbc2667b74280f87d939f640c5").WaitForCompletion();
            matShrineBossSymbolPrestige = Object.Instantiate(matShrineBossSymbol);
            matShrineBossSymbolPrestige.name = "matShrineBossSymbolPrestige";
            matShrineBossSymbolPrestige.SetTexture("_RemapTex", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/General/rampPrestige.png"));
            matShrineBossSymbolPrestige.SetColor("_TintColor", new Color(0.7f, 0.5f, 0.7f, 1f)); //0.2406 0.8645 1 1


            On.RoR2.ShrineBossBehavior.Start += ShrineBossBehavior_Start;
            On.RoR2.BossShrineCounter.RebuildIndicators += BossShrineCounter_RebuildIndicators;
       
        }
        
        public static Material matShrineBossSymbolPrestige;

        private static void BossShrineCounter_RebuildIndicators(On.RoR2.BossShrineCounter.orig_RebuildIndicators orig, BossShrineCounter self)
        {
            orig(self);
            if (WConfig.PrestigeColors.Value)
            {
                if (RunArtifactManager.instance && RunArtifactManager.instance.IsArtifactEnabled(DLC3Content.Artifacts.Prestige))
                {
                    for (int i = 0; i < self.targetTransform.childCount; i++)
                    {
                        self.targetTransform.GetChild(i).GetChild(0).GetComponent<MeshRenderer>().material = matShrineBossSymbolPrestige;
                    }
                }
            }

        }

        private static void ShrineBossBehavior_Start(On.RoR2.ShrineBossBehavior.orig_Start orig, ShrineBossBehavior self)
        {
            orig(self);
            if (WConfig.PrestigeColors.Value)
            {
                if (RunArtifactManager.instance && RunArtifactManager.instance.IsArtifactEnabled(DLC3Content.Artifacts.Prestige))
                {
                    self.symbolTransform.GetComponent<MeshRenderer>().material = matShrineBossSymbolPrestige;
                }
            }

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
