using RoR2;
using RoR2.Stats;
using RoR2.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace WolfoQoL_Client
{
    public class UIBorders
    {
        public static void UpdateHuds()
        {
            //So all this shit dont work

            float spacing = WConfig.cfgObjectiveHeight.Value; //Default 32
            float fonzSize = WConfig.cfgObjectiveFontSize.Value; //Default 12
            WQoLMain.log.LogMessage("Updating Hud spacing" + WConfig.cfgObjectiveHeight.Value + " / font" + WConfig.cfgObjectiveFontSize.Value);

            HGTextMeshProUGUI text;
            Transform ObjectivePannelRoot;

            UISkinData fontTwo = Addressables.LoadAssetAsync<UISkinData>(key: "RoR2/Base/UI/skinObjectivePanel.asset").WaitForCompletion();
            fontTwo.bodyTextStyle.fontSize = fonzSize;


            GameObject Hud = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ClassicRun/ClassicRunInfoHudPanel.prefab").WaitForCompletion();
            ObjectivePannelRoot = Hud.transform.GetChild(5).GetChild(1);
            ObjectivePannelRoot.GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;
            ObjectivePannelRoot.GetChild(1).GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;
            text = ObjectivePannelRoot.GetChild(1).GetChild(0).GetChild(0).GetComponent<HGTextMeshProUGUI>();
            text.fontSizeMin = fonzSize / 2;
            text.fontSizeMax = fonzSize;
            text.fontSize = fonzSize;

            Hud = Addressables.LoadAssetAsync<GameObject>(key: "658d6f42d13db6a419eaf88b19ea937f").WaitForCompletion();
            ObjectivePannelRoot = Hud.transform.GetChild(5).GetChild(1);
            ObjectivePannelRoot.GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;
            ObjectivePannelRoot.GetChild(1).GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;
            text = ObjectivePannelRoot.GetChild(1).GetChild(0).GetChild(0).GetComponent<HGTextMeshProUGUI>();
            text.fontSizeMin = fonzSize / 2;
            text.fontSizeMax = fonzSize;
            text.fontSize = fonzSize;

            if (HUD.instancesList.Count > 0)
            {
                Hud = HUD.instancesList[0].gameModeUiInstance;


                ObjectivePannelRoot = Hud.transform.GetChild(5).GetChild(1);
                ObjectivePannelRoot.GetChild(0).GetComponent<LayoutElement>().preferredHeight = spacing;

                ObjectivePannelRoot = ObjectivePannelRoot.GetChild(1);
                for (int i = 0; i < ObjectivePannelRoot.childCount; i++)
                {
                    ObjectivePannelRoot.GetChild(i).GetComponent<LayoutElement>().preferredHeight = spacing;
                    text = ObjectivePannelRoot.GetChild(i).GetChild(0).GetComponent<HGTextMeshProUGUI>();
                    text.fontSizeMin = fonzSize / 2;
                    text.fontSizeMax = fonzSize;
                    text.fontSize = fonzSize;
                }
            }

        }



        public static void Start()
        {
            //Border for Wave 50
            bool otherMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("depression_church.EclipseLevelInCharacterSelection");
            //bool otherMod2 = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("NotABot.SimpleEclipseDisplay");

            if (otherMod == false)
            {
                On.RoR2.UI.SurvivorIconController.Rebuild += Eclipse_Border_SurvivorIcon;
            }
            On.RoR2.UI.SurvivorIconController.Rebuild += Beat_Simulacrum_IconBorder;
        }

        private static void Eclipse_Border_SurvivorIcon(On.RoR2.UI.SurvivorIconController.orig_Rebuild orig, SurvivorIconController self)
        {
            orig(self);
            if (!WConfig.cfgUIEclipseBorder.Value)
            {
                return;
            }
            bool isEclipse = false;
            bool preLobby = false;
            //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.StartsWith("infinitet"))

            if (self.GetComponent<Image>().canvas.name.StartsWith("Eclipse"))
            {
                //PreLobby
                preLobby = true;
                isEclipse = true;
            }
            else if (PreGameController.instance && PreGameController.instance.gameModeIndex != (GameModeIndex)255)
            {
                //Lobby
                if (GameModeCatalog.GetGameModeName((PreGameController.instance.gameModeIndex)).StartsWith("Eclipse"))
                {
                    isEclipse = true;
                }
            }
            if (!isEclipse)
            {
                return;
            }
            LocalUser firstLocalUser = LocalUserManager.GetFirstLocalUser();
            //SurvivorDef survivorDef = (firstLocalUser != null) ? firstLocalUser.userProfile.GetSurvivorPreference() : null;

            if (self.survivorDef)
            {
                int localUserSurvivorCompletedEclipseLevel = EclipseRun.GetLocalUserSurvivorCompletedEclipseLevel(firstLocalUser, self.survivorDef);
                //WolfoMain.log.LogMessage(localUserSurvivorCompletedEclipseLevel);

                Transform HolderTransform = self.transform.Find("Eclipse_Holder");
                GameObject Holder = null;
                if (HolderTransform == null)
                {
                    Holder = new GameObject("Eclipse_Holder");
                    GameObject HolderBorder = new GameObject("Border");
                    GameObject HolderNumber = new GameObject("NumberHolder");
                    HolderBorder.transform.SetParent(Holder.transform, false);
                    HolderNumber.transform.SetParent(Holder.transform, false);

                    RectTransform rect = Holder.AddComponent<RectTransform>();
                    Holder.transform.SetParent(self.transform);

                    HolderTransform = Holder.transform;
                    rect.position = new Vector3(35, 0, 0);
                    rect.localScale = new Vector3(1, 1, 1);
                    rect.localEulerAngles = new Vector3(0, 0, 0);
                    rect.anchoredPosition = new Vector2(0, 0);
                    rect.anchoredPosition3D = new Vector3(0, 0, 0);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.anchorMin = new Vector2(0, 0);
                    rect.offsetMax = new Vector2(0f, 0f);
                    rect.offsetMin = new Vector2(0f, 0f);
                    //rect.position = new Vector3(35, 0, 0);

                    rect = HolderBorder.AddComponent<RectTransform>();
                    rect.localScale = new Vector3(1, 1, 1);
                    rect.localEulerAngles = new Vector3(0, 0, 0);
                    rect.anchoredPosition = new Vector2(0, 0);
                    rect.anchoredPosition3D = new Vector3(0, 0, 0);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.anchorMin = new Vector2(0, 0);
                    rect.offsetMax = new Vector2(0f, 0f);
                    rect.offsetMin = new Vector2(0f, 0f);

                    rect = HolderNumber.AddComponent<RectTransform>();
                    rect.localScale = new Vector3(1, 1, 1);
                    rect.localEulerAngles = new Vector3(0, 0, 0);
                    rect.anchoredPosition = new Vector2(0, 0);
                    rect.anchoredPosition3D = new Vector3(0, 0, 0);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.anchorMin = new Vector2(0, 0);
                    rect.offsetMax = new Vector2(0f, 0f);
                    rect.offsetMin = new Vector2(0f, 0f);

                    #region CompletionBorder
                    int num = 5;
                    GameObject IconToDupe = GameObject.Instantiate(self.gameObject.transform.GetChild(1).gameObject, HolderBorder.transform);
                    IconToDupe.GetComponent<Image>().color = new Color32(156, 195, 231, 255);
                    IconToDupe.transform.localScale = new Vector3(1f, 1f, 1f);
                    IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(num, num);
                    IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-num, -num);
                    num--;
                    IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                    IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(num, num);
                    IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-num, -num);
                    num--;
                    IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                    IconToDupe.GetComponent<Image>().color = new Color32(93, 117, 140, 255);
                    IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(num, num);
                    IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-num, -num);
                    num--;

                    IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                    IconToDupe.GetComponent<Image>().color = new Color32(66, 81, 99, 255);
                    IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(num, num);
                    IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-num, -num);
                    num--;

                    IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                    IconToDupe.GetComponent<Image>().color = new Color32(27, 35, 40, 255);
                    IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(num, num);
                    IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-num, -num);
                    num--;

                    /*IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                    IconToDupe.GetComponent<Image>().color = new Color32(0, 4, 8, 255);
                    IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(num, num);
                    IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-num, -num);*/

                    #endregion
                    #region Number
                    HolderNumber.GetComponent<RectTransform>().anchoredPosition = new Vector2(-4f, -4f);

                    GameObject BackDrop = new GameObject("BackDrop");
                    BackDrop.transform.SetParent(HolderNumber.transform, false);
                    Image Back = BackDrop.AddComponent<Image>();
                    Back.color = new Color(0, 0, 0, 0.95f);
                    BackDrop.GetComponent<RectTransform>().anchorMax = new Vector2(0.4f, 0.4f);
                    BackDrop.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                    BackDrop.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                    BackDrop.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                    BackDrop.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(3f, 3f, 0f);
                    BackDrop.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                    IconToDupe = GameObject.Instantiate(self.gameObject.transform.GetChild(1).gameObject, HolderNumber.transform);
                    IconToDupe.GetComponent<Image>().color = new Color32(156, 195, 231, 255);
                    IconToDupe.transform.localScale = new Vector3(1f, 1f, 1f);
                    IconToDupe.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                    IconToDupe.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                    IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(-1f, -1f);
                    IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);

                    IconToDupe = GameObject.Instantiate(IconToDupe, HolderNumber.transform);
                    IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(-2f, -2f);
                    IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(1f, 1f);

                    GameObject Number = new GameObject("Number");
                    Number.transform.SetParent(HolderNumber.transform, false);
                    HGTextMeshProUGUI Text = Number.AddComponent<RoR2.UI.HGTextMeshProUGUI>();
                    Text.text = localUserSurvivorCompletedEclipseLevel.ToString();
                    Text.fontSize = 30; //36     
                                        //Text.color = new Color32(156, 195, 231, 255);
                    Text.color = new Color32(242, 242, 242, 255);
                    Text.alignment = TMPro.TextAlignmentOptions.Center;
                    Text.color = new Color32(242, 242, 242, 255);
                    Text.alignment = TMPro.TextAlignmentOptions.Center;
                    Number.GetComponent<RectTransform>().anchorMax = new Vector2(0f, 0f);
                    Number.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                    Number.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f);
                    Number.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(17f, 17f, 0f);
                    Number.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    //Number.GetComponent<RectTransform>().offsetMin = new Vector2(17f, 17f);
                    //Number.GetComponent<RectTransform>().offsetMax = new Vector2(17f, 17f);
                    Number.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                    Number.GetComponent<RectTransform>().offsetMax = new Vector2(34, 34);
                    if (preLobby)
                    {
                        Number.GetComponent<RectTransform>().offsetMax = new Vector2(32, 34);
                    }

                    #endregion
                }
                else
                {
                    Holder = HolderTransform.gameObject;
                }

                Holder.transform.GetChild(1).GetComponentInChildren<HGTextMeshProUGUI>().text = localUserSurvivorCompletedEclipseLevel.ToString();

                bool borderMode = true;

                if (borderMode && localUserSurvivorCompletedEclipseLevel == EclipseRun.maxEclipseLevel)
                {
                    //Completed Show Border
                    Holder.transform.SetAsFirstSibling();
                    Holder.SetActive(true);
                    Holder.transform.GetChild(0).gameObject.SetActive(true);
                    Holder.transform.GetChild(1).gameObject.SetActive(false);
                }
                else if (borderMode || localUserSurvivorCompletedEclipseLevel > 0)
                {
                    //Incomplete just show number
                    Holder.transform.SetAsLastSibling();
                    Holder.SetActive(true);
                    Holder.transform.GetChild(0).gameObject.SetActive(false);
                    if (borderMode)
                    {
                        Holder.transform.GetChild(1).GetComponentInChildren<HGTextMeshProUGUI>().text = (1 + localUserSurvivorCompletedEclipseLevel).ToString();
                    }
                    else
                    {
                        Holder.transform.GetChild(1).GetComponentInChildren<HGTextMeshProUGUI>().text = (localUserSurvivorCompletedEclipseLevel).ToString();
                    }
                }
                else
                {
                    //Not started show nothing
                    Holder.SetActive(false);
                }
            }


        }

        private static void Beat_Simulacrum_IconBorder(On.RoR2.UI.SurvivorIconController.orig_Rebuild orig, RoR2.UI.SurvivorIconController self)
        {
            orig(self);
            if (!WConfig.cfgUISimuBorder.Value)
            {
                return;
            }

            bool isSimu = false;
            //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.StartsWith("infinitet"))

            if (self.GetComponent<UnityEngine.UI.Image>().canvas.name.StartsWith("InfiniteT"))
            {
                //PreLobby
                isSimu = true;
            }
            else if (PreGameController.instance && PreGameController.instance.gameModeIndex != (GameModeIndex)255)
            {
                //Lobby
                if (GameModeCatalog.GetGameModeName((PreGameController.instance.gameModeIndex)).StartsWith("InfiniteT"))
                {
                    isSimu = true;
                }
            }
            if (!isSimu)
            {
                return;
            }
            MPEventSystem eventSystem = self.characterSelectBarController.eventSystemLocator.eventSystem;
            UserProfile userProfile;
            if (eventSystem == null)
            {
                userProfile = null;
            }
            else
            {
                LocalUser localUser = eventSystem.localUser;
                userProfile = ((localUser != null) ? localUser.userProfile : null);
            }
            UserProfile userProfile2 = userProfile;
            if (userProfile2 != null)
            {
                StatSheet statSheet = userProfile2.statSheet;
                if (self.survivorBodyIndex != BodyIndex.None)
                {
                    string bodyName = BodyCatalog.GetBodyName(self.survivorBodyIndex);

                    //Probably actually set waves beaten to 50 in other mods
                    //bool hasWolfoSkinsAchievement = 
                    ulong statValueULong_Easy = statSheet.GetStatValueULong(PerBodyStatDef.highestInfiniteTowerWaveReachedEasy, bodyName);
                    ulong statValueULong_Normal = statSheet.GetStatValueULong(PerBodyStatDef.highestInfiniteTowerWaveReachedNormal, bodyName);
                    ulong statValueULong_Hard = statSheet.GetStatValueULong(PerBodyStatDef.highestInfiniteTowerWaveReachedHard, bodyName);

                    Transform HolderTransform = self.transform.Find("IT_Win_Holder");
                    GameObject Holder = null;
                    if (HolderTransform == null)
                    {
                        Holder = new GameObject("IT_Win_Holder");

                        RectTransform rect = Holder.AddComponent<RectTransform>();
                        Holder.transform.SetParent(self.transform, false);
                        HolderTransform = Holder.transform;
                        rect.position = new Vector3(35, 0, 0);
                        rect.localScale = new Vector3(1, 1, 1);
                        rect.localEulerAngles = new Vector3(0, 0, 0);
                        rect.anchoredPosition = new Vector2(0, 0);
                        rect.anchoredPosition3D = new Vector3(0, 0, 0);
                        rect.anchorMax = new Vector2(1, 1);
                        rect.anchorMin = new Vector2(0, 0);
                        rect.offsetMax = new Vector2(0f, 0f);
                        rect.offsetMin = new Vector2(0f, 0f);
                        //rect.position = new Vector3(35, 0, 0);



                        GameObject IconToDupe = GameObject.Instantiate(self.gameObject.transform.GetChild(1).gameObject, HolderTransform);
                        IconToDupe.GetComponent<Image>().color = new Color32(231, 154, 239, 255);
                        IconToDupe.transform.localScale = new Vector3(1f, 1f, 1f);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(5f, 5f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-5f, -5f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(4f, 4f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-4f, -4f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                        IconToDupe.GetComponent<Image>().color = new Color32(247, 117, 206, 255);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(3f, 3f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-3f, -3f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                        IconToDupe.GetComponent<Image>().color = new Color32(189, 4, 66, 255);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(2f, 2f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-2f, -2f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                        IconToDupe.GetComponent<Image>().color = new Color32(85, 8, 35, 255);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(1f, 1f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-1f, -1f);

                        /* IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                         IconToDupe.GetComponent<Image>().color = new Color32(8, 0, 0, 255);
                         IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                         IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);*/

                        Holder.transform.SetAsFirstSibling();
                    }
                    else
                    {
                        Holder = HolderTransform.gameObject;
                    }
                    if (statValueULong_Hard >= 50 || statValueULong_Normal >= 50 || statValueULong_Easy >= 50)
                    {
                        //Holder.transform.GetChild(1).gameObject.SetActive(false);
                        Holder.SetActive(true);
                    }
                    else
                    {
                        //self.transform.GetChild(1).gameObject.SetActive(true);
                        Holder.SetActive(false);
                    }


                }

            }
        }

    }

}


