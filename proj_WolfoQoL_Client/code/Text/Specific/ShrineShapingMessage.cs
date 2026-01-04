using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoQoL_Client.Text
{

    public static class ShrineShapingMessage
    {
        public static void Start()
        {
            On.RoR2.ShrineColossusAccessBehavior.RpcUpdateInteractionClients += ShrineShapingMessage_Client;
            On.RoR2.ShrineColossusAccessBehavior.ReviveAlliedPlayers += ShrineShapingMessage_Host;


        }

        private static void ShrineShapingMessage_Host(On.RoR2.ShrineColossusAccessBehavior.orig_ReviveAlliedPlayers orig, ShrineColossusAccessBehavior self)
        {
            //If there is no way to check if "justSpawned, justRevived, midSpawnAnimation"
            //Then just do it here ig
            try
            {
                ShrineShaping_Message();
            }
            catch (Exception e)
            {
                //Not risking it
                Debug.LogError(e);
                orig(self);
                return;
            }
            orig(self);
        }

        public static string NameOfAllDeadPlayers(out int anyDead)
        {
            string dead = "";
            anyDead = 0;
            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
            {
                if (player.master.lostBodyToDeath)
                {
                    anyDead++;
                    dead += Util.EscapeRichTextForTextMeshPro(player.networkUser.userName) + "'s";
                }
            }
            return dead;
        }

        public static void ShrineShaping_Message()
        {
            if (!WConfig.cfgMessagesShrineRevive.Value)
            {
                return;
            }
            Chat.AddMessage(Language.GetString("SHRINE_REVIVE_USE_MESSAGE"));
            List<string> deadPeopleNames = new List<string>();
            int deadPeople = 0;
            bool youAreDead = false;

            foreach (PlayerCharacterMasterController player in PlayerCharacterMasterController.instances)
            {
                if (!player.isConnected)
                {
                    continue; //?
                }
                CharacterMaster master = player.master;
                if (!master)
                {
                    continue;
                }
                var body = master.GetBodyObject();

                WQoLMain.log.LogMessage(master.lostBodyToDeath + " " + body);


                if (player.master.lostBodyToDeath || body == null)
                {
                    if (master.hasAuthority)
                    {
                        youAreDead = true;
                    }
                    else
                    {
                        deadPeopleNames.Add(Util.EscapeRichTextForTextMeshPro(player.networkUser.userName));
                    }
                    deadPeople++;
                }
            }
            if (deadPeople > 0)
            {
                string token = "";
                string deadNames = "";
                for (int i = 0; i < deadPeopleNames.Count; i++)
                {
                    if (i == 0 && !youAreDead)
                    {
                        //If first, just name
                        //If you, then skip
                        deadNames += deadPeopleNames[i];
                    }
                    else if (i + 1 == deadPeopleNames.Count)
                    {
                        //If is last && not first
                        deadNames += " & " + deadPeopleNames[i];
                    }
                    else
                    {
                        //If middle
                        deadNames += ", " + deadPeopleNames[i];
                    }
                }
                if (deadPeople == 1 && youAreDead)
                {
                    token = "SHRINE_REVIVE_MESSAGE_DEAD_YOU";
                    token = Language.GetString(token);
                }
                else if (youAreDead)
                {
                    token = "SHRINE_REVIVE_MESSAGE_DEAD_YOUAND";
                    token = string.Format(Language.GetString(token), deadNames);
                }
                else
                {
                    token = "SHRINE_REVIVE_MESSAGE_DEAD";
                    token = string.Format(Language.GetString(token), deadNames);
                }
                Chat.AddMessage(token);
            }
        }

        private static void ShrineShapingMessage_Client(On.RoR2.ShrineColossusAccessBehavior.orig_RpcUpdateInteractionClients orig, ShrineColossusAccessBehavior self)
        {
            orig(self);
            if (!NetworkServer.active)
            {
                //This does also run on Host, we just need to do it before revives take place.
                ShrineShaping_Message();
            }
        }

    }

}
