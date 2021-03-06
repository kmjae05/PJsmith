﻿using System.Collections;
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

    private Camera worldCam;
    private Camera uiCam;
    private Camera worldCam1;
    private Camera uiCam1;

    private MercenaryManager mercenaryManager;  //mercenary info
    private StatData statData;
    private MonsterData monsterdata;

    //스테이지 정보
    private StageData stageData;
    private List<StageInfo> stageInfoList;
    private List<StageInfo> stageInfoListtmp;   //임시 스테이지 정보 저장공간
    //약탈 정보
    private List<PlunderInfo> plunderInfoList;

    //private string curContSelect;              //현재 선택된 대륙
    private int curStageSelect;               //현재 선택된 스테이지 번호
    private GameObject curSelectObj;            //현재 선택된 스테이지 오브젝트
    private string curPlunderSelect;          //현재 선택된 상대 플레이어
    private float stageTime = 1800f;

    //스테이지 정보 팝업창
    private GameObject stagePopup;
    private Text nameText;
    private GameObject stageGetItemBox;
    private GameObject itemListObj;
    private GameObject ItemBox;

    private GameObject stageStatePopup;     //스테이지 현황 팝업창
    private GameObject stateItemList;
    private GameObject stateItemBox;
    private GameObject plunderPopup;        //약탈 팝업
    private GameObject plunderPlayerBox;
    private GameObject plunderEnemyBox;
    private GameObject plunderItemListObj;
    private GameObject plunderItemBox;

    private GameObject ClearFail;
    private GameObject Fail;

    private GameObject selectFrame;     //용병 선택 표시
    private bool selectMerFlag = false;         //용병 선택
    private GameObject selectMerObj;            //

    private GameObject SpotObj;
    private GameObject SpotButtonObj;
    private GameObject Monster;
    private GameObject[] MonsterObj;
    private List<GameObject> MonsterObjList;
    private GameObject light;


    private GameObject StageObj;
    private GameObject PlunderObj;
    private GameObject PlunderButtonObj;
    private GameObject WorldMapBackObj;

    private GameObject systemPopup; //시스템 팝업
    private Button sys_yesButton;
    private GameObject imdComPopup; //즉시 완료 팝업
    private Button imd_yesButton;


    private System.DateTime nowTime;
    private System.DateTime startTime;

    private void Start()
    {
        worldCam = GameObject.Find("Monster_Camera").GetComponent<Camera>();
        uiCam = GameObject.Find("UI_Camera").GetComponent<Camera>();
        worldCam1 = GameObject.Find("Monster_Camera (1)").GetComponent<Camera>();
        uiCam1 = GameObject.Find("UI_Camera (1)").GetComponent<Camera>();

        //worldmapManager = GameObject.Find("Menu").transform.Find("WorldMap").gameObject.GetComponent<WorldMapManager>();
        mercenaryManager = GameObject.Find("StageManager").GetComponent<MercenaryManager>();
        statData = GameObject.Find("PlayerManager").GetComponent<StatData>();
        stageData = GameObject.Find("StageData").GetComponent<StageData>();
        monsterdata = GameObject.Find("StageManager").GetComponent<MonsterData>();
        stageInfoList = new List<StageInfo>();
        stageInfoListtmp = new List<StageInfo>();
        plunderInfoList = new List<PlunderInfo>();

        stagePopup = GameObject.Find("System").transform.Find("StagePopup").gameObject;
        stagePopup.SetActive(true);
        stagePopup.SetActive(false);
        stageGetItemBox = stagePopup.transform.Find("UIPanel/GetItemBox").gameObject;
        itemListObj = stageGetItemBox.transform.Find("Scroll/ItemList").gameObject;
        ItemBox = itemListObj.transform.Find("ItemBox").gameObject;

        stageStatePopup = GameObject.Find("System").transform.Find("StageStatePopup").gameObject;
        stateItemList = stageStatePopup.transform.Find("StageStatePanel/GetItemList/Scroll/ItemList").gameObject;
        stateItemBox = stateItemList.transform.Find("ItemBox").gameObject;

        SpotObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Spot").gameObject;
        SpotButtonObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/SpotButton").gameObject;
        Monster = GameObject.Find("Monster");
        MonsterObj = new GameObject[3];
        MonsterObj[0] = Monster.transform.Find("Scollwarrior").gameObject;
        MonsterObj[1] = Monster.transform.Find("Keurot").gameObject;
        MonsterObj[2] = Monster.transform.Find("Syaonil").gameObject;

        MonsterObjList = new List<GameObject>();
        light = Monster.transform.Find("Spotlight").gameObject;
        curSelectObj = new GameObject();

        StageObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject;
        PlunderObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Plunder").gameObject;
        PlunderButtonObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/PlunderButton").gameObject;
        WorldMapBackObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back").gameObject;
        plunderPopup = GameObject.Find("System").transform.Find("PlunderPopup").gameObject;
        plunderPlayerBox = plunderPopup.transform.Find("UIPanel/PlayerBox").gameObject;
        plunderEnemyBox = plunderPopup.transform.Find("UIPanel/EnemyBox").gameObject;
        plunderItemListObj = plunderPopup.transform.Find("UIPanel/ItemBox/Scroll/ItemList").gameObject;
        plunderItemBox = plunderItemListObj.transform.Find("ItemBox").gameObject;
        ClearFail = GameObject.Find("System").transform.Find("ClearFail").gameObject;
        Fail = GameObject.Find("System").transform.Find("Fail").gameObject;

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

        GameObject.Find("Menu").transform.Find("WorldMap (1)/Stage/CloseButton").gameObject.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.SetActive(true);
                GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Plunder").gameObject.SetActive(false);
                GameObject.Find("Menu").transform.Find("WorldMap (1)/Stage/PlunderButton").gameObject.SetActive(true);
                GameObject.Find("Menu").transform.Find("WorldMap (1)/Stage/HuntingButton").gameObject.SetActive(false);
                mode = true;
            });


        //위치 재배치
        //stage버튼 개수만큼 각 대륙별로 버튼 생성. 배치.
        //스팟 transform 설정
        int count = SpotObj.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            StageData.spotList[StageData.spotList.FindIndex(x => x.getSpotName() == SpotObj.transform.GetChild(i).name)].position = SpotObj.transform.GetChild(i);
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
                spotButton.transform.Find("NameImage/NameText").GetComponent<Text>().text = sList[i].type + " " + sList[i].typeNum.ToString();
                spotButton.name = sList[i].stageName + "Button"; //오브젝트 이름 변경
                //spotButton.GetComponent<Image>().sprite = sList[i].sprite;
                if (sList[i].state)
                {
                    spotButton.transform.Find("State/Progress/sword").gameObject.SetActive(true);
                    spotButton.transform.Find("State/Progress/Dust").gameObject.SetActive(true);
                    spotButton.transform.Find("MercImage").gameObject.SetActive(true);
                    string merImageName = sList[i].mercenaryName;
                    spotButton.transform.Find("MercImage/Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Mercenary/" + merImageName);
                }

                //3D몬스터 생성
                int num = 0;
                if (sList[i].type == "전갈")
                    num = 0;
                else if (sList[i].type == "오쿰") num = 1;
                else if (sList[i].type == "인큐버스") num = 2;
                MonsterObjList.Add(Instantiate(MonsterObj[num].gameObject));
                MonsterObjList[i].transform.SetParent(GameObject.Find("Monster").transform);


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
                plunderButton.transform.Find("State/NameImage/NameText").gameObject.GetComponent<Text>().text = "Lv" + (1).ToString() + " " + pList[i].opponentName;
                plunderButton.transform.Find("State/NameImage/NameText").gameObject.GetComponent<Text>().color = new Color(1f, 0.2f, 0.21f);

                //리젠 상태일 때 반투명
                if (pList[i].regen) plunderButton.GetComponent<Image>().color = new Color(plunderButton.GetComponent<Image>().color.r, plunderButton.GetComponent<Image>().color.g, plunderButton.GetComponent<Image>().color.b, 0.5f);
                else plunderButton.GetComponent<Image>().color = new Color(plunderButton.GetComponent<Image>().color.r, plunderButton.GetComponent<Image>().color.g, plunderButton.GetComponent<Image>().color.b, 1.0f);
            }
        }

        //사냥 중인 용병 비활성화 처리
        for (int i = 0; i < stageInfoList.Count; i++)
        {
            if (stageInfoList[i].mercenaryName != null)
            {
                GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + stageInfoList[i].mercenaryName + "Selection").GetComponent<Button>().interactable = false;
                Color col = GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + stageInfoList[i].mercenaryName + "Selection/Image").GetComponent<Image>().color;
                GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + stageInfoList[i].mercenaryName + "Selection/Image").GetComponent<Image>().color = new Color(col.r, col.g, col.b, 0.5f);
            }
        }
    }



    private void Update()
    {
        stageInfoList = stageData.getStageInfoList();
        plunderInfoList = stageData.getPlunderInfoList();
        stageInfoListtmp.Clear();

        GameObject.Find("Menu").transform.Find("WorldMapBack/Stage/UIPanel/Back/Smithy/Level/LevelText").gameObject.GetComponent<Text>().text 
            = "Lv" + Player.instance.getUser().level.ToString() + " 대장간";

        GameObject.Find("Menu").transform.Find("WorldMapBack/Stage/UIPanel/Back").gameObject.transform.position
            = WorldMapBackObj.transform.position;


        if (GameObject.Find("Menu").transform.Find("WorldMap").gameObject.activeInHierarchy)
            SetPositionHUD();
        if (GameObject.Find("System").transform.Find("StagePopup").gameObject.activeInHierarchy)
            SetPositionHUDPopup();


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
                stageStatePopup.transform.Find("StageStatePanel/success").gameObject.SetActive(true);
                
                
            }
        }

        //스테이지 현황 창 활성화 중일 때.
        if (stageStatePopup.activeInHierarchy)
        {
            StageInfo stin = stageInfoList.Find(x => x.getStageNum() == curStageSelect);
            Text timeText = GameObject.Find("TimeBox").transform.Find("Text").gameObject.GetComponent<Text>();
            System.TimeSpan leadTime = stin.leadTime;
            float min = (stin.time + leadTime - System.DateTime.Now).Minutes;
            float sec = (stin.time + leadTime - System.DateTime.Now).Seconds;
            timeText.text = "남은 시간 : " + ((int)min).ToString() + "분 " + ((int)sec).ToString() + "초";
            if (stin.complete&& curStageSelect == stin.getStageNum())
                timeText.text = "사냥 완료";

            bool nothing = false;
            for (int i = 0; i < stin.getItem.Length; i++)
            {
                if (stin.getItem[i] != null)
                { nothing = true; }
            }
            if (nothing) { stageStatePopup.transform.Find("StageStatePanel/NothingText").gameObject.SetActive(false); }
            else { stageStatePopup.transform.Find("StageStatePanel/NothingText").gameObject.SetActive(true); }

            //새로 얻은 아이템
            if (stin.getRecentItemFlag)
            {
                stin.getRecentItemFlag = false;
                Debug.Log("1111");
                for (int i = 0; i < stin.getItem.Length; i++)
                {
                    if (stin.getItem[i] != null && stin.getItem[i] == stin.getRecentItem)
                    {
                        Debug.Log("i : " + i);
                        Debug.Log(stin.getItemNum[i]);
                        //최근 획득한 아이템이 기존에 있을 경우
                        if (stin.getItemNum[i] > 1)/////
                        {
                            Debug.Log("기존");
                            GameObject.Find("statePopupGetItem" + stin.getItem[i]).transform.Find("AmountText").gameObject.GetComponent<Text>().text = stin.getItemNum[i].ToString();
                            break;
                        }
                        else
                        {
                            Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == stin.getRecentItem).grade);
                            stateItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;
                            stateItemBox.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == stin.getRecentItem).icon);
                            stateItemBox.transform.Find("AmountText").gameObject.GetComponent<Text>().text = stin.getItemNum[i].ToString();
                            stateItemBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = stin.getRecentItem;

                            GameObject boxobj = Instantiate(stateItemBox);
                            boxobj.transform.SetParent(stateItemList.transform, false);
                            boxobj.name = "statePopupGetItem" + stin.getRecentItem;
                            boxobj.SetActive(true);
                            break;
                        }
                    }
                }
            }

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
                    System.TimeSpan time = System.DateTime.Now - stageInfoListtmp[i].time;

                    GameObject.Find(stageName + "Button").transform.Find("State").gameObject.SetActive(true);

                    //획득 효과
                    if (stageInfoListtmp[i].getRecentItemFlag)
                    {
                        Image spr = GameObject.Find(stageName + "Button").transform.Find("GetItemEff/GetItemFrame/GetItemImage").gameObject.GetComponent<Image>();
                        GameObject.Find(stageName + "Button").transform.Find("GetItemEff/GetItemFrame").gameObject.GetComponent<Image>().color
                            = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == stageInfoListtmp[i].getRecentItem).grade);
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

                    GameObject.Find(stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().value = GameObject.Find(stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().maxValue;
                    GameObject.Find(stageName + "Button").transform.Find("State/TimeSlider/TimeText").gameObject.GetComponent<Text>().text = "완료";
                    GameObject.Find(stageName + "Button").transform.Find("State/Progress/pickax").gameObject.SetActive(false);
                    GameObject.Find(stageName + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(false);
                    GameObject.Find(stageName + "Button").transform.Find("State/Progress/Dust").gameObject.SetActive(false);
                    if(stageStatePopup.activeInHierarchy)
                        imdComPopup.SetActive(false);
                }
                //리젠
                stageInfoListtmp = stageInfoList.FindAll(x => x.regen == true);
                for (int i = 0; i < stageInfoListtmp.Count; i++)
                {
                    string stageName = stageInfoListtmp[i].stageName;
                    //float time = stageInfoListtmp[i].time;
                    if (GameObject.Find(stageName + "Button").activeInHierarchy)
                    {
                        GameObject.Find(stageName + "Button").transform.Find("State").gameObject.SetActive(true);
                        //GameObject.Find(stageName + "Button").transform.Find("State/Text").gameObject.GetComponent<Text>().text
                        //    = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
                        GameObject.Find(stageName + "Button").transform.Find("State/TimeSlider").gameObject.SetActive(false);
                        GameObject.Find(stageName + "Button").transform.Find("State/Progress/pickax").gameObject.SetActive(false);
                        GameObject.Find(stageName + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(false);
                        GameObject.Find(stageName + "Button").transform.Find("State/Progress/Dust").gameObject.SetActive(false);
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
                        //GameObject.Find(stageName + "Button").transform.Find("State/NameImage/NameText").gameObject.GetComponent<Text>().text
                        //    = ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
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
                            GameObject.Find(stageName + "Button").transform.Find("State/NameImage/NameText").gameObject.GetComponent<Text>().text = "Lv" +
                                StageData.instance.getPlunderList().Find(x => x.getName() == plunderInfotmp[i].opponentName).level.ToString() + " " + plunderInfotmp[i].opponentName;
                    }
                }
            }

            //기상 변화
            if (StageData.instance.rainFlag)
            {
                Monster.transform.Find("Rain").gameObject.SetActive(true);
            }
            else
            {
                Monster.transform.Find("Rain").gameObject.SetActive(false);
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
        curSelectObj = obj;
        selectMerFlag = false;
        curStageSelect = System.Convert.ToInt32(obj.transform.Find("StageText").GetComponent<Text>().text);

        StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect);

        //리젠 상태
        if (result.regen)
        {
            return;
        }
        //용병 안 보낸 상태
        else if (!result.state && !result.complete && !result.mermove)
        {
            //스테이지 정보 창
            stagePopup.SetActive(true);
            nameText = GameObject.Find("StageNameText").GetComponent<Text>();
            selectFrame = stagePopup.transform.Find("UIPanel/MercenaryBox/selectFrame").gameObject;
            selectFrame.SetActive(false);
            nameText.text = result.type + " " + result.typeNum.ToString();
            float dist = Vector3.Distance(curSelectObj.transform.localPosition, GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.transform.localPosition);
            int time = (int)(dist / 10);
            GameObject.Find("StageTimeText").GetComponent<Text>().text =
                "이동 시간 " + (time/60).ToString() + "분 " + (time%60).ToString() + "초";
            //stage에 따라 획득 가능한 아이템
            setGetItemInfo(result);


            //몬스터 표시
            GameObject monsterObj = GameObject.Find("01_3D").transform.Find("Monster (1)").gameObject;
            monsterObj.SetActive(true);
            for (int i = 0; i < monsterObj.transform.childCount; i++)
            {
                monsterObj.transform.GetChild(i).gameObject.transform.position = new Vector3(60, 60, 60);
            }
            Button close = stagePopup.transform.Find("UIPanel/CloseButton").gameObject.GetComponent<Button>();
            close.onClick.RemoveAllListeners();

        }
        ////용병 보낸 상태
        //else if(result.mermove)
        //{
        //    //스테이지 현황 팝업창
        //    stageStatePopup.SetActive(true);
        //    GameObject.Find("StageStateText").GetComponent<Text>().text = result.type + " " + result.typeNum.ToString();
        //    stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(true);

        //    Text timeText = GameObject.Find("TimeBox").transform.Find("Text").gameObject.GetComponent<Text>();
        //    //float time = stageInfoList.Find(x => x.getStageNum() == curStageSelect).time;
        //    //timeText.text = "남은 시간 : " + ((int)(time / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";

        //    //얻은 아이템
        //    int num = result.getItem.Length;
        //    string[] item = new string[num];

        //    for (int i = 0; i < num; i++)
        //    {
        //        item[i] = result.getItem[i];
        //        if (item[i] != null)
        //        {
        //            Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).grade);
        //            stateItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;
        //            stateItemBox.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).icon);
        //            stateItemBox.transform.Find("AmountText").gameObject.GetComponent<Text>().text = result.getItemNum[i].ToString();
        //            stateItemBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = item[i];

        //            GameObject boxobj = Instantiate(stateItemBox);
        //            boxobj.transform.SetParent(stateItemList.transform, false);
        //            boxobj.name = "statePopupGetItem" + item[i];
        //            boxobj.SetActive(true);
        //        }
        //    }

        //    // stateItemList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + stateItemList.GetComponent<RectTransform>().sizeDelta.x / 2, 0);



        //    //완료
        //    if (result.complete)
        //    {
        //        GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(false);
        //        GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(true);
        //        stageStatePopup.transform.Find("StageStatePanel/success").gameObject.SetActive(true);
        //        imdComPopup.SetActive(false);
        //    }
        //    else
        //    {
        //        GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(true);
        //        GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(false);
        //        stageStatePopup.transform.Find("StageStatePanel/success").gameObject.SetActive(false);
        //    }

        //}
    }

    //스테이지 팝업창에서 용병 선택
    public void select(GameObject obj)
    {
        worldCam1.depth = 3;
        GameObject.Find("System").transform.Find("MercenaryInfoPopup/UIPanel/CloseButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("System").transform.Find("MercenaryInfoPopup/UIPanel/CloseButton").gameObject.GetComponent<Button>().onClick.AddListener(() => { worldCam1.depth = 5; });
        GameObject.Find("System").transform.Find("MercenaryInfoPopup/UIPanel/OKButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("System").transform.Find("MercenaryInfoPopup/UIPanel/OKButton").gameObject.GetComponent<Button>().onClick.AddListener(() => { worldCam1.depth = 5; });
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
        GameObject monsterObj = GameObject.Find("01_3D").transform.Find("Monster (1)").gameObject;
        stageStatePopup.transform.Find("StageStatePanel/success").gameObject.SetActive(false);
        if (selectMerFlag)
        {
            monsterObj.SetActive(false);
            stagePopup.SetActive(false);

            //보낸 상태로 변경
            StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect);
            //GameObject.Find("StageStateText").GetComponent<Text>().text = result.type + " " + result.typeNum.ToString();
            result.mermove = true;
            result.wait = false;
            for (int i = 0; i < result.getItem.Length; i++)
            {
                result.getItem[i] = null;
                result.getItemNum[i] = 0;
            }
            result.time = System.DateTime.Now;  //result.typeNum * 60f;
            //걸리는 시간. 거리 계산
            float dist = Vector3.Distance(curSelectObj.transform.localPosition, GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.transform.localPosition);
            int time = (int)(dist / 10);
            result.leadTime = new System.TimeSpan(0, time/60, time%60);
            result.mercenaryName = mercenaryManager.getCurSelect();
            result.monsterHP = 10000f*result.typeNum;

            stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + result.mercenaryName).gameObject.SetActive(true);
            stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == curStageSelect)] = result;

            Mercenary mer = mercenaryManager.getMercenary().Find(x => x.getName() == mercenaryManager.getCurSelect());
            mer.setStageNum(result.getStageNum());
            mer.setState(true);
            mer.active = "go";
            mer.posX = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.transform.localPosition.x;
            mer.posY = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.transform.localPosition.y;
            mercenaryManager.setMercenaryIndex(mercenaryManager.getMercenary().FindIndex(x => x.getName() == mer.getName()), mer);

            GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection").GetComponent<Button>().interactable = false;
            Color col = GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection/Image").GetComponent<Image>().color;
            GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection/Image").GetComponent<Image>().color = new Color(col.r, col.g, col.b, 0.5f);

            //Text timeText = GameObject.Find("TimeBox").transform.Find("Text").gameObject.GetComponent<Text>();
            //float time = stageInfoList.Find(x => x.getStageNum() == curStageSelect).time;
            //timeText.text = "남은 시간 : " + ((int)(time / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";


            GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(mer.getName() + " 용병이 출발했습니다.");

            GameObject.Find("stage" + result.getStageNum().ToString() + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(true);
            GameObject.Find("stage" + result.getStageNum().ToString() + "Button").transform.Find("State/Progress/Dust").gameObject.SetActive(true);

            //획득 가능 아이템 삭제
            destroyItemBox(itemListObj);

            //GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(true);
            //GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(false);
        }
        //용병 선택 안 하고 보내기 버튼 선택
        else
        {
            GameObject.Find("System").transform.Find("AlertImage").gameObject.SetActive(false);
            GameObject.Find("System").transform.Find("AlertImage/AlrImage/Text").gameObject.GetComponent<Text>().text = "용병을 선택하세요.";
            StartCoroutine(alrImageActive());
        }

    }


    //문구 출력 애니메이션
    IEnumerator alrImageActive()
    {
        GameObject.Find("System").transform.Find("AlertImage").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
    }


    public void destroyItemBox(GameObject Obj)
    {
        //오브젝트 삭제
        if (Obj.transform.childCount > 1)
        {
            for (int i = 1; i < Obj.transform.childCount; i++)
            {
                Destroy(Obj.transform.GetChild(i).gameObject);
            }
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
            if (Player.instance.getUser().gold < goldPrice)
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
            if (Player.instance.getUser().cash < cashPrice)
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

        //퀘스트 카운트
        if(result.type == QuestData.instance.getQuestList().Find(x=>x.type == "hunting").target)
            QuestData.questHunting += result.typeNum;
        else if(QuestData.instance.getQuestList().Find(x => x.type == "hunting").target == "몬스터 아무거나")
            QuestData.questHunting += result.typeNum;

        QuestData.questWeeklyHunting += result.typeNum;

        //남은 시간 계산해서 아이템 획득
        //int num = ((int)result.time / 10) + 1;
        //while (num > 0)
        //{
        //    stageData.getItem(result);
        //    num--;
        //}


        destroyItemBox(stateItemList);
        //얻은 아이템
        //num = result.getItem.Length;
        //string[] item = new string[num];

        //for (int i = 0; i < num; i++)
        //{
        //    item[i] = result.getItem[i];
        //    if (item[i] != null)
        //    {
        //        Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).grade);
        //        stateItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;
        //        stateItemBox.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).icon);
        //        stateItemBox.transform.Find("AmountText").gameObject.GetComponent<Text>().text = result.getItemNum[i].ToString();
        //        stateItemBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = item[i];

        //        GameObject boxobj = Instantiate(stateItemBox);
        //        boxobj.transform.SetParent(stateItemList.transform, false);
        //        boxobj.name = "statePopupGetItem" + item[i];
        //        boxobj.SetActive(true);
        //    }
        //}
        //result.time = 0f;
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
                //result.time = 0f;
                result.complete = false;
                for (int i = 0; i < result.getItem.Length; i++)
                {
                    result.getItem[i] = null;
                    result.getItemNum[i] = 0;
                }
                //result.getItemTimeFlag = false;
                destroyItemBox(stateItemList);

                GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection").GetComponent<Button>().interactable = true;
                Color col = GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection/Image").GetComponent<Image>().color;
                GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection/Image").GetComponent<Image>().color = new Color(col.r, col.g, col.b, 1f);
                WorldMapBackObj.transform.Find("Stage/stage" + result.getStageNum().ToString() + "Button/MercImage").gameObject.SetActive(false);

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
        GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle("사냥에 성공하여 아이템을 획득했습니다.");
        //보상
        //월드맵/스테이지에서 완료한 경우
        if (GameObject.Find("Menu").transform.Find("WorldMap").gameObject.activeInHierarchy)
        {
            Vector3 getItemListObj = GameObject.Find(result.stageName + "Button").transform.position; // 시작 위치
            GameObject stageItem = GameObject.Find("StageItemImagePos").transform.Find("StageItemImage").gameObject;
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(30);
            StartCoroutine(createGetItem(result, stageItem, "StageItemImagePos", getItemListObj));
            GameObject.Find("InventoryScript").GetComponent<GetItemManager>().getItem(result.getItem, result.getItemNum);
        }
        //로비에서 완료한 경우
        else
        {
            Vector3 getItemListObj = GameObject.Find("Mercenary" + result.mercenaryName + "Button").transform.position; // 시작 위치
            GameObject stageItem = GameObject.Find("StageItemImagePos").transform.Find("StageItemImage").gameObject;
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(30);
            StartCoroutine(createGetItem(result, stageItem, "StageItemImagePos", getItemListObj));
            GameObject.Find("InventoryScript").GetComponent<GetItemManager>().getItem(result.getItem, result.getItemNum);
        }

        destroyItemBox(stateItemList);

        result.complete = false;
        //result.getItemTimeFlag = false;
        GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection").GetComponent<Button>().interactable = true;
        Color col = GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection/Image").GetComponent<Image>().color;
        GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + result.mercenaryName + "Selection/Image").GetComponent<Image>().color = new Color(col.r, col.g, col.b, 1f);
        WorldMapBackObj.transform.Find("Stage/stage" + result.getStageNum().ToString() + "Button/MercImage").gameObject.SetActive(false);

        //용병 초기화
        Mercenary mer = mercenaryManager.getMercenary().Find(x => x.getName() == result.mercenaryName);
        mer.setStageNum(0);
        //mer.setState(false);
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
                    GameObject spottmp = SpotObj.transform.Find(stif[k].spotName).gameObject;
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
        random = Random.Range(1, 3 + 1);
        Debug.Log(random);
        result.type = stageData.typeNumToString(random);
        random = Random.Range(1, 4);
        result.typeNum = random;
        result.stageName = "stage" + result.getStageNum().ToString();
        spotButton.transform.Find("StageText").GetComponent<Text>().text = result.getStageNum().ToString();  //
        spotButton.name = result.stageName + "Button"; //오브젝트 이름 변경
                                                       //stageData.stageImageChange(result);
        result.time = System.DateTime.Now;  // typeNumToRegenTime();             //리젠 시간 설정
        //spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 0.5f);
        //spotButton.GetComponent<Image>().sprite = result.sprite;

        //3D 몬스터
        //Debug.Log(MonsterObjList[result.getStageNum()-1].gameObject);
        Destroy(MonsterObjList[result.getStageNum() - 1].gameObject);
        int num = 0;
        if (result.type == "전갈")
            num = 0;
        else if (result.type == "오쿰") num = 1;
        else if (result.type == "인큐버스") num = 2;
        MonsterObjList[result.getStageNum() - 1] = Instantiate(MonsterObj[num].gameObject);
        MonsterObjList[result.getStageNum() - 1].transform.SetParent(GameObject.Find("01_3D").transform.Find("Monster").gameObject.transform);

        spotButton.transform.Find("NameImage/NameText").GetComponent<Text>().text = result.type + " " + result.typeNum.ToString();

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

            string type = ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).type;
            //장비 구분
            if (type == "Helmet" || type == "Armor" || type == "Gloves" || type == "Pants" || type == "Weapon" || type == "Boots")
            {
                for (int j = 0; j < result.getItemNum[i]; j++)
                {
                    ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).type, result.getItem[i], 1));
                }
            }
            //장비 외 아이템
            else
            {
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == result.getItem[i]) != null)
                {
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == result.getItem[i]).possession += result.getItemNum[i];
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == result.getItem[i]).recent = true;
                }
                else
                {
                    ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).type, result.getItem[i], result.getItemNum[i]));
                }

            }
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
        FieldMonster monsterInfo = monsterdata.getMonsterList().Find(x => x.name == result.type);
        int num = monsterInfo.itemName.Length;
        string[] item = new string[num];

        for (int i = 0; i < num; i++)
        {
            item[i] = monsterInfo.itemName[i];
            if (item[i] != null)
            {
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).grade);
                ItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;

                ItemBox.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).icon);
                ItemBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = item[i];

                GameObject obj = Instantiate(ItemBox);
                obj.transform.SetParent(itemListObj.transform, false);
                obj.SetActive(true);
            }
        }

        itemListObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0 - itemListObj.GetComponent<RectTransform>().sizeDelta.y / 2);
    }

    //이미지 변경
    public void ItemImageChange(string stageInfoListtmp, Image spr)
    {
        spr.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == stageInfoListtmp).icon);
    }


    /// <summary>
    /// 약탈
    /// </summary>

    //약탈할 플레이어 클릭
    public void initPlunderButton(GameObject obj)
    {
        curPlunderSelect = obj.transform.Find("StageText").GetComponent<Text>().text;

        PlunderInfo result = plunderInfoList.Find(x => x.opponentName == curPlunderSelect);
        Plunder plunder = StageData.instance.getPlunderList().Find(x => x.getName() == curPlunderSelect);
        //리젠 상태
        if (result.regen)
        {
            return;
        }
        //팝업
        else
        {
            //플레이어 정보 갱신
            plunderPlayerBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = Player.instance.getUser().Name;
            plunderPlayerBox.transform.Find("TextGroup/LevelText").gameObject.GetComponent<Text>().text = Player.instance.getUser().level.ToString();

            plunderPlayerBox.transform.Find("TextGroup/DpsText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)statData.getRepreSetStat().dps));
            plunderPlayerBox.transform.Find("TextGroup/StrPowerText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)statData.getRepreSetStat().strPower));
            plunderPlayerBox.transform.Find("TextGroup/AttackSpeedText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", statData.getRepreSetStat().attackSpeed.ToString("N1"));
            plunderPlayerBox.transform.Find("TextGroup/FocusText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)statData.getRepreSetStat().focus));
            plunderPlayerBox.transform.Find("TextGroup/CriticalText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)statData.getRepreSetStat().critical));
            plunderPlayerBox.transform.Find("TextGroup/DefPowerText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)statData.getRepreSetStat().defPower));
            plunderPlayerBox.transform.Find("TextGroup/EvaRateText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)statData.getRepreSetStat().evaRate));

            //상대방 데이터 가져와서 갱신
            plunderEnemyBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = plunder.getName();
            plunderEnemyBox.transform.Find("TextGroup/LevelText").gameObject.GetComponent<Text>().text = plunder.level.ToString();

            plunderEnemyBox.transform.Find("TextGroup/DpsText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)plunder.stat.dps));
            plunderEnemyBox.transform.Find("TextGroup/StrPowerText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)plunder.stat.strPower));
            plunderEnemyBox.transform.Find("TextGroup/AttackSpeedText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", plunder.stat.attackSpeed.ToString("N1"));
            plunderEnemyBox.transform.Find("TextGroup/FocusText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)plunder.stat.focus));
            plunderEnemyBox.transform.Find("TextGroup/CriticalText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)plunder.stat.critical));
            plunderEnemyBox.transform.Find("TextGroup/DefPowerText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)plunder.stat.defPower));
            plunderEnemyBox.transform.Find("TextGroup/EvaRateText").gameObject.GetComponent<Text>().text = string.Format("{0:#,###}", ((int)plunder.stat.evaRate));

            plunderPopup.transform.Find("UIPanel/ItemBox/ItemTitleText").gameObject.GetComponent<Text>().text = "획득 가능한 아이템";
            plunderPopup.transform.Find("UIPanel/PlunderButton").gameObject.SetActive(true);
            plunderPopup.transform.Find("UIPanel/OKButton").gameObject.SetActive(false);
            plunderPlayerBox.transform.Find("Win").gameObject.SetActive(false);
            plunderPlayerBox.transform.Find("Lose").gameObject.SetActive(false);
            plunderEnemyBox.transform.Find("Win").gameObject.SetActive(false);
            plunderEnemyBox.transform.Find("Lose").gameObject.SetActive(false);
            plunderPopup.transform.Find("FrontBox").gameObject.SetActive(false);
            plunderPopup.SetActive(true);


            destroyItemBox(plunderItemListObj);

            //획득 가능한 아이템 리스트
            int num = plunder.getItem.Length;
            string[] item = new string[num];

            for (int i = 0; i < num; i++)
            {
                item[i] = plunder.getItem[i];
                if (item[i] != null)
                {
                    Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).grade);
                    plunderItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;
                    plunderItemBox.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).icon);
                    plunderItemBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = item[i];

                    GameObject boxobj = Instantiate(plunderItemBox);
                    boxobj.transform.SetParent(plunderItemListObj.transform, false);
                    boxobj.SetActive(true);
                }
            }




            //약탈 버튼
            plunderPopup.transform.Find("UIPanel/PlunderButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            plunderPopup.transform.Find("UIPanel/PlunderButton").gameObject.GetComponent<Button>().onClick.AddListener(() => PlunderButton(plunder, result));

        }
    }


    //약탈 시작 버튼
    public void PlunderButton(Plunder plunder, PlunderInfo result)
    {
        Debug.Log("약탈 시작");
        //리젠
        result.regen = true;
        StageData.spotList[StageData.spotList.FindIndex(x => x.plunderNum == result.getPlunderNum())].plunderActive = false;
        int random = 0;
        int index = 0;
        result.spotName = null;

        List<Spot> sList = StageData.spotList;
        //위치 변경
        while (true)
        {
            random = Random.Range(1, sList.Count + 1);
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
            if (StageData.spotList[index].plunderActive == false) Debug.Log("배치 완료"); break;
        }
        StageData.spotList[index].plunderActive = true;

        //기존 상대 데이터의 스팟 할당 해제
        plunder.assignment = false;
        //랜덤으로 리스트에 ai 정보 넣기
        PlunderInfo plInfo = plunderInfoList.Find(x => x.opponentName == plunder.getName());    //해당 스팟
        while (true)
        {
            random = Random.Range(0, 40);
            Plunder plunderTmp = StageData.instance.getPlunderList()[random];

            if (plunderTmp.assignment == false)
            {
                plInfo.opponentName = plunderTmp.getName();
                plunderTmp.assignment = true;
                break;
            }
            else continue;
        }


        result.time = 1f;

        GameObject spotButton = WorldMapBackObj.transform.Find("Plunder/" + result.PlunderName + "Button").gameObject;
        spotButton.transform.localPosition = StageData.spotList[index].getPosition().localPosition;

        StageData.spotList[index].plunderNum = result.getPlunderNum();    //스테이지 번호 저장
        StageData.spotList[index].plunderActive = true;    //스팟 활성화
        result.spotName = StageData.spotList[index].getPosition().name;
        result.PlunderName = "plunder" + result.getPlunderNum().ToString();
        spotButton.transform.Find("StageText").GetComponent<Text>().text = result.opponentName;
        spotButton.transform.Find("State/NameImage/NameText").GetComponent<Text>().text = result.opponentName;
        spotButton.name = result.PlunderName + "Button"; //오브젝트 이름 변경

        spotButton.GetComponent<Image>().color = new Color(spotButton.GetComponent<Image>().color.r, spotButton.GetComponent<Image>().color.g, spotButton.GetComponent<Image>().color.b, 0.5f);
        //spotButton.GetComponent<Image>().sprite = result.sprite;

        QuestData.questPlunder += 1;

        //계산
        bool dpsResult = dpsCal(plunder);

        //결과
        //획득 가능한 아이템->획득한 아이템
        plunderPopup.transform.Find("UIPanel/ItemBox/ItemTitleText").gameObject.GetComponent<Text>().text = "획득한 아이템";


        //약탈 버튼->확인 버튼
        plunderPopup.transform.Find("UIPanel/PlunderButton").gameObject.SetActive(false);
        plunderPopup.transform.Find("UIPanel/OKButton").gameObject.SetActive(true);
        //승리 패배 표시 애니메이션 
        //승리
        if (dpsResult)
        {
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(40);
            plunderPopup.transform.Find("FrontBox").gameObject.SetActive(true);

            //아이템 자동 획득
            for (int i = 0; i < plunder.getItem.Length; i++)
            {
                if (plunder.getItem[i] != null)
                {
                    int rand = Random.Range(0, 100);
                    if (rand < plunder.getItemWinProb[i])
                    {
                        result.getItem[i] = plunder.getItem[i];
                        result.getItemNum[i] = 1;
                    }
                }
            }


            ClearFail.SetActive(true);
            StartCoroutine(closeWin());
        }
        //패배
        else
        {
            GameObject.Find("PlayerData").GetComponent<Player>().getExp(10);
            plunderPopup.transform.Find("FrontBox").gameObject.SetActive(true);

            //아이템 자동 획득
            for (int i = 0; i < plunder.getItem.Length; i++)
            {
                if (plunder.getItem[i] != null)
                {
                    int rand = Random.Range(0, 100);
                    if (rand < plunder.getItemLoseProb[i])
                    {
                        result.getItem[i] = plunder.getItem[i];
                        result.getItemNum[i] = 1;
                    }
                }
            }
            Fail.SetActive(true);
            //애니
            StartCoroutine(closeLose());
        }

        //획득한 아이템 표시
        destroyItemBox(plunderItemListObj);

        int num = result.getItem.Length;
        string[] item = new string[num];

        for (int i = 0; i < num; i++)
        {
            item[i] = result.getItem[i];
            if (item[i] != null)
            {
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).grade);
                plunderItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;
                plunderItemBox.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).icon);
                plunderItemBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = item[i];

                GameObject boxobj = Instantiate(plunderItemBox);
                boxobj.transform.SetParent(plunderItemListObj.transform, false);
                boxobj.SetActive(true);
            }
        }

        //인벤토리 저장
        for (int i = 0; i < result.getItem.Length; i++)
        {
            if (result.getItem[i] != null)
            {
                string type = ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).type;
                //장비 구분
                if (type == "Helmet" || type == "Armor" || type == "Gloves" || type == "Pants" || type == "Weapon" || type == "Boots")
                {
                    for (int j = 0; j < result.getItemNum[i]; j++)
                    {
                        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == result.getItem[i]).type, result.getItem[i], 1));
                        //ThingsData.instance.getInventoryThingsList().Find(x => x.name == result.getItem[i]).recent = true;
                    }
                }
                //장비 외 아이템
                else
                {
                    if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == result.getItem[i]) != null)
                    {
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == result.getItem[i]).possession
                            += result.getItemNum[i];
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == result.getItem[i]).recent = true;
                    }
                    else
                    {
                        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                            x => x.name == result.getItem[i]).type, result.getItem[i], result.getItemNum[i]));
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == result.getItem[i]).recent = true;
                    }
                }
                //초기화
                result.getItem[i] = null;
                result.getItemNum[i] = 0;
            }
        }
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
        Fail.SetActive(false);
        plunderPlayerBox.transform.Find("Win").gameObject.SetActive(false);
        plunderPlayerBox.transform.Find("Lose").gameObject.SetActive(true);
        plunderEnemyBox.transform.Find("Win").gameObject.SetActive(true);
        plunderEnemyBox.transform.Find("Lose").gameObject.SetActive(false);
        plunderPopup.transform.Find("FrontBox").gameObject.SetActive(false);
    }



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

            StageData.instance.getPlunderList().Find(x => x.getName() == plunderInfoList[i].opponentName).assignment = false;
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
                        if (plif[k].opponentName == StageData.instance.getPlunderList()[random].getName())
                        {
                            flag = true;
                        }
                    }
                    if (flag) continue; else break;
                }
                else break;
            }
            plunderInfoList[i].opponentName = StageData.instance.getPlunderList()[random].getName();
            StageData.instance.getPlunderList()[random].assignment = true;
            GameObject obj = GameObject.Find("plunder" + (i + 1).ToString() + "Button");
            obj.transform.Find("StageText").gameObject.GetComponent<Text>().text = StageData.instance.getPlunderList()[random].getName();
            obj.transform.Find("State/NameImage/NameText").gameObject.GetComponent<Text>().text = "Lv" + 
                StageData.instance.getPlunderList()[random].level + " " + StageData.instance.getPlunderList()[random].getName();
            //obj.transform.Find("State/LevelText").gameObject.GetComponent<Text>().text = StageData.instance.getPlunderList()[random].level.ToString();
        }

    }


    //UI에 맞게 위치 고정
    void SetPositionHUD()
    {
        List<StageInfo> sList = stageInfoList;
        for (int i = 0; i < sList.Count; i++)
        {
            if (sList[i].spotName != null && MonsterObjList[i] !=null)
            {
                string spotName = sList[i].spotName;
                //스팟 인덱스
                int index = StageData.spotList.FindIndex(x => x.getSpotName() == spotName);

                Vector3 position = uiCam.ViewportToWorldPoint(StageData.spotList[index].getPosition().position);
                MonsterObjList[i].transform.position = worldCam.WorldToViewportPoint(position);

                //값 정리. 
                position = MonsterObjList[i].transform.localPosition;
                //position.x = Mathf.RoundToInt(position.x);
                //position.y = Mathf.RoundToInt(position.y);
                //position.y = position.y-2.0f;

                position.z = 20.0f;
                MonsterObjList[i].transform.localPosition = position;
                MonsterObjList[i].SetActive(true);
            }
        }
        light.transform.position = worldCam.transform.position;
    }

    //UI에 맞게 위치 고정
    void SetPositionHUDPopup()
    {
        StageInfo result = stageInfoList.Find(x => x.getStageNum() == curStageSelect);
        GameObject monsterObj = GameObject.Find("01_3D").transform.Find("Monster (1)").gameObject;
        Button close = stagePopup.transform.Find("UIPanel/CloseButton").gameObject.GetComponent<Button>();
        close.onClick.RemoveAllListeners();
        if (result.type == "전갈")
        {
            if (result.typeNum == 1)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position1").gameObject.transform.position);
                monsterObj.transform.GetChild(0).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(0).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(0).gameObject.transform.localPosition = position;

                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
            if (result.typeNum == 2)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position2").gameObject.transform.position);
                monsterObj.transform.GetChild(0).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(0).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(0).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(0).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position3").gameObject.transform.position);
                monsterObj.transform.GetChild(1).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(1).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(1).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(1).gameObject.SetActive(true);
                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
            if (result.typeNum == 3)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position1").gameObject.transform.position);
                monsterObj.transform.GetChild(0).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(0).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(0).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(0).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position2").gameObject.transform.position);
                monsterObj.transform.GetChild(1).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(1).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(1).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(1).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position3").gameObject.transform.position);
                monsterObj.transform.GetChild(2).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(2).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(2).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(2).gameObject.SetActive(true);
                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
        }
        if (result.type == "오쿰")
        {
            if (result.typeNum == 1)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position1").gameObject.transform.position);
                monsterObj.transform.GetChild(4).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(4).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(4).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(4).gameObject.SetActive(true);
                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
            if (result.typeNum == 2)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position2").gameObject.transform.position);
                monsterObj.transform.GetChild(4).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(4).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(4).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(4).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position3").gameObject.transform.position);
                monsterObj.transform.GetChild(5).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(5).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(5).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(5).gameObject.SetActive(true);
                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
            if (result.typeNum == 3)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position1").gameObject.transform.position);
                monsterObj.transform.GetChild(4).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(4).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(4).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(4).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position2").gameObject.transform.position);
                monsterObj.transform.GetChild(5).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(5).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(5).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(5).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position3").gameObject.transform.position);
                monsterObj.transform.GetChild(3).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(3).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(3).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(3).gameObject.SetActive(true);
                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
        }
        if (result.type == "인큐버스")
        {
            if (result.typeNum == 1)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position1").gameObject.transform.position);
                monsterObj.transform.GetChild(7).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(7).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(7).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(7).gameObject.SetActive(true);
                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
            if (result.typeNum == 2)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position2").gameObject.transform.position);
                monsterObj.transform.GetChild(7).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(7).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(7).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(7).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position3").gameObject.transform.position);
                monsterObj.transform.GetChild(8).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(8).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(8).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(8).gameObject.SetActive(true);
                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
            if (result.typeNum == 3)
            {
                Vector3 position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position1").gameObject.transform.position);
                monsterObj.transform.GetChild(7).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(7).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(7).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(7).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position2").gameObject.transform.position);
                monsterObj.transform.GetChild(8).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(8).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(8).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(8).gameObject.SetActive(true);

                position = uiCam1.ViewportToWorldPoint(stagePopup.transform.Find("UIPanel/MonsterBox/Position3").gameObject.transform.position);
                monsterObj.transform.GetChild(6).gameObject.transform.position = worldCam1.WorldToViewportPoint(position);
                position = monsterObj.transform.GetChild(6).gameObject.transform.localPosition;
                position.z = 20.0f;
                monsterObj.transform.GetChild(6).gameObject.transform.localPosition = position;
                monsterObj.transform.GetChild(6).gameObject.SetActive(true);
                close.onClick.AddListener(() =>
                {
                    monsterObj.SetActive(false);
                });
            }
        }
    }

    //return
    public void returnButton()
    {
        GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back").gameObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    //모드 전환
    public void modeChange(bool b)
    {
        mode = b;
    }

    //typeNum string 변환
    public string typeNumToString(int i) { if (i == 1) return "소"; else if (i == 2) return "중"; else if (i == 3) return "대"; else return null; }
    //typeNum time 변환
    //public float typeNumToTime(int i) { if (i == 1) return 300f; else if (i == 2) return 900f; else if (i == 3) return 1800f; else return 0; }
    //typeNum regen time 30m~1h
    public float typeNumToRegenTime() { int random = Random.Range(1800, 3600 + 1); return random; }

    public List<StageInfo> getStageInfoList() { return stageInfoList; }
    public void setStageInfoList(List<StageInfo> list) { stageInfoList = list; }
    public List<PlunderInfo> getPlunderInfoList() { return plunderInfoList; }
    public void setPlunderInfoList(List<PlunderInfo> list) { plunderInfoList = list; }

    public List<GameObject> getMonsterObjList() { return MonsterObjList; }

    public int getCurStageSelect() { return curStageSelect; }
    public void SetCurStageSelect(int cur) { curStageSelect = cur; }
}




