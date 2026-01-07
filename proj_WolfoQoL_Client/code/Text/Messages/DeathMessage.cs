using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;
using WolfoLibrary;
using WolfoQoL_Client.DeathScreen;

namespace WolfoQoL_Client.Text
{

    public static class DeathMessage
    {
        public static void OnDeathMessage(On.RoR2.GlobalEventManager.orig_OnPlayerCharacterDeath orig, GlobalEventManager self, DamageReport damageReport, NetworkUser victimNetworkUser)
        {
            try
            {
                orig(self, damageReport, victimNetworkUser);
                DetailedDeathMessages(damageReport);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                orig(self, damageReport, victimNetworkUser);
                return;
            }

        }

        public static void DetailedDeathMessages(DamageReport damageReport)
        {
            float damage = damageReport.damageInfo.damage;
            if (damageReport.damageInfo.crit)
            {
                //Crit damage may vary but whatever
                damage *= 2;
            }
            if (damageReport.damageDealt > damage)
            {
                //If damage dealt is still greater, due to Watches/Crystal
                //Just take it instead
                damage = damageReport.damageDealt;
            }
            DeathAndKillerInventoryMessage(damage, damageReport.victimBody, damageReport.attackerBody, damageReport.attackerMaster, damageReport.damageInfo.damageType, damageReport.damageInfo.damageColorIndex, damageReport.damageInfo.delayedDamageSecondHalf, true);
        }

        public static void DeathAndKillerInventoryMessage(float damage, CharacterBody victimBody, CharacterBody attackerBody, CharacterMaster attackerMaster, DamageTypeCombo damageType, DamageColorIndex dmgColor, bool echo, bool sendOverNetwork)
        {
            if (victimBody == null || victimBody.master == null)
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


            bool isFriendlyFire = attackerBody && attackerBody.teamComponent && attackerBody.teamComponent.teamIndex == victimBody.teamComponent.teamIndex;

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
            string damageVal = string.Empty;
            if (damage > 1000)
            {
                damageVal = $"{damage:F0}";
            }
            else if (damage > 100)
            {
                damageVal = $"{damage:F1}";
            }
            else
            {
                damageVal = $"{damage:F2}";
            }

            DetailedDeathMessage deathMessage = new DetailedDeathMessage
            {
                subjectAsCharacterBody = victimBody,
                baseToken = token,
                victimName = VictimName,
                victimMaster = victimBody.master.gameObject,
                killerBackupName = KillerName,
                damageDone = damageVal,
                attackerObject = attackerBody ? attackerBody.gameObject : null,
            };
            KillerInventoryMessage killerInventoryMessage = new KillerInventoryMessage
            {
                killerBackupName = KillerName,
                attackerObject = attackerBody ? attackerBody.gameObject : null,
                victimMaster = victimBody ? victimBody.master.gameObject : null,
            };
            if (attackerMaster)
            {
                killerInventoryMessage.itemStacks = attackerMaster.inventory.effectiveItemStacks;
                killerInventoryMessage.primaryEquipment = attackerMaster.inventory.currentEquipmentIndex;
            }

            //In actual round this should only run for the Host, or for clients if the Host doesn't have it
            Log.LogMessage(VictimName + " killed by " + KillerName + " | Networked? " + sendOverNetwork);
            if (sendOverNetwork)
            {
                Networker.SendWQoLMessage(deathMessage);
                if (victimBody.master.IsDeadAndOutOfLivesServer())
                {
                    Networker.SendWQoLMessage(killerInventoryMessage);
                }
            }
            else
            {
                Chat.AddMessage(deathMessage);
                Chat.AddMessage(killerInventoryMessage);
            }
        }


        public class DetailedDeathMessage : RoR2.SubjectChatMessage
        {
            public override string ConstructChatString()
            {
                if (!WConfig.module_text_chat.Value)
                {
                    return null;
                }
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

                string result = string.Empty;
                if (base.IsSecondPerson())
                {
                    baseToken += "_2P";
                }
                result += string.Format(Language.GetString(baseToken), victimName, KillerName);
                result += string.Format(Language.GetString("DEATH_DAMAGE"), damageDone);
                victimMaster.GetComponent<PerPlayer_ExtraStatTracker>().latestDetailedDeathMessage = result;
                if (WConfig.cfgMessageDeath.Value == false)
                {
                    return null;
                }
                result = "<style=cDeath>" + result;
                return result;
            }

            public string victimName;
            public string killerBackupName;
            public string damageDone;
            public bool crit;
            public bool doesNotShowEnemy;
            public GameObject attackerObject;
            public GameObject victimMaster;

            public override void Serialize(NetworkWriter writer)
            {
                base.Serialize(writer);
                writer.Write(victimName);
                writer.Write(killerBackupName);
                writer.Write(damageDone);
                writer.Write(attackerObject);
                writer.Write(victimMaster);
                writer.Write(crit);
                //WolfoMain.log.LogWarning("Serialize " + writer);
            }

            public override void Deserialize(NetworkReader reader)
            {
                if (WQoLMain.NoHostInfo == true)
                {
                    return;
                }
                base.Deserialize(reader);
                victimName = reader.ReadString();
                killerBackupName = reader.ReadString();
                damageDone = reader.ReadString();
                attackerObject = reader.ReadGameObject();
                victimMaster = reader.ReadGameObject();
                crit = reader.ReadBoolean();
                //WolfoMain.log.LogWarning("Deserialize " + reader);
            }
        }



    }





}

