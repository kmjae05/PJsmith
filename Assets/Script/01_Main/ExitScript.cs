using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour {

    public GameObject ExitPopup;
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(ExitPopup.active)
                ExitPopup.SetActive(false);
            else
                ExitPopup.SetActive(true);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
