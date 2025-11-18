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
    public class UI_Stuff
    {

        public static GameObject DifficultyStat;
        public static GameObject UnlockContainer;

        public static void Start()
        {




            On.RoR2.UI.ChatBox.Awake += AllowMoreChatMessages;

            On.RoR2.UI.CrosshairManager.OnEnable += CrosshairManager_OnEnable;

            On.RoR2.UI.PingIndicator.RebuildPing += (orig, self) =>
            {
                orig(self);
                self.fixedTimer *= 2;
                //self.fixedTimer *= WConfig.cfgPingDurationMultiplier.Value;
            };

            GameObject ChatBoxLobby = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/ChatBox.prefab").WaitForCompletion();
            GameObject ChatBoxRun = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/ChatBox, In Run.prefab").WaitForCompletion();
            GameObject GameEndReportPanel = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/GameEndReportPanel.prefab").WaitForCompletion();
            GameObject InfiniteTowerGameEndReportPanel = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/DLC1/GameModes/InfiniteTowerRun/InfiniteTowerGameEndReportPanel.prefab").WaitForCompletion();

            AllowScrolling(ChatBoxRun);
            AllowScrolling(ChatBoxLobby);
            DifficultyStat = GameEndReportPanel.GetComponent<GameEndReportPanelController>().statContentArea.GetChild(0).gameObject;
            UnlockContainer = GameEndReportPanel.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;



            IL.RoR2.UI.ScoreboardController.Rebuild += ScoreboardForDeadPeopleToo;

            //On.RoR2.SubjectChatMessage.GetSubjectName += IncludeEliteTypeInSubjectName;

        }


        private static string IncludeEliteTypeInSubjectName(On.RoR2.SubjectChatMessage.orig_GetSubjectName orig, SubjectChatMessage self)
        {
            if (self.subjectAsNetworkUser == null && self.subjectAsCharacterBody)
            {
                return RoR2.Util.GetBestBodyName(self.subjectAsCharacterBody.gameObject);
            }
            return orig(self);
        }

        private static void ScoreboardForDeadPeopleToo(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(MoveType.Before,
                x => x.MatchStloc(0)))
            {
                c.EmitDelegate<System.Func<List<PlayerCharacterMasterController>, List<PlayerCharacterMasterController>>>((self) =>
                {
                    List<PlayerCharacterMasterController> list = (from x in PlayerCharacterMasterController.instances
                                                                  where x.gameObject.activeInHierarchy && x.isConnected
                                                                  select x).ToList<PlayerCharacterMasterController>();
                    return list;
                });
            }
            else
            {
                WQoLMain.log.LogWarning("IL Failed: ScoreboardForDeadPeopleToo");
            }

        }



        public static void AllowScrolling(GameObject chatBox)
        {
            ChatBox chat = chatBox.GetComponent<ChatBox>();
            chat.scrollRect.vertical = true;
            chat.scrollRect.scrollSensitivity = 6;
            //There's just a weirdo second Input Field, that's there but not
            //And it being pseudo invisible breaks scrolling?
            Transform MessageArea = chatBox.transform.GetChild(2).GetChild(0).GetChild(3).GetChild(0);
            TMPro.TMP_InputField input = MessageArea.GetComponent<TMPro.TMP_InputField>();
            input.image.enabled = true;
            input.image.color = new Color(0, 0, 0, 0);
        }


        private static void AllowMoreChatMessages(On.RoR2.UI.ChatBox.orig_Awake orig, RoR2.UI.ChatBox self)
        {
            orig(self);
            if (self.name.EndsWith("In Run(Clone)"))
            {
                bool HasDPSMeter = self.transform.parent.parent.parent.parent.Find("DPSMeterPanel");
                self.allowExpandedChatbox = true;

                RectTransform tempRectTransformInput = self.inputField.GetComponent<RectTransform>();
                tempRectTransformInput.offsetMax = new Vector2(48, 48);
                tempRectTransformInput.offsetMin = new Vector2(-8, 0);

                RectTransform tempRectTransformExpanded = self.expandedChatboxRect;
                tempRectTransformExpanded.anchorMax = new Vector2(1, 1);
                tempRectTransformExpanded.offsetMax = new Vector2(120, 0);
                tempRectTransformExpanded.offsetMin = new Vector2(-10, 49);

                RectTransform tempRectTransform = self.standardChatboxRect;
                if (HasDPSMeter)
                {
                    tempRectTransform.offsetMax = new Vector2(120, 0);
                    tempRectTransform.offsetMin = new Vector2(-10, 49);
                }
                else
                {
                    tempRectTransform.offsetMax = new Vector2(120, -6);
                    tempRectTransform.offsetMin = new Vector2(-10, 0);
                }
            }
            else if (self.name.EndsWith("ChatBox")) //GameEndPannel
            {
                self.expandedChatboxRect.offsetMax = new Vector2(0, 196);
            }
        }


        private static void CrosshairManager_OnEnable(On.RoR2.UI.CrosshairManager.orig_OnEnable orig, RoR2.UI.CrosshairManager self)
        {
            //WolfoMain.log.LogMessage(" On.RoR2.UI.CrosshairManager.OnEnable " + self);
            orig(self);
            if (WConfig.cfgNewSprintCrosshair.Value == false)
            {
                return;
            }
            GameObject SprintingCrosshair = Addressables.LoadAssetAsync<GameObject>(key: "RoR2/Base/UI/SprintingCrosshair.prefab").WaitForCompletion();
            if (SprintingCrosshair.transform.childCount > 0)
            {
                return;
            }
            SprintingCrosshair.transform.localScale = Vector3.one * 1.1f;
            CrosshairController SprintingCrosshairUI = SprintingCrosshair.GetComponent<RoR2.UI.CrosshairController>();
            RawImage rawMid = SprintingCrosshair.GetComponent<RawImage>();
            SprintingCrosshairUI.maxSpreadAngle = 8;

            rawMid.texture = Addressables.LoadAssetAsync<Texture2D>(key: "RoR2/Base/UI/texCrosshairDot.png").WaitForCompletion();
            rawMid.color = new Color(1f, 1f, 1f, 0.9f);

            Texture2D Arrow = Addressables.LoadAssetAsync<Texture2D>(key: "RoR2/Base/UI/texCrosshairArrow.png").WaitForCompletion();
            GameObject Right = new GameObject("Right");
            RawImage raw = Right.AddComponent<RawImage>();
            raw.texture = Arrow;
            raw.color = rawMid.color;

            GameObject Left = new GameObject("Left");
            raw = Left.AddComponent<RawImage>();
            raw.texture = Arrow;
            raw.color = rawMid.color;


            Right.transform.SetParent(SprintingCrosshair.transform);
            Left.transform.SetParent(SprintingCrosshair.transform);
            Right.transform.SetLocalEulerAngles(new Vector3(0, 0, -90), RotationOrder.OrderXYZ);
            Left.transform.SetLocalEulerAngles(new Vector3(0, 0, 90), RotationOrder.OrderXYZ);
            Right.transform.localScale = Vector3.one;
            Left.transform.localScale = Vector3.one;

            SprintingCrosshairUI.spriteSpreadPositions = new RoR2.UI.CrosshairController.SpritePosition[]
            {
                new CrosshairController.SpritePosition
                {
                    onePosition = new Vector3(96,0,0),
                    target = Left.GetComponent<RectTransform>(),
                    zeroPosition = new Vector3(24,0,0)
                },
                new CrosshairController.SpritePosition
                {
                    onePosition = new Vector3(-96,0,0),
                    target = Right.GetComponent<RectTransform>(),
                    zeroPosition = new Vector3(-24,0,0)
                },
            };

        }



    }

}
