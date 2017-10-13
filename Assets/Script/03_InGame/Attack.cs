using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attack : MonoBehaviour {

    private GameObject InGameUI;
    private Animator CharAni;
    //노멀
    private Animator normalFx;
    private Animator criFx;
    private Animator lastFx;
    private Slider HpSlider;
    private Slider FeverSlider;
    private CameraShake cameraShake;
    private int chrPower = 0;

    private GameObject inventory;
    private GameObject item;

    private GameObject ObjCanvas;
    private GameObject Damage_Normal;
    private GameObject Damage_Critical;

    public float x_velocity = -50;
    public float y_velocity = 28;
    public float y_accelate = -1;

    public float d_vel = 20;
    public float d_acc = -1;
    public float d_y = 300;
    public float d_fade = 0.02f;
    

    void Awake()
    {
        InGameUI = GameObject.Find("InGameUI");

        CharAni = GameObject.Find("Chr_001").GetComponent<Animator>();
        normalFx = GameObject.Find("03_Effect").transform.Find("Attack_fx").GetComponent<Animator>();
        criFx = GameObject.Find("03_Effect").transform.Find("Attack_cri").GetComponent<Animator>();
        lastFx = GameObject.Find("03_Effect").transform.Find("Attack_Last").GetComponent<Animator>();

        normalFx.gameObject.SetActive(true);
        criFx.gameObject.SetActive(true);
        lastFx.gameObject.SetActive(false);
        HpSlider = GameObject.Find("HPBar").GetComponent<Slider>();
        //FeverSlider = GameObject.Find("FeverBar").GetComponent<Slider>();
        cameraShake = GameObject.Find("CameraShake").GetComponent<CameraShake>();


        ObjCanvas = GameObject.Find("GameObject");
        //inventory = ObjCanvas.transform.Find("Inventory").gameObject;
        item = ObjCanvas.transform.Find("Item").gameObject;
        Damage_Normal = ObjCanvas.transform.Find("Damage").gameObject;
        Damage_Critical = ObjCanvas.transform.Find("Damage_cri").gameObject;
    }
    void Start()
    {
        item.SetActive(false);
        chrPower = (int)Player.Play.stat.strPower + Player.equipHm.power;
    }

    void atkEffect()
    {
        int critical_rate = Random.Range(0, 100);

        if (critical_rate > 30)
        {
            //일반 공격
            NormalHit();
        }
        else if(critical_rate <= 30)
        {
            //크리티컬 공격
            CriticalHit();
        }
        iTween.ValueTo(gameObject, iTween.Hash("from", HpSlider.value, "to", InGameHandle.ore_hp, "onUpdate", "SetHpGauge", "time", 0.1));

        if (InGameHandle.ore_hp <= 0)
        {
            CharAni.speed = 0.02f;
            LastHit();
        }
    }

    void NormalHit()
    {
        InGameHandle.ore_hp -= chrPower;
        normalFx.SetBool("Click", true);
        //StartCoroutine(GetItem());
        StartCoroutine(PrintDamageNormal(chrPower));
    }

    void CriticalHit()
    {
        InGameHandle.ore_hp -= (int)(chrPower * 1.5f);
        cameraShake.EnableShake(0.1f);
        criFx.SetBool("Click", true);
        StartCoroutine(PrintDamageCritical((int)(chrPower * 1.5)));
    }
    void SkillHit()
    {
        InGameHandle.ore_hp -= (int)(chrPower * 10.0f);
        cameraShake.EnableShake(0.2f);
        if (!InGameHandle.fever)
        {
            InGameHandle.feverGauge += 20;
            iTween.ValueTo(gameObject, iTween.Hash("from", FeverSlider.value, "to", InGameHandle.feverGauge, "onUpdate", "SetFeverGauge", "time", 0.1));
        }

        iTween.ValueTo(gameObject, iTween.Hash("from", HpSlider.value, "to", InGameHandle.ore_hp, "onUpdate", "SetHpGauge", "time", 0.1));

        if (InGameHandle.ore_hp <= 0)
        {
            CharAni.speed = 0.02f;
            LastHit();
        }
        StartCoroutine(PrintDamageCritical((int)(chrPower * 10)));
    }
    void LastHit()
    {
        cameraShake.EnableShake(0.2f);
        lastFx.gameObject.SetActive(true);
        StartCoroutine(IncreaseAniSpeed());
    }
    IEnumerator GetItem()
    {
        RectTransform Target = inventory.GetComponent<RectTransform>();
        GameObject d_item = Instantiate(item);
        RectTransform Object = d_item.GetComponent<RectTransform>();
        d_item.transform.SetParent(InGameUI.transform, true);
        d_item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        d_item.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        d_item.SetActive(true);
        d_item.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        float d_xvel = x_velocity;
        float d_yvel = y_velocity;
        float d_acc = y_accelate;

        while (Object.anchoredPosition.x >= Target.anchoredPosition.x)
        {
            Object.anchoredPosition += new Vector2(d_xvel, d_yvel);
            d_yvel += d_acc;
            yield return null;
        }
        d_item.GetComponent<Image>().enabled = false;
        if (inventory.GetComponent<Animation>().isPlaying)
        {
            inventory.GetComponent<Animation>().Stop();
        }
        inventory.GetComponent<Animation>().Play();
        Destroy(d_item);
    }

    IEnumerator IncreaseAniSpeed()
    {
        for (float i = 0.05f; i <= 1.0f; i += 0.005f)
        {
            CharAni.speed = i;
            yield return null;
        }
        CharAni.speed = 1.0f;
    }

    void SetHpGauge(float num)
    {
        HpSlider.value = num;
    }
    void SetFeverGauge(float num)
    {
        FeverSlider.value = num;
    }
    IEnumerator PrintDamageNormal(int damage)
    {
        GameObject dmg = Instantiate(Damage_Normal);
        dmg.transform.SetParent(ObjCanvas.transform, false);
        dmg.SetActive(true);
        Text dmgText = dmg.GetComponent<Text>();
        Image dmgImage = dmg.transform.Find("Image").GetComponent<Image>();
        dmgText.text = damage.ToString();
        RectTransform dmgPosition = dmg.GetComponent<RectTransform>();

        float vel = d_vel;
        float acc = d_acc;
        float fade = 1.0f;
        float fadeRate = d_fade;

        while (d_y >= dmgPosition.anchoredPosition.y)
        {
            dmgPosition.anchoredPosition += new Vector2(0, vel);
            vel -= acc;
            yield return null;
        }

        while (fade > 0)
        {
            dmgPosition.anchoredPosition += new Vector2(0, fade);
            dmgText.color = new Color(dmgText.color.r, dmgText.color.g, dmgText.color.b, fade);
            dmgImage.color = new Color(dmgImage.color.r, dmgImage.color.g, dmgImage.color.b, fade);
            fade -= fadeRate;
            yield return null;
        }
        Destroy(dmg);
    }
    IEnumerator PrintDamageCritical(int damage)
    {
        GameObject dmg = Instantiate(Damage_Critical);
        dmg.transform.SetParent(ObjCanvas.transform, false);
        dmg.SetActive(true);
        Text dmgText = dmg.GetComponent<Text>();
        Image dmgImage = dmg.transform.Find("Image").GetComponent<Image>();
        dmgText.text = damage.ToString();
        RectTransform dmgPosition = dmg.GetComponent<RectTransform>();

        float vel = d_vel;
        float acc = d_acc;
        float fade = 1.0f;
        float fadeRate = d_fade;

        while (d_y+100 >= dmgPosition.anchoredPosition.y)
        {
            dmgPosition.anchoredPosition += new Vector2(0, vel);
            vel -= acc;
            yield return null;
        }

        while (fade > 0)
        {
            dmgPosition.anchoredPosition += new Vector2(0, fade);
            dmgText.color = new Color(dmgText.color.r, dmgText.color.g, dmgText.color.b, fade);
            dmgImage.color = new Color(dmgImage.color.r, dmgImage.color.g, dmgImage.color.b, fade);
            fade -= fadeRate;
            yield return null;
        }
        Destroy(dmg);
    }
}