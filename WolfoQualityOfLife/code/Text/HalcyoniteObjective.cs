using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace WolfoQualityOfLife
{
    public class HalcyoniteObjective
    {
        public static void Start()
        { 
            bool otherMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Gorakh.NoMoreMath");
            if (!otherMod && WConfig.cfgChargeHalcyShrine.Value)
            {
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState.OnEnter += AddObjective;
                On.RoR2.HalcyoniteShrineInteractable.StoreDrainValue += UpdateChargeAmount;
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality.OnEnter += AddKillObjective;
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished.OnEnter += ClearObjective;
            }

        }
 
        private static void ClearObjective(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished self)
        {
            orig(self);
            Object.Destroy(self.gameObject.GetComponent<GenericObjectiveProvider>());
        }

        private static void AddKillObjective(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality self)
        {
            orig(self);
            RoR2.Chat.SendBroadcastChat(new UpdateHalcyoniteObjective
            {
                halcyShrine = self.gameObject,
                chargePercent = 100,
                killNow = true
            });
        }

        private static void AddObjective(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState self)
        {
            orig(self);
            var obj = self.gameObject.GetComponent<GenericObjectiveProvider>();
            if (!obj)
            {
                self.gameObject.AddComponent<GenericObjectiveProvider>().objectiveToken = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), 0);
            }
        }

        private static void UpdateChargeAmount(On.RoR2.HalcyoniteShrineInteractable.orig_StoreDrainValue orig, HalcyoniteShrineInteractable self, int value)
        {
            int chargeFracPrev = self.goldDrained * 100 / self.maxGoldCost;
            orig(self, value);
            int chargeFracNew = self.goldDrained * 100 / self.maxGoldCost;
            //Debug.Log(chargeFracPrev +"  " +chargeFracNew);
            if (chargeFracNew > chargeFracPrev)
            {
                RoR2.Chat.SendBroadcastChat(new UpdateHalcyoniteObjective
                {
                    halcyShrine = self.gameObject,
                    chargePercent = chargeFracNew,
                    killNow = false
                });
            }
        }


        public class UpdateHalcyoniteObjective : RoR2.ChatMessageBase
        {
            public GameObject halcyShrine;
            public int chargePercent;
            public bool killNow;

            public override string ConstructChatString()
            {
                GenericObjectiveProvider objective = halcyShrine.GetComponent<GenericObjectiveProvider>();
                if (objective)
                {
                    if (killNow)
                    {
                        Object.Destroy(objective);
                        halcyShrine.AddComponent<GenericObjectiveProvider>().objectiveToken = Language.GetString("OBJECTIVE_KILL_HALCSHRINE");
                    }
                    else
                    {
                        objective.objectiveToken = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), chargePercent);
                    }
                }          
                return null;
            }

            

            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(halcyShrine);
                writer.Write(chargePercent);
                writer.Write(killNow);
            }

            public override void Deserialize(NetworkReader reader)
            {
                base.Deserialize(reader);
                halcyShrine = reader.ReadGameObject();
                chargePercent = (int)reader.ReadInt32();
                killNow = reader.ReadBoolean();
            }

        }
    }
}
