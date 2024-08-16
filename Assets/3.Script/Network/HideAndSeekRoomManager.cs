using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class HideAndSeekRoomManager : NetworkRoomManager
{
    public bool isRoom = true;

    public List<GameObject> hiders = new List<GameObject>();
    public List<GameObject> seekers = new List<GameObject>();

    [Header("스폰포인트")]
    [SerializeField] private Transform seekerSpawnpoint;
    [SerializeField] private Transform hiderSpawnpoint;

    [Header("각 플레이어 프리팹")]
    [SerializeField] private GameObject seeker_obj;
    [SerializeField] private List<GameObject> hider_obj;

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
            // Room 씬에서 대기
            PendingPlayer pending;
            pending.conn = conn;
            pending.roomPlayer = roomPlayer;
            pendingPlayers.Add(pending);
            Debug.Log(pendingPlayers.Count);
            return;
        }

        GameObject gamePlayer = OnRoomServerCreateGamePlayer(conn, roomPlayer);

        if (gamePlayer == null)
        {
            Transform startPos = GetStartPosition();
            gamePlayer = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        }

        AssignPlayerToTeam(gamePlayer);

        if (!OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer))
            return;

        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);
    }

    private void AssignPlayerToTeam(GameObject player)
    {
        var playerScript = player.GetComponent<GamePlayer>();
        int teamId = hiders.Count <= seekers.Count ? 1 : 2;


        if (teamId == 1)
        {
            //playerScript.RpcInitializePlayer(hider_obj[UnityEngine.Random.Range(0, hider_obj.Count)], hiderSpawnpoint);
            AssignToTeam(player, 1);  // 숨는 팀에 배정
        }
        else
        {
            //playerScript.RpcInitializePlayer(seeker_obj, seekerSpawnpoint);
            AssignToTeam(player, 2);  // 찾는 팀에 배정
        }
    }

    private void AssignToTeam(GameObject player, int teamId)
    {
        var playerScript = player.GetComponent<GamePlayer>();
        playerScript.CmdAssignTeam(teamId);

        if (teamId == 1)
        {
            hiders.Add(player);
        }
        else if (teamId == 2)
        {
            seekers.Add(player);
        }
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        GameObject player = conn.identity.gameObject;
        GamePlayer playerScript = player.GetComponent<GamePlayer>();

        if (playerScript.teamId == 1)
        {
            hiders.Remove(player);
        }
        else if (playerScript.teamId == 2)
        {
            seekers.Remove(player);
        }

        base.OnRoomServerDisconnect(conn);
    }

    public int GetTeamCount(int teamId)
    {
        return teamId == 1 ? hiders.Count : teamId == 2 ? seekers.Count : 0;
    }
}
