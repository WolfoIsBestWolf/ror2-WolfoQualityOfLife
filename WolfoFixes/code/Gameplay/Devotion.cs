using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    public class Devotion
    {

        public static void Start()
        {
            //IL failed to work so I'll do this stupider version

            On.RoR2.DevotionInventoryController.OnDevotionArtifactDisabled += Fix_EliteListBeingBlank;

            On.RoR2.CharacterAI.LemurianEggController.CreateItemTakenOrb += Fix_NullrefWhenOrb;


            On.RoR2.DevotionInventoryController.UpdateAllMinions += AddItemToAllFirst;
            IL.RoR2.DevotionInventoryController.UpdateMinionInventory += RemoveNormalItemGiving;


            IL.RoR2.DevotionInventoryController.UpdateMinionInventory += CheckIfNullBody;
            On.DevotedLemurianController.OnDevotedBodyDead += CheckIfInventoryNull;
            //On.RoR2.DevotionInventoryController.UpdateMinionInventory += FixBodyComponentsSometimesJustNotBeingThere;

            On.RoR2.DevotionInventoryController.GetOrCreateDevotionInventoryController += CheckIfRandomlyPlayerMasterNull;

            On.RoR2.DevotionInventoryController.EvolveDevotedLumerian += FixEvolveWithoutBody;

            //This fucking gets removed/added when it should be constant
            Run.onRunDestroyGlobal += DevotionInventoryController.OnRunDestroy;

            if (WConfig.cfgDevotionSpareDroneParts.Value)
            {
                GameObject DevotedLemurian = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBody.prefab").WaitForCompletion();
                GameObject DevotedLemurianElder = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBruiserBody.prefab").WaitForCompletion();

                DevotedLemurian.GetComponent<CharacterBody>().bodyFlags &= CharacterBody.BodyFlags.Devotion;
                DevotedLemurianElder.GetComponent<CharacterBody>().bodyFlags &= CharacterBody.BodyFlags.Devotion;

            }

            On.EntityStates.LemurianMonster.SpawnState.OnEnter += SoundBugged;
            On.EntityStates.LemurianBruiserMonster.SpawnState.OnEnter += NoSoundEven;
        }

        private static void NoSoundEven(On.EntityStates.LemurianBruiserMonster.SpawnState.orig_OnEnter orig, EntityStates.LemurianBruiserMonster.SpawnState self)
        {
            orig(self);
            if (self.characterBody.master.GetComponent<DevotedLemurianController>())
            {
                Util.PlaySound(EntityStates.LemurianMonster.SpawnState.devotionHissSoundString, self.gameObject);
                Util.PlaySound(EntityStates.LemurianMonster.SpawnState.devotionSpawnSoundString, self.gameObject);
            }
        }

        private static void SoundBugged(On.EntityStates.LemurianMonster.SpawnState.orig_OnEnter orig, EntityStates.LemurianMonster.SpawnState self)
        {
            orig(self);
            if (self.characterBody.master.GetComponent<DevotedLemurianController>())
            {
                Util.PlaySound(EntityStates.LemurianMonster.SpawnState.devotionHissSoundString, self.gameObject);
                Util.PlaySound(EntityStates.LemurianMonster.SpawnState.devotionSpawnSoundString, self.gameObject);
            }
        }

        private static void FixEvolveWithoutBody(On.RoR2.DevotionInventoryController.orig_EvolveDevotedLumerian orig, DevotionInventoryController self, DevotedLemurianController devotedLemurianController)
        {
            if (devotedLemurianController.LemurianBody == null)
            {
                switch (devotedLemurianController.DevotedEvolutionLevel)
                {
                    case 1:
                        devotedLemurianController.LemurianInventory.SetEquipmentIndex(DevotionInventoryController.lowLevelEliteBuffs[Random.Range(0, DevotionInventoryController.lowLevelEliteBuffs.Count)]);
                        return;
                    case 2:
                        devotedLemurianController.LemurianInventory.SetEquipmentIndex(EquipmentIndex.None);
                        devotedLemurianController._lemurianMaster.TransformBody("DevotedLemurianBruiserBody");
                        return;
                    case 3:
                        devotedLemurianController.LemurianInventory.SetEquipmentIndex(DevotionInventoryController.highLevelEliteBuffs[Random.Range(0, DevotionInventoryController.highLevelEliteBuffs.Count)]);
                        return;
                }
                return;
            }
            orig(self, devotedLemurianController);
        }


        private static DevotionInventoryController CheckIfRandomlyPlayerMasterNull(On.RoR2.DevotionInventoryController.orig_GetOrCreateDevotionInventoryController orig, Interactor summoner)
        {
            foreach (DevotionInventoryController devotionInventoryController2 in DevotionInventoryController.InstanceList)
            {
                if (devotionInventoryController2._summonerMaster == null)
                {
                    Object.Destroy(devotionInventoryController2);
                }
            }
            return orig(summoner);
        }

        private static void CheckIfInventoryNull(On.DevotedLemurianController.orig_OnDevotedBodyDead orig, DevotedLemurianController self)
        {
            //?? idk how but it sometimes was
            if (self && self._devotionInventoryController != null)
            {
                orig(self);
            }
        }

        private static void CheckIfNullBody(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
             x => x.MatchLdsfld("RoR2.DevotionInventoryController", "activationSoundEventDef")
            ))
            {
                c.Emit(OpCodes.Ldarg_1);
                c.EmitDelegate<System.Func<GameObject, DevotedLemurianController, GameObject>>((obj, lem) =>
                {
                    if (lem.LemurianBody == null)
                    {
                        return null;
                    }
                    return obj;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : CheckIfNullBody");
            }
        }

        private static void RemoveNormalItemGiving(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchCallvirt("RoR2.Inventory", "GiveItem")))
            {
                c.Prev.OpCode = OpCodes.Ldc_I4_0;
            }
            else
            {
                Debug.LogWarning("IL Failed: FixOneBehindRemoveItemHerel");
            }
        }


        private static void FixBodyComponentsSometimesJustNotBeingThere(On.RoR2.DevotionInventoryController.orig_UpdateMinionInventory orig, DevotionInventoryController self, DevotedLemurianController devotedLemurianController, bool shouldEvolve)
        {
            orig(self, devotedLemurianController, shouldEvolve);
            if (devotedLemurianController.LemurianBody)
            {
                if (devotedLemurianController.LemurianBody.inventory)
                {
                    devotedLemurianController.LemurianBody.OnInventoryChanged();
                }
            }
        }

        private static void AddItemToAllFirst(On.RoR2.DevotionInventoryController.orig_UpdateAllMinions orig, DevotionInventoryController self, bool shouldEvolve)
        {
            //Update the item count of all lemurians first, then copy it. Instead of copying lower counts.
            if (shouldEvolve)
            {
                if (self._summonerMaster)
                {
                    MinionOwnership.MinionGroup minionGroup = MinionOwnership.MinionGroup.FindGroup(self._summonerMaster.netId);
                    if (minionGroup != null)
                    {
                        foreach (MinionOwnership minionOwnership in minionGroup.members)
                        {
                            DevotedLemurianController devotedLemurianController;
                            if (minionOwnership && minionOwnership.GetComponent<CharacterMaster>().TryGetComponent<DevotedLemurianController>(out devotedLemurianController))
                            {
                                self._devotionMinionInventory.GiveItem(devotedLemurianController.DevotionItem, 1);
                            }
                        }
                    }
                }
            }
            orig(self, shouldEvolve);
            self.StartCoroutine(FixItemComponentsBeingDeleted(self));
        }

        public static IEnumerator FixItemComponentsBeingDeleted(DevotionInventoryController self)
        {
            yield return new WaitForSeconds(1);
            if (self._summonerMaster)
            {
                MinionOwnership.MinionGroup minionGroup = MinionOwnership.MinionGroup.FindGroup(self._summonerMaster.netId);
                if (minionGroup != null)
                {
                    foreach (MinionOwnership minionOwnership in minionGroup.members)
                    {
                        DevotedLemurianController devotedLemurianController;
                        if (minionOwnership && minionOwnership.GetComponent<CharacterMaster>().TryGetComponent<DevotedLemurianController>(out devotedLemurianController))
                        {
                            if (devotedLemurianController.LemurianBody)
                            {
                                if (devotedLemurianController.LemurianBody.inventory)
                                {
                                    devotedLemurianController.LemurianBody.OnInventoryChanged();
                                }
                            }
                        }
                    }
                }
            }
        }


        private static void Fix_EliteListBeingBlank(On.RoR2.DevotionInventoryController.orig_OnDevotionArtifactDisabled orig, RunArtifactManager runArtifactManager, ArtifactDef artifactDef)
        {
            if (artifactDef == CU8Content.Artifacts.Devotion)
            {
                List<EquipmentIndex> lowLevelEliteBuffs = new List<EquipmentIndex>(DevotionInventoryController.lowLevelEliteBuffs);
                List<EquipmentIndex> highLevelEliteBuffs = new List<EquipmentIndex>(DevotionInventoryController.highLevelEliteBuffs);
                orig(runArtifactManager, artifactDef);
                DevotionInventoryController.lowLevelEliteBuffs = lowLevelEliteBuffs;
                DevotionInventoryController.highLevelEliteBuffs = highLevelEliteBuffs;
            }
            else
            {
                orig(runArtifactManager, artifactDef);
            }
        }

        private static void Fix_NullrefWhenOrb(On.RoR2.CharacterAI.LemurianEggController.orig_CreateItemTakenOrb orig, RoR2.CharacterAI.LemurianEggController self, Vector3 effectOrigin, GameObject targetObject, ItemIndex itemIndex)
        {
            //Why the fuck do they null this after the artitact is disabled or before it's ever enabled.
            if (!DevotionInventoryController.s_effectPrefab)
            {
                DevotionInventoryController.OnDevotionArtifactEnabled(RunArtifactManager.instance, CU8Content.Artifacts.Devotion);
                RoR2Content.Items.BoostDamage.hidden = true;
                RoR2Content.Items.BoostHp.hidden = true;
            }
            orig(self, effectOrigin, targetObject, itemIndex);
        }



    }
}