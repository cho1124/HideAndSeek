using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{

    [SerializeField] float move_speed = 5f;
    [SerializeField] float jump_speed = 20f; // 점프에 사용할 힘

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform anchor_transform;
    public GameObject player_body;
    [SerializeField] private GameObject player_prefab;

    float input_cursor_h = 0f;
    float input_cursor_v = 0f;

    float input_move_h = 0f;
    float input_move_v = 0f;
    bool input_jump = false;

    Vector3 velocity_h = Vector3.zero;

    private bool is_jumpable = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anchor_transform = transform.Find("Root_Anchor");
        player_body = Instantiate(player_prefab);
        player_body.transform.SetParent(gameObject.transform);
        player_body.transform.position = transform.position + transform.up;
    }

    void Update()
    {

        input_move_h = Input.GetAxisRaw("Horizontal");
        input_move_v = Input.GetAxisRaw("Vertical");

        input_cursor_h = Input.GetAxis("Mouse X");
        input_cursor_v = Input.GetAxis("Mouse Y");

        // 스페이스 바를 눌렀을 때 점프
        input_jump = Input.GetKeyDown(KeyCode.Space);

        if (Input.GetMouseButtonDown(0))
        {
            On_Click();
        }
    }

    private void FixedUpdate()
    {
        Player_Move(input_move_h, input_move_v, input_jump);
        Player_Rotate(input_cursor_h, input_cursor_v);
    }

    private void Player_Move(float move_h, float move_v, bool is_jump)
    {
        Vector3 direction = (transform.right * move_h + transform.forward * move_v).normalized;
        velocity_h = direction * move_speed;
        if (is_jump)
        {
            rb.AddForce(transform.up * jump_speed, ForceMode.VelocityChange);
        }
        rb.velocity = new Vector3(velocity_h.x, rb.velocity.y, velocity_h.z);
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
            if (ray_hits[i].collider.CompareTag("Morphable"))
            {
                GameManager.instance.Morph(gameObject, ray_hits[i].collider.gameObject.GetComponent<Morphable_Object>().prefab_num);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(anchor_transform.position, anchor_transform.forward * 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        for(int i = 0; i < collision.contacts.Length; i++)
        {
            Debug.Log(collision.contacts[i].point);
        }
    }
}
