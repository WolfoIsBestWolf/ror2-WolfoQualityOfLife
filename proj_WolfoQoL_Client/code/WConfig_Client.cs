using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using System.Collections.Generic;
using UnityEngine;
using WolfoQoL_Client.Menu;

namespace WolfoQoL_Client
{
    public static class WConfig
    {

        public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL_Client.cfg", true);

        public static ConfigEntry<MessageWho> cfgDevotedInventory;

        public static ConfigEntry<bool> SH_Music_Restarter;
        //public static ConfigEntry<bool> OperatorDroneIndicator;
        public static ConfigEntry<bool> cfgChefMenuTweak;

        public static ConfigEntry<bool> RealTimeTimer;
        public static ConfigEntry<bool> DC_RealTimeTimerStat;

        public static ConfigEntry<bool> cfgHelminthLightingFix;
        public static ConfigEntry<bool> cfgSofterShadows;

        public static ConfigEntry<bool> DroneMessage_Repair;
        public static ConfigEntry<bool> DroneMessage_Combine;
        public static ConfigEntry<bool> DroneMessage_RemoteOp;

        public static ConfigEntry<bool> cfgTempItemCyanStack;
        public static ConfigEntry<bool> cfgRecipesInLog;

        public static ConfigEntry<bool> module_visuals_skins;
        public static ConfigEntry<bool> module_visuals_icons;
        public static ConfigEntry<bool> module_visuals_other;
        public static ConfigEntry<bool> module_audio;

        public static ConfigEntry<bool> module_menu_deathscreen;
        public static ConfigEntry<bool> module_menu_logbook;
        public static ConfigEntry<bool> module_menu_other;

        public static ConfigEntry<bool> module_text_reminders;
        public static ConfigEntry<bool> module_text_chat;
        public static ConfigEntry<bool> module_text_general;


        public static ConfigEntry<bool> DC_DroneInventory;
        public static ConfigEntry<bool> DC_CompactStats;
        public static ConfigEntry<bool> DC_MoreStats;
        public static ConfigEntry<bool> DC_Loadout;

        public static ConfigEntry<bool> DC_StageRecap;
        public static ConfigEntry<bool> DC_LatestWave;

        public static ConfigEntry<bool> DC_KillerInventory;
        public static ConfigEntry<bool> DC_KillerInventory_EvenIfJustElite;

        public static ConfigEntry<bool> LogbookSimuStages;



        public static ConfigEntry<bool> cfgSkipItemDisplays;

        public static ConfigEntry<bool> cfgDarkTwisted;
        public static ConfigEntry<bool> cfgTwistedFire;
        public static ConfigEntry<bool> cfgTwistedSound;
        public static ConfigEntry<bool> cfgMithrixPhase4SkipFix;

        public static ConfigEntry<bool> cfgSmoothCaptain;

        //Visuals
        public static ConfigEntry<bool> cfgSkinAcridBlight;
        public static ConfigEntry<bool> cfgSkinMercRed;
        public static ConfigEntry<bool> cfgSkinMercGreen;
        public static ConfigEntry<bool> cfgSkinMercPink;
        public static ConfigEntry<bool> cfgSkinMercSS2;
        public static ConfigEntry<bool> cfgSkinMercDisableCompletely;
        public static ConfigEntry<bool> cfgSkinBellBalls;


        public static ConfigEntry<bool> cfgSkinMakeBlightedAcrid;

        public static ConfigEntry<bool> cfgSkinOperatorAltChirp;


        //OjbectiveStuff
        //public static ConfigEntry<float> cfgObjectiveHeight;
        //public static ConfigEntry<float> cfgObjectiveFontSize;

        //MoreMessages
        public static ConfigEntry<bool> cfgMessageDeath;
        public static ConfigEntry<bool> cfgMessagePrint;
        public static ConfigEntry<bool> cfgMessageScrap;
        public static ConfigEntry<bool> cfgMessageDroneScrap;
        public static ConfigEntry<bool> cfgMessageVictory;
        public static ConfigEntry<bool> cfgMessageVoidTransform;
        public static ConfigEntry<MessageWho> cfgMessageElixirWatch;
        public static ConfigEntry<bool> cfgMessagesColoredItemPings;
        public static ConfigEntry<bool> cfgMessagesVoidQuantity;
        public static ConfigEntry<bool> cfgMessagesShrineRevive;
        public static ConfigEntry<bool> cfgMessagesRevive;
        public static ConfigEntry<bool> cfgMessagesSaleStar;
        public static ConfigEntry<bool> cfgMessagesRecycler;
        public static ConfigEntry<bool> cfgMessageDevotion;

        //Reminders


        public static ConfigEntry<bool> cfgReminder_Halcyon;
        public static ConfigEntry<bool> cfgReminder_AccessNode;

