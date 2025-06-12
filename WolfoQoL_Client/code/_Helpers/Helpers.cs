using RoR2;
using System.Linq;
using UnityEngine;

namespace WolfoQoL_Client
{
    public static class Help
    {
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
