using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using WolfoLibrary;
using WolfoQoL_Client.ModSupport;

namespace WolfoQoL_Client.Reminders
{
    public static class Reminders_Main
    {
        public static void Start()
        {
            //UIBorders.UpdateHuds();

            Reminders_Treasure.Start();
            Reminders_Portal.Start();
            Reminders_SecretGeode.Start();
            Objectives_General.Start();
            Objective_Halcyonite.Start();
            InteractableVisuals.Start();
            Reminders_Newt.Start();

            //If you scrap it the reminder should vanish
            //But what if you see the reminder and get another key to unlock the box?

            //If you void it it's just gone so that's fine
            //How would I check this in multiplayer I guess?


            //On.RoR2.SceneDirector.Start += AddReminders;
            Stage.onStageStartGlobal += Stage_onStageStartGlobal;
            On.RoR2.Stage.OnDisable += Stage_OnDisable;
            if (WConfig.module_text_reminders.Value)
            {
                On.RoR2.ScrapperController.BeginScrapping_UniquePickup += RemoveReminders_Scrapper;

                On.RoR2.HoldoutZoneController.OnEnable += IsFreeVoidChest_ClearNewt;

                ExtraActions.onMithrixPhase1 += FailAllReminders;
                ExtraActions.onFalseSonPhase1 += FailAllReminders;

                On.EntityStates.VoidRaidCrab.EscapeDeath.OnExit += ClearTreasure_OnVoidlingDeath;
                EntityStates.MeridianEvent.MeridianEventStart.OnMeridianEventStart += ClearReminders_OnFalseSon;
            }
            Run.onRunStartGlobal += Run_onRunStartGlobal;


            Addressables.LoadAssetAsync<GameObject>(key: "e97c0cb6c5d2e3f4fb7a64cb62fe971f").WaitForCompletion().AddComponent<SpawnListener>().interactable = SpawnListener.Interactable.lockbox;
            Addressables.LoadAssetAsync<GameObject>(key: "7cf417baadd2e8948a9645da22a57b3c").WaitForCompletion().AddComponent<SpawnListener>().interactable = SpawnListener.Interactable.lockboxVoid;
            Addressables.LoadAssetAsync<GameObject>(key: "357435043113a944c9a477d63dc9a893").WaitForCompletion().AddComponent<SpawnListener>().interactable = SpawnListener.Interactable.freeChest;

            Addressables.LoadAssetAsync<GameObject>(key: "f21b2c8a9cc028046935ea871dc4af54").WaitForCompletion().AddComponent<SpawnListener>().interactable = SpawnListener.Interactable.greenPrinter;
            Addressables.LoadAssetAsync<GameObject>(key: "c10fd181efcffc24f8fed4c2f246fac8").WaitForCompletion().AddComponent<SpawnListener>().interactable = SpawnListener.Interactable.halcyonShrine;

        }

        private static void Stage_OnDisable(On.RoR2.Stage.orig_OnDisable orig, Stage self)
        {
            orig(self);
            if (TreasureReminder.instance)
            {
                TreasureReminder.instance.Reset();
            }
        }

        private static void Stage_onStageStartGlobal(Stage obj)
        {
            if (WConfig.module_text_reminders.Value == true)
            {
                if (SceneInfo.instance.countsAsStage || SceneInfo.instance.sceneDef.allowItemsToSpawnObjects)
                {
                    TreasureReminder.SetupRemindersStatic();
                }
            }
        }


        public static void AddReminders(On.RoR2.SceneDirector.orig_Start orig, SceneDirector self)
        {
            orig(self);
            if (WConfig.module_text_reminders.Value == true)
            {
                if (SceneInfo.instance.countsAsStage || SceneInfo.instance.sceneDef.allowItemsToSpawnObjects)
                {
                    TreasureReminder.SetupRemindersStatic();
                }
            }
            Log.LogMessage("CurrentStage: " + SceneInfo.instance.sceneDef.baseSceneName);

        }

