using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoQoL_Client.Text
{

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
        public UniquePickup newPickup;
        public string token;
        
        public override string ConstructChatString()
        {
            if (!WConfig.module_text_chat.Value)
            {
                return null;
            }

            if (!WConfig.cfgMessagesRecycler.Value)
            {
                return null;
            }
         
            string token1 = "";
            string token2 = "";
            string hex = "";
            string hex2 = "";

            PickupDef pickup1 = PickupCatalog.GetPickupDef(oldPickup);
            PickupDef pickup2 = PickupCatalog.GetPickupDef(newPickup.pickupIndex);
            hex = ColorUtility.ToHtmlStringRGB(pickup1.baseColor);
            hex2 = ColorUtility.ToHtmlStringRGB(pickup2.baseColor);

            token1 = Language.GetString(pickup1.nameToken);
            token2 = Language.GetString(pickup2.nameToken);

            string pickupFormat = GenericPickupController.GetPickupFormat(newPickup);
            if (!string.IsNullOrWhiteSpace(pickupFormat))
            {
                token1 = Language.GetStringFormatted(GenericPickupController.GetPickupFormat(newPickup), Language.GetString(token1));
                token2 = Language.GetStringFormatted(GenericPickupController.GetPickupFormat(newPickup), Language.GetString(token2));
            }

            return string.Format(Language.GetString(token), token1, token2, hex, hex2);
        }

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write(token);
            writer.Write(oldPickup);
            writer.Write(newPickup);
        }
        public override void Deserialize(NetworkReader reader)
        {
            if (WQoLMain.NoHostInfo == true)
            {
                return;
            }
            base.Deserialize(reader);
            token = reader.ReadString();
            oldPickup = reader.ReadPickupIndex();
            newPickup = reader.ReadUniquePickup();
        }
    }

}

