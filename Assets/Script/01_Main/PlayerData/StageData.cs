using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class StageData : MonoBehaviour
{

    private int dist = 600; //스팟 거리 제한

    public static StageData instance = null;
    //Json Data
    public WWW reader;
    static public JsonData PlunderData;

    static private List<Plunder> plunderList = new List<Plunder>();

    //스테이지 데이터
    private static List<StageInfo> stageInfoList;
    private StageInfo stageInfoCreate;
    private List<StageInfo> stageInfoListtmp;   //임시 스테이지 정보 저장공간
    //약탈 스팟 데이터
    private static List<PlunderInfo> plunderInfoList;


    //스팟 데이터
    public static List<Spot> spotList;

    //스테이지 정보
    private StageManager stageManager;
    private GameObject SpotObj;
    private MonsterData monsterData;

    //기상 변화
    private float delayTime = 30 * 60;  //30분마다
    private float delayDeltaTime;
    private float rainTime = 5 * 60;        //5분
    private float rainDeltaTime;
    public bool rainFlag;          //플래그

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Plunder.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            PlunderData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Plunder.json");
            PlunderData = JsonMapper.ToObject(tmp);
        }
        for (int i = 0; i < PlunderData["Plunder"].Count; i++)
        {
            plunderList.Add(new Plunder(PlunderData, i));
        }

        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageInfoList = new List<StageInfo>();
        stageInfoListtmp = new List<StageInfo>();
        plunderInfoList = new List<PlunderInfo>();
        monsterData = GameObject.Find("StageManager").GetComponent<MonsterData>();

        SpotObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Spot").gameObject;
        spotList = new List<Spot>();

        //스테이지 생성
        for (int i = 1; i <= 20; i++)
        {
            stageInfoCreate = new StageInfo(i);
            stageInfoList.Add(stageInfoCreate);
            plunderInfoList.Add(new PlunderInfo(i));
        }
        //스팟 생성
        GameObject spottmp = SpotObj;
        int count = spottmp.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Spot create = new Spot(spottmp.transform.GetChild(i).name, spottmp.transform.GetChild(i));
            spotList.Add(create);
        }
        //스테이지 위치
        for (int i = 0; i < 20; i++)
        {
            //랜덤 위치에 스테이지 버튼 생성
            int random = 0;
            int index = 0;
            while (true)
            {
                random = UnityEngine.Random.Range(1, 100 + 1);
                index = spotList.FindIndex(x => x.getPosition().name == "spot" + random.ToString());
                //이미 위치한 스테이지 범위에 없게 배치
                List<StageInfo> stif = stageInfoList.FindAll(x => x.spotName != null);
                if (stif != null)
                {
                    bool distanceBool = false;
                    for (int k = 0; k < stif.Count; k++)
                    {
                        GameObject spottemp = SpotObj.transform.Find(stif[k].spotName).gameObject;
                        float disCul = Vector2.Distance(spotList[index].getPosition().transform.localPosition, spottemp.transform.localPosition);
                        if (disCul < dist) distanceBool = true;
                    }
                    if (distanceBool) continue;
                }

                if (!spotList[index].stageActive) break;
            }
            spotList[index].stageActive = true;

            //스팟이랑 스테이지 정보 공유
            StageInfo stin = stageInfoList.Find(x => x.spotName == null);

            spotList[index].stageNum = stin.getStageNum();    //스테이지 번호 저장
            stin.spotName = spotList[index].getPosition().name;

            //몬스터 종류
            random = UnityEngine.Random.Range(1, 3 + 1);
            stin.type = typeNumToString(random);
            random = UnityEngine.Random.Range(1, 4);
            stin.typeNum = random;
            stin.stageName = "stage" + stin.getStageNum().ToString();   // ex) stage1

            //스테이지 아이콘 변경
            //stageImageChange(stin);


            //랜덤 위치에 약탈 버튼 생성
            while (true)
            {
                random = UnityEngine.Random.Range(1, 100 + 1);
                index = spotList.FindIndex(x => x.getPosition().name == "spot" + random.ToString());
                //이미 위치한 스테이지 범위에 없게 배치
                List<PlunderInfo> plif = plunderInfoList.FindAll(x => x.spotName != null);
                if (plif != null)
                {
                    bool distanceBool = false;
                    for (int k = 0; k < plif.Count; k++)
                    {
                        GameObject spottemp = SpotObj.transform.Find(plif[k].spotName).gameObject;
                        float disCul = Vector2.Distance(spotList[index].getPosition().transform.localPosition, spottemp.transform.localPosition);
                        if (disCul < dist) distanceBool = true;
                    }
                    if (distanceBool) continue;
                }

                if (!spotList[index].plunderActive) break;
            }
            spotList[index].plunderActive = true;

            //스팟이랑 스테이지 정보 공유
            PlunderInfo plin = plunderInfoList.Find(x => x.spotName == null);

            spotList[index].plunderNum = plin.getPlunderNum();    //스테이지 번호 저장
            plin.spotName = spotList[index].getPosition().name;

            plin.PlunderName = "plunder" + plin.getPlunderNum().ToString();   // ex) plunder1

            //랜덤으로 리스트에 ai 정보 넣기
            while (true)
            {
                random = UnityEngine.Random.Range(0, 40);
                //중복 방지
                List<PlunderInfo> plif = plunderInfoList.FindAll(x => x.opponentName != null);  //이미 할당 된
                if (plif != null)
                {
                    bool flag = false;
                    for (int k = 0; k < plif.Count; k++)
                    {
                        if (plif[k].opponentName == plunderList[random].getName())
                        {
                            flag = true;
                        }
                    }
                    if (flag) continue; else break;
                }
                else break;
            }
            plunderInfoList[i].opponentName = plunderList[random].getName();
            plunderList[random].assignment = true;



        }
    }

    private void Start()
    {


        stageManager.setStageInfoList(stageInfoList);

    }

    private void Update()
    {
        stageInfoListtmp.Clear();
        //state true 상태 검색
        stageInfoListtmp = stageInfoList.FindAll(x => x.state == true);
        //탐험
        for (int i = 0; i < stageInfoListtmp.Count; i++)
        {
            //아이템 획득
            if (stageInfoListtmp[i].getItemTimeFlag)
            {
                if (stageInfoListtmp[i].time <= stageInfoListtmp[i].getItemTime)
                {
                    stageInfoListtmp[i].getItemTimeFlag = false;

                    //아이템 획득
                    getItem(stageInfoListtmp[i]);

                    //flag
                    stageInfoListtmp[i].getRecentItemFlag = true;
                    if (stageInfoListtmp[i].time <= 0)
                    {
                        stageInfoListtmp[i].getItemTimeFlag = false;
                    }
                }
            }
            else
            {
                stageInfoListtmp[i].getRecentItemFlag = false;
                //아이템 획득 시간 정하기
                stageInfoListtmp[i].getItemTimeFlag = true;
                float time = 10f;
                stageInfoListtmp[i].getItemTime = stageInfoListtmp[i].time - time;

            }



            //시간
            stageInfoListtmp[i].time = stageInfoListtmp[i].time - Time.deltaTime;
            if (stageInfoListtmp[i].time <= 0 && !stageInfoListtmp[i].getItemTimeFlag)
            {
                Debug.Log("t");
                stageInfoListtmp[i].time = 0;
                stageInfoListtmp[i].state = false;
                stageInfoListtmp[i].complete = true;
                //퀘스트 카운트
                if (stageInfoListtmp[i].type == QuestData.instance.getQuestList().Find(x => x.type == "hunting").target)
                    QuestData.questHunting += stageInfoListtmp[i].typeNum;
                else if (QuestData.instance.getQuestList().Find(x => x.type == "hunting").target == "몬스터 아무거나")
                    QuestData.questHunting += stageInfoListtmp[i].typeNum;

                QuestData.questWeeklyHunting += stageInfoListtmp[i].typeNum;
            }
        }
        //리젠 시간
        stageInfoListtmp = stageInfoList.FindAll(x => x.regen == true);
        for (int i = 0; i < stageInfoListtmp.Count; i++)
        {
            stageInfoListtmp[i].time = stageInfoListtmp[i].time - Time.deltaTime;
            if (stageInfoListtmp[i].time <= 0)
            {
                stageInfoListtmp[i].time = 0;
                stageInfoListtmp[i].regen = false;
                stageInfoListtmp[i].wait = true;
            }
        }

        //리스트에 다시 저장. 스테이지 검색.
        for (int i = 0; i < stageInfoListtmp.Count; i++)
        {
            stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == stageInfoListtmp[i].getStageNum())] = stageInfoListtmp[i];
        }




        //약탈 리젠
        List<PlunderInfo> plunderInfotmp = plunderInfoList.FindAll(x => x.regen == true);
        for (int i = 0; i < plunderInfoList.Count; i++)
        {
            plunderInfoList[i].time = plunderInfoList[i].time - Time.deltaTime;
            if (plunderInfoList[i].time <= 0)
            {
                plunderInfoList[i].time = 0;
                plunderInfoList[i].regen = false;
            }
        }


        //기상
        if (!rainFlag)
            delayDeltaTime += Time.deltaTime;
        if (delayDeltaTime > delayTime)
        {
            delayDeltaTime = 0;
            rainFlag = true;
        }
        if (rainFlag)
            rainDeltaTime += Time.deltaTime;
        if (rainDeltaTime > rainTime)
        {
            rainDeltaTime = 0;
            rainFlag = false;
        }

    }

    //아이템 획득
    public void getItem(StageInfo stin)
    {
        int rand = 0;
        for (int i = 0; i < monsterData.getMonsterList().Count; i++)
        {
            //몬스터 확인
            if (stin.type == monsterData.getMonsterList()[i].name)
            {
                //확률 체크
                int prob = 0;
                rand = UnityEngine.Random.Range(1, 101);
                for (int j = 0; j < monsterData.getMonsterList()[i].itemName.Length; j++)
                {
                    if (rand <= prob + monsterData.getMonsterList()[i].itemProb[j] && rand > prob)
                    {
                        stin.getItem[j] = monsterData.getMonsterList()[i].itemName[j];
                        stin.getItemNum[j] += monsterData.getMonsterList()[i].itemAmount[j];
                        stin.getRecentItem = stin.getItem[j]; stin.getRecentItemNum = 1;
                        //Debug.Log(stin.getRecentItem);
                        return;
                    }
                    else { prob += monsterData.getMonsterList()[i].itemProb[j]; }
                }
            }
        }

    }


    //type int -> string
    public string typeNumToString(int i)
    {
        ////사냥
        if (i == 1) return "전갈"; else if (i == 2) return "오쿰"; else if (i == 3) return "인큐버스"; else return null;
    }


    public List<Spot> getSpotList() { return spotList; }
    public void setSpotList(List<Spot> list) { spotList = list; }
    public List<StageInfo> getStageInfoList() { return stageInfoList; }
    public void setStageInfoList(List<StageInfo> list) { stageInfoList = list; }
    public List<PlunderInfo> getPlunderInfoList() { return plunderInfoList; }
    public void setPlunderInfoList(List<PlunderInfo> list) { plunderInfoList = list; }
    public List<Plunder> getPlunderList() { return plunderList; }
    public void setPlunderList(List<Plunder> list) { plunderList = list; }

    public int getDist() { return dist; }


}



