using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("흐름처리")]
    [SerializeField] private float timer = 0f;

    [SerializeField] private List<Player> player_seek;
    [SerializeField] private List<Player> player_hide;
    
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
