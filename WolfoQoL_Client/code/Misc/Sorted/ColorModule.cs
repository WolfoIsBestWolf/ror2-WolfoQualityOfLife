using R2API;
using RoR2;
using RoR2.UI;
//using System;
using System.Collections.Generic;
using UnityEngine;

namespace WolfoQoL_Client
{
    public class ColorModule
    {
        //Adding missing Highlights
        public static readonly GameObject HighlightOrangeItem = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipment", false);
        public static readonly GameObject HighlightOrangeBossItem = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipmentBoss", false);
        public static readonly GameObject HighlightOrangeLunarItem = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipmentLunar", false);
        public static bool HighlightEquipment = false;


        public static readonly GameObject EquipmentBossOrb = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/BossOrb"), "EquipmentBossOrb", false);
        public static readonly GameObject EquipmentLunarOrb = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/LunarOrb"), "EquipmentLunarOrb", false);
        public static readonly GameObject NoTierOrb = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/Tier1Orb"), "NoTierOrb", false);

        public static Color NewSurvivorLogbookNameColor = new Color32(80, 130, 173, 255);

        public static Color ColorEquip_Lunar;
        public static Color ColorEquip_Boss;
        public static Color ColorEquip_Consumed;
        //public static Color ColorEquip_Dark;
        //public static Color ColorEquip_LunarDark;
        //public static Color ColorEquip_BossDark;

        public static Color ColorVoid1;
        public static Color ColorVoid3;
        public static Color ColorVoid4;

        //public static Color ColorVoid1Dark;
        //public static Color ColorVoid3Dark;
        //public static Color ColorVoid4Dark;

        public static Texture2D texEquipmentBossBG = new Texture2D(512, 512, TextureFormat.DXT5, false);
        public static Texture2D texEquipmentLunarBG = new Texture2D(512, 512, TextureFormat.DXT5, false);

        public static ColorCatalog.ColorIndex index_EquipLunar = ColorCatalog.ColorIndex.LunarItem;
        public static ColorCatalog.ColorIndex index_EquipBoss = ColorCatalog.ColorIndex.BossItem;
        public static ColorCatalog.ColorIndex index_EquipConsumed = ColorCatalog.ColorIndex.Equipment;

        public static ColorCatalog.ColorIndex index_Void1 = ColorCatalog.ColorIndex.VoidItem;
        public static ColorCatalog.ColorIndex index_Void3 = ColorCatalog.ColorIndex.VoidItem;
        public static ColorCatalog.ColorIndex index_Void4 = ColorCatalog.ColorIndex.VoidItem;

