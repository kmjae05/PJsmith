using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterControl : MonoBehaviour {

    //몬스터 객체
    private GameObject[] monsterObj;
    //몬스터
    private MonsterMove[] monster;
    //몬스터 체력 바
    private GameObject[] monsterSlider;
    //몬스터 미니맵
    private GameObject[] minimonster;


    //보스
    private GameObject bossObj;
    private BossMonsterMove boss;
    //보스 체력 바
    private GameObject bossMonsterSlider;
    //보스 미니맵
    private GameObject miniBoss;

    //대미지
    private DamageImageScript damageImage;

    private bool waveFlag = true;

    ////이동 거리(newOffsetX)를 가져올 배경 스크립트
    //public CombatBackground background;

    //플레이어 컨트롤러
    private PlayerController playerController;
    

    void Awake()
    {
        monsterObj = new GameObject[3];
        for (int i = 0; i < monsterObj.Length; i++)
            monsterObj[i] = GameObject.Find("monster" + i.ToString());
        monster = new MonsterMove[monsterObj.Length];
        monsterSlider = new GameObject[monsterObj.Length];
        for (int i = 0; i < monsterSlider.Length; i++)
            monsterSlider[i] = GameObject.Find("MonsterHPSlider" + i.ToString());
        for (int i = 0; i < monster.Length; i++)
        {
            monster[i] = monsterObj[i].GetComponent<MonsterMove>();
            monster[i].setSlider(monsterSlider[i]);
        }
        minimonster = new GameObject[monsterObj.Length];
        for (int i = 0; i < minimonster.Length; i++)
            minimonster[i] = GameObject.Find("Minimonster"+ i.ToString());

        bossObj = GameObject.Find("BossMonster");
        boss = GameObject.Find("BossMonster").GetComponent<BossMonsterMove>();
        bossMonsterSlider = GameObject.Find("BossMonsterHPSlider");
        boss.setSlider(bossMonsterSlider);
        miniBoss = GameObject.Find("MiniBossMonster");

        damageImage = GameObject.Find("HPScript").GetComponent<DamageImageScript>();
        playerController = GameObject.Find("Chr").GetComponent<PlayerController>();

    }


    private void Update()
    {
        if(waveFlag)
        {
            GameObject.Find("Obstacle").transform.Find("right").gameObject.SetActive(false);
        }
        else GameObject.Find("Obstacle").transform.Find("right").gameObject.SetActive(true);
    }


    //스킬3 damage 처리
    public void skill3Damage(int d)
    {
        for(int i=0; i < monster.Length; i++)
        {
            if (monster[i].getSkill3Flag())
            {
                StartCoroutine(damageImage.damageImageUp(monsterObj[i].transform, d, false));
                monster[i].damage(d);
            }
        }
        //보스
        if (boss.getSkill3Flag())
        {
            StartCoroutine(damageImage.damageImageUp(boss.transform, d, false));
            boss.damage(d);
        }
    }

    //몬스터 전멸
    public bool annihilation()
    {
        bool death = true;
        for (int i = 0; i < monster.Length; i++)
        {
            if (!monster[i].getDeath())
                death = false;
        }
        if (!boss.getDeath())
            death = false;

        if (!death) return false;
        else return true;
    }

    //몬스터 사망 확인
    public bool monsterDeath(int _index)
    {
        return monster[_index].getDeath();
    }




    //미니맵 오브젝트 삭제
    public void deleteMiniObj(int _index)
    {
        minimonster[_index].SetActive(false);
    }
    //미니맵 보스 오브젝트 삭제
    public void deleteMiniBossObj()
    {
        miniBoss.SetActive(false);
    }

    public void setWaveFlag(bool b)
    {
        waveFlag = b;
    }
    public bool getWaveFlag()
    {
        return waveFlag;
    }

}

