using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageItem : MonoBehaviour {

    //인벤토리
    private GameObject inven;

    //속도 반비례
    float m = 0.5f;

    Vector3 velocity = Vector3.zero;

    void Start()
    {
        inven = GameObject.Find("InvenButton");
        StartCoroutine(coin());
    }


    IEnumerator coin()
    {
        while (true)
        {
            Vector3 force = inven.transform.position - transform.position + new Vector3(0f, 0f, 0f);
            float r = force.magnitude;

            force.Normalize();
            force *= 1000; // 적당한 힘(자력의 세기) 
            force /= r;  // 거리에 반비례 

            Vector3 acceleration = force / m; // m은 코인의 질량(적당한값) 

            velocity += acceleration * Time.deltaTime; // 속도 적분 
            transform.position += velocity * Time.deltaTime; // 거리 적분 

            //destroy & eff
            if (transform.position.x > inven.transform.position.x && transform.position.y < inven.transform.position.y)
            {
                GameObject eff = Instantiate(GameObject.Find("Dice").transform.Find("DiceRewardEff").gameObject);
                eff.transform.SetParent(GameObject.Find("Dice").transform, false);
                eff.SetActive(true);

                Destroy(gameObject);
            }
            yield return null;
        }
    }
}
