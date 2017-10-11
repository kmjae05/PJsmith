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

    private WorldMapManager worldmapManager;    //worldmap info
    private MercenaryManager mercenaryManager;  //mercenary info

    //스테이지 정보
    private StageData stageData;
    private List<StageInfo> stageInfoList;
    private List<StageInfo> stageInfoListtmp;   //임시 스테이지 정보 저장공간

    private string curContSelect;              //현재 선택된 대륙
    private int curStageSelect;              //현재 선택된 스테이지
    private float stageTime = 1800f;

    //스테이지 정보 팝업창
    private GameObject stagePopup;
    private Text nameText;
    private GameObject stageGetItemBox;

    //스테이지 현황 팝업창
    private GameObject stageStatePopup;

    private GameObject selectFrame;     //용병 선택 표시
    private bool selectMerFlag = false;         //용병 선택
    private GameObject selectMerObj;            //


    private GameObject systemPopup; //시스템 팝업
    private Button sys_yesButton;
    private GameObject imdComPopup; //즉시 완료 팝업
    private Button imd_yesButton;


    private void Start()
    {
        worldmapManager = GameObject.Find("Menu").transform.Find("WorldMapPopup").gameObject.GetComponent<WorldMapManager>();
        mercenaryManager = GameObject.Find("StageManager").GetComponent<MercenaryManager>();
        stageData = GameObject.Find("StageData").GetComponent<StageData>();
        stageInfoList = new List<StageInfo>();
        stageInfoListtmp = new List<StageInfo>();

        stagePopup = GameObject.Find("System").transform.Find("StagePopup").gameObject;
        stageGetItemBox = stagePopup.transform.Find("UIPanel/GetItemBox").gameObject;
        stageStatePopup = GameObject.Find("System").transform.Find("StageStatePopup").gameObject;


        stageInfoList = stageData.getStageInfoList();

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
        for (int j = 1; j < 6; j++)
        {
            //스팟 transform 설정
            GameObject spottmp = GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage" + j.ToString() + "/CONUIPanel/Back/Spot").gameObject;
            int count = spottmp.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                StageData.spotList[StageData.spotList.FindIndex(
                    x => x.getContName() == stageData.contNumToString(j) && x.getSpotName() == spottmp.transform.GetChild(i).name)].position 
                    = spottmp.transform.GetChild(i);
            }

            List<StageInfo> sList = stageInfoList.FindAll(x => x.getContName() == stageData.contNumToString(j));
            for (int i = 0; i < sList.Count; i++)
            {
                if (sList[i].spotName != null)
                {
                    string spotName = sList[i].spotName;
                    //스팟 인덱스
                    int index = StageData.spotList.FindIndex(x => x.getContName() == stageData.contNumToString(j) && x.getSpotName() == spotName);
                    
                    GameObject spotButton = Instantiate(GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage"+j.ToString()+"/CONUIPanel/Back/SpotButton").gameObject);
                    spotButton.transform.SetParent(GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage"+j.ToString()+"/CONUIPanel/Back/Stage").gameObject.transform);
                    spotButton.transform.localPosition = StageData.spotList[index].getPosition().localPosition;
                    spotButton.SetActive(true);
                    spotButton.transform.Find("StageText").GetComponent<Text>().text = sList[i].getStageNum().ToString();
                    spotButton.name = sList[i].stageName + "Button"; //오브젝트 이름 변경
                    spotButton.GetComponent<Image>().sprite = sList[i].sprite;
                    if (sList[i].state) {
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
        }

    }



    private void Update()
    {
        stageInfoList = stageData.getStageInfoList();
        stageInfoListtmp.Clear();

        //탐험 완료 시 버튼 변경.
        for (int i = 0; i < stageInfoList.Count; i++)
        {
            if (stageStatePopup.activeInHierarchy && stageInfoList[i].complete && 
                curStageSelect == stageInfoList[i].getStageNum() && curContSelect == stageInfoList[i].getContName())
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
            StageInfo stin = stageInfoList.Find(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect);
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
        if (GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage" + worldmapManager.getContNum().ToString()).gameObject.activeInHierarchy)
        {
            stageInfoListtmp.Clear();
            
            //시간
            stageInfoListtmp = stageInfoList.FindAll(x => x.state == true && x.getContName() == curContSelect);
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
                    GameObject.Find(stageName + "Button").transform.Find("GetItemEff/Text").gameObject.GetComponent<Text>().text = "+"+stageInfoListtmp[i].getRecentItemNum;
                    StartCoroutine(getItemEff(stageInfoListtmp[i], stageName));
                }

            }
            //완료
            stageInfoListtmp = stageInfoList.FindAll(x => x.complete == true && x.getContName() == curContSelect);
            for (int i = 0; i < stageInfoListtmp.Count; i++)
            {
                string stageName = stageInfoListtmp[i].stageName;
                GameObject.Find(stageName + "Button").transform.Find("State").gameObject.SetActive(true);
                GameObject.Find(stageName + "Button").transform.Find("State/Text").gameObject.GetComponent<Text>().text = "완료";

                GameObject.Find(stageName + "Button").transform.Find("State/Progress/pickax").gameObject.SetActive(false);
                GameObject.Find(stageName + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(false);
            }
            //리젠
            stageInfoListtmp = stageInfoList.FindAll(x => x.regen == true && x.getContName() == curContSelect);
            for (int i = 0; i < stageInfoListtmp.Count; i++)
            {
                string stageName = stageInfoListtmp[i].stageName;
                float time = stageInfoListtmp[i].time;
                GameObject.Find(stageName + "Button").transform.Find("State").gameObject.SetActive(true);
                GameObject.Find(stageName + "Button").transform.Find("State/Text").gameObject.GetComponent<Text>().text
                    = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
                GameObject.Find(stageName + "Button").transform.Find("State/Progress/pickax").gameObject.SetActive(false);
                GameObject.Find(stageName + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(false);
            }
            //대기 상태
            stageInfoListtmp = stageInfoList.FindAll(x => (x.wait == true && x.getContName() == curContSelect));
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
        curStageSelect = System.Convert.ToInt32( obj.transform.Find("StageText").GetComponent<Text>().text);
        
        StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect);

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
            nameText.text = result.getContName() + " " + result.type + " " + result.typeNum.ToString();
            GameObject.Find("StageTimeText").GetComponent<Text>().text = 
                "소요 시간 " + ((int)(typeNumToTime(result.typeNum)/60)).ToString() + "m " + ((int)(typeNumToTime(result.typeNum) % 60)).ToString() + "s";
            //stage에 따라 획득 가능한 아이템
            setGetItemInfo(result);

        }
        //용병 보낸 상태
        else
        {
            //스테이지 현황 팝업창
            stageStatePopup.SetActive(true);
            GameObject.Find("StageStateText").GetComponent<Text>().text = result.getContName() + " " + result.type + " " + result.typeNum.ToString();
            stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(true);

            Text timeText = GameObject.Find("TimeBox").transform.Find("Text").gameObject.GetComponent<Text>();
            float time = stageInfoList.Find(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect).time;
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
            StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect);
            GameObject.Find("StageStateText").GetComponent<Text>().text = result.getContName() + " " + result.type + " " + result.typeNum.ToString();
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
            stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect)] = result;

            Mercenary mer = mercenaryManager.getMercenary().Find(x => x.getName() == mercenaryManager.getCurSelect());
            mer.setContName(result.getContName());
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
            float time = stageInfoList.Find(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect).time;
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
        if(money == "골드")
            imdComPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = money + " "+ goldPrice+ "개를 사용하여 즉시 완료하시겠습니까?";
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

        StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect);
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
        stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect)] = result;
        GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(false);
        GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(true);
        GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage"+worldmapManager.getContNum().ToString()+"/CONUIPanel/Back/Stage/" + result.stageName + "Button/State/Text").gameObject.GetComponent<Text>().text = "완료";

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
                StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect);
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
                GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage" + stageData.contStringToInt(result.getContName()) + "CONUIPanel/Back/Stage/stage" + result.getStageNum().ToString() + "Button/MercImage").gameObject.SetActive(false);

                Mercenary mer = mercenaryManager.getMercenary().Find(x => x.getName() == result.mercenaryName);
                mer.setContName(null);
                mer.setStageNum(0);
                mer.setState(false);
                mercenaryManager.setMercenaryIndex(mercenaryManager.getMercenary().FindIndex(x => x.getName() == mer.getName()), mer);
                stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(false);

                result.mercenaryName = null;
                stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect)] = result;

                //stagePopup.SetActive(true);
                //GameObject.Find("StageNameText").GetComponent<Text>().text = "스테이지 " + curStageSelect;
                stageStatePopup.SetActive(false);

                GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage" + worldmapManager.getContNum().ToString() + "/CONUIPanel/Back/Stage/" + result.stageName + "Button/State").gameObject.SetActive(false);
            }
            );  //버튼 기능 추가
    }



    //보상 받기 버튼
    public void CompleteButton()
    {
        StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect);

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
            Vector3 getItemListObj = GameObject.Find("Mercenary"+result.mercenaryName+"Button").transform.position; // 시작 위치
            GameObject stageItem = GameObject.Find("StageItemImagePos").transform.Find("StageItemImage").gameObject;
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(30);
            StartCoroutine(createGetItem(result, stageItem, "StageItemImagePos", getItemListObj));
        }


        result.complete = false;
        result.getItemTimeFlag = false;
        GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection").GetComponent<Button>().interactable = true;

        GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage" + stageData.contStringToInt(result.getContName()) + "/CONUIPanel/Back/Stage/stage" + result.getStageNum().ToString() + "Button/MercImage").gameObject.SetActive(false);
        
        Mercenary mer = mercenaryManager.getMercenary().Find(x => x.getName() == result.mercenaryName);

        mer.setContName(null);
        mer.setStageNum(0);
        mer.setState(false);
        mercenaryManager.setMercenaryIndex(mercenaryManager.getMercenary().FindIndex(x => x.getName() == mer.getName()), mer);

        stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(false);
        result.mercenaryName =null;

        //리젠
        result.regen = true;
        StageData.spotList[StageData.spotList.FindIndex(x => x.getContName() == result.getContName() && x.stageNum == result.getStageNum())].active = false;
        int random = 0;
        int index = 0;

        List<Spot> sList = StageData.spotList.FindAll(x => x.getContName() == result.getContName());
        while (true)
        {
            random = Random.Range(1, sList.Count + 1);
            index = StageData.spotList.FindIndex(x => x.getContName() == result.getContName() && x.getPosition().name == "spot" + random.ToString());
            //이미 위치한 스테이지 범위에 없게 배치
            List<StageInfo> stif = stageInfoList.FindAll(x => x.getContName() == StageData.spotList[index].getContName() && x.spotName != null);
            if (stif != null)
            {
                bool distanceBool = false;
                for (int k = 0; k < stif.Count; k++)
                {
                    GameObject spottmp = GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage" + stageData.contStringToInt(result.getContName()).ToString() + "/CONUIPanel/Back/Spot/" + stif[k].spotName).gameObject;
                    float disCul = Vector2.Distance(StageData.spotList[index].getPosition().transform.localPosition, spottmp.transform.localPosition);
                    if (disCul < stageData.getDist()) distanceBool = true;
                }
                if (distanceBool) continue;
            }
            if (StageData.spotList[index].active == false) break;
        }
        GameObject spotButton = GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage"+stageData.contStringToInt(result.getContName()).ToString() + "/CONUIPanel/Back/Stage/"+ result.stageName + "Button").gameObject;
        spotButton.transform.localPosition = StageData.spotList[index].getPosition().localPosition;
        //스팟이랑 스테이지 정보 공유
        StageData.spotList[index].stageNum = result.getStageNum();    //스테이지 번호 저장
        StageData.spotList[index].active = true;    //스팟 활성화
        result.spotName = StageData.spotList[index].getPosition().name;
        random = Random.Range(1, 3 + 1);
        result.type = stageData.typeNumToString(result.getStageNum(), random);
        random = Random.Range(0, 100);
        if (random < 50) result.typeNum = 1;
        else if (random > 49 && random < 90) result.typeNum = 2;
        else if (random > 89) result.typeNum = 3;
        result.stageName = "stage" + result.getStageNum().ToString();   
        spotButton.transform.Find("StageText").GetComponent<Text>().text = result.getStageNum().ToString();  //
        spotButton.name = result.stageName + "Button"; //오브젝트 이름 변경
        stageData.stageImageChange(result);

        result.time = typeNumToRegenTime();             //리젠 시간 설정
        spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 0.5f);
        spotButton.GetComponent<Image>().sprite = result.sprite;

        //스테이지 변경된 정보 저장
        stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect && x.getContName() == curContSelect)] = result;
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
            if(result.getItem[i] == null)  continue;
            if (GameObject.Find("Menu").transform.Find("WorldMapPopup").gameObject.activeInHierarchy)
            {
                GameObject ItemInst = Instantiate(stageItem);
                ItemInst.transform.SetParent(GameObject.Find(createPos).transform, false);
                //위치
                ItemInst.transform.position = vec;
                //이미지 변경
                Image itemimage = ItemInst.GetComponent<Image>();
                ItemImageChange(result.getItem[i], itemimage);
                ItemInst.SetActive(true);
            }
            //로그
            GameObject ItemLogInst = Instantiate(GameObject.Find("GetItemLog").transform.Find("range/GetItemLogText").gameObject);
            ItemLogInst.transform.SetParent(GameObject.Find("GetItemLog").transform.Find("range").gameObject.transform, false);
            ItemLogInst.GetComponent<Text>().text = result.getItem[i] + " " + result.getItemNum[i] + "개 획득";
            ItemLogInst.SetActive(true);

            //획득 아이템 데이터 저장
            ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).possession += result.getItemNum[i];
            ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).recent = true;


            yield return new WaitForSeconds(0.3f);
        }
    }

    //스테이지 현황 창 닫기
    public void closeStageStatePopup()
    {
        for(int i=0;i<mercenaryManager.getMercenary().Count;i++)
            if(mercenaryManager.getMercenary()[i].getName() !=null)
                stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + mercenaryManager.getMercenary()[i].getName()).gameObject.SetActive(false);
    }







    //스테이지 정보창 획득 가능한 아이템 정리
    public void setGetItemInfo(StageInfo result)
    {
        ////사냥
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




    //typeNum string 변환
    public string typeNumToString(int i) { if (i == 1) return "소"; else if (i == 2) return "중"; else if (i == 3) return "대"; else return null; }
    //typeNum time 변환
    public float typeNumToTime(int i) { if (i == 1) return 300f; else if (i == 2) return 900f; else if (i == 3) return 1800f; else return 0; }
    //typeNum regen time 30m~1h
    public float typeNumToRegenTime() { int random = Random.Range(1800, 3600 + 1); return random;  }

    public List<StageInfo> getStageInfoList() { return stageInfoList; }
    public void setStageInfoList(List<StageInfo> list) { stageInfoList = list; }

    public int getCurStageSelect() { return curStageSelect; }
    public void SetCurStageSelect(int cur) { curStageSelect = cur; }
    public string getCurContSelect() { return curContSelect; }
    public void SetCurContSelect(string cur) { curContSelect = cur; }
}



