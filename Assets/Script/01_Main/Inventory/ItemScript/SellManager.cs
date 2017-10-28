using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellManager : MonoBehaviour {

    private GameObject sellPopup;
    private GameObject UIPanel;
    private GameObject itemBox;
    private Text priceText;
    private GameObject selectBox;
    private Text selectAmountText;
    private Button minButton;
    private Button pluButton;
    private Slider slider;
    private Text totalText;
    private Button allButton;
    private Button okButton;

    void Start ()
    {
        sellPopup = GameObject.Find("System").transform.Find("SellPopup").gameobject;
        UIPanel = sellPopup.transform.Find("UIPanel").gameobject;



    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
