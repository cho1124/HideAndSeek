/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Animation_Control : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Player_Control player_control;
    [SerializeField] GameObject Hammer;
    private NetworkIdentity id;

    [SyncVar]
    private float move_h = 0f;
    [SyncVar]
    private float move_v = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player_control = GetComponentInParent<Player_Control>();
        id = GetComponent<NetworkIdentity>();


    }


    private void FixedUpdate()
    {
        animator.SetBool("Attack", false);
        
        move_h = Mathf.Lerp(move_h, player_control.input_move_h, 0.1f);
        move_v = Mathf.Lerp(move_v, player_control.input_move_v, 0.1f);
        
        animator.SetFloat("Velocity_X", move_h);
        animator.SetFloat("Velocity_Y", move_v);
        animator.SetBool("Is_Jumping", player_control.is_jumping);
        animator.SetBool("Is_Ground", player_control.is_ground);
        
        animator.SetBool("Attack", player_control.is_clicked);
        //ClientAnimatorControl();
    }

    [Client]
    void ClientAnimatorControl()
    {
        CMDAnimatorControl();
    }

    [Command]
    void CMDAnimatorControl()
    {
        RPCAnimatorControl();
    }

    [ClientRpc]
    void RPCAnimatorControl()
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


    public void On_Attack()
    {
        //여기서 상대의 피격 메소드 호출
    }
}
*/
using UnityEngine;
using Mirror;

public class Animation_Control : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Player_Control player_control;
    private NetworkIdentity id;

    [SyncVar(hook = nameof(OnMoveChanged))]
    private float move_h = 0f;

    [SyncVar(hook = nameof(OnMoveChanged))]
    private float move_v = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player_control = GetComponentInParent<Player_Control>();
        id = GetComponent<NetworkIdentity>();
    }

    private void FixedUpdate()
    {


            CmdUpdateAnimatorParameters(
                player_control.input_move_h,
                player_control.input_move_v,
                player_control.is_jumping,
                player_control.is_ground,
                player_control.is_clicked
            );
        
    }

    //[Command]
    void CmdUpdateAnimatorParameters(float h, float v, bool isJumping, bool isGround, bool isClicked)
    {
        move_h = Mathf.Lerp(move_h, h, 0.1f);
        move_v = Mathf.Lerp(move_v, v, 0.1f);

        RpcUpdateAnimator(move_h, move_v, isJumping, isGround, isClicked);
    }

    //[ClientRpc]
    void RpcUpdateAnimator(float h, float v, bool isJumping, bool isGround, bool isClicked)
    {
        animator.SetFloat("Velocity_X", h);
        animator.SetFloat("Velocity_Y", v);
        animator.SetBool("Is_Jumping", isJumping);
        animator.SetBool("Is_Ground", isGround);
        animator.SetBool("Attack", isClicked);
    }

    private void OnMoveChanged(float oldValue, float newValue)
    {
        // SyncVar가 업데이트되었을 때 클라이언트에서 추가 작업이 필요할 경우 이 메서드를 사용
    }
}
