using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using LitJson;
using System.Text;

public class LoadData : MonoBehaviour {
    public class User
    {
        public int user_no;
        public string user_id;
        public int level;
        public int exp;
        public int gold;
        public int cash;
        public string logoutTime;
    }
    public class Mail
    {
        public string title;
        public string comments;
        public string mail_reward_type;
        public int mail_reward_quantity;
        public string date;
        public Mail(string title, string comments, string mail_reward_type, int mail_reward_quantity, string date)
        {
            this.title = title;
            this.comments = comments;
            this.mail_reward_type = mail_reward_type;
            this.mail_reward_quantity = mail_reward_quantity;
            this.date = date;
        }
    }

    public User user;           //유저 클래스
    public List<int> itemList;  //아이템 리스트
    public List<Mail> mailList; //메일 리스트

    public string google_id;    //유저 id

    public WWWForm FormTmp;
    private WWW wwwUrl;

    public bool isRegistered;
    public bool checkversion;

    public DateTime dt;

    public GameObject GameExitPanel;

    IEnumerator Start()
    {
        itemList = new List<int>();
        mailList = new List<Mail>();
        user = new User();
        dt = new DateTime();

        GPGSMng.GetInstance.InitializeGPGS();   // 구글 서비스 초기화

        if (!GPGSMng.GetInstance.bLogin)        //구글계정 로그인
        {
            GPGSMng.GetInstance.LoginGPGS();
        }
        while (GPGSMng.GetInstance.GetIdGPGS() == null) { }   //GooglePlayGameServices 로그인, 안드로이드 빌드 시 주석 해제

        isRegistered = false;   
        checkversion = false;
        FormTmp = new WWWForm();

        google_id = GPGSMng.GetInstance.GetIdGPGS();            //GPGS ID, 로그인할 때 사용한다.

        yield return StartCoroutine(VersionCheck("1.0.0"));     //현재 버전 입력

        if (!checkversion)
        {
            //버전이 맞지 않은 경우
            //최신버전이 필요하다는 메시지
            Application.Quit();
        }

        yield return StartCoroutine(LogIn(google_id));          //로그인 시도
        if (!isRegistered)
        {
            yield return StartCoroutine(Register(google_id));   //등록 안 된 유저일 경우 회원가입
            yield return StartCoroutine(LogIn(google_id));      //로그인 재시도
        }

        yield return StartCoroutine(GetItems(user.user_no));    //itemList에 내 아이템 저장(int형 List)
        yield return StartCoroutine(GetMails(user.user_no));    //mailList에 내 메일 저장(Mail class형 List)
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //뒤로가기 버튼으로 어플 종료
        {
            GameExitPanel.SetActive(true);
        }
    }
    public void GameExit()
    {
        StartCoroutine(LogOut(user.user_no));
    }
    void OnApplicationPause()
    {
        SetTime();
    }

