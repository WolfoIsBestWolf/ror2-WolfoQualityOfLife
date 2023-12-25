using RoR2;
using System;
using UnityEngine;


namespace WolfoQualityOfLife
{
    public class DuplicatorModel
    {

        public static AssetBundle DuplicatorOldModelBundle = LoadAssetBundle(Properties.Resources.oldduplicatorbundle);

        static AssetBundle LoadAssetBundle(Byte[] resourceBytes)
        {
            //Check to make sure that the byte array supplied is not null, and throw an appropriate exception if they are.
            if (resourceBytes == null) throw new ArgumentNullException(nameof(resourceBytes));

            //Actually load the bundle with a Unity function.
            var bundle = AssetBundle.LoadFromMemory(resourceBytes);

            return bundle;
        }

        public static UnityEngine.GameObject FBXDuplicatorOld = DuplicatorOldModelBundle.LoadAsset<GameObject>("Assets/mdlDuplicatorOld/mdlDuplicatorOld.fbx");
        public static UnityEngine.GameObject FBXDuplicatorOld2 = DuplicatorOldModelBundle.LoadAsset<GameObject>("Assets/mdlDuplicatorOld2/mdlDuplicatorOld2.fbx");
        public static UnityEngine.GameObject FBXDuplicatorOldMili = DuplicatorOldModelBundle.LoadAsset<GameObject>("Assets/mdlDuplicatorMiliOld/mdlDuplicatorMiliOld.fbx");
        public static UnityEngine.GameObject FBXDuplicatorOldWild = DuplicatorOldModelBundle.LoadAsset<GameObject>("Assets/mdlDuplicatorWildOld/mdlDuplicatorWildOld.fbx");
        public static UnityEngine.RuntimeAnimatorController OldAnimControll = DuplicatorOldModelBundle.LoadAsset<RuntimeAnimatorController>("Assets/animationcontroller/animDuplicator.controller");

        public static GameObject Duplicator = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/Duplicator");
        public static GameObject DuplicatorLarge = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge");
        public static GameObject DuplicatorMili = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary");
        public static GameObject DuplicatorWild = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorWild");

        [Obsolete]
        public static void DuplicatorModelChanger()
        {
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
            }

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
