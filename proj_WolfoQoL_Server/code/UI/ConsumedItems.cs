using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using WolfoQoL_Client;
using WolfoQoL_Client.ModSupport;


namespace WolfoQoL_Server
{

    public class ConsumedItems
    {
        public static ItemDef UsedRustedKey;
        public static ItemDef UsedEncrustedKey;
        public static ItemDef UsedPrayerBeads;

        public static ItemDef[] Quality_UsedRustedKey;
        public static ItemDef[] Quality_UsedEncrustedKey;
        public static ItemDef[] Quality_UsedPrayerBead;

        public static void Start()
        {
            CreateConsumedItems();
            if (WQoLMain.QualityModInstalled)
            {
                Quality_CreateConsumedItems();
                RoR2Application.onLoad += Quality_NameConsumedItems; 
            }
            if (WConfig.cfgIconsUsedKey.Value == true)
            {
                //On.RoR2.PurchaseInteraction.OnInteractionBegin += UsedKeyGiver;
            }
            if (WConfig.cfgIconsUsedPrayer.Value)
            {
                On.RoR2.CharacterMaster.OnBeadReset += CharacterMaster_OnBeadReset;
            }

            PurchaseInteraction.onPurchaseGlobalServer += UsedKeyGiver_PostAC;
        }

        private static void UsedKeyGiver_PostAC(CostTypeDef.PayCostContext context, CostTypeDef.PayCostResults result)
        {
            if (context.purchaseInteraction.costType == CostTypeIndex.TreasureCacheItem ||
                context.purchaseInteraction.costType == CostTypeIndex.TreasureCacheVoidItem)
            {
                bool VoidKey = context.purchaseInteraction.costType == CostTypeIndex.TreasureCacheVoidItem;
                ItemIndex indexTaken = result.itemStacksTaken[0].itemIndex;
                ItemIndex indexToGive = UsedRustedKey.itemIndex;

                
                if (WolfoQoL_Client.WQoLMain.QualityModInstalled)
                {
                    indexToGive = IndexToQuality(indexTaken, VoidKey ? Quality_UsedEncrustedKey : Quality_UsedRustedKey);
                }
          
                context.activatorInventory.GiveItemPermanent(indexToGive, (int)result.itemStacksTaken[0].stackValues.permanentStacks);
                context.activatorInventory.GiveItemTemp(indexToGive, result.itemStacksTaken[0].stackValues.temporaryStacksValue);
                CharacterMasterNotificationQueue.PushItemTransformNotification(context.activatorMaster, indexTaken, indexToGive, CharacterMasterNotificationQueue.TransformationType.Default);
            }
        }

        public static ItemIndex IndexToQuality(ItemIndex itemIndex, ItemDef[] itemArray)
        {
            int qTier = (int)ItemQualities.QualityCatalog.GetQualityTier(itemIndex) + 1;
            return itemArray[qTier].itemIndex;

        }

        public static void CreateConsumedItems()
        {
            ItemDef RustedKey = Addressables.LoadAssetAsync<ItemDef>(key: "afd6938eb60552c46a0f6fec67b9da7b").WaitForCompletion();
            ItemDef EncrustedKey = Addressables.LoadAssetAsync<ItemDef>(key: "d687f389f660f344cb6c09d4b9c8b272").WaitForCompletion();
            ItemDef PrayerBeads = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC2/Items/ExtraStatsOnLevelUp/ExtraStatsOnLevelUp.asset").WaitForCompletion();

            UsedRustedKey = MakeConsumedItemDef(RustedKey, "UsedKey");
            UsedEncrustedKey = MakeConsumedItemDef(EncrustedKey, "UsedKeyVoid");
            UsedPrayerBeads = MakeConsumedItemDef(PrayerBeads, "UsedBeads");
 
        }
        public static void Quality_CreateConsumedItems()
        {
            ItemDef RustedKey = Addressables.LoadAssetAsync<ItemDef>(key: "afd6938eb60552c46a0f6fec67b9da7b").WaitForCompletion();
            ItemDef EncrustedKey = Addressables.LoadAssetAsync<ItemDef>(key: "d687f389f660f344cb6c09d4b9c8b272").WaitForCompletion();
            ItemDef PrayerBeads = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC2/Items/ExtraStatsOnLevelUp/ExtraStatsOnLevelUp.asset").WaitForCompletion();

            Quality_UsedRustedKey = new ItemDef[]
            {
                UsedRustedKey,
                MakeConsumedItemDef(RustedKey, "Quality/UsedKey_Q1",1),
                MakeConsumedItemDef(RustedKey, "Quality/UsedKey_Q2",2),
                MakeConsumedItemDef(RustedKey, "Quality/UsedKey_Q3",3),
                MakeConsumedItemDef(RustedKey, "Quality/UsedKey_Q4",4),
            };
            Quality_UsedEncrustedKey = new ItemDef[]
            {
                UsedEncrustedKey,
                MakeConsumedItemDef(EncrustedKey, "Quality/UsedKeyVoid_Q1",1),
                MakeConsumedItemDef(EncrustedKey, "Quality/UsedKeyVoid_Q2",2),
                MakeConsumedItemDef(EncrustedKey, "Quality/UsedKeyVoid_Q3",3),
                MakeConsumedItemDef(EncrustedKey, "Quality/UsedKeyVoid_Q4",4),
            };
            Quality_UsedPrayerBead = new ItemDef[]
            {
                UsedPrayerBeads,
                MakeConsumedItemDef(PrayerBeads, "Quality/UsedBeads_Q1",1),
                MakeConsumedItemDef(PrayerBeads, "Quality/UsedBeads_Q2",2),
                MakeConsumedItemDef(PrayerBeads, "Quality/UsedBeads_Q3",3),
                MakeConsumedItemDef(PrayerBeads, "Quality/UsedBeads_Q4",4),
            };
        }
        
