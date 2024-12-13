using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using RiskOfOptions.OptionConfigs;
using UnityEngine;

namespace WolfoQualityOfLife
{
    public class WConfig
    {
        //public static ConfigFile ConfigFileVisuals = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL-Visuals.cfg", true);
        //public static ConfigFile ConfigFileText = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL-Text.cfg", true);
        //public static ConfigFile ConfigFileUI = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL-UI.cfg", true);
        //public static ConfigFile ConfigFileMisc = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL-Misc.cfg", true);
        public static ConfigFile ConfigFileUNSORTED = new ConfigFile(Paths.ConfigPath + "\\Wolfo.Wolfo_QualityOfLife.cfg", true);


        public static ConfigEntry<bool> NotRequireByAll;
        public static ConfigEntry<bool> BuffsAffectNectar;


        //OjbectiveStuff
        public static ConfigEntry<float> cfgObjectiveHeight;
        public static ConfigEntry<float> cfgObjectiveFontSize;


        //MoreMessages
        public static ConfigEntry<bool> cfgMessageDeath;
        public static ConfigEntry<bool> cfgMessagePrint;
        public static ConfigEntry<bool> cfgMessageScrap;
        public static ConfigEntry<bool> cfgMessageVictory;
        public static ConfigEntry<bool> cfgMessageVoidTransform;
        public static ConfigEntry<bool> cfgMessageElixir;
        public static ConfigEntry<bool> cfgMessagesColoredItemPings;
        public static ConfigEntry<bool> cfgMessagesVoidQuantity;

        //Reminders
        public static ConfigEntry<bool> cfgRemindersPortal;
        public static ConfigEntry<bool> cfgRemindersTreasure;
        public static ConfigEntry<bool> cfgChargeHalcyShrine;

        //Text Changes
        public static ConfigEntry<bool> cfgTextMain;
        public static ConfigEntry<bool> cfgTextItems;
        public static ConfigEntry<bool> cfgTextCharacters;

        public static ConfigEntry<bool> LunarChimeraNameChange;
        public static ConfigEntry<bool> cfgTextEndings;
        public static ConfigEntry<bool> cfgBlueTextPrimordial;

        public static ConfigEntry<bool> MoreLogEntries;
        public static ConfigEntry<bool> MoreLogEntriesAspect;




        //Misc 
        public static ConfigEntry<float> cfgPingDurationMultiplier;
        public static ConfigEntry<bool> cfgNewSprintCrosshair;
        public static ConfigEntry<bool> cfgEquipmentDroneName;
        public static ConfigEntry<bool> cfgMainMenuRandomizer;
        public static ConfigEntry<int> cfgMainMenuRandomizerSelector;
        public static ConfigEntry<bool> cfgMainMenuScav;
        public static ConfigEntry<bool> cfgMountainStacks;

        public static ConfigEntry<bool> cfgUISimuBorder;
        public static ConfigEntry<bool> cfgUIEclipseBorder;

        public static ConfigEntry<bool> cfgDelayGreenOrb;

        //Visuals
        public static ConfigEntry<bool> cfgSkinAcridBlight;
        public static ConfigEntry<bool> cfgSkinMercRed;
        public static ConfigEntry<bool> cfgSkinEngiHarpoons;
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

        public static ConfigEntry<bool> cfgLunarSeerName;

        public static void Start()
        {
            Debug.Log("WQoL InitConfig");
            InitConfig();
            BuffConfig();
            RiskConfig();
        }


