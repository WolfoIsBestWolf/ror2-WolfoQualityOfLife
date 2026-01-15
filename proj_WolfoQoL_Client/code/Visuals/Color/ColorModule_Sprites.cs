using RoR2;
using UnityEngine;
using WolfoLibrary;

namespace WolfoQoL_Client
{
    public static partial class ColorModule
    {
 
        public static void NewColorOutlineIcons()
        {
            RoR2Content.Equipment.AffixRed.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteFire.png");
            RoR2Content.Equipment.AffixBlue.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteLightning.png");
            RoR2Content.Equipment.AffixWhite.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteIce.png");
            RoR2Content.Equipment.AffixPoison.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/ElitePoison.png");
            RoR2Content.Equipment.AffixHaunted.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteHaunted.png");
            RoR2Content.Equipment.AffixLunar.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteLunar.png");

            MissedContent.Equipment.EliteEarthEquipment.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteEarth.png");
            DLC1Content.Equipment.EliteVoidEquipment.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteVoid.png");
            MissedContent.Equipment.EliteSecretSpeedEquipment.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteSecret.png");

            DLC2Content.Equipment.EliteAurelioniteEquipment.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteAurelionite.png");
            DLC2Content.Equipment.EliteBeadEquipment.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteBead.png");

            DLC3Content.Equipment.EliteCollectiveEquipment.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/EliteCollective.png");

            RoR2Content.Equipment.CrippleWard.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/LunarEffigy.png");
            RoR2Content.Equipment.Meteor.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/LunarMeteor.png");
            RoR2Content.Equipment.BurnNearby.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/LunarHelfire.png");
            RoR2Content.Equipment.Tonic.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/LunarPotion.png");

            DLC1Content.Equipment.LunarPortalOnUse.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/LunarPortal.png");

            #region Junk

            JunkContent.Items.CritHeal.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/Beta/betaCorpse.png");
            RoR2Content.Items.CrippleWardOnLevel.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/Beta/betaEffigy.png");
            JunkContent.Items.BurnNearby.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/Beta/betaHelfire.png");
            JunkContent.Items.TempestOnKill.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/Beta/betaTempest.png");
            JunkContent.Items.WarCryOnCombat.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/Beta/betaPauldron.png");
            JunkContent.Items.CooldownOnCrit.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/Beta/betaWicked.png");
            #endregion
        }

        public static void ModSupport()
        {

            /*EquipmentDef tempModDef;
            tempModDef = EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_AFFIXARAGONITE"));
            if (tempModDef != null)
            {
                tempModDef.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/ColorChanger/Modded/texAffixRaging.png");
            }

            tempModDef = EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_AFFIXVEILED"));
            if (tempModDef != null)
            {
                Sprite texture = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/ColorChanger/Modded/texAffixCloakIcon.png");
                tempModDef.pickupIconSprite = textureS;
            }

            tempModDef = EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_AFFIXWARPED"));
            if (tempModDef != null)
            {
                Sprite texture = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/ColorChanger/Modded/texAffixGravityIcon.png");
                tempModDef.pickupIconSprite = textureS;
            }

            tempModDef = EquipmentCatalog.GetEquipmentDef(EquipmentCatalog.FindEquipmentIndex("EQUIPMENT_AFFIXPLATED"));
            if (tempModDef != null)
            {
                Sprite texture = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/ColorChanger/Modded/texAffixPlatedIcon.png");
                tempModDef.pickupIconSprite = textureS;
            }*/
        }
    }

}
