using MonoMod.Cil;
using RoR2;
using RoR2.Stats;
using RoR2.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mono.Cecil.Cil;

namespace WolfoQoL_Client.DeathScreen
{
 
    public class ExtraStats
    {
        public static void PointsToolTip(On.RoR2.UI.GameEndReportPanelController.orig_AssignStatToStrip orig, GameEndReportPanelController self, StatSheet srcStatSheet, StatDef statDef, GameObject destStatStrip)
        {
            orig(self, srcStatSheet, statDef, destStatStrip);
            if (statDef.pointValue == 0.0)
            {
                destStatStrip.transform.Find("PointValueLabel").gameObject.SetActive(false);
            }
            else
            {
                if (destStatStrip.gameObject.GetComponent<TooltipProvider>() == null)
                {
                    TooltipProvider tool = destStatStrip.gameObject.AddComponent<TooltipProvider>();
                    tool.titleToken = Language.GetString(statDef.displayToken);
                    tool.bodyToken = destStatStrip.transform.GetChild(1).GetComponent<HGTextMeshProUGUI>().text;
                    tool.titleColor = new Color(0.5339f, 0.4794f, 0.5943f, 1f);
                }
            }
        }

        public static void CombineStats(On.RoR2.UI.GameEndReportPanelController.orig_AllocateUnlockStrips orig, GameEndReportPanelController self, int count)
        {
            orig(self, count);
            for (int j = 0; j < self.statStrips.Count; j++)
            {
                if (self.statStrips[j] != null)
                {
                    self.statStrips[j].name = self.statsToDisplay[j];
                    if (StatDef.Find(self.statsToDisplay[j]) == null)
                    {
                        self.statStrips[j].SetActive(false);
                    }
                }
            }
            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();
            if (WConfig.DC_CompactStats.Value && extras.compactedStats == false)
            {
                extras.compactedStats = true;
                RectTransform statContentArea = self.statContentArea;
                MakeCombinedStat(statContentArea, "totalTimeAlive"); //+Stages / Waves
                MakeCombinedStat(statContentArea, "totalItemsCollected"); //Items gained / Items Scrapped
                MakeCombinedStat(statContentArea, "totalDronesPurchased"); //Items gained / Items Scrapped
                MakeCombinedStat(statContentArea, "totalKills", 2); //Kills / Minion Kills
                MakeCombinedStat(statContentArea, "totalDamageTaken"); //Damage Taken / Minion Damage
                MakeCombinedStat(statContentArea, "totalDeaths"); //Damage Taken / Damage Healed
                MakeCombinedStat(statContentArea, "totalPurchases", 2); //Purchases / Lunar Coins / Blood
                MakeCombinedStat(statContentArea, "custom_ChestsMissed", 2);
                MakeCombinedStat(statContentArea, "totalLunarPurchases", 2);
                MakeCombinedStat(statContentArea, "custom_LunarCoinsSpent", 2);  
                MakeCombinedStat(statContentArea, "totalDistanceTraveled", 2);
            }

        }

        public static void MakeCombinedStat(RectTransform statContentArea, string name, int combine = 2)
        {
            Transform stat = statContentArea.Find(name);
            if (stat == null)
            {
                Debug.Log(name + " does not exist");
                return;
            }
            int childIndex = stat.GetSiblingIndex();
            string newName = string.Empty;

            GameObject statHolder = GameObject.Instantiate(DeathScreenExpanded.statHolderPrefab, statContentArea);
            for (int i = 0; i < combine; i++)
            {
                Transform child = statContentArea.GetChild(childIndex);
                child.SetParent(statHolder.transform); //Goes down by 1, hopefully just consistent??
                child.GetChild(1).gameObject.SetActive(false);
                newName += "+" + child.name;
            }
            statHolder.transform.SetSiblingIndex(childIndex);
            statHolder.name = newName;
        }
 
