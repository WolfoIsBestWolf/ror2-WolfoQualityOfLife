using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine.Networking;
using WolfoLibrary;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.Text
{
    public static class ItemLoss_Host
    {

        public static void Start()
        {
            //Needed for tracking other junk
            IL.RoR2.ScrapperController.BeginScrapping_UniquePickup += ScrappingMessage;
            On.RoR2.DroneScrapperController.TryDestroyDrone += DroneScrapperController_TryDestroyDrone;

            if (!WConfig.module_text_chat.Value)
            {
                return;
            }
            IL.RoR2.PurchaseInteraction.OnInteractionBegin += ItemGiveUpMessages;

            //On.RoR2.ScrapperController.BeginScrapping += Old_ScrappingMessage;

            //On.EntityStates.QuestVolatileBattery.CountDown.OnExit += CountDown_OnExit;
        }

        private static bool DroneScrapperController_TryDestroyDrone(On.RoR2.DroneScrapperController.orig_TryDestroyDrone orig, DroneScrapperController self, CharacterBody body)
        {
            bool scrapped = orig(self, body);
            if (scrapped && self.interactor)
            {
                CharacterBody component = self.interactor.GetComponent<CharacterBody>();
                if (component && component.inventory)
                {
                    Networker.SendWQoLMessage(new ItemLossMessage
                    {
                        source = ItemLossMessage.Source.DroneScrapper,
                        baseToken = "ITEM_LOSS_SCRAP",
                        itemCount = body.inventory.GetItemCountEffective(DLC3Content.Items.DroneUpgradeHidden),
                        pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex(DroneCatalog.GetDroneIndexFromBodyIndex(body.bodyIndex)),
                        subjectAsCharacterBody = component,
                    });

                }
            }
            return scrapped;
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
                        Networker.SendWQoLMessage(new ItemLossMessage
                        {
                            baseToken = "ITEM_LOSS_LOST",
                            pickupIndexOnlyOneItem = new PickupIndex(RoR2Content.Equipment.QuestVolatileBattery.equipmentIndex),
                            subjectAsCharacterBody = self.attachedHealthComponent.body,
                        });
                    }
                }
            }

        }

        //ServerOnly
        private static void ScrappingMessage(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("RoR2.Inventory/ItemTransformation/TryTransformResult", "takenItem")))
            {
                //WolfoMain.log.LogMessage(c);
                c.Emit(OpCodes.Ldarg_0);
                //c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<Inventory.ItemTransformation.TryTransformResult, ScrapperController, Inventory.ItemTransformation.TryTransformResult>>((tryTransformResult, self) =>
                {
                    if (self.interactor)
                    {
                        CharacterBody component = self.interactor.GetComponent<CharacterBody>();
                        if (component && component.inventory)
                        {
                            Networker.SendWQoLMessage(new ItemLossMessage
                            {
                                source = ItemLossMessage.Source.Scrapper,
                                baseToken = "ITEM_LOSS_SCRAP",
                                itemCount = tryTransformResult.totalTransformed,
                                pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex(tryTransformResult.takenItem.itemIndex),
                                subjectAsCharacterBody = component,
                                //subjectAsNetworkUser = component.master.playerCharacterMasterController.networkUser,
                            });

                        }
                    }

                    return tryTransformResult;
                });
            }
            else
            {
                Log.LogWarning("IL Failed: IL.ScrappingMessage");
            }
        }


        //Server only
        /*private static void Old_ScrappingMessage(On.RoR2.ScrapperController., ScrapperController self, int intPickupIndex)
        {
            orig(self, intPickupIndex);
            if (self.interactor)
            {
                CharacterBody component = self.interactor.GetComponent<CharacterBody>();
                PickupDef pickupDef = PickupCatalog.GetPickupDef(new PickupIndex(intPickupIndex));
                if (component && component.inventory && pickupDef != null)
                {
                    Networking.SendWQoLMessage(new ItemLossMessage
                    {
                        baseToken = "ITEM_LOSS_SCRAP",
                        //itemCount = self.itemsEaten,
                        pickupIndexOnlyOneItem = new PickupIndex(intPickupIndex),
                        subjectAsCharacterBody = component,
                        //subjectAsNetworkUser = component.master.playerCharacterMasterController.networkUser,
                    });

                }
            }
        }*/


        //Server only
        private static void ItemGiveUpMessages(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
                    x => x.MatchCallvirt("RoR2.CostTypeDef/PayCostResults", "get_itemStacksTaken")))
            {
                //WolfoMain.log.LogMessage(c);
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<CostTypeDef.PayCostResults, PurchaseInteraction, Interactor, CostTypeDef.PayCostResults>>((payResults, purchase, interactor) =>
                {

                    //These 3 should also say used I guess?
                    if (payResults.itemStacksTaken.Count > 0 || payResults.equipmentTaken.Count > 0)
                    {
                        string token = "";
                        bool usedMessage = false;
                        //string user = interactor.GetComponent<CharacterBody>().GetUserName();
                        if (purchase.costType == CostTypeIndex.TreasureCacheItem ||
                       purchase.costType == CostTypeIndex.TreasureCacheVoidItem ||
                       purchase.costType == CostTypeIndex.ArtifactShellKillerItem)
                        {
                            usedMessage = true;
                        }

                        var LossMessage = new ItemLossMessage
                        {
                            subjectAsCharacterBody = interactor.gameObject.GetComponent<CharacterBody>(),
                        };

                        if (payResults.equipmentTaken.Count > 0)
                        {
                            LossMessage.pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex(payResults.equipmentTaken[0]);
                        }
                        else if (payResults.itemStacksTaken.Count == 1)
                        {
                            ItemDef tempDef = ItemCatalog.GetItemDef(payResults.itemStacksTaken[0].itemIndex);
                            if (payResults.itemStacksTaken[0].stackValues.permanentStacks > 0)
                            {

                            }
                            else if (payResults.itemStacksTaken[0].stackValues.temporaryStacksValue > 0)
                            {
                                LossMessage.hasTempItems = true;
                            }
                            usedMessage = usedMessage || tempDef.ContainsTag(ItemTag.Scrap) || tempDef.ContainsTag(ItemTag.PriorityScrap);
                            LossMessage.pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex(tempDef.itemIndex);
                        }
                        else if (payResults.itemStacksTaken.Count > 1)
                        {
                            //No purchase interaction that asks for "x of Tier" allows temp items so probably fine if we just skipped it tbh.

                            LossMessage.itemStacks = ItemCatalog.RequestItemStackArray();
                            //LossMessage.tempItemStacks = ItemCatalog.RequestItemStackArray();
                            foreach (var taken in payResults.itemStacksTaken)
                            {
                                if (taken.stackValues.permanentStacks > 0)
                                {
                                    LossMessage.itemStacks[(int)taken.itemIndex] = taken.stackValues.permanentStacks;
                                }
                                /*else if (taken.stackValues.temporaryStacksValue > 0)
                                {
                                    LossMessage.hasTempItems = true;
                                    LossMessage.tempItemStacks[(int)taken.itemIndex] = taken.stackValues.permanentStacks;
                                }*/
                            }
                        }

                        if (purchase.gameObject.name.StartsWith("LunarCauldron"))
                        {
                            LossMessage.source = ItemLossMessage.Source.LunarCauldron;
                            token = "ITEM_LOSS_CAULDRON";
                        }
                        else if (purchase.gameObject.name.StartsWith("ShrineCleanse"))
                        {
                            LossMessage.source = ItemLossMessage.Source.CleansingPool;
                            token = "ITEM_LOSS_CLEANSING";
                        }
                        else if (usedMessage)
                        {
                            token = "ITEM_LOSS_USED";
                        }
                        else
                        {
                            token = "ITEM_LOSS_GENERIC";
                        }
                        LossMessage.baseToken = token;
                        Networker.SendWQoLMessage(LossMessage);
                    }
                    return payResults;
                });
            }
            else
            {
                Log.LogWarning("IL Failed: IL.PurchaseInteraction.OnInteractionBegin");
            }
        }




    }


    public class ItemLossMessage : SubjectChatMessage
    {
        public enum Source
        {
            Default,
            Scrapper,
            DroneScrapper,
            LemurianEgg,
            LunarCauldron,
            CleansingPool,
            MealStation,
        }
        public Source source;
        public override string ConstructChatString()
        {
            var Tracker = subjectAsNetworkUser.master.GetComponent<PerPlayer_ExtraStatTracker>();

            if (source == Source.Scrapper)
            {
                Tracker.scrappedItems += itemCount;
                if (!WConfig.cfgMessageScrap.Value)
                {
                    return null;
                }
            }
            else if (source == Source.DroneScrapper)
            {
                Tracker.scrappedDrones += DroneUpgradeUtils.GetDroneCountFromUpgradeCount(itemCount);
                if (!WConfig.cfgMessageScrap.Value)
                {
                    return null;
                }
            }
            else if (source == Source.LemurianEgg)
            {
                Tracker.lemuriansHatched++;
                if (!WConfig.cfgMessageDevotion.Value)
                {
                    return null;
                }
            }
            else
            {
                if (!WConfig.cfgMessagePrint.Value)
                {
                    return null;
                }
            }
            if (!WConfig.module_text_chat.Value)
            {
                return null;
            }

            if (base.IsSecondPerson())
            {
                baseToken += "_2P";
            }
            string itemsLost = "";

            if (pickupIndex2Cook != PickupIndex.none)
            {
                itemsLost += Help.GetColoredName(pickupIndexOnlyOneItem);
                itemsLost += " + ";
                itemsLost += Help.GetColoredName(pickupIndex2Cook);
            }
            else if (pickupIndexOnlyOneItem != PickupIndex.none)
            {
                if (source == Source.DroneScrapper)
                {
                    itemsLost += Help.GetColoredName(pickupIndexOnlyOneItem, itemCount);
                }
                else
                {
                    itemsLost += Help.GetColoredName(pickupIndexOnlyOneItem, 0, hasTempItems);
                    if (itemCount > 1)
                    {
                        itemsLost += "(" + this.itemCount + ")";
                    }
                }
            }
            else
            {
                bool addedItem = false;
                if (hasTempItems)
                {
                    for (int i = 0; i < tempItemStacks.Length; i++)
                    {
                        if (tempItemStacks[i] > 0)
                        {
                            bool scrap = ItemCatalog.GetItemDef((ItemIndex)i).ContainsTag(ItemTag.Scrap);
                            if (scrap)
                            {
                                if (addedItem == true)
                                {
                                    itemsLost = ", " + itemsLost;
                                }
                                addedItem = true;
                                if (tempItemStacks[i] > 1)
                                {
                                    itemsLost = tempItemStacks[i].ToString() + "x " + itemsLost;
                                }
                                itemsLost = Help.GetColoredName((ItemIndex)i, true) + itemsLost;
                            }
                            else
                            {
                                if (addedItem == true)
                                {
                                    itemsLost += ", ";
                                }
                                addedItem = true;
                                if (tempItemStacks[i] > 1)
                                {
                                    itemsLost += tempItemStacks[i].ToString() + "x ";
                                }
                                itemsLost += Help.GetColoredName((ItemIndex)i, true);
                            }

                        }
                    }
                }
                for (int i = 0; i < itemStacks.Length; i++)
                {
                    if (itemStacks[i] > 0)
                    {
                        bool scrap = ItemCatalog.GetItemDef((ItemIndex)i).ContainsTag(ItemTag.Scrap);
                        if (scrap)
                        {
                            if (addedItem == true)
                            {
                                itemsLost = ", " + itemsLost;
                            }
                            addedItem = true;
                            if (itemStacks[i] > 1)
                            {
                                itemsLost = itemStacks[i].ToString() + "x " + itemsLost;
                            }
                            itemsLost = Help.GetColoredName((ItemIndex)i) + itemsLost;
                        }
                        else
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
            }
            string result = string.Format(Language.GetString(baseToken), this.GetSubjectName(), itemsLost);
            return result;
        }


        public int itemCount;
        public bool hasTempItems;
        public PickupIndex pickupIndexOnlyOneItem = PickupIndex.none;
        public PickupIndex pickupIndex2Cook = PickupIndex.none;


        //Could redo this like how other things do dynamic list length
        public int[] itemStacks = ItemCatalog.RequestItemStackArray();
        public int[] tempItemStacks = ItemCatalog.RequestItemStackArray();

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(itemCount);
            writer.Write(pickupIndexOnlyOneItem);
            writer.Write(pickupIndex2Cook);
            writer.Write((int)source);
            writer.Write(hasTempItems);
            if (pickupIndexOnlyOneItem == PickupIndex.none)
            {
                writer.WriteItemStacks(itemStacks);
                if (hasTempItems)
                {
                    writer.WriteItemStacks(tempItemStacks);
                }
            }

        }

        public override void Deserialize(NetworkReader reader)
        {
            if (WQoLMain.NoHostInfo == true)
            {
                return;
            }
            base.Deserialize(reader);
            itemCount = reader.ReadInt32();
            pickupIndexOnlyOneItem = reader.ReadPickupIndex();
            pickupIndex2Cook = reader.ReadPickupIndex();
            source = (Source)reader.ReadInt32();
            hasTempItems = reader.ReadBoolean();
            if (pickupIndexOnlyOneItem == PickupIndex.none)
            {
                reader.ReadItemStacks(itemStacks);
                if (hasTempItems)
                {
                    reader.ReadItemStacks(tempItemStacks);
                }
            }
            //WolfoMain.log.LogWarning("Deserialize " + reader);
        }
    }



}

