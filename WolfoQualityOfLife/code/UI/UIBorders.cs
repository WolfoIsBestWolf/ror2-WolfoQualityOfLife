using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2.UI;
using RoR2.Stats;

namespace WolfoQualityOfLife
{
    public class UIBorders
    {
 
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
            if (WConfig.cfgUIEclipseBorder.Value)
            {
            bool isEclipse = false;
            //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.StartsWith("infinitet"))

            if (self.GetComponent<UnityEngine.UI.Image>().canvas.name.StartsWith("Eclipse"))
            {
                //PreLobby
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
            if (isEclipse)
            {
                LocalUser firstLocalUser = LocalUserManager.GetFirstLocalUser();
                //SurvivorDef survivorDef = (firstLocalUser != null) ? firstLocalUser.userProfile.GetSurvivorPreference() : null;

                if (self.survivorDef)
                {
                    int localUserSurvivorCompletedEclipseLevel = EclipseRun.GetLocalUserSurvivorCompletedEclipseLevel(firstLocalUser, self.survivorDef);
                    //Debug.Log(localUserSurvivorCompletedEclipseLevel);

                    Transform HolderTransform = self.transform.Find("Eclipse_Holder");
                    GameObject Holder = null;
                    if (HolderTransform == null)
                    {
                        Holder = new GameObject("Eclipse_Holder");
                        GameObject HolderBorder = new GameObject("Border");
                        GameObject HolderNumber = new GameObject("Number");
                        HolderBorder.transform.SetParent(Holder.transform);
                        HolderNumber.transform.SetParent(Holder.transform);

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
                        GameObject IconToDupe = GameObject.Instantiate(self.gameObject.transform.GetChild(1).gameObject, HolderBorder.transform);
                        IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(156, 195, 231, 255);
                        IconToDupe.transform.localScale = new Vector3(1f, 1f, 1f);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(5f, 5f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-5f, -5f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(4f, 4f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-4f, -4f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                        IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(93, 117, 140, 255);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(3f, 3f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-3f, -3f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                        IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(66, 81, 99, 255);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(2f, 2f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-2f, -2f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                        IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(27, 35, 40, 255);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(1f, 1f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-1f, -1f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderBorder.transform);
                        IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(0, 4, 8, 255);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                        #endregion
                        #region Number
                        HolderNumber.GetComponent<RectTransform>().anchoredPosition = new Vector2(-4f, -4f);

                        GameObject BackDrop = new GameObject("BackDrop");
                        BackDrop.transform.SetParent(HolderNumber.transform);
                        UnityEngine.UI.Image Back = BackDrop.AddComponent<UnityEngine.UI.Image>();
                        Back.color = new Color(0, 0, 0, 0.96f);
                        BackDrop.GetComponent<RectTransform>().anchorMax = new Vector2(0.4f, 0.4f);
                        BackDrop.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                        BackDrop.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                        BackDrop.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);
                        BackDrop.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(3f, 3f, 0f);
                        BackDrop.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                        IconToDupe = GameObject.Instantiate(self.gameObject.transform.GetChild(1).gameObject, HolderNumber.transform);
                        IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(156, 195, 231, 255);
                        IconToDupe.transform.localScale = new Vector3(1f, 1f, 1f);
                        IconToDupe.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
                        IconToDupe.GetComponent<RectTransform>().anchorMin = new Vector2(0f, 0f);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(-1f, -1f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);

                        IconToDupe = GameObject.Instantiate(IconToDupe, HolderNumber.transform);
                        IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(-2f, -2f);
                        IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(1f, 1f);

                        GameObject Number = new GameObject("Number");
                        Number.transform.SetParent(HolderNumber.transform);
                        HGTextMeshProUGUI Text = Number.AddComponent<RoR2.UI.HGTextMeshProUGUI>();
                        Text.text = localUserSurvivorCompletedEclipseLevel.ToString();
                        Text.fontSize = 30; //36     
                        //Text.color = new Color32(156, 195, 231, 255);
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

                        #endregion
                    }
                    else
                    {
                        Holder = HolderTransform.gameObject;
                    }

                    Holder.transform.GetChild(1).GetComponentInChildren<HGTextMeshProUGUI>().text = localUserSurvivorCompletedEclipseLevel.ToString();

                    if (localUserSurvivorCompletedEclipseLevel == EclipseRun.maxEclipseLevel)
                    {
                        //Completed Show Border
                        Holder.SetActive(true);
                        Holder.transform.GetChild(0).gameObject.SetActive(false);
                        //self.transform.GetChild(1).gameObject.SetActive(false);
                    }
                    else if (localUserSurvivorCompletedEclipseLevel > 0)
                    {
                        //Incomplete just show number
                        Holder.SetActive(true);
                        Holder.transform.GetChild(0).gameObject.SetActive(false);
                        Holder.transform.GetChild(1).GetComponentInChildren<HGTextMeshProUGUI>().text = localUserSurvivorCompletedEclipseLevel.ToString();
                    }
                    else
                    {
                        //Not started show nothing
                        Holder.SetActive(false);
                    }




                }
            }
            }
        }

        private static void Beat_Simulacrum_IconBorder(On.RoR2.UI.SurvivorIconController.orig_Rebuild orig, RoR2.UI.SurvivorIconController self)
        {
            orig(self);
            if (WConfig.cfgUISimuBorder.Value)
            {
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
            if (isSimu)
            {
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

                        ulong statValueULong_Easy = statSheet.GetStatValueULong(PerBodyStatDef.highestInfiniteTowerWaveReachedEasy, bodyName);
                        ulong statValueULong_Normal = statSheet.GetStatValueULong(PerBodyStatDef.highestInfiniteTowerWaveReachedNormal, bodyName);
                        ulong statValueULong_Hard = statSheet.GetStatValueULong(PerBodyStatDef.highestInfiniteTowerWaveReachedHard, bodyName);

                        Transform HolderTransform = self.transform.Find("IT_Win_Holder");
                        GameObject Holder = null;
                        if (HolderTransform == null)
                        {
                            Holder = new GameObject("IT_Win_Holder");

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



                            GameObject IconToDupe = GameObject.Instantiate(self.gameObject.transform.GetChild(1).gameObject, HolderTransform);
                            IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(231, 154, 239, 255);
                            IconToDupe.transform.localScale = new Vector3(1f, 1f, 1f);
                            IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(5f, 5f);
                            IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-5f, -5f);

                            IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                            IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(4f, 4f);
                            IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-4f, -4f);

                            IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                            IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(247, 117, 206, 255);
                            IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(3f, 3f);
                            IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-3f, -3f);

                            IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                            IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(189, 4, 66, 255);
                            IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(2f, 2f);
                            IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-2f, -2f);

                            IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                            IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(85, 8, 35, 255);
                            IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(1f, 1f);
                            IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(-1f, -1f);

                            IconToDupe = GameObject.Instantiate(IconToDupe, HolderTransform);
                            IconToDupe.GetComponent<UnityEngine.UI.Image>().color = new Color32(8, 0, 0, 255);
                            IconToDupe.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 0f);
                            IconToDupe.GetComponent<RectTransform>().offsetMin = new Vector2(0f, 0f);

                        }
                        else
                        {
                            Holder = HolderTransform.gameObject;
                        }
                        if (statValueULong_Hard >= 50 || statValueULong_Normal >= 50 || statValueULong_Easy >= 50)
                        {
                            self.transform.GetChild(1).gameObject.SetActive(false);
                            Holder.SetActive(true);
                        }
                        else
                        {
                            self.transform.GetChild(1).gameObject.SetActive(true);
                            Holder.SetActive(false);
                        }


                    }

                }
            }
            }
        }

    }

}
