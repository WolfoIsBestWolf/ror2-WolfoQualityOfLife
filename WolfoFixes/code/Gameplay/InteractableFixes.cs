using HG;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;
using MonoMod.Cil;
using System.Collections.Generic;

namespace WolfoFixes
{
    internal class InteractableFixes
    {
        public static void Start()
        {
            ShrineShapingFixes.Start();
            ShrineHalcyonFixes.Start();


            //Avoid duplicate Golemplains & BlackBeach
            //Avoid Special Stages removing a choice for 3rd Seers.
            On.RoR2.BazaarController.SetUpSeerStations += FixDuplicateAndEmptyThirdSeers;
            //For any mods adding extra seer stations.
            //Doubt it'd come up in vanilla.
            On.RoR2.SeerStationController.SetRunNextStageToTarget += BazaarDisableAllSeers;
        }

        private static void BazaarDisableAllSeers(On.RoR2.SeerStationController.orig_SetRunNextStageToTarget orig, SeerStationController self)
        {
            orig(self);
            if (BazaarController.instance)
            {
                for (int i = 0; i < BazaarController.instance.seerStations.Length; i++)
                {
                    BazaarController.instance.seerStations[i].GetComponent<PurchaseInteraction>().SetAvailable(false);
                }
            }
        }
        private static void FixDuplicateAndEmptyThirdSeers(On.RoR2.BazaarController.orig_SetUpSeerStations orig, BazaarController self)
        {
            orig(self);

            List<string> takenBaseScenes = new List<string>();
            List<SeerStationController> seersToFix = new List<SeerStationController>();
            foreach (SeerStationController seer in self.seerStations)
            {
                if (seer.targetSceneDefIndex != -1)
                {
                    string a = SceneCatalog.indexToSceneDef[seer.targetSceneDefIndex].baseSceneName;
                    //Debug.Log(a);
                    if (takenBaseScenes.Contains(a))
                    {
                        seersToFix.Add(seer);
                    }
                    else
                    {
                        takenBaseScenes.Add(a);
                    }
                }
                else
                {
                    seersToFix.Add(seer);
                }
            }
            if (seersToFix.Count > 0)
            {
                SceneDef nextStageScene = Run.instance.nextStageScene;
                List<SceneDef> list = new List<SceneDef>();
                if (nextStageScene != null)
                {
                    int stageOrder = nextStageScene.stageOrder;
                    foreach (SceneDef sceneDef in SceneCatalog.allSceneDefs)
                    {
                        if (sceneDef.stageOrder == stageOrder && (sceneDef.requiredExpansion == null || Run.instance.IsExpansionEnabled(sceneDef.requiredExpansion)) && self.IsUnlockedBeforeLooping(sceneDef))
                        {
                            if (!takenBaseScenes.Contains(sceneDef.baseSceneName))
                            {
                               // Debug.Log(sceneDef);
                                list.Add(sceneDef);
                            }
                        }
                    }
                }
                if (list.Count > 0)
                {
                    foreach (var seer in seersToFix)
                    {
                        Util.ShuffleList<SceneDef>(list, self.rng);
                        int index = list.Count - 1;
                        seer.SetTargetScene(list[index]);
                        list.RemoveAt(index);
                        seer.GetComponent<PurchaseInteraction>().SetAvailable(true);
                    }
                }
            }
 
        }
  
    }
    

}
