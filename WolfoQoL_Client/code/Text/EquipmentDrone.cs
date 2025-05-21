using RoR2;
//using System;
using UnityEngine;

namespace WolfoQoL_Client
{

    public class EquipmentDrone
    {
        public static void Start()
        {
            GameObject EquipmentDrone = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/EquipmentDroneMaster");
            EquipmentDrone.AddComponent<EquipmentDroneNameComponent>();

        }


    }
    public class EquipmentDroneNameComponent : MonoBehaviour
    {
        //Start for some reason refuses to work on Clients so I guess we'll just fucking run it until it works
        public CharacterMaster master;
        public CharacterBody body;
        public Inventory inventory;
        private bool enigma = false;
        private EquipmentIndex lastEquipmentIndex = EquipmentIndex.None;

        public void OnEnable()
        {
            if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.Enigma))
            {
                enigma = true;
            }
            if (WConfig.cfgEquipmentDroneName.Value == false)
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
            inventory.onEquipmentChangedClient -= Inventory_onEquipmentChangedClient;
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
                    var equip = EquipmentCatalog.GetEquipmentDef(inventory.currentEquipmentIndex);
                    body.baseNameToken = Language.GetString("EQUIPMENTDRONE_BODY_NAME") + "\n(" + Language.GetString(equip.nameToken) + ")";
                    //Debug.Log(body.baseNameToken);
                }
            }
        }
    }

}
