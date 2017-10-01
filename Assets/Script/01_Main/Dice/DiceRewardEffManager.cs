using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRewardEffManager : MonoBehaviour {

    private GameObject worldCamObj;
    private Camera worldCam;

    private GameObject uiCamObj;
    private Camera uiCam;



    private void Start()
    {
        worldCamObj = GameObject.Find("00_Camera").transform.Find("Dice_Camera").gameObject;
        worldCam = worldCamObj.GetComponent<Camera>();
        uiCamObj = GameObject.Find("UI_Camera");
        uiCam = uiCamObj.GetComponent<Camera>();



        //위치 조절
        SetPositionHUD();

        //destroy
        StartCoroutine(destroy());
    }


    //UI에 맞게 위치 고정
    void SetPositionHUD()
    {
        Vector3 position = uiCam.ViewportToWorldPoint(GameObject.Find("InvenButton").transform.position);
        transform.position = worldCam.WorldToViewportPoint(position);
        transform.Translate(-0.5f, 0, 0);
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }


}
