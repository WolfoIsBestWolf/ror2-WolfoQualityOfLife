using R2API;
using RoR2;
//using System;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQualityOfLife
{
    public class TextChanges
    {
        public static void Main()
        {
            if (WConfig.cfgTextItems.Value)
            {
                ItemText();
            }
            if (WConfig.cfgTextCharacters.Value)
            {
                CharacterText();
            }
            if (WConfig.cfgTextMain.Value)
            {
                OtherText();
            }
           
            LanguageAPI.Add("CHEST2_NAME", "Groﬂe Kiste", "de");
            LanguageAPI.Add("VOIDCOINBARREL_NAME", "Stengel", "de");

            LanguageAPI.Add("STATNAME_TOTALLUNARPURCHASES", "Lunar Purchases", "en");
            LanguageAPI.Add("STATNAME_HIGHESTLUNARPURCHASES", "Most Lunar Purchases", "en");
            LanguageAPI.Add("STATNAME_HIGHESTBLOODPURCHASES", "Most Blood Purchases", "en");

            /*LanguageAPI.Add("LUNARWISP_BODY_NAME_LONG", "Lunar Chimera Wisp", "en");
            LanguageAPI.Add("LUNARGOLEM_BODY_NAME_LONG", "Lunar Chimera Golem", "en");
            LanguageAPI.Add("LUNAREXPLODER_BODY_NAME_LONG", "Lunar Chimera Exploder", "en");
            LanguageAPI.Add("LUNARWISP_BODY_NAME_LONG", "Mondchim‰re Wisch", "de");
            LanguageAPI.Add("LUNARGOLEM_BODY_NAME_LONG", "Mondchim‰re Golem", "de");
            LanguageAPI.Add("LUNAREXPLODER_BODY_NAME_LONG", "Mondchim‰re Sprengkapsel", "de");

            LanguageAPI.Add("LUNARWISP_BODY_NAME_SHORT", "Lunar Wisp", "en");
            LanguageAPI.Add("LUNARGOLEM_BODY_NAME_SHORT", "Lunar Golem", "en");
            LanguageAPI.Add("LUNAREXPLODER_BODY_NAME_SHORT", "Lunar Exploder", "en");
            LanguageAPI.Add("LUNARWISP_BODY_NAME_SHORT", "Mond Wisch", "de");
            LanguageAPI.Add("LUNARGOLEM_BODY_NAME_SHORT", "Mond Golem", "de");
            LanguageAPI.Add("LUNAREXPLODER_BODY_NAME_SHORT", "Mond Sprengkapsel", "de");*/


            if (WConfig.LunarChimeraNameChange.Value == "Long")
            {
                LanguageAPI.Add("LUNARWISP_BODY_NAME", "Lunar Chimera Wisp", "en");
                LanguageAPI.Add("LUNARGOLEM_BODY_NAME", "Lunar Chimera Golem", "en");
                LanguageAPI.Add("LUNAREXPLODER_BODY_NAME", "Lunar Chimera Exploder", "en");
                LanguageAPI.Add("LUNARWISP_BODY_NAME", "Mondchim‰re Wisch", "de");
                LanguageAPI.Add("LUNARGOLEM_BODY_NAME", "Mondchim‰re Golem", "de");
                LanguageAPI.Add("LUNAREXPLODER_BODY_NAME", "Mondchim‰re Sprengkapsel", "de");
            }
            else if (WConfig.LunarChimeraNameChange.Value == "Short")
            {
                LanguageAPI.Add("LUNARWISP_BODY_NAME", "Lunar Wisp", "en");
                LanguageAPI.Add("LUNARGOLEM_BODY_NAME", "Lunar Golem", "en");
                LanguageAPI.Add("LUNAREXPLODER_BODY_NAME", "Lunar Exploder", "en");
                LanguageAPI.Add("LUNARWISP_BODY_NAME", "Mond Wisch", "de");
                LanguageAPI.Add("LUNARGOLEM_BODY_NAME", "Mond Golem", "de");
                LanguageAPI.Add("LUNAREXPLODER_BODY_NAME", "Mond Sprengkapsel", "de");
            }


            if (WConfig.cfgTextEndings.Value == true)
            {
                //RoR2.LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/EscapeSequenceFailed").icon = RoR2.LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/MainEnding").icon;

                GameEndingDef ObliterationEnding = RoR2.LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/ObliterationEnding");
                ObliterationEnding.foregroundColor = ObliterationEnding.backgroundColor;

                GameEndingDef LimboEnding = RoR2.LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/LimboEnding");
                LanguageAPI.Add("GAME_RESULT_LIMBOWIN", "At Peace..", "en");
                LanguageAPI.Add("GAME_RESULT_LIMBOWIN", "In Frieden..", "de");
                LimboEnding.endingTextToken = "GAME_RESULT_LIMBOWIN";
 
                LimboEnding.backgroundColor = new Color32(227, 236, 252, 215);
                LimboEnding.foregroundColor = new Color32(232, 239, 255, 190);



                GameEndingDef VoidEnding = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/DLC1/GameModes/VoidEnding.asset").WaitForCompletion();
                LanguageAPI.Add("GAME_RESULT_VOIDWIN", "Deeper, yet Deeper..", "en");
                VoidEnding.endingTextToken = "GAME_RESULT_VOIDWIN";
                VoidEnding.showCredits = true;

                GameEndingDef MainEnding = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/MainEnding.asset").WaitForCompletion();
                GameEndingDef EscapeSequenceFailed = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/EscapeSequenceFailed.asset").WaitForCompletion();
                LanguageAPI.Add("GAME_RESULT_ESCAPEFAILED", "Failed Escape..", "en");
                EscapeSequenceFailed.endingTextToken = "GAME_RESULT_ESCAPEFAILED";
                EscapeSequenceFailed.icon = MainEnding.icon;
            }



        }

        internal static void OtherText()
        {
            //Additional Key Words
            LanguageAPI.Add("KEYWORD_SLOWING", "<style=cKeywordName>Slowing</style><style=cSub>Apply a slowing debuff reducing enemy <style=cIsUtility>movement speed</style> by <style=cIsUtility>50%</style>.</style>", "en");

            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Huntress/HuntressBodyBlink.asset").WaitForCompletion().keywordTokens = new string[] { "KEYWORD_AGILE" };
            //Arrow Rain Slows

            //Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "RoR2/Base/Merc/MercBodyFocusedAssault.asset").WaitForCompletion().keywordTokens = new string[] { "KEYWORD_STUNNING", "KEYWORD_EXPOSE" };


            LanguageAPI.Add("FROG_NAME", "Glass frog", "en");
            /*LanguageAPI.Add("FROG_CONTEXT", "Pet the glass frog", "en");
            LanguageAPI.Add("PET_FROG", "{0} pet the glass frog.", "en");
            LanguageAPI.Add("PET_FROG_2P", "You pet the glass frog.", "en");*/

 
            LanguageAPI.Add("BAZAAR_SEER_SNOWYFOREST", "<style=cWorldEvent>You dream of campfires and ice.</style>", "en");
            LanguageAPI.Add("BAZAAR_SEER_SHIPGRAVEYARD", "<style=cWorldEvent>You dream of windy cliffs.</style>", "en");
            LanguageAPI.Add("BAZAAR_SEER_DAMPCAVESIMPLE", "<style=cWorldEvent>You dream of fiery caverns.</style>", "en");


            LanguageAPI.Add("VULTUREEGG_BODY_NAME", "Vulture Egg", "en");
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/shipgraveyard/VultureEggBody.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "VULTUREEGG_BODY_NAME";


            //LanguageAPI.Add("ACIDLARVA_BODY_NAME", "Acid Larva", "en");


            Texture GenericPlanetDeath = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").GetComponent<CharacterBody>().portraitIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ExplosivePotDestructibleBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FusionCellDestructibleBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TimeCrystalBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AltarSkeletonBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SulfurPodBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody").GetComponent<CharacterBody>().subtitleNameToken = "LUNAREXPLODER_BODY_SUBTITLE";
            //LanguageAPI.Add("IMP_TINY_BODY_NAME", "Tiny Imp", "en");
            //LanguageAPI.Add("SNIPER_BODY_NAME", "Sniper", "en");
            //LanguageAPI.Add("MAJORCONSTRUCT_BODY_NAME", "Sigma Construct", "en");
            //LanguageAPI.Add("ARCHWISP_BODY_NAME", "Archaic Wisp", "en");
            //LanguageAPI.Add("ANCIENTWISP_BODY_NAME", "Ancient Wisp", "en");

            //LanguageAPI.Add("MONSTER_PICKUP", "<style=cWorldEvent>{0} scavenged for {1}{2}</color>", "en");
            LanguageAPI.Add("MONSTER_PICKUP_2P", "<style=cWorldEvent>You scavenged for {1}{2}</color>", "en");

            LanguageAPI.Add("DIFFICULTY_EASY_DESCRIPTION", "Reduces difficulty for players new to the game. Weeping and gnashing is replaced by laughter and tickles.<style=cStack>\n\n>Player Health Regeneration: <style=cIsHealing>+50%</style> \n>Player Damage Reduction: <style=cIsHealing>+40%</style> \n>Difficulty Scaling: <style=cIsHealing>-50%</style></style>", "en");



            //LanguageAPI.Add("PLAYER_DEATH_QUOTE_0_2P", "You are dead. Not big surprise.", "en");
            //LanguageAPI.Add("PLAYER_DEATH_QUOTE_0", "{0} is dead. Not big surprise.", "en");

            LanguageAPI.Add("PLAYER_DEATH_QUOTE_33_2P", "You are not OK from this encounter.", "en");
            LanguageAPI.Add("PLAYER_DEATH_QUOTE_33", "{0} is not OK from this encounter.", "en");



            LanguageAPI.Add("MAP_ARENA_TITLE", "Void Fields", "en");
            LanguageAPI.Add("MAP_VOIDSTAGE_TITLE", "Void Locus", "en");
            LanguageAPI.Add("MAP_VOIDRAID_TITLE", "The Planetarium", "en");


            //LanguageAPI.Add("MAP_BAZAAR_SUBTITLE", "", "en");
            LanguageAPI.Add("MAP_GOLDSHORES_SUBTITLE", "Beautiful Cage", "en");
            //LanguageAPI.Add("MAP_MYSTERYSPACE_SUBTITLE", "", "en");
            //LanguageAPI.Add("MAP_LIMBO_SUBTITLE", "", "en");
            LanguageAPI.Add("MAP_ARTIFACTWORLD_SUBTITLE", "Sacred Treasures", "en");

            //Objectives
            LanguageAPI.Add("OBJECTIVE_GOLDSHORES_ACTIVATE_BEACONS", "Rebuild the <color=#FFE880>Halcyon Beacons</color> ({0}/{1})", "en");
            LanguageAPI.Add("OBJECTIVE_CLEAR_ARENA", "Activate all <style=cIsVoid>Cell Vents</style> ({0}/{1})", "en");
            LanguageAPI.Add("OBJECTIVE_ARENA_CHARGE_CELL", "Breach the <style=cIsVoid>Cell</style> ({0}%)", "en");


            //Updated Interactable Names
            LanguageAPI.Add("DRONE_MEGA_CONTEXT", "Repair TC-280", "en");
            LanguageAPI.Add("LOCKEDTREEBOT_CONTEXT", "Repair the survivor", "en");
            LanguageAPI.Add("LOCKEDMAGE_NAME", "Frozen Survivor", "en");

            //Context is completely fucking random whether it captializes all words or none.
            //LanguageAPI.Add("CHEST2_CONTEXT", "Open Large Chest", "en");

            LanguageAPI.Add("MULTISHOP_LARGE_TERMINAL_NAME", "Large Multishop Terminal", "en");
            //LanguageAPI.Add("MULTISHOP_LARGE_TERMINAL_CONTEXT", "Open terminal", "en");
            LanguageAPI.Add("MULTISHOP_EQUIPMENT_TERMINAL_NAME", "Equipment Shop Terminal", "en");
            //LanguageAPI.Add("MULTISHOP_EQUIPMENT_TERMINAL_CONTEXT", "Open terminal", "en");
            LanguageAPI.Add("DUPLICATOR_LARGE_NAME", "Large 3D Printer", "en"); //Large or Uncommon idk
            LanguageAPI.Add("DUPLICATOR_LARGE_CONTEXT", "Use Large 3D Printer", "en"); //Like we make em Large


            LanguageAPI.Add("FREECHEST_TERMINAL_NAME", "Shipping Terminal", "en");
            LanguageAPI.Add("FREECHEST_TERMINAL_CONTEXT", "Accept delivery.", "en");


            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab").WaitForCompletion().GetComponent<RoR2.PurchaseInteraction>().displayNameToken = "FREECHEST_TERMINAL_NAME";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab").WaitForCompletion().GetComponent<RoR2.PurchaseInteraction>().contextToken = "FREECHEST_TERMINAL_CONTEXT";


            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopLargeTerminal").GetComponent<RoR2.PurchaseInteraction>().displayNameToken = "MULTISHOP_LARGE_TERMINAL_NAME";
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopLargeTerminal").GetComponent<RoR2.PurchaseInteraction>().contextToken = "MULTISHOP_LARGE_TERMINAL_CONTEXT";
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopEquipmentTerminal").GetComponent<RoR2.PurchaseInteraction>().displayNameToken = "MULTISHOP_EQUIPMENT_TERMINAL_NAME";
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/MultiShopEquipmentTerminal").GetComponent<RoR2.PurchaseInteraction>().contextToken = "MULTISHOP_EQUIPMENT_TERMINAL_CONTEXT";

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").GetComponent<RoR2.PurchaseInteraction>().displayNameToken = "DUPLICATOR_LARGE_NAME";
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge").GetComponent<RoR2.PurchaseInteraction>().contextToken = "DUPLICATOR_LARGE_CONTEXT";
            //
        }

        internal static void ItemText()
        {
            //Corrected Items and Equipment
            LanguageAPI.Add("ITEM_ANCESTRALINCUBATOR_DESC", "<style=cIsDamage>7%</style> chance <style=cStack>(+1% per stack)</style> on kill to <style=cIsUtility>summon an Ancestral Pod</style> that distracts enemies. \nOnce it fully grows, it will hatch into an allied <style=cIsDamage>Parent</style> with <style=cIsHealing>200% health</style> <style=cStack>(+100% per stack)</style> and with <style=cIsDamage>400% damage</style>. <style=cIsDamage>Parents</style> limited to 1 <style=cStack>(+1 per stack)</style>", "en");

            LanguageAPI.Add("ITEM_SHINYPEARL_DESC", "Increases <style=cIsDamage>damage</style>, <style=cIsDamage>attack speed</style>, <style=cIsDamage>critical strike chance</style>, <style=cIsHealing>maximum health</style>, <style=cIsHealing>base health regeneration</style>, <style=cIsHealing>base armor</style>, <style=cIsUtility>movement speed</style> by <style=cIsDamage>1<style=cIsHealing>0<style=cIsUtility>%<style=cStack> (+10% per stack)</style></style></style></style>", "en");


            LanguageAPI.Add("ITEM_SCRAPWHITE_PICKUP", "Does nothing. Prioritized when used with <color=#FAFAFA>3D Printers</color>.", "en");
            LanguageAPI.Add("ITEM_SCRAPGREEN_PICKUP", "Does nothing. Prioritized when used with <style=cIsHealing>Large 3D Printers</style>.", "en");
            LanguageAPI.Add("ITEM_SCRAPRED_PICKUP", "Does nothing. Prioritized when used with <style=cIsHealth>Mili-Tech 3D Printers</style>.", "en");
            LanguageAPI.Add("ITEM_SCRAPYELLOW_PICKUP", "Does nothing. Prioritized when used with <style=cIsTierBoss>Overgrown 3D Printers.</style>.</style>", "en");

            LanguageAPI.Add("ITEM_SCRAPWHITE_DESC", "Does nothing. Prioritized when used with <color=#FAFAFA>3D Printers</color> and Cauldrons.", "en");
            LanguageAPI.Add("ITEM_SCRAPGREEN_DESC", "Does nothing. Prioritized when used with <style=cIsHealing>Large 3D Printers</style> and Cauldrons.", "en");
            LanguageAPI.Add("ITEM_SCRAPRED_DESC", "Does nothing. Prioritized when used with <style=cIsHealth>Mili-Tech 3D Printers</style> and Cauldrons.", "en");
            LanguageAPI.Add("ITEM_SCRAPYELLOW_DESC", "Does nothing. Prioritized when used with <style=cIsTierBoss>Overgrown 3D Printers</style>.", "en");



            LanguageAPI.Add("EQUIPMENT_BOSSHUNTER_PICKUP", "Execute a large monster and claim its <style=cIsTierBoss>trophy</style>. Consumed on use.", "en");
            LanguageAPI.Add("EQUIPMENT_BOSSHUNTER_DESC", "<style=cIsDamage>Execute</style> any enemy capable of spawning a <style=cIsTierBoss>unique reward</style>, and it will drop that <style=cIsDamage>item</style>. Equipment is <style=cIsUtility>consumed</style> on use.", "en");


            LanguageAPI.Add("ITEM_MISSILEVOID_DESC", "Gain a <style=cIsHealing>shield</style> equal to <style=cIsHealing>10%</style> of your maximum health. While you have a <style=cIsHealing>shield</style>, hitting an enemy fires a missile that deals <style=cIsDamage>40%</style> <style=cStack>(+40% per stack)</style> TOTAL damage. <style=cIsVoid>Corrupts all AtG Missile Mk. 1s</style>.", "en");
            //LanguageAPI.Add("ITEM_CLOVERVOID_DESC", "<style=cIsUtility>Upgrades 3</style> <style=cStack>(+3 per stack)</style> random stacks of items to items of the next <style=cIsUtility>higher rarity</style> at the <style=cIsUtility>start of each stage</style>. <style=cIsVoid>Corrupts all 57 Leaf Clovers</style>.", "en");
            LanguageAPI.Add("ITEM_EQUIPMENTMAGAZINEVOID_PICKUP", "Add an extra charge of your Special skill. Reduce Special skill cooldown. <style=cIsVoid>Corrupts all Fuel Cells</style>.", "en");
            LanguageAPI.Add("ITEM_EQUIPMENTMAGAZINEVOID_DESC", "Add <style=cIsUtility>+1</style> <style=cStack>(+1 per stack)</style> charge of your <style=cIsUtility>Special skill</style>. <style=cIsUtility>Reduces Special skill cooldown</style> by <style=cIsUtility>33%</style>. <style=cIsVoid>Corrupts all Fuel Cells</style>.", "en");
            //LanguageAPI.Add("ITEM_ELEMENTALRINGVOID_DESC", "Hits that deal <style=cIsDamage>more than 400% damage</style> also fire a black hole that <style=cIsUtility>draws enemies within 15m into its center</style>. Lasts <style=cIsUtility>5</style> seconds before collapsing, dealing <style=cIsDamage>100%</style> <style=cStack>(+100% per stack)</style> TOTAL damage. Recharges every <style=cIsUtility>20</style> seconds. <style=cIsVoid>Corrupts all Runald's and Kjaro's Bands.</style>.", "en");
            //LanguageAPI.Add("ITEM_VOIDMEGACRABITEM_DESC", "Every <style=cIsUtility>60</style><style=cStack>(-50% per stack)</style> seconds, gain a random <style=cIsVoid>Void</style> ally. Can have up to <style=cIsUtility>1</style><style=cStack>(+1 per stack)</style> allies at a time. <style=cIsVoid>Corrupts all </style><style=cIsTierBoss>yellow items</style><style=cIsVoid>.</style>", "en");
            //LanguageAPI.Add("EQUIPMENT_GUMMYCLONE_DESC", "Spawn a gummy clone that has <style=cIsDamage>170% damage</style> and <style=cIsHealing>170% health</style>. Expires in <style=cIsUtility>30</style> seconds.", "en");
            LanguageAPI.Add("EQUIPMENT_BOSSHUNTERCONSUMED_DESC", "Exclaim an Ahoy!", "en");
            LanguageAPI.Add("ITEM_TREASURECACHEVOID_DESC", "A <style=cIsUtility>hidden cache</style> containing an item (42%/<style=cIsHealing>42%</style>/<style=cIsHealth>16%</style>) will appear in a random location <style=cIsUtility>on each stage</style>. Opening the cache <style=cIsUtility>consumes</style> this item. <style=cIsVoid>Corrupts all Rusted Keys</style>.", "en");
            //LanguageAPI.Add("text", "text", "en");



            //LanguageAPI.Add("ITEM_WARCRYONMULTIKILL_PICKUP", "Enter a frenzy after killing 4 enemies in quick succession.", "en");
            //LanguageAPI.Add("ITEM_WARCRYONMULTIKILL_DESC", "<style=cIsDamage>Killing 4 enemies</style> within <style=cIsDamage>1</style> second sends you into a <style=cIsDamage>frenzy</style> for <style=cIsDamage>6s</style> <style=cStack>(+4s per stack)</style>. Increases <style=cIsUtility>movement speed</style> by <style=cIsUtility>50%</style> and <style=cIsDamage>attack speed</style> by <style=cIsDamage>100%</style>.", "en");

            LanguageAPI.Add("ITEM_NOVAONLOWHEALTH_DESC", "Falling below <style=cIsHealth>25% health</style> causes you to explode, dealing <style=cIsDamage>6000% base damage</style>. Recharges every <style=cIsUtility>15 seconds</style> <style=cStack>(-33% per stack)</style>.");
            LanguageAPI.Add("ITEM_LIGHTNINGSTRIKEONHIT_DESC", "<style=cIsDamage>10%</style> chance on hit to down a lightning strike, dealing <style=cIsDamage>500%</style> <style=cStack>(+500% per stack)</style> TOTAL damage.");
            LanguageAPI.Add("ITEM_FIREBALLSONHIT_DESC", "<style=cIsDamage>10%</style> chance on hit to call forth <style=cIsDamage>3 magma balls</style> from an enemy, dealing <style=cIsDamage>300%</style> <style=cStack>(+300% per stack)</style> TOTAL damage and <style=cIsDamage>igniting</style> all enemies for an additional <style=cIsDamage>50%</style> TOTAL damage over time.");
            LanguageAPI.Add("ITEM_BEETLEGLAND_DESC", "<style=cIsUtility>Summon a Beetle Guard</style> with bonus <style=cIsDamage>300%</style> damage and <style=cIsHealing>100% health</style>. Can have up to <style=cIsUtility>1</style> <style=cStack>(+1 per stack)</style> Guards at a time.");


            LanguageAPI.Add("ITEM_BLEEDONHIT_DESC", "<style=cIsDamage>10%</style> <style=cStack>(+10% per stack)</style> chance to <style=cIsDamage>bleed</style> an enemy for <style=cIsDamage>240%</style> base damage. Inflicting <style=cIsDamage>bleed</style> refreshes all stacks.", "en");

            //LanguageAPI.Add("BODY_MODIFIER_ECHO", "{0} Echo", "en");
            //LanguageAPI.Add("ELITE_MODIFIER_SECRETSPEED", "Furry {0}", "en");


            LanguageAPI.Add("ITEM_FRAGILEDAMAGEBONUSCONSUMED_DESC", "...well, it's still right twice a day.", "en");
            LanguageAPI.Add("ITEM_HEALINGPOTIONCONSUMED_DESC", "An empty container for an Elixir. Does nothing.", "en");


            LanguageAPI.Add("ITEM_INTERSTELLARDESKPLANT_DESC", "On kill, plant a <style=cIsHealing>healing</style> fruit seed that grows into a plant after <style=cIsUtility>5</style> seconds. \n\nThe plant <style=cIsHealing>heals</style> for <style=cIsHealing>10%</style> of <style=cIsHealing>maximum health</style> every second to all allies within <style=cIsHealing>10m</style> <style=cStack>(+5.0m per stack)</style>. Lasts <style=cIsUtility>10</style> seconds.", "en");
            LanguageAPI.Add("ITEM_MUSHROOM_DESC", "After standing still for <style=cIsHealing>1</style> second, create a zone that <style=cIsHealing>heals</style> for <style=cIsHealing>4.5%</style> <style=cStack>(+2.25% per stack)</style> of your <style=cIsHealing>health</style> every second to all allies within <style=cIsHealing>3.5m</style> <style=cStack>(+1.5m per stack)</style>.", "en");
            LanguageAPI.Add("ITEM_BEHEMOTH_DESC", "All your <style=cIsDamage>attacks explode</style> in a <style=cIsDamage>4m </style><style=cStack>(+2.5m per stack)</style> radius for a bonus <style=cIsDamage>60%</style> TOTAL damage to nearby enemies.", "en");


            LanguageAPI.Add("ITEM_ATTACKSPEEDONCRIT_DESC", "Gain <style=cIsDamage>5% critical chance</style>. <style=cIsDamage>Critical strikes</style> increase <style=cIsDamage>attack speed</style> by <style=cIsDamage>12%</style>. Maximum cap of <style=cIsDamage>36% <style=cStack>(+24% per stack)</style> attack speed</style>.", "en");
            LanguageAPI.Add("ITEM_BLEEDONHITANDEXPLODE_DESC", "Gain <style=cIsDamage>5% critical chance</style>. <style=cIsDamage>Critical Strikes bleed</style> enemies for <style=cIsDamage>240%</style> base damage. <style=cIsDamage>Bleeding</style> enemies <style=cIsDamage>explode</style> on death for <style=cIsDamage>400%</style> <style=cStack>(+400% per stack)</style> damage, plus an additional <style=cIsDamage>15%</style> <style=cStack>(+15% per stack)</style> of their maximum health.", "en");

            LanguageAPI.Add("EQUIPMENT_JETPACK_DESC", "Sprout wings and <style=cIsUtility>fly for 15 seconds</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_BFG_DESC", "Fires preon tendrils, zapping enemies within 35m for up to <style=cIsDamage>1200% damage/second</style>. On contact, detonate in an enormous 20m explosion for <style=cIsDamage>8000% damage</style>.");

            LanguageAPI.Add("EQUIPMENT_CRIPPLEWARD_DESC", "<color=#FF7F7F>ALL characters</color> within have their <style=cIsUtility>movement speed slowed by 100%</style> and have their <style=cIsDamage>armor reduced by 20</style>. Can place up to <style=cIsUtility>5</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_DEATHPROJECTILE_DESC", "Throw a cursed doll out that <style=cIsDamage>triggers</style> any <style=cIsDamage>On-Kill</style> effects you have every <style=cIsUtility>1</style> second for <style=cIsUtility>8</style> seconds. Cannot throw out more than <style=cIsUtility>3</style> dolls at a time.", "en");


            //New Descriptions
            LanguageAPI.Add("ITEM_TONICAFFLICTION_DESC", "<color=#FF7F7F>Reduce ALL stats by -5% </color><style=cStack>(-5% per stack)</style><color=#FF7F7F> when not under the effects of Spinel Tonic</color>", "en");
            LanguageAPI.Add("EQUIPMENT_QUESTVOLATILEBATTERY_DESC", "Falling below <style=cIsHealth>50% health</style> causes this to explode in a large radius dealing <style=cIsDamage>300% the carriers maximum health as damage</style>. Consumed on usage.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXRED_DESC", "Attacks <style=cIsDamage>ignite</style> enemies dealing <style=cIsDamage>50%</style> TOTAL damage over time. Leave behind a <style=cIsDamage>fire trail</style> dealing damage on contact.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXBLUE_DESC", "All attacks leave a <style=cIsDamage>lightning orb</style> which explodes after <style=cIsUtility>1.5s</style> dealing <style=cIsDamage>50%</style> TOTAL damage. Convert <style=cIsHealing>50%</style> of your <style=cIsHealing>health</style> into <style=cIsHealing>shields</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXWHITE_DESC", "<style=cIsUtility>Slow</style> enemies on hit for <style=cIsUtility>-80% movement speed</style> for <style=cIsUtility>1.5s</style>. <style=cDeath>On death</style> leave an <style=cIsDamage>ice explosion</style> <style=cIsUtility>freezing</style> enemies for <style=cIsUtility>1.5s</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXPOISON_DESC", "Enemies hit will have their <style=cIsHealing>healing disabled</style> for <style=cIsUtility>8s</style>. Periodically shoot out 3 <style=cIsDamage>spike clusters</style> dealing <style=cIsDamage>100%</style> base damage. <style=cDeath>On death</style> leave a <style=cIsDamage>Malachite Urchin</style> lasting <style=cIsUtility>20s</style>.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXHAUNTED_DESC", "<style=cIsUtility>Slow</style> enemies on hit for <style=cIsUtility>-80% movement speed</style> for <style=cIsUtility>3s</style>. <style=cIsUtility>Cloak</style> all allies in a large radius.", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXLUNAR_DESC", "Increases <style=cIsHealing>maximum health</style> by <style=cIsHealing>25%</style> and <style=cIsUtility>movement speed</style> by <style=cIsUtility>30%</style>. \nConvert all <style=cIsHealing>health</style> into <style=cIsHealing>regenerating shields</style>. \n<style=cIsDamage>Cripple</style> enemies on hit for <style=cIsUtility>4s</style> reducing <style=cIsUtility>movement speed</style> by <style=cIsUtility>100%</style> and <style=cIsDamage>armor</style> by <style=cIsDamage>20</style>. \nPeriodically shoot out 4 orbs dealing <style=cIsDamage>30%</style> base damage when attacking. ", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXEARTH_DESC", "<style=cIsHealing>Heal</style> a nearby hurt ally for <style=cIsHealing>120%</style> of your <style=cIsDamage>base damage</style> per second. <style=cDeath>On death</style> leave a <style=cIsHealing>Healing Core</style> which will <style=cIsHealing>heal</style> all creatures for <style=cIsHealing>80</style> health.", "en");


            //LanguageAPI.Add("ITEM_DRONEWEAPONS_DESC", "Gain <style=cIsDamage>Col. Droneman</style>. \nDrones gain <style=cIsDamage>+50%</style> <style=cStack>(+50% per stack)</style> attack speed and cooldown reduction. \nDrones gain <style=cIsDamage>10%</style> chance to fire a <style=cIsDamage>missile</style> on hit, dealing <style=cIsDamage>300%</style> base damage. \nDrones gain an <style=cIsDamage>automatic chain gun</style> that deals <style=cIsDamage>6x100%</style>damage, bouncing to <style=cIsDamage>2</style> enemies.", "en");

            //CostTypeCatalog.

            //Untiered Item Descriptions
            //
            //ALL untiered items I suppose
            LanguageAPI.Add("ITEM_GUMMYCLONEIDENTIFIER_NAME", "Gummy Clone Identifier", "en");
            LanguageAPI.Add("ITEM_GUMMYCLONEIDENTIFIER_PICKUP", "Turn into a Gummy", "en");
            LanguageAPI.Add("ITEM_GUMMYCLONEIDENTIFIER_DESC", "Visually turns you into a Gummy character created by Goobo Jr.", "en");

            LanguageAPI.Add("ITEM_EMPOWERALWAYS_NAME", "Empower Always", "en");
            LanguageAPI.Add("ITEM_EMPOWERALWAYS_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_EMPOWERALWAYS_DESC", "A unfinished item that does nothing.", "en");

            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY1_NAME", "Drone Weapons Common Display", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY1_PICKUP", "Visuals only for Drones.", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY1_DESC", "Spare Drone Parts common Item Display for Drones.", "en");

            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY2_NAME", "Drone Weapons Rare Display", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY2_PICKUP", "Visuals only for Drones.", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSDISPLAY2_DESC", "Spare Drone Parts rare Item Display for Drones.", "en");

            LanguageAPI.Add("ITEM_DRONEWEAPONSBOOST_NAME", "Drone Weapons Stat Boost", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSBOOST_PICKUP", "Bonus stats granted by Spare Drone Parts.", "en");
            LanguageAPI.Add("ITEM_DRONEWEAPONSBOOST_DESC", "Gain <style=cIsDamage>+50%</style><style=cStack>(+50% per stack)</style> <style=cIsDamage>attack speed</style> and <style=cIsDamage>skill cooldown reduction</style>, a <style=cIsDamage>10%</style> chance to fire a <style=cIsDamage>Micro Missile</style> dealing <style=cIsDamage>300%</style> base damage and gain an <style=cIsDamage>automatic chain gun</style> that deals <style=cIsDamage>6x100%</style>damage, bouncing to <style=cIsDamage>2</style> enemies.", "en");

            LanguageAPI.Add("ITEM_CONVERTCRITCHANCETOCRITDAMAGE_NAME", "Convert Crit Chance", "en");
            LanguageAPI.Add("ITEM_CONVERTCRITCHANCETOCRITDAMAGE_PICKUP", "Convert all crit chance into crit damage multiplier", "en");
            LanguageAPI.Add("ITEM_CONVERTCRITCHANCETOCRITDAMAGE_DESC", "All <style=cIsDamage>crit chance</style> will be converted into <style=cIsDamage>crit damage multiplier</style> making them deal more damage. You will be unable to deal <style=cIsDamage>Critical Strikes</style> randomly.", "en");



            LanguageAPI.Add("ITEM_BURNNEARBY_NAME", "Burn Nearby", "en");
            LanguageAPI.Add("ITEM_BURNNEARBY_PICKUP", "(Old Helfire Tincture)Permament Helfire Tincture", "en");
            LanguageAPI.Add("ITEM_BURNNEARBY_DESC", "Gain a permament <color=#307FFF>Helfire Tincture</color> burn", "en");

            LanguageAPI.Add("ITEM_CRIPPLEWARDONLEVEL_NAME", "Cripple Ward on Level", "en");
            LanguageAPI.Add("ITEM_CRIPPLEWARDONLEVEL_PICKUP", "(Old Effigy of Grief) Drop a crippling effigy upon enemy level up", "en");
            LanguageAPI.Add("ITEM_CRIPPLEWARDONLEVEL_DESC", "When the enemy team level ups, summon a <color=#307FFF>Effigy of Grief</color> <style=cIsUtility>cripple</style> zone <style=cIsHealth>at your location</style>", "en");

            LanguageAPI.Add("ITEM_CRITHEAL_NAME", "Crit Healing", "en");
            LanguageAPI.Add("ITEM_CRITHEAL_PICKUP", "(Old Corpsebloom) Chance to double healing", "en");
            LanguageAPI.Add("ITEM_CRITHEAL_DESC", "When <style=cIsHealing>healing</style>, have a chance equivelent to your <style=cIsDamage>critical rate</style> to <style=cIsHealing>double the amount</style>. \nGain <style=cIsDamage>5% critical chance</style> and then <style=cIsDamage>halve</style><style=cStack> (-33% per stack)</style> your <style=cIsDamage>critical chance</style>", "en");

            LanguageAPI.Add("ITEM_WARCRYONCOMBAT_NAME", "Warcry on Combat", "en");
            LanguageAPI.Add("ITEM_WARCRYONCOMBAT_PICKUP", "(Old Berzerker's Pauldron) Enter a frenzy at the beginning of combat.", "en");
            LanguageAPI.Add("ITEM_WARCRYONCOMBAT_DESC", "When entering combat emmit a <style=cIsDamage>War Cry</style> for <style=cIsDamage>6 seconds</style><style=cStack> (+4s per stack)</style> buffing you and allies in a <style=cIsDamage>12m radius</style><style=cStack> (+4m per stack)</style> increasing <style=cIsUtility>movement speed</style> by <style=cIsUtility>50%</style> and <style=cIsDamage>attack speed</style> by <style=cIsDamage>100%</style>. Recharges every 30 seconds.", "en");

            LanguageAPI.Add("ITEM_TEMPESTONKILL_NAME", "Tempest On Kill", "en");
            LanguageAPI.Add("ITEM_TEMPESTONKILL_PICKUP", "(Old Wax Quail) Chance to summon a buffing tempest on kill", "en");
            LanguageAPI.Add("ITEM_TEMPESTONKILL_DESC", "<style=cIsUtility>25%</style> chance on kill to spawn a <style=cIsUtility>buffing tempest</style> which will buff you with <style=cIsDamage>Malachite Elite</style> for <style=cIsDamage>2 seconds</style> and expire after <style=cIsUtility>8<style=cStack> (+6 per stack)</style> seconds</style>", "en");

            LanguageAPI.Add("ITEM_MINIONLEASH_NAME", "Minion Leash", "en");
            LanguageAPI.Add("ITEM_MINIONLEASH_PICKUP", "Teleport to your master if he is too far away.", "en");
            LanguageAPI.Add("ITEM_MINIONLEASH_DESC", "You are on a leashed to your master. Teleport you back to him if too far away. If you have no master to be leashed to, lag the game instead.", "en");


            LanguageAPI.Add("ITEM_AACANNON_NAME", "AACannon", "en");
            LanguageAPI.Add("ITEM_AACANNON_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_AACANNON_DESC", "A unfinished item that does nothing.", "en");

            LanguageAPI.Add("ITEM_PLASMACORE_NAME", "Plasma Core", "en");
            LanguageAPI.Add("ITEM_PLASMACORE_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_PLASMACORE_DESC", "A unfinished item that does nothing.", "en");

            LanguageAPI.Add("ITEM_PLANTONHIT_NAME", "Plant On Hit", "en");
            LanguageAPI.Add("ITEM_PLANTONHIT_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_PLANTONHIT_DESC", "A unfinished item that does nothing. Potentially early version of Deskplant.", "en");

            LanguageAPI.Add("ITEM_MAGEATTUNEMENT_NAME", "Artificer Attunement", "en");
            LanguageAPI.Add("ITEM_MAGEATTUNEMENT_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_MAGEATTUNEMENT_DESC", "A unfinished idea that does not function.", "en");

            LanguageAPI.Add("ITEM_GHOST_NAME", "Ghost", "en");
            LanguageAPI.Add("ITEM_GHOST_PICKUP", "Become a Ghost without a hitbox.", "en");
            LanguageAPI.Add("ITEM_GHOST_DESC", "Become permamently <style=cIsUtility>immune to damage</style> and <style=cIsUtility>undetectable</style> if you don't deal damage", "en");



            LanguageAPI.Add("ITEM_BOOSTATTACKSPEED_NAME", "Boost Attack Speed", "en");
            LanguageAPI.Add("ITEM_BOOSTATTACKSPEED_PICKUP", "Increases attack speed.", "en");
            LanguageAPI.Add("ITEM_BOOSTATTACKSPEED_DESC", "Increases <style=cIsDamage>attack speed</style> by <style=cIsDamage>10% <style=cStack>(+10% per stack)</style></style>.", "en");

            LanguageAPI.Add("ITEM_BOOSTDAMAGE_NAME", "Boost Damage", "en");
            LanguageAPI.Add("ITEM_BOOSTDAMAGE_PICKUP", "Increases damage.", "en");
            LanguageAPI.Add("ITEM_BOOSTDAMAGE_DESC", "Increases <style=cIsDamage>damage</style> by <style=cIsDamage>10% <style=cStack>(+10% per stack)</style></style>.", "en");

            LanguageAPI.Add("ITEM_BOOSTHP_NAME", "Boost Health", "en");
            LanguageAPI.Add("ITEM_BOOSTHP_PICKUP", "Increases health.", "en");
            LanguageAPI.Add("ITEM_BOOSTHP_DESC", "Increases <style=cIsHealing>maximum health</style> by <style=cIsHealing>10% <style=cStack>(+10% per stack)</style></style>.", "en");

            LanguageAPI.Add("ITEM_CUTHP_NAME", "Half Health", "en");
            LanguageAPI.Add("ITEM_CUTHP_PICKUP", "Halves your health.", "en");
            LanguageAPI.Add("ITEM_CUTHP_DESC", "Reduce <style=cIsHealing>maximum health</style> <style=cIsHealing>by 50%</style> <style=cStack>(+50% per stack)</style>.", "en");


            LanguageAPI.Add("ITEM_SKULLCOUNTER_DESC", "Proof of a specific kind of kill. Has no direct effect of its own. \nPreviously used for Bandits Desperado skill", "en");



            LanguageAPI.Add("ITEM_BOOSTEQUIPMENTRECHARGE_NAME", "Reduce Equipment Cooldown", "en");
            LanguageAPI.Add("ITEM_BOOSTEQUIPMENTRECHARGE_PICKUP", "Reduce equipment cooldown.", "en");
            LanguageAPI.Add("ITEM_BOOSTEQUIPMENTRECHARGE_DESC", "<style=cIsUtility>Reduce equipment cooldown</style> by <style=cIsUtility>10%</style> <style=cStack>(+10% per stack)</style>.", "en");

            LanguageAPI.Add("ITEM_ADAPTIVEARMOR_NAME", "Adaptive Armor", "en");
            LanguageAPI.Add("ITEM_ADAPTIVEARMOR_PICKUP", "Gain temporary Armor on hit.", "en");
            LanguageAPI.Add("ITEM_ADAPTIVEARMOR_DESC", "Gain upwards of <style=cIsUtility>400 Armor</style> depending on the amount of damage taken recently", "en");


            LanguageAPI.Add("ITEM_INVADINGDOPPELGANGER_NAME", "Invading Umbra", "en");
            LanguageAPI.Add("ITEM_INVADINGDOPPELGANGER_PICKUP", "Become an Umbra.", "en");
            LanguageAPI.Add("ITEM_INVADINGDOPPELGANGER_DESC", "Reduce <style=cIsDamage>damage</style> by <style=cIsDamage>96%</style> and increase <style=cIsHealing>maximum health</style> by <style=cIsHealing>900%</style>. ", "en");

            LanguageAPI.Add("ITEM_MONSOONPLAYERHELPER_NAME", "Monsoon Helper", "en");
            LanguageAPI.Add("ITEM_MONSOONPLAYERHELPER_PICKUP", "Apply Monsoon difficulty stat changes.", "en");
            LanguageAPI.Add("ITEM_MONSOONPLAYERHELPER_DESC", "Reduce <style=cIsHealing>base health regeneration</style> by <style=cIsHealing>40%</style>. ", "en");

            LanguageAPI.Add("ITEM_DRIZZLEPLAYERHELPER_NAME", "Drizzle Helper", "en");
            LanguageAPI.Add("ITEM_DRIZZLEPLAYERHELPER_PICKUP", "Apply Drizzle difficulty stat changes.", "en");
            LanguageAPI.Add("ITEM_DRIZZLEPLAYERHELPER_DESC", "Increase <style=cIsHealing>base health regeneration</style> by <style=cIsHealing>50%</style> and <style=cIsUtility>armor</style> by <style=cIsUtility>70 <style=cStack>(+70% per stack)</style></style>. ", "en");

            LanguageAPI.Add("ITEM_HEALTHDECAY_NAME", "Health Decay", "en");
            LanguageAPI.Add("ITEM_HEALTHDECAY_PICKUP", "Rapidly drain your health", "en");
            LanguageAPI.Add("ITEM_HEALTHDECAY_DESC", "<style=cIsHealth>Die</style> in <style=cIsHealth>1 second<style=cStack> (+1 second per stack)</style></style>.", "en");

            LanguageAPI.Add("ITEM_LEVELBONUS_NAME", "Bonus Level", "en");
            LanguageAPI.Add("ITEM_LEVELBONUS_PICKUP", "Increase your level", "en");
            LanguageAPI.Add("ITEM_LEVELBONUS_DESC", "Gain <style=cIsUtility>1 Level</style><style=cStack> (+1 per stack)</style>. ", "en");


            LanguageAPI.Add("ITEM_TEAMSIZEDAMAGEBONUS_NAME", "Team Damage Boost", "en");
            LanguageAPI.Add("ITEM_TEAMSIZEDAMAGEBONUS_PICKUP", "Increase damage per owned minion", "en");
            LanguageAPI.Add("ITEM_TEAMSIZEDAMAGEBONUS_DESC", "Increases <style=cIsDamage>damage</style> by <style=cIsDamage>100% <style=cStack>(+100% per stack)</style></style> for every <style=cIsUtility>minion</style> you own.", "en");

            LanguageAPI.Add("ITEM_USEAMBIENTLEVEL_NAME", "Use Ambient Level", "en");
            LanguageAPI.Add("ITEM_USEAMBIENTLEVEL_PICKUP", "Use the Ambient level as your level", "en");
            LanguageAPI.Add("ITEM_USEAMBIENTLEVEL_DESC", "Your <style=cIsUtility>level</style> is equal or greater than the <style=cIsUtility>ambient level</style>", "en");

            LanguageAPI.Add("ITEM_SUMMONEDECHO_NAME", "Summoned Echo", "en");
            LanguageAPI.Add("ITEM_SUMMONEDECHO_PICKUP", "Become an Echo", "en");
            LanguageAPI.Add("ITEM_SUMMONEDECHO_DESC", "Reduce <style=cIsHealing>maximum health</style> by <style=cIsHealing>90%</style> and increase <style=cIsUtility>base movement speed</style> by <style=cIsUtility>66%</style>. Periodically shoot out <style=cIsDamage>homing projectiles</style> dealing <style=cIsDamage>275%</style> base damage and <style=cIsUtility>slowing</style> enemies on hit by <style=cIsUtility>50%</style> for <style=cIsUtility>2</style> seconds.", "en");



            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TeamSizeDamageBonus").nameToken = "ITEM_TEAMSIZEDAMAGEBONUS_NAME";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TeamSizeDamageBonus").pickupToken = "ITEM_TEAMSIZEDAMAGEBONUS_PICKUP";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TeamSizeDamageBonus").descriptionToken = "ITEM_TEAMSIZEDAMAGEBONUS_DESC";

            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/UseAmbientLevel").nameToken = "ITEM_USEAMBIENTLEVEL_NAME";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/UseAmbientLevel").pickupToken = "ITEM_USEAMBIENTLEVEL_PICKUP";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/UseAmbientLevel").descriptionToken = "ITEM_USEAMBIENTLEVEL_DESC";

            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/SummonedEcho").nameToken = "ITEM_SUMMONEDECHO_NAME";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/SummonedEcho").pickupToken = "ITEM_SUMMONEDECHO_PICKUP";
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/SummonedEcho").descriptionToken = "ITEM_SUMMONEDECHO_DESC";




            ItemDef MinHealthPercentage = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Base/MinHealthPercentage/MinHealthPercentage.asset").WaitForCompletion();
            LanguageAPI.Add("ITEM_MINHEALTHPERCENTAGE_NAME", "Minimum Health Percentage", "en");
            LanguageAPI.Add("ITEM_MINHEALTHPERCENTAGE_PICKUP", "Prevent HP from going below an amount", "en");
            LanguageAPI.Add("ITEM_MINHEALTHPERCENTAGE_DESC", "<style=cIsHealing>Health</style> will not go below <style=cIsHealing>1%<style=cStack> (+1% per stack)", "en");
            MinHealthPercentage.nameToken = "ITEM_MINHEALTHPERCENTAGE_NAME";
            MinHealthPercentage.pickupToken = "ITEM_MINHEALTHPERCENTAGE_PICKUP";
            MinHealthPercentage.descriptionToken = "ITEM_MINHEALTHPERCENTAGE_DESC";

            ItemDef VoidmanPassiveItem = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC1/VoidSurvivor/VoidmanPassiveItem.asset").WaitForCompletion();
            LanguageAPI.Add("ITEM_VOIDMANPASSIVEITEM_NAME", "Void Fiend Passive Item", "en");
            LanguageAPI.Add("ITEM_VOIDMANPASSIVEITEM_PICKUP", "Does nothing.", "en");
            LanguageAPI.Add("ITEM_VOIDMANPASSIVEITEM_DESC", "A item at one point responsible for Void Fiends passive, however it does nothing.", "en");
            VoidmanPassiveItem.nameToken = "ITEM_VOIDMANPASSIVEITEM_NAME";
            VoidmanPassiveItem.pickupToken = "ITEM_VOIDMANPASSIVEITEM_PICKUP";
            VoidmanPassiveItem.descriptionToken = "ITEM_VOIDMANPASSIVEITEM_DESC";

            ItemDef TeleportWhenOob = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/Base/TeleportWhenOob/TeleportWhenOob.asset").WaitForCompletion();
            LanguageAPI.Add("ITEM_TELEPORTWHENOOB_NAME", "Teleport when out of bounds", "en");
            LanguageAPI.Add("ITEM_TELEPORTWHENOOB_PICKUP", "Teleport to safety when out of bounds.", "en");
            LanguageAPI.Add("ITEM_TELEPORTWHENOOB_DESC", "You will teleport back to the map when out of bounds. This is just for enemies as players do this normally.", "en");
            TeleportWhenOob.nameToken = "ITEM_TELEPORTWHENOOB_NAME";
            TeleportWhenOob.pickupToken = "ITEM_TELEPORTWHENOOB_PICKUP";
            TeleportWhenOob.descriptionToken = "ITEM_TELEPORTWHENOOB_DESC";


            //Unused Equipment Descriptions
            LanguageAPI.Add("EQUIPMENT_LUNARPOTION_NAME", "Lunar Potion", "en");
            LanguageAPI.Add("EQUIPMENT_LUNARPOTION_PICKUP", "Cannot activate this.", "en");
            LanguageAPI.Add("EQUIPMENT_LUNARPOTION_DESC", "Cannot be used. <style=cIsHealing>100%</style> of <style=cIsHealing>healing</style> is stored in the potion and returned upon using it.", "en");

            LanguageAPI.Add("EQUIPMENT_ORBONUSE_NAME", "Orb on use", "en");
            LanguageAPI.Add("EQUIPMENT_ORBONUSE_PICKUP", "Cannot activate this.", "en");
            LanguageAPI.Add("EQUIPMENT_ORBONUSE_DESC", "Cannot be used. Purpose unknown. Many unused orb passive items exist in the files but relationship unknown.", "en");


            LanguageAPI.Add("EQUIPMENT_ORBITALLASER_NAME", "Orbital Laser", "en");
            LanguageAPI.Add("EQUIPMENT_ORBITALLASER_PICKUP", "Fire a guided orbital laser.", "en");
            LanguageAPI.Add("EQUIPMENT_ORBITALLASER_DESC", "Deal <style=cIsDamage>2000% damage</style> multiple times per second at your cursors location.", "en");

            LanguageAPI.Add("EQUIPMENT_SOULCORRUPTOR_NAME", "Soul Corrupter", "en");
            LanguageAPI.Add("EQUIPMENT_SOULCORRUPTOR_PICKUP", "Turn targeted low health monsters into a friendly ghost.", "en");
            LanguageAPI.Add("EQUIPMENT_SOULCORRUPTOR_DESC", "<style=cIsDamage>Instantly kill</style> the targeted monster if its below <style=cIsHealing>25% health</style> and turn it into a <style=cIsUtility>ghost</style>", "en");

            LanguageAPI.Add("EQUIPMENT_ENIGMA_NAME", "Enigma", "en");
            LanguageAPI.Add("EQUIPMENT_ENIGMA_PICKUP", "Cannot activate this.", "en");
            LanguageAPI.Add("EQUIPMENT_ENIGMA_DESC", "Previously used for Artifact of Enigma before they decided to use regular equipment that reroll instead of a single equipment activating random ones.", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXUNFINISHED_PICKUP", "Become an Aspect of unknown intentions", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXYELLOW_NAME", "EliteYellowEquipment", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXYELLOW_DESC", "Increases <style=cIsDamage>attack speed</style> by <style=cIsDamage>50% </style> and <style=cIsUtility>movement speed</style> by <style=cIsUtility>25%</style>.", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXECHO_NAME", "EliteEchoEquipment", "en");
            LanguageAPI.Add("EQUIPMENT_AFFIXECHO_DESC", "Gain 2 <style=cIsDamage>Echoes</style> that will fight for you.", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXSECRETSPEED_DESC", "Bunny", "en");


            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixYellow").nameToken = "EQUIPMENT_AFFIXYELLOW_NAME";
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixYellow").pickupToken = "EQUIPMENT_AFFIXUNFINISHED_PICKUP";
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixYellow").descriptionToken = "EQUIPMENT_AFFIXYELLOW_DESC";

            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixEcho").nameToken = "EQUIPMENT_AFFIXECHO_NAME";
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixEcho").pickupToken = "EQUIPMENT_AFFIXUNFINISHED_PICKUP";
            RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixEcho").descriptionToken = "EQUIPMENT_AFFIXECHO_DESC";


            LanguageAPI.Add("EQUIPMENT_SOULJAR_DESC", "Cannot be used. <style=cIsUtility>Duplicate</style> every enemy as a <style=cIsUtility>ghost</style> to fight on your side for <style=cIsUtility>15 seconds</style>.", "en");

            LanguageAPI.Add("EQUIPMENT_GHOSTGUN_DESC", "Shoot nearby enemies for <style=cIsDamage>500% damage 6 times</style> . Shots have a proc coefficient of 0. Supposed to multiply damage based on kills but does not track kills.", "en");

            LanguageAPI.Add("EQUIPMENT_AFFIXGOLD_DESC", "Does nothing due to missing elite implementation.", "en");

            GameObject tempghostgun = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/GhostGun");
            tempghostgun.transform.GetChild(0).localScale = new Vector3(1, 1, 1);

            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/DrizzlePlayerHelper").pickupIconSprite = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/difficultyicons/texDifficultyEasyIconDisabled"); ;
            RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/MonsoonPlayerHelper").pickupIconSprite = RoR2.LegacyResourcesAPI.Load<Sprite>("textures/difficultyicons/texDifficultyHardIconDisabled"); ;

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

            LanguageAPI.Add("SKILL_LUNAR_SECONDARY_REPLACEMENT_DESCRIPTION", "Charge up a ball of blades that repeatedly deals <style=cIsDamage>875%</style> damage. After a delay, explode and <style=cIsDamage>root</style> all enemies for <style=cIsDamage>700%</style>damage.");
            LanguageAPI.Add("SKILL_LUNAR_UTILITY_REPLACEMENT_DESCRIPTION", "Fade away, becoming <style=cIsUtility>intangible</style> and <style=cIsUtility>gaining movement speed</style>. <style=cIsHealing>Heal</style> for <style=cIsHealing>18% of your maximum health</style>.");

            LanguageAPI.Add("ITEM_LUNARSECONDARYREPLACEMENT_DESC", "<style=cIsUtility>Replace your Secondary Skill </style> with <style=cIsUtility>Slicing Maelstrom</style>.  \n\nCharge up a projectile that deals <style=cIsDamage>875% damage per second</style> to nearby enemies, exploding after <style=cIsUtility>3</style> seconds to deal <style=cIsDamage>700% damage</style> and <style=cIsDamage>root</style> enemies for <style=cIsUtility>3</style> <style=cStack>(+3 per stack)</style> seconds. Recharges after 5 <style=cStack>(+5 per stack)</style> seconds.");
            LanguageAPI.Add("ITEM_LUNARUTILITYREPLACEMENT_DESC", "<style=cIsUtility>Replace your Utility Skill</style> with <style=cIsUtility>Shadowfade</style>. \n\nFade away, becoming <style=cIsUtility>intangible</style> and gaining <style=cIsUtility>+30% movement speed</style>. <style=cIsHealing>Heal</style> for <style=cIsHealing>18% <style=cStack>(+18% per stack)</style> of your maximum health</style>. Lasts 3 <style=cStack>(+3 per stack)</style> seconds.");


        }

    }

}
