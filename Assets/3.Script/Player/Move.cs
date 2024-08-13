using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float Speed = 5f;
    public Camera mainCamera; // ���� ī�޶� �����ϱ� ���� ����
    public float jumpForce = 3f; // ������ ����� ��
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody ������Ʈ�� ��������
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float moveX = Input.GetAxis("Horizontal") * Speed*Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * Speed*Time.deltaTime;

        // �̵� ����
        transform.Translate(moveX, 0, moveZ);

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            // ���콺�� ����Ű�� ��ġ�� XZ ��� ��ǥ�� ���
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Y�� ����

            // �÷��̾ ���콺�� �ٶ󺸵��� ȸ�� (XZ ��鸸)
            Vector3 direction = (targetPosition - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*1/2);
        }

        // �����̽� �ٸ� ������ �� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
