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
    private Text changeInfoBoxAbilityInfoText;
    private Text changeInfoBoxAbilityText;

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
    private int tmprein= 0;             //미리보기 강화 수치
    private int selectNum = 0;          //선택 개수
    private List<GameObject> selectObjList = new List<GameObject>();
    private List<InventoryThings> selectInvenList = new List<InventoryThings>();

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
        changeInfoBoxAbilityInfoText = changeInfoBox.transform.Find("AbilityInfoText").gameObject.GetComponent<Text>();
        changeInfoBoxAbilityText = changeInfoBox.transform.Find("AbilityText").gameObject.GetComponent<Text>();

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

        curInventoryThings = new InventoryThings();
        curType = null;

        Button closeButton = inventoryUIPanel.transform.Find("CloseButton").gameObject.GetComponent<Button>();
        closeButton.onClick.AddListener(() =>
       {
           for (int i = 0; i < inventoryThings.Count; i++) Destroy(inventoryThings[i]);
           inventoryThings.Clear();
       });

    }

    //강화
    public void ReinforceEquip(InventoryThings equipThings)
    {
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

        if (equip.stat.dps > 0) { abstr += "전투력\n"; statstr += equip.stat.dps + "\n"; }
        if (equip.stat.strPower > 0) { abstr += "공격력\n"; statstr += equip.stat.strPower + "\n"; }
        if (equip.stat.attackSpeed > 0) { abstr += "공격속도\n"; statstr += equip.stat.attackSpeed + "\n"; }
        if (equip.stat.focus > 0) { abstr += "명중률\n"; statstr += equip.stat.focus + "\n"; }
        if (equip.stat.critical > 0) { abstr += "크리티컬\n"; statstr += equip.stat.critical + "\n"; }
        if (equip.stat.defPower > 0) { abstr += "방어력\n"; statstr += equip.stat.defPower + "\n"; }
        if (equip.stat.evaRate > 0) { abstr += "회피율\n"; statstr += equip.stat.evaRate + "\n"; }
        if (equip.attribute != null) { abstr += "속성"; statstr += equip.attribute.ToString(); }
        curInfoBoxAbilityInfoText.text = abstr;
        curInfoBoxAbilityText.text = statstr;


        //강화 후 장비
        changeItemBoxGradeFrame.color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipThings.name).grade);
        changeItemBoxIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == equipThings.name).icon);
        changeItemBoxNameText.text = equipThings.name;
        if (equipThings.reinforcement > 0)
            changeInfoBoxReinText.text = "+" + equipThings.reinforcement.ToString();
        else changeInfoBoxReinText.text = null;

        changeInfoBoxAbilityInfoText.text = abstr;
        changeInfoBoxAbilityText.text = statstr;

        changeItemBoxIcon.transform.gameObject.SetActive(true);
        changeItemBoxNameText.transform.gameObject.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(true);
        changeInfoBoxAbilityText.transform.gameObject.SetActive(true);
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
        if(tmpexp + invenThings.exp >= 100)
        {
            tmpexp = 0;
            tmprein += 1;
        }

        //밑에 장비 표시
        if (invenThings.reinforcement > 0)
            changeInfoBoxReinText.text = invenThings.reinforcement.ToString();
        else changeInfoBoxReinText.text = null;

        string abstr = "";
        string statstr = "";
        Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == curInventoryThings.name);

        float dps = 0;
        if (equip.stat.strPower > 0)
        {
            dps = (equip.stat.strPower * tmprein * 1.5f) * ((float)equip.stat.attackSpeed * tmprein * 1.5f) * (equip.stat.critical * tmprein * 1.5f);
        }
        else if(equip.stat.defPower > 0)
        {
            dps = (equip.stat.defPower * tmprein * 1.5f) * ((float)equip.stat.evaRate * tmprein * 1.5f) ;
        }
        if (dps == 0)
        {
            //기본 스탯
            dps = equip.stat.dps;
            if (equip.stat.dps > 0) { abstr += "전투력\n"; statstr += (int)dps + "\n"; }
            if (equip.stat.strPower > 0) { abstr += "공격력\n"; statstr += (int)(equip.stat.strPower ) + "\n"; }
            if (equip.stat.attackSpeed > 0) { abstr += "공격속도\n"; statstr += (equip.stat.attackSpeed) + "\n"; }
            if (equip.stat.focus > 0) { abstr += "명중률\n"; statstr += (int)(equip.stat.focus ) + "\n"; }
            if (equip.stat.critical > 0) { abstr += "크리티컬\n"; statstr += (int)(equip.stat.critical ) + "\n"; }
            if (equip.stat.defPower > 0) { abstr += "방어력\n"; statstr += (int)(equip.stat.defPower ) + "\n"; }
            if (equip.stat.evaRate > 0) { abstr += "회피율\n"; statstr += (int)(equip.stat.evaRate ) + "\n"; }
            if (equip.attribute != null) { abstr += "속성"; statstr += equip.attribute.ToString(); }
        }
        else
        {
            if (equip.stat.dps > 0) { abstr += "전투력\n"; statstr += (int)dps + "\n"; }
            if (equip.stat.strPower > 0) { abstr += "공격력\n"; statstr += (int)(equip.stat.strPower * tmprein * 1.5f) + "\n"; }
            if (equip.stat.attackSpeed > 0) { abstr += "공격속도\n"; statstr += (equip.stat.attackSpeed * tmprein * 1.5f) + "\n"; }
            if (equip.stat.focus > 0) { abstr += "명중률\n"; statstr += (int)(equip.stat.focus * tmprein * 1.5f) + "\n"; }
            if (equip.stat.critical > 0) { abstr += "크리티컬\n"; statstr += (int)(equip.stat.critical * tmprein * 1.5f) + "\n"; }
            if (equip.stat.defPower > 0) { abstr += "방어력\n"; statstr += (int)(equip.stat.defPower * tmprein * 1.5f) + "\n"; }
            if (equip.stat.evaRate > 0) { abstr += "회피율\n"; statstr += (int)(equip.stat.evaRate * tmprein * 1.5f) + "\n"; }
            if (equip.attribute != null) { abstr += "속성"; statstr += equip.attribute.ToString(); }
        }
        
        changeInfoBoxAbilityInfoText.text = abstr;
        changeInfoBoxAbilityText.text = statstr;
        if (curInventoryThings.reinforcement > 0)
            changeInfoBoxReinText.text = "+" + (tmprein + curInventoryThings.reinforcement).ToString();
        else changeInfoBoxReinText.text = null;

        changeItemBoxIcon.transform.gameObject.SetActive(true);
        changeItemBoxNameText.transform.gameObject.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(true);
        changeInfoBoxAbilityText.transform.gameObject.SetActive(true);

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
        if (tmpexp + invenThings.exp < 0)
        {
            //
            tmpexp = 90;
            tmprein -= 1;
        }


        //밑에 장비 표시
        if (invenThings.reinforcement > 0)
            changeInfoBoxReinText.text = invenThings.reinforcement.ToString();
        else changeInfoBoxReinText.text = null;

        string abstr = "";
        string statstr = "";
        Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == curInventoryThings.name);

        float dps = 0;
        if (equip.stat.strPower > 0)
        {
            dps = (equip.stat.strPower * tmprein * 1.5f) * ((float)equip.stat.attackSpeed * tmprein * 1.5f) * (equip.stat.critical * tmprein * 1.5f);
        }
        else if (equip.stat.defPower > 0)
        {
            dps = (equip.stat.defPower * tmprein * 1.5f) * ((float)equip.stat.evaRate * tmprein * 1.5f);
        }
        if (dps == 0)
        {
            //기본 스탯
            dps = equip.stat.dps;
            if (equip.stat.dps > 0) { abstr += "전투력\n"; statstr += (int)dps + "\n"; }
            if (equip.stat.strPower > 0) { abstr += "공격력\n"; statstr += (int)(equip.stat.strPower) + "\n"; }
            if (equip.stat.attackSpeed > 0) { abstr += "공격속도\n"; statstr += (equip.stat.attackSpeed) + "\n"; }
            if (equip.stat.focus > 0) { abstr += "명중률\n"; statstr += (int)(equip.stat.focus) + "\n"; }
            if (equip.stat.critical > 0) { abstr += "크리티컬\n"; statstr += (int)(equip.stat.critical) + "\n"; }
            if (equip.stat.defPower > 0) { abstr += "방어력\n"; statstr += (int)(equip.stat.defPower) + "\n"; }
            if (equip.stat.evaRate > 0) { abstr += "회피율\n"; statstr += (int)(equip.stat.evaRate) + "\n"; }
            if (equip.attribute != null) { abstr += "속성"; statstr += equip.attribute.ToString(); }
        }
        else
        {
            if (equip.stat.dps > 0) { abstr += "전투력\n"; statstr += (int)dps + "\n"; }
            if (equip.stat.strPower > 0) { abstr += "공격력\n"; statstr += (int)(equip.stat.strPower * tmprein * 1.5f) + "\n"; }
            if (equip.stat.attackSpeed > 0) { abstr += "공격속도\n"; statstr += (equip.stat.attackSpeed * tmprein * 1.5f) + "\n"; }
            if (equip.stat.focus > 0) { abstr += "명중률\n"; statstr += (int)(equip.stat.focus * tmprein * 1.5f) + "\n"; }
            if (equip.stat.critical > 0) { abstr += "크리티컬\n"; statstr += (int)(equip.stat.critical * tmprein * 1.5f) + "\n"; }
            if (equip.stat.defPower > 0) { abstr += "방어력\n"; statstr += (int)(equip.stat.defPower * tmprein * 1.5f) + "\n"; }
            if (equip.stat.evaRate > 0) { abstr += "회피율\n"; statstr += (int)(equip.stat.evaRate * tmprein * 1.5f) + "\n"; }
            if (equip.attribute != null) { abstr += "속성"; statstr += equip.attribute.ToString(); }
        }

        changeInfoBoxAbilityInfoText.text = abstr;
        changeInfoBoxAbilityText.text = statstr;
        if (curInventoryThings.reinforcement > 0)
            changeInfoBoxReinText.text = "+" + (tmprein + curInventoryThings.reinforcement).ToString();
        else changeInfoBoxReinText.text = null;

        changeItemBoxIcon.transform.gameObject.SetActive(true);
        changeItemBoxNameText.transform.gameObject.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(true);
        changeInfoBoxAbilityText.transform.gameObject.SetActive(true);



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

    }







    //인벤토리 생성
    void createInventory(Equipment equip)
    {
        List<InventoryThings> invenThingsList = ThingsData.instance.getInventoryThingsList().FindAll(x
            => x.type == ThingsData.instance.getThingsList().Find(y => y.name == equip.name).type);
        for (int i = 0; i < inventoryThings.Count; i++) Destroy(inventoryThings[i]);
        inventoryThings.Clear();

        for (int i = 0; i < invenThingsList.Count; i++)
        {
            if (!invenThingsList[i].equip && invenThingsList[i].possession > 0)
            {
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
    }
}
