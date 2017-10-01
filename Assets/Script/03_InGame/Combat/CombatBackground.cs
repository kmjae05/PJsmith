using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBackground : MonoBehaviour {

    //public float scrollSpeed = 1.5f;
    Material myMaterial;

    private float newOffsetX;

    //조이스틱 스크립트
    public Joystick2D joystick;

    //캐릭터와 충돌했을 때 앞으로 움직이지 않게.
    public GameObject joystickImage;
    //캐릭터와 몬스터 충돌 flag
    public PlayerController player;


    //void Start()
    //{
    //    myMaterial = GetComponent<Renderer>().material;
    //}

    //void Update()
    //{
    //    //충돌하고 오른쪽으로 이동할 경우
    //    if (player.getCollMonster() && joystickImage.transform.position.x >= -12.19)
    //    { }
    //    else
    //    {
    //        Vector2 direction = Vector2.zero;
    //        direction.x = joystick.Horizontal();
    //        //float newOffsetX = myMaterial.mainTextureOffset.x + scrollSpeed * Time.deltaTime;
    //        //조이스틱이 움직이는 정도에 따라 속도 조절. 배경 반복.
    //        newOffsetX = myMaterial.mainTextureOffset.x + direction.x * Time.deltaTime * 0.2f;
    //        Vector2 newOffset = new Vector2(newOffsetX, 0);
    //        myMaterial.mainTextureOffset = newOffset;
    //    }
    //}

    //public float getNewOffsetX()
    //{
    //    return newOffsetX;
    //}

}
