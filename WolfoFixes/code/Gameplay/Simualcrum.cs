//using System;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    internal class Simualcrum
    {
        public static void Start()
        {
            IL.RoR2.InfiniteTowerWaveController.FixedUpdate += FixRequestIndicatorsClient;
            On.EntityStates.InfiniteTowerSafeWard.AwaitingActivation.OnEnter += Waiting_SetRadius;
            On.RoR2.InfiniteTowerRun.OverrideRuleChoices += ForceSotVOn;

            //Simulacrums Fog was not update to account for the the newly introduced FogDamageController.healthFractionRampIncreaseCooldown
            //This left it dealing pitiful amounts of damage
            FogDamageController InfiniteTowerFogDamager = Addressables.LoadAssetAsync<GameObject>(key: "9c7ca1b454882464f90010d3a68b6795").WaitForCompletion().GetComponent<FogDamageController>();
            InfiniteTowerFogDamager.healthFractionRampIncreaseCooldown = 0;

        }


        public static void CallLate()
        {
            bool simulacrumAdds = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Wolfo.SimulacrumAdditions");

            if (simulacrumAdds == false)
            {
                //Simulacrum normally has no Shrines of Chance
                InfiniteTowerRun infiniteTowerRun = Addressables.LoadAssetAsync<GameObject>(key: "ba84d17b27db8b84d925071b4af1e352").WaitForCompletion().GetComponent<InfiniteTowerRun>();
                HG.ArrayUtils.ArrayAppend(ref infiniteTowerRun.blacklistedItems, DLC2Content.Items.ExtraShrineItem);

            }

        }

        private static void ForceSotVOn(On.RoR2.InfiniteTowerRun.orig_OverrideRuleChoices orig, InfiniteTowerRun self, RuleChoiceMask mustInclude, RuleChoiceMask mustExclude, ulong runSeed)
        {
            orig(self, mustInclude, mustExclude, runSeed);
            RuleDef ruleDef = RuleCatalog.FindRuleDef("Expansions.DLC1");
            RuleChoiceDef ruleChoiceDef = (ruleDef != null) ? ruleDef.FindChoice("On") : null;
            if (ruleChoiceDef != null)
            {
                self.ForceChoice(mustInclude, mustExclude, ruleChoiceDef);
            }
        }



        public static void Waiting_SetRadius(On.EntityStates.InfiniteTowerSafeWard.AwaitingActivation.orig_OnEnter orig, EntityStates.InfiniteTowerSafeWard.AwaitingActivation self)
        {
            orig(self);

            //Client fix??
            InfiniteTowerRun run = Run.instance.GetComponent<InfiniteTowerRun>();
            if (!run.safeWardController)
            {
                run.safeWardController = self.gameObject.GetComponent<InfiniteTowerSafeWardController>();
            }
        }


        public static void FixRequestIndicatorsClient(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
             x => x.MatchCallvirt("RoR2.CombatSquad", "get_readOnlyMembersList"));

            if (c.TryGotoPrev(MoveType.Before,
             x => x.MatchLdfld("RoR2.InfiniteTowerWaveController", "combatSquad")
            ))
            {
                c.EmitDelegate<System.Func<InfiniteTowerWaveController, InfiniteTowerWaveController>>((wave) =>
                {
                    if (wave.combatSquad.readOnlyMembersList.Count == 0)
                    {
                        Debug.Log("Couln't do indicators the normal way");
                        for (int i = 0; wave.combatSquad.membersList.Count > i; i++)
                        {
                            wave.RequestIndicatorForMaster(wave.combatSquad.membersList[i]);
                        }
                    }
                    return wave;
                });
                Debug.Log("IL Found : IL.RoR2.InfiniteTowerWaveController.FixedUpdate");
            }
            else
            {
                Debug.LogWarning("IL Failed : IL.RoR2.InfiniteTowerWaveController.FixedUpdate");
            }
        }

    }


}
