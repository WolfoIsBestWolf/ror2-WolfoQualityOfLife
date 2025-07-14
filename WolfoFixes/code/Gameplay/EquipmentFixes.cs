using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{

    internal class EquipmentFixes
    {
        public static void Start()
        {
            Addressables.LoadAssetAsync<GameObject>(key: "9ca7d392fa3bb444b827d475b36b9253").WaitForCompletion().AddComponent<ThisIsASawmarang>();
            Addressables.LoadAssetAsync<EquipmentDef>(key: "f2ddbb7586240e648945ad494ebe3984").WaitForCompletion().cooldown = 0; //Why does this have a cooldown it just fucks you up when buying shops quickly
            IL.RoR2.GlobalEventManager.ProcessHitEnemy += FixSawmarang;
 
            On.RoR2.Util.HealthComponentToTransform += FixTwisteds_NotWorkingOnPlayers;

            On.RoR2.JetpackController.SetupWings += BugWingsAlways_SetupWings;
            On.RoR2.JetpackController.OnDisable += BugWingsAlways_OnDisable;
 
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
 
        private static Transform FixTwisteds_NotWorkingOnPlayers(On.RoR2.Util.orig_HealthComponentToTransform orig, HealthComponent healthComponent)
        {
            if (healthComponent && healthComponent.body && healthComponent.body.mainHurtBox)
            {
                return healthComponent.body.mainHurtBox.transform;
            }
            return orig(healthComponent);
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
            if (self.targetBody && self.targetBody.isPlayerControlled)
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
    internal class ThisIsASawmarang : MonoBehaviour
    {

    }

    internal class DontDestroyIfJetpack : MonoBehaviour
    {
        public CharacterModel characterModel;
  
        public CharacterModel.ParentedPrefabDisplay storedDisplay;
        public EquipmentIndex index;

        public void Setup()
        {
            index = RoR2Content.Equipment.Jetpack.equipmentIndex;
 
   
            for (int i = characterModel.parentedPrefabDisplays.Count - 1; i >= 0; i--)
            {
                if (characterModel.parentedPrefabDisplays[i].equipmentIndex == index)
                {
                    storedDisplay = characterModel.parentedPrefabDisplays[i];
                    characterModel.parentedPrefabDisplays.RemoveAt(i);
                    break;
                }
            }
 
        }
        public void UnSetup()
        {

            if (characterModel.currentEquipmentDisplayIndex == index)
            {
                characterModel.parentedPrefabDisplays.Add(storedDisplay);
            }
            else
            {
                storedDisplay.Undo();
            }
        }

    }
}
