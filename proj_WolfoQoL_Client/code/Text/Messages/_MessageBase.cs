using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoQoL_Client.Text
{

    /*public class CookMessage : RoR2.SubjectChatMessage
    {
        public override string ConstructChatString()
        {
            baseToken = "MEALPREP_COOKMEAL";
            if (base.IsSecondPerson())
            {
                baseToken += "_2P";
            }
            string quantityS = string.Empty;
            if (quantity > 1)
            {
                quantityS = "(" + quantity + ")";
            }
            return Language.GetStringFormatted("MEALPREP_COOKMEAL", new object[]
            {
                    this.GetSubjectName(),
                    Help.GetColoredName(reward),
                    quantityS,
                    Help.GetColoredName(ingredient1),
                    Help.GetColoredName(ingredient2),
            });
        }

        public PickupIndex ingredient1;
        public PickupIndex ingredient2;
        public PickupIndex reward;
        public int quantity;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(ingredient1);
            writer.Write(ingredient2);
            writer.Write(reward);
            writer.Write(quantity);
        }

        public override void Deserialize(NetworkReader reader)
        {
            if (WQoLMain.NoHostInfo == true)
            {
                return;
            }
            base.Deserialize(reader);
            ingredient1 = reader.ReadPickupIndex();
            ingredient2 = reader.ReadPickupIndex();
            reward = reader.ReadPickupIndex();
            quantity = reader.ReadInt32();
        }
    }*/

    public class InteractableMessage : SubjectChatMessage
    {
        public string interactableToken;
        public override string ConstructChatString()
        {
            if (!WConfig.module_text_chat.Value)
            {
                return null;
            }
            if (!WConfig.cfgMessagesSaleStar.Value)
            {
                return null;
            }
            baseToken = "ITEM_USED_STAR";
            return string.Format(Language.GetString(this.GetResolvedToken()), this.GetSubjectName(), Language.GetString(interactableToken));
        }
        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(interactableToken);
        }
        public override void Deserialize(NetworkReader reader)
        {
            if (WQoLMain.NoHostInfo == true)
            {
                return;
            }
            base.Deserialize(reader);
            interactableToken = reader.ReadString();
        }


    }

    public class RecycleMessage : ChatMessageBase
    {
        public PickupIndex oldPickup;
        public PickupIndex newPickup;
        public bool isTemp;
        public override string ConstructChatString()
        {
            if (!WConfig.cfgMessagesRecycler.Value)
            {
                return null;
            }
            string token1 = "";
            string token2 = "";
            string hex = "";

            PickupDef pickup1 = PickupCatalog.GetPickupDef(oldPickup);
            PickupDef pickup2 = PickupCatalog.GetPickupDef(newPickup);
            hex = ColorUtility.ToHtmlStringRGB(pickup1.baseColor);
            if (pickup1.itemIndex != ItemIndex.None)
            {
                token1 = ItemCatalog.GetItemDef(pickup1.itemIndex).nameToken;
                token2 = ItemCatalog.GetItemDef(pickup2.itemIndex).nameToken;
            }
            else if (pickup1.equipmentIndex != EquipmentIndex.None)
            {
                token1 = EquipmentCatalog.GetEquipmentDef(pickup1.equipmentIndex).nameToken;
                token2 = EquipmentCatalog.GetEquipmentDef(pickup2.equipmentIndex).nameToken;
            }
            if (this.isTemp)
            {
                token1 = Language.GetStringFormatted("ITEM_MODIFIER_TEMP", Language.GetString(token1));
                token2 = Language.GetStringFormatted("ITEM_MODIFIER_TEMP", Language.GetString(token2));
            }
            else
            {
                token1 = Language.GetString(token1);
                token2 = Language.GetString(token2);
            }
            return string.Format(Language.GetString("ITEM_RECYCLED_GLOBAL"), token1, token2, hex);
        }

    }




}

