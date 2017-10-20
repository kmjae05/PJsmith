using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItemManager : MonoBehaviour {

    public static GetItemManager instance = new GetItemManager();

    private GameObject GetItemPopup;
    private GameObject itemListObj;
    private GameObject itemBox;
    private Color gradeframeColor;
    private Image icon;
    private Text amountText;
    private Text nameText;

    void Start () {

        GetItemPopup = GameObject.Find("System").transform.Find("GetItemPopup").gameObject;
        itemListObj = GetItemPopup.transform.Find("UIPanel/Scroll/ItemList").gameObject;
        itemBox = itemListObj.transform.Find("ItemBox").gameObject;
        gradeframeColor = itemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color;
        icon = itemBox.transform.Find("Icon").gameObject.gameObject.GetComponent<Image>();
        amountText = itemBox.transform.Find("AmountText").gameObject.GetComponent<Text>();
        nameText = itemBox.transform.Find("NameText").gameObject.GetComponent<Text>();
    }
	

    //단일
    public void getItem(string name, int num)
    {
        Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == name).grade);
        gradeframeColor = col;

        icon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == name).icon);
        amountText.text = num.ToString();
        nameText.text = name;

        GameObject obj = Instantiate(itemBox);
        obj.transform.SetParent(itemListObj.transform, false);
        obj.SetActive(true);

        GetItemPopup.SetActive(true);

        StartCoroutine(disappear());
    }

    //복수
    public void getItem(string[] name, int[] num)
    {
        for(int i = 0; i < name.Length; i++)
        {
            if (num[i] > 0)
            {
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == name[i]).grade);
                gradeframeColor = col;

                icon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == name[i]).icon);
                amountText.text = num[i].ToString();
                nameText.text = name[i];

                GameObject obj = Instantiate(itemBox);
                obj.transform.SetParent(itemListObj.transform, false);
                obj.SetActive(true);
            }
        }

        GetItemPopup.SetActive(true);
        StartCoroutine(disappear());
    }


    IEnumerator disappear()
    {

        yield return new WaitForSeconds(3.0f);

        //오브젝트 삭제
        if (itemListObj.transform.childCount > 1)
        {
            for(int i=1;i< itemListObj.transform.childCount; i++)
            {
                Destroy(itemListObj.transform.GetChild(i).gameObject);
            }
        }

        GetItemPopup.SetActive(false);

    }


}
