using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    /*
    UI��,, ���� �����ϴ°� �����ͺ��� 1���� �ִ°� ���Ƽ� 
    ������ ������ �������� ������ǰ� ȿ���� ��� �ѹ��� �����ϰ� �߽��ϴ�. 

    ȿ������ Ű����1���� �ҷ��� �� �ְ� �߽��ϴ�. 

    SoundManager.instance.PlaySFX("Explosion");

    */

    public static SoundManager instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Audio Sources")]  //������� ȿ����
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

    void InitializeSFXDictionary() //�����ϸ� ��ųʸ��� ȿ������ ����ϴ�.
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
            Debug.LogWarning("������ �����" +bgmName);
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
            Debug.LogWarning("ȿ������ �����"+sfxName);
        }
    }

    public void SetMasterVolume(float volume) // ������ͼ��� ������ ��������
    { 
        float normalizedVolume = Mathf.Clamp(volume, 0f, 1f) * 100f; // 0-1 ������ ���� 0-100 ���̷� ��ȯ
        audioMixer.SetFloat("MasterVolume", normalizedVolume); //SetFloat ; ������ͼ��� �Ķ���Ͱ��� �����ϴ� �޼���
    }


    //  public void setBGMVolume(float volume) // ������ͼ��� BGM ��������
    //  {
    //      audioMixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
    //  }
    //
    //  public void setSFXVolume(float volume) // ������ͼ��� SFX ��������
    //  {
    //      audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    //  }

    public void OnMasterVolumeChange(float value) //UI���� �����̴��� ����Ǿ� ���� �����Ұ���
    {
        SoundManager.instance.SetMasterVolume(value);
    }
}
