using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using static WolfoQoL_Client.Assets;

namespace WolfoQoL_Client
{

    public static class PingIcons
    {
        public static Sprite LunarIcon;
        //public static Sprite Chest_Large;
        //public static Sprite LunarCauldron;
        //public static Sprite LunarSeer;
        public static Sprite ExclamationIcon;
        //public static Sprite Chest_Legendary;
        //public static Sprite Chest_Lunar;
        public static Sprite NullVentIcon;
        //public static Sprite TimedChestIcon;
        public static Sprite PortalIcon;
        public static Sprite PrimordialTeleporterIcon;
        public static Sprite PrimordialTeleporterChargedIcon;
        public static Sprite CubeIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texLunarPillarIcon");
        //public static Sprite DroneIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texDroneIconOutlined");
        public static Sprite Chest_Small = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texInventoryIconOutlined");
        public static Sprite QuestionMarkIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texMysteryIcon");

        public static bool otherPingIconMods = false;
        public static Dictionary<string, Sprite> pingIconDict = new Dictionary<string, Sprite>();
        public static void SetAllStageInteractablePingIcons()
        {
            //Ideally a smaller list of types to check

            //-DroneScrapper
            //-Unused Junk

            //Chests are checked double because pickupController, but that is needed to be checked for so uhhh

            Type[] array = ChestRevealer.typesToCheck;


            for (int i = 0; i < array.Length; i++)
            {
                foreach (MonoBehaviour monoBehaviour in InstanceTracker.FindInstancesEnumerable(array[i]))
                {
                    //Debug.Log(monoBehaviour.gameObject.name.Split(" ")[0]);
                    //Debug.Log($"{monoBehaviour} | {monoBehaviour.gameObject.name}");
                    if (monoBehaviour.TryGetComponent<NetworkIdentity>(out var net) && net.sceneId.Value != 0)// && net.sceneId.Value != 0)
                    {
                        if (pingIconDict.TryGetValue(monoBehaviour.gameObject.name.Split(" ")[0], out Sprite sprite))
                        {
                            if (sprite == null)
                            {
                                monoBehaviour.gameObject.AddComponent<BlockScanner>();
                            }
                            if (monoBehaviour.TryGetComponent<PingInfoProvider>(out var ping))
                            {
                                ping.pingIconOverride = sprite;
                            }
                            else
                            {
                                monoBehaviour.gameObject.AddComponent<PingInfoProvider>().pingIconOverride = sprite;
                            }
                        }
                    }
                    //Debug.Log(monoBehaviour.gameObject.name + " | " + (net ? net.sceneId.Value : "-"));
                }
            }
        }


        public static void SetAllIcons(string obj, string sprite, bool setInspect = true)
        {
            SetAllIcons(Addressables.LoadAssetAsync<GameObject>(key: obj).WaitForCompletion(), Assets.Bundle.LoadAsset<Sprite>($"Assets/WQoL/PingIcons/{sprite}.png"), setInspect);

        }
        public static void SetAllIcons(string obj, Sprite sprite)
        {
            SetAllIcons(Addressables.LoadAssetAsync<GameObject>(key: obj).WaitForCompletion(), sprite, true);
        }
        public static void SetAllIcons(GameObject obj, Sprite sprite, bool setInspect = true)
        {
            if (!sprite)
            {
                WQoLMain.log.LogError($"{obj} trying to set NULL SPRITE");
                return;
            }

            if (obj.TryGetComponent<PingInfoProvider>(out var ping))
            {
                ping.pingIconOverride = sprite;
            }
            else
            {
                obj.AddComponent<PingInfoProvider>().pingIconOverride = sprite;
            }
            if (obj.TryGetComponent<SpecialObjectAttributes>(out var special))
            {
                special.portraitIcon = sprite.texture;
            }
            if (setInspect)
            {
                if (obj.TryGetComponent<GenericInspectInfoProvider>(out var inspect))
                {
                    inspect.InspectInfo.Info.Visual = sprite;
                }
                else if (obj.TryGetComponent<SummonMasterBehavior>(out var drone))
                {
                    if (drone.inspectDef)
                    {
                        drone.inspectDef.Info.Visual = sprite;
                    }
                }
                else if (obj.TryGetComponent<PickupDistributorBehavior>(out var pickupDistributorBehavior))
                {
                    if (pickupDistributorBehavior.inspectDef)
                    {
                        pickupDistributorBehavior.inspectDef.Info.Visual = sprite;
                    }
                }
            }
        }

