using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using WolfoQoL_Client.Reminders;
using WolfoQoL_Client.Text;

namespace WolfoQoL_Client
{
    public class ClientChecks
    {
        public static void Start()
        {
            //Maybe we could add most of these only when needed?
            On.RoR2.PlayerCharacterMasterController.Start += AddClientListeners;
            On.RoR2.PlayerCharacterMasterController.SetBodyPrefabToPreference += PlayerCharacterMasterController_SetBodyPrefabToPreference;

            On.RoR2.MultiShopController.OnDeserialize += ClientCheck_Multishop;
            On.EntityStates.Scrapper.ScrapperBaseState.OnEnter += ScrapperBaseState_OnEnter;

            IL.RoR2.PurchaseInteraction.OnDeserialize += ClientCheck_Purchase;
            IL.RoR2.Inventory.OnDeserialize += Inventory_OnDeserialize;

            On.RoR2.SummonMasterBehavior.OnDisable += EquipmentDrone_ForceMessage;
        }

        private static void PlayerCharacterMasterController_SetBodyPrefabToPreference(On.RoR2.PlayerCharacterMasterController.orig_SetBodyPrefabToPreference orig, PlayerCharacterMasterController self)
        {
            Debug.Log("PlayerCharacterMasterController_SetBodyPrefabToPreference");
            orig(self);
            if (self.TryGetComponent<ItemLossBase_ClientListener>(out var a))
            {
                a.subjectAsNetworkUser = self.networkUser;
            }
        }

        private static void EquipmentDrone_ForceMessage(On.RoR2.SummonMasterBehavior.orig_OnDisable orig, SummonMasterBehavior self)
        {
            //EquipmentDrone purchaseInteraction does not get Deserealized before being Destroyed
            //So we kinda just have to force it?
            orig(self);
            if (self.callOnEquipmentSpentOnPurchase)
            {
                var purchasse = self.GetComponent<PurchaseInteraction>();
                if (purchasse && purchasse.costType == CostTypeIndex.Equipment)
                {
                    PlayerItemLoss_ClientListener.forceEquip = true;
                    PlayerItemLoss_ClientListener.token = "ITEM_LOSS_GENERIC";
                    PlayerItemLoss_ClientListener.TryMessage();
                }
            }
        }

        private static void ClientCheck_Purchase(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.PurchaseInteraction", "available"));
            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.PurchaseInteraction", "available")
            ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<bool, PurchaseInteraction, bool>>((t, pur) =>
                {
                    if (t == false)
                    {
                        CheckInteractable_ClientOnly(pur, false);
                    }
                    return t;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : PurchaseInteraction_OnDeserialize");
            }
        }

        private static void Inventory_OnDeserialize(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
             x => x.MatchLdfld("RoR2.Inventory", "itemStacks"),
             x => x.MatchCall("RoR2.NetworkExtensions", "ReadItemStacks")))
            {
                c.EmitDelegate<Func<Inventory, Inventory>>((inventory) =>
                {
                    PlayerItemLoss_ClientListener a;
                    if (inventory.TryGetComponent<PlayerItemLoss_ClientListener>(out a))
                    {
                        //Debug.Log("Desearal Items");
                        Array.Copy(inventory.itemStacks, a.prevItems, a.prevItems.Length);
                        a.prevEquip = inventory.currentEquipmentIndex;
                    }
                    return inventory;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : Inventory_OnDeserialize");
            }
            c.TryGotoNext(MoveType.Before,
             x => x.MatchStfld("RoR2.Inventory", "equipmentDisabled"));


            //Maybe it's just this as an issue?
            if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdarg(0)))
            {
                c.EmitDelegate<Func<Inventory, Inventory>>((inventory) =>
                {
                    //Latest equipment
                    PlayerItemLoss_ClientListener a;
                    if (inventory.TryGetComponent<PlayerItemLoss_ClientListener>(out a))
                    {
                        a.prevEquip = inventory.currentEquipmentIndex;
                    }
                    return inventory;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : Inventory_OnDeserialize");
            }
        }

        private static void ScrapperBaseState_OnEnter(On.EntityStates.Scrapper.ScrapperBaseState.orig_OnEnter orig, EntityStates.Scrapper.ScrapperBaseState self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                if (self is EntityStates.Scrapper.WaitToBeginScrapping)
                {
                    PlayerItemLoss_ClientListener.isScrapper = true;
                    PlayerItemLoss_ClientListener.TryMessage();
                }
            }
        }

