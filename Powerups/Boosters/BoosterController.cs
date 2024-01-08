using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoosterController : MonoBehaviour
{
    [SerializeField] private UIController uIController;
    [SerializeField] private Image keyVIPButton, DublicatorKeyButton;
    [SerializeField] private PlayerCameraController playerCamera;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DropGenerator dropGenerator;
    private int boosterKeyVIP;
    private int boosterDublicator;
    private int boosterFirework;
    private Sequence flashingSequenceKey;
    private Sequence flashingSequenceDublicator;
    private float flashingSpeed;
    private float flashingInterval;
    private float elevatorVIPTime;
    private float elevatorVIPDoorsCount;
    private float timeToShowBoosters;
    private float timeToWorkFirework = 15f;

    private void Start()
    {
        LoadPlayerPrefs();
        if (boosterKeyVIP > 0)
        {
            keyVIPButton.gameObject.SetActive(true);
            Debug.Log(keyVIPButton + " keyVIPButton.Count");
        }
        if (boosterDublicator > 0)
        {
            DublicatorKeyButton.gameObject.SetActive(true);
            Debug.Log(DublicatorKeyButton + " DublicatorKeyButton.Count");
        }
        SetGameSettings();
    }

    public void StartShowBoosters()
    {
        uIController.ShowBoostersPanel(true);
        StartCoroutine(ShowBoosters());
    }

    private IEnumerator ShowBoosters()
    {
        yield return new WaitForSeconds(timeToShowBoosters / 2);
        FlashingBoosters();
        yield return new WaitForSeconds(timeToShowBoosters / 2);
        HideBoosters();
    }

    private void SetGameSettings()
    {
        flashingSpeed = GameSettings.Instance.GetBoosterFlashingButtonSpeed();
        flashingInterval = GameSettings.Instance.GetBoosterFlashingButtonInterval();
        elevatorVIPTime = GameSettings.Instance.GetElevatorVIPTime();
        elevatorVIPDoorsCount = GameSettings.Instance.GetElevatorVIPDoorsCount();
        timeToShowBoosters = GameSettings.Instance.GetTimeToShowBoosters();
    }

    private void LoadPlayerPrefs()
    {
        boosterKeyVIP = PlayerPrefs.GetInt(StringCollection.Instance.BoosterKeyVIPCount, 0);
        boosterDublicator = PlayerPrefs.GetInt(StringCollection.Instance.BoosterDublicatorCount, 0);
        boosterFirework = PlayerPrefs.GetInt(StringCollection.Instance.BoosterFireworkCount, 0);
    }

    public void FlashingBoosters()
    {
        if (flashingSequenceKey == null || !flashingSequenceKey.IsActive() &&
            flashingSequenceDublicator == null || !flashingSequenceDublicator.IsActive())
        {
            flashingSequenceKey = DOTween.Sequence();
            flashingSequenceKey.Append(keyVIPButton.DOFade(1f, flashingSpeed));
            flashingSequenceKey.AppendInterval(flashingInterval);
            flashingSequenceKey.Append(keyVIPButton.DOFade(0f, flashingSpeed));
            flashingSequenceKey.AppendInterval(flashingInterval);
            flashingSequenceKey.SetLoops(-1);

            flashingSequenceDublicator = DOTween.Sequence();
            flashingSequenceDublicator.Append(DublicatorKeyButton.DOFade(1f, flashingSpeed));
            flashingSequenceDublicator.AppendInterval(flashingInterval);
            flashingSequenceDublicator.Append(DublicatorKeyButton.DOFade(0f, flashingSpeed));
            flashingSequenceDublicator.AppendInterval(flashingInterval);
            flashingSequenceDublicator.SetLoops(-1);
        }
    }

    public void HideBoosters()
    {
        flashingSequenceKey.Kill();
        flashingSequenceDublicator.Kill();
        uIController.ShowBoostersPanel(false);
    }

    public void UseKeyVIPBooster()
    {
        if (boosterKeyVIP > 0)
        {
            boosterKeyVIP--;
            PlayerPrefs.SetInt(StringCollection.Instance.BoosterKeyVIPCount, boosterKeyVIP);
            PlayerPrefs.Save();
            keyVIPButton.gameObject.SetActive(false);
            playerCamera.SetKeyTime(elevatorVIPTime, elevatorVIPDoorsCount);
            playerCamera.ActivateKeyDropEffect(true);
        }
    }

    public void UseKeyDublicator()
    {
        if (boosterDublicator > 0)
        {
            boosterDublicator--;
            PlayerPrefs.SetInt(StringCollection.Instance.BoosterDublicatorCount, boosterDublicator);
            PlayerPrefs.Save();
            DublicatorKeyButton.gameObject.SetActive(false);
            gameManager.ActivateDublicatorMode();
        }
    }

    public void UseFirework()
    {
        if (boosterFirework > 0)
        {
            boosterFirework--;
            PlayerPrefs.SetInt(StringCollection.Instance.BoosterFireworkCount, boosterFirework);
            PlayerPrefs.Save();
            playerCamera.ActivateFireworkBooster(true);
            StartCoroutine(StartDiactivateFirework(timeToWorkFirework));
            dropGenerator.ActivateDropViewPanel(timeToWorkFirework, SpriteCollection.Instance.BoosterFirework);
        }
    }

    public IEnumerator StartDiactivateFirework(float time)
    {
        yield return new WaitForSeconds(time);
        gameManager.ActivateFirework(false);
        playerCamera.ActivateFireworkBooster(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            boosterKeyVIP++;
            PlayerPrefs.SetInt(StringCollection.Instance.BoosterKeyVIPCount, boosterKeyVIP);
            PlayerPrefs.Save();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            boosterDublicator++;
            PlayerPrefs.SetInt(StringCollection.Instance.BoosterDublicatorCount, boosterDublicator);
            PlayerPrefs.Save();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            boosterFirework++;
            PlayerPrefs.SetInt(StringCollection.Instance.BoosterFireworkCount, boosterFirework);
            PlayerPrefs.Save();
        }
    }
}
