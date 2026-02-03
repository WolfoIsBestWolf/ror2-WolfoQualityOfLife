using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WQoL_Gameplay
{
    public class ItemOrbPhysicsChanger : MonoBehaviour
    {
        public float forceTeleportY;
        public Rigidbody body;
        public void Awake()
        {
            //Force teleport if lower than this Y coord ig
            //But still use map bounds otherwise.
            forceTeleportY = (transform.position.y - 150);
            body = this.GetComponent<Rigidbody>();
            if (body && body.velocity.y < -30)
            {
                body = null;
                this.gameObject.layer = 8;
            }
        }
        public void FixedUpdate()
        {

            if (body && body.velocity.y < -30)
            {
                body = null;
                this.gameObject.layer = 8;
                //Destroy(this);
            }
            else if (forceTeleportY > transform.position.y)
            {
                GameplayQualityOfLife.TeleportItem(this.gameObject);
            }
        }
    }


    public class GameplayQualityOfLife
    {
        public static void Start()
        {

            RedWhip.BetterRedWhipCheck();
            On.EntityStates.LunarTeleporter.LunarTeleporterBaseState.FixedUpdate += LunarTeleporterBaseState_FixedUpdate;

            if (WConfig.ItemsTeleport.Value)
            {
                Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Common/PickupDroplet.prefab").WaitForCompletion().AddComponent<ItemOrbPhysicsChanger>();

                //Change Command Cube phys layer so it collides with MapBoundries
                //Other 2 option pickups too
                //Also pushes itself now, kinda whatever maybe even good
                Addressables.LoadAssetAsync<GameObject>(key: "d9c642aeefb6c974da664933489071d4").WaitForCompletion().layer = LayerIndex.fakeActor.intVal;
                Addressables.LoadAssetAsync<GameObject>(key: "f8e3413a378bd7c44aa09bed0020eaf5").WaitForCompletion().layer = LayerIndex.fakeActor.intVal;
                Addressables.LoadAssetAsync<GameObject>(key: "cda6217a2c0775c42a3f456b3bb5e073").WaitForCompletion().layer = LayerIndex.fakeActor.intVal;

                On.RoR2.MapZone.TryZoneStart += TeleportItems;
                On.EntityStates.Drone.DeathState.OnEnter += TeleportBrokenDronesUp;
            }
 
            SceneDirector.onGenerateInteractableCardSelection += MORESCANNERS;


            //More compact drop angle
            LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/LunarCauldron, RedToWhite Variant").GetComponent<ShopTerminalBehavior>().dropVelocity = new Vector3(5, 10, 5);
            //STOP STEALING ME FUKIN MONEY
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/PM DestinationPortal.prefab").WaitForCompletion().GetComponent<ConvertPlayerMoneyToExperience>().enabled = false;

            if (WConfig.cfgLava.Value)
            {
                IL.RoR2.CharacterBody.InflictLavaDamage += ConsistentLavaDamageForAllies;
            }

            On.PowerOrbKeySpawner.SpawnKey += PowerOrbKeySpawner_SpawnKey;


            //Move this somewhere
            On.EntityStates.Croco.Spawn.OnEnter += (orig, self) =>
            {
                orig(self);
                if (NetworkServer.active)
                {
                    self.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
                }
            };
            On.EntityStates.Croco.Spawn.OnExit += (orig, self) =>
            {
                orig(self);
                if (NetworkServer.active)
                {
                    self.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
                    self.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 3f);
                }
            };  

        }

        private static void PowerOrbKeySpawner_SpawnKey(On.PowerOrbKeySpawner.orig_SpawnKey orig, PowerOrbKeySpawner self, DamageReport damageReport)
        {
            if (damageReport.victimMaster && damageReport.victimMaster.killedByUnsafeArea)
            {
                return;
            }
            orig(self, damageReport);
        }

        private static void ConsistentLavaDamageForAllies(ILContext il)
        {
            //10% max hp lava damage seems intended for players only
            //Not allies like Devoted Lems
            //May-be a bit overstepping but whatever

            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchCallvirt("RoR2.TeamComponent", "get_teamIndex")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<TeamIndex, CharacterBody, TeamIndex>>((team, body) =>
                {
                    if (team == TeamIndex.Player)
                    {
                        if (!body.isPlayerControlled)
                        {
                            return TeamIndex.Monster;
                        }
                    }
                    return team;
                });
            }
            else
            {
                WGQoLMain.log.LogWarning("IL Failed : ConsistentLavaDamageForAllies");
            }
        }


        public static void MORESCANNERS(SceneDirector scene, DirectorCardCategorySelection dccs)
        {
            int rare = dccs.FindCategoryIndexByName("Rare");
            if (rare != -1)
            {
                var a = dccs.categories[rare];
                for (int i = 0; i < a.cards.Length; i++)
                {
                    if (a.cards[i].forbiddenUnlockableDef)
                    {
                        a.cards[i].selectionWeight = 50;
                    }
                }
            }

        }

        private static void TeleportBrokenDronesUp(On.EntityStates.Drone.DeathState.orig_OnEnter orig, EntityStates.Drone.DeathState self)
        {
            orig(self);
            self.gameObject.layer = LayerIndex.playerBody.intVal;
            self.deathDuration *= 4; //Make sure they "die" long enough to fall into a tp zone if possible
        }

        public static bool IsItem(Collider collider)
        {
            if (collider.GetComponent<PickupDropletController>()) return true;
            if (collider.GetComponent<GenericPickupController>()) return true;
            if (collider.GetComponent<PickupPickerController>()) return true;
            return false;
        }


        public static void TeleportItem(GameObject item)
        {
            GameObject teleportEffectPrefab = Run.instance.GetTeleportEffectPrefab(item);
            InfiniteTowerRun itRun = Run.instance.GetComponent<InfiniteTowerRun>();
            if (itRun && itRun.safeWardController)
            {
                WGQoLMain.log.LogMessage("it tp item back");
                TeleportHelper.TeleportGameObject(item, itRun.safeWardController.transform.position);
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
                    position = item.transform.position
                };

                GameObject gameObject = DirectorCore.instance.TrySpawnObject(new DirectorSpawnRequest(spawnCard, placementRule, RoR2Application.rng));
                if (gameObject)
                {
                    //WolfoMain.log.LogMessage("tp item back");
                    TeleportHelper.TeleportGameObject(item, gameObject.transform.position);
                    if (teleportEffectPrefab)
                    {
                        EffectManager.SimpleEffect(teleportEffectPrefab, gameObject.transform.position, Quaternion.identity, true);
                    }
                    UnityEngine.Object.Destroy(gameObject);
                }
                UnityEngine.Object.Destroy(spawnCard);
                GameObject.Destroy(item.GetComponent<ItemOrbPhysicsChanger>());
            }
        }

        public static void TeleportItems(On.RoR2.MapZone.orig_TryZoneStart orig, MapZone self, Collider collider)
        {
            orig(self, collider);
            if (self.zoneType == MapZone.ZoneType.OutOfBounds)
            {
                if (IsItem(collider))
                {
                    TeleportItem(collider.gameObject);
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
                    if (TeleporterInteraction.instance.isCharging || TeleporterInteraction.instance.isInFinalSequence)
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
