using RoR2;
using System.Collections.Generic;

namespace WolfoFixes
{

    public static class ModUtil
    {

        public static List<EquipmentIndex> GetEliteEquipment(bool droppableOnly = true)
        {
            List<EquipmentIndex> eliteEquips = new List<EquipmentIndex>();

            for (int i = 0; i < EquipmentCatalog.equipmentDefs.Length; i++)
            {
                EquipmentDef def = EquipmentCatalog.equipmentDefs[i];
                if (def.passiveBuffDef && def.passiveBuffDef.eliteDef)
                {
                    if (droppableOnly && def.dropOnDeathChance > 0 || !droppableOnly)
                    {
                        eliteEquips.Add(def.equipmentIndex);
                    }
                }
            }
            if (!droppableOnly)
            {
                eliteEquips.Remove(MissedContent.Equipment.EliteSecretSpeedEquipment.equipmentIndex);
                eliteEquips.Remove(JunkContent.Equipment.EliteYellowEquipment.equipmentIndex);
                eliteEquips.Remove(JunkContent.Equipment.EliteGoldEquipment.equipmentIndex);
            }

            return eliteEquips;
        }
        /*public static List<EliteDef> GetEliteDefs(bool includeUnique)
        {
            List<EliteDef> eliteDefs = new List<EliteDef>();

          
            return eliteDefs;
        }*/

    }

}
