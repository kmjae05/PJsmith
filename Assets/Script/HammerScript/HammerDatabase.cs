using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class HammerDatabase : MonoBehaviour
{

    private List<Hammer> database = new List<Hammer>();
    private JsonData HammerData;
    public static RuntimePlatform platform;
    public Encoding enkr = Encoding.GetEncoding(51949);

    public WWW reader;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Hammer.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            HammerData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Hammer.json");
            HammerData = JsonMapper.ToObject(tmp);
        }
        ConstructItemDatabase();
    }

    public Hammer FetchItemByID(int id) //잘못된 id가 있는지 확인
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].Id == id)
            {
                return database[i];
            }
        }
        return null; 
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < HammerData["Hammer"].Count; i++) //데이터베이스에서 아이템의 고유값 스테이터스를 가져옴 메모리에 저장
        {
            database.Add(new Hammer(HammerData, i));
        }
    }                                      
}
[System.Serializable]
public class Hammer
{
    public int Id { get; set; }
    public string name { get; set; }
    public int power { get; set; }
    public int critical { get; set; }
    public int rarity { get; set; }
    public int type { get; set; }
    public string HmNum { get; set; }
    public string meshName { get; set; }
    public int price { get; set; }
    public string Slug { get; set; }
    public Sprite sprite { get; set; }
    public int ItemEigen { get; set; }

    //아이템 생성자
    public Hammer(int Id)
    {
        this.Id = Id;
    }

    public Hammer(JsonData HammerData, int i)
    {
        this.Id = (int)HammerData["Hammer"][i]["id"];
        this.name = HammerData["Hammer"][i]["name"].ToString();
        this.power = (int)HammerData["Hammer"][i]["power"];
        this.critical = (int)HammerData["Hammer"][i]["critical"];
        this.rarity = (int)HammerData["Hammer"][i]["rarity"];
        this.type = (int)HammerData["Hammer"][i]["type"];
        this.HmNum = HammerData["Hammer"][i]["HmNum"].ToString();
        this.meshName = HammerData["Hammer"][i]["meshName"].ToString();
        this.price = (int)HammerData["Hammer"][i]["price"];
        this.Slug = HammerData["Hammer"][i]["slug"].ToString(); //이미지 이름
        this.sprite = Resources.Load<Sprite>("Hammer/Icon/" + Slug); //Resources/Spirites/Item/의 경로에있는 이미지 이름이 slug와 같은 것을
    }

    public Hammer()
    {
        this.Id = -1;
        this.ItemEigen = -1;
    }
}
