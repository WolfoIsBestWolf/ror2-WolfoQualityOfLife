using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;


namespace WolfoQualityOfLife
{
    public class RandomMiscWithConfig
    {
        public static GameObject SprintingCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/SprintingCrosshair.prefab").WaitForCompletion();
        public static GameObject LoaderCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Loader/LoaderCrosshair.prefab").WaitForCompletion();


        public static void Start()
        {
            //Mod shouldn't be used with HistoryFix
            On.RoR2.MorgueManager.EnforceHistoryLimit += (orig) =>
            {
                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("local.fix.history"))
                {
                    orig();
                    return;
                }
                List<MorgueManager.HistoryFileInfo> list = HG.CollectionPool<MorgueManager.HistoryFileInfo, List<MorgueManager.HistoryFileInfo>>.RentCollection();
                MorgueManager.GetHistoryFiles(list);
                int i = list.Count - 1;
                int num = System.Math.Max(MorgueManager.morgueHistoryLimit.value, 0);
                while (i >= num)
                {
                    i--;
                    MorgueManager.RemoveOldestHistoryFile();
                }
                HG.CollectionPool<MorgueManager.HistoryFileInfo, List<MorgueManager.HistoryFileInfo>>.ReturnCollection(list);
            };


            if (WConfig.cfgMountainStacks.Value == true)
            {
                //Goofy, rather compact and did it before RoRR
                //On.RoR2.TeleporterInteraction.AddShrineStack += MultipleMountainSymbols;
                On.RoR2.TeleporterInteraction.AddShrineStack += (orig, self) =>
                {
                    orig(self);
                    //This is Host to Client, Client to Host is entirely different beast
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

                    RightBracket.transform.SetParent(SprintingCrosshair.transform);
                    LeftBracket.transform.SetParent(SprintingCrosshair.transform);

                    SprintingCrosshairUI.spriteSpreadPositions[0].target = LeftBracket.GetComponent<RectTransform>();
                    SprintingCrosshairUI.spriteSpreadPositions[1].target = RightBracket.GetComponent<RectTransform>();
                };
            }
            //Equipment Drone Equipment Name
            if (WConfig.cfgEquipmentDroneName.Value == true)
            {
                On.RoR2.Items.MinionLeashBodyBehavior.Start += (orig, self) =>
                {
                    orig(self);
                    //Debug.LogWarning("CharacterBody.MinionLeashBehavior.Start");
                    if (self.name.StartsWith("EquipmentDrone") && self.body)
                    {
                        if (self.body.inventory.currentEquipmentIndex != EquipmentIndex.None && self.body.inventory.currentEquipmentIndex != DLC1Content.Equipment.BossHunterConsumed.equipmentIndex && !RunArtifactManager.instance.IsArtifactEnabled(RoR2Content.Artifacts.enigmaArtifactDef))
                        {
                            self.body.baseNameToken = Language.GetString("EQUIPMENTDRONE_BODY_NAME") + "\n(" + Language.GetString(EquipmentCatalog.GetEquipmentDef(self.body.inventory.currentEquipmentIndex).nameToken) + ")";
                        }
                    }
                };
            }

            /*
            GameObject mdlGup = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody").transform.GetChild(0).GetChild(0).gameObject;
            GameObject mdlGeep = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GeepBody").transform.GetChild(0).GetChild(0).gameObject;
            GameObject mdlGip = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GipBody").transform.GetChild(0).GetChild(0).gameObject;

            mdlGeep.AddComponent<ModelPanelParameters>();
            mdlGip.AddComponent<ModelPanelParameters>();
            */
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
                    Debug.LogWarning("Sent ExtraMountainIcon withut Teleporter Object");
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

