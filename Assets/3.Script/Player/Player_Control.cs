using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player_Control : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform anchor_transform;
    [SerializeField] private GameObject player_prefab;
    [SerializeField] private GameObject main_camera;
    [SerializeField] public Animator player_ani;

    [SerializeField] float move_speed = 5f;
    [SerializeField] float jump_speed = 5f; // 점프에 사용할 힘

    public float input_cursor_h, input_cursor_v, input_move_h, input_move_v;
    public bool is_clicked = false;
    public bool input_jump = false;
    public bool is_jumping = false;
    public bool is_ground = false;

    private float move_h = 0f;
    private float move_v = 0f;
    private Vector3 last_contact = new Vector3();

    private Vector3 velocity_h = Vector3.zero;
    private Vector3 velocity_v = Vector3.zero;
    private Coroutine jumpCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anchor_transform = transform.Find("Root_Anchor");
        main_camera = GameObject.Find("Main_Camera");

        if (!player_ani)
        {
            player_ani = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return; //You shall not pass!!!
        }

        //키보드 및 마우스 입력은 Update에서, 처리는 FixedUpdate에서.
        input_move_h = Input.GetAxisRaw("Horizontal");
        input_move_v = Input.GetAxisRaw("Vertical");
        input_cursor_h = Input.GetAxis("Mouse X");
        input_cursor_v = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.Space) && jumpCoroutine == null)
        {
            jumpCoroutine = StartCoroutine(Jump_Co());
        }

        

        Player_MoveAni();
        is_clicked = Input.GetMouseButtonDown(0);
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        if (is_clicked)
        {
            Morph();
        }

        Player_Move(input_move_h, input_move_v, input_jump);
        Player_Rotate(input_cursor_h, input_cursor_v);
    }

    private void Player_MoveAni()
    {
        if (player_ani == null) return;

        player_ani.SetBool("Attack", false);

        move_h = Mathf.Lerp(move_h, input_move_h, 0.1f);
        move_v = Mathf.Lerp(move_v, input_move_v, 0.1f);

        player_ani.SetFloat("Velocity_X", move_h);
        player_ani.SetFloat("Velocity_Y", move_v);
        player_ani.SetBool("Is_Jumping", is_jumping);
        player_ani.SetBool("Is_Ground", is_ground);

        player_ani.SetBool("Attack", is_clicked);
    }

    private void Player_Move(float move_h, float move_v, bool input_jump)
    {
        Vector3 direction = (transform.right * move_h + transform.forward * move_v).normalized;
        velocity_h = direction * move_speed;

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
        else
        {
            velocity_v = Vector3.Lerp(velocity_v, Physics.gravity, Time.deltaTime);
        }

        rb.velocity = velocity_h + velocity_v;
    }

    private void Player_Rotate(float cursor_h, float cursor_v)
    {
        float target_x = anchor_transform.eulerAngles.x - cursor_v;
        if (85f < target_x && target_x < 180f) target_x = 85f;
        else if (180f < target_x && target_x < 275f) target_x = 275f;

        anchor_transform.rotation = Quaternion.Euler(target_x, anchor_transform.eulerAngles.y + cursor_h, 0f);

        Quaternion anchor_rotation = anchor_transform.rotation;
        transform.rotation = Quaternion.Euler(0f, anchor_transform.rotation.eulerAngles.y, 0f);
        anchor_transform.rotation = anchor_rotation;

        main_camera.transform.position = anchor_transform.position + anchor_transform.forward * -7f;
        main_camera.transform.LookAt(anchor_transform.position);
    }

    private void Morph()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(anchor_transform.position, anchor_transform.forward), out hit, 10f))
        {
            if (hit.collider.CompareTag("Morphable"))
            {
                //GameManager.instance.Morph(gameObject, hit.collider.gameObject.GetComponent<Morphable_Object>().prefab_num);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (var contact in collision.contacts)
        {
            if (contact.point.y < transform.position.y + 0.1f && !is_jumping)
            {
                is_ground = true;
                break;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].point.y < transform.position.y + 0.05f)
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
        jumpCoroutine = null;
    }
}
