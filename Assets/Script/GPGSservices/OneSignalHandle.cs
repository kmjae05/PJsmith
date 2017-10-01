using UnityEngine;
using System.Collections;
using System.Collections.Generic;  

public class OneSignalHandle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        OneSignal.StartInit("50ccf00d-da6b-423a-9df4-a16e5c4e7c87")
       .HandleNotificationOpened(HandleNotificationOpened)
       .EndInit();
	}
	
	// Update is called once per frame
    private static void HandleNotificationOpened(OSNotificationOpenedResult result)
    {
    }
}
