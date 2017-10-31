using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour {

    public static Player instance = null;


    static private User Play = new User();          //플레이어
    static public Hammer equipHm = new Hammer();   //장착 망치
    private EquipmentData equipmentData;


    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

        equipmentData = GameObject.Find("ThingsData").GetComponent<EquipmentData>();
        Play.equipWeapon[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 단검");
        Play.equipWeapon[1] = equipmentData.getEquipmentList().Find(x => x.name == "날카로운 단검");
        Play.equipArmor[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
        Play.equipArmor[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천갑옷");
        Play.equipPants[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천바지");
        Play.equipPants[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천바지");
        Play.equipHelmet[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
        Play.equipHelmet[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천투구");
        Play.equipGloves[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천장갑");
        Play.equipGloves[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천장갑");
        Play.equipBoots[0] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");
        Play.equipBoots[1] = equipmentData.getEquipmentList().Find(x => x.name == "초보자의 천부츠");

        for (int i = 0; i < 2; i++)
        {
            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == Play.equipWeapon[i].name).type, Play.equipWeapon[i].name, 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = getUser().Name;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = (i+ 1);

            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == Play.equipArmor[i].name).type, Play.equipArmor[i].name, 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = getUser().Name;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = (i + 1);

            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == Play.equipPants[i].name).type, Play.equipPants[i].name, 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = getUser().Name;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = (i + 1);

            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == Play.equipHelmet[i].name).type, Play.equipHelmet[i].name, 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = getUser().Name;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = (i + 1);

            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == Play.equipGloves[i].name).type, Play.equipGloves[i].name, 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = getUser().Name;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = (i + 1);

            ThingsData.instance.getInventoryThingsList().Add(new InventoryThings(ThingsData.instance.getThingsList().Find(x => x.name == Play.equipBoots[i].name).type, Play.equipBoots[i].name, 1));
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equip = true;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipChrName = getUser().Name;
            ThingsData.instance.getInventoryThingsList()[ThingsData.instance.getInventoryThingsList().Count - 1].equipSetNum = (i + 1);

        }



    }


    public void GetGameReward(int exp, int gold)        //제련 보상 획득
    {
        if (Play.level <= 30)
            StartCoroutine(GetExp(exp));
        //StartCoroutine(GetGold(gold));
    }
    public void getExp(int exp)
    {
        if (Play.level <= 30)
            StartCoroutine(GetExp(exp));
    }
    public IEnumerator GetExp(int exp)                  //경험치 획득 함수
    {
        if (SceneManager.GetActiveScene().name == "02_Lobby")
        {
            StartCoroutine(GameObject.Find("PlayerManager").transform.GetComponent<PlayerManager>().GetExp(exp));
        }
        else
        {
            Play.exp += exp;
            if (Play.exp >= Play.max_exp)
            {
                Play.exp -= Play.max_exp;
                Play.level += 1;
                Play.max_exp = Play.level * 20;
                Play.stat.strPower += Play.stat.strPower * 0.1f;
                Play.stat.dps = Play.stat.strPower * (float)Play.stat.attackSpeed * Play.stat.critical + Play.stat.defPower*Play.stat.evaRate;
                MineData.instance.Unlock();                 //레벨업하면 광산 건설 잠금 해제 체크
            }
        }
        yield return null;
    }
    public void GetMoney(string type, int amount)   //골드 획득, 골드 표시
    {
        if (string.Compare(type, "gold") == 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", Player.Play.gold, "to", Player.Play.gold + amount, "onUpdate", "GoldCount", "time", 1));
            Play.gold += amount;
            if (SceneManager.GetActiveScene().name == "02_Lobby")
                StartCoroutine(GameObject.Find("PlayerManager").transform.GetComponent<PlayerManager>().TextAnimation_Gold(new GameObject(), amount));
        }
        else if (string.Compare(type, "cash") == 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", Player.Play.cash, "to", Player.Play.cash + amount, "onUpdate", "CashCount", "time", 1));
            Play.cash += amount;
            if (SceneManager.GetActiveScene().name == "02_Lobby")
                StartCoroutine(GameObject.Find("PlayerManager").transform.GetComponent<PlayerManager>().TextAnimation_Cash(new GameObject(), amount));
        }

    }
    public void LostMoney(string type, int amount)
    {
        if(string.Compare(type, "gold")==0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", Player.Play.gold, "to", Player.Play.gold - amount, "onUpdate", "GoldCount", "time", 1));
            Play.gold -= amount;
        }
        else if(string.Compare(type, "cash") == 0)
        {
            iTween.ValueTo(gameObject, iTween.Hash("from", Player.Play.cash, "to", Player.Play.cash - amount, "onUpdate", "CashCount", "time", 1));
            Play.cash -= amount;
        }
    }



    public User getUser()
    {
        return Play;
    }

}



public class User
{
    public int user_no;
    public string user_id;
    public string Name;
    public int level;
    public int max_exp;
    public int exp;
    public int gold;
    public int cash;
    public int achvScore;
    public string title;

    //기본 능력치
    public Stat stat;
    public string attribute;

    //장비 Equipment로
    public Equipment[] equipWeapon;
    public Equipment[] equipArmor;
    public Equipment[] equipPants;
    public Equipment[] equipHelmet;
    public Equipment[] equipGloves;
    public Equipment[] equipBoots;

    public string logoutTime;

    public User()
    {
        this.user_no = 0;
        this.user_id = "0";
        this.Name = "대장장이 스미스";
        this.level = 1;
        this.exp = 0;
        this.max_exp = this.level * 10;
        this.gold = 500;
        this.cash = 100;
        this.achvScore = 0;
        this.title = "구리 마스터";

        stat = new Stat();
        this.stat.strPower = 50;
        this.stat.attackSpeed = 1.0f;
        this.stat.focus = 50;
        this.stat.critical = 20;
        this.stat.defPower = 5;
        this.stat.evaRate = 3;
        this.attribute = "no";
        this.stat.collectSpeed = 1.0f;
        this.stat.collectAmount = 1;
        this.stat.dps = this.stat.strPower * (float)this.stat.attackSpeed * this.stat.critical + this.stat.defPower*this.stat.evaRate;

        this.equipWeapon = new Equipment[2];
        this.equipArmor = new Equipment[2];
        this.equipPants = new Equipment[2];
        this.equipHelmet = new Equipment[2];
        this.equipGloves = new Equipment[2];
        this.equipBoots = new Equipment[2];

        this.logoutTime = "0";
    }
}