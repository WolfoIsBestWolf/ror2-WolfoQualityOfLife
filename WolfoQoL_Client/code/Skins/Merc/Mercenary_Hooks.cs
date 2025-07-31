using EntityStates.Merc;
using EntityStates.Merc.Weapon;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;

namespace WolfoQoL_Client.Skins
{
    public class Mercenary_Hooks
    {

        //Since R2API added a skin dependent effects replacer, maybe we should just go with that nowadays



        public static void Hooks()
        {

            //Primary
            On.EntityStates.Merc.Weapon.GroundLight2.OnEnter += GroundLight2_OnEnter;
            IL.EntityStates.Merc.Weapon.GroundLight2.OnEnter += Finisher_GroundLight2_OnEnter;

            //Secondary
            IL.EntityStates.Merc.WhirlwindBase.OnEnter += WhirlwindBase_OnEnter;
            IL.EntityStates.Merc.WhirlwindBase.FixedUpdate += WhirlwindBase_FixedUpdate;

            //Alt Secondary
            IL.EntityStates.Merc.Uppercut.OnEnter += Uppercut_OnEnter;
            IL.EntityStates.Merc.Uppercut.FixedUpdate += Uppercut_FixedUpdate;

            //Utility 
            IL.EntityStates.Merc.Assaulter2.OnEnter += Assaulter2_OnEnter;
            On.EntityStates.Merc.Assaulter2.OnEnter += Assaulter2_OnEnter_On;

            //Utility Alt
            On.EntityStates.Merc.FocusedAssaultDash.OnEnter += AltUtility_OnEnter;

            //Special Dash
            IL.EntityStates.Merc.EvisDash.FixedUpdate += Special_Dash; //Hopefully works better
            IL.EntityStates.Merc.EvisDash.CreateBlinkEffect += EvisDash_CreateBlinkEffect;

            //Special Evis
            IL.EntityStates.Merc.Evis.CreateBlinkEffect += MercRed_SpecialEvis;
            IL.EntityStates.Merc.Evis.FixedUpdate += Evis_FixedUpdate;
            IL.EntityStates.Merc.Evis.OnExit += Evis_OnExit;

            //Special Alt
            On.EntityStates.Merc.Weapon.ThrowEvisProjectile.OnEnter += AltSpecial_OnEnter;

        }

