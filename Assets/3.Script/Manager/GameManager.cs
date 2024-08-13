using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("흐름처리")]
    [SerializeField] private float timer = 180f;

    [Header("변동 가능 카운트")]
    [SerializeField] private int seeker_count = 1;

    [Header("인 게임 생존 카운트")]
    [SerializeField] private int seeker_survived_count;
    [SerializeField] private int hider_survived_count;
    
    [Header("각 플레이어 리스트")]
    [SerializeField] private List<Player> player_seek;
    [SerializeField] private List<Player> player_hide;

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject player_prefab;

    [Header("오브젝트 프리팹")]
    [SerializeField] private List<GameObject> object_prefab;

    [Header("스폰포인트")]
    [SerializeField] private Transform seeker_spawnpoint;
    [SerializeField] private Transform hider_spawnpoint;


    private HideAndSeekRoomManager roomManager;

    private bool is_gameover = false;

    //todo: >>>>> 게임이 시작 될 때 술래인 유저는 player_seek, 술래가 아닌 유저는 player_hide에 list add 하고
    
    //오잉
    //플레이어가 죽을 때마다 List에서 제거
    //리스트가 먼저 0이 된 팀이 패배
    //혹은 타임 아웃 되면 술래 패배
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroy할지 안할지는 
        }
        else
        {
            Destroy(gameObject);
            return;
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

    private void Start()
    {
        roomManager = FindAnyObjectByType<HideAndSeekRoomManager>();

        Set_Player(player_seek, seeker_count ,100, true);
        Set_Player(player_hide, roomManager.clientIndex - seeker_count ,5, false); //이 부분을 네트워크에서 받아올 예정
        Time.timeScale = 1;
        
        foreach(KeyValuePair<uint, NetworkIdentity> dic in NetworkServer.spawned)
        {
            Debug.Log("dic netid : " + dic.Key + "dic value : " + dic.Value);
        }

        //Debug.LogError(NetworkServer.spawned[0].netId);


        //Debug.Log(NetworkManager.singleton.spawnPrefabs.Count);

        
    }


    private void Update()
    {
        Check_Timer();

    }

    private void Check_Timer()
    {
        if (timer <= 0 && !is_gameover)
        {
            //GameOver;
            Debug.Log("Hider Win!");
            GameOver();
            
        }
        else
        {
            timer -= Time.deltaTime;
        }

    }

    private void GameOver()
    {
        is_gameover = true;
        Time.timeScale = 0; // 게임을 일시정지
                            // 추가적인 게임 종료 처리 로직
    }

    private void Set_Player(List<Player> players,int player_count, int hp, bool is_seeker)
    {

        Transform spawnPoint = is_seeker ? seeker_spawnpoint : hider_spawnpoint;

        for (int i = 0; i < player_count; i++) //이 부분은 멀티 플레이어에서 유저의 카운트를 받아온 다음에 seeker_count를 뺀 나머지 값으로 계산하라 수 있도록 할 예정
        {
            //Player player = Instantiate(object_prefab[0], spawnPoint).AddComponent<Player>(); //여기서 바꿔야 할 부분, object 프리팹을 난수화
            Player player = NetworkManager.singleton.spawnPrefabs[0].AddComponent<Player>();
            player.Initialize(hp, is_seeker);
            players.Add(player);
        }
    }

    public void Morph(GameObject player, int prefab_num)
    {
        GameObject player_body = player.GetComponent<Player_Control>().player_body;
        Destroy(player_body);
        player_body = Instantiate(object_prefab[prefab_num]);
        player_body.transform.SetParent(player.transform);
        player_body.transform.localPosition = Vector3.zero;
        player_body.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
