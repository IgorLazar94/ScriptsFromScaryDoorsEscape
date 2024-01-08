using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public static bool isActiveElevatorKey { get; private set; }
    public bool isReadyToMove { private get; set; }
    [SerializeField] private UIController uIController;
    [SerializeField] private TrackMainController trackController;
    [SerializeField] private ScreamerController screamerController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CountDownTimerFlashLight flashLightTimer;
    [SerializeField] private DropGenerator dropGenerator;
    [SerializeField] private GameObject fireworkObject;
    [SerializeField] private BoosterController boosterController;
    [SerializeField] private Animator flashLightAnimator;
    [SerializeField] private Animator cameraInElevatorAnimator;
    private float cameraSpeedWithPotion;
    private InputController inputController;
    private bool isScreamerMode = false;
    private int playerScores = 0;
    private int bestResult;
    private float timeToOffset = 0.8f;
    private float offsetXValue = 2.5f;
    private int scoresByOneDoor = 100;
    private int scoreMultiplier;
    private int defaultScoresMultiplier = 3;
    private int maxScoresMultiplier;
    private int comboUpgrade;
    private int keysForOneDoor = 10;
    private int doorCounter;
    private int doorCounterForPlayerView;
    private float keyDropTimeElevator;
    private int keyDropDoorsCount;
    private bool isActivatePotion;
    private int localPlayerKeys = 0;
    private float cameraSpeed;
    private float accelerationSpeed;

    private void Start()
    {
        comboUpgrade = PlayerPrefs.GetInt(StringCollection.Instance.ComboUpgrade, 1);
        maxScoresMultiplier = defaultScoresMultiplier + comboUpgrade + 1;
        scoreMultiplier = 0;
        SetGameSettings();
        isActiveElevatorKey = false;
        inputController = GetComponent<InputController>();
        bestResult = PlayerPrefs.GetInt(StringCollection.Instance.HighScore, 0);
        uIController.UpdatePassDoorText(playerScores, scoreMultiplier);
        isReadyToMove = true;
        SwitchFlashlight(true);
    }

    private void SetGameSettings()
    {
        cameraSpeed = GameSettings.Instance.GetPlayerCameraSpeed();
        accelerationSpeed = GameSettings.Instance.GetPlayerCameraAccelerationSpeed();
    }

    private void LateUpdate()
    {
        if (isReadyToMove && !isScreamerMode)
        {
            if (!isActivatePotion)
            {
                transform.Translate(Vector3.forward * cameraSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.forward * cameraSpeedWithPotion * Time.deltaTime);
            }
        }
    }

    public void SwitchFlashlight(bool isRun)
    {
        flashLightAnimator.SetBool(StringCollection.Instance.isRun, isRun);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagList.StopZone) && !other.transform.parent.GetComponent<DoorGroupController>().IsDoorOpen)
        {
            isReadyToMove = false;
            SwitchFlashlight(false);
            scoreMultiplier = 0;
            uIController.UpdatePassDoorText(playerScores, scoreMultiplier);
            //uIController.ActivateTimer(true);
        }

        if (other.CompareTag(TagList.PrepareZone))
        {
            var doorGroup = other.transform.parent.GetComponent<DoorGroupController>();
            if (!isActiveElevatorKey)
            {
                screamerController.PrepareToAttackMonster();
            }
            if (GameManager.IsActivateFirework)
            {
                DropFirework(doorGroup.GetOtherDoorPos());
                //doorGroup.IsPrepareScreamer = false;
            }
        }

        if (other.TryGetComponent(out ElevatorController elevatorController))
        {
            ReturnToDefaultOffset();
            ReturnCameraRotation(timeToOffset);
            isReadyToMove = false;
            cameraInElevatorAnimator.SetTrigger(StringCollection.Instance.InertialPush);
            flashLightTimer.ActivateElevatorMode(keyDropTimeElevator);
            flashLightTimer.ActivateLight(false);
            if (elevatorController.GetTypeOfElevator() == TypeOfElevator.Simple)
            {
                dropGenerator.ActivateDropViewPanel(keyDropTimeElevator, SpriteCollection.Instance.KeyDrop);
            }
            else if (elevatorController.GetTypeOfElevator() == TypeOfElevator.VIP)
            {
                dropGenerator.ActivateDropViewPanel(keyDropTimeElevator, SpriteCollection.Instance.BoosterKeyVIP);
            }
            elevatorController.SimulateMoveElevator();
            elevatorController.DisableTriggerZone();
            AddDynamicScoresOnElevator();
            StartCoroutine(ExitElevator(keyDropTimeElevator, elevatorController));
        }

        if (other.TryGetComponent(out Door door))
        {
            door.RemoveDoor();
            if (!door.isCorrectDoor)
            {

                flashLightTimer.ReduceTimeOfFlashLight();
            }
            else
            {
                // Хорошая дверь (без зарядки фонарика)
                doorCounter++;
                doorCounterForPlayerView++;
                scoreMultiplier++;
                scoreMultiplier = Mathf.Clamp(scoreMultiplier, 0, maxScoresMultiplier);
                AddDynamicScores();
                uIController.UpdatePassDoorText(playerScores, scoreMultiplier);
                // Зарядка фонарика от двери

                //flashLightTimer.AddTimeToFlashLight();
                //flashLightTimer.ReduceDischargeTime();

                localPlayerKeys += keysForOneDoor;
                uIController.UpdateLocalKeysText(localPlayerKeys);
                dropGenerator.SubtractNoDropCounter();
                dropGenerator.UpdateDoorCounter(doorCounter);
                dropGenerator.CheckChanceToSpawnDrop();

                // Сюда добавлять заряд за 20 пройденных дверей
                if (doorCounter % 20 == 0)
                {
                    flashLightTimer.AddTimeToFlashLightForTwentyDoors();
                }
            }
            if (!isScreamerMode)
            {
                ReturnToDefaultOffset();
                ReturnCameraRotation(timeToOffset);
            }
            else
            {
                ReturnCameraRotation(timeToOffset);
            }
        }
    }

    private void AddDynamicScoresOnElevator()
    {
        scoreMultiplier = maxScoresMultiplier;
        Sequence sequence = DOTween.Sequence();
        for (int i = 0; i < keyDropDoorsCount; i++)
        {
            localPlayerKeys += keysForOneDoor;
            sequence.AppendCallback(AddDynamicScores)
                    .AppendInterval(keyDropTimeElevator / keyDropDoorsCount);
        }
    }

    private void AddDynamicScores()
    {
        uIController.UpdateLocalKeysText(localPlayerKeys);
        int newScore = (int)(scoresByOneDoor * scoreMultiplier);
        DOTween.To(() => playerScores, x => playerScores = x, playerScores + newScore, timeToOffset * 2)
            .SetEase(Ease.Linear)
            .OnUpdate(() => uIController.FastUpdatePassDoorText(playerScores))
            .OnComplete(() =>
            {
                uIController.UpdatePassDoorText(playerScores, scoreMultiplier);
            });
    }

    private IEnumerator ExitElevator(float time, ElevatorController elevator)
    {
        yield return new WaitForSeconds(time);
        trackController.ChooseAndInitRandomTrack();
        elevator.OpenDoorAnimation();
        cameraInElevatorAnimator.SetTrigger(StringCollection.Instance.InertialPush);
        yield return new WaitForSeconds(1.5f); // time to open door animation
        trackController.ActivateElevatorMode(false, false);
        isReadyToMove = true;
        isActiveElevatorKey = false;
        flashLightTimer.ActivateLight(true);
        yield return new WaitForSeconds(5f);
        elevator.DestroyElevator();
    }

    public void RessurectPlayer()
    {
        transform.DOMoveY(5f, 0.1f);
        ReturnToDefaultOffset();
        ReturnCameraRotation(0.1f);
        inputController.enabled = true;
        isScreamerMode = false;
    }

    public void SetTypeOfDoor(TypeOfDoor _typeOfDoor)
    {
        SetCameraOffset(_typeOfDoor);
    }

    private void SetCameraOffset(TypeOfDoor _typeOfDoor)
    {
        float targetX = 0f;
        float targetRotationZ = 0f;

        switch (_typeOfDoor)
        {
            case TypeOfDoor.Right:
                targetX = offsetXValue;
                targetRotationZ = -10f;
                break;
            case TypeOfDoor.Left:
                targetX = -offsetXValue;
                targetRotationZ = 10f;
                break;
            default:
                break;
        }

        transform.DOMoveX(targetX, timeToOffset).SetEase(Ease.OutQuad);
        transform.DORotate(new Vector3(0f, 0f, targetRotationZ), timeToOffset).SetEase(Ease.OutQuad);
    }

    private void ReturnToDefaultOffset()
    {
        transform.DOMoveX(0f, 0.5f).SetEase(Ease.OutQuad);
    }

    private void ReturnCameraRotation(float value)
    {
        transform.DORotate(Vector3.zero, value).SetEase(Ease.OutQuad);
    }

    public void UpdateCameraSpeed()
    {
        cameraSpeed += accelerationSpeed;
    }

    public void UpdateCameraOffsetSpeed()
    {
        timeToOffset -= 0.2f;
        if (timeToOffset < 0.1f)
        {
            timeToOffset = 0.1f;
        }
    }

    public void CameraDrop()
    {
        transform.DORotate(new Vector3(-30f, 0f, 0f), 0.25f).SetEase(Ease.OutQuad);
        transform.DOMoveY(0.4f, 0.25f);
    }

    private void CheckBestResult()
    {
        if (playerScores > bestResult)
        {
            bestResult = playerScores;
            PlayerPrefs.SetInt(StringCollection.Instance.HighScore, bestResult);
            PlayerPrefs.Save();
        }
        //uIController.UpdateBestResultText(bestResult);
    }

    public int GetBestResult()
    {
        return bestResult;
    }

    public void AddChargeFlashlightFromBatteryDrop(int batteryChargeLevel)
    {
        flashLightTimer.AddChargeFromBattery(batteryChargeLevel);
    }

    public void ActivateSlowMotionFromPotion(float potionTime)
    {
        cameraSpeedWithPotion = cameraSpeed * 0.4f;
        flashLightTimer.ActivateSlowMotionFromPotion();
        StartCoroutine(flashLightTimer.DeactivateSlowMotionFromPotion(potionTime));
        // смещение камеры
        isActivatePotion = true;
        StartCoroutine(DisablePotionEffect(potionTime));
    }

    private IEnumerator DisablePotionEffect(float time)
    {
        yield return new WaitForSeconds(time);
        isActivatePotion = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = 0.3f;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            ActivateCameraDropEffect(10f);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateFireworkBooster(true);
        }
    }

    public void ActivateCameraDropEffect(float time)
    {
        StartCoroutine(trackController.EnableDoorEmissionFromCameraDrop(time));
        UIController.OnActivateCameraFadeFX.Invoke(true);
    }

    public void ActivateKeyDropEffect(bool isVIP)
    {
        trackController.ActivateElevatorMode(true, isVIP);
        isActiveElevatorKey = true;
        //if (isVIP)
        //{
        //    isVIPElevator = true;
        //}
    }

    public void SetKeyTime(float keyDropTime, float countDoors)
    {
        keyDropTimeElevator = keyDropTime;
        keyDropDoorsCount = (int)countDoors;
        doorCounterForPlayerView += (int)countDoors;
    }

    public int GetPointsForLosePanel()
    {
        return playerScores;
    }

    public int GetDoorsForLosePanel()
    {
        return doorCounterForPlayerView;
    }

    public int GetLocalPlayerKeys()
    {
        return localPlayerKeys;
    }

    public void ActivateFireworkBooster(bool value)
    {
        gameManager.ActivateFirework(value);
        fireworkObject.SetActive(value);
        fireworkObject.transform.DOMoveY(4f, 1f);
    }

    private void DropFirework(Vector3 doorBadPos)
    {
        Vector3 dropPos = new Vector3(doorBadPos.x, doorBadPos.y, doorBadPos.z + 5f);
        fireworkObject.transform.DOJump(dropPos, 1f, 1, 0.5f);
        fireworkObject.transform.parent = null;
        screamerController.RetreatTheMonster();
        dropGenerator.DisableFireworkViewPanel();
        StartCoroutine(boosterController.StartDiactivateFirework(1.0f));
        StartCoroutine(DestroyFirework());
    }

    private IEnumerator DestroyFirework()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(fireworkObject);
    }

    public Vector3 GetCameraPosition()
    {
        return transform.position;
    }

    public void PrepareMonsterToAttack()
    {
        if (!GameManager.IsActivateFirework)
        {
            // Размещение монстра 
            ScreamerController.SetMonsterToPos.Invoke(transform.position);
            inputController.enabled = false;
            isScreamerMode = true;
            // Атака монстра 
            StartCoroutine(MonsterAttack());
        }
    }

    private IEnumerator MonsterAttack()
    {
        yield return new WaitForSeconds(1.0f);
        screamerController.ActivateAttackMonster();
        CheckBestResult();
    }
}
