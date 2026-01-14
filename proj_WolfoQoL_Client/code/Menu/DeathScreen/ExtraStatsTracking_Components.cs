using HG;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace WolfoQoL_Client.DeathScreen
{
    public class RunExtraStatTracker : MonoBehaviour
    {
        public static RunExtraStatTracker instance;
        public bool alreadyCountedDronesForStage = false;

        //public Dictionary<string, int> dic_missedChests;
        //public Dictionary<string, int> dic_missedDrones;

        public GameObject latestWaveUiPrefab;
        public int missedChests = 0;
        public int missedShrineChanceItems = 0;
        //public int missedItems;
        public int missedDrones;
        public int missedLemurians;
        public bool expectingLoop = false;
        public bool isSaveAndContinuedRun = false;
        public List<SceneDef> visitedScenes;
        //public List<List<SceneDef>> visitedScenesTOTAL;

        public void OnEnable()
        {
            instance = this;
            //dic_missedChests = new Dictionary<string, int>();
            //dic_missedDrones = new Dictionary<string, int>();
            visitedScenes = new List<SceneDef>();
            //visitedScenesTOTAL = new List<List<SceneDef>>();
            //visitedScenesTOTAL.Add(visitedScenes);
            SceneCatalog.onMostRecentSceneDefChanged += this.HandleMostRecentSceneDefChanged;

            isSaveAndContinuedRun = this.GetComponent<Run>().stageClearCount != 0;
        }
        public void OnDisable()
        {
            instance = null;
            SceneCatalog.onMostRecentSceneDefChanged -= this.HandleMostRecentSceneDefChanged;
            /*foreach (SceneDef sceneDef in visitedScenes)
            {
                Log.LogMessage(visitedScenes);
            }*/
        }

        public void TestUnlocks(int amount = 5)
        {
            int done = 0;
            while (done < amount)
            {
                int random = Run.instance.runRNG.RangeInt(0, UnlockableCatalog.unlockableCount);
                var def = UnlockableCatalog.GetUnlockableDef((UnlockableIndex)random);
                if (!def.hidden)
                {
                    done++;
                    foreach (NetworkUser networkUser in NetworkUser.readOnlyInstancesList)
                    {
                        if (networkUser.isParticipating)
                        {
                            networkUser.ServerHandleUnlock(def);
                        }
                    }
                }

            }
        }

        public void TestStageList(int amount = 19)
        {
            int done = 0;
            int lookingFor = 2;
            while (done < amount)
            {
                SceneDef def = SceneCatalog.allStageSceneDefs[Run.instance.runRNG.RangeInt(0, SceneCatalog.allStageSceneDefs.Length)];
                while (def.stageOrder != lookingFor)
                {
                    def = SceneCatalog.allStageSceneDefs[Run.instance.runRNG.RangeInt(0, SceneCatalog.allStageSceneDefs.Length)];
                }
                done++;
                lookingFor++;
                if (lookingFor == 6)
                {
                    lookingFor = 1;
                }
                visitedScenes.Add(def);
            }
        }
        public void TestStageList_LongestLegitRun()
        {
            visitedScenes = new List<SceneDef>
            {
                SceneCatalog.FindSceneDef("golemplains"),
                SceneCatalog.FindSceneDef("bazaar"),
                SceneCatalog.FindSceneDef("goldshores"),
                SceneCatalog.FindSceneDef("goolake"),
                SceneCatalog.FindSceneDef("bazaar"),
                SceneCatalog.FindSceneDef("arena"),
                SceneCatalog.FindSceneDef("frozenwall"),
                SceneCatalog.FindSceneDef("bazaar"),
                SceneCatalog.FindSceneDef("meridian"),
                SceneCatalog.FindSceneDef("conduitcanyon"),
                SceneCatalog.FindSceneDef("solutionalhaunt"),
                SceneCatalog.FindSceneDef("computationalexchange"),
                SceneCatalog.FindSceneDef("solusweb"),
                SceneCatalog.FindSceneDef("skymeadow"),
                SceneCatalog.FindSceneDef("moon2"),
                SceneCatalog.FindSceneDef("voidraid"),
            };
        }
        public int lastSeenLoopCount = 0;
        private void HandleMostRecentSceneDefChanged(SceneDef newSceneDef)
        {
            if (newSceneDef.stageOrder == 0)
            {
                return;
            }
            if (newSceneDef.sceneType == SceneType.Menu ||
                newSceneDef.sceneType == SceneType.Cutscene ||
                newSceneDef.sceneType == SceneType.Invalid)
            {
                return;
            }
            //Was on Stage 5 -> Next normal stage
            //As loop decider
            alreadyCountedDronesForStage = false;
            if (Run.instance.ruleBook.stageOrder == StageOrder.Normal)
            {
                //If 5 -> 1 then consider loop, only then.
                if (newSceneDef.stageOrder == 5)
                {
                    expectingLoop = true;
                    lastSeenLoopCount = Run.instance.loopClearCount;
                }
                else if (newSceneDef.stageOrder == 1 && expectingLoop)
                {
                    expectingLoop = false;
                    //visitedScenes = new List<SceneDef>();
                    //visitedScenesTOTAL.Add(visitedScenes);
                }
            }
            else
            {
                if (lastSeenLoopCount != Run.instance.loopClearCount)
                {
                    expectingLoop = true;
                    lastSeenLoopCount = Run.instance.loopClearCount;
                }
                if (expectingLoop)
                {
                    if (newSceneDef.stageOrder < 6)
                    {
                        expectingLoop = false;
                        //visitedScenes = new List<SceneDef>();
                        //visitedScenesTOTAL.Add(visitedScenes);
                    }
                }
            }
            visitedScenes.Add(newSceneDef);
        }


    }

    public class PlayerMaster_ExtraStatTracker : MonoBehaviour
    {
        public static Dictionary<GameObject, PlayerMaster_ExtraStatTracker> playerBodyToTracker = new Dictionary<GameObject, PlayerMaster_ExtraStatTracker>();

        public bool gameOverWithDisabled = false;

        public float timeAliveReal;

        //Unused
        /*public int totalBuffsGotten;
        public float timeSpentInChargeZone;
        public int skillActivations;
        */

        public ulong[] perMinionDamage;
        public BodyIndex strongestMinion = (BodyIndex)(-2); //DeathScreenOnly
        public ulong strongestMinionDamage; //DeathScreenOnly
 
        public int timesJumped; //Client-Only
        public ulong minionDamageTaken;
        public ulong minionHealing;
        public int minionDeaths;

        public int spentLunarCoins;

        public int scrappedItems;
        public int scrappedDrones;
        public int lemuriansHatched;

        public ulong dotDamageDone; //Can be client??
        public ulong damageBlocked; //Definitely Server Only

        public string latestDetailedDeathMessage;

        public CharacterMaster master;
        public void OnEnable()
        {
            perMinionDamage = new ulong[BodyCatalog.bodyCount];
            master = this.GetComponent<CharacterMaster>();
            master.onBodyStart += Master_onBodyStart;

        }

        //Cannot alter vanilla one because vanilla tracking all server side.
        public void FixedUpdate()
        {
            if (this.master.hasBody)
            {
                timeAliveReal += Time.fixedDeltaTime;
            }
        }


        public void Start()
        {
            if (!master.hasEffectiveAuthority)
            {
                timesJumped = -1;
                damageBlocked = 0;
            }
        }

        public void OnDisable()
        {
            if (master)
            {
                master.onBodyStart -= Master_onBodyStart;
            }
            //This shit would need to be per profile somehow ig.
            //AddAllMinionDamageToStats();
        }

        public void EvaluateStrongestMinion()
        {
            ulong val = 0;
            int chosen = -1;
            for (int i = 0; i < perMinionDamage.Length; i++)
            {
                if (perMinionDamage[i] > val)
                {
                    val = perMinionDamage[i];
                    chosen = i;
                }
            }
            strongestMinion = (BodyIndex)chosen;
            strongestMinionDamage = val;
        }
        public void AddAllMinionDamageToStats()
        {
            for (int i = 0; i < perMinionDamage.Length; i++)
            {
                if (perMinionDamage[i] > 0)
                {
                    WStats.AddStat(i, perMinionDamage[i]);
                }
            }
        }

        private GameObject lastSeenBody;
        private void Master_onBodyStart(CharacterBody body)
        {
            latestDetailedDeathMessage = string.Empty;
            if (master.hasEffectiveAuthority)
            {
                body.onJump += OnJump;
            }
            body.gameObject.AddComponent<PlayerBody_DamageBlockedTracker>().tracker = this;

            if (lastSeenBody)
            {
                playerBodyToTracker.Remove(lastSeenBody);
            }
            else
            {
                foreach (var key in playerBodyToTracker.Keys.ToArray())
                {
                    if (key == null)
                    {
                        playerBodyToTracker.Remove(key);
                    }
                }
            }
            lastSeenBody = body.gameObject;
            playerBodyToTracker.Add(body.gameObject, this);
        }

        public void OnJump()
        {
            timesJumped++;
        }

        public class SyncValues : ChatMessageBase
        {
            public GameObject masterObject;

            public override string ConstructChatString()
            {
                return null;
            }
            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(masterObject);

                var tracker = masterObject.EnsureComponent<PlayerMaster_ExtraStatTracker>();

                writer.Write(tracker.timesJumped);
                writer.Write(tracker.damageBlocked);

            }
            public override void Deserialize(NetworkReader reader)
            {
                if (WQoLMain.NoHostInfo == true)
                {
                    return;
                }
                base.Deserialize(reader);
                masterObject = reader.ReadGameObject();
                if (masterObject == null)
                {
                    Log.LogWarning("No Master");
                    return;
                }
                var tracker = masterObject.EnsureComponent<PlayerMaster_ExtraStatTracker>();

                tracker.timesJumped = Mathf.Max(tracker.timesJumped, reader.ReadInt32());
                tracker.damageBlocked = System.Math.Max(tracker.damageBlocked, reader.ReadUInt64());

            }
        }

    }

    public class PlayerBody_DamageBlockedTracker : MonoBehaviour, IOnTakeDamageServerReceiver, IOnIncomingDamageServerReceiver
    {
        public PlayerMaster_ExtraStatTracker tracker;
        public void OnEnable()
        {
            GetComponent<HealthComponent>().onIncomingDamageReceivers = this.GetComponents<IOnIncomingDamageServerReceiver>();
            GetComponent<HealthComponent>().onTakeDamageReceivers = this.GetComponents<IOnTakeDamageServerReceiver>();
        }

        public void OnIncomingDamageServer(DamageInfo damageInfo)
        {
            //Damage reduced by block
            if (damageInfo.rejected)
            {
                tracker.damageBlocked += (ulong)damageInfo.damage;
            }
        }
        public void OnTakeDamageServer(DamageReport damageReport)
        {
            //Damage reduced by armor / rap etc
            if (damageReport.damageInfo.damage != damageReport.damageDealt)
            {
                tracker.damageBlocked += (ulong)(damageReport.damageInfo.damage - damageReport.damageDealt);
            }
        }
    }

    public class MinionMaster_ExtraStatsTracker : MonoBehaviour
    {
        public static Dictionary<GameObject, MinionMaster_ExtraStatsTracker> minionBodyToTracker = new Dictionary<GameObject, MinionMaster_ExtraStatsTracker>();

        public PlayerMaster_ExtraStatTracker tracker;
        public CharacterMaster master;
        public GameObject bodyObject;
        //public HealthComponent body;
        //public UnityAction deathEvent;
        public int bodyIndex = -1;

        public void OnEnable()
        {
            master = this.GetComponent<CharacterMaster>();
            tracker = master.minionOwnership.ownerMaster.GetComponent<PlayerMaster_ExtraStatTracker>();

            master.onBodyStart += OnBodyStart;
            master.onBodyDeath.AddListener(new UnityAction(this.OnBodyDeath));
        }
        public void OnDisable()
        {
            if (master)
            {
                master.onBodyDeath.RemoveListener(new UnityAction(this.OnBodyDeath));
            }
        }
        private void OnBodyStart(CharacterBody newBody)
        {
            if (bodyObject)
            {
                minionBodyToTracker.Remove(bodyObject);
            }
            else
            {
                foreach (var key in minionBodyToTracker.Keys.ToArray())
                {
                    if (key == null)
                    {
                        minionBodyToTracker.Remove(key);
                    }
                }
            }
            bodyObject = newBody.gameObject;
            bodyIndex = (int)newBody.bodyIndex;
            minionBodyToTracker.Add(bodyObject, this);


        }

        public void OnBodyDeath()
        {
            if (bodyObject)
            {
                if (DroneCollection.isPermamentMinion(this.gameObject))
                {
                    Log.LogMessage(this.gameObject.name + " died");
                    tracker.minionDeaths++;
                }
            }
        }


    }

    /*public class MinionBody_StatLocator : MonoBehaviour
    {
        public int bodyIndex;
        public PerPlayerMaster_ExtraStatTracker tracker;
    }*/


}
