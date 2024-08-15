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

    [Header("�÷��̾� ������")]
    [SerializeField] private GameObject playerPrefab;

    [Header("������Ʈ ������")]
    [SerializeField] private List<GameObject> objectPrefab;

    [Header("��������Ʈ")]
    [SerializeField] private Transform seekerSpawnpoint;
    [SerializeField] private Transform hiderSpawnpoint;

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

    
    public void AddPlayer(GameObject player)
    {
        playerSeek.Add(player);
    }

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

    [Command]
    public void CmdMorph(GameObject player, int prefabNum)
    {
        RpcMorph(player, prefabNum);
    }

    [ClientRpc]
    private void RpcMorph(GameObject player, int prefabNum)
    {
        GameObject playerBody = player.GetComponent<Player_Control>().player_body;
        Destroy(playerBody);

        playerBody = Instantiate(objectPrefab[prefabNum], player.transform);
        playerBody.transform.localPosition = Vector3.zero;
        playerBody.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
