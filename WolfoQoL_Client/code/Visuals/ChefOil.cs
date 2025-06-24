using RoR2;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client
{

    public class ChefOil : MonoBehaviour
    {
        public static void Start()
        {
            //The Oil is kinda lacking in the vfx department
            GameObject ChefImpactOilGhost = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Chef/ChefImpactOilGhost.prefab").WaitForCompletion();



            On.OilGhostController.DoGhostVisual += OilGhostController_DoGhostVisual;

            ChefImpactOilGhost.transform.GetChild(0).GetChild(1).gameObject.AddComponent<PlaySoundOnEvent>().soundEvent = "Play_scorchling_slagBomb_explo";
            ChefImpactOilGhost.transform.GetChild(0).GetChild(2).gameObject.AddComponent<PlaySoundOnEvent>().soundEvent = "Play_scorchling_slagBomb_explo";
            Destroy(ChefImpactOilGhost.transform.GetChild(0).GetChild(4).gameObject.GetComponent<AkEvent>());
        }

        private static void OilGhostController_DoGhostVisual(On.OilGhostController.orig_DoGhostVisual orig, OilGhostController self, bool ignited, bool boosted, bool frozen)
        {

            orig(self, ignited, boosted, frozen);
            if (ignited)
            {
                //FireMeatBallExplosion
                EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>(key: "02e06953c75c74746b1e1f957f804e37").WaitForCompletion(), new EffectData
                {
                    origin = self.transform.position,
                    scale = 15f,
                }, false);
            }
            else if (frozen)
            {
                //ChefIceBoxExplosionVFX
                //EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>(key: "d4f722af5c54e554b9ac663e3a37bef4").WaitForCompletion(), new EffectData
                EffectManager.SpawnEffect(Addressables.LoadAssetAsync<GameObject>(key: "d7fd97a7825c2644e83cea446b03e54b").WaitForCompletion(), new EffectData
                {
                    origin = self.transform.position,
                    scale = 7.5f,
                }, false);
            }
        }
    }

}
