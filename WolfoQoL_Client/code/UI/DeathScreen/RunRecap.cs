using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace WolfoQoL_Client.DeathScreen
{

    public class RunRecap
    {
        public static void Start()
        {

            Run.onRunStartGlobal += Run_onRunStartGlobal;
            On.RoR2.PlayerCharacterMasterController.Start += PlayerCharacterMasterController_Start;
            On.RoR2.InfiniteTowerWaveController.InstantiateUi += SaveLatestWaveUI;
        }

        private static void SaveLatestWaveUI(On.RoR2.InfiniteTowerWaveController.orig_InstantiateUi orig, InfiniteTowerWaveController self, Transform uiRoot)
        {
            orig(self, uiRoot);
            Run.instance.GetComponent<RunExtraStatTracker>().latestWaveUiPrefab = self.overlayEntries[1].prefab;

        }

        private static void PlayerCharacterMasterController_Start(On.RoR2.PlayerCharacterMasterController.orig_Start orig, PlayerCharacterMasterController self)
        {
            orig(self);
            self.gameObject.AddComponent<PerPlayer_ExtraStatTracker>();
        }

        public static void Run_onRunStartGlobal(Run obj)
        {
            obj.gameObject.AddComponent<RunExtraStatTracker>();
        }

        public static void AddRunRecap(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo)
        {
            DeathScreenExpanded extras = self.GetComponent<DeathScreenExpanded>();
            if (extras.isLogRunReport)
            {
                return;
            }
            if (extras.addedRunRecap)
            {
                return;
            }
            extras.addedRunRecap = true;
            bool eitherChange = false;
            bool hadAnyUnlockables = self.unlockContentArea.childCount > 0;
            if (WConfig.DC_StageRecap.Value)
            {
                Debug.Log("Run Recap");
                eitherChange = true;
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
                }
                if (extras.addedStageRecap == true)
                {
                    for (int i = 0; i < extras.stageStrips.Count; i++)
                    {
                        extras.stageStrips[i].transform.SetSiblingIndex(i);
                    }
                }

            }
            if (WConfig.DC_LatestWave.Value)
            {
                eitherChange = true;
                InfiniteTowerRun Simu = Run.instance.GetComponent<InfiniteTowerRun>();
                if (Simu && !extras.latestWaveStrip)
                {
                    GameObject waveUI = RunExtraStatTracker.instance.latestWaveUiPrefab;
                    if (waveUI == null)
                    {
                        return;
                    }
                    Transform waveUIRoot = waveUI.transform.GetChild(0);
                    GameObject waveStrip = Object.Instantiate(DeathScreenExpanded.waveStripPrefab, self.unlockContentArea);

                    //Text
                    string waveToken = waveUIRoot.GetChild(1).GetChild(0).GetComponent<InfiniteTowerWaveCounter>().token;
                    waveToken = Language.GetString(waveToken);
                    waveToken = string.Format(waveToken, Simu.waveIndex);
                    Color waveColor = waveUIRoot.GetChild(1).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color;
                    waveStrip.transform.GetChild(2).GetComponent<HGTextMeshProUGUI>().color = waveColor;
                    waveStrip.transform.GetChild(2).GetComponent<LanguageTextMeshController>().token = waveToken;
                    //waveStrip.transform.GetChild(2).SetAsLastSibling();

                    //waveStrip.transform.GetChild(2).GetComponent<HGTextMeshProUGUI>().m_underlineColor = waveUIRoot.GetChild(2).GetComponent<Image>().color;
                    //waveStrip.transform.GetChild(2).GetComponent<HGTextMeshProUGUI>().fontStyle |= TMPro.FontStyles.Underline;


                    //Icon
                    Image newSprite = waveStrip.transform.GetChild(3).GetComponent<Image>();
                    Image ogWaveSprite = waveUIRoot.GetChild(0).GetChild(0).GetComponent<Image>();

                    newSprite.sprite = ogWaveSprite.sprite;
                    newSprite.color = ogWaveSprite.color;

                    //Tooltip
                    TooltipProvider toolTip = waveStrip.AddComponent<TooltipProvider>();
                    toolTip.titleToken = waveToken;
                    toolTip.titleColor = waveUIRoot.GetChild(2).GetComponent<Image>().color;
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
                if (extras.latestWaveStrip)
                {
                    extras.latestWaveStrip.transform.SetSiblingIndex(0);
                }

            }

            if (eitherChange)
            { 
                //Shrink this so it only fits 1 and expand it up to 4.
                Transform unlockArea = self.unlockContentArea.parent.parent.parent;
                string token = "RUN_RECAP";
                /*if (hadAnyUnlockables)
                {
                    token = string.Format("{0} & {1}", new string[]
                    {
                        Language.GetString(token),
                        Language.GetString("TOOLTIP_UNLOCK_GENERIC_NAME"),
                    });
                }*/
                unlockArea.GetChild(0).GetChild(0).GetComponent<LanguageTextMeshController>().token = token;

                float flexHeight = Mathf.Pow(2, Mathf.Min(self.unlockContentArea.childCount, 1f)) * 0.1f;
                flexHeight = Mathf.Min(flexHeight, 1.5f);
                unlockArea.GetComponent<LayoutElement>().flexibleHeight = flexHeight;
            }
 
        }




    }



}
