using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace WolfoQoL_Client
{

    public class DeathMessage
    {
        public static void OnDeathMessage(On.RoR2.GlobalEventManager.orig_OnPlayerCharacterDeath orig, GlobalEventManager self, DamageReport damageReport, NetworkUser victimNetworkUser)
        {
            try
            {
                if (WolfoMain.ShouldHostGiveInfo)
                {
                    DetailedDeathMessages(damageReport);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                orig(self, damageReport, victimNetworkUser);
                return;
            }
            orig(self, damageReport, victimNetworkUser);
        }

        public static void DetailedDeathMessages(DamageReport damageReport)
        {
            DetailedDeathMessages(damageReport.damageDealt, damageReport.victimBody, damageReport.attackerBody, damageReport.damageInfo.damageType, damageReport.damageInfo.damageColorIndex, damageReport.damageInfo.delayedDamageSecondHalf, WolfoMain.ShouldHostGiveInfo);
        }

        public static void DetailedDeathMessages(float damage, CharacterBody victimBody, CharacterBody attackerBody, DamageTypeCombo damageType, DamageColorIndex dmgColor, bool echo, bool sendOverNetwork)
        {
            if (WConfig.cfgMessageDeath.Value == false)
            {
                return;
            }
            string VictimName = Util.GetBestBodyName(victimBody.gameObject);
            string KillerName = "UNIDENTIFIED_KILLER_NAME";
            if (attackerBody != null)
            {
                KillerName = Util.GetBestBodyName(attackerBody.gameObject);
                KillerName = KillerName.Replace("\n", " ");
            }


            bool isFriendlyFire = attackerBody && attackerBody.teamComponent.teamIndex == victimBody.teamComponent.teamIndex;

            bool isFallDamage = damageType.damageType.HasFlag(DamageType.FallDamage);

            bool isDoT = damageType.damageType.HasFlag(DamageType.DoT);

            string token = "";
            if (isDoT && isFriendlyFire == false)
            {
                //Voidtouched Dot Death
                if (isDoT && dmgColor == DamageColorIndex.Void)
                {
                    token = "DEATH_DOT_FRACTURE";
                }
                else
                {
                    token = "DEATH_DOT_GENERIC";
                }
            }
            else if (damageType.damageType.HasFlag(DamageType.BypassArmor | DamageType.BypassBlock) && dmgColor == DamageColorIndex.Void)
            {
                token = "DEATH_VOID_FOG";
            }
            else if (damageType.damageType.HasFlag(DamageType.VoidDeath))
            {
                token = "DEATH_VOID_EXPLODE";
            }
            /*else if (damageReport.damageInfo.damageType.damageType.HasFlag(DamageType.LunarRuin))
            {
                token = "DEATH_TWISTED";
            }*/
            else if (isFallDamage)
            {
                token = "DEATH_FALL_DAMAGE";
            }
            else if (attackerBody != null && isFriendlyFire)
            {
                if (attackerBody == victimBody)
                {
                    token = "DEATH_FRIENDLY_SUICIDE";
                }
                else
                {
                    token = "DEATH_FRIENDLY";
                }
            }
            else
            {
                if (echo)
                {
                    token = "DEATH_ECHO";
                }
                else if (attackerBody == null && victimBody.inLava)
                {
                    token = "DEATH_LAVA";
                }
                else if (victimBody.isGlass)
                {
                    token = "DEATH_GLASS";
                }
                else
                {
                    token = "DEATH_GENERIC";
                }
            }

            string damageVal = $"{damage:F2}";


            var temp = new DetailedDeathMessage
            {
                subjectAsCharacterBody = victimBody,
                baseToken = token,
                victimName = VictimName,
                killerBackupName = KillerName,
                damageDone = damageVal,
                attackerObject = attackerBody ? attackerBody.gameObject : null,
            };
            //Chat.AddMessage(temp.ConstructChatString());

            //In actual round this should only run for the Host, or for clients if the Host doesn't have it
            if (sendOverNetwork)
            {
                Chat.SendBroadcastChat(temp);
            }
            else
            {
                Chat.AddMessage(temp);
            }
            Debug.Log(VictimName + " killed by " + KillerName + " | Networked? " + sendOverNetwork);



        }


        public class DetailedDeathMessage : RoR2.SubjectChatMessage
        {
            public override string ConstructChatString()
            {
                string KillerName = "";
                if (attackerObject != null)
                {
                    KillerName = Util.GetBestBodyName(attackerObject);
                    KillerName = KillerName.Replace("\n", " ");
                }
                else
                {
                    KillerName = Language.GetString(killerBackupName);
                }

                string result = "<style=cDeath>";
                if (base.IsSecondPerson())
                {
                    baseToken += "_2P";
                }
                result += string.Format(Language.GetString(baseToken), victimName, KillerName);
                result += string.Format(Language.GetString("DEATH_DAMAGE"), damageDone);

                return result;
            }

            public string victimName;
            public string killerBackupName;
            public string damageDone;
            public bool doesNotShowEnemy;
            public GameObject attackerObject;

            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(victimName);
                writer.Write(killerBackupName);
                writer.Write(damageDone);
                writer.Write(attackerObject);
                //Debug.LogWarning("Serialize " + writer);
            }

            public override void Deserialize(NetworkReader reader)
            {
                base.Deserialize(reader);
                victimName = reader.ReadString();
                killerBackupName = reader.ReadString();
                damageDone = reader.ReadString();
                attackerObject = reader.ReadGameObject();
                //Debug.LogWarning("Deserialize " + reader);
            }
        }



    }





}

