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
    private GameObject stateItemList;
    private GameObject stateItemBox;

    private ProfilePopupManager profilePopupManager;

    private void Start()
    {
        mercenaryData = GameObject.Find("MercenaryData").GetComponent<MercenaryData>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageStatePopup = GameObject.Find("System").transform.Find("StageStatePopup").gameObject;
        stateItemList = stageStatePopup.transform.Find("StageStatePanel/GetItemList/Scroll/ItemList").gameObject;
        stateItemBox = stateItemList.transform.Find("ItemBox").gameObject;

        profilePopupManager = GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>();

        mercenary = new List<Mercenary>();
        mercenarytmp = new Mercenary();
    }

    private void Update()
    {
        mercenary = mercenaryData.getMercenary();
        for (int i = 0; i < mercenary.Count; i++)
            mercenary[i].level = Player.instance.getUser().level;
        LobbyMerActive();
        //mercenaryData.setMercenary(mercenary);
    }


    //로비 캐릭터 정보창에서 선택
    public void setMerInfo(GameObject obj)
    {
        Mercenary merTemp = mercenary.Find(x => x.getName() == obj.transform.Find("NameText").GetComponent<Text>().text);

        //장비 찾기
        profilePopupManager.setEquipment(merTemp.getName(), profilePopupManager.getCurSetNum());

        //장비 이미지, 텍스트 변경
        GameObject.Find("EquipHelmet/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipHelmet.name).icon);
        GameObject.Find("EquipHelmet/Text").GetComponent<Text>().text =
            profilePopupManager.equipHelmet.name;

        GameObject.Find("EquipArmor/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipArmor.name).icon);
        GameObject.Find("EquipArmor/Text").GetComponent<Text>().text =
            profilePopupManager.equipArmor.name;

        GameObject.Find("EquipWeapon/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipWeapon.name).icon);
        GameObject.Find("EquipWeapon/Text").GetComponent<Text>().text =
            profilePopupManager.equipWeapon.name;

        GameObject.Find("EquipBoots/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipBoots.name).icon);
        GameObject.Find("EquipBoots/Text").GetComponent<Text>().text =
            profilePopupManager.equipBoots.name;

        GameObject.Find("EquipGloves/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipGloves.name).icon);
        GameObject.Find("EquipGloves/Text").GetComponent<Text>().text =
            profilePopupManager.equipGloves.name;

        GameObject.Find("EquipPants/Image").GetComponent<Image>().sprite =
            Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipPants.name).icon);
        GameObject.Find("EquipPants/Text").GetComponent<Text>().text =
            profilePopupManager.equipPants.name;


        //등급 프레임
        GameObject.Find("EquipHelmet/GradeFrame").GetComponent<Image>().color =
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipHelmet.name).grade);
        GameObject.Find("EquipArmor/GradeFrame").GetComponent<Image>().color =
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipArmor.name).grade);
        GameObject.Find("EquipWeapon/GradeFrame").GetComponent<Image>().color =
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipWeapon.name).grade);
        GameObject.Find("EquipBoots/GradeFrame").GetComponent<Image>().color =
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipBoots.name).grade);
        GameObject.Find("EquipGloves/GradeFrame").GetComponent<Image>().color =
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipGloves.name).grade);
        GameObject.Find("EquipPants/GradeFrame").GetComponent<Image>().color =
            ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == profilePopupManager.equipPants.name).grade);

        //장비 인포 버튼
        GameObject.Find("EquipWeapon").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("EquipWeapon").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(profilePopupManager.equipWeapon);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                () => {
                    GameObject.Find("EquipItemInfoPopup").SetActive(false);
                    GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(profilePopupManager.equipWeapon);
                });
        });
        GameObject.Find("EquipHelmet").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("EquipHelmet").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(profilePopupManager.equipHelmet);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                () => {
                    GameObject.Find("EquipItemInfoPopup").SetActive(false);
                    GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(profilePopupManager.equipHelmet);
                });
        });
        GameObject.Find("EquipArmor").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("EquipArmor").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(profilePopupManager.equipArmor);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                () => {
                    GameObject.Find("EquipItemInfoPopup").SetActive(false);
                    GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(profilePopupManager.equipArmor);
                });
        });
        GameObject.Find("EquipGloves").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("EquipGloves").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(profilePopupManager.equipGloves);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                () => {
                    GameObject.Find("EquipItemInfoPopup").SetActive(false);
                    GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(profilePopupManager.equipGloves);
                });
        });
        GameObject.Find("EquipPants").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("EquipPants").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(profilePopupManager.equipPants);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                () => {
                    GameObject.Find("EquipItemInfoPopup").SetActive(false);
                    GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(profilePopupManager.equipPants);
                });
        });
        GameObject.Find("EquipBoots").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("EquipBoots").GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(profilePopupManager.equipBoots);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                () => {
                    GameObject.Find("EquipItemInfoPopup").SetActive(false);
                    GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(profilePopupManager.equipBoots);
                });
        });


        //*****캐릭터 3D모델 변경
        //GameObject.Find("01_3D").transform.Find("Chr/Chr_Profile").gameObject.SetActive(false);

        //*****캐릭터 이미지 변경

        GameObject.Find("LevelText").GetComponent<Text>().text = merTemp.level.ToString();
        GameObject.Find("PlayerNameText").GetComponent<Text>().text = merTemp.getName();

        Stat stat = GameObject.Find("PlayerManager").GetComponent<StatData>().getMercenaryStat(merTemp.getMer_no())[profilePopupManager.getCurSetNum() - 1];
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


    //로비에 있는 용병 버튼 밑 게이지
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
                            button.transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().maxValue = 180f;
                            button.transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().value = (180f - time);
                            button.transform.Find("State/TimeSlider/TimeText").gameObject.GetComponent<Text>().text = ( (int)((180 - time) / 180 *100)) + "%";
                            //button.transform.Find("State/Text").gameObject.GetComponent<Text>().text =
                            //    ((int)(time / 60)).ToString() + " : " + ((int)(time % 60)).ToString();
                        }
                        //완료
                        else
                        {
                            button.transform.Find("State/TimeSlider/TimeText").gameObject.GetComponent<Text>().text = "완료";
                            button.transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().value = 180f;
                        }
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

            stageManager.destroyItemBox(stateItemList);
            //얻은 아이템
            int num = stageInfo.getItem.Length;
            string[] item = new string[num];

            for (int i = 0; i < num; i++)
            {
                item[i] = stageInfo.getItem[i];
                if (item[i] != null)
                {
                    Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).grade);
                    stateItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;
                    stateItemBox.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == item[i]).icon);
                    stateItemBox.transform.Find("AmountText").gameObject.GetComponent<Text>().text = stageInfo.getItemNum[i].ToString();
                    stateItemBox.transform.Find("NameText").gameObject.GetComponent<Text>().text = item[i];

                    GameObject boxobj = Instantiate(stateItemBox);
                    boxobj.transform.SetParent(stateItemList.transform, false);
                    boxobj.name = "statePopupGetItem" + item[i];
                    boxobj.SetActive(true);
                }
            }

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

        merInfoPopup.transform.Find("UIPanel/ChrBox/ChrFrame/ChrIcon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Mercenary/" + merTemp.getName());
        merInfoPopup.transform.Find("UIPanel/ChrBox/Text").GetComponent<Text>().text = merTemp.getName();
        Stat stat = GameObject.Find("PlayerManager").GetComponent<StatData>().getMercenaryStat(merTemp.getMer_no())[profilePopupManager.getCurSetNum() - 1];

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




