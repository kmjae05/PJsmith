using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageData : MonoBehaviour {

    private int dist = 600; //스팟 거리 제한

    public static StageData instance = null;

    //스테이지 데이터
    private static List<StageInfo> stageInfoList;
    private StageInfo stageInfoCreate;
    private List<StageInfo> stageInfoListtmp;   //임시 스테이지 정보 저장공간

    //스팟 데이터
    public static List<Spot> spotList;

    //스테이지 정보
    private StageManager stageManager;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        stageInfoList = new List<StageInfo>();
        stageInfoListtmp = new List<StageInfo>();

        spotList = new List<Spot>();

        //각 대륙
        for (int j = 1; j < 6; j++)
        {
            //스테이지 생성
            for (int i = 1; i <= 20; i++)
            {
                stageInfoCreate = new StageInfo(contNumToString(j), i);
                stageInfoList.Add(stageInfoCreate);
            }
        }
        //스팟 생성
        for (int j = 1; j < 6; j++)
        {
            GameObject spottmp = GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage"+ (j).ToString() +"/CONUIPanel/Back/Spot").gameObject;
            int count = spottmp.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Spot create = new Spot(contNumToString(j), spottmp.transform.GetChild(i).name, spottmp.transform.GetChild(i));
                spotList.Add(create);
            }
        }
        //스테이지 생성
        for (int j = 1; j < 6; j++)
        {
            List<Spot> sList = spotList.FindAll(x => x.getContName() == contNumToString(j));
            for (int i = 0; i < 20; i++)
            {
                //랜덤 위치에 스테이지 버튼 생성
                int random = 0;
                int index = 0;
                while (true)
                {
                    random = Random.Range(1, sList.Count + 1);
                    index = spotList.FindIndex(x => x.getContName() == contNumToString(j) && x.getPosition().name == "spot" + random.ToString());
                    //이미 위치한 스테이지 범위에 없게 배치
                    List<StageInfo> stif = stageInfoList.FindAll(x => x.getContName() == spotList[index].getContName() && x.spotName != null);
                    if(stif != null) {
                        bool distanceBool = false;
                        for(int k=0; k < stif.Count; k++)
                        {
                            GameObject spottmp = GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage" + j.ToString() + "/CONUIPanel/Back/Spot/"+ stif[k].spotName).gameObject;
                            float disCul = Vector2.Distance(spotList[index].getPosition().transform.localPosition, spottmp.transform.localPosition);
                            if (disCul < dist) distanceBool = true;
                        }
                        if (distanceBool) continue;
                    }

                    if (!spotList[index].active) break;
                }
                spotList[index].active = true;

                //스팟이랑 스테이지 정보 공유
                StageInfo stin = stageInfoList.Find(x => x.getContName() == contNumToString(j) && x.spotName == null);

                spotList[index].stageNum = stin.getStageNum();    //스테이지 번호 저장
                stin.spotName = spotList[index].getPosition().name;

                random = Random.Range(1, 3 + 1);
                stin.type = typeNumToString(stin.getStageNum(), random);
                random = Random.Range(0, 100);
                if (random < 50) stin.typeNum = 1;
                else if (random > 49 && random < 90) stin.typeNum = 2;
                else if (random > 89) stin.typeNum = 3;
                stin.stageName = "stage" + stin.getStageNum().ToString();   // ex) stage1

                //스테이지 아이콘 변경
                stageImageChange(stin);

            }
        }
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
                if (stageInfoListtmp[i].time < stageInfoListtmp[i].getItemTime)
                {
                    stageInfoListtmp[i].getItemTimeFlag = false;

                    //아이템 획득
                    getItem(stageInfoListtmp[i]);

                    //flag
                    stageInfoListtmp[i].getRecentItemFlag = true;

                    //for (int k=0;k<4;k++)
                    //    Debug.Log(stageInfoListtmp[i].getItem[k]);
                }
            }
            else
            {
                stageInfoListtmp[i].getRecentItemFlag = false;
                //아이템 획득 시간 정하기
                stageInfoListtmp[i].getItemTimeFlag = true;
                float time = Random.Range(30f, 60f);
                stageInfoListtmp[i].getItemTime = stageInfoListtmp[i].time - time;
            }



            //시간
            stageInfoListtmp[i].time = stageInfoListtmp[i].time - Time.deltaTime;
            if (stageInfoListtmp[i].time <= 0)
            {
                stageInfoListtmp[i].time = 0;
                stageInfoListtmp[i].state = false;
                stageInfoListtmp[i].complete = true;
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
            stageInfoList[stageInfoList.FindIndex(x => x.getStageNum() == stageInfoListtmp[i].getStageNum() && x.getContName() == stageInfoListtmp[i].getContName())] = stageInfoListtmp[i];
        }


    }





    //스테이지 이미지 변경.
    public void stageImageChange(StageInfo stin)
    {
        if (stin.type == "동광")
            stin.sprite = Resources.Load<Sprite>("Gather/copper"+ stin.typeNum.ToString());
        else if (stin.type == "철광")
            stin.sprite = Resources.Load<Sprite>("Gather/iron" + stin.typeNum.ToString());
        else if (stin.type == "은광")
            stin.sprite = Resources.Load<Sprite>("Gather/silver" + stin.typeNum.ToString());
        else if (stin.type == "던전")
            stin.sprite = Resources.Load<Sprite>("Gather/minimonster" + stin.typeNum.ToString());
    }

    //아이템 획득
    public void getItem(StageInfo stin)
    {
        int rand = 0;
        if (stin.type == "동광")
        {   //구리30, 원석30, 철 가루30, 철10
            rand = Random.Range(0, 100);
            if (rand < 30) { stin.getItem[0] = "구리"; stin.getItemNum[0]++; stin.getRecentItem = stin.getItem[0]; stin.getRecentItemNum = 1; return; }
            else if (rand < 60) { stin.getItem[1] = "원석"; stin.getItemNum[1]++; stin.getRecentItem = stin.getItem[1]; stin.getRecentItemNum = 1; return; }
            else if (rand < 90) { stin.getItem[2] = "철 가루"; stin.getItemNum[2]++; stin.getRecentItem = stin.getItem[2]; stin.getRecentItemNum = 1; return; }
            else { stin.getItem[3] = "철"; stin.getItemNum[3]++; stin.getRecentItem = stin.getItem[3]; stin.getRecentItemNum = 1; return; }
        }
        else if (stin.type == "철광")
        {
            //철30, 구리30, 원석30, 은10
            rand = Random.Range(0, 100);
            if (rand < 30) { stin.getItem[0] = "철"; stin.getItemNum[0]++; stin.getRecentItem = stin.getItem[0]; stin.getRecentItemNum = 1; return; }
            else if (rand < 60) { stin.getItem[1] = "구리"; stin.getItemNum[1]++; stin.getRecentItem = stin.getItem[1]; stin.getRecentItemNum = 1; return; }
            else if (rand < 90) { stin.getItem[2] = "원석"; stin.getItemNum[2]++; stin.getRecentItem = stin.getItem[2]; stin.getRecentItemNum = 1; return; }
            else { stin.getItem[3] = "은"; stin.getItemNum[3]++; stin.getRecentItem = stin.getItem[3]; stin.getRecentItemNum = 1; return; }
        }
        else if (stin.type == "은광")
        {
            //은30, 철30, 구리30, 금10
            rand = Random.Range(0, 100);
            if (rand < 30) { stin.getItem[0] = "은"; stin.getItemNum[0]++; stin.getRecentItem = stin.getItem[0]; stin.getRecentItemNum = 1; return; }
            else if (rand < 60) { stin.getItem[1] = "철"; stin.getItemNum[1]++; stin.getRecentItem = stin.getItem[1]; stin.getRecentItemNum = 1; return; }
            else if (rand < 90) { stin.getItem[2] = "구리"; stin.getItemNum[2]++; stin.getRecentItem = stin.getItem[2]; stin.getRecentItemNum = 1; return; }
            else { stin.getItem[3] = "금"; stin.getItemNum[3]++; stin.getRecentItem = stin.getItem[3]; stin.getRecentItemNum = 1; return; }
        }
        else if (stin.type == "던전")
        {
            //하이그라스 단검30, 엘더 소드30, 팔라딘 소드30, 고급 하이그라스 단검10
            rand = Random.Range(0, 100);
            if (rand < 30) { stin.getItem[0] = "하이그라스 단검"; stin.getItemNum[0]++; stin.getRecentItem = stin.getItem[0]; stin.getRecentItemNum =1; return; }
            else if (rand < 60) { stin.getItem[1] = "엘더 소드"; stin.getItemNum[1]++; stin.getRecentItem = stin.getItem[1]; stin.getRecentItemNum = 1; return; }
            else if (rand < 90) { stin.getItem[2] = "팔라딘 소드"; stin.getItemNum[2]++; stin.getRecentItem = stin.getItem[2]; stin.getRecentItemNum =1; return; }
            else { stin.getItem[3] = "고급 하이그라스 단검"; stin.getItemNum[3]++; stin.getRecentItem = stin.getItem[3]; stin.getRecentItemNum = 1; return; }
        }

    }



    //대륙 번호->string
    public string contNumToString(int i) {
        if (i == 1) return "아케도니아";        else if (i == 2) return "플루오네";
        else if (i == 3) return "일사바드";        else if (i == 4) return "원무제국";
        else if (i == 5) return "드래곤로드"; else return null;
    }
    //대륙 string->번호
    public int contStringToInt(string i)
    {
        if (i == "아케도니아") return 1;
        else if (i == "플루오네") return 2;
        else if (i == "일사바드") return 3;
        else if (i == "원무제국") return 4;
        else if (i == "드래곤로드") return 5; else return 0;
    }

    //type int -> string
    public string typeNumToString(int stageNum, int i)
    {
        //채집
        if (stageNum <= 15) {
            if (i == 1) return "동광"; else if (i == 2) return "철광"; else if (i == 3) return "은광"; else return null;
        }
        //사냥
        else   {
            if (i == 1) return "던전"; else if (i == 2) return "던전"; else if (i == 3) return "던전"; else return null;
        }
        
    }

    public List<StageInfo> getStageInfoList() { return stageInfoList; }
    public void setStageInfoList(List<StageInfo> list) { stageInfoList = list; }

    public int getDist() { return dist; }


}


