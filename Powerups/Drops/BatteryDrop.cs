using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryDrop : DropItem
{
    private int batteryDropLevel = 1; // ���� �������� ��� ���������!

    private void OnMouseDown()
    {
        ActivateSkill();
        Destroy(gameObject);
    }

    protected void ActivateSkill()
    {
        playerCamera.AddChargeFlashlightFromBatteryDrop(batteryDropLevel);
    }
}
