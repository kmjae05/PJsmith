using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


public class QuestData : MonoBehaviour {


    public static QuestData instance = null;

    private JsonData AchvData;
    private WWW reader;
    private static List<Quest> QuestList;
    private static List<Quest> WeeklyQuestList;

    //퀘스트 카운트
    static public int questLogin = 1;
    static public int questHunting = 0;
    static public int questWeeklyHunting = 0;
    static public int questRefine = 0;
    static public int questReinforcement = 0;
    static public int questProduction = 0;
    static public int questPlunder = 0;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        QuestList = new List<Quest>();
        WeeklyQuestList = new List<Quest>();

        #region 업적관련 Json데이터 읽어온 후 List 저장
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Quest.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            AchvData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Quest.json");
            AchvData = JsonMapper.ToObject(tmp);
        }

        for (int i = 0; i < AchvData["Quest"].Count; i++)
        {
            QuestList.Add(new Quest(
                            (int)AchvData["Quest"][i]["no"],
                            AchvData["Quest"][i]["type"].ToString(),
                            (int)AchvData["Quest"][i]["amount"],
                            AchvData["Quest"][i]["alertText"].ToString(),
                            AchvData["Quest"][i]["reward_name"].ToString(),
                            (int)AchvData["Quest"][i]["reward_quantity"]
                            ));
        }
        for (int i = 1; i < AchvData["Quest"].Count; i++)
        {
            WeeklyQuestList.Add(new Quest(
                            (int)AchvData["Quest"][i]["no"],
                            AchvData["Quest"][i]["type"].ToString(),
                            (int)AchvData["Quest"][i]["amount"]*5,
                            AchvData["Quest"][i]["alertText"].ToString(),
                            AchvData["Quest"][i]["reward_name"].ToString(),
                            (int)AchvData["Quest"][i]["reward_quantity"]*5
                            ));
        }
        #endregion




    }

    public List<Quest> getQuestList() { return QuestList; }
    public List<Quest> getWeeklyQuestList() { return WeeklyQuestList; }

}




public class Quest
{
    public int no;
    public string type;
    public string target;
    public int amount;
    public string alertText;
    public string reward_name;
    public int reward_quantity;

    public bool completeFlag;   //완료했는지
    public bool rewardFlag;     //보상 받았는지

    public Quest(int no, string type, int amount, string alertText, string reward_name, int reward_quantity)
    {
        this.no = no;
        this.type = type;
        target = null;
        this.amount = amount;
        this.alertText = alertText;
        this.reward_name = reward_name;
        this.reward_quantity = reward_quantity;

        completeFlag = false;
        rewardFlag = false;
    }







}