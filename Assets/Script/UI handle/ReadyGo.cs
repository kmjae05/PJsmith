using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReadyGo : MonoBehaviour {
    public GameObject GameStartCanvas;
    public GameObject ReadyText;
    public GameObject GoText;
    public GameObject SlowArea;
    public GameObject Clicker;
    public float velocity_fast;
    public float velocity_slow;

    static public bool getReady = false;
    static public bool getGo = false;
	// Use this for initialization
	void Start () {
        StartCoroutine(AnimateReadyText());
        StartCoroutine(AnimateGoText());
	}

    IEnumerator AnimateReadyText()  //준비 텍스트 애니메이션
    {
        while (true)
        {
            yield return new WaitUntil(() => getReady);
            Clicker.SetActive(false);
            ReadyText.SetActive(true);
            ReadyText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-ReadyText.GetComponent<RectTransform>().rect.width / 2, 0);

            while (true)
            {
                if (ReadyText.GetComponent<RectTransform>().localPosition.x + ReadyText.GetComponent<RectTransform>().rect.width / 5 >= SlowArea.GetComponent<RectTransform>().localPosition.x - SlowArea.GetComponent<RectTransform>().rect.width / 2
                    && ReadyText.GetComponent<RectTransform>().localPosition.x - ReadyText.GetComponent<RectTransform>().rect.width / 6 < SlowArea.GetComponent<RectTransform>().localPosition.x + SlowArea.GetComponent<RectTransform>().rect.width / 2)
                {
                    ReadyText.GetComponent<RectTransform>().anchoredPosition += new Vector2(velocity_slow, 0);
                }
                else
                {
                    ReadyText.GetComponent<RectTransform>().anchoredPosition += new Vector2(velocity_fast, 0);
                }

                if (ReadyText.GetComponent<RectTransform>().anchoredPosition.x >= 3000)
                {
                    break;
                }
                yield return null;
            }
            ReadyText.SetActive(false);
            getReady = false;
            getGo = true;
        }
    }
    IEnumerator AnimateGoText()     //시작 텍스트 애니메이션
    {
        while (true)
        {
            yield return new WaitUntil(() => getGo);
            GoText.SetActive(true);
            GoText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-ReadyText.GetComponent<RectTransform>().rect.width / 2, 0);

            while (true)
            {
                if (GoText.GetComponent<RectTransform>().localPosition.x + GoText.GetComponent<RectTransform>().rect.width / 5 >= SlowArea.GetComponent<RectTransform>().localPosition.x - SlowArea.GetComponent<RectTransform>().rect.width / 2
                    && GoText.GetComponent<RectTransform>().localPosition.x - GoText.GetComponent<RectTransform>().rect.width / 6 < SlowArea.GetComponent<RectTransform>().localPosition.x + SlowArea.GetComponent<RectTransform>().rect.width / 2)
                {
                    GoText.GetComponent<RectTransform>().anchoredPosition += new Vector2(velocity_slow, 0);
                }
                else
                {
                    GoText.GetComponent<RectTransform>().anchoredPosition += new Vector2(velocity_fast, 0);
                }

                if (GoText.GetComponent<RectTransform>().anchoredPosition.x >= 3000)
                {
                    break;
                }
                yield return null;
            }
            GoText.SetActive(false);
            getGo = false;
            Clicker.SetActive(true);
        }
    }
}
