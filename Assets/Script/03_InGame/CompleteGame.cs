using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CompleteGame : MonoBehaviour
{
    private GameObject RewardPopup;
    private GameObject ClearFail;
    private GameObject Fail;

    private GameObject defaultItem;
    private GameObject Panel;
    private GameObject[] RewardItems;

    private bool win_lose = true;

    void Awake()
    {
        RewardPopup = GameObject.Find("Popup").transform.Find("GameReward").gameObject;
        ClearFail = GameObject.Find("Popup").transform.Find("ClearFail").gameObject;
        Fail = GameObject.Find("Popup").transform.Find("Fail").gameObject;
        RewardPopup.SetActive(false);
        Panel = RewardPopup.transform.Find("UIBack/Scroll/Panel").gameObject;
        //
        defaultItem = Panel.transform.Find("ItemBox").gameObject;
        defaultItem.SetActive(false);
    }

    void Start()
    {
        //        RewardItems = new GameObject[Random.Range(1, 6)];
        RewardItems = new GameObject[1];
        for (int i = 0; i < RewardItems.Length; i++)
        {
            RewardItems[i] = Instantiate(defaultItem);
            RewardItems[i].transform.SetParent(Panel.transform, false);
            RewardItems[i].SetActive(false);
        }
    }
    public void Complete_Win()
    {
        ClearFail.SetActive(true);
        win_lose = true;
        StartCoroutine(ActiveRewardPopup());
    }
    IEnumerator ActiveRewardPopup()
    {
        if (win_lose)
            yield return new WaitUntil(() => ClearFail.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        else yield return new WaitUntil(() => Fail.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        RewardPopup.SetActive(true);
        ClearFail.SetActive(false);
        Fail.SetActive(false);
        StartCoroutine(OpenRewards());
    }
    public void Complete_Lose()
    {
        Fail.SetActive(true);
        win_lose = false;
        StartCoroutine(ActiveRewardPopup());    //보상 동일

    }

    IEnumerator OpenRewards()
    {
        yield return new WaitForSeconds(0.5f);
        if (win_lose)
        {
            GameObject.Find("UIBack").transform.Find("Win").gameObject.SetActive(true);
            GameObject.Find("UIBack").transform.Find("Lose").gameObject.SetActive(false);
            GameObject.Find("UIBack").transform.Find("failText").gameObject.SetActive(false);
            for (int i = 0; i < RewardItems.Length; i++)
            {
                Things things = new Things();
                if (Player.instance.getUser().isOre)
                {
                    things = ThingsData.instance.getThingsList().Find(x => x.name == (OreSelect.SelectOre.name + "주괴"));
                    //아이템 획득
                    if (ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name) != null)
                    {
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name).possession += 1;
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name).recent = true;
                    }
                    else
                    {
                        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                            x => x.name == things.name).type, things.name, 1));
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name).recent = true;
                    }
                }
                else
                {
                    things = ThingsData.instance.getThingsList().Find(x => x.name == Player.instance.getUser().equipName);
                    //아이템 획득
                        ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(
                            x => x.name == things.name).type, things.name, 1));
                        ThingsData.instance.getInventoryThingsList().Find(x => x.name == things.name).recent = true;
                    Color col = ThingsData.instance.ChangeFrameColor(ThingsData.instance.getThingsList().Find(x => x.name == things.name).grade);
                    RewardItems[i].GetComponent<Image>().color = col;
                }


                RewardItems[i].transform.Find("ItemIcon").gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(things.icon);
                RewardItems[i].transform.Find("Text").gameObject.GetComponent<Text>().text = things.name;
                RewardItems[i].SetActive(true);
                //yield return new WaitUntil(() => RewardItems[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            GameObject.Find("UIBack").transform.Find("Win").gameObject.SetActive(false);
            GameObject.Find("UIBack").transform.Find("Lose").gameObject.SetActive(true);
            GameObject.Find("UIBack").transform.Find("failText").gameObject.SetActive(true);
        }
        RewardPopup.GetComponent<Animator>().SetTrigger("moveNext");
    }


    public bool getwin_lose() { return win_lose; }
}
