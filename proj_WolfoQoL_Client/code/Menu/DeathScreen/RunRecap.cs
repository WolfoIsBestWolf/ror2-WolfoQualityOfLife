using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;


namespace WolfoQoL_Client.DeathScreen
{
    public class RunRecap
    {


        public static void Start()
        {

            Run.onRunStartGlobal += Run_onRunStartGlobal;
            On.RoR2.PlayerCharacterMasterController.PreStartClient += PlayerCharacterMasterController_PreStartClient;
            On.RoR2.InfiniteTowerWaveController.InstantiateUi += SaveLatestWaveUI;
        }

        private static void PlayerCharacterMasterController_PreStartClient(On.RoR2.PlayerCharacterMasterController.orig_PreStartClient orig, PlayerCharacterMasterController self)
        {
            orig(self);
            self.gameObject.AddComponent<PerPlayer_ExtraStatTracker>();
        }

        private static void SaveLatestWaveUI(On.RoR2.InfiniteTowerWaveController.orig_InstantiateUi orig, InfiniteTowerWaveController self, Transform uiRoot)
        {
            orig(self, uiRoot);
            Run.instance.GetComponent<RunExtraStatTracker>().latestWaveUiPrefab = self.overlayEntries[1].prefab;

        }

        public static void Run_onRunStartGlobal(Run obj)
        {
            obj.gameObject.AddComponent<RunExtraStatTracker>();
        }



        public static void AddRunRecapV2(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo)
        {
            //Clone Right Area
            //Destroy all Children
            //Clone Unlock Area

            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();
            if (extras.isLogRunReport)
            {
                return;
            }
            if (extras.isSaveAndContinuedRun)
            {
                return;
            }
            if (extras.addedRunRecap)
            {
                return;
            }
            if (!WConfig.DC_StageRecap.Value)
            {
                return;
            }
            extras.addedRunRecap = true;

            RectTransform BodyArea = (RectTransform)self.transform.GetChild(0).GetChild(1);
            RectTransform RightArea = (RectTransform)BodyArea.Find("RightArea");
            RightArea.anchorMin = new Vector2(0.5f, 0f);
            //Make Wider to accomedate for runrecap

            GameObject StageRecapMain = GameObject.Instantiate(DeathScreenExpanded.RightAreaPrefab);
            StageRecapMain.transform.localEulerAngles = Vector3.zero;

            GameObject Holder = new GameObject("Holder");
            Holder.transform.SetParent(RightArea, false);

            LayoutElement layout = Holder.AddComponent<LayoutElement>();
            layout.flexibleHeight = 1;
            layout.flexibleWidth = 1;
            //HorizontalLayoutGroup horizontal = Holder.AddComponent<HorizontalLayoutGroup>();
            //horizontal.spacing = -12;
            //horizontal.childForceExpandHeight = false;
            //horizontal.childForceExpandWidth = false;


            RectTransform infoArea = (RectTransform)RightArea.Find("InfoArea");
            RectTransform recapArea = (RectTransform)StageRecapMain.transform;

            infoArea.SetParent(Holder.transform, false);
            StageRecapMain.transform.SetParent(Holder.transform, false);
            Holder.transform.SetAsFirstSibling();

            infoArea.anchoredPosition3D = Vector3.zero;
            infoArea.offsetMax = Vector2.zero;
            infoArea.offsetMin = Vector2.zero;
            infoArea.anchorMax = new Vector2(0.8f, 1f);
            infoArea.anchorMin = Vector2.zero;

            recapArea.anchoredPosition3D = Vector3.zero;
            recapArea.offsetMax = Vector2.zero;
            recapArea.offsetMin = Vector2.zero;
            recapArea.anchorMax = Vector3.one;
            recapArea.anchorMin = new Vector2(0.8f, 0f);


            StageRecapMain.AddComponent<LayoutElement>();

            GameObject.Destroy(StageRecapMain.transform.GetChild(2).gameObject); //Destroy ???
            GameObject.Destroy(StageRecapMain.transform.GetChild(1).gameObject); //Destroy Button
            Transform NewInfoArea = StageRecapMain.transform.GetChild(0);
            Transform InfoHeader = NewInfoArea.transform.GetChild(2);
            InfoHeader.GetChild(0).GetComponent<LanguageTextMeshController>().token = "RUN_RECAP";
            StageRecapMain.name = "StageRecapMain";

            //layout = StageRecapMain.AddComponent<LayoutElement>();
            //layout.ignoreLayout = false;
            RectTransform RECT = StageRecapMain.GetComponent<RectTransform>();


            GameObject StageRecapArea = GameObject.Instantiate(DeathScreenExpanded.unlockAreaPrefab);
            StageRecapArea.transform.SetParent(NewInfoArea, false);
            StageRecapArea.name = "StageRecapArea";
            GameObject.Destroy(NewInfoArea.transform.GetChild(3).gameObject); //Destroy InfoBody
            StageRecapArea.transform.GetChild(0).gameObject.SetActive(false); //Destroy Unlock Header

            extras.stageRecap = StageRecapMain;
            extras.stageRecapArea = StageRecapArea;
            layout = NewInfoArea.GetComponent<LayoutElement>();
            //layout.flexibleWidth = 0;
            //layout.minWidth = 156;
            //layout.preferredWidth = 160;
            extras.stageRecapContent = StageRecapArea.transform.GetChild(1).GetChild(0).GetChild(0);
            VerticalLayoutGroup verticalLayout = extras.stageRecapContent.GetComponent<VerticalLayoutGroup>();

            verticalLayout.childForceExpandWidth = false;
            verticalLayout.padding = new RectOffset(0, 0, 12, 0);

            #region //Populate Icons
            WQoLMain.log.LogMessage("Stage Recap");

            var scenes = RunExtraStatTracker.instance.visitedScenes;



            float iconHeight = 60f;
            verticalLayout.spacing = 12;
            //Fuck trying to do math
            if (scenes.Count >= 16) //16 & >
            {         
                verticalLayout.spacing = 0;
                iconHeight = 48 - ((scenes.Count - 16) * 2.4f);
                iconHeight = Mathf.Max(iconHeight, 36);
            }
            else if (scenes.Count >= 14) //14 & 15
            {
                verticalLayout.spacing = 2;
                iconHeight = 48;
            }
            else if (scenes.Count >= 12) //12 & 13
            {
                verticalLayout.spacing = 4;
                iconHeight = 54;
            }
            else if (scenes.Count >= 10) //10 & 11
            {
                verticalLayout.spacing = 9;
                iconHeight = 60;
            }
            else
            {
                //Shows 10.5 stage icons by default
                //Should shrink to accomedate 16, then have scroll
                verticalLayout.spacing = 12;
                iconHeight = 60;
            }

            for (int i = 0; i < RunExtraStatTracker.instance.visitedScenes.Count; i++)
            {


                GameObject icon = Object.Instantiate(DeathScreenExpanded.stageIconPrefab, extras.stageRecapContent);
                TooltipProvider info = icon.GetComponent<TooltipProvider>();
                if (scenes[i].environmentColor.a != 0)
                {
                    info.titleColor = scenes[i].environmentColor;
                }
                else
                {
                    info.titleColor = Color.gray;
                }
                //icon.name = scenes[i].cachedName;
                info.titleToken = scenes[i].nameToken;
                info.bodyToken = scenes[i].subtitleToken;
                icon.GetComponent<RawImage>().texture = scenes[i].previewTexture;

                icon.GetComponent<LayoutElement>().preferredHeight = iconHeight;

            }
            #endregion

        }



