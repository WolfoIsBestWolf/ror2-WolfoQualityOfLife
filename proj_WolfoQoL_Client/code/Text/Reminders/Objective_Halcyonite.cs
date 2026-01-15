using EntityStates.ShrineHalcyonite;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace WolfoQoL_Client.Reminders
{
    public static class Objective_Halcyonite
    {
        public static void Start()
        {
            On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteBaseState.OnEnter += HalcyoniteObjectives;

            if (WConfig.cfgChargeHalcyShrine.Value && !WQoLMain.NoMoreMathMod)
            {
                IL.RoR2.GoldSiphonNearbyBodyController.DrainGold += Update_Objective;
            }
        }

        private static void HalcyoniteObjectives(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteBaseState.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteBaseState self)
        {
            orig(self);
            if (self is ShrineHalcyoniteActivatedState)
            {
                //Clear Find Objective
                if (TreasureReminder.instance && TreasureReminder.instance.Objective_Halcyon)
                {
                    TreasureReminder.instance.Objective_Halcyon.enabled = false;
                }
            }
            if (WConfig.cfgChargeHalcyShrine.Value && !WQoLMain.NoMoreMathMod)
            {
                if (self is ShrineHalcyoniteActivatedState)
                {
                    //Add Charge Objective
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
                if (self is ShrineHalcyoniteMaxQuality)
                {
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
                    self.outer.transform.GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = false;
                }
                if (self is ShrineHalcyoniteFinished)
                {
                    //Clear Kill Objective
                    HalcyoniteShrineObjective obj = self.gameObject.GetComponent<HalcyoniteShrineObjective>();
                    if (obj)
                    {
                        Object.Destroy(obj.killObjective);
                    }
                }
            }

        }

        private static void ClearFindObj(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteActivatedState self)
        {
            orig(self);
            if (TreasureReminder.instance.Objective_Halcyon)
            {
                TreasureReminder.instance.Objective_Halcyon.enabled = false;
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

            self.outer.transform.GetChild(0).GetChild(0).GetComponent<BoxCollider>().enabled = false;
        }
        private static void Clear_KillObjective(On.EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished.orig_OnEnter orig, EntityStates.ShrineHalcyonite.ShrineHalcyoniteFinished self)
        {
            //Runs on client

        }


        private static void Update_Objective(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            bool a = c.TryGotoNext(MoveType.After,
            x => x.MatchCall("UnityEngine.Networking.NetworkServer", "get_active"));

            if (a && c.TryGotoPrev(MoveType.After,
               x => x.MatchLdfld("RoR2.HealthComponent", "body")
               ))
            {
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<System.Func<CharacterBody, GoldSiphonNearbyBodyController, CharacterBody>>((body, self) =>
                {
                    if (body)
                    {
                        //HalcyoniteShrineObjective.GoldDiffStatic(body.master);
                        self.parentShrineReference.GetComponent<HalcyoniteShrineObjective>()?.StoreGold();
                    }
                    return body;
                });
            }
            else
            {
                Log.LogWarning("IL Failed: Update_Objective");
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

        public void Awake()
        {
            instance = this;
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
            Reminders_Main.CompleteObjective(this);
            killObjective = gameObject.AddComponent<GenericObjectiveProvider>();
            killObjective.objectiveToken = Language.GetString("OBJECTIVE_KILL_HALCSHRINE");
            this.enabled = false;
        }
        public GenericObjectiveProvider killObjective;
    }

}
