using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject HostPanel;
    [SerializeField] private GameObject ClientPanel;

    private void Awake()
    {
        HostPanel.SetActive(false);
        ClientPanel.SetActive(false);
    }
    public void HostBtn()
    {
        SoundManager.instance.PlaySFX("Click");
        HostPanel.SetActive(true);
    }

    public void ClientBtn()
    {
        SoundManager.instance.PlaySFX("Click");
        ClientPanel.SetActive(true);
    }
    public void ExitBtn()
    {
        SoundManager.instance.PlaySFX("Click");
        Application.Quit();
    }

}
