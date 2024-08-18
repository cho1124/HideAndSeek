using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lovely_Hammer : MonoBehaviour 
    {
    [SerializeField] private Collider hammer_collider = null; // �ڽ� ������Ʈ�� �ݶ��̴��� ���⼭ �������� �Ҵ�
    private Dictionary<GameObject, bool> victim_dictionary = new Dictionary<GameObject, bool>(); //�̹� ������ ���� ������Ʈ�� �����Ͽ� ���� ������Ʈ�� �ߺ��ؼ� ���ظ� ���� �ʵ��� ���� 
    [SerializeField] private GameObject self; //�÷��̾ �����Ͽ� ���� ���� �÷��̾ �������� Ȯ��

    private void Start()
    {
        if (hammer_collider == null) {
            // �ڽ� �� Ư�� �̸��� �ݶ��̴��� �������� �ݶ��̴� �ޱ��� �ʵ���
            hammer_collider = transform.Find("collider").GetComponent<Collider>();
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        GetComponentInParent<Player_Control>().hand = gameObject;
    }

    public void Collider_On() //�ִϸ��̼� �̺�Ʈ�� ���� ȣ��Ǹ� �ݶ��̴� Ȱ��ȭ 
    {
        if (hammer_collider != null) {
            hammer_collider.enabled = true;
        }
    }

    public void Collider_Off() //�ݶ��̴���Ȱ��ȭ�ϰ� ��ųʸ� �ʱ�ȭ�Ͽ� ���� ���ݿ� ��� 
    {
        if (hammer_collider != null) {
            hammer_collider.enabled = false;
            victim_dictionary.Clear();
        }
    }

    private void OnTriggerStay(Collider victim) //Player_Hide ���� ���� ������Ʈ��, ������ �ƴϸ�, ������ ���ݹ��� �� ���ٸ� �ش� ������Ʈ�� ���� ����ó��
    {
        if (victim.CompareTag("Player_Hide")) {
            if (victim.gameObject != self && (!victim_dictionary.ContainsKey(victim.gameObject) || !victim_dictionary[victim.gameObject])) {
                victim_dictionary[victim.gameObject] = true;
                //�θ����� ������ �ƴϸ� ���⼭ �ǰ� ó������
                Debug.Log("�����Ҵ�");

              //  �÷��̾� ��� ó��
            }
        }
    }
}

