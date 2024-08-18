using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterSound : MonoBehaviour
{
    public AudioSource master_sound_as = null;

    [SerializeField] private Slider master_sound_slider = null;

    public void MasterVolume()
    {
        master_sound_as.volume = master_sound_slider.value;

        PlayerPrefs.SetFloat("Master volume", master_sound_as.volume);
    }
}