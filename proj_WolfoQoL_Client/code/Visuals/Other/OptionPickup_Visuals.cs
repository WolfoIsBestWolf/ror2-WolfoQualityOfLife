using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{

    public class OptionPickup_Visuals
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

            On.RoR2.PickupPickerController.SetOptionsInternal += OptionPickup_Fixes;

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
                    if (player && player.master && player.master.inventory.GetItemCount(RoR2Content.Items.TitanGoldDuringTP) > 0)
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
        private static void OptionPickup_Fixes(On.RoR2.PickupPickerController.orig_SetOptionsInternal orig, PickupPickerController self, PickupPickerController.Option[] newOptions)
        {
            orig(self, newOptions);

            PickupIndexNetworker index = self.GetComponent<PickupIndexNetworker>();

            bool isVoid = self.name.StartsWith("Option");
            bool isFragment = self.name.StartsWith("Fragment");
            if (isFragment)
            {
                Highlight original = self.GetComponent<Highlight>();
                if (MeridianEventTriggerInteraction.instance)
                {
                    if (index.NetworkpickupState.pickupIndex == PickupCatalog.itemTierToPickupIndex[ItemTier.Tier1])
                    {
                        ItemTier tier = MeridianFragmentRedOrGreenOrWhite(false);
                        if (tier == ItemTier.Tier3)
                        {
                            /*GameObject AurelioniteHeart = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/AurelioniteHeart.prefab").WaitForCompletion();
                            SkinnedMeshRenderer Heart = AurelioniteHeart.GetComponentInChildren<SkinnedMeshRenderer>();
                            MeshRenderer FragmentMat = (original.targetRenderer as MeshRenderer);
                            MeshFilter FragmentMesh = original.targetRenderer.GetComponent<MeshFilter>();

                            FragmentMesh.sharedMesh = Heart.sharedMesh;
                            FragmentMat.sharedMaterials = Heart.sharedMaterials;*/
                        }
                        index.pickupIndex = PickupCatalog.itemTierToPickupIndex[tier];
                    }
                }
                if (WConfig.cfgFragmentColor.Value)
                {

                    GameObject newMesh = GameObject.Instantiate(original.targetRenderer.gameObject, original.targetRenderer.transform);
                    newMesh.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
                    newMesh.transform.GetChild(0).gameObject.SetActive(false);
                    newMesh.transform.GetChild(1).gameObject.SetActive(false);

                    Highlight newLine = newMesh.AddComponent<Highlight>();
                    newLine.highlightColor = Highlight.HighlightColor.custom;
                    //newLine.CustomColor = new Color(0.9f, 0.8f, 0.4f, 1);
                    //newLine.CustomColor = new Color(0.918f, 0.761f, 0.564f, 1);
                    //newLine.CustomColor = new Color(0.933f, 0.741f, 0.545f, 1);
                    newLine.CustomColor = new Color(0.933f, 0.791f, 0.505f, 1);
                    //newLine.CustomColor = new Color(1f, 0.766f, 0.5f, 1);
                    newLine.targetRenderer = original.targetRenderer;
                    newLine.isOn = true;
                    original.targetRenderer = newMesh.GetComponent<MeshRenderer>();
                    original.targetRenderer.enabled = false;
                    // new Color(0.9f, 0.8f, 0.4f);
                    //More accurate but worse color ; 0.933 0.741 0.545 1
                }
            }
            if (isVoid || isFragment)
            {

                PickupDisplay pickupDisplay = self.transform.GetChild(0).GetComponent<PickupDisplay>();
                pickupDisplay.pickupState = index.pickupState;
                pickupDisplay.modelObject = self.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                self.GetComponent<Highlight>().pickupState = index.pickupState;
                self.GetComponent<Highlight>().isOn = true;

                if (index.pickupState.pickupIndex != PickupIndex.none)
                {
                   
                    switch (index.pickupState.pickupIndex.pickupDef.itemTier)
                    {
                        case ItemTier.Tier1:
                            pickupDisplay.tier1ParticleEffect.SetActive(true);
                            break;
                        case ItemTier.Tier2:
                            pickupDisplay.tier2ParticleEffect.SetActive(true);
                            break;
                        case ItemTier.Tier3:
                            pickupDisplay.tier3ParticleEffect.SetActive(true);
                            break;
                        case ItemTier.Boss:
                            pickupDisplay.bossParticleEffect.SetActive(true);
                            break;
                        case ItemTier.Lunar:
                            pickupDisplay.lunarParticleEffect.SetActive(true);
                            break;
                        case ItemTier.VoidTier1:
                        case ItemTier.VoidTier2:
                        case ItemTier.VoidTier3:
                        case ItemTier.VoidBoss:
                            if (!pickupDisplay.voidParticleEffect)
                            {
                                pickupDisplay.voidParticleEffect = Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GenericPickup").GetComponent<GenericPickupController>().pickupDisplay.voidParticleEffect, pickupDisplay.transform);
                            }
                            pickupDisplay.voidParticleEffect.SetActive(true);
                            break;
                    }
                    if (index.pickupState.pickupIndex.pickupDef.itemTier == orange)
                    {
                        pickupDisplay.equipmentParticleEffect.SetActive(true);
                    }
                }
            }



        }



    }
}