using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ItemDatabase : MonoBehaviour {

    private List<Item> database0 = new List<Item>();
    private List<Item> database1 = new List<Item>();
    private List<Item> database2 = new List<Item>();
    private List<Item> database3 = new List<Item>();

    private JsonData itemData0;
    private JsonData itemData1;
    private JsonData itemData2;
    private JsonData itemData3;

    public static RuntimePlatform platform;
    public Encoding enkr = Encoding.GetEncoding(51949);

    public WWW reader0;
    public WWW reader1;
    public WWW reader2;
    public WWW reader3;

    static public int DatabaseListSize0;
    static public int DatabaseListSize1;
    static public int DatabaseListSize2;
    static public int DatabaseListSize3;

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "CollectionTap1.json");
            reader0 = new WWW(mypath);

            mypath = Path.Combine(Application.streamingAssetsPath, "CollectionTap2.json");
            reader1 = new WWW(mypath);

            mypath = Path.Combine(Application.streamingAssetsPath, "CollectionTap3.json");
            reader2 = new WWW(mypath);

            mypath = Path.Combine(Application.streamingAssetsPath, "CollectionTap4.json");
            reader3 = new WWW(mypath);

            while (!reader0.isDone) { }
            itemData0 = JsonMapper.ToObject(reader0.text);

            while (!reader1.isDone) { }
            itemData1 = JsonMapper.ToObject(reader1.text);

            while (!reader2.isDone) { }
            itemData2 = JsonMapper.ToObject(reader2.text);

            while (!reader3.isDone) { }
            itemData3 = JsonMapper.ToObject(reader3.text);
        }
        else
        {
            string tmp;
            tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/CollectionTap1.json");
            itemData0 = JsonMapper.ToObject(tmp);

            tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/CollectionTap2.json");
            itemData1 = JsonMapper.ToObject(tmp);

            tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/CollectionTap3.json");
            itemData2 = JsonMapper.ToObject(tmp);

            tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/CollectionTap4.json");
            itemData3 = JsonMapper.ToObject(tmp);
        }
        ConstructItemDatabase();

    }

    public Item FetchItemByID(int DatabaseNo, int id)//, List<Item> ItamDatabase) //잘못된 id가 있는지 확인 -> 아이템 database 부분 수정 무기 탭에 따라 바뀌도록
    {
        List<Item> database = new List<Item>();
        if (DatabaseNo == 0)
        {
            database = database0;
        }
        else if (DatabaseNo == 1)
        {
            database = database1;
        }
        else if (DatabaseNo == 2)
        {
            database = database2;
        }
        else if (DatabaseNo == 3)
        {
            database = database3;
        }
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == id)
            {
                return database[i];
            }
        }
        return null; 
    }

    void ConstructItemDatabase()
    {
        for (int i = 0; i < itemData0["Item"].Count; i++) //데이터베이스에서 아이템의 고유값 스테이터스를 가져옴 메모리에 저장
        {
            database0.Add(new Item(itemData0, i, "Sword"));
        }
        for (int i = 0; i < itemData1["Item"].Count; i++) //데이터베이스에서 아이템의 고유값 스테이터스를 가져옴 메모리에 저장
        {
            database1.Add(new Item(itemData1, i, "TwohandSword"));
        }
        for (int i = 0; i < itemData2["Item"].Count; i++) //데이터베이스에서 아이템의 고유값 스테이터스를 가져옴 메모리에 저장
        {
            database2.Add(new Item(itemData2, i, "Lance"));
        }
        for (int i = 0; i < itemData3["Item"].Count; i++) //데이터베이스에서 아이템의 고유값 스테이터스를 가져옴 메모리에 저장
        {
            database3.Add(new Item(itemData3, i, "Ax"));
        }
        DatabaseListSize0 = database0.Count;
        DatabaseListSize1 = database1.Count;
        DatabaseListSize2 = database2.Count;
        DatabaseListSize3 = database3.Count;
    }                                      
}
[System.Serializable]
public class Item
{
    public int ID { get; set; } //아이템 값들 나중에 아이템 스테이터스 수정예정
    public string Title { get; set; }
    public int Value { get; set; }
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public string Decription { get; set; }
    public bool Stackable { get; set; }
    public int Raqarity { get; set; }
    public string Slug { get; set; } //이미지 불러오기
    public Sprite sprite { get; set; }
    public bool Possession { get; set; }

    //아이템 생성자
    public Item(int id)
    {
        this.ID = id;
    }
    //          제이슨        인덱스  파일경로
    public Item(JsonData json, int i, string str)
    {
        this.ID = (int)json["Item"][i]["id"];
        this.Title = json["Item"][i]["title"].ToString();
        this.Value = (int)json["Item"][i]["value"];
        this.Power = (int)json["Item"][i]["power"];
        this.Defence = (int)json["Item"][i]["defence"];
        this.Vitality = (int)json["Item"][i]["vitality"];
        this.Decription = json["Item"][i]["description"].ToString();
        this.Stackable = System.Convert.ToBoolean(json["Item"][i]["stackable"].ToString()); //아이템의 중복처리 트루일경우 아이템이 겹쳐지고 개수가 나옴
        this.Raqarity = (int)json["Item"][i]["rarity"];
        this.Slug = json["Item"][i]["slug"].ToString(); //이미지 이름
        this.sprite = Resources.Load<Sprite>("Collection/" + str + "/" + Slug); //Resources/Spirites/Item/의 경로에있는 이미지 이름이 slug와 같은 것을 불러옴
        this.Possession = System.Convert.ToBoolean(json["Item"][i]["possession"].ToString());
    }

    public Item(int id, int quantity)
    {
        this.ID = id;
        this.Value = quantity;
    }
    public Item()
    {
        this.ID = -1;
    }
}