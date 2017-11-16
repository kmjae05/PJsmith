using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour {

    private GameObject QuestPopup;

    private GameObject scroll;

    private GameObject panel1;
    private GameObject panel2;



    private GameObject questBox;    //복사할 객체
    private Image questBoxFrame;
    private Image questBoxIcon;
    private Text questBoxTitle;     //퀘스트 제목
    private Text questBoxComments;  //퀘스트 설명
    private Slider questBoxSlider;
    private Text questBoxSliderText;
    private Image questBoxRewardFrame;
    private Image questBoxRewardIcon;
    private Text questBoxRewardText;
    private GameObject questBoxRewardButtonImage;       //setActive
    private Image questBoxRewardButtonFrame;
    private Image questBoxRewardButtonIcon;
    private Text questBoxRewardButtonText;
    private Button questBoxRewardButton;        //보상 받기 버튼
    private GameObject questBoxCompleteImage;       //완료 이미지




    private void Start()
    {
        QuestPopup = GameObject.Find("Menu").transform.Find("QuestPopup").gameObject;

        scroll = QuestPopup.transform.Find("UIPanel/Scroll").gameObject;
        panel1 = scroll.transform.Find("Panel_1").gameObject;
        panel2 = scroll.transform.Find("Panel_2").gameObject;





        scroll.GetComponent<ScrollRect>().content = panel1.GetComponent<RectTransform>();

        GameObject.Find("BrazierButton").transform.Find("BrazierMiniButton/QuestButton").gameObject.GetComponent<Button>().onClick.AddListener(() => {
            Debug.Log("dfa");
            panel1.SetActive(true); SwitchScrollPanel(1);
        });
    }



    public void SwitchScrollPanel(int i)
    {
        if (i == 1)
            scroll.GetComponent<ScrollRect>().content = panel1.GetComponent<RectTransform>();
        else if (i == 2)
            scroll.GetComponent<ScrollRect>().content = panel2.GetComponent<RectTransform>();
    }

}
