using RoR2;
using RoR2.UI;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoFixes.Testing
{
    class Commands
    {

        public static void Start()
        {
            On.RoR2.TeamManager.Start += TeamManager_Start;
            SceneDirector.onPrePopulateSceneServer += SceneDirector_onPrePopulateSceneServer;
        }

        private static void TeamManager_Start(On.RoR2.TeamManager.orig_Start orig, TeamManager self)
        {
            orig(self);
            if (godEnemyBool)
            {
                TeamManager.instance.teamLevels[2] = godEnemyBool ? (uint)100001 : (uint)0;
            }
        }

        public static void SceneDirector_onPrePopulateSceneServer(SceneDirector obj)
        {
            if (noInteractables)
            {
                obj.interactableCredit = 0;
                obj.monsterCredit = 0;
                obj.teleporterSpawnCard = null;
            }
        }

        [ConCommand(commandName = "remove_all_unlocks", flags = ConVarFlags.None, helpText = "Removes all unlockables and achievements on current profile")]
        public static void CC_RemoveUnlocks(ConCommandArgs args)
        {
            var a = LocalUserManager.GetFirstLocalUser();
            var b = a.userProfile;
            var c = b.statSheet;

            if (b != null)
            {
                for (int i = 0; UnlockableCatalog.unlockableCount > i; i++)
                {
                    c.RemoveUnlockable((UnlockableIndex)i);
                }
                for (int i = b.achievementsList.Count - 1; 0 <= i; i--)
                {
                    b.RevokeAchievement(b.achievementsList[i]);
                }
                b.RequestEventualSave();
            }

        }



        [ConCommand(commandName = "cooldown", flags = ConVarFlags.ExecuteOnServer, helpText = "Removes Skill Cooldown for current stage")]
        [ConCommand(commandName = "nocooldown", flags = ConVarFlags.ExecuteOnServer, helpText = "Removes Skill Cooldown for current stage")]
        public static void CC_Cooldown(ConCommandArgs args)
        {
            if (!args.senderMaster)
            {
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                return;
            }
            GenericSkill[] slots = args.senderMaster.GetBody().GetComponents<GenericSkill>();
            foreach (GenericSkill slot in slots)
            {
                slot.cooldownOverride = 0.01f;
            }
        }


        [ConCommand(commandName = "skill", flags = ConVarFlags.ExecuteOnServer, helpText = "Switch Skills of current survivor Shorthand")]
        [ConCommand(commandName = "swapskill", flags = ConVarFlags.ExecuteOnServer, helpText = "Switch Skills of current survivor Shorthand")]
        public static void CC_Skill(ConCommandArgs args)
        {
            if (!args.senderMaster)
            {
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                return;
            }
            if (!NetworkServer.active)
            {
                return;
            }
            BodyIndex argBodyIndex = args.senderMaster.GetBody().bodyIndex;
            int argInt = args.GetArgInt(0);
            int argInt2 = args.GetArgInt(1);
            UserProfile userProfile = args.GetSenderLocalUser().userProfile;
            Loadout loadout = new Loadout();
            userProfile.loadout.Copy(loadout);
            loadout.bodyLoadoutManager.SetSkillVariant(argBodyIndex, argInt, (uint)argInt2);
            userProfile.SetLoadout(loadout);
            if (args.senderMaster)
            {
                args.senderMaster.SetLoadoutServer(loadout);
            }
            if (args.senderBody)
            {
                args.senderBody.SetLoadoutServer(loadout);
            }
        }


        [ConCommand(commandName = "invis", flags = ConVarFlags.None, helpText = "Turn off your model for screenshots")]
        [ConCommand(commandName = "model", flags = ConVarFlags.None, helpText = "Turn off your model for screenshots")]
        public static void CC_TurnOffModel(ConCommandArgs args)
        {
            if (!args.senderMaster)
            {
                Debug.Log("No Master");
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                Debug.Log("No Body");
                return;
            }
            GameObject mdl = args.senderMaster.GetBody().GetComponent<ModelLocator>().modelTransform.gameObject;
            mdl.SetActive(!mdl.activeSelf);

        }

        [ConCommand(commandName = "hud", flags = ConVarFlags.None, helpText = "Enable/disable the HUD")]
        [ConCommand(commandName = "ui", flags = ConVarFlags.None, helpText = "Enable/disable the HUD")]
        public static void CC_ToggleHUD(ConCommandArgs args)
        {
            HUD.cvHudEnable.SetBool(!HUD.cvHudEnable.value);
        }

        public static bool noInteractables;
        [ConCommand(commandName = "no_interactables", flags = ConVarFlags.ExecuteOnServer, helpText = "Prevent interactables from being spawned")]
        public static void CC_NoInteractables(ConCommandArgs args)
        {
            noInteractables = !noInteractables;
            Debug.Log(noInteractables ? "Interactables no longer spawn" : "Interactables spawn again");
        }

        public static bool godEnemyBool;
        [ConCommand(commandName = "godenemy", flags = ConVarFlags.ExecuteOnServer, helpText = "Inflate enemy level to make them invulnerable")]
        public static void CCGodEnemy(ConCommandArgs args)
        {
            godEnemyBool = !godEnemyBool;
            TeamManager.instance.teamLevels[2] = godEnemyBool ? (uint)100001 : (uint)0;
            Debug.Log(godEnemyBool ? "Enemy level set to 100001" : "Enemy level set back to normal");
        }

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

        [ConCommand(commandName = "set_damage", flags = ConVarFlags.ExecuteOnServer, helpText = "Sets damage.")]
        public static void CC_Damage(ConCommandArgs args)
        {
            if (!args.senderMaster)
            {
                return;
            }
            if (!args.senderMaster.GetBody())
            {
                Debug.Log("No Body");
                return;
            }
            float newDamage = (float)Convert.ToInt16(args[0]);
            args.senderMaster.GetBody().baseDamage = newDamage;
            args.senderMaster.GetBody().levelDamage = 0;
            args.senderMaster.GetBody().damage = newDamage;

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
            else if (Stage.instance.sceneDef.cachedName == "mysteryspace")
            {
                newPosition = new Vector3(362.9097f, -151.5964f, 213.0157f);
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
                for (int i = 0; i < CharacterBody.instancesList.Count; i++)
                {
                    if (CharacterBody.instancesList[i].isBoss)
                    {
                        newPosition = CharacterBody.instancesList[i].corePosition;
                    }
                }
            }
            if (newPosition == Vector3.zero)
            {
                Debug.Log("No Teleporter, Specific Location or Boss Monster found.");
                return;
            }
            TeleportHelper.TeleportGameObject(senderBody.gameObject, newPosition);
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
                        GameObject.Destroy(run.waveInstance);
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


    }

}
