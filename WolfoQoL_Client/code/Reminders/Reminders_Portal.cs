using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQoL_Client.Reminders
{
    public class Reminders_Portal
    {
        public static void Start()
        {
            if (WConfig.cfgRemindersPortal.Value == true)
            {
                LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalArtifactworld").AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_ARTIFACT";
                LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalGoldshores").AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_GOLD";
                LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalShop").AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_LUNAR";
                LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/PortalMS").AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_MS";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_VOID";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "OBJECTIVE_VOID_DEEP_PORTAL";

                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_GREEN";
                //Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PM DestinationPortal.prefab").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "Proceed through the <style=cIsHealing>Destination Portal</style>";
            }
            On.RoR2.SceneExitController.Begin += RemoveReminder_Host;
            On.RoR2.GenericInteraction.OnDeserialize += RemoveReminder_Client;
            On.RoR2.ArenaMissionController.MissionCompleted.OnEnter += MissionCompleted_OnEnter;
        }

        private static void MissionCompleted_OnEnter(On.RoR2.ArenaMissionController.MissionCompleted.orig_OnEnter orig, ArenaMissionController.MissionCompleted self)
        {
            orig(self);
            if (WConfig.cfgRemindersPortal.Value == true)
            {
                GameObject PortalArena = GameObject.Find("/PortalArena");
                if (PortalArena)
                {
                    PortalArena.AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_NULL";
                }
            }
        }

        private static void RemoveReminder_Host(On.RoR2.SceneExitController.orig_Begin orig, SceneExitController self)
        {
            orig(self);
            GenericObjectiveProvider objective = self.GetComponent<GenericObjectiveProvider>();
            if (objective)
            {
                Object.Destroy(objective);
            }
        }

        private static void RemoveReminder_Client(On.RoR2.GenericInteraction.orig_OnDeserialize orig, GenericInteraction self, NetworkReader reader, bool initialState)
        {
            var available = self.interactability;
            orig(self, reader, initialState);
            if (available == Interactability.Available && self.interactability != Interactability.Available)
            {
                //Probably means used
                GenericObjectiveProvider objective = self.GetComponent<GenericObjectiveProvider>();
                if (objective)
                {
                    Object.Destroy(objective);
                }
            }
        }

    }
}
