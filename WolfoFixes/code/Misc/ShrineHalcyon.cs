using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace WolfoFixes
{
    public class ShrineHalcyon
    {
        public static void Start()
        {
            On.RoR2.CombatDirector.SpendAllCreditsOnMapSpawns += CombatDirector_SpendAllCreditsOnMapSpawns;
            On.RoR2.CombatDirector.HalcyoniteShrineActivation += CombatDirector_HalcyoniteShrineActivation;
            On.RoR2.HalcyoniteShrineInteractable.IsDraining += HalcyoniteShrineInteractable_IsDraining;

            On.RoR2.HalcyoniteShrineInteractable.Awake += MoreValuesForClients;
            On.RoR2.HalcyoniteShrineInteractable.Start += MoreValuesClient2;

            IL.RoR2.PortalSpawner.Start += DelayThunderMessage;

        }

        private static void DelayThunderMessage(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdfld("RoR2.PortalSpawner", "spawnPreviewMessageToken")))
            {
                c.EmitDelegate<System.Func<string, string>>((target) =>
                {
                    if (ClassicStageInfo.instance && !string.IsNullOrEmpty(target))
                    {
                        ClassicStageInfo.instance.StartCoroutine(Delayed_ChatBroadcast(target, 0.9f));
                        return null;
                    }
                    return target;
                });
                Debug.Log("IL Found: Delay Thunder Message");
            }
            else
            {
                Debug.LogWarning("IL Failed: Delay Thunder Message");
            }
        }

        public static IEnumerator Delayed_ChatBroadcast(string token, float delay)
        {
            yield return new WaitForSeconds(delay);
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage
            {
                baseToken = token
            });
            yield break;
        }

        private static void HalcyoniteShrineInteractable_IsDraining(On.RoR2.HalcyoniteShrineInteractable.orig_IsDraining orig, HalcyoniteShrineInteractable self, bool drainingActive)
        {
            if (!NetworkServer.active)
            {
                //STFU
                return;
            }
            orig(self, drainingActive);
        }

        private static void CombatDirector_SpendAllCreditsOnMapSpawns(On.RoR2.CombatDirector.orig_SpendAllCreditsOnMapSpawns orig, CombatDirector self, Transform mapSpawnTarget)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("CombatDirector_SpendAllCreditsOnMapSpawns | This isn't meant to run on Client, Gearbox Software");
                return;
            }
            orig(self, mapSpawnTarget);
        }

        private static void CombatDirector_HalcyoniteShrineActivation(On.RoR2.CombatDirector.orig_HalcyoniteShrineActivation orig, CombatDirector self, float monsterCredit, DirectorCard chosenDirectorCard, int difficultyLevel, Transform shrineTransform)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("CombatDirector_HalcyoniteShrineActivation | This isn't meant to run on Client, Gearbox Software");
                return;
            }
            orig(self, monsterCredit, chosenDirectorCard, difficultyLevel, shrineTransform);
        }

        private static void MoreValuesForClients(On.RoR2.HalcyoniteShrineInteractable.orig_Awake orig, HalcyoniteShrineInteractable self)
        {
            orig(self);
            if (!NetworkServer.active)
            {

                self.lowGoldCost = Run.instance.GetDifficultyScaledCost(self.lowGoldCost);
                self.midGoldCost = Run.instance.GetDifficultyScaledCost(self.midGoldCost);
                self.maxGoldCost = Run.instance.GetDifficultyScaledCost(self.maxGoldCost);

            }
        }
        private static void MoreValuesClient2(On.RoR2.HalcyoniteShrineInteractable.orig_Start orig, HalcyoniteShrineInteractable self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                if (self.purchaseInteraction.Networkcost > 0)
                {
                    //Base value set in LGT. Ig hope no one else modifies it
                    //Or that Gbx listens and makes it networked
                    self.goldDrainValue = self.purchaseInteraction.Networkcost;
                }
                self.goldDrainValue = Run.instance.GetDifficultyScaledCost(self.goldDrainValue);
                GoldSiphonNearbyBodyController controller = self.transform.GetChild(1).GetComponent<GoldSiphonNearbyBodyController>();
                controller.goldDrainValue = self.goldDrainValue;
            }
        }


    }

}