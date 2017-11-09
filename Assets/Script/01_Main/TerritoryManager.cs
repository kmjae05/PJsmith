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
    private GameObject ExhaustionPopup;

    private GameObject BlackBack;
    private Button CancleButton;
    private GameObject BottomMenu;
    private GameObject BottomMenuLock;
    private GameObject StartLock;

    private GameObject SystemPopup;
    private Button sys_yesButton;
    private Button sys_NoButton;
    private Button sys_OkButton;

    private GameObject itemList;
    private GameObject itemBox;

    private List<GameObject> mineObj;           //광산 스팟
    private List<GameObject> bottonButtonList;  //하단 버튼

    private List<MineInfo> mineInfo;

    private string curType = null;          //현재 선택된 광산 종류
    private int curMineNum = 0;             //현재 선택한 광산 번호
    private int level = 1;                  //건설할 때 레벨



    void Start()
    {
        uiPanel = GameObject.Find("Menu").transform.Find("TerritoryPopup/UIPanel").gameObject;
        buildInfoPopup = GameObject.Find("System").transform.Find("BuildInfoPopup").gameObject;
        BeUnderPopup = GameObject.Find("System").transform.Find("BeUnderPopup").gameObject;
        MiningPopup = GameObject.Find("System").transform.Find("MiningPopup").gameObject;
        ExhaustionPopup = GameObject.Find("System").transform.Find("ExhaustionPopup").gameObject;

        BlackBack = uiPanel.transform.Find("BlackBack").gameObject;
        CancleButton = BlackBack.transform.Find("CancleButton").gameObject.GetComponent<Button>();
        BottomMenu = uiPanel.transform.Find("BottomMenu").gameObject;
        BottomMenuLock = uiPanel.transform.Find("BottomMenu/BottomMenuLock").gameObject;
        StartLock = GameObject.Find("MenuButton").transform.Find("LockImage").gameObject;
        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        sys_yesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        sys_NoButton = SystemPopup.transform.Find("UIPanel/NoButton").gameObject.GetComponent<Button>();
        sys_OkButton = SystemPopup.transform.Find("UIPanel/OKButton").gameObject.GetComponent<Button>();

        itemList = ExhaustionPopup.transform.Find("UIPanel/ItemList").gameObject;
        itemBox = ExhaustionPopup.transform.Find("UIPanel/ItemList/ItemBox").gameObject;

        mineObj = new List<GameObject>();
        bottonButtonList = new List<GameObject>();
        BottomMenu.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            mineObj.Add(uiPanel.transform.Find("Back/Mine/Spot" + (i + 1).ToString()).gameObject);
            mineObj[i].SetActive(true);
            //광산 스팟 버튼 설정.
            mineObj[i].GetComponent<Button>().onClick.RemoveAllListeners();
            int num = i;
            mineObj[i].GetComponent<Button>().onClick.AddListener(() => 
            {
                BottomMenu.SetActive(true);
                BlackBack.SetActive(true);
                GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(false);
                GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(false);
                GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(false);
                uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(false);
                for (int j = 0; j < MineData.instance.getMineList().Count; j++)
                {
                    if (MineData.instance.getMineList()[j].buildState == "nothing")
                    {
                        mineObj[j].transform.Find("Image").gameObject.SetActive(false);
                        mineObj[j].transform.Find("Text").gameObject.SetActive(false);
                        mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(false);
                        mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
                        //info.upgradeFlag = false;
                    }
                }
                mineObj[num].transform.Find("DottedCircle").gameObject.SetActive(true);
                //건설 취소 버튼
                CancleButton.onClick.AddListener(() =>
                {
                    BottomMenu.SetActive(false);
                    BlackBack.SetActive(false);
                    GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(true);
                    GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(true);
                    GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(true);
                    uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(true);

                    for (int j = 0; j < MineData.instance.getMineList().Count; j++)
                    {
                        if (MineData.instance.getMineList()[j].buildState == "nothing")
                        {
                            mineObj[j].transform.Find("Image").gameObject.SetActive(false);
                            mineObj[j].transform.Find("Text").gameObject.SetActive(false);
                            mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(true);
                            mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
                            //info.upgradeFlag = false;
                        }
                    }
                });
            });
        }
        for (int i = 0; i < 6; i++)
        {
            bottonButtonList.Add(uiPanel.transform.Find("BottomMenu/Button/BottomButton" + (i + 1).ToString()).gameObject);
        }

        mineInfo = MineData.instance.getMineInfoList();

        StateUpdate();
        for (int i = 0; i < bottonButtonList.Count; i++)
        {
            int index = i;
            bottonButtonList[i].transform.Find("OreImage").gameObject.GetComponent<Button>().onClick.AddListener(() => BottomButtonSetting(index));
            if (i>=2 && Player.instance.getUser().level >= MineData.instance.getMineInfoList()[i].buildLevel)
                bottonButtonList[i].transform.Find("LockImage").gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        uiPanel.transform.Find("Back/Smithy/Level/LevelText").gameObject.GetComponent<Text>().text = "Level " + Player.instance.getUser().level.ToString();
        for (int i = 2; i < bottonButtonList.Count; i++)
            if (Player.instance.getUser().level >= MineData.instance.getMineInfoList()[i].buildLevel)
                bottonButtonList[i].transform.Find("LockImage").gameObject.SetActive(false);
        for (int i = 0; i < MineData.instance.getMineList().Count; i++)
        {
            //건설 중
            if (MineData.instance.getMineList()[i].buildState == "beunder"
                || MineData.instance.getMineList()[i].buildState == "upgrade")
            {
                mineObj[i].transform.Find("Dust").gameObject.SetActive(true);
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
                    float curTime = MineData.instance.getMineList()[curMineNum].buildTime;
                    mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(1, 1, 1);
                    if (time > 3600)
                    {
                        mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)(time / 3600)).ToString() + "시간 " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)(curTime / 3600)).ToString() + "시간 " + ((int)((curTime % 3600) / 60)).ToString() + "분 " + ((int)(curTime % 60)).ToString() + "초";
                    }
                    else if (time > 60)
                    {
                        mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)((curTime % 3600) / 60)).ToString() + "분 " + ((int)(curTime % 60)).ToString() + "초";
                    }
                    else
                    {
                        mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)(curTime % 60)).ToString() + "초";
                    }
                }
            }
            //건설 완료 & 채굴 상태
            if (MineData.instance.getMineList()[i].buildState == "complete" && MineData.instance.getMineList()[i].miningState)
            {
                Color clr = mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color;
                mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
                mineObj[i].transform.Find("Image").gameObject.SetActive(true);
                mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
                mineObj[i].transform.Find("Text").gameObject.SetActive(true);
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[i].transform.Find("pickax").gameObject.SetActive(true);
                mineObj[i].transform.Find("Dust").gameObject.SetActive(false);

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
                mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(1f, 0.2f, 0.21f);

                mineObj[i].transform.Find("pickax").gameObject.SetActive(false);
                mineObj[i].transform.Find("Dust").gameObject.SetActive(false);
                if (curMineNum == i) { MiningPopup.SetActive(false); }

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
        buildInfoPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text =mineInfo[index].level.ToString();
        buildInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = mineInfo[index].buildTime + "초";
        buildInfoPopup.transform.Find("UIPanel/Frame/Ore").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == mineInfo[index].getThingName[0]).icon);

        string mtr = null;
        destroyItemBox(buildInfoPopup.transform.Find("UIPanel/ItemList").gameObject);
        for (int i = 0; i < mineInfo[index].getThingName.Length; i++)
        {
            if (mineInfo[index].getThingName.Length == 1)
            {
                mtr = mineInfo[index].getThingName[0] ;
                GameObject box = Instantiate(buildInfoPopup.transform.Find("UIPanel/ItemList/ItemBox").gameObject);
                box.transform.SetParent(buildInfoPopup.transform.Find("UIPanel/ItemList").gameObject.transform, false);
                box.transform.Find("Icon").gameObject.GetComponent<Image>().sprite =Resources.Load<Sprite>( ThingsData.instance.getThingsList().Find(x => x.name == mineInfo[index].getThingName[0]).icon);
                box.transform.Find("NameText").gameObject.GetComponent<Text>().text = mineInfo[index].getThingName[0];
                box.SetActive(true);

                break;
            }
            else
            {
                if (i == 0) { mtr = mineInfo[index].getThingName[0] ; }
                else { mtr += ", " + mineInfo[index].getThingName[i]; }
                GameObject box = Instantiate(buildInfoPopup.transform.Find("UIPanel/ItemList/ItemBox").gameObject);
                box.transform.SetParent(buildInfoPopup.transform.Find("UIPanel/ItemList").gameObject.transform, false);
                box.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == mineInfo[index].getThingName[i]).icon);
                box.transform.Find("NameText").gameObject.GetComponent<Text>().text = mineInfo[index].getThingName[i];
                box.SetActive(true);
            }
        }
        //buildInfoPopup.transform.Find("UIPanel/InfoBox/GetItemText").gameObject.GetComponent<Text>().text = mtr;

        buildInfoPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text =  MineData.instance.getMineBuildList().Find(x => x.level == mineInfo[index].level).deposit.ToString();
        mtr = null;
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
        mineInfo[index].curMaterial = mineInfo[index].necessaryMaterialsNum[0];
        buildInfoPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text =  mtr + "개";
        //보유 개수
        InventoryThings have = ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineInfo[index].type);
        if(have == null)
        {
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().text = "보유 재료 개수 : 0";
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }
        else
        {
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().text = "보유 재료 개수 : " + have.possession;
            if (have.possession >= mineInfo[index].necessaryMaterialsNum[0])
                buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
            else buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }
        buildInfoPopup.transform.Find("UIPanel/CurInfoBox").gameObject.SetActive(false);
        buildInfoPopup.transform.Find("UIPanel/ArrowBox").gameObject.SetActive(false);

        buildInfoPopup.transform.Find("UIPanel/UpgradeButton").gameObject.SetActive(true);
        buildInfoPopup.transform.Find("UIPanel/UpgradeInitButton").gameObject.SetActive(false);

        curType = mineInfo[index].type;

    }

    //건설 버튼
    public void BuildButton()
    {
        MineInfo info = mineInfo.Find(x => x.type == curType);

        //재료랑 갖고 있는 아이템 체크
        InventoryThings thing = ThingsData.instance.getInventoryThingsList().Find(x => x.name == info.necessaryMaterials[0]);
        if (thing != null)
        {
            if (thing.possession >= info.curMaterial)
            {
                //건설 조건 만족
                Debug.Log("y");
            }
            else
            {
                //재료 수량 부족
                Debug.Log("no");
                //팝업
                SystemPopup.SetActive(true);
                SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "재료 부족";
                SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "광산 건설에 필요한 재료가 부족합니다.";
                sys_yesButton.gameObject.SetActive(false);
                sys_NoButton.gameObject.SetActive(false);
                sys_OkButton.gameObject.SetActive(true);

                return;
            }
        }
        //아이템 못 찾음
        else
        {
            Debug.Log("null");
            SystemPopup.SetActive(true);
            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "재료 부족";
                SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "광산 건설에 필요한 재료가 부족합니다.";
                sys_yesButton.gameObject.SetActive(false);
                sys_NoButton.gameObject.SetActive(false);
                sys_OkButton.gameObject.SetActive(true);
                return;
            }

        //}
        //재료 체크 완료//
        BottomMenu.SetActive(false);
        BlackBack.SetActive(false);
        GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(true);
        GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(true);
        GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(true);
        uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(true);

        buildInfoPopup.SetActive(false);
        //건설 취소 닫기 버튼
        CancleButton.onClick.AddListener(() => 
        {
            BlackBack.SetActive(false);
            GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(true);
            GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(true);
            GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(true);

            for (int j = 0; j < MineData.instance.getMineList().Count; j++)
            {
                if (MineData.instance.getMineList()[j].buildState == "nothing")
                {
                    mineObj[j].transform.Find("Image").gameObject.SetActive(false);
                    mineObj[j].transform.Find("Text").gameObject.SetActive(false);
                    mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(true);
                    mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
                    info.upgradeFlag = false;
                }
            }
        });

        int num = curMineNum;// MineData.instance.getMineList().FindIndex(x => x.type == curType)-1;
        //광산에 정보 저장
        MineData.instance.getMineList()[num].type = info.type;
        MineData.instance.getMineList()[num].level = info.afterLevel;
        if (info.upgradeFlag)
        {
            MineData.instance.getMineList()[num].buildState = "upgrade";
            info.upgradeFlag = false;
        }
        else
        {
            MineData.instance.getMineList()[num].buildState = "beunder";
            info.afterLevel = info.level;
            info.afterTime = (int)info.buildTime;
            info.afterDeposit = MineData.instance.getMineBuildList().Find(x => x.level == info.level).deposit;
            info.curMaterial = info.necessaryMaterialsNum[0];
        }

        MineData.instance.getMineList()[num].buildTime = info.afterTime;
        Debug.Log(info.afterTime);
        for (int i = 0; i < info.getThingName.Length; i++)
        {
            MineData.instance.getMineList()[num].getThingName[i] = info.getThingName[i];
        }
        MineData.instance.getMineList()[num].getOnceAmount = 1;
        MineData.instance.getMineList()[num].deposit = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).deposit;
        MineData.instance.getMineList()[num].miningTime = 1f;

        //재료 소모
        //for (int i = 0; i < info.necessaryMaterials.Length; i++)
        //{
        thing.possession -= info.curMaterial;
        //}
        GameObject obj = GameObject.Find("Mine").transform.Find("Spot" + (num + 1).ToString()).gameObject;
        obj.GetComponent<Button>().onClick.RemoveAllListeners();
        Debug.Log(curMineNum);
        obj.GetComponent<Button>().onClick.AddListener(() => BuildCondition(obj, num));

        obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        obj.transform.Find("Image").gameObject.SetActive(true);
        obj.transform.Find("Text").gameObject.SetActive(true);
        obj.transform.Find("DottedCircle").gameObject.SetActive(false);
        obj.transform.Find("pickax").gameObject.SetActive(false);
        obj.transform.Find("Dust").gameObject.SetActive(true);
        obj.transform.Find("TypeName").gameObject.SetActive(true);
        obj.transform.Find("TypeName/TypeNameText").gameObject.GetComponent<Text>().text = info.type + " 광산";


        //나머지 nothing없애고 건설선택 블랙백 없애기
        for (int j = 0; j < MineData.instance.getMineList().Count; j++)
        {
            if (MineData.instance.getMineList()[j].buildState == "nothing")
            {
                mineObj[j].transform.Find("Image").gameObject.SetActive(false);
                mineObj[j].transform.Find("Text").gameObject.SetActive(false);
                mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(true);
                mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
            }
        }
        BlackBack.SetActive(false);
        //하단메뉴 잠금
        //BottomMenuLock.SetActive(true);
        //대장장이 행동 제한
        //StartLock.SetActive(true);

        ////빈 스팟 띄우기
        //for (int j = 0; j < MineData.instance.getMineList().Count; j++)
        //{
        //    if (MineData.instance.getMineList()[j].buildState == "nothing")
        //    {
        //        mineObj[j].GetComponent<Button>().onClick.RemoveAllListeners();
        //        GameObject obj = mineObj[j];
        //        int num = j;
        //        mineObj[j].GetComponent<Button>().onClick.AddListener(() => BuildSpotClick(obj, num));
        //        mineObj[j].transform.Find("Image").gameObject.SetActive(false);
        //        mineObj[j].transform.Find("Text").gameObject.SetActive(false);
        //        mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(true);
        //        mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
        //    }
        //}
    }
    //업그레이드 버튼
    public void UpgradeButton()
    {
        MineInfo info = mineInfo.Find(x => x.type == curType);
        info.upgradeFlag = true;
        
        info.afterLevel = info.level + 1;
        info.afterTime = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).time; Debug.Log(info.afterTime);
        info.afterDeposit = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).deposit;
        info.curMaterial = MineData.instance.getMineBuildList().Find(x => x.level == info.level).material;

        //텍스트 변경.
        buildInfoPopup.transform.Find("UIPanel/CurInfoBox/LevelText").gameObject.GetComponent<Text>().text = info.level.ToString();
        buildInfoPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
        buildInfoPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = info.afterLevel.ToString();

        buildInfoPopup.transform.Find("UIPanel/CurInfoBox/TimeText").gameObject.GetComponent<Text>().text = info.buildTime.ToString() + "초";
        buildInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
        buildInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = info.afterTime + "초";

        buildInfoPopup.transform.Find("UIPanel/CurInfoBox/DepositText").gameObject.GetComponent<Text>().text = MineData.instance.getMineBuildList().Find(x => x.level == info.level).deposit.ToString();
        buildInfoPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
        buildInfoPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text = info.afterDeposit.ToString();

        buildInfoPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text = curType + " " + info.curMaterial + "개";

        //보유 개수
        InventoryThings have = ThingsData.instance.getInventoryThingsList().Find(x => x.name == info.type);
        if (have == null)
        {
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().text = "보유 재료 개수 : 0";
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }
        else
        {
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().text = "보유 재료 개수 : " + have.possession;
            if (have.possession >= info.curMaterial)
                buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
            else buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }
        buildInfoPopup.transform.Find("UIPanel/CurInfoBox").gameObject.SetActive(true);
        buildInfoPopup.transform.Find("UIPanel/ArrowBox").gameObject.SetActive(true);


    }
    //업그레이드 초기화 버튼
    public void UpgradeInitButton()
    {
        MineInfo info = mineInfo.Find(x => x.type == curType);
        info.upgradeFlag = false;
        info.afterLevel = info.level;
        info.afterTime = (int)info.buildTime;
        info.afterDeposit = MineData.instance.getMineBuildList().Find(x => x.level == info.level).deposit;
        info.curMaterial = info.necessaryMaterialsNum[0];
        //텍스트 변경.
        buildInfoPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = info.level.ToString();
        buildInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = info.buildTime + "초";
        buildInfoPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text =  MineData.instance.getMineBuildList().Find(x => x.level == info.level).deposit.ToString();
        buildInfoPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text = curType + " " + info.necessaryMaterialsNum[0].ToString() + "개";

        //텍스트 색 변경
        buildInfoPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().color = buildInfoPopup.transform.Find("UIPanel/CurInfoBox/LevelText").gameObject.GetComponent<Text>().color;
        buildInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().color = buildInfoPopup.transform.Find("UIPanel/CurInfoBox/TimeText").gameObject.GetComponent<Text>().color;
        buildInfoPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().color = buildInfoPopup.transform.Find("UIPanel/CurInfoBox/DepositText").gameObject.GetComponent<Text>().color;

        //보유 개수
        InventoryThings have = ThingsData.instance.getInventoryThingsList().Find(x => x.name == info.type);
        if (have == null)
        {
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().text = "보유 재료 개수 : 0";
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }
        else
        {
            buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().text = "보유 재료 개수 : " + have.possession;
            if (have.possession >= info.curMaterial)
                buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
            else buildInfoPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }
        buildInfoPopup.transform.Find("UIPanel/CurInfoBox").gameObject.SetActive(false);
        buildInfoPopup.transform.Find("UIPanel/ArrowBox").gameObject.SetActive(false);

    }

    ////건설 스팟을 선택했을 때 동작
    //public void BuildSpotClick(GameObject obj, int num)
    //{
    //    MineInfo info = mineInfo.Find(x => x.type == curType);

    //    //광산에 정보 저장
    //    MineData.instance.getMineList()[num].type = info.type;
    //    MineData.instance.getMineList()[num].level = info.afterLevel;
    //    if (info.upgradeFlag)
    //    {
    //        MineData.instance.getMineList()[num].buildState = "upgrade";
    //        info.upgradeFlag = false;
    //    }
    //    else
    //    {
    //        MineData.instance.getMineList()[num].buildState = "beunder";
    //        info.afterLevel = info.level;
    //        info.afterTime = (int)info.buildTime;
    //        info.afterDeposit = MineData.instance.getMineBuildList().Find(x => x.level == info.level).deposit;
    //        info.curMaterial = info.necessaryMaterialsNum[0];
    //    }

    //    MineData.instance.getMineList()[num].buildTime = info.afterTime;
    //    Debug.Log(info.afterTime);
    //    for (int i = 0; i < info.getThingName.Length; i++)
    //    {
    //        MineData.instance.getMineList()[num].getThingName[i] = info.getThingName[i];
    //    }
    //    MineData.instance.getMineList()[num].getOnceAmount = 1;
    //    MineData.instance.getMineList()[num].deposit = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).deposit;
    //    MineData.instance.getMineList()[num].miningTime = 1f;

    //    //재료 소모
    //    //for (int i = 0; i < info.necessaryMaterials.Length; i++)
    //    //{
    //    InventoryThings thing = ThingsData.instance.getInventoryThingsList().Find(x => x.name == info.necessaryMaterials[0]);
    //        thing.possession -= info.curMaterial;
    //    //}

    //    obj.GetComponent<Button>().onClick.RemoveAllListeners();
    //    obj.GetComponent<Button>().onClick.AddListener(() => BuildCondition(obj, num));

    //    obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
    //    obj.transform.Find("Image").gameObject.SetActive(true);
    //    obj.transform.Find("Text").gameObject.SetActive(true);
    //    obj.transform.Find("DottedCircle").gameObject.SetActive(false);
    //    obj.transform.Find("pickax").gameObject.SetActive(false);
    //    obj.transform.Find("TypeName").gameObject.SetActive(true);
    //    obj.transform.Find("TypeName/TypeNameText").gameObject.GetComponent<Text>().text = info.type + " 광산";


    //    //나머지 nothing없애고 건설선택 블랙백 없애기
    //    for (int j = 0; j < MineData.instance.getMineList().Count; j++)
    //    {
    //        if (MineData.instance.getMineList()[j].buildState == "nothing")
    //        {
    //            mineObj[j].transform.Find("Image").gameObject.SetActive(false);
    //            mineObj[j].transform.Find("Text").gameObject.SetActive(false);
    //            mineObj[j].transform.Find("DottedCircle").gameObject.SetActive(false);
    //            mineObj[j].transform.Find("pickax").gameObject.SetActive(false);
    //        }
    //    }
    //    BlackBack.SetActive(false);
    //    //하단메뉴 잠금
    //    BottomMenuLock.SetActive(true);
    //    //대장장이 행동 제한
    //    //StartLock.SetActive(true);

    //}




    //건설 진행 중 버튼 (현황 창
    public void BuildCondition(GameObject obj, int num)
    {
        Debug.Log(num);
        string type = MineData.instance.getMineList()[num].type;

        if (MineData.instance.getMineList()[num].buildState == "upgrade")
        {
            BeUnderPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = type + " 광산 업그레이드 중";

            BeUnderPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = 
                "레벨 : " + (MineData.instance.getMineList()[num].level).ToString();
            BeUnderPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text
                = "매장량 : " + MineData.instance.getMineBuildList().Find(x=>x.level ==  MineData.instance.getMineList()[num].level).deposit.ToString();
        }
        if (MineData.instance.getMineList()[num].buildState == "beunder")
        {
            BeUnderPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = type + " 광산 건설 중";

            BeUnderPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = "레벨 : " + MineData.instance.getMineList()[num].level.ToString();
            BeUnderPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text = "매장량 : " + MineData.instance.getMineList()[num].deposit.ToString();
        }
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
                
                if (Player.instance.getUser().cash < 20)
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

                Player.instance.LostMoney("cash", 5);

                //
                if (MineData.instance.getMineList()[num].buildState == "upgrade")
                {
                    MineData.instance.getMineInfoList().Find(x => x.type == type).level++;
                    MineData.instance.getMineList()[num].level = MineData.instance.getMineInfoList().Find(x => x.type == type).level;
                    MineData.instance.getMineInfoList().Find(x => x.type == type).buildTime 
                        = MineData.instance.getMineBuildList().Find(x => x.level == MineData.instance.getMineList()[num].level).time;
                    MineData.instance.getMineList()[num].buildTime = MineData.instance.getMineInfoList().Find(x => x.type == type).buildTime;
                    MineData.instance.getMineList()[num].deposit 
                        = MineData.instance.getMineBuildList().Find(x => x.level == MineData.instance.getMineList()[num].level).deposit;
                }

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
                mineObj[num].transform.Find("Dust").gameObject.SetActive(false);

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
                obj.transform.Find("DottedCircle").gameObject.SetActive(true);
                obj.transform.Find("TypeName").gameObject.SetActive(false);
                BottomMenuLock.SetActive(false);
                StartLock.SetActive(false);
                if (BeUnderPopup.activeInHierarchy) BeUnderPopup.SetActive(false);
                //광산 스팟 버튼 설정.
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    BottomMenu.SetActive(true);
                    BlackBack.SetActive(true);
                    GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(false);
                    GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(false);
                    GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(false);
                    uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(false);
                    //건설 취소 버튼
                    CancleButton.onClick.AddListener(() =>
                    {
                        BottomMenu.SetActive(false);
                        BlackBack.SetActive(false);
                        GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(true);
                        GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(true);
                        GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(true);
                        uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(true);
                        obj.transform.Find("DottedCircle").gameObject.SetActive(true);
                    
                    });
                });
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
                
                if (Player.instance.getUser().cash < 5)
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
                Player.instance.LostMoney("cash", 1);

                MineData.instance.getMineList()[num].miningState = false;

                //부가 아이템
                int rest = MineData.instance.getMineList()[num].deposit - MineData.instance.getMineList()[num].getAmount;
                for (int i = 0; i < rest; i++)
                {
                    for (int j = 1; j < MineData.instance.getMineList()[num].getThingName.Length; j++)
                    {
                        if (MineData.instance.getMineList()[num].getThingName[j] != null)
                        {
                            int random = UnityEngine.Random.Range(1, 100 + 1);      //100확률
                                                                                    //Debug.Log(random);
                            int prob = MineData.instance.getMineInfoList().Find(x => x.type == MineData.instance.getMineList()[num].type).getThingProb[j];  //아이템 확률
                            if (random <= prob)
                            {
                                //Debug.Log(MineData.instance.getMineList()[num].getThingName[j] + " 획득");
                                MineData.instance.getMineList()[num].getThingNum[j]++;
                            }
                        }
                    }
                }


                obj.transform.Find("Text").gameObject.GetComponent<Text>().text = MineData.instance.getMineList()[num].deposit.ToString() + "개 채굴 완료";
                obj.transform.Find("pickax").gameObject.SetActive(false);
                obj.transform.Find("Dust").gameObject.SetActive(false);
                obj.transform.Find("BoostIcon").gameObject.SetActive(false);
            });

        });

        //부스트
        MiningPopup.transform.Find("UIPanel/BoostButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        MiningPopup.transform.Find("UIPanel/BoostButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "부스트 아이템 사용";
            //부스트 아이템 없는 경우
            if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "부스트") == null)
            {
                SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "부스트 아이템을 사용하시겠습니까? (남은 개수 : 0)";
            }
            //있는 경우
            else
            {
                SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "부스트 아이템을 사용하시겠습니까?"
                + "(남은 개수 : " + ThingsData.instance.getInventoryThingsList().Find(x => x.name == "부스트").possession.ToString() + ")";
            }
            sys_yesButton.gameObject.SetActive(true);
            sys_NoButton.gameObject.SetActive(true);
            sys_OkButton.gameObject.SetActive(false);
            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "부스트") == null)
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
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == "부스트").possession -= 1;
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
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "채굴을 중지하고 폐광하겠습니까?";
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
                MineData.instance.getMineList()[num].getAmount = 0;
                obj.transform.Find("Image").gameObject.SetActive(false);
                obj.transform.Find("Text").gameObject.SetActive(false);
                obj.transform.Find("DottedCircle").gameObject.SetActive(true);
                obj.transform.Find("pickax").gameObject.SetActive(false);
                obj.transform.Find("Dust").gameObject.SetActive(false);
                obj.transform.Find("TypeName").gameObject.SetActive(false);
                obj.transform.Find("BoostIcon").gameObject.SetActive(false);
                //광산 스팟 버튼 설정.
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    BottomMenu.SetActive(true);
                    BlackBack.SetActive(true);
                    GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(false);
                    GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(false);
                    GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(false);
                    uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(false);
                    //건설 취소 버튼
                    CancleButton.onClick.AddListener(() =>
                    {
                        BottomMenu.SetActive(false);
                        BlackBack.SetActive(false);
                        GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(true);
                        GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(true);
                        GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(true);
                        uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(true);
                        obj.transform.Find("DottedCircle").gameObject.SetActive(true);

                    });
                });
            });

        });
    }

    //광석 획득
    public void getOre(GameObject obj, int num)
    {
        //Debug.Log("완료");

        Player.instance.getExp(10);

        MineData.instance.getMineList()[num].buildState = "exhaustion";
        MineData.instance.getMineList()[num].getAmount = 0;
        MineData.instance.getMineList()[num].curTime = 0f;
        MineData.instance.getMineList()[num].boostState = false;

        obj.transform.Find("Text").gameObject.GetComponent<Text>().text = "고갈";
        obj.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(0.78f, 0.78f, 0.78f);
        obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        //obj.transform.Find("Image").gameObject.SetActive(false);
        //obj.transform.Find("Text").gameObject.SetActive(false);
        //obj.transform.Find("DottedCircle").gameObject.SetActive(false);
        obj.transform.Find("pickax").gameObject.SetActive(false);
        obj.transform.Find("Dust").gameObject.SetActive(false);
        //obj.transform.Find("TypeName").gameObject.SetActive(false);
        obj.transform.Find("BoostIcon").gameObject.SetActive(false);

        //기본 광석 획득
        if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName[0]) != null)
        {
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName[0]).possession
                += MineData.instance.getMineList()[num].deposit;
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName[0]).recent = true;
        }
        else
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                x => x.name == MineData.instance.getMineList()[num].getThingName[0]).type, MineData.instance.getMineList()[num].getThingName[0], MineData.instance.getMineList()[num].deposit));
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName[0]).recent = true;
        }

        //부가 아이템 획득
        for (int i = 1; i < MineData.instance.getMineList()[num].getThingName.Length; i++)
        {
            if (MineData.instance.getMineList()[num].getThingName[i] != null)
            {
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName[i]) != null)
                {
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName[i]).possession
                        += MineData.instance.getMineList()[num].getThingNum[i];
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName[i]).recent = true;

                }
                else
                {
                    ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                        x => x.name == MineData.instance.getMineList()[num].getThingName[i]).type, MineData.instance.getMineList()[num].getThingName[i], MineData.instance.getMineList()[num].getThingNum[i]));
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == MineData.instance.getMineList()[num].getThingName[i]).recent = true;
                }
            }
        }

        int[] itemnum = new int[3];
        itemnum[0] = MineData.instance.getMineList()[num].deposit;
        itemnum[1] = MineData.instance.getMineList()[num].getThingNum[1];
        itemnum[2] = MineData.instance.getMineList()[num].getThingNum[2];
        GameObject.Find("InventoryScript").GetComponent<GetItemManager>().getItem(MineData.instance.getMineList()[num].getThingName, itemnum);


        //로그
        //GameObject ItemLogInst = Instantiate(GameObject.Find("GetItemLog").transform.Find("range/GetItemLogText").gameObject);
        //ItemLogInst.transform.SetParent(GameObject.Find("GetItemLog").transform.Find("range").gameObject.transform, false);
        //ItemLogInst.GetComponent<Text>().text = MineData.instance.getMineList()[num].getThingName[0] + " " + 
        //    MineData.instance.getMineList()[num].deposit + "개 획득";
        //ItemLogInst.SetActive(true);

        //획득한 아이템 개수 0으로 초기화
        for (int i = 0; i < MineData.instance.getMineList()[num].getThingNum.Length; i++)
            MineData.instance.getMineList()[num].getThingNum[i] = 0;

        obj.GetComponent<Button>().onClick.RemoveAllListeners();
        obj.GetComponent<Button>().onClick.AddListener(() => ExhaustionCondition(obj, num));

    }



    //광산 고갈 상태 팝업
    public void ExhaustionCondition(GameObject obj, int num)
    {
        MineInfo info = mineInfo.Find(x => x.type == curType);

        ExhaustionPopup.SetActive(true);

        info.afterLevel = info.level + 1;
        info.afterTime = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).time;
        info.afterDeposit = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).deposit;
        info.curMaterial = MineData.instance.getMineBuildList().Find(x => x.level == info.level).material;

        ExhaustionPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = info.type + "광산 고갈";

        ExhaustionPopup.transform.Find("UIPanel/CurInfoBox/LevelText").gameObject.GetComponent<Text>().text = info.level.ToString();
        ExhaustionPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
        ExhaustionPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = info.afterLevel.ToString();

        ExhaustionPopup.transform.Find("UIPanel/CurInfoBox/TimeText").gameObject.GetComponent<Text>().text = info.buildTime.ToString() + "초";
        ExhaustionPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
        ExhaustionPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = info.afterTime + "초";

        ExhaustionPopup.transform.Find("UIPanel/CurInfoBox/DepositText").gameObject.GetComponent<Text>().text = MineData.instance.getMineBuildList().Find(x => x.level == info.level).deposit.ToString();
        ExhaustionPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
        ExhaustionPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text = info.afterDeposit.ToString();

        string mtr = null;
        destroyItemBox(itemList);
        for (int i = 0; i < info.getThingName.Length; i++)
        {
            if (info.getThingName.Length == 1)
            {
                mtr = info.getThingName[0];
                GameObject box = Instantiate(itemBox);
                box.transform.SetParent(itemList.transform, false);
                box.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == info.getThingName[0]).icon);
                box.transform.Find("NameText").gameObject.GetComponent<Text>().text = info.getThingName[0];
                box.SetActive(true);

                break;
            }
            else
            {
                if (i == 0) { mtr = info.getThingName[0]; }
                else { mtr += ", " + info.getThingName[i]; }
                GameObject box = Instantiate(itemBox);
                box.transform.SetParent(itemList.transform, false);
                box.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == info.getThingName[i]).icon);
                box.transform.Find("NameText").gameObject.GetComponent<Text>().text = info.getThingName[i];
                box.SetActive(true);

            }
        }
        //ExhaustionPopup.transform.Find("UIPanel/InfoBox/GetItemText").gameObject.GetComponent<Text>().text =  mtr;
        
        ExhaustionPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text =  curType + " " + info.curMaterial + "개";
        //보유 개수
        InventoryThings have = ThingsData.instance.getInventoryThingsList().Find(x => x.name == info.type);
        if (have == null)
        {
            ExhaustionPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().text = "보유 재료 개수 : 0";
            ExhaustionPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }
        else
        {
            ExhaustionPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().text = "보유 재료 개수 : " + have.possession;
            if (have.possession >= info.curMaterial)
                ExhaustionPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
            else ExhaustionPopup.transform.Find("UIPanel/HaveText").gameObject.GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }

        //레벨업
        ExhaustionPopup.transform.Find("UIPanel/UpgradeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        ExhaustionPopup.transform.Find("UIPanel/UpgradeButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {

            //재료 모자람
            InventoryThings thing = ThingsData.instance.getInventoryThingsList().Find(x => x.name == info.necessaryMaterials[0]);
            if (thing != null)
            {
                if (thing.possession >= info.curMaterial)
                {
                    //건설 조건 만족
                    Debug.Log("y");
                }
                else
                {
                    //재료 수량 부족
                    Debug.Log("no");
                    //팝업
                    SystemPopup.SetActive(true);
                    SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "재료 부족";
                    SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "광산 건설에 필요한 재료가 부족합니다.";
                    sys_yesButton.gameObject.SetActive(false);
                    sys_NoButton.gameObject.SetActive(false);
                    sys_OkButton.gameObject.SetActive(true);

                    return;
                }
            }
            //아이템 못 찾음
            else
            {
                Debug.Log("null");
                SystemPopup.SetActive(true);
                SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "재료 부족";
                SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "광산 건설에 필요한 재료가 부족합니다.";
                sys_yesButton.gameObject.SetActive(false);
                sys_NoButton.gameObject.SetActive(false);
                sys_OkButton.gameObject.SetActive(true);
                return;
            }
            //재료 체크 완료//

            //스팟 선택 없이 바로 그 자리에 건설.

            //광산에 정보 저장

            MineData.instance.getMineList()[num].type = info.type;
            MineData.instance.getMineList()[num].level = info.afterLevel;
            MineData.instance.getMineList()[num].buildState = "upgrade";
            info.upgradeFlag = false;
            
            MineData.instance.getMineList()[num].buildTime = info.afterTime;
            Debug.Log(info.afterTime);
            for (int i = 0; i < info.getThingName.Length; i++)
            {
                MineData.instance.getMineList()[num].getThingName[i] = info.getThingName[i];
            }
            MineData.instance.getMineList()[num].getOnceAmount = 1;
            MineData.instance.getMineList()[num].deposit = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).deposit;
            MineData.instance.getMineList()[num].miningTime = 1f;

            //재료 소모
            thing.possession -= info.curMaterial;

            obj.GetComponent<Button>().onClick.RemoveAllListeners();
            obj.GetComponent<Button>().onClick.AddListener(() => BuildCondition(obj, num));

            obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            obj.transform.Find("Image").gameObject.SetActive(true);
            obj.transform.Find("Text").gameObject.SetActive(true);
            obj.transform.Find("DottedCircle").gameObject.SetActive(false);
            obj.transform.Find("pickax").gameObject.SetActive(false);
            obj.transform.Find("Dust").gameObject.SetActive(true);
            obj.transform.Find("TypeName").gameObject.SetActive(true);
            obj.transform.Find("TypeName/TypeNameText").gameObject.GetComponent<Text>().text = info.type + " 광산";


            BlackBack.SetActive(false);
            //하단메뉴 잠금
            //BottomMenuLock.SetActive(true);
            //대장장이 행동 제한
            //StartLock.SetActive(true);

            ExhaustionPopup.SetActive(false);
        });



        //폐광
        ExhaustionPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        ExhaustionPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "광산 폐광";
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "폐광하겠습니까?";
            sys_yesButton.gameObject.SetActive(true);
            sys_NoButton.gameObject.SetActive(true);
            sys_OkButton.gameObject.SetActive(false);
            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                SystemPopup.SetActive(false);
                ExhaustionPopup.SetActive(false);

                MineData.instance.getMineList()[num].buildTime = 0f;
                MineData.instance.getMineList()[num].buildState = "nothing";
                MineData.instance.getMineList()[num].miningState = false;
                MineData.instance.getMineList()[num].boostState = false;
                MineData.instance.getMineList()[num].getAmount = 0;
                Color clr = obj.transform.Find("Image").gameObject.GetComponent<Image>().color;
                obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 0.5f);
                obj.transform.Find("Image").gameObject.SetActive(false);
                obj.transform.Find("Text").gameObject.SetActive(false);
                obj.transform.Find("DottedCircle").gameObject.SetActive(true);
                obj.transform.Find("pickax").gameObject.SetActive(false);
                obj.transform.Find("TypeName").gameObject.SetActive(false);
                obj.transform.Find("BoostIcon").gameObject.SetActive(false);
                //광산 스팟 버튼 설정.
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
                obj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    BottomMenu.SetActive(true);
                    BlackBack.SetActive(true);
                    GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(false);
                    GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(false);
                    GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(false);
                    uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(false);
                    //건설 취소 버튼
                    CancleButton.onClick.AddListener(() =>
                    {
                        BottomMenu.SetActive(false);
                        BlackBack.SetActive(false);
                        GameObject.Find("Lobby").transform.Find("ProfilePanel").gameObject.SetActive(true);
                        GameObject.Find("Lobby").transform.Find("MoneyPanel").gameObject.SetActive(true);
                        GameObject.Find("Lobby").transform.Find("MenuButton").gameObject.SetActive(true);
                        uiPanel.transform.Find("WorldMapButton").gameObject.SetActive(true);
                        obj.transform.Find("DottedCircle").gameObject.SetActive(true);

                    });
                });
            });

        });

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
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(true);
                mineObj[i].transform.Find("pickax").gameObject.SetActive(false);
                mineObj[i].transform.Find("Dust").gameObject.SetActive(false);
                mineObj[i].transform.Find("TypeName").gameObject.SetActive(false);
                mineObj[i].transform.Find("BoostIcon").gameObject.SetActive(false);
            }
            //건설중
            if (MineData.instance.getMineList()[i].buildState == "beunder" || MineData.instance.getMineList()[i].buildState == "upgrade")
            {
                mineObj[i].transform.Find("Image").gameObject.SetActive(true);
                Color clr = mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color;
                mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 0.5f);
                mineObj[i].transform.Find("Text").gameObject.SetActive(true);               //시간은 update에서
                mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(1, 1, 1);
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[i].transform.Find("pickax").gameObject.SetActive(false);
                mineObj[i].transform.Find("Dust").gameObject.SetActive(true);
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
                mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[i].transform.Find("TypeName").gameObject.SetActive(true);
                mineObj[i].transform.Find("Dust").gameObject.SetActive(false);

                //채굴 상태
                if (MineData.instance.getMineList()[i].miningState)
                    mineObj[i].transform.Find("pickax").gameObject.SetActive(true);
                else mineObj[i].transform.Find("pickax").gameObject.SetActive(false);

                //부스트 상태
                if (MineData.instance.getMineList()[i].boostState)
                    mineObj[i].transform.Find("BoostIcon").gameObject.SetActive(true);
                else mineObj[i].transform.Find("BoostIcon").gameObject.SetActive(false);
            }
            //고갈
            if (MineData.instance.getMineList()[i].buildState == "exhaustion")
            {
                mineObj[i].transform.Find("Image").gameObject.SetActive(true);
                mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 0.8f);
                mineObj[i].transform.Find("Text").gameObject.SetActive(true);               //시간은 update에서
                mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(1f, 0.2f, 0.21f);
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj[i].transform.Find("pickax").gameObject.SetActive(false);
                mineObj[i].transform.Find("Dust").gameObject.SetActive(false);
                mineObj[i].transform.Find("TypeName").gameObject.SetActive(true);
                mineObj[i].transform.Find("BoostIcon").gameObject.SetActive(false);
            }



        }
    }


    //월드맵 위치 조정
    public void worldMapPosition()
    {
        GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back").gameObject.transform.localPosition = new Vector3(0, 0, 0);
        //크기 조정
        GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel").gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        GameObject monster = GameObject.Find("Monster");
        if (monster.transform.childCount > 2)
        {
            for (int i = 5; i < monster.transform.childCount; i++)
            {

                if (monster.transform.GetChild(i).gameObject.name == "Syaonil(Clone)")
                {
                    monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                }
                else
                {
                    monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(4f, 4f, 4f);
                }
            }
        }
    }
    //영지 위치 조정
    public void territoryPosition()
    {
        uiPanel.transform.Find("Back").gameObject.transform.localPosition = new Vector3(0, 0, 0);
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


    public void MineClick(int i) { curMineNum = i; }


}