        public static void Main()
        {
            ColorUtility.TryParseHtmlString("#78AFFF", out ColorEquip_Lunar);
            ColorUtility.TryParseHtmlString("#FFC211", out ColorEquip_Boss);
            ColorUtility.TryParseHtmlString("#C9731D", out ColorEquip_Consumed);
            //ColorUtility.TryParseHtmlString("#C9731D", out ColorEquip_Dark);
            //ColorUtility.TryParseHtmlString("#7BA0D6", out ColorEquip_LunarDark);
            //ColorUtility.TryParseHtmlString("#CAA127", out ColorEquip_BossDark);

            //Void Green stays the default
            ColorUtility.TryParseHtmlString("#FF9EEC", out ColorVoid1);
            ColorUtility.TryParseHtmlString("#FF73BF", out ColorVoid3); //1 0.45 0.75 1
            ColorUtility.TryParseHtmlString("#E658A6", out ColorVoid4);

            /*ColorUtility.TryParseHtmlString("#B065A1", out ColorVoidDarkWhite);
            ColorUtility.TryParseHtmlString("#B24681", out ColorVoidDarkRed);
            ColorUtility.TryParseHtmlString("#A13470", out ColorVoidDarkYellow);*/
            OrbMaker();

            //ColorAPI now actually works
            index_EquipLunar = ColorsAPI.RegisterColor(ColorEquip_Lunar);
            index_EquipBoss = ColorsAPI.RegisterColor(ColorEquip_Boss);
            index_EquipConsumed = ColorsAPI.RegisterColor(ColorEquip_Consumed);
            index_Void1 = ColorsAPI.RegisterColor(ColorVoid1);
            index_Void3 = ColorsAPI.RegisterColor(ColorVoid3);
            index_Void4 = ColorsAPI.RegisterColor(ColorVoid4);

            Debug.Log(index_EquipBoss);

            texEquipmentBossBG = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/General/texEquipmentBossBG.png");
            texEquipmentLunarBG = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/General/texEquipmentLunarBG.png");


            EquipmentCatalog.availability.CallWhenAvailable(ColorModule_Sprites.NewColorOutlineIcons);
            On.RoR2.PickupCatalog.Init += PickupCatalog_Init;

            On.RoR2.ItemDef.CreatePickupDef += ItemDef_CreatePickupDef;
            On.RoR2.EquipmentDef.CreatePickupDef += EquipmentDef_CreatePickupDef;
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

        private static System.Collections.IEnumerator PickupCatalog_Init(On.RoR2.PickupCatalog.orig_Init orig)
        {
            ChangeColorsViaIndex();
            return orig();
        }

        public static void OrbMaker()
        {

            GameObject EquipmentOrb = LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/EquipmentOrb");
            Color reduction = new Color(0.2f, 0.2f, 0.2f, 0f);
            Color reduction2 = new Color(0.5f, 0.5f, 0.5f, 1f);

            #region Equipment + Boss

            PlaySoundOnEvent[] sounds = EquipmentBossOrb.GetComponents<PlaySoundOnEvent>();
            sounds[0].soundEvent = "Play_item_use_bossHunter";
            sounds[1].soundEvent = "Play_UI_item_land_tier3";

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

        }


        public static void ChangeColorsViaIndex()
        {
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier1).colorIndex = index_Void1;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier3).colorIndex = index_Void3;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidBoss).colorIndex = index_Void4;

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
            //JunkContent.Equipment.EliteGoldEquipment.isBoss = true;
            //JunkContent.Equipment.EliteYellowEquipment.isBoss = true;
        }

        public static void ChangeColorsPost()
        {
            //Void Coins already use their own color
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).baseColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin);
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).darkColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin);

            Color YellowGreen = new Color(0.9f, 1f, 0.1f, 1);
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(RoR2Content.Items.DrizzlePlayerHelper.itemIndex)).baseColor = YellowGreen;
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(RoR2Content.Items.DrizzlePlayerHelper.itemIndex)).darkColor = YellowGreen;
        }

        public static void AddMissingItemHighlights()
        {
            GameObject HighlightBlueItem = LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightLunarItem");
            HighlightBlueItem.GetComponent<HighlightRect>().highlightColor = new Color32(55, 101, 255, 255);//new Color(0.3f, 0.6f, 1, 1);

            GameObject HighlightYellowItem = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier3Item"), "HighlightBossItem", false);
            GameObject HighlightPinkT1Item = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier1Item"), "HighlightVoidT1Item", false);
            GameObject HighlightPinkT2Item = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightVoidT2Item", false);
            GameObject HighlightPinkT3Item = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier3Item"), "HighlightVoidT3Item", false);

            HighlightYellowItem.GetComponent<HighlightRect>().highlightColor = new Color(1f, 0.9373f, 0.2667f, 1);
            HighlightPinkT1Item.GetComponent<HighlightRect>().highlightColor = new Color(1f, 0.498f, 0.9059f, 1);
            HighlightPinkT2Item.GetComponent<HighlightRect>().highlightColor = new Color(1f, 0.498f, 0.9059f, 1);
            HighlightPinkT3Item.GetComponent<HighlightRect>().highlightColor = new Color(1f, 0.498f, 0.9059f, 1);
            HighlightOrangeItem.GetComponent<HighlightRect>().highlightColor = new Color(1f, 0.6471f, 0.298f, 1);
            HighlightOrangeBossItem.GetComponent<HighlightRect>().highlightColor = new Color(1, 0.75f, 0f, 1);
            HighlightOrangeLunarItem.GetComponent<HighlightRect>().highlightColor = ColorEquip_Lunar;


            ItemTierCatalog.GetItemTierDef(ItemTier.Boss).highlightPrefab = HighlightYellowItem;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier1).highlightPrefab = HighlightPinkT1Item;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier2).highlightPrefab = HighlightPinkT2Item;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier3).highlightPrefab = HighlightPinkT3Item;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidBoss).highlightPrefab = HighlightPinkT3Item;


            On.RoR2.EquipmentDef.AttemptGrant += EquipmentDef_AttemptGrant;
            On.RoR2.CharacterModel.SetEquipmentDisplay += EquipmentHighlighter;
        }

        private static void EquipmentDef_AttemptGrant(On.RoR2.EquipmentDef.orig_AttemptGrant orig, ref PickupDef.GrantContext context)
        {
            orig(ref context);
            if (context.body.hasAuthority)
            {
                HighlightEquipment = true;
            }
        }

        public static void EquipmentHighlighter(On.RoR2.CharacterModel.orig_SetEquipmentDisplay orig, global::RoR2.CharacterModel self, EquipmentIndex newEquipmentIndex)
        {
            orig(self, newEquipmentIndex);
            if (newEquipmentIndex != EquipmentIndex.None && HighlightEquipment == true)
            {
                GameObject Highlight = HighlightOrangeItem;
                if (EquipmentCatalog.GetEquipmentDef(newEquipmentIndex).isLunar == true)
                {
                    Highlight = HighlightOrangeLunarItem;
                }
                else if (EquipmentCatalog.GetEquipmentDef(newEquipmentIndex).isBoss == true)
                {
                    Highlight = HighlightOrangeBossItem;
                }
                List<GameObject> tempList = self.GetEquipmentDisplayObjects(newEquipmentIndex);
                for (int i = 0; i < tempList.Count; i++)
                {
                    Renderer renderer = tempList[i].GetComponentInChildren<Renderer>();
                    if (renderer)
                    {
                        HighlightRect.CreateHighlight(self.body.gameObject, renderer, Highlight, -1, false);
                    }
                    else
                    {
                        renderer = tempList[i].GetComponent<Renderer>();
                        if (renderer)
                        {
                            HighlightRect.CreateHighlight(self.body.gameObject, renderer, Highlight, -1, false);
                        }
                    }

                };
            }
            HighlightEquipment = false;
        }



    }

}
