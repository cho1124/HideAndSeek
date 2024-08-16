using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance = null;

    [Header("�帧ó��")]
    [SyncVar] public float timer = 180f;
    [SyncVar] private bool isGameOver = false;

    [Header("���� ���� ī��Ʈ")]
    [SyncVar] private int seekerCount = 1;

    [Header("�� ���� ���� ī��Ʈ")]
    [SyncVar] private int seekerSurvivedCount;
    [SyncVar] private int hiderSurvivedCount;

    [Header("�� �÷��̾� ����Ʈ")]
    [SyncVar] public List<GameObject> playerSeek = new List<GameObject>();
    [SyncVar] public List<GameObject> playerHide = new List<GameObject>();
    [SyncVar] public List<GameObject> deadPlayers = new List<GameObject>();

    [Header("��������Ʈ")]
    [SerializeField] private Transform seekerSpawnpoint;
    [SerializeField] private Transform hiderSpawnpoint;

    [Header("�� �÷��̾� ������")]
    [SerializeField] private GameObject seeker_obj;
    [SerializeField] private List<GameObject> hider_obj;


    private HideAndSeekRoomManager roomManager;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }

    //todo: >>>>> ������ ���� �� �� ������ ������ player_seek, ������ �ƴ� ������ player_hide�� list add �ϰ�

    //�÷��̾ ���� ������ List���� ����
    //����Ʈ�� ���� 0�� �� ���� �й�
    //Ȥ�� Ÿ�� �ƿ� �Ǹ� ���� �й�v

    private void Start()
    {
        
        roomManager = FindAnyObjectByType<HideAndSeekRoomManager>();
        if (isServer)
        {
            Time.timeScale = 1;
        }
    }

    //1. ���� �������� �� ��� �������� ���� ������, 1 / n Ȯ���� ����
    //2. ���� ���� �������� �� ����Ʈ�� �Ҵ�
    //3. hide �÷��̾�� ó���� ���� ������Ʈ �Ҵ�
    //4. case 1 : 3�� ���� 1�и��� �÷��̾��� ������Ʈ�� �������� �ٽ� �ٲ��.
    //   case 2 : ������ �ƴ� �÷��̾�� ª�� ����ĳ��Ʈ�� �ް� �� ����ĳ��Ʈ�� ������ ���� ������ ������Ʈ�� ������ �� ������Ʈ�� ����
    //5. ����Ʈ�� ���� �κ��� �� ó���� �Ҵ��� �� ����� ���ų� ������ ���� ����, �����δ� �÷��̾ ������ ������ ���� �� �� �ֵ��� �ϱ� ���ؼ�
    //6. ���� ���ӸŴ����� �ܼ��� static Ȥ�� �̱����� �̿��Ͽ� �÷��̾� ���� ī��Ʈ�� ó���� �� �ֵ��� �ص� �� �� ���� �ѵ�
    //

    private void Update()
    {

        if (SceneManager.GetActiveScene().name != System.IO.Path.GetFileNameWithoutExtension(roomManager.GameplayScene)) return;

        if (isServer)
        {
            CheckTimer();
        }
    }

    [Server]
    public void AddPlayerToGame(GameObject player)
    {
        // ���Ƿ� �÷��̾ �й� (���� ��� ù ��° �÷��̾�� ����) >>> �� �κ� ���� ������ ���� ���� ����
        if (playerSeek.Count == 0)
        {
            playerSeek.Add(player);
            player.GetComponent<Player>().Initialize(100, true);  // hp�� 100, �� �÷��̾�� ����
            player.GetComponent<Player_Control>().Initiallize_Player(seeker_obj, seekerSpawnpoint);
            //Initialize_Player �Ұ�
        }
        else
        {
            playerHide.Add(player);
            player.GetComponent<Player>().Initialize(100, false);  // hp�� 100, �� �÷��̾�� ���� ���
            player.GetComponent<Player_Control>().Initiallize_Player(hider_obj[Random.Range(0, hider_obj.Count)], hiderSpawnpoint);
        }
    }

    [Server]
    private void CheckTimer()
    {
        if (timer <= 0 && !isGameOver)
        {
            isGameOver = true;
            RpcGameOver("Hider Win!");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    [ClientRpc]
    private void RpcGameOver(string message)
    {
        Debug.Log(message);
        Time.timeScale = 0; // ���� �Ͻ�����
        // �߰����� ���� ���� ó�� ����
    }

    [Command]
    public void CmdPlayerSet()
    {
        Set_Player(playerSeek, seekerCount, 100, true);
        Set_Player(playerHide, roomManager.clientIndex - seekerCount, 5, false); //�� �κ��� ��Ʈ��ũ���� �޾ƿ� ����
    }


    //�� �κ��� �÷��̾��� ���� ��ġ���� �Ҵ��ϴ� ������ �� ����
    [Server]
    private void Set_Player(List<GameObject> players, int playerCount, int hp, bool isSeeker)
    {
        Transform spawnPoint = isSeeker ? seekerSpawnpoint : hiderSpawnpoint;


        for (int i = 0; i < playerCount; i++)
        {
            GameObject player = null;
            NetworkServer.Spawn(player);

            // �÷��̾� �ʱ�ȭ �� ����Ʈ �߰�
            player.GetComponent<Player>().Initialize(hp, isSeeker);
            players.Add(player);
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

        CheckGameEndCondition();
    }

    [Server]
    private void CheckGameEndCondition()
    {
        //��� ���� ����� �׾��� �� ���� ����
        if (playerHide.Count == 0)
        {
            RpcEndGame("Seeker Wins!");
        }
        else if (playerSeek.Count == 0)
        {
            RpcEndGame("Hiders Win!");
        }
    }

    [ClientRpc]
    private void RpcEndGame(string result)
    {
        //��� Ŭ���̾�Ʈ���� ���� ����� �˸�
        Debug.Log(result);
        //���� ���� ȭ���� ǥ���ϰų�, ���ο� ���� �ε��� �� ����
    }


}
