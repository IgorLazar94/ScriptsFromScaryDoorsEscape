using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringCollection : MonoBehaviour
{
    public static StringCollection Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // PlayerPrefs
    [HideInInspector] public string PlayerNickname = "PlayerNickname";
    [HideInInspector] public string PlayerKeys = "PlayerKeys";
    [HideInInspector] public string HighScore = "HighScore";
    [HideInInspector] public string BoosterKeyVIPCount = "BoosterKeyVIP";
    [HideInInspector] public string BoosterDublicatorCount = "BoosterDublicator";
    [HideInInspector] public string BoosterFireworkCount = "BoosterFirework";
    [HideInInspector] public string ComboUpgrade = "ComboUpgrade";
    [HideInInspector] public string DropBatteryLevel = "DropBatteryLevel";
    [HideInInspector] public string DropCameraLevel = "DropCameraLevel";
    [HideInInspector] public string DropPotionLevel = "DropPotionLevel";
    [HideInInspector] public string DropKeyLevel = "DropKeyLevel";

    // Level Parameters PlayerPrefs
    [HideInInspector] public string MapID = "MapID";
    /* MapID = 1 => Main
     * MapID = 2 => Castle
     * MapID = 3 => Hospital
     * MapID = 4 => School
     * MapID = 5 => Subway */


    [HideInInspector] public string MonsterID = "MonsterID";
    /* MonsterID = 1 => Clown
     * MonsterID = 2 => Maniac
     * MonsterID = 3 => Gastaroid
     * MonsterID = 4 => Slug
     * MonsterID = 5 => Undertaker
     * MonsterID = 6 => Vampire */

    // Animations
        //Door
    [HideInInspector] public string isOpen = "isOpen";
        //Monster
    [HideInInspector] public string Attack = "Attack";
    [HideInInspector] public string TypeOfAttack = "TypeOfAttack";
    [HideInInspector] public string Prepare = "Prepare";
    [HideInInspector] public string Retreat = "Retreat";
        //Player
    [HideInInspector] public string isRun = "isRun";
    //Camera
    [HideInInspector] public string InertialPush = "InertialPush";
    // Scene names
    [HideInInspector] public string GameScene = "GameScene";
    [HideInInspector] public string StartScene = "StartScene";
}
