using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using System.Collections.Generic;
using UnityEngine;

namespace WolfoQoL_Server
{
    public class WConfig
    {

        public static ConfigFile ConfigFile_Server = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL_Server.cfg", true);




        //UI
        public static ConfigEntry<bool> cfgIconsUsedKey;
        public static ConfigEntry<bool> cfgIconsUsedPrayer;


        public static ConfigEntry<bool> BuffsAffectNectar;
        public static ConfigEntry<bool> BuffsAffectDeathMark;
        //Buff Timers
        public static ConfigEntry<bool> cfgBuff_BugFlight;
        public static ConfigEntry<bool> cfgBuff_Strides;

        public static ConfigEntry<bool> cfgBuff_SprintArmor;
        public static ConfigEntry<bool> cfgBuff_Frozen;

        public static ConfigEntry<bool> cfgBuff_Headstomper;
        public static ConfigEntry<bool> cfgBuff_Feather;
        public static ConfigEntry<bool> cfgBuff_ShieldOpalCooldown;
        public static ConfigEntry<bool> cfgBuff_FrostRelic;

        public static ConfigEntry<bool> cfgBuff_Shocked; //??

        //public static ConfigEntry<bool> cfgBuff_Egg;
        //public static ConfigEntry<bool> cfgBuff_HelfireDuration;
        //
 

        public static void Start()
        {
            Debug.Log("WQoL InitConfig");
            BuffConfig(); 
            InitConfig();
           
            RiskConfig();
        }


        public static void InitConfig()
        {
            /* cfgRiskyStuff = ConfigFileUNSORTED.Bind(
                "Oddities",
                "Disable buff sorter",
                true,
                "As an emergency in case it breaks I guess."
             );*/
 
          
            cfgIconsUsedKey = ConfigFile_Server.Bind(
                  "Extra Icons",
                  "Consumed Rusted Keys",
                  true,
                  "Just like how Dio leaves a Consumed Dio"
               );
            cfgIconsUsedPrayer = ConfigFile_Server.Bind(
               "Extra Icons",
               "Consumed Prayer Beads",
               true,
               "This should tell you how many stats a player got from their Prayer Beads."
            );


        }



        public static void RiskConfig()
        {

            ModSettingsManager.SetModDescription("Random assortment of Quality of Life. Required by every player.");

            List<ConfigEntry<bool>> noResetB = new List<ConfigEntry<bool>>()
            {


            };


            var entries = ConfigFile_Server.GetConfigEntries();
            foreach (ConfigEntryBase entry in entries)
            {
                if (entry.SettingType == typeof(bool))
                {
                    var temp = (ConfigEntry<bool>)entry;
                    ModSettingsManager.AddOption(new CheckBoxOption(temp, true));
                    //ModSettingsManager.AddOption(new CheckBoxOption(temp, !noResetB.Contains(temp)));
                }
                else
                {
                    Debug.LogWarning("Could not add config " + entry.Definition.Key + " of type : " + entry.SettingType);
                }
            }



        }





        public static void BuffConfig()
        {
            BuffsAffectNectar = ConfigFile_Server.Bind(
               "Buffs",
               "Mod Buffs affect Nectar",
               false,
               "Should various buffs added by mod, (Rose Buckler, Head Stomper, Frost Relic) count for Growth Nectar."
            );
            cfgBuff_Frozen = ConfigFile_Server.Bind(
                "Buffs",
                "Frozen",
                true,
                "Icon for; 1 Stack per 0.5s of being frozen, to help visualize how long things are frozen for."
            );

            cfgBuff_Feather = ConfigFile_Server.Bind(
                "Buffs",
                "Hopoo Feathers",
                true,
                "Icon for; Extra jumps granted by Hopoo Feathers, to know how many jumps you have left, useful especially if you have a lot of them.\nAlso works for VanillaVoids : Quill"
            );

            cfgBuff_ShieldOpalCooldown = ConfigFile_Server.Bind(
                "Buffs",
                "Shield & Opal Cooldown",
                true,
                "Icon for; Shield and Opal being on cooldown. This does not come in stacks, so a mod that adds visual buff timers is recommended."
            );

            cfgBuff_Headstomper = ConfigFile_Server.Bind(
                "Buffs",
                "Headstomper",
                true,
                "Ready and Cooldown buff, like Bands or other items."
            );
            cfgBuff_FrostRelic = ConfigFile_Server.Bind(
                "Buffs",
                "Frost Relic",
                true,
                "Icon for; Frost Relic kill stacks."
            );


            cfgBuff_Strides = ConfigFile_Server.Bind(
                "Buffs",
                "Strides of Heresy",
                true,
                "Icon for; Being in Strides of Heresy. Duration not visible by default"
            );
            cfgBuff_SprintArmor = ConfigFile_Server.Bind(
                "Buffs",
                "Rose Buckler",
                true,
                "Icon for; Rose Buckler being active. Weeping Fungus has a buff, why not this."
            );

            cfgBuff_BugFlight = ConfigFile_Server.Bind(
                "Buffs",
                "Milky Chrysalis",
                true,
                "Duration not visible by default, but will tell you when you start descending."
            );
            /*
            cfgBuff_HelfireDuration = ConfigFileUNSORTED.Bind(
                "Buffs",
                "Helfire Tincture",
                true,
                "Use BetterUI Buff Timers"
            );

            cfgBuff_Egg = ConfigFileUNSORTED.Bind(
                "Buffs",
                "Volcanic Egg",
                true,
                "Use BetterUI Buff Timers"
            );*/
        }


    }

}
