using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System;


public class MineData : MonoBehaviour {

    public static MineData instance = null;
    //
    //Json Data
    public WWW reader;
    static public JsonData MineInfoData;

    static private List<MineInfo> mineInfoList = new List<MineInfo>();


    static private List<Mine> mineList;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "MineInfo.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            MineInfoData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/MineInfo.json");
            MineInfoData = JsonMapper.ToObject(tmp);
        }

        for (int i = 0; i < MineInfoData["Mine"].Count; i++)
        {
            mineInfoList.Add(new MineInfo(MineInfoData, i));
        }

        mineList = new List<Mine>();

        for(int i = 0; i < 10; i++)
        {
            mineList.Add(new Mine(i));
        }

	}
	
	void Update ()
    {
        for (int i = 0; i < 10; i++)
        {
            //건설 진행 중
            if (mineList[i].buildState == "beunder")
            {
                mineList[i].buildTime -= Time.deltaTime;
                if (mineList[i].buildTime < 0)
                {
                    mineList[i].buildTime = 0f;
                    mineList[i].buildState = "complete";
                    //완료 되면 바로 채굴
                    mineList[i].miningState = true;
                }
            }
            //채굴 진행 중
            if (mineList[i].miningState)
            {
                //획득 주기에 따라 획득
                mineList[i].curTime += Time.deltaTime;
                if(mineList[i].curTime > mineList[i].miningTime)
                {
                    mineList[i].curTime = 0f;
                    mineList[i].getAmount += mineList[i].getOnceAmount;
                    //획득 가능한 양에 도달
                    if (mineList[i].getAmount >= mineList[i].deposit)
                    {
                        mineList[i].getAmount = mineList[i].deposit;
                        mineList[i].miningState = false;    //채굴 완료




                    }
                }
            }
        }
    }

    public void setMineList(List<Mine> mine) { mineList = mine; }
    public void setMineList(Mine mine, int i) { mineList[i] = mine; }
    public List<Mine> getMineList() { return mineList; }
    public List<MineInfo> getMineInfoList() { return mineInfoList; }

    public void Unlock()
    {
        for(int i = 0; i < mineInfoList.Count; i++)
            if(mineInfoList[i].buildLevel <= Player.Play.level)
                mineInfoList[i].isLock = false;
    }



}


public class Mine
{
    private int mineNum;    //고유 번호
    public string type;     //종류
    public int level;       //레벨
    public string buildState;   // 건설 상태 - nothing, beunder, complete
    public float buildTime;     //건설 중 시간
    public string getThingName; //획득 가능 아이템 이름
    public int getAmount;       //획득한 양
    public int getOnceAmount;   //한 주기에 획득 가능한 양
    public int deposit;    //매장량
    public float miningTime;    //획득 주기
    public float curTime;       //현재 채굴 시간
    public bool miningState;    //채굴 상태     t채굴 중, f채굴 완료

    public Mine(int mineNum)
    {
        this.mineNum = mineNum;
        type = null;
        level = 0;
        buildState = "nothing";
        buildTime = 0f;
        getAmount = 0;
        getOnceAmount = 0;
        deposit = 0;
        miningTime = 0f;
        curTime = 0f;
        miningState = false;
    }


    public void setMineNum(int num) { mineNum = num; }
    public int getMineNum() { return mineNum; }
}

public class MineInfo
{
    public string type;         //종류
    public int buildLevel;      //건설 가능 레벨
    public float buildTime;     //걸리는 시간
    public string getThingName; //획득 가능 아이템 이름
    private string material;             //json으로 데이터 불러옴

    public string[] necessaryMaterials; //필요 재료
    public int[] necessaryMaterialsNum; //수량
    public bool isLock;

    public MineInfo(JsonData MineInfoData, int index)
    {
        this.type = MineInfoData["Mine"][index]["type"].ToString();
        this.buildLevel = (int)MineInfoData["Mine"][index]["buildLevel"];
        this.buildTime = (int)MineInfoData["Mine"][index]["buildTime"];
        this.getThingName = MineInfoData["Mine"][index]["getThingName"].ToString();
        this.material = MineInfoData["Mine"][index]["material"].ToString();
        char[] del = { ' ' };
        string[] words = material.Split(del, StringSplitOptions.RemoveEmptyEntries);
        this.necessaryMaterials = new string[words.Length / 2];
        this.necessaryMaterialsNum = new int[words.Length / 2];
        for (int i = 0; i < words.Length; i++)
        {
            if (i % 2 == 0)
                necessaryMaterials[i / 2] = words[i];
            else necessaryMaterialsNum[i / 2] = Convert.ToInt32(words[i]);
        }

        isLock = true;
    }

}

