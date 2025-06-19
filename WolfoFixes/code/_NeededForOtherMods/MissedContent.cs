using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoFixes
{
    public static class MissedContent
    {
        public static class Items
        {
            public static ItemDef ScrapWhiteSuppressed = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC1/ScrapVoid/ScrapWhiteSuppressed.asset").WaitForCompletion();
            public static ItemDef ScrapGreenSuppressed = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC1/ScrapVoid/ScrapGreenSuppressed.asset").WaitForCompletion();
            public static ItemDef ScrapRedSuppressed = Addressables.LoadAssetAsync<ItemDef>(key: "RoR2/DLC1/ScrapVoid/ScrapRedSuppressed.asset").WaitForCompletion();
        }
        public static class Equipment
        {
            public static EquipmentDef EliteEarthEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "463f8f6917dd8af40860b023a520f051").WaitForCompletion();
            public static EquipmentDef EliteSecretSpeedEquipment = Addressables.LoadAssetAsync<EquipmentDef>(key: "RoR2/DLC1/EliteSecretSpeedEquipment.asset").WaitForCompletion();
        }

        public static class GameEndings
        {
            public static GameEndingDef EscapeSequenceFailed = Addressables.LoadAssetAsync<GameEndingDef>(key: "RoR2/Base/ClassicRun/EscapeSequenceFailed.asset").WaitForCompletion();
        }
        public static class Survivors
        {
            public static SurvivorDef VoidSurvivor = Addressables.LoadAssetAsync<SurvivorDef>(key: "dc32bd426643dce478dd1b04fd07cdf6").WaitForCompletion();
        }
        public static class Elites
        {
            public static EliteDef edSecretSpeed = Addressables.LoadAssetAsync<EliteDef>(key: "9752d818bdea9b449845fc4df8aed07a").WaitForCompletion();
        }
    }

}
