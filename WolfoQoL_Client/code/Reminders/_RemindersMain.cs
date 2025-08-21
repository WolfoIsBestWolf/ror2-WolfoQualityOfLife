using RoR2;
using System.Collections.Generic;
using UnityEngine;
using WolfoFixes;

namespace WolfoQoL_Client.Reminders
{
    public class Reminders_Main
    {
        public static void Start()
        {
            UIBorders.UpdateHuds();

            Reminders_Treasure.Start();
            Reminders_Portal.Start();
            Reminders_SecretGeode.Start();
            Objective_Halcyonite.Start();
            InteractableStuff.Start();
            Reminders_Newt.Start();

            //If you scrap it the reminder should vanish
            //But what if you see the reminder and get another key to unlock the box?

            //If you void it it's just gone so that's fine
            //How would I check this in multiplayer I guess?


            if (WConfig.cfgRemindersGeneral.Value)
            {
                On.RoR2.ScrapperController.BeginScrapping += RemoveReminders_Scrapper;

                On.RoR2.HoldoutZoneController.PreStartClient += CheckForFreeChestVoid;
                On.RoR2.HoldoutZoneController.OnEnable += IsFreeVoidChest_ClearNewt;

                ExtraActions.onMithrixPhase1 += FailAllReminders;
                ExtraActions.onFalseSonPhase1 += FailAllReminders;

                On.EntityStates.VoidRaidCrab.EscapeDeath.OnExit += ClearTreasure_OnVoidlingDeath;
                EntityStates.MeridianEvent.MeridianEventStart.OnMeridianEventStart += ClearReminders_OnFalseSon;
            }
            RoR2.Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        private static void Run_onRunStartGlobal(Run obj)
        {
            obj.gameObject.AddComponent<TreasureReminder>();
        }

        private static void RemoveReminders_Scrapper(On.RoR2.ScrapperController.orig_BeginScrapping orig, ScrapperController self, int intPickupIndex)
        {
            orig(self, intPickupIndex);

            PickupDef pickupDef = PickupCatalog.GetPickupDef(new PickupIndex(intPickupIndex));
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

        private static void IsFreeVoidChest_ClearNewt(On.RoR2.HoldoutZoneController.orig_OnEnable orig, HoldoutZoneController self)
        {
            orig(self);
            if (TreasureReminder.instance)
            {
                if (self.name.StartsWith("VoidShellBattery"))
                {
                    if (TreasureReminder.instance.Objective_FreeChestVVVoid)
                    {
                        //Activate teleporter doesn't get striked through ig
                        Object.Destroy(TreasureReminder.instance.Objective_FreeChestVVVoid);
                    }
                }
                if (self.GetComponent<TeleporterInteraction>())
                {
                    if (TreasureReminder.instance.Objective_NewtShrine)
                    {
                        FailObjective(TreasureReminder.instance.Objective_NewtShrine);
                    }

                }
            }

        }

        private static void CheckForFreeChestVoid(On.RoR2.HoldoutZoneController.orig_PreStartClient orig, HoldoutZoneController self)
        {
            orig(self);
            if (self.name.StartsWith("VoidShellBattery"))
            {
                TreasureReminder.voidFreeChestSpawned = true;
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
                WolfoMain.log.LogMessage("FailObjective Null Objective");
                return;
            }
            objective.markCompletedOnRetired = false;
            objective.objectiveToken = "<color=#808080><s>" + objective.objectiveToken + "</color></s>";
            objective.enabled = false;
        }
        public static void CompleteObjective(GenericObjectiveProvider objective)
        {
            if (!objective)
            {
                WolfoMain.log.LogMessage("CompleteObjective Null Objective");
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
        public bool voidFreeChest = false;
        public bool greenPrinter = false;
        public bool newtAltar = false;
        public void OnEnable()
        {
            if (greenPrinter)
            {
                TreasureReminder.greenPrinterSpawned = true;
            }
            if (newtAltar)
            {
                TreasureReminder.newtShrineSpawned++;
            }
        }
        public void OnDisable()
        {
            if (newtAltar)
            {
                if (TreasureReminder.newtShrineSpawned != 0)
                {
                    TreasureReminder.newtShrineSpawned--;
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

        public static bool greenPrinterSpawned = false;
        public static bool voidFreeChestSpawned = false;
        public static int newtShrineSpawned = 0;


        public GenericObjectiveProvider Objective_Lockbox;
        public GenericObjectiveProvider Objective_LockboxVoid;
        public GenericObjectiveProvider Objective_FreeChest;
        public GenericObjectiveProvider Objective_FreeChestVVVoid;
        public GenericObjectiveProvider Objective_SaleStar;
        public GenericObjectiveProvider Objective_RegenScrap;
        public GenericObjectiveProvider Objective_NewtShrine;
        public GenericObjectiveProvider Objective_SecretGeode;
        public GenericObjectiveProvider Objective_Delusional;

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
            lockboxCount = 0;
            lockboxVoidCount = 0;
            freeChestCount = 0;
            freeChestVoidBool = false;
            localHasSaleStar = false;
            localHasRegenScrap = false;


            int maximumKeys = 0;
            int maximumKeysVoid = 0;
            using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.networkUser && enumerator.Current.networkUser.isLocalPlayer)
                    {
                        if (enumerator.Current.master.inventory.GetItemCount(DLC2Content.Items.LowerPricedChests) > 0) { localHasSaleStar = true; }
                        if (enumerator.Current.master.inventory.GetItemCount(DLC2Content.Items.LowerPricedChestsConsumed) > 0) { localHasSaleStar = true; }
                        if (enumerator.Current.master.inventory.GetItemCount(DLC1Content.Items.RegeneratingScrap) > 0) { localHasRegenScrap = true; }
                        if (enumerator.Current.master.inventory.GetItemCount(DLC1Content.Items.RegeneratingScrapConsumed) > 0) { localHasRegenScrap = true; }
                    }
                    maximumKeys += enumerator.Current.master.inventory.GetItemCount(RoR2Content.Items.TreasureCache);
                    maximumKeysVoid += enumerator.Current.master.inventory.GetItemCount(DLC1Content.Items.TreasureCacheVoid);
                }
            }

            using (IEnumerator<CharacterMaster> enumerator = CharacterMaster.readOnlyInstancesList.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.inventory.GetItemCount(RoR2Content.Items.TreasureCache) > 0) { lockboxCount++; }
                    if (enumerator.Current.inventory.GetItemCount(DLC1Content.Items.TreasureCacheVoid) > 0) { lockboxVoidCount++; }
                    if (enumerator.Current.inventory.GetItemCount(DLC1Content.Items.FreeChest) > 0) { freeChestCount++; }
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
            if (!Run.instance.GetComponent<InfiniteTowerRun>())
            {
                if (SceneInfo.instance && SceneInfo.instance.sceneDef.stageOrder > 5)
                {
                    localHasSaleStar = false;
                    localHasRegenScrap = false;
                }
            }

            if (WConfig.cfgRemindersKeys.Value)
            {
                if (lockboxVoidCount > 0)
                {
                    WolfoMain.log.LogMessage("TreasureCacheVoidCount " + lockboxVoidCount);
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
                    WolfoMain.log.LogMessage("TreasureCacheCount " + lockboxCount);
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
                    WolfoMain.log.LogMessage("FreeChestCount " + freeChestCount);
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
                if (voidFreeChestSpawned)
                {
                    WolfoMain.log.LogMessage("FreeChestVoid Reminder");
                    string token = Language.GetString("REMINDER_FREECHESTVOID");
                    Objective_FreeChestVVVoid = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_FreeChestVVVoid.objectiveToken = token;
                }
            }
            if (WConfig.cfgRemindersRegenScrap.Value != WConfig.ReminderChoice.Off)
            {
                if (greenPrinterSpawned || WConfig.cfgRemindersRegenScrap.Value == WConfig.ReminderChoice.Always)
                {
                    if (localHasRegenScrap)
                    {
                        WolfoMain.log.LogMessage("Local RegenScrap Reminder");
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
                    WolfoMain.log.LogMessage("Local SaleStar Reminder");
                    string token = Language.GetString("REMINDER_SALESTAR");
                    Objective_SaleStar = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_SaleStar.objectiveToken = token;
                }
            }
            if (WConfig.cfgRemindersNewt.Value != WConfig.ReminderChoice.Off)
            {
                if (newtShrineSpawned > 0 || WConfig.cfgRemindersNewt.Value == WConfig.ReminderChoice.Always)
                {
                    WolfoMain.log.LogMessage("Newt Reminder");
                    string token = Language.GetString("REMINDER_NEWT");
                    Objective_NewtShrine = SceneInfo.instance.gameObject.AddComponent<GenericObjectiveProvider>();
                    Objective_NewtShrine.objectiveToken = token;
                }
            }

            voidFreeChestSpawned = false;
            greenPrinterSpawned = false;
            newtShrineSpawned = 0;
        }



        public static void CheckKeysVoided()
        {
            int maximumKeys = 0;
            //int maximumKeysVoid = 0;

            using (IEnumerator<PlayerCharacterMasterController> enumerator = PlayerCharacterMasterController.instances.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    maximumKeys += enumerator.Current.master.inventory.GetItemCount(RoR2Content.Items.TreasureCache);
                    //maximumKeysVoid += enumerator.Current.master.inventory.GetItemCount(DLC1Content.Items.TreasureCacheVoid);
                }
            }
            WolfoMain.log.LogMessage("Check if voided Lockbox reminder : " + maximumKeys);
            ShouldRemoveKeyObjective(maximumKeys);
        }


        public static void ShouldRemoveKeyObjective(int keys)
        {
            if (instance == null)
            {
                return;
            }
            if (!instance.Objective_Lockbox)
            {
                return;
            }
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

        }

        public void DeductLockboxCount()
        {
            if (Objective_Lockbox == null)
            {
                return;
            }

            WolfoMain.log.LogMessage("TreasureCacheCount " + lockboxCount);
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
            WolfoMain.log.LogMessage("TreasureCacheVoidCount " + lockboxVoidCount);
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
            WolfoMain.log.LogMessage("FreeChestCount " + freeChestCount);
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
                    WolfoMain.log.LogWarning("Remove Sale Star Objective No master");
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
