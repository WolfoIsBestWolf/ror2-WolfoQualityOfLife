using Newtonsoft.Json.Utilities;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{
    class Commands
    {

        [ConCommand(commandName = "scanner", flags = ConVarFlags.ExecuteOnServer, helpText = "Give Radar Scanner and Equipment Cooldown Reduction hidden item")]
        public static void CCScanner(ConCommandArgs args)
        {
            if (!args.senderMaster)
            {
                return;
            }
            if (!NetworkServer.active)
            {
                return;
            }
            args.senderMaster.inventory.SetEquipmentIndex(RoR2Content.Equipment.Scanner.equipmentIndex);
            args.senderMaster.inventory.GiveItem(RoR2Content.Items.BoostEquipmentRecharge, 100);
        }

        [ConCommand(commandName = "list_mods", flags = ConVarFlags.None, helpText = "Give Radar Scanner and Equipment Cooldown Reduction hidden item")]
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
        public static void CCList_Pickups(ConCommandArgs args)
        {
            string debug = "";
            for(int i = 0; i < PickupCatalog.entries.Length; i++)
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
        public static void CCList_Music(ConCommandArgs args)
        {
            string debug = "";
            for(int i = 0; i < MusicTrackCatalog.musicTrackDefs.Length; i++)
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
        
        [ConCommand(commandName = "goto_boss", flags = ConVarFlags.ExecuteOnServer, helpText = "Tp to Teleporter, Mithrix or False son depending on stage.")]
        public static void CC_GotoMithrix(ConCommandArgs args)
        {
            Component senderBody = args.GetSenderBody();
            Vector3 newPosition = Vector3.zero;
            if (Stage.instance.sceneDef.cachedName == "moon2")
            {
                newPosition = new Vector3(-11, 490, 80);
            }
            else if (Stage.instance.sceneDef.cachedName == "meridian")
            {
                newPosition = new Vector3(85.2065f, 146.5167f, -70.5265f);
            }
            else if (Stage.instance.sceneDef.cachedName == "voidraid")
            {
                bool crab = false;
                for (int i = 0; i < CharacterBody.instancesList.Count; i++)
                {
                    if (CharacterBody.instancesList[i].name.StartsWith("MiniVoidRaid"))
                    {
                        crab = true;
                        newPosition = CharacterBody.instancesList[i].corePosition;
                    }
                }
                if (!crab)
                {
                    newPosition = new Vector3(-105f, 0.2f, 92f);
                }
                
            }
            else if (TeleporterInteraction.instance)
            {
                newPosition = TeleporterInteraction.instance.transform.position;
            }
            else
            {
                Debug.Log("No Teleporter or Boss location set");
                return;
            }
            TeleportHelper.TeleportGameObject(senderBody.gameObject, newPosition);
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
            List<InfiniteTowerWaveCategory> categories = Resources.FindObjectsOfTypeAll<InfiniteTowerWaveCategory>().ToList();
            foreach (InfiniteTowerWaveCategory category in categories)
            {
                string debug = "_____\n" + category.name+"";
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

        [ConCommand(commandName = "simu_softlock", flags = ConVarFlags.SenderMustBeServer, helpText = "Attempts to end current wave or otherwise spawn a portal")]
        public static void CCSimuSoftlock(ConCommandArgs args)
        {

            if (!Run.instance)
            {
                Debug.LogWarning("No Run");
            };
            if (Run.instance is not InfiniteTowerRun)
            {
                Debug.LogWarning("No Simu Run");
            };

            InfiniteTowerRun run = (InfiniteTowerRun)Run.instance;
            try
            {
                if (run.waveController)
                {
                    run.waveController.OnAllEnemiesDefeatedServer();
                    run.waveController.ForceFinish();
                }
                else
                {
                    run.OnWaveAllEnemiesDefeatedServer(null);
                    run.CleanUpCurrentWave();
                    if (run.waveInstance)
                    {
                        Object.Destroy(run.waveInstance);
                    }
                    if (run.IsStageTransitionWave())
                    {
                    }
                    else
                    {
                        run.BeginNextWave();

                    }
                }
                if (run.safeWardController)
                {
                    if (run.safeWardController.wardStateMachine.state is not EntityStates.InfiniteTowerSafeWard.Active)
                    {
                        run.safeWardController.wardStateMachine.SetState(new EntityStates.InfiniteTowerSafeWard.Active());
                    }

                }
            }
            catch (System.Exception e)
            {
                run.PickNextStageSceneFromCurrentSceneDestinations();
                DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(run.stageTransitionPortalCard, new DirectorPlacementRule
                {
                    minDistance = 0f,
                    maxDistance = run.stageTransitionPortalMaxDistance,
                    placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                    position = run.safeWardController.transform.position,
                    spawnOnTarget = run.safeWardController.transform
                }, run.safeWardRng));
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage
                {
                    baseToken = run.stageTransitionChatToken
                });
                if (run.safeWardController)
                {
                    run.safeWardController.WaitForPortal();
                }
                Debug.LogWarning(e.ToString());
            }
            Debug.LogWarning("Report any errors");
        }


        [ConCommand(commandName = "list_itemTags", flags = ConVarFlags.None, helpText = "List all items and their item tags")]
        public static void CCDumpItemTags(ConCommandArgs args)
        {
            for (int i = 0; i < ItemCatalog.allItemDefs.Length; i++)
            {
                ItemDef def = ItemCatalog.allItemDefs[i];
                string info = "\n__" + def.nameToken + "__";
                for (int j = 0; j < def.tags.Length; j++)
                {
                    info += "\n" + def.tags[j].ToString();
                }
                Debug.Log(info);
            }
        }

        [ConCommand(commandName = "list_dccs", flags = ConVarFlags.None, helpText = "List all dccs, dp, csc, isc. Will take a moment & Restart afterwards. Does not get modded dccs or dp")]
        public static void CCDumpAllDCCS(ConCommandArgs args)
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
