using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using RoR2.Skills;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using UnityEngine.Networking;
using EntityStates.Engi.SpiderMine;

namespace WolfoFixes
{
    public class Audio
    {


        public static void Start()
        {

            //Would be nice to sync sound only to spawn if it has missiles
            IL.RoR2.BarrageOnBossBehaviour.StartMissileCountdown += WarBondsNoise_OnlyIfActuallyMissile;
            //Steal this
            GameObject VoidOrb = LegacyResourcesAPI.Load<GameObject>("Prefabs/itempickups/VoidOrb");
            foreach (AkEvent a in VoidOrb.GetComponents<AkEvent>())
            {
                GameObject.Destroy(a);
            }
            GameObject.Destroy(VoidOrb.GetComponent<AkGameObj>());
            PlaySoundOnEvent sound = VoidOrb.AddComponent<PlaySoundOnEvent>();
            sound.triggeringEvent = PlaySoundOnEvent.PlaySoundEvent.Start;
            sound.soundEvent = "Play_UI_item_spawn_tier3";
            sound = VoidOrb.AddComponent<PlaySoundOnEvent>();
            sound.triggeringEvent = PlaySoundOnEvent.PlaySoundEvent.Destroy;
            sound.soundEvent = "Play_nullifier_death_vortex_explode";

            On.RoR2.WwiseUtils.SoundbankLoader.Start += SoundbankLoader_Start;

            On.EntityStates.Engi.SpiderMine.Detonate.OnEnter += FixClientNoiseSpam;


    
            //Add unused cool bubbly noise
            On.RoR2.ShopTerminalBehavior.PreStartClient += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("LunarCauldron,") || self.name.StartsWith("ShrineCleanse"))
                {
                    Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                    Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                    Util.PlaySound("Play_ui_obj_lunarPool_idle_loop", self.gameObject);
                }
            };
            On.RoR2.ShopTerminalBehavior.DropPickup += (orig, self) =>
            {
                orig(self);
                if (self.name.StartsWith("LunarCauldron") || self.name.StartsWith("ShrineCleanse"))
                {
                    Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                    Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                    Util.PlaySound("Play_ui_obj_lunarPool_activate", self.gameObject);
                };
            };

            //Unused Scav Spawn Sound
            On.EntityStates.ScavMonster.Sit.OnEnter += (orig, self) =>
            {
                orig(self);
                if (!self.outer.gameObject) { return; }
                Util.PlaySound(EntityStates.ScavMonster.Sit.soundString, self.outer.gameObject);
            };


            //Pretty sure they added these sounds at some point but keeping it for good measure I guess
            On.EntityStates.GlobalSkills.LunarDetonator.Detonate.OnEnter += (orig, self) =>
            {
                orig(self);
                RoR2.Util.PlaySound("Play_item_lunar_specialReplace_apply", self.outer.gameObject);
            };
 
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBoss/RoboBallMiniBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallRedBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallGreenBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";


            //Sound is just too quiet
            On.RoR2.CharacterMaster.PlayExtraLifeSFX += (orig, self) =>
            {
                orig(self);

                GameObject bodyInstanceObject = self.GetBodyObject();
                if (bodyInstanceObject)
                {
                    Util.PlaySound("Play_item_proc_extraLife", bodyInstanceObject);
                    Util.PlaySound("Play_item_proc_extraLife", bodyInstanceObject);
                }
            };
     
        }


        private static void FixClientNoiseSpam(On.EntityStates.Engi.SpiderMine.Detonate.orig_OnEnter orig, EntityStates.Engi.SpiderMine.Detonate self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                float duration = 0.25f;
                self.PlayAnimation("Base", BaseSpiderMineState.IdleToArmedStateHash, BaseSpiderMineState.IdleToArmedParamHash, duration);
                foreach (string childName in self.childLocatorStringToDisable)
                {
                    Transform transform = self.FindModelChild(childName);
                    if (transform)
                    {
                        transform.gameObject.SetActive(false);
                    }
                }
            }
        }


        private static void SoundbankLoader_Start(On.RoR2.WwiseUtils.SoundbankLoader.orig_Start orig, RoR2.WwiseUtils.SoundbankLoader self)
        {
            HG.ArrayUtils.ArrayAppend(ref self.soundbankStrings, "char_Toolbot");
            orig(self);
        }


        private static void WarBondsNoise_OnlyIfActuallyMissile(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("RoR2.BarrageOnBossBehaviour", "missileStartPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<GameObject, BarrageOnBossBehaviour, GameObject>>((effect, self) =>
                {
                    if (self.barrageQuantity == 0)
                    {
                        return null;
                    }
                    return effect;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: WarBondsNoise_OnlyIfActuallyMissile");
            }
        }





    }
 
}