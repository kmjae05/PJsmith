using UnityEngine;
using UnityEngine.UI;

public class PinchZoom : MonoBehaviour
{
    public float perspectiveZoomSpeed = 0.1f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.000000000000001f;        // The rate of change of the orthographic size in orthographic mode.

    private GameObject worldMap;
    private RectTransform uiPanel;
    private GameObject monster;
    

    private void Start()
    {
        worldMap = GameObject.Find("Menu").transform.Find("WorldMap").gameObject;
        uiPanel = worldMap.transform.Find("Stage/UIPanel").gameObject.GetComponent<RectTransform>();
        monster = GameObject.Find("Monster");
    }


    void Update()
    {
        worldMap.transform.Find("Stage/UIPanel").gameObject.GetComponent<ScrollRect>().enabled = true;
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            worldMap.transform.Find("Stage/UIPanel").gameObject.GetComponent<ScrollRect>().enabled = false;
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            // ... change the orthographic size based on the change in distance between the touches.
            //GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

            // Make sure the orthographic size never drops below zero.
            //GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize, 0.1f);
            Debug.Log(deltaMagnitudeDiff);
            //worldMapCanvas.scaleFactor -= deltaMagnitudeDiff * orthoZoomSpeed;
            //worldMapCanvas.scaleFactor = Mathf.Clamp(worldMapCanvas.scaleFactor, 0.1f, 0.4f);
            //Debug.Log(deltaMagnitudeDiff * orthoZoomSpeed);
            if (deltaMagnitudeDiff > 2)
            {
                uiPanel.localScale = new Vector2(
                    uiPanel.localScale.x - 0.02f, uiPanel.localScale.y - 0.02f);
                uiPanel.localScale = new Vector2(
                    Mathf.Clamp(uiPanel.localScale.x, 0.4f, 1f), Mathf.Clamp(uiPanel.localScale.y, 0.4f, 1f));

                //몬스터 축소
                if (monster.transform.childCount > 2)
                {
                    for (int i = 5; i < monster.transform.childCount; i++)
                    {
                        
                        if (monster.transform.GetChild(i).gameObject.name == "Syaonil(Clone)")
                        {
                            monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(
                            monster.transform.GetChild(i).gameObject.transform.localScale.x - 0.05f,
                            monster.transform.GetChild(i).gameObject.transform.localScale.y - 0.05f,
                            monster.transform.GetChild(i).gameObject.transform.localScale.z - 0.05f);

                            monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.x, 1f, 2.5f),
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.y, 1f, 2.5f),
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.z, 1f, 2.5f));
                        }
                        else
                        {
                            monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(
                            monster.transform.GetChild(i).gameObject.transform.localScale.x - 0.066f,
                            monster.transform.GetChild(i).gameObject.transform.localScale.y - 0.066f,
                            monster.transform.GetChild(i).gameObject.transform.localScale.z - 0.066f);

                            monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.x, 2f, 4f),
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.y, 2f, 4f),
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.z, 2f, 4f));
                        }
                    }
                }
            }
            if (deltaMagnitudeDiff < -2)
            {
                uiPanel.localScale = new Vector2(
                    uiPanel.localScale.x + 0.02f, uiPanel.localScale.y + 0.02f);
                uiPanel.localScale = new Vector2(
                    Mathf.Clamp(uiPanel.localScale.x, 0.4f, 1f), Mathf.Clamp(uiPanel.localScale.y, 0.4f, 1f));

                //몬스터 축소
                if (monster.transform.childCount > 2)
                {
                    for (int i = 5; i < monster.transform.childCount; i++)
                    {

                        if (monster.transform.GetChild(i).gameObject.name == "Syaonil(Clone)")
                        {
                            monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(
                            monster.transform.GetChild(i).gameObject.transform.localScale.x + 0.05f,
                            monster.transform.GetChild(i).gameObject.transform.localScale.y + 0.05f,
                            monster.transform.GetChild(i).gameObject.transform.localScale.z + 0.05f);

                            monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.x, 1f, 2.5f),
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.y, 1f, 2.5f),
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.z, 1f, 2.5f));
                        }
                        else
                        {
                            monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(
                            monster.transform.GetChild(i).gameObject.transform.localScale.x + 0.066f,
                            monster.transform.GetChild(i).gameObject.transform.localScale.y + 0.066f,
                            monster.transform.GetChild(i).gameObject.transform.localScale.z + 0.066f);

                            monster.transform.GetChild(i).gameObject.transform.localScale = new Vector3(
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.x, 2f, 4f),
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.y, 2f, 4f),
                                Mathf.Clamp(monster.transform.GetChild(i).gameObject.transform.localScale.z, 2f, 4f));
                        }
                    }
                }
            }
            uiPanel.localPosition = new Vector3(0, 0, 0);
        }
    }
}