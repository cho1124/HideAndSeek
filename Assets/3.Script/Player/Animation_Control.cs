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
        lovely_hammer = Hammer.GetComponent<Lovely_Hammer>();  // Hammer ������Ʈ�� �پ��ִ� Lovely_Hammer ��ũ��Ʈ�� ������
    }

    private void FixedUpdate()
    {
        // ���� ���¿����� ó��
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

    // �ִϸ��̼� �̺�Ʈ�� ����� �ݶ��̴��� �Ѱ� ���� �޼��� Stable Sword Outward Slash �ִϸ����Ϳ� �̺�Ʈ �߰��ص�
    public void EnableCollider() {
        lovely_hammer.Collider_On();
    }

    public void DisableCollider() {
        lovely_hammer.Collider_Off();
    }
}
