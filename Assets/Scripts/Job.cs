using System;
using UnityEngine;

public class Job : MonoBehaviour
{
    private GameObject progressBarPrefab;
    private GameObject smokeCloudPrefab;

    private GameObject smokeCloudReference;


    private ProgressBar progressBar;

    [SerializeField]
    private int jobTimeInMinutes = 60;
    [SerializeField]
    private int jobPayout = 1000;

    public int JobTimeInMinutes { get { return jobTimeInMinutes; } }
    public int JobPayout { get { return jobPayout; } }


    public float jobProgress { get; private set; }
    public bool isBeingWorked { get; private set; }

    private Worker _assignedWorker;

    public Worker assignedWorker
    {
        get
        {
            return _assignedWorker;

        }

        private set
        {
            if (assignedWorker != null)
                _assignedWorker.CurrentJob = null;

            _assignedWorker = value;

            if (assignedWorker != null)
                _assignedWorker.CurrentJob = this;
        }
    }

    private Transform canvas;

    private void Awake ()
    {
        canvas = GameObject.Find ("Canvas").transform;
        if (canvas == null)
            throw new NullReferenceException (string.Format ("{0} failed to find and object name \"Canvas\"", GetType ().Name));

        progressBarPrefab = Resources.Load ("ProgressBar", typeof (GameObject)) as GameObject;
        smokeCloudPrefab = Resources.Load ("SmokeCloud", typeof (GameObject)) as GameObject;


        //TEST FUNCTION
        RandomizeJobStats ();

    }

    private void OnEnable ()
    {
        MasterManager.TimeAndScoreMan.oneMinuteTick += OnTimeTick;
    }

    private void OnDisable ()
    {
        if (FindObjectOfType<MasterManager> () == null)
            return;

        MasterManager.TimeAndScoreMan.oneMinuteTick -= OnTimeTick;
    }

    private void OnTimeTick ()
    {
        if (isBeingWorked == false)
            return;

        if (jobProgress >= 1.0f)
        {
            Debug.Log ("Job Finished");
            FinishJob ();
        }
        if (isBeingWorked)
        {
            float jobProgressToAdd = (1.0f / jobTimeInMinutes) * ProgressionFormulas.WorkerSpeed (assignedWorker.Level);
            //print (jobProgressToAdd);
            jobProgress += jobProgressToAdd;
            progressBar.Progress = jobProgress;
        }
    }

    public void StartJob (Worker worker)
    {
        assignedWorker = worker;
        WorkerController.HideWorker (worker);
        isBeingWorked = true;

        progressBar = Instantiate (progressBarPrefab, canvas, true).GetComponent<ProgressBar> ();
        smokeCloudReference = Instantiate (smokeCloudPrefab, this.gameObject.transform, false);

        progressBar.targetToFollow = gameObject.transform;
        progressBar.SetColor (new Color (88f / 255f, 133f / 255f, 103f / 255f), Color.green);
        progressBar.Progress = jobProgress;
    }

    public void FinishJob ()
    {
        WorkerController.ShowWorker (assignedWorker);

        assignedWorker.AddExp (JobTimeInMinutes * 0.08f);
        assignedWorker = null;
        isBeingWorked = false;
        MasterManager.TimeAndScoreMan.Gold += jobPayout;

        Destroy (smokeCloudReference);
        Destroy (progressBar.gameObject);
        Destroy (GetComponentInChildren<Ship> ().gameObject);
        Destroy (this);
    }

    private void RandomizeJobStats ()
    {
        jobTimeInMinutes = UnityEngine.Random.Range (30, 120);
        jobPayout = UnityEngine.Random.Range (10, 75);
    }


}