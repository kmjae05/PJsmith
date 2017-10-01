using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class PlayerController : MonoBehaviour
{

    //public Rigidbody rb { get; set; }

    //캐릭터
    private GameObject chr;

    //플레이어 이동 방향
    public Vector2 moveVector { get; set; }
    //플레이어 이동 속도
    public float moveSpeed = 0.15f;

    //체력
    private int HP = 2000;
    //체력 슬라이더
    private GameObject slider;

    //몬스터 충돌 여부
    private bool collMonster = false;
    //스킬3 범위와 몬스터 충돌 flag
    public CombatSkill3Collision skill3collision;
    //충돌한 몬스터 이름
    private string monsterName;
    private DamageImageScript damageImage;
    private GameObject ObjCanvas;
    private GameObject Damaged1, Damaged2;
    //대미지 쿨타임 flag
    private bool damagedFlag = false;
    //이펙트 포지션
    private GameObject effPos;
    private GameObject effect;
    private GameObject normalDamagedFx;
    //보스 공격 범위 체크
    private bool range1Flag = false;
    private bool range2Flag = false;


    //애니메이터
    private Animator anim;

    //등장 이펙트
    private GameObject appearanceEff;
    private bool appearanceFlag = false;

    //조이스틱 스크립트
    public Joystick2D joystick;

    private float time = 0;
    private float prePositionX = 0;
    private bool resetPositionFlag = false;

    ////메인 카메라
    //private Camera mainCamera;
    ////카메라 줌인/아웃 속도
    //private float rate = 0.06f;
    ////줌 상태
    //private bool zoom = false;

    void Start()
    {
        //메모리 정리
        System.GC.Collect();

        //rb = GetComponent<Rigidbody>();

        //플레이어 이동 방향 초기화
        moveVector = new Vector2(0, 0);

        chr = GameObject.Find("Chr_001_");
        anim = GameObject.Find("Chr_001_").GetComponent<Animator>();

        //mainCamera = GameObject.Find("Main_Camera").GetComponent<Camera>();
        slider = GameObject.Find("PlayerHPSlider");
        slider.GetComponent<Slider>().value = HP;

        skill3collision = GameObject.Find("skill3Effect").GetComponent<CombatSkill3Collision>();
        effect = GameObject.Find("Effect");
        effPos = GameObject.Find("chrDamagedEffectPosition");
        normalDamagedFx = effect.transform.Find("DamagedEff").gameObject;

        appearanceEff = GameObject.Find("Effect").transform.Find("3D_GroundAttack_02").gameObject;

        damageImage = GameObject.Find("HPScript").GetComponent<DamageImageScript>();

        ObjCanvas = GameObject.Find("DamageObj");


        StartCoroutine(appear());
    }

    void Update()
    {
        //조이스틱 입력 받기
        HandleInput();


        //애니메이션
        setAnimation();

        //위치 리셋
        if (resetPositionFlag)
            resetPosition();
        else prePositionX = transform.position.x;

    }

    void FixedUpdate()
    {
            //플레이어 이동
            Move();

            //플레이어 감속
            //EaseVelocity();
    }

    public void HandleInput()
    {
        moveVector = PoolInput();
    }

    public Vector2 PoolInput()
    {
        Vector2 direction = Vector2.zero;

        //x축 이동만.
        direction.x = joystick.Horizontal();
        //direction.y = joystick.Vertical();

        if (direction.magnitude > 1)
            direction.Normalize();

        return direction;
    }

    public void Move()
    {
        transform.Translate(moveVector * moveSpeed);
        //rb.AddForce(moveVector * moveSpeed);
    }

    ////감속
    //public void EaseVelocity()
    //{
    //    Vector2 easeVelocity = rb.velocity;
    //    easeVelocity.x *= 0.85f;
    //    easeVelocity.y *= 0.85f;
    //    rb.velocity = easeVelocity;
    //}

    //등장 효과
    IEnumerator appear()
    {
        //yield return new WaitForSeconds(0.1f);
        anim.SetTrigger("appear");
        
        yield return new WaitForSeconds(0.5f);
        appearanceEff.SetActive(true);
        
        yield return new WaitForSeconds(2.0f);
        appearanceEff.SetActive(false);

    }


    //캐릭터 위치 초기화
    public void resetPosition()
    {
        if (transform.position.x != -5)
        {
            if (transform.position.x < -5) anim.SetFloat("speed", 1.0f);
            else anim.SetFloat("speed", -1.0f);

            anim.SetBool("walk", true);
            time += Time.deltaTime * 1.0f;
            transform.position = new Vector3(Mathf.Lerp(prePositionX, -5, time), transform.position.y, transform.position.z);
        }
        else
        {
            time = 0;
            resetPositionFlag = false;
            anim.SetBool("walk", false);
        }
    }


    //애니메이션
    private void setAnimation()
    {
        //조이스틱 눌렀을 때
        if (joystick.getJoystick())
        {
            if(joystick.getFront()) anim.SetFloat("speed", 1.0f);
            else anim.SetFloat("speed", -1.0f);
            anim.SetBool("walk", true);
        }
        else if (!joystick.getJoystick())
        {
            anim.SetBool("walk", false);
        }
        //기본 상태
        else
        {
            anim.SetTrigger("idle");
        }
    }

    //기본 공격
    public void attackAnimation()
    {
        if (!GameObject.Find("ItemScript").GetComponent<ItemScript>().getHugeFlag())
        {
            anim.ResetTrigger("skill1");
            anim.ResetTrigger("skill2");
            anim.ResetTrigger("skill3");
            anim.ResetTrigger("damaged");
            
            //anim.SetTrigger("attack");
            anim.CrossFade("attack", 0.01f);
        }
    }
    //스킬1
    public void skill1Animation()
    {
        if (!GameObject.Find("ItemScript").GetComponent<ItemScript>().getHugeFlag())
        {
            anim.ResetTrigger("attack");
            anim.ResetTrigger("skill2");
            anim.ResetTrigger("skill3");
            anim.ResetTrigger("damaged");

            anim.SetTrigger("skill1");
        }
    }
    //스킬2
    public void skill2Animation()
    {
        if (!GameObject.Find("ItemScript").GetComponent<ItemScript>().getHugeFlag())
        {
            anim.ResetTrigger("skill1");
            anim.ResetTrigger("attack");
            anim.ResetTrigger("skill3");
            anim.ResetTrigger("damaged");

            anim.SetTrigger("skill2");
        }
    }
    //스킬3
    public void skill3Animation()
    {
        if (!GameObject.Find("ItemScript").GetComponent<ItemScript>().getHugeFlag())
        {
            anim.ResetTrigger("skill1");
            anim.ResetTrigger("skill2");
            anim.ResetTrigger("attack");
            anim.ResetTrigger("damaged");

            anim.SetTrigger("skill3");
            anim.ResetTrigger("damaged");
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Coin") StartCoroutine(getCoin());
    }

    //트리거. 보스 몬스터 범위 감지
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "range1") range1Flag = true;
        if (other.transform.tag == "range2") range2Flag = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "range1") range1Flag = false;
        if (other.transform.tag == "range2") range2Flag = false;
    }

    //collision
    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Monster")
        {
            monsterName = other.transform.name;
            //StartCoroutine(cameraZoomIn());
        }
    }
    //충돌
    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag == "Monster")
        {
            //일반 몬스터
            if (!damagedFlag && other.transform.name != "BossMonster"
                &&!GameObject.Find(other.transform.name).GetComponent<MonsterMove>().getDeath())
            {
                GameObject.Find(other.transform.name).GetComponent<MonsterMove>().Attack();
                StartCoroutine(damaged(1));
                StartCoroutine(PrintDamagedNormalEffect(0.5f));
            }
        }
    }
    //void OnCollisionExit(Collision other)
    //{
    //    if (other.transform.tag == "Monster")
    //    {
    //        //StartCoroutine(cameraZoomOut());
    //    }
    //}

    //일반 피격 이펙트
    public IEnumerator PrintDamagedNormalEffect(float time)
    {

        yield return new WaitForSeconds(time);
        //animation
        anim.SetTrigger("damaged");


        //이펙트 위치
        effPos.transform.position = chr.transform.position;
        effPos.transform.Translate(0, 2f, -10f);
        //이펙트
        GameObject eff = Instantiate(normalDamagedFx);
        eff.transform.SetParent(effPos.transform.Find("chrDamagedEffPosition").transform, false); //effect.transform, false);
        eff.transform.position = effPos.transform.position;
        eff.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(eff);
    }

    public IEnumerator damaged(int d)
    {
        HP -= d;
        slider.GetComponent<Slider>().value = HP;
        StartCoroutine(damageImage.damageImageDown(chr.transform, d, false));
        yield return null;
    }

    public void heal(int h)
    {
        HP += h;
        if (HP > 2000) HP = 2000;
        slider.GetComponent<Slider>().value = HP;
    }

    public void winAnimation()
    {
        anim.ResetTrigger("skill1");
        anim.ResetTrigger("skill2");
        anim.ResetTrigger("skill3");
        anim.ResetTrigger("attack");
        anim.ResetTrigger("damaged");
        anim.SetBool("walk", false);

        anim.SetTrigger("win");
    }

    //코인 획득 효과
    IEnumerator getCoin()
    {
        effPos.transform.position = chr.transform.position;
        effPos.transform.Translate(0, 2f, -10f);

        //이펙트
        GameObject eff = GameObject.Find("Effect").transform.Find("3D_PickItem_02").gameObject;
        eff.transform.position = effPos.transform.position;

        eff.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        eff.SetActive(false);
    }

    ////카메라 줌인
    //public IEnumerator cameraZoomIn()
    //{
    //    zoom = true;
    //    //float size = 1f;
    //    float pos = 1f;
    //    while (pos > 0)
    //    {
    //        mainCamera.orthographicSize -= rate;
    //        //size -= rate;
    //        mainCamera.transform.Translate(new Vector2(-rate, -rate));
    //        pos -= rate;
    //        yield return null;
    //    }
    //}
    ////카메라 줌아웃
    //public IEnumerator cameraZoomOut()
    //{
    //    zoom = false;
    //    //float size = 0f;
    //    float pos = 0f;
    //    while (pos < 1)
    //    {
    //        mainCamera.orthographicSize += rate;
    //        //size += rate;
    //        mainCamera.transform.Translate(new Vector2(rate, rate));
    //        pos += rate;
    //        yield return null;
    //    }
    //}

    //public void setCollMonster(bool b)
    //{
    //    collMonster = b;
    //}
    //public bool getCollMonster()
    //{
    //    return collMonster;
    //}
    //public bool getZoom()
    //{
    //    return zoom;
    //}
    public void setDamagedFlag(bool flag)
    {
        damagedFlag = flag;
    }
    public string getMonsterName()
    {
        return monsterName;
    }
    public void resetMonsterName()
    {
        monsterName = null;
    }
    public bool getResetPositionFlag()
    {
        return resetPositionFlag;
    }
    public void setResetPositionFlag(bool b)
    {
        resetPositionFlag = b;
    }
    public bool getRange1Flag()
    {
        return range1Flag;
    }
    public bool getRange2Flag()
    {
        return range2Flag;
    }
}
