using R2API;
using RoR2;
using RoR2.Stats;
using RoR2.UI;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
namespace WolfoQoL_Client.DeathScreen
{

    public class DeathScreenExpanded : MonoBehaviour
    {
        public bool compactedStats;
        public bool addedRunRecap;
        public bool isLogRunReport;
        //public static DeathScreenExpanded instance;
        public GameObject killerInventory;
        public bool IsEvoInventory;

        public GameObject stat_loadout;
        public GameObject latestWaveStrip;

        public static GameObject difficulty_stat;
 
        public static GameObject item_area;
        public static GameObject unlock_area;
        public static GameObject statHolderPrefab;

        public bool oneTimeExtras = false;
        public bool madeStats = false;

        public bool addedStageRecap;
        public List<GameObject> stageStrips;
        public static GameObject stageIconPrefab;
        public static GameObject stageStripPrefab;
        public static GameObject waveStripPrefab;
        public GameEndReportPanelController GameEndReportPanel;

        public GameObject chatToggleButton;
        public bool chatActive;
        
        public void ToggleChat()
        {
            Util.PlaySound("Play_UI_menuClick", this.gameObject);
            chatToggleButton.transform.localScale = new Vector3(0.5f, -chatToggleButton.transform.localScale.y, 0.5f);
            chatActive = !chatActive;
            GameEndReportPanel.chatboxTransform.gameObject.SetActive(chatActive);
        }
    }

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

        }
 
        private static void AddManyExtras(On.RoR2.UI.GameEndReportPanelController.orig_SetPlayerInfo orig, GameEndReportPanelController self, RunReport.PlayerInfo playerInfo, int playerIndex)
        {
            EquipOnDeathInventory.DeathEquip_Enemy1 = EquipmentIndex.None;
            if (playerInfo == null)
            {
                EquipOnDeathInventory.DeathEquips_Player = System.Array.Empty<EquipmentIndex>();
                return;
            }
            EquipOnDeathInventory.DeathEquips_Player = playerInfo.equipment;

            orig(self, playerInfo, playerIndex);

            try
            {
                LoadoutStat.Add_Loadout(self, playerInfo);
                RunRecap.AddRunRecap(self, playerInfo);
                KillerInventory.AddKillerInventory(self, playerInfo);
                ExtraStats.AddCustomStats(self, playerInfo);

            }
            catch (System.Exception ex) 
            {
                    //It's the death screen, it can afford it
                 Debug.LogException(ex);
            }

        
            //Dont just remove the spacer because it looks too bunched together
            GameObject killerArea = self.killerPanelObject;
            if (killerArea != null)
            {
                killerArea.SetActive(true);
                killerArea.transform.GetChild(0).gameObject.SetActive(playerInfo.isDead);
                killerArea.transform.GetChild(1).gameObject.SetActive(playerInfo.isDead);
            }
            if (self.finalMessageLabel)
            {
                self.finalMessageLabel.fontSizeMin = self.finalMessageLabel.fontSizeMax;
            }
        }

        public static void MakePrefabs()
        {
            GameObject GameEndReportPanel = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/GameEndReportPanel.prefab").WaitForCompletion();
            GameEndReportPanelController game = GameEndReportPanel.GetComponent<GameEndReportPanelController>();

            DeathScreenExpanded.difficulty_stat = game.statContentArea.GetChild(0).gameObject;
            DeathScreenExpanded.unlock_area = game.unlockContentArea.parent.parent.parent.gameObject;
            DeathScreenExpanded.item_area = DeathScreenExpanded.unlock_area.transform.parent.GetChild(2).gameObject;

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
            waveStrip.transform.GetChild(0).name = "WaveName";
            waveStrip.transform.GetChild(2).name = "WaveIcon";
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

            stageIcon.GetComponent<LayoutElement>().preferredHeight = 52;
            RectTransform border = stageIcon.transform.GetChild(0).GetComponent<RectTransform>();
            Image borderIcon = stageIcon.transform.GetChild(0).GetComponent<Image>();
            /*border.offsetMax = new Vector2(3, 3);
            border.offsetMin = new Vector2(-3, -3);
            borderIcon.color = new Color(1f, 1f, 1f, 0.7f); */
            borderIcon.color = new Color(1f, 1f, 1f, 0.25f);
            border.offsetMax = new Vector2(2, 2);
            border.offsetMin = new Vector2(-2, -2);
            borderIcon.sprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/General/texUIHighlightBoxOutline.png");
            border.gameObject.SetActive(true);
            #endregion


        }

        private static void OneTimeExtras(On.RoR2.UI.GameEndReportPanelController.orig_SetDisplayData orig, GameEndReportPanelController self, GameEndReportPanelController.DisplayData newDisplayData)
        {
            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();
            if (extras == null)
            {
                extras = self.gameObject.AddComponent<DeathScreenExpanded>();
                extras.GameEndReportPanel = self;
                if (WConfig.DC_MoreStats.Value)
                {
                    ExtraStats.ChangeStats(self, newDisplayData.runReport);
                }
              
            }
            extras.isLogRunReport = newDisplayData.runReport.FindFirstPlayerInfo().master == null;

            orig(self, newDisplayData);
            if (extras.oneTimeExtras == false)
            {
                extras.chatActive = self.chatboxTransform.gameObject.activeSelf;
            }
            if (self.chatboxTransform)
            {
                self.chatboxTransform.gameObject.SetActive(extras.chatActive);
            }
            if (extras.oneTimeExtras)
            {
                return;
            }
            extras.oneTimeExtras = true;
            extras.chatActive = self.chatboxTransform.gameObject.activeSelf;

            ExtraStats.DifficultyTooltip(self);

            if (!extras.isLogRunReport)
            {
                RectTransform SafeArea = (RectTransform)self.transform.GetChild(0);

                //Less Header space
                LayoutElement header = SafeArea.GetChild(0).GetComponent<LayoutElement>();
                header.minHeight = 64;
                header.preferredHeight = 64;

                //Expand in all directions
                SafeArea.offsetMin = new Vector2(0, -26);
                SafeArea.sizeDelta = new Vector2(10, SafeArea.sizeDelta.y);
                Transform bodyArea = SafeArea.GetChild(1);

                //Match Header Width of RightSide header
                LayoutElement statInfo = bodyArea.GetChild(0).GetChild(0).GetChild(2).GetComponent<LayoutElement>();
                statInfo.preferredHeight = 48;

                //Make Left even to Right
                bodyArea.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(0, -28);

                //Make Right a bit wider
                (bodyArea.GetChild(1) as RectTransform).anchorMin = new Vector2(0.59f, 0f); //Even just 0.01 makes a difference

                Transform InfoBody = bodyArea.GetChild(1).GetChild(0).GetChild(3);
                VerticalLayoutGroup vert = InfoBody.GetComponent<VerticalLayoutGroup>();
                vert.spacing = 8;
                vert.padding = new RectOffset(8, 8, 8, 8);
                InfoBody.GetChild(0).GetComponent<HorizontalLayoutGroup>().padding.left = 24;
                InfoBody.GetChild(1).GetComponent<HorizontalLayoutGroup>().padding.right = 24;



                //Chat closing button?
                //PageIndicator
                //Stat footer?
                GameObject toggleChatButton = new GameObject("ChatToggle");
                var I = toggleChatButton.AddComponent<Image>();
                var B = toggleChatButton.AddComponent<Button>();
                var L = toggleChatButton.AddComponent<LayoutElement>();
                 
                L.ignoreLayout = true;

                I.sprite = Addressables.LoadAssetAsync<Sprite>(key: "040ee8a9d9afe894088ab8a0875b1925").WaitForCompletion();
                I.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

                B.onClick.AddListener(extras.ToggleChat);
                B.image = I;
                B.m_Colors.highlightedColor = new Color(2, 2, 2, 2);
                B.m_Colors.selectedColor = new Color(2, 2, 2, 2);
                //B.m_Colors.pressedColor = new Color(2, 2, 1, 2);

                int mult = extras.chatActive ? 1 : -1;

                toggleChatButton.transform.SetParent(bodyArea.GetChild(0).GetChild(0).GetChild(4));
                toggleChatButton.transform.localScale = new Vector3(0.5f, 0.5f * mult, 0.5f);
                toggleChatButton.transform.localEulerAngles = Vector3.zero;
                extras.chatToggleButton = toggleChatButton;
                extras.chatToggleButton.transform.localPosition = new Vector3(-360, 0, 0);
            }


        }





    }

}
