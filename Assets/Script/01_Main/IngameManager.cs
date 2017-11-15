using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameManager : MonoBehaviour {

    private Animator CharAni;

    private GameObject startButton;

    GameObject ingameUI;
    Button ingameButton;
    Slider slider;
    Text oreText;
    private Image frame;
    Image oreImage;
    Text timeText;

    int goldPrice = 100;

    private void Start()
    {
        CharAni = GameObject.Find("Chr_001").GetComponent<Animator>();
        startButton = GameObject.Find("StartButton");

        ingameUI = GameObject.Find("System").transform.Find("InGameUI").gameObject;
        ingameButton = ingameUI.transform.Find("HpBox").gameObject.GetComponent<Button>();
        slider = ingameUI.transform.Find("HpBox/HPBar").gameObject.GetComponent<Slider>();
        oreText = ingameUI.transform.Find("HpBox/HPBar/OreText").gameObject.GetComponent<Text>();
        frame = ingameUI.transform.Find("HpBox/OreBox").gameObject.GetComponent<Image>();
        oreImage = ingameUI.transform.Find("HpBox/OreBox/OreIcon").gameObject.GetComponent<Image>();
        timeText = ingameUI.transform.Find("HpBox/TimeText").gameObject.GetComponent<Text>();

        if (Player.instance.getUser().isOre)
        {
            if (Player.instance.getUser().ingameState)
            {
                Debug.Log("ingamestate");
                frame.color = new Color(1, 1, 1);
                oreImage.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == Player.instance.getUser().oreName).icon);
                oreText.text = Player.instance.getUser().oreName;
                slider.maxValue = Player.instance.getUser().TargetOre.hp;
                slider.value = Player.instance.getUser().orehp;

                CharAni.SetBool("Atk_State", true);
                CharAni.SetTrigger("Atk_Start");

                startButton.SetActive(false);
            }
        }
        else
        {
            if (Player.instance.getUser().equipState)
            {
                Debug.Log("equipstate");
                Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == Player.instance.getUser().equipName).grade);
                frame.color = col;
                oreImage.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == Player.instance.getUser().equipName).icon);
                oreText.text = Player.instance.getUser().equipName;
                slider.maxValue = Player.instance.getUser().equipmaxhp;
                slider.value = Player.instance.getUser().equiphp;

                CharAni.SetBool("Atk_State", true);
                CharAni.SetTrigger("Atk_Start");

                startButton.SetActive(false);
            }

        }
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        while (true)
        {
            //제련이나 제작중일 때 장비 제작 버튼 막기
            if (Player.instance.getUser().ingameState || Player.instance.getUser().equipState)
            {
                if (GameObject.Find("System").transform.Find("EquipItemInfoPopup").gameObject.activeInHierarchy)
                {
                    GameObject.Find("System").transform.Find("EquipItemInfoPopup/UIPanel/ProductionButton").gameObject.SetActive(false);
                }
            }

            //제련
            if (Player.instance.getUser().isOre)
            {
                if (Player.instance.getUser().ingameState)
                {
                    ingameUI.SetActive(true);

                    Player.instance.getUser().oretime -= Time.deltaTime;
                    timeText.text = ((int)Player.instance.getUser().oretime).ToString();
                    slider.value = Player.instance.getUser().orehp;

                    //완성
                    //광석 hp 0
                    if (Player.instance.getUser().orehp <= 0)
                    {
                        Player.instance.getUser().orehp = 0;
                        CharAni.SetBool("complete_win", true);
                        CharAni.SetBool("Atk_State", false);

                        StartCoroutine(success());
                    }
                    //타임오버
                    if (Player.instance.getUser().oretime <= 0)
                    {
                        Player.instance.getUser().oretime = 0;
                        CharAni.speed = 1.0f;
                        CharAni.SetBool("complete_lose", true);
                        CharAni.SetBool("Atk_State", false);

                        StartCoroutine(fail());
                    }
                }
                else
                {
                    ingameUI.SetActive(false);
                }
            }

            //제작
            else
            {
                if (Player.instance.getUser().equipState)
                {
                    ingameUI.SetActive(true);

                    Player.instance.getUser().equiptime -= Time.deltaTime;
                    timeText.text = ((int)Player.instance.getUser().equiptime).ToString();
                    slider.value = Player.instance.getUser().equiphp;

                    //완성
                    //hp 0
                    if (Player.instance.getUser().equiphp <= 0)
                    {
                        Player.instance.getUser().equiphp = 0;
                        CharAni.SetBool("complete_win", true);
                        CharAni.SetBool("Atk_State", false);

                        StartCoroutine(equipsuccess());
                    }
                    //타임오버
                    if (Player.instance.getUser().equiptime <= 0)
                    {
                        Player.instance.getUser().equiptime = 0;
                        CharAni.speed = 1.0f;
                        CharAni.SetBool("complete_lose", true);
                        CharAni.SetBool("Atk_State", false);

                        StartCoroutine(equipfail());
                    }
                }
                else
                {
                    ingameUI.SetActive(false);
                }
            }

            yield return null;
        }


    }


    //성공
    IEnumerator success()
    {
        Achievementhandle.ore_crash_count++;
        GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(Player.instance.getUser().oreName + " 제련에 성공했습니다.");

        //아이템 추가
        if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == (Player.instance.getUser().oreName + "주괴")) != null)
        {
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == (Player.instance.getUser().oreName + "주괴")).possession += 1;
            ThingsData.instance.getInventoryThingsList().Find(x => x.name == (Player.instance.getUser().oreName + "주괴")).recent = true;
        }
        else
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(
                ThingsData.instance.getThingsList().Find(x => x.name == (Player.instance.getUser().oreName + "주괴")).type,
                (Player.instance.getUser().oreName + "주괴"), 1));
        }

        //기타 보상
        int gold = GameObject.Find("PlayerManager").GetComponent<OreSelect>().oreList.Find(x => x.name == Player.instance.getUser().oreName).gold;
        GameObject.Find("PlayerData").GetComponent<Player>().GetMoney("gold", gold);
        GameObject.Find("PlayerData").GetComponent<Player>().getExp(Player.instance.getUser().oreexp);
        Player.instance.getUser().ingameState = false;

        yield return new WaitForSeconds(3.0f);
        startButton.SetActive(true);
        if (GameObject.Find("Menu").transform.Find("TerritoryPopup").gameObject.activeInHierarchy
            || GameObject.Find("Menu").transform.Find("WorldMap").gameObject.activeInHierarchy)
            startButton.SetActive(false);
        ingameUI.SetActive(false);

    }

    //실패
    IEnumerator fail()
    {
        GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(Player.instance.getUser().oreName + " 제련에 실패했습니다.");
        GameObject.Find("PlayerData").GetComponent<Player>().GetMoney("gold", Player.instance.getUser().oreexp / 2);
        GameObject.Find("PlayerData").GetComponent<Player>().getExp(Player.instance.getUser().oreexp / 2);
        Player.instance.getUser().ingameState = false;
        yield return new WaitForSeconds(3.0f);
        startButton.SetActive(true);
        ingameUI.SetActive(false);
    }

    //제작
    //성공
    IEnumerator equipsuccess()
    {
        GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(Player.instance.getUser().oreName + " 제작에 성공했습니다.");

        //아이템 추가
        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(
                ThingsData.instance.getThingsList().Find(x => x.name == Player.instance.getUser().equipName).type,
                Player.instance.getUser().equipName, 1));
      
        GameObject.Find("PlayerData").GetComponent<Player>().GetMoney("gold", Player.instance.getUser().equipexp);
        GameObject.Find("PlayerData").GetComponent<Player>().getExp(Player.instance.getUser().equipexp);
        Debug.Log(Player.instance.getUser().equipexp);
        Player.instance.getUser().equipState = false;
        yield return new WaitForSeconds(3.0f);
        startButton.SetActive(true);
        if (GameObject.Find("Menu").transform.Find("TerritoryPopup").gameObject.activeInHierarchy
            || GameObject.Find("Menu").transform.Find("WorldMap").gameObject.activeInHierarchy)
            startButton.SetActive(false);
        ingameUI.SetActive(false);

    }

    //실패
    IEnumerator equipfail()
    {
        GameObject.Find("PlayerManager").GetComponent<AlertManager>().AcvBoxHandle(Player.instance.getUser().equipName + " 제작에 실패했습니다.");
        GameObject.Find("PlayerData").GetComponent<Player>().GetMoney("gold", Player.instance.getUser().equipexp / 2);
        GameObject.Find("PlayerData").GetComponent<Player>().getExp(Player.instance.getUser().equipexp / 2);
        Player.instance.getUser().equipState = false;
        yield return new WaitForSeconds(3.0f);
        startButton.SetActive(true);
        ingameUI.SetActive(false);
    }

    void atkEffect()
    {
        Debug.Log("atkeff");
        int critical_rate = Random.Range(0, 100);

        if (critical_rate > 30)
        {
            //일반 공격
            int random = Random.Range((int)Player.instance.getUser().stat.strPower + Player.equipHm.power - 10,
                (int)Player.instance.getUser().stat.strPower + Player.equipHm.power + 10);
            if(Player.instance.getUser().isOre)
                Player.instance.getUser().orehp -= random;
            else Player.instance.getUser().equiphp -= random;

            Achievementhandle.normal_atk_count++;
        }
        else if (critical_rate <= 30)
        {
            //크리티컬 공격
            int random = (int)Random.Range(((int)Player.instance.getUser().stat.strPower + Player.equipHm.power - 10) * 1.5f,
                ((int)Player.instance.getUser().stat.strPower + Player.equipHm.power + 10) * 1.5f);
            if (Player.instance.getUser().isOre)
                Player.instance.getUser().orehp -= random;
            else Player.instance.getUser().equiphp -= random;
            Achievementhandle.critical_atk_count++;
        }

    }



    public void immButton()
    {
        //시스템 팝업창 띄우기
        GameObject popup = GameObject.Find("System").transform.Find("ImdCompletePopup").gameObject;
        popup.transform.Find("UIPanel/BackBox/TitleText").GetComponent<Text>().text = "즉시 완료";
       
        popup.transform.Find("UIPanel/InfoText").GetComponent<Text>().text = "골드 " + goldPrice + "개를 사용하여 즉시 완료하시겠습니까?";

        popup.transform.Find("UIPanel/YesButton").GetComponent<Button>().onClick.RemoveAllListeners();      //버튼 리스너 모두 삭제
        popup.transform.Find("UIPanel/YesButton").GetComponent<Button>().onClick.AddListener(() => ImmediatelyComplete());  //버튼 기능 추가

        popup.SetActive(true);
    }

    void ImmediatelyComplete()
    {
        if (Player.instance.getUser().isOre)
            Player.instance.getUser().orehp = 0;
        else Player.instance.getUser().equiphp = 0;

        CharAni.SetBool("complete_win", true);
        CharAni.SetBool("Atk_State", false);
        GameObject.Find("PlayerData").GetComponent<Player>().LostMoney("gold", goldPrice);
        if (Player.instance.getUser().isOre)
            StartCoroutine(success());
        else StartCoroutine(equipsuccess());
    }



}
