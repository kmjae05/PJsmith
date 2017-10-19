using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

//아이템 생성자, 중복아이템의 경우 카운트
public class Inventory : MonoBehaviour
{
    public GameObject InventorySlot;
    public GameObject InventoryItem;

    public int slotAmount; //슬롯개수 -> 향후 탭별로 갯수에 맞도록 관리
    //ItemDatabase database;
    ThingsData thingsData;
    EquipmentData equipmentData;

    List<Things> Tap1Items = new List<Things>(); //각 아이템별 리스트
    List<GameObject> Tap1Slots = new List<GameObject>();

    public List<Things> Tap2Items = new List<Things>();
    public List<GameObject> Tap2Slots = new List<GameObject>();

    public List<Things> Tap3Items = new List<Things>();
    public List<GameObject> Tap3Slots = new List<GameObject>();

    public List<Things> Tap4Items = new List<Things>();
    public List<GameObject> Tap4Slots = new List<GameObject>();

    public List<Things> Tap5Items = new List<Things>();
    public List<GameObject> Tap5Slots = new List<GameObject>();

    //public List<Things> Tap6Items = new List<Things>();
    //public List<GameObject> Tap6Slots = new List<GameObject>();

    GameObject CollectionPopup;
    GameObject CollectionPanel;

    GameObject BackSlot;
    //컬렉션 탭
    GameObject Tap1Panel;
    GameObject Tap2Panel;
    GameObject Tap3Panel;
    GameObject Tap4Panel;
    GameObject Tap5Panel;
    //GameObject Tap6Panel;

    //컬렉션 탭 전환
    GameObject Tap1Push;
    GameObject Tap2Push;
    GameObject Tap3Push;
    GameObject Tap4Push;
    GameObject Tap5Push;
    //GameObject Tap6Push;

    //컬렉션 탭 버튼
    GameObject Tap1Button;
    GameObject Tap2Button;
    GameObject Tap3Button;
    GameObject Tap4Button;
    GameObject Tap5Button;
    //GameObject Tap6Button;

    Text t_SlotText; //인벤토리 아이템 갯수
    Text t_SlotTitleText; //인벤토리 카테고리 타이틀

    //string data; //툴팁 아이템정보 스트링
    //Text t_Tooltip; //툴팁 텍스트
    GameObject ItemInfoPopup; //툴팁
    //GameObject TooltipImage; //툴팁 이미지
    GameObject EquipItemInfoPopup;


    public GameObject ItemImage; //아이템생성완료창 스프라이트
    public Text ItemText; //아이템 생성완료창 텍스트

    GameObject NewItemIcon; //new아이콘
    int NewItemCount = 0; //새로운 아이템 카운트 new아이콘 활성화

    void Awake()
    {
        CollectionPopup = transform.Find("/02_UI/Main/Menu/InventoryPopup").gameObject;
        CollectionPanel = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/Scroll").gameObject;

        BackSlot = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/Scroll").gameObject;

        //컬렉션 탭
        Tap1Panel = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/Scroll/Panel_1").gameObject;
        Tap2Panel = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/Scroll/Panel_2").gameObject;
        Tap3Panel = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/Scroll/Panel_3").gameObject;
        Tap4Panel = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/Scroll/Panel_4").gameObject;
        Tap5Panel = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/Scroll/Panel_5").gameObject;
        //Tap6Panel = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/Scroll/Panel_6").gameObject;

        Tap1Button = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Normal1").gameObject;
        Tap2Button = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Normal2").gameObject;
        Tap3Button = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Normal3").gameObject;
        Tap4Button = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Normal4").gameObject;
        Tap5Button = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Normal5").gameObject;
        //Tap6Button = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Normal6").gameObject;

        //컬렉션 탭 전환
        Tap1Push = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Tab1").gameObject;
        Tap2Push = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Tab2").gameObject;
        Tap3Push = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Tab3").gameObject;
        Tap4Push = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Tab4").gameObject;
        Tap5Push = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Tab5").gameObject;
        //Tap6Push = transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/TabPanel/Tab6").gameObject;


        ItemInfoPopup = transform.Find("/02_UI/System/ItemInfoPopup").gameObject;
        EquipItemInfoPopup = GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject;
        //NewItemIcon = transform.Find("/02_UI/Main/Button/CollectionButton/Icon").gameObject;

        t_SlotText = gameObject.transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/SlotPlusBox/SlotText").gameObject.GetComponent<Text>();
        t_SlotTitleText = gameObject.transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/BoxBack/Text").gameObject.GetComponent<Text>();
        //InventorySlot.SetActive(true);
        CollectionPopup.SetActive(true);
    }
    void Start()
    {
        //database = GetComponent<ItemDatabase>();
        thingsData = GameObject.Find("ThingsData").GetComponent<ThingsData>();
        //ItemSlotCreate();
        TapButtonSetup();
        CollectionPopup.SetActive(false);
        //StartCoroutine(StartRoutine());
        //InventorySlot.SetActive(false);
        equipmentData = GameObject.Find("PlayerData").GetComponent<EquipmentData>();

        GameObject.Find("InvenButton").GetComponent<Button>().onClick.AddListener(() => {
            Tap1Panel.SetActive(true); Tap1Push.SetActive(true); SwitchScrollPanel();
        });
    }

