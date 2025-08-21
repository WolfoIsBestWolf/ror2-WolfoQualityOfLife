using RoR2;
using RoR2.Stats;
using RoR2.UI;
using System.Collections.Generic;
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
                TooltipProvider tool = destStatStrip.gameObject.GetComponent<TooltipProvider>();
                if (tool == null)
                {
                    tool = destStatStrip.gameObject.AddComponent<TooltipProvider>();
                    tool.titleColor = new Color(0.5339f, 0.4794f, 0.5943f, 1f);
                }
                tool.titleToken = Language.GetString(statDef.displayToken);
                tool.bodyToken = destStatStrip.transform.GetChild(1).GetComponent<HGTextMeshProUGUI>().text;
            
                 
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
                WolfoMain.log.LogMessage(name + " does not exist");
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
            var ruleSS2 = RuleCatalog.FindRuleDef("Expansions.SS2");
            if (ruleSS2 != null)
            {
                SS2 = runReport.ruleBook.GetRuleChoice(ruleSS2).localIndex == 0;
            }
    
           
            bool isLog = runReport.FindFirstPlayerInfo().master == null;
            self.statsToDisplay = new string[] {
                            "totalTimeAlive",
                            IsSimu ? "highestInfiniteTowerWaveReached" : "totalStagesCompleted",

                            //"totalItemsCollected",
                            "highestItemsCollected",
                            "custom_ItemsScrapped",

                            IsSimu ? "null" : "totalDronesPurchased",
                            IsSimu || !SS2 ? "null" : "custom_DronesScrapped",
                           
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
                             "custom_StrongestMinion"
                    };
        }

        public static Transform FindStatStrip(GameEndReportPanelController self, string statName)
        {
            for (int i = 0; i < self.statsToDisplay.Length; i++)
            {
                if (self.statsToDisplay[i] == statName)
                {
                    if (self.statStrips[i])
                    {
                        return self.statStrips[i].transform;
                    }
                    else
                    {
                        WolfoMain.log.LogMessage(statName + " Stat does not exist.");
                        return null;
                    }
                }
            }
            return null;
        }

        public static void AddCustomStats(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo)
        {
            WolfoMain.log.LogMessage("ExtraStats");
            //Transform totalEliteKills = FindStatStrip(self, "totalEliteKills");
            Transform mostKilled = self.statContentArea.Find("custom_MostKilledEnemy");
            Transform mostHurtby = self.statContentArea.Find("custom_EnemyMostHurtBy");
            Transform mvpMinion = self.statContentArea.Find("custom_StrongestMinion");
 
            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();
            if (!extras.madeStats)
            {
                extras.madeStats = true;

                GameObject newMostKilled = GameObject.Instantiate(DeathScreenExpanded.difficulty_stat, mostKilled.parent);
                newMostKilled.transform.SetSiblingIndex(mostKilled.GetSiblingIndex());
                newMostKilled.GetComponent<LayoutElement>().preferredHeight = 50; //56
                newMostKilled.transform.GetChild(3).gameObject.AddComponent<AspectRatioFitter>().aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
                newMostKilled.transform.GetChild(3).gameObject.GetComponent<LayoutElement>().preferredWidth = 42; //48
                newMostKilled.name = mostKilled.name;

                GameObject.Destroy(mostKilled.gameObject);
                mostKilled = newMostKilled.transform;


                GameObject newMostHurtby = GameObject.Instantiate(newMostKilled, mostKilled.parent);
                newMostHurtby.transform.SetSiblingIndex(mostHurtby.GetSiblingIndex());
                newMostHurtby.name = mostHurtby.name;

                GameObject.Destroy(mostHurtby.gameObject);
                mostHurtby = newMostHurtby.transform;

                GameObject newMinionMVP = GameObject.Instantiate(newMostKilled, mostKilled.parent);
                newMinionMVP.transform.SetSiblingIndex(mvpMinion.GetSiblingIndex());
                newMinionMVP.name = mvpMinion.name;

                GameObject.Destroy(mvpMinion.gameObject);
                mvpMinion = newMinionMVP.transform;
            }
            mvpMinion.gameObject.SetActive(false);
            if (playerInfo.statSheet == null)
            {
                mostKilled.gameObject.SetActive(false);
                mostHurtby.gameObject.SetActive(false);     
                return;
            }
            else
            {
                BodyIndex kills = Help.FindBodyWithHighestStatNotMasterless(playerInfo.statSheet, PerBodyStatDef.killsAgainst);
                if (kills != BodyIndex.None)
                {
                    ulong killed = playerInfo.statSheet.fields[PerBodyStatDef.killsAgainst.FindStatDef(kills).index].ulongValue;
                    SetupBodyStat(mostKilled, killed, kills, "STAT_MOST_KILLED", "STAT_AMOUNT_KILLED");
                }
                else
                {
                    mostKilled.gameObject.SetActive(false);
                }

                BodyIndex hurts = Help.FindBodyWithHighestStatNotMasterless(playerInfo.statSheet, PerBodyStatDef.damageTakenFrom);
                if (hurts != BodyIndex.None)
                {
                    ulong damageTaken = playerInfo.statSheet.fields[PerBodyStatDef.damageTakenFrom.FindStatDef(hurts).index].ulongValue;
                    SetupBodyStat(mostHurtby, damageTaken, hurts, "STAT_MOST_HURTBY", "STAT_AMOUNT_HURTBY");
                }
                else
                {
                    mostHurtby.gameObject.SetActive(false);
                }
   
            }

            float damageDealt = playerInfo.statSheet.GetStatValueULong(StatDef.totalDamageDealt);
            float minionDamage = playerInfo.statSheet.GetStatValueULong(StatDef.totalMinionDamageDealt);
            float TotalDamage = damageDealt + minionDamage;

            DamagePercentTooltip(self, "totalDamageDealt", TotalDamage, damageDealt);
            DamagePercentTooltip(self, "totalMinionDamageDealt", TotalDamage, minionDamage);
            DamagePercentTooltip(self, "highestDamageDealt", TotalDamage, playerInfo.statSheet.GetStatValueULong(StatDef.highestDamageDealt));

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

            if (playerTracker.strongestMinion == (BodyIndex)(-2))
            {
                playerTracker.EvaluateStrongestMinion();
            }
            if (playerTracker.strongestMinion != BodyIndex.None)
            {
                SetupBodyStat(mvpMinion, playerTracker.strongestMinionDamage, playerTracker.strongestMinion, "STAT_MOST_MVPDRONE", "STAT_AMOUNT_MVPDRONE");
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

        
            //Custom damage Done
            DamagePercentTooltip(self, "custom_DotDamage", TotalDamage, playerTracker.dotDamageDone);
            DamagePercentTooltip(self, "custom_StrongestMinion", TotalDamage, playerTracker.strongestMinionDamage);

            //Per Run Stats / Only do once ?
            if (!extras.addedRunTrackedStats)
            {
                extras.addedRunTrackedStats = true;
                var runTracker = Run.instance.GetComponent<RunExtraStatTracker>();

                var ChestsMissed = SetupStat(self, "custom_ChestsMissed", "STAT_MISSED_CHEST", runTracker.missedChests);
                var DronesMissed = SetupStat(self, "custom_DronesMissed", "STAT_MISSED_DRONE", runTracker.missedDrones);
                if (ChestsMissed)
                {
                    TooltipProvider toolTip;
                    toolTip = AddDetailedTooltip(ChestsMissed, runTracker.dic_missedChests);
                    if (runTracker.missedShrineChanceItems > 0)
                    {
                        toolTip.bodyToken += GetStatFormatted("STAT_MISSED_SHRINECHANCE", runTracker.missedShrineChanceItems) + "\n";
                    }
                    AddDetailedTooltip(DronesMissed, runTracker.dic_missedDrones);
                }
            }
        }

        public static void DamagePercentTooltip(GameEndReportPanelController self, string lookingFor, float TotalDamage, float thisDamage)
        {
            Transform stat = FindStatStrip(self, lookingFor);
            if (!stat)
            {
                return;
            }
            TooltipProvider tool = stat.GetComponent<TooltipProvider>();
            if (tool == null)
            {
                tool = stat.gameObject.AddComponent<TooltipProvider>();
                tool.titleColor = new Color(0.5339f, 0.4794f, 0.5943f, 1f);
            }
            
            tool.overrideBodyText = tool.bodyToken + "\n"+string.Format(Language.GetString("TOTALDAMAGE_PERCENTTIP"), $"{(thisDamage / TotalDamage) * 100f:F2}");
        }

        public static TooltipProvider AddDetailedTooltip(Transform stat, Dictionary<string, int> data)
        {
            TooltipProvider tip = stat.gameObject.AddComponent<TooltipProvider>();
            tip.titleColor = new Color(0.5339f, 0.4794f, 0.5943f, 1f);
            tip.titleToken = "STAT_DETAILS";
            string bodyToken = string.Empty;
            for (int i = 0; data.Count > i; i++)
            {
                var pair = data.ElementAt(i);
                bodyToken += GetStatFormatted(pair.Key, pair.Value) + "\n";
            }
            tip.bodyToken = bodyToken;
            return tip;
        }

        public static void SetupBodyStat(Transform transform, float value, BodyIndex bodyIndex, string displayToken, string displayToken2)
        {
            if (bodyIndex == BodyIndex.None)
            {
                transform.gameObject.SetActive(false);
                return;
            }
            transform.gameObject.SetActive(true);
            CharacterBody body = BodyCatalog.bodyPrefabBodyComponents[(int)bodyIndex];

            transform.GetChild(0).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString(displayToken), Language.GetString(body.baseNameToken));
            transform.GetChild(2).GetComponent<LanguageTextMeshController>().token = string.Format(Language.GetString(displayToken2), TextSerialization.ToStringNumeric(value));

            Transform icon = transform.GetChild(3);
            if (!icon.gameObject.TryGetComponent<RawImage>(out var raw))
            {
                Object.DestroyImmediate(icon.GetComponent<Image>());
                raw = icon.gameObject.AddComponent<RawImage>();
            };
            raw.texture = body.portraitIcon;
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
                WolfoMain.log.LogMessage(displayToken + " does not exist");
                return null;
            }
            TooltipProvider tool = stat.gameObject.GetComponent<TooltipProvider>();
            if (tool == null)
            {
                tool = stat.gameObject.AddComponent<TooltipProvider>();
                tool.titleColor = new Color(0.5339f, 0.4794f, 0.5943f, 1f);
                tool.titleToken = displayToken;
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
