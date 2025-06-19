using MonoMod.Cil;
using Mono.Cecil.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using EntityStates.Fauna;
using HarmonyLib;
using EntityStates.EngiTurret.EngiTurretWeapon;
using RoR2.Projectile;
using EntityStates;
 
namespace WolfoFixes
{
    public class MithrixPhase4Fix : MonoBehaviour
    {
        public bool stoleItems = false;
        public void Start()
        {
            //-> PreAwake Fix Hurtable in body prefab
            //-> Awake does his thing
            //-> Start, we need to turn them on again if he's not doing the animation.
            SetStateOnHurt component = this.GetComponent<SetStateOnHurt>();
            if (component)
            {
                component.canBeHitStunned = true;
            }
            HurtBoxGroup hurtBoxGroup = this.GetComponent<HurtBoxGroup>();
            if (hurtBoxGroup)
            {
                //Debug.Log(component.hurtBoxesDeactivatorCounter);
                if (hurtBoxGroup.hurtBoxesDeactivatorCounter == 0)
                {
                    hurtBoxGroup.SetHurtboxesActive(true);
                }
            }
        }
    }
    public class BodyFixes
    {
        public static void SetSkippable(object sender, System.EventArgs e)
        {
            GameObject BrotherHurtBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/BrotherHurtBody.prefab").WaitForCompletion();
            SetStateOnHurt component = BrotherHurtBody.GetComponent<SetStateOnHurt>();
            if (component)
            {
                component.canBeHitStunned = !WConfig.cfgMithrix4Skip.Value;
            }
            if (BrotherHurtBody.GetComponent<MithrixPhase4Fix>() == null)
            {
                component.gameObject.AddComponent<MithrixPhase4Fix>();
            }


            /*HurtBoxGroup hurtBoxGroup = BrotherHurtBody.GetComponentInChildren<HurtBoxGroup>();
               hurtBoxGroup.gameObject.AddComponent<MithrixPhase4Fix>();
               if (hurtBoxGroup)
               {
                   hurtBoxGroup.SetHurtboxesActive(false);
               }*/

        }

        public static void Start()
        {
            SetSkippable(null, null);
 
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

            IL.EntityStates.CaptainSupplyDrop.HitGroundState.OnEnter += FixCaptainBeaconNoCrit;

            On.EntityStates.Chef.OilSpillBase.OnExit += OilSpillBase_OnExit;
 
            if (WConfig.cfgFunnyIceSpear.Value)
            {
                //Ice Spear wrong phys layer
                Addressables.LoadAssetAsync<GameObject>(key: "7a5eecba2b015474dbed965c120860d0").WaitForCompletion().layer = 8;

            }

        }
  
        public static void CallLate()
        {
            //Fix Back-Up drones not scaling with level
            Addressables.LoadAssetAsync<GameObject>(key: "3a44327eee358a74ba0580dbca78897e").WaitForCompletion().AddComponent<GivePickupsOnStart>().itemDefInfos = new GivePickupsOnStart.ItemDefInfo[]
            {
                new GivePickupsOnStart.ItemDefInfo
                {
                    itemDef = RoR2Content.Items.UseAmbientLevel,
                    count = 1,
                }
            };
        }

        private static void OilSpillBase_OnExit(On.EntityStates.Chef.OilSpillBase.orig_OnExit orig, EntityStates.Chef.OilSpillBase self)
        {
            //In case gets cancelled
            self.characterBody.AddTimedBuff(JunkContent.Buffs.IgnoreFallDamage, 0.66f);
            orig(self);

        }
 
        private static void FixCaptainBeaconNoCrit(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchStfld("RoR2.BulletAttack", "isCrit")))
            {
                c.Emit(OpCodes.Ldloc_1);
                c.EmitDelegate<System.Func<bool, ProjectileDamage, bool>>((a,projectileDamage) =>
                {
                    return projectileDamage.crit;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : FixCaptainBeaconNoCrit");
            }
        }

        private static void XI_LaserFix(On.EntityStates.MajorConstruct.Weapon.FireLaser.orig_OnExit orig, EntityStates.MajorConstruct.Weapon.FireLaser self)
        {
            orig(self);
            self.outer.SetNextState(self.GetNextState()); 
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
                Debug.LogWarning("IL Failed : CommandoReloadStateRemove");
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
