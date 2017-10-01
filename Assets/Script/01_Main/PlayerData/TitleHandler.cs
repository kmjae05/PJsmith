using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class TitleHandler : MonoBehaviour {
    private GameObject TitlePopup;
    private Text ChrTitleText;
    private GameObject Slot;
    private GameObject defaultTitle;
    private Text NowTitleText;
    private Text NowTitleText_BonusStat;
    private Text player_Title_Panel;
    private Text AmountText;
    private GameObject Profile_New_Icon;

    static public List<GameObject> G_TitleList;
    static public List<Title> titleList;
    public class Title  //업적 클래스
    {
        public int no;
        public string name;
        public string stat_type;
        public int stat_amount;
        public string comment;
        public bool isGet;
        public Title(int no, string name, string stat_type, int stat_amount, string comment)
        {
            this.no = no;
            this.name = name;
            this.stat_type = stat_type;
            this.stat_amount = stat_amount;
            this.comment = comment;
            this.isGet = false;
        }
    }

    private JsonData titleData;
    private WWW reader;

    public string none_title_name;
    static public int equip_title = 0;
    static public int selected_title = -1;

    private Text[] TitleText;
    private Image[] SelectPanel;
    private Image[] EquipPanel;
    private Text[] BonusText;


	void Awake () {
        TitlePopup = GameObject.Find("System").transform.Find("TitlePopup").gameObject;
        Slot = TitlePopup.transform.Find("UIPanel/Scroll/Slot").gameObject;
        defaultTitle = Slot.transform.Find("defaultTitle").gameObject;
        defaultTitle.SetActive(false);
        NowTitleText = TitlePopup.transform.Find("UIPanel/NowTitleBox/TitleText").gameObject.GetComponent<Text>();
        NowTitleText_BonusStat = TitlePopup.transform.Find("UIPanel/NowTitleBox/BonusText").gameObject.GetComponent<Text>();
        player_Title_Panel = GameObject.Find("PlayerTitleText").GetComponent<Text>();
        AmountText = TitlePopup.transform.Find("UIPanel/AmountText").GetComponent<Text>();
        ChrTitleText = GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/PlayerInfo/ChrTitleBox/ChrTitleText").gameObject.GetComponent<Text>();

        G_TitleList = new List<GameObject>();
        titleList = new List<Title>();

        #region 칭호관련 Json데이터 읽어오기
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Title.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            titleData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Title.json");
            titleData = JsonMapper.ToObject(tmp);
        }

        for (int i = 0; i < titleData["Title"].Count; i++)
        {
            titleList.Add(new Title(
                (int)titleData["Title"][i]["no"],
                titleData["Title"][i]["name"].ToString(),
                titleData["Title"][i]["stat_type"].ToString(),
                (int)titleData["Title"][i]["stat_amount"],
                titleData["Title"][i]["comment"].ToString()));
        }

        TitleText = new Text[titleList.Count];
        SelectPanel = new Image[titleList.Count];
        EquipPanel = new Image[titleList.Count];
        BonusText = new Text[titleList.Count];
        #endregion
        #region Json데이터에 따라 GameObject 생성
        for (int i = 0; i < titleList.Count; i++)
        {
            GameObject TITLE;
            TITLE = Instantiate(defaultTitle);
            TITLE.GetComponent<SelectTitle>().title_id = i;
            TITLE.SetActive(true);
            TITLE.transform.SetParent(Slot.transform, false);
            G_TitleList.Add(TITLE);
            Transform[] objects = TITLE.gameObject.GetComponentsInChildren<Transform>(true);

            for(int j=0; j<objects.Length; j++){
                switch(objects[j].name){
                    case "Title":
                        TitleText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "Back_Click":
                        SelectPanel[i] = objects[j].gameObject.GetComponent<Image>();
                        break;
                    case "Back_Equip":
                        EquipPanel[i] = objects[j].gameObject.GetComponent<Image>();
                        break;
                    case "BonusText":
                        BonusText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                }
            }

            TitleText[i].text = titleList[i].name;
            SelectPanel[i].gameObject.SetActive(false);
            EquipPanel[i].gameObject.SetActive(false);
            BonusText[i].gameObject.SetActive(false);
            G_TitleList[i].SetActive(false);
            if (string.Compare(titleList[i].stat_type, "0") != 0)
            {
                BonusText[i].text = titleList[i].comment;
                BonusText[i].gameObject.SetActive(true);
            }
        }
        #endregion

        player_Title_Panel.text = none_title_name;
        ChrTitleText.text = none_title_name;
        titleList[0].isGet = true;
        //equip_title = myServerTitle;  //서버에서 받은 타이틀로 변경
        //GetBonusStat(titleList[equip_title]);
	}

    void Start()
    {
        StartCoroutine(PinPanel());
    }
    void Update()
    {
        int title_amount = 0;
        for (int i = 0; i < G_TitleList.Count; i++)
        {
            if (titleList[i].isGet)     //얻은 칭호는 활성화
            {
                G_TitleList[i].SetActive(true);
                title_amount++;
            }
            if (i == selected_title && titleList[i].isGet)  //칭호 선택
            {
                SelectPanel[i].gameObject.SetActive(true);
            }
            else
            {
                SelectPanel[i].gameObject.SetActive(false);
            }
            if (i == equip_title)   //착용 중인 칭호 표기
            {
                EquipPanel[equip_title].gameObject.SetActive(true);
            }
            else
            {
                EquipPanel[i].gameObject.SetActive(false);
            }
        }
        NowTitleText.text = titleList[equip_title].name;
        if (titleList[equip_title].stat_type != "0")
        {
            NowTitleText_BonusStat.text = titleList[equip_title].comment;
        }
        else
        {
            NowTitleText_BonusStat.text = "";
        }
        AmountText.text = title_amount +"/"+ titleList.Count;
    }
    public void Equip_Title()   //칭호 착용 함수
    {
        if (selected_title == -1)
        {
            return;
        }
        if (titleList[selected_title].isGet)
        {
            DiscardBonusStat(titleList[equip_title]);
            equip_title = selected_title;   //선택한 칭호 착용
            if (equip_title == 0)
            {
                player_Title_Panel.text = none_title_name;
                ChrTitleText.text = none_title_name;
            }
            else
            {
                player_Title_Panel.text = titleList[equip_title].name;
                ChrTitleText.text = titleList[equip_title].name;
            }
            GetBonusStat(titleList[equip_title]);
        }
    }
    public void check_new_Title()   //new 버튼 비활성화, 칭호 창 열었을 때 실행
    {
        Slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(396, 0);
    }
    /*
    public void check_indv_new_Title()  //각 타이틀 옆 new 버튼 비활성화
    {

    }
     * */
    public void ArrangeTitle()      //칭호 정렬 함수, 얻은 것 우선하며 최초 위치 고수
    {
        int[] check = new int[titleList.Count];

        for (int a = 0; a < titleList.Count; a++)
        {
            int minNumber = titleList.Count;
            for (int i = a; i < titleList.Count; i++)
            {
                if (titleList[i].isGet && titleList[i].no < minNumber && check[i] != 1)
                {
                    minNumber = titleList[i].no;
                }
            }
            if (minNumber != titleList.Count)
            {
                G_TitleList[minNumber].transform.SetSiblingIndex(a);
                check[minNumber] = 1;
            }
        }
    }
    public void GetBonusStat(Title title)   //칭호 효과 추가
    {
        string statType = title.stat_type;
        int statAmount = title.stat_amount;

        switch (statType)
        {
            case "attack":
                Player.Play.strPower += statAmount;
                break;
            case "attack_Speed":
                Player.Play.strPower += statAmount;
                break;
            case "critical_rate":
                break;
            case "critical_damage":
                break;
            default:
                break;
        }
    }
    public void DiscardBonusStat(Title equip_title) //착용 중이던 칭호 효과 해제
    {
        string statType = equip_title.stat_type;
        int statAmount = equip_title.stat_amount;
        if (string.Compare(statType, "0") != 0)
        {
            switch (statType)
            {
                case "attack":
                    Player.Play.strPower -= statAmount;
                    break;
                case "attack_Speed":
                    break;
                case "critical_rate":
                    break;
                case "critical_damage":
                    break;
                default:
                    break;
            }
        }
        else
        {
            return;
        }
    }

    public IEnumerator PinPanel()
    {
        while (true)
        {
            yield return new WaitUntil(() => TitlePopup.activeInHierarchy);
            Slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(483, 349);
            selected_title = -1;
            yield return new WaitUntil(() => !TitlePopup.activeInHierarchy);

        }
    }
}