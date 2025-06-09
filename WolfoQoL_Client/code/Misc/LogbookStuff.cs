using RoR2;
using RoR2.ExpansionManagement;
using RoR2.UI.LogBook;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace WolfoQoL_Client
{

    public class LogbookStuff
    {

        public static void NormalLogbook()
        {
            On.RoR2.UI.LogBook.LogBookController.BuildMonsterEntries += AddMissingMonsters;
            On.RoR2.UI.LogBook.LogBookController.BuildMonsterEntries += SortBossMonster;
            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += AddMissingEquipment;
            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += SortBossEquip_AddBG;
            On.RoR2.UI.LogBook.LogBookController.BuildSurvivorEntries += SurvivorEntryColor;

            On.RoR2.UI.LogBook.LogBookController.GetPickupStatus += EliteEquipmentViewable;  
        }

        private static EntryStatus EliteEquipmentViewable(On.RoR2.UI.LogBook.LogBookController.orig_GetPickupStatus orig, ref Entry entry, UserProfile viewerProfile)
        {
            PickupIndex pickupIndex = (PickupIndex)entry.extraData;
            PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
            EquipmentIndex equipmentIndex = (pickupDef != null) ? pickupDef.equipmentIndex : EquipmentIndex.None;
            if (equipmentIndex != EquipmentIndex.None)
            {
                EquipmentDef def = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
                if (def.passiveBuffDef && def.passiveBuffDef.isElite)
                {
                    if (viewerProfile.HasDiscoveredPickup(pickupIndex))
                    {
                        return EntryStatus.Available;
                    }
                    bool available = false;
                    if (def == RoR2Content.Equipment.AffixLunar)
                    {
                        available = viewerProfile.HasUnlockable(RoR2Content.Items.LunarBadLuck.unlockableDef);
                    }
                    else if (def == DLC1Content.Equipment.EliteVoidEquipment)
                    {
                        available = viewerProfile.HasAchievement("Characters.VoidSurvivor");
                    }
                    else if (def == DLC2Content.Equipment.EliteAurelioniteEquipment)
                    {
                        available = viewerProfile.HasUnlockable(DLC2Content.Equipment.HealAndRevive.unlockableDef);
                    }
                    else
                    {
                        EliteDef eliteDef = def.passiveBuffDef.eliteDef;
                        if (eliteDef.devotionLevel == EliteDef.DevotionEvolutionLevel.High)
                        {
                            available = viewerProfile.HasUnlockable(RoR2Content.Items.Clover.unlockableDef);
                        }
                        else
                        {
                            available = viewerProfile.HasUnlockable(RoR2Content.Survivors.Captain.unlockableDef);
                        }
                    }
                    if (available)
                    {
                        return EntryStatus.Available;
                    }
                    else
                    {
                        return EntryStatus.Unencountered;
                    }
                }
            }
            return orig(ref entry, viewerProfile);
        }

        private static Entry[] SortBossMonster(On.RoR2.UI.LogBook.LogBookController.orig_BuildMonsterEntries orig, Dictionary<ExpansionDef, bool> expansionAvailability)
        {
            if (!WConfig.cfgLogbook_SortBosses.Value)
            {
                return orig(expansionAvailability);
            }
            Entry[] entries = orig(expansionAvailability);

            //Config val for this
            List<Entry> NotBosses = new List<Entry>();
            List<Entry> Bosses = new List<Entry>();

            for (int i = 0; i < entries.Length; i++)
            {
                if ((entries[i].extraData as CharacterBody).isChampion)
                {
                    Bosses.Add(entries[i]);
                }
                else
                {
                    NotBosses.Add(entries[i]);
                }
            }
            NotBosses.AddRange(Bosses);
            return NotBosses.ToArray();
        }

        public static Entry[] AddMissingMonsters(On.RoR2.UI.LogBook.LogBookController.orig_BuildMonsterEntries orig, Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            if (WConfig.cfgLogbook_More.Value == false)
            {
                return orig(expansionAvailability);
            }
            GameObject GupBody = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GupBody");
            GameObject GeepBody = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GeepBody");
            GameObject GipBody = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GipBody");

            ModelPanelParameters Geep = GeepBody.transform.GetChild(0).GetChild(0).gameObject.AddComponent<ModelPanelParameters>();
            ModelPanelParameters Gip = GipBody.transform.GetChild(0).GetChild(0).gameObject.AddComponent<ModelPanelParameters>();

            UnlockableDef Log_Gup = GupBody.GetComponent<DeathRewards>().logUnlockableDef;
            GeepBody.GetComponent<DeathRewards>().logUnlockableDef = Log_Gup;
            GipBody.GetComponent<DeathRewards>().logUnlockableDef = Log_Gup;

            UnlockableDef Log_Limbo = LegacyResourcesAPI.Load<UnlockableDef>("UnlockableDefs/Logs.Stages.limbo");
            GameObject ScavLunar1 = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavLunar1Body");
            ScavLunar1.GetComponent<CharacterBody>().baseNameToken = "SCAVLUNAR_BODY_SUBTITLE";
            ScavLunar1.GetComponent<DeathRewards>().logUnlockableDef = Log_Limbo;

            UnlockableDef Unlock_Loop = LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Items.Clover");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/UrchinTurretBody").AddComponent<DeathRewards>().logUnlockableDef = Unlock_Loop;

            GameObject newtBody = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody");
            Transform mdlNewt = newtBody.transform.GetChild(0).GetChild(0);
            ModelPanelParameters Newt = mdlNewt.gameObject.AddComponent<ModelPanelParameters>();

            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/bazaar").dioramaPrefab = mdlNewt.gameObject;

            Entry[] entries_TEMP = orig(expansionAvailability);
            ScavLunar1.GetComponent<CharacterBody>().baseNameToken = "SCAVLUNAR1_BODY_NAME";
            return entries_TEMP;
        }

        public static Entry[] AddMissingEquipment(On.RoR2.UI.LogBook.LogBookController.orig_BuildPickupEntries orig, Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            JunkContent.Equipment.EliteGoldEquipment.dropOnDeathChance = 0;
            List<EquipmentDef> equipmentChanged = new List<EquipmentDef>();
            if (!BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TPDespair.ZetAspects") && WConfig.cfgLogbook_EliteEquip.Value)
            {
                for (var i = 0; i < EliteCatalog.eliteDefs.Length; i++)
                {
                    EquipmentDef equipDef = EliteCatalog.eliteDefs[i].eliteEquipmentDef;
                    if (equipDef && equipDef.dropOnDeathChance != 0)
                    {
                        equipmentChanged.Add(equipDef);
                    }
                }
            }
            if (WConfig.cfgLogbook_More.Value)
            {
                equipmentChanged.Add(RoR2Content.Equipment.QuestVolatileBattery);
                equipmentChanged.Add(DLC1Content.Equipment.BossHunterConsumed);
                equipmentChanged.Add(DLC2Content.Equipment.HealAndReviveConsumed);
            }
            for (var i = 0; i < equipmentChanged.Count; i++)
            {
                equipmentChanged[i].canDrop = true;
            }
            RoR2Content.Equipment.AffixLunar.isLunar = false;

            var VALUES = orig(expansionAvailability);

            for (var i = 0; i < equipmentChanged.Count; i++)
            {
                equipmentChanged[i].canDrop = false;
            }
            RoR2Content.Equipment.AffixLunar.isLunar = true;

            return VALUES;
        }

        public static Entry[] SortBossEquip_AddBG(On.RoR2.UI.LogBook.LogBookController.orig_BuildPickupEntries orig, Dictionary<ExpansionDef, bool> expansionAvailability)
        {
            Entry[] array = orig(expansionAvailability);

            #region Sort Boss Equipment
            int num = -1;
            List<Entry> list = new List<Entry>();
            for (int j = 0; j < array.Length; j++)
            {
                if (!list.Contains(array[j]))
                {
                    PickupDef pickupDef2 = ((PickupIndex)array[j].extraData).pickupDef;
                    EquipmentIndex equipmentIndex = pickupDef2.equipmentIndex;
                    if (equipmentIndex != EquipmentIndex.None)
                    {
                        EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
                        if (equipmentDef && equipmentDef.isBoss)
                        {
                            Entry entry = array[j];
                            list.Add(array[j]);

                            HG.ArrayUtils.ArrayRemoveAtAndResize<RoR2.UI.LogBook.Entry>(ref array, j, 1);
                            num = array.Length;
                            j--;
                            HG.ArrayUtils.ArrayInsert<RoR2.UI.LogBook.Entry>(ref array, num, entry);
                            num++;
                        }
                    }
                }
            }
            #endregion

            #region ChangeBG
            for (int i = 0; i < array.Length; i++)
            {
                PickupIndex tempind = PickupCatalog.FindPickupIndex(array[i].extraData.ToString());
                PickupDef temppickdef = PickupCatalog.GetPickupDef(tempind);
                if (temppickdef.equipmentIndex != EquipmentIndex.None)
                {
                    EquipmentDef tempeqdef = EquipmentCatalog.GetEquipmentDef(temppickdef.equipmentIndex);
                    if (tempeqdef.isBoss == true)
                    {
                        array[i].bgTexture = ColorModule.texEquipmentBossBG;
                    }
                    else if (tempeqdef.isLunar == true)
                    {
                        array[i].bgTexture = ColorModule.texEquipmentLunarBG;
                    }

                }
            }
            #endregion
            return array;
        }

        public static Entry[] SurvivorEntryColor(On.RoR2.UI.LogBook.LogBookController.orig_BuildSurvivorEntries orig, Dictionary<ExpansionDef, bool> expansionAvailability)
        {
            Entry[] array = orig(expansionAvailability);
            for (int i = 0; i < array.Length; i++)
            {
                array[i].color = ColorModule.NewSurvivorLogbookNameColor;
            }
            return array;
        }


        public static void CheatLogbook()
        {


            //If you need to relaunch for it to take effect might as well right?
            On.RoR2.UI.LogBook.LogBookController.CanSelectItemEntry += LogBookController_CanSelectItemEntry;
            On.RoR2.UI.LogBook.LogBookController.CanSelectEquipmentEntry += LogBookController_CanSelectEquipmentEntry;
            On.RoR2.UI.LogBook.LogBookController.BuildMonsterEntries += LogBookController_BuildMonsterEntries;

            On.RoR2.UI.LogBook.LogBookController.IsEntryPickupItemWithoutLore += LogBookController_IsEntryPickupItemWithoutLore;
            On.RoR2.UI.LogBook.LogBookController.IsEntryPickupEquipmentWithoutLore += LogBookController_IsEntryPickupEquipmentWithoutLore;
            On.RoR2.UI.LogBook.LogBookController.IsEntryBodyWithoutLore += LogBookController_IsEntryBodyWithoutLore;

            On.RoR2.UI.LogBook.LogBookController.GetPickupStatus += LogBookController_GetPickupStatus;
            On.RoR2.UI.LogBook.LogBookController.GetMonsterStatus += LogBookController_GetMonsterStatus;
            On.RoR2.UI.LogBook.LogBookController.GetStageStatus += LogBookController_GetStageStatus;

        }

        private static bool LogBookController_IsEntryBodyWithoutLore(On.RoR2.UI.LogBook.LogBookController.orig_IsEntryBodyWithoutLore orig, ref Entry entry)
        {
            return true;
        }

        private static bool LogBookController_IsEntryPickupEquipmentWithoutLore(On.RoR2.UI.LogBook.LogBookController.orig_IsEntryPickupEquipmentWithoutLore orig, ref Entry entry)
        {
            return true;
        }

        private static bool LogBookController_IsEntryPickupItemWithoutLore(On.RoR2.UI.LogBook.LogBookController.orig_IsEntryPickupItemWithoutLore orig, ref Entry entry)
        {
            return true;
        }

        private static Entry[] LogBookController_BuildMonsterEntries(On.RoR2.UI.LogBook.LogBookController.orig_BuildMonsterEntries orig, System.Collections.Generic.Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            //Default filters out entries of similiar name so we should just overwrite it ig.
            Entry[] array = (from characterBody in BodyCatalog.allBodyPrefabBodyBodyComponents
                             orderby characterBody.baseMaxHealth
                             select characterBody).Select(delegate (CharacterBody characterBody)
                             {
                                 Entry entry2 = new Entry();
                                 //Debug.Log(characterBody);
                                 entry2.nameToken = characterBody.baseNameToken;
                                 entry2.color = ColorCatalog.GetColor(ColorCatalog.ColorIndex.HardDifficulty);
                                 entry2.iconTexture = characterBody.portraitIcon;
                                 entry2.extraData = characterBody;
                                 ModelLocator component = characterBody.GetComponent<ModelLocator>();
                                 GameObject modelPrefab;
                                 if (component == null)
                                 {
                                     modelPrefab = null;
                                 }
                                 else
                                 {
                                     Transform modelTransform = component.modelTransform;
                                     modelPrefab = ((modelTransform != null) ? modelTransform.gameObject : null);
                                 }
                                 entry2.modelPrefab = modelPrefab;
                                 entry2.getStatusImplementation = new Entry.GetStatusDelegate(LogBookController.GetMonsterStatus);
                                 entry2.getTooltipContentImplementation = new Entry.GetTooltipContentDelegate(LogBookController.GetMonsterTooltipContent);
                                 entry2.pageBuilderMethod = new Action<PageBuilder>(PageBuilder.MonsterBody);
                                 entry2.bgTexture = (characterBody.isChampion ? LegacyResourcesAPI.Load<Texture>("Textures/ItemIcons/BG/texTier3BGIcon") : LegacyResourcesAPI.Load<Texture>("Textures/ItemIcons/BG/texTier1BGIcon"));
                                 entry2.isWIPImplementation = new Entry.IsWIPDelegate(LogBookController.IsEntryBodyWithoutLore);
                                 return entry2;
                             }).ToArray<Entry>();
            return array;
        }

        private static bool LogBookController_CanSelectItemEntry(On.RoR2.UI.LogBook.LogBookController.orig_CanSelectItemEntry orig, ItemDef itemDef, System.Collections.Generic.Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            return itemDef != null;
        }
        private static bool LogBookController_CanSelectEquipmentEntry(On.RoR2.UI.LogBook.LogBookController.orig_CanSelectEquipmentEntry orig, EquipmentDef equipmentDef, System.Collections.Generic.Dictionary<RoR2.ExpansionManagement.ExpansionDef, bool> expansionAvailability)
        {
            return equipmentDef != null;
        }

        private static EntryStatus LogBookController_GetMonsterStatus(On.RoR2.UI.LogBook.LogBookController.orig_GetMonsterStatus orig, ref Entry entry, UserProfile viewerProfile)
        {
            return EntryStatus.Available;
        }

        private static EntryStatus LogBookController_GetPickupStatus(On.RoR2.UI.LogBook.LogBookController.orig_GetPickupStatus orig, ref RoR2.UI.LogBook.Entry entry, UserProfile viewerProfile)
        {
            return EntryStatus.Available;
        }
        private static EntryStatus LogBookController_GetStageStatus(On.RoR2.UI.LogBook.LogBookController.orig_GetStageStatus orig, ref Entry entry, UserProfile viewerProfile)
        {
            return EntryStatus.Available;
        }



    }

}