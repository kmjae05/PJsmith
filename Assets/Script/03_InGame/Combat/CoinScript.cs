using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {

    //플레이어
    private GameObject player;
    //속도 반비례
    public float m = 1.0f;

    Vector3 velocity = Vector3.zero;

    void Start() {
        player = GameObject.Find("Chr");
        StartCoroutine(coin());
    }


    IEnumerator coin()
    {
        while (true)
        {
            Vector3 force = player.transform.position - transform.position + new Vector3(0f, 0f, 0f);
            float r = force.magnitude;

            force.Normalize();
            force *= 1000; // 적당한 힘(자력의 세기) 
            force /= r * r;  // 거리 제곱에 반비례 

            Vector3 acceleration = force / m; // m은 코인의 질량(적당한값) 

            velocity += acceleration * Time.deltaTime; // 속도 적분 
            transform.position += velocity * Time.deltaTime; // 거리 적분 


            yield return null;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //item pick 효과


            Destroy(gameObject);
        }
    }
}
