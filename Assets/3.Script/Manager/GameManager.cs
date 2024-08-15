using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance = null;

    [Header("흐름처리")]
    [SyncVar] public float timer = 180f;
    [SyncVar] private bool isGameOver = false;

    [Header("변동 가능 카운트")]
    [SyncVar] private int seekerCount = 1;

    [Header("인 게임 생존 카운트")]
    [SyncVar] private int seekerSurvivedCount;
    [SyncVar] private int hiderSurvivedCount;

    [Header("각 플레이어 리스트")]
    [SyncVar] public List<GameObject> playerSeek = new List<GameObject>();
    [SyncVar] public List<GameObject> playerHide = new List<GameObject>();

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject playerPrefab;

    [Header("오브젝트 프리팹")]
    [SerializeField] private List<GameObject> objectPrefab;

    [Header("스폰포인트")]
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

    //todo: >>>>> 게임이 시작 될 때 술래인 유저는 player_seek, 술래가 아닌 유저는 player_hide에 list add 하고

    //플레이어가 죽을 때마다 List에서 제거
    //리스트가 먼저 0이 된 팀이 패배
    //혹은 타임 아웃 되면 술래 패배v

    private void Start()
    {
        
        roomManager = FindAnyObjectByType<HideAndSeekRoomManager>();
        if (isServer)
        {
            Time.timeScale = 1;
        }
    }

    //1. 이전 서버에서 방 멤버 기준으로 난수 돌리기, 1 / n 확률로 술래
    //2. 난수 돌린 기준으로 각 리스트에 할당
    //3. hide 플레이어는 처음에 랜덤 오브젝트 할당
    //4. case 1 : 3분 동안 1분마다 플레이어의 오브젝트가 랜덤으로 다시 바뀐다.
    //   case 2 : 술래가 아닌 플레이어에게 짧은 레이캐스트를 달고 그 레이캐스트에 감지된 변신 가능한 오브젝트가 있으면 그 오브젝트로 변신
    //5. 리스트에 대한 부분은 맨 처음에 할당할 때 빼고는 빼거나 더하지 않을 예정, 이유로는 플레이어가 죽으면 관전자 모드로 들어갈 수 있도록 하기 위해서
    //6. 따라서 게임매니저를 단순한 static 혹은 싱글턴을 이용하여 플레이어 생존 카운트를 처리할 수 있도록 해도 될 거 같긴 한데
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
        Time.timeScale = 0; // 게임 일시정지
        // 추가적인 게임 종료 처리 로직
    }

    [Command]
    public void CmdPlayerSet()
    {
        Set_Player(playerSeek, seekerCount, 100, true);
        Set_Player(playerHide, roomManager.clientIndex - seekerCount, 5, false); //이 부분을 네트워크에서 받아올 예정
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

            // 플레이어 초기화 및 리스트 추가
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
