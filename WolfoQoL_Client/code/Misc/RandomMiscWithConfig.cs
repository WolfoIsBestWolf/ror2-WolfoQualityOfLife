using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace WolfoQoL_Client
{
    public class RandomMiscWithConfig
    {

        public static void BuffColorChanger()
        {
            LegacyResourcesAPI.Load<BuffDef>("buffdefs/AffixHauntedRecipient").buffColor = new Color32(148, 215, 214, 255); //94D7D6 Celestine Elite
            LegacyResourcesAPI.Load<BuffDef>("buffdefs/SmallArmorBoost").buffColor = LegacyResourcesAPI.Load<BuffDef>("buffdefs/Slow60").buffColor;
            LegacyResourcesAPI.Load<BuffDef>("buffdefs/WhipBoost").buffColor = new Color32(245, 158, 73, 255); //E8813D

            Texture2D texBuffRaincoat = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffRaincoat.png");
            Sprite texBuffRaincoatS = Sprite.Create(texBuffRaincoat, new Rect(0, 0, 128, 128), v.half);
            BuffDef RainCoat = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC1/ImmuneToDebuff/bdImmuneToDebuffReady.asset").WaitForCompletion();
            if (RainCoat.iconSprite.name.StartsWith("texBuffImmune"))
            {
                RainCoat.iconSprite = texBuffRaincoatS;
            }



            BuffDef TeleportOnLowHealthActive = Addressables.LoadAssetAsync<BuffDef>(key: "RoR2/DLC2/Items/TeleportOnLowHealth/bdTeleportOnLowHealthActive.asset").WaitForCompletion();
            TeleportOnLowHealthActive.isHidden = false;

            Texture2D texBuffTeleportOnLowHealthIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Buffs/texBuffTeleportOnLowHealthIcon.png");
            Sprite texBuffTeleportOnLowHealthIconS = Sprite.Create(texBuffTeleportOnLowHealthIcon, v.rec128, v.half);
            TeleportOnLowHealthActive.iconSprite = texBuffTeleportOnLowHealthIconS;

        }


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
                voidAlly.SetColor("_EmColor", new Color(0, 3, 2.25f, 1)); //1,1,1
                voidAlly = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC1/VoidJailer/matVoidJailerEyesAlly.mat").WaitForCompletion();
                voidAlly.SetColor("_EmColor", new Color(0f, 1f, 0.75f, 1)); //0.8706 0.4764 1 1
                voidAlly = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC1/VoidMegaCrab/matVoidMegaCrabAlly.mat").WaitForCompletion();
                voidAlly.SetColor("_EmColor", new Color(0f, 2f, 1f, 1f)); //0.7306 0 0.8208 1
            }


            On.RoR2.UI.ChatBox.Awake += AllowMoreChatMessages;





            On.RoR2.UI.CrosshairManager.OnEnable += CrosshairManager_OnEnable;


            //Equipment Drone Equipment Name


            /*
            GameObject mdlGup = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody").transform.GetChild(0).GetChild(0).gameObject;
            GameObject mdlGeep = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GeepBody").transform.GetChild(0).GetChild(0).gameObject;
            GameObject mdlGip = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GipBody").transform.GetChild(0).GetChild(0).gameObject;

            mdlGeep.AddComponent<ModelPanelParameters>();
            mdlGip.AddComponent<ModelPanelParameters>();
            */
            On.RoR2.UI.PingIndicator.RebuildPing += (orig, self) =>
            {
                orig(self);
                self.fixedTimer *= 2;
                //self.fixedTimer *= WConfig.cfgPingDurationMultiplier.Value;
            };
        }



        private static void CrosshairManager_OnEnable(On.RoR2.UI.CrosshairManager.orig_OnEnable orig, RoR2.UI.CrosshairManager self)
        {
            //Debug.Log(" On.RoR2.UI.CrosshairManager.OnEnable " + self);
            orig(self);
            if (WConfig.cfgNewSprintCrosshair.Value == false)
            {
                return;
            }
            GameObject SprintingCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/SprintingCrosshair.prefab").WaitForCompletion();
            if (SprintingCrosshair.transform.childCount > 0)
            {
                return;
            }
            SprintingCrosshair.transform.localScale = Vector3.one * 1.15f;
            CrosshairController SprintingCrosshairUI = SprintingCrosshair.GetComponent<RoR2.UI.CrosshairController>();
            RawImage rawMid = SprintingCrosshair.GetComponent<RawImage>();
            SprintingCrosshairUI.maxSpreadAngle = 10;

            rawMid.texture = Addressables.LoadAssetAsync<Texture2D>(key: "RoR2/Base/UI/texCrosshairDot.png").WaitForCompletion();
            rawMid.color = new Color(1f, 1f, 1f, 0.5f);

            Texture2D Arrow = Addressables.LoadAssetAsync<Texture2D>(key: "RoR2/Base/UI/texCrosshairArrow.png").WaitForCompletion();
            GameObject Right = new GameObject("Right");
            RawImage raw = Right.AddComponent<RawImage>();
            raw.texture = Arrow;
            raw.color = rawMid.color;

            GameObject Left = new GameObject("Left");
            raw = Left.AddComponent<RawImage>();
            raw.texture = Arrow;
            raw.color = rawMid.color;


            Right.transform.SetParent(SprintingCrosshair.transform);
            Left.transform.SetParent(SprintingCrosshair.transform);
            Right.transform.SetLocalEulerAngles(new Vector3(0, 0, -90), RotationOrder.OrderXYZ);
            Left.transform.SetLocalEulerAngles(new Vector3(0, 0, 90), RotationOrder.OrderXYZ);

            SprintingCrosshairUI.spriteSpreadPositions = new RoR2.UI.CrosshairController.SpritePosition[]
            {
                new CrosshairController.SpritePosition
                {
                    onePosition = new Vector3(96,0,0),
                    target = Left.GetComponent<RectTransform>(),
                    zeroPosition = new Vector3(24,0,0)
                },
                new CrosshairController.SpritePosition
                {
                    onePosition = new Vector3(-96,0,0),
                    target = Right.GetComponent<RectTransform>(),
                    zeroPosition = new Vector3(-24,0,0)
                },
            };

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
                        ClassicStageInfo.instance.StartCoroutine(Courtines.Delayed_ChatBroadcast(target, 0.9f));
                        return null;
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


        /*public class SendExtraMountainIcon : RoR2.ChatMessageBase
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

        }*/

        public static RoR2.UI.LogBook.Entry[] LogbookEntryAdderMonsters(On.RoR2.UI.LogBook.LogBookController.orig_BuildMonsterEntries orig, Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            if (WConfig.cfgLogbook_More.Value == true)
            {
                //UnlockableDef dummyunlock = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<DeathRewards>().logUnlockableDef;

                UnlockableDef gupunlock = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody").GetComponent<DeathRewards>().logUnlockableDef;
                UnlockableDef loopunlock = LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Items.BounceNearby");
                //UnlockableDef bazaar = LegacyResourcesAPI.Load<UnlockableDef>("UnlockableDefs/Logs.Stages.bazaar");
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

            if (WConfig.cfgLogbook_More.Value == true)
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
            List<EquipmentDef> tempEliteList = new List<EquipmentDef>();

            if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TPDespair.ZetAspects"))
            {
                if (WConfig.cfgLogbook_EliteEquip.Value == true)
                {
                    EquipmentDef EliteSecretSpeedEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteSecretSpeedEquipment.asset").WaitForCompletion();

                    for (var i = 0; i < invoutput; i++)
                    {
                        EquipmentDef equipDef = EquipmentCatalog.GetEquipmentDef((EquipmentIndex)i);
                        if (EliteSecretSpeedEquipment == equipDef)
                        {
                            break;
                        }
                        if (equipDef.passiveBuffDef && equipDef.passiveBuffDef.isElite)
                        {
                            if (equipDef.dropOnDeathChance != 0)
                            {
                                equipDef.canDrop = true;
                                tempEliteList.Add(equipDef);
                            }
                        }
                    }
           
 
                }
            }

            if (WConfig.cfgLogbook_More.Value == true)
            {
                RoR2Content.Equipment.QuestVolatileBattery.canDrop = true;
                DLC1Content.Equipment.BossHunterConsumed.canDrop = true;
                DLC2Content.Equipment.HealAndReviveConsumed.canDrop = true;
                ModelPanelParameters camera = DLC2Content.Equipment.HealAndReviveConsumed.pickupModelPrefab.AddComponent<ModelPanelParameters>();
                camera.cameraPositionTransform = camera.transform;
                camera.focusPointTransform = camera.transform;
                camera.maxDistance = 3;
                camera.modelRotation = new Quaternion(0, 0.7071f, 0f, -0.7071f);
            }
            var VALUES = orig(expansionAvailability);
            if (WConfig.cfgLogbook_More.Value == true)
            {
                RoR2Content.Equipment.QuestVolatileBattery.canDrop = false;
                DLC1Content.Equipment.BossHunterConsumed.canDrop = false;
                DLC2Content.Equipment.HealAndReviveConsumed.canDrop = false;
            }

            for (var i = 0; i < tempEliteList.Count; i++)
            {
                tempEliteList[i].canDrop = false;
            }
            Debug.Log("WolfoQoL: LogbookChangerELITE");
            return VALUES;
        }

    }

}
