using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrazierManager : MonoBehaviour {


    private Camera worldCam;
    private Camera uiCam;
    private Transform brazier;  //화로
    private GameObject brazierButtonPos;    //버튼 위치

    private GameObject miniButton;   //미니 버튼 5개




    private void Start()
    {
        worldCam = GameObject.Find("Main_Camera").GetComponent<Camera>();
        uiCam = GameObject.Find("UI_Camera").GetComponent<Camera>();
        brazier = GameObject.Find("brazierImg").transform;
        brazierButtonPos = GameObject.Find("BrazierButton");

        miniButton = brazierButtonPos.transform.Find("BrazierMiniButton").gameObject;
        miniButton.SetActive(false);


        SetPositionHUD();

    }

    //private void Update()
    //{
    //    SetPositionHUD();
    //}

    //화로 메뉴 위치 설정
    void SetPositionHUD()
    {
        //playerposition을 게임카메라의 viewPort 좌표로 변경. 
        Vector3 position = worldCam.WorldToViewportPoint(brazier.position);
        position.x -= 0.01f;
        position.y += 0.05f;
        //해당 좌표를 uiCamera의 World좌표로 변경. 
        brazierButtonPos.transform.position = uiCam.ViewportToWorldPoint(position);
        //값 정리. 
        position = brazierButtonPos.transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = 0.0f;
        brazierButtonPos.transform.localPosition = position;

    }

}
