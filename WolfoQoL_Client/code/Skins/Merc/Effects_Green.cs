using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client
{
    public class Merc_Green
    {

        public static Material matMercEnergized_Green = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matMercEnergized"));
        //public static Material matMercEvisTarget_Green = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matMercEvisTarget"));

        public static Material matHuntressFlashBright_Green = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashBright"));
        public static Material matHuntressFlashExpanded_Green = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashExpanded"));

        public static GameObject MercFocusedAssaultOrbEffect_Green = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/MercFocusedAssaultOrbEffect"), "MercFocusedAssaultOrbEffect_Green", false);

        public static GameObject OmniImpactVFXSlashMerc_Green = null; //Primary, Secondary1, Secondary2
        public static GameObject MercSwordSlash_Green = null; //Primary
        public static GameObject MercSwordFinisherSlash_Green = null; //Primary, Special2

        public static GameObject MercSwordSlashWhirlwind_Green = null;  //Secondary1
        public static GameObject MercDashHitOverlay_Green = null; //Utility1, Utility2

        public static GameObject HuntressBlinkEffect_Green = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/HuntressBlinkEffect"), "HuntressBlinkEffect_Green", false); //Special1 (Enter)

        public static GameObject HuntressFireArrowRain_Green = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/HuntressFireArrowRain"), "HuntressFireArrowRain_Green", false); //Special1 (Attack)

        public static GameObject OmniImpactVFXSlashMercEvis_Green = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/omnieffect/OmniImpactVFXSlashMercEvis"), "OmniImpactVFXSlashMercEvis_Green", false); //Special1 (Attack)


        public static GameObject MercSwordUppercutSlash_Green = null;  //Secondary2
        public static GameObject ImpactMercFocusedAssault_Green = null;  //Utility2
        public static GameObject ImpactMercAssaulter_Green = null;  //Utility2
        public static GameObject MercAssaulterEffect_Green = null;  //Utility2

        public static GameObject EvisProjectileGhost_Green = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisProjectileGhost"), "EvisProjectileGhost_Green", false);
        public static GameObject EvisOverlapProjectileGhost_Green = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisOverlapProjectileGhost"), "EvisOverlapProjectileGhost_Green", false);

        public static GameObject ImpactMercEvis_Green = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/ImpactMercEvis"), "ImpactMercEvis_Green", false);


        public static GameObject MercExposeEffect_Green;
        public static GameObject MercExposeConsumeEffect_Green;


        public static void Start()
        {
            MercExposeEffect_Green = PrefabAPI.InstantiateClone(Merc_Blue.MercExposeEffect, "MercExposeEffect_Green", false);
            MercExposeConsumeEffect_Green = PrefabAPI.InstantiateClone(Merc_Blue.MercExposeConsumeEffect, "MercExposeConsumeEffect_Green", false);

            MercSwordSlash_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion(), "MercSwordSlash_Green", false);

            //MercSwordFinisherSlash = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordFinisherSlash.prefab").WaitForCompletion();
            MercSwordFinisherSlash_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordFinisherSlash.prefab").WaitForCompletion(), "MercSwordFinisherSlash_Green", false);

            //MercSwordSlashWhirlwind = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion();
            MercSwordSlashWhirlwind_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion(), "MercSwordSlashWhirlwind_Green", false);


            //MercSwordUppercutSlash = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion();
            MercSwordUppercutSlash_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion(), "MercSwordUppercutSlash_Green", false);


            //OmniImpactVFXSlashMerc = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion();
            OmniImpactVFXSlashMerc_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion(), "OmniImpactVFXSlashMerc_Green", false);


            //OmniImpactVFXSlashMerc = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercAssaulterEffect.prefab").WaitForCompletion();
            MercAssaulterEffect_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercAssaulterEffect.prefab").WaitForCompletion(), "MercAssaulterEffect_Green", false);
            //ContentAddition.AddEffect(MercAssaulterEffect_Green);

            MercDashHitOverlay_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercDashHitOverlay.prefab").WaitForCompletion(), "MercDashHitOverlay_Green", false);
            //ContentAddition.AddEffect(MercDashHitOverlay_Green);

            ImpactMercAssaulter_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercAssaulter.prefab").WaitForCompletion(), "ImpactMercAssaulter_Green", false);


            ImpactMercFocusedAssault_Green = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercFocusedAssault.prefab").WaitForCompletion(), "ImpactMercFocusedAssault_Green", false);


            //AddEffects_ServerSided();
        }

        public static Material matMercIgnition_Green = null;
        [Obsolete]
        public static void Merc_GreenEffects()
        {

            Material matHuntressSwipe_Green;
            Material matHuntressCharged_Green;

            Material matMercDelayedBillboard2_Green = null;
            Material matMercFocusedAssaultIcon_Green = null;
            Material matMercExposedBackdrop_Green = null;

            Material matMercSwipe1_Green = null;
            Material matMercSwipe2_Green = null;
            Material matMercSwipe3_Green = null;

            Material matMercExposedSlash_Green = null;
            Material matOmniHitspark3Merc_Green = null;
            Material matOmniRadialSlash1Merc_Green = null;
            Material matOmniHitspark4Merc_Green = null;
            Material matMercExposed_Green;

            Material matMercHologram_Green = LegacyResourcesAPI.Load<Material>("materials/matMercHologram");

            matHuntressFlashBright_Green.SetColor("_TintColor", new Color(0.6f, 1.3f, 0.6f, 1f));//0.0191 1.1386 1.2973 1 
            matHuntressFlashExpanded_Green.SetColor("_TintColor", new Color(0.2f, 0.58f, 0.2f, 1f));//0 0.4367 0.5809 1

            Texture2D texRampFallboots_Green = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampFallbootsGreen.png");
            texRampFallboots_Green.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampMercDust_Green = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampMercDustGreen.png");
            texRampMercDust_Green.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampHuntressSoft_Green = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampHuntressSoftGreen.png");
            texRampHuntressSoft_Green.wrapMode = TextureWrapMode.Clamp;
            texRampHuntressSoft_Green.filterMode = FilterMode.Point;

            Texture2D texRampHuntress_Green = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampHuntressGreen.png");
            texRampHuntress_Green.wrapMode = TextureWrapMode.Clamp;
            texRampHuntress_Green.filterMode = FilterMode.Point;

            ParticleSystemRenderer MercSwordSlash_GreenRenderer0 = MercSwordSlash_Green.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercSwipe1_Green = UnityEngine.Object.Instantiate(MercSwordSlash_GreenRenderer0.material);
            matMercSwipe1_Green.SetTexture("_RemapTex", texRampFallboots_Green);
            matMercSwipe1_Green.SetColor("_TintColor", new Color(0, 1, 0, 1));        //Default Color : {r: 0, g: 0.314069, b: 1, a: 1}
            MercSwordSlash_GreenRenderer0.material = matMercSwipe1_Green;

            //Child 0 has weird TracerBright
            ParticleSystem MercSwordFinisherSlash_GreenParticle0 = MercSwordFinisherSlash_Green.transform.GetChild(0).GetComponent<ParticleSystem>();
            MercSwordFinisherSlash_GreenParticle0.startColor = new Color(0.2f, 1f, 0.2f, 1);
            ParticleSystemRenderer MercSwordFinisherSlash_GreenRenderer1 = MercSwordFinisherSlash_Green.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercSwipe2_Green = UnityEngine.Object.Instantiate(MercSwordFinisherSlash_GreenRenderer1.material);
            matMercSwipe2_Green.SetTexture("_RemapTex", texRampFallboots_Green);
            matMercSwipe2_Green.SetColor("_TintColor", new Color(0.22f, 1, 0.22f, 1));  //Default Color r: 0.3632075, g: 0.6593511, b: 1, a: 1
            MercSwordFinisherSlash_GreenRenderer1.material = matMercSwipe2_Green;

            ParticleSystemRenderer MercSwordSlashWhirlwind_GreenRenderer0 = MercSwordSlashWhirlwind_Green.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            MercSwordSlashWhirlwind_GreenRenderer0.material = matMercSwipe1_Green;

            ParticleSystemRenderer MercSwordUppercutSlash_GreenRenderer0 = MercSwordUppercutSlash_Green.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            ParticleSystemRenderer MercSwordUppercutSlash_GreenRenderer1 = MercSwordUppercutSlash_Green.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            MercSwordUppercutSlash_GreenRenderer0.material = matMercSwipe2_Green;
            MercSwordUppercutSlash_GreenRenderer1.material = matMercSwipe1_Green;




            OmniEffect OmniImpactVFXSlashMerc_GreenOmniEffect = OmniImpactVFXSlashMerc_Green.GetComponent<OmniEffect>();


            //Material matOmniHitspark4Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMerc_GreenOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial);
            //OmniImpactVFXSlashMerc_GreenOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4Merc;

            Material matOmniRadialSlash1Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMerc_GreenOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1Merc.SetTexture("_RemapTex", texRampMercDust_Green);
            OmniImpactVFXSlashMerc_GreenOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1Merc;

            Material matOmniHitspark3Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMerc_GreenOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniHitspark3Merc.SetTexture("_RemapTex", texRampMercDust_Green);
            OmniImpactVFXSlashMerc_GreenOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3Merc;

            Material matOmniHitspark2Merc_Green = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMerc_GreenOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial);
            matOmniHitspark2Merc_Green.SetTexture("_RemapTex", texRampMercDust_Green);
            OmniImpactVFXSlashMerc_GreenOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2Merc_Green;

            ParticleSystem OmniImpactVFXSlashMerc_GreenParticle1 = OmniImpactVFXSlashMerc_Green.transform.GetChild(1).GetComponent<ParticleSystem>(); //matOmniHitspark3 (Instance)
            OmniImpactVFXSlashMerc_GreenParticle1.startColor = new Color(0.3f, 0.7264f, 0.3f, 1); //Default C0lor 0 0.7264 0.7039 1

            ParticleSystem OmniImpactVFXSlashMerc_GreenParticle2 = OmniImpactVFXSlashMerc_Green.transform.GetChild(2).GetComponent<ParticleSystem>(); //matGenericFlash (Instance)
            OmniImpactVFXSlashMerc_GreenParticle2.startColor = new Color(0.2f, 0.9333f, 0.2f, 1); //0 0.4951 0.9333 1

            ParticleSystem OmniImpactVFXSlashMerc_GreenParticle3 = OmniImpactVFXSlashMerc_Green.transform.GetChild(3).GetComponent<ParticleSystem>(); //matTracerBright (Instance)
            OmniImpactVFXSlashMerc_GreenParticle3.startColor = new Color(0.4245f, 0.5f, 0.0501f, 1); //0.3854 0.4245 0.0501 1





            //Figure out if start color needs to actually be changed because they all use it
            ParticleSystemRenderer MercAssaulterEffect_GreenRenderer5 = MercAssaulterEffect_Green.transform.GetChild(5).GetComponent<ParticleSystemRenderer>();
            matMercIgnition_Green = UnityEngine.Object.Instantiate(MercAssaulterEffect_GreenRenderer5.material);
            matMercIgnition_Green.SetTexture("_RemapTex", texRampHuntress_Green);
            matMercIgnition_Green.SetColor("_TintColor", new Color(0.06f, 0.8867924f, 0.06f, 1)); //{r: 0, g: 0.1362783, b: 0.8867924, a: 1}
            MercAssaulterEffect_GreenRenderer5.material = matMercIgnition_Green;

            MercAssaulterEffect_Green.transform.GetChild(6).GetComponent<ParticleSystem>().startColor = new Color(0.3f, 1f, 0.3f, 1); //0 0.4409 0.9811 1
            MercAssaulterEffect_Green.transform.GetChild(8).GetComponent<Light>().color = new Color(0.3765f, 0.9804f, 0.4868f, 1); //0.3765 0.4868 0.9804 1
            MercAssaulterEffect_Green.transform.GetChild(9).GetComponent<TrailRenderer>().material = matMercSwipe1_Green;
            MercAssaulterEffect_Green.transform.GetChild(10).GetChild(2).GetComponent<TrailRenderer>().material = matMercIgnition_Green;
            MercAssaulterEffect_Green.transform.GetChild(10).GetChild(3).GetComponent<TrailRenderer>().material = matMercIgnition_Green;




            ParticleSystem particleSystem = ImpactMercAssaulter_Green.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.45f, 1f, 0.45f, 1f);//0.3538 0.6316 1 1
            particleSystem = ImpactMercAssaulter_Green.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.575f, 1f, 0.575f, 1f);//0.467 0.7022 1 1

            ParticleSystemRenderer particleSystemRenderer = ImpactMercAssaulter_Green.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercSwipe1_Green;




            particleSystem = ImpactMercFocusedAssault_Green.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.45f, 1f, 0.45f, 0.2667f);//0.3538 0.6316 1 0.2667
            particleSystem = ImpactMercFocusedAssault_Green.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.26f, 0.934f, 0.26f, 1f);//0.0925 0.4637 0.934 1
            particleSystemRenderer = ImpactMercFocusedAssault_Green.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercSwipe3_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercSwipe3_Green.SetTexture("_RemapTex", texRampMercDust_Green);
            particleSystemRenderer.material = matMercSwipe3_Green;



            particleSystemRenderer = MercFocusedAssaultOrbEffect_Green.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercDelayedBillboard2_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercDelayedBillboard2_Green.SetTexture("_RemapTex", texRampMercDust_Green);
            particleSystemRenderer.material = matMercDelayedBillboard2_Green;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Green.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercFocusedAssaultIcon_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercFocusedAssaultIcon_Green.SetTexture("_RemapTex", texRampMercDust_Green);
            particleSystemRenderer.material = matMercFocusedAssaultIcon_Green;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Green.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercExposedBackdrop_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            //matMercExposedBackdrop = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposedBackdrop_Green.SetColor("_TintColor", new Color(-2f, 3f, -2f, 0.9f));
            particleSystemRenderer.material = matMercExposedBackdrop_Green;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Green.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            matMercExposedSlash_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposedSlash_Green.SetTexture("_RemapTex", texRampHuntress_Green);
            matMercExposedSlash_Green.SetColor("_TintColor", new Color(0.06f, 0.8868f, 0.06f, 1)); //r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposedSlash_Green;


            matMercEnergized_Green.SetTexture("_RemapTex", texRampHuntressSoft_Green);
            matMercEnergized_Green.SetColor("_TintColor", new Color(0.35f, 1.8f, 0.35f, 1));



            //HuntressBlinkEffect_Green
            particleSystem = HuntressBlinkEffect_Green.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.154f, 0.6324f, 0.154f, 1f);//0.1534 0.1567 0.6324 1
            //particleSystem.colorOverLifetime.SetPropertyValue<bool>("enabled", false);
            var stupid = particleSystem.colorOverLifetime;
            stupid.enabled = false;
            //particleSystem.SetPropertyValue("colorOverLifetime", stupid);

            Light light = HuntressBlinkEffect_Green.transform.GetChild(0).GetChild(2).GetComponent<Light>();
            light.color = new Color(0.6f, 1f, 0.6f, 1); //0.2721 0.9699 1 1

            particleSystemRenderer = HuntressBlinkEffect_Green.transform.GetChild(0).GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressSwipe_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matHuntressSwipe_Green.SetTexture("_RemapTex", texRampHuntressSoft_Green);
            particleSystemRenderer.material = matHuntressSwipe_Green;

            particleSystemRenderer = HuntressBlinkEffect_Green.transform.GetChild(0).GetChild(5).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Green;


            //HuntressFireArrowRain_Green
            particleSystemRenderer = HuntressFireArrowRain_Green.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Green;

            particleSystemRenderer = HuntressFireArrowRain_Green.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Green;

            particleSystemRenderer = HuntressFireArrowRain_Green.transform.GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressCharged_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matHuntressCharged_Green.SetTexture("_RemapTex", texRampHuntress_Green);
            particleSystemRenderer.material = matHuntressCharged_Green;

            light = HuntressFireArrowRain_Green.transform.GetChild(5).GetComponent<Light>();
            light.color = new Color(0.55f, 1f, 0.55f, 1f); //0.3456 0.7563 1 1


            //OmniImpactVFXSlashMercEvis_Green
            OmniEffect omniEffect = OmniImpactVFXSlashMercEvis_Green.GetComponent<OmniEffect>();

            omniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4Merc_Green;




            matOmniRadialSlash1Merc_Green = UnityEngine.Object.Instantiate(omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1Merc_Green.SetTexture("_RemapTex", texRampMercDust_Green);

            omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1Merc_Green;


            omniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3Merc_Green;
            omniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2Merc_Green;

            particleSystemRenderer = OmniImpactVFXSlashMercEvis_Green.transform.GetChild(7).GetComponent<ParticleSystemRenderer>();
            matMercHologram_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercHologram_Green.SetTexture("_RemapTex", texRampFallboots_Green);
            matMercHologram_Green.SetColor("_TintColor", new Color(-0.25f, 1.825f, 0f, 1f));//0.2842 0.4328 1.826 1
            particleSystemRenderer.material = matMercHologram_Green;


            EvisProjectileGhost_Green.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2_Green;
            EvisProjectileGhost_Green.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Green;
            EvisProjectileGhost_Green.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercIgnition_Green;
            EvisProjectileGhost_Green.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(0f, 1f, 0f, 1f); //0 0.5827 1 1
            EvisProjectileGhost_Green.transform.GetChild(5).GetComponent<Light>().color = new Color(0.3f, 1f, 0.3f, 1f); //0.1274 0.4704 1 1


            EvisOverlapProjectileGhost_Green.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2_Green;
            EvisOverlapProjectileGhost_Green.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matOmniRadialSlash1Merc_Green;
            EvisOverlapProjectileGhost_Green.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matOmniHitspark2Merc_Green;
            EvisOverlapProjectileGhost_Green.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(0.51f, 1f, 0.51f, 1f);//0.3066 0.7276 1 1
            EvisOverlapProjectileGhost_Green.transform.GetChild(4).GetComponent<Light>().color = new Color(0.3f, 1f, 0.3f, 1f);//0.1274 0.4704 1 1
            EvisOverlapProjectileGhost_Green.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Green;
            EvisOverlapProjectileGhost_Green.transform.GetChild(6).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Green;

            ImpactMercEvis_Green.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Green;
            ImpactMercEvis_Green.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Green;
            ImpactMercEvis_Green.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Green;
            ImpactMercEvis_Green.transform.GetChild(3).GetComponent<Light>().color = new Color(0.425f, 1f, 0.425f, 1f);//0 0.8542 1 1





            particleSystemRenderer = MercExposeEffect_Green.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            //matMercExposed = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposed_Green = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposed_Green.SetTexture("_RemapTex", texRampHuntress_Green);
            matMercExposed_Green.SetColor("_TintColor", new Color(-0.1f, 1f, -0.1f, 1f));//r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposed_Green;
            particleSystemRenderer = MercExposeEffect_Green.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedBackdrop_Green;


            particleSystemRenderer = MercExposeConsumeEffect_Green.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposed_Green;
            particleSystemRenderer = MercExposeConsumeEffect_Green.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedSlash_Green;


            MercExposeEffect_Green.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Green;
            MercExposeEffect_Green.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.100f, 0.23f, 0.080f, 0.29f);//0.1335 0.1455 0.2264 0.3412
            MercExposeEffect_Green.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop_Green;

            MercExposeConsumeEffect_Green.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Green;
            MercExposeConsumeEffect_Green.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop_Green;
            MercExposeConsumeEffect_Green.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.2f, 0.35f, 0.2f, 0.175f);

            MercExposeConsumeEffect_Green.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Green;

            //ContentAddition.AddEffect(MercSwordFinisherSlash_Green); //Game tries to spawn this as an effect even tho it isn't one

        }

    }

}
