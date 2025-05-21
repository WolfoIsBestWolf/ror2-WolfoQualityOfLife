using RoR2;
using UnityEngine.AddressableAssets;

using UnityEngine;

//using System;
using UnityEngine.Networking;

namespace WolfoQoL_Client
{
    public class GameplayQualityOfLife
    {
        public static void Start()
        {
            if (!WConfig.cfgGameplay.Value)
            {
                return;
            }
            PrismaticTrial.AllowPrismaticTrials();
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

            On.EntityStates.LunarTeleporter.LunarTeleporterBaseState.FixedUpdate += LunarTeleporterBaseState_FixedUpdate;

            On.EntityStates.MajorConstruct.Weapon.FireLaser.OnExit += XI_LaserFix;

            GameObject BrotherHurtBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/BrotherHurtBody.prefab").WaitForCompletion();
            HurtBoxGroup component = BrotherHurtBody.GetComponentInChildren<HurtBoxGroup>();
            if (component)
            {
                component.hurtBoxesDeactivatorCounter = 1;
            }
        }

        private static void XI_LaserFix(On.EntityStates.MajorConstruct.Weapon.FireLaser.orig_OnExit orig, EntityStates.MajorConstruct.Weapon.FireLaser self)
        {
            orig(self);
            self.outer.SetNextState(self.GetNextState());
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

        private static void WhirlwindBase_OnEnter(On.EntityStates.Merc.WhirlwindBase.orig_OnEnter orig, EntityStates.Merc.WhirlwindBase self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                self.characterBody.AddTimedBuff(JunkContent.Buffs.IgnoreFallDamage, 0.3f, 1);
            }
        }
    }


}
