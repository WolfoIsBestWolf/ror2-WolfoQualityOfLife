using RoR2;
using RoR2.UI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using WolfoLibrary;
using System.Text;

namespace WolfoQoL_Client
{
    //Maybe rework into inspectable character icon but uhhhhhhh work
    //This works for the record
    public static class UI_InspectableSurvivor
    {
        public static void Start()
        {
            GameObject HUDSimple = Addressables.LoadAssetAsync<GameObject>(key: "d79990e6848003d438cabcf79e7e5bf7").WaitForCompletion();
            Transform InspectionPanel = HUDSimple.transform.Find("MainContainer/ScoreboardPanel/Container/InspectPanel/InspectPanelArea/InspectionPanel");
            GameObject CharacterSelectUIMain = Addressables.LoadAssetAsync<GameObject>(key: "dec6511c8857bdf4aac9690648fc9d5e").WaitForCompletion();
            GameObject SkillStripPrefab = CharacterSelectUIMain.GetComponent<CharacterSelectController>().skillStripPrefab;

            GameObject ScoreboardStrip = Addressables.LoadAssetAsync<GameObject>(key: "31e5b34853548984190e9e20a6e027d3").WaitForCompletion();
            GameObject ClassBackground = ScoreboardStrip.transform.GetChild(0).GetChild(1).gameObject;
            GameObject KeyItemBackground = ScoreboardStrip.transform.GetChild(0).GetChild(6).gameObject;
            GameObject ItemCounterText = ScoreboardStrip.transform.GetChild(0).GetChild(4).gameObject;

            HGButton ogButton = KeyItemBackground.GetComponent<HGButton>();

            
            ClassBackground.AddComponent<MPEventSystemLocator>(); //What does this do
            HGButton button = ClassBackground.AddComponent<HGButton>();
            UILayerSetFromParent uILayerSetFromParent = ClassBackground.AddComponent<UILayerSetFromParent>();
            ClickableSurvivor custom = ClassBackground.AddComponent<ClickableSurvivor>();

            //Button is too small what dat mean

            button.enabled = false;
            button.hoverToken = "";
            button.uiClickSoundOverride = "";
            button.selectOnPointerEnter = false;
            button.m_Colors = ogButton.colors;
            button.originalColor = ogButton.originalColor;
            button.m_Transition = ogButton.transition;
            button.onClick.m_PersistentCalls.AddListener(
                    new PersistentCall
                    {
                        m_Target = uILayerSetFromParent,
                        m_MethodName = "SetLayerActive",
                        m_Mode = PersistentListenerMode.Bool,
                        m_Arguments = new ArgumentCache
                        {
                            boolArgument = true,
                        }
                    });
           
            uILayerSetFromParent.layerToFind = KeyItemBackground.GetComponent<UILayerSetFromParent>().layerToFind; //What does this do
            
            custom.NavFocusHighlight_reference = KeyItemBackground.transform.GetChild(1).gameObject;


            /*
            /----------------------
            | [2] text | [2] text |
            | [2] text | [2] text |
            |      [2] text       | 
            |---------------------/
            */
            //HUDSimple(Clone)/MainContainer/ScoreboardPanel/Container/InspectPanel/InspectPanelArea/InspectionPanel/HorizontalBox/
            //Box horizontal aligner
            //And then multiple smaller thingies

            #region Prefab
            GameObject SkillBox = R2API.PrefabAPI.InstantiateClone(InspectionPanel.gameObject, "InspectSkillBox", false);
            ClickableSurvivor.boxPrefab = SkillBox;
            Object.DestroyImmediate(SkillBox.GetComponent<VerticalLayoutGroup>());
            GridLayoutGroup grid = SkillBox.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(504f, 80f); //Auto do this in uhh start
            grid.startAxis = GridLayoutGroup.Axis.Vertical;
            grid.padding = new RectOffset(8,8,8,8);
            //grid.spacing = new Vector2(-4, -4);

            GameObject InspectSkill = R2API.PrefabAPI.InstantiateClone(SkillStripPrefab, "InspectSkill", false);
            ClickableSurvivor.iconPrefab = InspectSkill;

            TooltipProvider tooltip = InspectSkill.AddComponent<TooltipProvider>();
            //tooltip.titleToken = "STAT_INFO";
            //tooltip.bodyColor = new Color(0.3255f, 0.4039f, 0.4706f, 0.8706f); //This value does nothing!!!

            Image image = InspectSkill.GetComponent<Image>();
            image.enabled = true; //Needs to be active for tooltip
            //image.color = new Color(0f, 0f, 0f, 0.778f);
            image.color = Color.clear;
            image.sprite = Addressables.LoadAssetAsync<Sprite>(key: "d415f9d76e34e314fb8b990191414fef").WaitForCompletion();
            InspectSkill.GetComponent<LayoutElement>().enabled = true;
            InspectSkill.GetComponent<HGButton>().showImageOnHover = false;

            GameObject.Destroy(InspectSkill.GetComponent<HGButton>());
            GameObject.Destroy(InspectSkill.transform.GetChild(0).GetChild(3).gameObject);
            GameObject.Destroy(InspectSkill.transform.GetChild(0).GetChild(2).gameObject);
            InspectSkill.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<HGTextMeshProUGUI>().alignment = TMPro.TextAlignmentOptions.TopLeft;

            RectTransform skillIcon = InspectSkill.transform.GetChild(0).GetChild(0) as RectTransform;
            skillIcon.sizeDelta = new Vector2(60, 60); //64 64
            skillIcon.anchoredPosition = new Vector2(8, 2); //4 0
            #endregion

            #region ItemCounterFix
            ItemCounterText.transform.GetChild(0).GetComponent<HGTextMeshProUGUI>().raycastTarget = false;
            ItemCounterText.transform.GetChild(1).GetComponent<HGTextMeshProUGUI>().raycastTarget = false;
            #endregion

            On.RoR2.UI.InspectPanelController.Show_InspectInfo_bool_UserProfile += InspectPanelController_Show_InspectInfo_bool_UserProfile;
        }

