using RoR2;

using UnityEngine;


namespace WolfoQoL_Client
{
    public class DuplicatorModel
    {
        public static RuntimeAnimatorController oldAnimationControllerNOT;
        public static void DuplicatorModelChanger()
        {
            //GameObject Duplicator = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Duplicator");
            GameObject DuplicatorLarge = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge");
            GameObject DuplicatorMili = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary");
            //GameObject DuplicatorWild = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild");

            /*if (WConfig.OldModelDupli.Value == true)
            {
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh = FBXDuplicatorOld.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("PickupDisplay").localPosition = FBXDuplicatorOld.transform.GetChild(0).transform.GetChild(4).localPosition;
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("DropPivot").localPosition = FBXDuplicatorOld.transform.GetChild(0).transform.GetChild(0).localPosition;
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localScale = new Vector3(1.15f, 1.15f, 1.15f);
                Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localPosition = new Vector3(0f, 0.55f, 1.64f);
                //Duplicator.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.FindChild("Base").transform.FindChild("Lid").localPosition = new Vector3(1.32885f, 1.801457f, -1.350426f);
                Duplicator.transform.FindChild("mdlDuplicator").GetComponent<Animator>().runtimeAnimatorController = OldAnimControll;
            }*/

            if (WConfig.OldModelDuplcators.Value == true)
            {
                Vector3 pickupDisplay = new Vector3(-1.23f, 1.832f, -0.108f);
                Vector3 hologramPivot = new Vector3(0f, 0.55f, 1.64f);
                Vector3 hologramScale = new Vector3(1.15f, 1.15f, 1.15f);
                Vector3 DropPivot = new Vector3(1.35f, 1.0877f, 0.1f);

                Mesh oldDuplicatorMesh = Assets.Bundle.LoadAsset<Mesh>("Assets/WQoL/General/DuplicatorOldMesh.asset");
                RuntimeAnimatorController oldAnimationController = Assets.Bundle.LoadAsset<RuntimeAnimatorController>("Assets/WQoL/General/animDuplicatorOld.controller");

                Transform Large = DuplicatorLarge.transform.GetChild(0);
                Large.GetComponent<RandomizeSplatBias>().enabled = false;
                Large.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = oldDuplicatorMesh;
                Large.GetChild(3).localPosition = pickupDisplay;
                Large.GetChild(5).localPosition = DropPivot;
                Large.GetChild(4).localScale = hologramScale;
                Large.GetChild(4).localPosition = hologramPivot;
                //DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.FindChild("Base").transform.FindChild("Lid").localPosition = new Vector3(1.32885f, 1.801457f, -1.350426f);
                Large.GetComponent<Animator>().runtimeAnimatorController = oldAnimationController;

                Transform Mili = DuplicatorMili.transform.GetChild(0);
                Mili.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = oldDuplicatorMesh;
                Mili.GetChild(3).localPosition = pickupDisplay;
                Mili.GetChild(5).localPosition = DropPivot;
                Mili.GetChild(4).localScale = hologramScale;
                Mili.GetChild(4).localPosition = hologramPivot;
                //Mili.transform.FindChild("DuplicatorArmature").transform.GetChild(0).transform.FindChild("Lid").localPosition = new Vector3(1.328f, 1.801f, -1.350f);
                Mili.GetComponent<Animator>().runtimeAnimatorController = oldAnimationController;
            }




            /*if (WConfig.OldModelDuplcators.Value == true)
             {
                 Transform Large = DuplicatorLarge.transform.GetChild(1);
                 DuplicatorLarge.transform.FindChild("mdlDuplicator").GetComponent<RandomizeSplatBias>().enabled = false;
                 DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh = FBXDuplicatorOld2.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                 DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("PickupDisplay").localPosition = FBXDuplicatorOld2.transform.GetChild(0).transform.GetChild(4).localPosition;
                 DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("DropPivot").localPosition = FBXDuplicatorOld2.transform.GetChild(0).transform.GetChild(0).localPosition;
                 DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localScale = new Vector3(1.15f, 1.15f, 1.15f);
                 DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localPosition = new Vector3(0f, 0.55f, 1.64f);
                 //DuplicatorLarge.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.FindChild("Base").transform.FindChild("Lid").localPosition = new Vector3(1.32885f, 1.801457f, -1.350426f);
                 DuplicatorLarge.transform.FindChild("mdlDuplicator").GetComponent<Animator>().runtimeAnimatorController = OldAnimControll;
             }

             if (WConfig.OldModelDuplcators.Value == true)
             {
                 DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh = FBXDuplicatorOldMili.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                 DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("PickupDisplay").localPosition = FBXDuplicatorOldMili.transform.GetChild(0).transform.GetChild(4).localPosition;
                 DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("DropPivot").localPosition = FBXDuplicatorOldMili.transform.GetChild(0).transform.GetChild(0).localPosition;
                 DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localScale = new Vector3(1.15f, 1.15f, 1.15f);
                 DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localPosition = new Vector3(0f, 0.55f, 1.64f);
                 //DuplicatorMili.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.GetChild(0).transform.FindChild("Lid").localPosition = new Vector3(1.328f, 1.801f, -1.350f);
                 DuplicatorMili.transform.FindChild("mdlDuplicator").GetComponent<Animator>().runtimeAnimatorController = OldAnimControll;
             }*/

            /*if (WConfig.OldModelDupliWild.Value == true)
            {
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh = FBXDuplicatorOldWild.transform.GetChild(0).transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().sharedMesh;
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("PickupDisplay").localPosition = FBXDuplicatorOldWild.transform.GetChild(0).transform.GetChild(7).localPosition;
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("DropPivot").localPosition = FBXDuplicatorOldWild.transform.GetChild(0).transform.GetChild(0).localPosition;
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localScale = new Vector3(1.15f, 1.15f, 1.15f);
                DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("HologramPivot").localPosition = new Vector3(0f, 0.55f, 1.64f);
                //DuplicatorWild.transform.FindChild("mdlDuplicator").transform.FindChild("DuplicatorArmature").transform.GetChild(0).transform.FindChild("Lid").localPosition = new Vector3(1.328f, 1.801f, -1.350f);
                DuplicatorWild.transform.FindChild("mdlDuplicator").GetComponent<Animator>().runtimeAnimatorController = OldAnimControll;
            }*/
        }


    }

}
