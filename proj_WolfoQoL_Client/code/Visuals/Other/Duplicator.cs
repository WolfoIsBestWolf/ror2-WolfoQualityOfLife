using RoR2;

using UnityEngine;


namespace WolfoQoL_Client
{
    public static class DuplicatorModel
    {
        public static RuntimeAnimatorController oldAnimationControllerNOT;
        public static void DuplicatorModelChanger()
        {
            GameObject DuplicatorLarge = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorLarge");
            GameObject DuplicatorMili = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/chest/DuplicatorMilitary");
        
            if (WConfig.OldModelDuplcators.Value == true)
            {
                MakeOld(DuplicatorLarge);
                MakeOld(DuplicatorMili);

            }
        }

        public static void MakeOld(GameObject printer)
        {
            if (WConfig.OldModelDuplcators.Value == false)
            {
                return;
            }
            Transform Large = printer.transform.GetChild(0);
            Large.GetComponent<RandomizeSplatBias>().enabled = false;
            Large.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = Assets.Bundle.LoadAsset<Mesh>("Assets/WQoL/General/DuplicatorOldMesh.asset");
            Large.GetChild(3).localPosition = new Vector3(-1.23f, 1.832f, -0.108f);
            Large.GetChild(5).localPosition = new Vector3(1.35f, 1.0877f, 0.1f);
            Large.GetChild(4).localScale = new Vector3(1.15f, 1.15f, 1.15f);
            Large.GetChild(4).localPosition = new Vector3(0f, 0.55f, 1.64f);
            Large.GetComponent<Animator>().runtimeAnimatorController = Assets.Bundle.LoadAsset<RuntimeAnimatorController>("Assets/WQoL/General/animDuplicatorOld.controller");

        }
    }

}
