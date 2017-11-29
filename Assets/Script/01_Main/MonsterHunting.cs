using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHunting : MonoBehaviour {


    private float[] dist;                     //떨어진 거리
    private System.DateTime[] leadTime;       //걸리는 시간
    private int speed = 10;                   //속력

    private GameObject WorldMapBackObj;
    private GameObject mercenaryObj;
    private List<Mercenary> mercenary;
    private GameObject merObj;

    private StageManager stageManager;
    private MonsterData monsterData;

    private void Start()
    {
        WorldMapBackObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back").gameObject;
        mercenaryObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Mercenary").gameObject;
        mercenary = MercenaryData.instance.getMercenary();
        merObj = new GameObject();

        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        monsterData = GameObject.Find("StageManager").GetComponent<MonsterData>();

        StartCoroutine(loop());
        StartCoroutine(MonsterAttack());
        StartCoroutine(MercenaryAttack());
    }



    IEnumerator loop()
    {
        while (true)
        {
            for (int i = 0; i < mercenary.Count;i++)
            {
                StageInfo info = StageData.instance.getStageInfoList().Find(y => y.getStageNum() == mercenary[i].stageNum);
                if (mercenary[i].active =="go")
                {
                    Vector3 spot = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage/stage"+ mercenary[i].stageNum +"Button").gameObject.transform.localPosition;
                    
                    //용병 오브젝트 찾기
                    for(int k = 0; i < MercenaryData.instance.getMercenary().Count; k++)
                    {
                        if(mercenaryObj.transform.GetChild(k).Find("NameText").gameObject.GetComponent<Text>().text 
                            == mercenary[i].getName())
                        {
                            merObj = mercenaryObj.transform.GetChild(k).gameObject;
                            break;
                        }
                    }

                    merObj.SetActive(true);
                    merObj.transform.Find("TimeSlider").gameObject.SetActive(false);
                    //merObj.transform.localPosition.Set(mercenary[i].posX, mercenary[i].posY, 0);
                    //남은 시간 계산
                    System.TimeSpan leadTime = System.DateTime.Now - info.time;
                    //방향
                    Vector3 dir = (spot - GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.transform.localPosition).normalized;
                    //간 거리
                    Vector3 dist = new Vector3(dir.x * (float)leadTime.TotalSeconds * speed, dir.y * (float)leadTime.TotalSeconds * speed, 0);

                    merObj.transform.localPosition = dist;
                    mercenary[i].posX = merObj.transform.localPosition.x;
                    mercenary[i].posY = merObj.transform.localPosition.y;
                    
                    //위치 도착
                    if(System.DateTime.Now > info.time + info.leadTime)
                    {
                        mercenary[i].active = "hunt";
                        info.mermove = false;
                        info.state = true;
                        merObj.SetActive(false);
                        GameObject mercImage = WorldMapBackObj.transform.Find("Stage/" + info.stageName + "Button/MercImage").gameObject;
                        mercImage.SetActive(true);
                        string merImageName = info.mercenaryName;
                        mercImage.transform.Find("Image").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Mercenary/" + merImageName);

                    }
                }
                //사냥 마치고 귀환
                else if(mercenary[i].active == "back")
                {
                    //마지막 위치에서 영지 방향으로.
                    Vector3 spot = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Spot/" + info.spotName).gameObject.transform.localPosition;

                    //용병 오브젝트 찾기
                    for (int k = 0; i < MercenaryData.instance.getMercenary().Count; k++)
                    {
                        if (mercenaryObj.transform.GetChild(k).Find("NameText").gameObject.GetComponent<Text>().text
                            == mercenary[i].getName())
                        {
                            merObj = mercenaryObj.transform.GetChild(k).gameObject;
                            break;
                        }
                    }

                    merObj.SetActive(true);
                    merObj.transform.Find("TimeSlider").gameObject.SetActive(false);
                    //남은 시간 계산
                    System.TimeSpan leadTime = System.DateTime.Now - info.time;
                    //방향
                    Vector3 dir = (GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.transform.localPosition - spot).normalized;
                    //간 거리
                    Vector3 dist = new Vector3(dir.x * (float)leadTime.TotalSeconds * speed, dir.y * (float)leadTime.TotalSeconds * speed, 0);
                    Debug.Log(dist.x + " " + dist.y);
                    merObj.transform.localPosition = spot + dist;
                    mercenary[i].posX = merObj.transform.localPosition.x;
                    mercenary[i].posY = merObj.transform.localPosition.y;

                    //위치 도착
                    if (System.DateTime.Now > info.time + info.leadTime)
                    {
                        mercenary[i].active = "ready";
                        mercenary[i].state = false;
                        info.mermove = false;
                        merObj.SetActive(false);
                        GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + info.mercenaryName + "Selection").GetComponent<Button>().interactable = true;
                        Debug.Log("도착");
                    }




                }

                //사냥
                else if(mercenary[i].active == "hunt")
                {
                    //공격주기 최소 1초

                    //몬스터 체력
                    GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.SetActive(true);
                    GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().maxValue = 10000 * info.typeNum;
                    GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().value = info.monsterHP;
                    GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider/TimeText").gameObject.GetComponent<Text>().text
                        = (int)(GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().value / GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().maxValue * 100) + "%";

                    GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.SetActive(true);
                    GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.GetComponent<Slider>().maxValue = 1000;
                    GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.GetComponent<Slider>().value = mercenary[i].stat.HP;
                    GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider/TimeText").gameObject.GetComponent<Text>().text
                        = (int)(GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.GetComponent<Slider>().value / GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.GetComponent<Slider>().maxValue * 100) + "%";


                }

                //회복 //임시
                else if(mercenary[i].active == "recovery")
                {
                    //마지막 위치에서 영지 방향으로.
                    Vector3 spot = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Spot/" + info.spotName).gameObject.transform.localPosition;

                    //용병 오브젝트 찾기
                    for (int k = 0; i < MercenaryData.instance.getMercenary().Count; k++)
                    {
                        if (mercenaryObj.transform.GetChild(k).Find("NameText").gameObject.GetComponent<Text>().text
                            == mercenary[i].getName())
                        {
                            merObj = mercenaryObj.transform.GetChild(k).gameObject;
                            break;
                        }
                    }

                    merObj.SetActive(true);
                    merObj.transform.Find("TimeSlider").gameObject.SetActive(false);
                    //남은 시간 계산
                    System.TimeSpan leadTime = System.DateTime.Now - info.time;
                    //방향
                    Vector3 dir = (GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.transform.localPosition - spot).normalized;
                    //간 거리
                    Vector3 dist = new Vector3(dir.x * (float)leadTime.TotalSeconds * speed, dir.y * (float)leadTime.TotalSeconds * speed, 0);
                    Debug.Log(dist.x + " " + dist.y);
                    merObj.transform.localPosition = spot + dist;
                    mercenary[i].posX = merObj.transform.localPosition.x;
                    mercenary[i].posY = merObj.transform.localPosition.y;

                    //위치 도착
                    if (System.DateTime.Now > info.time + info.leadTime)
                    {
                        mercenary[i].active = "ready";
                        mercenary[i].state = false;
                        info.mermove = false;
                        merObj.SetActive(false);
                        GameObject.Find("System").transform.Find("StagePopup/UIPanel/MercenaryBox/Mercenary" + info.mercenaryName + "Selection").GetComponent<Button>().interactable = true;
                        Debug.Log("도착");
                    }
                }

            }



            yield return null;
        }


    }




    //몬스터 공격 주기
    IEnumerator MonsterAttack()
    {
        while (true)
        {
            for (int i = 0; i < mercenary.Count; i++)
            {
                StageInfo info = StageData.instance.getStageInfoList().Find(y => y.getStageNum() == mercenary[i].stageNum);
                if (info != null && info.state)
                {
                    int index = StageData.instance.getStageInfoList().FindIndex(x => x.getStageNum() == mercenary[i].stageNum);

                    //공격 주기 시간 체크
                    info.monsterAttackTime += Time.deltaTime;
                    if (info.monsterAttackSpeed < info.monsterAttackTime)
                    {
                        info.monsterAttackTime = 0;
                        stageManager.getMonsterObjList()[index].GetComponent<Animation>().Play("attack");
                        mercenary[i].stat.HP -= monsterData.getMonsterList().Find(z => z.name == info.type).stat.dps * info.typeNum;
                        Debug.Log(mercenary[i].stat.HP);
                    }
                    if (!stageManager.getMonsterObjList()[index].GetComponent<Animation>().IsPlaying("attack")
                        && !stageManager.getMonsterObjList()[index].GetComponent<Animation>().IsPlaying("damage"))
                        stageManager.getMonsterObjList()[index].GetComponent<Animation>().CrossFade("stand");

                    //용병 패배
                    if (mercenary[i].stat.HP < 0)
                    {
                        info.state = false;
                        mercenary[i].active = "recovery";
                        stageManager.getMonsterObjList()[index].GetComponent<Animation>().CrossFade("stand");
                        Debug.Log("패배");
                        GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.SetActive(false);
                        GameObject.Find(info.stageName + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(false);
                        GameObject.Find(info.stageName + "Button").transform.Find("State/Progress/Dust").gameObject.SetActive(false);
                        GameObject.Find(info.stageName + "Button").transform.Find("MercImage").gameObject.SetActive(false);
                    }
                }



            }


            yield return null;
        }
    }

    //용병 공격 주기
    IEnumerator MercenaryAttack()
    {
        while (true)
        {
            for (int i = 0; i < mercenary.Count; i++)
            {
                StageInfo info = StageData.instance.getStageInfoList().Find(y => y.getStageNum() == mercenary[i].stageNum);
                if (info != null && info.state)
                {
                    int index = StageData.instance.getStageInfoList().FindIndex(x => x.getStageNum() == mercenary[i].stageNum);

                    //공격 주기 시간 체크
                    mercenary[i].AttackTime += Time.deltaTime;
                    if (mercenary[i].AttackSpeed < mercenary[i].AttackTime)
                    {
                        mercenary[i].AttackTime = 0;
                        stageManager.getMonsterObjList()[index].GetComponent<Animation>().Play("damage");
                        info.monsterHP -= mercenary[i].stat.dps;
                        Debug.Log(info.monsterHP);
                    }
                    if (!stageManager.getMonsterObjList()[index].GetComponent<Animation>().IsPlaying("attack")
                        && !stageManager.getMonsterObjList()[index].GetComponent<Animation>().IsPlaying("damage"))
                        stageManager.getMonsterObjList()[index].GetComponent<Animation>().CrossFade("stand");

                    //몬스터 토벌
                    if (info.monsterHP < 0)
                    {
                        info.state = false;
                        info.regen = true;
                        info.complete = true;
                        info.mermove = true;
                        mercenary[i].active = "back";
                        
                        stageManager.getMonsterObjList()[index].GetComponent<Animation>().CrossFade("die");
                        Debug.Log("승리");
                        GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.SetActive(false);
                        GameObject.Find(info.stageName + "Button").transform.Find("State/Progress/sword").gameObject.SetActive(false);
                        GameObject.Find(info.stageName + "Button").transform.Find("State/Progress/Dust").gameObject.SetActive(false);
                        GameObject.Find(info.stageName + "Button").transform.Find("MercImage").gameObject.SetActive(false);

                        for (int k = 0; i < MercenaryData.instance.getMercenary().Count; k++)
                        {
                            if (mercenaryObj.transform.GetChild(k).Find("NameText").gameObject.GetComponent<Text>().text
                                == mercenary[i].getName())
                            {
                                merObj = mercenaryObj.transform.GetChild(k).gameObject;
                                break;
                            }
                        }
                        info.time = System.DateTime.Now;
                        //걸리는 시간. 거리 계산
                        float dist = Vector3.Distance(merObj.transform.localPosition, GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Stage").gameObject.transform.localPosition);
                        int time = (int)(dist / 10);
                        info.leadTime = new System.TimeSpan(0, time / 60, time % 60);
                        mercenary[i].posX = merObj.transform.localPosition.x;
                        mercenary[i].posY = merObj.transform.localPosition.y;
                        Debug.Log(info.leadTime);

                        destroyMonster(stageManager.getMonsterObjList()[index]);
                    }
                }



            }




            yield return null;

            
            
        }
    }


    IEnumerator destroyMonster(GameObject monObj)
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("삭제");
        Destroy(monObj);
    }



}
