using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    private Text t_PlayerGold;   //코인 텍스트
    private Text t_PlayerCash;   //보석 텍스트
    private Text t_PlayerLevel;  //프로필 패널 레벨 텍스트
    private Text t_ProfileLevel; //프로필 팝업 레벨 텍스트
    private Text t_playerExp;    //프로필 패널 경험치 텍스트
    private Slider PlayerExpBarSlider;

    private GameObject GoldPanel;
    private GameObject CashPanel;

    static public User Play = new User();          //플레이어
    static public Hammer equipHm = new Hammer();   //장착 망치

    private Animator chrAni;

    public class User
    {
        public int user_no;
        public string user_id;
        public string Name;
        public int level;
        public int max_exp;
        public int exp;
        public int gold;
        public int cash;
        public int achvScore;
        public string title;

        //능력치
        public float dps;
        public float strPower;
        public float attackSpeed;
        public float focus;
        public float critical;
        public float defPower;
        public float evaRate;
        public string attribute;
        public float collectSpeed;
        public float collectAmount;
        //장비
        public string[] equipHelmet;
        public string[] equipArmor;
        public string[] equipWeapon;
        public string[] equipBoots;

        public string logoutTime;
        
        public User(){
            this.user_no = 0;
            this.user_id = "0";
            this.Name = "대장장이 스미스";
            this.level = 1;
            this.exp = 0;
            this.max_exp = this.level * 10;
            this.gold = 500;
            this.cash = 100;
            this.achvScore = 0;
            this.title = "구리 마스터";

            
            this.strPower = 50;
            this.attackSpeed = 1.0f;
            this.focus = 50;
            this.critical = 20;
            this.defPower = 5;
            this.evaRate = 3;
            this.attribute = "no";
            this.collectSpeed = 1.0f;
            this.collectAmount = 1;
            this.dps = this.strPower + this.defPower * 0.2f;

            this.equipHelmet = new string[2];
            equipHelmet[0] = "helmet5"; this.equipHelmet[1] = "helmet6";
            this.equipArmor = new string[2];
            this.equipArmor[0] = "armor5"; this.equipArmor[1] = "armor6";
            this.equipWeapon = new string[2];
            this.equipWeapon[0] = "weapon6"; this.equipWeapon[1] = "weapon5";
            this.equipBoots = new string[2];
            this.equipBoots[0] = "boots1"; this.equipBoots[1] = "boots2";

            this.logoutTime = "0";
        }
    }
    void Awake()
    {
        t_PlayerGold = GameObject.Find("GoldText").GetComponent<Text>();
        t_PlayerCash = GameObject.Find("CashText").GetComponent<Text>();
        t_PlayerLevel = GameObject.Find("LevelText").GetComponent<Text>();
        t_ProfileLevel = GameObject.Find("Menu").gameObject.transform.Find("ProfilePopup/UIPanel/ProfilePanel/PlayerInfo/LevelText").gameObject.GetComponent<Text>(); //프로필 팝업 레벨 텍스트
        t_playerExp = GameObject.Find("ExpText").GetComponent<Text>();    //프로필 패널 경험치 텍스트
        PlayerExpBarSlider = GameObject.Find("PlayerExpSlider").GetComponent<Slider>();
        chrAni = GameObject.Find("Chr_001").GetComponent<Animator>();

        GoldPanel = GameObject.Find("Gold");
        CashPanel = GameObject.Find("Cash");


    }
    void Start()
    {
        t_PlayerGold.text = Play.gold.ToString();
        t_PlayerCash.text = Play.cash.ToString();
        PlayerExpBarSlider.maxValue = Play.max_exp;
        PlayerExpBarSlider.value = Play.exp;
    }

    void Update()
    {
        //t_PlayerGold.text = GetThousandCommaText(Play.gold);       //골드출력
        //t_PlayerCash.text = GetThousandCommaText(Play.cash);       //보석 출력
        t_PlayerLevel.text = (Play.level).ToString();              //프로필 패널 레벨 출력
        t_ProfileLevel.text = (Play.level).ToString();             //프로필 팝업 레벨 출력
        t_playerExp.text = ((int)((PlayerExpBarSlider.value / PlayerExpBarSlider.maxValue) * 100)).ToString() +"%";   //경험치 량 출력

        #region 공격 애니메이션 속도 조절
        if (chrAni.GetCurrentAnimatorStateInfo(0).IsName("attack_longin")
            || chrAni.GetCurrentAnimatorStateInfo(0).IsName("attack_long")
            || chrAni.GetCurrentAnimatorStateInfo(0).IsName("attack_long0")
            || chrAni.GetCurrentAnimatorStateInfo(0).IsName("attack_logout"))
        {
            chrAni.speed = Play.attackSpeed;
        }
        else
        {
            chrAni.speed = 1.0f;
        }
        #endregion
    }
    
    public void GetGameReward(int exp, int gold)        //제련 보상 획득
    {
        if (Play.level <= 30)
            StartCoroutine(GetExp(exp));
        //StartCoroutine(GetGold(gold));
    }
    public void getExp(int exp)
    {
        if (Play.level <= 30)
            StartCoroutine(GetExp(exp));
    }
    public IEnumerator GetExp(int exp)                  //경험치 획득 함수
    {
        float i = Play.exp;         //경험치를 얻기 전 경험치 값을 핸들에 대입
        float v = exp * 0.015f;     //경험치량 차오르는 속도
        float move_sum = 0.0f;      //슬라이더 핸들이 움직인 총 거리
        PlayerExpBarSlider.maxValue = Play.max_exp; //현재레벨 경험치 총량

        Play.exp += exp;

        while (move_sum <= exp)
        {
            i += v;
            move_sum += v;
            PlayerExpBarSlider.value = i;

            if (i >= PlayerExpBarSlider.maxValue)
            {
                i -= PlayerExpBarSlider.maxValue;           //경험치가 넘치면 핸들 값을 0으로 만들고
                Play.exp -= Play.max_exp;                   //경험치도 0으로 초기화
                Play.level += 1;                            //레벨 업
                Play.max_exp = Play.level * 20;             //경험치 총량 재 조정
                PlayerExpBarSlider.maxValue = Play.max_exp; //슬라이더 총량 재 조정

                Play.strPower += Play.strPower * 0.1f;
                Play.dps = Play.strPower + Play.defPower * 0.2f;
                MineData.instance.Unlock();                 //레벨업하면 광산 건설 잠금 해제 체크
            }
            yield return null;
        }
        PlayerExpBarSlider.value = Mathf.MoveTowards(PlayerExpBarSlider.value, Play.exp, 100); //보정
    }
    public string GetThousandCommaText(int data)
    {
        if (data == 0)
        {
            return "0";
        }
        return string.Format("{0:#,###}", data);
    }
    public void GetMoney(string type, int amount)   //골드 획득, 골드 표시
    {
        if (string.Compare(type, "gold") == 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", Player.Play.gold, "to", Player.Play.gold + amount, "onUpdate", "GoldCount", "time", 1));
            Play.gold += amount;
            StartCoroutine(TextAnimation_Gold(new GameObject(), amount));
        }
        else if (string.Compare(type, "cash") == 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", Player.Play.cash, "to", Player.Play.cash + amount, "onUpdate", "CashCount", "time", 1));
            Play.cash += amount;
            StartCoroutine(TextAnimation_Cash(new GameObject(), amount));
        }

    }
    public void LostMoney(string type, int amount)
    {
        if(string.Compare(type, "gold")==0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", Player.Play.gold, "to", Player.Play.gold - amount, "onUpdate", "GoldCount", "time", 1));
            Play.gold -= amount;
        }
        else if(string.Compare(type, "cash") == 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", Player.Play.cash, "to", Player.Play.cash - amount, "onUpdate", "CashCount", "time", 1));
            Play.cash -= amount;
        }
    }
    private void GoldCount(int num)
    {
        t_PlayerGold.GetComponent<Text>().text = num.ToString();
    }
    private void CashCount(int num)
    {
        t_PlayerCash.GetComponent<Text>().text = num.ToString();
    }
    private IEnumerator TextAnimation_Gold(GameObject text, int amount)
    {
        float fade = 1;
        text = Instantiate(t_PlayerGold.gameObject);
        text.GetComponent<Text>().text = "+" + amount;
        text.GetComponent<Text>().color = new Color(1, 1, 1);
        text.transform.SetParent(GoldPanel.transform);
        text.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        text.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        
        while (fade > 0)
        {
            text.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 5);
            text.GetComponent<Text>().color = new Color(1, 1, 1, fade);
            fade -= 0.03f;
            yield return null;
        }
        Destroy(text);
    }
    private IEnumerator TextAnimation_Cash(GameObject text, int amount)
    {
        float fade = 1;
        text = Instantiate(t_PlayerCash.gameObject);
        text.GetComponent<Text>().text = "+" + amount;
        text.GetComponent<Text>().color = new Color(1, 1, 1);
        text.transform.SetParent(CashPanel.transform);
        text.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        text.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        while (fade > 0)
        {
            text.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 5);
            text.GetComponent<Text>().color = new Color(1, 1, 1, fade);
            fade -= 0.03f;
            yield return null;
        }
        Destroy(text);
    }
}