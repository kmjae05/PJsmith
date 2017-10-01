    using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIanimationHandle : MonoBehaviour {
    public List<GameObject> UI_first = new List<GameObject>();
    public List<GameObject> UI_last = new List<GameObject>();

    public GameObject UI1_size;
    public GameObject UI2_size;

    public GameObject UI1_up;
    public GameObject UI1_shadow_up;
    public GameObject UI2_up;
    public GameObject UI2_shadow_up;
    public GameObject UI3_up;
    public GameObject UI3_shadow_up;
    public GameObject UI4_up;
    public GameObject UI4_shadow_up;

    void Start()
    {
        UI_first.Add(UI1_size);         //먼저 실행 될 아이콘 추가
        UI_first.Add(UI2_up);
        UI_first.Add(UI2_shadow_up);
        UI_first.Add(UI4_up);
        UI_first.Add(UI4_shadow_up);

        UI_last.Add(UI2_size);          //늦게 실행 될 아이콘 추가
        UI_last.Add(UI1_up);
        UI_last.Add(UI1_shadow_up);
        UI_last.Add(UI3_up);
        UI_last.Add(UI3_shadow_up);

        StartCoroutine(PlayAnimation());
        StartCoroutine(Check_Animation_Playing());
	}

    IEnumerator PlayAnimation()                     //1초 간격으로 애니메이션 실행
    {
        for (int i = 0; i < UI_first.Count; i++)
        {
            UI_first[i].GetComponent<Animation>().Play();
        }
        yield return new WaitForSeconds(1.0f);

        for(int i=0; i<UI_last.Count; i++){
            UI_last[i].GetComponent<Animation>().Play();
        }
    }
    IEnumerator Check_Animation_Playing()           //애니메이션이 멈췄다면 다시 실행
    {
        while (true)
        {
            yield return new WaitWhile(() => UI_first[0].GetComponent<Animation>().isPlaying);
            StartCoroutine(PlayAnimation());
        }
    }
}
