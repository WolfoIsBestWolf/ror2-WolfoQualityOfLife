using MonoMod.Cil;
using RoR2;
using RoR2.Stats;
using RoR2.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Mono.Cecil.Cil;
 
namespace WolfoQoL_Client.DeathScreen
{
    public class RunExtraStatTracker : MonoBehaviour
    {
        public static RunExtraStatTracker instance;

        public GameObject latestWaveUiPrefab;
        public int missedChests;
        public int missedShrineChanceItems;
        public int missedItems;
        public int missedDrones;
        public bool expectingLoop = false;
        public List<SceneDef> visitedScenes;
        public List<List<SceneDef>> visitedScenesTOTAL;

        public void OnEnable()
        {
            instance = this;
            visitedScenes = new List<SceneDef>();
            visitedScenesTOTAL = new List<List<SceneDef>>();
            visitedScenesTOTAL.Add(visitedScenes);
            SceneCatalog.onMostRecentSceneDefChanged += this.HandleMostRecentSceneDefChanged;
        }
        public void OnDisable()
        {
            instance = null;
            SceneCatalog.onMostRecentSceneDefChanged -= this.HandleMostRecentSceneDefChanged;
            foreach (SceneDef sceneDef in visitedScenes)
            {
                Debug.Log(visitedScenes);
            }
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
            if (lastSeenLoopCount != Run.instance.loopClearCount)
            {
                expectingLoop = true;
                lastSeenLoopCount = Run.instance.loopClearCount;
            }
            if (expectingLoop)
            {
                if (newSceneDef.stageOrder < 6 || Run.instance is InfiniteTowerRun)
                {
                    expectingLoop = false;
                    visitedScenes = new List<SceneDef>();
                    visitedScenesTOTAL.Add(visitedScenes);
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
                if (purchase.costType == CostTypeIndex.LunarCoin)
                {
                    return;
                }
                if (self.GetComponent<ChestBehavior>()
                 || self.GetComponent<OptionChestBehavior>()
                 || self.GetComponent<RouletteChestController>())
                {
                    //Help.Log.LogWarning("Missed Chest: " + self.name);
                    instance.missedChests++;
                }
                if (self.TryGetComponent<ShrineChanceBehavior>(out var chance))
                {
                    //Help.Log.LogWarning("Missed Chance: " + self.name);
                    instance.missedShrineChanceItems += chance.maxPurchaseCount - chance.successfulPurchaseCount;
                }
                if (self.GetComponent<SummonMasterBehavior>())
                {
                    //Help.Log.LogWarning("Missed Drone: " + self.name);
                    instance.missedDrones++;
                }
            }
        }
    }

    public class PerPlayer_ExtraStatTracker : MonoBehaviour
    {
        //Unused
        /*public int totalBuffsGotten;
        public float timeSpentInChargeZone;
        public int totalMinionDeaths;
        public int DoTsInflcited;
        public float minionHealingTotal; //Can't do
        
        public int spentVoidCoins;
        */

        public int timesJumped = -1; //Client-Only
        public float minionDamageTaken; //Server-Only
        public int spentLunarCoins; //Works
        public int scrappedItems; //Works
        public int scrappedDrones; //Idk if this is even real

        public string latestDetailedDeathMessage;

        public CharacterMaster master;
        public void OnEnable()
        {
            master = this.GetComponent<CharacterMaster>();
            master.onBodyStart += Master_onBodyStart;
            if (master.hasEffectiveAuthority)
            {
                timesJumped = 0;
            }
        }
        public void OnDisable()
        {
            if (master)
            {
                master.onBodyStart -= Master_onBodyStart;
            }
          
        }


        private void Master_onBodyStart(CharacterBody body)
        {
            latestDetailedDeathMessage = string.Empty;
            if (master.hasEffectiveAuthority)
            {
                body.onJump += OnJump;
            }
       
        }

        public void OnJump()
        {
            timesJumped++;
        }

        public class SyncValues : ChatMessageBase
        {
            public GameObject masterObject;
            public int timesJumped;
            public override string ConstructChatString()
            {
                if (masterObject == null)
                {
                    Debug.LogWarning("No Master");
                    return null;
                }
                var tracker = masterObject.GetComponent<PerPlayer_ExtraStatTracker>();
                if (timesJumped > 0) 
                {
                    tracker.timesJumped = timesJumped;
                    Debug.Log("timesJumped" + timesJumped);
                }
          
                return null;
            }
            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(masterObject);
                writer.Write(timesJumped);
            }
            public override void Deserialize(NetworkReader reader)
            {
                if (WolfoMain.NoHostInfo == true)
                {
                    return;
                }
                base.Deserialize(reader);
                masterObject = reader.ReadGameObject();
                timesJumped = reader.ReadInt32();
            }
        }

    }

