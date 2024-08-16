using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lovely_Hammer : MonoBehaviour
{
    [SerializeField] private Collider hammer_collider = null;
    private Dictionary<GameObject, bool> victim_dictionary = new Dictionary<GameObject, bool>();
    [SerializeField] private GameObject self;
    [SerializeField] private Animation_Control animation_control;

    public void Collider_On()
    {
        animation_control = GetComponentInParent<Animation_Control>();
        hammer_collider.enabled = true;
    }
    public void Collider_Off()
    {
        hammer_collider.enabled = false;
        victim_dictionary.Clear();
    }
    private void OnTriggerStay(Collider victim)
    {
        if (victim.CompareTag("Player_Hide"))
        {
            if (victim.gameObject != self && (!victim_dictionary.ContainsKey(victim.gameObject) || !victim_dictionary[victim.gameObject]))
            {
                victim_dictionary[victim.gameObject] = true;
                //부모한테 보낼지 아니면 여기서 피격 처리할지
            }
        }
    }
}
