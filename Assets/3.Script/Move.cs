using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float Speed = 5f;
    public Camera mainCamera; // 메인 카메라를 참조하기 위한 변수
    public float jumpForce = 3f; // 점프에 사용할 힘
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody 컴포넌트를 가져오기
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float moveX = Input.GetAxis("Horizontal") * Speed*Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * Speed*Time.deltaTime;

        // 이동 적용
        transform.Translate(moveX, 0, moveZ);

        // 마우스 위치를 월드 좌표로 변환
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            // 마우스가 가리키는 위치의 XZ 평면 좌표만 사용
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Y축 고정

            // 플레이어가 마우스를 바라보도록 회전 (XZ 평면만)
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*1/2);
        }

        // 스페이스 바를 눌렀을 때 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
