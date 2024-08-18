using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{

    public static GameManager instance = null;

    [Header("흐름처리")]
    [SyncVar(hook = nameof(ChangeHookTimer))]
    public float timer = 210f;
    private bool isGameOver = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI Timer_UI;
    [SerializeField] private GameObject timerObj;
    [SerializeField] private TextMeshProUGUI HP_UI;
    [SerializeField] private TextMeshProUGUI hider_text;
    [SerializeField] private TextMeshProUGUI seeker_text;

    [SyncVar(hook = nameof(OnHiderCountChanged))]
    public int hiderCount;

    [SyncVar(hook = nameof(OnSeekerCountChanged))]
    public int seekerCount;

    public HideAndSeekRoomManager roomManager;
    
    
    public GameObject wallMaria;


    public bool isWallMariaAlive = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        hider_text = GameObject.Find("Seeker text").GetComponent<TextMeshProUGUI>();
        seeker_text = GameObject.Find("Hider text").GetComponent<TextMeshProUGUI>();
        timerObj = GameObject.Find("Timer UI");
        if (timerObj != null)
        {
            Timer_UI = timerObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Timer UI object not found!");
        }

        if (Timer_UI == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on Timer UI object!");
        }
    }



    private void Start()
    {
        roomManager = FindAnyObjectByType<HideAndSeekRoomManager>();

        wallMaria = GameObject.Find("WallMaria");
        if (roomManager != null)
        {
            // 플레이어 수 변경 이벤트에 UI 업데이트 메서드 연결
            hiderCount = roomManager.GetTeamCount(1);
            seekerCount = roomManager.GetTeamCount(2);
            roomManager.OnPlayerCountChanged.AddListener(UpdatePlayerCounts);
        }


        if (isServer)
        {
            InitializeGame();
        }
    }

    [Server]
    private void InitializeGame()
    {
        // 초기화 로직 (예: 게임 시작 시 플레이어 초기화, 타이머 설정 등)
        Time.timeScale = 1;
        timer = 210f;
        isGameOver = false;

        isWallMariaAlive = true;


    }

    private void Update()
    {
        if(isServer)
        {

            CheckTimer();

            if(isWallMariaAlive)
                WallMariaControl();
        }
    }

    [Server]
    private void WallMariaControl()
    {
        if (timer <= 180f)
        {
            Debug.Log("WallMaria almost down");
            RpcWallMariaControl();
            isWallMariaAlive = false;
        }
        else
            return;
    }


    [ClientRpc]
    private void RpcWallMariaControl()
    {
        wallMaria.SetActive(false);
        Debug.Log("WallMaria down");
    }

    [Server]
    private void CheckTimer()
    {
        if (timer <= 0 && !isGameOver)
        {
            isGameOver = true;
            RpcGameOver("Hider Win!"); // 클라이언트들에게 게임 종료 알림
        }
        else
        {
            timer -= Time.deltaTime;
            //Debug.Log($"Server: Timer updated to {timer}"); // 서버 로그 추가
        }
    }

    private void ChangeHookTimer(float oldvalue, float newvalue)
    {
        //Debug.Log("timer : " + newvalue);
        UpdateTimerUI(newvalue); // 타이머 값이 변경될 때 UI 업데이트
    }

    private void UpdateTimerUI(float time)
    {
        Timer_UI.text = ((int)time).ToString(); // 타이머를 정수로 변환하여 텍스트 업데이트

        if(time <= 30f)
        {
            Timer_UI.color = Color.red;
        }
        else
        {
            Timer_UI.color = Color.black;
        }

        //Debug.Log("sync!");
    }

    private void RpcGameOver(string result)
    {
        Debug.Log(result);
        Time.timeScale = 0;
        // 추가적인 게임 종료 처리
    }

    private void OnHiderCountChanged(int oldCount, int newCount)
    {
        hider_text.text = $"Hiders : {newCount} ";
        
    }

    private void OnSeekerCountChanged(int oldCount, int newCount)
    {
        seeker_text.text = $"Seekers : {newCount}";
    }

    private void UpdatePlayerCounts()
    {
        hiderCount = roomManager.GetTeamCount(1);
        seekerCount = roomManager.GetTeamCount(2);

        
    }

}