//나중에 DB로.
public class StageInfo
{
    //바뀌지 않는 data
    private string ContName;         //대륙 이름
    private int stageNum;           //스테이지 번호

    //바뀌는 data
    public string spotName;        //스팟 이름
    public string type;            //채집or몬스터 종류
    public int typeNum;            //1-소, 2-중, 3-대
    public string stageName;       //스테이지 이름
    public Sprite sprite;          //스프라이트

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
    public StageInfo() { wait = true; spotName = null; sprite = new Sprite(); getItem = new string[4]; getItemNum = new int[4]; }
    public StageInfo(string ContName, int stageNum)
    {
        this.ContName = ContName; this.stageNum = stageNum; this.wait = true; spotName = null; sprite = new Sprite();
        getItem = new string[4]; getItemNum = new int[4];
    }



    public string getContName() { return ContName; }
    public int getStageNum() { return stageNum; }

    //public void setSpotName(string n) { spotName = n; }
    //public string getSpotName() { return spotName; }

    //public string[] getGetItem() { return getItem; }

    //public string getStageName() { return stageName; }
    //public void setStageName(string n) { stageName = n; }
    //public void setState(bool b) { state = b; }
    //public bool getState() { return state; }
    //public void setMercenaryName(string n) { mercenaryName = n; }
    //public string getMercenaryName() { return mercenaryName; }
    //public void setTime(float t) { time = t; }
    //public float getTime() { return time; }
    //public void setComplete(bool b) { complete = b; }
    //public bool getComplete() { return complete; }
}


public class Spot{

    private string ContName;        //대륙 이름
    private string spotName;

    public Transform position;        //위치 localposition 사용
    public int stageNum;
    public bool active;     // 활성화/비활성화

    public Spot()    { active = false; }
    public Spot(string cont, string spotN, Transform pos) { this.ContName = cont; this.spotName = spotN; this.position = pos; active = false; }

    public string getContName() { return ContName; }
    public string getSpotName() { return spotName; }
    public Transform getPosition() { return position; }
}
