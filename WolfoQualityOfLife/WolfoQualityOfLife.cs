using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using RoR2.ExpansionManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WolfoQualityOfLife
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfoQualityOfLife", "WolfoQualityOfLife", "2.0.5")]
    [R2APISubmoduleDependency(nameof(ContentAddition), nameof(LoadoutAPI), nameof(PrefabAPI), nameof(LanguageAPI), nameof(ItemAPI))]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]


    public class WolfoMain : BaseUnityPlugin
    {
        static readonly System.Random random = new System.Random();

        public static int SceneInfoTreasureCacheCount = 0;
        public static int SceneInfoTreasureCacheVoidCount = 0;
        public static int SceneInfoFreeChestCount = 0;

        public static int VoidCloverCount = 0;
        public static CharacterMaster VoidCloverMaster = null;
        public static Inventory VoidCloverInventory = null;

        public static GameObject SprintingCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/SprintingCrosshair.prefab").WaitForCompletion();
        public static RoR2.UI.CrosshairController SprintingCrosshairUI = SprintingCrosshair.GetComponent<RoR2.UI.CrosshairController>();
        public static GameObject LoaderCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Loader/LoaderCrosshair.prefab").WaitForCompletion();

        public static GameObject CaptainShockBeacon = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Captain/CaptainSupplyDrop, Shocking.prefab").WaitForCompletion().transform.GetChild(2).GetChild(0).gameObject;
        public static GameObject CaptainHackingBeaconIndicator = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Captain/CaptainSupplyDrop, Hacking.prefab").WaitForCompletion().transform.GetChild(2).GetChild(0).GetChild(4).gameObject;
        public static Material CaptainHackingBeaconIndicatorMaterial = Instantiate(CaptainHackingBeaconIndicator.transform.GetChild(0).GetComponent<MeshRenderer>().material);

        public static MusicTrackDef CreditsTrack = Addressables.LoadAssetAsync<MusicTrackDef>(key: "RoR2/Base/Common/muSong21.asset").WaitForCompletion();
        public static MusicTrackDef CreditsTrackVoid = Addressables.LoadAssetAsync<MusicTrackDef>(key: "RoR2/DLC1/Common/muMenuDLC1.asset").WaitForCompletion();


        public static Color FakeBlueEquip = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarItem);
        public static Color FakeYellowEquip = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Equipment);
        //public static Color FakeBlueYellowEquip = ColorCatalog.GetColor(ColorCatalog.ColorIndex.Equipment);
        public static Color FakeLunarCoin = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarItem);
        //public static Color LunarColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarItem);

        public static Color FakeVoidWhite = ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidItem);
        public static Color FakeVoidGreen = ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidItem);
        public static Color FakeVoidRed = ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidItem);
        public static Color FakeVoidYellow = ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidItem);


        public static Color NewSurvivorLogbookNameColor = new Color32(80, 130, 173, 255);

        public static bool DidEliteBrassBalls = true;
        public static bool LastStageGoolake = false;

        public static List<Inventory> GameOverInventories = new List<Inventory>();
        public static List<string> GameOverBodyNames = new List<string>();
        public static List<DamageReport> GameEndingDamageReports = new List<DamageReport>();



        public static GameObject LeptonDaisyTeleporterDecoration = null;

        //private static GameObject GShrineCleanse = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineCleanse");
        //private static GameObject GLockbox = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/Lockbox");
        private static GameObject RedToWhiteSoup = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, RedToWhite Variant");


        private static GameObject PrefabLockbox = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion();
        private static GameObject PrefabLockboxVoid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion();
        private static GameObject PrefabVendingMachine = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion();



        private static GameObject MoffeinClayMan = null;
        private static GameObject NullTempPosIndicator = null;
        public static Color VoidDefault = new Color(0.8211f, 0.5f, 1, 1);
        //public static Color VoidFocused = new Color(0.3f, 3f, 4f, 1);
        public static Color VoidFocused = new Color(0f, 3.9411764f, 5f, 1f);

        public static UnlockableDef ClayManLog = ScriptableObject.CreateInstance<UnlockableDef>();



        public static GameObject Duplicator = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Duplicator");
        public static GameObject DuplicatorLarge = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge");
        public static GameObject DuplicatorMili = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary");
        public static GameObject DuplicatorWild = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild");


        public static bool YellowHighlightBool = false;
        public static bool PinkHighlightBool = false;
        public static bool BlueHighlightBool = false;
        public static int HighlighterIntChoice = 0;

        public static readonly GameObject HighlightBlueItem = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightLunarItem"), "HighlightLunarItemAlt", false);
        public static readonly GameObject HighlightYellowItem = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightBossItem", false);
        public static readonly GameObject HighlightPinkItem = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier3Item"), "HighlightVoidItem", false);
        public static readonly GameObject HighlightOrangeItem = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipment", false);
        public static readonly GameObject HighlightOrangeBossItem = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipmentBoss", false);
        public static readonly GameObject HighlightOrangeLunarItem = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipmentLunar", false);

        public static readonly GameObject VEquipmentOrb = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/EquipmentOrb");
        public static readonly GameObject EquipmentBossOrb = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/BossOrb"), "EquipmentBossOrb", false);
        public static readonly GameObject EquipmentLunarOrb = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/LunarOrb"), "EquipmentLunarOrb", false);
        public static readonly GameObject NoTierOrb = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/Tier1Orb"), "NoTierOrb", false);
        public static readonly GameObject CoinOrb = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/Tier1Orb"), "CoinOrb", false);

        public static Texture2D texEquipmentBossBG = new Texture2D(512, 512, TextureFormat.DXT5, false);
        public static Texture2D texEquipmentLunarBG = new Texture2D(512, 512, TextureFormat.DXT5, false);


        public static Material MatGeyserSulfurPools = null;

        public static GameObject MithrixCrystalYellow = null;
        public static GameObject MithrixCrystalOrange = null;
        public static GameObject MithrixCrystalPink = null;
        public static GameObject MithrixCrystalPinkSmall = null;




        public static GameObject GlowFlowerForPillar = null;





        public static BuffDef FakeHellFire;
        public static BuffDef FakePercentBurn;
        public static BuffDef FakeBugWings;
        public static BuffDef FakeStrides;
        public static BuffDef FakeRoseBuckle;
        public static BuffDef FakeFrozen;
        public static BuffDef FakeHeadstompOn;
        public static BuffDef FakeHeadstompOff;
        public static BuffDef FakeFeather;
        public static BuffDef FakeShieldDelay;
        public static BuffDef FakeEgg;
        public static BuffDef FakeHellFireDuration;
        public static BuffDef FakeFrostRelic;
        public static BuffDef FakeShurikenStock;
        public static BuffDef FakeOpalCooldown;
        public static BuffDef FakeVagrantExplosion;

        public static Sprite AttackIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texAttackIcon");
        public static Sprite CubeIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texLunarPillarIcon");
        public static Sprite ChestIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texInventoryIconOutlined");
        public static Sprite ShrineIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texShrineIconOutlined");

        public static Texture RedGoldTitan = RoR2.LegacyResourcesAPI.Load<Texture>("textures/bodyicons/TitanGoldBody");
        public static Texture2D BlueGoldTitan;

        public static EquipmentIndex DeathEquip1 = EquipmentIndex.None;
        public static EquipmentIndex DeathEquip2 = EquipmentIndex.None;
        public static EquipmentIndex DeathEquipEnemy1 = EquipmentIndex.None;
        public static EquipmentIndex DeathEquipEnemy2 = EquipmentIndex.None;

        public static bool RunFirstTimeDone = false;


        public static SceneDef[] prevDestinations = new SceneDef[] { };
        //public static SceneDef[] prevDestinationsClone = new SceneDef[] { };

        /*
        public readonly ExplicitPickupDropTable dtPearls = RoR2.LegacyResourcesAPI.Load<ExplicitPickupDropTable>("droptables/dtPearls");
        public readonly ExplicitPickupDropTable.Entry Pearl = new ExplicitPickupDropTable.Entry
        {
            pickupWeight = 0.8f,
            pickupName = ("ItemIndex.Pearl")
        };
        public readonly ExplicitPickupDropTable.Entry ShinyPearl = new ExplicitPickupDropTable.Entry
        {
            pickupWeight = 0.2f,
            pickupName = ("ItemIndex.ShinyPearl")
        };
        */

        private static Rect recnothing = new Rect(0, 0, 0, 0);
        private static Rect recwide = new Rect(0, 0, 384, 256);
        private static Rect rechalftall = new Rect(0, 0, 256, 320);
        private static Rect rechalfwide = new Rect(0, 0, 320, 256);
        private static Rect rectall = new Rect(0, 0, 256, 384);
        private static Rect rec512 = new Rect(0, 0, 512, 512);
        private static Rect rec320 = new Rect(0, 0, 320, 320);
        private static Rect rec256 = new Rect(0, 0, 256, 256);
        private static Rect rec192 = new Rect(0, 0, 192, 192);
        private static Rect rec128 = new Rect(0, 0, 128, 128);
        private static Rect rec106 = new Rect(0, 0, 106, 106);
        private static Rect rec64 = new Rect(0, 0, 64, 64);
        private static Vector2 half = new Vector2(0.5f, 0.5f);

        private static Texture2D dummytex192 = new Texture2D(192, 192, TextureFormat.DXT5, false);
        private static Texture2D dummytex256 = new Texture2D(256, 256, TextureFormat.DXT5, false);
        private static Texture2D dummytexwide = new Texture2D(384, 256, TextureFormat.DXT5, false);
        private static Texture2D dummytex320 = new Texture2D(320, 320, TextureFormat.DXT5, false);
        private static Texture2D dummytex512 = new Texture2D(512, 512, TextureFormat.DXT5, false);
        private static Texture2D dummytexhalftall = new Texture2D(256, 320, TextureFormat.DXT5, false);

        public static Sprite CauldronIcon = Sprite.Create(dummytex256, rec256, half);
        public static Sprite SeerIcon = Sprite.Create(dummytex256, rec256, half);
        public static Sprite ExclamationIcon = Sprite.Create(dummytex192, rec192, half);
        public static Sprite LegendaryChestIcon = Sprite.Create(dummytexwide, recwide, half);
        public static Sprite ChestLunarIcon = Sprite.Create(dummytex256, rec256, half);
        public static Sprite ShrineOrderIcon = Sprite.Create(dummytex256, rec256, half);
        public static Sprite NullVentIcon = Sprite.Create(dummytex320, rec320, half);
        public static Sprite TimedChestIcon = Sprite.Create(dummytex256, rec256, half);
        public static Sprite PortalIcon = Sprite.Create(dummytexhalftall, rechalftall, half);
        public static Sprite PrimordialTeleporterIcon = Sprite.Create(dummytex512, rechalftall, half);
        public static Sprite PrimordialTeleporterChargedIcon = Sprite.Create(dummytex512, rechalftall, half);
        public static Sprite TeleporterChargedIcon = null;

        private static Highlight[] highlightlist;
        private static PurchaseInteraction[] purchaserlist;
        private static GenericDisplayNameProvider[] genericlist;
        //private static GenericPickupController[] pickuplist;
        //private static DummyPingableInteraction[] desertplatelist;
        //private static UnlockableGranter[] portalstatuelist;

        public static ConfigEntry<bool> SkinAcridBlight;
        public static ConfigEntry<bool> SkinMercBlueSword;
        public static ConfigEntry<bool> MessagesLootItems;
        public static ConfigEntry<bool> MessagesPortals;
        public static ConfigEntry<bool> MessagesElixirWatches;
        public static ConfigEntry<bool> MessagesBenthicEgo;
        public static ConfigEntry<bool> LysateCellHuntress;
        //public static ConfigEntry<bool> LysateCellCaptain;
        public static ConfigEntry<bool> ConfigSprintingCrosshair;

        public static ConfigEntry<bool> EclipseDifficultyAlways;
        //public static ConfigEntry<bool> EclipseArtifacts;
        public static ConfigEntry<bool> PrismaticTrialsOffline;
        public static ConfigEntry<bool> PrismaticTrialsDIY;
        public static ConfigEntry<int> PrismaticTrialsStageLimit;
        public static ConfigEntry<int> PrismaticTrialsEliteBossCount;
        public static ConfigEntry<string> PrismaticTrialsAllElites;
        public static ConfigEntry<uint> PrismaticTrialsCrystalsNeeded;
        public static ConfigEntry<uint> PrismaticTrialsCrystalsTotal;

        public static ConfigEntry<bool> FasterPrinter;
        public static ConfigEntry<bool> FasterScrapper;
        public static ConfigEntry<bool> FasterShrines;
        //public static ConfigEntry<float> YellowPercentage;

        public static ConfigEntry<bool> MoreLogEntries;
        public static ConfigEntry<bool> MoreLogEntriesAspect;
        public static ConfigEntry<bool> LimboEndingMildChange;

        public static ConfigEntry<bool> EquipmentDroneName;
        public static ConfigEntry<bool> RemoveEngiTurretEquip;

        public static ConfigEntry<string> LunarChimeraNameChange;
        //public static ConfigEntry<bool> RuinSound;
        public static ConfigEntry<bool> AlwaysDisplayLockedArti;
        public static ConfigEntry<bool> ConsumedRustedKeyConfig;
        //public static ConfigEntry<bool> UntieredCaptainConfig;
        //public static ConfigEntry<bool> TwistedScavBeads;
        public static ConfigEntry<bool> EquipmentInDeath;
        public static ConfigEntry<bool> SkinTouchUps;
        public static ConfigEntry<bool> ColorfulBalls;
        public static ConfigEntry<bool> BlightAcrid;

        public static ConfigEntry<bool> MercOniRedSword;
        public static ConfigEntry<bool> MercOniRedEffects;
        public static ConfigEntry<bool> REXGreenEffects;


        public static ConfigEntry<bool> UpdateBodyIconsConfig;

        /*
        public static ConfigEntry<bool> EngiWalkerTurretIconConfig;
        public static ConfigEntry<bool> ProbeIconsConfig;
        public static ConfigEntry<bool> BeetleAllyIconConfig;
        public static ConfigEntry<bool> MissileDroneIconConfig;
        public static ConfigEntry<bool> GoldTitanIconConfig;
        public static ConfigEntry<bool> BrotherHurtIconConfig;
        public static ConfigEntry<bool> BlueSquidTurret;
        public static ConfigEntry<bool> BlueHereticConfig;
        */

        public static ConfigEntry<bool> GuaranteedRedToWhite;
        public static ConfigEntry<bool> ThirdLunarSeer;
        public static ConfigEntry<bool> CommencementSeer;
        public static ConfigEntry<bool> RepeatableVoid;

        //public static ConfigEntry<float> FamilyEventChance;
        //public static ConfigEntry<bool> FamilyEventAdditions;

        public static ConfigEntry<bool> MountainShrineStack;

        /*
        public static ConfigEntry<bool> StupidPriceChangerConfig;
        public static ConfigEntry<bool> NoLunarCost;
        public static ConfigEntry<int> HealingShrineCost;
        public static ConfigEntry<float> HealingShrineMulti;
        public static ConfigEntry<int> GoldShrineCost;
        public static ConfigEntry<int> RadarScannerCost;
        public static ConfigEntry<float> BloodShrineMulti;
        public static ConfigEntry<int> EquipmentMultishopCost;
        public static ConfigEntry<int> RedSoupAmount;
        public static ConfigEntry<int> ScrapperAmount;

        public static ConfigEntry<int> GunDroneCost;
        public static ConfigEntry<int> HealingDroneCost;
        public static ConfigEntry<int> MissileDroneCost;
        public static ConfigEntry<int> FlameDroneCost;
        public static ConfigEntry<int> EmergencyDroneCost;
        public static ConfigEntry<int> MegaDroneCost;
        public static ConfigEntry<int> TurretDroneCost;
        */

        //public static ConfigEntry<string> PlayerBurnColor;
        //public static ConfigEntry<bool> EnemyPercentColor;
        public static ConfigEntry<string> BugWingsVisual;
        public static ConfigEntry<bool> StridesVisual;
        public static ConfigEntry<bool> ChangeRepeatBuffColors;
        public static ConfigEntry<bool> HelfireDebuffConfig;
        public static ConfigEntry<bool> RoseBuckleVisual;
        public static ConfigEntry<bool> FrozenVisual;
        //public static ConfigEntry<bool> SleepVisual;
        public static ConfigEntry<bool> HeadstomperVisual;
        public static ConfigEntry<bool> FeatherVisual;
        public static ConfigEntry<bool> ShieldRegenVisual;
        public static ConfigEntry<bool> EggVisualA;
        public static ConfigEntry<bool> TinctureVisual;
        public static ConfigEntry<bool> FrostRelicVisual;

        public static ConfigEntry<bool> PingIcons;
        //public static ConfigEntry<bool> PingIconsExpensive;


        /*
        public static ConfigEntry<bool> ChestMiscEnable;
        public static ConfigEntry<bool> ChestLargeEnable;
        public static ConfigEntry<bool> ChestCategoryEnable;
        public static ConfigEntry<bool> ChestInvisEnable;
        public static ConfigEntry<bool> ChestLegendaryEnable;
        public static ConfigEntry<bool> ChestCasinoEnable;

        public static ConfigEntry<bool> ShrinesRestEnable;
        public static ConfigEntry<bool> ShrinesMountainEnable;
        public static ConfigEntry<bool> ShrinesGoldEnable;

        public static ConfigEntry<bool> DronesMainEnabled;
        public static ConfigEntry<bool> DronesMegaEnabled;
        public static ConfigEntry<bool> DronesOffensiveEnable;
        public static ConfigEntry<bool> DronesEmergencyEnable;

        public static ConfigEntry<bool> PrintersSpecificEnable;
        public static ConfigEntry<bool> ScrapperEnable;
        public static ConfigEntry<bool> NullSafeVentEnable;
        public static ConfigEntry<bool> PrimordialTPEnable;
        */

        public static bool ChangedColors = false;

        public static ConfigEntry<bool> ArtifactOutline;
        public static ConfigEntry<bool> LunarCoinColorOutline;
        public static ConfigEntry<bool> EnableColorChangeModule;
        public static ConfigEntry<bool> YellowEliteEquip;
        public static ConfigEntry<bool> LunarYellowEliteEquipTexture;

        /*
        public static ConfigEntry<bool> YellowEliteEquipTexture;
        public static ConfigEntry<string> FakeBlueEquipColor;
        public static ConfigEntry<string> FakeYellowEquipColor;
        public static ConfigEntry<string> FakeBlueYellowEquipColor;
        public static ConfigEntry<string> FakeLunarCoinColor;
        */

        public static bool ChangeDropletColor = true;

        public static ConfigEntry<bool> OldModelDupli;
        public static ConfigEntry<bool> OldModelDupliLarge;
        public static ConfigEntry<bool> OldModelDupliMili;
        public static ConfigEntry<bool> OldModelDupliWild;

        public static ConfigEntry<string> WolfoPrivate;

        public static ConfigEntry<bool> EnableDeathMessage;
        public static ConfigEntry<bool> EnableTradeMessage;
        public static ConfigEntry<bool> DummyModelViewer;

        //public static ConfigEntry<int> MainMenuColorConfig;



        //public static AssetBundle MainAssetBundle = null;




        //public static GameObject EquipIcon1;
        //public static GameObject EquipIcon2;
        //public static GameObject loguiparent;


        private void InitConfig()
        {
            FasterPrinter = Config.Bind(
                "2c - Faster Interactables",
                "Faster Printers",
                true,
                ""
            );
            FasterScrapper = Config.Bind(
                "2c - Faster Interactables",
                "Faster Scrappers",
                true,
                ""
            );
            FasterShrines = Config.Bind(
                "2c - Faster Interactables",
                "Time between Shrine uses",
                true,
                ""
            );
            PingIcons = Config.Bind(
                "1 - Config",
                "Enable/Disable Ping Icons",
                true,
                "Enable or Disable new Ping Icons in general"
            );
            MoreLogEntries = Config.Bind(
                "2d - Logbook",
                "More log entries (Main)",
                true,
                "Should the Volatile Battery, Twisted Scav, Newt, Malachite Urchin have a log Entry\nTwisted Scav and Newt Log is unlocked by their respective areas logbook, Malachite Urchin Log is unlocked by looping for the first time."
            );
            MoreLogEntriesAspect = Config.Bind(
                "2d - Logbook",
                "More log entries (Aspects)",
                true,
                "Should the Elite Aspects have a log Entry.\nIf you're making the Aspects Boss Equipment via Color Changer; Boss Equipment won't normally have a yellow background, download BossEquipmentFix to fix that.\nIf you're using ZetAspects disable this or that mods config else it'll result in duplicate entries for all aspects."
            );


            UpdateBodyIconsConfig = Config.Bind(
                "1 - Config",
                "More unique Body Icons",
                true,
                "Add new or updated Body Icons for things such as Engi Walker Turrets, Empathy Cores or blue outline around Beetle Guard Ally"
            );

            /*
            EngiWalkerTurretIconConfig = Config.Bind(
                "2b - Body Icons",
                "Unique Icon for Engis Walker Turrets",
                true,
                ""
            );
            ProbeIconsConfig = Config.Bind(
                "2b - Body Icons",
                "Unique Icon for Empathy Core Probes",
                true,
                ""
            );
            BeetleAllyIconConfig = Config.Bind(
                "2b - Body Icons",
                "Updated Icon for Beetle Guard Ally",
                true,
                "Red to Blue outline"
            );
            MissileDroneIconConfig = Config.Bind(
                "2b - Body Icons",
                "Updated Icon for Missile Drone",
                true,
                "Didn't have an outline"
            );
            BlueSquidTurret = Config.Bind(
                "2b - Body Icons",
                "Updated Icon for Squid Turret",
                true,
                "Green to Blue outline."
            );

            BlueHereticConfig = Config.Bind(
                "2b - Body Icons",
                "Updated Icon for Heretic",
                true,
                "Red to Blue outline. Not sure why she has a red outline."
            );
            GoldTitanIconConfig = Config.Bind(
                "2b - Body Icons",
                "Updated Icon for Aurelionite",
                false,
                "Red to Blue outline. Both Friendly and Enemy due to limitations"
            );
            BrotherHurtIconConfig = Config.Bind(
                "2b - Body Icons",
                "Unique Icon for Hurt Mithrix",
                true,
                ""
            );
            */
            //////////////////////////

            FrozenVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon to being Frozen",
                true,
                "Adds 4 stacks depleting one every half second as a timer.\nHelps Artificer gameplay"
            );

            FeatherVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon for Hopoo Feathers",
                true,
                "One stack of visual Feather Buff for each bonus mid air jump you can make due to Hopoo Feathers\nCurrently will start displaying the buff after you hit the ground for the next time instead of right after you pick up the item."
            );

            ShieldRegenVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon for Shield Regen Delay",
                true,
                "Same icon as Medkit Delay but to show you how long until shields regenerate."
            );

            HeadstomperVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon to Headstomper usage",
                true,
                "Ready and Cooldown icons just like the bands."
            );
            FrostRelicVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon to Frost Relic effect",
                true,
                "Stacks equals active kills"
            );


            StridesVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon to Strides of Heresy usage",
                true,
                ""
            );
            RoseBuckleVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon to Rose Buckler usage",
                true,
                "Might be easy to forget that you have this item and a buff indicator could remind you"
            );

            BugWingsVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff icon to Milky Chrysalis usage",
                "BugupIcon",
                "Options:\nDisabled\nBugupIcon\nMovespeedIcon\n(It used to have a buff with extra speed but it got removed)"
            );
            TinctureVisual = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon to Helfire Tincture usage",
                true,
                "Use BetterUI Buff Timers"
            );
            
            EggVisualA = Config.Bind(
                "2a - Buff Icons",
                "Add buff Icon for being in Volcanic Egg",
                true,
                "Use BetterUI Buff Timers"
            );
            

            HelfireDebuffConfig = Config.Bind(
                "2a - Buff Icons",
                "Debuff Icon for Helfire Tincutre DoT",
                true,
                "Only DoT to not have a debuff icon"
            );
            /*
            EnemyPercentColor = Config.Bind(
                "2a - Buff Icons",
                "Change color of Enemy Burn",
                true,
                "Makes enemy inflicted burns or PercentBurns have a Red Buff Icon"
            );
            */
            ChangeRepeatBuffColors = Config.Bind(
                "2a - Buff Icons",
                "Change Colors of buffs that use the same color/symbol",
                true,
                ""
            );
            /*
            SleepVisual = Config.Bind(
                "2a - Buff Icons",
                "Acrid Sleep tweaks",
                true,
                "Make Acrid immune and make enemies not target him while he's sleeping."
            );
            */



            EnableDeathMessage = Config.Bind(
                "2x - Extra Info",
                "Enable Detailed Death Messages",
                true,
                "Death Messages telling you who died to what or in what way."
            );
            EnableTradeMessage = Config.Bind(
                "2x - Extra Info",
                "Enable Item Loss Messages",
                true,
                "When you Scrap, Print, Cleanse, Reforge, Buy an Equipment Drone, etc. send a message telling everyone what item you lost."
            );
            EquipmentDroneName = Config.Bind(
                "2x - Extra Info",
                "Equipment Name under Equipment Drone name",
                true,
                "Equipment Drone\n(Equipment Name)"
            );
            MountainShrineStack = Config.Bind(
                            "2x - Extra Info",
                            "Show a Mountain Shrine symbol for every Mountain Shrine you activated",
                            true,
                            "Helps you count and looks funny for very high amounts"
                        );

            MessagesLootItems = Config.Bind(
                "2x - Extra Info",
                "Objective Reminders for Lockboxes and Delivery",
                true,
                "Friendly reminder to open your Lockboxes and get your free items from Shipping Form."
            );

            MessagesBenthicEgo = Config.Bind(
                "2x - Extra Info",
                "Transformation Messages for Benthic Bloom and Egocentrism",
                true,
                "Messages only viewable by the person that has those items. Helpful for when the item notifcations lag behind or you just miss it."
            );
            MessagesElixirWatches = Config.Bind(
                "2x - Extra Info",
                "Messages when a Elixir or Watch gets used",
                true,
                "Global Messages. Tells you how many Watches or Elixir got used up. For when the item notifications lag behind or just another way to tell."
            );

            MessagesPortals = Config.Bind(
                "2x - Extra Info",
                "Objectives for Portals",
                true,
                "Just looks nice but can be disabled if you think it's cluttery."
            );


            ///////////////////////////
            SkinTouchUps = Config.Bind(
                "2y - Visual Details",
                "Fix up some small things for vanilla skins",
                true,
                "Red Sword Oni Merc, Green Vine Smoothie Rex, Proper Color Power Mode Janitor MulT, Proper Glowing EOD Engi"
            );
            MercOniRedSword = Config.Bind(
                "2y - Visual Details",
                "Oni Merc Red Sword and Red Attack Visuals",
                true,
                "To fit better with his Red skin"
            );
            SkinMercBlueSword = Config.Bind(
                "2y - Visual Details",
                "Oni Merc Blue Sword Skin",
                true,
                "Separate Skin for people who might like both or might play with people that like one but not the other."
            );


            BlightAcrid = Config.Bind(
                "2y - Visual Details",
                "Blight Acrid attacks different color",
                true,
                "Attacks that apply blight are colored Purple/Orange instead of Green/Yellow."
            );
            SkinAcridBlight = Config.Bind(
                "2y - Visual Details",
                "Blight Default Acrid Skin",
                true,
                "A skin to use instead when using Blight and the Default Skin as the Green would not match"
            );

            ColorfulBalls = Config.Bind(
                "2y - Visual Details",
                "Elite Brass Contraption colored Balls",
                true,
                "They shoot balls that look similiar to their elite coloration."
            );


            ///////////////////////////
            ConfigSprintingCrosshair = Config.Bind(
                "2z - Misc Tweaks",
                "Sprinting Crosshair Changes",
                true,
                "Should this mod replace the Sprint crosshair with a different one that works with charging abilities."
            );

            ConsumedRustedKeyConfig = Config.Bind(
                "2z - Misc Tweaks",
                "Consumed Rusted Keys",
                true,
                "Just like how Dio leaves a Consumed Dio"
            );
            /*
            MainMenuColorConfig = Config.Bind(
                "2z - Misc Tweaks",
                "Main Menu Background Reverter",
                0,
                "Change the look of the main menu scene back to what it was during updates. Scav will always be there\n 0 : Default\n 1 : CU 1 Scorched Acres, orange and sunny with embers\n2 : CU 2 Sirens Call, deep green with a broken ship in the background\n3 : CU 3, purple with an acid stain\n4 : CU 4, pink with the artifact teleporter in the background"
            );
            */


            LunarChimeraNameChange = Config.Bind(
                "2z - Misc Tweaks",
                "Lunar Chimera name changes",
                "Long",
                "Change the name of the 3 Lunar Chimera\nPossible Options\nShort :       Lunar Wisp, Lunar Golem, Lunar Exploder\nLong :       Lunar Chimera (Wisp), Lunar Chimera (Golem), Lunar Chimera (Exploder)\nVanilla :   Lunar Chimera, Lunar Chimera, Lunar Chimera"
            );





            EquipmentInDeath = Config.Bind(
                "2d - Logbook",
                "Show Equipment alongside Items in end of run report",
                true,
                "Shows your equipment after all the items"
            );

            LimboEndingMildChange = Config.Bind(
                "2d - Logbook",
                "Limbo Ending log tweaks",
                true,
                "When you beat Twisted Scavs the Icons will be White instead of blue/purple like when oblitarating and will say At Peace... in reference to the Moment Whole logbook\nThis will be updated in accordance to the lore"
            );
            AlwaysDisplayLockedArti = Config.Bind(
                "2z - Misc Tweaks",
                "Always display Locked Artificer",
                false,
                ""
            );




            /*
            UntieredCaptainConfig = Config.Bind(
                "2z - Misc Tweaks",
                "Untiered Defensive Microbots",
                false,
                "There's no reason for Defensive Microbots to be a Red item but also close to no reason to change it. But for the few that want to they can."
            );
            */
            //////////////////////////////
            OldModelDupli = Config.Bind(
                "2e - Old Printer",
                "(White) Printer Old Model",
                false,
                "Change the model of (White) Printers to the bigger bulkier one used before 1.0"
            );
            OldModelDupliLarge = Config.Bind(
                "2e - Old Printer",
                "(Green) Printer Old Model",
                true,
                "Change the model of (Green) Printers to the bigger bulkier one used before 1.0"
            );
            OldModelDupliMili = Config.Bind(
                "2e - Old Printer",
                "(Red) Printer Old Model",
                true,
                "Change the model of (Red) Printers to the bigger bulkier one used before 1.0"
            );
            OldModelDupliWild = Config.Bind(
                "2e - Old Printer",
                "(Yellow) Printer Old Model",
                false,
                "Change the model of (Yellow) Printers to the bigger bulkier one used before 1.0"
            );

            //////////////////////////////
            /*
            ChestMiscEnable = Config.Bind(
                "4a - Ping Icons - Chests",
                "Chests - Misc",
                true,
                "Icons for: Equipment Barrel/Multipshop, Lunar Buds"
            );
            ChestCategoryEnable = Config.Bind(
                "4a - Ping Icons - Chests",
                "Chests - Category",
                true,
                "Icons for: Damage Chest, Healing Chest, Utility Chest"
            );
            ChestLargeEnable = Config.Bind(
                "4a - Ping Icons - Chests",
                "Chests - Large",
                true,
                "Icon for: Large Chest/Large Multishop/Casino Chest"
            );
            ChestInvisEnable = Config.Bind(
                "4a - Ping Icons - Chests",
                "Chests - Invisible",
                true,
                "Icon for: Invisible Chest"
            );
            ChestLegendaryEnable = Config.Bind(
                "4a - Ping Icons - Chests",
                "Chests - Legendary",
                true,
                "Icon for: Legendary Chest"
            );
            ChestCasinoEnable = Config.Bind(
                "4a - Ping Icons - Chests",
                "Chests - Casino",
                true,
                "Icon for: Casino Chest (Adaptive Chest)\nIf disabled but Large Chest enabled it will use that icon."
            );
            //////////////////////////////
            ShrinesRestEnable = Config.Bind(
                "4b - Ping Icons - Shrines",
                "Shrines - Rest",
                true,
                "Icons for: Chance Shrine, Blood Shrine, Healing Shrine, Order Shrine"
            );
            ShrinesGoldEnable = Config.Bind(
                "4b - Ping Icons - Shrines",
                "Shrines - Gold",
                true,
                "Icons for: Gold Shrine"
            );
            ShrinesMountainEnable = Config.Bind(
                "4b - Ping Icons - Shrines",
                "Shrines - Mountain",
                true,
                "Icons for: Mountain Shrine"
            );
            //////////////////////////////
            DronesMainEnabled = Config.Bind(
                "4c - Ping Icons - Drones",
                "Drones - Main",
                true,
                "Icons for: Turret, Healing Drone/Emergency Drone, Equipment Drone"
            );
            DronesMegaEnabled = Config.Bind(
                "4c - Ping Icons - Drones",
                "Drones - Mega",
                true,
                "Icon for: Mega Drone (TC 280)"
            );
            DronesOffensiveEnable = Config.Bind(
                "4c - Ping Icons - Drones",
                "Drones - Offensive",
                true,
                "Icons for: Flame Drone, Missile Drone"
            );
            DronesEmergencyEnable = Config.Bind(
                "4c - Ping Icons - Drones",
                "Drones - Emergency",
                true,
                "Icon for: Emergency Drone\nIf disabled will use Healing Drone icon if that is enabled."
            );
            ///////////////////////////////
            PrintersSpecificEnable = Config.Bind(
                "4d - Ping Icons - Misc",
                "Printers - Specific",
                true,
                "Icons for: Large Printer, Mili Tech Printer, Overgrown Printer\nIf false will use default Printer Icon."
            );
            ScrapperEnable = Config.Bind(
                "4d - Ping Icons - Misc",
                "Scrapper",
                true,
                "Icons for: Scrapper"
            );
            NullSafeVentEnable = Config.Bind(
                "4d - Ping Icons - Misc",
                "Void Fields Cell Vent",
                true,
                "If true will use unique icon, if fales will use generic Attack icon. Used for both Pinging and Charging."
            );
            PrimordialTPEnable = Config.Bind(
                "4d - Ping Icons - Misc",
                "Primordial Teleporter Unique Icon",
                true,
                "Unique sprite for pinging and charging Primordial Teleporter based on first games sprite for Divine Teleporter"
            );
            */

            ///////////////////////////////
            EnableColorChangeModule = Config.Bind(
                "3a - Color Changer - Module",
                "Enable Module",
                true,
                "If this is turned off no colors will be changed\nChange the outline/chat color for Lunar/Elite/Boss Equipment and Lunar Coin\nMust Include Hashtag (They can't be shown in the config description)#\nSome default vanilla color values\nEquipment Orange: #FF8000\nLunar Blue: #307FFF\nBoss Yellow: #FFEB04\nLunar Coin Cost Pink: #C6ADFA"
            );


            YellowEliteEquip = Config.Bind(
                "3a - Color Changer - Module",
                "Yellow Elite Equip",
                true,
                "Change the description color and make them glow yellow in the overworld\nIdeal if you're making them Yellow/Yellow-Orange"
            );

            /*
            YellowEliteEquipTexture = Config.Bind(
                "3a - Color Changer - Module",
                "Yellow Elite Equip (Texture)",
                true,
                "Replace the Elite Equipment Icons with ones that have a Yellow outline\nShould work with modded ones too."
            );
            */

            LunarYellowEliteEquipTexture = Config.Bind(
                "3a - Color Changer - Module",
                "Yellow Elite Equip for (Perfected Elite Equip) (Both)",
                false,
                "Should the changes that are enabled above also apply to Shared Design\nIt normally has a blue outline"
            );

            LunarCoinColorOutline = Config.Bind(
                "3a - Color Changer - Module",
                "Lunar Coin Outline",
                true,
                "If true Lunar Coins use their pinkish color instead or their vanilla blue color."
            );

            ArtifactOutline = Config.Bind(
                "3a - Color Changer - Module",
                "Artifact Outline",
                false,
                "Should Artifact Pickups in the Artifact World have a purple (Artifact Color) outline."
            );

            /*
            FakeBlueEquipColor = Config.Bind(
                "3b - Color Changer - Color Values",
                "Lunar Equipment Color",
                "#78AFFF",
                "What color should be used for Lunar Equipment (Hex)\nDefault is a lighter very slightly purple blue\nVanilla is Lunar Blue"
            );
            FakeYellowEquipColor = Config.Bind(
                "3b - Color Changer - Color Values",
                "Boss Equipment Color",
                "#FFC211",
                "What color should be used for Elite/Boss Equipment (Hex)\nDefault is a yellowish orange\nVanilla is Equipment Orange"
            );
            FakeBlueYellowEquipColor = Config.Bind(
                "3b - Color Changer - Color Values",
                "Elite+Lunar Equipment Color",
                "#DEDEDE",
                "What color should be used for Elite/Boss Lunar Equipment (Hex)\nShared Design (Perfected Lunar Equipment) is the only normal item that falls in this catagory\nDefault is a less bright white\nVanilla is Equipment Orange"
            );
            FakeLunarCoinColor = Config.Bind(
                "3b - Color Changer - Color Values",
                "Lunar Coin Color",
                "#C6ADFA",
                "What color should be used for Lunar Coins (Hex)\nDefault is a Lunar Coin Cost mild pink\nVanilla is Lunar Blue"
            );
            */
            /*
StartingCash = Config.Bind(
    "7 - Gameplay Main",
    "Starting Cash",
    (uint)15,
    "How much money you start stage 1 with;"
);
*/
            LysateCellHuntress = Config.Bind(
              "4a - Item Tweaks",
              "Lysate Cell Huntress",
              true,
              "Lysate Cell will combined up to 3 casts of Ballista if you have the stock available."
            );
            /*
            LysateCellCaptain = Config.Bind(
                "4a - Item Tweaks",
                "Lysate Cell Captain",
                true,
                "Should Lysate Cell work on Captain. Why do people want to disable this"
            );
            */


            GuaranteedRedToWhite = Config.Bind(
                "7a - Misc Tweaks",
                "Guaranteed RedToWhite Cauldron on Commencement",
                true,
                "It does not guarantee it will be a good item in any way. Simply makes it more consistent at getting rid of Red Scrap if you have any."
            );

            ThirdLunarSeer = Config.Bind(
                "7c - Bazaar Tweaks",
                "Third Lunar Seer",
                true,
                "Future proofing for when they add more stage without really making the bazaar more powerful."
            );
            CommencementSeer = Config.Bind(
                "7c - Bazaar Tweaks",
                "Commencement Lunar Seer",
                false,
                "Lunar Seer to Commencement after Skymeadow/1st stage of loop"
            );


            PrismaticTrialsOffline = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Prismatic Trials Offline",
                true,
                "Play Prismatic Trials despite having a modded version, seed is randomly generated each time and your victory should not be recorded anywhere. Also should allow for Mulitiplayer"
            );
            PrismaticTrialsStageLimit = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Prismatic Trials - Stage Limit",
                2,
                "How many stages until the Trial ends."
            );
            PrismaticTrialsEliteBossCount = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Prismatic Trials - Elite Bosses",
                2,
                "At which stage onwards should Elite Bosses be forced. Normally an Elite Boss is forced stage 2 and onwards. They have 10% more damage and 50% more health as they aren't real elites.\nAny boss encounter is forced to be elite even if they can't normally be one."
            );
            PrismaticTrialsAllElites = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Prismatic Trials - Extra Elite types for bosses",
                "Enabled",
                "Normally the forced Elite Bosses can only be Overloading or Blazing.\nEnabled: To allow them to spawn as any Tier 1 Elite\nLunar: To allow them to spawn as any Tier 1 Elite and Perfected\nDisabled: To not change anything"
            );
            PrismaticTrialsCrystalsNeeded = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Prismatic Trials - Crystals Needed",
                (uint)3,
                "How many Crystals need to be killed to progress."
            );
            PrismaticTrialsCrystalsTotal = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Prismatic Trials - Crystals Total",
                (uint)5,
                "How many Crystals will spawn in total."
            );

            PrismaticTrialsDIY = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Prismatic Trials - Customizability",
                false,
                "Instead of being random Artifacts, allow yourself to choose which Artifacts you wanna use. Seed gets randomized every time instead of only when entering main menu."
            );




            /*
            EclipseArtifacts = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Eclipse allows Artifacts",
                false,
                "Allow Artifacts to be chosen in the proper Eclipse game mode"
            );
            */
            EclipseDifficultyAlways = Config.Bind(
                "7b - Gameplay - Vanilla Game Modes",
                "Eclipse Difficulty always available",
                false,
                "Allows you to choose the 8 Eclipse difficulties in normal lobbies. Doesn't break the Eclipse gamemode but it becomes redundant."
            );




            ///////////////////////////////
            DummyModelViewer = Config.Bind(
                "9 - Testing",
                "Sort of Model Viewer",
                false,
                "Just adds basically all mobs to the Logbook"
            );



        }


        public static readonly Func<ItemIndex, bool> Tier1DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier1DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Tier2DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier2DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Tier3DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier3DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> BossDeathItemFilterDelegate = new Func<ItemIndex, bool>(BossDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> LunarDeathItemFilterDelegate = new Func<ItemIndex, bool>(LunarDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Void1DeathItemFilterDelegate = new Func<ItemIndex, bool>(Void1DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Void2DeathItemFilterDelegate = new Func<ItemIndex, bool>(Void2DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Void3DeathItemFilterDelegate = new Func<ItemIndex, bool>(Void3DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Void4DeathItemFilterDelegate = new Func<ItemIndex, bool>(Void4DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> NoTierDeathItemFilterDelegate = new Func<ItemIndex, bool>(NoTierDeathItemCopyFilter);
        private static bool NoTierDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.NoTier) { return false; }

            if (!tempdef.pickupIconTexture || tempdef.pickupIconTexture.name.StartsWith("texNullIcon"))
            {
                return false;
            }
            return true;
        }
        private static bool Tier1DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier1) { return false; }
            return true;
        }
        private static bool Tier2DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier2) { return false; }
            return true;
        }
        private static bool Tier3DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier3) { return false; }
            return true;
        }
        private static bool LunarDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Lunar) { return false; }
            return true;
        }
        private static bool BossDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Boss) { return false; }
            return true;
        }

        private static bool Void1DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.VoidTier1) { return false; }
            return true;
        }
        private static bool Void2DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.VoidTier2) { return false; }
            return true;
        }
        private static bool Void3DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.VoidTier3) { return false; }
            return true;
        }
        private static bool Void4DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.VoidBoss) { return false; }
            return true;
        }

        public static AssetBundle MainAssetBundle = LoadAssetBundle(Properties.Resources.oldduplicatorbundle);

        public static UnityEngine.GameObject FBXDuplicatorOld = MainAssetBundle.LoadAsset<GameObject>("Assets/mdlDuplicatorOld/mdlDuplicatorOld.fbx");
        public static UnityEngine.GameObject FBXDuplicatorOld2 = MainAssetBundle.LoadAsset<GameObject>("Assets/mdlDuplicatorOld2/mdlDuplicatorOld2.fbx");
        public static UnityEngine.GameObject FBXDuplicatorOldMili = MainAssetBundle.LoadAsset<GameObject>("Assets/mdlDuplicatorMiliOld/mdlDuplicatorMiliOld.fbx");
        public static UnityEngine.GameObject FBXDuplicatorOldWild = MainAssetBundle.LoadAsset<GameObject>("Assets/mdlDuplicatorWildOld/mdlDuplicatorWildOld.fbx");
        public static UnityEngine.RuntimeAnimatorController OldAnimControll = MainAssetBundle.LoadAsset<RuntimeAnimatorController>("Assets/animationcontroller/animDuplicator.controller");

        static AssetBundle LoadAssetBundle(Byte[] resourceBytes)
        {
            //Check to make sure that the byte array supplied is not null, and throw an appropriate exception if they are.
            if (resourceBytes == null) throw new ArgumentNullException(nameof(resourceBytes));

            //Actually load the bundle with a Unity function.
            var bundle = AssetBundle.LoadFromMemory(resourceBytes);

            return bundle;
        }


        [Obsolete]
        public void DuplicatorModelChanger()
        {



            if (OldModelDupli.Value == true)
            {
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh = FBXDuplicatorOld.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("PickupDisplay").localPosition = FBXDuplicatorOld.transform.GetChild(0).transform.GetChild(4).localPosition;
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("DropPivot").localPosition = FBXDuplicatorOld.transform.GetChild(0).transform.GetChild(0).localPosition;
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localScale = new Vector3(1.15f, 1.15f, 1.15f);
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localPosition = new Vector3(0f, 0.55f, 1.64f);
                //Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.FindChild("Base").transform.FindChild("Lid").localPosition = new Vector3(1.32885f, 1.801457f, -1.350426f);
                Duplicator.transform.FindChild("mdlDuplicator").GetComponent<Animator>().runtimeAnimatorController = OldAnimControll;
            }

            if (OldModelDupliLarge.Value == true)
            {
                DuplicatorLarge.transform.FindChild("mdlDuplicator").GetComponent<RandomizeSplatBias>().enabled = false;
                DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh = FBXDuplicatorOld2.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("PickupDisplay").localPosition = FBXDuplicatorOld2.transform.GetChild(0).transform.GetChild(4).localPosition;
                DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("DropPivot").localPosition = FBXDuplicatorOld2.transform.GetChild(0).transform.GetChild(0).localPosition;
                DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localScale = new Vector3(1.15f, 1.15f, 1.15f);
                DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localPosition = new Vector3(0f, 0.55f, 1.64f);
                //DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.FindChild("Base").transform.FindChild("Lid").localPosition = new Vector3(1.32885f, 1.801457f, -1.350426f);
                DuplicatorLarge.transform.FindChild("mdlDuplicator").GetComponent<Animator>().runtimeAnimatorController = OldAnimControll;
            }

            if (OldModelDupliMili.Value == true)
            {
                DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh = FBXDuplicatorOldMili.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("PickupDisplay").localPosition = FBXDuplicatorOldMili.transform.GetChild(0).transform.GetChild(4).localPosition;
                DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("DropPivot").localPosition = FBXDuplicatorOldMili.transform.GetChild(0).transform.GetChild(0).localPosition;
                DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localScale = new Vector3(1.15f, 1.15f, 1.15f);
                DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localPosition = new Vector3(0f, 0.55f, 1.64f);
                //DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.GetChild(0).transform.FindChild("Lid").localPosition = new Vector3(1.328f, 1.801f, -1.350f);
                DuplicatorMili.transform.FindChild("mdlDuplicator").GetComponent<Animator>().runtimeAnimatorController = OldAnimControll;
            }

            if (OldModelDupliWild.Value == true)
            {
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh = FBXDuplicatorOldWild.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("PickupDisplay").localPosition = FBXDuplicatorOldWild.transform.GetChild(0).transform.GetChild(7).localPosition;
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("DropPivot").localPosition = FBXDuplicatorOldWild.transform.GetChild(0).transform.GetChild(0).localPosition;
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localScale = new Vector3(1.15f, 1.15f, 1.15f);
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localPosition = new Vector3(0f, 0.55f, 1.64f);
                //DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.GetChild(0).transform.FindChild("Lid").localPosition = new Vector3(1.328f, 1.801f, -1.350f);
                DuplicatorWild.transform.FindChild("mdlDuplicator").GetComponent<Animator>().runtimeAnimatorController = OldAnimControll;
            }
        }







        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }





        public static void GameEndEquipHelper(On.RoR2.UI.GameEndReportPanelController.orig_SetPlayerInfo orig, global::RoR2.UI.GameEndReportPanelController self, global::RoR2.RunReport.PlayerInfo playerInfo)
        {
            orig(self, playerInfo);

            DeathEquip1 = EquipmentIndex.None;
            DeathEquip2 = EquipmentIndex.None;
            DeathEquipEnemy1 = EquipmentIndex.None;
            DeathEquipEnemy2 = EquipmentIndex.None;




            if (Run.instance)
            {



                //CharacterMaster KillerMaster = null;
                Inventory KillerMaster = null;
                string KillerNameBase = "Killed By: <color=#FFFF7F>";
                string KillerName = "";
                //Debug.LogWarning(GameEndingDamageReports.Count);
                //Debug.LogWarning(KillerMaster);

                //Debug.LogWarning(GameEndingDamageReports.Count);

                bool IsWinWithEvo = false;
                bool IsLossToPlanet = false;

                for (int i = 0; i < GameEndingDamageReports.Count; i++)
                {
                    if (playerInfo.networkUser == null)
                    {
                        Debug.LogWarning("No NetworkUser");
                    }
                    if (playerInfo.networkUser != null)
                    {
                        Debug.Log(playerInfo.networkUser.id.value);
                        if (GameEndingDamageReports[i].victimMaster.playerCharacterMasterController.networkUser.id.value == playerInfo.networkUser.id.value)
                        {

                            //KillerMaster = GameEndingDamageReports[i].attackerMaster;
                            //Debug.LogWarning(GameOverBodyNames[i]);

                            KillerName = KillerNameBase + GameOverBodyNames[i] + "</color>";


                            if (GameOverBodyNames[i] != "")
                            {
                                self.killerBodyLabel.SetText(KillerName);
                            }
                            else
                            {
                                IsLossToPlanet = true;
                            }


                            KillerMaster = GameOverInventories[i];

                            //Debug.LogWarning(GameOverInventories[i]);



                            //if (GameEndingDamageReports[i].attacker && GameEndingDamageReports[i].attackerBody)

                            //KillerName = KillerName + RoR2.Util.GetBestBodyName(GameEndingDamageReports[i].attacker) + "</color>";


                        }
                    }

                }


                self.playerBodyLabel.alignment = TMPro.TextAlignmentOptions.Left;

                //Debug.LogWarning(KillerMaster);

                if (self.displayData.runReport.gameEnding.isWin || KillerMaster == null || IsLossToPlanet == true)
                {
                    Debug.Log("Could not find Killer Inventory");
                    GameObject DummyInventoryHolder = new GameObject("DummyInventoryHolder");
                    NetworkServer.Spawn(DummyInventoryHolder);
                    DummyInventoryHolder.AddComponent<Inventory>();
                    if (RunArtifactManager.instance && RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.monsterTeamGainsItemsArtifactDef))
                    {
                        IsWinWithEvo = true;
                        GameObject EvoInventory = GameObject.Find("MonsterTeamGainsItemsArtifactInventory(Clone)");
                        if (EvoInventory)
                        {
                            DummyInventoryHolder.GetComponent<Inventory>().AddItemsFrom(EvoInventory.GetComponent<Inventory>());
                        }

                    }
                    if (Run.instance && Run.instance.name.StartsWith("InfiniteTowerRun(Clone)"))
                    {
                        IsWinWithEvo = true;
                        DummyInventoryHolder.GetComponent<Inventory>().AddItemsFrom(Run.instance.GetComponent<Inventory>());
                    }
                    GameObject ArenaMissionController = GameObject.Find("/ArenaMissionController");
                    if (ArenaMissionController)
                    {
                        IsWinWithEvo = true;
                        DummyInventoryHolder.GetComponent<Inventory>().AddItemsFrom(ArenaMissionController.GetComponent<Inventory>());
                    }

                    if (IsWinWithEvo)
                    {
                        KillerMaster = DummyInventoryHolder.GetComponent<Inventory>();
                    }
                  

                }



                if (KillerMaster != null)
                {
                    /*
                    if (KillerMaster.GetTotalItemCountOfTier(ItemTier.Tier1) > 0 || KillerMaster.GetTotalItemCountOfTier(ItemTier.Tier2) > 0 || KillerMaster.GetTotalItemCountOfTier(ItemTier.Tier3) > 0 || KillerMaster.GetTotalItemCountOfTier(ItemTier.Boss) > 0 || KillerMaster.GetTotalItemCountOfTier(ItemTier.Lunar) > 0)
                    { }*/

                    self.unlockContentArea.parent.parent.parent.gameObject.SetActive(false);

                    int[] writestacks = new int[ItemCatalog.itemCount];
                    KillerMaster.WriteItemStacks(writestacks);

                    GameObject KillerInvDisplayObj = null;
                    if (self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent.childCount == 4)
                    {
                        self.itemInventoryDisplay.enabled = false;
                        KillerInvDisplayObj = Instantiate(self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.gameObject, self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent);
                        self.itemInventoryDisplay.enabled = true;
                    }
                    else
                    {
                        KillerInvDisplayObj = self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent.GetChild(4).gameObject;
                    }

                    if (IsWinWithEvo)
                    {
                        KillerInvDisplayObj.name = "EvolutionArea";
                    }
                    else
                    {
                        KillerInvDisplayObj.name = "ItemArea(Clone)";
                    }


                    RoR2.UI.ItemInventoryDisplay KillerInvDisplay = KillerInvDisplayObj.GetComponentInChildren<RoR2.UI.ItemInventoryDisplay>();
                    KillerInvDisplay.enabled = true;


                    DeathEquipEnemy1 = KillerMaster.currentEquipmentIndex;
                    DeathEquipEnemy2 = KillerMaster.alternateEquipmentIndex;


                    KillerInvDisplay.SetItems(KillerMaster.itemAcquisitionOrder, writestacks);
                    KillerInvDisplay.UpdateDisplay();

                    if (self.unlockContentArea.childCount > 0)
                    {
                        self.unlockContentArea.parent.parent.parent.SetAsLastSibling();
                        self.unlockContentArea.parent.parent.parent.gameObject.SetActive(true);

                    }


                }
                else if (self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent.childCount == 5)
                {
                    GameObject KillerInvDisplayObj = self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent.GetChild(4).gameObject;
                    RoR2.UI.ItemInventoryDisplay KillerInvDisplay = KillerInvDisplayObj.GetComponentInChildren<RoR2.UI.ItemInventoryDisplay>();

                    int[] writestacks = new int[ItemCatalog.itemCount];
                    List<ItemIndex> fakeitemorder = new List<ItemIndex>();
                    KillerInvDisplay.ResetItems();
                    KillerInvDisplay.SetItems(fakeitemorder, writestacks);

                    if (self.unlockContentArea.childCount > 0)
                    {
                        self.unlockContentArea.parent.parent.parent.gameObject.SetActive(true);
                    }
                }


            };





            if (playerInfo.equipment.Length > 0)
            {
                DeathEquip1 = playerInfo.equipment[0];
            }
            if (playerInfo.equipment.Length > 1)
            {
                DeathEquip2 = playerInfo.equipment[1];
            }


            //Debug.LogWarning(DeathEquip1);
            //Debug.LogWarning(DeathEquip2);
        }

        public static void GameEndEquipInv(On.RoR2.UI.ItemInventoryDisplay.orig_AllocateIcons orig, global::RoR2.UI.ItemInventoryDisplay self, int desiredItemCount)
        {

            //Debug.LogWarning("ItemInventoryDisplay : AllocateIcons");
            if (self.name.StartsWith("Content"))
            {
                //Debug.LogWarning(DeathEquip1);
                //Debug.LogWarning(DeathEquip2);
                if (self.transform.parent.parent.parent.name.StartsWith("ItemArea(Clone)"))
                {
                    if (DeathEquipEnemy1 != EquipmentIndex.None && DeathEquipEnemy2 == EquipmentIndex.None)
                    {
                        //Debug.LogWarning("Equip 1");
                        orig(self, desiredItemCount + 1);

                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        EquipmentDef equipdef = EquipmentCatalog.GetEquipmentDef(DeathEquipEnemy1);
                        EquipIcon1.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip = EquipIcon1.GetComponent<RoR2.UI.TooltipProvider>();
                        temptooltip.SetFieldValue("titleToken", equipdef.nameToken);
                        temptooltip.SetFieldValue("bodyToken", equipdef.descriptionToken);
                        temptooltip.overrideTitleText = Language.GetString(equipdef.nameToken);
                        temptooltip.overrideBodyText = Language.GetString(equipdef.descriptionToken);
                        temptooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip.titleColor = ColorCatalog.GetColor(equipdef.colorIndex);
                        if (equipdef.isBoss) { temptooltip.titleColor = FakeYellowEquip; }
                        if (equipdef.isLunar) { temptooltip.titleColor = FakeBlueEquip; }
                        EquipIcon1.name = "EquipmentIcon";
                    }//Have 1 Equipment
                    else if (DeathEquipEnemy1 == EquipmentIndex.None && DeathEquipEnemy2 == EquipmentIndex.None)
                    {
                        orig(self, desiredItemCount);
                    }//Have No Equipment
                    else if (DeathEquipEnemy1 != EquipmentIndex.None && DeathEquipEnemy2 != EquipmentIndex.None && DeathEquipEnemy1 != DeathEquipEnemy2)
                    {
                        //Debug.LogWarning("Equip 1 + Equip 2");
                        orig(self, desiredItemCount + 2);

                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        EquipmentDef equipdef = EquipmentCatalog.GetEquipmentDef(DeathEquipEnemy1);
                        EquipIcon1.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip = EquipIcon1.GetComponent<RoR2.UI.TooltipProvider>();
                        temptooltip.SetFieldValue("titleToken", equipdef.nameToken);
                        temptooltip.SetFieldValue("bodyToken", equipdef.descriptionToken);
                        temptooltip.overrideTitleText = Language.GetString(equipdef.nameToken);
                        temptooltip.overrideBodyText = Language.GetString(equipdef.descriptionToken);
                        temptooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip.titleColor = ColorCatalog.GetColor(equipdef.colorIndex);
                        if (equipdef.isBoss) { temptooltip.titleColor = FakeYellowEquip; }
                        if (equipdef.isLunar) { temptooltip.titleColor = FakeBlueEquip; }
                        EquipIcon1.name = "EquipmentIcon";


                        GameObject EquipIcon2 = self.gameObject.transform.GetChild(desiredItemCount + 1).gameObject;
                        EquipIcon2.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        EquipmentDef equipdef2 = EquipmentCatalog.GetEquipmentDef(DeathEquipEnemy2);
                        EquipIcon2.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef2.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip2 = EquipIcon2.GetComponent<RoR2.UI.TooltipProvider>();
                        temptooltip2.SetFieldValue("titleToken", equipdef2.nameToken);
                        temptooltip2.SetFieldValue("bodyToken", equipdef2.descriptionToken);
                        temptooltip2.overrideTitleText = Language.GetString(equipdef2.nameToken);
                        temptooltip2.overrideBodyText = Language.GetString(equipdef2.descriptionToken);
                        temptooltip2.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip2.titleColor = ColorCatalog.GetColor(equipdef2.colorIndex);
                        if (equipdef2.isBoss) { temptooltip2.titleColor = FakeYellowEquip; }
                        if (equipdef2.isLunar) { temptooltip2.titleColor = FakeBlueEquip; }
                        EquipIcon2.name = "EquipmentIcon";

                    }//Have 2 different Equipment
                    else if (DeathEquipEnemy1 != EquipmentIndex.None && DeathEquipEnemy1 == DeathEquipEnemy2)
                    {
                        orig(self, desiredItemCount + 1);
                        //Debug.LogWarning("Equip 1 = Equip 2");
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;

                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 2);
                        EquipmentDef equipdef = EquipmentCatalog.GetEquipmentDef(DeathEquipEnemy1);
                        EquipIcon1.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip = EquipIcon1.GetComponent<RoR2.UI.TooltipProvider>();

                        temptooltip.SetFieldValue("titleToken", equipdef.nameToken);
                        temptooltip.SetFieldValue("bodyToken", equipdef.descriptionToken);
                        temptooltip.overrideTitleText = Language.GetString(equipdef.nameToken);
                        temptooltip.overrideBodyText = Language.GetString(equipdef.descriptionToken);
                        temptooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip.titleColor = ColorCatalog.GetColor(equipdef.colorIndex);
                        if (equipdef.isBoss) { temptooltip.titleColor = FakeYellowEquip; }
                        if (equipdef.isLunar) { temptooltip.titleColor = FakeBlueEquip; }

                        EquipIcon1.name = "EquipmentIcon";
                    }//Have 2 of the same Equipment
                    else if (DeathEquipEnemy2 != EquipmentIndex.None && DeathEquipEnemy1 == EquipmentIndex.None)
                    {
                        //Debug.LogWarning("Equip 1");
                        orig(self, desiredItemCount + 1);

                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        EquipmentDef equipdef = EquipmentCatalog.GetEquipmentDef(DeathEquipEnemy2);
                        EquipIcon1.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip = EquipIcon1.GetComponent<RoR2.UI.TooltipProvider>();
                        temptooltip.overrideTitleText = Language.GetString(equipdef.nameToken);
                        temptooltip.overrideBodyText = Language.GetString(equipdef.descriptionToken);
                        temptooltip.overrideTitleText = equipdef.nameToken;
                        temptooltip.overrideBodyText = equipdef.descriptionToken;
                        temptooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip.titleColor = ColorCatalog.GetColor(equipdef.colorIndex);
                        if (equipdef.isBoss) { temptooltip.titleColor = FakeYellowEquip; }
                        if (equipdef.isLunar) { temptooltip.titleColor = FakeBlueEquip; }
                        EquipIcon1.name = "EquipmentIcon";

                    }//Have 1 Equipment in Alt slot
                    else
                    {
                        orig(self, desiredItemCount);
                    }
                }
                else if (self.transform.parent.parent.parent.name.StartsWith("EvolutionArea"))
                {
                    orig(self, desiredItemCount);
                }
                else
                {
                    if (DeathEquip1 != EquipmentIndex.None && DeathEquip2 == EquipmentIndex.None)
                    {
                        //Debug.LogWarning("Equip 1");
                        orig(self, desiredItemCount + 1);

                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        EquipmentDef equipdef = EquipmentCatalog.GetEquipmentDef(DeathEquip1);
                        EquipIcon1.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip = EquipIcon1.GetComponent<RoR2.UI.TooltipProvider>();
                        temptooltip.SetFieldValue("titleToken", equipdef.nameToken);
                        temptooltip.SetFieldValue("bodyToken", equipdef.descriptionToken);
                        temptooltip.overrideTitleText = Language.GetString(equipdef.nameToken);
                        temptooltip.overrideBodyText = Language.GetString(equipdef.descriptionToken);
                        temptooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip.titleColor = ColorCatalog.GetColor(equipdef.colorIndex);
                        if (equipdef.isBoss) { temptooltip.titleColor = FakeYellowEquip; }
                        if (equipdef.isLunar) { temptooltip.titleColor = FakeBlueEquip; }
                        EquipIcon1.name = "EquipmentIcon";
                    }//Have 1 Equipment
                    else if (DeathEquip1 == EquipmentIndex.None && DeathEquip2 == EquipmentIndex.None)
                    {
                        orig(self, desiredItemCount);
                    }//Have No Equipment
                    else if (DeathEquip1 != EquipmentIndex.None && DeathEquip2 != EquipmentIndex.None && DeathEquip1 != DeathEquip2)
                    {
                        //Debug.LogWarning("Equip 1 + Equip 2");
                        orig(self, desiredItemCount + 2);

                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        EquipmentDef equipdef = EquipmentCatalog.GetEquipmentDef(DeathEquip1);
                        EquipIcon1.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip = EquipIcon1.GetComponent<RoR2.UI.TooltipProvider>();
                        temptooltip.SetFieldValue("titleToken", equipdef.nameToken);
                        temptooltip.SetFieldValue("bodyToken", equipdef.descriptionToken);
                        temptooltip.overrideTitleText = Language.GetString(equipdef.nameToken);
                        temptooltip.overrideBodyText = Language.GetString(equipdef.descriptionToken);
                        temptooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip.titleColor = ColorCatalog.GetColor(equipdef.colorIndex);
                        if (equipdef.isBoss) { temptooltip.titleColor = FakeYellowEquip; }
                        if (equipdef.isLunar) { temptooltip.titleColor = FakeBlueEquip; }
                        EquipIcon1.name = "EquipmentIcon";


                        GameObject EquipIcon2 = self.gameObject.transform.GetChild(desiredItemCount + 1).gameObject;
                        EquipIcon2.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        EquipmentDef equipdef2 = EquipmentCatalog.GetEquipmentDef(DeathEquip2);
                        EquipIcon2.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef2.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip2 = EquipIcon2.GetComponent<RoR2.UI.TooltipProvider>();
                        temptooltip2.SetFieldValue("titleToken", equipdef2.nameToken);
                        temptooltip2.SetFieldValue("bodyToken", equipdef2.descriptionToken);
                        temptooltip2.overrideTitleText = Language.GetString(equipdef2.nameToken);
                        temptooltip2.overrideBodyText = Language.GetString(equipdef2.descriptionToken);
                        temptooltip2.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip2.titleColor = ColorCatalog.GetColor(equipdef2.colorIndex);
                        if (equipdef2.isBoss) { temptooltip2.titleColor = FakeYellowEquip; }
                        if (equipdef2.isLunar) { temptooltip2.titleColor = FakeBlueEquip; }
                        EquipIcon2.name = "EquipmentIcon";

                    }//Have 2 different Equipment
                    else if (DeathEquip1 != EquipmentIndex.None && DeathEquip1 == DeathEquip2)
                    {
                        orig(self, desiredItemCount + 1);
                        //Debug.LogWarning("Equip 1 = Equip 2");
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;

                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 2);
                        EquipmentDef equipdef = EquipmentCatalog.GetEquipmentDef(DeathEquip1);
                        EquipIcon1.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip = EquipIcon1.GetComponent<RoR2.UI.TooltipProvider>();

                        temptooltip.SetFieldValue("titleToken", equipdef.nameToken);
                        temptooltip.SetFieldValue("bodyToken", equipdef.descriptionToken);
                        temptooltip.overrideTitleText = Language.GetString(equipdef.nameToken);
                        temptooltip.overrideBodyText = Language.GetString(equipdef.descriptionToken);
                        temptooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip.titleColor = ColorCatalog.GetColor(equipdef.colorIndex);
                        if (equipdef.isBoss) { temptooltip.titleColor = FakeYellowEquip; }
                        if (equipdef.isLunar) { temptooltip.titleColor = FakeBlueEquip; }

                        EquipIcon1.name = "EquipmentIcon";
                    }//Have 2 of the same Equipment
                    else if (DeathEquip2 != EquipmentIndex.None && DeathEquip1 == EquipmentIndex.None)
                    {
                        //Debug.LogWarning("Equip 1");
                        orig(self, desiredItemCount + 1);

                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        EquipmentDef equipdef = EquipmentCatalog.GetEquipmentDef(DeathEquip2);
                        EquipIcon1.GetComponent<UnityEngine.UI.RawImage>().texture = equipdef.pickupIconTexture;
                        RoR2.UI.TooltipProvider temptooltip = EquipIcon1.GetComponent<RoR2.UI.TooltipProvider>();
                        temptooltip.overrideTitleText = Language.GetString(equipdef.nameToken);
                        temptooltip.overrideBodyText = Language.GetString(equipdef.descriptionToken);
                        temptooltip.overrideTitleText = equipdef.nameToken;
                        temptooltip.overrideBodyText = equipdef.descriptionToken;
                        temptooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
                        temptooltip.titleColor = ColorCatalog.GetColor(equipdef.colorIndex);
                        if (equipdef.isBoss) { temptooltip.titleColor = FakeYellowEquip; }
                        if (equipdef.isLunar) { temptooltip.titleColor = FakeBlueEquip; }
                        EquipIcon1.name = "EquipmentIcon";

                    }//Have 1 Equipment in Alt slot

                }




                var tempHeader = self.gameObject.transform.parent.parent.parent.GetChild(0).GetChild(0).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>();
                if (tempHeader)
                {
                    if (self.transform.parent.parent.parent.name.StartsWith("ItemArea(Clone)"))
                    {
                        tempHeader.GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText("Killers Items");
                    }
                    else if (self.transform.parent.parent.parent.name.StartsWith("EvolutionArea"))
                    {
                        tempHeader.GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText("Monsters Items");
                    }
                    else
                    {
                        tempHeader.GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText("Items & Equipment Collected");
                    }
                }
                else
                {
                    tempHeader = self.gameObject.transform.parent.parent.GetChild(0).GetChild(0).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>();
                    if (tempHeader)
                    {
                        tempHeader.GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText("Items & Equipment Collected");
                    }
                }

            }
            else
            {
                orig(self, desiredItemCount);
            }

        }


        public void EquipmentHighlighter(On.RoR2.CharacterModel.orig_SetEquipmentDisplay orig, global::RoR2.CharacterModel self, EquipmentIndex newEquipmentIndex)
        {
            //Debug.LogWarning("Read1");
            orig(self, newEquipmentIndex);
            //Debug.LogWarning("Read2");
            //Debug.Log("Equipment Highlighter");
            if (newEquipmentIndex != EquipmentIndex.None)
            {
                GameObject Highlight = HighlightOrangeItem;
                if (EquipmentCatalog.GetEquipmentDef(newEquipmentIndex).isLunar == true)
                {
                    Highlight = HighlightOrangeLunarItem;
                }
                else if (EquipmentCatalog.GetEquipmentDef(newEquipmentIndex).isBoss == true)
                {
                    Highlight = HighlightOrangeBossItem;

                }

                List<GameObject> tempList = self.GetEquipmentDisplayObjects(newEquipmentIndex);
                for (int i = 0; i < tempList.Count; i++)
                {
                    Renderer renderer = tempList[i].GetComponentInChildren<Renderer>();
                    if (renderer)
                    {
                        RoR2.UI.HighlightRect.CreateHighlight(self.body.gameObject, renderer, Highlight, -1, false);
                    }
                    else
                    {
                        renderer = tempList[i].GetComponent<Renderer>();
                        if (renderer)
                        {
                            RoR2.UI.HighlightRect.CreateHighlight(self.body.gameObject, renderer, Highlight, -1, false);
                        }
                    }

                };
            }

            On.RoR2.CharacterModel.SetEquipmentDisplay -= EquipmentHighlighter;
        }




        public static void MULTEquipmentThing(On.EntityStates.Toolbot.ToolbotStanceA.orig_OnEnter orig, global::EntityStates.Toolbot.ToolbotStanceA self)
        {
            if (NetworkServer.active)
            {
                Inventory tempinv = self.outer.gameObject.GetComponent<CharacterBody>().inventory;
                //Debug.LogWarning(tempinv.GetEquipmentSlotCount());
                if (tempinv.GetEquipmentSlotCount() < 2)
                {
                    SkillLocator tempSkill = self.outer.gameObject.GetComponent<SkillLocator>();
                    if (tempSkill && tempSkill.special.skillDef == tempSkill.special.defaultSkillDef)
                    {
                        if (tempinv)
                        {
                            /*
                            tempinv.SetActiveEquipmentSlot(1);
                            tempinv.SetEquipmentIndex(0);
                            tempinv.SetEquipmentIndex(EquipmentIndex.None);
                            tempinv.DeductEquipmentCharges(1, 1);
                            tempinv.SetActiveEquipmentSlot(0);
                            */

                            if (tempinv.currentEquipmentIndex == EquipmentIndex.None)
                            {
                                tempinv.SetEquipment(new EquipmentState(EquipmentIndex.None, Run.FixedTimeStamp.negativeInfinity, 0), 0);
                            }
                            if (tempinv.alternateEquipmentIndex == EquipmentIndex.None)
                            {
                                tempinv.SetEquipment(new EquipmentState(EquipmentIndex.None, Run.FixedTimeStamp.negativeInfinity, 0), 1);
                            }



                            Debug.Log("MulT equipment thing");
                        }
                    }
                }
            }
            orig(self);
        }




        internal static void ModSupport()
        {

            LateRunningMethod();

            //RoR2.Skills.SkillCatalog

            EquipmentBossOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color32(232, 185, 70, 0);
            EquipmentBossOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color32(166, 112, 38, 255);
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].SetFieldValue<Color>("color", new Color32(234, 153, 6, 255)); //Core Start
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(234, 136, 6, 255); //Core End
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color32(255, 189, 0, 255); //PulseGlow Start
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(255, 189, 0, 255); //PulseGlow End
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].SetFieldValue<Color>("color", new Color32(234, 153, 6, 255)); //Core Start
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(234, 136, 6, 255); //Core End
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color32(255, 189, 0, 255); //PulseGlow Start
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(255, 189, 0, 255); //PulseGlow End

            EquipmentBossOrb.transform.GetChild(0).GetChild(2).GetComponent<Light>().color = new Color32(255, 192, 74, 255);

            EquipmentLunarOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color32(43, 46, 232, 0);
            EquipmentLunarOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color32(26, 29, 225, 255);

            GradientColorKey LunarC1 = new GradientColorKey
            {
                color = new Color32(91, 101, 226, 255),
                time = 0
            };
            GradientColorKey LunarC2 = new GradientColorKey
            {
                color = new Color32(91, 101, 226, 255),
                time = 1
            };
            GradientColorKey[] sex = new GradientColorKey[1];
            sex = sex.Add(LunarC1, LunarC2);

            //EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.SetKeys(sex, EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.alphaKeys);

            //EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys = sex;
            //EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys = sex;


            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color32(91, 101, 226, 255); //Core
            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(91, 101, 226, 255); //Core
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color32(43, 110, 225, 255); //PulseGlow
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(43, 110, 225, 255); //PulseGlow
            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color32(91, 101, 226, 255); //Core
            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(91, 101, 226, 255); //Core
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color32(43, 110, 225, 255); //PulseGlow
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(43, 110, 225, 255); //PulseGlow

            Instantiate(VEquipmentOrb.transform.GetChild(0).GetChild(2).gameObject, EquipmentBossOrb.transform.GetChild(0));
            Instantiate(VEquipmentOrb.transform.GetChild(0).GetChild(2).gameObject, EquipmentLunarOrb.transform.GetChild(0));




            EquipmentLunarOrb.transform.GetChild(0).GetChild(1).GetComponent<Light>().color = new Color32(114, 112, 232, 255);

            NoTierOrb.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            NoTierOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color(0f, 0f, 0f, 0);
            NoTierOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color(0.5f, 0.5f, 0.5f, 1);
            NoTierOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color(0.5f, 0.5f, 0.5f, 1); //Core Start
            NoTierOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color(0.3f, 0.3f, 0.3f, 1); //Core End
            NoTierOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color(0.5f, 0.5f, 0.5f, 1); //Core Start
            NoTierOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color(0.3f, 0.3f, 0.3f, 1); //Core End

            NoTierOrb.transform.GetChild(0).GetChild(1).GetComponent<Light>().color = new Color(0.5f, 0.5f, 0.5f, 1);

            CoinOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color32(198, 173, 250, 0);
            CoinOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color32(168, 147, 212, 255);
            CoinOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color32(198, 173, 250, 255); //Core Start
            CoinOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(198, 173, 250, 255); //Core End
            CoinOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color32(198, 173, 250, 255); //Core Start
            CoinOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(198, 173, 250, 255); //Core End

            CoinOrb.transform.GetChild(0).GetChild(1).GetComponent<Light>().color = new Color32(210, 178, 255, 255);




            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse1).SetFieldValue<Color>("color", new Color(0.4f, 0.4f, 0.5f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse2).SetFieldValue<Color>("color", new Color(0.371f, 0.371f, 0.471f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse3).SetFieldValue<Color>("color", new Color(0.342f, 0.342f, 0.442f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse4).SetFieldValue<Color>("color", new Color(0.314f, 0.314f, 0.414f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse5).SetFieldValue<Color>("color", new Color(0.285f, 0.285f, 0.385f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse6).SetFieldValue<Color>("color", new Color(0.257f, 0.257f, 0.357f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse7).SetFieldValue<Color>("color", new Color(0.228f, 0.228f, 0.328f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse8).SetFieldValue<Color>("color", new Color(0.2f, 0.2f, 0.3f, 1));

            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse1").tooltipNameColor = new Color(0.4f, 0.4f, 0.5f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse2").tooltipNameColor = new Color(0.371f, 0.371f, 0.471f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse3").tooltipNameColor = new Color(0.342f, 0.342f, 0.442f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse4").tooltipNameColor = new Color(0.314f, 0.314f, 0.414f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse5").tooltipNameColor = new Color(0.285f, 0.285f, 0.385f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse6").tooltipNameColor = new Color(0.257f, 0.257f, 0.357f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse7").tooltipNameColor = new Color(0.228f, 0.228f, 0.328f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse8").tooltipNameColor = new Color(0.2f, 0.2f, 0.3f, 1);

            /*
            uint tempcash = StartingCash.Value;
            RuleCatalog.FindRuleDef("Misc.StartingMoney").FindChoice("15").extraData = tempcash;
            */




            /*


            CharacterSpawnCard[] CSCList = FindObjectsOfType(typeof(CharacterSpawnCard)) as CharacterSpawnCard[];
            for (var i = 0; i < CSCList.Length; i++)
            {
                //Debug.LogWarning(CSCList[i]);
                switch (CSCList[i].name)
                {
                    case "cscArchWisp":
                        DirectorCard DC_ArchWisp = new DirectorCard
                        {
                            spawnCard = CSCList[i],
                            selectionWeight = 2,
                            preventOverhead = true,
                            minimumStageCompletions = 0,
                            spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
                        };
                        //RoR2Content.mixEnemyMonsterCards.AddCard(1, DC_ArchWisp);  //
                        break;
                    case "cscClayMan":
                        DirectorCard DC_ClayMan = new DirectorCard
                        {
                            spawnCard = CSCList[i],
                            selectionWeight = 1,
                            preventOverhead = false,
                            minimumStageCompletions = 0,
                            spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
                        };
                        MoffeinClayMan = CSCList[i].prefab;
                        //RoR2Content.mixEnemyMonsterCards.AddCard(2, DC_ClayMan);  //30
                        //ClayFamilyEvent.monsterFamilyCategories.AddCategory("Basic Monsters", 6);
                        //ClayFamilyEvent.monsterFamilyCategories.AddCard(2, DC_ClayMan);
                        //ClayFamilyEvent.minimumStageCompletion = 1;
                        break;
                    case "cscAncientWisp":
                        DirectorCard DC_AncientWisp = new DirectorCard
                        {
                            spawnCard = CSCList[i],
                            selectionWeight = 2,
                            preventOverhead = true,
                            minimumStageCompletions = 0,
                            spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
                        };
                        //RoR2Content.mixEnemyMonsterCards.AddCard(0, DC_AncientWisp);  //30
                        break;


                }
            }



            */







        }


        public static void AffixLunarItemDisplay()
        {

            DisplayRuleGroup originallunargolemrule = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarGolemBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet.keyAssetRuleGroups[0].displayRuleGroup;

            GameObject MithrixCrystalRed = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet.keyAssetRuleGroups[0].displayRuleGroup.rules[0].followerPrefab;


            EquipmentDef EliteSecretSpeedEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteSecretSpeedEquipment.asset").WaitForCompletion();
            GameObject DisplayEliteRabbitEars = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DisplayEliteRabbitEars.prefab").WaitForCompletion();




            MithrixCrystalOrange = R2API.PrefabAPI.InstantiateClone(MithrixCrystalRed, "ItemInfection, Orange", false);
            MithrixCrystalYellow = R2API.PrefabAPI.InstantiateClone(MithrixCrystalRed, "ItemInfection, Yellow", false);
            MithrixCrystalPink = R2API.PrefabAPI.InstantiateClone(MithrixCrystalRed, "ItemInfection, Pink", false);
            MithrixCrystalPinkSmall = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/ItemInfection, White.prefab").WaitForCompletion(), "ItemInfection, PinkSingle", false);

            MithrixCrystalOrange.GetComponent<MeshRenderer>().material = Instantiate<Material>(MithrixCrystalOrange.GetComponent<MeshRenderer>().material);
            MithrixCrystalOrange.GetComponent<MeshRenderer>().material.SetColor("_EmColor", new Color(1.4f, 0.7f, 0f, 1f));
            MithrixCrystalYellow.GetComponent<MeshRenderer>().material = Instantiate<Material>(MithrixCrystalYellow.GetComponent<MeshRenderer>().material);
            MithrixCrystalYellow.GetComponent<MeshRenderer>().material.SetColor("_EmColor", new Color(1f, 1f, 0f, 1f));
            MithrixCrystalOrange.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = MithrixCrystalOrange.GetComponent<MeshRenderer>().material;
            MithrixCrystalYellow.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = MithrixCrystalYellow.GetComponent<MeshRenderer>().material;

            Material PinkCrystal = Instantiate<Material>(MithrixCrystalPink.GetComponent<MeshRenderer>().material);
            PinkCrystal.SetColor("_EmColor", new Color(1f, 0f, 0.5f, 1f));
            PinkCrystal.name = "matBrotherInfectionPink";


            MithrixCrystalPink.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = PinkCrystal;
            MithrixCrystalPinkSmall.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = PinkCrystal;
            MithrixCrystalPink.GetComponent<MeshRenderer>().material = PinkCrystal;
            MithrixCrystalPinkSmall.GetComponent<MeshRenderer>().material = PinkCrystal;






            //newLunarDisplay.displayRuleGroup.rules[0].childName = tempItemDisplayRules.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixWhite.equipmentIndex).rules[0].childName;

            var tempMandoRule = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;

            var DisplayAffixBluePrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixBlue.equipmentIndex).rules[0].followerPrefab;
            var DisplayAffixHaubtedPrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixHaunted.equipmentIndex).rules[0].followerPrefab;
            var DisplayAffixPoisonPrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixPoison.equipmentIndex).rules[0].followerPrefab;
            var DisplayAffixRedPrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixRed.equipmentIndex).rules[0].followerPrefab;
            var DisplayAffixWhitePrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixWhite.equipmentIndex).rules[0].followerPrefab;
            var DisplayVolatileBatteryPrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.QuestVolatileBattery.equipmentIndex).rules[0].followerPrefab;


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixCommandoDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.2787723f, 0.2163888f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.275f, 0.275f, 0.275f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixCommandoDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.35f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f,1f,1f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            var tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixCommandoDisplay, SecretSpeedAffixCommandoDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixHuntressDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, -0.05f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.7f,0.7f,0.7f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HuntressBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixCommandoDisplay, SecretSpeedAffixHuntressDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixBandit = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Bandit2Body").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBandit);
            tempItemDisplayRules.GenerateRuntimeValues();
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixToolBot = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 3.75f, -1.15f),
                            localAngles = new Vector3(60f,0,0f),
                            localScale = new Vector3(3f, 3f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ToolbotBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixToolBot);
            tempItemDisplayRules.GenerateRuntimeValues();




            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixMercDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.12f, 0.2164f),
                            localAngles = originallunargolemrule.rules[0].localAngles,
                            localScale = new Vector3(0.275f, 0.275f, 0.275f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixMercDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();

            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MageBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBandit);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixREXDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "FlowerBase",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = new Vector3(90f,0,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TreebotBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixREXDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixMercDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixCrocoDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0,0f),
                            localScale = new Vector3(4f, 4f, 4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixCrocoDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, -0.5f, 0.9f),
                            localAngles = new Vector3(300f,0,180f),
                            localScale = new Vector3(10f,10f,10f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixCrocoDisplay, SecretSpeedAffixCrocoDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixEngi = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Chest",
                            localPos = new Vector3(0f, 0.6f, 0.3f),
                            localAngles = new Vector3(0f,0,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixEngi);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixCaptainDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(335f,0f,0f),
                            localScale = new Vector3(0.9f,0.9f,0.9f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CaptainBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixMercDisplay, SecretSpeedAffixCaptainDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBeetle = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.575f, -0.5f),
                            localAngles = new Vector3(40f, 0, 0f),
                            localScale = new Vector3(0.8f, 0.8f, 0.8f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBeetle);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBeetleGuard = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 2f, 0.6f),
                            localAngles = new Vector3(50f, 170, 170f),
                            localScale = new Vector3(3f, 3f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleGuardBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBeetleGuard);
            tempItemDisplayRules.GenerateRuntimeValues();



            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBeetleQueen = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, -2.3f),
                            localAngles = new Vector3(350f, 0f, 0f),
                            localScale = new Vector3(2.3f, 2.3f, 2.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleQueen2Body").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBeetleQueen);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBell = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Chain",
                            localPos = new Vector3(0f, -1f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BellBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBell);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBison = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.65f, 0f),
                            localAngles = new Vector3(90f, 0, 0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BisonBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBison);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayClayBoss = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "PotLidTop",
                            localPos = new Vector3(0f, 1f, 1f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.25f, 2.25f, 2.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBossBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayClayBoss);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayClayTemplar = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.45f, 0.1f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.6f, 0.6f, 0.6f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Muzzle",
                            localPos = new Vector3(0f, -0.05f, -0.05f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.6f, 0.6f, 0.6f),
                            limbMask = LimbFlags.None
                        },

}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayClayTemplar);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayWorms = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 3f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.75f, 2.75f, 2.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayWorms = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.4f, -2.1f),
                            localAngles = new Vector3(0f,180f,0f),
                            localScale = new Vector3(0.35f, 0.35f, 0.35f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ElectricWormBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayWorms, HauntedAffixDisplayWorms);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.575f, 0.55f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GolemBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGolem);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGrandparent = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 8f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(5f, 5f, 5f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GrandParentBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGrandparent);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGravekeeper = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, 3.75f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(3.3f, 3.3f, 3.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GravekeeperBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGravekeeper);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGreaterWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MaskBase",
                            localPos = new Vector3(0f, 0.25f, 0.8f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GreaterWispBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGreaterWisp);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayCrab = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Base",
                            localPos = new Vector3(0f, 1.5f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HermitCrabBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayCrab);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayImp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Neck",
                            localPos = new Vector3(0f, -0.25f, -0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.4f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ImpBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayImp);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayImpBoss = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Chest",
                            localPos = new Vector3(0f, 0.6f, 1.85f),
                            localAngles = new Vector3(10f,180f,0f),
                            localScale = new Vector3(1.6f, 1.6f, 1.6f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ImpBossBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayImpBoss);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayJellyfish = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Hull2",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.75f, 1.75f, 1.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/JellyfishBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayJellyfish);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayLemurian = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(3f, 3f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LemurianBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayLemurian);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayElderLemurian = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, -0.5f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(4f, 4f, 4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LemurianBruiserBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayElderLemurian);
            tempItemDisplayRules.GenerateRuntimeValues();
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MagmaWormBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayWorms, HauntedAffixDisplayWorms);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayMushroom = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.25f, 0f, 0f),
                            localAngles = new Vector3(0f,90f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MiniMushroomBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayMushroom);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidReaver = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Muzzle",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/NullifierBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidReaver);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayParent = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(60f, 100f, 0f),
                            localAngles = new Vector3(315f,90f,0f),
                            localScale = new Vector3(70f, 120f, 70f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ParentBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayParent);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayRoboBallBoss = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Center",
                            localPos = new Vector3(0f, 0f, 1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallBossBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayRoboBallBoss);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayRoboBallMini = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Muzzle",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 0.75f, 0.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallMiniBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayRoboBallMini);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayScav = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 4f, -6f),
                            localAngles = new Vector3(65f,0f,0f),
                            localScale = new Vector3(7f, 7f, 7f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixDisplayScav = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Chest",
                            localPos = new Vector3(0f, 5.25f, 1.5f),
                            localAngles = new Vector3(335f,180f,0f),
                            localScale = new Vector3(40f, 30f, 30f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayScav, SecretSpeedAffixDisplayScav);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayTitan = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 2f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(3f, 3f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TitanBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayTitan);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVagrant = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Hull",
                            localPos = new Vector3(0f, 0.8f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.2f, 2.2f, 2.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/VagrantBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVagrant);
            tempItemDisplayRules.GenerateRuntimeValues();
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/VultureBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayElderLemurian);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.75f, 0.75f, 0.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/WispBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayWisp);
            tempItemDisplayRules.GenerateRuntimeValues();





            //DLC Mobs and Characters
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayRailgunner = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.06f, 0.15f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Railgunner/RailgunnerBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayRailgunner);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidFiend = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.325f, 0.325f, 0.325f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidFiend);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayAcidLarva = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "BeakUpper",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "BodyBase",
                            localPos = new Vector3(0f, 4f, -3.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(4f, 4f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/AcidLarva/AcidLarvaBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayAcidLarva);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayFlyingVerminBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Body",
                            localPos = new Vector3(0f, 0f, 1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.1f, 1.1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FlyingVermin/FlyingVerminBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayFlyingVerminBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVerminBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Vermin/VerminBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVerminBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayMinorConstructBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "CapTop",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/MajorAndMinorConstruct/MinorConstructBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayMinorConstructBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGipBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MainBody2",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GipBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGipBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGeepBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MainBody2",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GeepBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGeepBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGupBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MainBody2",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(1.5f, 1.5f, 1.1f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GupBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGupBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayClayGrenadierBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Torso",
                            localPos = new Vector3(0f, 0.1f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.4f, 0.4f, 0.4f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/ClayGrenadier/ClayGrenadierBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayClayGrenadierBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidJailerBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.2f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
                                                new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "ClawMuzzle",
                            localPos = new Vector3(0f, -0.125f, -1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.25f, 1.25f, 1.25f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidJailer/VoidJailerBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidJailerBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidBarnacleBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(0f,90f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        }
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidBarnacle/VoidBarnacleBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidBarnacleBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidMegaCrabBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MuzzleBlackCannon",
                            localPos = new Vector3(0f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
                                                new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MuzzleWhiteCannon",
                            localPos = new Vector3(0f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidMegaCrab/VoidMegaCrabBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidMegaCrabBody);
            tempItemDisplayRules.GenerateRuntimeValues();


            if (MoffeinClayMan != null)
            {
                tempItemDisplayRules = MoffeinClayMan.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
                tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBandit);
                tempItemDisplayRules.GenerateRuntimeValues();

                Texture2D texBodyClayMan = new Texture2D(128, 128, TextureFormat.DXT5, false);
                texBodyClayMan.LoadImage(Properties.Resources.texBodyClayMan, false);
                texBodyClayMan.filterMode = FilterMode.Bilinear;
                texBodyClayMan.wrapMode = TextureWrapMode.Clamp;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBody").GetComponent<CharacterBody>().portraitIcon = texBodyClayMan;
                MoffeinClayMan.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>().portraitIcon = texBodyClayMan;






                MoffeinClayMan.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponent<DeathRewards>().logUnlockableDef = RoR2.LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Logs.ClayBody.0");


                //UnlockableDef dummyunlock = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<DeathRewards>().logUnlockableDef;

                // MoffeinClayMan.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponent<DeathRewards>().logUnlockableDef = dummyunlock;
            }





            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixBrotherDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0.25f),
                            localAngles = originallunargolemrule.rules[0].localAngles,
                            localScale = new Vector3(0.35f, 0.35f, 0.35f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.15f, 0.15f, 0.05f),
                            localAngles = new Vector3(0f,350f,0f),
                            localScale = new Vector3(0.15f, 0.15f, 0.15f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.15f, 0.15f, 0.05f),
                            localAngles = new Vector3(0f,10f,0f),
                            localScale = new Vector3(-0.15f, 0.15f, 0.15f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, 0f),
                            localAngles = new Vector3(90f,180f,0f),
                            localScale = new Vector3(0.06f, 0.06f, 0.06f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.035f, 0.035f, 0.035f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.15f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.075f, 0.075f, 0.075f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.15f, 0.15f),
                            localAngles = new Vector3(320f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.05f, 0.2f),
                            localAngles = new Vector3(330f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBrotherDisplay, RedAffixDisplayBrother, BlueAffixDisplayBrother, PoisonAffixDisplayBrother, HauntedAffixDisplayBrother, WhiteAffixDisplayBrother);
            tempItemDisplayRules.GenerateRuntimeValues();
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherGlassBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBrotherDisplay, RedAffixDisplayBrother, BlueAffixDisplayBrother, PoisonAffixDisplayBrother, HauntedAffixDisplayBrother, WhiteAffixDisplayBrother);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayBrotherHurt = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.05f, 0.05f, 0.05f),
                            localAngles = new Vector3(0f,350f,0f),
                            localScale = new Vector3(0.12f, 0.12f, 0.12f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.05f, 0.05f, 0.05f),
                            localAngles = new Vector3(0f,10f,0f),
                            localScale = new Vector3(-0.12f, 0.12f, 0.12f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixBrotherHurtDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0.25f),
                            localAngles = originallunargolemrule.rules[0].localAngles,
                            localScale = new Vector3(0.3f, 0.3f, 0.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            //tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBrotherHurtDisplay, RedAffixDisplayBrotherHurt, BlueAffixDisplayBrother, PoisonAffixDisplayBrother, HauntedAffixDisplayBrother, WhiteAffixDisplayBrother);
            tempItemDisplayRules.GenerateRuntimeValues();




            //Engi Turret garbage
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.65f, 1.6f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.25f, 1.25f, 1.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.8f, 0.8f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.35f, 1.35f, 1.5f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.3f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.8f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.4f, -0.45f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.7f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0, -0.25f, 0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 1.5f, 1.25f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0, 0f, 0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };




            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.65f, 0.5f, -0.1f),
                            localAngles = new Vector3(60f,0f,0f),
                            localScale = new Vector3(-0.4f, 0.35f, 0.35f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.65f, 0.5f, -0.1f),
                            localAngles = new Vector3(60f,0f,0f),
                            localScale = new Vector3(0.4f, 0.35f, 0.35f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.4f, 1f, -0.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(-0.45f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.4f, 1f, -0.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(0.45f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BatteryDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.QuestVolatileBattery,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayVolatileBatteryPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.65f, -0.5f),
                            localAngles = new Vector3(270f,180f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BatteryDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.QuestVolatileBattery,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayVolatileBatteryPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 0),
                            localAngles = new Vector3(270f,180f,0f),
                            localScale = new Vector3(1.2f, 1.2f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayEngiTurret, BlueAffixDisplayEngiTurretWalker, PoisonAffixDisplayEngiTurret, HauntedAffixDisplayEngiTurret, WhiteAffixDisplayEngiTurret, LunarAffixDisplayEngiTurret, BatteryDisplayEngiTurret);
            tempItemDisplayRules.GenerateRuntimeValues();

            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayEngiTurretWalker, BlueAffixDisplayEngiTurretWalker, PoisonAffixDisplayEngiTurretWalker, HauntedAffixDisplayEngiTurretWalker, WhiteAffixDisplayEngiTurretWalker, LunarAffixDisplayEngiTurretWalker, BatteryDisplayEngiTurretWalker);
            tempItemDisplayRules.GenerateRuntimeValues();





            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0.5f, 0.5f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.45f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(-0.5f, 0.5f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(-0.45f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 0.8f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 1f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 0.7f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0, 0f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1.25f, 1.25f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0, 0.4f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayLunarExploder, BlueAffixDisplayLunarExploder, PoisonAffixDisplayLunarExploder, HauntedAffixDisplayLunarExploder, WhiteAffixDisplayLunarExploder);
            tempItemDisplayRules.GenerateRuntimeValues();






            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "MuzzleRT",
                            localPos = new Vector3(0.1f, -0.1f, -0.7f),
                            localAngles = new Vector3(320f,335f,180f),
                            localScale = new Vector3(0.5f, 0.45f, 0.45f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "MuzzleLT",
                            localPos = new Vector3(-0.1f, -0.1f, -0.7f),
                            localAngles = new Vector3(320f,25f,180f),
                            localScale = new Vector3(-0.5f, 0.45f, 0.45f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.15f, 0.15f, 0.15f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.4f, 0.5f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0, 0f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1.25f, 1.25f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0, 0.4f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarGolemBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayLunarGolem, BlueAffixDisplayLunarGolem, PoisonAffixDisplayLunarGolem, HauntedAffixDisplayLunarGolem, WhiteAffixDisplayLunarGolem);
            tempItemDisplayRules.GenerateRuntimeValues();



            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Mask",
                            localPos = new Vector3(0.5f, -1f, 2f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.2f, 1.2f, 1.2f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Mask",
                            localPos = new Vector3(-0.5f, -1f, 2f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(-1.2f, 1.2f, 1.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.5f, 0.5f, 0.5f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 3.5f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 2.5f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.45f, 0.45f, 0.45f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Mask",
                            localPos = new Vector3(0, -3f, 1.1f),
                            localAngles = new Vector3(50f,0f,0f),
                            localScale = new Vector3(1.6f, 1.6f, 1.6f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Mask",
                            localPos = new Vector3(0, -2.5f, 1.8f),
                            localAngles = new Vector3(50f,0f,0f),
                            localScale = new Vector3(1.4f, 1.2f, 1.2f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarWispBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayLunarWisp, BlueAffixDisplayLunarWisp, PoisonAffixDisplayLunarWisp, HauntedAffixDisplayLunarWisp, WhiteAffixDisplayLunarWisp);
            tempItemDisplayRules.GenerateRuntimeValues();





















            /*
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayNewt = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "head", //needs to be changed
                            localPos = new Vector3(0f, 0.2f, 0.2f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayNewt);
            tempItemDisplayRules.GenerateRuntimeValues();
            */





        }



 
        public static RoR2.UI.LogBook.Entry[] ChangeEquipmentBGLogbook(On.RoR2.UI.LogBook.LogBookController.orig_BuildPickupEntries orig, Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            RoR2.UI.LogBook.Entry[] array = orig(expansionAvailability);


            bool flag = false;
            int num = -1;
            for (int i = 0; i < array.Length; i++)
            {
                bool flag2 = false;
                PickupDef pickupDef = ((PickupIndex)array[i].extraData).pickupDef;
                ItemIndex itemIndex = pickupDef.itemIndex;
                bool flag3 = itemIndex != ItemIndex.None;
                if (flag3)
                {
                    ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);
                    bool flag4 = itemDef && itemDef.tier == ItemTier.Boss;
                    if (flag4)
                    {
                        flag2 = true;
                    }
                }
                bool flag5 = !flag && flag2;
                if (flag5)
                {
                    //base.Logger.LogInfo("Found boss item start");
                    flag = true;
                }
                bool flag6 = flag && !flag2 && num == -1;
                if (flag6)
                {
                    //base.Logger.LogInfo("Found boss item end");
                    num = i;
                }
            }
            bool flag7 = false;
            bool flag8 = num == -1;
            if (flag8)
            {
                //base.Logger.LogError("Did not find boss items: Placing equipment at logbook end");
                flag7 = true;
            }
            List<RoR2.UI.LogBook.Entry> list = new List<RoR2.UI.LogBook.Entry>();
            for (int j = 0; j < array.Length; j++)
            {
                bool flag9 = !list.Contains(array[j]);
                if (flag9)
                {
                    PickupDef pickupDef2 = ((PickupIndex)array[j].extraData).pickupDef;
                    EquipmentIndex equipmentIndex = pickupDef2.equipmentIndex;
                    bool flag10 = equipmentIndex != EquipmentIndex.None;
                    if (flag10)
                    {
                        EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
                        bool flag11 = equipmentDef && equipmentDef.isBoss;
                        if (flag11)
                        {
                            //base.Logger.LogInfo("Found Boss Equipment : " + equipmentDef.name);
                            RoR2.UI.LogBook.Entry entry = array[j];
                            list.Add(array[j]);
                            //entry.bgTexture = Resources.Load<Texture>("Textures/ItemIcons/BG/texBossBGIcon");
                            HG.ArrayUtils.ArrayRemoveAtAndResize<RoR2.UI.LogBook.Entry>(ref array, j, 1);
                            bool flag12 = flag7;
                            if (flag12)
                            {
                                num = array.Length;
                            }
                            HG.ArrayUtils.ArrayInsert<RoR2.UI.LogBook.Entry>(ref array, num, entry);
                            num++;
                        }
                    }
                }
            }








            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].nameToken.StartsWith("EQUIPMENT_"))
                {
                    PickupIndex tempind = PickupCatalog.FindPickupIndex(array[i].extraData.ToString());
                    PickupDef temppickdef = PickupCatalog.GetPickupDef(tempind);
                    EquipmentDef tempeqdef = EquipmentCatalog.GetEquipmentDef(temppickdef.equipmentIndex);

                    if (tempeqdef.isBoss == true)
                    {
                        array[i].bgTexture = texEquipmentBossBG;
                        array[i].color = FakeYellowEquip;
                    }
                    else if (tempeqdef.isLunar == true)
                    {
                        array[i].bgTexture = texEquipmentLunarBG;
                        array[i].color = FakeBlueEquip;
                    }
                }

                //Debug.LogWarning(array[i].nameToken + " " + array[i].extraData);
            }





            return array;
        }

        public static RoR2.UI.LogBook.Entry[] ChangeSurvivorLogbookEntry(On.RoR2.UI.LogBook.LogBookController.orig_BuildSurvivorEntries orig, Dictionary<ExpansionDef, bool> expansionAvailability)
        {
            RoR2.UI.LogBook.Entry[] array = orig(expansionAvailability);

            for (int i = 0; i < array.Length; i++)
            {
                array[i].color = NewSurvivorLogbookNameColor;

                //Debug.LogWarning(array[i].nameToken + " " + array[i].extraData);
            }





            return array;
        }



   



        private void OneTimeOnlyLateRunner(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);


            RoR2Content.Items.MinHealthPercentage.hidden = false;

            /*
            for (int i = 0; i< BuffCatalog.eliteBuffIndices.Length;i++)
            {
                Debug.LogWarning(BuffCatalog.GetBuffDef(BuffCatalog.eliteBuffIndices[i]));
            }
            */


            //BuffCatalog.SetBuffDefs(BuffCatalog.buffDefs);

            if (ConfigSprintingCrosshair.Value == true)
            {
                SprintingCrosshair.GetComponent<UnityEngine.UI.RawImage>().texture = LoaderCrosshair.GetComponent<UnityEngine.UI.RawImage>().texture;
                SprintingCrosshairUI.maxSpreadAngle = 11;
                SprintingCrosshairUI.spriteSpreadPositions = new RoR2.UI.CrosshairController.SpritePosition[]
                {
                new RoR2.UI.CrosshairController.SpritePosition
                {
                    onePosition = new Vector3(96,0,0),
                    target = null,
                    zeroPosition = new Vector3(24,0,0)
                },
                new RoR2.UI.CrosshairController.SpritePosition
                {
                    onePosition = new Vector3(-96,0,0),
                    target = null,
                    zeroPosition = new Vector3(-24,0,0)
                },
                };
            }




            //Mithrix Void Stuff

            ItemDisplayRuleSet tempIDRS = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;


            foreach (RoR2.Items.ContagiousItemManager.TransformationInfo transformationInfo in RoR2.Items.ContagiousItemManager.transformationInfos)
            {
                DisplayRuleGroup originalDisplayRule = tempIDRS.GetItemDisplayRuleGroup(transformationInfo.originalItem);
                //Debug.LogWarning(originalDisplayRule + "  " + originalDisplayRule.isEmpty);


                ItemDef transformedItem = ItemCatalog.GetItemDef(transformationInfo.transformedItem);

                GameObject FollowerPrefab = MithrixCrystalPink;
                if (transformedItem.tier == ItemTier.VoidTier1)
                {
                    FollowerPrefab = MithrixCrystalPinkSmall;
                }

                if (originalDisplayRule.isEmpty == false)
                {

                    //originalDisplayRule.rules

                    ItemDisplayRule[] newDisplayRules = new ItemDisplayRule[0];
                    for (int i = 0; i < originalDisplayRule.rules.Length; i++)
                    {
                        ItemDisplayRule newRule = new ItemDisplayRule
                        {
                            ruleType = originalDisplayRule.rules[i].ruleType,
                            followerPrefab = FollowerPrefab,
                            childName = originalDisplayRule.rules[i].childName,
                            localPos = originalDisplayRule.rules[i].localPos,
                            localAngles = originalDisplayRule.rules[i].localAngles,
                            localScale = originalDisplayRule.rules[i].localScale,
                            limbMask = originalDisplayRule.rules[i].limbMask,
                        };
                        newDisplayRules = newDisplayRules.Add(newRule);
                    }


                    ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetGroup = new ItemDisplayRuleSet.KeyAssetRuleGroup
                    {
                        keyAsset = transformedItem,
                        displayRuleGroup = new DisplayRuleGroup
                        {
                            rules = newDisplayRules,
                        }
                    };

                    tempIDRS.keyAssetRuleGroups = tempIDRS.keyAssetRuleGroups.Add(keyAssetGroup);
                }



            }

            tempIDRS.GenerateRuntimeValues();



            On.RoR2.UI.MainMenu.MainMenuController.Start -= OneTimeOnlyLateRunner;
        }


        public static void VoidAlliesDifferentLooks()
        {
            GameObject NullifierBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Nullifier/NullifierBody.prefab").WaitForCompletion();
            GameObject VoidJailerBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidJailer/VoidJailerBody.prefab").WaitForCompletion();
            GameObject VoidMegaCrabBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidMegaCrab/VoidMegaCrabBody.prefab").WaitForCompletion();

            GameObject NullifierAllyBody = PrefabAPI.InstantiateClone(NullifierBody, "NullifierAllyBody", true);
            ContentAddition.AddBody(NullifierAllyBody);
            NullifierAllyBody.GetComponent<DeathRewards>().logUnlockableDef = null;
            GameObject VoidJailerAllyBody = PrefabAPI.InstantiateClone(VoidJailerBody, "VoidJailerAllyBody", true);
            ContentAddition.AddBody(VoidJailerAllyBody);
            VoidJailerAllyBody.GetComponent<DeathRewards>().logUnlockableDef = null;
            GameObject VoidMegaCrabAllyBody = PrefabAPI.InstantiateClone(VoidMegaCrabBody, "VoidMegaCrabAllyBody", true);
            ContentAddition.AddBody(VoidMegaCrabAllyBody);
            VoidMegaCrabAllyBody.GetComponent<DeathRewards>().logUnlockableDef = null;


            GameObject NullifierAllyMaster = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Nullifier/NullifierAllyMaster.prefab").WaitForCompletion();
            GameObject VoidJailerAllyMaster = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidJailer/VoidJailerAllyMaster.prefab").WaitForCompletion();
            GameObject VoidMegaCrabAllyMaster = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidMegaCrab/VoidMegaCrabAllyMaster.prefab").WaitForCompletion();

            GameObject mdlVoidSuperMegaCrab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/mdlVoidSuperMegaCrab.fbx").WaitForCompletion();
            CharacterSpawnCard cscVoidMegaCrabAlly = Instantiate(Addressables.LoadAssetAsync<CharacterSpawnCard>(key: "RoR2/DLC1/VoidMegaCrab/cscVoidMegaCrab.asset").WaitForCompletion());
            cscVoidMegaCrabAlly.name = "cscVoidMegaCrabAlly";
            cscVoidMegaCrabAlly.prefab = VoidMegaCrabAllyMaster;

            NullifierAllyMaster.GetComponent<CharacterMaster>().bodyPrefab = NullifierAllyBody;
            VoidJailerAllyMaster.GetComponent<CharacterMaster>().bodyPrefab = VoidJailerAllyBody;
            VoidMegaCrabAllyMaster.GetComponent<CharacterMaster>().bodyPrefab = VoidMegaCrabAllyBody;


            //
            CharacterModel.RendererInfo[] NullifierRenderInfos = NullifierAllyBody.transform.GetChild(0).GetChild(0).GetComponent<CharacterModel>().baseRendererInfos;

            Texture2D texNullifierDiffuse2 = new Texture2D(512, 512, TextureFormat.DXT1, false);
            texNullifierDiffuse2.LoadImage(Properties.Resources.texNullifierDiffuse2, false);
            texNullifierDiffuse2.filterMode = FilterMode.Bilinear;
            texNullifierDiffuse2.wrapMode = TextureWrapMode.Repeat;

            Material matNullifierAlly = Instantiate(NullifierRenderInfos[0].defaultMaterial);
            matNullifierAlly.name = "matNullifierAlly";
            matNullifierAlly.mainTexture = texNullifierDiffuse2;
            matNullifierAlly.color = new Color(0.9f, 1.1f, 1f, 1f);
            matNullifierAlly.SetColor("_EmColor", new Color(0.5f, 2f, 0.7f, 1));
            NullifierRenderInfos[0].defaultMaterial = matNullifierAlly;

            Material matNullifierArmorAlly = Instantiate(NullifierRenderInfos[1].defaultMaterial);
            matNullifierArmorAlly.name = "matNullifierArmorAlly";
            matNullifierArmorAlly.color = new Color(0.3f, 0.55f, 0.4f, 1);
            NullifierRenderInfos[1].defaultMaterial = matNullifierArmorAlly;
            //
            //
            CharacterModel.RendererInfo[] VoidJailerRenderInfos = VoidJailerAllyBody.transform.GetChild(0).GetChild(0).GetComponent<CharacterModel>().baseRendererInfos;

            Texture2D texTrimsheetJailerDiffuse = new Texture2D(512, 1024, TextureFormat.DXT5, false);
            texTrimsheetJailerDiffuse.LoadImage(Properties.Resources.texTrimsheetJailerDiffuse, false);
            texTrimsheetJailerDiffuse.filterMode = FilterMode.Bilinear;
            texTrimsheetJailerDiffuse.wrapMode = TextureWrapMode.Repeat;
            texTrimsheetJailerDiffuse.wrapModeV = TextureWrapMode.Mirror;

            Material matVoidJailerAlly = Instantiate(VoidJailerRenderInfos[0].defaultMaterial);
            matVoidJailerAlly.name = "matVoidJailerAlly";
            matVoidJailerAlly.mainTexture = texTrimsheetJailerDiffuse;
            matVoidJailerAlly.color = new Color(1f, 1.2f, 1.2f, 1);
            VoidJailerRenderInfos[0].defaultMaterial = matVoidJailerAlly;

            Material matVoidJailerMetalAlly = Instantiate(VoidJailerRenderInfos[1].defaultMaterial);
            matVoidJailerMetalAlly.name = "matVoidJailerMetalAlly";
            matVoidJailerMetalAlly.color = new Color(0.6f, 0.6f, 0.6f, 1);
            VoidJailerRenderInfos[1].defaultMaterial = matVoidJailerMetalAlly;

            Material matVoidJailerEyesAlly = Instantiate(VoidJailerRenderInfos[2].defaultMaterial);
            matVoidJailerEyesAlly.name = "matVoidJailerEyesAlly";
            matVoidJailerEyesAlly.SetColor("_EmColor", new Color(0.6f, 1f, 0.6f, 1));
            VoidJailerRenderInfos[2].defaultMaterial = matVoidJailerEyesAlly;


            CharacterModel.RendererInfo[] VoidMegaCrabRenderInfos = VoidMegaCrabAllyBody.transform.GetChild(0).GetChild(3).GetComponent<CharacterModel>().baseRendererInfos;

            Texture2D texVoidMegaCrabDiffuse = new Texture2D(2048, 2048, TextureFormat.DXT1, false);
            texVoidMegaCrabDiffuse.LoadImage(Properties.Resources.texVoidMegaCrabDiffuse, false);
            texVoidMegaCrabDiffuse.filterMode = FilterMode.Bilinear;
            texVoidMegaCrabDiffuse.wrapMode = TextureWrapMode.Clamp;

            Material matVoidMegaCrabAlly = Instantiate(VoidMegaCrabRenderInfos[2].defaultMaterial);
            matVoidMegaCrabAlly.name = "matVoidMegaCrabAlly";
            matVoidMegaCrabAlly.mainTexture = texVoidMegaCrabDiffuse;
            matVoidMegaCrabAlly.SetColor("_EmColor", new Color(0f, 1.7f, 1.53f, 1));
            VoidMegaCrabRenderInfos[2].defaultMaterial = matVoidMegaCrabAlly;

            Material matVoidMegaCrabArmorAlly = Instantiate(VoidMegaCrabRenderInfos[3].defaultMaterial);
            matVoidMegaCrabArmorAlly.name = "matVoidMegaCrabArmorAlly";
            matVoidMegaCrabArmorAlly.color = new Color(0.45f, 0.65f, 0.45f, 1);
            VoidMegaCrabRenderInfos[3].defaultMaterial = matVoidMegaCrabArmorAlly;





            Texture2D BodyIconNullifierBody = new Texture2D(128, 128, TextureFormat.DXT5, false);
            BodyIconNullifierBody.LoadImage(Properties.Resources.BodyIconNullifierBody, false);
            BodyIconNullifierBody.filterMode = FilterMode.Bilinear;
            BodyIconNullifierBody.wrapMode = TextureWrapMode.Clamp;
            Texture2D BodyIconVoidJailer = new Texture2D(128, 128, TextureFormat.DXT5, false);
            BodyIconVoidJailer.LoadImage(Properties.Resources.BodyIconVoidJailer, false);
            BodyIconVoidJailer.filterMode = FilterMode.Bilinear;
            BodyIconVoidJailer.wrapMode = TextureWrapMode.Clamp;
            Texture2D BodyIconDevastatorBody = new Texture2D(128, 128, TextureFormat.DXT5, false);
            BodyIconDevastatorBody.LoadImage(Properties.Resources.BodyIconDevastatorBody, false);
            BodyIconDevastatorBody.filterMode = FilterMode.Bilinear;
            BodyIconDevastatorBody.wrapMode = TextureWrapMode.Clamp;

            NullifierAllyBody.GetComponent<CharacterBody>().portraitIcon = BodyIconNullifierBody;
            VoidJailerAllyBody.GetComponent<CharacterBody>().portraitIcon = BodyIconVoidJailer;
            VoidMegaCrabAllyBody.GetComponent<CharacterBody>().portraitIcon = BodyIconDevastatorBody;



            /*
            //Void Super Mega Crab not functional
            VoidMegaCrabRenderInfos[2].defaultMaterial = mdlVoidSuperMegaCrab.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
            (VoidMegaCrabRenderInfos[2].renderer as SkinnedMeshRenderer).sharedMesh= mdlVoidSuperMegaCrab.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh;

            VoidMegaCrabRenderInfos[3].defaultMaterial = mdlVoidSuperMegaCrab.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
            (VoidMegaCrabRenderInfos[3].renderer as SkinnedMeshRenderer).sharedMesh = mdlVoidSuperMegaCrab.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().sharedMesh;
            */

            On.RoR2.VoidMegaCrabItemBehavior.Awake += (orig, self) =>
            {
                orig(self);
                self.spawnSelection.choices[2].value = cscVoidMegaCrabAlly;
            };
        }


        public void Awake()
        {
            InitConfig();

            SkinChanges.Awake();
            GameplayQoL.Awake();

            DuplicatorModelChanger();
            LanguageChanger();

           
            BuffColorChanger();

            //Ripped
            PickupBackgroundCollision();
            PickupTeleportHook();
            On.RoR2.UI.MainMenu.MainMenuController.Start += OneTimeOnlyLateRunner;

            //VoidAlliesDifferentLooks();

            On.RoR2.UI.ChatBox.Awake += (orig, self) =>
            {
                orig(self);
                //Debug.LogWarning(self);
                if (self.name.StartsWith("ChatBox, In Run"))
                {

                    bool HasDPSMeter = self.transform.parent.parent.parent.parent.Find("DPSMeterPanel");


                    RectTransform ChatboxTranform = self.gameObject.GetComponent<RectTransform>();
                    if (ChatboxTranform)
                    {
                        ChatboxTranform.GetComponent<RoR2.UI.ChatBox>().allowExpandedChatbox = true;

                        RectTransform tempRectTransformInput = ChatboxTranform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
                        tempRectTransformInput.offsetMax = new Vector2(48, 48);
                        tempRectTransformInput.offsetMin = new Vector2(-8, 0);

                        RectTransform tempRectTransformExpanded = ChatboxTranform.GetChild(1).GetComponent<RectTransform>();
                        tempRectTransformExpanded.anchorMax = new Vector2(1, 1);
                        tempRectTransformExpanded.offsetMax = new Vector2(120, 0);
                        tempRectTransformExpanded.offsetMin = new Vector2(-10, 49);

                        RectTransform tempRectTransform = ChatboxTranform.GetChild(2).GetComponent<RectTransform>();
                        if (HasDPSMeter)
                        {
                            tempRectTransform.offsetMax = new Vector2(120, 0);
                            tempRectTransform.offsetMin = new Vector2(-10, 49);
                        }
                        else
                        {
                            tempRectTransform.offsetMax = new Vector2(120, -6);
                            tempRectTransform.offsetMin = new Vector2(-10, 0);
                        }


                    }

                }
            };




            On.EntityStates.VagrantNovaItem.ChargeState.OnEnter += (orig, self) =>
            {
                orig(self);
                if (self.areaIndicatorVfxInstance)
                {
                    VagrantExplosionWarningBuff vagrantExplosionWarningBuff = self.areaIndicatorVfxInstance.GetComponent<VagrantExplosionWarningBuff>();
                    if (vagrantExplosionWarningBuff)
                    {
                        vagrantExplosionWarningBuff.attacker = self.attachedBody.gameObject;
                        vagrantExplosionWarningBuff.radius = EntityStates.VagrantNovaItem.DetonateState.blastRadius;
                        vagrantExplosionWarningBuff.teamIndex = self.attachedBody.teamComponent.teamIndex;
                        vagrantExplosionWarningBuff.transPos = self.attachedBody.coreTransform;
                    }
                }
            };
            On.EntityStates.VagrantMonster.ChargeMegaNova.OnEnter += (orig, self) =>
            {
                orig(self);
                if (self.areaIndicatorInstance)
                {
                    VagrantExplosionWarningBuff vagrantExplosionWarningBuff = self.areaIndicatorInstance.GetComponent<VagrantExplosionWarningBuff>();
                    if (vagrantExplosionWarningBuff)
                    {
                        vagrantExplosionWarningBuff.attacker = self.gameObject;
                        vagrantExplosionWarningBuff.radius = EntityStates.VagrantMonster.ChargeMegaNova.novaRadius;
                        vagrantExplosionWarningBuff.teamIndex = self.GetTeam();
                        vagrantExplosionWarningBuff.transPos = self.transform;
                    }
                }
            };

            VagrantExplosionWarningBuff vagrantexplosionthing = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Vagrant/VagrantNovaAreaIndicator.prefab").WaitForCompletion().AddComponent<VagrantExplosionWarningBuff>();
            vagrantexplosionthing.buffDef = FakeVagrantExplosion;





            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBoss/RoboBallMiniBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallRedBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallGreenBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU1 Props.prefab").WaitForCompletion();
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU2 Props.prefab").WaitForCompletion();
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU3 Props.prefab").WaitForCompletion();
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU4 Props.prefab").WaitForCompletion();





            GameObject SulfurpoolsDioramaDisplay = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/sulfurpools/SulfurpoolsDioramaDisplay.prefab").WaitForCompletion();
            MeshRenderer SPDiaramaRenderer = SulfurpoolsDioramaDisplay.transform.GetChild(2).GetComponent<MeshRenderer>();
            Material SPRingAltered = Instantiate(SPDiaramaRenderer.material);
            SPRingAltered.SetTexture("_SnowTex", Addressables.LoadAssetAsync<Texture2D>(key: "RoR2/DLC1/sulfurpools/texSPGroundDIFVein.tga").WaitForCompletion());
            SPDiaramaRenderer.material = SPRingAltered;

            RoR2.ModelPanelParameters VoidStageDiorama = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/voidstage/VoidStageDiorama.prefab").WaitForCompletion().GetComponent<ModelPanelParameters>();
            VoidStageDiorama.minDistance = 20;
            VoidStageDiorama.minDistance = 240;

            /*
            On.RoR2.UI.PingIndicator.RebuildPing += (orig, self) =>
            {
                orig(self);
                if (self.pingType == RoR2.UI.PingIndicator.PingType.Interactable)
                {
                    GenericPickupController tempgeneric = self.pingTarget.GetComponent<GenericPickupController>();
                    if (tempgeneric)
                    {
                        self.pingColor = tempgeneric.pickupIndex.pickupDef.baseColor * self.textBaseColor;
                        self.interactablePingGameObjects[0].GetComponent<SpriteRenderer>().color = self.pingColor;
                        self.interactablePingGameObjects[1].GetComponent<ParticleSystem>().startColor = self.pingColor;
                        self.pingText.color = self.pingColor;
                    }
                    else if (self.pingTarget.GetComponent<TeleporterInteraction>())
                    {
                        self.pingColor = new Color(0.4f, 0.8f, 1f, 1) * self.textBaseColor;
                        self.pingText.color = self.pingColor;
                        self.interactablePingGameObjects[0].GetComponent<SpriteRenderer>().color = self.pingColor;
                        self.interactablePingGameObjects[1].GetComponent<ParticleSystem>().startColor = self.pingColor;
                      
                    }
                    else
                    {
                        self.interactablePingGameObjects[0].GetComponent<SpriteRenderer>().color = self.interactablePingColor;
                        self.interactablePingGameObjects[1].GetComponent<ParticleSystem>().startColor = new Color(1f, 0.9168f, 0f, 1f);
                    }
                }

            };
            */

            On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.OnEnter += (orig, self) =>
            {
                orig(self);
                self.characterBody.AddSpreadBloom(0.5f);
            };
            On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.FixedUpdate += (orig, self) =>
            {
                orig(self);
                self.characterBody.SetSpreadBloom(0.25f, true);
            };


            CaptainHackingBeaconIndicatorMaterial.SetColor("_TintColor", new Color(0, 0.4f, 0.8f, 1f));

            On.EntityStates.TitanMonster.RechargeRocks.OnEnter += (orig, self) =>
            {
                ParticleSystemRenderer temprenderer = EntityStates.TitanMonster.RechargeRocks.rockControllerPrefab.transform.GetChild(1).GetComponent<UnityEngine.ParticleSystemRenderer>();
                if (self.gameObject.name.StartsWith("TitanGoldBody(Clone)"))
                {
                    temprenderer.material = Instantiate(temprenderer.material);
                    temprenderer.material.color = new Color(0.566f, 0.4352f, 0f, 1);
                }
                else
                {
                    temprenderer.material = Instantiate(temprenderer.material);
                    temprenderer.material.color = new Color(0.1587f, 0.1567f, 0.1691f, 1);
                }

                orig(self);
            };


            GameObject CaptainSupplyDropCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Captain/CaptainSupplyDropCrosshair.prefab").WaitForCompletion();
            RoR2.UI.CrosshairController CaptainSupplyUI = CaptainSupplyDropCrosshair.GetComponent<RoR2.UI.CrosshairController>();
            CaptainSupplyUI.skillStockSpriteDisplays[0].maximumStockCountToBeValid = 200000;
            CaptainSupplyUI.skillStockSpriteDisplays[1].maximumStockCountToBeValid = 200000;
            CaptainSupplyUI.skillStockSpriteDisplays[2].maximumStockCountToBeValid = 200000;
            CaptainSupplyUI.skillStockSpriteDisplays[3].maximumStockCountToBeValid = 200000;





           




            if (LysateCellHuntress.Value == true)
            {
                On.EntityStates.Huntress.AimArrowSnipe.OnEnter += (orig, self) =>
                {
                    orig(self);

                    //Debug.LogWarning(self.skillLocator.special.finalRechargeInterval);
                    //Debug.LogWarning(self.skillLocator.special.stock);

                    int specialStock = self.skillLocator.special.stock;
                    int baseMaxStock = self.primarySkillSlot.skillDef.baseMaxStock;
                    if (specialStock > 2)
                    {
                        specialStock = 2;
                    }
                    else if (self.skillLocator.special.finalRechargeInterval == 0.5)
                    {
                        specialStock--;
                    }

                    int stock = baseMaxStock * (specialStock + 1);
                    if (specialStock >= 0)
                    {
                        self.skillLocator.special.DeductStock(specialStock);
                    }
                    // if (stock < 3) { stock= 3; }
                    self.skillLocator.primary.stock = stock;


                };

            }




            On.RoR2.UI.PingIndicator.RebuildPing += (orig, self) =>
            {
                orig(self);
                self.fixedTimer = 3600;
            };


            EliteDef edSecretSpeed = Addressables.LoadAssetAsync<EliteDef>(key: "RoR2/DLC1/edSecretSpeed.asset").WaitForCompletion();
            EquipmentDef EliteSecretSpeedEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteSecretSpeedEquipment.asset").WaitForCompletion();
            EliteDef edEcho = Addressables.LoadAssetAsync<EliteDef>(key: "RoR2/InDev/edEcho.asset").WaitForCompletion();
            EquipmentDef EliteEchoEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/InDev/EliteEchoEquipment.asset").WaitForCompletion();

            edSecretSpeed.eliteEquipmentDef = EliteSecretSpeedEquipment;
            edSecretSpeed.shaderEliteRampIndex = -1;
            edEcho.eliteEquipmentDef = EliteEchoEquipment;


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion().GetComponent<WormBodyPositions2>().blastAttackEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Junk/ElectricWorm/ElectricWormImpactExplosion.prefab").WaitForCompletion();

            /*
            SceneDef renderitem = ScriptableObject.CreateInstance<SceneDef>();
            renderitem.sceneAddress = new AssetReferenceScene("722873b571c73734c8572658dbb8f0db");
            renderitem._cachedName = "renderitem";
            renderitem.baseSceneNameOverride = "renderitem";
            renderitem.shouldIncludeInLogbook = false;
            ContentAddition.AddSceneDef(renderitem);
            */


            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Commando/CommandoBodyFireFMJ.asset").WaitForCompletion().mustKeyPress = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Croco/CrocoSpit.asset").WaitForCompletion().mustKeyPress = false;
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/LunarSkillReplacements/LunarSecondaryReplacement.asset").WaitForCompletion().mustKeyPress = true;


            GameObject BeetleGuardAllyBodyPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/BeetleGland/BeetleGuardAllyBody.prefab").WaitForCompletion();
            Destroy(BeetleGuardAllyBodyPrefab.transform.GetChild(0).GetChild(0).GetComponent<ModelSkinController>());


            //On.RoR2.CharacterMaster.TryCloverVoidUpgrades += CharacterMaster_TryCloverVoidUpgrades;


            EquipmentCatalog.availability.CallWhenAvailable(YellowIconAdder);
            GameModeCatalog.availability.CallWhenAvailable(ModSupport);
            On.RoR2.Stage.Start += StageStartMethod;




            RoR2.LegacyResourcesAPI.Load<SceneDef>("SceneDefs/artifactworld").shouldIncludeInLogbook = true;


            RoR2.LegacyResourcesAPI.Load<SceneDef>("SceneDefs/voidstage").stageOrder = 97;
            Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC1/voidraid/voidraid.asset").WaitForCompletion().stageOrder = 98;

            RoR2.LegacyResourcesAPI.Load<SceneDef>("SceneDefs/artifactworld").stageOrder = 99;
            RoR2.LegacyResourcesAPI.Load<SceneDef>("SceneDefs/goldshores").stageOrder = 99;

            RoR2.LegacyResourcesAPI.Load<SceneDef>("SceneDefs/mysteryspace").stageOrder = 99;
            RoR2.LegacyResourcesAPI.Load<SceneDef>("SceneDefs/limbo").stageOrder = 100;

            RoR2.LegacyResourcesAPI.Load<SceneDef>("SceneDefs/bazaar").stageOrder = 101;
            RoR2.LegacyResourcesAPI.Load<SceneDef>("SceneDefs/arena").stageOrder = 102;


            //RunArtifactManager.onArtifactEnabledGlobal += RunArtifactManager_onArtifactEnabledGlobal;
            //RunArtifactManager.onArtifactDisabledGlobal += RunArtifactManager_onArtifactDisabledGlobal;

            On.RoR2.SeerStationController.OnStartClient += (orig, self) =>
            {
                orig(self);

                if (self.NetworktargetSceneDefIndex != -1)
                {
                    string temp = Language.GetString(SceneCatalog.GetSceneDef((SceneIndex)self.NetworktargetSceneDefIndex).nameToken);
                    temp = temp.Replace("Hidden Realm: ", "");
                    self.gameObject.GetComponent<PurchaseInteraction>().contextToken = (Language.GetString("BAZAAR_SEER_CONTEXT") + " of " + temp);
                    self.gameObject.GetComponent<PurchaseInteraction>().displayNameToken = (Language.GetString("BAZAAR_SEER_NAME") + " (" + temp + ")");
                }


            };

            On.RoR2.SeerStationController.OnTargetSceneChanged += (orig, self, sceneDef) =>
            {
                orig(self, sceneDef);

                //Debug.LogWarning(sceneDef);
                if (sceneDef != null)
                {
                    string temp = Language.GetString(SceneCatalog.GetSceneDef((SceneIndex)self.NetworktargetSceneDefIndex).nameToken);
                    temp = temp.Replace("Hidden Realm: ", "");
                    self.gameObject.GetComponent<PurchaseInteraction>().contextToken = (Language.GetString("BAZAAR_SEER_CONTEXT") + " of " + temp);
                    self.gameObject.GetComponent<PurchaseInteraction>().displayNameToken = (Language.GetString("BAZAAR_SEER_NAME") + " (" + temp + ")");
                }
            };




            TeleporterChargedIcon = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator").GetComponent<RoR2.UI.ChargeIndicatorController>().iconSprites[0].sprite;

            On.RoR2.TeleporterInteraction.Awake += (orig, self) =>
            {
                RoR2.UI.ChargeIndicatorController original = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator").GetComponent<RoR2.UI.ChargeIndicatorController>();
                if (self.name.StartsWith("LunarTeleporter Variant(Clone)"))
                {
                    original.iconSprites[0].sprite = PrimordialTeleporterChargedIcon;
                }
                else
                {
                    original.iconSprites[0].sprite = TeleporterChargedIcon;
                }
                orig(self);
            };

            On.RoR2.TeleporterInteraction.Start += (orig, self) =>
            {
                orig(self);
                RoR2.UI.ChargeIndicatorController original = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator").GetComponent<RoR2.UI.ChargeIndicatorController>();
                if (self.name.StartsWith("LunarTeleporter Variant(Clone)"))
                {
                    original.iconSprites[0].sprite = PrimordialTeleporterChargedIcon;
                }
                else
                {
                    original.iconSprites[0].sprite = TeleporterChargedIcon;
                }
            };




            GameObject RachisObject = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/DamageZoneWard");

            RachisObject.transform.GetChild(1).GetChild(2).GetChild(1).localScale = new Vector3(2f, 2f, 2f);


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Plant/InterstellarDeskPlant.prefab").WaitForCompletion().transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);



            /*
            On.RoR2.ShopTerminalBehavior.Start += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("LunarCauldron,") || self.name.StartsWith("ShrineCleanse"))
                {
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                };
            };
            */

            On.RoR2.ShopTerminalBehavior.DropPickup += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("LunarCauldron,") || self.name.StartsWith("ShrineCleanse"))
                {
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                    RoR2.Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                };
            };




            //Command Troll
            On.RoR2.PickupPickerController.GetOptionsFromPickupIndex += (orig, pickupIndex) =>
            {

                if (pickupIndex == PickupCatalog.FindPickupIndex(JunkContent.Items.AACannon.itemIndex))
                {
                    int count = ItemCatalog.itemCount + EquipmentCatalog.equipmentCount;

                    PickupPickerController.Option[] array = new PickupPickerController.Option[count];
                    for (int i = 0; i < count; i++)
                    {

                        if (i < ItemCatalog.itemCount)
                        {
                            //Debug.LogWarning(pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i));
                            array[i] = new PickupPickerController.Option
                            {
                                available = true,
                                pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i)
                            };
                        }
                        else
                        {
                            //Debug.LogWarning(PickupCatalog.FindPickupIndex((EquipmentIndex)i - ItemCatalog.itemCount));
                            array[i] = new PickupPickerController.Option
                            {
                                available = true,
                                pickupIndex = PickupCatalog.FindPickupIndex((EquipmentIndex)i - ItemCatalog.itemCount)
                            };
                        }

                    }

                    return array;
                }

                return orig(pickupIndex);
            };






            On.EntityStates.Loader.FireHook.SetHookReference += (orig, self, hook) =>
            {
                if (self.characterBody.skinIndex != 0)
                {
                    hook.transform.GetChild(0).GetComponent<MeshRenderer>().material = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial;
                }
                orig(self, hook);
            };
            /*
            On.EntityStates.Loader.ThrowPylon.OnEnter += (orig, self) =>
            {
                orig(self);

                if (self.characterBody.skinIndex != LoaderPylonSkinIndex)
                {
                    LoaderPylonSkinIndex = self.characterBody.skinIndex;
                    SkinnedMeshRenderer temprender = EntityStates.Loader.ThrowPylon.projectilePrefab.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
                    temprender.materials[1] = Instantiate(temprender.materials[1]);
                    temprender.materials[1].mainTexture = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial.mainTexture;
                    //Debug.LogWarning("CHANGE LOADER PYLON");
                }
            };
            */

            RoR2.HoldoutZoneController TempLeptonDasiy1 = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/Teleporter1").GetComponent<RoR2.HoldoutZoneController>();
            LeptonDaisyTeleporterDecoration = TempLeptonDasiy1.healingNovaItemEffect;

            GlowFlowerForPillar = R2API.PrefabAPI.InstantiateClone(LeptonDaisyTeleporterDecoration, "GlowFlowerForPillar", false); //Special1 (Enter)


            GlowFlowerForPillar.transform.localScale = new Vector3(0.6f, 0.6f, 0.55f);
            GlowFlowerForPillar.transform.localPosition = new Vector3(0f, 0f, 1f);
            GlowFlowerForPillar.transform.localRotation = new Quaternion(0.5373f, 0.8434f, 0, 0);

            GlowFlowerForPillar.transform.GetChild(0).localPosition = new Vector3(-0.9f, -2.9f, 4.35f);
            GlowFlowerForPillar.transform.GetChild(0).localRotation = new Quaternion(-0.4991f, 0.0297f, 0.2375f, -0.8328f);
            GlowFlowerForPillar.transform.GetChild(0).localScale = new Vector3(0.4f, 0.4f, 0.4f);

            GlowFlowerForPillar.transform.GetChild(1).localPosition = new Vector3(0.9f, 2.9f, 4.3f);
            GlowFlowerForPillar.transform.GetChild(1).localRotation = new Quaternion(0.5f, 0f, 0, -0.866f);
            GlowFlowerForPillar.transform.GetChild(1).localScale = new Vector3(0.55f, 0.55f, 0.55f);

            GlowFlowerForPillar.transform.GetChild(2).localPosition = new Vector3(-2.7f, 1.3f, 4.4f);
            GlowFlowerForPillar.transform.GetChild(2).localRotation = new Quaternion(-0.1504f, -0.4924f, -0.0868f, 0.8529f);
            GlowFlowerForPillar.transform.GetChild(2).localScale = new Vector3(0.45f, 0.45f, 0.45f);

            GlowFlowerForPillar.transform.GetChild(3).localPosition = new Vector3(2.8f, -1f, 4.3f);
            GlowFlowerForPillar.transform.GetChild(3).localRotation = new Quaternion(0.1504f, 0.4924f, -0.0868f, 0.8529f);
            GlowFlowerForPillar.transform.GetChild(3).localScale = new Vector3(0.5f, 0.5f, 0.5f);


            //Debug.LogWarning(LeptonDaisyTeleporterDecoration);



            On.RoR2.HoldoutZoneController.Awake += (orig, self) =>
            {
                orig(self);

                //Debug.LogWarning(self);

                if (self.applyHealingNova)
                {
                    if (!self.healingNovaItemEffect)
                    {
                        if (self.name.StartsWith("InfiniteTowerSafeWardAwaitingInteraction(Clone)"))
                        {
                            self.healingNovaItemEffect = Instantiate(LeptonDaisyTeleporterDecoration, self.transform.GetChild(0).GetChild(0).GetChild(7).GetChild(0).GetChild(1).GetChild(0));
                            self.healingNovaItemEffect.transform.rotation = new Quaternion(0, -0.7071f, -0.7071f, 0);
                            self.healingNovaItemEffect.transform.localScale = new Vector3(0.1928f, 0.1928f, 0.1928f);
                            self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.4193f, 0);
                        }
                        else if (self.name.StartsWith("MoonBattery"))
                        {
                            self.healingNovaItemEffect = Instantiate(GlowFlowerForPillar, self.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));
                            //self.healingNovaItemEffect.transform.rotation = new Quaternion(-0.683f, -0.183f, -0.183f, 0.683f);
                            //self.healingNovaItemEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.55f);
                            //self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -2.4f, 0f);
                        }
                        else if (self.name.StartsWith("NullSafeZone"))
                        {
                            self.healingNovaItemEffect = Instantiate(LeptonDaisyTeleporterDecoration, self.transform);
                            self.healingNovaItemEffect.transform.rotation = new Quaternion(-0.5649f, -0.4254f, -0.4254f, 0.5649f);
                            self.healingNovaItemEffect.transform.localScale = new Vector3(0.275f, 0.275f, 0.225f);
                            self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.3f, 0f);
                        }
                        else if (self.name.StartsWith("DeepVoidPortalBattery(Clone)"))
                        {
                            self.healingNovaItemEffect = self.healingNovaItemEffect = Instantiate(GlowFlowerForPillar, self.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetChild(0));
                            self.healingNovaItemEffect.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
                            self.healingNovaItemEffect.transform.localScale = new Vector3(0.375f, 0.375f, 0.375f);
                            self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.4f, 0f);
                        }

                    }
                    if (Util.GetItemCountForTeam(TeamIndex.Player, RoR2Content.Items.TPHealingNova.itemIndex, false, true) > 0)
                    {
                        if (self.healingNovaItemEffect)
                        {
                            self.healingNovaItemEffect.SetActive(true);
                            self.healingNovaItemEffect = null;
                        }
                    }
                }



                //Debug.LogWarning("HoldoutZoneController.Awake");
            };

            On.RoR2.HoldoutZoneController.Start += (orig, self) =>
            {
                orig(self);


                if (self.applyHealingNova)
                {
                    if (Util.GetItemCountForTeam(TeamIndex.Player, RoR2Content.Items.TPHealingNova.itemIndex, false, true) > 0)
                    {
                        if (self.healingNovaItemEffect)
                        {
                            self.healingNovaItemEffect.SetActive(true);
                            self.healingNovaItemEffect = null;
                        }
                    }
                    else
                    {
                        if (self.healingNovaItemEffect)
                        {
                            self.healingNovaItemEffect.SetActive(false);
                        }
                    }
                }



                //Debug.LogWarning("HoldoutZoneController.Awake");
            };



            On.RoR2.GravitatePickup.OnTriggerEnter += (orig, self, collidor) =>
            {
                if (self.teamFilter.teamIndex == TeamIndex.Player)
                {
                    CharacterBody tempbody = collidor.gameObject.GetComponent<CharacterBody>();
                    if (tempbody && tempbody.isPlayerControlled)
                    {
                        orig(self, collidor);
                    }
                    return;
                }
                orig(self, collidor);
            };



            On.RoR2.AmmoPickup.OnTriggerStay += (orig, self, other) =>
            {
                if (self.teamFilter.teamIndex == TeamIndex.Player)
                {
                    CharacterBody component = other.GetComponent<CharacterBody>();
                    if (component && component.isPlayerControlled)
                    {
                        orig(self, other);
                    }
                    return;
                }
                orig(self, other);
            };

            On.RoR2.HealthPickup.OnTriggerStay += (orig, self, other) =>
            {
                if (self.teamFilter.teamIndex == TeamIndex.Player)
                {
                    CharacterBody component = other.GetComponent<CharacterBody>();
                    if (component && component.isPlayerControlled)
                    {
                        orig(self, other);
                    }
                    return;
                }
                orig(self, other);
            };




            On.EntityStates.Toolbot.ToolbotStanceA.OnEnter += MULTEquipmentThing;




            if (MountainShrineStack.Value == true)
            {
                On.RoR2.TeleporterInteraction.AddShrineStack += (orig, self) =>
                {
                    orig(self);


                    if (self.shrineBonusStacks == 1)
                    {
                        GameObject tempbossicon = self.bossShrineIndicator;

                        GameObject tempbossiconclone = Instantiate(tempbossicon, tempbossicon.transform);
                        NetworkServer.Spawn(tempbossiconclone);

                        Destroy(tempbossicon.GetComponent<MeshRenderer>());
                        Destroy(tempbossicon.GetComponent<Billboard>());
                        tempbossicon.transform.localPosition = new Vector3(0, 0, 6);
                        tempbossiconclone.transform.localScale = new Vector3(1, 1, 1);
                        tempbossiconclone.transform.localPosition = new Vector3(0, 0, 0);
                        tempbossiconclone.SetActive(true);
                    }

                    if (self.shrineBonusStacks > 1)
                    {
                        GameObject tempbossicon = self.bossShrineIndicator.transform.GetChild(0).gameObject;

                        GameObject tempbossiconclone = Instantiate(tempbossicon, tempbossicon.transform.parent);
                        NetworkServer.Spawn(tempbossiconclone);
                        tempbossiconclone.transform.localPosition = new Vector3(0, (self.shrineBonusStacks - 1), 0);
                    }

                };


            }






            //RoR2.LegacyResourcesAPI.Load<CharacterBody>("Prefabs/CharacterBodies/AssassinBody").baseNameToken = "Assassin";

            On.RoR2.GenericDisplayNameProvider.SetDisplayToken += (orig, self, token) =>
            {




                // Debug.LogWarning(token);
                if (token == "ARTIFACT_COMMAND_CUBE_BLUE_NAME" || token == "ARTIFACT_COMMAND_CUBE_YELLOW_NAME")
                {
                    PickupDisplay pickupDisplay = self.gameObject.GetComponentInChildren<PickupDisplay>();

                    //Debug.LogWarning(pickupDisplay);
                    //Debug.LogWarning(pickupDisplay.equipmentParticleEffect.activeSelf);
                    //Debug.LogWarning(pickupDisplay.bossParticleEffect.activeSelf);
                    //Debug.LogWarning(pickupDisplay.lunarParticleEffect.activeSelf);

                    if (pickupDisplay)
                    {
                        if (pickupDisplay.equipmentParticleEffect.activeSelf && pickupDisplay.bossParticleEffect.activeSelf)
                        {
                            token = "Marigold Command Essence";
                        }
                        else if (pickupDisplay.equipmentParticleEffect.activeSelf && pickupDisplay.lunarParticleEffect.activeSelf)
                        {
                            token = "Sapphire Command Essence";
                        }
                    }
                }
                orig(self, token);



            };


            /*
            ColorUtility.TryParseHtmlString(FakeBlueEquipColor.Value, out FakeBlueEquip);
            ColorUtility.TryParseHtmlString(FakeYellowEquipColor.Value, out FakeYellowEquip);
            ColorUtility.TryParseHtmlString(FakeBlueYellowEquipColor.Value, out FakeBlueYellowEquip);
            ColorUtility.TryParseHtmlString(FakeLunarCoinColor.Value, out FakeLunarCoin);
            */
            ColorUtility.TryParseHtmlString("#78AFFF", out FakeBlueEquip);
            ColorUtility.TryParseHtmlString("#FFC211", out FakeYellowEquip);
            //ColorUtility.TryParseHtmlString("#DEDEDE", out FakeBlueYellowEquip);
            //ColorUtility.TryParseHtmlString("#C6ADFA", out FakeLunarCoin);
            //ColorUtility.TryParseHtmlString("#ADBDFA", out FakeLunarCoin);
            FakeLunarCoin = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin);

            ColorUtility.TryParseHtmlString("#FF9EEC", out FakeVoidWhite);
            //ColorUtility.TryParseHtmlString("#C9E2FF", out FakeVoidGreen);
            ColorUtility.TryParseHtmlString("#F972F0", out FakeVoidRed);
            ColorUtility.TryParseHtmlString("#FF8FC1", out FakeVoidYellow);




            if (LunarCoinColorOutline.Value == false)
            {
                FakeLunarCoin = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarItem);
            }


            On.RoR2.UI.GenericNotification.SetEquipment += PickupEquipmentNotification;
            On.RoR2.UI.EquipmentIcon.Update += UIEquipmentIconColorChanger;
            On.RoR2.PickupPickerController.OnDisplayBegin += PickupPickerController_OnDisplayBegin;

            /*
            On.RoR2.CharacterBody.OnEquipmentGained += (orig, self, equipDef) =>
            {
                orig(self, equipDef);
                if (self.isPlayerControlled)
                {
                    if (equipDef.isBoss == true || equipDef.isLunar == true)
                    {
                        On.RoR2.UI.EquipmentIcon.Update += UIEquipmentIconColorChanger;
                    }
                };
                Debug.LogWarning("Sex");
            };
            */









            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += (orig, self, damageReport, networkUser) =>
            {
                orig(self, damageReport, networkUser);

                if (damageReport.victimMaster.IsDeadAndOutOfLivesServer())
                {
                    Inventory newInventory = new Inventory();
                    string newString = "";
                    GameEndingDamageReports.Add(damageReport);

                    if (damageReport.attackerBody)
                    {
                        newString = RoR2.Util.GetBestBodyName(damageReport.attacker);
                        newString = newString.Replace("\n", " ");
                        //Debug.LogWarning(newString);

                    }
                    GameOverBodyNames.Add(newString);

                    //newInventory = Instantiate(damageReport.attackerMaster.inventory);

                    if (damageReport.attackerMaster)
                    {
                        //newInventory = Instantiate(damageReport.attackerMaster.inventory);
                        Inventory temp = new Inventory();

                        var donddestroy = damageReport.attackerMaster.gameObject.GetComponent<SetDontDestroyOnLoad>();
                        if (donddestroy)
                        {
                            donddestroy.enabled = false;
                        }


                        newInventory = Instantiate(damageReport.attackerMaster.inventory);
                        NetworkServer.Spawn(newInventory.gameObject);

                        MonoBehaviour[] scav4 = newInventory.gameObject.GetComponentsInChildren<MonoBehaviour>();
                        for (int i = 0; i < scav4.Length; i++)
                        {
                            scav4[i].enabled = false;
                        }
                        newInventory.gameObject.GetComponent<NetworkIdentity>().enabled = true;
                        newInventory.enabled = true;


                        newInventory.CopyEquipmentFrom(damageReport.attackerMaster.inventory);
                        newInventory.itemAcquisitionOrder.Clear();
                        int[] array = newInventory.itemStacks;
                        int num = 0;
                        HG.ArrayUtils.SetAll<int>(array, num);

                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, Tier1DeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, Tier2DeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, Tier3DeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, BossDeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, LunarDeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, Void1DeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, Void2DeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, Void3DeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, Void4DeathItemFilterDelegate);
                        newInventory.AddItemsFrom(damageReport.attackerMaster.inventory, NoTierDeathItemFilterDelegate);


                        /* Umbras get messy, Artifact of Evo isn't in order anyways
                        List<ItemIndex> temporder = new List<ItemIndex>();
                        temporder.AddRange(damageReport.attackerMaster.inventory.itemAcquisitionOrder);
                        newInventory.SetFieldValue<List<ItemIndex>>("itemAcquisitionOrder", temporder);
                        */
                        //newInventory.CopyItemsFrom(damageReport.attackerMaster.inventory);

                    }

                    GameOverInventories.Add(newInventory);


                    //Debug.LogWarning(newString);
                    //Debug.LogWarning(newInventory);
                    //Debug.LogWarning(damageReport.attackerMaster.inventory);
                }

            };



            /*On.RoR2.Chat.AddPickupMessage += (orig, body, pickupToken, pickupColor, pickupQuantity) =>
            {
                orig(body, pickupToken, pickupColor, pickupQuantity);
            };
            */



            texEquipmentBossBG.LoadImage(Properties.Resources.texEquipmentBossBG, false);
            texEquipmentBossBG.filterMode = FilterMode.Bilinear;
            texEquipmentLunarBG.LoadImage(Properties.Resources.texEquipmentLunarBG, false);
            texEquipmentLunarBG.filterMode = FilterMode.Bilinear;

            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += ChangeEquipmentBGLogbook;
            On.RoR2.UI.LogBook.LogBookController.BuildSurvivorEntries += ChangeSurvivorLogbookEntry;











            //On.RoR2.Run.AdvanceStage += RunAdvanceStageMethodAlways;
            //On.RoR2.Run.Start += RunStartMethodAlways;



            /*
            On.RoR2.CharacterMaster.Start += (orig, self) =>
            {
                orig(self);
                if (self.inventory)
                {
                    if (self.inventory.GetItemCount(RoR2Content.Items.Ghost) > 0)
                    {
                        CharacterBody tempbod = self.GetBody();
                        if (tempbod)
                        {
                            tempbod.baseNameToken = "Ghost " + Language.GetString(tempbod.baseNameToken);
                            //Debug.LogWarning(tempbod.baseNameToken);
                        }
                    }
                    if (self.inventory && self.inventory.GetItemCount(RoR2Content.Items.SummonedEcho) > 0)
                    {
                        CharacterBody tempbod = self.GetBody();
                        if (tempbod)
                        {
                            tempbod.baseNameToken = "Echo " + Language.GetString(tempbod.baseNameToken);
                            //Debug.LogWarning(tempbod.baseNameToken);
                        }
                    }
                }
            };
            */













            On.RoR2.SeerStationController.SetRunNextStageToTarget += (orig, self) =>
            {
                bool isbazaar = false;
                if (SceneInfo.instance && SceneInfo.instance.sceneDef.baseSceneName == "bazaar")
                {
                    isbazaar = true;
                    GameObject ShopPortal = GameObject.Find("/PortalShop");
                    SceneDef tempscenedef = SceneCatalog.GetSceneDef((SceneIndex)self.NetworktargetSceneDefIndex);
                    //Debug.LogWarning(tempscenedef);

                    if (tempscenedef && ShopPortal)
                    {
                        ShopPortal.transform.localScale = new Vector3(0.7f, 1.26f, 0.7f);
                        ShopPortal.transform.position = new Vector3(12.88f, -5.53f, -7.34f);
                        GameObject tempportal = null;
                        GenericObjectiveProvider objective = null;
                        GameObject temp = null;
                        switch (tempscenedef.baseSceneName)
                        {
                            case "goldshores":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalGoldshores/PortalGoldshores.prefab").WaitForCompletion();
                                break;
                            case "mysteryspace":
                            case "limbo":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalMS/PortalMS.prefab").WaitForCompletion();
                                break;
                            case "artifactworld":
                                ShopPortal.transform.localPosition = new Vector3(12.88f, 0f, -7.34f);
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab").WaitForCompletion();
                                break;
                            case "arena":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArena/PortalArena.prefab").WaitForCompletion();
                                break;
                            case "voidstage":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion();
                                break;
                            case "voidraid":
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion();
                                break;
                            default:
                                tempportal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalShop/PortalShop.prefab").WaitForCompletion();
                                break;
                        }
                        if (tempportal != null)
                        {
                            objective = tempportal.GetComponent<GenericObjectiveProvider>();
                            if (objective) { objective.enabled = false; }
                            temp = Instantiate(tempportal, ShopPortal.transform.position, ShopPortal.transform.rotation);
                            NetworkServer.Spawn(temp);
                            if (objective) { objective.enabled = true; }
                            temp.GetComponent<SceneExitController>().destinationScene = tempscenedef;
                            temp.GetComponent<SceneExitController>().useRunNextStageScene = false;
                            temp.name = ShopPortal.name;
                            Destroy(ShopPortal);
                        }

                    }


                }




                orig(self);
                //Debug.LogWarning("SeerStationController.SetRunNextStageToTarget");
                if (isbazaar)
                {
                    SeerStationController[] SeerStationList = FindObjectsOfType(typeof(SeerStationController)) as SeerStationController[];
                    for (var i = 0; i < SeerStationList.Length; i++)
                    {
                        SeerStationList[i].gameObject.GetComponent<PurchaseInteraction>().SetAvailable(false);
                    }
                }
            };



            
            On.RoR2.MorgueManager.EnforceHistoryLimit += (orig) =>
            {
                List<MorgueManager.HistoryFileInfo> list = HG.CollectionPool<MorgueManager.HistoryFileInfo, List<MorgueManager.HistoryFileInfo>>.RentCollection();
                MorgueManager.GetHistoryFiles(list);
                int i = list.Count - 1;
                int num = Math.Max(MorgueManager.morgueHistoryLimit.value, 0);
                while (i >= num)
                {
                    i--;
                    MorgueManager.RemoveOldestHistoryFile();
                }
                HG.CollectionPool<MorgueManager.HistoryFileInfo, List<MorgueManager.HistoryFileInfo>>.ReturnCollection(list);
            };
            


            if (EquipmentDroneName.Value == true)
            {

                On.RoR2.Items.MinionLeashBodyBehavior.Start += (orig, self) =>
                {
                    orig(self);
                    //Debug.LogWarning("CharacterBody.MinionLeashBehavior.Start");
                    if (self.name.StartsWith("EquipmentDroneBody(Clone)") && self.body)
                    {
                        if (self.body.inventory.currentEquipmentIndex != EquipmentIndex.None && self.body.inventory.currentEquipmentIndex != DLC1Content.Equipment.BossHunterConsumed.equipmentIndex)
                        {
                            self.body.baseNameToken = Language.GetString("EQUIPMENTDRONE_BODY_NAME") + "\n(" + Language.GetString(EquipmentCatalog.GetEquipmentDef(self.body.inventory.currentEquipmentIndex).nameToken) + ")";
                        }
                    }

                };


                On.RoR2.Artifacts.EnigmaArtifactManager.OnServerEquipmentActivated += (orig, equipmentSlot, equipmentIndex) =>
                {
                    orig(equipmentSlot, equipmentIndex);

                    if (equipmentSlot.name.StartsWith("EquipmentDroneBody(Clone)"))
                    {
                        equipmentSlot.characterBody.baseNameToken = Language.GetString("EQUIPMENTDRONE_BODY_NAME") + "\n(" + Language.GetString(EquipmentCatalog.GetEquipmentDef(equipmentSlot.characterBody.inventory.currentEquipmentIndex).nameToken) + ")";
                    }
                };




            }








            HighlightYellowItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.9373f, 0.2667f, 1);
            HighlightPinkItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.498f, 0.9059f, 1);
            //HighlightOrangeItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.6f, 0.25f, 1); //Color(1f, 0.6471f, 0.298f, 1)
            HighlightOrangeItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.6471f, 0.298f, 1);
            HighlightOrangeBossItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1, 0.75f, 0f, 1);
            //HighlightOrangeLunarItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(0.35f, 0.5f, 1, 1);
            HighlightOrangeLunarItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = HighlightBlueItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor;
            HighlightBlueItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(0.3f, 0.6f, 1, 1); ;



            On.RoR2.Inventory.SetEquipmentIndex += (orig, self, equipmentIndex) =>
            {
                if (self.gameObject.GetComponent<RoR2.PlayerCharacterMasterController>() && equipmentIndex != self.currentEquipmentIndex && !RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.enigmaArtifactDef))
                {
                    On.RoR2.CharacterModel.SetEquipmentDisplay -= EquipmentHighlighter;
                    On.RoR2.CharacterModel.SetEquipmentDisplay += EquipmentHighlighter;
                    //body.modelLocator.modelTransform.gameObject.GetComponent<CharacterModel>().UpdateItemDisplay(inventory);
                    //Debug.LogWarning("Pickup " + equipmentIndex);
                    //Debug.LogWarning("Current " + self.currentEquipmentIndex);
                }
                orig(self, equipmentIndex);
            };






            On.RoR2.CharacterModel.HighlightItemDisplay += (orig, self, itemIndex) =>
            {
                switch (ItemCatalog.GetItemDef(itemIndex).tier)
                {
                    case ItemTier.Boss:
                        HighlighterIntChoice = 1;
                        break;
                    case ItemTier.Lunar:
                        HighlighterIntChoice = 2;
                        break;
                    case ItemTier.VoidTier1:
                    case ItemTier.VoidTier2:
                    case ItemTier.VoidTier3:
                    case ItemTier.VoidBoss:
                        HighlighterIntChoice = 3;
                        break;
                    default:
                        break;
                }

                orig(self, itemIndex);
                HighlighterIntChoice = 0;
            };
            On.RoR2.UI.HighlightRect.CreateHighlight += (orig, viewerBodyObject, targetRenderer, highlightPrefab, overrideDuration, visibleToAll) =>
            {
                GameObject tempprefab = highlightPrefab;
                switch (HighlighterIntChoice)
                {
                    case 1:
                        tempprefab = HighlightYellowItem;
                        break;
                    case 2:
                        tempprefab = HighlightBlueItem;
                        break;
                    case 3:
                        tempprefab = HighlightPinkItem;
                        break;
                    default:
                        break;
                }



                orig(viewerBodyObject, targetRenderer, tempprefab, overrideDuration, visibleToAll);

            };



            if (EquipmentInDeath.Value == true)
            {
                On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += GameEndEquipHelper;
                On.RoR2.UI.ItemInventoryDisplay.AllocateIcons += GameEndEquipInv;
            };




            On.RoR2.UI.MainMenu.MainMenuController.Start += (orig, self) =>
            {

                orig(self);
                //Debug.LogWarning(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);


                GameplayQoL.PrismaticTrialSeed = (uint)random.Next(999999999);


                GameObject tempmain = GameObject.Find("/HOLDER: Title Background");
                //Transform temp3 = tempmain.transform.FindChild("CU3 Props");

                GameObject ScavHolder = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU3 Props.prefab").WaitForCompletion(), tempmain.transform);

                Destroy(ScavHolder.transform.GetChild(3).gameObject);
                Destroy(ScavHolder.transform.GetChild(1).gameObject);
                Destroy(ScavHolder.transform.GetChild(0).gameObject);
                ScavHolder.SetActive(true);


                GameObject mdlGup = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GupBody.prefab").WaitForCompletion().transform.GetChild(0).GetChild(0).gameObject);
                mdlGup.GetComponent<Animator>().Play("Spawn1", 0, 1);
                mdlGup.transform.localScale = new Vector3(7, 7, 7);
                mdlGup.transform.localPosition = new Vector3(55.2201f, -0.9796f, 11.5f);
                mdlGup.transform.localEulerAngles = new Vector3(5.7003f, 247.4198f, 0);

                Transform extragamemodepos = self.extraGameModeMenuScreen.gameObject.transform.parent.GetChild(1);
                extragamemodepos.localPosition = new Vector3(36.24f, 2.7204f, 3.1807f);
                extragamemodepos.localEulerAngles = new Vector3(5.2085f, 38.4904f, 0);

                /*
                switch (MainMenuColorConfig.Value)
                {
                    case 0:
                        temp3.gameObject.SetActive(true);
                        temp3.GetChild(0).gameObject.SetActive(false);
                        temp3.GetChild(1).gameObject.SetActive(false);
                        temp3.GetChild(3).gameObject.SetActive(false);
                        break;
                    case 1:
                        temp3.gameObject.SetActive(true);
                        temp3.GetChild(0).gameObject.SetActive(false);
                        temp3.GetChild(1).gameObject.SetActive(false);
                        temp3.GetChild(3).gameObject.SetActive(false);
                        tempmain.transform.FindChild("CU1 Props").gameObject.SetActive(true);
                        tempmain.transform.FindChild("CU1 Props").GetChild(0).gameObject.SetActive(true);
                        break;
                    case 2:
                        temp3.gameObject.SetActive(true);
                        temp3.GetChild(0).gameObject.SetActive(false);
                        temp3.GetChild(1).gameObject.SetActive(false);
                        temp3.GetChild(3).gameObject.SetActive(false);
                        tempmain.transform.FindChild("CU2 Props").gameObject.SetActive(true);
                        break;
                    case 3:
                        tempmain.transform.FindChild("CU3 Props").gameObject.SetActive(true);
                        break;
                    case 4:
                        tempmain.transform.FindChild("CU4 Props").gameObject.SetActive(true);
                        break;
                }
                */

            };









            //Add Automatic mod compatibility here I think
            if (EnableDeathMessage.Value == true)
            {
                WolfoItemMessages.DetailedDeathMessage();
            }
            if (EnableTradeMessage.Value == true)
            {
                WolfoItemMessages.ItemLostMessages();
            }



            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;
            
            if (PingIcons.Value == true)
            {
                PingInfo();
                On.RoR2.SceneDirector.Start += PingIconChanger;

            }

            if (EnableColorChangeModule.Value == true)
            {
                On.RoR2.Run.Start += StupidColorChanger;
            }








            if (ConsumedRustedKeyConfig.Value == true)
            {
                CreateUsedKey();
            }

            /*
            if (UntieredCaptainConfig.Value == true)
            {
                Texture2D TexUntierCaptain = new Texture2D(128, 128, TextureFormat.DXT5, false);
                TexUntierCaptain.LoadImage(Properties.Resources.texItemUntieredCaptain, false);
                TexUntierCaptain.filterMode = FilterMode.Bilinear;
                TexUntierCaptain.wrapMode = TextureWrapMode.Clamp;
                Sprite TexUntierCaptainS = Sprite.Create(TexUntierCaptain, rec128, half);

                ItemDef CaptainDefense = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/CaptainDefenseMatrix");
                CaptainDefense.tier = ItemTier.NoTier;
                CaptainDefense.pickupIconSprite = TexUntierCaptainS;
            }
            */



            GameObject PrefabShrineCleanse = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanse.prefab").WaitForCompletion();
            GameObject PrefabShrineCleanseSand = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSandy Variant.prefab").WaitForCompletion();
            GameObject PrefabShrineCleanseSnow = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSnowy Variant.prefab").WaitForCompletion();

            PrefabLockbox = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion();
            PrefabLockboxVoid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion();

            PrefabVendingMachine = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion();



            PrefabShrineCleanse.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
            PrefabShrineCleanseSand.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
            PrefabShrineCleanseSnow.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
            PrefabLockbox.AddComponent<RoR2.Hologram.HologramProjector>();
            PrefabLockboxVoid.AddComponent<RoR2.Hologram.HologramProjector>();
            PrefabVendingMachine.AddComponent<RoR2.Hologram.HologramProjector>();


            //PrefabLockbox.AddComponent<GenericObjectiveProvider>().objectiveToken = "Unlock the <style=cisDamage>Rusty Lockbox</style>";
            //PrefabLockboxVoid.AddComponent<GenericObjectiveProvider>().objectiveToken = "Unlock the <color=#FF9EEC>Encrusted Lockbox</color>";


            PrefabShrineCleanse.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);
            PrefabShrineCleanseSand.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);
            PrefabShrineCleanseSnow.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);

            PrefabShrineCleanse.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanse.transform.GetChild(1);
            PrefabShrineCleanseSand.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanseSand.transform.GetChild(1);
            PrefabShrineCleanseSnow.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanseSnow.transform.GetChild(1);




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
                    NullCell.iconSprites[0].sprite = NullVentIcon;
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
                Destroy(NullTempPosIndicator);
                self.outer.gameObject.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_TintColor", VoidDefault);

            };



            //Bead Giver
            GivePickupsOnStart.ItemInfo Beads = new GivePickupsOnStart.ItemInfo { itemString = ("LunarTrinket"), count = 1, };
            GivePickupsOnStart.ItemInfo Steak = new GivePickupsOnStart.ItemInfo { itemString = ("FlatHealth"), count = 1, };
            //GivePickupsOnStart.ItemInfo TPHealingNova = new GivePickupsOnStart.ItemInfo { itemString = ("TPHealingNova"), count = 1, };
            //GivePickupsOnStart.ItemInfo BossDamageBonus = new GivePickupsOnStart.ItemInfo { itemString = ("BossDamageBonus"), count = 1, };
            //GivePickupsOnStart.ItemInfo ExecuteLowHealthElite = new GivePickupsOnStart.ItemInfo { itemString = ("ExecuteLowHealthElite"), count = 1, };
            //GivePickupsOnStart.ItemInfo Firework = new GivePickupsOnStart.ItemInfo { itemString = ("Firework"), count = 1, };
            //GivePickupsOnStart.ItemInfo HeadHunter = new GivePickupsOnStart.ItemInfo { itemString = ("HeadHunter"), count = 1, };
            //GivePickupsOnStart.ItemInfo KillEliteFrenzy = new GivePickupsOnStart.ItemInfo { itemString = ("KillEliteFrenzy"), count = 1, };


            //GivePickupsOnStart.ItemInfo[] ScavLunarBeadsGiver = new GivePickupsOnStart.ItemInfo[0];
            GivePickupsOnStart.ItemInfo[] ScavLunarKipKipGiver = new GivePickupsOnStart.ItemInfo[0];
            //GivePickupsOnStart.ItemInfo[] ScavLunarWipWipGiver = new GivePickupsOnStart.ItemInfo[0];
            //GivePickupsOnStart.ItemInfo[] ScavLunarTwipTwipGiver = new GivePickupsOnStart.ItemInfo[0];
            //GivePickupsOnStart.ItemInfo[] ScavLunarGuraGuraGiver = new GivePickupsOnStart.ItemInfo[0];
            GivePickupsOnStart.ItemInfo[] ScavLunarBeadsGiver = new GivePickupsOnStart.ItemInfo[0];

            ScavLunarKipKipGiver = ScavLunarKipKipGiver.Add(Beads, Steak);
            //ScavLunarWipWipGiver = ScavLunarWipWipGiver.Add(Beads, BossDamageBonus, Firework);
            //ScavLunarTwipTwipGiver = ScavLunarTwipTwipGiver.Add(Beads, HeadHunter);
            //ScavLunarGuraGuraGiver = ScavLunarGuraGuraGiver.Add(Beads, KillEliteFrenzy);
            ScavLunarBeadsGiver = ScavLunarBeadsGiver.Add(Beads);


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar1Master").AddComponent<GivePickupsOnStart>().itemInfos = ScavLunarKipKipGiver;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar2Master").AddComponent<GivePickupsOnStart>().itemInfos = ScavLunarBeadsGiver;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar3Master").AddComponent<GivePickupsOnStart>().itemInfos = ScavLunarBeadsGiver;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar4Master").AddComponent<GivePickupsOnStart>().itemInfos = ScavLunarBeadsGiver;

            Destroy(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar1Master").GetComponents<RoR2.CharacterAI.AISkillDriver>()[1]);
            Destroy(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar2Master").GetComponents<RoR2.CharacterAI.AISkillDriver>()[1]);
            Destroy(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar3Master").GetComponents<RoR2.CharacterAI.AISkillDriver>()[1]);
            Destroy(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar4Master").GetComponents<RoR2.CharacterAI.AISkillDriver>()[1]);
            //







            On.RoR2.UI.LogBook.LogBookController.BuildStaticData += LogbookEntryAdder;


            if (UpdateBodyIconsConfig.Value == true)
            {
                Texture2D TexBodyBrotherHurt = new Texture2D(128, 128, TextureFormat.DXT5, false);
                TexBodyBrotherHurt.LoadImage(Properties.Resources.texBodyBrotherHurt, false);
                TexBodyBrotherHurt.filterMode = FilterMode.Bilinear;
                TexBodyBrotherHurt.wrapMode = TextureWrapMode.Clamp;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrotherHurt;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHauntBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrotherHurt;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHauntBody").GetComponent<CharacterBody>().baseNameToken = "BROTHER_BODY_NAME";
                //LanguageAPI.Add("BROTHERHURT_BODY_NAME", "Hurt Brother", "en");
                //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponent<CharacterBody>().baseNameToken = "BROTHERHURT_BODY_NAME";

                Texture2D TexWalkerTurretIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
                TexWalkerTurretIcon.LoadImage(Properties.Resources.texBodyEngiWalkerTurret, false);
                TexWalkerTurretIcon.filterMode = FilterMode.Bilinear;
                TexWalkerTurretIcon.wrapMode = TextureWrapMode.Clamp;

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody").GetComponent<CharacterBody>().portraitIcon = TexWalkerTurretIcon;

                Texture2D TexProbeGreen = new Texture2D(128, 128, TextureFormat.DXT5, false);
                TexProbeGreen.LoadImage(Properties.Resources.texBodyRoboBallBuddyGreen, false);
                TexProbeGreen.filterMode = FilterMode.Bilinear;
                TexProbeGreen.wrapMode = TextureWrapMode.Clamp;
                Texture2D TexProbeRed = new Texture2D(128, 128, TextureFormat.DXT5, false);
                TexProbeRed.LoadImage(Properties.Resources.texBodyRoboBallBuddyRed, false);
                TexProbeRed.filterMode = FilterMode.Bilinear;
                TexProbeRed.wrapMode = TextureWrapMode.Clamp;

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallRedBuddyBody").GetComponent<CharacterBody>().portraitIcon = TexProbeRed;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallGreenBuddyBody").GetComponent<CharacterBody>().portraitIcon = TexProbeGreen;


            }





            On.RoR2.UI.GameEndReportPanelController.SetDisplayData += (orig, self, data) =>
            {
                //Debug.LogWarning(self.statsToDisplay.Length);


                if (Run.instance)
                {
                    if (Run.instance.GetComponent<InfiniteTowerRun>())
                    {
                        string[] newstats = new string[] { "totalTimeAlive", "highestInfiniteTowerWaveReached", "totalItemsCollected", "totalKills", "totalEliteKills", "totalDamageDealt", "highestDamageDealt", "totalDamageTaken", "totalHealthHealed", "totalDeaths", "totalDistanceTraveled", "highestLevel", "totalPurchases", "totalLunarPurchases", "totalBloodPurchases", "totalGoldCollected", "totalMinionKills", "totalMinionDamageDealt" };
                        self.statsToDisplay = newstats;

                    }
                    else
                    {
                        string[] newstats = new string[] { "totalTimeAlive", "totalStagesCompleted", "totalItemsCollected", "totalKills", "totalEliteKills", "totalDamageDealt", "highestDamageDealt", "totalDamageTaken", "totalHealthHealed", "totalDeaths", "totalDistanceTraveled", "highestLevel", "totalPurchases", "totalLunarPurchases", "totalBloodPurchases", "totalDronesPurchased", "totalGoldCollected", "totalMinionKills", "totalMinionDamageDealt" };
                        self.statsToDisplay = newstats;
                    };
                }


                orig(self, data);
                if (self.chatboxTransform)
                {
                    self.chatboxTransform.gameObject.SetActive(true);
                }


            };

            On.RoR2.Run.BeginGameOver += (orig, self, gameEnd) =>
            {
                orig(self, gameEnd);
                //  Debug.LogWarning(gameEnd);

                if (gameEnd != RoR2Content.GameEndings.StandardLoss)
                {
                    foreach (var playerController in PlayerCharacterMasterController.instances)
                    {
                        if (gameEnd == RoR2Content.GameEndings.LimboEnding | gameEnd == RoR2Content.GameEndings.ObliterationEnding)
                        {
                            var tempsurv = SurvivorCatalog.FindSurvivorDefFromBody(playerController.master.bodyPrefab);
                            if (tempsurv && tempsurv.mainEndingEscapeFailureFlavorToken != null)
                            {
                                string token = "<style=cLunarObjective>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString(tempsurv.mainEndingEscapeFailureFlavorToken) + " <sprite name=\"CloudRight\" tint=1></style>";
                                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                            }
                            else
                            {
                                string token = "<style=cLunarObjective>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString("GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR") + " <sprite name=\"CloudRight\" tint=1></style>";
                                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                            }
                        }

                    }
                }

            };

            On.EntityStates.GameOver.VoidEndingFadeToBlack.OnExit += (orig, self) =>
            {
                orig(self);

                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/CreditsPanel.prefab").WaitForCompletion().transform.GetChild(4).GetComponent<MusicTrackOverride>().track = CreditsTrackVoid;

                foreach (var playerController in PlayerCharacterMasterController.instances)
                {
                    var tempsurv = SurvivorCatalog.FindSurvivorDefFromBody(playerController.master.bodyPrefab);
                    if (tempsurv && tempsurv.mainEndingEscapeFailureFlavorToken != null)
                    {
                        string token = "<style=cIsVoid>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString(tempsurv.mainEndingEscapeFailureFlavorToken) + " <sprite name=\"CloudRight\" tint=1></style>";
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                    }
                    else
                    {
                        string token = "<style=cIsVoid>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString("GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR") + " <sprite name=\"CloudRight\" tint=1></style>";
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                    }
                }
            };



            On.RoR2.OutroCutsceneController.OnEnable += (orig, self) =>
            {
                orig(self);
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/CreditsPanel.prefab").WaitForCompletion().transform.GetChild(4).GetComponent<MusicTrackOverride>().track = CreditsTrack;
                foreach (var playerController in PlayerCharacterMasterController.instances)
                {
                    var tempsurv = SurvivorCatalog.FindSurvivorDefFromBody(playerController.master.bodyPrefab);

                    if (playerController.master.lostBodyToDeath)
                    {
                        if (tempsurv && tempsurv.mainEndingEscapeFailureFlavorToken != null)
                        {
                            string token = "<style=cDeath><sprite name=\"Skull\" tint=1> " + Language.GetString(tempsurv.mainEndingEscapeFailureFlavorToken) + " <sprite name=\"Skull\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                        }
                        else
                        {
                            string token = "<style=cDeath><sprite name=\"Skull\" tint=1> " + Language.GetString("GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR") + " <sprite name=\"Skull\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });

                        }
                    }
                    else
                    {
                        if (tempsurv && tempsurv.outroFlavorToken != null)
                        {
                            string token2 = "<style=cHumanObjective>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString(tempsurv.outroFlavorToken) + " <sprite name=\"CloudRight\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token2 });
                        }
                        else
                        {
                            string token2 = "<style=cHumanObjective>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString("GENERIC_OUTRO_FLAVOR") + " <sprite name=\"CloudRight\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token2 });
                        }
                    }
                }
            };










            RoR2.CharacterAI.AISkillDriver[] skilllist;
            skilllist = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/EquipmentDroneMaster").GetComponents<RoR2.CharacterAI.AISkillDriver>();
            for (var i = 0; i < skilllist.Length; i++)
            {
                if (skilllist[i].customName.Contains("IdleNearLeaderWhenNoEnemies") || skilllist[i].customName.Contains("ChaseDownRandomEnemiesIfLeaderIsDead") || skilllist[i].customName.Contains("FireLongRange") || skilllist[i].customName.Contains("FireShotgun") || skilllist[i].customName.Contains("BackUpIfClose"))
                {
                    skilllist[i].shouldFireEquipment = true;
                }
            }




            //R2API.ContentAddition.AddProjectile(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavSackProjectile"));

            On.EntityStates.GlobalSkills.LunarNeedle.FireLunarNeedle.OnEnter += (orig, self) =>
            {

                self.animationLayerName = "Body";
                self.animationStateName = "no";
                self.playbackRateParam = "no";

                orig(self);

            };

            /*
            On.EntityStates.UrchinTurret.SpawnState.ctor += (orig, self) =>
            {
                orig(self);
                if (!self.outer.gameObject) { return; }
                RoR2.Util.PlaySound("Play_elite_antiHeal_urchin_spawn", self.outer.gameObject);
            };
            */

            On.EntityStates.ScavMonster.Sit.OnEnter += (orig, self) =>
            {
                orig(self);
                if (!self.outer.gameObject) { return; }
                RoR2.Util.PlaySound(EntityStates.ScavMonster.Sit.soundString, self.outer.gameObject);
            };
            /*
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += (orig, self) =>
            {
                orig(self);
                RoR2.Util.PlaySound("Play_item_lunar_specialReplace_apply", self.outer.gameObject);
                RoR2.Util.PlaySound("Play_item_lunar_specialReplace_apply", self.outer.gameObject);
            };
            */



            On.RoR2.CharacterMaster.PlayExtraLifeSFX += (orig, self) =>
            {
                orig(self);

                GameObject bodyInstanceObject = self.GetBodyObject();
                if (bodyInstanceObject)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Util.PlaySound("Play_item_proc_extraLife", bodyInstanceObject);
                    }
                }
            };

            if (DummyModelViewer.Value == true)
            {
                ModelViewer();
                GameModeCatalog.availability.CallWhenAvailable(ModelViewer);
            }
        }

        private void FireMegaFireball_OnEnter(On.EntityStates.LemurianBruiserMonster.FireMegaFireball.orig_OnEnter orig, EntityStates.LemurianBruiserMonster.FireMegaFireball self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                CharacterBody tempbod = self.outer.gameObject.GetComponent<CharacterBody>();
                if (tempbod && tempbod.master.name.StartsWith("LemurianBruiserIceMaster") || tempbod && tempbod.master.name.StartsWith("LemurianBruiserFireMaster"))
                {
                    tempbod.ClearTimedBuffs(RoR2Content.Buffs.ElementalRingsCooldown);
                    tempbod.baseDamage = 8; tempbod.levelDamage = 1.6f;
                    EntityStates.LemurianBruiserMonster.FireMegaFireball.damageCoefficient = 4;
                }
                else
                {
                    EntityStates.LemurianBruiserMonster.FireMegaFireball.damageCoefficient = 2;
                }
            }
        }

        private static List<ItemCount> getAllCurrentItems(Inventory inv)
        {
            List<ItemCount> list = new List<ItemCount>();
            foreach (ItemIndex itemIndex in inv.itemAcquisitionOrder)
            {
                ItemCount itemCount = new ItemCount(itemIndex, inv.GetItemCount(itemIndex));
                list.Add(itemCount);
            }
            return list;
        }


        private void CharacterMaster_TryCloverVoidUpgrades(On.RoR2.CharacterMaster.orig_TryCloverVoidUpgrades orig, CharacterMaster self)
        {

            Inventory inventory = self.inventory;
            if (inventory)
            {
                List<ItemCount> listBefore = new List<ItemCount>();
                List<ItemCount> listAfter = new List<ItemCount>();




                listBefore = getAllCurrentItems(inventory);

                orig(self);

                listAfter = getAllCurrentItems(inventory);

                if (listBefore.Count >= 0)
                {
                    List<ItemCount> difference = new List<ItemCount>();
                    foreach (ItemCount beforeItem in listBefore)
                    {
                        bool itemAppears = false;
                        foreach (ItemCount afterItem in listAfter)
                        {
                            if (beforeItem.itemIndex == afterItem.itemIndex)
                            {

                                itemAppears = true;
                                if (beforeItem.count > afterItem.count)
                                {
                                    ItemCount differenceItem = new ItemCount(beforeItem.itemIndex, beforeItem.count - afterItem.count);
                                    difference.Add(differenceItem);
                                }
                                break;
                            }
                        }
                        if (!itemAppears)
                        {
                            ItemCount differenceItem = new ItemCount(beforeItem.itemIndex, beforeItem.count);
                            difference.Add(differenceItem);
                        }
                    }

                }


                //Chat.AddMessage(string.Format(Language.GetString("MUSHROOM_VOID_UPGRADE"), self.netId, text, PingIndicator.sharedStringBuilder.ToString()));




            }
        }

        private void RunArtifactManager_onArtifactDisabledGlobal(RunArtifactManager runArtifactManager, ArtifactDef artifactDef)
        {
            if (artifactDef == RoR2Content.Artifacts.enigmaArtifactDef)
            {

            }
        }

        private void RunArtifactManager_onArtifactEnabledGlobal(RunArtifactManager runArtifactManager, ArtifactDef artifactDef)
        {
            if (artifactDef == RoR2Content.Artifacts.enigmaArtifactDef)
            {

            }
        }

        private void PickupPickerController_OnDisplayBegin(On.RoR2.PickupPickerController.orig_OnDisplayBegin orig, PickupPickerController self, NetworkUIPromptController networkUIPromptController, LocalUser localUser, CameraRigController cameraRigController)
        {
            orig(self, networkUIPromptController, localUser, cameraRigController);

            RoR2.UI.PickupPickerPanel temppannel = self.panelInstanceController;
            for (int i = 1; i < temppannel.buttonContainer.childCount; i++)
            {
                var temp = temppannel.buttonContainer.GetChild(i).GetComponent<RoR2.UI.TooltipProvider>();
                //Debug.LogWarning(temp);
                if (temp)
                {
                    if (temp.titleToken.StartsWith("EQUIPMENT_"))
                    {
                        if (temp.titleColor.r == 1 && temp.titleColor.g > 0.9f)
                        {
                            temp.titleColor = FakeYellowEquip;
                        }
                        else if (temp.titleColor.b == 1)
                        {
                            temp.titleColor = FakeBlueEquip;
                        }
                    }
                }
            }
        }


        public static IEnumerator DelayedChatMessage(string chatMessage)
        {
            yield return new WaitForSeconds(1f);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage
            {
                baseToken = chatMessage
            });
            yield break;
        }
        public static IEnumerator DelayedChatMessageNonGlobal(string chatMessage, NetworkConnection networkConnection)
        {
            yield return new WaitForSeconds(0.5f);
            Chat.SimpleChatMessage simpleChatMessage = new Chat.SimpleChatMessage
            {
                baseToken = chatMessage
            };

            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.StartMessage(59);
            networkWriter.Write(simpleChatMessage.GetTypeIndex());
            networkWriter.Write(simpleChatMessage);
            networkWriter.FinishMessage();
            if (networkConnection == null)
            {
                yield break;
            }
            networkConnection.SendWriter(networkWriter, RoR2.Networking.QosChannelIndex.chat.intVal);
            yield break;
        }


        public static void LanguageChanger()
        {


            if (MessagesBenthicEgo.Value == true)
            {
                On.RoR2.CharacterMaster.TryCloverVoidUpgrades += (orig, self) =>
                {
                    if (self.inventory.GetItemCount(DLC1Content.Items.CloverVoid) > 0)
                    {
                        On.RoR2.Inventory.RemoveItem_ItemIndex_int += VoidCloverListener;
                        orig(self);
                        On.RoR2.Inventory.RemoveItem_ItemIndex_int -= VoidCloverListener;
                        return;
                    }
                    orig(self);
                };

                On.RoR2.CharacterMasterNotificationQueue.PushItemTransformNotification += (orig, characterMaster, oldIndex, newIndex, transformationType) =>
                {
                    orig(characterMaster, oldIndex, newIndex, transformationType);

                    //Debug.LogWarning(transformationType);

                    if (transformationType == CharacterMasterNotificationQueue.TransformationType.LunarSun)
                    {
                        string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                        string name = Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken);
                        int count = characterMaster.inventory.GetItemCount(DLC1Content.Items.LunarSun);
                        string token = "<style=cEvent><color=#458CFF>Egocentrism</color>(" + count + ") assimilated <color=#" + hex + ">" + name + "</color></style>";
                        Chat.SimpleChatMessage simpleChatMessage = new Chat.SimpleChatMessage
                        {
                            baseToken = token
                        };

                        NetworkConnection clientAuthorityOwner = characterMaster.GetComponent<NetworkIdentity>().clientAuthorityOwner;
                        NetworkWriter networkWriter = new NetworkWriter();
                        networkWriter.StartMessage(59);
                        networkWriter.Write(simpleChatMessage.GetTypeIndex());
                        networkWriter.Write(simpleChatMessage);
                        networkWriter.FinishMessage();
                        if (clientAuthorityOwner == null)
                        {
                            return;
                        }
                        clientAuthorityOwner.SendWriter(networkWriter, RoR2.Networking.QosChannelIndex.chat.intVal);

                    }
                    else if (transformationType == CharacterMasterNotificationQueue.TransformationType.CloverVoid)
                    {
                        string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                        string hex2 = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(newIndex).pickupDef.baseColor);
                        string name = Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken) + "</color>";
                        string name2 = Language.GetString(ItemCatalog.GetItemDef(newIndex).nameToken) + "</color>";
                        int newitemcount = characterMaster.inventory.GetItemCount(newIndex);
                        //string count1 = "";
                        //string count2 = "";
                        /*
                        if (!name2.EndsWith("s"))
                        {
                            name2 += "s";
                        }
                        */
                        if (VoidCloverInventory == characterMaster.inventory && VoidCloverCount > 1)
                        {
                            name += "(" + VoidCloverCount + ")";
                        }
                        if (newitemcount > 1)
                        {
                            name2 += "(" + newitemcount + ")";
                        }

                        string token = "<style=cEvent><style=cIsVoid>Benthic Bloom</style> upgraded <color=#" + hex + ">" + name + " into <color=#" + hex2 + ">" + name2 + "</style>";

                        NetworkConnection clientAuthorityOwner = characterMaster.GetComponent<NetworkIdentity>().clientAuthorityOwner;
                        characterMaster.StartCoroutine(DelayedChatMessageNonGlobal(token, clientAuthorityOwner));

                    }
                };
            }

            //Additional Key Words
            LanguageAPI.Add("KEYWORD_SLOWING", "<style=cKeywordName>Slowing</style><style=cSub>Apply a slowing debuff reducing enemy <style=cIsUtility>movement speed</style> by <style=cIsUtility>50%</style>.</style>", "en");

            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Huntress/HuntressBodyBlink.asset").WaitForCompletion().keywordTokens = new string[] { "KEYWORD_AGILE" };
            //Arrow Rain Slows

            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Merc/MercBodyFocusedAssault.asset").WaitForCompletion().keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD_EXPOSE" };


            //



            LanguageAPI.Add("FROG_NAME", "Glass frog", "en");
            LanguageAPI.Add("FROG_CONTEXT", "Pet the glass frog", "en");
            LanguageAPI.Add("PET_FROG", "{0} pet the glass frog.", "en");
            LanguageAPI.Add("PET_FROG_2P", "You pet the glass frog.", "en");


            LanguageAPI.Add("LUNARSUN_TRANSFORM", "Egocentrism assassimilated {0}'s {0}", "en");
            LanguageAPI.Add("LUNARSUN_TRANSFORM_2P", "Egocentrism assassimilated your {0}", "en");
            LanguageAPI.Add("BAZAAR_SEER_SNOWYFOREST", "<style=cWorldEvent>You dream of campfires and ice.</style>", "en");



            LanguageAPI.Add("VULTUREEGG_BODY_NAME", "Vulture Egg", "en");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/shipgraveyard/VultureEggBody.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "VULTUREEGG_BODY_NAME";


            LanguageAPI.Add("ACIDLARVA_BODY_NAME", "Acid Larva", "en");

            LanguageAPI.Add("LUNARWISP_BODY_NAME_LONG", "Lunar Chimera Wisp", "en");
            LanguageAPI.Add("LUNARGOLEM_BODY_NAME_LONG", "Lunar Chimera Golem", "en");
            LanguageAPI.Add("LUNAREXPLODER_BODY_NAME_LONG", "Lunar Chimera Exploder", "en");
            LanguageAPI.Add("LUNARWISP_BODY_NAME_LONG", "Mondchimäre Wisch", "de");
            LanguageAPI.Add("LUNARGOLEM_BODY_NAME_LONG", "Mondchimäre Golem", "de");
            LanguageAPI.Add("LUNAREXPLODER_BODY_NAME_LONG", "Mondchimäre Sprengkapsel", "de");

            LanguageAPI.Add("LUNARWISP_BODY_NAME_SHORT", "Lunar Wisp", "en");
            LanguageAPI.Add("LUNARGOLEM_BODY_NAME_SHORT", "Lunar Golem", "en");
            LanguageAPI.Add("LUNAREXPLODER_BODY_NAME_SHORT", "Lunar Exploder", "en");
            LanguageAPI.Add("LUNARWISP_BODY_NAME_SHORT", "Mond Wisch", "de");
            LanguageAPI.Add("LUNARGOLEM_BODY_NAME_SHORT", "Mond Golem", "de");
            LanguageAPI.Add("LUNAREXPLODER_BODY_NAME_SHORT", "Mond Sprengkapsel", "de");


            if (LunarChimeraNameChange.Value == "Long")
            {
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarWispBody").GetComponent<CharacterBody>().baseNameToken = "LUNARWISP_BODY_NAME_LONG";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarGolemBody").GetComponent<CharacterBody>().baseNameToken = "LUNARGOLEM_BODY_NAME_LONG";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody").GetComponent<CharacterBody>().baseNameToken = "LUNAREXPLODER_BODY_NAME_LONG";
            }
            else if (LunarChimeraNameChange.Value == "Short")
            {
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarWispBody").GetComponent<CharacterBody>().baseNameToken = "LUNARWISP_BODY_NAME_SHORT";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarGolemBody").GetComponent<CharacterBody>().baseNameToken = "LUNARGOLEM_BODY_NAME_SHORT";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody").GetComponent<CharacterBody>().baseNameToken = "LUNAREXPLODER_BODY_NAME_SHORT";

            }


            if (LimboEndingMildChange.Value == true)
            {
                //RoR2.LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/EscapeSequenceFailed").icon = RoR2.LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/MainEnding").icon;

                GameEndingDef LimboEnding = RoR2.LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/LimboEnding");
                LanguageAPI.Add("GAME_RESULT_LIMBOWIN", "At Peace..", "en");
                LanguageAPI.Add("GAME_RESULT_LIMBOWIN", "In Frieden..", "de");
                LanguageAPI.Add("GAME_RESULT_LIMBOWIN", "En Paix..", "FR");
                LanguageAPI.Add("GAME_RESULT_LIMBOWIN", "In Pace..", "IT");
                LanguageAPI.Add("GAME_RESULT_LIMBOWIN", "En Paz..", "es-419");
                LimboEnding.endingTextToken = "GAME_RESULT_LIMBOWIN";
                //LimboEnding.backgroundColor = new Color32(230, 230, 230, 179);
                //LimboEnding.foregroundColor = new Color32(255, 255, 255, 128);

                LimboEnding.backgroundColor = new Color32(227, 236, 252, 215);
                LimboEnding.foregroundColor = new Color32(232, 239, 255, 190);



                GameEndingDef VoidEnding = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/DLC1/GameModes/VoidEnding.asset").WaitForCompletion();
                LanguageAPI.Add("GAME_RESULT_VOIDWIN", "Deeper, Deeper, yet Deeper..", "en");
                VoidEnding.endingTextToken = "GAME_RESULT_VOIDWIN";
                VoidEnding.showCredits = true;


                GameEndingDef MainEnding = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/MainEnding.asset").WaitForCompletion();
                GameEndingDef EscapeSequenceFailed = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/EscapeSequenceFailed.asset").WaitForCompletion();
                LanguageAPI.Add("GAME_RESULT_ESCAPEFAILED", "Failed Escape..", "en");
                EscapeSequenceFailed.endingTextToken = "GAME_RESULT_ESCAPEFAILED";
                EscapeSequenceFailed.icon = MainEnding.icon;


            }


            Texture GenericPlanetDeath = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").GetComponent<CharacterBody>().portraitIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ExplosivePotDestructibleBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FusionCellDestructibleBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TimeCrystalBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AltarSkeletonBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SulfurPodBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody").GetComponent<CharacterBody>().subtitleNameToken = "LUNAREXPLODER_BODY_SUBTITLE";
            LanguageAPI.Add("IMP_TINY_BODY_NAME", "Tiny Imp", "en");
            LanguageAPI.Add("SNIPER_BODY_NAME", "Sniper", "en");
            //LanguageAPI.Add("MAJORCONSTRUCT_BODY_NAME", "Sigma Construct", "en");
            LanguageAPI.Add("ARCHWISP_BODY_NAME", "Archaic Wisp", "en");
            LanguageAPI.Add("ANCIENTWISP_BODY_NAME", "Ancient Wisp", "en");

            LanguageAPI.Add("MONSTER_PICKUP", "<style=cWorldEvent>{0} scavenged for {1}{2}</color>", "en");
            LanguageAPI.Add("MONSTER_PICKUP_2P", "<style=cWorldEvent>You scavenged for {1}{2}</color>", "en");


            LanguageAPI.Add("DIFFICULTY_EASY_DESCRIPTION", "Reduces difficulty for players new to the game. Weeping and gnashing is replaced by laughter and tickles.<style=cStack>\n\n>Player Health Regeneration: <style=cIsHealing>+50%</style> \n>Player Damage Reduction: <style=cIsHealing>+40%</style> \n>Difficulty Scaling: <style=cIsHealing>-50%</style></style>", "en");


            LanguageAPI.Add("STATNAME_TOTALLUNARPURCHASES", "Lunar Purchases", "en");
            LanguageAPI.Add("STATNAME_HIGHESTLUNARPURCHASES", "Most Lunar Purchases", "en");
            LanguageAPI.Add("STATNAME_HIGHESTBLOODPURCHASES", "Most Blood Purchases", "en");

            LanguageAPI.Add("PLAYER_DEATH_QUOTE_0_2P", "You are dead. Not big surprise.", "en");
            LanguageAPI.Add("PLAYER_DEATH_QUOTE_0", "{0} is dead. Not big surprise.", "en");

            LanguageAPI.Add("PLAYER_DEATH_QUOTE_33_2P", "You are not OK from this encounter.", "en");
            LanguageAPI.Add("PLAYER_DEATH_QUOTE_33", "{0} is not OK from this encounter.", "en");


            /*
            On.RoR2.GlobalEventManager.OnEnable += (orig, self) =>
            {
                orig(self);
                Debug.LogWarning(GlobalEventManager.standardDeathQuoteTokens.Length);
                GlobalEventManager.standardDeathQuoteTokens = GlobalEventManager.standardDeathQuoteTokens.Add("PLAYER_DEATH_QUOTE_38");
                GlobalEventManager.standardDeathQuoteTokens =  standardDeathQuoteTokens = (from i in Enumerable.Range(0, 38)
                select "PLAYER_DEATH_QUOTE_" + TextSerialization.ToStringInvariant(i)).ToArray<string>();

            };
            */


            LanguageAPI.Add("MAP_ARENA_TITLE", "Void Fields", "en");
            LanguageAPI.Add("MAP_VOIDSTAGE_TITLE", "Void Locus", "en");
            LanguageAPI.Add("MAP_VOIDRAID_TITLE", "The Planetarium", "en");


            LanguageAPI.Add("MAP_BAZAAR_SUBTITLE", "", "en");
            LanguageAPI.Add("MAP_GOLDSHORES_SUBTITLE", "Beautiful Cage", "en");
            LanguageAPI.Add("MAP_MYSTERYSPACE_SUBTITLE", "", "en");
            LanguageAPI.Add("MAP_LIMBO_SUBTITLE", "", "en");
            LanguageAPI.Add("MAP_ARTIFACTWORLD_SUBTITLE", "Sacred Treasures", "en");

            //Objectives
            LanguageAPI.Add("OBJECTIVE_GOLDSHORES_ACTIVATE_BEACONS", "Rebuild the <color=#FFE880>Halcyon Beacons</color> ({0}/{1})", "en");
            LanguageAPI.Add("OBJECTIVE_CLEAR_ARENA", "Activate all <style=cIsVoid>Cell Vents</style> ({0}/{1})", "en");
            LanguageAPI.Add("OBJECTIVE_ARENA_CHARGE_CELL", "Breach the <style=cIsVoid>Cell</style> ({0}%)", "en");

            LanguageAPI.Add("INFINITETOWER_SUDDEN_DEATH", "<style=cWorldEvent>[WARNING] The Focus begins to falter..</style>", "en");
            LanguageAPI.Add("INFINITETOWER_OBJECTIVE_AWAITINGACTIVATION", "Activate the <style=cIsVoid>Focus</style>", "en");
            LanguageAPI.Add("INFINITETOWER_OBJECTIVE_TRAVEL", "Follow the <style=cIsVoid>Focus</style>", "en");
            LanguageAPI.Add("INFINITETOWER_OBJECTIVE_PORTAL", "Advance through the <style=cIsVoid>Infinite Portal</style>", "en");

            LanguageAPI.Add("INFINITETOWER_TIME_HIGHEST_WAVE_NORMAL", "> Highest Wave (Rainstorm): <style=cIsHealing>{0}</style>", "en");

            //Add Focus stuff and the focus shrinks stuff 


            //Updated Interactable Names
            LanguageAPI.Add("DRONE_MEGA_CONTEXT", "Repair TC-280", "en");
            LanguageAPI.Add("LOCKEDTREEBOT_CONTEXT", "Repair the survivor", "en");
            LanguageAPI.Add("LOCKEDMAGE_NAME", "Frozen Survivor", "en");

            LanguageAPI.Add("CHEST2_CONTEXT", "Open Large chest", "en");

            LanguageAPI.Add("MULTISHOP_LARGE_TERMINAL_NAME", "Large Multishop Terminal", "en");
            LanguageAPI.Add("MULTISHOP_LARGE_TERMINAL_CONTEXT", "Open terminal", "en");
            LanguageAPI.Add("MULTISHOP_EQUIPMENT_TERMINAL_NAME", "Equipment Shop Terminal", "en");
            LanguageAPI.Add("MULTISHOP_EQUIPMENT_TERMINAL_CONTEXT", "Open terminal", "en");
            LanguageAPI.Add("DUPLICATOR_LARGE_NAME", "Large 3D Printer", "en");
            LanguageAPI.Add("DUPLICATOR_LARGE_CONTEXT", "Use Large 3D Printer", "en");


            LanguageAPI.Add("FREECHEST_TERMINAL_NAME", "Shipping Terminal", "en");
            LanguageAPI.Add("FREECHEST_TERMINAL_CONTEXT", "Accept delivery.", "en");


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab").WaitForCompletion().GetComponent<RoR2.PurchaseInteraction>().displayNameToken = "FREECHEST_TERMINAL_NAME";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab").WaitForCompletion().GetComponent<RoR2.PurchaseInteraction>().contextToken = "FREECHEST_TERMINAL_CONTEXT";


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopLargeTerminal").GetComponent<RoR2.PurchaseInteraction>().displayNameToken = "MULTISHOP_LARGE_TERMINAL_NAME";
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopLargeTerminal").GetComponent<RoR2.PurchaseInteraction>().contextToken = "MULTISHOP_LARGE_TERMINAL_CONTEXT";
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopEquipmentTerminal").GetComponent<RoR2.PurchaseInteraction>().displayNameToken = "MULTISHOP_EQUIPMENT_TERMINAL_NAME";
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopEquipmentTerminal").GetComponent<RoR2.PurchaseInteraction>().contextToken = "MULTISHOP_EQUIPMENT_TERMINAL_CONTEXT";

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").GetComponent<RoR2.PurchaseInteraction>().displayNameToken = "DUPLICATOR_LARGE_NAME";
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").GetComponent<RoR2.PurchaseInteraction>().contextToken = "DUPLICATOR_LARGE_CONTEXT";
            //


            //Skills
            LanguageAPI.Add("RAILGUNNER_SNIPE_CRYO_DESCRIPTION", "<style=cIsUtility>Freezing</style>.  Launch a super-cooled projectile for <style=cIsDamage>2000% damage</style>.");


            LanguageAPI.Add("HUNTRESS_UTILITY_DESCRIPTION", "<style=cIsUtility>Agile</style>. <style=cIsUtility>Disappear</style> and <style=cIsUtility>teleport</style> forward.");
            LanguageAPI.Add("HUNTRESS_SPECIAL_DESCRIPTION", "<style=cIsUtility>Teleport</style> into the sky. Target an area to rain arrows, <style=cIsUtility>slowing</style> all enemies and dealing <style=cIsDamage>330% damage per second</style>.");
            LanguageAPI.Add("BANDIT2_SPECIAL_ALT_DESCRIPTION", "<style=cIsDamage>Slayer</style>. Fire a revolver shot for <style=cIsDamage>600% damage</style>. Kills grant <style=cIsDamage>stacking tokens</style> for a flat <style=cIsDamage>60%</style> more damage on Desperado.");

            LanguageAPI.Add("ENGI_SPECIAL_DESCRIPTION", "Place a turret that <style=cIsUtility>inherits all your items.</style> Fires a cannon for <style=cIsDamage>70% damage</style>. Can place up to 2.");

            LanguageAPI.Add("TOOLBOT_SPECIAL_ALT_DESCRIPTION", "Enter a heavy stance, equipping both your <style=cIsDamage>primary attacks</style> at once. Gain <style=cIsUtility>100 armor</style>, but lose <style=cIsHealth>-50% movement speed</style>.");

            LanguageAPI.Add("TREEBOT_SECONDARY_DESCRIPTION", "<style=cIsHealth>13% HP</style>. Launch a mortar into the sky for <style=cIsDamage>450% damage</style>.");
            LanguageAPI.Add("TREEBOT_UTILITY_ALT1_DESCRIPTION", "<style=cIsHealth>17% HP</style>. Fire a <style=cIsUtility>Sonic Boom</style> that <style=cIsDamage>damages</style> enemies for <style=cIsDamage>550% damage</style>. <style=cIsHealing>Heals for every target hit</style>.");

            //LanguageAPI.Add("MAGE_SPECIAL_FIRE_DESCRIPTION", "Burn all enemies in front of you for <style=cIsDamage>2000% damage</style>.");

            LanguageAPI.Add("LOADER_UTILITY_ALT1_DESCRIPTION", "<style=cIsUtility>Heavy</style>. Charge up a <style=cIsUtility>single-target</style> punch for <style=cIsDamage>2100% damage</style> that <style=cIsDamage>shocks</style> enemies in a cone for <style=cIsDamage>900% damage</style>.");


            //LanguageAPI.Add("CAPTAIN_UTILITY_ALT1_DESCRIPTION", "<style=cIsDamage>Stunning</style>. Request a <style=cIsDamage>kinetic strike</style> from the <style=cIsDamage>UES Safe Travels</style>. After <style=cIsUtility>20 seconds</style>, it deals <style=cIsDamage>40,000% damage</style> to enemies and <style=cIsDamage>20,000% damage</style> to ALL ALLIES..");

            LanguageAPI.Add("KEYWORD_WEAK", "<style=cKeywordName>Weaken</style><style=cSub>Reduce movement speed and damage by <style=cIsDamage>40%</style>. Reduce armor by <style=cIsDamage>30</style>.</style>");

            LanguageAPI.Add("SKILL_LUNAR_SECONDARY_REPLACEMENT_DESCRIPTION", "Charge up a ball of blades that repeatedly deals <style=cIsDamage>875%</style> damage. After a delay, explode and <style=cIsDamage>root</style> all enemies for <style=cIsDamage>700%</style>damage.");
            LanguageAPI.Add("SKILL_LUNAR_UTILITY_REPLACEMENT_DESCRIPTION", "Fade away, becoming <style=cIsUtility>intangible</style> and <style=cIsUtility>gaining movement speed</style>. <style=cIsHealing>Heal</style> for <style=cIsHealing>18% of your maximum health</style>.");

            LanguageAPI.Add("ITEM_LUNARSECONDARYREPLACEMENT_DESC", "<style=cIsUtility>Replace your Secondary Skill </style> with <style=cIsUtility>Slicing Maelstrom</style>.  \n\nCharge up a projectile that deals <style=cIsDamage>875% damage per second</style> to nearby enemies, exploding after <style=cIsUtility>3</style> seconds to deal <style=cIsDamage>700% damage</style> and <style=cIsDamage>root</style> enemies for <style=cIsUtility>3</style> <style=cStack>(+3 per stack)</style> seconds. Recharges after 5 <style=cStack>(+5 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_LUNARUTILITYREPLACEMENT_DESC", "<style=cIsUtility>Replace your Utility Skill</style> with <style=cIsUtility>Shadowfade</style>. \n\nFade away, becoming <style=cIsUtility>intangible</style> and gaining <style=cIsUtility>+30% movement speed</style>. <style=cIsHealing>Heal</style> for <style=cIsHealing>18% <style=cStack>(+18% per stack)</style> of your maximum health</style>. Lasts 3 <style=cStack>(+3 per stack)</style> seconds.");



            //Corrected Items and Equipment
            LanguageAPI.Add("ITEM_ANCESTRALINCUBATOR_DESC", "<style=cIsDamage>7%</style> chance <style=cStack>(+1% per stack)</style> on kill to <style=cIsUtility>summon an Ancestral Pod</style> that distracts enemies. \nOnce it fully grows, it will hatch into an allied <style=cIsDamage>Parent</style> with <style=cIsHealing>200% health</style> <style=cStack>(+100% per stack)</style> and with <style=cIsDamage>400% damage</style>. <style=cIsDamage>Parents</style> limited to 1 <style=cStack>(+1 per stack)</style>", "en");

            LanguageAPI.Add("ITEM_SHINYPEARL_DESC", "Increases <style=cIsDamage>damage</style>, <style=cIsDamage>attack speed</style>, <style=cIsDamage>critical strike chance</style>, <style=cIsHealing>maximum health</style>, <style=cIsHealing>base armor</style>, <style=cIsUtility>movement speed</style> by <style=cIsDamage>1<style=cIsHealing>0<style=cIsUtility>%<style=cStack> (+10% per stack)</style></style></style></style> and <style=cIsHealing>base health regeneration</style> by <style=cIsHealing>+0.1 hp/s</style><style=cStack> (+0.1 hp/s)</style>", "en");


            LanguageAPI.Add("ITEM_SCRAPWHITE_PICKUP", "Does nothing. Prioritized when used with <color=#FAFAFA>3D Printers</color>.", "en");
            LanguageAPI.Add("ITEM_SCRAPGREEN_PICKUP", "Does nothing. Prioritized when used with <style=cIsHealing>Large 3D Printers</style>.", "en");
            LanguageAPI.Add("ITEM_SCRAPRED_PICKUP", "Does nothing. Prioritized when used with <style=cIsHealth>Mili-Tech 3D Printers</style>.", "en");
            LanguageAPI.Add("ITEM_SCRAPYELLOW_PICKUP", "Does nothing. Prioritized when used with <style=cIsTierBoss>Overgrown 3D Printers.</style>.</style>", "en");

            LanguageAPI.Add("ITEM_SCRAPWHITE_DESC", "Does nothing. Prioritized when used with <color=#FAFAFA>3D Printers</color> and Cauldrons.", "en");
            LanguageAPI.Add("ITEM_SCRAPGREEN_DESC", "Does nothing. Prioritized when used with <style=cIsHealing>Large 3D Printers</style> and Cauldrons.", "en");
            LanguageAPI.Add("ITEM_SCRAPRED_DESC", "Does nothing. Prioritized when used with <style=cIsHealth>Mili-Tech 3D Printers</style> and Cauldrons.", "en");
            LanguageAPI.Add("ITEM_SCRAPYELLOW_DESC", "Does nothing. Prioritized when used with <style=cIsTierBoss>Overgrown 3D Printers</style>.", "en");



            LanguageAPI.Add("EQUIPMENT_BOSSHUNTER_PICKUP", "Execute a large monster and claim its <style=cIsTierBoss>trophy</style>. Consumed on use.", "en");
            LanguageAPI.Add("EQUIPMENT_BOSSHUNTER_DESC", "<style=cIsDamage>Execute</style> any enemy capable of spawning a <style=cIsTierBoss>unique reward</style>, and it will drop that <style=cIsDamage>item</style>. Equipment is <style=cIsUtility>consumed</style> on use.", "en");


            LanguageAPI.Add("ITEM_MISSILEVOID_DESC", "Gain a <style=cIsHealing>shield</style> equal to <style=cIsHealing>10%</style> of your maximum health. While you have a <style=cIsHealing>shield</style>, hitting an enemy fires a missile that deals <style=cIsDamage>40%</style> <style=cStack>(+40% per stack)</style> TOTAL damage. <style=cIsVoid>Corrupts all AtG Missile Mk. 1s</style>.", "en");
            LanguageAPI.Add("ITEM_CLOVERVOID_DESC", "<style=cIsUtility>Upgrades 3</style> <style=cStack>(+3 per stack)</style> random stacks of items to items of the next <style=cIsUtility>higher rarity</style> at the <style=cIsUtility>start of each stage</style>. <style=cIsVoid>Corrupts all 57 Leaf Clovers</style>.", "en");
            LanguageAPI.Add("ITEM_EQUIPMENTMAGAZINEVOID_PICKUP", "Add an extra charge of your Special skill. Reduce Special skill cooldown. <style=cIsVoid>Corrupts all Fuel Cells</style>.", "en");
            LanguageAPI.Add("ITEM_EQUIPMENTMAGAZINEVOID_DESC", "Add <style=cIsUtility>+1</style> <style=cStack>(+1 per stack)</style> charge of your <style=cIsUtility>Special skill</style>. <style=cIsUtility>Reduces Special skill cooldown</style> by <style=cIsUtility>33%</style>. <style=cIsVoid>Corrupts all Fuel Cells</style>.", "en");
            LanguageAPI.Add("ITEM_ELEMENTALRINGVOID_DESC", "Hits that deal <style=cIsDamage>more than 400% damage</style> also fire a black hole that <style=cIsUtility>draws enemies within 15m into its center</style>. Lasts <style=cIsUtility>5</style> seconds before collapsing, dealing <style=cIsDamage>100%</style> <style=cStack>(+100% per stack)</style> TOTAL damage. Recharges every <style=cIsUtility>20</style> seconds. <style=cIsVoid>Corrupts all Runald's and Kjaro's Bands.</style>.", "en");
            LanguageAPI.Add("ITEM_VOIDMEGACRABITEM_DESC", "Every <style=cIsUtility>60</style><style=cStack>(-50% per stack)</style> seconds, gain a random <style=cIsVoid>Void</style> ally. Can have up to <style=cIsUtility>1</style><style=cStack>(+1 per stack)</style> allies at a time. <style=cIsVoid>Corrupts all </style><style=cIsTierBoss>yellow items</style><style=cIsVoid>.</style>", "en");
            //LanguageAPI.Add("EQUIPMENT_GUMMYCLONE_DESC", "Spawn a gummy clone that has <style=cIsDamage>170% damage</style> and <style=cIsHealing>170% health</style>. Expires in <style=cIsUtility>30</style> seconds.", "en");
            LanguageAPI.Add("EQUIPMENT_BOSSHUNTERCONSUMED_DESC", "Exclaim an Ahoy!", "en");
            LanguageAPI.Add("ITEM_TREASURECACHEVOID_DESC", "A <style=cIsUtility>hidden cache</style> containing an item (42%/<style=cIsHealing>42%</style>/<style=cIsHealth>16%</style>) will appear in a random location <style=cIsUtility>on each stage</style>. Opening the cache <style=cIsUtility>consumes</style> this item. <style=cIsVoid>Corrupts all Rusted Keys</style>.", "en");
            LanguageAPI.Add("text", "text", "en");



            //LanguageAPI.Add("ITEM_WARCRYONMULTIKILL_PICKUP", "Enter a frenzy after killing 4 enemies in quick succession.", "en");
            //LanguageAPI.Add("ITEM_WARCRYONMULTIKILL_DESC", "<style=cIsDamage>Killing 4 enemies</style> within <style=cIsDamage>1</style> second sends you into a <style=cIsDamage>frenzy</style> for <style=cIsDamage>6s</style> <style=cStack>(+4s per stack)</style>. Increases <style=cIsUtility>movement speed</style> by <style=cIsUtility>50%</style> and <style=cIsDamage>attack speed</style> by <style=cIsDamage>100%</style>.", "en");

            LanguageAPI.Add("ITEM_NOVAONLOWHEALTH_DESC", "Falling below <style=cIsHealth>25% health</style> causes you to explode, dealing <style=cIsDamage>6000% base damage</style>. Recharges every <style=cIsUtility>15 seconds</style> <style=cStack>(-33% per stack)</style>.");
            LanguageAPI.Add("ITEM_LIGHTNINGSTRIKEONHIT_DESC", "<style=cIsDamage>10%</style> chance on hit to down a lightning strike, dealing <style=cIsDamage>500%</style> <style=cStack>(+500% per stack)</style> TOTAL damage.");
            LanguageAPI.Add("ITEM_FIREBALLSONHIT_DESC", "<style=cIsDamage>10%</style> chance on hit to call forth <style=cIsDamage>3 magma balls</style> from an enemy, dealing <style=cIsDamage>300%</style> <style=cStack>(+300% per stack)</style> TOTAL damage and <style=cIsDamage>igniting</style> all enemies for an additional <style=cIsDamage>50%</style> TOTAL damage over time.");
            LanguageAPI.Add("ITEM_BEETLEGLAND_DESC", "<style=cIsUtility>Summon a Beetle Guard</style> with bonus <style=cIsDamage>300%</style> damage and <style=cIsHealing>100% health</style>. Can have up to <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style> Guards at a time.");


            LanguageAPI.Add("ITEM_BLEEDONHIT_DESC", "<style=cIsDamage>10%</style> <style=cStack>(+10% per stack)</style> chance to <style=cIsDamage>bleed</style> an enemy for <style=cIsDamage>240%</style> base damage. Inflicting <style=cIsDamage>bleed</style> refreshes all stacks.", "en");

            LanguageAPI.Add("BODY_MODIFIER_ECHO", "{0} Echo", "en");
            LanguageAPI.Add("ELITE_MODIFIER_SECRETSPEED", "Furry {0}", "en");


            LanguageAPI.Add("ITEM_FRAGILEDAMAGEBONUSCONSUMED_DESC", "...well, it's still right twice a day.", "en");
            LanguageAPI.Add("ITEM_HEALINGPOTIONCONSUMED_DESC", "An empty container for an Elixir. Does nothing.", "en");


            LanguageAPI.Add("ITEM_INTERSTELLARDESKPLANT_DESC", "On kill, plant a <style=cIsHealing>healing</style> fruit seed that grows into a plant after <style=cIsUtility>5</style> seconds. \n\nThe plant <style=cIsHealing>heals</style> for <style=cIsHealing>10%</style> of <style=cIsHealing>maximum health</style> every second to all allies within <style=cIsHealing>10m</style> <style=cStack>(+5.0m per stack)</style>. Lasts <style=cIsUtility>10</style> seconds.", "en");
            LanguageAPI.Add("ITEM_MUSHROOM_DESC", "After standing still for <style=cIsHealing>1</style> second, create a zone that <style=cIsHealing>heals</style> for <style=cIsHealing>4.5%</style> <style=cStack>(+2.25% per stack)</style> of your <style=cIsHealing>health</style> every second to all allies within <style=cIsHealing>3.5m</style> <style=cStack>(+1.5m per stack)</style>.", "en");
            LanguageAPI.Add("ITEM_BEHEMOTH_DESC", "All your <style=cIsDamage>attacks explode</style> in a <style=cIsDamage>4m </style><style=cStack>(+2.5m per stack)</style> radius for a bonus <style=cIsDamage>60%</style> TOTAL damage to nearby enemies.", "en");


            LanguageAPI.Add("ITEM_ATTACKSPEEDONCRIT_DESC", "Gain <style=cIsDamage>5% critical chance</style>. <style=cIsDamage>Critical strikes</style> increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>12%</style>. Maximum cap of <style=cIsDamage>36% <style=cStack>(+24% per stack)</style> attack speed</style>.", "en");
            LanguageAPI.Add("ITEM_BLEEDONHITANDEXPLODE_DESC", "Gain <style=cIsDamage>5% critical chance</style>. <style=cIsDamage>Critical Strikes bleed</style> enemies for <style=cIsDamage>240%</style> base damage. <style=cIsDamage>Bleeding</style> enemies <style=cIsDamage>explode</style> on death for <style=cIsDamage>400%</style> <style=cStack>(+400% per stack)</style> damage, plus an additional <style=cIsDamage>15%</style> <style=cStack>(+15% per stack)</style> of their maximum health.", "en");

            LanguageAPI.Add("EQUIPMENT_JETPACK_DESC", "Sprout wings and <style=cIsUtility>fly for 15 seconds</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_BFG_DESC", "Fires preon tendrils, zapping enemies within 35m for up to <style=cIsDamage>1200% damage/second</style>. On contact, detonate in an enormous 20m explosion for <style=cIsDamage>8000% damage</style>.");




            LanguageAPI.Add("EQUIPMENT_CRIPPLEWARD_DESC", "<color=#FF7F7F>ALL characters</color> within have their <style=cIsUtility>movement speed slowed by 100%</style> and have their <style=cIsDamage>armor reduced by 20</style>. Can place up to <style=cIsUtility>5</style>.", "en");

            LanguageAPI.Add("EQUIPMENT_DEATHPROJECTILE_DESC", "Throw a cursed doll out that <style=cIsDamage>triggers</style> any <style=cIsDamage>On-Kill</style> effects you have every <style=cIsUtility>1</style> second for <style=cIsUtility>8</style> seconds. Cannot throw out more than <style=cIsUtility>3</style> dolls at a time.", "en");


            //New Descriptions
            LanguageAPI.Add("ITEM_TONICAFFLICTION_DESC", "<color=#FF7F7F>Reduce ALL stats by -5% </color><style=cStack>(-5% per stack)</style><color=#FF7F7F> when not under the effects of Spinel Tonic</color>", "en");
            LanguageAPI.Add("EQUIPMENT_QUESTVOLATILEBATTERY_DESC", "Falling below <style=cIsHealth>50% health</style> causes this to explode in a large radius dealing <style=cIsDamage>300% the carriers maximum health as damage</style>. Consumed on usage.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXRED_DESC", "Attacks <style=cIsDamage>ignite</style> enemies dealing <style=cIsDamage>50%</style> TOTAL damage over time. Leave behind a <style=cIsDamage>fire trail</style> dealing damage on contact.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXBLUE_DESC", "All attacks leave a <style=cIsDamage>lightning orb</style> which explodes after <style=cIsUtility>1.5s</style> dealing <style=cIsDamage>50%</style> TOTAL damage. Convert <style=cIsHealing>50%</style> of your <style=cIsHealing>health</style> into <style=cIsHealing>shields</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXWHITE_DESC", "<style=cIsUtility>Slow</style> enemies on hit for <style=cIsUtility>-80% movement speed</style> for <style=cIsUtility>1.5s</style>. <style=cDeath>On death</style> leave an <style=cIsDamage>ice explosion</style> <style=cIsUtility>freezing</style> enemies for <style=cIsUtility>1.5s</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXPOISON_DESC", "Enemies hit will have their <style=cIsHealing>healing disabled</style> for <style=cIsUtility>8s</style>. Periodically shoot out 3 <style=cIsDamage>spike clusters</style> dealing <style=cIsDamage>100%</style> base damage. <style=cDeath>On death</style> leave a <style=cIsDamage>Malachite Urchin</style> lasting <style=cIsUtility>20s</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXHAUNTED_DESC", "<style=cIsUtility>Slow</style> enemies on hit for <style=cIsUtility>-80% movement speed</style> for <style=cIsUtility>3s</style>. <style=cIsUtility>Cloak</style> all allies in a large radius.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXLUNAR_DESC", "Increases <style=cIsHealing>maximum health</style> by <style=cIsHealing>25%</style> and <style=cIsUtility>movement speed</style> by <style=cIsUtility>30%</style>. \nConvert all <style=cIsHealing>health</style> into <style=cIsHealing>regenerating shields</style>. \n<style=cIsDamage>Cripple</style> enemies on hit for <style=cIsUtility>4s</style> reducing <style=cIsUtility>movement speed</style> by <style=cIsUtility>100%</style> and <style=cIsDamage>armor</style> by <style=cIsDamage>20</style>. \nPeriodically shoot out 4 orbs dealing <style=cIsDamage>30%</style> base damage when attacking. ", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXEARTH_DESC", "<style=cIsHealing>Heal</style> a nearby hurt ally for <style=cIsHealing>120%</style> of your <style=cIsDamage>base damage</style> per second. <style=cDeath>On death</style> leave a <style=cIsHealing>Healing Core</style> which will <style=cIsHealing>heal</style> all creatures for <style=cIsHealing>80</style> health.", "en");


            LanguageAPI.Add("ITEM_DRONEWEAPONS_DESC", "Gain <style=cIsDamage>Col. Droneman</style>. \nDrones gain <style=cIsDamage>+50%</style> <style=cStack>(+50% per stack)</style> attack speed and cooldown reduction. \nDrones gain <style=cIsDamage>10%</style> chance to fire a <style=cIsDamage>missile</style> on hit, dealing <style=cIsDamage>300%</style> base damage. \nDrones gain an <style=cIsDamage>automatic chain gun</style> that deals <style=cIsDamage>6x100%</style>damage, bouncing to <style=cIsDamage>2</style> enemies.", "en");

            //Untiered Item Descriptions

            LanguageAPI.Add("ITEM_GUMMYCLONEIDENTIFIER_NAME", "Gummy Clone Identifier", "en");
            LanguageAPI.Add("ITEM_GUMMYCLONEIDENTIFIER_PICKUP", "Turn into a Gummy", "en");
            LanguageAPI.Add("ITEM_GUMMYCLONEIDENTIFIER_DESC", "Visually turns you into a Gummy character created by Goobo Jr.", "en");

            LanguageAPI.Add("ITEM_EMPOWERALWAYS_NAME", "Empower Always", "en");
            LanguageAPI.Add("ITEM_EMPOWERALWAYS_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_EMPOWERALWAYS_DESC", "A unfinished item that does nothing.", "en");

            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY1_NAME", "Drone Weapons Common Display", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY1_PICKUP", "Visuals only for Drones.", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY1_DESC", "Spare Drone Parts common Item Display for Drones.", "en");

            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY2_NAME", "Drone Weapons Rare Display", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY2_PICKUP", "Visuals only for Drones.", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY2_DESC", "Spare Drone Parts rare Item Display for Drones.", "en");

            LanguageAPI.Add("ITEM_DRONEWEAPONSBOOST_NAME", "Drone Weapons Stat Boost", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSBOOST_PICKUP", "Bonus stats granted by Spare Drone Parts.", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSBOOST_DESC", "Gain <style=cIsDamage>+50%</style><style=cStack>(+50% per stack)</style> <style=cIsDamage>attack speed</style> and <style=cIsDamage>skill cooldown reduction</style>, a <style=cIsDamage>10%</style> chance to fire a <style=cIsDamage>Micro Missile</style> dealing <style=cIsDamage>300%</style> base damage and gain an <style=cIsDamage>automatic chain gun</style> that deals <style=cIsDamage>6x100%</style>damage, bouncing to <style=cIsDamage>2</style> enemies.", "en");

            LanguageAPI.Add("ITEM_CONVERTCRITCHANCETOCRITDAMAGE_NAME", "Convert Crit Chance", "en");
            LanguageAPI.Add("ITEM_CONVERTCRITCHANCETOCRITDAMAGE_PICKUP", "Convert all crit chance into crit damage multiplier", "en");
            LanguageAPI.Add("ITEM_CONVERTCRITCHANCETOCRITDAMAGE_DESC", "All <style=cIsDamage>crit chance</style> will be converted into <style=cIsDamage>crit damage multiplier</style> making them deal more damage. You will be unable to deal <style=cIsDamage>Critical Strikes</style> randomly.", "en");



            LanguageAPI.Add("ITEM_BURNNEARBY_NAME", "Burn Nearby", "en");
            LanguageAPI.Add("ITEM_BURNNEARBY_PICKUP", "(Old Helfire Tincture)Permament Helfire Tincture", "en");
            LanguageAPI.Add("ITEM_BURNNEARBY_DESC", "Gain a permament <color=#307FFF>Helfire Tincture</color> burn", "en");

            LanguageAPI.Add("ITEM_CRIPPLEWARDONLEVEL_NAME", "Cripple Ward on Level", "en");
            LanguageAPI.Add("ITEM_CRIPPLEWARDONLEVEL_PICKUP", "(Old Effigy of Grief) Drop a crippling effigy upon enemy level up", "en");
            LanguageAPI.Add("ITEM_CRIPPLEWARDONLEVEL_DESC", "When the enemy team level ups, summon a <color=#307FFF>Effigy of Grief</color> <style=cIsUtility>cripple</style> zone <style=cIsHealth>at your location</style>", "en");

            LanguageAPI.Add("ITEM_CRITHEAL_NAME", "Crit Healing", "en");
            LanguageAPI.Add("ITEM_CRITHEAL_PICKUP", "(Old Corpsebloom) Chance to double healing", "en");
            LanguageAPI.Add("ITEM_CRITHEAL_DESC", "When <style=cIsHealing>healing</style>, have a chance equivelent to your <style=cIsDamage>critical rate</style> to <style=cIsHealing>double the amount</style>. \nGain <style=cIsDamage>5% critical chance</style> and then <style=cIsDamage>halve</style><style=cStack> (-33% per stack)</style> your <style=cIsDamage>critical chance</style>", "en");

            LanguageAPI.Add("ITEM_WARCRYONCOMBAT_NAME", "Warcry on Combat", "en");
            LanguageAPI.Add("ITEM_WARCRYONCOMBAT_PICKUP", "(Old Berzerker's Pauldron) Enter a frenzy at the beginning of combat.", "en");
            LanguageAPI.Add("ITEM_WARCRYONCOMBAT_DESC", "When entering combat emmit a <style=cIsDamage>War Cry</style> for <style=cIsDamage>6 seconds</style><style=cStack> (+4s per stack)</style> buffing you and allies in a <style=cIsDamage>12m radius</style><style=cStack> (+4m per stack)</style> increasing <style=cIsUtility>movement speed</style> by <style=cIsUtility>50%</style> and <style=cIsDamage>attack speed</style> by <style=cIsDamage>100%</style>. Recharges every 30 seconds.", "en");

            LanguageAPI.Add("ITEM_TEMPESTONKILL_NAME", "Tempest On Kill", "en");
            LanguageAPI.Add("ITEM_TEMPESTONKILL_PICKUP", "(Old Wax Quail) Chance to summon a buffing tempest on kill", "en");
            LanguageAPI.Add("ITEM_TEMPESTONKILL_DESC", "<style=cIsUtility>25%</style> chance on kill to spawn a <style=cIsUtility>buffing tempest</style> which will buff you with <style=cIsDamage>Malachite Elite</style> for <style=cIsDamage>2 seconds</style> and expire after <style=cIsUtility>8<style=cStack> (+6 per stack)</style> seconds</style>", "en");

            LanguageAPI.Add("ITEM_MINIONLEASH_NAME", "Minion Leash", "en");
            LanguageAPI.Add("ITEM_MINIONLEASH_PICKUP", "Teleport to your master if he is too far away.", "en");
            LanguageAPI.Add("ITEM_MINIONLEASH_DESC", "You are on a leashed to your master. Teleport you back to him if too far away. If you have no master to be leashed to, lag the game instead.", "en");


            LanguageAPI.Add("ITEM_AACANNON_NAME", "AACannon", "en");
            LanguageAPI.Add("ITEM_AACANNON_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_AACANNON_DESC", "A unfinished item that does nothing.", "en");

            LanguageAPI.Add("ITEM_PLASMACORE_NAME", "Plasma Core", "en");
            LanguageAPI.Add("ITEM_PLASMACORE_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_PLASMACORE_DESC", "A unfinished item that does nothing.", "en");

            LanguageAPI.Add("ITEM_PLANTONHIT_NAME", "Plant On Hit", "en");
            LanguageAPI.Add("ITEM_PLANTONHIT_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_PLANTONHIT_DESC", "A unfinished item that does nothing. Potentially early version of Deskplant.", "en");

            LanguageAPI.Add("ITEM_MAGEATTUNEMENT_NAME", "Artificer Attunement", "en");
            LanguageAPI.Add("ITEM_MAGEATTUNEMENT_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_MAGEATTUNEMENT_DESC", "A unfinished idea that does not function.", "en");

            LanguageAPI.Add("ITEM_GHOST_NAME", "Ghost", "en");
            LanguageAPI.Add("ITEM_GHOST_PICKUP", "Become a Ghost without a hitbox.", "en");
            LanguageAPI.Add("ITEM_GHOST_DESC", "Become permamently <style=cIsUtility>immune to damage</style> and <style=cIsUtility>undetectable</style> if you don't deal damage", "en");



            LanguageAPI.Add("ITEM_BOOSTATTACKSPEED_NAME", "Boost Attack Speed", "en");
            LanguageAPI.Add("ITEM_BOOSTATTACKSPEED_PICKUP", "Increases attack speed.", "en");
            LanguageAPI.Add("ITEM_BOOSTATTACKSPEED_DESC", "Increases <style=cIsDamage>attack speed</style> by <style=cIsDamage>10% <style=cStack>(+10% per stack)</style></style>.", "en");

            LanguageAPI.Add("ITEM_BOOSTDAMAGE_NAME", "Boost Damage", "en");
            LanguageAPI.Add("ITEM_BOOSTDAMAGE_PICKUP", "Increases damage.", "en");
            LanguageAPI.Add("ITEM_BOOSTDAMAGE_DESC", "Increases <style=cIsDamage>damage</style> by <style=cIsDamage>10% <style=cStack>(+10% per stack)</style></style>.", "en");

            LanguageAPI.Add("ITEM_BOOSTHP_NAME", "Boost Health", "en");
            LanguageAPI.Add("ITEM_BOOSTHP_PICKUP", "Increases health.", "en");
            LanguageAPI.Add("ITEM_BOOSTHP_DESC", "Increases <style=cIsHealing>maximum health</style> by <style=cIsHealing>10% <style=cStack>(+10% per stack)</style></style>.", "en");

            LanguageAPI.Add("ITEM_CUTHP_NAME", "Half Health", "en");
            LanguageAPI.Add("ITEM_CUTHP_PICKUP", "Halves your health.", "en");
            LanguageAPI.Add("ITEM_CUTHP_DESC", "Reduce <style=cIsHealing>maximum health</style> <style=cIsHealing>by 50%</style> <style=cStack>(+50% per stack)</style>.", "en");


            LanguageAPI.Add("ITEM_SKULLCOUNTER_DESC", "Proof of a specific kind of kill. Has no direct effect of its own. \nPreviously used for Bandits Desperado skill", "en");



            LanguageAPI.Add("ITEM_BOOSTEQUIPMENTRECHARGE_NAME", "Reduce Equipment Cooldown", "en");
            LanguageAPI.Add("ITEM_BOOSTEQUIPMENTRECHARGE_PICKUP", "Reduce equipment cooldown.", "en");
            LanguageAPI.Add("ITEM_BOOSTEQUIPMENTRECHARGE_DESC", "<style=cIsUtility>Reduce equipment cooldown</style> by <style=cIsUtility>10%</style> <style=cStack>(+10% per stack)</style>.", "en");

            LanguageAPI.Add("ITEM_ADAPTIVEARMOR_NAME", "Adaptive Armor", "en");
            LanguageAPI.Add("ITEM_ADAPTIVEARMOR_PICKUP", "Gain temporary Armor on hit.", "en");
            LanguageAPI.Add("ITEM_ADAPTIVEARMOR_DESC", "Gain upwards of <style=cIsUtility>400 Armor</style> depending on the amount of damage taken recently", "en");


            LanguageAPI.Add("ITEM_INVADINGDOPPELGANGER_NAME", "Invading Umbra", "en");
            LanguageAPI.Add("ITEM_INVADINGDOPPELGANGER_PICKUP", "Become an Umbra.", "en");
            LanguageAPI.Add("ITEM_INVADINGDOPPELGANGER_DESC", "Reduce <style=cIsDamage>damage</style> by <style=cIsDamage>96%</style> and increase <style=cIsHealing>maximum health</style> by <style=cIsHealing>900%</style>. ", "en");

            LanguageAPI.Add("ITEM_MONSOONPLAYERHELPER_NAME", "Monsoon Helper", "en");
            LanguageAPI.Add("ITEM_MONSOONPLAYERHELPER_PICKUP", "Apply Monsoon difficulty stat changes.", "en");
            LanguageAPI.Add("ITEM_MONSOONPLAYERHELPER_DESC", "Reduce <style=cIsHealing>base health regeneration</style> by <style=cIsHealing>40%</style>. ", "en");

            LanguageAPI.Add("ITEM_DRIZZLEPLAYERHELPER_NAME", "Drizzle Helper", "en");
            LanguageAPI.Add("ITEM_DRIZZLEPLAYERHELPER_PICKUP", "Apply Drizzle difficulty stat changes.", "en");
            LanguageAPI.Add("ITEM_DRIZZLEPLAYERHELPER_DESC", "Increase <style=cIsHealing>base health regeneration</style> by <style=cIsHealing>50%</style> and <style=cIsUtility>armor</style> by <style=cIsUtility>70 <style=cStack>(+70% per stack)</style></style>. ", "en");

            LanguageAPI.Add("ITEM_HEALTHDECAY_NAME", "Health Decay", "en");
            LanguageAPI.Add("ITEM_HEALTHDECAY_PICKUP", "Rapidly drain your health", "en");
            LanguageAPI.Add("ITEM_HEALTHDECAY_DESC", "<style=cIsHealth>Die</style> in <style=cIsHealth>1 second<style=cStack> (+1 second per stack)</style></style>.", "en");

            LanguageAPI.Add("ITEM_LEVELBONUS_NAME", "Bonus Level", "en");
            LanguageAPI.Add("ITEM_LEVELBONUS_PICKUP", "Increase your level", "en");
            LanguageAPI.Add("ITEM_LEVELBONUS_DESC", "Gain <style=cIsUtility>1 Level</style><style=cStack> (+1 per stack)</style>. ", "en");


            LanguageAPI.Add("ITEM_TEAMSIZEDAMAGEBONUS_NAME", "Team Damage Boost", "en");
            LanguageAPI.Add("ITEM_TEAMSIZEDAMAGEBONUS_PICKUP", "Increase damage per owned minion", "en");
            LanguageAPI.Add("ITEM_TEAMSIZEDAMAGEBONUS_DESC", "Increases <style=cIsDamage>damage</style> by <style=cIsDamage>100% <style=cStack>(+100% per stack)</style></style> for every <style=cIsUtility>minion</style> you own.", "en");

            LanguageAPI.Add("ITEM_USEAMBIENTLEVEL_NAME", "Use Ambient Level", "en");
            LanguageAPI.Add("ITEM_USEAMBIENTLEVEL_PICKUP", "Use the Ambient level as your level", "en");
            LanguageAPI.Add("ITEM_USEAMBIENTLEVEL_DESC", "Your <style=cIsUtility>level</style> is equal or greater than the <style=cIsUtility>ambient level</style>", "en");

            LanguageAPI.Add("ITEM_SUMMONEDECHO_NAME", "Summoned Echo", "en");
            LanguageAPI.Add("ITEM_SUMMONEDECHO_PICKUP", "Become an Echo", "en");
            LanguageAPI.Add("ITEM_SUMMONEDECHO_DESC", "Reduce <style=cIsHealing>maximum health</style> by <style=cIsHealing>90%</style> and increase <style=cIsUtility>base movement speed</style> by <style=cIsUtility>66%</style>. Periodically shoot out <style=cIsDamage>homing projectiles</style> dealing <style=cIsDamage>275%</style> base damage and <style=cIsUtility>slowing</style> enemies on hit by <style=cIsUtility>50%</style> for <style=cIsUtility>2</style> seconds.", "en");



            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TeamSizeDamageBonus").nameToken = "ITEM_TEAMSIZEDAMAGEBONUS_NAME";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TeamSizeDamageBonus").pickupToken = "ITEM_TEAMSIZEDAMAGEBONUS_PICKUP";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TeamSizeDamageBonus").descriptionToken = "ITEM_TEAMSIZEDAMAGEBONUS_DESC";

            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/UseAmbientLevel").nameToken = "ITEM_USEAMBIENTLEVEL_NAME";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/UseAmbientLevel").pickupToken = "ITEM_USEAMBIENTLEVEL_PICKUP";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/UseAmbientLevel").descriptionToken = "ITEM_USEAMBIENTLEVEL_DESC";

            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/SummonedEcho").nameToken = "ITEM_SUMMONEDECHO_NAME";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/SummonedEcho").pickupToken = "ITEM_SUMMONEDECHO_PICKUP";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/SummonedEcho").descriptionToken = "ITEM_SUMMONEDECHO_DESC";




            ItemDef MinHealthPercentage = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Base/MinHealthPercentage/MinHealthPercentage.asset").WaitForCompletion();
            LanguageAPI.Add("ITEM_MINHEALTHPERCENTAGE_NAME", "Minimum Health Percentage", "en");
            LanguageAPI.Add("ITEM_MINHEALTHPERCENTAGE_PICKUP", "Prevent HP from going below an amount", "en");
            LanguageAPI.Add("ITEM_MINHEALTHPERCENTAGE_DESC", "<style=cIsHealing>Health</style> will not go below <style=cIsHealing>1%<style=cStack> (+1% per stack)", "en");
            MinHealthPercentage.nameToken = "ITEM_MINHEALTHPERCENTAGE_NAME";
            MinHealthPercentage.pickupToken = "ITEM_MINHEALTHPERCENTAGE_PICKUP";
            MinHealthPercentage.descriptionToken = "ITEM_MINHEALTHPERCENTAGE_DESC";

            ItemDef VoidmanPassiveItem = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC1/VoidSurvivor/VoidmanPassiveItem.asset").WaitForCompletion();
            LanguageAPI.Add("ITEM_VOIDMANPASSIVEITEM_NAME", "Void Fiend Passive Item", "en");
            LanguageAPI.Add("ITEM_VOIDMANPASSIVEITEM_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_VOIDMANPASSIVEITEM_DESC", "A item supposed to be responsible for Void Fiends passive, however it does nothing.", "en");
            VoidmanPassiveItem.nameToken = "ITEM_VOIDMANPASSIVEITEM_NAME";
            VoidmanPassiveItem.pickupToken = "ITEM_VOIDMANPASSIVEITEM_PICKUP";
            VoidmanPassiveItem.descriptionToken = "ITEM_VOIDMANPASSIVEITEM_DESC";

            ItemDef TeleportWhenOob = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Base/TeleportWhenOob/TeleportWhenOob.asset").WaitForCompletion();
            LanguageAPI.Add("ITEM_TELEPORTWHENOOB_NAME", "Teleport when out of bounds", "en");
            LanguageAPI.Add("ITEM_TELEPORTWHENOOB_PICKUP", "Teleport to safety when out of bounds.", "en");
            LanguageAPI.Add("ITEM_TELEPORTWHENOOB_DESC", "You will teleport back to the map when out of bounds. This is just for enemies as players do this normally.", "en");
            TeleportWhenOob.nameToken = "ITEM_TELEPORTWHENOOB_NAME";
            TeleportWhenOob.pickupToken = "ITEM_TELEPORTWHENOOB_PICKUP";
            TeleportWhenOob.descriptionToken = "ITEM_TELEPORTWHENOOB_DESC";


            //Unused Equipment Descriptions
            LanguageAPI.Add("EQUIPMENT_LUNARPOTION_NAME", "Lunar Potion", "en");
            LanguageAPI.Add("EQUIPMENT_LUNARPOTION_PICKUP", "Cannot activate this.", "en");
            LanguageAPI.Add("EQUIPMENT_LUNARPOTION_DESC", "Cannot be used. <style=cIsHealing>100%</style> of <style=cIsHealing>healing</style> is stored in the potion and returned upon using it.", "en");

            LanguageAPI.Add("EQUIPMENT_ORBONUSE_NAME", "Orb on use", "en");
            LanguageAPI.Add("EQUIPMENT_ORBONUSE_PICKUP", "Cannot activate this.", "en");
            LanguageAPI.Add("EQUIPMENT_ORBONUSE_DESC", "Cannot be used. Purpose unknown. Many unused orb passive items exist in the files but relationship unknown.", "en");


            LanguageAPI.Add("EQUIPMENT_ORBITALLASER_NAME", "Orbital Laser", "en");
            LanguageAPI.Add("EQUIPMENT_ORBITALLASER_PICKUP", "Fire a guided orbital laser.", "en");
            LanguageAPI.Add("EQUIPMENT_ORBITALLASER_DESC", "Deal <style=cIsDamage>2000% damage</style> multiple times per second at your cursors location.", "en");

            LanguageAPI.Add("EQUIPMENT_SOULCORRUPTOR_NAME", "Soul Corrupter", "en");
            LanguageAPI.Add("EQUIPMENT_SOULCORRUPTOR_PICKUP", "Turn targeted low health monsters into a friendly ghost.", "en");
            LanguageAPI.Add("EQUIPMENT_SOULCORRUPTOR_DESC", "<style=cIsDamage>Instantly kill</style> the targeted monster if its below <style=cIsHealing>25% health</style> and turn it into a <style=cIsUtility>ghost</style>", "en");

            LanguageAPI.Add("EQUIPMENT_ENIGMA_NAME", "Enigma", "en");
            LanguageAPI.Add("EQUIPMENT_ENIGMA_PICKUP", "Cannot activate this.", "en");
            LanguageAPI.Add("EQUIPMENT_ENIGMA_DESC", "Previously used for Artifact of Enigma before they decided to use regular equipment that reroll instead of a single equipment activating random ones.", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXUNFINISHED_PICKUP", "Become an Aspect of unknown intentions", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXYELLOW_NAME", "EliteYellowEquipment", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXYELLOW_DESC", "Increases <style=cIsDamage>attack speed</style> by <style=cIsDamage>50% </style> and <style=cIsUtility>movement speed</style> by <style=cIsUtility>25%</style>.", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXECHO_NAME", "EliteEchoEquipment", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXECHO_DESC", "Gain 2 <style=cIsDamage>Echoes</style> that will fight for you.", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXSECRETSPEED_DESC", "Turns you into a furry. Does nothing.", "en");


            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixYellow").nameToken = "EQUIPMENT_AFFIXYELLOW_NAME";
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixYellow").pickupToken = "EQUIPMENT_AFFIXUNFINISHED_PICKUP";
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixYellow").descriptionToken = "EQUIPMENT_AFFIXYELLOW_DESC";

            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixEcho").nameToken = "EQUIPMENT_AFFIXECHO_NAME";
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixEcho").pickupToken = "EQUIPMENT_AFFIXUNFINISHED_PICKUP";
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixEcho").descriptionToken = "EQUIPMENT_AFFIXECHO_DESC";




            EquipmentDef VoidAffix = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteVoid/EliteVoidEquipment.asset").WaitForCompletion();
            GameObject VoidAffixDisplay = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/EliteVoid/DisplayAffixVoid.prefab").WaitForCompletion(), "PickupAffixVoidW", false);
            VoidAffixDisplay.transform.GetChild(0).GetChild(1).SetAsFirstSibling();
            VoidAffixDisplay.transform.GetChild(1).localPosition = new Vector3(0f, 0.7f, 0f);
            VoidAffixDisplay.transform.GetChild(0).eulerAngles = new Vector3(330, 0, 0);

            LanguageAPI.Add("EQUIPMENT_AFFIXVOID_NAME", "Voidborn Curiosity", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXVOID_PICKUP", "Lose your aspect of self.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXVOID_DESC", "Increases <style=cIsHealing>maximum health</style> by <style=cIsHealing>50%</style> and decrease <style=cIsDamage>base damage</style> by <style=cIsDamage>30%</style>. <style=cIsDamage>Collapse</style> enemies on hit and <style=cIsHealing>block</style> incoming damage once every <style=cIsUtility>15 seconds</style>. ", "en");
            //VoidAffix.dropOnDeathChance = 0.00025f;

            Texture2D UniqueAffixVoid = new Texture2D(128, 128, TextureFormat.DXT5, false);
            UniqueAffixVoid.LoadImage(Properties.Resources.UniqueAffixVoid, false);
            UniqueAffixVoid.filterMode = FilterMode.Bilinear;
            UniqueAffixVoid.wrapMode = TextureWrapMode.Clamp;
            Sprite UniqueAffixVoidS = Sprite.Create(UniqueAffixVoid, rec128, half);

            VoidAffix.pickupIconSprite = UniqueAffixVoidS;
            VoidAffix.pickupModelPrefab = VoidAffixDisplay;





            LanguageAPI.Add("EQUIPMENT_SOULJAR_DESC", "Cannot be used. <style=cIsUtility>Duplicate</style> every enemy as a <style=cIsUtility>ghost</style> to fight on your side for <style=cIsUtility>15 seconds</style>.", "en");

            LanguageAPI.Add("EQUIPMENT_GHOSTGUN_DESC", "Shoot nearby enemies for <style=cIsDamage>500% damage 6 times</style> . Shots have a proc coefficient of 0. Supposed to multiply damage based on kills but does not track kills.", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXGOLD_DESC", "Does nothing due to missing elite implementation.", "en");






            GameObject tempghostgun = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GhostGun");
            tempghostgun.transform.GetChild(0).localScale = new Vector3(1, 1, 1);

            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/DrizzlePlayerHelper").pickupIconSprite = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/difficultyicons/texDifficultyEasyIconDisabled"); ;
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/MonsoonPlayerHelper").pickupIconSprite = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/difficultyicons/texDifficultyHardIconDisabled"); ;





        }

        private static void VoidCloverListener(On.RoR2.Inventory.orig_RemoveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);
            VoidCloverInventory = self;
            VoidCloverCount = count;

        }

        public static void ModelViewer()
        {

            Texture2D texArchWisp = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texArchWisp.LoadImage(Properties.Resources.autogen_ArchWispBody, false);
            texArchWisp.filterMode = FilterMode.Bilinear;
            texArchWisp.wrapMode = TextureWrapMode.Clamp;
            Texture2D texAncientWisp = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texAncientWisp.LoadImage(Properties.Resources.autogen_AncientWispBody, false);
            texAncientWisp.filterMode = FilterMode.Bilinear;
            texAncientWisp.wrapMode = TextureWrapMode.Clamp;
            Texture2D texHAND = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texHAND.LoadImage(Properties.Resources.autogen_HANDBody, false);
            texHAND.filterMode = FilterMode.Bilinear;
            texHAND.wrapMode = TextureWrapMode.Clamp;
            Texture2D texOldBandit = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texOldBandit.LoadImage(Properties.Resources.autogen_texBodyOldBanditIcon, false);
            texOldBandit.filterMode = FilterMode.Bilinear;
            texOldBandit.wrapMode = TextureWrapMode.Clamp;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArchWispBody").GetComponent<CharacterBody>().portraitIcon = texArchWisp; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AncientWispBody").GetComponent<CharacterBody>().portraitIcon = texAncientWisp; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HANDBody").GetComponent<CharacterBody>().portraitIcon = texHAND; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BanditBody").GetComponent<CharacterBody>().portraitIcon = texOldBandit; //

            UnlockableDef dummyunlock = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleBody").GetComponent<DeathRewards>().logUnlockableDef;

            for (int i = 0; i < BodyCatalog.bodyCount; i++)
            {
                DeathRewards deathRewards = BodyCatalog.bodyPrefabs[i].GetComponent<DeathRewards>();
                if (deathRewards)
                {
                    deathRewards.logUnlockableDef = dummyunlock;
                }
                else
                {
                    BodyCatalog.bodyPrefabs[i].AddComponent<DeathRewards>().logUnlockableDef = dummyunlock;
                }
            }

            //Debug.LogWarning(BodyCatalog.bodyCount);


            /*
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallRedBuddyBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallGreenBuddyBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //

            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/GravekeeperTrackingFireball").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/GravekeeperTrackingFireball").GetComponent<CharacterBody>().baseNameToken = "GRAVEKEEPER_BODY_NAME"; //

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleGuardAllyBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArchWispBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AncientWispBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock; //

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherGlassBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock; //

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ParentPodBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SquidTurretBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AltarSkeletonBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ExplosivePotDestructibleBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FusionCellDestructibleBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BanditBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/WispSoulBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HANDBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BackupDroneBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EmergencyDroneBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Drone2Body").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Drone1Body").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FlameDroneBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MegaDroneBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MissileDroneBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EquipmentDroneBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Turret1Body").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock; //
        */


        }



        public static void UIEquipmentIconColorChanger(On.RoR2.UI.EquipmentIcon.orig_Update orig, global::RoR2.UI.EquipmentIcon self)
        {
            orig(self);

            if (!self.targetInventory) { return; }
            if (self.targetInventory.currentEquipmentIndex != EquipmentIndex.None)
            {
                if (self.targetInventory.currentEquipmentState.equipmentDef.isLunar == true)
                {
                    self.tooltipProvider.titleColor = FakeBlueEquip;
                }
                else if (self.targetInventory.currentEquipmentState.equipmentDef.isBoss == true)
                {
                    self.tooltipProvider.titleColor = FakeYellowEquip;
                }
            }
            //if (!self.displayAlternateEquipment) { return; }
            if (self.targetInventory.alternateEquipmentIndex != EquipmentIndex.None)
            {
                /*
                if (self.displayAlternateEquipment)
                {
                    self.displayRoot.SetActive(true);
                }
                */
                if (self.targetInventory.alternateEquipmentState.equipmentDef.isLunar == true)
                {
                    self.tooltipProvider.titleColor = FakeBlueEquip;
                }
                else if (self.targetInventory.alternateEquipmentState.equipmentDef.isBoss == true)
                {
                    self.tooltipProvider.titleColor = FakeYellowEquip;
                }
            }

        }

        public static void PickupEquipmentNotification(On.RoR2.UI.GenericNotification.orig_SetEquipment orig, RoR2.UI.GenericNotification self, global::RoR2.EquipmentDef equipmentDef)
        {
            orig(self, equipmentDef);

            if (equipmentDef.isBoss == true)
            {
                self.titleTMP.color = FakeYellowEquip;
            }
            else if (equipmentDef.isLunar == true)
            {
                self.titleTMP.color = FakeBlueEquip;
            }
        }


        public static void LateRunningMethod()
        {
            
            RoR2.Stats.StatDef.highestLunarPurchases.displayToken = "STATNAME_HIGHESTLUNARPURCHASES";
            RoR2.Stats.StatDef.highestBloodPurchases.displayToken = "STATNAME_HIGHESTBLOODPURCHASES";

            /*
            foreach (CharacterBody charBod in BodyCatalog.allBodyPrefabBodyBodyComponents)
            {
                charBod.portraitIcon = null;
            }
            */

            //RoR2Content.Buffs.GoldEmpowered.eliteDef = RoR2Content.Elites.Gold;
            //RoR2Content.Equipment.AffixGold.passiveBuffDef = RoR2Content.Buffs.GoldEmpowered;

            if (BlightAcrid.Value == true)
            {
                BurnEffectController.blightEffect.fireEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = SkinChanges.matCrocoGooLargeBlight;
            }


            AffixLunarItemDisplay();




            //RoR2Content.Elites.Echo.eliteEquipmentDef = RoR2Content.Equipment.AffixEcho;

            Texture2D WickedRingTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
            WickedRingTex.LoadImage(Properties.Resources.betaWicked, false);
            WickedRingTex.filterMode = FilterMode.Bilinear;
            WickedRingTex.wrapMode = TextureWrapMode.Clamp;
            Sprite WickedRing = Sprite.Create(WickedRingTex, rec128, half);
            JunkContent.Items.CooldownOnCrit.pickupIconSprite = WickedRing;

            Texture2D betaCorpse = new Texture2D(128, 128, TextureFormat.DXT5, false);
            betaCorpse.LoadImage(Properties.Resources.betaCorpse, false);
            betaCorpse.filterMode = FilterMode.Bilinear;
            betaCorpse.wrapMode = TextureWrapMode.Clamp;
            Sprite betaCorpseS = Sprite.Create(betaCorpse, rec128, half);
            JunkContent.Items.CritHeal.pickupIconSprite = betaCorpseS;

            Texture2D betaEffigy = new Texture2D(128, 128, TextureFormat.DXT5, false);
            betaEffigy.LoadImage(Properties.Resources.betaEffigy, false);
            betaEffigy.filterMode = FilterMode.Bilinear;
            betaEffigy.wrapMode = TextureWrapMode.Clamp;
            Sprite betaEffigyS = Sprite.Create(betaEffigy, rec128, half);
            RoR2Content.Items.CrippleWardOnLevel.pickupIconSprite = betaEffigyS;

            Texture2D betaHelfire = new Texture2D(128, 128, TextureFormat.DXT5, false);
            betaHelfire.LoadImage(Properties.Resources.betaHelfire, false);
            betaHelfire.filterMode = FilterMode.Bilinear;
            betaHelfire.wrapMode = TextureWrapMode.Clamp;
            Sprite betaHelfireS = Sprite.Create(betaHelfire, rec128, half);
            JunkContent.Items.BurnNearby.pickupIconSprite = betaHelfireS;

            Texture2D betaPauldron = new Texture2D(128, 128, TextureFormat.DXT5, false);
            betaPauldron.LoadImage(Properties.Resources.betaPauldron, false);
            betaPauldron.filterMode = FilterMode.Bilinear;
            betaPauldron.wrapMode = TextureWrapMode.Clamp;
            Sprite betaPauldronS = Sprite.Create(betaPauldron, rec128, half);
            JunkContent.Items.WarCryOnCombat.pickupIconSprite = betaPauldronS;

            Texture2D betaTempest = new Texture2D(128, 128, TextureFormat.DXT5, false);
            betaTempest.LoadImage(Properties.Resources.betaTempest, false);
            betaTempest.filterMode = FilterMode.Bilinear;
            betaTempest.wrapMode = TextureWrapMode.Clamp;
            Sprite betaTempestS = Sprite.Create(betaTempest, rec128, half);
            JunkContent.Items.TempestOnKill.pickupIconSprite = betaTempestS;

            RoR2Content.Items.AdaptiveArmor.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;
            RoR2Content.Items.BoostEquipmentRecharge.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;
            JunkContent.Equipment.Enigma.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;




            Texture2D texBuffMercExposeIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            texBuffMercExposeIcon.LoadImage(Properties.Resources.texBuffMercExposeIcon, false);
            texBuffMercExposeIcon.filterMode = FilterMode.Bilinear;
            texBuffMercExposeIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite texBuffMercExposeIconS = Sprite.Create(texBuffMercExposeIcon, rec128, half);
            if (MercOniRedSword.Value == true)
            {
                RoR2Content.Buffs.MercExpose.iconSprite = texBuffMercExposeIconS;
            }




            /*
            Texture2D BuffAffixEchoTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
            BuffAffixEchoTex.LoadImage(Properties.Resources.texBuffAffixEcho, false);
            BuffAffixEchoTex.filterMode = FilterMode.Trilinear;
            BuffAffixEchoTex.wrapMode = TextureWrapMode.Clamp;
            Sprite BuffAffixEchoS = Sprite.Create(BuffAffixEchoTex, rec128, half);

            RoR2Content.Elites.Echo.shaderEliteRampIndex = 7;
            RoR2Content.Buffs.AffixEcho.iconSprite = BuffAffixEchoS;
            RoR2Content.Buffs.AffixEcho.buffColor = new Color(1, 1, 1, 1);

            Texture2D AspectEchoTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
            AspectEchoTex.LoadImage(Properties.Resources.AffixRoR2Echo, false);
            AspectEchoTex.filterMode = FilterMode.Trilinear;
            AspectEchoTex.wrapMode = TextureWrapMode.Clamp;
            Sprite AspectEchoS = Sprite.Create(AspectEchoTex, rec128, half);

            RoR2Content.Equipment.AffixEcho.pickupIconSprite = AspectEchoS;
            */



            Texture2D TexBodyBrother = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexBodyBrother.LoadImage(Properties.Resources.texBodyBrother, false);
            TexBodyBrother.filterMode = FilterMode.Bilinear;
            TexBodyBrother.wrapMode = TextureWrapMode.Clamp;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrother;


            if (UpdateBodyIconsConfig.Value == true)
            {
                Texture2D TexBlueSquidTurret = new Texture2D(128, 128, TextureFormat.DXT5, false);
                TexBlueSquidTurret.LoadImage(Properties.Resources.texBodySquidTurret, false);
                TexBlueSquidTurret.filterMode = FilterMode.Bilinear;
                TexBlueSquidTurret.wrapMode = TextureWrapMode.Clamp;

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SquidTurretBody").GetComponent<CharacterBody>().portraitIcon = TexBlueSquidTurret;


            }

        }


        public static void LogbookEntryAdder(On.RoR2.UI.LogBook.LogBookController.orig_BuildStaticData orig)
        {



            if (MoreLogEntries.Value == true)
            {
                UnlockableDef dummyunlock = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<DeathRewards>().logUnlockableDef;

                UnlockableDef gupunlock = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody").GetComponent<DeathRewards>().logUnlockableDef;
                UnlockableDef loopunlock = RoR2.LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Items.BounceNearby");

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar3Body").GetComponent<CharacterBody>().baseNameToken = "SCAVLUNAR_BODY_SUBTITLE";

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar3Body").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody").GetComponent<DeathRewards>().logUnlockableDef = dummyunlock;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/UrchinTurretBody").AddComponent<DeathRewards>().logUnlockableDef = dummyunlock;

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GeepBody").GetComponent<DeathRewards>().logUnlockableDef = gupunlock;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GipBody").GetComponent<DeathRewards>().logUnlockableDef = gupunlock;

                RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/QuestVolatileBattery").canDrop = true;
                RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/BossHunterConsumed").canDrop = true;
            }


            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").isLunar = true;
            List<EquipmentIndex> FullEquipmentList = EquipmentCatalog.equipmentList;
            int[] invoutput = new int[EquipmentCatalog.equipmentCount];


            if (MoreLogEntriesAspect.Value == true)
            {
                for (var i = 0; i < invoutput.Length; i++)
                {

                    EquipmentDef tempequipdef = EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i);
                    string tempname = tempequipdef.name;
                    //Debug.LogWarning(tempequipdef + "   " + tempname + "   " + tempequipdef.pickupModelPrefab);


                    if (tempname.Contains("Affix") || tempname.Contains("AFFIX") || tempname.StartsWith("Elite"))
                    {
                        if (tempname.Contains("Gold") || tempname.Contains("Echo") || tempname.Contains("Yellow") || tempname.Contains("Secret") || tempname.StartsWith("ELITE_EQUIPMENT_AFFIX_EXAMPLE"))
                        {
                        }
                        else
                        {
                            if (!tempname.Contains("Poison") && !tempname.Contains("Haunted") && !tempname.Contains("Blighted"))
                            {

                                if (tempname.Contains("Lunar"))
                                {
                                    if (PrismaticTrialsAllElites.Value == "Lunar")
                                    {
                                        GameplayQoL.bossAffixes = GameplayQoL.bossAffixes.Add((EquipmentIndex)i);
                                    }
                                }
                                else
                                {
                                    GameplayQoL.bossAffixes = GameplayQoL.bossAffixes.Add((EquipmentIndex)i);
                                }
                            }

                            if (YellowEliteEquip.Value == true)
                            {
                                EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).isBoss = true;
                                EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).colorIndex = ColorCatalog.ColorIndex.BossItem;
                            }


                            if (EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).dropOnDeathChance != 0)
                            {
                                tempequipdef.canDrop = true;
                            }
                        }
                    }


                    //Debug.LogWarning(tempequipdef);
                    //Debug.LogWarning(tempequipdef.GetPropertyValue<Texture>("bgIconTexture"));


                    //Debug.LogWarning("End");
                }


                if (LunarYellowEliteEquipTexture.Value == true)
                {
                    RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").isBoss = true;
                    RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").colorIndex = ColorCatalog.ColorIndex.BossItem;
                }
                else
                {
                    RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").isBoss = false;
                    RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").colorIndex = ColorCatalog.ColorIndex.LunarItem;
                }

            }




            for (int i = 0; i < PickupCatalog.entries.Length; i++)
            {
                if (PickupCatalog.entries[i].itemIndex != ItemIndex.None)
                {
                    PickupCatalog.entries[i].iconSprite = ItemCatalog.GetItemDef(PickupCatalog.entries[i].itemIndex).pickupIconSprite;
                }
                else if (PickupCatalog.entries[i].equipmentIndex != EquipmentIndex.None)
                {
                    PickupCatalog.entries[i].iconSprite = EquipmentCatalog.GetEquipmentDef(PickupCatalog.entries[i].equipmentIndex).pickupIconSprite;
                }
            }



            orig();

            //Debug.LogWarning("MainMenu Logbook changer");

            if (MoreLogEntries.Value == true)
            {
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar3Body").GetComponent<CharacterBody>().baseNameToken = "SCAVLUNAR3_BODY_NAME";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar3Body").GetComponent<DeathRewards>().logUnlockableDef = SceneCatalog.GetUnlockableLogFromBaseSceneName("limbo");
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody").GetComponent<DeathRewards>().logUnlockableDef = SceneCatalog.GetUnlockableLogFromBaseSceneName("bazaar");

                if (MoffeinClayMan != null)
                {
                    //MoffeinClayMan.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<DeathRewards>().logUnlockableDef = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBossBody").GetComponent<DeathRewards>().logUnlockableDef;
                };

                RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/QuestVolatileBattery").canDrop = false;
                RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/BossHunterConsumed").canDrop = false;
            }

            FullEquipmentList = EquipmentCatalog.equipmentList;

            for (var i = 0; i < invoutput.Length; i++)
            {
                string tempname = EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).name;
                if (tempname.StartsWith("Affix") || tempname.StartsWith("Elite"))
                {
                    if (tempname.Contains("Gold") || tempname.Contains("Echo") || tempname.Contains("Yellow"))
                    {
                    }
                    else
                    {
                        EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).colorIndex = ColorCatalog.ColorIndex.Equipment;
                        EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).canDrop = false;
                    }
                }
            }
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").colorIndex = ColorCatalog.ColorIndex.LunarItem;
            //On.RoR2.UI.LogBook.LogBookController.BuildStaticData -= LogbookEntryAdder;
            Debug.LogWarning("LogbookChanger");

        }


        public void BuffColorChanger()
        {

            if (ChangeRepeatBuffColors.Value == true)
            {
                RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/AffixHauntedRecipient").buffColor = new Color32(148, 215, 214, 255); //94D7D6 Celestine Elite
                RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/SmallArmorBoost").buffColor = RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/Slow60").buffColor;
                RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/WhipBoost").buffColor = new Color32(245, 158, 73, 255); //E8813D
            }

            BuffDef NormalBurn = RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/OnFire");


            FakeHellFire = ScriptableObject.CreateInstance<BuffDef>();
            FakeHellFire.iconSprite = NormalBurn.iconSprite;
            FakeHellFire.buffColor = new Color32(50, 188, 255, 255);
            FakeHellFire.name = "visual_HelFire";
            FakeHellFire.isDebuff = false;
            FakeHellFire.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeHellFire);


            FakePercentBurn = ScriptableObject.CreateInstance<BuffDef>();
            FakePercentBurn.iconSprite = NormalBurn.iconSprite;
            //FakePercentBurn.buffColor = new Color32(226, 91, 69, 255); //E25B45 Overheat
            FakePercentBurn.buffColor = new Color32(203, 53, 38, 255); //CB3526 Blazing Elite
            FakePercentBurn.name = "visual_EnemyBurn";
            FakePercentBurn.isDebuff = false;
            FakePercentBurn.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakePercentBurn);




            Texture2D BugUp = new Texture2D(128, 128, TextureFormat.DXT5, false);
            BugUp.LoadImage(Properties.Resources.texBuffBeetleUp, false);
            BugUp.filterMode = FilterMode.Bilinear;
            Sprite BugUpS = Sprite.Create(BugUp, rec128, half);

            BuffDef BugWings = RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/BugWings");
            FakeBugWings = ScriptableObject.CreateInstance<BuffDef>();
            if (BugWingsVisual.Value == "MovespeedIcon")
            {
                FakeBugWings.iconSprite = BugWings.iconSprite;
            }
            else
            {
                FakeBugWings.iconSprite = BugUpS;
            }
            FakeBugWings.buffColor = new Color32(218, 136, 251, 255); //DA88FB Tesla Coil
            FakeBugWings.name = "visual_BugFlight";
            FakeBugWings.isDebuff = false;
            FakeBugWings.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeBugWings);

            if (BugWingsVisual.Value != "Disabled")
            {
                On.RoR2.JetpackController.StartFlight += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.GetComponentInParent<CharacterBody>().AddTimedBuff(FakeBugWings, self.duration);
                    }
                };
                On.RoR2.JetpackController.OnDestroy += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self)
                        {
                            self.GetComponentInParent<CharacterBody>().ClearTimedBuffs(FakeBugWings);
                        }
                    }
                };
            }


            Texture2D BWLunarShell = new Texture2D(128, 128, TextureFormat.DXT5, false);
            BWLunarShell.LoadImage(Properties.Resources.texBuffLunarShellIcon, false);
            BWLunarShell.filterMode = FilterMode.Bilinear;
            Sprite BWLunarShellS = Sprite.Create(BWLunarShell, rec128, half);

            FakeStrides = ScriptableObject.CreateInstance<BuffDef>();
            FakeStrides.iconSprite = BWLunarShellS;
            FakeStrides.buffColor = new Color32(189, 176, 255, 255); //BDB0FF
            FakeStrides.name = "visual_ShadowIntangible";
            FakeStrides.isDebuff = false;
            FakeStrides.canStack = false;

            R2API.ContentAddition.AddBuffDef(FakeStrides);


            if (StridesVisual.Value == true)
            {
                On.EntityStates.GhostUtilitySkillState.OnEnter += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        float tempdur = EntityStates.GhostUtilitySkillState.baseDuration * self.outer.GetComponentInParent<CharacterBody>().inventory.GetItemCount(RoR2Content.Items.LunarUtilityReplacement);
                        self.outer.GetComponentInParent<CharacterBody>().AddTimedBuff(FakeStrides, tempdur);
                    }
                };

                On.EntityStates.GhostUtilitySkillState.OnExit += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.outer.GetComponentInParent<CharacterBody>().ClearTimedBuffs(FakeStrides);
                    }
                };
            }



            Texture2D RoundShieldTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
            RoundShieldTex.LoadImage(Properties.Resources.texBuffBodyArmor, false);
            RoundShieldTex.filterMode = FilterMode.Bilinear;
            Sprite RoundShieldS = Sprite.Create(RoundShieldTex, rec128, half);
            Texture2D RoundShieldTex2 = new Texture2D(128, 128, TextureFormat.DXT5, false);
            RoundShieldTex2.LoadImage(Properties.Resources.texBuffRoundShieldBuff, false);
            RoundShieldTex2.filterMode = FilterMode.Bilinear;
            Sprite RoundShieldS2 = Sprite.Create(RoundShieldTex2, rec128, half);


            FakeRoseBuckle = ScriptableObject.CreateInstance<BuffDef>();
            FakeRoseBuckle.iconSprite = RoundShieldS;
            FakeRoseBuckle.buffColor = new Color32(251, 199, 38, 255); //FBC726
            FakeRoseBuckle.name = "visual_SprintArmor";
            FakeRoseBuckle.isDebuff = false;
            FakeRoseBuckle.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeRoseBuckle);

            if (RoseBuckleVisual.Value == true)
            {
                On.RoR2.CharacterBody.OnSprintStart += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self.inventory && self.inventory.GetItemCount(RoR2Content.Items.SprintArmor) > 0)
                        {
                            self.AddBuff(FakeRoseBuckle);
                        }
                    }
                };

                On.RoR2.CharacterBody.OnSprintStop += (orig, self) =>
                {

                    orig(self);
                    if (UnityEngine.Networking.NetworkServer.active)
                    {
                        if (self.inventory && self.inventory.GetItemCount(RoR2Content.Items.SprintArmor) > 0)
                        {
                            if (!self.HasBuff(FakeRoseBuckle)) { return; }
                            self.RemoveBuff(FakeRoseBuckle);
                        }
                    }
                };
            }


            Texture2D CubeBroke = new Texture2D(128, 128, TextureFormat.DXT5, false);
            CubeBroke.LoadImage(Properties.Resources.texBuffBrokenCube, false);
            CubeBroke.filterMode = FilterMode.Bilinear;
            Sprite CubeBrokeS = Sprite.Create(CubeBroke, rec128, half);

            FakeFrozen = ScriptableObject.CreateInstance<BuffDef>();
            FakeFrozen.iconSprite = CubeBrokeS;
            FakeFrozen.buffColor = new Color32(184, 216, 239, 255); //CFDBE0 // B8D8EF
            FakeFrozen.name = "visual_Frozen";
            FakeFrozen.isDebuff = false;
            FakeFrozen.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeFrozen);

            if (FrozenVisual.Value == true)
            {
                On.EntityStates.FrozenState.OnEnter += (orig, self) =>
                {
                    orig(self);

                    if (NetworkServer.active)
                    {
                        for (float num5 = 0; num5 <= self.freezeDuration; num5 = num5 + 0.5f)
                        {
                            self.outer.GetComponentInParent<CharacterBody>().AddTimedBuff(FakeFrozen, (float)num5);
                        }
                    }
                };

                On.EntityStates.FrozenState.OnExit += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        self.outer.GetComponentInParent<CharacterBody>().ClearTimedBuffs(FakeFrozen);
                    }
                };
            }

            // if (SleepVisual.Value == true) {}
            /*
            Texture2D ZZZSleep = new Texture2D(128, 128, TextureFormat.DXT5, false);
            ZZZSleep.LoadImage(Properties.Resources.texBuffSleep, false);
            ZZZSleep.filterMode = FilterMode.Bilinear;
            Sprite ZZZSleepS = Sprite.Create(ZZZSleep, rec128, half);

            BuffDef Intangible = RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/Intangible");
            Intangible.iconSprite = ZZZSleepS;
            Intangible.buffColor = new Color32(240, 226, 239, 255);
            Intangible.name = "IntangibleSleep";
            Intangible.isDebuff = false;
            Intangible.canStack = false;
            */

            On.EntityStates.Croco.Spawn.OnEnter += (orig, self) =>
            {
                orig(self);
                Debug.Log("Poison Doggo is sleep");
                if (NetworkServer.active)
                {
                    self.outer.GetComponentInParent<CharacterBody>().AddBuff(RoR2Content.Buffs.HiddenInvincibility);
                }
            };
            On.EntityStates.Croco.Spawn.OnExit += (orig, self) =>
            {
                orig(self);
                if (NetworkServer.active)
                {
                    var temp = self.outer.GetComponentInParent<CharacterBody>();
                    if (temp)
                    {
                        temp.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
                        temp.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 2.8f);
                    }
                }
            };



            Texture2D HeadStompOn = new Texture2D(128, 128, TextureFormat.DXT5, false);
            HeadStompOn.LoadImage(Properties.Resources.texBuffHeadStompOn, false);
            HeadStompOn.filterMode = FilterMode.Bilinear;
            Sprite HeadStompOnS = Sprite.Create(HeadStompOn, rec128, half);
            Texture2D HeadStompOff = new Texture2D(128, 128, TextureFormat.DXT5, false);
            HeadStompOff.LoadImage(Properties.Resources.texBuffHeadStompOff, false);
            HeadStompOff.filterMode = FilterMode.Bilinear;
            Sprite HeadStompOffS = Sprite.Create(HeadStompOff, rec128, half);

            FakeHeadstompOn = ScriptableObject.CreateInstance<BuffDef>();
            FakeHeadstompOn.iconSprite = HeadStompOnS;
            FakeHeadstompOn.buffColor = new Color32(255, 250, 250, 255);
            FakeHeadstompOn.name = "visual_HeadstomperReady";
            FakeHeadstompOn.isDebuff = false;
            FakeHeadstompOn.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeHeadstompOn);


            FakeHeadstompOff = ScriptableObject.CreateInstance<BuffDef>();
            FakeHeadstompOff.iconSprite = HeadStompOffS;
            FakeHeadstompOff.buffColor = new Color32(250, 250, 255, 255);
            FakeHeadstompOff.name = "visual_HeadstomperCooldown";
            FakeHeadstompOff.isDebuff = false;
            FakeHeadstompOff.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeHeadstompOff);


            if (HeadstomperVisual.Value == true)
            {
                On.EntityStates.Headstompers.BaseHeadstompersState.OnEnter += StupidHeadStomper;



                On.RoR2.Items.HeadstomperBodyBehavior.OnDisable += (orig, self) =>
                {
                    orig(self);
                    if (NetworkServer.active)
                    {
                        if (self.body != null)
                        {
                            self.body.RemoveBuff(FakeHeadstompOn);
                        }
                    }
                    //Debug.Log("Removed Stomper");
                };
            }



            Texture2D FeatherBuff = new Texture2D(128, 128, TextureFormat.DXT5, false);
            FeatherBuff.LoadImage(Properties.Resources.texBuffFeather, false);
            FeatherBuff.filterMode = FilterMode.Bilinear;
            Sprite FeatherBuffS = Sprite.Create(FeatherBuff, rec128, half);

            FakeFeather = ScriptableObject.CreateInstance<BuffDef>();
            FakeFeather.iconSprite = FeatherBuffS;
            FakeFeather.buffColor = new Color32(99, 192, 255, 255); //3FC5E3
            FakeFeather.name = "visual_BonusJump";
            FakeFeather.isDebuff = false;
            FakeFeather.canStack = true;
            R2API.ContentAddition.AddBuffDef(FakeFeather);

            if (FeatherVisual.Value == true)
            {
                On.RoR2.CharacterMotor.OnLanded += (orig, self) =>
                {
                    orig(self);

                    if (NetworkServer.active)
                    {
                        CharacterBody tempbody = self.gameObject.GetComponent<CharacterBody>();
                        if (!tempbody | !tempbody.inventory) { return; }

                        int buffcount = tempbody.GetBuffCount(FakeFeather);
                        int feathercount = tempbody.inventory.GetItemCount(RoR2Content.Items.Feather);


                        if (buffcount > feathercount)
                        {
                            for (var v = buffcount; v > feathercount; v--)
                            {
                                tempbody.RemoveBuff(FakeFeather);
                            };
                        }
                        else
                        {
                            for (var f = buffcount; f < feathercount; f++)
                            {
                                tempbody.AddBuff(FakeFeather);
                            };
                        }
                    }
                };

                On.EntityStates.GenericCharacterMain.ApplyJumpVelocity += (orig, characterMotor, characterBody, horizontalBonus, verticalBonus, vault) =>
                {
                    orig(characterMotor, characterBody, horizontalBonus, verticalBonus, vault);

                    if (NetworkServer.active)
                    {
                        if (characterMotor.jumpCount + 1 > characterBody.baseJumpCount)
                        {
                            characterBody.RemoveBuff(FakeFeather);
                        }
                    }
                };
                /*
                On.RoR2.CharacterBody.OnInventoryChanged += (orig, self) =>
                {
                    if (!self.inventory) { orig(self); }
                    int prefeather = self.inventory.GetItemCount(RoR2Content.Items.Feather);
                    orig(self);
                    if(self.inventory.GetItemCount(RoR2Content.Items.Feather) != prefeather && self.characterMotor.isGrounded)
                    {
                        if (NetworkServer.active)
                        {

                            int buffcount = self.GetBuffCount(FakeFeather);
                            int feathercount = self.inventory.GetItemCount(RoR2Content.Items.Feather);
                            if (buffcount > feathercount)
                            {
                                for (var v = buffcount; v > feathercount; v--)
                                {
                                    self.RemoveBuff(FakeFeather);
                                };
                            }
                            else
                            {
                                for (var f = buffcount; f < feathercount; f++)
                                {
                                    self.AddBuff(FakeFeather);
                                };
                            }
                        }
                    }
                };
                */


            }


            BuffDef MedkitDelay = RoR2.LegacyResourcesAPI.Load<BuffDef>("buffdefs/MedkitHeal");


            FakeShieldDelay = ScriptableObject.CreateInstance<BuffDef>();
            FakeShieldDelay.iconSprite = MedkitDelay.iconSprite;
            FakeShieldDelay.buffColor = new Color32(66, 100, 220, 255); //4264DC
            FakeShieldDelay.name = "visual_ShieldDelay";

            FakeShieldDelay.isDebuff = false;
            FakeShieldDelay.canStack = false;

            R2API.ContentAddition.AddBuffDef(FakeShieldDelay);


            BuffDef bdOutOfCombatArmorBuff = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC1/OutOfCombatArmor/bdOutOfCombatArmorBuff.asset").WaitForCompletion();
            FakeOpalCooldown = ScriptableObject.CreateInstance<BuffDef>();
            FakeOpalCooldown.iconSprite = bdOutOfCombatArmorBuff.iconSprite;
            FakeOpalCooldown.buffColor = new Color(0.4151f, 0.4014f, 0.4014f, 1); //4264DC
            FakeOpalCooldown.name = "visual_OutOfCombatArmorCooldown";
            FakeOpalCooldown.isDebuff = false;
            FakeOpalCooldown.canStack = false;

            R2API.ContentAddition.AddBuffDef(FakeOpalCooldown);


            if (ShieldRegenVisual.Value == true)
            {
                On.RoR2.HealthComponent.UpdateLastHitTime += (orig, self, damageValue, damagePosition, damageIsSilent, attacker) =>
                {
                    orig(self, damageValue, damagePosition, damageIsSilent, attacker);
                    if (NetworkServer.active && self.body && self.body.inventory && damageValue > 0f)
                    {
                        if (self.fullShield > 0 && self.shield < self.fullShield)
                        {
                            self.body.AddTimedBuff(FakeShieldDelay, 7);
                        }
                        if (self.body.inventory.GetItemCount(DLC1Content.Items.OutOfCombatArmor) > 0)
                        {
                            self.body.AddTimedBuff(FakeOpalCooldown, 7);
                        }
                    }
                };




                On.RoR2.HealthComponent.ForceShieldRegen += (orig, self) =>
                {
                    orig(self);
                    if (self.body && self.body.HasBuff(FakeShieldDelay))
                    {
                        self.body.ClearTimedBuffs(FakeShieldDelay);
                    }
                };

            }

            if (MessagesElixirWatches.Value == true)
            {
                On.RoR2.HealthComponent.UpdateLastHitTime += (orig, self, damageValue, damagePosition, damageIsSilent, attacker) =>
                {
                    int watchcount = 0;
                    bool elixirdunk = false;
                    if (NetworkServer.active && damageValue > 0f && self.body && self.body.isPlayerControlled && self.isHealthLow)
                    {
                        watchcount += self.itemCounts.fragileDamageBonus;
                        if (self.itemCounts.healingPotion > 0)
                        {
                            elixirdunk = true;
                        }
                    }
                    orig(self, damageValue, damagePosition, damageIsSilent, attacker);

                    if (elixirdunk)
                    {
                        string token = "<style=cEvent>" + Util.GetBestBodyName(self.body.gameObject) + " drank an <color=#FFFFFF>Elixir</color>";
                        if (watchcount > 0 && self.itemCounts.fragileDamageBonus == 0)
                        {
                            token += " and broke their <color=#FFFFFF>Delicate Watch";
                            if (watchcount > 1) { token += "es</color>(" + watchcount + ")"; }

                        }
                        token += "</style>";
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                    }
                    else if (watchcount > 0 && self.itemCounts.fragileDamageBonus == 0)
                    {
                        string token = "<style=cEvent>" + Util.GetBestBodyName(self.body.gameObject) + " broke their <color=#FFFFFF>Delicate Watch";
                        if (watchcount > 1) { token += "es</color>(" + watchcount + ")" + "</style>"; }
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                    }

                };
            }




            Texture2D EggIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            EggIcon.LoadImage(Properties.Resources.texBuffEgg, false);
            EggIcon.filterMode = FilterMode.Bilinear;
            Sprite EggIconS = Sprite.Create(EggIcon, rec128, half);

            FakeEgg = ScriptableObject.CreateInstance<BuffDef>();
            FakeEgg.iconSprite = EggIconS;
            //FakeEgg.buffColor = new Color32(255, 157, 174, 255); //FF9DAE
            FakeEgg.buffColor = new Color32(255, 180, 137, 255); //FFB489
            FakeEgg.name = "visual_VolcanoEgg";
            FakeEgg.isDebuff = false;
            FakeEgg.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeEgg);

            

            if (EggVisualA.Value == true)
            { 
                On.RoR2.FireballVehicle.OnPassengerEnter += (orig, self, passenger) =>
                    {
                        orig(self, passenger);
                        if (NetworkServer.active)
                        {
                            passenger.GetComponent<CharacterBody>().AddTimedBuff(FakeEgg, self.duration);
                        }
                    };
                On.RoR2.FireballVehicle.FixedUpdate += (orig, self) =>
                {
                    if (NetworkServer.active)
                    {
                        if ((self.overlapFireAge + Time.fixedDeltaTime) > 1f / (self.overlapFireFrequency += Time.fixedDeltaTime))
                        {
                            self.vehicleSeat.currentPassengerBody.AddTimedBuff(FakeEgg, self.duration - self.age);
                        }
                    }
                    orig(self);
                };

                On.RoR2.FireballVehicle.OnPassengerExit += (orig, self, passenger) =>
                {
                    orig(self, passenger);
                    if (NetworkServer.active)
                    {
                        passenger.GetComponent<CharacterBody>().ClearTimedBuffs(FakeEgg);
                    }
                };
            }


            Texture2D TinctureIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TinctureIcon.LoadImage(Properties.Resources.texBuffTincture, false);
            TinctureIcon.filterMode = FilterMode.Bilinear;
            Sprite TinctureIconS = Sprite.Create(TinctureIcon, rec128, half);

            FakeHellFireDuration = ScriptableObject.CreateInstance<BuffDef>();
            FakeHellFireDuration.iconSprite = TinctureIconS;
            FakeHellFireDuration.buffColor = new Color32(115, 182, 165, 255); //
            FakeHellFireDuration.name = "visual_TinctureIgnition";
            FakeHellFireDuration.isDebuff = false;
            FakeHellFireDuration.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeHellFireDuration);

            if (TinctureVisual.Value == true)
            {
                On.RoR2.CharacterBody.AddHelfireDuration += (orig, self, duration) =>
                {
                    orig(self, duration);
                    if (NetworkServer.active)
                    {
                        self.AddTimedBuff(FakeHellFireDuration, duration);
                    }
                };

            }


            Texture2D FrostRelicIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            FrostRelicIcon.LoadImage(Properties.Resources.texBuffFrostRelic, false);
            FrostRelicIcon.filterMode = FilterMode.Bilinear;
            Sprite FrostRelicIconS = Sprite.Create(FrostRelicIcon, rec128, half);

            FakeFrostRelic = ScriptableObject.CreateInstance<BuffDef>();
            FakeFrostRelic.iconSprite = FrostRelicIconS;
            FakeFrostRelic.buffColor = new Color32(202, 229, 255, 255); //CAE5FF
            FakeFrostRelic.name = "visual_FrostRelicGrowth";
            FakeFrostRelic.isDebuff = false;
            FakeFrostRelic.canStack = true;

            R2API.ContentAddition.AddBuffDef(FakeFrostRelic);

            if (FrostRelicVisual.Value == true)
            {
                On.RoR2.IcicleAuraController.OnOwnerKillOther += (orig, self) =>
                {
                    orig(self);

                    if (NetworkServer.active)
                    {
                        CharacterBody tempbod = self.owner.GetComponent<CharacterBody>();
                        tempbod.ClearTimedBuffs(FakeFrostRelic);

                        List<Single> templist = self.icicleLifetimes;

                        for (int i = templist.Count - 1; i > -1; i--)
                        {
                            tempbod.AddTimedBuff(FakeFrostRelic, templist[i]);
                            if (tempbod.GetBuffCount(FakeFrostRelic) >= (tempbod.inventory.GetItemCount(RoR2Content.Items.Icicle) * 6))
                            {
                                return;
                            }
                        }
                    }
                };

            }




            Texture2D texBuffShuriken = new Texture2D(128, 128, TextureFormat.DXT5, false);
            texBuffShuriken.LoadImage(Properties.Resources.texBuffShuriken, false);
            texBuffShuriken.filterMode = FilterMode.Bilinear;
            Sprite texBuffShurikenS = Sprite.Create(texBuffShuriken, rec128, half);


            FakeShurikenStock = ScriptableObject.CreateInstance<BuffDef>();
            FakeShurikenStock.iconSprite = texBuffShurikenS;
            FakeShurikenStock.buffColor = new Color32(202, 202, 202, 255); //CAE5FF
            FakeShurikenStock.name = "visual_FakeShurikenStock";
            FakeShurikenStock.isDebuff = false;
            FakeShurikenStock.canStack = true;

            R2API.ContentAddition.AddBuffDef(FakeShurikenStock);



            BuffDef bdOverheat = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/Base/Grandparent/bdOverheat.asset").WaitForCompletion();
            FakeVagrantExplosion = ScriptableObject.CreateInstance<BuffDef>();
            FakeVagrantExplosion.iconSprite = bdOverheat.iconSprite;
            //FakeVagrantExplosion.buffColor = new Color(0.8f, 1f, 1f, 1); //4264DC
            FakeVagrantExplosion.buffColor = new Color32(189, 229, 252, 255); //4264DC
            FakeVagrantExplosion.name = "visual_ImpendingVagrantExplosion";
            FakeVagrantExplosion.isDebuff = false;
            FakeVagrantExplosion.canStack = false;
            R2API.ContentAddition.AddBuffDef(FakeVagrantExplosion);

        }




        public void CreateUsedKey()
        {
            ItemDef ExtraLifeConsumed = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/ExtraLifeConsumed");
            ItemDef TreasureCacheVoid = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCacheVoid");

            Texture2D TexUsedRustedKey = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexUsedRustedKey.LoadImage(Properties.Resources.texItemUsedKey, false);
            TexUsedRustedKey.filterMode = FilterMode.Bilinear;
            TexUsedRustedKey.wrapMode = TextureWrapMode.Clamp;
            Sprite TexUsedRustedKeyS = Sprite.Create(TexUsedRustedKey, rec128, half);

            ItemDef UsedRustedKey = ScriptableObject.CreateInstance<ItemDef>();

            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Rusted Key (Consumed)", "en");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Verrosteter Schlüssel (Verbraucht)", "de");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Clé rouillée (utilisé)", "FR");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Chiave arrugginita (Consumato)", "IT");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Llave oxidada (consumido)", "es-419");


            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_DESC", "A spent key to remember an item well earned.", "en");

            UsedRustedKey.name = "TreasureCacheConsumed";
            UsedRustedKey.deprecatedTier = ItemTier.NoTier;
            UsedRustedKey.pickupModelPrefab = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCache").pickupModelPrefab;
            UsedRustedKey.pickupIconSprite = TexUsedRustedKeyS;
            UsedRustedKey.nameToken = "ITEM_TREASURECACHECONSUMED_NAME";
            UsedRustedKey.pickupToken = "ITEM_TREASURECACHECONSUMED_DESC";
            UsedRustedKey.descriptionToken = "ITEM_TREASURECACHECONSUMED_DESC";
            UsedRustedKey.loreToken = "";
            UsedRustedKey.hidden = false;
            UsedRustedKey.canRemove = false;
            UsedRustedKey.tags = new ItemTag[]
            {
                ItemTag.WorldUnique
            };
            CustomItem customItem = new CustomItem(UsedRustedKey, new ItemDisplayRule[0]);
            ItemAPI.Add(customItem);



            Texture2D texItemUsedKeyVoid = new Texture2D(128, 128, TextureFormat.DXT5, false);
            texItemUsedKeyVoid.LoadImage(Properties.Resources.texItemUsedKeyVoid, false);
            texItemUsedKeyVoid.filterMode = FilterMode.Bilinear;
            texItemUsedKeyVoid.wrapMode = TextureWrapMode.Clamp;
            Sprite texItemUsedKeyVoidS = Sprite.Create(texItemUsedKeyVoid, rec128, half);

            ItemDef UsedEncrustedKey = ScriptableObject.CreateInstance<ItemDef>();

            LanguageAPI.Add("ITEM_TREASURECACHEVOIDCONSUMED_NAME", "Encrusted Key (Consumed)", "en");
            LanguageAPI.Add("ITEM_TREASURECACHEVOIDCONSUMED_DESC", "A spent key to remember an item well earned.", "en");

            UsedEncrustedKey.name = "TreasureCacheVoidConsumed";
            UsedEncrustedKey.deprecatedTier = ItemTier.NoTier;
            UsedEncrustedKey.pickupModelPrefab = TreasureCacheVoid.pickupModelPrefab;
            UsedEncrustedKey.pickupIconSprite = texItemUsedKeyVoidS;
            UsedEncrustedKey.nameToken = "ITEM_TREASURECACHEVOIDCONSUMED_NAME";
            UsedEncrustedKey.pickupToken = "ITEM_TREASURECACHEVOIDCONSUMED_DESC";
            UsedEncrustedKey.descriptionToken = "ITEM_TREASURECACHEVOIDCONSUMED_DESC";
            UsedEncrustedKey.loreToken = "";
            UsedEncrustedKey.hidden = false;
            UsedEncrustedKey.canRemove = false;
            UsedEncrustedKey.tags = new ItemTag[]
            {
                ItemTag.WorldUnique
            };
            CustomItem customItem2 = new CustomItem(UsedEncrustedKey, new ItemDisplayRule[0]);
            ItemAPI.Add(customItem2);




            On.RoR2.PurchaseInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                orig(self, activator);
                if (self.costType == CostTypeIndex.TreasureCacheItem)
                {
                    if (self.Networkavailable == false)
                    {
                        Debug.Log("TreasureCacheCount " + SceneInfoTreasureCacheCount);
                        GenericObjectiveProvider[] tempobjectives = SceneInfo.instance.GetComponents<GenericObjectiveProvider>();
                        for (int i = 0; i < tempobjectives.Length; i++)
                        {
                            if (tempobjectives[i].objectiveToken.Contains("Rusty Lockbox"))
                            {
                                if (SceneInfoTreasureCacheCount > 0)
                                {
                                    string old = "(" + SceneInfoTreasureCacheCount + "/";
                                    SceneInfoTreasureCacheCount--;
                                    string newstring = "(" + SceneInfoTreasureCacheCount + "/";
                                    tempobjectives[i].objectiveToken = tempobjectives[i].objectiveToken.Replace(old, newstring);
                                }
                                if (SceneInfoTreasureCacheCount == 0)
                                {
                                    Destroy(tempobjectives[i]);
                                }
                            }
                        }
                        if (NetworkServer.active)
                        {
                            activator.GetComponent<CharacterBody>().inventory.GiveItem(UsedRustedKey, self.cost);
                            CharacterMasterNotificationQueue.PushItemTransformNotification(activator.GetComponent<CharacterBody>().master, RoR2Content.Items.TreasureCache.itemIndex, UsedRustedKey.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                        }
                    }

                }
                else if (self.costType == CostTypeIndex.TreasureCacheVoidItem)
                {

                    if (self.Networkavailable == false)
                    {
                        Debug.Log("TreasureCacheVoidCount " + SceneInfoTreasureCacheVoidCount);
                        GenericObjectiveProvider[] tempobjectives = SceneInfo.instance.GetComponents<GenericObjectiveProvider>();
                        for (int i = 0; i < tempobjectives.Length; i++)
                        {
                            //Unlock the<color=#FF9EEC>Encrusted Lockbox</color>
                            if (tempobjectives[i].objectiveToken.Contains("Encrusted Lockbox"))
                            {
                                if (SceneInfoTreasureCacheVoidCount > 0)
                                {
                                    string old = "(" + SceneInfoTreasureCacheVoidCount + "/";
                                    SceneInfoTreasureCacheVoidCount--;
                                    string newstring = "(" + SceneInfoTreasureCacheVoidCount + "/";
                                    tempobjectives[i].objectiveToken = tempobjectives[i].objectiveToken.Replace(old, newstring);
                                }
                                if (SceneInfoTreasureCacheVoidCount == 0)
                                {
                                    Destroy(tempobjectives[i]);
                                }
                            }
                        }
                        if (NetworkServer.active)
                        {
                            activator.GetComponent<CharacterBody>().inventory.GiveItem(UsedEncrustedKey, self.cost);
                            CharacterMasterNotificationQueue.PushItemTransformNotification(activator.GetComponent<CharacterBody>().master, DLC1Content.Items.TreasureCacheVoid.itemIndex, UsedEncrustedKey.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                        }
                    }

                }

            };


            On.RoR2.MultiShopController.OnPurchase += (orig, self, interactor, purchaseInteraction) =>
            {
                orig(self, interactor, purchaseInteraction);

                //Debug.LogWarning(self);

                if (!self.Networkavailable && self.name.StartsWith("FreeChestMultiShop(Clone)"))
                {
                    Debug.Log("FreeChestCount " + SceneInfoFreeChestCount);
                    GenericObjectiveProvider[] tempobjectives = SceneInfo.instance.GetComponents<GenericObjectiveProvider>();
                    for (int i = 0; i < tempobjectives.Length; i++)
                    {
                        if (tempobjectives[i].objectiveToken.StartsWith("Collect free"))
                        {
                            if (SceneInfoFreeChestCount > 0)
                            {
                                string old = "(" + SceneInfoFreeChestCount + "/";
                                SceneInfoFreeChestCount--;
                                string newstring = "(" + SceneInfoFreeChestCount + "/";
                                tempobjectives[i].objectiveToken = tempobjectives[i].objectiveToken.Replace(old, newstring);
                            }
                            if (SceneInfoFreeChestCount == 0)
                            {
                                Destroy(tempobjectives[i]);
                            }
                        }
                    }
                }
            };



            Texture2D TexUsedArtifactKey = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexUsedArtifactKey.LoadImage(Properties.Resources.texItemUsedArtifactKey, false);
            TexUsedArtifactKey.filterMode = FilterMode.Bilinear;
            TexUsedArtifactKey.wrapMode = TextureWrapMode.Clamp;
            Sprite TexUsedArtifactKeyS = Sprite.Create(TexUsedArtifactKey, rec128, half);

            ItemDef UsedArtifactKey = ScriptableObject.CreateInstance<ItemDef>();

            LanguageAPI.Add("ITEM_ARTIFACTKEYCONSUMED_NAME", "Artifact Key (Consumed)", "en");

            UsedArtifactKey.name = "ArtifactKeyConsumed";
            UsedArtifactKey.tier = ItemTier.NoTier;
            UsedArtifactKey.pickupModelPrefab = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/ArtifactKey").pickupModelPrefab;
            UsedArtifactKey.pickupIconSprite = TexUsedArtifactKeyS;
            UsedArtifactKey.nameToken = "ITEM_ARTIFACTKEYCONSUMED_NAME";
            UsedArtifactKey.pickupToken = ExtraLifeConsumed.pickupToken;
            UsedArtifactKey.descriptionToken = ExtraLifeConsumed.descriptionToken;
            UsedArtifactKey.loreToken = ExtraLifeConsumed.loreToken;
            UsedArtifactKey.hidden = false;
            UsedArtifactKey.canRemove = false;
            UsedArtifactKey.tags = new ItemTag[]
            {
                ItemTag.WorldUnique
            };
            //CustomItem customItem2 = new CustomItem(UsedArtifactKey, new ItemDisplayRule[0]);
            //ItemAPI.Add(customItem2);



        }




        public void StageStartMethod(On.RoR2.Stage.orig_Start orig, Stage self)
        {
            orig(self);
            GameEndingDamageReports.Clear();
            GameOverInventories.Clear();
            GameOverBodyNames.Clear();


            if (ColorfulBalls.Value == true)
            {
                if (DidEliteBrassBalls == true)
                {
                    DidEliteBrassBalls = false;
                    On.RoR2.Projectile.ProjectileManager.FireProjectile_GameObject_Vector3_Quaternion_GameObject_float_float_bool_DamageColorIndex_GameObject_float -= SkinChanges.ColorfulBellBalls;
                    On.EntityStates.Bell.BellWeapon.ChargeTrioBomb.OnEnter += SkinChanges.ColorfulBellBallsInit;
                }

            }



            if (Stage.instance && Stage.instance.sceneDef.destinations.Length > 0)
            {
                prevDestinations = Stage.instance.sceneDef.destinations;
            }

            if (NetworkServer.active)
            {
                if (FasterPrinter.Value == true)
                {
                    typeof(EntityStates.Duplicator.Duplicating).SetFieldValue("initialDelayDuration", 0.6f);  //Default 1.5
                    typeof(EntityStates.Duplicator.Duplicating).SetFieldValue("timeBetweenStartAndDropDroplet", 1.25f); //Default 1.33
                }
                if (FasterScrapper.Value == true)
                {
                    typeof(EntityStates.Scrapper.WaitToBeginScrapping).SetFieldValue("duration", 0.9f);  //Default 1.5
                    typeof(EntityStates.Scrapper.Scrapping).SetFieldValue("duration", 1f); //Default 2
                    typeof(EntityStates.Scrapper.ScrappingToIdle).SetFieldValue("duration", 0.4f); //Default 1
                }

            }
        }

    
        public static void StupidHeadStomper(On.EntityStates.Headstompers.BaseHeadstompersState.orig_OnEnter orig, global::EntityStates.Headstompers.BaseHeadstompersState self)
        {
            orig(self);

            var tempbody = self.outer.GetComponentInParent<CharacterBody>();
            if (!tempbody) { return; }
            if (self.ToString().StartsWith("EntityStates.Headstompers.HeadstompersIdle"))
            {
                if (NetworkServer.active)
                {
                    while (tempbody.GetBuffCount(FakeHeadstompOn) == 0)
                    {
                        tempbody.AddBuff(FakeHeadstompOn);
                    }
                    tempbody.ClearTimedBuffs(FakeHeadstompOff);
                }
            }

            if (self.ToString().StartsWith("EntityStates.Headstompers.HeadstompersCooldown"))
            {
                if (NetworkServer.active)
                {
                    while (tempbody.GetBuffCount(FakeHeadstompOn) != 0)
                    {
                        tempbody.RemoveBuff(FakeHeadstompOn);
                    }
                    for (float newtimer = (float)10 / tempbody.inventory.GetItemCount(RoR2Content.Items.FallBoots); 0 < newtimer; newtimer--)
                    {
                        tempbody.AddTimedBuff(FakeHeadstompOff, (float)newtimer);
                    }
                }
            }

        }


        public static void StupidColorChanger(On.RoR2.Run.orig_Start orig, Run self)
        {
            orig(self);







            Debug.LogWarning("Changing Colors");

            if (ChangedColors == false)
            {
                On.RoR2.Run.Start -= StupidColorChanger;
                ChangedColors = true;

                //7DA0F0
                //GameObject BlueOrb = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("ItemIndex.LunarDagger")).dropletDisplayPrefab;
                //GameObject YellowOrb = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("ItemIndex.Knurl")).dropletDisplayPrefab;
                GameObject OrangeOrb = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("EquipmentIndex.Fruit")).dropletDisplayPrefab;

                GameObject YellowOrb = EquipmentBossOrb;
                GameObject BlueOrb = EquipmentLunarOrb;



                PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).baseColor = FakeLunarCoin;
                PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).darkColor = FakeLunarCoin;
                //PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).dropletDisplayPrefab = CoinOrb;

                int TotalItemCount = ItemCatalog.itemCount;
                int TotalEquipmentCount = EquipmentCatalog.equipmentCount;

                for (int i = 0; i < TotalItemCount; i++)
                {
                    string tempname = ItemCatalog.GetItemDef((ItemIndex)i).name;
                    string tempindexname = ("ItemIndex." + tempname);

                    if (ItemCatalog.GetItemDef((ItemIndex)i).tier == ItemTier.NoTier)
                    {
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).dropletDisplayPrefab = NoTierOrb;
                    }
                    else if (ItemCatalog.GetItemDef((ItemIndex)i).tier == ItemTier.VoidTier1)
                    {
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).baseColor = FakeVoidWhite;
                    }
                    else if (ItemCatalog.GetItemDef((ItemIndex)i).tier == ItemTier.VoidTier3)
                    {
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).baseColor = FakeVoidRed;
                    }
                    else if (ItemCatalog.GetItemDef((ItemIndex)i).tier == ItemTier.VoidBoss)
                    {
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).baseColor = FakeVoidYellow;
                    }
                }

                for (int i = 0; i < TotalEquipmentCount; i++)
                {

                    string tempname = EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).name;
                    string tempindexname = ("EquipmentIndex." + tempname);
                    EquipmentDef tempequipdef = EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i);


                    if (EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).isBoss == true)
                    {
                        if (ChangeDropletColor == true)
                        {
                            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).dropletDisplayPrefab = YellowOrb;
                        }
                    }

                    if (tempname.Contains("Affix") || tempname.Contains("AFFIX") || tempname.StartsWith("Elite"))
                    {

                        if (YellowEliteEquip.Value == true)
                        {
                            EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).colorIndex = ColorCatalog.ColorIndex.BossItem;
                            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).isBoss = true;
                            if (ChangeDropletColor == true)
                            {
                                PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).dropletDisplayPrefab = YellowOrb;
                            }
                        }
                        if (EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).isLunar == true)
                        {
                            if (LunarYellowEliteEquipTexture.Value == true)
                            {
                                EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).isBoss = true;
                                EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).colorIndex = ColorCatalog.ColorIndex.BossItem;
                                if (ChangeDropletColor == true)
                                {
                                    PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).dropletDisplayPrefab = YellowOrb;
                                }
                                PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).baseColor = FakeYellowEquip;
                                PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).darkColor = FakeYellowEquip;
                            }
                            else
                            {
                                EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).colorIndex = ColorCatalog.ColorIndex.LunarItem;
                                if (ChangeDropletColor == true)
                                {
                                    PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).dropletDisplayPrefab = BlueOrb;
                                }
                                PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).baseColor = FakeBlueEquip;
                                PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).darkColor = FakeBlueEquip;

                            }


                        }
                        else
                        {
                            EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).isBoss = true;
                            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).baseColor = FakeYellowEquip;
                            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).darkColor = FakeYellowEquip;
                        }


                        if (tempequipdef.pickupModelPrefab && ColorfulBalls.Value == true)
                        {
                            MeshRenderer tempmeshrender = tempequipdef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>();
                            if (tempmeshrender)
                            {
                                tempmeshrender.material.enableInstancing = false;
                                tempmeshrender.material.DisableKeyword("FORCE_SPEC");

                                if (tempname.StartsWith("EliteVoidEquipment"))
                                {
                                    tempmeshrender.material.DisableKeyword("FLOWMAP");
                                    tempmeshrender.material.DisableKeyword("FRESNEL_EMISSION");
                                }

                            }
                        }

                    }
                    else if (EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).isBoss == true)
                    {
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).baseColor = FakeYellowEquip;
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).darkColor = FakeYellowEquip;
                        if (ChangeDropletColor == true)
                        {
                            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).dropletDisplayPrefab = YellowOrb;
                        }
                    }
                    else if (EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i).isLunar == true)
                    {
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).isLunar = true;
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).baseColor = FakeBlueEquip;
                        PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).darkColor = FakeBlueEquip;
                        if (ChangeDropletColor == true)
                        {
                            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname)).dropletDisplayPrefab = BlueOrb;
                        }
                    }


                }

            }
        }


        public static void YellowIconAdder()
        {
            if (EquipmentCatalog.FindEquipmentIndex("AffixBlighted") != EquipmentIndex.None)
            {
                Texture2D LoTEliteBlightedTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                LoTEliteBlightedTex.LoadImage(Properties.Resources.AffixLostTranistBlighted, false);
                LoTEliteBlightedTex.filterMode = FilterMode.Bilinear;
                LoTEliteBlightedTex.wrapMode = TextureWrapMode.Clamp;
                Sprite LoTEliteBlighted = Sprite.Create(LoTEliteBlightedTex, rec128, half);
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("AffixBlighted")).pickupIconSprite = LoTEliteBlighted;
            }
            if (EquipmentCatalog.FindEquipmentIndex("AffixLeeching") != EquipmentIndex.None)
            {
                Texture2D LoTEliteLeechTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                LoTEliteLeechTex.LoadImage(Properties.Resources.AffixLostTransitLeech, false);
                LoTEliteLeechTex.filterMode = FilterMode.Bilinear;
                LoTEliteLeechTex.wrapMode = TextureWrapMode.Clamp;
                Sprite LoTEliteLeech = Sprite.Create(LoTEliteLeechTex, rec128, half);
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("AffixLeeching")).pickupIconSprite = LoTEliteLeech;
            }
            if (EquipmentCatalog.FindEquipmentIndex("AffixFrenzied") != EquipmentIndex.None)
            {
                Texture2D LoTEliteFrenziedTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                LoTEliteFrenziedTex.LoadImage(Properties.Resources.AffixLostTransitFrenzied, false);
                LoTEliteFrenziedTex.filterMode = FilterMode.Bilinear;
                LoTEliteFrenziedTex.wrapMode = TextureWrapMode.Clamp;
                Sprite LoTEliteFrenzied = Sprite.Create(LoTEliteFrenziedTex, rec128, half);
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("AffixFrenzied")).pickupIconSprite = LoTEliteFrenzied;
            }
            if (EquipmentCatalog.FindEquipmentIndex("AffixVolatile") != EquipmentIndex.None)
            {
                Texture2D LoTEliteVolatileTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                LoTEliteVolatileTex.LoadImage(Properties.Resources.AffixLostTransitVolatile, false);
                LoTEliteVolatileTex.filterMode = FilterMode.Bilinear;
                LoTEliteVolatileTex.wrapMode = TextureWrapMode.Clamp;
                Sprite LoTEliteVolatile = Sprite.Create(LoTEliteVolatileTex, rec128, half);
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("AffixVolatile")).pickupIconSprite = LoTEliteVolatile;
            }

            if (EquipmentCatalog.FindEquipmentIndex("EliteVariety_AffixArmored") != EquipmentIndex.None)
            {
                Texture2D VarietyEliteArmoredTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                VarietyEliteArmoredTex.LoadImage(Properties.Resources.AffixVarietyArmor, false);
                VarietyEliteArmoredTex.filterMode = FilterMode.Bilinear;
                VarietyEliteArmoredTex.wrapMode = TextureWrapMode.Clamp;
                Sprite VarietyEliteArmored = Sprite.Create(VarietyEliteArmoredTex, rec128, half);

                Texture2D VarietyEliteBannerTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                VarietyEliteBannerTex.LoadImage(Properties.Resources.AffixVarietyBanner, false);
                VarietyEliteBannerTex.filterMode = FilterMode.Bilinear;
                VarietyEliteBannerTex.wrapMode = TextureWrapMode.Clamp;
                Sprite VarietyEliteBanner = Sprite.Create(VarietyEliteBannerTex, rec128, half);

                Texture2D VarietyEliteGoldTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                VarietyEliteGoldTex.LoadImage(Properties.Resources.AffixVarietyGold, false);
                VarietyEliteGoldTex.filterMode = FilterMode.Bilinear;
                VarietyEliteGoldTex.wrapMode = TextureWrapMode.Clamp;
                Sprite VarietyEliteGold = Sprite.Create(VarietyEliteGoldTex, rec128, half);

                Texture2D VarietyEliteSandTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                VarietyEliteSandTex.LoadImage(Properties.Resources.AffixVarietySandstorm, false);
                VarietyEliteSandTex.filterMode = FilterMode.Bilinear;
                VarietyEliteSandTex.wrapMode = TextureWrapMode.Clamp;
                Sprite VarietyEliteSand = Sprite.Create(VarietyEliteSandTex, rec128, half);

                Texture2D VarietyEliteImpTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                VarietyEliteImpTex.LoadImage(Properties.Resources.AffixVarietyImp, false);
                VarietyEliteImpTex.filterMode = FilterMode.Bilinear;
                VarietyEliteImpTex.wrapMode = TextureWrapMode.Clamp;
                Sprite VarietyEliteImp = Sprite.Create(VarietyEliteImpTex, rec128, half);

                Texture2D VarietyEliteTinkeringTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                VarietyEliteTinkeringTex.LoadImage(Properties.Resources.AffixVarietyTinker, false);
                VarietyEliteTinkeringTex.filterMode = FilterMode.Bilinear;
                VarietyEliteTinkeringTex.wrapMode = TextureWrapMode.Clamp;
                Sprite VarietyEliteTinkering = Sprite.Create(VarietyEliteTinkeringTex, rec128, half);



                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EliteVariety_AffixArmored")).pickupIconSprite = VarietyEliteArmored;
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EliteVariety_AffixBuffing")).pickupIconSprite = VarietyEliteBanner;
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EliteVariety_AffixImpPlane")).pickupIconSprite = VarietyEliteImp;
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EliteVariety_AffixPillaging")).pickupIconSprite = VarietyEliteGold;
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EliteVariety_AffixSandstorm")).pickupIconSprite = VarietyEliteSand;
                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EliteVariety_AffixTinkerer")).pickupIconSprite = VarietyEliteTinkering;
            }

            if (EquipmentCatalog.FindEquipmentIndex("AETHERIUM_ELITE_EQUIPMENT_AFFIX_SANGUINE") != EquipmentIndex.None)
            {
                Texture2D AetherEliteBloodTex = new Texture2D(128, 128, TextureFormat.DXT5, false);
                AetherEliteBloodTex.LoadImage(Properties.Resources.AffixAetheriumSanguine, false);
                AetherEliteBloodTex.filterMode = FilterMode.Bilinear;
                AetherEliteBloodTex.wrapMode = TextureWrapMode.Clamp;
                Sprite AetherEliteBlood = Sprite.Create(AetherEliteBloodTex, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("AETHERIUM_ELITE_EQUIPMENT_AFFIX_SANGUINE")).pickupIconSprite = AetherEliteBlood;
            }

            if (EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_CLAYFLOWER") != EquipmentIndex.None)
            {
                Texture2D EQUIPMENT_CLAYFLOWER = new Texture2D(128, 128, TextureFormat.DXT5, false);
                EQUIPMENT_CLAYFLOWER.LoadImage(Properties.Resources.EQUIPMENT_CLAYFLOWER, false);
                EQUIPMENT_CLAYFLOWER.filterMode = FilterMode.Bilinear;
                EQUIPMENT_CLAYFLOWER.wrapMode = TextureWrapMode.Clamp;
                Sprite EQUIPMENT_CLAYFLOWERS = Sprite.Create(EQUIPMENT_CLAYFLOWER, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_CLAYFLOWER")).pickupIconSprite = EQUIPMENT_CLAYFLOWERS;
            }

            if (EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_LUNARTELEPORT") != EquipmentIndex.None)
            {
                Texture2D EQUIPMENT_LUNARTELEPORT = new Texture2D(128, 128, TextureFormat.DXT5, false);
                EQUIPMENT_LUNARTELEPORT.LoadImage(Properties.Resources.EQUIPMENT_LUNARTELEPORT, false);
                EQUIPMENT_LUNARTELEPORT.filterMode = FilterMode.Bilinear;
                EQUIPMENT_LUNARTELEPORT.wrapMode = TextureWrapMode.Clamp;
                Sprite EQUIPMENT_LUNARTELEPORTS = Sprite.Create(EQUIPMENT_LUNARTELEPORT, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_LUNARTELEPORT")).pickupIconSprite = EQUIPMENT_LUNARTELEPORTS;
            }

            if (EquipmentCatalog.FindEquipmentIndex("RuinaBackwardsClock") != EquipmentIndex.None)
            {
                Texture2D RuinaBackwardsClock = new Texture2D(128, 128, TextureFormat.DXT5, false);
                RuinaBackwardsClock.LoadImage(Properties.Resources.RuinaBackwardsClock, false);
                RuinaBackwardsClock.filterMode = FilterMode.Bilinear;
                RuinaBackwardsClock.wrapMode = TextureWrapMode.Clamp;
                Sprite RuinaBackwardsClockS = Sprite.Create(RuinaBackwardsClock, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("RuinaBackwardsClock")).pickupIconSprite = RuinaBackwardsClockS;
            }
            //
            //Tusnami
            if (EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_TSUNAMI_EVSD") != EquipmentIndex.None)
            {
                Texture2D EQUIPMENT_TSUNAMI_EVSD = new Texture2D(128, 128, TextureFormat.DXT5, false);
                EQUIPMENT_TSUNAMI_EVSD.LoadImage(Properties.Resources.EQUIPMENT_TSUNAMI_EVSD, false);
                EQUIPMENT_TSUNAMI_EVSD.filterMode = FilterMode.Bilinear;
                EQUIPMENT_TSUNAMI_EVSD.wrapMode = TextureWrapMode.Clamp;
                Sprite EQUIPMENT_TSUNAMI_EVSDS = Sprite.Create(EQUIPMENT_TSUNAMI_EVSD, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_TSUNAMI_EVSD")).pickupIconSprite = EQUIPMENT_TSUNAMI_EVSDS;
            }

            if (EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_TSUNAMI_VOID_CLAM") != EquipmentIndex.None)
            {
                Texture2D EQUIPMENT_TSUNAMI_VOID_CLAM = new Texture2D(128, 128, TextureFormat.DXT5, false);
                EQUIPMENT_TSUNAMI_VOID_CLAM.LoadImage(Properties.Resources.EQUIPMENT_TSUNAMI_VOID_CLAM, false);
                EQUIPMENT_TSUNAMI_VOID_CLAM.filterMode = FilterMode.Bilinear;
                EQUIPMENT_TSUNAMI_VOID_CLAM.wrapMode = TextureWrapMode.Clamp;
                Sprite EQUIPMENT_TSUNAMI_VOID_CLAMS = Sprite.Create(EQUIPMENT_TSUNAMI_VOID_CLAM, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_TSUNAMI_VOID_CLAM")).pickupIconSprite = EQUIPMENT_TSUNAMI_VOID_CLAMS;
            }
            //
            //MysticItems
            if (EquipmentCatalog.FindEquipmentIndex("MysticsItems_FragileMask") != EquipmentIndex.None)
            {
                Texture2D MysticsItems_FragileMask = new Texture2D(128, 128, TextureFormat.DXT5, false);
                MysticsItems_FragileMask.LoadImage(Properties.Resources.MysticsItems_FragileMask, false);
                MysticsItems_FragileMask.filterMode = FilterMode.Bilinear;
                MysticsItems_FragileMask.wrapMode = TextureWrapMode.Clamp;
                Sprite MysticsItems_FragileMaskS = Sprite.Create(MysticsItems_FragileMask, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("MysticsItems_FragileMask")).pickupIconSprite = MysticsItems_FragileMaskS;
            }

            if (EquipmentCatalog.FindEquipmentIndex("MysticsItems_GateChalice") != EquipmentIndex.None)
            {
                Texture2D MysticsItems_GateChalice = new Texture2D(128, 128, TextureFormat.DXT5, false);
                MysticsItems_GateChalice.LoadImage(Properties.Resources.MysticsItems_GateChalice, false);
                MysticsItems_GateChalice.filterMode = FilterMode.Bilinear;
                MysticsItems_GateChalice.wrapMode = TextureWrapMode.Clamp;
                Sprite MysticsItems_GateChaliceS = Sprite.Create(MysticsItems_GateChalice, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("MysticsItems_GateChalice")).pickupIconSprite = MysticsItems_GateChaliceS;
            }

            if (EquipmentCatalog.FindEquipmentIndex("MysticsItems_TuningFork") != EquipmentIndex.None)
            {
                Texture2D MysticsItems_TuningFork = new Texture2D(128, 128, TextureFormat.DXT5, false);
                MysticsItems_TuningFork.LoadImage(Properties.Resources.MysticsItems_TuningFork, false);
                MysticsItems_TuningFork.filterMode = FilterMode.Bilinear;
                MysticsItems_TuningFork.wrapMode = TextureWrapMode.Clamp;
                Sprite MysticsItems_TuningForkS = Sprite.Create(MysticsItems_TuningFork, rec128, half);

                EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("MysticsItems_TuningFork")).pickupIconSprite = MysticsItems_TuningForkS;
            }
            //




            Texture2D OutlineChangedtexAffixBlueIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexAffixBlueIcon.LoadImage(Properties.Resources.OutlineChangedtexAffixBlueIcon, false);
            OutlineChangedtexAffixBlueIcon.filterMode = FilterMode.Bilinear;
            OutlineChangedtexAffixBlueIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixBlueIconS = Sprite.Create(OutlineChangedtexAffixBlueIcon, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixBlue").pickupIconSprite = OutlineChangedtexAffixBlueIconS;

            Texture2D OutlineChangedtexAffixHauntedIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexAffixHauntedIcon.LoadImage(Properties.Resources.OutlineChangedtexAffixHauntedIcon, false);
            OutlineChangedtexAffixHauntedIcon.filterMode = FilterMode.Bilinear;
            OutlineChangedtexAffixHauntedIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixHauntedIconS = Sprite.Create(OutlineChangedtexAffixHauntedIcon, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixHaunted").pickupIconSprite = OutlineChangedtexAffixHauntedIconS;




            if (LunarYellowEliteEquipTexture.Value == true)
            {
                Texture2D OutlineChangedtexAffixLunarIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
                OutlineChangedtexAffixLunarIcon.LoadImage(Properties.Resources.AffixRoR2Lunar, false);
                OutlineChangedtexAffixLunarIcon.filterMode = FilterMode.Bilinear;
                OutlineChangedtexAffixLunarIcon.wrapMode = TextureWrapMode.Clamp;
                Sprite OutlineChangedtexAffixLunarIconS = Sprite.Create(OutlineChangedtexAffixLunarIcon, rec128, half);
                RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").pickupIconSprite = OutlineChangedtexAffixLunarIconS;
            }
            else
            {
                Texture2D OutlineChangedtexAffixLunarIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
                OutlineChangedtexAffixLunarIcon.LoadImage(Properties.Resources.OutlineChangedtexAffixLunarIcon, false);
                OutlineChangedtexAffixLunarIcon.filterMode = FilterMode.Bilinear;
                OutlineChangedtexAffixLunarIcon.wrapMode = TextureWrapMode.Clamp;
                Sprite OutlineChangedtexAffixLunarIconS = Sprite.Create(OutlineChangedtexAffixLunarIcon, rec128, half);
                RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").pickupIconSprite = OutlineChangedtexAffixLunarIconS;
            }

            Texture2D OutlineChangedtexAffixPoisonIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexAffixPoisonIcon.LoadImage(Properties.Resources.OutlineChangedtexAffixPoisonIcon, false);
            OutlineChangedtexAffixPoisonIcon.filterMode = FilterMode.Bilinear;
            OutlineChangedtexAffixPoisonIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixPoisonIconS = Sprite.Create(OutlineChangedtexAffixPoisonIcon, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixPoison").pickupIconSprite = OutlineChangedtexAffixPoisonIconS;

            Texture2D OutlineChangedtexAffixRedIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexAffixRedIcon.LoadImage(Properties.Resources.OutlineChangedtexAffixRedIcon, false);
            OutlineChangedtexAffixRedIcon.filterMode = FilterMode.Bilinear;
            OutlineChangedtexAffixRedIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixRedIconS = Sprite.Create(OutlineChangedtexAffixRedIcon, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixRed").pickupIconSprite = OutlineChangedtexAffixRedIconS;

            Texture2D OutlineChangedtexAffixGreenIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexAffixGreenIcon.LoadImage(Properties.Resources.OutlineChangedtexAffixGreenIcon, false);
            OutlineChangedtexAffixGreenIcon.filterMode = FilterMode.Bilinear;
            OutlineChangedtexAffixGreenIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixGreenIconS = Sprite.Create(OutlineChangedtexAffixGreenIcon, rec128, half);
            Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteEarth/EliteEarthEquipment.asset").WaitForCompletion().pickupIconSprite = OutlineChangedtexAffixGreenIconS;



            Texture2D OutlineChangedtexAffixWhiteIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexAffixWhiteIcon.LoadImage(Properties.Resources.OutlineChangedtexAffixWhiteIcon, false);
            OutlineChangedtexAffixWhiteIcon.filterMode = FilterMode.Bilinear;
            OutlineChangedtexAffixWhiteIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixWhiteIconS = Sprite.Create(OutlineChangedtexAffixWhiteIcon, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixWhite").pickupIconSprite = OutlineChangedtexAffixWhiteIconS;

            Texture2D OutlineChangedtexEffigyIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexEffigyIcon.LoadImage(Properties.Resources.OutlineChangedtexEffigyIcon, false);
            OutlineChangedtexEffigyIcon.filterMode = FilterMode.Bilinear;
            OutlineChangedtexEffigyIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexEffigyIconS = Sprite.Create(OutlineChangedtexEffigyIcon, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/CrippleWard").pickupIconSprite = OutlineChangedtexEffigyIconS;

            Texture2D OutlineChangedtexPotionIconChanged = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexPotionIconChanged.LoadImage(Properties.Resources.OutlineChangedtexPotionIconChanged, false);
            OutlineChangedtexPotionIconChanged.filterMode = FilterMode.Bilinear;
            OutlineChangedtexPotionIconChanged.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexPotionIconChangedS = Sprite.Create(OutlineChangedtexPotionIconChanged, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/BurnNearby").pickupIconSprite = OutlineChangedtexPotionIconChangedS;

            Texture2D OutlineChangedtexMeteorIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexMeteorIcon.LoadImage(Properties.Resources.OutlineChangedtexMeteorIcon, false);
            OutlineChangedtexMeteorIcon.filterMode = FilterMode.Bilinear;
            OutlineChangedtexMeteorIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexMeteorIconS = Sprite.Create(OutlineChangedtexMeteorIcon, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/Meteor").pickupIconSprite = OutlineChangedtexMeteorIconS;

            Texture2D OutlineChangedtexTonicIcontonic = new Texture2D(128, 128, TextureFormat.DXT5, false);
            OutlineChangedtexTonicIcontonic.LoadImage(Properties.Resources.OutlineChangedtexTonicIcontonic, false);
            OutlineChangedtexTonicIcontonic.filterMode = FilterMode.Bilinear;
            OutlineChangedtexTonicIcontonic.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexTonicIcontonicS = Sprite.Create(OutlineChangedtexTonicIcontonic, rec128, half);
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/Tonic").pickupIconSprite = OutlineChangedtexTonicIcontonicS;










        }

        public void PingInfo()
        {
            Debug.Log("Ping Info Loaded");

            Texture2D TexCauldronIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexCleanseIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexExclamationIcon = new Texture2D(192, 192, TextureFormat.DXT5, false);
            Texture2D TexLockedIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexPrinterIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexScrapperIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexSeerIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);

            TexCauldronIcon.LoadImage(Properties.Resources.CauldronIcon, false);
            TexCleanseIcon.LoadImage(Properties.Resources.CleanseIcon, false);
            TexExclamationIcon.LoadImage(Properties.Resources.ExclamationIcon, false);
            TexLockedIcon.LoadImage(Properties.Resources.LockedIcon, false);
            TexPrinterIcon.LoadImage(Properties.Resources.PrinterIcon, false);
            TexScrapperIcon.LoadImage(Properties.Resources.ScrapperIcon, false);
            TexSeerIcon.LoadImage(Properties.Resources.SeerIcon, false);

            TexCauldronIcon.filterMode = FilterMode.Bilinear;
            TexCleanseIcon.filterMode = FilterMode.Bilinear;
            TexExclamationIcon.filterMode = FilterMode.Bilinear;
            TexLockedIcon.filterMode = FilterMode.Bilinear;
            TexPrinterIcon.filterMode = FilterMode.Bilinear;
            TexScrapperIcon.filterMode = FilterMode.Bilinear;
            TexSeerIcon.filterMode = FilterMode.Bilinear;

            TexCauldronIcon.Apply();
            TexCleanseIcon.Apply();
            TexExclamationIcon.Apply();
            TexLockedIcon.Apply();
            TexPrinterIcon.Apply();
            TexScrapperIcon.Apply();
            TexSeerIcon.Apply();

            CauldronIcon = Sprite.Create(TexCauldronIcon, rec256, half);
            SeerIcon = Sprite.Create(TexSeerIcon, rec256, half);
            ExclamationIcon = Sprite.Create(TexExclamationIcon, rec192, half);
            Sprite CleanseIcon = Sprite.Create(TexCleanseIcon, rec256, half);
            Sprite LockedIcon = Sprite.Create(TexLockedIcon, rec256, half);
            Sprite PrinterIcon = Sprite.Create(TexPrinterIcon, rec256, half);
            Sprite ScrapperIcon = Sprite.Create(TexScrapperIcon, rec256, half);



            //GShrineCleanse.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CleanseIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanse.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CleanseIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSandy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CleanseIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSnowy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CleanseIcon;



            PrefabLockbox.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LockedIcon;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/SeerStation").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = SeerIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/GoldshoresBeacon").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/RadarTower").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;

            //if (ScrapperEnable.Value == true)

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Scrapper").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ScrapperIcon;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, GreenToRed Variant").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, WhiteToGreen").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, RedToWhite Variant").GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;



            Texture2D TexNothing = new Texture2D(0, 0, TextureFormat.DXT5, false);
            TexNothing.LoadImage(Properties.Resources.ExclamationIcon, false);
            Sprite NoIcon = Sprite.Create(TexNothing, recnothing, half);
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = NoIcon;


            Texture2D TexChestLegendaryIcon = new Texture2D(384, 256, TextureFormat.DXT5, false);
            TexChestLegendaryIcon.LoadImage(Properties.Resources.ChestLegendaryIcon, false);
            TexChestLegendaryIcon.filterMode = FilterMode.Bilinear;


            Texture2D TexChestInvisibleIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexChestInvisibleIcon.LoadImage(Properties.Resources.ChestInvisibleIcon, false);
            TexChestInvisibleIcon.filterMode = FilterMode.Bilinear;
            Sprite ChestInvisibleIcon = Sprite.Create(TexChestInvisibleIcon, rec256, half);


            Texture2D TexChestCasinoIcon = new Texture2D(256, 320, TextureFormat.DXT5, false);
            TexChestCasinoIcon.LoadImage(Properties.Resources.ChestCasinoIcon, false);
            TexChestCasinoIcon.filterMode = FilterMode.Bilinear;
            Sprite ChestCasinoIcon = Sprite.Create(TexChestCasinoIcon, rechalftall, half);


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

            TexChestLargeIcon.LoadImage(Properties.Resources.ChestLargeIcon, false);
            TexChestEquipmentIcon.LoadImage(Properties.Resources.ChestEquipIcon, false);
            TexChestLunarIcon.LoadImage(Properties.Resources.ChestLunarIcon, false);

            TexShrineBloodIcon.LoadImage(Properties.Resources.ShrineBloodIcon, false);
            TexShrineChanceIcon.LoadImage(Properties.Resources.ShrineChanceIcon, false);
            TexShrineHealIcon.LoadImage(Properties.Resources.ShrineHealIcon, false);
            TexShrineMountainIcon.LoadImage(Properties.Resources.ShrineMountainIcon, false);
            TexShrineOrderIcon.LoadImage(Properties.Resources.ShrineOrderIcon, false);
            TexShrineGoldIcon.LoadImage(Properties.Resources.ShrineGoldIcon, false);

            TexDroneHealIcon.LoadImage(Properties.Resources.DroneHealIcon, false);
            TexDroneEquipmentIcon.LoadImage(Properties.Resources.DroneEquipmentIcon, false);
            TexDroneTurretIcon.LoadImage(Properties.Resources.DroneTurretIcon, false);
            TexDroneMegaIcon.LoadImage(Properties.Resources.DroneMegaIcon, false);
            TexDroneFlameIcon.LoadImage(Properties.Resources.DroneFlameIcon, false);
            TexDroneMissileIcon.LoadImage(Properties.Resources.DroneMissileIcon, false);
            TexDroneEmergencyIcon.LoadImage(Properties.Resources.DroneEmergencyIcon, false);

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

            Sprite ChestLargeIcon = Sprite.Create(TexChestLargeIcon, rec256, half);
            Sprite ChestEquipmentIcon = Sprite.Create(TexChestEquipmentIcon, rec256, half);

            Sprite ShrineBloodIcon = Sprite.Create(TexShrineBloodIcon, rec256, half);
            Sprite ShrineChanceIcon = Sprite.Create(TexShrineChanceIcon, rec256, half);
            Sprite ShrineHealIcon = Sprite.Create(TexShrineHealIcon, rec256, half);
            Sprite ShrineMountainIcon = Sprite.Create(TexShrineMountainIcon, rec256, half);
            //ShrineOrderIcon = Sprite.Create(TexShrineOrderIcon, rec256, half);
            Sprite ShrineGoldIcon = Sprite.Create(TexShrineGoldIcon, rectall, half);

            Sprite DroneHealIcon = Sprite.Create(TexDroneHealIcon, rec256, half);
            Sprite DroneEquipmentIcon = Sprite.Create(TexDroneEquipmentIcon, rec256, half);
            Sprite DroneTurretIcon = Sprite.Create(TexDroneTurretIcon, rec256, half);
            Sprite DroneMegaIcon = Sprite.Create(TexDroneMegaIcon, recwide, half);
            Sprite DroneFlameIcon = Sprite.Create(TexDroneFlameIcon, rechalftall, half);
            Sprite DroneMissileIcon = Sprite.Create(TexDroneMissileIcon, rechalftall, half);
            Sprite DroneEmergencyIcon = Sprite.Create(TexDroneEmergencyIcon, rec320, half);

            //Will be replaced if specific module enabled

            ChestLunarIcon = ChestIcon;
            //ChestInvisibleIcon = ChestIcon;
            LegendaryChestIcon = ChestIcon;
            ShrineOrderIcon = ShrineIcon;
            //Order

            TimedChestIcon = ChestIcon;

            Texture2D TexChestTimedIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexChestTimedIcon.LoadImage(Properties.Resources.ChestTimedIcon, false);
            TexChestTimedIcon.filterMode = FilterMode.Bilinear;



            //if (ChestMiscEnable.Value == true) //Misc Chest

            ChestLunarIcon = Sprite.Create(TexChestLunarIcon, rec256, half);
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/EquipmentBarrel").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestEquipmentIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopEquipmentTerminal").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestEquipmentIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/LunarChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLunarIcon;

            //if (ChestLargeEnable.Value == true) //Chest


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Chest2").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopLargeTerminal").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeIcon;
            TimedChestIcon = Sprite.Create(TexChestTimedIcon, rec256, half);

            //if (ChestCasinoEnable.Value == false && ChestLargeEnable.Value == true)

            //if (ChestCasinoEnable.Value == true)

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CasinoChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestCasinoIcon;

            //if (ChestInvisEnable.Value == true) //Chest


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Chest1StealthedVariant").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestInvisibleIcon;

            //if (ChestLegendaryEnable.Value == true) //Chest

            LegendaryChestIcon = Sprite.Create(TexChestLegendaryIcon, recwide, half);
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/GoldChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LegendaryChestIcon;




            //if (ShrinesMountainEnable.Value == true)

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineBoss").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineMountainIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBoss/ShrineBossSandy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineMountainIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineBoss/ShrineBossSnowy Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineMountainIcon;

            //if (ShrinesGoldEnable.Value == true)

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineGoldshoresAccess").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineGoldIcon;

            //if (ShrinesRestEnable.Value == true)

            //Combat Default
            ShrineOrderIcon = Sprite.Create(TexShrineOrderIcon, rec256, half);
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





            //if (DronesMainEnabled.Value == true)

            //Gunner Drone Default
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/Turret1Broken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneTurretIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/Drone2Broken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneHealIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/EmergencyDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneEmergencyIcon;  //Not Unique
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/EquipmentDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneEquipmentIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/MegaDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneMegaIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/FlameDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneFlameIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/brokendrones/MissileDroneBroken").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DroneMissileIcon;



            Texture2D TexDuplicatorLarge = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexDuplicatorMili = new Texture2D(320, 256, TextureFormat.DXT5, false);
            Texture2D TexDuplicatorWild = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexDuplicatorLarge.LoadImage(Properties.Resources.PrinterLargeIcon, false);
            TexDuplicatorMili.LoadImage(Properties.Resources.PrinterMiliIcon, false);
            TexDuplicatorWild.LoadImage(Properties.Resources.PrinterWildIcon, false);
            TexDuplicatorLarge.filterMode = FilterMode.Bilinear;
            TexDuplicatorMili.filterMode = FilterMode.Bilinear;
            TexDuplicatorWild.filterMode = FilterMode.Bilinear;
            Sprite DuplicatorLarge = Sprite.Create(TexDuplicatorLarge, rec256, half);
            Sprite DuplicatorMili = Sprite.Create(TexDuplicatorMili, rechalfwide, half);
            Sprite DuplicatorWild = Sprite.Create(TexDuplicatorWild, rec256, half);



            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Duplicator").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PrinterIcon;

            //if (PrintersSpecificEnable.Value == false)

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DuplicatorLarge;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DuplicatorMili;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = DuplicatorWild;




            Texture2D TexChestDamageIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexChestHealingIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D TexChestUtilIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexChestDamageIcon.LoadImage(Properties.Resources.ChestDamageIcon, false);
            TexChestHealingIcon.LoadImage(Properties.Resources.ChestHealIcon, false);
            TexChestUtilIcon.LoadImage(Properties.Resources.ChestUtilIcon, false);
            TexChestDamageIcon.filterMode = FilterMode.Bilinear;
            TexChestHealingIcon.filterMode = FilterMode.Bilinear;
            TexChestUtilIcon.filterMode = FilterMode.Bilinear;
            Sprite ChestDamageIcon = Sprite.Create(TexChestDamageIcon, rec256, half);
            Sprite ChestHealingIcon = Sprite.Create(TexChestHealingIcon, rec256, half);
            Sprite ChestUtilIcon = Sprite.Create(TexChestUtilIcon, rec256, half);

            //if (ChestCategoryEnable.Value == true) //Category Chests

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestDamage").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestDamageIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestHealing").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestHealingIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChestUtility").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestUtilIcon;



            Texture2D ChestLargeDamageIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D ChestLargeHealIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            Texture2D ChestLargeUtilIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            ChestLargeDamageIcon.LoadImage(Properties.Resources.ChestLargeDamageIcon, false);
            ChestLargeHealIcon.LoadImage(Properties.Resources.ChestLargeHealIcon, false);
            ChestLargeUtilIcon.LoadImage(Properties.Resources.ChestLargeUtilIcon, false);
            ChestLargeDamageIcon.filterMode = FilterMode.Bilinear;
            ChestLargeHealIcon.filterMode = FilterMode.Bilinear;
            ChestLargeUtilIcon.filterMode = FilterMode.Bilinear;
            Sprite ChestLargeDamageIconS = Sprite.Create(ChestLargeDamageIcon, rec256, half);
            Sprite ChestLargeHealIconS = Sprite.Create(ChestLargeHealIcon, rec256, half);
            Sprite ChestLargeUtilIconS = Sprite.Create(ChestLargeUtilIcon, rec256, half);

            //if (ChestCategoryEnable.Value == true) //Category Chests
            /*
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChest2Damage Variant").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeDamageIconS;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChest2Damage Variant").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeHealIconS;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/CategoryChest2Utility Variant").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeUtilIconS;
            */



            Sprite QuestionMarkIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texMysteryIcon");

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Damage Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeDamageIconS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Healing Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeHealIconS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/CategoryChest2/CategoryChest2Utility Variant.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLargeUtilIconS;



            Texture2D PingIconShippingDrone = new Texture2D(256, 256, TextureFormat.DXT5, false);
            PingIconShippingDrone.LoadImage(Properties.Resources.PingIconShippingDrone, false);
            PingIconShippingDrone.filterMode = FilterMode.Bilinear;
            Sprite PingIconShippingDroneS = Sprite.Create(PingIconShippingDrone, rec256, half);

            Texture2D PingIconVoidEradicator = new Texture2D(128, 128, TextureFormat.DXT5, false);
            PingIconVoidEradicator.LoadImage(Properties.Resources.PingIconVoidEradicator, false);
            PingIconVoidEradicator.filterMode = FilterMode.Bilinear;
            Sprite PingIconVoidEradicatorS = Sprite.Create(PingIconVoidEradicator, rec128, half);


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestMultiShop/FreeChestMultiShop.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminal/FreeChestTerminal.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIconShippingDroneS;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSuppressor/VoidSuppressor.prefab").WaitForCompletion().AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIconVoidEradicatorS;



            Texture2D ChestVoid = new Texture2D(256, 320, TextureFormat.DXT5, false);
            ChestVoid.LoadImage(Properties.Resources.ChestVoid, false);
            ChestVoid.filterMode = FilterMode.Bilinear;
            Sprite ChestVoidS = Sprite.Create(ChestVoid, rechalftall, half);

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/VoidChest").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestVoidS;

            Texture2D ChestVoidPotential = new Texture2D(256, 384, TextureFormat.DXT5, false);
            ChestVoidPotential.LoadImage(Properties.Resources.ChestVoidPotential, false);
            ChestVoidPotential.filterMode = FilterMode.Bilinear;
            Sprite ChestVoidPotentialS = Sprite.Create(ChestVoidPotential, rectall, half);

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/VoidTriple").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestVoidPotentialS;






            Sprite VoidDeepSymbol = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBatteryPositionIndicator.prefab").WaitForCompletion().GetComponent<RoR2.UI.ChargeIndicatorController>().iconSprites[0].sprite;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBattery.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = VoidDeepSymbol;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidCamp/VoidCamp.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = VoidDeepSymbol;



            Texture2D LockedIconAlt = new Texture2D(256, 256, TextureFormat.DXT5, false);
            LockedIconAlt.LoadImage(Properties.Resources.LockedIconAlt, false);
            LockedIconAlt.filterMode = FilterMode.Bilinear;
            Sprite LockedIconAltS = Sprite.Create(LockedIconAlt, rec256, half);

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = LockedIconAltS;







            Texture2D TexNullVentIcon = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexNullVentIcon.LoadImage(Properties.Resources.CellIcon, false);
            TexNullVentIcon.filterMode = FilterMode.Bilinear;
            NullVentIcon = AttackIcon;
            //if (NullSafeVentEnable.Value == true)

            NullVentIcon = Sprite.Create(TexNullVentIcon, rec320, half);

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerSafeWard.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = NullVentIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/InfiniteTowerSafeWardAwaitingInteraction.prefab").WaitForCompletion().GetComponent<RoR2.PingInfoProvider>().pingIconOverride = NullVentIcon;


            Texture2D TexScavBag = new Texture2D(256, 256, TextureFormat.DXT5, false);
            TexScavBag.LoadImage(Properties.Resources.ScavBagIcon, false);
            TexScavBag.filterMode = FilterMode.Bilinear;
            Sprite ScavBagIcon = Sprite.Create(TexScavBag, rec256, half);

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/ScavBackpack").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ScavBagIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/ScavLunarBackpack").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ScavBagIcon;



            Texture2D TexPortalIcon = new Texture2D(256, 320, TextureFormat.DXT5, false);
            TexPortalIcon.LoadImage(Properties.Resources.PortalIcon, false);
            TexPortalIcon.filterMode = FilterMode.Bilinear;
            PortalIcon = Sprite.Create(TexPortalIcon, rechalftall, half);


            if (MessagesPortals.Value == true)
            {
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArtifactworld").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cArtifact>Artifact Portal</style>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalGoldshores").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#FFE880>Gold Portal</color>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalShop").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsLunar>Blue Portal</style>";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalMS").AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <color=#A0FFD6>Celestial Portal</color>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsVoid>Void Portal</style>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "OBJECTIVE_VOID_DEEP_PORTAL";

                On.RoR2.SceneExitController.Begin += (orig, self) =>
                {
                    orig(self);
                    if (self.exitState == SceneExitController.ExitState.ExtractExp || self.exitState == SceneExitController.ExitState.TeleportOut)
                    {
                        GenericObjectiveProvider objective = self.GetComponent<GenericObjectiveProvider>();
                        if (objective)
                        {
                            Destroy(objective);
                        }
                    }
                };

                On.RoR2.SceneExitController.SetState += (orig, self, newState) =>
                {
                    orig(self, newState);
                    if (newState == SceneExitController.ExitState.ExtractExp || newState == SceneExitController.ExitState.TeleportOut)
                    {
                        GenericObjectiveProvider objective = self.GetComponent<GenericObjectiveProvider>();
                        if (objective)
                        {
                            Destroy(objective);
                        }
                    }
                };
            }



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






            Texture2D TexPrimordialTeleporter = new Texture2D(256, 320, TextureFormat.DXT5, false);
            TexPrimordialTeleporter.LoadImage(Properties.Resources.PrimordialTeleporterIcon, false);
            TexPrimordialTeleporter.filterMode = FilterMode.Bilinear;
            PrimordialTeleporterIcon = Sprite.Create(TexPrimordialTeleporter, rec512, half);

            Texture2D TexPrimordialTeleporterC = new Texture2D(256, 320, TextureFormat.DXT5, false);
            TexPrimordialTeleporterC.LoadImage(Properties.Resources.PrimordialTeleporterChargedIcon, false);
            TexPrimordialTeleporterC.filterMode = FilterMode.Bilinear;
            PrimordialTeleporterChargedIcon = Sprite.Create(TexPrimordialTeleporterC, rec512, half);

            /*
            if (PrimordialTPEnable.Value == false)
            {
                Sprite TeleporterIcon = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texTeleporterIconOutlined");
                Sprite TeleporterChargedIcon = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/TeleporterChargingPositionIndicator").GetComponent<RoR2.UI.ChargeIndicatorController>().iconSprites[0].sprite;
                PrimordialTeleporterIcon = TeleporterIcon;
                PrimordialTeleporterChargedIcon = TeleporterChargedIcon;
            }
            */


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporter Variant").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PrimordialTeleporterIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporterProngs").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PrimordialTeleporterIcon;




        }







        public void Run_onRunDestroyGlobal(Run obj)
        {
            //dtPearls.entries = dtPearls.entries.Add(Pearl);
            //dtPearls.entries = dtPearls.entries.Add(ShinyPearl);
            //Logger.LogMessage($"RunEnd: Added Pearl and ShinyPearl to Dtpearls.");
        }

        public void Run_onRunStartGlobal(Run obj)
        {

            //dtPearls.entries = dtPearls.entries.Remove(Pearl, ShinyPearl);
            //Logger.LogMessage($"RunStart: Removed Pearl and ShinyPearl to Dtpearls.");



            if (RunFirstTimeDone == false)
            {
                if (SkinChanges.BellBalls == null)
                {
                    SkinChanges.BellBalls = EntityStates.Bell.BellWeapon.ChargeTrioBomb.preppedBombPrefab.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().material;
                    SkinChanges.BellBalls.enableInstancing = false;
                }

                RunFirstTimeDone = true;
                GetDotDef();
                PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("EquipmentIndex.EliteLunarEquipment")).isLunar = true;
            }


        }


        public static void GameObjectAttatcher()
        {
            GameObject LockboxHoloPivot = new GameObject("LockboxHoloPivot");
            DontDestroyOnLoad(LockboxHoloPivot);
            LockboxHoloPivot.transform.localPosition = new Vector3(0.0f, 2f, 0f);
            LockboxHoloPivot.transform.SetParent(PrefabLockbox.transform, false);
            PrefabLockbox.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LockboxHoloPivot.transform;

            GameObject LockboxHoloVoidPivot = new GameObject("LockboxVoidHoloPivot");
            DontDestroyOnLoad(LockboxHoloVoidPivot);
            LockboxHoloVoidPivot.transform.localPosition = new Vector3(0.0f, 2f, 0f);
            LockboxHoloVoidPivot.transform.SetParent(PrefabLockboxVoid.transform, false);
            PrefabLockboxVoid.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LockboxHoloVoidPivot.transform;

            GameObject VendingMachineHoloPivot = new GameObject("VendingMachineHoloPivot");
            DontDestroyOnLoad(VendingMachineHoloPivot);
            VendingMachineHoloPivot.transform.localPosition = new Vector3(0.0f, 4.3f, 0f);
            VendingMachineHoloPivot.transform.SetParent(PrefabVendingMachine.transform, false);
            PrefabVendingMachine.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = VendingMachineHoloPivot.transform;

            if (ConfigSprintingCrosshair.Value == true)
            {
                GameObject RightBracket = Instantiate(LoaderCrosshair.transform.GetChild(2).gameObject);
                GameObject LeftBracket = Instantiate(LoaderCrosshair.transform.GetChild(3).gameObject);

                DontDestroyOnLoad(RightBracket);
                DontDestroyOnLoad(LeftBracket);

                RightBracket.transform.SetParent(SprintingCrosshair.transform);
                LeftBracket.transform.SetParent(SprintingCrosshair.transform);

                SprintingCrosshairUI.spriteSpreadPositions[0].target = LeftBracket.GetComponent<RectTransform>();
                SprintingCrosshairUI.spriteSpreadPositions[1].target = RightBracket.GetComponent<RectTransform>();
            }

            GameObject beacon = Instantiate(CaptainHackingBeaconIndicator);
            //Destroy(beacon.GetComponent<ObjectScaleCurve>());
            DontDestroyOnLoad(beacon);
            beacon.transform.localScale = new Vector3(6.67f, 6.67f, 6.67f);
            beacon.transform.GetChild(0).GetComponent<MeshRenderer>().material = CaptainHackingBeaconIndicatorMaterial;
            beacon.transform.SetParent(CaptainShockBeacon.transform, false);

        }


        public void PingIconChanger(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            GameObjectAttatcher();
            Debug.Log("Wqol preSceneDic");

            if (!SceneInfo.instance) { orig(self); return; }

            if (LastStageGoolake)
            {
                On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter -= FireMegaFireball_OnEnter;
                LastStageGoolake = false;
            }




            switch (SceneInfo.instance.sceneDef.baseSceneName)
            {
                case "skymeadow":
                    GameObject.Find("/PortalDialerEvent/Final Zone/ButtonContainer/PortalDialer").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                    GameObject.Find("/HOLDER: Zones/OOB Zone").GetComponent<MapZone>().zoneType = MapZone.ZoneType.OutOfBounds;


                    Material CorrectGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser (2)/mdlGeyser").GetComponent<MeshRenderer>().material;
                    Transform WrongGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser").transform;
                    WrongGeyser.GetChild(0).GetComponent<MeshRenderer>().material = CorrectGeyser;
                    WrongGeyser.GetChild(1).GetComponent<MeshRenderer>().material = CorrectGeyser;
                    break;
                case "frozenwall":
                case "itfrozenwall":
                    genericlist = FindObjectsOfType(typeof(GenericDisplayNameProvider)) as GenericDisplayNameProvider[];

                    for (var i = 0; i < genericlist.Length; i++)
                    {
                        //Debug.LogWarning(genericlist[i]); ////DISABLE THIS
                        if (genericlist[i].name.Contains("HumanFan"))
                        {
                            genericlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                        }
                        else if (genericlist[i].name.StartsWith("TimedChest"))
                        {
                            genericlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = TimedChestIcon;
                        }

                    }
                    genericlist = null;
                    GC.Collect();
                    break;
                case "blackbeach":
                    GameObject tempobj = GameObject.Find("/HOLDER: Preplaced Objects");
                    if (tempobj != null)
                    {
                        UnlockableGranter[] portalstatuelist = tempobj.GetComponentsInChildren<RoR2.UnlockableGranter>(true);
                        int unavailable = 0;
                        for (var i = 0; i < portalstatuelist.Length; i++)
                        {
                            //Debug.LogWarning(portalstatuelist[i]); ////DISABLE THIS
                            //Debug.LogWarning(portalstatuelist[i].gameObject.activeSelf); ////DISABLE THIS
                            if (portalstatuelist[i].gameObject.activeSelf == false)
                            {
                                unavailable++;
                            }
                            if (unavailable == 3)
                            {
                                portalstatuelist[1].gameObject.SetActive(true);
                            }
                        }
                        portalstatuelist = null;
                        GC.Collect();
                    }
                    break;
                case "goolake":
                    LastStageGoolake = true;
                    On.EntityStates.LemurianBruiserMonster.FireMegaFireball.OnEnter += FireMegaFireball_OnEnter;
                    /*
                    BuffDef SlowTar = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/Base/Common/bdClayGoo.asset").WaitForCompletion();
                    GameObject HOLDERZones = GameObject.Find("/HOLDER: Zones/");
                    HOLDERZones.transform.GetChild(1).localPosition = new Vector3(105.5f, -166.4f, 54.1f);
                    HOLDERZones.transform.GetChild(2).localPosition = new Vector3(172f, -155.5f, -236f);
                    */

                    GameObject DoorIsOpenedEffect = GameObject.Find("/HOLDER: Secret Ring Area Content/Entrance/GLRuinGate/DoorIsOpenedEffect/");
                    DoorIsOpenedEffect.AddComponent<GenericObjectiveProvider>().objectiveToken = "Explore the hidden chamber";

                    //GameObject tempobj = GameObject.Find("/HOLDER: Misc Props/GooPlane, High");
                    GameObject GooPlaneOriginal = GameObject.Find("/HOLDER: Misc Props/GooPlane");

                    GameObject GooPlaneNew1 = Instantiate(GooPlaneOriginal, GooPlaneOriginal.transform.parent);
                    GameObject GooPlaneNew2 = Instantiate(GooPlaneOriginal, GooPlaneOriginal.transform.parent);

                    GooPlaneNew1.transform.localPosition = new Vector3(319.28f, -135.9f, 143f);
                    GooPlaneNew1.transform.localScale = new Vector3(33.8897f, 126.1369f, 23.5065f);
                    GooPlaneNew1.transform.localEulerAngles = new Vector3(0f, 326.1636f, 0f);

                    GooPlaneNew2.transform.localPosition = new Vector3(264.7182f, -138.35f, 213.0005f);
                    GooPlaneNew2.transform.localScale = new Vector3(20f, 126.1369f, 23.5065f);
                    GooPlaneNew2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

                    /*
                    DebuffZone[] GooLakeDebuffZones = FindObjectsOfType(typeof(DebuffZone)) as DebuffZone[];
                    for (var i = 2; i < GooLakeDebuffZones.Length; i++)
                    {
                        Debug.LogWarning(GooLakeDebuffZones[i]);
                        GooLakeDebuffZones[i].buffType = SlowTar;
                    }
                    */

                    DummyPingableInteraction[] desertplatelist = FindObjectsOfType(typeof(DummyPingableInteraction)) as DummyPingableInteraction[];
                    for (var i = 0; i < desertplatelist.Length; i++)
                    {
                        //Debug.LogWarning(desertplatelist[i]); ////DISABLE THIS
                        if (desertplatelist[i].name.Contains("GLPressurePlate"))
                        {
                            desertplatelist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                        }
                    }
                    Inventory[] InventoryList = FindObjectsOfType(typeof(RoR2.Inventory)) as RoR2.Inventory[];
                    for (var i = 0; i < InventoryList.Length; i++)
                    {
                        if (NetworkServer.active)
                        {
                            //Debug.LogWarning(desertplatelist[i]); ////DISABLE THIS
                            if (InventoryList[i].name.StartsWith("LemurianBruiserFireMaster") || InventoryList[i].name.StartsWith("LemurianBruiserIceMaster"))
                            {
                                int temp = Run.instance.stageClearCount * Run.instance.stageClearCount - 1;
                                if (temp > 0)
                                {
                                    InventoryList[i].GiveItem(RoR2Content.Items.LevelBonus, temp);
                                }

                            }
                        }
                    }
                    GC.Collect();
                    break;
                case "sulfurpools":
                    GameObject GeyserHolder = GameObject.Find("/HOLDER: Geysers");
                    MatGeyserSulfurPools = Instantiate(GeyserHolder.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material);
                    MatGeyserSulfurPools.SetColor("_EmissionColor", new Color(0.15f, 0.2f, 0.08f));

                    for (int i = 0; i < GeyserHolder.transform.childCount; i++)
                    {
                        Transform LoopParticles = GeyserHolder.transform.GetChild(i).GetChild(2).GetChild(0);
                        LoopParticles.GetChild(1).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                        LoopParticles.GetChild(2).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                        LoopParticles.GetChild(3).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                    }
                    break;
                case "foggyswamp":
                    GameObjectUnlockableFilter[] dummylist = FindObjectsOfType(typeof(RoR2.GameObjectUnlockableFilter)) as RoR2.GameObjectUnlockableFilter[];
                    for (var i = 0; i < dummylist.Length; i++)
                    {
                        Destroy(dummylist[i]);
                    }
                    GC.Collect();
                    break;
                case "dampcavesimple":
                case "itdampcave":
                    purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                    for (var i = 0; i < purchaserlist.Length; i++)
                    {
                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (purchaserlist[i].name.StartsWith("TreebotUnlockInteractable"))
                        {
                            purchaserlist[i].gameObject.GetComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                            GameObject BrokenRexHoloPivot = new GameObject("BrokenRexHoloPivot");
                            BrokenRexHoloPivot.transform.localPosition = new Vector3(0.8f, 4f, -0.2f);
                            BrokenRexHoloPivot.transform.SetParent(purchaserlist[i].gameObject.transform, false);
                            purchaserlist[i].gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = BrokenRexHoloPivot.transform;
                        }
                        else if (purchaserlist[i].name.StartsWith("GoldChest"))
                        {
                            purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LegendaryChestIcon;
                        }


                    }
                    purchaserlist = null;
                    GC.Collect();
                    break;
                case "rootjungle":
                    purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                    for (var i = 0; i < purchaserlist.Length; i++)
                    {
                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (purchaserlist[i].name.StartsWith("GoldChest"))
                        {
                            purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = LegendaryChestIcon;
                        }
                    }
                    UnityEngine.LODGroup[] lodlist = GameObject.Find("HOLDER: Randomization/GROUP: Large Treasure Chests").GetComponentsInChildren<UnityEngine.LODGroup>();

                    for (var i = 0; i < lodlist.Length; i++)
                    {
                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (lodlist[i].name.StartsWith("RJpinkshroom"))
                        {
                            Destroy(lodlist[i]);
                        }
                    }
                    purchaserlist = null;
                    GC.Collect();
                    break;
                case "wispgraveyard":
                    break;
                case "bazaar":
                    purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                    genericlist = FindObjectsOfType(typeof(GenericDisplayNameProvider)) as GenericDisplayNameProvider[];
                    int mooned = 10;
                    int instant = 10;
                    if (ThirdLunarSeer.Value == true)
                    {
                        instant = 0;
                    }
                    if (CommencementSeer.Value == true)
                    {
                        mooned = 0;
                    }
                    //int voidportalpresent = 0;

                    SceneDef nextStageScene = Run.instance.nextStageScene;
                    List<SceneDef> nextscenelist = new List<SceneDef>();
                    if (nextStageScene != null)
                    {
                        int stageOrder = nextStageScene.stageOrder;
                        foreach (SceneDef sceneDef in SceneCatalog.allSceneDefs)
                        {
                            if (sceneDef.stageOrder == stageOrder && (sceneDef.requiredExpansion == null || Run.instance.IsExpansionEnabled(sceneDef.requiredExpansion)) && !sceneDef.baseSceneName.EndsWith("2"))
                            {
                                nextscenelist.Add(sceneDef);
                            }
                        }
                    }

                    SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = "Leave the <style=cIsLunar>Bazaar Between Time</style>";
                    for (var i = 0; i < purchaserlist.Length; i++)
                    {

                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (purchaserlist[i].name.Contains("LunarCauldron,"))
                        {
                            purchaserlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
                        }
                        else if (purchaserlist[i].name.Contains("SeerStation"))
                        {
                            purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = SeerIcon;
                            if (NetworkServer.active)
                            {
                                if (Run.instance && Run.instance.NetworkstageClearCount % Run.stagesPerLoop == 0)
                                {
                                    if (mooned == 0)
                                    {
                                        mooned++;
                                        purchaserlist[i].gameObject.GetComponent<SeerStationController>().SetTargetScene(SceneCatalog.GetSceneDefFromSceneName("moon2"));
                                    }
                                }

                                if (nextscenelist.Count > 0)
                                {

                                    SceneDef sceneDefForRemoval = SceneCatalog.GetSceneDef((SceneIndex)purchaserlist[i].gameObject.GetComponent<SeerStationController>().NetworktargetSceneDefIndex);
                                    if (sceneDefForRemoval)
                                    {
                                        if (sceneDefForRemoval.cachedName.StartsWith("blackbeach") || sceneDefForRemoval.cachedName.StartsWith("golemplains"))
                                        {
                                            nextscenelist.Remove(SceneCatalog.FindSceneDef(sceneDefForRemoval.baseSceneName));
                                            nextscenelist.Remove(SceneCatalog.FindSceneDef(sceneDefForRemoval.baseSceneName + "2"));
                                        }
                                        else
                                        {
                                            nextscenelist.Remove(sceneDefForRemoval);
                                        }
                                    }

                                   
                                }

                                if (instant == 1)
                                {
                                    instant = 2;
                                    GameObject newseer = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/bazaar/SeerStation.prefab").WaitForCompletion(), purchaserlist[i].gameObject.transform.parent);
                                    NetworkServer.Spawn(newseer);
                                    newseer.transform.localPosition = new Vector3(10f, -0.81f, 4f);
                                    newseer.transform.localRotation = new Quaternion(0f, 0.3827f, 0f, -0.9239f);
                                    if (nextscenelist.Count > 0)
                                    {
                                        newseer.GetComponent<SeerStationController>().SetTargetScene(nextscenelist[random.Next(nextscenelist.Count)]);
                                        newseer.GetComponent<SeerStationController>().OnStartClient();
                                    }
                                    else
                                    {
                                        newseer.GetComponent<PurchaseInteraction>().SetAvailable(false);
                                        newseer.GetComponent<SeerStationController>().OnStartClient();
                                    }



                                }
                                instant++;
                            }
                        }
                        else if (purchaserlist[i].name.Contains("LunarShopTerminal"))
                        {
                            purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLunarIcon;
                        }
                        else if (purchaserlist[i].name.StartsWith("LunarRecycler"))
                        {
                            GameObject LunarRecyclerPivot = new GameObject("LunarRecyclerPivot");
                            LunarRecyclerPivot.transform.localPosition = new Vector3(0.1f, -1f, 1f);
                            LunarRecyclerPivot.transform.localRotation = new Quaternion(0f, -0.7071f, -0.7071f, 0f);
                            LunarRecyclerPivot.transform.SetParent(purchaserlist[i].gameObject.transform, false);
                            purchaserlist[i].gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LunarRecyclerPivot.transform;
                            purchaserlist[i].gameObject.GetComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
                        }
                        else if (purchaserlist[i].name.Contains("LockedMage"))
                        {
                            if (AlwaysDisplayLockedArti.Value == true)
                            {
                                Destroy(purchaserlist[i].gameObject.GetComponent<RoR2.GameObjectUnlockableFilter>());
                            }
                        }
                    }
                    for (var j = 0; j < genericlist.Length; j++)
                    {
                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (genericlist[j].name.Contains("PortalArena"))
                        {
                            //voidportalpresent++;
                            genericlist[j].gameObject.transform.parent.GetChild(2).gameObject.GetComponent<SphereCollider>().radius = 130;
                            genericlist[j].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
                            //Destroy(genericlist[j].gameObject.GetComponent<RoR2.RunEventFlagResponse>());
                            //Destroy(genericlist[j].gameObject.GetComponent<RoR2.EventFunctions>());
                        }
                        else if (genericlist[j].name.Contains("Portal"))
                        {
                            genericlist[j].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PortalIcon;
                        }
                    }
                    purchaserlist = null;
                    genericlist = null;
                    GC.Collect();
                    break;
                case "moon2":
                    highlightlist = FindObjectsOfType(typeof(Highlight)) as Highlight[];
                    BossGroup[] bossgrouplist = FindObjectsOfType(typeof(BossGroup)) as BossGroup[];
                    int cauldronthing = 0;
                    int cauldronRWcount = 0;
                    Transform tempcauldronparent = null;
                    Transform tempcauldron1 = null;
                    for (var i = 0; i < highlightlist.Length; i++)
                    {
                        //Debug.LogWarning(highlightlist[i]); ////DISABLE THIS
                        if (highlightlist[i].name.Contains("MoonBatteryDesign") || highlightlist[i].name.Contains("MoonBatteryBlood") || highlightlist[i].name.Contains("MoonBatteryMass") || highlightlist[i].name.Contains("MoonBatterySoul"))
                        {
                            highlightlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = CubeIcon;
                        }
                        else if (highlightlist[i].name.Contains("LunarCauldron,"))
                        {
                            if (cauldronthing == 0)
                            {
                                tempcauldron1 = highlightlist[i].gameObject.transform;
                                tempcauldronparent = highlightlist[i].gameObject.transform.parent;
                            };
                            cauldronthing++;
                            if (highlightlist[i].name.Contains("LunarCauldron, RedToWhite Variant"))
                            {
                                cauldronRWcount++;
                                highlightlist[i].gameObject.GetComponent<ShopTerminalBehavior>().dropVelocity = new Vector3(5, 10, 5);
                            };
                            highlightlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = CauldronIcon;
                            if (cauldronthing == 5 && cauldronRWcount == 0 && tempcauldronparent != null)
                            {
                                if (GuaranteedRedToWhite.Value == true)
                                {
                                    GameObject newSoup = Instantiate<GameObject>(RedToWhiteSoup, tempcauldron1.position, tempcauldron1.rotation);
                                    NetworkServer.Spawn(newSoup);
                                    tempcauldron1.gameObject.SetActive(false);
                                    Debug.Log("No White Soup, making one");
                                }
                                else
                                {
                                    Debug.Log("No White Soup");
                                }
                            };
                            //RedToWhiteSoup
                        }
                        else if (highlightlist[i].name.StartsWith("MoonElevator"))
                        {
                            highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                        }
                        else if (highlightlist[i].name.StartsWith("LunarChest"))
                        {
                            highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ChestLunarIcon;
                        }
                        else if (highlightlist[i].name.StartsWith("ShrineRestack"))
                        {
                            highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ShrineOrderIcon;
                        }
                    }

                    for (var i = 0; i < bossgrouplist.Length; i++)
                    {
                        if (bossgrouplist[i].name.StartsWith("BrotherEncounter, Phase 2"))
                        {
                            bossgrouplist[i].bestObservedName = Language.GetString("LUNARGOLEM_BODY_NAME");
                            bossgrouplist[i].bestObservedSubtitle = "<sprite name=\"CloudLeft\" tint=1> " + Language.GetString("LUNARGOLEM_BODY_SUBTITLE") + " <sprite name=\"CloudRight\" tint=1>";
                        }
                    }

                    highlightlist = null;
                    bossgrouplist = null;
                    GC.Collect();
                    break;
                case "arena":
                    GameObject PortalArena = GameObject.Find("/PortalArena");
                    PortalArena.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PortalIcon;
                    PortalArena.AddComponent<GenericObjectiveProvider>().objectiveToken = "Exit the <style=cIsVoid>Void Fields</style>";

                    purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                    for (var i = 0; i < purchaserlist.Length; i++)
                    {
                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (purchaserlist[i].name.Contains("NullSafeZone"))
                        {
                            purchaserlist[i].gameObject.GetComponent<RoR2.PingInfoProvider>().pingIconOverride = NullVentIcon;
                        }
                    }
                    purchaserlist = null;
                    GC.Collect();
                    break;
                case "mysteryspace":
                    GameObject MSObelisk = GameObject.Find("/MSObelisk");
                    MSObelisk.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = ExclamationIcon;
                    MSObelisk.AddComponent<GenericObjectiveProvider>().objectiveToken = "Obliterate yourself from existence";
                    break;
                case "artifactworld":
                    GenericPickupController[] pickuplist = FindObjectsOfType(typeof(GenericPickupController)) as GenericPickupController[];
                    for (var i = 0; i < pickuplist.Length; i++)
                    {
                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (pickuplist[i].name.Contains("SetpiecePickup"))
                        {
                            pickuplist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = CubeIcon;


                            ArtifactDef tempartifactdef = ArtifactCatalog.GetArtifactDef(pickuplist[i].pickupIndex.pickupDef.artifactIndex);
                            if (tempartifactdef)
                            {
                                string artifactname = Language.GetString(tempartifactdef.nameToken);
                                artifactname = artifactname.Replace("Artifact of ", "");
                                artifactname = artifactname.Replace("the ", "");
                                pickuplist[i].gameObject.AddComponent<RoR2.GenericObjectiveProvider>().objectiveToken = "Complete the <style=cArtifact>Trial of " + artifactname + "</style>";
                            }



                            if (ArtifactOutline.Value == true)
                            {
                                RoR2.Highlight tempartifact = pickuplist[i].gameObject.GetComponent<Highlight>();
                                tempartifact.pickupIndex = pickuplist[i].pickupIndex;
                                tempartifact.highlightColor = Highlight.HighlightColor.pickup;
                                tempartifact.isOn = true;
                            }

                        }
                    }
                    pickuplist = null;
                    GC.Collect();
                    break;
                case "voidstage":
                    GameObject MissionController = GameObject.Find("/MissionController");
                    MissionController.GetComponent<VoidStageMissionController>().deepVoidPortalObjectiveProvider = null;
                    break;
                case "voidraid":

                    break;
            };

            orig(self);


            if (MessagesLootItems.Value == true)
            {
                if (SceneInfo.instance.countsAsStage)
                {
                    SceneInfoTreasureCacheCount = 0;
                    SceneInfoTreasureCacheVoidCount = 0;
                    SceneInfoFreeChestCount = 0;
                    using (IEnumerator<CharacterMaster> enumerator = CharacterMaster.readOnlyInstancesList.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (enumerator.Current.inventory.GetItemCount(RoR2Content.Items.TreasureCache) > 0) { SceneInfoTreasureCacheCount++; }
                            if (enumerator.Current.inventory.GetItemCount(DLC1Content.Items.TreasureCacheVoid) > 0) { SceneInfoTreasureCacheVoidCount++; }
                            if (enumerator.Current.inventory.GetItemCount(DLC1Content.Items.FreeChest) > 0) { SceneInfoFreeChestCount++; }

                        }
                    }
                    if (SceneInfoTreasureCacheCount > 0)
                    {
                        Debug.Log("TreasureCacheCount " + SceneInfoTreasureCacheCount);
                        string token = "Unlock the <style=cisDamage>Rusty Lockbox</style>";
                        if (SceneInfoTreasureCacheCount > 1)
                        {
                            token += " (" + SceneInfoTreasureCacheCount + "/" + SceneInfoTreasureCacheCount + ")";
                        }
                        SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = token;
                    }
                    if (SceneInfoTreasureCacheVoidCount > 0)
                    {
                        Debug.Log("TreasureCacheVoidCount " + SceneInfoTreasureCacheVoidCount);
                        string token = "Unlock the <color=#FF9EEC>Encrusted Lockbox</color>";
                        if (SceneInfoTreasureCacheVoidCount > 1)
                        {
                            token += " (" + SceneInfoTreasureCacheVoidCount + "/" + SceneInfoTreasureCacheVoidCount + ")";
                        }
                        SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = token;
                    }
                    if (SceneInfoFreeChestCount > 0)
                    {
                        Debug.Log("FreeChestCount " + SceneInfoFreeChestCount);
                        string token = "Collect free <style=cIsHealing>delivery</style>";
                        if (SceneInfoFreeChestCount > 1)
                        {
                            token += " (" + SceneInfoFreeChestCount + "/" + SceneInfoFreeChestCount + ")";
                        }
                        SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = token;
                    }
                }
            }

            if (self.teleporterSpawnCard != null)
            {
                if (self.teleporterSpawnCard.name == "iscLunarTeleporter")
                {
                    RoR2.UI.ChargeIndicatorController[] tempchargelist = FindObjectsOfTypeAll(typeof(RoR2.UI.ChargeIndicatorController)) as RoR2.UI.ChargeIndicatorController[];
                    for (var i = 0; i < tempchargelist.Length; i++)
                    {
                        if (tempchargelist[i].name.StartsWith("TeleporterChargingPositionIndicator(Clone)"))
                        {
                            tempchargelist[i].iconSprites[0].sprite = PrimordialTeleporterChargedIcon;
                        }
                    }
                    tempchargelist = null;
                }
            }


        }

        static void GetDotDef()
        {

            DotController.GetDotDef(DotController.DotIndex.Helfire).associatedBuff = FakeHellFire;
            DotController.GetDotDef(DotController.DotIndex.PercentBurn).associatedBuff = FakePercentBurn;

            /*
            if (HelfireDebuffConfig.Value == true)
            {
                DotController.GetDotDef(DotController.DotIndex.Helfire).associatedBuff = FakeHellFire;

                object[] argument = new object[0];
                argument = argument.Add(DotController.DotIndex.Helfire);
                (typeof(DotController).GetMethod("GetDotDef", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new DotController(), argument)).SetFieldValue<BuffDef>("associatedBuff", FakeHellFire);
            }
            if (EnemyPercentColor.Value == true)
            {
                object[] argument2 = new object[0];
                argument2 = argument2.Add(DotController.DotIndex.PercentBurn);
                (typeof(DotController).GetMethod("GetDotDef", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(new DotController(), argument2)).SetFieldValue<BuffDef>("associatedBuff", FakePercentBurn);
            }
            */
        }




        private static void PassengerExitMethod(On.RoR2.SurvivorPodController.orig_OnPassengerExit orig, global::RoR2.SurvivorPodController self, GameObject passenger)
        {
            orig(self, passenger);

            //On.EntityStates.Toolbot.ToolbotStanceA.OnEnter -= MULTEquipmentThing;
            //On.RoR2.SurvivorPodController.OnPassengerExit -= PassengerExitMethod;
            //On.RoR2.Run.AdvanceStage -= RunAdvanceStageMethodT;
        }

        private static void RunAdvanceStageMethodT(On.RoR2.Run.orig_AdvanceStage orig, global::RoR2.Run self, global::RoR2.SceneDef nextScene)
        {
            orig(self, nextScene);

            //On.EntityStates.Toolbot.ToolbotStanceA.OnEnter -= MULTEquipmentThing;
            //On.RoR2.SurvivorPodController.OnPassengerExit -= PassengerExitMethod;
            //On.RoR2.Run.AdvanceStage -= RunAdvanceStageMethodT;


        }

        private static void RunAdvanceStageMethodAlways(On.RoR2.Run.orig_AdvanceStage orig, global::RoR2.Run self, global::RoR2.SceneDef nextScene)
        {

            orig(self, nextScene);
        }








        private static bool ColliderPickup(Collider collider)
        {
            if (collider.GetComponent<RoR2.PickupDropletController>()) return true;
            if (collider.GetComponent<GenericPickupController>()) return true;
            if (collider.GetComponent<PickupPickerController>()) return true;

            return false;
        }

        private static void PickupTeleportHook()
        {
            On.RoR2.MapZone.TryZoneStart += (orig, self, collider) =>
            {
                orig(self, collider);
                if (self.zoneType == MapZone.ZoneType.OutOfBounds)
                {
                    //Debug.LogWarning(collider);
                    //Debug.LogWarning(ColliderPickup(collider));
                    if (ColliderPickup(collider))
                    {

                        SpawnCard spawnCard = ScriptableObject.CreateInstance<SpawnCard>();
                        spawnCard.hullSize = HullClassification.Human;
                        spawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
                        spawnCard.prefab = LegacyResourcesAPI.Load<GameObject>("SpawnCards/HelperPrefab");

                        DirectorPlacementRule placementRule = new DirectorPlacementRule
                        {
                            placementMode = DirectorPlacementRule.PlacementMode.NearestNode,
                            position = collider.transform.position
                        };

                        GameObject gameObject = DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(spawnCard, placementRule, RoR2Application.rng));


                        if (gameObject)
                        {
                            Debug.Log("tp item back");
                            TeleportHelper.TeleportGameObject(collider.gameObject, gameObject.transform.position);
                            UnityEngine.Object.Destroy(gameObject);
                        }

                        UnityEngine.Object.Destroy(spawnCard);
                    }
                }


            };
        }

        private static void PickupBackgroundCollision()
        {
            // - 15 : Background , 13 : Pickup
            Physics.IgnoreLayerCollision(15, 13, false);
        }




    }




    //[RequireComponent(typeof(GenericOwnership))]
    public class VagrantExplosionWarningBuff : NetworkBehaviour
    {
        private void FixedUpdate()
        {
            if (NetworkServer.active)
            {
                this.ServerFixedUpdate();
            }
        }

        private void ServerFixedUpdate()
        {
            float num = Mathf.Clamp01(this.previousCycle.timeSince / this.cycleInterval);
            int num2 = (num == 1f) ? this.cycleTargets.Count : Mathf.FloorToInt((float)this.cycleTargets.Count * num);
            while (this.cycleIndex < num2)
            {
                HurtBox hurtBox = this.cycleTargets[this.cycleIndex];
                if (hurtBox)
                {
                    CharacterBody body = hurtBox.healthComponent.body;
                    body.AddTimedBuff(this.buffDef, this.nearBuffDuration);
                }
                this.cycleIndex++;
            }
            if (this.previousCycle.timeSince >= this.cycleInterval)
            {
                this.previousCycle = Run.FixedTimeStamp.now;
                this.cycleIndex = 0;
                this.cycleTargets.Clear();
                this.SearchForTagets3(this.cycleTargets);

            }
        }

        private void SearchForTagets3(List<HurtBox> dest)
        {

            BlastAttack.HitPoint[] array = new BlastAttack
            {
                attacker = attacker,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
                position = transPos.position,
                radius = this.radius,
                losType = BlastAttack.LoSType.NearestHit,
                teamIndex = this.teamIndex,
            }.CollectHits();

            foreach (BlastAttack.HitPoint hitpoint in array)
            {
                dest.Add(hitpoint.hurtBox);
            }
        }

        public TeamIndex teamIndex = TeamIndex.None;

        public BuffDef buffDef;

        public GameObject attacker;
        public Transform transPos;

        public float cycleInterval = 0.1f;

        public float nearBuffDuration = 0.2f;

        public float radius = 80f;


        private Run.FixedTimeStamp previousCycle = Run.FixedTimeStamp.negativeInfinity;

        private int cycleIndex;

        private List<HurtBox> cycleTargets = new List<HurtBox>();

    }



    public class LootBoxObjectiveProvider : NetworkBehaviour
    {

        private void OnEnable()
        {
            if (!InstanceTracker.Any<LootBoxObjectiveProvider>())
            {
                ObjectivePanelController.collectObjectiveSources += LootBoxObjectiveProvider.collectObjectiveSourcesDelegate;
            }
            InstanceTracker.Add<LootBoxObjectiveProvider>(this);
        }

        private void OnDisable()
        {
            InstanceTracker.Remove<LootBoxObjectiveProvider>(this);
            if (!InstanceTracker.Any<LootBoxObjectiveProvider>())
            {
                ObjectivePanelController.collectObjectiveSources -= LootBoxObjectiveProvider.collectObjectiveSourcesDelegate;
            }
        }


        private static void CollectObjectiveSources(CharacterMaster viewer, List<ObjectivePanelController.ObjectiveSourceDescriptor> dest)
        {
            foreach (LootBoxObjectiveProvider source in InstanceTracker.GetInstancesList<LootBoxObjectiveProvider>())
            {
                dest.Add(new ObjectivePanelController.ObjectiveSourceDescriptor
                {
                    master = viewer,
                    objectiveType = typeof(LootBoxObjectiveProvider.GenericObjectiveTracker),
                    source = source
                });
            }
        }


        public string objectiveToken;
        public int currentCount;
        public int maxCount;

        public bool markCompletedOnRetired = true;

        private static readonly Action<CharacterMaster, List<ObjectivePanelController.ObjectiveSourceDescriptor>> collectObjectiveSourcesDelegate = new Action<CharacterMaster, List<ObjectivePanelController.ObjectiveSourceDescriptor>>(GenericObjectiveProvider.CollectObjectiveSources);


        public class GenericObjectiveTracker : ObjectivePanelController.ObjectiveTracker
        {
            public override string GenerateString()
            {
                LootBoxObjectiveProvider provider = (LootBoxObjectiveProvider)this.sourceDescriptor.source;
                this.previousToken = provider.objectiveToken;

                if (provider.maxCount > 1)
                {
                    return provider.objectiveToken + "(" + provider.currentCount + "/" + provider.maxCount + ")";
                }

                return provider.objectiveToken;
            }


            public override bool IsDirty()
            {
                return ((LootBoxObjectiveProvider)this.sourceDescriptor.source).objectiveToken != this.previousToken;
            }

            public override bool shouldConsiderComplete
            {
                get
                {
                    return this.retired && ((LootBoxObjectiveProvider)this.sourceDescriptor.source).markCompletedOnRetired;
                }
            }

            private string previousToken;
        }
    }

}