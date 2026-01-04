using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client
{

    public static class BuffIcons
    {

        public static void BuffColorChanger()
        {
            LegacyResourcesAPI.Load<BuffDef>("buffdefs/AffixHauntedRecipient").buffColor = new Color32(148, 215, 214, 255); //94D7D6 Celestine Elite
            LegacyResourcesAPI.Load<BuffDef>("buffdefs/SmallArmorBoost").buffColor = new Color(0.6784f, 0.6118f, 0.4118f, 1f);
            LegacyResourcesAPI.Load<BuffDef>("buffdefs/WhipBoost").buffColor = new Color32(245, 158, 73, 255); //E8813D
        }

        public static void Awake()
        {
            if (WConfig.cfgBuff_RepeatColors.Value == true)
            {
                BuffColorChanger();
            }

            BuffDef RainCoat = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC1/ImmuneToDebuff/bdImmuneToDebuffReady.asset").WaitForCompletion();
            if (RainCoat.iconSprite.name.StartsWith("texBuffImmune"))
            {
                RainCoat.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffRaincoat.png");
            }

            BuffDef bdBeadArmor = Addressables.LoadAssetAsync<BuffDef>(key: "64016639a97b88f43b182dfebe9fee92").WaitForCompletion();
            bdBeadArmor.isHidden = false;
            bdBeadArmor.canStack = false;

            BuffDef TeleportOnLowHealthActive = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC2/Items/TeleportOnLowHealth/bdTeleportOnLowHealthActive.asset").WaitForCompletion();
            TeleportOnLowHealthActive.isHidden = false;
            TeleportOnLowHealthActive.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffTeleportOnLowHealthIcon.png");


            BuffDef bdJailed = Addressables.LoadAssetAsync<BuffDef>(key: "048bb7e2972c1ee47a80f56c8283ff48").WaitForCompletion();
            bdJailed.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffJailed.png");
            bdJailed.buffColor = Color.white;//0.8302 0.6986 0.0744 1

        }

    }

}
