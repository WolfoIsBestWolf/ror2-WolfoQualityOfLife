using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQoL_Client
{
    public class ItemOrbPhysicsChanger : MonoBehaviour
    {
        public Rigidbody body;
        public void Awake()
        {
            body = this.GetComponent<Rigidbody>();
        }
        public void FixedUpdate()
        {
            if (body && body.velocity.y < -30)
            {
                this.gameObject.layer = 8;
                Destroy(this);
            }
        }
    }


    public class GameplayQualityOfLife
    {
        public static void Start()
        {
            if (WConfig.cfgGameplay.Value)
            {
                PrismaticTrial.AllowPrismaticTrials();

                On.EntityStates.LunarTeleporter.LunarTeleporterBaseState.FixedUpdate += LunarTeleporterBaseState_FixedUpdate;

            }
            if (WConfig.ItemsTeleport.Value)
            {
                GameObject PickupDroplet = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Common/PickupDroplet.prefab").WaitForCompletion();
                PickupDroplet.AddComponent<ItemOrbPhysicsChanger>();
                On.RoR2.MapZone.TryZoneStart += TeleportItems;
            }
            GeyserLock.Start();

        }

        public static bool IsItem(Collider collider)
        {
            if (collider.GetComponent<PickupDropletController>()) return true;
            if (collider.GetComponent<GenericPickupController>()) return true;
            if (collider.GetComponent<PickupPickerController>()) return true;
            return false;
        }

        public static void TeleportItems(On.RoR2.MapZone.orig_TryZoneStart orig, MapZone self, Collider collider)
        {
            orig(self, collider);
            if (self.zoneType == MapZone.ZoneType.OutOfBounds)
            {
                if (IsItem(collider))
                {
                    GameObject teleportEffectPrefab = Run.instance.GetTeleportEffectPrefab(collider.gameObject);
                    InfiniteTowerRun itRun = Run.instance.GetComponent<InfiniteTowerRun>();
                    if (itRun && itRun.safeWardController)
                    {
                        Debug.Log("it tp item back");
                        TeleportHelper.TeleportGameObject(collider.gameObject, itRun.safeWardController.transform.position);
                        if (teleportEffectPrefab)
                        {
                            EffectManager.SimpleEffect(teleportEffectPrefab, itRun.safeWardController.transform.position, Quaternion.identity, true);
                        }
                    }
                    else
                    {
                        SpawnCard spawnCard = ScriptableObject.CreateInstance<SpawnCard>();
                        spawnCard.hullSize = HullClassification.Human;
                        spawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Ground;
                        spawnCard.prefab = LegacyResourcesAPI.Load<GameObject>("SpawnCards/HelperPrefab");

                        DirectorPlacementRule placementRule = new DirectorPlacementRule
                        {
                            placementMode = DirectorPlacementRule.PlacementMode.NearestNode,
                            position = collider.transform.position
                        };

                        GameObject gameObject = DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(spawnCard, placementRule, RoR2Application.rng));
                        if (gameObject)
                        {
                            //Debug.Log("tp item back");
                            TeleportHelper.TeleportGameObject(collider.gameObject, gameObject.transform.position);
                            if (teleportEffectPrefab)
                            {
                                EffectManager.SimpleEffect(teleportEffectPrefab, gameObject.transform.position, Quaternion.identity, true);
                            }
                            UnityEngine.Object.Destroy(gameObject);
                        }
                        UnityEngine.Object.Destroy(spawnCard);
                    }
                }
            }
        }


        public static void LunarTeleporterBaseState_FixedUpdate(On.EntityStates.LunarTeleporter.LunarTeleporterBaseState.orig_FixedUpdate orig, EntityStates.LunarTeleporter.LunarTeleporterBaseState self)
        {
            self.fixedAge += self.GetDeltaTime();
            if (NetworkServer.active)
            {
                if (TeleporterInteraction.instance)
                {
                    if (TeleporterInteraction.instance.isInFinalSequence)
                    {
                        self.genericInteraction.Networkinteractability = Interactability.Disabled;
                        return;
                    }
                    self.genericInteraction.Networkinteractability = self.preferredInteractability;
                }
            }
        }

    }


}
