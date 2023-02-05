using RoR2;
using System.Collections.Generic;
using UnityEngine;


namespace WolfoQualityOfLife
{

    public class WolfoItemMessages
    {
        //private string note = "Updating Mods that will likely never be updated for personal use. Not indended to be used by anyone else. Just here so I can share it with friends.";
        private static EquipmentDef equipdef;
        private static string tempusername;
        private static Interactor tempscrapinteractor;
        private static string fakeCleanseMessage;
        private static string fakeCleanseMessage_P2;
        private static bool IsCleanseShrine = false;
        private static bool IsCauldron = false;
        private static bool IsScrapper = false;
        private static bool IsUsedItem = false;



        public static void DetailedDeathMessage()
        {

            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += (orig, self, damageReport, networkUser) =>
            {
                orig(self, damageReport, networkUser);

                if (!networkUser) return;


                //Debug.LogWarning(damageReport.damageInfo.damageType);
                //Debug.LogWarning(damageReport.damageInfo.damageColorIndex);


                //networkUser.GetCurrentBody().isGlass

                //<color=#77FF17> //Green Item - Friendly/User
                //<color=#E7543A> //Red Item - Enemy/Killer
                //<color=#95CDE5> //Blue UtilityText - World/Unknown Killer
                //<color=#42D7F7> //Blue from Lunar Items UI Render
                //<color=#EBE87A> //Money Color - Damage Value

                string KillerName = "Unknown Killer";
                string VictimName = RoR2.Util.GetBestBodyName(damageReport.victimBody.gameObject);
                if (damageReport.attackerBody != null)
                {
                    KillerName = RoR2.Util.GetBestBodyName(damageReport.attacker);
                    KillerName = KillerName.Replace("\n", " ");
                }
                float DamageValue = damageReport.damageInfo.damage;


                Debug.LogWarning(VictimName);
                Debug.LogWarning(KillerName);
                //Debug.LogWarning(DamageValue);

                //Debug.LogWarning(damageReport.damageInfo.damageType);




                string token = $"<color=#77FF17>{VictimName}</color> was killed by <color=#95CDE5>The Planet</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";

                if (damageReport.dotType != DotController.DotIndex.None && damageReport.isFriendlyFire == false)
                {
                    if (damageReport.dotType == DotController.DotIndex.Fracture)
                    {
                        if (damageReport.attackerBody != null)
                        {
                            token = $"<color=#77FF17>{VictimName}</color> was collapsed by <color=#E7543A>{KillerName}</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                        }
                        else
                        {
                            token = $"<color=#77FF17>{VictimName}</color> was collapsed by <color=#95CDE5>something avoidable</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                        }
                    }
                    else
                    {
                        if (damageReport.attackerBody != null)
                        {
                            token = $"<color=#77FF17>{VictimName}</color> slowly died to <color=#E7543A>{KillerName}</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                        }
                        else
                        {
                            token = $"<color=#77FF17>{VictimName}</color> slowly died to <color=#95CDE5>unknown causes</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                        }
                    }
                }
                else if (damageReport.damageInfo.damageType.HasFlag(DamageType.BypassArmor | DamageType.BypassBlock) && damageReport.damageInfo.damageColorIndex == DamageColorIndex.Void)
                {
                    token = $"<color=#77FF17>{VictimName}</color> drowned in <color=#95CDE5>The Void</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                }
                else if (damageReport.damageInfo.damageType.HasFlag(DamageType.VoidDeath))
                {
                    token = $"<color=#77FF17>{VictimName}</color> was vanquished <color=#EBE87A>instantly</color> by <color=#E7543A>{KillerName}</color>";
                }
                else if (damageReport.isFallDamage)
                {
                    token = $"<color=#77FF17>{VictimName}</color> succumbed to <color=#95CDE5>Gravity</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                }
                else if (damageReport.attackerBody != null && damageReport.isFriendlyFire)
                {
                    if (VictimName.Equals(KillerName))
                    {
                        if (damageReport.dotType != DotController.DotIndex.None)
                        {
                            token = $"<color=#77FF17>{VictimName}</color> was slowly killed by themselves! <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                        }
                        else
                        {
                            token = $"<color=#77FF17>{VictimName}</color> was killed by themselves! <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                        }
                    }
                    else
                    {
                        if (damageReport.dotType != DotController.DotIndex.None)
                        {
                            token = $"<color=#77FF17>{VictimName}</color> couldn't endure <color=#77FF17>{KillerName}</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                        }
                        else
                        {
                            token = $"<color=#77FF17>{VictimName}</color> was betrayed by <color=#77FF17>{KillerName}</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                        }
                    }
                }
                else if (damageReport.attackerBody != null)
                {
                    token = $"<color=#77FF17>{VictimName}</color> was killed by <color=#E7543A>{KillerName}</color> <color=#EBE87A>({DamageValue:F2} damage taken)</color>";
                }

                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
            };

        }

        public static void ItemLostMessages()
        {
            //Debug.LogWarning("Loaded Wolfo edits");

            On.RoR2.ScrapperController.AssignPotentialInteractor += (orig, self, potentialInteractor) =>
            {
                orig(self, potentialInteractor);
                tempscrapinteractor = potentialInteractor;
            };

            On.RoR2.ScrapperController.BeginScrapping += (orig, self, intPickupIndex) =>
            {
                IsScrapper = true;
                IsCleanseShrine = false;
                IsCauldron = false;
                IsUsedItem = false;
                CharacterBody component = tempscrapinteractor.GetComponent<CharacterBody>();
                tempusername = component.GetUserName();
                if (component)
                {
                    Inventory inventory = component.inventory;
                    if (inventory)
                    {
                        List<ItemCount> listBefore = new List<ItemCount>();
                        List<ItemCount> listAfter = new List<ItemCount>();

                        listBefore = getAllCurrentItems(inventory);

                        orig(self, intPickupIndex);

                        listAfter = getAllCurrentItems(inventory);

                        if (listBefore.Count > 0)
                        {
                            List<ItemCount> difference = new List<ItemCount>();
                            foreach (ItemCount beforeItem in listBefore)
                            {
                                bool itemAppears = false;
                                foreach (ItemCount afterItem in listAfter)
                                {
                                    if (beforeItem.itemIndex == afterItem.itemIndex)
                                    {

                                        itemAppears = true;
                                        if (beforeItem.count > afterItem.count)
                                        {
                                            ItemCount differenceItem = new ItemCount(beforeItem.itemIndex, beforeItem.count - afterItem.count);
                                            difference.Add(differenceItem);
                                        }
                                        break;
                                    }
                                }
                                if (!itemAppears)
                                {
                                    ItemCount differenceItem = new ItemCount(beforeItem.itemIndex, beforeItem.count);
                                    difference.Add(differenceItem);
                                }
                            }
                            string tradeInMessage = constructItemsLostString(difference);
                            SendCustomMessage(tempscrapinteractor, tradeInMessage);
                        }
                    }
                    else
                    {
                        orig(self, intPickupIndex);
                    }
                }
                else
                {
                    orig(self, intPickupIndex);
                }

            };

            On.RoR2.PurchaseInteraction.OnInteractionBegin += (orig, self, activator) =>
            {

                if (!self.CanBeAffordedByInteractor(activator))
                {
                    orig(self, activator);
                    return;
                }
                switch (self.costType)
                {
                    case CostTypeIndex.WhiteItem:
                    case CostTypeIndex.GreenItem:
                    case CostTypeIndex.RedItem:
                    case CostTypeIndex.BossItem:
                    case CostTypeIndex.LunarItemOrEquipment:
                    case CostTypeIndex.Equipment:
                    case CostTypeIndex.VolatileBattery:
                        {
                            CharacterBody component = activator.GetComponent<CharacterBody>();
                            tempusername = component.GetUserName();
                            IsCleanseShrine = false;
                            IsScrapper = false;
                            IsUsedItem = false;
                            IsCauldron = false;
                            if (self.gameObject.name.Contains("ShrineCleanse")) { IsCleanseShrine = true; };
                            if (self.gameObject.name.Contains("LunarCauldron")) { IsCauldron = true; };
                            if (self.costType == CostTypeIndex.TreasureCacheItem || self.costType == CostTypeIndex.TreasureCacheVoidItem || self.costType == CostTypeIndex.ArtifactShellKillerItem) { IsUsedItem = true; }
                            if (component)
                            {
                                equipdef = EquipmentCatalog.GetEquipmentDef(component.equipmentSlot.equipmentIndex);
                                Inventory inventory = component.inventory;
                                if (inventory)
                                {
                                    List<ItemCount> listBefore = new List<ItemCount>();
                                    List<ItemCount> listAfter = new List<ItemCount>();

                                    listBefore = getAllCurrentItems(inventory);

                                    orig(self, activator);

                                    listAfter = getAllCurrentItems(inventory);

                                    if (listBefore.Count >= 0)
                                    {
                                        List<ItemCount> difference = new List<ItemCount>();
                                        foreach (ItemCount beforeItem in listBefore)
                                        {
                                            bool itemAppears = false;
                                            foreach (ItemCount afterItem in listAfter)
                                            {
                                                if (beforeItem.itemIndex == afterItem.itemIndex)
                                                {

                                                    itemAppears = true;
                                                    if (beforeItem.count > afterItem.count)
                                                    {
                                                        ItemCount differenceItem = new ItemCount(beforeItem.itemIndex, beforeItem.count - afterItem.count);
                                                        difference.Add(differenceItem);
                                                    }
                                                    break;
                                                }
                                            }
                                            if (!itemAppears)
                                            {
                                                ItemCount differenceItem = new ItemCount(beforeItem.itemIndex, beforeItem.count);
                                                difference.Add(differenceItem);
                                            }
                                        }
                                        string tradeInMessage = constructItemsLostString(difference);
                                        SendCustomMessage(activator, tradeInMessage);
                                    }
                                    return;
                                }
                            }
                            break;
                        }
                }

                orig(self, activator);

            };
        }

        private static void SendCustomMessage(Interactor activator, string tradeInMessage)
        {
            if (IsCleanseShrine == true)
            {
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = fakeCleanseMessage });
            }
            else
            {
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = tradeInMessage });
            }
        }

        private static string constructItemsLostString(List<ItemCount> difference)
        {
            //fakeCleanseMessage_P2 = "<style=cShrine>You have cleansed yourself of </color>";
            fakeCleanseMessage = "<style=cShrine>" + tempusername + " has cleansed themself of </color>";

            string result = "<color=#8296ae>" + tempusername + " gave up ";


            if (difference.Count == 0)
            {
                string equipname = Language.GetString(equipdef.nameToken);
                string hexColorequip = "#FF8000";
                hexColorequip = "#" + ColorUtility.ToHtmlStringRGB(PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("EquipmentIndex." + equipdef.name)).baseColor);

                result += "their</color> <color=" + hexColorequip + ">" + equipname + "</color>";
                fakeCleanseMessage += "<color=" + hexColorequip + ">" + equipname + "</color>";
                //fakeCleanseMessage_P2 += "<color=" + hexColorequip + ">" + equipname + "</color>";
            }
            else if (difference.Count > 0)
            {
                result = "<color=#8296ae>" + tempusername + " printed away ";
            }


            if (IsScrapper == true)
            {
                result = "<color=#8296ae>" + tempusername + " scrapped ";
            }
            else if (IsUsedItem == true)
            {
                result = "<color=#8296ae>" + tempusername + " used up ";
            }
            else if (IsCauldron == true)
            {
                result = "<color=#8296ae>" + tempusername + " reforged ";
            }


            for (int i = 0; i < difference.Count; i++)
            {
                ItemCount diffItem = difference[i];
                //PickupIndex pickupIndex = new PickupIndex(diffItem.itemIndex);
                PickupIndex pickupIndex = PickupCatalog.FindPickupIndex(diffItem.itemIndex);
                string itemName = Language.GetString(ItemCatalog.GetItemDef(diffItem.itemIndex).nameToken);
                string hexColor = "#" + ColorUtility.ToHtmlStringRGB(pickupIndex.GetPickupColor());


                if (diffItem.count > 1)
                {
                    result += "x" + diffItem.count + " " + "<color=" + hexColor + ">" + itemName + "</color>";
                }
                else
                {
                    result += "<color=" + hexColor + ">" + itemName + "</color>";
                }


                fakeCleanseMessage += "<color=" + hexColor + ">" + itemName + "</color>";
                //fakeCleanseMessage_P2 += "<color=" + hexColor + ">" + itemName + "</color>";

                if (i + 1 < difference.Count)
                {
                    result += ", ";
                }
                else
                {
                    result += "</color>";
                }
            }

            return result;
        }

        private static List<ItemCount> getAllCurrentItems(Inventory inv)
        {
            List<ItemCount> list = new List<ItemCount>();
            foreach (ItemIndex itemIndex in inv.itemAcquisitionOrder)
            {
                ItemCount itemCount = new ItemCount(itemIndex, inv.GetItemCount(itemIndex));
                list.Add(itemCount);
            }
            return list;
        }





    }
    class ItemCount
    {
        public ItemIndex itemIndex;
        public int count;

        public ItemCount(ItemIndex index, int count)
        {
            this.itemIndex = index;
            this.count = count;
        }

    }




}

