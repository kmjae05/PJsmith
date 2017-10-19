using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItemManager : MonoBehaviour {

    public static GetItemManager instance = new GetItemManager();

    private GameObject GetItemPopup;
    private GameObject itemList;
    private GameObject itemBox;
    private Image icon;
    private Text nameText;

    void Start () {

        GetItemPopup = GameObject.Find("System/GetItemPopup");
        itemList = GetItemPopup.transform.Find("UIPanel/Scroll/ItemList").gameObject;
        itemBox = itemList.transform.Find("ItemBox").gameObject;
        icon = itemBox.transform.Find("Icon").gameObject.GetComponent<Image>();
        nameText = itemBox.transform.Find("NameText").gameObject.GetComponent<Text>();
	}
	

    //단일
    public void getItem(string name, int num)
    {





        transform.gameObject.SetActive(true);

        StartCoroutine(disappear());
    }

    //복수
    public void getItem(string[] name, int[] num)
    {



        transform.gameObject.SetActive(true);
        StartCoroutine(disappear());
    }


    IEnumerator disappear()
    {

        yield return new WaitForSeconds(3.0f);


    }


}
