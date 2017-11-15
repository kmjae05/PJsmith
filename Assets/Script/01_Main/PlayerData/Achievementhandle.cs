using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class Achievementhandle : MonoBehaviour {

    public float velocity;
    public float accelate;
    public float limit;

    private GameObject AchievementPopup;
    private GameObject Panel;
    private GameObject defaultAchv;
    private GameObject AcvBox;
    private GameObject NewIcon;
    private Text AcvText;

    /*
    private GameObject Achievement_AlertIcon;
    private GameObject Profile_AlertIcon;
    private GameObject New_Title_Icon;
    private GameObject New_Emblem_Icon;
    */
    /*
    public GameObject SystemPopup;
    public GameObject Sys_TitleText;
    public GameObject Sys_InfoText;
    public GameObject Sys_YesButton;
    public GameObject Sys_NoButton;
    public GameObject Sys_OkButton;
    */
    private List<GameObject> G_AchvList;
    private List<CAchievement> AchvList;


    //업적 달성 관련
    private bool popUp = false;
    private bool AchvExist = false;
    private Text PlayerScoreText;
    //업적에 필요한 수치 관련
    static public int normal_atk_count = 0;
    static public int critical_atk_count = 0;
    static public int ore_crash_count = 0;
    static public int get_gold_count = 0;
    static public int get_cash_count = 0;
    static public int get_collection_count = 0;

    //AchievePanel Objects
    private Text[] TitleText;
    private Text[] CommentsText;
    private GameObject[] RewardBox;
    private Text[] Reward_Amount;
    private Slider[] ProceedSlider;
    private Text[] ProceedText;
    private Text[] ScoreText;
    private Text[] SpecialReward;

    //RewardPanel Objects
    private GameObject[] RewardPanel;
    private Text[] TitleText_Reward;
    private Text[] CommentsText_Reward;
    private GameObject[] RewardBox_Reward;
    private Text[] RewardAmount_Reward;
    private Text[] SpecialReward_Reward;
    private Button[] RewardButton;
    private Text[] ScoreText_Reward;

    void Awake()
    {
        AchievementPopup = GameObject.Find("Menu").transform.Find("AchievePopup").gameObject;
        AcvBox = GameObject.Find("System").transform.Find("AlertPopup").gameObject;
        AcvText = AcvBox.GetComponentInChildren<Text>();
        Panel = AchievementPopup.transform.Find("UIPanel/Scroll/Panel").gameObject;
        defaultAchv = Panel.transform.Find("AchievePanel_Normal").gameObject;
        defaultAchv.SetActive(false);
        NewIcon = GameObject.Find("AchieveButton").transform.Find("NewIcon").gameObject;
        PlayerScoreText = AchievementPopup.transform.Find("UIPanel/AchivePointBox/PointText").gameObject.GetComponent<Text>();
        G_AchvList = new List<GameObject>();
        AchvList = new List<CAchievement>();
        //Achievement_AlertIcon = GameObject.Find("NewIcon_Achievement");
        //Achievement_AlertIcon.SetActive(false);
    }

    void Start()
    {
        AchvList = AchievementData.instance.getAchvList();

        #region GameObject 내부 요소 배열 초기화
        TitleText = new Text[AchvList.Count];
        CommentsText = new Text[AchvList.Count];
        RewardBox = new GameObject[AchvList.Count];
        Reward_Amount = new Text[AchvList.Count];
        ProceedSlider = new Slider[AchvList.Count];
        ProceedText = new Text[AchvList.Count];
        ScoreText = new Text[AchvList.Count];
        SpecialReward = new Text[AchvList.Count];

        RewardPanel = new GameObject[AchvList.Count];
        TitleText_Reward = new Text[AchvList.Count];
        CommentsText_Reward = new Text[AchvList.Count];
        RewardBox_Reward = new GameObject[AchvList.Count];
        RewardAmount_Reward = new Text[AchvList.Count];
        SpecialReward_Reward = new Text[AchvList.Count];
        RewardButton = new Button[AchvList.Count];
        ScoreText_Reward = new Text[AchvList.Count];

        #endregion
        #region Json데이터에 따라 GameObject 생성
        for (int i = 0; i < AchvList.Count; i++)
        {
            G_AchvList.Add(Instantiate(defaultAchv));
            G_AchvList[i].SetActive(true);
            G_AchvList[i].transform.SetParent(Panel.transform, false);

            Transform[] objects = G_AchvList[i].GetComponentsInChildren<Transform>(true);

            for (int j = 0; j < objects.Length; j++)
            {
                switch(objects[j].gameObject.name)
                {
                    //진행창
                    case "Title":
                        TitleText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "Comments":
                        CommentsText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    //case "SpecialReward":
                    //    SpecialReward[i] = objects[j].gameObject.GetComponent<Text>();
                    //    break;
                    case "RewardBox_Cash":
                        if (string.Compare(AchvList[i].achv_reward_type, "cash") == 0)
                        {
                            RewardBox[i] = objects[j].gameObject;
                            RewardBox[i].SetActive(true);
                        }
                        else
                        {
                            objects[j].gameObject.SetActive(false);
                        }
                        break;
                    case "RewardBox_Coin":
                        if (string.Compare(AchvList[i].achv_reward_type, "gold") == 0)
                        {
                            RewardBox[i] = objects[j].gameObject;
                            RewardBox[i].SetActive(true);
                        }
                        else
                        {
                            objects[j].gameObject.SetActive(false);
                        }
                        break;
                    case "Cost_Cash":
                        if (string.Compare(AchvList[i].achv_reward_type, "cash") == 0)
                        {
                            Reward_Amount[i] = objects[j].gameObject.GetComponent<Text>();
                        }
                        break;
                    case "Cost_Coin":
                        if (string.Compare(AchvList[i].achv_reward_type, "gold") == 0)
                        {
                            Reward_Amount[i] = objects[j].gameObject.GetComponent<Text>();
                        }
                        break;
                    case "Slider":
                        ProceedSlider[i] = objects[j].gameObject.GetComponent<Slider>();
                        break;
                    case "SliderText":
                        ProceedText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "PointText":
                        ScoreText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;

                    //완료창
                    case "AchievePanel_Reward":
                        RewardPanel[i] = objects[j].gameObject;
                        RewardPanel[i].SetActive(false);
                        break;
                    case "_Title":
                        TitleText_Reward[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "_Comments":
                        CommentsText_Reward[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    //case "_SpecialReward":
                    //    SpecialReward_Reward[i] = objects[j].gameObject.GetComponent<Text>();
                    //    break;
                    case "_RewardBox_Cash":
                        if (string.Compare(AchvList[i].achv_reward_type, "cash") == 0)
                        {
                            RewardBox_Reward[i] = objects[j].gameObject;
                        }
                        break;
                    case "_RewardBox_Coin":
                        if (string.Compare(AchvList[i].achv_reward_type, "gold") == 0)
                        {
                            RewardBox_Reward[i] = objects[j].gameObject;
                        }
                        break;
                    case "_Cost_Cash":
                        if (string.Compare(AchvList[i].achv_reward_type, "cash") == 0)
                        {
                            RewardAmount_Reward[i] = objects[j].gameObject.GetComponent<Text>();
                        }
                        break;
                    case "_Cost_Coin":
                        if (string.Compare(AchvList[i].achv_reward_type, "gold") == 0)
                        {
                            RewardAmount_Reward[i] = objects[j].gameObject.GetComponent<Text>();
                        }
                        break;
                    case "_PointText":
                        ScoreText_Reward[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "RewardButton":
                        RewardButton[i] = objects[j].gameObject.GetComponent<Button>();
                        break;
                }
            }
            TitleText[i].text = AchvList[i].alertText;
            StartCoroutine(UpdateData(G_AchvList[i], i));      //업적 내용 갱신
            //Check_Special_Reward(i);
            StartCoroutine(GetAchievement(AchvList[i].no, AchvList[i].type, AchvList[i].amount));   //업적 달성 대기 코루틴
        }
        #endregion

        Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        StartCoroutine(PinPanel());
    }

    void Update()
    {
        for (int i = 0; i < AchvList.Count; i++)
        {
            if (RewardPanel[i].activeSelf)
            {
                NewIcon.SetActive(true);
                break;
            }
            NewIcon.SetActive(false);
        }
        PlayerScoreText.text = GetThousandCommaText(Player.instance.getUser().achvScore);
    }


    private IEnumerator UpdateData(GameObject achv, int index) //업적 내용 업데이트
    {
        int data = 0;
        string commentsText = "";
        while (true)
        {
            switch (AchvList[index].type)
            {
                case "normal_atk":
                    commentsText = "일반 공격 " + GetThousandCommaText(AchvList[index].amount) + "회";
                    data = normal_atk_count;
                    break;
                case "critical_atk":
                    commentsText = "크리티컬 공격 " + GetThousandCommaText(AchvList[index].amount) + "회";
                    data = critical_atk_count;
                    break;
                case "gold":
                    commentsText = "골드 " + GetThousandCommaText(AchvList[index].amount) + "개 획득";
                    data = get_gold_count;
                    break;
                case "cash":
                    commentsText = "보석 " + GetThousandCommaText(AchvList[index].amount) + "개 획득";
                    data = get_cash_count;
                    break;
                case "level":
                    commentsText = "레벨 " + GetThousandCommaText(AchvList[index].amount) + " 달성";
                    data = Player.instance.getUser().level;
                    break;
                case "ore":
                    commentsText = "광석 " + GetThousandCommaText(AchvList[index].amount) + "개 제련";
                    data = ore_crash_count;
                    break;
                case "collection":
                    commentsText = "컬렉션 " + GetThousandCommaText(AchvList[index].amount) + "개 수집";
                    break;
            }
            Reward_Amount[index].text = GetThousandCommaText(AchvList[index].achv_reward_quantity);
            CommentsText[index].text = commentsText;
            int data_tmp = data;
            ProceedText[index].text = GetThousandCommaText(data_tmp) + "/" + GetThousandCommaText(AchvList[index].amount);
            ProceedSlider[index].value = (float)data_tmp / (float)AchvList[index].amount * 100;
            ScoreText[index].text = GetThousandCommaText(AchvList[index].score);

            TitleText_Reward[index].text = TitleText[index].text;
            CommentsText_Reward[index].text = CommentsText[index].text;
            //SpecialReward_Reward[index].text = SpecialReward[index].text;
            RewardAmount_Reward[index].text = Reward_Amount[index].text;
            ScoreText_Reward[index].text = ScoreText[index].text;
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    
    public void Check_Special_Reward(int index)  //업적에 포함된 칭호가 있는지 검사
    {
        SpecialReward[index].gameObject.SetActive(false);           //특수보상 숨기기
        SpecialReward_Reward[index].gameObject.SetActive(false);
        
        for (int i = 0; i < AchvList[index].special_reward.Length; i++)
        {
            if (AchvList[index].special_reward[i] != 0
                && AchvList[index].amount_for_sw[i] <= AchvList[index].amount
                && !TitleHandler.titleList[AchvList[index].special_reward[i]].isGet) //업적번호가 0번 일 경우 없음.
            {
                //업적창에 특별보상 활성화
                SpecialReward[index].text = "특별보상 : " + TitleHandler.titleList[AchvList[index].special_reward[i]].name;
                SpecialReward[index].gameObject.SetActive(true);
                SpecialReward_Reward[index].gameObject.SetActive(true);
                break;
            }
            else
            {
                SpecialReward[index].gameObject.SetActive(false);
                SpecialReward_Reward[index].gameObject.SetActive(false);
            }
        }
    }
    
    public IEnumerator GetAchievement(int index, string type, int amount) //업적 달성 시 호출되는 함수
    {
        switch (type)
        {
            case "normal_atk":
                yield return new WaitUntil(() => normal_atk_count >= amount);
                break;
            case "critical_atk":
                yield return new WaitUntil(() => critical_atk_count >= amount);
                break;
            case "gold":
                yield return new WaitUntil(() => get_gold_count >= amount);
                break;
            case "cash":
                yield return new WaitUntil(() => get_cash_count >= amount);
                break;
            case "level":
                yield return new WaitUntil(() => Player.instance.getUser().level >= amount);
                break;
            case "ore":
                yield return new WaitUntil(() => ore_crash_count >= amount);
                break;
            case "collection":
                yield return new WaitUntil(() => get_collection_count >= amount);
                break;
        }
        Debug.Log(index);
        RewardPanel[index].SetActive(true);
        G_AchvList[index].transform.SetAsFirstSibling();
        RewardButton[index].onClick.AddListener(() => ReceiveAchv(index));
        Debug.Log("달성");
        //yield return StartCoroutine(AcvBoxHandle(alertMessageHandle(type, amount)));
    }

    public string alertMessageHandle(string type, int amount)   //업적 달성 시 알림 창 내용 설정
    {
        switch (type)
        {
            case "normal_atk":
                return "일반 공격 " + GetThousandCommaText(amount) + "회 돌파!";
            case "critical_atk":
                return "크리티컬 히트 " + GetThousandCommaText(amount) + "회 돌파!";
            case "gold":
                return "누적 골드 획득량 " + GetThousandCommaText(amount) + "골드 돌파!";
            case "cash":
                return "누적 보석 획득량 " + GetThousandCommaText(amount) + "개 돌파!";
            case "level":
                return "레벨 " + GetThousandCommaText(amount) + "달성!";
            case "ore":
                return "광석 " + GetThousandCommaText(amount) + "개 격파!";
            case "colletion":
                return "콜렉션 " + GetThousandCommaText(amount) + "개 수집!";
        }
        return null;
    }
    public IEnumerator AcvBoxHandle(string text)   //업적 달성 시 알림 UI 애니메이션
    {
        yield return new WaitWhile(() => popUp);
        int time = UnityEngine.Random.Range(0, 10);
        yield return new WaitForSeconds(time / 10);
        popUp = true;
        GameObject AcvBoxPosition = AcvBox.transform.Find("UIPanel").gameObject;
        iTween.MoveTo(AcvBox, iTween.Hash("y", 75, "time", 0.1, "isLocal", true));
        iTween.MoveTo(AcvBox, iTween.Hash("y", -75, "time", 0.5, "delay", 0.5, "isLocal", true));

        AcvText.text = text;
        AcvBox.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        iTween.MoveTo(AcvBox, iTween.Hash("y", 75, "time", 0.5f, "isLocal", true, "oncomplete", "CloseAcvBox"));
        AcvBox.SetActive(false);
    }
    void CloseAcvBox()
    {
        AcvBox.SetActive(false);
        popUp = false;
    }
    
    
    public void ReceiveAchv(int r_index) //업적 보상 받기 함수
    {
        Debug.Log("받기");
        RewardButton[r_index].onClick.RemoveAllListeners();
        NewIcon.SetActive(false); //new 아이콘 비활성화

        string type = AchvList[r_index].achv_reward_type;
        int quantity = AchvList[r_index].achv_reward_quantity;
        Player.instance.GetMoney(type, quantity);
        Player.instance.getUser().achvScore += AchvList[r_index].score;
        #region 업적갱신
        AchvList[r_index].amount *= 2;  //목표 갱신
        //if (AchvList[r_index].type == "level")
        //{
        //    //만렙
        //    if(AchvList[r_index].amount == 30)
        //    {
        //        //비활성화

        //    }
        //    else if (AchvList[r_index].amount > 30) AchvList[r_index].amount = 30;
        //}
        AchvList[r_index].achv_reward_quantity *= 2; //보상 갱신
        AchvList[r_index].score *= 2;   //스코어 갱신
        StartCoroutine(GetAchievement(r_index, AchvList[r_index].type, AchvList[r_index].amount));   //갱신된 업적 완료대기
        #endregion
        RewardPanel[r_index].SetActive(false);
        //if (SpecialReward[r_index].enabled)     //획득할 칭호가 있다면
        //{
        //    for (int i = 0; i < AchvList[r_index].special_reward.Length; i++){
        //        string title = SpecialReward[r_index].text.Replace("특별보상 : ", "");
        //        if (string.Compare(title, TitleHandler.titleList[AchvList[r_index].special_reward[i]].name) == 0)
        //        {
        //            TitleHandler.titleList[AchvList[r_index].special_reward[i]].isGet = true;      //소유 여부 true값으로 변경
        //            TitleHandler.G_TitleList[AchvList[r_index].special_reward[i]].SetActive(true);
        //            StartCoroutine(AcvBoxHandle("새로운 칭호를 얻었습니다."));
        //            //New_Title_Icon.SetActive(true);    //ProfilePopup 안 new 아이콘 활성화(칭호)
        //            //TitleHandler.G_TitleList[AchvList[r_index].special_reward[i]].GetComponentsInChildren<Image>()[3].enabled = true;
        //            //TitleHandler.G_TitleList[AchvList[r_index].special_reward[i]].GetComponentsInChildren<Text>()[1].enabled = true;
        //            //RewardPanel[r_index].SetActive(false);
        //            GetComponent<TitleHandler>().ArrangeTitle();
        //            G_AchvList[r_index].transform.SetSiblingIndex(ArrangeAchv(r_index));
        //        }
        //    }
        //}
        //Check_Special_Reward(r_index);
        
    }
    public int ArrangeAchv(int index)
    {
        for (int i = 0; i < AchvList.Count; i++)
        {
            if (AchvList[i].no > index)
            {
                return AchvList[i].no;
            }
        }
        return AchvList.Count-1;
    }
    public string GetThousandCommaText(int data)    //천 단위마다 콤마 찍는 함수
    {
        if (data == 0)
        {
            return "0";
        }
        else
        {
            return string.Format("{0:#,###}", data);
        }
    }
    public int GetIntFromCommaText(string data)     //콤마 찍힌 숫자 인식
    {
        return Int32.Parse(data.Replace(",", ""));
    }
    public IEnumerator PinPanel()
    {
        while (true)
        {
            yield return new WaitUntil(() => AchievementPopup.activeInHierarchy);
            Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            yield return new WaitUntil(() => !AchievementPopup.activeInHierarchy);
        }
    }
}