        public static void Start()
        {
            otherPingIconMods = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("NotAJunkie.PingIconsOverhaul");

            if (otherPingIconMods)
            {
                return;
            }
         
            ExclamationIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/ExclamationIcon.png");
            PortalIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PortalIcon.png");


            Pings_Dictionary();
            Pings_Other();
            Pings_Chests();
            Pings_Shrines();
            Pings_Drones();
            Pings_Printer();
 
        }


        public static void Pings_Dictionary()
        {
            ////Annoying naming conventions for these:
            //Do Portals through Hooks
            //Do Newt Shrineks through Hooks


            //pingIconDict.Add("Chest1", Chest_Small);
            pingIconDict.Add("Chest2", Load("ChestLargeIcon"));
            pingIconDict.Add("Chest1StealthedVariant", Load("ChestInvisibleIcon"));
            pingIconDict.Add("GoldChest", Load("ChestLegendaryIcon"));
            pingIconDict.Add("LunarChest", Load("ChestLunarIcon"));
            pingIconDict.Add("Scrapper", Load("ScrapperIcon"));
            pingIconDict.Add("ShrineRestack", Load("ShrineOrderIcon"));
            pingIconDict.Add("TemporaryItemsShopTerminal", Load("DLC3/TempVendor"));



            ////Other Stage Specific
            //pingIconDict.Add("GLPressurePlate", Load(""));
            pingIconDict.Add("HumanFan", ExclamationIcon);
            pingIconDict.Add("FW_HumanFan", ExclamationIcon);
            pingIconDict.Add("TimedChest", Load("ChestTimedIcon"));
            //pingIconDict.Add("TreebotUnlockInteractable", QuestionMarkIcon);
            pingIconDict.Add("PortalDialer", QuestionMarkIcon); //Shows up as Chest
            //pingIconDict.Add("ChefWok", Load(""));
            pingIconDict.Add("NullSafeZone", Load("Void_Cell"));

            //pingIconDict.Add("MSObelisk", Load("")); //QuestionMark
            pingIconDict.Add("ShrineRebirth", Load("SotsIcon3_Shrunk"));
            //Geodes dont use spaces so would need to be found differently ig


            #region Bazaar & Moon
            pingIconDict.Add("LunarShopTerminal", Load("ChestLunarIcon"));
            pingIconDict.Add("LunarCauldron,", Load("LunarCauldron"));
            //pingIconDict.Add("LunarRecycler", ExclamationIcon);
            pingIconDict.Add("SeerStation", Load("LunarSeer"));
            //pingIconDict.Add("LockedMage", Load(""));

            pingIconDict.Add("MoonBatteryMass", CubeIcon);
            pingIconDict.Add("MoonBatteryBlood", CubeIcon);
            pingIconDict.Add("MoonBatterySoul", CubeIcon);
            pingIconDict.Add("MoonBatteryDesign", CubeIcon);
            //pingIconDict.Add("MoonElevator", ExclamationIcon);

            #endregion
            #region AC Junk
            pingIconDict.Add("Access", Load("DLC3/Access_Node"));
            pingIconDict.Add("PowerPedestal_Pyramid", Load("DLC3/Power_Pyramid2"));
            pingIconDict.Add("PowerPedestal_Cube", Load("DLC3/Power_Cube2"));
            pingIconDict.Add("CoinSlotStatue_SH_PostBossPuzzle", ExclamationIcon);
            pingIconDict.Add("DrifterBagChest", Load("ScavBagIcon"));
            pingIconDict.Add("BrokenHAND", QuestionMarkIcon);
            //pingIconDict.Add("ShrineCombatCollective", Load(""));
            //pingIconDict.Add("Teleporter_ConduitCanyonVariant", Load(""));

            #endregion
            #region Computational Exchange & AC Stages
            pingIconDict.Add("DroneCombinerStation", Load("DLC3/Drone__Combiner"));
            pingIconDict.Add("DroneScrapper", Load("DLC3/Drone__Scrapper"));
            pingIconDict.Add("MealPrep", Load("DLC3/Meal_Station"));
            pingIconDict.Add("VendingMachine", Load("DLC1/VendingMachine"));
            pingIconDict.Add("SolusVendorShrine2", QuestionMarkIcon);
            pingIconDict.Add("Duplicator", Load("Printer"));
            pingIconDict.Add("DuplicatorLarge", Load("PrinterLarge"));
            pingIconDict.Add("DuplicatorMilitary", Load("PrinterMili"));
            pingIconDict.Add("DuplicatorWild", Load("PrinterWild"));

            pingIconDict.Add("LemurianEgg", Load("LemurianEgg"));

            //pingIconDict.Add("Drone1Broken", Load(""));
            pingIconDict.Add("Drone2Broken", Load("DroneHealIcon"));
            pingIconDict.Add("HaulerDroneBroken", Load("DLC3/Drone_Transport"));
            pingIconDict.Add("JunkDroneBroken", Load("DLC3/Drone_Junk"));
            pingIconDict.Add("MissileDroneBroken", Load("DroneMissileIcon"));
            pingIconDict.Add("RechargeDroneBroken", Load("DLC3/Drone_Barrier"));
            pingIconDict.Add("JailerDroneBroken", Load("DLC3/Drone_Jailer"));
            pingIconDict.Add("CleanupDroneBroken", Load("DLC3/Drone_Cleanup"));
            pingIconDict.Add("CopycatDroneBroken", Load("DLC3/Drone_Freeze"));
            pingIconDict.Add("BombardmentDroneBroken", Load("DLC3/Drone_Bomber"));
            pingIconDict.Add("MegaDroneBroken", Load("DroneMegaIcon"));
            #endregion

        }

