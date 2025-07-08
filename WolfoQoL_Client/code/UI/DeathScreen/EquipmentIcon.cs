using RoR2;
using RoR2.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace WolfoQoL_Client.DeathScreen
{

    public class EquipOnDeathInventory
    {
        public static EquipmentIndex[] DeathEquips_Player = Array.Empty<EquipmentIndex>();
        public static EquipmentIndex DeathEquip_Enemy1 = EquipmentIndex.None;

        public static void AddEquipmentIcons(On.RoR2.UI.ItemInventoryDisplay.orig_AllocateIcons orig, global::RoR2.UI.ItemInventoryDisplay self, int desiredItemCount)
        {
            if (Run.instance && Run.instance.isGameOverServer || self.inventory == null)
            {
              
                //It'd probably be good if this only ran on death screens but uhhhh
                if (self.name == "Content")
                {
                    bool isLog = self.inventory == null;
                    Transform area = self.transform.parent.parent.parent;
                    Transform area2 = self.transform.parent.parent;

                    Dictionary<EquipmentIndex, int> equipmentCount = new Dictionary<EquipmentIndex, int>();
                    if (area.name == "KillerItemArea")
                    {
                        if (DeathEquip_Enemy1 != EquipmentIndex.None)
                        {
                            equipmentCount.Add(DeathEquip_Enemy1, 1);
                        }
                    }
                    else if (area.name == "EvolutionArea")
                    {

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
                    }

                    if (equipmentCount.Count == 0)
                    {
                        orig(self, desiredItemCount);
                    }
                    else
                    {
                        orig(self, desiredItemCount + equipmentCount.Count);
                        for (int i = 0; i < equipmentCount.Count; i++)
                        {
                            var pair = equipmentCount.ElementAt(i);
                            //Debug.Log(pair.Key + " : " + pair.Value);
                            MakeEquipmentIcon(self.itemIcons[desiredItemCount + i], EquipmentCatalog.GetEquipmentDef(pair.Key), pair.Value);
                        }
                    }
                    return;
                }
            }
            orig(self, desiredItemCount);

        }

        
        public static void MakeEquipmentIcon(ItemIcon equipmentIcon, EquipmentDef equipmentDef, int stack)
        {
            Debug.Log("Making EquipmentIcon");
            equipmentIcon.GetComponent<ItemIcon>().SetItemIndex(RoR2Content.Items.BoostAttackSpeed.itemIndex, stack);
 
            equipmentIcon.GetComponent<UnityEngine.UI.RawImage>().texture = equipmentDef.pickupIconTexture;
            TooltipProvider tempTooltip = equipmentIcon.GetComponent<TooltipProvider>();
            equipmentIcon.name = "EquipmentIcon";

            tempTooltip.titleToken = equipmentDef.nameToken;
            tempTooltip.bodyToken = equipmentDef.descriptionToken;
            tempTooltip.overrideBodyText = Language.GetString(equipmentDef.descriptionToken);//Fukin lookin glass
            tempTooltip.bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
            tempTooltip.titleColor = ColorCatalog.GetColor(equipmentDef.colorIndex);
        }
    }


}
