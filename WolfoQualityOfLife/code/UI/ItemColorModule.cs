using R2API.Utils;
using RoR2;
using RoR2.ExpansionManagement;
//using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using R2API;

namespace WolfoQualityOfLife
{
    public class ItemColorModule
    {
        //Adding missing Highlights
        public static readonly GameObject HighlightYellowItem = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightBossItem", false);
        public static readonly GameObject HighlightPinkT1Item = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier1Item"), "HighlightVoidT1Item", false);
        public static readonly GameObject HighlightPinkT2Item = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightVoidT2Item", false);
        public static readonly GameObject HighlightPinkT3Item = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier3Item"), "HighlightVoidT3Item", false);
        public static readonly GameObject HighlightOrangeItem = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipment", false);
        public static readonly GameObject HighlightOrangeBossItem = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipmentBoss", false);
        public static readonly GameObject HighlightOrangeLunarItem = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightTier2Item"), "HighlightEquipmentLunar", false);
        //public static int HighlighterIntChoice = 0;
        public static bool HighlightEquipment = false;

        //public static bool ChangedColors = false;

        public static readonly GameObject EquipmentBossOrb = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/BossOrb"), "EquipmentBossOrb", false);
        public static readonly GameObject EquipmentLunarOrb = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/LunarOrb"), "EquipmentLunarOrb", false);
        public static readonly GameObject NoTierOrb = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/Tier1Orb"), "NoTierOrb", false);
        public static readonly GameObject CoinOrb = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/Tier1Orb"), "CoinOrb", false);

        public static Color NewSurvivorLogbookNameColor = new Color32(80, 130, 173, 255);

        public static Color ColorLunarEquip;
        public static Color ColorBossEquip;

        public static Color ColorVoidWhite;
        //public static Color CustomVoidGreen 
        public static Color ColorVoidRed;
        public static Color ColorVoidYellow;

        public static Color ColorVoidDarkWhite;
        //public static Color CustomVoidDarkGreen
        public static Color ColorVoidDarkRed;
        public static Color ColorVoidDarkYellow;


        public static Texture2D texEquipmentBossBG = new Texture2D(512, 512, TextureFormat.DXT5, false);
        public static Texture2D texEquipmentLunarBG = new Texture2D(512, 512, TextureFormat.DXT5, false);

        public static void Main()
        {
            //Bro ColorAPI don't do shit
            OrbMaker();
            ColorUtility.TryParseHtmlString("#78AFFF", out ColorLunarEquip);
            ColorUtility.TryParseHtmlString("#FFC211", out ColorBossEquip);

            //Void Green stays the default
            ColorUtility.TryParseHtmlString("#FF9EEC", out ColorVoidWhite);
            ColorUtility.TryParseHtmlString("#FF73BF", out ColorVoidRed); //1 0.45 0.75 1
            ColorUtility.TryParseHtmlString("#E658A6", out ColorVoidYellow);

            ColorUtility.TryParseHtmlString("#B065A1", out ColorVoidDarkWhite);
            ColorUtility.TryParseHtmlString("#B24681", out ColorVoidDarkRed);
            ColorUtility.TryParseHtmlString("#A13470", out ColorVoidDarkYellow);



            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += ItemColorModule.ChangeEquipmentBGLogbook;
            On.RoR2.UI.LogBook.LogBookController.BuildSurvivorEntries += ItemColorModule.ChangeSurvivorLogbookEntry;

            texEquipmentBossBG = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/texEquipmentBossBG.png");
            texEquipmentLunarBG = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/texEquipmentLunarBG.png");



            On.RoR2.UI.GenericNotification.SetItem += PickupNotifColorOverrideItems; ; //Notification Title
            On.RoR2.UI.GenericNotification.SetEquipment += PickupNotifColorOverrideEquip; //
            if (WConfig.EnableColorChangeModule.Value == true)
            {
                On.RoR2.UI.EquipmentIcon.Update += UIEquipmentIconColorChanger; //When you hover over it I guess
                On.RoR2.PickupPickerController.OnDisplayBegin += PickupPickerController_OnDisplayBegin; //Command I think?
            }


            GameObject GoldFragmentPotential = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/FragmentPotentialPickup.prefab").WaitForCompletion();
            GoldFragmentPotential.GetComponent<Highlight>().highlightColor = Highlight.HighlightColor.custom;
            GoldFragmentPotential.GetComponent<Highlight>().CustomColor = new Color(0.9f, 0.8f, 0.4f);
        }

