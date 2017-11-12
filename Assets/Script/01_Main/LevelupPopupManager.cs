using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelupPopupManager : MonoBehaviour {

    private GameObject lightImage;
    float speed = 0.5f;


	void Start ()
    {
        lightImage = GameObject.Find("System").transform.Find("LevelupPopup/UIPanel/LightImage").gameObject;
        
	}
	
	void Update ()
    {
        lightImage.transform.Rotate(new Vector3(0, 0, 1), 1*speed);
	}

    public void appear()
    {
        GameObject.Find("System").transform.Find("LevelupPopup").gameObject.SetActive(true);
        StartCoroutine(disappear());
    }


    IEnumerator disappear()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("System").transform.Find("LevelupPopup").gameObject.SetActive(false);
    }


}
