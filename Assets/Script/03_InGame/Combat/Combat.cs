using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour {


    //캐릭터
    private GameObject chr;
    //이펙트 포지션
    private GameObject effPos;
    private GameObject effPosSkill3;

    //대미지
    private DamageImageScript damageImage;
    private GameObject ObjCanvas;
    private GameObject Damage_Normal;
    private GameObject Damage_Critical;
    private GameObject Damage_Skill3;

    private GameObject effect;
    private GameObject normalFx;
    private GameObject criFx;
    private GameObject comboFx;
    private GameObject skill1Fx, skill1Fx2;
    private GameObject skill2Fx, skill2Fx2;
    private GameObject skill3_1Fx, skill3_2Fx;
    private GameObject[] skill3_3Fx;
    //스킬3 활성화
    private bool skill3 = false;
    //스킬info 쿨타임 flag
    private bool skill1InfoTimeFlag = false;
    private float skill1InfoTime = 0;
    private bool skill2InfoTimeFlag = false;
    private float skill2InfoTime = 0;

    //콤보
    private int combo = 0;
    private int comboCount = 0;
    ////스킬 쿨타임
    //public int cooltime = 3; //애니메이션에서 조절


    private int chrPower = 0;
    private int chrPowerPlus = 0;

    private float d_vel = 20;
    private float d_acc = -1;
    private float d_y = 300;
    private float d_fade = 0.02f;

    //몬스터 충돌 여부
    private bool collMonster = false;    
    //충돌한 몬스터 이름
    private string monsterName;


    //캐릭터와 몬스터 충돌 flag
    private PlayerController player;
    //스킬3 범위와 몬스터 충돌 flag
    private CombatSkill3Collision skill3collision;
    //몬스터에게 대미지
    private MonsterControl monControl;


    void Start()
    {
        player = GameObject.Find("Chr").GetComponent<PlayerController>();
        skill3collision = GameObject.Find("skill3Effect").GetComponent<CombatSkill3Collision>();
        monControl = GameObject.Find("Monsters").GetComponent<MonsterControl>();

        chrPower = (int)Player.instance.getUser().stat.strPower + Player.equipHm.power + 20;

        chr = GameObject.Find("Chr");
        effPos = GameObject.Find("EffectPosition");
        effPosSkill3 = GameObject.Find("EffectPositionSkill3");

        damageImage = GameObject.Find("HPScript").GetComponent<DamageImageScript>();
        ObjCanvas = GameObject.Find("DamageObj");

        effect = GameObject.Find("Effect");
        normalFx = effect.transform.Find("3D_Hit_01").gameObject;
        criFx = effect.transform.Find("3D_CriticalHit_01").gameObject;
        comboFx = effect.transform.Find("3D_Hit_03").gameObject;

        skill1Fx = effect.transform.Find("3D_Recovery_04").gameObject;
        skill1Fx2 = effect.transform.Find("2D_BoostDefense_01").gameObject;
        skill2Fx = effect.transform.Find("3D_AirExplode_01").gameObject;
        skill2Fx2 = effect.transform.Find("2D_BoostAttack_01").gameObject;
        skill3_2Fx = effect.transform.Find("3D_Charge").gameObject;
        skill3_3Fx = new GameObject[6];
        for (int i = 0; i < 6; i++)
            skill3_3Fx[i] = effect.transform.Find("3D_ThunderMagic_02 (" + i.ToString() + ")").gameObject;
    }

    //void Update()
    //{
        
    //    if (skill1InfoTimeFlag)
    //    {
    //        skill1InfoTime += Time.deltaTime;
    //        if (skill1InfoTime > 5)
    //        {
    //            //계속 유지
    //            //skill1InfoTimeFlag = false;
    //            skill1InfoTime = 0;
    //        }
    //    }
    //    if (skill2InfoTimeFlag)
    //    {
    //        skill2InfoTime += Time.deltaTime;
    //        if (skill2InfoTime > 5)
    //        {
    //            //skill2InfoTimeFlag = false;
    //            skill2InfoTime = 0;
    //        }
    //    }
    //}


    //attack 애니메이션 이벤트
    public void AnimationEventHit()
    {

        //콤보 이후 쿨타임
        if (comboCount >= 5)
        {
            comboCount = 0;
            if (collMonster)
            {
                ////attack, skill 쿨타임
                //GameObject.Find("AttackButton").transform.Find("AttackCooldowntime").gameObject.SetActive(true);
                //GameObject.Find("Skill1Button").transform.Find("Skill1Cooldowntime").gameObject.SetActive(true);
                //GameObject.Find("Skill2Button").transform.Find("Skill2Cooldowntime").gameObject.SetActive(true);
                //GameObject.Find("Skill3Button").transform.Find("Skill3Cooldowntime").gameObject.SetActive(true);

                StartCoroutine(comboAttack());
                //쿨타임 off
                //StartCoroutine(disappear());
            }
        }
        else
        {

            //확률
            int critical_rate = Random.Range(0, 100);

            if (critical_rate > 30)
            {
                //일반 공격
                NormalHit();
            }
            else if (critical_rate <= 30)
            {
                //크리티컬 공격
                CriticalHit();
            }

            if (combo != 0)
            {
                if (GameObject.Find("ComboImage(Clone)"))
                {
                    Destroy(GameObject.Find("ComboImage(Clone)"));
                    Destroy(GameObject.Find("ComboImage(Clone)"));
                }

                GameObject.Find("HPScript").GetComponent<ComboImageScript>().setCombo();
            }
            else Destroy(GameObject.Find("ComboImage(Clone)"));

        }
        chr.transform.Find("Chr_001_").gameObject.GetComponent<Animator>().ResetTrigger("damaged");

    }


    //일반 공격
    void NormalHit()
    {
        if (collMonster)
        {
            int damage = 0;
            int random = Random.Range(1, 7);
            switch (random)
            {
                case 1: damage = chrPower; break;
                case 2: damage = (int)(chrPower * 1.05f); break;
                case 3: damage = (int)(chrPower * 1.1f); break;
                case 4: damage = (int)(chrPower * 0.95f); break;
                case 5: damage = (int)(chrPower * 0.9f); break;
                case 6: damage = (int)(chrPower * 0.85f); break;
            }
            if (monsterName == "BossMonster")
            {
                if (!GameObject.Find(monsterName).GetComponent<BossMonsterMove>().getDeath())
                {
                    combo++;
                    comboCount++;

                    GameObject.Find(monsterName).GetComponent<BossMonsterMove>().damage(damage);
                    StartCoroutine(damageImage.damageImageUp(GameObject.Find(monsterName).transform, damage, false));
                    StartCoroutine(PrintDamageNormalEffect());
                }
                else { combo = 0; comboCount = 0; }
            }
            else
            {
                if (!GameObject.Find(monsterName).GetComponent<MonsterMove>().getDeath())
                {
                    combo++;
                    comboCount++;

                    GameObject.Find(monsterName).GetComponent<MonsterMove>().damage(damage);
                    StartCoroutine(damageImage.damageImageUp(GameObject.Find(monsterName).transform, damage, false));
                    StartCoroutine(PrintDamageNormalEffect());
                }
                else { combo = 0; comboCount = 0; }
            }

        }
        else { combo = 0; comboCount = 0; }

    }
    //크리티컬 공격
    void CriticalHit()
    {
        if (collMonster)
        {
            int damage = 0;
            int random = Random.Range(1, 3);
            switch (random)
            {
                case 1: damage = (int)(chrPower * 1.7f); break;
                case 2: damage = (int)(chrPower * 1.9f); break;
            }
            if (monsterName == "BossMonster")
            {
                if (!GameObject.Find(monsterName).GetComponent<BossMonsterMove>().getDeath())
                {
                    combo++;
                    comboCount++;

                    GameObject.Find(monsterName).GetComponent<BossMonsterMove>().damage(damage);
                    StartCoroutine(damageImage.damageImageUp(GameObject.Find(monsterName).transform, damage, true));
                    StartCoroutine(PrintDamageCriticalEffect());
                }
                else { combo = 0; comboCount = 0; }
            }
            else
            {
                if (!GameObject.Find(monsterName).GetComponent<MonsterMove>().getDeath())
                {
                    combo++;
                    comboCount++;

                    GameObject.Find(monsterName).GetComponent<MonsterMove>().damage(damage);
                    StartCoroutine(damageImage.damageImageUp(GameObject.Find(monsterName).transform, damage, true));
                    StartCoroutine(PrintDamageCriticalEffect());
                }
                else { combo = 0; comboCount = 0; }
            }
        }
        else { combo = 0; comboCount = 0; }

    }

    //콤보 공격
    IEnumerator comboAttack()
    {
        for (int i = 0; i < 5; i++)
        {

            combo++;
            if(GameObject.Find("ComboImage(Clone)"))
                Destroy(GameObject.Find("ComboImage(Clone)"));
            GameObject.Find("HPScript").GetComponent<ComboImageScript>().setCombo();

            int damage = 0;
            int random = Random.Range(1, 9);
            switch (random)
            {
                case 1: damage = chrPower; break;
                case 2: damage = (int)(chrPower * 1.05f); break;
                case 3: damage = (int)(chrPower * 1.1f); break;
                case 4: damage = (int)(chrPower * 0.95f); break;
                case 5: damage = (int)(chrPower * 0.9f); break;
                case 6: damage = (int)(chrPower * 0.85f); break;
                case 7: damage = (int)(chrPower * 1.15f); break;
                case 8: damage = (int)(chrPower * 1.2f); break;
            }
            if (monsterName == "BossMonster")
            {
                if (!GameObject.Find(monsterName).GetComponent<BossMonsterMove>().getDeath())
                {
                    GameObject.Find(monsterName).GetComponent<BossMonsterMove>().damage(damage);
                    StartCoroutine(damageImage.damageImageUp(GameObject.Find(monsterName).transform, damage, false));
                    StartCoroutine(PrintDamageComboEffect());
                }
            }
            else
            {
                if (!GameObject.Find(monsterName).GetComponent<MonsterMove>().getDeath())
                {
                    GameObject.Find(monsterName).GetComponent<MonsterMove>().damage(damage);
                    StartCoroutine(damageImage.damageImageUp(GameObject.Find(monsterName).transform, damage, false));
                    StartCoroutine(PrintDamageComboEffect());
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    //일반 공격 이펙트
    IEnumerator PrintDamageNormalEffect()
    {
        //이펙트 위치
        effPos.transform.position = GameObject.Find(monsterName).transform.position;// chr.transform.position;
        //이펙트
        GameObject eff = Instantiate(normalFx);
        eff.transform.SetParent(effPos.transform.Find("EffPosition").transform, false); //effect.transform, false);
        if (monsterName == "BossMonster")
        {
            effPos.transform.Translate(0, 3f, 0);
            eff.transform.localScale.Set(2f, 2f, 0);
        }

        eff.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(eff);
    }

    //크리티컬 공격 이펙트
    IEnumerator PrintDamageCriticalEffect()
    {
        //이펙트 위치
        effPos.transform.position = GameObject.Find(monsterName).transform.position;
        if (monsterName == "BossMonster")
            effPos.transform.Translate(0, 3f, 0);
        //이펙트
        GameObject eff = Instantiate(criFx);
        eff.transform.SetParent(effPos.transform.Find("EffPosition").transform, false); //effect.transform, false);
        eff.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(eff);
    }

    //콤보 공격 이펙트
    IEnumerator PrintDamageComboEffect()
    {
        //이펙트 위치
        effPos.transform.position = GameObject.Find(monsterName).transform.position;
        if (monsterName == "BossMonster")
            effPos.transform.Translate(0, 3f, 0);
        //이펙트
        GameObject eff = Instantiate(comboFx);
        eff.transform.SetParent(effPos.transform.Find("EffPosition").transform, false);
        eff.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(eff);
    }


    #region 임시 스킬

    //스킬1 attack_lose
    void AnimationEventSkill1()
    {
        chr.transform.Find("Chr_001_").gameObject.GetComponent<Animator>().ResetTrigger("damaged");
        StartCoroutine(PrintDamageSkill1());
    }

    //스킬2 attack_win
    void AnimationEventSkill2()
    {
        chr.transform.Find("Chr_001_").gameObject.GetComponent<Animator>().ResetTrigger("damaged");
        StartCoroutine(PrintDamageSkill2());
    }
    //스킬3 attack_chargein
    void AnimationEventSkill3chargein()
    {

    }
    //스킬3 attack_charge
    void AnimationEventSkill3charge()
    {
        skill3_2Fx.SetActive(true);
    }
    //스킬3 attack_chargeout
    void AnimationEventSkill3chargeout()
    {
        skill3_2Fx.SetActive(false);
        //적이 스킬 범위에 있는 경우
        if (skill3collision.getCollMonster())
        {
            if (!skill3) skill3 = true;

            int random = Random.Range(1, 3);
            if(random == 1)
                monControl.skill3Damage(132);
            else if (random == 2)
                monControl.skill3Damage(146);
            //damageImage는 MonsterControl에서 처리
        }
        else skill3 = false;
        chr.transform.Find("Chr_001_").gameObject.GetComponent<Animator>().ResetTrigger("damaged");
        StartCoroutine(PrintDamageSkill3effect());
    }


    IEnumerator PrintDamageSkill1()
    {
        //이펙트
        GameObject eff = Instantiate(skill1Fx);
        eff.transform.SetParent(effect.transform, false);
        eff.SetActive(true);
        GameObject eff2 = Instantiate(skill1Fx2);
        eff2.transform.SetParent(effect.transform, false);
        eff2.SetActive(true);

        GameObject effInfo = null;
        //ui 표시
        if (!skill1InfoTimeFlag)
        {
            skill1InfoTimeFlag = true;
            effInfo = Instantiate(GameObject.Find("EffectInfo").transform.Find("2D_BoostDefense_01").gameObject);
            effInfo.transform.SetParent(GameObject.Find("EffectInfo").transform, false);
            effInfo.SetActive(true);
        }
        yield return new WaitForSeconds(2.0f);
        Destroy(eff);
        Destroy(eff2);
        //yield return new WaitForSeconds(3.0f);
        //Destroy(effInfo);
    }

    IEnumerator PrintDamageSkill2()
    {        
        //이펙트
        GameObject eff = Instantiate(skill2Fx);
        eff.transform.SetParent(effect.transform, false);
        eff.SetActive(true);
        GameObject eff2 = Instantiate(skill2Fx2);
        eff2.transform.SetParent(effect.transform, false);
        eff2.SetActive(true);
        GameObject effInfo = null;
        //ui 표시
        if (!skill2InfoTimeFlag)
        {
            chrPowerPlus = 10;
            chrPower += chrPowerPlus;
            skill2InfoTimeFlag = true;
            effInfo = Instantiate(GameObject.Find("EffectInfo").transform.Find("2D_BoostAttack_01").gameObject);
            effInfo.transform.SetParent(GameObject.Find("EffectInfo").transform, false);
            effInfo.SetActive(true);
        }
        yield return new WaitForSeconds(2.0f);
        Destroy(eff);
        Destroy(eff2);
        //yield return new WaitForSeconds(3.0f);
        //chrPower -= chrPowerPlus;
        //Destroy(effInfo);
    }


    IEnumerator PrintDamageSkill3effect()
    {
        //카메라 흔들
        GameObject.Find("CameraShake").GetComponent<CameraShake>().EnableShake(0.3f);

        //이펙트 위치
        effPosSkill3.transform.position = chr.transform.position;

        //이펙트
        GameObject[] eff = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            eff[i] = Instantiate(skill3_3Fx[i]);
            eff[i].transform.SetParent(effPosSkill3.transform.Find("EffPosition").transform, false); //effect.transform, false);
            eff[i].SetActive(true);
        }
        yield return new WaitForSeconds(1.2f);
        for (int i = 0; i < 6; i++)
            Destroy(eff[i]);
        skill3 = false;
    }


    #endregion

    //collider
    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Monster")
        {
            collMonster = true;
            monsterName = other.transform.name;
        }
    }
    //void OnTriggerStay(Collider other)
    //{
    //    //if (other.transform.tag == "Monster")
    //    //{
            
    //    //}
    //    //Debug.Log(monsterName);
    //}
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Monster")
        {
            collMonster = false;
            monsterName = null;
        }
    }


    //IEnumerator disappear()
    //{
    //    yield return new WaitForSeconds(cooltime);
    //    GameObject.Find("AttackButton").transform.Find("AttackCooldowntime").gameObject.SetActive(false);
    //    GameObject.Find("Skill1Button").transform.Find("Skill1Cooldowntime").gameObject.SetActive(false);
    //    GameObject.Find("Skill2Button").transform.Find("Skill2Cooldowntime").gameObject.SetActive(false);
    //    GameObject.Find("Skill3Button").transform.Find("Skill3Cooldowntime").gameObject.SetActive(false);
    //}


    public bool getSkill3()
    {
        return skill3;
    }
    public bool getCollMonster()
    {
        return collMonster;
    }
    public void setCollMonster(bool b)
    {
        collMonster = b;
    }
    public void nullMonsterName()
    {
        monsterName = null;
    }
    public void resetMonsterInfo()
    {
        collMonster = false;
        monsterName = null;
    }

    public int getCombo()
    {
        return combo;
    }

}