        private static void PickupNotifColorOverrideItems(On.RoR2.UI.GenericNotification.orig_SetItem orig, RoR2.UI.GenericNotification self, ItemDef itemDef)
        {
            orig(self, itemDef);
            self.titleTMP.color = PickupCatalog.FindPickupIndex(itemDef.itemIndex).pickupDef.baseColor;
        }

        public static void PickupNotifColorOverrideEquip(On.RoR2.UI.GenericNotification.orig_SetEquipment orig, RoR2.UI.GenericNotification self, global::RoR2.EquipmentDef equipmentDef)
        {
            orig(self, equipmentDef);
            self.titleTMP.color = PickupCatalog.FindPickupIndex(equipmentDef.equipmentIndex).pickupDef.baseColor;
        }

        public static void EquipmentColorIconChanger()
        {

            Texture2D OutlineChangedtexAffixBlueIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexAffixBlueIcon.png");
            OutlineChangedtexAffixBlueIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixBlueIconS = Sprite.Create(OutlineChangedtexAffixBlueIcon, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixBlue").pickupIconSprite = OutlineChangedtexAffixBlueIconS;

            Texture2D OutlineChangedtexAffixHauntedIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexAffixHauntedIcon.png");
            OutlineChangedtexAffixHauntedIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixHauntedIconS = Sprite.Create(OutlineChangedtexAffixHauntedIcon, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixHaunted").pickupIconSprite = OutlineChangedtexAffixHauntedIconS;





            Texture2D OutlineChangedtexAffixLunarIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexAffixLunarIcon.png");
            OutlineChangedtexAffixLunarIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixLunarIconS = Sprite.Create(OutlineChangedtexAffixLunarIcon, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").pickupIconSprite = OutlineChangedtexAffixLunarIconS;


            Texture2D OutlineChangedtexAffixPoisonIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexAffixPoisonIcon.png");
            OutlineChangedtexAffixPoisonIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixPoisonIconS = Sprite.Create(OutlineChangedtexAffixPoisonIcon, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixPoison").pickupIconSprite = OutlineChangedtexAffixPoisonIconS;

            Texture2D OutlineChangedtexAffixRedIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexAffixRedIcon.png");
            OutlineChangedtexAffixRedIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixRedIconS = Sprite.Create(OutlineChangedtexAffixRedIcon, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixRed").pickupIconSprite = OutlineChangedtexAffixRedIconS;

            Texture2D OutlineChangedtexAffixGreenIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexAffixGreenIcon.png");
            OutlineChangedtexAffixGreenIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixGreenIconS = Sprite.Create(OutlineChangedtexAffixGreenIcon, v.rec128, v.half);
            Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteEarth/EliteEarthEquipment.asset").WaitForCompletion().pickupIconSprite = OutlineChangedtexAffixGreenIconS;

            Texture2D OutlineChangedtexAffixWhiteIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexAffixWhiteIcon.png");
            OutlineChangedtexAffixWhiteIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexAffixWhiteIconS = Sprite.Create(OutlineChangedtexAffixWhiteIcon, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixWhite").pickupIconSprite = OutlineChangedtexAffixWhiteIconS;



            Texture2D texAffixBeadIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/texAffixBeadIcon.png");
            texAffixBeadIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite texAffixBeadIconS = Sprite.Create(texAffixBeadIcon, v.rec128, v.half);
            Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC2/Elites/EliteBead/EliteBeadEquipment.asset").WaitForCompletion().pickupIconSprite = texAffixBeadIconS;

            Texture2D texAffixAurelioniteIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/texAffixAurelioniteIcon.png");
            texAffixAurelioniteIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite texAffixAurelioniteIconS = Sprite.Create(texAffixAurelioniteIcon, v.rec128, v.half);
            Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC2/Elites/EliteAurelionite/EliteAurelioniteEquipment.asset").WaitForCompletion().pickupIconSprite = texAffixAurelioniteIconS;



            //Lunar Equipment
            Texture2D OutlineChangedtexEffigyIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexEffigyIcon.png");
            OutlineChangedtexEffigyIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexEffigyIconS = Sprite.Create(OutlineChangedtexEffigyIcon, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/CrippleWard").pickupIconSprite = OutlineChangedtexEffigyIconS;

            Texture2D OutlineChangedtexPotionIconChanged = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexPotionIconChanged.png");
            OutlineChangedtexPotionIconChanged.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexPotionIconChangedS = Sprite.Create(OutlineChangedtexPotionIconChanged, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/BurnNearby").pickupIconSprite = OutlineChangedtexPotionIconChangedS;

            Texture2D OutlineChangedtexMeteorIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexMeteorIcon.png");
            OutlineChangedtexMeteorIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexMeteorIconS = Sprite.Create(OutlineChangedtexMeteorIcon, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/Meteor").pickupIconSprite = OutlineChangedtexMeteorIconS;

            Texture2D OutlineChangedtexTonicIcontonic = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/OutlineChangedtexTonicIcontonic.png");
            OutlineChangedtexTonicIcontonic.wrapMode = TextureWrapMode.Clamp;
            Sprite OutlineChangedtexTonicIcontonicS = Sprite.Create(OutlineChangedtexTonicIcontonic, v.rec128, v.half);
            LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/Tonic").pickupIconSprite = OutlineChangedtexTonicIcontonicS;


            Texture2D texLunarPortalOnUseIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/texLunarPortalOnUseIcon.png");
            texLunarPortalOnUseIcon.wrapMode = TextureWrapMode.Clamp;
            Sprite texLunarPortalOnUseIconS = Sprite.Create(texLunarPortalOnUseIcon, v.rec128, v.half);
            Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/LunarPortalOnUse/LunarPortalOnUse.asset").WaitForCompletion().pickupIconSprite = texLunarPortalOnUseIconS;

        }

        public static void ItemIcons()
        {
            Texture2D WickedRingTex = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaWicked.png");
            WickedRingTex.wrapMode = TextureWrapMode.Clamp;
            Sprite WickedRing = Sprite.Create(WickedRingTex, v.rec128, v.half);
            JunkContent.Items.CooldownOnCrit.pickupIconSprite = WickedRing;

            Texture2D betaCorpse = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaCorpse.png");
            betaCorpse.wrapMode = TextureWrapMode.Clamp;
            Sprite betaCorpseS = Sprite.Create(betaCorpse, v.rec128, v.half);
            JunkContent.Items.CritHeal.pickupIconSprite = betaCorpseS;

            Texture2D betaEffigy = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaEffigy.png");
            betaEffigy.wrapMode = TextureWrapMode.Clamp;
            Sprite betaEffigyS = Sprite.Create(betaEffigy, v.rec128, v.half);
            RoR2Content.Items.CrippleWardOnLevel.pickupIconSprite = betaEffigyS;

            Texture2D betaHelfire = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaHelfire.png");
            betaHelfire.wrapMode = TextureWrapMode.Clamp;
            Sprite betaHelfireS = Sprite.Create(betaHelfire, v.rec128, v.half);
            JunkContent.Items.BurnNearby.pickupIconSprite = betaHelfireS;

            Texture2D betaPauldron = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaPauldron.png");
            betaPauldron.wrapMode = TextureWrapMode.Clamp;
            Sprite betaPauldronS = Sprite.Create(betaPauldron, v.rec128, v.half);
            JunkContent.Items.WarCryOnCombat.pickupIconSprite = betaPauldronS;

            Texture2D betaTempest = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaTempest.png");
            betaTempest.wrapMode = TextureWrapMode.Clamp;
            Sprite betaTempestS = Sprite.Create(betaTempest, v.rec128, v.half);
            JunkContent.Items.TempestOnKill.pickupIconSprite = betaTempestS;

            RoR2Content.Items.AdaptiveArmor.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;
            RoR2Content.Items.BoostEquipmentRecharge.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;
            JunkContent.Equipment.Enigma.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;


        }

        public static void OrbMaker()
        {

            EquipmentBossOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color32(232, 185, 70, 0);
            EquipmentBossOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color32(166, 112, 38, 255);
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].SetFieldValue<Color>("color", new Color32(234, 153, 6, 255)); //Core Start
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(234, 136, 6, 255); //Core End
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color32(255, 189, 0, 255); //PulseGlow Start
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(255, 189, 0, 255); //PulseGlow End
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].SetFieldValue<Color>("color", new Color32(234, 153, 6, 255)); //Core Start
            EquipmentBossOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(234, 136, 6, 255); //Core End
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color32(255, 189, 0, 255); //PulseGlow Start
            EquipmentBossOrb.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(255, 189, 0, 255); //PulseGlow End

            EquipmentBossOrb.transform.GetChild(0).GetChild(2).GetComponent<Light>().color = new Color32(255, 192, 74, 255);

            EquipmentLunarOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color32(43, 46, 232, 0);
            EquipmentLunarOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color32(26, 29, 225, 255);

            GradientColorKey LunarC1 = new GradientColorKey
            {
                color = new Color32(91, 101, 226, 255),
                time = 0
            };
            GradientColorKey LunarC2 = new GradientColorKey
            {
                color = new Color32(91, 101, 226, 255),
                time = 1
            };
            GradientColorKey[] sex = new GradientColorKey[1];
            sex = sex.Add(LunarC1, LunarC2);

            //EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.SetKeys(sex, EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.alphaKeys);

            //EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys = sex;
            //EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys = sex;


            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color32(91, 101, 226, 255); //Core
            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(91, 101, 226, 255); //Core
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color32(43, 110, 225, 255); //PulseGlow
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(43, 110, 225, 255); //PulseGlow
            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color32(91, 101, 226, 255); //Core
            EquipmentLunarOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(91, 101, 226, 255); //Core
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color32(43, 110, 225, 255); //PulseGlow
            EquipmentLunarOrb.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(43, 110, 225, 255); //PulseGlow

            GameObject VEquipmentOrb = LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/EquipmentOrb");
            Object.Instantiate(VEquipmentOrb.transform.GetChild(0).GetChild(2).gameObject, EquipmentBossOrb.transform.GetChild(0));
            Object.Instantiate(VEquipmentOrb.transform.GetChild(0).GetChild(2).gameObject, EquipmentLunarOrb.transform.GetChild(0));




            EquipmentLunarOrb.transform.GetChild(0).GetChild(1).GetComponent<Light>().color = new Color32(114, 112, 232, 255);

            NoTierOrb.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            NoTierOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color(0f, 0f, 0f, 0);
            NoTierOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color(0.5f, 0.5f, 0.5f, 1);
            NoTierOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color(0.5f, 0.5f, 0.5f, 1); //Core Start
            NoTierOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color(0.3f, 0.3f, 0.3f, 1); //Core End
            NoTierOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color(0.5f, 0.5f, 0.5f, 1); //Core Start
            NoTierOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color(0.3f, 0.3f, 0.3f, 1); //Core End

            NoTierOrb.transform.GetChild(0).GetChild(1).GetComponent<Light>().color = new Color(0.5f, 0.5f, 0.5f, 1);

            CoinOrb.transform.GetChild(0).GetComponent<TrailRenderer>().startColor = new Color32(198, 173, 250, 0);
            CoinOrb.transform.GetChild(0).GetComponent<TrailRenderer>().endColor = new Color32(168, 147, 212, 255);
            CoinOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[0].color = new Color32(198, 173, 250, 255); //Core Start
            CoinOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradientMax.colorKeys[1].color = new Color32(198, 173, 250, 255); //Core End
            CoinOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[0].color = new Color32(198, 173, 250, 255); //Core Start
            CoinOrb.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1].color = new Color32(198, 173, 250, 255); //Core End

            CoinOrb.transform.GetChild(0).GetChild(1).GetComponent<Light>().color = new Color32(210, 178, 255, 255);

        }

        public static RoR2.UI.LogBook.Entry[] ChangeEquipmentBGLogbook(On.RoR2.UI.LogBook.LogBookController.orig_BuildPickupEntries orig, Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            RoR2.UI.LogBook.Entry[] array = orig(expansionAvailability);
            
            //Sorting Boss Equips after Boss items ripped from some Boss Equip mod
            bool bossTierFound = false;
            int num = -1;
            /*for (int i = 0; i < array.Length; i++)
            {
                bool bossTierItem = false;
                PickupDef pickupDef = ((PickupIndex)array[i].extraData).pickupDef;
                ItemIndex itemIndex = pickupDef.itemIndex;
                if (itemIndex != ItemIndex.None)
                {
                    ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);
                    bool flag4 = itemDef && itemDef.tier == ItemTier.Boss;
                    if (flag4)
                    {
                        bossTierItem = true;
                    }
                }
                if (bossTierItem)
                {
                    //base.Logger.LogInfo("Found boss item start");
                    bossTierFound = true;
                }
                if (bossTierFound && !bossTierItem && num == -1)
                {
                    //base.Logger.LogInfo("Found boss item end");
                    num = i;
                }
            }*/

            bool sortAtEnd = true;

            List<RoR2.UI.LogBook.Entry> list = new List<RoR2.UI.LogBook.Entry>();
           

            for (int j = 0; j < array.Length; j++)
            {
                if (!list.Contains(array[j]))
                {
                    PickupDef pickupDef2 = ((PickupIndex)array[j].extraData).pickupDef;
                    EquipmentIndex equipmentIndex = pickupDef2.equipmentIndex;
                    if (equipmentIndex != EquipmentIndex.None)
                    {
                        EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
                        //Debug.Log("Found Equipment : " + equipmentDef.name);
                        if (equipmentDef && equipmentDef.isBoss)
                        {
                            //Debug.LogWarning("Found Boss Equipment : " + equipmentDef.name);
                            RoR2.UI.LogBook.Entry entry = array[j];
                            list.Add(array[j]);

                            HG.ArrayUtils.ArrayRemoveAtAndResize<RoR2.UI.LogBook.Entry>(ref array, j, 1);
                            if (sortAtEnd || num == -1)
                            {
                                num = array.Length;
                                j--;
                            }
                            HG.ArrayUtils.ArrayInsert<RoR2.UI.LogBook.Entry>(ref array, num, entry);
                            num++;  
                        }
                    }
                }
            }
            
            //Custom BG
            for (int i = 0; i < array.Length; i++)
            {
                PickupIndex tempind = PickupCatalog.FindPickupIndex(array[i].extraData.ToString());
                PickupDef temppickdef = PickupCatalog.GetPickupDef(tempind);

                if (temppickdef.equipmentIndex != EquipmentIndex.None)
                {
                    EquipmentDef tempeqdef = EquipmentCatalog.GetEquipmentDef(temppickdef.equipmentIndex);

                    if (tempeqdef.isBoss == true)
                    {
                        array[i].bgTexture = texEquipmentBossBG;
                        array[i].color = ItemColorModule.ColorBossEquip;
                    }
                    else if (tempeqdef.isLunar == true)
                    {
                        array[i].bgTexture = texEquipmentLunarBG;
                        array[i].color = ItemColorModule.ColorLunarEquip;
                    }

                }
                //Debug.LogWarning(array[i].nameToken + " " + array[i].extraData);
            }

            return array;
        }


        public static RoR2.UI.LogBook.Entry[] ChangeSurvivorLogbookEntry(On.RoR2.UI.LogBook.LogBookController.orig_BuildSurvivorEntries orig, Dictionary<ExpansionDef, bool> expansionAvailability)
        {
            RoR2.UI.LogBook.Entry[] array = orig(expansionAvailability);

            for (int i = 0; i < array.Length; i++)
            {
                array[i].color = NewSurvivorLogbookNameColor;

                //Debug.LogWarning(array[i].nameToken + " " + array[i].extraData);
            }
            return array;
        }

        public static void ChangeColors()
        {
            Debug.Log("WolfoQoL : Changing Colors");


            PickupCatalog.FindPickupIndex(ItemTier.VoidTier1).pickupDef.baseColor = ColorVoidWhite;
            PickupCatalog.FindPickupIndex(ItemTier.VoidTier3).pickupDef.baseColor = ColorVoidRed;
            PickupCatalog.FindPickupIndex(ItemTier.VoidBoss).pickupDef.baseColor = ColorVoidYellow;

            //Void Coins already use their own color
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).baseColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin);
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("LunarCoin.Coin0")).darkColor = ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarCoin);

            Color YellowGreen = new Color(0.9f, 1f, 0.1f, 1);
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(RoR2Content.Items.DrizzlePlayerHelper.itemIndex)).baseColor = YellowGreen;
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(RoR2Content.Items.DrizzlePlayerHelper.itemIndex)).darkColor = YellowGreen;


            int TotalItemCount = ItemCatalog.itemCount;
            int TotalEquipmentCount = EquipmentCatalog.equipmentCount;

            for (int i = 0; i < TotalItemCount; i++)
            {
                string tempname = ItemCatalog.GetItemDef((ItemIndex)i).name;
                string tempindexname = ("ItemIndex." + tempname);
                PickupDef tempPickupDef = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname));

