using EntityStates.Croco;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public class Acrid_Hooks
    {
        public static GameObject CrocoDiseaseOrbEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/orbeffects/CrocoDiseaseOrbEffect"); //CrocoDiseaseOrbEffect 
        public static GameObject CrocoDiseaseImpactEffect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Croco/CrocoDiseaseImpactEffect.prefab").WaitForCompletion(); //CrocoDiseaseOrbEffect 

        public static void Start()
        {

            if (WConfig.cfgSkinAcridBlight.Value)
            {

                On.EntityStates.Croco.Slash.BeginMeleeAttackEffect += AcridBlight_M1;

                On.EntityStates.Croco.FireSpit.OnEnter += AcridBlight_M2_M4;

                On.EntityStates.Croco.Bite.BeginMeleeAttackEffect += AcridBlight_M2Alt;

                On.EntityStates.Croco.BaseLeap.OnEnter += AcridBlight_M3_Start;
                On.EntityStates.Croco.Leap.DoImpactAuthority += AcridBlight_M3_Pool;


                //IL.RoR2.Orbs.LightningOrb.Begin += BlightedOrbEffect_HostOnly;
                //RoR2.Orbs.OrbStorageUtility._orbDictionary.Add("BlightOrb", Effects_Blight.CrocoDiseaseOrbEffect_Blight);

                //Fucking everything Orb related is ServerSided so the latest we can do it is during the attack I guess.

            }


        }


        private static void BlightedOrbEffect_HostOnly(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
            x => x.MatchLdstr("Prefabs/Effects/OrbEffects/CrocoDiseaseOrbEffect")))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<string, RoR2.Orbs.LightningOrb, string>>((target, orb) =>
                {
                    if (orb.damageType == DamageType.BlightOnHit)
                    {
                        //Technically should also activae Impact here, but probably fine;
                        EffectReplacer.Activate(CrocoDiseaseOrbEffect, 1);
                    }
                    else
                    {
                        EffectReplacer.Activate(CrocoDiseaseOrbEffect, 0);
                    }
                    return target;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: Blight: IL.RoR2.Orbs.LightningOrb.Begin");
            }
        }




        private static void AcridBlight_M3_Start(On.EntityStates.Croco.BaseLeap.orig_OnEnter orig, BaseLeap self)
        {
            if (self is Leap)
            {
                if (self.outer.GetComponent<MakeThisAcridBlight>())
                {
                    self.fistEffectPrefab = Effects_Blight.CrocoFistEffectBlight;
                };
            }
            orig(self);
        }

        private static void AcridBlight_M3_Pool(On.EntityStates.Croco.Leap.orig_DoImpactAuthority orig, Leap self)
        {
            EffectReplacer.ActivateAcrid(self.blastEffectPrefab, self.outer.gameObject);
            orig(self);
        }

        private static void AcridBlight_M2Alt(On.EntityStates.Croco.Bite.orig_BeginMeleeAttackEffect orig, Bite self)
        {
            if (self.outer.GetComponent<MakeThisAcridBlight>())
            {
                self.swingEffectPrefab = Effects_Blight.CrocoBiteEffectBlight;
            };
            orig(self);
        }

        private static void AcridBlight_M2_M4(On.EntityStates.Croco.FireSpit.orig_OnEnter orig, FireSpit self)
        {
            EffectReplacer.ActivateAcrid(self.effectPrefab, self.outer.gameObject);
            orig(self);
        }

        private static void AcridBlight_M1(On.EntityStates.Croco.Slash.orig_BeginMeleeAttackEffect orig, Slash self)
        {
            if (self.outer.GetComponent<MakeThisAcridBlight>())
            {
                if (self.isComboFinisher)
                {
                    self.swingEffectPrefab = Effects_Blight.CrocoComboFinisherSlashBlight;
                }
                else
                {
                    self.swingEffectPrefab = Effects_Blight.CrocoSlashBlight;
                }
            };
            orig(self);
        }

    }
}