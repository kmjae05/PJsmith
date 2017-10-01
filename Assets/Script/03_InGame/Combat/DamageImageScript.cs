using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamageImageScript : MonoBehaviour
{
    private Camera worldCam;
    private Camera uiCam;


    private GameObject ObjCanvas;
    private GameObject damageImage;
    //대미지 쿨타임 flag
    private bool damagedFlag = false;
    //대미지 이미지
    private GameObject[] number;
    private GameObject minus;
    private GameObject[] number_Red;
    private GameObject minus_Red;
    private GameObject[] number_Green;


    private GameObject critical;

    private PlayerController player;


    void Start ()
    {
        worldCam = GameObject.Find("Main_Camera").GetComponent<Camera>();
        uiCam = GameObject.Find("HPBarCamera").GetComponent<Camera>();

        ObjCanvas = GameObject.Find("DamageObj");
        damageImage = ObjCanvas.transform.Find("DamageImage").gameObject;

        number = new GameObject[10];
        for (int i = 0; i < number.Length; i++)
            number[i] = ObjCanvas.transform.Find("number").transform.Find("damage" + i).gameObject;
        minus = ObjCanvas.transform.Find("number").transform.Find("damageM").gameObject;
        number_Red = new GameObject[10];
        for (int i = 0; i < number_Red.Length; i++)
            number_Red[i] = ObjCanvas.transform.Find("number_Red").transform.Find("damage" + i).gameObject;
        minus_Red = ObjCanvas.transform.Find("number_Red").transform.Find("damageM").gameObject;
        number_Green = new GameObject[10];
        for (int i = 0; i < number_Green.Length; i++)
            number_Green[i] = ObjCanvas.transform.Find("number_Green").transform.Find("damage" + i).gameObject;

        critical = ObjCanvas.transform.Find("DamageImage").transform.Find("Critical").gameObject;

        player = GameObject.Find("Chr").GetComponent<PlayerController>();
    }



    //올라가는 이미지
    public IEnumerator damageImageUp(Transform target, int dam, bool cri)
    {

        #region 숫자 하나하나 이미지

        //자릿수 (일의 자리부터 1000자리까지)
        int[] positional = new int[4];
        positional[0] = dam % 10;
        positional[1] = dam / 10; if (positional[1] >= 10) positional[1] %= 10;
        positional[2] = dam / 100; if (positional[2] >= 10) positional[2] %= 10;
        positional[3] = dam / 1000; //if (positional[3] >= 10) positional[3] = 0;

        //이미지 선택
        GameObject[] damageNumImg = new GameObject[4];
        //1000자리
        if (positional[3] != 0)
        {
            damageNumImg[0] = number[positional[3]];
            damageNumImg[1] = number[positional[2]];
            damageNumImg[2] = number[positional[1]];
            damageNumImg[3] = number[positional[0]];
        }
        //100자리
        else if (positional[2] != 0)
        {
            damageNumImg[0] = number[positional[2]];
            damageNumImg[1] = number[positional[1]];
            damageNumImg[2] = number[positional[0]];
        }
        else if (positional[1] != 0)
        {
            damageNumImg[0] = number[positional[1]];
            damageNumImg[1] = number[positional[0]];
        }
        else if (positional[0] != 0)
        {
            damageNumImg[0] = number[positional[0]];
        }
        //이미지 생성
        GameObject minusImg = Instantiate(minus);
        minusImg.transform.SetParent(damageImage.transform, false);
        minusImg.SetActive(true);
        GameObject[] dImg = new GameObject[damageNumImg.Length];
        for (int i = 0; i < damageNumImg.Length; i++)
        {
            if (damageNumImg[i])
            {
                dImg[i] = Instantiate(damageNumImg[i]);
                dImg[i].transform.SetParent(damageImage.transform, false);
                dImg[i].SetActive(true);
                dImg[i].transform.Translate(1.0f+i, 0, 0);
            }
        }
        //크리티컬 이미지
        if (cri)    critical.SetActive(true);
        else        critical.SetActive(false);

        GameObject damage = damageImage;
        GameObject dmg = Instantiate(damage);
        dmg.transform.SetParent(ObjCanvas.transform, false);
        dmg.SetActive(true);
        RectTransform dmgPosition = dmg.GetComponent<RectTransform>();

        Destroy(minusImg);
        for (int i = 0; i < dImg.Length; i++)
            Destroy(dImg[i]);


        //포지션 조절
        Vector3 position = worldCam.WorldToViewportPoint(target.position);
        position.x += 0.05f;
        position.y += 0.1f;
        dmg.transform.position = uiCam.ViewportToWorldPoint(position);
        position = dmg.transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = 0.0f;
        dmg.transform.localPosition = position;

        #endregion

        float vel = 20;
        float acc = -1;
        float fade = 1.0f;
        float fadeRate = 0.02f;

        while (300 >= dmgPosition.anchoredPosition.y)
        {
            dmgPosition.anchoredPosition += new Vector2(0, vel);
            vel -= acc;
            yield return null;
        }

        while (fade > 0)
        {
            dmgPosition.anchoredPosition += new Vector2(0, fade);
            fade -= fadeRate;
            yield return null;
        }
        Destroy(dmg);
        yield return new WaitForSeconds(1.0f);
        damagedFlag = false;

    }


    //내려가는 이미지(플레이어 피격)
    public IEnumerator damageImageDown(Transform target, int dam, bool cri)
    {

        player.setDamagedFlag(true);

        yield return new WaitForSeconds(0.5f);


        #region 숫자 하나하나 이미지

        //자릿수 (일의 자리부터 1000자리까지)
        int[] positional = new int[4];
        positional[0] = dam % 10;
        positional[1] = dam / 10; if (positional[1] >= 10) positional[1] %= 10;
        positional[2] = dam / 100; if (positional[2] >= 10) positional[2] %= 10;
        positional[3] = dam / 1000; //if (positional[3] >= 10) positional[3] = 0;

        //이미지 선택
        GameObject[] damageNumImg = new GameObject[4];
        //1000자리
        if (positional[3] != 0)
        {
            damageNumImg[0] = number_Red[positional[3]];
            damageNumImg[1] = number_Red[positional[2]];
            damageNumImg[2] = number_Red[positional[1]];
            damageNumImg[3] = number_Red[positional[0]];
        }
        //100자리
        else if (positional[2] != 0)
        {
            damageNumImg[0] = number_Red[positional[2]];
            damageNumImg[1] = number_Red[positional[1]];
            damageNumImg[2] = number_Red[positional[0]];
        }
        else if (positional[1] != 0)
        {
            damageNumImg[0] = number_Red[positional[1]];
            damageNumImg[1] = number_Red[positional[0]];
        }
        else if (positional[0] != 0)
        {
            damageNumImg[0] = number_Red[positional[0]];
        }
        //이미지 생성
        GameObject minusImg = Instantiate(minus_Red);
        minusImg.transform.SetParent(damageImage.transform, false);
        minusImg.SetActive(true);
        GameObject[] dImg = new GameObject[damageNumImg.Length];
        for (int i = 0; i < damageNumImg.Length; i++)
        {
            if (damageNumImg[i])
            {
                dImg[i] = Instantiate(damageNumImg[i]);
                dImg[i].transform.SetParent(damageImage.transform, false);
                dImg[i].SetActive(true);
                dImg[i].transform.Translate(1.0f + i, 0, 0);
            }
        }
        //크리티컬 이미지
        if (cri) critical.SetActive(true);
        else critical.SetActive(false);

        GameObject damage = damageImage;
        GameObject dmg = Instantiate(damage);
        dmg.transform.SetParent(ObjCanvas.transform, false);
        dmg.SetActive(true);
        RectTransform dmgPosition = dmg.GetComponent<RectTransform>();

        Destroy(minusImg);
        for (int i = 0; i < dImg.Length; i++)
            Destroy(dImg[i]);


        //포지션 조절
        Vector3 position = worldCam.WorldToViewportPoint(target.position);
        position.x -= 0.1f;
        position.y += 0.1f;
        dmg.transform.position = uiCam.ViewportToWorldPoint(position);
        position = dmg.transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = 0.0f;
        dmg.transform.localPosition = position;

        #endregion


        float vel = 20;
        float acc = -1;
        float fade = 1.0f;
        float fadeRate = 0.02f;

        while (-200 <= dmgPosition.anchoredPosition.y)
        {
            dmgPosition.anchoredPosition -= new Vector2(0, vel);
            vel += acc;
            yield return null;
        }

        while (fade > 0)
        {
            dmgPosition.anchoredPosition -= new Vector2(0, fade);
            fade -= fadeRate;
            yield return null;
        }
        Destroy(dmg);
        yield return new WaitForSeconds(1.0f);
        player.setDamagedFlag(false);

    }

    //올라가는 이미지 (회복)
    public IEnumerator healImageUp(Transform target, int dam)
    {

        #region 숫자 하나하나 이미지

        //자릿수 (일의 자리부터 1000자리까지)
        int[] positional = new int[4];
        positional[0] = dam % 10;
        positional[1] = dam / 10; if (positional[1] >= 10) positional[1] %= 10;
        positional[2] = dam / 100; if (positional[2] >= 10) positional[2] %= 10;
        positional[3] = dam / 1000; //if (positional[3] >= 10) positional[3] = 0;

        //이미지 선택
        GameObject[] damageNumImg = new GameObject[4];
        //1000자리
        if (positional[3] != 0)
        {
            damageNumImg[0] = number_Green[positional[3]];
            damageNumImg[1] = number_Green[positional[2]];
            damageNumImg[2] = number_Green[positional[1]];
            damageNumImg[3] = number_Green[positional[0]];
        }
        //100자리
        else if (positional[2] != 0)
        {
            damageNumImg[0] = number_Green[positional[2]];
            damageNumImg[1] = number_Green[positional[1]];
            damageNumImg[2] = number_Green[positional[0]];
        }
        else if (positional[1] != 0)
        {
            damageNumImg[0] = number_Green[positional[1]];
            damageNumImg[1] = number_Green[positional[0]];
        }
        else if (positional[0] != 0)
        {
            damageNumImg[0] = number_Green[positional[0]];
        }
        //이미지 생성
        GameObject[] dImg = new GameObject[damageNumImg.Length];
        for (int i = 0; i < damageNumImg.Length; i++)
        {
            if (damageNumImg[i])
            {
                dImg[i] = Instantiate(damageNumImg[i]);
                dImg[i].transform.SetParent(damageImage.transform, false);
                dImg[i].SetActive(true);
                dImg[i].transform.Translate(1.0f + i, 0, 0);
            }
        }
        critical.SetActive(false);
        
        GameObject damage = damageImage;
        GameObject dmg = Instantiate(damage);
        dmg.transform.SetParent(ObjCanvas.transform, false);
        dmg.SetActive(true);
        RectTransform dmgPosition = dmg.GetComponent<RectTransform>();

        for (int i = 0; i < dImg.Length; i++)
            Destroy(dImg[i]);


        //포지션 조절
        Vector3 position = worldCam.WorldToViewportPoint(target.position);
        position.x -= 0.1f;
        position.y += 0.15f;
        dmg.transform.position = uiCam.ViewportToWorldPoint(position);
        position = dmg.transform.localPosition;
        position.x = Mathf.RoundToInt(position.x);
        position.y = Mathf.RoundToInt(position.y);
        position.z = 0.0f;
        dmg.transform.localPosition = position;

        #endregion

        float vel = 20;
        float acc = -1;
        float fade = 1.0f;
        float fadeRate = 0.02f;

        while (200 >= dmgPosition.anchoredPosition.y)
        {
            dmgPosition.anchoredPosition += new Vector2(0, vel);
            vel -= acc;
            yield return null;
        }

        while (fade > 0)
        {
            dmgPosition.anchoredPosition += new Vector2(0, fade);
            fade -= fadeRate;
            yield return null;
        }
        Destroy(dmg);
        yield return new WaitForSeconds(1.0f);
        damagedFlag = false;

    }




}
