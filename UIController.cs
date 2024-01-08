using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static Action<bool> OnActivateCameraFadeFX;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScreamerController screamerController;
    [SerializeField] private InputController inputController;
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private Sprite[] scaryImages;
    [SerializeField] private Image scaryPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject hUDPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject inputFieldPanel;
    [SerializeField] private GameObject tableRecordPanel;
    [SerializeField] private GameObject clarifyingPanel;
    [SerializeField] private GameObject boostersPanel;
    [SerializeField] private CountDownTimerFlashLight countDownTimer;
    [SerializeField] private TextMeshProUGUI doorCounterText, scoresMultText, messageText;
    [SerializeField] private TextMeshProUGUI losePointsText, loseDoorsText, loseKeysText;
    //[SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private Button startGameButton, pauseButton;
    [SerializeField] private Button ressurectADbutton, ressurectKeysButton;
    [SerializeField] private Button soundPauseButton, musicPauseButton;
    [SerializeField] private Button rewardADkeyMultiplierButton;
    [SerializeField] private GameObject ressurectChancePanel;
    [SerializeField] private TextMeshProUGUI ressurectCostKeysText, localKeysHUDText;
    [SerializeField] private Image ressurectChanceImageFull;
    [SerializeField] private Image fadeEffectImage, greenEffect;
    private float timeToWatchRessurectAD = 2f;
    private Tween ressurectChanceTween;
    private int surviveKeysLevel = 3;
    private int surviveADLevel = 1;
    private int costKeysForSurvive;
    private bool isActivePauseTimer = false;

    private void OnEnable()
    {
        OnActivateCameraFadeFX += ActivateFadeEffect;
        AudioManager.OnSoundSettingsChanged += CheckSoundState;
    }

    private void OnDisable()
    {
        OnActivateCameraFadeFX -= ActivateFadeEffect;
        AudioManager.OnSoundSettingsChanged -= CheckSoundState;
    }

    public void ShowScaryImage()
    {
        var random = UnityEngine.Random.Range(0, scaryImages.Length);
        scaryPanel.sprite = scaryImages[random];
        scaryPanel.gameObject.SetActive(true);
        inputController.enabled = false;
        PlayRandomScreamerSound();
        StartCoroutine(gameManager.GameOver());
    }

    public void UpdateLocalKeysText(int localKeysCount)
    {
        localKeysHUDText.text = localKeysCount.ToString();
    }

    public void ShowBoostersPanel(bool value)
    {
        boostersPanel.SetActive(value);
    }

    private void PlayRandomScreamerSound()
    {
        var random = UnityEngine.Random.Range(0, 2);
        if (random > 0)
        {
            AudioManager.instance.PlaySFX(AudioCollection.ImageScreamer_1);
        }
        else
        {
            AudioManager.instance.PlaySFX(AudioCollection.ImageScreamer_2);
        }
    }

    public void ShowLosePanel(bool isActivate, int countOfPoints, int countOfDoors, int countOfKeys)
    {
        hUDPanel.SetActive(!isActivate);
        losePanel.SetActive(isActivate);
        UpdateLosePanelText(countOfPoints, countOfDoors, countOfKeys);
        gameManager.ChangeGlobalPlayerKeys(countOfKeys);
        if (!isActivate)
        {
            ContinueGame();
        }
    }

    private void UpdateLosePanelText(int countOfPoints, int countOfDoors, int countOfKeys)
    {
        losePointsText.text = countOfPoints.ToString();
        loseDoorsText.text = countOfDoors.ToString();
        loseKeysText.text = countOfKeys.ToString();
    }

    public void ShowPausePanel(bool isActivate)
    {
        if (isActivePauseTimer)
        {
            return;
        }
        pausePanel.SetActive(isActivate);
        hUDPanel.SetActive(!isActivate);
        if (isActivate)
        {
            Time.timeScale = 0;
            inputController.enabled = false;
            ShowBoostersPanel(false);
            CheckSoundState();
        }
        else
        {
            isActivePauseTimer = true;
            DOTween.Sequence()
           .SetUpdate(UpdateType.Normal, true)
           .AppendCallback(() => ShowTextMessage("3"))
           .AppendInterval(1f)
           .AppendCallback(() => ShowTextMessage("2"))
           .AppendInterval(1f)
           .AppendCallback(() => ShowTextMessage("1"))
           .AppendInterval(1f)
           .AppendCallback(() => ShowTextMessage("Start"))
           .AppendInterval(1f);
        }
    }

    private void CheckSoundState()
    {
        if (AudioManager.instance.musicSource.mute)
        {
            musicPauseButton.GetComponent<Image>().sprite = SpriteCollection.Instance.music_Off;
        }
        else
        {
            musicPauseButton.GetComponent<Image>().sprite = SpriteCollection.Instance.music_On;
        }

        if (AudioManager.instance.sfxSource.mute)
        {
            soundPauseButton.GetComponent<Image>().sprite = SpriteCollection.Instance.sound_Off;
        }
        else
        {
            soundPauseButton.GetComponent<Image>().sprite = SpriteCollection.Instance.sound_On;
        }
    }

    private void ShowTextMessage(string message)
    {
        messageText.gameObject.SetActive(true);
        messageText.DOFade(0f, 0f);
        messageText.text = message;
        messageText.DOFade(1f, 0.25f).OnComplete(() => messageText.gameObject.SetActive(false));
        if (message == "Start")
        {
            ContinueGame();
            Time.timeScale = 1;
            inputController.enabled = true;
            isActivePauseTimer = false;
        }
    }

    public void ContinueGame()
    {
        ActivatePauseButton(true);
        RessurectPlayer();
        hUDPanel.SetActive(true);
        losePanel.SetActive(false);
        scaryPanel.gameObject.SetActive(false);
        cameraController.RessurectPlayer();
        gameManager.StartGame();
        //screamerController.ChooseRandomMonster();
        inputController.enabled = true;
    }

    public void ShowClarifyingMenuPanel()
    {
        clarifyingPanel.gameObject.SetActive(true);
    }

    public void BackToMenu()
    {
        gameManager.LoadMenuLevel();
    }

    public void HideClarifyingMenuPanel()
    {
        clarifyingPanel.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        gameManager.RestartLevel();
    }

    //public void ActivateTimer(bool isActivate)
    //{
    //    countDownTimer.gameObject.SetActive(isActivate);
    //    countDownTimer.StartTimer();
    //}

    public void UpdatePassDoorText(int scores, float scoresView)
    {
        doorCounterText.text = scores.ToString();
        ActivateScoresMultipilerText(scoresView);
    }

    public void FastUpdatePassDoorText(int scores)
    {
        doorCounterText.text = scores.ToString();
    }

    private void ActivateScoresMultipilerText(float _scoresView)
    {
        if (_scoresView >= 3)
        {
            scoresMultText.DOFade(0f, 0.25f).OnComplete(() => ChangeMultText(_scoresView));
        }
        else
        {
            scoresMultText.DOFade(0f, 0f);
        }
    }

    private void ChangeMultText(float _scoresView)
    {
        string formattedMultiplier = "X" + _scoresView.ToString();
        scoresMultText.text = formattedMultiplier;
        scoresMultText.DOFade(1f, 0.25f);
    }

    //public void UpdateBestResultText(int count)
    //{
    //    string message = "Your best result: ";
    //    highScoreText.text = message + count.ToString();
    //}

    public void HideStartPanel()
    {
        inputController.enabled = true;
        ActivatePauseButton(true);
        startPanel.SetActive(false);
        hUDPanel.SetActive(true);
        gameManager.StartGame();
        AudioManager.instance.PlayRandomMusic();
    }

    private void ActivatePauseButton(bool isActive)
    {
        pauseButton.enabled = isActive;
    }

    public void ActivateInputFieldPanel(bool value)
    {
        inputFieldPanel.SetActive(value);
    }

    public void ActivateStartGameButton(bool isActivate)
    {
        startGameButton.enabled = isActivate;
    }

    public void ShowTableRecord()
    {
        losePanel.SetActive(false);
        tableRecordPanel.SetActive(true);
    }

    public void ActivateRessurectChancePanel()
    {
        ActivatePauseButton(false);
        float timeCoefficient = 0.2f;
        CheckButtonsRequirements();
        ressurectChancePanel.SetActive(true);
        ressurectCostKeysText.text = costKeysForSurvive.ToString();
        //ressurectKeysText.text = GameManager.playerKeysGlobal.ToString();
        ressurectChanceTween = DOTween.To(() => ressurectChanceImageFull.fillAmount, 
                                           x => ressurectChanceImageFull.fillAmount = x, 
                                           1f, 
                                           timeToWatchRessurectAD * timeCoefficient)
            .OnComplete(HideRessurectChancePanel);
        Time.timeScale = timeCoefficient;
    }

    public void CompleteRessurectChanceTween()
    {
        if (ressurectChanceTween != null && ressurectChanceTween.IsActive())
        {
            ressurectChanceTween.Kill();
            HideRessurectChancePanel();
        }
    }

    private void CheckButtonsRequirements()
    {
        CalculateKeysCostForSurvive();
        if (GameManager.playerKeysGlobal > costKeysForSurvive && surviveKeysLevel > 0)
        {
            ressurectKeysButton.enabled = true;
            ressurectKeysButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            ressurectKeysButton.enabled = false;
            ressurectKeysButton.GetComponent<Image>().color = Color.red;
        }

        if (surviveADLevel > 0)
        {
            ressurectADbutton.enabled = true;
            ressurectADbutton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            ressurectADbutton.enabled = false;
            ressurectADbutton.GetComponent<Image>().color = Color.red;
        }
    }

    private void HideRessurectChancePanel()
    {
        ActivatePauseButton(true);
        ressurectChanceImageFull.fillAmount = 0f;
        ressurectChancePanel.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(gameManager.GameOver());
    }

    public void RessurectPlayer()
    {
        ressurectChanceImageFull.fillAmount = 0f;
        ressurectChancePanel.SetActive(false);
        Time.timeScale = 1f;
        ressurectChanceTween.Kill();
    }

    public void RessurectPlayerForKeys()
    {
        gameManager.ChangeGlobalPlayerKeys(-costKeysForSurvive);
        surviveKeysLevel--;
        ContinueGame();
    }

    public void RessurectPlayerForAD()
    {
        surviveADLevel--;
        ressurectChanceTween.Kill();
        ContinueGame();
    }

    private void CalculateKeysCostForSurvive()
    {
        switch (surviveKeysLevel)
        {
            case 1:
                costKeysForSurvive = 2000;
                break;
            case 2:
                costKeysForSurvive = 1000;
                break;
            case 3:
                costKeysForSurvive = 500;
                break;
            default:
                break;
        }
    }

    public void ActivateFadeEffect(bool isActivate)
    {
        if (isActivate)
        {
            fadeEffectImage.DOFade(1f, 0.25f).OnComplete(() => ActivateGreenPanel());
        }
        else
        {
            fadeEffectImage.DOFade(1f, 0.25f).OnComplete(() => DeactivateGreenPanel());
        }
    }

    private void ActivateGreenPanel()
    {
        greenEffect.gameObject.SetActive(true);
        fadeEffectImage.DOFade(0f, 0.25f);
    }

    private void DeactivateGreenPanel()
    {
        greenEffect.gameObject.SetActive(false);
        fadeEffectImage.DOFade(0f, 0.25f);
    }

    public void ShowADKeyMultiplierButton(bool value, Sprite buttonIcon)
    {
        rewardADkeyMultiplierButton.enabled = value;
        rewardADkeyMultiplierButton.gameObject.GetComponent<Image>().sprite = buttonIcon;
    }

    public void HideADKeyMultipleButton()
    {
        rewardADkeyMultiplierButton.gameObject.SetActive(false);
    }
}
