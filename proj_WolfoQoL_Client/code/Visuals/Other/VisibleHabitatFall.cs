using RoR2;

using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoLibrary;


namespace WolfoQoL_Client
{
    public static class VisibleHabitatFall
    {
        public static bool Should()
        {
            var a = new SceneExitController();
            if (a.GetLoopedStageOrDefault(SceneList.Habitat) == SceneList.Habitat)
            {
                return false;
            }
            GameObject.Destroy(a);
            return true;
        }
        public static void LemurianTemple()
        {
            #region Material
            //Color LeafColor = new Color(1.8f, 0.8f, 2.3f, 2f) * 0.9f;
            Color LeafColor = new Color(0.6604f, 0.3208f, 0.529f, 1) * 1.1f;

 
            Material matBHFallEnvfxLeaves = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "58c26464c97ed5247914273e05f683c5").WaitForCompletion());
            Material matBHDistantTree_Billboard = Addressables.LoadAssetAsync<Material>(key: "75319e5dc7ca5ee47ae5608402e9e977").WaitForCompletion();
            Material matBHDistantTree = Addressables.LoadAssetAsync<Material>(key: "c631f00ba144f1648911a6d238b200df").WaitForCompletion();

            GameObject LTFallenLeaf = Addressables.LoadAssetAsync<GameObject>(key: "935ddd13feca6a04b8d7e3e9bc9701d2").WaitForCompletion();
            Material LTFallenLeaf00 = Object.Instantiate(LTFallenLeaf.transform.GetChild(0).GetComponent<MeshRenderer>().material);
            Material LTFallenLeaf01 = LTFallenLeaf00;

            LTFallenLeaf00.color = LeafColor;
            LTFallenLeaf00.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/LemurianTemple/LTFallenLeaf_Atlas.png");
            LTFallenLeaf00.SetColor("_HueVariation", new Color(1, 0.5f, 0.1f, 0.5f));
            LTFallenLeaf01.color = LeafColor;
            LTFallenLeaf01.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/LemurianTemple/LTFallenLeaf_Atlas.png");
            LTFallenLeaf01.SetColor("_HueVariation", new Color(1, 0.5f, 0.1f, 0.5f));
            matBHFallEnvfxLeaves.color = LeafColor;
            matBHFallEnvfxLeaves.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/LemurianTemple/BHDistantTreeP_Atlas.png");

