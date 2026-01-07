using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client.Skins
{
    public static class EliteEffects
    {
        public class CopyEliteShaderToGhost : MonoBehaviour
        {
            public void Start()
            {
                ProjectileController proj = GetComponent<ProjectileController>();
                if (proj.ghost && proj.owner)
                {
                    ModelLocator component = proj.owner.GetComponent<ModelLocator>();
                    if (component)
                    {
                        Transform modelTransform = component.modelTransform;
                        if (modelTransform)
                        {
                            CharacterModel component2 = modelTransform.GetComponent<CharacterModel>();
                            if (component2)
                            {
                                proj.ghost.GetComponentInChildren<MeshRenderer>().material.SetFloat("_EliteIndex", component2.shaderEliteRampIndex + 1);
                            }
                        }
                    }

                }

            }
        }
        public class CopyEliteIndexAlt : MonoBehaviour
        {
            public Renderer renderer;
            public Material material;
            public void OnDisable()
            {
                //Prepped Balls do not have owner
                //We can still use this for the 2 projectiles but manual balls

                //Fist is not a projec?
                //Fist is explosionEffect so uhh idk about that


                ProjectileController proj = GetComponent<ProjectileController>();
                if (proj.ghost && proj.owner)
                {

                    ModelLocator component = proj.owner.GetComponent<ModelLocator>();
                    if (component)
                    {
                        Transform modelTransform = component.modelTransform;
                        if (modelTransform)
                        {
                            CharacterModel component2 = modelTransform.GetComponent<CharacterModel>();
                            if (component2)
                            {
                                renderer.material = material;
                                renderer.material.SetFloat("_EliteIndex", component2.shaderEliteRampIndex + 1);
                            }
                        }
                    }

                }

            }
        }

        public static void Start()
        {
            if (WConfig.cfgSkinBellBalls.Value == false)
            {
                return;
            }

            GameObject BellBall = Addressables.LoadAssetAsync<GameObject>(key: "99151650efe03be4c9a332ffe1466cb2").WaitForCompletion();
            GameObject BellBallGhost = Addressables.LoadAssetAsync<GameObject>(key: "30f2aa5ecd9832b4f82b1eb537c6dedf").WaitForCompletion();
            GameObject TitanPreFistProjectile = Addressables.LoadAssetAsync<GameObject>(key: "44b979afbdb688c438bd14bcf12e249f").WaitForCompletion();
            GameObject TitanFistEffect = Addressables.LoadAssetAsync<GameObject>(key: "9858bb44cb951674f813a6a0d033e799").WaitForCompletion();

            BellBall.AddComponent<CopyEliteShaderToGhost>();
            BellBallGhost.AddComponent<VFXAttributes>().DoNotPool = true;
            TitanFistEffect.GetComponent<VFXAttributes>().DoNotPool = true;
            TitanPreFistProjectile.AddComponent<CopyEliteIndexAlt>();
            TitanPreFistProjectile.GetComponent<CopyEliteIndexAlt>().renderer = TitanFistEffect.transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
            TitanPreFistProjectile.GetComponent<CopyEliteIndexAlt>().material = Addressables.LoadAssetAsync<Material>(key: "a56bd29548ccd9e439c5da67285a6281").WaitForCompletion();

            //0 fist

            IL.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FixedUpdate += BellBalls_ChargeTrioBomb_FixedUpdate;
        }


        private static void BellBalls_ChargeTrioBomb_FixedUpdate(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.Before,
                 //x => x.MatchLdsfld("EntityStates.Bell.BellWeapon.ChargeTrioBomb", "preppedBombPrefab")
                 x => x.MatchStloc(1)
                ))
            {
                //This is instance right
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Bell.BellWeapon.ChargeTrioBomb, GameObject>>((ballPrep, entityState) =>
                {
                    //Instance of Object not base Object
                    if (entityState.characterBody.isElite)
                    {
                        ballPrep.transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_EliteIndex", entityState.GetModelTransform().GetComponent<CharacterModel>().shaderEliteRampIndex + 1);
                        //Debug.Log(ballPrep.transform.GetChild(0).GetComponent<Renderer>().material.GetFloat("_EliteIndex"));
                    }
                    return ballPrep;
                });

            }
            else
            {
                Log.LogWarning("IL Failed: IL.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FixedUpdate");
            }
        }



    }


}