        public static ConfigEntry<bool> cfgRemindersNOTINSIMU;
        public static ConfigEntry<bool> cfgRemindersKeys;
        public static ConfigEntry<bool> cfgRemindersFreechest;
        public static ConfigEntry<bool> cfgReminders_VV_FreechestVoid;
        public static ConfigEntry<bool> cfgReminders_Quality_Collectors;
        public static ConfigEntry<ReminderChoice> cfgRemindersRegenScrap;
        public static ConfigEntry<bool> cfgRemindersSaleStar;
        public static ConfigEntry<ReminderChoice> cfgRemindersNewt;
        public static ConfigEntry<bool> cfgRemindersPortal;
        public static ConfigEntry<bool> cfgRemindersSecretGeode;
        public static ConfigEntry<bool> cfgChargeHalcyShrine;

        public enum ReminderChoice
        {
            Off,
            IfSpawned,
            Always
        }
        public enum MessageWho
        {
            Off,
            Your,
            Anybody
        }
        public enum Position
        {
            Off,
            Top,
            Bottom
        }
        public enum ColorOrNot
        {
            Off,
            White,
            Colored
        }
        /*public enum AllyExpands
        {
            Off,
            EngiTurret,
            Orbs,
            All,
        }*/
        public enum MainMenuTheme
        {
            Default,
            Random,
            RandomStatic,
            Acres,
            Sirens,
            Realms,
            Artifact,
        }


        //Text Changes
        public static ConfigEntry<bool> cfgTextOther;
        public static ConfigEntry<bool> cfgAltBodyNames;


        public static ConfigEntry<bool> cfgPrimordialBlueText;
        public static ConfigEntry<bool> cfgPrimordialBlueIcon;
        public static ConfigEntry<bool> cfgPrimordialBlueHighlight;

        public static ConfigEntry<bool> Logbook_More;
        public static ConfigEntry<bool> Logbook_WeirdoEnemies;
        public static ConfigEntry<bool> Logbook_EliteEquip;
        public static ConfigEntry<bool> Logbook_EliteEquipEarlyViewable;
        public static ConfigEntry<bool> Logbook_Recipes;
        public static ConfigEntry<bool> cfgLogbook_Subtitles;
        public static ConfigEntry<bool> cfgLogbook_Drones_EngiTurret;
        public static ConfigEntry<bool> Logbook_AllyExpansion;
        public static ConfigEntry<bool> Logbook_SortBosses;


        //Misc 

        public static ConfigEntry<bool> cfgNewSprintCrosshair;
        public static ConfigEntry<ColorOrNot> cfgEquipmentDroneName;
        public static ConfigEntry<MainMenuTheme> cfgMainMenuRandomizer;
        public static ConfigEntry<bool> cfgMainMenuScav;

        public static ConfigEntry<bool> cfgUISimuBorder;
        public static ConfigEntry<bool> cfgUIEclipseBorder;


        //Visuals

        public static ConfigEntry<bool> cfgSkinMercRedSword;
        public static ConfigEntry<bool> cfgSkinMercGreenGlow;
        public static ConfigEntry<bool> cfgSkinEngiHarpoons;
        public static ConfigEntry<bool> cfgSkinMisc;

        public static ConfigEntry<bool> OldModelDuplcators;

        //UI

        public static ConfigEntry<bool> cfgIconsBodyIcons;

        //
        public static ConfigEntry<bool> cfgTpIconDiscoveredRed;
        public static ConfigEntry<bool> cfgVoidAllyCyanEyes;
        public static ConfigEntry<bool> cfgLoopPM_LemurianTemple;
        public static ConfigEntry<bool> cfgNewGeysers;


        public static ConfigEntry<bool> cfgPingIcons;

        public static ConfigEntry<bool> cfgLootIcons;
        public static ConfigEntry<bool> cfgVoidFieldsChargeIndicator;

        public static ConfigEntry<bool> cfgColorMain;

        public static ConfigEntry<bool> ArtifactOutline;
        public static ConfigEntry<bool> cfgBuff_RepeatColors;
        public static ConfigEntry<bool> cfgFragmentColor;
        public static ConfigEntry<bool> cfgFragmentSheen;


        public static ConfigEntry<bool> cfgLunarSeerName;

        public static ConfigEntry<bool> cfgMissionPointers;
        public static ConfigEntry<bool> cfgMissionPointersVoid;
        public static ConfigEntry<bool> cfgMissionPointersLunar;
        public static ConfigEntry<bool> cfgMissionPointersGold;

        //public static ConfigEntry<bool> cfgTestDisable;

        public static ConfigEntry<bool> cfgTestClient;
        public static ConfigEntry<bool> cfgTestDisableHostInfo;
        public static ConfigEntry<bool> cfgTestDisableMod;
        
        public static ConfigEntry<bool> cfgVoidPotential_ItemsInPing;
        public static ConfigEntry<bool> cfgAurFragment_ItemsInPing;
        public static ConfigEntry<bool> cfgPlayerPing;
        public static ConfigEntry<bool> SulfurPoolsSkin;
        public static ConfigEntry<bool> ArtificerBazaarAlways;
        public static ConfigEntry<float> ObjectiveHudSpacing;

        public enum Player
        {
            Off,
            Multiplayer,
            Always,
        }

        public const string RestartNotif = "\n\n(Requires a restart)";

