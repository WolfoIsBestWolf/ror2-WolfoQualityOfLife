using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public static class Merc_Pink
    {

        public static Material matMercEnergized_Pink = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("Materials/matMercEnergized"));
        public static Material matMercEvisTarget_Pink = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matMercEvisTarget"));

        public static Material matHuntressFlashBright_Pink = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashBright"));
        public static Material matHuntressFlashExpanded_Pink = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashExpanded"));

        public static GameObject MercFocusedAssaultOrbEffect_Pink = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/MercFocusedAssaultOrbEffect"), "MercFocusedAssaultOrbEffect_Pink", false);

        public static GameObject OmniImpactVFXSlashMerc_Pink = null; //Primary, Secondary1, Secondary2
        public static GameObject MercSwordSlash_Pink = null; //Primary
        public static GameObject MercSwordFinisherSlash_Pink = null; //Primary, Special2

        public static GameObject MercSwordSlashWhirlwind_Pink = null;  //Secondary1
        public static GameObject MercDashHitOverlay_Pink = null; //Utility1, Utility2

        public static GameObject HuntressBlinkEffect_Pink = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/HuntressBlinkEffect"), "HuntressBlinkEffect_Pink", false); //Special1 (Enter)
        public static GameObject HuntressFireArrowRain_Pink = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/HuntressFireArrowRain"), "HuntressFireArrowRain_Pink", false); //Special1 (Attack)
        public static GameObject OmniImpactVFXSlashMercEvis_Pink = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/omnieffect/OmniImpactVFXSlashMercEvis"), "OmniImpactVFXSlashMercEvis_Pink", false); //Special1 (Attack)


        public static GameObject MercSwordUppercutSlash_Pink = null;  //Secondary2
        public static GameObject ImpactMercFocusedAssault_Pink = null;  //Utility2
        public static GameObject ImpactMercAssaulter_Pink = null;  //Utility2
        public static GameObject MercAssaulterEffect_Pink = null;  //Utility2

        public static GameObject EvisProjectileGhost_Pink = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisProjectileGhost"), "EvisProjectileGhost_Pink", false);
        public static GameObject EvisOverlapProjectileGhost_Pink = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisOverlapProjectileGhost"), "EvisOverlapProjectileGhost_Pink", false);

        public static GameObject ImpactMercEvis_Pink = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/ImpactMercEvis"), "ImpactMercEvis_Pink", false);


        public static GameObject MercExposeEffect_Pink;
        public static GameObject MercExposeConsumeEffect_Pink;


        public static void Start()
        {
            MercExposeEffect_Pink = PrefabAPI.InstantiateClone(Merc_Blue.MercExposeEffect, "MercExposeEffect_Pink", false);
            MercExposeConsumeEffect_Pink = PrefabAPI.InstantiateClone(Merc_Blue.MercExposeConsumeEffect, "MercExposeConsumeEffect_Pink", false);


            //Not Indexed
            MercSwordSlash_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion(), "MercSwordSlash_Pink", false);

            //Not Indexed
            MercSwordFinisherSlash_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordFinisherSlash.prefab").WaitForCompletion(), "MercSwordFinisherSlash_Pink", false);

            //.rootObject Body
            MercSwordSlashWhirlwind_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion(), "MercSwordSlashWhirlwind_Pink", false);


            MercSwordUppercutSlash_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion(), "MercSwordUppercutSlash_Pink", false);


            OmniImpactVFXSlashMerc_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion(), "OmniImpactVFXSlashMerc_Pink", false);


            //Not Indexed
            MercAssaulterEffect_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercAssaulterEffect.prefab").WaitForCompletion(), "MercAssaulterEffect_Pink", false);

            //Not Used?
            MercDashHitOverlay_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercDashHitOverlay.prefab").WaitForCompletion(), "MercDashHitOverlay_Pink", false);

            ImpactMercAssaulter_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercAssaulter.prefab").WaitForCompletion(), "ImpactMercAssaulter_Pink", false);


            ImpactMercFocusedAssault_Pink = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercFocusedAssault.prefab").WaitForCompletion(), "ImpactMercFocusedAssault_Pink", false);

        }

        public static Material matMercIgnition_Pink = null;

        [Obsolete]
        public static void MakeEffects_Pink()
        {
            Material matMercDelayedBillboard2_Pink = null;
            Material matMercFocusedAssaultIcon_Pink = null;
            Material matMercExposedBackdrop_Pink = null;

            Material matMercSwipe1_Pink = null;
            Material matMercSwipe2_Pink = null;
            Material matMercSwipe3_Pink = null;


            Material matMercExposedSlash_Pink = null;
            Material matOmniHitspark3Merc_Pink = null;
            Material matOmniRadialSlash1Merc_Pink = null;
            Material matOmniHitspark4Merc_Pink = null;
            Material matMercExposed_Pink;

            Material matMercHologram_Pink = LegacyResourcesAPI.Load<Material>("materials/matMercHologram");

            Material matHuntressSwipe_Pink;
            Material matHuntressCharged_Pink;

            matHuntressFlashBright_Pink.SetColor("_TintColor", new Color(1.3f, 0.6f, 1.3f * 0.9f, 1f));//0.0191 1.1386 1.2973 1 
            matHuntressFlashExpanded_Pink.SetColor("_TintColor", new Color(0.58f, 0.2f, 0.58f * 0.9f, 1f));//0 0.4367 0.5809 1

            Texture2D texRampFallboots_Pink = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampFallbootsPink.png");
            Texture2D texRampMercDust_Pink = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampMercDustPink.png");
            Texture2D texRampHuntressSoft_Pink = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampHuntressSoftPink.png");
            Texture2D texRampHuntress_Pink = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampHuntressPink.png");

            matMercEvisTarget_Pink = GameObject.Instantiate(LegacyResourcesAPI.Load<Material>("Materials/matMercEvisTarget"));
            matMercEvisTarget_Pink.SetTexture("_RemapTex", texRampHuntressSoft_Pink);
            matMercEvisTarget_Pink.SetColor("_TintColor", new Color(1, 0.5f, 1f));
            //matMercEvisTarget_Pink.SetColor("_CutoffScroll", new Color(0f, 12f, 12f));


            ParticleSystemRenderer MercSwordSlash_PinkRenderer0 = MercSwordSlash_Pink.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercSwipe1_Pink = UnityEngine.Object.Instantiate(MercSwordSlash_PinkRenderer0.material);
            matMercSwipe1_Pink.SetTexture("_RemapTex", texRampFallboots_Pink);
            matMercSwipe1_Pink.SetColor("_TintColor", new Color(0.5f, 0.314f, 0.5f * 0.9f, 1));        //Default Color : {r: 0, g: 0.314069, b: 1, a: 1}
            MercSwordSlash_PinkRenderer0.material = matMercSwipe1_Pink;

            //Child 0 has weird TracerBright
            ParticleSystem MercSwordFinisherSlash_PinkParticle0 = MercSwordFinisherSlash_Pink.transform.GetChild(0).GetComponent<ParticleSystem>();
            MercSwordFinisherSlash_PinkParticle0.startColor = new Color(1, 0.2f, 1f * 0.9f, 1);
            ParticleSystemRenderer MercSwordFinisherSlash_PinkRenderer1 = MercSwordFinisherSlash_Pink.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercSwipe2_Pink = UnityEngine.Object.Instantiate(MercSwordFinisherSlash_PinkRenderer1.material);
            matMercSwipe2_Pink.SetTexture("_RemapTex", texRampFallboots_Pink);
            matMercSwipe2_Pink.SetColor("_TintColor", new Color(0.8f, 0.366f, 0.8f * 0.9f, 1));  //Default Color r: 0.3632075, g: 0.6593511, b: 1, a: 1
            MercSwordFinisherSlash_PinkRenderer1.material = matMercSwipe2_Pink;

            ParticleSystemRenderer MercSwordSlashWhirlwind_PinkRenderer0 = MercSwordSlashWhirlwind_Pink.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            MercSwordSlashWhirlwind_PinkRenderer0.material = matMercSwipe1_Pink;

            ParticleSystemRenderer MercSwordUppercutSlash_PinkRenderer0 = MercSwordUppercutSlash_Pink.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            ParticleSystemRenderer MercSwordUppercutSlash_PinkRenderer1 = MercSwordUppercutSlash_Pink.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            MercSwordUppercutSlash_PinkRenderer0.material = matMercSwipe2_Pink;
            MercSwordUppercutSlash_PinkRenderer1.material = matMercSwipe1_Pink;




            OmniEffect OmniImpactVFXSlashMerc_PinkOmniEffect = OmniImpactVFXSlashMerc_Pink.GetComponent<OmniEffect>();


            //Material matOmniHitspark4Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMerc_PinkOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial);
            //OmniImpactVFXSlashMerc_PinkOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4Merc;

            Material matOmniRadialSlash1Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMerc_PinkOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1Merc.SetTexture("_RemapTex", texRampMercDust_Pink);
            OmniImpactVFXSlashMerc_PinkOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1Merc;

            Material matOmniHitspark3Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMerc_PinkOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniHitspark3Merc.SetTexture("_RemapTex", texRampMercDust_Pink);
            OmniImpactVFXSlashMerc_PinkOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3Merc;

            Material matOmniHitspark2Merc_Pink = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMerc_PinkOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial);
            matOmniHitspark2Merc_Pink.SetTexture("_RemapTex", texRampMercDust_Pink);
            OmniImpactVFXSlashMerc_PinkOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2Merc_Pink;

            ParticleSystem OmniImpactVFXSlashMerc_PinkParticle1 = OmniImpactVFXSlashMerc_Pink.transform.GetChild(1).GetComponent<ParticleSystem>(); //matOmniHitspark3 (Instance)
            OmniImpactVFXSlashMerc_PinkParticle1.startColor = new Color(0.7264f, 0.3f, 0.7264f * 0.9f, 1); //Default C0lor 0 0.7264 0.7039 1

            ParticleSystem OmniImpactVFXSlashMerc_PinkParticle2 = OmniImpactVFXSlashMerc_Pink.transform.GetChild(2).GetComponent<ParticleSystem>(); //matGenericFlash (Instance)
            OmniImpactVFXSlashMerc_PinkParticle2.startColor = new Color(0.9333f, 0.2f, 0.9333f * 0.9f, 1); //0 0.4951 0.9333 1

            ParticleSystem OmniImpactVFXSlashMerc_PinkParticle3 = OmniImpactVFXSlashMerc_Pink.transform.GetChild(3).GetComponent<ParticleSystem>(); //matTracerBright (Instance)
            OmniImpactVFXSlashMerc_PinkParticle3.startColor = new Color(0.5f, 0.4245f, 0.5f * 0.9f, 1); //0.3854 0.4245 0.0501 1





            //Figure out if start color needs to actually be changed because they all use it
            ParticleSystemRenderer MercAssaulterEffect_PinkRenderer5 = MercAssaulterEffect_Pink.transform.GetChild(5).GetComponent<ParticleSystemRenderer>();
            matMercIgnition_Pink = UnityEngine.Object.Instantiate(MercAssaulterEffect_PinkRenderer5.material);
            matMercIgnition_Pink.SetTexture("_RemapTex", texRampHuntress_Pink);
            matMercIgnition_Pink.SetColor("_TintColor", new Color(0.44f, 0.44f, 0.44f * 0.9f, 1)); //{r: 0, g: 0.1362783, b: 0.8867924, a: 1}
            MercAssaulterEffect_PinkRenderer5.material = matMercIgnition_Pink;

            MercAssaulterEffect_Pink.transform.GetChild(6).GetComponent<ParticleSystem>().startColor = new Color(0.71f, 0.3f, 0.71f * 0.9f, 1); //0 0.4409 0.9811 1
            MercAssaulterEffect_Pink.transform.GetChild(8).GetComponent<Light>().color = new Color(0.68f, 0.3765f, 0.68f * 0.9f, 1); //0.3765 0.4868 0.9804 1
            MercAssaulterEffect_Pink.transform.GetChild(9).GetComponent<TrailRenderer>().material = matMercSwipe1_Pink;
            MercAssaulterEffect_Pink.transform.GetChild(10).GetChild(2).GetComponent<TrailRenderer>().material = matMercIgnition_Pink;
            MercAssaulterEffect_Pink.transform.GetChild(10).GetChild(3).GetComponent<TrailRenderer>().material = matMercIgnition_Pink;




            ParticleSystem particleSystem = ImpactMercAssaulter_Pink.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.8f, 0.45f, 0.8f * 0.9f, 1f);//0.3538 0.6316 1 1
            particleSystem = ImpactMercAssaulter_Pink.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.72f, 0.522f, 0.72f * 0.9f, 1f);//0.467 0.7022 1 1

            ParticleSystemRenderer particleSystemRenderer = ImpactMercAssaulter_Pink.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercSwipe1_Pink;




            particleSystem = ImpactMercFocusedAssault_Pink.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.8f, 0.4f, 0.8f * 0.9f, 0.2667f);//0.3538 0.6316 1 0.2667
            particleSystem = ImpactMercFocusedAssault_Pink.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.7f, 0.26f, 0.7f * 0.9f, 1f);//0.0925 0.4637 0.934 1
            particleSystemRenderer = ImpactMercFocusedAssault_Pink.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercSwipe3_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercSwipe3_Pink.SetTexture("_RemapTex", texRampMercDust_Pink);
            particleSystemRenderer.material = matMercSwipe3_Pink;



            particleSystemRenderer = MercFocusedAssaultOrbEffect_Pink.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercDelayedBillboard2_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercDelayedBillboard2_Pink.SetTexture("_RemapTex", texRampMercDust_Pink);
            particleSystemRenderer.material = matMercDelayedBillboard2_Pink;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Pink.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercFocusedAssaultIcon_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercFocusedAssaultIcon_Pink.SetTexture("_RemapTex", texRampMercDust_Pink);
            particleSystemRenderer.material = matMercFocusedAssaultIcon_Pink;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Pink.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercExposedBackdrop_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            //matMercExposedBackdrop = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposedBackdrop_Pink.SetColor("_TintColor", new Color(1f, 0f, 1f, 0.9f)); //1 1 1 1
            particleSystemRenderer.material = matMercExposedBackdrop_Pink;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Pink.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            matMercExposedSlash_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposedSlash_Pink.SetTexture("_RemapTex", texRampHuntress_Pink);
            matMercExposedSlash_Pink.SetColor("_TintColor", new Color(0.512f, 0.06f, 0.512f, 1)); //r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposedSlash_Pink;

            matMercEnergized_Pink.SetTexture("_RemapTex", texRampHuntressSoft_Pink);
            matMercEnergized_Pink.SetColor("_TintColor", new Color(0.66f, 0.88f, 0.88f, 1)); //- _TintColor: {r: 0.28423718, g: 0.4328456, b: 1.8260084, a: 1}



            //HuntressBlinkEffect_Pink
            particleSystem = HuntressBlinkEffect_Pink.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.39f, 0.154f, 0.39f * 0.9f, 1f);//0.1534 0.1567 0.6324 1
            //particleSystem.colorOverLifetime.SetPropertyValue<bool>("enabled", false);
            var stupid = particleSystem.colorOverLifetime;
            stupid.enabled = false;
            //particleSystem.SetPropertyValue("colorOverLifetime", stupid);

            Light light = HuntressBlinkEffect_Pink.transform.GetChild(0).GetChild(2).GetComponent<Light>();
            light.color = new Color(1f, 0.6f, 1f * 0.9f, 1); //0.2721 0.9699 1 1

            particleSystemRenderer = HuntressBlinkEffect_Pink.transform.GetChild(0).GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressSwipe_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matHuntressSwipe_Pink.SetTexture("_RemapTex", texRampHuntressSoft_Pink);
            particleSystemRenderer.material = matHuntressSwipe_Pink;

            particleSystemRenderer = HuntressBlinkEffect_Pink.transform.GetChild(0).GetChild(5).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Pink;



            //

            //HuntressFireArrowRain_Pink
            particleSystemRenderer = HuntressFireArrowRain_Pink.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Pink;

            particleSystemRenderer = HuntressFireArrowRain_Pink.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Pink;

            particleSystemRenderer = HuntressFireArrowRain_Pink.transform.GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressCharged_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matHuntressCharged_Pink.SetTexture("_RemapTex", texRampHuntress_Pink);
            particleSystemRenderer.material = matHuntressCharged_Pink;

            light = HuntressFireArrowRain_Pink.transform.GetChild(5).GetComponent<Light>();
            light.color = new Color(0.87f, 0.5f, 0.87f * 0.9f, 1f); //0.3456 0.7563 1 1


            //

            //OmniImpactVFXSlashMercEvis_Pink
            OmniEffect omniEffect = OmniImpactVFXSlashMercEvis_Pink.GetComponent<OmniEffect>();

            omniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4Merc_Pink;




            matOmniRadialSlash1Merc_Pink = UnityEngine.Object.Instantiate(omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1Merc_Pink.SetTexture("_RemapTex", texRampMercDust_Pink);

            omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1Merc_Pink;

            omniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3Merc_Pink;
            omniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2Merc_Pink;

            particleSystemRenderer = OmniImpactVFXSlashMercEvis_Pink.transform.GetChild(7).GetComponent<ParticleSystemRenderer>();
            matMercHologram_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercHologram_Pink.SetTexture("_RemapTex", texRampFallboots_Pink);
            matMercHologram_Pink.SetColor("_TintColor", new Color(1.12f, 0.3f, 1.12f * 0.9f, 1f));//0.2842 0.4328 1.826 1
            particleSystemRenderer.material = matMercHologram_Pink;


            EvisProjectileGhost_Pink.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2_Pink;
            EvisProjectileGhost_Pink.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Pink;
            EvisProjectileGhost_Pink.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercIgnition_Pink;
            EvisProjectileGhost_Pink.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(0.63f, 0.23f, 0.71f * 0.9f, 1f); //0 0.5827 1 1
            EvisProjectileGhost_Pink.transform.GetChild(5).GetComponent<Light>().color = new Color(0.59f, 0.25f, 0.65f * 0.9f, 1f); //0.1274 0.4704 1 1


            EvisOverlapProjectileGhost_Pink.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2_Pink;
            EvisOverlapProjectileGhost_Pink.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matOmniRadialSlash1Merc_Pink;
            EvisOverlapProjectileGhost_Pink.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matOmniHitspark2Merc_Pink;
            EvisOverlapProjectileGhost_Pink.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(0.8f, 0.51f, 0.86f * 0.9f, 1f);//0.3066 0.7276 1 1
            EvisOverlapProjectileGhost_Pink.transform.GetChild(4).GetComponent<Light>().color = new Color(0.627f, 0.2772f, 0.7f * 0.9f, 1f);//0.1274 0.4704 1 1
            EvisOverlapProjectileGhost_Pink.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Pink;
            EvisOverlapProjectileGhost_Pink.transform.GetChild(6).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Pink;

            ImpactMercEvis_Pink.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Pink;
            ImpactMercEvis_Pink.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Pink;
            ImpactMercEvis_Pink.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Pink;
            ImpactMercEvis_Pink.transform.GetChild(3).GetComponent<Light>().color = new Color(0.92f, 0.425f, 0.92f * 0.9f, 1f);//0 0.8542 1 1





            particleSystemRenderer = MercExposeEffect_Pink.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            //matMercExposed = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposed_Pink = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposed_Pink.SetTexture("_RemapTex", texRampHuntress_Pink);
            matMercExposed_Pink.SetColor("_TintColor", new Color(0.25f, 0.25f, 0.25f * 0.9f, 1f));//r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposed_Pink;
            particleSystemRenderer = MercExposeEffect_Pink.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedBackdrop_Pink;


            particleSystemRenderer = MercExposeConsumeEffect_Pink.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposed_Pink;
            particleSystemRenderer = MercExposeConsumeEffect_Pink.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedSlash_Pink;


            MercExposeEffect_Pink.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Pink;
            MercExposeEffect_Pink.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.15f, 0.100f, 0.15f * 0.9f, 0.29f);//0.1335 0.1455 0.2264 0.3412
            MercExposeEffect_Pink.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop_Pink;

            MercExposeConsumeEffect_Pink.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Pink;
            MercExposeConsumeEffect_Pink.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop_Pink;
            MercExposeConsumeEffect_Pink.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.3f, 0.11f, 0.3f * 0.9f, 0.175f); //0.1076 0.2301 0.3868 0.2745

            MercExposeConsumeEffect_Pink.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Pink;

            //ContentAddition.AddEffect(MercSwordFinisherSlash_Pink); //Game tries to spawn this as an effect even tho it isn't one

        }

    }

}
