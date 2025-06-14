using RoR2;
using RoR2.Skills;
using RoR2.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


namespace WolfoQoL_Client
{
    public class Death_Loadout
    {


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
                GameObject difficulty = panel.selectedDifficultyLabel.gameObject.transform.parent.gameObject;
                rootObject = Instantiate(difficulty);
                rootObject.name = "LoadoutRootStrip";
                rootObject.transform.SetParent(difficulty.transform.parent, false);

                MPEventSystemLocator whatever1 = difficulty.GetComponent<RoR2.UI.MPEventSystemLocator>();
                MPEventSystemLocator whatever2 = rootObject.GetComponent<RoR2.UI.MPEventSystemLocator>();
                whatever2.eventSystemProvider = whatever2.eventSystemProvider;
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
                image.sprite = Addressables.LoadAssetAsync<Sprite>(key: "RoR2/Base/UI/texUIHighlightBoxOutline.png").WaitForCompletion(); ;
                image.type = Image.Type.Sliced;

                if (WConfig.cfgLoadoutOnDeathScreen.Value == WConfig.Position.Top)
                {
                    rootObject.transform.SetSiblingIndex(1);
                }
            }
            setup = true;
            scaleBig = scale * 1.25f;
        }

        public void SetLoadout(Loadout.BodyLoadoutManager.BodyLoadout loadout)
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
                Debug.LogWarning("Null BodyPrefab");
                return;
            }
            Color color = bodyPrefab.GetComponent<CharacterBody>().bodyColor;
            List<GenericSkill> gameObjectComponents = GetComponentsCache<GenericSkill>.GetGameObjectComponents(bodyPrefab);

            int length = loadout.skillPreferences.Length;
            for (int i = 0; i < length; i++)
            {
                GenericSkill skillSlot = gameObjectComponents[i];
                SkillDef def = skillSlot.skillFamily.variants[loadout.skillPreferences[i]].skillDef;
                //Debug.Log(def);
                GameObject icon = new GameObject("SkillIcon");
                icon.transform.SetParent(skills, false);
                icon.AddComponent<Image>();
                icon.GetComponent<RectTransform>().sizeDelta = new Vector2(scale, scale);
                icon.GetComponent<Image>().sprite = def.icon;
                TooltipProvider tooltipProvider = icon.AddComponent<TooltipProvider>();
                tooltipProvider.titleColor = color;
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
