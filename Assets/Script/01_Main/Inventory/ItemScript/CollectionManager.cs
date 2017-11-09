using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionManager : MonoBehaviour
{

    private GameObject collectionPopup;
    private GameObject collectionList;
    private GameObject alrImage;

    private GameObject panel1;
    private GameObject panel2;
    private GameObject panel3;
    private GameObject panel4;
    private GameObject panel5;
    private GameObject panel6;
    private GameObject panel7;

    private List<GameObject> Tap1Slots = new List<GameObject>();
    private List<GameObject> Tap2Slots = new List<GameObject>();
    private List<GameObject> Tap3Slots = new List<GameObject>();
    private List<GameObject> Tap4Slots = new List<GameObject>();
    private List<GameObject> Tap5Slots = new List<GameObject>();
    private List<GameObject> Tap6Slots = new List<GameObject>();
    private List<GameObject> Tap7Slots = new List<GameObject>();

    GameObject Tap1Button;
    GameObject Tap2Button;
    GameObject Tap3Button;
    GameObject Tap4Button;
    GameObject Tap5Button;
    GameObject Tap6Button;
    GameObject Tap7Button;


    private GameObject collectionBox;

    private List<Things> things;
    private List<InventoryThings> invenThings;


    GameObject EquipItemInfoPopup;
    EquipmentData equipmentData;

    void Start()
    {
        collectionPopup = GameObject.Find("System").transform.Find("Collection").gameObject;
        collectionList = collectionPopup.transform.Find("UIBox/CollectionList").gameObject;
        alrImage = collectionPopup.transform.Find("UIBox/AlrImage").gameObject;

        panel1 = collectionList.transform.Find("Panel1").gameObject;
        panel2 = collectionList.transform.Find("Panel2").gameObject;
        panel3 = collectionList.transform.Find("Panel3").gameObject;
        panel4 = collectionList.transform.Find("Panel4").gameObject;
        panel5 = collectionList.transform.Find("Panel5").gameObject;
        panel6 = collectionList.transform.Find("Panel6").gameObject;
        panel7 = collectionList.transform.Find("Panel7").gameObject;

        Tap1Button = collectionPopup.transform.Find("UIBox/Tab/Tab1").gameObject;
        Tap2Button = collectionPopup.transform.Find("UIBox/Tab/Tab2").gameObject;
        Tap3Button = collectionPopup.transform.Find("UIBox/Tab/Tab3").gameObject;
        Tap4Button = collectionPopup.transform.Find("UIBox/Tab/Tab4").gameObject;
        Tap5Button = collectionPopup.transform.Find("UIBox/Tab/Tab5").gameObject;
        Tap6Button = collectionPopup.transform.Find("UIBox/Tab/Tab6").gameObject;
        Tap7Button = collectionPopup.transform.Find("UIBox/Tab/Tab7").gameObject;


        collectionBox = panel1.transform.Find("CollectionBox").gameObject;

        things = ThingsData.instance.getThingsList();
        invenThings = ThingsData.instance.getInventoryThingsList();


        EquipItemInfoPopup = GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject;
        equipmentData = GameObject.Find("ThingsData").GetComponent<EquipmentData>();

        GameObject.Find("CollectButton").GetComponent<Button>().onClick.AddListener(() => {
            panel1.SetActive(true); SwitchScrollPanel();
        });
        CreateCollection();
    }


   public void CreateCollection()
    {
        for (int i = 0; i < Tap1Slots.Count; i++) Destroy(Tap1Slots[i]);
        for (int i = 0; i < Tap2Slots.Count; i++) Destroy(Tap2Slots[i]);
        for (int i = 0; i < Tap3Slots.Count; i++) Destroy(Tap3Slots[i]);
        for (int i = 0; i < Tap4Slots.Count; i++) Destroy(Tap4Slots[i]);
        for (int i = 0; i < Tap5Slots.Count; i++) Destroy(Tap5Slots[i]);
        for (int i = 0; i < Tap6Slots.Count; i++) Destroy(Tap6Slots[i]);
        for (int i = 0; i < Tap7Slots.Count; i++) Destroy(Tap7Slots[i]);

        Tap1Slots.Clear();
        Tap2Slots.Clear();
        Tap3Slots.Clear();
        Tap4Slots.Clear();
        Tap5Slots.Clear();
        Tap6Slots.Clear();
        Tap7Slots.Clear();

        if (panel1.transform.childCount > 1)
        {
            for (int i = 1; i < panel1.transform.childCount; i++)
            {
                Destroy(panel1.transform.GetChild(i).gameObject);
            }
        }

        //도감 목록 생성

        //Panel1
        List<Things> thn = things.FindAll(x => x.type == "Weapon" || x.type == "Helmet" || x.type == "Armor" || x.type == "Gloves" || x.type == "Pants" || x.type == "Boots");
        for (int i = 0; i < thn.Count; i++)
        {
            Tap1Slots.Add(Instantiate(collectionBox)); // 아이템 생성
            Tap1Slots[Tap1Slots.Count - 1].transform.SetParent(panel1.transform);
            Tap1Slots[Tap1Slots.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn[i].grade);
            Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(thn[i].icon);
            Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().text = thn[i].name;
            Tap1Slots[Tap1Slots.Count - 1].SetActive(true);

            //장비 획득 유무
            if (thn[i].illustrate)
            {
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn[i].grade);
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(1, 1, 1);
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(1, 1, 1);
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { EquipInfoPopup(thn[num]); });

            }
            else
            {
                Color frameColor = Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color;
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = new Color(frameColor.r / 2f, frameColor.g / 2f, frameColor.b / 2f);
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                //획득 못했다는 문구
                Tap1Slots[Tap1Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    alrImage.SetActive(false);
                    StartCoroutine(alrImageActive());
                });
            }
        }//1

        //Panel2
        List<Things> thn2 = things.FindAll(x => x.type == "Weapon");
        for (int i = 0; i < thn2.Count; i++)
        {
            Tap2Slots.Add(Instantiate(collectionBox)); // 아이템 생성
            Tap2Slots[Tap2Slots.Count - 1].transform.SetParent(panel2.transform);
            Tap2Slots[Tap2Slots.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn2[i].grade);
            Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(thn2[i].icon);
            Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().text = thn2[i].name;
            Tap2Slots[Tap2Slots.Count - 1].SetActive(true);

            //장비 획득 유무
            if (thn2[i].illustrate)
            {
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn2[i].grade);
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(1, 1, 1);
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(1, 1, 1);
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { EquipInfoPopup(thn2[num]); });

            }
            else
            {
                Color frameColor = Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color;
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = new Color(frameColor.r / 2f, frameColor.g / 2f, frameColor.b / 2f);
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                //획득 못했다는 문구
                Tap2Slots[Tap2Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    alrImage.SetActive(false);
                    StartCoroutine(alrImageActive());
                });
            }
        }//2

        //Panel3
        List<Things> thn3 = things.FindAll(x => x.type == "Helmet");
        for (int i = 0; i < thn3.Count; i++)
        {
            Tap3Slots.Add(Instantiate(collectionBox)); // 아이템 생성
            Tap3Slots[Tap3Slots.Count - 1].transform.SetParent(panel3.transform);
            Tap3Slots[Tap3Slots.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn3[i].grade);
            Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(thn3[i].icon);
            Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().text = thn3[i].name;
            Tap3Slots[Tap3Slots.Count - 1].SetActive(true);

            //장비 획득 유무
            if (thn3[i].illustrate)
            {
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn3[i].grade);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(1, 1, 1);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(1, 1, 1);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { EquipInfoPopup(thn3[num]); });

            }
            else
            {
                Color frameColor = Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color;
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = new Color(frameColor.r / 2f, frameColor.g / 2f, frameColor.b / 2f);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                //획득 못했다는 문구
                Tap3Slots[Tap3Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    alrImage.SetActive(false);
                    StartCoroutine(alrImageActive());
                });
            }
        }//3

        //Panel4
        List<Things> thn4 = things.FindAll(x => x.type == "Armor");
        for (int i = 0; i < thn4.Count; i++)
        {
            Tap4Slots.Add(Instantiate(collectionBox)); // 아이템 생성
            Tap4Slots[Tap4Slots.Count - 1].transform.SetParent(panel4.transform);
            Tap4Slots[Tap4Slots.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn4[i].grade);
            Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(thn4[i].icon);
            Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().text = thn4[i].name;
            Tap4Slots[Tap4Slots.Count - 1].SetActive(true);

            //장비 획득 유무
            if (thn4[i].illustrate)
            {
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn4[i].grade);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(1, 1, 1);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(1, 1, 1);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { EquipInfoPopup(thn4[num]); });

            }
            else
            {
                Color frameColor = Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color;
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = new Color(frameColor.r / 2f, frameColor.g / 2f, frameColor.b / 2f);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                //획득 못했다는 문구
                Tap4Slots[Tap4Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    alrImage.SetActive(false);
                    StartCoroutine(alrImageActive());
                });
            }
        }//4


        //Panel5
        List<Things> thn5 = things.FindAll(x => x.type == "Gloves");
        for (int i = 0; i < thn5.Count; i++)
        {
            Tap5Slots.Add(Instantiate(collectionBox)); // 아이템 생성
            Tap5Slots[Tap5Slots.Count - 1].transform.SetParent(panel5.transform);
            Tap5Slots[Tap5Slots.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn5[i].grade);
            Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(thn5[i].icon);
            Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().text = thn5[i].name;
            Tap5Slots[Tap5Slots.Count - 1].SetActive(true);

            //장비 획득 유무
            if (thn5[i].illustrate)
            {
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn5[i].grade);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(1, 1, 1);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(1, 1, 1);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { EquipInfoPopup(thn5[num]); });

            }
            else
            {
                Color frameColor = Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color;
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = new Color(frameColor.r / 2f, frameColor.g / 2f, frameColor.b / 2f);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                //획득 못했다는 문구
                Tap5Slots[Tap5Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    alrImage.SetActive(false);
                    StartCoroutine(alrImageActive());
                });
            }
        }//5


        //Panel6
        List<Things> thn6 = things.FindAll(x => x.type == "Pants");
        for (int i = 0; i < thn6.Count; i++)
        {
            Tap6Slots.Add(Instantiate(collectionBox)); // 아이템 생성
            Tap6Slots[Tap6Slots.Count - 1].transform.SetParent(panel6.transform);
            Tap6Slots[Tap6Slots.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn6[i].grade);
            Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(thn6[i].icon);
            Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().text = thn6[i].name;
            Tap6Slots[Tap6Slots.Count - 1].SetActive(true);

            //장비 획득 유무
            if (thn6[i].illustrate)
            {
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn6[i].grade);
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(1, 1, 1);
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(1, 1, 1);
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { EquipInfoPopup(thn6[num]); });

            }
            else
            {
                Color frameColor = Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color;
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = new Color(frameColor.r / 2f, frameColor.g / 2f, frameColor.b / 2f);
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                //획득 못했다는 문구
                Tap6Slots[Tap6Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    alrImage.SetActive(false);
                    StartCoroutine(alrImageActive());
                });
            }
        }//6

        //Panel7
        List<Things> thn7 = things.FindAll(x => x.type == "Boots");
        for (int i = 0; i < thn7.Count; i++)
        {
            Tap7Slots.Add(Instantiate(collectionBox)); // 아이템 생성
            Tap7Slots[Tap7Slots.Count - 1].transform.SetParent(panel7.transform);
            Tap7Slots[Tap7Slots.Count - 1].transform.localScale = new Vector3(1, 1, 1);
            Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn7[i].grade);
            Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(thn7[i].icon);
            Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().text = thn7[i].name;
            Tap7Slots[Tap7Slots.Count - 1].SetActive(true);

            //장비 획득 유무
            if (thn7[i].illustrate)
            {
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(thn7[i].grade);
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(1, 1, 1);
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(1, 1, 1);
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { EquipInfoPopup(thn7[num]); });

            }
            else
            {
                Color frameColor = Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color;
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button").GetComponent<Image>().color = new Color(frameColor.r / 2f, frameColor.g / 2f, frameColor.b / 2f);
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button/Image").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button/Text").GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
                //획득 못했다는 문구
                Tap7Slots[Tap7Slots.Count - 1].transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
                {
                    alrImage.SetActive(false);
                    StartCoroutine(alrImageActive());
                });
            }
        }//7

    }


    void SwitchScrollPanel()
    {
        Tap1Button.GetComponent<Button>().onClick.AddListener(() =>   {
            collectionList.GetComponent<ScrollRect>().content = panel1.GetComponent<RectTransform>();
            panel1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel1.GetComponent<RectTransform>().anchoredPosition.y);
        });
        Tap2Button.GetComponent<Button>().onClick.AddListener(() => {
            collectionList.GetComponent<ScrollRect>().content = panel2.GetComponent<RectTransform>();
            panel2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel2.GetComponent<RectTransform>().anchoredPosition.y);
        });
        Tap3Button.GetComponent<Button>().onClick.AddListener(() => {
            collectionList.GetComponent<ScrollRect>().content = panel3.GetComponent<RectTransform>();
            panel3.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel3.GetComponent<RectTransform>().anchoredPosition.y);
        });
        Tap4Button.GetComponent<Button>().onClick.AddListener(() => {
            collectionList.GetComponent<ScrollRect>().content = panel4.GetComponent<RectTransform>();
            panel4.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel4.GetComponent<RectTransform>().anchoredPosition.y);
        });
        Tap5Button.GetComponent<Button>().onClick.AddListener(() => {
            collectionList.GetComponent<ScrollRect>().content = panel5.GetComponent<RectTransform>();
            panel5.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel5.GetComponent<RectTransform>().anchoredPosition.y);
        });
        Tap6Button.GetComponent<Button>().onClick.AddListener(() => {
            collectionList.GetComponent<ScrollRect>().content = panel6.GetComponent<RectTransform>();
            panel6.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel6.GetComponent<RectTransform>().anchoredPosition.y);
        });
        Tap7Button.GetComponent<Button>().onClick.AddListener(() => {
            collectionList.GetComponent<ScrollRect>().content = panel7.GetComponent<RectTransform>();
            panel7.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, panel7.GetComponent<RectTransform>().anchoredPosition.y);
        });

    }

    //문구 출력 애니메이션
    IEnumerator alrImageActive()
    {
        alrImage.SetActive(true);
        yield return new WaitForSeconds(1.0f);
    }


    //무기 정보 팝업
    public void EquipInfoPopup(Things things)
    {
        EquipItemInfoPopup.SetActive(true);

        Equipment equip = equipmentData.getEquipmentList().Find(x => x.name == things.name);

        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/NameBox/ItemNameText").gameObject.GetComponent<Text>().text = equip.name;
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/ReinText").gameObject.SetActive(false);
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/ItemInfoText").gameObject.GetComponent<Text>().text = equip.explanation;
        string nesMtr = "";
        for (int i = 0; i < equip.necessaryMaterials.Length; i++)
        {
            nesMtr += equip.necessaryMaterials[i] + " " + equip.necessaryMaterialsNum[i];
            if (i < equip.necessaryMaterials.Length - 1) nesMtr += "\n";
        }
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text = nesMtr;
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = equip.time.ToString();
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/EquipText").gameObject.SetActive(false);
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/SkillBox/Text").gameObject.GetComponent<Text>().text = "스킬 : " + equip.skill;

        Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equip.name).grade);
        EquipItemInfoPopup.transform.Find("UIPanel/ItemBox/GradeFrame").gameObject.GetComponent<Image>().color = col;
        EquipItemInfoPopup.transform.Find("UIPanel/ItemBox/Icon").gameObject.GetComponent<Image>().sprite
            = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equip.name).icon);


        string abstr = "";
        if (equip.stat.dps > 0) abstr += "전투력 " + (int)equip.stat.dps + "\n";
        if (equip.stat.strPower > 0) abstr += "공격력 " + equip.stat.strPower + "\n";
        if (equip.stat.attackSpeed > 0) abstr += "공격속도 " + equip.stat.attackSpeed + "\n";
        if (equip.stat.focus > 0) abstr += "명중률 " + equip.stat.focus + "\n";
        if (equip.stat.critical > 0) abstr += "크리티컬 " + equip.stat.critical + "\n";
        if (equip.stat.defPower > 0) abstr += "방어력 " + equip.stat.defPower + "\n";
        if (equip.stat.evaRate > 0) abstr += "회피율 " + equip.stat.evaRate + "\n";
        abstr += "속성 " + equip.attribute;
        EquipItemInfoPopup.transform.Find("UIPanel/InfoBox/AbilityText").gameObject.GetComponent<Text>().text = abstr;

        EquipItemInfoPopup.transform.Find("UIPanel/SellButton").gameObject.SetActive(false);
        EquipItemInfoPopup.transform.Find("UIPanel/ChangeButton").gameObject.SetActive(false);
        EquipItemInfoPopup.transform.Find("UIPanel/ReinforceButton").gameObject.SetActive(false);
    }




}
