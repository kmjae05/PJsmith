using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

    public GameObject TargetObj; //이동 목표
    public GameObject TargetPos; //기존의 좌표값

    public GameObject itemObj;


	// Use this for initialization
    void Awake()
    {
        itemObj.GetComponent<iTweenPath>().nodes[0] = TargetPos.transform.position;
        itemObj.GetComponent<iTweenPath>().nodes[1] = TargetObj.transform.position;
    }

	void Start () {
        itemObj.GetComponent<iTweenPath>().nodes[0] = TargetPos.transform.position;
        itemObj.GetComponent<iTweenPath>().nodes[1] = TargetObj.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ImageMoveController()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("New Path 1"), "easeType", "easeInOutCirc", "Time", 1, "looktarget", TargetObj));
    }

}
