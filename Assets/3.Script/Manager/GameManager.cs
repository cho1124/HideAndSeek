using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("�帧ó��")]
    [SerializeField] private float timer = 180f;
    private bool is_gameover = false;

    [Header("���� ���� ī��Ʈ")]
    [SerializeField] private int seeker_count = 1;

    [Header("�� ���� ���� ī��Ʈ")]
    [SerializeField] private int seeker_survived_count;
    [SerializeField] private int hider_survived_count;

    [Header("�� �÷��̾� ����Ʈ")]
    //[SerializeField] public List<Player> player_seek;
    //[SerializeField] public List<Player> player_hide;
    //public List<Player> test_player;
    [SerializeField] public List<GameObject> player_seek;
    [SerializeField] public List<GameObject> player_hide;
    public List<GameObject> test_player;


    [Header("�÷��̾� ������")]
    [SerializeField] private GameObject player_prefab;

    [Header("������Ʈ ������")]
    [SerializeField] private List<GameObject> object_prefab;

    [Header("��������Ʈ")]
    [SerializeField] private Transform seeker_spawnpoint;
    [SerializeField] private Transform hider_spawnpoint;

    private HideAndSeekRoomManager roomManager;


    //todo: >>>>> ������ ���� �� �� ������ ������ player_seek, ������ �ƴ� ������ player_hide�� list add �ϰ�

    //�÷��̾ ���� ������ List���� ����
    //����Ʈ�� ���� 0�� �� ���� �й�
    //Ȥ�� Ÿ�� �ƿ� �Ǹ� ���� �й�

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroy���� �������� 
        }
        else
        {
            Destroy(gameObject);
            return;
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

    private void Start()
    {
        roomManager = FindAnyObjectByType<HideAndSeekRoomManager>();

        //Set_Player(player_seek, seeker_count, 100, true);
        //Set_Player(player_hide, roomManager.clientIndex - seeker_count, 5, false); //�� �κ��� ��Ʈ��ũ���� �޾ƿ� ����
        Time.timeScale = 1;

        Debug.Log("�����÷��̾� ����Ʈ :  " + roomManager.gamePlayer_list.Count);

        //Debug.LogError(NetworkServer.spawned[0].netId);
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
        Time.timeScale = 0; // ������ �Ͻ�����
                            // �߰����� ���� ���� ó�� ����
    }

    private void Set_Player(List<Player> players, int player_count, int hp, bool is_seeker)
    {

        Transform spawnPoint = is_seeker ? seeker_spawnpoint : hider_spawnpoint;

        for (int i = 0; i < player_count; i++) //�� �κ��� ��Ƽ �÷��̾�� ������ ī��Ʈ�� �޾ƿ� ������ seeker_count�� �� ������ ������ ����϶� �� �ֵ��� �� ����
        {
            Player player = Instantiate(object_prefab[0], spawnPoint).AddComponent<Player>(); //���⼭ �ٲ�� �� �κ�, object �������� ����ȭ �߰��� �����ϴ� �� �ƴ� siball �������� �޾Ƽ� ó���� ��

            //Player player = 
            player.transform.position = spawnPoint.position;
            
            
            player.Initialize(hp, is_seeker);
            players.Add(player);
        }
    }

    public void Morph(GameObject player, int prefab_num)
    {
        GameObject player_body = player.GetComponent<Player_Control>().player_body;
        Destroy(player_body);
        player_body = 
            (object_prefab[prefab_num]);
        player_body.transform.SetParent(player.transform);
        player_body.transform.localPosition = Vector3.zero;
        player_body.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}

