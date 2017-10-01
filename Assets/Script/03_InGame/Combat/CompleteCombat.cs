using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteCombat : MonoBehaviour {

    private MonsterControl monsterControl;
    private GameObject combatComplete;
    private GameObject combatReward;
    private bool flag = false;  //1번 실행

    private void Awake()
    {
        monsterControl = GameObject.Find("Monsters").GetComponent<MonsterControl>();
        combatComplete = GameObject.Find("System").transform.Find("CombatComplete").gameObject;
        combatReward = GameObject.Find("System").transform.Find("CombatReward").gameObject;
    }

    private void Update()
    {
        if (!flag)
        {
            if (monsterControl.annihilation())
            {
                flag = true;
                StartCoroutine(rewardPopup());
            }
        }
    }

    IEnumerator rewardPopup()
    {
        GameObject.Find("CombatManager").GetComponent<CombatManager>().setScale(1.0f);
        //터치 방지
        GameObject.Find("FadeCanvas").transform.Find("back").gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        //캐릭터 승리 애니메이션
        GameObject.Find("Chr").GetComponent<PlayerController>().winAnimation();
        yield return new WaitForSeconds(2.5f);
        GameObject.Find("FadeCanvas").transform.Find("back").gameObject.SetActive(false);
        GameObject.Find("System").transform.Find("EffectCanvas").gameObject.SetActive(true);
        combatComplete.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("System").transform.Find("EffectCanvas").gameObject.SetActive(false);
        combatReward.SetActive(true);

    }




}
