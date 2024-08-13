using UnityEngine;
using Mirror;

public class RoomScript : NetworkBehaviour
{
    //var roomManager = HideAndSeekRoomManager.singleton;
    private void Awake()
    {
        
        //
        //// host
        //if (GamePlayer.isHost)
        //{
        //
        //    roomManager.StartHost();
        //}
        //
        //
        //// client
        //if(!GamePlayer.isHost)
        //{
        //    roomManager.networkAddress = GamePlayer.connectToIp;
        //    roomManager.StartClient();
        //}


        //void OnReadyButton()
        //{
        //
        //}

        
    }


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

}
