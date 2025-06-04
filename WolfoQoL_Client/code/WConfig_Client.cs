using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using System.Collections.Generic;
using UnityEngine;

namespace WolfoQoL_Client
{
    public class WConfig
    {

        public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL_Client.cfg", true);

        public static ConfigEntry<bool> cfgSkipItemDisplays;
        public static ConfigEntry<bool> cfgOldSotsEliteIcons;
        public static ConfigEntry<bool> cfgDarkTwisted;
        public static ConfigEntry<bool> cfgMithrixPhase4SkipFix;
        public static ConfigEntry<bool> cfgLoadoutOnDeathScreen;
        public static ConfigEntry<bool> cfgSmoothCaptain;

        //Visuals
        public static ConfigEntry<bool> cfgSkinAcridBlight;
        public static ConfigEntry<bool> cfgSkinMercRed;
        public static ConfigEntry<bool> cfgSkinMercGreen;
        public static ConfigEntry<bool> cfgSkinMercDisableCompletely;
        public static ConfigEntry<bool> cfgSkinBellBalls;

        //public static ConfigEntry<bool> cfgSkinMakeOniBackup;
        public static ConfigEntry<bool> cfgSkinMakeBlightedAcrid;
        public static ConfigEntry<bool> cfgLogbook_SortBosses;


        //OjbectiveStuff
        public static ConfigEntry<float> cfgObjectiveHeight;
        public static ConfigEntry<float> cfgObjectiveFontSize;

        //MoreMessages
        public static ConfigEntry<bool> cfgMessageDeath;
        public static ConfigEntry<bool> cfgMessagePrint;
        public static ConfigEntry<bool> cfgMessageScrap;
        public static ConfigEntry<bool> cfgMessageVictory;
        public static ConfigEntry<bool> cfgMessageVoidTransform;
        public static ConfigEntry<MessageWho> cfgMessageElixir;
        public static ConfigEntry<bool> cfgMessagesColoredItemPings;
        public static ConfigEntry<bool> cfgMessagesVoidQuantity;
        public static ConfigEntry<bool> cfgMessagesShrineRevive;
        public static ConfigEntry<bool> cfgMessagesRevive;
        public static ConfigEntry<bool> cfgMessagesSaleStar;
        public static ConfigEntry<bool> cfgMessagesRecycler;

        //Reminders
        public static ConfigEntry<bool> cfgRemindersGeneral;

        public static ConfigEntry<bool> cfgHelminthFix;

        public static ConfigEntry<bool> cfgRemindersKeys;
        public static ConfigEntry<bool> cfgRemindersFreechest;
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
            You,
            Anybody
        }
        public enum Position
        {
            Off,
            Top,
            Bottom
        }

        //DeathScreen
        public static ConfigEntry<bool> cfgExpandedDeathScreen;
        public static ConfigEntry<bool> cfgDeathScreenStats;
        public static ConfigEntry<bool> cfgDeathScreenAlwaysChatBox;



        //Text Changes
        public static ConfigEntry<bool> cfgTextOther;
        public static ConfigEntry<bool> cfgTextItems;
        public static ConfigEntry<bool> cfgTextCharacters;


        public static ConfigEntry<bool> cfgPrimordialBlueText;
        public static ConfigEntry<bool> cfgPrimordialBlueIcon;
        public static ConfigEntry<bool> cfgPrimordialBlueHighlight;
        public static ConfigEntry<bool> cfgPrismArtifacts;

        public static ConfigEntry<bool> cfgLogbook_More;
        public static ConfigEntry<bool> cfgLogbook_EliteEquip;


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
 
        public static ConfigEntry<bool> cfgSkinMercRedSword;
        public static ConfigEntry<bool> cfgSkinMercGreenGlow;
        public static ConfigEntry<bool> cfgSkinEngiHarpoons;
        public static ConfigEntry<bool> cfgSkinMisc;

 

        public static ConfigEntry<bool> cfgLunarEliteDisplay;
        public static ConfigEntry<bool> OldModelDuplcators;

        //UI

        public static ConfigEntry<bool> cfgIconsBodyIcons;

        //
        public static ConfigEntry<bool> cfgTpIconDiscoveredRed;
        public static ConfigEntry<bool> cfgVoidAllyCyanEyes;
        public static ConfigEntry<bool> cfgNewGeysers;


