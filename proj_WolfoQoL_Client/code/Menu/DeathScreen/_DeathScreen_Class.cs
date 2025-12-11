using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace WolfoQoL_Client.DeathScreen
{

    public class InventoryDisplayFitter : UIBehaviour
    {
        public ItemInventoryDisplay inv;
        public float previousHeight = 0;
        public RectTransform rectTransform;
        public override void OnEnable()
        {
            base.OnEnable();
            rectTransform = GetComponent<RectTransform>();
            inv = GetComponentInChildren<ItemInventoryDisplay>();
        }
        public override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            if (this.rectTransform)
            {
                float width = this.rectTransform.rect.height;
                if (width != this.previousHeight)
                {
                    this.previousHeight = width;
                    inv.maxHeight = rectTransform.sizeDelta.y;
                    inv.LayoutAllIcons();
                    inv.transform.parent.GetComponent<Image>().OnEnable();
                }
            }
        }

    }
    public class DeathScreenExpanded : MonoBehaviour
    {
        public GameObject hideInventory;

        public GameObject lastSimuWaveStat;
        public GameObject stageRecap;
        public GameObject stageRecapArea;
        public Transform stageRecapContent;
        public ChildLocator childLocator;
        public GameObject lastWaveStat;

        public bool isSaveAndContinuedRun;
        public bool isDevotionRun;

        public bool compactedStats;
        public bool addedRunRecap;
        public bool addedRunTrackedStats;
        public bool isLogRunReport;

        public float deathTimeStamp;

        public GameEndReportPanelController GameEndReportPanel;
        public Transform ItemArea;

        //Instance
        public GameObject latestWaveStrip;

        public GameObject bonusInventoyHolder;
        public GameObject killerInventory;
        public GameObject minionInventory;
        public bool IsEvoInventory;


        //References
        public static GameObject difficulty_stat;

        public static GameObject itemAreaPrefab;
        public static GameObject unlockAreaPrefab;
        public static GameObject RightAreaPrefab;
        public static GameObject statHolderPrefab;

        public bool oneTimeExtras = false;
        public bool madeStats = false;

        public bool addedStageRecap;
 
        public static GameObject stageIconPrefab;
      

        public GameObject chatToggleButton;
        public bool chatActive;
  
        public GameObject MakeInventory()
        {
            if (!bonusInventoyHolder)
            {
                GameObject inventoryHolder = new GameObject("inventoryHolder");
                this.bonusInventoyHolder = inventoryHolder;

                LayoutElement lay = inventoryHolder.AddComponent<LayoutElement>();
                HorizontalLayoutGroup hor = inventoryHolder.AddComponent<HorizontalLayoutGroup>();
                lay.flexibleHeight = 0.1f;
                lay.minHeight = 124;
                lay.preferredHeight = 210;
                hor.spacing = 2;

                inventoryHolder.transform.SetParent(ItemArea.parent, false);
                inventoryHolder.transform.SetSiblingIndex(3);

            }


            GameObject newInventory = UnityEngine.Object.Instantiate(DeathScreenExpanded.itemAreaPrefab, bonusInventoyHolder.transform);
            LayoutElement layout = newInventory.GetComponent<LayoutElement>();
            ItemInventoryDisplay inv = newInventory.GetComponentInChildren<ItemInventoryDisplay>();
            newInventory.transform.GetChild(1).gameObject.AddComponent<InventoryDisplayFitter>();
            inv.transform.parent.GetComponent<Image>().enabled = false; //Disable image clipping

            layout.flexibleHeight = -1;
            layout.preferredHeight = -1;
            //inv.maxHeight = 116f; 
            //inv.UpdateDisplay();
            return newInventory;
        }


        public void ToggleChat()
        {
            Util.PlaySound("Play_UI_menuClick", this.gameObject);
            chatToggleButton.transform.localScale = new Vector3(0.5f, -chatToggleButton.transform.localScale.y, 0.5f);
            chatActive = !chatActive;
            GameEndReportPanel.chatboxTransform.gameObject.SetActive(chatActive);
        }
  
    }

}
