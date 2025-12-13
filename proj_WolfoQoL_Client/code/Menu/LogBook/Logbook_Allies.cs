using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.EntitlementManagement;
using RoR2.ExpansionManagement;
using RoR2.UI.LogBook;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using WolfoLibrary;

namespace WolfoQoL_Client
{
    public class Logbook_AllyStatsMult : MonoBehaviour
    {
        public float healthMult = 1f;
        public float damageMult = 1f;
        public int healthDecay;
    }
    public class Logbook_Allies
    {

        public static void MoreStats()
        {

            Logbook_AllyStatsMult stats = null;
            GameObject Squid = Addressables.LoadAssetAsync<GameObject>(key: "5290eaf612d386740ac26f289e06b62f").WaitForCompletion();
            stats = Squid.AddComponent<Logbook_AllyStatsMult>();
            stats.healthDecay = 30;

            GameObject BeetleGuard = Addressables.LoadAssetAsync<GameObject>(key: "e21e3b9cdc2802148986eda1c923c9a1").WaitForCompletion();
            stats = BeetleGuard.AddComponent<Logbook_AllyStatsMult>();
            stats.healthMult = 2;
            stats.damageMult = 4;

            GameObject DefenseNecleus = Addressables.LoadAssetAsync<GameObject>(key: "bf5b53aa9306bce4b910220a43b30062").WaitForCompletion();
            stats = DefenseNecleus.AddComponent<Logbook_AllyStatsMult>();
            stats.healthMult = 4;
            stats.damageMult = 4;

            GameObject Infestor = Addressables.LoadAssetAsync<GameObject>(key: "1553966a1aafdb743bf87ff4d6602c22").WaitForCompletion();
            stats = Infestor.AddComponent<Logbook_AllyStatsMult>();
            stats.healthDecay = 30;

            //Mult pre and post or just IL
            On.RoR2.UI.LogBook.PageBuilder.AddBodyStatsPanel += PageBuilder_AddBodyStatsPanel1;

            On.RoR2.UI.LogBook.LogBookController.BuildDroneEntries += LogBookController_BuildDroneEntries;
            On.RoR2.UI.LogBook.PageBuilder.AddDronePanel += PageBuilder_AddDronePanel;
            IL.RoR2.UI.LogBook.PageBuilder.AddBodyStatsPanel += FixNegativeRegenStatNotShowingInLog;
            IL.RoR2.UI.LogBook.PageBuilder.DroneBody += AddOperatorCommand;

           
            BodyCatalog.availability.CallWhenAvailable(AllyCatalog.Make);
        
            LogBookController.availability.CallWhenAvailable(SetDroneIcon);
        }
 
        public static void SetDroneIcon()
        {
            LogBookController.CommonAssets.droneEntryIcon.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0.6588f, 0.708f, 0.741f, 1); //0.6588 0.6588 0.6588 1
        }

