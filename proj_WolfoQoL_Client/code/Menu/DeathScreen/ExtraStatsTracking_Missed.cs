using RoR2;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.DeathScreen
{

    public static partial class ExtraStatsTracking
    {

        public static void Start_Missed()
        {
            On.RoR2.PurchaseInteraction.Start += MissedCallback_Most;
            On.RoR2.MultiShopController.Start += MissedCallback_Shop;
            On.RoR2.DroneVendorMultiShopController.Start += MissedCallback_DroneShop;
            On.RoR2.CharacterAI.LemurianEggController.OnDestroy += LemurianEggController_OnDestroy;


            //Manually add so we dont have to check
            //Adaptive Chest
            Addressables.LoadAssetAsync<GameObject>(key: "fd68bfe758b25714288565a552d40d83").WaitForCompletion().AddComponent<OnDestroyMissedInteractable>().type = OnDestroyMissedInteractable.Cases.Chest;
            //TempShop
            Addressables.LoadAssetAsync<GameObject>(key: "d31df5066858329458b33f21b3b22d2e").WaitForCompletion().AddComponent<OnDestroyMissedInteractable>().type = OnDestroyMissedInteractable.Cases.Chest;

        }
        private static void MissedCallback_Most(On.RoR2.PurchaseInteraction.orig_Start orig, PurchaseInteraction self)
        {
            orig(self);
            //CANNOT filter for cost type none because Cloaked Chest
            //(which is half the joke of this stat)
            if (self.gameObject.activeInHierarchy)
            {
                if (self.TryGetComponent<ShrineChanceBehavior>(out var chance))
                {
                    self.gameObject.AddComponent<OnDestroyMissedInteractable>().type = OnDestroyMissedInteractable.Cases.ShrineChance;

                }
                else if (self.GetComponent<SummonMasterBehavior>())
                {
                    self.gameObject.AddComponent<OnDestroyMissedInteractable>().type = OnDestroyMissedInteractable.Cases.Drone;
                }
                else if (self.costType != CostTypeIndex.LunarCoin)
                {
                    bool realChest = false;
                    if (self.TryGetComponent<ChestBehavior>(out var chest))
                    {
                        //Filter out frozenwall fan because hopoo james
                        if (chest.dropTable)
                        {
                            realChest = true;
                        }
                        if (SceneCatalog.mostRecentSceneDef.isFinalStage)
                        {
                            realChest = self.costType == CostTypeIndex.TreasureCacheItem;
                        }
                    }
                    if (realChest || self.GetComponent<OptionChestBehavior>())
                    {
                        self.gameObject.AddComponent<OnDestroyMissedInteractable>().type = OnDestroyMissedInteractable.Cases.Chest;
                    }
                }


            }
        }


        private static void LemurianEggController_OnDestroy(On.RoR2.CharacterAI.LemurianEggController.orig_OnDestroy orig, LemurianEggController self)
        {
            orig(self);
            if (self.GetComponent<PickupPickerController>().available)
            {
                if (RunExtraStatTracker.instance)
                {
                    RunExtraStatTracker.instance.missedLemurians++;
                }

            }
        }

        private static void MissedCallback_DroneShop(On.RoR2.DroneVendorMultiShopController.orig_Start orig, DroneVendorMultiShopController self)
        {
            orig(self);
            if (self.gameObject.activeInHierarchy)
            {
                self.gameObject.AddComponent<OnDestroyMissedInteractable>().type = OnDestroyMissedInteractable.Cases.DroneShop;
            }
        }

        private static void MissedCallback_Shop(On.RoR2.MultiShopController.orig_Start orig, MultiShopController self)
        {
            orig(self);
            if (self.gameObject.activeInHierarchy)
            {
                self.gameObject.AddComponent<OnDestroyMissedInteractable>().type = OnDestroyMissedInteractable.Cases.MultiShop;
            }
        }


    }

    public class OnDestroyMissedInteractable : MonoBehaviour
    {
        public enum Cases
        {
            Chest,
            Drone,
            ShrineChance,
            LemEgg,
            MultiShop,
            DroneShop,
        }
        public Cases type;

        public void OnDestroy()
        {
            //Debug.Log(this);
            if (!RunExtraStatTracker.instance)
            {
                return;
            }
            if (type < Cases.LemEgg)
            {
                if (GetComponent<PurchaseInteraction>().available)
                {
                    switch (type)
                    {
                        case Cases.Chest:
                            RunExtraStatTracker.instance.missedChests++;
                            break;
                        case Cases.Drone:
                            RunExtraStatTracker.instance.missedDrones++;
                            break;
                        case Cases.ShrineChance:
                            ShrineChanceBehavior shrine = GetComponent<ShrineChanceBehavior>();
                            RunExtraStatTracker.instance.missedShrineChanceItems += (shrine.maxPurchaseCount - shrine.successfulPurchaseCount);
                            break;
                    }
                }
            }
            else if (type == Cases.MultiShop)
            {
                if (this.GetComponent<MultiShopController>().available)
                {
                    RunExtraStatTracker.instance.missedChests++;
                }
            }
            else if (type == Cases.Drone)
            {
                if (this.GetComponent<DroneVendorMultiShopController>().available)
                {
                    RunExtraStatTracker.instance.missedDrones++;
                }
            }

        }
    }

}
