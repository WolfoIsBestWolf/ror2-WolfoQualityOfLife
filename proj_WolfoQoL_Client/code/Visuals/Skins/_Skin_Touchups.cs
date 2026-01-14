using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public static class SkinTouchups
    {


        public static void Start()
        {

            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnLoadoutChangedGlobal += SkinTouchUpsLobby;

            Skins_Merc.Start();
            Skins_Engi.Start();
            if (WConfig.cfgSkinMisc.Value)
            {
                Skins_REX.Start();
                GameModeCatalog.availability.CallWhenAvailable(Skins_Loader.Start);
                Skins_Toolbot();
            }
            Skins_Operator();
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
        public static void Skins_Operator()
        {
            if (!WConfig.cfgSkinOperatorAltChirp.Value)
            {
                return;
            }
            SkinDef DTHaulerMastery = ScriptableObject.Instantiate(Addressables.LoadAssetAsync<SkinDef>(key: "a725c698056caf04790c1f808f62d948").WaitForCompletion());
            SkinDefParams DTHaulerMasteryP = ScriptableObject.Instantiate(Addressables.LoadAssetAsync<SkinDefParams>(key: "e2f9475836a89c5499ecf3e6934fea49").WaitForCompletion());
            Material matDroneNew = Material.Instantiate(Addressables.LoadAssetAsync<Material>(key: "448eba93c01ca6046970593a3cc2d234").WaitForCompletion());
            matDroneNew.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texDTHauler_Mastery.png");
            matDroneNew.SetTexture("_EmTex", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texDTHauler_MasteryEM.png"));
            matDroneNew.SetColor("_EmColor", new Color(1f, 0.3377f, 0.3377f, 1f)); //1f, 0.3377f, 0.3377f, 1f

            DTHaulerMastery.nameToken = "RAPIDEMANDSTHIS";
            DTHaulerMastery.skinDefParams = DTHaulerMasteryP;
            DTHaulerMastery.skinDefParamsAddress = new AssetReferenceT<SkinDefParams>("");
            DTHaulerMasteryP.rendererInfos[0].defaultMaterial = matDroneNew;
            DTHaulerMasteryP.rendererInfos[0].defaultMaterialAddress = null;

            R2API.Skins.AddSkinToCharacter(DTHaulerMastery.rootObject.transform.root.gameObject, DTHaulerMastery);
            SkinDefParams skinDroneTechDefAlt_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "4c92b69e995926b46876ed3c644a2e81").WaitForCompletion();
            skinDroneTechDefAlt_params.minionSkinReplacements[2].minionSkin = DTHaulerMastery;
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
            Log.LogMessage(self + " User: " + changedNetworkUser.id.value);

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
                    Log.LogMessage(toolBotSkill);

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