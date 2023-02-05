/*
 * 
 * 
 * 
 * 
 *             On.EntityStates.Bison.SpawnState.OnEnter += (orig, self) =>
            {
               EntityStates.Bison.SpawnState.spawnEffectPrefab;
               orig(self);
            };

 * 
 * 
 * 
 * 
 * 
 
        /*
        public static void MercStepMethod(On.EntityStates.BasicMeleeAttack.orig_AuthorityFireAttack orig, EntityStates.BasicMeleeAttack self)
        {
            orig(self);
            //Debug.LogWarning(self.ToString());
            if (self.ToString().Equals("EntityStates.Merc.Weapon.GroundLight2"))
            {
                self.SetFieldValue("duration", self.baseDuration);
                //On.EntityStates.BasicMeleeAttack.AuthorityFireAttack -= MercStepMethod;
            }
        }
        */
/*

*
 * 
 * 
            On.RoR2.UI.PieChartLegend.Rebuild += (orig, self) =>
            {
                var stripAllocator = self.GetFieldValue<RoR2.UI.UIElementAllocator<ChildLocator>>("stripAllocator");

                if (!self.source || !stripAllocator.containerTransform || !stripAllocator.elementPrefab)
                {
                    stripAllocator.AllocateElements(0);
                    return;
                }
                int num = 0;
                for (int i = 0; i < self.source.sliceCount; i++)
                {
                    if (self.source.GetSliceInfo(i).weight / self.source.totalSliceWeight > self.source.minimumRequiredWeightToDisplay || num < 20)
                    {
                        num++;
                    }
                }
                stripAllocator.AllocateElements(num);
                System.Collections.ObjectModel.ReadOnlyCollection<ChildLocator> elements = stripAllocator.elements;
                int j = 0;
                int num2 = Math.Min(num, elements.Count);
                int num3 = 0;
                while (j < num2)
                {
                    RoR2.UI.PieChartMeshController.SliceInfo sliceInfo = self.source.GetSliceInfo(j);
                    if (self.source.GetSliceInfo(j).weight / self.source.totalSliceWeight > self.source.minimumRequiredWeightToDisplay || num3 < 20)
                    {
                        num3++;
                        ChildLocator childLocator = elements[j];
                        Transform transform = childLocator.FindChild("ColorBox");
                        UnityEngine.UI.Graphic graphic = (transform != null) ? transform.GetComponent<UnityEngine.UI.Graphic>() : null;
                        Transform transform2 = childLocator.FindChild("Label");
                        TMPro.TMP_Text tmp_Text = (transform2 != null) ? transform2.GetComponent<TMPro.TMP_Text>() : null;
                        if (graphic)
                        {
                            graphic.color = sliceInfo.color;
                        }
                        if (tmp_Text)
                        {
                            tmp_Text.SetText(sliceInfo.tooltipContent.GetTitleText());
                        }
                    }
                    j++;
                }
            };

 * 
 * 
 * 
 * 
 * 
 * 
 * 
using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using EntityStates;

namespace SlightlyFasterInteractables
{


    public class PrinterStupid : BaseState
    {
        public static bool HasDroppedDroplet = true;


        public static void Awake()
        {

            Debug.LogWarning("Read");

            On.EntityStates.Duplicator.Duplicating.DropDroplet += (orig, self) =>
            {
                orig(self);
               
                if (NetworkServer.active)
                {   
                    if (self.outer.GetComponent<PurchaseInteraction>().Networkavailable == false)
                    {
                        HasDroppedDroplet = false;
                    }

                    self.outer.GetComponent<PurchaseInteraction>().Networkavailable = true;

                    if (self.outer.GetComponent<PurchaseInteraction>().Networkavailable == true && HasDroppedDroplet == false)
                    {
                        HasDroppedDroplet = true;
                    }

                }
            };


            On.EntityStates.Duplicator.Duplicating.BeginCooking += (orig, self) =>
            {
                
                Debug.LogWarning(HasDroppedDroplet);
                Debug.LogWarning("He do be cooking");

                if (HasDroppedDroplet == true && self.outer.GetComponent<PurchaseInteraction>().Networkavailable == false)
                {
                    HasDroppedDroplet = false;
                    Debug.LogWarning(HasDroppedDroplet);

                    //base.GetModelAnimator();

                    Animator animtemp = self.outer.GetComponentInChildren<UnityEngine.Animator>();
                    animtemp.SetFloat("Emote.playbackRate", 0.1f);

                    EntityStates.EntityState.PlayAnimationOnAnimator(animtemp, "Body", "Cook", "", 0.2f);
                }
                //orig(self);
            };

            On.EntityStates.Duplicator.Duplicating.OnEnter += (orig, self) =>
            {
                Debug.LogWarning("Logsex444444444444444444444444444444444444444444444444");
                orig(self);
            };



            }
    
    }
}
*/
/*
using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using EntityStates;

namespace SlightlyFasterInteractables
{


    public class PrinterStupid : RoR2.DotController
    {
        public static bool HasDroppedDroplet = true;


        public void Awake()
        {

            DotController.DotDef 

            Debug.LogWarning("Read");

            On.EntityStates.Duplicator.Duplicating.DropDroplet += (orig, self) =>
            {
                orig(self);
               
                if (NetworkServer.active)
                {   
                    if (self.outer.GetComponent<PurchaseInteraction>().Networkavailable == false)
                    {
                        HasDroppedDroplet = false;
                    }

                    self.outer.GetComponent<PurchaseInteraction>().Networkavailable = true;

                    if (self.outer.GetComponent<PurchaseInteraction>().Networkavailable == true && HasDroppedDroplet == false)
                    {
                        HasDroppedDroplet = true;
                    }

                }
            };


            On.EntityStates.Duplicator.Duplicating.BeginCooking += (orig, self) =>
            {
                
                Debug.LogWarning(HasDroppedDroplet);
                Debug.LogWarning("He do be cooking");

                if (HasDroppedDroplet == true && self.outer.GetComponent<PurchaseInteraction>().Networkavailable == false)
                {
                    HasDroppedDroplet = false;
                    Debug.LogWarning(HasDroppedDroplet);

                    //base.GetModelAnimator();

                    Animator animtemp = self.outer.GetComponentInChildren<UnityEngine.Animator>();
                    animtemp.SetFloat("Emote.playbackRate", 0.1f);

                    EntityStates.EntityState.PlayAnimationOnAnimator(animtemp, "Body", "Cook", "", 0.2f);
                }
                //orig(self);
            };

            On.EntityStates.Duplicator.Duplicating.OnEnter += (orig, self) =>
            {
                Debug.LogWarning("Logsex444444444444444444444444444444444444444444444444");
                orig(self);
            };



            }
    
    }
}



















            /*

            
                GameObject printerevent = Resources.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, WhiteToGreen");

                PurchaseEvent sex = printerevent.GetComponent<RoR2.PurchaseInteraction>().onPurchase;


            Debug.LogWarning(sex.GetPersistentMethodName(0));
            Debug.LogWarning(sex.GetPersistentMethodName(1));
            //Debug.LogWarning(sex.GetValidMethodInfo(sex, "CallDelayed", float);


            //R2API.Utils.Reflection.SetFieldValue<Single>(sex, "Arg_1", 1);

            //R2API.Utils.Reflection.SetFieldValue<float>(sex, "CallDelayed", 0);
            //R2API.Utils.Reflection.SetFieldValue<Single>(sex, "CallDelayed", 0);



            /*

            sex.AddListener(PingInfo);


            //sex.AddListener()
            sex2 += RoR2.EntityLogic.DelayedEvent.CallDelayed(4f);

            //Resources.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild").GetComponent<RoR2.EntityLogic.DelayedEvent>();

            sex2

            RoR2.EntityLogic.DelayedEvent.CallDelayed(4f);
            //RoR2.PurchaseInteraction.SetAvailable();

            Action(RoR2.EntityLogic.DelayedEvent.CallDelayed());

            

            sex2 += Resources.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild").GetComponent<RoR2.EntityLogic.DelayedEvent>().CallDelayed;


            

            sex.AddListener(RoR2.EntityLogic.DelayedEvent.CallDelayed);

            sex2 = new UnityAction(sexprintder.CallDelayed);

            UnityEvent sexprinterex = sexprintder.action;

            //sex2 += sexprintder.CallDelayed(1f);

            sexprinterex.RemoveListener(sexprintder.CallDelayed(1f));


            RoR2.EntityLogic.DelayedEvent sexprintder = Resources.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild").GetComponent<RoR2.EntityLogic.DelayedEvent>();
            UnityEvent<float> sex;
            sex.AddListener(sexprintder.CallDelayed);
            sexprintder.action = sex;
            */


