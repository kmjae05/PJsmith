﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


//아이템 생성자, 중복아이템의 경우 카운트
public class Inventory : MonoBehaviour
{
    public GameObject InventorySlot;        //복사할 아이템 객체
    private GameObject InventoryItem;
    private SellManager sellManager;

    //private int slotAmount; //슬롯개수 -> 향후 탭별로 갯수에 맞도록 관리
    //ItemDatabase database;
    ThingsData thingsData;
    EquipmentData equipmentData;

    private List<InventoryThings> Tap1Items = new List<InventoryThings>(); //각 아이템별 리스트
    private List<GameObject> Tap1Slots = new List<GameObject>();

    private List<InventoryThings> Tap2Items = new List<InventoryThings>();
    private List<GameObject> Tap2Slots = new List<GameObject>();

    private List<InventoryThings> Tap3Items = new List<InventoryThings>();
    private List<GameObject> Tap3Slots = new List<GameObject>();

    private List<InventoryThings> Tap4Items = new List<InventoryThings>();
    private List<GameObject> Tap4Slots = new List<GameObject>();

    private List<InventoryThings> Tap5Items = new List<InventoryThings>();
    private List<GameObject> Tap5Slots = new List<GameObject>();

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


    private GameObject ItemImage; //아이템생성완료창 스프라이트
    private Text ItemText; //아이템 생성완료창 텍스트

    GameObject NewItemIcon; //new아이콘
    int NewItemCount = 0; //새로운 아이템 카운트 new아이콘 활성화

    private GameObject SystemPopup;
    private Text Sys_TitleText;
    private Text Sys_InfoText;
    private Button Sys_YesButton;
    private Button Sys_NoButton;
    private Button Sys_OkButton;


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


        ItemInfoPopup = GameObject.Find("System").transform.Find("ItemInfoPopup").gameObject;
        EquipItemInfoPopup = GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject;
        //NewItemIcon = transform.Find("/02_UI/Main/Button/CollectionButton/Icon").gameObject;

