using BepInEx;
using BepInEx.Configuration;


namespace WolfoQualityOfLife
{
    public class WConfig
    {
        //public static ConfigFile ConfigFileVisuals = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL-Visuals.cfg", true);
        //public static ConfigFile ConfigFileText = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL-Text.cfg", true);
        //public static ConfigFile ConfigFileUI = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL-UI.cfg", true);
        //public static ConfigFile ConfigFileMisc = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL-Misc.cfg", true);
        public static ConfigFile ConfigFileUNSORTED = new ConfigFile(Paths.ConfigPath + "\\Wolfo.Wolfo_QualityOfLife.cfg", true);

        //MoreMessages
        public static ConfigEntry<bool> cfgMessageDeath;
        public static ConfigEntry<bool> cfgMessagePrint;
        public static ConfigEntry<bool> cfgMessageScrap;
        public static ConfigEntry<bool> cfgMessageVictory;
        public static ConfigEntry<bool> cfgMessageVoidTransform;
        public static ConfigEntry<bool> cfgMessageElixir;
        public static ConfigEntry<bool> cfgMessagesColoredItemPings;

        //Reminders
        public static ConfigEntry<bool> cfgRemindersPortal;
        public static ConfigEntry<bool> cfgRemindersTreasure;
        public static ConfigEntry<bool> cfgChargeHalcyShrine;

        //Text Changes
        public static ConfigEntry<bool> cfgTextMain;
        public static ConfigEntry<bool> cfgTextItems;
        public static ConfigEntry<bool> cfgTextCharacters;

        public static ConfigEntry<string> LunarChimeraNameChange;
        public static ConfigEntry<bool> cfgTextEndings;
        public static ConfigEntry<bool> cfgBlueTextPrimordial;

        public static ConfigEntry<bool> MoreLogEntries;
        public static ConfigEntry<bool> MoreLogEntriesAspect;




        //Misc 
        public static ConfigEntry<float> cfgPingDurationMultiplier;
        public static ConfigEntry<bool> cfgNewSprintCrosshair;
        public static ConfigEntry<bool> cfgEquipmentDroneName;
        public static ConfigEntry<bool> cfgMainMenuRandomizer;
        public static ConfigEntry<bool> cfgMainMenuScav;
        public static ConfigEntry<bool> cfgMountainStacks;



        //Visuals
        public static ConfigEntry<bool> cfgSkinAcridBlight;
        public static ConfigEntry<bool> cfgSkinMercRed;
        public static ConfigEntry<bool> cfgSkinMisc;
        public static ConfigEntry<bool> cfgSkinBellBalls;

        public static ConfigEntry<bool> cfgSkinMakeOniBackup;
        public static ConfigEntry<bool> cfgSkinMakeBlightedAcrid;

        public static ConfigEntry<bool> cfgLunarEliteDisplay;
        public static ConfigEntry<bool> OldModelDuplcators;

        //UI
        public static ConfigEntry<bool> cfgIconsUsedKey;
        public static ConfigEntry<bool> cfgIconsUsedPrayer;
        public static ConfigEntry<bool> cfgIconsBodyIcons;

        //Buff Timers
        public static ConfigEntry<bool> cfgBuff_BugFlight;
        public static ConfigEntry<bool> cfgBuff_Strides;
        public static ConfigEntry<bool> cfgBuff_RepeatColors;
        public static ConfigEntry<bool> cfgBuff_SprintArmor;
        public static ConfigEntry<bool> cfgBuff_Frozen;

        public static ConfigEntry<bool> cfgBuff_Headstomper;
        public static ConfigEntry<bool> cfgBuff_Feather;
        public static ConfigEntry<bool> cfgBuff_ShieldOpalCooldown;
        public static ConfigEntry<bool> cfgBuff_FrostRelic;

        //public static ConfigEntry<bool> cfgBuff_Egg;
        //public static ConfigEntry<bool> cfgBuff_HelfireDuration;
        //
        public static ConfigEntry<bool> cfgTpIconDiscoveredRed;
        public static ConfigEntry<bool> cfgVoidAllyCyanEyes;
        public static ConfigEntry<bool> cfgNewGeysers;
        public static ConfigEntry<bool> cfgExpandedDeathScreen;
        public static ConfigEntry<bool> cfgDeathScreenStats;

