using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour {

    private Text goldText;
    private Text cashText;


    private void Start()
    {
        goldText = GameObject.Find("GoldText").GetComponent<Text>();
        cashText = GameObject.Find("CashText").GetComponent<Text>();

        goldText.text = Player.Play.gold.ToString();
        cashText.text = Player.Play.cash.ToString();
    }


}
