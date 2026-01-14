using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


namespace WolfoQoL_Client.DeathScreen
{
    public static class LoadoutStat
    {


        public static void Add_Loadout(GameEndReportPanelController self, RunReport.PlayerInfo playerInfo)
        {


            if (WConfig.DC_Loadout.Value == false)
            {
                return;
            }
            if (playerInfo.master == null)
            {
                Log.LogWarning("Loadout Inventory : Null Master");
                return;
            }
            if (playerInfo.master.loadout == null)
            {
                Log.LogWarning("Loadout Inventory : Null Loadout");
                return;
            }
            if (playerInfo.master.loadout.bodyLoadoutManager == null)
            {
                Log.LogWarning("Loadout Inventory : Null BodyLoadoutManager");
                return;
            }
            Log.LogMessage("Add Loadout to run death screen");
            Loadout.BodyLoadoutManager.BodyLoadout loadout = playerInfo.master.loadout.bodyLoadoutManager.GetOrCreateModifiedBodyLoadout(playerInfo.bodyIndex);

            GameEndLoadoutAsStat storage = self.gameObject.GetComponent<GameEndLoadoutAsStat>();
            if (storage == null)
            {
                storage = self.gameObject.AddComponent<GameEndLoadoutAsStat>();
            }
            storage.panel = self;
            storage.SetLoadout(loadout, playerInfo.itemStacks);

        }


    }



    public class GameEndLoadoutAsStat : MonoBehaviour
    {
        public GameEndReportPanelController panel;
        public GameObject rootObject;
        public GameObject skillRoot;
        public GameObject backgroundObject;
        public Transform skills;
        public RectTransform background;
        public Loadout.BodyLoadoutManager.BodyLoadout latest;
        public static float scale = 40f;
        public float scaleBig = 50f;

        private bool setup = false;

        public void SetupStruct()
        {
            if (panel == null)
            {
                panel = GetComponent<GameEndReportPanelController>();
            }
            if (rootObject == null)
            {
                //GameObject difficulty = panel.selectedDifficultyLabel.gameObject.transform.parent.gameObject;
                GameObject difficulty = DeathScreenExpanded.difficulty_stat;
                rootObject = Instantiate(difficulty);
                rootObject.name = "LoadoutRootStrip";
                rootObject.transform.SetParent(panel.statContentArea, false);
                rootObject.GetComponent<LayoutElement>().preferredWidth = 300;
                MPEventSystemLocator whatever1 = difficulty.GetComponent<RoR2.UI.MPEventSystemLocator>();
                MPEventSystemLocator whatever2 = rootObject.GetComponent<RoR2.UI.MPEventSystemLocator>();
                whatever2.eventSystemProvider = whatever1.eventSystemProvider;
                whatever2.eventSystem = whatever1.eventSystem;

                LanguageTextMeshController lang = rootObject.transform.GetChild(0).GetComponent<RoR2.UI.LanguageTextMeshController>();
                lang.token = Language.GetString("SURVIVOR_LOADOUT") + ":";
                lang.OnEnable();
                Destroy(rootObject.transform.Find("SelectedDifficultyLabel").gameObject);
                Destroy(rootObject.transform.Find("SelectedDifficultyIcon").gameObject);

                GameObject middle = new GameObject("Holder");
                middle.transform.SetParent(rootObject.transform, false);
                middle.AddComponent<RectTransform>();
                GameObject middle2 = new GameObject("Holder2");
                middle2.transform.SetParent(middle.transform, false);
                middle2.transform.localPosition = new Vector3(-20f, 0f, 0f);

                backgroundObject = new GameObject("Background");
                backgroundObject.transform.SetParent(middle2.transform, false);

                skillRoot = new GameObject("SkillGroup");
                skillRoot.transform.SetParent(middle2.transform, false);
                skills = skillRoot.transform;


                background = backgroundObject.AddComponent<RectTransform>();
                Image image = backgroundObject.AddComponent<Image>();
                image.sprite = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/Base/UI/texUIHighlightBoxOutline.png").WaitForCompletion();
                image.type = Image.Type.Sliced;
                image.color = new Color(1f, 1f, 1f, 0.7f);
                rootObject.transform.SetSiblingIndex(1);
            }
            setup = true;
            scaleBig = scale * 1.25f;

            if (panel.artifactDisplayPanelController.gameObject.activeSelf)
            {
                ExtraStats.CombineDifficultyLoadout(panel.statContentArea); //
            }

        }

