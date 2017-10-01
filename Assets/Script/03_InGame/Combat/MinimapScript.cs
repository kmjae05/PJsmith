using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour {

    //미니맵 플레이어
    private GameObject miniplayer;

    private WaveControl waveControl;
    private bool waveFlag = false;
    private float time = 0f;

    private void Awake()
    {
        miniplayer = GameObject.Find("Miniplayer");
        waveControl = GameObject.Find("WaveControl").GetComponent<WaveControl>();
    }

    private void Update()
    {
        if (waveFlag)
        {
            if (waveControl.getNowWave() == 3)
                wave3();

            if (waveControl.getNowWave() == 2)
                wave2();
        }
    }

    //wave2
    void wave2()
    {
        if (miniplayer.transform.position.x != -0.25f)
        {
            time += Time.deltaTime * 1.0f;
            miniplayer.transform.position = new Vector3(Mathf.Lerp(-2f, -0.25f, time),
                miniplayer.transform.position.y, miniplayer.transform.position.z);
        }
        else
        {
            time = 0f;
            waveFlag = false;
        }
    }
    //wave3
    void wave3()
    {
        if (miniplayer.transform.position.x != 1.7f)
        {
            time += Time.deltaTime * 1.0f;
            miniplayer.transform.position = new Vector3(Mathf.Lerp(-0.25f, 1.7f, time),
                miniplayer.transform.position.y, miniplayer.transform.position.z);
        }
        else
        {
            time = 0f;
            waveFlag = false;
        }
    }


    public void setWaveFlag(bool b)
    {
        waveFlag = b;
    }
}
