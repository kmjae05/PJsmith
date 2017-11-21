using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    private GameObject QuestPopup;

    private Text stateText;

    private GameObject scroll;

    private GameObject panel1;
    private GameObject panel2;

    private GameObject questBox;    //복사할 객체
    private Image questBoxFrame;
    private Image questBoxIcon;
    private Text questBoxTitle;     //퀘스트 제목
    private Text questBoxComments;  //퀘스트 설명
    private Slider questBoxSlider;
    private Text questBoxSliderText;
    private Image questBoxRewardFrame;
    private Image questBoxRewardIcon;
    private Text questBoxRewardText;
    private GameObject questBoxRewardButtonImage;       //setActive
    private Image questBoxRewardButtonFrame;
    private Image questBoxRewardButtonIcon;
    private Text questBoxRewardButtonText;
    private Button questBoxRewardButton;            //보상 받기 버튼
    private GameObject questBoxCompleteImage;       //완료 이미지

    private List<GameObject> questObj;
    private List<GameObject> questWeeklyObj;
    private List<Quest> questList;
    private List<Quest> weeklyQuestList;

    private void Start()
    {
        QuestPopup = GameObject.Find("Menu").transform.Find("QuestPopup").gameObject;

        stateText = QuestPopup.transform.Find("UIPanel/StateBox/StateText").gameObject.GetComponent<Text>();

        scroll = QuestPopup.transform.Find("UIPanel/Scroll").gameObject;
        panel1 = scroll.transform.Find("Panel_1").gameObject;
        panel2 = scroll.transform.Find("Panel_2").gameObject;

        questBox = panel1.transform.Find("QuestBox").gameObject;
        questBoxFrame = questBox.transform.Find("IconBox/Frame").gameObject.GetComponent<Image>();
        questBoxIcon = questBox.transform.Find("IconBox/Icon").gameObject.GetComponent<Image>();
        questBoxTitle = questBox.transform.Find("Title").gameObject.GetComponent<Text>();
        questBoxComments = questBox.transform.Find("Comments").gameObject.GetComponent<Text>();
        questBoxSlider = questBox.transform.Find("Slider").gameObject.GetComponent<Slider>();
        questBoxSliderText = questBox.transform.Find("Slider/SliderText").gameObject.GetComponent<Text>();
        questBoxRewardFrame = questBox.transform.Find("RewardList/RewardBox/Frame").gameObject.GetComponent<Image>();
        questBoxRewardIcon = questBox.transform.Find("RewardList/RewardBox/Icon").gameObject.GetComponent<Image>();
        questBoxRewardText = questBox.transform.Find("RewardList/RewardBox/Text").gameObject.GetComponent<Text>();
        questBoxRewardButtonImage = questBox.transform.Find("RewardButtonImage").gameObject;
        questBoxRewardButtonFrame = questBoxRewardButtonImage.transform.Find("RewardBox/Frame").gameObject.GetComponent<Image>();
        questBoxRewardButtonIcon = questBoxRewardButtonImage.transform.Find("RewardBox/Icon").gameObject.GetComponent<Image>();
        questBoxRewardButtonText = questBoxRewardButtonImage.transform.Find("RewardBox/Text").gameObject.GetComponent<Text>();
        questBoxRewardButton = questBoxRewardButtonImage.transform.Find("RewardButton").gameObject.GetComponent<Button>();
        questBoxCompleteImage = questBox.transform.Find("CompleteImage").gameObject;

        questObj = new List<GameObject>();
        questWeeklyObj = new List<GameObject>();
        questList = QuestData.instance.getQuestList();
        weeklyQuestList = QuestData.instance.getWeeklyQuestList();

        scroll.GetComponent<ScrollRect>().content = panel1.GetComponent<RectTransform>();

        GameObject.Find("BrazierButton").transform.Find("BrazierMiniButton/QuestButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            panel1.SetActive(true); SwitchScrollPanel(1);
        });

        
        //일일 퀘스트 목록
        for(int i = 0; i < questList.Count; i++)
        {
            if (questList[i].reward_name == "gold" || questList[i].reward_name == "cash")
            {
                questBoxFrame.color = new Color(1, 1, 1);
                questBoxRewardFrame.color = new Color(1, 1, 1);
                questBoxRewardIcon.sprite = Resources.Load<Sprite>("Icon/" + questList[i].reward_name);
                questBoxRewardButtonFrame.color = new Color(1, 1, 1);
                questBoxRewardButtonIcon.sprite = Resources.Load<Sprite>("Icon/" + questList[i].reward_name);
            }
            else
            {
                questBoxFrame.color = new Color(1, 1, 1);
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == questList[i].reward_name).grade);
                questBoxRewardFrame.color = col;
                questBoxRewardIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x=>x.name == questList[i].reward_name).icon);
                questBoxRewardButtonFrame.color = col;
                questBoxRewardButtonIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == questList[i].reward_name).icon);
            }
            //questBoxIcon
            questBoxTitle.text = questList[i].alertText;

            int amount = 0;
            switch (questList[i].type)
            {
                case "login":
                    questBoxComments.text = "로그인 " + questList[i].amount + "회 하기";
                    amount = QuestData.questLogin;
                    questBoxIcon.sprite = Resources.Load<Sprite>("Icon/Smithy");
                    break;
                case "hunting":
                    if (questList[i].target == null)
                    {
                        int random = Random.Range(0, 4);
                        if (random == 0) questList[i].target = "몬스터 아무거나";
                        if (random == 1) questList[i].target = "전갈";
                        if (random == 2) questList[i].target = "오쿰";
                        if (random == 3) questList[i].target = "인큐버스";
                    }
                    questBoxComments.text = questList[i].target + " " + questList[i].amount + "마리 처치하기";
                    amount = QuestData.questHunting;
                    questBoxIcon.sprite = Resources.Load<Sprite>("Icon/hunting_icon");
                    break;
                case "refine":
                    questBoxComments.text = "제련 " + questList[i].amount + "회 하기";
                    amount = QuestData.questRefine;
                    questBoxIcon.sprite = Resources.Load<Sprite>("Hammer/hm_t_06");
                    break;
                case "reinforcement":
                    questBoxComments.text = "강화 " + questList[i].amount + "회 하기";
                    amount = QuestData.questReinforcement;
                    break;
                case "production":
                    questBoxComments.text = "제작 " + questList[i].amount + "회 하기";
                    amount = QuestData.questProduction;
                    break;
                case "plunder":
                    questBoxComments.text = "약탈 " + questList[i].amount + "회 하기";
                    amount = QuestData.questPlunder;
                    questBoxIcon.sprite = Resources.Load<Sprite>("Icon/plunder_icon");
                    break;
            }
            questBoxSlider.maxValue = questList[i].amount;
            questBoxSlider.value = amount;
            questBoxSliderText.text = amount + "/" + questList[i].amount;
            if (amount > questList[i].amount) questBoxSliderText.text = questList[i].amount + "/" + questList[i].amount;
            questBoxRewardText.text = "x" + questList[i].reward_quantity;
            questBoxRewardButtonText.text = "x" + questList[i].reward_quantity;


            questObj.Add(Instantiate(questBox));
            questObj[i].SetActive(true);
            questObj[i].transform.SetParent(panel1.transform, false);


            //보상을 받은 경우
            if (questList[i].rewardFlag)
            {
                questObj[i].transform.Find("CompleteImage").gameObject.SetActive(true);
                questObj[i].transform.SetAsLastSibling();
            }
            else questObj[i].transform.Find("CompleteImage").gameObject.SetActive(false);

            //완료했을 경우
            if (amount >= questList[i].amount && !questList[i].rewardFlag && !questList[i].completeFlag)
            {
                questList[i].completeFlag = true;
                GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(questList[i].alertText + " 퀘스트를 완료했습니다.");
                
            }
            if (questList[i].completeFlag)
            {
                questObj[i].transform.Find("RewardButtonImage").gameObject.SetActive(true);
                questObj[i].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                questObj[i].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    getReward(num);
                });
            }
            StartCoroutine(UpdateData(i));      //일일 퀘스트 내용 갱신
        }


        //주간 퀘스트 목록
        for (int i = 0; i < weeklyQuestList.Count; i++)
        {
            if (weeklyQuestList[i].reward_name == "gold" || weeklyQuestList[i].reward_name == "cash")
            {
                questBoxFrame.color = new Color(1, 1, 1);
                questBoxRewardFrame.color = new Color(1, 1, 1);
                questBoxRewardIcon.sprite = Resources.Load<Sprite>("Icon/" + weeklyQuestList[i].reward_name);
                questBoxRewardButtonFrame.color = new Color(1, 1, 1);
                questBoxRewardButtonIcon.sprite = Resources.Load<Sprite>("Icon/" + weeklyQuestList[i].reward_name);
            }
            else
            {
                questBoxFrame.color = new Color(1, 1, 1);
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == weeklyQuestList[i].reward_name).grade);
                questBoxRewardFrame.color = col;
                questBoxRewardIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == weeklyQuestList[i].reward_name).icon);
                questBoxRewardButtonFrame.color = col;
                questBoxRewardButtonIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == weeklyQuestList[i].reward_name).icon);
            }
            //questBoxIcon
            questBoxTitle.text = weeklyQuestList[i].alertText;

            int amount = 0;
            switch (weeklyQuestList[i].type)
            {
                case "hunting":
                    questBoxComments.text = "몬스터 " + weeklyQuestList[i].amount + "마리 처치하기";
                    amount = QuestData.questWeeklyHunting;
                    questBoxIcon.sprite = Resources.Load<Sprite>("Icon/hunting_icon");
                    break;
                case "refine":
                    questBoxComments.text = "제련 " + weeklyQuestList[i].amount + "회 하기";
                    amount = QuestData.questRefine;
                    questBoxIcon.sprite = Resources.Load<Sprite>("Hammer/hm_t_06");
                    break;
                case "reinforcement":
                    questBoxComments.text = "강화 " + weeklyQuestList[i].amount + "회 하기";
                    amount = QuestData.questReinforcement;
                    break;
                case "production":
                    questBoxComments.text = "제작 " + weeklyQuestList[i].amount + "회 하기";
                    amount = QuestData.questProduction;
                    break;
                case "plunder":
                    questBoxComments.text = "약탈 " + weeklyQuestList[i].amount + "회 하기";
                    amount = QuestData.questPlunder;
                    questBoxIcon.sprite = Resources.Load<Sprite>("Icon/plunder_icon");
                    break;
            }
            questBoxSlider.maxValue = weeklyQuestList[i].amount;
            questBoxSlider.value = amount;
            questBoxSliderText.text = amount + "/" + weeklyQuestList[i].amount;
            if (amount > weeklyQuestList[i].amount) questBoxSliderText.text = weeklyQuestList[i].amount + "/" + weeklyQuestList[i].amount;
            questBoxRewardText.text = "x" + weeklyQuestList[i].reward_quantity;
            questBoxRewardButtonText.text = "x" + weeklyQuestList[i].reward_quantity;


            questWeeklyObj.Add(Instantiate(questBox));
            questWeeklyObj[i].SetActive(true);
            questWeeklyObj[i].transform.SetParent(panel2.transform, false);


            //보상을 받은 경우
            if (weeklyQuestList[i].rewardFlag)
            {
                questWeeklyObj[i].transform.Find("CompleteImage").gameObject.SetActive(true);
                questWeeklyObj[i].transform.SetAsLastSibling();
            }
            else questWeeklyObj[i].transform.Find("CompleteImage").gameObject.SetActive(false);

            //완료했을 경우
            if (amount >= weeklyQuestList[i].amount && !weeklyQuestList[i].rewardFlag && !weeklyQuestList[i].completeFlag)
            {
                weeklyQuestList[i].completeFlag = true;
                GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(weeklyQuestList[i].alertText + " 주간 퀘스트를 완료했습니다.");
                
            }
            if (weeklyQuestList[i].completeFlag)
            {
                questWeeklyObj[i].transform.Find("RewardButtonImage").gameObject.SetActive(true);
                questWeeklyObj[i].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                questWeeklyObj[i].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
                    getReward(num);
                });
            }
            StartCoroutine(UpdateWeeklyData(i));      //일일 퀘스트 내용 갱신
        }



        StartCoroutine(TimeRemaining());


    }

    //일일 퀘스트
    private IEnumerator UpdateData(int index)
    {
        while (true)
        {
            int amount = 0;
            switch (questList[index].type)
            {
                case "login":
                    amount = QuestData.questLogin;
                    break;
                case "hunting":
                    amount = QuestData.questHunting;
                    break;
                case "refine":
                    amount = QuestData.questRefine;
                    break;
                case "reinforcement":
                    amount = QuestData.questReinforcement;
                    break;
                case "production":
                    amount = QuestData.questProduction;
                    break;
                case "plunder":
                    amount = QuestData.questPlunder;
                    break;
            }
            questObj[index].transform.Find("Slider").gameObject.GetComponent<Slider>().maxValue = questList[index].amount;
            questObj[index].transform.Find("Slider").gameObject.GetComponent<Slider>().value = amount;
            questObj[index].transform.Find("Slider/SliderText").gameObject.GetComponent<Text>().text = amount + "/" + questList[index].amount;
            if (amount > questList[index].amount)
                questObj[index].transform.Find("Slider/SliderText").gameObject.GetComponent<Text>().text 
                    = questList[index].amount + "/" + questList[index].amount;

            //보상을 받은 경우
            if (questList[index].rewardFlag)
            {
                questObj[index].transform.Find("CompleteImage").gameObject.SetActive(true);
                questObj[index].transform.SetAsLastSibling();
            }
            else questObj[index].transform.Find("CompleteImage").gameObject.SetActive(false);

            //완료했을 경우
            if (amount >= questList[index].amount && !questList[index].rewardFlag && !questList[index].completeFlag)
            {
                Debug.Log(index);

                //알림
                questList[index].completeFlag = true;
                GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(questList[index].alertText + " 퀘스트를 완료했습니다.");
                questObj[index].transform.SetAsFirstSibling();
                questObj[index].transform.Find("RewardButtonImage").gameObject.SetActive(true);
                questObj[index].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                int num = index;
                questObj[index].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
                    getReward(num);
                });
            }

            //state 완료 현황
            if (panel1.activeInHierarchy)
            {
                int compl = questList.FindAll(x => x.rewardFlag == true).Count;
                stateText.text = compl + " / " + questList.Count;
            }

            //느낌표
            if (questList[index].completeFlag && !questList[index].rewardFlag)
            {
                GameObject.Find("MenuButton").transform.Find("BrazierButton/BrazierMiniButton/QuestButton/NewIcon").gameObject.SetActive(true);
                GameObject.Find("MenuButton").transform.Find("BrazierButton/NewIcon").gameObject.SetActive(true);
            }
                

            yield return null;
        }
    }


    //주간 퀘스트
    private IEnumerator UpdateWeeklyData(int index)
    {
        while (true)
        {
            int amount = 0;
            switch (weeklyQuestList[index].type)
            {
                case "login":
                    amount = QuestData.questLogin;
                    break;
                case "hunting":
                    amount = QuestData.questWeeklyHunting;
                    break;
                case "refine":
                    amount = QuestData.questRefine;
                    break;
                case "reinforcement":
                    amount = QuestData.questReinforcement;
                    break;
                case "production":
                    amount = QuestData.questProduction;
                    break;
                case "plunder":
                    amount = QuestData.questPlunder;
                    break;
            }
            questWeeklyObj[index].transform.Find("Slider").gameObject.GetComponent<Slider>().maxValue = weeklyQuestList[index].amount;
            questWeeklyObj[index].transform.Find("Slider").gameObject.GetComponent<Slider>().value = amount;
            questWeeklyObj[index].transform.Find("Slider/SliderText").gameObject.GetComponent<Text>().text = amount + "/" + weeklyQuestList[index].amount;
            if (amount > weeklyQuestList[index].amount)
                questWeeklyObj[index].transform.Find("Slider/SliderText").gameObject.GetComponent<Text>().text
                    = weeklyQuestList[index].amount + "/" + weeklyQuestList[index].amount;

            //보상을 받은 경우
            if (weeklyQuestList[index].rewardFlag)
            {
                questWeeklyObj[index].transform.Find("CompleteImage").gameObject.SetActive(true);
                questWeeklyObj[index].transform.SetAsLastSibling();
            }
            else questWeeklyObj[index].transform.Find("CompleteImage").gameObject.SetActive(false);

            //완료했을 경우
            if (amount >= weeklyQuestList[index].amount && !weeklyQuestList[index].rewardFlag && !weeklyQuestList[index].completeFlag)
            {
                Debug.Log(index);
                //알림
                weeklyQuestList[index].completeFlag = true;
                GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(weeklyQuestList[index].alertText + " 주간 퀘스트를 완료했습니다.");
                questWeeklyObj[index].transform.SetAsFirstSibling();
                questWeeklyObj[index].transform.Find("RewardButtonImage").gameObject.SetActive(true);
                questWeeklyObj[index].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                int num = index;
                questWeeklyObj[index].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
                    getWeeklyReward(num);
                });
            }
            if (panel2.activeInHierarchy)
            {
                int compl = weeklyQuestList.FindAll(x => x.rewardFlag == true).Count;
                stateText.text = compl + " / " + weeklyQuestList.Count;
            }

            //느낌표
            if (weeklyQuestList[index].completeFlag && !weeklyQuestList[index].rewardFlag)
            {
                GameObject.Find("MenuButton").transform.Find("BrazierButton/BrazierMiniButton/QuestButton/NewIcon").gameObject.SetActive(true);
                GameObject.Find("MenuButton").transform.Find("BrazierButton/NewIcon").gameObject.SetActive(true);
            }
            yield return null;
        }
    }



    private void getReward(int num)
    {
        questList[num].rewardFlag = true;
        if(questList[num].reward_name == "gold" || questList[num].reward_name == "cash")
        {
            Player.instance.GetMoney(questList[num].reward_name, questList[num].reward_quantity);
        }
        else
        {
            if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == questList[num].reward_name) != null)
            {
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == questList[num].reward_name).possession
                    += questList[num].reward_quantity;
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == questList[num].reward_name).recent = true;
            }
            else
            {
                ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                    x => x.name == questList[num].reward_name).type, questList[num].reward_name, questList[num].reward_quantity));
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == questList[num].reward_name).recent = true;
            }
        }
        GameObject.Find("MenuButton").transform.Find("BrazierButton/BrazierMiniButton/QuestButton/NewIcon").gameObject.SetActive(false);
        GameObject.Find("MenuButton").transform.Find("BrazierButton/NewIcon").gameObject.SetActive(false);

        questObj[num].transform.Find("CompleteImage").gameObject.SetActive(true);
        questObj[num].transform.SetAsLastSibling(); //마지막 순서로 보내기
    }

    private void getWeeklyReward(int num)
    {
        weeklyQuestList[num].rewardFlag = true;
        if (weeklyQuestList[num].reward_name == "gold" || weeklyQuestList[num].reward_name == "cash")
        {
            Player.instance.GetMoney(weeklyQuestList[num].reward_name, weeklyQuestList[num].reward_quantity);
        }
        else
        {
            if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == weeklyQuestList[num].reward_name) != null)
            {
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == weeklyQuestList[num].reward_name).possession
                    += weeklyQuestList[num].reward_quantity;
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == weeklyQuestList[num].reward_name).recent = true;
            }
            else
            {
                ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                    x => x.name == weeklyQuestList[num].reward_name).type, weeklyQuestList[num].reward_name, weeklyQuestList[num].reward_quantity));
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == weeklyQuestList[num].reward_name).recent = true;
            }
        }

        GameObject.Find("MenuButton").transform.Find("BrazierButton/BrazierMiniButton/QuestButton/NewIcon").gameObject.SetActive(false);
        GameObject.Find("MenuButton").transform.Find("BrazierButton/NewIcon").gameObject.SetActive(false);

        questWeeklyObj[num].transform.Find("CompleteImage").gameObject.SetActive(true);
        questWeeklyObj[num].transform.SetAsLastSibling(); //마지막 순서로 보내기
    }


    public void SwitchScrollPanel(int i)
    {
        if (i == 1)
        {
            scroll.GetComponent<ScrollRect>().content = panel1.GetComponent<RectTransform>();
            panel1.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10);
        }
        else if (i == 2)
        {
            scroll.GetComponent<ScrollRect>().content = panel2.GetComponent<RectTransform>();
            panel2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -10);
        }
    }


    IEnumerator TimeRemaining()
    {
        while (true)
        {
            System.DateTime nowTime = System.DateTime.Now;

            if (panel1.activeInHierarchy)
            {
                QuestPopup.transform.Find("UIPanel/TimeBox/TimeText").gameObject.GetComponent<Text>().text
                    = "남은 시간 : " + (23 - nowTime.Hour) + "시간 " + (59 - nowTime.Minute) + "분 " + (60 - nowTime.Second) + "초";
            }
            else if (panel2.activeInHierarchy)
            {
                int day = 0;
                switch(nowTime.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        day = 6;
                        break;
                    case System.DayOfWeek.Tuesday:
                        day = 5;
                        break;
                    case System.DayOfWeek.Wednesday:
                        day = 4;
                        break;
                    case System.DayOfWeek.Thursday:
                        day = 3;
                        break;
                    case System.DayOfWeek.Friday:
                        day = 2;
                        break;
                    case System.DayOfWeek.Saturday:
                        day = 1;
                        break;
                    case System.DayOfWeek.Sunday:
                        day = 0;
                        break;
                }

                QuestPopup.transform.Find("UIPanel/TimeBox/TimeText").gameObject.GetComponent<Text>().text
                    = "남은 시간 : " +day+"일 " + (23 - nowTime.Hour) + "시간 " + (59 - nowTime.Minute) + "분 " + (60 - nowTime.Second) + "초";
            }
            yield return null;
        }
    }


}
