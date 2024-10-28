using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQualityOfLife
{

    public class MoreMessages
    {
        //public static string DetailedDeathString = "UNASSIGNED";
        //public static string DetailedDeathString_P2 = "UNASSIGNED_P2";
        //public static MusicTrackDef CreditsTrack = Addressables.LoadAssetAsync<MusicTrackDef>(key: "RoR2/Base/Common/MusicTrackDefs/muSong21.asset").WaitForCompletion();
        //public static MusicTrackDef CreditsTrackVoid = Addressables.LoadAssetAsync<MusicTrackDef>(key: "RoR2/DLC1/Common/muMenuDLC1.asset").WaitForCompletion();
        //public static GameObject CreditsPanel = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/CreditsPanel.prefab").WaitForCompletion();
        private static GameEndingDef EscapeSequenceFailed = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/EscapeSequenceFailed.asset").WaitForCompletion();

        public static void Start()
        {
            if (WConfig.cfgMessageDeath.Value == true)
            {
                On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += DeathMessage.DetailedDeathMessages;
            }
            if (WConfig.cfgMessagePrint.Value == true)
            {
                IL.RoR2.PurchaseInteraction.OnInteractionBegin += ItemGiveUpMessages;
            }
            if (WConfig.cfgMessageScrap.Value == true)
            {
                On.RoR2.ScrapperController.BeginScrapping += ScrappingMessage;
            }
            if (WConfig.cfgMessageVoidTransform.Value == true)
            {
                On.RoR2.CharacterMasterNotificationQueue.PushItemTransformNotification += VoidLunarTransformMessages;
            }
            if (WConfig.cfgMessageElixir.Value)
            {
                On.RoR2.CharacterMasterNotificationQueue.PushItemTransformNotification += ElixirMessage2;
            }
            if (WConfig.cfgMessageVictory.Value)
            {
                On.RoR2.Run.BeginGameOver += WinVanishMessage;
                //WinVanishingMessages();
            }
            if (WConfig.cfgMessagesColoredItemPings.Value)
            {
                ColoredItemNamesPings();
            }
        }

        //Server only
        private static void ScrappingMessage(On.RoR2.ScrapperController.orig_BeginScrapping orig, ScrapperController self, int intPickupIndex)
        {
            orig(self, intPickupIndex);

            if (self.interactor)
            {
                CharacterBody component = self.interactor.GetComponent<CharacterBody>();
                PickupDef pickupDef = PickupCatalog.GetPickupDef(new PickupIndex(intPickupIndex));
                if (component && component.inventory && pickupDef != null)
                {
                    Chat.SendBroadcastChat(new ItemMessage
                    {
                        baseToken = "ITEM_LOSS_SCRAP",
                        itemCount = self.itemsEaten,
                        pickupIndex = new PickupIndex(intPickupIndex),
                        subjectAsNetworkUser = component.master.playerCharacterMasterController.networkUser,
                    });
                }
            }
        }

        internal static void ColoredItemNamesPings()
        {
            //Colors in Messages when pinging Items
            On.RoR2.GenericPickupController.GetDisplayName += (orig, self) =>
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
            };
            //Item Color in messages when pinging Shops
            IL.RoR2.UI.PingIndicator.RebuildPing += (ILContext il) =>
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
            };
        }


        private static void ElixirMessage2(On.RoR2.CharacterMasterNotificationQueue.orig_PushItemTransformNotification orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            //Can't get item counts here idk dude
            orig(characterMaster, oldIndex, newIndex, transformationType);

            if (transformationType == CharacterMasterNotificationQueue.TransformationType.Default)
            {
                if (newIndex == DLC1Content.Items.HealingPotionConsumed.itemIndex)
                {
                    //string result = "<style=cEvent>You drank a <color=#FFFFFF>Power Elixir</color></style>";
                    Chat.AddMessage(Language.GetString("ITEM_USE_ELIXIR"));
                }
                else if (newIndex == DLC1Content.Items.FragileDamageBonusConsumed.itemIndex)
                {
                    //string result = "<style=cEvent>You broke your <color=#FFFFFF>Delicate Watches</color></style>";
                    Chat.AddMessage(Language.GetString("ITEM_USE_WATCH"));
                }
                else if (ItemCatalog.GetItemDef(newIndex).name.EndsWith("BROKEN_MESS"))
                {
                    string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                    hex = "<color=#" + hex + ">" + Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken) + "</color>";
                    //string result = "<style=cEvent>" + hex + " ran out of time...</style>";
                    string result = string.Format(Language.GetString("ITEM_USE_VV_VOIDWATCH"), hex);

                    Chat.AddMessage(result);
                }
            }
        }

        public static void WinVanishingMessages()
        {
            //Victory Vanishing messages
            /*
            On.RoR2.Run.BeginGameOver += (orig, self, gameEnd) =>
            {
                orig(self, gameEnd);
                //Debug.LogWarning(gameEnd);
                if (gameEnd != RoR2Content.GameEndings.StandardLoss)
                {
                    foreach (var playerController in PlayerCharacterMasterController.instances)
                    {
                        if (gameEnd == RoR2Content.GameEndings.LimboEnding | gameEnd == RoR2Content.GameEndings.ObliterationEnding)
                        {
                           
                            if (tempsurv && tempsurv.mainEndingEscapeFailureFlavorToken != null)
                            {
                                string token = "<style=cLunarObjective>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString(tempsurv.mainEndingEscapeFailureFlavorToken) + " <sprite name=\"CloudRight\" tint=1></style>";
                                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                            }
                            else
                            {
                                string token = "<style=cLunarObjective>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString("GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR") + " <sprite name=\"CloudRight\" tint=1></style>";
                                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                            }
                        }
                    }
                }
            };*/

            On.EntityStates.GameOver.VoidEndingFadeToBlack.OnExit += (orig, self) =>
            {
                orig(self);
                //CreditsPanel.transform.GetChild(4).GetComponent<MusicTrackOverride>().track = CreditsTrackVoid;

                foreach (var playerController in PlayerCharacterMasterController.instances)
                {
                    SurvivorDef tempsurv = SurvivorCatalog.FindSurvivorDefFromBody(playerController.master.bodyPrefab);
                    if (tempsurv && tempsurv.mainEndingEscapeFailureFlavorToken != null)
                    {
                        string token = "<style=cIsVoid>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString(tempsurv.mainEndingEscapeFailureFlavorToken) + " <sprite name=\"CloudRight\" tint=1></style>";
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                    }
                    else
                    {
                        string token = "<style=cIsVoid>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString("GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR") + " <sprite name=\"CloudRight\" tint=1></style>";
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                    }
                }
            };

            On.RoR2.OutroCutsceneController.OnEnable += (orig, self) =>
            {
                orig(self);
                //CreditsPanel.transform.GetChild(4).GetComponent<MusicTrackOverride>().track = CreditsTrack;
                foreach (var playerController in PlayerCharacterMasterController.instances)
                {
                    SurvivorDef tempsurv = SurvivorCatalog.FindSurvivorDefFromBody(playerController.master.bodyPrefab);

                    if (playerController.master.lostBodyToDeath)
                    {
                        if (tempsurv && tempsurv.mainEndingEscapeFailureFlavorToken != null)
                        {
                            string token = "<style=cDeath><sprite name=\"Skull\" tint=1> " + Language.GetString(tempsurv.mainEndingEscapeFailureFlavorToken) + " <sprite name=\"Skull\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                        }
                        else
                        {
                            string token = "<style=cDeath><sprite name=\"Skull\" tint=1> " + Language.GetString("GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR") + " <sprite name=\"Skull\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });

                        }
                    }
                    else
                    {
                        if (tempsurv && tempsurv.outroFlavorToken != null)
                        {
                            string token2 = "<style=cIsHealing>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString(tempsurv.outroFlavorToken) + " <sprite name=\"CloudRight\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token2 });
                        }
                        else
                        {
                            string token2 = "<style=cIsHealing>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString("GENERIC_OUTRO_FLAVOR") + " <sprite name=\"CloudRight\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token2 });
                        }
                    }
                }
            };
        }

        private static void WinVanishMessage(On.RoR2.Run.orig_BeginGameOver orig, Run self, GameEndingDef gameEnd)
        {
            orig(self, gameEnd);
            if (gameEnd == DLC2Content.GameEndings.RebirthEndingDef)
            {
                string token = "<color=#7CFE7C>   <sprite name=\"CloudLeft\" tint=1> " + Language.GetString("REBIRTH_ENDING_CHAT") + " <sprite name=\"CloudRight\" tint=1></color>";
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
            }
            else if (gameEnd != RoR2Content.GameEndings.StandardLoss)
            {
                foreach (var playerController in PlayerCharacterMasterController.instances)
                {
                    SurvivorDef tempsurv = SurvivorCatalog.FindSurvivorDefFromBody(playerController.master.bodyPrefab);
                    string survToken_WIN = "GENERIC_OUTRO_FLAVOR";
                    string survToken_VANISH = "GENERIC_MAIN_ENDING_ESCAPE_FAILURE_FLAVOR";
                    if (tempsurv && tempsurv.mainEndingEscapeFailureFlavorToken != null)
                    {
                        survToken_WIN = tempsurv.outroFlavorToken;
                        survToken_VANISH = tempsurv.mainEndingEscapeFailureFlavorToken;
                    }
                    survToken_WIN = Language.GetString(survToken_WIN);
                    survToken_VANISH = Language.GetString(survToken_VANISH);

                    if (gameEnd == RoR2Content.GameEndings.MainEnding)
                    {
                        if (playerController.master.lostBodyToDeath)
                        {
                            string token = "<style=cDeath><sprite name=\"Skull\" tint=1> " + survToken_VANISH + " <sprite name=\"Skull\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                        }
                        else
                        {
                            string token2 = "<style=cIsHealing>   <sprite name=\"CloudLeft\" tint=1> " + survToken_WIN + " <sprite name=\"CloudRight\" tint=1></style>";
                            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token2 });
                        }
                    }
                    else if (gameEnd == EscapeSequenceFailed)
                    {
                        string token2 = "<style=cIsHealing>   <sprite name=\"CloudLeft\" tint=1> " + survToken_VANISH + " <sprite name=\"CloudRight\" tint=1></style>";
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token2 });
                    }
                    else if (gameEnd == RoR2Content.GameEndings.LimboEnding | gameEnd == RoR2Content.GameEndings.ObliterationEnding)
                    {
                        string token = "<style=cLunarObjective>   <sprite name=\"CloudLeft\" tint=1> " + survToken_VANISH + " <sprite name=\"CloudRight\" tint=1></style>";
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                    }
                    else if (gameEnd == DLC1Content.GameEndings.VoidEnding)
                    {
                        string token = "<style=cIsVoid>   <sprite name=\"CloudLeft\" tint=1> " + survToken_VANISH + " <sprite name=\"CloudRight\" tint=1></style>";
                        Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = token });
                    }
                    
                }
            }
        }

        //Server only
        private static void ItemGiveUpMessages(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
                    x => x.MatchLdfld("RoR2.CostTypeDef/PayCostResults", "itemsTaken")))
            {
                //Debug.Log(c);
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<RoR2.CostTypeDef.PayCostResults, RoR2.PurchaseInteraction, RoR2.Interactor, RoR2.CostTypeDef.PayCostResults>>((payResults, purchase, interactor) =>
                {
                    if (payResults.itemsTaken.Count > 0 || payResults.equipmentTaken.Count > 0)
                    {

                        //Might need some saftey checks for this idk
                        string token = "";
                        string message = "";
                        string message2P = "";


                        string user = interactor.GetComponent<CharacterBody>().GetUserName();






                        if (purchase.gameObject.name.StartsWith("LunarCauldron"))
                        {
                            token = "ITEM_LOSS_CAULDRON";
                        }
                        else if (purchase.gameObject.name.StartsWith("ShrineCleanse"))
                        {
                            token = "ITEM_LOSS_CLEANSING";
                        }
                        else
                        {
                            token = "ITEM_LOSS_GENERIC";
                        }

                        if (payResults.equipmentTaken.Count > 0)
                        {
                            Chat.SendBroadcastChat(new ItemMessage
                            {
                                baseToken = token,
                                pickupIndex = PickupCatalog.FindPickupIndex(payResults.equipmentTaken[0]),
                                subjectAsNetworkUser = interactor.gameObject.GetComponent<CharacterBody>().master.playerCharacterMasterController.networkUser,
                            });
                        }
                        else if (payResults.itemsTaken.Count == 1)
                        {
                            Chat.SendBroadcastChat(new ItemMessage
                            {
                                baseToken = token,
                                pickupIndex = PickupCatalog.FindPickupIndex(payResults.itemsTaken[0]),
                                itemCount = 1,
                                subjectAsNetworkUser = interactor.gameObject.GetComponent<CharacterBody>().master.playerCharacterMasterController.networkUser,
                            });
                        }
                        else if (payResults.itemsTaken.Count > 1)
                        {
                            int[] itemStacks = ItemCatalog.RequestItemStackArray();
                            for (int i = 0; i < payResults.itemsTaken.Count; i++)
                            {
                                itemStacks[(int)payResults.itemsTaken[i]]++;
                            }
                            Chat.SendBroadcastChat(new ItemMessage
                            {
                                baseToken = token,
                                itemStacks = itemStacks,
                                subjectAsNetworkUser = interactor.gameObject.GetComponent<CharacterBody>().master.playerCharacterMasterController.networkUser,
                            });
                        }

                    }
                    return payResults;
                });
                //Debug.Log("IL Found: IL.RoR2.PurchaseInteraction.OnInteractionBegin");
            }
            else
            {
                Debug.LogWarning("IL Failed: IL.RoR2.PurchaseInteraction.OnInteractionBegin");
            }
        }

        private static void VoidLunarTransformMessages(On.RoR2.CharacterMasterNotificationQueue.orig_PushItemTransformNotification orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            orig(characterMaster, oldIndex, newIndex, transformationType);
            //Debug.LogWarning(transformationType);
            if (transformationType == CharacterMasterNotificationQueue.TransformationType.LunarSun)
            {
                string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                string name = Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken);
                string ego = Language.GetString("ITEM_LUNARSUN_NAME");
                string token = Language.GetString("ITEM_TRANSFORM_EGO");

                //string result = "<style=cEvent><color=#458CFF>Egocentrism</color> assimilated <color=#" + hex + ">" + name + "</color></style>";
                string result = string.Format(token, ego, hex, name);

                Chat.AddMessage(result);
            }
            else if (transformationType == CharacterMasterNotificationQueue.TransformationType.CloverVoid)
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
                //string result = "<style=cEvent><style=cIsVoid>The Void</style> upgraded <color=#" + hex + ">" + name + " into <color=#" + hex2 + ">" + name2 + "</style>";
                string result = string.Format(token, hex, name, hex2, name2);

                characterMaster.StartCoroutine(DelayedChatMessageNonGlobal(result, 0.5f));
            }
        }

        private static void ElixirWatchMessages(On.RoR2.HealthComponent.orig_UpdateLastHitTime orig, HealthComponent self, float damageValue, Vector3 damagePosition, bool damageIsSilent, GameObject attacker)
        {
            int watchcount = 0;
            bool elixirdunk = false;
            if (damageValue > 0f && self.body && self.body.isPlayerControlled && self.isHealthLow)
            {
                watchcount += self.itemCounts.fragileDamageBonus;
                if (self.itemCounts.healingPotion > 0)
                {
                    elixirdunk = true;
                }
            }
            orig(self, damageValue, damagePosition, damageIsSilent, attacker);

            if (elixirdunk)
            {
                string token = "<style=cEvent>" + Util.GetBestBodyName(self.body.gameObject) + " drank an <color=#FFFFFF>Elixir</color>";
                if (watchcount > 0 && self.itemCounts.fragileDamageBonus == 0)
                {
                    token += " and broke their <color=#FFFFFF>Delicate Watch";
                    if (watchcount > 1) { token += "es</color>(" + watchcount + ")"; }

                }
                token += "</style>";
                Chat.AddMessage(token);
            }
            else if (watchcount > 0 && self.itemCounts.fragileDamageBonus == 0)
            {
                string token = "<style=cEvent>" + Util.GetBestBodyName(self.body.gameObject) + " broke their <color=#FFFFFF>Delicate Watch";
                if (watchcount > 1) { token += "es</color>(" + watchcount + ")" + "</style>"; }
                Chat.AddMessage(token);
            }
        }

        public static System.Collections.IEnumerator DelayedChatMessageNonGlobal(string chatMessage, float Delay)
        {
            yield return new WaitForSeconds(Delay);
            Chat.AddMessage(chatMessage);
        }



        public class ItemMessage : RoR2.SubjectChatMessage
        {
            public override string ConstructChatString()
            {
                if (!base.IsSecondPerson())
                {
                    baseToken += "_P2";
                }
                string itemsLost = "";
                string hex = "";
                string nameToken = "";

                if (pickupIndex != PickupIndex.none)
                {
                    hex = ColorUtility.ToHtmlStringRGB(pickupIndex.pickupDef.baseColor);
                    if (pickupIndex.pickupDef.equipmentIndex != EquipmentIndex.None)
                    {
                        nameToken = Language.GetString(EquipmentCatalog.GetEquipmentDef(pickupIndex.pickupDef.equipmentIndex).nameToken);

                        itemsLost = "<color=#" + hex + ">" + nameToken + " </color>";
                    }
                    else if (pickupIndex.pickupDef.itemIndex != ItemIndex.None)
                    {
                        nameToken = Language.GetString(ItemCatalog.GetItemDef(pickupIndex.pickupDef.itemIndex).nameToken);

                        itemsLost = "<color=#" + hex + ">" + nameToken + "</color>";

                        if (this.itemCount > 1)
                        {
                            itemsLost += "(" + this.itemCount + ")";
                        }
                    }
                }
                else
                {
                    bool addedItem = false;
                    for (int i = 0; i < itemStacks.Length; i++)
                    {
                        if (itemStacks[i] > 0)
                        {
                            nameToken = Language.GetString(ItemCatalog.GetItemDef((ItemIndex)i).nameToken);
                            hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex((ItemIndex)i).pickupDef.baseColor);

                            if (addedItem == true)
                            {
                                itemsLost += ", ";
                            }
                            addedItem = true;
                            if (itemStacks[i] > 1)
                            {
                                itemsLost += itemStacks[i].ToString() + "x ";
                            }
                            itemsLost += "<color=#" + hex + ">" + nameToken + "</color>";
                        }
                    }
                }



                string result = string.Format(Language.GetString(baseToken), this.subjectAsCharacterBody.GetDisplayName(), itemsLost);
                return result;
            }


            public int itemCount;
            public PickupIndex pickupIndex = PickupIndex.none;
            public int[] itemStacks = ItemCatalog.RequestItemStackArray();

            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(itemCount);
                writer.Write(pickupIndex);
                if (pickupIndex == PickupIndex.none)
                {
                    writer.WriteItemStacks(itemStacks);
                }

            }

            public override void Deserialize(NetworkReader reader)
            {
                base.Deserialize(reader);
                itemCount = reader.ReadInt32();
                pickupIndex = reader.ReadPickupIndex();
                if (pickupIndex == PickupIndex.none)
                {
                    reader.ReadItemStacks(itemStacks);
                }
                //Debug.LogWarning("Deserialize " + reader);
            }
        }



    }





}

