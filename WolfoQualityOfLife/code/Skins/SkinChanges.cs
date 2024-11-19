using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using R2API;

namespace WolfoQualityOfLife
{
    public class SkinChanges
    {
        public static Material MatOniSword;
        public static Material MatHANDToolbot;
        //public static Material MatGreenFlowerRex;
        public static Material MatTreebot_VineSots;

        public static Material MatEngiTurretGreen;
        public static Material MatEngiTurret_Sots;
        public static Material matEngiTrail_Alt;

        //public static uint LoaderPylonSkinIndex = 255;
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

        public static GameObject BellBallElite;
        public static GameObject BellBallGhostElite; //Bell Balls Prep

        public static void Start()
        {
            if (WConfig.cfgSkinMisc.Value == true)
            {
                SkinTouchups();
                //REXSkinnedAttacks();
            }

            if (!WConfig.NotRequireByAll.Value)
            {
                RedMercSkin.Start();
                AcridBlight.Start();

                BellBallElite = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/BellBall"), "BellBallElite", true);
                BellBallGhostElite = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/BellBallGhost"), "BellBallGhostElite", false);


                BellBallElite.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = BellBallGhostElite;
                R2API.ContentAddition.AddProjectile(BellBallElite);
                if (WConfig.cfgSkinBellBalls.Value == true)
                {
                    IL.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FixedUpdate += BellBalls_ChargeTrioBomb_FixedUpdate;
                    On.RoR2.EffectManager.OnSceneUnloaded += EffectManager_OnSceneUnloaded;
                }
               
            }
        }

        private static void EffectManager_OnSceneUnloaded(On.RoR2.EffectManager.orig_OnSceneUnloaded orig, UnityEngine.SceneManagement.Scene scene)
        {
            orig(scene);
            EffectManager._ShouldUsePooledEffectMap.Add(BellBallGhostElite, false);
        }

