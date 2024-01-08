using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static Action OnSoundSettingsChanged;
    public static AudioManager instance;
    public AudioSource musicSource, sfxSource;
    [SerializeField] private Sound[] musicSounds, sfxSounds;
    [SerializeField] private Image soundButtonSprite;
    private bool isSFXPlaying = false;


    private void Awake()
    {
        MakeSingleton();
    }

    private void Start()
    {
        PlaySFX(AudioCollection.Intro);
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Music not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        //if (isSFXPlaying)
        //{
        //    return;
        //}

        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound not found");
        }
        else
        {
            isSFXPlaying = true;
            sfxSource.PlayOneShot(s.clip);
            StartCoroutine(ResetSFXPlayingStatus(s.clip.length));
        }
    }

    private IEnumerator ResetSFXPlayingStatus(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSFXPlaying = false;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        if (musicSource.mute == true)
        {
            soundButtonSprite.sprite = SpriteCollection.Instance.music_Off;
        }
        else
        {
            soundButtonSprite.sprite = SpriteCollection.Instance.music_On;
        }
        OnSoundSettingsChanged?.Invoke();
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        if (sfxSource.mute == true)
        {
            soundButtonSprite.sprite = SpriteCollection.Instance.sound_Off;
        }
        else
        {
            soundButtonSprite.sprite = SpriteCollection.Instance.sound_On;
        }
        OnSoundSettingsChanged?.Invoke();
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    private void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRandomMusic()
    {
        int random = UnityEngine.Random.Range(0, musicSounds.Length);
        if (random > 0)
        {
            PlayMusic(AudioCollection.BackgroundMusic_1);
        }
        else
        {
            PlayMusic(AudioCollection.BackgroundMusic_2);
        }
    }
}
