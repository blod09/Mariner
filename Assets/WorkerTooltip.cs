using System;
using UnityEngine;
using UnityEngine.UI;

public class WorkerTooltip : MonoBehaviour
{

    private Transform target;

    [SerializeField]
    private Vector3 offset;


    private Image panelGraphic;
    private Text panelText;

    private void Awake ()
    {
        panelGraphic = GetComponentInChildren<Image> ();
        panelText = GetComponentInChildren<Text> ();

        if (panelGraphic == null || panelText == null)
            throw new Exception ("Error: Worker tooltip doesn't have a reference to the panel or the text objects.");

    }

    private void Update ()
    {
        if (target == null)
            return;
        transform.position = Vector3.Lerp (transform.position, Camera.main.WorldToScreenPoint (target.position) + offset, 0.5f);
        //print (target.gameObject.name);
        //print (target.transform.position);
    }

    public void Show (Worker worker, Transform target)
    {
        this.target = target;
        string tooltipString = "Name: " + worker.Name;
        foreach (Skill s in worker.Skills)
        {
            tooltipString += "\n" + s.Type + ": " + s.SkillLevel;
        }

        panelText.text = tooltipString;
        panelGraphic.enabled = true;
        panelText.enabled = true;
    }

    public void Hide ()
    {
        target = null;
        panelGraphic.enabled = false;
        panelText.enabled = false;
    }
}