            Material matBHFallTerrainVines = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "07c4ea42012852645847968d57881370").WaitForCompletion());
            matBHFallTerrainVines.SetFloat("_SnowBias", 0);
            #endregion

            #region Roots

            GameObject meshLTCeilingRoots = GameObject.Find("/HOLDER:Terrain/LTTerrain/meshLTCeilingRoots");
            meshLTCeilingRoots.GetComponent<MeshRenderer>().material = matBHFallTerrainVines; //matAncientLoft_BoulderInfiniteTower 


            GameObject LTColumnTGCVine = GameObject.Find("/HOLDER: ToggleGroups/HOLDER: TG FrontC/LTColumnTGCVine");
            if (LTColumnTGCVine)
            {
                LTColumnTGCVine.GetComponent<MeshRenderer>().material = matBHFallTerrainVines; //matAncientLoft_BoulderInfiniteTower 
            }


            #endregion


            #region Foliage
            GameObject Foliage = GameObject.Find("/HOLDER: Foliage");


            GameObject Weather = GameObject.Find("/Weather, LemurianTemple");

            Transform LeafParticles = Weather.transform.GetChild(5);
            ParticleSystemRenderer[] particleList = LeafParticles.GetComponentsInChildren<ParticleSystemRenderer>();
            foreach (ParticleSystemRenderer particle in particleList)
            {
                particle.material = matBHFallEnvfxLeaves;
            }
            //
            //
            Transform Leaves = Foliage.transform.GetChild(1);
            BillboardRenderer[] billboardList = Leaves.GetComponentsInChildren<BillboardRenderer>();
            foreach (BillboardRenderer renderer in billboardList)
            {
                renderer.billboard.material = matBHDistantTree_Billboard;
            }
            MeshRenderer[] rendererList = Leaves.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in rendererList)
            {
                switch (renderer.material.name)
                {
                    case "Leaves_0_LOD0 (Instance)":
                        renderer.material = matBHDistantTree;
                        break;
                    case "matBHDistantTree (Instance)":
                        renderer.material = matBHDistantTree;
                        break;
                }
            }
            //
            //



            //There's more fallen leaves in other holders


            Transform LeafPiles = Foliage.transform.GetChild(4);
            rendererList = LeafPiles.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in rendererList)
            {
                switch (renderer.material.name)
                {
                    case "Leaves_0_LOD0 (Instance)":
                        renderer.material = LTFallenLeaf00; //Different Lod0 leaves
                        break;
                    case "Leaves_0_LOD1 (Instance)":
                        renderer.material = LTFallenLeaf01;
                        break;
                }
            }


            GameObject Foliage2 = GameObject.Find("/HOLDER: ToggleGroups/HOLDER: TG BackA/TGBackB/LTFallenLeaf");
            GameObject Foliage3 = GameObject.Find("/HOLDER: ToggleGroups/HOLDER: TG FrontA");
            GameObject Foliage4 = GameObject.Find("/HOLDER: ToggleGroups/HOLDER: TG FrontC/LTFallenLeaf (9)");
            if (Foliage2)
            {
                Foliage2.transform.GetChild(0).GetComponent<MeshRenderer>().material = LTFallenLeaf00;
                Foliage2.transform.GetChild(1).GetComponent<MeshRenderer>().material = LTFallenLeaf01;
            }
            if (Foliage3)
            {
                Foliage3.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = LTFallenLeaf00;
                Foliage3.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = LTFallenLeaf01;
                Foliage3.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = LTFallenLeaf00;
                Foliage3.transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().material = LTFallenLeaf01;
            }
            if (Foliage4)
            {
                Foliage4.transform.GetChild(0).GetComponent<MeshRenderer>().material = LTFallenLeaf00;
                Foliage4.transform.GetChild(1).GetComponent<MeshRenderer>().material = LTFallenLeaf01;
            }
            #endregion
        }
        public static void PrimeMeridian()
        {
            #region Materials
            Material matBHFallPlatformSimple = Addressables.LoadAssetAsync<Material>(key: "11b42e952d609ef43a3f771e8cace014").WaitForCompletion();
            Material matBHFallTerrainVines = Addressables.LoadAssetAsync<Material>(key: "f5ed1a209186f114985eba4c17e7b537").WaitForCompletion();
            Material matBHFallDomeTrim = Addressables.LoadAssetAsync<Material>(key: "7a1c7e89aba14dd4bb118fee877fd603").WaitForCompletion();

            Material matBHDistantTree_Billboard = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "75319e5dc7ca5ee47ae5608402e9e977").WaitForCompletion());
            Material matBHDistantTree = Addressables.LoadAssetAsync<Material>(key: "947053d63d00a6b40adf4f1bd4274995").WaitForCompletion();
            #endregion

            GameObject Terrain = GameObject.Find("/HOLDER: Art/Terrain");
            Transform BHFoliage = Terrain.transform.GetChild(5);


            BillboardRenderer[] billboardList = BHFoliage.GetComponentsInChildren<BillboardRenderer>();
            foreach (BillboardRenderer renderer in billboardList)
            {
                renderer.billboard.material = matBHDistantTree_Billboard;
            }
            //Why does this one work lol
            MeshRenderer[] rendererList = BHFoliage.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in rendererList)
            {
                switch (renderer.material.name)
                {
                    case "matBHDistantTree (Instance)":
                        renderer.material = matBHDistantTree;
                        break;
                }
            }

            Transform T3Objects = Terrain.transform.GetChild(6);
            rendererList = T3Objects.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in rendererList)
            {
                switch (renderer.material.name)
                {
                    case "matBHPlatformSimple (Instance)":
                        renderer.material = matBHFallPlatformSimple;
                        break;
                    case "matBHFallTerrainVines (Instance)":
                        renderer.material = matBHFallTerrainVines;
                        break;
                    case "matBHDomeTrim (Instance)":
                        renderer.material = matBHFallDomeTrim;
                        break;
                }
            }
        }

    }

}
