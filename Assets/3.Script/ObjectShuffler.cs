using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectShuffler : NetworkBehaviour {
    /*
    ���� �پ缺? Ȥ�� �÷��̾�� ȥ���� �ֱ� ����...
    �ʿ� ���� ������Ʈ��(�����ϸ��Ѱ͵�, Ŀ�ٶ� �� �̷��� �ƴ�...)��
    ��ġ�� �迭�� �ְ� �׵��� ������ ���۵ɶ����� �ڸ��� �ٲ� �� �ֵ��� �ϴ� ��ũ��Ʈ

    Fisher-Yates ���� �˰����� ����Ͽ� �������� �����߽��ϴ�.
    objects �迭�� null�̰ų� ��� ���� ���� ó���� �߰��Ͽ����ϴ�.

    ������ ��ġ�� ������ �� Ŭ���̾�Ʈ�鿡�� �ش� ��ġ�� �����Ͽ� ������ ��ġ�� ����ǵ��� �ϴ����Դϴ�...
    */

    public GameObject[] objects; // �ν����Ϳ��� ������Ʈ�� ���� �� �ֵ��� �迭�� ����

    public override void OnStartServer() {
        Debug.Log("OnStartServer called");

        if (objects == null || objects.Length == 0) {
            Debug.LogWarning("Objects array is null or empty. Shuffling aborted.");
            return;
        }
        if (isServer) {
            Debug.Log("������");
            // ���������� ������ ����
            ShufflePositions();
            // Ŭ���̾�Ʈ�鿡�� ���õ� ��ġ�� ����ȭ
            RpcUpdatePositions(GetPositions());
        }
    }

    public override void OnStartClient() //Ŭ���̾�Ʈ���� ���� ��ġ ��û ���� �������� �ٷ� ����ȭ�� ��ġ�� ���� �� �ֵ��� ���� 
{
        base.OnStartClient();
    }
  
  //      if (!isServer) {
  //          CmdRequestPositions();
  //      }
  //  }

  //  [Command] //Ŭ���̾�Ʈ���� ������ ����� ����, ���� ������Ʈ ��ġ ��û
  //  void CmdRequestPositions() {
  //      RpcUpdatePositions(GetPositions());
  //  }

    void ShufflePositions() {
        // �迭�� ������Ʈ ����ŭ ��ġ�� ������ �迭�� ����
        Vector3[] positions = new Vector3[objects.Length];

        // �� ������Ʈ�� ���� ��ġ�� ����
        for (int i = 0; i < objects.Length; i++) {
            positions[i] = objects[i].transform.position;
        }


        // Fisher-Yates �˰����� ����Ͽ� ��ġ �迭�� �����ϰ� ����
        for (int i = positions.Length - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        //  // ���� ��ġ�� �� ������Ʈ�� ����
        //  for (int i = 0; i < objects.Length; i++) {
        //      objects[i].transform.position = positions[i];
        //  }

        // ���� ��ġ�� �� ������Ʈ�� ����
        SetPositions(positions);

    }

    Vector3[] GetPositions() //objects�迭�� �ִ� ��� ���� ������Ʈ�� ��ġ�� �����ͼ� Vector3 �迭�� ��ȯ
        {
        Vector3[] positions = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++) {
            positions[i] = objects[i].transform.position;
        }
        return positions; //�� ������Ʈ�� ��ġ�� ���� positions �迭�� ��ȯ�մϴ�.
    }

    void SetPositions(Vector3[] positions) //���޹��� positions �迭�� ����Ͽ� objects �迭�� �ִ� �� ���� ������Ʈ�� ��ġ�� �����մϴ�.
{
        for (int i = 0; i < objects.Length; i++) {
            // `NetworkTransform` ��Ȱ��ȭ ��� �ڵ����� ��ġ�� ����ȭ�ϵ��� �Ӵϴ�.
            objects[i].transform.position = positions[i]; // ��ġ ����
        }
    }

    [ClientRpc] //������ Ŭ���̾�Ʈ�鿡�� �����͸� ����
    void RpcUpdatePositions(Vector3[] positions) {
        // Ŭ���̾�Ʈ�� �������� ���� ��ġ ������ ����
        SetPositions(positions);
    }

}

