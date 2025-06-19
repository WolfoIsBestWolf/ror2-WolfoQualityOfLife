using HG;
using RoR2;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    public class ItemTags
    {

        public static ItemTag evoBlacklistTag = (ItemTag)94;
        public static void Start()
        {
            //Using more tags because most of these tags are meant to be AIBlacklisted anyways.
            //Ie all Sprint -> AiBlacklist but not always tagged as such

            BasicPickupDropTable dtMonsterTeamTier1Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier1Item.asset").WaitForCompletion();
            BasicPickupDropTable dtMonsterTeamTier2Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier2Item.asset").WaitForCompletion();
            BasicPickupDropTable dtMonsterTeamTier3Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/MonsterTeamGainsItems/dtMonsterTeamTier3Item.asset").WaitForCompletion();

            ArenaMonsterItemDropTable dtArenaMonsterTier1 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier1.asset").WaitForCompletion();
            ArenaMonsterItemDropTable dtArenaMonsterTier2 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier2.asset").WaitForCompletion();
            ArenaMonsterItemDropTable dtArenaMonsterTier3 = Addressables.LoadAssetAsync<ArenaMonsterItemDropTable>(key: "RoR2/Base/arena/dtArenaMonsterTier3.asset").WaitForCompletion();

            BasicPickupDropTable dtAISafeTier1Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier1Item.asset").WaitForCompletion();
            BasicPickupDropTable dtAISafeTier2Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier2Item.asset").WaitForCompletion();
            BasicPickupDropTable dtAISafeTier3Item = Addressables.LoadAssetAsync<BasicPickupDropTable>(key: "RoR2/Base/Common/dtAISafeTier3Item.asset").WaitForCompletion();


            ItemTag[] TagsScav = { 
                ItemTag.AIBlacklist, 
                ItemTag.SprintRelated, 
                ItemTag.InteractableRelated, 
                ItemTag.OnStageBeginEffect, 
                ItemTag.HoldoutZoneRelated 
            };
            //No Equipment
            //No Onkill
            //
            ItemTag[] TagsMobs = { 
                ItemTag.AIBlacklist, 
                ItemTag.OnKillEffect,
                ItemTag.CannotCopy, 
                evoBlacklistTag, 
                ItemTag.EquipmentRelated, 
                ItemTag.InteractableRelated, 
                ItemTag.OnStageBeginEffect, 
                ItemTag.HoldoutZoneRelated 
            };

            dtMonsterTeamTier1Item.bannedItemTags = TagsMobs;
            dtMonsterTeamTier2Item.bannedItemTags = TagsMobs;
            dtMonsterTeamTier3Item.bannedItemTags = TagsMobs;

            dtArenaMonsterTier1.bannedItemTags = TagsMobs;
            dtArenaMonsterTier2.bannedItemTags = TagsMobs;
            dtArenaMonsterTier3.bannedItemTags = TagsMobs;

            dtAISafeTier1Item.bannedItemTags = TagsScav;
            dtAISafeTier2Item.bannedItemTags = TagsScav;
            dtAISafeTier3Item.bannedItemTags = TagsScav;


        }

        public static void CallLate()
        {
            #region Tag Fixes

            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.FlatHealth.tags, 1, 1); //Remove OnkillTag

             
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.BonusGoldPackOnKill.tags, ItemTag.AIBlacklist); //Enemies cannot use Gold

            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MoveSpeedOnKill.tags, ItemTag.OnKillEffect); //Missed Tag
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MushroomVoid.tags, ItemTag.SprintRelated); //Missed Tag


            ArrayUtils.ArrayAppend(ref RoR2Content.Items.MonstersOnShrineUse.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.GoldOnHit.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.LunarTrinket.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.FocusConvergence.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC2Content.Items.OnLevelUpFreeUnlock.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MushroomVoid.tags, ItemTag.AIBlacklist); //Sprint is blacklisted


            RoR2Content.Items.ParentEgg.tags[0] = ItemTag.Healing;

            #endregion



            if (!WConfig.cfgItemTags.Value)
            {
                return;
            }

            #region AI Blacklist
            #region White


            ArrayUtils.ArrayAppend(ref RoR2Content.Items.FlatHealth.tags, evoBlacklistTag); //Useless

            #endregion
            #region Green
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.Infusion.tags, ItemTag.AIBlacklist); //Useless
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.PrimarySkillShuriken.tags, evoBlacklistTag); //Borderline Overpowered
           
            #endregion
            #region Red
            ArrayUtils.ArrayAppend(ref RoR2Content.Items.NovaOnHeal.tags, ItemTag.AIBlacklist); //Overpowered

            ArrayUtils.ArrayAppend(ref RoR2Content.Items.BarrierOnOverHeal.tags, evoBlacklistTag); //Useless
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.MoreMissile.tags, evoBlacklistTag); //Useless

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
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.LunarSun.tags, ItemTag.AIBlacklist);
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.RandomlyLunar.tags, ItemTag.AIBlacklist);
            
            #endregion
            #region Void
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ElementalRingVoid.tags, ItemTag.AIBlacklist); //Unfun
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ExplodeOnDeathVoid.tags, ItemTag.AIBlacklist); //Op
           
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
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.HalfSpeedDoubleHealth.tags, ItemTag.Healing);

            //Boss
            ArrayUtils.ArrayRemoveAtAndResize(ref RoR2Content.Items.Knurl.tags, 0); //Remove Utility

            //Void
            ArrayUtils.ArrayAppend(ref DLC1Content.Items.ElementalRingVoid.tags, ItemTag.Utility);

            #endregion

        }



    }


}
