using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapManager : MonoBehaviour {

    private Camera worldCam;
    private Camera uiCam;

    //선택 대륙 번호
    private int ContNum =1;
    private string Contname;
    private string info;

    //대륙 정보 팝업창
    private GameObject ContPopup;
    public Text nameText;
    public Text infoText;

    //monster 위치 조절
    private GameObject monster;
    public GameObject monsterPos;
    //등장하는 몬스터
    private GameObject appearance;

    //스테이지 정보
    private StageManager stageManager;



    private void Awake()
    {
        worldCam = GameObject.Find("Profile_Camera").GetComponent<Camera>();
        uiCam = GameObject.Find("UI_Camera (1)").GetComponent<Camera>();

        ContPopup = GameObject.Find("System").transform.Find("ContinentPopup").gameObject;

        monster = GameObject.Find("Monster");

        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }
    private void Update()
    {
        SetPositionHUD();
        Loop();
    }


    void Loop()
    {
        if (ContNum == 1) { Contname = "아케도니아"; info = "재해로 인해 폐허가 된 도시."; }
        if (ContNum == 2) { Contname = "플루오네"; info = "천계 전쟁으로 사라져 버린 신들의 강력한 무기들이 숨겨져 있다."; }
        if (ContNum == 3) { Contname = "일사바드"; info = "죽은 자들의 원한이 활개치는 곳."; }
        if (ContNum == 4) { Contname = "원무제국"; info = "약탈을 일삼는 소수민족이 살고 있다."; }
        if (ContNum == 5) { Contname = "드래곤로드"; info = "드래곤로드"; }

        nameText.text = Contname;
        infoText.text = info;
    }



    public void getContinentNum(int n)
    {
        ContNum = n;
        string str = "아케도니아";
        if (n == 1) str = "아케도니아"; if (n == 2) str = "플루오네"; if (n == 3) str = "일사바드";
        if (n == 4) str = "원무제국"; if (n == 5) str = "드래곤로드";
        stageManager.SetCurContSelect(str);
    }

    //팝업창 켜고 끌 때
    public void appearMonster(bool b)
    {
        if (b) { selectMonster(); appearance.SetActive(true);  }
        else appearance.SetActive(false);
    }
    private void selectMonster()
    {
        monster.transform.Find("Skeleton").gameObject.SetActive(false);
        monster.transform.Find("Golem").gameObject.SetActive(false);
        if (ContNum == 2 || ContNum == 3)
        {
            appearance = monster.transform.Find("Skeleton").gameObject;
        }
        else if(ContNum == 1 || ContNum == 4 || ContNum == 5)
        {
            appearance = monster.transform.Find("Golem").gameObject;
        }
    }

    //스테이지 입장 버튼
    public void stageEnterButton()
    {
        GameObject.Find("Menu").transform.Find("WorldMapPopup/ContinentStage" + ContNum).gameObject.SetActive(true);
    }

    //UI에 맞게 위치 고정
    void SetPositionHUD()
    {
        Vector3 position = uiCam.ViewportToWorldPoint(monsterPos.transform.position);

        monster.transform.position = worldCam.WorldToViewportPoint(position);

        //값 정리. 
        position = monster.transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = 1.0f;
        monster.transform.localPosition = position;
    }



    public int getContNum() { return ContNum; }
    public void setContNum(int i) { ContNum = i; }

}
