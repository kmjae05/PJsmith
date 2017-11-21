using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FestivalData : MonoBehaviour {

    public static FestivalData instance = null;

    private static List<ForSale> saleList;

    bool salecheck = false;
    float tmpTime = 0;
    bool tmpFlag = false;
    float timecheck = 0;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        saleList = new List<ForSale>();
        for(int i = 0; i < 8; i++)
        {
            saleList.Add(new ForSale());
        }

        StartCoroutine(buyAI());
    }


    //코루틴 아이템 팔림.
    IEnumerator buyAI()
    {
        while (true)
        {
            timecheck += Time.deltaTime;

            for (int i = 0; i < 8; i++)
            {
                if (saleList[i].state == "sale")
                {
                    salecheck = true;
                }
            }
            if (salecheck) salecheck = false;
            else timecheck = 0;

            for (int i = 0; i < 8; i++)
            {
                if(saleList[i].state == "sale")
                {
                    if (!tmpFlag)
                    {
                        tmpFlag = true;
                        tmpTime = Random.Range(10, 20);
                        timecheck = 0;
                    }

                    if (timecheck > tmpTime)
                    {
                        saleList[i].state = "sellout";
                        Debug.Log("sell");
                        tmpFlag = false;



                    }
                }


            }
                    yield return null;
        }
    }




    public List<ForSale> getSaleList() { return saleList; }
}




public class ForSale
{
    public InventoryThings saleThings;
    public string state;            // empty, sale, sellout
    public bool alrFlag;

    public int possession;      //개수
    public int unitPrice;       //개당 가격

    public ForSale()
    {
        saleThings = new InventoryThings();
        state = "empty";
    }
    public ForSale(InventoryThings inven)
    {
        saleThings = inven;
        state = "empty";
    }




}