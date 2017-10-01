using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;
using System.IO;

public class MailHandle : MonoBehaviour
{
    private GameObject MailPopup;
    private GameObject Panel;
    private GameObject defaultPost;
    private GameObject emptyText;
    private WWW reader;
    private JsonData MailData;
    private Button ReceiveAllButton;
    private GameObject ReceiveTextBox;
    private GameObject AlertIcon;
    private Image GoldIcon;
    private Image CashIcon;

    private List<GameObject> G_mailList = new List<GameObject>();
    private Text[] TitleText;
    private Text[] CommentText;
    private Image[] Icon;
    private Text[] Reward_Type;
    private Text[] Reward_Amount;
    private Text[] DateText;

	void Start ()
    {
        MailPopup = GameObject.Find("Menu").transform.Find("MailPopup").gameObject;
        Panel = MailPopup.transform.Find("UIPanel/Scroll/Panel").gameObject;
        defaultPost = Panel.transform.Find("PostPanel").gameObject;
        defaultPost.SetActive(false);
        emptyText = MailPopup.transform.Find("UIPanel/EmptyText").gameObject;
        ReceiveAllButton = MailPopup.transform.Find("UIPanel/ReceiveAllButton").gameObject.GetComponent<Button>();
        ReceiveTextBox = MailPopup.transform.Find("UIPanel/ReceiveTextBox").gameObject;
        AlertIcon = GameObject.Find("NewIcon_Mail");
        GoldIcon = GameObject.Find("GoldIcon").GetComponent<Image>();
        CashIcon = GameObject.Find("CashIcon").GetComponent<Image>();

        RefreshMailList();
        StartCoroutine(PinPanel());
	}

    void Update()
    {
        if (G_mailList.Count == 0)
        {
            emptyText.SetActive(true);
            AlertIcon.SetActive(false); //알림 아이콘 비활성화
        }
        else
        {
            emptyText.SetActive(false);
            AlertIcon.SetActive(true);  //알림 아이콘 활성화
        }
    }

    public void RefreshMailList()       //메일함 갱신
    {
        for (int i = 0; i < G_mailList.Count; i++)
        {
            Destroy(G_mailList[i]);
        }
        G_mailList.Clear();
        #region 메일 관련 Json Data 읽기
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Mailtest.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            MailData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Mailtest.json");
            MailData = JsonMapper.ToObject(tmp);
        }
        #endregion

        int mailCount = MailData["Post"].Count;

        #region 객체 정보 GameObject 생성
        TitleText = new Text[mailCount];
        CommentText = new Text[mailCount];
        Icon = new Image[mailCount];
        Reward_Type = new Text[mailCount];
        Reward_Amount = new Text[mailCount];
        DateText = new Text[mailCount];

