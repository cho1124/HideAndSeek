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

    [Header("플레이어")]
    [SerializeField] private GameObject player_body;

    private HideAndSeekRoomManager room_manager;

    private void Start()
    {
        //gameObject.transform.SetParent(GameObject.Find("PlayerList").transform);
        //GameManager.instance.test_player.Add(gameObject);
        //
        //Debug.Log(nickName);

        room_manager = FindAnyObjectByType<HideAndSeekRoomManager>();

        
        Initiallize_Player(testobj, testtr);
        Debug.Log("player's team id is " + teamId);
    }

    private void OnConnectedToServer()
    {
        
    }

    public void Initiallize_Player(GameObject player, Transform spawnPoint)
    {

        //player_body = teamId == 1 ? Instantiate(room_manager.hider_obj[Random.Range(0, room_manager.hider_obj.Count)]) : Instantiate(room_manager.seeker_obj);


        if(teamId == 1)
        {
            player_body = Instantiate(room_manager.hider_obj[Random.Range(0, room_manager.hider_obj.Count)]);
            gameObject.transform.position = room_manager.hiderSpawnpoint.position;
        }
        else
        {
            player_body = Instantiate(room_manager.seeker_obj);
            gameObject.transform.position = room_manager.seekerSpawnpoint.position;
        }



        if (player_body == null)
        {
            Debug.LogError("player body is null");
        }

        player_body.transform.SetParent(gameObject.transform);
        player_body.transform.localPosition = Vector3.zero;
        //room_manager.
        //gameObject.transform.position = spawnPoint.position;

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
