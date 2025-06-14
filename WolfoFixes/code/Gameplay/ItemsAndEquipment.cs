﻿using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
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
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += FixSawmarang;
            IL.RoR2.HealthComponent.TakeDamageProcess += FixWarpedEchoE8;

            On.RoR2.Util.HealthComponentToTransform += FixTwisteds_NotWorkingOnPlayers;

            On.RoR2.JetpackController.SetupWings += BugWingsAlways_SetupWings;
            On.RoR2.JetpackController.OnDisable += BugWingsAlways_OnDisable;

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
