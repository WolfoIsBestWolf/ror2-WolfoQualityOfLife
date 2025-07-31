using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public static class Merc_Red
    {

        public static Material matMercEnergized_Red = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matMercEnergized"));
        public static Material matMercEvisTarget_Red = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matMercEvisTarget"));

        public static Material matHuntressFlashBright_Red = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashBright"));
        public static Material matHuntressFlashExpanded_Red = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashExpanded"));

        public static GameObject MercFocusedAssaultOrbEffect_Red = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/MercFocusedAssaultOrbEffect"), "MercFocusedAssaultOrbEffect_Red", false);

        public static GameObject OmniImpactVFXSlashMerc_Red = null; //Primary, Secondary1, Secondary2
        public static GameObject MercSwordSlash_Red = null; //Primary
        public static GameObject MercSwordFinisherSlash_Red = null; //Primary, Special2

        public static GameObject MercSwordSlashWhirlwind_Red = null;  //Secondary1
        public static GameObject MercDashHitOverlay_Red = null; //Utility1, Utility2

        public static GameObject HuntressBlinkEffect_Red = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/HuntressBlinkEffect"), "HuntressBlinkEffect_Red", false); //Special1 (Enter)
        public static GameObject HuntressFireArrowRain_Red = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/HuntressFireArrowRain"), "HuntressFireArrowRain_Red", false); //Special1 (Attack)
        public static GameObject OmniImpactVFXSlashMercEvis_Red = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/omnieffect/OmniImpactVFXSlashMercEvis"), "OmniImpactVFXSlashMercEvis_Red", false); //Special1 (Attack)


        public static GameObject MercSwordUppercutSlash_Red = null;  //Secondary2
        public static GameObject ImpactMercFocusedAssault_Red = null;  //Utility2
        public static GameObject ImpactMercAssaulter_Red = null;  //Utility2
        public static GameObject MercAssaulterEffect_Red = null;  //Utility2

        public static GameObject EvisProjectileGhost_Red = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisProjectileGhost"), "EvisProjectileGhost_Red", false);
        public static GameObject EvisOverlapProjectileGhost_Red = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisOverlapProjectileGhost"), "EvisOverlapProjectileGhost_Red", false);

        public static GameObject ImpactMercEvis_Red = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/ImpactMercEvis"), "ImpactMercEvis_Red", false);


        public static GameObject MercExposeEffect_Red;
        public static GameObject MercExposeConsumeEffect_Red;


        public static void Start()
        {
            MercExposeEffect_Red = PrefabAPI.InstantiateClone(Merc_Blue.MercExposeEffect, "MercExposeEffect_Red", false);
            MercExposeConsumeEffect_Red = PrefabAPI.InstantiateClone(Merc_Blue.MercExposeConsumeEffect, "MercExposeConsumeEffect_Red", false);


            //Not Indexed
            MercSwordSlash_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion(), "MercSwordSlash_Red", false);

            //Not Indexed
            MercSwordFinisherSlash_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordFinisherSlash.prefab").WaitForCompletion(), "MercSwordFinisherSlash_Red", false);

            //.rootObject Body
            MercSwordSlashWhirlwind_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion(), "MercSwordSlashWhirlwind_Red", false);


            MercSwordUppercutSlash_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion(), "MercSwordUppercutSlash_Red", false);


            OmniImpactVFXSlashMerc_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion(), "OmniImpactVFXSlashMerc_Red", false);


            //Not Indexed
            MercAssaulterEffect_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercAssaulterEffect.prefab").WaitForCompletion(), "MercAssaulterEffect_Red", false);

            //Not Used?
            MercDashHitOverlay_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercDashHitOverlay.prefab").WaitForCompletion(), "MercDashHitOverlay_Red", false);

            ImpactMercAssaulter_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercAssaulter.prefab").WaitForCompletion(), "ImpactMercAssaulter_Red", false);


            ImpactMercFocusedAssault_Red = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercFocusedAssault.prefab").WaitForCompletion(), "ImpactMercFocusedAssault_Red", false);


            //AddEffects_ServerSided();
        }

        /*public static void AddEffects_ServerSided()
        {
            //M2
            ContentAddition.AddEffect(MercSwordSlashWhirlwind_Red);
            //M2 Alt
            ContentAddition.AddEffect(MercSwordUppercutSlash_Red);

            //M3 ?
            ContentAddition.AddEffect(ImpactMercAssaulter_Red);
            //M3 Alt?
            ContentAddition.AddEffect(ImpactMercFocusedAssault_Red);
            //M4
            ContentAddition.AddEffect(OmniImpactVFXSlashMerc_Red);

            ContentAddition.AddEffect(ImpactMercEvis_Red);

            ContentAddition.AddEffect(MercFocusedAssaultOrbEffect_Red);

            //No info at all
            ContentAddition.AddEffect(HuntressBlinkEffect_Red);

            //No info at all
            ContentAddition.AddEffect(HuntressFireArrowRain_Red);

            //Evis
            //No info at all
            ContentAddition.AddEffect(OmniImpactVFXSlashMercEvis_Red);

            ContentAddition.AddEffect(MercExposeConsumeEffect_Red);

        }*/
        public static Material matMercIgnition_Red = null;

        [Obsolete]
        public static void MakeEffects_Red()
        {
            Material matMercDelayedBillboard2_Red = null;
            Material matMercFocusedAssaultIcon_Red = null;
            Material matMercExposedBackdrop_Red = null;

            Material matMercSwipe1_Red = null;
            Material matMercSwipe2_Red = null;
            Material matMercSwipe3_Red = null;


            Material matMercExposedSlash_Red = null;
            Material matOmniHitspark3Merc_Red = null;
            Material matOmniRadialSlash1Merc_Red = null;
            Material matOmniHitspark4Merc_Red = null;
            Material matMercExposed_Red;

            Material matMercHologram_Red = LegacyResourcesAPI.Load<Material>("materials/matMercHologram");

            Material matHuntressSwipe_Red;
            Material matHuntressCharged_Red;

            matHuntressFlashBright_Red.SetColor("_TintColor", new Color(1.3f, 0.6f, 0.6f, 1f));//0.0191 1.1386 1.2973 1 
            matHuntressFlashExpanded_Red.SetColor("_TintColor", new Color(0.58f, 0.2f, 0.2f, 1f));//0 0.4367 0.5809 1

            Texture2D texRampFallbootsRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampFallbootsRed.png");
            Texture2D texRampMercDustRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampMercDustRed.png");
            Texture2D texRampHuntressSoftRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampHuntressSoftRed.png");
            Texture2D texRampHuntressRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampHuntressRed.png");

            matMercEvisTarget_Red = GameObject.Instantiate(LegacyResourcesAPI.Load<Material>("Materials/matMercEvisTarget"));
            matMercEvisTarget_Red.SetTexture("_RemapTex", texRampHuntressSoftRed);
            matMercEvisTarget_Red.SetColor("_TintColor", Color.red);
            matMercEvisTarget_Red.SetColor("_CutoffScroll", new Color(0f, 12f, 12f));


            ParticleSystemRenderer MercSwordSlashRedRenderer0 = MercSwordSlash_Red.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercSwipe1_Red = UnityEngine.Object.Instantiate(MercSwordSlashRedRenderer0.material);
            matMercSwipe1_Red.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercSwipe1_Red.SetColor("_TintColor", new Color(1, 0, 0, 1));        //Default Color : {r: 0, g: 0.314069, b: 1, a: 1}
            MercSwordSlashRedRenderer0.material = matMercSwipe1_Red;

            //Child 0 has weird TracerBright
            ParticleSystem MercSwordFinisherSlashRedParticle0 = MercSwordFinisherSlash_Red.transform.GetChild(0).GetComponent<ParticleSystem>();
            MercSwordFinisherSlashRedParticle0.startColor = new Color(1, 0.2f, 0.2f, 1);
            ParticleSystemRenderer MercSwordFinisherSlashRedRenderer1 = MercSwordFinisherSlash_Red.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercSwipe2_Red = UnityEngine.Object.Instantiate(MercSwordFinisherSlashRedRenderer1.material);
            matMercSwipe2_Red.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercSwipe2_Red.SetColor("_TintColor", new Color(1, 0.22f, 0.22f, 1));  //Default Color r: 0.3632075, g: 0.6593511, b: 1, a: 1
            MercSwordFinisherSlashRedRenderer1.material = matMercSwipe2_Red;

            ParticleSystemRenderer MercSwordSlashWhirlwindRedRenderer0 = MercSwordSlashWhirlwind_Red.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            MercSwordSlashWhirlwindRedRenderer0.material = matMercSwipe1_Red;

            ParticleSystemRenderer MercSwordUppercutSlashRedRenderer0 = MercSwordUppercutSlash_Red.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            ParticleSystemRenderer MercSwordUppercutSlashRedRenderer1 = MercSwordUppercutSlash_Red.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            MercSwordUppercutSlashRedRenderer0.material = matMercSwipe2_Red;
            MercSwordUppercutSlashRedRenderer1.material = matMercSwipe1_Red;




            OmniEffect OmniImpactVFXSlashMercRedOmniEffect = OmniImpactVFXSlashMerc_Red.GetComponent<OmniEffect>();


            //Material matOmniHitspark4Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial);
            //OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4Merc;

            Material matOmniRadialSlash1Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1Merc.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1Merc;

            Material matOmniHitspark3Merc = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniHitspark3Merc.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3Merc;

            Material matOmniHitspark2MercRed = UnityEngine.Object.Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial);
            matOmniHitspark2MercRed.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2MercRed;

            ParticleSystem OmniImpactVFXSlashMercRedParticle1 = OmniImpactVFXSlashMerc_Red.transform.GetChild(1).GetComponent<ParticleSystem>(); //matOmniHitspark3 (Instance)
            OmniImpactVFXSlashMercRedParticle1.startColor = new Color(0.7264f, 0.3f, 0.3f, 1); //Default C0lor 0 0.7264 0.7039 1

            ParticleSystem OmniImpactVFXSlashMercRedParticle2 = OmniImpactVFXSlashMerc_Red.transform.GetChild(2).GetComponent<ParticleSystem>(); //matGenericFlash (Instance)
            OmniImpactVFXSlashMercRedParticle2.startColor = new Color(0.9333f, 0.2f, 0.2f, 1); //0 0.4951 0.9333 1

            ParticleSystem OmniImpactVFXSlashMercRedParticle3 = OmniImpactVFXSlashMerc_Red.transform.GetChild(3).GetComponent<ParticleSystem>(); //matTracerBright (Instance)
            OmniImpactVFXSlashMercRedParticle3.startColor = new Color(0.5f, 0.4245f, 0.0501f, 1); //0.3854 0.4245 0.0501 1





            //Figure out if start color needs to actually be changed because they all use it
            ParticleSystemRenderer MercAssaulterEffectRedRenderer5 = MercAssaulterEffect_Red.transform.GetChild(5).GetComponent<ParticleSystemRenderer>();
            matMercIgnition_Red = UnityEngine.Object.Instantiate(MercAssaulterEffectRedRenderer5.material);
            matMercIgnition_Red.SetTexture("_RemapTex", texRampHuntressRed);
            matMercIgnition_Red.SetColor("_TintColor", new Color(0.8867924f, 0.06f, 0.06f, 1)); //{r: 0, g: 0.1362783, b: 0.8867924, a: 1}
            MercAssaulterEffectRedRenderer5.material = matMercIgnition_Red;

            MercAssaulterEffect_Red.transform.GetChild(6).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.3f, 0.3f, 1); //0 0.4409 0.9811 1
            MercAssaulterEffect_Red.transform.GetChild(8).GetComponent<Light>().color = new Color(0.9804f, 0.3765f, 0.4868f, 1); //0.3765 0.4868 0.9804 1
            MercAssaulterEffect_Red.transform.GetChild(9).GetComponent<TrailRenderer>().material = matMercSwipe1_Red;
            MercAssaulterEffect_Red.transform.GetChild(10).GetChild(2).GetComponent<TrailRenderer>().material = matMercIgnition_Red;
            MercAssaulterEffect_Red.transform.GetChild(10).GetChild(3).GetComponent<TrailRenderer>().material = matMercIgnition_Red;




            ParticleSystem particleSystem = ImpactMercAssaulter_Red.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.45f, 0.45f, 1f);//0.3538 0.6316 1 1
            particleSystem = ImpactMercAssaulter_Red.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.575f, 0.575f, 1f);//0.467 0.7022 1 1

            ParticleSystemRenderer particleSystemRenderer = ImpactMercAssaulter_Red.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercSwipe1_Red;




            particleSystem = ImpactMercFocusedAssault_Red.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.45f, 0.45f, 0.2667f);//0.3538 0.6316 1 0.2667
            particleSystem = ImpactMercFocusedAssault_Red.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.934f, 0.26f, 0.26f, 1f);//0.0925 0.4637 0.934 1
            particleSystemRenderer = ImpactMercFocusedAssault_Red.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercSwipe3_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercSwipe3_Red.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercSwipe3_Red;



            particleSystemRenderer = MercFocusedAssaultOrbEffect_Red.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercDelayedBillboard2_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercDelayedBillboard2_Red.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercDelayedBillboard2_Red;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Red.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercFocusedAssaultIcon_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercFocusedAssaultIcon_Red.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercFocusedAssaultIcon_Red;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Red.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercExposedBackdrop_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            //matMercExposedBackdrop = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposedBackdrop_Red.SetColor("_TintColor", new Color(3, -2, -2, 0.9f));
            particleSystemRenderer.material = matMercExposedBackdrop_Red;

            particleSystemRenderer = MercFocusedAssaultOrbEffect_Red.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            matMercExposedSlash_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposedSlash_Red.SetTexture("_RemapTex", texRampHuntressRed);
            matMercExposedSlash_Red.SetColor("_TintColor", new Color(0.8868f, 0.06f, 0.06f, 1)); //r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposedSlash_Red;

            matMercEnergized_Red.SetTexture("_RemapTex", texRampHuntressSoftRed);
            matMercEnergized_Red.SetColor("_TintColor", new Color(1.8f, 0.35f, 0.35f, 1));



            //HuntressBlinkEffectRed
            particleSystem = HuntressBlinkEffect_Red.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.6324f, 0.154f, 0.154f, 1f);//0.1534 0.1567 0.6324 1
            //particleSystem.colorOverLifetime.SetPropertyValue<bool>("enabled", false);
            var stupid = particleSystem.colorOverLifetime;
            stupid.enabled = false;
            //particleSystem.SetPropertyValue("colorOverLifetime", stupid);

            Light light = HuntressBlinkEffect_Red.transform.GetChild(0).GetChild(2).GetComponent<Light>();
            light.color = new Color(1f, 0.6f, 0.6f, 1); //0.2721 0.9699 1 1

            particleSystemRenderer = HuntressBlinkEffect_Red.transform.GetChild(0).GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressSwipe_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matHuntressSwipe_Red.SetTexture("_RemapTex", texRampHuntressSoftRed);
            particleSystemRenderer.material = matHuntressSwipe_Red;

            particleSystemRenderer = HuntressBlinkEffect_Red.transform.GetChild(0).GetChild(5).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Red;



            //

            //HuntressFireArrowRainRed
            particleSystemRenderer = HuntressFireArrowRain_Red.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Red;

            particleSystemRenderer = HuntressFireArrowRain_Red.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipe_Red;

            particleSystemRenderer = HuntressFireArrowRain_Red.transform.GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressCharged_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matHuntressCharged_Red.SetTexture("_RemapTex", texRampHuntressRed);
            particleSystemRenderer.material = matHuntressCharged_Red;

            light = HuntressFireArrowRain_Red.transform.GetChild(5).GetComponent<Light>();
            light.color = new Color(1f, 0.55f, 0.55f, 1f); //0.3456 0.7563 1 1


            //

            //OmniImpactVFXSlashMercEvisRed
            OmniEffect omniEffect = OmniImpactVFXSlashMercEvis_Red.GetComponent<OmniEffect>();

            omniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4Merc_Red;




            matOmniRadialSlash1Merc_Red = UnityEngine.Object.Instantiate(omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1Merc_Red.SetTexture("_RemapTex", texRampMercDustRed);

            omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1Merc_Red;


            omniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3Merc_Red;
            omniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2MercRed;

            particleSystemRenderer = OmniImpactVFXSlashMercEvis_Red.transform.GetChild(7).GetComponent<ParticleSystemRenderer>();
            matMercHologram_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercHologram_Red.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercHologram_Red.SetColor("_TintColor", new Color(1.825f, -0.25f, 0f, 1f));//0.2842 0.4328 1.826 1
            particleSystemRenderer.material = matMercHologram_Red;


            EvisProjectileGhost_Red.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2_Red;
            EvisProjectileGhost_Red.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Red;
            EvisProjectileGhost_Red.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercIgnition_Red;
            EvisProjectileGhost_Red.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f); //0 0.5827 1 1
            EvisProjectileGhost_Red.transform.GetChild(5).GetComponent<Light>().color = new Color(1f, 0.3f, 0.3f, 1f); //0.1274 0.4704 1 1


            EvisOverlapProjectileGhost_Red.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2_Red;
            EvisOverlapProjectileGhost_Red.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matOmniRadialSlash1Merc_Red;
            EvisOverlapProjectileGhost_Red.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matOmniHitspark2MercRed;
            EvisOverlapProjectileGhost_Red.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.51f, 0.51f, 1f);//0.3066 0.7276 1 1
            EvisOverlapProjectileGhost_Red.transform.GetChild(4).GetComponent<Light>().color = new Color(1f, 0.3f, 0.3f, 1f);//0.1274 0.4704 1 1
            EvisOverlapProjectileGhost_Red.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Red;
            EvisOverlapProjectileGhost_Red.transform.GetChild(6).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Red;

            ImpactMercEvis_Red.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercHologram_Red;
            ImpactMercEvis_Red.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Red;
            ImpactMercEvis_Red.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1_Red;
            ImpactMercEvis_Red.transform.GetChild(3).GetComponent<Light>().color = new Color(1f, 0.425f, 0.425f, 1f);//0 0.8542 1 1





            particleSystemRenderer = MercExposeEffect_Red.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            //matMercExposed = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposed_Red = UnityEngine.Object.Instantiate(particleSystemRenderer.material);
            matMercExposed_Red.SetTexture("_RemapTex", texRampHuntressRed);
            matMercExposed_Red.SetColor("_TintColor", new Color(0.9f, -0.1f, -0.1f, 1f));//r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposed_Red;
            particleSystemRenderer = MercExposeEffect_Red.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedBackdrop_Red;


            particleSystemRenderer = MercExposeConsumeEffect_Red.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposed_Red;
            particleSystemRenderer = MercExposeConsumeEffect_Red.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedSlash_Red;


            MercExposeEffect_Red.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Red;
            MercExposeEffect_Red.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.23f, 0.100f, 0.080f, 0.29f);//0.1335 0.1455 0.2264 0.3412
            MercExposeEffect_Red.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop_Red;

            MercExposeConsumeEffect_Red.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Red;
            MercExposeConsumeEffect_Red.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop_Red;
            MercExposeConsumeEffect_Red.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.35f, 0.2f, 0.2f, 0.175f);

            MercExposeConsumeEffect_Red.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercExposed_Red;

            //ContentAddition.AddEffect(MercSwordFinisherSlashRed); //Game tries to spawn this as an effect even tho it isn't one

        }

    }

}
