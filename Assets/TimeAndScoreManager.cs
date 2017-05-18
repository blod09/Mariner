﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeAndScoreManager : MonoBehaviour, IManager
{
    public ManagerState ManState { get; private set; }

    public int gold;
    private int minutes;
    private int hours;

    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text goldText;

    private WorkerController workerController;

    public Action oneMinuteTick;


    public void BootSequence ()
    {

        ManState = ManagerState.Initializing;
        Debug.Log (string.Format ("{0} is {1}...", this.GetType ().Name, ManState));

        workerController = FindObjectOfType<WorkerController> ();

        gold = 450;

        hours = 9;
        minutes = 0;
        StartCoroutine (Clock ());

        ManState = ManagerState.Online;
        Debug.Log (string.Format ("{0} is {1}.", this.GetType ().Name, ManState));

    }

    IEnumerator Clock ()
    {
        while (true)
        {
            minutes += 1;

            if (minutes >= 60.0f)
            {
                minutes = 0;
                hours += 1;

                gold -= workerController.TotalWorkerCost;
            }

            if (hours >= 24)
            {
                hours = 0;
            }



            string timeString = (hours >= 10) ? string.Format ("{0}:", hours) : string.Format ("0{0}:", hours);
            timeString += (minutes > 9) ? (minutes - minutes % 10).ToString () : "0" + (minutes - minutes % 10);
            timeText.text = timeString;
            goldText.text = gold + "$";

            if (oneMinuteTick != null)
                oneMinuteTick ();
            yield return new WaitForSeconds (.1f);
        }

    }
}