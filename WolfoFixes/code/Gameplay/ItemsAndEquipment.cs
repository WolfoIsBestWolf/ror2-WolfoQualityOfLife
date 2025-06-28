using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    public class ItemsAndEquipment
    {
        public static void Start()
        {
            Addressables.LoadAssetAsync<GameObject>(key: "9ca7d392fa3bb444b827d475b36b9253").WaitForCompletion().AddComponent<ThisIsASawmarang>();
            Addressables.LoadAssetAsync<EquipmentDef>(key: "f2ddbb7586240e648945ad494ebe3984").WaitForCompletion().cooldown = 0; //Why does this have a cooldown it just fucks you up when buying shops quickly
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += FixSawmarang;

            // IL.RoR2.HealthComponent.TakeDamageProcess += FixEchoOSP;
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWarpedEchoE8;

            On.RoR2.Util.HealthComponentToTransform += FixTwisteds_NotWorkingOnPlayers;

            On.RoR2.JetpackController.SetupWings += BugWingsAlways_SetupWings;
            On.RoR2.JetpackController.OnDisable += BugWingsAlways_OnDisable;

            IL.RoR2.GlobalEventManager.ProcessHitEnemy += FixChargedPerferatorCrit;

            //Needed for SimuAdds
            On.EntityStates.QuestVolatileBattery.CountDown.OnEnter += FallbackIfNoItemDisplay;
        }

        private static void FallbackIfNoItemDisplay(On.EntityStates.QuestVolatileBattery.CountDown.orig_OnEnter orig, EntityStates.QuestVolatileBattery.CountDown self)
        {
            orig(self);
            if (self.vfxInstances.Length == 0)
            {
                if (!self.networkedBodyAttachment.attachedBody)
                {
                    return;
                }
                if (EntityStates.QuestVolatileBattery.CountDown.vfxPrefab && self.attachedCharacterModel)
                {
                    self.vfxInstances = new GameObject[1];
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EntityStates.QuestVolatileBattery.CountDown.vfxPrefab, self.networkedBodyAttachment.attachedBody.coreTransform);
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    gameObject.transform.localScale = new Vector3(2.5f / gameObject.transform.lossyScale.x, 2.5f / gameObject.transform.lossyScale.y, 2.5f / gameObject.transform.lossyScale.z);
                    if (gameObject.transform.localScale.z < 1)
                    {
                        gameObject.transform.localScale = new Vector3(1, 1, 1); //Idk dude
                    }
                    gameObject.transform.GetChild(0).GetComponent<Light>().range *= 3;
                    gameObject.transform.GetChild(0).GetComponent<Light>().intensity *= 3;
                    gameObject.transform.GetChild(0).GetComponent<LightIntensityCurve>().timeMax /= 2;
                    self.vfxInstances[0] = gameObject;
                }
            }
        }

        private static void FixEchoOSP(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            int beforeEcho = -1;
            int beforeOSP = -1;
            int afterOSP = -1;

            ILCursor cbeforeEcho;
            ILCursor cbeforeOSP;
            ILCursor cafterOSP;



            //Goto X
            //Y
            //Echo Code
            //Goto Z
            //X
            //OSP
            //Goto Y
            //Z

            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.DLC2Content/Buff", "DelayedDamageBuff"));
            c.TryGotoNext(MoveType.After,
            x => x.MatchNewobj("RoR2.Orbs.SimpleLightningStrikeOrb"));


            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.Orbs.GenericDamageOrb", "isCrit")
            ))
            {

                cbeforeEcho = c.Emit(OpCodes.Break);


            }
            else
            {
                Debug.LogWarning("IL Failed : FixChargedPerferatorCrit");
            }

        }



        public static void FixChargedPerferatorCrit(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchLdsfld("RoR2.RoR2Content/Items", "LightningStrikeOnHit"));
            c.TryGotoNext(MoveType.After,
            x => x.MatchNewobj("RoR2.Orbs.SimpleLightningStrikeOrb"));


            if (c.TryGotoNext(MoveType.Before,
            x => x.MatchStfld("RoR2.Orbs.GenericDamageOrb", "isCrit")
            ))
            {

                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<Func<bool, DamageInfo, bool>>((isCrit, damageInfo) =>
                {
                    return damageInfo.crit;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : FixChargedPerferatorCrit");
            }
        }

        private static Transform FixTwisteds_NotWorkingOnPlayers(On.RoR2.Util.orig_HealthComponentToTransform orig, HealthComponent healthComponent)
        {
            if (healthComponent.body && healthComponent.body.mainHurtBox)
            {
                return healthComponent.body.mainHurtBox.transform;
            }
            return orig(healthComponent);
        }

        private static void FixWarpedEchoE8(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.TryGotoNext(MoveType.After,
            x => x.MatchCall("RoR2.Run", "get_instance"),
            x => x.MatchCallvirt("RoR2.Run", "get_selectedDifficulty"),
            x => x.MatchLdcI4((int)DifficultyIndex.Eclipse8));

            if (c.TryGotoNext(MoveType.Before,
           x => x.MatchLdfld("RoR2.DamageInfo", "delayedDamageSecondHalf")
           ))
            {

                c.Remove();
                c.EmitDelegate<System.Func<DamageInfo, bool>>((damageInfo) =>
                {
                    return false;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : FixWarpedEchoE8");
            }
        }

        private static void FixSawmarang(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
             x => x.MatchLdsfld("RoR2.RoR2Content/Equipment", "Saw"),
             x => x.MatchCallvirt("RoR2.EquipmentDef", "get_equipmentIndex")
            ))
            {
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<System.Func<EquipmentIndex, DamageInfo, EquipmentIndex>>((eq, damageInfo) =>
                {
                    Debug.Log(eq);
                    if (damageInfo.inflictor.GetComponent<ThisIsASawmarang>())
                    {
                        //Coulndt find the bool ig
                        return RoR2Content.Equipment.Saw.equipmentIndex;
                    }
                    return eq;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : FixSawmarang");
            }
        }

        private static void BugWingsAlways_SetupWings(On.RoR2.JetpackController.orig_SetupWings orig, JetpackController self)
        {
            CharacterModel model = null;
            if (self.targetBody && self.targetBody.isPlayerControlled)
            {
                model = self.targetBody.modelLocator.modelTransform.GetComponent<CharacterModel>();
                if (model.itemDisplayRuleSet)
                {
                    if (model.currentEquipmentDisplayIndex != RoR2Content.Equipment.Jetpack.equipmentIndex)
                    {
                        DisplayRuleGroup equipmentDisplayRuleGroup = model.itemDisplayRuleSet.GetEquipmentDisplayRuleGroup(RoR2Content.Equipment.Jetpack.equipmentIndex);
                        model.InstantiateDisplayRuleGroup(equipmentDisplayRuleGroup, ItemIndex.None, RoR2Content.Equipment.Jetpack.equipmentIndex);
                    }
                }
            }
            orig(self);
            if (model != null)
            {
                DontDestroyIfJetpack storage = model.GetComponent<DontDestroyIfJetpack>();
                if (!storage)
                {
                    storage = model.gameObject.AddComponent<DontDestroyIfJetpack>();
                }
                storage.characterModel = model;
                storage.Setup();
            }
        }

        private static void BugWingsAlways_OnDisable(On.RoR2.JetpackController.orig_OnDisable orig, JetpackController self)
        {
            orig(self);
            if (self.targetBody && self.targetBody.isPlayerControlled && self.targetBody.inventory.currentEquipmentIndex != RoR2Content.Equipment.Jetpack.equipmentIndex)
            {
                CharacterModel model = self.targetBody.modelLocator.modelTransform.GetComponent<CharacterModel>();
                if (model.itemDisplayRuleSet)
                {
                    DontDestroyIfJetpack storage = model.GetComponent<DontDestroyIfJetpack>();
                    if (storage)
                    {
                        storage.UnSetup();
                    }
                }
            }
        }


    }
    public class ThisIsASawmarang : MonoBehaviour
    {

    }
    public class DontDestroyIfJetpack : MonoBehaviour
    {
        public CharacterModel characterModel;
        public List<GameObject> storedWings;
        public List<CharacterModel.ParentedPrefabDisplay> storedDisplays;
        public EquipmentIndex index;

        public void Setup()
        {

            index = RoR2Content.Equipment.Jetpack.equipmentIndex;
            storedWings = new List<GameObject>();
            storedDisplays = new List<CharacterModel.ParentedPrefabDisplay>();

            for (int i = characterModel.parentedPrefabDisplays.Count - 1; i >= 0; i--)
            {
                if (characterModel.parentedPrefabDisplays[i].equipmentIndex == index)
                {
                    var a = characterModel.parentedPrefabDisplays[i];
                    a.equipmentIndex = EquipmentIndex.None;
                    characterModel.parentedPrefabDisplays[i] = a;
                    storedDisplays.Add(a);
                    storedWings.Add(a.instance);
                }
            }
        }
        public void UnSetup()
        {

            if (characterModel.currentEquipmentDisplayIndex == index)
            {
                for (int i = storedDisplays.Count - 1; i >= 0; i--)
                {
                    var a = storedDisplays[i];
                    a.equipmentIndex = index;
                    storedDisplays[i] = a;
                }
            }
            else
            {
                for (int i = storedDisplays.Count - 1; i >= 0; i--)
                {
                    storedDisplays[i].Undo();
                }
            }
            Destroy(this);

        }

    }
}
