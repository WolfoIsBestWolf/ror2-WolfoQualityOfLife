using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Stats;
using System.Collections.Generic;
using UnityEngine;
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

            IL.RoR2.HealthComponent.HandleDamageDealt += Track_DoTDamage_MinionHurt;
            IL.RoR2.HealthComponent.HandleHeal += Track_MinionHealing;

            // IL.RoR2.Items.ContagiousItemManager.StepInventoryInfection += Host_TrackVoidedItems;

            On.RoR2.Stats.StatManager.OnServerItemGiven += StatManager_OnServerItemGiven;
            On.RoR2.Inventory.GiveItemPermanent_ItemIndex_int += Inventory_GiveItemPermanent_ItemIndex_int;

        }

        public static Dictionary<GameObject, MinionMasterStatTracker> a;

        private static void Inventory_GiveItemPermanent_ItemIndex_int(On.RoR2.Inventory.orig_GiveItemPermanent_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int countToAdd)
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

        private static void StatManager_OnServerItemGiven(On.RoR2.Stats.StatManager.orig_OnServerItemGiven orig, Inventory inventory, ItemIndex itemIndex, int quantity)
        {
            orig(inventory, itemIndex, quantity);
        }

        private static void REMOVINGITEMDEDUCTSFROMSTAT(On.RoR2.Inventory.orig_RemoveItemPermanent_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            int itemCountBefore = self.GetItemCountPermanent(itemIndex);
            orig(self, itemIndex, count);
            if (ItemCatalog.GetItemDef(itemIndex).hidden)
            {
                return;
            }
            int itemCountAfter = self.GetItemCountPermanent(itemIndex);
            StatManager.itemCollectedEvents.Enqueue(new StatManager.ItemCollectedEvent
            {
                inventory = self,
                itemIndex = itemIndex,
                quantity = (itemCountAfter - itemCountBefore),
                newCount = 0
            });
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
                    //This is past checking for minion
                    if (damageEvent.attacker)
                    {
                        if (damageEvent.attacker.TryGetComponent(out MinionBody_StatLocator attacker))
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
                Log.LogWarning("IL Failed: Track_DoTDamage_MinionHurt");
            }
        }

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
                    if (inventory.TryGetComponent<PerPlayer_ExtraStatTracker>(out var a))
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
                Log.LogWarning("IL Failed: HealthComponent_HandleHeal");
            }
        }

        public static void SendSyncMessages(Run arg1, RunReport arg2)
        {
            foreach (var player in PlayerCharacterMasterController.instances)
            {

                Networker.SendWQoLMessage(new PerPlayer_ExtraStatTracker.SyncValues
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
                if (self.GetComponent<MinionMasterStatTracker>() == null)
                {
                    self.gameObject.AddComponent<MinionMasterStatTracker>();
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
