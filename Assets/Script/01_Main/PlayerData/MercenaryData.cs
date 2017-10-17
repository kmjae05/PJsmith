using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercenaryData : MonoBehaviour {

    public static MercenaryData instance = null;
    private MercenaryManager mercenaryManager;  //mercenary info


    static private List<Mercenary> mercenary;
    private Mercenary mercenaryCreate;

    EquipmentData equipmentData;


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
        equipmentData = GameObject.Find("ThingsData").GetComponent<EquipmentData>();

        mercenary = new List<Mercenary>();
        
        //용병 A, B, C 생성.
        for (int i = 1; i < 4; i++)
        {
            mercenaryCreate = new Mercenary();
            if (i == 1) {
                mercenaryCreate.setName("A"); mercenaryCreate.setMer_no(1);
                mercenaryCreate.level = Player.Play.level;
                mercenaryCreate.stat.dps = 100;
                mercenaryCreate.stat.strPower = 100;
                mercenaryCreate.stat.attackSpeed = 100;
                mercenaryCreate.stat.focus = 100;
                mercenaryCreate.stat.critical = 100;
                mercenaryCreate.stat.defPower = 100;
                mercenaryCreate.stat.evaRate = 100;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.stat.collectSpeed = 100;
                mercenaryCreate.stat.collectAmount = 100;
                //장비
                mercenaryCreate.equipHelmet[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
                mercenaryCreate.equipHelmet[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구"); 
                mercenaryCreate.equipArmor[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷"); 
                mercenaryCreate.equipArmor[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷"); 
                mercenaryCreate.equipWeapon[0] = equipmentData.getEquipmentList().Find(x => x.name == "수련자의 단검"); 
                mercenaryCreate.equipWeapon[1] = equipmentData.getEquipmentList().Find(x => x.name == "기사의 단검"); 
                mercenaryCreate.equipBoots[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠"); 
                mercenaryCreate.equipBoots[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠"); 

            }
            if (i == 2) {
                mercenaryCreate.setName("B"); mercenaryCreate.setMer_no(2);
                mercenaryCreate.level = Player.Play.level;
                mercenaryCreate.stat.dps = 100;
                mercenaryCreate.stat.strPower = 100;
                mercenaryCreate.stat.attackSpeed = 100;
                mercenaryCreate.stat.focus = 100;
                mercenaryCreate.stat.critical = 100;
                mercenaryCreate.stat.defPower = 100;
                mercenaryCreate.stat.evaRate = 100;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.stat.collectSpeed = 100;
                mercenaryCreate.stat.collectAmount = 100;
                mercenaryCreate.equipHelmet[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구"); 
                mercenaryCreate.equipHelmet[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구"); 
                mercenaryCreate.equipArmor[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷"); 
                mercenaryCreate.equipArmor[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
                mercenaryCreate.equipWeapon[0] = equipmentData.getEquipmentList().Find(x => x.name == "영웅의 단검");
                mercenaryCreate.equipWeapon[1] = equipmentData.getEquipmentList().Find(x => x.name == "오딘의 단검"); 
                mercenaryCreate.equipBoots[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");
                mercenaryCreate.equipBoots[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠"); 

            }
            if (i == 3) {
                mercenaryCreate.setName("C"); mercenaryCreate.setMer_no(3);
                mercenaryCreate.level = Player.Play.level;
                mercenaryCreate.stat.dps = 100;
                mercenaryCreate.stat.strPower = 100;
                mercenaryCreate.stat.attackSpeed = 100;
                mercenaryCreate.stat.focus = 100;
                mercenaryCreate.stat.critical = 100;
                mercenaryCreate.stat.defPower = 100;
                mercenaryCreate.stat.evaRate = 100;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.stat.collectSpeed = 100;
                mercenaryCreate.stat.collectAmount = 100;
                mercenaryCreate.equipHelmet[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구"); 
                mercenaryCreate.equipHelmet[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구"); 
                mercenaryCreate.equipArmor[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷"); 
                mercenaryCreate.equipArmor[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷"); 
                mercenaryCreate.equipWeapon[0] = equipmentData.getEquipmentList().Find(x => x.name == "여신의 단검"); 
                mercenaryCreate.equipWeapon[1] = equipmentData.getEquipmentList().Find(x => x.name == "드래곤의 단검"); 
                mercenaryCreate.equipBoots[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠"); 
                mercenaryCreate.equipBoots[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠"); 

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
    public Stat stat;
    public string attribute;
    //장비
    public Equipment[] equipHelmet;
    public Equipment[] equipArmor;
    public Equipment[] equipWeapon;
    public Equipment[] equipBoots;

    public bool state = false;         //용병 상태
    public int stageNum;            //위치한 스테이지

    //대표 캐릭터

    //생성자
    public Mercenary() {
        this.mer_no = 0;
        this.merName = null;
        this.level = 1;
        this.exp = 0;
        this.max_exp = this.level * 55;
        this.stat = new Stat();
        this.stat.dps = 1;
        this.stat.strPower = 1;
        this.stat.attackSpeed = 1.0f;
        this.stat.focus = 50;
        this.stat.critical = 20;
        this.stat.defPower = 5;
        this.stat.evaRate = 3;
        this.attribute = "no";
        this.stat.collectSpeed = 1.0f;
        this.stat.collectAmount = 1;
        this.equipHelmet = new Equipment[2];
        this.equipArmor = new Equipment[2];
        this.equipWeapon = new Equipment[2];
        this.equipBoots = new Equipment[2];
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

