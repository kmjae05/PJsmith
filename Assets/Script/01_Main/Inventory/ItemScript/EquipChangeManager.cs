using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipChangeManager : MonoBehaviour
{
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
    private GameObject InventoryBox;
    private Image inventoryBoxGradeFrame;
    private Image inventoryBoxIcon;
    private Text inventoryAmount;
    private GameObject selectImage;


    private InventoryThings curInventoryThings;     //현재 선택된 장비
    private string curType;                         //현재 선택된 장비 종류








}
