using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace WolfoQualityOfLife
{
    public class SkinChanges
    {
        public static Material MatOniSword;
        public static Material MatHANDToolbot;
        public static Material MatGreenFlowerRex;
        public static Material MatEngiTurretGreen;
        public static Material MatEngiAltTrail;

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

        public static GameObject BellBallElite = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/BellBall"), "BellBallElite", true);
        public static GameObject BellBallGhostElite = R2API.PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/BellBallGhost"), "BellBallGhostElite", false);
        //Bell Balls Prep

        public static void Start()
        {
            RedMercSkin.Start();
            AcridBlight.Start();
            if (WConfig.cfgSkinMisc.Value == true)
            {
                SkinTouchups();
                REXSkinnedAttacks();
            }

            BellBallElite.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab = BellBallGhostElite;
            R2API.ContentAddition.AddProjectile(BellBallElite);

            if (WConfig.cfgSkinBellBalls.Value == true)
            {
                IL.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FixedUpdate += BellBalls_ChargeTrioBomb_FixedUpdate;
            }
            //
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
                        BellBallGhostElite.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().material = entityState.characterBody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
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
            MatHANDToolbot = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ToolbotBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1].rendererInfos[1].defaultMaterial;
            MatGreenFlowerRex = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TreebotBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1].rendererInfos[1].defaultMaterial;
            MatEngiTurretGreen = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].defaultMaterial;
            MatEngiAltTrail = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").transform.GetChild(0).GetChild(0).gameObject.GetComponents<SprintEffectController>()[1].loopRootObject.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material);

            Material MatCrocoAlt = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").transform.GetChild(0).GetChild(2).gameObject.GetComponent<ModelSkinController>().skins[1].rendererInfos[0].defaultMaterial;


            Texture2D texCrocoEmissionAlt = new Texture2D(2048, 2048, TextureFormat.DXT1, false);
            texCrocoEmissionAlt.LoadImage(Properties.Resources.texCrocoEmissionAlt, true);
            texCrocoEmissionAlt.filterMode = FilterMode.Bilinear;
            texCrocoEmissionAlt.wrapMode = TextureWrapMode.Clamp;

            MatCrocoAlt.SetTexture("_Emtex", texCrocoEmissionAlt);


            //SkinDef SkinDefEngiAlt = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[1];

            //SkinDefEngiAlt.projectileGhostReplacements[1].projectileGhostReplacementPrefab.GetComponentInChildren<Light>().color = new Color(0.251f, 0.7373f, 0.6451f, 1);
            //SkinDefEngiAlt.projectileGhostReplacements[2].projectileGhostReplacementPrefab.GetComponentInChildren<Light>().color = new Color(0.251f, 0.7373f, 0.6451f, 1);




            Texture2D texRampEngiAlt = new Texture2D(256, 16, TextureFormat.DXT5, false);
            texRampEngiAlt.LoadImage(Properties.Resources.texRampEngiAlt, true);
            texRampEngiAlt.filterMode = FilterMode.Bilinear;
            texRampEngiAlt.wrapMode = TextureWrapMode.Clamp;
            MatEngiAltTrail.SetTexture("_RemapTex", texRampEngiAlt);

            Texture2D TexEngiTurretAlt = new Texture2D(256, 512, TextureFormat.DXT5, false);
            TexEngiTurretAlt.LoadImage(Properties.Resources.texEngiDiffuseAlt, true);
            TexEngiTurretAlt.filterMode = FilterMode.Bilinear;

            MatEngiTurretGreen = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").transform.GetChild(0).GetChild(0).gameObject.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].defaultMaterial);
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
            TexRedSwordDiffuse.LoadImage(Properties.Resources.texOniMercSwordDiffuse, true);
            TexRedSwordDiffuse.filterMode = FilterMode.Bilinear;
            TexRedSwordDiffuse.name = "texOniMercSwordDiffuse";

            MatOniSword = UnityEngine.Object.Instantiate(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody").transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().material);
            MatOniSword.name = "matOniMercSword";
            MatOniSword.mainTexture = TexRedSwordDiffuse;
            MatOniSword.SetColor("_EmColor", new Color32(125, 64, 64, 255));

            On.RoR2.SkinDef.Apply += (orig, self, modelObject) =>
            {
                orig(self, modelObject);
                //Debug.LogWarning(self + " SkinDef Apply " + modelObject);

                if (modelObject.name.StartsWith("mdlMerc"))
                {
                    if (WConfig.cfgSkinMercRed.Value == true)
                    {
                        if (self.name.Equals("skinMercAlt"))
                        {
                            if (modelObject)
                            {
                                if (modelObject.GetComponent<RoR2.CharacterModel>() && modelObject.GetComponent<RoR2.CharacterModel>().body)
                                {
                                    modelObject.GetComponent<RoR2.CharacterModel>().body.gameObject.AddComponent<MakeThisMercRed>();
                                }


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
                                    PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = RedMercSkin.matMercIgnitionRed; //matMercIgnition (Instance)
                                    PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1 
                                    PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = RedMercSkin.matMercIgnitionRed; //matMercIgnition (Instance)
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







            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnLoadoutChangedGlobal += (orig, self, networkUser) =>
            {
                orig(self, networkUser);
                //This works on all Models at once, when any user changes stuff, how do we filter Models
                //Well it should work maybe it's the Multiplayer test mods issue
                Debug.Log(self + " User: " + networkUser.id.value);

                if (self.name.StartsWith("EngiDisplay(Clone)"))
                {
                    Loadout temploadout = self.networkUser.networkLoadout.loadout;
                    BodyIndex Engi = BodyCatalog.FindBodyIndexCaseInsensitive("EngiBody");
                    if (temploadout != null && self.networkUser.bodyIndexPreference == Engi)
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

            //Loader correct Hand/Pylon color, seems whatever at this point
            /*On.EntityStates.Loader.FireHook.SetHookReference += (orig, self, hook) =>
             {
                 if (self.characterBody.skinIndex != 0)
                 {
                     hook.transform.GetChild(0).GetComponent<MeshRenderer>().material = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial;
                 }
                 orig(self, hook);
             };*/
            /*
            On.EntityStates.Loader.ThrowPylon.OnEnter += (orig, self) =>
            {
                orig(self);

                if (self.characterBody.skinIndex != LoaderPylonSkinIndex)
                {
                    LoaderPylonSkinIndex = self.characterBody.skinIndex;
                    SkinnedMeshRenderer temprender = EntityStates.Loader.ThrowPylon.projectilePrefab.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
                    temprender.materials[1] = UnityEngine.Object.Instantiate(temprender.materials[1]);
                    temprender.materials[1].mainTexture = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial.mainTexture;
                    //Debug.LogWarning("CHANGE LOADER PYLON");
                }
            };
            */
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

}