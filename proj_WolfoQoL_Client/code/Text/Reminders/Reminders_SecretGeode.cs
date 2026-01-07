using RoR2;
using UnityEngine;

namespace WolfoQoL_Client.Reminders
{
    public static class Reminders_SecretGeode
    {
        private static GeodeSecretMissionController geodeInstance;

        public static void Start()
        {
            On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionEntityStates.OnEnter += GeodeObjective_Add;
            //On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionRewardState.OnEnter += GeodeObjective_Clear;

            On.RoR2.GeodeSecretMissionController.AdvanceGeodeSecretMission += GeodeObjective_Update;
            On.RoR2.GeodeController.OnDeserialize += MakeUpdateOnClient;

        }

        private static void MakeUpdateOnClient(On.RoR2.GeodeController.orig_OnDeserialize orig, GeodeController self, UnityEngine.Networking.NetworkReader reader, bool initialState)
        {
            //Deseralize already only runs on Clients
            bool pre = self.available;

            orig(self, reader, initialState);
            if (pre == true && self.available == false && self.isSecretMissionGeode)
            {
                geodeInstance.AdvanceGeodeSecretMission();
            }
        }

        private static void GeodeObjective_Clear(On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionRewardState.orig_OnEnter orig, EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionRewardState self)
        {
            //Does not run on client for whatever reason
            //Collider event just happens on network ig?
            orig(self);
            Log.LogWarning("Secret Geode Reward");
            Object.Destroy(self.gameObject.GetComponent<GenericObjectiveProvider>());
        }

        private static void GeodeObjective_Add(On.EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionEntityStates.orig_OnEnter orig, EntityStates.Missions.GeodeSecretMission.GeodeSecretMissionEntityStates self)
        {
            orig(self);
            geodeInstance = self.geodeSecretMissionController;
            if (WConfig.cfgRemindersSecretGeode.Value)
            {
                Log.LogWarning("Secret Geode Start");
                if (geodeInstance.geodeInteractionsTracker == 0)
                {
                    string text = string.Format(Language.GetString("REMINDER_SECRET_GEODE"), 0, self.geodeSecretMissionController.numberOfGeodesNecessary);
                    self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = text;
                }
            }

        }

        private static void GeodeObjective_Update(On.RoR2.GeodeSecretMissionController.orig_AdvanceGeodeSecretMission orig, GeodeSecretMissionController self)
        {
            orig(self);
            Log.LogWarning("Secret Geode Advance");
            GenericObjectiveProvider Objective = self.gameObject.GetComponent<GenericObjectiveProvider>();
            if (Objective)
            {
                string text = string.Format(Language.GetString("REMINDER_SECRET_GEODE"), self.geodeInteractionsTracker, self.numberOfGeodesNecessary);
                Objective.objectiveToken = text;
                if (self.numberOfGeodesNecessary <= self.geodeInteractionsTracker)
                {
                    Log.LogWarning("Secret Geode Complete");
                    Object.Destroy(Objective);
                }
            }
        }

        private static void GeodeSecretMissionController_CheckIfRewardShouldBeGranted(On.RoR2.GeodeSecretMissionController.orig_CheckIfRewardShouldBeGranted orig, GeodeSecretMissionController self)
        {
            orig(self);
            if (self.numberOfGeodesNecessary <= self.geodeInteractionsTracker)
            {
                var objective = self.gameObject.GetComponent<GenericObjectiveProvider>();
                if (objective)
                {
                    Reminders_Main.CompleteObjective(objective);
                    Object.Destroy(objective);
                }
                Log.LogMessage("Secret Geode End");
            }
        }



    }
}