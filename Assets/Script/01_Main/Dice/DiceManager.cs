using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour {

    private GameObject worldCamObj;
    private Vector3 worldCamPos;
    private Camera worldCam;

    private GameObject uiCamObj;
    private Camera uiCam;


    private GameObject DiceGamePopup;   //팝업 창
    
    private GameObject dice;    //숫자 주사위
    private GameObject mulDice; //배수 주사위

    private GameObject buttonBox;   //주사위를 굴리고 있을 때 버튼 클릭 방지.

    private int ticket = 0;             //티켓 수
    private Text ticketText;            //티켓 수 텍스트

    private GameObject checkImage;

    //3D주사위
    private GameObject Dice3DObj;   //3D주사위 상위 오브젝트

    private GameObject dice_pips;   //눈이 있는 주사위
    private Die_d6 dice_pip;        //주사위의 눈
    private GameObject dice_num;    //숫자가 있는 주사위
    private Die_d6 dice_numN;       //주사위의 숫자

    private void Start()
    {
        worldCamObj = GameObject.Find("00_Camera").transform.Find("Dice_Camera").gameObject;
        worldCamPos = worldCamObj.transform.position;
        worldCam = worldCamObj.GetComponent<Camera>();
        uiCamObj = GameObject.Find("UI_Camera");
        uiCam = uiCamObj.GetComponent<Camera>();

        DiceGamePopup = GameObject.Find("System").transform.Find("DiceGamePopup").gameObject;
        buttonBox = DiceGamePopup.transform.Find("buttonBox").gameObject;

        Dice3DObj = GameObject.Find("Dice");

        dice_pips = GameObject.Find("Dice_pips");
        dice_pip = dice_pips.gameObject.GetComponent<Die_d6>();
        dice_num = GameObject.Find("Dice_num");
        dice_numN = dice_num.gameObject.GetComponent<Die_d6>();
        SetPositionHUD();
        
    }
    //private void Update()
    //{
    //    //if (flag)
            //SetPositionHUD();
    //}


    //팝업창 나타날 때 초기화
    public void init()
    {
        if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "티켓") != null)
            ticket = ThingsData.instance.getInventoryThingsList().Find(x => x.name == "티켓").possession;
        else ticket = 0;
        ticketText = GameObject.Find("PossessText").gameObject.GetComponent<Text>();
        ticketText.text = "소지 : " + ticket.ToString();
        checkImage = GameObject.Find("DiceGameUIPanel (1)").transform.Find("checkImage").gameObject;
        checkImage.SetActive(false);
    }



    //주사위 굴리기 버튼
    public void throwButton()
    {
        if (ticket > 0)
        {
            buttonBox.SetActive(true);
            ticket = ticket -1;
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == "티켓").possession = ticket;
            ticketText.text = "소지 : " + ticket.ToString();
            StartCoroutine(throwDice());
        }
        //티켓이 없을 때
        else
        {
            ticket = 0;
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == "티켓").possession = ticket;
        }
        

        
    }


    //주사위 굴리기
    IEnumerator throwDice()
    {
        //위치 이동
        dice_pips.transform.localPosition = new Vector3(-4, -6, -10);
        dice_num.transform.localPosition = new Vector3(5, -6, -10);
        //던지기
        Rigidbody rb = dice_pips.GetComponent<Rigidbody>();
        Vector3 direction = rb.transform.localPosition - GameObject.Find("DiceWall").gameObject.transform.localPosition;
        float rd = Random.Range(65f, 75f);
        rb.AddForce(-direction * rd);
        Rigidbody rb2 = dice_num.GetComponent<Rigidbody>();
        Vector3 direction2 = rb2.transform.localPosition - GameObject.Find("DiceWall").gameObject.transform.localPosition;
        rd = Random.Range(55f, 65f);
        rb2.AddForce(-direction2 * rd);
        //회전 랜덤으로 주기
        rd = Random.Range(100f, 1000f);
        rb.AddTorque(rd, rd, rd);
        rb2.AddTorque(rd, rd, rd);

        yield return new WaitForSeconds(3.0f);
        //주사위 확인. 걸쳤을 경우 다시 굴리기
        if(dice_pip.value == 0 || dice_numN.value == 0)
        {
            StopCoroutine(throwDice());
            StartCoroutine(throwDice());
        }

        //주사위 확인 표시
        if (dice_pip.value != 0)
        {
            GameObject reward = GameObject.Find("reward" + dice_pip.value.ToString());
            checkImage.transform.position = reward.transform.position;
            checkImage.SetActive(true);
        }

        //보상 획득
        int num = dice_numN.value;
        switch (dice_pip.value)
        {
            case 1:
                ticket += num; ticketText.text = "소지 : " + ticket.ToString();
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "티켓") == null)
                    ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "티켓").type, "티켓", num));
                else
                    ThingsData.instance.getInventoryThingsList().Find(x => x.name == "티켓").possession += num;
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == "티켓").recent = true;
                break;
            case 2:
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "철 주괴") == null)
                    { ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "철 주괴").type, "철 주괴", num));  }
                else
                { ThingsData.instance.getInventoryThingsList().Find(x => x.name == "철 주괴").possession += num; }
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == "철 주괴").recent = true;
                break;
            case 3:
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "구리 주괴") == null)
                { ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "구리 주괴").type, "구리 주괴", num)); }
                else
                { ThingsData.instance.getInventoryThingsList().Find(x => x.name == "구리 주괴").possession += num; }
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == "구리 주괴").recent = true; break;
            case 4:
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "돌 주괴") == null)
                { ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "돌 주괴").type, "돌 주괴", num)); }
                else
                { ThingsData.instance.getInventoryThingsList().Find(x => x.name == "돌 주괴").possession += num; }
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == "돌 주괴").recent = true; break;
            case 5:
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "금 주괴") == null)
                { ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "금 주괴").type, "금 주괴", num)); }
                else
                { ThingsData.instance.getInventoryThingsList().Find(x => x.name == "금 주괴").possession += num; }
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == "금 주괴").recent = true; break;
            case 6:
                if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == "은 주괴") == null)
                { ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == "은 주괴").type, "은 주괴", num)); }
                else
                { ThingsData.instance.getInventoryThingsList().Find(x => x.name == "은 주괴").possession += num; }
                ThingsData.instance.getInventoryThingsList().Find(x => x.name == "은 주괴").recent = true; break;
            default: // case 0
                break;
                //
        }
        //획득 효과
        while (num > 0)
        {
            num--;
            GameObject rewardPrefab = Instantiate(GameObject.Find("DiceGameUIPanel (1)").transform.Find("reward").gameObject);
            rewardPrefab.transform.SetParent(GameObject.Find("InfoBox").transform, false);
            //위치 지정
            GameObject reward = GameObject.Find("reward" + dice_pip.value.ToString());
            rewardPrefab.transform.position = reward.transform.position;
            //이미지 교체
            string path = null;
            switch (dice_pip.value)
            {
                case 1: path = ThingsData.instance.getThingsList().Find(x => x.name == "티켓").icon; break;
                case 2: path = ThingsData.instance.getThingsList().Find(x => x.name == "철 주괴").icon; break;
                case 3: path = ThingsData.instance.getThingsList().Find(x => x.name == "구리 주괴").icon; break;
                case 4: path = ThingsData.instance.getThingsList().Find(x => x.name == "돌 주괴").icon; break;
                case 5: path = ThingsData.instance.getThingsList().Find(x => x.name == "금 주괴").icon; break;
                case 6: path = ThingsData.instance.getThingsList().Find(x => x.name == "은 주괴").icon; break;
            }
            rewardPrefab.transform.Find("rewardImg").GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
            rewardPrefab.SetActive(true);

            yield return new WaitForSeconds(0.3f);
        }



        yield return new WaitForSeconds(1.0f);
        checkImage.SetActive(false);
        buttonBox.SetActive(false);

    }







    //UI에 맞게 위치 고정
    void SetPositionHUD()
    {
        Vector3 position = uiCam.ViewportToWorldPoint(Dice3DObj.transform.position);

        transform.position = worldCam.WorldToViewportPoint(position);

        //값 정리. 
        position = transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = -100.0f;
        transform.localPosition = position;
        position.z = -115.0f;
    }

}
