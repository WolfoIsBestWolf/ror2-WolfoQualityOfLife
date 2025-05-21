using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace WolfoQoL_Client
{
    public class OtherEnemies
    {
 
        public static void Start()
        {
            if (WConfig.SulfurPoolsSkin.Value)
            {
                GameObject BeetleBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Beetle/BeetleBody.prefab").WaitForCompletion();
                GameObject BeetleGuardBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Beetle/BeetleGuardBody.prefab").WaitForCompletion();
                GameObject BeetleQueenBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Beetle/BeetleQueen2Body.prefab").WaitForCompletion();
                SceneDef sulfurpools = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC1/sulfurpools/sulfurpools.asset").WaitForCompletion();
                //SceneIndex sulfur = SceneCatalog.FindSceneIndex("sulfurpools");

                BeetleBody.AddComponent<ChangeSkinOnStage>().sceneDef = sulfurpools;
                BeetleGuardBody.AddComponent<ChangeSkinOnStage>().sceneDef = sulfurpools;
                BeetleQueenBody.AddComponent<ChangeSkinOnStage>().sceneDef = sulfurpools;

            }


        }



    }
    public class ChangeSkinOnStage : MonoBehaviour
    {
        public SceneDef sceneDef;
        public SceneIndex SceneIndex;
        public uint localSkinIndex = 1;
        public void Start()
        {
            //if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.MixEnemy))
            if (SceneCatalog.mostRecentSceneDef == sceneDef)
            {
                GetComponent<CharacterBody>().skinIndex = localSkinIndex;
            }
        }
    }

}