using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [Header("흐름처리")]
    [SyncVar(hook = nameof(ChangeHookTimer))]
    public float timer = 180f;
    private bool isGameOver = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI Timer_UI;
    [SerializeField] private GameObject timerUI;

    
    private void Start()
    {
        timerUI = GameObject.Find("Timer UI");
        Timer_UI = timerUI.GetComponent<TextMeshProUGUI>();
        InitializeGame();
    }

    [Server]
    private void InitializeGame()
    {
        // 초기화 로직 (예: 게임 시작 시 플레이어 초기화, 타이머 설정 등)
        Time.timeScale = 1;
        timer = 180f;
        isGameOver = false;
    }

    private void Update()
    {
        CheckTimer();
    }

    [Server]
    private void CheckTimer()
    {
        if (timer <= 0 && !isGameOver)
        {
            isGameOver = true;
            RpcGameOver("Hider Win!"); // 클라이언트들에게 게임 종료 알림
        }
        else
        {
            timer -= Time.deltaTime;
            Debug.Log($"Server: Timer updated to {timer}"); // 서버 로그 추가
        }
    }

    private void ChangeHookTimer(float oldvalue, float newvalue)
    {
        Debug.Log("timer : " + newvalue);
        UpdateTimerUI(newvalue); // 타이머 값이 변경될 때 UI 업데이트
    }

    private void UpdateTimerUI(float time)
    {
        Timer_UI.text = ((int)time).ToString(); // 타이머를 정수로 변환하여 텍스트 업데이트
        Debug.Log("sync!");
    }

    private void RpcGameOver(string result)
    {
        Debug.Log(result);
        Time.timeScale = 0;
        // 추가적인 게임 종료 처리 (예: UI 업데이트)
    }
}
