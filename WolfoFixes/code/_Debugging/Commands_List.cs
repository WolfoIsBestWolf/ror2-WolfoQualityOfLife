using Newtonsoft.Json.Utilities;
using RoR2;
using RoR2.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes.Testing
{
    class Commands_Lists
    {
        [ConCommand(commandName = "list_mystats", flags = ConVarFlags.None, helpText = "List all StatDefs")]
        public static void CCList_CurrentStats(ConCommandArgs args)
        {
            if (PlayerCharacterMasterController.instances.Count == 0)
            {
                Debug.Log("No Players");
                return;
            }
            string log = "";
            var aa = PlayerCharacterMasterController.instances[0].master.playerStatsComponent.currentStats;
            for (int i = 0; i < aa.fields.Length; i++)
            {
                StatDef def = aa.fields[i].statDef;
                if (def.dataType == StatDataType.ULong)
                {
                    if (aa.fields[i].ulongValue > 0)
                    {
                        log += string.Format("\n[{0}] {1} | {2}", new object[]
                        {
                            i,
                            def.name,
                             aa.fields[i].ulongValue
                        });
                    }
                }
                else if (def.dataType == StatDataType.Double)
                {
                    if (aa.fields[i].doubleValue > 0)
                    {
                        log += string.Format("\n[{0}] {1} | {2}", new object[]
                        {
                            i,
                            def.name,
                             aa.fields[i].doubleValue
                        });
                    }
                }
            }

            Debug.Log(log);
        }

        [ConCommand(commandName = "list_statdef", flags = ConVarFlags.None, helpText = "List all StatDefs")]
        public static void CCList_StatDef(ConCommandArgs args)
        {
            string log = "";
            foreach (StatField statField in StatSheet.fieldsTemplate)
            {
                log += "\n" + statField.statDef.name + " | " + Language.GetString(statField.statDef.displayToken);
            }
            Debug.Log(log);
        }
        
        [ConCommand(commandName = "list_sceneDefColors", flags = ConVarFlags.None, helpText = "List all StatDefs")]
        public static void CCList_SceneDef(ConCommandArgs args)
        {
            string log = "";
            foreach (SceneDef def in SceneCatalog.allSceneDefs)
            {
                if (!def.isOfflineScene)
                {
                    log += "\n" + def.cachedName + " | " + def.environmentColor;
                }
            }
            Debug.Log(log);
        }
        
        [ConCommand(commandName = "dccs_catPercent", flags = ConVarFlags.None, helpText = "List all StatDefs")]
        public static void CCListCategoriesInPercent(ConCommandArgs args)
        {
            if (!ClassicStageInfo.instance)
            {
                Debug.Log("Must be on stage");
            }
            var dccs = ClassicStageInfo.instance.interactableCategories;
            if (!dccs)
            {
                Debug.Log("No DCCS");
            }
            float totalWeight = dccs.SumOfAllCategories();
            string log = "--" + SceneInfo.instance.sceneDef.cachedName + "--\n";
            foreach (var cat in dccs.categories)
            {
                log += cat.name + " | " + cat.selectionWeight / totalWeight * 100 + "%\n";
            }
            Debug.Log(log);
        }

        [ConCommand(commandName = "list_rules", flags = ConVarFlags.None, helpText = "List all RuleDef and RuleChoiceDef")]
        public static void CCList_RuleDef(ConCommandArgs args)
        {
            string log = "";
            foreach (var rule in RuleCatalog.allRuleDefs)
            {
                log += "\n" + rule.globalName;
                foreach (var choice in rule.choices)
                {
                    log += "\n-" + choice.globalName;
                }
            }
            Debug.Log(log);
        }

        [ConCommand(commandName = "list_effectdef", flags = ConVarFlags.None, helpText = "List all EffectDefs")]
        public static void CCList_EffectDef(ConCommandArgs args)
        {
            string log = "";
            foreach (var def in EffectCatalog.entries)
            {
                log += "\n" + def.prefabName;
            }
            Debug.Log(log);
        }

        [ConCommand(commandName = "list_combatdirector", flags = ConVarFlags.None, helpText = "WeightedSelection of monsters on current stage.")]
        public static void CCList_CombatDirector(ConCommandArgs args)
        {
            var a = SceneDirector.FindFirstObjectByType<SceneDirector>();
            if (!ClassicStageInfo.instance)
            {
                Debug.Log("No SceneDirector");
                return;
            }

            string log = string.Empty;
            float totalWeight = ClassicStageInfo.instance.modifiableMonsterCategories.SumOfAllCategories();
            for (int i = 0; i < ClassicStageInfo.instance.monsterSelection.Count; i++)
            {
                var c = ClassicStageInfo.instance.monsterSelection.choices[i];
                log += string.Format("\n{0} | {1}", new object[] { c.value.spawnCard.name, c.weight / totalWeight * 100 + "%" });
            }

            Debug.Log(log);
        }

        [ConCommand(commandName = "list_scenedirector", flags = ConVarFlags.None, helpText = "WeightedSelection of interactables on current stage.")]
        public static void CCList_SceneDirector(ConCommandArgs args)
        {
            var a = SceneDirector.FindFirstObjectByType<SceneDirector>();
            if (!a)
            {
                Debug.Log("No SceneDirector");
                return;
            }
            string log = string.Empty;
            float totalWeight = ClassicStageInfo.instance.interactableCategories.SumOfAllCategories();
            var b = a.GenerateInteractableCardSelection();
            for (int i = 0; i < b.Count; i++)
            {
                var c = b.choices[i];
                log += string.Format("\n{0} | {1}", new object[] { c.value.spawnCard.name, c.weight / totalWeight * 100 + "%" });
            }

            Debug.Log(log);

        }


        [ConCommand(commandName = "list_mods", flags = ConVarFlags.None, helpText = "List installed mod names to get them for compatibility.")]
        public static void CCMods(ConCommandArgs args)
        {
            string log1 = "";
            string log2 = "";
            foreach (var a in BepInEx.Bootstrap.Chainloader.PluginInfos)
            {
                log1 += a.ToString() + "\n";
            }

            foreach (var a in NetworkModCompatibilityHelper._networkModList)
            {
                log2 += a.ToString() + "\n";
            }
            Debug.Log("All loaded mods");
            Debug.Log(log1);
            Debug.Log("All RequiredByAllTaggedMods");
            Debug.Log(log2);
        }

        [ConCommand(commandName = "list_pickupdef", flags = ConVarFlags.None, helpText = "Give Radar Scanner and Equipment Cooldown Reduction hidden item")]
        public static void CCList_PickupDef(ConCommandArgs args)
        {
            string debug = "";
            for (int i = 0; i < PickupCatalog.entries.Length; i++)
            {
                PickupDef def = PickupCatalog.entries[i];
                debug += string.Format("{0} | <color=#{2}>{1}</color>\n", new object[]
                 {
                    def.internalName,
                    Language.GetString(def.nameToken),
                    ColorUtility.ToHtmlStringRGB(def.baseColor),
                 });
            }
            Debug.Log(debug);
        }

        [ConCommand(commandName = "list_music", flags = ConVarFlags.None, helpText = "Give Radar Scanner and Equipment Cooldown Reduction hidden item")]
        public static void CCList_MusicDef(ConCommandArgs args)
        {
            string debug = "";
            for (int i = 0; i < MusicTrackCatalog.musicTrackDefs.Length; i++)
            {
                MusicTrackDef def = MusicTrackCatalog.musicTrackDefs[i];
                debug += string.Format("{0} | {1}\n", new object[]
                 {
                    def.cachedName,
                    def.comment,
                 });
            }
            Debug.Log(debug);
        }

        [ConCommand(commandName = "list_buffflags", flags = ConVarFlags.None, helpText = "List of buffs tagged as various things.")]
        public static void CCList_BuffDefFlags(ConCommandArgs args)
        {
            string logHiddenBuffs = "\n--HiddenBuffs--";
            string logNectar = "\n--IgnoredByNectar--";
            string logNoxBlacklist = "\n--IgnoredByNoxThorn--";

            foreach (BuffDef buff in BuffCatalog.buffDefs)
            {
                if (buff.isHidden)
                {
                    logHiddenBuffs += "\n" + buff.name;
                }
                else
                {
                    if (buff.flags.HasFlag(BuffDef.Flags.ExcludeFromNoxiousThorns))
                    {
                        logNoxBlacklist += "\n" + buff.name;
                    }
                    if (buff.ignoreGrowthNectar)
                    {
                        if (buff.isDebuff == false && buff.isCooldown == false && buff.isDOT == false)
                        {
                            logNectar += "\n" + buff.name;
                        }
                    }
                }


            };
            Debug.Log(logHiddenBuffs);
            Debug.Log(logNectar);
            Debug.Log(logNoxBlacklist);
        }

        [ConCommand(commandName = "list_bodyflags", flags = ConVarFlags.None, helpText = "List all bodies and their body flags")]
        public static void CCList_BodyFlags(ConCommandArgs args)
        {
            foreach (CharacterBody body in BodyCatalog.bodyPrefabBodyComponents)
            {
                Debug.LogFormat("\n{0}\n{1}", new object[]
                {
                    body.name,
                    body.bodyFlags,
                });
            };
        }

        [ConCommand(commandName = "list_simuwaves", flags = ConVarFlags.None, helpText = "Simple info dump of Simulacrum waves")]
        public static void CCList_SimuWaves(ConCommandArgs args)
        {
            List<DetachParticleOnDestroyAndEndEmission> aaaa = Resources.FindObjectsOfTypeAll<DetachParticleOnDestroyAndEndEmission>().ToList();
            foreach (var a in aaaa)
            {
                if (a.GetComponent<EffectComponent>())
                {
                    if (a.enabled && a.particleSystem != null)
                    {
                        Debug.LogWarning(a.transform.root.name);
                    }
                    else
                    {
                        Debug.Log(a.transform.root.name);
                    }
                }


            }


            List<InfiniteTowerWaveCategory> categories = Resources.FindObjectsOfTypeAll<InfiniteTowerWaveCategory>().ToList();
            foreach (InfiniteTowerWaveCategory category in categories)
            {
                string debug = "_____\n" + category.name + "";
                for (int i = 0; i < category.wavePrefabs.Length; i++)
                {
                    Transform wave = category.wavePrefabs[i].wavePrefab.GetComponent<InfiniteTowerWaveController>().overlayEntries[1].prefab.transform.GetChild(0).GetChild(1);
                    debug += string.Format("\n<color=#{2}>{0}</color>\n<color=#{3}>{1}</color>\nWeight:{4}", new object[]
                    {
                        Language.GetString(wave.GetChild(0).GetComponent<RoR2.UI.InfiniteTowerWaveCounter>().token),
                        Language.GetString(wave.GetChild(1).GetComponent<RoR2.UI.LanguageTextMeshController>().token),
                        ColorUtility.ToHtmlStringRGB(wave.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color),
                        ColorUtility.ToHtmlStringRGB(wave.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().color),
                        category.wavePrefabs[i].weight,
                    });
                }
                Debug.Log(debug);
            }
        }

        [ConCommand(commandName = "list_itemTags", flags = ConVarFlags.None, helpText = "List all items and their item tags")]
        public static void CCList_ItemTags(ConCommandArgs args)
        {
            for (int i = 0; i < ItemCatalog.allItemDefs.Length; i++)
            {
                ItemDef def = ItemCatalog.allItemDefs[i];
                string info = "\n__" + def.name + " | " + Language.GetString(def.nameToken) + "__";
                for (int j = 0; j < def.tags.Length; j++)
                {
                    info += "\n" + def.tags[j].ToString();
                }
                Debug.Log(info);
            }
        }

        [ConCommand(commandName = "list_itemCategories", flags = ConVarFlags.None, helpText = "List item categories")]
        public static void CCList_ItemCategory(ConCommandArgs args)
        {

            string Damage = "______\nItems tagged as Damage";
            string Health = "______\nItems tagged as Healing";
            string Utility = "______\nItems tagged as Utility";
            string AIBlacklist = "______\nItems tagged as AIBlacklist";
            string CannotDuplicate = "______\nItems tagged as CannotDuplicate";
            for (int i = 0; i < ItemCatalog.allItemDefs.Length; i++)
            {
                ItemDef itemDef = ItemCatalog.allItemDefs[i];
                if (itemDef.tier == ItemTier.NoTier)
                {
                    continue;
                }
                string info = string.Format("\n<color=#{1}>{0}</color>", new object[]
 {
                    Language.GetString(itemDef.nameToken),
                    ColorCatalog.GetColorHexString(itemDef.colorIndex),
 });
                if (Array.IndexOf<ItemTag>(itemDef.tags, ItemTag.Damage) != -1)
                {
                    Damage += info;
                }
                if (Array.IndexOf<ItemTag>(itemDef.tags, ItemTag.Healing) != -1)
                {
                    Health += info;
                }
                if (Array.IndexOf<ItemTag>(itemDef.tags, ItemTag.Utility) != -1)
                {
                    Utility += info;
                }
                if (Array.IndexOf<ItemTag>(itemDef.tags, ItemTag.AIBlacklist) != -1)
                {
                    AIBlacklist += info;
                }
                if (Array.IndexOf<ItemTag>(itemDef.tags, ItemTag.CannotDuplicate) != -1)
                {
                    CannotDuplicate += info;
                }

            }

            Debug.Log(Damage);
            Debug.Log(Health);
            Debug.Log(Utility);
            Debug.Log(AIBlacklist);
            Debug.Log(CannotDuplicate);

        }

        [ConCommand(commandName = "list_all_dccs", flags = ConVarFlags.None, helpText = "List all dccs, dp, csc, isc. Will take a moment & Restart afterwards. Does not get modded dccs or dp")]
        public static void CCList_DCCS(ConCommandArgs args)
        {
            List<DirectorCardCategorySelection> allVanillaDccs = new List<DirectorCardCategorySelection>();
            foreach (var item in Addressables.ResourceLocators)
            {
                foreach (var strng in item.Keys)
                {
                    var dccs = Addressables.LoadAssetAsync<DirectorCardCategorySelection>(key: strng).WaitForCompletion();
                    if (dccs != null)
                    {
                        allVanillaDccs.AddDistinct(dccs);
                    }
                    Addressables.LoadAssetAsync<SpawnCard>(key: strng).WaitForCompletion();
                    Addressables.LoadAssetAsync<DccsPool>(key: strng).WaitForCompletion();
                }
            }
            List<DirectorCardCategorySelection> interactablesDccs = new List<DirectorCardCategorySelection>();
            List<DirectorCardCategorySelection> monsterDccs = new List<DirectorCardCategorySelection>();
            List<DirectorCardCategorySelection> familyDccs = new List<DirectorCardCategorySelection>();
            List<DirectorCardCategorySelection> miscDccs = new List<DirectorCardCategorySelection>();
            List<DirectorCardCategorySelection> moddedDccs = Resources.FindObjectsOfTypeAll<DirectorCardCategorySelection>().ToList();
            List<DccsPool> allDCCSPool = Resources.FindObjectsOfTypeAll<DccsPool>().OfType<DccsPool>().ToList();
            List<CharacterSpawnCard> allCSC = Resources.FindObjectsOfTypeAll<CharacterSpawnCard>().OfType<CharacterSpawnCard>().ToList();
            List<InteractableSpawnCard> allISC = Resources.FindObjectsOfTypeAll<InteractableSpawnCard>().OfType<InteractableSpawnCard>().ToList();

            foreach (DirectorCardCategorySelection dccs in allVanillaDccs)
            {
                moddedDccs.Remove(dccs);
                if (dccs is FamilyDirectorCardCategorySelection)
                {
                    familyDccs.Add(dccs);
                }
                else if (dccs.name.Contains("Interactables"))
                {
                    interactablesDccs.Add(dccs);
                }
                else if (dccs.name.Contains("Monsters"))
                {
                    monsterDccs.Add(dccs);
                }
                else
                {
                    miscDccs.Add(dccs);
                }
            }


            monsterDccs.Sort((x, y) => string.Compare(x.name, y.name));
            interactablesDccs.Sort((x, y) => string.Compare(x.name, y.name));
            familyDccs.Sort((x, y) => string.Compare(x.name, y.name));
            miscDccs.Sort((x, y) => string.Compare(x.name, y.name));
            moddedDccs.Sort((x, y) => string.Compare(x.name, y.name));
            allDCCSPool.Sort((x, y) => string.Compare(x.name, y.name));
            allCSC.Sort((x, y) => string.Compare(x.name, y.name));
            allISC.Sort((x, y) => string.Compare(x.name, y.name));



            Debug.Log("\nDCCS | All Interactables");
            DumpDCCSList(interactablesDccs);
            Debug.Log("\nDCCS | All Monsters");
            DumpDCCSList(monsterDccs);
            Debug.Log("\nDCCS | All Families");
            DumpDCCSList(familyDccs);
            Debug.Log("\nDCCS | Misc");
            DumpDCCSList(miscDccs);
            Debug.Log("\nDCCS | Not Vanilla");
            DumpDCCSList(moddedDccs);

            Debug.Log("\nDP | All DccsPools");
            for (int dccs = 0; allDCCSPool.Count > dccs; dccs++)
            {
                string log = "\n--------------------\n";
                log += allDCCSPool[dccs].name;

                for (int cat = 0; allDCCSPool[dccs].poolCategories.Length > cat; cat++)
                {
                    var category = allDCCSPool[dccs].poolCategories[cat];
                    log += "\n--[" + cat + "]--" + category.name;
                    for (int card = 0; category.alwaysIncluded.Length > card; card++)
                    {
                        log += "\n[" + card + "] " + category.alwaysIncluded[card].dccs.name + "  wt:" + category.alwaysIncluded[card].weight;
                    }
                    for (int card = 0; category.includedIfConditionsMet.Length > card; card++)
                    {
                        log += "\n[" + card + "] " + category.includedIfConditionsMet[card].dccs.name + "  wt:" + category.includedIfConditionsMet[card].weight;
                    }
                    for (int card = 0; category.includedIfNoConditionsMet.Length > card; card++)
                    {
                        log += "\n[" + card + "] " + category.includedIfNoConditionsMet[card].dccs.name + "  wt:" + category.includedIfNoConditionsMet[card].weight;
                    }
                }
                Debug.Log(log);
            }

            Debug.Log("--\nCharacterSpawnCards\n--");
            for (int i = 0; allCSC.Count > i; i++)
            {
                Debug.Log(allCSC[i].name + " Cost:" + allCSC[i].directorCreditCost);
            }
            Debug.Log("--\nInteractableSpawnCards\n--");
            for (int i = 0; allISC.Count > i; i++)
            {
                Debug.Log(allISC[i].name + " Cost:" + allISC[i].directorCreditCost);
            }



        }

        public static void DumpDCCSList(List<DirectorCardCategorySelection> allDCCS)
        {

            for (int dccs = 0; allDCCS.Count > dccs; dccs++)
            {
                string log = "\n-------------------\n";
                log += allDCCS[dccs].name;
                FamilyDirectorCardCategorySelection family = allDCCS[dccs] as FamilyDirectorCardCategorySelection;
                if (family)
                {
                    log += "\nfamilyStart: " + family.minimumStageCompletion;
                    log += "\nfamilyEnd: " + family.maximumStageCompletion;
                }
                for (int cat = 0; allDCCS[dccs].categories.Length > cat; cat++)
                {
                    var CATEGORY = allDCCS[dccs].categories[cat];
                    log += "\n--[" + cat + "]--" + CATEGORY.name + "--" + "  wt:" + allDCCS[dccs].categories[cat].selectionWeight;
                    for (int card = 0; CATEGORY.cards.Length > card; card++)
                    {
                        log += "\n[" + card + "] ";

                        if (!CATEGORY.cards[card].spawnCard)
                        {
                            log += "NULL Spawn Card";
                        }
                        else
                        {
                            log += CATEGORY.cards[card].spawnCard.name;
                        }
                        log += "  wt:" + CATEGORY.cards[card].selectionWeight + "  minStage:" + CATEGORY.cards[card].minimumStageCompletions;
                        if (CATEGORY.cards[card].forbiddenUnlockableDef || !string.IsNullOrEmpty(CATEGORY.cards[card].forbiddenUnlockable))
                        {
                            log += " forbiddenWhen: " + CATEGORY.cards[card].forbiddenUnlockable + CATEGORY.cards[card].forbiddenUnlockableDef;
                        }
                    }
                }
                Debug.Log(log);

            }

        }



    }

}
