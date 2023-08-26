using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private SoundClip[] backgroundSounds, SFXSounds;
    [SerializeField] private AudioSource SFXSource, backgroundSource;
    [SerializeField] private string initialBackgroundSound;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PlayBackground(initialBackgroundSound);
    }

    public void PlayBackground(string soundName)
    {
        SoundClip sound = Array.Find(backgroundSounds, sound => sound.name == soundName);

        if (sound == null)
        {
            Debug.LogFormat("No clip found for {0}", soundName);
            return;
        }

        backgroundSource.clip = sound.clip;
        backgroundSource.Play();
    }

    public void PlaySFXSound(string soundName)
    {
        SoundClip sound = Array.Find(SFXSounds, x => x.name == soundName);

        if (sound == null)
        {
            Debug.LogFormat("No clip found for {0}", soundName);
            return;
        }

        SFXSource.PlayOneShot(sound.clip);
    }
}
