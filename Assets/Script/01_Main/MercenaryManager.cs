using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MercenaryManager : MonoBehaviour {

    //용병
    private MercenaryData mercenaryData;
    static private List<Mercenary> mercenary;
    private Mercenary mercenarytmp;

    private string curSelect;   //현재 선택된 용병

    //스테이지 정보
    private StageManager stageManager;
    //스테이지 현황 팝업창
    private GameObject stageStatePopup;

    private ProfilePopupManager profilePopupManager;

    private void Start()
    {
        mercenaryData = GameObject.Find("MercenaryData").GetComponent<MercenaryData>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageStatePopup = GameObject.Find("System").transform.Find("StageStatePopup").gameObject;
        profilePopupManager = GameObject.Find("PlayerData").GetComponent<ProfilePopupManager>();

        mercenary = new List<Mercenary>();
        mercenarytmp = new Mercenary();
    }

    private void Update()
    {
        mercenary = mercenaryData.getMercenary();
        for (int i = 0; i < mercenary.Count; i++)
            mercenary[i].level = Player.Play.level;
        LobbyMerActive();
        mercenaryData.setMercenary(mercenary);
    }


    //로비 캐릭터 정보창에서 선택
    public void setMerInfo(GameObject obj)
    {
        Mercenary merTemp = mercenary.Find(x => x.getName() == obj.transform.Find("NameText").GetComponent<Text>().text);

        //장비 이미지, 텍스트 변경
        GameObject.Find("EquipHelmet/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Equipment/Helmet/" + merTemp.equipHelmet[profilePopupManager.getCurSetNum() - 1]);
        GameObject.Find("EquipHelmet/Text").GetComponent<Text>().text = merTemp.equipHelmet[profilePopupManager.getCurSetNum() - 1];
        GameObject.Find("EquipArmor/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Equipment/Armor/" + merTemp.equipArmor[profilePopupManager.getCurSetNum() - 1]);
        GameObject.Find("EquipArmor/Text").GetComponent<Text>().text = merTemp.equipArmor[profilePopupManager.getCurSetNum() - 1];
        GameObject.Find("EquipWeapon/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Equipment/Weapon/" + merTemp.equipWeapon[profilePopupManager.getCurSetNum() - 1]);
        GameObject.Find("EquipWeapon/Text").GetComponent<Text>().text = merTemp.equipWeapon[profilePopupManager.getCurSetNum() - 1];
        GameObject.Find("EquipBoots/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Equipment/Boots/" + merTemp.equipBoots[profilePopupManager.getCurSetNum() - 1]);
        GameObject.Find("EquipBoots/Text").GetComponent<Text>().text = merTemp.equipBoots[profilePopupManager.getCurSetNum() - 1];

        //*****캐릭터 3D모델 변경
        GameObject.Find("01_3D").transform.Find("Chr/Chr_Profile").gameObject.SetActive(false);

        //*****캐릭터 이미지 변경
        GameObject.Find("LevelText").GetComponent<Text>().text = merTemp.level.ToString();
        GameObject.Find("PlayerNameText").GetComponent<Text>().text = merTemp.getName();

        statusCal(profilePopupManager.getCurSetNum(), merTemp);

        GameObject.Find("DPS/Text").GetComponent<Text>().text = mercenarytmp.dps.ToString();
        GameObject.Find("StrPower/Text").GetComponent<Text>().text = mercenarytmp.strPower.ToString();
        GameObject.Find("AttackSpeed/Text").GetComponent<Text>().text = mercenarytmp.attackSpeed.ToString();
        GameObject.Find("Focus/Text").GetComponent<Text>().text = mercenarytmp.focus.ToString();
        GameObject.Find("Critical/Text").GetComponent<Text>().text = mercenarytmp.critical.ToString();
        GameObject.Find("DefPower/Text").GetComponent<Text>().text = mercenarytmp.defPower.ToString();
        GameObject.Find("EvaRate/Text").GetComponent<Text>().text = mercenarytmp.evaRate.ToString();
        GameObject.Find("Attribute/Text").GetComponent<Text>().text = mercenarytmp.attribute;
        GameObject.Find("CollectSpeed/Text").GetComponent<Text>().text = mercenarytmp.collectSpeed.ToString();
        GameObject.Find("CollectAmount/Text").GetComponent<Text>().text = mercenarytmp.collectAmount.ToString();

        

        GameObject.Find("ChrTitleText").GetComponent<Text>().text = "";
    }


    //로비에 있는 용병 버튼 밑 text
    private void LobbyMerActive()
    {
        for (int i = 0; i < mercenary.Count; i++)
        {
            //버튼 active 확인
            if (GameObject.Find("Mercenary" + mercenary[i].getName() + "Button"))
            {
                GameObject button = GameObject.Find("Mercenary" + mercenary[i].getName() + "Button");

                //용병 탐험 중
                if (mercenary[i].getState())
                {
                    button.transform.Find("State").gameObject.SetActive(true);  //남은 시간
                    StageInfo stageInfo = stageManager.getStageInfoList().Find(x => x.mercenaryName == mercenary[i].getName());
                    if(stageInfo != null)
                    {
                        //탐험 중
                        if (!stageInfo.complete)
                        {
                            float time = stageInfo.time;
                            button.transform.Find("State/Text").gameObject.GetComponent<Text>().text =
                                ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
                        }
                        //완료
                        else button.transform.Find("State/Text").gameObject.GetComponent<Text>().text = "완료";
                    }
                }
                //대기 중
                else
                {
                    button.transform.Find("State").gameObject.SetActive(false);
                }
            }
        }

    }

    //로비에 있는 용병 버튼
    public void LobbyMerButton(Text nameText)
    {
        Mercenary merTemp = mercenary.Find(x => x.getName() == nameText.text);
        curSelect = merTemp.getName();
        
        //용병이 탐험 상태일 때
        if (merTemp.getState())
        {
            stageStatePopup.SetActive(true);

            StageInfo stageInfo = stageManager.getStageInfoList().Find(x => x.mercenaryName == merTemp.getName());
            stageManager.SetCurStageSelect(stageInfo.getStageNum());
            GameObject.Find("StageStateText").GetComponent<Text>().text = stageInfo.type + " " + stageInfo.typeNum.ToString();
            stageStatePopup.transform.Find("StageStatePanel/MercenaryBox/Mercenary" + nameText.text).gameObject.SetActive(true);


            //완료 안 된 상태 stage state = true, stage complete =  false
            if (stageInfo.state && !stageInfo.complete)
            {
                GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(true);
                GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(false);
            }
            //완료된 상태 stage state = false, stage complete =  true
            if (!stageInfo.state && stageInfo.complete)
            {
                //Debug.Log("용병 선택 " + merTemp.getContName() + " " + merTemp.getStageNum());
                GameObject.Find("StageStatePanel").transform.Find("ImdCompleteButton").gameObject.SetActive(false);
                GameObject.Find("StageStatePanel").transform.Find("CompleteButton").gameObject.SetActive(true);
            }

        }





    }


    //스테이지 화면에서 용병 선택
    public void openMerInfoPopup(GameObject obj)
    {
        GameObject merInfoPopup = GameObject.Find("System").transform.Find("MercenaryInfoPopup").gameObject;
        Mercenary merTemp = mercenary.Find(x => x.getName() == obj.transform.Find("NameText").GetComponent<Text>().text);

        //*****캐릭터 이미지 변경

        statusCal(profilePopupManager.getSetNum(), merTemp);
        merInfoPopup.transform.Find("UIPanel/ChrBox/Text").GetComponent<Text>().text = merTemp.getName();

        merInfoPopup.transform.Find("UIPanel/InfoPanel/DPS/Text").GetComponent<Text>().text = mercenarytmp.dps.ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/StrPower/Text").GetComponent<Text>().text = mercenarytmp.strPower.ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/AttackSpeed/Text").GetComponent<Text>().text = mercenarytmp.attackSpeed.ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/Focus/Text").GetComponent<Text>().text = mercenarytmp.focus.ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/Critical/Text").GetComponent<Text>().text = mercenarytmp.critical.ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/DefPower/Text").GetComponent<Text>().text = mercenarytmp.defPower.ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/EvaRate/Text").GetComponent<Text>().text = mercenarytmp.evaRate.ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/Attribute/Text").GetComponent<Text>().text = mercenarytmp.attribute;
        merInfoPopup.transform.Find("UIPanel/InfoPanel/CollectSpeed/Text").GetComponent<Text>().text = mercenarytmp.collectSpeed.ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/CollectAmount/Text").GetComponent<Text>().text = mercenarytmp.collectAmount.ToString();

        merInfoPopup.SetActive(true);
    }



    //스탯 계산
    public void statusCal(int setNum, Mercenary mer)
    {
        if (setNum == 1)
        {
            mercenarytmp.dps = mer.dps*2;
            mercenarytmp.strPower = mer.strPower * 1.5f;
            mercenarytmp.attackSpeed = mer.attackSpeed + 0.3f;
            mercenarytmp.focus = mer.focus + 20;
            mercenarytmp.critical = mer.critical * 1.4f;
            mercenarytmp.defPower = mer.defPower * 1.3f;
            mercenarytmp.evaRate = mer.evaRate * 1.1f;
            mercenarytmp.attribute = "no";
            mercenarytmp.collectSpeed = mer.collectSpeed* 0.7f;
            mercenarytmp.collectAmount = mer.collectAmount * 0.8f;
        }
        else
        {
            mercenarytmp.dps = mer.dps * 1;
            mercenarytmp.strPower = mer.strPower * 1.1f;
            mercenarytmp.attackSpeed = mer.attackSpeed + 0.1f;
            mercenarytmp.focus = mer.focus + 10;
            mercenarytmp.critical = mer.critical * 1.0f;
            mercenarytmp.defPower = mer.defPower * 1.1f;
            mercenarytmp.evaRate = mer.evaRate * 1.2f;
            mercenarytmp.attribute = "no";
            mercenarytmp.collectSpeed = mer.collectSpeed * 1.6f;
            mercenarytmp.collectAmount = mer.collectAmount * 1.5f;
        }

    }



    //용병 data
    public void setMercenary(List<Mercenary> mer) { mercenary = mer; }
    public void setMercenaryIndex(int i, Mercenary m) { mercenary[i] = m; }
    public List<Mercenary> getMercenary() { return mercenary; }
    

    public void setCurSelect(string s) { curSelect = s; }
    public string getCurSelect() { return curSelect; }

}




