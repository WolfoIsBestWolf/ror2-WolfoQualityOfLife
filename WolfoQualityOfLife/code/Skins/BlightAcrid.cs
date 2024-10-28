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

        public static GameObject CrocoDiseaseProjectileBlight = R2API.PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoDiseaseProjectile"), "CrocoDiseaseProjectileBlight", true); //CrocoDiseaseProjectile //Spit + Disease
        public static GameObject CrocoSpitBlight = R2API.PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoSpit"), "CrocoSpitBlight", true); //CrocoSpit //Spit + Disease

        public static GameObject CrocoDiseaseOrbEffectBlight = R2API.PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/CrocoDiseaseOrbEffect"), "CrocoDiseaseOrbEffectBlight", false); //CrocoDiseaseOrbEffect 
        public static GameObject CrocoDiseaseGhostBlight = R2API.PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/CrocoDiseaseGhost"), "CrocoDiseaseProjectileGhostBlight", false); //CrocoDiseaseProjectile //Spit + Disease
        public static GameObject CrocoSpitGhostBlight = R2API.PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/CrocoSpitGhost"), "CrocoSpitGhostBlight", false); //CrocoSpit //Spit + Disease

        public static GameObject CrocoDiseaseImpactEffectBlight = null;
        public static GameObject MuzzleflashCrocoBlight = null;

        public static GameObject CrocoLeapExplosionBlight = R2API.PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/impacteffects/CrocoLeapExplosion"), "CrocoLeapExplosionBlight", false); //CrocoLeapExplosion //Leap
        public static GameObject CrocoFistEffectBlight = null; //CrocoFistEffect //Leap
        //public static GameObject CrocoChainableFistEffect = null; //CrocoFistEffect //Leap
        public static GameObject CrocoLeapAcid_Ghost = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoLeapAcidGhost.prefab").WaitForCompletion(); //CrocoLeapAcid //Leap
        public static GameObject CrocoLeapAcid_GhostBlight = R2API.PrefabAPI.InstantiateClone(CrocoLeapAcid_Ghost, "CrocoLeapAcidBlight", true); //CrocoLeapAcid //Leap
        //public static GameObject CrocoLeapAcidPoison = LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/CrocoLeapAcid"); //CrocoLeapAcid //Leap

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

            /*
            On.RoR2.Orbs.LightningOrb.Begin += (orig, self) =>
            {
                //if (self.lightningType == RoR2.Orbs.LightningOrb.LightningType.CrocoDisease && self.damageType == DamageType.BlightOnHit)
                if (self.damageType == DamageType.BlightOnHit) { BlightedOrb = true; }
                else { BlightedOrb = false; }
                orig(self);
            }; */

            IL.RoR2.Orbs.LightningOrb.Begin += BlightedOrbEffect;
            RoR2.Orbs.OrbStorageUtility._orbDictionary.Add("BlightOrb", CrocoDiseaseOrbEffectBlight);
            On.RoR2.Orbs.OrbStorageUtility.Clear += OrbStorageUtility_Clear;
            //x => x.MatchLdstr("Prefabs/Effects/OrbEffects/CrocoDiseaseOrbEffect")))
        }

        private static void OrbStorageUtility_Clear(On.RoR2.Orbs.OrbStorageUtility.orig_Clear orig)
        {
            orig();
            RoR2.Orbs.OrbStorageUtility._orbDictionary.Add("BlightOrb", CrocoDiseaseOrbEffectBlight);
        }

        private static void BlightedOrbEffect(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdstr("Prefabs/Effects/OrbEffects/CrocoDiseaseOrbEffect")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<string, RoR2.Orbs.LightningOrb, string>>((target, orb) =>
                {
                    if (orb.damageType == DamageType.BlightOnHit)
                    {
                        return "BlightOrb";
                    }
                    return target;
                });
                Debug.Log("IL Found: Blight: IL.RoR2.Orbs.LightningOrb.Begin");
            }
            else
            {
                Debug.LogWarning("IL Failed: Blight: IL.RoR2.Orbs.LightningOrb.Begin");
            }
        }

        public static void CrocoBlightSkin()
        {

            //Textures
            Texture2D texCrocoDiffuseBlight = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texCrocoDiffuseBlight.png");
            Texture2D texCrocoEmissionBlight = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texCrocoEmissionBlight.png");
            Texture2D texCrocoPoisonMaskBlight = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texCrocoPoisonMaskBlight.png");

            Texture2D texCrocoBlightSkin = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texCrocoBlightSkin.png");
            Sprite texCrocoBlightSkinS = Sprite.Create(texCrocoBlightSkin, v.rec128, v.half);
 
            Texture2D texCrocoSkinFlow = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texCrocoSkinFlow.png");
            texCrocoSkinFlow.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampCrocoDiseaseDarkLessDark = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampCrocoDiseaseDarkLessDark.png");
            texRampCrocoDiseaseDarkLessDark.wrapMode = TextureWrapMode.Clamp;


            SkinDef SkinDefAcridDefault = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").transform.GetChild(0).GetChild(2).gameObject.GetComponent<ModelSkinController>().skins[0];

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
                    UnlockableDef = LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Skills.Croco.PassivePoisonLethal"),
                    RootObject = SkinDefAcridDefault.rootObject,
                    RendererInfos = AcridBlightedRenderInfos,
                    MeshReplacements = SkinDefAcridDefault.meshReplacements,
                    GameObjectActivations = SkinDefAcridDefault.gameObjectActivations,
                    Name = "skinCrocoDefaultBlighted",
                    Icon = texCrocoBlightSkinS,
                };
                R2API.Skins.AddSkinToCharacter(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody"), AcridBlightSkinInfo);

            }
        }


        public static void CrocoBlightChanger()
        {
            GameObject CrocoChainableFistEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoChainableFistEffect.prefab").WaitForCompletion();


            //Blight Billboard
            GameObject CrocoDisplay = LegacyResourcesAPI.Load<GameObject>("Prefabs/characterdisplays/CrocoDisplay");
            if (CrocoDisplay)
            {
                CrocoDisplay.AddComponent<CharacterSelectSurvivorPreviewDisplayController>();
            }


            Texture2D texRampCrocoDiseaseBlight = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampCrocoDiseaseBlight.png");
            texRampCrocoDiseaseBlight.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampCrocoDiseaseDarkDark = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampCrocoDiseaseDarkDark.png");
            texRampCrocoDiseaseDarkDark.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampCrocoDiseaseBlightAlt = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampCrocoDiseaseBlightAlt.png");
            texRampCrocoDiseaseBlightAlt.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampBeetleBreathBlight = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampBeetleBreathBlight.png");
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
            CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(0).GetComponent<ThreeEyedGames.Decal>().Material = matCrocoGooDecalBlight;
            CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseSporeBlight;
            CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>().sharedMaterial = matCrocoDiseaseDrippingsBlight;
            //CrocoLeapAcidBlight.transform.GetChild(0).GetChild(3).GetComponent<Light>().color = new Color32(255, 110, 177, 255);
            CrocoLeapAcid_GhostBlight.transform.GetChild(0).GetChild(3).GetComponent<Light>().color = new Color32(255, 177, 110, 200);
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
            //R2API.ContentAddition.AddProjectile(CrocoLeapAcid_GhostBlight);


            if (WConfig.cfgSkinAcridBlight.Value == true)
            {
                On.EntityStates.Croco.Slash.BeginMeleeAttackEffect += AcridBlight_M1;

                On.EntityStates.Croco.FireSpit.OnEnter += AcridBlight_M2_M4;

                On.EntityStates.Croco.Bite.BeginMeleeAttackEffect += AcridBlight_M2Alt;

                On.EntityStates.Croco.BaseLeap.OnEnter += AcridBlight_M3_Start;
                On.EntityStates.Croco.Leap.DoImpactAuthority += AcridBlight_M3_Pool;

                IL.EntityStates.Croco.BaseLeap.DropAcidPoolAuthority += AcridBlight_M3_IL_BaseLeap_DropAcidPoolAuthority;

            }
        }

        private static void AcridBlight_M3_Start(On.EntityStates.Croco.BaseLeap.orig_OnEnter orig, EntityStates.Croco.BaseLeap self)
        {
            if (self.outer.GetComponent<CrocoDamageTypeController>() && self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
            {
                if (self.fistEffectPrefab.name.StartsWith("CrocoFistEffect"))
                {
                    self.fistEffectPrefab = CrocoFistEffectBlight;
                };
            };
            orig(self);
        }

        private static void AcridBlight_M3_Pool(On.EntityStates.Croco.Leap.orig_DoImpactAuthority orig, EntityStates.Croco.Leap self)
        {
            if (self.outer.GetComponent<CrocoDamageTypeController>() && self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
            {
                self.blastEffectPrefab = CrocoLeapExplosionBlight;
                //EntityStates.Croco.BaseLeap.projectilePrefab = CrocoLeapAcidBlight;
            }
            orig(self);
        }

        private static void AcridBlight_M2Alt(On.EntityStates.Croco.Bite.orig_BeginMeleeAttackEffect orig, EntityStates.Croco.Bite self)
        {
            if (self.outer.GetComponent<CrocoDamageTypeController>() && self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
            {
                self.swingEffectPrefab = CrocoBiteEffectBlight;
            };
            orig(self);
        }

        private static void AcridBlight_M2_M4(On.EntityStates.Croco.FireSpit.orig_OnEnter orig, EntityStates.Croco.FireSpit self)
        {
            if (self.outer.GetComponent<CrocoDamageTypeController>() && self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
            {
                //Debug.Log(self.projectilePrefab);
                if (self.projectilePrefab.name.StartsWith("CrocoSpit"))
                {
                    self.projectilePrefab = CrocoSpitBlight;
                }
                else if (self.projectilePrefab.name.StartsWith("CrocoDisease"))
                {
                    self.projectilePrefab = CrocoDiseaseProjectileBlight;
                }
                self.effectPrefab = MuzzleflashCrocoBlight;
            };
            orig(self);
        }

        private static void AcridBlight_M1(On.EntityStates.Croco.Slash.orig_BeginMeleeAttackEffect orig, EntityStates.Croco.Slash self)
        {
            if (self.outer.GetComponent<CrocoDamageTypeController>() && self.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
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
        }

        private static void AcridBlight_M3_IL_BaseLeap_DropAcidPoolAuthority(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("EntityStates.Croco.BaseLeap", "projectilePrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Croco.BaseLeap, GameObject>>((target, entityState) =>
                {
                    if (entityState.outer.GetComponent<CrocoDamageTypeController>().GetDamageType() == DamageType.BlightOnHit)
                    {
                        target.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = CrocoLeapAcid_GhostBlight;
                    }
                    else
                    {
                        target.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = CrocoLeapAcid_Ghost;
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