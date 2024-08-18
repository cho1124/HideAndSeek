using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct Client_Input
{
    public float cursor_h;
    public float cursor_v;
    public float move_h;
    public float move_v;
    public bool is_clicked;
    public bool jump;

    public Client_Input(float input_cursor_h, float input_cursor_v, float input_move_h, float input_move_v, bool is_clicked, bool jump)
    {
        cursor_h = input_cursor_h;
        cursor_v = input_cursor_v;
        move_h = input_move_h;
        move_v = input_move_v;
        this.is_clicked = is_clicked;
        this.jump = jump;
    }
}

public class Player_Control : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform anchor_transform;
    [SerializeField] private GameObject player_prefab;
    [SerializeField] private GameObject main_camera;
    [SerializeField] private NetworkIdentity net_ID;



    [SerializeField] float move_speed = 5f;
    [SerializeField] float jump_speed = 5f; // 점프에 사용할 힘
    public float input_cursor_h, input_cursor_v, input_move_h, input_move_v;
    public bool is_clicked = false;
    public bool input_jump = false;
    public bool is_jumping = false;
    public bool is_ground = false;
    Vector3 last_contact = new Vector3();

    Vector3 velocity_h = Vector3.zero;
    Vector3 velocity_v = Vector3.zero;

    Client_Input input;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anchor_transform = transform.Find("Root_Anchor");
        main_camera = GameObject.Find("Main_Camera");
        net_ID = GetComponent<NetworkIdentity>();

        //맨 처음에 지정된 플레이어 모델링으로 시작하고 트랜스폼 초기화해줌
        input = new Client_Input(0f, 0f, 0f, 0f, false, false);
    }

    void Update()
    {
        //if (!isLocalPlayer) return; //You shall not pass!!!

        //키보드 및 마우스 입력은 Update에서, 처리는 FixedUpdate에서.
        if (isLocalPlayer)
        {
            input.move_h = Input.GetAxisRaw("Horizontal");
            input.move_v = Input.GetAxisRaw("Vertical");
            input.cursor_h = Input.GetAxis("Mouse X");
            input.cursor_v = Input.GetAxis("Mouse Y");
            input.is_clicked = Input.GetMouseButtonDown(0);
            input.jump = Input.GetKeyDown(KeyCode.Space);

            if (net_ID != null) Input_CMD(net_ID, input);
        }
    }

    private void FixedUpdate()
    {
        Player_Move(input_move_h, input_move_v, input_jump);
        Player_Rotate(input_cursor_h, input_cursor_v);

        if (is_clicked)
        {
            Morph();
        }
    }

    [Command]
    public void Input_CMD(NetworkIdentity net_ID, Client_Input input)
    {
        if (net_ID != null && !input.Equals(null)) Input_RPC(net_ID, input);
    }

    [ClientRpc]
    public void Input_RPC(NetworkIdentity net_ID, Client_Input input)
    {
        if (this.net_ID.netId == net_ID.netId)
        {
            input_move_h = input.move_h;
            input_move_v = input.move_v;
            input_cursor_h = input.cursor_h;
            input_cursor_v = input.cursor_v;
            if (input.jump)
            {
                StopCoroutine(Jump_Co());
                StartCoroutine(Jump_Co());
            }
            is_clicked = input.is_clicked;
        }
    }




    private void Player_Move(float move_h, float move_v, bool input_jump)
    {
        //이동키 입력값에 대한 방향 벡터를 통해 수평 가속도를 구함
        Vector3 direction = (transform.right * move_h + transform.forward * move_v).normalized;
        velocity_h = direction * move_speed;

        //점프키 입력 시 수직 가속도를 직접 설정하고 점프했는지 체크
        if (input_jump && is_ground)
        {
            velocity_v = transform.up * jump_speed;
            is_ground = false;
            StartCoroutine(Jumping_Co());
        }
        else if (is_ground && !is_jumping)
        {
            velocity_v = Vector3.zero;
        }
        //그 외에 모든 상황에서는 중력 가속도 적용을 받음
        else velocity_v = Vector3.Lerp(velocity_v, Physics.gravity, Time.deltaTime);


        //if (velocity_v.y == 0f) Debug.Log($"수직 가속도 : {velocity_v.y}");
        //else Debug.Log("수직 가속도 : != 0");
        //Debug.Log($"is_ground : {is_ground}");
        //if (is_jumping) Debug.Log("is_jumping : true");
        //위에서 계산한 가속도를 합하여 적용
        rb.velocity = velocity_h + velocity_v;
    }

    private void Player_Rotate(float cursor_h, float cursor_v)
    {
        //앵커가 먼저 회전
        //anchor_transform.Rotate(new Vector3(cursor_v * 1.5f, -cursor_h * 1.5f, 0f));

        float target_x = anchor_transform.eulerAngles.x - cursor_v;
        if (85f < target_x && target_x < 180f) target_x = 85f;
        else if (180f < target_x && target_x < 275f) target_x = 275f;

        anchor_transform.rotation = Quaternion.Euler(target_x, anchor_transform.eulerAngles.y + cursor_h, 0f);
        //Debug.Log($"{anchor_transform.rotation.x}");
        //앵커 로테이션 값 저장
        Quaternion anchor_rotation = anchor_transform.rotation;

        //플레이어 회전
        transform.rotation = Quaternion.Euler(0f, anchor_transform.rotation.eulerAngles.y, 0f);
        //같이 회전해버린 앵커를 정상화
        anchor_transform.rotation = anchor_rotation;

        if (isLocalPlayer)
        {
            main_camera.transform.position = anchor_transform.position + anchor_transform.forward * -7f;
            main_camera.transform.LookAt(anchor_transform.position);
        }
    }

    private void Morph()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(new Ray(anchor_transform.position, anchor_transform.forward), out hit, 10f);
        if (hit.collider.CompareTag("Morphable"))
        {
            //GameManager.instance.Morph(gameObject, hit.collider.gameObject.GetComponent<Morphable_Object>().prefab_num);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            //콜라이더 충돌 시 접촉점의 방향을 구하고, 만약 방향이 아래 방향, 즉 땅일 경우 다시 점프 가능하도록 설정
            if ((collision.contacts[i].point - transform.position).y < 0.1f && !is_jumping)
            {
                last_contact = collision.contacts[i].point;
                is_ground = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((last_contact - transform.position).y < 0.05f)
        {
            is_ground = false;
        }
    }

    private IEnumerator Jumping_Co()
    {
        is_jumping = true;
        yield return new WaitForSeconds(0.2f);
        is_jumping = false;
    }

    private IEnumerator Jump_Co()
    {
        input_jump = true;
        yield return new WaitForSeconds(0.1f);
        input_jump = false;
    }
}
