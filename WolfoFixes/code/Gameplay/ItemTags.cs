using HG;
using RoR2;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    public class ItemTags
    {

        public static ItemTag evoBlacklist = (ItemTag)94;
        public static void Start()
        {
            BasicPickupDropTable dtMonsterTeamTier1Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier1Item.asset").WaitForCompletion();
            BasicPickupDropTable dtMonsterTeamTier2Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier2Item.asset").WaitForCompletion();
            BasicPickupDropTable dtMonsterTeamTier3Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier3Item.asset").WaitForCompletion();

            ArenaMonsterItemDropTable dtArenaMonsterTier1 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier1.asset").WaitForCompletion();
            ArenaMonsterItemDropTable dtArenaMonsterTier2 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier2.asset").WaitForCompletion();
            ArenaMonsterItemDropTable dtArenaMonsterTier3 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier3.asset").WaitForCompletion();

            BasicPickupDropTable dtAISafeTier1Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier1Item.asset").WaitForCompletion();
            BasicPickupDropTable dtAISafeTier2Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier2Item.asset").WaitForCompletion();
            BasicPickupDropTable dtAISafeTier3Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier3Item.asset").WaitForCompletion();


            ItemTag[] TagsAISafe = { ItemTag.AIBlacklist, ItemTag.SprintRelated, ItemTag.PriorityScrap, ItemTag.InteractableRelated, ItemTag.HoldoutZoneRelated, ItemTag.OnStageBeginEffect };
            ItemTag[] TagsMonsterTeamGain = { ItemTag.AIBlacklist, ItemTag.CannotCopy, ItemTag.OnKillEffect, ItemTag.EquipmentRelated, ItemTag.SprintRelated, ItemTag.PriorityScrap, ItemTag.InteractableRelated, ItemTag.OnStageBeginEffect, ItemTag.HoldoutZoneRelated, ItemTag.Count };

            dtMonsterTeamTier1Item.bannedItemTags = TagsMonsterTeamGain;
            dtMonsterTeamTier2Item.bannedItemTags = TagsMonsterTeamGain;
            dtMonsterTeamTier3Item.bannedItemTags = TagsMonsterTeamGain;

            dtArenaMonsterTier1.bannedItemTags = TagsMonsterTeamGain;
            dtArenaMonsterTier2.bannedItemTags = TagsMonsterTeamGain;
            dtArenaMonsterTier3.bannedItemTags = TagsMonsterTeamGain;

            dtAISafeTier1Item.bannedItemTags = TagsAISafe;
            dtAISafeTier2Item.bannedItemTags = TagsAISafe;
            dtAISafeTier3Item.bannedItemTags = TagsAISafe;


        }

        public static void ItemTagChanges()
        {
            #region AI Blacklist
            #region White



            #endregion
            #region Green
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.BonusGoldPackOnKill.tags, ItemTag.AIBlacklist); //Useless
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.Infusion.tags, ItemTag.AIBlacklist); //Useless
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.PrimarySkillShuriken.tags, evoBlacklist); //Borderline Overpowered
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MoveSpeedOnKill.tags, ItemTag.OnKillEffect); //Missed Tag

            #endregion
            #region Red
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.NovaOnHeal.tags, ItemTag.AIBlacklist); //Overpowered

            ArrayUtils.ArrayAppend(ref RoR2Content.Items.BarrierOnOverHeal.tags, evoBlacklist); //Useless
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MoreMissile.tags, evoBlacklist); //Useless

            #endregion
            #region Boss
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.TitanGoldDuringTP.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.SprintWisp.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.SiphonOnLowHealth.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MinorConstructOnKill.tags, ItemTag.AIBlacklist);

            ArrayUtils.ArrayAppend(ref RoR2Content.Items.RoboBallBuddy.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MinorConstructOnKill.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.CaptainDefenseMatrix.tags, ItemTag.CannotSteal);


            #endregion
            #region Lunar
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.MonstersOnShrineUse.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.GoldOnHit.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.LunarTrinket.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.FocusConvergence.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.LunarSun.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.RandomlyLunar.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC2Content.Items.OnLevelUpFreeUnlock.tags, ItemTag.AIBlacklist);

            #endregion
            #region Void
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ElementalRingVoid.tags, ItemTag.AIBlacklist); //Unfun
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ExplodeOnDeathVoid.tags, ItemTag.AIBlacklist); //Op
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MushroomVoid.tags, ItemTag.SprintRelated);
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MushroomVoid.tags, ItemTag.AIBlacklist); //Sprint is blacklisted

            #endregion
            #endregion

            #region Modded
            ItemDef tempDef = ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex("WatchMetronome"));
            if (tempDef != null)
            {
                ArrayUtils.ArrayAppend(ref tempDef.tags, ItemTag.AIBlacklist);
            }
            tempDef = ItemCatalog.GetItemDef(ItemCatalog.FindItemIndex("VV_ITEM_EHANCE_VIALS_ITEM"));
            if (tempDef != null)
            {
                ArrayUtils.ArrayAppend(ref tempDef.tags, ItemTag.AIBlacklist);
            }
            #endregion

            #region Category stuff

            //Green
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.Infusion.tags, 0); //Remove Utility

            //Red
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.HeadHunter.tags, 0); //Remove Utility
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.BarrierOnKill.tags, 0); //Remove Utility
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.BarrierOnOverHeal.tags, 0); //Remove Utility
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.NovaOnHeal.tags, ItemTag.Healing);
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ImmuneToDebuff.tags, ItemTag.Healing);

            //Lunar
            RoR2Content.Items.RandomDamageZone.tags[0] = ItemTag.Damage;
            DLC1Content.Items.HalfSpeedDoubleHealth.tags[0] = ItemTag.Healing;
            DLC1Content.Items.LunarSun.tags[0] = ItemTag.Damage;
            RoR2Content.Items.LunarUtilityReplacement.tags[0] = ItemTag.Healing;
            RoR2Content.Items.ShieldOnly.tags[0] = ItemTag.Healing;

            //Boss
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.HalfSpeedDoubleHealth.tags, ItemTag.Healing);
            RoR2Content.Items.ParentEgg.tags[0] = ItemTag.Healing;
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.Knurl.tags, 0); //Remove Utility

            //Void
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ElementalRingVoid.tags, ItemTag.Utility);

            #endregion

        }



    }


}
