using RoR2;
using UnityEngine;

namespace WolfoQualityOfLife
{
    public class Stupid
    {
        public static void Start()
        {
            //Command view all items
            On.RoR2.PickupPickerController.GetOptionsFromPickupIndex += (orig, pickupIndex) =>
            {
                if (pickupIndex == PickupCatalog.FindPickupIndex(JunkContent.Items.AACannon.itemIndex))
                {
                    PickupPickerController.Option[] array = new PickupPickerController.Option[ItemCatalog.itemCount];
                    for (int i = 0; i < ItemCatalog.itemCount; i++)
                    {
                        //Debug.LogWarning(pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i));
                        array[i] = new PickupPickerController.Option
                        {
                            available = true,
                            pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i)
                        };
                    }
                    return array;
                }
                return orig(pickupIndex);
            };




        }




        public static void ModelViewer()
        {
            Texture2D texArchWisp = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texArchWisp.LoadImage(Properties.Resources.autogen_ArchWispBody, true);
            texArchWisp.filterMode = FilterMode.Bilinear;
            texArchWisp.wrapMode = TextureWrapMode.Clamp;
            Texture2D texAncientWisp = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texAncientWisp.LoadImage(Properties.Resources.autogen_AncientWispBody, true);
            texAncientWisp.filterMode = FilterMode.Bilinear;
            texAncientWisp.wrapMode = TextureWrapMode.Clamp;
            Texture2D texHAND = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texHAND.LoadImage(Properties.Resources.autogen_HANDBody, true);
            texHAND.filterMode = FilterMode.Bilinear;
            texHAND.wrapMode = TextureWrapMode.Clamp;
            Texture2D texOldBandit = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texOldBandit.LoadImage(Properties.Resources.autogen_texBodyOldBanditIcon, true);
            texOldBandit.filterMode = FilterMode.Bilinear;
            texOldBandit.wrapMode = TextureWrapMode.Clamp;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArchWispBody").GetComponent<CharacterBody>().portraitIcon = texArchWisp; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AncientWispBody").GetComponent<CharacterBody>().portraitIcon = texAncientWisp; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HANDBody").GetComponent<CharacterBody>().portraitIcon = texHAND; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BanditBody").GetComponent<CharacterBody>().portraitIcon = texOldBandit; //

            UnlockableDef dummyunlock = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleBody").GetComponent<DeathRewards>().logUnlockableDef;

            for (int i = 0; i < BodyCatalog.bodyCount; i++)
            {
                DeathRewards deathRewards = BodyCatalog.bodyPrefabs[i].GetComponent<DeathRewards>();
                if (deathRewards)
                {
                    deathRewards.logUnlockableDef = dummyunlock;
                }
                else
                {
                    BodyCatalog.bodyPrefabs[i].AddComponent<DeathRewards>().logUnlockableDef = dummyunlock;
                }
            }


        }

    }

}
