﻿using BepInEx;
using R2API.Utils;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("Early.Wolfo.WolfFixes", "WolfoBugFixes", "1.1.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WolfoMain : BaseUnityPlugin
    {
        public void Awake()
        {
            WConfig.Start();
        }

        public void Start()
        {
            if (WConfig.cfgLoadOrder.Value)
            {
                TestLoadOrder.Logger = base.Logger;
            }
            Testing.Start();
            AchievementManager.availability.CallWhenAvailable(onLoadFinished);
            if (WConfig.cfgDisable.Value)
            {
                //So far nothing is needed for Mod Compat even if other mods really want certain features.
                return;
            }
            Gameplay.Start();
            Visuals.Start();
            Audio.Start();

            BodyFixes.Start();
            Simualcrum.Start();

            Devotion.Start();
            ItemsAndEquipment.Start();
            Glass.Start();

            OptionPickupStuff.Start();
            ShrineShaping.Start();
            ShrineHalcyon.Start();
            PrayerBeads.Start();
            RandomFixes2.Start();

            ItemTags.Start();

            VoidSuppressor.Start();

            GameModeCatalog.availability.CallWhenAvailable(ModSupport_CallLate);

            
        }

        void onLoadFinished()
        {
            RoR2Application.onLoadFinished = (Action)Delegate.Remove(RoR2Application.onLoadFinished, new Action(onLoadFinished));
            bool riskOfOptions = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
            if (riskOfOptions)
            {
                WConfig.RiskConfig();
            }
        }

        internal static void ModSupport_CallLate()
        {
            if (WConfig.cfgDisable.Value)
            {
                //So far nothing is needed for Mod Compat even if other mods really want certain features.
                return;
            }
            TextFixes.Start();

            RoR2Content.Buffs.HiddenInvincibility.canStack = false;
            DLC2Content.Buffs.Boosted.canStack = false;

            RoR2.Stats.StatDef.highestLunarPurchases.displayToken = "STATNAME_HIGHESTLUNARPURCHASES";
            RoR2.Stats.StatDef.highestBloodPurchases.displayToken = "STATNAME_HIGHESTBLOODPURCHASES";

            //Prevent scrapping regen scrap.
            HG.ArrayUtils.ArrayAppend(ref DLC1Content.Items.RegeneratingScrap.tags, ItemTag.Scrap);

            MissingEliteDisplays.Start();
            ItemTags.ItemTagChanges();



            Addressables.LoadAssetAsync<GameObject>(key: "3a44327eee358a74ba0580dbca78897e").WaitForCompletion().AddComponent<GivePickupsOnStart>().itemDefInfos = new GivePickupsOnStart.ItemDefInfo[]
            {
                new GivePickupsOnStart.ItemDefInfo
                {
                    itemDef = RoR2Content.Items.UseAmbientLevel,
                    count = 1,
                }
            };

        }



    }

}