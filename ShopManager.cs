using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    private bool isMapIDMethodCalled = false;
    private bool isMonsterIDMethodCalled = false;

    public Text ComboUpgradeCostText;
    public Text DropBatteryCostText;
    public Text DropCameraCostText;
    public Text DropPotionCostText;
    public Text DropKeyCostText;
    public Button comboButton;
    public Button batteryButton;
    public Button cameraButton;
    public Button potionButton;
    public Button keyButton;

    public Text MapID1Button;
    public Text MapID2Button;
    public Text MapID3Button;
    public Text MapID4Button;
    public Text MapID5Button;
    public Text MapID6Button;

    public Text MonsterID1Button;
    public Text MonsterID2Button;
    public Text MonsterID3Button;
    public Text MonsterID4Button;
    public Text MonsterID5Button;
    public Text MonsterID6Button;

    public Sprite[] buttonSprites;


    public Text PlayerKeysText;
    public Text boosterKeyVIPText;
    public Text boosterDublicatorText;
    public Text boosterFireworkText;
    public Text ComboUpgradeText;
    public Text dropBatteryLevelText;
    public Text dropCameraLevelText;
    public Text dropPotionLevelText;
    public Text dropKeyLevelText;

    public int PlayerKeys { get; private set; }
    public int boosterKeyVIP { get; private set; }
    public int BoosterDublicator { get; private set; }
    public int BoosterFirework { get; private set; }
    public int ComboUpgrade { get; private set; }
    public int DropBatteryLevel { get; private set; }
    public int DropCameraLevel { get; private set; }
    public int DropPotionLevel { get; private set; }
    public int DropKeyLevel { get; private set; }

    public int MapID { get; private set; }
    public int MonsterID { get; private set; }
    public Image[] mapSprites; // Масив Image для MapID
    public Image[] monsterSprites; // Масив Image для MonsterID
    public Image mapImage; // Посилання на Image для MapID
    public Image monsterImage; // Посилання на Image для MonsterID

    [SerializeField] private StartUI startUi;

    private void Start()
    {
        LoadPlayerPrefs();
    startUi.UpdateKeysText(PlayerKeys);
    UpdateButtonSprite(comboButton, ComboUpgrade);
    UpdateButtonSprite(batteryButton, DropBatteryLevel);
    UpdateButtonSprite(cameraButton, DropCameraLevel);
    UpdateButtonSprite(potionButton, DropPotionLevel);
    UpdateButtonSprite(keyButton, DropKeyLevel);

    UpdateUpgradeCostTexts();
    UpdateMapMonsterSprites();
   
    }

    public void SavePlayerKeys()
    {
        PlayerPrefs.SetInt(StringCollection.Instance.PlayerKeys, PlayerKeys);
        PlayerPrefs.Save();
    }

    private void LoadPlayerPrefs()
{
    PlayerKeys = PlayerPrefs.GetInt(StringCollection.Instance.PlayerKeys, 0);
    boosterKeyVIP = PlayerPrefs.GetInt(StringCollection.Instance.BoosterKeyVIPCount, 0);
    BoosterDublicator = PlayerPrefs.GetInt(StringCollection.Instance.BoosterDublicatorCount, 0);
    BoosterFirework = PlayerPrefs.GetInt(StringCollection.Instance.BoosterFireworkCount, 0);
    ComboUpgrade = PlayerPrefs.GetInt(StringCollection.Instance.ComboUpgrade, 1);
    DropBatteryLevel = PlayerPrefs.GetInt(StringCollection.Instance.DropBatteryLevel, 1);
    DropCameraLevel = PlayerPrefs.GetInt(StringCollection.Instance.DropCameraLevel, 1);
    DropPotionLevel = PlayerPrefs.GetInt(StringCollection.Instance.DropPotionLevel, 1);
    DropKeyLevel = PlayerPrefs.GetInt(StringCollection.Instance.DropKeyLevel, 1);
    MapID = PlayerPrefs.GetInt(StringCollection.Instance.MapID, 2);
    MonsterID = PlayerPrefs.GetInt(StringCollection.Instance.MonsterID, 6);

    CheckMethodCalled("MapID1Called", MapID1Button);
    CheckMethodCalled("MapID2Called", MapID2Button);
    CheckMethodCalled("MapID3Called", MapID3Button);
    CheckMethodCalled("MapID4Called", MapID4Button);
    CheckMethodCalled("MapID5Called", MapID5Button);
    CheckMethodCalled("MapID6Called", MapID6Button);

    CheckMethodCalled("MonsterID1Called", MonsterID1Button);
    CheckMethodCalled("MonsterID2Called", MonsterID2Button);
    CheckMethodCalled("MonsterID3Called", MonsterID3Button);
    CheckMethodCalled("MonsterID4Called", MonsterID4Button);
    CheckMethodCalled("MonsterID5Called", MonsterID5Button);
    CheckMethodCalled("MonsterID6Called", MonsterID6Button);
}
private void UpdateMapMonsterSprites()
{
    if (mapImage != null && mapSprites.Length > 0)
    {
        int mapSpriteIndex = Mathf.Clamp(MapID - 1, 0, mapSprites.Length - 1);
        mapImage.sprite = mapSprites[mapSpriteIndex].sprite;
    }

    if (monsterImage != null && monsterSprites.Length > 0)
    {
        int monsterSpriteIndex = Mathf.Clamp(MonsterID - 1, 0, monsterSprites.Length - 1);
        monsterImage.sprite = monsterSprites[monsterSpriteIndex].sprite;
    }
}


