using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace WolfoQoL_Client.DeathScreen
{

    public static class EquipOnDeathInventory
    {
        public static EquipmentIndex[] DeathEquips_Player = Array.Empty<EquipmentIndex>();
        public static EquipmentIndex DeathEquip_Enemy1 = EquipmentIndex.None;
        public static KeyItemIndex DeathKeyItem_Player = KeyItemIndex.None;

        public static void AddEquipmentIcons(On.RoR2.UI.ItemInventoryDisplay.orig_AllocateIcons orig, global::RoR2.UI.ItemInventoryDisplay self, int desiredItemCount)
        {
            if (Run.instance && Run.instance.isGameOverServer || self.inventory == null)
            {

                //It'd probably be good if this only ran on death screens but uhhhh
                if (self.name == "Content")
                {
                    Transform area = self.transform.parent.parent.parent;
                    Transform area2 = self.transform.parent.parent;

                    Dictionary<EquipmentIndex, int> equipmentCount = new Dictionary<EquipmentIndex, int>();
                    bool keyItem = false;

                    if (area.name == "KillerItemArea")
                    {
                        if (DeathEquip_Enemy1 != EquipmentIndex.None)
                        {
                            equipmentCount.Add(DeathEquip_Enemy1, 1);
                        }
                    }
                    else if (area.name == "ItemArea" || area2.name == "ItemArea")
                    {
                        if (!DeathScreen_Main.otherMod)
                        {
                            for (int i = 0; i < DeathEquips_Player.Length; i++)
                            {
                                if (DeathEquips_Player[i] != EquipmentIndex.None)
                                {
                                    if (equipmentCount.ContainsKey(DeathEquips_Player[i]))
                                    {
                                        equipmentCount[DeathEquips_Player[i]]++;
                                    }
                                    else
                                    {
                                        equipmentCount.Add(DeathEquips_Player[i], 1);
                                    }
                                }
                            }

                        }
                        if (DeathKeyItem_Player != KeyItemIndex.None)
                        {
                            keyItem = true;
                        }
                    }

                    if (equipmentCount.Count == 0 && !keyItem)
                    {
                        orig(self, desiredItemCount);
                    }
                    else
                    {
                        orig(self, desiredItemCount + equipmentCount.Count + (keyItem ? 1 : 0));
                        for (int i = 0; i < equipmentCount.Count; i++)
                        {
                            var pair = equipmentCount.ElementAt(i);
                            //WolfoMain.log.LogMessage(pair.Key + " : " + pair.Value);
                            MakeEquipmentIcon(self.itemIcons[desiredItemCount + i], pair.Key, pair.Value);
                        }
                        if (keyItem)
                        {
                            MakeKeyItemIcon(self.itemIcons[desiredItemCount + equipmentCount.Count], DeathKeyItem_Player);
                        }
                    }
                    return;
                }
            }
            orig(self, desiredItemCount);

        }


        public static void MakeEquipmentIcon(ItemIcon icon, EquipmentIndex index, int stack)
        {
            Log.LogMessage("Making EquipmentIcon");
            icon.SetItemIndex(new ItemIcon.SetItemIndexArgs
            {
                newItemIndex = (ItemIndex)1,
                newItemCount = stack,
                newDurationPercent = 0,
                isRental = false,
                isItemDisabled = false
            });

            EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(index);
            icon.name = "EquipmentIcon";
            icon.asyncRawImage.SetAssetOrDirectReference(equipmentDef.pickupIconSpriteReference, equipmentDef.pickupIconSprite);

            TooltipProvider tooltip = icon.GetComponent<TooltipProvider>();
            tooltip.titleToken = equipmentDef.nameToken;
            tooltip.bodyToken = equipmentDef.descriptionToken;
            tooltip.overrideBodyText = Language.GetString(equipmentDef.descriptionToken);
            tooltip.titleColor = ColorCatalog.GetColor(equipmentDef.colorIndex);
            tooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        }
        public static void MakeKeyItemIcon(ItemIcon icon, KeyItemIndex index)
        {
            Log.LogMessage("Making KeyItemIcon");
            icon.SetItemIndex(new ItemIcon.SetItemIndexArgs
            {
                newItemIndex = (ItemIndex)1,
                newItemCount = 1,
                newDurationPercent = 0,
                isRental = false,
                isItemDisabled = false
            });

            KeyItemDef keyItemDef = KeyItemCatalog.GetKeyItemDef(index);
            icon.name = "KeyItemIcon";
            icon.asyncRawImage.SetAssetOrDirectReference(keyItemDef.pickupIconSpriteReference, keyItemDef.pickupIconSprite);

            TooltipProvider tooltip = icon.GetComponent<TooltipProvider>();   
            tooltip.titleToken = keyItemDef.nameToken;
            tooltip.bodyToken = keyItemDef.descriptionToken;
            tooltip.overrideBodyText = Language.GetString(keyItemDef.descriptionToken);
            tooltip.titleColor = ColorCatalog.GetColor(keyItemDef.colorIndex);
            tooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        }
    }


}
