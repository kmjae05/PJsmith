using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour {

    private bool popUp = false;

    private GameObject AcvBox;
    private Text AcvText;


    private void Start()
    {
        AcvBox = GameObject.Find("System").transform.Find("AlertPopup").gameObject;
        AcvText = AcvBox.transform.Find("UIPanel/AchievementText").gameObject.GetComponent<Text>();

    }

    public IEnumerator AcvBoxHandle(string text)   //업적 달성 시 알림 UI 애니메이션
    {
        Debug.Log("al");        
        AcvBox.SetActive(false);
        GameObject AcvBoxPosition = AcvBox.transform.Find("UIPanel").gameObject;
        iTween.MoveTo(AcvBox, iTween.Hash("y", 75, "time", 0.1, "isLocal", true));
        iTween.MoveTo(AcvBox, iTween.Hash("y", -75, "time", 0.5, "delay", 0.5, "isLocal", true));

        AcvText.text = text;
        AcvBox.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        iTween.MoveTo(AcvBox, iTween.Hash("y", 75, "time", 0.5f, "isLocal", true, "oncomplete", "CloseAcvBox"));
        AcvBox.SetActive(false);
    }
    void CloseAcvBox()
    {
        AcvBox.SetActive(false);
    }
}
