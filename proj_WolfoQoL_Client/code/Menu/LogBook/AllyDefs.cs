using RoR2;
using RoR2.ExpansionManagement;
using RoR2.Stats;
using RoR2.UI.LogBook;
using System.Collections.Generic;
using UnityEngine;
using WolfoLibrary;

namespace WolfoQoL_Client
{
    public struct AllyDef
    {



        public ItemTier tier;
        public float desiredSortPosition;
        //public string nameToken;
        public ColorCatalog.ColorIndex colorOverride;
        public Texture bgTextureOverride;
        public ExpansionDef requiredExpansion;

    }
    public class AllyCatalog
    {
        public static Dictionary<BodyIndex, AllyDef> AllyDefs;
        public static void Make()
        {
            Debug.Log("AllyCatalog");
            RoR2Content.DroneDefs.BackupDrone.colorOverride = ColorCatalog.ColorIndex.Equipment;
            DLC3Content.DroneDefs.DTGunnerDrone.colorOverride = ColorCatalog.ColorIndex.LunarCoin;
            DLC3Content.DroneDefs.DTHealDrone.colorOverride = ColorCatalog.ColorIndex.LunarCoin;
            DLC3Content.DroneDefs.DTHaulerDrone.colorOverride = ColorCatalog.ColorIndex.LunarCoin;

            bool RealerCheatsInstalled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("prodzpod.RealerCheatUnlocks");

            if (AllyDefs != null)
            {
                return;
            }
            AllyDefs = new Dictionary<BodyIndex, AllyDef>();

            if (!WConfig.cfgLogbook_AllyExpansion.Value || RealerCheatsInstalled)
            {
                return;
            }
            if (Language.currentLanguage.TokenIsRegistered("LOGBOOK_CATEGORY_DRONE_AND_ALLIES"))
            {
                R2API.LanguageAPI.Add("LOGBOOK_CATEGORY_DRONE", Language.GetString("LOGBOOK_CATEGORY_DRONE_AND_ALLIES"));
            }


            //SquidTurret

            //Oprhan Core

            //Beetle Guard
            //Empathy1
            //Empathy2
            //Defense Nucleus

            AllyDefs.Add(MissedContent.BodyPrefabs.SquidTurretBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 200,
                tier = ItemTier.Tier2,
            });
            AllyDefs.Add(DLC3Content.BodyPrefabs.FriendUnitBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 200,
                tier = ItemTier.Tier3,
                requiredExpansion = DLCS.DLC3,
            });

            AllyDefs.Add(MissedContent.BodyPrefabs.BeetleGuardAllyBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 1,
                tier = ItemTier.Boss,
            });
            AllyDefs.Add(MissedContent.BodyPrefabs.RoboBallRedBuddyBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 2,
                tier = ItemTier.Boss,
            });
            AllyDefs.Add(MissedContent.BodyPrefabs.RoboBallGreenBuddyBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 2,
                tier = ItemTier.Boss,
            });
            AllyDefs.Add(DLC1Content.BodyPrefabs.MinorConstructOnKillBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 3,
                tier = ItemTier.Boss,
                requiredExpansion = DLCS.DLC1
            });

            AllyDefs.Add(RoR2Content.BodyPrefabs.EngiTurretBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 30,
                tier = ItemTier.VoidTier1,
                colorOverride = ColorCatalog.ColorIndex.LunarCoin,
                bgTextureOverride = LegacyResourcesAPI.Load<Texture>("Textures/ItemIcons/BG/texSurvivorBGIcon"),
            });
            AllyDefs.Add(RoR2Content.BodyPrefabs.EngiWalkerTurretBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 31,
                tier = ItemTier.VoidTier1,
                colorOverride = ColorCatalog.ColorIndex.LunarCoin,
                bgTextureOverride = LegacyResourcesAPI.Load<Texture>("Textures/ItemIcons/BG/texSurvivorBGIcon"),
            });

            AllyDefs.Add(RoR2Content.BodyPrefabs.NullifierAllyBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 3,
                tier = ItemTier.VoidBoss,
                requiredExpansion = DLCS.DLC1
            });
            AllyDefs.Add(DLC1Content.BodyPrefabs.VoidJailerAllyBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 3,
                tier = ItemTier.VoidBoss,
                requiredExpansion = DLCS.DLC1
            });
            AllyDefs.Add(DLC1Content.BodyPrefabs.VoidMegaCrabAllyBody.bodyIndex, new AllyDef
            {
                desiredSortPosition = 3,
                tier = ItemTier.VoidBoss,
                requiredExpansion = DLCS.DLC1
            });

            On.RoR2.UI.LogBook.PageBuilder.DroneBody += PageBuilder_DroneBody;
        }

        private static void PageBuilder_DroneBody(On.RoR2.UI.LogBook.PageBuilder.orig_DroneBody orig, PageBuilder builder)
        {
            CharacterBody characterBody = (CharacterBody)builder.entry.extraData;
            if (DroneCatalog.GetDroneIndexFromBodyIndex(characterBody.bodyIndex) == DroneIndex.None)
            {
                //AllyDefs[characterBody.bodyIndex].descToken
                string allyDesc = characterBody.name.Replace("Body", "_SKILL_DESCRIPTION").ToUpper();
                if (Language.english.TokenIsRegistered(allyDesc))
                {
                    builder.AddDescriptionPanel(Language.GetString(allyDesc));
                }
                builder.AddSimpleBody(characterBody);
                builder.AddDronePanel(characterBody);
                builder.AddBodyLore(characterBody);
            }
            else
            {
                orig(builder);
            }
        }

        public static bool CanSelectDroneEntry(AllyDef allyDef, Dictionary<ExpansionDef, bool> expansionAvailability)
        {
            ExpansionDef requiredExpansion = allyDef.requiredExpansion;
            return !requiredExpansion || !expansionAvailability.ContainsKey(requiredExpansion) || expansionAvailability[requiredExpansion];
        }

        public static EntryStatus GetAllyStatus(in Entry entry, UserProfile viewerProfile)
        {
            if (viewerProfile == null)
            {
                return EntryStatus.Unencountered;
            }
            if (viewerProfile.statSheet.GetStatValueULong(PerBodyStatDef.timesSummoned, ((CharacterBody)entry.extraData).gameObject.name) > 0UL)
            {
                return EntryStatus.Available;
            }
            return EntryStatus.Unencountered;
        }

        public static ItemTier TierFromBodyDroneOrAlly(CharacterBody body)
        {
            DroneIndex index = DroneCatalog.GetDroneIndexFromBodyIndex(body.bodyIndex);
            if (index != DroneIndex.None)
            {
                return DroneCatalog.GetDroneDef(index).tier;
            }
            if (AllyCatalog.AllyDefs.TryGetValue(body.bodyIndex, out var allyDef))
            {
                return allyDef.tier;
            }
            return ItemTier.Tier1;
        }
        public static float SortFromBodyDroneOrAlly(CharacterBody body)
        {
            DroneIndex index = DroneCatalog.GetDroneIndexFromBodyIndex(body.bodyIndex);
            if (index != DroneIndex.None)
            {
                return DroneCatalog.GetDroneDef(index).desiredSortPosition;
            }
            if (AllyCatalog.AllyDefs.TryGetValue(body.bodyIndex, out var allyDef))
            {
                return allyDef.desiredSortPosition;
            }
            return 0f;
        }
    }

}