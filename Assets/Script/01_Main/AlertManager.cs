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

    public void AcvBoxHandle(string text)   //업적 달성 시 알림 UI 애니메이션
    {
        Debug.Log("al");     
        
        AcvBox.SetActive(false);
        AcvText.text = text;

        StartCoroutine(alrImageActive());
    }
    IEnumerator alrImageActive()
    {
        AcvBox.SetActive(true);
        yield return new WaitForSeconds(3.0f);
    }
}
