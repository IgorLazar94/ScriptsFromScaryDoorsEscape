using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(this.gameObject);
    }

    [Space]
    [Header("Player camera config")]
    [SerializeField] private float playerCameraSpeed = 30f;
    [SerializeField] private float playerCameraAccelerationSpeed = 1.8f;

    [Space]
    [Header("Flashlight battery config")]
    [Tooltip("Настройки для батарейки фонарика")]
    [SerializeField] private float batteryTotalTime = 50f;
    [SerializeField] private float flashlightMaxIntensity = 4f;
    [SerializeField] private float chargeTimeBonusForOneDoor = 2.5f;
    [SerializeField] private float chargeTimeBonusForTwentyDoor = 50f;
    [SerializeField] private float reduceChargeFromBadDoor = 2.5f;

    [Space]
    [Header("Booster Controller config")]
    [SerializeField] private float boosterFlashingButtonSpeed = 0.1f;
    [SerializeField] private float boosterFlashingButtonInterval = 0.25f;
    [SerializeField] private float timeToShowBoosters = 5f;

    [Space]
    [Header("Booster VIP key")]
    [SerializeField] private float elevatorVIPTime = 7f;
    [SerializeField] private float elevatorVIPDoorsCount = 40f;

    [Space]
    [Header("Drop generator config")]
    [SerializeField] private int spawnBatteryAfterDoorsCount = 10;
    [SerializeField] private int spawnCameraAfterDoorsCount = 30;
    [SerializeField] private int spawnPotionAfterDoorsCount = 20;
    [SerializeField] private int spawnKeyAfterDoorsCount = 40;

    [Space]
    [Tooltip("Шанс на выпадение дропа")]
    [SerializeField][Range(0f, 100f)] private float chanceOfGettingBattery = 6f;
    [SerializeField][Range(0f, 100f)] private float chanceOfGettingCamera = 2.5f;
    [SerializeField][Range(0f, 100f)] private float chanceOfGettingPotion = 4f;
    [SerializeField][Range(0f, 100f)] private float chanceOfGettingKey = 2.5f;

    [Space]
    [Tooltip("После выпадения сколько коридоров не будет паверапа")]
    [SerializeField] private ushort noBatteryDropLimit = 10;
    [SerializeField] private ushort noPotionDropLimit = 15;
    [SerializeField] private ushort noCameraDropLimit = 20;
    [SerializeField] private ushort noKeyDropLimit = 20;

    [Space]
    [Space]
    [Header("Drop battery")]
    [SerializeField] private int defaultChargeFromBattery = 20;
    [Tooltip("Процент увеличения заряда батарейки за один апгрейд")]
    [SerializeField] private int updatePercentageChargeForBattery = 4;
    [SerializeField] private int maxPercentageChargeForBattery = 40;

    [Space]
    [Space]
    [Header("Drop Potion")]
    [SerializeField] private float defaultPotionTime = 9f;
    [SerializeField] private float maxPotionTime = 18f;

    [Space]
    [Space]
    [Header("Drop Camera")]
    [SerializeField] private float defaultCameraTime = 4f;
    [SerializeField] private float maxCameraTime = 8f;

    [Space]
    [Space]
    [Header("Drop Key")]
    [SerializeField] private float defaultKeyDoors = 8f;
    [SerializeField] private float maxKeyDoors = 20f;

    //Player Camera config
    public float GetPlayerCameraSpeed()
    {
        return playerCameraSpeed;
    }

    public float GetPlayerCameraAccelerationSpeed()
    {
        return playerCameraAccelerationSpeed;
    }

    // Flashlight battery config
    public float GetBatteryTotalTime()
    {
        return batteryTotalTime;
    }
    public float GetFlashlightMaxIntensity()
    {
        return flashlightMaxIntensity;
    }
    public float GetChargeTimeBonusForOneDoor()
    {
        return chargeTimeBonusForOneDoor;
    }
    public float GetChargeTimeBonusForTwentyDoor()
    {
        return chargeTimeBonusForTwentyDoor;
    }
    public float GetReduceChargeFromBadDoor()
    {
        return reduceChargeFromBadDoor;
    }

    //Booster Controller config
    public float GetBoosterFlashingButtonSpeed()
    {
        return boosterFlashingButtonSpeed;
    }
    public float GetBoosterFlashingButtonInterval()
    {
        return boosterFlashingButtonInterval;
    }

    public float GetTimeToShowBoosters()
    {
        return timeToShowBoosters;
    }

    // Booster VIP key
    public float GetElevatorVIPTime()
    {
        return elevatorVIPTime;
    }

    public float GetElevatorVIPDoorsCount()
    {
        return elevatorVIPDoorsCount;
    }

    //Drop Generator
    public int GetSpawnBatteryAfterDoorsCount()
    {
        return spawnBatteryAfterDoorsCount;
    }
    public int GetSpawnCameraAfterDoorsCount()
    {
        return spawnCameraAfterDoorsCount;
    }
    public int GetSpawnPotionAfterDoorsCount()
    {
        return spawnPotionAfterDoorsCount;
    }
    public int GetSpawnKeyAfterDoorsCount()
    {
        return spawnKeyAfterDoorsCount;
    }

    // Chance To Drop
    public float GetChanceOfGettingBattery()
    {
        return chanceOfGettingBattery;
    }
    public float GetChanceOfGettingCamera()
    {
        return chanceOfGettingCamera;
    }
    public float GetChanceOfGettingPotion()
    {
        return chanceOfGettingPotion;
    }
    public float GetChanceOfGettingKey()
    {
        return chanceOfGettingKey;
    }

    // DropGeneratorLimits
    public ushort GetNoBatteryDropLimit()
    {
        return noBatteryDropLimit;
    }
    public ushort GetNoCameraDropLimit()
    {
        return noCameraDropLimit;
    }
    public ushort GetNoPotionDropLimit()
    {
        return noPotionDropLimit;
    }
    public ushort GetNoKeyDropLimit()
    {
        return noKeyDropLimit;
    }

    // Drop Battery
    public int GetDefaultChargeFromBattery ()
    {
        return defaultChargeFromBattery;
    }

    public int GetUpdatePercentageChargeForBattery()
    {
        return updatePercentageChargeForBattery;
    }

    public int GetMaxPercentageChargeForBattery()
    {
        return maxPercentageChargeForBattery;
    }

    // Drop Potion
    public float GetDefaultPotionTime()
    {
        return defaultPotionTime;
    }

    public float GetMaxPotionTime()
    {
        return maxPotionTime;
    }

    // Drop Camera
    public float GetDefaultCameraTime()
    {
        return defaultCameraTime;
    }

    public float GetMaxCameraTime()
    {
        return maxCameraTime;
    }

    //Drop Key
    public float GetDefaultKeyDoors()
    {
        return defaultKeyDoors;
    }

    public float GetMaxKeyDoors()
    {
        return maxKeyDoors;
    }
}
