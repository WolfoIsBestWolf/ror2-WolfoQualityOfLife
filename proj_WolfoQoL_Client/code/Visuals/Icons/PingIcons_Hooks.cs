using RoR2;
//using System;
using UnityEngine;


namespace WolfoQoL_Client
{

    public static partial class PingIcons
    {

        public static GameObject NullTempPosIndicator = null;
        public static Color VoidDefault = new Color(0.8211f, 0.5f, 1, 1);
        public static Color VoidFocused = new Color(0f, 3.9411764f, 5f, 1f);

        public static void AddHooks()
        {
            if (otherPingIconMods)
            {
                return;
            }
            On.EntityStates.Missions.Arena.NullWard.Active.OnEnter += VoidCell_Inidicator;
            On.EntityStates.Missions.Arena.NullWard.Complete.OnEnter += VoidCell_DestroyIndicator;
            On.RoR2.TeleporterInteraction.Start += TeleporterInteraction_Start;

            //On.RoR2.UI.ChargeIndicatorController.Awake += PingIconsShareColor;
            On.RoR2.PortalStatueBehavior.PreStartClient += NewtPingIcon;
            On.RoR2.GeodeController.PreStartClient += GeodePingIcon;


            On.RoR2.SceneExitController.OnEnable += SetPortalIcon;

            On.RoR2.PurchaseInteraction.OnInteractionBegin += RemoveScannerIcon_General;
            On.RoR2.MultiShopController.OnPurchase += RemoveScannerIcon_Shop;
            On.RoR2.BarrelInteraction.OnInteractionBegin += RemoveScannerIcon_Barrel;

            On.RoR2.PurchaseInteraction.ShouldShowOnScanner += BlockScanner_Purchase;
            On.RoR2.GenericInteraction.ShouldShowOnScanner += BlockScanner_Generic;
            On.RoR2.ChestRevealer.RevealedObject.OnEnable += ScannerOverrideIcon;

            On.RoR2.GenericPickupController.SyncPickupState += EquipmentPingIcon;

            //On.RoR2.DroneVendorTerminalBehavior.UpdatePickupDisplayAndAnimations += DroneShop_IconFromDrone;

            On.RoR2.Stage.PreStartClient += PingIconsOnStage;
        }


        private static void GeodePingIcon(On.RoR2.GeodeController.orig_PreStartClient orig, GeodeController self)
        {
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
        /*private static void DroneShop_IconFromDrone(On.RoR2.DroneVendorTerminalBehavior.orig_UpdatePickupDisplayAndAnimations orig, DroneVendorTerminalBehavior self)
        {
            orig(self);
            PingInfoProvider pingInfoProvider = self.GetComponent<PingInfoProvider>();
            if (self.CurrentPickupIndex != PickupIndex.none)
            {
                DroneIndex droneIndex = self.CurrentPickupIndex.pickupDef.droneIndex;
                if (droneIndex != DroneIndex.None)
                {
                    DroneDef droneDef = DroneCatalog.GetDroneDef(droneIndex);
                    if (droneDef && droneDef.droneBrokenSpawnCard)
                    {
                        PingInfoProvider pingInfoProviderDrone =  droneDef.droneBrokenSpawnCard.prefab.GetComponent<PingInfoProvider>();
                        if (pingInfoProviderDrone)
                        {
                            pingInfoProvider.pingIconOverride = pingInfoProviderDrone.pingIconOverride;
                        }
                        else
                        {
                            pingInfoProvider.pingIconOverride = PingIcons.DroneIcon;
                        }
                    }

                }
            }

        }
*/
        private static void EquipmentPingIcon(On.RoR2.GenericPickupController.orig_SyncPickupState orig, GenericPickupController self, UniquePickup newPickupState)
        {
            orig(self, newPickupState);
            if (!self.TryGetComponent<PingInfoProvider>(out _))
            {
                if (newPickupState.pickupIndex.equipmentIndex != EquipmentIndex.None)
                {
                    self.gameObject.AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/Equip.png");
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


        public static Color lunarHighlight = new Color(0.5223f, 0.8071f, 0.9151f, 1);

        private static void TeleporterInteraction_Start(On.RoR2.TeleporterInteraction.orig_Start orig, TeleporterInteraction self)
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
                        //self.teleporterChargeIndicatorController.playerPingColor = self.teleporterChargeIndicatorController.spriteFlashColor;
                    }
                }
            }
        }

        private static void ScannerOverrideIcon(On.RoR2.ChestRevealer.RevealedObject.orig_OnEnable orig, ChestRevealer.RevealedObject self)
        {
            orig(self);
            if (self.TryGetComponent<DifferentIconScanner>(out var icon))
            {
                if (icon.scannerIconOverride)
                {
                    self.positionIndicator.insideViewObject.GetComponent<SpriteRenderer>().sprite = icon.scannerIconOverride;
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

        private static void VoidCell_DestroyIndicator(On.EntityStates.Missions.Arena.NullWard.Complete.orig_OnEnter orig, EntityStates.Missions.Arena.NullWard.Complete self)
        {
            orig(self);
            Log.LogMessage("Destroy NullCell Indicator");
            Object.Destroy(NullTempPosIndicator);
            self.outer.gameObject.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_TintColor", VoidDefault);

        }

        private static void VoidCell_Inidicator(On.EntityStates.Missions.Arena.NullWard.Active.orig_OnEnter orig, EntityStates.Missions.Arena.NullWard.Active self)
        {
            orig(self);
            //if (NetworkServer.active)

            NullTempPosIndicator = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/PillarChargingPositionIndicator"));
            NullTempPosIndicator.name = "NullCellPositionIndicator";

            NullTempPosIndicator.GetComponent<PositionIndicator>().targetTransform = self.outer.transform;
            //RoR2.UI.ChargeIndicatorController NullCell = NullTempPosIndicator.GetComponent<PositionIndicator>().GetComponent<RoR2.UI.ChargeIndicatorController>();
            RoR2.UI.ChargeIndicatorController NullCell = NullTempPosIndicator.GetComponent<RoR2.UI.ChargeIndicatorController>();
            NullCell.holdoutZoneController = self.outer.GetComponent<HoldoutZoneController>();
            NullCell.spriteBaseColor = new Color(0.915f, 0.807f, 0.915f);
            NullCell.spriteChargedColor = new Color(0.977f, 0.877f, 0.977f);
            NullCell.spriteChargingColor = new Color(0.943f, 0.621f, 0.943f);
            NullCell.spriteFlashColor = new Color(0.92f, 0.411f, 0.92f);
            NullCell.textBaseColor = new Color(0.858f, 0.714f, 0.858f);
            NullCell.textChargingColor = new Color(1f, 1f, 1f);
            if (NullCell.iconSprites.Length > 0)
            {
                NullCell.iconSprites[0].sprite = PingIcons.NullVentIcon;
            }

            if (Util.GetItemCountForTeam(TeamIndex.Player, RoR2Content.Items.FocusConvergence.itemIndex, true, false) > 0)
            {
                self.outer.gameObject.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().materials[0].SetColor("_TintColor", VoidFocused);
                self.outer.gameObject.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().materials[1].SetColor("_TintColor", VoidFocused);
            }
        }


    }



    public class BlockScanner : PingInfoProvider
    {

    }
    public class DifferentIconScanner : PingInfoProvider
    {
        public Sprite scannerIconOverride;
    }

}
