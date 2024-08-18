using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBlink : MonoBehaviour 
    {
    /*
    ��� �̹��� ��¦�̰� �ϰ�;...
    */
    public Image targetImage;       // ��¦�̰� �� �̹���
    public float blinkSpeed = 1f;   // ��¦�̴� �ӵ�
    public float minAlpha = 0.5f;   // �ּ� ���� ��
    public float maxAlpha = 1f;     // �ִ� ���� ��

    private Color originalColor;

    void Start() {
        // �̹����� ���� ������ ����
        originalColor = targetImage.color;
    }

    void Update() {
        // �ð��� ���� ���� �� ��� (Sin �Լ��� ���)
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * blinkSpeed, 1));
        targetImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }
}
