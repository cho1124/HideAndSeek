using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FullScreen : MonoBehaviour
{
    private TextMeshPro on_off_text;

    private bool on_off_tr = true;

    private void Awake()
    {
        on_off_text = this.GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        on_off_text.text = "ON";
    }

    public void ScreenSetting()
    {
        on_off_tr = !on_off_tr;

        if (on_off_tr)
        {
            SetResolution(1920, 1080, true);

            on_off_text.text = "ON";
        }
        else
        {
            SetResolution(1280, 720, false);

            on_off_text.text = "OFF";
        }
    }

    private void SetResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
    }
}