using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Animation_Control : NetworkBehaviour
{
    /*
    ������ �ݶ��̴� Ű�� ���� �޼��尡 �ظӿ� �޷��ִ� ��ũ��Ʈ�� �ִµ�, �׷��� �ϸ� �ִϸ��̼� �̺�Ʈ ȣ���� �� 
    ã�� �� ���ٰ� ���ͼ� �÷��̾ ������ �ִ� �� ��ũ��Ʈ���� �ظ� ��ũ��Ʈ�� �����ͼ� �ű��ִ� �޼��带 ȣ���ؾ���



    */

    [SerializeField] private Animator animator;
    [SerializeField] private Player_Control player_control;
    [SerializeField] GameObject Hammer;
    private Lovely_Hammer lovely_hammer; // Lovely_Hammer ��ũ��Ʈ ����
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


    // �ִϸ��̼� �̺�Ʈ�� ���� ȣ��� �޼���
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
        //���⼭ ����� �ǰ� �޼ҵ� ȣ��
    }
}
