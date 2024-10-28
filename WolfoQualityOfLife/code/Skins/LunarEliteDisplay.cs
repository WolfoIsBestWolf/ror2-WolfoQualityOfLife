using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQualityOfLife
{
    public class LunarEliteDisplay
    {
        public static GameObject MithrixCrystalYellow = null;
        public static GameObject MithrixCrystalOrange = null;
        public static GameObject MithrixCrystalPink = null;
        public static GameObject MithrixCrystalPinkSmall = null;

        public static void AffixLunarItemDisplay()
        {

            DisplayRuleGroup originallunargolemrule = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarGolemBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet.keyAssetRuleGroups[0].displayRuleGroup;

            GameObject MithrixCrystalRed = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet.keyAssetRuleGroups[0].displayRuleGroup.rules[0].followerPrefab;


            EquipmentDef EliteSecretSpeedEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteSecretSpeedEquipment.asset").WaitForCompletion();
            GameObject DisplayEliteRabbitEars = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DisplayEliteRabbitEars.prefab").WaitForCompletion();




            MithrixCrystalOrange = R2API.PrefabAPI.InstantiateClone(MithrixCrystalRed, "ItemInfection, Orange", false);
            MithrixCrystalYellow = R2API.PrefabAPI.InstantiateClone(MithrixCrystalRed, "ItemInfection, Yellow", false);
            MithrixCrystalPink = R2API.PrefabAPI.InstantiateClone(MithrixCrystalRed, "ItemInfection, Pink", false);
            MithrixCrystalPinkSmall = R2API.PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/ItemInfection, White.prefab").WaitForCompletion(), "ItemInfection, PinkSingle", false);

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






            //newLunarDisplay.displayRuleGroup.rules[0].childName = tempItemDisplayRules.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixWhite.equipmentIndex).rules[0].childName;

            var tempMandoRule = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;

            var DisplayAffixBluePrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixBlue.equipmentIndex).rules[0].followerPrefab;
            var DisplayAffixHaubtedPrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixHaunted.equipmentIndex).rules[0].followerPrefab;
            var DisplayAffixPoisonPrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixPoison.equipmentIndex).rules[0].followerPrefab;
            var DisplayAffixRedPrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixRed.equipmentIndex).rules[0].followerPrefab;
            var DisplayAffixWhitePrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.AffixWhite.equipmentIndex).rules[0].followerPrefab;
            var DisplayVolatileBatteryPrefab = tempMandoRule.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.QuestVolatileBattery.equipmentIndex).rules[0].followerPrefab;


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixCommandoDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.2787723f, 0.2163888f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.275f, 0.275f, 0.275f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixCommandoDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.35f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f,1f,1f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            var tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixCommandoDisplay, SecretSpeedAffixCommandoDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixHuntressDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, -0.05f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.7f,0.7f,0.7f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HuntressBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixCommandoDisplay, SecretSpeedAffixHuntressDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixBandit = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/Bandit2Body").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBandit);
            tempItemDisplayRules.GenerateRuntimeValues();
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixToolBot = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 3.75f, -1.15f),
                            localAngles = new Vector3(60f,0,0f),
                            localScale = new Vector3(3f, 3f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ToolbotBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixToolBot);
            tempItemDisplayRules.GenerateRuntimeValues();




            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixMercDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.12f, 0.2164f),
                            localAngles = originallunargolemrule.rules[0].localAngles,
                            localScale = new Vector3(0.275f, 0.275f, 0.275f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixMercDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();

            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MageBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBandit);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixREXDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "FlowerBase",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = new Vector3(90f,0,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TreebotBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixREXDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LoaderBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixMercDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixCrocoDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0,0f),
                            localScale = new Vector3(4f, 4f, 4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixCrocoDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, -0.5f, 0.9f),
                            localAngles = new Vector3(300f,0,180f),
                            localScale = new Vector3(10f,10f,10f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixCrocoDisplay, SecretSpeedAffixCrocoDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixEngi = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Chest",
                            localPos = new Vector3(0f, 0.6f, 0.3f),
                            localAngles = new Vector3(0f,0,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixEngi);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixCaptainDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(335f,0f,0f),
                            localScale = new Vector3(0.9f,0.9f,0.9f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/CaptainBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixMercDisplay, SecretSpeedAffixCaptainDisplay);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBeetle = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.575f, -0.5f),
                            localAngles = new Vector3(40f, 0, 0f),
                            localScale = new Vector3(0.8f, 0.8f, 0.8f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBeetle);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBeetleGuard = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 2f, 0.6f),
                            localAngles = new Vector3(50f, 170, 170f),
                            localScale = new Vector3(3f, 3f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleGuardBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBeetleGuard);
            tempItemDisplayRules.GenerateRuntimeValues();



            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBeetleQueen = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, -2.3f),
                            localAngles = new Vector3(350f, 0f, 0f),
                            localScale = new Vector3(2.3f, 2.3f, 2.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BeetleQueen2Body").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBeetleQueen);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBell = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Chain",
                            localPos = new Vector3(0f, -1f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BellBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBell);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayBison = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.65f, 0f),
                            localAngles = new Vector3(90f, 0, 0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BisonBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayBison);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayClayBoss = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "PotLidTop",
                            localPos = new Vector3(0f, 1f, 1f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.25f, 2.25f, 2.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBossBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayClayBoss);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayClayTemplar = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.45f, 0.1f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.6f, 0.6f, 0.6f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Muzzle",
                            localPos = new Vector3(0f, -0.05f, -0.05f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.6f, 0.6f, 0.6f),
                            limbMask = LimbFlags.None
                        },

}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBruiserBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayClayTemplar);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayWorms = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 3f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.75f, 2.75f, 2.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayWorms = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.4f, -2.1f),
                            localAngles = new Vector3(0f,180f,0f),
                            localScale = new Vector3(0.35f, 0.35f, 0.35f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ElectricWormBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayWorms, HauntedAffixDisplayWorms);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.575f, 0.55f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GolemBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGolem);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGrandparent = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 8f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(5f, 5f, 5f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GrandParentBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGrandparent);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGravekeeper = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, 3.75f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(3.3f, 3.3f, 3.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GravekeeperBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGravekeeper);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGreaterWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MaskBase",
                            localPos = new Vector3(0f, 0.25f, 0.8f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/GreaterWispBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGreaterWisp);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayCrab = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Base",
                            localPos = new Vector3(0f, 1.5f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/HermitCrabBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayCrab);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayImp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Neck",
                            localPos = new Vector3(0f, -0.25f, -0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.4f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ImpBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayImp);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayImpBoss = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Chest",
                            localPos = new Vector3(0f, 0.6f, 1.85f),
                            localAngles = new Vector3(10f,180f,0f),
                            localScale = new Vector3(1.6f, 1.6f, 1.6f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ImpBossBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayImpBoss);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayJellyfish = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Hull2",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.75f, 1.75f, 1.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/JellyfishBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayJellyfish);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayLemurian = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(3f, 3f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LemurianBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayLemurian);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayElderLemurian = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, -0.5f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(4f, 4f, 4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LemurianBruiserBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayElderLemurian);
            tempItemDisplayRules.GenerateRuntimeValues();
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MagmaWormBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayWorms, HauntedAffixDisplayWorms);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayMushroom = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.25f, 0f, 0f),
                            localAngles = new Vector3(0f,90f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MiniMushroomBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayMushroom);
            tempItemDisplayRules.GenerateRuntimeValues();


            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidReaver = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Muzzle",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/NullifierBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidReaver);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayParent = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(60f, 100f, 0f),
                            localAngles = new Vector3(315f,90f,0f),
                            localScale = new Vector3(70f, 120f, 70f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ParentBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayParent);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayRoboBallBoss = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Center",
                            localPos = new Vector3(0f, 0f, 1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallBossBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayRoboBallBoss);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayRoboBallMini = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Muzzle",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 0.75f, 0.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallMiniBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayRoboBallMini);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayScav = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 4f, -6f),
                            localAngles = new Vector3(65f,0f,0f),
                            localScale = new Vector3(7f, 7f, 7f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup SecretSpeedAffixDisplayScav = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = EliteSecretSpeedEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayEliteRabbitEars,
                            childName = "Chest",
                            localPos = new Vector3(0f, 5.25f, 1.5f),
                            localAngles = new Vector3(335f,180f,0f),
                            localScale = new Vector3(40f, 30f, 30f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ScavBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayScav, SecretSpeedAffixDisplayScav);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayTitan = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 2f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(3f, 3f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TitanBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayTitan);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVagrant = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Hull",
                            localPos = new Vector3(0f, 0.8f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2.2f, 2.2f, 2.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/VagrantBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVagrant);
            tempItemDisplayRules.GenerateRuntimeValues();
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/VultureBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayElderLemurian);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(0.75f, 0.75f, 0.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/WispBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayWisp);
            tempItemDisplayRules.GenerateRuntimeValues();





            //DLC Mobs and Characters
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayRailgunner = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.06f, 0.15f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Railgunner/RailgunnerBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayRailgunner);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidFiend = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(340f,0f,0f),
                            localScale = new Vector3(0.325f, 0.325f, 0.325f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidFiend);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayAcidLarva = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "BeakUpper",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(2f, 2f, 2f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "BodyBase",
                            localPos = new Vector3(0f, 4f, -3.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(4f, 4f, 3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/AcidLarva/AcidLarvaBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayAcidLarva);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayFlyingVerminBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Body",
                            localPos = new Vector3(0f, 0f, 1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.1f, 1.1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/FlyingVermin/FlyingVerminBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayFlyingVerminBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVerminBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Vermin/VerminBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVerminBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayMinorConstructBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "CapTop",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/MajorAndMinorConstruct/MinorConstructBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayMinorConstructBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGipBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MainBody2",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GipBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGipBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGeepBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MainBody2",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(1.4f, 1.4f, 1.2f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GeepBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGeepBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayGupBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MainBody2",
                            localPos = new Vector3(0f, 0.6f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(1.5f, 1.5f, 1.1f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GupBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayGupBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayClayGrenadierBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Torso",
                            localPos = new Vector3(0f, 0.1f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.4f, 0.4f, 0.4f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/ClayGrenadier/ClayGrenadierBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayClayGrenadierBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidJailerBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.2f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
                                                new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "ClawMuzzle",
                            localPos = new Vector3(0f, -0.125f, -1f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.25f, 1.25f, 1.25f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidJailer/VoidJailerBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidJailerBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidBarnacleBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0f),
                            localAngles = new Vector3(0f,90f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        }
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidBarnacle/VoidBarnacleBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidBarnacleBody);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayVoidMegaCrabBody = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MuzzleBlackCannon",
                            localPos = new Vector3(0f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
                                                new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "MuzzleWhiteCannon",
                            localPos = new Vector3(0f, 0f, 0.3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                        },
}
                }
            };
            tempItemDisplayRules = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidMegaCrab/VoidMegaCrabBody.prefab").WaitForCompletion().GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayVoidMegaCrabBody);
            tempItemDisplayRules.GenerateRuntimeValues();


            /*if (MoffeinClayMan != null)
            {
                tempItemDisplayRules = MoffeinClayMan.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
                tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBandit);
                tempItemDisplayRules.GenerateRuntimeValues();

                Texture2D texBodyClayMan = new Texture2D(128, 128, TextureFormat.DXT5, false);
                texBodyClayMan.LoadImage(Properties.Resources.texBodyClayMan, false);
                texBodyClayMan.filterMode = FilterMode.Bilinear;
                texBodyClayMan.wrapMode = TextureWrapMode.Clamp;
                LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ClayBody").GetComponent<CharacterBody>().portraitIcon = texBodyClayMan;
                MoffeinClayMan.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponent<CharacterBody>().portraitIcon = texBodyClayMan;

                MoffeinClayMan.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponent<DeathRewards>().logUnlockableDef = LegacyResourcesAPI.Load<UnlockableDef>("unlockabledefs/Logs.ClayBody.0");


                //UnlockableDef dummyunlock = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<DeathRewards>().logUnlockableDef;

                // MoffeinClayMan.GetComponent<RoR2.CharacterMaster>().bodyPrefab.GetComponent<DeathRewards>().logUnlockableDef = dummyunlock;
            }*/





            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixBrotherDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0.25f),
                            localAngles = originallunargolemrule.rules[0].localAngles,
                            localScale = new Vector3(0.35f, 0.35f, 0.35f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.15f, 0.15f, 0.05f),
                            localAngles = new Vector3(0f,350f,0f),
                            localScale = new Vector3(0.15f, 0.15f, 0.15f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.15f, 0.15f, 0.05f),
                            localAngles = new Vector3(0f,10f,0f),
                            localScale = new Vector3(-0.15f, 0.15f, 0.15f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.3f, 0f),
                            localAngles = new Vector3(90f,180f,0f),
                            localScale = new Vector3(0.06f, 0.06f, 0.06f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.25f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.035f, 0.035f, 0.035f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.15f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.075f, 0.075f, 0.075f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayBrother = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.15f, 0.15f),
                            localAngles = new Vector3(320f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.05f, 0.2f),
                            localAngles = new Vector3(330f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBrotherDisplay, RedAffixDisplayBrother, BlueAffixDisplayBrother, PoisonAffixDisplayBrother, HauntedAffixDisplayBrother, WhiteAffixDisplayBrother);
            tempItemDisplayRules.GenerateRuntimeValues();
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherGlassBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBrotherDisplay, RedAffixDisplayBrother, BlueAffixDisplayBrother, PoisonAffixDisplayBrother, HauntedAffixDisplayBrother, WhiteAffixDisplayBrother);
            tempItemDisplayRules.GenerateRuntimeValues();

            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayBrotherHurt = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.05f, 0.05f, 0.05f),
                            localAngles = new Vector3(0f,350f,0f),
                            localScale = new Vector3(0.12f, 0.12f, 0.12f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.05f, 0.05f, 0.05f),
                            localAngles = new Vector3(0f,10f,0f),
                            localScale = new Vector3(-0.12f, 0.12f, 0.12f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixBrotherHurtDisplay = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0f, 0.25f),
                            localAngles = originallunargolemrule.rules[0].localAngles,
                            localScale = new Vector3(0.3f, 0.3f, 0.75f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            //tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixBrotherHurtDisplay, RedAffixDisplayBrotherHurt, BlueAffixDisplayBrother, PoisonAffixDisplayBrother, HauntedAffixDisplayBrother, WhiteAffixDisplayBrother);
            tempItemDisplayRules.GenerateRuntimeValues();




            //Engi Turret garbage
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.65f, 1.6f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.25f, 1.25f, 1.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.8f, 0.8f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(1.35f, 1.35f, 1.5f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.3f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.8f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.4f, -0.45f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 0.7f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0, -0.25f, 0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 1.5f, 1.25f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0, 0f, 0.25f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.75f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };




            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.65f, 0.5f, -0.1f),
                            localAngles = new Vector3(60f,0f,0f),
                            localScale = new Vector3(-0.4f, 0.35f, 0.35f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.65f, 0.5f, -0.1f),
                            localAngles = new Vector3(60f,0f,0f),
                            localScale = new Vector3(0.4f, 0.35f, 0.35f),
                            limbMask = LimbFlags.None
                        },
        }
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(-0.4f, 1f, -0.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(-0.45f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0.4f, 1f, -0.5f),
                            localAngles = new Vector3(30f,0f,0f),
                            localScale = new Vector3(0.45f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BatteryDisplayEngiTurretWalker = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.QuestVolatileBattery,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayVolatileBatteryPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.65f, -0.5f),
                            localAngles = new Vector3(270f,180f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BatteryDisplayEngiTurret = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.QuestVolatileBattery,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayVolatileBatteryPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1f, 0),
                            localAngles = new Vector3(270f,180f,0f),
                            localScale = new Vector3(1.2f, 1.2f, 1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiTurretBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayEngiTurret, BlueAffixDisplayEngiTurretWalker, PoisonAffixDisplayEngiTurret, HauntedAffixDisplayEngiTurret, WhiteAffixDisplayEngiTurret, LunarAffixDisplayEngiTurret, BatteryDisplayEngiTurret);
            tempItemDisplayRules.GenerateRuntimeValues();

            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayEngiTurretWalker, BlueAffixDisplayEngiTurretWalker, PoisonAffixDisplayEngiTurretWalker, HauntedAffixDisplayEngiTurretWalker, WhiteAffixDisplayEngiTurretWalker, LunarAffixDisplayEngiTurretWalker, BatteryDisplayEngiTurretWalker);
            tempItemDisplayRules.GenerateRuntimeValues();





            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0.5f, 0.5f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.45f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(-0.5f, 0.5f, 0f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(-0.45f, 0.4f, 0.4f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 0.8f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 1f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.1f, 0.1f, 0.1f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0f, 0.7f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayLunarExploder = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0, 0f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1.25f, 1.25f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "MuzzleCore",
                            localPos = new Vector3(0, 0.4f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarExploderBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayLunarExploder, BlueAffixDisplayLunarExploder, PoisonAffixDisplayLunarExploder, HauntedAffixDisplayLunarExploder, WhiteAffixDisplayLunarExploder);
            tempItemDisplayRules.GenerateRuntimeValues();






            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "MuzzleRT",
                            localPos = new Vector3(0.1f, -0.1f, -0.7f),
                            localAngles = new Vector3(320f,335f,180f),
                            localScale = new Vector3(0.5f, 0.45f, 0.45f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "MuzzleLT",
                            localPos = new Vector3(-0.1f, -0.1f, -0.7f),
                            localAngles = new Vector3(320f,25f,180f),
                            localScale = new Vector3(-0.5f, 0.45f, 0.45f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.5f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 2f, 0f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.15f, 0.15f, 0.15f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Head",
                            localPos = new Vector3(0f, 1.4f, 0.5f),
                            localAngles = new Vector3(270f,0f,0f),
                            localScale = new Vector3(0.25f, 0.25f, 0.25f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayLunarGolem = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0, 0f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1.25f, 1.25f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Head",
                            localPos = new Vector3(0, 0.4f, 1f),
                            localAngles = new Vector3(325f,0f,0f),
                            localScale = new Vector3(1f, 1f, 1f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarGolemBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayLunarGolem, BlueAffixDisplayLunarGolem, PoisonAffixDisplayLunarGolem, HauntedAffixDisplayLunarGolem, WhiteAffixDisplayLunarGolem);
            tempItemDisplayRules.GenerateRuntimeValues();



            ItemDisplayRuleSet.KeyAssetRuleGroup RedAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Mask",
                            localPos = new Vector3(0.5f, -1f, 2f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(1.2f, 1.2f, 1.2f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixRedPrefab,
                            childName = "Mask",
                            localPos = new Vector3(-0.5f, -1f, 2f),
                            localAngles = new Vector3(90f,0f,0f),
                            localScale = new Vector3(-1.2f, 1.2f, 1.2f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup HauntedAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixHaubtedPrefab,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 3f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.5f, 0.5f, 0.5f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup WhiteAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixWhitePrefab,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 3.5f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            ItemDisplayRuleSet.KeyAssetRuleGroup PoisonAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixPoisonPrefab,
                            childName = "Mask",
                            localPos = new Vector3(0f, 0f, 2.5f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.45f, 0.45f, 0.45f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };

            ItemDisplayRuleSet.KeyAssetRuleGroup BlueAffixDisplayLunarWisp = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Mask",
                            localPos = new Vector3(0, -3f, 1.1f),
                            localAngles = new Vector3(50f,0f,0f),
                            localScale = new Vector3(1.6f, 1.6f, 1.6f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = DisplayAffixBluePrefab,
                            childName = "Mask",
                            localPos = new Vector3(0, -2.5f, 1.8f),
                            localAngles = new Vector3(50f,0f,0f),
                            localScale = new Vector3(1.4f, 1.2f, 1.2f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/LunarWispBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(RedAffixDisplayLunarWisp, BlueAffixDisplayLunarWisp, PoisonAffixDisplayLunarWisp, HauntedAffixDisplayLunarWisp, WhiteAffixDisplayLunarWisp);
            tempItemDisplayRules.GenerateRuntimeValues();





















            /*
            ItemDisplayRuleSet.KeyAssetRuleGroup LunarAffixDisplayNewt = new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = originallunargolemrule.rules[0].followerPrefab,
                            childName = "head", //needs to be changed
                            localPos = new Vector3(0f, 0.2f, 0.2f),
                            localAngles = new Vector3(0f,0f,0f),
                            localScale = new Vector3(0.3f, 0.3f, 0.3f),
                            limbMask = LimbFlags.None
                        },
}
                }
            };
            tempItemDisplayRules = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ShopkeeperBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;
            tempItemDisplayRules.keyAssetRuleGroups = tempItemDisplayRules.keyAssetRuleGroups.Add(LunarAffixDisplayNewt);
            tempItemDisplayRules.GenerateRuntimeValues();
            */

        }


        public static void VoidDisplaysMithrix()
        {
            //Mithrix Void Stuff
            ItemDisplayRuleSet tempIDRS = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponentInChildren<RoR2.CharacterModel>().itemDisplayRuleSet;

            foreach (RoR2.Items.ContagiousItemManager.TransformationInfo transformationInfo in RoR2.Items.ContagiousItemManager.transformationInfos)
            {
                DisplayRuleGroup originalDisplayRule = tempIDRS.GetItemDisplayRuleGroup(transformationInfo.originalItem);
                //Debug.LogWarning(originalDisplayRule + "  " + originalDisplayRule.isEmpty);

                ItemDef transformedItem = ItemCatalog.GetItemDef(transformationInfo.transformedItem);

                GameObject FollowerPrefab = LunarEliteDisplay.MithrixCrystalPink;
                if (transformedItem.tier == ItemTier.VoidTier1)
                {
                    FollowerPrefab = LunarEliteDisplay.MithrixCrystalPinkSmall;
                }

                if (originalDisplayRule.isEmpty == false)
                {

                    //originalDisplayRule.rules

                    ItemDisplayRule[] newDisplayRules = new ItemDisplayRule[0];
                    for (int i = 0; i < originalDisplayRule.rules.Length; i++)
                    {
                        ItemDisplayRule newRule = new ItemDisplayRule
                        {
                            ruleType = originalDisplayRule.rules[i].ruleType,
                            followerPrefab = FollowerPrefab,
                            childName = originalDisplayRule.rules[i].childName,
                            localPos = originalDisplayRule.rules[i].localPos,
                            localAngles = originalDisplayRule.rules[i].localAngles,
                            localScale = originalDisplayRule.rules[i].localScale,
                            limbMask = originalDisplayRule.rules[i].limbMask,
                        };
                        newDisplayRules = newDisplayRules.Add(newRule);
                    }


                    ItemDisplayRuleSet.KeyAssetRuleGroup keyAssetGroup = new ItemDisplayRuleSet.KeyAssetRuleGroup
                    {
                        keyAsset = transformedItem,
                        displayRuleGroup = new DisplayRuleGroup
                        {
                            rules = newDisplayRules,
                        }
                    };

                    tempIDRS.keyAssetRuleGroups = tempIDRS.keyAssetRuleGroups.Add(keyAssetGroup);
                }
            }

            tempIDRS.GenerateRuntimeValues();
        }
    }
}