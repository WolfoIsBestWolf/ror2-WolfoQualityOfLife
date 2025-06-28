using RoR2;
using UnityEngine;

namespace WolfoQoL_Client
{
    public class Eclipse
    {

        public static void Start()
        {
            DifficultyColors();

            //On.RoR2.EclipseRun.OverrideRuleChoices += EclipseRun_OverrideRuleChoices;
            //On.RoR2.EclipseRun.GetEclipseDifficultyIndex += EclipseRun_GetEclipseDifficultyIndex;

        }


        private static DifficultyIndex EclipseRun_GetEclipseDifficultyIndex(On.RoR2.EclipseRun.orig_GetEclipseDifficultyIndex orig, int eclipseLevel)
        {
            Debug.Log("EclipseLevel" + orig(eclipseLevel));
            return orig(eclipseLevel);
        }

        private static void EclipseRun_OverrideRuleChoices(On.RoR2.EclipseRun.orig_OverrideRuleChoices orig, EclipseRun self, RuleChoiceMask mustInclude, RuleChoiceMask mustExclude, ulong runSeed)
        {
            Debug.Log("EclipseRuleoverride");
            orig(self, mustInclude, mustExclude, runSeed);



            for (int i = 1; i > 17; i++)
            {
                // base.ForceChoice(mustInclude, mustExclude, string.Format("Difficulty.{0}", EclipseRun.GetEclipseDifficultyIndex(num).ToString()));
                RuleChoiceDef eclipse = RuleCatalog.FindChoiceDef(string.Format("Difficulty.Eclipse{0}", i));
                if (mustInclude[eclipse.globalIndex] == true)
                {
                    PreGameController.instance.ruleBookBuffer.ApplyChoice(eclipse);
                    break;
                }
                mustInclude[eclipse.globalIndex] = true;
                mustExclude[eclipse.globalIndex] = false;




                /*foreach (RuleChoiceDef ruleChoiceDef in rule.ruleDef.choices)
                {
                    mustInclude[ruleChoiceDef.globalIndex] = true;
                    mustExclude[ruleChoiceDef.globalIndex] = true;
                }
                mustInclude[choiceDef.globalIndex] = true;
                mustExclude[choiceDef.globalIndex] = false;*/
            }


        }


        private static void DifficultyColors()
        {
            Color eclipseColor = new Color(0.2f, 0.22f, 0.4f, 1);

            DifficultyCatalog.difficultyDefs[3].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[4].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[5].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[6].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[7].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[8].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[9].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[10].color = eclipseColor;

            DifficultyCatalog.difficultyDefs[3].serverTag = "e1";
            DifficultyCatalog.difficultyDefs[4].serverTag = "e2";
            DifficultyCatalog.difficultyDefs[5].serverTag = "e3";
            DifficultyCatalog.difficultyDefs[6].serverTag = "e4";
            DifficultyCatalog.difficultyDefs[7].serverTag = "e5";
            DifficultyCatalog.difficultyDefs[8].serverTag = "e6";
            DifficultyCatalog.difficultyDefs[9].serverTag = "e7";
            DifficultyCatalog.difficultyDefs[10].serverTag = "e8";
        }

    }

}
