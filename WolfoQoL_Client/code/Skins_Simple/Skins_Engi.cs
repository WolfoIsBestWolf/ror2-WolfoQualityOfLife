using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{
    public class Skins_Engi
    {
        public static Material matEngiTurretAlt;
        public static Material MatEngiTurret_Sots;
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


            Texture2D texRampEngiColossus = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampEngiColossus.png");
            matEngiTrail_Sots.SetTexture("_RemapTex", texRampEngiColossus);

            GameObject EngiHarpoonProjectile = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoon.prefab").WaitForCompletion();
            GameObject EngiHarpoonGhostSkin_Alt = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoonGhost.prefab").WaitForCompletion(), "EngiHarpoonGhostSkin_Alt", false);
            GameObject EngiHarpoonGhostSkin_Sots = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoonGhost.prefab").WaitForCompletion(), "EngiHarpoonGhostSkin_Sots", false);


            HG.ArrayUtils.ArrayAppend(ref skinEngiAlt.projectileGhostReplacements, new SkinDefParams.ProjectileGhostReplacement
            {
                projectileGhostReplacementPrefab = EngiHarpoonGhostSkin_Alt,
                projectilePrefab = EngiHarpoonProjectile
            });
            HG.ArrayUtils.ArrayAppend(ref skinEngiAlt.projectileGhostReplacements, new SkinDefParams.ProjectileGhostReplacement
            {
                projectileGhostReplacementPrefab = EngiHarpoonGhostSkin_Sots,
                projectilePrefab = EngiHarpoonProjectile
            });

            #region Blue
            ParticleSystemRenderer particleSystem = EngiHarpoonGhostSkin_Alt.transform.GetChild(0).GetComponent<ParticleSystemRenderer>(); //matEngiHarpoonRing 
            Material newRings = GameObject.Instantiate(particleSystem.material);
            newRings.SetColor("_TintColor", new Color(0.1f, 1f, 1f, 1f)); //0.1533 1 0.0047 1
            //particleSystem.material = newRings;
            EngiHarpoonGhostSkin_Alt.transform.GetChild(1).GetComponent<MeshRenderer>().material = skinEngiAlt.rendererInfos[2].defaultMaterial;
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
            //newShards.SetTexture("_RemapTex", texRampEngiAlt);
            newShards.SetColor("_TintColor", new Color(0.6f, 1f, 0.2f, 1));
            particleSystem.material = newShards;
            #endregion
            #region Colossus
            particleSystem = EngiHarpoonGhostSkin_Sots.transform.GetChild(0).GetComponent<ParticleSystemRenderer>(); //matEngiHarpoonRing 
            //newRings = GameObject.Instantiate(particleSystem.material);
            //newRings.SetColor("_TintColor", new Color(1f, 0.6f, 0.1f, 1f)); //0.1533 1 0.0047 1
            //particleSystem.material = newRings;
            EngiHarpoonGhostSkin_Sots.transform.GetChild(1).GetComponent<MeshRenderer>().material = skinEngiAlt.rendererInfos[2].defaultMaterial;
            trailRender = EngiHarpoonGhostSkin_Sots.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>(); //matEngiHarpoonTrail 
            trailRender.startColor = new Color32(255, 190, 60, 0);
            trailRender.endColor = new Color32(255, 190, 60, 0);
            //trailRender.startColor = new Color(1f, 0.75f, 0.25f, 0f);
            //trailRender.endColor = trailRender.startColor * 0.75f;
            newTrail = GameObject.Instantiate(trailRender.material);
            newTrail.SetTexture("_RemapTex", texRampEngiColossus);
            trailRender.material = newTrail;
            //GenericFlash
            particleSystem = EngiHarpoonGhostSkin_Sots.transform.GetChild(3).GetComponent<ParticleSystemRenderer>(); //matEngiShieldSHards 
            newShards = GameObject.Instantiate(particleSystem.material);
            //newShards.SetTexture("_RemapTex", texRampEngiColossus);
            newShards.SetColor("_TintColor", new Color(0.6f, 1f, 0.2f, 1));
            particleSystem.material = newShards;
            #endregion
        }
        public static void FixedTurret()
        {
            Texture2D TexEngiTurretAlt = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texEngiTurretDiffuseAlt.png");
            Texture2D texEngiTurretAltColossusDiffuse = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texEngiTurretAltColossusDiffuse.png");

            matEngiTurretAlt = UnityEngine.Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "a9b91ae3f60a5d14a97ec97e5991dc57").WaitForCompletion());
            matEngiTurretAlt.mainTexture = TexEngiTurretAlt;
            matEngiTurretAlt.SetColor("_EmColor", new Color32(28, 194, 182, 255));

            MatEngiTurret_Sots = UnityEngine.Object.Instantiate(matEngiTurretAlt);
            MatEngiTurret_Sots.mainTexture = texEngiTurretAltColossusDiffuse;
            MatEngiTurret_Sots.SetColor("_EmColor", new Color(1, 0.5f, 0.1f, 1f));


            LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EngiGrenadeGhostSkin2").GetComponentInChildren<MeshRenderer>().material = matEngiTurretAlt;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EngiMineGhost2").GetComponentInChildren<SkinnedMeshRenderer>().material = matEngiTurretAlt;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiGrenadeGhostSkin3.prefab").WaitForCompletion().GetComponentInChildren<MeshRenderer>().material = MatEngiTurret_Sots;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiMineGhost3.prefab").WaitForCompletion().GetComponentInChildren<SkinnedMeshRenderer>().material = MatEngiTurret_Sots;

            //LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/SpiderMineGhost2").GetComponentInChildren<SkinnedMeshRenderer>().material = MatEngiTurretGreen;

            var EngiDisplayMines = LegacyResourcesAPI.Load<GameObject>("Prefabs/characterdisplays/EngiDisplay").transform.GetChild(1).GetChild(0).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            //Debug.LogWarning(EngiDisplayMines.Length);
            for (int i = 0; i < EngiDisplayMines.Length; i++)
            {
                if (EngiDisplayMines[i].material.name.StartsWith("matEngiAltCol") && EngiDisplayMines[i].name.StartsWith("EngiMineMesh"))
                {
                    EngiDisplayMines[i].material = MatEngiTurret_Sots;
                }
                else if (EngiDisplayMines[i].material.name.StartsWith("matEngiAlt") && EngiDisplayMines[i].name.StartsWith("EngiMineMesh"))
                {
                    EngiDisplayMines[i].material = matEngiTurretAlt;
                };
            };


            SkinDefParams Alt = Addressables.LoadAssetAsync<SkinDefParams>(key: "b98626ea4e0476344af7e02f66c2d5e0").WaitForCompletion();
            SkinDefParams Colossus = Addressables.LoadAssetAsync<SkinDefParams>(key: "c8f3b0a0035b6734783a9fde35234ba3").WaitForCompletion();

            Alt.rendererInfos[0].defaultMaterial = matEngiTurretAlt;
            Alt.rendererInfos[0].defaultMaterialAddress = null;
            Colossus.rendererInfos[0].defaultMaterial = MatEngiTurret_Sots;
            Colossus.rendererInfos[0].defaultMaterialAddress = null;

        }



    }

}