        private static void BellBalls_ChargeTrioBomb_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("EntityStates.Bell.BellWeapon.ChargeTrioBomb", "preppedBombPrefab")))
            {
                //CurrentIndex 37
                c.Index += 6;
                //Debug.Log(c + " " + c.Next + " " + c.Next.Operand);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Bell.BellWeapon.ChargeTrioBomb, GameObject>>((ballPrep, entityState) =>
                {
                    //Instance of Object not base Object
                    if (entityState.characterBody.isElite)
                    {
                        //ballPrep; //Do Ball Stuff
                        //ballPrep.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().material = EquipmentCatalog.GetEquipmentDef(entityState.characterBody.equipmentSlot.equipmentIndex).pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                        if (!entityState.isAuthority)
                        {
                            BellBallGhostElite.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().material = entityState.characterBody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                        }
                        ballPrep.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().material = entityState.characterBody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                    }
                    return ballPrep;
                });
                c.Index += 4;
                c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("EntityStates.Bell.BellWeapon.ChargeTrioBomb", "bombProjectilePrefab"));
                //Debug.Log(c + " " + c.Next + " " + c.Next.Operand);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Bell.BellWeapon.ChargeTrioBomb, GameObject>>((ballProj, entityState) =>
                {
                    if (entityState.characterBody.isElite)
                    {
                        BellBallGhostElite.transform.GetChild(0).GetComponent<MeshRenderer>().material = entityState.characterBody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                        return BellBallElite;
                    }
                    return ballProj;
                });
                //Debug.Log("IL Found: IL.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FixedUpdate");
            }
            else
            {
                Debug.LogWarning("IL Failed: IL.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FixedUpdate");
            }
        }

        public static void SkinTouchups()
        {
            MatHANDToolbot = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ToolbotBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1].rendererInfos[1].defaultMaterial;
            Material MatGreenFlowerRex = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TreebotBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1].rendererInfos[1].defaultMaterial;
            MatTreebot_VineSots = GameObject.Instantiate(MatGreenFlowerRex);
            MatEngiTurretGreen = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].defaultMaterial;
            MatEngiTurret_Sots = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].defaultMaterial;
            matEngiTrail_Alt = GameObject.Instantiate(Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Engi/matEngiTrail.mat").WaitForCompletion());
            Material matEngiTrail_Sots = GameObject.Instantiate(Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Engi/matEngiTrail.mat").WaitForCompletion());


            //SkinDef SkinDefEngiAlt = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1];

            //SkinDefEngiAlt.projectileGhostReplacements[1].projectileGhostReplacementPrefab.GetComponentInChildren<Light>().color = new Color(0.251f, 0.7373f, 0.6451f, 1);
            //SkinDefEngiAlt.projectileGhostReplacements[2].projectileGhostReplacementPrefab.GetComponentInChildren<Light>().color = new Color(0.251f, 0.7373f, 0.6451f, 1);

            //Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Treebot/matTreebotColossus.mat").WaitForCompletion();


            #region REX
            Texture2D texTreebotVineForColossus = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texTreebotVineForColossus.png");
            texTreebotVineForColossus.wrapMode = TextureWrapMode.Clamp;
            MatTreebot_VineSots.mainTexture = texTreebotVineForColossus;


            SkinDef skinTreebotAltColossus = Addressables.LoadAssetAsync<SkinDef>(key: "RoR2/Base/Treebot/skinTreebotAltColossus.asset").WaitForCompletion();
            var NewRender = new CharacterModel.RendererInfo
            {
                defaultMaterial = MatTreebot_VineSots,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off,
                renderer = skinTreebotAltColossus.rootObject.transform.GetChild(5).GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>()
            };

            skinTreebotAltColossus.rendererInfos = skinTreebotAltColossus.rendererInfos.Add(NewRender);
            skinTreebotAltColossus.runtimeSkin = null;
            #endregion
            #region Engi Stuff

            SkinDef skinEngiAlt = Addressables.LoadAssetAsync<SkinDef>(key: "RoR2/Base/Engi/skinEngiAlt.asset").WaitForCompletion();
            SkinDef skinEngiAltColossus = Addressables.LoadAssetAsync<SkinDef>(key: "RoR2/Base/Engi/skinEngiAltColossus.asset").WaitForCompletion();

            Texture2D texRampEngiAlt = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampEngiAlt.png");
            texRampEngiAlt.wrapMode = TextureWrapMode.Clamp;
            matEngiTrail_Alt.SetTexture("_RemapTex", texRampEngiAlt);

            Texture2D texRampEngiColossus = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampEngiColossus.png");
            texRampEngiColossus.wrapMode = TextureWrapMode.Clamp;
            matEngiTrail_Sots.SetTexture("_RemapTex", texRampEngiColossus);

            GameObject EngiHarpoonProjectile = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoon.prefab").WaitForCompletion();
            GameObject EngiHarpoonGhostSkin_Alt = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoonGhost.prefab").WaitForCompletion(), "EngiHarpoonGhostSkin_Alt", false);
            GameObject EngiHarpoonGhostSkin_Sots = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Engi/EngiHarpoonGhost.prefab").WaitForCompletion(), "EngiHarpoonGhostSkin_Sots", false);

            if (WConfig.cfgSkinEngiHarpoons.Value)
            {
                var proj = new SkinDef.ProjectileGhostReplacement
                {
                    projectileGhostReplacementPrefab = EngiHarpoonGhostSkin_Alt,
                    projectilePrefab = EngiHarpoonProjectile
                };
                skinEngiAlt.projectileGhostReplacements = skinEngiAlt.projectileGhostReplacements.Add(proj);
                proj = new SkinDef.ProjectileGhostReplacement
                {
                    projectileGhostReplacementPrefab = EngiHarpoonGhostSkin_Sots,
                    projectilePrefab = EngiHarpoonProjectile
                };
                skinEngiAltColossus.projectileGhostReplacements = skinEngiAltColossus.projectileGhostReplacements.Add(proj);
            }
    

            //BLUE
            ParticleSystemRenderer particleSystem = EngiHarpoonGhostSkin_Alt.transform.GetChild(0).GetComponent<ParticleSystemRenderer>(); //matEngiHarpoonRing 
            Material newRings = GameObject.Instantiate(particleSystem.material);
            newRings.SetColor("_TintColor", new Color(0.1f,1f,1f,1f)); //0.1533 1 0.0047 1
            //particleSystem.material = newRings;
            EngiHarpoonGhostSkin_Alt.transform.GetChild(1).GetComponent<MeshRenderer>().material = skinEngiAlt.rendererInfos[2].defaultMaterial;
            TrailRenderer trailRender = EngiHarpoonGhostSkin_Alt.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>(); //matEngiHarpoonTrail 
            trailRender.startColor = new Color(0.3f, 0.9f, 0.8f, 0f);
            trailRender.endColor = new Color(0.3f, 0.9f, 0.8f, 0f);
            //trailRender.startColor = trailRender.endColor;
            //trailRender.endColor *= 0.75f;
            Material newTrail = GameObject.Instantiate(trailRender.material);
            newTrail.SetTexture("_RemapTex", texRampEngiAlt);
            trailRender.material = newTrail;
            //GenericFlash
            particleSystem = EngiHarpoonGhostSkin_Alt.transform.GetChild(3).GetComponent<ParticleSystemRenderer>(); //matEngiShieldSHards 
            Material newShards = GameObject.Instantiate(particleSystem.material);
            //newShards.SetTexture("_RemapTex", texRampEngiAlt);
            newShards.SetColor("_TintColor", new Color(0.6f, 1f, 0.2f, 1));
            particleSystem.material = newShards;


            //ORANGE
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


            //
            Texture2D TexEngiTurretAlt = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texEngiTurretDiffuseAlt.png");
            Texture2D texEngiTurretAltColossusDiffuse = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texEngiTurretAltColossusDiffuse.png");
 
            MatEngiTurretGreen = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].defaultMaterial);
            MatEngiTurretGreen.mainTexture = TexEngiTurretAlt;
            MatEngiTurretGreen.SetColor("_EmColor", new Color32(28, 194, 182, 255));

            MatEngiTurret_Sots = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].defaultMaterial);
            MatEngiTurret_Sots.mainTexture = texEngiTurretAltColossusDiffuse;
            MatEngiTurret_Sots.SetColor("_EmColor", new Color(1, 0.5f, 0.1f, 1f));


            LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EngiGrenadeGhostSkin2").GetComponentInChildren<MeshRenderer>().material = MatEngiTurretGreen;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/EngiMineGhost2").GetComponentInChildren<SkinnedMeshRenderer>().material = MatEngiTurretGreen;

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
                    EngiDisplayMines[i].material = MatEngiTurretGreen;
                };
            };
            #endregion

            #region Merc 
            Texture2D TexRedSwordDiffuse = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texOniMercSwordDiffuse.png");


            Texture2D texRampFallbootsRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampFallbootsRed.png");
            texRampFallbootsRed.wrapMode = TextureWrapMode.Clamp;

            Texture2D texRampHuntressRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinStuff/texRampHuntressRed.png");
            texRampHuntressRed.filterMode = FilterMode.Point;
            texRampHuntressRed.wrapMode = TextureWrapMode.Clamp;

            MatOniSword = UnityEngine.Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody").transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().material);
            MatOniSword.name = "matOniMercSword";
            MatOniSword.mainTexture = TexRedSwordDiffuse;
            MatOniSword.SetColor("_EmColor", new Color32(125, 64, 64, 255));
            MatOniSword.SetTexture("_FlowHeightRamp", texRampFallbootsRed);
            MatOniSword.SetTexture("_FresnelRamp", texRampHuntressRed);
            #endregion

            On.RoR2.SkinDef.Apply += SkinDef_Apply;


            GameObject ToolbotDisplay = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Toolbot/ToolbotDisplay.prefab").WaitForCompletion();
            ToolbotDisplay.AddComponent<CharacterSelectSurvivorPreviewDisplayController>();

 
            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnLoadoutChangedGlobal += SkinTouchUpsLobby;

            On.EntityStates.Toolbot.ToolbotDualWield.OnEnter += (orig, self) =>
            {
                EntityStates.Toolbot.ToolbotDualWield.coverPrefab.GetComponentInChildren<SkinnedMeshRenderer>().material = self.outer.commonComponents.modelLocator.modelTransform.GetChild(1).GetComponent<UnityEngine.SkinnedMeshRenderer>().material;
                orig(self);
            };

            //Has like 6 different renderers for some reason
            On.EntityStates.Toolbot.ToolbotDualWield.OnExit += (orig, self) =>
            {
                //Material material = self.outer.commonComponents.modelLocator.modelTransform.GetChild(1).GetComponent<UnityEngine.SkinnedMeshRenderer>().material;
                EntityStates.Toolbot.ToolbotDualWield.coverEjectEffect.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = self.outer.commonComponents.modelLocator.modelTransform.GetChild(1).GetComponent<UnityEngine.SkinnedMeshRenderer>().material;
                orig(self);
            };

            //Loader correct Hand/Pylon color, seems whatever at this point
            On.EntityStates.Loader.FireHook.SetHookReference += (orig, self, hook) =>
             {
                 if (self.characterBody.skinIndex != 0)
                 {
                     hook.transform.GetChild(0).GetComponent<MeshRenderer>().material = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial;
                 }
                 orig(self, hook);
             };

            On.EntityStates.Loader.ThrowPylon.OnEnter += (orig, self) =>
            {
                //orig(self);
                SkinnedMeshRenderer temprender = EntityStates.Loader.ThrowPylon.projectilePrefab.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
                temprender.materials[1].mainTexture = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial.mainTexture;
                //Debug.LogWarning("CHANGE LOADER PYLON");
                orig(self);
            };

        }

        private static void SkinDef_Apply(On.RoR2.SkinDef.orig_Apply orig, SkinDef self, GameObject modelObject)
        {
            orig(self, modelObject);
            //Debug.LogWarning(self + " SkinDef Apply " + modelObject);

            if (!modelObject)
            {
                return;
            }

            if (modelObject.name.StartsWith("mdlMerc"))
            {
                if (WConfig.cfgSkinMercRed.Value == true)
                {
                    bool alt = self.name.EndsWith("Alt");
                    bool Colossus = self.name.EndsWith("Colossus");


                    if (alt || self.name.EndsWith("Red"))
                    {
                        if (modelObject.GetComponent<RoR2.CharacterModel>() && modelObject.GetComponent<RoR2.CharacterModel>().body)
                        {
                            modelObject.GetComponent<RoR2.CharacterModel>().body.gameObject.AddComponent<MakeThisMercRed>();
                        }
                        if (alt)
                        {
                            CharacterModel tempmodel = modelObject.GetComponent<CharacterModel>();
                            tempmodel.baseLightInfos[0].defaultColor = new Color(1, 0.2f, 0.1f, 1);
                            tempmodel.baseLightInfos[1].defaultColor = new Color(1, 0.15f, 0.15f, 1);
                            tempmodel.baseRendererInfos[1].defaultMaterial = MatOniSword;
                        }

                        ChildLocator childLocator = modelObject.GetComponent<ChildLocator>();
                        if (childLocator)
                        {
                            Transform PreDashEffect = childLocator.FindChild("PreDashEffect");
                            PreDashEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1); //0.5613 0.6875 1 1 
                            PreDashEffect.GetChild(1).GetComponent<Light>().color = new Color(1f, 0.2f, 0.2f, 1); //0.2028 0.6199 1 1
                            PreDashEffect.GetChild(2).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1
                            PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1 
                            if (!WConfig.NotRequireByAll.Value)
                            {
                                PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = RedMercSkin.matMercIgnitionRed; //matMercIgnition (Instance)
                                PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = RedMercSkin.matMercIgnitionRed; //matMercIgnition (Instance)
                            }
                        }
                    }
                    else if (Colossus || self.name.EndsWith("Green"))
                    {
                        if (modelObject.GetComponent<RoR2.CharacterModel>() && modelObject.GetComponent<RoR2.CharacterModel>().body)
                        {
                            modelObject.GetComponent<RoR2.CharacterModel>().body.gameObject.AddComponent<MakeThisMercGreen>();
                        }
                        if (Colossus)
                        {
                            CharacterModel tempmodel = modelObject.GetComponent<CharacterModel>();
                            tempmodel.baseLightInfos[0].defaultColor = new Color(0.2f, 1f, 0.1f, 1);
                            tempmodel.baseLightInfos[1].defaultColor = new Color(0.15f, 0.6f, 0.15f, 1);
                        }

                        ChildLocator childLocator = modelObject.GetComponent<ChildLocator>();
                        if (childLocator)
                        {
                            Transform PreDashEffect = childLocator.FindChild("PreDashEffect");
                            PreDashEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1); //0.5613 0.6875 1 1 
                            PreDashEffect.GetChild(1).GetComponent<Light>().color = new Color(0.2f, 1f, 0.2f, 1); //0.2028 0.6199 1 1
                            PreDashEffect.GetChild(2).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1);  //0.5613 0.6875 1 1
                            PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1);  //0.5613 0.6875 1 1 

                            if (!WConfig.NotRequireByAll.Value)
                            {
                                PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = RedMercSkin.matMercIgnition_Green; //matMercIgnition (Instance)
                                PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = RedMercSkin.matMercIgnition_Green; //matMercIgnition (Instance)
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
                if (modelObject.transform.childCount > 5)
                {
                    ParticleSystemRenderer vine = modelObject.transform.GetChild(5).GetChild(0).GetChild(2).gameObject.GetComponent<ParticleSystemRenderer>();
                    bool doAuto = vine.material.Equals("matTreebotTreeFlower");
                    if (!self.name.Contains("Colossus") && !self.name.EndsWith("Default"))
                    {
                        vine.material = modelObject.GetComponent<CharacterModel>().baseRendererInfos[1].defaultMaterial;
                    }
                }
            }
            else if (modelObject.name.StartsWith("mdlEngiTurret"))
            {
                if (self.name.Equals("skinEngiTurretAlt"))
                {
                    modelObject.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial = MatEngiTurretGreen;
                }
                else if (self.name.EndsWith("Colossus"))
                {
                    modelObject.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial = MatEngiTurret_Sots;
                }
            }
            else if (modelObject.name.StartsWith("mdlEngi"))
            {
                if (self.name.Equals("skinEngiAlt"))
                {
                    CharacterModel tempmodel = modelObject.GetComponent<CharacterModel>();

                    if (tempmodel.baseRendererInfos.Length > 1)
                    {
                        tempmodel.baseRendererInfos[0].defaultMaterial = matEngiTrail_Alt;
                        tempmodel.baseRendererInfos[1].defaultMaterial = matEngiTrail_Alt;
                    }

                    RoR2.SprintEffectController[] component = modelObject.GetComponents<SprintEffectController>();
                    if (component.Length != 0)
                    {
                        for (int i = 0; i < component.Length; i++)
                        {
                            if (component[i].loopRootObject.name.StartsWith("EngiJet"))
                            {
                                component[i].loopRootObject.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = matEngiTrail_Alt;
                                component[i].loopRootObject.transform.GetChild(2).GetComponent<Light>().color = new Color(0, 0.77f, 0.77f, 1f); // 0 1 0.5508 1
                            }
                        }
                    }
                }
            }

        }

        private static void SkinTouchUpsLobby(On.RoR2.CharacterSelectSurvivorPreviewDisplayController.orig_OnLoadoutChangedGlobal orig, CharacterSelectSurvivorPreviewDisplayController self, NetworkUser changedNetworkUser)
        {
            orig(self, changedNetworkUser);
            //This works on all Models at once, when any user changes stuff, how do we filter Models
            //Well it should work maybe it's the Multiplayer test mods issue
            if (changedNetworkUser != self.networkUser)
            {
                return;
            }
            Debug.Log(self + " User: " + changedNetworkUser.id.value);

            if (self.name.StartsWith("EngiDisplay"))
            {
                Loadout temploadout = self.networkUser.networkLoadout.loadout;
                BodyIndex Engi = BodyCatalog.FindBodyIndex("EngiBody");
                if (temploadout != null && self.networkUser.bodyIndexPreference == Engi)
                {
                    uint skin = temploadout.bodyLoadoutManager.GetSkinIndex(Engi);
                    if (skin == 1)
                    {
                        self.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = MatEngiTurretGreen;
                    }
                    else if (skin == 2)
                    {
                        self.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = MatEngiTurret_Sots;
                    }
                }
            }
            else if (self.name.StartsWith("CrocoDisplay"))
            {
                if (!WConfig.NotRequireByAll.Value)
                {
                    if (WConfig.cfgSkinAcridBlight.Value == true)
                    {
                        Loadout temploadout = self.networkUser.networkLoadout.loadout;
                        BodyIndex Croco = BodyCatalog.FindBodyIndexCaseInsensitive("CrocoBody");
                        //Debug.LogWarning(Croco);
                        if (temploadout != null && self.networkUser.bodyIndexPreference == Croco)
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
                                //self.transform.GetChild(0).GetChild(4).GetChild(1).gameObject.SetActive(false);
                                self.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                            }
                            else if (skill == 1)
                            {
                                self.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                                //self.transform.GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(false);
                                self.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
            else if (self.name.StartsWith("ToolbotDisplay"))
            {
                /*
                Loadout temploadout = self.networkUser.networkLoadout.loadout;
                BodyIndex Toolbot = BodyCatalog.FindBodyIndexCaseInsensitive("ToolbotBody");
                //Debug.LogWarning(Croco);
                if (temploadout != null && self.networkUser.bodyIndexPreference == Toolbot)
                {
                    int skill = (int)temploadout.bodyLoadoutManager.GetSkillVariant(Toolbot, 0);

                    //var BodyLoadout = temploadout.bodyLoadoutManager.GetReadOnlyBodyLoadout(Toolbot);
                    //SkillFamily family = BodyLoadout.GetSkillFamily(0);
                    //ToolbotWeaponSkillDef toolBotSkill = (ToolbotWeaponSkillDef)family.variants[skill].skillDef;

                    Transform mdlToolbot = self.transform.GetChild(0).transform.GetChild(0);
                    Transform toolbase = mdlToolbot.Find("ToolbotArmature/ROOT/base/stomach/chest/upper_arm.l/lower_arm.l/toolbase");
                    
                    //mdlToolbot.gameObject.AddComponent<DelayedAnimatorMULT>().toolbase = toolbase;
                    //mdlToolbot.gameObject.GetComponent<DelayedAnimatorMULT>().skill = skill;
                    
                    Vector3 small = new Vector3(0.001f, 0.001f, 0.001f);

                    if (!toolbase.GetChild(0).GetComponent<DelayedAnimatorMULT>())
                    {
                        toolbase.GetChild(0).gameObject.AddComponent<DelayedAnimatorMULT>().vector = small;
                        toolbase.GetChild(1).gameObject.AddComponent<DelayedAnimatorMULT>().vector = small;
                        toolbase.GetChild(2).gameObject.AddComponent<DelayedAnimatorMULT>().vector = small;
                        toolbase.GetChild(3).gameObject.AddComponent<DelayedAnimatorMULT>().vector = small;
                    }
                    switch (skill)
                    {
                        case 0:
                            toolbase.GetChild(1).GetComponent<DelayedAnimatorMULT>().vector = new Vector3(1, 1, 1);  
                            toolbase.GetChild(0).GetComponent<DelayedAnimatorMULT>().vector = small;
                            toolbase.GetChild(2).GetComponent<DelayedAnimatorMULT>().vector = small;
                            toolbase.GetChild(3).GetComponent<DelayedAnimatorMULT>().vector = small;
                            break;
                        case 1:
                            toolbase.GetChild(3).GetComponent<DelayedAnimatorMULT>().vector = new Vector3(0.8f, 0.8f, 0.8f); ;
                            toolbase.GetChild(0).GetComponent<DelayedAnimatorMULT>().vector = small;
                            toolbase.GetChild(2).GetComponent<DelayedAnimatorMULT>().vector = small;
                            toolbase.GetChild(1).GetComponent<DelayedAnimatorMULT>().vector = small;
                            break;
                        case 2:
                            toolbase.GetChild(0).GetComponent<DelayedAnimatorMULT>().vector = new Vector3(1, 1, 1); ;
                            toolbase.GetChild(1).GetComponent<DelayedAnimatorMULT>().vector = small;
                            toolbase.GetChild(2).GetComponent<DelayedAnimatorMULT>().vector = small;
                            toolbase.GetChild(3).GetComponent<DelayedAnimatorMULT>().vector = small;
                            break;
                        case 3:
                            toolbase.GetChild(2).GetComponent<DelayedAnimatorMULT>().vector = new Vector3(0.8f, 0.8f, 0.8f); ;
                            toolbase.GetChild(0).GetComponent<DelayedAnimatorMULT>().vector = small;
                            toolbase.GetChild(1).GetComponent<DelayedAnimatorMULT>().vector = small;
                            toolbase.GetChild(3).GetComponent<DelayedAnimatorMULT>().vector = small;
                            break;
                    }
                    
                }
                */
            }

        }



        public static void REXSkinnedAttacks()
        {
            On.RoR2.Orbs.OrbEffect.Start += (orig, self) =>
            {
                //Debug.LogWarning(self);
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
    }

    public class MakeThisMercRed : MonoBehaviour
    {
        public bool real;
    }
    public class MakeThisMercGreen : MonoBehaviour
    {
        public bool theGreen;
    }

}