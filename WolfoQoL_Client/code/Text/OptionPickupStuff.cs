using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{

    public class OptionPickupStuff
    {
        public static void Start()
        {
            #region



            //Add glows to Option Pickups
            GameObject VoidPotential = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/OptionPickup/OptionPickup.prefab").WaitForCompletion();
            GameObject OptionPickerPanel = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/OptionPickup/OptionPickerPanel.prefab").WaitForCompletion();
            VoidPotential.transform.GetChild(0).GetChild(0).GetComponent<SphereCollider>().radius = 1.5f;
            VoidPotential.transform.GetChild(0).GetChild(1).localPosition = new Vector3(0, -0.5f, 0);
            VoidPotential.transform.GetChild(0).GetChild(2).localPosition = new Vector3(0, -0.5f, 0);
            VoidPotential.transform.GetChild(0).GetChild(3).localPosition = new Vector3(0, -0.5f, 0);
            VoidPotential.transform.GetChild(0).GetChild(4).localPosition = new Vector3(0, -0.5f, 0);
            VoidPotential.transform.GetChild(0).GetChild(5).localPosition = new Vector3(0, -0.5f, 0);
            VoidPotential.transform.GetChild(0).GetChild(6).localPosition = new Vector3(0, -0.5f, 0);

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
            On.RoR2.UI.PickupPickerPanel.RemovePickupButtonAvailability += UpdateFragmentText;
            On.RoR2.PickupPickerController.RpcHandlePickupSelected += PickupPickerController_RpcHandlePickupSelected;
        }

        private static void PickupPickerController_RpcHandlePickupSelected(On.RoR2.PickupPickerController.orig_RpcHandlePickupSelected orig, PickupPickerController self, int choiceIndex)
        {
            orig(self, choiceIndex);
            bool isFragment = self.name.StartsWith("Fragment");
            if (isFragment)
            {
                OptionsInPing(self, "AURELIONITE_FRAGMENT_PICKUP_NAME");
            }
        }

        private static void UpdateFragmentText(On.RoR2.UI.PickupPickerPanel.orig_RemovePickupButtonAvailability orig, RoR2.UI.PickupPickerPanel self, int pickupOption)
        {
            orig(self, pickupOption);
            bool isFragment = self.name.StartsWith("Fragment");
            if (isFragment)
            {
                OptionsInPing(self.pickerController, "AURELIONITE_FRAGMENT_PICKUP_NAME");
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
            self.rewardDisplayTier = MeridianFragmentRedOrGreenOrWhite(true);
            orig(self);
        }

        public static void OptionsInPing(PickupPickerController self, string baseToken)
        {
            string NameWithOptions = Language.GetString(baseToken);
            NameWithOptions += "\n(";
            for (int i = 0; i < self.options.Length; i++)
            {
                string ItemName = "";
                string Hex = "636363";
                PickupDef defTemp = self.options[i].pickupIndex.pickupDef;
                if (defTemp.itemIndex != ItemIndex.None)
                {
                    ItemName = Language.GetString(ItemCatalog.GetItemDef(defTemp.itemIndex).nameToken);
                }
                else if (defTemp.equipmentIndex != EquipmentIndex.None)
                {
                    ItemName = Language.GetString(EquipmentCatalog.GetEquipmentDef(defTemp.equipmentIndex).nameToken);
                }
                if (self.options[i].available)
                {
                    Hex = ColorUtility.ToHtmlStringRGB(defTemp.baseColor);
                }
                else
                {
                    ItemName = "<s>" + ItemName + "</s>";
                }

                if (i != 0)
                {
                    NameWithOptions += " | <color=#" + Hex + ">" + ItemName + "</color>";

                }
                else
                {
                    NameWithOptions += "<color=#" + Hex + ">" + ItemName + "</color>";
                }
            }
            NameWithOptions += ")";
            self.GetComponent<GenericDisplayNameProvider>().SetDisplayToken(NameWithOptions);
        }

        public static ItemTier orange = (ItemTier)100;
        private static void OptionPickup_Fixes(On.RoR2.PickupPickerController.orig_SetOptionsInternal orig, PickupPickerController self, PickupPickerController.Option[] newOptions)
        {
            orig(self, newOptions);
            bool isCommand = self.name.StartsWith("Command");
            PickupIndexNetworker index = self.GetComponent<PickupIndexNetworker>();
            if (isCommand)
            {
                if (index.pickupDisplay)
                {
                    var pickupDef = index.pickupIndex.pickupDef;
                    if (pickupDef.itemTier >= ItemTier.VoidTier1 && pickupDef.itemTier <= ItemTier.VoidBoss)
                    {
                        self.gameObject.GetComponent<GenericDisplayNameProvider>().displayToken = "ARTIFACT_COMMAND_CUBE_PINK_NAME";
                        if (!index.pickupDisplay.voidParticleEffect)
                        {
                            GameObject newVoidParticle = Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GenericPickup").GetComponent<GenericPickupController>().pickupDisplay.voidParticleEffect, self.transform.GetChild(0));
                            newVoidParticle.SetActive(true);
                            GameObject newOrb = Object.Instantiate(index.pickupDisplay.tier2ParticleEffect.transform.GetChild(2).gameObject, newVoidParticle.transform);
                            newOrb.GetComponent<ParticleSystem>().startColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.VoidItem);
                            index.pickupDisplay.voidParticleEffect = newVoidParticle;
                        }
                    }
                }
            }
            else
            {
                bool isVoid = self.name.StartsWith("Option");
                bool isFragment = self.name.StartsWith("Fragment");
                if (isFragment)
                {
                    if (MeridianEventTriggerInteraction.instance)
                    {
                        index.NetworkpickupIndex = PickupCatalog.itemTierToPickupIndex[MeridianFragmentRedOrGreenOrWhite(false)];
                    }
                    if (WConfig.cfgAurFragment_ItemsInPing.Value)
                    {
                        OptionsInPing(self, "AURELIONITE_FRAGMENT_PICKUP_NAME");
                    }
                    if (WConfig.cfgFragmentColor.Value)
                    {
                        Highlight original = self.GetComponent<Highlight>();
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
                    pickupDisplay.pickupIndex = index.pickupIndex;
                    pickupDisplay.modelObject = self.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                    self.GetComponent<Highlight>().pickupIndex = index.pickupIndex;
                    self.GetComponent<Highlight>().isOn = true;

                    if (index.pickupIndex != PickupIndex.none)
                    {
                        switch (index.pickupIndex.pickupDef.itemTier)
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
                        if (index.pickupIndex.pickupDef.itemTier == orange)
                        {
                            pickupDisplay.equipmentParticleEffect.SetActive(true);
                        }
                    }
                }
                if (isVoid && WConfig.cfgVoidPotential_ItemsInPing.Value)
                {
                    if (WConfig.cfgVoidPotential_ItemsInPing.Value)
                    {
                        OptionsInPing(self, "OPTION_PICKUP_UNKNOWN_NAME");
                    }
                    else
                    {
                        self.GetComponent<GenericDisplayNameProvider>().SetDisplayToken("OPTION_PICKUP_UNKNOWN_NAME");
                    }
                }
                
            }




        }



    }
}