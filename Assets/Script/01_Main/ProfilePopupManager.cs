using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePopupManager : MonoBehaviour {

    private int setNum = 0;     //선택된 세트 번호
    private int curSetNum = 0;  //현재 선택된 세트 번호

    private GameObject curSelectChr; //현재 선택된 캐릭터 이름

    Player.User playerTemp;

    private MercenaryManager mercenaryManager;

    private void Start()
    {
        setNum = SetSlotData.instance.getRepreSet();
        curSetNum = 1;

        playerTemp = new Player.User();

        mercenaryManager = GameObject.Find("StageManager").GetComponent<MercenaryManager>();

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
    }

    //
    public void setChrInfo(GameObject obj)
    {
        curSelectChr = obj;

        if (obj.name == "SmithSelect")
        {
            //장비 이미지, 텍스트 변경
            GameObject.Find("EquipHelmet/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Equipment/Helmet/" + Player.Play.equipHelmet[curSetNum - 1]);
            GameObject.Find("EquipHelmet/Text").GetComponent<Text>().text = Player.Play.equipHelmet[curSetNum - 1];
            GameObject.Find("EquipArmor/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Equipment/Armor/" + Player.Play.equipArmor[curSetNum - 1]);
            GameObject.Find("EquipArmor/Text").GetComponent<Text>().text = Player.Play.equipArmor[curSetNum - 1];
            GameObject.Find("EquipWeapon/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Equipment/Weapon/" + Player.Play.equipWeapon[curSetNum - 1]);
            GameObject.Find("EquipWeapon/Text").GetComponent<Text>().text = Player.Play.equipWeapon[curSetNum - 1];
            GameObject.Find("EquipBoots/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Equipment/Boots/" + Player.Play.equipBoots[curSetNum - 1]);
            GameObject.Find("EquipBoots/Text").GetComponent<Text>().text = Player.Play.equipBoots[curSetNum - 1];


            //*****캐릭터 3D모델 변경
            GameObject.Find("01_3D").transform.Find("Chr/Chr_Profile").gameObject.SetActive(true);
            //*****캐릭터 이미지 변경

            GameObject.Find("LevelText").GetComponent<Text>().text = Player.Play.level.ToString();
            GameObject.Find("PlayerNameText").GetComponent<Text>().text = Player.Play.Name;
            statusCal(curSetNum);
            GameObject.Find("DPS/Text").GetComponent<Text>().text = playerTemp.dps.ToString();
            GameObject.Find("StrPower/Text").GetComponent<Text>().text = playerTemp.strPower.ToString(); //equi[setnum].strPower
            GameObject.Find("AttackSpeed/Text").GetComponent<Text>().text = playerTemp.attackSpeed.ToString();
            GameObject.Find("Focus/Text").GetComponent<Text>().text = playerTemp.focus.ToString();
            GameObject.Find("Critical/Text").GetComponent<Text>().text = playerTemp.critical.ToString();
            GameObject.Find("DefPower/Text").GetComponent<Text>().text = playerTemp.defPower.ToString();
            GameObject.Find("EvaRate/Text").GetComponent<Text>().text = playerTemp.evaRate.ToString();
            GameObject.Find("Attribute/Text").GetComponent<Text>().text = playerTemp.attribute;
            GameObject.Find("CollectSpeed/Text").GetComponent<Text>().text = playerTemp.collectSpeed.ToString();
            GameObject.Find("CollectAmount/Text").GetComponent<Text>().text = playerTemp.collectAmount.ToString();


            GameObject.Find("ChrTitleText").GetComponent<Text>().text = Player.Play.title;
        }
        else
        {
            mercenaryManager.setMerInfo(obj);
        }
    }

    //

    //스탯 계산
    public void statusCal(int setNum)
    {
        if (setNum == 1)
        {
            playerTemp.dps = Player.Play.dps * 2;
            playerTemp.strPower = Player.Play.strPower * 1.5f;
            playerTemp.attackSpeed = Player.Play.attackSpeed + 0.3f;
            playerTemp.focus = Player.Play.focus + 20;
            playerTemp.critical = Player.Play.critical * 1.4f;
            playerTemp.defPower = Player.Play.defPower * 1.3f;
            playerTemp.evaRate = Player.Play.evaRate * 1.1f;
            playerTemp.attribute = "no";
            playerTemp.collectSpeed = Player.Play.collectSpeed * 0.7f;
            playerTemp.collectAmount = Player.Play.collectAmount * 0.8f;
        }
        else
        {
            playerTemp.dps = Player.Play.dps * 1;
            playerTemp.strPower = Player.Play.strPower * 1.1f;
            playerTemp.attackSpeed = Player.Play.attackSpeed + 0.1f;
            playerTemp.focus = Player.Play.focus + 10;
            playerTemp.critical = Player.Play.critical * 1.0f;
            playerTemp.defPower = Player.Play.defPower * 1.1f;
            playerTemp.evaRate = Player.Play.evaRate * 1.2f;
            playerTemp.attribute = "no";
            playerTemp.collectSpeed = Player.Play.collectSpeed * 1.6f;
            playerTemp.collectAmount = Player.Play.collectAmount * 1.5f;
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


