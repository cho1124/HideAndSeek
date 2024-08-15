using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GamePlayer : NetworkBehaviour
{ 

    public static string nickName;
    public static string ip;
    public static string connectToIp;
    public static bool isHost;

    

    private void Start()
    {
        //gameObject.transform.SetParent(GameObject.Find("PlayerList").transform);
        //GameManager.instance.test_player.Add(gameObject);
        //
        //Debug.Log(nickName);
    }

}
