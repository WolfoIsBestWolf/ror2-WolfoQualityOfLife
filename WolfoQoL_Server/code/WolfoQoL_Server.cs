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
    [BepInPlugin("Wolfo.WolfoQoL_Server", "WolfoQoL_Extras", "4.0.0")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    public class WolfoMain : BaseUnityPlugin
    {
        static readonly System.Random random = new System.Random();
        public static bool ClientModInstalled = false;


        public void Awake()
        {
            //Assets.Init(Info);
            WConfig.Start();
            ClientModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Wolfo.WolfoQoL_Client");
            if (!ClientModInstalled)
            {
                Debug.LogWarning("WolfoQoL_Server without WolfoQoL_Client");
                return;
            }
            Texture2D modIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/icon.png");
            ModSettingsManager.SetModIcon(Sprite.Create(modIcon, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f)));

            BuffTimers.Buffs_NewBuffs();
            ConsumedItems.Start();
      

            GameModeCatalog.availability.CallWhenAvailable(ModSupport);

            On.RoR2.UI.MainMenu.MainMenuController.Start += OneTimeOnlyLateRunner;
        }





        internal static void ModSupport()
        {
            BuffTimers.ModSupport();
        }



      

        public void OneTimeOnlyLateRunner(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);

            BuffTimers.GetDotDef();
            On.RoR2.UI.MainMenu.MainMenuController.Start -= OneTimeOnlyLateRunner;
        }



    }

}