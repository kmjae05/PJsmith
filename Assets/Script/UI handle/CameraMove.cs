using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{

    public Transform mCamera;
    public GameObject Moru;
    public GameObject MoruShadow;
    public GameObject Iron;
    public float cameraPosition_x;
    public float cameraPosition_y;
    public float cameraSize;

    public float d_cameraPosition_x;
    public float d_cameraPosition_y;
    public float d_cameraSize;

    public float v_cameraPosition_x;
    public float v_cameraPosition_y;
    public float v_cameraSize;

    public bool clickInv = false;

    public IEnumerator Move(float start, float destination, float velocity)             //이동 함수, s=시작지점, d=끝지점, v=속도
    {
        while (start < destination)
        {
            start += velocity;
            mCamera.transform.position = new Vector3(start, 82.9f, -180.0f);

            yield return null;
        }
        start = destination;
        mCamera.transform.position = new Vector3(start, 82.9f, -180.0f);
    }
    public IEnumerator Ex_Move(float start, float destination, float velocity, int id)   //확대 이동 함수, s=시작지점, d=끝지점, v=속도, id=x/y 구분자
    {
        float move_sum = 0.0f;
        while (true)
        {
            start += velocity;
            move_sum += velocity;
            switch (id)
            {
                case 0:
                    mCamera.transform.position = new Vector3(start, mCamera.transform.position.y, -180.0f);
                    break;
                case 1:
                    mCamera.transform.position = new Vector3(mCamera.transform.position.x, start, -180.0f);
                    break;
            }
            yield return null;

            if (start >= destination)
            {
                start = destination;
                switch (id)
                {
                    case 0:
                        mCamera.transform.position = new Vector3(start, mCamera.transform.position.y, -180.0f);
                        break;
                    case 1:
                        mCamera.transform.position = new Vector3(mCamera.transform.position.x, start, -180.0f);
                        break;
                }
                yield break;
            }
        }
    }
    public IEnumerator SizeUp(float start, float destination, float velocity)            //확대 함수, s=시작지점, d=끝지점, v=속도, id=x/y 구분자 
    {
        while (start>destination)
        {
            start -= velocity;
            mCamera.GetComponent<Camera>().orthographicSize = start;
            yield return null;
        }
        start = destination;
        mCamera.GetComponent<Camera>().orthographicSize = start;
    }

    public void Click_Inventory()
    {
        StartCoroutine(Move(cameraPosition_x, d_cameraPosition_x, v_cameraPosition_x));
        /*
        StartCoroutine(Ex_Move(cameraPosition_x, d_cameraPosition_x, v_cameraPosition_x, 0));
        StartCoroutine(Ex_Move(cameraPosition_y, d_cameraPosition_y, v_cameraPosition_y, 1));
        StartCoroutine(SizeUp(cameraSize, d_cameraSize, v_cameraSize));
         * */
    }

    public void Ex_Click_Inventory()
    {
        Moru.SetActive(false);
        MoruShadow.SetActive(false);
        Iron.GetComponent<MeshRenderer>().enabled = false;

        cameraPosition_x = 4.8f;
        cameraPosition_y = 84.35f;
        cameraSize = 5.4f;
        mCamera.transform.position = new Vector3(cameraPosition_x, cameraPosition_y, -180.0f);
        mCamera.GetComponent<Camera>().orthographicSize = cameraSize;
    }
    public void Exit_Inventory()
    {
        Moru.SetActive(true);
        MoruShadow.SetActive(true); 
        Iron.GetComponent<MeshRenderer>().enabled = true;

        cameraPosition_x = 0.3f;
        cameraPosition_y = 82.9f;
        cameraSize = 6.7f;
        mCamera.transform.position = new Vector3(cameraPosition_x, cameraPosition_y, -180.0f);
        mCamera.GetComponent<Camera>().orthographicSize = cameraSize;
    }
}

