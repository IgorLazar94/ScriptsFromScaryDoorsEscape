using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private GameObject startPosterWithoutLight;
    [SerializeField] private GameObject homePanel, shopPanel, mapsPanel, missionsPanel;
    [SerializeField] private GameObject upgradesPanel, itemsPanel, settingsPanel;
    [SerializeField] private GridLayoutGroup navGridLayoutGroup;
    [SerializeField] private TextMeshProUGUI keysText;
    [SerializeField] private Button musicButton, soundButton;
    private Sequence sequencePosterAnim;
    private GameObject[] mainPanels;

    private void OnEnable()
    {
        AudioManager.OnSoundSettingsChanged += CheckSoundState;
    }

    private void OnDisable()
    {
        AudioManager.OnSoundSettingsChanged -= CheckSoundState;
    }

    private void Start()
    {
        PosterFlashing();
        mainPanels = new GameObject[] { homePanel, shopPanel, mapsPanel, missionsPanel };
    }

    public void LoadNextScene()
    {
        sequencePosterAnim.Kill();
        shopManager.SavePlayerKeys();
        SceneManager.LoadScene(StringCollection.Instance.GameScene);
    }

    private void PosterFlashing()
    {
        sequencePosterAnim = DOTween.Sequence().SetUpdate(UpdateType.Normal, true);
        sequencePosterAnim.SetUpdate(true);
        for (int i = 0; i < 3; i++)
        {
            sequencePosterAnim.AppendCallback(() => EnableLamp());
            sequencePosterAnim.AppendInterval(0.2f);
            sequencePosterAnim.AppendCallback(() => startPosterWithoutLight.SetActive(false));
            sequencePosterAnim.AppendInterval(0.2f);
        }
        sequencePosterAnim.AppendCallback(() => startPosterWithoutLight.SetActive(false));
        for (int i = 0; i < 2; i++)
        {
            sequencePosterAnim.AppendCallback(() => EnableLamp());
            sequencePosterAnim.AppendInterval(0.1f);
            sequencePosterAnim.AppendCallback(() => startPosterWithoutLight.SetActive(false));
            sequencePosterAnim.AppendInterval(0.1f);
        }
        sequencePosterAnim.AppendInterval(2f);
        sequencePosterAnim.AppendCallback(() => startPosterWithoutLight.SetActive(false));
        sequencePosterAnim.SetLoops(-1);
        sequencePosterAnim.Play();
    }

    private void EnableLamp()
    {
        AudioManager.instance.PlaySFX(AudioCollection.FluorescentLamp);
        startPosterWithoutLight.SetActive(true);
    }

    public void ActivateNewPanel(GameObject _panel)
    {
        foreach (var panel in mainPanels)
        {
            panel.SetActive(false);
        }
        _panel.SetActive(true);
    }

    private void AlignmentNavigationButtons()
    {
        int buttonCount = 4;
        float cellWidth = Screen.width / buttonCount;
        navGridLayoutGroup.cellSize = new Vector2(cellWidth * 3, navGridLayoutGroup.cellSize.y);
        float spacingValue = 6f;
        navGridLayoutGroup.spacing = new Vector2(spacingValue, navGridLayoutGroup.spacing.y);

        //if (Screen.width > 1920)
        //{
        //    // ���� ��������� ��� ����������?
        //}
    }

    public void SwitchShopPanels(bool isShowItems)
    {
        if (isShowItems)
        {
            itemsPanel.SetActive(true);
            upgradesPanel.SetActive(false);
        }
        else
        {
            upgradesPanel.SetActive(true);
            itemsPanel.SetActive(false);
        }
    }

    public void UpdateKeysText(int keysCount)
    {
        keysText.text = ": " + keysCount.ToString();
    }

    public void ShowSettingPanel(bool value)
    {
        
        settingsPanel.gameObject.SetActive(value);
        if (value)
        {
            AudioManager.OnSoundSettingsChanged?.Invoke();
            CheckSoundState();
        }
    }

    private void CheckSoundState()
    {
        if (AudioManager.instance.musicSource.mute)
        {
            musicButton.GetComponent<Image>().sprite = SpriteCollection.Instance.music_Off;
        }
        else
        {
            musicButton.GetComponent<Image>().sprite = SpriteCollection.Instance.music_On;
        }

        if (AudioManager.instance.sfxSource.mute)
        {
            soundButton.GetComponent<Image>().sprite = SpriteCollection.Instance.sound_Off;
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = SpriteCollection.Instance.sound_On;
        }
    }
}
