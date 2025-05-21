using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQoL_Client
{
    internal class MissionPointers
    {
        //Show Halcyonite Beacons after buying 5
        //Show next Void Cell
        //Briefly show Pillars after completing one
        //Void Pillars too I guess

        public static void Start()
        {
            On.EntityStates.Missions.Arena.NullWard.WardOnAndReady.OnEnter += VoidFields_AddPointer;
            On.EntityStates.Missions.Arena.NullWard.WardOnAndReady.OnExit += VoidFields_RemovePointer;
            On.EntityStates.Missions.Moon.MoonBatteryComplete.OnEnter += LunarPillar_AddPointers;

            On.EntityStates.Interactables.GoldBeacon.NotReady.OnEnter += GildedCoast_Client;
            On.EntityStates.Interactables.GoldBeacon.Ready.OnEnter += GildedCoast_Add;



            AnimationCurve newCurve = new AnimationCurve
            {
                keys = new Keyframe[]
                {
                    new Keyframe(0f,1f),
                    new Keyframe(0.5f,1f),
                    new Keyframe(1f,0f)
                },
                postWrapMode = WrapMode.ClampForever,
                preWrapMode = WrapMode.ClampForever,
            };


            #region Lunar Pillar Indicator
            GameObject newIndicator = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidCamp/VoidCampPositionIndicator.prefab").WaitForCompletion(), "LunarPillarPositionIndicator", false);
            MissionPointer.LunarPillarPositionIndicator = newIndicator;
            var positionIndicator = newIndicator.GetComponent<PositionIndicator>();
            positionIndicator.outsideViewObject.SetActive(false);
            positionIndicator.outsideViewObject = positionIndicator.insideViewObject;
            var sprites = positionIndicator.GetComponentsInChildren<SpriteRenderer>(true);
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = new Color(0.5223f, 0.8071f, 0.9151f, 1f);
            }
            ObjectScaleCurve scale = newIndicator.transform.GetChild(2).GetChild(0).GetComponent<ObjectScaleCurve>();
            scale.timeMax = 11;
            scale.useOverallCurveOnly = true;
            scale.overallCurve = newCurve;
            DestroyOnTimer destroy = newIndicator.AddComponent<DestroyOnTimer>();
            destroy.duration = scale.timeMax + 1;
            #endregion
            #region Gold Indicator
            newIndicator = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerAssets/WaveEndPositionIndicator.prefab").WaitForCompletion(), "GoldShoresPositionIndicator", false);
            MissionPointer.GoldShoresPositionIndicator = newIndicator;
            Object.Destroy(newIndicator.transform.GetChild(2).GetChild(0).GetComponent<ObjectScaleCurve>());
            scale = newIndicator.AddComponent<ObjectScaleCurve>();
            scale.timeMax = 2.5f;
            scale.useOverallCurveOnly = true;
            scale.overallCurve = newCurve;

            destroy = newIndicator.AddComponent<DestroyOnTimer>();
            destroy.duration = scale.timeMax + 1;
            #endregion

            GameObject DeepVoidPortalBattery = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/DeepVoidPortalBattery/DeepVoidPortalBattery.prefab").WaitForCompletion();
            Transform Beam = DeepVoidPortalBattery.transform.GetChild(0).GetChild(2).GetChild(3).GetChild(0);
            Beam.localScale = new Vector3(1, 3, 1); //LongerBeam

            //2D beam fix
            ParticleSystem ps = Beam.GetComponent<ParticleSystem>();
            var psM = ps.main;
            //psM.startRotationX = 6.66f;
            psM.startRotationXMultiplier = 6.66f;
            psM.startRotation3D = true;

            ////Explain
            //Arenas beams aren't 3d, they're 2d, but start with random Rotation,
            //If we add random rotation, and fix the starting point, it'd work

            //Random X axis dont work it seems

            //270 90 0
        }


        private static void GildedCoast_Client(On.EntityStates.Interactables.GoldBeacon.NotReady.orig_OnEnter orig, EntityStates.Interactables.GoldBeacon.NotReady self)
        {
            orig(self);
            if (!NetworkServer.active && GoldshoresMissionController.instance)
            {
                GoldshoresMissionController.instance.beaconInstanceList.Add(self.gameObject);
            }
        }

        private static void GildedCoast_Add(On.EntityStates.Interactables.GoldBeacon.Ready.orig_OnEnter orig, EntityStates.Interactables.GoldBeacon.Ready self)
        {
            orig(self);
            if (WConfig.cfgMissionPointers.Value == false)
            {
                return;
            }
            if (GoldshoresMissionController.instance)
            {
                if (GoldshoresMissionController.instance.beaconsActive > 3)
                {
                    var objectList = GoldshoresMissionController.instance.beaconInstanceList;
                    for (int i = 0; i < objectList.Count; i++)
                    {
                        GameObject.Destroy(objectList[i].gameObject.GetComponent<MissionPointer>());
                        if (objectList[i].GetComponent<EntityStateMachine>().state is EntityStates.Interactables.GoldBeacon.NotReady)
                        {
                            MissionPointer pointer = objectList[i].gameObject.AddComponent<MissionPointer>();
                            pointer.version = MissionPointer.Version.Gold;
                            pointer.AddIndicator();
                        }
                    }
                }
            }
        }

        private static void VoidFields_RemovePointer(On.EntityStates.Missions.Arena.NullWard.WardOnAndReady.orig_OnExit orig, EntityStates.Missions.Arena.NullWard.WardOnAndReady self)
        {
            orig(self);
            GameObject.Destroy(self.gameObject.GetComponent<MissionPointer>());
        }

        private static void VoidFields_AddPointer(On.EntityStates.Missions.Arena.NullWard.WardOnAndReady.orig_OnEnter orig, EntityStates.Missions.Arena.NullWard.WardOnAndReady self)
        {
            orig(self);
            self.gameObject.transform.GetChild(3).GetChild(0).GetChild(0).localScale = new Vector3(1.5f, 3f, 1.5f);
            if (WConfig.cfgMissionPointers.Value == false)
            {
                return;
            }
            MissionPointer pointer = self.gameObject.AddComponent<MissionPointer>();
            pointer.version = MissionPointer.Version.Void;
            pointer.AddIndicator();
        }

        private static void LunarPillar_AddPointers(On.EntityStates.Missions.Moon.MoonBatteryComplete.orig_OnEnter orig, EntityStates.Missions.Moon.MoonBatteryComplete self)
        {
            orig(self);
            if (WConfig.cfgMissionPointers.Value == false)
            {
                return;
            }
            //For every pillar, not completed, 
            if (MoonBatteryMissionController.instance)
            {
                var battery = MoonBatteryMissionController.instance.batteryHoldoutZones;
                for (int i = 0; i < battery.Length; i++)
                {
                    GameObject.Destroy(battery[i].gameObject.GetComponent<MissionPointer>());
                    if (battery[i].charge == 0)
                    {
                        MissionPointer pointer = battery[i].gameObject.AddComponent<MissionPointer>();
                        pointer.version = MissionPointer.Version.Lunar;
                        pointer.AddIndicator();
                    }
                }
            }

        }

        public class MissionPointer : MonoBehaviour
        {
            public PositionIndicator positionIndicator;
            public GameObject indicator;
            public GameObject indicatorPrefab;


            public static GameObject LunarPillarPositionIndicator;
            public static GameObject GoldShoresPositionIndicator;
            public Version version;
            public float destroyAfter;


            public enum Version
            {
                Gold,
                Void,
                Lunar,
                Item,
            }
            public void OnEnable()
            {
                if (WConfig.cfgMissionPointers.Value == false)
                {
                    Destroy(this);
                    return;
                }
                //Can't seem to do it here
            }
            public void AddIndicator()
            {
                if (WConfig.cfgMissionPointers.Value == false)
                {
                    Destroy(this);
                    return;
                }
                if (indicator)
                {
                    return;
                }
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/skinObjectivePanel.asset").WaitForCompletion();
                if (version == Version.Void)
                {
                    indicatorPrefab = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidCamp/VoidCampPositionIndicator.prefab").WaitForCompletion();
                }
                else if (version == Version.Lunar)
                {
                    indicatorPrefab = LunarPillarPositionIndicator;
                }
                else
                {
                    indicatorPrefab = GoldShoresPositionIndicator;
                }
                if (indicatorPrefab)
                {
                    indicator = Instantiate<GameObject>(indicatorPrefab, transform);
                    positionIndicator = indicator.GetComponent<PositionIndicator>();
                    positionIndicator.targetTransform = gameObject.transform;
                }
                if (version == Version.Void)
                {
                    var scale = indicator.GetComponentInChildren<ObjectScaleCurve>(true);
                    if (scale)
                    {
                        scale.timeMax = 22f;
                    }

                }
            }
            public void OnDisable()
            {
                if (indicator)
                {
                    UnityEngine.Object.Destroy(indicator);
                }
            }
        }
    }
}
