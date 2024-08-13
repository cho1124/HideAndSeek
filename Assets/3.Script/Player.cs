using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int max_hp;
    private int current_hp;
    public bool isDead = false;
    public bool is_seeker = false;
    
    
    public void Initialize(int max_hp, bool is_seeker)
    {
        current_hp = max_hp;
        
    }




    public void TakeDamage(int damage)
    {
        current_hp -= damage;

        if (current_hp <= 0)
        {
            Die();
        }

    }

    public void Die()
    {

        isDead = true;

        //술래 혹은 술래가 아닌 경우를 게임매니저에서 받아와서 할 것
        //GameManager.instance.
    }


    private void Update()
    {
        
    }

}
