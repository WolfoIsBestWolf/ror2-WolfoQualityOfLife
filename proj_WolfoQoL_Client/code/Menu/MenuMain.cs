using RoR2;
using RoR2.ContentManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.Menu
{
    public class MenuMain
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

            }


        }


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

                Transform temp = self.extraGameModeMenuScreen.desiredCameraTransform;
                self.extraGameModeMenuScreen.desiredCameraTransform = self.moreMenuScreen.desiredCameraTransform;
                self.moreMenuScreen.desiredCameraTransform = temp;


                temp.localPosition = new Vector3(36.24f, 2.7204f, 3.1807f);
                temp.localEulerAngles = new Vector3(5.2085f, 38.4904f, 0);
            }

            int selector = WConfig.cfgMainMenuRandomizerSelector.Value;
            if (selector != 0)
            {
                if (selector == 1 || selector == 11)
                {
                    GameObject CU2 = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU1 Props.prefab").WaitForCompletion());
                    CU2.SetActive(true);
                }
                else if (selector == 2 || selector == 12)
                {
                    GameObject CU2 = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU2 Props.prefab").WaitForCompletion());
                    CU2.SetActive(true);
                }
                else if (selector == 3 || selector == 13)
                {
                    GameObject CU2 = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU3 Props.prefab").WaitForCompletion());
                    CU2.SetActive(true);
                }
                else if (selector == 4 || selector == 14)
                {
                    GameObject CU2 = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU4 Props.prefab").WaitForCompletion());
                    CU2.SetActive(true);
                }
                if (selector > 9)
                {
                    GameObject StarStorm2 = GameObject.Find("/StormMainMenuEffect(Clone)");
                    if (StarStorm2)
                    {
                        StarStorm2.SetActive(false);
                    }
                }
            }
            else if (WConfig.cfgMainMenuRandomizer.Value)
            {
                GameObject StarStorm2 = GameObject.Find("/StormMainMenuEffect(Clone)");
                GameObject CU1 = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU1 Props.prefab").WaitForCompletion());
                GameObject CU2 = Object.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU2 Props.prefab").WaitForCompletion());

                int randomMenu = WQoLMain.random.Next(6);
                switch (randomMenu)
                {
                    case 0:
                        //Default
                        break;
                    case 1:
                        CU1.SetActive(true); //Orange
                        break;
                    case 2:
                        CU2.SetActive(true); //Green
                        break;
                    case 3:
                        if (StarStorm2)
                        {
                            StarStorm2.SetActive(false);
                        }
                        break;
                    case 4:
                        if (StarStorm2)
                        {
                            StarStorm2.SetActive(false);
                        }
                        CU1.SetActive(true);
                        break;
                    case 5:
                        if (StarStorm2)
                        {
                            StarStorm2.SetActive(false);
                        }
                        CU2.SetActive(true);
                        break;
                }
            }
        }


    }

}
