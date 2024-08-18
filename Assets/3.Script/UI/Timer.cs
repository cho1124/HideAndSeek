using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    /*
     �������̴ٰ� 30�ʰ� ������ ������+�ִϸ��̼�2��� ������
     */
    public TextMeshProUGUI timerText; // Ÿ�̸� �ؽ�Ʈ ����
    public float maxTime = 180f;       // �ִ� �ð� (��)
    private float timeLeft;           // ���� �ð�
    public Animator animator;         // Animator ���� ����

    void Start() {
        timeLeft = maxTime;
        timerText.color = Color.black; // �ʱ� ����: ������
        animator.speed = 1f;           // �ִϸ��̼� �ʱ� �ӵ� ����
    }

    void Update() {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            UpdateTimerDisplay();

            // 30�� ������ �� ������ ���������� ����
            if (timeLeft <= 30f) {
                timerText.color = Color.red;
                animator.speed = 2f; // �ִϸ��̼� �ӵ� 2��
            }
            // 10�� ������ �� �ִϸ��̼� �� ������
            if (timeLeft <= 10f) {
                animator.speed = 3f; // �ִϸ��̼� �ӵ� 3��
            }
        }
    }

    void UpdateTimerDisplay() {
        // ���� �ð��� �� ������ ǥ��
        int secondsLeft = Mathf.CeilToInt(timeLeft);
        timerText.text = secondsLeft.ToString();
    }
}
