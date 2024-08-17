using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Control : MonoBehaviour
{
    /*

    */


    [SerializeField] private Animator animator;
    [SerializeField] private Player_Control player_control;
    [SerializeField] GameObject Hammer;
    private Lovely_Hammer lovely_hammer;
    private float move_h = 0f;
    private float move_v = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player_control = GetComponentInParent<Player_Control>();
        lovely_hammer = Hammer.GetComponent<Lovely_Hammer>();  // Hammer 오브젝트에 붙어있는 Lovely_Hammer 스크립트를 가져옴
    }

    private void FixedUpdate()
    {
        // 공격 상태에서의 처리
        if (player_control.is_clicked) {
            animator.SetBool("Attack", true);
        }
        else {
            animator.SetBool("Attack", false);
        }

        move_h = Mathf.Lerp(move_h, player_control.input_move_h, 0.1f);
        move_v = Mathf.Lerp(move_v, player_control.input_move_v, 0.1f);

        animator.SetFloat("Velocity_X", move_h);
        animator.SetFloat("Velocity_Y", move_v);
        animator.SetBool("Is_Jumping", player_control.is_jumping);
        animator.SetBool("Is_Ground", player_control.is_ground);

    }

    // 애니메이션 이벤트를 사용해 콜라이더를 켜고 끄는 메서드 Stable Sword Outward Slash 애니메이터에 이벤트 추가해둠
    public void EnableCollider() {
        lovely_hammer.Collider_On();
    }

    public void DisableCollider() {
        lovely_hammer.Collider_Off();
    }
}
