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
    static public JsonData MineBuildData;

    static private List<MineInfo> mineInfoList = new List<MineInfo>();
    static private List<MineBuild> mineBuildList = new List<MineBuild>();


    static private List<Mine> mineList;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        Debug.Log("MineDataAwake");


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
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "MineBuild.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            MineBuildData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp =  File.ReadAllText(Application.dataPath + "/StreamingAssets/MineBuild.json");
            MineBuildData = JsonMapper.ToObject(tmp);
        }


        for (int i = 0; i < MineInfoData["Mine"].Count; i++)
        {
            mineInfoList.Add(new MineInfo(MineInfoData, i));
        }
        for (int i = 0; i < MineBuildData["MineBuild"].Count; i++)
        {
            mineBuildList.Add(new MineBuild(MineBuildData, i));
        }

        mineList = new List<Mine>();

        for(int i = 0; i < 10; i++)
        {
            mineList.Add(new Mine(i));
        }

        //레벨에 따른 건설 시간 설정
        for(int i=0;i< mineInfoList.Count; i++)
        {
            mineInfoList[i].buildTime = mineBuildList.Find(x => x.level == mineInfoList[i].level).time;
        }

    }

    void Start ()
    {
        Debug.Log("MineDataStart");

	}

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            //건설 진행 중
            if (mineList[i].buildState == "beunder" || mineList[i].buildState == "upgrade")
            {
                mineList[i].buildTime -= Time.deltaTime;
                if (mineList[i].buildTime < 0)
                {
                    mineList[i].buildTime = 0f;
                    if (mineList[i].buildState == "upgrade")
                    {
                        Debug.Log(mineList[i].level);
                        mineList[i].deposit = mineBuildList.Find(x => x.level == mineList[i].level).deposit;
                        mineInfoList.Find(x => x.type == mineList[i].type).level++;
                        mineInfoList.Find(x => x.type == mineList[i].type).buildTime = mineBuildList.Find(x=>x.level == mineInfoList.Find(y => y.type == mineList[i].type).level).time;
                        mineInfoList.Find(x => x.type == mineList[i].type).upgradeState = false;
                    }

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
                    //확률에 따른 아이템 획득
                    for(int j = 1; j < mineList[i].getThingName.Length; j++)
                    {
                        if (mineList[i].getThingName[j] != null)
                        {
                            int random = UnityEngine.Random.Range(1, 100 + 1);      //100확률
                            //Debug.Log(random);
                            int prob = mineInfoList.Find(x => x.type == mineList[i].type).getThingProb[j];  //아이템 확률
                            if (random <= prob)
                            {
                                //Debug.Log(mineList[i].getThingName[j] + " 획득1");
                                mineList[i].getThingNum[j]++;
                            }
                        }
                    }

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
    public void setMineInfoList(List<MineInfo> mine) { mineInfoList = mine; }
    public List<Mine> getMineList() { return mineList; }
    public List<MineInfo> getMineInfoList() { return mineInfoList; }
    public List<MineBuild> getMineBuildList() { return mineBuildList; }

    public void Unlock()
    {
        for(int i = 0; i < mineInfoList.Count; i++)
            if(mineInfoList[i].buildLevel <= Player.instance.getUser().level)
                mineInfoList[i].isLock = false;
    }



}

[Serializable]
public class Mine
{
    private int mineNum;    //고유 번호
    public string type;     //종류
    public int level;       //레벨
    public string buildState;   // 건설 상태 - nothing, beunder, complete, exhaustion, upgrade
    public float buildTime;     //건설 중 시간
    public string[] getThingName; //획득 가능 아이템 이름 json
    public int[] getThingNum;   //획득한 부가 아이템

    public int getAmount;       //획득한 양
    public int getOnceAmount;   //한 주기에 획득 가능한 양
    public int deposit;         //매장량
    public float miningTime;    //획득 주기
    public float curTime;       //현재 채굴 시간
    public bool miningState;    //채굴 상태     t채굴 중, f채굴 완료
    public bool boostState;     //부스트 아이템 사용 상태

    //스테이지에서 위치
    public string spotName;     //위치한 곳 이름
    public string stageName;    //스테이지에서 보일 이름


    public Mine(int mineNum)
    {
        this.mineNum = mineNum;
        type = null;
        level = 0;
        buildState = "nothing";
        buildTime = 0f;
        getThingName = new string[3];
        getThingNum = new int[3];
        getAmount = 0;
        getOnceAmount = 0;
        deposit = 0;
        miningTime = 0f;
        curTime = 0f;
        miningState = false;
        boostState = false;

        spotName = null;
    }
    public Mine() { }

    public void setMineNum(int num) { mineNum = num; }
    public int getMineNum() { return mineNum; }
}

[Serializable]
public class MineInfo
{
    public string type;         //종류
    public int buildLevel;      //건설 가능 레벨
    public float buildTime;     //걸리는 시간
    private string getThingNameData; //json으로 불러옴 획득 가능 아이템 이름
    public string[] getThingName; //획득 가능 아이템 이름
    public int[] getThingProb; //획득 가능 아이템 확률

    private string material;             //json으로 데이터 불러옴
    public string[] necessaryMaterials; //필요 재료
    public int[] necessaryMaterialsNum; //수량

    public int level;
    //업그레이드 상태
    public bool upgradeState = false;

    //레벨업 시 비교
    public int afterLevel;
    public int afterTime;
    public int afterDeposit;
    public int curMaterial;
    public bool upgradeFlag = false;

    public bool isLock;

    public MineInfo(JsonData MineInfoData, int index)
    {
        this.type = MineInfoData["Mine"][index]["type"].ToString();
        this.buildLevel = (int)MineInfoData["Mine"][index]["buildLevel"];
        this.buildTime = (int)MineInfoData["Mine"][index]["buildTime"];

        this.getThingNameData = MineInfoData["Mine"][index]["getThingName"].ToString();
        char[] dele = { ' ' };
        string[] words = getThingNameData.Split(dele, StringSplitOptions.RemoveEmptyEntries);
        this.getThingName = new string[words.Length / 2];
        this.getThingProb = new int[words.Length / 2];
        for (int i = 0; i < words.Length; i++)
        {
            if (i % 2 == 0)
                getThingName[i / 2] = words[i];
            else getThingProb[i / 2] = Convert.ToInt32(words[i]);
            
        }

        this.material = MineInfoData["Mine"][index]["material"].ToString();
        char[] del = { ' ' };
        string[] words2 = material.Split(del, StringSplitOptions.RemoveEmptyEntries);
        this.necessaryMaterials = new string[words2.Length / 2];
        this.necessaryMaterialsNum = new int[words2.Length / 2];
        for (int i = 0; i < words2.Length; i++)
        {
            if (i % 2 == 0)
                necessaryMaterials[i / 2] = words2[i];
            else necessaryMaterialsNum[i / 2] = Convert.ToInt32(words2[i]);
        }

        level = 1;
        afterLevel = level;
        isLock = true;
    }

    public MineInfo()
    {
        level = 1;
        afterLevel = level;
        isLock = true;
    }

}


//레벨에 따른 자원량과 레벨업 기준 등
public class MineBuild
{
    public int level;
    public int deposit;
    public int material;
    public int time;

    public MineBuild(JsonData MineInfoData, int index)
    {
        this.level = (int)MineInfoData["MineBuild"][index]["level"];
        this.deposit = (int)MineInfoData["MineBuild"][index]["deposit"];
        this.material = (int)MineInfoData["MineBuild"][index]["material"];
        this.time = (int)MineInfoData["MineBuild"][index]["time"];
    }



}
