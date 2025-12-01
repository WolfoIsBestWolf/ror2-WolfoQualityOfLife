using HG;
using RoR2;
using RoR2.Stats;
using UnityEngine;

namespace WolfoQoL_Client
{
    public static class Help
    {

        public static ItemIndex FindItemWithHighestStat(StatSheet statSheet, PerItemStatDef perItemStatDef)
        {
            StatField statField = statSheet.fields[perItemStatDef.FindStatDef((ItemIndex)0).index];
            ItemIndex result = (ItemIndex)0;
            for (int i = 1; i < ItemCatalog.itemCount; i++)
            {
                ref StatField ptr = ref statSheet.fields[perItemStatDef.FindStatDef((ItemIndex)i).index];
                if (statField.CompareTo(ptr) < 0)
                {
                    statField = ptr;
                    result = (ItemIndex)i;
                }
            }
            if (statField.IsDefault())
            {
                return ItemIndex.None;
            }
            return result;
        }

        internal static bool IconIsNotSuitable(CharacterBody body)
        {
            //Used by game to determine for body generate
            //But not really ideal for death screen I feel
            if (!body.portraitIcon)
            {
                return true;
            }
            string name = body.portraitIcon.name;
            return name == "texMysteryIcon" || name == "texNullIcon";
        }
        public static BodyIndex FindBodyWithHighestStatNotMasterless(StatSheet statSheet, PerBodyStatDef perBodyStatDef)
        {
            StatField statField = statSheet.fields[perBodyStatDef.FindStatDef((BodyIndex)0).index];
            int result = 0;
            for (int bodyIndex = 1; bodyIndex < BodyCatalog.bodyCount; bodyIndex++)
            {
                ref StatField ptr = ref statSheet.fields[ArrayUtils.GetSafe(perBodyStatDef.bodyIndexToStatDef, bodyIndex).index];
                if (ptr.ulongValue > (ulong)0 && statField.CompareTo(ptr) < 0)
                //if (statField.ulongValue < ptr.ulongValue) //??
                {
                    //Only check if Is Bigger, not check every body
                    if (!IconIsNotSuitable(BodyCatalog.bodyPrefabBodyComponents[bodyIndex]))
                    {
                        statField = ptr;
                        result = bodyIndex;
                    }
                }
            }
            if (statField.IsDefault())
            {
                return BodyIndex.None;
            }
            return (BodyIndex)result;
        }


        public static SceneDef ITtoNormal(SceneDef scene)
        {
            if (scene.cachedName == "itdampcave")
            {
                return SceneCatalog.FindSceneDef("dampcavesimple");
            }
            string shortened = scene.cachedName.Remove(0, 2);
            return SceneCatalog.FindSceneDef(scene.cachedName.Remove(0, 2));

        }

        public static string GetColoredName(ItemIndex index, bool temp = false)
        {
            return GetColoredName(PickupCatalog.FindPickupIndex(index), 0, temp);
        }
        public static string GetColoredName(EquipmentIndex index)
        {
            return GetColoredName(PickupCatalog.FindPickupIndex(index));
        }
        public static string GetColoredName(DroneIndex index, int tier)
        {
            return GetColoredName(PickupCatalog.FindPickupIndex(index), tier);
        }
        public static string GetColoredName(PickupIndex index, int tier = 0, bool isTemporary = false)
        {
            PickupDef def = PickupCatalog.GetPickupDef(index);
            if (def == null)
            {
                return "Null";
            }
            string name = ColorUtility.ToHtmlStringRGB(def.baseColor);
            name = "<color=#" + name + ">" + Language.GetString(def.nameToken);
            if (isTemporary)
            {
                name = Language.GetStringFormatted("ITEM_MODIFIER_TEMP", new object[]
                {
                        name
                });
            }
            if (tier > 0)
            {
                name = Language.GetStringFormatted("DRONE_TIER_SUFFIX", new object[]
                {
                    name,
                    tier+1
                });
            }
            name += "</color>";
            return name;
        }
        public static string GetColoredName(PickupDef def, bool isTemporary)
        {
            string color = ColorUtility.ToHtmlStringRGB(def.baseColor);
            if (isTemporary)
            {
                return "<color=#" + color + ">" + Language.GetStringFormatted("ITEM_MODIFIER_TEMP", new object[]
                {
                        Language.GetString(def.nameToken)
                }) + "</color>";
            }
            return "<color=#" + color + ">" + Language.GetString(def.nameToken) + "</color>";
        }

    }

}
