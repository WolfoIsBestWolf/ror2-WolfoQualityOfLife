using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{
    public class Skins_REX
    {

        public static void Start()
        {
            Material matTreebotTreeFlower = Addressables.LoadAssetAsync<Material>(key: "RoR2/Base/Treebot/matTreebotTreeFlower.mat").WaitForCompletion();
            Material MatTreebot_VineSots = GameObject.Instantiate(matTreebotTreeFlower);

            Texture2D texTreebotVineForColossus = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/SkinScalable/texTreebotVineForColossus.png");
            MatTreebot_VineSots.mainTexture = texTreebotVineForColossus;

            SkinDefParams skinTreebotAltColossus = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Treebot/skinTreebotAltColossus_params.asset").WaitForCompletion();
            SkinDef skins = Addressables.LoadAssetAsync<SkinDef>(key: "RoR2/Base/Treebot/skinTreebotAltColossus.asset").WaitForCompletion();

            Renderer vine = skins.rootObject.transform.GetChild(5).GetChild(0).GetChild(2).GetComponent<ParticleSystemRenderer>();

            HG.ArrayUtils.ArrayAppend(ref skinTreebotAltColossus.rendererInfos, new CharacterModel.RendererInfo
            {
                defaultMaterial = MatTreebot_VineSots,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off,
                renderer = vine
            });



            SkinDefParams skinTreebotAlt_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "RoR2/Base/Treebot/skinTreebotAlt_params.asset").WaitForCompletion();
            HG.ArrayUtils.ArrayAppend(ref skinTreebotAlt_params.rendererInfos, new CharacterModel.RendererInfo
            {
                defaultMaterialAddress = skinTreebotAlt_params.rendererInfos[1].defaultMaterialAddress,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off,
                renderer = vine
            });
            SkinDefParams skinTreebotDefault_params = Addressables.LoadAssetAsync<SkinDefParams>(key: "1ce7132e738187a4782795e7ac6a3232").WaitForCompletion();
            HG.ArrayUtils.ArrayAppend(ref skinTreebotDefault_params.rendererInfos, new CharacterModel.RendererInfo
            {
                defaultMaterialAddress = skinTreebotDefault_params.rendererInfos[1].defaultMaterialAddress,
                defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off,
                renderer = vine
            });
        }

        #region Skinned REX attacks (Old)
        /*
        //public static uint MULTCoverPrefab = 255;
        public static uint REXSkinMortar = 255;
        public static uint REXSkinShock = 255;
        public static uint REXSkinFlowerP = 255;
        //public static uint REXSkinFlowerSpawn = 255;
        //public static uint REXSkinFlowerE = 255;
        public static uint REXSkinFlowerEnter = 255;
        public static uint REXSkinFlowerExit = 255;
        //public static uint REXSkinFlowerR = 255;
        public static Material REXFlowerTempMat = null;
        */

        /*
  public static void REXSkinnedAttacks()
  {
      On.RoR2.Orbs.OrbEffect.Start += (orig, self) =>
      {
          //Debug.LogWarning(self);
          orig(self);
          if (self.name.StartsWith("EntangleOrbEffect(Clone)"))
          {
              if (self.gameObject.transform.childCount > 0)
              {
                  self.gameObject.transform.GetChild(0).GetComponent<LineRenderer>().materials[0].SetTexture("_RemapTex", REXFlowerTempMat.mainTexture);
                  self.gameObject.transform.GetChild(0).GetComponent<LineRenderer>().materials[1].SetTexture("_RemapTex", REXFlowerTempMat.mainTexture);
                  self.gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = REXFlowerTempMat;
              }
          }
      };

      On.EntityStates.FireFlower2.OnEnter += (orig, self) =>
      {
          //self.outer
          //Debug.LogWarning("FireFlower2.OnEnter "+EntityStates.FireFlower2.projectilePrefab);
          if (self.outer.commonComponents.characterBody.skinIndex != REXSkinFlowerP)
          {
              REXSkinFlowerP = self.outer.commonComponents.characterBody.skinIndex;

              Material temp = self.outer.commonComponents.modelLocator.modelTransform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
              EntityStates.FireFlower2.projectilePrefab.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
              EntityStates.FireFlower2.projectilePrefab.GetComponent<RoR2.Projectile.ProjectileController>().ghostPrefab.transform.GetChild(1).GetComponent<MeshRenderer>().material = temp;
          }
          orig(self);
      };

      On.EntityStates.Treebot.TreebotFlower.SpawnState.OnEnter += (orig, self) =>
      {
          //Debug.LogWarning("SpawnState.OnEnter " + self.outer.gameObject);
          orig(self);
          Material temp = self.outer.commonComponents.projectileController.owner.GetComponent<ModelLocator>().modelTransform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;

          //self.outer.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Light>().color = new Color(0.6991f, 1f, 0.0627f, 1f);
          self.outer.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<Light>().color = new Color(0.6991f, 0.3627f, 0.8f, 1f);

          self.outer.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<SkinnedMeshRenderer>().material = temp;
      };

      On.EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.OnEnter += (orig, self) =>
      {
          //Debug.LogWarning("TreebotFlower2Projectile.OnEnter " + EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.enterEffectPrefab);
          //Debug.LogWarning("TreebotFlower2Projectile.OnEnter " + EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.exitEffectPrefab);
          //Debug.LogWarning(self.outer.gameObject);

          uint utemp = self.outer.commonComponents.projectileController.owner.GetComponent<CharacterBody>().skinIndex;
          if (utemp != REXSkinFlowerEnter)
          {
              REXSkinFlowerEnter = utemp;
              Transform tempt = self.outer.commonComponents.projectileController.owner.GetComponent<ModelLocator>().modelTransform;


              GameObject tempobj = EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.enterEffectPrefab;

              Material temp = tempt.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
              Material tempL = tempt.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
              REXFlowerTempMat = temp;
              tempobj.transform.GetChild(7).GetComponent<ParticleSystemRenderer>().material = tempL;
              tempobj.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
          }

          orig(self);
      };

      On.EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.OnExit += (orig, self) =>
      {
          //Debug.LogWarning("TreebotFlower2Projectile.OnExit " + EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.enterEffectPrefab);
          //Debug.LogWarning("TreebotFlower2Projectile.OnExit " + EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.exitEffectPrefab);
          GameObject tempowner = self.outer.commonComponents.projectileController.owner;
          if (tempowner)
          {
              uint utemp = tempowner.GetComponent<CharacterBody>().skinIndex;
              if (utemp != REXSkinFlowerExit)
              {
                  REXSkinFlowerExit = utemp;
                  Transform tempt = self.outer.commonComponents.projectileController.owner.GetComponent<ModelLocator>().modelTransform;

                  GameObject tempobj = EntityStates.Treebot.TreebotFlower.TreebotFlower2Projectile.enterEffectPrefab;

                  Material temp = tempt.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
                  Material tempL = tempt.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
                  tempobj.transform.GetChild(7).GetComponent<ParticleSystemRenderer>().material = tempL;
                  tempobj.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
              }
          }

          orig(self);
      };

      On.EntityStates.Treebot.Weapon.FirePlantSonicBoom.OnEnter += (orig, self) =>
      {
          if (self.outer.commonComponents.characterBody.skinIndex != REXSkinShock)
          {
              REXSkinFlowerP = self.outer.commonComponents.characterBody.skinIndex;

              Material temp = self.outer.commonComponents.modelLocator.modelTransform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
              Material tempL = self.outer.commonComponents.modelLocator.modelTransform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
              self.fireEffectPrefab.transform.GetChild(10).GetComponent<ParticleSystemRenderer>().material = temp;
              self.fireEffectPrefab.transform.GetChild(11).GetComponent<ParticleSystemRenderer>().material = tempL;
              self.fireEffectPrefab.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material.SetTexture("_RemapTex", temp.mainTexture);

              EntityStates.Treebot.Weapon.FirePlantSonicBoom.hitEffectPrefab.transform.GetChild(7).GetComponent<ParticleSystemRenderer>().material = tempL;
              EntityStates.Treebot.Weapon.FirePlantSonicBoom.hitEffectPrefab.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
          }
          orig(self);
      };

      On.EntityStates.Treebot.Weapon.AimMortar2.OnEnter += (orig, self) =>
      {
          if (self.projectilePrefab.name.StartsWith("TreebotMortar2"))
          {
              if (self.outer.commonComponents.characterBody.skinIndex != REXSkinMortar)
              {
                  REXSkinFlowerP = self.outer.commonComponents.characterBody.skinIndex;
                  GameObject tempobj = self.projectilePrefab.GetComponent<RoR2.Projectile.ProjectileImpactExplosion>().impactEffect;

                  Material temp = self.outer.commonComponents.modelLocator.modelTransform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material;
                  Material tempL = self.outer.commonComponents.modelLocator.modelTransform.GetChild(3).GetComponent<SkinnedMeshRenderer>().material;
                  tempobj.transform.GetChild(7).GetComponent<ParticleSystemRenderer>().material = tempL;
                  tempobj.transform.GetChild(11).GetComponent<Light>().color = new Color(0.8817f, 0f, 0.9922f, 1f);
                  tempobj.transform.GetChild(12).GetComponent<ParticleSystemRenderer>().material = temp;
                  tempobj.transform.GetChild(14).GetComponent<ParticleSystemRenderer>().material = tempL;
              }

          }
          //Debug.LogWarning("AimMortar2 " + self.projectilePrefab);
          orig(self);
      };


  }
  */
        #endregion

    }

}