using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryData : MonoBehaviour {

    public static MercenaryData instance = null;
    private MercenaryManager mercenaryManager;  //mercenary info


    static private List<Mercenary> mercenary;
    private Mercenary mercenaryCreate;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mercenaryManager = GameObject.Find("StageManager").GetComponent<MercenaryManager>();

        mercenary = new List<Mercenary>();

        //용병 A, B, C 생성.
        for (int i = 1; i < 4; i++)
        {
            mercenaryCreate = new Mercenary();
            if (i == 1) {
                mercenaryCreate.setName("A"); mercenaryCreate.setMer_no(1);
                mercenaryCreate.level = Player.Play.level;
                mercenaryCreate.dps = 100;
                mercenaryCreate.strPower = 100;
                mercenaryCreate.attackSpeed = 100;
                mercenaryCreate.focus = 100;
                mercenaryCreate.critical = 100;
                mercenaryCreate.defPower = 100;
                mercenaryCreate.evaRate = 100;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.collectSpeed = 100;
                mercenaryCreate.collectAmount = 100;
                //장비
                mercenaryCreate.equipHelmet[0] = "helmet8"; mercenaryCreate.equipHelmet[1] = "helmet7";
                mercenaryCreate.equipArmor[0] = "armor1"; mercenaryCreate.equipArmor[1] = "armor2";
                mercenaryCreate.equipWeapon[0] = "weapon2"; mercenaryCreate.equipWeapon[1] = "weapon8";
                mercenaryCreate.equipBoots[0] = "boots3"; mercenaryCreate.equipBoots[1] = "boots6";

            }
            if (i == 2) {
                mercenaryCreate.setName("B"); mercenaryCreate.setMer_no(2);
                mercenaryCreate.level = Player.Play.level;
                mercenaryCreate.dps = 100;
                mercenaryCreate.strPower = 100;
                mercenaryCreate.attackSpeed = 100;
                mercenaryCreate.focus = 100;
                mercenaryCreate.critical = 100;
                mercenaryCreate.defPower = 100;
                mercenaryCreate.evaRate = 100;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.collectSpeed = 100;
                mercenaryCreate.collectAmount = 100;
                mercenaryCreate.equipHelmet[0] = "helmet3"; mercenaryCreate.equipHelmet[1] = "helmet2";
                mercenaryCreate.equipArmor[0] = "armor3"; mercenaryCreate.equipArmor[1] = "armor8";
                mercenaryCreate.equipWeapon[0] = "weapon7"; mercenaryCreate.equipWeapon[1] = "weapon4";
                mercenaryCreate.equipBoots[0] = "boots4"; mercenaryCreate.equipBoots[1] = "boots7";

            }
            if (i == 3) {
                mercenaryCreate.setName("C"); mercenaryCreate.setMer_no(3);
                mercenaryCreate.level = Player.Play.level;
                mercenaryCreate.dps = 100;
                mercenaryCreate.strPower = 100;
                mercenaryCreate.attackSpeed = 100;
                mercenaryCreate.focus = 100;
                mercenaryCreate.critical = 100;
                mercenaryCreate.defPower = 100;
                mercenaryCreate.evaRate = 100;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.collectSpeed = 100;
                mercenaryCreate.collectAmount = 100;
                mercenaryCreate.equipHelmet[0] = "helmet1"; mercenaryCreate.equipHelmet[1] = "helmet4";
                mercenaryCreate.equipArmor[0] = "armor4"; mercenaryCreate.equipArmor[1] = "armor7";
                mercenaryCreate.equipWeapon[0] = "weapon1"; mercenaryCreate.equipWeapon[1] = "weapon3";
                mercenaryCreate.equipBoots[0] = "boots5"; mercenaryCreate.equipBoots[1] = "boots8";

            }
            mercenary.Add(mercenaryCreate);
        }
        mercenaryManager.setMercenary(mercenary);
    }

    //용병 data
    public void setMercenary(List<Mercenary> mer) { mercenary = mer; }
    public List<Mercenary> getMercenary() { return mercenary; }

}


public class Mercenary
{
    private int mer_no;                  //용병 고유 번호
    private string merName;             //용병 이름
    public int level;
    public int exp;
    public int max_exp;
    //능력치
    public float dps;
    public float strPower;
    public float attackSpeed;
    public float focus;
    public float critical;
    public float defPower;
    public float evaRate;
    public string attribute;
    public float collectSpeed;
    public float collectAmount;
    //장비
    public string[] equipHelmet;
    public string[] equipArmor;
    public string[] equipWeapon;
    public string[] equipBoots;

    public bool state = false;         //용병 상태
    public int stageNum;            //위치한 스테이지


    //생성자
    public Mercenary() {
        this.mer_no = 0;
        this.merName = null;
        this.level = 1;
        this.exp = 0;
        this.max_exp = this.level * 55;
        this.dps = 1;
        this.strPower = 1;
        this.attackSpeed = 1.0f;
        this.focus = 50;
        this.critical = 20;
        this.defPower = 5;
        this.evaRate = 3;
        this.attribute = "no";
        this.collectSpeed = 1.0f;
        this.collectAmount = 1;
        this.equipHelmet = new string[2];
        this.equipArmor = new string[2];
        this.equipWeapon = new string[2];
        this.equipBoots = new string[2];
        this.state = false;
        this.stageNum = 0;
    }
    //public Mercenary(string merName)
    //{ this.merName = merName; }

    public void setMer_no(int num) { mer_no = num; }
    public int getMer_no() { return mer_no; }
    public void setName(string n) { merName = n; }
    public string getName() { return merName; }

    public void setState(bool s) { state = s; }
    public bool getState() { return state; }
    public void setStageNum(int stage) { stageNum = stage; }
    public int getStageNum() { return stageNum; }
}

