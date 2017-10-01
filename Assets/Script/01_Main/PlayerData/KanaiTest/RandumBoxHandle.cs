using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RandumBoxHandle : MonoBehaviour
{
    public GameObject RandumSlot;
    public GameObject RandumSlotPanel;
    public List<GameObject> RandumItemList = new List<GameObject>();

    public GameObject HammerInventoryPopup;

    List<int> SlotSave = new List<int>();

    //public GameObject ResultPopup;
    //public GameObject Res_TitleText;
    //public GameObject Res_InfoText;
    //public GameObject Res_Image;

	// Use this for initialization
	void Start () {
	    for(int i=0; i<9; i++){
            RandumItemList.Add(Instantiate(RandumSlot));
            RandumItemList[i].transform.SetParent(RandumSlotPanel.transform, false);
            RandumItemList[i].GetComponent<RandumBoxItem>().SlotNum = i;
            RandumItemList[i].GetComponent<RandumBoxItem>().itemType = -1;
            RandumItemList[i].GetComponent<RandumBoxItem>().itemNum = -1;
            RandumItemList[i].SetActive(true);
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
    public void ItemSelect(List<Hammer> Item, GameObject Select, int id)
    {
        for (int i = 0; i < RandumItemList.Count; i++)
        {
            if (RandumItemList[i].GetComponent<RandumBoxItem>().itemNum == -1)
            {
                SlotSave.Add(id);
                GameObject ItemImage;
                RandumItemList[i].GetComponent<RandumBoxItem>().itemNum = Item[id].ItemEigen;
                ItemImage = RandumItemList[i].transform.GetChild(0).gameObject;
                ItemImage.GetComponent<Image>().sprite = Item[id].sprite;
                Select.SetActive(true);
                break;
            }
        }
    }
    public void ItemCompound()
    {
        GameObject ItemImage;
        for (int i = 0; i < RandumItemList.Count; i++)
        {
            if (RandumItemList[i].GetComponent<RandumBoxItem>().itemNum != -1)
            {
                ItemImage = RandumItemList[i].transform.GetChild(0).gameObject;
                ItemImage.GetComponent<Image>().sprite = null;
                RandumItemList[i].GetComponent<RandumBoxItem>().itemNum = -1;
            }
        }

        for (int i = 0; i < SlotSave.Count; i++)
        {
            HammerInventory.RemoveItem(SlotSave[i]);
        }
        HammerInventoryPopup.GetComponent<HammerInventory>().itemPowerAscending(0);
        HammerInventoryPopup.GetComponent<HammerInventory>().AddItem(Random.Range(0, 4));
    }

}