private void CheckMethodCalled(string calledKey, Text buttonText)
{
    if (PlayerPrefs.HasKey(calledKey))
    {
        // Якщо метод вже викликався, змініть текст кнопки
        ChangeButtonText(buttonText, "SET");
    }
}



public void MapID1()
{
    ProcessMapID(1, StringCollection.Instance.MapID, 9999, MapID1Button, "MapID1Called");
}

public void MapID2()
{
    ProcessMapID(2, StringCollection.Instance.MapID, 0, MapID2Button, "MapID2Called");
}
public void MapID3()
{
    ProcessMapID(3, StringCollection.Instance.MapID, 1, MapID3Button, "MapID3Called");
}
public void MapID4()
{
    ProcessMapID(4, StringCollection.Instance.MapID, 2, MapID4Button, "MapID4Called");
}
public void MapID5()
{
    ProcessMapID(5, StringCollection.Instance.MapID, 3, MapID5Button, "MapID5Called");
}
public void MapID6()
{
    ProcessMapID(6, StringCollection.Instance.MapID, 4, MapID6Button,"MapID6Called");
}

// Додайте інші методи MapID з відповідними вартостями

public void MonsterID1()
{
    ProcessMonsterID(1, StringCollection.Instance.MonsterID, 1, MonsterID1Button, "MonsterID1Called");
}

public void MonsterID2()
{
    ProcessMonsterID(2, StringCollection.Instance.MonsterID, 2, MonsterID2Button, "MonsterID2Called");
}

public void MonsterID3()
{
    ProcessMonsterID(3, StringCollection.Instance.MonsterID, 3, MonsterID3Button, "MonsterID3Called");
}
public void MonsterID4()
{
    ProcessMonsterID(4, StringCollection.Instance.MonsterID, 4, MonsterID4Button, "MonsterID4Called");
}
public void MonsterID5()
{
    ProcessMonsterID(5, StringCollection.Instance.MonsterID, 5, MonsterID5Button, "MonsterID5Called");
}
public void MonsterID6()
{
    ProcessMonsterID(6, StringCollection.Instance.MonsterID, 0, MonsterID6Button, "MonsterID6Called");
}


// Додайте інші методи MonsterID з відповідними вартостями

