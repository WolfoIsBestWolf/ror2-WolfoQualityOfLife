using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static RoR2.CharacterMasterNotificationQueue;

namespace WolfoQoL_Client
{

    public class MoreMessages
    {
        private static GameEndingDef EscapeSequenceFailed = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/EscapeSequenceFailed.asset").WaitForCompletion();

        public static void Start()
        {
            On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += DeathMessage.OnDeathMessage;
            ItemLoss_Host.Start();
            LunarSeer.Start();
            EquipmentDrone.Start();


            On.RoR2.Run.OnClientGameOver += WinMessage_Client;

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
                Debug.LogWarning("IL Failed : AttemptToFixClients");
            }
        }

        private static void WinMessage_Client(On.RoR2.Run.orig_OnClientGameOver orig, Run self, RunReport runReport)
        {
            orig(self, runReport);
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
            string temp = orig(self);
            PickupDef pickupDef = PickupCatalog.GetPickupDef(self.pickupIndex);
            if (pickupDef != null)
            {
                string hex = ColorUtility.ToHtmlStringRGB(pickupDef.baseColor);
                temp = "<color=#" + hex + ">" + temp + "</color>";
                return temp;
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
                //Debug.Log(c +"  Next:"+ c.Next.Operand);
                var A = c.Next.Operand;
                c.Index += 27; //UnIdeal
                               //Debug.Log(c + "  Next:" + c.Next.Operand);
                c.Emit(OpCodes.Ldloc_S, A);
                c.EmitDelegate<Func<string, PickupIndex, string>>((text, pickupIndex) =>
                {
                    PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                    //Debug.Log(pickupDef);
                    if (pickupDef != null)
                    {
                        string hex = ColorUtility.ToHtmlStringRGB(pickupDef.baseColor);
                        //Debug.Log(text);
                        text = text.Replace("(", "(<color=#" + hex + ">");
                        text = text.Replace(")", "</color>)");
                        //Debug.Log(text);
                    }
                    return text;
                });
                //Debug.Log("IL Found : IL.RoR2.UI.PingIndicator.RebuildPing");
            }
            else
            {
                Debug.LogWarning("IL Failed : IL.RoR2.UI.PingIndicator.RebuildPing");
            }
        }



        private static void WinMessageMethod(GameEndingDef gameEnd, RunReport.PlayerInfo playerInfo)
        {
            BodyIndex bodyIndex = playerInfo.bodyIndex;
            bool dead = playerInfo.isDead;
            bool send = false;
            string tokenFormat = "";
            string tokenOutro = "";
            string survToken_WIN = "GENERIC_OUTRO_FLAVOR";
            string survToken_VANISH = "GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR";
            SurvivorDef tempsurv = SurvivorCatalog.GetSurvivorDef(SurvivorCatalog.GetSurvivorIndexFromBodyIndex(bodyIndex));
            if (tempsurv && tempsurv.mainEndingEscapeFailureFlavorToken != null)
            {
                survToken_WIN = tempsurv.outroFlavorToken;
                survToken_VANISH = tempsurv.mainEndingEscapeFailureFlavorToken;
            }

            if (gameEnd == RoR2Content.GameEndings.MainEnding)
            {
                send = true;
                if (dead)
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
            else if (gameEnd == EscapeSequenceFailed)
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
                Chat.AddMessage("   " + string.Format(Language.GetString(tokenFormat), Language.GetString(tokenOutro)));
            }

        }

        public static ItemIndex VannilaVoids_WatchBrokeItem = ItemIndex.None;
        private static void TransformItem_Messages(On.RoR2.CharacterMasterNotificationQueue.orig_PushItemTransformNotification orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            /*Debug.Log(characterMaster + " | " +
                oldIndex + " | " +
                newIndex + " | " +
                transformationType);*/

            orig(characterMaster, oldIndex, newIndex, transformationType);
            if (!characterMaster.playerCharacterMasterController)
            {
                return;
            }
            //Any player message here
            if (transformationType == TransformationType.Default)
            {
                if (WConfig.cfgMessagesRevive.Value)
                {
                    if (newIndex == RoR2Content.Items.ExtraLifeConsumed.itemIndex)
                    {
                        string player = Util.GetBestMasterName(characterMaster);
                        Chat.AddMessage(string.Format(Language.GetString("ITEM_REVIVE_MESSAGE"), player, Language.GetString("ITEM_EXTRALIFE_NAME")));
                    }
                    else if (newIndex == DLC1Content.Items.ExtraLifeVoidConsumed.itemIndex)
                    {
                        string player = Util.GetBestMasterName(characterMaster);
                        Chat.AddMessage(string.Format(Language.GetString("ITEM_REVIVE_MESSAGE"), player, Language.GetString("ITEM_EXTRALIFEVOID_NAME")));
                    }

                }
            }
            if (!characterMaster.hasAuthority)
            {
                return;
            }
            if (transformationType == TransformationType.Default)
            {
                if (WConfig.cfgMessageElixir.Value)
                {
                    if (newIndex == DLC1Content.Items.HealingPotionConsumed.itemIndex)
                    {
                        Chat.AddMessage(Language.GetString("ITEM_USE_ELIXIR"));
                    }
                    else if (newIndex == DLC1Content.Items.FragileDamageBonusConsumed.itemIndex)
                    {
                        Chat.AddMessage(Language.GetString("ITEM_USE_WATCH"));
                    }
                    else if (ItemCatalog.GetItemDef(newIndex).name.EndsWith("BROKEN_MESS"))
                    {
                        string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                        hex = "<color=#" + hex + ">" + Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken) + "</color>";
                        string result = string.Format(Language.GetString("ITEM_USE_VV_VOIDWATCH"), hex);
                        Chat.AddMessage(result);
                    }
                }
            }
            else if (transformationType == TransformationType.LunarSun)
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
            else if (transformationType == TransformationType.CloverVoid)
            {
                if (WConfig.cfgMessageVoidTransform.Value)
                {
                    if (ItemCatalog.GetItemDef(newIndex).tier == ItemTier.NoTier)
                    {
                        return;
                    }
                    string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                    string hex2 = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(newIndex).pickupDef.baseColor);
                    string name = Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken) + "</color>";
                    string name2 = Language.GetString(ItemCatalog.GetItemDef(newIndex).nameToken) + "</color>";
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

        private static void TransformEquipment_Messages(On.RoR2.CharacterMasterNotificationQueue.orig_PushEquipmentTransformNotification orig, CharacterMaster characterMaster, EquipmentIndex oldIndex, EquipmentIndex newIndex, TransformationType transformationType)
        {
            /*Debug.Log(characterMaster + " | " +
                  oldIndex + " | " +
                  newIndex + " | " +
                  transformationType);*/

            orig(characterMaster, oldIndex, newIndex, transformationType);
            if (!characterMaster.playerCharacterMasterController)
            {
                return;
            }
            //Any player message here
            if (transformationType == TransformationType.Default)
            {
                if (WConfig.cfgMessagesRevive.Value)
                {
                    if (newIndex == DLC2Content.Equipment.HealAndReviveConsumed.equipmentIndex)
                    {
                        string player = Util.GetBestMasterName(characterMaster);
                        if (characterMaster.GetBody() == null)
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





}

