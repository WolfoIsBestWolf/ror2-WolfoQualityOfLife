using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

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



            //Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/BrotherBody.prefab").WaitForCompletion().GetComponent<CharacterBody>().bodyFlags &= ~CharacterBody.BodyFlags.IgnoresRecordDeathEvent;
            GameObject BrotherHurtBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/BrotherHurtBody.prefab").WaitForCompletion();
            HurtBoxGroup component = BrotherHurtBody.GetComponentInChildren<HurtBoxGroup>();
            component.gameObject.AddComponent<MithrixPhase4Fix>();
            if (component)
            {
                component.hurtBoxesDeactivatorCounter = 1;
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
