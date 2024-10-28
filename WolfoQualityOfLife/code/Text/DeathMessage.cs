using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace WolfoQualityOfLife
{

    public class DeathMessage
    {
        public static void DetailedDeathMessages(On.RoR2.GlobalEventManager.orig_OnPlayerCharacterDeath orig, GlobalEventManager self, DamageReport damageReport, NetworkUser victimNetworkUser)
        {
            if (!victimNetworkUser || damageReport.victimBody == null || damageReport.damageInfo == null)
            {
                orig(self, damageReport, victimNetworkUser);
                return;
            }

            string VictimName = RoR2.Util.GetBestBodyName(damageReport.victimBody.gameObject);
            string KillerName = "UNIDENTIFIED_KILLER_NAME";
            if (damageReport.attackerBody != null)
            {
                KillerName = RoR2.Util.GetBestBodyName(damageReport.attacker);
                KillerName = KillerName.Replace("\n", " ");
            }
         
            Debug.Log(VictimName);
            Debug.Log(KillerName);


            string token = "";
            if (damageReport.dotType != DotController.DotIndex.None && damageReport.isFriendlyFire == false)
            {
                //Voidtouched Dot Death
                if (damageReport.dotType == DotController.DotIndex.Fracture)
                {
                    token = "DEATH_DOT_FRACTURE";
                }
                else
                {
                    token = "DEATH_DOT_GENERIC";
                }
            }
            else if (damageReport.damageInfo.damageType.damageType.HasFlag(DamageType.BypassArmor | DamageType.BypassBlock) && damageReport.damageInfo.damageColorIndex == DamageColorIndex.Void)
            {
                token = "DEATH_VOID_FOG";
            }
            else if (damageReport.damageInfo.damageType.damageType.HasFlag(DamageType.VoidDeath))
            {
                token = "DEATH_VOID_EXPLODE";
            }
            else if (damageReport.damageInfo.procChainMask.HasProc(ProcType.Thorns))
            {
                token = "DEATH_TWISTED";
            }
            else if (damageReport.isFallDamage)
            {
                token = "DEATH_FALL_DAMAGE";
            }
            else if (damageReport.attackerBody != null && damageReport.isFriendlyFire)
            {
                if (damageReport.attackerBody == damageReport.victimBody)
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
                if (damageReport.damageInfo.delayedDamageSecondHalf)
                {
                    token = "DEATH_ECHO";
                }
                else if (damageReport.attackerBody == null && damageReport.victimBody.inLava)
                {
                    token = "DEATH_LAVA";
                }
                else if (damageReport.victimBody.isGlass)
                {
                    token = "DEATH_GLASS";
                }
                else
                {
                    token = "DEATH_GENERIC";
                }
            }

            float DamageValue = damageReport.damageInfo.damage;
            string damageVal = $"{DamageValue:F2}";


            var temp = new DeathSubjectMessage
            {
                subjectAsNetworkUser = victimNetworkUser,
                baseToken = token,
                victimName = VictimName,
                killerBackupName = KillerName,
                damageDone = damageVal,
                attackerObject = damageReport.attacker
            };
            if (WConfig.NotRequireByAll.Value)
            {
                Chat.AddMessage(temp.ConstructChatString());
            }
            else
            {
                Chat.SendBroadcastChat(temp);
            }

            orig(self, damageReport, victimNetworkUser);
        }

        public class DeathSubjectMessage : RoR2.SubjectChatMessage
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
                if (!base.IsSecondPerson())
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

