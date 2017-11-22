using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;
using System;

public class EquipmentData : MonoBehaviour
{
    //Json Data
    public WWW reader;
    static public JsonData equipData;

    static private List<Equipment> equipList = new List<Equipment>();


    void Awake ()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Equipment.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            equipData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Equipment.json");
            equipData = JsonMapper.ToObject(tmp);
        }


        for (int i = 0; i < equipData["Equipment"].Count; i++)
        {
            equipList.Add(new Equipment(equipData, i));
        }
    }
	

    public List<Equipment> getEquipmentList() { return equipList; }


}


public class Equipment
{
    public string name;
    public string explanation;

    private string material;             //json으로 데이터 불러옴
    public int time;
    public string skill;

    public Stat stat;
    public string attribute;


    public string[] necessaryMaterials; //필요 재료
    public int[] necessaryMaterialsNum; //수량

    public Equipment() { }
    public Equipment(JsonData EquipData, int index)
    {
        this.name = EquipData["Equipment"][index]["name"].ToString();
        this.explanation = EquipData["Equipment"][index]["explanation"].ToString();
        this.material = EquipData["Equipment"][index]["material"].ToString();
        char[] del = { ' ' };
        string[] words = material.Split(del, StringSplitOptions.RemoveEmptyEntries);
        this.necessaryMaterials = new string[words.Length / 2];
        this.necessaryMaterialsNum = new int[words.Length / 2];
        for (int i = 0; i < words.Length; i++)
        {
            if (i % 2 == 0)
                necessaryMaterials[i/2] = words[i];
            else necessaryMaterialsNum[i/2] = Convert.ToInt32(words[i]);
        }
        this.time = (int)EquipData["Equipment"][index]["time"];
        this.skill = EquipData["Equipment"][index]["skill"].ToString();

        stat = new Stat();
        this.stat.dps = (int)EquipData["Equipment"][index]["dps"];
        this.stat.strPower = (int)EquipData["Equipment"][index]["strPower"];
        this.stat.attackSpeed =  Convert.ToDouble(EquipData["Equipment"][index]["attackSpeed"].ToString());
        this.stat.focus = (int)EquipData["Equipment"][index]["focus"];
        this.stat.critical = (int)EquipData["Equipment"][index]["critical"];
        this.stat.defPower = (int)EquipData["Equipment"][index]["defPower"];
        this.stat.evaRate = (int)EquipData["Equipment"][index]["evaRate"];
        this.stat.collectSpeed = (int)EquipData["Equipment"][index]["collectSpeed"];
        this.stat.collectAmount = (int)EquipData["Equipment"][index]["collectAmount"];
        this.attribute = EquipData["Equipment"][index]["attribute"].ToString();

    }
}


