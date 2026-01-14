using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.Items;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Text
{
    public class PurchaseTokenOverwrite : MonoBehaviour
    {
        //For some ungodly reason
        //These tokens are networked
        //So we can't really set them for Clients who don't have it
        public string displayNameToken;
        public string contextToken;

        public void OnEnable()
        {
            if (!Language.currentLanguage.TokenIsRegistered(displayNameToken))
            {
                Destroy(this);
                return;
            }
        }
    }


    public static class TextChanges
    {
        public static R2API.LanguageAPI.LanguageOverlay TPLunar1;
        public static R2API.LanguageAPI.LanguageOverlay TPLunar2;
        public static R2API.LanguageAPI.LanguageOverlay TPLunar3;
        public static R2API.LanguageAPI.LanguageOverlay TPLunar4;
        public static bool LunaredAllOverIt = false;
        //public static R2API.LanguageAPI.LanguageOverlay TPLunar3;
        //public static R2API.LanguageAPI.LanguageOverlay TPLunar4;

        public static void Main()
        {
            EndingText();
            if (WConfig.cfgTextOther.Value)
            {
                OtherText();
            }

            On.RoR2.TeleporterInteraction.Start += BlueTeleporterObjective;

            //On.RoR2.Chat.PlayerPickupChatMessage.ConstructChatString += AddDuplicatorQuantity;
            if (WConfig.cfgMessagesVoidQuantity.Value)
            {
                On.RoR2.Chat.PlayerPickupChatMessage.ConstructChatString += AddVoidQuantity;
            }
            //On.RoR2.GenericPickupController.HandlePickupMessage += AddVoidQuantity_;
            //IL.RoR2.GenericPickupController.HandlePickupMessage += IL_PICKUP;

            OptionPickup_Visuals.Start();

            On.RoR2.PurchaseInteraction.GetDisplayName += PurchaseTokenOverwrite_PurchaseInteraction_GetDisplayName;
            IL.RoR2.PurchaseInteraction.GetContextString += PurchaseTokenOverwrite_PurchaseInteraction_GetContextString;


            /*TMP_SpriteAsset tmpsprDefault = Addressables.LoadAssetAsync<TMP_SpriteAsset>(key: "578432efa029e4f40901ce88be2f1feb").WaitForCompletion();

            TMP_SpriteAsset tempItemAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
            tempItemAsset.name = "tmpsprTempItem";
            tempItemAsset.material = Object.Instantiate(tmpsprDefault.material);
            tempItemAsset.spriteSheet = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/General/TempItemIcon.png");
            tempItemAsset.material.mainTexture = tempItemAsset.spriteSheet;
            tempItemAsset.spriteInfoList = new System.Collections.Generic.List<TMP_Sprite>();
           
            TMP_Sprite tempSprite = new TMP_Sprite();
            tempSprite.sprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/General/TempItemIcon.png");
            tempSprite.x = 0;
            tempSprite.y = 0;
            tempSprite.xOffset = -12;
            tempSprite.yOffset = 108;
            tempSprite.xAdvance = 108;
            tempSprite.id = 0;
            tempSprite.width = 128;
            tempSprite.height = 128;
            tempSprite.scale = 1.1f;
            tempSprite.pivot = new Vector2(-64, 64);
            tempSprite.name = "TempClock";
            tempItemAsset.spriteInfoList.Add(tempSprite);
 
            tempItemAsset.UpdateLookupTables();
            TMP_Settings.defaultSpriteAsset.fallbackSpriteAssets.Add(tempItemAsset); 
            LanguageAPI.Add("ITEM_MODIFIER_TEMP", "{0} <style=cIsTemporary>(<sprite name=\"TempClock\" tint=1>)</color>");
            */
        }

        private static string AddVoidQuantity(On.RoR2.Chat.PlayerPickupChatMessage.orig_ConstructChatString orig, Chat.PlayerPickupChatMessage self)
        {
            if (!self.subjectAsCharacterBody)
            {
                return orig(self);
            }
            if (!self.subjectAsCharacterBody.inventory)
            {
                return orig(self);
            }
            PickupDef pickupDef = PickupCatalog.GetPickupDef(GenericPickupController.pickupMessageInstance.pickupState.pickupIndex);
            ItemDef itemDef = ItemCatalog.GetItemDef((pickupDef != null) ? pickupDef.itemIndex : ItemIndex.None);
            if (itemDef)
            {
                int newPickupCount = (int)self.pickupQuantity;
                int VoidQuantity = newPickupCount;

                //If Is Void, 
                if (itemDef.tier >= ItemTier.VoidTier1 && itemDef.tier <= ItemTier.VoidBoss)
                {
                    //If is Void, shouldn't we only bother checking the first time?
                    //Check for permament to be sure, instead of using existing quantity ig
                    if (self.subjectAsCharacterBody.inventory.GetItemCountPermanent(itemDef) <= 1)
                    {
                        foreach (ContagiousItemManager.TransformationInfo transformationInfo in ContagiousItemManager._transformationInfos)
                        {
                            if (itemDef.itemIndex == transformationInfo.transformedItem)
                            {
                                int original = self.subjectAsCharacterBody.inventory.GetItemCountEffective(transformationInfo.originalItem);
                                if (original > 0)
                                {
                                    VoidQuantity += original;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ItemIndex voidIndex = ContagiousItemManager.GetTransformedItemIndex(itemDef.itemIndex);
                    if (voidIndex != ItemIndex.None)
                    {
                        //If Is Normal but you got Void
                        //If items are picked up too fast it doesn't count properly for items Voids that eat multiple
                        int voidCount = self.subjectAsCharacterBody.inventory.GetItemCountEffective(voidIndex);
                        if (voidCount > 0)
                        {
                            VoidQuantity += voidCount;
                        }
                    }
                }
                if (VoidQuantity > newPickupCount)
                {
                    self.pickupQuantity = 1;
                    return orig(self) + "<style=cIsVoid>(" + VoidQuantity + ")</style>";
                }

            }

            return orig(self);
        }
        /*
        private static string AddDuplicatorQuantity(On.RoR2.Chat.PlayerPickupChatMessage.orig_ConstructChatString orig, Chat.PlayerPickupChatMessage self)
        {
            //Meh?
            bool dup = self.subjectAsCharacterBody.inventory.GetItemCountEffective(DLC3Content.Items.Duplicator) > 0;
            if (dup)
            {
                self.pickupQuantity--;
            }
            string msg = orig(self);
            if (dup)
            {
                msg += "<style=cIsTemporary>(+1)</style>";
            }

            return msg;
        }
        */

        public static void AutoGeneratedText()
        {
            if (WConfig.cfgAltBodyNames.Value)
            {
                if (Language.currentLanguage.TokenIsRegistered("LUNARGOLEM_BODY_NAME_"))
                {
                    LanguageAPI.Add("LUNARGOLEM_BODY_NAME", Language.GetString("LUNARGOLEM_BODY_NAME_"));
                    LanguageAPI.Add("LUNARWISP_BODY_NAME", Language.GetString("LUNARWISP_BODY_NAME_"));
                    LanguageAPI.Add("LUNAREXPLODER_BODY_NAME", Language.GetString("LUNAREXPLODER_BODY_NAME_"));
                    LanguageAPI.Add("ACIDLARVA_BODY_NAME", Language.GetString("ACIDLARVA_BODY_NAME_"));

                    On.EntityStates.Missions.BrotherEncounter.Phase2.OnEnter += MithrixPhase2OverrideLunarChimeraHordeName;
                }
            }
            if (!WConfig.cfgTextOther.Value)
            {
                return;
            }
            #region Speech Colors

            LanguageAPI.Add("PROPER_SAVE_CHAT_SAVE", "<style=cEvent>" + Language.GetString("PROPER_SAVE_CHAT_SAVE").Replace("<b>", "") + "</style>");
            LanguageAPI.Add("VULTUREHUNTER_DIALOGUE_FORMAT", Language.GetString("VULTUREHUNTER_DIALOGUE_FORMAT").Replace("c6d5ff", "87E5C0"));
            //LanguageAPI.Add("FALSESONBOSS_DIALOGUE_FORMAT", Language.GetString("FALSESONBOSS_DIALOGUE_FORMAT").Replace("c6d5ff", "FFA288"));
            #endregion


            #region Boosted Skill Titles
            SkillDef skill = null;
            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC1/VoidSurvivor/FireCorruptBeam.asset").WaitForCompletion();
            BoostedNameToken(skill, "VOIDFIEND_BOOSTED_FORMAT");
            skill.skillDescriptionToken = "SKILL_VOIDFIEND_CORRUPT_M1";
            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC1/VoidSurvivor/FireCorruptDisk.asset").WaitForCompletion();
            BoostedNameToken(skill, "VOIDFIEND_BOOSTED_FORMAT");
            skill.skillDescriptionToken = "SKILL_VOIDFIEND_CORRUPT_M2";
            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC1/VoidSurvivor/VoidBlinkDown.asset").WaitForCompletion();
            BoostedNameToken(skill, "VOIDFIEND_BOOSTED_FORMAT");
            skill.skillDescriptionToken = "SKILL_VOIDFIEND_CORRUPT_M3";
            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC1/VoidSurvivor/CrushHealth.asset").WaitForCompletion();
            BoostedNameToken(skill, "VOIDFIEND_BOOSTED_FORMAT");
            skill.skillDescriptionToken = "SKILL_VOIDFIEND_CORRUPT_M4";


            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC2/Chef/ChefDiceBoosted.asset").WaitForCompletion();
            BoostedNameToken(skill, "CHEF_BOOSTED_FORMAT");
            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC2/Chef/ChefSearBoosted.asset").WaitForCompletion();
            BoostedNameToken(skill, "CHEF_BOOSTED_FORMAT");
            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC2/Chef/ChefIceBoxBoosted.asset").WaitForCompletion();
            BoostedNameToken(skill, "CHEF_BOOSTED_FORMAT");
            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC2/Chef/ChefRolyPolyBoosted.asset").WaitForCompletion();
            BoostedNameToken(skill, "CHEF_BOOSTED_FORMAT");
            skill = Addressables.LoadAssetAsync<SkillDef>(key: "RoR2/DLC2/Chef/ChefOilSpillBoosted.asset").WaitForCompletion();
            BoostedNameToken(skill, "CHEF_BOOSTED_FORMAT");
            #endregion


            skill = Addressables.LoadAssetAsync<SkillDef>(key: "dacc509e9126ce5488645a54da6a8509").WaitForCompletion();
            skill.skillNameToken = "FALSESON_PRIMARY_NAME";
            skill.skillDescriptionToken = "FALSESON_PRIMARY_DESCRIPTION";


            DLC1Content.Items.FragileDamageBonusConsumed.descriptionToken = DLC1Content.Items.FragileDamageBonusConsumed.pickupToken;
            DLC1Content.Items.HealingPotionConsumed.descriptionToken = DLC1Content.Items.HealingPotionConsumed.pickupToken;



        }

        private static void MithrixPhase2OverrideLunarChimeraHordeName(On.EntityStates.Missions.BrotherEncounter.Phase2.orig_OnEnter orig, EntityStates.Missions.BrotherEncounter.Phase2 self)
        {
            orig(self);
            if (self.phaseBossGroup)
            {
                BossGroup boss = self.phaseBossGroup.GetComponent<BossGroup>();
                boss.lastDisplayedPriority = 0;
                boss.currentPriority = 0;
                boss.bestObservedName = Language.GetString("LUNAR_CHIMERA");
                boss.bestObservedSubtitle = "<sprite name=\"CloudLeft\" tint=1> " + Language.GetString("LUNARGOLEM_BODY_SUBTITLE") + " <sprite name=\"CloudRight\" tint=1>";
            }
        }

        public static void BoostedNameToken(SkillDef skill, string format)
        {
            LanguageAPI.Add(skill.skillNameToken + "_B", string.Format(Language.GetString(format), Language.GetString(skill.skillNameToken)));
            skill.skillNameToken = skill.skillNameToken + "_B";
        }


        private static void PurchaseTokenOverwrite_PurchaseInteraction_GetContextString(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("RoR2.PurchaseInteraction", "contextToken")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<string, PurchaseInteraction, string>>((ret, self) =>
                {
                    PurchaseTokenOverwrite overwrite = self.GetComponent<PurchaseTokenOverwrite>();
                    if (overwrite)
                    {
                        return overwrite.contextToken;
                    }
                    return ret;
                });
            }
            else
            {
                Log.LogWarning("IL Failed: PurchaseInteraction_GetContextString");
            }
        }

        private static string PurchaseTokenOverwrite_PurchaseInteraction_GetDisplayName(On.RoR2.PurchaseInteraction.orig_GetDisplayName orig, PurchaseInteraction self)
        {
            PurchaseTokenOverwrite overwrite = self.GetComponent<PurchaseTokenOverwrite>();
            if (overwrite)
            {
                return Language.GetString(overwrite.displayNameToken);
            }
            return orig(self);
        }

        public static void EndingText()
        {
            GameEndingDef PrismEnding = LegacyResourcesAPI.Load<GameEndingDef>("GameEndingDefs/PrismaticTrialEnding");
            PrismEnding.backgroundColor = new Color(0.7f, 0.3f, 0.7f, 0.615f);
            PrismEnding.foregroundColor = new Color(0.9f, 0.6f, 0.9f, 0.833f);

            GameEndingDef MainEnding = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/MainEnding.asset").WaitForCompletion();
            GameEndingDef EscapeSequenceFailed = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/EscapeSequenceFailed.asset").WaitForCompletion();

            GameEndingDef LimboEnding = LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/LimboEnding");
            GameEndingDef VoidEnding = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/DLC1/GameModes/VoidEnding.asset").WaitForCompletion();

            GameEndingDef ObliterationEnding = LegacyResourcesAPI.Load<GameEndingDef>("gameendingdefs/ObliterationEnding");
            GameEndingDef RebirthEndingDef = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/DLC2/ClassicRun/Endings/RebirthEndingDef.asset").WaitForCompletion();
            GameEndingDef DecompileEnding = Addressables.LoadAssetAsync<GameEndingDef>(key: "bdddeb53086a7c8409359b7179f3a507").WaitForCompletion();


            EscapeSequenceFailed.icon = MainEnding.icon;

            Color colorMult = new Color(0.9f, 0.9f, 1f, 1f);
            LimboEnding.backgroundColor = new Color32(227, 236, 252, 215) * colorMult;
            LimboEnding.foregroundColor = new Color32(232, 239, 255, 190) * colorMult;

            ObliterationEnding.foregroundColor = ObliterationEnding.backgroundColor * 1.1f;
            ObliterationEnding.backgroundColor = ObliterationEnding.backgroundColor.AlphaMultiplied(0.75f);


            VoidEnding.icon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/General/texGameResultRebirth_SotV.png");
            RebirthEndingDef.icon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/General/texGameResult_SotS.png");
            DecompileEnding.icon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/General/texGameResult_AC.png");
            Color one = DecompileEnding.foregroundColor;
            DecompileEnding.foregroundColor = DecompileEnding.backgroundColor;
            DecompileEnding.backgroundColor = one;

            if (WConfig.cfgTextOther.Value == true)
            {
                PrismEnding.endingTextToken = "ACHIEVEMENT_COMPLETEPRISMATICTRIAL_NAME";

                LimboEnding.endingTextToken = "GAME_RESULT_LIMBOWIN";
                VoidEnding.endingTextToken = "GAME_RESULT_VOIDWIN";
                EscapeSequenceFailed.endingTextToken = "GAME_RESULT_ESCAPEFAILED";
            }

        }

        private static void BlueTeleporterObjective(On.RoR2.TeleporterInteraction.orig_Start orig, TeleporterInteraction self)
        {
            orig(self);
            if (WConfig.cfgPrimordialBlueText.Value == false)
            {
                return;
            }
            if (self.name.StartsWith("Lunar"))
            {
                if (TPLunar1 == null)
                {
                    LunaredAllOverIt = true;
                    TPLunar1 = LanguageAPI.AddOverlay("OBJECTIVE_FIND_TELEPORTER", Language.GetString("OBJECTIVE_FIND_TELEPORTER").Replace("cDeath", "cLunarObjective"));
                    TPLunar2 = LanguageAPI.AddOverlay("OBJECTIVE_CHARGE_TELEPORTER", Language.GetString("OBJECTIVE_CHARGE_TELEPORTER").Replace("cDeath", "cLunarObjective"));
                    TPLunar3 = LanguageAPI.AddOverlay("OBJECTIVE_CHARGE_TELEPORTER_OOB", Language.GetString("OBJECTIVE_CHARGE_TELEPORTER_OOB").Replace("cDeath", "cLunarObjective"));
                    TPLunar4 = LanguageAPI.AddOverlay("OBJECTIVE_FINISH_TELEPORTER", Language.GetString("OBJECTIVE_FINISH_TELEPORTER").Replace("cDeath", "cLunarObjective"));

                }
            }
            else if (TPLunar1 != null)
            {
                TPLunar1.Remove();
                TPLunar2.Remove();
                TPLunar3.Remove();
                TPLunar4.Remove();

            }

        }


        //What is this??
        /*private static void IL_PICKUP(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchCallOrCallvirt("RoR2.GenericPickupController/PickupMessage", "Reset")))
            {
                c.Index--;
                c.RemoveRange(2);
            }
            else
            {
                Log.LogWarning("IL Failed: SIL_PICKUP");
            }
        }
        */
        internal static void OtherText()
        {
            //Additional Key Words
            LanguageAPI.Add("KEYWORD_SLOWING", "<style=cKeywordName>Slowing</style><style=cSub>Apply a slowing debuff reducing enemy <style=cIsUtility>movement speed</style> by <style=cIsUtility>50%</style>.</style>", "en");

            Addressables.LoadAssetAsync<GameObject>(key: "357435043113a944c9a477d63dc9a893").WaitForCompletion().GetComponent<SpecialObjectAttributes>().bestName = "FREECHEST_TERMINAL_NAME";
            PurchaseTokenOverwrite changeTokenOnStart = Addressables.LoadAssetAsync<GameObject>(key: "0af0551c0f67fe249b04af841608a41c").WaitForCompletion().AddComponent<PurchaseTokenOverwrite>();
            changeTokenOnStart.displayNameToken = "FREECHEST_TERMINAL_NAME";
            changeTokenOnStart.contextToken = "FREECHEST_TERMINAL_CONTEXT";

            Addressables.LoadAssetAsync<GameObject>(key: "c814170ba819d7b45934c0fc54fd99d1").WaitForCompletion().GetComponent<SpecialObjectAttributes>().bestName = "MULTISHOP_LARGE_TERMINAL_NAME";
            changeTokenOnStart = Addressables.LoadAssetAsync<GameObject>(key: "c87d4eb171e0ee44cb2d4ec714c58c72").WaitForCompletion().AddComponent<PurchaseTokenOverwrite>();
            changeTokenOnStart.displayNameToken = "MULTISHOP_LARGE_TERMINAL_NAME";
            changeTokenOnStart.contextToken = "MULTISHOP_TERMINAL_CONTEXT";

            Addressables.LoadAssetAsync<GameObject>(key: "e992781bd4b52804c8d340a56db49a06").WaitForCompletion().GetComponent<SpecialObjectAttributes>().bestName = "MULTISHOP_EQUIPMENT_TERMINAL_NAME";
            changeTokenOnStart = Addressables.LoadAssetAsync<GameObject>(key: "63251d66a257b1a4c8eed4de5dd89561").WaitForCompletion().AddComponent<PurchaseTokenOverwrite>();
            changeTokenOnStart.displayNameToken = "MULTISHOP_EQUIPMENT_TERMINAL_NAME";
            changeTokenOnStart.contextToken = "MULTISHOP_TERMINAL_CONTEXT";

            GameObject duplicatorLarge = Addressables.LoadAssetAsync<GameObject>(key: "f21b2c8a9cc028046935ea871dc4af54").WaitForCompletion();
            duplicatorLarge.GetComponent<SpecialObjectAttributes>().bestName = "DUPLICATOR_LARGE_NAME";
            changeTokenOnStart = duplicatorLarge.AddComponent<PurchaseTokenOverwrite>();
            changeTokenOnStart.displayNameToken = "DUPLICATOR_LARGE_NAME";
            changeTokenOnStart.contextToken = "DUPLICATOR_LARGE_CONTEXT";



            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Vagrant/VagrantTrackingBomb.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "VagrantTrackingBomb";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Scav/ScavSackProjectile.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "ScavSackProjectile";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Scorchling/ScorchlingBombProjectile.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "ScorchlingBombProjectile";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/MiniGeodeBody.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "MiniGeodeBody";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/LunarWisp/LunarWispTrackingBomb.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "LunarWispTrackingBomb";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Gravekeeper/GravekeeperTrackingFireball.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "GravekeeperTrackingFireball";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Elites/EliteBead/BeadProjectileTrackingBomb.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "BeadProjectileTrackingBomb";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/AltarSkeleton/AltarSkeletonBody.prefab").WaitForCompletion().GetComponent<CharacterBody>().baseNameToken = "AltarSkeletonBody";


            Addressables.LoadAssetAsync<GameObject>(key: "34d770816ffbf0d468728c48853fd5f6").WaitForCompletion().GetComponent<GenericInteraction>().contextToken = "PORTAL_DESTINATION_CONTEXT";

        }

        public static void UntieredItemTokens()
        {
            RoR2Content.Items.TeamSizeDamageBonus.AutoPopulateTokens();
            RoR2Content.Items.UseAmbientLevel.AutoPopulateTokens();
            RoR2Content.Items.MinHealthPercentage.AutoPopulateTokens();
            DLC1Content.Items.VoidmanPassiveItem.AutoPopulateTokens();
            RoR2Content.Items.TeleportWhenOob.AutoPopulateTokens();
            JunkContent.Equipment.EliteYellowEquipment.AutoPopulateTokens();
            DLC3Content.Items.DroneUpgradeHidden.AutoPopulateTokens();
        }



    }

}
