using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickProfilePanel : MonoBehaviour, IPointerClickHandler
{
    private Camera MainCamera;
    private GameObject ProfilePopup;
    void Awake()
    {
        MainCamera = GameObject.Find("Main_Camera").GetComponent<Camera>();
        ProfilePopup = GameObject.Find("Menu").transform.Find("ProfilePopup").gameObject;
    }
    void Start()
    {
        //StartCoroutine(CameraMove());
    }
    public void OnPointerClick(PointerEventData data)
    {
        ProfilePopup.SetActive(true);
    }
    private IEnumerator CameraMove()
    {
        while (true)
        {
            yield return new WaitUntil(() => ProfilePopup.activeInHierarchy);
            //확대한 포지션으로 이동
            //MainCamera.trasform.Position = new Vector3(x,y);
            //MainCamera.orthographicSize = size;
            yield return new WaitUntil(() => !ProfilePopup.activeInHierarchy);
            //기본 포지션으로 이동
            //MainCamera.trasform.Position = new Vector3(x,y);
            //MainCamera.orthographicSize = size;
        }
    }
}
