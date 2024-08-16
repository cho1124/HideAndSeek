using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    /*
    UI에,, 볼륨 조절하는게 마스터볼륨 1개만 있는것 같아서 
    볼륨은 마스터 볼륨으로 배경음악과 효과음 모두 한번에 조절하게 했습니다. 

    효과음은 키워드1개로 불러올 수 있게 했습니다. 

    SoundManager.instance.PlaySFX("Explosion");

    */

    public static SoundManager instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]  //배경음과 효과음
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<AudioClip> bgmClips;
    public List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> sfxDict;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSFXDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeSFXDictionary() //시작하면 딕셔너리에 효과음을 담습니다.
    {
        sfxDict = new Dictionary<string, AudioClip>();
        foreach(var clip in sfxClips)
        {
            sfxDict.Add(clip.name, clip);
        }
    }

    public void PlayBGM(string bgmName)
    {
        AudioClip clip = bgmClips.Find(x => x.name == bgmName);
        if(clip)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
        else
        {
            Debug.LogWarning("음악이 없어용" +bgmName);
        }
    }

    public void PlaySFX(string sfxName)
    {
        if(sfxDict.ContainsKey(sfxName))
        {
            sfxSource.PlayOneShot(sfxDict[sfxName]);
        }
        else
        {
            Debug.LogWarning("효과음이 없어용"+sfxName);
        }
    }

    public void SetMasterVolume(float volume) // 오디오믹서의 마스터 볼륨조절
    { 
        float normalizedVolume = Mathf.Clamp(volume, 0f, 1f) * 100f; // 0-1 사이의 값을 0-100 사이로 변환
        audioMixer.SetFloat("MasterVolume", normalizedVolume); //SetFloat ; 오디오믹서의 파라미터값을 설정하는 메서드
    }


    //  public void setBGMVolume(float volume) // 오디오믹서의 BGM 볼륨조절
    //  {
    //      audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    //  }
    //
    //  public void setSFXVolume(float volume) // 오디오믹서의 SFX 볼륨조절
    //  {
    //      audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    //  }

    public void OnMasterVolumeChange(float value) //UI볼륨 슬라이더에 연결되어 볼륨 조절할것임
    {
        SoundManager.instance.SetMasterVolume(value);
    }
}
