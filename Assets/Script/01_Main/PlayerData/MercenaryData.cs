﻿using System.Collections;
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
                mercenaryCreate.level = Player.instance.getUser().level;
                mercenaryCreate.stat.strPower = 60;
                mercenaryCreate.stat.attackSpeed = 1.0f;
                mercenaryCreate.stat.focus = 50;
                mercenaryCreate.stat.critical = 20;
                mercenaryCreate.stat.defPower = 5;
                mercenaryCreate.stat.evaRate = 3;
                mercenaryCreate.stat.dps = mercenaryCreate.stat.strPower * (float)mercenaryCreate.stat.attackSpeed * mercenaryCreate.stat.critical
                    + mercenaryCreate.stat.defPower * mercenaryCreate.stat.evaRate;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.stat.collectSpeed = 100;
                mercenaryCreate.stat.collectAmount = 100;
                //장비                
                mercenaryCreate.equipWeapon[0] = equipmentData.getEquipmentList().Find(x => x.name == "수련자의 단검"); 
                mercenaryCreate.equipWeapon[1] = equipmentData.getEquipmentList().Find(x => x.name == "기사의 단검");
                mercenaryCreate.equipArmor[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
                mercenaryCreate.equipArmor[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
                mercenaryCreate.equipPants[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천바지");
                mercenaryCreate.equipPants[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천바지");
                mercenaryCreate.equipHelmet[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
                mercenaryCreate.equipHelmet[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
                mercenaryCreate.equipGloves[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천장갑");
                mercenaryCreate.equipGloves[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천장갑");
                mercenaryCreate.equipBoots[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");
                mercenaryCreate.equipBoots[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");

            }
            if (i == 2) {
                mercenaryCreate.setName("B"); mercenaryCreate.setMer_no(2);
                mercenaryCreate.level = Player.instance.getUser().level;
                
                mercenaryCreate.stat.strPower = 70;
                mercenaryCreate.stat.attackSpeed = 1.0f;
                mercenaryCreate.stat.focus = 50;
                mercenaryCreate.stat.critical = 20;
                mercenaryCreate.stat.defPower = 5;
                mercenaryCreate.stat.evaRate = 3;
                mercenaryCreate.stat.dps = mercenaryCreate.stat.strPower * (float)mercenaryCreate.stat.attackSpeed * mercenaryCreate.stat.critical
                    + mercenaryCreate.stat.defPower * mercenaryCreate.stat.evaRate;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.stat.collectSpeed = 100;
                mercenaryCreate.stat.collectAmount = 100;
                mercenaryCreate.equipWeapon[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 창");
                mercenaryCreate.equipWeapon[1] = equipmentData.getEquipmentList().Find(x => x.name == "수련자의 창");
                mercenaryCreate.equipArmor[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
                mercenaryCreate.equipArmor[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
                mercenaryCreate.equipPants[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천바지");
                mercenaryCreate.equipPants[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천바지");
                mercenaryCreate.equipHelmet[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
                mercenaryCreate.equipHelmet[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
                mercenaryCreate.equipGloves[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천장갑");
                mercenaryCreate.equipGloves[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천장갑");
                mercenaryCreate.equipBoots[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");
                mercenaryCreate.equipBoots[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");

            }
            if (i == 3) {
                mercenaryCreate.setName("C"); mercenaryCreate.setMer_no(3);
                mercenaryCreate.level = Player.instance.getUser().level;
               
                mercenaryCreate.stat.strPower = 80;
                mercenaryCreate.stat.attackSpeed = 1.0f;
                mercenaryCreate.stat.focus = 50;
                mercenaryCreate.stat.critical = 20;
                mercenaryCreate.stat.defPower = 5;
                mercenaryCreate.stat.evaRate = 3;
                mercenaryCreate.stat.dps = mercenaryCreate.stat.strPower * (float)mercenaryCreate.stat.attackSpeed * mercenaryCreate.stat.critical
                    + mercenaryCreate.stat.defPower * mercenaryCreate.stat.evaRate;
                mercenaryCreate.attribute = "no";
                mercenaryCreate.stat.collectSpeed = 100;
                mercenaryCreate.stat.collectAmount = 100;
                mercenaryCreate.equipWeapon[0] = equipmentData.getEquipmentList().Find(x => x.name == "여행자의 도끼");
                mercenaryCreate.equipWeapon[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 도끼");
                mercenaryCreate.equipArmor[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
                mercenaryCreate.equipArmor[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
                mercenaryCreate.equipPants[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천바지");
                mercenaryCreate.equipPants[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천바지");
                mercenaryCreate.equipHelmet[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
                mercenaryCreate.equipHelmet[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
                mercenaryCreate.equipGloves[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천장갑");
                mercenaryCreate.equipGloves[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천장갑");
                mercenaryCreate.equipBoots[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");
                mercenaryCreate.equipBoots[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");

            }
            mercenary.Add(mercenaryCreate);
        }
        
        mercenaryManager.setMercenary(mercenary);


       ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "수련자의 단검").type, "수련자의 단검", 1));
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "수련자의 단검").equip = true;
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "수련자의 단검").equipChrName = MercenaryData.instance.getMercenary()[0].getName();
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "수련자의 단검").equipSetNum = 1;

        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "기사의 단검").type, "기사의 단검", 1));
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "기사의 단검").equip = true;
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "기사의 단검").equipChrName = MercenaryData.instance.getMercenary()[0].getName();
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "기사의 단검").equipSetNum = 2;

        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 창").type, "초보자의 창", 1));
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "초보자의 창").equip = true;
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "초보자의 창").equipChrName = MercenaryData.instance.getMercenary()[1].getName();
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "초보자의 창").equipSetNum = 1;

        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "수련자의 창").type, "수련자의 창", 1));
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "수련자의 창").equip = true;
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "수련자의 창").equipChrName = MercenaryData.instance.getMercenary()[1].getName();
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "수련자의 창").equipSetNum = 2;

        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "여행자의 도끼").type, "여행자의 도끼", 1));
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "여행자의 도끼").equip = true;
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "여행자의 도끼").equipChrName = MercenaryData.instance.getMercenary()[2].getName();
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "여행자의 도끼").equipSetNum = 1;

        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 도끼").type, "초보자의 도끼", 1));
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "초보자의 도끼").equip = true;
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "초보자의 도끼").equipChrName = MercenaryData.instance.getMercenary()[2].getName();
        ThingsData.instance.getInventoryThingsList().Find(x => x.name == "초보자의 도끼").equipSetNum = 2;

        for (int i = 0; i < ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천갑옷").possession; i++)
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천갑옷").type, "초보자의 천갑옷", 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            if (i / 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = Player.instance.getUser().Name;
            else if (i / 2 == 1) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[0].getName();
            else if (i / 2 == 2) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[1].getName();
            else if (i / 2 == 3) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[2].getName();
            if (i % 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 1;
            else ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 2;
        }
        for (int i = 0; i < ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천바지").possession; i++)
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천바지").type, "초보자의 천바지", 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            if (i / 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = Player.instance.getUser().Name;
            else if (i / 2 == 1) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[0].getName();
            else if (i / 2 == 2) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[1].getName();
            else if (i / 2 == 3) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[2].getName();
            if (i % 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 1;
            else ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 2;
        }
        for (int i = 0; i < ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천투구").possession; i++)
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천투구").type, "초보자의 천투구", 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            if (i / 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = Player.instance.getUser().Name;
            else if (i / 2 == 1) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[0].getName();
            else if (i / 2 == 2) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[1].getName();
            else if (i / 2 == 3) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[2].getName();
            if (i % 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 1;
            else ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 2;
        }
        for (int i = 0; i < ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천장갑").possession; i++)
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천장갑").type, "초보자의 천장갑", 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            if (i / 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = Player.instance.getUser().Name;
            else if (i / 2 == 1) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[0].getName();
            else if (i / 2 == 2) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[1].getName();
            else if (i / 2 == 3) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[2].getName();
            if (i % 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 1;
            else ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 2;
        }
        for (int i = 0; i < ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천부츠").possession; i++)
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "초보자의 천부츠").type, "초보자의 천부츠", 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            if (i / 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = Player.instance.getUser().Name;
            else if (i / 2 == 1) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[0].getName();
            else if (i / 2 == 2) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[1].getName();
            else if (i / 2 == 3) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = MercenaryData.instance.getMercenary()[2].getName();
            if (i % 2 == 0) ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 1;
            else ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = 2;
        }

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
    public Equipment[] equipWeapon;
    public Equipment[] equipArmor;
    public Equipment[] equipPants;
    public Equipment[] equipHelmet;
    public Equipment[] equipGloves;
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
        this.stat.strPower = 1;
        this.stat.attackSpeed = 1.0f;
        this.stat.focus = 50;
        this.stat.critical = 20;
        this.stat.defPower = 5;
        this.stat.evaRate = 3;
        this.stat.dps = this.stat.strPower * (float)this.stat.attackSpeed * this.stat.critical;
        this.attribute = "no";
        this.stat.collectSpeed = 1.0f;
        this.stat.collectAmount = 1;
        this.equipWeapon = new Equipment[2];
        this.equipArmor = new Equipment[2];
        this.equipPants = new Equipment[2];
        this.equipHelmet = new Equipment[2];
        this.equipGloves = new Equipment[2];
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

