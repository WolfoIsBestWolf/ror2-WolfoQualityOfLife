using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoQoL_Client
{
    public class Objective_Halcyonite
    {
 
        public static void Start()
        {
            bool otherMod = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Gorakh.NoMoreMath");
            if (!otherMod && WConfig.cfgChargeHalcyShrine.Value)
            {
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState.OnEnter += AddObjective;
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality.OnEnter += Add_KillObjective;
                On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished.OnEnter += Clear_KillObjective;

                IL.RoR2.GoldSiphonNearbyBodyController.DrainGold += Update_Objective;
            }
            On.RoR2.HalcyoniteShrineInteractable.Awake += MoreValuesForClients;
            On.RoR2.HalcyoniteShrineInteractable.Start += MoreValuesClient2;

        }

        private static void MoreValuesClient2(On.RoR2.HalcyoniteShrineInteractable.orig_Start orig, HalcyoniteShrineInteractable self)
        {
            orig(self); 
            if (!NetworkServer.active)
            {
                if (self.purchaseInteraction.Networkcost > 0)
                {
                    //Base value set in LTG. Ig hope no one else modifies it
                    //Or that Gbx listens and makes it networked
                    self.goldDrainValue = self.purchaseInteraction.Networkcost;
                }
                self.goldDrainValue = Run.instance.GetDifficultyScaledCost(self.goldDrainValue);
                GoldSiphonNearbyBodyController controller = self.transform.GetChild(1).GetComponent<GoldSiphonNearbyBodyController>();
                controller.goldDrainValue = self.goldDrainValue;
            }
        }

        private static void AddObjective(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState self)
        {
            //Runs on client
            orig(self);
            var obj = self.gameObject.GetComponent<HalcyoniteShrineObjective>();
            if (!obj)
            {
                obj = self.gameObject.AddComponent<HalcyoniteShrineObjective>();
                obj.Halc = self.parentShrineReference;
                obj.sucker = self.gameObject.GetComponentInChildren<GoldSiphonNearbyBodyController>();
                obj.maxGoldCost = self.parentShrineReference.maxGoldCost;
                obj.objectiveToken = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), 0);
            }
        }
        private static void Add_KillObjective(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteMaxQuality self)
        {
            orig(self);
            HalcyoniteShrineObjective obj = self.gameObject.GetComponent<HalcyoniteShrineObjective>();
            if (obj)
            {
                if (self.parentShrineReference.Networkinteractions >= 3)
                {
                    obj.markCompletedOnRetired = false;
                }
                else
                {
                    obj.objectiveToken = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), 100);
                }
                obj.enabled = false;
                obj.AddKill();
            }
            //HAS TO BEFORE
            //Client Nres and rest doesn't get called
            //Nres if we do before too because parentshrine null
        }
        private static void Clear_KillObjective(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished self)
        {
            //Runs on client
            orig(self);
            HalcyoniteShrineObjective obj = self.gameObject.GetComponent<HalcyoniteShrineObjective>();
            if (obj)
            {
                Object.Destroy(obj.killObjective);
            }
        }


        private static void Update_Objective(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            c.GotoNext(MoveType.After,
            x => x.MatchCall("UnityEngine.Networking.NetworkServer", "get_active"));

            if (c.TryGotoPrev(MoveType.After,
               x => x.MatchLdfld("RoR2.HealthComponent", "body")
               ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<CharacterBody, GoldSiphonNearbyBodyController, CharacterBody>>((body, self) =>
                {
                    if (body)
                    {
                        //HalcyoniteShrineObjective.GoldDiffStatic(body.master);
                        self.parentShrineReference.GetComponent<HalcyoniteShrineObjective>().StoreGold();
                    }
                    return body;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: PlayerPingsIL");
            }

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

    }

    public class HalcyoniteShrineObjective : GenericObjectiveProvider
    {
        public static HalcyoniteShrineObjective instance;
        public HalcyoniteShrineInteractable Halc;
        public GoldSiphonNearbyBodyController sucker;
        public int goldDrained;
        public int fraction;
        public int maxGoldCost = 300;
        public int deducedGoldDrain = 1;

        public int missedGoldDrains = 0;

        public bool doGoldDiff = false;
        public uint moneyPre = 0;
        public CharacterMaster targetGoldDiff = null;

        public void Awake()
        {
            instance = this;
        }

        public static void GoldDiffStatic(CharacterMaster master)
        {
            if (instance && instance.doGoldDiff)
            {
                instance.TryGetGoldDiff(master);
            }
        }
        public void TryGetGoldDiff(CharacterMaster master)
        {

            if (targetGoldDiff == master)
            {
                if (master.money >= moneyPre)
                {
                    Debug.Log("No money was actually drained." + deducedGoldDrain);
                    return;
                }
                //Then second time he gets pre-drained, check money lost
                doGoldDiff = false;
                deducedGoldDrain = (int)(moneyPre - master.money);
                sucker.goldDrainValue = deducedGoldDrain;

                goldDrained += deducedGoldDrain * missedGoldDrains;

                Debug.Log("Gold Drain Deduced is " + deducedGoldDrain);
                Debug.Log("Expected Gold Drain was " + Halc.goldDrainValue);
                Debug.Log("Adding Missing Drains x" + missedGoldDrains);
            }
            else
            {
                //First set target
                targetGoldDiff = master;
                moneyPre = master.money;
            }
        }


        public void StoreGold()
        {
            //goldDrained += deducedGoldDrain;
            goldDrained += Halc.goldDrainValue;
            int newFraction = this.goldDrained * 100 / this.maxGoldCost;
            if (newFraction > 100)
            {
                newFraction = 100;
            }
            if (newFraction > fraction)
            {
                fraction = newFraction;
                objectiveToken = string.Format(Language.GetString("OBJECTIVE_CHARGE_HALCSHRINE"), newFraction);
            }
        }
        public void AddKill()
        {
            Reminders.CompleteObjective(this);
            killObjective = gameObject.AddComponent<GenericObjectiveProvider>();
            killObjective.objectiveToken = Language.GetString("OBJECTIVE_KILL_HALCSHRINE");
            this.enabled = false;
        }
        public GenericObjectiveProvider killObjective;
    }

}
