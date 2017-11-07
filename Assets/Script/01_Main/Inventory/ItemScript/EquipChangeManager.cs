using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipChangeManager : MonoBehaviour
{
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

    private int selectNum;          //선택 개수

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


    //교체
    public void ChangeEquip(InventoryThings equipThings)
    {
        ChangeButton.transform.gameObject.SetActive(false);
        ReinforceButton.transform.gameObject.SetActive(true);

        curInventoryThings = equipThings;

        TitleText.text = "장비 교체";
        curEquipBoxInfoText.text = "";
        changeEquipBoxInfoText.text = "";

        curItemBoxGradeFrame.color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == equipThings.name).grade);
        curItemBoxIcon.sprite = Resources.Load < Sprite > (ThingsData.instance.getThingsList().Find(x => x.name == equipThings.name).icon);
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


        //교체할 장비
        changeItemBoxIcon.transform.gameObject.SetActive(false);
        changeItemBoxNameText.transform.gameObject.SetActive(false);
        changeInfoBox.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(false);
        changeInfoBoxAbilityInfoText.text = "장비를 선택하세요.";
        changeInfoBoxAbilityText.transform.gameObject.SetActive(false);
        changeItemBoxGradeFrame.color = new Color(1, 1, 1);


        selectNumText.transform.gameObject.SetActive(false);

        createInventory(equip);

        equipSelectPopup.SetActive(true);
    }


    //교체 아이템 클릭 (1개 한정 (클릭 해제 없음
    //하나 선택하면 나머지 선택 해제
    void SelectChangeItem(GameObject obj, InventoryThings invenThings)
    {
        for (int i = 0; i < inventoryThings.Count; i++) { inventoryThings[i].transform.Find("SelectImage").gameObject.SetActive(false); }
        obj.transform.Find("SelectImage").gameObject.SetActive(true);

        //밑에 장비 표시
        changeItemBoxGradeFrame.color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == invenThings.name).grade);
        changeItemBoxIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == invenThings.name).icon);
        changeItemBoxNameText.text = invenThings.name;
        if (invenThings.reinforcement > 0)
            changeInfoBoxReinText.text = "+" + invenThings.reinforcement.ToString();
        else changeInfoBoxReinText.text = null;

        string abstr = "";
        string statstr = "";
        Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == invenThings.name);
        //강화 수치에 따라 스탯 계산 다시 해줘야 함.

        if (equip.stat.dps > 0) { abstr += "전투력\n"; statstr += equip.stat.dps + "\n"; }
        if (equip.stat.strPower > 0) { abstr += "공격력\n"; statstr += equip.stat.strPower + "\n"; }
        if (equip.stat.attackSpeed > 0) { abstr += "공격속도\n"; statstr += equip.stat.attackSpeed + "\n"; }
        if (equip.stat.focus > 0) { abstr += "명중률\n"; statstr += equip.stat.focus + "\n"; }
        if (equip.stat.critical > 0) { abstr += "크리티컬\n"; statstr += equip.stat.critical + "\n"; }
        if (equip.stat.defPower > 0) { abstr += "방어력\n"; statstr += equip.stat.defPower + "\n"; }
        if (equip.stat.evaRate > 0) { abstr += "회피율\n"; statstr += equip.stat.evaRate + "\n"; }
        if (equip.attribute != null) { abstr += "속성"; statstr += equip.attribute.ToString(); }
        changeInfoBoxAbilityInfoText.text = abstr;
        changeInfoBoxAbilityText.text = statstr;

        changeItemBoxIcon.transform.gameObject.SetActive(true);
        changeItemBoxNameText.transform.gameObject.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(true);
        changeInfoBoxAbilityText.transform.gameObject.SetActive(true);


        //버튼 
        ChangeButton.onClick.RemoveAllListeners();
        ChangeButton.onClick.AddListener(() => changeButton(invenThings));
    }


    //교체하기 버튼
    void changeButton(InventoryThings invenThings)
    {
        InventoryThings beforeThings = curInventoryThings;

        Mercenary merTemp = new Mercenary();

        //선택된 캐릭터 구별
        //profileManager.getCurChr();
        if (profileManager.getCurChr() != Player.instance.getUser().Name)
            merTemp = MercenaryData.instance.getMercenary().Find(x => x.getName() == profileManager.getCurChr());

        //선택한 장비 착용상태, 세트번호, 용병 이름 부여.
        curInventoryThings = invenThings;
        //무기
        if(curInventoryThings.type == "Weapon")
        {
            if(profileManager.getCurChr() == Player.instance.getUser().Name)
            {
                Player.instance.getUser().equipWeapon[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = Player.instance.getUser().Name;
            }
            else
            {
                merTemp.equipWeapon[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = merTemp.getName();
            }
            curInventoryThings.equip = true;                     

            curInventoryThings.equipSetNum = beforeThings.equipSetNum;

            GameObject.Find("EquipWeapon/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).icon);
            GameObject.Find("EquipWeapon/Text").GetComponent<Text>().text = curInventoryThings.name;
            GameObject.Find("EquipWeapon/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).grade);
            //장비 인포 버튼
            GameObject.Find("EquipWeapon").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipWeapon").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(curInventoryThings);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                    () => {
                        GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(curInventoryThings);
                    });
            });
        }
        //갑옷
        if (curInventoryThings.type == "Armor")
        {
            if (profileManager.getCurChr() == "SmithSelect")
            {
                Player.instance.getUser().equipArmor[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = Player.instance.getUser().Name;
            }
            else
            {
                merTemp.equipArmor[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = merTemp.getName();
            }
            curInventoryThings.equip = true;
            curInventoryThings.equipSetNum = beforeThings.equipSetNum;

            GameObject.Find("EquipArmor/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).icon);
            GameObject.Find("EquipArmor/Text").GetComponent<Text>().text = curInventoryThings.name;
            GameObject.Find("EquipArmor/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).grade);
            //장비 인포 버튼
            GameObject.Find("EquipArmor").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipArmor").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(curInventoryThings);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                    () => {
                        GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(curInventoryThings);
                    });
            });
        }
        //바지
        if (curInventoryThings.type == "Pants")
        {
            if (profileManager.getCurChr() == "SmithSelect")
            {
                Player.instance.getUser().equipPants[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = Player.instance.getUser().Name;
            }
            else
            {
                merTemp.equipPants[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = merTemp.getName();
            }
            curInventoryThings.equip = true;
            curInventoryThings.equipSetNum = beforeThings.equipSetNum;

            GameObject.Find("EquipPants/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).icon);
            GameObject.Find("EquipPants/Text").GetComponent<Text>().text = curInventoryThings.name;
            GameObject.Find("EquipPants/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).grade);
            //장비 인포 버튼
            GameObject.Find("EquipPants").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipPants").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(curInventoryThings);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                    () => {
                        GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(curInventoryThings);
                    });
            });
        }
        //헬멧
        if (curInventoryThings.type == "Helmet")
        {
             if (profileManager.getCurChr() == "SmithSelect")
                {
                Player.instance.getUser().equipHelmet[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = Player.instance.getUser().Name;
            }
            else
            {
                merTemp.equipHelmet[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = merTemp.getName();
            }
            curInventoryThings.equip = true;
            curInventoryThings.equipSetNum = beforeThings.equipSetNum;

            GameObject.Find("EquipHelmet/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).icon);
            GameObject.Find("EquipHelmet/Text").GetComponent<Text>().text = curInventoryThings.name;
            GameObject.Find("EquipHelmet/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).grade);
            //장비 인포 버튼
            GameObject.Find("EquipHelmet").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipHelmet").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(curInventoryThings);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                    () => {
                        GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(curInventoryThings);
                    });
            });
        }
        //장갑
        if (curInventoryThings.type == "Gloves")
        {
            if (profileManager.getCurChr() == "SmithSelect")
            {
                Player.instance.getUser().equipGloves[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = Player.instance.getUser().Name;
            }
            else
            {
                merTemp.equipGloves[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = merTemp.getName();
            }
            curInventoryThings.equip = true;
            curInventoryThings.equipSetNum = beforeThings.equipSetNum;

            GameObject.Find("EquipGloves/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).icon);
            GameObject.Find("EquipGloves/Text").GetComponent<Text>().text = curInventoryThings.name;
            GameObject.Find("EquipGloves/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).grade);
            //장비 인포 버튼
            GameObject.Find("EquipGloves").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipGloves").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(curInventoryThings);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                    () => {
                        GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(curInventoryThings);
                    });
            });
        }
        //부츠
        if (curInventoryThings.type == "Boots")
        {
            if (profileManager.getCurChr() == "SmithSelect")
            {
                Player.instance.getUser().equipBoots[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = Player.instance.getUser().Name;
            }
            else
            {
                merTemp.equipBoots[beforeThings.equipSetNum - 1] = equipmentData.getEquipmentList().Find(x => x.name == curInventoryThings.name);
                curInventoryThings.equipChrName = merTemp.getName();
            }
            curInventoryThings.equip = true;
            curInventoryThings.equipSetNum = beforeThings.equipSetNum;

            GameObject.Find("EquipBoots/Image").GetComponent<Image>().sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).icon);
            GameObject.Find("EquipBoots/Text").GetComponent<Text>().text = curInventoryThings.name;
            GameObject.Find("EquipBoots/GradeFrame").GetComponent<Image>().color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).grade);
            //장비 인포 버튼
            GameObject.Find("EquipBoots").GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject.Find("EquipBoots").GetComponent<Button>().onClick.AddListener(() => {
                GameObject.Find("InventoryScript").GetComponent<Inventory>().EquipInfoPopup(curInventoryThings);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.SetActive(true);
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ChangeButton").gameObject.GetComponent<Button>().onClick.AddListener(
                    () => {
                        GameObject.Find("PlayerManager").GetComponent<EquipChangeManager>().ChangeEquip(curInventoryThings);
                    });
            });
        }


        //기존 장비 세트 번호, 착용 유무, 용병 이름 저장. 바꿔줌.
        beforeThings.equip = false;
        beforeThings.equipChrName = null;
        beforeThings.equipSetNum = 0;

        curItemBoxGradeFrame.color = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).grade);
        curItemBoxIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == curInventoryThings.name).icon);
        curItemBoxNameText.text = curInventoryThings.name;
        if (curInventoryThings.reinforcement > 0)
            curInfoBoxReinText.text = "+" + curInventoryThings.reinforcement.ToString();
        else curInfoBoxReinText.text = null;

        string abstr = "";
        string statstr = "";
        Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == curInventoryThings.name);
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

        //교체할 장비
        changeItemBoxGradeFrame.color = new Color(1, 1, 1);
        changeItemBoxIcon.transform.gameObject.SetActive(false);
        changeItemBoxNameText.transform.gameObject.SetActive(false);
        changeInfoBox.SetActive(true);
        changeInfoBoxReinText.transform.gameObject.SetActive(false);
        changeInfoBoxAbilityInfoText.text = "장비를 선택하세요.";
        changeInfoBoxAbilityText.transform.gameObject.SetActive(false);

        //인벤토리 변경
        createInventory(equip);

        //전투력 계산, 변동 효과
        GameObject.Find("PlayerManager").GetComponent<StatData>().playerStatCal();
        GameObject.Find("PlayerManager").GetComponent<StatData>().mercenaryStatCal();
        GameObject.Find("PlayerManager").GetComponent<StatData>().repreSetStatCal();

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

        }

    }


    //전투력 변동 효과




    //인벤토리 생성
    public void createInventory(Equipment equip)
    {
        List<InventoryThings> invenThingsList = ThingsData.instance.getInventoryThingsList().FindAll(x
            => x.type == ThingsData.instance.getThingsList().Find(y => y.name == equip.name).type);
        for (int i = 0; i < inventoryThings.Count; i++) Destroy(inventoryThings[i]);
        inventoryThings.Clear();
        for (int i = 0; i < invenThingsList.Count; i++)
        {
            if (!invenThingsList[i].equip && invenThingsList[i].possession>0)
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
                    inventoryThings[inventoryThings.Count - 1].transform.Find("Item/AmountText").gameObject.GetComponent<Text>().text = "+" + invenThingsList[i].reinforcement.ToString();
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
                        SelectChangeItem(inventoryThings[objNum], invenThingsList[num]);

                    });
            }
        }
    }

}
