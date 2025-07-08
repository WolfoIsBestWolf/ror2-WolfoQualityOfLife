using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace WolfoFixes
{
    public class AdvancedPickupDropTable : BasicPickupDropTable
    {
        //Includes Aspects
        //Only generates when frist drop
        private bool generated;
        public bool clearAfterGenerating;
        public bool unbiasedByItemCount = false;

        public override PickupIndex GenerateDropPreReplacement(Xoroshiro128Plus rng)
        {
            RegenerateReal();
            return GenerateDropFromWeightedSelection(rng, this.selector);
        }
        public override PickupIndex[] GenerateUniqueDropsPreReplacement(int maxDrops, Xoroshiro128Plus rng)
        {
            RegenerateReal();
            return GenerateUniqueDropsFromWeightedSelection(maxDrops, rng, this.selector);
        }

        public override void Regenerate(Run run)
        {
            generated = false;
            selector.Clear();
        }
        public void RegenerateReal()
        {
            if (generated)
            {
                return;
            }
            generated = true;
            if (unbiasedByItemCount)
            {
                GenerateUnbiased(Run.instance);
            }
            else
            {
                RegenerateDropTable(Run.instance);
            }
            AddEliteEquip(Run.instance);
        }

        private void GenerateUnbiased(Run run)
        {
            this.selector.Clear();
            this.Add(run.availableTier1DropList, this.tier1Weight);
            this.Add(run.availableTier2DropList, this.tier2Weight);
            this.Add(run.availableTier3DropList, this.tier3Weight);
            this.Add(run.availableBossDropList, this.bossWeight);
            this.Add(run.availableLunarItemDropList, this.lunarItemWeight);
            this.Add(run.availableLunarEquipmentDropList, this.lunarEquipmentWeight);
            this.Add(run.availableLunarCombinedDropList, this.lunarCombinedWeight);
            this.Add(run.availableEquipmentDropList, this.equipmentWeight);
            this.Add(run.availableVoidTier1DropList, this.voidTier1Weight);
            this.Add(run.availableVoidTier2DropList, this.voidTier2Weight);
            this.Add(run.availableVoidTier3DropList, this.voidTier3Weight);
            this.Add(run.availableVoidBossDropList, this.voidBossWeight);
        }
        private void AddUnbiased(List<PickupIndex> sourceDropList, float chance)
        {
            if (chance <= 0f || sourceDropList.Count == 0)
            {
                return;
            }
            float chanceDiv = chance / sourceDropList.Count;
            foreach (PickupIndex pickupIndex in sourceDropList)
            {
                if (!this.IsFilterRequired() || this.PassesFilter(pickupIndex))
                {
                    this.selector.AddChoice(pickupIndex, chanceDiv);
                }
            }
        }

        public float eliteEquipmentWeight;
        private void AddEliteEquip(Run run)
        {
            if (eliteEquipmentWeight == 0)
            {
                return;
            }
            var list = ModUtil.GetEliteEquipment();

            foreach (EquipmentIndex equipmentIndex in list)
            {
                if (run.expansionLockedEquipment.Contains(equipmentIndex))
                {
                    list.Remove(equipmentIndex);
                }
            }
            float dropChance = (unbiasedByItemCount ? eliteEquipmentWeight / (float)list.Count : eliteEquipmentWeight);
            foreach (EquipmentIndex equipmentIndex in list)
            {
                selector.AddChoice(PickupCatalog.FindPickupIndex(equipmentIndex), dropChance);
            }
        }

    }


    public class ForcedTeamCSC : CharacterSpawnCard
    {
        public TeamIndex teamIndexOverride = TeamIndex.Monster;
        public override void Spawn(Vector3 position, Quaternion rotation, DirectorSpawnRequest directorSpawnRequest, ref SpawnCard.SpawnResult result)
        {
            directorSpawnRequest.teamIndexOverride = teamIndexOverride;
            base.Spawn(position, rotation, directorSpawnRequest, ref result);
        }
    }

    public static class Shared
    {
        public static BasicPickupDropTable dtAllTier
        {
            get
            {
                if (!_dtAllTier)
                {
                    SetupShared();
                }
                return _dtAllTier;
            }
        }
        public static BasicPickupDropTable _dtAllTier;

        public static void SetupShared()
        {
            if (_dtAllTier)
            {
                return;
            }
            //Normal
            //W80, G20, R1

            AdvancedPickupDropTable dtAllTier = ScriptableObject.CreateInstance<AdvancedPickupDropTable>();
            dtAllTier.name = "dtAllTier";
            dtAllTier.tier1Weight = 160;
            dtAllTier.tier2Weight = 40;
            dtAllTier.tier3Weight = 4;
            dtAllTier.bossWeight = 4;
            dtAllTier.equipmentWeight = 20;
            dtAllTier.lunarItemWeight = 15;
            dtAllTier.voidTier1Weight = 60;
            dtAllTier.voidTier2Weight = 30;
            dtAllTier.voidTier3Weight = 15;
            dtAllTier.voidBossWeight = 15;
            dtAllTier.lunarEquipmentWeight = 5;
            dtAllTier.eliteEquipmentWeight = 5;
            _dtAllTier = dtAllTier;

        }

    }

}