//PrinterStupid.Awake();

/*
On.EntityStates.Duplicator.Duplicating.BeginCooking += (orig, self) =>
{
    Debug.LogWarning(HasDroppedDroplet);
    Debug.LogWarning("He do be cooking");

    if (HasDroppedDroplet == true && self.outer.GetComponent<PurchaseInteraction>().Networkavailable == false)
    {
        HasDroppedDroplet = false;
        Debug.LogWarning(HasDroppedDroplet);
        self.PlayAnimation("Body", "Cook");
    }
    //orig(self);
};
*/

/*
On.EntityStates.Duplicator.Duplicating.FixedUpdate += (orig, self) =>
{
    Debug.LogWarning(typeof(EntityStates.EntityState).GetFieldValue<float>("fixedAge"));
    typeof(EntityStates.EntityState).GetFieldValue<float>("fixedAge");
    /*
    self.FixedUpdate();
    if (self.fixedAge >= Duplicating.initialDelayDuration)
    {
        On.EntityStates.Duplicator.Duplicating.BeginCooking += (orig2, self2) => { orig(self); };

    }
    if (self.fixedAge >= Duplicating.initialDelayDuration + Duplicating.timeBetweenStartAndDropDroplet)
    {
        On.EntityStates.Duplicator.Duplicating.DropDroplet();
    }


};
*/


