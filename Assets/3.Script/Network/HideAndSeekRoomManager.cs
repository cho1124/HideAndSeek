using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class HideAndSeekRoomManager : NetworkRoomManager
{
    string hostIP;
    string nickName;
    public bool isRoom = true;

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


    

    public override void SceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        if (Utils.IsSceneActive(RoomScene))
        {
            // cant be ready in room, add to ready list
            PendingPlayer pending;
            pending.conn = conn;
            pending.roomPlayer = roomPlayer;
            pendingPlayers.Add(pending);
            Debug.Log(pendingPlayers.Count);
            return;
        }

        GameObject gamePlayer;

        gamePlayer = OnRoomServerCreateGamePlayer(conn, roomPlayer);

        if (gamePlayer == null)
        {
            //Debug.Log("gamePlayer");

            // get start position from base class
            Transform startPos = GetStartPosition();


            //startPos.position = new Vector3(0, 5f, 0);

            gamePlayer = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            //Debug.Log(gamePlayer.transform.position);

            //Debug.Log(SceneManager.GetActiveScene().name);
            GameManager.instance.playerSeek.Add(gamePlayer);
        }

        if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
            return;

        // replace room player with game player

        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);

        //AddPlayerToGameManager(gamePlayer_list);
    }


    // 게임 매니저에 플레이어를 추가하는 메서드
    private void AddPlayerToGameManager(List<GameObject> player)
    {
        foreach(GameObject gamePlayer in player)
        {
            GameManager.instance.AddPlayer(gamePlayer);
        }


        
    }


}
