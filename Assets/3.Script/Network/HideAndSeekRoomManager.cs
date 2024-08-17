using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class HideAndSeekRoomManager : NetworkRoomManager
{
    public bool isRoom = true;

    private List<GameObject> hiders = new List<GameObject>();
    private List<GameObject> seekers = new List<GameObject>();

    [Header("스폰포인트")]
    public Transform seekerSpawnpoint;
    public Transform hiderSpawnpoint;

    [Header("각 플레이어 프리팹")]
    public GameObject seeker_obj;
    public List<GameObject> hider_obj;

    [Header("술래 카운트")]
    [SerializeField] private int seeker_count = 1;
    private int member_count;

    public override void OnStartServer()
    {
        member_count = 0;
    }


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

        GameObject spawnedObject = Instantiate(spawnPrefabs[0]);
        NetworkServer.Spawn(spawnedObject);
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

        int teamId = 1;

        // Check if the seekers count is less than seeker_count
        if (seekers.Count < seeker_count)
        {
            if (UnityEngine.Random.Range(0, 2) == 0 || member_count == NetworkServer.connections.Count - 1)
            {
                teamId = 2;
            }
        }

        member_count++;

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
        if (SceneManager.GetActiveScene().name.Equals("Map_v1"))
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
    }

    public int GetTeamCount(int teamId)
    {
        return teamId == 1 ? hiders.Count : teamId == 2 ? seekers.Count : 0;
    }
}