        public static void AddClientListeners(On.RoR2.PlayerCharacterMasterController.orig_Start orig, PlayerCharacterMasterController self)
        {
            orig(self);
            if (!WolfoMain.HostHasMod)
            {
                self.gameObject.AddComponent<KillerInfo_ClientListener>();
                self.gameObject.AddComponent<PlayerItemLoss_ClientListener>();
            }

        }

        //If interactables used too fast together, the clients never actually get it set to true
        //So true to false isn't accurate when holding E

        private static void ClientCheck_Multishop(On.RoR2.MultiShopController.orig_OnDeserialize orig, MultiShopController self, NetworkReader reader, bool initialState)
        {
            var available = self.available;
            orig(self, reader, initialState);
            if (available == true && self.available == false && self.name.StartsWith("FreeChestMultiShop"))
            {
                CheckInteractable_ClientOnly(null, true);
            }
        }

        public static void CheckInteractable_ClientOnly(PurchaseInteraction purchase, bool isMulti)
        {
            if (isMulti)
            {
                if (TreasureReminder.instance.Objective_FreeChest)
                {
                    TreasureReminder.instance.DeductFreeChestCount();
                }
            }
            else
            {
                if (purchase.saleStarCompatible)
                {
                    PlayerItemLoss_ClientListener.TrySaleStar(purchase);
                }
                switch (purchase.costType)
                {
                    case CostTypeIndex.WhiteItem:
                    case CostTypeIndex.GreenItem:
                    case CostTypeIndex.RedItem:
                    case CostTypeIndex.BossItem:

                        if (purchase.gameObject.name.StartsWith("LunarCauldron"))
                        {
                            PlayerItemLoss_ClientListener.token = "ITEM_LOSS_CAULDRON";
                        }
                        else
                        {
                            PlayerItemLoss_ClientListener.token = "ITEM_LOSS_GENERIC";
                        }
                        PlayerItemLoss_ClientListener.TryMessage();
                        break;
                    case CostTypeIndex.Equipment:
                        PlayerItemLoss_ClientListener.checkEquip = true;
                        PlayerItemLoss_ClientListener.token = "ITEM_LOSS_GENERIC";
                        PlayerItemLoss_ClientListener.TryMessage();
                        break;
                    case CostTypeIndex.LunarItemOrEquipment:
                        PlayerItemLoss_ClientListener.checkEquip = true;
                        PlayerItemLoss_ClientListener.token = "ITEM_LOSS_CLEANSING";
                        PlayerItemLoss_ClientListener.TryMessage();
                        break;
                    case CostTypeIndex.TreasureCacheItem:
                        if (TreasureReminder.instance)
                        {
                            TreasureReminder.instance.DeductLockboxCount();
                        }
                        break;
                    case CostTypeIndex.TreasureCacheVoidItem:
                        if (TreasureReminder.instance)
                        {
                            TreasureReminder.instance.DeductLockboxVoidCount();
                        }
                        break;
                    case CostTypeIndex.None:
                        if (purchase.gameObject.GetComponent<ShrineBossBehavior>())
                        {
                            ShrineBoss_IconStack.AddShrineIcon_Static();
                        }
                        break;
                }


            }
        }
    }

    public class KillerInfo_ClientListener : MonoBehaviour
    {
        public GameObject playerBodyObject;
        public CharacterBody charBody;
        public CharacterMaster playerMaster;
        public UnityAction deathEvent;

