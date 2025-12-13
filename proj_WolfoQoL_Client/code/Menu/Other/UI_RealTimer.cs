using MonoMod.Cil;
using RoR2;
using RoR2.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace WolfoQoL_Client
{
    public class UI_RealTimeTimer
    {
        public static bool scoreboardOpen = false;
      
        public static void Start()
        {
            On.RoR2.UI.ScoreboardController.OnEnable += ScoreboardController_OnEnable;
            On.RoR2.UI.ScoreboardController.OnDisable += ScoreboardController_OnDisable;
            On.RoR2.Run.InstantiateUi += Run_InstantiateUi;

            //Classic Hud && Simu Hud??
            var format = ScriptableObject.CreateInstance<TimerStringFormatter>();
            RealTimerUIController.format = format;
            format.format = new TimerStringFormatter.Format
            {
                prefix = "<mspace=0.5em>",
                suffix = "</mspace>",
                units = new TimerStringFormatter.Format.Unit[]
                {
                    new TimerStringFormatter.Format.Unit
                    {
                        name = "minutes",
                        conversionRate = 60,
                        minDigits = 2,
                        maxDigits = uint.MaxValue,
                    },
                    new TimerStringFormatter.Format.Unit
                    {
                         name = "seconds",
                         prefix = ":",
                        conversionRate = 1,
                        minDigits = 2,
                        maxDigits = 2,
                    }
                }
            };
            
            GameObject Timer = Addressables.LoadAssetAsync<GameObject>(key: "23b0bcee7bc7a0c4d91d884c778cc70a").WaitForCompletion().transform.GetChild(0).GetChild(1).gameObject;


        }
        private static GameObject Timer = Addressables.LoadAssetAsync<GameObject>(key: "23b0bcee7bc7a0c4d91d884c778cc70a").WaitForCompletion().transform.GetChild(0).GetChild(1).gameObject;

        private static GameObject Run_InstantiateUi(On.RoR2.Run.orig_InstantiateUi orig, Run self, Transform uiRoot)
        {
            GameObject UI = orig(self, uiRoot);

            UI.transform.GetChild(0);

            GameObject RealTimeTimer = GameObject.Instantiate(Timer, UI.transform.GetChild(0));

            Object.Destroy(RealTimeTimer.GetComponent<RunTimerUIController>());
            var rl = RealTimeTimer.AddComponent<RealTimerUIController>();
            rl.runStopwatchTimerTextController = RealTimeTimer.GetComponent<TimerText>();
            rl.Setup();
            RealTimerUIController.instance = RealTimeTimer;
            RealTimeTimer.SetActive(false);
            return UI;
        }

        private static void ScoreboardController_OnDisable(On.RoR2.UI.ScoreboardController.orig_OnDisable orig, ScoreboardController self)
        {
            orig(self);
            if (RealTimerUIController.instance)
            {
                RealTimerUIController.instance.SetActive(false);
            }
           
        }

        private static void ScoreboardController_OnEnable(On.RoR2.UI.ScoreboardController.orig_OnEnable orig, ScoreboardController self)
        {
            orig(self);
            if (RealTimerUIController.instance)
            {
                if (WConfig.RealTimeTimer.Value)
                {
                    RealTimerUIController.instance.SetActive(true);
                } 
            }
            
        }
    }
    public class RealTimerUIController : MonoBehaviour
    {
        public static TimerStringFormatter format;
        public static GameObject instance;
        public Transform realTimer;
        public float runStartingOffset;

        //We could time stamp on run start and use that

        public void OnEnable()
        {
            runStartingOffset = 0.2f;
            realTimer.localPosition += new Vector3(0,2,0);
        }
        public void OnDisable()
        {
            realTimer.localPosition -= new Vector3(0, 2, 0);
        }

       

        public void Setup()
        {
            this.runStopwatchTimerTextController.format = format;
            HGTextMeshProUGUI hgtextMeshProUGUI = this.GetComponent<HGTextMeshProUGUI>();
            hgtextMeshProUGUI.color = hgtextMeshProUGUI.color.AlphaMultiplied(0.25f);
            hgtextMeshProUGUI.alignment = TMPro.TextAlignmentOptions.Right;
            this.transform.localPosition = new Vector3(-32.5f, -10f, 0f);
            this.transform.localScale = Vector3.one * 0.5f;

            realTimer = this.transform.parent.GetChild(1);
        }

        private void Update()
        {
            if (this.runStopwatchTimerTextController)
            {
                this.runStopwatchTimerTextController.seconds = (double)(Run.instance ? Run.instance.fixedTime-runStartingOffset : 0f);
            }
        }
        public TimerText runStopwatchTimerTextController;
    }
}
