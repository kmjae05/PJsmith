using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

public class ScrollHiden : MonoBehaviour, IBeginDragHandler, IEndDragHandler//, IDragHandler
{
    public GameObject Scrollbar;
    GameObject ScrollHandle;

    public float ScrollHidenTime;
    float time;
    bool ClickState;

    void Awake()
    {
        ScrollHandle = Scrollbar.transform.GetChild(0).GetChild(0).gameObject;
    }

	void Start ()
    {
	
	}
	void Update ()
    {

        if (ClickState == false && time >= 0)
        {
            time -= Time.deltaTime;
        }
        else if (ClickState == false &&time <= 0)
        {
            time -= Time.deltaTime;
            float color = 255 + (time * 255);
            float barcolor = 194 + (time * 194);
            Color HandleColor;
            Color BarColor;

            if (color >= 0)
            {
                HandleColor = new Color(73 / 255f, 64 / 255f, 49 / 255f, color / 255f);
                BarColor = new Color(0 / 255f, 0 / 255f, 0 / 255f, barcolor / 255f);
            }
            else
            {
                HandleColor = new Color(73 / 255f, 64 / 255f, 49 / 255f, 0);
                BarColor = new Color(0 / 255f, 0 / 255f, 0 / 255f, 0);
            }
            ScrollHandle.GetComponent<Image>().color = HandleColor;
            Scrollbar.GetComponent<Image>().color = BarColor;
        }
	}
    public void OnBeginDrag(PointerEventData eventData) //드래그 시작
    {
        Color HandleColor = new Color(73 / 255f, 64 / 255f, 49 / 255f, 255/255f);
        Color BarColor = new Color(0 / 255f, 0 / 255f, 0 / 255f, 194 / 255f);

        ClickState = true;
        ScrollHandle.GetComponent<Image>().color = HandleColor;
        Scrollbar.GetComponent<Image>().color = BarColor;
    }
    public void OnEndDrag(PointerEventData eventData) //드래그 끝
    {
        time = ScrollHidenTime;
        ClickState = false;
    }
}
