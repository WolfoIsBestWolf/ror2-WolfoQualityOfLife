using RoR2;
namespace WolfoQoL_Client
{
    public class PrismaticTrial
    {
        public static void AllowPrismaticTrials()
        {
            On.RoR2.UI.WeeklyRunScreenController.OnEnable += (orig, self) =>
            {
                UnityEngine.Object.Destroy(self.leaderboard.gameObject.transform.parent.parent.gameObject);
                self.enabled = false;
            };
            On.RoR2.WeeklyRun.GetCurrentSeedCycle += (orig) =>
            {
                return (uint)(WeeklyRun.now.Ticks*WolfoMain.random.NextDouble());
            };
            On.RoR2.WeeklyRun.ClientSubmitLeaderboardScore += (orig, self, runReport) =>
            {
                return;
            };
            On.RoR2.DisableIfGameModded.OnEnable += (orig, self) =>
            {
                return;
            };
            On.RoR2.WeeklyRun.OverrideRuleChoices += WeeklyRun_OverrideRuleChoices;
        }

        private static void WeeklyRun_OverrideRuleChoices(On.RoR2.WeeklyRun.orig_OverrideRuleChoices orig, RoR2.WeeklyRun self, RoR2.RuleChoiceMask mustInclude, RoR2.RuleChoiceMask mustExclude, ulong runSeed)
        {
            orig(self, mustInclude, mustExclude, runSeed);
        }
    }
}
