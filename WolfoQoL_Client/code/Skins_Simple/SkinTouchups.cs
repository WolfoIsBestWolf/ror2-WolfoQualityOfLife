using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client
{
    public class SkinTouchups
    {


        public static void Start()
        {
            On.RoR2.SkinDef.Apply += SkinDef_Apply;
            Skins_Merc.Start();

            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnLoadoutChangedGlobal += SkinTouchUpsLobby;
            Skins_Engi.Start();
            Skins_REX.Start();
            Skins_Loader();
            Skins_Toolbot();


            On.RoR2.EffectManager.OnSceneUnloaded += EffectManager_OnSceneUnloaded;
        }

        private static void EffectManager_OnSceneUnloaded(On.RoR2.EffectManager.orig_OnSceneUnloaded orig, UnityEngine.SceneManagement.Scene scene)
        {
            orig(scene);
        }

        public static void Skins_Toolbot()
        {
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Toolbot/EjectToolbotDualWieldCover.prefab").WaitForCompletion().GetComponent<VFXAttributes>().DoNotPool = true;
            GameObject ToolbotDisplay = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Toolbot/ToolbotDisplay.prefab").WaitForCompletion();
            ToolbotDisplay.AddComponent<CharacterSelectSurvivorPreviewDisplayController>();

        
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

        }

        public static void Skins_Loader()
        {
            //Loader correct Hand/Pylon color, seems whatever at this point
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Loader/LoaderHookGhost.prefab").WaitForCompletion().AddComponent<VFXAttributes>().DoNotPool = true;
            On.EntityStates.Loader.FireHook.SetHookReference += (orig, self, hook) =>
            {
                if (self.characterBody.skinIndex == 1)
                {
                    hook.transform.GetChild(0).GetComponent<MeshRenderer>().material = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial;
                }
                orig(self, hook);
            };

            /*On.EntityStates.Loader.ThrowPylon.OnEnter += (orig, self) =>
            {
                //orig(self);
                SkinnedMeshRenderer temprender = EntityStates.Loader.ThrowPylon.projectilePrefab.transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
                temprender.materials[1].mainTexture = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial.mainTexture;
                //Debug.LogWarning("CHANGE LOADER PYLON");
                orig(self);
            };*/
        }



        private static void SkinDef_Apply(On.RoR2.SkinDef.orig_Apply orig, SkinDef self, GameObject modelObject)
        {
            orig(self, modelObject);
            //Debug.LogWarning(self + " SkinDef Apply " + modelObject);

            if (!modelObject)
            {
                return;
            }
            CharacterModel characterModel = modelObject.GetComponent<CharacterModel>();

            if (modelObject.name.StartsWith("mdlMerc"))
            {
                if (WConfig.cfgSkinMercRedSword.Value == true)
                {
                    bool alt = self.name.EndsWith("Alt");
                    bool Colossus = self.name.EndsWith("Colossus");
                    if (alt || self.name.EndsWith("Red"))
                    {
                        if (alt)
                        {
                            characterModel.baseLightInfos[0].defaultColor = new Color(1, 0.2f, 0.1f, 1);
                            characterModel.baseLightInfos[1].defaultColor = new Color(1, 0.15f, 0.15f, 1);
                            characterModel.baseRendererInfos[1].defaultMaterial = Skins_Merc.MatOniSword;
                        }
                        ChildLocator childLocator = modelObject.GetComponent<ChildLocator>();
                        if (childLocator)
                        {
                            Transform PreDashEffect = childLocator.FindChild("PreDashEffect");
                            PreDashEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1); //0.5613 0.6875 1 1 
                            PreDashEffect.GetChild(1).GetComponent<Light>().color = new Color(1f, 0.2f, 0.2f, 1); //0.2028 0.6199 1 1
                            PreDashEffect.GetChild(2).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1
                            PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1 
                            PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = Merc_Red.matMercIgnition_Red; //matMercIgnition (Instance)
                            PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = Merc_Red.matMercIgnition_Red; //matMercIgnition (Instance)
                        }
                    }
                    else if (Colossus || self.name.EndsWith("Green"))
                    {
                        if (Colossus)
                        {
                            characterModel.baseLightInfos[0].defaultColor = new Color(0.2f, 1f, 0.1f, 1);
                            characterModel.baseLightInfos[1].defaultColor = new Color(0.15f, 0.6f, 0.15f, 1);
                        }
                        ChildLocator childLocator = modelObject.GetComponent<ChildLocator>();
                        if (childLocator)
                        {
                            Transform PreDashEffect = childLocator.FindChild("PreDashEffect");
                            PreDashEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1); //0.5613 0.6875 1 1 
                            PreDashEffect.GetChild(1).GetComponent<Light>().color = new Color(0.2f, 1f, 0.2f, 1); //0.2028 0.6199 1 1
                            PreDashEffect.GetChild(2).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1);  //0.5613 0.6875 1 1
                            PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1);  //0.5613 0.6875 1 1 
                            PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = Merc_Green.matMercIgnition_Green; //matMercIgnition (Instance)
                            PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = Merc_Green.matMercIgnition_Green; //matMercIgnition (Instance)
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
                if (WConfig.cfgSkinMisc.Value == true)
                {
                    if (modelObject.transform.childCount > 5)
                    {
                        ParticleSystemRenderer vine = modelObject.transform.GetChild(5).GetChild(0).GetChild(2).gameObject.GetComponent<ParticleSystemRenderer>();
                        bool doAuto = vine.material.Equals("matTreebotTreeFlower");
                        if (!self.name.Contains("Colossus") && !self.name.EndsWith("Default"))
                        {
                            vine.material = characterModel.baseRendererInfos[1].defaultMaterial;
                        }
                    }
                }
            }
            else if (modelObject.name.StartsWith("mdlEngiTurret"))
            {
                if (WConfig.cfgSkinMisc.Value == true)
                {
                    if (self.name.Equals("skinEngiTurretAlt"))
                    {
                        characterModel.baseRendererInfos[0].defaultMaterial = Skins_Engi.MatEngiTurretGreen;
                    }
                    else if (self.name.EndsWith("Colossus"))
                    {
                        characterModel.baseRendererInfos[0].defaultMaterial = Skins_Engi.MatEngiTurret_Sots;
                    }
                }
            }
            else if (modelObject.name.StartsWith("mdlEngi"))
            {
                if (WConfig.cfgSkinMisc.Value == true)
                {
                    if (self.name.Equals("skinEngiAlt"))
                    {
                        if (characterModel.baseRendererInfos.Length > 1)
                        {
                            characterModel.baseRendererInfos[0].defaultMaterial = Skins_Engi.matEngiTrail_Alt;
                            characterModel.baseRendererInfos[1].defaultMaterial = Skins_Engi.matEngiTrail_Alt;
                        }
                        SprintEffectController[] component = modelObject.GetComponents<SprintEffectController>();
                        if (component.Length != 0)
                        {
                            for (int i = 0; i < component.Length; i++)
                            {
                                if (component[i].loopRootObject.name.StartsWith("EngiJet"))
                                {
                                    component[i].loopRootObject.transform.GetChild(1).GetComponent<ParticleSystemRenderer>().material = Skins_Engi.matEngiTrail_Alt;
                                    component[i].loopRootObject.transform.GetChild(2).GetComponent<Light>().color = new Color(0, 0.77f, 0.77f, 1f); // 0 1 0.5508 1
                                }
                            }
                        }
                    }
                }
            }
            else if (modelObject.name.StartsWith("mdlTitan"))
            {
                HG.ArrayUtils.Swap(characterModel.baseRendererInfos, characterModel.baseRendererInfos.Length - 1, 0);
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
                        self.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = Skins_Engi.MatEngiTurretGreen;
                    }
                    else if (skin == 2)
                    {
                        self.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = Skins_Engi.MatEngiTurret_Sots;
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



    }



}