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
            Texture2D TexBodyBrotherHurt = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyBrotherHurt.png");
            TexBodyBrotherHurt.wrapMode = TextureWrapMode.Clamp;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHurtBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrotherHurt;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherHauntBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrotherHurt;


            Texture2D TexWalkerTurretIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyEngiWalkerTurret.png");
            TexWalkerTurretIcon.wrapMode = TextureWrapMode.Clamp;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/EngiWalkerTurretBody").GetComponent<CharacterBody>().portraitIcon = TexWalkerTurretIcon;

            Texture2D TexProbeGreen = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyRoboBallBuddyGreen.png");
            TexProbeGreen.wrapMode = TextureWrapMode.Clamp;
            Texture2D TexProbeRed = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyRoboBallBuddyRed.png");
            TexProbeRed.wrapMode = TextureWrapMode.Clamp;

            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallRedBuddyBody").GetComponent<CharacterBody>().portraitIcon = TexProbeRed;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/RoboBallGreenBuddyBody").GetComponent<CharacterBody>().portraitIcon = TexProbeGreen;


            //Why needs to be run late? Also move to some body icon thing or whatever
            Texture2D TexBodyBrother = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyBrother.png");
            TexBodyBrother.wrapMode = TextureWrapMode.Clamp;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/BrotherBody").GetComponent<CharacterBody>().portraitIcon = TexBodyBrother;


            Texture2D TexBlueSquidTurret = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodySquidTurret.png");
            TexBlueSquidTurret.wrapMode = TextureWrapMode.Clamp;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SquidTurretBody").GetComponent<CharacterBody>().portraitIcon = TexBlueSquidTurret;


            Texture2D TexSoulWisp = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyWispSoul.png");
            TexSoulWisp.wrapMode = TextureWrapMode.Clamp;
            LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/WispSoulBody").GetComponent<CharacterBody>().portraitIcon = TexSoulWisp;


            //Devoted Lemurians
            Texture2D texBodyDevotedLemurian = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyDevotedLemurian.png");
            texBodyDevotedLemurian.wrapMode = TextureWrapMode.Clamp;

            Texture2D texBodyDevotedElder = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyDevotedElder.png");
            texBodyDevotedElder.wrapMode = TextureWrapMode.Clamp;

            GameObject DevotedLemurian = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBody.prefab").WaitForCompletion();
            GameObject DevotedLemurianElder = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/CU8/DevotedLemurianBruiserBody.prefab").WaitForCompletion();

            DevotedLemurian.GetComponent<CharacterBody>().portraitIcon = texBodyDevotedLemurian;
            DevotedLemurianElder.GetComponent<CharacterBody>().portraitIcon = texBodyDevotedElder;
            DevotedLemurian.GetComponent<DeathRewards>().logUnlockableDef = null;
            DevotedLemurianElder.GetComponent<DeathRewards>().logUnlockableDef = null;



            Texture GenericPlanetDeath = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ArtifactShellBody").GetComponent<CharacterBody>().portraitIcon;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ExplosivePotDestructibleBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/FusionCellDestructibleBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TimeCrystalBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/AltarSkeletonBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/SulfurPodBody").GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;

            Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC2/FalseSonBoss/LunarRain.prefab").WaitForCompletion().GetComponent<CharacterBody>().portraitIcon = GenericPlanetDeath;


            Texture2D texBodyTitanGoldAlly = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texBodyTitanGoldAlly.png");
            texBodyTitanGoldAlly.wrapMode = TextureWrapMode.Clamp;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/TitanGoldBody").AddComponent<AllyBodyIconOverwrite>().portraitIcon = texBodyTitanGoldAlly;

            Texture2D texAncestralIncubatorIcon = Assets.Bundle.LoadAsset<Texture2D>("Assets/WQoL/Icons/texAncestralIncubatorIcon.png");
            texAncestralIncubatorIcon.wrapMode = TextureWrapMode.Clamp;
            LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/ParentPodBody").GetComponent<CharacterBody>().portraitIcon = texAncestralIncubatorIcon;

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
