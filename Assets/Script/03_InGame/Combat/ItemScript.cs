using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    private Camera worldCam;
    private Camera uiCam;
    private Transform player;

    private GameObject[] ItemCooldowntime;
    private float time = 10.0f;

    //포션
    private GameObject PotionImage;

    //거대화 아이템 사용
    private bool hugeFlag = false;
    //반투명
    public GameObject[] transparent;
    public GameObject trans;
    private float hugeTime = 0f;

    private GameObject chr;

    private void Awake()
    {
        worldCam = GameObject.Find("Main_Camera").GetComponent<Camera>();
        uiCam = GameObject.Find("HPBarCamera").GetComponent<Camera>();
        player = GameObject.Find("Chr").transform;

        ItemCooldowntime = new GameObject[3];
        for (int i = 0; i < ItemCooldowntime.Length; i++)
            ItemCooldowntime[i] = GameObject.Find("ItemButton" + (i + 1).ToString()).transform.Find("ItemCooldowntime" + (i + 1).ToString()).gameObject;

        PotionImage = GameObject.Find("Slider").transform.Find("PotionImage").gameObject;
        chr = GameObject.Find("Chr");
    }

    void Update()
    {
        //for (int i = 0; i < transparent.Length; i++)
        //{
        //    transparent[i].transform.position = chr.transform.position + new Vector3(1f, 0f, 0f);
        //}
        trans.transform.position = chr.transform.position;
    }

    //아이템 전체 쿨타임 //애니메이션 수정
    public void useItem()
    {
        for (int i = 0; i < ItemCooldowntime.Length; i++)
            ItemCooldowntime[i].SetActive(true);
        StartCoroutine(disappear());
    }

    IEnumerator disappear()
    {
        yield return new WaitForSeconds(time);
        GameObject.Find("Effect").transform.Find("3D_Recovery_03").gameObject.SetActive(false);
        for (int i = 0; i < ItemCooldowntime.Length; i++)
            ItemCooldowntime[i].SetActive(false);
        //hugeFlag = false;
    }

    //빨간 물약 사용
    public void useRedpotion()
    {
        StartCoroutine(setPotionImage());

        GameObject.Find("Effect").transform.Find("3D_Recovery_03").gameObject.SetActive(true);
        StartCoroutine(GameObject.Find("HPScript").GetComponent<DamageImageScript>().healImageUp(GameObject.Find("Chr").transform, 50));
        GameObject.Find("Chr").GetComponent<PlayerController>().heal(50);
    }

    //거대화
    public void useHugeItem()
    {
        hugeFlag = true;
        StartCoroutine(huge());
    }

    IEnumerator huge()
    {
        for (int i = 0; i < transparent.Length; i++)
            transparent[i].SetActive(true);

        while (true)
        {
            if (chr.transform.localScale.x != 1.5f)
            {
                hugeTime += Time.deltaTime * 1.0f;
                chr.transform.localScale = new Vector3(Mathf.Lerp(1.0f, 1.5f, hugeTime), Mathf.Lerp(1.0f, 1.5f, hugeTime), Mathf.Lerp(1.0f, 1.5f, hugeTime));
                yield return null;
            }
            else
            {
                for (int i = 0; i < transparent.Length; i++)
                    transparent[i].SetActive(false);
                hugeTime = 0f;
                break;
            }
        }
        hugeFlag = false;
        yield return new WaitForSeconds(3.0f);

        while (true)
        {
            if (chr.transform.localScale.x != 1.0f)
            {
                hugeTime += Time.deltaTime * 1.0f;
                chr.transform.localScale = new Vector3(Mathf.Lerp(1.5f, 1.0f, hugeTime), Mathf.Lerp(1.5f, 1.0f, hugeTime), Mathf.Lerp(1.5f, 1.2f, hugeTime));
                yield return null;
            }
            else
            {
                hugeTime = 0f;
                break;
            }
        }
    }

    IEnumerator setPotionImage()
    {
        PotionImage.SetActive(true);
        StartCoroutine(setPosition());
        yield return new WaitForSeconds(1.0f);
        PotionImage.SetActive(false);
    }
    IEnumerator setPosition()
    {
        while (true)
        {
            //위치 조절
            Vector3 position = worldCam.WorldToViewportPoint(player.position);
            position.x += 0.28f;
            position.y += 0.12f;
            //해당 좌표를 uiCamera의 World좌표로 변경. 
            PotionImage.transform.position = uiCam.ViewportToWorldPoint(position);
            //값 정리. 
            position = PotionImage.transform.localPosition;
            position.x = Mathf.RoundToInt(position.x);
            position.y = Mathf.RoundToInt(position.y);
            position.z = 0.0f;
            PotionImage.transform.localPosition = position;


            yield return null;
        }
    }


    public bool getHugeFlag()
    {
        return hugeFlag;
    }
}
