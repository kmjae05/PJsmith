using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMineManager : MonoBehaviour {

    //스테이지 정보
    private StageMineData stageMineData;

    //광산 정보
    private List<Mine> mineList;
    private bool[] mineFlag;

    //광산 관련
    private GameObject MineButtonObj;
    private GameObject BeUnderPopup;
    private GameObject MiningPopup;
    private GameObject ExhaustionPopup;
    private int curMineNum = 0;             //현재 선택한 광산 번호

    private GameObject StageObj;

    private GameObject SystemPopup; //시스템 팝업
    private Button sys_yesButton;
    private Button sys_NoButton;
    private Button sys_OkButton;


    void Start ()
    {
        stageMineData = GameObject.Find("StageData").GetComponent<StageMineData>();

        mineList = stageMineData.getMineList();
        mineFlag = new bool[mineList.Count];

        MineButtonObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/MineButton").gameObject;
        BeUnderPopup = GameObject.Find("System").transform.Find("BeUnderPopup").gameObject;
        MiningPopup = GameObject.Find("System").transform.Find("MiningPopup").gameObject;
        ExhaustionPopup = GameObject.Find("System").transform.Find("ExhaustionPopup").gameObject;

        StageObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject;

        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        sys_yesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();
        sys_NoButton = SystemPopup.transform.Find("UIPanel/NoButton").gameObject.GetComponent<Button>();
        sys_OkButton = SystemPopup.transform.Find("UIPanel/OKButton").gameObject.GetComponent<Button>();


        //광산 오브젝트 배치
        List<Mine> mList = mineList;
        for (int i = 0; i < mList.Count; i++)
        {
            if (mList[i].spotName != null)
            {
                string spotName = mList[i].spotName;
                //스팟 인덱스
                int index = StageData.spotList.FindIndex(x => x.getSpotName() == spotName);

                GameObject spotButton = Instantiate(MineButtonObj.gameObject);
                spotButton.transform.SetParent(StageObj.gameObject.transform);
                spotButton.transform.localPosition = StageData.spotList[index].getPosition().localPosition;
                spotButton.SetActive(true);
                spotButton.transform.Find("StageText").GetComponent<Text>().text = mList[i].getMineNum().ToString();
                spotButton.name = mList[i].stageName + "Button"; //오브젝트 이름 변경
                Color clr = spotButton.transform.Find("Image").gameObject.GetComponent<Image>().color;
                spotButton.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
                spotButton.transform.Find("Text").gameObject.SetActive(false);
                spotButton.transform.Find("pickax").gameObject.SetActive(false);
                spotButton.transform.Find("TypeName").gameObject.SetActive(true);
                spotButton.transform.Find("TypeName/TypeNameText").gameObject.GetComponent<Text>().text = "Lv"+ mList[i].level+" "+ mList[i].type + " 광산";
                spotButton.transform.Find("BoostIcon").gameObject.SetActive(false);
                int num = i;
                spotButton.GetComponent<Button>().onClick.AddListener(() => ExhaustionCondition(spotButton, num));
            }
        }

    }

    void Update ()
    {
        //광산
        for (int i = 0; i < mineList.Count; i++)
        {
            GameObject mineObj = StageObj.transform.Find(mineList[i].stageName + "Button").gameObject;
            //건설 중
            if (mineList[i].buildState == "beunder"
                || mineList[i].buildState == "upgrade")
            {
                mineObj.transform.Find("Dust").gameObject.SetActive(true);
                //시간 0이면 건설완료 상태로
                if (mineList[i].buildTime < 0.5f)
                {
                    //mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text = "건설 완료";
                    //건설 완료 알림
                    if (mineFlag[i])
                    {
                        GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(mineList[i].type + " 광산 건설을 완료했습니다.");
                        mineFlag[i] = false;
                    }
                    if (BeUnderPopup.activeInHierarchy) BeUnderPopup.SetActive(false);
                }
                //시간 빼기
                else
                {
                    float time = mineList[i].buildTime;
                    mineObj.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(1, 1, 1);
                    if (time > 3600)
                    {
                        mineObj.transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)(time / 3600)).ToString() + "시간 " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)(time / 3600)).ToString() + "시간 " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                    }
                    else if (time > 60)
                    {
                        mineObj.transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)((time % 3600) / 60)).ToString() + "분 " + ((int)(time % 60)).ToString() + "초";
                    }
                    else
                    {
                        mineObj.transform.Find("Text").gameObject.GetComponent<Text>().text =
                           "남은 시간 : " + ((int)(time % 60)).ToString() + "초";
                        BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text =
                            "남은 시간 : " + ((int)(time % 60)).ToString() + "초";
                    }
                }
            }

            //채굴 상태
            if (mineList[i].buildState == "complete" && mineList[i].miningState)
            {
                mineFlag[i] = true;
                //Debug.Log("채굴상태");
                Color clr = mineObj.transform.Find("Image").gameObject.GetComponent<Image>().color;
                mineObj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
                mineObj.transform.Find("Image").gameObject.SetActive(true);
                mineObj.transform.Find("Text").gameObject.SetActive(true);
                mineObj.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(0.41f, 0.85f, 0.4f);
                mineObj.transform.Find("DottedCircle").gameObject.SetActive(false);
                mineObj.transform.Find("pickax").gameObject.SetActive(true);
                mineObj.transform.Find("Dust").gameObject.SetActive(false);

                mineObj.GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                mineObj.GetComponent<Button>().onClick.AddListener(() => Mining(mineObj, num));

                //획득
                mineObj.transform.Find("Text").gameObject.GetComponent<Text>().text = mineList[i].getAmount.ToString() + " / " + mineList[i].deposit.ToString();

                if (MiningPopup.activeInHierarchy)
                {
                    MiningPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = mineList[curMineNum].type + " 광산 채굴 중";
                    MiningPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = "레벨 : " + mineList[curMineNum].level.ToString();
                    MiningPopup.transform.Find("UIPanel/InfoBox/AmountText").gameObject.GetComponent<Text>().text = "채굴량 : " + mineList[curMineNum].getAmount.ToString() + " / " + mineList[curMineNum].deposit.ToString();
                    MiningPopup.transform.Find("UIPanel/InfoBox/MiningTimeText").gameObject.GetComponent<Text>().text = "채굴 주기 : " + mineList[curMineNum].miningTime.ToString();
                }

            }

            //매장량 다 채우면 pickax없애고 텍스트 바꾸기
            if (mineList[i].buildState == "complete" && !mineList[i].miningState)
            {
                mineObj.transform.Find("Text").gameObject.SetActive(true);
                mineObj.transform.Find("Text").gameObject.GetComponent<Text>().text = mineList[i].deposit.ToString() + "개 채굴 완료";
                mineObj.transform.Find("Text").gameObject.GetComponent<Text>().color = new Color(1f, 0.2f, 0.21f);
                mineObj.transform.Find("pickax").gameObject.SetActive(false);
                mineObj.transform.Find("Dust").gameObject.SetActive(false);
                MiningPopup.SetActive(false);

                //채굴 완료 알림
                if (mineFlag[i])
                {
                    GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(mineList[i].type + " 광산 채굴이 끝났습니다.");
                    mineFlag[i] = false;
                }

                mineObj.GetComponent<Button>().onClick.RemoveAllListeners();
                int num = i;
                mineObj.GetComponent<Button>().onClick.AddListener(() => getOre(mineObj, num));
            }
        }

    }


    //광산


    //광산 고갈 상태 팝업 //레벨업과 채광 용도
    public void ExhaustionCondition(GameObject obj, int num)
    {
        MineInfo info = MineData.instance.getMineInfoList().Find(x => x.type == mineList[num].type);

        ExhaustionPopup.SetActive(true);
        ExhaustionPopup.transform.Find("UIPanel/CancleButton").gameObject.SetActive(false);
        ExhaustionPopup.transform.Find("UIPanel/MiningButton").gameObject.SetActive(true);

        info.afterLevel = info.level + 1;
        info.afterTime = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).time;
        info.afterDeposit = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).deposit;
        info.curMaterial = MineData.instance.getMineBuildList().Find(x => x.level == info.level).material;

        ExhaustionPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = info.type + " 광산";
        ExhaustionPopup.transform.Find("UIPanel/Frame/Ore").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == mineList[num].type).icon);
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
        destroyItemBox(ExhaustionPopup.transform.Find("UIPanel/ItemList").gameObject);
        for (int i = 0; i < info.getThingName.Length; i++)
        {
            if (info.getThingName.Length == 1)
            {
                mtr = info.getThingName[0];
                GameObject box = Instantiate(ExhaustionPopup.transform.Find("UIPanel/ItemList/ItemBox").gameObject);
                box.transform.SetParent(ExhaustionPopup.transform.Find("UIPanel/ItemList").gameObject.transform, false);
                box.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == info.getThingName[0]).icon);
                box.transform.Find("NameText").gameObject.GetComponent<Text>().text = info.getThingName[0];
                box.SetActive(true);

                break;
            }
            else
            {
                if (i == 0) { mtr = info.getThingName[0]; }
                else { mtr += ", " + info.getThingName[i]; }
                GameObject box = Instantiate(ExhaustionPopup.transform.Find("UIPanel/ItemList/ItemBox").gameObject);
                box.transform.SetParent(ExhaustionPopup.transform.Find("UIPanel/ItemList").gameObject.transform, false);
                box.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == info.getThingName[i]).icon);
                box.transform.Find("NameText").gameObject.GetComponent<Text>().text = info.getThingName[i];
                box.SetActive(true);

            }
        }
        //ExhaustionPopup.transform.Find("UIPanel/InfoBox/GetItemText").gameObject.GetComponent<Text>().text =  mtr;
        ExhaustionPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text = mineList[num].type + " " + info.curMaterial + "개";
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
                //팝업
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

            mineList[num].type = info.type;
            mineList[num].level = info.afterLevel;
            mineList[num].buildState = "upgrade";
            info.upgradeFlag = false;
            mineFlag[num] = true;

            mineList[num].buildTime = MineData.instance.getMineBuildList().Find(x => x.level == info.level).time;
            for (int i = 0; i < info.getThingName.Length; i++)
            {
                mineList[num].getThingName[i] = info.getThingName[i];
            }
            mineList[num].getOnceAmount = 1;
            mineList[num].deposit = MineData.instance.getMineBuildList().Find(x => x.level == info.afterLevel).deposit;
            mineList[num].miningTime = 1f;

            //재료 소모
            thing.possession -= info.curMaterial;
            //획득한 아이템 개수 0으로 초기화
            for (int i = 0; i < mineList[num].getThingNum.Length; i++)
                mineList[num].getThingNum[i] = 0;

            obj.GetComponent<Button>().onClick.RemoveAllListeners();
            obj.GetComponent<Button>().onClick.AddListener(() => BuildCondition(obj, num));

            obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            obj.transform.Find("Image").gameObject.SetActive(true);
            obj.transform.Find("Text").gameObject.SetActive(true);
            obj.transform.Find("pickax").gameObject.SetActive(false);
            obj.transform.Find("Dust").gameObject.SetActive(true);
            obj.transform.Find("TypeName").gameObject.SetActive(true);
            obj.transform.Find("TypeName/TypeNameText").gameObject.GetComponent<Text>().text = "Lv" + mineList[num].level + " " + mineList[num].type + " 광산";

            for (int i = 0; i < mineList[num].getThingNum.Length; i++)
                Debug.Log(mineList[num].getThingNum[i]);
            //BlackBack.SetActive(false);
            //하단메뉴 잠금
            //BottomMenuLock.SetActive(true);
            //대장장이 행동 제한
            //StartLock.SetActive(true);

            ExhaustionPopup.SetActive(false);
            BeUnderPopup.SetActive(false);
        });

        //채굴
        ExhaustionPopup.transform.Find("UIPanel/MiningButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        ExhaustionPopup.transform.Find("UIPanel/MiningButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            for (int i = 0; i < info.getThingName.Length; i++)
            {
                mineList[num].getThingName[i] = info.getThingName[i];
            }
            mineList[num].getOnceAmount = 1;
            mineList[num].deposit = MineData.instance.getMineBuildList().Find(x => x.level == info.level).deposit;
            mineList[num].miningTime = 1f;

            mineList[num].buildTime = 0f;
            mineList[num].buildState = "complete";
            mineList[num].miningState = true;
            obj.transform.Find("Text").gameObject.GetComponent<Text>().text = "건설 완료";
            Color clr = obj.transform.Find("Image").gameObject.GetComponent<Image>().color;
            obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
            obj.transform.Find("Image").gameObject.SetActive(true);
            obj.transform.Find("Text").gameObject.SetActive(true);
            obj.transform.Find("DottedCircle").gameObject.SetActive(false);
            obj.transform.Find("pickax").gameObject.SetActive(true);
            obj.transform.Find("Dust").gameObject.SetActive(false);
            if (BeUnderPopup.activeInHierarchy)
                BeUnderPopup.SetActive(false);
            if (mineList[num].boostState == true)
                obj.transform.Find("BoostIcon").gameObject.SetActive(true);
            ExhaustionPopup.SetActive(false);

        });

    }

    //건설 진행 중 버튼 (현황 창
    public void BuildCondition(GameObject obj, int num)
    {
        string type = mineList[num].type;

        BeUnderPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = type + " 광산 업그레이드 중";
        BeUnderPopup.transform.Find("UIPanel/Frame/Ore").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == mineList[num].type).icon);
        BeUnderPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text =
            "레벨 : " + (mineList[num].level).ToString();
        BeUnderPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text
            = "매장량 : " + MineData.instance.getMineBuildList().Find(x => x.level == mineList[num].level).deposit.ToString();
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
                MineData.instance.getMineInfoList().Find(x => x.type == type).level++;
                mineList[num].level = MineData.instance.getMineInfoList().Find(x => x.type == type).level;
                MineData.instance.getMineInfoList().Find(x => x.type == type).buildTime
                    = MineData.instance.getMineBuildList().Find(x => x.level == mineList[num].level).time;
                mineList[num].buildTime = MineData.instance.getMineInfoList().Find(x => x.type == type).buildTime;
                mineList[num].deposit
                    = MineData.instance.getMineBuildList().Find(x => x.level == mineList[num].level).deposit;

                mineList[num].getAmount = 0;
                mineList[num].buildTime = 0f;
                mineList[num].buildState = "complete";
                mineList[num].miningState = true;
                obj.transform.Find("Text").gameObject.GetComponent<Text>().text = "건설 완료";
                Color clr = obj.transform.Find("Image").gameObject.GetComponent<Image>().color;
                obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
                obj.transform.Find("Image").gameObject.SetActive(true);
                obj.transform.Find("Text").gameObject.SetActive(true);
                obj.transform.Find("DottedCircle").gameObject.SetActive(false);
                obj.transform.Find("pickax").gameObject.SetActive(true);
                obj.transform.Find("Dust").gameObject.SetActive(false);

                //BottomMenuLock.SetActive(false);
                //StartLock.SetActive(false);
                if (BeUnderPopup.activeInHierarchy)
                    BeUnderPopup.SetActive(false);
                Debug.Log("업글 즉완");

            });

        });


        //건설 취소 버튼->SystemPopup
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
                mineList[num].buildTime = 0f;
                mineList[num].buildState = "nothing";
                mineList[num].level = MineData.instance.getMineInfoList().Find(x => x.type == mineList[num].type).afterLevel - 1;
                Color clr = obj.transform.Find("Image").gameObject.GetComponent<Image>().color;
                obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
                obj.transform.Find("Image").gameObject.SetActive(true);
                obj.transform.Find("Text").gameObject.SetActive(false);
                obj.transform.Find("DottedCircle").gameObject.SetActive(false);
                obj.transform.Find("pickax").gameObject.SetActive(false);
                obj.transform.Find("Dust").gameObject.SetActive(false);
                obj.GetComponent<Button>().onClick.AddListener(() => ExhaustionCondition(obj, num));
                //BottomMenuLock.SetActive(false);
                //StartLock.SetActive(false);
                if (BeUnderPopup.activeInHierarchy) BeUnderPopup.SetActive(false);
            });
        });
    }

    //채굴 중
    public void Mining(GameObject obj, int num)
    {
        Debug.Log("mining");
        MiningPopup.SetActive(true);
        MiningPopup.transform.Find("UIPanel/Frame/Ore").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == mineList[num].type).icon);
        MiningPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = mineList[num].type + " 광산 채굴 중";
        MiningPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = "레벨 : " + mineList[num].level.ToString();

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

                mineList[num].miningState = false;

                //부가 아이템
                int rest = mineList[num].deposit - mineList[num].getAmount;
                for (int i = 0; i < rest; i++)
                {
                    for (int j = 1; j < mineList[num].getThingName.Length; j++)
                    {
                        if (mineList[num].getThingName[j] != null)
                        {
                            int random = UnityEngine.Random.Range(1, 100 + 1);      //100확률
                                                                                    //Debug.Log(random);
                            int prob = MineData.instance.getMineInfoList().Find(x => x.type == mineList[num].type).getThingProb[j];  //아이템 확률
                            if (random <= prob)
                            {
                                //Debug.Log(mineList[num].getThingName[j] + " 획득");
                                mineList[num].getThingNum[j]++;
                            }
                        }
                    }
                }


                obj.transform.Find("Text").gameObject.GetComponent<Text>().text = mineList[num].deposit.ToString() + "개 채굴 완료";
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
                mineList[num].miningTime /= 2;
                mineList[num].boostState = true;

                obj.transform.Find("BoostIcon").gameObject.SetActive(true);
            });
        });
        //폐광
        MiningPopup.transform.Find("UIPanel/CancleButton/Text").gameObject.GetComponent<Text>().text = "중지";
        MiningPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        MiningPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "채굴 중지";
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "채굴을 중지하겠습니까?";
            sys_yesButton.gameObject.SetActive(true);
            sys_NoButton.gameObject.SetActive(true);
            sys_OkButton.gameObject.SetActive(false);
            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {
                SystemPopup.SetActive(false);
                MiningPopup.SetActive(false);

                mineList[num].buildTime = 0f;
                mineList[num].buildState = "nothing";
                mineList[num].miningState = false;
                mineList[num].boostState = false;
                //획득한 아이템 개수 0으로 초기화
                for (int i = 0; i < mineList[num].getThingNum.Length; i++)
                    mineList[num].getThingNum[i] = 0;

                obj.transform.Find("Image").gameObject.SetActive(true);
                obj.transform.Find("Text").gameObject.SetActive(false);
                obj.transform.Find("pickax").gameObject.SetActive(false);
                obj.transform.Find("Dust").gameObject.SetActive(false);
                obj.transform.Find("TypeName").gameObject.SetActive(true);
                obj.transform.Find("BoostIcon").gameObject.SetActive(false);
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
                obj.GetComponent<Button>().onClick.AddListener(() => ExhaustionCondition(obj, num));
            });

        });
    }

    //광석 획득
    public void getOre(GameObject obj, int num)
    {
        Debug.Log("완료");

        Player.instance.getExp(10);

        mineList[num].buildState = "nothing";
        mineList[num].getAmount = 0;
        mineList[num].curTime = 0f;
        mineList[num].boostState = false;

        obj.transform.Find("Text").gameObject.GetComponent<Text>().text = "고갈";

        //기본 광석 획득
        if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineList[num].getThingName[0]) != null)
        {
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineList[num].getThingName[0]).possession
                += mineList[num].deposit;
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineList[num].getThingName[0]).recent = true;
        }
        else
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                x => x.name == mineList[num].getThingName[0]).type, mineList[num].getThingName[0], mineList[num].deposit));
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineList[num].getThingName[0]).recent = true;
        }

        //부가 아이템 획득
        for (int i = 1; i < mineList[num].getThingName.Length; i++)
        {
            if (mineList[num].getThingName[i] != null)
            {
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineList[num].getThingName[i]) != null)
                {
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineList[num].getThingName[i]).possession
                        += mineList[num].getThingNum[i];
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineList[num].getThingName[i]).recent = true;

                }
                else
                {
                    ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                        x => x.name == mineList[num].getThingName[i]).type, mineList[num].getThingName[i], mineList[num].getThingNum[i]));
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == mineList[num].getThingName[i]).recent = true;
                }
            }
        }

        int[] itemnum = new int[3];
        itemnum[0] = mineList[num].deposit;
        itemnum[1] = mineList[num].getThingNum[1];
        itemnum[2] = mineList[num].getThingNum[2];
        GameObject.Find("InventoryScript").GetComponent<GetItemManager>().getItem(mineList[num].getThingName, itemnum);

        //획득한 아이템 개수 0으로 초기화
        for (int i = 0; i < mineList[num].getThingNum.Length; i++)
            mineList[num].getThingNum[i] = 0;

        //다른 위치로 배치
        StageData.spotList[StageData.spotList.FindIndex(x => x.stageNum == mineList[num].getMineNum())].stageActive = false;
        int random = 0;
        int index = 0;
        //범위 상관없이 배치
        while (true)
        {
            random = Random.Range(1, 100 + 1);
            index = StageData.spotList.FindIndex(x => x.getPosition().name == "spot" + random.ToString());
            if (!StageData.spotList[index].stageActive) break;
        }
        StageData.spotList[index].stageActive = true;
        Debug.Log(index);

        StageData.spotList[index].stageNum = mineList[num].getMineNum();    //스테이지 번호 저장
        mineList[num].spotName = StageData.spotList[index].getPosition().name;

        string spotName = mineList[num].spotName;
        //스팟 인덱스
        index = StageData.spotList.FindIndex(x => x.getSpotName() == spotName);

        obj.transform.localPosition = StageData.spotList[index].getPosition().localPosition;
        Color clr = obj.transform.Find("Image").gameObject.GetComponent<Image>().color;
        obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1f);
        obj.transform.Find("Text").gameObject.SetActive(false);
        obj.transform.Find("pickax").gameObject.SetActive(false);
        obj.transform.Find("TypeName").gameObject.SetActive(true);
        obj.transform.Find("TypeName/TypeNameText").gameObject.GetComponent<Text>().text = "Lv" + mineList[num].level + " " + mineList[num].type + " 광산";
        obj.transform.Find("BoostIcon").gameObject.SetActive(false);
        int num2 = num;
        obj.GetComponent<Button>().onClick.RemoveAllListeners();
        obj.GetComponent<Button>().onClick.AddListener(() => ExhaustionCondition(obj, num2));

    }

    public void MineClick(GameObject obj) { curMineNum = mineList.FindIndex(x => x.stageName + "Button" == obj.name); Debug.Log(curMineNum); }


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


}
