using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoFixes
{

   internal class OptionPickupStuff
    {
        public static void Start()
        {
            On.RoR2.PickupPickerController.SetOptionsInternal += OptionPickup_Fixes;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/OptionPickup/OptionPickerPanel.prefab").WaitForCompletion().GetComponent<RoR2.UI.PickupPickerPanel>().maxColumnCount = 3; //Hud does not support 5 columns

        }

        private static void OptionPickup_Fixes(On.RoR2.PickupPickerController.orig_SetOptionsInternal orig, PickupPickerController self, PickupPickerController.Option[] newOptions)
        {
            orig(self, newOptions);

            PickupIndexNetworker index = self.GetComponent<PickupIndexNetworker>();
            if (index != null)
            {

                //Void Command Essence lacks particles.
                if (self.name.StartsWith("Command"))
                {
                    if (index.pickupDisplay)
                    {
                        var pickupDef = index.pickupIndex.pickupDef;
                        if (pickupDef.itemTier >= ItemTier.VoidTier1 && pickupDef.itemTier <= ItemTier.VoidBoss)
                        {
                            //Not default token so double checking
                            if (Language.english.TokenIsRegistered("ARTIFACT_COMMAND_CUBE_PINK_NAME"))
                            {
                                self.gameObject.GetComponent<GenericDisplayNameProvider>().displayToken = "ARTIFACT_COMMAND_CUBE_PINK_NAME";
                            }
                            //Double checking for other mods
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
                    //Command does not Spin
                    //Fix spinning on client.
                    PickupDisplay pickupDisplay = self.transform.GetChild(0).GetComponent<PickupDisplay>();
                    pickupDisplay.pickupIndex = index.pickupIndex;
                    pickupDisplay.modelObject = self.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                    self.GetComponent<Highlight>().pickupIndex = index.pickupIndex;
                    self.GetComponent<Highlight>().isOn = true;

                }

            }






        }



    }
}