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
    [SerializeField]
    private Color expBarColor;
    [SerializeField]
    private Color expBarBG;

    private void Awake ()
    {
        expBar.SetColor (expBarBG, expBarColor);

        if (panelGraphic == null || panelText == null)
            throw new Exception ("Error: Worker tooltip doesn't have a reference to the panel or the text objects.");

        Hide ();

    }


    private void LateUpdate ()
    {

        if (target != null)
        {

            Vector3 targetPos = Vector3.Lerp (transform.position, Camera.main.WorldToScreenPoint (target.position) + offset, 0.5f);
            targetPos.z = 0.0f;
            transform.position = targetPos;
        }


    }

    public void Show (Worker workerData, Transform targetToFollow)
    {
        this.target = targetToFollow;
        worker = workerData;
        StartCoroutine (UpdateStats (statUpdateInSeconds));
        panelGraphic.gameObject.SetActive (true);


    }

    public void Hide ()
    {
        target = null;
        worker = null;
        panelGraphic.gameObject.SetActive (false);
        StopAllCoroutines ();
    }

    private IEnumerator UpdateStats (float updateInterval)
    {
        float speedBonus;

        while (true)
        {
            string tooltipString = worker.Name + "\n\n\n";
            speedBonus = ProgressionFormulas.WorkerSpeed (worker.Level) * 100.0f - 100.0f;

            if (speedBonus > 0)
            {
                tooltipString += "Level " + worker.Level + "\n\n";
                tooltipString += string.Format ("<size={0}> Speed Bonus:  +{1:F0}%</size>", levelProgressText.fontSize - 2, speedBonus);
            }
            else
            {
                tooltipString += "\nLevel " + worker.Level + "\n";
            }
            tooltipString += "<size=" + (levelProgressText.fontSize - 2) + ">" + "\n\n\n\nEXP</size>";
            expBar.Progress = worker.levelProgressInPercent;
            panelText.text = tooltipString;

            levelProgressText.text = string.Format ("{0:F0}/{1} ({2:F2}%)", worker.Exp, worker.ExpToLevelUp, worker.levelProgressInPercent * 100);

            yield return new WaitForSeconds (updateInterval);
        }
    }
}
