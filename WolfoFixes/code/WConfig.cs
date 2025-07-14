using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    internal class WConfig
    {

        public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoFixes.cfg", true);

        public enum Mith4
        {
            Unchanged,
            StealAfterStun,
            Fix
        }

        public static ConfigEntry<bool> cfgTextItems;
        public static ConfigEntry<bool> cfgTextCharacters;
        public static ConfigEntry<bool> cfgMithrix4Skip;

        public static ConfigEntry<bool> cfgItemTags;
        public static ConfigEntry<bool> cfgDevotionSpareDroneParts;
        public static ConfigEntry<bool> cfgXILaser;
        public static ConfigEntry<bool> cfgDisable;
        public static ConfigEntry<bool> cfgSlayerScale;
        public static ConfigEntry<bool> cfgFalseSonP2;
        public static ConfigEntry<bool> cfgFunnyIceSpear;

        public static ConfigEntry<bool> cfgTestMultiplayer;
        public static ConfigEntry<bool> cfgTestLogbook;
        public static ConfigEntry<bool> cfgLoadOrder;

        public static void Awake()
        {

            InitConfig();

        }


        public static void InitConfig()
        {

            cfgMithrix4Skip = ConfigFile_Client.Bind(
               "Gameplay",
               "Fix Mithrix P4 Skip",
               false,
               "This bug happens because SetStateOnHurt consider Mithrixes health 0 for 1 frame and staggers him.\n\nOff by default due to feedback."
           );
            cfgMithrix4Skip.SettingChanged += BodyFixes.SetSkippable;
            cfgFunnyIceSpear = ConfigFile_Client.Bind(
               "Gameplay",
               "Fix Ice Spear Physics",
               true,
               "This bug happens because Ice Spears physics layer was set incorrectly at some point."
           );
            //Is he tho?
            /*cfgFalseSonP2 = ConfigFile_Client.Bind(
                "Gameplay",
                "Fix False Son P2 Shotgun",
                true,
                "False Son Phase 2 is intended to use the spike shotgun, but can't due to a bugged skill driver."
            );*/
            cfgXILaser = ConfigFile_Client.Bind(
                "Gameplay",
                "Fix XI Laser",
                true,
                "This bug happens because the laser state just ends instead of checking what it should do next. \n\nProbably low on the priority which is why it has not been fixed."
            );
            cfgItemTags = ConfigFile_Client.Bind(
                "Gameplay",
                "Item Tag Changes",
                true,
                "AIBlacklist Nkuhanas Opinion and Infusion.\nMoves around certain categories on items.\nBug fixes like Harpoon not being tagged as OnKill still happen even if turned of"
            );
            cfgDevotionSpareDroneParts = ConfigFile_Client.Bind(
                "Gameplay",
                "Devotion Tag",
                true,
                "Add the Devotion Tag to Devoted Lemurians which will make them work with Spare Drone Parts.\n\nThis is a intended synergy, they just put in the wrong Lemurian."
            );
            cfgSlayerScale = ConfigFile_Client.Bind(
                "Gameplay",
                "Slayer scales Procs",
                false,
                "Slayer damage type scales proc damage\nMight not really be a bug."
            );


            cfgDisable = ConfigFile_Client.Bind(
                          "Main",
                          "Disable everything except needed",
                          false,
                          "Disable all changes except things needed for mod compatibility and debugging tools"
            );
            cfgTextItems = ConfigFile_Client.Bind(
                "Main",
                "Fixed Item Descriptions",
                true,
                "Updated and fixed descriptions for items. Disable if other mods change stats or items."
            );

            cfgTextCharacters = ConfigFile_Client.Bind(
                "Main",
                "Fixed Survivor Descriptions",
                true,
                "Updated and fixed descriptions for character. Disable if other mods change stats or items."
            );

            #region Test
            cfgTestLogbook = ConfigFile_Client.Bind(
               "Testing",
               "Everything Logbook",
               false,
               "Add all items, equipments and mobs to logbook, including untiered and cut."
            );
            cfgTestMultiplayer = ConfigFile_Client.Bind(
                "Testing",
                "Multiplayer Test",
                false,
                "Allows you to join yourself via connect localhost:7777"
            );
            cfgLoadOrder = ConfigFile_Client.Bind(
                 "Testing",
                 "Debugging",
                 false,
                 "Log prints when certain events happen"
             );
            #endregion
        }



        public static void RiskConfig()
        {
            ModSettingsManager.SetModIcon(Addressables.LoadAssetAsync<Sprite>(key: "8d5cb4f0268083645999f52a10c6904b").WaitForCompletion());
            ModSettingsManager.SetModDescription("Random assortment of fixes for bugs that bothered me.");

            List<ConfigEntry<bool>> noResetB = new List<ConfigEntry<bool>>()
            {
                cfgMithrix4Skip
            };

            ConfigEntryBase[] entries = ConfigFile_Client.GetConfigEntries();
            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    var temp = (ConfigEntry<bool>)entry;
                    ModSettingsManager.AddOption(new CheckBoxOption(temp, !noResetB.Contains(temp)));
                }
                else
                {
                    Debug.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }

        }

    }

}