private void ProcessMapID(int mapID, string playerPrefsKey, int keysCost, Text buttonText, string calledKey)
{
    if (!PlayerPrefs.HasKey(calledKey) && PlayerKeys >= keysCost)
    {
        MapID = mapID;
        PlayerPrefs.SetInt(playerPrefsKey, MapID);
        PlayerPrefs.SetInt(calledKey, 1); // Зберегти інформацію, що метод був викликаний
        PlayerPrefs.Save();

        // Змінити текст кнопки
        ChangeButtonText(buttonText, "SET");

        // Віднімати PlayerKeys лише при першому виклику методу
        PlayerKeys -= keysCost;
        SavePlayerKeys();
    }
    else if (!PlayerPrefs.HasKey(calledKey))
    {
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}

private void ProcessMonsterID(int monsterID, string playerPrefsKey, int keysCost, Text buttonText, string calledKey)
{
    if (!PlayerPrefs.HasKey(calledKey) && PlayerKeys >= keysCost)
    {
        MonsterID = monsterID;
        PlayerPrefs.SetInt(playerPrefsKey, MonsterID);
        PlayerPrefs.SetInt(calledKey, 1); // Зберегти інформацію, що метод був викликаний
        PlayerPrefs.Save();

        // Змінити текст кнопки
        ChangeButtonText(buttonText, "SET");

        // Віднімати PlayerKeys лише при першому виклику методу
        PlayerKeys -= keysCost;
        SavePlayerKeys();
    }
    else if (!PlayerPrefs.HasKey(calledKey))
    {
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}


private void ChangeButtonText(Text buttonText, string newText)
{
    if (buttonText != null)
    {
        buttonText.text = newText;
    }
}



    public void PlayerKeysP()
    {
        PlayerKeys++;
        PlayerPrefs.SetInt(StringCollection.Instance.PlayerKeys, PlayerKeys);
        PlayerPrefs.Save();
    }


    public void BoosterKeyVIPCountP()
{
    if (PlayerKeys >= 900)
    {
        // Виконати дію лише у випадку, якщо PlayerKeys більше або дорівнює 500
        boosterKeyVIP++;
        PlayerPrefs.SetInt(StringCollection.Instance.BoosterKeyVIPCount, boosterKeyVIP);
        PlayerPrefs.Save();

        // Відняти 500 від PlayerKeys
        PlayerKeys -= 900;
        SavePlayerKeys(); // Зберегти оновлене значення PlayerKeys
    }
    else
    {
        // Обробити випадок, коли PlayerKeys менше 500 (недостатньо ключів)
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}


    public void BoosterDublicatorCountP()
{
    if (PlayerKeys >= 2100)
    {
        // Виконати дію лише у випадку, якщо PlayerKeys більше або дорівнює 500
        BoosterDublicator++;
        PlayerPrefs.SetInt(StringCollection.Instance.BoosterDublicatorCount, BoosterDublicator);
        PlayerPrefs.Save();

        // Відняти 500 від PlayerKeys
        PlayerKeys -= 2100;
        SavePlayerKeys(); // Зберегти оновлене значення PlayerKeys
    }
    else
    {
        // Обробити випадок, коли PlayerKeys менше 500 (недостатньо ключів)
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}

public void BoosterFireworkCountP()
{
    if (PlayerKeys >= 180)
    {
        // Виконати дію лише у випадку, якщо PlayerKeys більше або дорівнює 500
        BoosterFirework++;
        PlayerPrefs.SetInt(StringCollection.Instance.BoosterFireworkCount, BoosterFirework);
        PlayerPrefs.Save();

        // Відняти 500 від PlayerKeys
        PlayerKeys -= 180;
        SavePlayerKeys(); // Зберегти оновлене значення PlayerKeys
    }
    else
    {
        // Обробити випадок, коли PlayerKeys менше 500 (недостатньо ключів)
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}





private void UpdateButtonSprite(Button button, int spriteIndex)
{
    if (button != null && buttonSprites != null && spriteIndex >= 1 && spriteIndex <= 7)
    {
        button.image.sprite = buttonSprites[spriteIndex - 1];
    }
}


    private void UpdateUpgradeCostTexts()
    {
        ComboUpgradeCostText.text = " " + GetDropLevelCost1(ComboUpgrade, new int[] { 700, 1200, 2100, 3600, 5400, 11600 });
        DropBatteryCostText.text = " " + GetDropLevelCost1(DropBatteryLevel, new int[] { 800, 2000, 3200, 4800, 7200, 14400 });
        DropCameraCostText.text = " " + GetDropLevelCost1(DropCameraLevel, new int[] { 1200, 2500, 3800, 5400, 8100, 12500 });
        DropPotionCostText.text = " " + GetDropLevelCost1(DropPotionLevel, new int[] { 900, 2100, 3600, 4900, 6400, 8100 });
        DropKeyCostText.text = " " + GetDropLevelCost1(DropKeyLevel, new int[] { 1600, 3200, 5400, 7200, 10800, 16200 });
    }

    private string GetDropLevelCost1(int currentLevel, int[] costs)
{
    if (currentLevel > 0 && currentLevel <= costs.Length)
    {
        return costs[currentLevel - 1].ToString();
    }
    else if (currentLevel > costs.Length)
    {
        return "MAX Level";
    }
    return "0"; // Return "0" if the current level is out of bounds
}


    


    public void ComboUpgradelP()
    {
        int cost = GetDropLevelCost(ComboUpgrade, new int[] { 1, 2, 3, 4, 5, 6 });
        if (PlayerKeys >= cost)
        {
            if (ComboUpgrade < 7)
            {
                ComboUpgrade++;
                PlayerPrefs.SetInt(StringCollection.Instance.ComboUpgrade, ComboUpgrade);
                PlayerPrefs.Save();
                PlayerKeys -= cost;
                SavePlayerKeys();
                UpdateUpgradeCostTexts();

                // Оновіть спрайт comboButton
                UpdateButtonSprite(comboButton, ComboUpgrade);
            }
            else
            {
                Debug.Log("Рівень вже досягнув максимального значення.");
            }
        }
        else
        {
            Debug.Log("Недостатньо ключів для виконання цієї дії.");
        }
    }

    public void DropBatteryLevelP()
{
    int cost = GetDropLevelCost(DropBatteryLevel, new int[] { 800, 2000, 3200, 4800, 7200, 14400 });
    if (PlayerKeys >= cost)
    {
        if (DropBatteryLevel < 7) // Check if it's less than the maximum level
        {
            DropBatteryLevel++;
            PlayerPrefs.SetInt(StringCollection.Instance.DropBatteryLevel, DropBatteryLevel);
            PlayerPrefs.Save();
            PlayerKeys -= cost;
            SavePlayerKeys();
            UpdateUpgradeCostTexts();

            // Оновіть спрайт batteryButton
            UpdateButtonSprite(batteryButton, DropBatteryLevel);
        }
        else
        {
            Debug.Log("Рівень вже досягнув максимального значення.");
        }
    }
    else
    {
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}

public void DropCameraLevelP()
{
    int cost = GetDropLevelCost(DropCameraLevel, new int[] { 1200, 2500, 3800, 5400, 8100, 12500 });
    if (PlayerKeys >= cost)
    {
        if (DropCameraLevel < 7)
        {
            DropCameraLevel++;
            PlayerPrefs.SetInt(StringCollection.Instance.DropCameraLevel, DropCameraLevel);
            PlayerPrefs.Save();
            PlayerKeys -= cost;
            SavePlayerKeys();
            UpdateUpgradeCostTexts();

            // Оновіть спрайт comboButton
                UpdateButtonSprite(cameraButton, DropCameraLevel);
        }
        else
        {
            Debug.Log("Рівень вже досягнув максимального значення.");
        }
    }
    else
    {
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}

public void DropPotionLevelP()
{
    int cost = GetDropLevelCost(DropPotionLevel, new int[] { 900, 2100, 3600, 4900, 6400, 8100 });
    if (PlayerKeys >= cost)
    {
        if (DropPotionLevel < 7)
        {
            DropPotionLevel++;
            PlayerPrefs.SetInt(StringCollection.Instance.DropPotionLevel, DropPotionLevel);
            PlayerPrefs.Save();
            PlayerKeys -= cost;
            SavePlayerKeys();
            UpdateUpgradeCostTexts();

            // Оновіть спрайт comboButton
                UpdateButtonSprite(potionButton, DropPotionLevel);
        }
        else
        {
            Debug.Log("Рівень вже досягнув максимального значення.");
        }
    }
    else
    {
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}

public void DropKeyLevelP()
{
    int cost = GetDropLevelCost(DropKeyLevel, new int[] { 1600, 3200, 5400, 7200, 10800, 16200 });
    if (PlayerKeys >= cost)
    {
        if (DropKeyLevel < 7)
        {
            DropKeyLevel++;
            PlayerPrefs.SetInt(StringCollection.Instance.DropKeyLevel, DropKeyLevel);
            PlayerPrefs.Save();
            PlayerKeys -= cost;
            SavePlayerKeys();
            UpdateUpgradeCostTexts();
            // Оновіть спрайт comboButton
                UpdateButtonSprite(keyButton, DropKeyLevel);
        }
        else
        {
            Debug.Log("Рівень вже досягнув максимального значення.");
        }
    }
    else
    {
        Debug.Log("Недостатньо ключів для виконання цієї дії.");
    }
}



// Метод для отримання вартості підвищення рівня
private int GetDropLevelCost(int currentLevel, int[] costs)
{
    if (currentLevel >= 1 && currentLevel < costs.Length)
    {
        return costs[currentLevel-1];
    }
    else
    {
        return 0; // Невідомий рівень
    }
}



    private void Update()
    {
        UpdateUI();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PlayerKeysP();
            BoosterKeyVIPCountP();
        }
    }
    private void UpdateUI()
    {
        PlayerKeysText.text = PlayerKeys.ToString();
        boosterKeyVIPText.text = boosterKeyVIP.ToString();
        boosterDublicatorText.text = BoosterDublicator.ToString();
        boosterFireworkText.text = BoosterFirework.ToString();
        ComboUpgradeText.text = ComboUpgrade.ToString();
        dropBatteryLevelText.text = DropBatteryLevel.ToString();
        dropCameraLevelText.text = DropCameraLevel.ToString();
        dropPotionLevelText.text = DropPotionLevel.ToString();
        dropKeyLevelText.text = DropKeyLevel.ToString();
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs reset complete.");
    }
}
