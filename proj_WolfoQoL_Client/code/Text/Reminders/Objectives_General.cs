using RoR2;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace WolfoQoL_Client.Reminders
{
    public static class Objectives_General
    {

        public static void Start()
        {
            On.RoR2.Stage.PreStartClient += ObjectivesOnStage;
            On.EntityStates.Interactables.MSObelisk.ReadyToEndGame.OnEnter += ReadyToEndGame_OnEnter;
        }

        private static void ReadyToEndGame_OnEnter(On.EntityStates.Interactables.MSObelisk.ReadyToEndGame.orig_OnEnter orig, EntityStates.Interactables.MSObelisk.ReadyToEndGame self)
        {
            orig(self);
            /*var objective = self.gameObject.GetComponent<GenericObjectiveProvider>();
            self.gameObject.GetComponentInChildren<OnPlayerEnterEvent>().action.m_PersistentCalls.AddListener(
            new PersistentCall
            {
                m_Target = objective,
                m_MethodName = "SetActive",
                m_Mode = PersistentListenerMode.Object,
                m_Arguments = new ArgumentCache
                {
                    boolArgument = true,
                }
            });*/



        }

        public static void ObjectivesOnStage(On.RoR2.Stage.orig_PreStartClient orig, Stage self)
        {
            //Goolake Secret Objective
            //Artifact Trial Objective
            orig(self);
            switch (SceneInfo.instance.sceneDef.baseSceneName)
            {
                case "goolake":
                    GameObject DoorIsOpenedEffect = GameObject.Find("/HOLDER: Secret Ring Area Content/Entrance/GLRuinGate/DoorIsOpenedEffect/");
                    GameObject RingEventController = GameObject.Find("/HOLDER: Secret Ring Area Content/ApproxCenter/RingEventController/");
                    GenericObjectiveProvider objective = DoorIsOpenedEffect.AddComponent<GenericObjectiveProvider>();
                    objective.objectiveToken = "OBJECTIVE_DESERT_ELDERS";
                    var Event = RingEventController.GetComponent<OnPlayerEnterEvent>();
                    UnityEngine.Events.PersistentCall newCall = new UnityEngine.Events.PersistentCall
                    {
                        m_Target = objective,
                        m_MethodName = "Destroy",
                        m_Mode = UnityEngine.Events.PersistentListenerMode.Object,
                        m_Arguments = new UnityEngine.Events.ArgumentCache
                        {
                            m_ObjectArgument = objective
                        }
                    };
                    Event.action.m_PersistentCalls.AddListener(newCall);

                    break;
                case "artifactworld":
                case "artifactworld01":
                case "artifactworld02":
                case "artifactworld03":
                    GameObject ArtifactDisplay = GameObject.Find("/ArtifactDisplay/SetpiecePickup");
                    if (ArtifactDisplay == null)
                    {
                        ArtifactDisplay = GameObject.Find("/HOLDER: Design/ArtifactDisplay/SetpiecePickup");
                    }
                    GenericPickupController pickup = ArtifactDisplay.GetComponent<GenericPickupController>();
                    string artifactname = "ArtifactIndex.None";
                    if (pickup.pickupIndex == PickupIndex.none)
                    {
                        pickup.pickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.BoostHp.itemIndex);
                    }
                    else if (pickup.pickupIndex != PickupIndex.none)
                    {
                        ArtifactDef tempartifactdef = ArtifactCatalog.GetArtifactDef(pickup.pickupIndex.pickupDef.artifactIndex);
                        if (tempartifactdef)
                        {
                            artifactname = Language.GetString(tempartifactdef.nameToken);
                            String[] spltis = artifactname.Split(" ");
                            if (spltis.Length > 0)
                            {
                                artifactname = spltis[spltis.Length - 1];
                                Log.LogMessage(artifactname);
                            }
                        }
                        if (WConfig.ArtifactOutline.Value == true)
                        {
                            Highlight tempartifact = pickup.gameObject.GetComponent<Highlight>();
                            tempartifact.pickupIndex = pickup.pickupIndex;
                            tempartifact.highlightColor = Highlight.HighlightColor.pickup;
                            tempartifact.isOn = true;
                        }
                    }
                    pickup.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = string.Format(Language.GetString("OBJECTIVE_ARTIFACT_TRIAL"), artifactname);
                    break;
                case "mysteryspace":
                    GameObject MSObelisk = GameObject.Find("/MS_Obelisk");
                    MSObelisk.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.QuestionMarkIcon;
                    GameObject MSObjective = new GameObject("MSObjective");
                    MSObjective.transform.SetParent(MSObelisk.transform);
                    //MSObjective.SetActive(false);
                    MSObjective.AddComponent<GenericObjectiveProvider>().objectiveToken = "OBJECTIVE_GLASS_YOURSELF";

                    /*MSObelisk.gameObject.GetComponentInChildren<OnPlayerEnterEvent>().action.m_PersistentCalls.AddListener(
                    new PersistentCall
                    {
                        m_Target = MSObjective,
                        m_MethodName = "SetActive",
                        m_Mode = PersistentListenerMode.Bool,
                        m_Arguments = new ArgumentCache
                        {
                            boolArgument = true,
                        }
                    });*/

                    break;
                case "conduitcanyon":
                    GameObject CC_StageEvents = GameObject.Find("/HOLDER: Stage Events");

                    CC_StageEvents.transform.GetChild(3).gameObject.AddComponent<ConduitCanyonKeysInstalledTracker>().pedestals = CC_StageEvents.transform.GetChild(1).GetChild(3).GetComponent<PowerPedestalObserver>();
                    CC_StageEvents.transform.GetChild(4).gameObject.AddComponent<ConduitCanyonKeysInstalledTracker>().pedestals = CC_StageEvents.transform.GetChild(2).GetChild(0).GetComponent<PowerPedestalObserver>();

                    break;
            }
        }


    }


    public class ConduitCanyonKeysInstalledTracker : MonoBehaviour
    {
        public string baseToken;
        public PowerPedestalObserver pedestals;
        public GenericObjectiveProvider objective;

        public void Start()
        {
            objective = this.GetComponent<GenericObjectiveProvider>();
            baseToken = objective.objectiveToken;

            pedestals.perPedestalObserverActions[0].ActionsOnCompletion.m_PersistentCalls.AddListener(new PersistentCall
            {
                m_Target = this,
                m_MethodName = "UpdateAmount",
                m_Mode = UnityEngine.Events.PersistentListenerMode.Void,
            });
            pedestals.perPedestalObserverActions[0].ActionsOnCompletion.m_CallsDirty = true;
            UpdateAmount();
        }

        public void UpdateAmount()
        {
            objective.objectiveToken = Language.GetString(baseToken) + $" ({pedestals.completedPowerPedestals.Count}/{pedestals.observedPedestals.Length})";
        }
    }


}
