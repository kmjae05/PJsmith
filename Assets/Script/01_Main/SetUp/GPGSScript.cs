using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;


public class GPGSScript : MonoBehaviour {

    

    private void Start()
    {
        //초기화
        //recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        //Activate the Google Play gaems platform
        PlayGamesPlatform.Activate();
        //


    }


    //로그인
    public void googleLoginButton()
    {
        Social.localUser.Authenticate(  (bool success) => {
            //handle success or failure
        });
    }

    //로그아웃
    public void googleLogoutButton()
    {
        // sign out
        ((PlayGamesPlatform)Social.Active).SignOut();
    }
}
