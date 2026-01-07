using HG;
using ProperSave;
using RoR2;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.ModSupport
{
    public static class AddProperSaveSupport
    {
        //Taken / Learnt from CommandQueue+ProperSave mod

        //public static readonly string ThePathR = System.IO.Path.Combine(Application.persistentDataPath, "ProperSave", "Saves") + "\\" + "WQoL_R" + ".csv";
        //public static readonly string ThePathP = System.IO.Path.Combine(Application.persistentDataPath, "ProperSave", "Saves") + "\\" + "WQoL_P" + ".csv";

        public static string ReturnPath(string profile, string type)
        {
            return System.IO.Path.Combine(Application.persistentDataPath, "ProperSave", "Saves") + "\\" + "WQoL_" + type + profile + ".csv";
        }
        public static void Start()
        {
            //Make this like, not work with uhh when config disabled

            ProperSave.SaveFile.OnGatherSaveData += SaveFile_OnGatherSaveData;
            ProperSave.Loading.OnLoadingEnded += Loading_OnLoadingEnded;
        }
        private static void SaveFile_OnGatherSaveData(System.Collections.Generic.Dictionary<string, object> obj)
        {
            string a = ReturnPath(LocalUserManager.GetFirstLocalUser().userProfile.fileName, "R_");
            string b = ReturnPath(LocalUserManager.GetFirstLocalUser().userProfile.fileName, "P_");

            RunTrackerData.Save(a);
            PlayerTrackerData.Save(b);

            if (WolfoLibrary.WConfig.cfgTestMultiplayer.Value)
            {
                Debug.Log(a);
            }

        }

        private static void Loading_OnLoadingEnded(SaveFile _)
        {
            string a = ReturnPath(LocalUserManager.GetFirstLocalUser().userProfile.fileName, "R_");
            string b = ReturnPath(LocalUserManager.GetFirstLocalUser().userProfile.fileName, "P_");

            if (File.Exists(a) && File.Exists(b))
            {
                RunTrackerData.Load(a);
                PlayerTrackerData.Load(b);
                Log.LogMessage("Loading WQoL save data");
            }
            else
            {
                Log.LogMessage("No WQOL Proper Save Data for this profile.");
            }
        }
    }

    public static class RunTrackerData
    {
        public static void Load(string path)
        {
            var stats = Run.instance.gameObject.EnsureComponent<RunExtraStatTracker>();
            stats.visitedScenes = new List<SceneDef>();

            string[] wholeFile = File.ReadAllLines(path);
            string[] splitStages = wholeFile[0].Split(',');
            for (int j = 0; j < splitStages.Length; j++)
            {
                Debug.Log(splitStages[j]);
                stats.visitedScenes.Add(SceneCatalog.GetSceneDef((SceneIndex)int.Parse(splitStages[j])));
            }

            stats.missedChests = int.Parse(wholeFile[1]);
            stats.missedDrones = int.Parse(wholeFile[2]);
            stats.missedShrineChanceItems = int.Parse(wholeFile[3]);
            stats.missedLemurians = int.Parse(wholeFile[4]);

        }

        public static void Save(string path)
        {
            if (!RunExtraStatTracker.instance)
            {
                File.Delete(path);
                return;
            }
            var Run = RunExtraStatTracker.instance;

            string textToWrite = string.Empty;
            //-1 because stage is added both on save and load
            int ii = Run.visitedScenes.Count - 1;
            for (int i = 0; i < ii; i++)
            {
                textToWrite += (int)Run.visitedScenes[i].sceneDefIndex;
                if (i < ii - 1)
                {
                    textToWrite += ",";
                }
            }

            textToWrite += "\n";
            textToWrite += Run.missedChests + "\n";
            textToWrite += Run.missedDrones + "\n";
            textToWrite += Run.missedShrineChanceItems + "\n";
            textToWrite += Run.missedLemurians + "\n";

            File.WriteAllText(path, textToWrite);
        }
    }
    public static class PlayerTrackerData
    {
        public static void Load(string path)
        {
            string[] allPlayers = File.ReadAllLines(path);
            foreach (var playerString in allPlayers)
            {
                string[] splitData = playerString.Split("###");

                ulong netId = ulong.Parse(splitData[0]);
                //Debug.Log(netId); 
                PlayerCharacterMasterController player = null;
                foreach (var find in PlayerCharacterMasterController.instances)
                {
                    //Debug.Log(find);
                    if (find.networkUser && find.networkUser.id.steamId.ID == netId)
                    {
                        //Debug.Log(find.networkUser.id.steamId.ID);
                        player = find;
                        break;
                    }
                }
                if (!player)
                {
                    continue;
                }
                var stats = player.EnsureComponent<PerPlayer_ExtraStatTracker>();

                //Debug.Log("FOUND PLAYER");
                //Debug.Log(playerString);

                stats.scrappedItems = int.Parse(splitData[1]);
                stats.scrappedDrones = int.Parse(splitData[2]);
                stats.minionDamageTaken = float.Parse(splitData[3]);
                stats.minionHealing = float.Parse(splitData[4]);
                stats.minionDeaths = int.Parse(splitData[5]);
                stats.spentLunarCoins = int.Parse(splitData[6]);
                stats.timesJumped = int.Parse(splitData[7]);
                stats.lemuriansHatched = int.Parse(splitData[8]);
                stats.dotDamageDone = float.Parse(splitData[9]);
                stats.damageBlocked = float.Parse(splitData[10]);

                string preSplitMinionDamage = splitData[11];
                string[] minionDamages = preSplitMinionDamage.Split(',');
                for (int i = 0; i < minionDamages.Length; i++)
                {
                    stats.perMinionDamage[i] = float.Parse(minionDamages[i]);
                }
                stats.timeAliveReal = float.Parse(splitData[12]);

            }

        }

        public static void Save(string path)
        {
            string sperator = "###";
            string textToWrite = string.Empty;
            foreach (var player in PlayerCharacterMasterController.instances)
            {
                if (!player.networkUser)
                {
                    continue;
                }
                var stats = player.GetComponent<PerPlayer_ExtraStatTracker>();
                if (!stats)
                {
                    File.Delete(path);
                    return;
                }
                string PlayerData = string.Empty;
                PlayerData += player.networkUser.id.steamId.ID + sperator;     //0
                PlayerData += stats.scrappedItems + sperator;       //1
                PlayerData += stats.scrappedDrones + sperator;      //2
                PlayerData += stats.minionDamageTaken + sperator;   //3
                PlayerData += stats.minionHealing + sperator;       //4
                PlayerData += stats.minionDeaths + sperator;
                PlayerData += stats.spentLunarCoins + sperator;
                PlayerData += stats.timesJumped + sperator;
                PlayerData += stats.lemuriansHatched + sperator;
                PlayerData += stats.dotDamageDone + sperator;
                PlayerData += stats.damageBlocked + sperator;
                for (int i = 0; i < stats.perMinionDamage.Length; i++)
                {
                    PlayerData += stats.perMinionDamage[i];
                    if (i < stats.perMinionDamage.Length - 1)
                    {
                        PlayerData += ",";
                    }
                }
                PlayerData += sperator;
                PlayerData += stats.timeAliveReal + sperator;

                PlayerData += "\n";
                textToWrite += PlayerData;
            }

            //Debug.Log(textToWrite);
            File.WriteAllText(path, textToWrite);
        }
    }
}
