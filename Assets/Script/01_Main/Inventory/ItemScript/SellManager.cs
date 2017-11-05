   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour {

    private GameObject sellPopup;
    private GameObject UIPanel;
    private GameObject itemBox;
    private Text priceText;
    private GameObject selectBox;
    private Text selectAmountText;
    private Button minButton;
    private Button pluButton;
    private Slider slider;
    private Text totalText;
    private Button allButton;
    private Button okButton;

    private GameObject SystemPopup;
    private Text Sys_TitleText;
    private Text Sys_InfoText;
    private Button Sys_YesButton;
    private Button Sys_NoButton;
    private Button Sys_OkButton;


    private InventoryThings curItem;

    private GameObject oreInfoPopup;
    private GameObject itemInfoPopup;
    private GameObject equipItemInfoPopup;
    private GameObject inventoryPopup;

    void Start ()
    {
        sellPopup = GameObject.Find("System").transform.Find("SellPopup").gameObject;
        UIPanel = sellPopup.transform.Find("UIPanel").gameObject;
        itemBox = UIPanel.transform.Find("ItemBox").gameObject;
        priceText = UIPanel.transform.Find("priceText").gameObject.GetComponent<Text>();
        selectBox = UIPanel.transform.Find("SelectBox").gameObject;
        selectAmountText = selectBox.transform.Find("SelectAmountText").gameObject.GetComponent<Text>();
        minButton = selectBox.transform.Find("MinButton").gameObject.GetComponent<Button>();
        pluButton = selectBox.transform.Find("PluButton").gameObject.GetComponent<Button>();
        slider = selectBox.transform.Find("Slider").gameObject.GetComponent<Slider>();
        totalText = selectBox.transform.Find("TotalText").gameObject.GetComponent<Text>();
        allButton = UIPanel.transform.Find("AllButton").gameObject.GetComponent<Button>();
        okButton = UIPanel.transform.Find("OKButton").gameObject.GetComponent<Button>();

        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        Sys_TitleText = SystemPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>();
        Sys_InfoText = SystemPopup.transform.Find("UIPanel/InfoText").gameObject.GetComponent<Text>();
        Sys_YesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        Sys_NoButton = SystemPopup.transform.Find("UIPanel/NoButton").gameObject.GetComponent<Button>();
        Sys_OkButton = SystemPopup.transform.Find("UIPanel/OKButton").gameObject.GetComponent<Button>();

        oreInfoPopup = GameObject.Find("System").transform.Find("OreInfoPopup").gameObject;
        itemInfoPopup = GameObject.Find("System").transform.Find("ItemInfoPopup").gameObject;
        equipItemInfoPopup = GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject;
        inventoryPopup = GameObject.Find("Menu").transform.Find("InventoryPopup").gameObject;
    }

    private void Update()
    {
        if (sellPopup.activeInHierarchy)
        {
            selectAmountText.text = ((int)slider.value).ToString() + " / " + curItem.possession;
            totalText.text = "합계 : " + ((int)slider.value * ThingsData.instance.getThingsList().Find(x=>x.name== curItem.name).sell);
        }
    }


    public void OpenSellPopup(InventoryThings inventoryThings)
    {
        curItem = inventoryThings;
        Things things = ThingsData.instance.getThingsList().Find(x => x.name == inventoryThings.name);

        Color col = ThingsData.instance.ChangeFrameColor(things.grade);
        itemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;
        itemBox.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(things.icon);
        itemBox.transform.Find("AmountText").gameObject.GetComponent<Text>().text = inventoryThings.possession.ToString();
        itemBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = inventoryThings.name;
        priceText.text = "개당 가격 : " + things.sell.ToString();
        selectAmountText.text = "1 / " + inventoryThings.possession.ToString();

        slider.minValue = 0;
        slider.maxValue = inventoryThings.possession;
        slider.value = 0;
        totalText.text = "합계 : " + ((int)slider.value * things.sell);

        minButton.onClick.RemoveAllListeners();
        minButton.onClick.AddListener(() => { slider.value--; });
        pluButton.onClick.RemoveAllListeners();
        pluButton.onClick.AddListener(() => { slider.value++; });
        allButton.onClick.RemoveAllListeners();
        allButton.onClick.AddListener(() => { slider.value = slider.maxValue; });

        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() 
            => {
                inventoryThings.possession -= (int)slider.value;    //소지 개수 감소
                //돈 획득
                Player.instance.GetMoney("gold", ((int)slider.value * ThingsData.instance.getThingsList().Find(x => x.name == curItem.name).sell));

                //인벤토리 팝업이 켜져 있을 때 아이템 갱신
                if ( inventoryPopup.activeInHierarchy)
                {
                    GameObject.Find("InventoryScript").GetComponent<Inventory>().ItemSlotCreate();
                }

                sellPopup.SetActive(false);
                if (oreInfoPopup.activeInHierarchy) oreInfoPopup.SetActive(false);
                if (itemInfoPopup.activeInHierarchy) itemInfoPopup.SetActive(false);
            });

        sellPopup.SetActive(true);

    }

    //장비 판매 팝업
    public void OpenEquipSellPopup(InventoryThings inventoryThings)
    {
        curItem = inventoryThings;
        Things things = ThingsData.instance.getThingsList().Find(x => x.name == inventoryThings.name);

        Sys_TitleText.GetComponent<Text>().text = inventoryThings.name + " 판매";
        Sys_InfoText.GetComponent<Text>().text = things.sell.ToString() + "에 판매하시겠습니까 ?";

        Sys_YesButton.gameObject.SetActive(true);     //예/아니오 버튼으로 수정
        Sys_NoButton.gameObject.SetActive(true);
        Sys_OkButton.gameObject.SetActive(false);

        Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
        Sys_YesButton.GetComponent<Button>().onClick.AddListener(() 
            => {
                inventoryThings.possession = 0;    //소지 개수 감소
                //돈 획득
                Player.instance.GetMoney("gold", things.sell);

                //인벤토리 팝업이 켜져 있을 때 아이템 갱신
                if (inventoryPopup.activeInHierarchy)
                {
                    GameObject.Find("InventoryScript").GetComponent<Inventory>().ItemSlotCreate();
                }
                
                sellPopup.SetActive(false);
                if (equipItemInfoPopup.activeInHierarchy) equipItemInfoPopup.SetActive(false);

                SystemPopup.SetActive(false);
                
                });    //광석 선택창 끄기

        SystemPopup.SetActive(true);

    }




}
