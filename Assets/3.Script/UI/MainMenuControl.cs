using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class MainMenuControl : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject hostMenu;
    public GameObject joinMenu;

    [HideInInspector]
    public TMP_InputField _nickName;


    public TMP_InputField _ip;
    void Start()
    {
        mainMenu.SetActive(true);
        hostMenu.SetActive(false);
        joinMenu.SetActive(false);
    }





    /// 
    /// 
    /// 
    /// 
    /// ��                              ư 
    /// 
    /// 
    /// 


    public void OnHostMenuButton()
    {
        mainMenu.SetActive(false);
        hostMenu.SetActive(true);
    }




    // �г��� �Է��ϰ� host
    public void OnHostButton()
    {
        _nickName = GameObject.Find("InputNameField").GetComponent<TMP_InputField>();
        if (_nickName.text != string.Empty)
        {
            GamePlayer.nickName = _nickName.text;
            //GamePlayer.ip =
            GamePlayer.isHost = true;


            var roomManager = HideAndSeekRoomManager.singleton;
            roomManager.StartHost();

            SceneManager.LoadScene("Room Scene");
        }
        // host ����

    }



    public void OnJoinMenuButton()
    {
        mainMenu.SetActive(false);
        joinMenu.SetActive(true);

    }



    // connect ��ư
    public void OnConnectButton()
    {
        // ip �Է��ϰ� ����
        _nickName = GameObject.Find("InputNameField").GetComponent<TMP_InputField>();
        _ip = GameObject.Find("InputIpField").GetComponent<TMP_InputField>();
        if(_nickName.text != string.Empty && _ip.text != string.Empty)
        {
            GamePlayer.nickName = _nickName.text;
            GamePlayer.connectToIp = _ip.text;
            GamePlayer.isHost = false;

            var roomManager = HideAndSeekRoomManager.singleton;
            //
            roomManager.networkAddress = _ip.text;
            roomManager.StartClient();
            SceneManager.LoadScene("Room Scene");
        }
    }

    public void OnBackButton()
    {
        if (hostMenu.activeSelf)
        {
            hostMenu.SetActive(false);
            mainMenu.SetActive(true);
        }


        if (joinMenu.activeSelf)
        {
            joinMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }





    public void OnExitButton()
    {
        Application.Quit();
    }



    /// 
    /// 
    /// 
    /// 
    /// ��                              ư                    ��
    /// 
    /// 
    /// 




}
