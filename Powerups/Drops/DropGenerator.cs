using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropGenerator : MonoBehaviour
{
    [SerializeField] private DropViewController dropViewController;
    [SerializeField] private BatteryDrop batteryDrop;
    [SerializeField] private PotionDrop potionDrop;
    [SerializeField] private CameraDrop cameraDrop;
    [SerializeField] private KeyDrop keyDrop;
    [SerializeField] private PlayerCameraController playerCamera;
    [SerializeField] private Transform dropContainer;
    private List<DropItem> listOfAvailableDrops = new List<DropItem>();
    private int doorCounter;
    private int counterForInitBattery;
    private int counterForInitCamera;
    private int counterForInitPotion;
    private int counterForInitKey;

    private float chanceOfGettingBattery;
    private float chanceOfGettingCamera;
    private float chanceOfGettingPotion;
    private float chanceOfGettingKey;

    private bool isBatteryAdded = false;
    private bool isCameraAdded = false;
    private bool isPotionAdded = false;
    private bool isKeyAdded = false;

    private int noBatteryDropCounter = 0;
    private int noCameraDropCounter = 0;
    private int noPotionDropCounter = 0;
    private int noKeyDropCounter = 0;

    private int noBatteryDropLimit;
    private int noCameraDropLimit;
    private int noPotionDropLimit;
    private int noKeyDropLimit;

    private void Start()
    {
        SetGameSettings();
    }

    private void SetGameSettings()
    {
        counterForInitBattery = GameSettings.Instance.GetSpawnBatteryAfterDoorsCount();
        counterForInitCamera = GameSettings.Instance.GetSpawnCameraAfterDoorsCount();
        counterForInitPotion = GameSettings.Instance.GetSpawnPotionAfterDoorsCount();
        counterForInitKey = GameSettings.Instance.GetSpawnKeyAfterDoorsCount();

        chanceOfGettingBattery = GameSettings.Instance.GetChanceOfGettingBattery();
        chanceOfGettingCamera = GameSettings.Instance.GetChanceOfGettingCamera();
        chanceOfGettingPotion = GameSettings.Instance.GetChanceOfGettingPotion();
        chanceOfGettingKey = GameSettings.Instance.GetChanceOfGettingKey();

        noBatteryDropLimit = (int)GameSettings.Instance.GetNoBatteryDropLimit();
        noCameraDropLimit = (int)GameSettings.Instance.GetNoCameraDropLimit();
        noPotionDropLimit = (int)GameSettings.Instance.GetNoPotionDropLimit();
        noKeyDropLimit = (int)GameSettings.Instance.GetNoKeyDropLimit();
    }

    private void CreateNewDrop(DropItem dropItem)
    {
        var newDrop = Instantiate(dropItem, ChooseRandomDropPos(), Quaternion.identity, dropContainer);
        newDrop.SetPlayerAndGeneratorToDropItem(playerCamera, this);
        //InitializeAnimation(newDrop);
    }

    private Vector3 ChooseRandomDropPos()
    {
        float randomX;
        Vector3 spawnPosition;

        int random = Random.Range(0, 2);
        if (random > 0)
        {
            randomX = 4f;
        }
        else
        {
            randomX = -4f;
        }
        spawnPosition = new Vector3(randomX, 6f, playerCamera.transform.position.z + 52f);
        return spawnPosition;
    }

    // Анимация появления дропа
    //private void InitializeAnimation(DropItem drop)
    //{
    //    drop.transform.localScale = Vector3.zero;
    //    drop.transform.DOScale(1f, 0.25f);
    //}

    public void UpdateDoorCounter(int value)
    {
        doorCounter = value;
        CheckNewActiveDrops();
    }

    private void CheckNewActiveDrops()
    {
        if (doorCounter >= counterForInitBattery && !isBatteryAdded)
        {
            if (!listOfAvailableDrops.Contains(batteryDrop))
            {
                listOfAvailableDrops.Add(batteryDrop);
                isBatteryAdded = true;
            }
        }

        if (doorCounter >= counterForInitCamera && !isCameraAdded)
        {
            if (!listOfAvailableDrops.Contains(cameraDrop))
            {
                listOfAvailableDrops.Add(cameraDrop);
                isCameraAdded = true;
            }
        }

        if (doorCounter >= counterForInitPotion && !isPotionAdded)
        {
            if (!listOfAvailableDrops.Contains(potionDrop))
            {
                listOfAvailableDrops.Add(potionDrop);
                isPotionAdded = true;
            }
        }

        if (doorCounter >= counterForInitKey && !isKeyAdded)
        {
            if (!listOfAvailableDrops.Contains(keyDrop))
            {
                listOfAvailableDrops.Add(keyDrop);
                isKeyAdded = true;
            }
        }
    }

    public void CheckChanceToSpawnDrop()
    {
        float randomCoefficient;

        if (listOfAvailableDrops.Contains(batteryDrop) && noBatteryDropCounter <= 0)
        {
            randomCoefficient = chanceOfGettingBattery;
            float random = Random.Range(0f, 100f);
            if (randomCoefficient >= random)
            {
                CreateNewDrop(batteryDrop);
                noBatteryDropCounter = noBatteryDropLimit;
                return;
            }
        }

        if (listOfAvailableDrops.Contains(potionDrop) && noPotionDropCounter <= 0)
        {
            randomCoefficient = chanceOfGettingPotion;
            float random = Random.Range(0f, 100f);
            if (randomCoefficient >= random)
            {
                CreateNewDrop(potionDrop);
                noPotionDropCounter = noPotionDropLimit;
                return;
            }
        }

        if (listOfAvailableDrops.Contains(cameraDrop) && noCameraDropCounter <= 0)
        {
            randomCoefficient = chanceOfGettingCamera;
            float random = Random.Range(0f, 100f);
            if (randomCoefficient >= random)
            {
                CreateNewDrop(cameraDrop);
                noCameraDropCounter = noCameraDropLimit;
                return;
            }
        }

        if (listOfAvailableDrops.Contains(keyDrop) && noKeyDropCounter <= 0)
        {
            randomCoefficient = chanceOfGettingKey;
            float random = Random.Range(0f, 100f);
            if (randomCoefficient >= random)
            {
                CreateNewDrop(keyDrop);
                noKeyDropCounter = noKeyDropLimit;
                return;
            }
        }

    }

    public void ActivateDropViewPanel(float durationEffect, Sprite dropIcon)
    {
        dropViewController.EnableViewPanel(durationEffect, dropIcon);
    }

    public void SubtractNoDropCounter()
    {
        noBatteryDropCounter--;
        noCameraDropCounter--;
        noPotionDropCounter--;
        noKeyDropCounter--;
    }

    public void DisableFireworkViewPanel()
    {
        dropViewController.DisableFireworkViewPanel();
    }
}
