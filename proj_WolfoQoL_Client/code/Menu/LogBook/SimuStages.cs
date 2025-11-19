using R2API;
using RoR2;
using RoR2.Stats;
using RoR2.UI.LogBook;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using WolfoFixes;
using static WolfoQoL_Client.Help;

namespace WolfoQoL_Client
{

    public class LogSimuStages
    {



        public static void Add()
        {

            SceneCollection sgInfiniteTowerStageX = Addressables.LoadAssetAsync<SceneCollection>(key: "262363a2da6e02f4da3c752da7310e9e").WaitForCompletion();
            GameObject InfiniteTowerSafeWard = Addressables.LoadAssetAsync<GameObject>(key: "ba70f68b08210a84f86ff7a12a324263").WaitForCompletion();
            GameObject mdlVoidWardCrab = InfiniteTowerSafeWard.transform.GetChild(0).GetChild(0).gameObject;
            mdlVoidWardCrab.AddComponent<ModelPanelParameters>();

            //Are they tagged as require DLC1 at least
            for (int i = 0; i < sgInfiniteTowerStageX.sceneEntries.Length; i++)
            {
                SceneDef scene = sgInfiniteTowerStageX.sceneEntries[i].sceneDef;
                SceneDef normalScene = ITtoNormal(scene);
                scene.stageOrder = 5130 + normalScene.stageOrder;
                scene.shouldIncludeInLogbook = WConfig.SimuStagesInLog.Value;
                scene.dioramaPrefab = mdlVoidWardCrab;
                scene.dioramaPrefabAddress = new AssetReferenceGameObject("");
                scene.previewTexture = normalScene.previewTexture;
                scene.loreToken = string.Empty;

                string newNameToken = string.Format("MAP_{0}_NAME", scene.cachedName);
                LanguageAPI.Add(newNameToken, string.Format("{0} ({1})", new string[]
                {
                    Language.GetString("MAP_INFINITETOWER_TITLE"),
                    Language.GetString(normalScene.nameToken),
                }));
                scene.nameToken = newNameToken;
            }
            SceneDef itmoon = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC1/itmoon/itmoon.asset").WaitForCompletion();

            GameObject ITMoonDiorama = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "a03e9bece79327549b82cc12874868af").WaitForCompletion(), "ITMoonDiorama", false);

            Material matConstellationShellBlue = Addressables.LoadAssetAsync<Material>(key: "700e3cf101d42a04bb09174d67a90137").WaitForCompletion();
            Material matConstellationShellPurple = Addressables.LoadAssetAsync<Material>(key: "3dd74c91566eae840abf59a09d6748ea").WaitForCompletion();
            Material matConstellationShellYellow = Addressables.LoadAssetAsync<Material>(key: "0318c1c3351c71c4aaad528f5b76191c").WaitForCompletion();
            matConstellationShellBlue.SetFloat("_AlphaBoost", 46.8f);
            matConstellationShellPurple.SetFloat("_AlphaBoost", 46.8f);
            matConstellationShellYellow.SetFloat("_AlphaBoost", 46.8f);
            Material matConstelationSolid = Object.Instantiate(matConstellationShellPurple);
            matConstelationSolid.SetFloat("_AlphaBoost", 10000f);

            ITMoonDiorama.transform.localScale = Vector3.one;
            ITMoonDiorama.transform.GetChild(0).gameObject.SetActive(false);
            ITMoonDiorama.transform.GetChild(2).GetComponent<Renderer>().material = matConstelationSolid;
            ITMoonDiorama.transform.GetChild(3).GetComponent<Renderer>().material = matConstelationSolid;


            ModelPanelParameters panelParams = ITMoonDiorama.AddComponent<ModelPanelParameters>();
            var Params2 = ITMoonDiorama.AddComponent<InstantiateModelParams>();
            panelParams.maxDistance = 5;
            Params2.CameraPosition = new Vector3(0f, 2f, 3f);
            Params2.FocusPosition = new Vector3(0f, 2f, 0f);



            itmoon.dioramaPrefab = ITMoonDiorama;
            itmoon.dioramaPrefabAddress = new AssetReferenceGameObject("");
            Addressables.LoadAssetAsync<SceneDef>(key: "dda7dbdf013275747949dfc522842635").WaitForCompletion().previewTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SimuPreview/texGolemplainsPreview.png");
            Addressables.LoadAssetAsync<SceneDef>(key: "0a35bec906d067941a8a378b963e9e9d").WaitForCompletion().previewTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SimuPreview/texGoolakePreview.png");
            Addressables.LoadAssetAsync<SceneDef>(key: "568295b69af09c241968fa18ca37b56d").WaitForCompletion().previewTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SimuPreview/texAncientLoftPreview.png");
            Addressables.LoadAssetAsync<SceneDef>(key: "0ab15976715ddd6438e2e5bd6ddbe224").WaitForCompletion().previewTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SimuPreview/texFrozenwallPreview.png");
            Addressables.LoadAssetAsync<SceneDef>(key: "c33b24f6ebf27db49a5cfb88dbe9b8ff").WaitForCompletion().previewTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SimuPreview/texDampcavePreview.png");
            Addressables.LoadAssetAsync<SceneDef>(key: "6a2712b5c8cf36f44b34c128f2759522").WaitForCompletion().previewTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SimuPreview/texSkymeadowPreview.png");
            itmoon.previewTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SimuPreview/texMoonPreview.png");


            On.RoR2.UI.LogBook.LogBookController.GetStageStatus += ITStageIfCompletedOnce;
        }

        private static EntryStatus ITStageIfCompletedOnce(On.RoR2.UI.LogBook.LogBookController.orig_GetStageStatus orig, ref Entry entry, UserProfile viewerProfile)
        {
            SceneDef sceneDef = (entry.extraData as SceneDef);
            if (sceneDef.stageOrder > 5129)
            {
                if (viewerProfile.statSheet.HasUnlockable(WolfoLibrary.MissedContent.Survivors.VoidSurvivor.unlockableDef) || viewerProfile.statSheet.GetStatValueULong(PerStageStatDef.totalTimesCleared.FindStatDef(sceneDef.baseSceneName)) > 0U)
                {
                    return EntryStatus.Available;
                }
            }
            return orig(ref entry, viewerProfile);
        }
    }

}