using RoR2;
//using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQualityOfLife
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
        public static Sprite TeleporterChargedIcon = null;
        public static Sprite AttackIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texAttackIcon");
        public static Sprite CubeIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texLunarPillarIcon");
        public static Sprite ChestIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texInventoryIconOutlined");
        public static Sprite ShrineIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texShrineIconOutlined");
        public static Sprite QuestionMarkIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texMysteryIcon");

        public static GameObject NullTempPosIndicator = null;
        public static Color VoidDefault = new Color(0.8211f, 0.5f, 1, 1);
        //public static Color VoidFocused = new Color(0.3f, 3f, 4f, 1);
        public static Color VoidFocused = new Color(0f, 3.9411764f, 5f, 1f);

        public static void Start()
        {
            PingInfo();

            PingIcons.TeleporterChargedIcon = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator").GetComponent<RoR2.UI.ChargeIndicatorController>().iconSprites[0].sprite;

            On.RoR2.TeleporterInteraction.Awake += (orig, self) =>
            {
                RoR2.UI.ChargeIndicatorController original = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator").GetComponent<RoR2.UI.ChargeIndicatorController>();
                if (self.name.StartsWith("LunarTeleporter Variant(Clone)"))
                {
                    original.iconSprites[0].sprite = PingIcons.PrimordialTeleporterChargedIcon;
                }
                else
                {
                    original.iconSprites[0].sprite = PingIcons.TeleporterChargedIcon;
                }
                orig(self);
            };

            On.RoR2.TeleporterInteraction.Start += (orig, self) =>
            {
                orig(self);
                RoR2.UI.ChargeIndicatorController original = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator").GetComponent<RoR2.UI.ChargeIndicatorController>();
                if (self.name.StartsWith("LunarTeleporter Variant(Clone)"))
                {
                    original.iconSprites[0].sprite = PingIcons.PrimordialTeleporterChargedIcon;
                }
                else
                {
                    original.iconSprites[0].sprite = PingIcons.TeleporterChargedIcon;
                }
            };



            On.EntityStates.Missions.Arena.NullWard.Active.OnEnter += (orig, self) =>
            {
                orig(self);
                //if (NetworkServer.active)

                NullTempPosIndicator = UnityEngine.Object.Instantiate<GameObject>(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/PillarChargingPositionIndicator"));
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
            };

            On.EntityStates.Missions.Arena.NullWard.Complete.OnEnter += (orig, self) =>
            {
                orig(self);
                Debug.Log("Destroy NullCell Indicator");
                Object.Destroy(NullTempPosIndicator);
                self.outer.gameObject.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_TintColor", VoidDefault);

            };
        }

        public static void PingInfo()
        {
            //Debug.Log("Ping Info Loaded");

            //Sprite QuestionMarkIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texMysteryIcon");

            Texture2D TexCauldronIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexCleanseIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexExclamationIcon = new Texture2D(192, 192, TextureFormat.DXT5, false);
            Texture2D TexLockedIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexPrinterIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexScrapperIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexSeerIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);

            TexCauldronIcon.LoadImage(Properties.Resources.CauldronIcon, true);
            TexCleanseIcon.LoadImage(Properties.Resources.CleanseIcon, true);
            TexExclamationIcon.LoadImage(Properties.Resources.ExclamationIcon, true);
            TexLockedIcon.LoadImage(Properties.Resources.LockedIcon, true);
            TexPrinterIcon.LoadImage(Properties.Resources.PrinterIcon, true);
            TexScrapperIcon.LoadImage(Properties.Resources.ScrapperIcon, true);
            TexSeerIcon.LoadImage(Properties.Resources.SeerIcon, true);

            TexCauldronIcon.filterMode = FilterMode.Bilinear;
            TexCleanseIcon.filterMode = FilterMode.Bilinear;
            TexExclamationIcon.filterMode = FilterMode.Bilinear;
            TexLockedIcon.filterMode = FilterMode.Bilinear;
            TexPrinterIcon.filterMode = FilterMode.Bilinear;
            TexScrapperIcon.filterMode = FilterMode.Bilinear;
            TexSeerIcon.filterMode = FilterMode.Bilinear;

            /*TexCauldronIcon.Apply();
            TexCleanseIcon.Apply();
            TexExclamationIcon.Apply();
            TexLockedIcon.Apply();
            TexPrinterIcon.Apply();
            TexScrapperIcon.Apply();
            TexSeerIcon.Apply();*/

            CauldronIcon = Sprite.Create(TexCauldronIcon, v.rec256, v.half);
            SeerIcon = Sprite.Create(TexSeerIcon, v.rec256, v.half);
            ExclamationIcon = Sprite.Create(TexExclamationIcon, v.rec192, v.half);
            Sprite CleanseIcon = Sprite.Create(TexCleanseIcon, v.rec256, v.half);
            Sprite LockedIcon = Sprite.Create(TexLockedIcon, v.rec256, v.half);
            Sprite PrinterIcon = Sprite.Create(TexPrinterIcon, v.rec256, v.half);
            ScrapperIcon = Sprite.Create(TexScrapperIcon, v.rec256, v.half);



            //GShrineCleanse.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CleanseIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanse.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CleanseIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSandy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CleanseIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSnowy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CleanseIcon;


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LockedIcon;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/SeerStation").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = SeerIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/GoldshoresBeacon").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/RadarTower").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;

            //if (ScrapperEnable.Value == true)

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Scrapper").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ScrapperIcon;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, GreenToRed Variant").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, WhiteToGreen").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, RedToWhite Variant").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;



            Texture2D TexNothing = new Texture2D(0, 0, TextureFormat.DXT5, false);
            TexNothing.LoadImage(Properties.Resources.ExclamationIcon, true);
            Sprite NoIcon = Sprite.Create(TexNothing, v.recnothing, v.half);
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = NoIcon;


            Texture2D TexChestLegendaryIcon = new Texture2D(384, 256, TextureFormat.DXT5, false);
            TexChestLegendaryIcon.LoadImage(Properties.Resources.ChestLegendaryIcon, true);
            TexChestLegendaryIcon.filterMode = FilterMode.Bilinear;


            Texture2D TexChestInvisibleIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexChestInvisibleIcon.LoadImage(Properties.Resources.ChestInvisibleIcon, true);
            TexChestInvisibleIcon.filterMode = FilterMode.Bilinear;
            Sprite ChestInvisibleIcon = Sprite.Create(TexChestInvisibleIcon, v.rec256, v.half);


            Texture2D TexChestCasinoIcon = new Texture2D(256, 320, TextureFormat.DXT5, false);
            TexChestCasinoIcon.LoadImage(Properties.Resources.ChestCasinoIcon, true);
            TexChestCasinoIcon.filterMode = FilterMode.Bilinear;
            Sprite ChestCasinoIcon = Sprite.Create(TexChestCasinoIcon, v.rechalftall, v.half);


            Texture2D TexChestLargeIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexChestEquipmentIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexChestLunarIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);

            Texture2D TexShrineBloodIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexShrineChanceIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexShrineHealIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexShrineMountainIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexShrineOrderIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexShrineGoldIcon = new Texture2D(256, 384, TextureFormat.DXT5, false);

            Texture2D TexDroneHealIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexDroneEquipmentIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexDroneTurretIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexDroneMegaIcon = new Texture2D(384, 256, TextureFormat.DXT5, false);
            Texture2D TexDroneFlameIcon = new Texture2D(256, 320, TextureFormat.DXT5, false);
            Texture2D TexDroneMissileIcon = new Texture2D(256, 320, TextureFormat.DXT5, false);
            Texture2D TexDroneEmergencyIcon = new Texture2D(320, 320, TextureFormat.DXT5, false);

            TexChestLargeIcon.LoadImage(Properties.Resources.ChestLargeIcon, true);
            TexChestEquipmentIcon.LoadImage(Properties.Resources.ChestEquipIcon, true);
            TexChestLunarIcon.LoadImage(Properties.Resources.ChestLunarIcon, true);

            TexShrineBloodIcon.LoadImage(Properties.Resources.ShrineBloodIcon, true);
            TexShrineChanceIcon.LoadImage(Properties.Resources.ShrineChanceIcon, true);
            TexShrineHealIcon.LoadImage(Properties.Resources.ShrineHealIcon, true);
            TexShrineMountainIcon.LoadImage(Properties.Resources.ShrineMountainIcon, true);
            TexShrineOrderIcon.LoadImage(Properties.Resources.ShrineOrderIcon, true);
            TexShrineGoldIcon.LoadImage(Properties.Resources.ShrineGoldIcon, true);

            TexDroneHealIcon.LoadImage(Properties.Resources.DroneHealIcon, true);
            TexDroneEquipmentIcon.LoadImage(Properties.Resources.DroneEquipmentIcon, true);
            TexDroneTurretIcon.LoadImage(Properties.Resources.DroneTurretIcon, true);
            TexDroneMegaIcon.LoadImage(Properties.Resources.DroneMegaIcon, true);
            TexDroneFlameIcon.LoadImage(Properties.Resources.DroneFlameIcon, true);
            TexDroneMissileIcon.LoadImage(Properties.Resources.DroneMissileIcon, true);
            TexDroneEmergencyIcon.LoadImage(Properties.Resources.DroneEmergencyIcon, true);

            TexChestLargeIcon.filterMode = FilterMode.Bilinear;
            TexChestEquipmentIcon.filterMode = FilterMode.Bilinear;
            TexChestLunarIcon.filterMode = FilterMode.Bilinear;

            TexShrineBloodIcon.filterMode = FilterMode.Bilinear;
            TexShrineChanceIcon.filterMode = FilterMode.Bilinear;
            TexShrineHealIcon.filterMode = FilterMode.Bilinear;
            TexShrineMountainIcon.filterMode = FilterMode.Bilinear;
            TexShrineOrderIcon.filterMode = FilterMode.Bilinear;
            TexShrineGoldIcon.filterMode = FilterMode.Bilinear;

            TexDroneHealIcon.filterMode = FilterMode.Bilinear;
            TexDroneEquipmentIcon.filterMode = FilterMode.Bilinear;
            TexDroneTurretIcon.filterMode = FilterMode.Bilinear;
            TexDroneMegaIcon.filterMode = FilterMode.Bilinear;
            TexDroneFlameIcon.filterMode = FilterMode.Bilinear;
            TexDroneMissileIcon.filterMode = FilterMode.Bilinear;
            TexDroneEmergencyIcon.filterMode = FilterMode.Bilinear;

            ChestLargeIcon = Sprite.Create(TexChestLargeIcon, v.rec256, v.half);
            Sprite ChestEquipmentIcon = Sprite.Create(TexChestEquipmentIcon, v.rec256, v.half);

            Sprite ShrineBloodIcon = Sprite.Create(TexShrineBloodIcon, v.rec256, v.half);
            Sprite ShrineChanceIcon = Sprite.Create(TexShrineChanceIcon, v.rec256, v.half);
            Sprite ShrineHealIcon = Sprite.Create(TexShrineHealIcon, v.rec256, v.half);
            Sprite ShrineMountainIcon = Sprite.Create(TexShrineMountainIcon, v.rec256, v.half);
            //ShrineOrderIcon = Sprite.Create(TexShrineOrderIcon, v.rec256, v.half);
            Sprite ShrineGoldIcon = Sprite.Create(TexShrineGoldIcon, v.rectall, v.half);

            Sprite DroneHealIcon = Sprite.Create(TexDroneHealIcon, v.rec256, v.half);
            Sprite DroneEquipmentIcon = Sprite.Create(TexDroneEquipmentIcon, v.rec256, v.half);
            Sprite DroneTurretIcon = Sprite.Create(TexDroneTurretIcon, v.rec256, v.half);
            Sprite DroneMegaIcon = Sprite.Create(TexDroneMegaIcon, v.recwide, v.half);
            Sprite DroneFlameIcon = Sprite.Create(TexDroneFlameIcon, v.rechalftall, v.half);
            Sprite DroneMissileIcon = Sprite.Create(TexDroneMissileIcon, v.rechalftall, v.half);
            Sprite DroneEmergencyIcon = Sprite.Create(TexDroneEmergencyIcon, v.rec320, v.half);

            //Will be replaced if specific module enabled

            ChestLunarIcon = ChestIcon;
            //ChestInvisibleIcon = ChestIcon;
            LegendaryChestIcon = ChestIcon;
            ShrineOrderIcon = ShrineIcon;
            //Order

            TimedChestIcon = ChestIcon;

            Texture2D TexChestTimedIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexChestTimedIcon.LoadImage(Properties.Resources.ChestTimedIcon, true);
            TexChestTimedIcon.filterMode = FilterMode.Bilinear;



            ChestLunarIcon = Sprite.Create(TexChestLunarIcon, v.rec256, v.half);
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/EquipmentBarrel").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestEquipmentIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopEquipmentTerminal").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestEquipmentIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/LunarChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLunarIcon;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Chest2").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopLargeTerminal").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeIcon;
            TimedChestIcon = Sprite.Create(TexChestTimedIcon, v.rec256, v.half);


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CasinoChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestCasinoIcon;


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Chest1StealthedVariant").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestInvisibleIcon;

            LegendaryChestIcon = Sprite.Create(TexChestLegendaryIcon, v.recwide, v.half);
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/GoldChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LegendaryChestIcon;


            //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineBoss").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineMountainIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBoss/ShrineBossSandy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineMountainIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBoss/ShrineBossSnowy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineMountainIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineGoldshoresAccess").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineGoldIcon;

            ShrineOrderIcon = Sprite.Create(TexShrineOrderIcon, v.rec256, v.half);
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineBlood").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineBloodIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBlood/ShrineBloodSandy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineBloodIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBlood/ShrineBloodSnowy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineBloodIcon;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineChance").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineChanceIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineChance/ShrineChanceSandy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineChanceIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineChance/ShrineChanceSnowy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineChanceIcon;


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineHealing").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineHealIcon;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineRestack").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineOrderIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineRestack/ShrineRestackSandy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineOrderIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineRestack/ShrineRestackSnowy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineOrderIcon;

            //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/Turret1Broken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneTurretIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/Drone2Broken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneHealIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/EmergencyDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneEmergencyIcon;  //Not Unique
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/EquipmentDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneEquipmentIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/MegaDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneMegaIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/FlameDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneFlameIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/MissileDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneMissileIcon;
            //
            //
            Texture2D TexDuplicatorLarge = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexDuplicatorMili = new Texture2D(320, 256, TextureFormat.DXT5, false);
            Texture2D TexDuplicatorMili2 = new Texture2D(320, 256, TextureFormat.DXT5, false);
            Texture2D TexDuplicatorWild = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexDuplicatorLarge.LoadImage(Properties.Resources.PrinterLargeIcon, true);
            TexDuplicatorMili.LoadImage(Properties.Resources.PrinterMiliIcon, true);
            TexDuplicatorMili2.LoadImage(Properties.Resources.PrinterSuperLarge, true);
            TexDuplicatorWild.LoadImage(Properties.Resources.PrinterWildIcon, true);
            TexDuplicatorLarge.filterMode = FilterMode.Bilinear;
            TexDuplicatorMili.filterMode = FilterMode.Bilinear;
            TexDuplicatorMili2.filterMode = FilterMode.Bilinear;
            TexDuplicatorWild.filterMode = FilterMode.Bilinear;
            Sprite DuplicatorLarge = Sprite.Create(TexDuplicatorLarge, v.rec256, v.half);
            Sprite DuplicatorMili = Sprite.Create(TexDuplicatorMili, v.rechalfwide, v.half);
            Sprite DuplicatorMili2 = Sprite.Create(TexDuplicatorMili2, v.rechalfwide, v.half);
            Sprite DuplicatorWild = Sprite.Create(TexDuplicatorWild, v.rec256, v.half);
            //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Duplicator").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PrinterIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DuplicatorMili;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DuplicatorMili2;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DuplicatorWild;
            //
            //
            //
            Texture2D TexChestDamageIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexChestHealingIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexChestUtilIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexChestDamageIcon.LoadImage(Properties.Resources.ChestDamageIcon, true);
            TexChestHealingIcon.LoadImage(Properties.Resources.ChestHealIcon, true);
            TexChestUtilIcon.LoadImage(Properties.Resources.ChestUtilIcon, true);
            TexChestDamageIcon.filterMode = FilterMode.Bilinear;
            TexChestHealingIcon.filterMode = FilterMode.Bilinear;
            TexChestUtilIcon.filterMode = FilterMode.Bilinear;
            Sprite ChestDamageIcon = Sprite.Create(TexChestDamageIcon, v.rec256, v.half);
            Sprite ChestHealingIcon = Sprite.Create(TexChestHealingIcon, v.rec256, v.half);
            Sprite ChestUtilIcon = Sprite.Create(TexChestUtilIcon, v.rec256, v.half);

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/Chest/Chest1").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestDamage").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestDamageIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestHealing").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestHealingIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestUtility").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestUtilIcon;

            Texture2D ChestLargeDamageIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D ChestLargeHealIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D ChestLargeUtilIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            ChestLargeDamageIcon.LoadImage(Properties.Resources.ChestLargeDamageIcon, true);
            ChestLargeHealIcon.LoadImage(Properties.Resources.ChestLargeHealIcon, true);
            ChestLargeUtilIcon.LoadImage(Properties.Resources.ChestLargeUtilIcon, true);
            ChestLargeDamageIcon.filterMode = FilterMode.Bilinear;
            ChestLargeHealIcon.filterMode = FilterMode.Bilinear;
            ChestLargeUtilIcon.filterMode = FilterMode.Bilinear;
            Sprite ChestLargeDamageIconS = Sprite.Create(ChestLargeDamageIcon, v.rec256, v.half);
            Sprite ChestLargeHealIconS = Sprite.Create(ChestLargeHealIcon, v.rec256, v.half);
            Sprite ChestLargeUtilIconS = Sprite.Create(ChestLargeUtilIcon, v.rec256, v.half);
            //
            //
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Damage Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeDamageIconS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Healing Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeHealIconS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Utility Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeUtilIconS;
            //
            //
            //
            Texture2D PingIconShippingDrone = new Texture2D(256, 256, TextureFormat.DXT5, false);
            PingIconShippingDrone.filterMode = FilterMode.Bilinear;
            PingIconShippingDrone.LoadImage(Properties.Resources.PingIconShippingDrone, true);
            Sprite PingIconShippingDroneS = Sprite.Create(PingIconShippingDrone, v.rec256, v.half);

            Texture2D PingIconVoidEradicator = new Texture2D(128, 128, TextureFormat.DXT5, false);
            PingIconVoidEradicator.LoadImage(Properties.Resources.PingIconVoidEradicator, true);
            PingIconVoidEradicator.filterMode = FilterMode.Bilinear;
            Sprite PingIconVoidEradicatorS = Sprite.Create(PingIconVoidEradicator, v.rec128, v.half);


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestMultiShop/FreeChestMultiShop.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminal/FreeChestTerminal.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSuppressor/VoidSuppressor.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIconVoidEradicatorS;



            Texture2D ChestVoid = new Texture2D(256, 320, TextureFormat.DXT5, false);
            ChestVoid.LoadImage(Properties.Resources.ChestVoid, true);
            ChestVoid.filterMode = FilterMode.Bilinear;
            Sprite ChestVoidS = Sprite.Create(ChestVoid, v.rechalftall, v.half);

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/VoidChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestVoidS;

            Texture2D ChestVoidPotential = new Texture2D(256, 384, TextureFormat.DXT5, false);
            ChestVoidPotential.LoadImage(Properties.Resources.ChestVoidPotential, true);
            ChestVoidPotential.filterMode = FilterMode.Bilinear;
            Sprite ChestVoidPotentialS = Sprite.Create(ChestVoidPotential, v.rectall, v.half);

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/VoidTriple").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestVoidPotentialS;
            //
            //
            //
            Sprite VoidDeepSymbol = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBatteryPositionIndicator.prefab").WaitForCompletion().GetComponent<RoR2.UI.ChargeIndicatorController>().iconSprites[0].sprite;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBattery.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = VoidDeepSymbol;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidCamp/VoidCamp.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = VoidDeepSymbol;


            //  
            Texture2D LockedIconAlt = new Texture2D(256, 256, TextureFormat.DXT5, false);
            LockedIconAlt.LoadImage(Properties.Resources.LockedIconAlt, true);
            LockedIconAlt.filterMode = FilterMode.Bilinear;
            Sprite LockedIconAltS = Sprite.Create(LockedIconAlt, v.rec256, v.half);

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = LockedIconAltS;
            //
            //
            //
            Texture2D TexNullVentIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexNullVentIcon.LoadImage(Properties.Resources.CellIcon, true);
            TexNullVentIcon.filterMode = FilterMode.Bilinear;
            NullVentIcon = AttackIcon;
            //if (NullSafeVentEnable.Value == true)

            NullVentIcon = Sprite.Create(TexNullVentIcon, v.rec320, v.half);

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerSafeWard.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = NullVentIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerSafeWardAwaitingInteraction.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = NullVentIcon;


            Texture2D TexScavBag = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexScavBag.LoadImage(Properties.Resources.ScavBagIcon, true);
            TexScavBag.filterMode = FilterMode.Bilinear;
            Sprite ScavBagIcon = Sprite.Create(TexScavBag, v.rec256, v.half);

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/ScavBackpack").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ScavBagIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/ScavLunarBackpack").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ScavBagIcon;



            Texture2D TexPortalIcon = new Texture2D(256, 320, TextureFormat.DXT5, false);
            TexPortalIcon.LoadImage(Properties.Resources.PortalIcon, true);
            TexPortalIcon.filterMode = FilterMode.Bilinear;
            PortalIcon = Sprite.Create(TexPortalIcon, v.rechalftall, v.half);
            //
            //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArtifactworld").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArena").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalGoldshores").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalShop").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalMS").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidOutroPortal/VoidOutroPortal.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/PortalInfiniteTower.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Healing").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Shocking").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, EquipmentRestock").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Hacking").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            //
            //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryBlood").GetComponent<RoR2.PingInfoProvider>().pingIconOverride = CubeIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryDesign").GetComponent<RoR2.PingInfoProvider>().pingIconOverride = CubeIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatteryMass").GetComponent<RoR2.PingInfoProvider>().pingIconOverride = CubeIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/MoonBatterySoul").GetComponent<RoR2.PingInfoProvider>().pingIconOverride = CubeIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/NullSafeWard").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = NullVentIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/TimedChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = TimedChestIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/HumanFan").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;

            Sprite ShipPodIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texRescueshipIcon");
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/QuestVolatileBatteryAttachment").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShipPodIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/QuestVolatileBatteryWorldPickup").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShipPodIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/SurvivorPodBatteryPanel").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShipPodIcon;

            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LogPickup").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LogPickup2").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            //
            Texture2D TexPrimordialTeleporter = new Texture2D(256, 320, TextureFormat.DXT5, false);
            TexPrimordialTeleporter.LoadImage(Properties.Resources.PrimordialTeleporterIcon, true);
            TexPrimordialTeleporter.filterMode = FilterMode.Bilinear;
            PrimordialTeleporterIcon = Sprite.Create(TexPrimordialTeleporter, v.rec512, v.half);

            Texture2D TexPrimordialTeleporterC = new Texture2D(256, 320, TextureFormat.DXT5, false);
            TexPrimordialTeleporterC.LoadImage(Properties.Resources.PrimordialTeleporterChargedIcon, true);
            TexPrimordialTeleporterC.filterMode = FilterMode.Bilinear;
            PrimordialTeleporterChargedIcon = Sprite.Create(TexPrimordialTeleporterC, v.rec512, v.half);


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporter Variant").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PrimordialTeleporterIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporterProngs").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PrimordialTeleporterIcon;

            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/AmmoPack").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/BonusMoneyPack").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/HealPacks").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;

            Texture2D PingLemurianEgg = new Texture2D(256, 256, TextureFormat.DXT5, false);
            PingLemurianEgg.LoadImage(Properties.Resources.PingLemurianEgg, true);
            PingLemurianEgg.filterMode = FilterMode.Bilinear;
            Sprite PingLemurianEggS = Sprite.Create(PingLemurianEgg, v.rec256, v.half);

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/LemurianEgg/LemurianEgg.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingLemurianEggS;






            //Seekers of the Storm

            Texture2D pingShrineShaping = new Texture2D(256, 256, TextureFormat.DXT5, false);
            pingShrineShaping.LoadImage(Properties.Resources.pingShrineShaping, true);
            pingShrineShaping.filterMode = FilterMode.Bilinear;
            Sprite pingShrineShapingS = Sprite.Create(pingShrineShaping, v.rec256, v.half);

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/ShrineHalcyonite.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CubeIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Geode.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PM DestinationPortal.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/ShrineColossusAccess.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = pingShrineShapingS;




        }


        public static void ModSupport()
        {
            InteractableSpawnCard[] ISCList = Resources.FindObjectsOfTypeAll(typeof(InteractableSpawnCard)) as InteractableSpawnCard[];
            //InteractableSpawnCard[] ISCList = Object.FindObjectsOfTypeAll(typeof(InteractableSpawnCard)) as InteractableSpawnCard[];
            for (var i = 0; i < ISCList.Length; i++)
            {
                //Debug.LogWarning(ISCList[i]);
                switch (ISCList[i].name)
                {
                    case "midcDroneTable":
                        Texture2D SS2_DroneScrapper = new Texture2D(256, 256, TextureFormat.DXT5, false);
                        SS2_DroneScrapper.LoadImage(Properties.Resources.SS2_DroneScrapper, true);
                        SS2_DroneScrapper.filterMode = FilterMode.Bilinear;
                        Sprite SS2_DroneScrapperS = Sprite.Create(SS2_DroneScrapper, v.rec256, v.half);
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = SS2_DroneScrapperS;
                        break;
                    case "msidcCloneDrone":
                        Texture2D SS2_DroneDuplicate = new Texture2D(256, 256, TextureFormat.DXT5, false);
                        SS2_DroneDuplicate.LoadImage(Properties.Resources.SS2_DroneDuplicate, true);
                        SS2_DroneDuplicate.filterMode = FilterMode.Bilinear;
                        Sprite SS2_DroneDuplicateS = Sprite.Create(SS2_DroneDuplicate, v.rec256, v.half);
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SS2_DroneDuplicateS;
                        break;
                    case "msidcShockDrone":
                        Texture2D SS2_DroneShock = new Texture2D(256, 320, TextureFormat.DXT5, false);
                        SS2_DroneShock.LoadImage(Properties.Resources.SS2_DroneShock, true);
                        SS2_DroneShock.filterMode = FilterMode.Bilinear;
                        Sprite SS2_DroneShockS = Sprite.Create(SS2_DroneShock, v.rechalftall, v.half);
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SS2_DroneShockS;
                        break;
                    case "iscCloakedShrine":
                    case "iscCloakedShrineSnowy":
                    case "iscCloakedShrineSandy":
                        Texture2D SpStShrineCloaked = new Texture2D(256, 256, TextureFormat.DXT5, false);
                        SpStShrineCloaked.LoadImage(Properties.Resources.SpStShrineCloaked, true);
                        SpStShrineCloaked.filterMode = FilterMode.Bilinear;
                        Sprite SpStShrineCloakedS = Sprite.Create(SpStShrineCloaked, v.rec256, v.half);
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SpStShrineCloakedS;
                        break;
                    case "iscAegisShrine":
                    case "iscAegisShrineSnowy":
                    case "iscAegisShrineSandy":
                        Texture2D SpStShrineAegis = new Texture2D(256, 256, TextureFormat.DXT5, false);
                        SpStShrineAegis.LoadImage(Properties.Resources.SpStShrineAegis, true);
                        SpStShrineAegis.filterMode = FilterMode.Bilinear;
                        Sprite SpStShrineAegisS = Sprite.Create(SpStShrineAegis, v.rec256, v.half);
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SpStShrineAegisS;
                        break;
                    case "iscVoidPortalInteractable":
                        Texture2D PingIconVoidEradicator = new Texture2D(128, 128, TextureFormat.DXT5, false);
                        PingIconVoidEradicator.LoadImage(Properties.Resources.PingIconVoidEradicator, true);
                        PingIconVoidEradicator.filterMode = FilterMode.Bilinear;
                        Sprite PingIconVoidEradicatorS = Sprite.Create(PingIconVoidEradicator, v.rec128, v.half);
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = PingIconVoidEradicatorS;
                        break;
                }
            }

            //This is stupidd idk how else to find stuff like Broken Han-d
            //Debug.LogWarning(RedMercSkin.MercSwordSlashRed.transform.parent);
            PurchaseInteraction[] moddedPurchasesList = Resources.FindObjectsOfTypeAll(typeof(PurchaseInteraction)) as PurchaseInteraction[];
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
