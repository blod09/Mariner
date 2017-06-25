using UnityEngine;
using UnityEngine.UI;

public class WorkerTile : MonoBehaviour
{

    private Text text;
    private RawImage image;

    private void Awake ()
    {
        text = GetComponentInChildren<Text> ();
        image = GetComponentInChildren<RawImage> ();
    }

    public void SetTile (Worker worker)
    {
        text.text = string.Format ("{0}   Level {1}\n\n{2}$ Per Hour", worker.Name, worker.Level, worker.salary);
        image.texture = WorkerVisuals.workerPortraits[(int)worker.type];
    }
}
