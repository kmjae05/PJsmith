using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ComboImageScript : MonoBehaviour {

    private GameObject ObjCanvas;
    private GameObject comboImage;

    private GameObject[] number;

    private GameObject comboObj;
    private GameObject com;

    private Combat combat;


    void Start()
    {
        ObjCanvas = GameObject.Find("ComboObj");
        comboImage = ObjCanvas.transform.Find("ComboImage").gameObject;

        number = new GameObject[10];
        for (int i = 0; i < number.Length; i++)
            number[i] = ObjCanvas.transform.Find("number").transform.Find("combo" + i).gameObject;

        comboObj = ObjCanvas.transform.Find("ComboImage").transform.Find("Combo").gameObject;

        combat = GameObject.Find("Chr_001_").GetComponent<Combat>();
    }

    private void Update()
    {
        if (combat.getCombo() == 0)
            Destroy(GameObject.Find("ComboImage(Clone)"));
    }

    public void setCombo()
    {
        //이전 이미지 삭제
        Destroy(GameObject.Find("ComboImage(Clone)"));

        int combo = combat.getCombo();
        int[] positional = new int[3];
        positional[0] = combo % 10;
        positional[1] = combo / 10; if (positional[1] >= 10) positional[1] %= 10;
        positional[2] = combo / 100; if (positional[2] >= 10) positional[2] %= 10;

        //이미지 선택
        GameObject[] ComboNumImg = new GameObject[4];
        //100자리
        if (positional[2] != 0)
        {
            ComboNumImg[0] = number[positional[0]];
            ComboNumImg[1] = number[positional[1]];
            ComboNumImg[2] = number[positional[2]];
        }
        else if (positional[1] != 0)
        {
            ComboNumImg[0] = number[positional[0]];
            ComboNumImg[1] = number[positional[1]];
        }
        else if (positional[0] != 0)
        {
            ComboNumImg[0] = number[positional[0]];
        }

        //숫자 이미지 생성
        GameObject[] cImg = new GameObject[ComboNumImg.Length];
        for (int i = 0; i < ComboNumImg.Length; i++)
        {
            if (ComboNumImg[i])
            {
                cImg[i] = Instantiate(ComboNumImg[i]);
                cImg[i].transform.SetParent(comboImage.transform, false);
                cImg[i].SetActive(true);
                cImg[i].transform.Translate(-3 - (i+0.8f), 0f, 0);
            }
        }

        GameObject damage = comboImage;
        com = Instantiate(damage);
        com.transform.SetParent(ObjCanvas.transform, false);
        com.SetActive(true);

        for (int i = 0; i < cImg.Length; i++)
            Destroy(cImg[i]);


    }




}
