using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Animation_Control : NetworkBehaviour
{
    /*
    무기의 콜라이더 키고 끄는 메서드가 해머에 달려있는 스크립트에 있는데, 그렇게 하면 애니메이션 이벤트 호출할 때 
    찾을 수 없다고 나와서 플레이어가 가지고 있는 이 스크립트에서 해머 스크립트를 가져와서 거기있는 메서드를 호출해야함



    */

    [SerializeField] private Animator animator;
    [SerializeField] private Player_Control player_control;
    [SerializeField] GameObject Hammer;
    private Lovely_Hammer lovely_hammer; // Lovely_Hammer 스크립트 참조
    private float move_h = 0f;
    private float move_v = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player_control = GetComponentInParent<Player_Control>();

        if (Hammer != null) {
            lovely_hammer = Hammer.GetComponent<Lovely_Hammer>();
        }
    }

    private void FixedUpdate()
    {
        if(player_control != null && !player_control.isLocalPlayer)
        {
            //Debug.Log("not local");
            return;
        }

        
    }

    private void Player_Move()
    {
        animator.SetBool("Attack", false);

        move_h = Mathf.Lerp(move_h, player_control.input_move_h, 0.1f);
        move_v = Mathf.Lerp(move_v, player_control.input_move_v, 0.1f);

        animator.SetFloat("Velocity_X", move_h);
        animator.SetFloat("Velocity_Y", move_v);
        animator.SetBool("Is_Jumping", player_control.is_jumping);
        animator.SetBool("Is_Ground", player_control.is_ground);

        animator.SetBool("Attack", player_control.is_clicked);
    }


    // 애니메이션 이벤트를 통해 호출될 메서드
    public void EnableCollider() {
        if (lovely_hammer != null) {
            lovely_hammer.Collider_On();
        }
    }

    public void DisableCollider() {
        if (lovely_hammer != null) {
            lovely_hammer.Collider_Off();
        }
    }

    public void On_Attack()
    {
        //여기서 상대의 피격 메소드 호출
    }
}
