using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using WolfoQoL_Client.Skins;


namespace WolfoQoL_Client
{

    public class VisualsMain
    {
        public static void Start()
        {
            if (WConfig.module_visuals_icons.Value)
            {
                BodyIcons.Start();
                if (WConfig.cfgPingIcons.Value)
                {
                    PingIcons.Start();
                    PingIcons_Hooks.Start();
                   
                }
                BuffIcons.Awake();
                //Ig not text but it is a reminder?
                Icon_Colors.Start();
                MissionPointers.Start();

            }
            if (WConfig.module_visuals_skins.Value)
            {
                SkinChanges.Start();
                SkinTouchups.Start();

            }
            if (WConfig.module_visuals_other.Value)
            {
                ColorModule.Main();
                DuplicatorModel.DuplicatorModelChanger();
                OptionPickup_Visuals.Start();

                VisualsMisc.Start();

                HoldoutZoneFlowers.FlowersForOtherHoldoutZones();
                ChefOil.Start();

                On.RoR2.Stage.PreStartClient += VisualStageChanges;

                GameModeCatalog.availability.CallWhenAvailable(CallLate);
            }
 
        }



        public static void VisualStageChanges(On.RoR2.Stage.orig_PreStartClient orig, Stage self)
        {
            orig(self);
            switch (SceneInfo.instance.sceneDef.baseSceneName)
            {
                case "foggyswamp":
                    GameObjectUnlockableFilter[] dummylist = Object.FindObjectsOfType(typeof(RoR2.GameObjectUnlockableFilter)) as RoR2.GameObjectUnlockableFilter[];
                    for (var i = 0; i < dummylist.Length; i++)
                    {
                        WQoLMain.log.LogMessage(dummylist[i]);
                        Object.Destroy(dummylist[i]);
                    }
                    break;
                case "wispgraveyard":
                    if (WConfig.cfgSofterShadows.Value)
                    {
                        GameObject lighting = GameObject.Find("/Weather, Wispgraveyard/Directional Light (SUN)");
                        Light sun = lighting.GetComponent<Light>();
                        if (sun.intensity == 1.3f) //Default lighting check
                        {
                            if (sun.shadowStrength == 1f)
                            {
                                sun.shadowStrength = 0.8f;
                            }
                        }
                    }
                    break;
                case "sulfurpools":
                    if (WConfig.cfgNewGeysers.Value)
                    {
                        GameObject GeyserHolder = GameObject.Find("/HOLDER: Geysers");
                        Material MatGeyserSulfurPools = Object.Instantiate(GeyserHolder.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material);
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
                    GameObject REX = GameObject.Find("/TreebotUnlockInteractable");
                    if (REX)
                    {
                        GameObject BrokenRexHoloPivot = new GameObject("BrokenRexHoloPivot");
                        BrokenRexHoloPivot.transform.localPosition = new Vector3(0.8f, 4f, -0.2f);
                        BrokenRexHoloPivot.transform.SetParent(REX.gameObject.transform, false);
                        REX.gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = BrokenRexHoloPivot.transform;
                        REX.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.QuestionMarkIcon;
                    }
                    if (WConfig.cfgNewGeysers.Value)
                    {
                        //Geyser
                        GameObject GeyserHolderDamp = GameObject.Find("/HOLDER: Geyser");
                        Material MatLavaGeyser = Object.Instantiate(GeyserHolderDamp.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<ParticleSystemRenderer>().material);
                        Material MatLavaGeyser2 = Object.Instantiate(MatLavaGeyser);
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
                    if (WConfig.cfgSofterShadows.Value)
                    {
                        GameObject lighting = GameObject.Find("/HOLDER: Lighting, PP, Wind, Misc/Directional Light (SUN)");
                        Light sun = lighting.GetComponent<Light>();
                        if (sun.intensity == 0.5f)
                        {
                            if (sun.shadowStrength == 1f)
                            {
                                sun.shadowStrength = 0.8f; //1f
                            }
                        }
                    }
                    break;
                case "rootjungle":
                    if (WConfig.cfgSofterShadows.Value)
                    {
                        GameObject lighting = GameObject.Find("/HOLDER: Weather Set 1/Directional Light (SUN)");
                        Light sun = lighting.GetComponent<Light>();
                        if (sun.intensity == 0.7f)
                        {
                            //sun.intensity = 0.8f; //0.7f
                            if (sun.shadowStrength == 1f)
                            {
                                sun.shadowStrength = 0.6f; //1f
                            }
                        }
                    }
                    //Pink Mushroom always
                    LODGroup[] lodlist = GameObject.Find("HOLDER: Randomization/GROUP: Large Treasure Chests").GetComponentsInChildren<LODGroup>();
                    for (var i = 0; i < lodlist.Length; i++)
                    {
                        //WolfoMain.log.LogWarning(purchaserlist[i]); ////DISABLE THIS
                        if (lodlist[i].name.StartsWith("RJpinkshroom"))
                        {
                            Object.Destroy(lodlist[i]);
                        }
                    }
                    break;
                case "skymeadow":

                    GameObject.Find("/HOLDER: Zones/OOB Zone").GetComponent<MapZone>().zoneType = MapZone.ZoneType.OutOfBounds;
                    Material CorrectGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser (2)/mdlGeyser").GetComponent<MeshRenderer>().material;
                    Transform WrongGeyser = GameObject.Find("/HOLDER: Randomization/GROUP: Plateau 13 and Underground/Underground/Geyser").transform;
                    WrongGeyser.GetChild(0).GetComponent<MeshRenderer>().material = CorrectGeyser;
                    WrongGeyser.GetChild(1).GetComponent<MeshRenderer>().material = CorrectGeyser;
                    break;
                case "helminthroost":
                    if (WConfig.cfgHelminthLightingFix.Value)
                    {
                        GameObject lighting = GameObject.Find("/HOLDER: Lighting/Weather, Helminthroost/PP + Amb");
                        var PP = lighting.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessVolume>();
                        PP.priority = 2.1f;
                    }
                    break;
                case "moon2":
                    //Maybe this o
                    GameObject Phase2 = GameObject.Find("/SceneInfo/BrotherMissionController/BrotherEncounter, Phase 2");
                    if (Phase2)
                    {
                        Phase2.GetComponent<BossGroup>().bestObservedName = Language.GetString("LUNAR_CHIMERA");
                        Phase2.GetComponent<BossGroup>().bestObservedSubtitle = "<sprite name=\"CloudLeft\" tint=1> " + Language.GetString("LUNARGOLEM_BODY_SUBTITLE") + " <sprite name=\"CloudRight\" tint=1>";
                    }
                    GameObject Elevators = GameObject.Find("/HOLDER: Elevators");
                    if (Elevators)
                    {
                        var platforms = Elevators.GetComponentsInChildren<GenericInteraction>();
                        foreach (GenericInteraction a in platforms)
                        {
                            a.gameObject.AddComponent<BlockScanner>();
                        }
                    }
                    break;
                case "bazaar":
                    GameObject LunarRecycler = GameObject.Find("/HOLDER: Store/LunarShop/LunarRecycler");
                    if (LunarRecycler)
                    {
                        GameObject LunarRecyclerPivot = new GameObject("LunarRecyclerPivot");
                        LunarRecyclerPivot.transform.localPosition = new Vector3(0.1f, -1f, 1f);
                        LunarRecyclerPivot.transform.localRotation = new Quaternion(0f, -0.7071f, -0.7071f, 0f);
                        LunarRecyclerPivot.transform.SetParent(LunarRecycler.gameObject.transform, false);
                        LunarRecycler.gameObject.AddComponent<RoR2.Hologram.HologramProjector>().hologramPivot = LunarRecyclerPivot.transform;
                        LunarRecycler.gameObject.GetComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
                        //LunarRecycler.AddComponent<PingInfoProvider>().pingIconOverride = PingIcons.QuestionMarkIcon;
                    }
                    GameObject PortalArena = GameObject.Find("/HOLDER: Starting Cave/HOLDER: Arena Entrance/Trigger/PortalArena/");
                    PortalArena.transform.parent.GetChild(2).gameObject.GetComponent<SphereCollider>().radius = 130;



                    if (WConfig.ArtificerBazaarAlways.Value)
                    {
                        GameObject Artificer = GameObject.Find("/HOLDER: Store/HOLDER: Store Platforms/LockedMage/");
                        Artificer.GetComponent<GameObjectUnlockableFilter>().enabled = false;
                        if (Artificer.GetComponent<GameObjectUnlockableFilter>().ShouldShowGameObject() == false)
                        {
                            Artificer.GetComponent<PurchaseInteraction>().Networkavailable = false;
                        }
                    }

                    break;
                case "meridian":
                    if (VisibleHabitatFall.Should())
                    {
                        VisibleHabitatFall.PrimeMeridian();
                    }
             
                    GameObject MeridianEventTriggerCore = GameObject.Find("/HOLDER: Design/MeridianEventTriggerCore/");
                    MeridianEventTriggerCore.GetComponent<NetworkIdentity>().isPingable = false;

                    GameObject PMHead = GameObject.Find("/HOLDER: Art/Colossus Statue/PMHead/");
                    SphereCollider ping = PMHead.AddComponent<SphereCollider>();
                    ping.isTrigger = true;
                    ping.radius = 250;
                    ping.center = new Vector3(0, 100, 0);
                    ModelLocator modelLocator = PMHead.AddComponent<ModelLocator>();
                    modelLocator.dontDetatchFromParent = true;
                    modelLocator.autoUpdateModelTransform = false;
                    modelLocator._modelTransform = PMHead.transform;
                    CharacterModel characterModel = PMHead.AddComponent<CharacterModel>();
                    /*characterModel.baseRendererInfos = new CharacterModel.RendererInfo[]
                    {
                        new CharacterModel.RendererInfo
                        {
                            renderer = PMHead.transform.GetChild(2).GetComponent<MeshRenderer>(),
                        }, new CharacterModel.RendererInfo
                        {
                            renderer = PMHead.transform.GetChild(5).GetComponent<MeshRenderer>(),
                        }, new CharacterModel.RendererInfo
                        {
                            renderer = PMHead.transform.GetChild(7).GetComponent<MeshRenderer>(),
                        }, new CharacterModel.RendererInfo
                        {
                            renderer = PMHead.transform.GetChild(8).GetComponent<MeshRenderer>(),
                        }, new CharacterModel.RendererInfo
                        {
                            renderer = PMHead.transform.GetChild(9).GetComponent<MeshRenderer>(),
                        }, new CharacterModel.RendererInfo
                        {
                            renderer = PMHead.transform.GetChild(10).GetComponent<MeshRenderer>(),
                        }
                    };*/
                    break;
                case "lemuriantemple":
                    if (VisibleHabitatFall.Should())
                    {
                        VisibleHabitatFall.LemurianTemple();
                    }
                    break;

            }

        }




        public static void CallLate()
        {
            OtherEnemies.CallLate();

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
            if (WConfig.cfgColorMain.Value == true)
            {
                ColorModule.AddMissingItemHighlights();
                ColorModule.ChangeColorsPost();
            }

            DLC2Content.Equipment.HealAndReviveConsumed.pickupModelPrefab.AddComponent<ModelPanelParameters>();
            //DLC2Content.Buffs.ExtraLifeBuff.iconSprite = DLC2Content.Buffs.HealAndReviveRegenBuff.iconSprite;

            RoR2Content.Items.MinHealthPercentage.hidden = false;
            var orang = ItemTierCatalog.FindTierDef("OrangeTierDef");
            if (orang != null)
            {
                OptionPickup_Visuals.orange = orang.tier;
            }
        }



    }
}