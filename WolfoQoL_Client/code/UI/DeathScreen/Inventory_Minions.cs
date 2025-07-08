using RoR2;
using RoR2.Artifacts;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
 
namespace WolfoQoL_Client.DeathScreen
{
    public class Inventory_Minions
    {
        public static void CountOnGameover(Run run, RunReport runReport)
        {
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
            {
                player.gameObject.AddComponent<DroneCollection>();
            }
        }
        
        public static void AddMinionInventory(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo)
        {
            if (!Run.instance)
            {
                return;
            }
            //Inventory for Devotion
            //I guess Inventory that can also show how many Drones you have?
            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();

            if (extras.minionInventory == null)
            {
                GameObject minionInventory = extras.MakeInventory();

                minionInventory.name = "DroneCollection";
                LayoutElement layout = minionInventory.GetComponent<LayoutElement>();
                ItemInventoryDisplay inv = minionInventory.GetComponentInChildren<ItemInventoryDisplay>();

                //What we just did 1 time changes and cloned killer inventory instead?

                //Just have shared values somewhere

                //How do headers stay the same size just apply that to the body??   

                //If both inventories active and side by side maxIconWidit = 48??

                //Devotion?

                extras.minionInventory = minionInventory;
                var aaa = inv.gameObject.AddComponent<DroneCollectionDisplay>();
                aaa.root = minionInventory;
               
            }
            if (playerInfo.master == null)
            {
                extras.minionInventory.SetActive(false);
                return;
            }

            DroneCollection helper = playerInfo.master.GetComponent<DroneCollection>();
            if (helper == null)
            {
                extras.minionInventory.SetActive(false);
                return;
            }

 
            if (helper.hasDevotion && helper.hasDrones)
            {
                extras.minionInventory.transform.GetChild(0).GetChild(0).GetComponent<LanguageTextMeshController>().token = "INVENTORY_DEVOTIONDRONE";
            }
            else if (helper.hasDevotion)
            {
                extras.minionInventory.transform.GetChild(0).GetChild(0).GetComponent<LanguageTextMeshController>().token = "INVENTORY_DEVOTION";
            }
            else
            {
                extras.minionInventory.transform.GetChild(0).GetChild(0).GetComponent<LanguageTextMeshController>().token = "INVENTORY_DRONES";
            }
            extras.minionInventory.GetComponentInChildren<DroneCollectionDisplay>().Display(helper);
     
 
        }

    }

    public class DroneCollectionDisplay : MonoBehaviour
    {
        public ItemInventoryDisplay itemDisplay;
        public GameObject root;
        public DroneCollection latest;
        public void OnEnable()
        {
            itemDisplay = GetComponent<ItemInventoryDisplay>();
        }
        public void Display(DroneCollection droneCollection)
        {
            latest = droneCollection;
            if (droneCollection == null)
            {
                root.SetActive(false); 
                return;
            }
            if (droneCollection.noMinions)
            {
                root.SetActive(false);
                return;
            }
            root.SetActive(true);
            itemDisplay = GetComponent<ItemInventoryDisplay>();
            itemDisplay.enabled = false;
            int count = droneCollection.bodyDict_Drone.Count + droneCollection.bodyDict_Devotion.Count;
            count += droneCollection.itemDict_Drone.Count + droneCollection.itemDict_Devotion.Count;
            count += droneCollection.equipmentDict_Drone.Count + droneCollection.equipmentDict_Devotion.Count;
            Debug.Log("Drone Display "+ count);
  
            icon = 0;
            itemDisplay.AllocateIcons(count);
            AssignIcon(droneCollection.bodyDict_Drone);
            AssignIcon(droneCollection.itemDict_Drone);
            AssignIcon(droneCollection.equipmentDict_Drone);
            AssignIcon(droneCollection.bodyDict_Devotion);
            AssignIcon(droneCollection.itemDict_Devotion);
            AssignIcon(droneCollection.equipmentDict_Devotion);
            if (icon == 0)
            {
                root.SetActive(false);
            }
        }

