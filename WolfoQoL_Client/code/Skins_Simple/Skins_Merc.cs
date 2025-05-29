using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{
    public class Skins_Merc
    {
        public static Material MatOniSword;


        public static void Start()
        {
            Texture2D TexRedSwordDiffuse = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texOniMercSwordDiffuse.png");
            Texture2D texRampFallbootsRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampFallbootsRed.png");
            Texture2D texRampHuntressRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinRamps/texRampHuntressRed.png");

            MatOniSword = Object.Instantiate(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody").transform.GetChild(0).GetChild(0).GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().material);
            MatOniSword.name = "matOniMercSword";
            MatOniSword.mainTexture = TexRedSwordDiffuse;
            MatOniSword.SetColor("_EmColor", new Color32(125, 64, 64, 255));
            MatOniSword.SetTexture("_FlowHeightRamp", texRampFallbootsRed);
            MatOniSword.SetTexture("_FresnelRamp", texRampHuntressRed);
 
        }


    }

}