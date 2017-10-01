using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControl : MonoBehaviour {

    private GameObject combatStart;
    private GameObject combatWave1;
    private GameObject combatWave2;
    private GameObject combatWave3;

    //현재 웨이브 상태
    private int nowWave = 1;


    private MonsterControl monsterControl;

    
    void Start () {
        combatStart = GameObject.Find("System").transform.Find("CombatStart").gameObject;
        combatWave1 = GameObject.Find("System").transform.Find("CombatWave1").gameObject;
        combatWave2 = GameObject.Find("System").transform.Find("CombatWave2").gameObject;
        combatWave3 = GameObject.Find("System").transform.Find("CombatWave3").gameObject;

        monsterControl = GameObject.Find("Monsters").GetComponent<MonsterControl>();

        StartCoroutine(startPanel());
    }

    IEnumerator startPanel()
    {
        combatStart.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        combatStart.SetActive(false);
        StartCoroutine(wave1Panel());
    }

    IEnumerator wave1Panel()
    {
        combatWave1.SetActive(true);
        nowWave = 1;
        yield return new WaitForSeconds(1.5f);
        combatWave1.SetActive(false);
        StartCoroutine(wave2Panel());
    }

    IEnumerator wave2Panel()
    {
        while (!monsterControl.monsterDeath(0))
        {
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);
        combatWave2.SetActive(true);
        nowWave = 2;
        GameObject.Find("MinimapScript").GetComponent<MinimapScript>().setWaveFlag(true);
        GameObject.Find("Monsters").GetComponent<MonsterControl>().setWaveFlag(true);
        GameObject.Find("Chr_001_").GetComponent<Animator>().ResetTrigger("skill1");
        GameObject.Find("Chr_001_").GetComponent<Animator>().ResetTrigger("skill2");
        GameObject.Find("Chr_001_").GetComponent<Animator>().ResetTrigger("skill3");
        GameObject.Find("Chr_001_").GetComponent<Animator>().ResetTrigger("attack");
        //캐릭터 위치 초기화
        GameObject.Find("Chr").GetComponent<PlayerController>().setResetPositionFlag(true);
        yield return new WaitForSeconds(1.5f);
        combatWave2.SetActive(false);

        StartCoroutine(wave3Panel());
    }
    IEnumerator wave3Panel()
    {
        while (monsterControl.monsterDeath(1)==false || monsterControl.monsterDeath(2)==false)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);
        combatWave3.SetActive(true);
        nowWave = 3;
        GameObject.Find("MinimapScript").GetComponent<MinimapScript>().setWaveFlag(true);
        GameObject.Find("Monsters").GetComponent<MonsterControl>().setWaveFlag(true);
        GameObject.Find("Chr_001_").GetComponent<Animator>().ResetTrigger("skill1");
        GameObject.Find("Chr_001_").GetComponent<Animator>().ResetTrigger("skill2");
        GameObject.Find("Chr_001_").GetComponent<Animator>().ResetTrigger("skill3");
        GameObject.Find("Chr_001_").GetComponent<Animator>().ResetTrigger("attack");
        //캐릭터 위치 초기화
        GameObject.Find("Chr").GetComponent<PlayerController>().setResetPositionFlag(true);
        yield return new WaitForSeconds(1.5f);
        combatWave3.SetActive(false);

    }


    public int getNowWave()
    {
        return nowWave;
    }

}
