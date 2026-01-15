using R2API;
using RoR2;
using RoR2.ExpansionManagement;
using RoR2.UI;
using RoR2.UI.LogBook;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client
{
    public static partial class ColorModule
    {
        //Adding missing Highlights

        public static GameObject EquipmentBossOrb;
        public static GameObject EquipmentLunarOrb;
        public static GameObject NoTierOrb;

        public static Color NewSurvivorLogbookNameColor = new Color32(80, 130, 173, 255);

        public static Color ColorEquip_Lunar;
        public static Color ColorEquip_Boss;
        public static Color ColorEquip_Consumed;
  
        public static Color Color_Quest;
        public static Color Color_QuestDark; 

      

        public static Texture2D texEquipmentBossBG = new Texture2D(512, 512, TextureFormat.DXT5, false);
        public static Texture2D texEquipmentLunarBG = new Texture2D(512, 512, TextureFormat.DXT5, false);

        public static ColorCatalog.ColorIndex index_EquipLunar = ColorCatalog.ColorIndex.LunarItem;
        public static ColorCatalog.ColorIndex index_EquipBoss = ColorCatalog.ColorIndex.BossItem;
        public static ColorCatalog.ColorIndex index_EquipConsumed = ColorCatalog.ColorIndex.Equipment;

        public static ColorCatalog.ColorIndex index_Void1 = ColorCatalog.ColorIndex.VoidItem;
        public static ColorCatalog.ColorIndex index_Void3 = ColorCatalog.ColorIndex.VoidItem;
        public static ColorCatalog.ColorIndex index_Void4 = ColorCatalog.ColorIndex.VoidItem;

        public static ColorCatalog.ColorIndex index_Food = ColorCatalog.ColorIndex.Equipment;
        public static ColorCatalog.ColorIndex index_Quest = ColorCatalog.ColorIndex.BossItem;
        public static ColorCatalog.ColorIndex index_QuestDark = ColorCatalog.ColorIndex.BossItemDark;

        public static void Main()
        {
            ColorUtility.TryParseHtmlString("#78AFFF", out ColorEquip_Lunar);
            ColorUtility.TryParseHtmlString("#FFC211", out ColorEquip_Boss);
            ColorUtility.TryParseHtmlString("#C9731D", out ColorEquip_Consumed);
            ColorUtility.TryParseHtmlString("#FF9EEC", out var ColorVoid1);
            //Void Green stays the default
            ColorUtility.TryParseHtmlString("#FF73BF", out var ColorVoid3); //1 0.45 0.75 1
            ColorUtility.TryParseHtmlString("#E658A6", out var ColorVoid4);
            ColorUtility.TryParseHtmlString("#FF994B", out var Color_Food);

            OrbMaker();

            //ColorAPI now actually works
            index_EquipLunar = ColorsAPI.RegisterColor(ColorEquip_Lunar);
            index_EquipBoss = ColorsAPI.RegisterColor(ColorEquip_Boss);
            index_EquipConsumed = ColorsAPI.RegisterColor(ColorEquip_Consumed);
            index_Void1 = ColorsAPI.RegisterColor(ColorVoid1);
            index_Void3 = ColorsAPI.RegisterColor(ColorVoid3);
            index_Void4 = ColorsAPI.RegisterColor(ColorVoid4);
            index_Food = ColorsAPI.RegisterColor(Color_Food);

            Color_Quest = new Color(0.9216f, 1f, 0.0157f, 1);
            Color_QuestDark = new Color(0.7059f, 0.7412f, 0.2353f, 1);

            index_Quest = ColorsAPI.RegisterColor(Color_Quest);
            index_QuestDark = ColorsAPI.RegisterColor(Color_QuestDark);


            texEquipmentBossBG = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/General/texEquipmentBossBG.png");
            texEquipmentLunarBG = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/General/texEquipmentLunarBG.png");

            if (WConfig.cfgColorMain.Value)
            {
                On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += EquipmentAddBG;

                EquipmentCatalog.availability.CallWhenAvailable(NewColorOutlineIcons);
                On.RoR2.PickupCatalog.Init += PickupCatalog_Init;

                On.RoR2.ItemDef.CreatePickupDef += ItemDef_CreatePickupDef;
                On.RoR2.EquipmentDef.CreatePickupDef += EquipmentDef_CreatePickupDef;
            }

            PlayerPing.Hooks();

        }

        public static Entry[] EquipmentAddBG(On.RoR2.UI.LogBook.LogBookController.orig_BuildPickupEntries orig, Dictionary<ExpansionDef, bool> expansionAvailability)
        {
            //Only run with color module.
            Entry[] array = orig(expansionAvailability);
            for (int i = 0; i < array.Length; i++)
            {
                PickupIndex tempind = PickupCatalog.FindPickupIndex(array[i].extraData.ToString());
                PickupDef temppickdef = PickupCatalog.GetPickupDef(tempind);
                if (temppickdef.equipmentIndex != EquipmentIndex.None)
                {
                    EquipmentDef tempeqdef = EquipmentCatalog.GetEquipmentDef(temppickdef.equipmentIndex);
                    if (WConfig.cfgColorMain.Value)
                    {
                        if (tempeqdef.isBoss == true)
                        {
                            array[i].bgTexture = ColorModule.texEquipmentBossBG;
                        }
                        else if (tempeqdef.isLunar == true)
                        {
                            array[i].bgTexture = ColorModule.texEquipmentLunarBG;
                        }
                    }
                }
            }
            return array;
        }


        private static PickupDef ItemDef_CreatePickupDef(On.RoR2.ItemDef.orig_CreatePickupDef orig, ItemDef self)
        {
            if (self.tier == ItemTier.NoTier)
            {
                PickupDef pickup = orig(self);
                pickup.dropletDisplayPrefab = NoTierOrb;
                return pickup;
            }
            return orig(self);
        }

        private static PickupDef EquipmentDef_CreatePickupDef(On.RoR2.EquipmentDef.orig_CreatePickupDef orig, EquipmentDef self)
        {
            PickupDef pickup = orig(self);
            if (self.isBoss)
            {
                pickup.dropletDisplayPrefab = EquipmentBossOrb;
            }
            else if (self.isLunar)
            {
                pickup.dropletDisplayPrefab = EquipmentLunarOrb;
            }
            return pickup;
        }

        public static void MissingSceneColors()
        {
            //SceneCatalog.FindSceneDef("arena").environmentColor = new Color32(153, 247, 219, 255);

            SceneCatalog.FindSceneDef("lakes").environmentColor = new Color32(153, 247, 219, 255);
            SceneCatalog.FindSceneDef("lakesnight").environmentColor = new Color32(116, 93, 166, 255);
            SceneCatalog.FindSceneDef("village").environmentColor = new Color32(170, 179, 145, 255);
            SceneCatalog.FindSceneDef("villagenight").environmentColor = new Color32(71, 137, 168, 255);
            SceneCatalog.FindSceneDef("lemuriantemple").environmentColor = new Color32(246, 188, 67, 255);
            SceneCatalog.FindSceneDef("habitat").environmentColor = new Color32(163, 182, 94, 255);
            SceneCatalog.FindSceneDef("habitatfall").environmentColor = new Color32(214, 121, 66, 255);
            SceneCatalog.FindSceneDef("meridian").environmentColor = new Color32(132, 201, 255, 255);
            SceneCatalog.FindSceneDef("helminthroost").environmentColor = new Color32(203, 0, 0, 255);

            SceneCatalog.FindSceneDef("artifactworld01").environmentColor = new Color(0.6f, 0.2f, 0.18f, 1f);
            SceneCatalog.FindSceneDef("artifactworld02").environmentColor = new Color(0.5f, 0.5f, 0.18f, 1f);
            SceneCatalog.FindSceneDef("artifactworld03").environmentColor = new Color(0.5f, 0.25f, 0.18f, 1f);

            //AC
            SceneCatalog.FindSceneDef("nest").environmentColor = new Color32(145, 170, 198, 255);
            SceneCatalog.FindSceneDef("ironalluvium").environmentColor = new Color32(228, 194, 134, 255);
            SceneCatalog.FindSceneDef("ironalluvium2").environmentColor = new Color32(87, 136, 105, 255);
            SceneCatalog.FindSceneDef("repurposedcrater").environmentColor = new Color32(230, 158, 139, 255);
            SceneCatalog.FindSceneDef("conduitcanyon").environmentColor = new Color32(16, 77, 107, 255);
            SceneCatalog.FindSceneDef("solutionalhaunt").environmentColor = new Color32(79, 163, 170, 255);
            SceneCatalog.FindSceneDef("computationalexchange").environmentColor = new Color32(90, 219, 181, 255);
            SceneCatalog.FindSceneDef("solusweb").environmentColor = new Color32(255, 0, 148, 255);



        }



        private static System.Collections.IEnumerator PickupCatalog_Init(On.RoR2.PickupCatalog.orig_Init orig)
        {
            ChangeColorsViaIndex();
            return orig();
        }

        public static void OrbMaker()
        {
            GameObject EquipmentOrb = Addressables.LoadAssetAsync<GameObject>(key: "7c61d88eadef8a94ebf06138b6d2c2cb").WaitForCompletion();


            EquipmentBossOrb = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "47decc91b30009d41a6b60735cd38ed9").WaitForCompletion(), "EquipmentBossOrb", false); //boss orb base
            EquipmentLunarOrb = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "69411ae2cb84bd744827d19b2042f749").WaitForCompletion(), "EquipmentLunarOrb", false); //lunar orb base
            NoTierOrb = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "62b4a745e2a56ad4880ba1418a12ee85").WaitForCompletion(), "NoTierOrb", false); //tier 1 orb

            Color reduction = new Color(0.2f, 0.2f, 0.2f, 0f);
            Color reduction2 = new Color(0.5f, 0.5f, 0.5f, 1f);

            #region Equipment + Boss

            PlaySoundOnEvent[] sounds = EquipmentBossOrb.GetComponents<PlaySoundOnEvent>();
            sounds[0].soundEvent = "Play_item_use_bossHunter";
            sounds[1].soundEvent = "Play_UI_item_land_tier3";
            sounds[0].akSoundEvent = null;
            sounds[1].akSoundEvent = null;

            EquipmentBossOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = ColorEquip_Boss * reduction2;
            EquipmentBossOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color(1, 0f, 0f, 0f);
            EquipmentBossOrb.transform.GetChild(0).GetChild(2).GetComponent<Light>().color = ColorEquip_Boss;

            GradientColorKey[] colorKeysB = new GradientColorKey[]
            {
                new GradientColorKey
                {
                    color = ColorEquip_Boss,
                    time = 0f,
                },
                new GradientColorKey
                {
                     color = ColorEquip_Boss,
                     time = 1
                }
            };
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys = colorKeysB;
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys = colorKeysB;
            #endregion
            #region Equipment + Lunar
            EquipmentLunarOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = ColorEquip_Lunar * reduction2;
            EquipmentLunarOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color(0f, 0f, 1f, 0f);
            EquipmentLunarOrb.transform.GetChild(0).GetChild(1).GetComponent<Light>().color = ColorEquip_Lunar;

            GradientColorKey[] colorKeysL = new GradientColorKey[]
             {
                new GradientColorKey
                {
                    color = ColorEquip_Lunar,
                    time = 0f,
                },
                new GradientColorKey
                {
                     color = ColorEquip_Lunar,
                     time = 1
                }
             };
            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys = colorKeysL;
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys = colorKeysL;
            Object.Instantiate(EquipmentOrb.transform.GetChild(0).GetChild(2).gameObject, EquipmentBossOrb.transform.GetChild(0));
            Object.Instantiate(EquipmentOrb.transform.GetChild(0).GetChild(2).gameObject, EquipmentLunarOrb.transform.GetChild(0));
            #endregion


            GradientColorKey[] colorKeysN = new GradientColorKey[]
             {
                new GradientColorKey
                {
                    color = ColorEquip_Lunar,
                    time = 0f,
                },
                new GradientColorKey
                {
                     color = ColorEquip_Lunar,
                     time = 1
                }
             };

            NoTierOrb.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            NoTierOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color(0f, 0f, 0f, 0f);
            NoTierOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color(0.5f, 0.5f, 0.5f, 1);

            sounds = NoTierOrb.GetComponents<PlaySoundOnEvent>();
            sounds[0].soundEvent = "Play_ui_lunar_coin_drop";
            sounds[1].soundEvent = "Play_UI_lunarCache_open";
            sounds[0].akSoundEvent = null;
            sounds[1].akSoundEvent = null;
        }


        public static void ChangeColorsViaIndex()
        {
            if (WConfig.cfgColorVoids.Value)
            {
                ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier1).colorIndex = index_Void1;
                ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier3).colorIndex = index_Void3;
                ItemTierCatalog.GetItemTierDef(ItemTier.VoidBoss).colorIndex = index_Void4;
            }
          
            ItemTierCatalog.GetItemTierDef(ItemTier.FoodTier).colorIndex = index_Food;
            ItemTierCatalog.GetItemTierDef(ItemTier.FoodTier).bgIconTexture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/General/FoodBGIcon.png");
 
            for (int i = 0; EquipmentCatalog.equipmentDefs.Length > i; i++)
            {
                EquipmentDef def = EquipmentCatalog.equipmentDefs[i];
                if (def.passiveBuffDef && def.passiveBuffDef.isElite)
                {
                    def.isBoss = true;
                }
                if (def.isBoss)
                {
                    def.colorIndex = index_EquipBoss;
                }
                else if (def.isLunar)
                {
                    def.colorIndex = index_EquipLunar;
                }
                else if (def.isConsumed)
                {
                    def.colorIndex = index_EquipConsumed;
                }
            }
 
        }

        public static void ChangeColorsPost()
        {
   
            //Void Coins already use their own color
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).baseColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin);
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).darkColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin);
 
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(RoR2Content.Items.DrizzlePlayerHelper.itemIndex)).baseColor = Color_Quest;
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(RoR2Content.Items.DrizzlePlayerHelper.itemIndex)).darkColor = Color_QuestDark;
        }

        public static void AddMissingItemHighlights()
        {
            GameObject Tier2 = Addressables.LoadAssetAsync<GameObject>(key: "730ea84bde179504e985c5f3c66db36b").WaitForCompletion();
            Sprite texUICornerTier3 = Addressables.LoadAssetAsync<Sprite>(key: "43a45be290137ab439552af4ff6eb6c4").WaitForCompletion();


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/HighlightLunarItem.prefab").WaitForCompletion().GetComponent<HighlightRect>().highlightColor = new Color32(55, 101, 255, 255);//new Color(0.3f, 0.6f, 1, 1);
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/HighlightBossItem.prefab").WaitForCompletion().GetComponent<HighlightRect>().cornerImage = texUICornerTier3;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/HighlightVoidBossItem.prefab").WaitForCompletion().GetComponent<HighlightRect>().cornerImage = texUICornerTier3;

            GameObject HighlightEquipmentLunar = PrefabAPI.InstantiateClone(Tier2, "HighlightEquipmentLunar", false);
            GameObject HighlightEquipmentBoss = PrefabAPI.InstantiateClone(Tier2, "HighlightEquipmentBoss", false);

            HighlightEquipmentLunar.GetComponent<HighlightRect>().highlightColor = ColorEquip_Lunar;
            HighlightEquipmentBoss.GetComponent<HighlightRect>().highlightColor = new Color(1, 0.75f, 0f, 1);
            HighlightEquipmentBoss.GetComponent<HighlightRect>().cornerImage = texUICornerTier3;

            EquipmentCatalog.lunarEquipmentHighlightPrefab = HighlightEquipmentLunar;
            EquipmentCatalog.bossEquipmentHighlightPrefab = HighlightEquipmentBoss;

        }



    }

}
