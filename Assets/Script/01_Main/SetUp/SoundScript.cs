using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class SoundScript : MonoBehaviour {

    GameObject SetPopup;
  //  [HideInInspector]
    public Slider EffectVolumeTest;  //reference for slider

    [SerializeField]
     Slider EffectVolumeTest2;  //reference for slider


    Slider EffectVolume;
    Slider BGMVolume;

    Text EffectText;
    Text BGMText;

    private AudioSource hsAudioBGM;//미스사운드
    public AudioClip sound_BGM;//기본사운드

    private AudioSource hsAudioWin;//미스사운드
    public AudioClip sound_Win;//기본사운드


    private AudioSource hsAudioNomal; //기본사운드
    private AudioSource hsAudiocri; //크리티컬사운드1
    private AudioSource hsAudiomiss;//미스사운드

    public AudioClip sound_fx;//기본사운드
    public AudioClip sound_fxcri;//크리티컬사운드1
    public AudioClip sound_miss;//미스사운드

    void Awake()
    {
        SetPopup = transform.Find("/02_UI/Main/Menu/SetupPopup").gameObject;
        EffectVolume = transform.Find("/02_UI/Main/Menu/SetupPopup/UIPanel/SetupBox/Slider_Sound").gameObject.GetComponent<Slider>();
        BGMVolume = transform.Find("/02_UI/Main/Menu/SetupPopup/UIPanel/SetupBox/Slider_Music").gameObject.GetComponent<Slider>();

        EffectText = transform.Find("/02_UI/Main/Menu/SetupPopup/UIPanel/SetupBox/Slider_Sound/Text").gameObject.GetComponent<Text>();
        BGMText = transform.Find("/02_UI/Main/Menu/SetupPopup/UIPanel/SetupBox/Slider_Music/Text").gameObject.GetComponent<Text>();
    }

    void Start()
    {
        EffectVolume.maxValue = 1.0f;
        EffectVolume.value = EffectVolume.maxValue;

        BGMVolume.maxValue = 1.0f;
        BGMVolume.value = BGMVolume.maxValue;

        //BGM초기화
        this.hsAudioBGM = this.gameObject.AddComponent<AudioSource>();
        this.hsAudioBGM.clip = this.sound_BGM;
        hsAudioBGM.Play();
        this.hsAudioBGM.loop = true;

        this.hsAudioWin = this.gameObject.AddComponent<AudioSource>();
        this.hsAudioWin.clip = this.sound_Win;
        this.hsAudioWin.time = 2.7f;
        this.hsAudioWin.loop = false;

        //사운드초기화
        this.hsAudioNomal = this.gameObject.AddComponent<AudioSource>();
        this.hsAudioNomal.clip = this.sound_fx;
        this.hsAudioNomal.loop = false;

        this.hsAudiocri = this.gameObject.AddComponent<AudioSource>();
        this.hsAudiocri.clip = this.sound_fxcri;
        this.hsAudiocri.loop = false;

        this.hsAudiomiss = this.gameObject.AddComponent<AudioSource>();
        this.hsAudiomiss.clip = this.sound_miss;
        this.hsAudiomiss.loop = false;

    }
    void Update()
    {//슬라이드바 게이지 설정

        if (SetPopup == true)
        {
            hsAudioNomal.volume = EffectVolume.value;
            hsAudiocri.volume = EffectVolume.value;
            hsAudiomiss.volume = EffectVolume.value;

            hsAudioBGM.volume = BGMVolume.value;
            hsAudioWin.volume = BGMVolume.value;

            BGMText.text = ((int)(BGMVolume.value * 100)).ToString();
            EffectText.text = ((int)(EffectVolume.value * 100)).ToString();
        }
    }
    public void EffectSoundPlay(int SoundNum)
    {
        if (SoundNum == 0)
        {
            this.hsAudioNomal.Play(); //일반공격사운드호출
        }
        else if (SoundNum == 1)
        {
            this.hsAudiocri.Play();//크리티컬1사운드호출
        }
        else if (SoundNum == 2)
        {
            this.hsAudiomiss.Play();//미스 사운드호출
        }
        else if (SoundNum == 3)
        {
            this.hsAudioWin.time = 2.7f;
            this.hsAudioWin.Play();//아이템 완성 사운드호출
        }
        else if (SoundNum == -1)
        {
            hsAudioBGM.Play();
            this.hsAudioBGM.loop = true;
        }
    }
    public void EffectSoundStop(int SoundNum)
    {
        if (SoundNum == 3)
        {
            this.hsAudioWin.Stop();//아이템 완성 사운드호출
        }
        else if (SoundNum == -1)
        {
            hsAudioBGM.Stop();
        }
    }
}
