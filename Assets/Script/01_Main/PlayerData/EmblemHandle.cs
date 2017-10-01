using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class EmblemHandle : MonoBehaviour{
    private GameObject EmblemPopup;
    private GameObject ProfileOutLine_Popup;
    private GameObject Slot;
    private GameObject defaultEmblem;
    private GameObject ProfileOutLine_Panel;
    private Text AmountText;
    private Text NowEmblemText;
    private Text NowEmblemText_BonusStat;

    public class Emblem
    {
        public int no;
        public int level;
        public string name;
        public string EbNum;
        public string stat_type;
        public int stat_amount;
        public string comment;
        public bool isGet;

        public Emblem(int no, int level, string name, string EbNum, string stat_type, int stat_amount, string comment)
        {
            this.no = no;
            this.level = level;
            this.name = name;
            this.EbNum = EbNum;
            this.stat_type = stat_type;
            this.stat_amount = stat_amount;
            this.comment = comment;
            this.isGet = false;
        }
    }
    static public List<Emblem> emblemList = new List<Emblem>();
    private List<GameObject> G_EmblemList = new List<GameObject>();
    
    private JsonData emblemData;
    private WWW reader;

    static public int equip_Emblem = 0;
    static public int selected_Emblem = 0;

    private Image[] EquipLine;
    private Image[] Select;
    private Image[] OutLine;
    private Image[] Deactivate;
    private Image[] EquipIcon;
    private Text[] InfoText;
    private Image[] NewIcon;

    void Awake()
    {
        EmblemPopup = GameObject.Find("System").transform.Find("EmblemPopup").gameObject;
        ProfileOutLine_Popup = GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/PlayerInfo/PlayerIconPanel/ProfileOutline").gameObject;
        Slot = EmblemPopup.transform.Find("UIPanel/Scroll/Slot").gameObject;
        defaultEmblem = Slot.transform.Find("defaultEmblem").gameObject;
        defaultEmblem.SetActive(false);
        ProfileOutLine_Panel = GameObject.Find("ProfileOutline");
        AmountText = EmblemPopup.transform.Find("UIPanel/AmountText").gameObject.GetComponent<Text>();
        NowEmblemText = EmblemPopup.transform.Find("UIPanel/NowEmblemBox/NameText").gameObject.GetComponent<Text>();
        NowEmblemText_BonusStat = EmblemPopup.transform.Find("UIPanel/NowEmblemBox/BonusText").gameObject.GetComponent<Text>();
        #region 엠블렘관련 Json데이터 읽어오기
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Emblem.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            emblemData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Emblem.json");
            emblemData = JsonMapper.ToObject(tmp);
        }

        for (int i = 0; i < emblemData["Emblem"].Count; i++)
        {
            emblemList.Add(new Emblem(
                (int)emblemData["Emblem"][i]["no"],
                (int)emblemData["Emblem"][i]["level"],
                emblemData["Emblem"][i]["name"].ToString(),
                emblemData["Emblem"][i]["EbNum"].ToString(),
                emblemData["Emblem"][i]["stat_type"].ToString(),
                (int)emblemData["Emblem"][i]["stat_amount"],
                emblemData["Emblem"][i]["comment"].ToString()));
        }

        EquipLine = new Image[emblemList.Count];
        Select = new Image[emblemList.Count];
        OutLine = new Image[emblemList.Count];
        Deactivate = new Image[emblemList.Count];
        EquipIcon = new Image[emblemList.Count];
        NewIcon = new Image[emblemList.Count];
        InfoText = new Text[emblemList.Count];
        #endregion
        #region Json데이터에 따라 GameObject 생성
        for (int i = 0; i < emblemList.Count; i++)
        {
            GameObject Emblem;
            Emblem = Instantiate(defaultEmblem);
            Emblem.GetComponent<SelectEmblem>().emblem_id = i;
            Emblem.SetActive(true);
            Emblem.transform.SetParent(Slot.transform, false);
            G_EmblemList.Add(Emblem);
            Transform[] objects = Emblem.GetComponentsInChildren<Transform>(true);
            for (int j = 0; j < objects.Length; j++)
            {
                switch (objects[j].name)
                {
                    case "Back_Equip":
                        EquipLine[i] = objects[j].gameObject.GetComponent<Image>();
                        EquipLine[i].gameObject.SetActive(false);
                        break;
                    case "Back_Click":
                        Select[i] = objects[j].gameObject.GetComponent<Image>();
                        Select[i].gameObject.SetActive(false);
                        break;
                        
                    case "Emblem":
                        OutLine[i] = objects[j].gameObject.GetComponent<Image>();
                        break;
                    case "InfoText":
                        InfoText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                        /*
                    case "Deactivate":
                        Deactivate[i] = objects[j].gameObject.GetComponent<Image>();
                        break;
                    case "EquipIcon":
                        EquipIcon[i] = objects[j].gameObject.GetComponent<Image>();
                        break;
                    case "NewIcon":
                        NewIcon[i] = objects[j].gameObject.GetComponent<Image>();
                        break;
                         * */
                }
            }

            //OutLine[i].sprite = Resources.Load("Emblem/ProfileOutline_" + emblemList[i].EbNum, typeof(Sprite)) as Sprite;
            //NewIcon[i].gameObject.SetActive(false);   //new 텍스트

            G_EmblemList[i].SetActive(false);


            if (i == 0)     //기본 테두리 획득기능 뛰어넘기
            {
                continue;
            }
            else
            {
                //StartCoroutine(getEmblem(emblemList[i]));
            }
        }
        #endregion
    }

    void Start()
    {
        emblemList[0].isGet = true;
        equip_Emblem = 0;
        StartCoroutine(PinPanel());

        for (int i = 0; i < G_EmblemList.Count; i++)
        {
            if (emblemList[i].stat_type != "0")
            {
                InfoText[i].text = emblemList[i].comment;
            }
            else
            {
                InfoText[i].text = "";
            }
        }

    }
    void Update()
    {
        int emblem_amount = 0;
        for (int i = 0; i < G_EmblemList.Count; i++)
        {
            if (emblemList[i].isGet)        //획득한 초상화
            {
                G_EmblemList[i].SetActive(true);
                emblem_amount++;
            }
            if (selected_Emblem == i && emblemList[i].isGet)    //선택한 초상화
            {
                Select[i].gameObject.SetActive(true);         //초상화 선택 시 표기
            }
            else
            {
                Select[i].gameObject.SetActive(false);  //선택 안 한 초상화는 Select 비활성화
            }

            EquipLine[i].gameObject.SetActive(false);       //equip
        }
        EquipLine[equip_Emblem].gameObject.SetActive(true); //equip

        NowEmblemText.text = emblemList[equip_Emblem].name;
        if (emblemList[equip_Emblem].stat_type != "0")
        {
            NowEmblemText_BonusStat.text = emblemList[equip_Emblem].stat_type;
        }
        else
        {
            NowEmblemText_BonusStat.text = "";
        }
        AmountText.text = emblem_amount + "/" + emblemList.Count;
    }
    IEnumerator getEmblem(Emblem emblem)    //테두리 획득 함수
    {
        yield return new WaitUntil(() => Player.Play.level >= emblem.level);
        yield return new WaitForSeconds(0.1f);
        emblem.isGet = true;
        //StartCoroutine(transform.GetComponent<Achievementhandle>().AcvBoxHandle(emblem.no + "번째 테두리 획득"));
        //NewIcon[emblem.no].gameObject.SetActive(true);   //new 텍스트

    }
    public void Equip_Emblem()      //테두리 장착 함수
    {
        if (equip_Emblem == -1)
        {
            return;
        }
        if (emblemList[selected_Emblem].isGet)
        {
            DiscardBonusStat(emblemList[equip_Emblem]);
            equip_Emblem = selected_Emblem;
            GetBonusStat(emblemList[equip_Emblem]);
            ProfileOutLine_Popup.GetComponent<Image>().sprite = OutLine[equip_Emblem].sprite;    //장착 중인 테두리로 변경
            ProfileOutLine_Panel.GetComponent<Image>().sprite = OutLine[equip_Emblem].sprite;
        }
    }
    public void Check_New_Emblem()  //new버튼 비활성화, 초상화 창 열었을 때 실행
    {
        Slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(478, 0);
    }
    public void Check_indv_new_Emblem() //개별 new 버튼 비활성화
    {
        for (int i = 0; i < G_EmblemList.Count; i++)
        {
            //NewIcon[i].gameObject.SetActive(false);   //new 텍스트
        }
    }
    public void GetBonusStat(Emblem emblem)   //칭호 효과 추가
    {
        string statType = emblem.stat_type;
        int statAmount = emblem.stat_amount;

        switch (statType)
        {
            case "attack":
                Player.Play.strPower += statAmount;
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
    public void DiscardBonusStat(Emblem equip_emblem) //착용 중이던 초상화 효과 해제
    {
        string statType = equip_emblem.stat_type;
        int statAmount = equip_emblem.stat_amount;
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
        yield return new WaitUntil(()=>EmblemPopup.activeInHierarchy);
        Slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(465, 448);
        selected_Emblem = -1;
        yield return new WaitUntil(() => !EmblemPopup.activeInHierarchy);
    }


}