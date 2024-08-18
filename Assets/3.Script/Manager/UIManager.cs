using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hp_text;

    private GamePlayer localPlayer;

    private void Start()
    {
        Debug.Log("Started!");
        FindLocalPlayer();
    }

    private void FindLocalPlayer()
    {
        // 현재 씬에 있는 모든 Player 객체를 검색
        GamePlayer[] players = FindObjectsOfType<GamePlayer>();

        if(players.Length == 0)
        {
            Debug.Log("player count is null!");
        }


        foreach (var player in players)
        {
            if (player.isLocalPlayer)
            {
                Debug.Log("Found Local Player!");
                localPlayer = player;
                break;
            }
        }
    }

}
