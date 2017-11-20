using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FestivalManager : MonoBehaviour {

    private GameObject uibox;
    private GameObject ItemList;
    private GameObject panel1;
    private GameObject panel2;


    private List<ForSale> saleList;
    private List<GameObject> saleObj1;


	void Start ()
    {
        saleList = FestivalData.instance.getSaleList();

        uibox = GameObject.Find("System").transform.Find("Festival/UIBox").gameObject;
        ItemList = uibox.transform.Find("ItemList").gameObject;
        panel1 = ItemList.transform.Find("Panel1").gameObject;
        panel2 = ItemList.transform.Find("Panel2").gameObject;

        saleObj1 = new List<GameObject>();

        for (int i = 0; i < saleList.Count; i++)
            saleObj1.Add(panel1.transform.Find("ItemBox (" + i + ")").gameObject);


        //상태 갱신
        updateState();





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
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                saleObj1[i].transform.Find("Button").gameObject.GetComponent<Button>().onClick.AddListener(() => 
                {
                    GameObject.Find("System").transform.Find("FestivalItem").gameObject.SetActive(true);
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









}
