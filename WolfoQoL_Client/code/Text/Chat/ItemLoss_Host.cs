using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.Text
{
    public class ItemLoss_Host
    {

        public static void Start()
        {
            IL.RoR2.PurchaseInteraction.OnInteractionBegin += ItemGiveUpMessages;
            On.RoR2.ScrapperController.BeginScrapping += ScrappingMessage;

            //On.EntityStates.QuestVolatileBattery.CountDown.OnExit += CountDown_OnExit;
        }

        private static void CountDown_OnExit(On.EntityStates.QuestVolatileBattery.CountDown.orig_OnExit orig, EntityStates.QuestVolatileBattery.CountDown self)
        {
            orig(self);
            if (self.attachedHealthComponent.body)
            {
                if (self.attachedHealthComponent.body.master)
                {
                    if (self.attachedHealthComponent.body.master.playerCharacterMasterController)
                    {
                        Chat.SendBroadcastChat(new ItemLossMessage
                        {
                            baseToken = "ITEM_LOSS_LOST",
                            pickupIndexOnlyOneItem = new PickupIndex(RoR2Content.Equipment.QuestVolatileBattery.equipmentIndex),
                            subjectAsCharacterBody = self.attachedHealthComponent.body,
                        });
                    }
                }
            }

        }

        //Server only
        private static void ScrappingMessage(On.RoR2.ScrapperController.orig_BeginScrapping orig, ScrapperController self, int intPickupIndex)
        {
            orig(self, intPickupIndex);
            if (self.interactor)
            {
                CharacterBody component = self.interactor.GetComponent<CharacterBody>();
                PickupDef pickupDef = PickupCatalog.GetPickupDef(new PickupIndex(intPickupIndex));
                if (component && component.inventory && pickupDef != null)
                {
                    Chat.SendBroadcastChat(new ItemLossMessage
                    {
                        baseToken = "ITEM_LOSS_SCRAP",
                        itemCount = self.itemsEaten,
                        pickupIndexOnlyOneItem = new PickupIndex(intPickupIndex),
                        subjectAsCharacterBody = component,
                        //subjectAsNetworkUser = component.master.playerCharacterMasterController.networkUser,
                    });

                }
            }
        }


        //Server only
        private static void ItemGiveUpMessages(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
                    x => x.MatchLdfld("RoR2.CostTypeDef/PayCostResults", "itemsTaken")))
            {
                //WolfoMain.log.LogMessage(c);
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<RoR2.CostTypeDef.PayCostResults, PurchaseInteraction, RoR2.Interactor, RoR2.CostTypeDef.PayCostResults>>((payResults, purchase, interactor) =>
                {
                    if (payResults.itemsTaken.Count > 0 || payResults.equipmentTaken.Count > 0)
                    {
                        string token = "";
                        bool onlyScrap = false;
                        //string user = interactor.GetComponent<CharacterBody>().GetUserName();


                        var LossMessage = new ItemLossMessage
                        {
                            subjectAsCharacterBody = interactor.gameObject.GetComponent<CharacterBody>(),
                        };

                        int[] itemStacks = null;
                        if (payResults.equipmentTaken.Count > 0)
                        {
                            LossMessage.pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex(payResults.equipmentTaken[0]);
                        }
                        else if (payResults.itemsTaken.Count == 1)
                        {
                            ItemDef tempDef = ItemCatalog.GetItemDef(payResults.itemsTaken[0]);
                            onlyScrap = tempDef.ContainsTag(ItemTag.Scrap);
                            LossMessage.pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex(payResults.itemsTaken[0]);
                        }
                        else if (payResults.itemsTaken.Count > 1)
                        {
                            itemStacks = ItemCatalog.RequestItemStackArray();
                            for (int i = 0; i < payResults.itemsTaken.Count; i++)
                            {
                                itemStacks[(int)payResults.itemsTaken[i]]++;
                            }
                            LossMessage.itemStacks = itemStacks;
                        }

                        if (purchase.gameObject.name.StartsWith("LunarCauldron"))
                        {
                            token = "ITEM_LOSS_CAULDRON";
                        }
                        else if (purchase.gameObject.name.StartsWith("ShrineCleanse"))
                        {
                            token = "ITEM_LOSS_CLEANSING";
                        }
                        else if (onlyScrap)
                        {
                            token = "ITEM_LOSS_USED";
                        }
                        else
                        {
                            token = "ITEM_LOSS_GENERIC";
                        }
                        LossMessage.baseToken = token;
                        Chat.SendBroadcastChat(LossMessage);
                    }
                    return payResults;
                });
                //WolfoMain.log.LogMessage("IL Found: IL.PurchaseInteraction.OnInteractionBegin");
            }
            else
            {
                WolfoMain.log.LogWarning("IL Failed: IL.PurchaseInteraction.OnInteractionBegin");
            }
        }



        public class ItemLossMessage : SubjectChatMessage
        {
            public override string ConstructChatString()
            {
                if (baseToken == "ITEM_LOSS_SCRAP")
                {
                    if (subjectAsNetworkUser && subjectAsNetworkUser.master)
                    {
                        var Tracker = subjectAsNetworkUser.master.GetComponent<PerPlayer_ExtraStatTracker>();
                        Tracker.scrappedItems += itemCount;
                        /*if (Tracker.scrappedItemsTotal == null)
                        {
                            Tracker.scrappedItemsTotal = new System.Collections.Generic.Dictionary<PickupIndex, int>();
                        }
                        if (Tracker.scrappedItemsTotal.TryGetValue(pickupIndexOnlyOneItem, out int i))
                        {
                            Tracker.scrappedItemsTotal[pickupIndexOnlyOneItem] = i + itemCount;
                        }
                        else
                        {
                            Tracker.scrappedItemsTotal.Add(pickupIndexOnlyOneItem, itemCount);
                        }*/

                    }
                    if (WConfig.cfgMessageScrap.Value == false)
                    {
                        return null;
                    }
                }
                else if (WConfig.cfgMessagePrint.Value == false)
                {
                    return null;
                }

                if (base.IsSecondPerson())
                {
                    baseToken += "_2P";
                }
                string itemsLost = "";
                string hex = "";
                string nameToken = "";

                if (pickupIndexOnlyOneItem != PickupIndex.none)
                {
                    itemsLost += Help.GetColoredName(pickupIndexOnlyOneItem);
                    if (this.itemCount > 1)
                    {
                        itemsLost += "(" + this.itemCount + ")";
                    }
                }
                else
                {
                    bool addedItem = false;
                    for (int i = 0; i < itemStacks.Length; i++)
                    {
                        if (itemStacks[i] > 0)
                        {

                            if (addedItem == true)
                            {
                                itemsLost += ", ";
                            }
                            addedItem = true;
                            if (itemStacks[i] > 1)
                            {
                                itemsLost += itemStacks[i].ToString() + "x ";
                            }
                            itemsLost += Help.GetColoredName((ItemIndex)i);
                        }
                    }
                }
                string result = string.Format(Language.GetString(baseToken), this.GetSubjectName(), itemsLost);
                return result;
            }


            public int itemCount;
            public PickupIndex pickupIndexOnlyOneItem = PickupIndex.none;
            public int[] itemStacks = ItemCatalog.RequestItemStackArray();

            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(itemCount);
                writer.Write(pickupIndexOnlyOneItem);
                if (pickupIndexOnlyOneItem == PickupIndex.none)
                {
                    writer.WriteItemStacks(itemStacks);
                }

            }

            public override void Deserialize(NetworkReader reader)
            {
                if (WolfoMain.NoHostInfo == true)
                {
                    return;
                }
                base.Deserialize(reader);
                itemCount = reader.ReadInt32();
                pickupIndexOnlyOneItem = reader.ReadPickupIndex();
                if (pickupIndexOnlyOneItem == PickupIndex.none)
                {
                    reader.ReadItemStacks(itemStacks);
                }
                //WolfoMain.log.LogWarning("Deserialize " + reader);
            }
        }


    }





}

