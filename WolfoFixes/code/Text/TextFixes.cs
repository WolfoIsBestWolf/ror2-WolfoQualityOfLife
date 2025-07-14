using R2API;
using RoR2;
using UnityEngine;

namespace WolfoFixes
{

    internal class TextFixes
    {

        public static void CallLate()
        {
            if (WConfig.cfgTextItems.Value)
            {
                ItemText();
            }
            if (WConfig.cfgTextCharacters.Value)
            {
                CharacterText();
            }
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody").GetComponent<CharacterBody>().subtitleNameToken = "LUNAREXPLODER_BODY_SUBTITLE";
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AssassinBody").GetComponent<CharacterBody>().baseNameToken = ""; //He is not ENGI_BODY

            LanguageAPI.Add("CHEF_UTILITY_ALT_BOOST_DESCRIPTION_SKILL", "Twirl upwards then slam down, exploding with <style=cIsUtility>activated oil</style> for <style=cIsDamage>200% damage</style>. <style=cIsDamage>Launch again</style> by reactivating this ability before landing.");

        }




        internal static void ItemText()
        {
            #region Items
            //Funny All Stats
            LanguageAPI.Add("ITEM_SHINYPEARL_DESC", "Increases <style=cIsDamage>damage</style>, <style=cIsDamage>attack speed</style>, <style=cIsDamage>critical strike chance</style>, <style=cIsHealing>maximum health</style>, <style=cIsHealing>base health regeneration</style>, <style=cIsHealing>base armor</style>, <style=cIsUtility>movement speed</style> by <style=cIsDamage>1<style=cIsHealing>0<style=cIsUtility>%<style=cStack> (+10% per stack)</style></style></style></style>", "en");

            //Danger not Combat
            LanguageAPI.Add("ITEM_HEALWHILESAFE_DESC", "Increases <style=cIsHealing>base health regeneration</style> by <style=cIsHealing>+3 hp/s</style> <style=cStack>(+3 hp/s per stack)</style> while outside of danger.", "en");

            //Wrong damage per stack
            LanguageAPI.Add("ITEM_MISSILEVOID_DESC", "Gain a <style=cIsHealing>shield</style> equal to <style=cIsHealing>10%</style> of your maximum health. While you have a <style=cIsHealing>shield</style>, hitting an enemy fires a missile that deals <style=cIsDamage>40%</style> <style=cStack>(+40% per stack)</style> TOTAL damage. <style=cIsVoid>Corrupts all AtG Missile Mk. 1s</style>.", "en");

            //Doesn't mention Special Cooldown Reduction
            LanguageAPI.Add("ITEM_EQUIPMENTMAGAZINEVOID_PICKUP", "Add an extra charge of your Special skill. Reduce Special skill cooldown. <style=cIsVoid>Corrupts all Fuel Cells</style>.", "en");
            LanguageAPI.Add("ITEM_EQUIPMENTMAGAZINEVOID_DESC", "Add <style=cIsUtility>+1</style> <style=cStack>(+1 per stack)</style> charge of your <style=cIsUtility>Special skill</style>. <style=cIsUtility>Reduces Special skill cooldown</style> by <style=cIsUtility>33%</style>. <style=cIsVoid>Corrupts all Fuel Cells</style>.", "en");

            //Straight up just doesn't say it corrupts any items.
            LanguageAPI.Add("ITEM_VOIDMEGACRABITEM_DESC", "Every <style=cIsUtility>60</style><style=cStack>(-50% per stack)</style> seconds, gain a random <style=cIsVoid>Void</style> ally. Can have up to <style=cIsUtility>1</style><style=cStack>(+1 per stack)</style> allies at a time. <style=cIsVoid>Corrupts all </style><style=cIsTierBoss>yellow items</style><style=cIsVoid>.</style>", "en");
            //Wrong rarities
            LanguageAPI.Add("ITEM_TREASURECACHEVOID_DESC", "A <style=cIsUtility>hidden cache</style> containing an item (42%/<style=cIsHealing>42%</style>/<style=cIsHealth>16%</style>) will appear in a random location <style=cIsUtility>on each stage</style>. Opening the cache <style=cIsUtility>consumes</style> this item. <style=cIsVoid>Corrupts all Rusted Keys</style>.", "en");

            //First stack counts for cooldown reduction, 15s start
            LanguageAPI.Add("ITEM_NOVAONLOWHEALTH_DESC", "Falling below <style=cIsHealth>25% health</style> causes you to explode, dealing <style=cIsDamage>6000% base damage</style>. Recharges every <style=cIsUtility>15 seconds</style> <style=cStack>(-33% per stack)</style>.");
            //Is TOTAL Damage
            LanguageAPI.Add("ITEM_LIGHTNINGSTRIKEONHIT_DESC", "<style=cIsDamage>10%</style> chance on hit to down a lightning strike, dealing <style=cIsDamage>500%</style> <style=cStack>(+500% per stack)</style> TOTAL damage.");
            //Is TOTAL Damage
            LanguageAPI.Add("ITEM_FIREBALLSONHIT_DESC", "<style=cIsDamage>10%</style> chance on hit to call forth <style=cIsDamage>3 magma balls</style> from an enemy, dealing <style=cIsDamage>300%</style> <style=cStack>(+300% per stack)</style> TOTAL damage and <style=cIsDamage>igniting</style> all enemies for an additional <style=cIsDamage>50%</style> TOTAL damage over time.");
            //Removed unused cooldown and dont say "bonus"
            LanguageAPI.Add("ITEM_BEETLEGLAND_DESC", "<style=cIsUtility>Summon a Beetle Guard</style> with <style=cIsDamage>400%</style> damage and <style=cIsHealing>200% health</style>. Can have up to <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style> Guards at a time.");

            //Added Radius
            LanguageAPI.Add("ITEM_BOUNCENEARBY_DESC", "<style=cIsDamage>20%</style> <style=cStack>(+20% per stack)</style> chance on hit to <style=cIsDamage>fire homing hooks</style> at up to <style=cIsDamage>10</style> <style=cStack>(+5 per stack)</style> enemies within <style=cIsDamage>30m</style> for <style=cIsDamage>100%</style> TOTAL damage.");

            //Stacks as 2 + 1, 10m start
            LanguageAPI.Add("ITEM_INTERSTELLARDESKPLANT_DESC", "On kill, plant a <style=cIsHealing>healing</style> fruit seed that grows into a plant after <style=cIsUtility>5</style> seconds. \n\nThe plant <style=cIsHealing>heals</style> for <style=cIsHealing>10%</style> of <style=cIsHealing>maximum health</style> every second to all allies within <style=cIsHealing>10m</style> <style=cStack>(+5m per stack)</style>. Lasts <style=cIsUtility>10</style> seconds.", "en");
            //Wrong radius
            LanguageAPI.Add("ITEM_MUSHROOM_DESC", "After standing still for <style=cIsHealing>1</style> second, create a zone that <style=cIsHealing>heals</style> for <style=cIsHealing>4.5%</style> <style=cStack>(+2.25% per stack)</style> of your <style=cIsHealing>health</style> every second to all allies within <style=cIsHealing>3.5m</style> <style=cStack>(+1.5m per stack)</style>.", "en");
            //Wrong radius Stack
            LanguageAPI.Add("ITEM_BEHEMOTH_DESC", "All your <style=cIsDamage>attacks explode</style> in a <style=cIsDamage>4m </style><style=cStack>(+2.5m per stack)</style> radius for a bonus <style=cIsDamage>60%</style> TOTAL damage to nearby enemies.", "en");

            //Missing 5% Crit
            LanguageAPI.Add("ITEM_ATTACKSPEEDONCRIT_DESC", "Gain <style=cIsDamage>5% critical chance</style>. <style=cIsDamage>Critical strikes</style> increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>12%</style> for <style=cIsDamage>3s</style>. Maximum cap of <style=cIsDamage>36% <style=cStack>(+24% per stack)</style> attack speed</style>.", "en");
            //Missing 5% Crit
            LanguageAPI.Add("ITEM_BLEEDONHITANDEXPLODE_DESC", "Gain <style=cIsDamage>5% critical chance</style>. <style=cIsDamage>Critical Strikes bleed</style> enemies for <style=cIsDamage>240%</style> base damage. <style=cIsDamage>Bleeding</style> enemies <style=cIsDamage>explode</style> on death for <style=cIsDamage>400%</style> <style=cStack>(+400% per stack)</style> damage, plus an additional <style=cIsDamage>15%</style> <style=cStack>(+15% per stack)</style> of their maximum health.", "en");

            //16x not 15x // or Bonus +15, either way not mentioned correctly
            LanguageAPI.Add("ITEM_GHOSTONKILL_DESC", "Killing enemies has a <style=cIsDamage>7%</style> chance to <style=cIsDamage>spawn a ghost</style> of the killed enemy with <style=cIsDamage>1600%</style> damage. Lasts <style=cIsDamage>30s</style> <style=cStack>(+30s per stack)</style>.", "en");

            if (!WolfoMain.riskyFixes)
            {
                //First stack counts for cooldown reduction
                LanguageAPI.Add("ITEM_BEARVOID_DESC", "<style=cIsHealing>Blocks</style> incoming damage once. Recharges after <style=cIsUtility>13.5 seconds</style> <style=cStack>(-10% per stack)</style>. <style=cIsVoid>Corrupts all Tougher Times</style>.", "en");
            }

            //Elec boomerang is 40% per tick but ig it ticks 3 times per second.

            //5% not 4%, also just written cleaner and mention rarities.
            LanguageAPI.Add("ITEM_ITEMDROPCHANCEONKILL_DESC", "Killing a large monster will always drop an <style=cIsUtility>item</style>. (90%/<style=cIsHealing>10%</style>/<style=cIsHealth>0.1%</style>) Killing a elite has a <style=cIsUtility>5%</style> <style=cStack>(+1% per stack)</style> chance of dropping an <style=cIsUtility>item</style>.", "en");

            //Mention rarities, cleaner written?
            LanguageAPI.Add("ITEM_EXTRASHRINEITEM_DESC", "Shrine of Chance rewards have a <style=cIsUtility>40%</style> <style=cStack>(+10% per stack)</style> chance to be of higher rarity. (<style=cIsHealing>79%</style>/<style=cIsHealth>20%</style>/<style=cIsTierBoss>1%</style>)", "en");

            //Unique buff (nitpick)
            LanguageAPI.Add("ITEM_BOOSTALLSTATS_DESC", "Grants <style=cIsUtility>4%</style> increase to <style=cIsUtility>ALL stats</style> for each unique buff, up to a maximum of <style=cIsUtility>4</style> <style=cStack>(+4 per stack)</style>.", "en");

            //Nonsensical stacking
            //LanguageAPI.Add("ITEM_LOWERPRICEDCHESTS_DESC", "The first chest opened per stage drops an <style=cIsUtility>extra item</style>. Having 2 Sale Stars adds a chance to gain upwards of <style=cIsUtility>5 items</style>. <style=cStack>Each additional Sale Star increases the chance by 5%</style>.", "en");
            //LanguageAPI.Add("ITEM_LOWERPRICEDCHESTS_DESC", "The first chest opened per stage drops an <style=cIsUtility>extra item</style>. Multiple Sale Stars add a 30% chance to gain 3, 4.5% chance for 4, 0.1% chance for 5 items. <style=cStack>Additional Sale Stars increases each chance by 5%</style>. At the start of each stage, it regenerates.", "en");
            LanguageAPI.Add("ITEM_LOWERPRICEDCHESTS_DESC", "The first chest opened per stage drops an <style=cIsUtility>extra item</style>. Multiple Sale Stars add a <style=cIsUtility>(30%/4.5%/0.1%)</style> <style=cStack>(+5% per stack)</style> chance to gain <style=cIsUtility>3</style>, <style=cIsUtility>4</style> or <style=cIsUtility>5</style> items. At the start of each stage, it regenerates.", "en");

            //I guess?
            //LanguageAPI.Add("ITEM_METEORATTACKONHIGHDAMAGE_DESC", "<style=cIsDamage>3%</style> chance on hit to call a meteor strike, dealing <style=cIsDamage>2000%</style> base damage. Every <style=cIsDamage>100%</style> attack damage dealt increases the activation chance by <style=cIsDamage>3%</style> <style=cStack>(+3% per stack)</style> and damage by <style=cIsDamage>150%</style> <style=cStack>(+50% per stack)</style>. Cannot exceed 7500% damage and 50% chance.", "en");
            LanguageAPI.Add("ITEM_METEORATTACKONHIGHDAMAGE_DESC", "<style=cIsDamage>3%</style> chance on hit to call a meteor strike, dealing <style=cIsDamage>2000%</style> base damage. For every <style=cIsDamage>100%</style> attack damage, increase activation chance by <style=cIsDamage>3%</style> <style=cStack>(+3% per stack)</style> and damage by <style=cIsDamage>150%</style> <style=cStack>(+50% per stack)</style>. Maximum cap of <style=cIsDamage>7500%</style> damage and <style=cIsDamage>50%</style> chance.", "en");

            #endregion
            #region Equipment
            //Remove, removed Movement Speed Buff
            LanguageAPI.Add("EQUIPMENT_JETPACK_DESC", "Sprout wings and <style=cIsUtility>fly for 15 seconds</style>.", "en");
            //Higher damage than stated
            LanguageAPI.Add("EQUIPMENT_BFG_DESC", "Fires preon tendrils, zapping enemies within 35m for up to <style=cIsDamage>1200% damage/second</style>. On contact, detonate in an enormous 20m explosion for <style=cIsDamage>8000% damage</style>.");
            //Red downside text
            LanguageAPI.Add("EQUIPMENT_CRIPPLEWARD_DESC", "<color=#FF7F7F>ALL characters</color> within have their <style=cIsUtility>movement speed slowed by 100%</style> and have their <style=cIsDamage>armor reduced by 20</style>. Can place up to <style=cIsUtility>5</style>.", "en");
            //Limit of 3
            LanguageAPI.Add("EQUIPMENT_DEATHPROJECTILE_DESC", "Throw a cursed doll out that <style=cIsDamage>triggers</style> any <style=cIsDamage>On-Kill</style> effects you have every <style=cIsUtility>1</style> second for <style=cIsUtility>8</style> seconds. Cannot throw out more than <style=cIsUtility>3</style> dolls at a time.", "en");
            //Yellow Item text
            LanguageAPI.Add("EQUIPMENT_BOSSHUNTER_DESC", "<style=cIsDamage>Execute</style> any enemy capable of spawning a <style=cIsTierBoss>unique reward</style>, and it will drop that <style=cIsDamage>item</style>. Equipment is <style=cIsUtility>consumed</style> on use.", "en");
            #endregion
        }

