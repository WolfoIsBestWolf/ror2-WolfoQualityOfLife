using HG;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public static class Skins_Merc
    {
        //public static Material MatOniSword;


        public static void Start()
        {
            if (WConfig.cfgSkinMercRedSword.Value)
            {
                Material MatOniSword = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "fb5eba8da12e09749b69b1a7f24afc00").WaitForCompletion());
                MatOniSword.name = "matOniMercSword";
                MatOniSword.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texOniMercSwordDiffuse.png");
                MatOniSword.SetColor("_EmColor", new Color32(125, 64, 64, 255));
                MatOniSword.SetTexture("_FlowHeightRamp", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampFallbootsRed.png"));
                MatOniSword.SetTexture("_FresnelRamp", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampHuntressRed.png"));

                SkinDefParams skinMercAlt_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "04f0dc07e902aaf4980ae949446cc6e9").WaitForCompletion();
                skinMercAlt_params.rendererInfos[1].defaultMaterialAddress = null;
                skinMercAlt_params.rendererInfos[1].defaultMaterial = MatOniSword;
            }
            #region Lights
            GameObject MercDisplay = Addressables.LoadAssetAsync<GameObject>(key: "c9d3d0b5ada36b74e83b3a2709467be2").WaitForCompletion();
            MercDisplay.transform.GetChild(0).GetComponent<CharacterModel>().baseLightInfos[0].light.name = "Point Light"; //Fix
            MercDisplay.transform.GetChild(0).gameObject.AddComponent<SkinMercListener>();
            SkinDefParams skinMercDefault_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "47240317eb88bd240a032186e1275fbb").WaitForCompletion();
            GameObject MercBody = Addressables.LoadAssetAsync<GameObject>(key: "c9898f15e54a0194dbd2ab62ad507bd4").WaitForCompletion();
            skinMercDefault_params.lightReplacements = MercBody.transform.GetChild(0).GetChild(0).GetComponent<CharacterModel>().baseLightInfos;
            MercBody.transform.GetChild(0).GetChild(0).gameObject.AddComponent<SkinMercListener>();
            if (WConfig.cfgSkinMercRed.Value)
            {
                SkinDefParams skinMercAlt_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "04f0dc07e902aaf4980ae949446cc6e9").WaitForCompletion();

                CharacterModel.LightInfo[] newLights = new CharacterModel.LightInfo[2];
                System.Array.Copy(skinMercDefault_params.lightReplacements, newLights, 2);
                newLights[0].defaultColor = new Color(1, 0.2f, 0.1f, 1);
                newLights[1].defaultColor = new Color(1, 0.15f, 0.15f, 1);
                skinMercAlt_params.lightReplacements = newLights;
            }
            if (WConfig.cfgSkinMercGreen.Value)
            {
                SkinDefParams skinMercAltColossus_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "5162e57e36a5758428c406bbe339fe24").WaitForCompletion();

                CharacterModel.LightInfo[] newLights = new CharacterModel.LightInfo[2];
                System.Array.Copy(skinMercDefault_params.lightReplacements, newLights, 2);
                newLights[0].defaultColor = new Color(0.2f, 1f, 0.1f, 1);
                newLights[1].defaultColor = new Color(0.15f, 0.6f, 0.15f, 1);
                skinMercAltColossus_params.lightReplacements = newLights;
            }
            if (WConfig.cfgSkinMercPink.Value)
            {
                SkinDefParams skinMercAltVulture_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Merc/skinMercAltVulture_params.asset").WaitForCompletion();

                CharacterModel.LightInfo[] newLights = new CharacterModel.LightInfo[2];
                System.Array.Copy(skinMercDefault_params.lightReplacements, newLights, 2);
                newLights[0].defaultColor = new Color(1f, 0.609f, 0.9f, 1);
                newLights[1].defaultColor = new Color(1f, 0.609f, 0.9f, 1);
                skinMercAltVulture_params.lightReplacements = newLights;
            }
            if (WConfig.cfgSkinMercSS2.Value)
            {
                ModelSkinController skins = MercBody.transform.GetChild(0).GetChild(0).GetComponent<ModelSkinController>();

                /*CharacterModel.LightInfo[] newLights = new CharacterModel.LightInfo[2];
                System.Array.Copy(skinMercDefault_params.lightReplacements, newLights, 2);
                newLights[0].defaultColor = new Color(1f, 0.609f, 0f, 1);
                newLights[1].defaultColor = new Color(1f, 0.609f,0f, 1);
                skinMercAltVulture_params.lightReplacements = newLights;*/

            }

            #endregion
        }


    }

    public class SkinMercListener : MonoBehaviour
    {
        public void OnEnable()
        {
            this.GetComponent<ModelSkinController>().onSkinApplied += OnMercSkinApplied;
        }
        public void OnDisable()
        {
            this.GetComponent<ModelSkinController>().onSkinApplied -= OnMercSkinApplied;
        }
        private void OnMercSkinApplied(int skinIndex)
        {
            CharacterModel model = this.GetComponent<CharacterModel>();
            if (!model)
            {
                return;
            }

            ChildLocator childLocator = this.GetComponent<ChildLocator>();
            ModelSkinController skins = GetComponent<ModelSkinController>();
            SkinDef skinDef = skins.skins[skinIndex];

            bool red = skinDef.name.EndsWith("Alt") && WConfig.cfgSkinMercRed.Value || skinDef.name.EndsWith("Red");
            bool green = skinDef.name.EndsWith("Colossus") && WConfig.cfgSkinMercGreen.Value || skinDef.name.EndsWith("Green");
            bool pink = skinDef.name.EndsWith("Vulture") && WConfig.cfgSkinMercPink.Value;
            bool ss2 = skinDef.name.StartsWith("skinMercAltVestige") && WConfig.cfgSkinMercSS2.Value;
            if (model.body)
            {
                ColorThisMerc mercColor = model.body.gameObject.EnsureComponent<ColorThisMerc>();
                if (red || ss2)
                {
                    mercColor.ThisColorEnum = MercColors.Red;
                }
                else if (green)
                {
                    mercColor.ThisColorEnum = MercColors.Green;
                }
                else if (pink)
                {
                    mercColor.ThisColorEnum = MercColors.Pink;
                }
                mercColor.ThisColor = (int)mercColor.ThisColorEnum;
            }

            if (childLocator == null)
            {
                Log.LogWarning("mdlMerc without childLocator");
                return;
            }
            Transform PreDashEffect = childLocator.FindChild("PreDashEffect");
            if (PreDashEffect == null)
            {
                Log.LogMessage("mdlMerc without PreDashEffect");
                return;
            }

            //Doing this in SkinDef might be good but guhh fuck that shit
            //I dont think we could set Particle Color there anyhow
            Color particleColor = Color.white;  //0.5613 0.6875 1 1 
            Color lightColor = Color.white;     //0.2028 0.6199 1 1
            Material ignition = null;           //matMercIgnition
            if (red)
            {
                particleColor = new Color(1f, 0.5613f, 0.6875f, 1);
                lightColor = new Color(1f, 0.2f, 0.2f, 1);
                ignition = Merc_Red.matMercIgnition_Red;
            }
            else if (green)
            {
                particleColor = new Color(0.6875f, 1f, 0.5613f, 1);
                lightColor = new Color(0.2f, 1f, 0.2f, 1);
                ignition = Merc_Green.matMercIgnition_Green;
            }
            else if (pink)
            {
                particleColor = new Color(0.844f, 0.561f, 0.844f, 1);
                lightColor = new Color(0.72f, 0.3f, 0.81f, 1);
                ignition = Merc_Pink.matMercIgnition_Pink;
            }
            else if (ss2)
            {
                particleColor = new Color(1, 0.7f, 0.5f, 1);
                lightColor = new Color(1f, 0.62f, 0.203f, 1);
                ignition = Merc_Red.matMercIgnition_Red;

                if (model.baseLightInfos.Length >= 2)
                {
                    model.baseLightInfos[0].defaultColor = new Color(1f, 0.609f, 0f, 1f);
                    model.baseLightInfos[1].defaultColor = new Color(1f, 0.609f, 0f, 1f);
                }
            }
            if (ignition != null)
            {
                PreDashEffect.GetChild(0).GetComponent<ParticleSystem>().startColor = particleColor;
                PreDashEffect.GetChild(1).GetComponent<Light>().color = lightColor;
                PreDashEffect.GetChild(2).GetComponent<ParticleSystem>().startColor = particleColor;
                PreDashEffect.GetChild(3).GetComponent<ParticleSystem>().startColor = particleColor;
                PreDashEffect.GetChild(2).GetComponent<ParticleSystemRenderer>().material = ignition; //matMercIgnition
                PreDashEffect.GetChild(3).GetComponent<ParticleSystemRenderer>().material = ignition;
            }

        }
    }

}