using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryManager : MonoBehaviour
{

    private GameObject uiPanel;
    private GameObject buildInfoPopup;


    private List<GameObject> mineObj;           //광산 스팟
    private List<GameObject> bottonButtonList;  //하단 버튼

    private List<MineInfo> mineInfo;

    private string curType = null;                 //현재 선택된 광산 종류
    private int level = 1;              //건설할 때 레벨


    void Start()
    {
        uiPanel = GameObject.Find("Menu").transform.Find("TerritoryPopup/UIPanel").gameObject;
        buildInfoPopup = GameObject.Find("System").transform.Find("BuildInfoPopup").gameObject;


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
                //시간 빼기
                //시간 0이면 건설완료 상태로

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
        //재료 체크


        //미건설 스팟 띄우기

    }

    //스팟 선택




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
