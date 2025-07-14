using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static WolfoFixes.MissedContent.Equipment;

namespace WolfoFixes
{
    internal class MissingEliteDisplays
    {
        public static GameObject MithrixCrystalYellow = null;
        public static GameObject MithrixCrystalOrange = null;
        public static GameObject MithrixCrystalPink = null;
        public static GameObject MithrixCrystalPinkSmall = null;
        public static GameObject LunarHalo = null;

        public static void AddLunarDisplay(DisplayRuleGroup dsr, string asset)
        {
            dsr.rules[0].followerPrefab = LunarHalo;
            if (dsr.rules.Length > 1)
            {
                dsr.rules[1].followerPrefab = LunarHalo;
            }
            AddDisplay(asset, RoR2Content.Equipment.AffixLunar, dsr);
        }

        public static void AddDisplay(DisplayRuleGroup dsr, EquipmentDef equipmentDef, string asset)
        {
            AddDisplay(asset, equipmentDef, dsr);
        }

        public static void AddDisplay(string asset, EquipmentDef equipmentDef, DisplayRuleGroup dsr)
        {
            ItemDisplayRuleSet idrs = Addressables.LoadAssetAsync<ItemDisplayRuleSet>(key: asset).WaitForCompletion();
            //idrs.runtimeEquipmentRuleGroups[(int)equipmentDef.equipmentIndex].rules = dsr.rules;
            HG.ArrayUtils.ArrayAppend(ref idrs.keyAssetRuleGroups, new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = equipmentDef,
                displayRuleGroup = dsr
            });


        }

