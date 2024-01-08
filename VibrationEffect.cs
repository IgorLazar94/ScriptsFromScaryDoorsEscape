using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationEffect : MonoBehaviour
{
    [SerializeField] private float vibrationAmount = 0.1f;
    [SerializeField] private float vibrationSpeed = 30f;
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        InvokeRepeating(nameof(ApplyVibration), 0f, 0.1f);
    }

    void OnDisable()
    {
        CancelInvoke(nameof(ApplyVibration));
        rectTransform.anchoredPosition = originalPosition;
    }

    private void ApplyVibration()
    {
        float offsetX = Random.Range(-vibrationAmount, vibrationAmount);
        float offsetY = Random.Range(-vibrationAmount, vibrationAmount);
        Vector3 newPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
        rectTransform.anchoredPosition = Vector3.Lerp(rectTransform.anchoredPosition, newPosition, Time.deltaTime * vibrationSpeed);
    }
}
