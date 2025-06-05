using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.Skills;

namespace WolfoFixes
{
    public class Visuals
    {


        public static void Start()
        {
      
            FixTitan();


            //Scope Alpha fix
            GameObject RailgunnerScopeHeavyOverlay = Addressables.LoadAssetAsync<GameObject>(key: "db5a0c21c1f689c4292ae5e292fd4f0e").WaitForCompletion();
            UnityEngine.UI.RawImage scope = RailgunnerScopeHeavyOverlay.transform.GetChild(1).GetComponent<UnityEngine.UI.RawImage>();
            scope.color = scope.color.AlphaMultiplied(0.6f);
            GameObject RailgunnerScopeLightOverlay = Addressables.LoadAssetAsync<GameObject>(key: "c305c2dadaa35d840bd91dd48987c55e").WaitForCompletion();
            scope = RailgunnerScopeHeavyOverlay.transform.GetChild(1).GetComponent<UnityEngine.UI.RawImage>();
            scope.color = scope.color.AlphaMultiplied(0.6f);




            //Sulfur Pools Diagram is Red instead of Yellow for ???
            GameObject SulfurpoolsDioramaDisplay = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/sulfurpools/SulfurpoolsDioramaDisplay.prefab").WaitForCompletion();
            MeshRenderer SPDiaramaRenderer = SulfurpoolsDioramaDisplay.transform.GetChild(2).GetComponent<MeshRenderer>();
            Material SPRingAltered = Object.Instantiate(SPDiaramaRenderer.material);
            SPRingAltered.SetTexture("_SnowTex", Addressables.LoadAssetAsync<Texture2D>(key: "RoR2/DLC1/sulfurpools/texSPGroundDIFVein.tga").WaitForCompletion());
            SPDiaramaRenderer.material = SPRingAltered;

            ModelPanelParameters VoidStageDiorama = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/voidstage/VoidStageDiorama.prefab").WaitForCompletion().GetComponent<ModelPanelParameters>();
            VoidStageDiorama.minDistance = 60;
            VoidStageDiorama.maxDistance = 320;

            //Rachis Radius is slightly wrong, noticible on high stacks 
            GameObject RachisObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/DamageZoneWard");
            RachisObject.transform.GetChild(1).GetChild(2).GetChild(1).localScale = new Vector3(2f, 2f, 2f);

            //Too small plant normally
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Plant/InterstellarDeskPlant.prefab").WaitForCompletion().transform.GetChild(0).localScale = new Vector3(0.6f, 0.6f, 0.6f);

            //Unused like blue explosion so he doesn't use magma explosion ig, probably unused for a reason but it looks fine
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion().GetComponent<WormBodyPositions2>().blastAttackEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Junk/ElectricWorm/ElectricWormImpactExplosion.prefab").WaitForCompletion();



        }

        public static void FixTitan()
        {
            SkinDefParams titan0 = Addressables.LoadAssetAsync<SkinDefParams>(key: "8aa47f1e288b32f4abb8e63aea8c2ea0").WaitForCompletion();
            HG.ArrayUtils.Swap(titan0.rendererInfos, 19, 0);
            SkinDefParams titanG = Addressables.LoadAssetAsync<SkinDefParams>(key: "ea05e89f54cbdee409061420648b0cd9").WaitForCompletion();
            HG.ArrayUtils.Swap(titanG.rendererInfos, 19, 0);
  
        }
      

    
 


    }
 
}