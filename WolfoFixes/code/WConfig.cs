using BepInEx;
using BepInEx.Configuration;

namespace WolfoFixes
{
    public class WConfig
    {

        public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoFixes.cfg", true);

        public static ConfigEntry<bool> cfgTextItems;
        public static ConfigEntry<bool> cfgTextCharacters;
        public static ConfigEntry<bool> cfgMithrix4Skip;
        public static ConfigEntry<bool> cfgItemTags;

        public static void Start()
        {

            InitConfig();

        }


        public static void InitConfig()
        {

            cfgTextItems = ConfigFile_Client.Bind(
                "Gameplay",
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
               "Mithrix Phase 4 Skip",
               true,
               "This is without question a bug. Some people may still like it despite this."
           );
            cfgItemTags = ConfigFile_Client.Bind(
                "Gameplay",
                "Item Tag Changes",
                true,
                "Adds AIBlacklist to more items that are uselss or overpowered on enemies."
            );


        }



    }

}