        private static void RemoveReminders_Scrapper(On.RoR2.ScrapperController.orig_BeginScrapping_UniquePickup orig, ScrapperController self, UniquePickup pickupToTake)
        {
            orig(self, pickupToTake);

            PickupDef pickupDef = PickupCatalog.GetPickupDef(pickupToTake.pickupIndex);
            if (pickupDef != null)
            {
                if (pickupDef.itemIndex != ItemIndex.None)
                {
                    if (pickupDef.itemIndex == RoR2Content.Items.TreasureCache.itemIndex)
                    {
                        TreasureReminder.CheckKeysVoided();
                    }
                    else if (pickupDef.itemIndex == DLC2Content.Items.LowerPricedChests.itemIndex)
                    {
                        CharacterBody component = self.interactor.GetComponent<CharacterBody>();
                        if (TreasureReminder.instance != null)
                        {
                            TreasureReminder.instance.ScrappedSaleStar(component);
                        }
                    }
                }
            }
        }

        private static void Run_onRunStartGlobal(Run obj)
        {
            obj.gameObject.AddComponent<TreasureReminder>();
        }

        private static void IsFreeVoidChest_ClearNewt(On.RoR2.HoldoutZoneController.orig_OnEnable orig, HoldoutZoneController self)
        {
            orig(self);
            if (TreasureReminder.instance)
            {
                if (self.GetComponent<TeleporterInteraction>())
                {
                    if (TreasureReminder.instance.Objective_NewtShrine)
                    {
                        FailObjective(TreasureReminder.instance.Objective_NewtShrine);
                    }
                }
                else if (self.name.StartsWith("VoidShellBattery"))
                {
                    if (TreasureReminder.instance.Objective_FreeChestVVVoid)
                    {
                        //Activate teleporter doesn't get striked through ig
                        Object.Destroy(TreasureReminder.instance.Objective_FreeChestVVVoid);
                    }
                }

            }

        }

        private static void CheckForFreeChestVoid(On.RoR2.HoldoutZoneController.orig_PreStartClient orig, HoldoutZoneController self)
        {
            orig(self);
            if (self.name.StartsWith("VoidShellBattery"))
            {
                TreasureReminder.instance.voidFreeChestSpawned = true;
            }
        }

        private static void ClearTreasure_OnVoidlingDeath(On.EntityStates.VoidRaidCrab.EscapeDeath.orig_OnExit orig, EntityStates.VoidRaidCrab.EscapeDeath self)
        {
            orig(self);
            FailAllReminders();
        }


        public static void FailAllReminders()
        {
            TreasureReminder reminder = TreasureReminder.instance;
            if (!reminder)
            {
                return;
            }
            if (reminder.Objective_Lockbox)
            {
                FailObjective(reminder.Objective_Lockbox);
            }
            if (reminder.Objective_LockboxVoid)
            {
                FailObjective(reminder.Objective_LockboxVoid);
            }
            if (reminder.Objective_FreeChest)
            {
                FailObjective(reminder.Objective_FreeChest);
            }
            if (reminder.Objective_FreeChestVVVoid)
            {
                FailObjective(reminder.Objective_FreeChestVVVoid);
            }
            if (reminder.Objective_SaleStar)
            {
                FailObjective(reminder.Objective_SaleStar);
            }
            if (reminder.Objective_RegenScrap)
            {
                FailObjective(reminder.Objective_RegenScrap);
            }
            if (reminder.Objective_NewtShrine)
            {
                FailObjective(reminder.Objective_NewtShrine);
            }
        }

        public static void FailObjective(GenericObjectiveProvider objective)
        {
            if (!objective)
            {
                Log.LogMessage("FailObjective Null Objective");
                return;
            }
            objective.markCompletedOnRetired = false;
            objective.objectiveToken = "<color=#808080><s>" + Language.GetString(objective.objectiveToken) + "</color></s>";
            objective.enabled = false;
        }
        public static void CompleteObjective(GenericObjectiveProvider objective)
        {
            if (!objective)
            {
                Log.LogMessage("CompleteObjective Null Objective");
                return;
            }
            objective.objectiveToken = "<color=#808080><s>" + objective.objectiveToken + "</color></s>";
            objective.enabled = false;
        }

