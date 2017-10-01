using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Effect : MonoBehaviour {

    public GameObject Attack_fx; //일반타격
    public GameObject Attack_fx_cri; //크리티컬

    public GameObject CShake;
    public GameObject IronShake;
    private GameObject EffectSound;
    public GameObject Chr_001;
    public GameObject MainCamera;

    public float cri_blur_strength;
    public float cri_blur_velocity;

    GameObject dmg;

    void Awake()
    {
        EffectSound = transform.Find("/05_Script/SetUp").gameObject;

        CShake = transform.Find("/03_Effect/CameraShake").gameObject;
        IronShake = transform.Find("").gameObject;
        Chr_001 = transform.Find("/01_3D/Chr/Chr_001").gameObject;
        MainCamera = transform.Find("/00_Camera/Main_Camera").gameObject;

        CShake.SetActive(false);
    }
    void Start()
    {
        CShake.SetActive(false); ;
    }
    public void WinSound() //아이템 생성 완료시
    {
        EffectSound.GetComponent<SoundScript>().EffectSoundStop(-1);
        EffectSound.GetComponent<SoundScript>().EffectSoundPlay(3);
        MainCamera.GetComponent<UnityStandardAssets.ImageEffects.Blur>().enabled = true;
    }
    public void LoseSound()//아이템 생성 실패시
    {
       // GameObject.Find("Iron").GetComponent<Attack>().CompleteLose();
    }

    public IEnumerator CriticalBlurEffect(float strength, float velocity)   //크리티컬 히트 시 Blur효과
    {
        yield return null;
        //MainCamera.GetComponent<UnityStandardAssets.ImageEffects.Radialblur2>().fSampleStrength = strength;
        //while(MainCamera.GetComponent<UnityStandardAssets.ImageEffects.Radialblur2>().fSampleStrength > 0){
        //    MainCamera.GetComponent<UnityStandardAssets.ImageEffects.Radialblur2>().fSampleStrength -= velocity;
        //    yield return null;
        //}
        //MainCamera.GetComponent<UnityStandardAssets.ImageEffects.Radialblur2>().enabled = false;
    }

    public void On_Trail()  //TrailRenderer 켬
    {
        GetComponentInChildren<TrailRenderer>().enabled = true;
    }
    public void Off_Trail()  //TrailRenderer 끔
    {
        GetComponentInChildren<TrailRenderer>().enabled = false;
    }

    public void AtkEffect(int AtkState)
    {
        if (AtkState != 2)// 공격
        {
            //CShake.GetComponent<CameraShake>().CameraS(); //카메라 진동 호출
            //IronShake.GetComponent<CameraShake>().CameraS(); //광석진동호출
            if (ToggleSetupScript.HaptipState)
            {
                AndroidManager.HapticFeedback(); //햅틱피드맥 호출
            }
            if (AtkState == 0)//일반공격
            {
                Attack_fx.GetComponent<Animator>().SetTrigger("Click");//애니메이션 트리거 작동
                EffectSound.GetComponent<SoundScript>().EffectSoundPlay(0);
                //StartCoroutine(Chr_001.GetComponent<PrintDamage>().dmgAnimation(dmg)); //노말 데미지 애니메이션 
            }
            else if (AtkState == 1)//크리티컬공격
            {
                Attack_fx_cri.GetComponent<Animator>().SetTrigger("Click");//애니메이션 트리거 작동
                if (ToggleSetupScript.VibrationState)
                {
                    Handheld.Vibrate(); //크리티컬시 진동 호출
                }
                EffectSound.GetComponent<SoundScript>().EffectSoundPlay(1);
                // MainCamera.GetComponent<UnityStandardAssets.ImageEffects.Radialblur2>().enabled = true;
                StartCoroutine(CriticalBlurEffect(cri_blur_strength, cri_blur_velocity));
                //StartCoroutine(Chr_001.GetComponent<PrintDamage>().criticaldmgAnimation(dmg));  //크리티컬 데미지 애니메이션
            }
        }
        else if (AtkState == 2) //미스
        {
            EffectSound.GetComponent<SoundScript>().EffectSoundPlay(2);
        }
    }
}