using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    private GameObject QuestPopup;

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
    private List<Quest> questList;
    
    private void Start()
    {
        QuestPopup = GameObject.Find("Menu").transform.Find("QuestPopup").gameObject;

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
        questList = QuestData.instance.getQuestList();

        scroll.GetComponent<ScrollRect>().content = panel1.GetComponent<RectTransform>();

        GameObject.Find("BrazierButton").transform.Find("BrazierMiniButton/QuestButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            panel1.SetActive(true); SwitchScrollPanel(1);
        });

        

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
                Debug.Log(i);
                questList[i].completeFlag = true;
                GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(questList[i].alertText + " 퀘스트를 완료했습니다.");

                questObj[i].transform.Find("RewardButtonImage").gameObject.SetActive(true);
                questObj[i].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                questObj[i].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
                    getReward(num);
                });
            }



            StartCoroutine(UpdateData(i));      //퀘스트 내용 갱신
        }

       



    }


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

                questObj[index].transform.Find("RewardButtonImage").gameObject.SetActive(true);
                questObj[index].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                int num = index;
                questObj[index].transform.Find("RewardButtonImage/RewardButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
                    getReward(num);
                });
            }
            yield return null;
        }
    }


    private void getReward(int num)
    {
        questList[num].rewardFlag = true;
        questObj[num].transform.Find("CompleteImage").gameObject.SetActive(true);

        questObj[num].transform.SetAsLastSibling(); //마지막 순서로 보내기

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

}
