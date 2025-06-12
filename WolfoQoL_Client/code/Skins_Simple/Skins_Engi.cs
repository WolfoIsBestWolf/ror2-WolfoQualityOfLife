using R2API;
using RoR2;
using RoR2.ContentManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{
    public class Skins_Engi
    {
        public static Material matEngiTurretAlt;
        public static Material matEngiTurretColossus;
        public static Material matEngiTrail_Alt;


        public static void AltTrail()
        {
            matEngiTrail_Alt = GameObject.Instantiate(Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Engi/matEngiTrail.mat").WaitForCompletion());

            matEngiTrail_Alt.SetTexture("_RemapTex", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampEngiAlt.png"));

            SkinDefParams skinEngiAlt_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "eab24449944ca9141b8da8adf7020611").WaitForCompletion();
            skinEngiAlt_params.rendererInfos[0].defaultMaterial = matEngiTrail_Alt;
            skinEngiAlt_params.rendererInfos[0].defaultMaterialAddress = null;
            skinEngiAlt_params.rendererInfos[1].defaultMaterial = matEngiTrail_Alt;
            skinEngiAlt_params.rendererInfos[1].defaultMaterialAddress = null;


            skinEngiAlt_params.lightReplacements = new CharacterModel.LightInfo[]
            {
                new CharacterModel.LightInfo
                {
                    light = skinEngiAlt_params.rendererInfos[0].renderer.transform.parent.GetChild(2).GetComponent<Light>(),
                    defaultColor = new Color(0, 0.77f, 0.77f, 1f),
                },
                new CharacterModel.LightInfo
                {
                    light = skinEngiAlt_params.rendererInfos[1].renderer.transform.parent.GetChild(2).GetComponent<Light>(),
                    defaultColor = new Color(0, 0.77f, 0.77f, 1f),
                },
            };
        }

        public static void Harpoons()
        {
            if (!WConfig.cfgSkinEngiHarpoons.Value)
            {
                return;
            }

            Material matEngiTrail_Sots = GameObject.Instantiate(Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Engi/matEngiTrail.mat").WaitForCompletion());

            SkinDefParams skinEngiAlt = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Engi/skinEngiAlt_params.asset").WaitForCompletion();
            SkinDefParams skinEngiAltColossus = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Engi/skinEngiAltColossus_params.asset").WaitForCompletion();
 
            GameObject EngiHarpoonProjectile = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoon.prefab").WaitForCompletion();
            GameObject EngiHarpoonGhostSkin_Alt = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoonGhost.prefab").WaitForCompletion(), "EngiHarpoonGhostSkin_Alt", false);
            GameObject EngiHarpoonGhostSkin_Sots = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoonGhost.prefab").WaitForCompletion(), "EngiHarpoonGhostSkin_Sots", false);


            HG.ArrayUtils.ArrayAppend(ref skinEngiAlt.projectileGhostReplacements, new SkinDefParams.ProjectileGhostReplacement
            {
                projectileGhostReplacementPrefab = EngiHarpoonGhostSkin_Alt,
                projectilePrefab = EngiHarpoonProjectile
            });
            HG.ArrayUtils.ArrayAppend(ref skinEngiAltColossus.projectileGhostReplacements, new SkinDefParams.ProjectileGhostReplacement
            {
                projectileGhostReplacementPrefab = EngiHarpoonGhostSkin_Sots,
                projectilePrefab = EngiHarpoonProjectile
            });

            #region Blue
            ParticleSystemRenderer particleSystem = EngiHarpoonGhostSkin_Alt.transform.GetChild(0).GetComponent<ParticleSystemRenderer>(); //matEngiHarpoonRing 
            Material newRings = GameObject.Instantiate(particleSystem.material);
            newRings.SetColor("_TintColor", new Color(0.1f, 1f, 1f, 1f)); //0.1533 1 0.0047 1
            //particleSystem.material = newRings;
            EngiHarpoonGhostSkin_Alt.transform.GetChild(1).GetComponent<MeshRenderer>().material = AssetAsyncReferenceManager<Material>.LoadAsset(skinEngiAlt.rendererInfos[2].defaultMaterialAddress, AsyncReferenceHandleUnloadType.PreloadInMenu).WaitForCompletion();
            TrailRenderer trailRender = EngiHarpoonGhostSkin_Alt.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>(); //matEngiHarpoonTrail 
            trailRender.startColor = new Color(0.3f, 0.9f, 0.8f, 0f);
            trailRender.endColor = new Color(0.3f, 0.9f, 0.8f, 0f);
            //trailRender.startColor = trailRender.endColor;
            //trailRender.endColor *= 0.75f;
            Material newTrail = GameObject.Instantiate(trailRender.material);
            newTrail.SetTexture("_RemapTex", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampEngiAlt.png"));
            trailRender.material = newTrail;
            //GenericFlash
            particleSystem = EngiHarpoonGhostSkin_Alt.transform.GetChild(3).GetComponent<ParticleSystemRenderer>(); //matEngiShieldSHards 
            Material newShards = GameObject.Instantiate(particleSystem.material);
            newShards.SetColor("_TintColor", new Color(0.6f, 1f, 0.2f, 1));
            particleSystem.material = newShards;
            #endregion
            #region Colossus
            particleSystem = EngiHarpoonGhostSkin_Sots.transform.GetChild(0).GetComponent<ParticleSystemRenderer>(); //matEngiHarpoonRing 
            //newRings = GameObject.Instantiate(particleSystem.material);
            //newRings.SetColor("_TintColor", new Color(1f, 0.6f, 0.1f, 1f)); //0.1533 1 0.0047 1
            //particleSystem.material = newRings;
            EngiHarpoonGhostSkin_Sots.transform.GetChild(1).GetComponent<MeshRenderer>().material = matEngiTurretColossus;
            trailRender = EngiHarpoonGhostSkin_Sots.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>(); //matEngiHarpoonTrail 
            trailRender.startColor = new Color32(255, 160, 60, 0);
            trailRender.endColor = new Color32(255, 160, 60, 0);
            /*trailRender.startColor = new Color(1, 1, 0, 0);
            trailRender.endColor = new Color32(1, 1, 1, 0);*/
            //trailRender.startColor = new Color(1f, 0.75f, 0.25f, 0f);
            //trailRender.endColor = trailRender.startColor * 0.75f;
            newTrail = GameObject.Instantiate(trailRender.material);
            newTrail.SetTexture("_RemapTex", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampEngiColossus.png"));
            trailRender.material = newTrail;
            //GenericFlash
            particleSystem = EngiHarpoonGhostSkin_Sots.transform.GetChild(3).GetComponent<ParticleSystemRenderer>(); //matEngiShieldSHards 
            newShards = GameObject.Instantiate(particleSystem.material);
            newShards.SetColor("_TintColor", new Color(0.6f, 1f, 0.2f, 1));
            particleSystem.material = newShards;
            #endregion
        }
        public static void FixedTurret()
        {
            Texture2D TexEngiTurretAlt = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texEngiTurretDiffuseAlt.png");
            Texture2D texEngiTurretAltColossusDiffuse = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texEngiTurretAltColossusDiffuse.png");
            Material matEngiAltColossus = Addressables.LoadAssetAsync<Material>(key: "5e174f0491412c341a6615fe95839efe").WaitForCompletion();
            
            matEngiTurretAlt = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "a9b91ae3f60a5d14a97ec97e5991dc57").WaitForCompletion());
            matEngiTurretAlt.mainTexture = TexEngiTurretAlt;
            matEngiTurretAlt.SetColor("_EmColor", new Color32(28, 194, 182, 255));
            matEngiTurretAlt.name = "matEngiTurretAlt";

            matEngiTurretColossus = UnityEngine.Object.Instantiate(matEngiTurretAlt);
            matEngiTurretColossus.mainTexture = texEngiTurretAltColossusDiffuse;
            matEngiTurretColossus.SetColor("_EmColor", new Color(1, 0.5f, -1f, 1f));
            matEngiTurretColossus.SetTexture("_FresnelRamp", matEngiAltColossus.GetTexture("_FresnelRamp"));
            matEngiTurretColossus.SetTexture("_FresnelMask", matEngiTurretAlt.GetTexture("_EmTex"));
            matEngiTurretColossus.EnableKeyword("FRESNEL_EMISSION");
            matEngiTurretColossus.SetFloat("_EmPower", 0.2f);
            matEngiTurretColossus.SetFloat("_FresnelBoost", 30f);
            matEngiTurretColossus.SetFloat("_FresnelPower", 3f);
            matEngiTurretColossus.name = "matEngiTurretColossus";




            LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EngiGrenadeGhostSkin2").GetComponentInChildren<MeshRenderer>().material = matEngiTurretAlt;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EngiMineGhost2").GetComponentInChildren<SkinnedMeshRenderer>().material = matEngiTurretAlt;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiGrenadeGhostSkin3.prefab").WaitForCompletion().GetComponentInChildren<MeshRenderer>().material = matEngiTurretColossus;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiMineGhost3.prefab").WaitForCompletion().GetComponentInChildren<SkinnedMeshRenderer>().material = matEngiTurretColossus;

            //LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/SpiderMineGhost2").GetComponentInChildren<SkinnedMeshRenderer>().material = MatEngiTurretGreen;

            var EngiDisplayMines = LegacyResourcesAPI.Load<GameObject>("Prefabs/characterdisplays/EngiDisplay").transform.Find("mdlEngi/EngiArmature/ROOT/base/stomach/chest/upper_arm.l/lower_arm.l/hand.l/IKBoneStart/IKBoneMid/MineHolder/").transform.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            //Debug.LogWarning(EngiDisplayMines.Length);
            for (int i = 0; i < EngiDisplayMines.Length; i++)
            {
                if (EngiDisplayMines[i].material.name.EndsWith("Colossus") && EngiDisplayMines[i].name.StartsWith("EngiMineMesh"))
                {
                    EngiDisplayMines[i].material = matEngiTurretColossus;
                }
                else if (EngiDisplayMines[i].material.name.EndsWith("Alt") && EngiDisplayMines[i].name.StartsWith("EngiMineMesh"))
                {
                    EngiDisplayMines[i].material = matEngiTurretAlt;
                };
            };


            SkinDefParams Alt = Addressables.LoadAssetAsync<SkinDefParams>(key: "b98626ea4e0476344af7e02f66c2d5e0").WaitForCompletion();
            SkinDefParams Colossus = Addressables.LoadAssetAsync<SkinDefParams>(key: "c8f3b0a0035b6734783a9fde35234ba3").WaitForCompletion();

            SkinDefParams AltW = Addressables.LoadAssetAsync<SkinDefParams>(key: "82a114b330c509e4c8ea6872ea789478").WaitForCompletion();
            SkinDefParams ColossusW = Addressables.LoadAssetAsync<SkinDefParams>(key: "b71e9d62b572c4b4e945699e105566e7").WaitForCompletion();


            Alt.rendererInfos[0].defaultMaterial = matEngiTurretAlt;
            Alt.rendererInfos[0].defaultMaterialAddress = null;
            AltW.rendererInfos[0].defaultMaterial = matEngiTurretAlt;
            AltW.rendererInfos[0].defaultMaterialAddress = null;
            Colossus.rendererInfos[0].defaultMaterial = matEngiTurretColossus;
            Colossus.rendererInfos[0].defaultMaterialAddress = null;
            ColossusW.rendererInfos[0].defaultMaterial = matEngiTurretColossus;
            ColossusW.rendererInfos[0].defaultMaterialAddress = null;

        }



    }

}