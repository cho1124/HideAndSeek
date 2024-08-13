using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [Header("�帧ó��")]
    [SerializeField] private float timer = 0f;

    [SerializeField] private List<Player> player_seek;
    [SerializeField] private List<Player> player_hide;
    
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
