using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDrop : DropItem
{
    private int potionDropLevel = 1; // —юда прогресс апа зель€!
    private float defaultPotionTime;
    private float maxPotionTime;
    private float potionCoefficient;

    protected void Awake()
    {
        SetGameSettings();
        potionCoefficient = (defaultPotionTime / maxPotionTime) / 7f; // кол-во стадий
    }

    private void SetGameSettings()
    {
        defaultPotionTime = GameSettings.Instance.GetDefaultPotionTime();
        maxPotionTime = GameSettings.Instance.GetMaxPotionTime();
    }

    private void OnMouseDown()
    {
        ActivateSkill();
        Destroy(gameObject);
    }

    protected void ActivateSkill()
    {
        playerCamera.ActivateSlowMotionFromPotion(potionCoefficient * potionDropLevel + defaultPotionTime);
        dropGenerator.ActivateDropViewPanel(potionCoefficient * potionDropLevel + defaultPotionTime, SpriteCollection.Instance.PotionDrop);
    }
}
