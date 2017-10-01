using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryHandle : MonoBehaviour
{
    private GameObject InventoryPopup;
    private GameObject UIPanel;
    private GameObject Scroll;
    private GameObject Panel_1;
    private GameObject defaultSlot;

    private List<Item> itemList;
    private int inventorySize;

    //item object data
    private Image[] IconImage;
    private Text[] HaveText;


    public class Item{
        int type;
        int no;
        string name;
        int price;
        int power;
        string comments;
        Sprite icon;
    }
    void Awake()
    {
        InventoryPopup = GameObject.Find("Menu").transform.Find("InventoryPopup").gameObject;
        UIPanel = InventoryPopup.transform.Find("UIPanel").gameObject;
        Scroll = UIPanel.transform.Find("Scroll").gameObject;
        Panel_1 = Scroll.transform.Find("Panel_1").gameObject;
        defaultSlot = Panel_1.transform.Find("InventoryBox").gameObject;
        defaultSlot.SetActive(false);
    }
    void Start()
    {
        inventorySize = 15;
        itemList = new List<Item>(inventorySize);

        for (int i = 0; i < inventorySize; i++)
        {
            GameObject slot = Instantiate(defaultSlot);
            slot.transform.SetParent(Panel_1.transform,false);
            slot.SetActive(true);
        }
    }
}
