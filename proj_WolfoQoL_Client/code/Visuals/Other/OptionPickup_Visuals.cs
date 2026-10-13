using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{

    public static class OptionPickup_Visuals
    {
        public static void Start()
        {
            #region

            //Add glows to Option Pickups
            GameObject VoidPotential = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/OptionPickup/OptionPickup.prefab").WaitForCompletion();
            Transform voidPotRoot = VoidPotential.transform.GetChild(0);
            GameObject OptionPickerPanel = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/OptionPickup/OptionPickerPanel.prefab").WaitForCompletion();
            voidPotRoot.GetChild(0).GetComponent<SphereCollider>().radius = 1.5f;
            for (int i = 1; i < voidPotRoot.childCount; i++)
            {
                voidPotRoot.GetChild(i).localPosition = new Vector3(0, -0.5f, 0);
                voidPotRoot.GetChild(i).GetComponent<OnEnableEvent>().enabled = false;
            }
            OptionPickerPanel.GetComponent<RoR2.UI.PickupPickerPanel>().maxColumnCount = 3;
            #endregion
            #region
            On.EntityStates.FalseSonBoss.SkyJumpDeathState.GiveColossusItem += FalseSon_GreenOrRed;


            GameObject GoldFragmentPotential = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/FragmentPotentialPickup.prefab").WaitForCompletion();

            Transform fragment = GoldFragmentPotential.transform.GetChild(0);
            //fragment.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
            fragment.GetChild(1).GetChild(0).GetChild(3).gameObject.SetActive(false);
            fragment.GetChild(2).GetChild(2).gameObject.SetActive(false);
            fragment.GetChild(3).GetChild(2).gameObject.SetActive(false);
            //fragment.GetChild(5).GetChild(0).GetChild(0).gameObject.SetActive(false);
            fragment.GetChild(5).GetChild(2).gameObject.SetActive(false);

            GameObject.Destroy(fragment.GetChild(1).GetComponent<OnEnableEvent>());
            GameObject.Destroy(fragment.GetChild(2).GetComponent<OnEnableEvent>());
            GameObject.Destroy(fragment.GetChild(3).GetComponent<OnEnableEvent>());
            GameObject.Destroy(fragment.GetChild(5).GetComponent<OnEnableEvent>());

            fragment.GetChild(1).localPosition = new Vector3(0, -1, 0);
            fragment.GetChild(2).localPosition = new Vector3(0, -1, 0);
            fragment.GetChild(3).localPosition = new Vector3(0, -1, 0);
            fragment.GetChild(5).localPosition = new Vector3(0, -1, 0);
            #endregion

            On.RoR2.PickupPickerController.SetOptionsInternal += OptionPickup_Edits;
            On.RoR2.PickupDisplay.RebuildModel += OptionPickupParticles;

        }


        private static void OptionPickupParticles(On.RoR2.PickupDisplay.orig_RebuildModel orig, PickupDisplay self, GameObject modelObjectOverride)
        {
            orig(self, modelObjectOverride);
          
            PickupDef pickupDef = PickupCatalog.GetPickupDef(self.pickupState.pickupIndex);

            if (pickupDef != null && pickupDef.itemTier != ItemTier.NoTier)
            {
                var root = self.transform.root;
                if (root.name.StartsWith("Option") || root.name.StartsWith("Fragment"))
                {
                    switch (pickupDef.itemTier)
                    {
                        case ItemTier.Tier1:
                            self.tier1ParticleEffect.SetActive(true);
                            break;
                        case ItemTier.Tier2:
                            self.tier2ParticleEffect.SetActive(true);
                            break;
                        case ItemTier.Tier3:
                            self.tier3ParticleEffect.SetActive(true);
                            break;
                        case ItemTier.Boss:
                            self.bossParticleEffect.SetActive(true);
                            break;
                        case ItemTier.Lunar:
                            self.lunarParticleEffect.SetActive(true);
                            break;
                        case ItemTier.VoidTier1:
                        case ItemTier.VoidTier2:
                        case ItemTier.VoidTier3:
                        case ItemTier.VoidBoss:
                            if (!self.voidParticleEffect)
                            {
                                self.voidParticleEffect = Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GenericPickup").GetComponent<GenericPickupController>().pickupDisplay.voidParticleEffect, self.transform);
                            }
                            self.voidParticleEffect.SetActive(true);
                            break;
                    }
                    if (pickupDef.itemTier == orange)
                    {
                        self.equipmentParticleEffect.SetActive(true);
                    }
                }
            }
        }


        public static ItemTier MeridianFragmentRedOrGreenOrWhite(bool colossus)
        {
            if (colossus || MeridianEventTriggerInteraction.instance.meridianEventState >= MeridianEventState.Phase3)
            {
                if (MeridianEventTriggerInteraction.instance.isGoldTitanSpawned)
                {
                    return ItemTier.Tier3;
                }
                foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
                {
                    if (player && player.master && player.master.inventory.GetItemCountEffective(RoR2Content.Items.TitanGoldDuringTP) > 0)
                    {
                        return ItemTier.Tier3;
                    }
                }
                return ItemTier.Tier2;
            }
            return ItemTier.Tier1;
        }


        private static void FalseSon_GreenOrRed(On.EntityStates.FalseSonBoss.SkyJumpDeathState.orig_GiveColossusItem orig, EntityStates.FalseSonBoss.SkyJumpDeathState self)
        {
            if (self.FalseSonUnlockStateMet())
            {
                self.rewardDisplayTier = ItemTier.Tier3;
            }
            else
            {
                self.rewardDisplayTier = ItemTier.Tier2;
            }
            orig(self);
        }



        public static ItemTier orange = (ItemTier)100;
        private static void OptionPickup_Edits(On.RoR2.PickupPickerController.orig_SetOptionsInternal orig, PickupPickerController self, PickupPickerController.Option[] newOptions)
        {
            orig(self, newOptions);
            PickupIndexNetworker index = self.GetComponent<PickupIndexNetworker>();

            bool isVoid = self.name.StartsWith("Option");
            bool isFragment = self.name.StartsWith("Fragment");
            if (isFragment)
            {
                if (MeridianEventTriggerInteraction.instance)
                {
                    if (index.NetworkpickupState.pickupIndex == PickupCatalog.itemTierToPickupIndex[ItemTier.Tier1])
                    {
                        ItemTier tier = MeridianFragmentRedOrGreenOrWhite(false);
                        index.pickupState = new UniquePickup(PickupCatalog.itemTierToPickupIndex[tier]);
                    }
                }
                if (WConfig.cfgFragmentColor.Value)
                {
                    Highlight original = self.GetComponent<Highlight>();
                    original.highlightColor = Highlight.HighlightColor.custom;
                    original.CustomColor = new Color(0.933f, 0.791f, 0.505f, 1);

                }
            }
            if (isVoid || isFragment)
            {
                //Does this even do anything??
                PickupDisplay pickupDisplay = self.transform.GetChild(0).GetComponent<PickupDisplay>();
                pickupDisplay.pickupState = index.pickupState;
                pickupDisplay.modelObject = self.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                self.GetComponent<Highlight>().pickupState = index.pickupState;
                self.GetComponent<Highlight>().isOn = true;

            }



        }



    }
}