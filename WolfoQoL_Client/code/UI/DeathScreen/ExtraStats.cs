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
                MakeCombinedStat(statContentArea, "totalDronesPurchased", 2);
                MakeCombinedStat(statContentArea, "totalDamageDealt", 2);
                MakeCombinedStat(statContentArea, "totalKills", 2);
                MakeCombinedStat(statContentArea, "totalDamageTaken");
                MakeCombinedStat(statContentArea, "totalHealthHealed");
                MakeCombinedStat(statContentArea, "custom_DotDamage");
                MakeCombinedStat(statContentArea, "custom_DamageBlocked");
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
            bool IsSimu = runReport.gameMode is InfiniteTowerRun;
            bool DLC1 = runReport.ruleBook.GetRuleChoice(RuleCatalog.FindRuleDef("Expansions.DLC1")).localIndex == 0;
            //bool DLC3 = runReport.ruleBook.GetRuleChoice(RuleCatalog.FindRuleDef("Expansions.DLC3")).localIndex == 0;
            bool DLC3 = false;
            bool SS2 = false;
           
            bool isLog = runReport.FindFirstPlayerInfo().master == null;
            self.statsToDisplay = new string[] {
                            "totalTimeAlive",
                            IsSimu ? "highestInfiniteTowerWaveReached" : "totalStagesCompleted",

                            "totalItemsCollected",
                            "custom_ItemsScrapped",

                            IsSimu ? "null" : "totalDronesPurchased",
                            IsSimu || !DLC3 ? "null" : "custom_DronesScrapped",
                           
                            "totalKills",
                            "totalMinionKills",

                            "totalDamageDealt",
                            "totalMinionDamageDealt",


                            "totalDamageTaken",
                            "custom_MinionDamageTaken",

                            "totalHealthHealed",
                            "custom_MinionHealthHealed",

                            "totalDeaths",
                            "custom_MinionDeaths",



                            IsSimu ? "null" : "custom_ChestsMissed",
                            IsSimu ? "null" : "custom_DronesMissed",

                            "totalPurchases",
                             "totalGoldCollected",

                            isLog ? "totalLunarPurchases" : "custom_LunarCoinsSpent",
                            //DLC1 && !isLog ? "custom_ItemsVoided" : "totalBloodPurchases",
                            "totalBloodPurchases",
                            //"totalEliteKills", //Remove?
   
                            "totalDistanceTraveled",
                            "custom_TimesJumped",


                            "custom_DotDamage",
                            "highestDamageDealt", //Funny big number

                            "custom_DamageBlocked",
                            "highestLevel", //Meh

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
            //Transform totalEliteKills = FindStatStrip(self, "totalEliteKills");
            Transform mostKilled = self.statContentArea.Find("custom_MostKilledEnemy");
            Transform mostHurtby = self.statContentArea.Find("custom_EnemyMostHurtBy");


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
                 
            }

            if (extras.isLogRunReport)
            {
                //All custom stats get auto disabled
                return;
            }

            var playerTracker = playerInfo.master.GetComponent<PerPlayer_ExtraStatTracker>();

            if (playerTracker.latestDetailedDeathMessage != string.Empty)
            {
                string newFinal = self.finalMessageLabel.text + "\n" + playerTracker.latestDetailedDeathMessage;
                self.finalMessageLabel.SetText(newFinal, true);
            }


            //Transform totalDronesPurchased = FindStatStrip(self, "totalDronesPurchased");
            //Transform totalTurretsPurchased = FindStatStrip(self, "totalTurretsPurchased");

            int visibleItems = 0;
            for (int i = 0; i < playerInfo.itemStacks.Length; i++)
            {
                if (ItemInventoryDisplay.ItemIsVisible((ItemIndex)i))
                {
                    visibleItems += playerInfo.itemStacks[i];
                }
            }
        

            // SetupStat(self, "totalItemsCollected", "STATNAME_TOTALITEMSCOLLECTED", visibleItems);
 
            SetupStat(self, "custom_MinionDamageTaken", "STAT_MINION_DAMAGETAKEN", (int)playerTracker.minionDamageTaken);
            SetupStat(self, "custom_MinionHealthHealed", "STAT_MINION_HEALTHHEALED", (int)playerTracker.minionHealing);
            SetupStat(self, "custom_MinionDeaths", "STAT_MINION_DEATH", playerTracker.minionDeaths);

            SetupStat(self, "custom_ItemsScrapped", "STAT_SCRAPPED_ITEMS", playerTracker.scrappedItems);
            var dronesScrapped = SetupStat(self, "custom_DronesScrapped", "STAT_SCRAPPED_DRONES", playerTracker.scrappedDrones);
            if (dronesScrapped)
            {
                dronesScrapped.gameObject.SetActive(false); //Figure out how to SS2
            }
            SetupStat(self, "custom_TimesJumped", "STAT_TIMESJUMPED", playerTracker.timesJumped, true);
            SetupStat(self, "custom_LunarCoinsSpent", "STAT_SPENT_LUNARCOIN", playerTracker.spentLunarCoins);
            //SetupStat(self, "custom_ItemsVoided", "STAT_SPENT_VOIDSTUFF", playerTracker.itemsVoided);

            SetupStat(self, "custom_DotDamage", "STAT_DAMAGE_DOT", (int)playerTracker.dotDamageDone);
            Transform damageBlocked = SetupStat(self, "custom_DamageBlocked", "STAT_DAMAGE_BLOCKED", (int)playerTracker.damageBlocked, true);
            if (playerTracker.damageBlocked == -1)
            {
                damageBlocked.gameObject.SetActive(false);
            }

            if (extras.addedRunTrackedStats)
            {
                return;
            }
            extras.addedRunTrackedStats = true;
            var runTracker = Run.instance.GetComponent<RunExtraStatTracker>();

            var ChestsMissed = SetupStat(self, "custom_ChestsMissed", "STAT_MISSED_CHEST", runTracker.missedChests);
            var DronesMissed = SetupStat(self, "custom_DronesMissed", "STAT_MISSED_DRONE", runTracker.missedDrones);
            if (ChestsMissed)
            {
               
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

        public static Transform SetupStat(GameEndReportPanelController self, string lookingFor, string displayToken, int value, bool disableIfNeg = false)
        {
            Transform stat = FindStatStrip(self, lookingFor);
            if (!stat)
            {
                Debug.Log(displayToken + " does not exist");
                return null;
            }
         
            string formatted = TextSerialization.ToStringNumeric(value);
            if (disableIfNeg && value == -1)
            {
                self.gameObject.SetActive(false);
            }
            else
            {
                stat.gameObject.SetActive(true);
            }
      
            stat.GetChild(0).GetComponent<TextMeshProUGUI>().text = string.Format(Language.GetString("STAT_NAME_VALUE_FORMAT"), Language.GetString(displayToken), formatted);
            stat.GetChild(1).gameObject.SetActive(false);
            return stat;
        }


    }



}
