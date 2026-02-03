using HG;
using RoR2;
using RoR2.Stats;
using System.Text;
using UnityEngine;

namespace WolfoQoL_Client
{
    public static class Help
    {
        private static readonly StringBuilder stringBuilder = new StringBuilder();

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

        public static ulong GetTotalEquipmentAcivations(StatSheet statSheet)
        {            
            ulong totalActivations = 0;
            for (int i = 0; i < EquipmentCatalog.equipmentCount; i++)
            {
                totalActivations += statSheet.GetStatValueULong(PerEquipmentStatDef.totalTimesFired.FindStatDef((EquipmentIndex)i));
            }
            Log.LogMessage($"Equipment Activations: {totalActivations}");
            return totalActivations;
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
            StringBuilder name = stringBuilder;
            name.Clear();
            name.Append("<color=#");
            name.Append(ColorUtility.ToHtmlStringRGB(def.baseColor));
            name.Append(">"); 
            if (isTemporary)
            {
                name.Append(Language.GetStringFormatted("ITEM_MODIFIER_TEMP", Language.GetString(def.nameToken)));
            }
            else if (tier > 0)
            {
                name.Append(Language.GetStringFormatted("DRONE_TIER_SUFFIX", Language.GetString(def.nameToken), tier+1));
            }
            else
            {
                name.Append(Language.GetString(def.nameToken));
            }
            name.Append("</color>");
            return name.ToString();
        }
        public static string GetColoredName(PickupDef def, bool isTemporary)
        {
            StringBuilder name = stringBuilder;
            name.Clear();
            name.Append("<color=#");
            name.Append(ColorUtility.ToHtmlStringRGB(def.baseColor));
            name.Append(">");
            if (isTemporary)
            {
                name.Append(Language.GetStringFormatted("ITEM_MODIFIER_TEMP", def.nameToken));
            }
            else
            {
                name.Append(Language.GetString(def.nameToken));
            }
            name.Append("</color>");
            return name.ToString();
        }

    }

}
