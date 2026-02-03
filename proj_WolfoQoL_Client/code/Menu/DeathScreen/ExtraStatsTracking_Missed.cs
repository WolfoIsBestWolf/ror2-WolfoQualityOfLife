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
            None = -1,
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
            Cases type2 = Cases.None;
            IDisplayNameProvider nameProvider = GetComponent<IDisplayNameProvider>();
            if (nameProvider == null)
            {
                return;
            }
            string name = nameProvider.GetDisplayName();
            if (type < Cases.LemEgg)
            {
                if (GetComponent<PurchaseInteraction>().available)
                {
                    switch (type)
                    {
                        case Cases.Chest:
                            type2 = Cases.Chest;
                            break;
                        case Cases.Drone:
                            type2 = Cases.Drone;
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
                if (GetComponent<MultiShopController>().available)
                {
                    type2 = Cases.Chest;
                }
            }
            else if (type == Cases.Drone)
            {
                if (GetComponent<DroneVendorMultiShopController>().available)
                {
                    type2 = Cases.Drone;
                }
            }

     
            if (type2 == Cases.Chest)
            {
                RunExtraStatTracker.instance.missedChests++;
                if (RunExtraStatTracker.instance.missedChests_Dict.ContainsKey(name))
                {
                    RunExtraStatTracker.instance.missedChests_Dict[name]++;
                }
                else
                {
                    RunExtraStatTracker.instance.missedChests_Dict.Add(name, 1);
                }
            }
            else if (type2 == Cases.Drone)
            {
                RunExtraStatTracker.instance.missedDrones++;
                if (RunExtraStatTracker.instance.missedDrones_Dict.ContainsKey(name))
                {
                    RunExtraStatTracker.instance.missedDrones_Dict[name]++;
                }
                else
                {
                    RunExtraStatTracker.instance.missedDrones_Dict.Add(name, 1);
                }
            }

        }
    }

}
