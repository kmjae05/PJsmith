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
        //mercenaryData.setMercenary(mercenary);
    }


    //로비 캐릭터 정보창에서 선택
    public void setMerInfo(GameObject obj)
    {
        Mercenary merTemp = mercenary.Find(x => x.getName() == obj.transform.Find("NameText").GetComponent<Text>().text);
        //장비 이미지, 텍스트 변경
        GameObject.Find("EquipHelmet/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipHelmet[profilePopupManager.getCurSetNum() - 1].name).icon);
        GameObject.Find("EquipHelmet/Text").GetComponent<Text>().text = 
            merTemp.equipHelmet[profilePopupManager.getCurSetNum() - 1].name;

        GameObject.Find("EquipArmor/Image").GetComponent<Image>().sprite = 
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipArmor[profilePopupManager.getCurSetNum() - 1].name).icon);
        GameObject.Find("EquipArmor/Text").GetComponent<Text>().text = 
            merTemp.equipArmor[profilePopupManager.getCurSetNum() - 1].name;

        GameObject.Find("EquipWeapon/Image").GetComponent<Image>().sprite = 
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipWeapon[profilePopupManager.getCurSetNum() - 1].name).icon);
        GameObject.Find("EquipWeapon/Text").GetComponent<Text>().text = 
            merTemp.equipWeapon[profilePopupManager.getCurSetNum() - 1].name;

        GameObject.Find("EquipBoots/Image").GetComponent<Image>().sprite = 
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipBoots[profilePopupManager.getCurSetNum() - 1].name).icon);
        GameObject.Find("EquipBoots/Text").GetComponent<Text>().text = 
            merTemp.equipBoots[profilePopupManager.getCurSetNum() - 1].name;

        GameObject.Find("EquipGloves/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipGloves[profilePopupManager.getCurSetNum() - 1].name).icon);
        GameObject.Find("EquipGloves/Text").GetComponent<Text>().text =
            merTemp.equipGloves[profilePopupManager.getCurSetNum() - 1].name;

        GameObject.Find("EquipPants/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipPants[profilePopupManager.getCurSetNum() - 1].name).icon);
        GameObject.Find("EquipPants/Text").GetComponent<Text>().text =
            merTemp.equipPants[profilePopupManager.getCurSetNum() - 1].name;


        //등급 프레임
        GameObject.Find("EquipHelmet/GradeFrame").GetComponent<Image>().color = 
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipHelmet[profilePopupManager.getCurSetNum() - 1].name).grade);
        GameObject.Find("EquipArmor/GradeFrame").GetComponent<Image>().color = 
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipArmor[profilePopupManager.getCurSetNum() - 1].name).grade);
        GameObject.Find("EquipWeapon/GradeFrame").GetComponent<Image>().color = 
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipWeapon[profilePopupManager.getCurSetNum() - 1].name).grade);
        GameObject.Find("EquipBoots/GradeFrame").GetComponent<Image>().color = 
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipBoots[profilePopupManager.getCurSetNum() - 1].name).grade);
        GameObject.Find("EquipGloves/GradeFrame").GetComponent<Image>().color =
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipGloves[profilePopupManager.getCurSetNum() - 1].name).grade);
        GameObject.Find("EquipPants/GradeFrame").GetComponent<Image>().color =
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == merTemp.equipPants[profilePopupManager.getCurSetNum() - 1].name).grade);

        //*****캐릭터 3D모델 변경
        GameObject.Find("01_3D").transform.Find("Chr/Chr_Profile").gameObject.SetActive(false);

        //*****캐릭터 이미지 변경
        GameObject.Find("LevelText").GetComponent<Text>().text = merTemp.level.ToString();
        GameObject.Find("PlayerNameText").GetComponent<Text>().text = merTemp.getName();

        //statusCal(profilePopupManager.getCurSetNum(), merTemp);
        Stat stat = GameObject.Find("PlayerData").GetComponent<StatData>().getMercenaryStat(merTemp.getMer_no())[profilePopupManager.getCurSetNum() - 1];
        
        GameObject.Find("DPS/Text").GetComponent<Text>().text = stat.dps.ToString();
        GameObject.Find("StrPower/Text").GetComponent<Text>().text = stat.strPower.ToString();
        GameObject.Find("AttackSpeed/Text").GetComponent<Text>().text = stat.attackSpeed.ToString();
        GameObject.Find("Focus/Text").GetComponent<Text>().text = stat.focus.ToString();
        GameObject.Find("Critical/Text").GetComponent<Text>().text = stat.critical.ToString();
        GameObject.Find("DefPower/Text").GetComponent<Text>().text = stat.defPower.ToString();
        GameObject.Find("EvaRate/Text").GetComponent<Text>().text = stat.evaRate.ToString();
        GameObject.Find("Attribute/Text").GetComponent<Text>().text = merTemp.attribute;
        //GameObject.Find("CollectSpeed/Text").GetComponent<Text>().text = stat.collectSpeed.ToString();
        //GameObject.Find("CollectAmount/Text").GetComponent<Text>().text = stat.collectAmount.ToString();

        

        //GameObject.Find("ChrTitleText").GetComponent<Text>().text = "";
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

        //statusCal(profilePopupManager.getSetNum(), merTemp);
        merInfoPopup.transform.Find("UIPanel/ChrBox/Text").GetComponent<Text>().text = merTemp.getName();
        Stat stat = GameObject.Find("PlayerData").GetComponent<StatData>().getMercenaryStat(merTemp.getMer_no())[profilePopupManager.getCurSetNum() - 1];

        merInfoPopup.transform.Find("UIPanel/InfoPanel/DPS/Text").GetComponent<Text>().text = ((int)stat.dps).ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/StrPower/Text").GetComponent<Text>().text = ((int)stat.strPower).ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/AttackSpeed/Text").GetComponent<Text>().text = ((int)stat.attackSpeed).ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/Focus/Text").GetComponent<Text>().text = ((int)stat.focus).ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/Critical/Text").GetComponent<Text>().text = ((int)stat.critical).ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/DefPower/Text").GetComponent<Text>().text = ((int)stat.defPower).ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/EvaRate/Text").GetComponent<Text>().text = ((int)stat.evaRate).ToString();
        merInfoPopup.transform.Find("UIPanel/InfoPanel/Attribute/Text").GetComponent<Text>().text = merTemp.attribute;
        //merInfoPopup.transform.Find("UIPanel/InfoPanel/CollectSpeed/Text").GetComponent<Text>().text = ((int)stat.collectSpeed).ToString();
        //merInfoPopup.transform.Find("UIPanel/InfoPanel/CollectAmount/Text").GetComponent<Text>().text = ((int)stat.collectAmount).ToString();

        merInfoPopup.SetActive(true);
    }






    //용병 data
    public void setMercenary(List<Mercenary> mer) { mercenary = mer; }
    public void setMercenaryIndex(int i, Mercenary m) { mercenary[i] = m; }
    public List<Mercenary> getMercenary() { return mercenary; }
    

    public void setCurSelect(string s) { curSelect = s; }
    public string getCurSelect() { return curSelect; }

}