    IEnumerator StartRoutine() //초기 아이템 생성
    {
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < thingsData.getThingsList().Count; i++)
        {
            AddItem(0, i);
        }
        for (int i = 0; i < ItemDatabase.DatabaseListSize1; i++)
        {
            AddItem(1, i);
        }
        for (int i = 0; i < ItemDatabase.DatabaseListSize2; i++)
        {
            AddItem(2, i);
        }
        for (int i = 0; i < ItemDatabase.DatabaseListSize3; i++)
        {
            AddItem(3, i);
        }
        for (int i = 0; i < ItemDatabase.DatabaseListSize3; i++)
        {
            AddItem(3, i);
        }
        for (int i = 0; i < ItemDatabase.DatabaseListSize3; i++)
        {
            AddItem(3, i);
        }
    }

    public void Itemparsing(int tapNo, int ItemNO) //아이템 획득시 이미지값 삽입 탭에 맞는 부분을 바꾸는부분 미완성
    {
        List<Things> TempItem = new List<Things>();
        List<GameObject> TempSlot = new List<GameObject>();
        if (tapNo == 0)
        {
            TempItem = Tap1Items;
            TempSlot = Tap1Slots;
        }
        else if (tapNo == 1)
        {
            TempItem = Tap2Items;
            TempSlot = Tap2Slots;
        }
        else if (tapNo == 2)
        {
            TempItem = Tap3Items;
            TempSlot = Tap3Slots;
        }
        else if (tapNo == 3)
        {
            TempItem = Tap4Items;
            TempSlot = Tap4Slots;
        }
        else if (tapNo == 4)
        {
            TempItem = Tap5Items;
            TempSlot = Tap5Slots;
        }
        //else if (tapNo == 5)
        //{
        //    TempItem = Tap6Items;
        //    TempSlot = Tap6Slots;
        //}
        if (NewItemCount <= 0)
        {
            NewItemIcon.SetActive(false);
        }
        //for (int i = 0; i < TempItem.Count; i++)
        //{
        //    if (TempItem[i].ID == ItemNO && TempItem[i].Possession == false)
        //    {
        //        TempItem[i].Possession = true;
        //        GameObject itemObj = TempSlot[i].transform.GetChild(0).gameObject;
        //        itemObj.GetComponent<Image>().color = new Color(255, 255, 255, 255);

        //        ItemSprite(TempItem[i].sprite);
        //        ItemText.text = TempItem[i].Title;
        //        itemObj = itemObj.transform.Find("NewIcon").gameObject;
        //        itemObj.SetActive(true);

        //        NewItemCount++;
        //        if (NewItemCount > 0)
        //        {
        //            NewItemIcon.SetActive(true);
        //        }
        //        break;
        //    }
        //    else if (TempItem[i].ID == ItemNO && TempItem[i].Possession == true)
        //    {
        //        ItemSprite(TempItem[i].sprite);
        //        ItemText.text = TempItem[i].Title;
        //    }
        //}

    }
    bool CheckIfItemIsInInventory(int tatNo, Things item) //아이템이 db에 있는지 확인
    {
        List<Things> items = new List<Things>();
        if (tatNo == 0)
        {
            items = Tap1Items;
        }
        else if (tatNo == 1)
        {
            items = Tap2Items;
        }
        else if (tatNo == 2)
        {
            items = Tap3Items;
        }
        else if (tatNo == 3)
        {
            items = Tap4Items;
        }
        else if (tatNo == 4)
        {
            items = Tap5Items;
        }
        //else if (tatNo == 5)
        //{
        //    items = Tap6Items;
        //}
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item_no == item.item_no)
            {
                return true;
            }
        }
        return false;
    }
    void ItemSprite(Sprite sprite) //아이템 생성 완료창 스프라이트 넣음
    {
        ItemImage.GetComponent<Image>().sprite = sprite;
    }

    public void RemoveItem(int tapNo, int id) //아이템 삭제전 삭제할 탭 지적
    {
        List<Things> item = new List<Things>();
        List<GameObject> slot = new List<GameObject>();

        if (tapNo == 0)
        {
            item = Tap1Items;
            slot = Tap1Slots;
        }
        else if (tapNo == 1)
        {
            item = Tap2Items;
            slot = Tap2Slots;
        }
        else if (tapNo == 2)
        {
            item = Tap3Items;
            slot = Tap3Slots;
        }
        else if (tapNo == 3)
        {
            item = Tap4Items;
            slot = Tap4Slots;
        }
        else if (tapNo == 4)
        {
            item = Tap5Items;
            slot = Tap5Slots;
        }
        //else if (tapNo == 5)
        //{
        //    item = Tap6Items;
        //    slot = Tap6Slots;
        //}
        //RemoveItem(item, slot, tapNo, id);
    }
    //void RemoveItem(List<Things> Items, List<GameObject> Slots, int tapNo, int id) //삭제할 아이템을 지움
    //{
    //    Things itemToRemove = database.FetchItemByID(tapNo, id);
    //    if (itemToRemove.Stackable && CheckIfItemIsInInventory(tapNo, itemToRemove))
    //    {
    //        for (int j = 0; j < Items.Count; j++)
    //        {
    //            if (Items[j].ID == id)
    //            {
    //                ItemData data = Slots[j].transform.GetChild(0).GetComponent<ItemData>();
    //                data.amount--;
    //                data.transform.GetChild(3).GetComponent<Text>().text = data.amount.ToString();
    //                if (data.amount <= 0)
    //                {
    //                    Destroy(Slots[j].transform.GetChild(3).gameObject);
    //                    Items[j] = new Item();
    //                    break;
    //                }
    //                if (data.amount == 1)
    //                {
    //                    Slots[j].transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "";
    //                    break;
    //                }
    //                break;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < Items.Count; i++)
    //            if (Items[i].ID != -1 && Items[i].ID == id)
    //            {
    //                Destroy(Slots[i].transform.GetChild(0).gameObject);
    //                Items[i] = new Item();
    //                break;
    //            }
    //    }
    //}

    public void Activate(int DatabaseNo, int id) //클릭시 아이템의 값들을 받아와서 아이템 타입을 설정
    {
        List<Things> TempItems = new List<Things>();
        List<GameObject> TempISlot = new List<GameObject>();
        if (DatabaseNo == 0)
        {
            TempItems = Tap1Items;
            TempISlot = Tap1Slots;
        }
        else if (DatabaseNo == 1)
        {
            TempItems = Tap2Items;
            TempISlot = Tap2Slots;
        }
        else if (DatabaseNo == 2)
        {
            TempItems = Tap3Items;
            TempISlot = Tap3Slots;
        }
        else if (DatabaseNo == 3)
        {
            TempItems = Tap4Items;
            TempISlot = Tap4Slots;
        }
        else if (DatabaseNo == 4)
        {
            TempItems = Tap5Items;
            TempISlot = Tap5Slots;
        }
        //else if (DatabaseNo == 5)
        //{
        //    TempItems = Tap6Items;
        //    TempISlot = Tap6Slots;
        //}

        //ConstructDataString(TempItems, TempISlot, id);
    }
    void ConstructDataString(List<Things> TempItems, List<GameObject> TempISlot, int id)//설정한 아이템 타입의 값들을 툴팁에 대입
    {
        // GameObject itemObj = TempISlot[id].transform.GetChild(0).gameObject;

        // itemObj = itemObj.transform.FindChild("NewIcon").gameObject;
        // if (itemObj.activeSelf == true)
        // {
        //     NewItemCount--;
        //     itemObj.SetActive(false);
        // }
        // if (NewItemCount <= 0)
        // {
        //     NewItemIcon.SetActive(false);
        // }

        //data = "<color=#0473f0><b>" + TempItems[id].Title + "</b></color>\n\n" + TempItems[id].Decription + "\nPower : " + TempItems[id].Power;
        ////tooltip.transform.GetChild(0).GetComponent<Text>().text = data;
        //t_Tooltip.text = data;
        //ItemInfoPopup.SetActive(true);
        //if (TempItems[id].Possession == true)
        //{
        //    TooltipImage.GetComponent<Image>().sprite = TempItems[id].sprite;
        //    TooltipImage.GetComponent<Image>().color = new Color(255, 255, 255, 255);
        //}
        //else
        //{
        //    TooltipImage.GetComponent<Image>().sprite = TempItems[id].sprite;
        //    TooltipImage.GetComponent<Image>().color = new Color(0, 0, 0, 255);
        //}
    }
    public void ItemSlotCreate()
    {
        for(int i = 0; i < Tap1Slots.Count; i++)             Destroy(Tap1Slots[i]);
        for (int i = 0; i < Tap2Slots.Count; i++)            Destroy(Tap2Slots[i]);
        for (int i = 0; i < Tap3Slots.Count; i++)            Destroy(Tap3Slots[i]);
        for (int i = 0; i < Tap4Slots.Count; i++)            Destroy(Tap4Slots[i]);
        for (int i = 0; i < Tap5Slots.Count; i++)            Destroy(Tap5Slots[i]);
        //for (int i = 0; i < Tap6Slots.Count; i++)            Destroy(Tap6Slots[i]);

        Tap1Items.Clear();
        Tap2Items.Clear();
        Tap3Items.Clear();
        Tap4Items.Clear();
        Tap5Items.Clear();
        //Tap6Items.Clear();

        Tap1Slots.Clear();
        Tap2Slots.Clear();
        Tap3Slots.Clear();
        Tap4Slots.Clear();
        Tap5Slots.Clear();
        //Tap6Slots.Clear();

        //아이템 전체
        List<Things> tempItemList1 = new List<Things>();
        for(int i=0;i< thingsData.getThingsList().Count; i++) tempItemList1.Add(thingsData.getThingsList()[i]);
        for (int i = 0; i < tempItemList1.Count; i++)
        {
            if (tempItemList1[i].possession > 0)
            {
                Tap1Items.Add(tempItemList1[i]);
                Tap1Slots.Add(Instantiate(InventorySlot)); // 아이템 생성
                Tap1Slots[Tap1Slots.Count - 1].SetActive(true);
                Tap1Slots[Tap1Slots.Count - 1].GetComponent<Slot>().id = Tap1Slots.Count - 1;
                Tap1Slots[Tap1Slots.Count - 1].transform.SetParent(Tap1Panel.transform);
                Tap1Slots[Tap1Slots.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
                Tap1Slots[Tap1Slots.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.one;
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(tempItemList1[i].icon);
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList1[i].possession.ToString();
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList1[i].recent);

                //광석 팝업 
                if (tempItemList1[i].type == "Ore")
                {
                    Debug.Log(tempItemList1[i].name);
                    int index = i;
                    Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject.Find("PlayerData").GetComponent<OreSelect>().ClickInventory(tempItemList1[index]);
                        GameObject.Find("System").transform.Find("OreInfoPopup").gameObject.SetActive(true);
                    });
                }
                //무기 팝업
                else if (tempItemList1[i].type == "Helmet" || tempItemList1[i].type == "Armor"
                    || tempItemList1[i].type == "Weapon" || tempItemList1[i].type == "Boots")
                {
                    int index = i;
                    Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        EquipInfoPopup(tempItemList1[index]);
                        EquipItemInfoPopup.SetActive(true);
                    });
                }
                //기타 아이템 팝업
                else if (tempItemList1[i].type == "Others")
                {
                    int index = i;
                    Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OthersItemInfoPopup(tempItemList1[index]);
                        ItemInfoPopup.SetActive(true);
                    });
                }
            }
        }
        //무기
        List<Things> tempItemList2 = new List<Things>();
        tempItemList2 = thingsData.getThingsList().FindAll(x => x.type == "Weapon");
        for (int i = 0; i < tempItemList2.Count; i++)
        {
            if (tempItemList2[i].possession > 0)
            {
                Tap2Items.Add(tempItemList2[i]);
                Tap2Slots.Add(Instantiate(InventorySlot)); // 아이템 생성
                Tap2Slots[Tap2Slots.Count - 1].SetActive(true);
                Tap2Slots[Tap2Slots.Count - 1].GetComponent<Slot>().id = Tap2Slots.Count - 1;
                Tap2Slots[Tap2Slots.Count - 1].transform.SetParent(Tap2Panel.transform);
                Tap2Slots[Tap2Slots.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
                Tap2Slots[Tap2Slots.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.one;
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(tempItemList2[i].icon);
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList2[i].possession.ToString();
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList2[i].recent);

                //무기 팝업
                if (tempItemList2[i].type == "Weapon")
                {
                    int index = i;
                    Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        EquipInfoPopup(tempItemList2[index]);
                        EquipItemInfoPopup.SetActive(true);
                    });
                }
            }

        }
        //방어구
        List<Things> tempItemList3 = new List<Things>();
        tempItemList3 = thingsData.getThingsList().FindAll(x => x.type == "Armor" || x.type == "Helmet" || x.type == "Gloves" || x.type == "Pants"|| x.type == "Boots");
        for (int i = 0; i < tempItemList3.Count; i++)
        {
            if (tempItemList3[i].possession > 0)
            {
                Tap3Items.Add(tempItemList3[i]);
                Tap3Slots.Add(Instantiate(InventorySlot)); // 아이템 생성Helmetv     
                Tap3Slots[Tap3Slots.Count - 1].SetActive(true);
                Tap3Slots[Tap3Slots.Count - 1].GetComponent<Slot>().id = Tap3Slots.Count - 1;
                Tap3Slots[Tap3Slots.Count - 1].transform.SetParent(Tap3Panel.transform);
                Tap3Slots[Tap3Slots.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
                Tap3Slots[Tap3Slots.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.one;
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(tempItemList3[i].icon);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList3[i].possession.ToString();
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList3[i].recent);

                //방어구 팝업
                if (tempItemList3[i].type == "Armor" || tempItemList3[i].type == "Helmet" || tempItemList3[i].type == "Gloves" || tempItemList3[i].type == "Pants" || tempItemList3[i].type == "Boots")
                {
                    int index = i;
                    Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        EquipInfoPopup(tempItemList3[index]);
                        EquipItemInfoPopup.SetActive(true);
                    });
                }
            }
        }
        //재료
        List<Things> tempItemList4 = new List<Things>();
        tempItemList4 = thingsData.getThingsList().FindAll(x => x.type == "Book" || x.type == "Ore");
        for (int i = 0; i < tempItemList4.Count; i++)
        {
            if (tempItemList4[i].possession > 0)
            {
                Tap4Items.Add(tempItemList4[i]);
                Tap4Slots.Add(Instantiate(InventorySlot)); // 아이템 생성
                Tap4Slots[Tap4Slots.Count - 1].SetActive(true);
                Tap4Slots[Tap4Slots.Count - 1].GetComponent<Slot>().id = Tap4Slots.Count - 1;
                Tap4Slots[Tap4Slots.Count - 1].transform.SetParent(Tap4Panel.transform);
                Tap4Slots[Tap4Slots.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
                Tap4Slots[Tap4Slots.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.one;
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(tempItemList4[i].icon);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList4[i].possession.ToString();
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList4[i].recent);

                //재료 팝업
                if (tempItemList4[i].type == "Book" || tempItemList4[i].type == "Ore")
                {
                    int index = i;
                    Tap4Slots[Tap4Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OthersItemInfoPopup(tempItemList4[index]);
                        ItemInfoPopup.SetActive(true);
                    });
                }
            }
        }
        //기타
        List<Things> tempItemList5 = new List<Things>();
        tempItemList5 = thingsData.getThingsList().FindAll(x => x.type == "Others");
        for (int i = 0; i < tempItemList5.Count; i++)
        {
            if (tempItemList5[i].possession > 0)
            {
                Tap5Items.Add(tempItemList5[i]);
                Tap5Slots.Add(Instantiate(InventorySlot)); // 아이템 생성
                Tap5Slots[Tap5Slots.Count - 1].SetActive(true);
                Tap5Slots[Tap5Slots.Count - 1].GetComponent<Slot>().id = Tap5Slots.Count - 1;
                Tap5Slots[Tap5Slots.Count - 1].transform.SetParent(Tap5Panel.transform);
                Tap5Slots[Tap5Slots.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
                Tap5Slots[Tap5Slots.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.one;
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(tempItemList5[i].icon);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList5[i].possession.ToString();
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList5[i].recent);

                if (tempItemList5[i].type == "Others")
                {
                    int index = i;
                    Tap5Slots[Tap5Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OthersItemInfoPopup(tempItemList5[index]);
                        ItemInfoPopup.SetActive(true);
                    });
                }
            }
        }
        //재료, 기타 아이템
        //List<Things> tempItemList6 = new List<Things>();
        //tempItemList6 = thingsData.getThingsList().FindAll(x => (x.type == "Ore" || x.type == "Others") );
        //for (int i = 0; i < tempItemList6.Count; i++)
        //{
        //    if (tempItemList6[i].possession > 0)
        //    {
        //        Tap6Items.Add(tempItemList6[i]);
        //        Tap6Slots.Add(Instantiate(InventorySlot)); //인벤토리 슬롯 생성
        //        Tap6Slots[Tap6Slots.Count - 1].SetActive(true);
        //        Tap6Slots[Tap6Slots.Count - 1].GetComponent<Slot>().id = Tap6Slots.Count - 1;
        //        Tap6Slots[Tap6Slots.Count - 1].transform.SetParent(Tap6Panel.transform);
        //        Tap6Slots[Tap6Slots.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
        //        Tap6Slots[Tap6Slots.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.one;
        //        Tap6Slots[Tap6Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(tempItemList6[i].icon);
        //        Tap6Slots[Tap6Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList6[i].possession.ToString();
        //        Tap6Slots[Tap6Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList6[i].recent);

        //        //광석 팝업
        //        if (tempItemList6[i].type == "Ore")
        //        {
        //            int index = i;
        //            Tap6Slots[Tap6Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
        //            {
        //                GameObject.Find("PlayerData").GetComponent<OreSelect>().ClickInventory(tempItemList6[index]);
        //                GameObject.Find("System").transform.Find("OreInfoPopup").gameObject.SetActive(true);
        //            });
        //        }
        //        //기타 아이템 팝업
        //        if (tempItemList6[i].type == "Others")
        //        {
        //            int index = i;
        //            Tap6Slots[Tap6Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
        //            {
        //                OthersItemInfoPopup(tempItemList6[index]);
        //                ItemInfoPopup.SetActive(true);
        //            });
        //        }

        //    }
        //}

        //new icon 감추기
        for(int i=0;i< thingsData.getThingsList().Count; i++)
        {
            if (thingsData.getThingsList()[i].recent) thingsData.getThingsList()[i].recent = false;
        }


    } //아이템 슬롯 생성

    public void Button() //아이템 테스트버튼 -> 아이템 생성 아이템 넘버 탭 랜덤
    {
        AddItem(Random.Range(0, 4), Random.Range(0, 9));
    }

    //아이템 생성
    public void AddItem(int tapNo, int id) 
    {
        List<Things> item = new List<Things>();
        List<GameObject> slot = new List<GameObject>();

        if (tapNo == 0)
        {
            item = Tap1Items;
            slot = Tap1Slots;
        }
        else if (tapNo == 1)
        {
            item = Tap2Items;
            slot = Tap2Slots;
        }
        else if (tapNo == 2)
        {
            item = Tap3Items;
            slot = Tap3Slots;
        }
        else if (tapNo == 3)
        {
            item = Tap4Items;
            slot = Tap4Slots;
        }
        else if (tapNo == 4)
        {
            item = Tap5Items;
            slot = Tap5Slots;
        }
        //else if (tapNo == 5)
        //{
        //    item = Tap6Items;
        //    slot = Tap6Slots;
        //}
        //AddItem(item, slot, tapNo, id);
    }

    void AddItem(List<Things> TempItems, List<GameObject> TempSlots, int tapNo, int id) //탭 넘버와 아이디 값을 가져와서 특정 탭에 아이템을 생성
    {
        Things itemToAdd = thingsData.FetchItemByID(id);
        if (CheckIfItemIsInInventory(tapNo, itemToAdd))
        {
            for (int i = 0; i < TempItems.Count; i++)
            {
                if (TempItems[i].item_no == id)
                {

                    ItemData data = TempSlots[i].transform.GetChild(3).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = data.amount.ToString();
                    //  ItemSprite(TempItems[i].sprite);
                    break;
                }
            }

        }
        else
        {
            for (int i = 0; i < TempItems.Count; i++)
            {
                if (TempItems[i].item_no == -1)
                {
                    TempItems[i] = itemToAdd;
                    GameObject itemObj = Instantiate(InventoryItem);
                    itemObj.SetActive(true);
                    //itemObj.GetComponent<ThingsData>().item = itemToAdd;
                    //itemObj.GetComponent<ThingsData>().amount = 1;
                    //itemObj.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = itemObj.GetComponent<ThingsData>().amount.ToString();
                    //itemObj.GetComponent<ThingsData>().slot = i;
                    //itemObj.transform.SetParent(TempSlots[i].transform);
                    //itemObj.transform.position = Vector2.zero;
                    //itemObj.GetComponent<Image>().sprite = itemToAdd.sprite;
                    //GameObject image = itemObj.transform.FindChild("Image").gameObject;
                    // image.SetActive(false);
                    // image.GetComponent<Image>().sprite = itemToAdd.sprite;

                    //if (itemObj.GetComponent<ItemData>().item.Possession == true)
                    //{
                    //    itemObj.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                    //}
                    //else
                    //{
                    //    itemObj.GetComponent<Image>().color = new Color(0, 0, 0, 255);
                    //}
                    itemObj.name = itemToAdd.name;
                    itemObj.GetComponent<RectTransform>().transform.localScale = Vector3.one;
                    itemObj.GetComponent<Button>().onClick.AddListener(() => Activate(tapNo, i));
                    //    ItemSprite(items[i].sprite);
                    itemObj.transform.localPosition = Vector3.one;
                    break;
                }
            }
        }
    } //아이템 생성 -> 아이템을 특정탭에 생성

    //스크롤 패널 변경
    public void SwitchScrollPanel()//GameObject obj)
    {
        if (Tap1Panel.activeSelf == true)
        {
            Tap1Push.SetActive(true);
            Tap2Push.SetActive(false);
            Tap3Push.SetActive(false);
            Tap4Push.SetActive(false);
            Tap5Push.SetActive(false);
            //Tap6Push.SetActive(false);

            Tap1Panel.SetActive(true);
            Tap2Panel.SetActive(false);
            Tap3Panel.SetActive(false);
            Tap4Panel.SetActive(false);
            Tap5Panel.SetActive(false);
            //Tap6Panel.SetActive(false);

            SlotSize(Tap1Items);
            t_SlotTitleText.text = "전체";

            //Tap1Panel.transform.localPosition = Vector3.one;
            BackSlot.GetComponent<ScrollRect>().content = Tap1Panel.GetComponent<RectTransform>();
        }
        else if (Tap2Panel.activeSelf == true)
        {
            Tap1Push.SetActive(false);
            Tap2Push.SetActive(true);
            Tap3Push.SetActive(false);
            Tap4Push.SetActive(false);
            Tap5Push.SetActive(false);
            //Tap6Push.SetActive(false);

            //SlotSize(Tap2Items);
            t_SlotTitleText.text = "무기";

            //Tap2Panel.transform.localPosition = Vector3.one;
            BackSlot.GetComponent<ScrollRect>().content = Tap2Panel.GetComponent<RectTransform>();

        }
        else if (Tap3Panel.activeSelf == true)
        {
            Tap1Push.SetActive(false);
            Tap2Push.SetActive(false);
            Tap3Push.SetActive(true);
            Tap4Push.SetActive(false);
            Tap5Push.SetActive(false);
            //Tap6Push.SetActive(false);

           // SlotSize(Tap3Items);
            t_SlotTitleText.text = "방어구";

            //Tap3Panel.transform.localPosition = Vector3.one;
            BackSlot.GetComponent<ScrollRect>().content = Tap3Panel.GetComponent<RectTransform>();

        }
        else if (Tap4Panel.activeSelf == true)
        {
            Tap1Push.SetActive(false);
            Tap2Push.SetActive(false);
            Tap3Push.SetActive(false);
            Tap4Push.SetActive(true);
            Tap5Push.SetActive(false);
            //Tap6Push.SetActive(false);

            //SlotSize(Tap4Items);
            t_SlotTitleText.text = "재료";

            //Tap4Panel.transform.localPosition = Vector3.one;
            BackSlot.GetComponent<ScrollRect>().content = Tap4Panel.GetComponent<RectTransform>();

        }
        else if (Tap5Panel.activeSelf == true)
        {
            Tap1Push.SetActive(false);
            Tap2Push.SetActive(false);
            Tap3Push.SetActive(false);
            Tap4Push.SetActive(false);
            Tap5Push.SetActive(true);
            //Tap6Push.SetActive(false);

            //SlotSize(Tap5Items);
            t_SlotTitleText.text = "기타";

            //Tap5Panel.transform.localPosition = Vector3.one;
            BackSlot.GetComponent<ScrollRect>().content = Tap5Panel.GetComponent<RectTransform>();

        }
        //else if (Tap6Panel.activeSelf == true)
        //{
        //    Tap1Push.SetActive(false);
        //    Tap2Push.SetActive(false);
        //    Tap3Push.SetActive(false);
        //    Tap4Push.SetActive(false);
        //    Tap5Push.SetActive(false);
        //    Tap6Push.SetActive(true);

        //    //SlotSize(Tap6Items);
        //    t_SlotTitleText.text = "재료, 기타 아이템";

        //    //Tap6Panel.transform.localPosition = Vector3.one;
        //    BackSlot.GetComponent<ScrollRect>().content = Tap6Panel.GetComponent<RectTransform>();

        //}
    } //아이템 탭을 선택할경우 스크롤 변경


    public void SlotSize(List<Things> items)
    {
        int SlotTextSize = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item_no != -1)
            {
                SlotTextSize++;
            }
        }
        t_SlotText.text = SlotTextSize.ToString() + "/ 50";// + items.Count;
    }

    //버튼 설정
    void TapButtonSetup()
    {
        Tap1Button.GetComponent<Button>().onClick.AddListener(() => TapButtonSetup(1));
        Tap2Button.GetComponent<Button>().onClick.AddListener(() => TapButtonSetup(2));
        Tap3Button.GetComponent<Button>().onClick.AddListener(() => TapButtonSetup(3));
        Tap4Button.GetComponent<Button>().onClick.AddListener(() => TapButtonSetup(4));
        Tap5Button.GetComponent<Button>().onClick.AddListener(() => TapButtonSetup(5));
        //Tap6Button.GetComponent<Button>().onClick.AddListener(() => TapButtonSetup(6));
    }
    //버튼 이벤트 추가
    void TapButtonSetup(int TapNum)
    {
        if (TapNum == 1)
        {
            Tap1Panel.SetActive(true);
            Tap2Panel.SetActive(false);
            Tap3Panel.SetActive(false);
            Tap4Panel.SetActive(false);
            Tap5Panel.SetActive(false);
            //Tap6Panel.SetActive(false);
        }
        else if (TapNum == 2)
        {
            Tap1Panel.SetActive(false);
            Tap2Panel.SetActive(true);
            Tap3Panel.SetActive(false);
            Tap4Panel.SetActive(false);
            Tap5Panel.SetActive(false);
            //Tap6Panel.SetActive(false);
        }
        else if (TapNum == 3)
        {
            Tap1Panel.SetActive(false);
            Tap2Panel.SetActive(false);
            Tap3Panel.SetActive(true);
            Tap4Panel.SetActive(false);
            Tap5Panel.SetActive(false);
            //Tap6Panel.SetActive(false);
        }
        else if (TapNum == 4)
        {
            Tap1Panel.SetActive(false);
            Tap2Panel.SetActive(false);
            Tap3Panel.SetActive(false);
            Tap4Panel.SetActive(true);
            Tap5Panel.SetActive(false);
            //Tap6Panel.SetActive(false);
        }
        else if (TapNum == 5)
        {
            Tap1Panel.SetActive(false);
            Tap2Panel.SetActive(false);
            Tap3Panel.SetActive(false);
            Tap4Panel.SetActive(false);
            Tap5Panel.SetActive(true);
            //Tap6Panel.SetActive(false);
        }
        else if (TapNum == 6)
        {
            Tap1Panel.SetActive(false);
            Tap2Panel.SetActive(false);
            Tap3Panel.SetActive(false);
            Tap4Panel.SetActive(false);
            Tap5Panel.SetActive(false);
            //Tap6Panel.SetActive(true);
        }
        SwitchScrollPanel();
    }

    //무기 정보 팝업
    public void EquipInfoPopup(Things things)
    {
        Equipment equip = equipmentData.getEquipmentList().Find(x => x.name == things.name);
        Debug.Log(things.name);
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/NameBox/ItemNameText").gameObject.GetComponent<Text>().text = equip.name;
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/ItemInfoText").gameObject.GetComponent<Text>().text = equip.explanation;
        string nesMtr = "";
        for(int i = 0; i < equip.necessaryMaterials.Length; i++)
        {
            nesMtr += equip.necessaryMaterials[i] + " " + equip.necessaryMaterialsNum[i];
            if (i < equip.necessaryMaterials.Length - 1) nesMtr += "\n";
        }
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text = nesMtr;
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = equip.time.ToString();
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/AttributeText").gameObject.GetComponent<Text>().text = equip.attribute;
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/SkillBox/Text").gameObject.GetComponent<Text>().text = "스킬 : " + equip.skill;
        EquipItemInfoPopup.transform.Find("UIPanel/ItemBox/Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(things.icon);

        string abstr = "";
        if (equip.stat.dps > 0) abstr += "전투력 " + equip.stat.dps + "\n";
        if (equip.stat.strPower > 0) abstr += "공격력 " + equip.stat.strPower + "\n";
        if (equip.stat.attackSpeed > 0) abstr += "공격속도 " + equip.stat.attackSpeed + "\n";
        if (equip.stat.focus > 0) abstr += "명중률 " + equip.stat.focus + "\n";
        if (equip.stat.critical > 0) abstr += "크리티컬 " + equip.stat.critical + "\n";
        if (equip.stat.defPower > 0) abstr += "방어력 " + equip.stat.defPower + "\n";
        if (equip.stat.evaRate > 0) abstr += "회피율 " + equip.stat.evaRate + "\n";
        abstr += "속성 " + equip.attribute;
        //if (equip.stat.collectSpeed > 0) abstr += "채집속도 " + equip.stat.collectSpeed + "\n";
        //if (equip.stat.collectAmount > 0) abstr += "채집량 " + equip.stat.collectAmount;
        //되도록이면 최대 5개까지만..
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/AbilityText").gameObject.GetComponent<Text>().text = abstr;

    }



    //기타 아이템 정보 팝업
    public void OthersItemInfoPopup(Things things)
    {
        GameObject.Find("System").transform.Find("ItemInfoPopup/UIPanel/InfoBox/ItemNameText").gameObject.GetComponent<Text>().text = things.name;
        GameObject.Find("System").transform.Find("ItemInfoPopup/UIPanel/InfoBox/ItemInfoText").gameObject.GetComponent<Text>().text = things.explanation;
        GameObject.Find("System").transform.Find("ItemInfoPopup/UIPanel/ItemBox/HaveText").gameObject.GetComponent<Text>().text = things.possession.ToString();
        GameObject.Find("System").transform.Find("ItemInfoPopup/UIPanel/ItemBox/Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(things.icon);

    }






}