        public static void InitConfig()
        {
            NotRequireByAll = ConfigFileUNSORTED.Bind(
               "!Main",
               "Allow Host/Client only",
               false,
               "This mod is intended to be used by everyone in a group. But this config can make this mod functional with people who do not have it.\nTo do this, all content that is indexed or ID'ed by the game such as Items, Buffs, Projectiles and Effects aren't added.\n\nThis will disable features related to Consumed Items, Buffs, Red Oni Merc, Blight Acrid."
            );
            BuffsAffectNectar = ConfigFileUNSORTED.Bind(
               "!Main",
               "Mod Buffs affect Nectar",
               false,
               "Should various buffs added by mod, like Rose Buckler, Head Stomper, Frost Relic, count for Growth Nectar."
            );


            cfgObjectiveHeight = ConfigFileUNSORTED.Bind(
               "UI",
               "Hud Objective Spacing",
               30f,
               "Vanilla is 32. \nHow much space between objectives in the hud. Can look better or worse depending on language."
            );

            cfgObjectiveFontSize = ConfigFileUNSORTED.Bind(
               "UI",
               "Hud Objective Font Size",
               12f,
               "Vanilla is 12. \nSize of letters for objectives in the hud. Can look better or worse depending on language."
            );
            cfgObjectiveHeight.SettingChanged += UpdateHuds;
            cfgObjectiveFontSize.SettingChanged += UpdateHuds;






            cfgRiskyStuff = ConfigFileUNSORTED.Bind(
               "Oddities",
               "Disable buff sorter",
               true,
               "As an emergency in case it breaks I guess."
            );
            cfgDelayGreenOrb = ConfigFileUNSORTED.Bind(
               "Other",
               "Delayed Green Orb message",
               true,
               "Delays Thunder Rumbling Sots message by 1s so it opens the chat box"
            );

            cfgMainMenuRandomizer = ConfigFileUNSORTED.Bind(
               "UI",
               "Main Menu Colors",
               true,
               "Should the Main Menu change between themes. If Starstorm is enabled stormtheme will also random be disabled"
            );
            cfgMainMenuRandomizerSelector = ConfigFileUNSORTED.Bind(
               "UI",
               "Main Menu Selector",
               0,
               "Specifically select a main menu color :\n0 : Default or Random depending on other config \n1 : Acres update\n2 : Sirens update\n3 : Void Fields update\n4 : Artifact update\n11, 12, 13, 14 : Same but force off SS2 Storm"
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
                "Chat message when Egocentrism, Benthic Bloom or VanillaVoids Enhancement Vials upgrades a item."
            );
            cfgMessageElixir = ConfigFileUNSORTED.Bind(
                "More Messages",
                "Messages when a Elixir gets used",
                true,
                "Chat message for when you use Elixir, lose Watches, lose random item with VanillaVoids Clockwork Mechanism"
            );
            cfgMessagesColoredItemPings = ConfigFileUNSORTED.Bind(
                "UI",
                "Colored Item names in Pings",
                true,
                "When pinging an item or something containing an item, the items name will use it's color."
            );
            cfgMessagesVoidQuantity = ConfigFileUNSORTED.Bind(
                "More Messages",
                "Void Quantity",
                true,
                "When picking up a item or void, it will add up the quantities of the normal and void together in the message.\nie if you pickup a Bear with Safer Spaces itll show Tougher Times(2) in a void color"
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
                "Objective : Charging Halcyon Shrines",
                true,
                "To match other waiting sections in the game."
            );
            //
            cfgUISimuBorder = ConfigFileUNSORTED.Bind(
               "UI",
               "Simulacrum Completed Border",
               true,
               "Border for having beaten wave 50"
           ); 
            cfgUIEclipseBorder = ConfigFileUNSORTED.Bind(
                "UI",
                "Eclipse Number",
                true,
                "What eclipse was beaten as number under character."
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
                true,
                "Change the name of the 3 Lunar Chimera to include their type."
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
            cfgLunarSeerName = ConfigFileUNSORTED.Bind(
                "Text Changes",
                "Lunar Seer include Stage Name",
                true,
                "Include Stage Name in Ping and Interact info."
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
                "Elite Brass Contraption colored Balls.",
                true,
                "Their balls use the elite aspects texture. (This is currently broken, the effect system was reworked and this doesn't work properly with it anymore because the new version is lacking.)"
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
            cfgSkinEngiHarpoons = ConfigFileUNSORTED.Bind(
                 "Visuals",
                "Skinned Engi Harpoons",
                true,
                "His other projectiles are skinned but not harpoons. If the different colored trails bother you ig"
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
                "UI",
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

        private static void UpdateHuds(object sender, System.EventArgs e)
        {
            Reminders.UpdateHuds();
        }

        public static void RiskConfig()
        {
            Texture2D TexChestCasinoIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/icon.png");
            Sprite ChestCasinoIcon = Sprite.Create(TexChestCasinoIcon, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
            ModSettingsManager.SetModIcon(ChestCasinoIcon);
            ModSettingsManager.SetModDescription("Random assortment of Quality of Life.");


            ModSettingsManager.AddOption(new FloatFieldOption(cfgObjectiveHeight, false));
            ModSettingsManager.AddOption(new FloatFieldOption(cfgObjectiveFontSize, false));


            ModSettingsManager.AddOption(new CheckBoxOption(cfgMainMenuRandomizer));
            ModSettingsManager.AddOption(new IntFieldOption(cfgMainMenuRandomizerSelector));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgUISimuBorder));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgUIEclipseBorder));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgTpIconDiscoveredRed));

            

            ModSettingsManager.AddOption(new CheckBoxOption(cfgMessageDeath, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgMessagePrint, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgMessageScrap, true));
            CheckBoxConfig overwriteName = new CheckBoxConfig
            {
                name = "Messages for Benthic+Egocentrism",
                restartRequired = true,
            };
            ModSettingsManager.AddOption(new CheckBoxOption(cfgMessageVoidTransform, overwriteName));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgMessageElixir, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgMessagesColoredItemPings, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgMessagesVoidQuantity, true));

            ModSettingsManager.AddOption(new CheckBoxOption(cfgRemindersPortal, true));

            overwriteName = new CheckBoxConfig
            {
                name = "Reminders for Lockboxes/Delivery",
                restartRequired = true,
            };
            ModSettingsManager.AddOption(new CheckBoxOption(cfgRemindersTreasure, overwriteName));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgChargeHalcyShrine, true));

            ModSettingsManager.AddOption(new CheckBoxOption(cfgTextMain, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgTextItems, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgTextCharacters, true));
            //ModSettingsManager.AddOption(new CheckBoxOption(cfgBlueTextPrimordial, true));


            ModSettingsManager.AddOption(new CheckBoxOption(cfgBuff_ShieldOpalCooldown, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgBuff_Headstomper, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgBuff_SprintArmor, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgBuff_FrostRelic, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgBuff_BugFlight, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgBuff_Strides, true));     
            ModSettingsManager.AddOption(new CheckBoxOption(cfgBuff_Frozen, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgBuff_Feather, true));
           
           


            //ModSettingsManager.AddOption(new FloatFieldOption(cfgPingDurationMultiplier));

            ModSettingsManager.AddOption(new CheckBoxOption(NotRequireByAll, true));
            ModSettingsManager.AddOption(new CheckBoxOption(BuffsAffectNectar, true));

            
            ModSettingsManager.AddOption(new CheckBoxOption(cfgSkinBellBalls, true));
            ModSettingsManager.AddOption(new CheckBoxOption(cfgSkinEngiHarpoons, true));

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
