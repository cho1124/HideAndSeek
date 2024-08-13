using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_Physic_Material : MonoBehaviour
{
    [SerializeField] private PhysicMaterial physic_material;
    private void Start()
    {
        //������ ���ӵ��� ���� �����ؼ� �̵��� �����ϱ� ������ �������� �ʿ����
        //��� �������� 0�� PhysicMaterial�� �ʿ� �����ϴ� ��� �ݶ��̴��� ����
        GameObject map = GameObject.Find("Map_v1");
        Collider[] colliders = map.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].material = physic_material;
        }
    }
}
