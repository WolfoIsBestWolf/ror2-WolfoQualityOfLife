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
                //Destination portal has reminder


                Addressables.LoadAssetAsync<GameObject>(key: "7a6f6b9396b554745b555c02bf58ea0b").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_AC";
                Addressables.LoadAssetAsync<GameObject>(key: "aa32c88c45d3b974aba3b2cafedb5cb5").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_AC";
                //SolusWing portal has reminder
                //CE Virtual portal is just to leave no reminder
                Addressables.LoadAssetAsync<GameObject>(key: "9592265fcd09fc643b2495b5e4ebac8f").WaitForCompletion().AddComponent<GenericObjectiveProvider>().objectiveToken = "REMINDER_PORTAL_SOLUSWEB";

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
    public class PortalObjective : GenericObjectiveProvider
    {
        public void Start()
        {

        }
    }
}
