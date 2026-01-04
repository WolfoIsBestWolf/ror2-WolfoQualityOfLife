using RoR2;
using UnityEngine;


namespace WolfoQoL_Client.Text
{

    public static class OptionsInChat
    {
        public static void Start()
        {


            On.RoR2.PickupPickerController.SetOptionsInternal += ChoicesInName;
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

        private static void ChoicesInName(On.RoR2.PickupPickerController.orig_SetOptionsInternal orig, PickupPickerController self, PickupPickerController.Option[] newOptions)
        {
            orig(self, newOptions);

            PickupIndexNetworker index = self.GetComponent<PickupIndexNetworker>();

            bool isVoid = self.name.StartsWith("Option");
            bool isFragment = self.name.StartsWith("Fragment");
            if (isFragment && WConfig.cfgAurFragment_ItemsInPing.Value)
            {
                OptionsInPing(self, "AURELIONITE_FRAGMENT_PICKUP_NAME");
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