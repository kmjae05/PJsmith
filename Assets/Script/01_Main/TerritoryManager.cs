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

    private GameObject BlackBack;
    private Button CancleButton;
    private GameObject BottomMenuLock;

    private GameObject SystemPopup;
    private Button sys_yesButton;


    private List<GameObject> mineObj;           //광산 스팟
    private List<GameObject> bottonButtonList;  //하단 버튼

    private List<MineInfo> mineInfo;

    private string curType = null;                 //현재 선택된 광산 종류
    private int level = 1;              //건설할 때 레벨


    void Start()
    {
        uiPanel = GameObject.Find("Menu").transform.Find("TerritoryPopup/UIPanel").gameObject;
        buildInfoPopup = GameObject.Find("System").transform.Find("BuildInfoPopup").gameObject;
        BeUnderPopup = GameObject.Find("System").transform.Find("BeUnderPopup").gameObject;
        BlackBack = uiPanel.transform.Find("BlackBack").gameObject;
        CancleButton = BlackBack.transform.Find("CancleButton").gameObject.GetComponent<Button>();
        BottomMenuLock = uiPanel.transform.Find("BottomMenu/BottomMenuLock").gameObject;
        SystemPopup = GameObject.Find("System").transform.Find("SystemPopup").gameObject;
        sys_yesButton = SystemPopup.transform.Find("UIPanel/YesButton").gameObject.GetComponent<Button>();


        mineObj = new List<GameObject>();
        bottonButtonList = new List<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            mineObj.Add(uiPanel.transform.Find("Mine/Spot" + (i + 1).ToString()).gameObject);
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
        }
    }

    private void Update()
    {
        for (int i = 0; i < MineData.instance.getMineList().Count; i++)
        {
            //건설 중
            if (MineData.instance.getMineList()[i].buildState == "beunder")
            {
                //시간 0이면 건설완료 상태로
                if (MineData.instance.getMineList()[i].buildTime < 0)
                {
                    MineData.instance.getMineList()[i].buildState = "complete";
                    //MineData.instance.getMineList()[i].miningState = true;
                    mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text = "건설 완료";
                    BottomMenuLock.SetActive(false);
                    if (BeUnderPopup.activeInHierarchy) BeUnderPopup.SetActive(false);

                    mineObj[i].GetComponent<Button>().onClick.RemoveAllListeners();
                }
                //시간 빼기
                else
                {
                    MineData.instance.getMineList()[i].buildTime -= Time.deltaTime;
                    mineObj[i].transform.Find("Text").gameObject.GetComponent<Text>().text = ((int)MineData.instance.getMineList()[i].buildTime).ToString();
                    BeUnderPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = ((int)MineData.instance.getMineList()[i].buildTime).ToString();
                }

            }
            //건설 완료 & 채굴 상태
            if (MineData.instance.getMineList()[i].buildState == "complete" && MineData.instance.getMineList()[i].miningState)
            {
                //획득
                //매장량 다 채우면 pickax없애고 텍스트 바꾸기
                
            }
        }
    }
    
    //버튼 세팅
    private void BottomButtonSetting(int index)
    {
        buildInfoPopup.SetActive(true);
        buildInfoPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = mineInfo[index].type + " 광산 건설";
        buildInfoPopup.transform.Find("UIPanel/InfoBox/LevelText").gameObject.GetComponent<Text>().text = "레벨 : " + level.ToString();
        buildInfoPopup.transform.Find("UIPanel/InfoBox/TimeText").gameObject.GetComponent<Text>().text = "소요 시간 : " + mineInfo[index].buildTime + "분";
        buildInfoPopup.transform.Find("UIPanel/InfoBox/DepositText").gameObject.GetComponent<Text>().text = "매장량 : 100";
        buildInfoPopup.transform.Find("UIPanel/InfoBox/MaterialText").gameObject.GetComponent<Text>().text = "필요 재료 : "
            + mineInfo[index].necessaryMaterials[0] + " " + mineInfo[index].necessaryMaterialsNum[0];
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
        //재료 소모
        MineInfo info = mineInfo.Find(x => x.type == curType);
        for (int i = 0; i < info.necessaryMaterials.Length; i++)
        {
            Things thing = ThingsData.instance.getThingsList().Find(x => x.name == info.necessaryMaterials[i]);
            thing.possession -= info.necessaryMaterialsNum[i];
            Debug.Log(thing.possession);
        }

        obj.GetComponent<Button>().onClick.RemoveAllListeners();
        obj.GetComponent<Button>().onClick.AddListener(() => BuildCondition(obj, num));

        Color clr = obj.transform.Find("Image").gameObject.GetComponent<Image>().color;
        obj.transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 0.5f);
        obj.transform.Find("Image").gameObject.SetActive(true);
        obj.transform.Find("Text").gameObject.SetActive(true);
        obj.transform.Find("DottedCircle").gameObject.SetActive(false);
        obj.transform.Find("pickax").gameObject.SetActive(false);
        //광산에 정보 저장
        MineData.instance.getMineList()[num].buildState = "beunder";
        MineData.instance.getMineList()[num].type = info.type;
        MineData.instance.getMineList()[num].buildTime = info.buildTime;

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

    }

    //건설 진행 중 버튼 (현황 창
    public void BuildCondition(GameObject obj, int num)
    {
        string type = MineData.instance.getMineList()[num].type;
        BeUnderPopup.transform.Find("UIPanel/BackBox/TitleText").gameObject.GetComponent<Text>().text = type + " 광산 건설 중";
        //팝업 올리기
        BeUnderPopup.SetActive(true);

        //즉시 완료 버튼
        BeUnderPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        BeUnderPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);

            SystemPopup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "즉시 완료";
            SystemPopup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "보석 50개를 사용하여 즉시 완료하시겠습니까?";
            sys_yesButton.GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
            sys_yesButton.GetComponent<Button>().onClick.AddListener(() => {

            });

        });


        //건설 취소 버튼->systempopup
        BeUnderPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        BeUnderPopup.transform.Find("UIPanel/CancleButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            SystemPopup.SetActive(true);


        });


    }

    //채굴




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
            }
            //건설 완료
            if (MineData.instance.getMineList()[i].buildState == "complete")
            {
                mineObj[i].transform.Find("Image").gameObject.SetActive(true);
                Color clr = mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color;
                mineObj[i].transform.Find("Image").gameObject.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1);
                mineObj[i].transform.Find("Text").gameObject.SetActive(true);               //획득량은 update에서
                mineObj[i].transform.Find("DottedCircle").gameObject.SetActive(false);

                //채굴 상태
                if (MineData.instance.getMineList()[i].miningState)
                    mineObj[i].transform.Find("pickax").gameObject.SetActive(true);
                else mineObj[i].transform.Find("pickax").gameObject.SetActive(false);

            }
        }
    }


}
