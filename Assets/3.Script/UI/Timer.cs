using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    /*
     검정색이다가 30초가 남으면 빨강색+애니메이션2배로 빨라짐
     */
    public TextMeshProUGUI timerText; // 타이머 텍스트 참조
    public float maxTime = 180f;       // 최대 시간 (초)
    private float timeLeft;           // 남은 시간
    public Animator animator;         // Animator 참조 변수

    void Start() {
        timeLeft = maxTime;
        timerText.color = Color.black; // 초기 색상: 검정색
        animator.speed = 1f;           // 애니메이션 초기 속도 설정
    }

    void Update() {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            UpdateTimerDisplay();

            // 30초 남았을 때 색상을 빨간색으로 변경
            if (timeLeft <= 30f) {
                timerText.color = Color.red;
                animator.speed = 2f; // 애니메이션 속도 2배
            }
            // 10초 남았을 때 애니메이션 더 빠르게
            if (timeLeft <= 10f) {
                animator.speed = 3f; // 애니메이션 속도 3배
            }
        }
    }

    void UpdateTimerDisplay() {
        // 남은 시간을 초 단위로 표시
        int secondsLeft = Mathf.CeilToInt(timeLeft);
        timerText.text = secondsLeft.ToString();
    }
}
