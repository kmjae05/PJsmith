using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpecialAttackHandle : MonoBehaviour {
    public GameObject Iron;
    public GameObject HPSlider;
    public GameObject CameraShake;  //화면 흔들림
    public GameObject Clicker;
    public Image SpecialAttackCooltimeImage;
    public Text SpecialAttackCooltimeText;
    public Camera mCamera;
    public Animator chargeAttack;
    public float zoomDistance;  //카메라 확대효과
    public float zoomRate;

    public float blur_strength; //blur 효과
    public float blur_duration;

    private GameObject dmg;

    static public bool special_attack = false;
    void Start()
    {
        chargeAttack = GetComponent<Animator>();
        SpecialAttackCooltimeImage.fillAmount = 0;   //쿨타임 초기화
    }
    public void SpecialAttak()  //특수공격 버튼 눌렀을 때 실행
    {
        if (!special_attack && SpecialAttackCooltimeImage.fillAmount == 0 && Clicker.activeSelf)
        {
            //StartCoroutine(CoolDownTime());
        }
    }
    public void Damage()    //타격 시 실행
    {
        if (ToggleSetupScript.VibrationState)
        {
            Handheld.Vibrate(); //진동 호출
        }

        //mCamera.GetComponent<UnityStandardAssets.ImageEffects.Radialblur2>().enabled = true;
        //CameraShake.GetComponent<CameraShake>().CameraS();
        StartCoroutine(GetComponent<Effect>().CriticalBlurEffect(blur_strength, blur_duration));
        special_attack = false;

        //if (InfinityMode.Infinity_Mode)
        //{
        //    Iron.GetComponent<InfinityMode>().Damage(2);
        //}
        //else
        //{
        //    Iron.GetComponent<Attack>().Attack_State(2);
        //}
        //StartCoroutine(GetComponent<PrintDamage>().criticaldmgAnimation(dmg));      //크리티컬 데미지 애니메이션
        Clicker.SetActive(true);
    }
    //IEnumerator CoolDownTime()  //쿨타임 함수
    //{
    //    float cooltime = 15.0f;
    //    float lefttime = cooltime;

    //    Clicker.SetActive(false);
    //    special_attack = true;
    //    chargeAttack.Play("attack_chargein");
    //    SpecialAttackCooltimeImage.fillAmount = lefttime / cooltime;
    //    SpecialAttackCooltimeText.text = ((int)lefttime).ToString();
    //    SpecialAttackCooltimeText.enabled = true;
    //    //while (lefttime > 0 && Attack.Timer > 0 && (Iron.GetComponent<Attack>().NowHP > 0 || InfinityMode.Infinity_Mode))
    //    //{
    //    //    if (!Attack.stop_game)
    //    //    {
    //    //        lefttime -= Time.deltaTime;
    //    //        SpecialAttackCooltimeImage.fillAmount = lefttime / cooltime;
    //    //        SpecialAttackCooltimeText.text = ((int)lefttime).ToString();
    //    //    }
    //    //    yield return null;
    //    //}
    //    special_attack = false;
    //    SpecialAttackCooltimeText.enabled = false;
    //}
}