        private static void EvisDash_CreateBlinkEffect(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                   x => x.MatchLdsfld("EntityStates.Merc.EvisDash", "blinkPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EvisDash, GameObject>>((target, entityState) =>
                {
                    EffectReplacer.ActivateMerc(EvisDash.blinkPrefab, entityState.gameObject);
                    return target;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: Evis.CreateBlinkEffect");
            }
        }


        private static void AltSpecial_OnEnter(On.EntityStates.Merc.Weapon.ThrowEvisProjectile.orig_OnEnter orig, EntityStates.Merc.Weapon.ThrowEvisProjectile self)
        {
            self.effectPrefab = null; //It can't spawn this because it's not an effect for both blue&red
            SetExposeColor(self.gameObject);
            orig(self);
        }

        private static void AltUtility_OnEnter(On.EntityStates.Merc.FocusedAssaultDash.orig_OnEnter orig, EntityStates.Merc.FocusedAssaultDash self)
        {
            int i = SkinChanges.DetectCaseMerc(self.gameObject);
            EffectReplacer.Activate(self.delayedEffectPrefab, i);
            EffectReplacer.Activate(self.hitEffectPrefab, i);
            //EffectReplacer.Activate(self.selfOnHitOverlayEffectPrefab, i);
            EffectReplacer.Activate(self.orbEffect, i);
            switch (i)
            {
                case 1:
                    self.swingEffectPrefab = Merc_Red.MercAssaulterEffect_Red;
                    self.enterOverlayMaterial = Merc_Red.matMercEnergized_Red;
                    break;
                case 2:
                    self.swingEffectPrefab = Merc_Green.MercAssaulterEffect_Green;
                    self.enterOverlayMaterial = Merc_Green.matMercEnergized_Green;
                    break;
            }
            SetExposeColor(i);
            orig(self);
        }

        public static void SetExposeColor(GameObject gameObject)
        {
            SetExposeColor(SkinChanges.DetectCaseMerc(gameObject));
        }

        public static void SetExposeColor(int i)
        {
            EffectReplacer.Activate(HealthComponent.AssetReferences.mercExposeConsumeEffectPrefab, i);
            switch (i)
            {
                case 0:
                    CharacterBody.AssetReferences.mercExposeEffectPrefab = Merc_Blue.MercExposeEffect;
                    return;
                case 1:
                    CharacterBody.AssetReferences.mercExposeEffectPrefab = Merc_Red.MercExposeEffect_Red;
                    return;
                case 2:
                    CharacterBody.AssetReferences.mercExposeEffectPrefab = Merc_Green.MercExposeEffect_Green;
                    return;
            }
        }


        private static void Assaulter2_OnEnter_On(On.EntityStates.Merc.Assaulter2.orig_OnEnter orig, EntityStates.Merc.Assaulter2 self)
        {
            if (self.gameObject.GetComponent<MakeThisMercRed>())
            {
                EffectReplacer.Activate(self.hitEffectPrefab, 1);
                self.swingEffectPrefab = Merc_Red.MercAssaulterEffect_Red;
            }
            else if (self.gameObject.GetComponent<MakeThisMercGreen>())
            {
                EffectReplacer.Activate(self.hitEffectPrefab, 2);
                self.swingEffectPrefab = Merc_Green.MercAssaulterEffect_Green;
            }
            else
            {
                EffectReplacer.Activate(self.hitEffectPrefab, 0);
            }
            orig(self);
        }

        private static void GroundLight2_OnEnter(On.EntityStates.Merc.Weapon.GroundLight2.orig_OnEnter orig, EntityStates.Merc.Weapon.GroundLight2 self)
        {
            if (!self.isComboFinisher)
            {
                int i = SkinChanges.DetectCaseMerc(self.gameObject);
                EffectReplacer.Activate(self.hitEffectPrefab, i);
                if (i == 1)
                {
                    self.swingEffectPrefab = Merc_Red.MercSwordSlash_Red;
                }
                else if (i == 2)
                {
                    self.swingEffectPrefab = Merc_Green.MercSwordSlash_Green;
                }
            }
            orig(self);
        }

        private static void Evis_OnExit(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
                    x => x.MatchStfld("RoR2.TemporaryOverlayInstance", "originalMaterial")))
            {
                //First one is matMercEvisTarget probably not needed
                c.Index += 4;
                c.TryGotoNext(MoveType.Before,
                x => x.MatchStfld("RoR2.TemporaryOverlayInstance", "originalMaterial"));
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<Material, EvisDash, Material>>((material, entityState) =>
                {
                    if (entityState.gameObject.GetComponent<MakeThisMercRed>())
                    {
                        return Merc_Red.matHuntressFlashExpanded_Red;
                    }
                    else if (entityState.gameObject.GetComponent<MakeThisMercGreen>())
                    {
                        return Merc_Green.matHuntressFlashExpanded_Green;
                    }
                    return material;
                });
                //Debug.Log("IL Found: Red Merc: IL.EntityStates.Merc.Evis.OnExit");
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: IL.EntityStates.Merc.Evis.OnExit");
            }
        }

