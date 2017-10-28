using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Clicker : MonoBehaviour {

    private GameObject Chr_001;
    float click_state;

    private Camera ui_camera;
    private Vector3 touchedPos; //터치 위치
    private GameObject touchEffectObj;

    void Awake()
    {
        Chr_001 = GameObject.Find("Chr_001");

    }
	// Use this for initialization
	void Start ()
    {
        click_state = 1.0f; //공격을 하지 않을 경우에 일정시간이 지난후 어택아이들로 변경

        ui_camera = GameObject.Find("UI_Camera").GetComponent<Camera>();
        touchEffectObj = GameObject.Find("Effect").transform.Find("UI_Press_01").gameObject;
    }
	// Update is called once per frame
    void Update()
    {
        if (click_state >= 0.0f)
        {
            GameObject.Find("Chr_001").GetComponent<Animator>().SetFloat("next", click_state -= Time.deltaTime); // 공격을 하지 않을 경우에 일정시간이 지난후 어택아이들로 상태를 변경
        }
        Touch tempTouchs; //터치값
        if (Input.touchCount > 0)
        {    //터치가 1개 이상이면
            //Debug.Log(Input.touchCount);
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(i) == false)
                {
                    tempTouchs = Input.GetTouch(i);
                    if (tempTouchs.phase == TouchPhase.Began)
                    {//해당 터치가 시작됐다면
                        touchedPos = ui_camera.ScreenToViewportPoint(tempTouchs.position);
                        Debug.Log(tempTouchs.position.x);
                        
                        if (click_state <= 0.0f && Chr_001.GetComponent<Animator>().GetBool("Atk_State"))
                        {
                            Debug.Log(tempTouchs.position.x);
                            GameObject touchEff = Instantiate(touchEffectObj);//, touchedPos, Quaternion.identity);
                            touchEff.transform.SetParent(GameObject.Find("Effect").transform);
                            touchEff.transform.localPosition = new Vector3(tempTouchs.position.x, tempTouchs.position.y, 0f);
                            touchEff.transform.localScale = new Vector3(1, 1, 1);
                            Debug.Log(touchEff.transform.position.z);
                            touchEff.SetActive(true);

                            click_state = 0.5f; //어택-> 어택아이들 애니메이션 트리거 
                            Chr_001.GetComponent<Animator>().SetTrigger("Click");//애니메이션 트리거 작동
                            AndroidManager.HapticFeedback();
                            break;   //한 프레임(update)에는 하나만
                        }
                    }
                }
            }
        }
    }

    //void OnMouseDown() //마우스클릭 안드로이드 빌드시 주석처리                      //?? 게임내 UI터치시 게임진행 막는 부분이 되지 않음
    //{
    //    if (EventSystem.current.IsPointerOverGameObject() == false) //UI의 경우 클릭해도 카운트되지 않음
    //    {  //UI이 위가 아니면.
    //        if (Input.GetMouseButtonDown(0) && !ReadyGo.getGo && !ReadyGo.getReady) //마우스 클릭
    //        {
    //            if (click_state <= 0.0f)
    //            {
    //                if (Chr_001.GetComponent<Animator>().GetBool("Atk_State"))
    //                {
                        
    //                    GameObject touchEff = Instantiate(touchEffectObj);
    //                    touchEff.transform.SetParent(GameObject.Find("03_Effect").transform);
    //                    touchEff.transform.position = touchedPos;
    //                    touchEff.SetActive(true);

    //                    Debug.Log("click");
    //                    click_state = 0.5f;
    //                    Chr_001.GetComponent<Animator>().SetTrigger("Click");//애니메이션 트리거 작동
    //                }
    //                else
    //                {

    //                }
    //            }
    //        }
    //    }
    //}
}