        public bool killed = false;

        public void OnEnable()
        {
            if (WolfoMain.HostHasMod)
            {
                Debug.Log("Host has mod, removing Client Workaround : KillerInfo_ClientListener");
                Destroy(this);
                return;
            }

            var Player = this.GetComponent<PlayerCharacterMasterController>();
            if (Player == null)
            {
                return;
            }
            GlobalEventManager.onClientDamageNotified += this.onClientDamageNotified;
            playerMaster = Player.master;
            playerMaster.onBodyStart += Master_onBodyStart;
            if (deathEvent == null)
            {
                deathEvent = new UnityAction(this.OnBodyDeath);
                playerMaster.onBodyDeath.AddListener(deathEvent);
            }

        }
        public void OnDisable()
        {
            if (deathEvent != null)
            {
                playerMaster.onBodyDeath.RemoveListener(deathEvent);
            }
            playerMaster.onBodyStart -= Master_onBodyStart;
            GlobalEventManager.onClientDamageNotified -= this.onClientDamageNotified;
        }

        public void SendMessage()
        {
            if (latestDmg == null)
            {
                return;
            }
            if (charBody.healthComponent.alive)
            {
                Debug.Log("KillerInfo_ClientListener | Sending message but is Alive?" + charBody);
            }
            if (killed)
            {
                Debug.Log("KillerInfo_ClientListener | Already sent message, how are they dying again?" + charBody);
                return; //Prevents multiple messages rarely
            }
            killed = true;

            string killerName = Util.GetBestBodyName(latestDmg.attacker);
            CharacterMaster attackerMaster = null;
            CharacterBody attackerBody = null;
            if (latestDmg.attacker)
            {
                attackerBody = latestDmg.attacker.GetComponent<CharacterBody>();
                if (attackerBody)
                {
                    attackerMaster = attackerBody.master;
                }
            }
            DeathMessage.DeathAndKillerInventoryMessage(latestDmg.damage, charBody, attackerBody, attackerMaster, latestDmg.damageType, latestDmg.damageColorIndex, latestDmg.delayedHit, false);

        }

        public System.Collections.IEnumerator Delay_DeathMessage()
        {
            yield return new WaitForSeconds(0.3f);
            SendMessage();
            latestDmg = null;
            yield break;
        }

        private void Master_onBodyStart(CharacterBody obj)
        {
            killed = false;
            playerBodyObject = obj.gameObject;
            charBody = obj;
        }

        private void OnBodyDeath()
        {
            //Delayed message to try and ensure correct killer
            StartCoroutine(Delay_DeathMessage());
            if (WConfig.cfgTestClient.Value)
            {
                SendMessage();
                killed = false;
            }

        }
        private DamageDealtMessage latestDmg;
        private void onClientDamageNotified(DamageDealtMessage dmg)
        {
            if (dmg.victim == playerBodyObject)
            {
                latestDmg = dmg;
            }
        }
    }



    public class ItemLossBase_ClientListener : MonoBehaviour
    {
        //Used for all players at once ig?
        public static ItemLossBase_ClientListener latest;
        public Inventory inventory;

        public NetworkUser subjectAsNetworkUser;

        public int[] prevItems = ItemCatalog.RequestItemStackArray();
        public EquipmentIndex prevEquip = EquipmentIndex.None;

        public virtual void OnEnable()
        {
            if (WolfoMain.HostHasMod)
            {
                Debug.Log("Host has mod, removing Client Workaround : ItemLoss_ClientListener");
                Destroy(this);
                return;
            }
            inventory = GetComponent<Inventory>();
            inventory.onInventoryChanged += Inventory_onInventoryChanged;
        }
        public virtual void OnDisable()
        {
            if (inventory)
            {
                inventory.onInventoryChanged -= Inventory_onInventoryChanged;
            }
        }