        private static void Finisher_GroundLight2_OnEnter(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("EntityStates.Merc.Weapon.GroundLight2", "comboFinisherSwingEffectPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, GroundLight2, GameObject>>((target, entityState) =>
                {
                    int i = SkinChanges.DetectCaseMerc(entityState.gameObject);
                    EffectReplacer.Activate(entityState.hitEffectPrefab, i);
                    SetExposeColor(i);
                    if (i == 1)
                    {
                        return Merc_Red.MercSwordFinisherSlash_Red;
                    }
                    else if (i == 2)
                    {
                        return Merc_Green.MercSwordFinisherSlash_Green;
                    }
                    return target;
                });
                Debug.Log("IL Found: Red Merc: GroundLight2_OnEnter");
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: GroundLight2_OnEnter");
            }
        }

        private static void WhirlwindBase_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("EntityStates.Merc.WhirlwindBase", "swingEffectPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, WhirlwindBase, GameObject>>((target, entityState) =>
                {
                    EffectReplacer.ActivateMerc(target, entityState.outer.gameObject);
                    return target;
                });
                Debug.Log("IL Found: Red Merc: WhirlwindBase_FixedUpdate");
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: WhirlwindBase_FixedUpdate");
            }
        }

        private static void WhirlwindBase_OnEnter(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("EntityStates.Merc.WhirlwindBase", "hitEffectPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, WhirlwindBase, GameObject>>((target, entityState) =>
                {
                    EffectReplacer.ActivateMerc(target, entityState.outer.gameObject);
                    return target;
                });
                //Debug.Log("IL Found: Red Merc: WhirlwindBase_OnEnter");
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: WhirlwindBase_OnEnter");
            }
        }

        private static void Uppercut_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("EntityStates.Merc.Uppercut", "swingEffectPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, Uppercut, GameObject>>((target, entityState) =>
                {
                    EffectReplacer.ActivateMerc(target, entityState.outer.gameObject);
                    return target;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: Uppercut_FixedUpdate");
            }
        }

        private static void Uppercut_OnEnter(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("EntityStates.Merc.Uppercut", "hitEffectPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, Uppercut, GameObject>>((target, entityState) =>
                {
                    EffectReplacer.ActivateMerc(target, entityState.outer.gameObject);
                    return target;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: Uppercut_OnEnter");
            }
        }

        private static void Assaulter2_OnEnter(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.TemporaryOverlayInstance", "originalMaterial")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<Material, EntityStates.Merc.EvisDash, Material>>((material, entityState) =>
                {
                    if (entityState.gameObject.GetComponent<MakeThisMercRed>())
                    {
                        return Merc_Red.matMercEnergized_Red;
                    }
                    else if (entityState.gameObject.GetComponent<MakeThisMercGreen>())
                    {
                        return Merc_Green.matMercEnergized_Green;
                    }
                    return material;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: Assaulter2_OnEnter");
            }
        }

        private static void MercRed_SpecialEvis(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
                    x => x.MatchLdsfld("EntityStates.Merc.Evis", "blinkPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Merc.Evis, GameObject>>((target, entityState) =>
                {
                    EffectReplacer.ActivateMerc(target, entityState.outer.gameObject);
                    return target;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: Evis.CreateBlinkEffect");
            }
        }

        private static void Evis_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
                    x => x.MatchLdsfld("EntityStates.Merc.Evis", "hitEffectPrefab")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Merc.Evis, GameObject>>((target, entityState) =>
                {
                    EffectReplacer.ActivateMerc(target, entityState.outer.gameObject);
                    return target;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: Evis_FixedUpdate");
            }

            if (c.TryGotoNext(MoveType.Before,
                    x => x.MatchStfld("RoR2.TemporaryOverlayInstance","originalMaterial")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<Material, Evis, Material>>((material, entityState) =>
                {
                    if (entityState.gameObject.GetComponent<MakeThisMercRed>())
                    {
                        return Merc_Red.matMercEvisTarget_Red;
                    }
                    else if (entityState.gameObject.GetComponent<MakeThisMercGreen>())
                    {
                        return Merc_Green.matMercEvisTarget_Green;
                    }
                    return material;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: Evis_FixedUpdate");
            }
        }

        public static void Special_Dash(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
                    x => x.MatchStfld("RoR2.TemporaryOverlayInstance", "originalMaterial")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<Material, EvisDash, Material>>((material, entityState) =>
                {
                    if (entityState.gameObject.GetComponent<MakeThisMercRed>())
                    {
                        return Merc_Red.matHuntressFlashBright_Red;
                    }
                    else if (entityState.gameObject.GetComponent<MakeThisMercGreen>())
                    {
                        return Merc_Green.matHuntressFlashBright_Green;
                    }
                    return material;
                });
                c.Index += 4;
                c.TryGotoNext(MoveType.Before,
                x => x.MatchStfld("RoR2.TemporaryOverlayInstance", "originalMaterial"));
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<Material, EvisDash, Material>>((material, entityState) =>
                {
                    if (entityState.gameObject.GetComponent<MakeThisMercRed>())
                    {
                        return Merc_Red.matHuntressFlashExpanded_Red;
                    }
                    else if (entityState.gameObject.GetComponent<MakeThisMercGreen>())
                    {
                        return Merc_Green.matHuntressFlashExpanded_Green;
                    }
                    return material;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Red Merc: Merc.EvisDash.FixedUpdate");
            }
        }

    }
}