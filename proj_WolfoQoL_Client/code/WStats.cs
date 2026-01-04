using BepInEx;
using BepInEx.Configuration;
using RoR2;

namespace WolfoQoL_Client
{
    public static class WStats
    {

        public static ConfigFile StatsFile = new ConfigFile(Paths.ConfigPath + "\\Wolfo.WolfoQoL_StatStorage.cfg", true);

        public static void MakeStats()
        {
            for (int i = 0; i < BodyCatalog.bodyPrefabs.Length; i++)
            {
                StatsFile.Bind(
                   "MinionDamage",
                   BodyCatalog.bodyPrefabs[i].name,
                   (double)0,
                   ""
               );
            }
        }

        public static void AddStat(int index, float newDamage)
        {
            if (StatsFile.TryGetEntry<double>(new ConfigDefinition("MinionDamage", BodyCatalog.bodyPrefabs[index].name), out var stats))
            {
                stats.Value += newDamage;
            }

        }

        public static double GetStat(int index)
        {
            if (StatsFile.TryGetEntry<double>(new ConfigDefinition("MinionDamage", BodyCatalog.bodyPrefabs[index].name), out var stats))
            {
                return stats.Value;
            }
            return 0;
        }



    }

}
