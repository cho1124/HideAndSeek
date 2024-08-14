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

        // ���� �÷��̾� ��
        var curPlayers = FindObjectsOfType<HideAndSeekRoomPlayer>(); //���̤��Ƥ����Ƥää̤��������Ӥ̤����Ӥ���
        roomPlayersText.text = string.Format($"Player: {curPlayers.Length} / 5");

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
