using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardHandle : MonoBehaviour
{

    private Text GoldText;
    private Text ExpText;
    private Image RewardIcon;
    private Text RewardText;
    int gold = 0;
    int exp = 0;

    void Awake()
    {
        GoldText = GameObject.Find("Popup").transform.Find("GameReward/UIBack/GoldBox/Text").GetComponent<Text>();
        ExpText = GameObject.Find("Popup").transform.Find("GameReward/UIBack/ExpBox/Text").GetComponent<Text>();
    }


    void Reward_Gold()
    {
        if (Player.instance.getUser().isOre)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", OreSelect.SelectOre.gold, "onUpdate", "GoldCount", "time", 1));

            if (GameObject.Find("Chr_001").GetComponent<CompleteGame>().getwin_lose())
                gold = OreSelect.SelectOre.gold;
            else gold = OreSelect.SelectOre.gold / 2;
        }
        else gold = 0;
        GameObject.Find("PlayerData").GetComponent<Player>().GetMoney("gold", gold);
    }
    void Reward_Exp()
    {
        if (Player.instance.getUser().isOre)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", 0, "to", OreSelect.SelectOre.exp, "onUpdate", "ExpCount", "time", 1));

            if (GameObject.Find("Chr_001").GetComponent<CompleteGame>().getwin_lose())
                exp = OreSelect.SelectOre.exp;
            else exp = OreSelect.SelectOre.exp / 2;
        }
        else exp = Player.instance.getUser().equipexp;
        GameObject.Find("PlayerData").GetComponent<Player>().getExp(exp);
    }
    void GoldCount()
    {
        GoldText.text = gold.ToString();
    }
    void ExpCount()
    {
        ExpText.text = exp.ToString();
    }
}
