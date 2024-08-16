using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{

    public static GameManager instance = null;

    [Header("흐름처리")]
    public float timer = 180f;
    private bool isGameOver = false;

    [Header("각 플레이어 리스트")]
    public List<GameObject> playerSeek = new List<GameObject>();
    public List<GameObject> playerHide = new List<GameObject>();
    public List<GameObject> deadPlayers = new List<GameObject>();

    [Header("스폰포인트")]
    [SerializeField] private Transform seekerSpawnpoint;
    [SerializeField] private Transform hiderSpawnpoint;

    [Header("각 플레이어 프리팹")]
    [SerializeField] private GameObject seeker_obj;
    [SerializeField] private List<GameObject> hider_obj;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        
        InitializeGame();
    }


    private void InitializeGame()
    {
        

        // 초기화 로직 (예: 게임 시작 시 플레이어 초기화, 타이머 설정 등)
        Time.timeScale = 1;
        timer = 180f;
        isGameOver = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "GameplayScene") return;

        CheckTimer();
    }

    
    public void AddPlayerToGame(GameObject player)
    {
        // 플레이어를 술래 또는 숨는 사람으로 분배
        if (playerSeek.Count == 0)
        {
            playerSeek.Add(player);
            player.GetComponent<Player_Control>().Initiallize_Player(seeker_obj, seekerSpawnpoint);
        }
        else
        {
            playerHide.Add(player);
            player.GetComponent<Player_Control>().Initiallize_Player(hider_obj[Random.Range(0, hider_obj.Count)], hiderSpawnpoint);
        }
    }

    
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
        }
    }

    
    public void PlayerDied(GameObject player)
    {
        if (playerSeek.Contains(player))
        {
            playerSeek.Remove(player);
        }
        else if (playerHide.Contains(player))
        {
            playerHide.Remove(player);
        }

        deadPlayers.Add(player);

        CheckGameEndCondition(); // 게임 종료 조건 체크
    }

    
    private void CheckGameEndCondition()
    {
        if (playerSeek.Count == 0)
        {
            RpcGameOver("Hider Win!"); // 술래 팀이 전멸하면 숨는 팀 승리
        }
        else if (playerHide.Count == 0)
        {
            RpcGameOver("Seeker Win!"); // 숨는 팀이 전멸하면 술래 팀 승리
        }
    }

    
    private void RpcGameOver(string result)
    {
        Debug.Log(result);
        Time.timeScale = 0;
        // 추가적인 게임 종료 처리 (예: UI 업데이트)
    }
}
