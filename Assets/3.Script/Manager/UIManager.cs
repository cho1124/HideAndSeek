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

    public HideAndSeekRoomManager roomManager;

    [SerializeField] private TextMeshProUGUI hider_text;
    [SerializeField] private TextMeshProUGUI seeker_text;


    private void Start()
    {
        roomManager = FindAnyObjectByType<HideAndSeekRoomManager>();


        if (roomManager != null)
        {
            // 플레이어 수 변경 이벤트에 UI 업데이트 메서드 연결
            roomManager.OnPlayerCountChanged.AddListener(UpdatePlayerCounts);
        }
        
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

    private void UpdatePlayerCounts()
    {

        hider_text.text = $"Hiders : {roomManager.GetTeamCount(1)} ";
        seeker_text.text = $"Seekers : {roomManager.GetTeamCount(2)}";
    }


}