        for (int i = 0; i < mailCount; i++)
        {
            G_mailList.Add(Instantiate(defaultPost));
            Transform[] objects = G_mailList[i].GetComponentsInChildren<Transform>();

            for (int j = 0; j < objects.Length; j++)
            {
                switch (objects[j].name)
                {
                    case "Title":
                        TitleText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "Comments":
                        CommentText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "Icon":
                        Icon[i] = objects[j].gameObject.GetComponent<Image>();
                        break;
                    case "Type":
                        Reward_Type[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "Quantity":
                        Reward_Amount[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                    case "Date":
                        DateText[i] = objects[j].gameObject.GetComponent<Text>();
                        break;
                }
            }
            Reward_Type[i].text = MailData["Post"][i]["mail_reward_type"].ToString();
            TitleText[i].text = MailData["Post"][i]["title"].ToString();
            CommentText[i].text = MailData["Post"][i]["comments"].ToString();

            switch (Reward_Type[i].text)
            {
                case "0":
                    Icon[i].color = new Color(0, 0, 0, 0);   
                    break;
                case "gold":
                    Icon[i].color = new Color(1, 1, 1, 1);
                    Icon[i].sprite = GoldIcon.sprite;
                    break;
                case "cash":
                    Icon[i].color = new Color(1, 1, 1, 1);
                    Icon[i].sprite = CashIcon.sprite;
                    break;
                default:
                    break;
            }
            Reward_Amount[i].text = GetThousandCommaText((int)MailData["Post"][i]["mail_reward_quantity"]);

            int index = i;
            SetBttnListener(G_mailList[i].GetComponentInChildren<Button>(), G_mailList[i]);
            G_mailList[i].SetActive(true);
            G_mailList[i].transform.SetParent(Panel.transform, false);
        }
        #endregion
        CheckMailTime();    //메일 시간 체크
        ReceiveAllButton.onClick.AddListener(()=>ReceiveAllMail(G_mailList)); //모두받기 버튼 이벤트 트리거
    }

    public void SetBttnListener(Button bttn, GameObject mail)
    {
        bttn.onClick.AddListener(() => ReceiveMail(mail));
    }

    public void ReceiveMail(GameObject Mail)    //메일 받기
    {
        string type = Mail.transform.Find("IconBox/Type").gameObject.GetComponent<Text>().text;
        if (type != "0")
        {
            int amount = GetIntFromCommaText(Mail.transform.Find("IconBox/Quantity").gameObject.GetComponent<Text>().text);
            if (string.Compare(type, "gold") == 0)  //골드
            {
                GetComponent<Player>().GetMoney(type, amount);
                Achievementhandle.get_gold_count += amount;
                StartCoroutine(PrintRcvText("골드", amount));
            }
            else if (string.Compare(type, "cash") == 0) //보석
            {
                GetComponent<Player>().GetMoney(type, amount);
                Achievementhandle.get_cash_count += amount;
                StartCoroutine(PrintRcvText("보석", amount));
            }
        }
        Destroy(Mail);
        G_mailList.Remove(Mail);
    }

    public void ReceiveAllMail(List<GameObject> r_mailList) //메일 모두받기
    {
        int gold = 0;
        int cash = 0;
        string text = "";
        for (int i = r_mailList.Count - 1; i >= 0; i--)
        {
            string type = r_mailList[i].transform.Find("IconBox/Type").gameObject.GetComponent<Text>().text;
            if (type != "0")
            {
                int quantity = GetIntFromCommaText(r_mailList[i].transform.Find("IconBox/Quantity").gameObject.GetComponent<Text>().text);
                if (string.Compare(type,"gold") == 0)
                {
                    Achievementhandle.get_gold_count += quantity;
                    gold += quantity;
                }
                else if (string.Compare(type,"cash") == 0)
                {
                    Achievementhandle.get_cash_count += quantity;
                    cash += quantity;
                }
            }
            for (int j = 0; j < G_mailList.Count; j++)
            {
                if (r_mailList[i] == G_mailList[j])
                {
                    Destroy(r_mailList[i]);
                    G_mailList.Remove(G_mailList[j]);
                }
            }
        }
        GetComponent<Player>().GetMoney("gold", gold);  //골드 획득
        GetComponent<Player>().GetMoney("cash", cash);  //보석 획득

        if (gold != 0 && cash != 0)
        {
            text += "골드 " + gold + "개,\n" + "보석 " + cash + "개를 받았습니다.";
        }
        else if (gold ==0 && cash != 0)
        {
            text += "보석 " + cash + "개를 받았습니다.";
        }
        else if (gold != 0 && cash == 0)
        {
            text += "골드 " + gold + "개를 받았습니다.";
        }
        else
        {
            return;
        }

        StartCoroutine(PrintRcvText(text));
    }

    
    public IEnumerator PrintRcvText(string type, int quantity)   //메일 받을 때 출력되는 텍스트
    {
        float fade = 1.0f;
        float fade_img = 0.6f;
        float yPos = 110.0f;  
        ReceiveTextBox.GetComponentInChildren<Text>().text = type + " " + quantity + "개를 받았습니다.";
        ReceiveTextBox.GetComponentInChildren<Text>().color = new Vector4(1.0f, 1.0f, 1.0f, fade);
        ReceiveTextBox.GetComponentInChildren<Image>().color = new Vector4(0.0f, 0.0f, 0.0f, fade_img);
        ReceiveTextBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-13.35f, yPos);
        ReceiveTextBox.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        while (fade > 0)
        {
            ReceiveTextBox.GetComponentInChildren<Text>().color = new Vector4(1.0f, 1.0f, 1.0f, fade);
            ReceiveTextBox.GetComponentInChildren<Image>().color = new Vector4(0.0f, 0.0f, 0.0f, fade_img);
            ReceiveTextBox.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-13.35f, yPos);

            fade -= 0.15f;
            fade_img -= 0.09f;
            yPos += 1.7f;
            yield return null;
        }
        ReceiveTextBox.SetActive(false);
    }

    public IEnumerator PrintRcvText(string text)
    {
        float fade = 1.0f;
        float fade_img = 0.6f;
        float yPos = 110.0f;
        ReceiveTextBox.GetComponentInChildren<Text>().text = text;
        ReceiveTextBox.GetComponentInChildren<Text>().color = new Vector4(1.0f, 1.0f, 1.0f, fade);
        ReceiveTextBox.GetComponentInChildren<Image>().color = new Vector4(0.0f, 0.0f, 0.0f, fade_img);
        ReceiveTextBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-13.35f, yPos);
        ReceiveTextBox.SetActive(true);
        yield return new WaitForSeconds(0.8f);

        while (fade > 0)
        {
            ReceiveTextBox.GetComponentInChildren<Text>().color = new Vector4(1.0f, 1.0f, 1.0f, fade);
            ReceiveTextBox.GetComponentInChildren<Image>().color = new Vector4(0.0f, 0.0f, 0.0f, fade_img);
            ReceiveTextBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-13.35f, yPos);

            fade -= 0.15f;
            fade_img -= 0.09f;
            yPos += 1.7f;
            yield return null;
        }
        ReceiveTextBox.SetActive(false);
    }
    
    public void CheckMailTime()  //메일 시간체크
    {
        for (int i = G_mailList.Count-1; i >= 0; i--)
        {
            DateTime mailTime = DateTime.Parse(MailData["Post"][i]["date"].ToString());
            DateTime deadTime = mailTime + new TimeSpan(7, 0, 0, 0);
            TimeSpan leftTime = deadTime - DateTime.Now;    //남은 시간 계산
                    
            if (leftTime.Days <= 0)
            {
                DateText[i].text = leftTime.Hours + "시간 남음";
                if (leftTime.Hours <= 0)
                {
                    Destroy(G_mailList[i]);
                    G_mailList.Remove(G_mailList[i]);
                }
            }
            else
            {
                DateText[i].text = leftTime.Days + "일" + leftTime.Hours + "시간 남음";
            }
        }
    }
    public string GetThousandCommaText(int data)    //천 단위마다 콤마 찍는 함수
    {
        return string.Format("{0:#,###}", data);
    }
    public int GetIntFromCommaText(string data)  //콤마 찍힌 숫자 인식
    {
        return Int32.Parse(data.Replace(",", ""));
    }

    public IEnumerator PinPanel()
    {
        while (true)
        {
            yield return new WaitUntil(()=>MailPopup.activeInHierarchy);
            Panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -540);
            yield return new WaitUntil(() => !MailPopup.activeInHierarchy);
        }
    }
}