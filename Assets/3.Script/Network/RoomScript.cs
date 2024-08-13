using UnityEngine;
using Mirror;
using TMPro;

public class RoomScript : NetworkBehaviour
{
    public TMP_Text roomPlayersText;
    
    private void Awake()
    {
        var roomManager = HideAndSeekRoomManager.singleton;

        
    }

    private void Update()
    {
        UpdateRoomText();
    }



    void UpdateRoomText()
    {

        // 현재 플레이어 수
        var curPlayers = FindObjectsOfType<HideAndSeekRoomPlayer>();
        roomPlayersText.text = string.Format($"플레이어 : {curPlayers.Length} / 5");

    }


    /// 
    /// 버                                                  튼
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
    /// 버                       끝                         튼
    /// 

}
