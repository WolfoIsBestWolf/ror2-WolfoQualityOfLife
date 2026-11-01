using RoR2;
using RoR2.UI;
using RoR2.Skills;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using WolfoLibrary;

namespace WolfoQoL_Client
{
    //Maybe rework into inspectable character icon but uhhhhhhh work
    //This works for the record
   /*
    public static class UI_InspectableSurvivor
    {
        
        public static void Start()
        {
            GameObject ScoreboardStripObject = null;
            GameObject ClassIcon = null;

            ClassIcon.AddComponent<InspectPanelLocator>();
            HGButton button = ClassIcon.AddComponent<HGButton>();
            




        }
        
      
    }

    public class ClickableSurvivor : MonoBehaviour
    {
        public static GameObject iconPrefab;
        public ScoreboardStrip scoreboardStrip;
        public GameObject InspectSurvivorStrip;

        public List<GameObject> icons;

        public void OnEnable()
        {
            GameObject NavFocusHighlightPrefab = null;
            GameObject NavFocusHighlight = GameObject.Instantiate(NavFocusHighlightPrefab, transform);
            NavFocusHighlight.AddComponent<Canvas>();
            NavFocusHighlight.AddComponent<GraphicRaycaster>();
        }

        public void ItemClicked()
        {
            if (!scoreboardStrip.userBody)
            {
                return;
            }
            if (!scoreboardStrip.userPlayerCharacterMasterController)
            {
                return;
            }
            if (!scoreboardStrip.userPlayerCharacterMasterController.networkUser)
            {
                return;
            }
            PopulateIcons();
        }

        private void PopulateIcons()
        {
            for (int i = 0; i < icons.Count; i++)
            {
                GameObject.Destroy(icons[i]);
            }

            Loadout loadout = scoreboardStrip.userPlayerCharacterMasterController.networkUser.networkLoadout.loadout;

            BodyIndex bodyIndex = scoreboardStrip.userBody.bodyIndex;
            SkillLocator skillLocator = scoreboardStrip.userBody.skillLocator;
            GenericSkill[] skillSlots = skillLocator.allSkills;
            Color bodyColor = scoreboardStrip.userBody.bodyColor;

            GameObject icon = null;
            TooltipProvider tooltip;
            if (skillLocator.passiveSkill.enabled)
            {
                icon = Instantiate(iconPrefab);
                tooltip = icon.GetComponent<TooltipProvider>();
                tooltip.titleColor = bodyColor;
                tooltip.titleToken = skillLocator.passiveSkill.skillNameToken;
                tooltip.bodyToken = skillLocator.passiveSkill.skillNameToken;
                icon.GetComponent<Image>().sprite = skillLocator.passiveSkill.icon;
                icons.Add(icon);
            }
            for (int i = 0; i < skillSlots.Length; i++)
            {
                GenericSkill genericSkill = skillSlots[i];
                if (!genericSkill.hideInCharacterSelect)
                {
                    icon = Instantiate(iconPrefab);
                    tooltip = icon.GetComponent<TooltipProvider>();
                    uint skillVariant = loadout.bodyLoadoutManager.GetSkillVariant(bodyIndex, i);
                    SkillDef skillDef = genericSkill.skillFamily.variants[(int)skillVariant].skillDef;

                    tooltip = icon.GetComponent<TooltipProvider>();
                    tooltip.titleColor = bodyColor;
                    tooltip.titleToken = skillDef.skillNameToken;
                    tooltip.bodyToken = skillDef.skillNameToken;
                    icon.GetComponent<Image>().sprite = skillDef.icon;
                    icons.Add(icon);
                }
            }
        }


 
    }
   */
}
