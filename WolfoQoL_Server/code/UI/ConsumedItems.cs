using R2API;
using RoR2;
using System.Drawing;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;


namespace WolfoQoL_Server
{

    public class ConsumedItems
    {
        public static ItemDef UsedRustedKey;
        public static ItemDef UsedEncrustedKey;
        public static ItemDef UsedPrayerBeads;
 
        public static void Start()
        {
            CreateItems();
            if (WConfig.cfgIconsUsedKey.Value == true)
            {
                On.RoR2.PurchaseInteraction.OnInteractionBegin += UsedKeyGiver;
            }
            if (WConfig.cfgIconsUsedPrayer.Value)
            {
                On.RoR2.CharacterMaster.OnBeadReset += CharacterMaster_OnBeadReset;
 
            }
        }
 

        private static void CharacterMaster_OnBeadReset(On.RoR2.CharacterMaster.orig_OnBeadReset orig, CharacterMaster self, bool gainedStats)
        {
            orig(self, gainedStats);
            if (gainedStats && NetworkServer.active)
            {
                self.inventory.GiveItem(UsedPrayerBeads, 1);
                //Text handled on Client Mod
            }
        }

        public static void CreateItems()
        {
            #region Key Consumed
            UsedRustedKey = ScriptableObject.CreateInstance<ItemDef>(); 
            UsedRustedKey.name = "TreasureCacheConsumed";
            UsedRustedKey.deprecatedTier = ItemTier.NoTier;
            UsedRustedKey.pickupModelPrefab = LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCache").pickupModelPrefab;
            UsedRustedKey.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/UsedKey.png");
            UsedRustedKey.nameToken = "ITEM_TREASURECACHECONSUMED_NAME";
            UsedRustedKey.pickupToken = "ITEM_TREASURECACHECONSUMED_DESC";
            UsedRustedKey.descriptionToken = "ITEM_TREASURECACHECONSUMED_DESC";
            UsedRustedKey.loreToken = "";
            UsedRustedKey.hidden = false;
            UsedRustedKey.canRemove = false;
            UsedRustedKey.isConsumed = true;
            UsedRustedKey.tags = new ItemTag[]
            {
                ItemTag.WorldUnique
            };
            CustomItem customItem = new CustomItem(UsedRustedKey, new ItemDisplayRule[0]);
            ItemAPI.Add(customItem);
            #endregion
            #region Void Key
            UsedEncrustedKey = ScriptableObject.CreateInstance<ItemDef>();
            UsedEncrustedKey.name = "TreasureCacheVoidConsumed";
            UsedEncrustedKey.deprecatedTier = ItemTier.NoTier;
            UsedEncrustedKey.pickupModelPrefab = LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCacheVoid").pickupModelPrefab;
            UsedEncrustedKey.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/UsedKeyVoid.png");
            UsedEncrustedKey.nameToken = "ITEM_TREASURECACHEVOIDCONSUMED_NAME";
            UsedEncrustedKey.pickupToken = "ITEM_TREASURECACHEVOIDCONSUMED_DESC";
            UsedEncrustedKey.descriptionToken = "ITEM_TREASURECACHEVOIDCONSUMED_DESC";
            UsedEncrustedKey.loreToken = "";
            UsedEncrustedKey.hidden = false;
            UsedEncrustedKey.canRemove = false;
            UsedEncrustedKey.isConsumed = true;
            UsedEncrustedKey.tags = new ItemTag[]
            {
                ItemTag.WorldUnique
            };
            CustomItem customItem2 = new CustomItem(UsedEncrustedKey, new ItemDisplayRule[0]);
            ItemAPI.Add(customItem2);
            #endregion
            #region Prayer Beads 2
            ItemDef PrayerBeads = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC2/Items/ExtraStatsOnLevelUp/ExtraStatsOnLevelUp.asset").WaitForCompletion();
 
            UsedPrayerBeads = ScriptableObject.CreateInstance<ItemDef>();
            UsedPrayerBeads.name = "ExtraStatsOnLevelUpConsumed";
            UsedPrayerBeads.deprecatedTier = ItemTier.NoTier;
            UsedPrayerBeads.pickupModelPrefab = PrayerBeads.pickupModelPrefab;
            UsedPrayerBeads.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/Items/UsedBeads.png");
            UsedPrayerBeads.nameToken = "ITEM_EXTRASTATSONLEVELUP_CONSUMED_NAME";
            UsedPrayerBeads.pickupToken = "ITEM_EXTRASTATSONLEVELUP_CONSUMED_PICKUP";
            UsedPrayerBeads.descriptionToken = "ITEM_EXTRASTATSONLEVELUP_CONSUMED_DESC";
            UsedPrayerBeads.loreToken = "Lore?";
            UsedPrayerBeads.hidden = false;
            UsedPrayerBeads.canRemove = false;
            UsedPrayerBeads.isConsumed = true;
            UsedPrayerBeads.tags = new ItemTag[]
            {
                ItemTag.WorldUnique
            };
            customItem2 = new CustomItem(UsedPrayerBeads, new ItemDisplayRule[0]);
            ItemAPI.Add(customItem2);
            #endregion
        }

        public static void UsedKeyGiver(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            orig(self, activator);
            if (NetworkServer.active && self.available == false)
            {
                if (self.costType == CostTypeIndex.TreasureCacheItem)
                {
                    activator.GetComponent<CharacterBody>().inventory.GiveItem(UsedRustedKey, self.cost);
                    CharacterMasterNotificationQueue.PushItemTransformNotification(activator.GetComponent<CharacterBody>().master, RoR2Content.Items.TreasureCache.itemIndex, UsedRustedKey.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                }
                else if (self.costType == CostTypeIndex.TreasureCacheVoidItem)
                {
                    activator.GetComponent<CharacterBody>().inventory.GiveItem(UsedEncrustedKey, self.cost);
                    CharacterMasterNotificationQueue.PushItemTransformNotification(activator.GetComponent<CharacterBody>().master, DLC1Content.Items.TreasureCacheVoid.itemIndex, UsedEncrustedKey.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
                }
            }
        }

    }

}
