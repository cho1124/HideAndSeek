/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectShuffler : NetworkBehaviour {
    
    ///���� �پ缺? Ȥ�� �÷��̾�� ȥ���� �ֱ� ����...
    ///�ʿ� ���� ������Ʈ��(�����ϸ��Ѱ͵�, Ŀ�ٶ� �� �̷��� �ƴ�...)��
    ///��ġ�� �迭�� �ְ� �׵��� ������ ���۵ɶ����� �ڸ��� �ٲ� �� �ֵ��� �ϴ� ��ũ��Ʈ
    ///
    ///Fisher-Yates ���� �˰����� ����Ͽ� �������� �����߽��ϴ�.
    ///objects �迭�� null�̰ų� ��� ���� ���� ó���� �߰��Ͽ����ϴ�.

    

    public GameObject[] objects; // �ν����Ϳ��� ������Ʈ�� ���� �� �ֵ��� �迭�� ����

    void Start() {
        if (objects == null || objects.Length == 0) {
            Debug.LogWarning("Objects array is null or empty. Shuffling aborted.");
            return;
        }
        RpcShufflePositions();
                   
    }





    [Server]
    public void ServerShufflePositions() {
        // �迭�� ������Ʈ ����ŭ ��ġ�� ������ �迭�� ����
        Vector3[] positions = new Vector3[objects.Length];

        // �� ������Ʈ�� ���� ��ġ�� ����
        for (int i = 0; i < objects.Length; i++)
        {
            positions[i] = objects[i].transform.position;
        }

        // Fisher-Yates �˰����� ����Ͽ� ��ġ �迭�� �����ϰ� ����
        for (int i = positions.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // ���� ��ġ�� �� ������Ʈ�� ����
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = positions[i];
            Debug.Log(objects[i].transform.position);
        }
    }

    [ClientRpc]
    public void RpcShufflePositions()
    {
        ServerShufflePositions();

    }
    
}

*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectShuffler : NetworkBehaviour
{
    public GameObject[] objects; // �ν����Ϳ��� ������Ʈ�� ���� �� �ֵ��� �迭�� ����

    void Start()
    {
        if (objects == null || objects.Length == 0)
        {
            Debug.LogWarning("Objects array is null or empty. Shuffling aborted.");
            return;
        }

        // ���������� ��ġ�� �����ϰ� Ŭ���̾�Ʈ�� ����
        if (isServer)
        {
            ServerShufflePositions();
        }
    }

    [Server]
    private void ServerShufflePositions()
    {
        // �迭�� ������Ʈ ����ŭ ��ġ�� ������ �迭�� ����
        Vector3[] positions = new Vector3[objects.Length];

        // �� ������Ʈ�� ���� ��ġ�� ����
        for (int i = 0; i < objects.Length; i++)
        {
            positions[i] = objects[i].transform.position;
        }

        // Fisher-Yates �˰����� ����Ͽ� ��ġ �迭�� �����ϰ� ����
        for (int i = positions.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // ���� ��ġ�� Ŭ���̾�Ʈ���� ����
        RpcShufflePositions(positions);
    }

    [ClientRpc]
    private void RpcShufflePositions(Vector3[] positions)
    {
        // ���� ��ġ�� �� ������Ʈ�� ���� (������ ��� Ŭ���̾�Ʈ���� �����)
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = positions[i];
            Debug.Log($"Object {objects[i].name} new position: {positions[i]}");
        }
    }
}
