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
            On.EntityStates.NkuhanaChest.OpenPageReward.OnEnter += TrialPortalObjectives;
            
            if (WConfig.cfgRemindersPortal.Value)
            {
                PortalObjective portal;

                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalArtifactworld/PortalArtifactworld.prefab").WaitForCompletion().AddComponent<PortalObjective>().style = "<style=cDeath>";
                portal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/PortalGoldshores/PortalGoldshores.prefab").WaitForCompletion().AddComponent<PortalObjective>(); 
                portal.style = "<style=cGold>";
                portal.addAfterTP = true;   
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


                portal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PortalColossus.prefab").WaitForCompletion().AddComponent<PortalObjective>(); 
                portal.style = "<style=cRebirth>";
                portal.addAfterTP = true;

                portal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PM DestinationPortal.prefab").WaitForCompletion().AddComponent<PortalObjective>();
                portal.style = "<style=cRebirth>";
                portal.onlyEclipse = true;

                Addressables.LoadAssetAsync<GameObject>(key: "7a6f6b9396b554745b555c02bf58ea0b").WaitForCompletion().AddComponent<PortalObjective>().style = "<color=#99D9FF>";
                Addressables.LoadAssetAsync<GameObject>(key: "aa32c88c45d3b974aba3b2cafedb5cb5").WaitForCompletion().AddComponent<PortalObjective>().style = "<color=#99D9FF>";
                //SolusWing portal has reminder
                //CE Virtual portal is just to leave no reminder
                Addressables.LoadAssetAsync<GameObject>(key: "9592265fcd09fc643b2495b5e4ebac8f").WaitForCompletion().AddComponent<PortalObjective>().style = "<color=#FF269C>";

                portal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC4/TrialPortalEntry.prefab").WaitForCompletion().AddComponent<PortalObjective>();
                portal.style = "<color=#C8F30D>";
                portal.addAfterTP = true;
                portal.trialPortal = true;
                portal = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC4/TrialPortalEntryDanger.prefab").WaitForCompletion().AddComponent<PortalObjective>();
                portal.style = "<color=#C8F30D>";
                portal.addAfterTP = true;
                portal.trialPortal = true;
                
            }
        }

        //We tried putting it somewhere else
        //But the timer gets set so late and then needs to desereliaze that it's feesible.
        private static void TrialPortalObjectives(On.EntityStates.NkuhanaChest.OpenPageReward.orig_OnEnter orig, EntityStates.NkuhanaChest.OpenPageReward self)
        {
            orig(self);
            if (WConfig.cfgRemindersPortal.Value)
            {
                GameObject holder = ClassicStageInfo.instance.gameObject;
                GameObject More = new GameObject("TrialObjectiveTrial");
                GameObject Exit = new GameObject("TrialObjectiveLeave");

                More.transform.SetParent(holder.transform);
                Exit.transform.SetParent(holder.transform);

                string time = System.TimeSpan.FromSeconds((double)DLC4ProgressionController.instance.trialSelector.GetRemainingTrialTimeServer()).ToString("mm\\:ss");
                if (DLC4ProgressionController.instance.trialSelector.GetRemainingTrialTimeServer() <= 60)
                {
                    time = string.Format("<style=cDeath>{0}</style>", time);
                }
                More.AddComponent<GenericObjectiveProvider>().objectiveToken = Language.GetStringFormatted("REMINDER_PORTAL_TRIAL", time);
                Exit.AddComponent<GenericObjectiveProvider>().objectiveToken = Language.GetString("OBJECTIVE_CHAMPRUSH_EXIT1");

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
            RemoveReminder(self);
        }

        private static void RemoveReminder_Client(On.RoR2.GenericInteraction.orig_OnDeserialize orig, GenericInteraction self, NetworkReader reader, bool initialState)
        {
            var available = self.interactability;
            orig(self, reader, initialState);
            if (available == Interactability.Available && self.interactability != Interactability.Available)
            {
                SceneExitController sceneExitController = self.GetComponent<SceneExitController>();
                if (sceneExitController != null)
                {
                    RemoveReminder(sceneExitController);
                }
            }
        }

        private static void RemoveReminder(SceneExitController self)
        {
            GenericObjectiveProvider objective = self.GetComponent<PortalObjective>();
            if (objective && objective.enabled)
            {
                Object.Destroy(objective);
                return;
            }
            if (InstanceTracker.Any<TrialController>())
            {
                Transform TrialObjectiveTrial = ClassicStageInfo.instance.transform.Find("TrialObjectiveTrial");
                Transform TrialObjectiveLeave = ClassicStageInfo.instance.transform.Find("TrialObjectiveLeave");
                if (TrialObjectiveTrial == null || TrialObjectiveLeave == null)
                {
                    return;
                }
                if (self.useRunNextStageScene)
                {
                    TrialObjectiveTrial.GetComponent<GenericObjectiveProvider>().markCompletedOnRetired = false;
                }
                else
                {
                    TrialObjectiveLeave.GetComponent<GenericObjectiveProvider>().markCompletedOnRetired = false;
                }
                TrialObjectiveTrial.gameObject.SetActive(false);
                TrialObjectiveLeave.gameObject.SetActive(false);

            }
        }

    }
    public class PortalObjective : GenericObjectiveProvider
    {
        
        public string style;
        public bool exit;
        public bool onlyEclipse;
        public bool addAfterTP;
        public bool trialPortal;
        public SceneDef blacklistedScene;
        public bool blacklistedInHidden;
        public void Awake()
        {
            if (WConfig.cfgRemindersPortal.Value == false)
            {
                enabled = false;
                return;
            }
            if (onlyEclipse && Run.instance && !(Run.instance.selectedDifficulty >= DifficultyIndex.Eclipse1))
            {
                enabled = false;
            }
            if (blacklistedScene && blacklistedScene == SceneInfo.instance.sceneDef)
            {
                if (!exit)
                {
                    enabled = false;
                }
            }
            if (blacklistedInHidden && SceneInfo.instance.sceneDef.stageOrder > 5)
            {
                enabled = false;
                return;
            }
            if (trialPortal)
            {
                if (InstanceTracker.Any<TrialController>())
                {
                    enabled = false;
                    return;
                }
            }
            if (addAfterTP)
            {
                if (TeleporterInteraction.instance && !TeleporterInteraction.instance.isCharged)
                {
                    enabled = false;
                    TeleporterInteraction.onTeleporterChargedGlobal += TeleporterInteraction_onTeleporterChargedGlobal;
                }
            }
        }

        private void TeleporterInteraction_onTeleporterChargedGlobal(TeleporterInteraction obj)
        {
            enabled = true;
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
            if (!string.IsNullOrEmpty(this.objectiveToken))
            {
                baseToken = this.objectiveToken;
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
            if (this.trialPortal)
            {
                string time = System.TimeSpan.FromSeconds((double)DLC4ProgressionController.instance.trialSelector.GetRemainingTrialTimeServer()).ToString("mm\\:ss");
                if (DLC4ProgressionController.instance.trialSelector.GetRemainingTrialTimeServer() <= 60)
                {
                    objectiveToken = string.Format("{0} <style=cDeath>({1})</style>", objectiveToken, time);
                }
                else
                {
                    objectiveToken = string.Format("{0} ({1})", objectiveToken, time);
                }
            }
        }
    

        public void OnDestroy()
        {
            if (addAfterTP)
            {
                TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterInteraction_onTeleporterChargedGlobal;
            }
        }
    }
}
