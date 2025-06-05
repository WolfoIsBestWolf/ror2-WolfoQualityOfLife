using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using RoR2.ContentManagement;

namespace WolfoQoL_Client
{
    public class Mercenary_Main
    {
        public static void Start()
        {
            Merc_Red.Start();
            Merc_Red.MakeEffects_Red();
            Merc_Green.Start();
            Merc_Green.Merc_GreenEffects();

            Mercenary_Hooks.Hooks();

            GhostReplacements();
            GameModeCatalog.availability.CallWhenAvailable(EffectReplacements); //After effect catalog

   
        }
 
        public static void EffectReplacements()
        {
             
            GameObject effect = null;
            //M2
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordSlashWhirlwind.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.MercSwordSlashWhirlwind_Red, Merc_Green.MercSwordSlashWhirlwind_Green, false);
            //M2 Alt
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercSwordUppercutSlash.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.MercSwordUppercutSlash_Red, Merc_Green.MercSwordUppercutSlash_Green, false);
             //M3
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercAssaulter.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.ImpactMercAssaulter_Red, Merc_Green.ImpactMercAssaulter_Green, false);
             //M3 Alt
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercFocusedAssault.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.ImpactMercFocusedAssault_Red, Merc_Green.ImpactMercFocusedAssault_Green, false);
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercFocusedAssaultOrbEffect.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.MercFocusedAssaultOrbEffect_Red, Merc_Green.MercFocusedAssaultOrbEffect_Green, false);
            

            //M4 ?
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/ImpactMercEvis.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.ImpactMercEvis_Red, Merc_Green.ImpactMercEvis_Green, false);

            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMercEvis.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.OmniImpactVFXSlashMercEvis_Red, Merc_Green.OmniImpactVFXSlashMercEvis_Green, false);

            //General
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/OmniImpactVFXSlashMerc.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.OmniImpactVFXSlashMerc_Red, Merc_Green.OmniImpactVFXSlashMerc_Green, false);
            
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Merc/MercExposeConsumeEffect.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.MercExposeConsumeEffect_Red, Merc_Green.MercExposeConsumeEffect_Green, false);


            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Huntress/HuntressBlinkEffect.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.HuntressBlinkEffect_Red, Merc_Green.HuntressBlinkEffect_Green, true);
            effect = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/Huntress/HuntressFireArrowRain.prefab").WaitForCompletion();
            EffectReplacer.SetupComponent(effect, Merc_Red.HuntressFireArrowRain_Red, Merc_Green.HuntressFireArrowRain_Green, true);

             
        }


        public static void GhostReplacements()
        {
            GameObject evisProj = LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/EvisProjectile");
            GameObject evisProj2 = LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/EvisOverlapProjectile");

            ProjectileGhostReplacer replacer1 = evisProj.AddComponent<ProjectileGhostReplacer>();
            ProjectileGhostReplacer replacer2 = evisProj2.AddComponent<ProjectileGhostReplacer>();

            replacer1.condition = SkinChanges.Case.Merc;
            replacer2.condition = SkinChanges.Case.Merc;

            replacer1.ghostPrefab_1 = Merc_Red.EvisProjectileGhost_Red;
            replacer1.impactPrefab_1 = Merc_Red.MercSwordFinisherSlash_Red;
            replacer1.ghostPrefab_2 = Merc_Green.EvisProjectileGhost_Green;
            replacer1.impactPrefab_2 = Merc_Green.MercSwordFinisherSlash_Green;
            replacer1.useReplacement = false;

            replacer2.ghostPrefab_1 = Merc_Red.EvisOverlapProjectileGhost_Red;
            //replacer2.impactPrefab_1 = Merc_Red.ImpactMercEvis_Red;
            replacer2.ghostPrefab_2 = Merc_Green.EvisOverlapProjectileGhost_Green;
            //replacer2.impactPrefab_2 = Merc_Green.ImpactMercEvis_Green;

        }




        public static void BackupSkin()
        {
            SkinDef SkinDefMercOni = Addressables.LoadAssetAsync<SkinDef>(key: "RoR2/Base/Merc/skinMercAlt.asset").WaitForCompletion();
            SkinDef skinDef = Object.Instantiate(SkinDefMercOni);
            Sprite texMercOniBluesS = Assets.Bundle.LoadAsset<Sprite>("Assets/WQoL/SkinRamps/texMercOniBlues.png");
            skinDef.icon = texMercOniBluesS;

            Skins.AddSkinToCharacter(LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/MercBody"), skinDef);


        }


    }
}