//사냥 스팟
[Serializable]
public class StageInfo
{
    //바뀌지 않는 data
    private int stageNum;           //본인 번호

    //바뀌는 data
    public string spotName;        //위치한 스팟 이름
    public string type;            //몬스터 종류
    public int typeNum;            //1-소, 2-중, 3-대
    public string stageName;       //오브젝트 이름

    public bool state;             //진행 상태
    public bool complete;          //완료
    public bool wait;              //탐험 대기
    public bool regen;              //리젠 중

    public string mercenaryName;   //용병 이름
    public float time;             //남은 시간

    public string[] getItem;       //전체 획득한 아이템
    public int[] getItemNum;       //전체 획득한 아이템 수량
    public float getItemTime;       //아이템 획득 가능한 시간
    public bool getItemTimeFlag;    //아이템 획득 시간 기록
    public string getRecentItem;   //최근 획득한 아이템
    public int getRecentItemNum;    //최근 획득한 아이템 수
    public bool getRecentItemFlag;  //아이템 획득 타이밍

    //생성자
    public StageInfo() { wait = true; spotName = null;  getItem = new string[30]; getItemNum = new int[30]; }
    public StageInfo(int stageNum)
    {
        this.stageNum = stageNum; this.wait = true; spotName = null;
        getItem = new string[30]; getItemNum = new int[30];
    }