        public static void LogbookEntryAdder(On.RoR2.UI.LogBook.LogBookController.orig_BuildStaticData orig)
        {
            if (WConfig.MoreLogEntries.Value == true)
            {
                //UnlockableDef dummyunlock = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<DeathRewards>().logUnlockableDef;

                UnlockableDef gupunlock = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody").GetComponent<DeathRewards>().logUnlockableDef;
                UnlockableDef loopunlock = RoR2.LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Items.BounceNearby");
                UnlockableDef bazaar = RoR2.LegacyResourcesAPI.Load<UnlockableDef>("UnlockableDefs/Logs.Stages.bazaar");
                UnlockableDef limbo = RoR2.LegacyResourcesAPI.Load<UnlockableDef>("UnlockableDefs/Logs.Stages.limbo");

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar3Body").GetComponent<CharacterBody>().baseNameToken = "SCAVLUNAR_BODY_SUBTITLE";

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar3Body").GetComponent<DeathRewards>().logUnlockableDef = limbo;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody").GetComponent<DeathRewards>().logUnlockableDef = bazaar;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/UrchinTurretBody").AddComponent<DeathRewards>().logUnlockableDef = loopunlock;

                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GeepBody").GetComponent<DeathRewards>().logUnlockableDef = gupunlock;
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GipBody").GetComponent<DeathRewards>().logUnlockableDef = gupunlock;

                RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/QuestVolatileBattery").canDrop = true;
                //RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/BossHunterConsumed").canDrop = true;
            }

            int invoutput = EquipmentCatalog.equipmentCount;
            System.Collections.Generic.List<EquipmentDef> tempList = new System.Collections.Generic.List<EquipmentDef>();

            if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TPDespair.ZetAspects"))
            {
                if (WConfig.MoreLogEntriesAspect.Value == true)
                {
                    for (var i = 0; i < invoutput; i++)
                    {
                        EquipmentDef tempequipdef = EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i);
                        string tempname = tempequipdef.name;
                        //Debug.LogWarning(tempequipdef + "   " + tempname + "   " + tempequipdef.pickupModelPrefab);

                        if (tempequipdef.passiveBuffDef && tempequipdef.passiveBuffDef.isElite)
                        {
                            if (tempname.StartsWith("EliteEcho") || tempname.StartsWith("EliteSecretSpeed"))
                            {
                            }
                            else
                            {
                                if (!tempname.StartsWith("ElitePoison") && !tempname.StartsWith("EliteHaunted"))
                                {
                                }
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
                    RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").isBoss = false;
                    RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").isLunar = true;
                    RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/AffixLunar").colorIndex = ColorCatalog.ColorIndex.LunarItem;
                }
            }
            //Idk what the hell this is 
            for (int i = 0; i < PickupCatalog.entries.Length; i++)
            {
                if (PickupCatalog.entries[i].itemIndex != ItemIndex.None)
                {
                    PickupCatalog.entries[i].iconSprite = ItemCatalog.GetItemDef(PickupCatalog.entries[i].itemIndex).pickupIconSprite;
                }
                else if (PickupCatalog.entries[i].equipmentIndex != EquipmentIndex.None)
                {
                    PickupCatalog.entries[i].iconSprite = EquipmentCatalog.GetEquipmentDef(PickupCatalog.entries[i].equipmentIndex).pickupIconSprite;
                }
            }

            orig();

            if (WConfig.MoreLogEntries.Value == true)
            {
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar3Body").GetComponent<CharacterBody>().baseNameToken = "SCAVLUNAR3_BODY_NAME";
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar3Body").GetComponent<DeathRewards>().logUnlockableDef = SceneCatalog.GetUnlockableLogFromBaseSceneName("limbo");
                RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody").GetComponent<DeathRewards>().logUnlockableDef = SceneCatalog.GetUnlockableLogFromBaseSceneName("bazaar");
                RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/QuestVolatileBattery").canDrop = false;
                //RoR2.LegacyResourcesAPI.Load<EquipmentDef>("equipmentdefs/BossHunterConsumed").canDrop = false;
            }

            for (var i = 0; i < tempList.Count; i++)
            {
                tempList[i].canDrop = false;
            }
            Debug.Log("WolfoQoL: LogbookChanger");

        }

    }

}