    public class MinionDamageTakenListener : MonoBehaviour
    {
        public PerPlayer_ExtraStatTracker tracker;
        public CharacterMaster master;
        public GameObject bodyObject;
        public void OnEnable()
        {
            master = this.GetComponent<CharacterMaster>();
            tracker = master.minionOwnership.ownerMaster.GetComponent<PerPlayer_ExtraStatTracker>();   

            master.onBodyStart += Master_onBodyStart;
            master.onBodyDestroyed += Master_onBodyDestroyed;
        }
        private void Master_onBodyStart(CharacterBody body)
        {
            bodyObject = body.gameObject;
            GlobalEventManager.onClientDamageNotified += onClientDamageNotified;
        }
        private void Master_onBodyDestroyed(CharacterBody obj)
        {
            GlobalEventManager.onClientDamageNotified -= onClientDamageNotified;
            bodyObject = null;
        }

        private void onClientDamageNotified(DamageDealtMessage message)
        {
            if (message.victim == bodyObject)
            {
                tracker.minionDamageTaken += message.damage;
            }
        }

       
    }

    public class ExtraStatsTracking
    {
        
        public static void Start()
        {
            On.RoR2.NetworkUser.RpcDeductLunarCoins += LunarCoinSpentTracking;
    
            On.RoR2.PurchaseInteraction.Start += AddMissedCallBack;
            On.RoR2.MultiShopController.OnDestroy += AddMissedShops;

            //On.RoR2.Stage.OnDisable += Stage_OnDisable;

            //IL.RoR2.Stats.StatManager.ProcessDamageEvents += MinionDamageTakenStat;

            On.RoR2.MinionOwnership.OnStartClient += MinionDamageTakenStat;
            Run.onClientGameOverGlobal += Run_onClientGameOverGlobal;


        }

        public static void Run_onClientGameOverGlobal(Run arg1, RunReport arg2)
        {
            foreach (var player in PlayerCharacterMasterController.instances)
            {
                var tracker = player.GetComponent<PerPlayer_ExtraStatTracker>();
                Chat.SendBroadcastChat(new PerPlayer_ExtraStatTracker.SyncValues
                {
                    masterObject = player.gameObject,
                    timesJumped = tracker.timesJumped,
                });
            }
            
        }
        private static void MinionDamageTakenStat(On.RoR2.MinionOwnership.orig_OnStartClient orig, MinionOwnership self)
        {
            orig(self);
            if (self.ownerMaster && self.ownerMaster.playerCharacterMasterController != null)
            {
                if (self.GetComponent<MinionDamageTakenListener>() == null)
                {
                    self.gameObject.AddComponent<MinionDamageTakenListener>();
                }
            }
        }

        private static void MinionDamageTakenStat(ILContext il)
        {
            ILCursor c = new ILCursor(il);
         
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("RoR2.Stats.StatManager/DamageEvent", "damageDealt")
                ))
            {
                c.EmitDelegate<System.Func<StatManager.DamageEvent, StatManager.DamageEvent>>((damageEvent) =>
                {
                    if (damageEvent.victimMaster)
                    {
                        if (damageEvent.victimMaster.minionOwnership && damageEvent.victimMaster.minionOwnership.ownerMaster)
                        {
                            var a = damageEvent.victimMaster.minionOwnership.ownerMaster.GetComponent<PerPlayer_ExtraStatTracker>();
                            if (a)
                            {
                                a.minionDamageTaken += damageEvent.damageDealt;
                            }                      
                        }
                    }
                    return damageEvent;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: MinionDamageTakenStat");
            }
        }

        private static void Stage_OnDisable(On.RoR2.Stage.orig_OnDisable orig, Stage self)
        {
            orig(self);
            GenericPickupController[] pickups = Object.FindObjectsOfType<GenericPickupController>();
            Debug.Log(pickups.Length);
            foreach (var pickup in pickups)
            {
                Debug.Log(pickup.pickupIndex);
            }
        }

        private static void AddMissedCallBack(On.RoR2.PurchaseInteraction.orig_Start orig, PurchaseInteraction self)
        {
            orig(self);
            if (self.gameObject.activeInHierarchy)
            {
                OnDestroyCallback.AddCallback(self.gameObject, new System.Action<OnDestroyCallback>(RunExtraStatTracker.OnPurchaseDestroyed));
            }     
        }
        private static void AddMissedShops(On.RoR2.MultiShopController.orig_OnDestroy orig, MultiShopController self)
        {
            orig(self);
            if (RunExtraStatTracker.instance)
            {
                if (self.available)
                {
                    //Help.Log.LogWarning("Missed MultiShop: " + self.gameObject);
                    RunExtraStatTracker.instance.missedChests++;
                }
            }
        }

        private static void LunarCoinSpentTracking(On.RoR2.NetworkUser.orig_RpcDeductLunarCoins orig, NetworkUser self, uint count)
        {
            //Debug.Log("RpcDeductLunarCoins "+ self+self.master + " | "+count);
            orig(self, count);
            if (self.master)
            {
                self.master.GetComponent<PerPlayer_ExtraStatTracker>().spentLunarCoins += (int)count;
            }
        }

    }



}
