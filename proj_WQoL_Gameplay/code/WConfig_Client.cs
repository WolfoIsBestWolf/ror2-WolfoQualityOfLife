using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WQoL_Gameplay
{
    public class WConfig
    {

        public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL_Gameplay.cfg", true);


        //public static ConfigEntry<bool> CancelGeyserLock_Jump;
        //public static ConfigEntry<bool> CancelGeyserLock_Skill;
        public static ConfigEntry<bool> ItemsTeleport;
        public static ConfigEntry<bool> cfgLava;
        public static ConfigEntry<bool> cfgRedWhip;

        public static ConfigEntry<EclipseArtifact> EclipseAllowArtifacts;
        public static ConfigEntry<bool> cfgPrismatic;

        public enum EclipseArtifact
        {
            Off,
            VanillaWhitelist,
            Blacklist,
        }

        public static void Start()
        {
            WGQoLMain.log.LogMessage("WQoL InitConfig");

            InitConfig();


        }


        public static void InitConfig()
        {

            /* CancelGeyserLock_Jump = ConfigFile_Client.Bind(
                 "Other",
                 "Regain Air Control from Jumping",
                 true,
                 "Using midair jump unlocks air movement if locked by Geyser. Such as Hopoo Feather"
             );
             CancelGeyserLock_Skill = ConfigFile_Client.Bind(
                 "Other",
                 "Regain Air Control from Skills",
                 true,
                 "Using utility skills unlocks air movement if locked by Geyser."
             );*/
            ItemsTeleport = ConfigFile_Client.Bind(
                "Main",
                "Items Teleport",
                true,
                "Items and Dead Drones will teleport if they fall of the map."
            );
            EclipseAllowArtifacts = ConfigFile_Client.Bind(
                "Game",
                "Eclipse Artifacts",
                EclipseArtifact.VanillaWhitelist,
                "Allows some artifacts to be voted for in Eclipse.\nJust ones that are purely challenge or variety. (And are compatible with the mode).\n\nOption to enable most artifacts if desired."
            );
            cfgPrismatic = ConfigFile_Client.Bind(
            "Game",
            "Prismatic Trials",
           true,
            "Allow Prismatic Trials during modded"
        );
            cfgLava = ConfigFile_Client.Bind(
                "Main",
                "Lava Consistency",
                true,
                "Makes damage for player-allies 2%, like enemies. Instead of 10%, which is intended for players only.\n\nLava can make things like Devotion borderline unplayable"
            );
            cfgRedWhip = ConfigFile_Client.Bind(
                "Main",
                "Red Whip Consistency",
                true,
                "More non damaging moves no longer put you into combat.\n\nREX Util\nLoader Secondary\nRailgunner Zoom"
            );


        }



        public static void RiskConfig()
        {
            ModSettingsManager.SetModIcon(Addressables.LoadAssetAsync<Sprite>(key: "0630116480f210e489cde8f7e69e28a9").WaitForCompletion());
            ModSettingsManager.SetModDescription("Random assortment of gameplay quality of life.");

            List<ConfigEntry<bool>> noReset = new List<ConfigEntry<bool>>()
            {
                //CancelGeyserLock_Skill,

            };

            var entries = ConfigFile_Client.GetConfigEntries();
            //WGQoLMain.log.LogMessage("Config Values Total : " + entries.Length);
            //WGQoLMain.log.LogMessage("Config Values Reset : " + (resetB.Count));
            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    var temp = (ConfigEntry<bool>)entry;
                    ModSettingsManager.AddOption(new CheckBoxOption(temp, true));
                }
                else if (entry.SettingType == typeof(int))
                {
                    ModSettingsManager.AddOption(new IntFieldOption((ConfigEntry<int>)entry, false));
                }
                else if (entry.SettingType == typeof(float))
                {
                    ModSettingsManager.AddOption(new FloatFieldOption((ConfigEntry<float>)entry, false));
                }
                else if (entry.SettingType == typeof(EclipseArtifact))
                {
                    ModSettingsManager.AddOption(new ChoiceOption((ConfigEntry<EclipseArtifact>)entry, false));
                }
                else
                {
                    WGQoLMain.log.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }


        }







    }

}
