using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;

    private MineData mineData;
    private AchievementData achvData;
    private QuestData questData;

    void Awake()
    {
        if (gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
        Debug.Log("GameManagerAwake");
    }

    void Start ()
    {
        Debug.Log("GameManagerStart");
        mineData = GameObject.Find("MineData").GetComponent<MineData>();
        achvData = GameObject.Find("AchievementData").GetComponent<AchievementData>();
        questData = GameObject.Find("QuestData").GetComponent<QuestData>();

        //1번만 실행. 장면 전환할 때도 불러와야.
        Load();

    }

    private void Update()
    {
        Invoke("Save", 5f);
    }

    public void Save()
    {
        Debug.Log("save");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData data = new PlayerData();
        data.user = new User();
        data.user = Player.instance.getUser();
        data.mine = new List<Mine>();
        data.mine = mineData.getMineList();
        data.mineInfo = new List<MineInfo>();
        data.mineInfo = mineData.getMineInfoList();
        data.achv = new List<CAchievement>();
        data.achv = achvData.getAchvList();
        data.quest = new List<Quest>();
        data.quest = questData.getQuestList();
        data.weeklyquest = new List<Quest>();
        data.weeklyquest = questData.getWeeklyQuestList();
        data.questLogin = QuestData.questLogin;
        data.questHunting = QuestData.questHunting;
        data.questWeeklyHunting = QuestData.questWeeklyHunting;
        data.questRefine = QuestData.questRefine;
        data.questReinforcement = QuestData.questReinforcement;
        data.questProduction = QuestData.questProduction;
        data.questPlunder = QuestData.questPlunder;

        data.things = new List<Things>();
        data.things = ThingsData.instance.getThingsList();
        data.invenThings = new List<InventoryThings>();
        data.invenThings = ThingsData.instance.getInventoryThingsList();
        data.saleList = new List<ForSale>();
        data.saleList = FestivalData.instance.getSaleList();
        data.setSlot = new List<SetSlot>();
        data.setSlot = SetSlotData.instance.getSetSlot();
        data.repreSet = SetSlotData.instance.getRepreSet();
        data.mercenary = new List<Mercenary>();
        data.mercenary = MercenaryData.instance.getMercenary();

        data.stageMine = new List<Mine>();
        data.stageMine = StageMineData.instance.getMineList();

        bf.Serialize(file, data);
        file.Close();
    }


    public void Load()
    {
        Debug.Log("load");
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            Player.instance.setUser(data.user);
            mineData.setMineList(data.mine);
            mineData.setMineInfoList(data.mineInfo);
            achvData.setAchvList(data.achv);
            questData.setQuestList(data.quest);
            questData.setWeeklyQuestList(data.weeklyquest);
            QuestData.questLogin = data.questLogin;
            QuestData.questHunting = data.questHunting;
            QuestData.questWeeklyHunting = data.questWeeklyHunting;
            QuestData.questRefine = data.questRefine;
            QuestData.questReinforcement = data.questReinforcement;
            QuestData.questProduction = data.questProduction;
            QuestData.questPlunder = data.questPlunder;

            ThingsData.instance.setThingsList(data.things);
            ThingsData.instance.setInventoryThingsList(data.invenThings);
            FestivalData.instance.setSaleList(data.saleList);
            SetSlotData.instance.setSetSlot(data.setSlot);
            SetSlotData.instance.setRepreSet(data.repreSet);
            MercenaryData.instance.setMercenary(data.mercenary);

            StageMineData.instance.setMineList(data.stageMine);
        }
    }


}



[Serializable]
class PlayerData
{
    public User user;
    public List<Mine> mine;
    public List<MineInfo> mineInfo;
    public List<CAchievement> achv;
    public List<Quest> quest;
    public List<Quest> weeklyquest;
    public int questLogin;
    public int questHunting;
    public int questWeeklyHunting;
    public int questRefine;
    public int questReinforcement;
    public int questProduction;
    public int questPlunder;

    public List<Things> things;
    public List<InventoryThings> invenThings;
    public List<ForSale> saleList;
    public List<SetSlot> setSlot;
    public int repreSet;

    public List<Mercenary> mercenary;
    // public List<StageInfo> stageInfo;
    //public List<Spot> spot;

    public List<Mine> stageMine;
}