        public static void DifficultyTooltip(GameEndReportPanelController self)
        {
            //Difficulty tooltip
            DifficultyIndex difficultyIndex = DifficultyIndex.Invalid;
            if (self.displayData.runReport != null)
            {
                difficultyIndex = self.displayData.runReport.ruleBook.FindDifficulty();
            }

            DifficultyDef difficultyDef = DifficultyCatalog.GetDifficultyDef(difficultyIndex);
            self.selectedDifficultyImage.raycastTarget = true;//? doesn't work without?
            TooltipProvider difficulty = self.selectedDifficultyImage.gameObject.AddComponent<TooltipProvider>();
            difficulty.titleToken = difficultyDef.nameToken;
            difficulty.bodyToken = difficultyDef.descriptionToken;
            difficulty.titleColor = difficultyDef.color;


            Transform StatNameLabel = self.selectedDifficultyImage.transform.parent.Find("StatNameLabel");
            Transform DifficultyLabel = self.selectedDifficultyImage.transform.parent.Find("SelectedDifficultyLabel");
            DifficultyLabel.transform.SetSiblingIndex(1);
            StatNameLabel.GetComponent<LanguageTextMeshController>().token = "RULE_HEADER_DIFFICULTY_WITH_COLON";
            self.selectedDifficultyLabel = DifficultyLabel.GetComponent<LanguageTextMeshController>();
            self.selectedDifficultyLabel.token = difficultyDef.nameToken;

        }

        public static void ChangeStats(GameEndReportPanelController self, RunReport runReport)
        {
            bool simu = runReport.gameMode is InfiniteTowerRun;
            bool isLog = runReport.FindFirstPlayerInfo().master == null;
            self.statsToDisplay = new string[] {
                            "totalTimeAlive",
                            simu ? "highestInfiniteTowerWaveReached" : "totalStagesCompleted",

                            "totalItemsCollected",
                            "custom_ItemsScrapped",

                            simu ? "null" : "totalDronesPurchased",
                            simu ? "null" : "custom_DronesScrapped",

                            "totalKills",
                            "totalMinionKills",

                            "totalDamageDealt",
                            "totalMinionDamageDealt",
                            "highestDamageDealt", //Funny big number should be kept tho
                           
                            "totalDamageTaken",
                            "custom_MinionDamageTaken",

                            "totalDeaths",
                            "totalHealthHealed",
 

                            simu ? "null" : "custom_ChestsMissed",
                            simu ? "null" : "custom_DronesMissed",

                            "totalPurchases",
                            "totalGoldCollected",

                            isLog ? "totalLunarPurchases" : "custom_LunarCoinsSpent",
                            "totalBloodPurchases",

                            "totalEliteKills", //Remove?
                            "highestLevel", //Meh

                            "totalDistanceTraveled",
                            "custom_TimesJumped",

                            "custom_MostKilledEnemy",
                            "custom_EnemyMostHurtBy",
                            "custom_HighestStackedItem"
                    };
        }

        public static Transform FindStatStrip(GameEndReportPanelController self, string statName)
        {
            for (int i = 0; i < self.statsToDisplay.Length; i++)
            {
                if (self.statsToDisplay[i] == statName)
                {
                    return self.statStrips[i].transform;
                }
            }
            return null;
        }