    public int getStageNum() { return stageNum; }

}

//스팟 고정 위치
[Serializable]
public class Spot
{
    private string spotName;

    public Transform position;        //위치 localposition 사용
    public int stageNum;            //사냥 스팟 이름
    public int plunderNum;          //약탈 스팟 이름
    public bool stageActive;        // 활성화/비활성화
    public bool plunderActive;        // 활성화/비활성화

    public Spot() { stageActive = false; plunderActive = false; }
    public Spot(string spotN, Transform pos) { this.spotName = spotN; this.position = pos; stageActive = false; plunderActive = false; }

    public string getSpotName() { return spotName; }
    public Transform getPosition() { return position; }
}


//약탈 스팟
[Serializable]
public class PlunderInfo
{
    //바뀌지 않는 data
    //private string ContName;        //대륙 이름
    private int PlunderNum;         //고유 번호

    //바뀌는 data
    public string spotName;        //위치한 스팟 이름
    public string PlunderName;     //오브젝트 이름
    public string opponentName;     //상대 이름

    public bool state;             //진행 상태
    public bool regen;              //리젠 중
    public float time;             //남은 시간

    public string[] getItem;        //전체 획득한 아이템
    public int[] getItemNum;        //전체 획득한 아이템 수량
    public float getItemTime;       //아이템 획득 가능한 시간
    public bool getItemTimeFlag;    //아이템 획득 시간 기록
    public string getRecentItem;    //최근 획득한 아이템
    public int getRecentItemNum;    //최근 획득한 아이템 수
    public bool getRecentItemFlag;  //아이템 획득 타이밍

