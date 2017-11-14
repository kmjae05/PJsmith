using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour {

    private Text goldText;
    private Text cashText;


    private void Start()
    {
        goldText = GameObject.Find("System").transform.Find("Shop/UIBox/Back/StoreBox/MoneyPanel/Panel/Gold/GoldText").gameObject.GetComponent<Text>();
        cashText = GameObject.Find("System").transform.Find("Shop/UIBox/Back/StoreBox/MoneyPanel/Panel/Cash/CashText").gameObject.GetComponent<Text>();


        StartCoroutine(loop());

    }

    IEnumerator loop()
    {

        while (true)
        {
            goldText.text = Player.instance.getUser().gold.ToString();
            cashText.text = Player.instance.getUser().cash.ToString();

            yield return null;
        }
    }

}
