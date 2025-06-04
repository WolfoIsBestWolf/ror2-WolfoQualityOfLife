using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.ContentManagement;

namespace WolfoQoL_Client
{
    public class Acrid_Main
    {
        public static void Start()
        {
            if (!WConfig.cfgSkinAcridBlight.Value)
            {
                //Since they're not ID'd 
                //Or used by other mods
                //We can just skip
                return;
            }
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoBody.prefab").WaitForCompletion().AddComponent<MakeThisAcridBlight>();

            Effects_Blight.Start();
            Acrid_Hooks.Start();
            Effects_Blight.GhostReplacements();
            GameModeCatalog.availability.CallWhenAvailable(EffectReplacements);


            if (WConfig.cfgSkinAcridBlight.Value == true)
            {
                CrocoBlightSkin();
            }

            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnEnable += SpawnBlightPuddleInLobby;
            On.RoR2.CharacterSelectSurvivorPreviewDisplayController.OnLoadoutChangedGlobal += UpdateBlightPuddleLobby;

        }
        public static bool justSpawned = true;
        private static void UpdateBlightPuddleLobby(On.RoR2.CharacterSelectSurvivorPreviewDisplayController.orig_OnLoadoutChangedGlobal orig, CharacterSelectSurvivorPreviewDisplayController self, NetworkUser changedNetworkUser)
        {
            orig(self, changedNetworkUser);
            if (changedNetworkUser != self.networkUser)
            {
                return;
            }
            if (self.name.StartsWith("CrocoD"))
            {
                Loadout temploadout = self.networkUser.networkLoadout.loadout;
                BodyIndex Croco = BodyCatalog.FindBodyIndexCaseInsensitive("CrocoBody");
              
                if (temploadout != null && self.networkUser.bodyIndexPreference == Croco)
                {
                    uint skill = temploadout.bodyLoadoutManager.GetSkillVariant(Croco, 0);
                    bool isBlight = (skill == 1);
                    bool firstSelected = justSpawned;
                    justSpawned = false;

                    Transform poison = self.transform.GetChild(0).Find("CrocoDisplaySpawnEffect(Clone)");
                    Transform blight = self.transform.GetChild(0).Find("SpawnBlight");

                    poison.gameObject.SetActive(!isBlight);
                    blight.gameObject.SetActive(isBlight);
                    //Play_MULT_m1_sawblade_impact_loop
                    //Play_MULT_m1_sawblade_impact_loop
                    poison.GetChild(1).gameObject.SetActive(firstSelected);
                    blight.GetChild(1).gameObject.SetActive(firstSelected);
                }

            }

        }
        private static void SpawnBlightPuddleInLobby(On.RoR2.CharacterSelectSurvivorPreviewDisplayController.orig_OnEnable orig, CharacterSelectSurvivorPreviewDisplayController self)
        {
            orig(self);
            if (self.name.StartsWith("CrocoD"))
            {
                justSpawned = true;
                /*GameObject spawnPoison = self.transform.GetChild(0).GetChild(3).gameObject;
                spawnPoison.name = "SpawnPoison";*/
                GameObject blightpuddle = UnityEngine.Object.Instantiate(Effects_Blight.lobbyPool, self.transform.GetChild(0));
                blightpuddle.name = "SpawnBlight";
            }
        }


       
        public static void EffectReplacements()
        {

 
            GameObject effect = null;
           
            //M2s
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/MuzzleflashCroco.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Effects_Blight.MuzzleflashCroco_Blight, null, false);
            
            //M2M4
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoDiseaseImpactEffect.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Effects_Blight.CrocoDiseaseImpactEffect_Blight, null, false);
        
            //M4 Orb
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoDiseaseOrbEffect.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Effects_Blight.CrocoDiseaseOrbEffect_Blight, null, false);
          
            //M3 Default
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoLeapExplosion.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Effects_Blight.CrocoLeapExplosion_Blight, null, false);
            
        }


        public static void CrocoBlightSkin()
        {
            SkinDefParams paramCrocoDefault = Addressables.LoadAssetAsync<SkinDefParams>(key: "e067d66a43eb8e245a1ecc10e24974bc").WaitForCompletion();
            SkinDef skinCrocoDefault = Addressables.LoadAssetAsync<SkinDef>(key: "186040c993aa5f240809215921b730be").WaitForCompletion();

            int length = paramCrocoDefault.rendererInfos.Length;
            CharacterModel.RendererInfo[] newRenderInfos = new CharacterModel.RendererInfo[length];
            Array.Copy(paramCrocoDefault.rendererInfos, newRenderInfos, length);
 
            Material matCrocoBlight =  UnityEngine.Object.Instantiate(AssetAsyncReferenceManager<Material>.LoadAsset(newRenderInfos[0].defaultMaterialAddress).WaitForCompletion());

            matCrocoBlight.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texCrocoDiffuseBlight.png");
            matCrocoBlight.SetTexture("_EmTex", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texCrocoEmissionBlight.png"));
            matCrocoBlight.SetTexture("_FlowHeightmap", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texCrocoPoisonMaskBlight.png"));
            matCrocoBlight.SetTexture("_FlowHeightRamp", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampCrocoDiseaseDarkLessDark.png"));//texRampCrocoDiseaseBlight
            matCrocoBlight.SetColor("_EmColor", new Color(1.2f, 1.2f, 0.75f, 1));

            newRenderInfos[0].defaultMaterial = matCrocoBlight;  //matCroco
            newRenderInfos[0].defaultMaterialAddress = null;
            newRenderInfos[1].defaultMaterial = Effects_Blight.matCrocoDiseaseDrippingsBlight; //matCrocoDiseaseDrippings
            newRenderInfos[1].defaultMaterialAddress = null; //matCrocoDiseaseDrippings

            SkinDef newSkin = ScriptableObject.CreateInstance<SkinDef>();
  
            newSkin.name = "skinCrocoDefault_Blight";
            newSkin.nameToken = "SKIN_CROCO_BLIGHT";
            newSkin.unlockableDef = LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Skills.Croco.PassivePoisonLethal");
            newSkin.icon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/General/texCrocoBlightSkin.png");
            newSkin.rendererInfos = newRenderInfos;
            newSkin.rootObject = skinCrocoDefault.rootObject;
            newSkin.baseSkins = new SkinDef[] {skinCrocoDefault};
            
            Skins.AddSkinToCharacter(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody"), newSkin);
           

        }


    }
}
