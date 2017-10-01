using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;


public class ThingsData : MonoBehaviour
{
    public static ThingsData instance = null;

    //Json Data
    public WWW reader;
    static public JsonData thingsData;

    static private List<Things> thingsList = new List<Things>();
    private List<GameObject> thingsListObj = new List<GameObject>();


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        GameObject.Find("InventoryScript").GetComponent<Inventory>().enabled = true;
    }

    private void Start()
    {
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
        thingsList.Find(x => x.name == "티켓").possession = 3;

    }

    public Things FetchItemByID(int id)//, List<Item> ItamDatabase) //잘못된 id가 있는지 확인 -> 아이템 database 부분 수정 무기 탭에 따라 바뀌도록
    {
        List<Things> database = new List<Things>();
        database = thingsList;
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].item_no == id)
            {
                return database[i];
            }
        }
        return null;
    }

    public List<Things> getThingsList() { return thingsList; }
    public void setThingsList(Things things, int index) { thingsList[index] = things; }

}


public class Things
{
    public int item_no;
    public string type;
    public string name;
    public string explanation;
    public string icon;

    public int possession;
    public bool isLock;
    public bool recent;

    

    public Things(JsonData thingsData, int index)
    {
        this.item_no = (int)thingsData["Things"][index]["item_no"];
        this.type = thingsData["Things"][index]["type"].ToString();
        this.name = thingsData["Things"][index]["name"].ToString();
        this.explanation = thingsData["Things"][index]["explanation"].ToString();
        this.icon = thingsData["Things"][index]["icon"].ToString();

        this.possession = 0;
        this.isLock = false;
        this.recent = true;

    }


}