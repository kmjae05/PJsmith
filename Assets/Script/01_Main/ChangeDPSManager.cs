using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDPSManager : MonoBehaviour {


    private Text DPSText;
    private int dps;
    private int amount;
    private int predps;
    private int curdps;

    private Color Imagecol;
    private Color Textcol;
    private Color imgup;
    private Color textup;

    void Start ()
    {
        DPSText = GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image/Text").GetComponent<Text>();
        Imagecol = GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image").GetComponent<Image>().color;
        Textcol = GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image/Text").GetComponent<Text>().color;

        imgup = Imagecol;
        textup = Textcol;
    }
	


    public void changeDPS(int pre, int cur)
    {
        GameObject.Find("System").transform.Find("DPSEff").gameObject.SetActive(true);

        dps = pre;
        predps = pre;
        curdps = cur;
        amount = (int)((cur - pre) / 60f);
        iTween.ValueTo(gameObject, iTween.Hash("from", pre, "to", cur, "onUpdate", "DPSCount", "time", 1));

        StartCoroutine(setactive());
    }

    void DPSCount()
    {
        dps += amount;
        //증가
        if (curdps > predps)
        {
            if (dps > curdps) dps = curdps;
            DPSText.text = "▲" + dps.ToString();
            GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image").GetComponent<Image>().color = imgup;
            GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image/Text").GetComponent<Text>().color = textup;
        }
        //같은 경우
        else if (curdps == predps)
        {
            DPSText.text = "-" + dps.ToString();
            GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image").GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
            GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image/Text").GetComponent<Text>().color = new Color(0.8f, 0.8f, 0.8f);
        }
        //감소
        else if( curdps < predps)
        {
            if (dps < curdps) dps = curdps;
            DPSText.text = "▼" + dps.ToString();
            GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image").GetComponent<Image>().color = new Color(0.7f, 0.5f, 0.5f);
            GameObject.Find("System").transform.Find("DPSEff/UIPanel/Image/Text").GetComponent<Text>().color = new Color(1f, 0.32f, 0.21f);
        }
    }

    IEnumerator setactive()
    {
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("System").transform.Find("DPSEff").gameObject.SetActive(false);

    }
}

