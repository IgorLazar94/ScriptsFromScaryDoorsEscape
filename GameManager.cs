using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static int playerKeysGlobal { get; set; }
    public static bool IsActivateFirework { get; private set; }

    public static Action OnActivateSlowmod;
    public string playerNickname { get; private set; }
    public int playerScore { get; private set; }
    public float realtimeSpeed { get; set; }
    [SerializeField] private UIController uIController;
    [SerializeField] private TMP_InputField nicknameInputField;
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private BoosterController boosterController;
    private bool isActiveDublicatorMode = false;
    private void Start()
    {
        realtimeSpeed = 1f;
        IsActivateFirework = false;
        LoadPlayerKeys();
        //CheckNickname();
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        OnActivateSlowmod += ShowRessurectPlayerChance;
    }

    private void OnDisable()
    {
        OnActivateSlowmod -= ShowRessurectPlayerChance;
    }

    private void CheckNickname()
    {
        string savedNickname = PlayerPrefs.GetString(StringCollection.Instance.PlayerNickname);
        if (string.IsNullOrEmpty(savedNickname))
        {
            uIController.ActivateInputFieldPanel(true);
            uIController.ActivateStartGameButton(false);
        }
        else
        {
            playerNickname = savedNickname;
        }
    }

    private void SavePlayerKeys(int newKeys)
    {
        PlayerPrefs.SetInt(StringCollection.Instance.PlayerKeys, playerKeysGlobal);
        PlayerPrefs.Save();
    }

    private void LoadPlayerKeys()
    {
        playerKeysGlobal = PlayerPrefs.GetInt(StringCollection.Instance.PlayerKeys, 0);
    }

    public void SaveNickname()
    {
        playerNickname = nicknameInputField.text;
        if (string.IsNullOrEmpty(playerNickname))
        {
            playerNickname = "Player";
        }
        PlayerPrefs.SetString(StringCollection.Instance.PlayerNickname, playerNickname);
        uIController.ActivateInputFieldPanel(false);
        uIController.ActivateStartGameButton(true);
    }

    public void StartGame()
    {
        Time.timeScale = realtimeSpeed;
        boosterController.StartShowBoosters();
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.8f);
        cameraController.CameraDrop();
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 0;
        var points = cameraController.GetPointsForLosePanel();
        var doors = cameraController.GetDoorsForLosePanel();
        var localKeys = cameraController.GetLocalPlayerKeys();
        if (isActiveDublicatorMode)
        {
            uIController.ShowADKeyMultiplierButton(false, SpriteCollection.Instance.BoosterKeyDublicator);
            localKeys *= 3;
            uIController.ShowLosePanel(true, points, doors, localKeys);
        }
        else
        {
            uIController.ShowLosePanel(true, points, doors, localKeys);
            uIController.ShowADKeyMultiplierButton(true, SpriteCollection.Instance.ADIcon);
        }
        SavePlayerKeys(localKeys + playerKeysGlobal);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(StringCollection.Instance.GameScene);
    }

    public void LoadMenuLevel()
    {
        SceneManager.LoadScene(StringCollection.Instance.StartScene);
    }

    public int GetPlayerScore()
    {
        return cameraController.GetBestResult();
    }

    public void ChangeGlobalPlayerKeys(int count)
    {
        playerKeysGlobal += count;
        SavePlayerKeys(playerKeysGlobal);
    }

    private void ShowRessurectPlayerChance()
    {
        uIController.ActivateRessurectChancePanel();
    }

    public void ActivateDublicatorMode()
    {
        isActiveDublicatorMode = true;
    }

    public void ShowADAndUpdateKeys()
    {
        var points = cameraController.GetPointsForLosePanel();
        var doors = cameraController.GetDoorsForLosePanel();
        var localKeys = cameraController.GetLocalPlayerKeys();
        localKeys *= 2;
        uIController.ShowLosePanel(true, points, doors, localKeys);
        uIController.HideADKeyMultipleButton();
    }

    public void ActivateFirework(bool value)
    {
        IsActivateFirework = value;
    }
}
