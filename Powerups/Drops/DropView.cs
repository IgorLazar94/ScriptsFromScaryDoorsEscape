using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropView : MonoBehaviour
{
    public bool IsActive { get; private set; }
    [SerializeField] private Image dropIcon;
    [SerializeField] private Image dropProgressBar;

    private void Start()
    {
        IsActive = false;
    }

    public void ActivateDrop(float time)
    {
        //if (IsActive)
        //{
        //    return;
        //}
        dropProgressBar.fillAmount = 0f;
        IsActive = true;
        float targetFillAmount = 1f;
        dropProgressBar.DOFillAmount(targetFillAmount, time)
            .SetEase(Ease.Linear)
            .OnComplete(() => DeactivateDrop());
    }

    private void DeactivateDrop()
    {
        IsActive = false;
        gameObject.SetActive(false);
    }

    public void SetSprite(Sprite _dropIcon)
    {
        dropIcon.sprite = _dropIcon;
    }

    public Sprite GetSprite()
    {
        return dropIcon.sprite;
    }
}
