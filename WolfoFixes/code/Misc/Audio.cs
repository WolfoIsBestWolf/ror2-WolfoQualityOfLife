using EntityStates.Engi.SpiderMine;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{
    internal class Audio
    {

        public static void UnusedSounds()
        {

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
                Util.PlaySound("Play_item_lunar_specialReplace_apply", self.outer.gameObject);
            };
        }


        public static void Start()
        {
            UnusedSounds();

            //Would be nice to sync sound only to spawn if it has missiles
            IL.RoR2.BarrageOnBossBehaviour.StartMissileCountdown += WarBondsNoise_OnlyIfActuallyMissile;

            //Fix Void Orb sound
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


            On.RoR2.WwiseUtils.SoundbankLoader.Start += LoadMulTSoundsForScrapper;

            On.EntityStates.Engi.SpiderMine.Detonate.OnEnter += FixClientNoiseSpam;


            //Spawn sound not actually set?
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBoss/RoboBallMiniBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallRedBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/RoboBallBuddy/RoboBallGreenBuddyBody.prefab").WaitForCompletion().GetComponent<SfxLocator>().aliveLoopStart = "Play_roboBall_attack2_mini_spawn";

            //Syringe Impact sound somehow gone
            var REX_m1 = Addressables.LoadAssetAsync<GameObject>(key: "8fa98ca49d9137045a738929c63567c7").WaitForCompletion().AddComponent<PlaySoundOnEvent>();
            REX_m1.soundEvent = "Play_treeBot_m1_impact";
            REX_m1.triggeringEvent = PlaySoundOnEvent.PlaySoundEvent.Destroy;

            REX_m1 = Addressables.LoadAssetAsync<GameObject>(key: "1e58dd828d88f5d46bd5ac5a33bbd589").WaitForCompletion().AddComponent<PlaySoundOnEvent>();
            REX_m1.soundEvent = "Play_treeBot_m1_impact";
            REX_m1.triggeringEvent = PlaySoundOnEvent.PlaySoundEvent.Destroy;
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


        private static void LoadMulTSoundsForScrapper(On.RoR2.WwiseUtils.SoundbankLoader.orig_Start orig, RoR2.WwiseUtils.SoundbankLoader self)
        {
            HG.ArrayUtils.ArrayAppend(ref self.soundbankStrings, "char_Toolbot");
            HG.ArrayUtils.ArrayAppend(ref self.soundbankStrings, "Boss_FalseSon");
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