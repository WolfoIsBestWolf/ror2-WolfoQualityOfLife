using EntityStates;
using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client
{

    public class BuffIcons
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


            bdFrozen = ScriptableObject.CreateInstance<BuffDef>();
            bdFrozen.iconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Buffs/texBuffBrokenCube.png");
            bdFrozen.buffColor = new Color32(184, 216, 239, 255); //CFDBE0 // B8D8EF
            bdFrozen.name = "wqol_Frozen";
            bdFrozen.isDebuff = false;
            bdFrozen.canStack = true;
            bdFrozen.ignoreGrowthNectar = true;

        }


        public static void Start()
        {
            bool riskyModEnabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.RiskyLives.RiskyMod");
            if (!WolfoMain.ServerModInstalled && WConfig.cfgFreezeTimer.Value && !riskyModEnabled)
            {
                On.RoR2.UI.BuffDisplay.AllocateIcons += BuffDisplay_AllocateIcons;

                On.EntityStates.FrozenState.OnEnter += (orig, self) =>
                {
                    orig(self);
                    self.gameObject.AddComponent<FrozenTracker>().state = self;
                };
                On.EntityStates.FrozenState.OnExit += (orig, self) =>
                {
                    orig(self);
                    GameObject.DestroyImmediate(self.GetComponent<FrozenTracker>());
                };
            }
           
        }

        public static BuffDef bdFrozen;
        private static void BuffDisplay_AllocateIcons(On.RoR2.UI.BuffDisplay.orig_AllocateIcons orig, RoR2.UI.BuffDisplay self)
        {
            orig(self);
            if (self.source && self.source.TryGetComponent(out FrozenTracker frozen))
            {
                BuffDisplay.BuffIconDisplayData buffIconDisplayData = new BuffDisplay.BuffIconDisplayData(BuffIndex.None);
                buffIconDisplayData.buffCount = frozen.GetStacks();
                buffIconDisplayData.buffDef = bdFrozen;
                self.buffIconDisplayData.Insert(0, buffIconDisplayData);
            }
        }
    }
    public class FrozenTracker : MonoBehaviour
    {
        public int GetStacks()
        {
            return Mathf.CeilToInt((state.duration - state.age) * 2f);
        }

        public FrozenState state;
    }
}
