using BepInEx;
using BepInEx.Logging;
using R2API.Utils;
using RoR2;
using System;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using WolfoQoL_Client.DeathScreen;
using WolfoQoL_Client.Menu;
using WolfoQoL_Client.ProperSaveSupport;
using WolfoQoL_Client.Text;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WolfoQoL_Client
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.Wolfo.WolfoLibrary")]
    [BepInPlugin("Wolfo.WolfoQoL_Client", "WolfoQualityOfLife", "5.0.4")]
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
          
            ClientChecks.Start();
            MenuMain.Start();
            TextMain.Start();
            VisualsMain.Start();
            
            //Generally Host send -> Host recieve always works fine for these messages
            //Just obviously a lot of this expects Host given info and will not work on Clients alone
            //So work arounds can be added
             
            ChatMessageBase.chatMessageIndexToType.AddRange(new Type[160]);

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(ItemLossMessage), 160);
            ChatMessageBase.chatMessageIndexToType[160] = typeof(ItemLossMessage);

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(InteractableMessage), 161);
            ChatMessageBase.chatMessageIndexToType[161] = typeof(InteractableMessage);

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(DroneChatMessage), 162);
            ChatMessageBase.chatMessageIndexToType[162] = typeof(DroneChatMessage);

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(DeathMessage.DetailedDeathMessage), 163);
            ChatMessageBase.chatMessageIndexToType[163] = typeof(DeathMessage.DetailedDeathMessage);

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(KillerInventoryMessage), 164);
            ChatMessageBase.chatMessageIndexToType[164] = typeof(KillerInventoryMessage);


            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(HostPingAllClients), 166);
            ChatMessageBase.chatMessageIndexToType[166] = typeof(HostPingAllClients);

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(PerPlayer_ExtraStatTracker.SyncValues), 167);
            ChatMessageBase.chatMessageIndexToType[167] = typeof(PerPlayer_ExtraStatTracker.SyncValues);

 
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;

            BodyCatalog.availability.CallWhenAvailable(WStats.MakeStats);
             
        }

        public void Start()
        {
            ServerModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Wolfo.WolfoQoL_Server");
            ProperSaveInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KingEnderBrine.ProperSave");
            WQoLMain.log.LogMessage("WolfoQoL_Extras installed? : " + ServerModInstalled);
            WQoLMain.log.LogMessage("ProperSaveInstalled installed? : " + ProperSaveInstalled);

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
                Chat.SendBroadcastChat(new HostPingAllClients());
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