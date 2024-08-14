using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour
{


    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform anchor_transform;
    [SerializeField] private GameObject player_prefab;

    public GameObject player_body;

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
        //
        ////�� ó���� ������ �÷��̾� �𵨸����� �����ϰ� Ʈ������ �ʱ�ȭ����
        //player_body = Instantiate(player_prefab);
        //player_body.transform.SetParent(gameObject.transform);
        //player_body.transform.position = transform.position + transform.up;
    }

    void Update()
    {
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
        Player_Move(input_move_h, input_move_v, input_jump);
        Player_Rotate(input_cursor_h, input_cursor_v);
    }

    private void Player_Move(float move_h, float move_v, bool input_jump)
    {
        //�̵�Ű �Է°��� ���� ���� ���͸� ���� ���� ���ӵ��� ����
        Vector3 direction = (transform.right * move_h + transform.forward * move_v).normalized;
        velocity_h = direction * move_speed;
        
        //����Ű �Է� �� ���� ���ӵ��� ���� �����ϰ� �����ߴ��� üũ
        if (input_jump && !is_jumped)
        {
            velocity_v = transform.up * jump_speed;
            is_jumped = true;
        }
        //�� �ܿ� ��� ��Ȳ������ �߷� ���ӵ� ������ ����
        else velocity_v = Vector3.Lerp(velocity_v, Physics.gravity, Time.deltaTime);

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
    }

    private void On_Click()
    {
        //RaycastHit[] ray_hits = Physics.RaycastAll(anchor_transform.position, anchor_transform.forward, 10f);
        //for (int i = 0; i < ray_hits.Length; i++)
        //{
        //    Debug.Log($"{ray_hits[i].collider.gameObject.name}");
        //    if (ray_hits[i].collider.CompareTag("Morphable"))
        //    {
        //        GameManager.instance.Morph(gameObject, ray_hits[i].collider.gameObject.GetComponent<Morphable_Object>().prefab_num);
        //        break;
        //    }
        //}

        RaycastHit hit = new RaycastHit();
        Physics.Raycast(new Ray(anchor_transform.position, anchor_transform.forward), out hit, 10f);
        if (hit.collider.CompareTag("Morphable"))
        {
            //GameManager.instance.Morph(gameObject, hit.collider.gameObject.GetComponent<Morphable_Object>().prefab_num);

            //Destroy(player_body);
            //
            //player_body = new GameObject();
            //player_body.transform.SetParent(gameObject.transform);
            //player_body.transform.localPosition = transform.up;
            //player_body.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            //
            //GameObject morph_to = hit.collider.gameObject;
            //
            //MeshFilter mesh_filter_to = morph_to.GetComponent<MeshFilter>();
            //MeshFilter mesh_filter_from = player_body.AddComponent<MeshFilter>();
            //mesh_filter_from.sharedMesh = mesh_filter_to.sharedMesh;
            //
            //
            //MeshRenderer mesh_renderer_to = morph_to.GetComponent<MeshRenderer>();
            //if (mesh_renderer_to != null)
            //{
            //    MeshRenderer mesh_renderer_from = player_body.AddComponent<MeshRenderer>();
            //    mesh_renderer_from.sharedMaterials = mesh_renderer_to.sharedMaterials;
            //}
            //
            //Collider collider_to = morph_to.GetComponent<Collider>();
            //if (collider_to is MeshCollider)
            //{
            //    MeshCollider mesh_collider_to = morph_to.GetComponent<MeshCollider>();
            //    MeshCollider mesh_collider_from = player_body.AddComponent<MeshCollider>();
            //    mesh_collider_from.sharedMesh = mesh_collider_to.sharedMesh;
            //}
        }
    }

    private void OnDrawGizmos()
    {
       // Gizmos.DrawRay(anchor_transform.position, anchor_transform.forward * 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        for(int i = 0; i < collision.contacts.Length; i++)
        {
            //�ݶ��̴� �浹 �� �������� ������ ���ϰ�, ���� ������ �Ʒ� ����, �� ���� ��� �ٽ� ���� �����ϵ��� ����
            if((collision.contacts[i].point - transform.position).y < 0.1f)
            {
                is_jumped = false;
            }
        }
    }
}
