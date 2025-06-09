//using System;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    public class Simualcrum
    {
        public static void Start()
        {
            IL.RoR2.InfiniteTowerWaveController.FixedUpdate += FixRequestIndicatorsClient;
            On.EntityStates.InfiniteTowerSafeWard.AwaitingActivation.OnEnter += Waiting_SetRadius;
            FixVoidSuppresor();
            On.RoR2.InfiniteTowerRun.OverrideRuleChoices += ForceSotVOn;
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

        public static void FixVoidSuppresor()
        {

            //Addressables.LoadAssetAsync<SpawnCard>(key: "RoR2/DLC1/VoidSuppressor/iscVoidSuppressor.asset").WaitForCompletion().directorCreditCost = 4;
            //Since we got Void Soupper in Dissim we gotta fix the vanilla up
            GameObject VoidSuppressor = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSuppressor/VoidSuppressor.prefab").WaitForCompletion();

            /*DitherModel dither = VoidSuppressor.AddComponent<DitherModel>();
            dither.renderers = VoidSuppressor.GetComponentsInChildren<Renderer>();*/

            VoidSuppressorBehavior suppressor = VoidSuppressor.GetComponent<VoidSuppressorBehavior>();
            //suppressor.itemRefreshDelay = 0.5f;
            suppressor.useRefreshDelay = 1.05f; //Cannot go above this
            suppressor.costMultiplierPerPurchase = 1;

            VoidSuppressor.GetComponent<PurchaseInteraction>().isShrine = true;
            VoidSuppressor.GetComponent<VoidSuppressorBehavior>().effectColor.a = 0.85f;
            Transform mdlVoidSuppressor = VoidSuppressor.transform.GetChild(0);
            mdlVoidSuppressor.GetChild(7).GetChild(1).GetChild(1).gameObject.SetActive(true);
            mdlVoidSuppressor.GetChild(7).localPosition = new Vector3(0, -0.5f, -0.5f);
            mdlVoidSuppressor.GetChild(7).GetChild(1).GetChild(0).localScale = new Vector3(1.2f, 1.2f, 1.2f);
            Renderer temp = mdlVoidSuppressor.GetChild(5).GetComponent<Renderer>();
            temp.localBounds = new Bounds
            {
                extents = new Vector3(5, 5, 5),
            };



           
            MiscContent.ScrapWhiteSuppressed.pickupToken = "ITEM_SCRAPWHITE_PICKUP";
            MiscContent.ScrapGreenSuppressed.pickupToken = "ITEM_SCRAPGREEN_PICKUP";
            MiscContent.ScrapRedSuppressed.pickupToken = "ITEM_SCRAPRED_PICKUP";

            MiscContent.ScrapWhiteSuppressed.descriptionToken = "ITEM_SCRAPWHITE_DESC";
            MiscContent.ScrapGreenSuppressed.descriptionToken = "ITEM_SCRAPGREEN_DESC";
            MiscContent.ScrapRedSuppressed.descriptionToken = "ITEM_SCRAPRED_DESC";

            MiscContent.ScrapWhiteSuppressed.deprecatedTier = ItemTier.Tier1;
            MiscContent.ScrapGreenSuppressed.deprecatedTier = ItemTier.Tier2;
            MiscContent.ScrapRedSuppressed.deprecatedTier = ItemTier.Tier3;
            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += DontIncludeInLog;


            GameObject Hud = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/HUDSimple.prefab").WaitForCompletion();
            try
            {
                Transform SuprresedItems = Hud.transform.GetChild(0).GetChild(8).GetChild(2).GetChild(8).GetChild(0).GetChild(3);
                SuprresedItems.GetComponent<UnityEngine.UI.LayoutElement>().minHeight = 96;
                SuprresedItems.GetComponent<RoR2.UI.ItemInventoryDisplay>().verticalMargin = 16;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            LanguageAPI.Add("VOID_SUPPRESSOR_DESCRIPTION", "Eradicate the shown item from existence, preventing it from appearing again for the current run. Existing copies of the item will be scrapped instantly.");
            InspectDef inspectDef = ScriptableObject.CreateInstance<InspectDef>();
            inspectDef.name = "VoidSuppressorInspectDef";
            var Inspect = new RoR2.UI.InspectInfo();
            Inspect.TitleToken = "VOID_SUPPRESSOR_NAME";
            Inspect.DescriptionToken = "VOID_SUPPRESSOR_DESCRIPTION";
            Inspect.FlavorToken = "VOID_SUPPRESSOR_DESCRIPTION";
            Inspect.Visual = Addressables.LoadAssetAsync<InspectDef>(key: "RoR2/DLC1/VoidChest/VoidChestInspectDef.asset").WaitForCompletion().Info.Visual;
            inspectDef.Info = Inspect;
            var InspectInfo = VoidSuppressor.AddComponent<GenericInspectInfoProvider>();
            InspectInfo.InspectInfo = inspectDef;

            On.RoR2.VoidSuppressorBehavior.PreStartClient += VoidSuppressorBehavior_PreStartClient;
            On.RoR2.VoidSuppressorBehavior.RefreshPickupDisplays += VoidSuppressorBehavior_RefreshPickupDisplays;

            On.RoR2.GameCompletionStatsHelper.ctor += GameCompletionStatsHelper_ctor;
        }

        private static void GameCompletionStatsHelper_ctor(On.RoR2.GameCompletionStatsHelper.orig_ctor orig, GameCompletionStatsHelper self)
        {
            orig(self);
            PickupDef pickupDef1 = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(MiscContent.ScrapWhiteSuppressed.itemIndex));
            PickupDef pickupDef2 = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(MiscContent.ScrapGreenSuppressed.itemIndex));
            PickupDef pickupDef3 = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(MiscContent.ScrapRedSuppressed.itemIndex));
            self.encounterablePickups.Remove(pickupDef1);
            self.encounterablePickups.Remove(pickupDef2);
            self.encounterablePickups.Remove(pickupDef3);
        }

        private static RoR2.UI.LogBook.Entry[] DontIncludeInLog(On.RoR2.UI.LogBook.LogBookController.orig_BuildPickupEntries orig, System.Collections.Generic.Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            MiscContent.ScrapWhiteSuppressed.deprecatedTier = ItemTier.NoTier;
            MiscContent.ScrapGreenSuppressed.deprecatedTier = ItemTier.NoTier;
            MiscContent.ScrapRedSuppressed.deprecatedTier = ItemTier.NoTier;
            var temp = orig(expansionAvailability);
            MiscContent.ScrapWhiteSuppressed.deprecatedTier = ItemTier.Tier1;
            MiscContent.ScrapGreenSuppressed.deprecatedTier = ItemTier.Tier2;
            MiscContent.ScrapRedSuppressed.deprecatedTier = ItemTier.Tier3;
            return temp;
        }

        private static void VoidSuppressorBehavior_RefreshPickupDisplays(On.RoR2.VoidSuppressorBehavior.orig_RefreshPickupDisplays orig, VoidSuppressorBehavior self)
        { 
            //Cirvumcent PickupDisplay using lossyScale
            //Which permamently makes some displays smaller
            //Randomly depending on when in the animation its set
            Transform pickup = self.pickupDisplays[0].transform;
            float size = 1.2f * 0.7469f / pickup.parent.localScale.x;
            pickup.localScale = new Vector3(size, size, size);
            orig(self);
        }

        private static void VoidSuppressorBehavior_PreStartClient(On.RoR2.VoidSuppressorBehavior.orig_PreStartClient orig, VoidSuppressorBehavior self)
        {
            orig(self);

            GameObject LockboxHoloPivot = new GameObject("PriceTransform");
            LockboxHoloPivot.transform.SetParent(self.transform, false);
            LockboxHoloPivot.transform.localPosition = new Vector3(0.2f, 1f, 2f);
            RoR2.Hologram.HologramProjector hologram = self.GetComponent<RoR2.Hologram.HologramProjector>();
            if (hologram == null)
            {
                hologram = self.gameObject.AddComponent<RoR2.Hologram.HologramProjector>();
            }
            hologram.disableHologramRotation = true;
            LockboxHoloPivot.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
            hologram.hologramPivot = LockboxHoloPivot.transform;
  
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
