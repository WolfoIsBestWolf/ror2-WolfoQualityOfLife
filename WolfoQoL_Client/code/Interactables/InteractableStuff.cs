using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQoL_Client
{

    public class MakePriceTransform : MonoBehaviour
    {
        public bool disableRotation;
        public Vector3 holoPos;
        public Vector3 holoRot;
        public void Awake()
        {
            GameObject LockboxHoloPivot = new GameObject("PriceTransform");
            LockboxHoloPivot.transform.SetParent(transform, false);
            LockboxHoloPivot.transform.localPosition = holoPos;
            RoR2.Hologram.HologramProjector hologram = GetComponent<RoR2.Hologram.HologramProjector>();
            if (hologram == null)
            {
                hologram = this.gameObject.AddComponent<RoR2.Hologram.HologramProjector>();
            }
            hologram.disableHologramRotation = disableRotation;
            LockboxHoloPivot.transform.localEulerAngles = holoRot;
            hologram.hologramPivot = LockboxHoloPivot.transform;
        }
    }


    public class InteractableStuff
    {
        public static void Start()
        {
            On.RoR2.TeleporterInteraction.AddShrineStack += MountainStack_Host;

            On.RoR2.ShrineColossusAccessBehavior.RpcUpdateInteractionClients += ShrineShapingExtras_Client;
            On.RoR2.ShrineColossusAccessBehavior.ReviveAlliedPlayers += ShrineShapingExtras_Host;

            PriceTransformStuff();
            //Meditate7thGateVFX
        }

        private static void ShrineShapingExtras_Host(On.RoR2.ShrineColossusAccessBehavior.orig_ReviveAlliedPlayers orig, ShrineColossusAccessBehavior self)
        {
            //If there is no way to check if "justSpawned, justRevived, midSpawnAnimation"
            //Then just do it here ig
            try
            {
                ShrineShapingExtras();
            }
            catch (Exception e)
            {
                //Not risking it
                Debug.LogError(e);
                orig(self);
                return;
            }
            orig(self);
        }

        public static string NameOfAllDeadPlayers(out int anyDead)
        {
            string dead = "";
            anyDead = 0;
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
            {
                if (player.master.lostBodyToDeath)
                {
                    anyDead++;
                    dead += Util.EscapeRichTextForTextMeshPro(player.networkUser.userName) + "'s";
                }
            }
            return dead;
        }

        private static void RezEffectOnDead(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
             x => x.MatchLdsfld("RoR2.RoR2Content/Buffs", "Immune")
            ))
            {
                c.EmitDelegate<Func<CharacterBody, CharacterBody>>((body) =>
                {
                    if (body)
                    {
                        //Debug.Log(body + " : " + body.transform.position);
                        GameObject gameObject = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect");
                        EffectManager.SpawnEffect(gameObject, new EffectData
                        {
                            origin = body.transform.position,
                            rotation = body.transform.rotation
                        }, true);
                        Util.PlaySound("Play_item_proc_extraLife", body.gameObject);
                    }
                    return body;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed : bro they forgot to make a rez effect2");
            }
        }

        internal static void PriceTransformStuff()
        {
            //Price Tag Transform things
            GameObject PrefabShrineCleanse = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanse.prefab").WaitForCompletion();
            GameObject PrefabShrineCleanseSand = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSandy Variant.prefab").WaitForCompletion();
            GameObject PrefabShrineCleanseSnow = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSnowy Variant.prefab").WaitForCompletion();
            //GameObject VoidSuppressor = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VoidSuppressor/VoidSuppressor.prefab").WaitForCompletion();

            GameObject PrefabLockbox = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion();
            GameObject PrefabLockboxVoid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion();
            GameObject PrefabVendingMachine = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion();

            PrefabShrineCleanse.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
            PrefabShrineCleanseSand.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;
            PrefabShrineCleanseSnow.AddComponent<RoR2.Hologram.HologramProjector>().disableHologramRotation = true;

            PrefabShrineCleanse.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);
            PrefabShrineCleanseSand.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);
            PrefabShrineCleanseSnow.transform.GetChild(1).localPosition = new Vector3(0.0f, 0.75f, -1.65f);

            PrefabShrineCleanse.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanse.transform.GetChild(1);
            PrefabShrineCleanseSand.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanseSand.transform.GetChild(1);
            PrefabShrineCleanseSnow.GetComponent<RoR2.Hologram.HologramProjector>().hologramPivot = PrefabShrineCleanseSnow.transform.GetChild(1);

            PrefabLockbox.AddComponent<RoR2.Hologram.HologramProjector>();
            PrefabLockboxVoid.AddComponent<RoR2.Hologram.HologramProjector>();
            PrefabVendingMachine.AddComponent<RoR2.Hologram.HologramProjector>();
 
            PrefabLockbox.AddComponent<MakePriceTransform>().holoPos = new Vector3(0f, 1.6f, 0f);
            PrefabLockboxVoid.AddComponent<MakePriceTransform>().holoPos = new Vector3(0f, 2f, 0f);
            PrefabVendingMachine.AddComponent<MakePriceTransform>().holoPos = new Vector3(0.0f, 4.5f, 0f);
     
        }

        private static void CharacterMaster_RespawnExtraLifeShrine(On.RoR2.CharacterMaster.orig_RespawnExtraLifeShrine orig, CharacterMaster self)
        {
            orig(self);
            if (NetworkServer.active)
            {
                if (self.bodyInstanceObject)
                {
                    EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect"), new EffectData
                    {
                        origin = self.deathFootPosition,
                        rotation = self.bodyInstanceObject.transform.rotation
                    }, true);
                }
            }

        }


        public static void ShrineShapingExtras()
        {
            List<string> deadPeopleNames = new List<string>();
            int deadPeople = 0;
            bool youAreDead = false;

            if (WConfig.cfgMessagesShrineRevive.Value)
            {
                Chat.AddMessage(Language.GetString("SHRINE_REVIVE_USE_MESSAGE"));
            }
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
            {
                if (!player.isConnected)
                {
                    continue; //?
                }
                CharacterMaster master = player.master;
                if (!master)
                {
                    continue;
                }
                var body = master.GetBodyObject();

                Debug.Log(master.lostBodyToDeath + " " + body);


                //Extra SFX & VFX
                if (body)
                {
                    Util.PlaySound("Play_obj_portalShaping_activate", body);

                    EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ShrineUseEffect"), new EffectData
                    {
                        origin = body.transform.position,
                        rotation = Quaternion.identity,
                        scale = 1f,
                        color = new Color(0.3137255f, 0.8392157f, 0.7529412f)
                    }, false);
                }
                else
                {
                    master.onBodyStart += SpawnEffectsOnRevive;
                }

                if (player.master.lostBodyToDeath || body == null)
                {
                    if (master.hasAuthority)
                    {
                        youAreDead = true;
                    }
                    else
                    {
                        deadPeopleNames.Add(Util.EscapeRichTextForTextMeshPro(player.networkUser.userName));
                    }
                    deadPeople++;
                }
            }
            if (deadPeople > 0)
            {
                string token = "";
                string deadNames = "";
                for (int i = 0; i < deadPeopleNames.Count; i++)
                {
                    if (i == 0 && !youAreDead)
                    {
                        //If first, just name
                        //If you, then skip
                        deadNames += deadPeopleNames[i];
                    }
                    else if (i + 1 == deadPeopleNames.Count)
                    {
                        //If is last && not first
                        deadNames += " & " + deadPeopleNames[i];
                    }
                    else
                    {
                        //If middle
                        deadNames += ", " + deadPeopleNames[i];
                    }
                }
                if (deadPeople == 1 && youAreDead)
                {
                    token = "SHRINE_REVIVE_MESSAGE_DEAD_YOU";
                    token = Language.GetString(token);
                }
                else if (youAreDead)
                {
                    token = "SHRINE_REVIVE_MESSAGE_DEAD_YOUAND";
                    token = string.Format(Language.GetString(token), deadNames);
                }
                else
                {
                    token = "SHRINE_REVIVE_MESSAGE_DEAD";
                    token = string.Format(Language.GetString(token), deadNames);
                }
                Chat.AddMessage(token);
            }
        }

        private static void ShrineShapingExtras_Client(On.RoR2.ShrineColossusAccessBehavior.orig_RpcUpdateInteractionClients orig, ShrineColossusAccessBehavior self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                //This does also run on Host, we just need to do it before revives take place.
                ShrineShapingExtras();
            }
        }

        private static void SpawnEffectsOnRevive(CharacterBody body)
        {
            Util.PlaySound("Play_obj_portalShaping_activate", body.gameObject);

            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ShrineUseEffect"), new EffectData
            {
                origin = body.transform.position,
                rotation = Quaternion.identity,
                scale = 1f,
                color = new Color(0.3137255f, 0.8392157f, 0.7529412f)
            }, false);
            EffectManager.SpawnEffect(LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/HippoRezEffect"), new EffectData
            {
                origin = body.transform.position,
                rotation = body.transform.rotation
            }, false);
            body.master.onBodyStart -= SpawnEffectsOnRevive;
        }


        private static void MountainStack_Host(On.RoR2.TeleporterInteraction.orig_AddShrineStack orig, TeleporterInteraction self)
        {
            orig(self);
            ShrineBoss_IconStack.AddShrineIcon_Static();
        }
    }


    public class ShrineBoss_IconStack : MonoBehaviour
    {
        public int shrineStacks;

        public static void AddShrineIcon_Static()
        {
            if (!TeleporterInteraction.instance)
            {
                return;
            }
            var boss = TeleporterInteraction.instance.GetComponent<ShrineBoss_IconStack>();
            if (!boss)
            {
                boss = TeleporterInteraction.instance.gameObject.AddComponent<ShrineBoss_IconStack>();
            }
            boss.AddShrineIcon();
        }

        public void AddShrineIcon()
        {
            shrineStacks++;
            Debug.Log("WQoL ExtraMountainIcon");
            if (shrineStacks == 1)
            {
                GameObject tempbossicon = TeleporterInteraction.instance.bossShrineIndicator;
                GameObject tempbossiconclone = GameObject.Instantiate(tempbossicon, tempbossicon.transform);

                GameObject.Destroy(tempbossicon.GetComponent<MeshRenderer>());
                GameObject.Destroy(tempbossicon.GetComponent<Billboard>());
                tempbossicon.transform.localPosition = new Vector3(0, 0, 6);
                tempbossiconclone.transform.localScale = new Vector3(1, 1, 1);
                tempbossiconclone.transform.localPosition = new Vector3(0, 0, 0);
                tempbossiconclone.SetActive(true);
            }
            if (shrineStacks > 1)
            {
                GameObject tempbossicon = TeleporterInteraction.instance.bossShrineIndicator.transform.GetChild(0).gameObject;
                GameObject tempbossiconclone = GameObject.Instantiate(tempbossicon, tempbossicon.transform.parent);
                tempbossiconclone.transform.localPosition = new Vector3(0, (shrineStacks - 1), 0);
            }
        }




    }

}