        public static void Pings_Chests()
        {
            SetAllIcons("19deacd9e155c044595f2c344e110dde", Chest_Small); //Chest1
            SetAllIcons("f460ead3fd64fe0408ec760850cf3301", "ChestLargeIcon"); //Chest2
            SetAllIcons("e4f9d194b928e2943962a39ad25cc9b7", "ChestLegendaryIcon"); //GoldChest

            SetAllIcons("7886981945e868f43852dfd0a6832ae2", "ChestCategoryDamage");
            SetAllIcons("624679952282b144a8ad63c93b0d8abc", "ChestCategoryHealing");
            SetAllIcons("6722ebff89fcfc94e84212a1ba51d46a", "ChestCategoryUtility");

            SetAllIcons("RoR2/DLC1/CategoryChest2/CategoryChest2Damage Variant.prefab", "ChestCategory2Damage");
            SetAllIcons("RoR2/DLC1/CategoryChest2/CategoryChest2Healing Variant.prefab", "ChestCategory2Healing");
            SetAllIcons("RoR2/DLC1/CategoryChest2/CategoryChest2Utility Variant.prefab", "ChestCategory2Utility");

            SetAllIcons("26aed741113c79c49806d3144ba32731", "ChestLunarIcon");
            SetAllIcons("70ee08f9f87fa3e41bb3dd8efb1c363f", "ChestEquipIcon");
            SetAllIcons("fd68bfe758b25714288565a552d40d83", "ChestCasinoIcon");
            SetAllIcons("7d97551ee4c1e904b9a225abc63c3f0d", "ChestInvisibleIcon");

            SetAllIcons("e82b1a3fea19dfd439109683ce4a14b7", "ChestVoid");
            SetAllIcons("341c368b343432b4dbe8ff30752fbbb4", "ChestVoidPotential");

            SetAllIcons("RoR2/Base/TreasureCache/Lockbox.prefab", "LockedIcon");
            SetAllIcons("RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab", "LockedIconAlt");

            //SetAllIcons("RoR2/DLC1/FreeChestMultiShop/FreeChestMultiShop.prefab", "PingIconShippingDrone");
            SetAllIcons("RoR2/DLC1/FreeChestTerminalShippingDrone/FreeChestTerminalShippingDrone.prefab", "PingIconShippingDrone");

            SetAllIcons("e6f68d343e0ec6645986c7af57417bee", "ScavBagIcon");
            SetAllIcons("269db96d60fd1df49a685e708d1aa81f", "ScavBagIcon"); //Lunar
            SetAllIcons("3e5352137390b2e4a8ced6c62df6be34", "ScavBagIcon"); //Drifter Temp Chest

            SetAllIcons("664d4964dd9d5cf408168f09b05a0cb3", "ChestTimedIcon");

            SetAllIcons("063fc18c1e1408b42a2c4aa3a789e906", "TrippleShop_Small");   //Terminal
            SetAllIcons("9eb9c2d7c9f1cb54c9aee8bb625259f5", "TrippleShop_Small");   //Shop
            SetAllIcons("c87d4eb171e0ee44cb2d4ec714c58c72", "TrippleShop_Large");
            SetAllIcons("c814170ba819d7b45934c0fc54fd99d1", "TrippleShop_Large");
            SetAllIcons("e992781bd4b52804c8d340a56db49a06", "TrippleShop_Equipment");
            SetAllIcons("63251d66a257b1a4c8eed4de5dd89561", "TrippleShop_Equipment");

            SetAllIcons("d31df5066858329458b33f21b3b22d2e", "DLC3/TempVendor");

        }