        public static ConfigEntry<bool> cfgPingIcons;

        public static ConfigEntry<bool> cfgColorMain;

        public static ConfigEntry<bool> ArtifactOutline;
        public static ConfigEntry<bool> cfgBuff_RepeatColors;
        public static ConfigEntry<bool> cfgFragmentColor;
        public static ConfigEntry<bool> cfgFragmentSheen;

        public static ConfigEntry<bool> cfgKillerInvAlwaysDisplay;

        public static ConfigEntry<bool> cfgLunarSeerName;

        public static ConfigEntry<bool> cfgMissionPointers;
        public static ConfigEntry<bool> cfgMissionPointersVoid;
        public static ConfigEntry<bool> cfgMissionPointersLunar;
        public static ConfigEntry<bool> cfgMissionPointersGold;

        //public static ConfigEntry<bool> cfgTestDisable;
        public static ConfigEntry<bool> cfgTestMultiplayer;
        public static ConfigEntry<bool> cfgTestClient;
        public static ConfigEntry<bool> cfgTestDisableHostInfo;
        public static ConfigEntry<bool> cfgTestLogbook;
        public static ConfigEntry<bool> cfgTestDisableMod;
        public static ConfigEntry<bool> cfgTestDisableMod2;

        public static ConfigEntry<bool> cfgVoidPotential_ItemsInPing;
        public static ConfigEntry<bool> cfgAurFragment_ItemsInPing;
        public static ConfigEntry<bool> cfgPlayerPing;
        public static ConfigEntry<bool> cfgGameplay;
        public static ConfigEntry<bool> SulfurPoolsSkin;

        public enum Player
        {
            Off,
            Multiplayer,
            Always,
        }


        public static void Start()
        {
            Debug.Log("WQoL InitConfig");
            InitConfig();
            TestConfig();


            cfgObjectiveHeight.SettingChanged += UpdateHuds;
            cfgObjectiveFontSize.SettingChanged += UpdateHuds;
        }