        public static void Quality_NameConsumedItems()
        {
            for (int i = 1; i < 5; i++)
            {
                MakeQualityNameTokens(Quality_UsedRustedKey[i], i);
                MakeQualityNameTokens(Quality_UsedEncrustedKey[i], i);
                MakeQualityNameTokens(Quality_UsedPrayerBead[i], i);
            }
            
        }

        private static void CharacterMaster_OnBeadReset(On.RoR2.CharacterMaster.orig_OnBeadReset orig, CharacterMaster self, bool gainedStats)
        {
            orig(self, gainedStats);
            if (gainedStats && NetworkServer.active)
            {
                self.inventory.GiveItemPermanent(UsedPrayerBeads, 1);
                //Text handled on Client Mod
            }
        }


        public static ItemDef MakeConsumedItemDef(ItemDef original, string sprite, int quality = 0)
        {
            ItemDef newItemDef = ScriptableObject.CreateInstance<ItemDef>();
            newItemDef.name = original.name+"Consumed";
            string arg = newItemDef.name.ToUpperInvariant();
            newItemDef.nameToken = string.Format("ITEM_{0}_NAME", arg);
            newItemDef.descriptionToken = string.Format("ITEM_{0}_DESC", arg);
            newItemDef.pickupToken = newItemDef.descriptionToken;
            if (quality > 0)
            {
                newItemDef.name = original.name + "Consumed" + "_Q" + quality;
            }
     
            newItemDef.deprecatedTier = ItemTier.NoTier;
            newItemDef.pickupModelPrefab = original.pickupModelPrefab;
            newItemDef.pickupModelReference = original.pickupModelReference;
            newItemDef.pickupIconSprite = Assets.Bundle.LoadAsset<Sprite>($"Assets/WQoL/Items/{sprite}.png");
            newItemDef.canRemove = false;
            newItemDef.isConsumed = true;
            ItemAPI.Add(new CustomItem(newItemDef, (ItemDisplayRuleDict)null));
            return newItemDef;
        }
     
        public static void MakeQualityNameTokens(ItemDef qualityDef, int quality)
        {
            string qName = qualityDef.nameToken + "_Q" + quality;
            string qModToken = string.Empty;
            switch (quality)
            {
                case 1:
                    qModToken = "QUALITY_UNCOMMON_CONSUMED_MODIFIER";
                    break;
                case 2:
                    qModToken = "QUALITY_RARE_CONSUMED_MODIFIER";
                    break;
                case 3:
                    qModToken = "QUALITY_EPIC_CONSUMED_MODIFIER";
                    break;
                case 4:
                    qModToken = "QUALITY_LEGENDARY_CONSUMED_MODIFIER";
                    break;
            }
            foreach (Language language in Language.GetAllLanguages())
            {
                string localizedFormattedStringByToken = language.GetLocalizedFormattedStringByToken(qModToken, new object[]
                {
                        language.GetLocalizedStringByToken(qualityDef.nameToken)
                });
                LanguageAPI.Add(qName, localizedFormattedStringByToken, language.name);
            }
            qualityDef.nameToken = qName;
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
