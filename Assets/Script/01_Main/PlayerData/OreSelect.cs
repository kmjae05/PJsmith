using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine.SceneManagement;

public class OreSelect : MonoBehaviour
{
    //Main GameObjects
    private GameObject MainCamera;
    private GameObject PlayerManager;
    private GameObject Main;

    //Json Data
    public WWW reader;
    static public JsonData OreData;

    //Ore GameObjects
    private GameObject OreSelectPopup;
    private GameObject Panel;
    private GameObject defaultOre;

    //Scroll Focus
    private RectTransform center;
    private RectTransform[] bttnPosition;
    private GameObject minBttn;
    private int minBttnNum;
    static private float[] distance;
    public float moveVel;
    private float expendSize = 1.0f;
    private float expendDistance = 30.0f;

    //Ore Data
    public string unknown_ore_name;
    public string unknown_ore_info;
    //시계
    //public float OreTimer = 30;
    //private Text[] Ore_TimeText;      
    private GameObject[] SelectLock;    //선택 빛 이미지_잠금
    private GameObject[] Select;        //선택 빛 이미지_일반
    private Text[] OreNameText;         //타이틀 텍스트
    static public Image[] Icon;               //광석 아이콘
    private Image[] LockIcon;           //잠금 아이콘
    private Text[] InfoText;            //레벨 제한 텍스트
    private Text[] HaveText;            //광석 갯수 텍스트


    //Ore Info Popup
    private GameObject _OreInfoPopup;
    private Text _OreNameText;
    private Text _OreInfoText;
    private Text _TimeText;    //시계
    private Text _AlertText;
    private Text _AmountText;
    private Text _TipText;
    private Image _OreIcon;
    private Image _LockIcon;
    private GameObject _SelectLock;
    private GameObject _Select;
    private Button _BuyButton;
    private Button _UnLockButton;
    private Text _Price_Gold;
    private Text _Price_Cash;

    //System Popup
    private GameObject SystemPopup;
    private Text Sys_TitleText;
    private Text Sys_InfoText;
    private Button Sys_YesButton;
    private Button Sys_NoButton;
    private Button Sys_OkButton;
    static public Ore SelectOre;
    static public bool auto_Restart;

    //etc
    private Image FadeImage;
    private List<GameObject> G_OreList = new List<GameObject>();
    public List<Ore> oreList = new List<Ore>();             //전체 광석 리스트
    static public int consume_ores = 1;                     //소모될 광석 갯수
    static public List<Ore> av_OreList = new List<Ore>();   //레벨에 따라 출력될 광석 리스트
    static public bool gameSucceed;                         //제련 성공,실패 구분
    //private GameObject gold;                              //골드 구매 시 출력되는 텍스트
    
    [HideInInspector]
    public bool can_Restart = true;

    public class Ore
    {
        public int no;
        public string name;
        public int hp;
        public int exp;
        public int gold;
        public int item_no;
        public int level;
        public int ava_level;
        public int price;
        public string comments;
        public string icon;
        public int unLockCost;
        public int have;
        public bool isLock;

        public Ore(JsonData oreData, int index)
        {
            this.no = (int)oreData["Ore"][index]["no"];
            this.name = oreData["Ore"][index]["name"].ToString();
            this.hp = (int)oreData["Ore"][index]["hp"];
            this.exp = (int)oreData["Ore"][index]["exp"];
            this.gold = (int)oreData["Ore"][index]["gold"];
            this.item_no = (int)oreData["Ore"][index]["item_no"];
            this.level = (int)oreData["Ore"][index]["level"];
            this.ava_level = (int)oreData["Ore"][index]["ava_level"];
            this.price = (int)oreData["Ore"][index]["price"];
            this.comments = oreData["Ore"][index]["comments"].ToString();
            this.icon = oreData["Ore"][index]["Icon"].ToString();
            this.unLockCost = (int)oreData["Ore"][index]["unlockCost"];
            this.have = 0;
            this.isLock = false;
        }
    }


