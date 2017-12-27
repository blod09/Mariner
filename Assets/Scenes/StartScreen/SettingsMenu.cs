using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {


    [SerializeField]
    private GameObject startMenuRefference;
    [SerializeField]
    private GameObject settingsMenuRefference;
    [SerializeField]
    private Slider bgSliderRefference;
    [SerializeField]
    private Slider sfxSliderRefference;


    void OnEnable () {

        if (PlayerPrefs.HasKey (AudioManager.bgPrefsKey) == false)
        {
            PlayerPrefs.SetFloat (AudioManager.bgPrefsKey, 0);
        }
        if (PlayerPrefs.HasKey (AudioManager.sfxPrefsKey) == false)
        {
            PlayerPrefs.SetFloat (AudioManager.sfxPrefsKey, 0);
        }

        bgSliderRefference.value = PlayerPrefs.GetFloat (AudioManager.bgPrefsKey);
        sfxSliderRefference.value = PlayerPrefs.GetFloat (AudioManager.sfxPrefsKey);
    }
	
    public void BGSlider(float value)
    {
        PlayerPrefs.SetFloat (AudioManager.bgPrefsKey, value);
        AudioManager.Instance.UpdateVolume ();
    }
    public void SFXSlider(float value)
    {
        PlayerPrefs.SetFloat (AudioManager.sfxPrefsKey, value);
        AudioManager.Instance.UpdateVolume ();
    }
    public void BackButton ()
    {
        startMenuRefference.SetActive (true);
        settingsMenuRefference.SetActive (false);
    }
}