/*
EntityStates.Duplicator.Duplicating.timeBetweenStartAndDropDroplet = 0f;
EntityStates.Duplicator.Duplicating.initialDelayDuration = 0f;


*/

/*
GameObject SoupWhiteGreen = Resources.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, WhiteToGreen");
GameObject SoupGreenRed = Resources.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, GreenToRed Variant");
GameObject SoupRedWhite = Resources.Load<GameObject>("Prefabs/networkedobjects/LunarCauldron, RedToWhite Variant");

GameObject CleanseShrine = Resources.Load<GameObject>("Prefabs/networkedobjects/shrines/ShrineCleanse");
GameObject Test = Resources.Load<GameObject>("Prefabs/networkedobjects/chest/LunarShopTerminal");

PurchaseInteraction PurchaseSoupWhiteGreen = SoupWhiteGreen.GetComponent<PurchaseInteraction>();
PurchaseInteraction PurchaseSoupGreenRed = SoupGreenRed.GetComponent<PurchaseInteraction>();
PurchaseInteraction PurchaseSoupRedWhite = SoupRedWhite.GetComponent<PurchaseInteraction>();

PurchaseInteraction PurchaseCleanseShrine = CleanseShrine.GetComponent<PurchaseInteraction>();
PurchaseInteraction PurchaseTest = Test.GetComponent<PurchaseInteraction>();
*/


//MethodInfo sex = UnityEvent.FindMethod_Impl("DropPickup", CauldronRW);
//UnityEngine.Events.PersistentCall.RegisterPersistentListener(CauldronRW, "DropPickup");


