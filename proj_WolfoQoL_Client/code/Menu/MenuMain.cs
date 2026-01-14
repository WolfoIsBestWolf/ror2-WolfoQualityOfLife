using RoR2;
using RoR2.ContentManagement;
using RoR2.EntitlementManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.Menu
{
    public static class MenuMain
    {

        public static void Start()
        {
            if (WConfig.module_menu_deathscreen.Value)
            {
                DeathScreen_Main.Start();
            }
            if (WConfig.module_menu_logbook.Value)
            {
                LogbookStuff.Start();
            }
            if (WConfig.module_menu_other.Value)
            {
                //Main Menu Stuff
                On.RoR2.UI.MainMenu.MainMenuController.Start += MainMenuExtras;
                UIBorders.Start();
                UI_Stuff.Start();
                UI_RealTimeTimer.Start();

                UI_Color.Start();
            }

            GameModeCatalog.availability.CallWhenAvailable(CallLate);
        }

        private static void CallLate()
        {
            ObjectiveHudSpacing_SettingChanged(null, null);
            On.RoR2.UI.ObjectivePanelController.StripExitAnimation.ctor += StripExitAnimation_ctor;
            On.RoR2.UI.ObjectivePanelController.StripExitAnimation.SetPercentComplete += StripExitAnimation_SetPercentComplete;
        }

        private static void StripExitAnimation_ctor(On.RoR2.UI.ObjectivePanelController.StripExitAnimation.orig_ctor orig, RoR2.UI.ObjectivePanelController.StripExitAnimation self, RoR2.UI.ObjectivePanelController.ObjectiveTracker objectiveTracker)
        {
            orig(self, objectiveTracker);
            if (objectiveTracker.stripObject)
            {
                self.originalHeight += WConfig.ObjectiveHudSpacing.Value;
            }
        }

        private static void StripExitAnimation_SetPercentComplete(On.RoR2.UI.ObjectivePanelController.StripExitAnimation.orig_SetPercentComplete orig, RoR2.UI.ObjectivePanelController.StripExitAnimation self, float newPercentComplete)
        {
            orig(self, newPercentComplete);
            if (self.objectiveTracker.stripObject)
            {
                self.layoutElement.preferredHeight -= WConfig.ObjectiveHudSpacing.Value;
            }
        }

        public static void ObjectiveHudSpacing_SettingChanged(object sender, System.EventArgs e)
        {
            //Debug.Log($"Setting UISpacing to {ObjectiveHudSpacing.Value}");
            Run classic = GameModeCatalog.FindGameModePrefabComponent("ClassicRun");
            Run simulac = GameModeCatalog.FindGameModePrefabComponent("InfiniteTowerRun");
 
            classic.uiPrefab.transform.Find("RightInfoBar/ObjectivePanel/StripContainer").GetComponent<VerticalLayoutGroup>().spacing = WConfig.ObjectiveHudSpacing.Value;
            simulac.uiPrefab.transform.Find("RightInfoBar/ObjectivePanel/StripContainer").GetComponent<VerticalLayoutGroup>().spacing = WConfig.ObjectiveHudSpacing.Value;
            if (Run.instance)
            {
                Run.instance.uiInstances[0].transform.Find("RightInfoBar/ObjectivePanel/StripContainer").GetComponent<VerticalLayoutGroup>().spacing = WConfig.ObjectiveHudSpacing.Value;
            }
            
        }

        private static int themeStatic = -1;
        public static void MainMenuExtras(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);
            if (WConfig.cfgMainMenuScav.Value)
            {
                GameObject tempmain = GameObject.Find("/HOLDER: Title Background");
                GameObject ScavHolder = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU3 Props.prefab").WaitForCompletion(), tempmain.transform);

                Object.Destroy(ScavHolder.transform.GetChild(3).gameObject);
                Object.Destroy(ScavHolder.transform.GetChild(1).gameObject);
                Object.Destroy(ScavHolder.transform.GetChild(0).gameObject);
                ScavHolder.SetActive(true);

                if (EntitlementManager.localUserEntitlementTracker.AnyUserHasEntitlement(WolfoLibrary.DLCS.entitlementDLC1))
                {

                    GameObject gupBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GupBody.prefab").WaitForCompletion();
                    GameObject mdlGup = Object.Instantiate(gupBody.transform.GetChild(0).GetChild(0).gameObject);

                    ModelSkinController gup = mdlGup.GetComponent<ModelSkinController>();
                    if (!gup.DelayLoadingAnimatorUntilMaterialsHaveCompleted && gup.animator)
                    {
                        if (gup.avatarRef != null)
                        {
                            gup.avatarRef.SetUnloadType(AsyncReferenceHandleUnloadType.OnSceneUnload);
                        }
                        if (gup.animatorControllerRef != null)
                        {
                            gup.animatorControllerRef.SetUnloadType(AsyncReferenceHandleUnloadType.OnSceneUnload);
                        }
                    }
                    self.StartCoroutine(gup.ApplySkinAsync(0, AsyncReferenceHandleUnloadType.OnSceneUnload));
                    mdlGup.GetComponent<Animator>().Play("Spawn1", 0, 1);
                    mdlGup.transform.localScale = new Vector3(7, 7, 7);
                    mdlGup.transform.localPosition = new Vector3(55.2f, -1f, 11.5f);
                    mdlGup.transform.localEulerAngles = new Vector3(7f, 262f, 0);

                    mdlGup.GetComponent<Animator>().Play("Spawn1", 0, 1);
                    mdlGup.transform.localScale = new Vector3(7, 7, 7);
                    mdlGup.transform.localPosition = new Vector3(52f, -1.2f, 160f);
                    mdlGup.transform.localEulerAngles = new Vector3(7f, 157f, 345f);
                }
                Transform temp = self.extraGameModeMenuScreen.desiredCameraTransform;
                self.extraGameModeMenuScreen.desiredCameraTransform = self.moreMenuScreen.desiredCameraTransform;
                self.moreMenuScreen.desiredCameraTransform = temp;


                temp.localPosition = new Vector3(36.24f, 2.7204f, 3.1807f);
                temp.localEulerAngles = new Vector3(5.2085f, 38.4904f, 0);
            }

            WConfig.MainMenuTheme selector = WConfig.cfgMainMenuRandomizer.Value;
            if (selector >= WConfig.MainMenuTheme.Acres)
            {
                GameObject theme = null;
                if (selector == WConfig.MainMenuTheme.Acres)
                {
                    theme = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU1 Props.prefab").WaitForCompletion());
                }
                else if (selector == WConfig.MainMenuTheme.Sirens)
                {
                    theme = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU2 Props.prefab").WaitForCompletion());
                }
                else if (selector == WConfig.MainMenuTheme.Realms)
                {
                    theme = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU3 Props.prefab").WaitForCompletion());
                }
                else if (selector == WConfig.MainMenuTheme.Artifact)
                {
                    theme = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU4 Props.prefab").WaitForCompletion());
                }
                if (theme)
                {
                    theme.SetActive(true);
                }
            }
            else if (selector >= WConfig.MainMenuTheme.Random)
            {
                GameObject StarStorm2 = GameObject.Find("/StormMainMenuEffect(Clone)");
                GameObject CU1 = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU1 Props.prefab").WaitForCompletion());
                GameObject CU2 = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU2 Props.prefab").WaitForCompletion());

                int randomMenu = WQoLMain.random.Next(6);
                if (selector == WConfig.MainMenuTheme.RandomStatic)
                {
                    if (themeStatic == -1)
                    {
                        themeStatic = randomMenu;
                    }
                    randomMenu = themeStatic;
                }
                switch (randomMenu)
                {
                    case 0:
                        //Default
                        break;
                    case 1:
                    case 3:
                        CU1.SetActive(true); //Orange
                        break;
                    case 2:
                    case 4:
                        CU2.SetActive(true); //Green
                        break;
                }
                if (randomMenu >= 3)
                {
                    if (StarStorm2)
                    {
                        StarStorm2.SetActive(false);
                    }
                }
            }

        }


    }

}
