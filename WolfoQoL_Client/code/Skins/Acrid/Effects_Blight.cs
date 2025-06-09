using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client
{
    public class Effects_Blight
    {
        //Fucking Croco Blight

        public static Material matCrocoDiseaseDrippingsBlight;

        public static GameObject CrocoBiteEffectBlight = null; //CrocoBiteEffect //Bite

        public static GameObject CrocoSlashBlight = null; //CrocoSlash //Primary
        public static GameObject CrocoComboFinisherSlashBlight = null; //CrocoSlashFinisher //Primary


        public static GameObject CrocoDiseaseOrbEffect_Blight = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/CrocoDiseaseOrbEffect"), "CrocoDiseaseOrbEffect_Blight", false); //CrocoDiseaseOrbEffect 
        public static GameObject CrocoDiseaseGhostBlight = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/CrocoDiseaseGhost"), "CrocoDiseaseProjectileGhost_Blight", false); //CrocoDiseaseProjectile //Spit + Disease
        public static GameObject CrocoSpitGhostBlight = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/CrocoSpitGhost"), "CrocoSpitGhost_Blight", false); //CrocoSpit //Spit + Disease

        public static GameObject CrocoDiseaseImpactEffect_Blight = null;
        public static GameObject MuzzleflashCroco_Blight = null;

        public static GameObject CrocoLeapExplosion_Blight = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/CrocoLeapExplosion"), "CrocoLeapExplosion_Blight", false); //CrocoLeapExplosion //Leap
        public static GameObject CrocoFistEffectBlight = null; //CrocoFistEffect //Leap
                                                               //public static GameObject CrocoChainableFistEffect = null; //CrocoFistEffect //Leap

        public static GameObject CrocoLeapAcid_GhostBlight = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoLeapAcidGhost.prefab").WaitForCompletion(), "CrocoLeapAcidGhost_Blight", false); //CrocoLeapAcid //Leap

        public static GameObject lobbyPool = null;


        public static void GhostReplacements()
        {
            GameObject CrocoDiseaseProjectile = LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoDiseaseProjectile");
            GameObject CrocoSpit = LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoSpit");

            ProjectileGhostReplacer replacerM4 = CrocoDiseaseProjectile.AddComponent<ProjectileGhostReplacer>();
            ProjectileGhostReplacer replacerM2 = CrocoSpit.AddComponent<ProjectileGhostReplacer>();

            replacerM4.condition = SkinChanges.Case.Acrid;
            replacerM2.condition = SkinChanges.Case.Acrid;

            replacerM2.ghostPrefab_1 = CrocoSpitGhostBlight;
            replacerM2.impactPrefab_1 = CrocoDiseaseImpactEffect_Blight;

            replacerM4.ghostPrefab_1 = CrocoDiseaseGhostBlight;
            replacerM4.impactPrefab_1 = CrocoDiseaseImpactEffect_Blight;
            replacerM4.orbOnEnd = true;

            GameObject CrocoLeapAcid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoLeapAcid.prefab").WaitForCompletion();
            ProjectileGhostReplacer replacerPool = CrocoLeapAcid.AddComponent<ProjectileGhostReplacer>();

            replacerPool.condition = SkinChanges.Case.Acrid;
            replacerPool.ghostPrefab_1 = CrocoLeapAcid_GhostBlight;
            replacerPool.useReplacement = false;
        }

        public static void Start()
        {
            //General VFX List for stuff
            CrocoFistEffectBlight = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoFistEffect.prefab").WaitForCompletion(), "CrocoFistEffect_Blight", false);
            CrocoBiteEffectBlight = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoBiteEffect.prefab").WaitForCompletion(), "CrocoBiteEffect_Blight", false);
            CrocoSlashBlight = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoSlash.prefab").WaitForCompletion(), "CrocoSlashBlight", false);
            CrocoComboFinisherSlashBlight = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoComboFinisherSlash.prefab").WaitForCompletion(), "CrocoComboFinisherSlash_Blight", false);
            CrocoDiseaseImpactEffect_Blight = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoDiseaseImpactEffect.prefab").WaitForCompletion(), "CrocoDiseaseImpactEffect_Blight", false);

            MuzzleflashCroco_Blight = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/MuzzleflashCroco.prefab").WaitForCompletion(), "MuzzleflashCroco_Blight", false);


            MakeEffects();

        }

        public static void MakeEffects()
        {
            Material matCrocoGooSmallBlightD;
            Material matCrocoGooLargeBlight;
            Material matCrocoGooSmallBlight;
            Material matCrocoDiseaseSporeBlight;
            Material matCrocoDiseaseTrailBlight;
            Material matCrocoDiseaseTrailLesserBlight;
            Material matCrocoDiseaseTrailOrangeBlight;
            Material matCrocoBiteDiseasedBlight;
            Material matCrocoDiseaseHeadBlight;
            Material matCrocoGooDecalBlight;

            GameObject CrocoChainableFistEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoChainableFistEffect.prefab").WaitForCompletion();


            Texture2D texRampCrocoDiseaseBlight = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampCrocoDiseaseBlight.png");
            Texture2D texRampCrocoDiseaseDarkDark = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampCrocoDiseaseDarkDark.png");
            Texture2D texRampCrocoDiseaseBlightAlt = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampCrocoDiseaseBlightAlt.png");
            Texture2D texRampBeetleBreathBlight = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampBeetleBreathBlight.png");

            //Disease orb research???
            //CrocoDiseaseOrbEffect
            //


            //BlightBillboard uses texRampCrocoDiseaseDark
            //PoisonBillboard uses texRampPoison

            //////Materials needed
            ////Bite (CrocoBiteEffect)
            //matCrocoBiteDiseased (0)
            //distortion (not needed) (1)
            //matCrocoGooLarge (2)
            //matTracerBright (3)
            //
            ////Spit Ghost (CrocoSpitGhost)
            //matCrocoDiseaseSpore (0)
            //Light (1)
            //matCrocoGooSmall (2)
            //matCrocoDiseaseDrippings (3)
            //unused (4)
            //unused (5)
            //matCrocoDiseaseTrail (6,0)
            //matCrocoDiseaseTrail (6,1)
            //
            ////Disease Ghost (CrocoDiseaseGhost)
            //matCrocoDiseaseSpore (0)
            //Light (1)
            //matCrocoDiseaseHead (2)
            //matCrocoGooSmall (3)
            //matCrocoDiseaseTrail (4,0)
            //matCrocoDiseaseTrailLesser (4,1)
            //matCrocoDiseaseTrailLesser (4,2)
            //
            ////MuzzleFlash (MuzzleflashCroco)
            //matCrocoGooLarge (0)
            //matCrocoGooSmall (1)
            //matCrocoGooLarge (2)
            //matTracerBright (3)
            //Light (4)
            //
            ////Projectile Impact (CrocoDiseaseImpactEffect)
            //matCrocoGooLarge (0)
            //matTracerBright (1)
            //matCrocoDiseaseTrail (2)
            //Light (3)
            //
            ////CrocoLeapAcid (CrocoLeapAcid)
            //matCrocoGooDecal (0,0) "ThreeEyedGames.Decal"
            //matCrocoDiseaseSpore (0,1)
            //matCrocoDiseaseDrippings (0,2)
            //Light (0,3)
            // - (0,4)
            //
            ////CrocoFistEffect (CrocoFistEffect)
            //matCrocoGooSmall (0)
            //matCrocoDiseaseTrail (1)
            //Light (2)
            //matCrocoSlashDiseased (3) (unused)
            //
            ////CrocoLeapExplosion (CrocoLeapExplosion)
            //matCrocoGooSmall (0)
            //matOmniHitspark1Generic (1)
            //matCrocoGooLarge (2)
            //matCrocoGooLarge (3)
            //matCrocoDiseaseSpore (4)
            //matCryoCanisterSphere (5) ???
            //Light (6)
            //unused (7 + 8 + 9)
            //
            ////CrocoDiseaseOrbEffect
            //matCrocoDiseaseSpore (0,0)
            //matCrocoDiseaseTrailOrange (1,0)
            //matCrocoDiseaseTrailLesser (1,1)
            //
            //

            ////Finalized mat List
            //
            //matCrocoGooLarge
            //matCrocoGooSmall
            //matCrocoDiseaseSpore
            //matCrocoDiseaseDrippings
            //matCrocoDiseaseTrail
            //matCrocoDiseaseTrailLesser
            //matTracerBright
            //
            //
            //matCrocoBiteDiseased
            //matCrocoDiseaseHead
            //matCrocoGooDecal
            //

            //BurnEffectController

            //Color32 GooEmission = new Color32(21, 0, 34, 255);
            Color32 GooEmission = new Color32(192, 144, 159, 255);
            //Color32 GooTint = new Color32(151, 53, 255, 255);
            Color32 GooTint = new Color32(255, 223, 240, 255);
            Color TracerColor = new Color32(185, 22, 179, 255);
            Color TrailColor = new Color(1.755f, 0.785f, 1.2f, 1f);

            //
            matCrocoGooSmallBlight = UnityEngine.Object.Instantiate(CrocoChainableFistEffect.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoGooSmallBlight.name = "matCrocoGooSmallBlight";
            matCrocoGooSmallBlight.SetColor("_EmissionColor", new Color(1.4f, 0.95f, 0.95f, 1f));
            matCrocoGooSmallBlight.SetColor("_TintColor", new Color(1.1f, 0.9f, 0.9f, 1f));


            matCrocoGooSmallBlightD = UnityEngine.Object.Instantiate(CrocoChainableFistEffect.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoGooSmallBlightD.name = "matCrocoGooSmallBlightD";
            matCrocoGooSmallBlightD.SetColor("_EmissionColor", new Color(0.8f, 0.65f, 0.65f, 1f));
            matCrocoGooSmallBlightD.SetColor("_TintColor", new Color(1f, 1f, 1f, 1f));
            /*

            //matCrocoGooSmallBlight.SetColor("_TintColor", new Color(0f, 0f, 0f, 1f);
            /*
            matCrocoGooSmallBlight.SetTexture("_RemapTex", texRampCrocoDiseaseDarkDarkAlt);
            matCrocoGooSmallBlight.SetColor("_EmissionColor", GooEmission);
            matCrocoGooSmallBlight.SetColor("_TintColor", GooTint);
            */
            //
            matCrocoGooLargeBlight = UnityEngine.Object.Instantiate(CrocoBiteEffectBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoGooLargeBlight.name = "matCrocoGooLargeBlight";
            matCrocoGooLargeBlight.SetTexture("_RemapTex", matCrocoGooSmallBlight.GetTexture("_RemapTex"));
            matCrocoGooLargeBlight.SetColor("_EmissionColor", new Color(0.85f, 0.7f, 0.7f, 1f));
            matCrocoGooLargeBlight.SetColor("_TintColor", new Color(1f, 1f, 1f, 1f));
            //
            /*
            matCrocoGooLargeBlight.SetTexture("_RemapTex", texRampCrocoDiseaseDarkDarkAlt);
            matCrocoGooLargeBlight.SetColor("_EmissionColor", GooEmission);
            matCrocoGooLargeBlight.SetColor("_TintColor", GooTint);
            */
            //
            matCrocoDiseaseSporeBlight = UnityEngine.Object.Instantiate(CrocoDiseaseGhostBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoDiseaseSporeBlight.name = "matCrocoDiseaseSporeBlight";
            matCrocoDiseaseSporeBlight.SetTexture("_RemapTex", matCrocoGooSmallBlight.GetTexture("_RemapTex"));
            matCrocoDiseaseSporeBlight.SetColor("_TintColor", new Color(1.7f, 0.8f, 1.25f, 1));
            //
            matCrocoDiseaseDrippingsBlight = UnityEngine.Object.Instantiate(CrocoSpitGhostBlight.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoDiseaseDrippingsBlight.name = "matCrocoDiseaseDrippingsBlight";
            matCrocoDiseaseDrippingsBlight.SetTexture("_RemapTex", texRampBeetleBreathBlight);
            //matCrocoDiseaseDrippingsBlight.SetColor("_TintColor", GooTint);
            //
            matCrocoDiseaseTrailBlight = UnityEngine.Object.Instantiate(CrocoDiseaseGhostBlight.transform.GetChild(4).GetChild(0).GetComponent<TrailRenderer>().materials[0]);
            matCrocoDiseaseTrailBlight.name = "matCrocoDiseaseTrailBlight";
            matCrocoDiseaseTrailBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlightAlt);
            matCrocoDiseaseTrailBlight.SetColor("_TintColor", TrailColor);
            //matCrocoDiseaseTrailBlight.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, 1f));
            //
            matCrocoDiseaseTrailLesserBlight = UnityEngine.Object.Instantiate(CrocoDiseaseGhostBlight.transform.GetChild(4).GetChild(1).GetComponent<TrailRenderer>().materials[0]);
            matCrocoDiseaseTrailLesserBlight.name = "matCrocoDiseaseTrailLesserBlight";
            matCrocoDiseaseTrailLesserBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlightAlt);
            matCrocoDiseaseTrailLesserBlight.SetColor("_TintColor", TrailColor);
            //matCrocoDiseaseTrailLesserBlight.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, 1f));
            //
            //matCrocoDiseaseTrailOrangeBlight
            matCrocoDiseaseTrailOrangeBlight = UnityEngine.Object.Instantiate(CrocoDiseaseOrbEffect_Blight.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>().materials[0]);
            matCrocoDiseaseTrailOrangeBlight.name = "matCrocoDiseaseTrailOrangeBlight";
            matCrocoDiseaseTrailOrangeBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlight);
            matCrocoDiseaseTrailOrangeBlight.SetColor("_TintColor", TrailColor);
            //matCrocoDiseaseTrailOrangeBlight.SetColor("_TintColor", new Color(1.732415f, 1.34599f, 0.04085885f, 1f));
            //matCrocoDiseaseTrailOrangeBlight.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, 1f));
            //
            //Material matTracerBrightBlight = UnityEngine.Object.Instantiate();
            //
            matCrocoBiteDiseasedBlight = UnityEngine.Object.Instantiate(CrocoBiteEffectBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoBiteDiseasedBlight.name = "matCrocoBiteDiseasedBlight";
            matCrocoBiteDiseasedBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlightAlt);
            //matCrocoBiteDiseasedBlight.SetColor("_TintColor", new Color32(222, 98, 255, 255));
            matCrocoBiteDiseasedBlight.SetColor("_TintColor", new Color(1.63f, 0.8f, 1.52f, 1));
            //matCrocoBiteDiseasedBlight.SetColor("_TintColor", new Color32(255, 98, 222, 255));
            //matCrocoBiteDiseasedBlight.SetColor("_TintColor", new Color32(200, 98, 225, 255));
            //
            matCrocoDiseaseHeadBlight = UnityEngine.Object.Instantiate(CrocoDiseaseGhostBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoDiseaseHeadBlight.name = "matCrocoDiseaseHeadBlight";
            matCrocoDiseaseHeadBlight.SetTexture("_RemapTex", texRampCrocoDiseaseDarkDark);
            matCrocoDiseaseHeadBlight.SetColor("_TintColor", new Color(2.5f, 1.5f, 1.5f, 1f));
            //
            matCrocoGooDecalBlight = UnityEngine.Object.Instantiate(CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(0).GetComponent<ThreeEyedGames.Decal>().Material);
            matCrocoGooDecalBlight.name = "matCrocoGooDecalBlight";
            matCrocoGooDecalBlight.color = new Color(2f, 0.5f, 1.8f, 1.1f);
            //matCrocoGooDecalBlight.SetColor("_EmissionColor", new Color(1.3f, 1.3f, 1.3f, 1f));
            //matCrocoGooDecalBlight.SetColor("_TintColor", new Color(1.3f, 1.3f, 1.3f, 1f));
            //matCrocoGooDecalBlight.color = new Color(1, 1f, 1);
            //matCrocoGooDecalBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlightAlt);
            //




            //Slash Blight

            Material matCrocoSlashDiseasedBlight = UnityEngine.Object.Instantiate(CrocoSlashBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial);
            matCrocoSlashDiseasedBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlight);
            CrocoSlashBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoSlashDiseasedBlight;

            Material matCrocoSlashDiseasedBrightBlight = UnityEngine.Object.Instantiate(CrocoComboFinisherSlashBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial);
            matCrocoSlashDiseasedBrightBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlight);
            matCrocoSlashDiseasedBrightBlight.SetColor("_TintColor", new Color(0.85f, 0.8f, 0.21f, 1));
            CrocoComboFinisherSlashBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoSlashDiseasedBrightBlight;




            //


            //Bite Blight
            CrocoBiteEffectBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoBiteDiseasedBlight;
            CrocoBiteEffectBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            CrocoBiteEffectBlight.transform.GetChild(3).GetComponent<UnityEngine.ParticleSystem>().startColor = TracerColor;
            //
            //Disease Ghost Blight
            CrocoDiseaseGhostBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            CrocoDiseaseGhostBlight.transform.GetChild(1).GetComponent<Light>().color = new Color(0.949f, 0.8863f, 0f, 1f);
            CrocoDiseaseGhostBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseHeadBlight;
            CrocoDiseaseGhostBlight.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlight;
            CrocoDiseaseGhostBlight.transform.GetChild(4).GetChild(0).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailBlight;
            CrocoDiseaseGhostBlight.transform.GetChild(4).GetChild(1).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailLesserBlight;
            CrocoDiseaseGhostBlight.transform.GetChild(4).GetChild(2).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailLesserBlight;
            //
            //Spit Ghost Blight
            CrocoSpitGhostBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            CrocoSpitGhostBlight.transform.GetChild(1).GetComponent<Light>().color = new Color(0.949f, 0.4596f, 0f, 1f);
            CrocoSpitGhostBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlight;
            CrocoSpitGhostBlight.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseDrippingsBlight;
            CrocoSpitGhostBlight.transform.GetChild(6).GetChild(0).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailBlight;
            CrocoSpitGhostBlight.transform.GetChild(6).GetChild(1).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailBlight;
            //
            //Disease Impact Blight
            CrocoDiseaseImpactEffect_Blight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            CrocoDiseaseImpactEffect_Blight.transform.GetChild(1).GetComponent<UnityEngine.ParticleSystem>().startColor = TracerColor;
            CrocoDiseaseImpactEffect_Blight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseTrailBlight;
            CrocoDiseaseImpactEffect_Blight.transform.GetChild(3).GetComponent<Light>().color = new Color(1f, 0.72f, 0f, 1f);
            //
            //Muzzle Flash Blight
            MuzzleflashCroco_Blight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            MuzzleflashCroco_Blight.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlight;
            MuzzleflashCroco_Blight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlight;
            MuzzleflashCroco_Blight.transform.GetChild(3).GetComponent<UnityEngine.ParticleSystem>().startColor = TracerColor;
            MuzzleflashCroco_Blight.transform.GetChild(4).GetComponent<Light>().color = new Color(1f, 0.72f, 0f, 1f);
            //
            //Leap Fist Effect
            CrocoFistEffectBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlight;
            CrocoFistEffectBlight.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseTrailBlight;
            CrocoFistEffectBlight.transform.GetChild(2).GetComponent<Light>().color = new Color(1f, 0.72f, 0f, 1f);
            //
            //Leap Acid Pool
            CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(0).GetComponent<ThreeEyedGames.Decal>().Material = matCrocoGooDecalBlight;
            CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseDrippingsBlight;
            //CrocoLeapAcidBlight.transform.GetChild(0).GetChild(3).GetComponent<Light>().color = new Color32(255, 110, 177, 255);
            CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(3).GetComponent<Light>().color = new Color32(255, 177, 110, 200);
            //
            //Leap Explosion
            CrocoLeapExplosion_Blight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlightD;
            //CrocoLeapExplosionBlight.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = omnispark;
            CrocoLeapExplosion_Blight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            CrocoLeapExplosion_Blight.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            CrocoLeapExplosion_Blight.transform.GetChild(4).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            //CrocoLeapExplosionBlight.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().sharedMaterial = cryocanister;
            CrocoLeapExplosion_Blight.transform.GetChild(6).GetComponent<Light>().color = new Color32(255, 177, 110, 200);
            //CrocoLeapExplosionBlight.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().sharedMaterial = cryocanister;
            //
            //Disease Orb
            CrocoDiseaseOrbEffect_Blight.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            CrocoDiseaseOrbEffect_Blight.transform.GetChild(1).GetChild(1).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailLesserBlight;
            //
            CrocoDiseaseOrbEffect_Blight.GetComponent<RoR2.Orbs.OrbEffect>().endEffect = CrocoDiseaseImpactEffect_Blight;


            #region Lobby Pool
            GameObject CrocoDisplay = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoDisplay.prefab").WaitForCompletion();
            CrocoDisplay.AddComponent<CharacterSelectSurvivorPreviewDisplayController>();
            GameObject puddle = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoDisplaySpawnEffect.prefab").WaitForCompletion();

            lobbyPool = PrefabAPI.InstantiateClone(puddle, "SpawnBlight", false);
            lobbyPool.transform.GetChild(0).GetChild(0).GetComponent<ThreeEyedGames.Decal>().Material = matCrocoGooDecalBlight;
            lobbyPool.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matCrocoDiseaseSporeBlight;
            lobbyPool.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matCrocoDiseaseDrippingsBlight;
            lobbyPool.transform.GetChild(0).GetChild(3).GetComponent<Light>().color = new Color32(255, 200, 150, 255);
            lobbyPool.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matCrocoGooSmallBlightD;
            lobbyPool.transform.GetChild(1).GetChild(3).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matCrocoGooLargeBlight;
            lobbyPool.transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matCrocoDiseaseSporeBlight;
            lobbyPool.transform.GetChild(1).GetChild(3).GetChild(3).GetComponent<Light>().color = new Color32(100, 75, 50, 255);
            lobbyPool.SetActive(false);


            Object.Destroy(lobbyPool.GetComponent<AkEvent>());
            #endregion
        }


    }
}