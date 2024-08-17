using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class GamePlayer : NetworkBehaviour
{ 

    public static string nickName;
    public static string ip;
    public static string connectToIp;
    public static bool isHost;

    [SyncVar(hook = nameof(OnTeamChanged))]
    public int teamId;

    [SyncVar(hook = nameof(NumberChanged))]
    int randomNumber;


    public GameObject testobj;
    public Transform testtr;

    [Header("플레이어")]
    [SerializeField] private GameObject player_body;

    [Header("디버깅")]
    public Text randomtext;

    private HideAndSeekRoomManager room_manager;

    private void Start()
    {
        //gameObject.transform.SetParent(GameObject.Find("PlayerList").transform);
        //GameManager.instance.test_player.Add(gameObject);
        //
        //Debug.Log(nickName);

        room_manager = FindAnyObjectByType<HideAndSeekRoomManager>();


        //if(isServer)
        //{
        //
        //}
        
        
        CmdGenerateNumber();
        //Debug.Log("randomNumber : " + randomNumber);
        //Initiallize_Player();
        //Debug.Log("player's team id is " + teamId);


        Initiallize_Player();

    }

    private void OnConnectedToServer()
    {
        
    }

    [Server]
    void CmdGenerateNumber()
    {
        randomNumber = Random.Range(0, room_manager.hider_obj.Count);
        RpcSetRandomNumber(randomNumber); // 클라이언트에 값을 전달
        if (isLocalPlayer)
        {
            RpcSetRandomNumber(randomNumber);
        }
    }

    [ClientRpc]
    void RpcSetRandomNumber(int generatedNumber)
    {
        if (isClient)
        {
            randomNumber = generatedNumber;
            Debug.Log("randomNumber (Host/Client) : " + randomNumber);
            //Initiallize_Player(); // 난수가 생성된 후에 플레이어 초기화
        }
    }

    public void Initiallize_Player()
    {
        //int randomIndex = Random.Range(0, room_manager.hider_obj.Count);
        //randomtext.text = randomNumber.ToString();
        AssignPlayerBody(randomNumber);
    }

    
    void AssignPlayerBody(int randomIndex)
    {
        if (teamId == 1)
        {
            player_body = Instantiate(room_manager.hider_obj[randomIndex]);
            gameObject.transform.position = room_manager.hiderSpawnpoint.position;
            gameObject.tag = "Player_Hide";
        }
        else
        {
            player_body = Instantiate(room_manager.seeker_obj);
            gameObject.transform.position = room_manager.seekerSpawnpoint.position;
            //GetComponent<NetworkAnimator>().animator = player_body.GetComponent<Animator>();
        }



        if (player_body == null)
        {
            Debug.LogError("player body is null");
        }

        player_body.transform.SetParent(gameObject.transform);
        player_body.transform.localPosition = Vector3.zero;
    }


    
    public void CmdAssignTeam(int newTeamId)
    {
        //Debug.Log("assigned!");
        teamId = newTeamId;

        
    }

    void OnTeamChanged(int oldTeam, int newTeam)
    {
        // 팀 변경에 따른 처리
        Debug.Log("team changed");

    }
    void NumberChanged(int oldValue, int newvalue)
    {
        randomNumber = newvalue;
        //print("Random number: " + randomNumber);
        //Initiallize_Player();
    }

}
