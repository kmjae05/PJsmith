using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarScript : MonoBehaviour {

    private Camera worldCam;
    private Camera uiCam;
    private Transform player;
    private Transform[] monster;
    private Transform bossMonster;

    //플레이어 HP
    private GameObject HPBar;

    //몬스터 HP
    private GameObject[] monsterHPBar;
    //보스 hp
    private GameObject bossMonsterHPBar;

    // Use this for initialization
    void Start()
    {
        worldCam = GameObject.Find("Main_Camera").GetComponent<Camera>();
        uiCam = GameObject.Find("HPBarCamera").GetComponent<Camera>();
        player = GameObject.Find("Chr").transform;
        monster = new Transform[3];
        for (int i = 0; i < monster.Length; i++)
            monster[i] = GameObject.Find("monster" + i).transform;
        bossMonster = GameObject.Find("BossMonster").transform;

        HPBar = GameObject.Find("PlayerHPSlider");
        monsterHPBar = new GameObject[3];
        for (int i = 0; i < monsterHPBar.Length; i++)
            monsterHPBar[i] = GameObject.Find("MonsterHPSlider" + i);
        bossMonsterHPBar = GameObject.Find("BossMonsterHPSlider");
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            SetPositionHUD();
        }
        for (int i = 0; i < monster.Length; i++)
        {
            if (monster[i])
                SetPositionHUD_Monster(i);
        }
        if (bossMonster)
        {
            SetPositionHUD_BossMonster();
        }
    }
  
    //플레이어 HP
    void SetPositionHUD()
    {
        //playerposition을 게임카메라의 viewPort 좌표로 변경. 
        Vector3 position = worldCam.WorldToViewportPoint(player.position);
        position.x += 0.01f;
        position.y += 0.31f;
        //해당 좌표를 uiCamera의 World좌표로 변경. 
        HPBar.transform.position = uiCam.ViewportToWorldPoint(position);
        //값 정리. 
        position = HPBar.transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = 0.0f;
        HPBar.transform.localPosition = position;

    }
    //몬스터 HP
    void SetPositionHUD_Monster(int i)
    {
        //position을 게임카메라의 viewPort 좌표로 변경. 
        Vector3 position = worldCam.WorldToViewportPoint(monster[i].position);
        //position.x += 0.01f;
        position.y += 0.21f;
        //해당 좌표를 uiCamera의 World좌표로 변경. 
        monsterHPBar[i].transform.position = uiCam.ViewportToWorldPoint(position);
        //값 정리. 
        position = monsterHPBar[i].transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = 0.0f;
        monsterHPBar[i].transform.localPosition = position;

    }
    //보스몬스터 HP
    void SetPositionHUD_BossMonster()
    {
        //position을 게임카메라의 viewPort 좌표로 변경. 
        Vector3 position = worldCam.WorldToViewportPoint(bossMonster.position);
        position.y += 0.23f;
        //해당 좌표를 uiCamera의 World좌표로 변경. 
        bossMonsterHPBar.transform.position = uiCam.ViewportToWorldPoint(position);
        //값 정리. 
        position = bossMonsterHPBar.transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = 0.0f;
        bossMonsterHPBar.transform.localPosition = position;

    }
}
