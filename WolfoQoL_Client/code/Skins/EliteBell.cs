using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using UnityEngine;

namespace WolfoQoL_Client.Skins
{
    public class EliteBell
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

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdsfld("EntityStates.Bell.BellWeapon.ChargeTrioBomb", "preppedBombPrefab")))
            {
                //CurrentIndex 37
                c.Index += 6;
                //Debug.Log(c + " " + c.Next + " " + c.Next.Operand);
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<GameObject, EntityStates.Bell.BellWeapon.ChargeTrioBomb, GameObject>>((ballPrep, entityState) =>
                {
                    //Instance of Object not base Object
                    if (entityState.characterBody.isElite)
                    {
                        //ballPrep; //Do Ball Stuff
                        //ballPrep.transform.GetChild(0).GetComponent<UnityEngine.MeshRenderer>().material = EquipmentCatalog.GetEquipmentDef(entityState.characterBody.equipmentSlot.equipmentIndex).pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                        if (!entityState.isAuthority)
                        {
                            BellBallGhostElite.transform.GetChild(0).GetComponent<MeshRenderer>().material = entityState.characterBody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                        }
                        ballPrep.transform.GetChild(0).GetComponent<MeshRenderer>().material = entityState.characterBody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                    }
                    return ballPrep;
                });
                /* c.Index += 4;
                 c.TryGotoNext(MoveType.After,
                 x => x.MatchLdsfld("EntityStates.Bell.BellWeapon.ChargeTrioBomb", "bombProjectilePrefab"));
                 //Debug.Log(c + " " + c.Next + " " + c.Next.Operand);
                 c.Emit(OpCodes.Ldarg_0);
                 c.EmitDelegate<Func<GameObject, EntityStates.Bell.BellWeapon.ChargeTrioBomb, GameObject>>((ballProj, entityState) =>
                 {
                     if (entityState.characterBody.isElite)
                     {
                         //BellBallGhostElite.transform.GetChild(0).GetComponent<MeshRenderer>().material = entityState.characterBody.inventory.currentEquipmentState.equipmentDef.pickupModelPrefab.GetComponentInChildren<MeshRenderer>().material;
                         //return BellBallElite;
                     }
                     return ballProj;
                 });*/

            }
            else
            {
                Debug.LogWarning("IL Failed: IL.EntityStates.Bell.BellWeapon.ChargeTrioBomb.FixedUpdate");
            }
        }



    }


}