using R2API;
using RoR2;
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
                //Overlay handled in client mod;
                self.inventory.GiveItem(UsedPrayerBeads, 1);
                CharacterMasterNotificationQueue.SendTransformNotification(self, DLC2Content.Items.ExtraStatsOnLevelUp.itemIndex, UsedPrayerBeads.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);
            }
        }

        public static void CreateItems()
        {
            //ItemDef ExtraLifeConsumed = LegacyResourcesAPI.Load<ItemDef>("itemdefs/ExtraLifeConsumed");
            //ItemDef TreasureCacheVoid = LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCacheVoid");

            Texture2D TexUsedRustedKey = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texItemUsedKey.png");
            TexUsedRustedKey.wrapMode = TextureWrapMode.Clamp;
            Sprite TexUsedRustedKeyS = Sprite.Create(TexUsedRustedKey, v.rec128, v.half);

            UsedRustedKey = ScriptableObject.CreateInstance<ItemDef>();

            /*LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Rusted Key (Consumed)", "en");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Verrosteter Schlüssel (Verbraucht)", "de");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Clé rouillée (utilisé)", "FR");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Chiave arrugginita (Consumato)", "IT");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Llave oxidada (consumido)", "es-419");*/

            UsedRustedKey.name = "TreasureCacheConsumed";
            UsedRustedKey.deprecatedTier = ItemTier.NoTier;
            UsedRustedKey.pickupModelPrefab = LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCache").pickupModelPrefab;
            UsedRustedKey.pickupIconSprite = TexUsedRustedKeyS;
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



            Texture2D texItemUsedKeyVoid = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texItemUsedKeyVoid.png");
            texItemUsedKeyVoid.wrapMode = TextureWrapMode.Clamp;
            Sprite texItemUsedKeyVoidS = Sprite.Create(texItemUsedKeyVoid, v.rec128, v.half);

            UsedEncrustedKey = ScriptableObject.CreateInstance<ItemDef>();

            //LanguageAPI.Add("ITEM_TREASURECACHEVOIDCONSUMED_NAME", "Encrusted Key (Consumed)", "en");
            //LanguageAPI.Add("ITEM_TREASURECACHEVOIDCONSUMED_DESC", "A spent key to remember an item well earned.", "en");

            UsedEncrustedKey.name = "TreasureCacheVoidConsumed";
            UsedEncrustedKey.deprecatedTier = ItemTier.NoTier;
            UsedEncrustedKey.pickupModelPrefab = LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCacheVoid").pickupModelPrefab;
            UsedEncrustedKey.pickupIconSprite = texItemUsedKeyVoidS;
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
            //
            //
            ItemDef PrayerBeads = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC2/Items/ExtraStatsOnLevelUp/ExtraStatsOnLevelUp.asset").WaitForCompletion();
            UsedPrayerBeads = ScriptableObject.CreateInstance<ItemDef>();

            Texture2D texItemUsedPrayer = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texItemUsedPrayer.png");
            texItemUsedPrayer.wrapMode = TextureWrapMode.Clamp;
            Sprite texItemUsedPrayerS = Sprite.Create(texItemUsedPrayer, v.rec256, v.half);

            /*LanguageAPI.Add("ITEM_EXTRASTATSONLEVELUP_CONSUMED_NAME", "Prayer Beads Blessing", "en");
            LanguageAPI.Add("ITEM_EXTRASTATSONLEVELUP_CONSUMED_PICKUP", "Your prayer beads have blessed your stats.", "en");
            LanguageAPI.Add("ITEM_EXTRASTATSONLEVELUP_CONSUMED_DESC", "Your prayer beads have blessed your stats. (Missing Description)", "en");*/

            UsedPrayerBeads.name = "ExtraStatsOnLevelUpConsumed";
            UsedPrayerBeads.deprecatedTier = ItemTier.NoTier;
            UsedPrayerBeads.pickupModelPrefab = PrayerBeads.pickupModelPrefab;
            UsedPrayerBeads.pickupIconSprite = texItemUsedPrayerS;
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
