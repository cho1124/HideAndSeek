using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("흐름처리")]
    [SerializeField] private float timer = 0f;

    [Header("카운트")]
    [SerializeField] private int seeker_count = 1;
    
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
    


    //todo >>>>> 게임이 시작 될 때 술래인 유저는 player_seek, 술래가 아닌 유저는 player_hide에 list add 하고
    //플레이어가 죽을 때마다 List에서 제거
    //리스트가 먼저 0이 된 팀이 패배
    //혹은 타임 아웃 되면 술래 패배
    

    private void Awake()
    {
        
        
    }

    private void Start()
    {
        //1. 이전 서버에서 방 멤버 기준으로 난수 돌리기, 1 / n 확률로 술래
        //2. 난수 돌린 기준으로 각 리스트에 할당
        //3. hide 플레이어는 처음에 랜덤 오브젝트 할당
        //4. case 1 : 3분 동안 1분마다 플레이어의 오브젝트가 랜덤으로 다시 바뀐다.
        //   case 2 : 술래가 아닌 플레이어에게 짧은 레이캐스트를 달고 그 레이캐스트에 감지된 변신 가능한 오브젝트가 있으면 그 오브젝트로 변신
        //5. 

        for(int i = 0; i < seeker_count; i++)
        {

            Player player = Instantiate(player_prefab, seeker_spawnpoint).AddComponent<Player>();
            player.Initialize(100, true);
            player_seek.Add(player);
        }

        for(int i = 0; i < player_hide.Count; i++) //이 부분은 멀티 플레이어에서 유저의 카운트를 받아온 다음에 seeker_count를 뺀 나머지 값으로 계산하라 수 있도록 할 예정
        {
            Player player = Instantiate(object_prefab[i], hider_spawnpoint).AddComponent<Player>(); //여기서 바꿔야 할 부분, object 프리팹을 난수화
            player.Initialize(5, false);
            player_hide.Add(player);
        }

    }

    // Update is called once per frame
    private void Update()
    {
        if (timer <= 0)
        {
            timer = 0;
            //GameOver;
            return;
        }

        timer -= Time.deltaTime;
    }
}
