using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Stats;
using RoR2.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace WolfoQoL_Client.DeathScreen
{

    public class DeathScreen_Main
    {

        public static bool otherMod = false;

        public static void Start()
        {
            otherMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("local.fix.history");

            MakePrefabs();

            On.RoR2.UI.GameEndReportPanelController.SetDisplayData += OneTimeExtras;
            On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += AddManyExtras;

            RunRecap.Start();
            ExtraStatsTracking.Start();
            On.RoR2.UI.ItemInventoryDisplay.AllocateIcons += EquipOnDeathInventory.AddEquipmentIcons;

            On.RoR2.UI.GameEndReportPanelController.AssignStatToStrip += ExtraStats.PointsToolTip;
            On.RoR2.UI.GameEndReportPanelController.AllocateUnlockStrips += ExtraStats.CombineStats;


            StatDef.totalEliteKills.pointValue = 20.0;
            Inventory_Minions.Hooks();

            IL.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += GameEndReportPanelController_SetPlayerInfo;
        }

        private static void GameEndReportPanelController_SetPlayerInfo(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdstr("GameEndReportPanelController.SetStatSheet: Could not find stat def \"{0}\".")
                ))
            {
                c.RemoveRange(8);
            }
            else
            {
                WQoLMain.log.LogWarning("IL Failed: MinionDamageTakenStat");
            }
        }

        public static ChildLocator AddChildLocator(GameEndReportPanelController self)
        {
            Transform SafeArea = self.transform.GetChild(0);
            Transform BodyArea = SafeArea.GetChild(1);
            Transform StatsAndChatArea = BodyArea.Find("StatsAndChatArea");
            Transform RightArea = BodyArea.Find("RightArea");

            ChildLocator childLocator = self.gameObject.AddComponent<ChildLocator>();
            childLocator.transformPairs = new ChildLocator.NameTransformPair[]
            {
                new ChildLocator.NameTransformPair
                {
                      name = "HeaderArea",
                      transform = SafeArea.GetChild(0)
                },
                new ChildLocator.NameTransformPair
                {
                      name = "BodyArea",
                      transform = SafeArea.GetChild(1)
                },
                new ChildLocator.NameTransformPair
                {
                      name = "StatsAndChatArea",
                      transform = StatsAndChatArea
                },
                new ChildLocator.NameTransformPair
                {
                      name = "StatsContainer",
                      transform = StatsAndChatArea.GetChild(0)
                },
                new ChildLocator.NameTransformPair
                {
                      name = "ChatArea",
                      transform = StatsAndChatArea.GetChild(1)
                },
                new ChildLocator.NameTransformPair
                {
                      name = "RightArea",
                      transform = RightArea
                },
                new ChildLocator.NameTransformPair
                {
                      name = "InfoArea",
                      transform = RightArea.GetChild(0)
                }

            };
            return childLocator;
        }
        public static void SizeChanges(GameEndReportPanelController self)
        {
            //Squish together "Class / Killer" Zone
            /*Transform InfoArea = self.transform.Find("SafeArea (JUICED)/BodyArea/RightArea/InfoArea");
            InfoArea.GetComponent<VerticalLayoutGroup>().spacing = 0;
            InfoArea.GetChild(3).GetComponent<VerticalLayoutGroup>().spacing = 8;*/
        }
        public static void GeneralQuality(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo, bool log)
        {
            //Dont just remove the spacer because it looks too bunched together
            GameObject killerArea = self.killerPanelObject;
            if (killerArea != null)
            {
                killerArea.SetActive(true);
                killerArea.transform.GetChild(0).gameObject.SetActive(playerInfo.isDead);
                killerArea.transform.GetChild(1).gameObject.SetActive(playerInfo.isDead);
            }

            //Hide Unlock box if nothing was unlocked
            bool hasAnyUnlocks = self.unlockStrips.Count > 0;
            if (log)
            {
                Transform unlockArea = self.unlockContentArea.parent.parent;
                unlockArea.gameObject.SetActive(hasAnyUnlocks);
            }
            else
            {
                RectTransform unlockArea = (RectTransform)self.unlockContentArea.parent.parent.parent;
                unlockArea.gameObject.SetActive(hasAnyUnlocks);

                //Scale to fit 1/2/3/4/Vanilla
                unlockArea.GetComponent<LayoutElement>().flexibleHeight = Mathf.Min(0.11f * Mathf.Pow(2, self.unlockStrips.Count), 1);
            }

        }


        private static void AddManyExtras(On.RoR2.UI.GameEndReportPanelController.orig_SetPlayerInfo orig, GameEndReportPanelController self, RunReport.PlayerInfo playerInfo, int playerIndex)
        {
            EquipOnDeathInventory.DeathEquip_Enemy1 = EquipmentIndex.None;
            if (playerInfo == null)
            {
                EquipOnDeathInventory.DeathEquips_Player = System.Array.Empty<EquipmentIndex>();
                orig(self, playerInfo, playerIndex);
                return;
            }
            EquipOnDeathInventory.DeathEquips_Player = playerInfo.equipment;

            orig(self, playerInfo, playerIndex);

            var extras = self.GetComponent<DeathScreenExpanded>();
            try
            {
                if (playerInfo.master)
                {
                    playerInfo.master.inventory.tempItemsStorage.SetDecayDurationServer(999999);
                    self.itemInventoryDisplay.SetSubscribedInventory(playerInfo.master.inventory);
                }
                else
                {
                    self.itemInventoryDisplay.SetSubscribedInventory(null);
                }

                GeneralQuality(self, playerInfo, extras.isLogRunReport);
                LoadoutStat.Add_Loadout(self, playerInfo);
                RunRecap.AddRunRecapV2(self, playerInfo);
                Inventory_Minions.AddMinionInventory(self, playerInfo);
                Inventory_Killer.AddKillerInventory(self, playerInfo);
                ExtraStats.AddCustomStats(self, playerInfo);



                if (self.finalMessageLabel)
                {
                    self.finalMessageLabel.fontSizeMin = self.finalMessageLabel.fontSizeMax;
                }
                if (!extras.isLogRunReport)
                {
                    bool eitherActive = extras.killerInventory.activeSelf || extras.minionInventory.activeSelf;
                    extras.bonusInventoyHolder.SetActive(eitherActive);
                }

            }
            catch (System.Exception ex)
            {
                //It's the death screen, it can afford it
                Debug.LogException(ex);
            }
        }

        public static void MakePrefabs()
        {
            GameObject GameEndReportPanel = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/GameEndReportPanel.prefab").WaitForCompletion();
            GameEndReportPanelController game = GameEndReportPanel.GetComponent<GameEndReportPanelController>();

            DeathScreenExpanded.difficulty_stat = game.statContentArea.GetChild(0).gameObject;

            DeathScreenExpanded.RightAreaPrefab = game.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
            DeathScreenExpanded.unlockAreaPrefab = game.unlockContentArea.parent.parent.parent.gameObject;
            DeathScreenExpanded.itemAreaPrefab = DeathScreenExpanded.unlockAreaPrefab.transform.parent.GetChild(2).gameObject;
            DeathScreenExpanded.scrollBoxPrefab = DeathScreenExpanded.unlockAreaPrefab.transform.GetChild(1).gameObject;
            #region MutliStat Holder
            GameObject statHolder = new GameObject("StatHolder");
            statHolder.AddComponent<RectTransform>();
            statHolder.AddComponent<LayoutElement>();
            statHolder.AddComponent<HorizontalLayoutGroup>();
            DeathScreenExpanded.statHolderPrefab = PrefabAPI.InstantiateClone(statHolder, "StatHolder", false);
            #endregion

            #region WaveStrip Strip
            GameObject waveStrip = PrefabAPI.InstantiateClone(DeathScreenExpanded.difficulty_stat, "latestWaveStrip", false);
            DeathScreenExpanded.waveStripPrefab = waveStrip;

            waveStrip.GetComponent<HorizontalLayoutGroup>().padding = new RectOffset(32, 32, 0, 0);

            Object.Destroy(waveStrip.GetComponent<HGButton>());
            Object.Destroy(waveStrip.GetComponent<MPEventSystemLocator>());
            waveStrip.GetComponent<Image>().sprite = game.unlockStripPrefab.GetComponent<Image>().sprite;
            waveStrip.GetComponent<Image>().color = game.unlockStripPrefab.GetComponent<Image>().color;

            //Stat
            waveStrip.transform.GetChild(0).name = "WaveStat";
            waveStrip.transform.GetChild(2).name = "WaveNamr";
            waveStrip.transform.GetChild(3).name = "WaveIcon";
            waveStrip.transform.GetChild(0).GetComponent<LanguageTextMeshController>().token = "LATEST_WAVE";
            waveStrip.transform.GetChild(0).GetComponent<HGTextMeshProUGUI>().fontSizeMax = 24;
            waveStrip.transform.GetChild(2).GetComponent<HGTextMeshProUGUI>().fontSizeMax = 24;

            LayoutElement icon = waveStrip.transform.GetChild(3).GetComponent<LayoutElement>();
            icon.preferredHeight = 54;
            icon.preferredWidth = 54;
            #endregion

            #region Stage Strip
            GameObject areaStrip = PrefabAPI.InstantiateClone(game.unlockStripPrefab, "stageStripPrefab", false);
            DeathScreenExpanded.stageStripPrefab = areaStrip;

            HorizontalLayoutGroup layout = areaStrip.GetComponent<HorizontalLayoutGroup>();
            layout.spacing = 8;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.padding = new RectOffset(16, 16, 0, 0);
            layout.childForceExpandWidth = true;
            areaStrip.SetActive(true);

            GameObject.Destroy(areaStrip.transform.GetChild(1).gameObject);
            GameObject.Destroy(areaStrip.transform.GetChild(0).gameObject);
            GameObject.Destroy(areaStrip.GetComponent<TooltipProvider>());
            #endregion

            #region Stage Icon
            DeathScreenExpanded.stageIconPrefab = PrefabAPI.InstantiateClone(game.unlockStripPrefab.transform.GetChild(0).gameObject, "stageIcon", false);

            GameObject stageIcon = DeathScreenExpanded.stageIconPrefab;
            stageIcon.AddComponent<TooltipProvider>();
            //Flexibile width 0??

            AspectRatioFitter ratio = stageIcon.GetComponent<AspectRatioFitter>();
            ratio.aspectRatio = 480f / 288f;
            ratio.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;

            stageIcon.GetComponent<LayoutElement>().preferredHeight = 60;
            RectTransform border = stageIcon.transform.GetChild(0).GetComponent<RectTransform>();
            Image borderIcon = stageIcon.transform.GetChild(0).GetComponent<Image>();
            borderIcon.color = new Color(0.1412f, 0.1059f, 0.1099f, 1f);
            border.offsetMax = new Vector2(2.5f, 2.5f);
            border.offsetMin = new Vector2(-2.5f, -2.5f);
            borderIcon.sprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/General/texUIHighlightBoxOutline.png");
            //border.gameObject.SetActive(false);
            #endregion


        }

        private static void OneTimeExtras(On.RoR2.UI.GameEndReportPanelController.orig_SetDisplayData orig, GameEndReportPanelController self, GameEndReportPanelController.DisplayData newDisplayData)
        {
            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();
            if (extras == null)
            {
                extras = self.gameObject.AddComponent<DeathScreenExpanded>();

                extras.isLogRunReport = newDisplayData.runReport.FindFirstPlayerInfo().master == null;
                if (!extras.isLogRunReport)
                {
                    extras.childLocator = AddChildLocator(self);
                }
                extras.isSaveAndContinuedRun = (RunExtraStatTracker.instance && RunExtraStatTracker.instance.isSaveAndContinuedRun);
                extras.GameEndReportPanel = self;
                extras.ItemArea = self.itemInventoryDisplay.transform.parent.parent.parent;

                extras.isDevotionRun = newDisplayData.runReport.ruleBook.GetRuleChoice(RuleCatalog.FindRuleDef("Artifacts.Devotion")).localIndex == 0;

                if (WConfig.DC_MoreStats.Value)
                {
                    ExtraStats.ChangeStats(self, newDisplayData.runReport);
                }
                extras.ItemArea.GetChild(1).gameObject.AddComponent<InventoryDisplayFitter>();

            }


            orig(self, newDisplayData);
            try
            {
                if (extras.oneTimeExtras)
                {
                    return;
                }
                if (extras.isLogRunReport)
                {
                    return;
                }
                if (self.chatboxTransform)
                {
                    if (extras.oneTimeExtras == false)
                    {
                        extras.chatActive = self.chatboxTransform.gameObject.activeSelf;
                    }
                    else
                    {
                        self.chatboxTransform.gameObject.SetActive(extras.chatActive);
                    }
                }



                extras.oneTimeExtras = true;
                ExtraStats.DifficultyTooltip(self);
                MakeChatToggleBotton(extras);



                RectTransform RECT = self.GetComponent<RectTransform>();
                RECT.anchorMax = new Vector3(0.96f, 0.95f);
                RECT.anchorMin = new Vector3(0.04f, 0.05f);

                RectTransform SafeArea = (RectTransform)self.transform.GetChild(0);
                RectTransform BodyArea = (RectTransform)self.transform.GetChild(0).GetChild(1);
                RectTransform StatsAndChatArea = (RectTransform)BodyArea.Find("StatsAndChatArea");
                RectTransform RightArea = (RectTransform)BodyArea.Find("RightArea");




                //Slightly, Wider
                RECT = StatsAndChatArea.GetComponent<RectTransform>();
                //RECT.anchorMax = new Vector3(0.48f, 1f);

                //Slighty thinner?
                RECT = RightArea.GetComponent<RectTransform>();
                //RECT.anchorMin = new Vector3(0.54f, 0f);
                //RECT.anchorMax = new Vector3(1f, 1f);

                //Less Header space
                LayoutElement header = SafeArea.GetChild(0).GetComponent<LayoutElement>();
                header.minHeight = 64;
                header.preferredHeight = 72;

                //Expand in all directions
                SafeArea.offsetMin = new Vector2(4, -20);
                SafeArea.offsetMax = new Vector2(4, 12);
                Transform bodyArea = SafeArea.GetChild(1);

                //Match Header Width of RightSide header
                LayoutElement statInfo = extras.childLocator.FindChild("StatsContainer").GetChild(2).GetComponent<LayoutElement>();
                statInfo.preferredHeight = 48;

                //Make Left even to Right
                bodyArea.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, -28);

                //Make Right a bit wider
                //(bodyArea.GetChild(1) as RectTransform).anchorMin = new Vector2(0.59f, 0f); //Even just 0.01 makes a difference
                //(bodyArea.GetChild(1) as RectTransform).offsetMax = new Vector2(0f, -24f);
                //(bodyArea.GetChild(1) as RectTransform).offsetMin = new Vector2(0f, -24f);

                Transform InfoBody = extras.childLocator.FindChild("InfoArea").GetChild(3);
                VerticalLayoutGroup vert = InfoBody.GetComponent<VerticalLayoutGroup>();
                vert.spacing = 8;
                vert.padding = new RectOffset(8, 8, 8, 8);
                InfoBody.GetChild(0).GetComponent<HorizontalLayoutGroup>().padding.left = 24;
                InfoBody.GetChild(1).GetComponent<HorizontalLayoutGroup>().padding.right = 24;



                //Chat closing button?
                //PageIndicator
                //Stat footer?


            }
            catch (System.Exception e)
            {
                WQoLMain.log.LogError(e);
            }


        }



        public static void MakeChatToggleBotton(DeathScreenExpanded extras)
        {
            GameObject toggleChatButton = new GameObject("ChatToggle");

            var I = toggleChatButton.AddComponent<Image>();
            var B = toggleChatButton.AddComponent<Button>();
            var L = toggleChatButton.AddComponent<LayoutElement>();
            var A = toggleChatButton.AddComponent<AspectRatioFitter>();

            A.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;

            L.ignoreLayout = false;

            I.sprite = Addressables.LoadAssetAsync<Sprite>(key: "040ee8a9d9afe894088ab8a0875b1925").WaitForCompletion();
            I.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

            B.onClick.AddListener(extras.ToggleChat);
            B.image = I;
            B.m_Colors.highlightedColor = new Color(2, 2, 2, 2);
            B.m_Colors.selectedColor = new Color(2, 2, 2, 2);
            //B.m_Colors.pressedColor = new Color(2, 2, 1, 2);

            int mult = extras.chatActive ? 1 : -1;

            toggleChatButton.transform.SetParent(extras.childLocator.FindChild("StatsContainer").Find("Stats Footer"), false);
            toggleChatButton.transform.SetSiblingIndex(0);
            toggleChatButton.transform.localScale = new Vector3(0.5f, 0.5f * mult, 0.5f);
            toggleChatButton.transform.localEulerAngles = Vector3.zero;
            extras.chatToggleButton = toggleChatButton;
            extras.chatToggleButton.transform.localPosition = new Vector3(-360, 0, 0);



        }

    }

}
