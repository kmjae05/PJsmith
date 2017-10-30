using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeHandle : MonoBehaviour {
    private Image FadeImage;

    void Awake()
    {
        FadeImage = gameObject.transform.Find("FadeImage").GetComponent<Image>();
        FadeImage.gameObject.SetActive(false);
    }
    //void OnLevelWasLoaded()
    //{
    //    StartCoroutine(FadeIn());
    //}
    IEnumerator FadeIn()
    {
        FadeImage.gameObject.SetActive(true);
        for (float fade = 1.0f; fade >= 0; fade -= 0.02f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        FadeImage.gameObject.SetActive(false);
    }
}
