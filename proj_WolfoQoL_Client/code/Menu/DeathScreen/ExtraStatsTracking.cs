using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Stats;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

namespace WolfoQoL_Client.DeathScreen
{

    public class ExtraStatsTracking
    {

        public static void Start()
        {
            On.RoR2.NetworkUser.RpcDeductLunarCoins += LunarCoinSpentTracking;

            //Done in Start to track Stage-Specific interactables too
            On.RoR2.PurchaseInteraction.Start += MissedCallback_Most;
            On.RoR2.MultiShopController.Start += MissedCallback_Shop;
            On.RoR2.DroneVendorMultiShopController.Start += MissedCallback_DroneShop;
            On.RoR2.CharacterAI.LemurianEggController.OnDestroy += LemurianEggController_OnDestroy;

            On.RoR2.MinionOwnership.OnStartClient += Add_MinionDamageTracker;
            Run.onClientGameOverGlobal += SendSyncMessages;

            IL.RoR2.HealthComponent.HandleDamageDealt += Track_DoTDamage_MinionHurt;
            IL.RoR2.HealthComponent.HandleHeal += Track_MinionHealing;

           // IL.RoR2.Items.ContagiousItemManager.StepInventoryInfection += Host_TrackVoidedItems;

            On.RoR2.Inventory.RemoveItemPermanent_ItemIndex_int += Inventory_RemoveItemPermanent_ItemIndex_int;
        }

        private static void Inventory_RemoveItemPermanent_ItemIndex_int(On.RoR2.Inventory.orig_RemoveItemPermanent_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            orig(self, itemIndex, count);   
            StatManager.itemCollectedEvents.Enqueue(new StatManager.ItemCollectedEvent
            {
                inventory = self,
                itemIndex = itemIndex,
                quantity = -count,
                newCount = 0
            });
        }

        private static void LemurianEggController_OnDestroy(On.RoR2.CharacterAI.LemurianEggController.orig_OnDestroy orig, LemurianEggController self)
        {
            orig(self);
            if (self.GetComponent<PickupPickerController>().available)
            {
                if (RunExtraStatTracker.instance)
                {
                    RunExtraStatTracker.instance.missedLemurians++;
                }

            }
        }

        private static void MissedCallback_DroneShop(On.RoR2.DroneVendorMultiShopController.orig_Start orig, DroneVendorMultiShopController self)
        {
            orig(self);
            if (self.gameObject.activeInHierarchy)
            {
                OnDestroyCallback.AddCallback(self.gameObject, new System.Action<OnDestroyCallback>(RunExtraStatTracker.OnMultiShopDestroyed));
            }
        }

        private static void MissedCallback_Shop(On.RoR2.MultiShopController.orig_Start orig, MultiShopController self)
        {
            orig(self);
            if (self.gameObject.activeInHierarchy)
            {
                OnDestroyCallback.AddCallback(self.gameObject, new System.Action<OnDestroyCallback>(RunExtraStatTracker.OnMultiShopDestroyed));
            }
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
                WQoLMain.log.LogWarning("IL Failed: Track_DoTDamage_MinionHurt");
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
                        a.itemsVoided += count;
                    }
                    return count;
                });
            }
            else
            {
                WQoLMain.log.LogWarning("IL Failed: Host_TrackVoidedItems");
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
                WQoLMain.log.LogWarning("IL Failed: HealthComponent_HandleHeal");
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
                WQoLMain.log.LogWarning("IL Failed: MinionDamageTakenStat");
            }
        }

        private static void Stage_OnDisable(On.RoR2.Stage.orig_OnDisable orig, Stage self)
        {
            orig(self);
            GenericPickupController[] pickups = Object.FindObjectsOfType<GenericPickupController>();
            WQoLMain.log.LogMessage(pickups.Length);
            foreach (var pickup in pickups)
            {
                WQoLMain.log.LogMessage(pickup.pickupIndex);
            }
        }

        private static void MissedCallback_Most(On.RoR2.PurchaseInteraction.orig_Start orig, PurchaseInteraction self)
        {
            orig(self);
            if (self.gameObject.activeInHierarchy)
            {
                OnDestroyCallback.AddCallback(self.gameObject, new System.Action<OnDestroyCallback>(RunExtraStatTracker.OnPurchaseDestroyed));
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
