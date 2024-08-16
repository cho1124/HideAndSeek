using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnOff : MonoBehaviour
{
    private TextMeshProUGUI on_off_text;

    private bool on_off_tr = false;

    private void Awake()
    {
        on_off_text = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        on_off_text.text = "OFF";
    }

    public void ScreenSetting()
    {
        SetResolution(1920, 1080, true);

        on_off_tr = !on_off_tr;

        if (on_off_tr)
        {
            on_off_text.text = "ON";
        }
        else
        {
            on_off_text.text = "OFF";
        }
    }

    public void SetResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
    }
}