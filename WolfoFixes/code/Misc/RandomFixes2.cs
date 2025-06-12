using RoR2;
using EntityStates;

namespace WolfoFixes
{
    public class RandomFixes2
    {

        public static void Start()
        {

            //Fix error spam on Captain Spawn
            On.RoR2.CaptainDefenseMatrixController.TryGrantItem += (orig, self) =>
            {
                orig(self);
                CaptainSupplyDropController supplyController = self.characterBody.GetComponent<CaptainSupplyDropController>();
                if (supplyController)
                {
                    supplyController.CallCmdSetSkillMask(3);
                    //Bonus stock from body 1 could work fine
                }
            };

            On.RoR2.PortalStatueBehavior.PreStartClient += NewtAvailableFix12;
            On.RoR2.TeleporterInteraction.OnSyncShouldAttemptToSpawnShopPortal += NewtAvailableFix2;

            //What the fuck is this for
            /*On.RoR2.ShopTerminalBehavior.UpdatePickupDisplayAndAnimations += (orig, self) =>
            {
                if (self.pickupIndex == PickupIndex.none && self.GetComponent<PurchaseInteraction>().available)
                {
                    self.hasStarted = false;
                    orig(self);
                    self.hasStarted = true;
                    return;
                }
                orig(self);
            };*/

            On.RoR2.PurchaseInteraction.ShouldDisplayHologram += DisableEmptyHologram;
            On.RoR2.MultiShopController.ShouldDisplayHologram += DisableEmptyHologram2;

            On.EntityStates.VagrantNovaItem.ChargeState.OnExit += ChargeState_OnExit;
        }

        private static void ChargeState_OnExit(On.EntityStates.VagrantNovaItem.ChargeState.orig_OnExit orig, EntityStates.VagrantNovaItem.ChargeState self)
        {
            //Idk does this fix it randomly being around for clientss
            //Probably has smth to do with lag skipping the OnExit?
            if (self.chargeVfxInstance != null)
            {
                EntityState.Destroy(self.chargeVfxInstance);
            }
            if (self.areaIndicatorVfxInstance != null)
            {
                EntityState.Destroy(self.areaIndicatorVfxInstance);
            }
            orig(self);
        }

        private static bool DisableEmptyHologram2(On.RoR2.MultiShopController.orig_ShouldDisplayHologram orig, MultiShopController self, UnityEngine.GameObject viewer)
        {
            if (self.costType == CostTypeIndex.None)
            {
                return false;
            }
            return orig(self, viewer);
        }

        private static bool DisableEmptyHologram(On.RoR2.PurchaseInteraction.orig_ShouldDisplayHologram orig, PurchaseInteraction self, UnityEngine.GameObject viewer)
        {
            if (self.costType == CostTypeIndex.None)
            {
                return false;
            }
            return orig(self,viewer);
        }

        private static void NewtAvailableFix12(On.RoR2.PortalStatueBehavior.orig_PreStartClient orig, PortalStatueBehavior self)
        {
            orig(self);
            self.GetComponent<PurchaseInteraction>().setUnavailableOnTeleporterActivated = true;
        }

        private static void NewtAvailableFix2(On.RoR2.TeleporterInteraction.orig_OnSyncShouldAttemptToSpawnShopPortal orig, TeleporterInteraction self, bool newValue)
        {
            orig(self, newValue);
            if (newValue == true)
            {
                foreach (PortalStatueBehavior portalStatueBehavior in UnityEngine.Object.FindObjectsOfType<PortalStatueBehavior>())
                {
                    if (portalStatueBehavior.portalType == PortalStatueBehavior.PortalType.Shop)
                    {
                        PurchaseInteraction component = portalStatueBehavior.GetComponent<PurchaseInteraction>();
                        if (component)
                        {
                            component.Networkavailable = false;
                            portalStatueBehavior.CallRpcSetPingable(portalStatueBehavior.gameObject, false);
                        }
                    }
                }
            }

        }






    }

}