        public static void AddDisplay(ItemDisplayRuleSet idrs, EquipmentDef equipmentDef, DisplayRuleGroup dsr, GameObject prefab, bool cloneRule = false)
        {
            if (prefab)
            {
                if (cloneRule)
                {
                    DisplayRuleGroup newDsr = new DisplayRuleGroup();
                    newDsr.rules = new ItemDisplayRule[dsr.rules.Length];
                    dsr.rules.CopyTo(newDsr.rules, 0);
                    dsr = newDsr;
                }
                for (int i = 0; i < dsr.rules.Length; i++)
                {
                    dsr.rules[i].followerPrefab = prefab;
                }
            }
            //idrs.runtimeEquipmentRuleGroups[(int)equipmentDef.equipmentIndex] = dsr;
            HG.ArrayUtils.ArrayAppend(ref idrs.keyAssetRuleGroups, new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = equipmentDef,
                displayRuleGroup = dsr
            });
        }

        public static void OtherItemDisplays()
        {
            GameObject DisplayVolatileBatteryPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/QuestVolatileBattery/DisplayBatteryArray.prefab").WaitForCompletion();

            DisplayRuleGroup drg;
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = DisplayVolatileBatteryPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 0),
                            localAngles = new Vector3(270f,180f,0f),
                            localScale = new Vector3(1.2f, 1.2f, 1f),

                        },
}
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiTurret.asset", RoR2Content.Equipment.QuestVolatileBattery, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            followerPrefab = DisplayVolatileBatteryPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.65f, -0.5f),
                            localAngles = new Vector3(270f,180f,0f),
                            localScale = new Vector3(1f, 1f, 1f),

                        },
}
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiWalkerTurret.asset", RoR2Content.Equipment.QuestVolatileBattery, drg);

        }

        public static void StupidRabbitEars()
        {
            GameObject DisplayEliteRabbitEars = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DisplayEliteRabbitEars.prefab").WaitForCompletion();
            DisplayRuleGroup drg;

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Chest",
                            localPos = new Vector3(0f, 5.25f, 1.5f),
                            localAngles = new Vector3(335f,180f,0f),
                            localScale = new Vector3(40f, 30f, 30f),

                        },
}
            };
            AddDisplay(drg, EliteSecretSpeedEquipment, "RoR2/Base/Scav/idrsScav.asset");
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
      {
                        new ItemDisplayRule
                        {

                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.35f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f,1f,1f),

                        },
      }
            };
            AddDisplay(drg, EliteSecretSpeedEquipment, "RoR2/Base/Commando/idrsCommando.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, -0.05f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.7f,0.7f,0.7f),

                        },
}
            };
            AddDisplay(drg, EliteSecretSpeedEquipment, "RoR2/Base/Huntress/idrsHuntress.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(335f,0f,0f),
                            localScale = new Vector3(0.9f,0.9f,0.9f),

                        },
}

            };
            AddDisplay(drg, EliteSecretSpeedEquipment, "RoR2/Base/Captain/idrsCaptain.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, -0.5f, 0.9f),
                            localAngles = new Vector3(300f,0,180f),
                            localScale = new Vector3(10f,10f,10f),

                        },
}
            };
            AddDisplay(drg, EliteSecretSpeedEquipment, "RoR2/Base/Croco/idrsCroco.asset");


        }




        public static void Start()
        {
            LunarEliteDisplays();
            MissingElites();
            OtherItemDisplays();
            StupidRabbitEars();
            MithrixItemDisplay();
        }

        public static void MissingElites()
        {

            GameObject EliteFireHorn = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/EliteFire/DisplayEliteHorn.prefab").WaitForCompletion();
            GameObject EliteLightningHorn = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/EliteLightning/DisplayEliteRhinoHorn.prefab").WaitForCompletion();
            GameObject EliteIceCrown = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/EliteIce/DisplayEliteIceCrown.prefab").WaitForCompletion();
            GameObject ElitePoisonCrown = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ElitePoison/DisplayEliteUrchinCrown.prefab").WaitForCompletion();
            GameObject EliteStealthCrown = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/EliteHaunted/DisplayEliteStealthCrown.prefab").WaitForCompletion();

            GameObject EliteEarthHorn = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/EliteEarth/DisplayEliteMendingAntlers.prefab").WaitForCompletion();
            GameObject EliteVoid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/EliteVoid/DisplayAffixVoid.prefab").WaitForCompletion();
            GameObject EliteRabbitEars = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DisplayEliteRabbitEars.prefab").WaitForCompletion();

            GameObject EliteAurCrown = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Elites/EliteAurelionite/DisplayEliteAurelioniteEquipment.prefab").WaitForCompletion();
            GameObject EliteBeadSpike = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Elites/EliteBead/DisplayEliteBeadSpike.prefab").WaitForCompletion();


            DisplayRuleGroup drg;
            #region Elites : Engi Turret
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                  {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "Head",
                            localPos = new Vector3(-0.65f, 0.5f, -0.1f),
                            localAngles = new Vector3(60f,0f,0f),
                            localScale = new Vector3(-0.4f, 0.35f, 0.35f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "Head",
                            localPos = new Vector3(0.65f, 0.5f, -0.1f),
                            localAngles = new Vector3(60f,0f,0f),
                            localScale = new Vector3(0.4f, 0.35f, 0.35f),

                        },
                  }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiTurret.asset", RoR2Content.Equipment.AffixRed, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                   {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.15f, 0.15f),
                            localAngles = new Vector3(320f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.05f, 0.2f),
                            localAngles = new Vector3(330f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),

                        },
                   }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiTurret.asset", RoR2Content.Equipment.AffixBlue, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
           {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteIceCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.3f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),

                        },
           }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiTurret.asset", RoR2Content.Equipment.AffixWhite, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
            {
                        new ItemDisplayRule
                        {

                            followerPrefab = ElitePoisonCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.7f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),

                        },
            }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiTurret.asset", RoR2Content.Equipment.AffixPoison, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
           {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteStealthCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),

                        },
           }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiTurret.asset", RoR2Content.Equipment.AffixHaunted, drg);
            #endregion

            #region Elites : Engi Walking Turret
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
          {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "Head",
                            localPos = new Vector3(-0.4f, 1f, -0.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(-0.45f, 0.4f, 0.4f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "Head",
                            localPos = new Vector3(0.4f, 1f, -0.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(0.45f, 0.4f, 0.4f),

                        },
          }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiWalkerTurret.asset", RoR2Content.Equipment.AffixRed, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                     {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "Head",
                            localPos = new Vector3(0, -0.25f, 0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 1.5f, 1.25f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "Head",
                            localPos = new Vector3(0, 0f, 0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 1f, 1f),

                        },
                     }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiTurret.asset", RoR2Content.Equipment.AffixBlue, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteIceCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.8f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),

                        },
}
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiWalkerTurret.asset", RoR2Content.Equipment.AffixWhite, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = ElitePoisonCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.15f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.075f, 0.075f, 0.075f),

                        },
}
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiWalkerTurret.asset", RoR2Content.Equipment.AffixPoison, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
           {
                        new ItemDisplayRule
                        {

                            followerPrefab = ElitePoisonCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.4f, -0.45f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),

                        },
           }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiWalkerTurret.asset", RoR2Content.Equipment.AffixHaunted, drg);
            #endregion


            #region Elites : Mithrix
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "Head",
                            localPos = new Vector3(0.15f, 0.15f, 0.05f),
                            localAngles = new Vector3(0f,350f,0f),
                            localScale = new Vector3(0.15f, 0.15f, 0.15f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "Head",
                            localPos = new Vector3(-0.15f, 0.15f, 0.05f),
                            localAngles = new Vector3(0f,10f,0f),
                            localScale = new Vector3(-0.15f, 0.15f, 0.15f),

                        },
}
            };
            AddDisplay("RoR2/Base/Brother/idrsBrother.asset", RoR2Content.Equipment.AffixRed, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                   {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.15f, 0.15f),
                            localAngles = new Vector3(320f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.05f, 0.2f),
                            localAngles = new Vector3(330f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),

                        },
                   }
            };
            AddDisplay("RoR2/Base/Brother/idrsBrother.asset", RoR2Content.Equipment.AffixBlue, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteIceCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.035f, 0.035f, 0.035f),

                        },
}
            };
            AddDisplay("RoR2/Base/Brother/idrsBrother.asset", RoR2Content.Equipment.AffixWhite, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = ElitePoisonCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.15f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.075f, 0.075f, 0.075f),

                        },
}
            };
            AddDisplay("RoR2/Base/Brother/idrsBrother.asset", RoR2Content.Equipment.AffixPoison, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteStealthCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, 0f),
                            localAngles = new Vector3(90f,180f,0f),
                            localScale = new Vector3(0.06f, 0.06f, 0.06f),

                        },
}
            };
            AddDisplay("RoR2/Base/Brother/idrsBrother.asset", RoR2Content.Equipment.AffixHaunted, drg);

            #endregion

            #region Elites : Lunar Exploder
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0.5f, 0.5f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.45f, 0.4f, 0.4f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "MuzzleCore",
                            localPos = new Vector3(-0.5f, 0.5f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(-0.45f, 0.4f, 0.4f),

                        },
}
            };
            AddDisplay("RoR2/Base/LunarExploder/idrsLunarExploder.asset", RoR2Content.Equipment.AffixRed, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0, 0f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1.25f, 1.25f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0, 0.4f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),

                        },
                    }
            };
            AddDisplay("RoR2/Base/LunarExploder/idrsLunarExploder.asset", RoR2Content.Equipment.AffixBlue, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
           {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteIceCrown,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 1f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),

                        },
           }
            };
            AddDisplay("RoR2/Base/LunarExploder/idrsLunarExploder.asset", RoR2Content.Equipment.AffixWhite, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
       {
                        new ItemDisplayRule
                        {

                            followerPrefab = ElitePoisonCrown,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 0.7f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),

                        },
       }
            };
            AddDisplay("RoR2/Base/LunarExploder/idrsLunarExploder.asset", RoR2Content.Equipment.AffixPoison, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
           {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteStealthCrown,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 0.8f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),

                        },
           }
            };
            AddDisplay("RoR2/Base/LunarExploder/idrsLunarExploder.asset", RoR2Content.Equipment.AffixHaunted, drg);
            #endregion

            #region Elites : Lunar Golem
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
 {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "MuzzleRT",
                            localPos = new Vector3(0.1f, -0.1f, -0.7f),
                            localAngles = new Vector3(320f,335f,180f),
                            localScale = new Vector3(0.5f, 0.45f, 0.45f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "MuzzleLT",
                            localPos = new Vector3(-0.1f, -0.1f, -0.7f),
                            localAngles = new Vector3(320f,25f,180f),
                            localScale = new Vector3(-0.5f, 0.45f, 0.45f),

                        },
 }
            };
            AddDisplay("RoR2/Base/LunarGolem/idrsLunarGolem.asset", RoR2Content.Equipment.AffixRed, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                               {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "Head",
                            localPos = new Vector3(0, 0f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1.25f, 1.25f),

                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteLightningHorn,
                            childName = "Head",
                            localPos = new Vector3(0, 0.4f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),

                        },
                               }
            };
            AddDisplay("RoR2/Base/LunarGolem/idrsLunarGolem.asset", RoR2Content.Equipment.AffixBlue, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
 {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteIceCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.15f, 0.15f, 0.15f),

                        },
 }
            };
            AddDisplay("RoR2/Base/LunarGolem/idrsLunarGolem.asset", RoR2Content.Equipment.AffixWhite, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
           {
                        new ItemDisplayRule
                        {

                            followerPrefab = ElitePoisonCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.4f, 0.5f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),

                        },
           }
            };
            AddDisplay("RoR2/Base/LunarGolem/idrsLunarGolem.asset", RoR2Content.Equipment.AffixPoison, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
           {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteStealthCrown,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),

                        },
           }
            };
            AddDisplay("RoR2/Base/LunarGolem/idrsLunarGolem.asset", RoR2Content.Equipment.AffixHaunted, drg);
            #endregion

            #region Elites : Lunar Wisp
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
   {
                        new ItemDisplayRule
                        {
                            followerPrefab = EliteFireHorn,
                            childName = "Mask",
                            localPos = new Vector3(0.5f, -1f, 2f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.2f, 1.2f, 1.2f),
                        },
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteFireHorn,
                            childName = "Mask",
                            localPos = new Vector3(-0.5f, -1f, 2f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(-1.2f, 1.2f, 1.2f),
                        },
   }
            };
            AddDisplay("RoR2/Base/LunarWisp/idrsLunarWisp.asset", RoR2Content.Equipment.AffixRed, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                     {
                        new ItemDisplayRule
                        {
                            followerPrefab = EliteLightningHorn,
                            childName = "Mask",
                            localPos = new Vector3(0, -3f, 1.1f),
                            localAngles = new Vector3(50f,0f,0f),
                            localScale = new Vector3(1.6f, 1.6f, 1.6f),
                        },
                        new ItemDisplayRule
                        {
                            followerPrefab = EliteLightningHorn,
                            childName = "Mask",
                            localPos = new Vector3(0, -2.5f, 1.8f),
                            localAngles = new Vector3(50f,0f,0f),
                            localScale = new Vector3(1.4f, 1.2f, 1.2f),
                        },
                     }
            };
            AddDisplay("RoR2/Base/LunarWisp/idrsLunarWisp.asset", RoR2Content.Equipment.AffixBlue, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            followerPrefab = EliteIceCrown,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 3.5f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                        },
}
            };
            AddDisplay("RoR2/Base/LunarWisp/idrsLunarWisp.asset", RoR2Content.Equipment.AffixWhite, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {

                            followerPrefab = ElitePoisonCrown,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 2.5f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.45f, 0.45f, 0.45f),

                        },
}
            };
            AddDisplay("RoR2/Base/LunarWisp/idrsLunarWisp.asset", RoR2Content.Equipment.AffixPoison, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
            {
                        new ItemDisplayRule
                        {

                            followerPrefab = EliteStealthCrown,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.5f, 0.5f, 0.5f),

                        },
            }
            };
            AddDisplay("RoR2/Base/LunarWisp/idrsLunarWisp.asset", RoR2Content.Equipment.AffixHaunted, drg);
            #endregion


            #region Elites : XI Construct
            GameObject XIBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/MajorAndMinorConstruct/MegaConstructBody.prefab").WaitForCompletion();
            ItemDisplayRuleSet XI = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            XI.name = "idrsMegaConstruct_WolfMade";
            XIBody.transform.GetChild(0).GetChild(0).GetComponent<CharacterModel>().itemDisplayRuleSet = XI;

            DisplayRuleGroup earth1XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
 {
                        new ItemDisplayRule
                        {
                            childName = "Eye",
                            localPos = new Vector3(0,0,2),
                            localAngles = new Vector3(90, 0F, 0F),
                            localScale = Vector3.one*9f,
                        },
 }
            };

            DisplayRuleGroup aur1XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
 {
                        new ItemDisplayRule
                        {
                            childName = "Eye",
                            localPos = new Vector3(0F, 1.7F, 4.25F),
                            localAngles = new Vector3(320F, 0F, 0F),
                            localScale = Vector3.one*5f
                        },
 }
            };
            DisplayRuleGroup haunted1XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
  {
                        new ItemDisplayRule
                        {
                            childName = "Eye",
                            localScale = Vector3.one*1.2f,
                            localPos = new Vector3(0,0,3),
                            localAngles=new Vector3(0,0,180)
                        },
  }
            };
            DisplayRuleGroup ice1XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Eye",
                            localScale = new Vector3(0.6f,0.6f,0.4f),
                            localPos = new Vector3(0,0,3.6f),
                            localAngles=new Vector3(0,0,180)
                        },
}
            };
            DisplayRuleGroup lunarXI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Eye",
                            localScale = new Vector3(5f,5f,2.5f),
                            localPos = new Vector3(0,3,0f),
                            localAngles=new Vector3(90,0,0)
                        },
}
            };
            DisplayRuleGroup voidXI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Eye",
                            localScale =Vector3.one*2.1f,
                            localPos = new Vector3(0,1.2f,0f),
                        },
}
            };
            DisplayRuleGroup lightning2XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
            {
                        new ItemDisplayRule
                        {
                            childName = "Eye",
                            localScale =Vector3.one*4.4f,
                            localAngles = new Vector3(310,0,180),
                            localPos = new Vector3(0,1.7f,0f),
                        },
                        new ItemDisplayRule
                        {
                            childName = "Eye",
                            localScale =Vector3.one*3.3f,
                            localAngles = new Vector3(320,0,180),
                            localPos = new Vector3(0,1f,1f),
                        },
            }
            };

            DisplayRuleGroup fire4XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
  {
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint0",
                            localScale = new Vector3(-1.2f,1.2f,1.2f),
                            localPos = new Vector3(0,-0.5f,0)
                        },
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint1",
                            localScale = new Vector3(1.2f,1.2f,1.2f),
                            localPos = new Vector3(0,-0.5f,0)
                        },
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint2",
                            localScale = new Vector3(1.2f,1.2f,1.2f),
                            localPos = new Vector3(0,-0.5f,0)
                        },
                          new ItemDisplayRule
                        {
                            childName = "AttachmentPoint3",
                            localScale = new Vector3(-1.2f,1.2f,1.2f),
                            localPos = new Vector3(0,-0.5f,0)
                        },
  }
            };

            DisplayRuleGroup rabbit4XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
  {
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint0",
                            localScale = new Vector3(-15F, 13F, 13F)
                        },
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint1",
                            localScale = new Vector3(15F, 13F, 13F)
                        },
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint2",
                            localScale = new Vector3(15F, 13F, 13F)
                        },
                          new ItemDisplayRule
                        {
                            childName = "AttachmentPoint3",
                            localScale = new Vector3(-15F, 13F, 13F)
                        },
  }
            };

            DisplayRuleGroup bead4XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
  {
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint0",
                            localScale = new Vector3(-0.2f,0.14f,0.2f),
                        },
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint1",
                            localScale = new Vector3(0.2f,0.14f,0.2f),
                        },
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint2",
                            localScale = new Vector3(0.2f,0.14f,0.2f),
                        },
                          new ItemDisplayRule
                        {
                            childName = "AttachmentPoint3",
                            localScale = new Vector3(-0.2f,0.14f,0.2f),
                        },
  }
            };
            DisplayRuleGroup poison4XI_drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
  {
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint0",
                            localAngles = new Vector3(270,0,0),
                            localPos = new Vector3(0,-0.4f,0),
                            localScale = new Vector3(-0.4f,0.4f,0.5f)
                        },
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint1",
                            localAngles = new Vector3(270,0,0),
                            localPos = new Vector3(0,-0.4f,0),
                            localScale = new Vector3(0.4f,0.4f,0.5f)
                        },
                        new ItemDisplayRule
                        {
                            childName = "AttachmentPoint2",
                            localAngles = new Vector3(270,0,0),
                            localPos = new Vector3(0,-0.4f,0),
                            localScale = new Vector3(0.4f,0.4f,0.5f)
                        },
                          new ItemDisplayRule
                        {
                            childName = "AttachmentPoint3",
                            localAngles = new Vector3(270,0,0),
                            localPos = new Vector3(0,-0.4f,0),
                            localScale = new Vector3(-0.4f,0.4f,0.5f)
                        },
  }
            };

            AddDisplay(XI, RoR2Content.Equipment.AffixRed, fire4XI_drg, EliteFireHorn, false);
            AddDisplay(XI, RoR2Content.Equipment.AffixBlue, lightning2XI_drg, EliteLightningHorn, false);
            AddDisplay(XI, RoR2Content.Equipment.AffixWhite, ice1XI_drg, EliteIceCrown, false);
            AddDisplay(XI, RoR2Content.Equipment.AffixHaunted, haunted1XI_drg, EliteStealthCrown, false);
            AddDisplay(XI, RoR2Content.Equipment.AffixPoison, poison4XI_drg, ElitePoisonCrown, false);
            AddDisplay(XI, RoR2Content.Equipment.AffixLunar, lunarXI_drg, LunarHalo, false);

            AddDisplay(XI, EliteEarthEquipment, earth1XI_drg, EliteEarthHorn, false);
            AddDisplay(XI, DLC1Content.Equipment.EliteVoidEquipment, voidXI_drg, EliteVoid, false);
            AddDisplay(XI, MissedContent.Equipment.EliteSecretSpeedEquipment, rabbit4XI_drg, EliteRabbitEars, false);

            AddDisplay(XI, DLC2Content.Equipment.EliteAurelioniteEquipment, aur1XI_drg, EliteAurCrown, false);
            AddDisplay(XI, DLC2Content.Equipment.EliteBeadEquipment, bead4XI_drg, EliteBeadSpike, false);

            XI.GenerateRuntimeValuesAsync();
            #endregion

        }

        public static void MithrixItemDisplay()
        {

            GameObject ItemInfectionRed = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/ItemInfection, Red.prefab").WaitForCompletion();
            MithrixCrystalOrange = PrefabAPI.InstantiateClone(ItemInfectionRed, "ItemInfection, Orange", false);
            MithrixCrystalYellow = PrefabAPI.InstantiateClone(ItemInfectionRed, "ItemInfection, Yellow", false);
            MithrixCrystalPink = PrefabAPI.InstantiateClone(ItemInfectionRed, "ItemInfection, Pink", false);
            MithrixCrystalPinkSmall = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/ItemInfection, White.prefab").WaitForCompletion(), "ItemInfection, PinkSingle", false);
            MithrixCrystalOrange.GetComponent<MeshRenderer>().material = Object.Instantiate<Material>(MithrixCrystalOrange.GetComponent<MeshRenderer>().material);
            MithrixCrystalOrange.GetComponent<MeshRenderer>().material.SetColor("_EmColor", new Color(1.4f, 0.7f, 0f, 1f));
            MithrixCrystalYellow.GetComponent<MeshRenderer>().material = Object.Instantiate<Material>(MithrixCrystalYellow.GetComponent<MeshRenderer>().material);
            MithrixCrystalYellow.GetComponent<MeshRenderer>().material.SetColor("_EmColor", new Color(1f, 1f, 0f, 1f));
            MithrixCrystalOrange.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = MithrixCrystalOrange.GetComponent<MeshRenderer>().material;
            MithrixCrystalYellow.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = MithrixCrystalYellow.GetComponent<MeshRenderer>().material;

            Material PinkCrystal = Object.Instantiate<Material>(MithrixCrystalPink.GetComponent<MeshRenderer>().material);
            PinkCrystal.SetColor("_EmColor", new Color(1f, 0f, 0.5f, 1f));
            PinkCrystal.name = "matBrotherInfectionPink";


            MithrixCrystalPink.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = PinkCrystal;
            MithrixCrystalPinkSmall.GetComponent<ItemDisplay>().rendererInfos[0].defaultMaterial = PinkCrystal;
            MithrixCrystalPink.GetComponent<MeshRenderer>().material = PinkCrystal;
            MithrixCrystalPinkSmall.GetComponent<MeshRenderer>().material = PinkCrystal;

            VoidDisplaysMithrix();
        }

        public static void LunarEliteDisplays()
        {
            LunarHalo = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/EliteLunar/DisplayEliteLunar,Eye.prefab").WaitForCompletion();
            GameObject EliteLunar2 = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/EliteLunar/DisplayEliteLunar, Fire.prefab").WaitForCompletion();
            DisplayRuleGroup drg;

            #region Base Survivors
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                   {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.2787723f, 0.2163888f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.275f, 0.275f, 0.275f),
                        },
                   }
            };
            AddLunarDisplay(drg, "RoR2/Base/Commando/idrsCommando.asset");
            AddLunarDisplay(drg, "RoR2/Base/Huntress/idrsHuntress.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                   {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                        },
                   }
            };
            AddLunarDisplay(drg, "RoR2/Base/Bandit2/idrsBandit2.asset");
            AddLunarDisplay(drg, "RoR2/Base/Mage/idrsMage.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 3.75f, -1.15f),
                            localAngles = new Vector3(60f,0,0f),
                            localScale = new Vector3(3f, 3f, 3f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Toolbot/idrsToolbot.asset");
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.12f, 0.2164f),
                            localAngles = Vector3.zero,
                            localScale = new Vector3(0.275f, 0.275f, 0.275f),
                        },
        }
            };
            AddLunarDisplay(drg, "RoR2/Base/Merc/idrsMerc.asset");
            AddLunarDisplay(drg, "RoR2/Base/Captain/idrsCaptain.asset");
            AddLunarDisplay(drg, "RoR2/Base/Loader/idrsLoader.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
    {
                        new ItemDisplayRule
                        {
                            childName = "FlowerBase",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = Vector3.zero,
                            localScale = new Vector3(2f, 2f, 2f),
                        },
    }
            };
            AddLunarDisplay(drg, "RoR2/Base/Treebot/idrsTreebot.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0,0f),
                            localScale = new Vector3(4f, 4f, 4f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Croco/idrsCroco.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Chest",
                            localPos = new Vector3(0f, 0.6f, 0.3f),
                            localAngles = new Vector3(0f,0,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Engi/idrsEngi.asset");
            #endregion
            #region Base Enemies
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.575f, -0.5f),
                            localAngles = new Vector3(40f, 0, 0f),
                            localScale = new Vector3(0.8f, 0.8f, 0.8f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Beetle/idrsBeetle.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 2f, 0.6f),
                            localAngles = new Vector3(50f, 170, 170f),
                            localScale = new Vector3(3f, 3f, 3f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/BeetleGuard/idrsBeetleGuard.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, -2.3f),
                            localAngles = new Vector3(350f, 0f, 0f),
                            localScale = new Vector3(2.3f, 2.3f, 2.3f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/BeetleQueen/idrsBeetleQueen.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Chain",
                            localPos = new Vector3(0f, -1f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Bell/idrsBell.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.65f, 0f),
                            localAngles = new Vector3(90f, 0, 0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Bison/idrsBison.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                   {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0.25f),
                            localAngles = Vector3.zero,
                            localScale = new Vector3(0.35f, 0.35f, 0.35f),
                        },
                   }
            };
            AddLunarDisplay(drg, "RoR2/Base/Brother/idrsBrother.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "PotLidTop",
                            localPos = new Vector3(0f, 1f, 1f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.25f, 2.25f, 2.25f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/ClayBoss/idrsClayBoss.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.45f, 0.1f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.6f, 0.6f, 0.6f),
                        },
                        new ItemDisplayRule
                        {
                            childName = "Muzzle",
                            localPos = new Vector3(0f, -0.05f, -0.05f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.6f, 0.6f, 0.6f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/ClayBruiser/idrsClayBruiser.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
      {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 3f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.75f, 2.75f, 2.75f),
                        },
      }
            };
            AddLunarDisplay(drg, "RoR2/Base/MagmaWorm/idrsMagmaWorm.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.575f, 0.55f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Golem/idrsGolem.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 8f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(5f, 5f, 5f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Grandparent/idrsGrandparent.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, 3.75f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(3.3f, 3.3f, 3.3f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Gravekeeper/idrsGravekeeper.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "MaskBase",
                            localPos = new Vector3(0f, 0.25f, 0.8f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/GreaterWisp/idrsGreaterWisp.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Base",
                            localPos = new Vector3(0f, 1.5f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/HermitCrab/idrsHermitCrab.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Neck",
                            localPos = new Vector3(0f, -0.25f, -0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.4f, 0.4f, 0.4f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Imp/idrsImp.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Chest",
                            localPos = new Vector3(0f, 0.6f, 1.85f),
                            localAngles = new Vector3(10f,180f,0f),
                            localScale = new Vector3(1.6f, 1.6f, 1.6f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/ImpBoss/idrsImpBoss.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Hull2",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.75f, 1.75f, 1.75f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Jellyfish/idrsJellyfish.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(3f, 3f, 3f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Lemurian/idrsLemurian.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, -0.5f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(4f, 4f, 4f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/LemurianBruiser/idrsLemurianBruiser.asset");
            AddLunarDisplay(drg, "RoR2/Base/Vulture/idrsVulture.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0.25f, 0f, 0f),
                            localAngles = new Vector3(0f,90f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/MiniMushroom/idrsMiniMushroom.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Muzzle",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Nullifier/idrsNullifier.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(60f, 100f, 0f),
                            localAngles = new Vector3(315f,90f,0f),
                            localScale = new Vector3(70f, 120f, 70f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Parent/idrsParent.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Center",
                            localPos = new Vector3(0f, 0f, 1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/RoboBallBoss/idrsRoboBallBoss.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Muzzle",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 0.75f, 0.75f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/RoboBallBoss/idrsRoboBallMini.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 4f, -6f),
                            localAngles = new Vector3(65f,0f,0f),
                            localScale = new Vector3(7f, 7f, 7f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Scav/idrsScav.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 2f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(4f, 4f, 4f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Titan/idrsTitan.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Hull",
                            localPos = new Vector3(0f, 0.8f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.2f, 2.2f, 2.2f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Vagrant/idrsVagrant.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.75f, 0.75f, 0.75f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/Base/Wisp/idrsWisp.asset");
            #endregion
            #region DLC1 Content
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.06f, 0.15f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                        },
                    }
            };
            AddLunarDisplay(drg, "RoR2/DLC1/Railgunner/idrsRailGunner.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.325f, 0.325f, 0.325f),
                        },
        }
            };
            AddLunarDisplay(drg, "RoR2/DLC1/VoidSurvivor/idrsVoidSurvivor.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "BeakUpper",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),

                        },
                        new ItemDisplayRule
                        {
                            childName = "BodyBase",
                            localPos = new Vector3(0f, 4f, -3.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(4f, 4f, 3f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/DLC1/AcidLarva/idrsAcidLarva.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Torso",
                            localPos = new Vector3(0f, 0.1f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.4f, 0.4f, 0.4f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/DLC1/ClayGrenadier/idrsClayGrenadier.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Body",
                            localPos = new Vector3(0f, 0f, 1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.1f, 1.1f, 1f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/DLC1/FlyingVermin/idrsFlyingVermin.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                {
                        new ItemDisplayRule
                        {
                            childName = "MainBody2",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                        },
                }
            };
            AddLunarDisplay(drg, "RoR2/DLC1/Gup/idrsGeep.asset");
            AddLunarDisplay(drg, "RoR2/DLC1/Gup/idrsGip.asset");
            AddLunarDisplay(drg, "RoR2/DLC1/Gup/idrsGup.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                {
                        new ItemDisplayRule
                        {
                            childName = "CapTop",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/DLC1/MajorAndMinorConstruct/idrsMinorConstruct.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/DLC1/Vermin/idrsVermin.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(0f,90f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        }
}
            };
            AddLunarDisplay(drg, "RoR2/DLC1/VoidBarnacle/idrsVoidBarnacle.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(-0.2f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
                                                new ItemDisplayRule
                        {
                            childName = "ClawMuzzle",
                            localPos = new Vector3(0f, -0.125f, -1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.25f, 1.25f, 1.25f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/DLC1/VoidJailer/idrsVoidJailer.asset");

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "MuzzleBlackCannon",
                            localPos = new Vector3(0f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
                                                new ItemDisplayRule
                        {
                            childName = "MuzzleWhiteCannon",
                            localPos = new Vector3(0f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
}
            };
            AddLunarDisplay(drg, "RoR2/DLC1/VoidMegaCrab/idrsVoidMegaCrab.asset");
            #endregion
            #region DLC2 Content
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                  {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.25f, 0.25f),
                            localScale = new Vector3(0.4f,0.4f,0.3f),
                        },
                  }
            };
            AddLunarDisplay(drg, "RoR2/DLC2/Child/idrsChild.asset");
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                           {
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localAngles = new Vector3(0f,270f,0f),
                            localScale = Vector3.one*1.2f,
                        },
                           }
            };
            AddLunarDisplay(drg, "RoR2/DLC2/Scorchling/idrsScorchling.asset");
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
                {
                        new ItemDisplayRule
                        {
                            childName = "ChestGlow",
                            localPos = new Vector3(0f, 0.25f, 1.5f),
                            localScale = new Vector3(2.2f,2.2f,1.5f)
                        },
                }
            };
            AddLunarDisplay(drg, "RoR2/DLC2/Halcyonite/idrsHalcyonite.asset");

            #endregion
            #region Other
            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    childName = "Head",
                    localPos = new Vector3(0f, 0.65f, 1.6f),
                    localAngles = new Vector3(0f, 0f, 0f),
                    localScale = new Vector3(1.25f, 1.25f, 1.25f),
                    limbMask = LimbFlags.None
                },
            }
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiTurret.asset", RoR2Content.Equipment.AffixLunar, drg);

            drg = new DisplayRuleGroup
            {
                rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            childName = "Head",
                            localPos = new Vector3(0f, 0.8f, 0.8f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.35f, 1.35f, 1.5f),

                        },
}
            };
            AddDisplay("RoR2/Base/Engi/idrsEngiWalkerTurret.asset", RoR2Content.Equipment.AffixLunar, drg);
            #endregion


        }


        public static void VoidDisplaysMithrix()
        {

            ItemDisplayRuleSet idrs = Addressables.LoadAssetAsync<ItemDisplayRuleSet>(key: "RoR2/Base/Brother/idrsBrother.asset").WaitForCompletion();

            foreach (RoR2.Items.ContagiousItemManager.TransformationInfo transformationInfo in RoR2.Items.ContagiousItemManager.transformationInfos)
            {
                DisplayRuleGroup originalDisplayRule = idrs.GetItemDisplayRuleGroup(transformationInfo.originalItem);
                if (originalDisplayRule.isEmpty == false)
                {
                    ItemDef transformedItem = ItemCatalog.GetItemDef(transformationInfo.transformedItem);

                    GameObject FollowerPrefab = MithrixCrystalPink;
                    if (transformedItem.tier == ItemTier.VoidTier1)
                    {
                        FollowerPrefab = MithrixCrystalPinkSmall;
                    }
                    ItemDisplayRule[] newDisplayRules = new ItemDisplayRule[originalDisplayRule.rules.Length];
                    originalDisplayRule.rules.CopyTo(newDisplayRules, 0);
                    for (int i = 0; i < newDisplayRules.Length; i++)
                    {
                        newDisplayRules[i].followerPrefab = FollowerPrefab;
                    }
                    var displayRuleGroup = new DisplayRuleGroup
                    {
                        rules = newDisplayRules,
                    };
                    idrs.runtimeItemRuleGroups[(int)transformedItem.itemIndex] = displayRuleGroup;
                }
            }


        }
    }
}