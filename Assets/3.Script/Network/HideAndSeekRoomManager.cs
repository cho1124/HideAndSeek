using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class HideAndSeekRoomManager : NetworkRoomManager
{
    string hostIP;
    string nickName;


    //private void OnConnectedToServer()
    //{
    //    NetworkClient.AddPlayer();
    //    Debug.Log("Added Player");
    //}


    

    private void OnApplicationQuit()
    {
        if (GamePlayer.isHost)
        {


            StopHost();

        }
        if (!GamePlayer.isHost)
        {
            StopClient();
        }
    }

}
