using HG;
using RoR2;
using RoR2.ExpansionManagement;
using RoR2.UI.LogBook;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoFixes;


namespace WolfoQoL_Client
{

    public class LogbookStuff
    {

        public static void Start()
        {
            On.RoR2.UI.LogBook.LogBookController.BuildMonsterEntries += AddMissingMonsters;
            On.RoR2.UI.LogBook.LogBookController.BuildMonsterEntries += SortBossMonster;
            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += AddMissingEquipment;
            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += SortBossEquip_AddBG;
            On.RoR2.UI.LogBook.LogBookController.BuildSurvivorEntries += SurvivorEntryColor;

            On.RoR2.UI.LogBook.LogBookController.GetPickupStatus += EliteEquipmentViewable;

            Logbook_Allies.MoreStats();
            Logbook_MoreStats.MoreStats();
            SceneCatalog.availability.CallWhenAvailable(LogSimuStages.Add);
            #region Sort Hidden Realms

            Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC2/meridian/meridian.asset").WaitForCompletion().stageOrder = 80;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/goldshores").stageOrder = 81; //Next to Meridian

            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/bazaar").stageOrder = 90; //Bazaar -> Void -> Void -> Void
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/arena").stageOrder = 91;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/voidstage").stageOrder = 92;
            Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC1/voidraid/voidraid.asset").WaitForCompletion().stageOrder = 93;

            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/mysteryspace").stageOrder = 150;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/limbo").stageOrder = 151;

            Addressables.LoadAssetAsync<SceneDef>(key: "808fe615086cfa042baf950436b1da20").WaitForCompletion().stageOrder = 95; //AC1
            Addressables.LoadAssetAsync<SceneDef>(key: "55dcf3dbd137f99458af33ad467bb574").WaitForCompletion().stageOrder = 96; //AC2
            Addressables.LoadAssetAsync<SceneDef>(key: "5272c5e76a99aa046b72ad8fb941974c").WaitForCompletion().stageOrder = 97; //AC3

            #endregion
        }

        private static EntryStatus EliteEquipmentViewable(On.RoR2.UI.LogBook.LogBookController.orig_GetPickupStatus orig, ref Entry entry, UserProfile viewerProfile)
        {
            var temp = orig(ref entry, viewerProfile);
            if (temp == EntryStatus.Available)
            {
                return temp;
            }
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
            return temp;
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

            UnlockableDef Log_Gup = DLC1Content.BodyPrefabs.GupBody.GetComponent<DeathRewards>().logUnlockableDef;
            DLC1Content.BodyPrefabs.GeepBody.GetComponent<DeathRewards>().logUnlockableDef = Log_Gup;
            DLC1Content.BodyPrefabs.GipBody.GetComponent<DeathRewards>().logUnlockableDef = Log_Gup;

            UnlockableDef Log_SolusAmalgam = DLC3Content.BodyPrefabs.SolusAmalgamatorBody.GetComponent<DeathRewards>().logUnlockableDef;
            DLC3Content.BodyPrefabs.SolusAmalgamatorFlamethrowerCannonBody.GetComponent<DeathRewards>().logUnlockableDef = Log_SolusAmalgam;
            DLC3Content.BodyPrefabs.SolusAmalgamatorMissilePodBody.GetComponent<DeathRewards>().logUnlockableDef = Log_SolusAmalgam;
            DLC3Content.BodyPrefabs.SolusAmalgamatorThrusterBody.GetComponent<DeathRewards>().logUnlockableDef = Log_SolusAmalgam;

            UnlockableDef Log_MinePod = DLC3Content.BodyPrefabs.MinePodBody.GetComponent<DeathRewards>().logUnlockableDef;
            DLC3Content.BodyPrefabs.SolusMineBody.GetComponent<DeathRewards>().logUnlockableDef = Log_MinePod;


            RoR2Content.BodyPrefabs.ScavLunar1Body.baseNameToken = "SCAVLUNAR_BODY_SUBTITLE";
            RoR2Content.BodyPrefabs.ScavLunar1Body.GetComponent<DeathRewards>().logUnlockableDef = LegacyResourcesAPI.Load<UnlockableDef>("UnlockableDefs/Logs.Stages.limbo"); ;

            UnlockableDef Unlock_Loop = LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Items.Clover");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/UrchinTurretBody").EnsureComponent<DeathRewards>().logUnlockableDef = Unlock_Loop;

            GameObject newtBody = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody");
            Transform mdlNewt = newtBody.transform.GetChild(0).GetChild(0);
            ModelPanelParameters Newt = mdlNewt.gameObject.EnsureComponent<ModelPanelParameters>();
            var AAAA = mdlNewt.gameObject.EnsureComponent<InstantiateModelParams>();
            Newt.minDistance = 5;
            Newt.maxDistance = 15;
            AAAA.CameraPosition = new Vector3(-1.5f, 4f, 3f);
            AAAA.FocusPosition = new Vector3(0f, 3f, 0f);


            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/bazaar").dioramaPrefab = mdlNewt.gameObject;

            Entry[] entries_TEMP = orig(expansionAvailability);
            RoR2Content.BodyPrefabs.ScavLunar1Body.baseNameToken = "SCAVLUNAR1_BODY_NAME";
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
                    if (WConfig.cfgColorMain.Value)
                    {
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

    }

}