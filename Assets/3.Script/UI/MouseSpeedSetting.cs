using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSpeedSetting : MonoBehaviour
{
    private Slider mouse_speed_slider = null;

    private void Awake()
    {
        mouse_speed_slider = this.GetComponent<Slider>();
    }

    private void Start()
    {
        mouse_speed_slider.value = PlayerPrefs.GetFloat("Mouse speed");
    }

    public void MSS()
    {
        Player_Control.mouse_speed = mouse_speed_slider.value;

        PlayerPrefs.SetFloat("Mouse speed", Player_Control.mouse_speed);

        Debug.Log(mouse_speed_slider.value);
    }
}