using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{

    public static GameManager instance = null;

    [Header("�帧ó��")]
    [SyncVar(hook = nameof(ChangeHookTimer))]
    public float timer = 180f;
    private bool isGameOver = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI Timer_UI;
    [SerializeField] private GameObject timerObj;
    [SerializeField] private TextMeshProUGUI HP_UI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        timerObj = GameObject.Find("Timer UI");
        if (timerObj != null)
        {
            Timer_UI = timerObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Timer UI object not found!");
        }

        if (Timer_UI == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on Timer UI object!");
        }
    }



    private void Start()
    {
        
        InitializeGame();
    }

    [Server]
    private void InitializeGame()
    {
        // �ʱ�ȭ ���� (��: ���� ���� �� �÷��̾� �ʱ�ȭ, Ÿ�̸� ���� ��)
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
            RpcGameOver("Hider Win!"); // Ŭ���̾�Ʈ�鿡�� ���� ���� �˸�
        }
        else
        {
            timer -= Time.deltaTime;
            //Debug.Log($"Server: Timer updated to {timer}"); // ���� �α� �߰�
        }
    }

    private void ChangeHookTimer(float oldvalue, float newvalue)
    {
        //Debug.Log("timer : " + newvalue);
        UpdateTimerUI(newvalue); // Ÿ�̸� ���� ����� �� UI ������Ʈ
    }

    private void UpdateTimerUI(float time)
    {
        Timer_UI.text = ((int)time).ToString(); // Ÿ�̸Ӹ� ������ ��ȯ�Ͽ� �ؽ�Ʈ ������Ʈ
        //Debug.Log("sync!");
    }

    private void RpcGameOver(string result)
    {
        Debug.Log(result);
        Time.timeScale = 0;
        // �߰����� ���� ���� ó��
    }
}
