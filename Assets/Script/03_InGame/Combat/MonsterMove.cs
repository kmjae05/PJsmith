using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MonsterMove : MonoBehaviour
{

    //배경 오프셋 속도에 따라 움직임.
    //이동 거리(newOffsetX)를 가져올 배경 스크립트
    //public CombatBackground background;

    private PlayerController playerController;

    //캐릭터와 몬스터 충돌 flag
    private Combat combat;
    //스킬3 범위와 충돌 flag
    private bool skill3Flag = false;

    //체력
    private int HP = 500;
    //체력 슬라이더
    private GameObject slider;

    char[] name = null;

    //초기 위치
    private float prePosition;
    private bool flag = false;
    private float returnTime = 0f;
    private Transform nowPos;
    float moveTime = 0f;

    //사망 이펙트
    private GameObject eff;
    private CombatSkill3Collision skill3Coll;
    //사망 여부
    private bool death = false;

    float time = 0;
    //wave
    private WaveControl waveControl;

    void Awake()
    {
        name = transform.name.ToCharArray();
        eff = transform.Find("3D_ItemDrop_01").gameObject;
        combat = GameObject.Find("Chr_001_").GetComponent<Combat>();
        playerController = GameObject.Find("Chr").GetComponent<PlayerController>();
        skill3Coll = GameObject.Find("skill3Effect").GetComponent<CombatSkill3Collision>();
        waveControl = GameObject.Find("WaveControl").GetComponent<WaveControl>();


        if (Convert.ToInt32(name[name.Length - 1].ToString()) == 0) prePosition = 3.0f;
        if (Convert.ToInt32(name[name.Length - 1].ToString()) == 1) prePosition = 2.0f;
        if (Convert.ToInt32(name[name.Length - 1].ToString()) == 2) prePosition = 5.5f;


    }

    void Update()
    {
        if (!death)
        {
            //몬스터 사망
            if (HP <= 0)
            {
                death = true;
                skill3Flag = false;

                StartCoroutine(die());
            }
            else
            {
                if (flag)
                {
                    returnTime += Time.deltaTime;
                    if (returnTime <= 3f)
                    {
                        nowPos = transform;
                    }
                    if (returnTime > 4.0f)
                    {
                        returnTime = 0f;
                        transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", false);
                    }
                    if (returnTime > 3.5f)
                    {
                        returnPosition();
                    }
                }
            }
        }

        if (GameObject.Find("Monsters").GetComponent<MonsterControl>().getWaveFlag())
        {
            //Wave 1/3
            wave1();

            //Wave 2/3
            wave2();
        }

    }
    //사망.
    IEnumerator die()
    {
        eff.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //float fadeRate = 0.02f;
        //float fade = 1.5f;

        //몬스터 애니메이션
        transform.Find("Skeleton").GetComponent<Animator>().Play("Death");
        ////스프라이트 알파값 감소
        //while (fade > 0)
        //{
        //    fade -= fadeRate;
        //    yield return null;
        //}

        yield return new WaitForSeconds(1.5f);
        combat.resetMonsterInfo();              //플레이어의 몬스터 충돌 상태 초기화
        playerController.resetMonsterName();
        slider.SetActive(false);                //슬라이더 비활성화
        skill3Coll.setCollMonster(false);       //플레이어의 스킬3 범위 충돌 삭제
        GameObject.Find("Monsters").GetComponent<MonsterControl>().deleteMiniObj(Convert.ToInt32(name[name.Length - 1].ToString()));


        gameObject.SetActive(false);

    }

    //
    void wave1()
    {
        if (waveControl.getNowWave() == 1)
        {
            //monster0만
            if (Convert.ToInt32(name[name.Length - 1].ToString()) == 0)
            {
                if (transform.position.x > 3)
                {
                    transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", true);
                    time += Time.deltaTime * 1.0f;
                    transform.position = new Vector3(Mathf.Lerp(15, 3, time), transform.position.y, transform.position.z);
                    flag = true;
                }
                else
                {
                    transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", false);
                    GameObject.Find("Monsters").GetComponent<MonsterControl>().setWaveFlag(false);
                    //StartCoroutine(returnPos());
                }
            }
        }
    }
    //
    void wave2()
    {
        if (waveControl.getNowWave() == 2)
        {
            //monster1
            if (Convert.ToInt32(name[name.Length - 1].ToString()) == 1)
            {
                if (transform.position.x > 2)
                {
                    transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", true);
                    time += Time.deltaTime * 1.0f;
                    transform.position = new Vector3(Mathf.Lerp(15, 2, time), transform.position.y, transform.position.z);
                }
                else
                {
                    transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", false);
                    GameObject.Find("Monsters").GetComponent<MonsterControl>().setWaveFlag(false);
                    //StartCoroutine(returnPos());
                    flag = true;
                }
            }
            //monster2
            if (Convert.ToInt32(name[name.Length - 1].ToString()) == 2)
            {
                if (transform.position.x > 5.5f)
                {
                    transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", true);
                    time += Time.deltaTime * 1.0f;
                    transform.position = new Vector3(Mathf.Lerp(15, 5.5f, time), transform.position.y, transform.position.z);
                }
                else
                {
                    transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", false);
                    flag = true;

                }
            }
        }

    }

    //밀렸을 때 원래 위치로 이동
    IEnumerator returnPos()
    {

        while (true)
        {
            if (!flag)
            {
                yield return new WaitForSeconds(2.0f);

                if (transform.position.x > prePosition)
                {
                    flag = true;
                    yield return new WaitForSeconds(2.0f);
                    flag = false;
                }
            }
            yield return null;
        }

    }

    void returnPosition()
    {

        if (transform.position.x > prePosition)
        {
            transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", true);
            moveTime += Time.deltaTime * 0.5f;
            transform.Translate(-0.1f, 0, 0);
            //transform.position = new Vector3(Mathf.Lerp(transform.position.x, nowPos.position.x-1f, moveTime), transform.position.y, transform.position.z);
        }
        else
        {
            moveTime = 0f;
            transform.Find("Skeleton").GetComponent<Animator>().SetBool("Run", false);
        }

    }



    //collider
    void OnTriggerEnter(Collider other)
    {
        //스킬3 범위
        if (other.transform.tag == "skill3")
        {
            skill3Flag = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "skill3")
        {
            skill3Flag = false;
        }
    }


    public void Attack()
    {
        //공격
        transform.Find("Skeleton").GetComponent<Animator>().SetTrigger("Attack");
        
        //transform.FindChild("Skeleton").GetComponent<Animator>().Play("Attack");

    }

    public void damage(int d)
    {
        HP -= d;
        slider.GetComponent<Slider>().value = HP;
        //피격
        if (!death)
            transform.Find("Skeleton").GetComponent<Animator>().Play("Damage");
    }
    public float getHP()
    {
        return HP;
    }
    public bool getSkill3Flag()
    {
        return skill3Flag;
    }
    public bool getDeath()
    {
        return death;
    }
    public void setSlider(GameObject sl)
    {
        slider = sl;
    }

}
