using RoR2;
//using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;


namespace WolfoQoL_Client
{
     
    public class BlockScanner : PingInfoProvider
    {

    }
    public class DifferentIconScanner : PingInfoProvider
    {
        public Sprite scannerIconOverride;
    }
    public class PingIcons
    {
        public static Sprite LunarIcon;
        public static Sprite ChestLargeIcon;
        public static Sprite CauldronIcon;
        public static Sprite SeerIcon;
        public static Sprite ExclamationIcon;
        public static Sprite LegendaryChestIcon;
        public static Sprite ChestLunarIcon;
        public static Sprite NullVentIcon;
        public static Sprite TimedChestIcon;
        public static Sprite PortalIcon;
        public static Sprite PrimordialTeleporterIcon;
        public static Sprite PrimordialTeleporterChargedIcon;
        public static Sprite CubeIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texLunarPillarIcon");
        public static Sprite ChestIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texInventoryIconOutlined");
        public static Sprite QuestionMarkIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texMysteryIcon");

        public static GameObject NullTempPosIndicator = null;
        public static Color VoidDefault = new Color(0.8211f, 0.5f, 1, 1);
        public static Color VoidFocused = new Color(0f, 3.9411764f, 5f, 1f);

        public static void Start()
        {
            Pings_Other();
            Pings_Chests();
            Pings_Shrines();
            Pings_Drones();
            Pings_Printer();

            On.EntityStates.Missions.Arena.NullWard.Active.OnEnter += VoidCell_Inidicator;
            On.EntityStates.Missions.Arena.NullWard.Complete.OnEnter += VoidCell_DestroyIndicator;
            On.RoR2.TeleporterInteraction.Start += TeleporterInteraction_Start;

            On.RoR2.UI.ChargeIndicatorController.Awake += PingIconsShareColor;
            On.RoR2.PortalStatueBehavior.PreStartClient += NewtPingIcon;

            On.RoR2.PurchaseInteraction.OnInteractionBegin += RemoveScannerIcon_General;
            On.RoR2.MultiShopController.OnPurchase += RemoveScannerIcon_Shop;
            On.RoR2.BarrelInteraction.OnInteractionBegin += RemoveScannerIcon_Barrel;

            On.RoR2.PurchaseInteraction.ShouldShowOnScanner += BlockScanner_Purchase;
            On.RoR2.GenericInteraction.ShouldShowOnScanner += BlockScanner_Generic;
            On.RoR2.ChestRevealer.RevealedObject.OnEnable += ScannerOverrideIcon;

            On.RoR2.GenericPickupController.SyncPickupIndex += GenericPickupController_SyncPickupIndex;
        }

        private static void GenericPickupController_SyncPickupIndex(On.RoR2.GenericPickupController.orig_SyncPickupIndex orig, GenericPickupController self, PickupIndex newPickupIndex)
        {
            orig(self, newPickupIndex);
            if (!self.TryGetComponent<PingInfoProvider>(out _))
            {
                if (newPickupIndex.equipmentIndex != EquipmentIndex.None)
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

        private static void RemoveScannerIcon_Shop(On.RoR2.MultiShopController.orig_OnPurchase orig, MultiShopController self, Interactor interactor, PurchaseInteraction purchaseInteraction)
        {
            orig(self, interactor, purchaseInteraction);
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
                ping.pingIconOverride = LunarIcon;
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
                        self.teleporterChargeIndicatorController.iconSprites[0].sprite = PrimordialTeleporterChargedIcon;
                        //self.teleporterChargeIndicatorController.playerPingColor = self.teleporterChargeIndicatorController.spriteFlashColor;
                    }
                }
            }
        }

