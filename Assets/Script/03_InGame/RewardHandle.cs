using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardHandle : MonoBehaviour
{

    private Text GoldText;
    private Text ExpText;
    private Image RewardIcon;
    private Text RewardText;

    void Awake()
    {
        GoldText = GameObject.Find("Popup").transform.Find("GameReward/UIBack/GoldBox/Text").GetComponent<Text>();
        ExpText = GameObject.Find("Popup").transform.Find("GameReward/UIBack/ExpBox/Text").GetComponent<Text>();
    }


    //void Reward_Gold()
    //{
    //    iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", OreSelect.SelectOre.gold, "onUpdate", "GoldCount", "time", 1));
    //}
    //void Reward_Exp()
    //{
    //    iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", OreSelect.SelectOre.exp, "onUpdate", "ExpCount", "time", 1));
    //}
    //void GoldCount(int num)
    //{
    //    GoldText.text = num.ToString();
    //}
    //void ExpCount(int num)
    //{
    //    ExpText.text = num.ToString();
    //}
}
