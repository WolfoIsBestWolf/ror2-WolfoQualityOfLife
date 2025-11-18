using R2API;
using RoR2;
using UnityEngine;

namespace WolfoQoL_Client
{
    public class HoldoutZoneFlowers
    {
        public static GameObject LeptonDaisyTeleporterDecoration = null;
        public static GameObject GlowFlowerForPillar = null;

        public static void FlowersForOtherHoldoutZones()
        {

            RoR2.HoldoutZoneController TempLeptonDasiy1 = LegacyResourcesAPI.Load<GameObject>("Prefabs/networkedobjects/teleporters/Teleporter1").GetComponent<RoR2.HoldoutZoneController>();
            LeptonDaisyTeleporterDecoration = TempLeptonDasiy1.healingNovaItemEffect;

            GlowFlowerForPillar = PrefabAPI.InstantiateClone(LeptonDaisyTeleporterDecoration, "GlowFlowerForPillar", false); //Special1 (Enter)


            GlowFlowerForPillar.transform.localScale = new Vector3(0.6f, 0.6f, 0.55f);
            GlowFlowerForPillar.transform.localPosition = new Vector3(0f, 0f, 1f);
            GlowFlowerForPillar.transform.localRotation = new Quaternion(0.5373f, 0.8434f, 0, 0);

            GlowFlowerForPillar.transform.GetChild(0).localPosition = new Vector3(-0.9f, -2.9f, 4.35f);
            GlowFlowerForPillar.transform.GetChild(0).localRotation = new Quaternion(-0.4991f, 0.0297f, 0.2375f, -0.8328f);
            GlowFlowerForPillar.transform.GetChild(0).localScale = new Vector3(0.4f, 0.4f, 0.4f);

            GlowFlowerForPillar.transform.GetChild(1).localPosition = new Vector3(0.9f, 2.9f, 4.3f);
            GlowFlowerForPillar.transform.GetChild(1).localRotation = new Quaternion(0.5f, 0f, 0, -0.866f);
            GlowFlowerForPillar.transform.GetChild(1).localScale = new Vector3(0.55f, 0.55f, 0.55f);

            GlowFlowerForPillar.transform.GetChild(2).localPosition = new Vector3(-2.7f, 1.3f, 4.4f);
            GlowFlowerForPillar.transform.GetChild(2).localRotation = new Quaternion(-0.1504f, -0.4924f, -0.0868f, 0.8529f);
            GlowFlowerForPillar.transform.GetChild(2).localScale = new Vector3(0.45f, 0.45f, 0.45f);

            GlowFlowerForPillar.transform.GetChild(3).localPosition = new Vector3(2.8f, -1f, 4.3f);
            GlowFlowerForPillar.transform.GetChild(3).localRotation = new Quaternion(0.1504f, 0.4924f, -0.0868f, 0.8529f);
            GlowFlowerForPillar.transform.GetChild(3).localScale = new Vector3(0.5f, 0.5f, 0.5f);


            //WolfoMain.log.LogWarning(LeptonDaisyTeleporterDecoration);

            On.RoR2.HoldoutZoneController.Awake += AddFlowersToMiscHoldoutZone;

            On.RoR2.HoldoutZoneController.Start += (orig, self) =>
            {
                orig(self);
                if (self.applyHealingNova)
                {
                    if (Util.GetItemCountForTeam(TeamIndex.Player, RoR2Content.Items.TPHealingNova.itemIndex, false, true) > 0)
                    {
                        if (self.healingNovaItemEffect)
                        {
                            self.healingNovaItemEffect.SetActive(true);
                            self.healingNovaItemEffect = null;
                        }
                    }
                    else
                    {
                        if (self.healingNovaItemEffect)
                        {
                            self.healingNovaItemEffect.SetActive(false);
                        }
                    }
                }
                //WolfoMain.log.LogWarning("HoldoutZoneController.Start");
            };

        }

        private static void AddFlowersToMiscHoldoutZone(On.RoR2.HoldoutZoneController.orig_Awake orig, HoldoutZoneController self)
        {
            orig(self);
            //WolfoMain.log.LogWarning(self);
            if (self.applyHealingNova)
            {
                if (!self.healingNovaItemEffect)
                {
                    if (self.name.StartsWith("InfiniteTowerSafeWard"))
                    {
                        self.healingNovaItemEffect = Object.Instantiate(LeptonDaisyTeleporterDecoration, self.transform.GetChild(0).GetChild(0).GetChild(8).GetChild(0).GetChild(1).GetChild(0));
                        self.healingNovaItemEffect.transform.rotation = new Quaternion(0, -0.7071f, -0.7071f, 0);
                        self.healingNovaItemEffect.transform.localScale = new Vector3(0.1928f, 0.1928f, 0.1928f);
                        self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.4193f, 0);
                    }
                    else if (self.name.StartsWith("MoonBattery"))
                    {
                        self.healingNovaItemEffect = Object.Instantiate(GlowFlowerForPillar, self.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(1));
                        //self.healingNovaItemEffect.transform.rotation = new Quaternion(-0.683f, -0.183f, -0.183f, 0.683f);
                        //self.healingNovaItemEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.55f);
                        //self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -2.4f, 0f);
                    }
                    else if (self.name.StartsWith("NullSafeZone"))
                    {
                        self.healingNovaItemEffect = Object.Instantiate(LeptonDaisyTeleporterDecoration, self.transform);
                        self.healingNovaItemEffect.transform.rotation = new Quaternion(-0.5649f, -0.4254f, -0.4254f, 0.5649f);
                        self.healingNovaItemEffect.transform.localScale = new Vector3(0.275f, 0.275f, 0.225f);
                        self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.3f, 0f);
                    }
                    else if (self.name.StartsWith("DeepVoidPortalBattery(Clone)"))
                    {
                        self.healingNovaItemEffect = self.healingNovaItemEffect = Object.Instantiate(GlowFlowerForPillar, self.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).GetChild(0));
                        self.healingNovaItemEffect.transform.localEulerAngles = new Vector3(270f, 0f, 0f);
                        self.healingNovaItemEffect.transform.localScale = new Vector3(0.375f, 0.375f, 0.375f);
                        self.healingNovaItemEffect.transform.localPosition = new Vector3(0f, -0.4f, 0f);
                    }

                }
                if (Util.GetItemCountForTeam(TeamIndex.Player, RoR2Content.Items.TPHealingNova.itemIndex, false, true) > 0)
                {
                    if (self.healingNovaItemEffect)
                    {
                        self.healingNovaItemEffect.SetActive(true);
                        self.healingNovaItemEffect = null;
                    }
                }
            }
        }


    }
}
