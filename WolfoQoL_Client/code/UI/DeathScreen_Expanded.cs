using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolfoQoL_Client
{
    public class DeathScreen_Expanded
    {
        public static EquipmentIndex[] DeathEquips_Player = Array.Empty<EquipmentIndex>();
        //public static EquipmentIndex[] DeathEquips_Killer = Array.Empty<EquipmentIndex>();
        //public static EquipmentIndex DeathEquip_Player1 = EquipmentIndex.None;
        //public static EquipmentIndex DeathEquip_Player2 = EquipmentIndex.None;
        public static EquipmentIndex DeathEquip_Enemy1 = EquipmentIndex.None;
        //public static EquipmentIndex DeathEquip_Enemy2 = EquipmentIndex.None;
        public static bool otherMod = false;

        public static void Start()
        {
            //Double check if there already is a GameEndInventoryHelper
            if (WConfig.cfgExpandedDeathScreen.Value == true)
            {
                On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += SetEquipmentInfoTemp;
                On.RoR2.UI.ItemInventoryDisplay.AllocateIcons += AddEquipmentIcons;

                On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += KillerInventory.AddKillerInventory;
                On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += KillerInventory.Send_KillerInventory_Host;
            };
            On.RoR2.UI.GameEndReportPanelController.SetDisplayData += GameEndMoreStatsShown;

            GameObject hud = LegacyResourcesAPI.Load<GameObject>("Prefabs/HUDSimple");
            hud.GetComponent<RoR2.UI.HUD>().lunarCoinContainer.transform.GetChild(0).GetComponent<UnityEngine.UI.RawImage>().color = new Color(0.5199f, 0.5837f, 0.66f, 0.1333f);//0.6288 0.4514 0.6509 0.1333

            otherMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("local.fix.history");

        }

        //This somehow got deleted?
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

        private static void SetEquipmentInfoTemp(On.RoR2.UI.GameEndReportPanelController.orig_SetPlayerInfo orig, GameEndReportPanelController self, RunReport.PlayerInfo playerInfo, int playerIndex)
        {
            orig(self, playerInfo, playerIndex);
            DeathEquips_Player = playerInfo.equipment;
            DeathEquip_Enemy1 = EquipmentIndex.None;
        }



        public static void AddEquipmentIcons(On.RoR2.UI.ItemInventoryDisplay.orig_AllocateIcons orig, global::RoR2.UI.ItemInventoryDisplay self, int desiredItemCount)
        {
            //It'd probably be good if this only ran on death screens but uhhhh
            if (self.name.StartsWith("Content"))
            {
                string areaName = self.transform.parent.parent.parent.name;

                Dictionary<EquipmentIndex, int> equipmentCount = new Dictionary<EquipmentIndex, int>();
                if (areaName == "KillerItemArea")
                {
                    if (DeathEquip_Enemy1 != EquipmentIndex.None)
                    {
                        equipmentCount.Add(DeathEquip_Enemy1, 1);
                    }
                }
                else if (areaName == "EvolutionArea")
                {
                }
                else
                {
                    if (!otherMod)
                    {
                        for (int i = 0; i < DeathEquips_Player.Length; i++)
                        {
                            if (DeathEquips_Player[i] != EquipmentIndex.None)
                            {
                                if (equipmentCount.ContainsKey(DeathEquips_Player[i]))
                                {
                                    equipmentCount[DeathEquips_Player[i]]++;
                                }
                                else
                                {
                                    equipmentCount.Add(DeathEquips_Player[i], 1);
                                }
                            }
                        }

                    }
                }
                //Debug.Log("equipmentCount : " + equipmentCount.Count);
                if (equipmentCount.Count == 0)
                {
                    orig(self, desiredItemCount);
                }
                else
                {
                    orig(self, desiredItemCount + equipmentCount.Count);
                    for (int i = 0; i < equipmentCount.Count; i++)
                    {
                        var pair = equipmentCount.ElementAt(i);
                        //Debug.Log(pair.Key + " : " + pair.Value);
                        MakeEquipmentIcon(self.gameObject.transform.GetChild(desiredItemCount + i).gameObject, pair.Key, pair.Value);
                    }
                }

                //Has to be past orig(self)?
                HGTextMeshProUGUI tempHeader = self.gameObject.transform.parent.parent.parent.GetChild(0).GetChild(0).gameObject.GetComponent<RoR2.UI.HGTextMeshProUGUI>();
                if (tempHeader)
                {
                    if (areaName.StartsWith("KillerItemArea"))
                    {
                        tempHeader.GetComponent<HGTextMeshProUGUI>().SetText(Language.GetString("INVENTORY_KILLER"));
                    }
                    else if (areaName.StartsWith("EvolutionArea"))
                    {
                        tempHeader.GetComponent<HGTextMeshProUGUI>().SetText(Language.GetString("INVENTORY_MONSTER"));
                    }
                }
                return;
            }
            else
            {
                orig(self, desiredItemCount);
            }

        }

        public static void MakeEquipmentIcon(GameObject equipmentIcon, EquipmentIndex equipmentIndex, int stack)
        {
            Debug.Log("Making EquipmentIcon");
            equipmentIcon.GetComponent<ItemIcon>().SetItemIndex(RoR2Content.Items.BoostAttackSpeed.itemIndex, stack);

            EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
  
            equipmentIcon.GetComponent<UnityEngine.UI.RawImage>().texture = equipmentDef.pickupIconTexture;
            TooltipProvider tempTooltip = equipmentIcon.GetComponent<TooltipProvider>();
            equipmentIcon.name = "EquipmentIcon";

            tempTooltip.titleToken = equipmentDef.nameToken;
            tempTooltip.bodyToken = equipmentDef.descriptionToken;
            tempTooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);

            //PickupDef pickupDef = PickupCatalog.FindPickupIndex(equipmentIndex).pickupDef;
            //tempTooltip.titleColor = pickupDef.darkColor;
            tempTooltip.titleColor = ColorCatalog.GetColor(equipmentDef.colorIndex);
        }




    }

}
