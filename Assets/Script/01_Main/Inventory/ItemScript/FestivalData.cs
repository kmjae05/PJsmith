using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FestivalData : MonoBehaviour {

    public static FestivalData instance = null;

    private static List<ForSale> saleList;


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


    }


    //코루틴 아이템 팔림.




    public List<ForSale> getSaleList() { return saleList; }
}




public class ForSale
{
    public InventoryThings saleThings;
    public string state;            // empty, sale, sellout

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