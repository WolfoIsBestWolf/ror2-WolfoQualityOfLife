using RoR2;
//using System;
using UnityEngine;
using UnityEngine.UI;

namespace WolfoQoL_Client.Text
{

    public class EquipmentDrone
    {
        public static void Start()
        {
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/EquipmentDroneMaster").AddComponent<EquipmentDroneNameComponent>(); ;
            On.RoR2.UI.AllyCardController.UpdateInfo += AllyCardController_UpdateInfo;
        }

        private static void AllyCardController_UpdateInfo(On.RoR2.UI.AllyCardController.orig_UpdateInfo orig, RoR2.UI.AllyCardController self)
        {
            orig(self);
            if (self.sourceMaster)
            {
                EquipmentDroneNameComponent drone = null;
                if (self.sourceMaster.TryGetComponent<EquipmentDroneNameComponent>(out drone))
                {
                    //self.GetComponent<LayoutElement>().preferredWidth += 12;
                    self.transform.GetChild(1).GetComponent<VerticalLayoutGroup>().spacing = 0;
                }
            }
        }
    }
    public class EquipmentDroneNameComponent : MonoBehaviour
    {
        //Start for some reason refuses to work on Clients so I guess we'll just fucking run it until it works
        public CharacterMaster master;
        public CharacterBody body;
        public Inventory inventory;
        private bool enigma = false;
        public bool setName = false;
        private EquipmentIndex lastEquipmentIndex = EquipmentIndex.None;

        public void OnEnable()
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.Enigma))
            {
                enigma = true;
            }
            if (WConfig.cfgEquipmentDroneName.Value == WConfig.ColorOrNot.Off)
            {
                Destroy(this);
                return;
            }
            master = this.GetComponent<CharacterMaster>();
            master.onBodyStart += Master_onBodyStart;
            body = master.GetBody();

            inventory = this.GetComponent<Inventory>();
            inventory.onEquipmentChangedClient += Inventory_onEquipmentChangedClient;
            lastEquipmentIndex = inventory.currentEquipmentIndex;


            AssignName();
        }
        public void OnDisable()
        {
            if (master)
            {
                master.onBodyStart -= Master_onBodyStart;
            }
            if (inventory)
            {
                inventory.onEquipmentChangedClient -= Inventory_onEquipmentChangedClient;
            }

        }
        private void Master_onBodyStart(CharacterBody obj)
        {
            body = obj;
            AssignName();
        }



        private void Inventory_onEquipmentChangedClient(EquipmentIndex arg1, uint arg2)
        {
            if (lastEquipmentIndex != inventory.currentEquipmentIndex)
            {
                lastEquipmentIndex = inventory.currentEquipmentIndex;
                AssignName();
            }
        }

        public void AssignName()
        {
            if (body && lastEquipmentIndex != EquipmentIndex.None)
            {
                if (inventory.currentEquipmentIndex == DLC1Content.Equipment.BossHunterConsumed.equipmentIndex)
                {
                    body.baseNameToken = "EQUIPMENTDRONE_BODY_NAME";
                }
                else
                {
                    if (WConfig.cfgEquipmentDroneName.Value == WConfig.ColorOrNot.Colored)
                    {
                        body.baseNameToken = Language.GetString("EQUIPMENTDRONE_BODY_NAME") + "\n(" + Help.GetColoredName(inventory.currentEquipmentIndex) + ")";
                    }
                    else
                    {
                        EquipmentDef equip = EquipmentCatalog.GetEquipmentDef(inventory.currentEquipmentIndex);
                        body.baseNameToken = Language.GetString("EQUIPMENTDRONE_BODY_NAME") + "\n(" + Language.GetString(equip.nameToken) + ")";
                    }
                }
            }
        }
    }

}
