using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Stats;
using System.Collections.Generic;
using System.Data;
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
        public bool expectingLoop = false;
        public List<SceneDef> visitedScenes;
        public List<List<SceneDef>> visitedScenesTOTAL;

        public void OnEnable()
        {
            instance = this;
            dic_missedChests = new Dictionary<string, int>();
            dic_missedDrones = new Dictionary<string, int>();
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
                WolfoMain.log.LogMessage(visitedScenes);
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
                    visitedScenes = new List<SceneDef>();
                    visitedScenesTOTAL.Add(visitedScenes);
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
                        visitedScenes = new List<SceneDef>();
                        visitedScenesTOTAL.Add(visitedScenes);
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
                if (self.GetComponent<SummonMasterBehavior>()||
                    self.GetComponent<LemurianEggController>())
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

        public Dictionary<ItemIndex, int> scrappedItemTotal;

        public int timesJumped; //Client-Only
        public float minionDamageTaken;
        public float minionHealing;
        public int minionDeaths; //Important for like, how many drones did bro rebuy i guess?
        //public int minionDeathsSuicide;

        public int spentLunarCoins; //Works
        public int itemsVoided;
         
        public int scrappedItems; //Works
        public int scrappedDrones; //DLC3?


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
            strongestMinionDamage =val;
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
                    WolfoMain.log.LogWarning("No Master");
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
                if (WolfoMain.NoHostInfo == true)
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
                    WolfoMain.log.LogMessage(this.gameObject.name + " died");
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

    public class ExtraStatsTracking
    {

        public static void Start()
        {
            On.RoR2.NetworkUser.RpcDeductLunarCoins += LunarCoinSpentTracking;

            On.RoR2.PurchaseInteraction.Start += AddMissedCallBack;
            On.RoR2.MultiShopController.OnDestroy += AddMissedShops;

 
            On.RoR2.MinionOwnership.OnStartClient += Add_MinionDamageTracker;
            Run.onClientGameOverGlobal += SendSyncMessages;
 
            IL.RoR2.HealthComponent.HandleDamageDealt += Track_DoTDamage_MinionHurt;
            IL.RoR2.HealthComponent.HandleHeal += Track_MinionHealing;

            IL.RoR2.Items.ContagiousItemManager.StepInventoryInfection += Host_TrackVoidedItems;
 
        }

     
        private static void Track_DoTDamage_MinionHurt(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchStloc(1)
                ))
            {
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate<System.Func<HealthComponent, DamageDealtMessage, HealthComponent>>((self, damageEvent) =>
                {
                    
                    if (damageEvent.attacker)
                    {
                        if (damageEvent.attacker.TryGetComponent<MinionBody_StatLocator>(out MinionBody_StatLocator attacker))
                        {
                            attacker.tracker.perMinionDamage[attacker.bodyIndex] += damageEvent.damage;
                        }
                        if ((damageEvent.damageType & DamageType.DoT) != 0UL && self && damageEvent.attacker.TryGetComponent<PlayerDamageBlockedTracker>(out var tracker))
                        {
                            if (self.health != 1 || (damageEvent.damageType & DamageType.NonLethal) == 0UL)
                            {
                                tracker.tracker.dotDamageDone += damageEvent.damage;
                            }
                        }
                    }
                    if (damageEvent.victim.TryGetComponent<MinionBody_StatLocator>(out var a))
                    {
                        a.tracker.minionDamageTaken += damageEvent.damage;
                    }
                    return self;
                });
            }
            else
            {
                WolfoMain.log.LogWarning("IL Failed: HealthComponent_HandleHeal");
            }
        }

        private static void Host_TrackVoidedItems(ILContext il)
        {
            ILCursor c = new ILCursor(il);
 
           if (c.TryGotoNext(MoveType.Before,
                x => x.MatchCallvirt("RoR2.Inventory","RemoveItem")
                ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<int, Inventory, int>>((count, inventory) =>
                {
                    if (inventory.TryGetComponent<PerPlayer_ExtraStatTracker>(out var a))
                    {
                        a.itemsVoided += count;
                    }
                    return count;
                });
            }
            else
            {
                WolfoMain.log.LogWarning("IL Failed: HealthComponent_HandleHeal");
            }
        }

 

        private static void Track_MinionHealing(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchStloc(0)
                ))
            {
                c.EmitDelegate<System.Func<HealthComponent.HealMessage, HealthComponent.HealMessage>>((healEvent) =>
                {
                    if (healEvent.target && healEvent.target.TryGetComponent<MinionBody_StatLocator>(out var a))
                    {
                        a.tracker.minionHealing += healEvent.amount;
                    }
                    return healEvent;
                });
            }
            else
            {
                WolfoMain.log.LogWarning("IL Failed: HealthComponent_HandleHeal");
            }
        }

        public static void SendSyncMessages(Run arg1, RunReport arg2)
        {
            foreach (var player in PlayerCharacterMasterController.instances)
            {
                var tracker = player.GetComponent<PerPlayer_ExtraStatTracker>();
                Chat.SendBroadcastChat(new PerPlayer_ExtraStatTracker.SyncValues
                {
                    masterObject = player.gameObject,
                    timesJumped = tracker.timesJumped,
                    itemsVoided = tracker.itemsVoided,
                    damageBlocked = tracker.damageBlocked,

                });
            }

        }
        private static void Add_MinionDamageTracker(On.RoR2.MinionOwnership.orig_OnStartClient orig, MinionOwnership self)
        {
            orig(self);
            if (self.ownerMaster && self.ownerMaster.playerCharacterMasterController != null)
            {
                if (self.GetComponent<MinionMasterStatTracker>() == null)
                {
                    self.gameObject.AddComponent<MinionMasterStatTracker>();
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
                WolfoMain.log.LogWarning("IL Failed: MinionDamageTakenStat");
            }
        }

        private static void Stage_OnDisable(On.RoR2.Stage.orig_OnDisable orig, Stage self)
        {
            orig(self);
            GenericPickupController[] pickups = Object.FindObjectsOfType<GenericPickupController>();
            WolfoMain.log.LogMessage(pickups.Length);
            foreach (var pickup in pickups)
            {
                WolfoMain.log.LogMessage(pickup.pickupIndex);
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
                    string display = self.terminalPrefab.GetComponent<PurchaseInteraction>().GetDisplayName();
                    if (RunExtraStatTracker.instance.dic_missedChests.ContainsKey(display))
                    {
                        RunExtraStatTracker.instance.dic_missedChests[display]++;
                    }
                    else
                    {
                        RunExtraStatTracker.instance.dic_missedChests.Add(display, 1);
                    }
                }
            }
        }

        private static void LunarCoinSpentTracking(On.RoR2.NetworkUser.orig_RpcDeductLunarCoins orig, NetworkUser self, uint count)
        {
            //WolfoMain.log.LogMessage("RpcDeductLunarCoins "+ self+self.master + " | "+count);
            orig(self, count);
            if (self.master)
            {
                self.master.GetComponent<PerPlayer_ExtraStatTracker>().spentLunarCoins += (int)count;
            }
        }

    }



}
