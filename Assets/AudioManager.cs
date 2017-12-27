using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoSingleton <AudioManager> {

    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private AudioSource sfx;

    public const string bgPrefsKey = "BGMusicVolume";
    public const string sfxPrefsKey = "SFXVolume";

    void Start () {
        DontDestroyOnLoad (gameObject);
        UpdateVolume ();
	}

    public void UpdateVolume ()
    {
        if (PlayerPrefs.HasKey (bgPrefsKey) == false)
        {
            PlayerPrefs.SetFloat (bgPrefsKey, 0);
        }
        if (PlayerPrefs.HasKey (sfxPrefsKey) == false)
        {
            PlayerPrefs.SetFloat (sfxPrefsKey, 0);
        }


        AudioMixerGroup bgMixer = bgMusic.outputAudioMixerGroup;
        AudioMixerGroup sfxMixer = sfx.outputAudioMixerGroup;
        bgMixer.audioMixer.SetFloat ("Volume", PlayerPrefs.GetFloat (bgPrefsKey));
        sfxMixer.audioMixer.SetFloat ("Volume", PlayerPrefs.GetFloat (sfxPrefsKey));

    }
}
