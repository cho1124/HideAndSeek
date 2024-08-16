using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public TMP_InputField _ip;

    [SerializeField] GameObject[] hide_objs = null;

    [HideInInspector]
    public TMP_InputField _nickName;

    public string SceneName = "WaitingRoom";


    void Start()
    {
        SoundManager.instance.PlayBGM("Lobby");

        for (int i = 0; i < hide_objs.Length; i++)
        {
            hide_objs[i].SetActive(false);
        }
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
            //roomManager.StartHost();
            roomManager.StartHost();
            //SceneManager.LoadScene("Room Scene");
            SceneManager.LoadScene(SceneName);
        }
        // host ����

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
            roomManager.networkAddress = GamePlayer.connectToIp;
            roomManager.StartClient();
            SceneManager.LoadScene("Room Scene");
        }
    }
}