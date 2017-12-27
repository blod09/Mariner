using System;
using UnityEngine;

public class BaseJob : MonoBehaviour
{

    protected GameObject progressBarPrefab;
    protected GameObject smokeCloudPrefab;

    protected ProgressBar progressBar;
    protected GameObject smokeCloud;

    protected Transform canvas;

    public float jobProgress { get; protected set; }
    public bool isBeingWorked { get; protected set; }

    protected Worker _assignedWorker;

    public Worker assignedWorker
    {
        get
        {
            return _assignedWorker;

        }

        protected set
        {
            if (assignedWorker != null)
                _assignedWorker.CurrentJob = null;

            _assignedWorker = value;

            if (assignedWorker != null)
                _assignedWorker.CurrentJob = this;
        }
    }

    protected virtual void Awake ()
    {
        // Get Canvas Refference
        canvas = GameObject.Find ("Canvas").transform;
        if (canvas == null)
        {
            throw new NullReferenceException (string.Format ("{0} failed to find and object name \"Canvas\"", GetType ().Name));
        }

        // Load Prefabs 
        progressBarPrefab = Resources.Load ("ProgressBar", typeof (GameObject)) as GameObject;
        smokeCloudPrefab = Resources.Load ("SmokeCloud", typeof (GameObject)) as GameObject;

    }

    protected virtual void OnEnable ()
    {
        MasterManager.TimeAndScoreMan.oneMinuteTick += OnTimeTick;
    }

    protected virtual void OnDisable ()
    {
        if (FindObjectOfType<MasterManager> () == null)
            return;

        MasterManager.TimeAndScoreMan.oneMinuteTick -= OnTimeTick;
    }

    protected virtual void OnTimeTick ()
    {

    }

    public virtual void TryToStartJob (Worker worker)
    {
        if (assignedWorker != null || isBeingWorked == true)
            return;
    }



}
