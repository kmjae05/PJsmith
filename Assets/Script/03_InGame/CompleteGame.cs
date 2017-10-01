using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompleteGame : MonoBehaviour {
    private GameObject RewardPopup;
    private GameObject ClearFail;

    private GameObject defaultItem;
    private GameObject Panel;
    private GameObject[] RewardItems;

    void Awake()
    {
        RewardPopup = GameObject.Find("Popup").transform.Find("GameReward").gameObject;
        ClearFail = GameObject.Find("Popup").transform.Find("ClearFail").gameObject;
        RewardPopup.SetActive(false);
        Panel = RewardPopup.transform.Find("UIBack/Scroll/Panel").gameObject;
        //
        //defaultItem = Panel.transform.Find("ItemBox").gameObject;
        //defaultItem.SetActive(false);
    }

    void Start()
    {   
        RewardItems = new GameObject[Random.Range(1, 6)];
        for (int i = 0; i < RewardItems.Length; i++)
        {
            //RewardItems[i] = Instantiate(defaultItem);
            //RewardItems[i].transform.SetParent(Panel.transform, false);
            //RewardItems[i].SetActive(false);
        }
    }
    public void Complete_Win()
    {
        ClearFail.SetActive(true);
        StartCoroutine(ActiveRewardPopup());
    }
    IEnumerator ActiveRewardPopup()
    {
        yield return new WaitUntil(()=>ClearFail.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        RewardPopup.SetActive(true);
        ClearFail.SetActive(false);
        StartCoroutine(OpenRewards());
    }
    public void Complete_Lose()
    {

    }

    IEnumerator OpenRewards()
    {
        yield return new WaitForSeconds(0.5f);
        //for (int i = 0; i < RewardItems.Length; i++)
        //{
        //    RewardItems[i].SetActive(true);
        //    yield return new WaitUntil(() => RewardItems[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        //    yield return new WaitForSeconds(0.1f);
        //}
        RewardPopup.GetComponent<Animator>().SetTrigger("moveNext");
    }
    
}
