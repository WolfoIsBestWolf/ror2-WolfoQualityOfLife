using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions.Options;
using RiskOfOptions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    public class WConfig
    {

        public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoFixes.cfg", true);

        public static ConfigEntry<bool> cfgTextItems;
        public static ConfigEntry<bool> cfgTextCharacters;
        public static ConfigEntry<bool> cfgMithrix4Skip;
        public static ConfigEntry<bool> cfgItemTags;
        public static ConfigEntry<bool> cfgDevotionSpareDroneParts;
        public static ConfigEntry<bool> cfgXILaser;
        public static ConfigEntry<bool> cfgDisable;
        public static ConfigEntry<bool> cfgSlayerScale;
        public static ConfigEntry<bool> cfgFalseSonP2;

        public static ConfigEntry<bool> cfgTestMultiplayer;
        public static ConfigEntry<bool> cfgTestLogbook;
        public static ConfigEntry<bool> cfgLoadOrder;

        public static void Start()
        {

            InitConfig();

        }


        public static void InitConfig()
        {
            cfgDisable = ConfigFile_Client.Bind(
               "Main",
               "Disable everything except needed",
               false,
               "Disable all changes except things needed for mod compatibility"
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
            cfgMithrix4Skip = ConfigFile_Client.Bind(
               "Gameplay",
               "Fix Mithrix P4 Skip",
               true,
               "This is without question a bug. Some people may still like it despite this."
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
               "Bug introduced with DLC2, probably low on the priority which is why it has not been fixed."
           );
            cfgItemTags = ConfigFile_Client.Bind(
                "Gameplay",
                "Item Tag Changes",
                true,
                "Adds AIBlacklist to more items that are uselss or overpowered on enemies as multiple of my mods deal with that.\nMoves around certain categories on items.\nBug fixes like Harpoon not being tagged as OnKill still happen even if turned of"
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
                "Slayer damage type scales proc damage"
            );
             cfgItemTags = ConfigFile_Client.Bind(
                "Gameplay",
                "Item Tag Changes",
                true,
                "Adds AIBlacklist to more items that are uselss or overpowered on enemies."
            );



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
 
            ConfigEntryBase[] entries = ConfigFile_Client.GetConfigEntries();
            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    var temp = (ConfigEntry<bool>)entry;
                    ModSettingsManager.AddOption(new CheckBoxOption(temp, true));
                }
                else
                {
                    Debug.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }

        }

    }

}
