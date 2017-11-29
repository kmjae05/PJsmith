using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class MonsterData : MonoBehaviour {



    private JsonData monsterData;
    private WWW reader;
    private static List<FieldMonster> MonsterList = new List<FieldMonster>();



    void Start () {

        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Monster.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            monsterData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Monster.json");
            monsterData = JsonMapper.ToObject(tmp);
        }
        for (int i = 0; i < monsterData["Monster"].Count; i++)
        {
            MonsterList.Add(new FieldMonster(monsterData, i));
        }

    }
	
    public List<FieldMonster> getMonsterList() { return MonsterList; }



}

public class FieldMonster
{
    public string name;
    public int grade;
    public Stat stat;
    public string[] itemName;
    public int[] itemAmount;
    public int[] itemProb;


    public FieldMonster(JsonData data, int index)
    {
        this.name = data["Monster"][index]["name"].ToString();
        this.grade = (int)data["Monster"][index]["grade"];
        this.stat = new Stat();
        this.stat.dps = (int)data["Monster"][index]["dps"];
        this.stat.strPower = (int)data["Monster"][index]["strPower"];
        this.stat.attackSpeed = (int)data["Monster"][index]["attackSpeed"];
        this.stat.focus = (int)data["Monster"][index]["focus"];
        this.stat.critical = (int)data["Monster"][index]["critical"];
        this.stat.defPower = (int)data["Monster"][index]["defPower"];
        this.stat.evaRate = (int)data["Monster"][index]["evaRate"];

        int idx = data["Monster"][index]["getThingName"].Count;
        itemName = new string[idx];
        itemAmount = new int[idx];
        itemProb = new int[idx];

        for (int i = 0; i < idx; i++)
        {
            itemName[i] = data["Monster"][index]["getThingName"][i]["itemName"].ToString();
            itemAmount[i] = (int)data["Monster"][index]["getThingName"][i]["itemAmount"];
            itemProb[i] = (int)data["Monster"][index]["getThingName"][i]["itemProb"];
        }

    }



}


