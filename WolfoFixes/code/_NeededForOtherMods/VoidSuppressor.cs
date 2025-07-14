//using System;
using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    internal class VoidSuppressor
    {
        public static void SuppresedScrap()
        {
            MissedContent.Items.ScrapWhiteSuppressed.pickupToken = "ITEM_SCRAPWHITE_PICKUP";
            MissedContent.Items.ScrapGreenSuppressed.pickupToken = "ITEM_SCRAPGREEN_PICKUP";
            MissedContent.Items.ScrapRedSuppressed.pickupToken = "ITEM_SCRAPRED_PICKUP";

            MissedContent.Items.ScrapWhiteSuppressed.descriptionToken = "ITEM_SCRAPWHITE_DESC";
            MissedContent.Items.ScrapGreenSuppressed.descriptionToken = "ITEM_SCRAPGREEN_DESC";
            MissedContent.Items.ScrapRedSuppressed.descriptionToken = "ITEM_SCRAPRED_DESC";

            On.RoR2.PickupCatalog.Init += MakeTiered;
            On.RoR2.PickupCatalog.SetEntries += MakeUntiered;
            Run.onRunStartGlobal += MakeTieredAgain;
            On.RoR2.GameCompletionStatsHelper.ctor += RemoveFromCompletion;
        }

        public static void FixInteractable()
        {
            GameObject VoidSuppressor = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSuppressor/VoidSuppressor.prefab").WaitForCompletion();

            /*DitherModel dither = VoidSuppressor.AddComponent<DitherModel>();
            dither.renderers = VoidSuppressor.GetComponentsInChildren<Renderer>();*/

            VoidSuppressorBehavior suppressor = VoidSuppressor.GetComponent<VoidSuppressorBehavior>();
            //suppressor.itemRefreshDelay = 0.5f;
            suppressor.useRefreshDelay = 1.05f; //Cannot go above this

            VoidSuppressor.GetComponent<PurchaseInteraction>().isShrine = true;
            VoidSuppressor.GetComponent<VoidSuppressorBehavior>().effectColor.a = 0.85f;
            //Better item position and effects
            Transform mdlVoidSuppressor = VoidSuppressor.transform.GetChild(0);
            mdlVoidSuppressor.GetChild(7).GetChild(1).GetChild(1).gameObject.SetActive(true);
            mdlVoidSuppressor.GetChild(7).localPosition = new Vector3(0, -0.5f, -0.5f);
            mdlVoidSuppressor.GetChild(7).GetChild(1).GetChild(0).localScale = new Vector3(1.2f, 1.2f, 1.2f);
            //Fix too close to camera
            Renderer temp = mdlVoidSuppressor.GetChild(5).GetComponent<Renderer>();
            temp.localBounds = new Bounds
            {
                extents = new Vector3(5, 5, 5),
            };


            //Hud being too small for item icons
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

            On.RoR2.VoidSuppressorBehavior.PreStartClient += AddPriceHologram;
            On.RoR2.VoidSuppressorBehavior.RefreshPickupDisplays += FixPickupDisplayTooSmallDueToLossyScale;

        }



        private static void MakeUntiered(On.RoR2.PickupCatalog.orig_SetEntries orig, PickupDef[] pickupDefs)
        {
            orig(pickupDefs);
            MissedContent.Items.ScrapWhiteSuppressed.tier = ItemTier.NoTier;
            MissedContent.Items.ScrapGreenSuppressed.tier = ItemTier.NoTier;
            MissedContent.Items.ScrapRedSuppressed.tier = ItemTier.NoTier;
        }

        private static void MakeTieredAgain(Run obj)
        {

            MissedContent.Items.ScrapWhiteSuppressed.tier = ItemTier.Tier1;
            MissedContent.Items.ScrapGreenSuppressed.tier = ItemTier.Tier2;
            MissedContent.Items.ScrapRedSuppressed.tier = ItemTier.Tier3;
            Run.onRunStartGlobal -= MakeTieredAgain;
        }

        private static System.Collections.IEnumerator MakeTiered(On.RoR2.PickupCatalog.orig_Init orig)
        {
            MissedContent.Items.ScrapWhiteSuppressed.tier = ItemTier.Tier1;
            MissedContent.Items.ScrapGreenSuppressed.tier = ItemTier.Tier2;
            MissedContent.Items.ScrapRedSuppressed.tier = ItemTier.Tier3;
            return orig();
        }

        private static void RemoveFromCompletion(On.RoR2.GameCompletionStatsHelper.orig_ctor orig, GameCompletionStatsHelper self)
        {
            orig(self);
            PickupDef pickupDef1 = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(MissedContent.Items.ScrapWhiteSuppressed.itemIndex));
            PickupDef pickupDef2 = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(MissedContent.Items.ScrapGreenSuppressed.itemIndex));
            PickupDef pickupDef3 = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(MissedContent.Items.ScrapRedSuppressed.itemIndex));
            self.encounterablePickups.Remove(pickupDef1);
            self.encounterablePickups.Remove(pickupDef2);
            self.encounterablePickups.Remove(pickupDef3);
        }



        private static void FixPickupDisplayTooSmallDueToLossyScale(On.RoR2.VoidSuppressorBehavior.orig_RefreshPickupDisplays orig, VoidSuppressorBehavior self)
        {
            //Cirvumcent PickupDisplay using lossyScale
            //Which permamently makes some displays smaller
            //Randomly depending on when in the animation its set
            Transform pickup = self.pickupDisplays[0].transform;
            float size = 1.2f * 0.7469f / pickup.parent.localScale.x;
            pickup.localScale = new Vector3(size, size, size);
            orig(self);
        }

        private static void AddPriceHologram(On.RoR2.VoidSuppressorBehavior.orig_PreStartClient orig, VoidSuppressorBehavior self)
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


    }


}
