using MonoMod.Cil;
using RoR2;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using RoR2.Stats;

namespace WolfoFixes
{
    public class LogFixes
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

        }

        private static void CountEclipseSimuAsWins(On.RoR2.Stats.StatManager.orig_OnServerGameOver orig, Run run, GameEndingDef gameEndingDef)
        {
            Debug.Log(run.GetType());
            Debug.Log(run.GetType() == typeof(Run));
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


    }

}