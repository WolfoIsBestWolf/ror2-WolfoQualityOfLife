using HG;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Stats;
using UnityEngine;
using UnityEngine.Networking;
using WolfoLibrary;

namespace WolfoQoL_Client.DeathScreen
{

    public static partial class ExtraStatsTracking
    {

        public static void Start()
        {
            On.RoR2.NetworkUser.RpcDeductLunarCoins += LunarCoinSpentTracking;

            //Done in Start to track Stage-Specific interactables too
            On.RoR2.MinionOwnership.OnStartClient += Add_MinionDamageTracker;
            Run.onClientGameOverGlobal += SendSyncMessages;

            //GlobalEventManager.onClientDamageNotified += GlobalEventManager_onClientDamageNotified;
            IL.RoR2.HealthComponent.HandleDamageDealt += Track_DoTDamage_MinionHurt;
            IL.RoR2.HealthComponent.HandleHeal += Track_MinionHealing;


            //Host only stat adjustments
            On.RoR2.Inventory.GiveItemPermanent_ItemIndex_int += RemovingItemDeductFromItemsCollectedStats;
            On.EntityStates.BrotherMonster.SpellChannelEnterState.OnEnter += StoreItemAcqOrderOnMithrixP4;
            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += ReturnItemAcqOrderAfterMithrixP4;

            Run.onClientGameOverGlobal += Run_onClientGameOverGlobal;
        }

        private static void Run_onClientGameOverGlobal(Run arg1, RunReport arg2)
        {
            foreach (var player in PlayerCharacterMasterController.instances)
            {
                var playerTracker = player.GetComponent<PlayerMaster_ExtraStatTracker>();
                if (playerTracker)
                {
                    playerTracker.EvaluateStrongestMinion();
                }
            }
            MinionMaster_ExtraStatsTracker.minionBodyToTracker.Clear();


        }

        private static void GlobalEventManager_onClientDamageNotified(DamageDealtMessage obj)
        {
            Debug.Log(obj.victim);
            if (obj.attacker)
            {
                if (MinionMaster_ExtraStatsTracker.minionBodyToTracker.TryGetValue(obj.attacker, out var attackerM))
                {
                    if (attackerM.bodyIndex != -1)
                    {
                        attackerM.tracker.perMinionDamage[attackerM.bodyIndex] += (ulong)obj.damage;
                    }
                }
            }
           
        }

        private static void ReturnItemAcqOrderAfterMithrixP4(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                return;
            }
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
            {
                player.gameObject.GetComponent<ItemAquisitionStorer>()?.Return();
            }
        }

        private static void StoreItemAcqOrderOnMithrixP4(On.EntityStates.BrotherMonster.SpellChannelEnterState.orig_OnEnter orig, EntityStates.BrotherMonster.SpellChannelEnterState self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                return;
            }
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
            {
                player.gameObject.EnsureComponent<ItemAquisitionStorer>().Store();
            }
        }

        private static void RemovingItemDeductFromItemsCollectedStats(On.RoR2.Inventory.orig_GiveItemPermanent_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int countToAdd)
        {
            if (countToAdd < 0)
            {
                int itemCountBefore = self.GetItemCountPermanent(itemIndex);
                orig(self, itemIndex, countToAdd);
                int itemCountAfter = self.GetItemCountPermanent(itemIndex);
                StatManager.itemCollectedEvents.Enqueue(new StatManager.ItemCollectedEvent
                {
                    inventory = self,
                    itemIndex = itemIndex,
                    quantity = (itemCountAfter - itemCountBefore),
                    newCount = 0
                });
                return;
            }
            orig(self, itemIndex, countToAdd);
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
                    //This is past nullCheck of victim
                    if (damageEvent.attacker)
                    {
                        if (MinionMaster_ExtraStatsTracker.minionBodyToTracker.TryGetValue(damageEvent.attacker, out var attackerM))
                        {
                            if (attackerM.bodyIndex != -1)
                            {
                                attackerM.tracker.perMinionDamage[attackerM.bodyIndex] += (ulong)damageEvent.damage;
                            }
                        }   
                        if ((damageEvent.damageType & DamageType.DoT) != 0UL && self && PlayerMaster_ExtraStatTracker.playerBodyToTracker.TryGetValue(damageEvent.attacker, out var attackerP))
                        {
                            if (self.health != 1 || (damageEvent.damageType & DamageType.NonLethal) == 0UL)
                            {
                                attackerP.dotDamageDone += (ulong)damageEvent.damage;
                            }
                        }
                    }
                    if (MinionMaster_ExtraStatsTracker.minionBodyToTracker.TryGetValue(damageEvent.victim, out var victim))
                    {
                        victim.tracker.minionDamageTaken += (ulong)damageEvent.damage;
                    }
                    return self;
                });
            }
            else
            {
                Log.LogWarning("IL Failed: Track_DoTDamage_MinionHurt");
            }
        }

        /*
        private static void Host_TrackVoidedItems(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
                 x => x.MatchCallvirt("RoR2.Inventory", "RemoveItem")
                 ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<int, Inventory, int>>((count, inventory) =>
                {
                    if (inventory.TryGetComponent<PerPlayerMaster_ExtraStatTracker>(out var a))
                    {
                        //a.itemsVoided += count;
                    }
                    return count;
                });
            }
            else
            {
                Log.LogWarning("IL Failed: Host_TrackVoidedItems");
            }
        }
       */


        private static void Track_MinionHealing(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchStloc(0)
                ))
            {
                c.EmitDelegate<System.Func<HealthComponent.HealMessage, HealthComponent.HealMessage>>((healEvent) =>
                {
                    if (MinionMaster_ExtraStatsTracker.minionBodyToTracker.TryGetValue(healEvent.target, out var a))
                    {
                        a.tracker.minionHealing += (ulong)healEvent.amount;
                    }
                    return healEvent;
                });
            }
            else
            {
                Log.LogWarning("IL Failed: HealthComponent_HandleHeal");
            }
        }

        public static void SendSyncMessages(Run arg1, RunReport arg2)
        {
            foreach (var player in PlayerCharacterMasterController.instances)
            {
                Networker.SendWQoLMessage(new PlayerMaster_ExtraStatTracker.SyncValues
                {
                    masterObject = player.gameObject,
                });
            }

        }
        private static void Add_MinionDamageTracker(On.RoR2.MinionOwnership.orig_OnStartClient orig, MinionOwnership self)
        {
            orig(self);
            if (self.ownerMaster && self.ownerMaster.playerCharacterMasterController != null)
            {
                if (self.GetComponent<MinionMaster_ExtraStatsTracker>() == null)
                {
                    self.gameObject.AddComponent<MinionMaster_ExtraStatsTracker>();
                }
            }
        }

        private static void LunarCoinSpentTracking(On.RoR2.NetworkUser.orig_RpcDeductLunarCoins orig, NetworkUser self, uint count)
        {
            //WolfoMain.log.LogMessage("RpcDeductLunarCoins "+ self+self.master + " | "+count);
            orig(self, count);
            if (self.master)
            {
                self.master.GetComponent<PlayerMaster_ExtraStatTracker>().spentLunarCoins += (int)count;
            }
        }

    }

}
