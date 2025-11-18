using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static WolfoLibrary.ExtraActions;

namespace WolfoQoL_Client.DeathScreen
{
    public class Inventory_Minions
    {
        public static void Hooks()
        {
            Run.onClientGameOverGlobal += CountOnGameover;

            onMithrixPhase1 += Count;
            onVoidlingPhase1 += Count;
            onFalseSonPhase1 += Count;
            onSolusWing += Count;
            onSolusHeart += Count;
        }
        public static void Count()
        {
            //Count Mith Start -> Prevent being counted again
            if (RunExtraStatTracker.instance && !RunExtraStatTracker.instance.alreadyCountedDronesForStage)
            {
                RunExtraStatTracker.instance.alreadyCountedDronesForStage = true;
                foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
                {
                    player.gameObject.GetComponent<DroneCollection>().CountDrones();
                }
            }
        }
        public static void CountOnGameover(Run run, RunReport runReport)
        {
            Count();
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
            //Display any that can be upgraded invdividually?
            //But Turrets as 1 fat stack ig
            //Equip Drones idk ig theycant be combined neither
            //Equip Drone
            //EquipDroneEquip1
            //EquipDrone
            //EquipDroneEquip2?


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
            root.transform.parent.gameObject.SetActive(true);
            itemDisplay = GetComponent<ItemInventoryDisplay>();
            itemDisplay.enabled = false;
            int count = droneCollection.droneDict.Count + droneCollection.bodyDict_Devotion.Count;
            count += droneCollection.itemDict_Devotion.Count;
            count += droneCollection.equipmentDrones.Count + droneCollection.equipmentDict_Devotion.Count;
            WQoLMain.log.LogMessage("Drone Display " + count);

            icon = 0;
            itemDisplay.AllocateIcons(count);
            if (droneCollection.droneDict.Count > 0)
            {
                AssignDroneIcon(droneCollection);
            }
            if (droneCollection.bodyDict_Devotion.Count > 0)
            {
                AssignDevotionIcon(droneCollection);
            }
            if (icon == 0)
            {
                //root.SetActive(false);
            }

        }

        public int equipDroneIt;
        public int icon = 0;
        public void AssignDroneIcon(DroneCollection droneCollection)
        {
            for (int i = 0; i < droneCollection.droneDict.Count; i++)
            {
                var itemIcon = itemDisplay.itemIcons[icon];
                icon++;
                //var value = droneCollection.droneList[i];
                var value = droneCollection.droneDict.ElementAt(i);


                CharacterBody body = BodyCatalog.GetBodyPrefabBodyComponent(value.Key.Item1);
                int tier = value.Key.Item2;
                DroneDef droneDef = DroneCatalog.FindDroneDefFromBody(body.gameObject);

                itemIcon.SetItemIndex(ItemIndex.Count, value.Value, 0);
                itemIcon.image.texture = body.portraitIcon;
                var toolTip = itemIcon.GetComponent<TooltipProvider>();

                body.bodyColor.a = 1;
                toolTip.titleColor = body.bodyColor;
                toolTip.titleToken = body.baseNameToken;
                toolTip.overrideTitleText = string.Empty;
                toolTip.overrideBodyText = string.Empty;
                if (droneDef)
                {
                    toolTip.bodyToken = droneDef.skillDescriptionToken;
                }
                else
                {
                    toolTip.bodyToken = string.Empty;
                }

                if (tier > 0)
                {
                    tier++;
                    toolTip.overrideTitleText = Language.GetStringFormatted("DRONE_TIER_SUFFIX", new object[] { Language.GetString(body.baseNameToken), tier });
                    itemIcon.stackText.gameObject.SetActive(true);
                    itemIcon.stackText.text = "T" + tier;
                    itemIcon.stackText.transform.localPosition = new Vector3(63.8f, -104, 0);
                    itemIcon.stackText.fontSizeMax = 18;
                }
                else
                {
                    itemIcon.stackText.gameObject.SetActive(false);
                }


                if (droneDef == RoR2Content.DroneDefs.EquipmentDrone)
                {
                    foreach (var equipDef in droneCollection.equipmentDrones)
                    {
                        EquipOnDeathInventory.MakeEquipmentIcon(itemDisplay.itemIcons[icon], equipDef, 1);
                        icon++;
                    }
                }


            }
        }