        public static void InitConfig()
        {
            #region Reminders
            cfgRemindersGeneral = ConfigFile_Client.Bind(
               "Reminders",
               "Objective Reminders : General",
               true,
               "Turn off all extra objective reminders by the mod. Use individual config if you only want to turn off one part."
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
               "Reminder for open portals in case you forget and to match the leave through teleporter message. Like Blue/Gold/Green."
           );

            cfgRemindersSecretGeode = ConfigFile_Client.Bind(
                "Reminders",
                "Meridian Geode Secret",
                true,
                "Cracking the 6 Aurelionite Geodes while climbing Prime Meridian gives an extra reward. This is to remind you to do that."
            );
            cfgChargeHalcyShrine = ConfigFile_Client.Bind(
                "Reminders",
                "Charging Halcyon Shrines",
                true,
                "To match other waiting sections in the game."
            );
            cfgRemindersNewt = ConfigFile_Client.Bind(
              "Reminders",
              "Newt Shrine",
              ReminderChoice.Off,
              "If you, really need to got the Bazaar.\nAuto clears if Teleporter spawns with a free blue orb."
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
                "Chat message when Egocentrism, Benthic Bloom or VanillaVoids Enhancement Vials upgrades a item."
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
            cfgMessageElixir = ConfigFile_Client.Bind(
                "Chat Messages",
                "Elixir & Watch lost",
                MessageWho.Anybody,
                "Chat message for when you use Elixir, lose Watches, lose random item with VanillaVoids Clockwork Mechanism"
            );
            cfgMessagesRecycler = ConfigFile_Client.Bind(
                 "Chat Messages",
                 "Recycler Message",
                 true,
                 "When an item is Recycled by anyone, a message will be displayed saying what turned into what."
              );

            cfgMessagesVoidQuantity = ConfigFile_Client.Bind(
                "Chat Messages",
                "Void Quantity",
                true,
                "When picking up a item or void, it will add up the quantities of the normal and void together in the message.\nie if you pickup a Bear with Safer Spaces itll show Tougher Times(2) in a void color"
            );
            cfgMessagesShrineRevive = ConfigFile_Client.Bind(
                "Chat Messages",
                "Shaping Activated",
                true,
                "Chat message for when Shrine of Shaping is activated."
            );
           
            #endregion
            #region Hud - MidRun
            //UI -> Hud?
            cfgMissionPointers = ConfigFile_Client.Bind(
               "Hud",
               "Mission Pointers",
               true,
               "Void Fields : Indicator where the next Cell is.\n\nCommencement : Lunar Pillar locations shown briefly after charging one.\n\nGilded Coast : Location of last 3 Halcyon Beacon"
           );

            cfgNewSprintCrosshair = ConfigFile_Client.Bind(
              "Hud",
              "Sprinting Crosshair Changes",
              true,
              "Should this mod replace the Sprint crosshair with a different one that works with charging abilities."
          );
            cfgBuff_RepeatColors = ConfigFile_Client.Bind(
                 "Hud",
                 "Buff Recolors",
                 true,
                 "Most notable I guess Red Whip buff is now Orange instead of Blue, which is now only for the Invisibility speed buff"
             );
            cfgTpIconDiscoveredRed = ConfigFile_Client.Bind(
              "Hud",
              "Teleporter Icon Discovered Color Red",
              true,
              "When you have the discover teleporter icon setting on. Makes the icon light red at first and only white when charged."
            );

            cfgPingIcons = ConfigFile_Client.Bind(
               "Hud",
               "Enable new Ping Icons",
               true,
               "Enable new Ping Icons"
           );
            cfgIconsBodyIcons = ConfigFile_Client.Bind(
                "Hud",
                "More unique Body Icons",
                true,
                "Add new or updated Body Icons\n Engi Walker Turrets, Empathy Cores and blue outline Squid Turret & Devoted Lemurians"
            );
            cfgObjectiveHeight = ConfigFile_Client.Bind(
               "Hud",
               "Hud Objective Spacing",
               30f,
               "Vanilla is 32. \nHow much space between objectives in the hud. Can look better or worse depending on language."
            );
            cfgObjectiveFontSize = ConfigFile_Client.Bind(
               "Hud",
               "Hud Objective Font Size",
               12f,
               "Vanilla is 12. \nSize of letters for objectives in the hud. Can look better or worse depending on language."
            );
            #endregion

            #region Visuals

            cfgHelminthFix = ConfigFile_Client.Bind(
               "Visuals",
               "Brighter Helminth",
               true,
               "Fix Helminths lighting being darker than what is seemingly intended."
           );

            cfgMountainStacks = ConfigFile_Client.Bind(
             "Visuals",
             "Mountain Shrine Stack",
             true,
             "One icon per mountain shrine activated."
          );
            SulfurPoolsSkin = ConfigFile_Client.Bind(
                "Visuals",
                "Sulfur Pool Beetles Skin",
                true,
                "Implement the unused SotV Sulfur Pools skin for the Beetle family"
            );
            cfgSkinBellBalls = ConfigFile_Client.Bind(
               "Visuals",
               "Elite Brass Contraption Balls",
               true,
               "Their balls use the elite aspects texture."
             );

            cfgColorMain = ConfigFile_Client.Bind(
                "Visuals",
                "Enable Color Changes",
                true,
                "Change the colors of Lunar/Elite equipment to something unique. Lunar Coins will use their color for their outline. Void items will have tiered colors."
            );
            cfgFragmentColor = ConfigFile_Client.Bind(
               "Visuals",
               "Fragment Gold Outline",
               true,
               "Should Aurelionite Fragments have a gold outline like it's sprite."
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
            cfgVoidAllyCyanEyes = ConfigFile_Client.Bind(
                "Visuals",
                "Void Ally Cyan Eyes",
                true,
                "Void Allies from Newly Hatched Zoea will have bright Cyan eyes for easier identification."
            );
            #endregion

            #region Text changes that aren't new chat messages
            cfgLunarSeerName = ConfigFile_Client.Bind(
              "Text",
              "Lunar Seer include Stage Name",
              true,
              "Include Stage Name in Ping and Interact info."
            );
            cfgEquipmentDroneName = ConfigFile_Client.Bind(
               "Text",
               "Equipment Name Equipment Drone",
               true,
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
            cfgTextItems = ConfigFile_Client.Bind(
                "Text",
                "Fixed Item Descriptions",
                true,
                "Updated and fixed descriptions for items. Disable if other mods change stats or items."
            );
            cfgTextCharacters = ConfigFile_Client.Bind(
                "Text",
                "Fixed Survivor Descriptions",
                true,
                "Updated and fixed descriptions for character. Disable if other mods change stats or items."
            );
            cfgTextOther = ConfigFile_Client.Bind(
                "Text",
                "Other Text Changes",
                true,
                "Adds unique name tokens for Large 3D Printers, Large Multi-Shops, Equipment Multi-Shops, Shipping Multi-Shop, some alt ending types."
            );


            #endregion
            #region Menus
            cfgUISimuBorder = ConfigFile_Client.Bind(
               "Menu",
               "Simulacrum Completed Border",
               true,
               "During Simulacrum Lobbies, Characters you have beaten Wave 50 with will have a pink border around their icon."
           );
            cfgUIEclipseBorder = ConfigFile_Client.Bind(
                "Menu",
                "Eclipse Number",
                true,
                "During Eclipse Lobbies, Show what eclipse was beaten as number under each character."
            );
            cfgLogbook_More = ConfigFile_Client.Bind(
                "Menu",
                "More log entries",
                true,
                "Add some missing mobs and equipment to the log.\n\nTwisted Scav, Geep, Gip, Malachite Urchin, Fuel Array, Consumed Seed of Life, Consumed Tricorn"
            );
            cfgLogbook_EliteEquip = ConfigFile_Client.Bind(
                "Menu",
                "Elite Aspect log entries",
                true,
                "Add Elite Aspect Equipment to the logbook."
            );
            cfgLogbook_SortBosses = ConfigFile_Client.Bind(
                "Menu",
                "Sort Bosses",
                true,
                "Sort Bosses to the end of the Monster category"
            );

            cfgExpandedDeathScreen = ConfigFile_Client.Bind(
               "Menu",
               "Expanded Death Screen",
               true,
               "What Equipment you had will be shown.\nIf your killer had any items, they will be displayed.\nUseful for things such as Scavengers and Phase 4 Mithrix."
           );
            cfgLoadoutOnDeathScreen = ConfigFile_Client.Bind(
               "Menu",
               "Loadout Death Screen",
               true,
               "Players skill loadouts will be shown, for sharing screenshots primarily."
           );
            cfgDeathScreenAlwaysChatBox = ConfigFile_Client.Bind(
                "Menu",
                "Death Screen Always show Chat",
                true,
                "Useful for showing recent activity. Intended to primarily alongside Death/Win messages so you can see them on the game over screen."
             );
            cfgDeathScreenStats = ConfigFile_Client.Bind(
                "Menu",
                "Death Screen More Stats",
                true,
                "Show more stats like Healing recieved, and reorder them."
            );
            cfgKillerInvAlwaysDisplay = ConfigFile_Client.Bind(
                "Menu",
                "Killer show Elite Equip",
                false,
                "Show the killers inventory even if they only have a Elite Equipment and no items."
            );

            cfgMainMenuRandomizer = ConfigFile_Client.Bind(
              "Menu",
              "Main Menu Colors",
              true,
              "Should the Main Menu change between themes.\nIf Starstorm is enabled, it's Storm Visuals are also sometimes disabled"
           );
            cfgMainMenuRandomizerSelector = ConfigFile_Client.Bind(
               "Menu",
               "Main Menu Selector",
               0,
               "Specifically select a main menu color :\n0 : Default or Random depending on other config \n1 : Acres update\n2 : Sirens update\n3 : Void Fields update\n4 : Artifact update\n11, 12, 13, 14 : Same but force off SS2 Storm"
            );
            cfgMainMenuScav = ConfigFile_Client.Bind(
                "Menu",
                "Main Menu Scav",
                true,
                "Should a Scav be readded to the options screen like in a old version of the game, and a Gup added to the alternative game modes screen."
            );


            #endregion
            #region Skins
            cfgSkinMercRedSword = ConfigFile_Client.Bind(
            "Skins",
            "Oni Merc Red Sword",
            true,
            "To fit better with his Red skin.\nRed effects are part of Server mod"
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
            /*cfgSkinMakeOniBackup = ConfigFile_Client.Bind(
               "Skins",
               "Oni Blue Sword Backup",
               false,
               "Make a backup of the Oni Skin, with blue sword skin so you can have both a Red and a Blue"
           );*/
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
                "Simple recolor where instead of green it's more yellow-orange"
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
            #endregion
            #region OTHER
            cfgGameplay = ConfigFile_Client.Bind(
                "Other",
                "Gameplay fixes",
                true,
                "Gameplay bug fixes and minor quality of life."
            );
            cfgSmoothCaptain = ConfigFile_Client.Bind(
                "Visuals",
                "Colossus Captain Smooth",
                true,
                "Makes the armor on the skin smooth instead of battered"
            );  
            cfgDarkTwisted = ConfigFile_Client.Bind(
                "Visuals",
                "Dark Twisted Elites",
                true,
                "Gives Twisted Elites a dark blue coloration instead of reusing Overloading."
            );
            cfgOldSotsEliteIcons = ConfigFile_Client.Bind(
                "Hud",
                "SotS Elite Icon change",
                false,
                "Change the Elite Icons of SotS elites to be more in line with the other elites.  These sprites are in the files. it is unknown why they were replaced."
            );
            cfgOldSotsEliteIcons.SettingChanged += OtherEnemies.UpdateSotsEliteIcon;
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
            cfgDelayGreenOrb = ConfigFile_Client.Bind(
              "Other",
              "Delayed Green Orb message",
              true,
              "Delays Thunder Rumbling Sots message by 1s so it opens the chat box"
           );

            #endregion
        }

        public static void TestConfig()
        {
            #region Test
            cfgTestLogbook = ConfigFile_Client.Bind(
               "Testing",
               "Everything Logbook",
               false,
               "Add all items, equipments and mobs to logbook, including unfished."
           );
            cfgTestMultiplayer = ConfigFile_Client.Bind(
                "Testing",
                "Multiplayer Test",
                false,
                "Allows you to join yourself via connect localhost:7777"
            );
            cfgTestDisableMod2 = ConfigFile_Client.Bind(
                "Testing",
                "Disable Mod",
                false,
                "Disables the mod beyond allowing multiplayer testing."
            );
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

        private static void UpdateHuds(object sender, System.EventArgs e)
        {
            UIBorders.UpdateHuds();
        }

        public static void RiskConfig()
        {
            ModSettingsManager.SetModIcon(Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/icon.png"));
            ModSettingsManager.SetModDescription("Random assortment of Quality of Life.");

            List<ConfigEntry<bool>> resetB = new List<ConfigEntry<bool>>()
            {
                SulfurPoolsSkin,
                cfgRemindersPortal,
                cfgChargeHalcyShrine,
                cfgTextItems,
                cfgTextCharacters,
                cfgTextOther,
                cfgBuff_RepeatColors,
                cfgLogbook_EliteEquip,
                cfgLogbook_More,
                cfgTestLogbook,
                cfgPingIcons,
                cfgIconsBodyIcons,
                cfgSkinEngiHarpoons,
                cfgTpIconDiscoveredRed,
                cfgVoidAllyCyanEyes,
                OldModelDuplcators,

                cfgColorMain,
                cfgTestDisableMod2,
                cfgSmoothCaptain,
                cfgDarkTwisted,
            };

            var entries = ConfigFile_Client.GetConfigEntries();
            Debug.Log("Config Values Total : " + entries.Length);
            Debug.Log("Config Values Reset : " + (resetB.Count));
            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    var temp = (ConfigEntry<bool>)entry;
                    ModSettingsManager.AddOption(new CheckBoxOption(temp, resetB.Contains(temp)));
                }
                else if (entry.SettingType == typeof(int))
                {
                    ModSettingsManager.AddOption(new IntFieldOption((ConfigEntry<int>)entry, false));
                }
                else if (entry.SettingType == typeof(float))
                {
                    ModSettingsManager.AddOption(new FloatFieldOption((ConfigEntry<float>)entry, false));
                }
                else if (entry.SettingType == typeof(MessageWho))
                {
                    ModSettingsManager.AddOption(new ChoiceOption((ConfigEntry<MessageWho>)entry, false));
                }
                else if (entry.SettingType == typeof(ReminderChoice))
                {
                    ModSettingsManager.AddOption(new ChoiceOption((ConfigEntry<ReminderChoice>)entry, false));
                }
                else
                {
                    Debug.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }

        }







    }

}
