using RoR2;
using UnityEngine.AddressableAssets;

using UnityEngine;

//using System;
using UnityEngine.Networking;
using System.ComponentModel;

namespace WolfoQoL_Client
{

    public class MithrixPhase4Fix : MonoBehaviour
    {
        //If he revives hitboxes aren't turned on again so we have to do it
        public void Start()
        {
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

            
            GameObject BrotherHurtBody = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Brother/BrotherHurtBody.prefab").WaitForCompletion();
            HurtBoxGroup component = BrotherHurtBody.GetComponentInChildren<HurtBoxGroup>();
            component.gameObject.AddComponent<MithrixPhase4Fix>();
            if (component)
            {
                component.hurtBoxesDeactivatorCounter = 1;
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