        public static void Pings_Shrines()
        {
            SetAllIcons("da96cf099344e8b4fa0f8198af665244", "ShrineMountainIcon");
            SetAllIcons("ae1455033c65c694e9abf728a10418b2", "ShrineMountainIcon");
            SetAllIcons("f7bb8988c277bc740bbc041c09dc81fd", "ShrineMountainIcon");

            SetAllIcons("afcf09fce0fd504498c42cc15c6e77ef", "ShrineBloodIcon");
            SetAllIcons("b77af4163bbc41549916df3bdfed560e", "ShrineBloodIcon");
            SetAllIcons("d7851739065b43748a1b1e22e5280558", "ShrineBloodIcon");

            SetAllIcons("8a681654848ac374980fea55c4cf55a7", "ShrineChanceIcon");
            SetAllIcons("ccb461071fd3f6d4c9bc0bd96af47721", "ShrineChanceIcon");
            SetAllIcons("a232aa071f626734d9e6349ef4f984ca", "ShrineChanceIcon");

            SetAllIcons("d59b10d76f9ca41489a04053300576c1", "ShrineOrderIcon");
            SetAllIcons("7cb2ac171b983124b851c8b883e6754f", "ShrineOrderIcon");
            SetAllIcons("36f43f9ec00ade748927e89df4b0e743", "ShrineOrderIcon");

            SetAllIcons("9f0ac77d429ca5d40b3870b5feaec1b5", "ShrineHealIcon");

            SetAllIcons("3e31184a8e111c54b9ea20fbcb67ee49", "ShrineGoldIcon");

            SetAllIcons("f521acffba61ae040a5cbd40c7d30141", "CleanseIcon");
            SetAllIcons("52e1e53e0ae52184897f5b940b270326", "CleanseIcon");
            SetAllIcons("7a4808b55ec828748bd6f8820c5b9faf", "CleanseIcon");

            SetAllIcons("50f9b823c49c5a8468dadcd770415e2f", "ShrineShaping");
            SetAllIcons("c10fd181efcffc24f8fed4c2f246fac8", "SotsIcon3_Shrunk");
            SetAllIcons("59244efe276f7bb47ae34fbbb0095b5f", "SotsIcon3_Shrunk"); //Rebirth


        }

        public static void Pings_Drones()
        {
            SetAllIcons("60e08168b0a17fd43a94b7493175608a", "DroneTurretIcon");
            SetAllIcons("61c9fc1ae1d9c54489ed062b6aee78f2", "DroneEquipmentIcon");

            SetAllIcons("e4fe5cc222934f043a63cafe4e9a7960", "DroneHealIcon");
            SetAllIcons("5d5f4ad58811263418cfe7e5e6fca555", "DroneEmergencyIcon");

            SetAllIcons("991dfa1354cf4aa4ca3758e8f0745312", "DroneMissileIcon");
            SetAllIcons("d9979d4c99d42ad4886f4dc13ef72b30", "DroneFlameIcon");
            SetAllIcons("5891e07c07cb11141ac34a5cd55e51ee", "DroneMegaIcon");


            SetAllIcons("bfc3ebeae2b68bd458bbf0059301053a", "DLC3/Drone_Transport");
            SetAllIcons("8e572d3d605684e408cc5480c24171b4", "DLC3/Drone_Junk");

            SetAllIcons("9842714e20c0b0e4696db39caa97dd07", "DLC3/Drone_Barrier");
            SetAllIcons("57a1adb4e1dfda540b9097b6bcbf3488", "DLC3/Drone_Cleanup");
            SetAllIcons("c8b005f303334c040b7011f563c4bfc9", "DLC3/Drone_Jailer");

            SetAllIcons("1452f88e28bf6524897c9abf2fabb9a5", "DLC3/Drone_Bomber");
            SetAllIcons("3e7219df186245c49a3d9f622ffe1955", "DLC3/Drone_Freeze");

            SetAllIcons("d3a49e0172a17c847aea4585b65cd418", "DLC3/Drone__Shop");

        }

