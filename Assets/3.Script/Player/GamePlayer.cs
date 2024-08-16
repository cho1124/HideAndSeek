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

    public GameObject testobj;
    public Transform testtr;

    private void Start()
    {
        //gameObject.transform.SetParent(GameObject.Find("PlayerList").transform);
        //GameManager.instance.test_player.Add(gameObject);
        //
        //Debug.Log(nickName);
        RpcInitializePlayer(testobj, testtr);
    }

    private void OnConnectedToServer()
    {
        
    }


    public void RpcInitializePlayer(GameObject playerPrefab, Transform spawnPoint)
    {


        Debug.Log("init");

        if (playerPrefab == null)
        {
            Debug.LogError("RpcInitializePlayer: playerPrefab is null!");
        }
        else
        {
            Debug.Log("RpcInitializePlayer called on client");
            GetComponent<Player_Control>().Initiallize_Player(playerPrefab, spawnPoint);
        }


    }

   
    
    public void CmdAssignTeam(int newTeamId)
    {
        Debug.Log("assigned!");
        teamId = newTeamId;
    }

    void OnTeamChanged(int oldTeam, int newTeam)
    {
        // 팀 변경에 따른 처리
        Debug.Log("team changed");


    }

}
