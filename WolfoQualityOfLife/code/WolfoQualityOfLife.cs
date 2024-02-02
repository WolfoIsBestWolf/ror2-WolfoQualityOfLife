using BepInEx;
using R2API.Utils;
using R2API;
using RoR2;
using System;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;

#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete
[module: UnverifiableCode]

namespace WolfoQualityOfLife
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Wolfo.WolfoQualityOfLife", "WolfoQualityOfLife", "2.5.3")]
    //[R2APISubmoduleDependency(nameof(ContentAddition), nameof(LoadoutAPI), nameof(PrefabAPI), nameof(LanguageAPI), nameof(ItemAPI))]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]


    public class WolfoMain : BaseUnityPlugin
    {
        static readonly System.Random random = new System.Random();

        //public static GameObject RedToWhiteSoup = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, RedToWhite Variant");
        //Surely you could just activate an already existing soup
 
        public void Awake()
        {
            WConfig.Start();

            SkinChanges.Start();

            //Misc
            RandomMisc.Start();
            DuplicatorModel.DuplicatorModelChanger();

            //Text
            MoreMessages.Start();
            Reminders.Start();
            TextChanges.Main();

            //UI
            BuffTimers.BuffColorChanger();
            ExpandedDeathScreen.Start();
            ExtraIcons.Start();
            ItemColorModule.Main();

            if (WConfig.cfgPingIcons.Value)
            {
                PingIcons.Start();
            }
            
            GameModeCatalog.availability.CallWhenAvailable(ModSupport);

            On.RoR2.SceneDirector.Start += PingIconChanger;

            On.RoR2.UI.LogBook.LogBookController.BuildStaticData += RandomMiscWithConfig.LogbookEntryAdder;
            On.RoR2.UI.MainMenu.MainMenuController.Start += OneTimeOnlyLateRunner;


            //
            //This is such a dogshit way to circumvent learning proper Networking but it's all the same I imagine
            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(MoreMessages.FakeSubjectMessage), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(MoreMessages.FakeSubjectMessage));
            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(ExpandedDeathScreen.SendGameEndInvHelper), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(ExpandedDeathScreen.SendGameEndInvHelper));
            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(RandomMiscWithConfig.SendExtraMountainIcon), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(RandomMiscWithConfig.SendExtraMountainIcon));
            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(Reminders.UpdateTreasureReminderCounts), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(Reminders.UpdateTreasureReminderCounts));

            ChatMessageBase.chatMessageTypeToIndex.Add(typeof(Reminders.DestroyPortalReminderClients), (byte)ChatMessageBase.chatMessageIndexToType.Count);
            ChatMessageBase.chatMessageIndexToType.Add(typeof(Reminders.DestroyPortalReminderClients));

            //Main Menu Stuff
            On.RoR2.UI.MainMenu.MainMenuController.Start += MainMenuExtras;


            Stupid.Start();
            if (WConfig.DummyModelViewer.Value == true)
            {
                //Stupid.ModelViewer();
                GameModeCatalog.availability.CallWhenAvailable(Stupid.ModelViewer);
            }
        }

        private void MainMenuExtras(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);
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


            if (WConfig.cfgMainMenuRandomizer.Value)
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

        internal static void ModSupport()
        {
            LunarEliteDisplay.AffixLunarItemDisplay();
            if (WConfig.EnableColorChangeModule.Value)
            {
                ItemColorModule.ModSupport();
            }
            if (WConfig.cfgPingIcons.Value)
            {
                PingIcons.ModSupport();
            }
            BuffTimers.ModSupport();

            if (WConfig.cfgSkinAcridBlight.Value == true)
            {
                BurnEffectController.blightEffect.fireEffectPrefab.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().sharedMaterial = AcridBlight.matCrocoGooLargeBlight;
            }
            RoR2.Stats.StatDef.highestLunarPurchases.displayToken = "STATNAME_HIGHESTLUNARPURCHASES";
            RoR2.Stats.StatDef.highestBloodPurchases.displayToken = "STATNAME_HIGHESTBLOODPURCHASES";

            //Eclipse coloring the tooltip because iirc it's Monsoon Red by default
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse1).SetFieldValue<Color>("color", new Color(0.4f, 0.4f, 0.5f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse2).SetFieldValue<Color>("color", new Color(0.371f, 0.371f, 0.471f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse3).SetFieldValue<Color>("color", new Color(0.342f, 0.342f, 0.442f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse4).SetFieldValue<Color>("color", new Color(0.314f, 0.314f, 0.414f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse5).SetFieldValue<Color>("color", new Color(0.285f, 0.285f, 0.385f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse6).SetFieldValue<Color>("color", new Color(0.257f, 0.257f, 0.357f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse7).SetFieldValue<Color>("color", new Color(0.228f, 0.228f, 0.328f, 1));
            DifficultyCatalog.GetDifficultyDef(DifficultyIndex.Eclipse8).SetFieldValue<Color>("color", new Color(0.2f, 0.2f, 0.3f, 1));

            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse1").tooltipNameColor = new Color(0.4f, 0.4f, 0.5f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse2").tooltipNameColor = new Color(0.371f, 0.371f, 0.471f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse3").tooltipNameColor = new Color(0.342f, 0.342f, 0.442f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse4").tooltipNameColor = new Color(0.314f, 0.314f, 0.414f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse5").tooltipNameColor = new Color(0.285f, 0.285f, 0.385f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse6").tooltipNameColor = new Color(0.257f, 0.257f, 0.357f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse7").tooltipNameColor = new Color(0.228f, 0.228f, 0.328f, 1);
            RuleCatalog.FindRuleDef("Difficulty").FindChoice("Eclipse8").tooltipNameColor = new Color(0.2f, 0.2f, 0.3f, 1);
        }



        public void OneTimeOnlyLateRunner(On.RoR2.UI.MainMenu.MainMenuController.orig_Start orig, RoR2.UI.MainMenu.MainMenuController self)
        {
            orig(self);

            BuffTimers.GetDotDef();
            LunarEliteDisplay.VoidDisplaysMithrix();
            PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex("EquipmentIndex.EliteLunarEquipment")).isLunar = true;
            if (WConfig.EnableColorChangeModule.Value == true)
            {
                ItemColorModule.ChangeColors();
            }
            ItemColorModule.AddMissingItemHighlights();
            RoR2Content.Items.MinHealthPercentage.hidden = false;

            if (WConfig.cfgNewSprintCrosshair.Value == true)
            {
                RandomMiscWithConfig.SprintUICallLate();
            }

            On.RoR2.UI.MainMenu.MainMenuController.Start -= OneTimeOnlyLateRunner;
        }






        public void PingIconChanger(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            Debug.Log("WolfoQoL preSceneDirector");

            if (!SceneInfo.instance) { orig(self); return; }

            switch (SceneInfo.instance.sceneDef.baseSceneName)
            {
                case "skymeadow":
                    GameObject.Find("/PortalDialerEvent/Final Zone/ButtonContainer/PortalDialer").AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                    GameObject.Find("/HOLDER: Zones/OOB Zone").GetComponent<MapZone>().zoneType = MapZone.ZoneType.OutOfBounds;

                    Material CorrectGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser (2)/mdlGeyser").GetComponent<MeshRenderer>().material;
                    Transform WrongGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser").transform;
                    WrongGeyser.GetChild(0).GetComponent<MeshRenderer>().material = CorrectGeyser;
                    WrongGeyser.GetChild(1).GetComponent<MeshRenderer>().material = CorrectGeyser;
                    break;
                case "frozenwall":
                case "itfrozenwall":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GenericDisplayNameProvider[] genericlist = FindObjectsOfType(typeof(GenericDisplayNameProvider)) as GenericDisplayNameProvider[];
                        for (var i = 0; i < genericlist.Length; i++)
                        {
                            //Debug.LogWarning(genericlist[i]); ////DISABLE THIS
                            if (genericlist[i].name.StartsWith("HumanFan"))
                            {
                                genericlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                            }
                            else if (genericlist[i].name.StartsWith("TimedChest"))
                            {
                                genericlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.TimedChestIcon;
                            }
                        }
                    }
                    break;
                case "goolake":
                    //GameObject DoorIsOpenedEffect = GameObject.Find("/HOLDER: Secret Ring Area Content/Entrance/GLRuinGate/DoorIsOpenedEffect/");
                    //DoorIsOpenedEffect.AddComponent<GenericObjectiveProvider>().objectiveToken = "Explore the hidden chamber";

                    /*DummyPingableInteraction[] desertplatelist = FindObjectsOfType(typeof(DummyPingableInteraction)) as DummyPingableInteraction[];
                    for (var i = 0; i < desertplatelist.Length; i++)
                    {
                        //Debug.LogWarning(desertplatelist[i]); ////DISABLE THIS
                        if (desertplatelist[i].name.Contains("GLPressurePlate"))
                        {
                            desertplatelist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                        }
                    }*/
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
                case "foggyswamp":
                    GameObjectUnlockableFilter[] dummylist = FindObjectsOfType(typeof(RoR2.GameObjectUnlockableFilter)) as RoR2.GameObjectUnlockableFilter[];
                    for (var i = 0; i < dummylist.Length; i++)
                    {
                        Destroy(dummylist[i]);
                    }
                    break;
                case "dampcavesimple":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        PurchaseInteraction[] purchaserlistDCS = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                        for (var i = 0; i < purchaserlistDCS.Length; i++)
                        {
                            //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                            if (purchaserlistDCS[i].name.StartsWith("TreebotUnlockInteractable"))
                            {
                                purchaserlistDCS[i].gameObject.GetComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                                GameObject BrokenRexHoloPivot = new GameObject("BrokenRexHoloPivot");
                                BrokenRexHoloPivot.transform.localPosition = new Vector3(0.8f, 4f, -0.2f);
                                BrokenRexHoloPivot.transform.SetParent(purchaserlistDCS[i].gameObject.transform, false);
                                purchaserlistDCS[i].gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = BrokenRexHoloPivot.transform;
                            }
                            else if (purchaserlistDCS[i].name.StartsWith("GoldChest"))
                            {
                                purchaserlistDCS[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                            }
                        }
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
                        PurchaseInteraction[] purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                        for (var i = 0; i < purchaserlist.Length; i++)
                        {
                            if (purchaserlist[i].name.StartsWith("GoldChest"))
                            {
                                purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                            }
                        }
                    }
                    break;
                case "rootjungle":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        PurchaseInteraction[] purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                        for (var i = 0; i < purchaserlist.Length; i++)
                        {
                            //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                            if (purchaserlist[i].name.StartsWith("GoldChest"))
                            {
                                purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.LegendaryChestIcon;
                            }
                        }
                    }

                    UnityEngine.LODGroup[] lodlist = GameObject.Find("HOLDER: Randomization/GROUP: Large Treasure Chests").GetComponentsInChildren<UnityEngine.LODGroup>();
                    for (var i = 0; i < lodlist.Length; i++)
                    {
                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (lodlist[i].name.StartsWith("RJpinkshroom"))
                        {
                            Destroy(lodlist[i]);
                        }
                    }
                    break;
                case "wispgraveyard":
                    break;
                case "bazaar":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        PurchaseInteraction[] purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                        GenericDisplayNameProvider[] genericlist = FindObjectsOfType(typeof(GenericDisplayNameProvider)) as GenericDisplayNameProvider[];

                        //SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = "Leave the <style=cIsLunar>Bazaar Between Time</style>";
                        for (var i = 0; i < purchaserlist.Length; i++)
                        {
                            //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                            if (purchaserlist[i].name.StartsWith("LunarCauldron,"))
                            {
                                purchaserlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.CauldronIcon;
                            }
                            else if (purchaserlist[i].name.StartsWith("SeerStation"))
                            {
                                purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.SeerIcon;
                            }
                            else if (purchaserlist[i].name.StartsWith("LunarShopTerminal"))
                            {
                                purchaserlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.ChestLunarIcon;
                            }
                            else if (purchaserlist[i].name.EndsWith("Recycler"))
                            {
                                GameObject LunarRecyclerPivot = new GameObject("LunarRecyclerPivot");
                                LunarRecyclerPivot.transform.localPosition = new Vector3(0.1f, -1f, 1f);
                                LunarRecyclerPivot.transform.localRotation = new Quaternion(0f, -0.7071f, -0.7071f, 0f);
                                LunarRecyclerPivot.transform.SetParent(purchaserlist[i].gameObject.transform, false);
                                purchaserlist[i].gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LunarRecyclerPivot.transform;
                                purchaserlist[i].gameObject.GetComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
                            }
                            else if (purchaserlist[i].name.StartsWith("LockedMage"))
                            {
                                Destroy(purchaserlist[i].gameObject.GetComponent<RoR2.GameObjectUnlockableFilter>());
                            }
                        }
                        for (var j = 0; j < genericlist.Length; j++)
                        {
                            //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                            if (genericlist[j].name.StartsWith("PortalArena"))
                            {
                                //voidportalpresent++;
                                genericlist[j].gameObject.transform.parent.GetChild(2).gameObject.GetComponent<SphereCollider>().radius = 130;
                                genericlist[j].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.PortalIcon;
                                //Destroy(genericlist[j].gameObject.GetComponent<RoR2.RunEventFlagResponse>());
                                //Destroy(genericlist[j].gameObject.GetComponent<RoR2.EventFunctions>());
                            }
                            else if (genericlist[j].name.StartsWith("Portal"))
                            {
                                genericlist[j].gameObject.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.PortalIcon;
                            }
                        }
                    }
                    break;
                case "moon2":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        Highlight[] highlightlist = FindObjectsOfType(typeof(Highlight)) as Highlight[];
                        BossGroup[] bossgrouplist = FindObjectsOfType(typeof(BossGroup)) as BossGroup[];
                        for (var i = 0; i < highlightlist.Length; i++)
                        {
                            //Debug.LogWarning(highlightlist[i]); ////DISABLE THIS
                            if (highlightlist[i].name.StartsWith("MoonBattery"))
                            {
                                highlightlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.CubeIcon;
                            }
                            else if (highlightlist[i].name.StartsWith("LunarCauldron,"))
                            {
                                highlightlist[i].gameObject.GetComponent<PingInfoProvider>().pingIconOverride = PingIcons.CauldronIcon;
                            }
                            else if (highlightlist[i].name.StartsWith("MoonElevator"))
                            {
                                highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.ExclamationIcon;
                            }
                            else if (highlightlist[i].name.StartsWith("LunarChest"))
                            {
                                highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.ChestLunarIcon;
                            }
                            else if (highlightlist[i].name.StartsWith("ShrineRestack"))
                            {
                                highlightlist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.ShrineOrderIcon;
                            }
                        }

                        for (var i = 0; i < bossgrouplist.Length; i++)
                        {
                            if (bossgrouplist[i].name.StartsWith("BrotherEncounter, Phase 2"))
                            {
                                bossgrouplist[i].bestObservedName = Language.GetString("LUNARGOLEM_BODY_NAME");
                                bossgrouplist[i].bestObservedSubtitle = "<sprite name=\"CloudLeft\" tint=1> " + Language.GetString("LUNARGOLEM_BODY_SUBTITLE") + " <sprite name=\"CloudRight\" tint=1>";
                            }
                        }
                    }
                    break;
                case "arena":
                    if (WConfig.cfgPingIcons.Value)
                    {
                        GameObject PortalArena = GameObject.Find("/PortalArena");
                        PortalArena.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.PortalIcon;
                        //PortalArena.AddComponent<GenericObjectiveProvider>().objectiveToken = "Exit the <style=cIsVoid>Void Fields</style>";

                        PurchaseInteraction[] purchaserlist = FindObjectsOfType(typeof(PurchaseInteraction)) as PurchaseInteraction[];
                        for (var i = 0; i < purchaserlist.Length; i++)
                        {
                            //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                            if (purchaserlist[i].name.StartsWith("NullSafeZone"))
                            {
                                purchaserlist[i].gameObject.GetComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.NullVentIcon;
                            }
                        }
                    }
                    break;
                case "mysteryspace":
                    GameObject MSObelisk = GameObject.Find("/MSObelisk");
                    MSObelisk.AddComponent<GenericObjectiveProvider>().objectiveToken = "Obliterate yourself from existence";
                    break;
                case "artifactworld":
                    GenericPickupController[] pickuplist = FindObjectsOfType(typeof(GenericPickupController)) as GenericPickupController[];
                    for (var i = 0; i < pickuplist.Length; i++)
                    {
                        //Debug.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (pickuplist[i].name.StartsWith("SetpiecePickup"))
                        {
                            //pickuplist[i].gameObject.AddComponent<RoR2.PingInfoProvider>().pingIconOverride = PingIcons.CubeIcon;
                            ArtifactDef tempartifactdef = ArtifactCatalog.GetArtifactDef(pickuplist[i].pickupIndex.pickupDef.artifactIndex);
                            if (tempartifactdef)
                            {
                                string artifactname = Language.GetString(tempartifactdef.nameToken);
                                artifactname = artifactname.Replace("Artifact of ", "");
                                artifactname = artifactname.Replace("the ", "");
                                pickuplist[i].gameObject.AddComponent<RoR2.GenericObjectiveProvider>().objectiveToken = "Complete the \n<style=cArtifact>Trial of " + artifactname + "</style>";
                            }
                            if (WConfig.ArtifactOutline.Value == true)
                            {
                                RoR2.Highlight tempartifact = pickuplist[i].gameObject.GetComponent<Highlight>();
                                tempartifact.pickupIndex = pickuplist[i].pickupIndex;
                                tempartifact.highlightColor = Highlight.HighlightColor.pickup;
                                tempartifact.isOn = true;
                            }
                        }
                    }
                    GC.Collect();
                    break;
                case "voidstage":
                    GameObject MissionController = GameObject.Find("/MissionController");
                    MissionController.GetComponent<VoidStageMissionController>().deepVoidPortalObjectiveProvider = null;
                    break;
                case "voidraid":
                    break;
            };

            orig(self);

            GC.Collect();
            if (WConfig.cfgRemindersTreasure.Value == true)
            {
                if (SceneInfo.instance.countsAsStage)
                {
                    Reminders.TreasureReminder.SetupReminders();
                }
            }
            if (WConfig.cfgPingIcons.Value)
            {
                if (self.teleporterSpawnCard != null)
                {
                    if (self.teleporterSpawnCard.name == "iscLunarTeleporter")
                    {
                        RoR2.UI.ChargeIndicatorController[] tempchargelist = FindObjectsOfTypeAll(typeof(RoR2.UI.ChargeIndicatorController)) as RoR2.UI.ChargeIndicatorController[];
                        for (var i = 0; i < tempchargelist.Length; i++)
                        {
                            if (tempchargelist[i].name.StartsWith("TeleporterChargingPositionIndicator(Clone)"))
                            {
                                tempchargelist[i].iconSprites[0].sprite = PingIcons.PrimordialTeleporterChargedIcon;
                            }
                        }
                        //Destroy(tempchargelist);
                    }
                }
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

}