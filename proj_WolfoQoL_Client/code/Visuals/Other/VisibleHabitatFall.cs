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


            Material matBHFallEnvfxLeaves = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallEnvfxLeaves.mat").WaitForCompletion());
            Material matBHDistantTree_Billboard = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallDistantTreeBillboard.mat").WaitForCompletion();
            Material matBHDistantTree = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallDistantTree.mat").WaitForCompletion();
            Material LTFallenLeaf = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/lemuriantemple/Assets/LTFallenLeaf.spm").WaitForCompletion());


 
            LTFallenLeaf.color = LeafColor;
            LTFallenLeaf.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/LemurianTemple/LTFallenLeaf_Atlas.png");
            LTFallenLeaf.SetColor("_HueVariation", new Color(1, 0.5f, 0.1f, 0.5f));
            matBHFallEnvfxLeaves.color = LeafColor;
            matBHFallEnvfxLeaves.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/LemurianTemple/BHDistantTreeP_Atlas.png");

            Material matBHFallTerrainVines = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallTerrainVines.mat").WaitForCompletion());
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
                        renderer.material = LTFallenLeaf; //Different Lod0 leaves
                        break;
                    case "Leaves_0_LOD1 (Instance)":
                        renderer.material = LTFallenLeaf;
                        break;
                }
            }


            GameObject Foliage2 = GameObject.Find("/HOLDER: ToggleGroups/HOLDER: TG BackA/TGBackB/LTFallenLeaf");
            GameObject Foliage3 = GameObject.Find("/HOLDER: ToggleGroups/HOLDER: TG FrontA");
            GameObject Foliage4 = GameObject.Find("/HOLDER: ToggleGroups/HOLDER: TG FrontC/LTFallenLeaf (9)");
            if (Foliage2)
            {
                Foliage2.transform.GetChild(0).GetComponent<MeshRenderer>().material = LTFallenLeaf;
                Foliage2.transform.GetChild(1).GetComponent<MeshRenderer>().material = LTFallenLeaf;
            }
            if (Foliage3)
            {
                Foliage3.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material = LTFallenLeaf;
                Foliage3.transform.GetChild(0).GetChild(1).GetComponent<MeshRenderer>().material = LTFallenLeaf;
                Foliage3.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = LTFallenLeaf;
                Foliage3.transform.GetChild(1).GetChild(1).GetComponent<MeshRenderer>().material = LTFallenLeaf;
            }
            if (Foliage4)
            {
                Foliage4.transform.GetChild(0).GetComponent<MeshRenderer>().material = LTFallenLeaf;
                Foliage4.transform.GetChild(1).GetComponent<MeshRenderer>().material = LTFallenLeaf;
            }
            #endregion
        }
        public static void PrimeMeridian()
        {
            #region Materials
            Material matBHFallPlatformSimple = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallPlatformSimple.mat").WaitForCompletion();
            Material matBHFallTerrainVines = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallTerrainVines.mat").WaitForCompletion();
            Material matBHFallDomeTrim = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallDomeTrim.mat").WaitForCompletion();

            Material matBHDistantTree_Billboard = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallDistantTreeBillboard.mat").WaitForCompletion());
            Material matBHDistantTree = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/habitatfall/Assets/matBHFallDistantTree.mat").WaitForCompletion();
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
