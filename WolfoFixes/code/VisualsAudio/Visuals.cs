using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
            scope.m_Color = scope.color.AlphaMultiplied(0.7f);
            GameObject RailgunnerScopeLightOverlay = Addressables.LoadAssetAsync<GameObject>(key: "c305c2dadaa35d840bd91dd48987c55e").WaitForCompletion();
            scope = RailgunnerScopeLightOverlay.transform.GetChild(1).GetComponent<UnityEngine.UI.RawImage>();
            scope.m_Color = scope.color.AlphaMultiplied(0.7f);

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


            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/bazaar").dioramaPrefab.AddComponent<ModelPanelParameters>().gameObject.AddComponent<ModelPanelParameters>();

            On.RoR2.UI.ModelPanel.CameraFramingCalculator.GetCharacterThumbnailPosition += CameraFramingCalculator_GetCharacterThumbnailPosition;
            On.RoR2.ModelPanelParameters.OnDrawGizmos += ModelPanelParameters_OnDrawGizmos;

            Addressables.LoadAssetAsync<GameObject>(key: "097b0e271757ce24581d4a8983d2c941").WaitForCompletion().transform.GetChild(0).GetChild(3).GetComponent<ModelPanelParameters>().maxDistance = 40;

        }


        private static void CameraFramingCalculator_GetCharacterThumbnailPosition(On.RoR2.UI.ModelPanel.CameraFramingCalculator.orig_GetCharacterThumbnailPosition orig, RoR2.UI.ModelPanel.CameraFramingCalculator self, float fov)
        {
            ModelPanelParameters component = self.modelInstance.GetComponent<ModelPanelParameters>();
            if (component)
            {
                component.OnDrawGizmos();
            }
            orig(self, fov);

        }

        private static void ModelPanelParameters_OnDrawGizmos(On.RoR2.ModelPanelParameters.orig_OnDrawGizmos orig, ModelPanelParameters self)
        {

            if (self.cameraPositionTransform == null && self.focusPointTransform == null)
            {
                GameObject cameraPos = new GameObject("cameraPos");
                cameraPos.transform.SetParent(self.transform, false);
                cameraPos.transform.localPosition = new Vector3(-1.5f, 1f, 3f);
                self.cameraPositionTransform = cameraPos.transform;

                GameObject focusPoint = new GameObject("focusPoint");
                focusPoint.transform.SetParent(self.transform, false);
                self.focusPointTransform = focusPoint.transform;
                self.minDistance = 2;
                self.maxDistance = 8;

                if (self.name.StartsWith("mdlNewt"))
                {
                    self.minDistance = 5;
                    self.maxDistance = 15;
                    cameraPos.transform.localPosition = new Vector3(-1.5f, 4f, 3f);
                    focusPoint.transform.localPosition = new Vector3(0f, 3f, 0f);
                }
                else if (self.GetComponent<CharacterModel>() != null)
                {
                    focusPoint.transform.localPosition = new Vector3(0f, 1f, 0f);
                }
            }
            orig(self);
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