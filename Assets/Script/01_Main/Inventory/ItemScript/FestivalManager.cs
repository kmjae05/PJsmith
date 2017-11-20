using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FestivalManager : MonoBehaviour {

    private GameObject uibox;
    private GameObject ItemList;
    private GameObject panel1;
    private GameObject panel2;

    private GameObject festivalItemPopup;

    private List<ForSale> saleList;
    private List<GameObject> saleObj1;


    private int curSelectNum;

	void Start ()
    {
        saleList = FestivalData.instance.getSaleList();

        uibox = GameObject.Find("System").transform.Find("Festival/UIBox").gameObject;
        ItemList = uibox.transform.Find("ItemList").gameObject;
        panel1 = ItemList.transform.Find("Panel1").gameObject;
        panel2 = ItemList.transform.Find("Panel2").gameObject;

        festivalItemPopup = GameObject.Find("System").transform.Find("FestivalItem").gameObject;

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

                festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/AmountText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", price);





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
            if(saleList[i].state == "empty")
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
                    //초기화
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = new Color(1,1,1);
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/Icon").gameObject.SetActive(false);
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/NameText").gameObject.GetComponent<Text>().text = null;
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/AmountText").gameObject.GetComponent<Text>().text = null;
                    festivalItemPopup.transform.Find("UIBox/Inbox/ItemBox/SellText").gameObject.GetComponent<Text>().text = null;

                    festivalItemPopup.transform.Find("UIBox/Inbox/Setting").gameObject.SetActive(false);
                    festivalItemPopup.transform.Find("UIBox/Inbox/SelectText").gameObject.SetActive(true);
                    festivalItemPopup.SetActive(true);
                    //인벤토리 불러오기. blackback 없애기.
                    GameObject.Find("InventoryScript").GetComponent<Inventory>().ItemSlotCreate();
                    GameObject.Find("Menu").transform.Find("InventoryPopup").gameObject.SetActive(true);
                    GameObject.Find("Menu").transform.Find("InventoryPopup/BlackBack").gameObject.SetActive(false);

                    Debug.Log(curSelectNum);
                });
            }
            //아이템 등록
            else if(saleList[i].state == "sale")
            {
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == saleList[i].saleThings.name).grade);
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Image>().color = col;
                saleObj1[i].transform.Find("Button/Icon").gameObject.GetComponent<Image>().sprite 
                    = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == saleList[i].saleThings.name).icon);
                saleObj1[i].transform.Find("Button/Icon").gameObject.SetActive(true);
                saleObj1[i].transform.Find("Button/NameText").gameObject.SetActive(true);
                saleObj1[i].transform.Find("Button/AddText").gameObject.SetActive(false);


                //button 아이템 판매 취소. 회수. systempopup
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {

                });
            }
            //아이템 팔림
            else if(saleList[i].state == "sellout")
            {



                //button 판매금액 회수
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {

                });
            }


        }

    }



    //아이템 등록 창에서 인벤토리 아이템 선택
    public void selectItem(InventoryThings inven)
    {
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

        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/Slider").gameObject.GetComponent<Slider>().minValue = 0;
        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/Slider").gameObject.GetComponent<Slider>().maxValue = inven.possession;
        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/AmountBox/Slider").gameObject.GetComponent<Slider>().value = 1;

        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/Slider").gameObject.GetComponent<Slider>().minValue = 1;
        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/Slider").gameObject.GetComponent<Slider>().maxValue = 9;
        festivalItemPopup.transform.Find("UIBox/Inbox/Setting/UnitPrice/Slider").gameObject.GetComponent<Slider>().value = 1;




    }





}
