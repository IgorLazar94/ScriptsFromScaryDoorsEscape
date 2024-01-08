using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDrop : DropItem
{
    private int keyDropLevel = 1; // ���� �������� ��� ������!
    private float defaultKeyDropDoors;
    private float maxKeyDropDoors;
    private float keyDropCoefficient;
    private float keyDropElevatorTime = 5f;

    protected void Awake()
    {
        SetGameSettings();
        keyDropCoefficient = (defaultKeyDropDoors / maxKeyDropDoors) / 7f; // ���-�� ������
    }

    private void SetGameSettings()
    {
        defaultKeyDropDoors = GameSettings.Instance.GetDefaultKeyDoors();
        maxKeyDropDoors = GameSettings.Instance.GetMaxKeyDoors();
    }

    private void OnMouseDown()
    {
        ActivateSkill();
        Destroy(gameObject);
    }

    protected void ActivateSkill()
    {
        playerCamera.SetKeyTime(keyDropElevatorTime, keyDropCoefficient * keyDropLevel + defaultKeyDropDoors);
        playerCamera.ActivateKeyDropEffect(false);
    }
}
