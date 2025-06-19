using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


namespace WolfoQoL_Client
{
    public class RunStatTracker : MonoBehaviour
    {
        //All of this can be tracked elsewere I think?
        public int Visits_Newt;
        public bool Defeated_ScavLunar;
        public bool Defeated_Brother;
        public bool Beat_VoidField;
        public bool Beat_VoidLocust;
        public bool Defeated_Voidling;
        public int Kills_Aurelionites;
        public bool Defeated_FalseSon;
        public int Kills_Scavs;
        //Highest Item (at least above 5)
        //Highest Enemy (at least above Amount)


    }
    public class Death_BossesBeaten
    {
        public static void Start()
        {

            On.EntityStates.BrotherMonster.TrueDeathState.OnEnter += TrueDeathState_OnEnter;
            On.EntityStates.VoidRaidCrab.DeathState.OnEnter += DeathState_OnEnter;

            On.EntityStates.ScavMonster.Death.OnEnter += Death_OnEnter;
            ArenaMissionController.onBeatArena += ArenaMissionController_onBeatArena;

            On.RoR2.UI.GameEndReportPanelController.SetPlayerInfo += GameEndReportPanelController_SetPlayerInfo;
        }

        private static void GameEndReportPanelController_SetPlayerInfo(On.RoR2.UI.GameEndReportPanelController.orig_SetPlayerInfo orig, GameEndReportPanelController self, RunReport.PlayerInfo playerInfo, int playerIndex)
        {
            orig(self, playerInfo, playerIndex);
            //New stat Accomplishments
            //Major bosses beaten
            //Highest stack of an item?
            //Most of X enemy killed?

            GameObject newStat = GameObject.Instantiate(UI_Stuff.DifficultyStat, self.statContentArea);


         

        }

        private static void ArenaMissionController_onBeatArena()
        {
            Run.instance.SetEventFlag("Defeated_Arena");
        }

        private static void Death_OnEnter(On.EntityStates.ScavMonster.Death.orig_OnEnter orig, EntityStates.ScavMonster.Death self)
        {
            orig(self);
            if (self is EntityStates.ScavMonster.DeathLunar)
            {
                Run.instance.SetEventFlag("Defeated_ScavLunar");
            }
          
        }

        private static void DeathState_OnEnter(On.EntityStates.VoidRaidCrab.DeathState.orig_OnEnter orig, EntityStates.VoidRaidCrab.DeathState self)
        {
            orig(self);
            Run.instance.SetEventFlag("Defeated_VoidRaidCrab");
        }

        private static void TrueDeathState_OnEnter(On.EntityStates.BrotherMonster.TrueDeathState.orig_OnEnter orig, EntityStates.BrotherMonster.TrueDeathState self)
        {
            orig(self);
            Run.instance.SetEventFlag("Defeated_Brother");
        }

        public static void Add_Loadout(On.RoR2.UI.GameEndReportPanelController.orig_SetPlayerInfo orig, global::RoR2.UI.GameEndReportPanelController self, global::RoR2.RunReport.PlayerInfo playerInfo, int playerIndex)
        {
            orig(self, playerInfo, playerIndex);


            if (WConfig.cfgLoadoutOnDeathScreen.Value == WConfig.Position.Off)
            {
                return;
            }
            if (!Run.instance)
            {
                return;
            }
            if (playerInfo.master == null)
            {
                Debug.LogWarning("Loadout Inventory : Null Master");
                return;
            }
            if (playerInfo.master.loadout == null)
            {
                Debug.LogWarning("Loadout Inventory : Null Loadout");
                return;
            }
            if (playerInfo.master.loadout.bodyLoadoutManager == null)
            {
                Debug.LogWarning("Loadout Inventory : Null BodyLoadoutManager");
                return;
            }
            Debug.Log("Add Loutout to run death screen");
            Loadout.BodyLoadoutManager.BodyLoadout loadout = playerInfo.master.loadout.bodyLoadoutManager.GetOrCreateModifiedBodyLoadout(playerInfo.bodyIndex);

            GameEndLoadoutAsStat storage = self.gameObject.GetComponent<GameEndLoadoutAsStat>();
            if (storage == null)
            {
                storage = self.gameObject.AddComponent<GameEndLoadoutAsStat>();
            }
            storage.panel = self;
            storage.SetLoadout(loadout);

        }


    }


 
}