        internal static void CharacterText()
        {

            //Skills
            //LanguageAPI.Add("RAILGUNNER_SNIPE_CRYO_DESCRIPTION", "<style=cIsUtility>Freezing</style>.  Launch a super-cooled projectile for <style=cIsDamage>2000% damage</style>.");

            //LanguageAPI.Add("HUNTRESS_UTILITY_DESCRIPTION", "<style=cIsUtility>Agile</style>. <style=cIsUtility>Disappear</style> and <style=cIsUtility>teleport</style> forward.");
            LanguageAPI.Add("HUNTRESS_SPECIAL_DESCRIPTION", "<style=cIsUtility>Teleport</style> into the sky. Target an area to rain arrows, <style=cIsUtility>slowing</style> all enemies and dealing <style=cIsDamage>330% damage per second</style>.");
            LanguageAPI.Add("BANDIT2_SPECIAL_ALT_DESCRIPTION", "<style=cIsDamage>Slayer</style>. Fire a revolver shot for <style=cIsDamage>600% damage</style>. Kills grant <style=cIsDamage>stacking tokens</style> for a flat <style=cIsDamage>60%</style> more damage on Desperado.");

            LanguageAPI.Add("ENGI_SPECIAL_DESCRIPTION", "Place a turret that <style=cIsUtility>inherits all your items.</style> Fires a cannon for <style=cIsDamage>210% damage per second</style>. Can place up to 2.");

            LanguageAPI.Add("TOOLBOT_SPECIAL_ALT_DESCRIPTION", "Enter a heavy stance, equipping both your <style=cIsDamage>primary attacks</style> at once. Gain <style=cIsUtility>100 armor</style>, but lose <style=cIsHealth>-50% movement speed</style>.");

            LanguageAPI.Add("TREEBOT_SECONDARY_DESCRIPTION", "<style=cIsHealth>13% HP</style>. Launch a mortar into the sky for <style=cIsDamage>450% damage</style>.");
            LanguageAPI.Add("TREEBOT_UTILITY_ALT1_DESCRIPTION", "<style=cIsHealth>17% HP</style>. Fire a <style=cIsUtility>Sonic Boom</style> that <style=cIsDamage>damages</style> enemies for <style=cIsDamage>550% damage</style>. <style=cIsHealing>Heals for every target hit</style>.");

            //LanguageAPI.Add("MAGE_SPECIAL_FIRE_DESCRIPTION", "Burn all enemies in front of you for <style=cIsDamage>2000% damage</style>.");

            LanguageAPI.Add("LOADER_UTILITY_ALT1_DESCRIPTION", "<style=cIsUtility>Heavy</style>. Charge up a <style=cIsUtility>single-target</style> punch for <style=cIsDamage>2100% damage</style> that <style=cIsDamage>shocks</style> enemies in a cone for <style=cIsDamage>900% damage</style>.");


            //LanguageAPI.Add("CAPTAIN_UTILITY_ALT1_DESCRIPTION", "<style=cIsDamage>Stunning</style>. Request a <style=cIsDamage>kinetic strike</style> from the <style=cIsDamage>UES Safe Travels</style>. After <style=cIsUtility>20 seconds</style>, it deals <style=cIsDamage>40,000% damage</style> to enemies and <style=cIsDamage>20,000% damage</style> to ALL ALLIES..");

            LanguageAPI.Add("KEYWORD_WEAK", "<style=cKeywordName>Weaken</style><style=cSub>Reduce movement speed and damage by <style=cIsDamage>40%</style>. Reduce armor by <style=cIsDamage>30</style>.</style>");

            LanguageAPI.Add("SKILL_LUNAR_UTILITY_REPLACEMENT_DESCRIPTION", "Fade away, becoming <style=cIsUtility>intangible</style> and <style=cIsUtility>gaining movement speed</style>. <style=cIsHealing>Heal</style> for <style=cIsHealing>18% of your maximum health</style>.");

            LanguageAPI.Add("ITEM_LUNARUTILITYREPLACEMENT_DESC", "<style=cIsUtility>Replace your Utility Skill</style> with <style=cIsUtility>Shadowfade</style>. \n\nFade away, becoming <style=cIsUtility>intangible</style> and gaining <style=cIsUtility>+30% movement speed</style>. <style=cIsHealing>Heal</style> for <style=cIsHealing>18% <style=cStack>(+18% per stack)</style> of your maximum health</style>. Lasts 3 <style=cStack>(+3 per stack)</style> seconds.");

        }


    }

}
