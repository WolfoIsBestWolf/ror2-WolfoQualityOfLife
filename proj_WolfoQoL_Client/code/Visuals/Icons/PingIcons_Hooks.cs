using RoR2;
using UnityEngine;

namespace WolfoQoL_Client
{
    public static partial class PingIcons
    {
        public static void AddHooks()
        {
            if (otherPingIconMods)
            {
                return;
            }
            On.RoR2.TeleporterInteraction.Start += PrimordialTPIcon;

            On.RoR2.PortalStatueBehavior.PreStartClient += NewtPingIcon;
            On.RoR2.GeodeController.PreStartClient += GeodePingIcon;

            On.RoR2.SceneExitController.OnEnable += SetPortalIcon;

            On.RoR2.PurchaseInteraction.OnInteractionBegin += RemoveScannerIcon_General;
            On.RoR2.MultiShopController.OnPurchase += RemoveScannerIcon_Shop;
            On.RoR2.BarrelInteraction.OnInteractionBegin += RemoveScannerIcon_Barrel;

            On.RoR2.PurchaseInteraction.ShouldShowOnScanner += BlockScanner_Purchase;
            On.RoR2.GenericInteraction.ShouldShowOnScanner += BlockScanner_Generic;

            On.RoR2.Stage.PreStartClient += PingIconsOnStage;
        }


        private static void GeodePingIcon(On.RoR2.GeodeController.orig_PreStartClient orig, GeodeController self)
        {
            //Name differs heavily so cant really use the dict
            orig(self);
            self.gameObject.AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/DLC2/Geode.png");
        }

        public static void PingIconsOnStage(On.RoR2.Stage.orig_PreStartClient orig, Stage self)
        {
            orig(self);
            PingIcons.SetAllStageInteractablePingIcons();
        }


        private static void SetPortalIcon(On.RoR2.SceneExitController.orig_OnEnable orig, SceneExitController self)
        {
            orig(self);
            if (self.transform.childCount > 0 && self.transform.GetChild(0).name == "PortalCenter")
            {
                if (self.TryGetComponent<PingInfoProvider>(out var ping))
                {
                    ping.pingIconOverride = PingIcons.PortalIcon;
                }
                else
                {
                    self.gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.PortalIcon;
                }
            }
        }

        private static void RemoveScannerIcon_Barrel(On.RoR2.BarrelInteraction.orig_OnInteractionBegin orig, BarrelInteraction self, Interactor activator)
        {
            orig(self, activator);
            Object.Destroy(self.GetComponent<ChestRevealer.RevealedObject>());
        }

        private static void RemoveScannerIcon_Shop(On.RoR2.MultiShopController.orig_OnPurchase orig, MultiShopController self, CostTypeDef.PayCostContext payCostContext, CostTypeDef.PayCostResults payCostResult)
        {
            orig(self, payCostContext, payCostResult);
            for (int i = 0; i < self.terminalGameObjects.Length; i++)
            {
                Object.Destroy(self.terminalGameObjects[i].GetComponent<ChestRevealer.RevealedObject>());
            }
        }
        private static void RemoveScannerIcon_General(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            orig(self, activator);
            Object.Destroy(self.GetComponent<ChestRevealer.RevealedObject>());
        }

        private static void NewtPingIcon(On.RoR2.PortalStatueBehavior.orig_PreStartClient orig, PortalStatueBehavior self)
        {
            orig(self);
            if (self.portalType == PortalStatueBehavior.PortalType.Shop)
            {
                PingInfoProvider ping = self.GetComponent<PingInfoProvider>();
                if (!ping)
                {
                    ping = self.gameObject.AddComponent<PingInfoProvider>();
                }
                ping.pingIconOverride = PingIcons.LunarIcon;
            }
        }

        private static void PingIconsShareColor(On.RoR2.UI.ChargeIndicatorController.orig_Awake orig, RoR2.UI.ChargeIndicatorController self)
        {
            orig(self);
            //self.playerPingColor = self.spriteFlashColor;
        }

        private static void PrimordialTPIcon(On.RoR2.TeleporterInteraction.orig_Start orig, TeleporterInteraction self)
        {
            orig(self);
            if (self.name.StartsWith("LunarT"))
            {
                if (self.teleporterChargeIndicatorController)
                {
                    if (WConfig.cfgPrimordialBlueHighlight.Value)
                    {
                        Highlight highlight = self.GetComponent<Highlight>();
                        highlight.CustomColor = new Color(0.5223f, 0.8071f, 0.9151f, 1);
                        highlight.highlightColor = Highlight.HighlightColor.custom;

                        Transform prong = self.transform.GetChild(0).GetChild(8);

                        //Check both cases
                        highlight = prong.GetComponent<InstantiatePrefabBehavior>().prefab.GetComponent<Highlight>();
                        if (highlight)
                        {
                            highlight.CustomColor = new Color(0.5223f, 0.8071f, 0.9151f, 1);
                            highlight.highlightColor = Highlight.HighlightColor.custom;
                        }
                        if (prong.childCount > 0)
                        {
                            highlight = prong.GetChild(0).GetComponent<Highlight>();
                            highlight.CustomColor = new Color(0.5223f, 0.8071f, 0.9151f, 1);
                            highlight.highlightColor = Highlight.HighlightColor.custom;
                        }
                    }
                    if (WConfig.cfgPrimordialBlueIcon.Value)
                    {
                        self.teleporterChargeIndicatorController.spriteFlashColor = new Color(0.5918f, 0.8219f, 0.9434f, 1f);
                        self.teleporterChargeIndicatorController.spriteChargingColor = new Color(0.5223f, 0.8071f, 0.9151f, 1);
                        self.teleporterChargeIndicatorController.spriteBaseColor = new Color(0.1745f, 0.4115f, 1f, 1f);
                    }
                    if (WConfig.cfgPingIcons.Value)
                    {
                        self.teleporterChargeIndicatorController.iconSprites[0].sprite = PingIcons.PrimordialTeleporterChargedIcon;
                    }
                }
            }
        }

        private static bool BlockScanner_Generic(On.RoR2.GenericInteraction.orig_ShouldShowOnScanner orig, GenericInteraction self)
        {
            if (self.GetComponent<BlockScanner>() != null)
            {
                return false;
            }
            return orig(self);
        }


        private static bool BlockScanner_Purchase(On.RoR2.PurchaseInteraction.orig_ShouldShowOnScanner orig, PurchaseInteraction self)
        {
            if (self.GetComponent<BlockScanner>() != null)
            {
                return false;
            }
            return orig(self);
        }

    }



    public class BlockScanner : PingInfoProvider
    {

    }


}
