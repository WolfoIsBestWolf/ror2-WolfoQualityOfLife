using BepInEx;
using R2API.Utils;
using RoR2;
using System;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;


#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WolfoQoL_Client
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("Wolfo.WolfoQoL_Client", "WolfoQualityOfLife", "4.0.0")]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    public class WolfoMain : BaseUnityPlugin
    {
        public static readonly System.Random random = new System.Random();
        //public static readonly UnityEngine.Random random = new(); //???

        public static bool ServerModInstalled = false;

        public static bool ShouldHostGiveInfo
        {
            get
            {
                if (NetworkServer.active)
                {
                    return WConfig.cfgTestDisableHostInfo.Value == false;
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



        public void Awake()
        {
            WConfig.Start();
            Assets.Init(Info);
            WConfig.RiskConfig();
            IL.RoR2.Networking.ServerAuthManager.HandleSetClientAuth += Testing.ServerAuthManager_HandleSetClientAuth;
            On.RoR2.Run.GetUserMaster += Testing.Run_GetUserMaster;
            if (WConfig.cfgTestDisableMod.Value || WConfig.cfgTestDisableMod2.Value)
            {
                Debug.LogWarning("Disabled Mod for Test");
                return;
            }

            GameplayQualityOfLife.Start();

         
            //Generally Host send -> Host recieve always works fine for these messages
            //Just obviously a lot of this expects Host given info and will not work on Clients alone
            //So work arounds can be added
            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(ItemLoss_Host.ItemLossMessage), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(ItemLoss_Host.ItemLossMessage));

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(DeathMessage.DetailedDeathMessage), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(DeathMessage.DetailedDeathMessage));

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(KillerInventory.KillerInventoryMessage), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(KillerInventory.KillerInventoryMessage));

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(HostPingAllClients), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(HostPingAllClients));



            SkinChanges.Start();
            SkinTouchups.Start();
            //Misc
            RandomMisc.Start();
            DuplicatorModel.DuplicatorModelChanger();

            //Text
            MoreMessages.Start();
            Reminders.Start();


            //UI
            if (WConfig.cfgBuff_RepeatColors.Value == true)
            {
                RandomMiscWithConfig.BuffColorChanger();
            }

            DeathScreen_Expanded.Start();
            BodyIcons.Start();
            ColorModule.Main();
            UIBorders.Start();

            if (WConfig.cfgPingIcons.Value)
            {
                PingIcons.Start();
            }

            GameModeCatalog.availability.CallWhenAvailable(ModSupport_CallLate);



            On.RoR2.UI.LogBook.LogBookController.BuildMonsterEntries += RandomMiscWithConfig.LogbookEntryAdderMonsters;
            On.RoR2.UI.LogBook.LogBookController.BuildPickupEntries += RandomMiscWithConfig.LogbookEliteAspectAdder;
            On.RoR2.UI.MainMenu.MainMenuController.Start += OneTimeOnlyLateRunner;


            //Main Menu Stuff
            On.RoR2.UI.MainMenu.MainMenuController.Start += MainMenuExtras;


            MissionPointers.Start();


            //Debug.Log(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect"));
            //Debug.Log(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/fxHealAndReviveGold"));
            Debug.Log(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/HealAndRevive/fxHealAndReviveGold.prefab").WaitForCompletion());
            Debug.Log(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/HealAndRevive/fxHealAndReviveGreen.prefab").WaitForCompletion());


            Run.onRunStartGlobal += Run_onRunStartGlobal;

            ClientChecks.Start();

            On.RoR2.SceneDirector.Start += AddReminders;

            On.RoR2.Stage.PreStartClient += PingVisualStageChanges;

            DifficultyColors();
        }

        private void DifficultyColors()
        {
            Color eclipseColor = new Color(0.2f, 0.22f, 0.4f, 1);

            DifficultyCatalog.difficultyDefs[3].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[4].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[5].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[6].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[7].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[8].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[9].color = eclipseColor;
            DifficultyCatalog.difficultyDefs[10].color = eclipseColor;

            DifficultyCatalog.difficultyDefs[3].serverTag = "e1";
            DifficultyCatalog.difficultyDefs[4].serverTag = "e2";
            DifficultyCatalog.difficultyDefs[5].serverTag = "e3";
            DifficultyCatalog.difficultyDefs[6].serverTag = "e4";
            DifficultyCatalog.difficultyDefs[7].serverTag = "e5";
            DifficultyCatalog.difficultyDefs[8].serverTag = "e6";
            DifficultyCatalog.difficultyDefs[9].serverTag = "e7";
            DifficultyCatalog.difficultyDefs[10].serverTag = "e8";
        }

        private void PingVisualStageChanges(On.RoR2.Stage.orig_PreStartClient orig, Stage self)
        {
            orig(self);

            switch (SceneInfo.instance.sceneDef.baseSceneName)
            {
                case "lakes":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject ChestScrapper = GameObject.Find("/HOLDER: Preplaced Interactables/Chest2/");
                        if (ChestScrapper)
                        {
                            ChestScrapper.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ChestLargeIcon;
                        }
                        ChestScrapper = GameObject.Find("/HOLDER: Preplaced Interactables/Scrapper/");
                        if (ChestScrapper)
                        {
                            ChestScrapper.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ScrapperIcon;
                        }
                    }
                    break;
                case "villagenight":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject LargeChest = GameObject.Find("/TOGGLE: HouseRoomA/Chest2 (2)/");
                        if (LargeChest)
                        {
                            LargeChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ChestLargeIcon;
                        }
                        LargeChest = GameObject.Find("/TOGGLE: HouseRoomB/Chest2 (1)/");
                        if (LargeChest)
                        {
                            LargeChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ChestLargeIcon;
                        }
                        LargeChest = GameObject.Find("/TOGGLE: HouseRoomC/Chest2/");
                        if (LargeChest)
                        {
                            LargeChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ChestLargeIcon;
                        }
                    }
                    break;
                case "goolake":
                    GameObject DoorIsOpenedEffect = GameObject.Find("/HOLDER: Secret Ring Area Content/Entrance/GLRuinGate/DoorIsOpenedEffect/");
                    GameObject RingEventController = GameObject.Find("/HOLDER: Secret Ring Area Content/ApproxCenter/RingEventController/");
                    GenericObjectiveProvider objective = DoorIsOpenedEffect.AddComponent<GenericObjectiveProvider>();
                    objective.objectiveToken = "OBJECTIVE_DESERT_ELDERS";
                    var Event = RingEventController.GetComponent<OnPlayerEnterEvent>();
                    UnityEngine.Events.PersistentCall newCall = new UnityEngine.Events.PersistentCall
                    {
                        m_Target = objective,
                        m_MethodName = "Destroy",
                        m_Mode = UnityEngine.Events.PersistentListenerMode.Object,
                        m_Arguments = new UnityEngine.Events.ArgumentCache
                        {
                            m_ObjectArgument = objective
                        }
                    };
                    Event.action.m_PersistentCalls.AddListener(newCall);

                    break;
                case "foggyswamp":
                    GameObjectUnlockableFilter[] dummylist = FindObjectsOfType(typeof(RoR2.GameObjectUnlockableFilter)) as RoR2.GameObjectUnlockableFilter[];
                    for (var i = 0; i < dummylist.Length; i++)
                    {
                        Destroy(dummylist[i]);
                    }
                    break;
                case "frozenwall":
                case "itfrozenwall":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject LargeChest = GameObject.Find("/HOLDER: Timed Chests");
                        GameObject Fan = GameObject.Find("/PERMUTATION: Human Fan");
                        var purchases = LargeChest.GetComponentsInChildren<GenericDisplayNameProvider>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.TimedChestIcon;
                        }
                        purchases = Fan.GetComponentsInChildren<GenericDisplayNameProvider>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                        }
                    }
                    break;
                case "sulfurpools":
                    if (WConfig.cfgNewGeysers.Value)
                    {
                        GameObject GeyserHolder = GameObject.Find("/HOLDER: Geysers");
                        Material MatGeyserSulfurPools = Instantiate(GeyserHolder.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material);
                        MatGeyserSulfurPools.SetColor("_EmissionColor", new Color(0.15f, 0.2f, 0.08f));

                        for (int i = 0; i < GeyserHolder.transform.childCount; i++)
                        {
                            Transform LoopParticles = GeyserHolder.transform.GetChild(i).GetChild(2).GetChild(0);
                            LoopParticles.GetChild(1).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                            LoopParticles.GetChild(2).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                            LoopParticles.GetChild(3).GetComponent<ParticleSystemRenderer>().material = MatGeyserSulfurPools;
                        }
                    }
                    break;
                case "dampcavesimple":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject LegendaryChest = GameObject.Find("/HOLDER: Newt Statues and Preplaced Chests/GoldChest/");
                        LegendaryChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;

                        GameObject REX = GameObject.Find("/TreebotUnlockInteractable");
                        REX.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                        GameObject BrokenRexHoloPivot = new GameObject("BrokenRexHoloPivot");
                        BrokenRexHoloPivot.transform.localPosition = new Vector3(0.8f, 4f, -0.2f);
                        BrokenRexHoloPivot.transform.SetParent(REX.gameObject.transform, false);
                        REX.gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = BrokenRexHoloPivot.transform;
                    }
                    if (WConfig.cfgNewGeysers.Value)
                    {
                        //Geyser
                        GameObject GeyserHolderDamp = GameObject.Find("/HOLDER: Geyser");
                        Material MatLavaGeyser = Instantiate(GeyserHolderDamp.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material);
                        Material MatLavaGeyser2 = Instantiate(MatLavaGeyser);
                        MatLavaGeyser.SetColor("_EmissionColor", new Color(2f, 0.5f, 0f));
                        MatLavaGeyser2.SetColor("_EmissionColor", new Color(1.8f, 0.4f, 0f));

                        for (int i = 0; i < GeyserHolderDamp.transform.childCount; i++)
                        {
                            Transform LoopParticles = GeyserHolderDamp.transform.GetChild(i).GetChild(2).GetChild(0);
                            LoopParticles.GetChild(1).GetComponent<ParticleSystemRenderer>().material = MatLavaGeyser;
                            LoopParticles.GetChild(2).GetComponent<ParticleSystemRenderer>().material = MatLavaGeyser2;
                            LoopParticles.GetChild(3).GetComponent<ParticleSystemRenderer>().material = MatLavaGeyser2;
                        }
                    }
                    break;
                case "itdampcave":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject LegendaryChest = GameObject.Find("/HOLDER: Newt Statues and Preplaced Chests/GoldChest/");
                        LegendaryChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                    }
                    break;
                case "rootjungle":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        LODGroup[] lodlist = GameObject.Find("HOLDER: Randomization/GROUP: Large Treasure Chests").GetComponentsInChildren<LODGroup>();
                        for (var i = 0; i < lodlist.Length; i++)
                        {
                            //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                            if (lodlist[i].name.StartsWith("RJpinkshroom"))
                            {
                                Destroy(lodlist[i]);
                            }
                        }
                        GameObject LegendaryChest = GameObject.Find("/HOLDER: Randomization/GROUP: Large Treasure Chests/CHOICE: Root Bridge Front Chest/GoldChest/");
                        if (LegendaryChest)
                        {
                            LegendaryChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                        }
                        LegendaryChest = GameObject.Find("/HOLDER: Randomization/GROUP: Large Treasure Chests/CHOICE: Mushroom Cave Chest/GoldChest/");
                        if (LegendaryChest)
                        {
                            LegendaryChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                        }
                        LegendaryChest = GameObject.Find("/HOLDER: Randomization/GROUP: Large Treasure Chests/CHOICE: Treehouse Hole/GoldChest/");
                        if (LegendaryChest)
                        {
                            LegendaryChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                        }
                        LegendaryChest = GameObject.Find("/HOLDER: Randomization/GROUP: Large Treasure Chests/CHOICE: Triangle Cave/GoldChest/");
                        if (LegendaryChest)
                        {
                            LegendaryChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                        }
                        LegendaryChest = GameObject.Find("/HOLDER: Randomization/GROUP: Large Treasure Chests/CHOICE: Downed Tree Roots/GoldChest/");
                        if (LegendaryChest)
                        {
                            LegendaryChest.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                        }
                    }
                    break;
                case "skymeadow":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject.Find("/PortalDialerEvent/Final Zone/ButtonContainer/PortalDialer").AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                    }
                    GameObject.Find("/HOLDER: Zones/OOB Zone").GetComponent<MapZone>().zoneType = MapZone.ZoneType.OutOfBounds;

                    Material CorrectGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser (2)/mdlGeyser").GetComponent<MeshRenderer>().material;
                    Transform WrongGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser").transform;
                    WrongGeyser.GetChild(0).GetComponent<MeshRenderer>().material = CorrectGeyser;
                    WrongGeyser.GetChild(1).GetComponent<MeshRenderer>().material = CorrectGeyser;
                    break;
                case "helminthroost":
                    if (WConfig.cfgHelminthFix.Value)
                    {
                        GameObject lighting = GameObject.Find("/HOLDER: Lighting/Weather, Helminthroost/PP + Amb");
                        var PP = lighting.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessVolume>();
                        PP.priority++;
                    }
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject.Find("/PortalDialerEvent/Final Zone/ButtonContainer/PortalDialer").AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                    }
                    break;
                case "moon2":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject LunarChest_Holder = GameObject.Find("/HOLDER: Gameplay Space/HOLDER: STATIC MESH/Quadrant 3: Greenhouse/Q3_OuterRing/Bud Holder/");
                        PurchaseInteraction[] purchases = LunarChest_Holder.GetComponentsInChildren<PurchaseInteraction>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ChestLunarIcon;
                        }

                        GameObject Cauldron_Holder = GameObject.Find("/HOLDER: Gameplay Space/HOLDER: STATIC MESH/Quadrant 2: Workshop/Q2_OuterRing/Island Q2: WorkshopInteriors/Cauldrons/");
                        purchases = Cauldron_Holder.GetComponentsInChildren<PurchaseInteraction>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.CauldronIcon;
                        }

                        GameObject Pillars = GameObject.Find("/HOLDER: Pillars");
                        purchases = Pillars.GetComponentsInChildren<PurchaseInteraction>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.CubeIcon;
                        }

                        //GameObject Elevators = GameObject.Find("/HOLDER: Elevators");
                        //Elevators.GetComponentsInChildren<GenericInteraction>();

                        GameObject ShrineOrder = GameObject.Find("/HOLDER: Gameplay Space/HOLDER: STATIC MESH/Quadrant 1: Quarry/Q1_OuterRing/ShrineRestack/");
                        ShrineOrder.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ShrineOrderIcon;

                    }
                    //Maybe this o
                    GameObject Phase2 = GameObject.Find("/SceneInfo/BrotherMissionController/BrotherEncounter, Phase 2");
                    if (Phase2)
                    {
                        Phase2.GetComponent<BossGroup>().bestObservedName = Language.GetString("LUNAR_CHIMERA");
                        Phase2.GetComponent<BossGroup>().bestObservedSubtitle = "<sprite name=\"CloudLeft\" tint=1> " + Language.GetString("LUNARGOLEM_BODY_SUBTITLE") + " <sprite name=\"CloudRight\" tint=1>";
                    }

                    break;
                case "bazaar":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject LunarShop = GameObject.Find("/HOLDER: Store/LunarShop/");
                        GameObject SeerShop = GameObject.Find("/HOLDER: Store/SeerShop/");
                        GameObject CauldronShop = GameObject.Find("/HOLDER: Store/CauldronShop/");

                        var purchases = LunarShop.GetComponentsInChildren<PurchaseInteraction>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            if (purchases[i].name.StartsWith("LunarShopTerminal"))
                            {
                                purchases[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ChestLunarIcon;
                            }
                            else if (purchases[i].name.EndsWith("Recycler"))
                            {
                                GameObject LunarRecyclerPivot = new GameObject("LunarRecyclerPivot");
                                LunarRecyclerPivot.transform.localPosition = new Vector3(0.1f, -1f, 1f);
                                LunarRecyclerPivot.transform.localRotation = new Quaternion(0f, -0.7071f, -0.7071f, 0f);
                                LunarRecyclerPivot.transform.SetParent(purchases[i].gameObject.transform, false);
                                purchases[i].gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LunarRecyclerPivot.transform;
                                purchases[i].gameObject.GetComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
                            }
                        }
                        purchases = SeerShop.GetComponentsInChildren<PurchaseInteraction>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.SeerIcon;
                        }
                        purchases = CauldronShop.GetComponentsInChildren<PurchaseInteraction>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.CauldronIcon;
                        }

                        GameObject PortalShop = GameObject.Find("/PortalShop/");
                        PortalShop.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.PortalIcon;
                        GameObject PortalArena = GameObject.Find("/HOLDER: Starting Cave/HOLDER: Arena Entrance/Trigger/PortalArena/");
                        PortalArena.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.PortalIcon;
                        PortalArena.transform.parent.GetChild(2).gameObject.GetComponent<SphereCollider>().radius = 130;
                    }
                    break;
                case "meridian":
                    GameObject PortalDestination = GameObject.Find("/HOLDER: Design/FSB/Boss Defeated Objects/");
                    if (PortalDestination)
                    {
                        PortalDestination = PortalDestination.transform.GetChild(2).gameObject;
                        PortalDestination.GetComponent<GenericInteraction>().contextToken = "PORTAL_DESTINATION_CONTEXT";

                    }
                    if (WConfig.cfgPingIcons.Value)
                    {
                        if (PortalDestination)
                        {
                            PortalDestination.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.PortalIcon;
                        }
                        //Not changing this one
                        Highlight[] highlightlist = FindObjectsOfType(typeof(Highlight)) as Highlight[];
                        for (var i = 0; i < highlightlist.Length; i++)
                        {
                            if (highlightlist[i].name.StartsWith("LunarCauldron,"))
                            {
                                highlightlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.CauldronIcon;
                            }
                            else if (highlightlist[i].name.StartsWith("Chest2"))
                            {
                                highlightlist[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ChestLargeIcon;
                            }
                            else if (highlightlist[i].name.StartsWith("Geode"))
                            {
                                highlightlist[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                            }
                        }
                        GameObject ShrineRebirth = GameObject.Find("/HOLDER: Design/FSB/Boss Defeated Objects/Shrine of Rebirth/ShrineRebirth/");
                        ShrineRebirth.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                    }
                    break;
                case "goldshores":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject Chests = GameObject.Find("/HOLDER: Preplaced Goodies");
                        var purchases = Chests.GetComponentsInChildren<PurchaseInteraction>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.ChestIcon;
                        }
                    }
                    break;
                case "arena":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject PortalArena = GameObject.Find("/PortalArena");
                        PortalArena.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.PortalIcon;
                        //PortalArena.AddComponent<GenericObjectiveProvider>().objectiveToken = "Exit the <style=cIsVoid>Void Fields</style>";

                        GameObject Chests = GameObject.Find("/ArenaMissionController");
                        var purchases = Chests.GetComponentsInChildren<PurchaseInteraction>(false);
                        for (int i = 0; i < purchases.Length; i++)
                        {
                            purchases[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.NullVentIcon;
                        }
                    }
                    break;
                case "mysteryspace":
                    GameObject MSObelisk = GameObject.Find("/MS_Obelisk");
                    MSObelisk.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.QuestionMarkIcon;
                    MSObelisk.AddComponent<GenericObjectiveProvider>().objectiveToken = "OBJECTIVE_GLASS_YOURSELF";
                    break;
                case "artifactworld":
                case "artifactworld01":
                case "artifactworld02":
                case "artifactworld03":
                    GameObject ArtifactDisplay = GameObject.Find("/ArtifactDisplay/SetpiecePickup");
                    if (ArtifactDisplay == null)
                    {
                        ArtifactDisplay = GameObject.Find("/HOLDER: Design/ArtifactDisplay/SetpiecePickup");
                    }
                    GenericPickupController pickup = ArtifactDisplay.GetComponent<GenericPickupController>();
                    string artifactname = "ArtifactIndex.None";
                    if (pickup.pickupIndex == PickupIndex.none)
                    {
                        pickup.pickupIndex = PickupCatalog.FindPickupIndex(RoR2Content.Items.BoostHp.itemIndex);
                    }
                    else if (pickup.pickupIndex != PickupIndex.none)
                    {
                        ArtifactDef tempartifactdef = ArtifactCatalog.GetArtifactDef(pickup.pickupIndex.pickupDef.artifactIndex);
                        if (tempartifactdef)
                        {
                            artifactname = Language.GetString(tempartifactdef.nameToken);
                            String[] spltis = artifactname.Split(" ");
                            if (spltis.Length > 0)
                            {
                                artifactname = spltis[spltis.Length - 1];
                                Debug.Log(artifactname);
                            }
                        }
                        if (WConfig.ArtifactOutline.Value == true)
                        {
                            Highlight tempartifact = pickup.gameObject.GetComponent<Highlight>();
                            tempartifact.pickupIndex = pickup.pickupIndex;
                            tempartifact.highlightColor = Highlight.HighlightColor.pickup;
                            tempartifact.isOn = true;
                        }
                    }
                    pickup.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = string.Format(Language.GetString("OBJECTIVE_ARTIFACT_TRIAL"), artifactname);

                    break;
                case "voidstage":
                    GameObject MissionController = GameObject.Find("/MissionController");
                    MissionController.GetComponent<VoidStageMissionController>().deepVoidPortalObjectiveProvider = null;
                    break;
                case "voidraid":
                    Debug.Log(GameObject.Find("/CenterSwirl"));
                    Debug.Log(GameObject.Find("/BrightSwirlPP"));
                    break;
            };

            GC.Collect();
        }

        public void Start()
        {
            ServerModInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Wolfo.WolfoQoL_Client");
            Debug.Log("WolfoQualityOfLife : WolfoQoL_Client installed? : " + ServerModInstalled);

            if (WConfig.cfgTestDisableMod.Value)
            {
                WConfig.cfgTestDisableMod.Value = false;
                return;
            }
            TextChanges.Main();
        }

        private void Run_onRunStartGlobal(Run obj)
        {
            HostHasMod = false;
            if (NetworkServer.active)
            {
                Chat.SendBroadcastChat(new HostPingAllClients());
            }
        }






        private void MainMenuExtras(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);
            if (WConfig.cfgMainMenuScav.Value)
            {
                GameObject tempmain = GameObject.Find("/HOLDER: Title Background");
                GameObject ScavHolder = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU3 Props.prefab").WaitForCompletion(), tempmain.transform);

                Destroy(ScavHolder.transform.GetChild(3).gameObject);
                Destroy(ScavHolder.transform.GetChild(1).gameObject);
                Destroy(ScavHolder.transform.GetChild(0).gameObject);
                ScavHolder.SetActive(true);

                GameObject mdlGup = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/Gup/GupBody.prefab").WaitForCompletion().transform.GetChild(0).GetChild(0).gameObject);
                mdlGup.GetComponent<Animator>().Play("Spawn1", 0, 1);
                mdlGup.transform.localScale = new Vector3(7, 7, 7);
                mdlGup.transform.localPosition = new Vector3(55.2201f, -0.9796f, 11.5f);
                mdlGup.transform.localEulerAngles = new Vector3(5.7003f, 247.4198f, 0);

                Transform extragamemodepos = self.extraGameModeMenuScreen.gameObject.transform.parent.GetChild(1);
                extragamemodepos.localPosition = new Vector3(36.24f, 2.7204f, 3.1807f);
                extragamemodepos.localEulerAngles = new Vector3(5.2085f, 38.4904f, 0);
            }

            int selector = WConfig.cfgMainMenuRandomizerSelector.Value;
            if (selector != 0)
            {
                if (selector == 1 || selector == 11)
                {
                    GameObject CU2 = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU1 Props.prefab").WaitForCompletion());
                    CU2.SetActive(true);
                }
                else if (selector == 2 || selector == 12)
                {
                    GameObject CU2 = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU2 Props.prefab").WaitForCompletion());
                    CU2.SetActive(true);
                }
                else if (selector == 3 || selector == 13)
                {
                    GameObject CU2 = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU3 Props.prefab").WaitForCompletion());
                    CU2.SetActive(true);
                }
                else if (selector == 4 || selector == 14)
                {
                    GameObject CU2 = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU4 Props.prefab").WaitForCompletion());
                    CU2.SetActive(true);
                }
                if (selector > 9)
                {
                    GameObject StarStorm2 = GameObject.Find("/StormMainMenuEffect(Clone)");
                    if (StarStorm2)
                    {
                        StarStorm2.SetActive(false);
                    }
                }
            }
            else if (WConfig.cfgMainMenuRandomizer.Value)
            {
                GameObject StarStorm2 = GameObject.Find("/StormMainMenuEffect(Clone)");
                GameObject CU1 = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU1 Props.prefab").WaitForCompletion());
                GameObject CU2 = Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/title/CU2 Props.prefab").WaitForCompletion());

                int randomMenu = random.Next(6);
                switch (randomMenu)
                {
                    case 0:
                        //Default
                        break;
                    case 1:
                        CU1.SetActive(true); //Orange
                        break;
                    case 2:
                        CU2.SetActive(true); //Green
                        break;
                    case 3:
                        if (StarStorm2)
                        {
                            StarStorm2.SetActive(false);
                        }
                        break;
                    case 4:
                        if (StarStorm2)
                        {
                            StarStorm2.SetActive(false);
                        }
                        CU1.SetActive(true);
                        break;
                    case 5:
                        if (StarStorm2)
                        {
                            StarStorm2.SetActive(false);
                        }
                        CU2.SetActive(true);
                        break;
                }
            }
        }

        internal static void ModSupport_CallLate()
        {

            RoR2Content.Items.AdaptiveArmor.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;
            RoR2Content.Items.BoostEquipmentRecharge.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;
            JunkContent.Equipment.Enigma.pickupIconSprite = JunkContent.Items.AACannon.pickupIconSprite;

            if (WConfig.cfgColorMain.Value)
            {
                ColorModule_Sprites.ModSupport();
            }
            if (WConfig.cfgPingIcons.Value)
            {
                PingIcons.ModSupport();
            }
            MoreMessages.VannilaVoids_WatchBrokeItem = ItemCatalog.FindItemIndex("VV_ITEM_BROKEN_MESS");
            RoR2.Stats.StatDef.highestLunarPurchases.displayToken = "STATNAME_HIGHESTLUNARPURCHASES";
            RoR2.Stats.StatDef.highestBloodPurchases.displayToken = "STATNAME_HIGHESTBLOODPURCHASES";
 
            PrayerBeads.moffeine = ItemCatalog.FindItemIndex("MoffeinBeadStatItem");

            RoR2Content.Buffs.HiddenInvincibility.canStack = false;

            //Prevent scrapping regen scrap.
            HG.ArrayUtils.ArrayAppend(ref DLC1Content.Items.RegeneratingScrap.tags, ItemTag.Scrap);

            DLC1Content.Items.FragileDamageBonusConsumed.descriptionToken = DLC1Content.Items.FragileDamageBonusConsumed.pickupToken;
            DLC1Content.Items.HealingPotionConsumed.descriptionToken = DLC1Content.Items.HealingPotionConsumed.pickupToken;


            Debug.Log("All RequiredByAllMods :");
            foreach (var a in NetworkModCompatibilityHelper._networkModList)
            {
                Debug.Log(a);
            }

            LunarEliteDisplay.Start();
            TextChanges.UntieredItemTokens();

            LunarEliteDisplay.VoidDisplaysMithrix();

            if (WConfig.cfgColorMain.Value == true)
            {
                ColorModule.ChangeColorsPost();
            }
            ColorModule.AddMissingItemHighlights();
            RoR2Content.Items.MinHealthPercentage.hidden = false;
            var orang = ItemTierCatalog.FindTierDef("OrangeTierDef");
            if (orang != null)
            {
                OptionPickupStuff.orange = orang.tier;
            }
        }



        public void OneTimeOnlyLateRunner(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);
            On.RoR2.UI.MainMenu.MainMenuController.Start -= OneTimeOnlyLateRunner;
            GC.Collect();
        }





        //Probably should start searching paths for all of these 
        public void AddReminders(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            orig(self);
            if (WConfig.cfgRemindersGeneral.Value == true)
            {
                if (SceneInfo.instance.countsAsStage || SceneInfo.instance.sceneDef.allowItemsToSpawnObjects)
                {
                    TreasureReminder.SetupRemindersStatic();
                }
            }
            Debug.Log("WolfoQoL-" + SceneInfo.instance.sceneDef.baseSceneName);

        }


        public class HostPingAllClients : ChatMessageBase
        {
            public override string ConstructChatString()
            {
                HostHasMod = true;
                Debug.Log("Host has WolfoQoL_Client: " + HostHasMod_);
                return null;
            }
        }


        /*public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }*/



    }
    public class MakeThisMercRed : MonoBehaviour
    {
    }
    public class MakeThisMercGreen : MonoBehaviour
    {
    }
    public class MakeThisAcridBlight : MonoBehaviour
    {
        public void Start()
        {
            //How reliable would this be on Client?
            CrocoDamageTypeController controller = this.gameObject.GetComponent<CrocoDamageTypeController>();
            if (controller == null)
            {
                Destroy(this);
            }
            if (controller.GetDamageType() != DamageType.BlightOnHit)
            {
                Destroy(this);
            }
        }
    }

}