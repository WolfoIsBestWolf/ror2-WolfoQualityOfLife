using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoQualityOfLife
{
    public class ExpandedDeathScreen
    {
        public static EquipmentIndex DeathEquipPlayer1 = EquipmentIndex.None;
        public static EquipmentIndex DeathEquipPlayer2 = EquipmentIndex.None;
        public static EquipmentIndex DeathEquipEnemy1 = EquipmentIndex.None;
        public static EquipmentIndex DeathEquipEnemy2 = EquipmentIndex.None;

        public static void Start()
        {
            //Double check if there already is a GameEndInventoryHelper
            if (WConfig.cfgExpandedDeathScreen.Value == true)
            {
                On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += GameEndMakeKillerInventory;
                On.RoR2.UI.ItemInventoryDisplay.AllocateIcons += GameEndEquipInv;
                On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += GameEndInventoryHelp;
            };
            On.RoR2.UI.GameEndReportPanelController.SetDisplayData += GameEndMoreStatsShown;

            GameObject hud = LegacyResourcesAPI.Load<GameObject>("Prefabs/HUDSimple");
            hud.GetComponent<RoR2.UI.HUD>().lunarCoinContainer.transform.GetChild(0).GetComponent<UnityEngine.UI.RawImage>().color = new Color(0.5199f, 0.5837f, 0.66f, 0.1333f);//0.6288 0.4514 0.6509 0.1333
        }

        private static void GameEndMoreStatsShown(On.RoR2.UI.GameEndReportPanelController.orig_SetDisplayData orig, GameEndReportPanelController self, GameEndReportPanelController.DisplayData newDisplayData)
        {
            //Debug.LogWarning(self.statsToDisplay.Length);
            if (WConfig.cfgDeathScreenStats.Value)
            {
                if (Run.instance)
                {
                    if (Run.instance.GetComponent<InfiniteTowerRun>())
                    {
                        string[] newstats = new string[] { "totalTimeAlive", "highestInfiniteTowerWaveReached", "totalItemsCollected", "totalKills", "totalEliteKills", "totalDamageDealt", "highestDamageDealt", "totalMinionKills", "totalMinionDamageDealt", "totalHealthHealed", "totalDamageTaken", "totalDeaths", "totalDistanceTraveled", "highestLevel", "totalPurchases", "totalLunarPurchases", "totalBloodPurchases", "totalGoldCollected" };
                        self.statsToDisplay = newstats;
                    }
                    else
                    {
                        string[] newstats = new string[] { "totalTimeAlive", "totalStagesCompleted", "totalItemsCollected", "totalKills", "totalEliteKills", "totalDamageDealt", "highestDamageDealt", "totalMinionKills", "totalMinionDamageDealt", "totalHealthHealed", "totalDamageTaken", "totalDeaths", "totalDistanceTraveled", "highestLevel", "totalPurchases", "totalLunarPurchases", "totalBloodPurchases", "totalDronesPurchased", "totalGoldCollected" };
                        self.statsToDisplay = newstats;
                    };
                }
            }
            
            orig(self, newDisplayData);
            if (WConfig.cfgDeathScreenAlwaysChatBox.Value)
            {
                if (self.chatboxTransform)
                {
                    self.chatboxTransform.gameObject.SetActive(true);
                }
            }
           
        }

        private static void GameEndInventoryHelp(On.RoR2.GlobalEventManager.orig_OnPlayerCharacterDeath orig, GlobalEventManager self, DamageReport damageReport, NetworkUser victimNetworkUser)
        {
            orig(self, damageReport, victimNetworkUser);

            if (damageReport.victimMaster.IsDeadAndOutOfLivesServer())
            {
                if (damageReport.attackerMaster && damageReport.attackerMaster.inventory)
                {
                    string newString = RoR2.Util.GetBestBodyName(damageReport.attacker);
                    newString = newString.Replace("\n", " ");

                    //This is Host to Client, Client to Host is entirely different beast
                    Debug.Log(newString);
                    RoR2.Chat.SendBroadcastChat(new SendGameEndInvHelper
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

                    RoR2.Chat.SendBroadcastChat(new SendGameEndInvHelper
                    {
                        killerName = newString,
                        victimMaster = damageReport.victimMaster.gameObject,
                    });
                }
            }
        }

        public static void GameEndMakeKillerInventory(On.RoR2.UI.GameEndReportPanelController.orig_SetPlayerInfo orig, global::RoR2.UI.GameEndReportPanelController self, global::RoR2.RunReport.PlayerInfo playerInfo, int playerIndex)
        {
            orig(self, playerInfo, playerIndex);

            //This seems kinda stupid but it seems to work 
            DeathEquipPlayer1 = EquipmentIndex.None;
            DeathEquipPlayer2 = EquipmentIndex.None;
            if (playerInfo.equipment.Length > 0)
            {
                DeathEquipPlayer1 = playerInfo.equipment[0];
            }
            if (playerInfo.equipment.Length > 1)
            {
                DeathEquipPlayer2 = playerInfo.equipment[1];
            }
            //
            //Killer Inventory Display
            DeathEquipEnemy1 = EquipmentIndex.None;
            DeathEquipEnemy2 = EquipmentIndex.None;

            if (Run.instance)
            {
                if (playerInfo.networkUser == null)
                {
                    Debug.LogWarning("Killer Inventory : No NetworkUser");
                    return;
                }

                GameEndInventoryHelper helper = playerInfo.master.GetComponent<GameEndInventoryHelper>();
                if (helper == null)
                {
                    Debug.Log("No GameEndInventoryHelper Found, Making One");
                    helper = playerInfo.master.gameObject.AddComponent<GameEndInventoryHelper>();
                    helper.victimMaster = playerInfo.master.gameObject;
                }
                else if (helper.victimMaster.GetComponent<RoR2.PlayerCharacterMasterController>().networkUser != playerInfo.networkUser)
                {
                    Debug.Log("Report for other Network User");
                }
                Debug.Log("Applying Killer Inventory Screen");



                string KillerName;
               
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

                bool HasViewableEquipment = helper.primaryEquipment != EquipmentIndex.None && EquipmentCatalog.GetEquipmentDef(helper.primaryEquipment).passiveBuffDef == null;

                if (helper.itemAcquisitionOrder.Count == 0 && !HasViewableEquipment)
                {
                    Debug.Log("Could not find Killer Inventory or Inventory empty");
                    if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.monsterTeamGainsItemsArtifactDef))
                    {
                        IsWinWithEvo = true;
                        GameObject EvoInventory = GameObject.Find("MonsterTeamGainsItemsArtifactInventory(Clone)");
                        if (EvoInventory)
                        {
                            helper.AddItemsFrom(EvoInventory.GetComponent<Inventory>().itemStacks, AllowAllItemFilterDelegate);
                        }
                    }
                    if (Run.instance && Run.instance.name.StartsWith("InfiniteTowerRun(Clone)"))
                    {
                        IsWinWithEvo = true;
                        helper.AddItemsFrom(Run.instance.GetComponent<Inventory>().itemStacks, AllowAllItemFilterDelegate);
                    }
                    GameObject ArenaMissionController = GameObject.Find("/ArenaMissionController");
                    if (ArenaMissionController)
                    {
                        IsWinWithEvo = true;
                        helper.AddItemsFrom(ArenaMissionController.GetComponent<Inventory>().itemStacks, AllowAllItemFilterDelegate);
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

                    DeathEquipEnemy1 = helper.primaryEquipment;
                    DeathEquipEnemy2 = helper.secondaryEquipment;

                    if (KillerInvDisplayObj)
                    {
                        if (IsWinWithEvo)
                        {
                            KillerInvDisplayObj.name = "EvolutionArea";
                        }
                        else
                        {
                            KillerInvDisplayObj.name = "ItemArea(Clone)";
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

        public static void GameEndEquipInv(On.RoR2.UI.ItemInventoryDisplay.orig_AllocateIcons orig, global::RoR2.UI.ItemInventoryDisplay self, int desiredItemCount)
        {
            //History Fix adds equipment Icons too, gotta add compatibility 
            //Debug.LogWarning("ItemInventoryDisplay : AllocateIcons");
            if (self.name.StartsWith("Content"))
            {
                //Debug.LogWarning(DeathEquip1);
                //Debug.LogWarning(DeathEquip2);


                int num = 0;
                ItemIndex[] array = ItemCatalog.RequestItemOrderBuffer();
                for (int i = 0; i < self.itemOrderCount; i++)
                {
                    if (ItemInventoryDisplay.ItemIsVisible(self.itemOrder[i]))
                    {
                        array[num++] = self.itemOrder[i];
                    }
                }
                desiredItemCount = num;

                if (self.transform.parent.parent.parent.name.StartsWith("ItemArea(Clone)"))
                {
                    if (DeathEquipEnemy1 != EquipmentIndex.None && DeathEquipEnemy2 == EquipmentIndex.None)
                    {
                        orig(self, desiredItemCount + 1);
                        //Debug.LogWarning("Equip 1");
                        //Have 1 Equipment
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        MakeEquipmentIcon(EquipIcon1, DeathEquipEnemy1);
                    }
                    else if (DeathEquipEnemy1 == EquipmentIndex.None && DeathEquipEnemy2 == EquipmentIndex.None)
                    {
                        //Have No Equipment
                        orig(self, desiredItemCount);
                    }
                    else if (DeathEquipEnemy1 != EquipmentIndex.None && DeathEquipEnemy1 == DeathEquipEnemy2)
                    {
                        orig(self, desiredItemCount + 1);
                        //Debug.LogWarning("Equip 1 = Equip 2");
                        //Have 2 of the same Equipment
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 2);
                        MakeEquipmentIcon(EquipIcon1, DeathEquipEnemy1);
                    }
                    else if (DeathEquipEnemy1 != EquipmentIndex.None && DeathEquipEnemy2 != EquipmentIndex.None)
                    {
                        orig(self, desiredItemCount + 2);
                        //Debug.LogWarning("Equip 1 + Equip 2");
                        //Have 2 different Equipment
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        MakeEquipmentIcon(EquipIcon1, DeathEquipEnemy1);

                        GameObject EquipIcon2 = self.gameObject.transform.GetChild(desiredItemCount + 1).gameObject;
                        EquipIcon2.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        MakeEquipmentIcon(EquipIcon2, DeathEquipEnemy2);
                    }
                    else if (DeathEquipEnemy2 != EquipmentIndex.None && DeathEquipEnemy1 == EquipmentIndex.None)
                    {
                        orig(self, desiredItemCount + 1);
                        //Debug.LogWarning("Equip 1");
                        //Have 1 Equipment in Alt slot
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        MakeEquipmentIcon(EquipIcon1, DeathEquipEnemy2);
                    }
                    else
                    {
                        orig(self, desiredItemCount);
                    }
                }
                else if (self.transform.parent.parent.parent.name.StartsWith("EvolutionArea"))
                {
                    orig(self, desiredItemCount);
                }
                else
                {
                    if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("local.fix.history"))
                    {
                        //Debug.Log("someone elses problem now");
                        orig(self, desiredItemCount);
                        return;
                    }

                    if (DeathEquipPlayer1 != EquipmentIndex.None && DeathEquipPlayer2 == EquipmentIndex.None)
                    {
                        orig(self, desiredItemCount + 1);
                        //Debug.LogWarning("Equip 1");
                        //Have 1 Equipment
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        MakeEquipmentIcon(EquipIcon1, DeathEquipPlayer1);
                    }
                    else if (DeathEquipPlayer1 == EquipmentIndex.None && DeathEquipPlayer2 == EquipmentIndex.None)
                    {
                        //Have No Equipment
                        orig(self, desiredItemCount);
                    }
                    else if (DeathEquipPlayer1 != EquipmentIndex.None && DeathEquipPlayer1 == DeathEquipPlayer2)
                    {
                        orig(self, desiredItemCount + 1);
                        //Debug.LogWarning("Equip 1 = Equip 2");
                        //Have 2 of the same Equipment
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 2);
                        MakeEquipmentIcon(EquipIcon1, DeathEquipPlayer1);
                    }
                    else if (DeathEquipPlayer1 != EquipmentIndex.None && DeathEquipPlayer2 != EquipmentIndex.None)
                    {
                        orig(self, desiredItemCount + 2);
                        //Debug.LogWarning("Equip 1 + Equip 2");
                        //Have 2 different Equipment
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        MakeEquipmentIcon(EquipIcon1, DeathEquipPlayer1);

                        GameObject EquipIcon2 = self.gameObject.transform.GetChild(desiredItemCount + 1).gameObject;
                        EquipIcon2.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        MakeEquipmentIcon(EquipIcon2, DeathEquipPlayer2);
                    }
                    else if (DeathEquipPlayer2 != EquipmentIndex.None && DeathEquipPlayer1 == EquipmentIndex.None)
                    {
                        orig(self, desiredItemCount + 1);
                        //Debug.LogWarning("Equip 1");
                        //Have 1 Equipment in Alt slot
                        GameObject EquipIcon1 = self.gameObject.transform.GetChild(desiredItemCount).gameObject;
                        EquipIcon1.GetComponent<RoR2.UI.ItemIcon>().SetItemIndex((ItemIndex)1, 1);
                        MakeEquipmentIcon(EquipIcon1, DeathEquipPlayer2);
                    }
                    else
                    {
                        //Failsafe IG
                        orig(self, desiredItemCount);
                    }
                }


                RoR2.UI.HGTextMeshProUGUI tempHeader = self.gameObject.transform.parent.parent.parent.GetChild(0).GetChild(0).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>();
                if (tempHeader)
                {
                    if (self.transform.parent.parent.parent.name.StartsWith("ItemArea(Clone)"))
                    {
                        tempHeader.GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText(Language.GetString("INVENTORY_KILLER"));
                    }
                    else if (self.transform.parent.parent.parent.name.StartsWith("EvolutionArea"))
                    {
                        tempHeader.GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText(Language.GetString("INVENTORY_MONSTER"));
                    }
                    else
                    {
                        //tempHeader.GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText("Items & Equipment Collected"); //Maybe a bit redundant we'll see
                    }
                }
                else
                {
                    tempHeader = self.gameObject.transform.parent.parent.GetChild(0).GetChild(0).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>();
                    if (tempHeader)
                    {
                        //tempHeader.GetComponent<RoR2.UI.HGTextMeshProUGUI>().SetText("Items & Equipment Collected");
                    }
                }

            }
            else
            {
                orig(self, desiredItemCount);
            }

        }



        public static void MakeEquipmentIcon(GameObject equipmentIcon, EquipmentIndex equipmentIndex)
        {
            Debug.Log("Making EquipmentIcon");
            EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
            equipmentIcon.GetComponent<UnityEngine.UI.RawImage>().texture = equipmentDef.pickupIconTexture;
            RoR2.UI.TooltipProvider tempTooltip = equipmentIcon.GetComponent<RoR2.UI.TooltipProvider>();
            equipmentIcon.name = "EquipmentIcon";

            tempTooltip.overrideTitleText = Language.GetString(equipmentDef.nameToken);
            tempTooltip.overrideBodyText = Language.GetString(equipmentDef.descriptionToken);
            tempTooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
            tempTooltip.titleColor = ColorCatalog.GetColor(equipmentDef.colorIndex);

        }

        public static readonly Func<ItemIndex, bool> Tier1DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier1DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Tier2DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier2DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Tier3DeathItemFilterDelegate = new Func<ItemIndex, bool>(Tier3DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> BossDeathItemFilterDelegate = new Func<ItemIndex, bool>(BossDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> LunarDeathItemFilterDelegate = new Func<ItemIndex, bool>(LunarDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> Void1DeathItemFilterDelegate = new Func<ItemIndex, bool>(Void1DeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> NoTierDeathItemFilterDelegate = new Func<ItemIndex, bool>(NoTierDeathItemCopyFilter);
        public static readonly Func<ItemIndex, bool> AllowAllItemFilterDelegate = new Func<ItemIndex, bool>(AllowAllItemFilter);

        public static bool AllowAllItemFilter(ItemIndex itemIndex)
        {
            return true;
        }

        public static bool NoTierDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.NoTier) { return false; }
            if (!tempdef.pickupIconTexture || tempdef.pickupIconTexture.name.StartsWith("texNullIcon"))
            {
                return false;
            }
            return true;
        }
        public static bool Tier1DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier1) { return false; }
            return true;
        }
        public static bool Tier2DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier2) { return false; }
            return true;
        }
        public static bool Tier3DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Tier3) { return false; }
            return true;
        }
        public static bool LunarDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Lunar) { return false; }
            return true;
        }
        public static bool BossDeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier != ItemTier.Boss) { return false; }
            return true;
        }

        public static bool Void1DeathItemCopyFilter(ItemIndex itemIndex)
        {
            ItemDef tempdef = ItemCatalog.GetItemDef(itemIndex);
            if (tempdef.tier == ItemTier.VoidTier1 |
                tempdef.tier == ItemTier.VoidTier2 |
                tempdef.tier == ItemTier.VoidTier3 |
                tempdef.tier == ItemTier.VoidBoss) { return true; }
            return false;
        }


        public class GameEndInventoryHelper : MonoBehaviour
        {
            public string killerName = "The Planet";
            public GameObject killerObject;
            public GameObject victimMaster;

            public List<ItemIndex> itemAcquisitionOrder = new List<ItemIndex>();
            public int[] itemStacks = ItemCatalog.RequestItemStackArray();

            public EquipmentIndex primaryEquipment = EquipmentIndex.None;
            public EquipmentIndex secondaryEquipment = EquipmentIndex.None;

            private bool overwrittenByWin = false;

            public static void SetupFromData(string killerName, GameObject killerObject, GameObject victimMaster, int[] itemStacks, EquipmentIndex primaryEquipment, EquipmentIndex secondaryEquipment)
            {
                Debug.Log("GameEndInventoryHelp : SetupFromData");
                if (victimMaster == null)
                {
                    Debug.LogWarning("GameEndInventoryHelper : Victim Object is null");
                }

                GameEndInventoryHelper[] oldHelpers = victimMaster.GetComponents<GameEndInventoryHelper>();
                if (oldHelpers.Length > 0)
                {
                    for (int i = 0; i < oldHelpers.Length; i++)
                    {
                        DestroyImmediate(oldHelpers[i]);
                    }
                }
                GameEndInventoryHelper helper = victimMaster.AddComponent<GameEndInventoryHelper>();

                helper.victimMaster = victimMaster;
                helper.killerName = killerName;
                helper.killerObject = killerObject;
                helper.primaryEquipment = primaryEquipment;
                helper.secondaryEquipment = secondaryEquipment;
                helper.SetItems(itemStacks);
            }

            public void SetItems(int[] otherItemStacks)
            {
                AddItemsFrom(otherItemStacks, Tier1DeathItemFilterDelegate);
                AddItemsFrom(otherItemStacks, Tier2DeathItemFilterDelegate);
                AddItemsFrom(otherItemStacks, Tier3DeathItemFilterDelegate);
                AddItemsFrom(otherItemStacks, BossDeathItemFilterDelegate);
                AddItemsFrom(otherItemStacks, LunarDeathItemFilterDelegate);
                AddItemsFrom(otherItemStacks, Void1DeathItemFilterDelegate);
                AddItemsFrom(otherItemStacks, NoTierDeathItemFilterDelegate);
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

        public class SendGameEndInvHelper : RoR2.ChatMessageBase
        {
            public override string ConstructChatString()
            {
                Debug.Log("SendGameEndInvHelper");
                ExpandedDeathScreen.GameEndInventoryHelper.SetupFromData(killerName, killerObject, victimMaster, itemStacks, primaryEquipment, secondaryEquipment);
                return null;
            }

            public string killerName;
            public GameObject killerObject;
            public GameObject victimMaster;
            public int[] itemStacks = ItemCatalog.RequestItemStackArray();
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

}