        public virtual void Inventory_onInventoryChanged()
        {
            //Debug.Log("onInventoryChanged"+this.gameObject);
            latest = this;
        }
        public static void TryMessage()
        {
            if (latest != null)
            {
                latest.TryMessageInstance();
            }
        }
        public virtual void TryMessageInstance()
        {
            //Debug.Log("TryMessageInstance"+this.gameObject);
        }
    }

    public class DevotionItemLoss_ClientListener : ItemLossBase_ClientListener
    {
 
        public override void OnEnable()
        {
            base.OnEnable();
            var a = GetComponent<DevotionInventoryController>().SummonerMaster;
            if (a == null)
            {
                Debug.LogWarning("DevotionItemLoss Client Listener null Summoner Master");
            }
            subjectAsNetworkUser = a.playerCharacterMasterController.networkUser;
        }
 
        public override void Inventory_onInventoryChanged()
        {
            base.Inventory_onInventoryChanged();
            SingleItemLoss(inventory.itemStacks); //Try every time I guess??
        }

        public bool SingleItemLoss(int[] now)
        {
            bool send = false;
            int item = -1;
            int itemCount = -1;
            for (int i = 0; i < prevItems.Length; i++)
            {
                if (prevItems[i] > now[i])
                {
                    send = true;
                    item = i;
                    itemCount = prevItems[i] - now[i];
                    break;
                }
            }
            if (send)
            {
                PickupDef def = PickupCatalog.FindPickupIndex((ItemIndex)item).pickupDef;
                Debug.Log(def.internalName);
                Chat.AddMessage(new DevotionMessage
                {
                    //subjectAsNetworkUser = self._devotionInventoryController.SummonerMaster.playerCharacterMasterController.networkUserObject.GetComponent<NetworkUser>(),
                    subjectAsNetworkUser = subjectAsNetworkUser,
                    baseToken = "DEVOTED_LEMURIAN_DEATH",
                    pickupToken = def.nameToken,
                    pickupColor = def.baseColor,
                    pickupQuantity = (uint)itemCount
                });
            }
            Array.Copy(inventory.itemStacks, prevItems, prevItems.Length);
            return send;
        }


    }


    public class PlayerItemLoss_ClientListener : ItemLossBase_ClientListener
    {
        //Used for all players at once ig?
 
        public static bool isScrapper = false;
        public static bool forceEquip = false;
        public static bool checkEquip = false;
        public static bool checkSaleStar = false;
        public static string token = "ITEM_LOSS_GENERIC";

        //OnEnable is too soon, might need a better way of doing this
        public void Start()
        {
            subjectAsNetworkUser = GetComponent<PlayerCharacterMasterController>().networkUser;

        }



        public override void Inventory_onInventoryChanged()
        {
            base.Inventory_onInventoryChanged();
            /*if (!subjectAsNetworkUser)
            {
                subjectAsNetworkUser = GetComponent<PlayerCharacterMasterController>().networkUser;
            }*/
            //Seems more like summon master happens first, before removing the equipment
            //But might be lag dependent, again, which sucks,
            if (forceEquip)
            {
                TryMessage();
            }
        }
   
        public static void TrySaleStar(PurchaseInteraction purchase)
        {
            if (!WConfig.cfgMessagesSaleStar.Value)
            {
                return;
            }
            if (latest != null)
            {
                int index = (int)DLC2Content.Items.LowerPricedChests.itemIndex;
                int salePre = latest.prevItems[index];
                int salePost = latest.inventory.itemStacks[index];
                Debug.Log("SaleStar " + salePre + " " + salePost);

                if (salePre > 0 && salePost == 0)
                {
                    latest.prevItems[index] = 0;
                    Chat.AddMessage(new SaleStarMessage
                    {
                        interactableToken = purchase.displayNameToken,
                        subjectAsNetworkUser = latest.subjectAsNetworkUser,
                    });
                }


            }
        }

