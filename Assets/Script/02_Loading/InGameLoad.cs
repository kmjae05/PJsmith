using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class InGameLoad : MonoBehaviour {

    private Image frame;
    private Image OreIcon;
    private Text OreNameText;
    private Text OreHaveText;
    private Image FadeImage;

    void Awake()
    {
        FadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        frame = GameObject.Find("IconBox").GetComponent<Image>();
        OreIcon = GameObject.Find("OreIcon").GetComponent<Image>();
        OreNameText = GameObject.Find("OreNameText").GetComponent<Text>();
        OreHaveText = GameObject.Find("HaveText").GetComponent<Text>();
    }
    void Start()
    {
        if (Player.instance.getUser().isOre)
        {
            //광석 정보표시, 레어광석 표시
            frame.color = new Color(1, 1, 1);
            OreIcon.sprite = OreSelect.Icon[OreSelect.SelectOre.no].sprite;
            OreNameText.text = OreSelect.SelectOre.name;
            OreHaveText.text = OreSelect.SelectOre.have + "개 남음";
        }
        else
        {
            Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == Player.instance.getUser().equipName).grade);
            frame.color = col;
            OreIcon.sprite = Resources.Load<Sprite>(ThingsData.instance.getThingsList().Find(x => x.name == Player.instance.getUser().equipName).icon);
            OreNameText.text = Player.instance.getUser().equipName;
            OreHaveText.transform.gameObject.SetActive(false);
        }
        StartCoroutine(EnterToInGame());
    }
    IEnumerator FadeIn()
    {
        FadeImage.gameObject.SetActive(true);
        for (float fade = 1.0f; fade >= 0; fade -= 0.02f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        FadeImage.gameObject.SetActive(true);
        for (float fade = 0.0f; fade < 1.0f; fade += 0.02f)
        {
            FadeImage.color = new Color(0, 0, 0, fade);
            yield return null;
        }
    }
    IEnumerator EnterToInGame()
    {
        yield return StartCoroutine(FadeIn());
        yield return StartCoroutine(LoadingTime());
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene("07_Ingame");
    }
    IEnumerator LoadingTime()
    {
        yield return new WaitForSeconds(2.0f);
    }
}
