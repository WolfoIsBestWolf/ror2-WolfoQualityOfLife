using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace WolfoQoL_Client
{
    public static class AudioStuff
    {

        public static void Start()
        {

            SceneDef rootjungle = Addressables.LoadAssetAsync<SceneDef>(key: "RoR2/Base/rootjungle/rootjungle.asset").WaitForCompletion();
            MusicTrackDef MusicSulfurPoolsBoss = Addressables.LoadAssetAsync<MusicTrackDef>(key: "RoR2/DLC1/Common/muBossfightDLC1_12.asset").WaitForCompletion();
            rootjungle.bossTrack = MusicSulfurPoolsBoss;

            //Parry Success play sound again(?)
            GameObject parryEffect = Addressables.LoadAssetAsync<GameObject>(key: "c6f87be1350f96b4c8545a4ae7f22207").WaitForCompletion();
            //SFX doesnt play again if effect is restarted, so we'll just not pool it ig.
            parryEffect.GetComponent<VFXAttributes>().DoNotPool = true;
            PlaySoundOnEvent parry = parryEffect.AddComponent<PlaySoundOnEvent>();
            parry.soundEvent = "Play_item_proc_extraShrineItem";
            parry.triggeringEvent = PlaySoundOnEvent.PlaySoundEvent.Start;
            parry = parryEffect.AddComponent<PlaySoundOnEvent>();
            parry.soundEvent = "Play_item_proc_extraShrineItem";
            parry.triggeringEvent = PlaySoundOnEvent.PlaySoundEvent.Start;

        }


    }

}
