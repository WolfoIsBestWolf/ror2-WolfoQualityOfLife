using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.Text
{
    public class DroneMessages
    {

        public static void Start()
        {
            On.EntityStates.DroneCombiner.DroneCombinerCombining.StartDroneCombineSequence += CombinerMessage_ClientHost;
            On.RoR2.MinionOwnership.MinionGroup.AddMinion += DroneMessage_ClientHost;
            On.RoR2.CharacterMaster.SpawnRemoteOperationDrone += RemoteOP_Host;
            //RemoteOP Client handled by KillerInv listener

            On.RoR2.SummonMasterBehavior.NotifySummonerInternal += DroneMessage_Repair1;
            On.RoR2.DroneVendorTerminalBehavior.NotifySummonerInternal += DroneMessage_Repair2;
        }



        public static void DroneMessage_ClientHost(On.RoR2.MinionOwnership.MinionGroup.orig_AddMinion orig, NetworkInstanceId ownerId, MinionOwnership minion)
        {
            //PURCHASE_DRONE
            orig(ownerId, minion);
            //Owned by Player

            if (minion.ownerMaster && minion.ownerMaster.playerCharacterMasterController)
            {
                //Too soon in the spawn cycle to determine is Goobo or Permament of sorts
                //For Host ^
                //Apparently works fine for clients, but so lets just do this for Client HostNotModded
                if (!WQoLMain.HostHasMod)
                {
                    minion.gameObject.AddComponent<DroneLoss_ClientListener>();
                    //If Perma
                    //If Player

                    if (DroneCollection.isPermamentMinion(minion.gameObject))
                    {
                        BodyIndex bodyIndex = BodyCatalog.FindBodyIndex(minion.GetComponent<CharacterMaster>().bodyPrefab);
                        DroneIndex droneIndexFromBodyIndex = DroneCatalog.GetDroneIndexFromBodyIndex(bodyIndex);
                        if (droneIndexFromBodyIndex != DroneIndex.None)
                        {
                            DroneMessages.DroneMessageClient(minion.group, minion.ownerMaster.playerCharacterMasterController.networkUser, minion.GetComponent<Inventory>().GetItemCountPermanent(DLC3Content.Items.DroneUpgradeHidden), droneIndexFromBodyIndex);
                        }
                    }
                }
            }
        }

        private static void CombinerMessage_ClientHost(On.EntityStates.DroneCombiner.DroneCombinerCombining.orig_StartDroneCombineSequence orig, EntityStates.DroneCombiner.DroneCombinerCombining self)
        {
            orig(self);


            //This shit does not work client side because of some dumb fuck error on GBX end

            var drone = self.toUpgrade;
            if (!drone)
            {
                Debug.Log("DRONE COMBINING BUT NO DRONE");
                return;
            }

            var owner = drone.GetOwnerBody();
            if (owner)
            {
                var newMsg = new DroneChatMessage
                {
                    baseToken = "UPGRADE_DRONE",
                    DroneTier = drone.inventory.GetItemCountEffective(DLC3Content.Items.DroneUpgradeHidden),
                    pickupQuantity = drone.inventory.GetItemCountEffective(DLC3Content.Items.DroneUpgradeHidden) + 2,
                    _DroneIndex = DroneCatalog.GetDroneIndexFromBodyIndex(drone.bodyIndex),
                    subjectAsCharacterBody = owner,
                    noBracketQuantity = true,
                };
                if (self.vehicleSeat.netId.Value == 0)
                {
                    Chat.SendBroadcastChat(newMsg);
                }
                else
                {
                    Chat.AddMessage(newMsg);
                }
            }
        }

        private static void DroneMessage_Repair2(On.RoR2.DroneVendorTerminalBehavior.orig_NotifySummonerInternal orig, DroneVendorTerminalBehavior self, GameObject summonerBodyObject, GameObject minionBodyObject)
        {
            orig(self, summonerBodyObject, minionBodyObject);
            DroneMessage(summonerBodyObject, minionBodyObject, self._cachedPickup.upgradeValue, "PURCHASE_DRONE");
        }

        private static void DroneMessage_Repair1(On.RoR2.SummonMasterBehavior.orig_NotifySummonerInternal orig, SummonMasterBehavior self, GameObject summonerBodyObject, GameObject minionBodyObject)
        {
            orig(self, summonerBodyObject, minionBodyObject);
            DroneMessage(summonerBodyObject, minionBodyObject, self.droneUpgradeCount, "REPAIR_DRONE");
        }

        private static void DroneMessage_Upgrading(On.EntityStates.DroneCombiner.DroneCombinerCombining.orig_SeatAndHideDrone orig, EntityStates.DroneCombiner.DroneCombinerCombining self, CharacterBody drone)
        {
            orig(self, drone);
            var owner = drone.GetOwnerBody();
            if (owner)
            {
                Chat.SendBroadcastChat(new DroneChatMessage
                {
                    baseToken = "UPGRADE_DRONE",
                    DroneTier = drone.inventory.GetItemCountEffective(DLC3Content.Items.DroneUpgradeHidden),
                    pickupQuantity = drone.inventory.GetItemCountEffective(DLC3Content.Items.DroneUpgradeHidden) + 2,
                    _DroneIndex = DroneCatalog.GetDroneIndexFromBodyIndex(drone.bodyIndex),
                    subjectAsCharacterBody = owner,
                    noBracketQuantity = true,
                });
            }
        }

        private static void RemoteOP_Host(On.RoR2.CharacterMaster.orig_SpawnRemoteOperationDrone orig, CharacterMaster self, string droneBodyToSpawn, uint moneyToUse)
        {
            orig(self, droneBodyToSpawn, moneyToUse);
            if (!string.IsNullOrEmpty(droneBodyToSpawn))
            {
                Chat.SendBroadcastChat(new DroneChatMessage
                {
                    baseToken = "REMOTEOP_DRONE",
                    _DroneIndex = DroneCatalog.GetDroneIndexFromBodyIndex(BodyCatalog.FindBodyIndexCaseInsensitive(droneBodyToSpawn)),
                    subjectAsNetworkUser = self.playerCharacterMasterController.networkUser,
                });
            }
        }

        public static void DroneMessage(GameObject summonerBodyObject, GameObject minionBodyObject, int upgradeCount, string token)
        {

            CharacterBody characterBody = (summonerBodyObject != null) ? summonerBodyObject.GetComponent<CharacterBody>() : null;
            if (characterBody && characterBody.master)
            {
                DroneIndex droneIndex = DroneCatalog.GetDroneIndexFromBodyIndex(minionBodyObject.GetComponent<CharacterBody>().bodyIndex);
                if (droneIndex != DroneIndex.None)
                {
                    int quantity = 0;
                    MinionOwnership.MinionGroup minionGroup = MinionOwnership.MinionGroup.FindGroup(characterBody.master.netId);
                    if (minionGroup != null)
                    {
                        foreach (MinionOwnership minionOwnership in minionGroup.members)
                        {
                            if (minionOwnership)
                            {
                                CharacterMaster droneMaster = minionOwnership.GetComponent<CharacterMaster>();
                                if (droneMaster)
                                {
                                    //Debug.Log(droneMaster);
                                    CharacterBody body = droneMaster.GetBody();
                                    if (body && droneMaster.inventory)
                                    {
                                        if (droneIndex == DroneCatalog.GetDroneIndexFromBodyIndex(body.bodyIndex))
                                        {
                                            quantity += DroneUpgradeUtils.GetDroneCountFromUpgradeCount(droneMaster.inventory.GetItemCountEffective(DLC3Content.Items.DroneUpgradeHidden));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //Debug.Log(quantity);
                    Chat.SendBroadcastChat(new DroneChatMessage
                    {
                        baseToken = token,
                        DroneTier = upgradeCount,
                        _DroneIndex = droneIndex,
                        subjectAsNetworkUser = characterBody.master.playerCharacterMasterController.networkUser,
                        pickupQuantity = quantity
                    });


                }
            }
        }


        public static void DroneMessageClient(MinionOwnership.MinionGroup minionGroup, NetworkUser networkUser, int upgradeCount, DroneIndex droneIndex)
        {
            int quantity = DroneUpgradeUtils.GetDroneCountFromUpgradeCount(upgradeCount);
            if (minionGroup != null)
            {
                foreach (MinionOwnership minionOwnership in minionGroup.members)
                {
                    if (minionOwnership)
                    {
                        CharacterMaster droneMaster = minionOwnership.GetComponent<CharacterMaster>();
                        if (droneMaster)
                        {
                            //Debug.Log(droneMaster);
                            CharacterBody body = droneMaster.GetBody();
                            if (body && droneMaster.inventory)
                            {
                                if (droneIndex == DroneCatalog.GetDroneIndexFromBodyIndex(body.bodyIndex))
                                {
                                    quantity += DroneUpgradeUtils.GetDroneCountFromUpgradeCount(droneMaster.inventory.GetItemCountEffective(DLC3Content.Items.DroneUpgradeHidden));
                                }
                            }
                        }
                    }
                }
            }
            Chat.AddMessage(new DroneChatMessage
            {
                baseToken = "REPAIR_DRONE",
                DroneTier = upgradeCount,
                _DroneIndex = droneIndex,
                subjectAsNetworkUser = networkUser,
                pickupQuantity = quantity
            });
        }
 

    }


    public class DroneChatMessage : SubjectChatMessage
    {

        public override string ConstructChatString()
        {
            if (!WConfig.module_text_chat.Value)
            {
                return null;
            }
            if (base.IsSecondPerson())
            {
                baseToken += "_2P";
            }
            string droneUpraded = "";

            if (_DroneIndex != DroneIndex.None)
            {
                droneUpraded += Help.GetColoredName(_DroneIndex, DroneTier);
            }
            string quantity = "";
            if (noBracketQuantity)
            {
                quantity = pickupQuantity.ToString();
            }
            else if (pickupQuantity > 1U)
            {
                quantity = "(" + pickupQuantity + ")";
            }
            string result = string.Format(Language.GetString(baseToken), this.GetSubjectName(), droneUpraded, quantity);

            return result;
        }


        public DroneIndex _DroneIndex;
        public int DroneTier;
        public int pickupQuantity;
        public bool noBracketQuantity = false;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)_DroneIndex);
            writer.Write(DroneTier);
            writer.Write(pickupQuantity);
            writer.Write(noBracketQuantity);
        }

        public override void Deserialize(NetworkReader reader)
        {
            if (WQoLMain.NoHostInfo == true)
            {
                return;
            }
            base.Deserialize(reader);
            _DroneIndex = (DroneIndex)reader.ReadInt32();
            DroneTier = reader.ReadInt32();
            pickupQuantity = reader.ReadInt32();
            noBracketQuantity = reader.ReadBoolean();

        }
    }




}