        private static void InspectPanelController_Show_InspectInfo_bool_UserProfile(On.RoR2.UI.InspectPanelController.orig_Show_InspectInfo_bool_UserProfile orig, InspectPanelController self, InspectInfo ToShow, bool WithSidecar, UserProfile incomingUserProfile)
        {
            orig(self, ToShow, WithSidecar, incomingUserProfile);
            Transform Skills = self.InspectPanel.transform.Find("InspectSkillBox");
            if (Skills)
            {
                self.InspectPanel.transform.GetChild(0).gameObject.SetActive(true);
                Skills.gameObject.SetActive(false);
            }
        }
    }

    public class ClickableSurvivor : MonoBehaviour
    {
        public static GameObject boxPrefab;
        public static GameObject iconPrefab;
        public GameObject NavFocusHighlight_reference;

        public GameObject skillBox;
      
        public ScoreboardStrip scoreboardStrip;
        public GameObject InspectSurvivorStrip;
        public InspectPanelController InspectPanel { get; private set; }
        private Color color;

        //On Awake, do thing
        //This would be on the icon presumably, maybe we need 2 components

        public void Awake()
        {
            this.InspectPanel = base.GetComponentInParent<InspectPanelProvider>().InspectPanel;
            scoreboardStrip = transform.parent.parent.GetComponent<ScoreboardStrip>();

            GameObject newNavFocusHighlight = GameObject.Instantiate(NavFocusHighlight_reference, transform);
            HGButton button = GetComponent<HGButton>();
            button.targetGraphic = newNavFocusHighlight.GetComponent<RawImage>();
            button.onClick.AddListener(new UnityAction(ItemClicked));
            button.enabled = true;

            Transform skillBoxMaybe = InspectPanel.InspectPanel.transform.Find("InspectSkillBox");
            if (skillBoxMaybe != null)
            {
                skillBox = skillBoxMaybe.gameObject;
            }
            else
            {
                skillBox = GameObject.Instantiate(boxPrefab, InspectPanel.InspectPanel.transform);
                skillBox.SetActive(false);
                skillBox.name = "InspectSkillBox";
            }
            InspectPanel.InspectPanel.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OnDisable()
        {
            for (int i = 0; i < skillBox.transform.childCount; i++)
            {
                Destroy(skillBox.transform.GetChild(i).gameObject);
            }
        }

        public void ItemClicked()
        {
            InspectPanel.InspectPanel.SetActive(false);
            Debug.Log("SurvivorClicked");
            if (!scoreboardStrip || !scoreboardStrip.userPlayerCharacterMasterController)
            {
                return;
            }
            if (!scoreboardStrip.userPlayerCharacterMasterController.networkUser)
            {
                return;
            }
            skillBox.SetActive(true);
            InspectPanel.InspectPanel.SetActive(true);
            InspectPanel.InspectPanel.transform.GetChild(0).gameObject.SetActive(false);

            PopulateIcons();
            Pause();
        }
        //WHY THE FUCK IT SOMETIMES WORKS SOMETIMES NOT (the spacing of the icon)

        private void Pause()
        {
            bool useInspectAutopause = InspectPanel.eventSystem.localUser.userProfile.useInspectAutopause;
            if (InspectPanel.isAutopauseCandidate && useInspectAutopause && RoR2Application.isInSinglePlayer)
            {
                PauseStopController instance = PauseStopController.instance;
                if (instance == null)
                {
                    return;
                }
                instance.Pause(true);
            }
        }

        private void PopulateIcons()
        {
            //If last inspectee same then don't bother?
            for (int i = 0; i < skillBox.transform.childCount; i++)
            {
                Destroy(skillBox.transform.GetChild(i).gameObject);
            }

            Loadout loadout = scoreboardStrip.userPlayerCharacterMasterController.networkUser.networkLoadout.loadout;

            BodyIndex bodyIndex = BodyIndex.None;
            SkillLocator skillLocator = null;
            CharacterBody body = scoreboardStrip.userBody;
            if (body == null) 
            {
                bodyIndex = scoreboardStrip.userPlayerCharacterMasterController.networkUser.bodyIndexPreference;
                body = BodyCatalog.GetBodyPrefabBodyComponent(bodyIndex);
                skillLocator = body.GetComponent<SkillLocator>();
            }
            else
            {
                skillLocator = scoreboardStrip.userBody.skillLocator;
                bodyIndex = scoreboardStrip.userBody.bodyIndex;
            }
          
            if (skillLocator.passiveSkill.enabled)
            {
                var passive = skillLocator.passiveSkill;
                MakeIcon(body.bodyColor, passive.skillNameToken, passive.skillDescriptionToken, passive.keywordToken, passive.icon);
            }
            for (int i = 0; i < skillLocator.allSkills.Length; i++)
            {
                GenericSkill genericSkill = skillLocator.allSkills[i];
                if (true) //!genericSkill.hideInLoadoutSelect
                {
                    SkillDef skillDef = null;
                    if (body)
                    {
                        skillDef = genericSkill.skillDef;
                    }
                    else
                    {
                        uint skillVariant = loadout.bodyLoadoutManager.GetSkillVariant(bodyIndex, i);
                        skillDef = genericSkill.skillFamily.variants[(int)skillVariant].skillDef;
                    }

                    string keywordString = string.Empty;
                    if (skillDef.keywordTokens != null)
                    {
                        StringBuilder stringBuilder = HG.StringBuilderPool.RentStringBuilder();
                        for (int j = 0; j < skillDef.keywordTokens.Length; j++)
                        {
                            string @string = Language.GetString(skillDef.keywordTokens[j]);
                            stringBuilder.Append(@string).Append("\n\n");
                        }
                        keywordString = stringBuilder.ToString();
                        stringBuilder = HG.StringBuilderPool.ReturnStringBuilder(stringBuilder);
                    }
                    MakeIcon(body.bodyColor, skillDef.skillNameToken, skillDef.skillDescriptionToken, keywordString, skillDef.icon);
                }
            }

           
            /*GridLayoutGroup grid = skillBox.GetComponent<GridLayoutGroup>();
            if (skillBox.transform.childCount > 6)
            {
                if (skillBox.transform.childCount == 7)
                {
                    FakeIcon(5);
                }
                grid.constraintCount = 3;
                grid.cellSize = new Vector2(1008f / 3f, 90f);

            }
            else
            {
                grid.constraintCount = 2;
                grid.cellSize = new Vector2(1008f / 2f, 90f);
            }*/
        }

        //Ig like replace fukin lunar yeah

        private void MakeIcon(Color bodyColor, string nameToken, string descToken, string keywords, Sprite sprite)
        {
            GameObject icon = Instantiate(iconPrefab);
            icon.transform.SetParent(skillBox.transform, false);
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            icon.SetActive(true);

            HGTextMeshProUGUI name = icon.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<HGTextMeshProUGUI>();
            name.color = bodyColor;
            name.text = Language.GetString(nameToken);
            HGTextMeshProUGUI desc = icon.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<HGTextMeshProUGUI>();
            desc.alignment = TMPro.TextAlignmentOptions.TopLeft; //Refuses to work if not set here
            desc.text = Language.GetString(descToken);
            icon.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sprite;

            TooltipProvider tooltipProvider = icon.GetComponent<TooltipProvider>();
            if (string.IsNullOrEmpty(keywords))
            {
                tooltipProvider.enabled = false;
            }
            else
            {
                tooltipProvider.titleColor = bodyColor;
                tooltipProvider.enabled = true;
                icon.GetComponent<TooltipProvider>().bodyToken = keywords;
            }

        }
        
        private void FakeIcon(int siblingIndex)
        {
            GameObject icon = Instantiate(iconPrefab);
            icon.transform.SetParent(skillBox.transform, false);
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            icon.transform.SetSiblingIndex(siblingIndex);
            Destroy(icon.transform.GetChild(0).gameObject);
            icon.SetActive(true);
        }

        //Maybe we can have keywords on hover idk like a tooltip?
        //Yeah tooltip for keywords

        private void MakeColumns()
        {
            int columns = 0;
        }
 
    }
  
}
