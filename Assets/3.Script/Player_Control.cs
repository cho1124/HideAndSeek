using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{

    [SerializeField] float move_speed = 5f;
    [SerializeField] float jump_speed = 20f; // 점프에 사용할 힘

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform anchor_transform;
    [SerializeField] private GameObject player_body;

    float input_cursor_h = 0f;
    float input_cursor_v = 0f;

    float input_move_h = 0f;
    float input_move_v = 0f;
    bool input_jump = false;

    Vector3 velocity_h = Vector3.zero;
    Vector3 velocity_v = Vector3.zero;

    private bool is_ground = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anchor_transform = transform.Find("Root_Anchor");
    }

    void Update()
    {

        input_move_h = Input.GetAxisRaw("Horizontal");
        input_move_v = Input.GetAxisRaw("Vertical");

        input_cursor_h = Input.GetAxis("Mouse X");
        input_cursor_v = Input.GetAxis("Mouse Y");

        // 스페이스 바를 눌렀을 때 점프
        input_jump = Input.GetKeyDown(KeyCode.Space);

        if (Input.GetMouseButton(0))
        {
            On_Click();
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(is_ground);
        Player_Move(input_move_h, input_move_v, input_jump);
        Player_Rotate(input_cursor_h, input_cursor_v);
    }

    private void Player_Move(float move_h, float move_v, bool is_jump)
    {
        Vector3 direction = (transform.right * move_h + transform.forward * move_v).normalized;
        velocity_h = direction * move_speed;
        if (is_ground && is_jump)
        {
            velocity_v = transform.up * jump_speed;
            is_ground = false;
        }
        else if (is_ground && !is_jump) velocity_v = Vector3.zero;
        else velocity_v = Vector3.Lerp(velocity_v, Physics.gravity, Time.deltaTime);
        rb.velocity = new Vector3(velocity_h.x, velocity_v.y, velocity_h.z);
    }

    private void Player_Rotate(float cursor_h, float cursor_v)
    {
        anchor_transform.Rotate(new Vector3(cursor_v * 1.5f, -cursor_h * 1.5f, 0f));
        Quaternion anchor_rotation = anchor_transform.rotation;

        transform.rotation = Quaternion.Euler(0f, anchor_transform.rotation.eulerAngles.y, 0f);
        anchor_transform.rotation = anchor_rotation;
    }

    private void On_Click()
    {
        RaycastHit[] ray_hits = Physics.RaycastAll(anchor_transform.position, anchor_transform.forward, 10f);
        for (int i = 0; i < ray_hits.Length; i++)
        {
            Debug.Log($"{ray_hits[i].collider.gameObject.name}");
            if (ray_hits[i].collider.CompareTag("Changeable"))
            {

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(anchor_transform.position, anchor_transform.forward * 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (velocity_v.y < 0f && collision.gameObject.layer == 8 && (collision.transform.position - transform.position).y <= 1f)
        {
            is_ground = true;
        }
    }
}
