using HG;
using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using WolfoLibrary;

namespace WolfoQoL_Client
{
    //Maybe rework into inspectable character icon but uhhhhhhh work
    //This works for the record
    /*
    public static class UI_PassiveIcon
    {
        
        public static void Start()
        {
            HUD.onHudTargetChangedGlobal += HUD_onHudTargetChangedGlobal;
            ScoreboardController.onScoreboardOpen += ScoreboardController_onScoreboardOpen;
            ExtraActions.onScoreboardClosed += ExtraActions_onScoreboardClosed;
        }
        
        private static void ScoreboardController_onScoreboardOpen()
        {
            PassiveIconUIController.instance.gameObject.SetActive(true);
        }
        private static void ExtraActions_onScoreboardClosed()
        {
            PassiveIconUIController.instance.gameObject.SetActive(false);
        }

      

        private static void HUD_onHudTargetChangedGlobal(HUD self)
        {
      
            if (PassiveIconUIController.instance)
            {
                PassiveIconUIController.instance.targetBodyObject = self.targetBodyObject;
                return;
            }
            GameObject passiveIcon = passiveIcon = GameObject.Instantiate(self.skillIcons[0].gameObject);
            passiveIcon.name = "PassiveIcon";
            passiveIcon.SetActive(false);
            PassiveIconUIController passiveUI = passiveIcon.AddComponent<PassiveIconUIController>();
            PassiveIconUIController.instance = passiveUI;


            passiveUI.targetBodyObject = self.targetBodyObject;
            passiveUI.skillIcon = self.skillIcons[0];
            passiveUI.xDif = self.skillIcons[1].transform.localPosition.x - self.skillIcons[0].transform.localPosition.x;

           
            Transform temp = passiveIcon.transform;
            temp.SetParent(self.skillIcons[0].transform.parent, false);

            var a = temp.localPosition;
            a.x = a.x - passiveUI.xDif;
            temp.localPosition = a;

            GameObject.Destroy(passiveIcon.GetComponent<SkillIcon>());
            GameObject.Destroy(passiveIcon.transform.GetChild(6).gameObject);
            GameObject.Destroy(passiveIcon.transform.GetChild(5).gameObject);
            GameObject.Destroy(passiveIcon.transform.GetChild(4).gameObject);
            GameObject.Destroy(passiveIcon.transform.GetChild(3).gameObject);
            GameObject.Destroy(passiveIcon.transform.GetChild(2).gameObject);

           
            self.skillIcons[0].LateUpdate();

            passiveUI.iconImage = passiveIcon.transform.GetChild(1).GetComponent<Image>();

            passiveUI.tooltipProvider = passiveIcon.GetComponent<TooltipProvider>();
            passiveUI.tooltipProvider.overrideBodyText = "";
            passiveUI.tooltipProvider.overrideTitleText = "";

            passiveUI.otherBottons = self.keyItemIcon.transform.parent;
        }

    }

    public class PassiveIconUIController : MonoBehaviour
    {
        public static PassiveIconUIController instance;

        public GameObject targetBodyObject;
        public float xDif;
        public TooltipProvider tooltipProvider;
        public Image iconImage;
        public SkillIcon skillIcon;

        public Transform otherBottons;

        private bool hasPassive;

        public void OnEnable()
        {
            hasPassive = UpdateDisplay();
            if (hasPassive)
            {
                Vector3 a = otherBottons.localPosition;
                a.x = a.x - xDif;
                otherBottons.localPosition = a;
            }
        }

        public void OnDisable()
        {
            if (hasPassive)
            {
                Vector3 a = otherBottons.localPosition;
                a.x = a.x + xDif;
                otherBottons.localPosition = a;
            }
            hasPassive = false;
        }
       
        

        public bool UpdateDisplay()
        {
            if (!targetBodyObject)
            {
                gameObject.SetActive(false);
                return false;
            }
      
            SkillLocator skillLocator = targetBodyObject.GetComponent<SkillLocator>();
            tooltipProvider.titleColor = skillIcon.tooltipProvider.titleColor;

            if (!skillLocator)
            {
                gameObject.SetActive(false);
                return false;
            }
            else if (skillLocator.passiveSkill.enabled)
            {
                iconImage.sprite = skillLocator.passiveSkill.icon;
                tooltipProvider.titleToken = skillLocator.passiveSkill.skillNameToken;
                tooltipProvider.bodyToken = skillLocator.passiveSkill.skillDescriptionToken;
            }
            else if (skillLocator.FindSkillSlot(skillLocator.allSkills[0]) == SkillSlot.None)
            {
                iconImage.sprite = skillLocator.allSkills[0].skillDef.icon;
                tooltipProvider.titleToken = skillLocator.allSkills[0].skillDef.skillNameToken;
                tooltipProvider.bodyToken = skillLocator.allSkills[0].skillDef.skillDescriptionToken;
            }
            else
            {
                gameObject.SetActive(false);
                return false;
            }
            return true;
        }

 
    }
    */
}
