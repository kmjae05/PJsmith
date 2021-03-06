﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System;


public class ThingsData : MonoBehaviour
{
    public static ThingsData instance = null;

    //Json Data
    public WWW reader;
    static public JsonData thingsData;

    static private List<Things> thingsList = new List<Things>();
    static private List<InventoryThings> invenThings = new List<InventoryThings>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        GameObject.Find("InventoryScript").GetComponent<Inventory>().enabled = true;

        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Things.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            thingsData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Things.json");
            thingsData = JsonMapper.ToObject(tmp);
        }


        for (int i = 0; i < thingsData["Things"].Count; i++)
        {
            thingsList.Add(new Things(thingsData, i));
        }
        thingsList.Find(x => x.name == "돌").possession = 1300;
        //thingsList.Find(x => x.name == "티켓").possession = 3;
        thingsList.Find(x => x.name == "부스트").possession = 3;

        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "돌").type, "돌", thingsList.Find(x=>x.name=="돌").possession));
        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "돌주괴").type, "돌주괴", 2));
        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "무기제작서-일반").type, "무기제작서-일반", 5));
        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "무기제작서-일반조각").type, "무기제작서-일반조각", 25));
        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "방어제작서-일반").type, "방어제작서-일반", 5));
        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "무기제작서-고급").type, "무기제작서-고급", 5));
        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "방어제작서-고급").type, "방어제작서-고급", 5));
        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "무기제작서-영웅").type, "무기제작서-영웅", 5));
        //invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "티켓").type, "티켓", thingsList.Find(x => x.name == "티켓").possession));
        invenThings.Add(new InventoryThings(thingsList.Find(x => x.name == "부스트").type, "부스트", thingsList.Find(x => x.name == "부스트").possession));


    }

    private void Start()
    {
        

    }

    private void Update()
    {
        invenThings.Remove(invenThings.Find(x => x.possession == 0));
    }

    //public Things FetchItemByID(int id)//, List<Item> ItamDatabase) //잘못된 id가 있는지 확인 -> 아이템 database 부분 수정 무기 탭에 따라 바뀌도록
    //{
    //    List<Things> database = new List<Things>();
    //    database = thingsList;
    //    for (int i = 0; i < database.Count; i++)
    //    {
    //        if (database[i].item_no == id)
    //        {
    //            return database[i];
    //        }
    //    }
    //    return null;
    //}

    public Color ChangeFrameColor(int grade)
    {
        Color col = new Color();
        switch (grade)
        {
            case 1:
                col = new Color(1, 1, 1);
                break;
            case 2:
                col = new Color(0.05f, 0.62f, 0.1f);
                break;
            case 3:
                col = new Color(0.05f, 0.55f, 0.72f);
                break;
            case 4:
                col = new Color(0.6f, 0.1f, 0.67f);
                break;
            case 5:
                col = new Color(1, 0.75f, 0);
                break;
        }

        return col;
    }
    
    public void getItem(Things things)
    {
        //장비 구분
        if (things.type == "Helmet" || things.type == "Armor" || things.type == "Gloves" || things.type == "Pants" || things.type == "Weapon" || things.type == "Boots")
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x
                => x.name == things.name).type, things.name, 1));
        }
        //장비 외 아이템
        else
        {
            if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name) != null)
            {
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name).possession += things.possession;
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name).recent = true;
            }
            else
            {
                ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x
                    => x.name == things.name).type, things.name, things.possession));
            }
        }
    }
    public void getItem(InventoryThings things)
    {
        //장비 구분
        if (things.type == "Helmet" || things.type == "Armor" || things.type == "Gloves" || things.type == "Pants" || things.type == "Weapon" || things.type == "Boots")
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x
                => x.name == things.name).type, things.name, 1));
        }
        //장비 외 아이템
        else
        {
            if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name) != null)
            {
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name).possession += things.possession;
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name).recent = true;
            }
            else
            {
                ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x
                    => x.name == things.name).type, things.name, things.possession));
            }
        }
    }



    public List<Things> getThingsList() { return thingsList; }
    public void setThingsList(Things things, int index) { thingsList[index] = things; }
    public void setThingsList(List<Things> things) { thingsList = things; }
    public List<InventoryThings> getInventoryThingsList() { return invenThings; }
    public void setInventoryThingsList(InventoryThings things, int index) { invenThings[index] = things; }
    public void setInventoryThingsList(List<InventoryThings> things) { invenThings = things; }
}

//아이템 기본 정보
[Serializable]
public class Things
{
    public int item_no;
    public string type;
    public string name;
    public string explanation;
    public int grade;
    public int sell;
    public string icon;

    public int possession;  //안 씀
    public bool isLock;     //안 씀

    public bool illustrate;    //도감

    public Things()
    {

    }
    public Things(JsonData thingsData, int index)
    {
        this.item_no = (int)thingsData["Things"][index]["item_no"];
        this.type = thingsData["Things"][index]["type"].ToString();
        this.name = thingsData["Things"][index]["name"].ToString();
        this.explanation = thingsData["Things"][index]["explanation"].ToString();
        this.grade = (int)thingsData["Things"][index]["grade"];
        this.sell = (int)thingsData["Things"][index]["sell"];
        this.icon = thingsData["Things"][index]["icon"].ToString();

        this.possession = 0;
        this.isLock = false;
        illustrate = false;
    }


}

//인벤토리 등에서 보여지는 아이템
[Serializable]
public class InventoryThings
{
    public string type;
    public string name;    

    public int possession;
    //public bool isLock;
    public bool recent;

    //장비관련
    public Stat stat;
    public string attribute;
    public int reinforcement;   //강화수치
    public bool equip;          //장비 착용 상태
    public int equipSetNum;     //장비 착용 세트 번호
    public string equipChrName; //착용한 캐릭터 이름
    public int exp;             //강화도

    public InventoryThings()
    {
        this.possession = 0;
        this.recent = true;
        this.reinforcement = 0;
        this.equip = false;
        equipChrName = null;
        equipSetNum = 0;
        exp = 0;
    }

    public InventoryThings(string type, string name, int possession)
    {
        this.type = type;
        this.name = name;
        this.possession = possession;
        this.recent = true;

        stat = new Stat();
        Equipment equip = GameObject.Find("ThingsData").GetComponent<EquipmentData>().getEquipmentList().Find(x => x.name == name);
        if (equip != null)
        {
            this.stat.strPower = equip.stat.strPower;
            this.stat.attackSpeed = equip.stat.attackSpeed;
            this.stat.focus = equip.stat.focus;
            this.stat.critical = equip.stat.critical;
            this.stat.defPower = equip.stat.defPower;
            this.stat.evaRate = equip.stat.evaRate;
            this.attribute = "없음";
            this.stat.dps = equip.stat.dps ;
        }
        this.reinforcement = 0;
        this.equip = false;
        equipChrName = null;
        equipSetNum = 0;
        exp = 0;

        ThingsData.instance.getThingsList().Find(x => x.name == name).illustrate = true;
    }

}
