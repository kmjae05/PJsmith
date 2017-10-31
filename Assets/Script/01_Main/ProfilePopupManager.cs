using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePopupManager : MonoBehaviour {

    private int setNum = 0;     //선택된 세트 번호
    private int curSetNum = 0;  //현재 선택된 세트 번호

    private GameObject curSelectChr; //현재 선택된 캐릭터 이름

    public InventoryThings equipWeapon = new InventoryThings();
    public InventoryThings equipHelmet = new InventoryThings();
    public InventoryThings equipArmor = new InventoryThings();
    public InventoryThings equipGloves = new InventoryThings();
    public InventoryThings equipPants = new InventoryThings();
    public InventoryThings equipBoots = new InventoryThings();

    private MercenaryManager mercenaryManager;
    private StatData statData;

    private void Start()
    {
        setNum = SetSlotData.instance.getRepreSet();
        curSetNum = 1;

        mercenaryManager = GameObject.Find("StageManager").GetComponent<MercenaryManager>();
        statData = GameObject.Find("PlayerManager").GetComponent<StatData>();

        if (setNum == 1)
        {
            GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/SetSlot/Set1Button").gameObject.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
            GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/SetSlot/Set1Button (1)").gameObject.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
            GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/SetSlot/Set2Button").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/SetSlot/Set2Button (1)").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
        if (setNum == 2)
        {
            GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/SetSlot/Set1Button").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/SetSlot/Set1Button (1)").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/SetSlot/Set2Button").gameObject.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
            GameObject.Find("Menu").transform.Find("ProfilePopup/UIPanel/ProfilePanel/SetSlot/Set2Button (1)").gameObject.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
        }
    }

    //팝업창 켤 때 첫번째 세트, 대장장이 띄우기
    public void openPopup()
    {
        curSetNum = 1;
        setChrInfo(GameObject.Find("SmithSelect"));
        //setOutLine(GameObject.Find("SmithSelect"));
    }


    //현재 세트 번호
    public void setCurSetNum(int num) { curSetNum = num; setChrInfo(curSelectChr); }

    //대표 세트 변경
    public void changeSet()
    {
        setNum = curSetNum;
        SetSlotData.instance.setRepreSet(setNum);

        if (setNum == 1)
        {
            GameObject.Find("SetSlot").transform.Find("Set1Button").gameObject.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
            GameObject.Find("SetSlot").transform.Find("Set1Button (1)").gameObject.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
            GameObject.Find("SetSlot").transform.Find("Set2Button").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            GameObject.Find("SetSlot").transform.Find("Set2Button (1)").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
        if (setNum == 2)
        {
            GameObject.Find("SetSlot").transform.Find("Set1Button").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            GameObject.Find("SetSlot").transform.Find("Set1Button (1)").gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            GameObject.Find("SetSlot").transform.Find("Set2Button").gameObject.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
            GameObject.Find("SetSlot").transform.Find("Set2Button (1)").gameObject.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
        }
        statData.repreSetStatCal();
    }

    //
    public void setChrInfo(GameObject obj)
    {
        curSelectChr = obj;

        if (obj.name == "SmithSelect")
        {
            //장비 찾기
            setEquipment(Player.instance.getUser().Name, curSetNum);

            //장비 이미지, 텍스트 변경
            GameObject.Find("EquipHelmet/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipHelmet.name).icon);
            GameObject.Find("EquipHelmet/Text").GetComponent<Text>().text = Player.instance.getUser().equipHelmet[curSetNum - 1].name;
            GameObject.Find("EquipArmor/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipArmor.name).icon);
            GameObject.Find("EquipArmor/Text").GetComponent<Text>().text = Player.instance.getUser().equipArmor[curSetNum - 1].name;
            GameObject.Find("EquipWeapon/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipWeapon.name).icon);
            GameObject.Find("EquipWeapon/Text").GetComponent<Text>().text = Player.instance.getUser().equipWeapon[curSetNum - 1].name;
            GameObject.Find("EquipBoots/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipBoots.name).icon);
            GameObject.Find("EquipBoots/Text").GetComponent<Text>().text = Player.instance.getUser().equipBoots[curSetNum - 1].name;
            GameObject.Find("EquipGloves/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipGloves.name).icon);
            GameObject.Find("EquipGloves/Text").GetComponent<Text>().text = Player.instance.getUser().equipGloves[curSetNum - 1].name;
            GameObject.Find("EquipPants/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipPants.name).icon);
            GameObject.Find("EquipPants/Text").GetComponent<Text>().text = Player.instance.getUser().equipPants[curSetNum - 1].name;

            //등급 프레임
            GameObject.Find("EquipHelmet/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipHelmet.name).grade);
            GameObject.Find("EquipArmor/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipArmor.name).grade);
            GameObject.Find("EquipWeapon/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipWeapon.name).grade);
            GameObject.Find("EquipBoots/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipBoots.name).grade);
            GameObject.Find("EquipGloves/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipGloves.name).grade);
            GameObject.Find("EquipPants/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipPants.name).grade);

            //장비 인포 버튼
            GameObject.Find("EquipWeapon").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipWeapon").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(equipWeapon);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            });
            GameObject.Find("EquipHelmet").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipHelmet").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(equipHelmet);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            });
            GameObject.Find("EquipArmor").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipArmor").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(equipArmor);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            });
            GameObject.Find("EquipGloves").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipGloves").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(equipGloves);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            });
            GameObject.Find("EquipPants").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipPants").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(equipPants);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            });
            GameObject.Find("EquipBoots").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipBoots").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(equipBoots);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
            });


            //*****캐릭터 3D모델 변경
            //GameObject.Find("01_3D").transform.Find("Chr/Chr_Profile").gameObject.SetActive(true);
            //*****캐릭터 이미지 변경

            GameObject.Find("LevelText").GetComponent<Text>().text = Player.instance.getUser().level.ToString();
            GameObject.Find("PlayerNameText").GetComponent<Text>().text = Player.instance.getUser().Name;

            Stat stat = GameObject.Find("PlayerManager").GetComponent<StatData>().getPlayerStat()[curSetNum - 1];
            GameObject.Find("DPS/Text").GetComponent<Text>().text = ((int)stat.dps).ToString();
            GameObject.Find("StrPower/Text").GetComponent<Text>().text = ((int)stat.strPower).ToString(); //equi[setnum].strPower
            GameObject.Find("AttackSpeed/Text").GetComponent<Text>().text = (stat.attackSpeed).ToString();
            GameObject.Find("Focus/Text").GetComponent<Text>().text = ((int)stat.focus).ToString();
            GameObject.Find("Critical/Text").GetComponent<Text>().text = ((int)stat.critical).ToString();
            GameObject.Find("DefPower/Text").GetComponent<Text>().text = ((int)stat.defPower).ToString();
            GameObject.Find("EvaRate/Text").GetComponent<Text>().text = ((int)stat.evaRate).ToString();
            GameObject.Find("Attribute/Text").GetComponent<Text>().text = Player.instance.getUser().attribute;
            //GameObject.Find("CollectSpeed/Text").GetComponent<Text>().text = ((int)stat.collectSpeed).ToString();
            //GameObject.Find("CollectAmount/Text").GetComponent<Text>().text = ((int)stat.collectAmount).ToString();


            //GameObject.Find("ChrTitleText").GetComponent<Text>().text = Player.instance.getUser().title;
        }
        else
        {
            mercenaryManager.setMerInfo(obj);
        }
    }

    //장비 찾기
    public void setEquipment(string chrName, int setNum)
    {
        equipWeapon = ThingsData.instance.getInventoryThingsList().Find(x => x.equipChrName == chrName && x.equipSetNum == setNum && x.type == "Weapon");
        equipHelmet = ThingsData.instance.getInventoryThingsList().Find(x => x.equipChrName == chrName && x.equipSetNum == setNum && x.type == "Helmet");
        equipArmor = ThingsData.instance.getInventoryThingsList().Find(x => x.equipChrName == chrName && x.equipSetNum == setNum && x.type == "Armor");
        equipGloves = ThingsData.instance.getInventoryThingsList().Find(x => x.equipChrName == chrName && x.equipSetNum == setNum && x.type == "Gloves");
        equipPants = ThingsData.instance.getInventoryThingsList().Find(x => x.equipChrName == chrName && x.equipSetNum == setNum && x.type == "Pants");
        equipBoots = ThingsData.instance.getInventoryThingsList().Find(x => x.equipChrName == chrName && x.equipSetNum == setNum && x.type == "Boots");
    }



    //캐릭터 선택 시 아웃라인 위치 변경
    public void setOutLine(GameObject chr)
    {
        GameObject.Find("ChrSelect/ProfileOutline").transform.localPosition = chr.transform.localPosition;
    }




    public void setSetNum(int num) { setNum = num; }
    public int getSetNum() { return setNum; }

    public int getCurSetNum() { return curSetNum; }

}


