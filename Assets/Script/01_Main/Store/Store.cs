using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour
{
    public GameObject HammerInventory;

    public GameObject Chr_001;
    public GameObject LackGold;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void HammerClaw(int money){

        if (Player.Play.gold < money) //골드가 부족할 경우
        {
            LackGold.SetActive(true);
            return;
        }
        else
        {
            //Chr_001.GetComponent<Player>().OrePrice(money);
            HammerInventory.GetComponent<HammerInventory>().Button();
            
           // StartCoroutine(Chr_001.GetComponent<PrintGoldConsume>().goldAnimation(gold));
        }

    }
}
