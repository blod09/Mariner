using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HireButton : MonoBehaviour
{
    [SerializeField]
    private WorkerController workerController;

    private Text text;
    private Button button;

    void Awake ()
    {
        text = GetComponentInChildren<Text> ();
        button = GetComponent<Button> ();
    }

    private void OnEnable ()
    {
        StartCoroutine (updateText (0.1f));
    }

    private void OnDisable ()
    {
        StopAllCoroutines ();
    }

    private IEnumerator updateText (float updateDelay)
    {
        while (true)
        {
            int workerCost = ProgressionFormulas.CurrentWorkerCost (workerController.GetCurrentNumberOfWorkers ());

            text.text = string.Format ("Hire ({0}$)", workerCost);

            if (workerCost > MasterManager.TimeAndScoreMan.Gold)
                button.interactable = false;
            else
                button.interactable = true;

            yield return new WaitForSeconds (updateDelay);
        }
    }


}
