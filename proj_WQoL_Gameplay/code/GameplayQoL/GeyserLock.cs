using MonoMod.Cil;
using RoR2;
using WQoL_Gameplay;

namespace WQoL_Gameplay
{
    public class GeyserLock
    {
        public static void Start()
        {

            IL.RoR2.CharacterBody.TriggerJumpEventGlobally += TriggerJumpEventGlobally;
            //On.RoR2.CharacterBody.OnSkillActivated += AllUtilitiesRemoveGeyserLock;

            On.EntityStates.Commando.DodgeState.OnEnter += DodgeState_OnEnter;
            On.EntityStates.Commando.SlideState.OnEnter += SlideState_OnEnter; //Alt

            On.EntityStates.Loader.BaseChargeFist.OnEnter += BaseChargeFist_OnEnter;

            On.EntityStates.Merc.Assaulter2.OnExit += Assaulter2_OnExit;
            On.EntityStates.Merc.FocusedAssaultDash.OnExit += FocusedAssaultDash_OnExit;
            On.EntityStates.Merc.EvisDash.OnExit += EvisDash_OnExit;

            On.EntityStates.Mage.FlyUpState.OnEnter += FlyUpState_OnEnter; //Ion

            On.EntityStates.VoidSurvivor.VoidBlinkBase.OnEnter += VoidBlinkBase_OnEnter;

            On.EntityStates.Chef.OilSpillBase.OnEnter += OilSpillBase_OnEnter;

            On.EntityStates.FalseSon.StepBrothers.OnEnter += StepBrothers_OnEnter;

            On.EntityStates.GhostUtilitySkillState.OnEnter += GhostUtilitySkillState_OnEnter;
        }

        private static void EvisDash_OnExit(On.EntityStates.Merc.EvisDash.orig_OnExit orig, EntityStates.Merc.EvisDash self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void Assaulter2_OnExit(On.EntityStates.Merc.Assaulter2.orig_OnExit orig, EntityStates.Merc.Assaulter2 self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void FocusedAssaultDash_OnExit(On.EntityStates.Merc.FocusedAssaultDash.orig_OnExit orig, EntityStates.Merc.FocusedAssaultDash self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void StepBrothers_OnEnter(On.EntityStates.FalseSon.StepBrothers.orig_OnEnter orig, EntityStates.FalseSon.StepBrothers self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void OilSpillBase_OnEnter(On.EntityStates.Chef.OilSpillBase.orig_OnEnter orig, EntityStates.Chef.OilSpillBase self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void VoidBlinkBase_OnEnter(On.EntityStates.VoidSurvivor.VoidBlinkBase.orig_OnEnter orig, EntityStates.VoidSurvivor.VoidBlinkBase self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void BaseChargeFist_OnEnter(On.EntityStates.Loader.BaseChargeFist.orig_OnEnter orig, EntityStates.Loader.BaseChargeFist self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void FlyUpState_OnEnter(On.EntityStates.Mage.FlyUpState.orig_OnEnter orig, EntityStates.Mage.FlyUpState self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void DodgeState_OnEnter(On.EntityStates.Commando.DodgeState.orig_OnEnter orig, EntityStates.Commando.DodgeState self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void GhostUtilitySkillState_OnEnter(On.EntityStates.GhostUtilitySkillState.orig_OnEnter orig, EntityStates.GhostUtilitySkillState self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void SlideState_OnEnter(On.EntityStates.Commando.SlideState.orig_OnEnter orig, EntityStates.Commando.SlideState self)
        {
            orig(self);
            self.characterMotor.disableAirControlUntilCollision = false;
        }

        private static void TriggerJumpEventGlobally(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
               x => x.MatchLdfld("RoR2.CharacterBody", "onJump")
               ))
            {
                c.EmitDelegate<System.Func<CharacterBody, CharacterBody>>((body) =>
                {
                    /*if (WConfig.CancelGeyserLock_Jump.Value)
                    {
                        body.characterMotor.disableAirControlUntilCollision = false;
                    }*/
                    return body;
                });
            }
            else
            {
                WGQoLMain.log.LogWarning("IL Failed: TriggerJumpEventGlobally");
            }

        }

        public static void AllUtilitiesRemoveGeyserLock(On.RoR2.CharacterBody.orig_OnSkillActivated orig, CharacterBody self, GenericSkill skill)
        {
            orig(self, skill);
            if (self.skillLocator.utility == skill)
            {
                if (self.characterMotor)
                {
                    self.characterMotor.disableAirControlUntilCollision = false;
                }
            }
        }

    }


}