        public void AssignDevotionIcon(DroneCollection droneCollection)
        {
            //LEMURIANS
            for (int i = 0; i < droneCollection.bodyDict_Devotion.Count; i++)
            {
                var itemIcon = itemDisplay.itemIcons[icon];
                icon++;

                var value = droneCollection.bodyDict_Devotion.ElementAt(i);
                CharacterBody body = BodyCatalog.GetBodyPrefabBodyComponent(value.Key);

                itemIcon.SetItemIndex(ItemIndex.Count, value.Value, 0);
                itemIcon.image.texture = body.portraitIcon;
                var toolTip = itemIcon.GetComponent<TooltipProvider>();
                toolTip.titleToken = body.baseNameToken;
                toolTip.overrideBodyText = "";
                toolTip.bodyToken = "";
                toolTip.titleColor = body.bodyColor;
                itemIcon.spriteAsNumberManager.transform.localPosition = new Vector3(41.944f, -48.39f, 0);

            }
            //ITEMS
            for (int i = 0; i < droneCollection.itemDict_Devotion.Count; i++)
            {
                var value = droneCollection.itemDict_Devotion.ElementAt(i);
                itemDisplay.itemIcons[icon].SetItemIndex(value.Key.itemIndex, value.Value, 0);
                icon++;
            }
            //EQUIPMENT
            for (int i = 0; i < droneCollection.equipmentDict_Devotion.Count; i++)
            {
                var value = droneCollection.equipmentDict_Devotion.ElementAt(i);

                itemDisplay.itemIcons[icon].SetItemIndex(ItemIndex.Count, value.Value, 0);
                itemDisplay.itemIcons[icon].image.texture = value.Key.pickupIconTexture;
                EquipOnDeathInventory.MakeEquipmentIcon(itemDisplay.itemIcons[icon], value.Key, value.Value);
                icon++;
            }
        }
    }

    public class DroneCollection : MonoBehaviour
    {
        public static bool isPermamentMinion(MinionOwnership minion)
        {
            if (!minion.GetComponent<SetDontDestroyOnLoad>())
            {
                return false;
            }
            if (minion.GetComponent<MasterSuicideOnTimer>())
            {
                return false;
            }
            Inventory inv = minion.GetComponent<Inventory>();
            if (inv.GetItemCountPermanent(RoR2Content.Items.HealthDecay) > 0)
            {
                return false;
            }
            if (inv.GetItemCountPermanent(RoR2Content.Items.Ghost) > 0)
            {
                return false;
            }
            if (inv.GetItemCountPermanent(DLC1Content.Items.GummyCloneIdentifier) > 0)
            {
                return false;
            }
            return true;
        }

        public Dictionary<ValueTuple<BodyIndex, int>, int> droneDict; //Drone, Tier, Quantity
        public List<ValueTuple<BodyIndex, int>> droneList;

        public List<EquipmentDef> equipmentDrones;

        public Dictionary<BodyIndex, int> bodyDict_Devotion;
        public Dictionary<ItemDef, int> itemDict_Devotion;
        public Dictionary<EquipmentDef, int> equipmentDict_Devotion;

        public bool hasDrones = false;
        public bool hasDevotion = false;
        public bool noMinions = true;
        public void CountDrones()
        {
            //If Drone, Cant Combine -> Stack
            //If Lem -> Stack
            //If EquipDrone -> Equip / Equip
            //If Drone, Can Upgrade -> Individual w Tier


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

            droneList = new List<(BodyIndex, int)>();
            droneDict = new Dictionary<(BodyIndex, int), int>();
            equipmentDrones = new List<EquipmentDef>();

            bodyDict_Devotion = new Dictionary<BodyIndex, int>();
            itemDict_Devotion = new Dictionary<ItemDef, int>();
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

            //hasDrones = droneList.Count > 0;
            hasDrones = droneDict.Count > 0;
        }
        public void Reset()
        {
            RunExtraStatTracker.instance.alreadyCountedDronesForStage = false;
            noMinions = true;
            droneDict = null;
            equipmentDrones = null;
            bodyDict_Devotion = null;
            itemDict_Devotion = null;
            equipmentDict_Devotion = null;
        }


        public void AddToCollection(MinionOwnership minion)
        {
            if (!minion)
            {
                return;
            }
            if (!isPermamentMinion(minion))
            {
                return;
            }

            CharacterMaster master = minion.GetComponent<CharacterMaster>();
            if (master == null || master.bodyPrefab == null)
            {
                return; //???
            }
            if (master.inventory.GetItemCountPermanent(RoR2Content.Items.HealthDecay) > 0)
            {
                return;
            }
            bool isDevotedLem = master.GetComponent<DevotedLemurianController>();
            if (!isDevotedLem)
            {
                hasDrones = true;
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
                ValueTuple<BodyIndex, int> drone = new(index, master.inventory.GetItemCountPermanent(DLC3Content.Items.DroneUpgradeHidden));
                if (droneDict.TryAdd(drone, 1) == false)
                {
                    droneDict[drone]++;
                }
                //droneList.Add(new(index, master.inventory.GetItemCountPermanent(DLC3Content.Items.DroneUpgradeHidden)));
            }

        }

        public void AddItemsFrom(Inventory inventory, bool isDevotion)
        {
            for (int itemAq = 0; itemAq < inventory.itemAcquisitionOrder.Count; itemAq++)
            {
                ItemIndex itemIndex = inventory.itemAcquisitionOrder[itemAq];
                int num = inventory.GetItemCountPermanent(itemIndex);
                if (num > 0)
                {
                    ItemDef def = ItemCatalog.GetItemDef(itemIndex);
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
                            //Cut
                        }
                    }
                }
            }

        }

        public void AddEquipmentFrom(Inventory inventory, bool isDevotion)
        {
            var slot = inventory.currentEquipmentState;
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
                    equipmentDrones.Add(slot.equipmentDef);
                }
            }
        }

    }

}
