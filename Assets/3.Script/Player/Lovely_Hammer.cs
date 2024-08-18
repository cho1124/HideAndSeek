using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lovely_Hammer : MonoBehaviour 
    {
    [SerializeField] private Collider hammer_collider = null; // 자식 오브젝트의 콜라이더를 여기서 수동으로 할당
    private Dictionary<GameObject, bool> victim_dictionary = new Dictionary<GameObject, bool>(); //이미 공격을 받은 오브젝트를 저장하여 같은 오브젝트가 중복해서 피해를 입지 않도록 관리 
    [SerializeField] private GameObject self; //플레이어를 참조하여 공격 대상과 플레이어가 동일한지 확인

    private void Start()
    {
        if (hammer_collider == null) {
            // 자식 중 특정 이름의 콜라이더를 가져오기 콜라이더 햇깔리지 않도록
            hammer_collider = transform.Find("collider").GetComponent<Collider>();
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        GetComponentInParent<Player_Control>().hand = gameObject;
    }

    public void Collider_On() //애니메이션 이벤트에 의해 호출되며 콜라이더 활성화 
    {
        if (hammer_collider != null) {
            hammer_collider.enabled = true;
        }
    }

    public void Collider_Off() //콜라이더비활성화하고 딕셔너리 초기화하여 다음 공격에 대비 
    {
        if (hammer_collider != null) {
            hammer_collider.enabled = false;
            victim_dictionary.Clear();
        }
    }

    private void OnTriggerStay(Collider victim) //Player_Hide 가진 게임 오브젝트고, 본인이 아니며, 이전에 공격받은 적 없다면 해당 오브젝트에 대한 피해처리
    {
        if (victim.CompareTag("Player_Hide")) {
            if (victim.gameObject != self && (!victim_dictionary.ContainsKey(victim.gameObject) || !victim_dictionary[victim.gameObject])) {
                victim_dictionary[victim.gameObject] = true;
                //부모한테 보낼지 아니면 여기서 피격 처리할지
                Debug.Log("요놈잡았다");

              //  플레이어 사망 처리
            }
        }
    }
}

