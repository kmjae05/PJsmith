using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryManager : MonoBehaviour
{

    private GameObject uiPanel;
    private GameObject buildInfoPopup;
    private GameObject BeUnderPopup;
    private GameObject MiningPopup;

    private GameObject BlackBack;
    private Button CancleButton;
    private GameObject BottomMenuLock;
    private GameObject StartLock;

    private GameObject SystemPopup;
    private Button sys_yesButton;
    private Button sys_NoButton;
    private Button sys_OkButton;


    private List<GameObject> mineObj;           //광산 스팟
    private List<GameObject> bottonButtonList;  //하단 버튼

    private List<MineInfo> mineInfo;

    private string curType = null;          //현재 선택된 광산 종류
    private int curMineNum = 0;             //현재 선택한 광산 번호
    private int level = 1;              //건설할 때 레벨


    void Start()
    {
        uiPanel = GameObject.Find("Menu").transform.Find("TerritoryPopup/UIPanel").gameObject;
        buildInfoPopup = GameObject.Find("System").transform.Find("BuildInfoPopup").gameObject;
        BeUnderPopup = GameObject.Find("System").transform.Find("BeUnderPopup").gameObject;
        MiningPopup = GameObject.Find("System").transform.Find("MiningPopup").gameObject;
        BlackBack = uiPanel.transform.Find("BlackBack").gameObject;
        CancleButton = BlackBack.transform.Find("CancleButton").gameObject.GetComponent<Button>();
        BottomMenuLock = uiPanel.transform.Find("BottomMenu/BottomMenuLock").gameObject;
        StartLock = GameObject.Find("MenuButton (1)").transform.Find("LockImage").gameObject;
        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        sys_yesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        sys_NoButton = SystemPopup.transform.Find("UIPanel/NoButton").gameObject.GetComponent<Button>();
        sys_OkButton = SystemPopup.transform.Find("UIPanel/OKButton").gameObject.GetComponent<Button>();


        mineObj = new List<GameObject>();
        bottonButtonList = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            mineObj.Add(uiPanel.transform.Find("Mine/Spot" + (i + 1).ToString()).gameObject);
            mineObj[i].SetActive(true);
        }
        for (int i = 0; i < 6; i++)
        {
            bottonButtonList.Add(uiPanel.transform.Find("BottomMenu/BottomButton" + (i + 1).ToString()).gameObject);
        }

        mineInfo = MineData.instance.getMineInfoList();

        StateUpdate();
        for (int i = 0; i < 6; i++)
        {
            int index = i;
            bottonButtonList[i].transform.Find("OreImage").gameObject.GetComponent<Button>().onClick.AddListener(() => BottomButtonSetting(index));
            if (i>=2 && Player.Play.level >= MineData.instance.getMineInfoList()[i].buildLevel)
                bottonButtonList[i].transform.Find("LockImage").gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 2; i < 6; i++)
            if (Player.Play.level >= MineData.instance.getMineInfoList()[i].buildLevel)
                bottonButtonList[i].transform.Find("LockImage").gameObject.SetActive(false);
        for (int i = 0; i < MineData.instance.getMineList().Count; i++)
        {
            //건설 중
            if (MineData.instance.getMineList()[i].buildState == "beunder")
            {
                //시간 0이면 건설완료 상태로
                if (MineData.instance.getMineList()[i].buildTime < 0.5f)
                {
                    //mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text = "건설 완료";

                    BottomMenuLock.SetActive(false);
                    StartLock.SetActive(false);
                    if (BeUnderPopup.activeInHierarchy) BeUnderPopup.SetActive(false);
                }
                //시간 빼기
                else
                {
                    float time = MineData.instance.getMineList()[i].buildTime;
                    if (time > 3600)
                    {
                        mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)(time / 3600)).ToString() + "시간 " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)(time / 3600)).ToString() + "시간 " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                    }
                    else if (time > 60)
                    {
                        mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                    }
                    else
                    {
                        mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)(time % 60)).ToString() + "초";
                    }
                }
            }
            //건설 완료 & 채굴 상태
            if (MineData.instance.getMineList()[i].buildState == "complete" && MineData.instance.getMineList()[i].miningState)
            {
                Color clr = mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color;
                mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
                mineObj[i].transform.Find("Image").gameObject.SetActive(true);
                mineObj[i].transform.Find("Text").gameObject.SetActive(true);
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[i].transform.Find("pickax").gameObject.SetActive(true);

                mineObj[i].GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                mineObj[i].GetComponent<Button>().onClick.AddListener(() => Mining(mineObj[num], num));

                //획득
                mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text = MineData.instance.getMineList()[i].getAmount.ToString() + " / " + MineData.instance.getMineList()[i].deposit.ToString();

                if (MiningPopup.activeInHierarchy)
                {
                    MiningPopup.transform.Find("UIPanel/InfoBox/AmountText").gameObject.GetComponent<Text>().text = "채굴량 : " + MineData.instance.getMineList()[curMineNum].getAmount.ToString() + " / " + MineData.instance.getMineList()[curMineNum].deposit.ToString();
                    MiningPopup.transform.Find("UIPanel/InfoBox/MiningTimeText").gameObject.GetComponent<Text>().text = "채굴 주기 : " + MineData.instance.getMineList()[curMineNum].miningTime.ToString();
                }

            }
            //매장량 다 채우면 pickax없애고 텍스트 바꾸기
            if (MineData.instance.getMineList()[i].buildState == "complete" && !MineData.instance.getMineList()[i].miningState)
            {
                mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text = MineData.instance.getMineList()[i].deposit.ToString() + "개 채굴 완료";
                mineObj[i].transform.Find("pickax").gameObject.SetActive(false);
                if(curMineNum == i) { MiningPopup.SetActive(false); }

                mineObj[i].GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                mineObj[i].GetComponent<Button>().onClick.AddListener(() => getOre(mineObj[num], num));
            }
        }
    }
    
    //버튼 세팅
    private void BottomButtonSetting(int index)
    {
        buildInfoPopup.SetActive(true);
        buildInfoPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = mineInfo[index].type + " 광산 건설";
        buildInfoPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = "레벨 : " + level.ToString();
        buildInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = "소요 시간 : " + mineInfo[index].buildTime + "초";
        buildInfoPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text = "매장량 : 100";
        string mtr = null;
        for(int i=0; i < mineInfo[index].necessaryMaterials.Length; i++)
        {
            if(mineInfo[index].necessaryMaterials.Length == 1)
            {
                mtr = mineInfo[index].necessaryMaterials[0] + " " + mineInfo[index].necessaryMaterialsNum[0];
                break;
            }
            else
            {
                if (i == 0) { mtr = mineInfo[index].necessaryMaterials[0] + " " + mineInfo[index].necessaryMaterialsNum[0]; }
                else { mtr += ", " + mineInfo[index].necessaryMaterials[i] + " " + mineInfo[index].necessaryMaterialsNum[i]; }
            }
        }
        buildInfoPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text = "필요 재료 : " + mtr;
        curType = mineInfo[index].type;
    }

    //건설 버튼
    public void BuildButton()
    {
        MineInfo info = mineInfo.Find(x => x.type == curType);
        //재료 체크
        for(int i = 0; i < info.necessaryMaterials.Length; i++)
        {
            //재료랑 갖고 있는 아이템 체크
            Things thing = ThingsData.instance.getThingsList().Find(x => x.name == info.necessaryMaterials[i]);
            if (thing != null)
            {
                if (thing.possession >= info.necessaryMaterialsNum[i])
                {
                    //건설 조건 만족
                    Debug.Log("y");
                }
                else
                {
                    //재료 수량 부족
                    Debug.Log("no");
                    return;
                }
            }
            //아이템 못 찾음
            else
            {
                Debug.Log("null");
                return;
            }
            
        }
        //재료 체크 완료//

        buildInfoPopup.SetActive(false);
        BlackBack.SetActive(true);
        //건설 취소 버튼
        CancleButton.onClick.AddListener(() => { BlackBack.SetActive(false);
            for (int j = 0; j < MineData.instance.getMineList().Count; j++)
            {
                if (MineData.instance.getMineList()[j].buildState == "nothing")
                {
                    mineObj[j].transform.Find("Image").gameObject.SetActive(false);
                    mineObj[j].transform.Find("Text").gameObject.SetActive(false);
                    mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(false);
                    mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
                }
            }
        });

        //빈 스팟 띄우기
        for (int j = 0; j < MineData.instance.getMineList().Count; j++)
        {
            if (MineData.instance.getMineList()[j].buildState == "nothing")
            {
                mineObj[j].GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject obj = mineObj[j];
                int num = j;
                mineObj[j].GetComponent<Button>().onClick.AddListener(() => BuildSpotClick(obj, num));
                mineObj[j].transform.Find("Image").gameObject.SetActive(false);
                mineObj[j].transform.Find("Text").gameObject.SetActive(false);
                mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(true);
                mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
            }
        }


    }

    //건설 스팟을 선택했을 때 동작
    public void BuildSpotClick(GameObject obj, int num)
    {
        MineInfo info = mineInfo.Find(x => x.type == curType);

        //광산에 정보 저장
        MineData.instance.getMineList()[num].type = info.type;
        MineData.instance.getMineList()[num].level = 1;
        MineData.instance.getMineList()[num].buildState = "beunder";
        MineData.instance.getMineList()[num].buildTime = info.buildTime;
        MineData.instance.getMineList()[num].getThingName = info.getThingName;
        MineData.instance.getMineList()[num].getOnceAmount = MineData.instance.getMineList()[num].level * 2;
        MineData.instance.getMineList()[num].deposit = MineData.instance.getMineList()[num].level * 30;
        MineData.instance.getMineList()[num].miningTime = 1f;

        //재료 소모
        for (int i = 0; i < info.necessaryMaterials.Length; i++)
        {
            Things thing = ThingsData.instance.getThingsList().Find(x => x.name == info.necessaryMaterials[i]);
            thing.possession -= info.necessaryMaterialsNum[i];
        }

        obj.GetComponent<Button>().onClick.RemoveAllListeners();
        obj.GetComponent<Button>().onClick.AddListener(() => BuildCondition(obj, num));

        Color clr = obj.transform.Find("Image").gameObject.GetComponent<Image>().color;
        obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 0.5f);
        obj.transform.Find("Image").gameObject.SetActive(true);
        obj.transform.Find("Text").gameObject.SetActive(true);
        obj.transform.Find("DottedCircle").gameObject.SetActive(false);
        obj.transform.Find("pickax").gameObject.SetActive(false);
        obj.transform.Find("TypeName").gameObject.SetActive(true);
        obj.transform.Find("TypeName/TypeNameText").gameObject.GetComponent<Text>().text = info.type + " 광산";


        //나머지 nothing없애고 건설선택 블랙백 없애기
        for (int j = 0; j < MineData.instance.getMineList().Count; j++)
        {
            if (MineData.instance.getMineList()[j].buildState == "nothing")
            {
                mineObj[j].transform.Find("Image").gameObject.SetActive(false);
                mineObj[j].transform.Find("Text").gameObject.SetActive(false);
                mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
            }
        }
        BlackBack.SetActive(false);
        //하단메뉴 잠금
        BottomMenuLock.SetActive(true);
        //대장장이 행동 제한
        StartLock.SetActive(true);

    }

    //건설 진행 중 버튼 (현황 창
    public void BuildCondition(GameObject obj, int num)
    {
        string type = MineData.instance.getMineList()[num].type;
        BeUnderPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = type + " 광산 건설 중";
        BeUnderPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = "레벨 : " + MineData.instance.getMineList()[num].level.ToString();
        BeUnderPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text = "매장량 : " + MineData.instance.getMineList()[num].deposit.ToString();
        //팝업 올리기
        BeUnderPopup.SetActive(true);

        //즉시 완료 버튼
        BeUnderPopup.transform.Find("UIPanel/ImdBuildButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        BeUnderPopup.transform.Find("UIPanel/ImdBuildButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "즉시 완료";
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "보석 5개를 사용하여 즉시 완료하시겠습니까?";
            sys_yesButton.gameObject.SetActive(true);
            sys_NoButton.gameObject.SetActive(true);
            sys_OkButton.gameObject.SetActive(false);

            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                
                if (Player.Play.cash < 20)
                {
                    SystemPopup.SetActive(true);
                    SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "보석이 부족합니다.";
                    SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "보석 구매 페이지로 이동하시겠습니까?";
                    sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
                    sys_yesButton.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        GameObject.Find("System").transform.Find("Shop").gameObject.SetActive(true);
                        BeUnderPopup.SetActive(false);
                    });
                    
                    return;
                }

                GameObject.Find("PlayerData").GetComponent<Player>().LostMoney("cash", 5);

                MineData.instance.getMineList()[num].buildTime = 0f;
                MineData.instance.getMineList()[num].buildState = "complete";
                MineData.instance.getMineList()[num].miningState = true;
                obj.transform.Find("Text").gameObject.GetComponent<Text>().text = "건설 완료";
                Color clr = mineObj[num].transform.Find("Image").gameObject.GetComponent<Image>().color;
                mineObj[num].transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
                mineObj[num].transform.Find("Image").gameObject.SetActive(true);
                mineObj[num].transform.Find("Text").gameObject.SetActive(true);
                mineObj[num].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[num].transform.Find("pickax").gameObject.SetActive(true);

                BottomMenuLock.SetActive(false);
                StartLock.SetActive(false);
                if (BeUnderPopup.activeInHierarchy) BeUnderPopup.SetActive(false);
                mineObj[num].GetComponent<Button>().onClick.RemoveAllListeners();
            });

        });


        //건설 취소 버튼->systempopup
        BeUnderPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        BeUnderPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "건설 취소";
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "광산 건설을 취소하시겠습니까?";
            sys_yesButton.gameObject.SetActive(true);
            sys_NoButton.gameObject.SetActive(true);
            sys_OkButton.gameObject.SetActive(false);
            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                SystemPopup.SetActive(false);
                MineData.instance.getMineList()[num].buildTime = 0f;
                MineData.instance.getMineList()[num].buildState = "nothing";
                obj.transform.Find("Image").gameObject.SetActive(false);
                obj.transform.Find("Text").gameObject.SetActive(false);
                obj.transform.Find("TypeName").gameObject.SetActive(false);
                BottomMenuLock.SetActive(false);
                StartLock.SetActive(false);
                if (BeUnderPopup.activeInHierarchy) BeUnderPopup.SetActive(false);
            });

        });


    }

    //채굴 중
    public void Mining(GameObject obj, int num)
    {
        MiningPopup.SetActive(true);
        MiningPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = MineData.instance.getMineList()[num].type + " 광산 채굴 중";
        MiningPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = "레벨 : " + MineData.instance.getMineList()[num].level.ToString();

        //즉시 완료
        MiningPopup.transform.Find("UIPanel/ImdButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        MiningPopup.transform.Find("UIPanel/ImdButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "즉시 완료";
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "보석 1개를 사용하여 즉시 완료하시겠습니까?";
            sys_yesButton.gameObject.SetActive(true);
            sys_NoButton.gameObject.SetActive(true);
            sys_OkButton.gameObject.SetActive(false);
            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                
                if (Player.Play.cash < 5)
                {
                    SystemPopup.SetActive(true);
                    SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "보석이 부족합니다.";
                    SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "보석 구매 페이지로 이동하시겠습니까?";
                    sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
                    sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                        GameObject.Find("System").transform.Find("Shop").gameObject.SetActive(true);
                        MiningPopup.SetActive(false);
                    });
                    
                    return;
                }
                MiningPopup.SetActive(false);
                GameObject.Find("PlayerData").GetComponent<Player>().LostMoney("cash", 1);

                MineData.instance.getMineList()[num].miningState = false;
                obj.transform.Find("Text").gameObject.GetComponent<Text>().text = MineData.instance.getMineList()[num].deposit.ToString() + "개 채굴 완료";
                obj.transform.Find("pickax").gameObject.SetActive(false);
                obj.transform.Find("BoostIcon").gameObject.SetActive(false);
            });

        });

        //부스트
        MiningPopup.transform.Find("UIPanel/BoostButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        MiningPopup.transform.Find("UIPanel/BoostButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "부스트 아이템 사용";
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "부스트 아이템을 사용하시겠습니까?" 
                + "(남은 개수 : " + ThingsData.instance.getThingsList().Find(x=>x.name == "부스트").possession.ToString() + ")";
            sys_yesButton.gameObject.SetActive(true);
            sys_NoButton.gameObject.SetActive(true);
            sys_OkButton.gameObject.SetActive(false);
            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                if (ThingsData.instance.getThingsList().Find(x => x.name == "부스트").possession <= 0)
                {
                    SystemPopup.SetActive(true);

                    SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "부스트 아이템이 부족합니다.";
                    SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "부스트 아이템이 부족합니다.";
                    sys_yesButton.gameObject.SetActive(false);
                    sys_NoButton.gameObject.SetActive(false);
                    sys_OkButton.gameObject.SetActive(true);
                    sys_OkButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
                    sys_OkButton.GetComponent<Button>().onClick.AddListener(() => { SystemPopup.SetActive(false); MiningPopup.SetActive(false); });
                    return;
                }
                MiningPopup.SetActive(false);
                ThingsData.instance.getThingsList().Find(x => x.name == "부스트").possession -= 1;
                //주기를 1/2로
                MineData.instance.getMineList()[num].miningTime /= 2;
                MineData.instance.getMineList()[num].boostState = true;

                obj.transform.Find("BoostIcon").gameObject.SetActive(true);
            });

        });






        //폐광
        MiningPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        MiningPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "광산 폐광";
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "채굴을 중지하고 폐광하시겠습니까?";
            sys_yesButton.gameObject.SetActive(true);
            sys_NoButton.gameObject.SetActive(true);
            sys_OkButton.gameObject.SetActive(false);
            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                SystemPopup.SetActive(false);
                MiningPopup.SetActive(false);

                MineData.instance.getMineList()[num].buildTime = 0f;
                MineData.instance.getMineList()[num].buildState = "nothing";
                MineData.instance.getMineList()[num].miningState = false;
                MineData.instance.getMineList()[num].boostState = false;
                obj.transform.Find("Image").gameObject.SetActive(false);
                obj.transform.Find("Text").gameObject.SetActive(false);
                obj.transform.Find("pickax").gameObject.SetActive(false);
                obj.transform.Find("TypeName").gameObject.SetActive(false);
                obj.transform.Find("BoostIcon").gameObject.SetActive(false);
            });

        });



    }

    //광석 획득
    public void getOre(GameObject obj, int num)
    {
        Debug.Log("완료");

        GameObject.Find("PlayerData").GetComponent<Player>().getExp(10);

        MineData.instance.getMineList()[num].buildState = "nothing";
        MineData.instance.getMineList()[num].getAmount = 0;
        MineData.instance.getMineList()[num].curTime = 0f;
        MineData.instance.getMineList()[num].boostState = false;

        obj.transform.Find("Image").gameObject.SetActive(false);
        obj.transform.Find("Text").gameObject.SetActive(false);
        obj.transform.Find("DottedCircle").gameObject.SetActive(false);
        obj.transform.Find("pickax").gameObject.SetActive(false);
        obj.transform.Find("TypeName").gameObject.SetActive(false);
        obj.transform.Find("BoostIcon").gameObject.SetActive(false);

        ThingsData.instance.getThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName).possession
            += MineData.instance.getMineList()[num].deposit;
        ThingsData.instance.getThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName).recent = true;

        //로그
        GameObject ItemLogInst = Instantiate(GameObject.Find("GetItemLog").transform.Find("range/GetItemLogText").gameObject);
        ItemLogInst.transform.SetParent(GameObject.Find("GetItemLog").transform.Find("range").gameObject.transform, false);
        ItemLogInst.GetComponent<Text>().text = MineData.instance.getMineList()[num].getThingName + " " + 
            MineData.instance.getMineList()[num].deposit + "개 획득";
        ItemLogInst.SetActive(true);



    }



    //광산 상태 갱신
    private void StateUpdate()
    {
        for (int i = 0; i < MineData.instance.getMineList().Count; i++)
        {
            //건설 상태
            //미건설
            if (MineData.instance.getMineList()[i].buildState == "nothing")
            {
                mineObj[i].transform.Find("Image").gameObject.SetActive(false);
                mineObj[i].transform.Find("Text").gameObject.SetActive(false);
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[i].transform.Find("pickax").gameObject.SetActive(false);
                mineObj[i].transform.Find("TypeName").gameObject.SetActive(false);
                mineObj[i].transform.Find("BoostIcon").gameObject.SetActive(false);
            }
            //건설중
            if (MineData.instance.getMineList()[i].buildState == "beunder")
            {
                mineObj[i].transform.Find("Image").gameObject.SetActive(true);
                Color clr = mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color;
                mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 0.5f);
                mineObj[i].transform.Find("Text").gameObject.SetActive(true);               //시간은 update에서
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[i].transform.Find("pickax").gameObject.SetActive(false);
                mineObj[i].transform.Find("TypeName").gameObject.SetActive(true);
                mineObj[i].transform.Find("BoostIcon").gameObject.SetActive(false);
            }
            //건설 완료
            if (MineData.instance.getMineList()[i].buildState == "complete")
            {
                mineObj[i].transform.Find("Image").gameObject.SetActive(true);
                Color clr = mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color;
                mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1);
                mineObj[i].transform.Find("Text").gameObject.SetActive(true);               //획득량은 update에서
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[i].transform.Find("TypeName").gameObject.SetActive(true);

                //채굴 상태
                if (MineData.instance.getMineList()[i].miningState)
                    mineObj[i].transform.Find("pickax").gameObject.SetActive(true);
                else mineObj[i].transform.Find("pickax").gameObject.SetActive(false);

                //부스트 상태
                if (MineData.instance.getMineList()[i].boostState)
                    mineObj[i].transform.Find("BoostIcon").gameObject.SetActive(true);
                else mineObj[i].transform.Find("BoostIcon").gameObject.SetActive(false);
            }
        }
    }

    public void MineClick(int i) { curMineNum = i; }


}
