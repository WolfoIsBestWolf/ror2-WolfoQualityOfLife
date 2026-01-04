using BepInEx;
using BepInEx.Logging;
using HG.Reflection;
using R2API.Utils;
using RoR2;
using System;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using WolfoLibrary;
using WolfoQoL_Client.DeathScreen;
using WolfoQoL_Client.Menu;
using WolfoQoL_Client.ModSupport;
using WolfoQoL_Client.Text;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]
[assembly: SearchableAttribute.OptIn]
namespace WolfoQoL_Client
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.Wolfo.WolfoLibrary", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("Wolfo.WolfoQoL_Client", "WolfoQualityOfLife", "5.1.2")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WQoLMain : BaseUnityPlugin
    {

        public static readonly System.Random random = new System.Random();

        public static bool ServerModInstalled = false;
        public static bool ProperSaveInstalled = false;

        public static bool NoHostInfo
        {
            get
            {
                if (!NetworkServer.active)
                {
                    return WConfig.cfgTestDisableHostInfo.Value;
                }
                return false;
            }
        }
        public static bool HostHasMod_ = false;
        public static bool HostHasMod
        {
            get
            {
                if (WConfig.cfgTestClient.Value)
                {
                    return false;
                }
                return HostHasMod_;
            }
            set
            {
                HostHasMod_ = value;
            }
        }
        public static ManualLogSource log;


        public void Awake()
        {
            log = base.Logger;

            WConfig.Start();
            Assets.Init(Info);
            WConfig.RiskConfig();

            //TMP Default Style Sheet
            TMPro.TMP_StyleSheet TMPStyles = Addressables.LoadAssetAsync<TMPro.TMP_StyleSheet>(key: "54d1085f9a2fdea4587fcfc7dddcd4bc").WaitForCompletion();
            TMPStyles.styles.Add(new TMPro.TMP_Style("cGold", "<color=#FFE880>", "</color>"));
            TMPStyles.styles.Add(new TMPro.TMP_Style("cRebirth", "<color=#7CFE7C>", "</color>"));
            TMPStyles.styles.Add(new TMPro.TMP_Style("cMysterySpace", "<color=#89F1E8>", "</color>"));
            TMPStyles.styles.Add(new TMPro.TMP_Style("cAlly", "<color=#A3C3EF>", "</color>"));

            if (WConfig.cfgTestDisableMod.Value || WConfig.cfgTestDisableMod2.Value)
            {
                WQoLMain.log.LogWarning("Disabled Mod for Test");
                return;
            }

            //Generally Host send -> Host recieve always works fine for these messages
            //Just obviously a lot of this expects Host given info and will not work on Clients alone
            //So work arounds can be added

            byte wqolByte = 0;

            Networker.message_to_index.Add(typeof(ItemLossMessage), wqolByte);
            Networker.index_to_message.Add(wqolByte, typeof(ItemLossMessage));
            wqolByte++;
            Networker.message_to_index.Add(typeof(InteractableMessage), wqolByte);
            Networker.index_to_message.Add(wqolByte, typeof(InteractableMessage));
            wqolByte++;
            Networker.message_to_index.Add(typeof(DroneChatMessage), wqolByte);
            Networker.index_to_message.Add(wqolByte, typeof(DroneChatMessage));
            wqolByte++;
            Networker.message_to_index.Add(typeof(DeathMessage.DetailedDeathMessage), wqolByte);
            Networker.index_to_message.Add(wqolByte, typeof(DeathMessage.DetailedDeathMessage));
            wqolByte++;
            Networker.message_to_index.Add(typeof(KillerInventoryMessage), wqolByte);
            Networker.index_to_message.Add(wqolByte, typeof(KillerInventoryMessage));
            wqolByte++;
            Networker.message_to_index.Add(typeof(HostPingAllClients), wqolByte);
            Networker.index_to_message.Add(wqolByte, typeof(HostPingAllClients));
            wqolByte++;
            Networker.message_to_index.Add(typeof(PerPlayer_ExtraStatTracker.SyncValues), wqolByte);
            Networker.index_to_message.Add(wqolByte, typeof(PerPlayer_ExtraStatTracker.SyncValues));
            wqolByte++;
            Networker.message_to_index.Add(typeof(DevotionDeathMessage), wqolByte);
            Networker.index_to_message.Add(wqolByte, typeof(DevotionDeathMessage));

            ClientChecks.Start();
            MenuMain.Start();
            TextMain.Start();
            VisualsMain.Start();



            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;

            BodyCatalog.availability.CallWhenAvailable(WStats.MakeStats);

            PreGameController.cvSvAllowMultiplayerPause.defaultValue = "1";

            On.RoR2.UI.PauseScreenController.InstantiateMinimalPauseScreen += PauseScreenController_InstantiateMinimalPauseScreen;
        }

        private GameObject PauseScreenController_InstantiateMinimalPauseScreen(On.RoR2.UI.PauseScreenController.orig_InstantiateMinimalPauseScreen orig)
        {
            var temp = orig();
            temp.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

            return temp;
        }

        public void Start()
        {
            ServerModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Wolfo.WolfoQoL_Server");
            ProperSaveInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KingEnderBrine.ProperSave");
            QualitySupport.QualityModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.Gorakh.ItemQualities");


            WQoLMain.log.LogMessage("WolfoQoL_Extras installed? : " + ServerModInstalled);
            WQoLMain.log.LogMessage("ProperSaveInstalled installed? : " + ProperSaveInstalled);
            WQoLMain.log.LogMessage("Qualtiy installed? : " + QualitySupport.QualityModInstalled);

            if (ProperSaveInstalled)
            {
                AddProperSaveSupport.Start();
            }


        }

        private void Run_onRunStartGlobal(Run obj)
        {
            GC.Collect();
            if (NetworkServer.active)
            {
                Networker.SendWQoLMessage(new HostPingAllClients());
            }
        }
        private void Run_onRunDestroyGlobal(Run obj)
        {
            HostHasMod = false;
        }

        public class HostPingAllClients : ChatMessageBase
        {
            public override string ConstructChatString()
            {
                HostHasMod = true;
                if (!NetworkServer.active)
                {
                    WQoLMain.log.LogMessage("Host has WolfoQoL_Client: " + HostHasMod_);
                }

                //Hard to account for, when does PlayerMaster get made
                //When does this get sent, when does this arrive
                //Does just doing it here instead.
                if (WConfig.cfgTestClient.Value == true)
                {
                    return null;
                }
                foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
                {
                    Destroy(player.GetComponent<KillerInfo_ClientListener>());
                    Destroy(player.GetComponent<PlayerItemLoss_ClientListener>());
                }
                return null;
            }
        }


    }


}