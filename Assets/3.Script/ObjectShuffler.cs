using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShuffler : MonoBehaviour
{
    /*
    ���� �پ缺? Ȥ�� �÷��̾�� ȥ���� �ֱ� ����...
    �ʿ� ���� ������Ʈ��(�����ϸ��Ѱ͵�, Ŀ�ٶ� �� �̷��� �ƴ�...)��
    ��ġ�� �迭�� �ְ� �׵��� ������ ���۵ɶ����� �ڸ��� �ٲ� �� �ֵ��� �ϴ� ��ũ��Ʈ

    -��ġ�� ������Ʈ 1���� ��ġ
    -Ground �±׸� ���� �ݶ��̴� �̿� �ٸ� �ݶ��̴��� ���� �ʰ� ��ġ
    */

    public GameObject[] objects; // �ν����Ϳ��� ������Ʈ�� ���� �� �ֵ��� �迭�� ����

    void Start()
    {
        ShufflePositions();
    }

    void ShufflePositions()
    {
        // �迭�� ������Ʈ ����ŭ ��ġ�� ������ �迭�� ����
        Vector3[] positions = new Vector3[objects.Length];

        // �� ������Ʈ�� ���� ��ġ�� ����
        for (int i = 0; i < objects.Length; i++)
        {
            positions[i] = objects[i].transform.position;
        }

        // ����� ��ġ �迭�� �����ϰ� ����
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 temp = positions[i];
            int randomIndex = Random.Range(0, positions.Length);
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }

        // ���� ��ġ�� �� ������Ʈ�� ����
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].transform.position = positions[i];
        }
    }
}
