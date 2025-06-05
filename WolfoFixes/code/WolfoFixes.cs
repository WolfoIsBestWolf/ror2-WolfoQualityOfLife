using BepInEx;
using R2API.Utils;
using RoR2;
using System;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using WolfoFixes;
using WolfoQoL_Client;


#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WolfoFixes
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("Wolfo.WolfoFixes", "WolfoBugFixes", "1.0.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WolfoMain : BaseUnityPlugin
    {
 
        public void Start()
        {
            WConfig.Start();
            Visuals.Start();
            Audio.Start();
            TextFixes.Start();
            BodyFixes.Start();
            Simualcrum.Start();
            Gameplay.Start();

            OptionPickupStuff.Start();
            ShrineShaping.Start();
            ShrineHalcyon.Start();
            PrayerBeads.Start();
            RandomFixes2.Start();

            GameModeCatalog.availability.CallWhenAvailable(ModSupport_CallLate);
        }
 

        internal static void ModSupport_CallLate()
        {
 
 
            RoR2Content.Buffs.HiddenInvincibility.canStack = false;
            DLC2Content.Buffs.Boosted.canStack = false;

            RoR2.Stats.StatDef.highestLunarPurchases.displayToken = "STATNAME_HIGHESTLUNARPURCHASES";
            RoR2.Stats.StatDef.highestBloodPurchases.displayToken = "STATNAME_HIGHESTBLOODPURCHASES";
 
            //Prevent scrapping regen scrap.
            HG.ArrayUtils.ArrayAppend(ref DLC1Content.Items.RegeneratingScrap.tags, ItemTag.Scrap);


        }
  
          

    }
 
}