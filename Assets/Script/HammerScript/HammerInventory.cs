using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

//아이템 생성자, 중복아이템의 경우 카운트
public class HammerInventory : MonoBehaviour
{
    //   GameObject inventoryPanel; 
    GameObject HammerSlotPanel;   //gameObject.transfom.Find("02_UI/Main/Window/HammerPopup/HammerInventory Panel/BackSlot/HammerSlot Panel").gameObject;
    HammerDatabase database;
    HammerInventory HammerInv;

    public GameObject HammerSlot;
    public GameObject HammerItem;

    GameObject HammerPopup; //gameObject.transfom.Find("02_UI/Main/Window/HammerPopup").gameObject;
    GameObject BackSlot; //gameObject.transform.Find("02_UI/Main/Window/HammerPopup/HammerInventory Panel/BackSlot").gameObject;
    Text t_SlotText; //슬롯 사이즈 표시부분  //gameObject.transform.Find("02_UI/Main/Window/HammerPopup/HammerInventory Panel/SlotPlusPanel/SlotText").gameObject;

    public int slotAmount;

    public static List<Hammer> items = new List<Hammer>();
    public static List<GameObject> slots = new List<GameObject>();

    public GameObject ItemImage; //아이템 생성 완료창이미지

    GameObject Chr_001;
    GameObject StorePopup; //gameObject.transform.Find("02_UI/Main/Store/StorePopup").gameObject;

    SystemController Sys_Controller;

    string data; //툴팁 스트링
    Text t_Tooltip; //툴팁 텍스트    //gameObject.transform.Find("02_UI/System/SystemCanvas/Tooltip/Panel/Text").gameObject;
    GameObject tooltip;      //gameObject.transform.Find("02_UI/System/SystemCanvas/Tooltip").gameObject;
    GameObject TooltipImage; //툴팁 이미지   //gameObject.transform.Find("02_UI/System/SystemCanvas/Tooltip/Panel/BackImage/Image").gameObject;
    public GameObject EquipButton; //아이템 장착 버튼

    int ItemEigen;
    int EquipNo = -1;

    GameObject NewItemIcon; //new아이콘
    int NewItemCount = 0; //새로운 아이템 카운트 new아이콘 활성화

    public GameObject RandumBoxPopup;

    void Awake()
    {
        Chr_001 = transform.Find("/01_3D/Chr/Chr_001").gameObject;

        HammerPopup = transform.Find("/02_UI/Main/Window/HammerPopup").gameObject;
        HammerSlotPanel = transform.Find("/02_UI/Main/Window/HammerPopup/HammerInventory Panel/BackSlot/HammerSlot Panel").gameObject;
        BackSlot = transform.Find("/02_UI/Main/Window/HammerPopup/HammerInventory Panel/BackSlot").gameObject;
        StorePopup = transform.Find("/02_UI/Main/Store/StorePopup").gameObject;
        tooltip = transform.Find("/02_UI/System/SystemCanvas/Tooltip").gameObject;
        TooltipImage = transform.Find("/02_UI/System/SystemCanvas/Tooltip/Panel/BackImage/Image").gameObject;
        NewItemIcon = transform.Find("/02_UI/Main/Button/HammerButton/Icon").gameObject;

        t_SlotText = transform.Find("/02_UI/Main/Window/HammerPopup/HammerInventory Panel/SlotPlusPanel/SlotText").gameObject.GetComponent<Text>();
        t_Tooltip = transform.Find("/02_UI/System/SystemCanvas/Tooltip/Panel/Text").gameObject.GetComponent<Text>();


    }

