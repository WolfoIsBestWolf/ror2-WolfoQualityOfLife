using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using WQoL_Gameplay;

namespace WQoL_Gameplay
{
    public class Eclipse
    {

        public static void Start()
        {

            On.RoR2.EclipseRun.OverrideRuleChoices += AllowCertainEclipseArtifacts;
            IL.RoR2.EclipseRun.OverrideRuleChoices += EclipseRun_DontRemoveArtifacts;

        }

        public static void EclipseRun_DontRemoveArtifacts(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchCall("RoR2.ArtifactCatalog", "get_artifactCount")))
            {
                c.EmitDelegate<Func<int, int>>((damageCoeff) =>
                {
                    if (WConfig.EclipseAllowArtifacts.Value == WConfig.EclipseArtifact.Blacklist)
                    {
                        return 0;
                    }
                    return damageCoeff;
                });
            }
            else
            {
                Debug.LogWarning("IL FAILED : EclipseRun_DontRemoveArtifacts");
            }
        }

        private static DifficultyIndex EclipseRun_GetEclipseDifficultyIndex(On.RoR2.EclipseRun.orig_GetEclipseDifficultyIndex orig, int eclipseLevel)
        {
            WGQoLMain.log.LogMessage("EclipseLevel" + orig(eclipseLevel));
            return orig(eclipseLevel);
        }

        private static void AllowCertainEclipseArtifacts(On.RoR2.EclipseRun.orig_OverrideRuleChoices orig, EclipseRun self, RuleChoiceMask mustInclude, RuleChoiceMask mustExclude, ulong runSeed)
        {
            WGQoLMain.log.LogMessage("EclipseRuleoverride");
            orig(self, mustInclude, mustExclude, runSeed);

            if (WConfig.EclipseAllowArtifacts.Value == WConfig.EclipseArtifact.VanillaWhitelist)
            {
                ReverseForceChoice(RoR2Content.Artifacts.EliteOnly, mustInclude, mustExclude);
                ReverseForceChoice(RoR2Content.Artifacts.TeamDeath, mustInclude, mustExclude);
                ReverseForceChoice(RoR2Content.Artifacts.MixEnemy, mustInclude, mustExclude);
                ReverseForceChoice(RoR2Content.Artifacts.MonsterTeamGainsItems, mustInclude, mustExclude);
                ReverseForceChoice(RoR2Content.Artifacts.ShadowClone, mustInclude, mustExclude);
                ReverseForceChoice(RoR2Content.Artifacts.Bomb, mustInclude, mustExclude);
                ReverseForceChoice(DLC3Content.Artifacts.Prestige, mustInclude, mustExclude);
            }
            if (WConfig.EclipseAllowArtifacts.Value == WConfig.EclipseArtifact.Blacklist)
            {
                ForceChoiceOff(RoR2Content.Artifacts.Command, mustInclude, mustExclude);
                ForceChoiceOff(RoR2Content.Artifacts.RandomSurvivorOnRespawn, mustInclude, mustExclude);
                ForceChoiceOff(RoR2Content.Artifacts.Glass, mustInclude, mustExclude);
                ForceChoiceOff(CU8Content.Artifacts.Delusion, mustInclude, mustExclude);
                ForceChoiceOff(CU8Content.Artifacts.Devotion, mustInclude, mustExclude); //Makes game hella easier fo sure
                ForceChoiceOff(DLC2Content.Artifacts.Rebirth, mustInclude, mustExclude);
                ForceChoiceOff(ArtifactCatalog.FindArtifactDef("CLASSICITEMSRETURNS_ARTIFACT_CLOVER"), mustInclude, mustExclude);
                ForceChoiceOff(ArtifactCatalog.FindArtifactDef("ARTIFACT_SPOILS"), mustInclude, mustExclude);
                ForceChoiceOff(ArtifactCatalog.FindArtifactDef("ARTIFACT_ZETDROPIFACT"), mustInclude, mustExclude);
                ForceChoiceOff(ArtifactCatalog.FindArtifactDef("ZZT_CommandTrueCommand"), mustInclude, mustExclude);

            }

        }


        public static void ReverseForceChoice(ArtifactDef artifact, RuleChoiceMask mustInclude, RuleChoiceMask mustExclude)
        {
            if (artifact)
            {
                RuleDef ruleDef = RuleCatalog.FindRuleDef("Artifacts." + artifact.cachedName);
                if (ruleDef != null)
                {
                    foreach (RuleChoiceDef ruleChoiceDef in ruleDef.choices)
                    {
                        mustInclude[ruleChoiceDef.globalIndex] = false;
                        mustExclude[ruleChoiceDef.globalIndex] = false;
                    }
                }
            }
        }
        public static void ForceChoiceOff(ArtifactDef artifact, RuleChoiceMask mustInclude, RuleChoiceMask mustExclude)
        {
            if (artifact)
            {
                RuleDef ruleDef = RuleCatalog.FindRuleDef("Artifacts." + artifact.cachedName);
                RuleChoiceDef choiceDef = ruleDef.FindChoice("Off");
                if (choiceDef != null)
                {
                    foreach (RuleChoiceDef ruleChoiceDef in choiceDef.ruleDef.choices)
                    {
                        mustInclude[ruleChoiceDef.globalIndex] = false;
                        mustExclude[ruleChoiceDef.globalIndex] = true;
                    }
                    mustInclude[choiceDef.globalIndex] = true;
                    mustExclude[choiceDef.globalIndex] = false;
                }
            }
        }


    }

}
