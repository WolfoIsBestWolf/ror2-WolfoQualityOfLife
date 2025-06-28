using BepInEx.Logging;
using RoR2;

namespace WolfoFixes.Testing
{
    internal class TestLoadOrder
    {
        public static ManualLogSource Logger;

        internal static void Start()
        {
            if (!WConfig.cfgLoadOrder.Value)
            {
                return;
            }


            RoR2Application.onStart += onStart;
            RoR2Application.onLoad += onLoad;
            RoR2Application.onLoadFinished += onLoadFinished;
            RoR2.UI.MainMenu.MainMenuController.OnMainMenuInitialised += OnMainMenuInitialised;

            Run.onRunStartGlobal += Run_onRunStartGlobal;
            On.RoR2.Run.PreStartClient += Run_PreStartClient;
            Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;

            On.RoR2.SceneInfo.Awake += SceneInfo_Awake;
            On.RoR2.SceneInfo.Start += SceneInfo_Start;
            On.RoR2.SceneInfo.OnDisable += SceneInfo_OnDisable;
            On.RoR2.SceneInfo.OnDestroy += SceneInfo_OnDestroy;

            On.RoR2.Stage.PreStartClient += Stage_PreStartClient;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
            On.RoR2.Stage.OnDisable += Stage_OnDisable;

            On.RoR2.SceneDirector.Start += SceneDirector_Start;


            RoR2.SceneCatalog.availability.CallWhenAvailable(SceneCatalog);
            RoR2.GameModeCatalog.availability.CallWhenAvailable(GameModeCatalog);

            RoR2.ArtifactCatalog.availability.CallWhenAvailable(ArtifactCatalog);
            RoR2.ItemCatalog.availability.CallWhenAvailable(ItemCatalog);
            RoR2.EquipmentCatalog.availability.CallWhenAvailable(EquipmentCatalog);
            On.RoR2.PickupCatalog.Init += PickupCatalog_Init;

            RoR2.BodyCatalog.availability.CallWhenAvailable(BodyCatalog);
            On.RoR2.MasterCatalog.Init += MasterCatalog_Init;
            On.RoR2.SkinCatalog.Init += SkinCatalog_Init;

            RoR2.UnlockableCatalog.availability.CallWhenAvailable(UnlockableCatalog);
            RoR2.AchievementManager.availability.CallWhenAvailable(AchievementManager);

            RoR2.RuleCatalog.availability.CallWhenAvailable(RuleCatalog);

        }

        private static void SceneInfo_OnDestroy(On.RoR2.SceneInfo.orig_OnDestroy orig, SceneInfo self)
        {
            orig(self);
            Logger.LogWarning("SceneInfo_OnDestroy");
        }

        private static void SceneInfo_OnDisable(On.RoR2.SceneInfo.orig_OnDisable orig, SceneInfo self)
        {
            orig(self);
            Logger.LogWarning("SceneInfo_OnDisable");
        }

        private static void SceneInfo_Start(On.RoR2.SceneInfo.orig_Start orig, SceneInfo self)
        {
            orig(self);
            Logger.LogWarning("SceneInfo_Start");
        }

        private static void SceneInfo_Awake(On.RoR2.SceneInfo.orig_Awake orig, SceneInfo self)
        {
            orig(self);
            Logger.LogWarning("SceneInfo_Awake");
        }

        private static void MasterCatalog_Init(On.RoR2.MasterCatalog.orig_Init orig)
        {
            Logger.LogWarning("MasterCatalog");
            orig();
        }

        private static System.Collections.IEnumerator PickupCatalog_Init(On.RoR2.PickupCatalog.orig_Init orig)
        {
            Logger.LogWarning("PickupCatalog");
            return orig();
        }

        private static System.Collections.IEnumerator SkinCatalog_Init(On.RoR2.SkinCatalog.orig_Init orig)
        {
            Logger.LogWarning("SkinCatalog");
            return orig();
        }

        private static void ItemCatalog()
        {
            Logger.LogWarning("ItemCatalog");
        }
        private static void BodyCatalog()
        {
            Logger.LogWarning("BodyCatalog");
        }
        private static void SceneCatalog()
        {
            Logger.LogWarning("SceneCatalog");
        }
        private static void ArtifactCatalog()
        {
            Logger.LogWarning("ArtifactCatalog");
        }
        private static void EquipmentCatalog()
        {
            Logger.LogWarning("EquipmentCatalog");
        }
        private static void GameModeCatalog()
        {
            Logger.LogWarning("GameModeCatalog");
        }
        private static void AchievementManager()
        {
            Logger.LogWarning("AchievementManager");
        }
        private static void RuleCatalog()
        {
            Logger.LogWarning("RuleCatalog");
        }
        private static void UnlockableCatalog()
        {
            Logger.LogWarning("UnlockableCatalog");
        }

        private static void Run_onRunDestroyGlobal(Run obj)
        {
            Logger.LogWarning("Run_onRunDestroyGlobal");
        }

        private static void Stage_OnDisable(On.RoR2.Stage.orig_OnDisable orig, Stage self)
        {
            orig(self);
            Logger.LogWarning("Stage_OnDisable");
        }

        private static void SceneDirector_Start(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            orig(self);
            Logger.LogWarning("SceneDirector_Start");
        }

        private static void Stage_PreStartClient(On.RoR2.Stage.orig_PreStartClient orig, Stage self)
        {
            orig(self);
            Logger.LogWarning("Stage_PreStartClient");
        }

        private static void Stage_onStageStartGlobal(Stage obj)
        {
            Logger.LogWarning("Stage_onStageStartGlobal " + obj.sceneDef);
        }

        private static void Run_PreStartClient(On.RoR2.Run.orig_PreStartClient orig, Run self)
        {
            orig(self);
            Logger.LogWarning("Run_PreStartClient");
        }

        private static void Run_onRunStartGlobal(Run obj)
        {
            Logger.LogWarning("Run_onRunStartGlobal");
        }

        public static void onStart()
        {
            Logger.LogWarning("RoR2Application.onStart");
        }
        public static void onLoadFinished()
        {
            Logger.LogWarning("RoR2Application.onLoadFinished");
        }
        public static void onLoad()
        {
            Logger.LogWarning("RoR2Application.onLoad");
        }
        public static void OnMainMenuInitialised()
        {
            Logger.LogWarning("RoR2.UI.MainMenu.MainMenuController.OnMainMenuInitialised");
        }



    }

}