using RoR2;
using RoR2.Artifacts;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
 
namespace WolfoQoL_Client.DeathScreen
{
    public class Inventory_Killer
    {
        public static void AddKillerInventory(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo)
        {
            if (!Run.instance)
            {
                return;
            }
            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();

            if (extras.killerInventory == null)
            {
                GameObject newInventory = extras.MakeInventory();
                newInventory.name = "KillerItemArea";
                extras.killerInventory = newInventory;
                newInventory.SetActive(true);
            }
            if (playerInfo.networkUser == null)
            {
                extras.killerInventory.SetActive(false);
                WolfoMain.log.LogWarning("Killer Inventory : No NetworkUser");
                return;
            }

            KillerInventoryInfoStorage helper = playerInfo.master.GetComponent<KillerInventoryInfoStorage>();
            if (helper == null)
            {
                WolfoMain.log.LogMessage("No GameEndInventoryHelper Found, Making One");
                helper = playerInfo.master.gameObject.AddComponent<KillerInventoryInfoStorage>();
                helper.victimMaster = playerInfo.master.gameObject;

            }
 
            WolfoMain.log.LogMessage("Applying Killer Inventory Screen");

            #region Detailed Killer Name
            if (helper.killerName != "")
            {
                string KillerNameOverride = string.Format(Language.GetString("STAT_KILLER_NAME_FORMAT"), helper.killerName);
                self.killerBodyLabel.SetText(KillerNameOverride);
            }
            self.playerBodyLabel.alignment = TMPro.TextAlignmentOptions.Left;
            if (!WConfig.DC_KillerInventory.Value)
            {
                extras.killerInventory.SetActive(false);
                return;
            }
            #endregion


            //Make it so this can run without a Helper present too
            if (self.displayData.runReport.gameEnding.isWin)
            {
                helper.primaryEquipment = EquipmentIndex.None;
                helper.itemAcquisitionOrder = new List<ItemIndex>();
            }

            bool NoKillerButWithEvo = false;
            bool HasViewableEquipment = helper.primaryEquipment != EquipmentIndex.None && (EquipmentCatalog.GetEquipmentDef(helper.primaryEquipment).passiveBuffDef == null || WConfig.DC_KillerInventory_EvenIfJustElite.Value);

            //If no Killer, try to find static sources of items.
            if (helper.itemAcquisitionOrder.Count == 0 && !HasViewableEquipment)
            {
                WolfoMain.log.LogMessage("Could not find Killer Inventory or Inventory empty");
                if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.monsterTeamGainsItemsArtifactDef))
                {
                    NoKillerButWithEvo = true;
                    if (MonsterTeamGainsItemsArtifactManager.monsterTeamInventory)
                    {
                        helper.AddItemsFrom(MonsterTeamGainsItemsArtifactManager.monsterTeamInventory.itemStacks, ItemFilters.AllowAllItemFilterDelegate);
                    }
                }
                if (Run.instance && Run.instance is InfiniteTowerRun)
                {
                    NoKillerButWithEvo = true;
                    helper.AddItemsFrom(Run.instance.GetComponent<Inventory>().itemStacks, ItemFilters.AllowAllItemFilterDelegate);
                }
                if (ArenaMissionController.instance)
                {
                    NoKillerButWithEvo = true;
                    helper.AddItemsFrom(ArenaMissionController.instance.GetComponent<Inventory>().itemStacks, ItemFilters.AllowAllItemFilterDelegate);
                }
            }

            extras.IsEvoInventory = NoKillerButWithEvo;
            if (NoKillerButWithEvo)
            {
                extras.killerInventory.transform.GetChild(0).GetChild(0).GetComponent<LanguageTextMeshController>().token = "INVENTORY_MONSTER";     
            }
            else
            {
                extras.killerInventory.transform.GetChild(0).GetChild(0).GetComponent<LanguageTextMeshController>().token = "INVENTORY_KILLER";    
            }


