using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer audioMixer; // ����� �ͼ� ����
    private const string MusicVolumeParam = "MusicVolume"; // ������ ���� �Ķ����
    private const string BGMVolumeParam = "BGMVolume";     // BGM ���� �Ķ����
    private const string SFXVolumeParam = "SFXVolume";     // SFX ���� �Ķ����

    private void Awake() {
        // �̱��� ���� ���� 
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        // ó�� ������ �� ����� PlayerPrefs�� ������ �ҷ����� , ���尪������ �⺻�� 0.85f������
        float musicVolume = PlayerPrefs.GetFloat(MusicVolumeParam, 0.85f);
        float bgmVolume = PlayerPrefs.GetFloat(BGMVolumeParam, 0.85f);
        float sfxVolume = PlayerPrefs.GetFloat(SFXVolumeParam, 0.85f);

        // ����� �ͼ��� ������ ����
        SetMasterVolume(musicVolume);
        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }


    //�� ������ �����ϰ� ������ ���� ������

    public void SetMasterVolume(float volume) //
        {
        audioMixer.SetFloat(MusicVolumeParam, Mathf.Log10(volume) * 20); //���� ���� ������ͼ����� ����ϴ� ���ú��� ��ȯ�Ͽ� ������ͼ��� �Ķ���͸� ����
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
