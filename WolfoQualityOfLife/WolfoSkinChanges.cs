using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.UI;
using RoR2.ExpansionManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WolfoQualityOfLife
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfoQualityOfLife", "WolfoQualityOfLife", "2.0.5")]
    [R2APISubmoduleDependency(nameof(ContentAddition), nameof(LoadoutAPI), nameof(PrefabAPI), nameof(LanguageAPI), nameof(ItemAPI))]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]


    public class SkinChanges : BaseUnityPlugin
    {
        static readonly System.Random random = new System.Random();

        public static Material MatOniSword;
        public static Material MatHANDToolbot;
        public static Material MatGreenFlowerRex;
        public static Material MatEngiTurretGreen;
        public static Material MatEngiAltTrail;

        public static uint LoaderPylonSkinIndex = 255;
        //public static uint MULTCoverPrefab = 255;
        public static uint REXSkinMortar = 255;
        public static uint REXSkinShock = 255;
        public static uint REXSkinFlowerP = 255;
        //public static uint REXSkinFlowerSpawn = 255;
        //public static uint REXSkinFlowerE = 255;
        public static uint REXSkinFlowerEnter = 255;
        public static uint REXSkinFlowerExit = 255;
        //public static uint REXSkinFlowerR = 255;
        public static Material REXFlowerTempMat = null;


        public static uint MercSkinExpose = 255;
        //public static uint MercSkinPrimary = 255;
        public static uint MercSkinSecondary = 255;
        public static uint MercSkinSecondaryAlt = 255;
        //public static uint MercSkinUtility = 255;
        //public static uint MercSkinUtilityAlt = 255;
        public static uint MercSkinSpecial = 255;
        //public static uint MercSkinSpecialAlt = 255;


        public static Texture2D texRampHuntressSoft = null;
        public static Texture2D texRampHuntressSoftRed = null;

        public static Material matMercDelayedBillboard2Red = null;
        public static Material matMercFocusedAssaultIconRed = null;
        public static Material matMercExposedBackdropRed = null;

        public static Material matMercSwipe1Red = null;
        public static Material matMercSwipe2Red = null;
        public static Material matMercSwipe3Red = null;

        public static Material matMercIgnitionRed = null;

        public static Material matMercExposedSlashRed = null;
        public static Material matOmniHitspark3MercRed = null;
        public static Material matOmniRadialSlash1MercRed = null;
        public static Material matOmniHitspark4MercRed = null;


        public static Material matMercEnergized = RoR2.LegacyResourcesAPI.Load<Material>("materials/matMercEnergized");
        public static Material matMercEnergizedRed = Instantiate(matMercEnergized);
        //public static Material matMercEvisTargetRed = RoR2.LegacyResourcesAPI.Load<Material>("materials/matMercEvisTarget");
        //public static Material matMercEvisTarget = RoR2.LegacyResourcesAPI.Load<Material>("materials/matMercEvisTarget");
        public static Material matMercHologramRed = RoR2.LegacyResourcesAPI.Load<Material>("materials/matMercHologram");
        //public static Material matMercHologram = RoR2.LegacyResourcesAPI.Load<Material>("materials/matMercHologram");

        //public static Material matHuntressFlashBrightRed = RoR2.LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashBright");
        public static Material matHuntressFlashBright = RoR2.LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashBright");
        //public static Material matHuntressFlashExpandedRed = RoR2.LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashExpanded");
        public static Material matHuntressFlashExpanded = RoR2.LegacyResourcesAPI.Load<Material>("materials/matHuntressFlashExpanded");


        public static GameObject MercFocusedAssaultOrbEffectRed = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/MercFocusedAssaultOrbEffect"), "MercFocusedAssaultOrbEffectRed", false);

        public static GameObject OmniImpactVFXSlashMerc = null; //Primary, Secondary1, Secondary2
        //public static GameObject MercSwordSlash = null; //Primary
        public static GameObject MercSwordFinisherSlash = null; //Primary, Special2

        public static GameObject MercSwordSlashWhirlwind = null;  //Secondary1
        //public static GameObject MercDashHitOverlay = null; //Utility1, Utility2




        public static GameObject MercSwordUppercutSlash = null;  //Secondary2


        public static GameObject OmniImpactVFXSlashMercRed = null; //Primary, Secondary1, Secondary2
        public static GameObject MercSwordSlashRed = null; //Primary
        public static GameObject MercSwordFinisherSlashRed = null; //Primary, Special2

        public static GameObject MercSwordSlashWhirlwindRed = null;  //Secondary1
        public static GameObject MercDashHitOverlayRed = null; //Utility1, Utility2



        public static GameObject HuntressBlinkEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/HuntressBlinkEffect"); //Special1 (Enter)
        public static GameObject HuntressBlinkEffectRed = R2API.PrefabAPI.InstantiateClone(HuntressBlinkEffect, "HuntressBlinkEffectRed", false); //Special1 (Enter)

        public static GameObject HuntressFireArrowRain = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/HuntressFireArrowRain"); //Special1 (Attack)
        public static GameObject HuntressFireArrowRainRed = R2API.PrefabAPI.InstantiateClone(HuntressFireArrowRain, "HuntressFireArrowRainRed", false); //Special1 (Attack)

        public static GameObject OmniImpactVFXSlashMercEvis = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/omnieffect/OmniImpactVFXSlashMercEvis"); //Special1 (Attack)
        public static GameObject OmniImpactVFXSlashMercEvisRed = R2API.PrefabAPI.InstantiateClone(OmniImpactVFXSlashMercEvis, "OmniImpactVFXSlashMercEvisRed", false); //Special1 (Attack)

        public static Material matHuntressSwipeRed;
        public static Material matHuntressChargedRed;



        public static GameObject MercSwordUppercutSlashRed = null;  //Secondary2
        public static GameObject ImpactMercFocusedAssaultRed = null;  //Utility2
        public static GameObject ImpactMercAssaulterRed = null;  //Utility2
        public static GameObject MercAssaulterEffectRed = null;  //Utility2

        public static GameObject EvisProjectileRed = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/EvisProjectile"), "EvisProjectileRed", true);  //Special2
        public static GameObject EvisProjectileGhostRed = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisProjectileGhost"), "EvisProjectileGhostRed", false);
        public static GameObject EvisOverlapProjectileRed = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/EvisOverlapProjectile"), "EvisOverlapProjectileRed", true);
        public static GameObject EvisOverlapProjectileGhostRed = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EvisOverlapProjectileGhost"), "EvisOverlapProjectileGhostRed", false);

        public static GameObject ImpactMercEvisRed = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/ImpactMercEvis"), "ImpactMercEvisRed", false);

        public static Material matMercExposed;
        public static Material matMercExposedRed;
        public static Material matMercExposedBackdrop;


        public static GameObject MercExposeEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/temporaryvisualeffects/MercExposeEffect");
        public static GameObject MercExposeConsumeEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/MercExposeConsumeEffect");
        public static GameObject MercExposeEffectRed = R2API.PrefabAPI.InstantiateClone(MercExposeEffect, "MercExposeEffectRed", false);
        public static GameObject MercExposeConsumeEffectRed = R2API.PrefabAPI.InstantiateClone(MercExposeConsumeEffect, "MercExposeConsumeEffectRed", false);



        //Fucking Croco Blight
        public static Material matCrocoGooSmallBlightD;
        public static Material matCrocoGooLargeBlight;
        public static Material matCrocoGooSmallBlight;
        public static Material matCrocoDiseaseSporeBlight;
        public static Material matCrocoDiseaseDrippingsBlight;
        public static Material matCrocoDiseaseTrailBlight;
        public static Material matCrocoDiseaseTrailLesserBlight;
        public static Material matCrocoDiseaseTrailOrangeBlight;
        public static Material matCrocoBiteDiseasedBlight;
        public static Material matCrocoDiseaseHeadBlight;
        public static Material matCrocoGooDecalBlight;


        public static GameObject CrocoBiteEffectBlight = null; //CrocoBiteEffect //Bite

        public static GameObject CrocoSlashBlight = null; //CrocoSlash //Primary
        public static GameObject CrocoComboFinisherSlashBlight = null; //CrocoSlashFinisher //Primary



        public static GameObject CrocoDiseaseProjectileBlight = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoDiseaseProjectile"), "CrocoDiseaseProjectileBlight", true); //CrocoDiseaseProjectile //Spit + Disease
        public static GameObject CrocoSpitBlight = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoSpit"), "CrocoSpitBlight", true); //CrocoSpit //Spit + Disease

        public static GameObject CrocoDiseaseOrbEffectBlight = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/CrocoDiseaseOrbEffect"), "CrocoDiseaseOrbEffectBlight", false); //CrocoDiseaseOrbEffect 
        public static GameObject CrocoDiseaseGhostBlight = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/CrocoDiseaseGhost"), "CrocoDiseaseProjectileGhostBlight", false); //CrocoDiseaseProjectile //Spit + Disease
        public static GameObject CrocoSpitGhostBlight = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/CrocoSpitGhost"), "CrocoSpitGhostBlight", false); //CrocoSpit //Spit + Disease

        public static GameObject CrocoDiseaseImpactEffectBlight = null;
        public static GameObject MuzzleflashCrocoBlight = null;

        public static GameObject CrocoLeapExplosionBlight = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/CrocoLeapExplosion"), "CrocoLeapExplosionBlight", false); //CrocoLeapExplosion //Leap
        public static GameObject CrocoFistEffectBlight = null; //CrocoFistEffect //Leap
        public static GameObject CrocoChainableFistEffect = null; //CrocoFistEffect //Leap
        public static GameObject CrocoLeapAcidBlight = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoLeapAcid"), "CrocoLeapAcidBlight", true); //CrocoLeapAcid //Leap
        public static GameObject CrocoLeapAcidPoison = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoLeapAcid"); //CrocoLeapAcid //Leap

        public static bool BlightedOrb = false;

        public static GameObject CommandCubeDisplay = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/CommandCube").transform.GetChild(0).gameObject, "CommandCubeDisplayNoSparkle", false);

        public static Material BellBalls = null;
        private static bool LastBallsElite = false;
        private static bool LastBallsElite2 = false;
        private static EquipmentIndex BellEquipIndex = EquipmentIndex.None;



        public static GameObject BellBallElite = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/BellBall"), "BellBallElite", true);
        public static GameObject BellBallGhostElite = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/BellBallGhost"), "BellBallGhostElite", false);




        private static Rect rec128 = new Rect(0, 0, 128, 128);
        private static Vector2 half = new Vector2(0.5f, 0.5f);
        public static bool DidEliteBrassBalls = true;

        public static void CrocoBlightChanger()
        {

            //Blight Billboard


            GameObject CrocoDisplay = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/characterdisplays/CrocoDisplay");
            if (CrocoDisplay)
            {
                CrocoDisplay.AddComponent<CharacterSelectSurvivorPreviewDisplayController>();
            }
            //CharacterSelectSurvivorPreviewDisplayController CrocoCSSPD = CrocoDisplay.AddComponent<CharacterSelectSurvivorPreviewDisplayController>();
            /*
            CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse CrocoChangeToPoison = new CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse
            {
                triggerSkillFamily = CrocoPassiveSkillFamily,
                triggerSkill = CrocoPassivePoison,
            };
            CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse CrocoChangeToBlight = new CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse
            {
                triggerSkillFamily = CrocoPassiveSkillFamily,
                triggerSkill = CrocoPassiveBlight,
            };
            CrocoCSSPD.skillChangeResponses = new CharacterSelectSurvivorPreviewDisplayController.SkillChangeResponse[] { CrocoChangeToBlight, CrocoChangeToPoison };
            */





            Texture2D texRampCrocoDiseaseBlight = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampCrocoDiseaseBlight.LoadImage(Properties.Resources.texRampCrocoDiseaseBlight, false);
            texRampCrocoDiseaseBlight.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseBlight.name = "texRampCrocoDiseaseBlight";
            texRampCrocoDiseaseBlight.wrapMode = TextureWrapMode.Clamp;
            /*
            Texture2D texRampCrocoDiseaseDarkBlight = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampCrocoDiseaseDarkBlight.LoadImage(Properties.Resources.texRampCrocoDiseaseDarkBlight, false);
            texRampCrocoDiseaseDarkBlight.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseDarkBlight.name = "texRampCrocoDiseaseDarkBlight";
            texRampCrocoDiseaseDarkBlight.wrapMode = TextureWrapMode.Clamp;
            */
            Texture2D texRampCrocoDiseaseDarkDark = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampCrocoDiseaseDarkDark.LoadImage(Properties.Resources.texRampCrocoDiseaseDarkDark, false);
            texRampCrocoDiseaseDarkDark.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseDarkDark.name = "texRampCrocoDiseaseDarkDark";
            texRampCrocoDiseaseDarkDark.wrapMode = TextureWrapMode.Clamp;

            /*
            Texture2D texRampCrocoDiseaseDarkDarkAlt = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampCrocoDiseaseDarkDarkAlt.LoadImage(Properties.Resources.texRampCrocoDiseaseDarkDarkAlt, false);
            texRampCrocoDiseaseDarkDarkAlt.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseDarkDarkAlt.name = "texRampCrocoDiseaseDarkDarkAlt";
            texRampCrocoDiseaseDarkDarkAlt.wrapMode = TextureWrapMode.Clamp;
            */
            /*
            Texture2D texRampCrocoDiseaseBlightPurple = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampCrocoDiseaseBlightPurple.LoadImage(Properties.Resources.texRampCrocoDiseaseBlightPurple, false);
            texRampCrocoDiseaseBlightPurple.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseBlightPurple.name = "texRampCrocoDiseaseBlightPurple";
            texRampCrocoDiseaseBlightPurple.wrapMode = TextureWrapMode.Clamp;
            */
            Texture2D texRampCrocoDiseaseBlightAlt = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampCrocoDiseaseBlightAlt.LoadImage(Properties.Resources.texRampCrocoDiseaseBlightAlt, false);
            texRampCrocoDiseaseBlightAlt.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseBlightAlt.name = "texRampCrocoDiseaseBlightAlt";
            texRampCrocoDiseaseBlightAlt.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampBeetleBreathBlight = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampBeetleBreathBlight.LoadImage(Properties.Resources.texRampBeetleBreathBlight, false);
            texRampBeetleBreathBlight.filterMode = FilterMode.Bilinear;
            texRampBeetleBreathBlight.name = "texRampBeetleBreathBlight";
            texRampBeetleBreathBlight.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampCrocoDiseaseDarkLessDark = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampCrocoDiseaseDarkLessDark.LoadImage(Properties.Resources.texRampCrocoDiseaseDarkLessDark, false);
            texRampCrocoDiseaseDarkLessDark.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseDarkLessDark.name = "texRampCrocoDiseaseDarkLessDark";
            texRampCrocoDiseaseDarkLessDark.wrapMode = TextureWrapMode.Clamp;

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
            matCrocoGooSmallBlight = Instantiate(CrocoChainableFistEffect.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoGooSmallBlight.name = "matCrocoGooSmallBlight";
            matCrocoGooSmallBlight.SetColor("_EmissionColor", new Color(1.4f, 0.95f, 0.95f, 1f));
            matCrocoGooSmallBlight.SetColor("_TintColor", new Color(1.1f, 0.9f, 0.9f, 1f));


            matCrocoGooSmallBlightD = Instantiate(CrocoChainableFistEffect.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[0]);
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
            matCrocoGooLargeBlight = Instantiate(CrocoBiteEffectBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().materials[0]);
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
            matCrocoDiseaseSporeBlight = Instantiate(CrocoDiseaseGhostBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoDiseaseSporeBlight.name = "matCrocoDiseaseSporeBlight";
            matCrocoDiseaseSporeBlight.SetTexture("_RemapTex", matCrocoGooSmallBlight.GetTexture("_RemapTex"));
            matCrocoDiseaseSporeBlight.SetColor("_TintColor", new Color(1.7f, 0.8f, 1.25f, 1));
            //
            matCrocoDiseaseDrippingsBlight = Instantiate(CrocoSpitGhostBlight.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoDiseaseDrippingsBlight.name = "matCrocoDiseaseDrippingsBlight";
            matCrocoDiseaseDrippingsBlight.SetTexture("_RemapTex", texRampBeetleBreathBlight);
            //matCrocoDiseaseDrippingsBlight.SetColor("_TintColor", GooTint);
            //
            matCrocoDiseaseTrailBlight = Instantiate(CrocoDiseaseGhostBlight.transform.GetChild(4).GetChild(0).GetComponent<TrailRenderer>().materials[0]);
            matCrocoDiseaseTrailBlight.name = "matCrocoDiseaseTrailBlight";
            matCrocoDiseaseTrailBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlightAlt);
            matCrocoDiseaseTrailBlight.SetColor("_TintColor", TrailColor);
            //matCrocoDiseaseTrailBlight.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, 1f));
            //
            matCrocoDiseaseTrailLesserBlight = Instantiate(CrocoDiseaseGhostBlight.transform.GetChild(4).GetChild(1).GetComponent<TrailRenderer>().materials[0]);
            matCrocoDiseaseTrailLesserBlight.name = "matCrocoDiseaseTrailLesserBlight";
            matCrocoDiseaseTrailLesserBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlightAlt);
            matCrocoDiseaseTrailLesserBlight.SetColor("_TintColor", TrailColor);
            //matCrocoDiseaseTrailLesserBlight.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, 1f));
            //
            //matCrocoDiseaseTrailOrangeBlight
            matCrocoDiseaseTrailOrangeBlight = Instantiate(CrocoDiseaseOrbEffectBlight.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>().materials[0]);
            matCrocoDiseaseTrailOrangeBlight.name = "matCrocoDiseaseTrailOrangeBlight";
            matCrocoDiseaseTrailOrangeBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlight);
            matCrocoDiseaseTrailOrangeBlight.SetColor("_TintColor", TrailColor);
            //matCrocoDiseaseTrailOrangeBlight.SetColor("_TintColor", new Color(1.732415f, 1.34599f, 0.04085885f, 1f));
            //matCrocoDiseaseTrailOrangeBlight.SetColor("_TintColor", new Color(0.8f, 0.8f, 0.8f, 1f));
            //
            //Material matTracerBrightBlight = Instantiate();
            //
            matCrocoBiteDiseasedBlight = Instantiate(CrocoBiteEffectBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoBiteDiseasedBlight.name = "matCrocoBiteDiseasedBlight";
            matCrocoBiteDiseasedBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlightAlt);
            //matCrocoBiteDiseasedBlight.SetColor("_TintColor", new Color32(222, 98, 255, 255));
            matCrocoBiteDiseasedBlight.SetColor("_TintColor", new Color(1.63f, 0.8f, 1.52f, 1));
            //matCrocoBiteDiseasedBlight.SetColor("_TintColor", new Color32(255, 98, 222, 255));
            //matCrocoBiteDiseasedBlight.SetColor("_TintColor", new Color32(200, 98, 225, 255));
            //
            matCrocoDiseaseHeadBlight = Instantiate(CrocoDiseaseGhostBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().materials[0]);
            matCrocoDiseaseHeadBlight.name = "matCrocoDiseaseHeadBlight";
            matCrocoDiseaseHeadBlight.SetTexture("_RemapTex", texRampCrocoDiseaseDarkDark);
            matCrocoDiseaseHeadBlight.SetColor("_TintColor", new Color(2.5f, 1.5f, 1.5f, 1f));
            //
            matCrocoGooDecalBlight = Instantiate(CrocoLeapAcidBlight.transform.GetChild(0).GetChild(0).GetComponent<ThreeEyedGames.Decal>().Material);
            matCrocoGooDecalBlight.name = "matCrocoGooDecalBlight";
            matCrocoGooDecalBlight.color = new Color(2f, 0.5f, 1.8f, 1.1f);
            //matCrocoGooDecalBlight.SetColor("_EmissionColor", new Color(1.3f, 1.3f, 1.3f, 1f));
            //matCrocoGooDecalBlight.SetColor("_TintColor", new Color(1.3f, 1.3f, 1.3f, 1f));
            //matCrocoGooDecalBlight.color = new Color(1, 1f, 1);
            //matCrocoGooDecalBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlightAlt);
            //




            //Slash Blight

            Material matCrocoSlashDiseasedBlight = Instantiate(CrocoSlashBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial);
            matCrocoSlashDiseasedBlight.SetTexture("_RemapTex", texRampCrocoDiseaseBlight);
            CrocoSlashBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoSlashDiseasedBlight;

            Material matCrocoSlashDiseasedBrightBlight = Instantiate(CrocoComboFinisherSlashBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial);
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
            CrocoDiseaseImpactEffectBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            CrocoDiseaseImpactEffectBlight.transform.GetChild(1).GetComponent<UnityEngine.ParticleSystem>().startColor = TracerColor;
            CrocoDiseaseImpactEffectBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseTrailBlight;
            CrocoDiseaseImpactEffectBlight.transform.GetChild(3).GetComponent<Light>().color = new Color(1f, 0.72f, 0f, 1f);
            //
            //Muzzle Flash Blight
            MuzzleflashCrocoBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            MuzzleflashCrocoBlight.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlight;
            MuzzleflashCrocoBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlight;
            MuzzleflashCrocoBlight.transform.GetChild(3).GetComponent<UnityEngine.ParticleSystem>().startColor = TracerColor;
            MuzzleflashCrocoBlight.transform.GetChild(4).GetComponent<Light>().color = new Color(1f, 0.72f, 0f, 1f);
            //
            //Leap Fist Effect
            CrocoFistEffectBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlight;
            CrocoFistEffectBlight.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseTrailBlight;
            CrocoFistEffectBlight.transform.GetChild(2).GetComponent<Light>().color = new Color(1f, 0.72f, 0f, 1f);
            //
            //Leap Acid Pool
            CrocoLeapAcidBlight.transform.GetChild(0).GetChild(0).GetComponent<ThreeEyedGames.Decal>().Material = matCrocoGooDecalBlight;
            CrocoLeapAcidBlight.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            CrocoLeapAcidBlight.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseDrippingsBlight;
            //CrocoLeapAcidBlight.transform.GetChild(0).GetChild(3).GetComponent<Light>().color = new Color32(255, 110, 177, 255);
            CrocoLeapAcidBlight.transform.GetChild(0).GetChild(3).GetComponent<Light>().color = new Color32(255, 177, 110, 200);
            //
            //Leap Explosion
            CrocoLeapExplosionBlight.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooSmallBlightD;
            //CrocoLeapExplosionBlight.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = omnispark;
            CrocoLeapExplosionBlight.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            CrocoLeapExplosionBlight.transform.GetChild(3).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoGooLargeBlight;
            CrocoLeapExplosionBlight.transform.GetChild(4).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            //CrocoLeapExplosionBlight.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().sharedMaterial = cryocanister;
            CrocoLeapExplosionBlight.transform.GetChild(6).GetComponent<Light>().color = new Color32(255, 177, 110, 200);
            //CrocoLeapExplosionBlight.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().sharedMaterial = cryocanister;
            //
            //Disease Orb
            CrocoDiseaseOrbEffectBlight.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            CrocoDiseaseOrbEffectBlight.transform.GetChild(1).GetChild(1).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailLesserBlight;
            //



            CrocoSpitBlight.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = CrocoSpitGhostBlight;
            CrocoSpitBlight.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect = CrocoDiseaseImpactEffectBlight;
            CrocoDiseaseProjectileBlight.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = CrocoDiseaseGhostBlight;
            CrocoDiseaseProjectileBlight.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect = CrocoDiseaseImpactEffectBlight;
            CrocoDiseaseOrbEffectBlight.GetComponent<RoR2.Orbs.OrbEffect>().endEffect = CrocoDiseaseImpactEffectBlight;

            R2API.ContentAddition.AddProjectile(CrocoDiseaseProjectileBlight);
            R2API.ContentAddition.AddProjectile(CrocoSpitBlight);


            if (WolfoMain.BlightAcrid.Value == true)
            {

                On.EntityStates.Croco.Slash.BeginMeleeAttackEffect += (orig, self) =>
                {

                    if (self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
                    {
                        if (self.isComboFinisher)
                        {
                            self.swingEffectPrefab = CrocoComboFinisherSlashBlight;
                        }
                        else
                        {
                            self.swingEffectPrefab = CrocoSlashBlight;
                        }
                    };
                    orig(self);
                };








                On.RoR2.Orbs.LightningOrb.Begin += (orig, self) =>
                {
                    if (self.damageType == DamageType.BlightOnHit) { BlightedOrb = true; }
                    else { BlightedOrb = false; }
                    orig(self);
                };

                On.EntityStates.Croco.FireSpit.OnEnter += (orig, self) =>
                {
                    if (self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
                    {
                        //Debug.Log(self.projectilePrefab);
                        if (self.projectilePrefab.name.StartsWith("CrocoSpit"))
                        {
                            self.projectilePrefab = CrocoSpitBlight;
                        }
                        else if (self.projectilePrefab.name.StartsWith("CrocoDiseaseProjectile"))
                        {
                            self.projectilePrefab = CrocoDiseaseProjectileBlight;
                        }
                        self.effectPrefab = MuzzleflashCrocoBlight;
                    };
                    orig(self);
                };


                On.EntityStates.Croco.Bite.BeginMeleeAttackEffect += (orig, self) =>
                {
                    if (self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
                    {
                        self.swingEffectPrefab = CrocoBiteEffectBlight;
                    };
                    orig(self);
                };


                On.EntityStates.Croco.BaseLeap.DoImpactAuthority += (orig, self) =>
                {
                    if (self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
                    {
                        if (self.blastEffectPrefab.name.StartsWith("CrocoLeapExplosion"))
                        {
                            self.blastEffectPrefab = CrocoLeapExplosionBlight;
                            EntityStates.Croco.BaseLeap.projectilePrefab = CrocoLeapAcidBlight;
                        };
                    }
                    else if (EntityStates.Croco.BaseLeap.projectilePrefab == CrocoLeapAcidBlight)
                    {
                        EntityStates.Croco.BaseLeap.projectilePrefab = CrocoLeapAcidPoison;
                    }
                    orig(self);
                };

                On.EntityStates.Croco.BaseLeap.OnEnter += (orig, self) =>
                {
                    if (self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
                    {
                        if (self.fistEffectPrefab.name.StartsWith("CrocoFistEffect"))
                        {
                            self.fistEffectPrefab = CrocoFistEffectBlight;
                        };
                    };
                    orig(self);
                };










                Texture2D texCrocoDiffuseBlight = new Texture2D(1024, 1024, TextureFormat.DXT5, false);
                texCrocoDiffuseBlight.LoadImage(Properties.Resources.texCrocoDiffuseBlight, false);
                texCrocoDiffuseBlight.filterMode = FilterMode.Bilinear;

                Texture2D texCrocoEmissionBlight = new Texture2D(2048, 2048, TextureFormat.DXT5, false);
                texCrocoEmissionBlight.LoadImage(Properties.Resources.texCrocoEmissionBlight, false);
                texCrocoEmissionBlight.filterMode = FilterMode.Bilinear;

                Texture2D texCrocoPoisonMaskBlight = new Texture2D(512, 512, TextureFormat.DXT5, false);
                texCrocoPoisonMaskBlight.LoadImage(Properties.Resources.texCrocoPoisonMaskBlight, false);
                texCrocoPoisonMaskBlight.filterMode = FilterMode.Bilinear;

                Texture2D texCrocoBlightSkin = new Texture2D(128, 128, TextureFormat.DXT5, false);
                texCrocoBlightSkin.LoadImage(Properties.Resources.texCrocoBlightSkin, false);
                texCrocoBlightSkin.filterMode = FilterMode.Bilinear;
                Sprite texCrocoBlightSkinS = Sprite.Create(texCrocoBlightSkin, rec128, half);

                Texture2D texCrocoSkinFlow = new Texture2D(256, 16, TextureFormat.DXT5, false);
                texCrocoSkinFlow.LoadImage(Properties.Resources.texCrocoSkinFlow, false);
                texCrocoSkinFlow.filterMode = FilterMode.Bilinear;
                texCrocoSkinFlow.wrapMode = TextureWrapMode.Clamp;


                SkinDef SkinDefAcridDefault = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").transform.GetChild(0).GetChild(2).gameObject.GetComponent<ModelSkinController>().skins[0];

                CharacterModel.RendererInfo[] AcridBlightedRenderInfos = new CharacterModel.RendererInfo[4];
                Array.Copy(SkinDefAcridDefault.rendererInfos, AcridBlightedRenderInfos, 4);

                Material matCrocoBlight = Instantiate(SkinDefAcridDefault.rendererInfos[0].defaultMaterial);


                matCrocoBlight.mainTexture = texCrocoDiffuseBlight;
                matCrocoBlight.SetTexture("_EmTex", texCrocoEmissionBlight);
                matCrocoBlight.SetTexture("_FlowHeightmap", texCrocoPoisonMaskBlight);
                matCrocoBlight.SetTexture("_FlowHeightRamp", texRampCrocoDiseaseDarkLessDark);//texRampCrocoDiseaseBlight
                matCrocoBlight.SetColor("_EmColor", new Color(1.1f, 1f, 1.1f, 1));


                AcridBlightedRenderInfos[0].defaultMaterial = matCrocoBlight;  //matCroco
                AcridBlightedRenderInfos[2].defaultMaterial = matCrocoDiseaseDrippingsBlight; //matCrocoDiseaseDrippings



                if (WolfoMain.SkinAcridBlight.Value == true)
                {
                    LoadoutAPI.SkinDefInfo AcridBlightSkinInfo = new LoadoutAPI.SkinDefInfo
                    {
                        BaseSkins = SkinDefAcridDefault.baseSkins,

                        NameToken = "Default Blighted",
                        UnlockableDef = RoR2.LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Skills.Croco.PassivePoisonLethal"),
                        RootObject = SkinDefAcridDefault.rootObject,
                        RendererInfos = AcridBlightedRenderInfos,
                        Name = "skinCrocoDefaultBlighted",
                        Icon = texCrocoBlightSkinS,
                    };
                    LoadoutAPI.AddSkinToCharacter(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody"), AcridBlightSkinInfo);

                }


            }




        }

        public static void MercRedEffects()
        {


            Texture2D texRampFallbootsRed = new Texture2D(256, 16, TextureFormat.DXT1, false);
            texRampFallbootsRed.LoadImage(Properties.Resources.texRampFallbootsRed, false);
            texRampFallbootsRed.filterMode = FilterMode.Bilinear;
            texRampFallbootsRed.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampMercDustRed = new Texture2D(128, 4, TextureFormat.DXT5, false);
            texRampMercDustRed.LoadImage(Properties.Resources.texRampMercDustRed, false);
            texRampMercDustRed.filterMode = FilterMode.Bilinear;
            texRampMercDustRed.wrapMode = TextureWrapMode.Clamp;

            texRampHuntressSoft = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampHuntressSoft.LoadImage(Properties.Resources.texRampHuntressSoft, false);
            texRampHuntressSoft.wrapMode = TextureWrapMode.Clamp;
            texRampHuntressSoft.filterMode = FilterMode.Point;

            texRampHuntressSoftRed = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampHuntressSoftRed.LoadImage(Properties.Resources.texRampHuntressSoftRed, false);
            texRampHuntressSoftRed.wrapMode = TextureWrapMode.Clamp;
            texRampHuntressSoftRed.filterMode = FilterMode.Point;

            Texture2D texRampHuntressRed = new Texture2D(256, 16, TextureFormat.DXT1, false);
            texRampHuntressRed.LoadImage(Properties.Resources.texRampHuntressRed, false);
            texRampHuntressRed.wrapMode = TextureWrapMode.Clamp;
            texRampHuntressRed.filterMode = FilterMode.Point;

            ParticleSystemRenderer MercSwordSlashRedRenderer0 = MercSwordSlashRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercSwipe1Red = Instantiate(MercSwordSlashRedRenderer0.material);
            matMercSwipe1Red.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercSwipe1Red.SetColor("_TintColor", new Color(1, 0, 0, 1));        //Default Color : {r: 0, g: 0.314069, b: 1, a: 1}
            MercSwordSlashRedRenderer0.material = matMercSwipe1Red;

            //Child 0 has weird TracerBright
            ParticleSystem MercSwordFinisherSlashRedParticle0 = MercSwordFinisherSlashRed.transform.GetChild(0).GetComponent<ParticleSystem>();
            MercSwordFinisherSlashRedParticle0.startColor = new Color(1, 0.2f, 0.2f, 1);
            ParticleSystemRenderer MercSwordFinisherSlashRedRenderer1 = MercSwordFinisherSlashRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercSwipe2Red = Instantiate(MercSwordFinisherSlashRedRenderer1.material);
            matMercSwipe2Red.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercSwipe2Red.SetColor("_TintColor", new Color(1, 0.22f, 0.22f, 1));  //Default Color r: 0.3632075, g: 0.6593511, b: 1, a: 1
            MercSwordFinisherSlashRedRenderer1.material = matMercSwipe2Red;

            ParticleSystemRenderer MercSwordSlashWhirlwindRedRenderer0 = MercSwordSlashWhirlwindRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            MercSwordSlashWhirlwindRedRenderer0.material = matMercSwipe1Red;

            ParticleSystemRenderer MercSwordUppercutSlashRedRenderer0 = MercSwordUppercutSlashRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            ParticleSystemRenderer MercSwordUppercutSlashRedRenderer1 = MercSwordUppercutSlashRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            MercSwordUppercutSlashRedRenderer0.material = matMercSwipe2Red;
            MercSwordUppercutSlashRedRenderer1.material = matMercSwipe1Red;




            OmniEffect OmniImpactVFXSlashMercRedOmniEffect = OmniImpactVFXSlashMercRed.GetComponent<OmniEffect>();


            //Material matOmniHitspark4Merc = Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial);
            //OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4Merc;

            Material matOmniRadialSlash1Merc = Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1Merc.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1Merc;

            Material matOmniHitspark3Merc = Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniHitspark3Merc.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3Merc;

            Material matOmniHitspark2MercRed = Instantiate(OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial);
            matOmniHitspark2MercRed.SetTexture("_RemapTex", texRampMercDustRed);
            OmniImpactVFXSlashMercRedOmniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2MercRed;

            ParticleSystem OmniImpactVFXSlashMercRedParticle1 = OmniImpactVFXSlashMercRed.transform.GetChild(1).GetComponent<ParticleSystem>(); //matOmniHitspark3 (Instance)
            OmniImpactVFXSlashMercRedParticle1.startColor = new Color(0.7264f, 0.3f, 0.3f, 1); //Default C0lor 0 0.7264 0.7039 1

            ParticleSystem OmniImpactVFXSlashMercRedParticle2 = OmniImpactVFXSlashMercRed.transform.GetChild(2).GetComponent<ParticleSystem>(); //matGenericFlash (Instance)
            OmniImpactVFXSlashMercRedParticle2.startColor = new Color(0.9333f, 0.2f, 0.2f, 1); //0 0.4951 0.9333 1

            ParticleSystem OmniImpactVFXSlashMercRedParticle3 = OmniImpactVFXSlashMercRed.transform.GetChild(3).GetComponent<ParticleSystem>(); //matTracerBright (Instance)
            OmniImpactVFXSlashMercRedParticle3.startColor = new Color(0.5f, 0.4245f, 0.0501f, 1); //0.3854 0.4245 0.0501 1





            //Figure out if start color needs to actually be changed because they all use it
            ParticleSystemRenderer MercAssaulterEffectRedRenderer5 = MercAssaulterEffectRed.transform.GetChild(5).GetComponent<ParticleSystemRenderer>();
            matMercIgnitionRed = Instantiate(MercAssaulterEffectRedRenderer5.material);
            matMercIgnitionRed.SetTexture("_RemapTex", texRampHuntressRed);
            matMercIgnitionRed.SetColor("_TintColor", new Color(0.8867924f, 0.06f, 0.06f, 1)); //{r: 0, g: 0.1362783, b: 0.8867924, a: 1}
            MercAssaulterEffectRedRenderer5.material = matMercIgnitionRed;

            MercAssaulterEffectRed.transform.GetChild(6).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.3f, 0.3f, 1); //0 0.4409 0.9811 1
            MercAssaulterEffectRed.transform.GetChild(8).GetComponent<Light>().color = new Color(0.9804f, 0.3765f, 0.4868f, 1); //0.3765 0.4868 0.9804 1
            MercAssaulterEffectRed.transform.GetChild(9).GetComponent<TrailRenderer>().material = matMercSwipe1Red;
            MercAssaulterEffectRed.transform.GetChild(10).GetChild(2).GetComponent<TrailRenderer>().material = matMercIgnitionRed;
            MercAssaulterEffectRed.transform.GetChild(10).GetChild(3).GetComponent<TrailRenderer>().material = matMercIgnitionRed;




            ParticleSystem particleSystem = ImpactMercAssaulterRed.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.45f, 0.45f, 1f);//0.3538 0.6316 1 1
            particleSystem = ImpactMercAssaulterRed.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.575f, 0.575f, 1f);//0.467 0.7022 1 1

            ParticleSystemRenderer particleSystemRenderer = ImpactMercAssaulterRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercSwipe1Red;




            particleSystem = ImpactMercFocusedAssaultRed.transform.GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(1f, 0.45f, 0.45f, 0.2667f);//0.3538 0.6316 1 0.2667
            particleSystem = ImpactMercFocusedAssaultRed.transform.GetChild(1).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.934f, 0.26f, 0.26f, 1f);//0.0925 0.4637 0.934 1
            particleSystemRenderer = ImpactMercFocusedAssaultRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercSwipe3Red = Instantiate(particleSystemRenderer.material);
            matMercSwipe3Red.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercSwipe3Red;



            particleSystemRenderer = MercFocusedAssaultOrbEffectRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercDelayedBillboard2Red = Instantiate(particleSystemRenderer.material);
            matMercDelayedBillboard2Red.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercDelayedBillboard2Red;

            particleSystemRenderer = MercFocusedAssaultOrbEffectRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
            matMercFocusedAssaultIconRed = Instantiate(particleSystemRenderer.material);
            matMercFocusedAssaultIconRed.SetTexture("_RemapTex", texRampMercDustRed);
            particleSystemRenderer.material = matMercFocusedAssaultIconRed;

            particleSystemRenderer = MercFocusedAssaultOrbEffectRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            matMercExposedBackdropRed = Instantiate(particleSystemRenderer.material);
            matMercExposedBackdrop = Instantiate(particleSystemRenderer.material);
            matMercExposedBackdropRed.SetColor("_TintColor", new Color(6, 0, 0, 0.5f));
            particleSystemRenderer.material = matMercExposedBackdropRed;

            particleSystemRenderer = MercFocusedAssaultOrbEffectRed.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            matMercExposedSlashRed = Instantiate(particleSystemRenderer.material);
            matMercExposedSlashRed.SetTexture("_RemapTex", texRampHuntressRed);
            matMercExposedSlashRed.SetColor("_TintColor", new Color(0.8868f, 0.06f, 0.06f, 1)); //r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposedSlashRed;

            R2API.ContentAddition.AddEffect(MercFocusedAssaultOrbEffectRed);

            matMercEnergizedRed.SetTexture("_RemapTex", texRampHuntressSoftRed);
            matMercEnergizedRed.SetColor("_TintColor", new Color(1.8f, 0.35f, 0.35f, 1));



            //HuntressBlinkEffectRed
            particleSystem = HuntressBlinkEffectRed.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>();
            particleSystem.startColor = new Color(0.6324f, 0.154f, 0.154f, 1f);//0.1534 0.1567 0.6324 1
            //particleSystem.colorOverLifetime.SetPropertyValue<bool>("enabled", false);
            var stupid = particleSystem.colorOverLifetime;
            stupid.enabled = false;
            //particleSystem.SetPropertyValue("colorOverLifetime", stupid);

            Light light = HuntressBlinkEffectRed.transform.GetChild(0).GetChild(2).GetComponent<Light>();
            light.color = new Color(1f, 0.6f, 0.6f, 1); //0.2721 0.9699 1 1

            particleSystemRenderer = HuntressBlinkEffectRed.transform.GetChild(0).GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressSwipeRed = Instantiate(particleSystemRenderer.material);
            matHuntressSwipeRed.SetTexture("_RemapTex", texRampHuntressSoftRed);
            particleSystemRenderer.material = matHuntressSwipeRed;

            particleSystemRenderer = HuntressBlinkEffectRed.transform.GetChild(0).GetChild(5).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipeRed;
            R2API.ContentAddition.AddEffect(HuntressBlinkEffectRed);
            //

            //HuntressFireArrowRainRed
            particleSystemRenderer = HuntressFireArrowRainRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipeRed;

            particleSystemRenderer = HuntressFireArrowRainRed.transform.GetChild(3).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matHuntressSwipeRed;

            particleSystemRenderer = HuntressFireArrowRainRed.transform.GetChild(4).GetComponent<ParticleSystemRenderer>();
            matHuntressChargedRed = Instantiate(particleSystemRenderer.material);
            matHuntressChargedRed.SetTexture("_RemapTex", texRampHuntressRed);
            particleSystemRenderer.material = matHuntressChargedRed;

            light = HuntressFireArrowRainRed.transform.GetChild(5).GetComponent<Light>();
            light.color = new Color(1f, 0.55f, 0.55f, 1f); //0.3456 0.7563 1 1
            R2API.ContentAddition.AddEffect(HuntressFireArrowRainRed);
            //

            //OmniImpactVFXSlashMercEvisRed
            OmniEffect omniEffect = OmniImpactVFXSlashMercEvisRed.GetComponent<OmniEffect>();

            omniEffect.omniEffectGroups[1].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark4MercRed;




            matOmniRadialSlash1MercRed = Instantiate(omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial);
            matOmniRadialSlash1MercRed.SetTexture("_RemapTex", texRampMercDustRed);

            omniEffect.omniEffectGroups[3].omniEffectElements[1].particleSystemOverrideMaterial = matOmniRadialSlash1MercRed;


            omniEffect.omniEffectGroups[4].omniEffectElements[1].particleSystemOverrideMaterial = matOmniHitspark3MercRed;
            omniEffect.omniEffectGroups[6].omniEffectElements[0].particleSystemOverrideMaterial = matOmniHitspark2MercRed;

            particleSystemRenderer = OmniImpactVFXSlashMercEvisRed.transform.GetChild(7).GetComponent<ParticleSystemRenderer>();
            matMercHologramRed = Instantiate(particleSystemRenderer.material);
            matMercHologramRed.SetTexture("_RemapTex", texRampFallbootsRed);
            matMercHologramRed.SetColor("_TintColor", new Color(1.825f, -0.25f, 0f, 1f));//0.2842 0.4328 1.826 1
            particleSystemRenderer.material = matMercHologramRed;


            R2API.ContentAddition.AddEffect(OmniImpactVFXSlashMercEvisRed);




            EvisProjectileRed.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = EvisProjectileGhostRed;
            EvisProjectileRed.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect = MercSwordFinisherSlashRed;
            EvisProjectileRed.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().childrenProjectilePrefab = EvisOverlapProjectileRed;
            R2API.ContentAddition.AddProjectile(EvisProjectileRed);


            EvisOverlapProjectileRed.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = EvisOverlapProjectileGhostRed;
            EvisOverlapProjectileRed.GetComponent<RoR2.Projectile.ProjectileOverlapAttack>().impactEffect = ImpactMercEvisRed;


            EvisProjectileGhostRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2Red;
            EvisProjectileGhostRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1Red;
            EvisProjectileGhostRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercIgnitionRed;
            EvisProjectileGhostRed.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0f, 0f, 1f); //0 0.5827 1 1
            EvisProjectileGhostRed.transform.GetChild(5).GetComponent<Light>().color = new Color(1f, 0.3f, 0.3f, 1f); //0.1274 0.4704 1 1


            EvisOverlapProjectileGhostRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercSwipe2Red;
            EvisOverlapProjectileGhostRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matOmniRadialSlash1MercRed;
            EvisOverlapProjectileGhostRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matOmniHitspark2MercRed;
            EvisOverlapProjectileGhostRed.transform.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.51f, 0.51f, 1f);//0.3066 0.7276 1 1
            EvisOverlapProjectileGhostRed.transform.GetChild(4).GetComponent<Light>().color = new Color(1f, 0.3f, 0.3f, 1f);//0.1274 0.4704 1 1
            EvisOverlapProjectileGhostRed.transform.GetChild(5).GetComponent<ParticleSystemRenderer>().material = matMercHologramRed;
            EvisOverlapProjectileGhostRed.transform.GetChild(6).GetComponent<ParticleSystemRenderer>().material = matMercHologramRed;

            ImpactMercEvisRed.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercHologramRed;
            ImpactMercEvisRed.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1Red;
            ImpactMercEvisRed.transform.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercSwipe1Red;
            ImpactMercEvisRed.transform.GetChild(3).GetComponent<Light>().color = new Color(1f, 0.425f, 0.425f, 1f);//0 0.8542 1 1

            R2API.ContentAddition.AddEffect(ImpactMercEvisRed);





            particleSystemRenderer = MercExposeEffectRed.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            matMercExposed = Instantiate(particleSystemRenderer.material);
            matMercExposedRed = Instantiate(particleSystemRenderer.material);
            matMercExposedRed.SetTexture("_RemapTex", texRampHuntressRed);
            matMercExposedRed.SetColor("_TintColor", new Color(0.9f, 0.06f, 0.06f, 1f));//r: 0, g: 0.1362783, b: 0.8867924, a: 1
            particleSystemRenderer.material = matMercExposedRed;
            particleSystemRenderer = MercExposeEffectRed.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedBackdropRed;


            particleSystemRenderer = MercExposeConsumeEffectRed.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedRed;
            particleSystemRenderer = MercExposeConsumeEffectRed.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>();
            particleSystemRenderer.material = matMercExposedSlashRed;



            if (WolfoMain.MercOniRedSword.Value == true)
            {


                SkinDef SkinDefMercOni = Addressables.LoadAssetAsync<SkinDef>(key: "RoR2/Base/Merc/skinMercAlt.asset").WaitForCompletion();


                //Replace this
                if (WolfoMain.SkinMercBlueSword.Value == true)
                {
                    Texture2D texMercOniBlues = new Texture2D(128, 128, TextureFormat.DXT5, false);
                    texMercOniBlues.LoadImage(Properties.Resources.texMercOniBlues, false);
                    texMercOniBlues.filterMode = FilterMode.Bilinear;
                    Sprite texMercOniBluesS = Sprite.Create(texMercOniBlues, rec128, half);

                    LoadoutAPI.SkinDefInfo SkinDefMercOniOriginalSkinInfo = new LoadoutAPI.SkinDefInfo
                    {
                        BaseSkins = SkinDefMercOni.baseSkins,
                        Icon = texMercOniBluesS,
                        NameToken = "Oni Traditional",
                        //UnlockableDef = SkinDefMercOni.unlockableDef,
                        RootObject = SkinDefMercOni.rootObject,
                        RendererInfos = SkinDefMercOni.rendererInfos,
                        MeshReplacements = SkinDefMercOni.meshReplacements,
                        GameObjectActivations = SkinDefMercOni.gameObjectActivations,
                        ProjectileGhostReplacements = SkinDefMercOni.projectileGhostReplacements,
                        MinionSkinReplacements = SkinDefMercOni.minionSkinReplacements,
                        Name = "skinMercAltNoEdit",
                    };
                    LoadoutAPI.AddSkinToCharacter(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody"), SkinDefMercOniOriginalSkinInfo);

                }



                On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
                {
                    orig(self, damageInfo);

                    if (damageInfo.damageType == DamageType.ApplyMercExpose)
                    {
                        uint skinindex = damageInfo.attacker.GetComponent<CharacterBody>().skinIndex;
                        if (skinindex != MercSkinExpose)
                        {
                            if (skinindex == 1)
                            {
                                MercExposeEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposedRed;
                                MercExposeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.225f, 0.125f, 0.125f, 0.275f);//0.1335 0.1455 0.2264 0.3412
                                MercExposeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdropRed;

                                MercExposeConsumeEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposedRed;
                                MercExposeConsumeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdropRed;
                                MercExposeConsumeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.35f, 0.2f, 0.2f, 0.175f);

                                MercExposeConsumeEffect.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercExposedRed;
                            }
                            else
                            {
                                MercExposeEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed;
                                MercExposeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.1335f, 0.1455f, 0.2264f, 0.325f);
                                MercExposeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop;

                                MercExposeConsumeEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matMercExposed;
                                MercExposeConsumeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matMercExposedBackdrop;
                                MercExposeConsumeEffect.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().startColor = new Color(0.1076f, 0.2301f, 0.3868f, 0.25f);
                                MercExposeConsumeEffect.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercExposed;
                            }
                        }
                    }
                };




                On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += (orig, self) =>
                {
                    if (self.outer.commonComponents.characterBody.skinIndex == 1)
                    {
                        self.hitEffectPrefab = OmniImpactVFXSlashMercRed;
                        self.swingEffectPrefab = MercSwordSlashRed;
                        if (self.isComboFinisher == true)
                        {
                            self.ignoreAttackSpeed = true;
                        }
                        EntityStates.Merc.Weapon.GroundLight2.comboFinisherSwingEffectPrefab = MercSwordFinisherSlashRed;
                    }
                    else
                    {
                        EntityStates.Merc.Weapon.GroundLight2.comboFinisherSwingEffectPrefab = MercSwordFinisherSlash;
                    }
                    if (self.isComboFinisher == true)
                    {
                        self.ignoreAttackSpeed = true;

                    }
                    orig(self);
                };

                On.EntityStates.Merc.WhirlwindBase.OnEnter += (orig, self) =>
                {

                    if (self.outer.commonComponents.characterBody.skinIndex != MercSkinSecondary)
                    {
                        if (self.outer.commonComponents.characterBody.skinIndex == 1) //Replace this with a check for skinDef
                        {
                            EntityStates.Merc.WhirlwindBase.swingEffectPrefab = MercSwordSlashWhirlwindRed;
                            EntityStates.Merc.WhirlwindBase.hitEffectPrefab = OmniImpactVFXSlashMercRed;
                        }
                        else
                        {
                            EntityStates.Merc.WhirlwindBase.swingEffectPrefab = MercSwordSlashWhirlwind;
                            EntityStates.Merc.WhirlwindBase.hitEffectPrefab = OmniImpactVFXSlashMerc;
                        }
                    }
                    orig(self);
                };

                On.EntityStates.Merc.Uppercut.OnEnter += (orig, self) =>
                {
                    if (self.outer.commonComponents.characterBody.skinIndex != MercSkinSecondaryAlt)
                    {
                        if (self.outer.commonComponents.characterBody.skinIndex == 1)
                        {
                            EntityStates.Merc.Uppercut.swingEffectPrefab = MercSwordUppercutSlashRed;
                            EntityStates.Merc.Uppercut.hitEffectPrefab = OmniImpactVFXSlashMercRed;
                        }
                        else
                        {
                            EntityStates.Merc.Uppercut.swingEffectPrefab = MercSwordUppercutSlash;
                            EntityStates.Merc.Uppercut.hitEffectPrefab = OmniImpactVFXSlashMerc;
                        }
                    }
                    orig(self);
                };

                On.EntityStates.Merc.Assaulter2.OnEnter += (orig, self) =>
                {

                    if (self.outer.commonComponents.characterBody.skinIndex == 1)
                    {
                        self.hitEffectPrefab = OmniImpactVFXSlashMercRed;
                        self.swingEffectPrefab = MercAssaulterEffectRed;
                        //EntityStates.Merc.Assaulter2.selfOnHitOverlayEffectPrefab = MercDashHitOverlayRed;

                        matMercEnergized.SetTexture("_RemapTex", texRampHuntressSoftRed);
                        matMercEnergized.SetColor("_TintColor", new Color(1.8f, 0.35f, 0.35f, 1));
                        orig(self);
                        matMercEnergized.SetTexture("_RemapTex", texRampHuntressSoft);
                        matMercEnergized.SetColor("_TintColor", new Color(0.2842f, 0.4328f, 1.826f, 1));
                        return;
                    }
                    orig(self);
                };



                
                On.EntityStates.Merc.FocusedAssaultDash.OnEnter += (orig, self) =>
                {
                    if (self.outer.commonComponents.characterBody.skinIndex == 1)
                    {
                        self.delayedEffectPrefab = ImpactMercFocusedAssaultRed;
                        self.hitEffectPrefab = ImpactMercAssaulterRed;
                        //self.selfOnHitOverlayEffectPrefab = MercDashHitOverlayRed;
                        self.swingEffectPrefab = MercAssaulterEffectRed;
                        self.enterOverlayMaterial = matMercEnergizedRed;
                        self.orbEffect = MercFocusedAssaultOrbEffectRed;
                    }
                    orig(self);
                };


                //Maybe hook into huntress as well?
                //Done to prevent wrong colorations
                On.EntityStates.Huntress.BlinkState.CreateBlinkEffect += (orig, self, origin) =>
                {
                    matHuntressFlashBright.SetColor("_TintColor", new Color(0.0191f, 1.1386f, 1.2973f, 1));//0.0191 1.1386 1.2973 1
                    matHuntressFlashExpanded.SetColor("_TintColor", new Color(0f, 0.4367f, 0.5809f, 1));//0 0.4367 0.5809 1
                    orig(self, origin);
                };

                On.EntityStates.Huntress.BaseBeginArrowBarrage.CreateBlinkEffect += (orig, self, origin) =>
                {
                    matHuntressFlashBright.SetColor("_TintColor", new Color(0.0191f, 1.1386f, 1.2973f, 1));//0.0191 1.1386 1.2973 1
                    matHuntressFlashExpanded.SetColor("_TintColor", new Color(0f, 0.4367f, 0.5809f, 1));//0 0.4367 0.5809 1
                    orig(self, origin);
                };


                //Needs to be done like this due to EvisDash.FixedUpdate() using LegacyLoad on the Material can't be replaced without IL
                On.EntityStates.Merc.EvisDash.CreateBlinkEffect += (orig, self, origin) =>
                {
                    if (self.outer.commonComponents.characterBody.skinIndex == 1)
                    {
                        EntityStates.Merc.EvisDash.blinkPrefab = HuntressBlinkEffectRed;
                        matHuntressFlashBright.SetColor("_TintColor", new Color(1.3f, 0.6f, 0.6f, 1f));//0.0191 1.1386 1.2973 1 
                        matHuntressFlashExpanded.SetColor("_TintColor", new Color(0.58f, 0.2f, 0.2f, 1f));//0 0.4367 0.5809 1
                        orig(self, origin);
                        return;
                    }
                    else
                    {
                        matHuntressFlashBright.SetColor("_TintColor", new Color(0.0191f, 1.1386f, 1.2973f, 1));//0.0191 1.1386 1.2973 1
                        matHuntressFlashExpanded.SetColor("_TintColor", new Color(0f, 0.4367f, 0.5809f, 1));//0 0.4367 0.5809 1
                    }
                    EntityStates.Merc.EvisDash.blinkPrefab = HuntressBlinkEffect;
                    orig(self, origin);

                };
                On.EntityStates.Merc.Evis.OnEnter += (orig, self) =>
                {
                    if (self.outer.commonComponents.characterBody.skinIndex != MercSkinSpecial)
                    {
                        if (self.outer.commonComponents.characterBody.skinIndex == 1)
                        {
                            EntityStates.Merc.Evis.blinkPrefab = HuntressFireArrowRainRed;
                            EntityStates.Merc.Evis.hitEffectPrefab = OmniImpactVFXSlashMercEvisRed;
                        }
                        else
                        {
                            EntityStates.Merc.Evis.blinkPrefab = HuntressFireArrowRain;
                            EntityStates.Merc.Evis.hitEffectPrefab = OmniImpactVFXSlashMercEvis;
                        }
                    }
                    orig(self);
                };

                On.EntityStates.Merc.Weapon.ThrowEvisProjectile.OnEnter += (orig, self) =>
                {
                    if (self.outer.commonComponents.characterBody.skinIndex == 1)
                    {
                        self.effectPrefab = MercSwordFinisherSlashRed;
                        self.projectilePrefab = EvisProjectileRed; //Replace Ghost Prefab not actual projectile
                    }
                    orig(self);
                };





                /*

            On.EntityStates.Merc.Assaulter.OnEnter += (orig, self) =>
            {
                orig(self);
                Debug.LogWarning("what the fuck is merc assaulter 1");
                Debug.LogWarning("Merc.Assaulter.OnEnter " + EntityStates.Merc.Assaulter.dashPrefab);
                Debug.LogWarning("Merc.Assaulter.OnEnter " + EntityStates.Merc.Assaulter.hitEffectPrefab);
            };



            On.EntityStates.Merc.GroundLight.OnEnter += (orig, self) =>
            {
                orig(self);

                Debug.LogWarning("what the fuck is GroundLight 1");
                Debug.LogWarning("Merc.GroundLight.OnEnter " + EntityStates.Merc.GroundLight.comboHitEffectPrefab);
                Debug.LogWarning("Merc.GroundLight.OnEnter " + EntityStates.Merc.GroundLight.comboSwingEffectPrefab);
                Debug.LogWarning("Merc.GroundLight.OnEnter " + EntityStates.Merc.GroundLight.finisherHitEffectPrefab);
                Debug.LogWarning("Merc.GroundLight.OnEnter " + EntityStates.Merc.GroundLight.finisherSwingEffectPrefab);
            };
                */


            }


        }


        public static void SkinTouchups()
        {
            MatHANDToolbot = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ToolbotBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1].rendererInfos[1].defaultMaterial;
            MatGreenFlowerRex = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TreebotBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1].rendererInfos[1].defaultMaterial;
            MatEngiTurretGreen = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].defaultMaterial;
            MatEngiAltTrail = Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").transform.GetChild(0).GetChild(0).gameObject.GetComponents<SprintEffectController>()[1].loopRootObject.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material);

            Material MatCrocoAlt = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").transform.GetChild(0).GetChild(2).gameObject.GetComponent<ModelSkinController>().skins[1].rendererInfos[0].defaultMaterial;


            Texture2D texCrocoEmissionAlt = new Texture2D(2048, 2048, TextureFormat.DXT1, false);
            texCrocoEmissionAlt.LoadImage(Properties.Resources.texCrocoEmissionAlt, false);
            texCrocoEmissionAlt.filterMode = FilterMode.Bilinear;
            texCrocoEmissionAlt.wrapMode = TextureWrapMode.Clamp;

            MatCrocoAlt.SetTexture("_Emtex", texCrocoEmissionAlt);


            //SkinDef SkinDefEngiAlt = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1];

            //SkinDefEngiAlt.projectileGhostReplacements[1].projectileGhostReplacementPrefab.GetComponentInChildren<Light>().color = new Color(0.251f, 0.7373f, 0.6451f, 1);
            //SkinDefEngiAlt.projectileGhostReplacements[2].projectileGhostReplacementPrefab.GetComponentInChildren<Light>().color = new Color(0.251f, 0.7373f, 0.6451f, 1);




            Texture2D texRampEngiAlt = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampEngiAlt.LoadImage(Properties.Resources.texRampEngiAlt, false);
            texRampEngiAlt.filterMode = FilterMode.Bilinear;
            texRampEngiAlt.wrapMode = TextureWrapMode.Clamp;
            MatEngiAltTrail.SetTexture("_RemapTex", texRampEngiAlt);

            Texture2D TexEngiTurretAlt = new Texture2D(256, 512, TextureFormat.DXT5, false);
            TexEngiTurretAlt.LoadImage(Properties.Resources.texEngiDiffuseAlt, false);
            TexEngiTurretAlt.filterMode = FilterMode.Bilinear;

            MatEngiTurretGreen = Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].defaultMaterial);
            MatEngiTurretGreen.mainTexture = TexEngiTurretAlt;
            MatEngiTurretGreen.SetColor("_EmColor", new Color32(28, 194, 182, 255));

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EngiGrenadeGhostSkin2").GetComponentInChildren<MeshRenderer>().material = MatEngiTurretGreen;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EngiMineGhost2").GetComponentInChildren<SkinnedMeshRenderer>().material = MatEngiTurretGreen;
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/SpiderMineGhost2").GetComponentInChildren<SkinnedMeshRenderer>().material = MatEngiTurretGreen;

            var EngiDisplayMines = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/characterdisplays/EngiDisplay").transform.GetChild(1).GetChild(0).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            //Debug.LogWarning(EngiDisplayMines.Length);
            for (int i = 0; i < EngiDisplayMines.Length; i++)
            {
                if (EngiDisplayMines[i].material.name.StartsWith("matEngiAlt (Instance)") && EngiDisplayMines[i].name.StartsWith("EngiMineMesh"))
                {
                    EngiDisplayMines[i].material = MatEngiTurretGreen;
                };
            };

            Texture2D TexRedSwordDiffuse = new Texture2D(256, 512, TextureFormat.DXT5, false);
            TexRedSwordDiffuse.LoadImage(Properties.Resources.texOniMercSwordDiffuse, false);
            TexRedSwordDiffuse.filterMode = FilterMode.Bilinear;
            TexRedSwordDiffuse.name = "texOniMercSwordDiffuse";

            MatOniSword = Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody").transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().material);
            MatOniSword.name = "matOniMercSword";
            MatOniSword.mainTexture = TexRedSwordDiffuse;
            MatOniSword.SetColor("_EmColor", new Color32(125, 64, 64, 255));

            On.RoR2.SkinDef.Apply += (orig, self, modelObject) =>
            {
                orig(self, modelObject);
                //Debug.LogWarning(self + " SkinDef Apply " + modelObject);

                if (modelObject.name.StartsWith("mdlMerc"))
                {
                    if (WolfoMain.MercOniRedSword.Value == true)
                    {
                        if (self.name.Equals("skinMercAlt"))
                        {
                            if (modelObject)
                            {
                                CharacterModel tempmodel = modelObject.GetComponent<CharacterModel>();
                                tempmodel.baseLightInfos[0].defaultColor = new Color(1, 0.2f, 0.1f, 1);
                                tempmodel.baseLightInfos[1].defaultColor = new Color(1, 0.15f, 0.15f, 1);
                                tempmodel.baseRendererInfos[1].defaultMaterial = MatOniSword;

                                ChildLocator childLocator = modelObject.GetComponent<ChildLocator>();
                                if (childLocator)
                                {
                                    Transform PreDashEffect = childLocator.FindChild("PreDashEffect");
                                    PreDashEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1); //0.5613 0.6875 1 1 
                                    PreDashEffect.GetChild(1).GetComponent<Light>().color = new Color(1f, 0.2f, 0.2f, 1); //0.2028 0.6199 1 1
                                    PreDashEffect.GetChild(2).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1
                                    PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = matMercIgnitionRed; //matMercIgnition (Instance)
                                    PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1 
                                    PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = matMercIgnitionRed; //matMercIgnition (Instance)

                                }

                            }
                        }
                        else
                        {
                            CharacterModel tempmodel = modelObject.GetComponent<CharacterModel>();
                            tempmodel.baseLightInfos[0].defaultColor = new Color(0, 0.609f, 1, 1);
                            tempmodel.baseLightInfos[1].defaultColor = new Color(0, 0.609f, 1, 1);
                        }
                    }
                }
                else if (modelObject.name.StartsWith("mdlTreebot"))
                {
                    if (!self.name.StartsWith("skinTreebotDefault"))
                    {
                        if (modelObject.transform.childCount > 5)
                        {
                            modelObject.transform.GetChild(5).GetChild(0).GetChild(2).gameObject.GetComponent<ParticleSystemRenderer>().material = modelObject.GetComponent<CharacterModel>().baseRendererInfos[1].defaultMaterial;
                        }
                    }
                }
                else if (modelObject.name.StartsWith("mdlEngiTurret"))
                {
                    if (self.name.StartsWith("skinEngiTurretAlt"))
                    {
                        modelObject.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial = MatEngiTurretGreen;
                    }
                }
                else if (modelObject.name.StartsWith("mdlEngi"))
                {
                    if (self.name.StartsWith("skinEngiAlt"))
                    {
                        CharacterModel tempmodel = modelObject.GetComponent<CharacterModel>();

                        if (tempmodel.baseRendererInfos.Length > 1)
                        {
                            tempmodel.baseRendererInfos[0].defaultMaterial = MatEngiAltTrail;
                            tempmodel.baseRendererInfos[1].defaultMaterial = MatEngiAltTrail;
                        }

                        RoR2.SprintEffectController[] component = modelObject.GetComponents<SprintEffectController>();
                        if (component.Length != 0)
                        {
                            for (int i = 0; i < component.Length; i++)
                            {
                                if (component[i].loopRootObject.name.StartsWith("EngiJet"))
                                {
                                    component[i].loopRootObject.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = MatEngiAltTrail;
                                    component[i].loopRootObject.transform.GetChild(2).GetComponent<Light>().color = new Color(0, 0.77f, 0.77f, 1f); // 0 1 0.5508 1
                                }
                            }
                        }
                    }
                }

            };



            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnEnable += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("CrocoDisplay(Clone)"))
                {
                    if (WolfoMain.BlightAcrid.Value == true)
                    {
                        if (self.transform.GetChild(0).childCount == 4)
                        {
                            GameObject tempspawnobj = self.transform.GetChild(0).GetChild(3).gameObject;
                            GameObject blightpuddle = Instantiate(tempspawnobj, self.transform.GetChild(0));

                            blightpuddle.transform.GetChild(0).GetChild(0).GetComponent<ThreeEyedGames.Decal>().Material = matCrocoGooDecalBlight;
                            blightpuddle.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matCrocoDiseaseSporeBlight;
                            blightpuddle.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matCrocoDiseaseDrippingsBlight;
                            blightpuddle.transform.GetChild(0).GetChild(3).GetComponent<Light>().color = new Color32(255, 200, 150, 255);
                            blightpuddle.transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<ParticleSystemRenderer>().material = matCrocoGooSmallBlightD;
                            blightpuddle.transform.GetChild(1).GetChild(3).GetChild(1).GetComponent<ParticleSystemRenderer>().material = matCrocoGooLargeBlight;
                            blightpuddle.transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<ParticleSystemRenderer>().material = matCrocoDiseaseSporeBlight;
                            blightpuddle.transform.GetChild(1).GetChild(3).GetChild(3).GetComponent<Light>().color = new Color32(100, 75, 50, 255);
                        }
                        self.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(true);
                        self.transform.GetChild(0).GetChild(4).GetChild(1).gameObject.SetActive(true);
                        //self.transform.GetChild(0).GetChild(3).name = "SpawnActive";
                        self.transform.GetChild(0).GetChild(4).name = "SpawnActive";
                    }
                }
            };



            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnLoadoutChangedGlobal += (orig, self, networkUser) =>
            {
                orig(self, networkUser);

                Debug.Log(self + " User: " + networkUser.id.value);

                if (self.name.StartsWith("EngiDisplay(Clone)"))
                {
                    Loadout temploadout = networkUser.networkLoadout.GetFieldValue<Loadout>("loadout");
                    BodyIndex Engi = BodyCatalog.FindBodyIndexCaseInsensitive("EngiBody");
                    if (temploadout != null && networkUser.bodyIndexPreference == Engi)
                    {
                        uint skin = temploadout.bodyLoadoutManager.GetSkinIndex(Engi);

                        if (skin == 1)
                        {
                            self.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = MatEngiTurretGreen;

                        }
                    }
                }
                else if (self.name.StartsWith("CrocoDisplay(Clone)"))
                {
                    if (WolfoMain.BlightAcrid.Value == true)
                    {
                        Loadout temploadout = networkUser.networkLoadout.GetFieldValue<Loadout>("loadout");
                        BodyIndex Croco = BodyCatalog.FindBodyIndexCaseInsensitive("CrocoBody");
                        //Debug.LogWarning(Croco);
                        if (temploadout != null && networkUser.bodyIndexPreference == Croco)
                        {
                            uint skill = temploadout.bodyLoadoutManager.GetSkillVariant(Croco, 0);

                            if (!self.transform.GetChild(0).GetChild(4).name.StartsWith("SpawnActive"))
                            {
                                self.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(false);
                                self.transform.GetChild(0).GetChild(4).GetChild(1).gameObject.SetActive(false);
                            }
                            else
                            {
                                self.transform.GetChild(0).GetChild(4).name = "SpawnInactive";
                            }

                            if (skill == 0)
                            {
                                self.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                                self.transform.GetChild(0).GetChild(4).GetChild(1).gameObject.SetActive(false);
                                self.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                            }
                            else if (skill == 1)
                            {
                                self.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                                self.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(false);
                                self.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                            }
                        }
                    }

                }
            };


            On.EntityStates.Toolbot.ToolbotDualWield.OnEnter += (orig, self) =>
            {
                EntityStates.Toolbot.ToolbotDualWield.coverPrefab.GetComponentInChildren<UnityEngine.SkinnedMeshRenderer>().material = self.outer.commonComponents.modelLocator.modelTransform.GetChild(1).GetComponent<UnityEngine.SkinnedMeshRenderer>().material;
                orig(self);
            };

            //Has like 6 different renderers for some reason
            On.EntityStates.Toolbot.ToolbotDualWield.OnExit += (orig, self) =>
            {

                //Material material = self.outer.commonComponents.modelLocator.modelTransform.GetChild(1).GetComponent<UnityEngine.SkinnedMeshRenderer>().material;

                EntityStates.Toolbot.ToolbotDualWield.coverEjectEffect.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.ParticleSystemRenderer>().material = self.outer.commonComponents.modelLocator.modelTransform.GetChild(1).GetComponent<UnityEngine.SkinnedMeshRenderer>().material;




                orig(self);
            };





            On.RoR2.Orbs.OrbEffect.Start += (orig, self) =>
            {
                //Debug.LogWarning(self);
                if (BlightedOrb == true)
                {
                    if (self.name.StartsWith("CrocoDiseaseOrbEffect(Clone)"))
                    {
                        self.gameObject.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailOrangeBlight;
                        self.gameObject.transform.GetChild(1).GetChild(1).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailLesserBlight;
                        self.endEffect = CrocoDiseaseImpactEffectBlight;

                    }
                }
                orig(self);
                if (self.name.StartsWith("EntangleOrbEffect(Clone)"))
                {
                    if (self.gameObject.transform.childCount > 0)
                    {
                        self.gameObject.transform.GetChild(0).GetComponent<LineRenderer>().materials[0].SetTexture("_RemapTex", REXFlowerTempMat.mainTexture);
                        self.gameObject.transform.GetChild(0).GetComponent<LineRenderer>().materials[1].SetTexture("_RemapTex", REXFlowerTempMat.mainTexture);
                        self.gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = REXFlowerTempMat;
                    }
                }


            };










            On.EntityStates.FireFlower2.OnEnter += (orig, self) =>
            {
                //self.outer
                //Debug.LogWarning("FireFlower2.OnEnter "+EntityStates.FireFlower2.projectilePrefab);
                if (self.outer.commonComponents.characterBody.skinIndex != REXSkinFlowerP)
                {
                    REXSkinFlowerP = self.outer.commonComponents.characterBody.skinIndex;

                    Material temp = self.outer.commonComponents.modelLocator.modelTransform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
                    EntityStates.FireFlower2.projectilePrefab.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
                    EntityStates.FireFlower2.projectilePrefab.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab.transform.GetChild(1).GetComponent<MeshRenderer>().material = temp;
                }
                orig(self);
            };

            On.EntityStates.Treebot.TreebotFlower.SpawnState.OnEnter += (orig, self) =>
            {
                //Debug.LogWarning("SpawnState.OnEnter " + self.outer.gameObject);
                orig(self);
                Material temp = self.outer.commonComponents.projectileController.owner.GetComponent<ModelLocator>().modelTransform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;

                //self.outer.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Light>().color = new Color(0.6991f, 1f, 0.0627f, 1f);
                self.outer.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Light>().color = new Color(0.6991f, 0.3627f, 0.8f, 1f);

                self.outer.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().material = temp;
            };

            On.EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.OnEnter += (orig, self) =>
            {
                //Debug.LogWarning("TreebotFlower2Projectile.OnEnter " + EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.enterEffectPrefab);
                //Debug.LogWarning("TreebotFlower2Projectile.OnEnter " + EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.exitEffectPrefab);
                //Debug.LogWarning(self.outer.gameObject);

                uint utemp = self.outer.commonComponents.projectileController.owner.GetComponent<CharacterBody>().skinIndex;
                if (utemp != REXSkinFlowerEnter)
                {
                    REXSkinFlowerEnter = utemp;
                    Transform tempt = self.outer.commonComponents.projectileController.owner.GetComponent<ModelLocator>().modelTransform;


                    GameObject tempobj = EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.enterEffectPrefab;

                    Material temp = tempt.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
                    Material tempL = tempt.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
                    REXFlowerTempMat = temp;
                    tempobj.transform.GetChild(7).GetComponent<ParticleSystemRenderer>().material = tempL;
                    tempobj.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
                }

                orig(self);
            };

            On.EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.OnExit += (orig, self) =>
            {
                //Debug.LogWarning("TreebotFlower2Projectile.OnExit " + EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.enterEffectPrefab);
                //Debug.LogWarning("TreebotFlower2Projectile.OnExit " + EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.exitEffectPrefab);
                GameObject tempowner = self.outer.commonComponents.projectileController.owner;
                if (tempowner)
                {
                    uint utemp = tempowner.GetComponent<CharacterBody>().skinIndex;
                    if (utemp != REXSkinFlowerExit)
                    {
                        REXSkinFlowerExit = utemp;
                        Transform tempt = self.outer.commonComponents.projectileController.owner.GetComponent<ModelLocator>().modelTransform;

                        GameObject tempobj = EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.enterEffectPrefab;

                        Material temp = tempt.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
                        Material tempL = tempt.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
                        tempobj.transform.GetChild(7).GetComponent<ParticleSystemRenderer>().material = tempL;
                        tempobj.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
                    }
                }

                orig(self);
            };

            On.EntityStates.Treebot.Weapon.FirePlantSonicBoom.OnEnter += (orig, self) =>
            {

                //Debug.LogWarning("FirePlantSonicBoom.OnEnter");
                /*
                Debug.LogWarning(EntityStates.Treebot.Weapon.FirePlantSonicBoom.hitEffectPrefab);
                Debug.LogWarning(self.fireEffectPrefab.transform.GetChild(10).GetComponent<ParticleSystemRenderer>().material);
                */

                if (self.outer.commonComponents.characterBody.skinIndex != REXSkinShock)
                {
                    REXSkinFlowerP = self.outer.commonComponents.characterBody.skinIndex;

                    Material temp = self.outer.commonComponents.modelLocator.modelTransform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
                    Material tempL = self.outer.commonComponents.modelLocator.modelTransform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
                    self.fireEffectPrefab.transform.GetChild(10).GetComponent<ParticleSystemRenderer>().material = temp;
                    self.fireEffectPrefab.transform.GetChild(11).GetComponent<ParticleSystemRenderer>().material = tempL;
                    self.fireEffectPrefab.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material.SetTexture("_RemapTex", temp.mainTexture);

                    EntityStates.Treebot.Weapon.FirePlantSonicBoom.hitEffectPrefab.transform.GetChild(7).GetComponent<ParticleSystemRenderer>().material = tempL;
                    EntityStates.Treebot.Weapon.FirePlantSonicBoom.hitEffectPrefab.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
                }


                orig(self);
            };

            On.EntityStates.Treebot.Weapon.AimMortar2.OnEnter += (orig, self) =>
            {
                if (self.projectilePrefab.name.StartsWith("TreebotMortar2"))
                {
                    if (self.outer.commonComponents.characterBody.skinIndex != REXSkinMortar)
                    {
                        REXSkinFlowerP = self.outer.commonComponents.characterBody.skinIndex;
                        GameObject tempobj = self.projectilePrefab.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect;

                        Material temp = self.outer.commonComponents.modelLocator.modelTransform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
                        Material tempL = self.outer.commonComponents.modelLocator.modelTransform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
                        tempobj.transform.GetChild(7).GetComponent<ParticleSystemRenderer>().material = tempL;
                        tempobj.transform.GetChild(11).GetComponent<Light>().color = new Color(0.8817f, 0f, 0.9922f, 1f);
                        tempobj.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
                        tempobj.transform.GetChild(14).GetComponent<ParticleSystemRenderer>().material = tempL;
                    }

                }
                //Debug.LogWarning("AimMortar2 " + self.projectilePrefab);
                orig(self);
            };



        }






        public static void Awake()
        {
            //General VFX List for stuff
            CrocoFistEffectBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoFistEffect.prefab").WaitForCompletion(), "CrocoFistEffectBlight", false);
            CrocoChainableFistEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoChainableFistEffect.prefab").WaitForCompletion();
            CrocoBiteEffectBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoBiteEffect.prefab").WaitForCompletion(), "CrocoBiteEffectBlight", false);
            CrocoSlashBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoSlash.prefab").WaitForCompletion(), "CrocoSlashBlight", false);
            CrocoComboFinisherSlashBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoComboFinisherSlash.prefab").WaitForCompletion(), "CrocoComboFinisherSlashBlight", false);
            CrocoDiseaseImpactEffectBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoDiseaseImpactEffect.prefab").WaitForCompletion(), "CrocoDiseaseImpactEffectBlight", false);
            //R2API.ContentAddition.AddEffect(CrocoDiseaseImpactEffectBlight);
            MuzzleflashCrocoBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/MuzzleflashCroco.prefab").WaitForCompletion(), "MuzzleflashCrocoBlight", false);
            R2API.ContentAddition.AddEffect(MuzzleflashCrocoBlight);
            MercSwordSlashRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlash.prefab").WaitForCompletion(), "MercSwordSlashRed", false);

            MercSwordFinisherSlash = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordFinisherSlash.prefab").WaitForCompletion();
            MercSwordFinisherSlashRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordFinisherSlash.prefab").WaitForCompletion(), "MercSwordFinisherSlashRed", false);

            MercSwordSlashWhirlwind = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion();
            MercSwordSlashWhirlwindRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion(), "MercSwordSlashWhirlwindRed", false);
            R2API.ContentAddition.AddEffect(MercSwordSlashWhirlwindRed);

            MercSwordUppercutSlash = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion();
            MercSwordUppercutSlashRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion(), "MercSwordUppercutSlashRed", false);
            R2API.ContentAddition.AddEffect(MercSwordUppercutSlashRed);

            OmniImpactVFXSlashMerc = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion();
            OmniImpactVFXSlashMercRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion(), "OmniImpactVFXSlashMercRed", false);
            R2API.ContentAddition.AddEffect(OmniImpactVFXSlashMercRed);

            //OmniImpactVFXSlashMerc = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercAssaulterEffect.prefab").WaitForCompletion();
            MercAssaulterEffectRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercAssaulterEffect.prefab").WaitForCompletion(), "MercAssaulterEffectRed", false);
            //R2API.ContentAddition.AddEffect(MercAssaulterEffectRed);

            MercDashHitOverlayRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercDashHitOverlay.prefab").WaitForCompletion(), "MercDashHitOverlayRed", false);
            //R2API.ContentAddition.AddEffect(MercDashHitOverlayRed);

            ImpactMercAssaulterRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercAssaulter.prefab").WaitForCompletion(), "ImpactMercAssaulterRed", false);
            R2API.ContentAddition.AddEffect(ImpactMercAssaulterRed);

            ImpactMercFocusedAssaultRed = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercFocusedAssault.prefab").WaitForCompletion(), "ImpactMercFocusedAssaultRed", false);
            R2API.ContentAddition.AddEffect(ImpactMercFocusedAssaultRed);




            //MercDashHitOverlay  ??? RoR2.TemporaryOverlay



            BellBallElite.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = BellBallGhostElite;
            R2API.ContentAddition.AddProjectile(BellBallElite);

            if (WolfoMain.SkinTouchUps.Value == true)
            {
                SkinTouchups();
            }
            MercRedEffects();
            CrocoBlightChanger();
            R2API.ContentAddition.AddEffect(CrocoDiseaseImpactEffectBlight);
            R2API.ContentAddition.AddEffect(CrocoDiseaseOrbEffectBlight);
            R2API.ContentAddition.AddEffect(CrocoLeapExplosionBlight);




            On.EntityStates.Loader.FireHook.SetHookReference += (orig, self, hook) =>
            {
                if (self.characterBody.skinIndex != 0)
                {
                    hook.transform.GetChild(0).GetComponent<MeshRenderer>().material = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial;
                }
                orig(self, hook);
            };
            /*
            On.EntityStates.Loader.ThrowPylon.OnEnter += (orig, self) =>
            {
                orig(self);

                if (self.characterBody.skinIndex != LoaderPylonSkinIndex)
                {
                    LoaderPylonSkinIndex = self.characterBody.skinIndex;
                    SkinnedMeshRenderer temprender = EntityStates.Loader.ThrowPylon.projectilePrefab.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
                    temprender.materials[1] = Instantiate(temprender.materials[1]);
                    temprender.materials[1].mainTexture = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial.mainTexture;
                    //Debug.LogWarning("CHANGE LOADER PYLON");
                }
            };
            */

            if (WolfoMain.ColorfulBalls.Value == true)
            {
                /*
                On.EntityStates.Bell.BellWeapon.ChargeTrioBomb.OnEnter += (orig, self) =>
                {
                    orig(self);
                    Debug.LogWarning("BellWeapon.ChargeTrioBomb.OnEnter ");
                    On.RoR2.Projectile.ProjectileManager.FireProjectile_GameObject_Vector3_Quaternion_GameObject_float_float_bool_DamageColorIndex_GameObject_float -= ColorfulBellBalls;
                    //On.RoR2.Projectile.ProjectileManager.FireProjectile_GameObject_Vector3_Quaternion_GameObject_float_float_bool_DamageColorIndex_GameObject_float += ColorfulBellBalls;
                };

                On.EntityStates.Bell.BellWeapon.ChargeTrioBomb.OnExit += (orig, self) =>
                {
                    orig(self);
                    Debug.LogWarning("BellWeapon.ChargeTrioBomb.OnExit ");
                    //On.RoR2.Projectile.ProjectileManager.FireProjectile_GameObject_Vector3_Quaternion_GameObject_float_float_bool_DamageColorIndex_GameObject_float -= ColorfulBellBalls;

                };
                */
                // On.RoR2.Projectile.ProjectileManager.FireProjectile_GameObject_Vector3_Quaternion_GameObject_float_float_bool_DamageColorIndex_GameObject_float += ColorfulBellBalls;

                On.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FindTargetChildTransformFromBombIndex += (orig, self) =>
                {

                    CharacterBody tempbody = self.outer.gameObject.GetComponent<CharacterBody>();

                    //Debug.LogWarning(RoR2.Util.GetBestBodyName(tempbody.gameObject) + " BellWeapon.ChargeTrioBomb.FindTargetChildStringFromBombIndex ");
                    if (tempbody.isElite)
                    {
                        LastBallsElite = true;
                        Material elitemat = EquipmentCatalog.GetEquipmentDef(tempbody.equipmentSlot.equipmentIndex).pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                        EntityStates.Bell.BellWeapon.ChargeTrioBomb.preppedBombPrefab.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().material = elitemat;
                        //EntityStates.Bell.BellWeapon.ChargeTrioBomb.bombProjectilePrefab.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab.GetComponentInChildren<UnityEngine.MeshRenderer>().material = elitemat;
                    }
                    else if (tempbody.isElite == false && LastBallsElite == true)
                    {
                        //Debug.LogWarning("No elite balls");
                        LastBallsElite = false;
                        EntityStates.Bell.BellWeapon.ChargeTrioBomb.preppedBombPrefab.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().material = BellBalls;
                        //EntityStates.Bell.BellWeapon.ChargeTrioBomb.bombProjectilePrefab.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab.GetComponentInChildren<UnityEngine.MeshRenderer>().material = BellBalls;
                    }


                    return orig(self);

                };
            }
            //
        }

        public static void ColorfulBellBallsInit(On.EntityStates.Bell.BellWeapon.ChargeTrioBomb.orig_OnEnter orig, global::EntityStates.Bell.BellWeapon.ChargeTrioBomb self)
        {
            orig(self);

            CharacterBody tempbody = self.outer.gameObject.GetComponent<CharacterBody>();

            //Debug.LogWarning(RoR2.Util.GetBestBodyName(tempbody.gameObject) + " BellWeapon.ChargeTrioBomb.FindTargetChildStringFromBombIndex ");
            if (tempbody.isElite)
            {
                DidEliteBrassBalls = true;
                Debug.Log("ColorfulBellBallsInit");
                On.RoR2.Projectile.ProjectileManager.FireProjectile_GameObject_Vector3_Quaternion_GameObject_float_float_bool_DamageColorIndex_GameObject_float += ColorfulBellBalls;
                On.EntityStates.Bell.BellWeapon.ChargeTrioBomb.OnEnter -= ColorfulBellBallsInit;
            }
        }

        public static void ColorfulBellBalls(On.RoR2.Projectile.ProjectileManager.orig_FireProjectile_GameObject_Vector3_Quaternion_GameObject_float_float_bool_DamageColorIndex_GameObject_float orig, global::RoR2.Projectile.ProjectileManager self, GameObject prefab, Vector3 position, Quaternion rotation, GameObject owner, float damage, float force, bool crit, DamageColorIndex damageColorIndex, GameObject target, float speedOverride)
        {
            //Debug.LogWarning("Prefab : " +prefab + " Owner " +owner);

            if (prefab.name.StartsWith("BellBall"))
            {
                CharacterBody tempbody = owner.GetComponent<CharacterBody>();

                //Debug.LogWarning(RoR2.Util.GetBestBodyName(tempbody.gameObject) + " BellWeapon.ChargeTrioBomb.FindTargetChildStringFromBombIndex ");

                if (tempbody.isElite)
                {
                    //LastBallsElite2 = true;
                    //BellEquipIndex = tempbody.equipmentSlot.equipmentIndex;  tempbody.equipmentSlot.equipmentIndex != BellEquipIndex
                    //Material elitemat = EquipmentCatalog.GetEquipmentDef(tempbody.equipmentSlot.equipmentIndex).pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                    Material elitemat = tempbody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;

                    BellBallGhostElite.GetComponentInChildren<UnityEngine.MeshRenderer>().material = elitemat;
                    prefab = BellBallElite;
                }

            }

            orig(self, prefab, position, rotation, owner, damage, force, crit, damageColorIndex, target, speedOverride);
        }


    }
}