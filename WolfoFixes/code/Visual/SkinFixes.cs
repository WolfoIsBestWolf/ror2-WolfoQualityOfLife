using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    internal class SkinFixes
    {


        public static void Start()
        {
            FixPrintController();
            FixTitanPing();
            FixMinorConstructOnKillBody();
            BrotherPingAndGlass();

            On.RoR2.CharacterModel.IsAurelioniteAffix += DontRemoveGildedVisualsOnDeath;
            On.RoR2.ModelSkinController.ApplySkinAsync += ApplyFallbackSkinIfSkinIndexOutofBounds;
        }

        private static bool DontRemoveGildedVisualsOnDeath(On.RoR2.CharacterModel.orig_IsAurelioniteAffix orig, CharacterModel self)
        {
            if (self.myEliteIndex == DLC2Content.Elites.Aurelionite.eliteIndex)
            {
                return true;
            }
            return orig(self);
        }

        private static System.Collections.IEnumerator ApplyFallbackSkinIfSkinIndexOutofBounds(On.RoR2.ModelSkinController.orig_ApplySkinAsync orig, ModelSkinController self, int skinIndex, RoR2.ContentManagement.AsyncReferenceHandleUnloadType unloadType)
        {
            if ((ulong)skinIndex >= (ulong)(long)self.skins.Length)
            {
                if (self.skins.Length > 0)
                {
                    skinIndex %= self.skins.Length;
                }
            }
            return orig(self, skinIndex, unloadType);
        }

        public static void FixPrintController()
        {

            //Missing spawn sliceHeight, leading to them not appearing slowly 
            Addressables.LoadAssetAsync<GameObject>(key: "58a9e9bdc0b198b4180505777ec9a034").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //Child
            Addressables.LoadAssetAsync<GameObject>(key: "b39683568507e2e498f005d3b225e52d").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //Parent
            Addressables.LoadAssetAsync<GameObject>(key: "64b97b2c7e3e0d949b41abbe57bf3c2d").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //MegaConstruct
            Addressables.LoadAssetAsync<GameObject>(key: "4331e499912b59b47a5e45cc2ea738bb").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //Vagrant
            Addressables.LoadAssetAsync<GameObject>(key: "a3fe70c460a83a34991cfd64a74006f0").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //LesserWisp
            Addressables.LoadAssetAsync<GameObject>(key: "940b43e2cd9ca7448b8812bae520cc9a").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //LesserWispSoul
            Addressables.LoadAssetAsync<GameObject>(key: "367cb012c5eb41c4c8f3aa69262b5c2c").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //GreaterWisp
            Addressables.LoadAssetAsync<GameObject>(key: "045f51ff80ad54e4a925be34fc3a8358").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //LunarWisp
            Addressables.LoadAssetAsync<GameObject>(key: "4ac56d0b387fe344bba43f086ba72fe7").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //Nullfier
            Addressables.LoadAssetAsync<GameObject>(key: "1b5bf2d36cd4e434382d764f2d98a88d").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //NullfierA
            Addressables.LoadAssetAsync<GameObject>(key: "097b0e271757ce24581d4a8983d2c941").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //VoidMegaCrab
            Addressables.LoadAssetAsync<GameObject>(key: "285347cf04a9df04b9dada8fed09832f").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //VoidMegaCrabA
            Addressables.LoadAssetAsync<GameObject>(key: "7646a10c923c7f848992947b4780038c").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //VoidlingB
            Addressables.LoadAssetAsync<GameObject>(key: "d76e0df3fd0ac2d4d8dd119e04219fad").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //Voidling1
            Addressables.LoadAssetAsync<GameObject>(key: "8de35bee04772824ea08de4d02c4e4fe").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //Voidling2
            Addressables.LoadAssetAsync<GameObject>(key: "8d48c8e6775417a458c7f8b539e237f3").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //Voidling3
            Addressables.LoadAssetAsync<GameObject>(key: "04508f474b420d546b5d55a9a18a9698").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //EngiTurret
            Addressables.LoadAssetAsync<GameObject>(key: "78e114e549394f945b82672fd4893994").WaitForCompletion().GetComponentInChildren<PrintController>().materialPrintCutoffPostSkinApplying = true; //EngiTurretW


        }

        public static void FixMinorConstructOnKillBody()
        {
            Addressables.LoadAssetAsync<GameObject>(key: "bf5b53aa9306bce4b910220a43b30062").WaitForCompletion().transform.GetChild(0).GetChild(0).GetComponent<ModelSkinController>().skins[0] = Addressables.LoadAssetAsync<SkinDef>(key: "114a09a96091efd43896f52f45be4adf").WaitForCompletion();
        }

        public static void FixTitanPing()
        {
            //Swap Body first so Body pinged instead of random particles.
            SkinDefParams titanGP = Addressables.LoadAssetAsync<SkinDefParams>(key: "21b8d0bf5fdccb94b9e5694f28491514").WaitForCompletion();
            HG.ArrayUtils.Swap(titanGP.rendererInfos, 19, 0);
            SkinDefParams titanDC = Addressables.LoadAssetAsync<SkinDefParams>(key: "980ab5f724b028847af364dc596630c8").WaitForCompletion();
            HG.ArrayUtils.Swap(titanDC.rendererInfos, 19, 0);
            SkinDefParams titanGL = Addressables.LoadAssetAsync<SkinDefParams>(key: "0fbedd25dbb06224e8615535dfe8c1d0").WaitForCompletion();
            HG.ArrayUtils.Swap(titanGL.rendererInfos, 19, 0);
            SkinDefParams titanBB = Addressables.LoadAssetAsync<SkinDefParams>(key: "5ef4744f083778346bd813d33dfd7c39").WaitForCompletion();
            HG.ArrayUtils.Swap(titanBB.rendererInfos, 19, 0);

            SkinDefParams aur = Addressables.LoadAssetAsync<SkinDefParams>(key: "ea05e89f54cbdee409061420648b0cd9").WaitForCompletion();
            HG.ArrayUtils.Swap(aur.rendererInfos, 19, 0);

        }


        public static void BrotherPingAndGlass()
        {
            //Swap Body first so Body pinged instead of hammer.
            SkinDef skinBrotherBodyDefault = Addressables.LoadAssetAsync<SkinDef>(key: "9f70f6d6114130041967d933f25db793").WaitForCompletion();
            SkinDefParams paramsBrotherBodyDefault = Addressables.LoadAssetAsync<SkinDefParams>(key: "40e2275652aaf89429ac1c68b5036259").WaitForCompletion();
            HG.ArrayUtils.Swap(paramsBrotherBodyDefault.rendererInfos, 6, 0);


            //Cannot do stacked renders with render infos or skins I believe.
            //The mats are still set in the bodies model so just having a skin where those 2 renders are not overwritten
            SkinDef skinBrotherGlass = Object.Instantiate(skinBrotherBodyDefault);
            SkinDefParams paramsBrotherGlass = Object.Instantiate(paramsBrotherBodyDefault);
            skinBrotherGlass.name = "skinBrotherGlass";
            paramsBrotherGlass.name = "paramsBrotherGlass";
            paramsBrotherGlass.rendererInfos = HG.ArrayUtils.Clone(paramsBrotherBodyDefault.rendererInfos);

            skinBrotherGlass.skinDefParams = paramsBrotherGlass;
            skinBrotherGlass.skinDefParamsAddress = new AssetReferenceT<SkinDefParams>("");

            HG.ArrayUtils.ArrayRemoveAtAndResize(ref paramsBrotherGlass.rendererInfos, 6);
            HG.ArrayUtils.ArrayRemoveAtAndResize(ref paramsBrotherGlass.rendererInfos, 0);

            GameObject brotherGlassBody = Addressables.LoadAssetAsync<GameObject>(key: "1a4c444a7f6a47e41bdc558a2c5e68a7").WaitForCompletion();
            var model = brotherGlassBody.GetComponentInChildren<ModelSkinController>();
            if (model)
            {
                model.skins = new SkinDef[] { skinBrotherGlass };
            }
        }



    }

}