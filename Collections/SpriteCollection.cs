using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCollection : MonoBehaviour
{
    public static SpriteCollection Instance;

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

    public Sprite good1;
    public Sprite good2;
    public Sprite good3;

    public Sprite bad1;
    public Sprite bad2;
    public Sprite bad3;

    public Sprite music_On;
    public Sprite music_Off;
    public Sprite sound_On;
    public Sprite sound_Off;

    public Sprite BatteryDrop;
    public Sprite CameraDrop;
    public Sprite PotionDrop;
    public Sprite KeyDrop;

    public Sprite ADIcon;
    public Sprite BoosterKeyDublicator;
    public Sprite BoosterFirework;
    public Sprite BoosterKeyVIP;
}
