using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour {

    [SerializeField]
    private GameObject startMenuRefference;
    [SerializeField]
    private GameObject settingsMenuRefference;

    public void NewGameButton ()
    {
        SceneManager.LoadScene (1);
    }
    public void LoadGameButton ()
    {
        // TODO
    }
    public void SettingsButton ()
    {
        startMenuRefference.SetActive (false);
        settingsMenuRefference.SetActive (true);
    }
    public void QuitButton ()
    {
        Application.Quit ();
    }

}
