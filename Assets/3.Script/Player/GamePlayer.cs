using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GamePlayer : NetworkBehaviour
{ 

    public static string nickName;
    public static string ip;
    public static string connectToIp;
    public static bool isHost;
    [SyncVar(hook = nameof(OnTeamChanged))]
    public int teamId;


    public override void OnStartClient()
    {
        base.OnStartClient();

        // 클라이언트가 시작할 때, 팀 ID에 따라 초기 설정을 할 수 있습니다.
        if (teamId == 1)
        {
            // 팀 1에 대한 초기화
        }
        else if (teamId == 2)
        {
            // 팀 2에 대한 초기화
        }
    }

    [Command]
    public void CmdAssignTeam(int newTeamId)
    {
        teamId = newTeamId;
    }

    [Command]
    public void CmdInitPlayer(GameObject playerPrefab, Transform spawnPoint)
    {
        // 플레이어 초기화 로직
        var playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.Spawn(playerInstance, connectionToClient);
    }


    public void RpcInitializePlayer(GameObject playerPrefab, Transform spawnPoint)
    {

        if (playerPrefab == null)
        {
            Debug.LogError("RpcInitializePlayer: playerPrefab is null!");
        }
        else
        {
            Debug.Log("RpcInitializePlayer called on client");
            GetComponent<Player_Control>().Initiallize_Player(playerPrefab, spawnPoint.position);
        }


    }

    

    void OnTeamChanged(int oldTeam, int newTeam)
    {
        // 팀 변경에 따른 처리
        Debug.Log("team changed");


    }

}
