using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoQoL_Client
{
    public class KillerInventory
    {
        public static void Send_KillerInventory_Host(On.RoR2.GlobalEventManager.orig_OnPlayerCharacterDeath orig, GlobalEventManager self, DamageReport damageReport, NetworkUser victimNetworkUser)
        {
            orig(self, damageReport, victimNetworkUser);

            if (damageReport.victimMaster.IsDeadAndOutOfLivesServer())
            {
                if (damageReport.attackerMaster && damageReport.attackerMaster.inventory)
                {
                    string newString = RoR2.Util.GetBestBodyName(damageReport.attacker);
                    newString = newString.Replace("\n", " ");

                    //This is Host to Client
                    Debug.Log(newString);
                    Chat.SendBroadcastChat(new KillerInventoryMessage
                    {
                        killerName = newString,
                        killerObject = damageReport.attacker,
                        victimMaster = damageReport.victimMaster.gameObject,
                        itemStacks = damageReport.attackerMaster.inventory.itemStacks,
                        primaryEquipment = damageReport.attackerMaster.inventory.currentEquipmentIndex,
                        secondaryEquipment = damageReport.attackerMaster.inventory.alternateEquipmentIndex,
                    });
                    //GameEndInventoryHelper.SetupFromData(newString, victimNetworkUser.gameObject, damageReport.attackerMaster.inventory.itemStacks, damageReport.attackerMaster.inventory.currentEquipmentIndex, damageReport.attackerMaster.inventory.alternateEquipmentIndex);
                }
                else if (damageReport.attackerBody != null)
                {
                    string newString = RoR2.Util.GetBestBodyName(damageReport.attacker);
                    newString = newString.Replace("\n", " ");

                    Chat.SendBroadcastChat(new KillerInventoryMessage
                    {
                        killerName = newString,
                        victimMaster = damageReport.victimMaster.gameObject,
                    });
                }
            }
        }

        public static void AddKillerInventory(On.RoR2.UI.GameEndReportPanelController.orig_SetPlayerInfo orig, global::RoR2.UI.GameEndReportPanelController self, global::RoR2.RunReport.PlayerInfo playerInfo, int playerIndex)
        {
            orig(self, playerInfo, playerIndex);

            if (Run.instance)
            {
                if (playerInfo.networkUser == null)
                {
                    Debug.LogWarning("Killer Inventory : No NetworkUser");
                    return;
                }

                KillerInventoryInfoStorage helper = playerInfo.master.GetComponent<KillerInventoryInfoStorage>();
                if (helper == null)
                {
                    Debug.Log("No GameEndInventoryHelper Found, Making One");
                    helper = playerInfo.master.gameObject.AddComponent<KillerInventoryInfoStorage>();
                    helper.victimMaster = playerInfo.master.gameObject;
                }
                else if (helper.victimMaster.GetComponent<RoR2.PlayerCharacterMasterController>().networkUser != playerInfo.networkUser)
                {
                    Debug.Log("Report for other Network User");
                }
                Debug.Log("Applying Killer Inventory Screen");




                string KillerNameBase = "Killed By: <color=#FFFF7F>" + helper.killerName;
                bool IsLossToPlanet = false;
                bool IsWinWithEvo = false;
                if (helper.killerObject)
                {
                    helper.killerName = Util.GetBestBodyName(helper.killerObject);
                }
                else
                {
                    helper.killerName = "";
                }

                //Set detailed name like with Elite prefix
                if (helper.killerName != "")
                {
                    string KillerNameOverride = string.Format(Language.GetString("STAT_KILLER_NAME_FORMAT"), helper.killerName);
                    self.killerBodyLabel.SetText(KillerNameOverride);
                }
                else
                {
                    IsLossToPlanet = true;
                }
                self.playerBodyLabel.alignment = TMPro.TextAlignmentOptions.Left;

                //Make it so this can run without a Helper present too
                if (self.displayData.runReport.gameEnding.isWin)
                {
                    helper.primaryEquipment = EquipmentIndex.None;
                    helper.itemAcquisitionOrder = new List<ItemIndex>();
                }

                //bool HasViewableEquipment = helper.primaryEquipment != EquipmentIndex.None && EquipmentCatalog.GetEquipmentDef(helper.primaryEquipment).passiveBuffDef == null;
                bool HasViewableEquipment = helper.primaryEquipment != EquipmentIndex.None && (EquipmentCatalog.GetEquipmentDef(helper.primaryEquipment).passiveBuffDef == null || WConfig.cfgKillerInvAlwaysDisplay.Value);

                if (helper.itemAcquisitionOrder.Count == 0 && !HasViewableEquipment)
                {
                    Debug.Log("Could not find Killer Inventory or Inventory empty");
                    if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.monsterTeamGainsItemsArtifactDef))
                    {
                        IsWinWithEvo = true;
                        GameObject EvoInventory = GameObject.Find("MonsterTeamGainsItemsArtifactInventory(Clone)");
                        if (EvoInventory)
                        {
                            helper.AddItemsFrom(EvoInventory.GetComponent<Inventory>().itemStacks, ItemFilters.AllowAllItemFilterDelegate);
                        }
                    }
                    if (Run.instance && Run.instance.name.StartsWith("InfiniteTowerRun(Clone)"))
                    {
                        IsWinWithEvo = true;
                        helper.AddItemsFrom(Run.instance.GetComponent<Inventory>().itemStacks, ItemFilters.AllowAllItemFilterDelegate);
                    }
                    GameObject ArenaMissionController = GameObject.Find("/ArenaMissionController");
                    if (ArenaMissionController)
                    {
                        IsWinWithEvo = true;
                        helper.AddItemsFrom(ArenaMissionController.GetComponent<Inventory>().itemStacks, ItemFilters.AllowAllItemFilterDelegate);
                    }
                }

                if (helper.itemAcquisitionOrder.Count != 0 || HasViewableEquipment)
                {
                    GameObject KillerInvDisplayObj;
                    if (self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent.childCount == 4)
                    {
                        self.itemInventoryDisplay.enabled = false;
                        KillerInvDisplayObj = UnityEngine.Object.Instantiate(self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.gameObject, self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent);
                        self.itemInventoryDisplay.enabled = true;
                    }
                    else
                    {
                        KillerInvDisplayObj = self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent.GetChild(4).gameObject;
                    }

                    DeathScreen_Expanded.DeathEquip_Enemy1 = helper.primaryEquipment;
                    //DeathScreen_Expanded.DeathEquip_Enemy2 = helper.secondaryEquipment;

                    if (KillerInvDisplayObj)
                    {
                        if (IsWinWithEvo)
                        {
                            KillerInvDisplayObj.name = "EvolutionArea";
                        }
                        else
                        {
                            KillerInvDisplayObj.name = "KillerItemArea";
                        }

                        RoR2.UI.ItemInventoryDisplay KillerInvDisplay = KillerInvDisplayObj.GetComponentInChildren<RoR2.UI.ItemInventoryDisplay>();
                        KillerInvDisplay.enabled = true;

                        KillerInvDisplay.SetItems(helper.itemAcquisitionOrder, helper.itemStacks);
                        KillerInvDisplay.UpdateDisplay();

                        if (self.unlockContentArea.childCount > 0)
                        {
                            self.unlockContentArea.parent.parent.parent.SetAsLastSibling();
                            self.unlockContentArea.parent.parent.parent.gameObject.SetActive(true);
                        }
                        else
                        {
                            self.unlockContentArea.parent.parent.parent.gameObject.SetActive(false);
                        }
                    }
                }
                else if (self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent.childCount == 5)
                {
                    //What the fuck is this for
                    GameObject KillerInvDisplayObj = self.itemInventoryDisplay.gameObject.transform.parent.parent.parent.parent.GetChild(4).gameObject;
                    RoR2.UI.ItemInventoryDisplay KillerInvDisplay = KillerInvDisplayObj.GetComponentInChildren<RoR2.UI.ItemInventoryDisplay>();

                    int[] writestacks = new int[ItemCatalog.itemCount];
                    List<ItemIndex> fakeitemorder = new List<ItemIndex>();
                    KillerInvDisplay.ResetItems();
                    KillerInvDisplay.SetItems(fakeitemorder, writestacks);

                    if (self.unlockContentArea.childCount > 0)
                    {
                        self.unlockContentArea.parent.parent.parent.gameObject.SetActive(true);
                    }
                }

            };
        }


        public class KillerInventoryMessage : RoR2.ChatMessageBase
        {
            public override string ConstructChatString()
            {
                if (WConfig.cfgTestClient.Value)
                {
                    return null;
                }
                Debug.Log("SendGameEndInvHelper");
                KillerInventoryInfoStorage.SetupFromData(killerName, killerObject, victimMaster, itemStacks, primaryEquipment, secondaryEquipment, false);
                return null;
            }

            public string killerName;
            public GameObject killerObject;
            public GameObject victimMaster;
            public int[] itemStacks = ItemCatalog.RequestItemStackArray();
            public EquipmentIndex[] equipmentStacks;
            public EquipmentIndex primaryEquipment = EquipmentIndex.None;
            public EquipmentIndex secondaryEquipment = EquipmentIndex.None;

            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(killerName);
                writer.Write(killerObject);
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
                killerName = reader.ReadString();
                killerObject = reader.ReadGameObject();
                victimMaster = reader.ReadGameObject();
                primaryEquipment = reader.ReadEquipmentIndex();
                secondaryEquipment = reader.ReadEquipmentIndex();
                reader.ReadItemStacks(itemStacks);
            }

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
            Debug.Log("GameEndInventoryHelp : SetupFromData");
            if (victimMaster == null)
            {
                Debug.LogWarning("GameEndInventoryHelper : Victim Object is null");
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
