using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace WolfoQualityOfLife
{
    public class RandomMiscWithConfig
    {
        public static GameObject SprintingCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/SprintingCrosshair.prefab").WaitForCompletion();
        public static GameObject LoaderCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Loader/LoaderCrosshair.prefab").WaitForCompletion();


        public static void Start()
        {
            if (WConfig.cfgDelayGreenOrb.Value)
            {
                IL.RoR2.PortalSpawner.Start += DelayThunderMessage;
            }

            if (WConfig.cfgTpIconDiscoveredRed.Value)
            {
                IL.RoR2.UI.ChargeIndicatorController.Update += TeleporterDiscoveredRed;
            }


            if (WConfig.cfgVoidAllyCyanEyes.Value)
            {
                Material voidAlly = Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Nullifier/matNullifierAlly.mat").WaitForCompletion();
                voidAlly.SetColor("_EmColor", new Color(0, 3, 4, 1)); //1,1,1
                voidAlly = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC1/VoidJailer/matVoidJailerEyesAlly.mat").WaitForCompletion();
                voidAlly.SetColor("_EmColor", new Color(0f, 1f, 1.33f, 1)); //0.8706 0.4764 1 1
                voidAlly = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC1/VoidMegaCrab/matVoidMegaCrabAlly.mat").WaitForCompletion();
                voidAlly.SetColor("_EmColor", new Color(0f, 1.5f, 2.5f, 1f)); //0.7306 0 0.8208 1
            }

            if (WConfig.cfgMountainStacks.Value == true)
            {
                //Goofy, rather compact and did it before RoRR
                //On.RoR2.TeleporterInteraction.AddShrineStack += MultipleMountainSymbols;
                On.RoR2.TeleporterInteraction.AddShrineStack += (orig, self) =>
                {
                    orig(self);
                    //This is Host to Client, Client to Host
                    Debug.Log("On.RoR2.TeleporterInteraction.AddShrineStack");
                    RoR2.Chat.SendBroadcastChat(new SendExtraMountainIcon
                    {
                        teleporterObject = self.gameObject,
                        shrineStacks = self.shrineBonusStacks
                    });
                };
            }

            On.RoR2.UI.ChatBox.Awake += AllowMoreChatMessages;

            On.RoR2.UI.PingIndicator.RebuildPing += (orig, self) =>
            {
                orig(self);
                self.fixedTimer *= 2;
                //self.fixedTimer *= WConfig.cfgPingDurationMultiplier.Value;
            };


            if (WConfig.cfgNewSprintCrosshair.Value == true)
            {
                On.RoR2.UI.CrosshairManager.OnEnable += (orig, self) =>
                {
                    //Debug.Log(" On.RoR2.UI.CrosshairManager.OnEnable " + self);
                    orig(self);

                    GameObject RightBracket = Object.Instantiate(LoaderCrosshair.transform.GetChild(2).gameObject);
                    GameObject LeftBracket = Object.Instantiate(LoaderCrosshair.transform.GetChild(3).gameObject);

                    RoR2.UI.CrosshairController SprintingCrosshairUI = SprintingCrosshair.GetComponent<RoR2.UI.CrosshairController>();

                    if (SprintingCrosshair.transform.childCount > 0)
                    {
                        Object.Destroy(SprintingCrosshair.transform.GetChild(1).gameObject);
                        Object.Destroy(SprintingCrosshair.transform.GetChild(0).gameObject);
                    }

                    RightBracket.transform.SetParent(SprintingCrosshair.transform);
                    LeftBracket.transform.SetParent(SprintingCrosshair.transform);

                    SprintingCrosshairUI.spriteSpreadPositions[0].target = LeftBracket.GetComponent<RectTransform>();
                    SprintingCrosshairUI.spriteSpreadPositions[1].target = RightBracket.GetComponent<RectTransform>();
                };

            }
            //Equipment Drone Equipment Name
            if (WConfig.cfgEquipmentDroneName.Value == true)
            {
                GameObject EquipmentDrone = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EquipmentDroneBody");
                EquipmentDrone.AddComponent<EquipmentDroneNameComponent>();

                On.RoR2.CharacterMasterNotificationQueue.SendTransformNotification_CharacterMaster_EquipmentIndex_EquipmentIndex_TransformationType += CharacterMasterNotificationQueue_SendTransformNotification_CharacterMaster_EquipmentIndex_EquipmentIndex_TransformationType;
                //On.RoR2.Artifacts.EnigmaArtifactManager.OnServerEquipmentActivated += Enigma_EquipmentDroneName;
            }

            /*
            GameObject mdlGup = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody").transform.GetChild(0).GetChild(0).gameObject;
            GameObject mdlGeep = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GeepBody").transform.GetChild(0).GetChild(0).gameObject;
            GameObject mdlGip = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GipBody").transform.GetChild(0).GetChild(0).gameObject;

            mdlGeep.AddComponent<ModelPanelParameters>();
            mdlGip.AddComponent<ModelPanelParameters>();
            */
        }

        private static void DelayThunderMessage(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("RoR2.PortalSpawner", "spawnPreviewMessageToken")))
            {
                c.EmitDelegate<System.Func<string, string>>((target) =>
                {
                    if (ClassicStageInfo.instance && !string.IsNullOrEmpty(target))
                    {
                        //Debug.Log(target);
                        //ClassicStageInfo.instance.StartCoroutine("BroadcastFamilySelection", target);
                        ClassicStageInfo.instance.StartCoroutine(DelayedBroadCast(target));
                        return null;
                    }
                    else
                    {
                        //DelayedBroadCast(target);
                    }
                    return target;
                });
                Debug.Log("IL Found: Delay Thunder Message");
            }
            else
            {
                Debug.LogWarning("IL Failed: Delay Thunder Message");
            }
        }
        public static System.Collections.IEnumerator DelayedBroadCast(string familySelectionChatString)
        {
            yield return new WaitForSeconds(1f);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage
            {
                baseToken = familySelectionChatString
            });
            yield break;
        }

        private static void Enigma_EquipmentDroneName(On.RoR2.Artifacts.EnigmaArtifactManager.orig_OnServerEquipmentActivated orig, EquipmentSlot equipmentSlot, EquipmentIndex equipmentIndex)
        {
            orig(equipmentSlot, equipmentIndex);
            if (equipmentSlot.characterBody)
            {
                if (equipmentSlot.characterBody.name.StartsWith("EquipmentDr"))
                {
                    equipmentSlot.characterBody.gameObject.AddComponent<EquipmentDroneNameComponent>();
                }
            }
        }

        private static void CharacterMasterNotificationQueue_SendTransformNotification_CharacterMaster_EquipmentIndex_EquipmentIndex_TransformationType(On.RoR2.CharacterMasterNotificationQueue.orig_SendTransformNotification_CharacterMaster_EquipmentIndex_EquipmentIndex_TransformationType orig, CharacterMaster characterMaster, EquipmentIndex oldIndex, EquipmentIndex newIndex, CharacterMasterNotificationQueue.TransformationType transformationType)
        {
            orig(characterMaster, oldIndex, newIndex, transformationType);
            if (characterMaster.name.StartsWith("EquipmentDr"))
            {
                if (characterMaster.GetBody())
                {
                    characterMaster.GetBody().gameObject.AddComponent<EquipmentDroneNameComponent>();
                }
            }
        }

        private static void TeleporterDiscoveredRed(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            c.TryGotoNext(MoveType.Before,
            x => x.MatchLdfld("RoR2.UI.ChargeIndicatorController", "isDiscovered"));


            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdfld("RoR2.UI.ChargeIndicatorController", "spriteChargedColor")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<Color, RoR2.UI.ChargeIndicatorController, Color>>((value, charg) =>
                {
                    return charg.spriteFlashColor;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: TP RED DISCOVER CHANGE");
            }
        }

        public static void SprintUICallLate()
        {
            RoR2.UI.CrosshairController SprintingCrosshairUI = SprintingCrosshair.GetComponent<RoR2.UI.CrosshairController>();
            RandomMiscWithConfig.SprintingCrosshair.GetComponent<UnityEngine.UI.RawImage>().texture = RandomMiscWithConfig.LoaderCrosshair.GetComponent<UnityEngine.UI.RawImage>().texture;
            SprintingCrosshairUI.maxSpreadAngle = 11;
            SprintingCrosshairUI.spriteSpreadPositions = new RoR2.UI.CrosshairController.SpritePosition[]
            {
                new RoR2.UI.CrosshairController.SpritePosition
                {
                    onePosition = new Vector3(96,0,0),
                    target = null,
                    zeroPosition = new Vector3(24,0,0)
                },
                new RoR2.UI.CrosshairController.SpritePosition
                {
                    onePosition = new Vector3(-96,0,0),
                    target = null,
                    zeroPosition = new Vector3(-24,0,0)
                },
            };
        }

        private static void AllowMoreChatMessages(On.RoR2.UI.ChatBox.orig_Awake orig, RoR2.UI.ChatBox self)
        {
            orig(self);
            //Debug.LogWarning(self);
            if (self.name.StartsWith("ChatBox, In Run"))
            {
                bool HasDPSMeter = self.transform.parent.parent.parent.parent.Find("DPSMeterPanel");
                RectTransform ChatboxTranform = self.gameObject.GetComponent<RectTransform>();
                if (ChatboxTranform)
                {
                    ChatboxTranform.GetComponent<RoR2.UI.ChatBox>().allowExpandedChatbox = true;

                    RectTransform tempRectTransformInput = ChatboxTranform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
                    tempRectTransformInput.offsetMax = new Vector2(48, 48);
                    tempRectTransformInput.offsetMin = new Vector2(-8, 0);

                    RectTransform tempRectTransformExpanded = ChatboxTranform.GetChild(1).GetComponent<RectTransform>();
                    tempRectTransformExpanded.anchorMax = new Vector2(1, 1);
                    tempRectTransformExpanded.offsetMax = new Vector2(120, 0);
                    tempRectTransformExpanded.offsetMin = new Vector2(-10, 49);

                    RectTransform tempRectTransform = ChatboxTranform.GetChild(2).GetComponent<RectTransform>();
                    if (HasDPSMeter)
                    {
                        tempRectTransform.offsetMax = new Vector2(120, 0);
                        tempRectTransform.offsetMin = new Vector2(-10, 49);
                    }
                    else
                    {
                        tempRectTransform.offsetMax = new Vector2(120, -6);
                        tempRectTransform.offsetMin = new Vector2(-10, 0);
                    }
                }
            }
        }


        public class SendExtraMountainIcon : RoR2.ChatMessageBase
        {
            public override string ConstructChatString()
            {
                TeleporterInteraction teleporter = teleporterObject.GetComponent<TeleporterInteraction>();
                if (teleporter)
                {
                    Debug.Log("Sent ExtraMountainIcon");
                    if (shrineStacks == 1)
                    {
                        GameObject tempbossicon = teleporter.bossShrineIndicator;
                        GameObject tempbossiconclone = Object.Instantiate(tempbossicon, tempbossicon.transform);

                        Object.Destroy(tempbossicon.GetComponent<MeshRenderer>());
                        Object.Destroy(tempbossicon.GetComponent<Billboard>());
                        tempbossicon.transform.localPosition = new Vector3(0, 0, 6);
                        tempbossiconclone.transform.localScale = new Vector3(1, 1, 1);
                        tempbossiconclone.transform.localPosition = new Vector3(0, 0, 0);
                        tempbossiconclone.SetActive(true);
                    }
                    if (shrineStacks > 1)
                    {
                        GameObject tempbossicon = teleporter.bossShrineIndicator.transform.GetChild(0).gameObject;
                        GameObject tempbossiconclone = Object.Instantiate(tempbossicon, tempbossicon.transform.parent);
                        tempbossiconclone.transform.localPosition = new Vector3(0, (shrineStacks - 1), 0);
                    }
                }
                else
                {
                    Debug.LogWarning("Sent ExtraMountainIcon without Teleporter Object");
                }


                return null;
            }

            public int shrineStacks;
            public GameObject teleporterObject;


            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(shrineStacks);
                writer.Write(teleporterObject);

            }

            public override void Deserialize(NetworkReader reader)
            {
                base.Deserialize(reader);
                shrineStacks = reader.ReadInt32();
                teleporterObject = reader.ReadGameObject();
            }

        }

        public class EquipmentDroneNameComponent : MonoBehaviour
        {
            //Start for some reason refuses to work on Clients so I guess we'll just fucking run it until it works
            public CharacterBody body;
            private bool changed = false;
            private bool enigma = false;

            public void Start()
            {
                //Debug.LogWarning("Start");
                if (RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.enigmaArtifactDef))
                {
                    Destroy(this);
                    enigma = true;
                };
                body = this.GetComponent<CharacterBody>();
            }

            public void FixedUpdate()
            {
                if (changed)
                {
                    Destroy(this);
                }
                if (!changed && body && body.inventory)
                {
                    if (body.inventory.currentEquipmentIndex != EquipmentIndex.None)
                    {
                        if (body.inventory.currentEquipmentIndex == DLC1Content.Equipment.BossHunterConsumed.equipmentIndex)
                        {
                            changed = true;
                            body.baseNameToken = "EQUIPMENTDRONE_BODY_NAME";
                        }
                        else
                        {
                            body.baseNameToken = Language.GetString("EQUIPMENTDRONE_BODY_NAME") + "\n(" + Language.GetString(EquipmentCatalog.GetEquipmentDef(body.inventory.currentEquipmentIndex).nameToken) + ")";
                            Debug.Log(body.baseNameToken);
                            changed = true;
                        }
                    }
                }
            }
        }

        public static RoR2.UI.LogBook.Entry[] LogbookEntryAdderMonsters(On.RoR2.UI.LogBook.LogBookController.orig_BuildMonsterEntries orig, Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            if (WConfig.MoreLogEntries.Value == true)
            {
                //UnlockableDef dummyunlock = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<DeathRewards>().logUnlockableDef;

                UnlockableDef gupunlock = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody").GetComponent<DeathRewards>().logUnlockableDef;
                UnlockableDef loopunlock = LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Items.BounceNearby");
                UnlockableDef bazaar = LegacyResourcesAPI.Load<UnlockableDef>("UnlockableDefs/Logs.Stages.bazaar");
                UnlockableDef limbo = LegacyResourcesAPI.Load<UnlockableDef>("UnlockableDefs/Logs.Stages.limbo");

                LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar1Body").GetComponent<CharacterBody>().baseNameToken = "SCAVLUNAR_BODY_SUBTITLE";

                LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar1Body").GetComponent<DeathRewards>().logUnlockableDef = limbo;
                //LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody").GetComponent<DeathRewards>().logUnlockableDef = bazaar;
                LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/UrchinTurretBody").AddComponent<DeathRewards>().logUnlockableDef = loopunlock;

                LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GeepBody").GetComponent<DeathRewards>().logUnlockableDef = gupunlock;
                LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GipBody").GetComponent<DeathRewards>().logUnlockableDef = gupunlock;

            }

            /*GameObject RobYoungVagrant = BodyCatalog.GetBodyPrefab(BodyCatalog.FindBodyIndex("RobYoungVagrantBody"));
            if (RobYoungVagrant)
            {
                if (!RobYoungVagrant.GetComponent<DeathRewards>().logUnlockableDef)
                {
                    RobYoungVagrant.GetComponent<DeathRewards>().logUnlockableDef = RobVag;
                    RobYoungVagrant.GetComponent<DeathRewards>().bossDropTable = null;
                }
            }*/


            var VALUES = orig(expansionAvailability);

            if (WConfig.MoreLogEntries.Value == true)
            {
                LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar1Body").GetComponent<CharacterBody>().baseNameToken = "SCAVLUNAR1_BODY_NAME";
                LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar1Body").GetComponent<DeathRewards>().logUnlockableDef = SceneCatalog.GetUnlockableLogFromBaseSceneName("limbo");
                //LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody").GetComponent<DeathRewards>().logUnlockableDef = SceneCatalog.GetUnlockableLogFromBaseSceneName("bazaar");
            }
            Debug.Log("WolfoQoL: LogbookChanger");

            return VALUES;
        }


        public static RoR2.UI.LogBook.Entry[] LogbookEliteAspectAdder(On.RoR2.UI.LogBook.LogBookController.orig_BuildPickupEntries orig, Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            int invoutput = EquipmentCatalog.equipmentCount;
            System.Collections.Generic.List<EquipmentDef> tempList = new System.Collections.Generic.List<EquipmentDef>();

            if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TPDespair.ZetAspects"))
            {
                if (WConfig.MoreLogEntriesAspect.Value == true)
                {
                    for (var i = 0; i < invoutput; i++)
                    {
                        EquipmentDef tempequipdef = EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i);
                        if (tempequipdef.passiveBuffDef && tempequipdef.passiveBuffDef.isElite)
                        {
                            string tempname = tempequipdef.name;
                            //Debug.LogWarning(tempequipdef + "   " + tempname + "   " + tempequipdef.pickupModelPrefab);

                            if (tempname.StartsWith("EliteEcho") || tempname.StartsWith("EliteSecretSpeed"))
                            {
                            }
                            else
                            {
                                if (WConfig.EnableColorChangeModule.Value == true)
                                {
                                    tempequipdef.isBoss = true;
                                    tempequipdef.colorIndex = ColorCatalog.ColorIndex.BossItem;
                                }
                                if (tempequipdef.dropOnDeathChance != 0)
                                {
                                    tempequipdef.canDrop = true;
                                    tempList.Add(tempequipdef);
                                }
                            }
                        }
                        //Debug.LogWarning(tempequipdef.GetPropertyValue<Texture>("bgIconTexture"));
                    }
                    LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").isBoss = true;
                    LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").isLunar = true;
                    LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").colorIndex = ColorCatalog.ColorIndex.LunarItem;
                }
            }

            if (WConfig.MoreLogEntries.Value == true)
            {
                LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/QuestVolatileBattery").canDrop = true;
                LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/BossHunterConsumed").canDrop = true;
                EquipmentDef HealAndReviveConsumed = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC2/HealAndRevive/HealAndReviveConsumed.asset").WaitForCompletion();
                HealAndReviveConsumed.canDrop = true;
                ModelPanelParameters camera = HealAndReviveConsumed.pickupModelPrefab.AddComponent<ModelPanelParameters>();
                camera.cameraPositionTransform = camera.transform;
                camera.focusPointTransform = camera.transform;
                camera.maxDistance = 3;
                camera.modelRotation = new Quaternion(0, 0.7071f, 0f, -0.7071f);
            }
            var VALUES = orig(expansionAvailability);
            if (WConfig.MoreLogEntries.Value == true)
            {
                LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/QuestVolatileBattery").canDrop = false;
                LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/BossHunterConsumed").canDrop = false;
                Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC2/HealAndRevive/HealAndReviveConsumed.asset").WaitForCompletion().canDrop = false;
            }

            for (var i = 0; i < tempList.Count; i++)
            {
                tempList[i].canDrop = false;
            }
            Debug.Log("WolfoQoL: LogbookChangerELITE");
            return VALUES;
        }

    }

}
