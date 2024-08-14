using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player_Control : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform anchor_transform;
    [SerializeField] private GameObject player_prefab;

    public GameObject player_body;
    private GameObject camera;
    
    [SerializeField] float move_speed = 5f;
    [SerializeField] float jump_speed = 5f; // ������ ����� ��
    float input_cursor_h, input_cursor_v, input_move_h, input_move_v;
    bool input_jump = false;
    bool is_jumped = false;

    Vector3 velocity_h = Vector3.zero;
    Vector3 velocity_v = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anchor_transform = transform.Find("Root_Anchor");
        
        camera = new GameObject();
        camera.AddComponent<NetworkIdentity>();
        camera.AddComponent<Camera>();


        //�� ó���� ������ �÷��̾� �𵨸����� �����ϰ� Ʈ������ �ʱ�ȭ����
        player_body = Instantiate(player_prefab);
        player_body.transform.SetParent(gameObject.transform);
        player_body.transform.position = transform.position + transform.up;
    }

    void Update()
    {
        if (!isLocalPlayer) return; //You shall not pass!!!

        //Ű���� �� ���콺 �Է��� Update����, ó���� FixedUpdate����.
        input_move_h = Input.GetAxisRaw("Horizontal");
        input_move_v = Input.GetAxisRaw("Vertical");
        input_cursor_h = Input.GetAxis("Mouse X");
        input_cursor_v = Input.GetAxis("Mouse Y");
        input_jump = Input.GetKeyDown(KeyCode.Space);

        //Ŭ������ �� �����ϴ� �� ����
        if (Input.GetMouseButtonDown(0))
        {
            On_Click();
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return; //You shall not pass!!!

        Player_Move(input_move_h, input_move_v, input_jump);
        Player_Rotate(input_cursor_h, input_cursor_v);
    }

    private void Player_Move(float move_h, float move_v, bool input_jump)
    {
        //�̵�Ű �Է°��� ���� ���� ���͸� ���� ���� ���ӵ��� ����
        Vector3 direction = (transform.right * move_h + transform.forward * move_v).normalized;
        velocity_h = direction * move_speed;

        //����Ű �Է� �� ���� ���ӵ��� ���� �����ϰ� �����ߴ��� üũ
        if (input_jump && is_ground)
        {
            velocity_v = transform.up * jump_speed;
            is_ground = false;
            StartCoroutine(Jumping_Co());
        }
        else if (is_ground)
        {
            velocity_v = Vector3.zero;
        }
        //�� �ܿ� ��� ��Ȳ������ �߷� ���ӵ� ������ ����
        else velocity_v = Vector3.Lerp(velocity_v, Physics.gravity, Time.deltaTime);


        if(velocity_v.y == 0f) Debug.Log($"���� ���ӵ� : {velocity_v.y}");
        Debug.Log($"is_ground : {is_ground}");
        //������ ����� ���ӵ��� ���Ͽ� ����
        rb.velocity = velocity_h + velocity_v;
    }

    private void Player_Rotate(float cursor_h, float cursor_v)
    {
        //��Ŀ�� ���� ȸ��
        anchor_transform.Rotate(new Vector3(cursor_v * 1.5f, -cursor_h * 1.5f, 0f));
        //��Ŀ �����̼� �� ����
        Quaternion anchor_rotation = anchor_transform.rotation;

        //�÷��̾� ȸ��
        transform.rotation = Quaternion.Euler(0f, anchor_transform.rotation.eulerAngles.y, 0f);
        //���� ȸ���ع��� ��Ŀ�� ����ȭ
        anchor_transform.rotation = anchor_rotation;

        Camera_Rotate();
    }

    private void Camera_Rotate()
    {
        camera.transform.position = anchor_transform.position + anchor_transform.forward * -7f + anchor_transform.up * 2f;
        camera.transform.LookAt(anchor_transform.position);
    }

    private void On_Click()
    { 
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(new Ray(anchor_transform.position, anchor_transform.forward), out hit, 10f);
        if (hit.collider.CompareTag("Morphable"))
        {
            //GameManager.instance.Morph(gameObject, hit.collider.gameObject.GetComponent<Morphable_Object>().prefab_num);
        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawRay(anchor_transform.position, anchor_transform.forward * 10f);
    }

    private void OnCollisionStay(Collision collision)
    {
        for(int i = 0; i < collision.contacts.Length; i++)
        {
            //�ݶ��̴� �浹 �� �������� ������ ���ϰ�, ���� ������ �Ʒ� ����, �� ���� ��� �ٽ� ���� �����ϵ��� ����
            if((collision.contacts[i].point - transform.position).y < 0.1f && !is_jumping)
            {
                last_contact = collision.contacts[i].point;
                is_ground = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if((last_contact - transform.position).y < 0.1f)
        {
            is_ground = false;
        }
    }

    Vector3 last_contact = new Vector3();

    bool is_ground = false;
    bool is_jumping = false;

    private IEnumerator Jumping_Co()
    {
        is_jumping = true;
        yield return new WaitForSeconds(0.2f);
        is_jumping = false;
    }
}
