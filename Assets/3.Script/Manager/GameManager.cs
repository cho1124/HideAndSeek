using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{

    public static GameManager instance = null;

    [Header("�帧ó��")]
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
            // �÷��̾� �� ���� �̺�Ʈ�� UI ������Ʈ �޼��� ����
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
        // �ʱ�ȭ ���� (��: ���� ���� �� �÷��̾� �ʱ�ȭ, Ÿ�̸� ���� ��)
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
            RpcGameOver("Hider Win!"); // Ŭ���̾�Ʈ�鿡�� ���� ���� �˸�
        }
        else
        {
            timer -= Time.deltaTime;
            //Debug.Log($"Server: Timer updated to {timer}"); // ���� �α� �߰�
        }
    }

    private void ChangeHookTimer(float oldvalue, float newvalue)
    {
        //Debug.Log("timer : " + newvalue);
        UpdateTimerUI(newvalue); // Ÿ�̸� ���� ����� �� UI ������Ʈ
    }

    private void UpdateTimerUI(float time)
    {
        Timer_UI.text = ((int)time).ToString(); // Ÿ�̸Ӹ� ������ ��ȯ�Ͽ� �ؽ�Ʈ ������Ʈ

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
        // �߰����� ���� ���� ó��
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