        public int icon = 0;
        public void AssignIcon(Dictionary<BodyIndex, int> dict)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                var value = dict.ElementAt(i);
                var body = BodyCatalog.GetBodyPrefabBodyComponent(value.Key);
                itemDisplay.itemIcons[icon].SetItemIndex(ItemIndex.Count, value.Value);
                itemDisplay.itemIcons[icon].image.texture = body.portraitIcon;
                var toolTip = itemDisplay.itemIcons[icon].GetComponent<TooltipProvider>();
                toolTip.titleToken = body.baseNameToken;
                toolTip.bodyToken = "";
                toolTip.overrideBodyText = "";
                toolTip.titleColor = new Color32(1, 112, 200, 255);
                icon++;
            }
        }
        public void AssignIcon(Dictionary<EquipmentDef, int> dict)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                var value = dict.ElementAt(i);
    
                itemDisplay.itemIcons[icon].SetItemIndex(ItemIndex.Count, value.Value);
                itemDisplay.itemIcons[icon].image.texture = value.Key.pickupIconTexture;
                EquipOnDeathInventory.MakeEquipmentIcon(itemDisplay.itemIcons[icon], value.Key, value.Value);
                icon++;
            }
        }
        public void AssignIcon(Dictionary<ItemDef, int> dict)
        {
            for (int i = 0; i < dict.Count; i++)
            {
                var value = dict.ElementAt(i);
                itemDisplay.itemIcons[icon].SetItemIndex(value.Key.itemIndex, value.Value);
                icon++;
            }
        }
    }
        
    public class DroneCollection : MonoBehaviour
    {
        public Dictionary<BodyIndex, int> bodyDict_Drone;
        public Dictionary<BodyIndex, int> bodyDict_Devotion;
   
        public Dictionary<ItemDef, int> itemDict_Drone;
        public Dictionary<ItemDef, int> itemDict_Devotion;

        public Dictionary<EquipmentDef, int> equipmentDict_Drone;
        public Dictionary<EquipmentDef, int> equipmentDict_Devotion;
        public bool hasDrones = false;
        public bool hasDevotion = false;
        public bool noMinions = true;
        public void OnEnable()
        {
            //Drone
            //Drone Items (?)
            //Drone Equipment
            //Devotion Symbol (?)
            //Devotion Lemurians
            //Devotion Items
            //Devotion Equipment

            CharacterMaster master = GetComponent<CharacterMaster>();   
            var owner = this.GetComponent<MinionOwnership.MinionGroup.MinionGroupDestroyer>();
            if (owner == null)
            {
                return;
            }
            if (owner.group == null)
            {
                return;
            }
            if (owner.group.memberCount == 0)
            {
                return;
            }
            noMinions = false;
            bodyDict_Drone = new Dictionary<BodyIndex, int>();
            bodyDict_Devotion = new Dictionary<BodyIndex, int>();
            itemDict_Drone = new Dictionary<ItemDef, int>();
            itemDict_Devotion = new Dictionary<ItemDef, int>();
            equipmentDict_Drone = new Dictionary<EquipmentDef, int>();
            equipmentDict_Devotion = new Dictionary<EquipmentDef, int>();
            for (int i = 0; i < owner.group.memberCount; i++)
            {
                AddToCollection(owner.group.members[i]);
            }
 
            foreach (var devotion in DevotionInventoryController.InstanceList)
            {
                if (devotion.SummonerMaster == master)
                {
                    hasDevotion = true;
                    AddItemsFrom(devotion._devotionMinionInventory, true);
                }
            }
        
        }

        public void AddToCollection(MinionOwnership minion)
        {
            if (!minion)
            {
                return;
            }
            if (!minion.GetComponent<SetDontDestroyOnLoad>())
            {
                return;
            }
            CharacterMaster master = minion.GetComponent<CharacterMaster>();
            if (master == null || master.bodyPrefab == null) 
            {
                return; //???
            }
            bool isDevotedLem = master.GetComponent<DevotedLemurianController>();
            if (!isDevotedLem)
            {
                hasDrones = true;
                AddItemsFrom(master.inventory, false);
            }
            AddEquipmentFrom(master.inventory, isDevotedLem);
            BodyIndex index = master.bodyPrefab.GetComponent<CharacterBody>().bodyIndex;
            if (isDevotedLem)
            {
                if (bodyDict_Devotion.TryAdd(index, 1) == false)
                {
                    bodyDict_Devotion[index]++;
                }
            }
            else
            {
                if (bodyDict_Drone.TryAdd(index, 1) == false)
                {
                    bodyDict_Drone[index]++;
                }
            }
 
        }

        public void AddItemsFrom(Inventory inventory, bool isDevotion)
        {
            for (int itemIndex = 0; itemIndex < inventory.itemStacks.Length; itemIndex++)
            {
                int num = inventory.itemStacks[(int)itemIndex];
                if (num > 0)
                {
                    ItemDef def = ItemCatalog.allItemDefs[itemIndex];
                    if (def.hidden == false && def.tier != ItemTier.NoTier)
                    {
                        if (isDevotion)
                        {
                            if (itemDict_Devotion.TryAdd(def, num) == false)
                            {
                                itemDict_Devotion[def] += num;
                            }
                        }
                        else
                        {
                            if (itemDict_Drone.TryAdd(def, num) == false)
                            {
                                itemDict_Drone[def] += num;
                            }
                        }
                    }
                }
            }

        }
        public void AddEquipmentFrom(Inventory inventory, bool isDevotion)
        {
            for (int e = 0; e < inventory.equipmentStateSlots.Length; e++)
            {
                var slot = inventory.equipmentStateSlots[e];
                if (slot.equipmentDef)
                {
                    if (isDevotion)
                    {
                        if (equipmentDict_Devotion.TryAdd(slot.equipmentDef, 1) == false)
                        {
                            equipmentDict_Devotion[slot.equipmentDef]++;
                        }
                    }
                    else
                    {
                        if (equipmentDict_Drone.TryAdd(slot.equipmentDef, 1) == false)
                        {
                            equipmentDict_Drone[slot.equipmentDef]++;
                        }
                    }

                }
            }
        }

    }

}
