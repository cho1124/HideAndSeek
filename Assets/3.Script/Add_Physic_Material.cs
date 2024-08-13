using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_Physic_Material : MonoBehaviour
{
    [SerializeField] private PhysicMaterial physic_material;
    private void Start()
    {
        //어차피 가속도를 직접 설정해서 이동을 구현하기 때문에 마찰력은 필요없음
        //모든 마찰력이 0인 PhysicMaterial을 맵에 존재하는 모든 콜라이더에 적용
        GameObject map = GameObject.Find("Map_v1");
        Collider[] colliders = map.GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].material = physic_material;
        }
    }
}
