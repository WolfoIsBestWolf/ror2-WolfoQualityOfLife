using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using WolfoLibrary;

namespace WolfoQoL_Client.Reminders
{
    public static class Reminders_Portal
    {
        public static void Start()
        {
            On.RoR2.SceneExitController.Begin += RemoveReminder_Host;
            On.RoR2.GenericInteraction.OnDeserialize += RemoveReminder_Client;
            On.RoR2.ArenaMissionController.MissionCompleted.OnEnter += MissionCompleted_OnEnter;

            if (WConfig.cfgRemindersPortal.Value)
            {
                PortalObjective portal;

                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab").WaitForCompletion().AddComponent<PortalObjective>().style = "<style=cDeath>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalGoldshores/PortalGoldshores.prefab").WaitForCompletion().AddComponent<PortalObjective>().style = "<style=cGold>";
                portal = Addressables.LoadAssetAsync<GameObject>(key: "eadfcaf9ea3275e49858ed19f874db5a").WaitForCompletion().AddComponent<PortalObjective>();
                portal.style = "<style=cMysterySpace>";
                portal.blacklistedScene = SceneList.MysterySpace;
                portal.exit = true;
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalShop/PortalShop.prefab").WaitForCompletion().AddComponent<PortalObjective>().style = "<style=cIsLunar>";
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArena/PortalArena.prefab").WaitForCompletion().AddComponent<PortalObjective>().style = "<style=cIsVoid>";

                //Simu portal has objective
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/PortalVoid/PortalVoid.prefab").WaitForCompletion().AddComponent<PortalObjective>().style = "<style=cIsVoid>";
                portal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortal/DeepVoidPortal.prefab").WaitForCompletion().AddComponent<PortalObjective>();
                portal.style = "<style=cIsVoid>";
                portal.blacklistedScene = SceneList.VoidStage;


                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<PortalObjective>().style = "<style=cRebirth>";

                portal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PM DestinationPortal.prefab").WaitForCompletion().AddComponent<PortalObjective>();
                portal.style = "<style=cRebirth>";
                portal.onlyEclipse = true;

                Addressables.LoadAssetAsync<GameObject>(key: "7a6f6b9396b554745b555c02bf58ea0b").WaitForCompletion().AddComponent<PortalObjective>().style = "<color=#c6d5ff>";
                Addressables.LoadAssetAsync<GameObject>(key: "aa32c88c45d3b974aba3b2cafedb5cb5").WaitForCompletion().AddComponent<PortalObjective>().style = "<color=#c6d5ff>";
                //SolusWing portal has reminder
                //CE Virtual portal is just to leave no reminder
                Addressables.LoadAssetAsync<GameObject>(key: "9592265fcd09fc643b2495b5e4ebac8f").WaitForCompletion().AddComponent<PortalObjective>().style = "<color=#FF269C>";
            }
        }

        private static void MissionCompleted_OnEnter(On.RoR2.ArenaMissionController.MissionCompleted.orig_OnEnter orig, ArenaMissionController.MissionCompleted self)
        {
            orig(self);
            if (WConfig.cfgRemindersPortal.Value == true)
            {
                GameObject PortalArena = GameObject.Find("/PortalArena");
                if (PortalArena)
                {
                    PortalArena.AddComponent<PortalObjective>().style = "<style=cIsVoid>";
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
        public string style;
        public bool exit;
        public bool onlyEclipse;
        public SceneDef blacklistedScene;
        public void Awake()
        {
            if (onlyEclipse && Run.instance && !(Run.instance.selectedDifficulty >= DifficultyIndex.Eclipse1))
            {
                enabled = false;
            }
            if (blacklistedScene && !exit && blacklistedScene == SceneInfo.instance.sceneDef)
            {
                enabled = false;
            }
            if (WConfig.cfgRemindersPortal.Value == false)
            {
                enabled = false;
            }
        }
        public void Start()
        {
            string baseToken = "REMINDER_PORTAL_MAIN";
            if (exit)
            {
                if (blacklistedScene && blacklistedScene == SceneInfo.instance.sceneDef)
                {
                    baseToken = "REMINDER_PORTAL_EXIT";
                }
            }
            string nameToken = this.GetComponent<GenericDisplayNameProvider>().displayToken;
            if (Language.currentLanguage == null)
            {
                //????
                return;
            }
            if (Language.currentLanguage.TokenIsRegistered(baseToken))
            {
                objectiveToken = Language.GetStringFormatted(baseToken, style + Language.GetString(nameToken) + "</color>");
            }
            else
            {
                objectiveToken = Language.GetStringFormatted(baseToken, style + Language.english.GetLocalizedStringByToken(nameToken) + "</color>");
            }
        }
    }
}
