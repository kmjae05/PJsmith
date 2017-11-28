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

    private void Start()
    {
        WorldMapBackObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back").gameObject;
        mercenaryObj = GameObject.Find("Menu").transform.Find("WorldMap/Stage/UIPanel/Back/Mercenary").gameObject;
        mercenary = MercenaryData.instance.getMercenary();
        merObj = new GameObject();

        StartCoroutine(loop());
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


                }

                //사냥
                else if(mercenary[i].active == "hunt")
                {
                    //공격주기 최소 1초

                    //몬스터 체력
                    GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.SetActive(true);
                    GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().maxValue = 1000 * info.typeNum;
                    GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().value = info.monsterHP;
                    GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider/TimeText").gameObject.GetComponent<Text>().text
                        = (int)(GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().value / GameObject.Find(info.stageName + "Button").transform.Find("State/TimeSlider").gameObject.GetComponent<Slider>().maxValue * 100) + "%";

                    GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.SetActive(true);
                    GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.GetComponent<Slider>().maxValue = 1000;
                    GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.GetComponent<Slider>().value = mercenary[i].stat.HP;
                    GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider/TimeText").gameObject.GetComponent<Text>().text
                        = (int)(GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.GetComponent<Slider>().value / GameObject.Find(info.stageName + "Button").transform.Find("MercImage/TimeSlider").gameObject.GetComponent<Slider>().maxValue * 100) + "%";


                }

            }



            yield return null;
        }


    }




    //몬스터 공격 주기
    IEnumerator MonsterAttack(float attackSpeed)
    {
        while (true)
        {



            yield return new WaitForSeconds(attackSpeed);
        }
    }

    //용병 공격 주기
    IEnumerator MercenaryAttack(float attackSpeed)
    {
        while (true)
        {


            yield return new WaitForSeconds(attackSpeed);
            
            
        }
    }



}
