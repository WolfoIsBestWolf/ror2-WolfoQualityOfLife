using BepInEx;
using BepInEx.Logging;
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
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("Wolfo.WolfoQoL_Gameplay", "SimpleGameplayQoL", "5.0.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WGQoLMain : BaseUnityPlugin
    {
        public static ManualLogSource log;


        public void Awake()
        {
            log = Logger;

            WConfig.Start();
            WConfig.RiskConfig();

            GameplayQualityOfLife.Start();

            PrismaticTrial.AllowPrismaticTrials();
            Eclipse.Start();
        }



    }


}