    void Start()
    {
        Sys_Controller = transform.Find("/05_Script/SystemPopUpObject").gameObject.GetComponent<SystemController>();

        HammerInv = HammerPopup.GetComponent<HammerInventory>();
        database = GetComponent<HammerDatabase>();
        HammerPopup.SetActive(true);

        HammerSlot.SetActive(true);
        int i;
        for (i = 0; i < slotAmount; i++)
        {
            items.Add(new Hammer());
            slots.Add(Instantiate(HammerSlot));     //인벤토리 슬롯 생성
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(HammerSlotPanel.transform);
            slots[i].GetComponent<RectTransform>().transform.localScale = Vector3.one;
            slots[i].GetComponent<RectTransform>().transform.localPosition = Vector3.one;
        }
        HammerSlot.SetActive(false);
        HammerPopup.SetActive(false);
    }
    public void Button()
    {
        AddItem(Random.Range(0, 5));
    }
    public void AddItem(int id)     //아이템 생성
    {
        Hammer itemToAdd = database.FetchItemByID(id);
        if (itemToAdd == null)
        {
            return;
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == -1)
            {
                NewItemCount++;
                NewItemIcon.SetActive(true);

                items[i] = itemToAdd;
                GameObject itemObj = Instantiate(HammerItem);
                itemObj.SetActive(true);
                itemObj.GetComponent<ItemData>().hammer = itemToAdd;
                itemObj.GetComponent<ItemData>().amount = 1;
                itemObj.GetComponent<ItemData>().slot = i;
                itemObj.transform.SetParent(slots[i].transform);
                itemObj.transform.position = slots[i].transform.position;

                itemObj.GetComponent<Image>().sprite = itemToAdd.sprite;
                GameObject image = itemObj.transform.Find("Image").gameObject;
                image.GetComponent<Image>().sprite = itemToAdd.sprite;

                GameObject TempItem = itemObj.transform.Find("NewIcon").gameObject;
                TempItem.SetActive(true);

                itemObj.name = itemToAdd.name;
                itemObj.GetComponent<Button>().onClick.AddListener(() => Activate(i));
                itemObj.GetComponent<RectTransform>().transform.localScale = Vector3.one;
                break;
            }
        }
    }

    bool CheckIfItemIsInInventory(Hammer item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == item.Id)
            {
                return true;
            }
        }
        return false;
    }
    public void ItemSprite(Sprite sprite) //아이템 생성 완료창 스프라이트 넣음
    {
        ItemImage.GetComponent<Image>().sprite = sprite;
    }

    public static void RemoveItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id != -1 && items[i].Id == id)
            {
                Destroy(slots[i].transform.GetChild(0).gameObject);
                items[i] = new Hammer();
                break;
            }
        }
    }﻿
    public void Activate(int id) //클릭시 아이템의 값들을 받아와서 아이템 타입을 설정
    {
        if (RandumBoxPopup.activeSelf == false)
        {
            ConstructDataString(id);
        }
        else if (RandumBoxPopup.activeSelf == true)
        {
            GameObject Select = slots[id].transform.GetChild(0).gameObject;
            Select = Select.transform.Find("Select").gameObject;
            if (Select.activeSelf == false)
            {
                RandumBoxPopup.GetComponent<RandumBoxHandle>().ItemSelect(items, Select, id);
            }
        }
    }

    public void ConstructDataString(int id) //클릭시 툴팁을 실행
    {
        GameObject NewIcon = slots[id].transform.GetChild(0).gameObject;
        NewIcon = NewIcon.transform.Find("NewIcon").gameObject;
        data = "<color=#0473f0><b>" + items[id].name + "</b></color>\n\n" + items[id].power + "\nPower : " + items[id].power;

        ItemEigen = id;
        if (EquipNo == id)//items[id].ItemEigen)
        {
            data = "<color=#0473f0><b>" + items[id].name + "</b></color>\n\n" + items[id].power + "\nPower : " + items[id].power + "\n\n장착중";
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id != -1)
            {
                GameObject Select = slots[i].transform.GetChild(0).gameObject;
                Select = Select.transform.Find("Select").gameObject;
                if (i == ItemEigen)
                {
                    Select.SetActive(true);
                }
                else
                {
                    Select.SetActive(false);
                }
            }
        }
        NewIcon.SetActive(false);
        if (NewItemIcon == true)
        {
            NewItemCount--;
        }

        if (NewItemCount <= 0)
        {
            NewItemIcon.SetActive(false);
        }
        //tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        t_Tooltip.text = data;
        tooltip.SetActive(true);
 //       EquipButton.SetActive(true);
        TooltipImage.GetComponent<Image>().sprite = items[id].sprite;
        TooltipImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }
    public void Equip()
    {
        Hammer hammer = items[ItemEigen];
        EquipNo = ItemEigen;
        //tooltip.SetActive(false);
        Player.equipHm = hammer;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id != -1)
            {
                GameObject EquipLine = slots[i].transform.GetChild(0).gameObject;
                EquipLine = EquipLine.transform.Find("EquipLine").gameObject;

                if (i == ItemEigen)
                {
                    EquipLine.SetActive(true);
                }
                else
                {
                    EquipLine.SetActive(false);
                }
            }
        }
    }
    public void AddSlorPopup()
    {
        Sys_Controller.Sys_TitleText.GetComponent<Text>().text = "아이템 슬롯 수 증가";
        Sys_Controller.Sys_InfoText.GetComponent<Text>().text = "보석을 사용하여 슬롯 수를 증가 시키시겠습니까?";

        Sys_Controller.Sys_YesButton.SetActive(true);
        Sys_Controller.Sys_NoButton.SetActive(true);
        Sys_Controller.Sys_OkButton.SetActive(false);

        Sys_Controller.Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
        Sys_Controller.Sys_YesButton.GetComponent<Button>().onClick.AddListener(() => Click_Yes_AddSlot());    //버튼 기능 추가
        Sys_Controller.SystemPopup.SetActive(true);
    }
    public void Click_Yes_AddSlot() //슬롯추가
    {
        if (Player.Play.cash < 30)
        {
            Sys_Controller.SystemPopup.SetActive(false);

            //SystemPopup 종료
            Sys_Controller.Sys_TitleText.GetComponent<Text>().text = "보석이 부족합니다";
            Sys_Controller.Sys_InfoText.GetComponent<Text>().text = "보석 구매 페이지로 이동하시겠습니까?";

            Sys_Controller.Sys_YesButton.SetActive(true);     //예/아니오 버튼으로 수정
            Sys_Controller.Sys_NoButton.SetActive(true);
            Sys_Controller.Sys_OkButton.SetActive(false);

            Sys_Controller.Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            Sys_Controller.Sys_YesButton.GetComponent<Button>().onClick.AddListener(() => Sys_Controller.SystemPopup.SetActive(false));   //시스템 팝업 끄기
            Sys_Controller.Sys_YesButton.GetComponent<Button>().onClick.AddListener(() => StorePopup.SetActive(true));     //상점 팝업 켜기

            Sys_Controller.SystemPopup.SetActive(true);
        }
        else
        {
            Player.Play.cash -= 30;
            Sys_Controller.SystemPopup.SetActive(false);

            Sys_Controller.Sys_OkButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            Sys_Controller.Sys_OkButton.GetComponent<Button>().onClick.AddListener(() => AddSlot());  //확인 버튼 클릭 시 ClickOre 실행
            Sys_Controller.Sys_TitleText.GetComponent<Text>().text = "알림";
            Sys_Controller.Sys_InfoText.GetComponent<Text>().text = "슬롯 수를 증가 시켰습니다.";
            Sys_Controller.Sys_YesButton.SetActive(false);     //확인 버튼으로 수정
            Sys_Controller.Sys_NoButton.SetActive(false);
            Sys_Controller.Sys_OkButton.SetActive(true);

            Sys_Controller.SystemPopup.SetActive(true);
        }
    }
    public void AddSlot()
    {
        int itemsSize = items.Count;
        for (int i = items.Count; i < itemsSize + 8; i++)
        {
            items.Add(new Hammer());
            slots.Add(Instantiate(HammerSlot));     //인벤토리 슬롯 생성
            slots[i].GetComponent<Slot>().id = i;
            slots[i].transform.SetParent(HammerSlotPanel.transform);
            slots[i].GetComponent<RectTransform>().transform.localScale = Vector3.one;
        }
        SlotSize();
    }
    public void SlotSize()
    {
        HammerSlotPanel.transform.position = BackSlot.transform.position;
        int SlotTextSize = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id != -1)
            {
                SlotTextSize++;
            }
        }

        t_SlotText.text = SlotTextSize.ToString() + "/" + items.Count;

        HammerSlotPanel.transform.localPosition = Vector3.one;
    }


    //public void itemPowerAscending(int ascending)
    //{
    //    List<GameObject> TempSlots = new List<GameObject>();
        
    //    int itemCount = 0;

    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        if (items[i].Id != -1)
    //        {
    //            itemCount++;
    //        }
    //    }
    //    if (itemCount != 0 && ascending == 0)
    //    {
    //        for (int i = 0; i < items.Count; i++)
    //        {
    //            for (int j = 0; j < items.Count - 1; j++)
    //            {
    //                if (items[j].power < items[j + 1].power) //내림차순
    //                {
    //                    Hammer temp;

    //                    temp = items[j];
    //                    items[j] = items[j + 1];
    //                    items[j + 1] = temp;
    //                }

    //            }
    //        }
    //    }
    //    else if (itemCount != 0 && ascending == 1)
    //    {
    //        for (int i = 0; i < items.Count; i++)
    //        {
    //            for (int j = 0; j < items.Count - 1; j++)
    //            {
    //                if (items[j].power > items[j + 1].power) //올림차순
    //                {
    //                    Hammer temp;

    //                    temp = items[j];
    //                    items[j] = items[j + 1];
    //                    items[j + 1] = temp;
    //                }
    //            }
    //        }
    //    }
    //    if (itemCount != 0)
    //    {
    //        for (int i = 0; i < slots.Count; i++)
    //        {
    //            if (items[i].Id != -1)
    //            {
    //             //   GameObject itemObj = slots[i].transform.GetChild(0).gameObject;
    //             ////   Debug.Log(itemObj);
    //             //   itemObj.transform.SetParent(slots[i].transform);
    //             //   itemObj.transform.localPosition = Vector3.one;
    //                slots[i].transform.GetChild(0).gameObject.transform.SetParent(slots[i].transform);
    //                slots[i].transform.GetChild(0).gameObject.transform.localPosition = Vector3.one;
    //            }
    //        }
    //    }
    //    SlotSize();
    //}
    public void itemPowerAscending(int ascending)
    {
        List<int> TempNo = new List<int>();
        List<int> TempPower = new List<int>();
        int itemCount = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id != -1)
            {
                TempPower.Add(items[i].power);
                TempNo.Add(items[i].Id);
                itemCount++;
            }
        }
        if (itemCount != 0 && ascending == 0)
        {
            for (int i = 0; i < TempPower.Count; i++)
            {
                for (int j = 0; j < TempPower.Count - 1; j++)
                {
                    if (TempPower[j] < TempPower[j + 1]) //내림차순
                    {
                        int temp;
                        temp = TempPower[j];
                        TempPower[j] = TempPower[j + 1];
                        TempPower[j + 1] = temp;
                        temp = TempNo[j];
                        TempNo[j] = TempNo[j + 1];
                        TempNo[j + 1] = temp;
                    }

                }
            }
        }
        else if (itemCount != 0 && ascending == 1)
        {
            for (int i = 0; i < TempPower.Count; i++)
            {
                for (int j = 0; j < TempPower.Count - 1; j++)
                {
                    if (TempPower[j] > TempPower[j + 1]) //올림차순
                    {
                        int temp;
                        temp = TempPower[j];
                        TempPower[j] = TempPower[j + 1];
                        TempPower[j + 1] = temp;
                        temp = TempNo[j];
                        TempNo[j] = TempNo[j + 1];
                        TempNo[j + 1] = temp;
                    }
                }
            }
        }

        if (itemCount != 0)
        {
            for (int i = 0; i < TempNo.Count; i++)
            {
                RemoveItem(items[i].Id);
            }
            for (int i = 0; i < TempNo.Count; i++)
            {
                AddItem(TempNo[i]);
            }
        }
        SlotSize();
    }
}
