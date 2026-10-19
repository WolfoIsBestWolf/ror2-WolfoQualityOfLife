using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{
    public static class BrokenDrone
    {
        public static void Start()
        {
            /*if (!WConfig.cfgBrokenDroneVisual.Value)
            {
                return;
            }*/
            //Makes broken gunner drone a bit lighter
            //Makes broken gunner drones particle effect more centered so it isn't clipped into the ground
            Material matDroneBrokenGeneric = Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Drones/matDroneBrokenGeneric.mat").WaitForCompletion();
            #region
        
            GameObject Drone1Broken = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Drones/Drone1Broken.prefab").WaitForCompletion();

            Transform mdlDrone1 = Drone1Broken.transform.GetChild(0).GetChild(0);
            mdlDrone1.GetChild(3).localPosition = new Vector3(0, -1f, 0);
            mdlDrone1.GetChild(3).localRotation = Quaternion.identity;
            #endregion
        }

        public static void VisualThing()
        {
            Material matDroneBrokenGeneric = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC4/WakeSacrifice/SacrificeBody.prefab").WaitForCompletion();

            //matHaulerJailerRechargeCleanup

            foreach (DroneDef drone in DroneCatalog.droneDefs)
            {
                if (drone.droneBrokenSpawnCard)
                {
                    SkinDef[] skins = SkinCatalog.FindSkinsForBody(DroneCatalog.GetBodyIndexFromDroneIndex(drone.droneIndex));
                    if (skins.Length == 0)
                    {
                        continue;
                    }
                    SkinDef skin = skins[0];

                    ModelLocator broken = drone.droneBrokenSpawnCard.prefab.GetComponent<ModelLocator>();
                    if (broken == null)
                    {
                        continue;
                    }
                    Renderer brokenRender = broken.modelTransform.GetComponentInChildren<Renderer>();
                    Debug.Log(brokenRender);
                    Debug.Log(brokenRender.material);


                }


            }

        }
    
    }

}
