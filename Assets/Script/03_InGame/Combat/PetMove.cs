using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMove : MonoBehaviour {

    private Transform target; // 따라갈 타겟의 트랜스 폼

    private float relativeHeigth = 1.0f; // 높이 즉 y값
    private float xDistance = 1.0f; // x값
    public float dampSpeed = 2;  // 따라가는 속도 짧으면 타겟과 같이 움직인다.


    void Start()
    {
        target = GameObject.Find("Chr").transform;

    }

    void Update()
    {
        Vector3 newPos = target.position + new Vector3(xDistance, relativeHeigth, 0); // 일정의 거리를 구하는 방법
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * dampSpeed);
    }

}



