using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource; // 배경음악 오디오 소스
    public AudioSource effectSource; // 효과음 오디오 소스
    public GameObject bgmSoundOn; // 배경음악 on 버튼
    public GameObject bgmSoundOff; // 배경음악 off 버튼
    public GameObject effectSoundOn; // 효과음 on 버튼
    public GameObject effectSoundOff; // 효과음 off 버튼

    private bool bgmMuted = false;
    private bool effectMuted = false;

    private void Awake()
    {
        var soundManagers = FindObjectsOfType<SoundManager>();
        if (soundManagers.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("bgmMuted"))
        {
            PlayerPrefs.SetInt("bgmMuted", 0);
            LoadBGM();
        }
        else
        {
            LoadBGM();
        }

        if (!PlayerPrefs.HasKey("effectMuted"))
        {
            PlayerPrefs.SetInt("effectMuted", 0);
            LoadEffect();
        }
        else
        {
            LoadEffect();
        }

        UpdateBGMBtnImg();
        UpdateEffectBtnImg();

        bgmSource.mute = bgmMuted;
        effectSource.mute = effectMuted;
        bgmSource.Play();
    }

    public void BGMBtnClick()
    {
        bgmMuted = !bgmMuted;
        bgmSource.mute = bgmMuted;
        SaveBGM();
        UpdateBGMBtnImg();
    }

    public void EffectBtnClick()
    {
        effectMuted = !effectMuted;
        effectSource.mute = effectMuted;
        SaveEffect();
        UpdateEffectBtnImg();
    }

    private void UpdateBGMBtnImg()
    {
        bgmSoundOn.SetActive(!bgmMuted);
        bgmSoundOff.SetActive(bgmMuted);
    }

    private void UpdateEffectBtnImg()
    {
        effectSoundOn.SetActive(!effectMuted);
        effectSoundOff.SetActive(effectMuted);
    }

    private void LoadBGM()
    {
        bgmMuted = PlayerPrefs.GetInt("bgmMuted") == 1;
    }

    private void SaveBGM()
    {
        PlayerPrefs.SetInt("bgmMuted", bgmMuted ? 1 : 0);
    }

    private void LoadEffect()
    {
        effectMuted = PlayerPrefs.GetInt("effectMuted") == 1;
    }

    private void SaveEffect()
    {
        PlayerPrefs.SetInt("effectMuted", effectMuted ? 1 : 0);
    }
}