        public static ConfigEntry<bool> cfgPingIcons;
    
        public static ConfigEntry<bool> EnableColorChangeModule;

        public static ConfigEntry<bool> ArtifactOutline;


        public static ConfigEntry<bool> DummyModelViewer;

        public static ConfigEntry<bool> cfgRiskyStuff;


        public static void Start()
        {
            InitConfig();
            BuffConfig();
        }


        public static void InitConfig()
        {
            cfgRiskyStuff = ConfigFileUNSORTED.Bind(
               "Oddities",
               "Disable buff sorter",
               true,
               "As an emergency in case it breaks I guess."
            );

            cfgMainMenuRandomizer = ConfigFileUNSORTED.Bind(
               "Other",
               "Main Menu Colors",
               true,
               "Should the Main Menu change between themes. If Starstorm is enabled stormtheme will also random be disabled"
           );
            cfgMainMenuScav = ConfigFileUNSORTED.Bind(
    "Other",
    "Main Menu Scav",
    true,
    "Should a Scav be readded to the options screen like in CU3, and a Gup added to the alternative game modes screen."
);
            cfgMessageDeath = ConfigFileUNSORTED.Bind(
               "More Messages",
               "Enable Detailed Death Messages",
               true,
               "Death Messages telling you who died to what or in what way."
            );
            cfgMessagePrint = ConfigFileUNSORTED.Bind(
                "More Messages",
                "Enable Item Loss Messages",
                true,
                "When someone Prints, Reforges or Cleanses a message will be sent"
            );
            cfgMessageScrap = ConfigFileUNSORTED.Bind(
                "More Messages",
                "Enable Item Loss Scrapping",
                true,
                "When someone Scraps, a message will be sent"
            );
            cfgMessageVictory = ConfigFileUNSORTED.Bind(
                "More Messages",
                "Enable Victory Messages",
                true,
                "Victory quote will be put into chat"
            );
            cfgMessageVoidTransform = ConfigFileUNSORTED.Bind(
                "More Messages",
                "Messages for Benthic Bloom and Egocentrism",
                true,
                "You get a message for what Void upgrade and Egocentrism transformations"
            );
            cfgMessageElixir = ConfigFileUNSORTED.Bind(
                "More Messages",
                "Messages when a Elixir gets used",
                true,
                "You get a message for when you use a Elixir"
            );
            cfgMessagesColoredItemPings = ConfigFileUNSORTED.Bind(
                "More Messages",
                "Colored Item names in Pings",
                true,
                "When pinging an item or something containing an item, the items name will use it's color."
            );
            //More Messages
            //
            //Reminder
            cfgRemindersPortal = ConfigFileUNSORTED.Bind(
                "Reminders",
                "Reminders for Portals",
                true,
                "Just looks nice but can be disabled if you think it's cluttery."
            );
            cfgRemindersTreasure = ConfigFileUNSORTED.Bind(
                "Reminders",
                "Objective Reminders for Lockboxes and Delivery",
                true,
                "Friendly reminder to open your Lockboxes and get your free items from Shipping Form."
            );
            cfgChargeHalcyShrine = ConfigFileUNSORTED.Bind(
                "Reminders",
                "Objective for charging any activated Halcyon Shrines",
                true,
                "To match other waiting sections in the game."
            );

            //
            //Text
            cfgTextMain = ConfigFileUNSORTED.Bind(
                "Text Changes",
                "Text Main",
                true,
                "Various text changes."
            );
            cfgTextItems = ConfigFileUNSORTED.Bind(
                "Text Changes",
                "Item descriptions",
                true,
                "Updated and fixed descriptions for items. Disable if other mods change stats or items."
            );
            cfgTextCharacters = ConfigFileUNSORTED.Bind(
                "Text Changes",
                "Character descriptions",
                true,
                "Updated and fixed descriptions for character. Disable if other mods change stats or items."
            );
            LunarChimeraNameChange = ConfigFileUNSORTED.Bind(
                "Text Changes",
                "Lunar Chimera name changes",
                "Long",
                "Change the name of the 3 Lunar Chimera\nPossible Options\nShort :       Lunar Wisp, Lunar Golem, Lunar Exploder\nLong :       Lunar Chimera (Wisp), Lunar Chimera (Golem), Lunar Chimera (Exploder)\nVanilla :   Lunar Chimera, Lunar Chimera, Lunar Chimera"
            );
            cfgTextEndings = ConfigFileUNSORTED.Bind(
                "Text Changes",
                "Endings have different text",
                true,
                "Twisted Scav and Void ending have their color and text changed to reflect their ending."
            );
            cfgBlueTextPrimordial = ConfigFileUNSORTED.Bind(
                "Text Changes",
                "Blue Text Primordial TP objective",
                false,
                "Make the objective have blue text when it is a primordial teleporter."
            );
            //
            //Extra Icons
            cfgIconsBodyIcons = ConfigFileUNSORTED.Bind(
                "Extra Icons",
                "More unique Body Icons",
                true,
                "Add new or updated Body Icons for things such as Engi Walker Turrets, Empathy Cores or blue outline Squid Turret"
            );
            cfgIconsUsedKey = ConfigFileUNSORTED.Bind(
               "Extra Icons",
               "Consumed Rusted Keys",
               true,
               "Just like how Dio leaves a Consumed Dio"
           );
            //
            //Visuals
            OldModelDuplcators = ConfigFileUNSORTED.Bind(
                "Visuals",
                "Large and Mili Printer Old Model",
                true,
                "Change the model of Green and Red Printers to the bigger bulkier one used before 1.0"
            );
            cfgSkinBellBalls = ConfigFileUNSORTED.Bind(
                "Visuals",
                "Elite Brass Contraption colored Balls",
                true,
                "Their balls use the elite aspects texture."
            );
            cfgSkinMisc = ConfigFileUNSORTED.Bind(
                "Visuals",
                "Fix up some small things for vanilla skins",
                true,
                "REX uses colored vines when moving, Fixes Alt Engi turrets looking wrong"
            );
            cfgSkinMercRed = ConfigFileUNSORTED.Bind(
                 "Visuals",
                "Oni Merc Red Sword and Red Attack Visuals",
                true,
                "To fit better with his Red skin. Includes a skin that keeps the blue sword for people who prefer it."
            );
            cfgSkinMakeOniBackup = ConfigFileUNSORTED.Bind(
                 "Visuals",
                "Make a backup of the Oni with blue sword skin so you can have both a Red and a Blue (v2)",
                false,
                ""
            );
            cfgSkinAcridBlight = ConfigFileUNSORTED.Bind(
                "Visuals",
               "Blight Acrid attacks different color",
               true,
               "Attacks that apply blight are colored Purple/Orange instead of Green/Yellow. Includes a variant of the default skin."
           );


            cfgPingIcons = ConfigFileUNSORTED.Bind(
                "Extra Icons",
                "Enable/Disable Ping Icons",
                true,
                "Enable or Disable new Ping Icons in general"
            );
            MoreLogEntries = ConfigFileUNSORTED.Bind(
                "Logbook",
                "More log entries",
                true,
                "Add Twisted Scav, Newt, Malachite Urchin to the logbook"
            );
            MoreLogEntriesAspect = ConfigFileUNSORTED.Bind(
                "Logbook",
                "Elite Aspect log entries",
                true,
                "Add Elite Aspects to the logbook.\nIf you're using ZetAspects disable this or that mods config else it'll result in duplicate entries for all aspects."
            );
            //
            //





            cfgEquipmentDroneName = ConfigFileUNSORTED.Bind(
                "Text Changes",
                "Equipment Name under Equipment Drone name",
                true,
                "Equipment Drone\n(Equipment Name)"
            );

            cfgNewGeysers = ConfigFileUNSORTED.Bind(
                "Visuals",
                "Change Geyser colors",
                true,
                "Abyssal Depths will have lava-like instead of water and Sulfur Pools will have greener Geysers"
            );
            cfgVoidAllyCyanEyes = ConfigFileUNSORTED.Bind(
                "Visuals",
                "Void Ally Cyan Eyes",
                true,
                "Void Allies from Newly Hatched Zoea will have bright Cyan eyes for easier identification."
            );
            cfgTpIconDiscoveredRed = ConfigFileUNSORTED.Bind(
                "Extra Icons",
                "Teleporter Icon Discovered Color Red",
                true,
                "When you have the discover teleporter icon setting on. Makes the icon light red at first and only white when charged."
            );

            cfgMountainStacks = ConfigFileUNSORTED.Bind(
                            "Visuals",
                            "Show a Mountain Shrine symbol for every Mountain Shrine you activated",
                            true,
                            "Helps you count and looks funny for very high amounts"
                        );





            ///////////////////////////







            ///////////////////////////
            cfgNewSprintCrosshair = ConfigFileUNSORTED.Bind(
                "Other",
                "Sprinting Crosshair Changes",
                true,
                "Should this mod replace the Sprint crosshair with a different one that works with charging abilities."
            );


            cfgExpandedDeathScreen = ConfigFileUNSORTED.Bind(
                "Main",
                "Expand the Death Screen to show more information",
                true,
                "Equipment will be shown, the Killers inventory will be shown, death stats will be expanded, chat box will always be there."
            );

            ///////////////////////////////
            EnableColorChangeModule = ConfigFileUNSORTED.Bind(
                "Main",
                "Enable Color Changes",
                true,
                "Change the colors of Lunar/Elite equipment to something unique. Lunar Coins will use their color for their outline. Void items will have tiered colors."
            );

            ArtifactOutline = ConfigFileUNSORTED.Bind(
                "Other",
                "Artifact Outline",
                false,
                "Should Artifact Pickups in the Artifact World have a purple (Artifact Color) outline."
            );

            ///////////////////////////////
            DummyModelViewer = ConfigFileUNSORTED.Bind(
                "Other",
                "Sort of Model Viewer",
                false,
                "Just adds basically all mobs to the Logbook"
            );



        }








