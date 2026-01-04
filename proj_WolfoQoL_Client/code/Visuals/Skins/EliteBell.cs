using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using UnityEngine;

namespace WolfoQoL_Client.Skins
{
    public static class EliteBell
    {


        public static GameObject BellBallGhostElite; //Bell Balls Prep

        public static void Start()
        {
            if (WConfig.cfgSkinBellBalls.Value == false)
            {
                return;
            }

            GameObject BellBall = LegacyResourcesAPI.Load<GameObject>("Prefabs/projectiles/BellBall");
            ProjectileGhostReplacer replacer = BellBall.AddComponent<ProjectileGhostReplacer>();

            BellBallGhostElite = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/projectileghosts/BellBallGhost"), "BellBallGhostElite", false);
            replacer.condition = SkinChanges.Case.Brass;
            replacer.ghostPrefab_1 = BellBallGhostElite;

            BellBallGhostElite.AddComponent<VFXAttributes>().DoNotPool = true;


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

                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Bell.BellWeapon.ChargeTrioBomb, GameObject>>((ballPrep, entityState) =>
                {
                    //Instance of Object not base Object
                    if (entityState.characterBody.isElite)
                    {
                        ballPrep.transform.GetChild(0).GetComponent<MeshRenderer>().material = entityState.characterBody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                    }
                    return ballPrep;
                });

            }
            else
            {
                WQoLMain.log.LogWarning("IL Failed: IL.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FixedUpdate");
            }
        }



    }


}