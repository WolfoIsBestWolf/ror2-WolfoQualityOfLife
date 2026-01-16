using HG;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static WolfoQoL_Client.Assets;

namespace WolfoQoL_Client
{
    public static partial class PingIcons
    {
        public static void MiscIcons()
        {      
            On.EntityStates.Missions.Arena.NullWard.Active.OnEnter += VoidCell_Inidicator;
            On.EntityStates.Missions.Arena.NullWard.Complete.OnEnter += VoidCell_DestroyIndicator;

            //Add providers to loot
            Addressables.LoadAssetAsync<GameObject>(key: "0db1724ef7010364b87726287e1c2088").WaitForCompletion().AddComponent<PingInfoProvider>(); //GenericPickup
            Addressables.LoadAssetAsync<GameObject>(key: "d9c642aeefb6c974da664933489071d4").WaitForCompletion().AddComponent<PingInfoProvider>(); //GenericPickup
            if (WConfig.cfgLootIcons.Value)
            {
                PickupCatalog.availability.CallWhenAvailable(MakePickupSpriteDict);
                On.RoR2.GenericPickupController.SyncPickupState += PingIconsForItems;
                On.RoR2.PickupIndexNetworker.SyncPickupState += PingIconsForCommandCubes;
            }

            //Addressables.LoadAssetAsync<SkillDef>(key: "7a8a37bdc572a7c40847661a5820ea39").WaitForCompletion().icon = Bundle.LoadAsset<Sprite>($"Assets/WQoL/SkillIcons/texDTGunnerIcon.png");
            //Addressables.LoadAssetAsync<SkillDef>(key: "366c4e73cd50a4a42ae7ec8d1d698f2c").WaitForCompletion().icon = Bundle.LoadAsset<Sprite>($"Assets/WQoL/SkillIcons/texDTHealingDroneIcon.png");
            //Addressables.LoadAssetAsync<SkillDef>(key: "684a9c8e7ddd0cf4fb10e40aedfa2561").WaitForCompletion().icon = Bundle.LoadAsset<Sprite>($"Assets/WQoL/SkillIcons/texDTHaulerDroneIcon.png");
        }

        private static void MakePickupSpriteDict()
        {
            pickupToPingIcon = new Dictionary<PickupIndex, Sprite>();

            pickupToPingIcon.Add(PickupCatalog.FindPickupIndex(RoR2Content.MiscPickups.LunarCoin.miscPickupIndex), Load("LunarCoin"));
            pickupToPingIcon.Add(PickupCatalog.FindPickupIndex(DLC1Content.MiscPickups.VoidCoin.miscPickupIndex), Load("VoidCoin"));

            pickupToPingIcon.Add(PickupCatalog.FindPickupIndex(DLC3Content.Items.PowerPyramid.itemIndex), Load("DLC3/Power_Pyramid2Shrunk"));
            pickupToPingIcon.Add(PickupCatalog.FindPickupIndex(DLC3Content.Items.PowerCube.itemIndex), Load("DLC3/Power_Cube2Shrunk"));
            pickupToPingIcon.Add(PickupCatalog.FindPickupIndex(DLC3Content.Items.MasterBattery.itemIndex), Load("DLC3/Power_Orb2"));
            pickupToPingIcon.Add(PickupCatalog.FindPickupIndex(DLC3Content.Items.MasterCore.itemIndex), Load("DLC3/Power_Orb2"));
        }

        public static Dictionary<PickupIndex, Sprite> pickupToPingIcon;

        private static void PingIconsForCommandCubes(On.RoR2.PickupIndexNetworker.orig_SyncPickupState orig, PickupIndexNetworker self, UniquePickup newPickupState)
        {
            orig(self, newPickupState);
            //Null Check icon to not override Geode/Potential
            if (self.TryGetComponent<PingInfoProvider>(out var ping) && ping.pingIconOverride == null)
            {
                ping.pingIconOverride = PingIconFromPickupIndex(newPickupState);
            }
        }

