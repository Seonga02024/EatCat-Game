using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public enum BGM
{
    Main_BGM = 0,
    SoloGame_BGM = 1,
    MTGame_BGM = 2
}

public enum SFX
{
    ChangeFood_SFX = 0,
    Click_SFX = 1,
    CountDown_SFX = 2,
    Eat_SFX = 3,
    GetPoint_SFX = 4,
    LosePoint_SFX = 5,
    WinResult_SFX = 6
}

public class CS_GameSoundManager : SingleTon<CS_GameSoundManager>
{
    [Header("[Object Setting]")]
    [SerializeField] private AudioMixer audioMixer;
    private AudioSource audioSource;

    [Header("[BGM]")]
    [SerializeField] private AudioClip[] bgmSources;

    [Header("[SFX]")]
    [SerializeField] private AudioSource[] sfxSource;

    private Coroutine fadeOutCoroutine;
    private bool isPause = false;
    public bool IsPause { get { return isPause; } }
    private BGM bgmCur = BGM.Main_BGM;
    private float audioVolume;
    private float audioVolumeCur = 0;

    // 0 ~ 1
    public void SetAudioMixer(float value)
    {
        if (value < 0.0001)
        {
            audioMixer.SetFloat("Master", -80);
        }
        else
        {
            audioMixer.SetFloat("Master", Mathf.Log10(value) * 20);
        }
    }

    public void SfxPlay(SFX sfx)
    {
        sfxSource[(int)sfx].Play();
    }

    public void BgmPlay()
    {
        if (!audioSource.isPlaying)
        {
            if (isPause)
            {
                isPause = false;
                audioSource.UnPause();
            }
            else
            {
                audioSource.clip = bgmSources[(int)bgmCur];
                audioSource.Play();
            }
        }
        else
        {
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine);
            }
            fadeOutCoroutine = StartCoroutine(FadeOut());
        }
    }

    // 1sec
    private IEnumerator FadeOut()
    {
        audioVolumeCur = audioVolume;
        float minIntervalTime = Time.deltaTime;
        while (true)
        {
            if (audioVolumeCur > 0)
            {
                audioVolumeCur -= audioVolume * minIntervalTime;
                audioSource.volume = audioVolumeCur;
                yield return new WaitForSeconds(minIntervalTime);
            }
            else
            {
                break;
            }
        }
        audioSource.clip = bgmSources[(int)bgmCur];
        audioSource.Play();
        audioSource.volume = audioVolume;
    }

    public void BgmPause()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPause = true;
        }
    }

    public void BgmSet(BGM index)
    {
        bgmCur = index;
    }

    private new void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioVolume = audioSource.volume;
        BgmPlay();
    }
}