                if (ItemCatalog.GetItemDef((ItemIndex)i).tier == ItemTier.NoTier)
                {
                    tempPickupDef.dropletDisplayPrefab = NoTierOrb;
                }
                else if (ItemCatalog.GetItemDef((ItemIndex)i).tier == ItemTier.VoidTier1)
                {
                    tempPickupDef.baseColor = ColorVoidWhite;
                    tempPickupDef.darkColor = ColorVoidDarkWhite;
                }
                else if (ItemCatalog.GetItemDef((ItemIndex)i).tier == ItemTier.VoidTier3)
                {
                    tempPickupDef.baseColor = ColorVoidRed;
                    tempPickupDef.darkColor = ColorVoidDarkRed;
                }
                else if (ItemCatalog.GetItemDef((ItemIndex)i).tier == ItemTier.VoidBoss)
                {
                    tempPickupDef.baseColor = ColorVoidYellow;
                    tempPickupDef.darkColor = ColorVoidDarkYellow;
                }
            }

            for (int i = 0; i < TotalEquipmentCount; i++)
            {
                EquipmentDef tempequipdef = EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i);
                string tempname = tempequipdef.name;
                string tempindexname = ("EquipmentIndex." + tempname);

                PickupDef tempPickupDef = PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(tempindexname));

                if (tempequipdef.passiveBuffDef && tempequipdef.passiveBuffDef.isElite)
                {
                    if (WConfig.EnableColorChangeModule.Value == true && !tempequipdef.isLunar)
                    {
                        tempequipdef.colorIndex = ColorCatalog.ColorIndex.BossItem;
                        tempPickupDef.isBoss = true;
                    }
                    else if (tempequipdef.isLunar == true)
                    {
                        tempequipdef.colorIndex = ColorCatalog.ColorIndex.LunarItem;
                        tempPickupDef.isBoss = true;
                        tempPickupDef.isLunar = true;
                    }
                }
                if (tempequipdef.isLunar == true)
                {
                    tempPickupDef.isLunar = true;
                    tempPickupDef.baseColor = ItemColorModule.ColorLunarEquip;
                    tempPickupDef.darkColor = ItemColorModule.ColorLunarEquip;
                    tempPickupDef.dropletDisplayPrefab = ItemColorModule.EquipmentLunarOrb;
                }
                else if (tempequipdef.isBoss == true)
                {
                    tempPickupDef.isBoss = true;
                    tempPickupDef.baseColor = ItemColorModule.ColorBossEquip;
                    tempPickupDef.darkColor = ItemColorModule.ColorBossEquip;
                    tempPickupDef.dropletDisplayPrefab = ItemColorModule.EquipmentBossOrb;
                }



            }


        }

        public static void AddMissingItemHighlights()
        {
            GameObject HighlightBlueItem = LegacyResourcesAPI.Load<GameObject>("Prefabs/ui/HighlightLunarItem");
            HighlightBlueItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color32(55, 101, 255, 255);//new Color(0.3f, 0.6f, 1, 1);

            HighlightYellowItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.9373f, 0.2667f, 1);
            HighlightPinkT1Item.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.498f, 0.9059f, 1);
            HighlightPinkT2Item.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.498f, 0.9059f, 1);
            HighlightPinkT3Item.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.498f, 0.9059f, 1);
            HighlightOrangeItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1f, 0.6471f, 0.298f, 1);
            HighlightOrangeBossItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = new Color(1, 0.75f, 0f, 1);
            HighlightOrangeLunarItem.GetComponent<RoR2.UI.HighlightRect>().highlightColor = ColorLunarEquip;


            ItemTierCatalog.GetItemTierDef(ItemTier.Boss).highlightPrefab = HighlightYellowItem;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier1).highlightPrefab = HighlightPinkT1Item;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier2).highlightPrefab = HighlightPinkT2Item;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidTier3).highlightPrefab = HighlightPinkT3Item;
            ItemTierCatalog.GetItemTierDef(ItemTier.VoidBoss).highlightPrefab = HighlightPinkT3Item;


            On.RoR2.Inventory.SetEquipmentIndex += (orig, self, equipmentIndex) =>
            {
                //self.gameObject.GetComponent<RoR2.PlayerCharacterMasterController>() && equipmentIndex != self.currentEquipmentIndex && 
                if (!RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.enigmaArtifactDef))
                {
                    HighlightEquipment = true;
                    //body.modelLocator.modelTransform.gameObject.GetComponent<CharacterModel>().UpdateItemDisplay(inventory);
                    //Debug.LogWarning("Pickup " + equipmentIndex);
                }
                orig(self, equipmentIndex);
            };
            On.RoR2.CharacterModel.SetEquipmentDisplay += EquipmentHighlighter;
        }

        public static void EquipmentHighlighter(On.RoR2.CharacterModel.orig_SetEquipmentDisplay orig, global::RoR2.CharacterModel self, EquipmentIndex newEquipmentIndex)
        {
            orig(self, newEquipmentIndex);
            //Debug.Log("Equipment Highlighter");
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
                        RoR2.UI.HighlightRect.CreateHighlight(self.body.gameObject, renderer, Highlight, -1, false);
                    }
                    else
                    {
                        renderer = tempList[i].GetComponent<Renderer>();
                        if (renderer)
                        {
                            RoR2.UI.HighlightRect.CreateHighlight(self.body.gameObject, renderer, Highlight, -1, false);
                        }
                    }

                };
            }
            HighlightEquipment = false;
        }


        public static void PickupPickerController_OnDisplayBegin(On.RoR2.PickupPickerController.orig_OnDisplayBegin orig, PickupPickerController self, NetworkUIPromptController networkUIPromptController, LocalUser localUser, CameraRigController cameraRigController)
        {
            orig(self, networkUIPromptController, localUser, cameraRigController);

            RoR2.UI.PickupPickerPanel temppannel = self.panelInstanceController;
            for (int i = 1; i < temppannel.buttonContainer.childCount; i++)
            {
                var temp = temppannel.buttonContainer.GetChild(i).GetComponent<RoR2.UI.TooltipProvider>();
                //Debug.LogWarning(temp);
                if (temp)
                {
                    if (temp.titleToken.StartsWith("EQUIPMENT_"))
                    {
                        if (temp.titleColor.r == 1 && temp.titleColor.g > 0.9f)
                        {
                            temp.titleColor = ItemColorModule.ColorBossEquip;
                        }
                        else if (temp.titleColor.b == 1)
                        {
                            temp.titleColor = ItemColorModule.ColorLunarEquip;
                        }
                    }
                }
            }
        }


        public static void UIEquipmentIconColorChanger(On.RoR2.UI.EquipmentIcon.orig_Update orig, global::RoR2.UI.EquipmentIcon self)
        {
            orig(self);

            if (!self.targetInventory) { return; }
            if (self.targetInventory.currentEquipmentIndex != EquipmentIndex.None)
            {
                if (self.targetInventory.currentEquipmentState.equipmentDef.isLunar == true)
                {
                    self.tooltipProvider.titleColor = ItemColorModule.ColorLunarEquip;
                }
                else if (self.targetInventory.currentEquipmentState.equipmentDef.isBoss == true)
                {
                    self.tooltipProvider.titleColor = ItemColorModule.ColorBossEquip;
                }
            }
            //if (!self.displayAlternateEquipment) { return; }
            if (self.targetInventory.alternateEquipmentIndex != EquipmentIndex.None)
            {
                /*
                if (self.displayAlternateEquipment)
                {
                    self.displayRoot.SetActive(true);
                }
                */
                if (self.targetInventory.alternateEquipmentState.equipmentDef.isLunar == true)
                {
                    self.tooltipProvider.titleColor = ItemColorModule.ColorLunarEquip;
                }
                else if (self.targetInventory.alternateEquipmentState.equipmentDef.isBoss == true)
                {
                    self.tooltipProvider.titleColor = ItemColorModule.ColorBossEquip;
                }
            }

        }


        public static void ModSupport()
        {
            //Late Running Method
            ItemColorModule.EquipmentColorIconChanger();
            ItemColorModule.ItemIcons();

            EquipmentDef tempModDef;
            tempModDef = EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_AFFIXARAGONITE"));
            if (tempModDef != null)
            {
                Texture2D texture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/Modded/texAffixRaging.png");
                texture.wrapMode = TextureWrapMode.Clamp;
                Sprite textureS = Sprite.Create(texture, v.rec128, v.half);
                tempModDef.pickupIconSprite = textureS;
            }

            tempModDef = EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_AFFIXVEILED"));
            if (tempModDef != null)
            {
                Texture2D texture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/Modded/texAffixCloakIcon.png");
                texture.wrapMode = TextureWrapMode.Clamp;
                Sprite textureS = Sprite.Create(texture, v.rec128, v.half);
                tempModDef.pickupIconSprite = textureS;
            }

            tempModDef = EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_AFFIXWARPED"));
            if (tempModDef != null)
            {
                Texture2D texture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/Modded/texAffixGravityIcon.png");
                texture.wrapMode = TextureWrapMode.Clamp;
                Sprite textureS = Sprite.Create(texture, v.rec128, v.half);
                tempModDef.pickupIconSprite = textureS;
            }

            tempModDef = EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_AFFIXPLATED"));
            if (tempModDef != null)
            {
                Texture2D texture = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/ColorChanger/Modded/texAffixPlatedIcon.png");
                texture.wrapMode = TextureWrapMode.Clamp;
                Sprite textureS = Sprite.Create(texture, v.rec128, v.half);
                tempModDef.pickupIconSprite = textureS;
            }
        }
    }

}
