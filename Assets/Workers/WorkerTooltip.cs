using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WorkerTooltip : MonoBehaviour
{
    private const float statUpdateInSeconds = .1f;

    private Worker worker;
    private Transform target;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private Image panelGraphic;
    [SerializeField]
    private Text panelText;
    [SerializeField]
    private Text levelProgressText;
    [SerializeField]
    private ProgressBar expBar;

    private void Awake ()
    {

        if (panelGraphic == null || panelText == null)
            throw new Exception ("Error: Worker tooltip doesn't have a reference to the panel or the text objects.");

        Hide ();

    }

    private void LateUpdate ()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp (transform.position, Camera.main.WorldToScreenPoint (target.gameObject.transform.position) + offset, 0.5f);
        }
    }

    public void Show (Worker workerData, Transform targetToFollow)
    {
        this.target = targetToFollow;
        worker = workerData;
        expBar.gameObject.SetActive (true);
        StartCoroutine (UpdateStats (statUpdateInSeconds));
        panelGraphic.enabled = true;
        panelText.enabled = true;
    }

    public void Hide ()
    {
        target = null;
        worker = null;
        panelGraphic.enabled = false;
        panelText.enabled = false;
        expBar.gameObject.SetActive (false);
        StopAllCoroutines ();
    }

    private IEnumerator UpdateStats (float updateInterval)
    {

        while (true)
        {
            string tooltipString = worker.Name + "\n\n";
            tooltipString += "Level " + worker.Level + "\n\n";
            tooltipString += "exp";
            expBar.Progress = worker.levelProgressInPercent;
            panelText.text = tooltipString;

            levelProgressText.text = string.Format ("{0:F0}/{1} ({2:F2}%)", worker.Exp, worker.ExpToLevelUp, worker.levelProgressInPercent * 100);

            yield return new WaitForSeconds (updateInterval);
        }
    }
}
