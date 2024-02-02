using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQualityOfLife
{
    public class AcridBlight
    {
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
        //public static GameObject CrocoChainableFistEffect = null; //CrocoFistEffect //Leap
        public static GameObject CrocoLeapAcidBlight = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoLeapAcid"), "CrocoLeapAcidBlight", true); //CrocoLeapAcid //Leap
        //public static GameObject CrocoLeapAcidPoison = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoLeapAcid"); //CrocoLeapAcid //Leap

        public static bool BlightedOrb = false;


        public static void Start()
        {
            //General VFX List for stuff
            CrocoFistEffectBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoFistEffect.prefab").WaitForCompletion(), "CrocoFistEffectBlight", false);
            CrocoBiteEffectBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoBiteEffect.prefab").WaitForCompletion(), "CrocoBiteEffectBlight", false);
            CrocoSlashBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoSlash.prefab").WaitForCompletion(), "CrocoSlashBlight", false);
            CrocoComboFinisherSlashBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoComboFinisherSlash.prefab").WaitForCompletion(), "CrocoComboFinisherSlashBlight", false);
            CrocoDiseaseImpactEffectBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoDiseaseImpactEffect.prefab").WaitForCompletion(), "CrocoDiseaseImpactEffectBlight", false);
            //R2API.ContentAddition.AddEffect(CrocoDiseaseImpactEffectBlight);
            MuzzleflashCrocoBlight = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/MuzzleflashCroco.prefab").WaitForCompletion(), "MuzzleflashCrocoBlight", false);
            R2API.ContentAddition.AddEffect(MuzzleflashCrocoBlight);

            CrocoBlightChanger();
            CrocoBlightSkin();
            R2API.ContentAddition.AddEffect(AcridBlight.CrocoDiseaseImpactEffectBlight);
            R2API.ContentAddition.AddEffect(AcridBlight.CrocoDiseaseOrbEffectBlight);
            R2API.ContentAddition.AddEffect(AcridBlight.CrocoLeapExplosionBlight);


            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnEnable += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("CrocoDisplay(Clone)"))
                {
                    if (WConfig.cfgSkinAcridBlight.Value == true)
                    {
                        if (self.transform.GetChild(0).childCount == 4)
                        {
                            GameObject tempspawnobj = self.transform.GetChild(0).GetChild(3).gameObject;
                            GameObject blightpuddle = UnityEngine.Object.Instantiate(tempspawnobj, self.transform.GetChild(0));

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



            On.RoR2.Orbs.LightningOrb.Begin += (orig, self) =>
            {
                //if (self.lightningType == RoR2.Orbs.LightningOrb.LightningType.CrocoDisease && self.damageType == DamageType.BlightOnHit)
                if (self.damageType == DamageType.BlightOnHit) { BlightedOrb = true; }
                else { BlightedOrb = false; }
                orig(self);
            };

            //Not a big fan
            On.RoR2.Orbs.OrbEffect.Start += (orig, self) =>
            {
                //Debug.LogWarning(self);
                if (BlightedOrb == true)
                {
                    if (self.name.StartsWith("Croc")) //Name check needed?
                    {
                        self.gameObject.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailOrangeBlight;
                        self.gameObject.transform.GetChild(1).GetChild(1).GetComponent<TrailRenderer>().sharedMaterial = matCrocoDiseaseTrailLesserBlight;
                        self.endEffect = CrocoDiseaseImpactEffectBlight;
                    }
                }
                orig(self);
            };
        }

        public static void CrocoBlightSkin()
        {
            
            //Textures
            Texture2D texCrocoDiffuseBlight = new Texture2D(1024, 1024, TextureFormat.DXT5, false);
            texCrocoDiffuseBlight.LoadImage(Properties.Resources.texCrocoDiffuseBlight, true);
            texCrocoDiffuseBlight.filterMode = FilterMode.Bilinear;

            Texture2D texCrocoEmissionBlight = new Texture2D(2048, 2048, TextureFormat.DXT5, false);
            texCrocoEmissionBlight.LoadImage(Properties.Resources.texCrocoEmissionBlight, true);
            texCrocoEmissionBlight.filterMode = FilterMode.Bilinear;

            Texture2D texCrocoPoisonMaskBlight = new Texture2D(512, 512, TextureFormat.DXT5, false);
            texCrocoPoisonMaskBlight.LoadImage(Properties.Resources.texCrocoPoisonMaskBlight, true);
            texCrocoPoisonMaskBlight.filterMode = FilterMode.Bilinear;

            Texture2D texCrocoBlightSkin = new Texture2D(128, 128, TextureFormat.DXT5, false);
            texCrocoBlightSkin.LoadImage(Properties.Resources.texCrocoBlightSkin, true);
            texCrocoBlightSkin.filterMode = FilterMode.Bilinear;
            Sprite texCrocoBlightSkinS = Sprite.Create(texCrocoBlightSkin, v.rec128, v.half);

            Texture2D texCrocoSkinFlow = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texCrocoSkinFlow.LoadImage(Properties.Resources.texCrocoSkinFlow, true);
            texCrocoSkinFlow.filterMode = FilterMode.Bilinear;
            texCrocoSkinFlow.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampCrocoDiseaseDarkLessDark = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampCrocoDiseaseDarkLessDark.LoadImage(Properties.Resources.texRampCrocoDiseaseDarkLessDark, true);
            texRampCrocoDiseaseDarkLessDark.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseDarkLessDark.name = "texRampCrocoDiseaseDarkLessDark";
            texRampCrocoDiseaseDarkLessDark.wrapMode = TextureWrapMode.Clamp;


            SkinDef SkinDefAcridDefault = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").transform.GetChild(0).GetChild(2).gameObject.GetComponent<ModelSkinController>().skins[0];

            CharacterModel.RendererInfo[] AcridBlightedRenderInfos = new CharacterModel.RendererInfo[4];
            Array.Copy(SkinDefAcridDefault.rendererInfos, AcridBlightedRenderInfos, 4);

            Material matCrocoBlight = UnityEngine.Object.Instantiate(SkinDefAcridDefault.rendererInfos[0].defaultMaterial);


            matCrocoBlight.mainTexture = texCrocoDiffuseBlight;
            matCrocoBlight.SetTexture("_EmTex", texCrocoEmissionBlight);
            matCrocoBlight.SetTexture("_FlowHeightmap", texCrocoPoisonMaskBlight);
            matCrocoBlight.SetTexture("_FlowHeightRamp", texRampCrocoDiseaseDarkLessDark);//texRampCrocoDiseaseBlight
            matCrocoBlight.SetColor("_EmColor", new Color(1.2f, 1.2f, 0.75f, 1));

            AcridBlightedRenderInfos[0].defaultMaterial = matCrocoBlight;  //matCroco
            AcridBlightedRenderInfos[2].defaultMaterial = matCrocoDiseaseDrippingsBlight; //matCrocoDiseaseDrippings

            if (WConfig.cfgSkinAcridBlight.Value == true)
            {
                SkinDefInfo AcridBlightSkinInfo = new SkinDefInfo
                {
                    BaseSkins = SkinDefAcridDefault.baseSkins,

                    NameToken = "Blighted",
                    UnlockableDef = RoR2.LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Skills.Croco.PassivePoisonLethal"),
                    RootObject = SkinDefAcridDefault.rootObject,
                    RendererInfos = AcridBlightedRenderInfos,
                    Name = "skinCrocoDefaultBlighted",
                    Icon = texCrocoBlightSkinS,
                };
                R2API.Skins.AddSkinToCharacter(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody"), AcridBlightSkinInfo);

            }
        }


        public static void CrocoBlightChanger()
        {
            GameObject CrocoChainableFistEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoChainableFistEffect.prefab").WaitForCompletion();


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
            texRampCrocoDiseaseBlight.LoadImage(Properties.Resources.texRampCrocoDiseaseBlight, true);
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
            texRampCrocoDiseaseDarkDark.LoadImage(Properties.Resources.texRampCrocoDiseaseDarkDark, true);
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
            texRampCrocoDiseaseBlightAlt.LoadImage(Properties.Resources.texRampCrocoDiseaseBlightAlt, true);
            texRampCrocoDiseaseBlightAlt.filterMode = FilterMode.Bilinear;
            texRampCrocoDiseaseBlightAlt.name = "texRampCrocoDiseaseBlightAlt";
            texRampCrocoDiseaseBlightAlt.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampBeetleBreathBlight = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampBeetleBreathBlight.LoadImage(Properties.Resources.texRampBeetleBreathBlight, true);
            texRampBeetleBreathBlight.filterMode = FilterMode.Bilinear;
            texRampBeetleBreathBlight.name = "texRampBeetleBreathBlight";
            texRampBeetleBreathBlight.wrapMode = TextureWrapMode.Clamp;



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
            matCrocoDiseaseTrailOrangeBlight = UnityEngine.Object.Instantiate(CrocoDiseaseOrbEffectBlight.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>().materials[0]);
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
            matCrocoGooDecalBlight = UnityEngine.Object.Instantiate(CrocoLeapAcidBlight.transform.GetChild(0).GetChild(0).GetComponent<ThreeEyedGames.Decal>().Material);
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
            R2API.ContentAddition.AddProjectile(CrocoLeapAcidBlight);


            if (WConfig.cfgSkinAcridBlight.Value == true)
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


                //Not Base Leap, Leap is Default Leap so probably better idk
                On.EntityStates.Croco.Leap.DoImpactAuthority += (orig, self) =>
                {
                    if (self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
                    {
                        self.blastEffectPrefab = CrocoLeapExplosionBlight;
                        //EntityStates.Croco.BaseLeap.projectilePrefab = CrocoLeapAcidBlight;
                    }
                    orig(self);
                };
                IL.EntityStates.Croco.BaseLeap.DropAcidPoolAuthority += BaseLeap_DropAcidPoolAuthority;

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













            }
        }

        private static void BaseLeap_DropAcidPoolAuthority(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("EntityStates.Croco.BaseLeap", "projectilePrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Merc.Weapon.GroundLight2, GameObject>>((target, entityState) =>
                {
                    if (entityState.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
                    {
                        return CrocoLeapAcidBlight;
                    }
                    return target;
                });
                //Debug.Log("IL Found: Blight: BaseLeap_DropAcidPoolAuthority");
            }
            else
            {
                Debug.LogWarning("IL Failed: Blight: BaseLeap_DropAcidPoolAuthority");
            }
        }
    }
}