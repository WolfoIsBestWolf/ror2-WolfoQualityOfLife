using RoR2;
//using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{
    public class PingIcons
    {
        public static Sprite ChestLargeIcon = Sprite.Create(v.dummytex256, v.rec256, v.half);
        public static Sprite ScrapperIcon = Sprite.Create(v.dummytex256, v.rec256, v.half);

        public static Sprite CauldronIcon = Sprite.Create(v.dummytex256, v.rec256, v.half);
        public static Sprite SeerIcon = Sprite.Create(v.dummytex256, v.rec256, v.half);
        public static Sprite ExclamationIcon = Sprite.Create(v.dummytex192, v.rec192, v.half);
        public static Sprite LegendaryChestIcon = Sprite.Create(v.dummytexwide, v.recwide, v.half);
        public static Sprite ChestLunarIcon = Sprite.Create(v.dummytex256, v.rec256, v.half);
        public static Sprite ShrineOrderIcon = Sprite.Create(v.dummytex256, v.rec256, v.half);
        public static Sprite NullVentIcon = Sprite.Create(v.dummytex320, v.rec320, v.half);
        public static Sprite TimedChestIcon = Sprite.Create(v.dummytex256, v.rec256, v.half);
        public static Sprite PortalIcon = Sprite.Create(v.dummytexhalftall, v.rechalftall, v.half);
        public static Sprite PrimordialTeleporterIcon = Sprite.Create(v.dummytex512, v.rechalftall, v.half);
        public static Sprite PrimordialTeleporterChargedIcon = Sprite.Create(v.dummytex512, v.rechalftall, v.half);
        public static Sprite AttackIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texAttackIcon");
        public static Sprite CubeIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texLunarPillarIcon");
        public static Sprite ChestIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texInventoryIconOutlined");
        public static Sprite ShrineIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texShrineIconOutlined");
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
        }

        private static void PingIconsShareColor(On.RoR2.UI.ChargeIndicatorController.orig_Awake orig, RoR2.UI.ChargeIndicatorController self)
        {
            orig(self);
            self.playerPingColor = self.spriteChargingColor;
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
                        self.teleporterChargeIndicatorController.playerPingColor = self.teleporterChargeIndicatorController.spriteChargingColor;
                    }
                    if (WConfig.cfgPingIcons.Value)
                    {
                        self.teleporterChargeIndicatorController.iconSprites[0].sprite = PrimordialTeleporterChargedIcon;
                        self.teleporterChargeIndicatorController.playerPingColor = self.teleporterChargeIndicatorController.spriteChargingColor;
                    }
                }
            }
        }

        public static void Pings_Chests()
        {
            //Chest
            LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/Chest/Chest1").AddComponent<PingInfoProvider>().pingIconOverride = ChestIcon;

            //Chest Large
            Texture2D TexChestLargeIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestLargeIcon.png");
            ChestLargeIcon = Sprite.Create(TexChestLargeIcon, v.rec256, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Chest2").AddComponent<PingInfoProvider>().pingIconOverride = ChestLargeIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopLargeTerminal").AddComponent<PingInfoProvider>().pingIconOverride = ChestLargeIcon;

            //Chest Legendary
            Texture2D TexChestLegendaryIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestLegendaryIcon.png");
            LegendaryChestIcon = Sprite.Create(TexChestLegendaryIcon, v.recwide, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/GoldChest").AddComponent<PingInfoProvider>().pingIconOverride = LegendaryChestIcon;

            //Category Chest Small
            Texture2D TexChestDamageIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestDamageIcon.png");
            Texture2D TexChestHealingIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestHealIcon.png");
            Texture2D TexChestUtilIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestUtilIcon.png");
            Sprite ChestDamageIcon = Sprite.Create(TexChestDamageIcon, v.rec256, v.half);
            Sprite ChestHealingIcon = Sprite.Create(TexChestHealingIcon, v.rec256, v.half);
            Sprite ChestUtilIcon = Sprite.Create(TexChestUtilIcon, v.rec256, v.half);

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestDamage").AddComponent<PingInfoProvider>().pingIconOverride = ChestDamageIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestHealing").AddComponent<PingInfoProvider>().pingIconOverride = ChestHealingIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestUtility").AddComponent<PingInfoProvider>().pingIconOverride = ChestUtilIcon;


            //Category Chest Large
            Texture2D ChestLargeDamageIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestLargeDamageIcon.png");
            Texture2D ChestLargeHealIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestLargeHealIcon.png");
            Texture2D ChestLargeUtilIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestLargeUtilIcon.png");
            Sprite ChestLargeDamageIconS = Sprite.Create(ChestLargeDamageIcon, v.rec256, v.half);
            Sprite ChestLargeHealIconS = Sprite.Create(ChestLargeHealIcon, v.rec256, v.half);
            Sprite ChestLargeUtilIconS = Sprite.Create(ChestLargeUtilIcon, v.rec256, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Damage Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ChestLargeDamageIconS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Healing Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ChestLargeHealIconS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Utility Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ChestLargeUtilIconS;


            //Lunar Chest
            Texture2D TexChestLunarIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestLunarIcon.png");
            ChestLunarIcon = Sprite.Create(TexChestLunarIcon, v.rec256, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/LunarChest").AddComponent<PingInfoProvider>().pingIconOverride = ChestLunarIcon;

            //Equipment Chest
            Texture2D TexChestEquipmentIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestEquipIcon.png");
            Sprite ChestEquipmentIcon = Sprite.Create(TexChestEquipmentIcon, v.rec256, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/EquipmentBarrel").AddComponent<PingInfoProvider>().pingIconOverride = ChestEquipmentIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopEquipmentTerminal").AddComponent<PingInfoProvider>().pingIconOverride = ChestEquipmentIcon;

            //Adapative Chest
            Texture2D TexChestCasinoIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestCasinoIcon.png");
            Sprite ChestCasinoIcon = Sprite.Create(TexChestCasinoIcon, v.rechalftall, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CasinoChest").AddComponent<PingInfoProvider>().pingIconOverride = ChestCasinoIcon;

            //Invisible Chest
            Texture2D TexChestInvisibleIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestInvisibleIcon.png");
            Sprite ChestInvisibleIcon = Sprite.Create(TexChestInvisibleIcon, v.rec256, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Chest1StealthedVariant").AddComponent<PingInfoProvider>().pingIconOverride = ChestInvisibleIcon;

            //Void Chest
            Texture2D ChestVoid = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestVoid.png");
            Sprite ChestVoidS = Sprite.Create(ChestVoid, v.rechalftall, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/VoidChest").AddComponent<PingInfoProvider>().pingIconOverride = ChestVoidS;

            //Void Triple
            Texture2D ChestVoidPotential = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestVoidPotential.png");
            Sprite ChestVoidPotentialS = Sprite.Create(ChestVoidPotential, v.rectall, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/VoidTriple").AddComponent<PingInfoProvider>().pingIconOverride = ChestVoidPotentialS;

            //Lockbox
            Texture2D TexLockedIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/LockedIcon.png");
            Sprite LockedIcon = Sprite.Create(TexLockedIcon, v.rec256, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = LockedIcon;

            //Lockbox Void
            Texture2D LockedIconAlt = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/LockedIconAlt.png");
            Sprite LockedIconAltS = Sprite.Create(LockedIconAlt, v.rec256, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = LockedIconAltS;


            //Shipping Terminal
            Texture2D PingIconShippingDrone = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PingIconShippingDrone.png");
            Sprite PingIconShippingDroneS = Sprite.Create(PingIconShippingDrone, v.rec256, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestMultiShop/FreeChestMultiShop.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminal/FreeChestTerminal.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;


            //Ugly af icon Scav Bag
            Texture2D TexScavBag = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ScavBagIcon.png");
            Sprite ScavBagIcon = Sprite.Create(TexScavBag, v.rec256, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/ScavBackpack").AddComponent<PingInfoProvider>().pingIconOverride = ScavBagIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/ScavLunarBackpack").AddComponent<PingInfoProvider>().pingIconOverride = ScavBagIcon;


            //Timed Chest
            Texture2D TexChestTimedIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ChestTimedIcon.png");
            TimedChestIcon = Sprite.Create(TexChestTimedIcon, v.rec256, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/TimedChest").AddComponent<PingInfoProvider>().pingIconOverride = TimedChestIcon;
        }

        public static void Pings_Shrines()
        {
            Texture2D TexShrineBloodIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ShrineBloodIcon.png");
            Texture2D TexShrineChanceIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ShrineChanceIcon.png");
            Texture2D TexShrineHealIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ShrineHealIcon.png");
            Texture2D TexShrineMountainIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ShrineMountainIcon.png");
            Texture2D TexShrineOrderIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ShrineOrderIcon.png");
            Texture2D TexShrineGoldIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ShrineGoldIcon.png");

            Sprite ShrineBloodIcon = Sprite.Create(TexShrineBloodIcon, v.rec256, v.half);
            Sprite ShrineChanceIcon = Sprite.Create(TexShrineChanceIcon, v.rec256, v.half);
            Sprite ShrineHealIcon = Sprite.Create(TexShrineHealIcon, v.rec256, v.half);
            Sprite ShrineMountainIcon = Sprite.Create(TexShrineMountainIcon, v.rec256, v.half);
            ShrineOrderIcon = Sprite.Create(TexShrineOrderIcon, v.rec256, v.half);
            Sprite ShrineGoldIcon = Sprite.Create(TexShrineGoldIcon, v.rectall, v.half);

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineBoss").AddComponent<PingInfoProvider>().pingIconOverride = ShrineMountainIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBoss/ShrineBossSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ShrineMountainIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBoss/ShrineBossSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ShrineMountainIcon;


            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineBlood").AddComponent<PingInfoProvider>().pingIconOverride = ShrineBloodIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBlood/ShrineBloodSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ShrineBloodIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBlood/ShrineBloodSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ShrineBloodIcon;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineChance").AddComponent<PingInfoProvider>().pingIconOverride = ShrineChanceIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineChance/ShrineChanceSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ShrineChanceIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineChance/ShrineChanceSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ShrineChanceIcon;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineRestack").AddComponent<PingInfoProvider>().pingIconOverride = ShrineOrderIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineRestack/ShrineRestackSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ShrineOrderIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineRestack/ShrineRestackSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ShrineOrderIcon;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineHealing").AddComponent<PingInfoProvider>().pingIconOverride = ShrineHealIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineGoldshoresAccess").AddComponent<PingInfoProvider>().pingIconOverride = ShrineGoldIcon;


            Texture2D TexCleanseIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/CleanseIcon.png");
            Sprite CleanseIcon = Sprite.Create(TexCleanseIcon, v.rec256, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanse.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = CleanseIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSandy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = CleanseIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSnowy Variant.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = CleanseIcon;


            Texture2D pingShrineShaping = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/pingShrineShaping.png");
            Sprite pingShrineShapingS = Sprite.Create(pingShrineShaping, new Rect(0, 0, pingShrineShaping.width, pingShrineShaping.height), v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/ShrineColossusAccess.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = pingShrineShapingS;
        }

        public static void Pings_Drones()
        {
            Texture2D TexDroneHealIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/DroneHealIcon.png");
            Texture2D TexDroneEquipmentIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/DroneEquipmentIcon.png");
            Texture2D TexDroneTurretIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/DroneTurretIcon.png");
            Texture2D TexDroneMegaIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/DroneMegaIcon.png");
            Texture2D TexDroneFlameIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/DroneFlameIcon.png");
            Texture2D TexDroneMissileIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/DroneMissileIcon.png");
            Texture2D TexDroneEmergencyIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/DroneEmergencyIcon.png");

            Sprite DroneHealIcon = Sprite.Create(TexDroneHealIcon, v.rec256, v.half);
            Sprite DroneEquipmentIcon = Sprite.Create(TexDroneEquipmentIcon, v.rec256, v.half);
            Sprite DroneTurretIcon = Sprite.Create(TexDroneTurretIcon, v.rec256, v.half);
            Sprite DroneMegaIcon = Sprite.Create(TexDroneMegaIcon, v.recwide, v.half);
            Sprite DroneFlameIcon = Sprite.Create(TexDroneFlameIcon, v.rechalftall, v.half);
            Sprite DroneMissileIcon = Sprite.Create(TexDroneMissileIcon, v.rechalftall, v.half);
            Sprite DroneEmergencyIcon = Sprite.Create(TexDroneEmergencyIcon, v.rec320, v.half);

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/Turret1Broken").AddComponent<PingInfoProvider>().pingIconOverride = DroneTurretIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/Drone2Broken").AddComponent<PingInfoProvider>().pingIconOverride = DroneHealIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/EmergencyDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = DroneEmergencyIcon;  //Not Unique
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/EquipmentDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = DroneEquipmentIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/MegaDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = DroneMegaIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/FlameDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = DroneFlameIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/MissileDroneBroken").AddComponent<PingInfoProvider>().pingIconOverride = DroneMissileIcon;

        }

        public static void Pings_Printer()
        {
            Texture2D TexPrinterIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PrinterIcon.png");
            Texture2D TexDuplicatorLarge = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PrinterLargeIcon.png");
            Texture2D TexDuplicatorMili = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PrinterMiliIcon.png");
            Texture2D TexDuplicatorMili2 = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PrinterSuperLarge.png");
            Texture2D TexDuplicatorWild = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PrinterWildIcon.png");

            Sprite PrinterIcon = Sprite.Create(TexPrinterIcon, v.rec256, v.half);
            Sprite DuplicatorLarge = Sprite.Create(TexDuplicatorLarge, v.rec256, v.half);
            Sprite DuplicatorMili = Sprite.Create(TexDuplicatorMili, v.rechalfwide, v.half);
            Sprite DuplicatorMili2 = Sprite.Create(TexDuplicatorMili2, v.rechalfwide, v.half);
            Sprite DuplicatorWild = Sprite.Create(TexDuplicatorWild, v.rec256, v.half);
            //
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Duplicator").AddComponent<PingInfoProvider>().pingIconOverride = PrinterIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").AddComponent<PingInfoProvider>().pingIconOverride = DuplicatorMili;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").AddComponent<Reminders.SpawnListener>().greenPrinter = true;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary").AddComponent<PingInfoProvider>().pingIconOverride = DuplicatorMili2;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild").AddComponent<PingInfoProvider>().pingIconOverride = DuplicatorWild;




            //Scrapper
            Texture2D TexScrapperIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ScrapperIcon.png");
            ScrapperIcon = Sprite.Create(TexScrapperIcon, v.rec256, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Scrapper").AddComponent<PingInfoProvider>().pingIconOverride = ScrapperIcon;

            //
            Texture2D TexCauldronIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/CauldronIcon.png");
            CauldronIcon = Sprite.Create(TexCauldronIcon, v.rec256, v.half);

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, GreenToRed Variant").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, WhiteToGreen").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, RedToWhite Variant").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
        }

        public static void Pings_Other()
        {
            //Portals
            Texture2D TexPortalIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PortalIcon.png");
            PortalIcon = Sprite.Create(TexPortalIcon, v.rechalftall, v.half);

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




            //Guhh
            Texture2D TexNothing = new Texture2D(0, 0, TextureFormat.DXT5, false);
            Sprite NoIcon = Sprite.Create(TexNothing, v.recnothing, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").AddComponent<PingInfoProvider>().pingIconOverride = NoIcon;
            //

            //Void Eradicator
            Texture2D texExclamationRevers = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PingIconVoidEradicator.png");
            Sprite texExclamationReversS = Sprite.Create(texExclamationRevers, v.rec128, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSuppressor/VoidSuppressor.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = texExclamationReversS;


            //Fuel Array
            Sprite ShipPodIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texRescueshipIcon");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/QuestVolatileBatteryAttachment").AddComponent<PingInfoProvider>().pingIconOverride = ShipPodIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/QuestVolatileBatteryWorldPickup").AddComponent<PingInfoProvider>().pingIconOverride = ShipPodIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/SurvivorPodBatteryPanel").AddComponent<PingInfoProvider>().pingIconOverride = ShipPodIcon;

            //Lunar Seer
            Texture2D TexSeerIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/SeerIcon.png");
            SeerIcon = Sprite.Create(TexSeerIcon, v.rec256, v.half);
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/SeerStation").AddComponent<PingInfoProvider>().pingIconOverride = SeerIcon;

            //Lunar Teleporter
            Texture2D TexPrimordialTeleporter = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PrimordialTeleporterIcon.png");
            PrimordialTeleporterIcon = Sprite.Create(TexPrimordialTeleporter, v.rec512, v.half);

            Texture2D TexPrimordialTeleporterC = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PrimordialTeleporterChargedIcon.png");
            PrimordialTeleporterChargedIcon = Sprite.Create(TexPrimordialTeleporterC, v.rec512, v.half);

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/NullSafeWard").AddComponent<PingInfoProvider>().pingIconOverride = NullVentIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporter Variant").AddComponent<PingInfoProvider>().pingIconOverride = PrimordialTeleporterIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporterProngs").AddComponent<PingInfoProvider>().pingIconOverride = PrimordialTeleporterChargedIcon;

            //
            Sprite VoidDeepSymbol = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBatteryPositionIndicator.prefab").WaitForCompletion().GetComponent<RoR2.UI.ChargeIndicatorController>().iconSprites[0].sprite;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBattery.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = VoidDeepSymbol;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidCamp/VoidCamp.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = VoidDeepSymbol;


            //Pillars
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryBlood").GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryDesign").GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryMass").GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatterySoul").GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;



            //Void Cell
            Texture2D TexNullVentIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/CellIcon.png");
            NullVentIcon = Sprite.Create(TexNullVentIcon, v.rec320, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerSafeWard.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = NullVentIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerSafeWardAwaitingInteraction.prefab").WaitForCompletion().GetComponent<PingInfoProvider>().pingIconOverride = NullVentIcon;

            Texture2D SotsIcon3 = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/SotsIcon3_Shrunk.png");
            Sprite SotsIcon3S = Sprite.Create(SotsIcon3, new Rect(0, 0, 384, 384), v.half);
            //Sprite SotsIcon3S2 = Sprite.Create(SotsIcon3, v.rec320, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/ShrineHalcyonite.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = SotsIcon3S;

            //Lem Egg
            Texture2D PingLemurianEgg = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PingLemurianEgg.png");
            Sprite PingLemurianEggS = Sprite.Create(PingLemurianEgg, v.rec256, v.half);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/LemurianEgg/LemurianEgg.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = PingLemurianEggS;

            //Random Stuff
            Texture2D TexExclamationIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/ExclamationIcon.png");
            ExclamationIcon = Sprite.Create(TexExclamationIcon, v.rec192, v.half);

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/HumanFan").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/RadarTower").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
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
            //Seems to NOT find vanilla spawn cards which is good
            InteractableSpawnCard[] ISCList = Resources.FindObjectsOfTypeAll(typeof(InteractableSpawnCard)) as InteractableSpawnCard[];
            //InteractableSpawnCard[] ISCList = Object.FindObjectsOfTypeAll(typeof(InteractableSpawnCard)) as InteractableSpawnCard[];
            for (var i = 0; i < ISCList.Length; i++)
            {
                //Debug.LogWarning(ISCList[i]);
                switch (ISCList[i].name)
                {
                    case "iscDroneTable":
                        Texture2D SS2_DroneScrapper = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/SS2_DroneScrapper.png");
                        Sprite SS2_DroneScrapperS = Sprite.Create(SS2_DroneScrapper, v.rec256, v.half);
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = SS2_DroneScrapperS;
                        break;
                    case "iscCloneDrone":
                        Texture2D SS2_DroneDuplicate = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/SS2_DroneDuplicate.png");
                        Sprite SS2_DroneDuplicateS = Sprite.Create(SS2_DroneDuplicate, v.rec256, v.half);
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SS2_DroneDuplicateS;
                        break;
                    case "iscShockDrone":
                        Texture2D SS2_DroneShock = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/SS2_DroneShock.png");
                        Sprite SS2_DroneShockS = Sprite.Create(SS2_DroneShock, v.rechalftall, v.half);
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SS2_DroneShockS;
                        break;
                    case "iscCloakedShrine":
                    case "iscCloakedShrineSnowy":
                    case "iscCloakedShrineSandy":
                        Texture2D SpStShrineCloaked = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/SpStShrineCloaked .png");
                        Sprite SpStShrineCloakedS = Sprite.Create(SpStShrineCloaked, v.rec256, v.half);
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SpStShrineCloakedS;
                        break;
                    case "iscAegisShrine":
                    case "iscAegisShrineSnowy":
                    case "iscAegisShrineSandy":
                        Texture2D SpStShrineAegis = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/SpStShrineAegis.png");
                        Sprite SpStShrineAegisS = Sprite.Create(SpStShrineAegis, v.rec256, v.half);
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SpStShrineAegisS;
                        break;
                    case "iscVoidPortalInteractable":
                        Texture2D PingIconVoidEradicator = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/PingIcons/PingIconVoidEradicator.png");
                        Sprite PingIconVoidEradicatorS = Sprite.Create(PingIconVoidEradicator, v.rec128, v.half);
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = PingIconVoidEradicatorS;
                        break;
                    case "iscWhorlCellInteractable":
                        Sprite VoidDeepSymbol = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBatteryPositionIndicator.prefab").WaitForCompletion().GetComponent<RoR2.UI.ChargeIndicatorController>().iconSprites[0].sprite;
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = VoidDeepSymbol;
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
