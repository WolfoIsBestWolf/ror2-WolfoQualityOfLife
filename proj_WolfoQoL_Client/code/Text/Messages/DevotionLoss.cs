using RoR2;
using RoR2.CharacterAI;
using UnityEngine.Networking;
using WolfoLibrary;

namespace WolfoQoL_Client.Text
{
    //Duplicate class that wont get to non mod owners, I guess?
    public class DevotionDeathMessage : Chat.PlayerPickupChatMessage
    {
        public override string ConstructChatString()
        {
            if (!WConfig.module_text_chat.Value)
            {
                return null;
            }
            if (!WConfig.cfgMessageDevotion.Value)
            {
                return null;
            }
            return base.ConstructChatString();
        }
    }

    public static class DevotionLoss
    {
        public static void Start()
        {
            //We could just see any item changes in the devoted inventory
            //But we really gotta like, figure out a way to set master


            //Summoner Master is null -> I don't think this is possible?
            //Item is null -> blegh deduction
            //Master is null -> Easy assign

            //Server side only dont do that

            On.RoR2.DevotionInventoryController.RemoveItem += Host_DevotionDeathMessage;
            On.DevotedLemurianController.InitializeDevotedLemurian += Host_EggMessage;

            On.RoR2.PickupPickerController.OnDeserialize += Client_TryEggMessage;
            On.RoR2.DevotionInventoryController.PreStartClient += Client_TryAssignMasterToInventory;
            //On.DevotedLemurianController.Start += Client_TryDeathMessage; //Inventory update -> Death
        }

        private static void Host_DevotionDeathMessage(On.RoR2.DevotionInventoryController.orig_RemoveItem orig, DevotionInventoryController self, ItemIndex itedmIndex, int count)
        {
            orig(self, itedmIndex, count);
            PickupDef def = PickupCatalog.FindPickupIndex(itedmIndex).pickupDef;
            Networker.SendWQoLMessage(new DevotionDeathMessage
            {
                subjectAsNetworkUser = self.SummonerMaster.playerCharacterMasterController.networkUser,
                baseToken = "DEVOTED_LEMURIAN_DEATH",
                pickupToken = def.nameToken,
                pickupColor = def.baseColor,
                pickupQuantity = (uint)(count)
            });
        }


        private static void Client_TryEggMessage(On.RoR2.PickupPickerController.orig_OnDeserialize orig, PickupPickerController self, NetworkReader reader, bool initialState)
        {
            bool was = self.available;
            orig(self, reader, initialState);
            if (was && !self.available)
            {
                //Cannot use LemEgg decereal
                if (self.GetComponent<LemurianEggController>())
                {
                    PlayerItemLoss_ClientListener.source = ItemLossMessage.Source.LemurianEgg;
                    PlayerItemLoss_ClientListener.token = "ITEM_LOSS_EGG";
                    PlayerItemLoss_ClientListener.TryMessage();
                }
            }
        }


        private static void Client_TryAssignMasterToInventory(On.RoR2.DevotionInventoryController.orig_PreStartClient orig, DevotionInventoryController self)
        {
            orig(self);
            if (DevotionItemLoss_ClientListener.latest)
            {
                self.SummonerMaster = DevotionItemLoss_ClientListener.latest.subjectAsNetworkUser.master;
                var a = self.gameObject.AddComponent<DevotionItemLoss_ClientListener>();
            }
        }

        private static void Client_TryDeathMessage(On.DevotedLemurianController.orig_Start orig, DevotedLemurianController self)
        {
            orig(self);
            if (!WQoLMain.HostHasMod)
            {
                self._lemurianMaster = self.GetComponent<CharacterMaster>();
                //self._lemurianMaster.onBodyDeath.AddListener(new UnityAction(DevotionItemLoss_ClientListener.Mark));
            }
        }

        private static void Host_EggMessage(On.DevotedLemurianController.orig_InitializeDevotedLemurian orig, DevotedLemurianController self, ItemIndex itemIndex, DevotionInventoryController devotionInventoryController)
        {
            orig(self, itemIndex, devotionInventoryController);
            Networker.SendWQoLMessage(new ItemLossMessage
            {
                baseToken = "ITEM_LOSS_EGG",
                itemCount = 1,
                pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex(itemIndex),
                subjectAsNetworkUser = self._devotionInventoryController.SummonerMaster.playerCharacterMasterController.networkUser,
            });
        }





    }





}

