using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace WolfoQoL_Client
{
    public static class UI_Color
    {


        public static void Start()
        {

            //Fix fukin Lunar Coin background being purple
            //Color lunarCoin = new Color(0.52 0.584 0.66 0.1333f);
            //Color lunarCoin = new Color(0.561f, 0.639f, 0.796f, 0.1333f);
            Color lunarCoin = new Color32(145, 162, 196, 34);
           
            GameObject hud = LegacyResourcesAPI.Load<GameObject>("Prefabs/HUDSimple");
            hud.GetComponent<HUD>().lunarCoinContainer.transform.GetChild(0).GetComponent<RawImage>().color = lunarCoin;//0.6288 0.4514 0.6509 0.1333

            GameObject AchievementNotificationPanel = Addressables.LoadAssetAsync<GameObject>(key: "a27b8dde5bc73be4fae7f40a46e3cdff").WaitForCompletion(); //0.6784 0.7412 0.9804 1
            AchievementNotificationPanel.GetComponent<AchievementNotificationPanel>().rewardArea.transform.GetChild(0).GetComponent<RawImage>().color = lunarCoin;//0.6288 0.4514 0.6509 0.1333

            /*if (WConfig.cfgTempItemCyanStack.Value)
            {
                //On.RoR2.UI.ItemIcon.SetItemIndex_ItemIndex_int_float += ShowTemporaryItemStacksDifferently;
            }*/

        }


        public static Color Temp = new Color(0.75f, 0.95f, 1f, 1f);
        private static void ShowTemporaryItemStacksDifferently(On.RoR2.UI.ItemIcon.orig_SetItemIndex_ItemIndex_int_float orig, ItemIcon self, ItemIndex newItemIndex, int newItemCount, float newDurationPercent)
        {
            orig(self, newItemIndex, newItemCount, newDurationPercent);
            if (newDurationPercent > 0)
            {
                if (newItemCount == 1)
                {
                    self.spriteAsNumberManager.SetItemCount(1);
                }
                self.spriteAsNumberManager.SetSpriteColor(Temp);
            }
            else
            {
                self.spriteAsNumberManager.SetSpriteColor(Color.white);
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



    }

}
