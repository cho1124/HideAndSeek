using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{

    public static GameManager instance = null;

    [Header("�帧ó��")]
    public float timer = 180f;
    private bool isGameOver = false;

    [Header("�� �÷��̾� ����Ʈ")]
    public List<GameObject> playerSeek = new List<GameObject>();
    public List<GameObject> playerHide = new List<GameObject>();
    public List<GameObject> deadPlayers = new List<GameObject>();

    [Header("��������Ʈ")]
    [SerializeField] private Transform seekerSpawnpoint;
    [SerializeField] private Transform hiderSpawnpoint;

    [Header("�� �÷��̾� ������")]
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
        

        // �ʱ�ȭ ���� (��: ���� ���� �� �÷��̾� �ʱ�ȭ, Ÿ�̸� ���� ��)
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
        // �÷��̾ ���� �Ǵ� ���� ������� �й�
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
            RpcGameOver("Hider Win!"); // Ŭ���̾�Ʈ�鿡�� ���� ���� �˸�
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

        CheckGameEndCondition(); // ���� ���� ���� üũ
    }

    
    private void CheckGameEndCondition()
    {
        if (playerSeek.Count == 0)
        {
            RpcGameOver("Hider Win!"); // ���� ���� �����ϸ� ���� �� �¸�
        }
        else if (playerHide.Count == 0)
        {
            RpcGameOver("Seeker Win!"); // ���� ���� �����ϸ� ���� �� �¸�
        }
    }

    
    private void RpcGameOver(string result)
    {
        Debug.Log(result);
        Time.timeScale = 0;
        // �߰����� ���� ���� ó�� (��: UI ������Ʈ)
    }
}
