using RoR2;
//using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQoL_Client
{

    public class LunarSeer
    {
        public static void Start()
        {
            On.RoR2.SeerStationController.OnStartClient += SeerStageName_Start;
            On.RoR2.SeerStationController.OnTargetSceneChanged += SeerStageName_SceneChanged;
            On.RoR2.SeerStationController.SetRunNextStageToTarget += FancyBazaarPortalChange;

            #region Make some Seers more obvious
            //Make goolake ancient loft more obvious
            Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/bazaar/matBazaarSeerGolemplains.mat").WaitForCompletion().SetFloat("_Boost", 1.5f);
            Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/bazaar/matBazaarSeerGoolake.mat").WaitForCompletion().SetFloat("_Boost", 1.5f);
            Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC1/ancientloft/matBazaarSeerAncientloft.mat").WaitForCompletion().SetFloat("_Boost", 2f);
            Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/lemuriantemple/matBazaarSeerLemurianTemple.mat").WaitForCompletion().SetFloat("_Boost", 2.5f);
            Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitat/matBazaarSeerHabitat.mat").WaitForCompletion().SetFloat("_Boost", 2.5f);


            #endregion
        }

        private static void FancyBazaarPortalChange(On.RoR2.SeerStationController.orig_SetRunNextStageToTarget orig, SeerStationController self)
        {
            bool isbazaar = false;
            if (SceneInfo.instance && SceneInfo.instance.sceneDef.baseSceneName == "bazaar")
            {
                isbazaar = true;
                GameObject ShopPortal = GameObject.Find("/PortalShop");
                SceneDef tempscenedef = SceneCatalog.GetSceneDef((SceneIndex)self.NetworktargetSceneDefIndex);
                //Debug.LogWarning(tempscenedef);

                if (tempscenedef && ShopPortal)
                {
                    ShopPortal.transform.localScale = new Vector3(0.7f, 1.26f, 0.7f);
                    ShopPortal.transform.position = new Vector3(12.88f, -5.53f, -7.34f);
                    GameObject portalPrefab = null;
                    GameObject temp = null;
                    switch (tempscenedef.baseSceneName)
                    {
                        case "goldshores":
                            portalPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalGoldshores/PortalGoldshores.prefab").WaitForCompletion();
                            break;
                        case "mysteryspace":
                        case "limbo":
                            portalPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalMS/PortalMS.prefab").WaitForCompletion();
                            break;
                        case "artifactworld":
                        case "artifactworld01":
                        case "artifactworld02":
                        case "artifactworld03":
                            ShopPortal.transform.localPosition = new Vector3(12.88f, 0f, -7.34f);
                            portalPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab").WaitForCompletion();
                            break;
                        case "arena":
                            portalPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArena/PortalArena.prefab").WaitForCompletion();
                            break;
                        case "voidstage":
                            portalPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion();
                            break;
                        case "voidraid":
                            portalPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion();
                            break;
                        case "lemuriantemple":
                        case "habitat":
                        case "habitatfall":
                        case "meridian":
                            portalPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion();
                            break;
                        default:
                            portalPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalShop/PortalShop.prefab").WaitForCompletion();
                            break;
                    }
                    if (portalPrefab != null)
                    {
                        temp = Object.Instantiate(portalPrefab, ShopPortal.transform.position, ShopPortal.transform.rotation);
                        NetworkServer.Spawn(temp);
                        temp.GetComponent<SceneExitController>().destinationScene = tempscenedef;
                        temp.GetComponent<SceneExitController>().useRunNextStageScene = false;
                        temp.name = ShopPortal.name;
                        Object.Destroy(ShopPortal);
                    }

                }
            }
            orig(self);
        }

        private static void SeerStageName_SceneChanged(On.RoR2.SeerStationController.orig_OnTargetSceneChanged orig, SeerStationController self, SceneDef targetScene)
        {
            orig(self, targetScene);
            if (WConfig.cfgLunarSeerName.Value == false)
            {
                return;
            }
            if (targetScene != null)
            {
                SeerName(self.gameObject, targetScene.sceneDefIndex);
            }
        }

        //Client sided not whatever weird
        public static void SeerName(GameObject self, SceneIndex index)
        {
            PurchaseTokenOverwrite overwrite = self.GetComponent<PurchaseTokenOverwrite>();
            if (overwrite == null)
            {
                overwrite = self.AddComponent<PurchaseTokenOverwrite>();
            }

            string temp = Language.GetString(SceneCatalog.GetSceneDef(index).nameToken);
            temp = temp.Replace("Hidden Realm: ", "");
            overwrite.contextToken = Language.GetString("BAZAAR_SEER_CONTEXT") + " of " + temp;
            overwrite.displayNameToken = Language.GetString("BAZAAR_SEER_NAME") + " (" + temp + ")";
        }


        private static void SeerStageName_Start(On.RoR2.SeerStationController.orig_OnStartClient orig, SeerStationController self)
        {
            orig(self);
            if (WConfig.cfgLunarSeerName.Value == false)
            {
                return;
            }
            if (self.NetworktargetSceneDefIndex != -1)
            {
                SeerName(self.gameObject, (SceneIndex)self.NetworktargetSceneDefIndex);
            }
        }
    }

}
