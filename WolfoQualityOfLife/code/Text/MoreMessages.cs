using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQualityOfLife
{

    public class MoreMessages
    {
        //public static string DetailedDeathString = "UNASSIGNED";
        //public static string DetailedDeathString_P2 = "UNASSIGNED_P2";
        public static MusicTrackDef CreditsTrack = Addressables.LoadAssetAsync<MusicTrackDef>(key: "RoR2/Base/Common/muSong21.asset").WaitForCompletion();
        public static MusicTrackDef CreditsTrackVoid = Addressables.LoadAssetAsync<MusicTrackDef>(key: "RoR2/DLC1/Common/muMenuDLC1.asset").WaitForCompletion();
        public static GameObject CreditsPanel = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/CreditsPanel.prefab").WaitForCompletion();

        public static void Start()
        {
            if (WConfig.cfgMessageDeath.Value == true)
            {
                On.RoR2.GlobalEventManager.OnPlayerCharacterDeath += DetailedDeathMessages;
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
                WinVanishingMessages();
            }
            if (WConfig.cfgMessagesColoredItemPings.Value)
            {
                ColoredItemNamesPings();
            }
        }

        private static void ScrappingMessage(On.RoR2.ScrapperController.orig_BeginScrapping orig, ScrapperController self, int intPickupIndex)
        {
            orig(self, intPickupIndex);

            if (self.interactor)
            {
                CharacterBody component = self.interactor.GetComponent<CharacterBody>();
                PickupDef pickupDef = PickupCatalog.GetPickupDef(new PickupIndex(intPickupIndex));
                if (component && component.inventory && pickupDef != null)
                {
                    string hex = ColorUtility.ToHtmlStringRGB(pickupDef.baseColor);
                    string nameToken = Language.GetString(ItemCatalog.GetItemDef(pickupDef.itemIndex).nameToken, "en");

                    string message = "<style=cEvent>You scrapped <color=#" + hex + ">"+ nameToken + "</color>";
                    string message2P = "<style=cEvent>"+component.GetUserName()+ " scrapped <color=#" + hex + ">"+ nameToken + "</color>";

                    if (self.itemsEaten > 1)
                    {
                        message += "("+ self.itemsEaten + ")";
                        message2P += "(" + self.itemsEaten + ")";
                    }

                    message += "</style>";
                    message2P += "</style>";

                    Chat.SendBroadcastChat(new FakeSubjectMessage
                    {
                        baseToken = message,
                        secondPersonToken = message2P,
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
                    string token = "<style=cEvent>You drank a <color=#FFFFFF>Power Elixir</color></style>";
                    Chat.AddMessage(token);
                }
                else if (newIndex == DLC1Content.Items.FragileDamageBonusConsumed.itemIndex)
                {
                    string token = "<style=cEvent>You broke your <color=#FFFFFF>Delicate Watches</color></style>";
                    Chat.AddMessage(token);
                }
                else if (ItemCatalog.GetItemDef(newIndex).name.EndsWith("BROKEN_MESS"))
                {
                    string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                    hex = "<color=#" + hex + ">" + Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken) + "</color>";
                    string token = "<style=cEvent>"+ hex + " ran out of time...</style>";
                    Chat.AddMessage(token);
                }
            }
        }

        public static void WinVanishingMessages()
        {
            //Victory Vanishing messages
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
                            SurvivorDef tempsurv = SurvivorCatalog.FindSurvivorDefFromBody(playerController.master.bodyPrefab);
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
                        else if (gameEnd.cachedName.Equals("InfiniteTowerEnding"))
                        {

                        }
                    }
                }
            };

            On.EntityStates.GameOver.VoidEndingFadeToBlack.OnExit += (orig, self) =>
            {
                orig(self);
                CreditsPanel.transform.GetChild(4).GetComponent<MusicTrackOverride>().track = CreditsTrackVoid;

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
                CreditsPanel.transform.GetChild(4).GetComponent<MusicTrackOverride>().track = CreditsTrack;
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
                        bool isFirstPerson = IsFirstPerson(interactor.gameObject.GetComponent<CharacterBody>().master.playerCharacterMasterController.networkUser);
                        string message = "";
                        string message2P = "";

                        message += "You";
                        message2P += interactor.GetComponent<CharacterBody>().GetUserName();


                        if (purchase.gameObject.name.StartsWith("LunarCauldron"))
                        {
                            message = "<style=cEvent>" + message;
                            message += " reforged ";

                            message2P = "<style=cEvent>" + message2P;
                            message2P += " reforged ";
                        }
                        else if (purchase.gameObject.name.StartsWith("ShrineCleanse"))
                        {
                            message = "<style=cShrine>" + message;
                            message += " have cleansed yourself of ";

                            message2P = "<style=cShrine>" + message2P;
                            message2P += " has cleansed themselves of ";
                        }
                        else
                        {
                            message = "<style=cEvent>" + message;
                            message += " got rid of ";

                            message2P = "<style=cEvent>" + message2P;
                            message2P += " got rid of ";
                        }
                        string hex;
                        if (payResults.itemsTaken.Count > 0)
                        {
                            if (payResults.itemsTaken.Count == 1)
                            {
                                hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(payResults.itemsTaken[0]).pickupDef.baseColor);
                                message += "<color=#" + hex + ">" + Language.GetString(ItemCatalog.GetItemDef(payResults.itemsTaken[0]).nameToken, "en") + " </color>";
                                message2P += "<color=#" + hex + ">" + Language.GetString(ItemCatalog.GetItemDef(payResults.itemsTaken[0]).nameToken, "en") + " </color>";
                            }
                            else
                            {
                                string name;
                                List<ItemIndex> tempList = new List<ItemIndex>(payResults.itemsTaken);

                                for (int i = 0; i < payResults.itemsTaken.Count; i++)
                                {
                                    string countS = "";
                                    int count = 0;
                                    name = Language.GetString(ItemCatalog.GetItemDef(payResults.itemsTaken[i]).nameToken, "en");
                                    hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(payResults.itemsTaken[i]).pickupDef.baseColor);

                                    for (int z = 0; z < tempList.Count; z++)
                                    {
                                        //Debug.Log("CheckingChecking " + payResults.itemsTaken[i] + " / " + tempList[z]);
                                        if (payResults.itemsTaken[i] == tempList[z])
                                        {
                                            count++;
                                            tempList.Remove(tempList[z]);
                                            z--;
                                        }
                                    }
                                    if (count > 0)
                                    {
                                        if (count > 1)
                                        {
                                            countS = count.ToString() + "x ";
                                        }
                                        if (i > 0)
                                        {
                                            message += ", ";
                                            message2P += ", ";
                                        }

                                        message += countS + "<color=#" + hex + ">" + name + "</color>";
                                        message2P += countS + "<color=#" + hex + ">" + name + "</color>";
                                    }
                                }
                            }
                        }
                        else if (payResults.equipmentTaken.Count > 0)
                        {
                            hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(payResults.equipmentTaken[0]).pickupDef.baseColor);
                            message += "<color=#" + hex + ">" + Language.GetString(EquipmentCatalog.GetEquipmentDef(payResults.equipmentTaken[0]).nameToken, "en") + " </color>";
                            message2P += "<color=#" + hex + ">" + Language.GetString(EquipmentCatalog.GetEquipmentDef(payResults.equipmentTaken[0]).nameToken, "en") + " </color>";
                        }
                        message += "</style>";
                        message2P += "</style>";

                        Chat.SendBroadcastChat(new FakeSubjectMessage
                        {
                            baseToken = message,
                            secondPersonToken = message2P,
                            subjectAsNetworkUser = interactor.gameObject.GetComponent<CharacterBody>().master.playerCharacterMasterController.networkUser,
                        });
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

        private static void DetailedDeathMessages(On.RoR2.GlobalEventManager.orig_OnPlayerCharacterDeath orig, GlobalEventManager self, DamageReport damageReport, NetworkUser victimNetworkUser)
        {
            if (!victimNetworkUser || damageReport.victimBody == null || damageReport.damageInfo == null)
            {
                orig(self, damageReport, victimNetworkUser);
                return;
            }


            string KillerName = "Unknown Killer";
            string VictimName = RoR2.Util.GetBestBodyName(damageReport.victimBody.gameObject);
            if (damageReport.attackerBody != null)
            {
                KillerName = RoR2.Util.GetBestBodyName(damageReport.attacker);
                KillerName = KillerName.Replace("\n", " ");
            }
            float DamageValue = damageReport.damageInfo.damage;


            Debug.Log(VictimName);
            Debug.Log(KillerName);
            //Debug.LogWarning(DamageValue);
            //Debug.LogWarning(damageReport.damageInfo.damageType);

            string tokenYou = $"<style=cDeath>";
            string token = $"<style=cDeath>";

            if (damageReport.dotType != DotController.DotIndex.None && damageReport.isFriendlyFire == false)
            {
                //Voidtouched Dot Death
                if (damageReport.dotType == DotController.DotIndex.Fracture)
                {
                    if (damageReport.attackerBody != null)
                    {
                        tokenYou += $"You were collapsed by {KillerName}.";
                        token += $"{VictimName} was collapsed by {KillerName}.";
                    }
                    else
                    {
                        tokenYou += $"You were collapsed by someone unknown.";
                        token += $"{VictimName} was collapsed by someone unknown.";
                    }
                }
                else
                {
                    //General death to Dot such as Bleed/Burn
                    if (damageReport.attackerBody != null)
                    {
                        tokenYou += $"You slowly died to {KillerName}.";
                        token += $"{VictimName} slowly died to {KillerName}.";
                    }
                    else
                    {
                        tokenYou += $"You slowly died to unknown causes.";
                        token += $"{VictimName} slowly died to unknown causes.";
                    }
                }
            }
            else if (damageReport.damageInfo.damageType.HasFlag(DamageType.BypassArmor | DamageType.BypassBlock) && damageReport.damageInfo.damageColorIndex == DamageColorIndex.Void)
            {
                //Simu Void Death maybe other Voids too
                tokenYou += $"You drowned in the void.";
                token += $"{VictimName} drowned in the void.";
            }
            else if (damageReport.damageInfo.damageType.HasFlag(DamageType.VoidDeath))
            {
                tokenYou += $"You were detained by {KillerName}</style>";
                token += $"{VictimName} was detained by {KillerName}</style>";
            }
            else if (damageReport.isFallDamage)
            {
                tokenYou += $"You succumbed to gravity.";
                token += $"{VictimName} succumbed to gravity.";
            }
            else if (damageReport.attackerBody != null && damageReport.isFriendlyFire)
            {
                if (VictimName.Equals(KillerName))
                {
                    tokenYou += $"You were killed by yourself.";
                    token += $"{VictimName} was killed by themselves.";
                }
                else
                {
                    tokenYou += $"You were betrayed by {KillerName}";
                    token += $"{VictimName} was betrayed by {KillerName}";
                }
            }
            else if (damageReport.attackerBody != null)
            {
                tokenYou += $"You were killed by {KillerName}.";
                token += $"{VictimName} was killed by {KillerName}.";
            }
            else
            {
                tokenYou += $"You were killed by the planet.";
                token += $"{VictimName} was killed by the planet.";
            }
            if (!damageReport.damageInfo.damageType.HasFlag(DamageType.VoidDeath))
            {
                tokenYou += $" ({DamageValue:F2} damage taken)</style>";
                token += $" ({DamageValue:F2} damage taken)</style>";
            }
            

            //DetailedDeathString = tokenYou;
            //DetailedDeathString_P2 = token;
            Chat.SendBroadcastChat(new FakeSubjectMessage
            {
                baseToken = tokenYou,
                secondPersonToken = token,
                subjectAsNetworkUser = victimNetworkUser,
            });

            orig(self, damageReport, victimNetworkUser);
        }

        private static void VoidLunarTransformMessages(On.RoR2.CharacterMasterNotificationQueue.orig_PushItemTransformNotification orig, CharacterMaster characterMaster, ItemIndex oldIndex, ItemIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            orig(characterMaster, oldIndex, newIndex, transformationType);
            //Debug.LogWarning(transformationType);
            if (transformationType == CharacterMasterNotificationQueue.TransformationType.LunarSun)
            {
                string hex = ColorUtility.ToHtmlStringRGB(PickupCatalog.FindPickupIndex(oldIndex).pickupDef.baseColor);
                string name = Language.GetString(ItemCatalog.GetItemDef(oldIndex).nameToken);
                //int count = characterMaster.inventory.GetItemCount(DLC1Content.Items.LunarSun);
                string token = "<style=cEvent><color=#458CFF>Egocentrism</color> assimilated <color=#" + hex + ">" + name + "</color></style>";
                Chat.AddMessage(token);
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
                /*if (!name2.EndsWith("s"))
                {
                    name2 += "s";
                }*/
                /*if (VoidCloverInventory == characterMaster.inventory && VoidCloverCount > 1)
                {
                    name += "(" + VoidCloverCount + ")";
                }*/
                if (newitemcount > 1)
                {
                    name2 += "(" + newitemcount + ")";
                }
                string token = "<style=cEvent><style=cIsVoid>The Void</style> upgraded <color=#" + hex + ">" + name + " into <color=#" + hex2 + ">" + name2 + "</style>";
                characterMaster.StartCoroutine(DelayedChatMessageNonGlobal(token, 0.5f));
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

        public static bool IsFirstPerson(NetworkUser networkUser)
        {
            return LocalUserManager.readOnlyLocalUsersList.Count == 1 && networkUser.isLocalPlayer;
        }

        public class FakeSubjectMessage : RoR2.SubjectChatMessage
        {
            public override string ConstructChatString()
            {
                //Debug.LogWarning("ConstructChatString");
                //Debug.LogWarning(baseToken);
                //Debug.LogWarning(secondPersonToken);
                if (!base.IsSecondPerson())
                {
                    return secondPersonToken;
                }
                return baseToken;
            }

            public string secondPersonToken;

            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(secondPersonToken);
                //Debug.LogWarning("Serialize " + writer);
            }

            public override void Deserialize(NetworkReader reader)
            {
                base.Deserialize(reader);
                secondPersonToken = reader.ReadString();
                //Debug.LogWarning("Deserialize " + reader);
            }
        }



    }





}

