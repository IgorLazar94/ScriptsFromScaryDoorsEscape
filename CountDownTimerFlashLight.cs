using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimerFlashLight : MonoBehaviour
{
    public static Action OnShortenTheTimer;
    [SerializeField] private PlayerCameraController playerCameraController;
    [SerializeField] private UIController uIController;
    [SerializeField] private Image progressBarEmpty;
    [SerializeField] private Light flashlight;
    [SerializeField] private ScreamerController screamerController;
    private float maxIntensity;
    private float totalTime;
    private float timeRemaining;
    private bool isTimerRunning = true;
    private float chargeTimeBonusForOneDoor;
    private float chargeTimeBonusForTwentyDoor;
    private float dischargeTime = 1f;
    private float slowDischargeTime;
    private float dischargeTimeMax = 2f;
    private float batteryChargeValue;
    private float maxChargeValue;
    private float defaultChargeFromBattery;
    private float updatePercentageChargeForBattery;
    private bool isActivePotion = false;
    private bool isActiveElevatorMode = false;
    private float reduceChargeFromBadDoor;

    void Start()
    {
        GetGameSettings();

        ResetTimer();
        StartTimer();
        StartCoroutine(ConstantDoubleFlashing());
    }

    private void GetGameSettings()
    {
        maxChargeValue = (float)GameSettings.Instance.GetMaxPercentageChargeForBattery() * 0.01f;
        defaultChargeFromBattery = (float)GameSettings.Instance.GetDefaultChargeFromBattery() * 0.01f;
        updatePercentageChargeForBattery = (float)GameSettings.Instance.GetUpdatePercentageChargeForBattery() * 0.01f;
        maxChargeValue *= totalTime;
        defaultChargeFromBattery *= totalTime;
        updatePercentageChargeForBattery *= 100f;

        totalTime = GameSettings.Instance.GetBatteryTotalTime();
        maxIntensity = GameSettings.Instance.GetFlashlightMaxIntensity();
        chargeTimeBonusForOneDoor = GameSettings.Instance.GetChargeTimeBonusForOneDoor();
        chargeTimeBonusForTwentyDoor = GameSettings.Instance.GetChargeTimeBonusForTwentyDoor();
        reduceChargeFromBadDoor = GameSettings.Instance.GetReduceChargeFromBadDoor();
    }

    private void OnEnable()
    {
        OnShortenTheTimer += ShortenTheTimer;
    }

    private void OnDisable()
    {
        OnShortenTheTimer -= ShortenTheTimer;
    }

    void Update()
    {
        if (isTimerRunning)
        {
            ConstantDischargeOfTheFlashlight();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReduceTimeOfFlashLight();
        }
    }

    private void ConstantDischargeOfTheFlashlight()
    {

        if (timeRemaining > 0)
        {
            if (isActiveElevatorMode)
            {
                return;
            }

            if (!isActivePotion)
            {
                timeRemaining -= dischargeTime * Time.deltaTime;
            }
            else
            {
                timeRemaining -= slowDischargeTime * Time.deltaTime;
            }
            UpdateTimerDisplay();
            CalculateIntensity();
        }
        else
        {
            if (GameManager.IsActivateFirework)
            {
                return;
            }
            timeRemaining = 0;
            isTimerRunning = false;
            //uIController.ShowScaryImage();
            playerCameraController.PrepareMonsterToAttack();
            //gameObject.SetActive(false);
        }
    }

    private void CalculateIntensity()
    {
        float targetIntensity = Mathf.Lerp(0f, maxIntensity, timeRemaining / totalTime);
        flashlight.intensity = targetIntensity + 0.5f;
    }

    void UpdateTimerDisplay()
    {
        progressBarEmpty.fillAmount = timeRemaining / totalTime;
    }

    public void ResetTimer()
    {
        timeRemaining = totalTime;
        UpdateTimerDisplay();
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        UpdateTimerDisplay();
    }

    private void ShortenTheTimer()
    {
        if (isTimerRunning)
        {
            totalTime--;
            if (totalTime < 0)
            {
                if (GameManager.IsActivateFirework)
                {
                    return;
                }
                totalTime = 0;
                isTimerRunning = false;
                uIController.ShowScaryImage();
                gameObject.SetActive(false);
            }
            UpdateTimerDisplay();
        }
    }

    public void AddTimeToFlashLightForOneDoor()
    {
        timeRemaining += chargeTimeBonusForOneDoor;
        timeRemaining = Mathf.Clamp(timeRemaining, 0f, totalTime);
    }

    public void AddTimeToFlashLightForTwentyDoors()
    {
        timeRemaining += chargeTimeBonusForTwentyDoor;
        timeRemaining = Mathf.Clamp(timeRemaining, 0f, totalTime);
    }

    public void ReduceTimeOfFlashLight()
    {
        timeRemaining -= reduceChargeFromBadDoor;
        timeRemaining = Mathf.Clamp(timeRemaining, 0f, totalTime);
    }

    public void ReduceDischargeTime()
    {
        dischargeTime += 0.0071f;  // Коэффициент 1 >> 2 / 140 дверей
        if (dischargeTime > dischargeTimeMax)
        {
            dischargeTime = dischargeTimeMax;
        }
    }

    public IEnumerator ConstantDoubleFlashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeRemaining * 3f);
            if (timeRemaining * 3f >= 1f && !isActiveElevatorMode)
            {
                for (int i = 0; i < 2; i++)
                {
                    ShowMonster();
                    flashlight.enabled = false;
                    yield return new WaitForSeconds(0.1f);
                    screamerController.ActivateMonster(false);
                    flashlight.enabled = true;
                    AudioManager.instance.PlaySFX(AudioCollection.FluorescentLamp);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private void ShowMonster()
    {
        Vector3 spawnPoint = playerCameraController.GetCameraPosition();
        ScreamerController.SetMonsterToPos.Invoke(new Vector3(playerCameraController.GetCameraPosition().x + 4f, 5f, spawnPoint.z + 10f));
    }

    public void AddChargeFromBattery(int batteryChargeLevel)
    {
        batteryChargeValue = defaultChargeFromBattery + (batteryChargeLevel * updatePercentageChargeForBattery);
        batteryChargeValue = Mathf.Clamp(batteryChargeValue, 0f, maxChargeValue);
        timeRemaining += batteryChargeValue;
    }

    public void ActivateSlowMotionFromPotion()
    {
        slowDischargeTime = dischargeTime * 0.4f;
        isActivePotion = true;
    }

    public IEnumerator DeactivateSlowMotionFromPotion(float time)
    {
        yield return new WaitForSeconds(time);
        isActivePotion = false;
    }

    public void ActivateElevatorMode(float time)
    {
        isActiveElevatorMode = true;
        StartCoroutine(DiactivateElevatorMode(time));
    }

    private IEnumerator DiactivateElevatorMode(float time)
    {
        yield return new WaitForSeconds(time + 0.5f);
        isActiveElevatorMode = false;
    }

    public void ActivateLight(bool isActive)
    {
        flashlight.enabled = isActive;
    }
}
