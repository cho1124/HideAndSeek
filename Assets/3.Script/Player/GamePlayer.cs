using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;

public class GamePlayer : NetworkBehaviour
{

    [SyncVar] public string nickName;
    [SyncVar] public string ip;

    public bool is_dead = false;
    public bool is_seeker = false;

    // UI 업데이트용 이벤트 (LocalPlayer에서만 리스너 연결)
    public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();

    [Header("Sync Variables")]
    [SyncVar(hook = nameof(OnTeamChanged))]
    public int teamId;


    [SyncVar(hook = nameof(OnHealthChangedHook))]
    private int hp_current;

    [SyncVar(hook = nameof(OnRandomNumberChanged))]
    private int randomNumber;

    [Header("플레이어")]
    [SerializeField] private GameObject player_body;

    [Header("UI")]
    [SerializeField] private UIManager UI_manager;

    private HideAndSeekRoomManager room_manager;


    public override void OnStartServer()
    {
        room_manager = FindAnyObjectByType<HideAndSeekRoomManager>();


        randomNumber = UnityEngine.Random.Range(0, room_manager.hider_obj.Count);

        // 팀에 따른 기본 체력 세팅
        hp_current = (teamId == 1) ? 5 : 100;
    }

    // 클라이언트에서 객체가 스폰될 때 실행 (모든 클라이언트)
    public override void OnStartClient()
    {
        room_manager = FindAnyObjectByType<HideAndSeekRoomManager>();


        AssignPlayerBody(randomNumber);
    }

    // 로컬 플레이어(본인) 전용 초기화
    public override void OnStartLocalPlayer()
    {
        UI_manager = FindAnyObjectByType<UIManager>();


        if (UI_manager != null)
        {
            OnHealthChanged.AddListener(UI_manager.UpdatePlayerHealth);
            OnHealthChanged?.Invoke(hp_current);
        }
    }

    private void AssignPlayerBody(int randomIndex)
    {

        if (room_manager == null) return;

        if (teamId == 1)
        {
            is_seeker = false;
            player_body = Instantiate(room_manager.hider_obj[randomIndex]);
            transform.position = room_manager.hiderSpawnpoint.position;
            gameObject.tag = "Player_Hide";
        }
        else
        {
            is_seeker = true;
            player_body = Instantiate(room_manager.seeker_obj);
            transform.position = room_manager.seekerSpawnpoint.position;

            Player_Control playercon = GetComponent<Player_Control>();
            if (playercon != null)
            {
                playercon.hand = player_body.transform.Find("Hand").gameObject;
            }
        }

        if (player_body == null)
        {
            Debug.LogError("Player body instantiation failed.");
            return;
        }

        player_body.transform.SetParent(gameObject.transform);
        player_body.transform.localPosition = Vector3.zero;
    }



    void OnTeamChanged(int oldTeam, int newTeam)
    {

    }

    void OnRandomNumberChanged(int oldNumber, int newNumber)
    {

    }

    void OnHealthChangedHook(int oldHealth, int newHealth)
    {
        // 체력이 동기화되어 변경될 때마다 로컬 플레이어의 UI 이벤트 발생
        if (isLocalPlayer)
        {
            OnHealthChanged?.Invoke(newHealth);
        }
    }


    [Server]
    public void TakeDamage(int damage)
    {
        if (is_dead) return;

        Debug.Log($"서버에서 데미지 연산됨: {damage}");
        hp_current -= damage; 

        if (hp_current <= 0)
        {
            Die();
        }
    }

    [Server]
    public void Die()
    {
        is_dead = true;

        // 서버에서 Rpc를 호출하여 모든 클라이언트 화면에서 사망 처리를 하도록 지시
        RpcHandleDeath();
    }

    [ClientRpc]
    private void RpcHandleDeath()
    {
        // 래그돌 생성이나 사망 애니메이션 등 클라이언트 시각적 처리

        if (isLocalPlayer)
        {


            Debug.Log("Local player has died.");

            // 임시 종료 로직
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}