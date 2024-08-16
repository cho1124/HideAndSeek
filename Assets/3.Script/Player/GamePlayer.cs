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

        // Ŭ���̾�Ʈ�� ������ ��, �� ID�� ���� �ʱ� ������ �� �� �ֽ��ϴ�.
        if (teamId == 1)
        {
            // �� 1�� ���� �ʱ�ȭ
        }
        else if (teamId == 2)
        {
            // �� 2�� ���� �ʱ�ȭ
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
        // �÷��̾� �ʱ�ȭ ����
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
        // �� ���濡 ���� ó��
        Debug.Log("team changed");


    }

}
