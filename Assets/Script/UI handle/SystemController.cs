using UnityEngine;
using System.Collections;

public class SystemController : MonoBehaviour {


    public GameObject SystemPopup;
    public GameObject Sys_TitleText;
    public GameObject Sys_InfoText;
    public GameObject Sys_YesButton;
    public GameObject Sys_NoButton;
    public GameObject Sys_OkButton;

    void Awake()
    {
        SystemPopup = transform.Find("/02_UI/System/SystemCanvas/SystemPopup").gameObject;
        Sys_TitleText = transform.Find("/02_UI/System/SystemCanvas/SystemPopup/PopupBox/BackBox/Box_TitleText").gameObject;
        Sys_InfoText = transform.Find("/02_UI/System/SystemCanvas/SystemPopup/PopupBox/InfoText").gameObject;
        Sys_YesButton = transform.Find("/02_UI/System/SystemCanvas/SystemPopup/PopupBox/YesButton").gameObject;
        Sys_NoButton = transform.Find("/02_UI/System/SystemCanvas/SystemPopup/PopupBox/NoButton").gameObject;
        Sys_OkButton = transform.Find("/02_UI/System/SystemCanvas/SystemPopup/PopupBox/OkButton").gameObject;
    }
}
