using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public class InteractableSkins
    {

        public static void Start()
        {

            /*Addressables.LoadAssetAsync<GameObject>(key: "5891e07c07cb11141ac34a5cd55e51ee").WaitForCompletion().transform.GetChild(1).GetChild(0).GetChild(1).gameObject.AddComponent<MegaDroneSkinner>();

            //Material matTrimSheetMetalLightSnow = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "2b04a7e517a51b546904c5c7d9b7e4d4").WaitForCompletion());
            Material matDroneBrokenGeneric = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "5eaf51b5b43efb14280d32de0efcd3b1").WaitForCompletion());

            matDroneBrokenGeneric.SetTexture("_SnowTex", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/General/texTrimSheetMetalDiffuseMediumWorn.png"));
            matDroneBrokenGeneric.SetFloat("_SnowBias", 0.2f);

            MegaDroneSkinner.mat = matDroneBrokenGeneric;*/


            SkinnedMeshRenderer CloakedChest = Addressables.LoadAssetAsync<GameObject>(key: "7d97551ee4c1e904b9a225abc63c3f0d").WaitForCompletion().transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            Material newCloaked = Object.Instantiate(CloakedChest.material);
            newCloaked.SetFloat("_Magnitude", 2);
            CloakedChest.material = newCloaked;

        }
    }

    /*public class MegaDroneSkinner : MonoBehaviour
    {
        public static Material mat;
        public void Start()
        {
            string name = SceneCatalog.mostRecentSceneDef.baseSceneName;
            if (name != "snowyforest" && name != "frozenwall")
            {
                Renderer renderer = GetComponent<Renderer>();
                renderer.material = mat;
            }

        }
    }*/

}