            //If items found
            if (helper.itemAcquisitionOrder.Count != 0 || HasViewableEquipment)
            {
                self.itemInventoryDisplay.maxHeight = 256;
                EquipOnDeathInventory.DeathEquip_Enemy1 = helper.primaryEquipment;
                extras.killerInventory.SetActive(true);

                ItemInventoryDisplay inventoryDisplay = extras.killerInventory.GetComponentInChildren<ItemInventoryDisplay>();
                inventoryDisplay.SetItems(helper.itemAcquisitionOrder, helper.itemStacks);
                inventoryDisplay.UpdateDisplay();
            }
            else
            {

                extras.killerInventory.SetActive(false);
                if (extras.killerInventory)
                {
                    ItemInventoryDisplay KillerInvDisplay = extras.killerInventory.GetComponentInChildren<ItemInventoryDisplay>();
                    KillerInvDisplay.ResetItems();
                }

            }


        }

    }
    
    public class KillerInventoryMessage : RoR2.ChatMessageBase
    {
        public override string ConstructChatString()
        {
            if (attackerObject != null)
            {
                killerBackupName = Util.GetBestBodyName(attackerObject);
                killerBackupName = killerBackupName.Replace("\n", " ");
            }
            else
            {
                killerBackupName = Language.GetString(killerBackupName);
            }

            //WolfoMain.log.LogMessage("SendGameEndInvHelper");
            KillerInventoryInfoStorage.SetupFromData(killerBackupName, attackerObject, victimMaster, itemStacks, primaryEquipment, secondaryEquipment, false);
            return null;
        }

        public string killerBackupName;
        public GameObject attackerObject;
        public GameObject victimMaster;
        public int[] itemStacks = ItemCatalog.RequestItemStackArray();
        public EquipmentIndex[] equipmentStacks;
        public EquipmentIndex primaryEquipment = EquipmentIndex.None;
        public EquipmentIndex secondaryEquipment = EquipmentIndex.None;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(killerBackupName);
            writer.Write(attackerObject);
            writer.Write(victimMaster);
            writer.Write(primaryEquipment);
            writer.Write(secondaryEquipment);
            writer.WriteItemStacks(itemStacks);
        }

        public override void Deserialize(NetworkReader reader)
        {
            if (WolfoMain.NoHostInfo == true)
            {
                return;
            }
            base.Deserialize(reader);
            killerBackupName = reader.ReadString();
            attackerObject = reader.ReadGameObject();
            victimMaster = reader.ReadGameObject();
            primaryEquipment = reader.ReadEquipmentIndex();
            secondaryEquipment = reader.ReadEquipmentIndex();
            reader.ReadItemStacks(itemStacks);
        }

    }

    public class KillerInventoryInfoStorage : MonoBehaviour
    {
        public string killerName = "The Planet";
        public GameObject killerObject;
        public GameObject victimMaster;

        public List<ItemIndex> itemAcquisitionOrder = new List<ItemIndex>();
        public int[] itemStacks = ItemCatalog.RequestItemStackArray();

        public EquipmentIndex primaryEquipment = EquipmentIndex.None;
        public EquipmentIndex secondaryEquipment = EquipmentIndex.None;
        //public EquipmentIndex[] equipment = Array.Empty<EquipmentIndex>();

        private bool overwrittenByWin = false;
        public bool killerNoMaster = false;

        public static void SetupFromData(string killerName, GameObject killerObject, GameObject victimMaster, int[] itemStacks, EquipmentIndex primaryEquipment, EquipmentIndex secondaryEquipment, bool killerNoMaster)
        {
            WolfoMain.log.LogMessage("GameEndInventoryHelp : SetupFromData");
            if (victimMaster == null)
            {
                WolfoMain.log.LogWarning("GameEndInventoryHelper : Victim Object is null");
                return;
            }

            KillerInventoryInfoStorage[] oldHelpers = victimMaster.GetComponents<KillerInventoryInfoStorage>();
            if (oldHelpers.Length > 0)
            {
                for (int i = 0; i < oldHelpers.Length; i++)
                {
                    DestroyImmediate(oldHelpers[i]);
                }
            }
            KillerInventoryInfoStorage helper = victimMaster.AddComponent<KillerInventoryInfoStorage>();

            helper.victimMaster = victimMaster;
            helper.killerName = killerName;
            helper.killerObject = killerObject;
            helper.killerNoMaster = killerNoMaster;
            helper.primaryEquipment = primaryEquipment;
            helper.secondaryEquipment = secondaryEquipment;

            helper.SetItems(itemStacks);
        }

        public void SetItems(int[] otherItemStacks)
        {
            if (otherItemStacks == null)
            {
                return;
            }
            AddItemsFrom(otherItemStacks, ItemFilters.Tier1DeathItemFilterDelegate);
            AddItemsFrom(otherItemStacks, ItemFilters.Tier2DeathItemFilterDelegate);
            AddItemsFrom(otherItemStacks, ItemFilters.Tier3DeathItemFilterDelegate);
            AddItemsFrom(otherItemStacks, ItemFilters.BossDeathItemFilterDelegate);
            AddItemsFrom(otherItemStacks, ItemFilters.LunarDeathItemFilterDelegate);
            AddItemsFrom(otherItemStacks, ItemFilters.Void1DeathItemFilterDelegate);
            AddItemsFrom(otherItemStacks, ItemFilters.NoTierDeathItemFilterDelegate);
        }

        public void AddItemsFrom([JetBrains.Annotations.NotNull] int[] otherItemStacks, [JetBrains.Annotations.NotNull] Func<ItemIndex, bool> filter)
        {
            for (ItemIndex itemIndex = (ItemIndex)0; itemIndex < (ItemIndex)this.itemStacks.Length; itemIndex++)
            {
                int num = otherItemStacks[(int)itemIndex];
                if (num > 0 && filter(itemIndex))
                {
                    int[] array = this.itemStacks;
                    ItemIndex itemIndex2 = itemIndex;
                    if (array[(int)itemIndex2] == 0)
                    {
                        this.itemAcquisitionOrder.Add(itemIndex);
                    }
                    array[(int)itemIndex2] += num;
                }
            }
            //base.SetDirtyBit(1U);
            //base.SetDirtyBit(8U);
        }



    }

}
