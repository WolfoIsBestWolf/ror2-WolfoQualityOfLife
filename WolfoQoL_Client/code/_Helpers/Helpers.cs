using BepInEx.Logging;
using RoR2;
using RoR2.Stats;
using UnityEngine;

namespace WolfoQoL_Client
{
    public static class Help
    {
        public static ManualLogSource Log;


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


        public static SceneDef ITtoNormal(SceneDef scene)
        {
            if (scene.cachedName == "itdampcave")
            {
                return SceneCatalog.FindSceneDef("dampcavesimple");
            }
            string shortened = scene.cachedName.Remove(0, 2);
            return SceneCatalog.FindSceneDef(scene.cachedName.Remove(0, 2));

        }

        public static string GetColoredName(ItemIndex index)
        {
            return GetColoredName(PickupCatalog.FindPickupIndex(index));
        }
        public static string GetColoredName(EquipmentIndex index)
        {
            return GetColoredName(PickupCatalog.FindPickupIndex(index));
        }
        public static string GetColoredName(PickupIndex index)
        {
            PickupDef def = PickupCatalog.GetPickupDef(index);
            if (def == null)
            {
                return "Null";
            }
            string name = ColorUtility.ToHtmlStringRGB(def.baseColor);
            name = "<color=#" + name + ">" + Language.GetString(def.nameToken) + "</color>";
            return name;
        }
        public static string GetColoredName(PickupDef def)
        {
            string name = ColorUtility.ToHtmlStringRGB(def.baseColor);
            name = "<color=#" + name + ">" + Language.GetString(def.nameToken) + "</color>";
            return name;
        }

    }

}
