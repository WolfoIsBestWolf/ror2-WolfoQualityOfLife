using MonoMod.Cil;
using RoR2;
using RoR2.Hologram;
using System;
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
            HologramProjector hologram = GetComponent<HologramProjector>();
            if (hologram == null)
            {
                hologram = this.gameObject.AddComponent<HologramProjector>();
            }
            hologram.disableHologramRotation = disableRotation;
            LockboxHoloPivot.transform.localEulerAngles = holoRot;
            hologram.hologramPivot = LockboxHoloPivot.transform;

            if (this.TryGetComponent<SpecialObjectAttributes>(out var special))
            {
                special.behavioursToDisable.Add(hologram);
            }
        }
    }


    public static class InteractableVisuals
    {
        public static void Start()
        {
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
                ShrineShapingExtra_VISUALS();
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
                        //WolfoMain.log.LogMessage(body + " : " + body.transform.position);
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
                WQoLMain.log.LogWarning("IL Failed : bro they forgot to make a rez effect2");
            }
        }

        internal static void PriceTransformStuff()
        {
            //Price Tag Transform things
            GameObject PrefabShrineCleanse = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanse.prefab").WaitForCompletion();
            GameObject PrefabShrineCleanseSand = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSandy Variant.prefab").WaitForCompletion();
            GameObject PrefabShrineCleanseSnow = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/ShrineCleanse/ShrineCleanseSnowy Variant.prefab").WaitForCompletion();

            GameObject PrefabLockbox = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/TreasureCache/Lockbox.prefab").WaitForCompletion();
            GameObject PrefabLockboxVoid = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/TreasureCacheVoid/LockboxVoid.prefab").WaitForCompletion();
            GameObject PrefabVendingMachine = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/VendingMachine/VendingMachine.prefab").WaitForCompletion();


            PrefabShrineCleanse.AddComponent<MakePriceTransform>().holoPos = new Vector3(0.0f, 0.75f, -1.65f);
            PrefabShrineCleanseSand.AddComponent<MakePriceTransform>().holoPos = new Vector3(0.0f, 0.75f, -1.65f);
            PrefabShrineCleanseSnow.AddComponent<MakePriceTransform>().holoPos = new Vector3(0.0f, 0.75f, -1.65f);

            PrefabShrineCleanse.GetComponent<MakePriceTransform>().disableRotation = true;
            PrefabShrineCleanseSand.GetComponent<MakePriceTransform>().disableRotation = true;
            PrefabShrineCleanseSnow.GetComponent<MakePriceTransform>().disableRotation = true;

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


        public static void ShrineShapingExtra_VISUALS()
        {

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

                WQoLMain.log.LogMessage(master.lostBodyToDeath + " " + body);


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


            }

        }

        private static void ShrineShapingExtras_Client(On.RoR2.ShrineColossusAccessBehavior.orig_RpcUpdateInteractionClients orig, ShrineColossusAccessBehavior self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                //This does also run on Host, we just need to do it before revives take place.
                ShrineShapingExtra_VISUALS();
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


        /* private static void MountainStack_Host(On.RoR2.TeleporterInteraction.orig_AddShrineStack orig, TeleporterInteraction self)
         {
             orig(self);
             ShrineBoss_IconStack.AddShrineIcon_Static();
         }*/
    }


    /*
    public static class ShrineBoss_IconStack : MonoBehaviour
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
            WolfoMain.log.LogMessage("WQoL ExtraMountainIcon");
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
    */
}
