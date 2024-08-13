using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    [Header("�帧ó��")]
    [SerializeField] private float timer = 0f;

    [Header("���� ���� ī��Ʈ")]
    [SerializeField] private int seeker_count = 1;

    [Header("�� ���� ���� ī��Ʈ")]
    [SerializeField] private int seeker_survivedCount;
    [SerializeField] private int hider_survivedCount;
    
    [Header("�� �÷��̾� ����Ʈ")]
    [SerializeField] private List<Player> player_seek;
    [SerializeField] private List<Player> player_hide;

    [Header("�÷��̾� ������")]
    [SerializeField] private GameObject player_prefab;

    [Header("������Ʈ ������")]
    [SerializeField] private List<GameObject> object_prefab;

    [Header("��������Ʈ")]
    [SerializeField] private Transform seeker_spawnpoint;
    [SerializeField] private Transform hider_spawnpoint;

    private bool is_gameover = false;

    //todo: >>>>> ������ ���� �� �� ������ ������ player_seek, ������ �ƴ� ������ player_hide�� list add �ϰ�
    //TODO: ������ ��õ�� ��� �������̾� ����ε�
    //����
    //�÷��̾ ���� ������ List���� ����
    //����Ʈ�� ���� 0�� �� ���� �й�
    //Ȥ�� Ÿ�� �ƿ� �Ǹ� ���� �й�
    

    private void Awake()
    {
        if(instance == null)
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
        Set_Player(player_seek, seeker_count ,100, true);
        Set_Player(player_hide, 10 - seeker_count ,5, false); //�� �κ��� ��Ʈ��ũ���� �޾ƿ� ����
        Time.timeScale = 1;
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

    private void Set_Player(List<Player> players,int player_count, int hp, bool is_seeker)
    {

        Transform spawnPoint = is_seeker ? seeker_spawnpoint : hider_spawnpoint;

        for (int i = 0; i < player_count; i++) //�� �κ��� ��Ƽ �÷��̾�� ������ ī��Ʈ�� �޾ƿ� ������ seeker_count�� �� ������ ������ ����϶� �� �ֵ��� �� ����
        {
            Player player = Instantiate(object_prefab[i], spawnPoint).AddComponent<Player>(); //���⼭ �ٲ�� �� �κ�, object �������� ����ȭ
            player.Initialize(hp, is_seeker);
            player_hide.Add(player);
        }
    }

}
