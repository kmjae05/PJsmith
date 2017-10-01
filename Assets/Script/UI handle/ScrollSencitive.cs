using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ScrollSencitive : MonoBehaviour {
    private const float inchToCm = 2.54f;

    [SerializeField]
    private EventSystem eventSystem = null;
    private float dragThresholdCM = 0.2f;

    private void SetDragThreshold()
    {
        if (eventSystem != null)
        {
            eventSystem.pixelDragThreshold = (int)(dragThresholdCM * Screen.dpi / inchToCm);
        }
    }

    void Awake()
    {
        SetDragThreshold();
    }
}