        public static void BuffConfig()
        {
            cfgBuff_RepeatColors = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Change Colors of buffs that use the same color/symbol",
                true,
                ""
            );
            cfgBuff_Frozen = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff Icon to being Frozen",
                true,
                "Adds 4 stacks depleting one every half second as a timer.\nHelps Artificer gameplay"
            );

            cfgBuff_Feather = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff Icon for Hopoo Feathers",
                true,
                "One stack of visual Feather Buff for each bonus mid air jump you can make due to Hopoo Feathers"
            );

            cfgBuff_ShieldOpalCooldown = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff icon for Shield & Opal",
                true,
                "Same icon as Medkit Delay but to show you how long until shields regenerate. Also adds one for Opal. This feature is intended for BetterUI users."
            );

            cfgBuff_Headstomper = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff Icon to Headstomper usage",
                true,
                "Ready and Cooldown icons just like the bands."
            );
            cfgBuff_FrostRelic = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff Icon to Frost Relic effect",
                true,
                "Stacks equals active kills"
            );


            cfgBuff_Strides = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff Icon to Strides of Heresy usage",
                true,
                ""
            );
            cfgBuff_SprintArmor = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff Icon to Rose Buckler usage",
                true,
                "Might be easy to forget that you have this item and a buff indicator could remind you"
            );

            cfgBuff_BugFlight = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff icon to Milky Chrysalis usage",
                true,
                ""
            );
            /*
            cfgBuff_HelfireDuration = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff Icon to Helfire Tincture usage",
                true,
                "Use BetterUI Buff Timers"
            );

            cfgBuff_Egg = ConfigFileUNSORTED.Bind(
                "Buff Icons",
                "Add buff Icon for being in Volcanic Egg",
                true,
                "Use BetterUI Buff Timers"
            );*/
        }


    }

}
