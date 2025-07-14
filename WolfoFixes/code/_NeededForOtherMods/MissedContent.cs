using RoR2;
using RoR2.EntitlementManagement;
using RoR2.ExpansionManagement;
using UnityEngine.AddressableAssets;

namespace WolfoFixes
{
    public static class DLCS
    {
        public static ExpansionDef DLC1 = Addressables.LoadAssetAsync<ExpansionDef>(key: "d4f30c23b971a9b428e2796dc04ae099").WaitForCompletion();
        public static ExpansionDef DLC2 = Addressables.LoadAssetAsync<ExpansionDef>(key: "851f234056d389b42822523d1be6a167").WaitForCompletion();
        //public static ExpansionDef DLC3 = Addressables.LoadAssetAsync<ExpansionDef>(key: "RoR2/DLC3/Common/DLC3.asset").WaitForCompletion();
        public static ExpansionDef DLC3 = null;

        public static EntitlementDef entitlementDLC1 = Addressables.LoadAssetAsync<EntitlementDef>(key: "0166774839e0bd345bb11554aecbfd32").WaitForCompletion();
        public static EntitlementDef entitlementDLC2 = Addressables.LoadAssetAsync<EntitlementDef>(key: "68fcfaddae5157f4581a1fc209e4eac6").WaitForCompletion();
        //public static EntitlementDef entitlementDLC3 = Addressables.LoadAssetAsync<EntitlementDef>(key: "RoR2/DLC3/Common/DLC3.asset").WaitForCompletion();

    }
    public static class SceneList
    {
        public static SceneDef SulfurPools = Addressables.LoadAssetAsync<SceneDef>(key: "796f9b67682b3db4c8c5af7294d0490c").WaitForCompletion();
        public static SceneDef RootJungle = Addressables.LoadAssetAsync<SceneDef>(key: "ced489f798226594db0d115af2101a9b").WaitForCompletion(); 
    }


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
