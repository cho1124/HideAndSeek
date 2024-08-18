using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;
using UnityEngine.UI; // 버튼을 다루기 위해 추가

public class RoomScript : NetworkBehaviour
{
    public TMP_Text roomPlayersText;
    public GameObject readyButtonGameObject; // Button 대신 GameObject로 참조(왠지모르겠는데버튼으로참조가안돼요...ㅡㅡ..)

    //private NetworkManager roomManager;



    private void Awake()
    {
        //roomManager = HideAndSeekRoomManager.singleton;
    }

    private void Update()
    {
        UpdateRoomText();
    }

    void UpdateRoomText()
    {

        // 현재 플레이어 수 업데이트
        var curPlayers = FindObjectsOfType<HideAndSeekRoomPlayer>(); 
        roomPlayersText.text = string.Format($"Player: {curPlayers.Length} / 5");

        // 플레이어 수가 1명인 경우 Ready 버튼 비활성화
        if (curPlayers.Length <= 1) {
            readyButtonGameObject.SetActive(false);
        }
        else {
            readyButtonGameObject.SetActive(true);
        }

    }


    /// 
    /// ��                                                  ư
    /// 
    public void OnReturnButton()
    {
        if (GamePlayer.isHost)
        {
            HideAndSeekRoomManager.singleton.StopHost();
        }

        if (!GamePlayer.isHost)
        {
            HideAndSeekRoomManager.singleton.StopClient();

        }
        //SceneManager.LoadScene("Lobby");

    }

    
    public void OnReadyButton()
    {
        //var roomManager = HideAndSeekRoomManager.singleton;
        var roomPlayer = NetworkClient.connection.identity.GetComponent<HideAndSeekRoomPlayer>();
        //NetworkClient.Ready();

        if(!roomPlayer.readyToBegin)
        {
            roomPlayer.CmdChangeReadyState(true);
        }
        else
        {
            roomPlayer.CmdChangeReadyState(false);
        }
        
       //if(roomPlayer.readyToBegin)
       //{
       //    roomPlayer.CmdChangeReadyState(true);
       //
       //}
       //else
       //{
       //    roomPlayer.CmdChangeReadyState(false);
       //}

    }


    /// 
    /// ��                       ��                         ư
    /// 

}
