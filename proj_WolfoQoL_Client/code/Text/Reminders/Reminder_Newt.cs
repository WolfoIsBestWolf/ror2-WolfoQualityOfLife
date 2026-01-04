using RoR2;

namespace WolfoQoL_Client.Reminders
{
    public static class Reminders_Newt
    {
        public static void Start()
        {
            On.RoR2.PortalStatueBehavior.PreStartClient += CheckIfAnyNewtsSpawnedForReminder;


            //On.RoR2.TeleporterInteraction.Start += Clear_Newt_SpawnedWithBlue;
            //On.RoR2.PortalStatueBehavior.GrantPortalEntry += Newt_ReminderClear;
            //Works for Host too?
            On.RoR2.TeleporterInteraction.OnSyncShouldAttemptToSpawnShopPortal += NewtClear_OnClient;


            //On.EntityStates.Missions.AccessCodes.Node.NodesOnAndReady.OnEnter += AccessCode_AddReminder;
            On.EntityStates.Missions.AccessCodes.Node.NodeActive.OnEnter += AccessCode_CompleteReminder;
            On.EntityStates.Missions.AccessCodes.Node.Off.OnEnter += AccessCode_FailReminder;
        }

        private static void AccessCode_FailReminder(On.EntityStates.Missions.AccessCodes.Node.Off.orig_OnEnter orig, EntityStates.Missions.AccessCodes.Node.Off self)
        {
            orig(self);
            if (WConfig.cfgReminder_AccessNode.Value)
            {
                Reminders_Main.FailObjective(TreasureReminder.instance.Objective_AccessNode);
            }

        }

        private static void AccessCode_AddReminder(On.EntityStates.Missions.AccessCodes.Node.NodesOnAndReady.orig_OnEnter orig, EntityStates.Missions.AccessCodes.Node.NodesOnAndReady self)
        {
            orig(self);
            //TreasureReminder.accessNodeSpawned = true;
            if (WConfig.cfgReminder_AccessNode.Value)
            {
                TreasureReminder.instance.Objective_AccessNode.enabled = true;
            }
        }

        private static void AccessCode_CompleteReminder(On.EntityStates.Missions.AccessCodes.Node.NodeActive.orig_OnEnter orig, EntityStates.Missions.AccessCodes.Node.NodeActive self)
        {
            orig(self);
            if (WConfig.cfgReminder_AccessNode.Value)
            {
                TreasureReminder.instance.Objective_AccessNode.enabled = false;
                //self.gameObject.GetComponent<GenericObjectiveProvider>().enabled = false;
            }
        }

        private static void NewtClear_OnClient(On.RoR2.TeleporterInteraction.orig_OnSyncShouldAttemptToSpawnShopPortal orig, TeleporterInteraction self, bool newValue)
        {
            orig(self, newValue);
            if (newValue)
            {
                if (TreasureReminder.instance)
                {
                    Reminders_Main.CompleteObjective(TreasureReminder.instance.Objective_NewtShrine);
                }
            }
        }


        private static void CheckIfAnyNewtsSpawnedForReminder(On.RoR2.PortalStatueBehavior.orig_PreStartClient orig, PortalStatueBehavior self)
        {
            orig(self);
            if (self.portalType == PortalStatueBehavior.PortalType.Shop)
            {
                TreasureReminder.instance.newtShrineSpawned++;
                //Adds -> OnEnable -> only then sees NewtAltar bool
                self.gameObject.AddComponent<SpawnListener>().interactable = SpawnListener.Interactable.newtAltar;
            }
        }

        private static void Clear_Newt_SpawnedWithBlue(On.RoR2.TeleporterInteraction.orig_Start orig, TeleporterInteraction self)
        {
            orig(self);
            if (self.Network_shouldAttemptToSpawnShopPortal)
            {
                if (TreasureReminder.instance)
                {
                    Reminders_Main.CompleteObjective(TreasureReminder.instance.Objective_NewtShrine);
                }
            }
        }

        private static void Newt_ReminderClear(On.RoR2.PortalStatueBehavior.orig_GrantPortalEntry orig, PortalStatueBehavior self)
        {
            orig(self);
            if (self.portalType == PortalStatueBehavior.PortalType.Shop)
            {
                if (TreasureReminder.instance)
                {
                    Reminders_Main.CompleteObjective(TreasureReminder.instance.Objective_NewtShrine);
                }
            }
        }


    }
}
