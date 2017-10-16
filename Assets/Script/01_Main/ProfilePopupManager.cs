using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePopupManager : MonoBehaviour {

    private int setNum = 0;     //선택된 세트 번호
    private int curSetNum = 0;  //현재 선택된 세트 번호

    private GameObject curSelectChr; //현재 선택된 캐릭터 이름

    Player.User player;

    private MercenaryManager mercenaryManager;
    private StatData statData;

    private void Start()
    {
        setNum = SetSlotData.instance.getRepreSet();
        curSetNum = 1;

        player = new Player.User();

        mercenaryManager = GameObject.Find("StageManager").GetComponent<MercenaryManager>();
        statData = GameObject.Find("PlayerData").GetComponent<StatData>();

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
        setOutLine(GameObject.Find("SmithSelect"));
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
            //장비 이미지, 텍스트 변경
            GameObject.Find("EquipHelmet/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x=>x.name ==  Player.Play.equipHelmet[curSetNum - 1].name).icon);
            GameObject.Find("EquipHelmet/Text").GetComponent<Text>().text = Player.Play.equipHelmet[curSetNum - 1].name;
            GameObject.Find("EquipArmor/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == Player.Play.equipArmor[curSetNum - 1].name).icon);
            GameObject.Find("EquipArmor/Text").GetComponent<Text>().text = Player.Play.equipArmor[curSetNum - 1].name;
            GameObject.Find("EquipWeapon/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == Player.Play.equipWeapon[curSetNum - 1].name).icon);
            GameObject.Find("EquipWeapon/Text").GetComponent<Text>().text = Player.Play.equipWeapon[curSetNum - 1].name;
            GameObject.Find("EquipBoots/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == Player.Play.equipBoots[curSetNum - 1].name).icon);
            GameObject.Find("EquipBoots/Text").GetComponent<Text>().text = Player.Play.equipBoots[curSetNum - 1].name;


            //*****캐릭터 3D모델 변경
            GameObject.Find("01_3D").transform.Find("Chr/Chr_Profile").gameObject.SetActive(true);
            //*****캐릭터 이미지 변경

            GameObject.Find("LevelText").GetComponent<Text>().text = Player.Play.level.ToString();
            GameObject.Find("PlayerNameText").GetComponent<Text>().text = Player.Play.Name;

            Stat stat = GameObject.Find("PlayerData").GetComponent<StatData>().getPlayerStat()[curSetNum - 1];
            GameObject.Find("DPS/Text").GetComponent<Text>().text = ((int)stat.dps).ToString();
            GameObject.Find("StrPower/Text").GetComponent<Text>().text = ((int)stat.strPower).ToString(); //equi[setnum].strPower
            GameObject.Find("AttackSpeed/Text").GetComponent<Text>().text = ((int)stat.attackSpeed).ToString();
            GameObject.Find("Focus/Text").GetComponent<Text>().text = ((int)stat.focus).ToString();
            GameObject.Find("Critical/Text").GetComponent<Text>().text = ((int)stat.critical).ToString();
            GameObject.Find("DefPower/Text").GetComponent<Text>().text = ((int)stat.defPower).ToString();
            GameObject.Find("EvaRate/Text").GetComponent<Text>().text = ((int)stat.evaRate).ToString();
            GameObject.Find("Attribute/Text").GetComponent<Text>().text = player.attribute;
            GameObject.Find("CollectSpeed/Text").GetComponent<Text>().text = ((int)stat.collectSpeed).ToString();
            GameObject.Find("CollectAmount/Text").GetComponent<Text>().text = ((int)stat.collectAmount).ToString();


            GameObject.Find("ChrTitleText").GetComponent<Text>().text = Player.Play.title;
        }
        else
        {
            mercenaryManager.setMerInfo(obj);
        }
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


