using BepInEx;
using R2API.Utils;
using RiskOfOptions;
using RoR2;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WolfoQoL_Server
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("Wolfo.WolfoQoL_Client")]
    [BepInPlugin("Wolfo.WolfoQoL_Server", "WolfoQoL_Extras", "4.1.0")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class WolfoMain : BaseUnityPlugin
    {
        
        public void Start()
        {
            WConfig.Start();

            BuffTimers.Buffs_NewBuffs();
            ConsumedItems.Start();
 
            GameModeCatalog.availability.CallWhenAvailable(ModSupport);
        }





        internal static void ModSupport()
        {
            BuffTimers.ModSupport();
        }




 


    }

}