        private static void ClearReminders_OnFalseSon()
        {
            //WolfoMain.log.LogWarning("Secret Geode : End Early Boss");
            GameObject SecretMission = GameObject.Find("/Meridian_Halcyonite_Encounter");
            if (SecretMission)
            {
                GenericObjectiveProvider objective = SecretMission.GetComponent<GenericObjectiveProvider>();
                if (objective)
                {
                    FailObjective(objective);
                }
            }
            GameObject Objective_ReachTemple = GameObject.Find("/HOLDER: Design/Objective_ReachTemple");
            GameObject Objective_ReachArena = GameObject.Find("/HOLDER: Design/Objective_ReachArena");
            if (Objective_ReachTemple)
            {
                Objective_ReachTemple.GetComponent<GenericObjectiveProvider>().enabled = false;
            }
            if (Objective_ReachArena)
            {
                Objective_ReachArena.GetComponent<GenericObjectiveProvider>().enabled = false;
            }

        }

    }

    public class SpawnListener : MonoBehaviour
    {
        public Interactable interactable;
        public enum Interactable
        {
            None,
            lockbox,
            lockboxVoid,
            freeChest,
            vvVoidFreeChest,
            greenPrinter,
            greenPrinterQuality,
            newtAltar,
            halcyonShrine
        }

        public void OnEnable()
        {
            if (!TreasureReminder.instance)
            {
                enabled = false;
                return;
            }
            switch (interactable)
            {
                case Interactable.greenPrinter:
                    TreasureReminder.instance.greenPrinterSpawned = true;
                    break;
                case Interactable.greenPrinterQuality:
                    TreasureReminder.instance.greenQualityPrinterSpawned = true;
                    break;
                case Interactable.vvVoidFreeChest:
                    TreasureReminder.instance.voidFreeChestSpawned = true;
                    break;
                case Interactable.newtAltar:
                    TreasureReminder.instance.newtShrineSpawned++;
                    break;
                case Interactable.halcyonShrine:
                    TreasureReminder.instance.halcyonSpawned = true;
                    break;
                case Interactable.lockbox:
                    TreasureReminder.instance.lockboxCount++;
                    break;
                case Interactable.lockboxVoid:
                    TreasureReminder.instance.lockboxVoidCount++;
                    break;
                case Interactable.freeChest:
                    TreasureReminder.instance.freeChestCount++;
                    break;
            }

        }
        public void OnDisable()
        {
            if (interactable == Interactable.newtAltar)
            {
                if (TreasureReminder.instance.newtShrineSpawned != 0)
                {
                    TreasureReminder.instance.newtShrineSpawned--;
                }

            }
        }
    }

    public class TreasureReminder : MonoBehaviour
    {
        public static TreasureReminder instance;
        public int lockboxCount = 0;
        public int lockboxVoidCount = 0;
        public int freeChestCount = 0;
        public bool freeChestVoidBool = false;
        public bool localHasSaleStar = false;
        public bool localHasRegenScrap = false;
        public bool localHasRegenScrapQuality = false;

        public bool halcyonSpawned = false;
        public bool greenPrinterSpawned = false;
        public bool greenQualityPrinterSpawned = false;
        public bool voidFreeChestSpawned = false;
        //public bool accessNodeSpawned = false;
        public int newtShrineSpawned = 0;

        public GenericObjectiveProvider Objective_Lockbox;
        public GenericObjectiveProvider Objective_LockboxVoid;
        public GenericObjectiveProvider Objective_FreeChest;
        public GenericObjectiveProvider Objective_FreeChestVVVoid;
        public GenericObjectiveProvider Objective_SaleStar;
        public GenericObjectiveProvider Objective_RegenScrap;

        public GenericObjectiveProvider Objective_NewtShrine;
        public GenericObjectiveProvider Objective_Halcyon;
        public GenericObjectiveProvider Objective_AccessNode;

        public GenericObjectiveProvider Objective_SecretGeode;

        public void Awake()
        {
            instance = this;
        }

        public static void SetupRemindersStatic()
        {
            if (instance == null)
            {
                TreasureReminder treasureReminder = Run.instance.gameObject.GetComponent<TreasureReminder>();
                if (treasureReminder == null)
                {
                    treasureReminder = Run.instance.gameObject.AddComponent<TreasureReminder>();
                    instance = treasureReminder;
                }
            }
            instance.SetupReminders();
        }


