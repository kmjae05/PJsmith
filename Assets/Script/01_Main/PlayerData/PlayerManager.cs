using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour {
    private Text t_PlayerGold;   //코인 텍스트
    private Text t_PlayerCash;   //보석 텍스트
    private Text t_PlayerLevel;  //프로필 패널 레벨 텍스트
    private Text t_ProfileLevel; //프로필 팝업 레벨 텍스트
    private Text t_playerExp;    //프로필 패널 경험치 텍스트
    private Slider PlayerExpBarSlider;

    private GameObject GoldPanel;
    private GameObject CashPanel;

    private Animator chrAni;


    void Awake()
    {
        t_PlayerGold = GameObject.Find("GoldText").GetComponent<Text>();
        t_PlayerCash = GameObject.Find("CashText").GetComponent<Text>();
        t_PlayerLevel = GameObject.Find("ProfilePanel").transform.Find("Panel/LevelText").gameObject.GetComponent<Text>();
        t_ProfileLevel = GameObject.Find("Menu").gameObject.transform.Find("ProfilePopup/UIPanel/ProfilePanel/PlayerInfo/InfoPanel/LevelText").gameObject.GetComponent<Text>(); //프로필 팝업 레벨 텍스트
        t_playerExp = GameObject.Find("ExpText").GetComponent<Text>();    //프로필 패널 경험치 텍스트
        PlayerExpBarSlider = GameObject.Find("PlayerExpSlider").GetComponent<Slider>();

        GoldPanel = GameObject.Find("Gold");
        CashPanel = GameObject.Find("Cash");

        chrAni = GameObject.Find("Chr_001").GetComponent<Animator>();

    }

    void Start()
    {
        t_PlayerGold.text = Player.instance.getUser().gold.ToString();
        t_PlayerCash.text = Player.instance.getUser().cash.ToString();
        PlayerExpBarSlider.maxValue = Player.instance.getUser().max_exp;
        PlayerExpBarSlider.value = Player.instance.getUser().exp;
    }


    void Update()
    {
            #region 공격 애니메이션 속도 조절
            if (chrAni.GetCurrentAnimatorStateInfo(0).IsName("attack_longin")
            || chrAni.GetCurrentAnimatorStateInfo(0).IsName("attack_long")
            || chrAni.GetCurrentAnimatorStateInfo(0).IsName("attack_long0")
            || chrAni.GetCurrentAnimatorStateInfo(0).IsName("attack_logout"))
            {
                chrAni.speed = 1.0f; // Play.stat.attackSpeed;
            }
            else
            {
                chrAni.speed = 1.0f;
            }
            #endregion

        t_PlayerGold.text = GetThousandCommaText(Player.instance.getUser().gold);       //골드출력
        t_PlayerCash.text = GetThousandCommaText(Player.instance.getUser().cash);       //보석 출력
        t_PlayerLevel.text = (Player.instance.getUser().level).ToString();              //프로필 패널 레벨 출력
        t_ProfileLevel.text = (Player.instance.getUser().level).ToString();             //프로필 팝업 레벨 출력
        t_playerExp.text = ((int)((PlayerExpBarSlider.value / PlayerExpBarSlider.maxValue) * 100)).ToString() + "%";   //경험치 량 출력
    }










    public IEnumerator GetExp(int exp)                  //경험치 획득 함수
    {
        float i = Player.instance.getUser().exp;         //경험치를 얻기 전 경험치 값을 핸들에 대입
        float v = exp * 0.015f;     //경험치량 차오르는 속도
        float move_sum = 0.0f;      //슬라이더 핸들이 움직인 총 거리
        PlayerExpBarSlider.maxValue = Player.instance.getUser().max_exp; //현재레벨 경험치 총량
        Player.instance.getUser().exp += exp;

        while (move_sum <= exp)
        {
            i += v;
            move_sum += v;

            PlayerExpBarSlider.value = i;
            //Debug.Log(PlayerExpBarSlider.value);
            
            if (i >= PlayerExpBarSlider.maxValue)
            {
                Debug.Log("levelup");
                i -= PlayerExpBarSlider.maxValue;           //경험치가 넘치면 핸들 값을 0으로 만들고
                Player.instance.getUser().exp -= Player.instance.getUser().max_exp;                   //
                Player.instance.getUser().level += 1;                            //레벨 업
                Player.instance.getUser().max_exp = Player.instance.getUser().level * 20;             //경험치 총량 재 조정
                PlayerExpBarSlider.maxValue = Player.instance.getUser().max_exp; //슬라이더 총량 재 조정

                Player.instance.getUser().stat.strPower += Player.instance.getUser().stat.strPower * 0.1f;
                Player.instance.getUser().stat.dps = Player.instance.getUser().stat.strPower * (float)Player.instance.getUser().stat.attackSpeed * Player.instance.getUser().stat.critical;
                MineData.instance.Unlock();                 //레벨업하면 광산 건설 잠금 해제 체크
                GameObject.Find("PlayerManager").GetComponent<StatData>().playerStatCal();
                GameObject.Find("PlayerManager").GetComponent<StatData>().mercenaryStatCal();
                GameObject.Find("PlayerManager").GetComponent<StatData>().repreSetStatCal();
                GameObject.Find("System").transform.Find("LevelupPopup/UIPanel/LevelText").gameObject.GetComponent<Text>().text = Player.instance.getUser().level.ToString();
                GameObject.Find("System").transform.Find("LevelupPopup").gameObject.GetComponent<LevelupPopupManager>().appear();

            }
            yield return null;
        }
        PlayerExpBarSlider.value = Mathf.MoveTowards(PlayerExpBarSlider.value, Player.instance.getUser().exp, 100); //보정
    }


    public string GetThousandCommaText(int data)
    {
        if (data == 0)
        {
            return "0";
        }
        return string.Format("{0:#,###}", data);
    }

    public void GoldCount(int num)
    {
        t_PlayerGold.GetComponent<Text>().text = num.ToString();
    }
    public void CashCount(int num)
    {
        t_PlayerCash.GetComponent<Text>().text = num.ToString();
    }
    public IEnumerator TextAnimation_Gold(GameObject text, int amount)
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
    public IEnumerator TextAnimation_Cash(GameObject text, int amount)
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
