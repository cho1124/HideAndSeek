using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Control : MonoBehaviour
{
    [SerializeField] Transform anchor_transform;

    private void Update()
    {
        transform.position = anchor_transform.position +  anchor_transform.forward * -7f;
        transform.LookAt(anchor_transform.position);
    }
}
