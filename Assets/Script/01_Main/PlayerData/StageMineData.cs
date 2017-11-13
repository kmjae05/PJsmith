using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMineData : MonoBehaviour
{

    public static StageMineData instance = null;

    //광산 데이터
    static private List<Mine> mineList;
    static private List<MineBuild> mineBuildList = new List<MineBuild>();

    StageData stageData;


    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {

        stageData = GameObject.Find("StageData").GetComponent<StageData>();
        mineBuildList = MineData.instance.getMineBuildList();

        //광산 생성
        mineList = new List<Mine>();
        for (int i = 0; i < 2; i++)
        {
            mineList.Add(new Mine(i + 100));

            int random = 0;
            int index = 0;
            //범위 상관없이 배치
            while (true)
            {
                random = Random.Range(1, 100 + 1);
                index = StageData.spotList.FindIndex(x => x.getPosition().name == "spot" + random.ToString());
                if (!StageData.spotList[index].stageActive) break;
            }
            StageData.spotList[index].stageActive = true;

            StageData.spotList[index].stageNum = mineList[i].getMineNum();    //스테이지 번호 저장
            mineList[i].spotName = StageData.spotList[index].getPosition().name;
            mineList[i].stageName = "mine" + mineList[i].getMineNum().ToString();   // ex) mine100
            //광산 종류
            if (i == 0)
            {
                mineList[i].type = "검은무쇠";
            }
            else { mineList[i].type = "아케나이트"; }
            mineList[i].level = MineData.instance.getMineInfoList().Find(x => x.type == mineList[i].type).level;
            mineList[i].deposit = mineBuildList.Find(x => x.level == mineList[i].level).deposit;
        }
    }

    void Update()
    {
        //광산
        for (int i = 0; i < mineList.Count; i++)
        {
            //건설 진행 중
            if (mineList[i].buildState == "upgrade")
            {
                mineList[i].buildTime -= Time.deltaTime;
                if (mineList[i].buildTime < 0)
                {
                    mineList[i].buildTime = 0f;
                    if (mineList[i].buildState == "upgrade")
                    {
                        mineList[i].level = MineData.instance.getMineInfoList().Find(x => x.type == mineList[i].type).afterLevel;
                        mineList[i].deposit = mineBuildList.Find(x => x.level == mineList[i].level).deposit;
                        MineData.instance.getMineInfoList().Find(x => x.type == mineList[i].type).level = MineData.instance.getMineInfoList().Find(x => x.type == mineList[i].type).afterLevel;
                        MineData.instance.getMineInfoList().Find(x => x.type == mineList[i].type).buildTime = mineBuildList.Find(x => x.level == MineData.instance.getMineInfoList().Find(y => y.type == mineList[i].type).level).time;
                    }

                    mineList[i].buildState = "complete";
                    mineList[i].miningState = true;
                    mineList[i].getAmount = 0;
                }
            }
            //채굴 진행 중
            if (mineList[i].miningState)
            {
                //Debug.Log("채굴진행중");
                //획득 주기에 따라 획득
                mineList[i].curTime += Time.deltaTime;
                if (mineList[i].curTime > mineList[i].miningTime)
                {
                    mineList[i].curTime = 0f;

                    mineList[i].getAmount += mineList[i].getOnceAmount;

                    //확률에 따른 아이템 획득
                    for (int j = 1; j < mineList[i].getThingName.Length; j++)
                    {
                        if (mineList[i].getThingName[j] != null)
                        {
                            int random = UnityEngine.Random.Range(1, 100 + 1);      //100확률
                            //Debug.Log(random);
                            int prob = MineData.instance.getMineInfoList().Find(x => x.type == mineList[i].type).getThingProb[j];  //아이템 확률
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
                        Debug.Log("획득 가능한 양에 도달");
                        mineList[i].getAmount = mineList[i].deposit;
                        mineList[i].miningState = false;    //채굴 완료
                    }
                }
            }
        }
    }




    public List<Mine> getMineList() { return mineList; }
    public void setMineList(List<Mine> list) { mineList = list; }


}
