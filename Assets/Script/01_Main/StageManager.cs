using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    private int goldPrice = 50; //즉시완료에 필요한 골드
    private int cashPrice = 5;  //즉시완료에 필요한 보석
    private bool mode = true;   // 사냥/약탈

    private MercenaryManager mercenaryManager;  //mercenary info
    private StatData statData;

    //스테이지 정보
    private StageData stageData;
    private List<StageInfo> stageInfoList;
    private List<StageInfo> stageInfoListtmp;   //임시 스테이지 정보 저장공간
    //약탈 정보
    private List<PlunderInfo> plunderInfoList;

    //private string curContSelect;              //현재 선택된 대륙
    private int curStageSelect;               //현재 선택된 스테이지
    private string curPlunderSelect;          //현재 선택된 상대 플레이어
    private float stageTime = 1800f;

    //스테이지 정보 팝업창
    private GameObject stagePopup;
    private Text nameText;
    private GameObject stageGetItemBox;

    private GameObject stageStatePopup;     //스테이지 현황 팝업창
    private GameObject plunderPopup;        //약탈 팝업
    private GameObject plunderPlayerBox;
    private GameObject plunderEnemyBox;
    private GameObject plunderItemBox;

    private GameObject ClearFail;

    private GameObject selectFrame;     //용병 선택 표시
    private bool selectMerFlag = false;         //용병 선택
    private GameObject selectMerObj;            //

    private GameObject SpotObj;
    private GameObject SpotButtonObj;
    private GameObject StageObj;
    private GameObject PlunderObj;
    private GameObject PlunderButtonObj;
    private GameObject WorldMapBackObj;

    private GameObject systemPopup; //시스템 팝업
    private Button sys_yesButton;
    private GameObject imdComPopup; //즉시 완료 팝업
    private Button imd_yesButton;


    private void Start()
    {
        //worldmapManager = GameObject.Find("Menu").transform.Find("WorldMap").gameObject.GetComponent<WorldMapManager>();
        mercenaryManager = GameObject.Find("StageManager").GetComponent<MercenaryManager>();
        statData = GameObject.Find("PlayerData").GetComponent<StatData>();
        stageData = GameObject.Find("StageData").GetComponent<StageData>();
        stageInfoList = new List<StageInfo>();
        stageInfoListtmp = new List<StageInfo>();
        plunderInfoList = new List<PlunderInfo>();

        stagePopup = GameObject.Find("System").transform.Find("StagePopup").gameObject;
        stageGetItemBox = stagePopup.transform.Find("UIPanel/GetItemBox").gameObject;
        stageStatePopup = GameObject.Find("System").transform.Find("StageStatePopup").gameObject;
        SpotObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Spot").gameObject;
        SpotButtonObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/SpotButton").gameObject;
        StageObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject;
        PlunderObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Plunder").gameObject;
        PlunderButtonObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/PlunderButton").gameObject;
        WorldMapBackObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back").gameObject;
        plunderPopup = GameObject.Find("System").transform.Find("PlunderPopup").gameObject;
        plunderPlayerBox = plunderPopup.transform.Find("UIPanel/PlayerBox").gameObject;
        plunderEnemyBox = plunderPopup.transform.Find("UIPanel/EnemyBox").gameObject;
        plunderItemBox = plunderPopup.transform.Find("UIPanel/ItemBox").gameObject;
        ClearFail = GameObject.Find("System").transform.Find("ClearFail").gameObject;

        stageInfoList = stageData.getStageInfoList();
        plunderInfoList = stageData.getPlunderInfoList();

        stageInfoListtmp = stageInfoList.FindAll(x => (x.state == true || x.complete == true));
        for (int i = 0; i < stageInfoListtmp.Count; i++)
        {
            GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + stageInfoListtmp[i].mercenaryName + "Selection").GetComponent<Button>().interactable = false;
        }
        systemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        sys_yesButton = systemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        imdComPopup = GameObject.Find("System").transform.Find("ImdCompletePopup").gameObject;
        imd_yesButton = imdComPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();

        //위치 재배치
        //stage버튼 개수만큼 각 대륙별로 버튼 생성. 배치.
        //스팟 transform 설정
        int count = SpotObj.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            StageData.spotList[StageData.spotList.FindIndex(
                x => x.getSpotName() == SpotObj.transform.GetChild(i).name)].position
                = SpotObj.transform.GetChild(i);
        }

        //사냥 오브젝트 배치
        List<StageInfo> sList = stageInfoList;
        for (int i = 0; i < sList.Count; i++)
        {
            if (sList[i].spotName != null)
            {
                string spotName = sList[i].spotName;
                //스팟 인덱스
                int index = StageData.spotList.FindIndex(x => x.getSpotName() == spotName);

                GameObject spotButton = Instantiate(SpotButtonObj.gameObject);
                spotButton.transform.SetParent(StageObj.gameObject.transform);
                spotButton.transform.localPosition = StageData.spotList[index].getPosition().localPosition;
                spotButton.SetActive(true);
                spotButton.transform.Find("StageText").GetComponent<Text>().text = sList[i].getStageNum().ToString();
                spotButton.name = sList[i].stageName + "Button"; //오브젝트 이름 변경
                spotButton.GetComponent<Image>().sprite = sList[i].sprite;
                if (sList[i].state)
                {
                    //if (sList[i].getStageNum() <= 15)
                    //    spotButton.transform.Find("State/Progress/pickax").gameObject.SetActive(true);
                    //else
                    spotButton.transform.Find("State/Progress/sword").gameObject.SetActive(true);
                    spotButton.transform.Find("MercImage").gameObject.SetActive(true);
                    spotButton.transform.Find("MercImage").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Mercenary/" + sList[i].mercenaryName);
                }

                //리젠 상태일 때 반투명
                if (sList[i].regen) spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 0.5f);
                else spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 1.0f);
            }
        }



        //약탈 오브젝트 배치
        List<PlunderInfo> pList = plunderInfoList;
        for (int i = 0; i < pList.Count; i++)
        {
            if (pList[i].spotName != null)
            {
                string spotName = pList[i].spotName;
                int index = StageData.spotList.FindIndex(x => x.getSpotName() == spotName);

                GameObject plunderButton = Instantiate(PlunderButtonObj.gameObject);
                plunderButton.transform.SetParent(PlunderObj.gameObject.transform);
                plunderButton.transform.localPosition = StageData.spotList[index].getPosition().localPosition;
                plunderButton.SetActive(true);
                plunderButton.transform.Find("StageText").GetComponent<Text>().text = pList[i].opponentName;
                plunderButton.name = pList[i].PlunderName + "Button";
                //sprite
                plunderButton.transform.Find("State/NameText").gameObject.GetComponent<Text>().text = pList[i].opponentName;
                plunderButton.transform.Find("State/LevelText").gameObject.GetComponent<Text>().text = "Level " + (1).ToString();//

                //리젠 상태일 때 반투명
                if (pList[i].regen) plunderButton.GetComponent<Image>().color = new Color(plunderButton.GetComponent<Image>().color.r, plunderButton.GetComponent<Image>().color.g, plunderButton.GetComponent<Image>().color.b, 0.5f);
                else plunderButton.GetComponent<Image>().color = new Color(plunderButton.GetComponent<Image>().color.r, plunderButton.GetComponent<Image>().color.g, plunderButton.GetComponent<Image>().color.b, 1.0f);
            }
        }

    }



    private void Update()
    {
        stageInfoList = stageData.getStageInfoList();
        plunderInfoList = stageData.getPlunderInfoList();
        stageInfoListtmp.Clear();

        WorldMapBackObj.transform.Find("Smithy/Level/LevelText").gameObject.GetComponent<Text>().text = "Level " + Player.Play.level.ToString();

        //탐험 완료 시 버튼 변경.
        for (int i = 0; i < stageInfoList.Count; i++)
        {
            if (stageStatePopup.activeInHierarchy && stageInfoList[i].complete &&
                curStageSelect == stageInfoList[i].getStageNum())
            {
                GameObject systemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
                systemPopup.SetActive(false);

                GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(false);
                GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(true);
            }
        }

        //스테이지 현황 창 활성화 중일 때.
        if (stageStatePopup.activeInHierarchy)
        {
            StageInfo stin = stageInfoList.Find(x => x.getStageNum() == curStageSelect);
            Text timeText = GameObject.Find("TimeBox").transform.Find("Text").gameObject.GetComponent<Text>();
            float time = stin.time;
            timeText.text = "남은 시간 : " + ((int)(time / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";

            //획득한 아이템 목록
            string str = "";
            for (int i = 0; i < stin.getItem.Length; i++)
            {
                if (stin.getItem[i] != null)
                {
                    str += stin.getItem[i] + " " + stin.getItemNum[i] + "개\n";
                }
            }
            GameObject.Find("GetItemListText").GetComponent<Text>().text = str;

        }


        // 스테이지 선택 창
        if (GameObject.Find("Menu").transform.Find("WorldMap").gameObject.activeInHierarchy)
        {
            stageInfoListtmp.Clear();

            //사냥 모드
            if (mode)
            {
                //시간
                stageInfoListtmp = stageInfoList.FindAll(x => x.state == true);
                for (int i = 0; i < stageInfoListtmp.Count; i++)
                {
                    string stageName = stageInfoListtmp[i].stageName;
                    float time = stageInfoListtmp[i].time;
                    GameObject.Find(stageName + "Button").transform.Find("State").gameObject.SetActive(true);
                    GameObject.Find(stageName + "Button").transform.Find("State/Text").gameObject.GetComponent<Text>().text
                        = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();

                    //획득 효과
                    if (stageInfoListtmp[i].getRecentItemFlag)
                    {
                        Image spr = GameObject.Find(stageName + "Button").transform.Find("GetItemEff/GetItemImage").gameObject.GetComponent<Image>();
                        //이미지 교체
                        ItemImageChange(stageInfoListtmp[i].getRecentItem, spr);
                        //획득 개수
                        GameObject.Find(stageName + "Button").transform.Find("GetItemEff/Text").gameObject.GetComponent<Text>().text = "+" + stageInfoListtmp[i].getRecentItemNum;
                        StartCoroutine(getItemEff(stageInfoListtmp[i], stageName));
                    }

                }
                //완료
                stageInfoListtmp = stageInfoList.FindAll(x => x.complete == true);
                for (int i = 0; i < stageInfoListtmp.Count; i++)
                {
                    string stageName = stageInfoListtmp[i].stageName;
                    GameObject.Find(stageName + "Button").transform.Find("State").gameObject.SetActive(true);
                    GameObject.Find(stageName + "Button").transform.Find("State/Text").gameObject.GetComponent<Text>().text = "완료";

                    GameObject.Find(stageName + "Button").transform.Find("State/Progress/pickax").gameObject.SetActive(false);
                    GameObject.Find(stageName + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(false);
                }
                //리젠
                stageInfoListtmp = stageInfoList.FindAll(x => x.regen == true);
                for (int i = 0; i < stageInfoListtmp.Count; i++)
                {
                    string stageName = stageInfoListtmp[i].stageName;
                    float time = stageInfoListtmp[i].time;
                    if (GameObject.Find(stageName + "Button").activeInHierarchy)
                    {
                        GameObject.Find(stageName + "Button").transform.Find("State").gameObject.SetActive(true);
                        GameObject.Find(stageName + "Button").transform.Find("State/Text").gameObject.GetComponent<Text>().text
                            = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
                        GameObject.Find(stageName + "Button").transform.Find("State/Progress/pickax").gameObject.SetActive(false);
                        GameObject.Find(stageName + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(false);
                    }
                }
                //대기 상태
                stageInfoListtmp = stageInfoList.FindAll(x => (x.wait == true));
                for (int i = 0; i < stageInfoListtmp.Count; i++)
                {
                    string stageName = stageInfoListtmp[i].stageName;
                    GameObject spotButton = GameObject.Find(stageName + "Button");
                    if (spotButton)
                    {
                        spotButton.transform.Find("State").gameObject.SetActive(false);
                        //리젠 끝나고 a값 복구
                        spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 1.0f);
                    }
                }
            }

            //약탈 모드
            if (!mode)
            {
                //약탈 리젠
                List<PlunderInfo> plunderInfotmp = plunderInfoList.FindAll(x => x.regen == true);
                for (int i = 0; i < plunderInfotmp.Count; i++)
                {
                    string stageName = plunderInfotmp[i].PlunderName;
                    float time = plunderInfotmp[i].time;
                    if (GameObject.Find(stageName + "Button").activeInHierarchy)
                    {
                        GameObject.Find(stageName + "Button").transform.Find("State").gameObject.SetActive(true);
                        GameObject.Find(stageName + "Button").transform.Find("State/LevelText").gameObject.GetComponent<Text>().text
                            = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
                    }
                }
                //대기 상태
                plunderInfotmp = plunderInfoList.FindAll(x => (x.regen == false));
                for (int i = 0; i < plunderInfotmp.Count; i++)
                {
                    string stageName = plunderInfotmp[i].PlunderName;
                    GameObject spotButton = GameObject.Find(stageName + "Button");
                    if (spotButton)
                    {
                        //레벨 spotButton.transform.Find("State").gameObject.SetActive(false);
                        //리젠 끝나고 a값 복구
                        spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 1.0f);
                        if (GameObject.Find(stageName + "Button").activeInHierarchy)
                            GameObject.Find(stageName + "Button").transform.Find("State/LevelText").gameObject.GetComponent<Text>().text = "Level " +
                                StageData.instance.getPlundeList().Find(x => x.getName() == plunderInfotmp[i].opponentName).level.ToString();
                    }
                }
            }
        }

        stageData.setStageInfoList(stageInfoList);


    }

    IEnumerator getItemEff(StageInfo stageInfoListtmp, string stageName)
    {
        GameObject getitemeff = GameObject.Find(stageName + "Button").transform.Find("GetItemEff").gameObject;
        getitemeff.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        stageInfoListtmp.getRecentItemFlag = false;
        getitemeff.SetActive(false);

    }




    //스테이지 팝업창 갱신. (정보창, 현황창)
    public void initStagePopup(GameObject obj)
    {
        selectMerFlag = false;
        curStageSelect = System.Convert.ToInt32(obj.transform.Find("StageText").GetComponent<Text>().text);

        StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect);

        //리젠 상태
        if (result.regen)
        {
            return;
        }
        //용병 안 보낸 상태
        else if (!result.state && !result.complete)
        {
            //스테이지 정보 창
            stagePopup.SetActive(true);
            nameText = GameObject.Find("StageNameText").GetComponent<Text>();
            selectFrame = stagePopup.transform.Find("UIPanel/MercenaryBox/selectFrame").gameObject;
            selectFrame.SetActive(false);
            nameText.text = result.type + " " + result.typeNum.ToString();
            GameObject.Find("StageTimeText").GetComponent<Text>().text =
                "소요 시간 " + ((int)(typeNumToTime(result.typeNum) / 60)).ToString() + "m " + ((int)(typeNumToTime(result.typeNum) % 60)).ToString() + "s";
            //stage에 따라 획득 가능한 아이템
            setGetItemInfo(result);

        }
        //용병 보낸 상태
        else
        {
            //스테이지 현황 팝업창
            stageStatePopup.SetActive(true);
            GameObject.Find("StageStateText").GetComponent<Text>().text = result.type + " " + result.typeNum.ToString();
            stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(true);

            Text timeText = GameObject.Find("TimeBox").transform.Find("Text").gameObject.GetComponent<Text>();
            float time = stageInfoList.Find(x => x.getStageNum() == curStageSelect).time;
            timeText.text = "남은 시간 : " + ((int)(time / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";

            //완료
            if (result.complete)
            {
                GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(false);
                GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(true);
            }
            else
            {
                GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(true);
                GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(false);
            }

        }
    }

    //스테이지 팝업창에서 용병 선택
    public void select(GameObject obj)
    {
        selectMerFlag = true;
        selectMerObj = obj;
        mercenaryManager.setCurSelect(obj.transform.Find("NameText").gameObject.GetComponent<Text>().text);

        //선택표시 이동
        selectFrame.transform.position = obj.transform.position;
        selectFrame.SetActive(true);
    }

    //용병 보내기
    public void send()
    {
        if (selectMerFlag)
        {
            stagePopup.SetActive(false);
            stageStatePopup.SetActive(true);

            //보낸 상태로 변경
            StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect);
            GameObject.Find("StageStateText").GetComponent<Text>().text = result.type + " " + result.typeNum.ToString();
            result.state = true;
            result.wait = false;
            for (int i = 0; i < result.getItem.Length; i++)
            {
                result.getItem[i] = null;
                result.getItemNum[i] = 0;
            }
            result.time = typeNumToTime(result.typeNum);
            result.mercenaryName = mercenaryManager.getCurSelect();

            stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(true);
            stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect)] = result;

            Mercenary mer = mercenaryManager.getMercenary().Find(x => x.getName() == mercenaryManager.getCurSelect());
            mer.setStageNum(result.getStageNum());
            mer.setState(true);
            mercenaryManager.setMercenaryIndex(mercenaryManager.getMercenary().FindIndex(x => x.getName() == mer.getName()), mer);

            GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection").GetComponent<Button>().interactable = false;

            GameObject mercImage = GameObject.Find(result.stageName + "Button").transform.Find("MercImage").gameObject;
            mercImage.SetActive(true);
            string merImageName = null;
            if (result.mercenaryName == "A") merImageName = "miner";
            if (result.mercenaryName == "B") merImageName = "ninja";
            if (result.mercenaryName == "C") merImageName = "knight";
            mercImage.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Mercenary/" + merImageName);

            Text timeText = GameObject.Find("TimeBox").transform.Find("Text").gameObject.GetComponent<Text>();
            float time = stageInfoList.Find(x => x.getStageNum() == curStageSelect).time;
            timeText.text = "남은 시간 : " + ((int)(time / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";

            //if(result.getStageNum() <= 15)
            //    GameObject.Find("stage" + result.getStageNum().ToString() + "Button").transform.Find("State/Progress/pickax").gameObject.SetActive(true);
            //else 
            GameObject.Find("stage" + result.getStageNum().ToString() + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(true);

            GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(true);
            GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(false);

        }

    }


    //즉시 완료 버튼 - 팝업창
    public void ImmediatelyCompleteButton(string money)
    {
        //시스템 팝업창 띄우기
        imdComPopup.SetActive(true);
        imdComPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "즉시 완료";
        if (money == "골드")
            imdComPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = money + " " + goldPrice + "개를 사용하여 즉시 완료하시겠습니까?";
        else imdComPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = money + " " + cashPrice + "개를 사용하여 즉시 완료하시겠습니까?";

        imd_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
        imd_yesButton.GetComponent<Button>().onClick.AddListener(() => ImmediatelyComplete(money));  //버튼 기능 추가
    }

    //즉시 완료 yes
    public void ImmediatelyComplete(string money)
    {
        if (money == "골드")
        {
            if (Player.Play.gold < goldPrice)
            {
                systemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = money + "가 부족합니다.";
                systemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = money + " 구매 페이지로 이동하시겠습니까?";
                sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
                sys_yesButton.GetComponent<Button>().onClick.AddListener(() => GameObject.Find("System").transform.Find("Shop").gameObject.SetActive(true));
                systemPopup.SetActive(true);
                return;
            }
            else GameObject.Find("PlayerData").GetComponent<Player>().LostMoney("gold", goldPrice);
        }
        else if (money == "보석")
        {
            if (Player.Play.cash < cashPrice)
            {
                systemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = money + "가 부족합니다.";
                systemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = money + " 구매 페이지로 이동하시겠습니까?";
                sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
                sys_yesButton.GetComponent<Button>().onClick.AddListener(() => GameObject.Find("System").transform.Find("Shop").gameObject.SetActive(true));
                systemPopup.SetActive(true);
                return;
            }
            else GameObject.Find("PlayerData").GetComponent<Player>().LostMoney("cash", cashPrice);
        }

        StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect);
        result.state = false;

        //남은 시간 계산해서 아이템 획득
        while (result.time >= 0f)
        {
            float time = Random.Range(30f, 60f);
            result.time -= time;
            stageData.getItem(result);
        }

        result.time = 0f;
        result.complete = true;
        stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect)] = result;
        GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(false);
        GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(true);
        WorldMapBackObj.transform.Find("Stage/" + result.stageName + "Button/State/Text").gameObject.GetComponent<Text>().text = "완료";

    }

    //용병 탐험 취소
    public void CancelButton()
    {
        //시스템 팝업창 띄우기
        systemPopup.SetActive(true);
        systemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "탐험 취소";
        systemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "탐험을 취소하고 용병을 불러오시겠습니까?";

        sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
        sys_yesButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect);
                result.state = false;
                result.time = 0f;
                result.complete = false;
                for (int i = 0; i < result.getItem.Length; i++)
                {
                    result.getItem[i] = null;
                    result.getItemNum[i] = 0;
                }
                result.getItemTimeFlag = false;
                GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection").GetComponent<Button>().interactable = true;
                WorldMapBackObj.transform.Find("/Stage/stage" + result.getStageNum().ToString() + "Button/MercImage").gameObject.SetActive(false);

                Mercenary mer = mercenaryManager.getMercenary().Find(x => x.getName() == result.mercenaryName);
                mer.setStageNum(0);
                mer.setState(false);
                mercenaryManager.setMercenaryIndex(mercenaryManager.getMercenary().FindIndex(x => x.getName() == mer.getName()), mer);
                stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(false);

                result.mercenaryName = null;
                stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect)] = result;

                //stagePopup.SetActive(true);
                //GameObject.Find("StageNameText").GetComponent<Text>().text = "스테이지 " + curStageSelect;
                stageStatePopup.SetActive(false);

                WorldMapBackObj.transform.Find("Stage/" + result.stageName + "Button/State").gameObject.SetActive(false);
            }
            );  //버튼 기능 추가
    }



    //보상 받기 버튼
    public void CompleteButton()
    {
        StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect);

        //보상
        //월드맵/스테이지에서 완료한 경우
        if (GameObject.Find("Menu").transform.Find("WorldMapPopup").gameObject.activeInHierarchy)
        {
            Vector3 getItemListObj = GameObject.Find(result.stageName + "Button").transform.position; // 시작 위치
            GameObject stageItem = GameObject.Find("StageItemImagePos").transform.Find("StageItemImage").gameObject;
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(30);
            StartCoroutine(createGetItem(result, stageItem, "StageItemImagePos", getItemListObj));
        }
        //로비에서 완료한 경우
        else
        {
            Vector3 getItemListObj = GameObject.Find("Mercenary" + result.mercenaryName + "Button").transform.position; // 시작 위치
            GameObject stageItem = GameObject.Find("StageItemImagePos").transform.Find("StageItemImage").gameObject;
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(30);
            StartCoroutine(createGetItem(result, stageItem, "StageItemImagePos", getItemListObj));
        }


        result.complete = false;
        result.getItemTimeFlag = false;
        GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection").GetComponent<Button>().interactable = true;

        WorldMapBackObj.transform.Find("Stage/stage" + result.getStageNum().ToString() + "Button/MercImage").gameObject.SetActive(false);

        Mercenary mer = mercenaryManager.getMercenary().Find(x => x.getName() == result.mercenaryName);

        mer.setStageNum(0);
        mer.setState(false);
        mercenaryManager.setMercenaryIndex(mercenaryManager.getMercenary().FindIndex(x => x.getName() == mer.getName()), mer);

        stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(false);
        result.mercenaryName = null;

        //리젠
        result.regen = true;
        StageData.spotList[StageData.spotList.FindIndex(x => x.stageNum == result.getStageNum())].stageActive = false;
        int random = 0;
        int index = 0;

        List<Spot> sList = StageData.spotList;
        while (true)
        {
            random = Random.Range(1, sList.Count + 1);
            index = StageData.spotList.FindIndex(x => x.getPosition().name == "spot" + random.ToString());
            //이미 위치한 스테이지 범위에 없게 배치
            List<StageInfo> stif = stageInfoList.FindAll(x => x.spotName != null);
            if (stif != null)
            {
                bool distanceBool = false;
                for (int k = 0; k < stif.Count; k++)
                {
                    GameObject spottmp = WorldMapBackObj.transform.Find("Spot/" + stif[k].spotName).gameObject;
                    float disCul = Vector2.Distance(StageData.spotList[index].getPosition().transform.localPosition, spottmp.transform.localPosition);
                    if (disCul < stageData.getDist()) distanceBool = true;
                }
                if (distanceBool) continue;
            }
            if (StageData.spotList[index].stageActive == false) break;
        }
        GameObject spotButton = WorldMapBackObj.transform.Find("Stage/" + result.stageName + "Button").gameObject;
        spotButton.transform.localPosition = StageData.spotList[index].getPosition().localPosition;
        //스팟이랑 스테이지 정보 공유
        StageData.spotList[index].stageNum = result.getStageNum();    //스테이지 번호 저장
        StageData.spotList[index].stageActive = true;    //스팟 활성화
        result.spotName = StageData.spotList[index].getPosition().name;
        result.type = stageData.typeNumToString(result.getStageNum(), 1);
        random = Random.Range(0, 100);
        result.typeNum = 1;
        result.stageName = "stage" + result.getStageNum().ToString();
        spotButton.transform.Find("StageText").GetComponent<Text>().text = result.getStageNum().ToString();  //
        spotButton.name = result.stageName + "Button"; //오브젝트 이름 변경
        stageData.stageImageChange(result);

        result.time = typeNumToRegenTime();             //리젠 시간 설정
        spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 0.5f);
        spotButton.GetComponent<Image>().sprite = result.sprite;

        //스테이지 변경된 정보 저장
        stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect)] = result;
        stageData.setStageInfoList(stageInfoList);

        GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(true);
        GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(false);

        stageStatePopup.SetActive(false);
    }

    //획득한 아이템 인벤토리에 넣는 효과
    IEnumerator createGetItem(StageInfo result, GameObject stageItem, string createPos, Vector3 startPos)
    {
        Vector3 vec = startPos;
        for (int i = 0; i < result.getItem.Length; i++)
        {
            if (result.getItem[i] == null) continue;
            //if (GameObject.Find("Menu").transform.Find("WorldMapPopup").gameObject.activeInHierarchy)
            //{
            //    GameObject ItemInst = Instantiate(stageItem);
            //    ItemInst.transform.SetParent(GameObject.Find(createPos).transform, false);
            //    //위치
            //    ItemInst.transform.position = vec;
            //    //이미지 변경
            //    Image itemimage = ItemInst.GetComponent<Image>();
            //    ItemImageChange(result.getItem[i], itemimage);
            //    ItemInst.SetActive(true);
            //}
            ////로그
            //GameObject ItemLogInst = Instantiate(GameObject.Find("GetItemLog").transform.Find("range/GetItemLogText").gameObject);
            //ItemLogInst.transform.SetParent(GameObject.Find("GetItemLog").transform.Find("range").gameObject.transform, false);
            //ItemLogInst.GetComponent<Text>().text = result.getItem[i] + " " + result.getItemNum[i] + "개 획득";
            //ItemLogInst.SetActive(true);

            //획득 아이템 데이터 저장
            ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).possession += result.getItemNum[i];
            ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).recent = true;


            //yield return new WaitForSeconds(0.3f);
            yield return null;
        }
    }

    //스테이지 현황 창 닫기
    public void closeStageStatePopup()
    {
        for (int i = 0; i < mercenaryManager.getMercenary().Count; i++)
            if (mercenaryManager.getMercenary()[i].getName() != null)
                stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + mercenaryManager.getMercenary()[i].getName()).gameObject.SetActive(false);
    }

    //스테이지 정보창 획득 가능한 아이템 정리
    public void setGetItemInfo(StageInfo result)
    {
        //사냥
        stageGetItemBox.transform.Find("item1").GetComponent<Image>().sprite = Resources.Load<Sprite>("Gather/sword");
        stageGetItemBox.transform.Find("item1/InfoText").GetComponent<Text>().text = "하이그라스 단검";
        stageGetItemBox.transform.Find("item2").GetComponent<Image>().sprite = Resources.Load<Sprite>("Gather/sword2");
        stageGetItemBox.transform.Find("item2/InfoText").GetComponent<Text>().text = "엘더 소드";
        stageGetItemBox.transform.Find("item3").GetComponent<Image>().sprite = Resources.Load<Sprite>("Gather/sword3");
        stageGetItemBox.transform.Find("item3/InfoText").GetComponent<Text>().text = "팔라딘 소드";
        stageGetItemBox.transform.Find("specialItem/Box1").GetComponent<Image>().sprite = Resources.Load<Sprite>("Gather/sword");
        stageGetItemBox.transform.Find("specialItem/SpecialItemText").GetComponent<Text>().text = "고급 하이그라스 단검";
    }

    //이미지 변경
    public void ItemImageChange(string stageInfoListtmp, Image spr)
    {
        if (stageInfoListtmp == "철 가루") { spr.sprite = Resources.Load<Sprite>("Gather/ironDust"); }
        else if (stageInfoListtmp == "원석") { spr.sprite = Resources.Load<Sprite>("Gather/gemstone"); }
        else if (stageInfoListtmp == "구리") { spr.sprite = Resources.Load<Sprite>("Gather/copper"); }
        else if (stageInfoListtmp == "철") { spr.sprite = Resources.Load<Sprite>("Gather/iron"); }
        else if (stageInfoListtmp == "은") { spr.sprite = Resources.Load<Sprite>("Gather/silver"); }
        else if (stageInfoListtmp == "금") { spr.sprite = Resources.Load<Sprite>("Gather/gold"); }
        else if (stageInfoListtmp == "하이그라스 단검") { spr.sprite = Resources.Load<Sprite>("Gather/sword"); }
        else if (stageInfoListtmp == "엘더 소드") { spr.sprite = Resources.Load<Sprite>("Gather/sword2"); }
        else if (stageInfoListtmp == "팔라딘 소드") { spr.sprite = Resources.Load<Sprite>("Gather/sword3"); }
        else if (stageInfoListtmp == "고급 하이그라스 단검") { spr.sprite = Resources.Load<Sprite>("Gather/sword"); }
    }


    /// <summary>
    /// 약탈
    /// </summary>

    //약탈할 플레이어 클릭
    public void initPlunderButton(GameObject obj)
    {
        curPlunderSelect = obj.transform.Find("StageText").GetComponent<Text>().text;

        PlunderInfo result = plunderInfoList.Find(x => x.opponentName == curPlunderSelect);
        Plunder plunder = StageData.instance.getPlundeList().Find(x => x.getName() == curPlunderSelect);
        //리젠 상태
        if (result.regen)
        {
            return;
        }
        //팝업
        else
        {
            //플레이어 정보 갱신
            plunderPlayerBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = Player.Play.Name;
            plunderPlayerBox.transform.Find("TextGroup/LevelText").gameObject.GetComponent<Text>().text = Player.Play.level.ToString();

            plunderPlayerBox.transform.Find("TextGroup/DpsText").gameObject.GetComponent<Text>().text = ((int)statData.getRepreSetStat().dps).ToString();
            plunderPlayerBox.transform.Find("TextGroup/StrPowerText").gameObject.GetComponent<Text>().text = ((int)statData.getRepreSetStat().strPower).ToString();
            plunderPlayerBox.transform.Find("TextGroup/AttackSpeedText").gameObject.GetComponent<Text>().text = ((int)statData.getRepreSetStat().attackSpeed).ToString();
            plunderPlayerBox.transform.Find("TextGroup/FocusText").gameObject.GetComponent<Text>().text = ((int)statData.getRepreSetStat().focus).ToString();
            plunderPlayerBox.transform.Find("TextGroup/CriticalText").gameObject.GetComponent<Text>().text = ((int)statData.getRepreSetStat().critical).ToString();
            plunderPlayerBox.transform.Find("TextGroup/DefPowerText").gameObject.GetComponent<Text>().text = ((int)statData.getRepreSetStat().defPower).ToString();
            plunderPlayerBox.transform.Find("TextGroup/EvaRateText").gameObject.GetComponent<Text>().text = ((int)statData.getRepreSetStat().evaRate).ToString();

            //상대방 데이터 가져와서 갱신
            plunderEnemyBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = plunder.getName();
            plunderEnemyBox.transform.Find("TextGroup/LevelText").gameObject.GetComponent<Text>().text = plunder.level.ToString();

            plunderEnemyBox.transform.Find("TextGroup/DpsText").gameObject.GetComponent<Text>().text = ((int)plunder.stat.dps).ToString();
            plunderEnemyBox.transform.Find("TextGroup/StrPowerText").gameObject.GetComponent<Text>().text = ((int)plunder.stat.strPower).ToString();
            plunderEnemyBox.transform.Find("TextGroup/AttackSpeedText").gameObject.GetComponent<Text>().text = ((int)plunder.stat.attackSpeed).ToString();
            plunderEnemyBox.transform.Find("TextGroup/FocusText").gameObject.GetComponent<Text>().text = ((int)plunder.stat.focus).ToString();
            plunderEnemyBox.transform.Find("TextGroup/CriticalText").gameObject.GetComponent<Text>().text = ((int)plunder.stat.critical).ToString();
            plunderEnemyBox.transform.Find("TextGroup/DefPowerText").gameObject.GetComponent<Text>().text = ((int)plunder.stat.defPower).ToString();
            plunderEnemyBox.transform.Find("TextGroup/EvaRateText").gameObject.GetComponent<Text>().text = ((int)plunder.stat.evaRate).ToString();

            plunderItemBox.transform.Find("ItemTitleText").gameObject.GetComponent<Text>().text = "획득 가능한 아이템";
            plunderPopup.transform.Find("UIPanel/PlunderButton").gameObject.SetActive(true);
            plunderPopup.transform.Find("UIPanel/OKButton").gameObject.SetActive(false);
            plunderPlayerBox.transform.Find("Win").gameObject.SetActive(false);
            plunderPlayerBox.transform.Find("Lose").gameObject.SetActive(false);
            plunderEnemyBox.transform.Find("Win").gameObject.SetActive(false);
            plunderEnemyBox.transform.Find("Lose").gameObject.SetActive(false);
            plunderPopup.transform.Find("FrontBox").gameObject.SetActive(false);
            plunderPopup.SetActive(true);

            //약탈 버튼
            plunderPopup.transform.Find("UIPanel/PlunderButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            plunderPopup.transform.Find("UIPanel/PlunderButton").gameObject.GetComponent<Button>().onClick.AddListener(() => PlunderButton(plunder, result));
        }
    }


    //약탈 시작 버튼
    public void PlunderButton(Plunder plunder, PlunderInfo result)
    {
        //리젠
        StageData.spotList[StageData.spotList.FindIndex(x => x.plunderNum == result.getPlunderNum())].plunderActive = false;
        int random = 0;
        int index = 0;

        //위치 변경
        while (true)
        {
            random = Random.Range(1, 100 + 1);
            index = StageData.spotList.FindIndex(x => x.getPosition().name == "spot" + random.ToString());
            //이미 위치한 스테이지 범위에 없게 배치
            List<PlunderInfo> plif = plunderInfoList.FindAll(x => x.spotName != null);
            if (plif != null)
            {
                bool distanceBool = false;
                for (int k = 0; k < plif.Count; k++)
                {
                    GameObject spottemp = SpotObj.transform.Find(plif[k].spotName).gameObject;
                    float disCul = Vector2.Distance(StageData.spotList[index].getPosition().transform.localPosition, spottemp.transform.localPosition);
                    if (disCul < stageData.getDist()) distanceBool = true;
                }
                if (distanceBool) continue;
            }
            if (!StageData.spotList[index].plunderActive) break;
        }
        StageData.spotList[index].plunderActive = true;


        //랜덤으로 리스트에 ai 정보 넣기
        PlunderInfo plInfo = plunderInfoList.Find(x => x.opponentName == plunder.getName());    //해당 스팟
        while (true)
        {
            random = Random.Range(0, 40);
            Plunder plunderTmp = StageData.instance.getPlundeList()[random];

            if (plunderTmp.assignment == false)
            {
                plInfo.opponentName = plunderTmp.getName();
                break;
            }
            else continue;
        }
        //기존 상대 데이터의 스팟 할당 해제
        plunder.assignment = false;
        result.regen = true;
        result.time = 30f;

        GameObject spotButton = WorldMapBackObj.transform.Find("Plunder/" + result.PlunderName + "Button").gameObject;
        spotButton.transform.localPosition = StageData.spotList[index].getPosition().localPosition;

        result.spotName = StageData.spotList[index].getPosition().name;
        result.PlunderName = "plunder" + result.getPlunderNum().ToString();
        spotButton.transform.Find("StageText").GetComponent<Text>().text = result.opponentName;
        spotButton.transform.Find("State/NameText").GetComponent<Text>().text = result.opponentName;
        spotButton.name = result.PlunderName + "Button"; //오브젝트 이름 변경

        spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 0.5f);
        //spotButton.GetComponent<Image>().sprite = result.sprite;



        //애니메이션 효과 투닥투닥 15프레임 정도

        //계산
        bool dpsResult = dpsCal(plunder);

        //결과
        //획득 가능한 아이템->획득한 아이템
        plunderItemBox.transform.Find("ItemTitleText").gameObject.GetComponent<Text>().text = "획득한 아이템";
        //아이템 자동 획득, 경험치

        //약탈 버튼->확인 버튼
        plunderPopup.transform.Find("UIPanel/PlunderButton").gameObject.SetActive(false);
        plunderPopup.transform.Find("UIPanel/OKButton").gameObject.SetActive(true);
        //승리 패배 표시 애니메이션 
        //승리
        if (dpsResult)
        {
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(40);
            plunderPopup.transform.Find("FrontBox").gameObject.SetActive(true);
            ClearFail.SetActive(true);
            StartCoroutine(closeWin());
        }
        //패배
        else
        {
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(10);
            plunderPopup.transform.Find("FrontBox").gameObject.SetActive(true);
            //애니
            StartCoroutine(closeLose());
        }
        //아이템 하나씩 띄우기 애니메이션
        //코루틴

    }

    IEnumerator closeWin()
    {
        yield return new WaitForSeconds(2.0f);
        ClearFail.SetActive(false);
        plunderPlayerBox.transform.Find("Win").gameObject.SetActive(true);
        plunderPlayerBox.transform.Find("Lose").gameObject.SetActive(false);
        plunderEnemyBox.transform.Find("Win").gameObject.SetActive(false);
        plunderEnemyBox.transform.Find("Lose").gameObject.SetActive(true);
        plunderPopup.transform.Find("FrontBox").gameObject.SetActive(false);
    }

    IEnumerator closeLose()
    {
        yield return new WaitForSeconds(2.0f);
        ClearFail.SetActive(false);
        plunderPlayerBox.transform.Find("Win").gameObject.SetActive(false);
        plunderPlayerBox.transform.Find("Lose").gameObject.SetActive(true);
        plunderEnemyBox.transform.Find("Win").gameObject.SetActive(true);
        plunderEnemyBox.transform.Find("Lose").gameObject.SetActive(false);
        plunderPopup.transform.Find("FrontBox").gameObject.SetActive(false);
    }
    //아이템 코루틴




    //약탈 대상과 전투력 비교 return
    public bool dpsCal(Plunder plunder)
    {
        Debug.Log(plunder.stat.dps);
        Debug.Log(statData.getRepreSetStat().dps);
        //플레이어의 전투력이 높은 경우에만 승리
        if (statData.getRepreSetStat().dps > plunder.stat.dps)
            return true;
        else
            return false;
    }


    //약탈 새로고침
    public void refreshButton()
    {
        int random = 0;
        for (int i = 0; i < 20; i++)
        {
            if (plunderInfoList[i].regen) continue;

            StageData.instance.getPlundeList().Find(x => x.getName() == plunderInfoList[i].opponentName).assignment = false;
            plunderInfoList[i].opponentName = null;

            //랜덤으로 리스트에 ai 정보 넣기
            while (true)
            {
                random = Random.Range(0, 40);
                //중복 방지
                List<PlunderInfo> plif = plunderInfoList.FindAll(x => x.opponentName != null);
                if (plif != null)
                {
                    bool flag = false;
                    for (int k = 0; k < plif.Count; k++)
                    {
                        if (plif[k].opponentName == StageData.instance.getPlundeList()[random].getName())
                        {
                            flag = true;
                        }
                    }
                    if (flag) continue; else break;
                }
                else break;
            }
            plunderInfoList[i].opponentName = StageData.instance.getPlundeList()[random].getName();
            StageData.instance.getPlundeList()[random].assignment = true;
            GameObject obj = GameObject.Find("plunder" + (i + 1).ToString() + "Button");
            obj.transform.Find("StageText").gameObject.GetComponent<Text>().text = StageData.instance.getPlundeList()[random].getName();
            obj.transform.Find("State/NameText").gameObject.GetComponent<Text>().text = StageData.instance.getPlundeList()[random].getName();
            obj.transform.Find("State/LevelText").gameObject.GetComponent<Text>().text = StageData.instance.getPlundeList()[random].level.ToString();
        }

    }




    //모드 전환
    public void modeChange(bool b)
    {
        mode = b;
    }

    //typeNum string 변환
    public string typeNumToString(int i) { if (i == 1) return "소"; else if (i == 2) return "중"; else if (i == 3) return "대"; else return null; }
    //typeNum time 변환
    public float typeNumToTime(int i) { if (i == 1) return 300f; else if (i == 2) return 900f; else if (i == 3) return 1800f; else return 0; }
    //typeNum regen time 30m~1h
    public float typeNumToRegenTime() { int random = Random.Range(1800, 3600 + 1); return random; }

    public List<StageInfo> getStageInfoList() { return stageInfoList; }
    public void setStageInfoList(List<StageInfo> list) { stageInfoList = list; }
    public List<PlunderInfo> getPlunderInfoList() { return plunderInfoList; }
    public void setPlunderInfoList(List<PlunderInfo> list) { plunderInfoList = list; }


    public int getCurStageSelect() { return curStageSelect; }
    public void SetCurStageSelect(int cur) { curStageSelect = cur; }
}