    //생성자
    public PlunderInfo() { spotName = null; getItem = new string[5]; getItemNum = new int[5]; }
    public PlunderInfo(int PlunderNum)
    {
        this.PlunderNum = PlunderNum; spotName = null; opponentName = null;
        getItem = new string[5]; getItemNum = new int[5];
    }

    public int getPlunderNum() { return PlunderNum; }

}


//AI 상대 40명
[Serializable]
public class Plunder
{
    private string user_id;
    private string Name;
    public int level;
    public string mercenary;     //대표 캐릭터

    //능력치
    public Stat stat;

    public string[] getItem;       //획득 가능한 아이템
    public int[] getItemWinProb;       //획득 가능한 아이템 승리했을 때 얻는 확률
    public int[] getItemLoseProb;       //획득 가능한 아이템 패배했을 때 얻는 확률

    public bool assignment;         //스팟에 할당 여부

    public Plunder(JsonData data, int index)
    {
        this.user_id = data["Plunder"][index]["user_id"].ToString();
        this.Name = data["Plunder"][index]["Name"].ToString();
        this.level = (int)data["Plunder"][index]["level"];
        this.mercenary = data["Plunder"][index]["mercenary"].ToString();
        stat = new Stat();
        this.stat.dps = (int)data["Plunder"][index]["dps"];
        this.stat.strPower = (int)data["Plunder"][index]["strPower"];
        this.stat.attackSpeed = (int)data["Plunder"][index]["attackSpeed"];
        this.stat.focus = (int)data["Plunder"][index]["focus"];
        this.stat.critical = (int)data["Plunder"][index]["critical"];
        this.stat.defPower = (int)data["Plunder"][index]["defPower"];
        this.stat.evaRate = (int)data["Plunder"][index]["evaRate"];

        this.getItem = new string[5];
        for(int i = 0; i < 5; i++)
        {
            List<Things> things = ThingsData.instance.getThingsList().FindAll(x => x.grade <= 3);
            int rand = UnityEngine.Random.Range(0, things.Count);

            getItem[i] = things[rand].name;
        }
        this.getItemWinProb = new int[5];
        this.getItemWinProb[0] = 100; this.getItemWinProb[1] = 50; this.getItemWinProb[2] = 50;
        this.getItemWinProb[3] = 50; this.getItemWinProb[4] = 50;
        this.getItemLoseProb = new int[5];
        this.getItemLoseProb[0] = 10; this.getItemLoseProb[1] = 10; this.getItemLoseProb[2] = 10;
        this.getItemLoseProb[3] = 0; this.getItemLoseProb[4] = 0;


        this.assignment = false;
    }

    public string getUser_id() { return user_id; }
    public string getName() { return Name; }

}