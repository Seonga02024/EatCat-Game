using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public enum BGM
{
    Main_Community = 0,
    Main_Seagrass = 1
}

public enum SFX
{
    TrashClick = 0,
    TrashSucess = 1,
    CoinGet = 2
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
    private BGM bgmCur = BGM.Main_Community;
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
    }
}