        t_SlotText = gameObject.transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/SlotPlusBox/SlotText").gameObject.GetComponent<Text>();
        t_SlotTitleText = gameObject.transform.Find("/02_UI/Main/Menu/InventoryPopup/UIPanel/BoxBack/Text").gameObject.GetComponent<Text>();
        //InventorySlot.SetActive(true);
        CollectionPopup.SetActive(true);

        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        Sys_TitleText = SystemPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>();
        Sys_InfoText = SystemPopup.transform.Find("UIPanel/InfoText").gameObject.GetComponent<Text>();
        Sys_YesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        Sys_NoButton = SystemPopup.transform.Find("UIPanel/NoButton").gameObject.GetComponent<Button>();
        Sys_OkButton = SystemPopup.transform.Find("UIPanel/OKButton").gameObject.GetComponent<Button>();

    }
    void Start()
    {
        thingsData = GameObject.Find("ThingsData").GetComponent<ThingsData>();
        sellManager = GameObject.Find("InventoryScript").GetComponent<SellManager>();

        ItemSlotCreate();
        TapButtonSetup();
        CollectionPopup.SetActive(false);
        equipmentData = GameObject.Find("ThingsData").GetComponent<EquipmentData>();

        GameObject.Find("InvenButton").GetComponent<Button>().onClick.AddListener(() => {
            Tap1Panel.SetActive(true); Tap1Push.SetActive(true); SwitchScrollPanel();
        });
    }

    void Update()
    {
        if (ItemInfoPopup.transform.Find("UIPanel/Get").gameObject.activeInHierarchy)
            ItemInfoPopup.transform.Find("UIPanel/Get/LightImage").gameObject.transform.Rotate(new Vector3(0, 0, 1), 1 * 0.5f);
        
    }

    void ItemSprite(Sprite sprite) //아이템 생성 완료창 스프라이트 넣음
    {
        ItemImage.GetComponent<Image>().sprite = sprite;
    }

    public void ItemSlotCreate()
    {
        //스크롤 위치 초기화
        Tap1Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(Tap1Panel.GetComponent<RectTransform>().anchoredPosition.x, 0);

        for (int i = 0; i < Tap1Slots.Count; i++) Destroy(Tap1Slots[i]);
        for (int i = 0; i < Tap2Slots.Count; i++) Destroy(Tap2Slots[i]);
        for (int i = 0; i < Tap3Slots.Count; i++) Destroy(Tap3Slots[i]);
        for (int i = 0; i < Tap4Slots.Count; i++) Destroy(Tap4Slots[i]);
        for (int i = 0; i < Tap5Slots.Count; i++) Destroy(Tap5Slots[i]);
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
        //List<Things> tempItemList1 = new List<Things>();
        List<InventoryThings> tempItemList1 = new List<InventoryThings>();
        for (int i=0;i< thingsData.getInventoryThingsList().Count; i++) tempItemList1.Add(thingsData.getInventoryThingsList()[i]);
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

                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList1[i].name).grade);
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;
                
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite 
                    = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList1[i].name).icon);

                //장비 장착 표시
                if (tempItemList1[i].equip) Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/EquipImage").gameObject.SetActive(true);
                else Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/EquipImage").gameObject.SetActive(false);

                //장비일 경우 수량 대신 강화 수치 표시
                if (tempItemList1[i].type == "Helmet" || tempItemList1[i].type == "Armor" || tempItemList1[i].type == "Gloves" || tempItemList1[i].type == "Pants"
                    || tempItemList1[i].type == "Weapon" || tempItemList1[i].type == "Boots")
                {
                    //강화 수치 있는 경우
                    if (tempItemList1[i].reinforcement > 0)
                    {
                        Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = "+" + tempItemList1[i].reinforcement.ToString();
                        Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(true);
                    }
                    //강화 수치 없는 경우
                    else Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(false);
                }
                //그 외 아이템
                else
                {
                    Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList1[i].possession.ToString();
                    Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(true);
                }
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList1[i].recent);


                //판매 아이템 창 열려있을 때
                if (GameObject.Find("System").transform.Find("FestivalItem").gameObject.activeInHierarchy)
                {
                    InventoryThings inven = tempItemList1[i];
                    Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.RemoveAllListeners();
                    Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject.Find("InventoryScript").GetComponent<FestivalManager>().selectItem(inven);
                    });
                }
                else
                {
                    //기본 인벤토리 창 모드
                    //광석 팝업 
                    if (tempItemList1[i].type == "Ore")
                    {
                        int index = i;
                        Tap1Slots[Tap1Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                        {
                            GameObject.Find("PlayerManager").GetComponent<OreSelect>().ClickInventory(tempItemList1[index]);
                            GameObject.Find("System").transform.Find("OreInfoPopup").gameObject.SetActive(true);
                        });
                    }
                    //장비 팝업
                    else if (tempItemList1[i].type == "Helmet" || tempItemList1[i].type == "Armor" || tempItemList1[i].type == "Gloves" || tempItemList1[i].type == "Pants"
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
                    else if (tempItemList1[i].type == "Others" || tempItemList1[i].type == "Material" || tempItemList1[i].type == "Book"
                        || tempItemList1[i].type == "Bookpiece")
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
        }
        //무기
        List<InventoryThings> tempItemList2 = new List<InventoryThings>();
        tempItemList2 = thingsData.getInventoryThingsList().FindAll(x => x.type == "Weapon");
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
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList2[i].name).grade);
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;

                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite 
                    = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList2[i].name).icon);

                //장비 장착 표시
                if (tempItemList2[i].equip) Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item/EquipImage").gameObject.SetActive(true);
                else Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item/EquipImage").gameObject.SetActive(false);

                //강화 수치 있는 경우
                if (tempItemList2[i].reinforcement > 0)
                {
                    Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = "+" + tempItemList2[i].reinforcement.ToString();
                    Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(true);
                }
                //강화 수치 없는 경우
                else Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(false);

                Tap2Slots[Tap2Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList2[i].recent);

                //판매 아이템 창 열려있을 때
                if (GameObject.Find("System").transform.Find("FestivalItem").gameObject.activeInHierarchy)
                {
                    InventoryThings inven = tempItemList2[i];
                    Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.RemoveAllListeners();
                    Tap2Slots[Tap2Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject.Find("InventoryScript").GetComponent<FestivalManager>().selectItem(inven);
                    });
                }

                //무기 팝업
                else if (tempItemList2[i].type == "Weapon")
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
        List<InventoryThings> tempItemList3 = new List<InventoryThings>();
        tempItemList3 = thingsData.getInventoryThingsList().FindAll(x => x.type == "Armor" || x.type == "Helmet" || x.type == "Gloves" || x.type == "Pants"|| x.type == "Boots");
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
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList3[i].name).grade);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;

                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite
                    = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList3[i].name).icon);

                //장비 장착 표시
                if (tempItemList3[i].equip) Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item/EquipImage").gameObject.SetActive(true);
                else Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item/EquipImage").gameObject.SetActive(false);

                //강화 수치 있는 경우
                if (tempItemList3[i].reinforcement > 0)
                {
                    Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = "+" + tempItemList3[i].reinforcement.ToString();
                    Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(true);
                }
                //강화 수치 없는 경우
                else Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(false);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList3[i].recent);


                //판매 아이템 창 열려있을 때
                if (GameObject.Find("System").transform.Find("FestivalItem").gameObject.activeInHierarchy)
                {
                    InventoryThings inven = tempItemList3[i];
                    Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.RemoveAllListeners();
                    Tap3Slots[Tap3Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject.Find("InventoryScript").GetComponent<FestivalManager>().selectItem(inven);
                    });
                }

                //방어구 팝업
                else if (tempItemList3[i].type == "Armor" || tempItemList3[i].type == "Helmet" || tempItemList3[i].type == "Gloves" || tempItemList3[i].type == "Pants" || tempItemList3[i].type == "Boots")
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
        List<InventoryThings> tempItemList4 = new List<InventoryThings>();
        tempItemList4 = thingsData.getInventoryThingsList().FindAll(x =>  x.type == "Ore" || x.type == "Material");
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
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList4[i].name).grade);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;

                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite 
                    = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList4[i].name).icon);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList4[i].possession.ToString();
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList4[i].recent);

                //판매 아이템 창 열려있을 때
                if (GameObject.Find("System").transform.Find("FestivalItem").gameObject.activeInHierarchy)
                {
                    InventoryThings inven = tempItemList4[i];
                    Tap4Slots[Tap4Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.RemoveAllListeners();
                    Tap4Slots[Tap4Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject.Find("InventoryScript").GetComponent<FestivalManager>().selectItem(inven);
                    });
                }

                //재료 팝업
                else if (tempItemList4[i].type == "Material" || tempItemList4[i].type == "Ore")
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
        List<InventoryThings> tempItemList5 = new List<InventoryThings>();
        tempItemList5 = thingsData.getInventoryThingsList().FindAll(x => x.type == "Others" || x.type == "Book" || x.type == "Bookpiece");
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
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList5[i].name).grade);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;

                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite 
                    = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == tempItemList5[i].name).icon);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = tempItemList5[i].possession.ToString();
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("NewIcon").gameObject.SetActive(tempItemList5[i].recent);


                //판매 아이템 창 열려있을 때
                if (GameObject.Find("System").transform.Find("FestivalItem").gameObject.activeInHierarchy)
                {
                    InventoryThings inven = tempItemList5[i];
                    Tap5Slots[Tap5Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.RemoveAllListeners();
                    Tap5Slots[Tap5Slots.Count - 1].transform.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject.Find("InventoryScript").GetComponent<FestivalManager>().selectItem(inven);
                    });
                }

                else if (tempItemList5[i].type == "Others" || tempItemList5[i].type == "Book" || tempItemList5[i].type == "Bookpiece")
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

        SlotSize(Tap1Items);
        //new icon 감추기
        for (int i=0;i< thingsData.getInventoryThingsList().Count; i++)
        {
            if (thingsData.getInventoryThingsList()[i].recent) thingsData.getInventoryThingsList()[i].recent = false;
        }


    } //아이템 슬롯 생성

    public void Button() //아이템 테스트버튼 -> 아이템 생성 아이템 넘버 탭 랜덤
    {
        AddItem(Random.Range(0, 4), Random.Range(0, 9));
    }

    //아이템 생성
    public void AddItem(int tapNo, int id) 
    {
        List<InventoryThings> item = new List<InventoryThings>();
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
    } //아이템 탭을 선택할경우 스크롤 변경


    public void SlotSize(List<InventoryThings> items)
    {
        int SlotTextSize = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (ThingsData.instance.getThingsList().Find(x=>x.name == items[i].name).item_no != -1)
            {
                SlotTextSize++;
            }
        }
        t_SlotText.text = SlotTextSize.ToString();// + "/ 50";// + items.Count;
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
    public void EquipInfoPopup(InventoryThings things)
    {
        GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ProductionButton").gameObject.SetActive(true);
        Equipment equip = equipmentData.getEquipmentList().Find(x => x.name == things.name);
        
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
        //EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/AttributeText").gameObject.GetComponent<Text>().text = equip.attribute;
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/SkillBox/Text").gameObject.GetComponent<Text>().text = "스킬 : " + equip.skill;

        Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == things.name).grade);
        EquipItemInfoPopup.transform.Find("UIPanel/ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = col;
        EquipItemInfoPopup.transform.Find("UIPanel/ItemBox/Icon").gameObject.GetComponent<Image>().sprite 
            = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == things.name).icon);

        //강화 표시
        if (things.reinforcement > 0)
        {
            EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/ReinText").gameObject.SetActive(true);
            EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/ReinText").gameObject.GetComponent<Text>().text = "+" + things.reinforcement;
        }
        else EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/ReinText").gameObject.SetActive(false);
        //강화 버튼
        if (things.reinforcement < 10)
        {
            EquipItemInfoPopup.transform.Find("UIPanel/ReinforceButton").gameObject.SetActive(true);
            EquipItemInfoPopup.transform.Find("UIPanel/ReinforceButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            EquipItemInfoPopup.transform.Find("UIPanel/ReinforceButton").gameObject.GetComponent<Button>().onClick.AddListener(
                () => { GameObject.Find("PlayerManager").GetComponent<EquipReinforceManager>().ReinforceEquip(things); });
        }
        else EquipItemInfoPopup.transform.Find("UIPanel/ReinforceButton").gameObject.SetActive(false);

        string abstr = "";
        if (things.stat.dps > 0) abstr += "전투력 " + (int)things.stat.dps + "\n";
        if (things.stat.strPower > 0) abstr += "공격력 " + things.stat.strPower + "\n";

        if (things.stat.attackSpeed > 0) { 
            string result = string.Format("{0:#.##}", things.stat.attackSpeed);
            abstr += "공격속도 " + result + "\n";
        }
        if (things.stat.focus > 0) abstr += "명중률 " + things.stat.focus + "\n";
        if (things.stat.critical > 0) abstr += "크리티컬 " + things.stat.critical + "\n";
        if (things.stat.defPower > 0) abstr += "방어력 " + things.stat.defPower + "\n";
        if (things.stat.evaRate > 0) abstr += "회피율 " + things.stat.evaRate + "\n";
        abstr += "속성 " + things.attribute;
        //if (equip.stat.collectSpeed > 0) abstr += "채집속도 " + equip.stat.collectSpeed + "\n";
        //if (equip.stat.collectAmount > 0) abstr += "채집량 " + equip.stat.collectAmount;
        //되도록이면 최대 5개까지만..
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/AbilityText").gameObject.GetComponent<Text>().text = abstr;

        //장비 중
        if (things.equip)
        {
            EquipItemInfoPopup.transform.Find("UIPanel/SellButton").gameObject.SetActive(false);
            if (GameObject.Find("Menu").transform.Find("ProfilePopup").gameObject.activeInHierarchy)
                EquipItemInfoPopup.transform.Find("UIPanel/ChangeButton").gameObject.SetActive(true);
            else EquipItemInfoPopup.transform.Find("UIPanel/ChangeButton").gameObject.SetActive(false);
            EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/EquipText").gameObject.SetActive(true);
            EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/EquipText").gameObject.GetComponent<Text>().text = things.equipChrName + " 세트" + things.equipSetNum.ToString() + "에 장비 중";
        }
        else
        {
            EquipItemInfoPopup.transform.Find("UIPanel/SellButton").gameObject.SetActive(true);
            EquipItemInfoPopup.transform.Find("UIPanel/ChangeButton").gameObject.SetActive(false);
            EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/EquipText").gameObject.SetActive(false);
            //시스템 팝업으로 판매
            EquipItemInfoPopup.transform.Find("UIPanel/SellButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            EquipItemInfoPopup.transform.Find("UIPanel/SellButton").gameObject.GetComponent<Button>().onClick.AddListener(() => sellManager.OpenEquipSellPopup(things));
        }

        EquipItemInfoPopup.transform.Find("UIPanel/ProductionButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        EquipItemInfoPopup.transform.Find("UIPanel/ProductionButton").gameObject.GetComponent<Button>().onClick.AddListener(()
          =>{

              bool flag = true;
              if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == equip.necessaryMaterials[0]) != null)
              {
                  if (equip.necessaryMaterialsNum[0] <= ThingsData.instance.getInventoryThingsList().Find(x => x.name == equip.necessaryMaterials[0]).possession)
                  {
                      //조건 만족
                      Debug.Log("y");
                  }
                  else
                  {
                      flag = false;
                      //재료 수량 부족
                      Debug.Log("no");
                  }
              }
              else
              {
                  flag = false;
                  //재료 수량 부족
                  Debug.Log("null");
              }
              if (!flag)
              {
                  //팝업
                  SystemPopup.SetActive(true);
                  SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "재료 부족";
                  SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "제작에 필요한 재료가 부족합니다.";
                  int mate = 0;
                  if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == equip.necessaryMaterials[0]) != null)
                      mate = equip.necessaryMaterialsNum[0] - ThingsData.instance.getInventoryThingsList().Find(x => x.name == equip.necessaryMaterials[0]).possession;
                  else
                      mate = equip.necessaryMaterialsNum[0];
                  SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text += "\n필요한 재료 : " + equip.necessaryMaterials[0] + " " + mate + "개";
                  Sys_YesButton.gameObject.SetActive(false);
                  Sys_NoButton.gameObject.SetActive(false);
                  Sys_OkButton.gameObject.SetActive(true);

                  return;
              }

              SystemPopup.SetActive(true);

              Sys_TitleText.GetComponent<Text>().text = things.name + " 제작";
              Sys_InfoText.GetComponent<Text>().text = things.name + " 장비를 제작하시겠습니까 ?";

              Sys_YesButton.gameObject.SetActive(true);     //예/아니오 버튼으로 수정
              Sys_NoButton.gameObject.SetActive(true);
              Sys_OkButton.gameObject.SetActive(false);


              Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
              Sys_YesButton.GetComponent<Button>().onClick.AddListener(()
                  => {
                      Player.instance.getUser().isOre = false;

                      //재료 감소
                      ThingsData.instance.getInventoryThingsList().Find(x => x.name == equip.necessaryMaterials[0]).possession
                        -= equip.necessaryMaterialsNum[0];

                      Player.instance.getUser().equipName = things.name;
                      Player.instance.getUser().equipmaxhp = 800;
                      Player.instance.getUser().equiphp = 800;
                      Player.instance.getUser().equiptime = 30;
                      Player.instance.getUser().equipexp = 10;

                      SystemPopup.SetActive(false);
                      StartCoroutine(FadeOut());
                  });    

          });

    }

    IEnumerator FadeOut()
    {
        Image FadeImage = GameObject.Find("FadeCanvas").transform.Find("FadeImage").GetComponent<Image>();
        FadeImage.gameObject.SetActive(true);
        for (float fade = 0; fade <= 1.0f; fade += 0.02f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        SceneManager.LoadScene("08_Loading_GameIn");
    }

    //기타 아이템 정보 팝업
    public void OthersItemInfoPopup(InventoryThings things)
    {
        ItemInfoPopup.transform.Find("UIPanel/InfoBox/ItemNameText").gameObject.GetComponent<Text>().text = things.name;
        ItemInfoPopup.transform.Find("UIPanel/InfoBox/ItemInfoText").gameObject.GetComponent<Text>().text 
            = ThingsData.instance.getThingsList().Find(x => x.name == things.name).explanation;
        ItemInfoPopup.transform.Find("UIPanel/ItemBox/HaveText").gameObject.GetComponent<Text>().text = things.possession.ToString();
        Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == things.name).grade);
        ItemInfoPopup.transform.Find("UIPanel/ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = col;

        ItemInfoPopup.transform.Find("UIPanel/ItemBox/Icon").gameObject.GetComponent<Image>().sprite
            = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == things.name).icon);

        ItemInfoPopup.transform.Find("UIPanel/SellButton").gameObject.SetActive(true);
        ItemInfoPopup.transform.Find("UIPanel/SellButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        ItemInfoPopup.transform.Find("UIPanel/SellButton").gameObject.GetComponent<Button>().onClick.AddListener(() => sellManager.OpenSellPopup(things));

        ItemInfoPopup.transform.Find("UIPanel/UseButton").gameObject.SetActive(false);

        //제련북 버튼    //하나씩
        if (things.type == "Book")
        {
            //사용하기 버튼
            ItemInfoPopup.transform.Find("UIPanel/UseButton").gameObject.SetActive(true);
            ItemInfoPopup.transform.Find("UIPanel/UseButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            ItemInfoPopup.transform.Find("UIPanel/UseButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
                
                things.possession -= 1;
                ItemInfoPopup.transform.Find("UIPanel/ItemBox/HaveText").gameObject.GetComponent<Text>().text = things.possession.ToString();
                if (GameObject.Find("Menu").transform.Find("InventoryPopup").gameObject.activeInHierarchy)
                {
                    GameObject.Find("InventoryScript").GetComponent<Inventory>().ItemSlotCreate();
                }

                Color colr = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == things.name).grade);
                ItemInfoPopup.transform.Find("UIPanel/UsePopup/ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = colr;
                ItemInfoPopup.transform.Find("UIPanel/UsePopup/ItemBox/Icon").gameObject.GetComponent<Image>().sprite =
                    Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == things.name).icon);
                ItemInfoPopup.transform.Find("UIPanel/UsePopup/AlrImage/Text").gameObject.GetComponent<Text>().text = things.name + " 읽는 중";
                ItemInfoPopup.transform.Find("UIPanel/UsePopup").gameObject.SetActive(true);

                ItemInfoPopup.transform.Find("UIPanel/UsePopup/Slider").gameObject.GetComponent<Slider>().value = 0;
                StartCoroutine(usePopupSlider(things));
            });

        }

        //제련북 조각 버튼     //여럿
        if (things.type == "Bookpiece")
        {
            ItemInfoPopup.transform.Find("UIPanel/UseButton").gameObject.SetActive(true);
            ItemInfoPopup.transform.Find("UIPanel/UseButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            ItemInfoPopup.transform.Find("UIPanel/UseButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {

                //개수 체크
                if (things.possession < 10)
                {
                    SystemPopup.SetActive(true);

                    Sys_TitleText.GetComponent<Text>().text = "개수 부족";
                    Sys_InfoText.GetComponent<Text>().text = "조각 개수가 부족합니다.\n(10개 이상 소지 시 사용 가능)";

                    Sys_YesButton.gameObject.SetActive(false);    
                    Sys_NoButton.gameObject.SetActive(false);
                    Sys_OkButton.gameObject.SetActive(true);
                }
                else
                {
                    things.possession -= 10;

                    //제작서 획득
                    string str = things.name.Substring(0, 8);
                    if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == str) != null)
                    {
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == str).possession
                            += 1;
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == str).recent = true;
                    }
                    else
                    {
                        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                            x => x.name == str).type, str, 1));
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == str).recent = true;
                    }
                    //정리
                    ItemInfoPopup.transform.Find("UIPanel/ItemBox/HaveText").gameObject.GetComponent<Text>().text = things.possession.ToString();
                    if (GameObject.Find("Menu").transform.Find("InventoryPopup").gameObject.activeInHierarchy)
                    {
                        GameObject.Find("InventoryScript").GetComponent<Inventory>().ItemSlotCreate();
                    }


                    GameObject.Find("System").transform.Find("AlertImage").gameObject.SetActive(false);
                    GameObject.Find("System").transform.Find("AlertImage/AlrImage/Text").gameObject.GetComponent<Text>().text = str + "조각을 제작서로 만들었습니다.";
                    StartCoroutine(alrImageActive());
                }
            });


        }

    }//문구 출력 애니메이션
    IEnumerator alrImageActive()
    {
        GameObject.Find("System").transform.Find("AlertImage").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator usePopupSlider(InventoryThings things)
    {
        ItemInfoPopup.transform.Find("UIPanel/UsePopup/Slider").gameObject.GetComponent<Slider>().value = 0;
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            ItemInfoPopup.transform.Find("UIPanel/UsePopup/Slider").gameObject.GetComponent<Slider>().value = time*50;
            if (ItemInfoPopup.transform.Find("UIPanel/UsePopup/Slider").gameObject.GetComponent<Slider>().value >=
                ItemInfoPopup.transform.Find("UIPanel/UsePopup/Slider").gameObject.GetComponent<Slider>().maxValue)
            {
                Debug.Log("dfs");
                ItemInfoPopup.transform.Find("UIPanel/UsePopup").gameObject.SetActive(false);


                //아이템 제작 방법 획득. (도감)
                List<Things> equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 1 && x.type == "Weapon");
                int random = Random.Range(0, equip.Count);
                if (things.name == "무기제작서-일반")
                {
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "무기제작서-고급")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 2 && x.type == "Weapon");
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "무기제작서-희귀")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 3 && x.type == "Weapon");
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "무기제작서-영웅")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 4 && x.type == "Weapon");
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "무기제작서-전설")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 5 && x.type == "Weapon");
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "방어제작서-일반")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 1 && (x.type == "Helmet" || x.type == "Armor" 
                    || x.type == "Gloves" || x.type == "Pants" || x.type == "Boots"));
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "방어제작서-고급")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 2 && (x.type == "Helmet" || x.type == "Armor"
                    || x.type == "Gloves" || x.type == "Pants" || x.type == "Boots"));
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "방어제작서-희귀")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 3 && (x.type == "Helmet" || x.type == "Armor"
                    || x.type == "Gloves" || x.type == "Pants" || x.type == "Boots"));
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "방어제작서-영웅")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 4 && (x.type == "Helmet" || x.type == "Armor"
                    || x.type == "Gloves" || x.type == "Pants" || x.type == "Boots"));
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }
                else if (things.name == "방어제작서-전설")
                {
                    equip = ThingsData.instance.getThingsList().FindAll(x => x.grade == 5 && (x.type == "Helmet" || x.type == "Armor"
                    || x.type == "Gloves" || x.type == "Pants" || x.type == "Boots"));
                    random = Random.Range(0, equip.Count);
                    ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).illustrate = true;
                }


                Color colr = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).grade);
                ItemInfoPopup.transform.Find("UIPanel/Get/ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = colr;
                ItemInfoPopup.transform.Find("UIPanel/Get/ItemBox/Icon").gameObject.GetComponent<Image>().sprite =
                    Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equip[random].name).icon);
                ItemInfoPopup.transform.Find("UIPanel/Get/AlrImage/Text").gameObject.GetComponent<Text>().text = "\""+equip[random].name + "\" 제작방법을 획득했습니다.";
                ItemInfoPopup.transform.Find("UIPanel/Get").gameObject.SetActive(true);


                if (things.possession <= 0)
                {
                    ItemInfoPopup.transform.Find("UIPanel/Get/YesButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                    ItemInfoPopup.transform.Find("UIPanel/Get/YesButton").gameObject.GetComponent<Button>().onClick.AddListener(()=> {
                        ItemInfoPopup.SetActive(false);
                    });
                    
                }

                yield break;
            }

            yield return null;
        }
    }






}