        public void SetupReminders()
        {
            bool isSimu = Run.instance is InfiniteTowerRun;
            if (isSimu && WConfig.cfgRemindersNOTINSIMU.Value)
            {
                return;
            }

            int maximumKeys = 0;
            int maximumKeysVoid = 0;

            if (QualitySupport.QualityModInstalled)
            {
                localHasSaleStar = QualitySupport.QualiyItemCountPermanent(DLC2Content.Items.LowerPricedChests, LocalUserManager.GetFirstLocalUser().cachedMaster.inventory) > 0;
                localHasRegenScrap = QualitySupport.QualiyItemCountPermanent(DLC1Content.Items.RegeneratingScrap, LocalUserManager.GetFirstLocalUser().cachedMaster.inventory) > 0;
                using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        maximumKeys += QualitySupport.QualiyItemCountPermanent(RoR2Content.Items.TreasureCache, LocalUserManager.GetFirstLocalUser().cachedMaster.inventory);
                        maximumKeysVoid += QualitySupport.QualiyItemCountPermanent(DLC1Content.Items.TreasureCacheVoid, LocalUserManager.GetFirstLocalUser().cachedMaster.inventory);
                    }
                }
            }
            else
            {
                localHasSaleStar = LocalUserManager.GetFirstLocalUser().cachedMaster.inventory.GetItemCountPermanent(DLC2Content.Items.LowerPricedChests) > 0;
                localHasRegenScrap = LocalUserManager.GetFirstLocalUser().cachedMaster.inventory.GetItemCountPermanent(DLC1Content.Items.RegeneratingScrap) > 0;
                using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        maximumKeys += enumerator.Current.master.inventory.GetItemCountPermanent(RoR2Content.Items.TreasureCache);
                        maximumKeysVoid += enumerator.Current.master.inventory.GetItemCountPermanent(DLC1Content.Items.TreasureCacheVoid);
                    }
                }

            }

            if (lockboxCount > maximumKeys)
            {
                lockboxCount = maximumKeys;
            }
            if (lockboxVoidCount > maximumKeysVoid)
            {
                lockboxVoidCount = maximumKeysVoid;
            }

            if (!isSimu && SceneCatalog.mostRecentSceneDef.stageOrder > 5)
            {
                localHasSaleStar = false;
                localHasRegenScrap = false;
            }

            if (!isSimu && WConfig.cfgReminder_AccessNode.Value && AccessCodesMissionController.instance)
            {
                bool flag = AccessCodesMissionController.instance.ignoreSolusWingDeath || !Run.instance.GetEventFlag("SolusWingBeaten");
                if (AccessCodesMissionController.instance.RunHasRequiredExpansion() && flag)
                {
                    Objective_AccessNode = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_AccessNode.objectiveToken = Language.GetString("REMINDER_ACCESSNODE");
                }
            }
            if (!isSimu && WConfig.cfgRemindersNewt.Value != WConfig.ReminderChoice.Off)
            {
                if (newtShrineSpawned > 0 || WConfig.cfgRemindersNewt.Value == WConfig.ReminderChoice.Always)
                {
                    if (TeleporterInteraction.instance && !TeleporterInteraction.instance.shouldAttemptToSpawnShopPortal)
                    {
                        //Log.LogMessage("Newt Reminder");
                        string token = Language.GetString("REMINDER_NEWT");
                        Objective_NewtShrine = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                        Objective_NewtShrine.objectiveToken = token;
                    }
                }
            }
            if (!isSimu && WConfig.cfgReminder_Halcyon.Value && halcyonSpawned)
            {
                string token = Language.GetString("REMINDER_HALCYON");
                Objective_Halcyon = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                Objective_Halcyon.objectiveToken = token;
            }
            if (WConfig.cfgRemindersKeys.Value)
            {
                if (lockboxVoidCount > 0)
                {
                    // Log.LogMessage("TreasureCacheVoidCount " + lockboxVoidCount);
                    string token = string.Empty;
                    if (lockboxVoidCount > 1)
                    {
                        token = string.Format(Language.GetString("REMINDER_KEYVOID_MANY"), lockboxVoidCount);
                    }
                    else
                    {
                        token = Language.GetString("REMINDER_KEYVOID");
                    }
                    Objective_LockboxVoid = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_LockboxVoid.objectiveToken = token;
                }
                if (lockboxCount > 0)
                {
                    //Log.LogMessage("TreasureCacheCount " + lockboxCount);
                    string token = string.Empty;
                    if (lockboxCount > 1)
                    {
                        token = string.Format(Language.GetString("REMINDER_KEY_MANY"), lockboxCount);
                    }
                    else
                    {
                        token = Language.GetString("REMINDER_KEY");
                    }
                    Objective_Lockbox = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_Lockbox.objectiveToken = token;
                }
            }
            if (WConfig.cfgRemindersFreechest.Value)
            {
                if (freeChestCount > 0)
                {
                    //Log.LogMessage("FreeChestCount " + freeChestCount);
                    string token = string.Empty;
                    if (freeChestCount > 1)
                    {
                        token = string.Format(Language.GetString("REMINDER_FREECHEST_MANY"), freeChestCount);
                    }
                    else
                    {
                        token = Language.GetString("REMINDER_FREECHEST");
                    }
                    Objective_FreeChest = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_FreeChest.objectiveToken = token;
                }
            }
            if (WConfig.cfgRemindersFreechestVV.Value)
            {
                if (voidFreeChestSpawned)
                {
                    //Log.LogMessage("FreeChestVoid Reminder");
                    string token = Language.GetString("REMINDER_FREECHESTVOID");
                    Objective_FreeChestVVVoid = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_FreeChestVVVoid.objectiveToken = token;
                }
            }
            if (!isSimu && WConfig.cfgRemindersRegenScrap.Value != WConfig.ReminderChoice.Off)
            {
                if (greenPrinterSpawned || WConfig.cfgRemindersRegenScrap.Value == WConfig.ReminderChoice.Always)
                {
                    if (localHasRegenScrap)
                    {
                        //Log.LogMessage("Local RegenScrap Reminder");
                        string token = Language.GetString("REMINDER_REGENSCRAP");
                        Objective_RegenScrap = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                        Objective_RegenScrap.objectiveToken = token;
                    }
                }
            }
            if (WConfig.cfgRemindersSaleStar.Value)
            {
                if (localHasSaleStar)
                {
                    //Log.LogMessage("Local SaleStar Reminder");
                    string token = Language.GetString("REMINDER_SALESTAR");
                    Objective_SaleStar = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_SaleStar.objectiveToken = token;
                }
            }



        }

        public void Reset()
        {

            lockboxCount = 0;
            lockboxVoidCount = 0;
            freeChestCount = 0;
            freeChestVoidBool = false;
            voidFreeChestSpawned = false;
            localHasSaleStar = false;
            localHasRegenScrap = false;

            greenPrinterSpawned = false;
            //accessNodeSpawned = false;
            halcyonSpawned = false;
            newtShrineSpawned = 0;
        }

        public static void CheckKeysVoided()
        {
            int keys = 0;
            int keysVoid = 0;

            using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    keys += enumerator.Current.master.inventory.GetItemCountPermanent(RoR2Content.Items.TreasureCache);
                    keysVoid += enumerator.Current.master.inventory.GetItemCountPermanent(DLC1Content.Items.TreasureCacheVoid);
                }
            }
            //Log.LogMessage("Check if voided Lockbox reminder : " + maximumKeys);
            ShouldRemoveKeyObjective(keys, keysVoid);
        }


        public static void CheckItemsDestroyed()
        {
            if (!instance)
            {
                return;
            }

            bool HasSaleStar = false;
            bool HasRegenScrap = false;

            if (QualitySupport.QualityModInstalled)
            {
                HasSaleStar = QualitySupport.QualiyItemCountPermanent(DLC2Content.Items.LowerPricedChests, LocalUserManager.GetFirstLocalUser().cachedMaster.inventory) > 0;
                HasRegenScrap = QualitySupport.QualiyItemCountPermanent(DLC1Content.Items.RegeneratingScrap, LocalUserManager.GetFirstLocalUser().cachedMaster.inventory) > 0;
            }
            else
            {
                HasSaleStar = LocalUserManager.GetFirstLocalUser().cachedMaster.inventory.GetItemCountPermanent(DLC2Content.Items.LowerPricedChests) > 0;
                HasRegenScrap = LocalUserManager.GetFirstLocalUser().cachedMaster.inventory.GetItemCountPermanent(DLC1Content.Items.RegeneratingScrap) > 0;
            }
            if (instance.localHasSaleStar && !HasSaleStar)
            {
                Reminders_Main.FailObjective(instance.Objective_SaleStar);
            }
            if (instance.localHasRegenScrap && !HasRegenScrap)
            {
                Reminders_Main.FailObjective(instance.Objective_RegenScrap);
            }
        }


        public static void ShouldRemoveKeyObjective(int keys, int voidKeys)
        {
            if (instance == null)
            {
                return;
            }
            /*if (!instance.Objective_Lockbox)
            {
                return;
            }*/
            if (keys == 0)
            {
                Reminders_Main.FailObjective(instance.Objective_Lockbox);
            }
            else if (instance.lockboxCount > keys)
            {
                for (int i = instance.lockboxCount; i > keys; i--)
                {
                    instance.DeductLockboxCount();
                }
            }
            if (voidKeys == 0)
            {
                Reminders_Main.FailObjective(instance.Objective_LockboxVoid);
            }
            else if (instance.lockboxVoidCount > voidKeys)
            {
                for (int i = instance.lockboxVoidCount; i > keys; i--)
                {
                    instance.DeductLockboxVoidCount();
                }
            }

        }

        public void DeductLockboxCount()
        {
            if (Objective_Lockbox == null)
            {
                return;
            }

            //Log.LogMessage("TreasureCacheCount " + lockboxCount);
            if (lockboxCount > 0)
            {
                string old = "(" + lockboxCount + "/";
                lockboxCount--;
                string newstring = "(" + lockboxCount + "/";
                Objective_Lockbox.objectiveToken = Objective_Lockbox.objectiveToken.Replace(old, newstring);
            }
            if (lockboxCount == 0)
            {
                Reminders_Main.CompleteObjective(Objective_Lockbox);
            }

        }

        public void DeductLockboxVoidCount()
        {
            if (Objective_LockboxVoid == null)
            {
                return;
            }
            //Log.LogMessage("TreasureCacheVoidCount " + lockboxVoidCount);
            if (lockboxVoidCount > 0)
            {
                string old = "(" + lockboxVoidCount + "/";
                lockboxVoidCount--;
                string newstring = "(" + lockboxVoidCount + "/";
                Objective_LockboxVoid.objectiveToken = Objective_LockboxVoid.objectiveToken.Replace(old, newstring);
            }
            if (lockboxVoidCount == 0)
            {
                Reminders_Main.CompleteObjective(Objective_LockboxVoid);
            }

        }

        public void DeductFreeChestCount()
        {
            if (Objective_FreeChest == null)
            {
                return;
            }
            //Log.LogMessage("FreeChestCount " + freeChestCount);
            if (freeChestCount > 0)
            {
                string old = "(" + freeChestCount + "/";
                freeChestCount--;
                string newstring = "(" + freeChestCount + "/";
                Objective_FreeChest.objectiveToken = Objective_FreeChest.objectiveToken.Replace(old, newstring);
            }
            if (freeChestCount == 0)
            {
                Reminders_Main.CompleteObjective(Objective_FreeChest);
            }
        }

        public void ScrappedSaleStar(CharacterBody body)
        {
            CharacterMaster master = body.master;
            if (Objective_SaleStar != null)
            {
                PlayerCharacterMasterController player;
                if (body && body.master)
                {
                    master = body.master;
                }
                if (!master)
                {
                    Log.LogWarning("Remove Sale Star Objective No master");
                }
                if (master && master.TryGetComponent<PlayerCharacterMasterController>(out player))
                {
                    if (player.networkUser.isLocalPlayer)
                    {
                        Reminders_Main.FailObjective(Objective_SaleStar);
                    }
                }
            }
        }

        public void RemoveFreeChestVoid()
        {
            if (Objective_FreeChestVVVoid != null)
            {
                Reminders_Main.CompleteObjective(this.Objective_FreeChestVVVoid);
            }
        }

    }


}
