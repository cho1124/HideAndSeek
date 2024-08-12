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
        HostPanel.SetActive(true);
    }

    public void ClientBtn()
    {
        ClientPanel.SetActive(true);
    }
    public void ExitBtn()
    {
        Application.Quit();
    }

}