        public static void AddRunRecap_Old(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo)
        {
            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();
            if (extras.isLogRunReport)
            {
                return;
            }
            if (extras.isSaveAndContinuedRun)
            {
                return;
            }
            bool hasAnyUnlocks = self.unlockStrips.Count > 0;
            Transform unlockArea = self.unlockContentArea.parent.parent.parent;
            if (!extras.addedRunRecap)
            {
                extras.addedRunRecap = true;
                if (WConfig.DC_LatestWave.Value)
                {
                    InfiniteTowerRun Simu = Run.instance.GetComponent<InfiniteTowerRun>();
                    if (Simu && !extras.latestWaveStrip)
                    {
                        GameObject waveUI = RunExtraStatTracker.instance.latestWaveUiPrefab;
                        if (waveUI != null)
                        {
                            Transform waveUIRoot = waveUI.transform.GetChild(0);
                            GameObject waveStrip = Object.Instantiate(DeathScreenExpanded.waveStripPrefab, self.unlockContentArea);
                            extras.latestWaveStrip = waveStrip;
                            //Text
                            string waveToken = waveUIRoot.GetChild(1).GetChild(0).GetComponent<InfiniteTowerWaveCounter>().token;
                            waveToken = Language.GetString(waveToken);
                            waveToken = string.Format(waveToken, Simu.waveIndex);
                            Color waveColor = waveUIRoot.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color;
                            waveStrip.transform.GetChild(2).GetComponent<HGTextMeshProUGUI>().color = waveColor;
                            waveStrip.transform.GetChild(2).GetComponent<LanguageTextMeshController>().token = waveToken;


                            //Icon
                            Image newSprite = waveStrip.transform.GetChild(3).GetComponent<Image>();
                            Image ogWaveSprite = waveUIRoot.GetChild(0).GetChild(0).GetComponent<Image>();

                            newSprite.sprite = ogWaveSprite.sprite;
                            newSprite.color = ogWaveSprite.color;

                            //Tooltip
                            TooltipProvider toolTip = waveStrip.AddComponent<TooltipProvider>();
                            toolTip.titleToken = waveToken;
                            toolTip.titleColor = waveColor;
                            toolTip.bodyToken = waveUIRoot.GetChild(1).GetChild(1).GetComponent<LanguageTextMeshController>().token;

                            try
                            {
                                if (waveUIRoot.childCount == 4)
                                {
                                    GameObject Sprite2 = GameObject.Instantiate(newSprite.gameObject, waveStrip.transform);
                                    ogWaveSprite = waveUIRoot.GetChild(3).GetChild(0).GetComponent<Image>();
                                    newSprite = Sprite2.GetComponent<Image>();
                                    newSprite.sprite = ogWaveSprite.sprite;
                                    newSprite.color = ogWaveSprite.color;
                                }
                            }
                            catch (System.Exception e)
                            {
                            }
                        }
                    }

                }
                if (WConfig.DC_StageRecap.Value)
                {/*
                    WQoLMain.log.LogMessage("Run Recap");
                    if (extras.addedStageRecap == false)
                    {
                        extras.addedStageRecap = true;
                        extras.stageStrips = new List<GameObject>();
                        for (int list = 0; list < RunExtraStatTracker.instance.visitedScenesTOTAL.Count; list++)
                        {
                            var scenes = RunExtraStatTracker.instance.visitedScenesTOTAL[list];
                            GameObject areaStrip = Object.Instantiate(DeathScreenExpanded.stageStripPrefab, self.unlockContentArea);
                            extras.stageStrips.Add(areaStrip);
                            //7 is fine but tight
                            float iconHeight = 48f;
                            float mult = 7f / scenes.Count;
                            if (mult < 1)
                            {
                                iconHeight = 52f * mult;
                            }
                     icon.GetComponent<LayoutElement>().preferredHeight = iconHeight;
                            int extraIcons = 5 - scenes.Count;
                            for (int i = 0; i < scenes.Count; i++)
                            {
                                GameObject icon = Object.Instantiate(DeathScreenExpanded.stageIconPrefab, areaStrip.transform);
                                TooltipProvider info = icon.GetComponent<TooltipProvider>();
                                if (scenes[i].environmentColor.a != 0)
                                {
                                    info.titleColor = scenes[i].environmentColor;
                                }
                                else
                                {
                                    info.titleColor = Color.gray;
                                }
                                //icon.name = scenes[i].cachedName;
                                icon.GetComponent<LayoutElement>().preferredHeight = iconHeight;
                                info.titleToken = scenes[i].nameToken;
                                info.bodyToken = scenes[i].subtitleToken;
                                icon.GetComponent<RawImage>().texture = scenes[i].previewTexture;
                            }
                            if (extraIcons > 0)
                            {
                                //Have at least 5 Icons for alignment
                                for (int i = 0; i < extraIcons; i++)
                                {
                                    GameObject icon = Object.Instantiate(DeathScreenExpanded.stageIconPrefab, areaStrip.transform);
                                    icon.GetComponent<RawImage>().enabled = false;
                                    icon.transform.GetChild(0).gameObject.SetActive(false);
                                }
                            }
                        }
                    }*/

                }
            }
            if (WConfig.DC_LatestWave.Value || WConfig.DC_StageRecap.Value)
            {
                //Shrink this so it only fits 1 and expand it up to 4.
                string token = "RUN_RECAP";
                if (hasAnyUnlocks)
                {
                    token = string.Format("{0} & {1}", new string[]
                    {
                            Language.GetString("TOOLTIP_UNLOCK_GENERIC_NAME"),
                            Language.GetString(token),
                    });
                }
                unlockArea.GetChild(0).GetChild(0).GetComponent<LanguageTextMeshController>().token = token;
            }
            //48 Header
            //4 Padding
            //64 Strip
            LayoutElement lay = unlockArea.GetComponent<LayoutElement>();
            WQoLMain.log.LogMessage(self.unlockContentArea.childCount);
            float childMult = Mathf.Max(self.unlockContentArea.childCount, 2f);
            childMult = Mathf.Min(childMult, 2.5f);
            float height = 64 * childMult + 52;
            lay.flexibleHeight = 0;
            lay.minHeight = 116f;
            lay.preferredHeight = height;

            if (extras.latestWaveStrip)
            {
                extras.latestWaveStrip.transform.SetAsLastSibling();
            }
            /*for (int i = 0; i < extras.stageStrips.Count; i++)
            {
                extras.stageStrips[i].transform.SetAsLastSibling();
            }*/
        }



        /*GameObject Spacer = new GameObject("Spacer");
           Spacer.AddComponent<LayoutElement>();
           Spacer.transform.SetParent(BodyArea, false);
           //Spacer.transform.SetSiblingIndex(1);
           Spacer.gameObject.SetActive(false);*/

        //HorizontalLayoutGroup horizontal = BodyArea.gameObject.AddComponent<HorizontalLayoutGroup>();
        //horizontal.childForceExpandWidth = false;
        //horizontal.childForceExpandHeight = false;
        //horizontal.childControlHeight = false;


    }



}
