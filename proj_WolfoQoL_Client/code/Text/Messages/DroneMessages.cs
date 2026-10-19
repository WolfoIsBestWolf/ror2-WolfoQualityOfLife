using EntityStates.AffixVoid;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using WolfoLibrary;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.Text
{
    public static class DroneMessages
    {

        public static void Start()
        {
            On.EntityStates.DroneCombiner.DroneCombinerCombining.StartDroneCombineSequence += CombinerMessage_ClientHost;
            On.RoR2.MinionOwnership.MinionGroup.AddMinion += DroneMessage_ClientOnly;
            On.RoR2.CharacterMaster.SpawnRemoteOperationDrone += RemoteOP_Host;
            //RemoteOP Client handled by KillerInv listener

            On.RoR2.SummonMasterBehavior.NotifySummonerInternal += DroneMessage_RepairHost;
            On.RoR2.DroneVendorTerminalBehavior.NotifySummonerInternal += DroneMessage_RepairHostShop;
            //On.RoR2.CharacterMasterNotificationQueue.PushDroneNotification //Technically better but LookingGlass used this without purchasing a drone so uhh

            On.EntityStates.Drone.DeathState.OnEnter += DeathState_OnEnter;
            On.EntityStates.Drone.MegaDroneDeathState.OnEnter += MegaDroneDeathState_OnEnter;
            On.EntityStates.Turret1.DeathState.OnEnter += DeathState_OnEnter1;
        }

        private static void MegaDroneDeathState_OnEnter(On.EntityStates.Drone.MegaDroneDeathState.orig_OnEnter orig, EntityStates.Drone.MegaDroneDeathState self)
        {
            orig(self);
            DroneDeathMessage(self.characterBody);
        }

        private static void DeathState_OnEnter(On.EntityStates.Drone.DeathState.orig_OnEnter orig, EntityStates.Drone.DeathState self)
        {
            orig(self);
            DroneDeathMessage(self.characterBody);
        }
        private static void DeathState_OnEnter1(On.EntityStates.Turret1.DeathState.orig_OnEnter orig, EntityStates.Turret1.DeathState self)
        {
            orig(self);
            DroneDeathMessage(self.characterBody);
        }

        public static void DroneDeathMessage(CharacterBody body)
        {
            if (!WConfig.DroneMessage_Death.Value)
            {
                return;
            }
            if (!body || !body.inventory)
            {
                return;
            }
            if (NetworkServer.active && body.master.IsDeadAndOutOfLivesServer() == false || Util.HasReviveItemOrEquipment(body.inventory))
            {
                return;
            }
            if (body.master.GetComponent<SetDontDestroyOnLoad>() == null)
            {
                return;
            }
            DroneIndex drone = DroneCatalog.GetDroneIndexFromBodyIndex(body.bodyIndex);
            if (drone == DroneIndex.None)
            {
                return;
            }
            if (!DroneCatalog.GetDroneDef(drone).droneBrokenSpawnCard)
            {
                return;
            }
            if (!body.master || !body.master.minionOwnership || !body.master.minionOwnership.ownerMaster || !body.master.minionOwnership.ownerMaster.playerCharacterMasterController)
            {
                //No owner
                return;
            }
            if (drone == DroneIndex.None)
            {
                return;
            }

            int random = UnityEngine.Random.Range(1, 7);

            Chat.AddMessage(new DroneChatMessage
            {
                baseToken = "DEATH_DRONE_"+ random,
                DroneTier = body.inventory.GetItemCountPermanent(DLC3Content.Items.DroneUpgradeHidden),
                _DroneIndex = drone,
                subjectNetworkUserObject = body.master.minionOwnership.ownerMaster.playerCharacterMasterController.networkUser.gameObject,
                pickupQuantity = 1,
                droneMaster = body.masterObject
            });
        }
        

        public static void DroneMessage_ClientOnly(On.RoR2.MinionOwnership.MinionGroup.orig_AddMinion orig, NetworkInstanceId ownerId, MinionOwnership minion)
        {
            //PURCHASE_DRONE
            orig(ownerId, minion);
            //Owned by Player
            if (!WConfig.DroneMessage_Repair.Value)
            {
                return;
            }
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
                           DroneMessages.DroneMessage_Client(minion.group, minion.ownerMaster.playerCharacterMasterController.networkUser, minion.GetComponent<Inventory>().GetItemCountPermanent(DLC3Content.Items.DroneUpgradeHidden), droneIndexFromBodyIndex);
                       }
                   }
                }
            }
        }

        private static void CombinerMessage_ClientHost(On.EntityStates.DroneCombiner.DroneCombinerCombining.orig_StartDroneCombineSequence orig, EntityStates.DroneCombiner.DroneCombinerCombining self)
        {
            orig(self);
            if (!WConfig.DroneMessage_Combine.Value)
            {
                return;
            }

            //This shit does not work client side because of some dumb error
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
                   Networker.SendWQoLMessage(newMsg);
                }
                else
                {
                   Chat.AddMessage(newMsg);
                }
            }
        }

        private static void DroneMessage_RepairHostShop(On.RoR2.DroneVendorTerminalBehavior.orig_NotifySummonerInternal orig, DroneVendorTerminalBehavior self, GameObject summonerBodyObject, GameObject minionBodyObject)
        {
            orig(self, summonerBodyObject, minionBodyObject);
            DroneMessage_Host(summonerBodyObject, minionBodyObject, self._cachedPickup.upgradeValue, "PURCHASE_DRONE");
        }

        private static void DroneMessage_RepairHost(On.RoR2.SummonMasterBehavior.orig_NotifySummonerInternal orig, SummonMasterBehavior self, GameObject summonerBodyObject, GameObject minionBodyObject)
        {
            orig(self, summonerBodyObject, minionBodyObject);
            DroneMessage_Host(summonerBodyObject, minionBodyObject, self.droneUpgradeCount, "REPAIR_DRONE");
        }

        private static void DroneMessage_Upgrading(On.EntityStates.DroneCombiner.DroneCombinerCombining.orig_SeatAndHideDrone orig, EntityStates.DroneCombiner.DroneCombinerCombining self, CharacterBody drone)
        {
            orig(self, drone);
            var owner = drone.GetOwnerBody();
            if (owner)
            {
                Networker.SendWQoLMessage(new DroneChatMessage
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
            if (!WConfig.DroneMessage_RemoteOp.Value)
            {
                return;
            }
            if (!string.IsNullOrEmpty(droneBodyToSpawn))
            {
                Networker.SendWQoLMessage(new DroneChatMessage
                {
                   baseToken = "REMOTEOP_DRONE",
                   _DroneIndex = DroneCatalog.GetDroneIndexFromBodyIndex(BodyCatalog.FindBodyIndexCaseInsensitive(droneBodyToSpawn)),
                   subjectAsNetworkUser = self.playerCharacterMasterController.networkUser,
                });
            }
        }

        public static void DroneMessage_Host(GameObject summonerBodyObject, GameObject minionBodyObject, int upgradeCount, string token)
        {
            if (!WConfig.DroneMessage_Repair.Value)
            {
                return;
            }
            if (!NetworkServer.active)
            {
                return;
            }
            CharacterBody characterBody = (summonerBodyObject != null) ? summonerBodyObject.GetComponent<CharacterBody>() : null;
            if (characterBody && characterBody.master && characterBody.master.playerCharacterMasterController)
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
                   Networker.SendWQoLMessage(new DroneChatMessage
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


        public static void DroneMessage_Client(MinionOwnership.MinionGroup minionGroup, NetworkUser networkUser, int upgradeCount, DroneIndex droneIndex)
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
            string droneText = "";

            if (_DroneIndex != DroneIndex.None)
            {
                droneText += Help.GetColoredName(_DroneIndex, DroneTier);
            }
            string quantity = "";
            
            if (_DroneIndex == RoR2Content.DroneDefs.EquipmentDrone.droneIndex)
            {
                if (droneMaster)
                {
                    Inventory inv = droneMaster.GetComponent<Inventory>();
                    if (inv && inv.currentEquipmentIndex != EquipmentIndex.None)
                    {
                        droneText += " (" + Language.GetString(EquipmentCatalog.GetEquipmentDef(inv.currentEquipmentIndex).nameToken) + ")";
                    }
                }
            }

            if (noBracketQuantity)
            {
                quantity = pickupQuantity.ToString();
            }
            else if (pickupQuantity > 1U)
            {
                quantity = "(" + pickupQuantity + ")";
            }
            string result = string.Format(Language.GetString(baseToken), this.GetSubjectName(), droneText, quantity);

            return result;
        }


        public DroneIndex _DroneIndex;
        public int DroneTier;
        public int pickupQuantity;
        public bool noBracketQuantity = false;
        public GameObject droneMaster;

        public override void Serialize(NetworkWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)_DroneIndex);
            writer.Write(DroneTier);
            writer.Write(pickupQuantity);
            writer.Write(noBracketQuantity);
            writer.Write(droneMaster);
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
            droneMaster = reader.ReadGameObject();
        }
    }




}

