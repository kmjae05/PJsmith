using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DontDestroyScript : MonoBehaviour {

    public static DontDestroyScript instance = null;

    private bool combatFlag = false;

	void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //combat에서 로비로 가는 버튼을 누르면
        //로비에서 월드맵 팝업창을 띄운다

        if (combatFlag)
        {
            if (SceneManager.GetActiveScene().name == "02_Lobby")
            {
                combatFlag = false;
                GameObject.Find("Menu").transform.Find("WorldMapPopup").gameObject.SetActive(true);
            }
        }
    }


    public bool getCombatFlag()
    {
        return combatFlag;
    }
    public void setCombatFlag(bool b)
    {
        combatFlag = b;
    }


}
