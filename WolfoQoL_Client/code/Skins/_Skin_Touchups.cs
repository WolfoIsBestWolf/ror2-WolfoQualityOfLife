using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public class SkinTouchups
    {


        public static void Start()
        {
            On.RoR2.SkinDef.ApplyAsync += SkinDef_ApplyAsync;
            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnLoadoutChangedGlobal += SkinTouchUpsLobby;

            Skins_Merc.Start();
            Skins_Engi.Start();
            Skins_REX.Start();
            Skins_Loader();
            Skins_Toolbot();
            OtherEnemies.Start();
            InteractableSkins.Start();

            if (WConfig.cfgSmoothCaptain.Value)
            {
                Material matCaptainColossusAltArmor = Addressables.LoadAssetAsync<Material>(key: "572c80ba738b3144f9590ef372d0b055").WaitForCompletion();
                matCaptainColossusAltArmor.SetTexture("_NormalTex", null);
                matCaptainColossusAltArmor.SetTexture("_GreenChannelNormalTex", null); //texTrimSheetLemurianRuins
            }

            //Random mat got switched
            SkinDefParams skinVoidSurvivorDefault_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "105cacfa3b8253c4c8cf34cb95818dff").WaitForCompletion();
            skinVoidSurvivorDefault_params.rendererInfos[3].defaultMaterialAddress = skinVoidSurvivorDefault_params.rendererInfos[0].defaultMaterialAddress;

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
            //Not compatible with Colossus so whatevs
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
                //WolfoMain.log.LogWarning("CHANGE LOADER PYLON");
                orig(self);
            };*/
        }


        private static System.Collections.IEnumerator SkinDef_ApplyAsync(On.RoR2.SkinDef.orig_ApplyAsync orig, SkinDef self, GameObject modelObject, System.Collections.Generic.List<AssetReferenceT<Material>> loadedMaterials, System.Collections.Generic.List<AssetReferenceT<Mesh>> loadedMeshes, RoR2.ContentManagement.AsyncReferenceHandleUnloadType unloadType)
        {
            if (modelObject == null)
            {
                return orig(self, modelObject, loadedMaterials, loadedMeshes, unloadType);
            }
            CharacterModel characterModel = modelObject.GetComponent<RoR2.CharacterModel>();
            if (modelObject.name == "mdlMerc")
            {
                bool red = self.name.EndsWith("Alt") && WConfig.cfgSkinMercRed.Value || self.name.EndsWith("Red");
                bool green = self.name.EndsWith("Colossus") && WConfig.cfgSkinMercGreen.Value || self.name.EndsWith("Green");

                if (characterModel.body)
                {
                    Object.Destroy(characterModel.body.gameObject.GetComponent<MakeThisMercRed>());
                    Object.Destroy(characterModel.body.gameObject.GetComponent<MakeThisMercGreen>());
                    if (red)
                    {
                        characterModel.body.gameObject.AddComponent<MakeThisMercRed>();
                    }
                    else if (green)
                    {
                        characterModel.body.gameObject.AddComponent<MakeThisMercGreen>();
                    }
                }

                ChildLocator childLocator = modelObject.GetComponent<ChildLocator>();
                if (childLocator == null)
                {
                    WolfoMain.log.LogWarning("mdlMerc without childLocator");
                    return orig(self, modelObject, loadedMaterials, loadedMeshes, unloadType);
                }
                Transform PreDashEffect = childLocator.FindChild("PreDashEffect");
                if (PreDashEffect == null)
                {
                    WolfoMain.log.LogMessage("mdlMerc without PreDashEffect");
                    return orig(self, modelObject, loadedMaterials, loadedMeshes, unloadType);
                }

                if (red)
                {
                    PreDashEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1); //0.5613 0.6875 1 1 
                    PreDashEffect.GetChild(1).GetComponent<Light>().color = new Color(1f, 0.2f, 0.2f, 1); //0.2028 0.6199 1 1
                    PreDashEffect.GetChild(2).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1
                    PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(1f, 0.5613f, 0.6875f, 1);  //0.5613 0.6875 1 1 
                    PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = Merc_Red.matMercIgnition_Red; //matMercIgnition (Instance)
                    PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = Merc_Red.matMercIgnition_Red; //matMercIgnition (Instance)
                }
                else if (green)
                {
                    PreDashEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1); //0.5613 0.6875 1 1 
                    PreDashEffect.GetChild(1).GetComponent<Light>().color = new Color(0.2f, 1f, 0.2f, 1); //0.2028 0.6199 1 1
                    PreDashEffect.GetChild(2).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1);  //0.5613 0.6875 1 1
                    PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = new Color(0.6875f, 1f, 0.5613f, 1);  //0.5613 0.6875 1 1 
                    PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = Merc_Green.matMercIgnition_Green; //matMercIgnition (Instance)
                    PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = Merc_Green.matMercIgnition_Green; //matMercIgnition (Instance)
                }

            }
            return orig(self, modelObject, loadedMaterials, loadedMeshes, unloadType);
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
            WolfoMain.log.LogMessage(self + " User: " + changedNetworkUser.id.value);

            if (self.name.StartsWith("EngiDisplay"))
            {
                //I forget why we cant just fix this in object
                Loadout temploadout = self.networkUser.networkLoadout.loadout;
                BodyIndex Engi = BodyCatalog.FindBodyIndex("EngiBody");
                if (temploadout != null && self.networkUser.bodyIndexPreference == Engi)
                {
                    uint skin = temploadout.bodyLoadoutManager.GetSkinIndex(Engi);
                    if (skin == 1)
                    {
                        self.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = Skins_Engi.matEngiTurretAlt;
                    }
                    else if (skin == 2)
                    {
                        self.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material = Skins_Engi.matEngiTurretColossus;
                    }
                }
            }
            if (self.name.StartsWith("ToolbotDisplay"))
            {

                Loadout temploadout = self.networkUser.networkLoadout.loadout;
                BodyIndex Toolbot = BodyCatalog.FindBodyIndexCaseInsensitive("ToolbotBody");
                //WolfoMain.log.LogWarning(Croco);
                if (temploadout != null && self.networkUser.bodyIndexPreference == Toolbot)
                {
                    int skill = (int)temploadout.bodyLoadoutManager.GetSkillVariant(Toolbot, 0);


                    var BodyLoadout = temploadout.bodyLoadoutManager.GetReadOnlyBodyLoadout(Toolbot);
                    SkillFamily family = BodyLoadout.GetSkillFamily(0);
                    ToolbotWeaponSkillDef toolBotSkill = (ToolbotWeaponSkillDef)family.variants[skill].skillDef;
                    WolfoMain.log.LogMessage(toolBotSkill);

                    Transform mdlToolbot = self.transform.GetChild(0).transform.GetChild(0);
                    Transform toolbase = mdlToolbot.Find("ToolbotArmature/ROOT/base/stomach/chest/upper_arm.l/lower_arm.l/toolbase");
                    MulTLobbySkill mulTLobbySkin = toolbase.gameObject.GetComponent<MulTLobbySkill>();
                    if (mulTLobbySkin == null)
                    {
                        mulTLobbySkin = toolbase.gameObject.AddComponent<MulTLobbySkill>();

                    }
                    int[] ints = new int[4];
                    ints[skill] = 1;
                    mulTLobbySkin.skill = ints;

                }

            }

        }



    }

    public class MulTLobbySkill : MonoBehaviour
    {
        public int[] skill = new int[4];
        public float x;

        public Transform nail_New;
        public Transform rebar_New;
        public Transform rocket_New;
        public Transform saw_New;

        public Transform nailOriginal;
        public Transform rebarOriginal;
        public Transform rocketOriginal;
        public Transform sawOriginal;

        public void OnEnable()
        {
            if (rocketOriginal)
            {
                return;
            }
            rocketOriginal = transform.GetChild(0);
            nailOriginal = transform.GetChild(1);
            sawOriginal = transform.GetChild(2);
            rebarOriginal = transform.GetChild(3);

            GameObject Nail = new GameObject("nail_New");
            nail_New = Nail.transform;
            nail_New.SetParent(transform, false);
            nailOriginal.transform.SetParent(nail_New);

            GameObject Rebar = new GameObject("rebar_New");
            rebar_New = Rebar.transform;
            rebar_New.transform.SetParent(transform, false);
            rebarOriginal.transform.SetParent(rebar_New);

            GameObject rocket = new GameObject("rocket_New");
            rocket_New = rocket.transform;
            rocket_New.transform.SetParent(transform, false);
            rocketOriginal.transform.SetParent(rocket_New);

            GameObject saw = new GameObject("saw_New");
            saw_New = saw.transform;
            saw_New.transform.SetParent(transform, false);
            sawOriginal.transform.SetParent(saw_New);

            nail_New.localPosition = nailOriginal.localPosition * -1;
            rebar_New.localPosition = rebarOriginal.localPosition * -100;
            rocket_New.localPosition = rocketOriginal.localPosition * -100;
            saw_New.localPosition = sawOriginal.localPosition * -100;

        }

        public void Update()
        {
            if (saw_New)
            {
                x = nailOriginal.localScale.x;
                nail_New.localScale = Vector3.one * x * skill[0];
                rebar_New.localScale = Vector3.one * x * skill[1] * 100;
                rocket_New.localScale = Vector3.one * x * skill[2] * 100;
                saw_New.localScale = Vector3.one * x * skill[3] * 100;

                nail_New.localPosition = nailOriginal.localPosition * -x;
                rebar_New.localPosition = rebarOriginal.localPosition * -x * 100;
                rocket_New.localPosition = rocketOriginal.localPosition * -x * 100;
                saw_New.localPosition = sawOriginal.localPosition * -x * 100;
            }

        }
    }

}