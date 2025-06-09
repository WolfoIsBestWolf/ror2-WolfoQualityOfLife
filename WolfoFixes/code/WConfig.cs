using BepInEx;
using BepInEx.Configuration;

namespace WolfoFixes
{
    public class WConfig
    {

        public static ConfigFile ConfigFile_Client = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoFixes.cfg", true);

        public static ConfigEntry<bool> cfgTextItems;
        public static ConfigEntry<bool> cfgTextCharacters;

        public static void Start()
        {

            InitConfig();

        }


        public static void InitConfig()
        {

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


        }



    }

}
