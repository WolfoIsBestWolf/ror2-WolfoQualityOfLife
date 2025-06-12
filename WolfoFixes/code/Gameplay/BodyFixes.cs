using MonoMod.Cil;
using Mono.Cecil.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using EntityStates.Fauna;
using HarmonyLib;
using EntityStates.EngiTurret.EngiTurretWeapon;

namespace WolfoFixes
{

    public class MithrixPhase4Fix : MonoBehaviour
    {
        public void Start()
        {
            //-> PreAwake Fix Hurtable in body prefab
            //-> Awake does his thing
            //-> Start, we need to turn them on again if he's not doing the animation.
            HurtBoxGroup component = this.GetComponent<HurtBoxGroup>();
            if (component)
            {
                //Debug.Log(component.hurtBoxesDeactivatorCounter);
                if (component.hurtBoxesDeactivatorCounter == 0)
                {
                    component.SetHurtboxesActive(true);
                }
            }
        }
    }

    public class BodyFixes
    {
        public static void Start()
        {


            On.EntityStates.Merc.WhirlwindBase.OnEnter += WhirlwindBase_OnEnter;

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


            if (WConfig.cfgMithrix4Skip.Value)
            {
                //Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/BrotherBody.prefab").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags &= ~CharacterBody.BodyFlags.IgnoresRecordDeathEvent;
                GameObject BrotherHurtBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/BrotherHurtBody.prefab").WaitForCompletion();
                HurtBoxGroup component = BrotherHurtBody.GetComponentInChildren<HurtBoxGroup>();
                component.gameObject.AddComponent<MithrixPhase4Fix>();
                if (component)
                {
                    component.hurtBoxesDeactivatorCounter = 1;
                }
            }
           

            GameObject ChildBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/Child/ChildBody.prefab").WaitForCompletion();
            CharacterBody Child = ChildBody.GetComponent<CharacterBody>();
            if (!Child.bodyFlags.HasFlag(CharacterBody.BodyFlags.OverheatImmune))
            {
                Child.bodyFlags |= CharacterBody.BodyFlags.OverheatImmune;
            }



            //For testing ig but also it spams the console
            IL.EntityStates.Commando.CommandoWeapon.FirePistol2.FixedUpdate += CommandoReloadStateRemove;
            //Huntress issue only starts at 780% attack speed who cares really

            //Boosted Icebox fix
            Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "6870bda0b12690048a9701539d1e2285").WaitForCompletion().activationState = Addressables.LoadAssetAsync<RoR2.Skills.SkillDef>(key: "c97062b172b41af4ebdb42c312ac1989").WaitForCompletion().activationState;
            On.EntityStates.Chef.IceBox.OnEnter += IceBox_OnEnter;

            On.RoR2.Projectile.CleaverProjectile.ChargeCleaver += CleaverProjectile_ChargeCleaver;


            //Mushroom tree do not produce fruit or die or whatever 
            On.EntityStates.Fauna.HabitatFruitDeathState.OnEnter += FixDumbFruit;
            
            HabitatFruitDeathState.deathSoundString = "Play_jellyfish_death";
            HabitatFruitDeathState.healPackMaxVelocity = 60;
            HabitatFruitDeathState.fractionalHealing = 0.15f;
            HabitatFruitDeathState.scale = 1;


            On.EntityStates.MajorConstruct.Weapon.FireLaser.OnExit += XI_LaserFix;

            //Addressables.LoadAssetAsync<GameObject>(key: "cdbb41712e896454da142ab00d046d9f").WaitForCompletion().GetComponents<RoR2.CharacterAI.AISkillDriver>()[2].requiredSkill = null;

        }

        private static void XI_LaserFix(On.EntityStates.MajorConstruct.Weapon.FireLaser.orig_OnExit orig, EntityStates.MajorConstruct.Weapon.FireLaser self)
        {
            orig(self);
            self.outer.SetNextState(self.GetNextState()); 
        }



        //Stolen from miscFixes because they decided to remove it
        //Didnt ask
        [HarmonyPatch(typeof(FireBeam), nameof(FireBeam.FixedUpdate))]
        [HarmonyILManipulator]
        public static void FireBeam_FixedUpdate(ILContext il)
        {
            var c = new ILCursor(il);
            // Using ShouldFireLaser as a landmark juuuust in case there are ever multiple SetNextStateToMain calls
            if (c.TryGotoNext(x => x.MatchCallOrCallvirt<FireBeam>(nameof(FireBeam.ShouldFireLaser))) &&
                c.TryGotoNext(x => x.MatchCallOrCallvirt<EntityStateMachine>(nameof(EntityStateMachine.SetNextStateToMain))))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.Emit(OpCodes.Callvirt, AccessTools.Method(typeof(FireBeam), nameof(FireBeam.GetNextState)));
                c.Next.Operand = AccessTools.Method(typeof(EntityStateMachine), nameof(EntityStateMachine.SetNextState));
            }
            else Debug.LogWarning(il);
        }

        public static GameObject JellyfishDeath;
        private static void FixDumbFruit(On.EntityStates.Fauna.HabitatFruitDeathState.orig_OnEnter orig, EntityStates.Fauna.HabitatFruitDeathState self)
        { 
            Transform Fruit = self.gameObject.transform.GetChild(1).GetChild(3);
            EffectManager.SimpleImpactEffect(Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Jellyfish/JellyfishDeath.prefab").WaitForCompletion(), Fruit.position, Vector3.up, false);
            if (NetworkServer.active)
            {
                
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/HealPack"), Fruit.position, UnityEngine.Random.rotation);
                gameObject.GetComponent<TeamFilter>().teamIndex = TeamIndex.Player;
                gameObject.GetComponentInChildren<HealthPickup>().fractionalHealing = HabitatFruitDeathState.fractionalHealing;
                gameObject.transform.localScale = new Vector3(HabitatFruitDeathState.scale, HabitatFruitDeathState.scale, HabitatFruitDeathState.scale);
                gameObject.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere * HabitatFruitDeathState.healPackMaxVelocity, ForceMode.VelocityChange);
                NetworkServer.Spawn(gameObject);
            }
            orig(self);
        }


        private static void IceBox_OnEnter(On.EntityStates.Chef.IceBox.orig_OnEnter orig, EntityStates.Chef.IceBox self)
        {
            orig(self);
            if (self.hasBoost)
            {
                self.attackSoundString = "Play_Chef_Secondary_IceBox_Boosted_Fire";
                Util.PlaySound(self.blizzardSFXString, self.gameObject);
            }
        }

        private static void CleaverProjectile_ChargeCleaver(On.RoR2.Projectile.CleaverProjectile.orig_ChargeCleaver orig, RoR2.Projectile.CleaverProjectile self)
        {
            orig(self);
            if (self.charged)
            {
                self.projectileOverlapAttack.overlapProcCoefficient = 1f;
            }
        }


        public static void CommandoReloadStateRemove(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchCallvirt("RoR2.GenericSkill", "get_stock")))
            {
                c.EmitDelegate<System.Func<int, int>>((stock) =>
                {
                    return 1;
                });
            }
            else
            {
                Debug.LogWarning("CommandoReloadStateRemove Failed");
            }
        }


        private static void WhirlwindBase_OnEnter(On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, EntityStates.Merc.WhirlwindBase self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                self.characterBody.AddTimedBuff(JunkContent.Buffs.IgnoreFallDamage, self.duration);
            }
        }
    }


}
