using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrop : DropItem
{
    private int cameraDropLevel = 1; // —юда прогресс апа камеры!
    private float defaultCameraTime;
    private float maxCameraTime;
    private float cameraCoefficient;

    protected void Awake()
    {
        SetGameSettings();
        cameraCoefficient = (defaultCameraTime / maxCameraTime) / 7f; // кол-во стадий
    }

    private void SetGameSettings()
    {
        defaultCameraTime = GameSettings.Instance.GetDefaultCameraTime();
        maxCameraTime = GameSettings.Instance.GetMaxCameraTime();
    }

    private void OnMouseDown()
    {
        ActivateSkill();
        Destroy(gameObject);
    }

    protected void ActivateSkill()
    {
        playerCamera.ActivateCameraDropEffect(cameraCoefficient * cameraDropLevel + defaultCameraTime);
        dropGenerator.ActivateDropViewPanel(cameraCoefficient * cameraDropLevel + defaultCameraTime, SpriteCollection.Instance.CameraDrop);
    }
}
