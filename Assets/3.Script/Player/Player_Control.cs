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
    [SerializeField] float jump_speed = 5f; // 점프에 사용할 힘
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
        ////맨 처음에 지정된 플레이어 모델링으로 시작하고 트랜스폼 초기화해줌
        //player_body = Instantiate(player_prefab);
        //player_body.transform.SetParent(gameObject.transform);
        //player_body.transform.position = transform.position + transform.up;
    }

    void Update()
    {
        //키보드 및 마우스 입력은 Update에서, 처리는 FixedUpdate에서.
        input_move_h = Input.GetAxisRaw("Horizontal");
        input_move_v = Input.GetAxisRaw("Vertical");
        input_cursor_h = Input.GetAxis("Mouse X");
        input_cursor_v = Input.GetAxis("Mouse Y");
        input_jump = Input.GetKeyDown(KeyCode.Space);

        //클릭했을 때 변신하는 거 예시
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
        //이동키 입력값에 대한 방향 벡터를 통해 수평 가속도를 구함
        Vector3 direction = (transform.right * move_h + transform.forward * move_v).normalized;
        velocity_h = direction * move_speed;
        
        //점프키 입력 시 수직 가속도를 직접 설정하고 점프했는지 체크
        if (input_jump && !is_jumped)
        {
            velocity_v = transform.up * jump_speed;
            is_jumped = true;
        }
        //그 외에 모든 상황에서는 중력 가속도 적용을 받음
        else velocity_v = Vector3.Lerp(velocity_v, Physics.gravity, Time.deltaTime);

        //위에서 계산한 가속도를 합하여 적용
        rb.velocity = velocity_h + velocity_v;
    }

    private void Player_Rotate(float cursor_h, float cursor_v)
    {
        //앵커가 먼저 회전
        anchor_transform.Rotate(new Vector3(cursor_v * 1.5f, -cursor_h * 1.5f, 0f));
        //앵커 로테이션 값 저장
        Quaternion anchor_rotation = anchor_transform.rotation;

        //플레이어 회전
        transform.rotation = Quaternion.Euler(0f, anchor_transform.rotation.eulerAngles.y, 0f);
        //같이 회전해버린 앵커를 정상화
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
            //콜라이더 충돌 시 접촉점의 방향을 구하고, 만약 방향이 아래 방향, 즉 땅일 경우 다시 점프 가능하도록 설정
            if((collision.contacts[i].point - transform.position).y < 0.1f)
            {
                is_jumped = false;
            }
        }
    }
}
