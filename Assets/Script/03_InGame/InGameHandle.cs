using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class InGameHandle : MonoBehaviour {
    private GameObject UPBox;
    private GameObject SkillButton;
    private Slider HpSlider;
    private Slider FeverSlider;
    private Text OreNameText;
    private Image frame;
    private Image OreImage;
    private Text TimeText;

    private Animator CharAni;
    private Image FadeImage;
    private GameObject ReadyGo;
    private GameObject RewardPopup;

    private Text _ScoreText;
    private Text ScoreText;
    private GameObject GoldBox;
    private Button AutoRestart;

    private Image CoolTimeImage;

    GameObject systemPopup;

    static public int ore_hp = 0;
    static public int feverGauge = 0;
    static public bool fever;
    private int ore_gold;
    private int ore_exp;
    private float leftTime = 30.0f;

    void Awake()
    {
        SceneData.Scenename = SceneManager.GetActiveScene().name;

        //UPBox = GameObject.Find("UPBox");
        //SkillButton = GameObject.Find("SkillButton");
        HpSlider = GameObject.Find("HPBar").GetComponent<Slider>();
        //FeverSlider = GameObject.Find("FeverBar").GetComponent<Slider>();
        OreNameText = GameObject.Find("OreText").GetComponent<Text>();
        frame = GameObject.Find("OreBox").GetComponent<Image>();
        OreImage = GameObject.Find("OreIcon").GetComponent<Image>();
        TimeText = GameObject.Find("TimeText").GetComponent<Text>();
        CharAni = GameObject.Find("Chr_001").GetComponent<Animator>();
        FadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        ReadyGo = GameObject.Find("Popup").transform.Find("ReadyGo").gameObject;

        //_ScoreText = GameObject.Find("UPBox").transform.Find("Text").GetComponent<Text>();
        //ScoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        GoldBox = GameObject.Find("GoldBox");

        //CoolTimeImage = GameObject.Find("NormalSkill").transform.Find("SkillLock").GetComponent<Image>();

        //AutoRestart = GameObject.Find("AutoPlayStop").GetComponent<Button>();

        systemPopup = GameObject.Find("Popup").transform.Find("SystemPopup").gameObject;
    }

	void Start ()
    {
        SceneData.Scenename = SceneManager.GetActiveScene().name;
        //UPBox.SetActive(false);
        //SkillButton.SetActive(false);

        if (Player.instance.getUser().isOre)
        {
            OreSelect.Ore TargetOre = OreSelect.SelectOre;
            OreNameText.text = TargetOre.name;
            OreImage.sprite = OreSelect.Icon[OreSelect.SelectOre.no].sprite;
            HpSlider.maxValue = TargetOre.hp;
            ore_hp = TargetOre.hp;
            ore_gold = TargetOre.gold;
            ore_exp = TargetOre.exp;
            //FeverSlider.maxValue = 100;
            fever = false;

            //게임 시작 전 HP와 시간, 피버게이지를 초기화
            HpSlider.value = ore_hp;
            TimeText.text = ((int)leftTime).ToString();
            feverGauge = 0;
            //FeverSlider.value = feverGauge;

            //제련 상태
            Player.instance.getUser().ingameState = true;
            Player.instance.getUser().oreName = TargetOre.name;
            Player.instance.getUser().TargetOre = OreSelect.SelectOre;
            Player.instance.getUser().oreexp = OreSelect.SelectOre.exp;
        }
        else
        {
            OreNameText.text = Player.instance.getUser().equipName;
            Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == Player.instance.getUser().equipName).grade);
            frame.color = col;
            OreImage.sprite = Resources.Load<Sprite>( ThingsData.instance.getThingsList().Find(x=>x.name == Player.instance.getUser().equipName).icon);
            HpSlider.maxValue = Player.instance.getUser().equipmaxhp;
            ore_hp = Player.instance.getUser().equipmaxhp;
            ore_gold = 0;
            ore_exp = Player.instance.getUser().equipexp;
            fever = false;

            //게임 시작 전 HP와 시간, 피버게이지를 초기화
            HpSlider.value = ore_hp;
            TimeText.text = ((int)leftTime).ToString();

            //제작 상태
            Player.instance.getUser().equipState = true;
        }

        StartCoroutine(GameStart());

        //무한모드가 아니라면 점수창 끔
        //_ScoreText.gameObject.SetActive(false);
        //ScoreText.gameObject.SetActive(false);

        //연속 제련 상태가 아니라면 연속 제련 버튼 끔
        //AutoRestart.gameObject.SetActive(false);

        GoldBox.transform.Find("GoldText").GetComponent<Text>().text = Player.instance.getUser().gold.ToString();

        
    }
    //#region UIAnimation
    //void UIAnimation_01()
    //{
    //    UPBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 900);
    //    UPBox.SetActive(true);
    //    iTween.MoveTo(UPBox, iTween.Hash("y", 0, "time", 0.5, "delay", 1.0, "islocal", true));
    //}
    //void UIAnimation_02()
    //{
    //    SkillButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(1740, -465);
    //    SkillButton.SetActive(true);
    //    iTween.MoveTo(SkillButton, iTween.Hash("x", 805, "time", 0.5, "delay", 1.0, "islocal", true));
    //}
    //void UIAnimation_03()
    //{
    //    GoldBox.GetComponent<RectTransform>().anchoredPosition = new Vector2(-280, 84);
    //    GoldBox.SetActive(true);
    //    iTween.MoveTo(GoldBox, iTween.Hash("x", -1005, "time", 0.5, "delay", 1.0, "islocal", true));
    //}
    //void UIAnimation_01_()
    //{
    //    iTween.MoveTo(UPBox, iTween.Hash("y", 900, "time", 0.5, "islocal", true));
    //}
    //void UIAnimation_02_()
    //{
    //    iTween.MoveTo(SkillButton, iTween.Hash("x", 1740, "time", 0.5, "islocal", true));
    //}
    //void UIAnimation_03_()
    //{
    //    iTween.MoveTo(GoldBox, iTween.Hash("x", -1560, "time", 0.5, "islocal", true));
    //}
    //#endregion

    IEnumerator GameStart()
    {
        //UIAnimation_01();
        //UIAnimation_02();
        //UIAnimation_03();
        yield return StartCoroutine(FadeIn());
        while (true)
        {
            leftTime -= Time.deltaTime;
            TimeText.text = ((int)leftTime).ToString();
            if (feverGauge >= 100)
            {
                feverGauge = 100;
                if (!fever)
                {
                    fever = true;
                    FeverMode();
                }
            }

            Player.instance.getUser().orehp = ore_hp;
            Player.instance.getUser().equiphp = ore_hp;
            Player.instance.getUser().oretime = leftTime;
            Player.instance.getUser().equiptime = leftTime;

            //광석 hp 0
            if (ore_hp <= 0)
            {
                Achievementhandle.ore_crash_count++;
                Time.timeScale = 1.0f;

                ore_hp = 0;
                CharAni.SetBool("complete_win", true);
                CharAni.SetBool("Atk_State", false);

                if (systemPopup.activeInHierarchy) systemPopup.SetActive(false);
                GameObject.Find("InGameUI").transform.Find("CloseButton").gameObject.SetActive(false);
                //UIAnimation_01_();
                //UIAnimation_02_();
                //UIAnimation_03_();
                Player.instance.getUser().ingameState = false;
                Player.instance.getUser().equipState = false;
                break;
            }
            //타임오버
            if (leftTime <= 0)
            {
                leftTime = 0;
                CharAni.speed = 1.0f;
                CharAni.SetBool("complete_lose", true);
                CharAni.SetBool("Atk_State", false);

                if (systemPopup.activeInHierarchy) systemPopup.SetActive(false);
                GameObject.Find("InGameUI").transform.Find("CloseButton").gameObject.SetActive(false);
                Player.instance.getUser().ingameState = false;
                Player.instance.getUser().equipState = false;
                break;
            }
            yield return null;
        }
    }

    void FeverMode()
    {
        CharAni.speed = 2.0f;
        //iTween.ValueTo(gameObject, iTween.Hash("from", FeverSlider.value, "to", 0, "onUpdate", "SetFeverGauge", "time", 5,"oncomplete", "NormalMode"));
    }

    void NormalMode()
    {
        CharAni.speed = (float)Player.instance.getUser().stat.attackSpeed;
        feverGauge = 0; 
        fever = false;
    }

    void SetFeverGauge(float num)
    {
        //FeverSlider.value = num;
    }

    public void ActiveSkill()
    {
        //if (CoolTimeImage.gameObject.activeInHierarchy)
        //{
        //    return;
        //}
        CharAni.Play("attack_chargein");
        StartCoroutine(CoolDownTime());
    }
    IEnumerator CoolDownTime()
    {
        float cooltime = 15.0f;
        //CoolTimeImage.gameObject.SetActive(true);
        while (cooltime > 0 && CharAni.GetBool("Atk_State"))
        {
            cooltime -= Time.deltaTime;
            yield return null;
        }
        //CoolTimeImage.gameObject.SetActive(false);
    }

    public void setTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    IEnumerator FadeIn()
    {
        FadeImage.gameObject.SetActive(true);
        for (float fade = 1.0f; fade >= 0; fade -= 0.017f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        FadeImage.gameObject.SetActive(false);
        CharAni.SetBool("Atk_State", true); 
        ReadyGo.SetActive(true);
        yield return new WaitUntil(() => ReadyGo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        ReadyGo.SetActive(false);
        CharAni.SetTrigger("Atk_Start");
    }
    IEnumerator FadeOut()
    {
        FadeImage.gameObject.SetActive(true);
        for (float fade = 0.0f; fade < 0.5f; fade += 0.02f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        SceneManager.LoadScene("09_Loading_Normal");
    }

    
    public void Go_Main()
    {
        StartCoroutine(FadeOut());
    }
}
