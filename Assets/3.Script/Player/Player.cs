using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int hp_max;
    private int hp_current;
    public bool is_dead = false;
    public bool is_seeker = false;
    
    
    public void Initialize(int max_hp, bool is_seeker)
    {
        hp_current = max_hp;

    }

    public void TakeDamage(int damage)
    {
        hp_current -= damage;

        if (hp_current <= 0)
        {
            Die();
        }

    }

    public void Die()
    {

        is_dead = true;
        GameManager.instance.PlayerDied(gameObject);

        //���� Ȥ�� ������ �ƴ� ��츦 ���ӸŴ������� �޾ƿͼ� �� ��
        //GameManager.instance.
    }


    private void Update()
    {
        
    }

}
