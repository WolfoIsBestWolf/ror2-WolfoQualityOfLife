using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;


namespace WolfoQualityOfLife
{
    public class ExtraIcons
    {
        public static ItemDef UsedRustedKey;
        public static ItemDef UsedEncrustedKey;

        public static void Start()
        {
            if (WConfig.cfgIconsBodyIcons.Value == true)
            {
                NewBodyIcons();
            }
            if (WConfig.cfgIconsUsedKey.Value == true)
            {
                On.RoR2.PurchaseInteraction.OnInteractionBegin += UsedKeyGiver;
            }
            CreateUsedKey(); 
        }

        public static void NewBodyIcons()
        {
            Texture2D TexBodyBrotherHurt = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexBodyBrotherHurt.LoadImage(Properties.Resources.texBodyBrotherHurt, true);
            TexBodyBrotherHurt.filterMode = FilterMode.Bilinear;
            TexBodyBrotherHurt.wrapMode = TextureWrapMode.Clamp;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrotherHurt;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHauntBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrotherHurt;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHauntBody").GetComponent<CharacterBody>().baseNameToken = "BROTHER_BODY_NAME";
            //LanguageAPI.Add("BROTHERHURT_BODY_NAME", "Hurt Brother", "en");
            //RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponent<CharacterBody>().baseNameToken = "BROTHERHURT_BODY_NAME";

            Texture2D TexWalkerTurretIcon = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexWalkerTurretIcon.LoadImage(Properties.Resources.texBodyEngiWalkerTurret, true);
            TexWalkerTurretIcon.filterMode = FilterMode.Bilinear;
            TexWalkerTurretIcon.wrapMode = TextureWrapMode.Clamp;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody").GetComponent<CharacterBody>().portraitIcon = TexWalkerTurretIcon;

            Texture2D TexProbeGreen = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexProbeGreen.LoadImage(Properties.Resources.texBodyRoboBallBuddyGreen, true);
            TexProbeGreen.filterMode = FilterMode.Bilinear;
            TexProbeGreen.wrapMode = TextureWrapMode.Clamp;
            Texture2D TexProbeRed = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexProbeRed.LoadImage(Properties.Resources.texBodyRoboBallBuddyRed, true);
            TexProbeRed.filterMode = FilterMode.Bilinear;
            TexProbeRed.wrapMode = TextureWrapMode.Clamp;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallRedBuddyBody").GetComponent<CharacterBody>().portraitIcon = TexProbeRed;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallGreenBuddyBody").GetComponent<CharacterBody>().portraitIcon = TexProbeGreen;

            //Why needs to be run late? Also move to some body icon thing or whatever
            Texture2D TexBodyBrother = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexBodyBrother.LoadImage(Properties.Resources.texBodyBrother, true);
            TexBodyBrother.filterMode = FilterMode.Bilinear;
            TexBodyBrother.wrapMode = TextureWrapMode.Clamp;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrother;


            Texture2D TexBlueSquidTurret = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexBlueSquidTurret.LoadImage(Properties.Resources.texBodySquidTurret, true);
            TexBlueSquidTurret.filterMode = FilterMode.Bilinear;
            TexBlueSquidTurret.wrapMode = TextureWrapMode.Clamp;

            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SquidTurretBody").GetComponent<CharacterBody>().portraitIcon = TexBlueSquidTurret;

        }



        public static void CreateUsedKey()
        {
            //ItemDef ExtraLifeConsumed = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/ExtraLifeConsumed");
            //ItemDef TreasureCacheVoid = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCacheVoid");

            Texture2D TexUsedRustedKey = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexUsedRustedKey.LoadImage(Properties.Resources.texItemUsedKey, true);
            TexUsedRustedKey.filterMode = FilterMode.Bilinear;
            TexUsedRustedKey.wrapMode = TextureWrapMode.Clamp;
            Sprite TexUsedRustedKeyS = Sprite.Create(TexUsedRustedKey, v.rec128, v.half);

            UsedRustedKey = ScriptableObject.CreateInstance<ItemDef>();

            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Rusted Key (Consumed)", "en");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Verrosteter Schlüssel (Verbraucht)", "de");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Clé rouillée (utilisé)", "FR");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Chiave arrugginita (Consumato)", "IT");
            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_NAME", "Llave oxidada (consumido)", "es-419");


            LanguageAPI.Add("ITEM_TREASURECACHECONSUMED_DESC", "A spent key to remember an item well earned.", "en");

            UsedRustedKey.name = "TreasureCacheConsumed";
            UsedRustedKey.deprecatedTier = ItemTier.NoTier;
            UsedRustedKey.pickupModelPrefab = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCache").pickupModelPrefab;
            UsedRustedKey.pickupIconSprite = TexUsedRustedKeyS;
            UsedRustedKey.nameToken = "ITEM_TREASURECACHECONSUMED_NAME";
            UsedRustedKey.pickupToken = "ITEM_TREASURECACHECONSUMED_DESC";
            UsedRustedKey.descriptionToken = "ITEM_TREASURECACHECONSUMED_DESC";
            UsedRustedKey.loreToken = "";
            UsedRustedKey.hidden = false;
            UsedRustedKey.canRemove = false;
            UsedRustedKey.tags = new ItemTag[]
            {
                ItemTag.WorldUnique
            };
            CustomItem customItem = new CustomItem(UsedRustedKey, new ItemDisplayRule[0]);
            ItemAPI.Add(customItem);



            Texture2D texItemUsedKeyVoid = new Texture2D(128, 128, TextureFormat.DXT5, false);
            texItemUsedKeyVoid.LoadImage(Properties.Resources.texItemUsedKeyVoid, true);
            texItemUsedKeyVoid.filterMode = FilterMode.Bilinear;
            texItemUsedKeyVoid.wrapMode = TextureWrapMode.Clamp;
            Sprite texItemUsedKeyVoidS = Sprite.Create(texItemUsedKeyVoid, v.rec128, v.half);

            UsedEncrustedKey = ScriptableObject.CreateInstance<ItemDef>();

            LanguageAPI.Add("ITEM_TREASURECACHEVOIDCONSUMED_NAME", "Encrusted Key (Consumed)", "en");
            LanguageAPI.Add("ITEM_TREASURECACHEVOIDCONSUMED_DESC", "A spent key to remember an item well earned.", "en");

            UsedEncrustedKey.name = "TreasureCacheVoidConsumed";
            UsedEncrustedKey.deprecatedTier = ItemTier.NoTier;
            UsedEncrustedKey.pickupModelPrefab = RoR2.LegacyResourcesAPI.Load<ItemDef>("itemdefs/TreasureCacheVoid").pickupModelPrefab;
            UsedEncrustedKey.pickupIconSprite = texItemUsedKeyVoidS;
            UsedEncrustedKey.nameToken = "ITEM_TREASURECACHEVOIDCONSUMED_NAME";
            UsedEncrustedKey.pickupToken = "ITEM_TREASURECACHEVOIDCONSUMED_DESC";
            UsedEncrustedKey.descriptionToken = "ITEM_TREASURECACHEVOIDCONSUMED_DESC";
            UsedEncrustedKey.loreToken = "";
            UsedEncrustedKey.hidden = false;
            UsedEncrustedKey.canRemove = false;
            UsedEncrustedKey.tags = new ItemTag[]
            {
                ItemTag.WorldUnique
            };
            CustomItem customItem2 = new CustomItem(UsedEncrustedKey, new ItemDisplayRule[0]);
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
