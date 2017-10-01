using UnityEngine;
using System.Collections;

public class CameraEffect : MonoBehaviour {
    private Camera mainCamera;
    private CameraShake cameraShake;
    void Awake()
    {
        mainCamera = GameObject.Find("Main_Camera").GetComponent<Camera>();
        cameraShake = GameObject.Find("CameraShake").GetComponent<CameraShake>();
    }

    void Performance()
    {
        StartCoroutine(Zoom());
    }
    IEnumerator Zoom()
    {
        while(true)
        {
            mainCamera.orthographicSize -= 0.002f;
            yield return null;
        }
    }
    void StopPerform()
    {
        StopAllCoroutines();
        mainCamera.orthographicSize = 6;
        cameraShake.EnableShake(0.3f);
    }
}
