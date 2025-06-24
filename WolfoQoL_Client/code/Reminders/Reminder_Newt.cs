using RoR2;

namespace WolfoQoL_Client.Reminders
{
    public class Reminders_Newt
    {
        public static void Start()
        {
            On.RoR2.PortalStatueBehavior.PreStartClient += CheckIfAnyNewtsSpawnedForReminder;


            //On.RoR2.TeleporterInteraction.Start += Clear_Newt_SpawnedWithBlue;
            //On.RoR2.PortalStatueBehavior.GrantPortalEntry += Newt_ReminderClear;
            //Works for Host too?
            On.RoR2.TeleporterInteraction.OnSyncShouldAttemptToSpawnShopPortal += NewtClear_OnClient;
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
                TreasureReminder.newtShrineSpawned++;
                //Adds -> OnEnable -> only then sees NewtAltar bool
                self.gameObject.AddComponent<Reminders.SpawnListener>().newtAltar = true;
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
