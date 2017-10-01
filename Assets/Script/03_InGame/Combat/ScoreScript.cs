using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    private Text scoreText;
    private Text expText;
    private Text goldText;

    private char[] scoreChar;
    private char[] expScoreChar;
    private char[] goldScoreChar;

    float preScore = 0f;
    float nowScore = 142873;
    float exp = 120;
    float gold = 450;

    //flag
    bool scoreFlag = false;
    bool expFlag = false;
    bool goldFlag = false;

    float[] time = new float[3];

    int score = 0;
    int expScore = 0;
    int goldScore = 0;

    private void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        expText = GameObject.Find("getExpText").GetComponent<Text>();
        goldText = GameObject.Find("getGoldText").GetComponent<Text>();

        scoreChar = new char[6];
        expScoreChar = new char[6];
        goldScoreChar = new char[6];
    }

    private void Update()
    {
        if (scoreFlag)
        {
            if (score != nowScore)
            {
                time[0] += Time.deltaTime * 0.8f;

                score = (int)Mathf.Lerp(preScore, nowScore, time[0]);

                scoreText.text = string.Format("{0:#,###}", score);
            }
        }
        if (expFlag)
        {
            if (expScore != exp)
            {
                time[1] += Time.deltaTime * 0.8f;
                expScore = (int)Mathf.Lerp(preScore, exp, time[1]);
                expText.text = string.Format("{0:#,###}", expScore);
            }
        }
        if (goldFlag)
        {
            if (goldScore != gold)
            {
                time[2] += Time.deltaTime * 0.8f;
                goldScore = (int)Mathf.Lerp(preScore, gold, time[2]);
                goldText.text = string.Format("{0:#,###}", goldScore);
            }
        }


    }

    public void setScore()
    {
        scoreFlag = true;
    }
    public void setExpScore()
    {
        expFlag = true;
    }
    public void setGoldScore()
    {
        goldFlag = true;
    }









}
