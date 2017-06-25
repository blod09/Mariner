using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        SceneManager.LoadScene ("_SCENE_01_", LoadSceneMode.Single);
    }
}