        public static void Start()
        {
            Log.LogMessage("WQoL InitConfig");

            InitConfig();
            TestConfig();



        }


        public static void InitConfig()
        {
            #region MODULES

            //Ig it's fine to just be like this tbh

            //Reminders + Objectives
            //Skin Stuff
            //Death Screen Stuff
            //Gameplay ig
            //Log stuff?
            //Color Stuff
            module_text_chat = ConfigFile_Client.Bind(
                "Modules",
                "Chat Messages",
                true,
                "Enable all new chat messages." + RestartNotif
            );
            module_text_reminders = ConfigFile_Client.Bind(
                "Modules",
                "Reminders",
                true,
                "Enable all new objectives and reminders." + RestartNotif
            );
            module_text_general = ConfigFile_Client.Bind(
                "Modules",
                "Text Additions",
                true,
                "Enable all misc text changes.\n(ie Lunar Seer, Equipment Drone Equipment, New interactable names)" + RestartNotif
            );
            module_menu_deathscreen = ConfigFile_Client.Bind(
                "Modules",
                "Death Screen",
                true,
                "Enable the remade Death Screen:\nMore Stats\nRun Recap\nKiller's Inventory\nDrone Collection\ntop right Win Message, bigger." + RestartNotif
            );
            module_menu_logbook = ConfigFile_Client.Bind(
                "Modules",
                "Logbook",
                true,
                "Enable Logbook Additions:\n\nDrones -> Drones & Allies\nItems entries show crafting recipes\nMonster & Survivor entries show more stats and subtitle\n" + RestartNotif
            );
            module_menu_other = ConfigFile_Client.Bind(
                "Modules",
                "Menu & Hud Tweaks",
                true,
                "Enable menu & hud tweaks:\n\nMain Menu theme randomizer\nEclipse number in lobby\nSimu Completed border\nReal time timer" + RestartNotif
            );

            module_visuals_skins = ConfigFile_Client.Bind(
                "Modules",
                "Skin & Skill Visuals",
                true,
                "Enable all skin & skill visual upgrades:\nRed Oni Merc\nGreen Frail Merc\nPurple/Yellow Blight Acrid\n" + RestartNotif
            );
            module_visuals_icons = ConfigFile_Client.Bind(
                "Modules",
                "Icons",
                true,
                "Enable all updated icons:\n\nUnique Ping Icons\nUpdated Body Icons" + RestartNotif
            );
            module_visuals_other = ConfigFile_Client.Bind(
                "Modules",
                "Visuals",
                true,
                "Enable all other visual tweaks:\n\nLarger Uncommon Printers\nSulfur & Magma Geysers\nDifferent colors for Lunar/Elite equips" + RestartNotif
            );
            module_audio = ConfigFile_Client.Bind(
                "Modules",
                "Audio",
                true,
                "Parrying effect is more noticible\nSolutional Haunt Music track restarts after boss fight." + RestartNotif
            );

            #endregion

            #region Reminders

            cfgRemindersNOTINSIMU = ConfigFile_Client.Bind(
                "Reminders",
                "Disable reminders in Simulacrum",
                true,
                "Disable all reminders in Simulacrum.\n\nSale Star and Regen Scrap will always be disabled."
            );
            cfgRemindersKeys = ConfigFile_Client.Bind(
               "Reminders",
               "Rusted Keys",
               true,
               "Reminder to open your Rusted & Encrusted Key lockboxes on stages where they spawn."
           );
            cfgRemindersFreechest = ConfigFile_Client.Bind(
                "Reminders",
                "Shipping Request",
                true,
                "Reminder to get your free item from Shipping Request Form."
            );
            cfgRemindersRegenScrap = ConfigFile_Client.Bind(
                "Reminders",
                "Regen Scrap",
                ReminderChoice.IfSpawned,
                "Reminder to use your Regenerating Scrap if you have one. Can choose to only be reminded if a Green Printer has spawned."
            );

            cfgRemindersSaleStar = ConfigFile_Client.Bind(
                "Reminders",
                "Sale Star",
                true,
                "Reminder, that you have Sale Star, so you hopefully do not waste it."
            );

            cfgRemindersPortal = ConfigFile_Client.Bind(
               "Reminders",
               "Portals",
               true,
               "Reminder for open portals, in case you forget one is opened and just default to the teleporter.\n\nAlso to match the leave through teleporter message."
           );

            cfgRemindersSecretGeode = ConfigFile_Client.Bind(
                "Reminders",
                "Meridian Geode Secret",
                true,
                "Cracking the 6 Aurelionite Geodes while climbing Prime Meridian gives an extra reward.\nThis is to remind you to do that."
            );
            cfgChargeHalcyShrine = ConfigFile_Client.Bind(
                "Reminders",
                "Charging Halcyon Shrines",
                true,
                "Shows percent. To match other waiting sections in the game."
            );
            cfgReminder_Halcyon = ConfigFile_Client.Bind(
             "Reminders",
             "Find Halcyon Shrine",
             false,
             "Reminder to find the Halcyon Shrine, if one spawned."
           );

            cfgRemindersNewt = ConfigFile_Client.Bind(
              "Reminders",
              "Newt Shrine",
              ReminderChoice.Off,
              "If you, really need to got the Bazaar.\nAuto clears if Teleporter spawns with a free blue orb."
            );
            cfgReminder_AccessNode = ConfigFile_Client.Bind(
              "Reminders",
              "Access Node",
              false,
              "Reminder to hit that Access Node before the teleport on Stage 3s and Repurposed Crater"
            );
            cfgReminders_VV_FreechestVoid = ConfigFile_Client.Bind(
              "Reminders",
              "(Vanilla Voids) Ceaseless Cornucopia",
              false,
              "Reminder to complete the Void Holdout Zone event that spawns from (VanillaVoids) Ceaseless Cornucopia (Void Shipping Request)"
            );
            cfgReminders_Quality_Collectors = ConfigFile_Client.Bind(
              "Reminders",
              "(Quality) Collectors Compulsion",
              true,
              "Reminder for Quality Collectors Compulsion in QualityMod, to open the golden stat up barrel that spawns."
            );


            #endregion
            #region More Messages -> Specially when we add chat messages
            cfgMessageDeath = ConfigFile_Client.Bind(
               "Chat Messages",
               "Detailed Death Messages",
               true,
               "Death Messages telling you who died to what and in what way."
            );
            cfgMessagePrint = ConfigFile_Client.Bind(
                "Chat Messages",
                "Printing Message",
                true,
                "When someone Prints, Reforges or Cleanses a message will be sent"
            );
            cfgMessageScrap = ConfigFile_Client.Bind(
                "Chat Messages",
                "Scrapping Message",
                true,
                "When someone Scraps, a message will be sent"
            );
            cfgMessageVictory = ConfigFile_Client.Bind(
                "Chat Messages",
                "Victory Messages",
                true,
                "Victory and Vanish quote will be put into chat."
            );
            cfgMessageVoidTransform = ConfigFile_Client.Bind(
                "Chat Messages",
                "Benthic & Ego Transform",
                true,
                "Chat message for when;\nEgocentrism eats an item\nBenthic Bloom upgrades an item.\n(VanillaVoids) Enhancement Vials corrupts an item."
            );
            cfgMessagesRevive = ConfigFile_Client.Bind(
                 "Chat Messages",
                 "Revive Messages",
                 true,
                 "Message for when someone dies and revives using an item.\n\nWith more ways to revive, might be good to know when it's because of an item."
              );
            cfgMessagesSaleStar = ConfigFile_Client.Bind(
                 "Chat Messages",
                 "Sale Star Message",
                 true,
                 "Message to show when and on what someone used their Sale Stars on."
              );
            cfgMessageElixirWatch = ConfigFile_Client.Bind(
                "Chat Messages",
                "Elixir & Watch lost",
                MessageWho.Anybody,
                "Chat message for when;\nAn Elixir is used\nDelicate Watches are broken\nA random item is eaten by (VanillaVoids) Clockwork Mechanism"
            );
            cfgMessagesRecycler = ConfigFile_Client.Bind(
                 "Chat Messages",
                 "Recycler Message",
                 true,
                 "When an item is Recycled by anyone, a message will be displayed saying what turned into what."
              );
            DroneMessage_Repair = ConfigFile_Client.Bind(
                 "Chat Messages",
                 "Drone Repair Message",
                 true,
                 "Message for repairing drones and purchasing them from a Drone Triple Shop."
              );
            DroneMessage_Combine = ConfigFile_Client.Bind(
                "Chat Messages",
                "Drone Combiner Message",
                true,
                "Message for combining/upgrading drones at the Drone CombinerStation."
             );
            DroneMessage_RemoteOp = ConfigFile_Client.Bind(
                "Chat Messages",
                "Drone RemoteOp Message",
                true,
                "Message for starting a remote operation / respawning as a drone."
             );
            cfgMessageDevotion = ConfigFile_Client.Bind(
                "Chat Messages",
                "Devotion Message",
                true,
                "Message for recruiting and losing Devoted Lemurians."
            );

            cfgMessagesVoidQuantity = ConfigFile_Client.Bind(
                "Chat Messages",
                "Void Quantity",
                true,
                "When picking up a item or void, it will add up the quantities of the normal and void together in the message.\n\nie if you pickup a Bear with Safer Spaces itll show Tougher Times(2) in a void color"
            );
            cfgMessagesShrineRevive = ConfigFile_Client.Bind(
                "Chat Messages",
                "Shaping Activated",
                true,
                "Chat message for when Shrine of Shaping is activated and who it is reviving."
            );

            #endregion

            #region Menu & Hud Stuff Misc
            //UI -> Hud?

            ObjectiveHudSpacing = ConfigFile_Client.Bind(
              "Menu & Hud",
              "Objective spacing",
              -2f,
              "Shrink or increase the spacing between each objective in the hud.\n\nThis mod may add many objectives which can look rather overwhelming, shrinking the spacing is to make it more compact."
            );
            RealTimeTimer = ConfigFile_Client.Bind(
            "Menu & Hud",
            "Real Time Timer",
            true,
            "Adds a secondary timer, underneath the Run Timer / Wave Counter when Scoreboard is open.\n\nThis uses the full time of the run including time spent in Hidden Realms"
          );
            cfgNewSprintCrosshair = ConfigFile_Client.Bind(
               "Menu & Hud",
               "Sprinting Crosshair Changes",
               true,
               "Should this mod replace the Sprint crosshair with a different one that works with charging abilities."
             );
            cfgDevotedInventory = ConfigFile_Client.Bind(
             "Menu & Hud",
             "Devoted Lemurian  Inventory",
             MessageWho.Anybody,
             "Show the inventories of Devoted Lemurians per player."
            );
            cfgMainMenuRandomizer = ConfigFile_Client.Bind(
                "Menu & Hud",
                "Main Menu Colors",
                MainMenuTheme.Random,
                "Main Menu theme config.\nDuring early access the main menu changed a couple times to reflect the update, this config this back.\n\nRandom: Switches between Default/Acres/Sirens only.\nRandomStatic: Randomizes once per session."
             );

            cfgMainMenuScav = ConfigFile_Client.Bind(
                "Menu & Hud",
                "Main Menu Scav",
                true,
                "Should a Scav be readded to the options screen like in a old version of the game, and a Gup added to the alternative game modes screen."
            );
            cfgChefMenuTweak = ConfigFile_Client.Bind(
              "Menu & Hud",
              "Bigger Crafting Menu",
              true,
              "Make the Chef crafting menu quite a bit wider and have more columns."
            );
            cfgTempItemCyanStack = ConfigFile_Client.Bind(
               "Menu & Hud",
               "Blue Stack Number for Temps",
               true,
               "Item stacks that have temporary items in it, have their stack number slightly blue.\n\nCan't show temp items seperately like in returns."
           );

            cfgUISimuBorder = ConfigFile_Client.Bind(
               "Menu & Hud",
               "Simulacrum Completed Border",
               true,
               "In Simulacrum lobby & pre-screen\nCharacters you have beaten Wave 50 with will have a pink border around their icon."
           );
            cfgUIEclipseBorder = ConfigFile_Client.Bind(
                "Menu & Hud",
                "Eclipse Number",
                true,
                "In Eclipse lobby & pre-screen\nShow what the next eclipse level to beat is, as number under each character, or a border if Eclipse 8 was beaten."
            );

            #endregion

            #region Icon Related
            cfgMissionPointers = ConfigFile_Client.Bind(
               "Icons",
               "Mission Pointers",
               true,
               "Void Fields : Indicator where the next Cell is.\n\nCommencement : Lunar Pillar locations shown briefly after charging one.\n\nGilded Coast : Location of last 3 Halcyon Beacon"
           );

            cfgBuff_RepeatColors = ConfigFile_Client.Bind(
                 "Icons",
                 "Buff Recolors",
                 true,
                 "Recolors buffs that use the same sprite and color as other buffs to be distinguishable.\n\nRed Whip -> Orange\nSmall Armor -> Dark Yellow\nCelestial Cloaking -> Greener."
             );
            cfgTpIconDiscoveredRed = ConfigFile_Client.Bind(
              "Icons",
              "Teleporter Icon Discovered Color Red",
              true,
              "When you have the discover teleporter icon setting on. Makes the icon light red at first and only white when charged."
            );

            cfgPingIcons = ConfigFile_Client.Bind(
               "Icons",
               "Enable new Ping Icons",
               true,
               "Enable new Ping Icons"
            );
            cfgLootIcons = ConfigFile_Client.Bind(
              "Icons",
              "Loot dependent Icons",
              true,
              "Certain pickups will use a different icon such as Equipment, Temp Items, Coins, Elite Aspects, Quest Items."
           );
            cfgVoidFieldsChargeIndicator = ConfigFile_Client.Bind(
              "Icons",
              "Cell Vent charge icon.",
              true,
              "Add a charge indicator to Void Fields holdout zones to match, every other holdout zone."
           );
            cfgIconsBodyIcons = ConfigFile_Client.Bind(
                "Icons",
                "More unique Body Icons",
                true,
                "Add new or updated Body Icons\n Engi Walker Turrets, Empathy Cores and blue outline Squid Turret & Devoted Lemurians"
            );

            #endregion


            #region Visuals

            cfgHelminthLightingFix = ConfigFile_Client.Bind(
               "Visuals",
               "Brighter Helminth Hatchery",
               true,
               "Change Helminths lighting to be considerably brighter and to what is seemingly intended.\n\nThis happens because the lighting is set to a too low priority and/or gets improperly loaded."
           );
            cfgSofterShadows = ConfigFile_Client.Bind(
               "Visuals",
               "Brighter Shadows",
               true,
               "Lessens the darkness of shadows in Scorched Acres, Sundered Grove and Abyssal Depths, increasing general visibility."
           );
  
            cfgColorMain = ConfigFile_Client.Bind(
                "Visuals",
                "Item & Equipment Outline Color Changes",
                true,
                "Change the colors of Lunar/Elite equipment to a different blue/yellow\nLunar Coins will use the Lunar Coin color instead of Lunar Item\nVoid White will be slightly lighter\nVoid Red slightly more saturated."
            );
            cfgFragmentColor = ConfigFile_Client.Bind(
               "Visuals",
               "Fragment Gold Outline",
               true,
               "Should Aurelionite Fragments have a gold outline like it's sprite."
           );

            cfgLoopPM_LemurianTemple = ConfigFile_Client.Bind(
                "Visuals",
                "Loop Meridian & Reformed",
                true,
                "Changes the colors the visible 'Treeborn Colony' parts in Prime Meridian & Reformed Altar to match Golden Dieback during loops, as it is visible from both stages."
            );
             cfgNewGeysers = ConfigFile_Client.Bind(
                "Visuals",
                "Change Geyser colors",
                true,
                "Abyssal Depths will have lava-like instead of water\nSulfur Pools will have greener Geysers"
            );
            OldModelDuplcators = ConfigFile_Client.Bind(
               "Visuals",
               "Large and Mili Printer Old Model",
               true,
               "Change the model of Green and Red Printers to the bigger bulkier one used before 1.0.\nUncommon 3D printers are the only interactable that looks exactly the same as a different one."
           );
 
            #endregion

            #region Text Stuff Other
            cfgLunarSeerName = ConfigFile_Client.Bind(
              "Text",
              "Lunar Seer include Stage Name",
              true,
              "Include Stage Name in Ping and Interact info."
            );
            cfgEquipmentDroneName = ConfigFile_Client.Bind(
               "Text",
               "Equipment Name     Equipment Drone",
               ColorOrNot.White,
               "Show the equipment a Equipment Drone is holding in its name."
            );
            cfgVoidPotential_ItemsInPing = ConfigFile_Client.Bind(
                 "Text",
                 "Void Potential contents in Ping",
                 true,
                 "When pinged, the items inside will be in the ping message. Requested for more easy sharing in Simulacrum"
             );
            cfgAurFragment_ItemsInPing = ConfigFile_Client.Bind(
                 "Text",
                 "Aurelionite Fragment contents in Ping",
                 true,
                 "When pinged, the items inside will be in the ping message."
             );
            cfgPlayerPing = ConfigFile_Client.Bind(
                 "Text",
                 "Player Ping",
                 true,
                 "When pinging a player or ally, it will be blue and friendly instead of attack:"
             );
            cfgMessagesColoredItemPings = ConfigFile_Client.Bind(
              "Text",
              "Colored Item names in Pings",
              true,
              "When pinging an item or something containing an item, the items name will use it's color."
          );
            cfgAltBodyNames = ConfigFile_Client.Bind(
                "Text",
                "Lunar Chimera / Larva name change",
                true,
                "Adds Lunar Chimera type to Chimera names\n\nRenames Larva to Acid Larva due to their internal name."
            );
            cfgTextOther = ConfigFile_Client.Bind(
                "Text",
                "Rename repeated tokens",
                true,
                "Adds a different name token for some interactables or endings dont use a unique one.\n\nLarge 3D Printers\nLarge Multishops\nEquipment Multishops\nShipping Request Multishop\nMoment Whole ending\nVoid ending"
            );

            #endregion
            #region Death Screen

            DC_Loadout = ConfigFile_Client.Bind(
               "Death Screen",
               "Skill Loadout",
               true,
               "Players skill loadouts will be shown, for sharing and inspecting."
           );

            DC_MoreStats = ConfigFile_Client.Bind(
                "Death Screen",
                "More Stats",
                true,
                "Add various custom stats and make some more vanilla stats viewable."
            );
            DC_CompactStats = ConfigFile_Client.Bind(
                "Death Screen",
                "Compact Stats",
                true,
                "Combine most stats into 2 per column and hide the score. Score per stat will be shown by hovering over it."
            );
            DC_RealTimeTimerStat = ConfigFile_Client.Bind(
                "Death Screen",
                "Total Run Time",
                true,
                "Show the total time spent in a run in the top middle."
            );
            DC_KillerInventory = ConfigFile_Client.Bind(
                "Death Screen",
                "Killer Inventory",
                true,
                "Show the Killers Inventory, if he had any items.\nIf no killer was found, tries to display Items from Artifact of Evolution & Simulacrum"
            );
            DC_StageRecap = ConfigFile_Client.Bind(
                "Death Screen",
                "Stage Recap",
                true,
                "Show stages traversed during run.\nUseful for sharing if you went through various alt paths or did optional things like Void Fields\n\nInspired by Spelunky 2"
            );
            DC_LatestWave = ConfigFile_Client.Bind(
                "Death Screen",
                "Latest Wave",
                true,
                "Show latest Simu Wave"
            );
            DC_KillerInventory_EvenIfJustElite = ConfigFile_Client.Bind(
                "Death Screen",
                "Killer Elite Equip",
                false,
                "Show the killers inventory even if they only have a Elite Equipment and no items."
            );

            #endregion
            #region Logbook
            Logbook_More = ConfigFile_Client.Bind(
                "Logbook",
                "Log | More Entries",
                true,
                "Add entries for some promiment content that is missing a log entry:\n\nTwisted Scav (Unlocked by having Moment Whole log)\nGeep & Gip\nSolus Mine\nFuel Array\nConsumed Equipment"
            );
            Logbook_WeirdoEnemies = ConfigFile_Client.Bind(
                "Logbook",
                "Monster Spawns Entries",
                false,
                "Adds entries for more rare content, that isn't too important:\n\nMalachite Urchin\nSevered Breaker\nSevered Probe\nSevered Scorcher"
            );
            LogbookSimuStages = ConfigFile_Client.Bind(
                "Logbook",
                "Log | Show Simu Stages",
                false,
                "Add Simulacrum stages to log. Unlocked if beaten once."
            );
            Logbook_EliteEquip = ConfigFile_Client.Bind(
                "Logbook",
                "Log | Elite Aspects",
                true,
                "Add Elite Aspect Equipment to the logbook."
            );
            Logbook_EliteEquipEarlyViewable = ConfigFile_Client.Bind(
                "Logbook",
                "Elite Aspect viewable without pickup",
                true,
                "Elite Aspects will be viewable without the need to be picked up first.\n\nInstead viewable based on progression, such as having beaten the game on Monsoon."
            );
            Logbook_AllyExpansion = ConfigFile_Client.Bind(
                "Logbook",
                "Drones & Allies",
                false,
                "Expand Drones category to Drones & Allies, giving info on allies given by items.\n\nAutomatically disabled with Realer Cheats"
            );
            Logbook_Recipes = ConfigFile_Client.Bind(
                "Logbook",
                "Recipe Info",
                true,
                "For Alloyed Collective; Adds Recipes info to Item entries." + RestartNotif
            );
            Logbook_SortBosses = ConfigFile_Client.Bind(
                "Logbook",
                "Log | Sort Bosses",
                true,
                "Sort Bosses to the end of the Monster category"
            );

            #endregion
            #region Menus & Hud




            #endregion
            #region Skins Stuff
            cfgSkinMercRedSword = ConfigFile_Client.Bind(
            "Skins",
            "Oni Merc Red Sword",
            true,
            "To fit better with his Red skin."
         );
            cfgSkinMercRed = ConfigFile_Client.Bind(
              "Skins",
              "Oni Merc | Red effects",
              true,
              "Replace blue effects with red effects for Oni Merc / Mastery Skin"
              );
            cfgSkinMercGreen = ConfigFile_Client.Bind(
                "Skins",
                "Frail Merc | Green effects",
                true,
                "Replace blue effects with green effects for Frail Merc / Colossus Skin"
                );
            cfgSkinMercPink = ConfigFile_Client.Bind(
                "Skins",
                "Murder Merc | Pink effects",
                true,
                "Replace blue effects with pink effects for Murder Merc / Vulture Skin"
                );

            cfgSkinMercSS2 = ConfigFile_Client.Bind(
                "Skins",
                "(SS2) Heel Merc | Red effects",
                true,
                "Replace blue effects with red effects for the StarStorm2 Heel skin."
                );
            cfgSkinAcridBlight = ConfigFile_Client.Bind(
                "Skins",
                "Blight Acrid effects",
                true,
                "Attacks that apply blight are colored Purple/Orange instead of Green/Yellow. Includes a variant of the default skin."
             );
            cfgSkinMakeBlightedAcrid = ConfigFile_Client.Bind(
                "Skins",
                "Default Acrid Blight Recolor",
                false,
                "Simple recolor where instead of green it's more yellow-orange\n\nThis is a seperate skin as of now."
            );
            cfgSkinOperatorAltChirp = ConfigFile_Client.Bind(
                "Skins",
                "Operator Mastery Recolor Chirp",
                true,
                "Recolors Chirp to match the colors of the mastery skin."
            );
            cfgSkinEngiHarpoons = ConfigFile_Client.Bind(
               "Skins",
               "Skinned Engi Harpoons",
               true,
               "His other projectiles are skinned but not harpoons."
           );
            cfgSkinMisc = ConfigFile_Client.Bind(
                "Skins",
                "Smaller Skin Fixes",
                true,
                "REX uses colored vines when moving\nFixes Alt Engi turrets looking wrong\nMulT power mode cylinders will be skinned\nLoader m2 hand will be skinned"
            );
            SulfurPoolsSkin = ConfigFile_Client.Bind(
               "Skins",
               "Sulfur Pool Beetles Skin",
               true,
               "Implement the unused SotV Sulfur Pools skin for the Beetle family"
           );
            cfgSkinBellBalls = ConfigFile_Client.Bind(
               "Skins",
               "Elite attack effects",
               true,
               "Certain attacks will be elite colored to match effect.\n\nBrass Contraption balls\nTitan Fists"
             );
            cfgVoidAllyCyanEyes = ConfigFile_Client.Bind(
                "Skins",
                "Void Ally Cyan Eyes",
                true,
                "Void Allies from Newly Hatched Zoea will have bright Cyan eyes for easier identification."
            );
            #endregion

            #region OTHER
            ArtificerBazaarAlways = ConfigFile_Client.Bind(
                "Other",
                "Artificer Always in Bazaar",
                false,
                "I just think she's a nice Decoration and REX is always there."
            );
            cfgSmoothCaptain = ConfigFile_Client.Bind(
                "Skins",
                "Colossus Captain Smooth",
                false,
                "Makes the armor on the skin smooth instead of battered"
            );
            cfgTwistedFire = ConfigFile_Client.Bind(
                "Visuals",
                "Fire Twisted Elites",
                true,
                "Readds the lunar fire visual effect that twisted elites once had."
            );
            cfgDarkTwisted = ConfigFile_Client.Bind(
                "Visuals",
                "Dark Twisted Elites",
                true,
                "Gives Twisted Elites a dark blue coloration instead of reusing Overloading."
            );
            SH_Music_Restarter = ConfigFile_Client.Bind(
                           "Other",
                           "Restart Solutional Haunt music",
                           true,
                           "Re-starts the Solutional Haunt music after the Solus Wing boss fight."
                       );
            cfgPrimordialBlueText = ConfigFile_Client.Bind(
                "Other",
                "Primordial | Blue objective",
                false,
                "Teleporter objective has blue text when it is a primordial teleporter."
            );
            cfgPrimordialBlueIcon = ConfigFile_Client.Bind(
                "Other",
                "Primordial | Blue Charge Icon",
                false,
                "Charging icon will be blue instead of red."
            );
            cfgPrimordialBlueHighlight = ConfigFile_Client.Bind(
                "Other",
                "Primordial | Blue Highlight",
                false,
                "Interactable highlight will be blue instead of red."
            );




            ArtifactOutline = ConfigFile_Client.Bind(
                "Other",
                "Artifact Outline",
                false,
                "Should Artifact Pickups in the Artifact World have a purple (Artifact Color) outline."
            );


            #endregion
        }

