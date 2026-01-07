using HG;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoQoL_Client.ModSupport;
using WolfoQoL_Client.Reminders;
using WolfoQoL_Client.Skins;

namespace WolfoQoL_Client
{

    public static partial class PingIcons
    {
        public static void ModSupport()
        {
            //Idk why where not just allowed to get the path directly.    
            Transform rapiRoot = Merc_Red.MercSwordSlash_Red.transform.parent;
            /*Transform hand = rapiRoot.Find("BrokenJanitorInteractable");
            if (hand)
            {
                hand.gameObject.AddComponent<PingInfoProvider>().pingIconOverride = QuestionMarkIcon;
            }*/

            if (QualitySupport.QualityModInstalled)
            {
                ModPing_Quality();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.TeamMoonstorm"))
            {
                ModPing_SS2();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Zenithrium.vanillaVoid"))
            {
                ModPing_VV();
            }
            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Viliger.EnemiesReturns"))
            {
                ModPing_EnemyReturns();
            }
        }


        public static void ModPing_EnemyReturns()
        {
            SetAllIcons(EnemiesReturns.Enemies.MechanicalSpider.MechanicalSpiderStuff.InteractablePrefab, "EnemyReturn/Drone_Spider");
            SetAllIcons(EnemiesReturns.Enemies.LynxTribe.LynxTribeStuff.LynxShrine1, "EnemyReturn/ShrineLynx");
            SetAllIcons(EnemiesReturns.Enemies.LynxTribe.LynxTribeStuff.LynxShrine2, "EnemyReturn/ShrineLynx");
            SetAllIcons(EnemiesReturns.Enemies.LynxTribe.LynxTribeStuff.LynxShrine3, "EnemyReturn/ShrineLynx");

            EnemiesReturns.Enemies.Judgement.SetupJudgementPath.PileOfDirt.GetComponent<GenericInteraction>().shouldShowOnScanner = true;
            EnemiesReturns.Enemies.Judgement.SetupJudgementPath.PileOfDirt.EnsureComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
        }
        public static void ModPing_SS2()
        {
            SetAllIcons(SS2.SS2Assets.LoadAsset<SS2.InteractableAssetCollection>("acShockDrone", SS2.SS2Bundle.Interactables).interactablePrefab, "SS2/DroneShock");
            SetAllIcons(SS2.SS2Assets.LoadAsset<SS2.InteractableAssetCollection>("acCloneDrone", SS2.SS2Bundle.Interactables).interactablePrefab, "SS2/DroneDuplicator");
            SetAllIcons(SS2.SS2Assets.LoadAsset<SS2.InteractableAssetCollection>("acDroneTable", SS2.SS2Bundle.Interactables).interactablePrefab, "SS2/DroneTable");
            SetAllIcons(SS2.SS2Assets.LoadAsset<SS2.InteractableAssetCollection>("acLunarGambler", SS2.SS2Bundle.Interactables).interactablePrefab, ExclamationIcon);
            SetAllIcons(SS2.EtherealBehavior.shrinePrefab, "SS2/Shrine_Ethereal");

            SS2.Components.VoidBehavior.rockPrefab.EnsureComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
        }
        public static void ModPing_VV()
        {
            //Shattered Monolith isn't in ModdedPrefabs so fuck it keep it like this
            if (vanillaVoid.Interactables.ShatteredMonolith.InteractableBodyModelPrefab)
            {
                //Idk how someone had this as null, that might not be right 
                SetAllIcons(vanillaVoid.Interactables.ShatteredMonolith.InteractableBodyModelPrefab, "PingIconVoidEradicator");
            }
            if (vanillaVoid.Items.VoidShell.PortalBattery)
            {
                SetAllIcons(vanillaVoid.Items.VoidShell.PortalBattery, Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC1/DeepVoidPortalBattery/texDeepVoidPortalBatteryIcon.png").WaitForCompletion());
                vanillaVoid.Items.VoidShell.PortalBattery.AddComponent<SpawnListener>().interactable = SpawnListener.Interactable.vvVoidFreeChest;
            }

        }
        public static void ModPing_Quality()
        {
            SetAllIcons(ItemQualities.ItemQualitiesContent.SpawnCards.QualityChest1.prefab, "Quality/Quality_Chest");
            SetAllIcons(ItemQualities.ItemQualitiesContent.SpawnCards.QualityChest2.prefab, "Quality/Quality_Chest2");
            SetAllIcons(ItemQualities.ItemQualitiesContent.SpawnCards.QualityEquipmentBarrel.prefab, "Quality/Quality_Equip");
            SetAllIcons(ItemQualities.ItemQualitiesContent.SpawnCards.QualityDuplicator.prefab, "Quality/Quality_Printer");
            ItemQualities.ItemQualitiesContent.SpawnCards.QualityDuplicatorLarge.prefab.AddComponent<SpawnListener>().interactable = SpawnListener.Interactable.greenPrinterQuality;
            SetAllIcons(ItemQualities.ItemQualitiesContent.SpawnCards.QualityDuplicatorLarge.prefab, "Quality/Quality_PrinterLarge");
            SetAllIcons(ItemQualities.ItemQualitiesContent.SpawnCards.QualityDuplicatorMilitary.prefab, "Quality/Quality_PrinterMili");
            SetAllIcons(ItemQualities.ItemQualitiesContent.SpawnCards.QualityDuplicatorWild.prefab, "Quality/Quality_PrinterWild");

        }


    }

}
