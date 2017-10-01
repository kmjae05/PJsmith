using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class Logo : MonoBehaviour {
	// Use this for initialization
    public GameObject img;
	IEnumerator Start () {
        img.GetComponent<Image>().color = new Vector4(1, 1, 1, 0);
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(FadeOut());
        yield return StartCoroutine(nextScene());
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2.0f);
        for (float i = 1f; i >= 0f; i -= 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            img.GetComponent<Image>().color = color;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator FadeIn()
    {
        for (float i = 0f; i <= 1f; i += 0.02f)
        {
            Color color = new Vector4(1, 1, 1, i);
            img.GetComponent<Image>().color = color;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator nextScene()
    {
        var load = SceneManager.LoadSceneAsync("LoadingScene");
        while (!load.isDone) { 
            yield return null;
        }
    }
}
