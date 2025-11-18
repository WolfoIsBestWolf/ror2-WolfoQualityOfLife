using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace WolfoQoL_Client.DeathScreen
{
    public class RunExtraStatTracker : MonoBehaviour
    {
        public static RunExtraStatTracker instance;
        public bool alreadyCountedDronesForStage = false;

        public Dictionary<string, int> dic_missedChests;
        public Dictionary<string, int> dic_missedDrones;

        public GameObject latestWaveUiPrefab;
        public int missedChests = 0;
        public int missedShrineChanceItems = 0;
        public int missedItems;
        public int missedDrones;
        public int missedLemurians;
        public bool expectingLoop = false;
        public bool isSaveAndContinuedRun = false;
        public List<SceneDef> visitedScenes;
        //public List<List<SceneDef>> visitedScenesTOTAL;

        public void OnEnable()
        {
            instance = this;
            dic_missedChests = new Dictionary<string, int>();
            dic_missedDrones = new Dictionary<string, int>();
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
            foreach (SceneDef sceneDef in visitedScenes)
            {
                WQoLMain.log.LogMessage(visitedScenes);
            }
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

        public static void OnPurchaseDestroyed(OnDestroyCallback self)
        {
            if (!instance)
            {
                return;
            }
            PurchaseInteraction purchase = self.GetComponent<PurchaseInteraction>();
            if (purchase.available)
            {
                bool realChest = false;
                if (self.TryGetComponent<ChestBehavior>(out var chest))
                {
                    //Filter out frozenwall fan because hopoo james
                    if (chest.dropTable)
                    {
                        realChest = true;
                    }
                    if (SceneCatalog.mostRecentSceneDef.isFinalStage)
                    {
                        realChest = purchase.costType == CostTypeIndex.TreasureCacheItem;
                    }
                }
                if (realChest
                 || self.GetComponent<OptionChestBehavior>()
                 || self.GetComponent<RouletteChestController>())
                {
                    if (purchase.costType != CostTypeIndex.LunarCoin)
                    {
                        instance.missedChests++;
                    }
                    string display = purchase.GetDisplayName();
                    if (instance.dic_missedChests.ContainsKey(display))
                    {
                        instance.dic_missedChests[display]++;
                    }
                    else
                    {
                        instance.dic_missedChests.Add(display, 1);
                    }
                }
                if (self.TryGetComponent<ShrineChanceBehavior>(out var chance))
                {
                    instance.missedShrineChanceItems += chance.maxPurchaseCount - chance.successfulPurchaseCount;
                }
                if (self.GetComponent<SummonMasterBehavior>())
                {
                    instance.missedDrones++;
                    string display = purchase.GetDisplayName();
                    if (instance.dic_missedDrones.ContainsKey(display))
                    {
                        instance.dic_missedDrones[display]++;
                    }
                    else
                    {
                        instance.dic_missedDrones.Add(display, 1);
                    }
                }
            }
        }
        public static void OnMultiShopDestroyed(OnDestroyCallback self)
        {
            if (!instance)
            {
                return;
            }

            if (self.TryGetComponent<MultiShopController>(out var multi))
            {
                if (multi.available)
                {
                    instance.missedChests++;
                    string display = multi.terminalPrefab.GetComponent<PurchaseInteraction>().GetDisplayName();
                    if (instance.dic_missedChests.ContainsKey(display))
                    {
                        instance.dic_missedChests[display]++;
                    }
                    else
                    {
                        instance.dic_missedChests.Add(display, 1);
                    }
                }
            }
            else if (self.TryGetComponent<DroneVendorMultiShopController>(out var multiD))
            {
                if (multiD.available)
                {
                    instance.missedDrones++;
                    string display = multiD.terminalPrefab.GetComponent<PurchaseInteraction>().GetDisplayName();
                    if (instance.dic_missedDrones.ContainsKey(display))
                    {
                        instance.dic_missedDrones[display]++;
                    }
                    else
                    {
                        instance.dic_missedDrones.Add(display, 1);
                    }
                }
            }
        }

    }

    public class PerPlayer_ExtraStatTracker : MonoBehaviour
    {
        //Unused
        /*public int totalBuffsGotten;
        public float timeSpentInChargeZone;
        public int skillActivations;
        */

        public float[] perMinionDamage;
        public BodyIndex strongestMinion = (BodyIndex)(-2);
        public float strongestMinionDamage;

        //public Dictionary<ItemIndex, int> scrappedItemTotal;

        public int timesJumped; //Client-Only
        public float minionDamageTaken;
        public float minionHealing;
        public int minionDeaths; //Important for like, how many drones did bro rebuy i guess?
        //public int minionDeathsSuicide;

        public int spentLunarCoins; //Works
        public int itemsVoided;

        public int scrappedItems; //Works
        public int scrappedDrones; //DLC3?

        public int lemuriansHatched;


        public float dotDamageDone; //Can be client??
        public float damageBlocked; //Definitely Server Only

        public string latestDetailedDeathMessage;

        public CharacterMaster master;
        public void OnEnable()
        {
            perMinionDamage = new float[BodyCatalog.bodyCount];
            master = this.GetComponent<CharacterMaster>();
            master.onBodyStart += Master_onBodyStart;
            if (!NetworkServer.active)
            {

            }
            //scrappedItemTotal = new Dictionary<ItemIndex, int>();
        }
        public void Start()
        {
            //WolfoMain.log.LogMessage(master.hasAuthority);
            //WolfoMain.log.LogMessage(master.hasEffectiveAuthority);
            if (!master.hasEffectiveAuthority)
            {
                timesJumped = -1;
                damageBlocked = -1;
            }

        }
        public void OnDisable()
        {
            if (master)
            {
                master.onBodyStart -= Master_onBodyStart;
            }

        }
        public void EvaluateStrongestMinion()
        {
            float val = 0f;
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
        private void Master_onBodyStart(CharacterBody body)
        {
            latestDetailedDeathMessage = string.Empty;
            if (master.hasEffectiveAuthority)
            {
                body.onJump += OnJump;
            }
            body.gameObject.AddComponent<PlayerDamageBlockedTracker>().tracker = this;

        }

        public void OnJump()
        {
            timesJumped++;
        }

        public class SyncValues : ChatMessageBase
        {
            public GameObject masterObject;
            public int timesJumped;
            public int itemsVoided;
            public float damageBlocked;
            public override string ConstructChatString()
            {
                if (masterObject == null)
                {
                    WQoLMain.log.LogWarning("No Master");
                    return null;
                }
                var tracker = masterObject.GetComponent<PerPlayer_ExtraStatTracker>();
                tracker.timesJumped = Mathf.Max(tracker.timesJumped, timesJumped);
                tracker.damageBlocked = Mathf.Max(tracker.damageBlocked, damageBlocked);
                tracker.itemsVoided = Mathf.Max(tracker.itemsVoided, itemsVoided);


                return null;
            }
            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(masterObject);
                writer.Write(timesJumped);
                writer.Write(damageBlocked);
                writer.Write(itemsVoided);
            }
            public override void Deserialize(NetworkReader reader)
            {
                if (WQoLMain.NoHostInfo == true)
                {
                    return;
                }
                base.Deserialize(reader);
                masterObject = reader.ReadGameObject();
                timesJumped = reader.ReadInt32();
                damageBlocked = (float)reader.ReadSingle();
                itemsVoided = reader.ReadInt32();

            }
        }

    }

    public class PlayerDamageBlockedTracker : MonoBehaviour, IOnTakeDamageServerReceiver, IOnIncomingDamageServerReceiver
    {
        public PerPlayer_ExtraStatTracker tracker;
        public void OnEnable()
        {
            GetComponent<HealthComponent>().onIncomingDamageReceivers = this.GetComponents<IOnIncomingDamageServerReceiver>();
            GetComponent<HealthComponent>().onTakeDamageReceivers = this.GetComponents<IOnTakeDamageServerReceiver>();
        }

        public void OnIncomingDamageServer(DamageInfo damageInfo)
        {
            //Does either of these like account for OSP and OSP timer?
            /* WolfoMain.log.LogMessage("Damage: " + damageInfo.damage);
             WolfoMain.log.LogMessage("Blocked?: " + damageInfo.rejected);*/
            if (damageInfo.rejected)
            {
                tracker.damageBlocked += damageInfo.damage;
            }
        }
        public void OnTakeDamageServer(DamageReport damageReport)
        {
            /*WolfoMain.log.LogMessage("DamagePre: "+damageReport.damageInfo.damage);
            WolfoMain.log.LogMessage("DamagePost: " + damageReport.damageDealt);*/
            if (damageReport.damageInfo.damage != damageReport.damageDealt)
            {
                tracker.damageBlocked += (damageReport.damageInfo.damage - damageReport.damageDealt);
            }

        }
    }

    public class MinionMasterStatTracker : MonoBehaviour
    {
        public PerPlayer_ExtraStatTracker tracker;
        public CharacterMaster master;
        public GameObject bodyObject;
        public HealthComponent body;
        public UnityAction deathEvent;

        public void OnEnable()
        {
            master = this.GetComponent<CharacterMaster>();
            tracker = master.minionOwnership.ownerMaster.GetComponent<PerPlayer_ExtraStatTracker>();

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
            bodyObject = newBody.gameObject;
            body = newBody.healthComponent;
            var subTracker = newBody.gameObject.AddComponent<MinionBody_StatLocator>();
            subTracker.tracker = tracker;
            subTracker.bodyIndex = (int)newBody.bodyIndex;

        }
        public void OnBodyDeath()
        {
            //Suicide Checker
            //If wasn't hit in the last short while
            //But still died
            //Then probably was due to Suicide or Negative Regen
            if (body && body.timeSinceLastHit < 0.1f)
            {
                //Only count deaths that matter
                if (this.GetComponent<SetDontDestroyOnLoad>() && !this.GetComponent<MasterSuicideOnTimer>())
                {
                    WQoLMain.log.LogMessage(this.gameObject.name + " died");
                    //Chat.AddMessage(this.gameObject.name + " died");

                    tracker.minionDeaths++;
                }

            }

        }


    }

    public class MinionBody_StatLocator : MonoBehaviour
    {
        public int bodyIndex;
        public PerPlayer_ExtraStatTracker tracker;
    }


}
