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

        //술래 혹은 술래가 아닌 경우를 게임매니저에서 받아와서 할 것
        //GameManager.instance.
    }


    private void Update()
    {
        
    }

}
