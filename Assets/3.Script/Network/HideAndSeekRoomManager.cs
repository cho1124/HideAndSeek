using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class HideAndSeekRoomManager : NetworkRoomManager
{
    public List<GameObject> hiders = new List<GameObject>();
    public List<GameObject> seekers = new List<GameObject>();

    [Header("��������Ʈ")]
    [SerializeField] private Transform seekerSpawnpoint;
    [SerializeField] private Transform hiderSpawnpoint;

    [Header("�� �÷��̾� ������")]
    [SerializeField] private GameObject seeker_obj;
    [SerializeField] private List<GameObject> hider_obj;

    public override void OnRoomServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnRoomServerAddPlayer(conn);

        GameObject roomPlayer = conn.identity.gameObject;

        if (Utils.IsSceneActive(RoomScene))
        {
            // �� ������ ��� ���� ��
            return;
        }

        AssignPlayerToTeam(roomPlayer);
    }

    private void AssignPlayerToTeam(GameObject player)
    {
        if (!NetworkServer.active)
        {
            Debug.LogWarning("AssignPlayerToTeam�� ���������� ȣ��Ǿ�� �մϴ�.");
            return;
        }

        var playerScript = player.GetComponent<GamePlayer>();
        int teamId = hiders.Count <= seekers.Count ? 1 : 2;

        if (teamId == 1)
        {
            playerScript.CmdAssignTeam(1);  // �� 1�� ����
            playerScript.CmdInitPlayer(hider_obj[UnityEngine.Random.Range(0, hider_obj.Count)], hiderSpawnpoint);
            hiders.Add(player);
        }
        else
        {
            playerScript.CmdAssignTeam(2);  // �� 2�� ����
            playerScript.CmdInitPlayer(seeker_obj, seekerSpawnpoint);
            seekers.Add(player);
        }
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        GameObject player = conn.identity.gameObject;
        GamePlayer playerScript = player.GetComponent<GamePlayer>();

        if (playerScript.teamId == 1)
        {
            hiders.Remove(player);
        }
        else if (playerScript.teamId == 2)
        {
            seekers.Remove(player);
        }

        base.OnRoomServerDisconnect(conn);
    }
}

