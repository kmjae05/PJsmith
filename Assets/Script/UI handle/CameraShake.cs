 using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;
    
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.1f; //강도
    public float decreaseFactor = 1.0f; //흔들리는 속도
    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }
    void Start()
    {
        originalPos = camTransform.localPosition;
    }

    public void EnableShake(float time)
    {
        StartCoroutine(ActiveShake(time));
    }
    IEnumerator ActiveShake(float time)
    {
        while (time > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            time -= Time.deltaTime * decreaseFactor;
            yield return null;
        }
        camTransform.localPosition = originalPos;
    }
}