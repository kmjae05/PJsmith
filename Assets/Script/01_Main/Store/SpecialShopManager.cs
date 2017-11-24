using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SpecialShopManager : MonoBehaviour {

    private GameObject specialshopPopup;

    private GameObject uibox;
    private GameObject itemList;
    private GameObject panel1;
    private List<Things> item1;
    private int[] itemPrice;
    private List<GameObject> slots1;

    private GameObject itembox;

    private Text goldText;
    private Text cashText;


    private GameObject SystemPopup;
    private Text Sys_TitleText;
    private Text Sys_InfoText;
    private Button Sys_YesButton;
    private Button Sys_NoButton;
    private Button Sys_OkButton;




    void Start ()
    {
        specialshopPopup = GameObject.Find("Menu").transform.Find("SpecialshopPopup").gameObject;
        uibox = specialshopPopup.transform.Find("UIBox").gameObject;
        itemList = uibox.transform.Find("ItemList").gameObject;
        panel1 = itemList.transform.Find("Panel1").gameObject;
        itembox = panel1.transform.Find("ShopBox").gameObject;
        item1 = new List<Things>();
        itemPrice = new int[4];
        slots1 = new List<GameObject>();

        goldText = uibox.transform.Find("Back/StoreBox/MoneyPanel/Panel/Gold/GoldText").gameObject.GetComponent<Text>();
        cashText = uibox.transform.Find("Back/StoreBox/MoneyPanel/Panel/Cash/CashText").gameObject.GetComponent<Text>();

        goldText.text = Player.instance.getUser().gold.ToString();
        cashText.text = Player.instance.getUser().cash.ToString();


        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        Sys_TitleText = SystemPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>();
        Sys_InfoText = SystemPopup.transform.Find("UIPanel/InfoText").gameObject.GetComponent<Text>();
        Sys_YesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        Sys_NoButton = SystemPopup.transform.Find("UIPanel/NoButton").gameObject.GetComponent<Button>();
        Sys_OkButton = SystemPopup.transform.Find("UIPanel/OKButton").gameObject.GetComponent<Button>();

        GameObject.Find("BrazierButton").transform.Find("BrazierMiniButton/SpecialshopButton").gameObject.GetComponent<Button>().onClick.AddListener(() => 
        {
            goldText.text = Player.instance.getUser().gold.ToString();
            cashText.text = Player.instance.getUser().cash.ToString();
        });

        //아이템 생성
        List<Things> things = ThingsData.instance.getThingsList().FindAll(x => x.grade > 3 );
        for (int i = 0; i < 4; i++)
        {
            int random = UnityEngine.Random.Range(0, things.Count);
            item1.Add(things[random]);

            slots1.Add(Instantiate(itembox));
            slots1[i].transform.SetParent(panel1.transform);
            slots1[i].GetComponent<RectTransform>().localScale = Vector3.one;
            slots1[i].GetComponent<RectTransform>().localPosition = Vector3.one;
            slots1[i].SetActive(true);
            Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == item1[i].name).grade);
            slots1[i].transform.Find("ItemBox/Item").gameObject.GetComponent<Image>().color = col;
            slots1[i].transform.Find("ItemBox/Item/Icon").gameObject.GetComponent<Image>().sprite
                = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == item1[i].name).icon);
            slots1[i].transform.Find("ItemNameText").gameObject.GetComponent<Text>().text = item1[i].name;
            //가격
            int price = 0;
            if (item1[i].type == "Weapon" || item1[i].type == "Helmet"|| item1[i].type == "Armor"
                || item1[i].type == "Gloves" || item1[i].type == "Pants" || item1[i].type == "Boots")
                price = item1[i].sell * 10;
            else price = 1000;
            itemPrice[i] = price;
            slots1[i].transform.Find("BuyButton/PriceText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", price);

            int num = i;
            slots1[i].transform.Find("BuyButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            slots1[i].transform.Find("BuyButton").gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                SystemPopup.SetActive(true);
                Sys_YesButton.gameObject.SetActive(true);
                Sys_NoButton.gameObject.SetActive(true);
                Sys_OkButton.gameObject.SetActive(false);
                Sys_TitleText.GetComponent<Text>().text = "아이템 구매";
                Sys_InfoText.GetComponent<Text>().text = "["+ item1[num].name + "] 구매하시겠습니까?";
                Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();
                Sys_YesButton.GetComponent<Button>().onClick.AddListener(()=>
                {
                    if (Player.instance.getUser().gold < itemPrice[num])
                    {
                        SystemPopup.SetActive(true);
                        Sys_YesButton.gameObject.SetActive(false);
                        Sys_NoButton.gameObject.SetActive(false);
                        Sys_OkButton.gameObject.SetActive(true);
                        Sys_TitleText.GetComponent<Text>().text = "골드 부족";
                        Sys_InfoText.GetComponent<Text>().text = "골드가 부족합니다.";
                        return;
                    }

                    Player.instance.LostMoney("gold", itemPrice[num]);

                    goldText.text = Player.instance.getUser().gold.ToString();
                    cashText.text = Player.instance.getUser().cash.ToString();

                    slots1[num].transform.Find("SelloutImage").gameObject.SetActive(true);
                    item1[num].possession = 1;
                    ThingsData.instance.getItem(item1[num]);
                });


            });
        }




    }





	
}