        public void SetLoadout(Loadout.BodyLoadoutManager.BodyLoadout loadout, int[] inventory)
        {


            latest = loadout;
            if (!setup)
            {
                SetupStruct();
            }
            else
            {
                for (int i = 0; i < skills.childCount; i++)
                {
                    Destroy(skills.GetChild(i).gameObject);
                }
            }
            GameObject bodyPrefab = BodyCatalog.GetBodyPrefab(loadout.bodyIndex);
            if (!bodyPrefab)
            {
                Log.LogWarning("Null BodyPrefab");
                return;
            }
            var body = bodyPrefab.GetComponent<CharacterBody>();
            body.bodyColor.a = 1;
            Color color = body.bodyColor;
            List<GenericSkill> gameObjectComponents = GetComponentsCache<GenericSkill>.GetGameObjectComponents(bodyPrefab);
            SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();


            int length = loadout.skillPreferences.Length;
            for (int i = 0; i < length; i++)
            {
                Color color2 = color;
                GenericSkill skillSlot = gameObjectComponents[i];
                SkillDef def = skillSlot.skillFamily.variants[loadout.skillPreferences[i]].skillDef;

                SkillSlot slot = skillLocator.FindSkillSlot(skillSlot);
                if (slot != SkillSlot.None)
                {
                    switch (slot)
                    {
                        case SkillSlot.Primary:
                            if (inventory[(int)RoR2Content.Items.LunarPrimaryReplacement.itemIndex] > 0)
                            {
                                def = CharacterBody.CommonAssets.lunarPrimaryReplacementSkillDef;
                                color2 = new Color(0.2018f, 0.182f, 0.2736f, 1f);
                            }
                            break;
                        case SkillSlot.Secondary:
                            bool heresy2 = inventory[(int)RoR2Content.Items.LunarSecondaryReplacement.itemIndex] > 0;
                            if (inventory[(int)RoR2Content.Items.LunarSecondaryReplacement.itemIndex] > 0)
                            {
                                def = CharacterBody.CommonAssets.lunarSecondaryReplacementSkillDef;
                                color2 = new Color(0.2018f, 0.182f, 0.2736f, 1f);
                            }
                            break;
                        case SkillSlot.Utility:
                            if (inventory[(int)RoR2Content.Items.LunarUtilityReplacement.itemIndex] > 0)
                            {
                                def = CharacterBody.CommonAssets.lunarUtilityReplacementSkillDef;
                                color2 = new Color(0.2018f, 0.182f, 0.2736f, 1f);
                            }
                            break;
                        case SkillSlot.Special:
                            if (inventory[(int)RoR2Content.Items.LunarSpecialReplacement.itemIndex] > 0)
                            {
                                def = CharacterBody.CommonAssets.lunarSpecialReplacementSkillDef;
                                color2 = new Color(0.2018f, 0.182f, 0.2736f, 1f);
                            }
                            break;


                    }
                }

                //WolfoMain.log.LogMessage(def);
                GameObject icon = new GameObject("SkillIcon");
                icon.transform.SetParent(skills, false);
                icon.AddComponent<Image>().sprite = def.icon;
                icon.GetComponent<RectTransform>().sizeDelta = new Vector2(scale, scale);
                TooltipProvider tooltipProvider = icon.AddComponent<TooltipProvider>();
                tooltipProvider.titleColor = color2;
                tooltipProvider.titleToken = def.skillNameToken;
                tooltipProvider.bodyToken = def.skillDescriptionToken;

                icon.transform.localPosition = new Vector3((-length + i + 1) * scale, 0, 0);
                icon.SetActive(true);
            }

            background.sizeDelta = new Vector2(scaleBig, scaleBig);
            background.offsetMax = new Vector2(scaleBig * 0.5f, scaleBig * 0.5f);
            background.offsetMin = new Vector2(-(loadout.skillPreferences.Length * scale - scale * 0.375f), scaleBig * -0.5f);






        }

    }

}
