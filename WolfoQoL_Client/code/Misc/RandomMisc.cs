using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;

namespace WolfoQoL_Client
{
    public class RandomMisc
    {

        public static void Start()
        {
           


            DifficultyColors();

            RandomMiscWithConfig.Start();

            HoldoutZoneFlowers.FlowersForOtherHoldoutZones();

            VoidAffix();

            bool otherMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("0p41.Sots_Items_Reworked");
            if (!otherMod)
            {
                //This like runs for every character in the game for a very minor benefit idk if that's really worth it.
                IL.RoR2.InteractionDriver.MyFixedUpdate += BiggerSaleStarRange;
            }

            Material matChild = Addressables.LoadAssetAsync<Material>(key: "RoR2/DLC2/Child/matChild.mat").WaitForCompletion();
            matChild.SetFloat("_EliteBrightnessMax", 1.09f); //2.18
            matChild.SetFloat("_EliteBrightnessMin", -0.7f); //-1.4


            GameObject LowerPricedChestsGlow = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/LowerPricedChestsGlow");
            LowerPricedChestsGlow.transform.GetChild(0).localPosition = new Vector3(0, 0.6f, 0f);
            LowerPricedChestsGlow.transform.GetChild(1).localPosition = new Vector3(0, 0.6f, 0f);
            LowerPricedChestsGlow.transform.GetChild(2).localPosition = new Vector3(0, 0.4f, 0f);
            LowerPricedChestsGlow.transform.GetChild(3).localPosition = new Vector3(0, 0.7f, 0f);
            LowerPricedChestsGlow.transform.GetChild(3).localScale = new Vector3(1, 1.4f, 1f);



            //Sorting the hidden stages in the menu, kinda dubmb but whatevs

            Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC2/meridian/meridian.asset").WaitForCompletion().stageOrder = 80;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/goldshores").stageOrder = 81; //Next to Meridian

            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/bazaar").stageOrder = 90; //Bazaar -> Void -> Void -> Void
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/arena").stageOrder = 91;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/voidstage").stageOrder = 92;
            Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/DLC1/voidraid/voidraid.asset").WaitForCompletion().stageOrder = 93;

            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/mysteryspace").stageOrder = 110;
            LegacyResourcesAPI.Load<SceneDef>("SceneDefs/limbo").stageOrder = 111;


            #region LunarScavLunarBeadBead
            GivePickupsOnStart.ItemDefInfo Beads = new GivePickupsOnStart.ItemDefInfo { itemDef = LegacyResourcesAPI.Load<ItemDef>("ItemDefs/LunarTrinket"), count = 1, dontExceedCount = true };
            GivePickupsOnStart.ItemDefInfo[] ScavLunarBeadsGiver = new GivePickupsOnStart.ItemDefInfo[] { Beads };

            LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar1Master").AddComponent<GivePickupsOnStart>().itemDefInfos = ScavLunarBeadsGiver;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar2Master").AddComponent<GivePickupsOnStart>().itemDefInfos = ScavLunarBeadsGiver;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar3Master").AddComponent<GivePickupsOnStart>().itemDefInfos = ScavLunarBeadsGiver;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/charactermasters/ScavLunar4Master").AddComponent<GivePickupsOnStart>().itemDefInfos = ScavLunarBeadsGiver;
            #endregion

            #region Captain Shock Beacon Radius
            GameObject CaptainShockBeaconRadius = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Captain/CaptainSupplyDrop, Hacking.prefab").WaitForCompletion().transform.GetChild(2).GetChild(0).GetChild(4).gameObject, "ShockIndicator", false);
            Material CaptainHackingBeaconIndicatorMaterial = Object.Instantiate(CaptainShockBeaconRadius.transform.GetChild(0).GetComponent<MeshRenderer>().material);
            CaptainHackingBeaconIndicatorMaterial.SetColor("_TintColor", new Color(0, 0.4f, 0.8f, 1f));
            CaptainShockBeaconRadius.transform.GetChild(0).GetComponent<MeshRenderer>().material = CaptainHackingBeaconIndicatorMaterial;
            CaptainShockBeaconRadius.transform.localScale = new Vector3(6.67f, 6.67f, 6.67f);


            GameObject shockBeacon = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Captain/CaptainSupplyDrop, Shocking.prefab").WaitForCompletion();
            shockBeacon.transform.GetChild(2).GetChild(0).gameObject.AddComponent<InstantiateGameObjectAtLocation>().objectToInstantiate = CaptainShockBeaconRadius;
            #endregion


            //UI spreading like how charging works on other characters
            On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.OnEnter += (orig, self) =>
            {
                orig(self);
                self.characterBody.AddSpreadBloom(0.4f);
            };
            On.EntityStates.VoidSurvivor.Weapon.ReadyMegaBlaster.FixedUpdate += (orig, self) =>
            {
                orig(self);
                self.characterBody.SetSpreadBloom(0.2f, true);
            };



            //Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/bazaar/LunarInfectionSmallMesh.prefab").WaitForCompletion();

            /*On.RoR2.ShopTerminalBehavior.PreStartClient += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("Duplicator"))
                {
                    if (self.pickupIndex != PickupIndex.none && self.pickupIndex.pickupDef.isLunar)
                    {
                        GameObject LunarInfection = GameObject.Instantiate(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/bazaar/LunarInfectionSmallMesh.prefab").WaitForCompletion(), self.gameObject.transform,false);
                        LunarInfection.transform.localPosition = new Vector3(-1.2f, 1.6f, -0.45f);
                        LunarInfection.transform.localScale = new Vector3(0.35f, 0.6f, 0.6f);
                        LunarInfection.transform.localEulerAngles = new Vector3(345f, 330f, 270f);

                    }
                }
            }; */



            SceneDef rootjungle = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/Base/rootjungle/rootjungle.asset").WaitForCompletion();
            MusicTrackDef MusicSulfurPoolsBoss = Addressables.LoadAssetAsync<MusicTrackDef>(key: "RoR2/DLC1/Common/muBossfightDLC1_12.asset").WaitForCompletion();
            rootjungle.bossTrack = MusicSulfurPoolsBoss;


            //On.RoR2.UI.PingIndicator.RebuildPing += PlayerPings;
            IL.RoR2.UI.PingIndicator.RebuildPing += PlayerPing.PlayerPingsIL;


            IL.RoR2.UI.ScoreboardController.Rebuild += ScoreboardForDeadPeopleToo;

            On.RoR2.SubjectChatMessage.GetSubjectName += SubjectChatMessage_GetSubjectName;


            GameObject MiniGeodeBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/MiniGeodeBody.prefab").WaitForCompletion();
            MiniGeodeBody.transform.localScale = Vector3.one * 1.5f;
            Light geode = MiniGeodeBody.transform.GetChild(0).GetChild(2).GetComponent<Light>();
            geode.range = 25;
            geode.intensity = 8;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/MiniGeodeBody.prefab").WaitForCompletion();

            On.RoR2.Util.GetBestBodyName += MarriedLemurianNameHook; //Maybe a little excessive idk


            GameObject NoCooldownEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/KillEliteFrenzy/NoCooldownEffect.prefab").WaitForCompletion();
            NoCooldownEffect.GetComponentInChildren<PostProcessVolume>().priority = 19;

        }

        public static BodyIndex LemurianBruiser = BodyIndex.None;
        public static string MarriedLemurianNameHook(On.RoR2.Util.orig_GetBestBodyName orig, GameObject bodyObject)
        {
            if (bodyObject)
            {
                CharacterBody characterBody = bodyObject.GetComponent<CharacterBody>();
                if (characterBody && characterBody.bodyIndex == LemurianBruiser)
                {
                    if (characterBody.inventory.GetItemCount(RoR2Content.Items.Clover) >= 20)
                    {
                        if (characterBody.inventory.GetItemCount(RoR2Content.Items.FireRing) > 0)
                        {
                            return "Kjaro";
                        }
                        else if (characterBody.inventory.GetItemCount(RoR2Content.Items.IceRing) > 0)
                        {
                            return "Runald";
                        }
                    }
                }
            }
            return orig(bodyObject);
        }


        private static string SubjectChatMessage_GetSubjectName(On.RoR2.SubjectChatMessage.orig_GetSubjectName orig, SubjectChatMessage self)
        {
            if (self.subjectAsNetworkUser == null && self.subjectAsCharacterBody)
            {
                return RoR2.Util.GetBestBodyName(self.subjectAsCharacterBody.gameObject);
            }
            return orig(self);
        }

        private static void DifficultyColors()
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


        private static void ScoreboardForDeadPeopleToo(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchStloc(0)))
            {
                c.EmitDelegate<System.Func<List<PlayerCharacterMasterController>, List<PlayerCharacterMasterController>>>((self) =>
                {
                    List<PlayerCharacterMasterController> list = (from x in PlayerCharacterMasterController.instances
                                                                  where x.gameObject.activeInHierarchy && x.isConnected
                                                                  select x).ToList<PlayerCharacterMasterController>();
                    return list;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: ScoreboardForDeadPeopleToo");
            }

        }

        private static void BiggerSaleStarRange(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Items", "LowerPricedChests"));

            if (c.TryGotoPrev(MoveType.After,
                x => x.MatchLdfld("RoR2.InteractionDriver", "currentInteractable")))
            {

                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<GameObject, InteractionDriver, GameObject>>((target, interactionDriver) =>
                {
                    if (interactionDriver.networkIdentity.hasAuthority) //Only players
                    {
                        //IS checkign for an item less intensive
                        //Or checking for every interactable in a decent radius
                        if (interactionDriver.characterBody.inventory.GetItemCount(DLC2Content.Items.LowerPricedChests) > 0)
                        {
                            if (target == null)
                            {
                                /*if (interactionDriver.interactableCheckCooldown > 0f)
                                {
                                    return interactionDriver.currentInteractable;
                                }
                                //What?
                                interactionDriver.interactableCheckCooldown = 0f;*/
                                float num = 0f;
                                float num2 = interactionDriver.interactor.maxInteractionDistance * 2f;

                                Vector3 vector = interactionDriver.inputBank.aimOrigin;
                                Vector3 vector2 = interactionDriver.inputBank.aimDirection;
                                if (interactionDriver.useAimOffset)
                                {
                                    Vector3 a = vector + vector2 * num2;
                                    vector = interactionDriver.inputBank.aimOrigin + interactionDriver.aimOffset;
                                    vector2 = (a - vector) / num2;
                                }
                                Ray originalAimRay = new Ray(vector, vector2);
                                Ray raycastRay = CameraRigController.ModifyAimRayIfApplicable(originalAimRay, interactionDriver.gameObject, out num);
                                target = interactionDriver.interactor.FindBestInteractableObject(raycastRay, num2 + num, originalAimRay.origin, num2);
                            };
                            if (target)
                            {
                                PurchaseInteraction component = target.GetComponent<PurchaseInteraction>();
                                if (component != null && component.saleStarCompatible && !interactionDriver.saleStarEffect)
                                {
                                    interactionDriver.saleStarEffect = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/LowerPricedChestsGlow"), target.transform.position, Quaternion.identity, target.transform);
                                    return null;
                                }
                            }
                            else if (interactionDriver.saleStarEffect)
                            {
                                UnityEngine.Object.Destroy(interactionDriver.saleStarEffect);
                                return null;
                            }
                        }
                        else if (interactionDriver.saleStarEffect)
                        {
                            if (!interactionDriver.saleStarEffect.GetComponent<ObjectScaleCurve>())
                            {
                                AnimationCurve newCurve = new AnimationCurve
                                {
                                    keys = new Keyframe[]
                                      {
                                            new Keyframe(0f,1f),
                                            new Keyframe(0.25f,1.05f),
                                            new Keyframe(1f,0f)
                                      },
                                    postWrapMode = WrapMode.Once,
                                    preWrapMode = WrapMode.Once,
                                };

                                //UnityEngine.Object.Destroy(interactionDriver.saleStarEffect);
                                interactionDriver.saleStarEffect.AddComponent<ObjectScaleCurve>();
                                ObjectScaleCurve scale = interactionDriver.saleStarEffect.transform.GetChild(2).gameObject.AddComponent<ObjectScaleCurve>();
                                scale.overallCurve = newCurve;
                                scale.useOverallCurveOnly = true;
                                scale.timeMax = 1.1f;
                                scale = interactionDriver.saleStarEffect.transform.GetChild(3).gameObject.AddComponent<ObjectScaleCurve>();
                                scale.overallCurve = newCurve;
                                scale.useOverallCurveOnly = true;
                                scale.timeMax = 1.1f;
                                scale = interactionDriver.saleStarEffect.transform.GetChild(0).gameObject.AddComponent<ObjectScaleCurve>();
                                scale.overallCurve = newCurve;
                                scale.useOverallCurveOnly = true;
                                scale.timeMax = 1.1f;
                                Object.Destroy(interactionDriver.saleStarEffect, 1.6f);
                            }
                            return null;
                        }
                    }
                    return null;
                });

                c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("RoR2.InteractionDriver", "saleStarEffect"),
                x => x.MatchCall("UnityEngine.Object", "Destroy"));
                c.TryGotoNext(MoveType.Before,
                x => x.MatchLdfld("RoR2.InteractionDriver", "saleStarEffect"),
                x => x.MatchCall("UnityEngine.Object", "Destroy"));
                c.TryGotoPrev(MoveType.After,
                x => x.MatchLdfld("RoR2.InteractionDriver", "saleStarEffect"));
                //Debug.Log(c);
                c.EmitDelegate<System.Func<GameObject, GameObject>>((target) =>
                {
                    return null;
                });



                Debug.Log("IL Found: Sale Star Range");
            }
            else
            {
                Debug.LogWarning("IL Failed: Sale Star Range");
            }
        }

        public static void VoidAffix()
        {
            //Do we even need to add a display?
            EquipmentDef VoidAffix = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteVoid/EliteVoidEquipment.asset").WaitForCompletion();
            GameObject VoidAffixDisplay = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/EliteVoid/DisplayAffixVoid.prefab").WaitForCompletion(), "PickupAffixVoidW", false);
            VoidAffixDisplay.transform.GetChild(0).GetChild(1).SetAsFirstSibling();
            VoidAffixDisplay.transform.GetChild(1).localPosition = new Vector3(0f, 0.7f, 0f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, -0.5f, -0.6f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(0).localScale = new Vector3(1.5f, 1.5f, 1.5f);
            VoidAffixDisplay.transform.GetChild(1).GetChild(1).gameObject.SetActive(false);
            VoidAffixDisplay.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
            VoidAffixDisplay.transform.GetChild(0).eulerAngles = new Vector3(310, 0, 0);
            VoidAffixDisplay.transform.GetChild(0).localScale = new Vector3(0.75f, 0.75f, 0.75f);

            ItemDisplay display = VoidAffixDisplay.GetComponent<ItemDisplay>();
            display.rendererInfos = display.rendererInfos.Remove(display.rendererInfos[4]);

            VoidAffix.pickupModelPrefab = VoidAffixDisplay;
        }


        public class InstantiateGameObjectAtLocation : MonoBehaviour
        {
            public GameObject objectToInstantiate;

            public void Start()
            {
                Instantiate(objectToInstantiate, this.transform);
            }
        }
    }

}
