using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("�帧ó��")]
    [SerializeField] private float timer = 0f;

    [Header("ī��Ʈ")]
    [SerializeField] private int seeker_count = 1;
    
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
    


    //todo >>>>> ������ ���� �� �� ������ ������ player_seek, ������ �ƴ� ������ player_hide�� list add �ϰ�
    //�÷��̾ ���� ������ List���� ����
    //����Ʈ�� ���� 0�� �� ���� �й�
    //Ȥ�� Ÿ�� �ƿ� �Ǹ� ���� �й�
    

    private void Awake()
    {
        
        
    }

    private void Start()
    {
        //1. ���� �������� �� ��� �������� ���� ������, 1 / n Ȯ���� ����
        //2. ���� ���� �������� �� ����Ʈ�� �Ҵ�
        //3. hide �÷��̾�� ó���� ���� ������Ʈ �Ҵ�
        //4. case 1 : 3�� ���� 1�и��� �÷��̾��� ������Ʈ�� �������� �ٽ� �ٲ��.
        //   case 2 : ������ �ƴ� �÷��̾�� ª�� ����ĳ��Ʈ�� �ް� �� ����ĳ��Ʈ�� ������ ���� ������ ������Ʈ�� ������ �� ������Ʈ�� ����
        //5. 

        for(int i = 0; i < seeker_count; i++)
        {

            Player player = Instantiate(player_prefab, seeker_spawnpoint).AddComponent<Player>();
            player.Initialize(100, true);
            player_seek.Add(player);
        }

        for(int i = 0; i < player_hide.Count; i++) //�� �κ��� ��Ƽ �÷��̾�� ������ ī��Ʈ�� �޾ƿ� ������ seeker_count�� �� ������ ������ ����϶� �� �ֵ��� �� ����
        {
            Player player = Instantiate(object_prefab[i], hider_spawnpoint).AddComponent<Player>(); //���⼭ �ٲ�� �� �κ�, object �������� ����ȭ
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
