using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace WolfoQoL_Client
{
    public class BodyIcons
    {
        public static void Start()
        {
            if (WConfig.cfgIconsBodyIcons.Value == true)
            {
                NewBodyIcons();
            }
        }
        public static void NewBodyIcons()
        {
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BanditBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/Bandit1.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyBrotherHurt.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHauntBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyBrotherHurt.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyEngiWalkerTurret.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallRedBuddyBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyRoboBallBuddyRed.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallGreenBuddyBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyRoboBallBuddyGreen.png");


            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyBrother.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SquidTurretBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodySquidTurret.png");

            LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/WispSoulBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyWispSoul.png");



            GameObject DevotedLemurian = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBody.prefab").WaitForCompletion();
            GameObject DevotedLemurianElder = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBruiserBody.prefab").WaitForCompletion();
            DevotedLemurian.GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyDevotedLemurian.png");
            DevotedLemurianElder.GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyDevotedElder.png");



            Texture GenericPlanetDeath = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").GetComponent<CharacterBody>().portraitIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ExplosivePotDestructibleBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/ClayPot.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FusionCellDestructibleBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/FusionCell.png");
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TimeCrystalBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AltarSkeletonBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SulfurPodBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/SulfurPod.png");

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/FalseSonBoss/LunarRain.prefab").WaitForCompletion().GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/CorruptionSpike/CorruptionSpike.prefab").WaitForCompletion().GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;


            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TitanGoldBody").AddComponent<AllyBodyIconOverwrite>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texBodyTitanGoldAlly.png");

            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ParentPodBody").GetComponent<CharacterBody>().portraitIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Body/texAncestralIncubatorIcon.png");

        }


        public class AllyBodyIconOverwrite : MonoBehaviour
        {
            public Texture2D portraitIcon;
            public void Start()
            {
                CharacterBody body = GetComponent<CharacterBody>();
                if (body && body.teamComponent)
                {
                    if (body.teamComponent.teamIndex == TeamIndex.Player)
                    {
                        body.portraitIcon = portraitIcon;
                    }
                }
            }
        }

    }

}