        private static void FixNegativeRegenStatNotShowingInLog(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("UnityEngine.Mathf", "Epsilon")))
            {
                c.Prev.OpCode = OpCodes.Ldc_R4;
                c.Prev.Operand = 0f;
                c.Next.OpCode = OpCodes.Bne_Un_S;
            }
            else
            {
                WQoLMain.log.LogWarning("IL Failed: FixNegativeRegenStatNotShowingInLog");
            }
        }

        private static void AddOperatorCommand(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchLdloc(0),
            x => x.MatchCallvirt("RoR2.UI.LogBook.PageBuilder", "AddSimpleBody")))
            {
                c.Emit(OpCodes.Ldloc_0);
                c.EmitDelegate<Func<PageBuilder, CharacterBody, PageBuilder>>((page, body) =>
                {
                    bool isDLC3 = false;
                    if (Run.instance && Run.instance.IsExpansionEnabled(DLCS.DLC3))
                    {
                        //Multiplayer DLC3, but doesnt have self.
                        isDLC3 = true;
                    }
                    else if (EntitlementManager.localUserEntitlementTracker.AnyUserHasEntitlement(DLCS.DLC3.requiredEntitlement))
                    {
                        //Fallback
                        isDLC3 = true;
                    }
                    if (isDLC3)
                    {
                        //TIERING
                        string tierToken = body.baseNameToken.Replace("DT", "").Replace("BODY_NAME", "TIERINFO");
                        if (Language.english.TokenIsRegistered(tierToken))
                        {
                            var textMesh = page.managedObjects.Last().GetComponent<ChildLocator>().FindChild("MainLabel").GetComponent<TextMeshProUGUI>();
                            textMesh.text += "\n" + Language.GetString(tierToken);
                        }


                        //SHOULD CHECK IF DLC3 IS LIKE IDK UNLOCKED
                        if (body.TryGetComponent<DroneCommandReceiver>(out var commandReceiver))
                        {
                            GenericSkill commandSkill = commandReceiver.commandSkill;
                            if (commandReceiver.commandSkill)
                            {
                                if (commandReceiver.commandSkill.skillFamily)
                                {

                                    page.AddSimpleTextPanel(string.Format("<color=#1FEFAD>{0}:</color>\n{1}", new object[]
                                    {
                                       Language.GetString("DRONETECH_SECONDARY_NAME"),
                                       Language.GetString(commandReceiver.commandSkill.skillFamily.defaultSkillDef.skillDescriptionToken)
                                    }));
                                }
                            }
                        }
                    }
                    return page;
                });
            }
            else
            {
                WQoLMain.log.LogWarning("IL Failed: AddOperatorCommand");
            }
        }
 
        private static void PageBuilder_AddDronePanel(On.RoR2.UI.LogBook.PageBuilder.orig_AddDronePanel orig, PageBuilder self, CharacterBody bodyPrefabComponent)
        {
            orig(self, bodyPrefabComponent);
            var textMesh = self.managedObjects.Last().GetComponent<ChildLocator>().FindChild("MainLabel").GetComponent<TextMeshProUGUI>();
            /*textMesh.text += Language.GetStringFormatted("DRONE_DAMAGE_DEALT", new object[]
            {
                       (int)WStats.GetStat((int)bodyPrefabComponent.bodyIndex)
            });*/
            DroneIndex droneIndex = DroneCatalog.GetDroneIndexFromBodyIndex(bodyPrefabComponent.bodyIndex);
            if (droneIndex != DroneIndex.None)
            {
                DroneDef droneDef = DroneCatalog.GetDroneDef(droneIndex);
                if (droneDef && droneDef.droneBrokenSpawnCard)
                {
                    PurchaseInteraction purchase = droneDef.droneBrokenSpawnCard.prefab.GetComponent<PurchaseInteraction>();
                    CostTypeDef costTypeDef = CostTypeCatalog.GetCostTypeDef(purchase.costType);
                    Language.GetStringFormatted(costTypeDef.costStringFormatToken, purchase.cost);


                    textMesh.text += Language.GetStringFormatted("DRONE_COST_STAT", new object[]
                   {
                       Language.GetStringFormatted(costTypeDef.costStringFormatToken, purchase.cost)
                   });
                }
            }


        }

        private static Entry[] LogBookController_BuildDroneEntries(On.RoR2.UI.LogBook.LogBookController.orig_BuildDroneEntries orig, Dictionary<ExpansionDef, bool> expansionAvailability)
        {

            DLC3Content.DroneDefs.DTHaulerDrone.desiredSortPosition = 14;
            DLC3Content.DroneDefs.DTHealDrone.desiredSortPosition = 13;
            DLC3Content.DroneDefs.BomberDrone.desiredSortPosition = 100;
            DLC1Content.DroneDefs.DroneCommander.desiredSortPosition = 100;

            RoR2Content.DroneDefs.MissileDrone.desiredSortPosition = 0;
            RoR2Content.DroneDefs.FlameDrone.desiredSortPosition = 1;

            RoR2Content.DroneDefs.BackupDrone.tier = ItemTier.Lunar;
            DLC3Content.DroneDefs.DTGunnerDrone.tier = ItemTier.VoidTier1;
            DLC3Content.DroneDefs.DTHealDrone.tier = ItemTier.VoidTier1;
            DLC3Content.DroneDefs.DTHaulerDrone.tier = ItemTier.VoidTier1;

            Entry[] entries = orig(expansionAvailability);

            foreach (Entry entry in entries)
            {
                if (entry.extraData == RoR2Content.BodyPrefabs.BackupDroneBody)
                {
                    entry.bgTexture = LegacyResourcesAPI.Load<Texture>("Textures/ItemIcons/BG/texEquipmentBGIcon");

                }
                else if (entry.extraData == DLC3Content.BodyPrefabs.DTGunnerDroneBody ||
                    entry.extraData == DLC3Content.BodyPrefabs.DTHaulerDroneBody ||
                    entry.extraData == DLC3Content.BodyPrefabs.DTHealingDroneBody)
                {
                    entry.bgTexture = LogBookController.CommonAssets.survivorBgIcon;
                }
            }

            List<Entry> newEntries = new List<Entry>();
            foreach (var allyDefPair in AllyCatalog.AllyDefs)
            {
                if (AllyCatalog.CanSelectDroneEntry(allyDefPair.Value, expansionAvailability))
                {
                    CharacterBody characterBody = BodyCatalog.GetBodyPrefabBodyComponent(allyDefPair.Key);
                    ItemTierDef itemTierDef = ItemTierCatalog.GetItemTierDef(allyDefPair.Value.tier);
                    GameObject modelPrefab = null;
                    ModelLocator component = characterBody.GetComponent<ModelLocator>();
                    if (component != null && component.logBookModel != null)
                    {
                        modelPrefab = component.logBookModel.gameObject;
                    }
                    else
                    {
                        Debug.LogError("MISSING MODEL PREFAB REF ON " + characterBody.baseNameToken);
                    }
                    newEntries.Add(new Entry
                    {
                        nameToken = characterBody.baseNameToken,
                        iconTexture = characterBody.portraitIcon,
                        color = ColorCatalog.GetColor(allyDefPair.Value.colorOverride != ColorCatalog.ColorIndex.None ? allyDefPair.Value.colorOverride : itemTierDef.colorIndex),
                        bgTexture = allyDefPair.Value.bgTextureOverride ? allyDefPair.Value.bgTextureOverride : itemTierDef.bgIconTexture,
                        extraData = characterBody,
                        modelPrefab = modelPrefab,
                        getStatusImplementation = new Entry.GetStatusDelegate(AllyCatalog.GetAllyStatus),
                        getTooltipContentImplementation = new Entry.GetTooltipContentDelegate(LogBookController.GetDroneTooltipContent),
                        pageBuilderMethod = new Action<PageBuilder>(PageBuilder.DroneBody),
                        isWIPImplementation = new Entry.IsWIPDelegate(LogBookController.IsEntryBodyWithoutLore)
                    });
                }
            }
            newEntries.AddRange(entries);

            var newerENTRIES = newEntries.OrderBy(entry => AllyCatalog.TierFromBodyDroneOrAlly((CharacterBody)entry.extraData)).ThenBy(entry => AllyCatalog.SortFromBodyDroneOrAlly((CharacterBody)entry.extraData)).ToArray();

            RoR2Content.DroneDefs.BackupDrone.tier = ItemTier.Tier2;
            DLC3Content.DroneDefs.DTGunnerDrone.tier = ItemTier.Tier2;
            DLC3Content.DroneDefs.DTHealDrone.tier = ItemTier.Tier2;
            DLC3Content.DroneDefs.DTHaulerDrone.tier = ItemTier.Tier2;

            return newerENTRIES;
            return entries;
        }

        private static void PageBuilder_AddBodyStatsPanel1(On.RoR2.UI.LogBook.PageBuilder.orig_AddBodyStatsPanel orig, PageBuilder self, CharacterBody body)
        {
            //This would work fine evne if IL would be better ig
            Logbook_AllyStatsMult statsMult = body.GetComponent<Logbook_AllyStatsMult>();
            if (statsMult)
            {
                CharacterBody bodyNew = new CharacterBody
                {
                    baseMaxHealth = body.baseMaxHealth * statsMult.healthMult,
                    levelMaxHealth = body.levelMaxHealth * statsMult.healthMult,
                    baseDamage = body.baseDamage * statsMult.damageMult,
                    levelDamage = body.levelDamage * statsMult.damageMult,
                    baseArmor = body.armor,
                    baseRegen = body.baseRegen,
                    levelRegen = body.levelRegen,
                    baseMoveSpeed = body.baseMoveSpeed,
                };
                if (statsMult.healthDecay > 0)
                {
                    bodyNew.baseRegen = -body.baseMaxHealth / statsMult.healthDecay;
                    bodyNew.levelRegen = -body.levelMaxHealth / statsMult.healthDecay;
                }
                orig(self, bodyNew);
                GameObject.Destroy(body);
                return;
            }
            orig(self, body);
        }


    }

}