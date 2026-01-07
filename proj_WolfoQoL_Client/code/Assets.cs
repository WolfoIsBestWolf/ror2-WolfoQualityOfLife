using BepInEx;
using System.IO;
using System.Linq;
using UnityEngine;

namespace WolfoQoL_Client
{
    //Taken from EnFucker, Thank you EnFucker
    public static class Assets
    {
        public static AssetBundle Bundle;
        public static PluginInfo PluginInfo;
        public static string Folder = "QualityOfLife\\";

        public static Sprite Load(string s)
        {
            return Bundle.LoadAsset<Sprite>($"Assets/WQoL/PingIcons/{s}.png");
        }

        internal static string assemblyDir
        {
            get
            {
                return System.IO.Path.GetDirectoryName(PluginInfo.Location);
            }
        }

        internal static void Init(PluginInfo info)
        {
            PluginInfo = info;

            //Check if manual install
            if (!Directory.Exists(GetPathToFile(Folder)))
            {
                Folder = "plugins\\" + Folder;
            }
            if (Directory.Exists(GetPathToFile(Folder + "AssetBundles")))
            {
                Bundle = AssetBundle.LoadFromFile(GetPathToFile(Folder + "AssetBundles", "wolfoqualityoflife"));
            }
            else
            {
                Debug.LogError("COULD NOT FIND ASSETBUNDLES FOLDER");
            }

            if (WConfig.cfgTestDisableMod.Value)
            {
                return;
            }
            if (Directory.Exists(GetPathToFile(Folder + "Languages")))
            {
                On.RoR2.Language.SetFolders += SetFolders;
            }
            else
            {
                Debug.LogError("COULD NOT FIND LANGUAGES FOLDER");
            }
        }

        private static void SetFolders(On.RoR2.Language.orig_SetFolders orig, RoR2.Language self, System.Collections.Generic.IEnumerable<string> newFolders)
        {
            var dirs = System.IO.Directory.EnumerateDirectories(Path.Combine(GetPathToFile(Folder + "Languages")), self.name);
            orig(self, newFolders.Union(dirs));
        }


        internal static string GetPathToFile(string folderName)
        {
            return Path.Combine(assemblyDir, folderName);
        }
        internal static string GetPathToFile(string folderName, string fileName)
        {
            return Path.Combine(GetPathToFile(folderName), fileName);
        }
    }
}