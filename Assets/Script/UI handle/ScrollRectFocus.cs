using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScrollRectFocus : MonoBehaviour {
    private GameObject SlotPanel;
    [HideInInspector]
    public List<Button> bttn = new List<Button>();
    public Button minBttn;
    public RectTransform center;
    public RectTransform[] bttnPosition;
    public float outline_width;
    
    public float[] distance;

    private int oreCnt = 0;

    public float ore_left_position;
    public float ore_distance;

    public float ore_expend_size;
    public float ore_expend_distance;
    public float ore_expend_max_size;

    public float move_v;
    public float move_a;


    int minBttnNum = -1;
    void Awake()
    {
        SlotPanel = GameObject.Find("Main").transform.Find("Menu/OreSelectPopup/UIPanel/Scroll/Panel").gameObject;
    }
	void Start () {
        for (int i = 0; i < SlotPanel.GetComponentsInChildren<Button>().Length; i++)
        {
            bttn.Add(SlotPanel.GetComponentsInChildren<Button>()[i]);
        }
        bttnPosition = new RectTransform[bttn.Count];


        int bttnLenth = bttn.Count;
        distance = new float[bttnLenth];

        //
        //for (int i = 0; i < bttn.Count; i++)   //버튼이 Panel의 Layout 옵션을 무시하도록 변경
        //{
        //    //bttn[i].GetComponent<LayoutElement>().ignoreLayout = true;
        //    bttn[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(++oreCnt * ore_distance + ore_left_position, 0);

        //    bttnPosition[i] = bttn[i].GetComponent<RectTransform>();
        //}
        
        //SlotPanel.GetComponent<GridLayoutGroup>().padding.right = 1170;


    }
    void Update()
    {
        for (int i = 0; i < bttn.Count; i++)
        {
            distance[i] = Mathf.Abs(center.transform.position.x - bttn[i].transform.position.x);
        }
        #region 범위 내에 들어온 버튼 확대
        for (int i = 0; i < bttn.Count; i++)
        {   
            if (distance[i] <= ore_expend_distance)
            {
                bttnPosition[i].transform.localScale = new Vector3((ore_expend_size - distance[i] * 0.02f) * 0.02f, (ore_expend_size - distance[i] * 0.02f) * 0.02f);
            }
            else
            {
                bttnPosition[i].transform.localScale = new Vector3(0.8f, 0.8f);
            }

            if (bttnPosition[i].transform.localScale.x >= ore_expend_max_size)
            {
                bttnPosition[i].transform.localScale = new Vector3(ore_expend_max_size, ore_expend_max_size);
            }

            if (bttnPosition[i].transform.localScale.x < 0.8f)
            {
                bttnPosition[i].transform.localScale = new Vector3(0.8f, 0.8f);
            }
            float minDistance = Mathf.Min(distance);

            if (minDistance == distance[i])
            {
                minBttn = bttn[i];
                minBttnNum = i;
                //bttn[i].GetComponentInChildren<Outline>().enabled = true;   //아웃라인 켜기
                //bttn[i].GetComponentInChildren<Image>().enabled = true;     //빛 켜기
                bttn[i].transform.SetAsLastSibling();                       //맨 앞으로
            }

            else
            {
                //bttn[i].GetComponentInChildren<Outline>().enabled = false;  //아웃라인 끄기
                //bttn[i].GetComponentInChildren<Image>().enabled = false;    //빛 끄기
            }
        }
        #endregion
    }

    //public void MoveToOre(int index)    //광석으로 이동
    //{
    //    if (index > minBttnNum)
    //    {
    //        StartCoroutine(MoveRight(index));
    //    }
    //    else
    //    {
    //        StartCoroutine(MoveLeft(index));
    //    }
    //}       
    IEnumerator MoveLeft(int index)     //왼쪽에 있다면 왼쪽으로 이동
    {
        float velocity = move_v;
        while (true)
        {
            SlotPanel.GetComponent<RectTransform>().anchoredPosition += new Vector2(velocity, 0);
            if (distance[index] <= 100)
            {
                yield break;
            }
            yield return null;
        }
    }   
    IEnumerator MoveRight(int index)    //오른쪽에 있다면 오른쪽으로 이동
    {
        float velocity = move_v;
        while (true)
        {
            SlotPanel.GetComponent<RectTransform>().anchoredPosition -= new Vector2(velocity, 0);
            if (distance[index] <= 100)
            {
                yield break;
            }
            yield return null;
        }
    }  
    
    public void PinPanel()      //광석선택 시작위치 고정
    {
        SlotPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1000.0f, -350.25f);
    }
}