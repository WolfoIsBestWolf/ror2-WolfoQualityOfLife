using MonoMod.Cil;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{
    public class ExtendedPickupDropTable : BasicPickupDropTable
    {
        public float eliteEquipmentWeight;
        public override void Regenerate(Run run)
        {
            GenerateWeightedSelection(run);
            AddToWeightedSelection(run);
        }
        private void AddToWeightedSelection(Run run)
        {
            if (PickupTransmutationManager.equipmentBossGroup.Length == 0)
            {
                return;
            }
            if (eliteEquipmentWeight == 0)
            {
                return;
            }
            var list = ModUtil.GetEliteEquipment();
            foreach (EquipmentIndex equipmentIndex in list)
            {         
                if (!run.expansionLockedEquipment.Contains(equipmentIndex))
                {
                    this.selector.AddChoice(PickupCatalog.FindPickupIndex(equipmentIndex), eliteEquipmentWeight);
                }
            }
        }
    }

    public static class Shared
    {
        public static BasicPickupDropTable dtAllTier;
        public static void SetupShared()
        {
            if (Shared.dtAllTier)
            {
                return;
            }
            ExtendedPickupDropTable dtAllTier = ScriptableObject.CreateInstance<ExtendedPickupDropTable>();
            dtAllTier.name = "dtAllTier";
            dtAllTier.tier1Weight = 120;
            dtAllTier.tier2Weight = 20;
            dtAllTier.tier3Weight = 2;
            dtAllTier.bossWeight = 2;
            dtAllTier.equipmentWeight = 15;
            dtAllTier.lunarItemWeight = 8;
            dtAllTier.voidTier1Weight = 50;
            dtAllTier.voidTier2Weight = 20;
            dtAllTier.voidTier3Weight = 2;
            dtAllTier.voidBossWeight = 1;
            dtAllTier.lunarEquipmentWeight = 1;
            dtAllTier.eliteEquipmentWeight = 1;
            Shared.dtAllTier = dtAllTier;

        }
       
    }

}
