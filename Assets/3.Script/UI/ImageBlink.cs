using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlink : MonoBehaviour 
    {
    /*
    배경 이미지 반짝이게 하고싶어서...
    */
    public Image targetImage;       // 반짝이게 할 이미지
    public float blinkSpeed = 1f;   // 반짝이는 속도
    public float minAlpha = 0.5f;   // 최소 알파 값
    public float maxAlpha = 1f;     // 최대 알파 값

    private Color originalColor;

    void Start() {
        // 이미지의 원래 색상을 저장
        originalColor = targetImage.color;
    }

    void Update() {
        // 시간에 따른 알파 값 계산 (Sin 함수를 사용)
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * blinkSpeed, 1));
        targetImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}