        public static void TestConfig()
        {
            #region Test

            cfgTestClient = ConfigFile_Client.Bind(
                 "Testing",
                 "Client Test",
                 false,
                 "Client work arounds wont get removed if host has mod."
             );
            cfgTestDisableHostInfo = ConfigFile_Client.Bind(
                "Testing",
                "Disable Host Info",
                false,
                "Host methods will be disabled"
            );
            cfgTestDisableMod = ConfigFile_Client.Bind(
                "Testing",
                "Disable mod next time",
                false,
                "Mod will not load the next time."
            );
  
            #endregion
        }

        private static void LoadAll()
        {
            Assets.Bundle.LoadAllAssets();
        }

        public static void RiskConfig()
        {
            ModSettingsManager.SetModIcon(Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/icon.png"));
            ModSettingsManager.SetModDescription("Random assortment of Quality of Life.");

            List<ConfigEntryBase> resetNeeded = new List<ConfigEntryBase>()
            {
                module_menu_deathscreen,
                 module_menu_logbook,
                 module_menu_other,
                 module_text_chat,
                 module_text_general,
                 module_text_reminders,
                 module_visuals_icons,
                 module_visuals_other,
                 module_visuals_skins,
                 cfgMessagesVoidQuantity,
                SulfurPoolsSkin,
                cfgRemindersPortal,
                cfgChargeHalcyShrine,
                cfgTextOther,
                cfgBuff_RepeatColors,
                Logbook_EliteEquip,
                Logbook_Recipes,
                Logbook_SortBosses,
                Logbook_AllyExpansion,
                Logbook_More,
                cfgPingIcons,
                cfgIconsBodyIcons,
                cfgSkinEngiHarpoons,
                cfgTpIconDiscoveredRed,
                cfgVoidAllyCyanEyes,
                OldModelDuplcators,

                cfgColorMain,
 
                cfgSmoothCaptain,
                cfgDarkTwisted,
                cfgTwistedFire,
                cfgChefMenuTweak,
                cfgSkinOperatorAltChirp,
                cfgSkinAcridBlight,
                cfgSkinMakeBlightedAcrid,
            };

            var entries = ConfigFile_Client.GetConfigEntries();

            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    ModSettingsManager.AddOption(new CheckBoxOption((ConfigEntry<bool>)entry, resetNeeded.Contains(entry)));
                }
                else if (entry.SettingType == typeof(int))
                {
                    ModSettingsManager.AddOption(new IntFieldOption((ConfigEntry<int>)entry, false));
                }
                else if (entry.SettingType == typeof(float))
                {
                    ModSettingsManager.AddOption(new SliderOption(ObjectiveHudSpacing, new SliderConfig { max = 0, min = -8, FormatString = "{0:0}" }));
                }
                else if (entry.SettingType.IsEnum)
                {
                    ModSettingsManager.AddOption(new ChoiceOption(entry, false));
                }
                else
                {
                    Log.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }

            ObjectiveHudSpacing.SettingChanged += MenuMain.ObjectiveHudSpacing_SettingChanged;
            ModSettingsManager.AddOption(new GenericButtonOption("Load all Assets", "Testing", "Loads all Assets", "Load", LoadAll));
 
        }



    }

}