        public static Sprite PingIconFromPickupIndex(UniquePickup pickup)
        {
            if (pickup.pickupIndex == PickupIndex.none)
            {
                return null;
            }
            if (pickupToPingIcon.TryGetValue(pickup.pickupIndex, out var icon))
            {
                return icon;
            }
            if (pickup.isTempItem)
            {
                return Load("Loot_TempItem2");
            }
            PickupDef def = PickupCatalog.GetPickupDef(pickup.pickupIndex);
            if (def == null)
            {
                return null;
            }
            if (def.equipmentIndex != EquipmentIndex.None)
            {
                EquipmentDef equipDef = EquipmentCatalog.GetEquipmentDef(def.equipmentIndex);
                if (equipDef && equipDef.passiveBuffDef && equipDef.passiveBuffDef.eliteDef)
                {
                    return Load("Loot_Aspect");
                }
                //Aspect?
                return Load("Loot_Equipment");
            }
            else if (def.itemIndex != ItemIndex.None)
            {
                ItemDef itemDef = ItemCatalog.GetItemDef(def.itemIndex);
                if (itemDef)
                {
                    if (WQoLMain.ZetAspects)
                    {
                        if (ModPing_ZetAspects(itemDef))
                        {
                            return Load("Loot_Aspect");
                        }                    
                    }
                    /*ItemTierDef itemTierDef = ItemTierCatalog.GetItemTierDef(itemDef.tier);
                    if (itemTierDef && (itemTierDef.pickupRules != ItemTierDef.PickupRules.Default))
                    {
                        return Load("Loot_AltTier");
                    }*/
                }
            }   
            else if (def.droneIndex != DroneIndex.None)
            {
                //Does anyone even use this.
                DroneDef droneDef = DroneCatalog.GetDroneDef(def.droneIndex);
                if (droneDef && droneDef.droneBrokenSpawnCard)
                {
                    PingInfoProvider pingInfoProviderDrone = droneDef.droneBrokenSpawnCard.prefab.GetComponent<PingInfoProvider>();
                    if (pingInfoProviderDrone)
                    {
                        return pingInfoProviderDrone.pingIconOverride;
                    }
                    else
                    {
                        return LegacyResourcesAPI.Load<Sprite>("textures/miscicons/texDroneIconOutlined");
                    }
                }
            }
            return null;
        }

        private static void PingIconsForItems(On.RoR2.GenericPickupController.orig_SyncPickupState orig, GenericPickupController self, UniquePickup newPickupState)
        {
            orig(self, newPickupState);
            if (self.TryGetComponent<PingInfoProvider>(out var ping))
            {
                ping.pingIconOverride = PingIconFromPickupIndex(newPickupState);
            }
        }



        private static void VoidCell_DestroyIndicator(On.EntityStates.Missions.Arena.NullWard.Complete.orig_OnEnter orig, EntityStates.Missions.Arena.NullWard.Complete self)
        {
            orig(self);

            Transform Indicator = self.transform.Find("NullCellPositionIndicator");
            if (Indicator != null)
            {
                GameObject.Destroy(Indicator.gameObject);
            }

            HoldoutZoneController.sharedColorPropertyBlock.SetColor("_TintColor", Color.white);
            self.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().SetPropertyBlock(HoldoutZoneController.sharedColorPropertyBlock);

        }

        private static void VoidCell_Inidicator(On.EntityStates.Missions.Arena.NullWard.Active.orig_OnEnter orig, EntityStates.Missions.Arena.NullWard.Active self)
        {
            orig(self);
            if (WConfig.cfgVoidFieldsChargeIndicator.Value)
            {
                HoldoutZoneController holdoutZone = self.outer.GetComponent<HoldoutZoneController>();

                GameObject newIndicator = Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/PositionIndicators/PillarChargingPositionIndicator"));
                newIndicator.name = "NullCellPositionIndicator";
                newIndicator.GetComponent<PositionIndicator>().targetTransform = self.outer.transform;
                newIndicator.transform.SetParent(self.outer.transform, false);


                ChargeIndicatorController NullCell = newIndicator.GetComponent<ChargeIndicatorController>();
                NullCell.holdoutZoneController = holdoutZone;
                NullCell.spriteBaseColor = new Color(0.915f, 0.807f, 0.915f);
                NullCell.spriteChargedColor = new Color(0.977f, 0.877f, 0.977f);
                NullCell.spriteChargingColor = new Color(0.943f, 0.621f, 0.943f);
                NullCell.spriteFlashColor = new Color(0.92f, 0.411f, 0.92f);
                NullCell.textBaseColor = new Color(0.858f, 0.714f, 0.858f);
                NullCell.textChargingColor = new Color(1f, 1f, 1f);
                if (NullCell.iconSprites.Length > 0)
                {
                    NullCell.iconSprites[0].sprite = PingIcons.NullVentIcon;
                }

                holdoutZone.baseIndicatorColor = new Color(1, 1, 1, 1);

            }

            if (Util.GetItemCountForTeam(TeamIndex.Player, RoR2Content.Items.FocusConvergence.itemIndex, true, false) > 0)
            {
                HoldoutZoneController.sharedColorPropertyBlock.SetColor("_TintColor", HoldoutZoneController.FocusConvergenceController.convergenceMaterialColor);
            }
            else
            {
                HoldoutZoneController.sharedColorPropertyBlock.SetColor("_TintColor", Color.white);
            }
            self.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().SetPropertyBlock(HoldoutZoneController.sharedColorPropertyBlock);

        }


    }
}
