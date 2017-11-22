using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class GPGSScript : MonoBehaviour {

    //게스트 로그인 확인



    private void Start()
    {
        //초기화
        //recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        //Activate the Google Play gaems platform
        PlayGamesPlatform.Activate();


        //구글 로그인 확인
        if (Social.localUser.authenticated)
        {
            GameObject.Find("AlertManager").GetComponent<AlertManager>().AcvBoxHandle("구글 계정 연동");
        }
        else GameObject.Find("AlertManager").GetComponent<AlertManager>().AcvBoxHandle("구글 계정 연동 실패");

    }


    //로그인
    public void googleLoginButton()
    {
        Social.localUser.Authenticate(  (bool success) => {
            if(success)
                GameObject.Find("AlertManager").GetComponent<AlertManager>().AcvBoxHandle("구글 계정 연동");
            //handle success or failure
        });
    }

    //로그아웃
    public void googleLogoutButton()
    {
        // sign out
        //((PlayGamesPlatform)Social.Active).SignOut();
    }
}
