using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using WolfoFixes;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.Text
{
    public class StoreLatestPickupindex : MonoBehaviour
    {
        public PickupIndex previousIndex;
    }

    public class MoreMessages
    {

        public static void Start()
        {
            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += DeathMessage.OnDeathMessage;
            ItemLoss_Host.Start();
            LunarSeer.Start();
            EquipmentDrone.Start();
            DevotionLoss.Start();

            RoR2.Run.onClientGameOverGlobal += WinMessage_Client;

            if (WConfig.cfgMessagesColoredItemPings.Value)
            {
                //Colors in Messages when pinging Items
                On.RoR2.GenericPickupController.GetDisplayName += ColoredItem_DisplayName;
                //Item Color in messages when pinging Shops
                IL.RoR2.UI.PingIndicator.RebuildPing += ColoredItemPing_RebuildPing;
            }
            On.RoR2.CharacterMasterNotificationQueue.PushItemTransformNotification += TransformItem_Messages;
            On.RoR2.CharacterMasterNotificationQueue.PushEquipmentTransformNotification += TransformEquipment_Messages;
            IL.RoR2.CharacterMasterNotificationQueue.HandleTransformNotification += SendAllTransformsLol;

            On.RoR2.EquipmentSlot.FireRecycle += RecylerMessage_Host;
            On.RoR2.GenericPickupController.OnDeserialize += RecylerMessage_Client;

            //On.RoR2.PressurePlateController.SetSwitch += PressurePlateController_SetSwitch;
        }

        private static void PressurePlateController_SetSwitch(On.RoR2.PressurePlateController.orig_SetSwitch orig, PressurePlateController self, bool switchIsDown)
        {
            //Message? idk
            orig(self, switchIsDown);
        }

        private static void RecylerMessage_Client(On.RoR2.GenericPickupController.orig_OnDeserialize orig, GenericPickupController self, NetworkReader reader, bool initialState)
        {
            //Sometimes the new Index and Recycle bool come together
            //Sometimes, they do not?
            bool preRecycle = self.Recycled;
            PickupIndex pre = self.NetworkpickupIndex;
            orig(self, reader, initialState);
            //WolfoMain.log.LogMessage(pre + " | "+self.NetworkpickupIndex + " | "+self.Recycled);

            bool newPickup = pre != PickupIndex.none && pre != self.NetworkpickupIndex;
            bool justRecyled = preRecycle == false && self.Recycled == true;
            if (justRecyled && newPickup)
            {
                Chat.AddMessage(new RecycleMessage
                {
                    oldPickup = pre,
                    newPickup = self.NetworkpickupIndex
                });
            }
            else if (newPickup)
            {
                var has = self.gameObject.GetComponent<StoreLatestPickupindex>();
                if (!has)
                {
                    has = self.gameObject.AddComponent<StoreLatestPickupindex>();
                }
                has.previousIndex = pre;
            }
            else if (justRecyled)
            {
                Chat.AddMessage(new RecycleMessage
                {
                    oldPickup = self.gameObject.GetComponent<StoreLatestPickupindex>().previousIndex,
                    newPickup = self.NetworkpickupIndex
                });
            }

        }

        private static bool RecylerMessage_Host(On.RoR2.EquipmentSlot.orig_FireRecycle orig, EquipmentSlot self)
        {
            GenericPickupController pickupController = self.currentTarget.pickupController;
            if (!pickupController || pickupController.Recycled)
            {
                return false;
            }
            PickupIndex pre = pickupController.pickupIndex;
            bool temp = orig(self);

            Chat.AddMessage(new RecycleMessage
            {
                //subjectAsCharacterBody = self.characterBody,
                oldPickup = pre,
                newPickup = pickupController.NetworkpickupIndex
            });

            return temp;
        }

        private static void Interactor_RpcInteractionResult(On.RoR2.Interactor.orig_RpcInteractionResult orig, Interactor self, bool anyInteractionSucceeded)
        {
            orig(self, anyInteractionSucceeded);
            if (anyInteractionSucceeded)
            {
                InteractionDriver driver = self.GetComponent<InteractionDriver>();
            }
        }

        private static void SendAllTransformsLol(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
            x => x.MatchCallvirt("UnityEngine.Networking.NetworkBehaviour", "get_hasAuthority")
            ))
            {
                c.EmitDelegate<Func<bool, bool>>((t) =>
                {
                    return true;
                });
            }
            else
            {
                WolfoMain.log.LogWarning("IL Failed : AttemptToFixClients");
            }
        }

        private static void WinMessage_Client(Run self, RunReport runReport)
        {
            if (runReport.gameEnding == RoR2Content.GameEndings.StandardLoss)
            {
                return;
            }
            else if (runReport.gameEnding == DLC2Content.GameEndings.RebirthEndingDef)
            {
                if (NetworkServer.active)
                {

                    self.StartCoroutine(Courtines.Delayed_RebirthMessage(runReport.playerInfos[0].networkUser, 0.1f));
                }
                else
                {
                    self.StartCoroutine(Courtines.Delayed_RebirthMessage(runReport.playerInfos[0].networkUser, 0.5f));
                }
                return;
            }
            for (int i = 0; runReport.playerInfos.Length > i; i++)
            {
                WinMessageMethod(runReport.gameEnding, runReport.playerInfos[i]);
            }

        }

        private static string ColoredItem_DisplayName(On.RoR2.GenericPickupController.orig_GetDisplayName orig, GenericPickupController self)
        {
            PickupDef pickupDef = PickupCatalog.GetPickupDef(self.pickupIndex);
            if (pickupDef != null)
            {
                return Help.GetColoredName(pickupDef);
            }
            return orig(self);
        }

        private static void ColoredItemPing_RebuildPing(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
             x => x.MatchCallvirt("RoR2.ShopTerminalBehavior", "CurrentPickupIndex")
            //x => x.MatchLdstr("?")
            ))
            {
                //WolfoMain.log.LogMessage(c +"  Next:"+ c.Next.Operand);
                var A = c.Next.Operand;
                c.Index += 27; //UnIdeal
                               //WolfoMain.log.LogMessage(c + "  Next:" + c.Next.Operand);
                c.Emit(OpCodes.Ldloc_S, A);
                c.EmitDelegate<Func<string, PickupIndex, string>>((text, pickupIndex) =>
                {
                    PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                    //WolfoMain.log.LogMessage(pickupDef);
                    if (pickupDef != null)
                    {
                        string hex = ColorUtility.ToHtmlStringRGB(pickupDef.baseColor);
                        //WolfoMain.log.LogMessage(text);
                        text = text.Replace("(", "(<color=#" + hex + ">");
                        text = text.Replace(")", "</color>)");
                        //WolfoMain.log.LogMessage(text);
                    }
                    return text;
                });
                //WolfoMain.log.LogMessage("IL Found : IL.RoR2.UI.PingIndicator.RebuildPing");
            }
            else
            {
                WolfoMain.log.LogWarning("IL Failed : IL.RoR2.UI.PingIndicator.RebuildPing");
            }
        }



        private static void WinMessageMethod(GameEndingDef gameEnd, RunReport.PlayerInfo playerInfo)
        {
            if (playerInfo == null)
            {
                return;
            }
            bool send = false;
            string tokenFormat = "";
            string tokenOutro = "";
            string survToken_WIN = "GENERIC_OUTRO_FLAVOR";
            string survToken_VANISH = "GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR";

            WolfoMain.log.LogMessage(playerInfo.bodyIndex);
            WolfoMain.log.LogMessage(SurvivorCatalog.GetSurvivorIndexFromBodyIndex(playerInfo.bodyIndex));
            SurvivorDef survivorDef = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.GetSurvivorIndexFromBodyIndex(playerInfo.bodyIndex));
            WolfoMain.log.LogMessage(survivorDef);

            if (survivorDef)
            {
                if (survivorDef.outroFlavorToken != null)
                {
                    survToken_WIN = survivorDef.outroFlavorToken;
                }
                if (survivorDef.mainEndingEscapeFailureFlavorToken != null)
                {
                    survToken_VANISH = survivorDef.mainEndingEscapeFailureFlavorToken;
                }              
            }
            if (gameEnd == RoR2Content.GameEndings.MainEnding)
            {
                send = true;
                if (playerInfo.isDead)
                {
                    tokenOutro = survToken_VANISH;
                    tokenFormat = "WIN_FORMAT_FAIL";
                }
                else
                {
                    tokenOutro = survToken_WIN;
                    tokenFormat = "WIN_FORMAT_WIN";
                }
            }
            else if (gameEnd == MissedContent.GameEndings.EscapeSequenceFailed)
            {
                send = true;
                tokenOutro = survToken_VANISH;
                tokenFormat = "WIN_FORMAT_FAIL";
            }
            else if (gameEnd == RoR2Content.GameEndings.LimboEnding | gameEnd == RoR2Content.GameEndings.ObliterationEnding)
            {
                send = true;
                tokenOutro = survToken_VANISH;
                tokenFormat = "WIN_FORMAT_MS";
            }
            else if (gameEnd == DLC1Content.GameEndings.VoidEnding)
            {
                send = true;
                tokenOutro = survToken_VANISH;
                tokenFormat = "WIN_FORMAT_VOID";
            }
            if (send)
            {
                playerInfo.finalMessageToken = tokenOutro;
                Chat.AddMessage("   " + string.Format(Language.GetString(tokenFormat), Language.GetString(tokenOutro)));
            }

        }

        public static ItemIndex VanillaVoids_WatchBrokeItem = (ItemIndex)(-3);
        private static void TransformItem_Messages(On.RoR2.CharacterMasterNotificationQueue.orig_PushItemTransformNotification orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            /*WolfoMain.log.LogMessage(characterMaster + " | " +
                oldIndex + " | " +
                newIndex + " | " +
                transformationType);*/

            //Cannot get Watch / old Benthic item count because this is after those items were removed.

            orig(characterMaster, oldIndex, newIndex, transformationType);
            if (!characterMaster.playerCharacterMasterController)
            {
                return;
            }
            string player = Util.GetBestMasterName(characterMaster);
            //Any player message here
            ItemDef newDef = ItemCatalog.GetItemDef(newIndex);
            /*if (newDef.tier >= ItemTier.VoidTier1 && newDef.tier <= ItemTier.VoidBoss) 
            {
                if (characterMaster.TryGetComponent<PerPlayer_ExtraStatTracker>(out var Stats))
                {
            //This doesn't have the item amount, how fix?
                    Stats.itemsVoided++;
                }
            }*/
 
            if (transformationType == CharacterMasterNotificationQueue.TransformationType.Default)
            {
                if (WConfig.cfgMessagesRevive.Value)
                {
                    if (newIndex == RoR2Content.Items.ExtraLifeConsumed.itemIndex)
                    {
                        Chat.AddMessage(string.Format(Language.GetString("ITEM_REVIVE_MESSAGE"), player, Language.GetString("ITEM_EXTRALIFE_NAME")));
                    }
                    else if (newIndex == DLC1Content.Items.ExtraLifeVoidConsumed.itemIndex)
                    {
                        Chat.AddMessage(string.Format(Language.GetString("ITEM_REVIVE_MESSAGE"), player, Language.GetString("ITEM_EXTRALIFEVOID_NAME")));
                    }

                }
                if (WConfig.cfgMessageElixir.Value != WConfig.MessageWho.Off)
                {
                    string authString = characterMaster.hasAuthority ? "" : "_1P";
                    if (characterMaster.hasAuthority == false && WConfig.cfgMessageElixir.Value != WConfig.MessageWho.Anybody)
                    {
                        return;
                    }
                    if (newIndex == DLC1Content.Items.HealingPotionConsumed.itemIndex)
                    {
                        Chat.AddMessage(Language.GetString("ITEM_USE_ELIXIR" + authString));
                    }
                    else if (newIndex == DLC1Content.Items.FragileDamageBonusConsumed.itemIndex)
                    {
                        Chat.AddMessage(Language.GetString("ITEM_USE_WATCH" + authString));
                    }
                    else if (newIndex == VanillaVoids_WatchBrokeItem)
                    {
                        string result = string.Format(Language.GetString("ITEM_USE_VV_VOIDWATCH" + authString), Help.GetColoredName(oldIndex));
                        Chat.AddMessage(result);
                    }
                }
            }
            else if (transformationType == CharacterMasterNotificationQueue.TransformationType.LunarSun)
            {
                if (WConfig.cfgMessageVoidTransform.Value)
                {
                    string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                    string name = Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken);
                    string ego = Language.GetString("ITEM_LUNARSUN_NAME");
                    string token = Language.GetString("ITEM_TRANSFORM_EGO");
                    string result = string.Format(token, ego, hex, name);
                    Chat.AddMessage(result);
                }
            }
            else if (transformationType == CharacterMasterNotificationQueue.TransformationType.CloverVoid)
            {
                if (WConfig.cfgMessageVoidTransform.Value)
                {
                    if (newDef.tier == ItemTier.NoTier)
                    {
                        return;
                    }
                    string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                    string hex2 = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(newIndex).pickupDef.baseColor);
                    string name = Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken) + "</color>";
                    string name2 = Language.GetString(newDef.nameToken) + "</color>";
                    int newitemcount = characterMaster.inventory.GetItemCount(newIndex);
                    if (newitemcount > 1)
                    {
                        name2 += "(" + newitemcount + ")";
                    }
                    string token = Language.GetString("ITEM_TRANSFORM_VOID");
                    string result = string.Format(token, hex, name, hex2, name2);
                    characterMaster.StartCoroutine(Courtines.Delayed_ChatAddMessage(result, 1.1f));
                }
            }

        }

        private static void TransformEquipment_Messages(On.RoR2.CharacterMasterNotificationQueue.orig_PushEquipmentTransformNotification orig, CharacterMaster characterMaster, EquipmentIndex oldIndex, EquipmentIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            /*WolfoMain.log.LogMessage(characterMaster + " | " +
                  oldIndex + " | " +
                  newIndex + " | " +
                  transformationType);*/

            orig(characterMaster, oldIndex, newIndex, transformationType);
            if (!characterMaster.playerCharacterMasterController)
            {
                return;
            }
            //Any player message here
            if (transformationType == CharacterMasterNotificationQueue.TransformationType.Default)
            {
                if (WConfig.cfgMessagesRevive.Value)
                {
                    if (newIndex == DLC2Content.Equipment.HealAndReviveConsumed.equipmentIndex)
                    {
                        string player = Util.GetBestMasterName(characterMaster);
                        if (characterMaster.lostBodyToDeath)
                        {
                            Chat.AddMessage(string.Format(Language.GetString("ITEM_REVIVE_MESSAGE"), player, Language.GetString("EQUIPMENT_HEALANDREVIVE_NAME")));
                        }
                        else
                        {
                            Chat.AddMessage(string.Format(Language.GetString("ITEM_REVIVE_MESSAGE_TEAM"), player, Language.GetString("EQUIPMENT_HEALANDREVIVE_NAME")));
                        }
                    }
                }
            }
            if (!characterMaster.hasAuthority)
            {
                return;
            }
        }



    }
    public class SaleStarMessage : SubjectChatMessage
    {
        public string interactableToken;
        public override string ConstructChatString()
        {
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
            if (WolfoMain.NoHostInfo == true)
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
            token1 = Language.GetString(token1);
            token2 = Language.GetString(token2);

            return string.Format(Language.GetString("ITEM_RECYCLED_GLOBAL"), token1, token2, hex);
        }

    }



}

