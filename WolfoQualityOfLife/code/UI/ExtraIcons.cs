using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;


namespace WolfoQualityOfLife
{
    public class ExtraIcons
    {
        public static ItemDef UsedRustedKey;
        public static ItemDef UsedEncrustedKey;
        public static ItemDef UsedPrayerBeads;
        public static LanguageAPI.LanguageOverlay UsedPeadsOverlay_Pickup;
        public static LanguageAPI.LanguageOverlay UsedPeadsOverlay_Desc;
        public static float LevelsGiven = 0f;

        public static void Start()
        {
            if (WConfig.cfgIconsBodyIcons.Value == true)
            {
                NewBodyIcons();
            }
            if (WConfig.cfgIconsUsedKey.Value == true)
            {
                On.RoR2.PurchaseInteraction.OnInteractionBegin += UsedKeyGiver;
                On.RoR2.CharacterMaster.OnBeadReset += CharacterMaster_OnBeadReset;
               
            }

            CreateUsedKey();
        }

        private static void CharacterMaster_OnBeadReset(On.RoR2.CharacterMaster.orig_OnBeadReset orig, CharacterMaster self, bool gainedStats)
        {
            if(gainedStats && NetworkServer.active)
            {
                //self.inventory.GiveItem(UsedPrayerBeads, (int)(self.newBeadLevel - 1));
                self.inventory.GiveItem(UsedPrayerBeads, 1);
            }
            orig(self, gainedStats);

            //Check if dude is like local
            if (self.hasAuthority)
            {
               
                if (self.GetBody())
                {
                    
                    float bonusLevels = self.inventory.beadAppliedDamage / self.GetBody().levelDamage;
                    string bonusStat0 = bonusLevels.ToString("0.##");
                    string bonusStat1 = self.inventory.beadAppliedHealth.ToString("0.##");
                    string bonusStat2 = self.inventory.beadAppliedRegen.ToString("0.##");
                    string bonusStat3 = self.inventory.beadAppliedDamage.ToString("0.##");
                    Debug.Log(bonusLevels);
                  
                    UsedPeadsOverlay_Pickup.Remove();
                    UsedPeadsOverlay_Pickup = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_CONSUMED_PICKUP", "Your prayer beads have blessed you with +" + bonusStat0 + " levels.", "en");

                    UsedPeadsOverlay_Desc.Remove();
                    UsedPeadsOverlay_Desc = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_CONSUMED_DESC", "Your prayer beads have blessed you with <style=cisUtility>+" + bonusStat0 + " levels</style>, increasing <style=cIsHealing>max health</style> by <style=cIsHealing>" + bonusStat1 + "</style>, <style=cIsHealing>health regeneration</style> by <style=cIsHealing>"+ bonusStat2 + " hp/s</style>, and <style=cIsDamage>damage</style> by <style=cIsDamage>"+ bonusStat3 + "</style>.", "en");

                    CharacterMasterNotificationQueue.PushItemTransformNotification(self, DLC2Content.Items.ExtraStatsOnLevelUp.itemIndex, UsedPrayerBeads.itemIndex, CharacterMasterNotificationQueue.TransformationType.Default);

                }
            }



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

            GameObject SoulLesserWispBody = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/WispSoulBody");
            
            Texture2D TexSoulWisp = new Texture2D(128, 128, TextureFormat.DXT5, false);
            TexSoulWisp.LoadImage(Properties.Resources.texBodyWispSoul, true);
            TexSoulWisp.filterMode = FilterMode.Bilinear;
            TexSoulWisp.wrapMode = TextureWrapMode.Clamp;

            SoulLesserWispBody.GetComponent<CharacterBody>().portraitIcon = TexSoulWisp;


            //Devoted Lemurians
            Texture2D texBodyDevotedLemurian = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texBodyDevotedLemurian.LoadImage(Properties.Resources.texBodyDevotedLemurian, true);
            texBodyDevotedLemurian.filterMode = FilterMode.Bilinear;
            texBodyDevotedLemurian.wrapMode = TextureWrapMode.Clamp;

            Texture2D texBodyDevotedElder = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texBodyDevotedElder.LoadImage(Properties.Resources.texBodyDevotedElder, true);
            texBodyDevotedElder.filterMode = FilterMode.Bilinear;
            texBodyDevotedElder.wrapMode = TextureWrapMode.Clamp;

            GameObject DevotedLemurian = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBody.prefab").WaitForCompletion();
            GameObject DevotedLemurianElder = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBruiserBody.prefab").WaitForCompletion();

            DevotedLemurian.GetComponent<CharacterBody>().portraitIcon = texBodyDevotedLemurian;
            DevotedLemurianElder.GetComponent<CharacterBody>().portraitIcon = texBodyDevotedElder;
            DevotedLemurian.GetComponent<DeathRewards>().logUnlockableDef = null;
            DevotedLemurianElder.GetComponent<DeathRewards>().logUnlockableDef = null;



            Texture GenericPlanetDeath = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").GetComponent<CharacterBody>().portraitIcon;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ExplosivePotDestructibleBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FusionCellDestructibleBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TimeCrystalBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AltarSkeletonBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath; //
            RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SulfurPodBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath; //

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
            UsedRustedKey.isConsumed = true;
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

            Texture2D texItemUsedPrayer = new Texture2D(256, 256, TextureFormat.DXT5, false);
            texItemUsedPrayer.LoadImage(Properties.Resources.texItemUsedPrayer, true);
            texItemUsedPrayer.filterMode = FilterMode.Bilinear;
            texItemUsedPrayer.wrapMode = TextureWrapMode.Clamp;
            Sprite texItemUsedPrayerS = Sprite.Create(texItemUsedPrayer, v.rec256, v.half);

            LanguageAPI.Add("ITEM_EXTRASTATSONLEVELUP_CONSUMED_NAME", "Prayer Beads Blessing", "en");
            LanguageAPI.Add("ITEM_EXTRASTATSONLEVELUP_CONSUMED_PICKUP", "Your prayer beads have blessed your stats.", "en");
            LanguageAPI.Add("ITEM_EXTRASTATSONLEVELUP_CONSUMED_DESC", "Your prayer beads have blessed your stats. (Missing Description)", "en");
            UsedPeadsOverlay_Pickup = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_CONSUMED_PICKUP", "Your prayer beads have blessed you with +0.0 levels", "en");
            UsedPeadsOverlay_Desc = LanguageAPI.AddOverlay("ITEM_EXTRASTATSONLEVELUP_CONSUMED_DESC", "Your prayer beads have blessed you with +0.00 levels, increasing <style=cIsHealing>max health</style> by <style=cIsHealing>+00.00</style>, <style=cIsHealing>health regeneration</style> by <style=cIsHealing>+00.00 hp/s</style>, and <style=cIsDamage>damage</style> by <style=cIsDamage>+00.00</style>.", "en");


            UsedPrayerBeads.name = "ExtraStatsOnLevelUpConsumed";
            UsedPrayerBeads.deprecatedTier = ItemTier.NoTier;
            UsedPrayerBeads.pickupModelPrefab = PrayerBeads.pickupModelPrefab;
            UsedPrayerBeads.pickupIconSprite = texItemUsedPrayerS;
            UsedPrayerBeads.nameToken = "ITEM_EXTRASTATSONLEVELUP_CONSUMED_NAME";
            UsedPrayerBeads.pickupToken = "ITEM_EXTRASTATSONLEVELUP_CONSUMED_PICKUP";
            UsedPrayerBeads.descriptionToken = "ITEM_EXTRASTATSONLEVELUP_CONSUMED_DESC";
            UsedPrayerBeads.loreToken = "";
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