        public static void AddCustomStats(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo)
        {
            Debug.Log("ExtraStats");
            Transform totalEliteKills = FindStatStrip(self, "totalEliteKills");
            Transform mostKilled = self.statContentArea.Find("custom_MostKilledEnemy");
            Transform mostHurtby = self.statContentArea.Find("custom_EnemyMostHurtBy");

            Transform missedInteractables = FindStatStrip(self, "custom_ChestsMissed");
            Transform itemsScrapped = FindStatStrip(self, "custom_ItemsScrapped");

            Transform dronesScrapped = FindStatStrip(self, "custom_DronesScrapped");
            Transform dronesMissed = FindStatStrip(self, "custom_DronesMissed");
            Transform LunarCoinsSpent = FindStatStrip(self, "custom_LunarCoinsSpent");

            Transform custom_TimesJumped = FindStatStrip(self, "custom_TimesJumped");
            Transform custom_MinionDamageTaken = FindStatStrip(self, "custom_MinionDamageTaken");

            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();
            if (!extras.madeStats)
            {
                extras.madeStats = true;

                GameObject newMostKilled = GameObject.Instantiate(DeathScreenExpanded.difficulty_stat, mostKilled.parent);
                newMostKilled.transform.SetSiblingIndex(mostKilled.GetSiblingIndex());
                newMostKilled.GetComponent<LayoutElement>().preferredHeight = 56;
                newMostKilled.transform.GetChild(3).gameObject.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                newMostKilled.name = mostKilled.name;

                GameObject.Destroy(mostKilled.gameObject);
                mostKilled = newMostKilled.transform;


                GameObject newMostHurtby = GameObject.Instantiate(newMostKilled, mostKilled.parent);
                newMostHurtby.transform.SetSiblingIndex(mostHurtby.GetSiblingIndex());
                newMostHurtby.name = mostHurtby.name;

                GameObject.Destroy(mostHurtby.gameObject);
                mostHurtby = newMostHurtby.transform;
            }

           
            if (playerInfo.statSheet == null)
            {
                mostKilled.gameObject.SetActive(false);
                mostHurtby.gameObject.SetActive(false);
                return;
            }
            else
            {
              
                string eliteKills = playerInfo.statSheet.GetStatDisplayValue(StatDef.totalEliteKills);
                totalEliteKills.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format(Language.GetString("MONSTER_PREFIX_ELITESKILLED").Replace("style=cEvent", "color=#FFFF7F").Replace("/style", "/color"), eliteKills);

                Transform icon;
                CharacterBody body = null;
                BodyIndex kills = playerInfo.statSheet.FindBodyWithHighestStat(PerBodyStatDef.killsAgainst);
                BodyIndex hurts = playerInfo.statSheet.FindBodyWithHighestStat(PerBodyStatDef.damageTakenFrom);
                ItemIndex item = Help.FindItemWithHighestStat(playerInfo.statSheet, PerItemStatDef.totalCollected);
                EquipmentIndex equip = playerInfo.statSheet.FindEquipmentWithHighestStat(PerEquipmentStatDef.totalTimesFired);


                if (kills != BodyIndex.None)
                {
                    mostKilled.gameObject.SetActive(true);
                    body = BodyCatalog.bodyPrefabBodyComponents[(int)kills];
                    ulong killed = playerInfo.statSheet.fields[PerBodyStatDef.killsAgainst.FindStatDef(kills).index].ulongValue;

                    mostKilled.GetChild(0).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString("STAT_MOSTKILLED"), Language.GetString(body.baseNameToken));
                    mostKilled.GetChild(2).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString("STAT_MOSTKILLED_AMOUNT"), killed);

                    icon = mostKilled.GetChild(3);
                    if (!icon.gameObject.TryGetComponent<RawImage>(out var raw))
                    {
                        Object.DestroyImmediate(icon.GetComponent<Image>());
                        raw = icon.gameObject.AddComponent<RawImage>();
                    };
                    raw.texture = body.portraitIcon;
                }
                else
                {
                    mostKilled.gameObject.SetActive(false);                 
                }

                if (hurts != BodyIndex.None)
                {
                    mostHurtby.gameObject.SetActive(true);
                    body = BodyCatalog.bodyPrefabBodyComponents[(int)hurts];
                    ulong damageTaken = playerInfo.statSheet.fields[PerBodyStatDef.damageTakenFrom.FindStatDef(hurts).index].ulongValue;

                    mostHurtby.GetChild(0).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString("STAT_MOSTHURTBY"), Language.GetString(body.baseNameToken));
                    mostHurtby.GetChild(2).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString("STAT_MOSTHURTBY_AMOUNT"), damageTaken);

