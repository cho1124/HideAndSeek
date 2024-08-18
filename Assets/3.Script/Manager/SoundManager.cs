using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer audioMixer; // 오디오 믹서 참조
    private const string MusicVolumeParam = "MusicVolume"; // 마스터 볼륨 파라미터
    private const string BGMVolumeParam = "BGMVolume";     // BGM 볼륨 파라미터
    private const string SFXVolumeParam = "SFXVolume";     // SFX 볼륨 파라미터

    private void Awake() {
        // 싱글톤 패턴 적용 
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        // 처음 시작할 때 저장된 PlayerPrefs에 볼륨값 불러오기 , 저장값없으면 기본값 0.85f가져옴
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeParam, 0.85f);
        float bgmVolume = PlayerPrefs.GetFloat(BGMVolumeParam, 0.85f);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeParam, 0.85f);

        // 오디오 믹서에 볼륨값 적용
        SetMasterVolume(musicVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }


    //각 볼륨을 조절하고 설정된 값을 저장함

    public void SetMasterVolume(float volume) //
        {
        audioMixer.SetFloat(MusicVolumeParam, Mathf.Log10(volume) * 20); //볼륨 값을 오디오믹서에서 사용하는 데시벨로 전환하여 오디오믹서의 파라미터를 설정
        PlayerPrefs.SetFloat(MusicVolumeParam, volume);
    }

    public void SetBGMVolume(float volume) 
        {
        audioMixer.SetFloat(BGMVolumeParam, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(BGMVolumeParam, volume);
    }

    public void SetSFXVolume(float volume) 
        {
        audioMixer.SetFloat(SFXVolumeParam, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFXVolumeParam, volume);
    }
}
