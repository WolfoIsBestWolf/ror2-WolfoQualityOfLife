using RoR2;
using RoR2.Stats;
using RoR2.UI;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
                MakeCombinedStat(statContentArea, "totalItemsCollected");
                MakeCombinedStat(statContentArea, "totalDronesPurchased", 3);
                MakeCombinedStat(statContentArea, "totalDamageDealt", 2);
                MakeCombinedStat(statContentArea, "totalKills", 2);
                MakeCombinedStat(statContentArea, "totalDamageTaken");
                MakeCombinedStat(statContentArea, "totalHealthHealed");
                MakeCombinedStat(statContentArea, "totalDeaths");
                MakeCombinedStat(statContentArea, "totalPurchases", 2);
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
        public static void CombineDifficultyLoadout(RectTransform statContentArea)
        {
            string newName = string.Empty;
            GameObject statHolder = GameObject.Instantiate(DeathScreenExpanded.statHolderPrefab, statContentArea);
            for (int i = 0; i < 2; i++)
            {
                Transform child = statContentArea.GetChild(0);
                child.SetParent(statHolder.transform); //Goes down by 1, hopefully just consistent??
                newName += "+" + child.name;
            }
            statHolder.transform.SetSiblingIndex(0);
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


            Transform difficultyStrip = self.selectedDifficultyImage.transform.parent;

            difficultyStrip.GetComponent<LayoutElement>().preferredWidth = 300;
            Transform StatNameLabel = difficultyStrip.Find("StatNameLabel");
            Transform DifficultyLabel = difficultyStrip.Find("SelectedDifficultyLabel");
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
                            simu ? "null" : "totalTurretsPurchased",

                            "totalKills",
                            "totalMinionKills",

                            "totalDamageDealt",
                            "totalMinionDamageDealt",

                            "highestDamageDealt", //Funny big number

                            "totalDamageTaken",
                            "custom_MinionDamageTaken",

                            "totalHealthHealed",
                            "custom_MinionHealthHealed",

                             "totalDeaths",
                            "custom_MinionDeaths",

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
                            //"custom_HighestStackedItem" //This would almost always be WhiteScrap
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


            Transform totalDronesPurchased = FindStatStrip(self, "totalDronesPurchased");
            Transform totalTurretsPurchased = FindStatStrip(self, "totalTurretsPurchased");


            Transform ChestsMissed = FindStatStrip(self, "custom_ChestsMissed");
            Transform DronesMissed = FindStatStrip(self, "custom_DronesMissed");

            Transform itemsScrapped = FindStatStrip(self, "custom_ItemsScrapped");
            Transform dronesScrapped = FindStatStrip(self, "custom_DronesScrapped");
            if (dronesScrapped)
            {
                //Figure out how to SS2 
                dronesScrapped.gameObject.SetActive(false);
            }
            Transform LunarCoinsSpent = FindStatStrip(self, "custom_LunarCoinsSpent");

            Transform custom_TimesJumped = FindStatStrip(self, "custom_TimesJumped");

            Transform custom_MinionDamageTaken = FindStatStrip(self, "custom_MinionDamageTaken");
            Transform custom_MinionHealthHealed = FindStatStrip(self, "custom_MinionHealthHealed");
            Transform custom_MinionDeaths = FindStatStrip(self, "custom_MinionDeaths");

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
                if (dronesScrapped && dronesScrapped.gameObject.activeSelf && totalTurretsPurchased)
                {
                    totalTurretsPurchased.gameObject.SetActive(false);
                    if (!totalDronesPurchased.TryGetComponent<TooltipProvider>(out var tooltipProvider))
                    {
                        tooltipProvider = totalDronesPurchased.gameObject.AddComponent<TooltipProvider>();
                        tooltipProvider.titleColor = new Color(0.5339f, 0.4794f, 0.5943f, 1f);
                    }
                    tooltipProvider.bodyToken = totalTurretsPurchased.GetChild(0).GetComponent<HGTextMeshProUGUI>().text;
                    tooltipProvider.titleToken = "STAT_EXTRAHEADER";
                }

                //string eliteKills = playerInfo.statSheet.GetStatDisplayValue(StatDef.totalEliteKills);
                //totalEliteKills.transform.GetChild(0).GetComponent<TMP_Text>().text = string.Format(Language.GetString("MONSTER_PREFIX_ELITESKILLED").Replace("style=cEvent", "color=#FFFF7F").Replace("/style", "/color"), eliteKills);

                Transform icon;
                CharacterBody body = null;
                BodyIndex kills = Help.FindBodyWithHighestStatNotMasterless(playerInfo.statSheet, PerBodyStatDef.killsAgainst);
                BodyIndex hurts = Help.FindBodyWithHighestStatNotMasterless(playerInfo.statSheet, PerBodyStatDef.damageTakenFrom);
                ItemIndex item = Help.FindItemWithHighestStat(playerInfo.statSheet, PerItemStatDef.totalCollected);
                EquipmentIndex equip = playerInfo.statSheet.FindEquipmentWithHighestStat(PerEquipmentStatDef.totalTimesFired);


                if (kills != BodyIndex.None)
                {
                    mostKilled.gameObject.SetActive(true);
                    body = BodyCatalog.bodyPrefabBodyComponents[(int)kills];
                    ulong killed = playerInfo.statSheet.fields[PerBodyStatDef.killsAgainst.FindStatDef(kills).index].ulongValue;

                    mostKilled.GetChild(0).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString("STAT_MOST_KILLED"), Language.GetString(body.baseNameToken));
                    mostKilled.GetChild(2).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString("STAT_AMOUNT_KILLED"), killed);

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

                    mostHurtby.GetChild(0).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString("STAT_MOST_HURTBY"), Language.GetString(body.baseNameToken));
                    mostHurtby.GetChild(2).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString("STAT_AMOUNT_HURTBY"), damageTaken);

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
                /*itemsScrapped.gameObject.SetActive(false);

                custom_TimesJumped.gameObject.SetActive(false);
                custom_MinionDamageTaken.gameObject.SetActive(false);
                custom_MinionHealthHealed.gameObject.SetActive(false);
                custom_MinionDeaths.gameObject.SetActive(false);

                //Check Simu/SS2/DLC?
                if (dronesScrapped)
                {
                    dronesScrapped.gameObject.SetActive(false);
                }
                if (DronesMissed)
                {
                    ChestsMissed.gameObject.SetActive(false);
                    DronesMissed.gameObject.SetActive(false);
                }*/
                return;
            }

            var playerTracker = playerInfo.master.GetComponent<PerPlayer_ExtraStatTracker>();

            if (playerTracker.latestDetailedDeathMessage != string.Empty)
            {
                string newFinal = self.finalMessageLabel.text + "\n" + playerTracker.latestDetailedDeathMessage;
                self.finalMessageLabel.SetText(newFinal, true);
            }

            SetupStat(custom_MinionDamageTaken, "STAT_MINION_DAMAGETAKEN", (int)playerTracker.minionDamageTaken);
            SetupStat(custom_MinionHealthHealed, "STAT_MINION_HEALTHHEALED", (int)playerTracker.minionHealing);
            SetupStat(custom_MinionDeaths, "STAT_MINION_DEATH", playerTracker.minionDeaths);

            SetupStat(itemsScrapped, "STAT_SCRAPPED_ITEMS", playerTracker.scrappedItems);
            SetupStat(dronesScrapped, "STAT_SCRAPPED_DRONES", playerTracker.scrappedDrones);
      
            SetupStat(custom_TimesJumped, "STAT_TIMESJUMPED", playerTracker.timesJumped);
            SetupStat(LunarCoinsSpent, "STAT_SPENT_LUNARCOIN", playerTracker.spentLunarCoins);

            if (extras.addedRunTrackedStats)
            {
                return;
            }
            extras.addedRunTrackedStats = true;
            var runTracker = Run.instance.GetComponent<RunExtraStatTracker>();

            if (ChestsMissed)
            {
                SetupStat(ChestsMissed, "STAT_MISSED_CHEST", runTracker.missedChests);
                SetupStat(DronesMissed, "STAT_MISSED_DRONE", runTracker.missedDrones);

                TooltipProvider tip = ChestsMissed.gameObject.AddComponent<TooltipProvider>();
                tip.titleColor = new Color(0.5339f, 0.4794f, 0.5943f, 1f);
                tip.titleToken = "STAT_DETAILS";
                string bodyToken = string.Empty;
                if (runTracker.missedShrineChanceItems > 0)
                {
                    bodyToken += GetStatFormatted("STAT_MISSED_SHRINECHANCE", runTracker.missedShrineChanceItems) + "\n";
                }
                for (int i = 0; runTracker.dic_missedChests.Count > i; i++)
                {
                    var pair = runTracker.dic_missedChests.ElementAt(i);
                    bodyToken += GetStatFormatted(pair.Key, pair.Value) + "\n";
                }
                tip.bodyToken = bodyToken;

                tip = DronesMissed.gameObject.AddComponent<TooltipProvider>();
                tip.titleColor = new Color(0.5339f, 0.4794f, 0.5943f, 1f);
                tip.titleToken = "STAT_DETAILS";
                bodyToken = string.Empty;
                for (int i = 0; runTracker.dic_missedDrones.Count > i; i++)
                {
                    var pair = runTracker.dic_missedDrones.ElementAt(i);
                    bodyToken += GetStatFormatted(pair.Key, pair.Value) + "\n";
                }
                tip.bodyToken = bodyToken;
            }

        }



        public static string GetStatFormatted(string displayToken, object value)
        {
            return string.Format(Language.GetString("STAT_NAME_VALUE_FORMAT"), Language.GetString(displayToken), value);
        }

        public static void SetupStat(Transform stat, string displayToken, int value)
        {
            if (!stat)
            {
                Debug.Log(displayToken + " does not exist");
                return;
            }

            string formatted = TextSerialization.ToStringNumeric(value);
            stat.gameObject.SetActive(true);
            stat.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_NAME_VALUE_FORMAT"), Language.GetString(displayToken), formatted);
            stat.GetChild(1).gameObject.SetActive(false);
        }


    }



}
