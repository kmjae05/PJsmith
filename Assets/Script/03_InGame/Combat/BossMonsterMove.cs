using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossMonsterMove : MonoBehaviour
{

    private Combat combat;
    private PlayerController playerController;
    //스킬3 범위와 충돌 flag
    private bool skill3Flag = false;

    //체력
    private int HP = 2000;
    //체력 슬라이더
    private GameObject slider;

    //초기 위치
    private float prePosition;
    //공격1 이동 위치
    private float hit1Position;
    //공격3 이동 위치
    private float hit3Position;
    //애니메이션 flag
    private bool hit3Flag = false;
    //이펙트 포지션
    private GameObject effPos;
    private GameObject landEff;

    //사망 이펙트
    private GameObject eff;
    private CombatSkill3Collision skill3Coll;

    //사망 여부
    private bool death = false;

    //wave
    private WaveControl waveControl;
    float time = 0;


    void Awake()
    {
        eff = transform.Find("3D_ItemDrop_01").gameObject;
        combat = GameObject.Find("Chr_001_").GetComponent<Combat>();
        playerController = GameObject.Find("Chr").GetComponent<PlayerController>();
        skill3Coll = GameObject.Find("skill3Effect").GetComponent<CombatSkill3Collision>();
        waveControl = GameObject.Find("WaveControl").GetComponent<WaveControl>();
        effPos = GameObject.Find("EffectPosition");
        landEff = GameObject.Find("Effect").transform.Find("3D_EarthMagic_02").gameObject;

        //애니메이션 속도 조절
        transform.Find("Golem").GetComponent<Animation>()["hit"].speed = 0.7f;

        prePosition = 3.0f;
        hit1Position = prePosition - 2.0f;
        hit3Position = prePosition + 3.0f;
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
            else if (GameObject.Find("Monsters").GetComponent<MonsterControl>().getWaveFlag())
                wave3();
        }
    }

    void wave3()
    {
        
        if (waveControl.getNowWave() == 3)
        {
            if (transform.position.x > 3.0f)
            {
                transform.Find("Golem").GetComponent<Animation>().CrossFade("walk");
                time += Time.deltaTime * 1.0f;
                transform.position = new Vector3(Mathf.Lerp(15f, prePosition, time), transform.position.y, transform.position.z);
            }
            else
            {
                time = 0;
                prePosition = transform.position.x;
                transform.Find("Golem").GetComponent<Animation>().CrossFade("idle");
                GameObject.Find("Monsters").GetComponent<MonsterControl>().setWaveFlag(false);

                // 공격 시작. 계속 반복
                StartCoroutine(Attack());

            }
        }
    }

    //3초 랜덤공격
    IEnumerator Attack()
    {
        float attackTime = 0f;
        while (!death)
        {
            attackTime += Time.deltaTime;
            //공격
            if(attackTime > 3f)
            {
                attackTime = 0f;
                int random = Random.Range(1, 4);
                if(random == 1) StartCoroutine(Hit1());
                if(random == 2) StartCoroutine(Hit2());
                if(random == 3) StartCoroutine(Hit3());

                yield return new WaitForSeconds(3f);
            }
            yield return null;
        }
    }

    // 1 공격 hit2 범위(소)
    // 약간 앞으로 가서 공격. 끝나고 다시 원래 자리로.
    public IEnumerator Hit1()
    {
        while (!death)
        {
            //앞으로 이동
            if (transform.position.x != hit1Position)
            {
                transform.Find("Golem").GetComponent<Animation>().CrossFade("walk");
                time += Time.deltaTime * 1.0f;
                transform.position = new Vector3(Mathf.Lerp(prePosition, hit1Position, time), transform.position.y, transform.position.z);
                yield return null;
            }
            else
            {
                transform.Find("Golem").GetComponent<Animation>().Play("hit2");
                //플레이어가 범위에 있으면 피격
                if (GameObject.Find("Chr").GetComponent<PlayerController>().getRange1Flag())
                {
                    StartCoroutine(GameObject.Find("Chr").GetComponent<PlayerController>().damaged(25));
                    StartCoroutine(GameObject.Find("Chr").GetComponent<PlayerController>().PrintDamagedNormalEffect(0.3f));
                }
                //idle 상태
                StartCoroutine(setIdle());
                time = 0f;
                break;
            }
        }
        //공격 끝나고 이동.
        yield return new WaitForSeconds(0.5f);
        while (!death)
        {
            if (transform.position.x != prePosition)
            {
                transform.Find("Golem").GetComponent<Animation>().CrossFade("walk");
                time += Time.deltaTime * 1.0f;
                transform.position = new Vector3(Mathf.Lerp(hit1Position, prePosition, time), transform.position.y, transform.position.z);
                yield return null;
            }
            else
            {
                StartCoroutine(setIdle());
                time = 0f;
                break;
            }
        }
    }
    // 2 공격 hit 범위(중)
    public IEnumerator Hit2()
    {
        yield return new WaitForSeconds(0.5f);
        transform.Find("Golem").GetComponent<Animation>().Play("hit");
        if (GameObject.Find("Chr").GetComponent<PlayerController>().getRange2Flag())
        {
            StartCoroutine(GameObject.Find("Chr").GetComponent<PlayerController>().damaged(74));
            StartCoroutine(GameObject.Find("Chr").GetComponent<PlayerController>().PrintDamagedNormalEffect(0.3f));
        }
        //idle 상태
        StartCoroutine(setIdle());
        yield return null;

    }
    // 3 공격 jump-fly-land 범위 전체
    // 뒷걸음질 치면서 약간 뒤로 갔다가 점프하고 쿵. 다시 앞으로.
    public IEnumerator Hit3()
    {

        while (!death)
        {
            //뒤로 이동
            if (transform.position.x != hit3Position)
            {
                transform.Find("Golem").GetComponent<Animation>().CrossFade("walk");
                time += Time.deltaTime * 1.0f;
                transform.position = new Vector3(Mathf.Lerp(prePosition, hit3Position, time), transform.position.y, transform.position.z);
                yield return null;
            }
            else
            {
                hit3Flag = true;
                transform.Find("Golem").GetComponent<Animation>().CrossFade("jump");
                yield return new WaitForSeconds(0.3f);
                transform.Find("Golem").GetComponent<Animation>().CrossFade("fly");
                yield return new WaitForSeconds(0.5f);
                transform.Find("Golem").GetComponent<Animation>().CrossFade("land");
                yield return new WaitForSeconds(0.2f);
                hit3Flag = false;

                //이펙트
                StartCoroutine(hit3effect());
                StartCoroutine(GameObject.Find("Chr").GetComponent<PlayerController>().damaged(100));
                StartCoroutine(GameObject.Find("Chr").GetComponent<PlayerController>().PrintDamagedNormalEffect(0.1f));
                //idle 상태
                StartCoroutine(setIdle());
                time = 0f;
                break;
            }
        }
        //공격 끝나고 이동.
        yield return new WaitForSeconds(0.5f);
        while (!death)
        {
            if (transform.position.x != prePosition)
            {
                transform.Find("Golem").GetComponent<Animation>().CrossFade("walk");
                time += Time.deltaTime * 1.0f;
                transform.position = new Vector3(Mathf.Lerp(hit3Position, prePosition, time), transform.position.y, transform.position.z);
                yield return null;
            }
            else
            {
                StartCoroutine(setIdle());
                time = 0f;
                break;
            }
        }
    }

    IEnumerator hit3effect()
    {
        //카메라 흔들
        GameObject.Find("CameraShake").GetComponent<CameraShake>().EnableShake(0.5f);

        //이펙트 위치
        effPos.transform.position = GameObject.Find("BossMonster").transform.position;
        effPos.transform.Translate(-18, 0, 0);
        //이펙트
        GameObject eff = Instantiate(landEff);
        eff.transform.SetParent(effPos.transform.Find("EffPosition").transform, false); //effect.transform, false);
        eff.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        Destroy(eff);
    }


    //피격
    public void damage(int d)
    {
        if (!death)
        {
            HP -= d;
            slider.GetComponent<Slider>().value = HP;

            if (!transform.Find("Golem").GetComponent<Animation>().IsPlaying("walk")
                && !transform.Find("Golem").GetComponent<Animation>().IsPlaying("die")
                && !transform.Find("Golem").GetComponent<Animation>().IsPlaying("hit")
                && !transform.Find("Golem").GetComponent<Animation>().IsPlaying("hit2")
                && !transform.Find("Golem").GetComponent<Animation>().IsPlaying("jump")
                && !transform.Find("Golem").GetComponent<Animation>().IsPlaying("fly")
                && !transform.Find("Golem").GetComponent<Animation>().IsPlaying("land")
                && !hit3Flag)
                transform.Find("Golem").GetComponent<Animation>().Play("damage");
            StartCoroutine(setIdle());
        }
    }

    //
    IEnumerator die()
    {
        eff.SetActive(true);

            StopCoroutine(setIdle());
            StopCoroutine(Attack());
            StopCoroutine(Hit1());
            StopCoroutine(Hit2());
            StopCoroutine(Hit3());


        //몬스터 애니메이션
        transform.Find("Golem").GetComponent<Animation>().Stop();
        transform.Find("Golem").GetComponent<Animation>().Play("die");

        yield return new WaitForSeconds(1.5f);
        combat.resetMonsterInfo();              //플레이어의 몬스터 충돌 상태 초기화
        playerController.resetMonsterName();
        slider.SetActive(false);              //슬라이더 비활성화
        skill3Coll.setCollMonster(false);       //플레이어의 스킬3 범위 충돌 삭제
        GameObject.Find("Monsters").GetComponent<MonsterControl>().deleteMiniBossObj();


        gameObject.SetActive(false);

    }

    IEnumerator setIdle()
    {
        yield return new WaitForSeconds(0.3f);
        if (!transform.Find("Golem").GetComponent<Animation>().IsPlaying("die"))
            transform.Find("Golem").GetComponent<Animation>().CrossFade("idle");
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
