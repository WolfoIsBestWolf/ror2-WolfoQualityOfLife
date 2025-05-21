using R2API;
using RoR2;
using RoR2.ExpansionManagement;
//using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.UI;
using RoR2.UI.LogBook;

namespace WolfoQoL_Client
{
    public class ColorModule_Sprites
    {
          
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
            Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Junk/CooldownOnCrit/CooldownOnCrit.asset").WaitForCompletion().pickupIconSprite = WickedRing;

            Texture2D betaCorpse = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaCorpse.png");
            betaCorpse.wrapMode = TextureWrapMode.Clamp;
            Sprite betaCorpseS = Sprite.Create(betaCorpse, v.rec128, v.half);
            Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Junk/CritHeal/CritHeal.asset").WaitForCompletion().pickupIconSprite = betaCorpseS;

            Texture2D betaEffigy = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaEffigy.png");
            betaEffigy.wrapMode = TextureWrapMode.Clamp;
            Sprite betaEffigyS = Sprite.Create(betaEffigy, v.rec128, v.half);
            Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Base/CrippleWardOnLevel/CrippleWardOnLevel.asset").WaitForCompletion().pickupIconSprite = betaEffigyS;

            Texture2D betaHelfire = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaHelfire.png");
            betaHelfire.wrapMode = TextureWrapMode.Clamp;
            Sprite betaHelfireS = Sprite.Create(betaHelfire, v.rec128, v.half);
            Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Junk/BurnNearby/BurnNearby.asset").WaitForCompletion().pickupIconSprite = betaHelfireS;

            Texture2D betaPauldron = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaPauldron.png");
            betaPauldron.wrapMode = TextureWrapMode.Clamp;
            Sprite betaPauldronS = Sprite.Create(betaPauldron, v.rec128, v.half);
            Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Junk/WarCryOnCombat/WarCryOnCombat.asset").WaitForCompletion().pickupIconSprite = betaPauldronS;

            Texture2D betaTempest = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/betaTempest.png");
            betaTempest.wrapMode = TextureWrapMode.Clamp;
            Sprite betaTempestS = Sprite.Create(betaTempest, v.rec128, v.half);
            Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Junk/TempestOnKill/TempestOnKill.asset").WaitForCompletion().pickupIconSprite = betaTempestS;




        }

        public static void ModSupport()
        {

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