        public static void Pings_Chests()
        {
            LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/Chest/Chest1").AddComponent<PingInfoProvider>().pingIconOverride = ChestIcon;

            ChestLargeIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestLargeIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Chest2").AddComponent<PingInfoProvider>().pingIconOverride = ChestLargeIcon;

            LegendaryChestIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestLegendaryIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestDamage").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestCategoryDamage.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestHealing").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestCategoryHealing.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestUtility").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestCategoryUtility.png");

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Damage Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestCategory2Damage.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Healing Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestCategory2Healing.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Utility Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestCategory2Utility.png");

            ChestLunarIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestLunarIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/LunarChest").AddComponent<PingInfoProvider>().pingIconOverride = ChestLunarIcon;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/EquipmentBarrel").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestEquipIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CasinoChest").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestCasinoIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Chest1StealthedVariant").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestInvisibleIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/VoidChest").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestVoid.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/VoidTriple").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestVoidPotential.png");


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/LockedIcon.png");

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/LockedIconAlt.png");


            Sprite PingIconShippingDroneS = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PingIconShippingDrone.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestMultiShop/FreeChestMultiShop.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminal/FreeChestTerminal.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/ScavBackpack").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ScavBagIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/ScavLunarBackpack").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ScavBagIcon.png");


            TimedChestIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestTimedIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/TimedChest").AddComponent<PingInfoProvider>().pingIconOverride = TimedChestIcon;


            #region TripleShop
            GameObject Terminal1 = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/Chest/MultiShopTerminal");
            Terminal1.AddComponent<DifferentIconScanner>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/MultiShop1_Shrunk.png");
            //Terminal1.GetComponent<DifferentIconScanner>().scannerIconOverride = ChestIcon;
            GameObject Terminal2 = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/Chest/MultiShopLargeTerminal");
            Terminal2.AddComponent<DifferentIconScanner>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/MultiShop2_Shrunk.png");
            //Terminal2.GetComponent<DifferentIconScanner>().scannerIconOverride = ChestLargeIcon;
            GameObject TerminalE = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/Chest/MultiShopEquipmentTerminal");
            TerminalE.AddComponent<DifferentIconScanner>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/MultiShopE_Shrunk.png");
            //TerminalE.GetComponent<DifferentIconScanner>().scannerIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ChestEquipIcon.png");
 
          
            #endregion
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

        public static void Pings_Shrines()
        {

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineBoss").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineMountainIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBoss/ShrineBossSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineMountainIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBoss/ShrineBossSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineMountainIcon.png");


            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineBlood").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineBloodIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBlood/ShrineBloodSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineBloodIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBlood/ShrineBloodSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineBloodIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineChance").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineChanceIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineChance/ShrineChanceSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineChanceIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineChance/ShrineChanceSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineChanceIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineRestack").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineOrderIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineRestack/ShrineRestackSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineOrderIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineRestack/ShrineRestackSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineOrderIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineHealing").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineHealIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineGoldshoresAccess").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineGoldIcon.png");

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanse.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/CleanseIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/CleanseIcon.png");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/CleanseIcon.png");

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/ShrineColossusAccess.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ShrineShaping.png");

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/ShrineHalcyonite.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/SotsIcon3_Shrunk.png");

        }

        public static void Pings_Drones()
        {


            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/Turret1Broken").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/DroneTurretIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/Drone2Broken").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/DroneHealIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/EmergencyDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/DroneEmergencyIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/EquipmentDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/DroneEquipmentIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/MegaDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/DroneMegaIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/FlameDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/DroneFlameIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/MissileDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/DroneMissileIcon.png");

        }

        public static void Pings_Printer()
        {

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Duplicator").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/Printer.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PrinterLarge.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").AddComponent<Reminders.SpawnListener>().greenPrinter = true;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PrinterMili.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PrinterWild.png");


            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Scrapper").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ScrapperIcon.png");

            CauldronIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/CauldronIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, GreenToRed Variant").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, WhiteToGreen").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, RedToWhite Variant").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
        }

        public static void Pings_Other()
        {
            PortalIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PortalIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArtifactworld").AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArena").AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalGoldshores").AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalShop").AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalMS").AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;

            GameObject PortalVoid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion();
            PortalVoid.AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
            PortalVoid.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
            PortalVoid.transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidOutroPortal/VoidOutroPortal.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/PortalInfiniteTower.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PM DestinationPortal.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;


            Sprite VoidDeepSymbol = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC1/DeepVoidPortalBattery/texDeepVoidPortalBatteryIcon.png").WaitForCompletion();
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ScrapperIcon.png");
            NullVentIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/CellIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/NullSafeWard").AddComponent<PingInfoProvider>().pingIconOverride = NullVentIcon;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSuppressor/VoidSuppressor.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = VoidDeepSymbol;


            Sprite ShipPodIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texRescueshipIcon");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/QuestVolatileBatteryAttachment").AddComponent<PingInfoProvider>().pingIconOverride = ShipPodIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/QuestVolatileBatteryWorldPickup").AddComponent<PingInfoProvider>().pingIconOverride = ShipPodIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/SurvivorPodBatteryPanel").AddComponent<PingInfoProvider>().pingIconOverride = ShipPodIcon;


            SeerIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/SeerIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/SeerStation").AddComponent<PingInfoProvider>().pingIconOverride = SeerIcon;


            PrimordialTeleporterIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PrimordialTeleporterIcon.png");
            PrimordialTeleporterChargedIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PrimordialTeleporterChargedIcon.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporter Variant").AddComponent<PingInfoProvider>().pingIconOverride = PrimordialTeleporterIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporterProngs").AddComponent<PingInfoProvider>().pingIconOverride = PrimordialTeleporterChargedIcon;



            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBattery.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = VoidDeepSymbol;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidCamp/VoidCamp.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/VoidIcon.png");


            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryBlood").GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryDesign").GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryMass").GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatterySoul").GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;



            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/RadarTower").AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/RadarTower.png");
            LunarIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/LunarIcon.png");


            Sprite VoidCrab = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/texInfiniteTowerSafeWardIcon.png").WaitForCompletion();
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerSafeWard.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = VoidCrab;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerSafeWardAwaitingInteraction.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = VoidCrab;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/LemurianEgg/LemurianEgg.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PingLemurianEgg.png");

            //Random Stuff
            ExclamationIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ExclamationIcon.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/HumanFan").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/GoldshoresBeacon").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Geode.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Healing").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Shocking").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, EquipmentRestock").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Hacking").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;

        }

        private static void VoidCell_DestroyIndicator(On.EntityStates.Missions.Arena.NullWard.Complete.orig_OnEnter orig, EntityStates.Missions.Arena.NullWard.Complete self)
        {
            orig(self);
            Debug.Log("Destroy NullCell Indicator");
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


        public static void ModSupport()
        {
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/GoldChest").AddComponent<PingInfoProvider>().pingIconOverride = LegendaryChestIcon;

            //Seems to NOT find vanilla spawn cards which is good
            InteractableSpawnCard[] ISCList = Resources.FindObjectsOfTypeAll(typeof(InteractableSpawnCard)) as InteractableSpawnCard[];
            for (var i = 0; i < ISCList.Length; i++)
            {
                //Debug.LogWarning(ISCList[i]);
                switch (ISCList[i].name)
                {
                    case "iscDroneTable":
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/SS2/SS2_DroneScrapper.png");
                        break;
                    case "iscCloneDrone":
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/SS2/SS2_DroneDuplicate.png");
                        break;
                    case "iscShockDrone":
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/SS2/SS2_DroneShock.png");
                        break;
                    case "iscTripleShopRed":
                        GameObject TerminalE = ISCList[i].prefab.GetComponent<MultiShopController>().terminalPrefab;
                        TerminalE.AddComponent<DifferentIconScanner>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/MultiShop3_Shrunk.png");
                        //TerminalE.GetComponent<DifferentIconScanner>().scannerIconOverride = LegendaryChestIcon;
                        break;

                    /*case "iscCloakedShrine":
                    case "iscCloakedShrineSnowy":
                    case "iscCloakedShrineSandy":
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SpStShrineCloakedS;
                        break;
                    case "iscAegisShrine":
                    case "iscAegisShrineSnowy":
                    case "iscAegisShrineSandy":
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SpStShrineAegisS;
                        break;*/
                    case "iscVoidPortalInteractable":
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PingIconVoidEradicator.png");
                        break;
                    case "iscWhorlCellInteractable":
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC1/DeepVoidPortalBattery/texDeepVoidPortalBatteryIcon.png").WaitForCompletion();
                        break;

                }
            }

            //This is stupidd idk how else to find stuff like Broken Han-d
            //Debug.LogWarning(RedMercSkin.MercSwordSlashRed.transform.parent);
            RoR2.ModelLocator.DestructionNotifier[] moddedPurchasesList = Resources.FindObjectsOfTypeAll(typeof(RoR2.ModelLocator.DestructionNotifier)) as RoR2.ModelLocator.DestructionNotifier[];
            for (var i = 0; i < moddedPurchasesList.Length; i++)
            {
                //Debug.LogWarning(moddedPurchasesList[i]);
                switch (moddedPurchasesList[i].name)
                {
                    case "BrokenJanitorInteractable":
                    case "BrokenJanitorRepair":
                        moddedPurchasesList[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
                        break;
                }
            }

        }

    }

}
