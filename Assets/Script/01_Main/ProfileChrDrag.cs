using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileChrDrag : MonoBehaviour
{
    private GameObject worldCamObj;
    private Vector3 worldCamPos;
    private Camera worldCam;

    private GameObject uiCamObj;
    private Camera uiCam;

    //position
    public GameObject chrStand;
    private GameObject light;

    private Vector3 trans;
    private bool flag = false;

    void Start()
    {
        worldCamObj = GameObject.Find("00_Camera").transform.Find("Profile_Camera").gameObject;
        worldCamPos = worldCamObj.transform.position;
        worldCam = worldCamObj.GetComponent<Camera>();

        uiCamObj = GameObject.Find("UI_Camera (1)");
        uiCam = uiCamObj.GetComponent<Camera>();

        light = GameObject.Find("ProfileLight");

        trans = new Vector3();
        trans = gameObject.transform.position;
    }

    private void Update()
    {
        if(flag)
            SetPositionHUD();
    }

    void OnMouseDrag()
    {
        float temp_y_axis = Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
        transform.Rotate(0, -temp_y_axis, 0, Space.World);
    }

    public void clickProfileBtt()
    {
        worldCam.depth = 10;
        flag = true;
        worldCamObj.transform.position = uiCamObj.transform.position;
    }

    public void resetRotation()
    {
        flag = false;
        worldCam.depth = -10;
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(0, -149f, 0);
        transform.rotation = rotation;
        transform.position = trans;
        worldCamObj.transform.position = worldCamPos;
    }

    //UI에 맞게 위치 고정
    void SetPositionHUD()
    {
        if (chrStand)
        {
            Vector3 position = uiCam.ViewportToWorldPoint(chrStand.transform.position);

            transform.position = worldCam.WorldToViewportPoint(position);
            light.transform.position = worldCam.WorldToViewportPoint(position);

            //값 정리. 
            position = transform.localPosition;
            position.x = Mathf.RoundToInt(position.x);
            position.y = Mathf.RoundToInt(position.y);
            position.z = -100.0f;
            transform.localPosition = position;
            position.z = -115.0f;
            light.transform.localPosition = position;
        }
    }



}
