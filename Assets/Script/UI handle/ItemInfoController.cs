using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemInfoController : MonoBehaviour {

    GameObject ItemInfoPopup;

    GameObject Icon;
    GameObject AmountText;
    GameObject CostText;

    public class InfoBox_Parts
    {
        public GameObject InfoBox_PartsPopup;
        public Text ItemNameText;
        public Text ItemInfoText;
        public Text TipText;
        public Text ItemType;
    }
    public class InfoBox_Sword
    {
        public GameObject InfoBox_SwordPopup;
        public Text ItemNameText;
        public Text ItemInfoText;
        public Text TipText;
        public Text ItemType;

        public Text DPS;
    }
    public class InfoBox_Hammer
    {
        public GameObject InfoBox_HammerPopup;
        public Text ItemNameText;
        public Text ItemInfoText;
        public Text ItemType;

        public Text DPS;
        public Text AttackSpeed;
        public Text Focus;
        public Text Critical;

        public Text SkillTitle;
        public Text SkillInfo;
        public Sprite SkillIcon;
        public Text SkillCooltime;
    }

    InfoBox_Parts InfoParts;
    InfoBox_Sword InfoSword;
    InfoBox_Hammer InfoHammer;

    void Awake()
    {
        ItemInfoPopup = transform.Find("/02_UI/System/ItemInfoPopup").gameObject;

        InfoParts.InfoBox_PartsPopup = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Parts").gameObject;
        InfoParts.ItemNameText = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Parts/ItemNameText").gameObject.GetComponent<Text>();
        InfoParts.ItemInfoText = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Parts/ItemInfoText").gameObject.GetComponent<Text>();
        InfoParts.TipText = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Parts/TipText").gameObject.GetComponent<Text>();
        InfoParts.ItemType = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Parts/ItemType").gameObject.GetComponent<Text>();


        InfoSword.InfoBox_SwordPopup = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Sword").gameObject;
        InfoSword.ItemNameText = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Sword/ItemNameText").gameObject.GetComponent<Text>();
        InfoSword.ItemInfoText = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Sword/ItemInfoText").gameObject.GetComponent<Text>();
        InfoSword.TipText = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Sword/TipText").gameObject.GetComponent<Text>();
        InfoSword.ItemType = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Sword/ItemType").gameObject.GetComponent<Text>();
        InfoSword.DPS = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Sword/DPS").gameObject.GetComponent<Text>();

        InfoHammer.InfoBox_HammerPopup = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer").gameObject;
        InfoHammer.ItemNameText = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/ItemNameText").gameObject.GetComponent<Text>();
        InfoHammer.ItemInfoText = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/ItemInfoText").gameObject.GetComponent<Text>();
        InfoHammer.ItemType = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/ItemType").gameObject.GetComponent<Text>();

        InfoHammer.DPS = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/DPS/Text").gameObject.GetComponent<Text>();
        InfoHammer.AttackSpeed = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/AttackSpeed/Text").gameObject.GetComponent<Text>();
        InfoHammer.Focus = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/Focus/Text").gameObject.GetComponent<Text>();
        InfoHammer.Critical = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/Critical/Text").gameObject.GetComponent<Text>();

        InfoHammer.SkillTitle = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/Skill/SkillTitle").gameObject.GetComponent<Text>();
        InfoHammer.SkillInfo = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/Skill/SkillInfo").gameObject.GetComponent<Text>();
        InfoHammer.SkillIcon = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/Skill/SkillIcon/Img").gameObject.GetComponent<Image>().sprite;
        InfoHammer.SkillCooltime = transform.Find("/02_UI/System/ItemInfoPopup/UIPanel/InfoBox_Hammer/Skill/SkillCooltime").gameObject.GetComponent<Text>();
    }

    void Start()
    {


    }
    void Update()
    {

    }
}
