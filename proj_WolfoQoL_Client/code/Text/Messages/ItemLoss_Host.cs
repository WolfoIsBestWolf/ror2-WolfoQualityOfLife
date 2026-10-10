using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
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
                        pickupIndex1 = PickupCatalog.FindPickupIndex(DroneCatalog.GetDroneIndexFromBodyIndex(body.bodyIndex)),
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
                            pickupIndex1 = new PickupIndex(RoR2Content.Equipment.QuestVolatileBattery.equipmentIndex),
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
                                pickupIndex1 = PickupCatalog.FindPickupIndex(tryTransformResult.takenItem.itemIndex),
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
                    
                    if (InstanceTracker.GetInstancesList<TrialController>().Count > 0)
                    {
                        return payResults;
                    }
                    //These 3 should also say used I guess?
                    if (payResults.itemStacksTaken.Count > 0 || payResults.equipmentTaken.Count > 0)
                    {
                        string token = "";
                        bool usedMessage = false;
                        //string user = interactor.GetComponent<CharacterBody>().GetUserName();
                        if (purchase.costType == CostTypeIndex.TreasureCacheItem ||
                           purchase.costType == CostTypeIndex.TreasureCacheVoidItem)
                        {
                            usedMessage = true;
                        }

                        var LossMessage = new ItemLossMessage
                        {
                            subjectAsCharacterBody = interactor.gameObject.GetComponent<CharacterBody>(),
                        };

                        if (payResults.equipmentTaken.Count > 0)
                        {
                            LossMessage.pickupIndex1 = PickupCatalog.FindPickupIndex(payResults.equipmentTaken[0]);
                        }
                        else if (payResults.itemStacksTaken.Count == 1)
                        {
                            ItemDef tempDef = ItemCatalog.GetItemDef(payResults.itemStacksTaken[0].itemIndex);
                            if (payResults.itemStacksTaken[0].stackValues.permanentStacks > 0)
                            {

                            }
                            else if (payResults.itemStacksTaken[0].stackValues.temporaryStacksValue > 0)
                            {
                                LossMessage.isTempItem = true;
                            }
                            else if (payResults.itemStacksTaken[0].stackValues.rentalStacks > 0)
                            {
                                LossMessage.isRentalItem = true;
                            }
                            usedMessage = usedMessage || tempDef.ContainsTag(ItemTag.Scrap) || tempDef.ContainsTag(ItemTag.PriorityScrap);
                            LossMessage.pickupIndex1 = PickupCatalog.FindPickupIndex(tempDef.itemIndex);
                            LossMessage.itemCount = payResults.itemStacksTaken[0].stackValues.totalStacks;
                        }
                        else if (payResults.itemStacksTaken.Count > 1)
                        {
                            //No purchase interaction that asks for "x of Tier" allows temp items so probably fine if we just skipped it tbh.
                            LossMessage.itemStacks = ItemCatalog.RequestItemStackArray();
                            foreach (var taken in payResults.itemStacksTaken)
                            {
                                if (taken.stackValues.permanentStacks > 0)
                                {
                                    LossMessage.itemStacks[(int)taken.itemIndex] += taken.stackValues.permanentStacks;
                                }
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
            DreamCrucible
        }
        public Source source;
        public override string ConstructChatString()
        {
            bool has2nd = true;
            PerPlayerMaster_ExtraStatTracker Tracker = null;
            if (subjectAsNetworkUser)
            {
                Tracker = subjectAsNetworkUser.master.GetComponent<PerPlayerMaster_ExtraStatTracker>();
            }
            
            switch (source)
            {
                case Source.Scrapper:
                    Tracker.scrappedItems += itemCount;
                    if (!WConfig.cfgMessageScrap.Value)
                    {
                        return null;
                    }
                    break;
                case Source.DroneScrapper:
                    Tracker.scrappedDrones += DroneUpgradeUtils.GetDroneCountFromUpgradeCount(itemCount);
                    if (!WConfig.cfgMessageScrap.Value)
                    {
                        return null;
                    }
                    break;
                case Source.LemurianEgg:
                    Tracker.lemuriansHatched++;
                    if (!WConfig.cfgMessageDevotion.Value)
                    {
                        return null;
                    }
                    break;
                case Source.DreamCrucible:
                    if (WConfig.cfgMessageDreamCrucible.Value == WConfig.MessageWho.Off)
                    {
                        return null;
                    }
                    if (WConfig.cfgMessageDreamCrucible.Value == WConfig.MessageWho.Off)
                    {
                        return null;
                    }
                    has2nd = false;
                    break;
                default:
                    if (!WConfig.cfgMessagePrint.Value)
                    {
                        return null;
                    }
                    break;

            }

            if (!WConfig.module_text_chat.Value)
            {
                return null;
            }

            if (base.IsSecondPerson() && has2nd)
            {
                baseToken += "_2P";
            }
            string itemsLost = "";

            if (pickupIndex2 != PickupIndex.none)
            {
                itemsLost += Help.GetColoredName(pickupIndex1);
                itemsLost += " + ";
                itemsLost += Help.GetColoredName(pickupIndex2);
            }
            else if (pickupIndex1 != PickupIndex.none)
            {
                if (source == Source.DroneScrapper)
                {
                    itemsLost += Help.GetColoredName(pickupIndex1, itemCount);
                }
                else
                {
                    if (source == Source.LunarCauldron)
                    {
                        if (itemCount > 1)
                        {
                            itemsLost += this.itemCount + "x " + Help.GetColoredName(pickupIndex1, 0, isTempItem, isRentalItem); ;
                        }
                    }
                    else //if (source == Source.Scrapper)
                    {
                        itemsLost += Help.GetColoredName(pickupIndex1, 0, isTempItem, isRentalItem);
                        if (itemCount > 1)
                        {
                            itemsLost += "(" + this.itemCount + ")";
                        }
                    }

                }

            }
            else
            {
                bool addedItem = false;
                for (int i = 0; i < itemStacks.Length; i++)
                {
                    if (itemStacks[i] > 0)
                    {
                        bool scrap = ItemCatalog.GetItemDef((ItemIndex)i).ContainsTag(ItemTag.Scrap);
                        if (scrap)
                        {
                            //This shit sucks and would not work with a StringBuilder
                            if (addedItem == true)
                            {
                                itemsLost = ", " + itemsLost;
                            }
                            if (itemStacks[i] > 1)
                            {
                                itemsLost = itemStacks[i].ToString() + "x " + Help.GetColoredName((ItemIndex)i) + itemsLost;
                            }
                            else
                            {
                                itemsLost = Help.GetColoredName((ItemIndex)i) + itemsLost;
                            }
                        }
                        else
                        {
                            if (addedItem == true)
                            {
                                itemsLost += ", ";
                            }
                            if (itemStacks[i] > 1)
                            {
                                itemsLost += itemStacks[i].ToString() + "x ";
                            }
                            itemsLost += Help.GetColoredName((ItemIndex)i);
                        }
                        addedItem = true;
                    }
                }
            }
            string result = string.Format(Language.GetString(baseToken), this.GetSubjectName(), itemsLost);
            return result;
        }


        public int itemCount;
        public bool isTempItem;
        public bool isRentalItem;
        public PickupIndex pickupIndex1 = PickupIndex.none;
        public PickupIndex pickupIndex2 = PickupIndex.none;


        //Could redo this like how other things do dynamic list length
        //Would need two arrays /dict touble or smth since we'd lose itemIndex
        public int[] itemStacks;
        
        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(itemCount);
            writer.Write(pickupIndex1);
            writer.Write(pickupIndex2);
            writer.Write((int)source);
            writer.Write(isTempItem);
            writer.Write(isRentalItem);
            if (pickupIndex1 == PickupIndex.none)
            {
                writer.WriteItemStacks(itemStacks);
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
            pickupIndex1 = reader.ReadPickupIndex();
            pickupIndex2 = reader.ReadPickupIndex();
            source = (Source)reader.ReadInt32();
            isTempItem = reader.ReadBoolean();
            isRentalItem = reader.ReadBoolean();
            if (pickupIndex1 == PickupIndex.none)
            {
                itemStacks = ItemCatalog.RequestItemStackArray();
                reader.ReadItemStacks(itemStacks);
              
            }
            //WolfoMain.log.LogWarning("Deserialize " + reader);
        }
    }



}