        public override void TryMessageInstance()
        {
            base.TryMessageInstance();
            bool succeeded = false;
            if (isScrapper)
            {
                succeeded = ItemScrapMessage(inventory.itemStacks);
            }
            else if (forceEquip)
            {
                //Since summonMaster might happen before or after inventory deserial ig?
                if (prevEquip == EquipmentIndex.None)
                {
                    prevEquip = inventory.currentEquipmentIndex;
                }
                succeeded = EquipmentMessage();
            }
            else if (checkEquip && inventory.currentEquipmentIndex == EquipmentIndex.None)
            {
                succeeded = EquipmentMessage();
            }
            else
            {
                succeeded = ItemMessage(inventory.itemStacks);
            }
            if (!succeeded)
            {
                //Debug.Log("Attempted message with no differences.");
            }
            //Always set?
            forceEquip = false;
            checkEquip = false;
            isScrapper = false;


        }

        public bool ItemScrapMessage(int[] now)
        {
            if (!WConfig.cfgMessageScrap.Value)
            {
                return false;
            }
            bool send = false;
            int item = -1;
            int itemCount = -1;
            for (int i = 0; i < prevItems.Length; i++)
            {
                if (prevItems[i] > now[i])
                {
                    send = true;
                    item = i;
                    itemCount = prevItems[i] - now[i];
                    break;
                }
            }
            if (send)
            {
                Chat.AddMessage(new ItemLoss_Host.ItemLossMessage
                {
                    baseToken = "ITEM_LOSS_SCRAP",
                    subjectAsNetworkUser = subjectAsNetworkUser,
                    pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex((ItemIndex)item),
                    itemCount = itemCount,
                });
            }
            Array.Copy(inventory.itemStacks, prevItems, prevItems.Length);
            return send;
        }

        public bool ItemMessage(int[] now)
        {
            int[] itemLosses = ItemCatalog.RequestItemStackArray();
            int itemsTaken = 0;
            int item = -1;
            for (int i = 0; i < prevItems.Length; i++)
            {
                if (prevItems[i] > now[i])
                {
                    itemsTaken++;
                    item = i;
                    itemLosses[i] = prevItems[i] - now[i];
                }
            }
            if (itemsTaken == 1)
            {
                if (token != "ITEM_LOSS_CAULDRON")
                {
                    ItemDef tempDef = ItemCatalog.GetItemDef((ItemIndex)item);
                    if (tempDef.ContainsTag(ItemTag.Scrap))
                    {
                        token = "ITEM_LOSS_USED";
                    }
                }
                Chat.AddMessage(new ItemLoss_Host.ItemLossMessage
                {
                    baseToken = token,
                    subjectAsNetworkUser = subjectAsNetworkUser,
                    pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex((ItemIndex)item),
                });
            }
            else if (itemsTaken > 0)
            {
                Chat.AddMessage(new ItemLoss_Host.ItemLossMessage
                {
                    baseToken = token,
                    subjectAsNetworkUser = subjectAsNetworkUser,
                    itemStacks = itemLosses,
                });
            }
            Array.Copy(inventory.itemStacks, prevItems, prevItems.Length);
            return itemsTaken > 0;
        }

        public bool EquipmentMessage()
        {
            bool succeeded = false;
            EquipmentIndex equip = inventory.currentEquipmentIndex;
            if (equip == EquipmentIndex.None)
            {
                equip = prevEquip;
            }
            if (equip != EquipmentIndex.None)
            {
                succeeded = true;
                Chat.AddMessage(new ItemLoss_Host.ItemLossMessage
                {
                    baseToken = token,
                    pickupIndexOnlyOneItem = PickupCatalog.FindPickupIndex(equip),
                    subjectAsNetworkUser = subjectAsNetworkUser,
                });
            }
            else
            {
                Debug.LogWarning("EquipmentMessage but without Equipment?");
            }
            prevEquip = inventory.currentEquipmentIndex;
            return succeeded;
        }


    }




}