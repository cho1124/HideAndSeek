using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;

public class GamePlayer : NetworkBehaviour
{ 

    public static string nickName;
    public static string ip;
    public static string connectToIp;
    public static bool isHost;

    private int hp_max;
    private int hp_current;
    public bool is_dead = false;
    public bool is_seeker = false;

    public event Action<int> OnHealthChanged;

    [SyncVar(hook = nameof(OnTeamChanged))]
    public int teamId;

    [SyncVar(hook = nameof(NumberChanged))]
    int randomNumber;


    public GameObject testobj;
    public Transform testtr;

    [Header("플레이어")]
    [SerializeField] private GameObject player_body;
    
    private HideAndSeekRoomManager room_manager;

    private void Start()
    {

        room_manager = FindAnyObjectByType<HideAndSeekRoomManager>();
        CmdGenerateNumber();
        
        AssignPlayerBody(randomNumber);


        


    }

    private void OnConnectedToServer()
    {
        
    }

    [Server]
    void CmdGenerateNumber()
    {
        randomNumber = UnityEngine.Random.Range(0, room_manager.hider_obj.Count);
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

    void AssignPlayerBody(int randomIndex)
    {
        if (teamId == 1)
        {
            hp_current = 5;

            if(isLocalPlayer)
            {
                OnHealthChanged?.Invoke(hp_current);
            }

            player_body = Instantiate(room_manager.hider_obj[randomIndex]);
            
            transform.position = room_manager.hiderSpawnpoint.position;
            gameObject.tag = "Player_Hide";
        }
        else
        {
            hp_current = 100;

            if (isLocalPlayer)
            {
                OnHealthChanged?.Invoke(hp_current);
            }

            player_body = Instantiate(room_manager.seeker_obj);
            Debug.Log("player_body" + player_body.name);
            transform.position = room_manager.seekerSpawnpoint.position;
            Player_Control playercon = GetComponent<Player_Control>();
            playercon.player_ani = player_body.GetComponent<Animator>();


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
    }

    public void TakeDamage(int damage)
    {
        OnHealthChanged?.Invoke(hp_current);
        hp_current -= damage;
        if (hp_current <= 0)
        {
            Die();
        }

    }

    public void Die()
    {
        is_dead = true;
    }



}