    #region 함수 모음
    public IEnumerator LogIn(string google_id)
    {
        FormTmp.AddField("user_id", google_id);
        wwwUrl = new WWW("http://clantown.dothome.co.kr/LogIn.php", FormTmp);

        yield return wwwUrl;
        JsonData User = JsonMapper.ToObject(wwwUrl.text);

        if (User["success"].ToString() == "True")       //유저 정보가 db 내에 존재함
        {
            isRegistered = true;

            user.user_no = Int32.Parse(User["rows"]["user_no"].ToString());
            user.user_id = google_id;
            user.level = Int32.Parse(User["rows"]["level"].ToString());
            user.exp = Int32.Parse(User["rows"]["exp"].ToString());
            user.gold = Int32.Parse(User["rows"]["gold"].ToString());
            user.cash = Int32.Parse(User["rows"]["cash"].ToString());
            user.logoutTime = User["rows"]["logoutTime"].ToString();
        }
        else
        {
            isRegistered = false;       //유저 정보가 db에 없음, 회원가입 진행
        }
    }
    public IEnumerator LogOut(int user_no)
    {
        dt = new DateTime();
        yield return StartCoroutine(GetTime());
        string logoutTime = dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;

        FormTmp.AddField("user_no", user_no);           //player number를 실어 보냄
        FormTmp.AddField("logoutTime", logoutTime);
        wwwUrl = new WWW("http://clantown.dothome.co.kr/LogOut.php", FormTmp);
      
        yield return wwwUrl;
        GPGSMng.GetInstance.LogoutGPGS();                   //구글계정 로그아웃
        Application.Quit();
    }
    public IEnumerator Register(string google_id)
    {
        FormTmp.AddField("user_id", google_id);    //googleID를 실어 보냄
        wwwUrl = new WWW("http://clantown.dothome.co.kr/Register.php", FormTmp);

        yield return wwwUrl;
    } 
    public IEnumerator GetItems(int user_no)
    {
        FormTmp.AddField("user_no", user_no);    //player number를 실어 보냄
        wwwUrl = new WWW("http://clantown.dothome.co.kr/GetItems.php", FormTmp);

        yield return wwwUrl;
        Debug.Log(wwwUrl.text);
        
        JsonData Items = JsonMapper.ToObject(wwwUrl.text);  //데이터 파싱

        if (Items["success"].ToString() == "True")          //아이템 목록을 itemList에 저장
        {
            for (int i = 0; i < Items["rows"].Count; i++)
            {
                itemList.Add(Int32.Parse(Items["rows"][i]["item_no"].ToString()));
            }
        }
    }
    public IEnumerator GetMails(int user_no)
    {
        FormTmp.AddField("user_no", user_no);    //player number를 실어 보냄
        wwwUrl = new WWW("http://clantown.dothome.co.kr/GetMails.php", FormTmp);

        yield return wwwUrl;
        string json_utf8 = Encoding.Default.GetString(wwwUrl.bytes);

        JsonData Mails = JsonMapper.ToObject(json_utf8);  //데이터 파싱
        if (Mails["success"].ToString() == "True")
        {
            for (int i = 0; i < Mails["rows"].Count; i++)
            {
                mailList.Add(new Mail(Mails["rows"][i]["title"].ToString(), 
                    Mails["rows"][i]["comments"].ToString(), 
                    Mails["rows"][i]["mail_reward_type"].ToString(), 
                    Int32.Parse(Mails["rows"][i]["mail_reward_quantity"].ToString()), 
                    Mails["rows"][i]["date"].ToString()));
            }
        }
        //Debug.Log(mailList[0].date);
    }
    public IEnumerator VersionCheck(string currentVersion)
    {
        FormTmp.AddField("current_build", currentVersion);
        wwwUrl = new WWW("http://clantown.dothome.co.kr/VersionCheck.php", FormTmp);

        yield return wwwUrl;
        JsonData version = JsonMapper.ToObject(wwwUrl.text);  //데이터 파싱
        if (version["success"].ToString() == "True")
        {
            checkversion = true;
        }
        //Debug.Log(wwwUrl.text);
    }
    public IEnumerator GetTime()
    {
        wwwUrl = new WWW("http://clantown.dothome.co.kr/GetTime.php");
        yield return wwwUrl;
        JsonData Time = JsonMapper.ToObject(wwwUrl.text);           //데이터 파싱
        dt = Convert.ToDateTime(Time["time"].ToString());           //dt에 시간 저장
    }
    public void SetTime()
    {
        dt = new DateTime();
        wwwUrl = new WWW("http://clantown.dothome.co.kr/GetTime.php");
        while (!wwwUrl.isDone) { }
        JsonData Time = JsonMapper.ToObject(wwwUrl.text);           //데이터 파싱
        dt = Convert.ToDateTime(Time["time"].ToString());           //dt에 시간 저장
        string logoutTime = dt.Year + "-" + dt.Month + "-" + dt.Day + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;

        FormTmp.AddField("user_no", user.user_no);           //player number를 실어 보냄
        FormTmp.AddField("logoutTime", logoutTime);
        wwwUrl = new WWW("http://clantown.dothome.co.kr/LogOut.php", FormTmp);
        while (!wwwUrl.isDone) { }
    }
    #endregion
}