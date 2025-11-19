using RoR2;
using RoR2.Stats;
using RoR2.UI.LogBook;
using System.Linq;
using TMPro;

namespace WolfoQoL_Client
{

    public class Logbook_MoreStats
    {

        public static void MoreStats()
        {
            Logbook_AddRecipes.Add();
            On.RoR2.UI.LogBook.PageBuilder.AddBodyStatsPanel += PageBuilder_AddBodyStatsPanel;
            On.RoR2.UI.LogBook.PageBuilder.AddMonsterPanel += ExtraMonsterStats;
            On.RoR2.UI.LogBook.PageBuilder.AddSurvivorPanel += PageBuilder_AddSurvivorPanel;

            On.RoR2.UI.LogBook.PageBuilder.SurvivorBody += PageBuilder_SurvivorBody;
            On.RoR2.UI.LogBook.PageBuilder.AddStagePanel += PageBuilder_AddStagePanel1;

        }

        private static void PageBuilder_AddStagePanel1(On.RoR2.UI.LogBook.PageBuilder.orig_AddStagePanel orig, PageBuilder self, SceneDef sceneDef)
        {
            string sub = Language.GetString(sceneDef.subtitleToken);
            if (sub.Length != 0)
            {
                //D5EBF2 color used for actual subtitles
                self.AddSimpleTextPanel("   <color=#D5EBF2><sprite name=\"CloudLeft\" tint=1> " + sub + " <sprite name=\"CloudRight\" tint=1></color>");
            }
            orig(self, sceneDef);


        }

        private static void PageBuilder_SurvivorBody(On.RoR2.UI.LogBook.PageBuilder.orig_SurvivorBody orig, PageBuilder builder)
        {
            orig(builder);
            CharacterBody characterBody = (CharacterBody)builder.entry.extraData;
            SurvivorDef survivorDef = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.bodyIndexToSurvivorIndex[(int)characterBody.bodyIndex]);

            string outroTexts = string.Empty;
            bool show = false;
            if (!string.IsNullOrEmpty(survivorDef.outroFlavorToken))
            {
                show = true;
                outroTexts += Language.GetString(survivorDef.outroFlavorToken) + "\n";
            }
            if (!string.IsNullOrEmpty(survivorDef.mainEndingEscapeFailureFlavorToken))
            {
                show = true;
                outroTexts += Language.GetString(survivorDef.mainEndingEscapeFailureFlavorToken) + "\n";
            }
            if (show)
            {
                builder.AddSimpleTextPanel(outroTexts);
            }


        }

        private static void PageBuilder_AddStagePanel(On.RoR2.UI.LogBook.PageBuilder.orig_AddStagePanel orig, PageBuilder self, SceneDef sceneDef)
        {
            orig(self, sceneDef);
            var textMesh = self.managedObjects.Last().GetComponent<ChildLocator>().FindChild("MainLabel").GetComponent<TextMeshProUGUI>();
            //textMesh.text = textMesh.text.Replace("cEvent", "cSub");
        }

        private static void PageBuilder_AddSimplePickup(On.RoR2.UI.LogBook.PageBuilder.orig_AddSimplePickup orig, PageBuilder self, PickupIndex pickupIndex)
        {
            orig(self, pickupIndex);
            var textMesh = self.managedObjects[self.managedObjects.Count - 2].GetComponent<ChildLocator>().FindChild("MainLabel").GetComponent<TextMeshProUGUI>();
            //textMesh.text = textMesh.text.Replace("cEvent", "cSub");
        }

        public static string TimeHHMMSSDisplayValueFormatter(double time2)
        {
            //Time is seconds

            ulong time = (ulong)time2;
            ulong hours = time / 3600UL;
            ulong minutes = (time - (hours * 3600UL)) / 60UL;
            ulong seconds = time - hours * 3600UL - minutes * 60UL;

            // Debug.Log(time);
            //Debug.Log(hours);
            //Debug.Log(minutes);
            //Debug.Log(seconds);
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }

        private static void PageBuilder_AddSurvivorPanel(On.RoR2.UI.LogBook.PageBuilder.orig_AddSurvivorPanel orig, PageBuilder self, CharacterBody bodyPrefabComponent)
        {
            orig(self, bodyPrefabComponent);


            //+Wins
            //+PlayTime
            //Longest Run
            //Pick Rate
            //Pick Rate%
            //+DeathsAs
            //+DamageTakenAs
            //+DamageDealthAs

            string totalTimeAlive = TimeHHMMSSDisplayValueFormatter(self.statSheet.GetStatValueDouble(PerBodyStatDef.totalTimeAlive, bodyPrefabComponent.gameObject.name));

            var textMesh = self.managedObjects.Last().GetComponent<ChildLocator>().FindChild("MainLabel").GetComponent<TextMeshProUGUI>(); ;
            string newStats = string.Empty;
            string name = bodyPrefabComponent.gameObject.name;
            //
            newStats += StatMethod(PerBodyStatDef.totalWins, name, self.statSheet);
            newStats += Language.GetString("PERBODYSTATNAME_TOTALTIMEALIVE") + $": <style=cEvent>{totalTimeAlive}</style>\n";
            newStats += textMesh.text;
            //newStats += "\n";
            newStats += StatMethod(PerBodyStatDef.killsAs, name, self.statSheet);
            newStats += StatMethod(PerBodyStatDef.damageDealtAs, name, self.statSheet);
            newStats += StatMethod(PerBodyStatDef.damageTakenAs, name, self.statSheet);
            newStats += StatMethod(PerBodyStatDef.deathsAs, name, self.statSheet);
            textMesh.text = newStats;

        }
        public static string StatMethod(PerBodyStatDef statDef, string bodyName, StatSheet self)
        {
            return Language.GetString(statDef.nameToken) + $": <style=cEvent>{TextSerialization.ToStringNumeric(self.GetStatValueULong(statDef, bodyName))}</style>\n";
        }

        private static void ExtraMonsterStats(On.RoR2.UI.LogBook.PageBuilder.orig_AddMonsterPanel orig, PageBuilder self, CharacterBody bodyPrefabComponent)
        {
            orig(self, bodyPrefabComponent);

            //+Damage To
            //Kills
            //EliteKills
            //Deaths
            //+DamageFrom

            string name = bodyPrefabComponent.gameObject.name;
            var textMesh = self.managedObjects.Last().GetComponent<ChildLocator>().FindChild("MainLabel").GetComponent<TextMeshProUGUI>();
            string newStats = string.Empty;
            newStats += StatMethod(PerBodyStatDef.damageDealtTo, name, self.statSheet);
            newStats += textMesh.text;
            newStats += "\n";
            newStats += StatMethod(PerBodyStatDef.damageTakenFrom, name, self.statSheet);
            textMesh.text = newStats;
        }

        private static void PageBuilder_AddBodyStatsPanel(On.RoR2.UI.LogBook.PageBuilder.orig_AddBodyStatsPanel orig, PageBuilder self, CharacterBody bodyPrefabComponent)
        {
            if (self.managedObjects.Count == 0)
            {
                //E9EFF5 actual color used
                //FFA6A6
                if (bodyPrefabComponent.subtitleNameToken.Length != 0)
                {
                    self.AddSimpleTextPanel("   <color=#D5EBF2><sprite name=\"CloudLeft\" tint=1> " + Language.GetString(bodyPrefabComponent.subtitleNameToken) + " <sprite name=\"CloudRight\" tint=1></style>");
                }
            }

            orig(self, bodyPrefabComponent);
            //+AttackSpeed?
        }
    }

}