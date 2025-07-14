using RoR2;
using RoR2.Stats;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    public class InstantiateModelParams : MonoBehaviour
    {
        public Vector3 CameraPosition = new Vector3(-1.5f, 1f, 3f);
        public Vector3 FocusPosition = new Vector3(0f, 1f, 0f);

    }
    internal class LogFixes
    {
        public static void Start()
        {
            GameObject DevotedLemurian = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBody.prefab").WaitForCompletion();
            GameObject DevotedLemurianElder = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBruiserBody.prefab").WaitForCompletion();
            DevotedLemurian.GetComponent<DeathRewards>().logUnlockableDef = null;
            DevotedLemurianElder.GetComponent<DeathRewards>().logUnlockableDef = null;

            On.RoR2.Stats.StatManager.OnServerGameOver += CountEclipseSimuAsWins;

            //NewtShrine missing in Log
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/bazaar").dioramaPrefab.AddComponent<ModelPanelParameters>().gameObject.AddComponent<ModelPanelParameters>();

            //VoidMegaCrab too zoomed in
            Addressables.LoadAssetAsync<GameObject>(key: "097b0e271757ce24581d4a8983d2c941").WaitForCompletion().transform.GetChild(0).GetChild(3).GetComponent<ModelPanelParameters>().maxDistance = 40;


            //Sulfur Pools Diagram is Red instead of Yellow ???
            GameObject SulfurpoolsDioramaDisplay = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/sulfurpools/SulfurpoolsDioramaDisplay.prefab").WaitForCompletion();
            MeshRenderer SPDiaramaRenderer = SulfurpoolsDioramaDisplay.transform.GetChild(2).GetComponent<MeshRenderer>();
            Material SPRingAltered = Object.Instantiate(SPDiaramaRenderer.material);
            SPRingAltered.SetTexture("_SnowTex", Addressables.LoadAssetAsync<Texture2D>(key: "RoR2/DLC1/sulfurpools/texSPGroundDIFVein.tga").WaitForCompletion());
            SPDiaramaRenderer.material = SPRingAltered;

            //Way too zoomed in
            ModelPanelParameters VoidStageDiorama = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/voidstage/VoidStageDiorama.prefab").WaitForCompletion().GetComponent<ModelPanelParameters>();
            VoidStageDiorama.minDistance = 60;
            VoidStageDiorama.maxDistance = 320;

            //Way too zoomed out
            ModelPanelParameters PickupDevilHorns = Addressables.LoadAssetAsync<GameObject>(key: "b3e07eca6dc3dae4f97bc2c4fba31ba6").WaitForCompletion().GetComponent<ModelPanelParameters>();
            PickupDevilHorns.minDistance = 0.35f;
            PickupDevilHorns.maxDistance = 2;
            PickupDevilHorns.cameraPositionTransform.localPosition = new Vector3(-0.0047f, 0.35f, -0.0111f);
            PickupDevilHorns.cameraPositionTransform.localPosition = new Vector3(0f, 0.35f, 0f);

            //Default fallback
            On.RoR2.UI.ModelPanel.CameraFramingCalculator.GetCharacterThumbnailPosition += AddOrSetDefaultModelPanelParamsIfMissing;
            On.RoR2.ModelPanelParameters.OnDrawGizmos += SetValuesForMissingPanelParams;

        }

        private static void CountEclipseSimuAsWins(On.RoR2.Stats.StatManager.orig_OnServerGameOver orig, Run run, GameEndingDef gameEndingDef)
        {
            orig(run,gameEndingDef);
            if (gameEndingDef.isWin && run.GetType() != typeof(Run) && run.GetType() != typeof(WeeklyRun))
            {
                foreach (PlayerStatsComponent playerStatsComponent in PlayerStatsComponent.instancesList)
                {
                    if (playerStatsComponent.playerCharacterMasterController.isConnected)
                    {
                        StatSheet currentStats = playerStatsComponent.currentStats;
                        PerBodyStatDef totalWins = PerBodyStatDef.totalWins;
                        GameObject bodyPrefab = playerStatsComponent.characterMaster.bodyPrefab;
                        currentStats.PushStatValue(totalWins.FindStatDef(((bodyPrefab != null) ? bodyPrefab.name : null) ?? ""), 1UL);
                    }
                }
            }
        }

        private static void AddOrSetDefaultModelPanelParamsIfMissing(On.RoR2.UI.ModelPanel.CameraFramingCalculator.orig_GetCharacterThumbnailPosition orig, RoR2.UI.ModelPanel.CameraFramingCalculator self, float fov)
        {
            ModelPanelParameters component = self.modelInstance.GetComponent<ModelPanelParameters>();
            if (component)
            {
                component.OnDrawGizmos();
            }
            orig(self, fov);

        }

        private static void SetValuesForMissingPanelParams(On.RoR2.ModelPanelParameters.orig_OnDrawGizmos orig, ModelPanelParameters self)
        {

            if (self.cameraPositionTransform == null && self.focusPointTransform == null)
            {
                var fallback = self.GetComponent<InstantiateModelParams>();
                if (fallback == null)
                {
                    fallback = self.gameObject.AddComponent<InstantiateModelParams>();
                }


                GameObject cameraPos = new GameObject("cameraPos");
                cameraPos.transform.SetParent(self.transform, false);
                cameraPos.transform.localPosition = fallback.CameraPosition;
                self.cameraPositionTransform = cameraPos.transform;

                GameObject focusPoint = new GameObject("focusPoint");
                focusPoint.transform.SetParent(self.transform, false);
                focusPoint.transform.localPosition = fallback.FocusPosition;
                self.focusPointTransform = focusPoint.transform;

                if (self.minDistance == 1 && self.maxDistance == 10)
                {
                    self.minDistance = 2;
                    self.maxDistance = 8;
                }


            }
            orig(self);
        }


    }

}