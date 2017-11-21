using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FestivalManager : MonoBehaviour {

    private GameObject uibox;
    private GameObject ItemList;
    private GameObject panel1;
    private GameObject panel2;

    private GameObject itemInfo;

    private GameObject festivalItemPopup;

    private GameObject SystemPopup;
    private Text Sys_TitleText;
    private Text Sys_InfoText;
    private Button Sys_YesButton;
    private Button Sys_NoButton;
    private Button Sys_OkButton;


    private List<ForSale> saleList;
    private List<GameObject> saleObj1;


    private int curSelectNum;           //물품 등록 위치
    private InventoryThings curInven;

	void Start ()
    {
        saleList = FestivalData.instance.getSaleList();

        uibox = GameObject.Find("System").transform.Find("Festival/UIBox").gameObject;
        ItemList = uibox.transform.Find("ItemList").gameObject;
        panel1 = ItemList.transform.Find("Panel1").gameObject;
        panel2 = ItemList.transform.Find("Panel2").gameObject;

        itemInfo = GameObject.Find("System").transform.Find("Festival/ItemInfo").gameObject;

        festivalItemPopup = GameObject.Find("System").transform.Find("FestivalItem").gameObject;

        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        Sys_TitleText = SystemPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>();
        Sys_InfoText = SystemPopup.transform.Find("UIPanel/InfoText").gameObject.GetComponent<Text>();
        Sys_YesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        Sys_NoButton = SystemPopup.transform.Find("UIPanel/NoButton").gameObject.GetComponent<Button>();
        Sys_OkButton = SystemPopup.transform.Find("UIPanel/OKButton").gameObject.GetComponent<Button>();


        saleObj1 = new List<GameObject>();

        for (int i = 0; i < saleList.Count; i++)
            saleObj1.Add(panel1.transform.Find("ItemBox (" + i + ")").gameObject);

        //상태 갱신
        updateState();

        
        StartCoroutine(sliderUpdate());


    }


    IEnumerator sliderUpdate()
    {
        while (true)
        {
            if (festivalItemPopup.transform.Find("UIBox/Inbox/Setting").gameObject.activeInHierarchy)
            {
                int amount = (int)festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/Slider").gameObject.GetComponent<Slider>().value;
                festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/AmountText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", amount);

                int price = (int)festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/Slider").gameObject.GetComponent<Slider>().value;

                if (price < 1) price = 1;
                festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/PriceText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", price);

                festivalItemPopup.transform.Find("UIBox/Inbox/Setting/Total/TotalText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", amount * price);
            }
            for (int i = 0; i < saleList.Count; i++)
            {
                if (saleList[i].state == "sellout")
                {
                    if (saleList[i].alrFlag)
                    {
                        saleList[i].alrFlag = false;
                        GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(saleList[i].saleThings.name + " x" + saleList[i].possession + " 판매 완료");
                        SystemPopup.SetActive(false);
                        itemInfo.SetActive(false);
                        saleObj1[i].transform.Find("Button/SelloutImage").gameObject.SetActive(true);

                        //button 판매금액 회수
                        int num = i;
                        saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                        saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            saleObj1[num].transform.Find("Button/SelloutImage").gameObject.SetActive(false);

                            Player.instance.GetMoney("gold", saleList[num].possession * saleList[num].unitPrice);

                            saleList[num].state = "empty";
                            updateState();
                            
                        });
                    }
                }
            }
                yield return null;
        }
    }




    //아이템 상태 갱신
	public void updateState()
    {
        for (int i = 0; i < saleList.Count; i++)
        {
            //아이템 등록x
            if (saleList[i].state == "empty")
            {
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Image>().color = new Color(0.8f, 0.68f, 0.47f);
                saleObj1[i].transform.Find("Button/Icon").gameObject.SetActive(false);
                saleObj1[i].transform.Find("Button/NameText").gameObject.SetActive(false);
                saleObj1[i].transform.Find("Button/AddText").gameObject.SetActive(true);


                //button 아이템 등록
                int num = i;
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    curSelectNum = num;
                    curInven = null;
                    //초기화
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = new Color(1, 1, 1);
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/Icon").gameObject.SetActive(false);
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/NameText").gameObject.GetComponent<Text>().text = null;
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/AmountText").gameObject.GetComponent<Text>().text = null;
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/SellText").gameObject.GetComponent<Text>().text = null;

                    festivalItemPopup.transform.Find("UIBox/Inbox/Setting").gameObject.SetActive(false);
                    festivalItemPopup.transform.Find("UIBox/Inbox/SelectText").gameObject.SetActive(true);
                    festivalItemPopup.SetActive(true);
                    //인벤토리 불러오기. blackback 없애기.
                    GameObject.Find("Menu").transform.Find("InventoryPopup").gameObject.SetActive(true);
                    GameObject.Find("InventoryScript").GetComponent<Inventory>().ItemSlotCreate();
                    GameObject.Find("Menu").transform.Find("InventoryPopup/BlackBack").gameObject.SetActive(false);
                });
            }
            //아이템 등록
            else if (saleList[i].state == "sale")
            {
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == saleList[i].saleThings.name).grade);
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Image>().color = col;
                saleObj1[i].transform.Find("Button/Icon").gameObject.GetComponent<Image>().sprite
                    = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == saleList[i].saleThings.name).icon);
                saleObj1[i].transform.Find("Button/Icon").gameObject.SetActive(true);
                saleObj1[i].transform.Find("Button/NameText").gameObject.GetComponent<Text>().text = saleList[i].saleThings.name + " x" + saleList[i].possession;
                saleObj1[i].transform.Find("Button/NameText").gameObject.SetActive(true);
                saleObj1[i].transform.Find("Button/AddText").gameObject.SetActive(false);

                //정보 팝업 button / 아이템 판매 취소. 회수. systempopup
                int num = i;
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    itemInfo.transform.Find("CancleButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();

                    itemInfo.SetActive(true);
                    itemInfo.transform.Find("InfoBox/ItemNameText").gameObject.GetComponent<Text>().text = saleList[num].saleThings.name;
                    itemInfo.transform.Find("InfoBox/AmountText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", saleList[num].possession);
                    itemInfo.transform.Find("InfoBox/PriceText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", saleList[num].unitPrice);
                    itemInfo.transform.Find("InfoBox/TotalText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", saleList[num].possession * saleList[num].unitPrice);

                    int poss = 0;
                    //장비일 때.
                    if (saleList[num].saleThings.type == "Weapon" || saleList[num].saleThings.type == "Helmet" || saleList[num].saleThings.type == "Armor"
                        || saleList[num].saleThings.type == "Gloves" || saleList[num].saleThings.type == "Pants" || saleList[num].saleThings.type == "Boots")
                    {
                        
                        poss = ThingsData.instance.getInventoryThingsList().FindAll(x => x.name == saleList[num].saleThings.name).Count;
                        if (poss <= 0) itemInfo.transform.Find("InfoBox/PossessionText").gameObject.GetComponent<Text>().text = 0.ToString();
                        else itemInfo.transform.Find("InfoBox/PossessionText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", poss);
                    }
                    else
                    {
                        if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == saleList[num].saleThings.name) == null)
                            itemInfo.transform.Find("InfoBox/PossessionText").gameObject.GetComponent<Text>().text = 0.ToString();
                        else
                        {
                            poss = ThingsData.instance.getInventoryThingsList().Find(x => x.name == saleList[num].saleThings.name).possession;
                            itemInfo.transform.Find("InfoBox/PossessionText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", poss);
                        }
                    }
                    

                    col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == saleList[num].saleThings.name).grade);
                    itemInfo.transform.Find("ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = col;
                    itemInfo.transform.Find("ItemBox/Icon").gameObject.GetComponent<Image>().sprite
                               = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == saleList[num].saleThings.name).icon);

                    itemInfo.transform.Find("CancleButton").gameObject.SetActive(true);
                    itemInfo.transform.Find("BuyButton").gameObject.SetActive(false);

                    //회수하기 버튼. system
                    itemInfo.transform.Find("CancleButton").gameObject.GetComponent<Button>().onClick.AddListener(
                        () => {
                            SystemPopup.SetActive(true);
                            Sys_TitleText.GetComponent<Text>().text = "아이템 회수";
                            Sys_InfoText.GetComponent<Text>().text = "판매를 중지하고 아이템을 회수하겠습니까?";
                            Sys_YesButton.gameObject.SetActive(true);    
                            Sys_NoButton.gameObject.SetActive(true);
                            Sys_OkButton.gameObject.SetActive(false);
                            Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
                            Sys_YesButton.GetComponent<Button>().onClick.AddListener(
                               () => {

                                   string type = ThingsData.instance.getThingsList().Find(x => x.name == saleList[num].saleThings.name).type;
                                   //장비 구분
                                   if (type == "Helmet" || type == "Armor" || type == "Gloves" || type == "Pants" || type == "Weapon" || type == "Boots")
                                   {
                                           ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x 
                                               => x.name == saleList[num].saleThings.name).type, saleList[num].saleThings.name, 1));
                                   }
                                   //장비 외 아이템
                                   else
                                   {
                                       if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == saleList[num].saleThings.name) != null)
                                       {
                                           ThingsData.instance.getInventoryThingsList().Find(x => x.name == saleList[num].saleThings.name).possession += saleList[num].possession;
                                           ThingsData.instance.getInventoryThingsList().Find(x => x.name == saleList[num].saleThings.name).recent = true;
                                       }
                                       else
                                       {
                                           ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x 
                                               => x.name == saleList[num].saleThings.name).type, saleList[num].saleThings.name, saleList[num].possession));
                                       }
                                   }

                                   //
                                   saleList[num].state = "empty";
                                   saleList[num].alrFlag = false;
                                   updateState();
                                   itemInfo.SetActive(false);
                               });
                        });

                });
            }
            //아이템 팔림
            else if(saleList[i].state == "sellout")
            {
                saleObj1[i].transform.Find("Button/SelloutImage").gameObject.SetActive(true);

                //button 판매금액 회수
                int num = i;
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    saleObj1[num].transform.Find("Button/SelloutImage").gameObject.SetActive(false);

                    Player.instance.GetMoney("gold", saleList[num].possession * saleList[num].unitPrice);

                    saleList[num].state = "empty";
                    updateState();
                });
            }


        }

    }



    //아이템 등록 창에서 인벤토리 아이템 선택
    public void selectItem(InventoryThings inven)
    {
        
        if (inven.equip)
        {
            GameObject.Find("System").transform.Find("AlertImage").gameObject.SetActive(false);
            GameObject.Find("System").transform.Find("AlertImage/AlrImage/Text").gameObject.GetComponent<Text>().text = "장착한 장비는 판매할 수 없습니다.";
            StartCoroutine(alrImageActive());
            return;
        }

        curInven = inven;

        festivalItemPopup.transform.Find("UIBox/Inbox/Setting").gameObject.SetActive(true);
        festivalItemPopup.transform.Find("UIBox/Inbox/SelectText").gameObject.SetActive(false);

        Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == inven.name).grade);
        festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = col;
        festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/Icon").gameObject.SetActive(true);
        festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/Icon").gameObject.GetComponent<Image>().sprite
                   = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == inven.name).icon);
        festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/NameText").gameObject.GetComponent<Text>().text = inven.name;
        festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/AmountText").gameObject.GetComponent<Text>().text = "보유 개수 : " + inven.possession;
        festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/SellText").gameObject.GetComponent<Text>().text = "기본 가격 : " 
            + ThingsData.instance.getThingsList().Find(x=>x.name== inven.name).sell;

        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/Slider").gameObject.GetComponent<Slider>().minValue = 1;
        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/Slider").gameObject.GetComponent<Slider>().maxValue = inven.possession;
        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/Slider").gameObject.GetComponent<Slider>().value = 1;

        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/Slider").gameObject.GetComponent<Slider>().minValue = 0.1f
            * ThingsData.instance.getThingsList().Find(x => x.name == inven.name).sell;
        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/Slider").gameObject.GetComponent<Slider>().maxValue = 10f
            * ThingsData.instance.getThingsList().Find(x => x.name == inven.name).sell;
        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/Slider").gameObject.GetComponent<Slider>().value 
            = ThingsData.instance.getThingsList().Find(x => x.name == inven.name).sell;




    }

    //문구 출력 애니메이션
    IEnumerator alrImageActive()
    {
        GameObject.Find("System").transform.Find("AlertImage").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
    }


    //아이템 등록 버튼
    public void SellButton()
    {
        if (curInven == null) return;

        int amount = (int)festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/Slider").gameObject.GetComponent<Slider>().value;
        int price = (int)festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/Slider").gameObject.GetComponent<Slider>().value;

        //인벤토리 수량 감소.
        curInven.possession -= amount;

        //가판대에 등록한 아이템 표시
        saleList[curSelectNum].saleThings = curInven;
        saleList[curSelectNum].possession = amount;
        saleList[curSelectNum].unitPrice = price;
        saleList[curSelectNum].state = "sale";
        saleList[curSelectNum].alrFlag = true;
        updateState();

        curInven = null;
        //창 끄기
        festivalItemPopup.SetActive(false);
        GameObject.Find("Menu").transform.Find("InventoryPopup").gameObject.SetActive(false);
    }




}
