using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class NormalLoad : MonoBehaviour
{
    private Image FadeImage;
    private GameObject FadeImageObject;


    void Awake ()
    {
        FadeImageObject = GameObject.Find("FadeImage");
        FadeImage = FadeImageObject.GetComponent<Image>();

        
        //Lobby 화면일 경우 차원이동 효과 오브젝트 불러오기
        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //    dimentionShifting = GameObject.Find("dimentionShifting").GetComponent<Image>();
    }
    void Start () 
    {
        StartCoroutine(EnterToMain());
        Time.timeScale = 1.0f;
	}
    IEnumerator FadeIn()
    {
        FadeImageObject.SetActive(true);
        for (float fade = 1.0f; fade >= 0; fade -= 0.05f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        FadeImageObject.SetActive(false);
    }
    IEnumerator FadeOut()
    {
        FadeImageObject.SetActive(true);
        for (float fade = 0.0f; fade < 1.0f; fade += 0.05f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        yield return new WaitForSeconds(0.8f);
    }


    IEnumerator EnterToMain()
    {
        yield return StartCoroutine(FadeIn());        //페이드인
        //yield return StartCoroutine(LoadingTime()); //로딩시간
        //yield return StartCoroutine(FadeOut());     //페이드아웃

         //Logo 화면일 경우 일정 시간 후 Title 화면으로.
        if (SceneManager.GetActiveScene().name == "00_Logo")
        {
            //GameObject.Find("Box").transform.Find("LogoImage").gameObject.SetActive(false);
            yield return StartCoroutine(LoadingTime()); //로딩시간
            //animation
            //GameObject.Find("Box").transform.Find("LogoImage").gameObject.SetActive(true);
            //yield return new WaitUntil(() => GameObject.Find("Box").transform.Find("LogoImage").gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            yield return StartCoroutine(FadeOut());     //페이드아웃
            SceneManager.LoadScene("01_Title");
        }
        //if (SceneManager.GetActiveScene().name == "01_Title")
        //{
        //    yield return StartCoroutine(LoadingTime()); //로딩시간
        //    ToLobby();
        //}

            //로딩화면
            if ( SceneManager.GetActiveScene().name == "08_Loading_GameIn"
            || SceneManager.GetActiveScene().name == "09_Loading_Normal")
        {
            yield return StartCoroutine(LoadingTime()); //로딩시간
            yield return StartCoroutine(FadeOut());     //페이드아웃
            SceneManager.LoadScene("02_Lobby");
        }

    }
    IEnumerator LoadingTime()
    {
        yield return new WaitForSeconds(1.5f);
    }


    public void ToLobby()
    {
        StartCoroutine(LobbyFadeOut());
    }
    IEnumerator LobbyFadeOut()
    {
        FadeImageObject.SetActive(true);
        for (float fade = 0.0f; fade < 1.0f; fade += 0.02f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("02_Lobby");

    }

    public void ToCollection()
    {
        SceneManager.LoadScene("11_Collection");
    }

    public void ToLoading_GameIn()
    {
        StartCoroutine(FadeOut());
        SceneManager.LoadScene("08_Loading_GameIn");
    }
    public void ToLoading_Normal()
    {
        StartCoroutine(FadeOut());
        SceneManager.LoadScene("09_Loading_Normal");
    }

    public void combatToLobby()
    {
        DontDestroyScript.instance.setCombatFlag(true);
    }


}
