using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChrFaceScript : MonoBehaviour {

    private Camera worldCam;
    private Camera uiCam;

    private GameObject profilePos;
    private GameObject player;

    private void Awake()
    {
        worldCam = transform.GetComponent<Camera>();
        uiCam = GameObject.Find("UI_Camera").GetComponent<Camera>();

        profilePos = GameObject.Find("ProfileBox").transform.Find("ProfileImage").gameObject;
        player = GameObject.Find("Chr");

    }
    private void Update()
    {
        SetPositionHUD();
    }

    //UI에 맞게 위치 고정
    void SetPositionHUD()
    {

        Vector3 position = uiCam.ViewportToWorldPoint(profilePos.transform.position);
        position.x += 1000.0f;
        position.y += 200.0f;
        transform.position = worldCam.WorldToViewportPoint(position);

        //값 정리. 
        position = transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        
        position.z = -108.62f;
        transform.localPosition = position;


    }


}
