using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{

    public float Speed = 5f;

    // Update is called once per frame
    void Update()
    {

        float moveX = Input.GetAxis("Horizontal") * Speed*Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * Speed*Time.deltaTime;

        transform.Translate(moveX,0,moveZ);
        
    }
}
