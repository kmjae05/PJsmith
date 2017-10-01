using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DimensionScript : MonoBehaviour {

    //차원이동 오브젝트
    public GameObject dimentionShifting;

    //페이드아웃
    private Image FadeImage;
    private GameObject FadeImageObject;

    private void Awake()
    {
        FadeImageObject = GameObject.Find("FadeCanvas");
        FadeImage = FadeImageObject.transform.Find("FadeImage").gameObject.GetComponent<Image>();
    }

    //차원이동
    IEnumerator dimensionFadeOut()
    {
        dimentionShifting.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        //페이드아웃
        FadeImageObject.transform.Find("FadeImage").gameObject.SetActive(true);
        for (float fade = 0.0f; fade < 1.0f; fade += 0.05f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        yield return new WaitForSeconds(0.7f);

        SceneManager.LoadScene("12_Combat");
    }


    public void ToCombat()
    {
        StartCoroutine(dimensionFadeOut());
    }


}
