using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSkill3Collision : MonoBehaviour {

    //몬스터 충돌 여부
    private bool collMonster = false;

    //collider
    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Monster")
        {
            collMonster = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Monster")
        {
            collMonster = false;
        }
    }
    public bool getCollMonster()
    {
        return collMonster;
    }
    public void setCollMonster(bool b)
    {
        collMonster = b;
    }

}
