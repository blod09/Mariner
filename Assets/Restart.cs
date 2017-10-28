using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{


    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKey (KeyCode.R))
            SceneManager.LoadScene ("_SCENE_01_", LoadSceneMode.Single);

    }
}
