using RoR2;
using UnityEngine;

namespace WolfoQoL_Client.Skins
{
    public static class Skins_Loader
    {
        public static bool[] LoaderFistAllowed;
        public static void Start()
        {
            SkinDef[] loaderSkins = SkinCatalog.FindSkinsForBody(RoR2Content.BodyPrefabs.LoaderBody.bodyIndex);
            LoaderFistAllowed = new bool[loaderSkins.Length];
            LoaderFistAllowed[0] = false;
            for (int i = 1; i < loaderSkins.Length; i++)
            {
                if (!loaderSkins[i].name.Contains("Colossus"))
                {
                    LoaderFistAllowed[i] = true;
                }
                else
                {
                    LoaderFistAllowed[i] = false;
                }
            }


            //Somehow not a ghost
            On.EntityStates.Loader.FireHook.SetHookReference += (orig, self, hook) =>
            {
                if (LoaderFistAllowed[self.characterBody.skinIndex])
                {
                    //hook.transform.GetChild(0).GetComponent<MeshRenderer>().material = self.modelLocator.modelTransform.GetComponent<CharacterModel>().baseRendererInfos[0].defaultMaterial;
                    hook.transform.GetChild(0).GetComponent<MeshRenderer>().material = self.modelLocator.modelTransform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
                }
                orig(self, hook);
            };
        }

    }

}