        public static void Pings_Printer()
        {
            SetAllIcons("f21b2c8a9cc028046935ea871dc4af54", "PrinterLarge");
            SetAllIcons("8a8411ca0d2c7de4f8f5e5f85a91a8f7", "Printer");
            SetAllIcons("5a46385c842a0cd4990bc33c0237dda4", "PrinterMili");
            SetAllIcons("c1498d4e50ce815458de7d84f1e903c3", "PrinterWild");

            SetAllIcons("f3f941fba20ea0a45bfad9d1b78474b7", "ScrapperIcon");

            SetAllIcons("60048e6f3cc383a418818ae8315ff8b3", "DLC3/Drone__Combiner");
            SetAllIcons("ce12b07631d644a4aa9b5191ed1956f5", "DLC3/Drone__Scrapper");

            SetAllIcons("b2eb207c2edb18b459a03c271b97be17", "LunarCauldron");
            SetAllIcons("277e9808d2bf40f45a4732c2ef8a5070", "LunarCauldron");
            SetAllIcons("205b0f1b9c2a8de47945946eca80e564", "LunarCauldron");

        }
   
        public static void Pings_Other()
        {
            NullVentIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/Void_Cell.png");
            //LunarSeer = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/LunarSeer.png");
            PrimordialTeleporterIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PrimordialTeleporterIcon.png");
            PrimordialTeleporterChargedIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PrimordialTeleporterChargedIcon.png");
            LunarIcon = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/LunarIcon.png");

            Sprite VoidDeepSymbol = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC1/DeepVoidPortalBattery/texDeepVoidPortalBatteryIcon.png").WaitForCompletion();
            Sprite texBarrelIcon = Addressables.LoadAssetAsync<Sprite>(key: "ccf87dcf9daaa0645a94a7e14fa5749a").WaitForCompletion();
            Sprite ShipPodIcon = LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texRescueshipIcon");
            Sprite VoidCrab = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/ITAssets/texInfiniteTowerSafeWardIcon.png").WaitForCompletion();

            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").AddComponent<BlockScanner>();

           

            SetAllIcons("06d0621d2cadf98439f357fe148e7791", VoidDeepSymbol); //VoidSuppressor
            SetAllIcons("8a6a0189c5090eb4bab48bd10e3074ea", "Void_Cell");
            SetAllIcons("09ebe2eaaf93ad640a8d505eb6d7f5b6", VoidDeepSymbol);//DeepVoidPortalBattery
            SetAllIcons("e515327d3d5e0144488357748ce1e899", "VoidIcon");//VoidCamp
            SetAllIcons("ba70f68b08210a84f86ff7a12a324263", VoidCrab);
            SetAllIcons("9ff23f803358e9d42a7d7c4c1200b6f7", VoidCrab);

            SetAllIcons("aa803cc8277f3b54fafd988a6fbabe64", "LunarSeer");

            SetAllIcons("b790659c0441e464e8921a2b0c536acb", CubeIcon); //Pillars
            SetAllIcons("aa2c395b0fc2d3b4e8102691b5da81af", CubeIcon);
            SetAllIcons("34a1c551103d9214aab8ded73405b0d3", CubeIcon);
            SetAllIcons("b3bcad23353b0434abbab25614098026", CubeIcon);

            SetAllIcons("019dc92c7d183e24bb69db690bbcff90", "RadarTower", false);

            SetAllIcons("452b7f73a60721b4eb546e2efe29d6d2", "LemurianEgg");

            SetAllIcons("ac7f5a5e3c6a70a4f88a2615b30653d1", "DLC1/VendingMachine");

            SetAllIcons("452b7f73a60721b4eb546e2efe29d6d2", "LemurianEgg");

            Addressables.LoadAssetAsync<GameObject>(key: "be6e220724818e54493d5447cc8caa4e").WaitForCompletion().GetComponent<SpecialObjectAttributes>().portraitIcon = texBarrelIcon.texture;
            SetAllIcons("3a7dc0d37d5875c4a9eafc8d7097c000", "DLC1/VoidStalk");


            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/QuestVolatileBatteryWorldPickup").AddComponent<PingInfoProvider>().pingIconOverride = ShipPodIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/SurvivorPodBatteryPanel").AddComponent<PingInfoProvider>().pingIconOverride = ShipPodIcon;



            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporter Variant").AddComponent<PingInfoProvider>().pingIconOverride = PrimordialTeleporterIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/LunarTeleporterProngs").AddComponent<PingInfoProvider>().pingIconOverride = PrimordialTeleporterChargedIcon;



            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/HumanFan").AddComponent<BlockScanner>();

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/GoldshoresBeacon").GetComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Geode.prefab").WaitForCompletion().AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Healing").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Shocking").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, EquipmentRestock").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/captainsupplydrops/CaptainSupplyDrop, Hacking").AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;

        }

        public static void ModSupport()
        {

            //Seems to NOT find vanilla spawn cards which is good
            InteractableSpawnCard[] ISCList = Resources.FindObjectsOfTypeAll(typeof(InteractableSpawnCard)) as InteractableSpawnCard[];
            for (var i = 0; i < ISCList.Length; i++)
            {
                //WolfoMain.log.LogWarning(ISCList[i]);
                switch (ISCList[i].name)
                {
                    case "iscDroneTable":
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/SS2/SS2_DroneScrapper.png");
                        break;
                    case "iscCloneDrone":
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/SS2/SS2_DroneDuplicate.png");
                        break;
                    case "iscShockDrone":
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/SS2/SS2_DroneShock.png");
                        break;
                    case "iscTripleShopRed":
                        GameObject TerminalE = ISCList[i].prefab.GetComponent<MultiShopController>().terminalPrefab;
                        TerminalE.AddComponent<DifferentIconScanner>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/MultiShop3_Shrunk.png");
                        //TerminalE.GetComponent<DifferentIconScanner>().scannerIconOverride = LegendaryChestIcon;
                        break;

                    /*case "iscCloakedShrine":
                    case "iscCloakedShrineSnowy":
                    case "iscCloakedShrineSandy":
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SpStShrineCloakedS;
                        break;
                    case "iscAegisShrine":
                    case "iscAegisShrineSnowy":
                    case "iscAegisShrineSandy":
                        ISCList[i].prefab.AddComponent<PingInfoProvider>().pingIconOverride = SpStShrineAegisS;
                        break;*/
                    case "iscVoidPortalInteractable":
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/PingIcons/PingIconVoidEradicator.png");
                        break;
                    case "iscWhorlCellInteractable":
                        ISCList[i].prefab.GetComponent<PingInfoProvider>().pingIconOverride = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/DLC1/DeepVoidPortalBattery/texDeepVoidPortalBatteryIcon.png").WaitForCompletion();
                        break;

                }
            }

            //This is stupidd idk how else to find stuff like Broken Han-d
            //WolfoMain.log.LogWarning(RedMercSkin.MercSwordSlashRed.transform.parent);
            RoR2.ModelLocator.DestructionNotifier[] moddedPurchasesList = Resources.FindObjectsOfTypeAll(typeof(RoR2.ModelLocator.DestructionNotifier)) as RoR2.ModelLocator.DestructionNotifier[];
            for (var i = 0; i < moddedPurchasesList.Length; i++)
            {
                //WolfoMain.log.LogWarning(moddedPurchasesList[i]);
                switch (moddedPurchasesList[i].name)
                {
                    case "BrokenJanitorInteractable":
                    case "BrokenJanitorRepair":
                        moddedPurchasesList[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = ExclamationIcon;
                        break;
                }
            }

        }

    }

}
