using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public class Skins_Merc
    {
        //public static Material MatOniSword;


        public static void Start()
        {
            if (!WConfig.cfgSkinMercRedSword.Value)
            {
                return;
            }
            Material MatOniSword = Object.Instantiate(Addressables.LoadAssetAsync<Material>(key: "fb5eba8da12e09749b69b1a7f24afc00").WaitForCompletion());
            MatOniSword.name = "matOniMercSword";
            MatOniSword.mainTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texOniMercSwordDiffuse.png");
            MatOniSword.SetColor("_EmColor", new Color32(125, 64, 64, 255));
            MatOniSword.SetTexture("_FlowHeightRamp", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampFallbootsRed.png"));
            MatOniSword.SetTexture("_FresnelRamp", Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampHuntressRed.png"));

            SkinDefParams skinMercAlt_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "04f0dc07e902aaf4980ae949446cc6e9").WaitForCompletion();
            skinMercAlt_params.rendererInfos[1].defaultMaterialAddress = null;
            skinMercAlt_params.rendererInfos[1].defaultMaterial = MatOniSword;

            #region Lights
            GameObject MercBody = Addressables.LoadAssetAsync<GameObject>(key: "c9898f15e54a0194dbd2ab62ad507bd4").WaitForCompletion();
            GameObject MercDisplay = Addressables.LoadAssetAsync<GameObject>(key: "c9d3d0b5ada36b74e83b3a2709467be2").WaitForCompletion();
            SkinDefParams skinMercDefault_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "47240317eb88bd240a032186e1275fbb").WaitForCompletion();
            SkinDefParams skinMercAltColossus_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "5162e57e36a5758428c406bbe339fe24").WaitForCompletion();
            SkinDefParams skinMercAltVulture_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Merc/skinMercAltVulture_params.asset").WaitForCompletion();

            skinMercDefault_params.lightReplacements = MercBody.transform.GetChild(0).GetChild(0).GetComponent<CharacterModel>().baseLightInfos;

            MercDisplay.transform.GetChild(0).GetComponent<CharacterModel>().baseLightInfos[0].light.name = "Point Light";

            CharacterModel.LightInfo[] red = new CharacterModel.LightInfo[2];
            System.Array.Copy(skinMercDefault_params.lightReplacements, red, 2);
            red[0].defaultColor = new Color(1, 0.2f, 0.1f, 1);
            red[1].defaultColor = new Color(1, 0.15f, 0.15f, 1);
            skinMercAlt_params.lightReplacements = red;

            CharacterModel.LightInfo[] green = new CharacterModel.LightInfo[2];
            System.Array.Copy(skinMercDefault_params.lightReplacements, green, 2);
            green[0].defaultColor = new Color(0.2f, 1f, 0.1f, 1);
            green[1].defaultColor = new Color(0.15f, 0.6f, 0.15f, 1);
            skinMercAltColossus_params.lightReplacements = green;


            CharacterModel.LightInfo[] pink = new CharacterModel.LightInfo[2];
            System.Array.Copy(skinMercDefault_params.lightReplacements, pink, 2);
            pink[0].defaultColor = new Color(1f, 0.609f, 0.9f, 1);
            pink[1].defaultColor = new Color(1f, 0.609f, 0.9f, 1);
            skinMercAltVulture_params.lightReplacements = pink;
            #endregion
        }


    }

}