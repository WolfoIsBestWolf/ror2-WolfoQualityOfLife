using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

namespace WolfoFixes.Testing
{
    internal class Test
    {
        public static void Start()
        {

            if (WConfig.cfgTestMultiplayer.Value)
            {
                IL.RoR2.Networking.ServerAuthManager.HandleSetClientAuth += ServerAuthManager_HandleSetClientAuth;
                On.RoR2.Run.GetUserMaster += Run_GetUserMaster;
                On.RoR2.SteamworksLobbyManager.OnLobbyJoined_bool_string += SteamworksLobbyManager_OnLobbyJoined_bool_string;
            }
            if (WConfig.cfgTestLogbook.Value)
            {
                Test_Logbook.CheatLogbook();
            }
            if (WConfig.cfgLoadOrder.Value)
            {
                TestLoadOrder.Start();
            }
            if (!WConfig.cfgDisable.Value)
            {
                On.RoR2.PickupPickerController.GetOptionsFromPickupIndex += CommandWithEveryItem;
            }
        }

        private static void SteamworksLobbyManager_OnLobbyJoined_bool_string(On.RoR2.SteamworksLobbyManager.orig_OnLobbyJoined_bool_string orig, SteamworksLobbyManager self, bool success, string tokenReasonFailed)
        {
            //Random error loop where it'd open 500 server full windows.
            if (string.IsNullOrEmpty(tokenReasonFailed))
            {
                success = true;
            }
            orig(self, success, tokenReasonFailed);
        }

        public static CharacterMaster Run_GetUserMaster(On.RoR2.Run.orig_GetUserMaster orig, Run self, NetworkUserId networkUserId)
        {
            if (WConfig.cfgTestMultiplayer.Value)
            {
                return null;
            }
            return orig(self, networkUserId);
        }

        public static void ServerAuthManager_HandleSetClientAuth(MonoMod.Cil.ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.After,
                x => x.MatchLdloc(3)
                ))
            {
                c.EmitDelegate<System.Func<NetworkConnection, NetworkConnection>>((self) =>
                {
                    if (WConfig.cfgTestMultiplayer.Value)
                    {
                        return null;
                    }
                    return self;
                });
            }
            else
            {
                Debug.LogWarning("IL Failed: ServerAuthManager_HandleSetClientAuth");
            }
        }

        public static void UnusedStages()
        {
            SceneDef newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("31b8f728a914c9a4faa3df76d1bc0c0e");
            newScenedDef.cachedName = "golemplains_trailer";
            newScenedDef.nameToken = "Trailer Plains";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("b30e537b28c514b4a99227ab56a3e1d3");
            newScenedDef.cachedName = "ItemLogBookPositionalOffsets";
            newScenedDef.nameToken = "ItemLogBookPositionalOffsets";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("722873b571c73734c8572658dbb8f0db");
            newScenedDef.cachedName = "renderitem";
            newScenedDef.nameToken = "renderitem";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("835344f0a7461cc4b8909469b31a3ccc");
            newScenedDef.cachedName = "slice2";
            newScenedDef.nameToken = "slice2";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("5d8ea33392b43b94daac86dcf06740ab");
            newScenedDef.cachedName = "space";
            newScenedDef.nameToken = "space";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);

            newScenedDef = ScriptableObject.CreateInstance<SceneDef>();
            newScenedDef.sceneAddress = new AssetReferenceScene("db3348f5ee64faa48b2c14c3c52d5186");
            newScenedDef.cachedName = "stage1";
            newScenedDef.nameToken = "stage1";
            newScenedDef.shouldIncludeInLogbook = false;
            R2API.ContentAddition.AddSceneDef(newScenedDef);


            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneBlackbeach_Eclipse.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneBlackbeach.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneDampcave.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneEclipseClose.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneEclipseStandard.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneGoldshores.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneGolemplains.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneMoonFoggy.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneWispGraveyardSoot.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneRootJungleClear.asset").WaitForCompletion();
            Addressables.LoadAssetAsync<PostProcessProfile>(key: "RoR2/Base/title/PostProcessing/ppSceneRootJungleRain.asset").WaitForCompletion();


        }

        private static PickupPickerController.Option[] CommandWithEveryItem(On.RoR2.PickupPickerController.orig_GetOptionsFromPickupIndex orig, PickupIndex pickupIndex)
        {
            if (pickupIndex == PickupCatalog.FindPickupIndex(JunkContent.Items.AACannon.itemIndex))
            {
                PickupPickerController.Option[] array = new PickupPickerController.Option[ItemCatalog.itemCount];
                for (int i = 0; i < ItemCatalog.itemCount; i++)
                {
                    //Debug.LogWarning(pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i));
                    array[i] = new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i)
                    };
                }
                return array;
            }
            else if (pickupIndex == PickupCatalog.FindPickupIndex(JunkContent.Equipment.Enigma.equipmentIndex))
            {
                PickupPickerController.Option[] array = new PickupPickerController.Option[EquipmentCatalog.equipmentCount];
                for (int i = 0; i < EquipmentCatalog.equipmentCount; i++)
                {
                    //Debug.LogWarning(pickupIndex = PickupCatalog.FindPickupIndex((ItemIndex)i));
                    array[i] = new PickupPickerController.Option
                    {
                        available = true,
                        pickupIndex = PickupCatalog.FindPickupIndex((EquipmentIndex)i)
                    };
                }
                return array;
            }
            return orig(pickupIndex);
        }

    }

}