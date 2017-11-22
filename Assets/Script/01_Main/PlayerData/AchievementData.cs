using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LitJson;


public class AchievementData : MonoBehaviour {

    public static AchievementData instance = null;

    private JsonData AchvData;
    private WWW reader;
    private static List<CAchievement> AchvList;


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
            AchvList = new List<CAchievement>();

        #region 업적관련 Json데이터 읽어온 후 List 저장
        if (Application.platform == RuntimePlatform.Android)
        {
            string mypath = Path.Combine(Application.streamingAssetsPath, "Achievement.json");
            reader = new WWW(mypath);
            while (!reader.isDone) { }
            AchvData = JsonMapper.ToObject(reader.text);
        }
        else
        {
            string tmp = File.ReadAllText(Application.dataPath + "/StreamingAssets/Achievement.json");
            AchvData = JsonMapper.ToObject(tmp);
        }

        for (int i = 0; i < AchvData["Achievement"].Count; i++)
        {
            int[] Special_reward = new int[AchvData["Achievement"][i]["special_reward"].Count];
            int[] amount_for_SW = new int[AchvData["Achievement"][i]["amount_for_SW"].Count];

            for (int j = 0; j < AchvData["Achievement"][i]["special_reward"].Count; j++)
            {
                Special_reward[j] = (int)AchvData["Achievement"][i]["special_reward"][j];
                amount_for_SW[j] = (int)AchvData["Achievement"][i]["amount_for_SW"][j];
            }

            AchvList.Add(new CAchievement(
                            (int)AchvData["Achievement"][i]["no"],
                            AchvData["Achievement"][i]["type"].ToString(),
                            (int)AchvData["Achievement"][i]["amount"],
                            (int)AchvData["Achievement"][i]["Score"],
                            AchvData["Achievement"][i]["alertText"].ToString(),
                            AchvData["Achievement"][i]["achv_reward_type"].ToString(),
                            (int)AchvData["Achievement"][i]["achv_reward_quantity"],
                            Special_reward,
                            amount_for_SW));
        }
        #endregion
    }

    void Start()
    {




    }



    public List<CAchievement> getAchvList() { return AchvList; }
    public void setAchvList(List<CAchievement> achv) { AchvList = achv; }
}



[Serializable]
public class CAchievement   //업적 클래스
{
    public int no;
    public string type;
    public int amount;
    public int score;
    public string alertText;
    public string achv_reward_type;
    public int achv_reward_quantity;
    public int[] special_reward;
    public int[] amount_for_sw;

    public CAchievement(int no, string type, int amount, int score, string alertText, string achv_reward_type, int achv_reward_quantity, int[] special_reward, int[] amount_for_sw)
    {
        this.no = no;
        this.type = type;
        this.amount = amount;
        this.score = score;
        this.alertText = alertText;
        this.achv_reward_type = achv_reward_type;
        this.achv_reward_quantity = achv_reward_quantity;
        this.special_reward = special_reward;
        this.amount_for_sw = amount_for_sw;
    }
}
