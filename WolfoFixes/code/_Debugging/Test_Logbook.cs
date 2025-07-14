using RoR2;
using RoR2.UI.LogBook;
using System;
using System.Linq;
using UnityEngine;


namespace WolfoFixes.Testing
{

    internal class Test_Logbook
    {


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