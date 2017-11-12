using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipReinforceManager : MonoBehaviour {

    private EquipmentData equipmentData;
    private ProfilePopupManager profileManager;

    private GameObject equipSelectPopup;
    private GameObject uiBox;

    // 교체/강화 표시
    private Text TitleText;
    private Text curEquipBoxInfoText;
    private Text changeEquipBoxInfoText;

    // 교체/강화할 장비
    private GameObject curEquipBox;
    private GameObject curItemBox;
    private Image curItemBoxGradeFrame;
    private Image curItemBoxIcon;
    private Text curItemBoxNameText;
    private GameObject curInfoBox;
    private Text curInfoBoxReinText;
    private Text curInfoBoxAbilityInfoText;
    private Text curInfoBoxAbilityText;

    // 교체/강화 후 장비
    private GameObject changeEquipBox;
    private GameObject changeItemBox;
    private Image changeItemBoxGradeFrame;
    private Image changeItemBoxIcon;
    private Text changeItemBoxNameText;
    private GameObject changeInfoBox;
    private Text changeInfoBoxReinText;
    private GameObject changeInfoBoxAbilityInfoTextGroup;
    private GameObject changeInfoBoxAbilityTextGroup;
    private Slider changeInfoBoxExpSlider;
    private Text changeInfoBoxExpText;
    private GameObject changeInfoBoxAdditionGroup;

    // 인벤토리
    private GameObject inventoryUIPanel;
    private GameObject inventoryPanel;
    private GameObject inventoryBox;
    private Image inventoryBoxGradeFrame;
    private Image inventoryBoxIcon;
    private Text inventoryBoxAmount;
    private GameObject selectImage;
    private List<GameObject> inventoryThings;       //인벤토리 오브젝트 리스트
    private Text selectNumText;

    //버튼
    private Button ChangeButton;
    private Button ReinforceButton;

    private InventoryThings curInventoryThings;     //현재 선택된 장비
    private string curType;                         //현재 선택된 장비 종류

    private int tmpexp = 0;             //미리보기 경험치
    private int sliderexp = 0;
    private int tmprein= 0;             //미리보기 강화 수치
    private int selectNum = 0;          //선택 개수
    private List<GameObject> selectObjList = new List<GameObject>();
    private List<InventoryThings> selectInvenList = new List<InventoryThings>();

    private Color green = new Color(0.41f, 0.85f, 0.4f);
    private Color red = new Color(1f, 0.32f, 0.21f);
    private Color defaultColor = new Color(0.84f, 0.83f, 0.8f);

    private GameObject success;
    private GameObject lightImage;
    float speed = 0.5f;

    private void Start()
    {
        equipmentData = GameObject.Find("ThingsData").GetComponent<EquipmentData>();
        profileManager = GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>();

        equipSelectPopup = GameObject.Find("System").transform.Find("EquipSelect").gameObject;
        uiBox = equipSelectPopup.transform.Find("UIBox").gameObject;

        TitleText = uiBox.transform.Find("TitleBox/TitleText").gameObject.GetComponent<Text>();
        curEquipBoxInfoText = uiBox.transform.Find("CurEquipBox/InfoText").gameObject.GetComponent<Text>();
        changeEquipBoxInfoText = uiBox.transform.Find("ChangeEquipBox/InfoText").gameObject.GetComponent<Text>();

        curEquipBox = uiBox.transform.Find("CurEquipBox").gameObject;
        curItemBox = curEquipBox.transform.Find("ItemBox").gameObject;
        curItemBoxGradeFrame = curItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>();
        curItemBoxIcon = curItemBox.transform.Find("Icon").gameObject.GetComponent<Image>();
        curItemBoxNameText = curItemBox.transform.Find("NameText").gameObject.GetComponent<Text>();
        curInfoBox = curEquipBox.transform.Find("InfoBox").gameObject;
        curInfoBoxReinText = curInfoBox.transform.Find("ReinText").gameObject.GetComponent<Text>();
        curInfoBoxAbilityInfoText = curInfoBox.transform.Find("AbilityInfoText").gameObject.GetComponent<Text>();
        curInfoBoxAbilityText = curInfoBox.transform.Find("AbilityText").gameObject.GetComponent<Text>();

        changeEquipBox = uiBox.transform.Find("ChangeEquipBox").gameObject;
        changeItemBox = changeEquipBox.transform.Find("ItemBox").gameObject;
        changeItemBoxGradeFrame = changeItemBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>();
        changeItemBoxIcon = changeItemBox.transform.Find("Icon").gameObject.GetComponent<Image>();
        changeItemBoxNameText = changeItemBox.transform.Find("NameText").gameObject.GetComponent<Text>();
        changeInfoBox = changeEquipBox.transform.Find("InfoBox").gameObject;
        changeInfoBoxReinText = changeInfoBox.transform.Find("ReinText").gameObject.GetComponent<Text>();
        changeInfoBoxAbilityInfoTextGroup = changeInfoBox.transform.Find("AbilityInfoTextGroup").gameObject;
        changeInfoBoxAbilityTextGroup = changeInfoBox.transform.Find("AbilityTextGroup").gameObject;
        changeInfoBoxExpSlider = changeInfoBox.transform.Find("ExpSlider").gameObject.GetComponent<Slider>();
        changeInfoBoxExpText = changeInfoBoxExpSlider.transform.Find("ExpText").gameObject.GetComponent<Text>();
        changeInfoBoxAdditionGroup = changeInfoBox.transform.Find("AdditionTextGroup").gameObject;

        inventoryUIPanel = equipSelectPopup.transform.Find("InventoryUIPanel").gameObject;
        inventoryPanel = inventoryUIPanel.transform.Find("Scroll/Panel").gameObject;
        inventoryBox = inventoryPanel.transform.Find("InventoryBox").gameObject;
        inventoryBoxGradeFrame = inventoryBox.transform.Find("GradeFrame").gameObject.GetComponent<Image>();
        inventoryBoxIcon = inventoryBox.transform.Find("Item/Icon").gameObject.GetComponent<Image>();
        inventoryBoxAmount = inventoryBox.transform.Find("Item/AmountText").gameObject.GetComponent<Text>();
        selectImage = inventoryBox.transform.Find("SelectImage").gameObject;
        inventoryThings = new List<GameObject>();
        selectNumText = inventoryUIPanel.transform.Find("SelectNumText").gameObject.GetComponent<Text>();

        ChangeButton = inventoryUIPanel.transform.Find("ChangeButton").gameObject.GetComponent<Button>();
        ReinforceButton = inventoryUIPanel.transform.Find("ReinforceButton").gameObject.GetComponent<Button>();

        success = equipSelectPopup.transform.Find("Success").gameObject;
        lightImage = success.transform.Find("LightImage").gameObject;

        curInventoryThings = new InventoryThings();
        curType = null;

        Button closeButton = inventoryUIPanel.transform.Find("CloseButton").gameObject.GetComponent<Button>();
        closeButton.onClick.AddListener(() =>
       {
           selectNum = 0;
           tmpexp = 0;
           tmprein = 0;
           for (int i = 0; i < inventoryThings.Count; i++) Destroy(inventoryThings[i]);
           inventoryThings.Clear();
       });

    }

    void Update()
    {
        if (success.activeInHierarchy)
            lightImage.transform.Rotate(new Vector3(0, 0, 1), 1 * speed);
    }


    //강화
    public void ReinforceEquip(InventoryThings equipThings)
    {
        ChangeButton.transform.gameObject.SetActive(false);
        ReinforceButton.transform.gameObject.SetActive(true);
        changeInfoBoxExpSlider.transform.gameObject.SetActive(true);
        changeInfoBoxAbilityTextGroup.SetActive(true);
        changeInfoBoxAdditionGroup.SetActive(false);

        curInventoryThings = equipThings;

        TitleText.text = "장비 강화";
        curEquipBoxInfoText.text = "";
        changeEquipBoxInfoText.text = "";

        curItemBoxGradeFrame.color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipThings.name).grade);
        curItemBoxIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipThings.name).icon);
        curItemBoxNameText.text = equipThings.name;
        if (equipThings.reinforcement > 0)
            curInfoBoxReinText.text = "+" + equipThings.reinforcement.ToString();
        else curInfoBoxReinText.text = null;

        string abstr = "";
        string statstr = "";
        Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == equipThings.name);
        //강화 수치에 따라 스탯 계산 다시 해줘야 함.

        if (equipThings.stat.dps > 0) { abstr += "전투력"; statstr += (int)equipThings.stat.dps; }
        if (equipThings.stat.strPower > 0) { abstr += "\n공격력"; statstr +=  "\n"+(int)equipThings.stat.strPower; }
        if (equipThings.stat.attackSpeed > 0) { abstr += "\n공격속도"; statstr +=  "\n"+equipThings.stat.attackSpeed ; }
        if (equipThings.stat.focus > 0) { abstr += "명중률\n"; statstr += (int)equipThings.stat.focus + "\n"; }
        if (equipThings.stat.critical > 0) { abstr += "\n크리티컬"; statstr += "\n"+(int)equipThings.stat.critical ; }
        if (equipThings.stat.defPower > 0) { abstr += "\n방어력"; statstr +="\n" + (int)equipThings.stat.defPower ; }
        if (equipThings.stat.evaRate > 0) { abstr += "\n회피율"; statstr +="\n" + (int)equipThings.stat.evaRate ; }
        //if (equipThings.attribute != null) { abstr += "속성"; statstr += equipThings.attribute.ToString(); }
        curInfoBoxAbilityInfoText.text = abstr;
        curInfoBoxAbilityText.text = statstr;


        //강화 후 장비
        changeItemBoxGradeFrame.color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipThings.name).grade);
        changeItemBoxIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipThings.name).icon);
        changeItemBoxNameText.text = equipThings.name;
        if (equipThings.reinforcement > 0)
            changeInfoBoxReinText.text = "+" + equipThings.reinforcement.ToString();
        else changeInfoBoxReinText.text = null;

        for (int i = 0; i < changeInfoBoxAbilityInfoTextGroup.transform.childCount; i++)
        {
            changeInfoBoxAbilityInfoTextGroup.transform.GetChild(i).gameObject.SetActive(false);
            changeInfoBoxAbilityTextGroup.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (equipThings.stat.dps > 0)
        {
            changeInfoBoxAbilityInfoTextGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = "전투력";
            changeInfoBoxAbilityInfoTextGroup.transform.Find("DpsText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("DpsText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = ((int)equipThings.stat.dps).ToString();
        }
        if (equipThings.stat.strPower > 0)
        {
            changeInfoBoxAbilityInfoTextGroup.transform.Find("StrPowerText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("StrPowerText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = ((int)equipThings.stat.strPower).ToString();
        }
        if (equipThings.stat.attackSpeed > 0)
        {
            changeInfoBoxAbilityInfoTextGroup.transform.Find("AttackSpeedText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("AttackSpeedText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = (equipThings.stat.attackSpeed).ToString();
        }
        if (equipThings.stat.critical > 0)
        {
            changeInfoBoxAbilityInfoTextGroup.transform.Find("CriticalText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("CriticalText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = ((int)equipThings.stat.critical).ToString();
        }
        if (equipThings.stat.focus > 0)
        {
            changeInfoBoxAbilityInfoTextGroup.transform.Find("FocusText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("FocusText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = ((int)equipThings.stat.focus).ToString();
        }
        if (equipThings.stat.defPower > 0)
        {
            changeInfoBoxAbilityInfoTextGroup.transform.Find("DefPowerText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("DefPowerText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = ((int)equipThings.stat.defPower).ToString();
        }
        if (equipThings.stat.evaRate > 0)
        {
            changeInfoBoxAbilityInfoTextGroup.transform.Find("EvaRateText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("EvaRateText").gameObject.SetActive(true);
            changeInfoBoxAbilityTextGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = ((int)equipThings.stat.evaRate).ToString();
        }


        changeItemBoxIcon.transform.gameObject.SetActive(true);
        changeItemBoxNameText.transform.gameObject.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(true);
        changeInfoBoxAbilityTextGroup.transform.gameObject.SetActive(true);
        changeInfoBoxExpSlider.value = curInventoryThings.exp;
        changeInfoBoxExpText.text = curInventoryThings.exp + "%";

        selectNumText.text = "재료 선택 0 / 10";

        selectNumText.transform.gameObject.SetActive(true);
            
        createInventory(equip);

        equipSelectPopup.SetActive(true);
    }



    //강화 재료 아이템 선택 (10개 한정 (터치 해제 가능
    IEnumerator SelectItem(GameObject obj, InventoryThings invenThings)
    {
        if (selectNum >= 10) yield break;
        selectNum++;
        Debug.Log(selectNum);

        selectObjList.Add(obj);
        selectInvenList.Add(invenThings);

        obj.transform.Find("SelectImage").gameObject.SetActive(true);

        //강화 상태 미리보기
        //tmprein = invenThings.reinforcement;
        tmpexp += 10;
        Debug.Log(tmpexp);
        sliderexp = tmpexp + curInventoryThings.exp;

        if (sliderexp >= 100)
        {
            //tmpexp = 0;
            sliderexp %= 100;
            if(sliderexp == 0)
                tmprein += 1;
            
        }
        

        //밑에 장비 표시
        if (invenThings.reinforcement > 0)
            changeInfoBoxReinText.text = invenThings.reinforcement.ToString();
        else changeInfoBoxReinText.text = null;

        string abstr = "";
        string statstr = "";
        //Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == curInventoryThings.name);

        float dps = 0;
        if (curInventoryThings.stat.strPower > 0)
        {
            dps = (int)(curInventoryThings.stat.strPower + tmprein * 1.5f) * ((float)curInventoryThings.stat.attackSpeed + tmprein * 1.5f) * (int)(curInventoryThings.stat.critical + tmprein * 1.5f);
        }
        else if(curInventoryThings.stat.defPower > 0)
        {
            dps = (int)(curInventoryThings.stat.defPower + tmprein * 1.5f) * (curInventoryThings.stat.evaRate + tmprein * 1.5f) ;
        }
        if (dps == 0)
        {
            //기본 스탯
            dps = curInventoryThings.stat.dps;
            if (curInventoryThings.stat.dps > 0) { abstr += "전투력"; statstr += (int)dps ; }
            if (curInventoryThings.stat.strPower > 0) { abstr += "\n공격력"; statstr += "\n" + (int)(curInventoryThings.stat.strPower ) ; }
            if (curInventoryThings.stat.attackSpeed > 0) { abstr += "\n공격속도"; statstr += "\n" + (curInventoryThings.stat.attackSpeed); }
            if (curInventoryThings.stat.focus > 0) { abstr += "명중률\n"; statstr += "\n" + (int)(curInventoryThings.stat.focus ); }
            if (curInventoryThings.stat.critical > 0) { abstr += "\n크리티컬"; statstr += "\n" + (int)(curInventoryThings.stat.critical ) ; }
            if (curInventoryThings.stat.defPower > 0) { abstr += "\n방어력"; statstr += "\n" + (int)(curInventoryThings.stat.defPower ); }
            if (curInventoryThings.stat.evaRate > 0) { abstr += "\n회피율"; statstr += "\n" + (int)(curInventoryThings.stat.evaRate ); }
            //if (curInventoryThings.attribute != null) { abstr += "속성"; statstr += curInventoryThings.attribute.ToString(); }
        }
        else
        {
            if (curInventoryThings.stat.dps > 0) { abstr += "전투력"; statstr += (int)dps + "\n";  }
            if (curInventoryThings.stat.strPower > 0) { abstr += "\n공격력"; statstr += "\n" + (int)(curInventoryThings.stat.strPower + tmprein * 1.5f) ; }
            if (curInventoryThings.stat.attackSpeed > 0) { abstr += "\n공격속도"; statstr += "\n" + (curInventoryThings.stat.attackSpeed + tmprein * 1.5f);  }
            if (curInventoryThings.stat.critical > 0) { abstr += "\n크리티컬"; statstr += "\n" + (int)(curInventoryThings.stat.critical + tmprein * 1.5f);  }
            if (curInventoryThings.stat.focus > 0) { abstr += "명중률\n"; statstr += "\n" + (int)(curInventoryThings.stat.focus + tmprein * 1.5f) ;  }
            if (curInventoryThings.stat.defPower > 0) { abstr += "\n방어력"; statstr += "\n" + (int)(curInventoryThings.stat.defPower + tmprein * 1.5f) ;}
            if (curInventoryThings.stat.evaRate > 0) { abstr += "\n회피율"; statstr += "\n" + (int)(curInventoryThings.stat.evaRate + tmprein * 1.5f) ;  }
            //if (curInventoryThings.attribute != null) { abstr += "속성"; statstr += curInventoryThings.attribute.ToString(); }
        }
        
        //changeInfoBoxAbilityInfoText.text = abstr;
        //changeInfoBoxAbilityText.text = statstr;
        if (curInventoryThings.reinforcement > 0 || tmprein>0)
            changeInfoBoxReinText.text = "+" + (tmprein + curInventoryThings.reinforcement).ToString();
        else changeInfoBoxReinText.text = null;

        if (tmprein > 0)
        {
            for (int i = 0; i < changeInfoBoxAbilityInfoTextGroup.transform.childCount; i++)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.GetChild(i).gameObject.SetActive(false);
                changeInfoBoxAbilityTextGroup.transform.GetChild(i).gameObject.SetActive(false);
                changeInfoBoxAdditionGroup.transform.GetChild(i).gameObject.SetActive(false);
            }

            if (invenThings.stat.dps > 0)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = "전투력";
                changeInfoBoxAbilityInfoTextGroup.transform.Find("DpsText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("DpsText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = ((int)dps).ToString();

                changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.SetActive(true);
                //
                if ((int)(dps - curInventoryThings.stat.dps) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = "+" + ((int)(dps - curInventoryThings.stat.dps)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)(dps - curInventoryThings.stat.dps) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = ((int)(dps - curInventoryThings.stat.dps)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)(dps - curInventoryThings.stat.dps) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = ((int)(dps - curInventoryThings.stat.dps)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.strPower > 0)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.Find("StrPowerText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("StrPowerText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.strPower + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.SetActive(true);
                //
                if ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = "+" + ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.attackSpeed > 0)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.Find("AttackSpeedText").gameObject.SetActive(true);

                changeInfoBoxAbilityTextGroup.transform.Find("AttackSpeedText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = (curInventoryThings.stat.attackSpeed + tmprein * 1.5f).ToString();

                changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.SetActive(true);
                //
                if ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = "+" + ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().color = red;
                }
            }

            if (invenThings.stat.critical > 0)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.Find("CriticalText").gameObject.SetActive(true);

                changeInfoBoxAbilityTextGroup.transform.Find("CriticalText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.critical + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.SetActive(true);
                //
                if ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = "+" + ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.focus > 0)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.Find("FocusText").gameObject.SetActive(true);

                changeInfoBoxAbilityTextGroup.transform.Find("FocusText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.focus + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.SetActive(true);
                //
                if ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = "+" + ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.defPower > 0)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.Find("DefPowerText").gameObject.SetActive(true);

                changeInfoBoxAbilityTextGroup.transform.Find("DefPowerText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.defPower + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.SetActive(true);
                //
                if ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = "+" + ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.evaRate > 0)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.Find("EvaRateText").gameObject.SetActive(true);

                changeInfoBoxAbilityTextGroup.transform.Find("EvaRateText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.evaRate + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.SetActive(true);
                //
                if ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = "+" + ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) ).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) ).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) ).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().color = red;
                }
            }
            changeInfoBoxAdditionGroup.transform.gameObject.SetActive(true);
        }
        else changeInfoBoxAdditionGroup.transform.gameObject.SetActive(false);

        Debug.Log(tmpexp);
        Debug.Log(curInventoryThings.exp);

        changeItemBoxIcon.transform.gameObject.SetActive(true);
        changeItemBoxNameText.transform.gameObject.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(true);
        changeInfoBoxAbilityTextGroup.transform.gameObject.SetActive(true);
        changeInfoBoxExpSlider.value = sliderexp;
        changeInfoBoxExpText.text = sliderexp + "%";

        if (tmprein > 0 && ((curInventoryThings.exp + tmpexp) == 0 || (curInventoryThings.exp + tmpexp) == 100))
        {
            changeInfoBoxExpSlider.value = changeInfoBoxExpSlider.maxValue;
            changeInfoBoxExpText.text = "100%";
        }


        selectNumText.text = "재료 선택 " + selectNum + " / 10";

        obj.GetComponent<Button>().onClick.RemoveAllListeners();
        obj.GetComponent<Button>().onClick.AddListener(
            () => {
                CancelItem(obj, invenThings);
            });

        //강화 버튼 
        ReinforceButton.onClick.RemoveAllListeners();
        ReinforceButton.onClick.AddListener(() => Reinforce(invenThings));
    }

    //선택 해제
    void CancelItem(GameObject obj, InventoryThings invenThings)
    {
        selectObjList.Remove(obj);
        selectInvenList.Remove(invenThings);

        selectNum--;

        obj.transform.Find("SelectImage").gameObject.SetActive(false);

        //강화 상태 미리보기
        
        tmpexp -= 10;
        Debug.Log(tmpexp);
        //if (tmpexp + curInventoryThings.exp < 0)
        //{
        //    //
        //    tmpexp = 90;
        //    tmprein -= 1;
        //}
        sliderexp = tmpexp + curInventoryThings.exp;
        if (sliderexp == 90 && tmprein > 0)
        {
            tmprein -= 1;
        }
        sliderexp %= 100;

        //밑에 장비 표시
        if (invenThings.reinforcement > 0)
            changeInfoBoxReinText.text = invenThings.reinforcement.ToString();
        else changeInfoBoxReinText.text = null;

        string abstr = "";
        string statstr = "";
        //Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == curInventoryThings.name);

        float dps = 0;
        if (curInventoryThings.stat.strPower > 0)
        {
            dps = (int)(curInventoryThings.stat.strPower + tmprein * 1.5f) * ((float)curInventoryThings.stat.attackSpeed + tmprein * 1.5f) * (int)(curInventoryThings.stat.critical + tmprein * 1.5f);
        }
        else if (curInventoryThings.stat.defPower > 0)
        {
            dps = (curInventoryThings.stat.defPower + tmprein * 1.5f) * ((float)curInventoryThings.stat.evaRate + tmprein * 1.5f);
        }
        if (dps == 0)
        {
            //기본 스탯
            dps = curInventoryThings.stat.dps;
            if (curInventoryThings.stat.dps > 0) { abstr += "전투력"; statstr += (int)dps; }
            if (curInventoryThings.stat.strPower > 0) { abstr += "\n공격력"; statstr += "\n" + (int)(curInventoryThings.stat.strPower) ; }
            if (curInventoryThings.stat.attackSpeed > 0) { abstr += "\n공격속도"; statstr += "\n" + (curInventoryThings.stat.attackSpeed); }
            if (curInventoryThings.stat.focus > 0) { abstr += "명중률\n"; statstr += "\n" + (int)(curInventoryThings.stat.focus) ; }
            if (curInventoryThings.stat.critical > 0) { abstr += "\n크리티컬"; statstr += "\n" + (int)(curInventoryThings.stat.critical); }
            if (curInventoryThings.stat.defPower > 0) { abstr += "\n방어력"; statstr += "\n" + (int)(curInventoryThings.stat.defPower) ; }
            if (curInventoryThings.stat.evaRate > 0) { abstr += "\n회피율"; statstr += "\n" + (int)(curInventoryThings.stat.evaRate) ; }
            //if (curInventoryThings.attribute != null) { abstr += "속성"; statstr += curInventoryThings.attribute.ToString(); }
        }
        else
        {
            if (curInventoryThings.stat.dps > 0) { abstr += "전투력"; statstr += (int)dps ; }
            if (curInventoryThings.stat.strPower > 0) { abstr += "\n공격력"; statstr += "\n" + (int)(curInventoryThings.stat.strPower + tmprein * 1.5f); }
            if (curInventoryThings.stat.attackSpeed > 0) { abstr += "\n공격속도"; statstr += "\n" + (curInventoryThings.stat.attackSpeed + tmprein * 1.5f); }
            if (curInventoryThings.stat.focus > 0) { abstr += "명중률\n"; statstr += "\n" + (int)(curInventoryThings.stat.focus + tmprein * 1.5f); }
            if (curInventoryThings.stat.critical > 0) { abstr += "\n크리티컬"; statstr += "\n" + (int)(curInventoryThings.stat.critical + tmprein * 1.5f); }
            if (curInventoryThings.stat.defPower > 0) { abstr += "\n방어력"; statstr += "\n" + (int)(curInventoryThings.stat.critical + tmprein * 1.5f); }
            if (curInventoryThings.stat.evaRate > 0) { abstr += "\n회피율"; statstr += "\n" + (int)(curInventoryThings.stat.critical + tmprein * 1.5f); }
            //if (curInventoryThings.attribute != null) { abstr += "속성"; statstr += curInventoryThings.attribute.ToString(); }
        }

        //changeInfoBoxAbilityInfoText.text = abstr;
        //changeInfoBoxAbilityText.text = statstr;
        if (curInventoryThings.reinforcement + tmprein > 0)
            changeInfoBoxReinText.text = "+" + (tmprein + curInventoryThings.reinforcement).ToString();
        else changeInfoBoxReinText.text = null;

        if (tmprein > 0)
        {

            for (int i = 0; i < changeInfoBoxAbilityTextGroup.transform.childCount; i++)
            {
                changeInfoBoxAbilityTextGroup.transform.GetChild(i).gameObject.SetActive(false);
                changeInfoBoxAdditionGroup.transform.GetChild(i).gameObject.SetActive(false);
            }

            if (invenThings.stat.dps > 0)
            {
                changeInfoBoxAbilityInfoTextGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = "전투력";
                changeInfoBoxAbilityTextGroup.transform.Find("DpsText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = ((int)dps).ToString();

                changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.SetActive(true);
                //
                if ((int)(dps - curInventoryThings.stat.dps) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = "+" + ((int)(dps - curInventoryThings.stat.dps)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)(dps - curInventoryThings.stat.dps) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = ((int)(dps - curInventoryThings.stat.dps)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)(dps - curInventoryThings.stat.dps) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = ((int)(dps - curInventoryThings.stat.dps)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.strPower > 0)
            {
                changeInfoBoxAbilityTextGroup.transform.Find("StrPowerText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.strPower + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.SetActive(true);
                //
                if ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = "+" + ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.strPower + tmprein * 1.5f) - curInventoryThings.stat.strPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.attackSpeed > 0)
            {
                changeInfoBoxAbilityTextGroup.transform.Find("AttackSpeedText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = (curInventoryThings.stat.attackSpeed + tmprein * 1.5f).ToString();

                changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.SetActive(true);
                //
                if ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed> 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = "+" + ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed== 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed< 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = ((curInventoryThings.stat.attackSpeed + tmprein * 1.5f) - curInventoryThings.stat.attackSpeed).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().color = red;
                }
            }

            if (invenThings.stat.critical > 0)
            {
                changeInfoBoxAbilityTextGroup.transform.Find("CriticalText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.critical + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.SetActive(true);
                //
                if ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = "+" + ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.critical + tmprein * 1.5f) - curInventoryThings.stat.critical)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.focus > 0)
            {
                changeInfoBoxAbilityTextGroup.transform.Find("FocusText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.focus + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.SetActive(true);
                //
                if ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = "+" + ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.focus + tmprein * 1.5f) - curInventoryThings.stat.focus)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.defPower > 0)
            {
                changeInfoBoxAbilityTextGroup.transform.Find("DefPowerText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.defPower + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.SetActive(true);
                //
                if ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = "+" + ((int)((invenThings.stat.defPower + tmprein * 1.5f )- curInventoryThings.stat.defPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = ((int)((invenThings.stat.defPower + tmprein * 1.5f) - curInventoryThings.stat.defPower)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().color = red;
                }
            }
            if (invenThings.stat.evaRate > 0)
            {
                changeInfoBoxAbilityTextGroup.transform.Find("EvaRateText").gameObject.SetActive(true);
                changeInfoBoxAbilityTextGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.evaRate + tmprein * 1.5f)).ToString();

                changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.SetActive(true);
                //
                if ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) > 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = "+" + ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().color = green;
                }
                else if ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) == 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().color = defaultColor;
                }
                else if ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate) < 0)
                {
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = ((int)((curInventoryThings.stat.evaRate + tmprein * 1.5f) - curInventoryThings.stat.evaRate)).ToString();
                    changeInfoBoxAdditionGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().color = red;
                }
            }
            changeInfoBoxAdditionGroup.transform.gameObject.SetActive(true);
        }
        else {
            changeInfoBoxAbilityTextGroup.transform.Find("DpsText").gameObject.GetComponent<Text>().text = ((int)dps).ToString();
            changeInfoBoxAbilityTextGroup.transform.Find("StrPowerText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.strPower + tmprein * 1.5f)).ToString();
            changeInfoBoxAbilityTextGroup.transform.Find("AttackSpeedText").gameObject.GetComponent<Text>().text = (curInventoryThings.stat.attackSpeed + tmprein * 1.5f).ToString();
            changeInfoBoxAbilityTextGroup.transform.Find("CriticalText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.critical + tmprein * 1.5f)).ToString();
            changeInfoBoxAbilityTextGroup.transform.Find("FocusText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.focus + tmprein * 1.5f)).ToString();
            changeInfoBoxAbilityTextGroup.transform.Find("DefPowerText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.defPower + tmprein * 1.5f)).ToString();
            changeInfoBoxAbilityTextGroup.transform.Find("EvaRateText").gameObject.GetComponent<Text>().text = ((int)(curInventoryThings.stat.evaRate + tmprein * 1.5f)).ToString();



            changeInfoBoxAdditionGroup.transform.gameObject.SetActive(false);
        }


        changeItemBoxIcon.transform.gameObject.SetActive(true);
        changeItemBoxNameText.transform.gameObject.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(true);
        changeInfoBoxAbilityTextGroup.transform.gameObject.SetActive(true);
        changeInfoBoxExpSlider.value = sliderexp;
        changeInfoBoxExpText.text = sliderexp + "%";

        if (tmprein > 0 && curInventoryThings.exp + tmpexp == 100)
        {
            changeInfoBoxExpSlider.value = changeInfoBoxExpSlider.maxValue;
            changeInfoBoxExpText.text = "100%";
        }


        selectNumText.text = "재료 선택 " + selectNum + " / 10";


        obj.GetComponent<Button>().onClick.RemoveAllListeners();
        obj.GetComponent<Button>().onClick.AddListener(
            () => {
                StartCoroutine( SelectItem(obj, invenThings));
            });

    }





    //강화
    public void Reinforce(InventoryThings equipThings)
    {
        ReinforceButton.onClick.RemoveAllListeners();
        //수치 변화
        Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == curInventoryThings.name);

        if (tmprein > 0)
        {
            if (curInventoryThings.stat.strPower > 0) curInventoryThings.stat.strPower = (int)(curInventoryThings.stat.strPower + tmprein * 1.5f);
            if (curInventoryThings.stat.attackSpeed > 0) curInventoryThings.stat.attackSpeed = (float)curInventoryThings.stat.attackSpeed + tmprein * 1.5f;
            if (curInventoryThings.stat.focus > 0) curInventoryThings.stat.focus = (int)(curInventoryThings.stat.focus + tmprein * 1.5f);
            if (curInventoryThings.stat.critical > 0) curInventoryThings.stat.critical = (int)(curInventoryThings.stat.critical + tmprein * 1.5f);
            if (curInventoryThings.stat.defPower > 0) curInventoryThings.stat.defPower = (int)(curInventoryThings.stat.defPower+ tmprein * 1.5f);
            if (curInventoryThings.stat.evaRate > 0) curInventoryThings.stat.evaRate = (int)(curInventoryThings.stat.evaRate + tmprein * 1.5f);

            if (curInventoryThings.stat.strPower > 0)
            {
                curInventoryThings.stat.dps = curInventoryThings.stat.strPower * (float)curInventoryThings.stat.attackSpeed * curInventoryThings.stat.critical;
            }
            else if (curInventoryThings.stat.defPower > 0)
            {
                curInventoryThings.stat.dps = curInventoryThings.stat.defPower * curInventoryThings.stat.evaRate;
            }
            curInventoryThings.reinforcement += tmprein;

            success.transform.Find("LevelText").gameObject.GetComponent<Text>().text = "+" + curInventoryThings.reinforcement.ToString();
            appear();
        }
        curInventoryThings.exp += tmpexp;
        if (curInventoryThings.exp >= 100) curInventoryThings.exp %= curInventoryThings.exp;

        //재료 삭제
        for (int i = 0; i < selectInvenList.Count; i++)
        {
            selectInvenList[i].possession = 0;
        }

        Mercenary merTemp = new Mercenary();
        //선택된 캐릭터 구별
        if (profileManager.getCurChr() != Player.instance.getUser().Name)
            merTemp = MercenaryData.instance.getMercenary().Find(x => x.getName() == profileManager.getCurChr());

        ////이전 전투력
        //Stat preStat = new Stat();
        //if (profileManager.getCurChr() == Player.instance.getUser().Name)
        //    preStat = GameObject.Find("PlayerManager").GetComponent<StatData>().getPlayerStat()[GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().getCurSetNum() - 1];
        //else preStat = GameObject.Find("PlayerManager").GetComponent<StatData>().getMercenaryStat(merTemp.getMer_no())[GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().getCurSetNum() - 1];


        //전투력 계산
        GameObject.Find("PlayerManager").GetComponent<StatData>().playerStatCal();
        GameObject.Find("PlayerManager").GetComponent<StatData>().mercenaryStatCal();
        GameObject.Find("PlayerManager").GetComponent<StatData>().repreSetStatCal();

        //초기화
        tmprein = 0;
        tmpexp = 0;
        selectNum = 0;
        selectObjList.Clear();
        selectInvenList.Clear();

        ReinforceEquip(curInventoryThings);
        GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(curInventoryThings);
        GameObject.Find("InventoryScript").GetComponent<Inventory>().ItemSlotCreate();
        if (GameObject.Find("Menu").transform.Find("ProfilePopup").gameObject.activeInHierarchy)
        {
            //강화 수치
            if (GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipHelmet.reinforcement > 0) GameObject.Find("EquipHelmet/AmountText").GetComponent<Text>().text = "+" + GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipHelmet.reinforcement;
            else GameObject.Find("EquipHelmet/AmountText").GetComponent<Text>().text = null;
            if (GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipArmor.reinforcement > 0) GameObject.Find("EquipArmor/AmountText").GetComponent<Text>().text = "+" + GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipArmor.reinforcement;
            else GameObject.Find("EquipArmor/AmountText").GetComponent<Text>().text = null;
            if (GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipWeapon.reinforcement > 0) GameObject.Find("EquipWeapon/AmountText").GetComponent<Text>().text = "+" + GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipWeapon.reinforcement;
            else GameObject.Find("EquipWeapon/AmountText").GetComponent<Text>().text = null;
            if (GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipBoots.reinforcement > 0) GameObject.Find("EquipBoots/AmountText").GetComponent<Text>().text = "+" + GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipBoots.reinforcement;
            else GameObject.Find("EquipBoots/AmountText").GetComponent<Text>().text = null;
            if (GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipGloves.reinforcement > 0) GameObject.Find("EquipGloves/AmountText").GetComponent<Text>().text = "+" + GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipGloves.reinforcement;
            else GameObject.Find("EquipGloves/AmountText").GetComponent<Text>().text = null;
            if (GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipPants.reinforcement > 0) GameObject.Find("EquipPants/AmountText").GetComponent<Text>().text = "+" + GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().equipPants.reinforcement;
            else GameObject.Find("EquipPants/AmountText").GetComponent<Text>().text = null;


            if (profileManager.getCurChr() == Player.instance.getUser().Name)
            {
                GameObject.Find("LevelText").GetComponent<Text>().text = Player.instance.getUser().level.ToString();
                GameObject.Find("PlayerNameText").GetComponent<Text>().text = Player.instance.getUser().Name;
                Stat stat = GameObject.Find("PlayerManager").GetComponent<StatData>().getPlayerStat()[GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().getCurSetNum() - 1];
                GameObject.Find("DPS/Text").GetComponent<Text>().text = ((int)stat.dps).ToString();
                GameObject.Find("StrPower/Text").GetComponent<Text>().text = ((int)stat.strPower).ToString(); //equi[setnum].strPower
                GameObject.Find("AttackSpeed/Text").GetComponent<Text>().text = (stat.attackSpeed).ToString();
                GameObject.Find("Focus/Text").GetComponent<Text>().text = ((int)stat.focus).ToString();
                GameObject.Find("Critical/Text").GetComponent<Text>().text = ((int)stat.critical).ToString();
                GameObject.Find("DefPower/Text").GetComponent<Text>().text = ((int)stat.defPower).ToString();
                GameObject.Find("EvaRate/Text").GetComponent<Text>().text = ((int)stat.evaRate).ToString();
                GameObject.Find("Attribute/Text").GetComponent<Text>().text = Player.instance.getUser().attribute;
                //GameObject.Find("System").transform.Find("DPSEff").gameObject.GetComponent<ChangeDPSManager>().changeDPS((int)preStat.dps, (int)stat.dps);
            }
            else
            {

                GameObject.Find("LevelText").GetComponent<Text>().text = merTemp.level.ToString();
                GameObject.Find("PlayerNameText").GetComponent<Text>().text = merTemp.getName();
                Stat stat = GameObject.Find("PlayerManager").GetComponent<StatData>().getMercenaryStat(merTemp.getMer_no())[GameObject.Find("PlayerManager").GetComponent<ProfilePopupManager>().getCurSetNum() - 1];
                GameObject.Find("DPS/Text").GetComponent<Text>().text = stat.dps.ToString();
                GameObject.Find("StrPower/Text").GetComponent<Text>().text = stat.strPower.ToString();
                GameObject.Find("AttackSpeed/Text").GetComponent<Text>().text = stat.attackSpeed.ToString();
                GameObject.Find("Focus/Text").GetComponent<Text>().text = stat.focus.ToString();
                GameObject.Find("Critical/Text").GetComponent<Text>().text = stat.critical.ToString();
                GameObject.Find("DefPower/Text").GetComponent<Text>().text = stat.defPower.ToString();
                GameObject.Find("EvaRate/Text").GetComponent<Text>().text = stat.evaRate.ToString();
                GameObject.Find("Attribute/Text").GetComponent<Text>().text = merTemp.attribute;
                //GameObject.Find("System").transform.Find("DPSEff").gameObject.GetComponent<ChangeDPSManager>().changeDPS((int)preStat.dps, (int)stat.dps);
            }



        }




    }







    //인벤토리 생성
    void createInventory(Equipment equip)
    {
        List<InventoryThings> invenThingsList = ThingsData.instance.getInventoryThingsList().FindAll(x
            => x.type == ThingsData.instance.getThingsList().Find(y => y.name == equip.name).type);
        for (int i = 0; i < inventoryThings.Count; i++) Destroy(inventoryThings[i]);
        inventoryThings.Clear();
        bool nothing = false;
        for (int i = 0; i < invenThingsList.Count; i++)
        {
            if (!invenThingsList[i].equip && invenThingsList[i].possession > 0 && invenThingsList[i] != curInventoryThings)
            {
                nothing = true;
                //
                inventoryThings.Add(Instantiate(inventoryBox));
                inventoryThings[inventoryThings.Count - 1].SetActive(true);
                inventoryThings[inventoryThings.Count - 1].transform.SetParent(inventoryPanel.transform);
                inventoryThings[inventoryThings.Count - 1].GetComponent<RectTransform>().localScale = Vector3.one;
                inventoryThings[inventoryThings.Count - 1].GetComponent<RectTransform>().localPosition = Vector3.one;
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == invenThingsList[i].name).grade);
                inventoryThings[inventoryThings.Count - 1].transform.Find("GradeFrame").gameObject.GetComponent<Image>().color = col;

                inventoryThings[inventoryThings.Count - 1].transform.Find("Item/Icon").gameObject.GetComponent<Image>().sprite
                    = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == invenThingsList[i].name).icon);
                //강화 수치 있는 경우
                if (invenThingsList[i].reinforcement > 0)
                {
                    inventoryThings[inventoryThings.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = invenThingsList[i].reinforcement.ToString();
                    inventoryThings[inventoryThings.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(true);
                }
                //강화 수치 없는 경우
                else inventoryThings[inventoryThings.Count - 1].transform.Find("Item/AmountText").gameObject.SetActive(false);

                //선택
                int objNum = inventoryThings.Count - 1;
                int num = i;
                inventoryThings[inventoryThings.Count - 1].GetComponent<Button>().onClick.RemoveAllListeners();
                inventoryThings[inventoryThings.Count - 1].GetComponent<Button>().onClick.AddListener(
                    () => {
                        StartCoroutine( SelectItem(inventoryThings[objNum], invenThingsList[num]));

                    });
            }
        }

        if (!nothing)
        {
            inventoryUIPanel.transform.Find("NothingText").gameObject.GetComponent<Text>().text = "강화에 필요한 재료가 없습니다.";
            inventoryUIPanel.transform.Find("NothingText").gameObject.SetActive(true);
        }
        else inventoryUIPanel.transform.Find("NothingText").gameObject.SetActive(false);
    }



    public void appear()
    {
        success.SetActive(true);
        //StartCoroutine(disappear());
    }


    //IEnumerator disappear()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    success.SetActive(false);
    //}





}
