using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ToggleSetupScript : MonoBehaviour {

    public Toggle Vibration;
    static public bool VibrationState;

    public Toggle Haptip;
    static public bool HaptipState;

    public Toggle PushAlarm;
    static public bool PushAlarmState;

	// Use this for initialization
	void Start () {
	
	}

    public void VibrationToggle(bool Value)
    {
        if (Vibration.GetComponent<Toggle>().isOn)
        {
            VibrationState = true;
        }
        else
        {
            VibrationState = false;
        }
    }

    public void HaptipToggle(bool Value)
    {
        if (Haptip.GetComponent<Toggle>().isOn)
        {
            HaptipState = true;
        }
        else
        {
            HaptipState = false;
        }
    }

    public void PushAlarmToggle(bool Value)
    {
        if (PushAlarm.GetComponent<Toggle>().isOn)
        {
            PushAlarmState = true;
        }
        else
        {
            PushAlarmState = false;
        }
        StartCoroutine(PushNotificationHandle(PushAlarmState));
    }

    IEnumerator PushNotificationHandle(bool value)
    {
        OneSignal.SetSubscription(value);
        yield return null;
    }
}   