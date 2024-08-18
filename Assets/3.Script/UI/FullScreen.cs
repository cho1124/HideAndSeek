using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FullScreen : MonoBehaviour
{
    private TextMeshProUGUI on_off_text;

    private bool on_off_tr = true;

    private void Awake()
    {
        on_off_text = this.GetComponentInChildren<TextMeshProUGUI>();
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
            aaaaaaaaaaaaaaaSetResolution(1920, 1080, true);

            on_off_text.text = "ON";
        }
        else
        {
            aaaaaaaaaaaaaaaSetResolution(1280, 720, false);

            on_off_text.text = "OFF";
        }
    }

    public void aaaaaaaaaaaaaaaSetResolution(int width, int height, bool fullscreen)
    {
        Screen.SetResolution(width, height, fullscreen);
    }
}