    void Awake()
    {
        Main = GameObject.Find("Main");
        MainCamera = GameObject.Find("Main_Camera");
        PlayerManager = GameObject.Find("PlayerManager");
        OreSelectPopup = GameObject.Find("Menu").transform.Find("OreSelectPopup").gameObject;
        Panel = OreSelectPopup.transform.Find("UIPanel/Scroll/Panel").gameObject;
        defaultOre = Panel.transform.Find("OreBox").gameObject;
        defaultOre.SetActive(false);
        center = OreSelectPopup.transform.Find("UIPanel/Center").gameObject.GetComponent<RectTransform>();

        _OreInfoPopup = GameObject.Find("System").transform.Find("OreInfoPopup").gameObject;
        _OreNameText = _OreInfoPopup.transform.Find("UIPanel/InfoBox/OreNameText").gameObject.GetComponent<Text>();
        _OreInfoText = _OreInfoPopup.transform.Find("UIPanel/InfoBox/OreInfoText").gameObject.GetComponent<Text>();
        _AmountText = _OreInfoPopup.transform.Find("UIPanel/OreBox/HaveText").gameObject.GetComponent<Text>();
        _TipText = _OreInfoPopup.transform.Find("UIPanel/TipText").gameObject.GetComponent<Text>();
        _OreIcon = _OreInfoPopup.transform.Find("UIPanel/OreBox/Icon").gameObject.GetComponent<Image>();
        _LockIcon = _OreInfoPopup.transform.Find("UIPanel/OreBox/LockIcon").gameObject.GetComponent<Image>();
        _AlertText = _LockIcon.transform.Find("InfoText").gameObject.GetComponent<Text>();
        _BuyButton = _OreInfoPopup.transform.Find("UIPanel/BuyButton").gameObject.GetComponent<Button>();
        _UnLockButton = _OreInfoPopup.transform.Find("UIPanel/UnlockButton").gameObject.GetComponent<Button>();
        _Price_Gold = _BuyButton.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
        _Price_Cash = _UnLockButton.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();

        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        Sys_TitleText = SystemPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>();
        Sys_InfoText = SystemPopup.transform.Find("UIPanel/InfoText").gameObject.GetComponent<Text>();
        Sys_YesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        Sys_NoButton = SystemPopup.transform.Find("UIPanel/NoButton").gameObject.GetComponent<Button>();
        Sys_OkButton = SystemPopup.transform.Find("UIPanel/OKButton").gameObject.GetComponent<Button>();

        FadeImage = GameObject.Find("FadeCanvas").transform.Find("FadeImage").GetComponent<Image>();
    }
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Ore.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            OreData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Ore.json");
            OreData = JsonMapper.ToObject(tmp);
        }

        bttnPosition = new RectTransform[OreData["Ore"].Count];
        distance = new float[OreData["Ore"].Count];

        // json데이터에서 광석정보 저장, 광석 오브젝트 세팅
        for (int i = 0; i < OreData["Ore"].Count; i++)
        {
            oreList.Add(new Ore(OreData, i));
            G_OreList.Add(Instantiate(defaultOre));
            G_OreList[i].transform.SetParent(Panel.transform, false);
            G_OreList[i].SetActive(true);
            bttnPosition[i] = G_OreList[i].GetComponent<RectTransform>();
            StartCoroutine(Get_Ore(i));
            int index = i;
            G_OreList[i].GetComponent<Button>().onClick.AddListener(() => { ClickOre(index); });
        }
        // 게임 오브젝트 배열 초기화
        SelectLock = new GameObject[oreList.Count];
        Select = new GameObject[oreList.Count];
        OreNameText = new Text[oreList.Count];
        Icon = new Image[oreList.Count];
        LockIcon = new Image[oreList.Count];
        InfoText = new Text[oreList.Count];
        HaveText = new Text[oreList.Count];

        // 각 광석박스 하위 오브젝트 초기화
        for (int i = 0; i < G_OreList.Count; i++)
        {
            Transform[] objects = G_OreList[i].GetComponentsInChildren<Transform>();
            for (int j = 0; j < objects.Length; j++)
            {
                switch (objects[j].name)
                {
                    case "SelectLock":
                        SelectLock[i] = objects[j].gameObject;
                        SelectLock[i].gameObject.SetActive(false);
                        break;
                    case "Select":
                        Select[i] = objects[j].gameObject;
                        Select[i].gameObject.SetActive(false);
                        break;
                    case "OreNameText":
                        OreNameText[i] = objects[j].gameObject.GetComponent<Text>();
                        OreNameText[i].text = unknown_ore_name;
                        break;
                    case "Icon":
                        Icon[i] = objects[j].gameObject.GetComponent<Image>();
                        Icon[i].sprite = Resources.Load<Sprite>("Ore/" + oreList[i].icon);
                        Icon[i].color = new Color(0.0f, 0.0f, 0.0f);
                        break;
                    case "LockIcon":
                        LockIcon[i] = objects[j].gameObject.GetComponent<Image>();
                        break;
                    case "InfoText":
                        InfoText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "HaveText":
                        HaveText[i] = objects[j].gameObject.GetComponent<Text>();
                        HaveText[i].gameObject.SetActive(false);
                        break;
                }
            }
        }

        //광석 오브젝트 초기화
        for (int i = 0; i < G_OreList.Count; i++)
        {
            if (!oreList[i].isLock)
            {
                OreNameText[i].text = oreList[i].name;
                LockIcon[i].gameObject.SetActive(false);
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == oreList[i].name) != null)
                    oreList[i].have = ThingsData.instance.getInventoryThingsList().Find(x => x.name == oreList[i].name).possession;
                else oreList[i].have = 0;
                HaveText[i].gameObject.SetActive(true);
                Icon[i].color = new Color(1.0f, 1.0f, 1.0f);
                SelectLock[i].SetActive(false);
            }
            else
            {
                OreNameText[i].text = "???";
                InfoText[i].text = "레벨 " + oreList[i].level + "이상";
            }
        }
        //oreList[0].have = 5;        //기본광석 5개로 시작

        Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(222, 0);
        StartCoroutine(PinPanel());

    }
    IEnumerator OnLevelWasLoaded()
    {
        int index = SelectOre.no;
        //OreSelectPopup.SetActive(true);
        yield return new WaitForSeconds(0.0003f);
        Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(Panel.GetComponent<RectTransform>().anchoredPosition.x - index * 500, 150);
    }

    void Update()
    {
        for (int i = 0; i < G_OreList.Count; i++)
        {
            if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == oreList[i].name) != null)
                oreList[i].have = ThingsData.instance.getInventoryThingsList().Find(x => x.name == oreList[i].name).possession;
            else oreList[i].have = 0;
            HaveText[i].text = oreList[i].have.ToString();  //갯수 최신화

            //오브젝트 위치에 따라 크기, 글자색 조정
            distance[i] = Mathf.Abs(center.transform.position.x - bttnPosition[i].transform.position.x);
            if (distance[i] <= expendDistance)
            {
                bttnPosition[i].transform.localScale = new Vector3(expendSize - distance[i] * 0.02f, expendSize - distance[i] * 0.02f);
            }
            else
            {
                bttnPosition[i].transform.localScale = new Vector3(0.8f, 0.8f);
            }

            if (bttnPosition[i].transform.localScale.x >= 1.0f)
            {
                bttnPosition[i].transform.localScale = new Vector3(1.0f, 1.0f);
            }

            else if (bttnPosition[i].transform.localScale.x < 0.8f)
            {
                bttnPosition[i].transform.localScale = new Vector3(0.8f, 0.8f);
            }
            float minDistance = Mathf.Min(distance);
            if (minDistance == distance[i]) //가운데 있는 광석
            {
                SelectOre = oreList[i];
                minBttn = G_OreList[i];
                minBttnNum = i;
                if (!oreList[i].isLock)
                {
                    Select[i].SetActive(true);
                }
                else
                {
                    SelectLock[i].SetActive(true);
                }
            }
            else
            {
                if (!oreList[i].isLock)
                {
                    Select[i].SetActive(false);
                }
                else
                {
                    SelectLock[i].SetActive(false);
                }
            }
        }
    }

    IEnumerator Get_Ore(int index)  //광석 잠금 해제
    {
        yield return new WaitUntil(() => oreList[index].level <= Player.instance.getUser().level);
        if (oreList[index].isLock)
        {
            oreList[index].isLock = false;
            OreNameText[index].text = oreList[index].name;
            LockIcon[index].gameObject.SetActive(false);
            HaveText[index].gameObject.SetActive(true);
            Icon[index].color = new Color(1.0f, 1.0f, 1.0f);
            SelectLock[index].SetActive(false);
        }
    }

    public void ClickOre(int index)     //광석 클릭했을 때 팝업 세팅
    {
        if (G_OreList[index] == minBttn)
        {
            _OreIcon.sprite = Icon[index].sprite;
            Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == oreList[index].name).grade);
            _OreInfoPopup.transform.Find("UIPanel/OreBox/GradeFrame").gameObject.GetComponent<Image>().color = col;
            _OreInfoPopup.SetActive(true);

            if (!oreList[index].isLock)     //사용 가능한 광석 선택 시
            {
                _LockIcon.enabled = false;
                _OreIcon.color = new Color(1.0f, 1.0f, 1.0f);
                _OreNameText.text = oreList[index].name;        //광석 이름 출력
                _OreNameText.color = new Color(1.0f, 0.573f, 0.0f);
                _OreInfoText.text = oreList[index].comments;    //광석 설명 출력
                _Price_Gold.text = oreList[index].price.ToString();
                _AlertText.gameObject.SetActive(false);
                _TipText.text = "골드로 구매하여 사용할 수 있습니다.";
                _BuyButton.gameObject.SetActive(true);          //구매버튼 활성화
                _UnLockButton.gameObject.SetActive(false);      //잠금해제 버튼 비활성화
                _AmountText.text = oreList[index].have.ToString();
            }
            else if (oreList[index].isLock)   //잠긴 광석 선택 시
            {
                _LockIcon.enabled = true;
                _OreIcon.color = new Color(0.0f, 0.0f, 0.0f);
                _OreNameText.text = unknown_ore_name;           //알 수 없는 이름 출력
                _OreNameText.color = new Color(0.514f, 0.514f, 0.514f);
                _OreInfoText.text = unknown_ore_info;           //알 수 없는 설명 출력
                _AlertText.text = "획득조건 : 레벨 " + oreList[index].level + "이상";
                _TipText.text = "보석을 사용하여 잠금을 해제할 수 있습니다.";
                _BuyButton.gameObject.SetActive(false);         //구매버튼 비활성화
                _UnLockButton.gameObject.SetActive(true);       //잠금해제 버튼 활성화
                _Price_Cash.text = oreList[index].unLockCost.ToString();    //잠금해제 가격 표기
                if (oreList[index].have == 0)                   //보유한 광석이 0개라면 갯수 표기 끔
                {
                    _AmountText.text = "";
                }
                else
                {
                    _AmountText.text = oreList[index].have.ToString();
                }
            }
        }
        else
        {
            MoveToOre(index);
        }
    }

    //인벤토리에서 버튼 클릭
    public void ClickInventory(InventoryThings things)
    {
        _OreIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x=>x.name == things.name).icon);
        _LockIcon.enabled = false;
        _OreIcon.color = new Color(1.0f, 1.0f, 1.0f);
        _OreNameText.text = things.name;        //광석 이름 출력
        _OreNameText.color = new Color(1.0f, 0.573f, 0.0f);
        _OreInfoText.text = ThingsData.instance.getThingsList().Find(x => x.name == things.name).explanation;    //광석 설명 출력
        _Price_Gold.text = oreList[ThingsData.instance.getThingsList().Find(x => x.name == things.name).item_no-3].price.ToString();
        _AlertText.gameObject.SetActive(false);
        _TipText.text = "";
        _BuyButton.gameObject.SetActive(false);          //구매버튼 활성화
        _UnLockButton.gameObject.SetActive(false);      //잠금해제 버튼 비활성화
        _AmountText.text = things.possession.ToString();
    }




    public void MoveToOre(int index)    //광석으로 이동
    {
        if (index > minBttnNum)
        {
            StartCoroutine(MoveRight(index));
        }
        else
        {            
            StartCoroutine(MoveLeft(index));
        }
    }
    IEnumerator MoveLeft(int index)     //왼쪽으로 이동
    {
        float velocity = moveVel;
        RectTransform panelPos = Panel.GetComponent<RectTransform>();
        float temp = 0; //이동한 거리
        float dist = distance[index];       //거리
        while (true)
        {
            float prePosition = panelPos.transform.position.x;
            panelPos.transform.position += new Vector3(velocity, 0);
            temp += Mathf.Abs( prePosition - panelPos.transform.position.x);            
            if (dist < temp)
            {
                panelPos.transform.position -= new Vector3(Mathf.Abs( dist-temp),0);
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator MoveRight(int index)    //오른쪽으로 이동
    {
        float velocity = moveVel;
        RectTransform panelPos = Panel.GetComponent<RectTransform>();
        float temp = 0; //이동한 거리
        float dist = distance[index];       //거리
        while (true)
        {
            float prePosition = panelPos.transform.position.x;
            panelPos.transform.position -= new Vector3(velocity, 0);
            temp += Mathf.Abs(prePosition - panelPos.transform.position.x);
            if (dist < temp)
            {
                panelPos.transform.position += new Vector3(Mathf.Abs(dist - temp), 0);
                yield break;
            }
            yield return null;
        }
    }

    public void BuyOre()            //광석 구매 버튼 클릭 시 호출되는 함수
    {
        if (Player.instance.getUser().gold < SelectOre.price)     //골드 부족하면 시스템 팝업 호출
        {
            Sys_YesButton.gameObject.SetActive(true);
            Sys_NoButton.gameObject.SetActive(true);
            Sys_OkButton.gameObject.SetActive(false);
            Sys_TitleText.GetComponent<Text>().text = "골드가 부족합니다.";
            Sys_InfoText.GetComponent<Text>().text = "골드 구매 페이지로 이동하시겠습니까?";
            Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            //상점 켜기
            //-----

            SystemPopup.SetActive(true);
            return;
        }
        Player.instance.LostMoney("gold", SelectOre.price);
        SelectOre.have += 1;

        if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == SelectOre.name) != null)
        {
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == SelectOre.name).possession += 1;
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == SelectOre.name).recent = true;
        }
        else
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(
                ThingsData.instance.getThingsList().Find(x => x.name == SelectOre.name).type, SelectOre.name, 1));
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == SelectOre.name).recent = true;
        }
        _AmountText.text = SelectOre.have.ToString();
        
    }



    public void UnLockOre()         //잠금된 광석 잠금해제 버튼 클릭 시 호출되는 함수
    {
        Sys_TitleText.GetComponent<Text>().text = "잠금 해제";
        Sys_InfoText.GetComponent<Text>().text = "보석을 사용하여 알 수 없는 광석의 잠금을 해제하시겠습니까?";

        Sys_YesButton.gameObject.SetActive(true);
        Sys_NoButton.gameObject.SetActive(true);
        Sys_OkButton.gameObject.SetActive(false);

        Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
        Sys_YesButton.GetComponent<Button>().onClick.AddListener(() => Click_Yes_UnLockOre());    //버튼 기능 추가
        SystemPopup.SetActive(true);
    }

    public void Click_Yes_UnLockOre()   //잠금해제 창 팝업 후 예 버튼을 눌렀을 때 실행되는 함수
    {
        if (Player.instance.getUser().cash < SelectOre.unLockCost)
        {
            SystemPopup.SetActive(false);

            //SystemPopup 종료
            Sys_TitleText.GetComponent<Text>().text = "보석이 부족합니다";
            Sys_InfoText.GetComponent<Text>().text = "보석 구매 페이지로 이동하시겠습니까?";

            Sys_YesButton.gameObject.SetActive(true);     //예/아니오 버튼으로 수정
            Sys_NoButton.gameObject.SetActive(true);
            Sys_OkButton.gameObject.SetActive(false);

            Sys_YesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            Sys_YesButton.GetComponent<Button>().onClick.AddListener(() => OreSelectPopup.SetActive(false));    //광석 선택창 끄기
            Sys_YesButton.GetComponent<Button>().onClick.AddListener(() => SystemPopup.SetActive(false));       //SystemPopup 끄기
            //상점켜기
            //-----

            SystemPopup.SetActive(true);
        }
        else
        {
            oreList[SelectOre.no].isLock = false;
            OreNameText[SelectOre.no].text = oreList[SelectOre.no].name;
            LockIcon[SelectOre.no].gameObject.SetActive(false);
            HaveText[SelectOre.no].gameObject.SetActive(true);
            Icon[SelectOre.no].color = new Color(1.0f, 1.0f, 1.0f);
            SelectLock[SelectOre.no].SetActive(false);

            Player.instance.LostMoney("cash", SelectOre.unLockCost);
            SystemPopup.SetActive(false);

            Sys_OkButton.GetComponent<Button>().onClick.RemoveAllListeners();       //버튼 리스너 모두 삭제
            Sys_OkButton.GetComponent<Button>().onClick.AddListener(() => ClickOre(SelectOre.no));
            Sys_TitleText.GetComponent<Text>().text = "알림";
            Sys_InfoText.GetComponent<Text>().text = "새로운 광석을 획득했습니다.";
            Sys_YesButton.gameObject.SetActive(false);                              //확인 버튼으로 수정
            Sys_NoButton.gameObject.SetActive(false);
            Sys_OkButton.gameObject.SetActive(true);

            SystemPopup.SetActive(true);
        }
    }

    IEnumerator PinPanel()       //OreSelect Popup이 켜졌을 때 호출
    {
        while (true)
        {
            yield return new WaitUntil(() => OreSelectPopup.activeInHierarchy);
            Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(222, 0);

            MainCamera.GetComponent<UnityStandardAssets.ImageEffects.Blur>().enabled = true;
            yield return new WaitUntil(() => !OreSelectPopup.activeInHierarchy);
            MainCamera.GetComponent<UnityStandardAssets.ImageEffects.Blur>().enabled = false;
        }
    }

    public void EnterToGame()        //제련하기 버튼 클릭 시 호출되는 함수
    {
        if (SelectOre.isLock)  //사용할 수 없는 광석선택 후 제련하기 눌렀을 때
        {
            return;
        }

        if (SelectOre.have < consume_ores)    //원석이 부족할 경우
        {
            Sys_YesButton.gameObject.SetActive(false);
            Sys_NoButton.gameObject.SetActive(false);
            Sys_OkButton.gameObject.SetActive(true);
            Sys_TitleText.GetComponent<Text>().text = "광석이 부족합니다.";
            Sys_InfoText.GetComponent<Text>().text = "광석을 구매해주세요.";
            Sys_OkButton.GetComponent<Button>().onClick.RemoveAllListeners();
            Sys_OkButton.GetComponent<Button>().onClick.AddListener(() => ClickOre(SelectOre.no));
            SystemPopup.SetActive(true);
            can_Restart = false;
        }
        else        //정상 시작
        {
            SelectOre.have -= consume_ores;
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == SelectOre.name).possession -= consume_ores;

            StartCoroutine("FadeOut");
        }
    }
    IEnumerator FadeOut()
    {
        FadeImage.gameObject.SetActive(true);
        for (float fade = 0; fade <= 1.0f; fade += 0.02f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        SceneManager.LoadScene("08_Loading_GameIn");
    }

    public void SetGameObject()
    {
        Main.SetActive(false);
    }
    /*
    public void Click_Restart_Button()
    {
        HPSlider.GetComponent<HP_Slider>().Exit_Game(); //hp관리 초기화
        ConsumeOre();
        
        if (!can_Restart)
        {
            Sys_OkButton.GetComponent<Button>().onClick.RemoveAllListeners();
            Sys_OkButton.GetComponent<Button>().onClick.AddListener(() => Go_Main());
        }
        else
        {
            if (gameSucceed)    //게임 성공 시 보상 부여
            {
                PlayerManager.GetComponent<Player>().GetGameReward(Iron.GetComponent<Attack>().Exp, Iron.GetComponent<Attack>().Gold);    //보상 관련 함수 실행
            }
        }
    }

    public void Check_AutoRestart()     //연속제련 체크되어있는지 확인
    {
        if (AutoRestart.isOn)
        {
            auto_Restart = true;
            Stop_AutoRestart.SetActive(true);
        }
        else if (!AutoRestart.isOn)
        {
            auto_Restart = false;
            Stop_AutoRestart.SetActive(false);
        }
    }
    public void Cancel_AutoRestart()    //연속제련 해제
    {
        auto_Restart = false;
    }

    */



}