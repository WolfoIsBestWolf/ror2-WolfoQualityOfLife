using BepInEx;
using BepInEx.Logging;
using RoR2;
using R2API.Utils;
using System.Security;
using System.Security.Permissions;
using WQoL_Gameplay;


#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WQoL_Gameplay
{
    [BepInPlugin("Wolfo.WolfoQoL_Gameplay", "WolfoQoL_Gameplay", "1.1.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WGQoLMain : BaseUnityPlugin
    {
        public static ManualLogSource log;


        public void Awake()
        {
            log = Logger;

            WConfig.Start();
           

            GameplayQualityOfLife.Start();

            PrismaticTrial.AllowPrismaticTrials();
            Eclipse.Start();

            GameModeCatalog.availability.CallWhenAvailable(WConfig.RiskConfig);
        }



    }


}