                    icon = mostHurtby.GetChild(3);
                    if (!icon.gameObject.TryGetComponent<RawImage>(out var raw))
                    {
                        Object.DestroyImmediate(icon.GetComponent<Image>());
                        raw = icon.gameObject.AddComponent<RawImage>();
                    };
                    raw.texture = body.portraitIcon;
                }
                else
                {
                    mostHurtby.gameObject.SetActive(false);
                }
                
                if (item != ItemIndex.None)
                {
                    ItemDef itemDef = ItemCatalog.GetItemDef(item);
                    ulong total = playerInfo.statSheet.fields[PerItemStatDef.totalCollected.FindStatDef(item).index].ulongValue;

                    Debug.LogFormat("Most gotten item: {0} | {1}", new object[]
                    {
                       Language.GetString(itemDef.nameToken),
                        total
                    });
                }
                else
                {

                }

                if (equip != EquipmentIndex.None)
                {
                    EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(equip);
                    ulong total = playerInfo.statSheet.fields[PerEquipmentStatDef.totalTimesFired.FindStatDef(equip).index].ulongValue;

                    Debug.LogFormat("Most used equip: {0} | {1}", new object[]
                    {
                       Language.GetString(equipmentDef.nameToken),
                        total
                    });
                }
                else
                {

                }
            }

            if (extras.isLogRunReport)
            {
                itemsScrapped.gameObject.SetActive(false);
                missedInteractables.gameObject.SetActive(false);
                custom_TimesJumped.gameObject.SetActive(false);
                custom_MinionDamageTaken.gameObject.SetActive(false);
                //Check Simu/DLC
                if (dronesScrapped)
                {
                    dronesScrapped.gameObject.SetActive(false);
                }
                if (dronesMissed)
                {
                    dronesMissed.gameObject.SetActive(false);
                }            
                return;
            }

            var playerTracker = playerInfo.master.GetComponent<PerPlayer_ExtraStatTracker>();
            var runTracker = Run.instance.GetComponent<RunExtraStatTracker>();

            if (playerTracker.latestDetailedDeathMessage != string.Empty) 
            {
                string newFinal = self.finalMessageLabel.text + "\n" + playerTracker.latestDetailedDeathMessage;
                self.finalMessageLabel.SetText(newFinal, true);
            }
   
 
            itemsScrapped.gameObject.SetActive(true);
            itemsScrapped.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_SCRAPPED_ITEMS"), playerTracker.scrappedItems);
            itemsScrapped.GetChild(1).gameObject.SetActive(false);

            missedInteractables.gameObject.SetActive(true);
            missedInteractables.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_MISSED_ITEMS"), runTracker.missedChests);
            missedInteractables.GetChild(1).gameObject.SetActive(false);

            TooltipProvider tip = missedInteractables.gameObject.AddComponent<TooltipProvider>();
            tip.titleColor =new Color(0.5339f, 0.4794f, 0.5943f, 1f);
            tip.titleToken = "STAT_EXTRAHEADER";
            tip.bodyToken = string.Format(Language.GetString("STAT_MISSED_SHRINECHANCE"), runTracker.missedShrineChanceItems);
    
            if (LunarCoinsSpent)
            {
                LunarCoinsSpent.gameObject.SetActive(true);
                LunarCoinsSpent.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_SPENT_LUNARCOIN"), playerTracker.spentLunarCoins);
                LunarCoinsSpent.GetChild(1).gameObject.SetActive(false);
            }


            if (dronesScrapped)
            {
                dronesScrapped.gameObject.SetActive(false); //IF REAL CHANGE ELSE UHHH idk ss2?
                dronesScrapped.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_SCRAPPED_DRONES"), playerTracker.scrappedDrones);
                dronesScrapped.GetChild(1).gameObject.SetActive(false);
            }
            if (dronesMissed)
            {
                dronesMissed.gameObject.SetActive(true);
                dronesMissed.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_MISSED_DRONE"), runTracker.missedDrones);
                dronesMissed.GetChild(1).gameObject.SetActive(false);
            }

            if (custom_MinionDamageTaken)
            {
                custom_MinionDamageTaken.gameObject.SetActive(true);
                custom_MinionDamageTaken.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_DAMAGETAKENBYMINIONS"), (int)playerTracker.minionDamageTaken);
                custom_MinionDamageTaken.GetChild(1).gameObject.SetActive(false);
            }
            if (custom_TimesJumped)
            {
                if (playerTracker.timesJumped == -1)
                {
                    custom_TimesJumped.gameObject.SetActive(false);
                }
                else
                {
                    custom_TimesJumped.gameObject.SetActive(true);
                    custom_TimesJumped.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_TIMESJUMPED"), playerTracker.timesJumped);
                    custom_TimesJumped.GetChild(1).gameObject.SetActive(false);
                }            
            }




        }




    }



}
