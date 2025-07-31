using BepInEx;
using BepInEx.Configuration;
using UnityEngine;

namespace WolfoQoL_Client
{
    /*
    public class LookingGlassPresets
    {
        public enum Preset
        {
            Set,
            Default,
            AllOff,
            Simpler,
            Minimal,
        }
        public static ConfigEntry<Preset> cfgLookingGlassPreset;




        public static void Start()
        {
            PluginInfo lookingGlass;
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.TryGetValue("droppod.lookingglass", out lookingGlass))
            {
                cfgLookingGlassPreset = WConfig.ConfigFile_Client.Bind(
                  "Hud",
                  "LookingGlass Preset",
                  Preset.Set,
                  "Some Presets for Looking Glass.\nWill overwrite your settings with a premade preset if changed in game, never on startup.\nProbably best to leave it alone if you already customized it."
                );
                cfgLookingGlassPreset.SettingChanged += CfgLookingGlassPreset_SettingChanged;
                return;
                ConfigFile config = lookingGlass.Instance.Config;
                var entries = config.GetConfigEntries();
                foreach (ConfigEntryBase entry in entries)
                {

                    Debug.LogFormat("\n{0}\n{1}\n{2}\n{3}\n{4}\n{5}", new object[]
                    {
                        entry,
                        entry.BoxedValue,
                        entry.DefaultValue,
                        entry.Definition,
                        entry.Definition.Section,
                        entry.Definition.Key,
                        entry.Description,
                    });

                }
            }
        }

        private static void CfgLookingGlassPreset_SettingChanged(object sender, System.EventArgs e)
        {

            PluginInfo lookingGlass;
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.TryGetValue("droppod.lookingglass", out lookingGlass))
            {

                ConfigFile config = lookingGlass.Instance.Config;
                var entries = config.GetConfigEntries();
                foreach (ConfigEntryBase entry in entries)
                {
                    if (cfgLookingGlassPreset.Value == Preset.Default)
                    {
                        entry.BoxedValue = entry.DefaultValue;
                    }
                    else if (cfgLookingGlassPreset.Value == Preset.AllOff)
                    {
                        if (entry is ConfigEntry<bool>)
                        {
                            switch (entry.Definition.Key)
                            {
                                case "Override Stats Display Height":
                                case "Attach To Objective Panel":
                                case "Use default colors":
                                    break;
                                default:
                                    entry.BoxedValue = false;
                                    break;
                            }
                        }
                    }
                    else if (cfgLookingGlassPreset.Value >= Preset.Simpler)
                    {
                        switch (entry.Definition.Key)
                        {
                            case "Stats Display String":
                                if (cfgLookingGlassPreset.Value == Preset.Simpler)
                                {
                                    entry.BoxedValue =
                                     "<margin-left=0.6em><line-height=110%>" +
                                    "<align=center><size=115%>Stats:</align></size>\n" +
                                     "Damage: [damage]\n" +
                                     "Crit Chance: [critWithLuck]\n" +
                                     "Attack Speed: [attackSpeed]\n" +
                                     "Armor: [armor] | [armorDamageReduction]\n" +
                                     "Regen: [regen]\n" +
                                     "Speed: [speed]\n" +
                                     "Jumps: [availableJumps]/[maxJumps]\n" +
                                     "Kills: [killCount]\n \n\n" +
                                     "</line-height></margin>";
                                }
                                else if (cfgLookingGlassPreset.Value == Preset.Minimal)
                                {
                                    entry.BoxedValue = string.Empty;
                                }
                                break;
                            case "Secondary Stats Display String":
                                if (cfgLookingGlassPreset.Value == Preset.Simpler)
                                {
                                    entry.BoxedValue =
                                     "<margin-left=0.6em><line-height=110%>" +
                                     "<align=center><size=115%>Stats:</align></size>\n" +
                                     "Damage: [damage]\n" +
                                     "Crit Chance: [critWithLuck]\n" +
                                     "Bleed Chance: [bleedChanceWithLuck]\n" +
                                     "Attack Speed: [attackSpeed]\n" +
                                     "Armor: [armor] | [armorDamageReduction]\n" +
                                     "Regen: [regen]\n" +
                                     "Speed: [speed]\n" +
                                     "Jumps: [availableJumps]/[maxJumps]\n" +
                                     "Kills: [killCount]\n" +
                                     "Mountain Shrines: [mountainShrines]\n" +
                                     "<size=70%>Bazaar: [shopPortal] Gold: [goldPortal] Void: [voidPortal]</size>" +
                                     "</line-height></margin>";
                                }
                                else if (cfgLookingGlassPreset.Value == Preset.Minimal)
                                {
                                    entry.BoxedValue =
                                     "<margin-left=0.6em><line-height=110%>" +
                                     "Crit Chance: [critWithLuck]\n" +
                                     "Bleed Chance: [bleedChanceWithLuck]\n" +
                                     "Mountain Shrines: [mountainShrines]\n" +
                                     "Bazaar Portal: [shopPortal]" +
                                     "</line-height></margin>";
                                }
                                break;
                            case "StatsDisplay":
                                entry.BoxedValue = true;
                                break;
                            case "Use Secondary StatsDisplay":
                                entry.BoxedValue = true;
                                break;
                            case "StatsDisplay font size":
                                entry.BoxedValue = 13f; //12 font size other objectives
                                break;
                            case "StatsDisplay update interval":
                                entry.BoxedValue = 0.3f;
                                break;
                            case "Sort Scrapper":
                                entry.BoxedValue = true;
                                break;
                            case "Tier Sort":
                                entry.BoxedValue = entry.DefaultValue;
                                break;
                            case "Stack Size Sort":
                                entry.BoxedValue = false;
                                break;
                            case "Sort Command Menu":
                                entry.BoxedValue = false;
                                break;
                            case "Command Menu Descending":
                                entry.BoxedValue = false;
                                break;
                            case "Disable Command Window Blur":
                                entry.BoxedValue = false;
                                break;
                            case "Hide Count If Zero":
                                entry.BoxedValue = true;
                                break;
                            case "Tier Order":
                                entry.BoxedValue = "Tier1 VoidTier1 Tier2 VoidTier2 Tier3 VoidTier3 Boss VoidBoss Lunar NoTier";
                                break;
                            case "Full Item Description On Pickup":
                                entry.BoxedValue = false;
                                break;
                            case "Item Stats On Ping":
                                entry.BoxedValue = false;
                                break;
                            case "Permanent Cooldown Indicator For Skills":
                                entry.BoxedValue = false;
                                break;
                            case "Permanent Cooldown Indicator For Equip":
                                if (cfgLookingGlassPreset.Value == Preset.Minimal)
                                {
                                    entry.BoxedValue = false;
                                    break;
                                }
                                entry.BoxedValue = true;
                                break;
                            case "Item Counters":
                                if (cfgLookingGlassPreset.Value == Preset.Minimal)
                                {
                                    entry.BoxedValue = false;
                                    break;
                                }
                                entry.BoxedValue = true;
                                break;

                        }
                    }

                }
            }

            cfgLookingGlassPreset.Value = Preset.Set;
        }
    }
    */
}
