using System.Collections;
using UnityEngine;

public class ShipJob : BaseJob
{
    private GameObject inspectionBarPrefab;

    private GameObject spyGlassIconPrefab;
    private GameObject sawIconPrefab;
    private GameObject crossedSawIconPrefab;

    private AudioClip soundEffect;
    private AudioSource audioSource;


    private GameObject spyGlassIcon;
    private GameObject sawIcon;
    private GameObject crossedSawIcon;


    private GameObject currentIcon;

    private Vector3 iconOffset;

    private Ship shipToWorkOn;

    public int JobTimeInMinutes { get { return shipToWorkOn.stats.timeRequirement; } }
    public int JobPayout { get { return shipToWorkOn.stats.payment; } }

    public float inspectionProgress { get; private set; }
    public bool isBeingInspected { get; private set; }

    protected override void Awake ()
    {
        base.Awake ();

        // Load Extra Prefabs.
        inspectionBarPrefab = Resources.Load ("InspectionBar", typeof (GameObject)) as GameObject;

        spyGlassIconPrefab = Resources.Load ("SpyGlassIcon", typeof (GameObject)) as GameObject;
        sawIconPrefab = Resources.Load ("SawIcon", typeof (GameObject)) as GameObject;
        crossedSawIconPrefab = Resources.Load ("CrossedSawIcon", typeof (GameObject)) as GameObject;

        soundEffect = Resources.Load ("Hammer", typeof (AudioClip)) as AudioClip;

        audioSource = GetComponent<AudioSource> ();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource> ();
            audioSource.clip = soundEffect;
            audioSource.loop = true;
            audioSource.Stop ();
        }

        // TODO: remove hardcoding the offset.
        iconOffset = new Vector3 (0.0f, 10.0f, 0.0f);

        spyGlassIcon = CreateIcon (spyGlassIconPrefab);
        sawIcon = CreateIcon (sawIconPrefab);
        crossedSawIcon = CreateIcon (crossedSawIconPrefab);

        currentIcon = spyGlassIcon;
        sawIcon.SetActive (false);
        crossedSawIcon.SetActive (false);


    }

    protected override void OnTimeTick ()
    {
        if (isBeingWorked == true)
        {
            if (jobProgress >= 1.0f)
            {
                //Debug.Log ("Job Finished");
                isBeingWorked = false;
                FinishJob ();
                return;
            }

            float jobProgressToAdd = (1.0f / shipToWorkOn.stats.timeRequirement) * ProgressionFormulas.WorkerSpeed (assignedWorker.Level);
            jobProgress += jobProgressToAdd;
            progressBar.Progress = jobProgress;
        }
        else if (isBeingInspected == true)
        {
            if (inspectionProgress >= 1.0f)
            {
                //Debug.Log ("Inspection Finished");
                isBeingInspected = false;
                FinishInspection ();
                return;
            }

            float inspectionProgressToAdd = (1.0f / Constants.Instance.InspectionTimeInMinutes);// *ProgressionFormulas.WorkerSpeed (assignedWorker.Level);
            inspectionProgress += inspectionProgressToAdd;
            progressBar.Progress = inspectionProgress;
        }
    }


    public void AssignShip (Ship ship)
    {
        shipToWorkOn = ship;
        StartCoroutine (UpdateIcon ());

    }

    public override void TryToStartJob (Worker worker)
    {
        base.TryToStartJob (worker);

        if (shipToWorkOn.stats.isInspected == false && isBeingInspected == false)
        {
            //shipToWorkOn.ShowTooltip ();
            InspectShip (worker);
        }
        else if (isBeingInspected == false && isBeingWorked == false && MasterManager.TimeAndScoreMan.IsEnoughResources (shipToWorkOn.stats.woodRequirement, shipToWorkOn.stats.clothRequirement, shipToWorkOn.stats.tarRequirement))
        {
            WorkOnShip (worker);
        }

    }

    public void InspectShip (Worker worker)
    {

        inspectionProgress = 0.0f;
        assignedWorker = worker;
        WorkerController.HideWorker (worker);
        isBeingInspected = true;

        progressBar = Instantiate (inspectionBarPrefab, canvas, false).GetComponent<ProgressBar> ();
        progressBar.targetToFollow = gameObject.transform;
        progressBar.Progress = inspectionProgress;


    }


    protected void FinishInspection ()
    {
        assignedWorker.AddExp (ProgressionFormulas.ExpForInspection ());

        WorkerController.ShowWorker (assignedWorker);
        assignedWorker = null;

        ShipStats s = shipToWorkOn.stats;
        s.isInspected = true;
        shipToWorkOn.stats = s;
        isBeingInspected = false;

        Destroy (progressBar.gameObject);


    }

    public void WorkOnShip (Worker worker)
    {
        jobProgress = 0.0f;
        TimeAndScoreManager m = MasterManager.TimeAndScoreMan;
        m.RemoveWood (shipToWorkOn.stats.woodRequirement);
        m.RemoveCloth (shipToWorkOn.stats.clothRequirement);
        m.RemoveTar (shipToWorkOn.stats.tarRequirement);

        assignedWorker = worker;
        WorkerController.HideWorker (worker);
        isBeingWorked = true;

        progressBar = Instantiate (progressBarPrefab, canvas, false).GetComponent<ProgressBar> ();
        smokeCloud = Instantiate (smokeCloudPrefab, this.gameObject.transform, false);

        progressBar.targetToFollow = gameObject.transform;
        progressBar.Progress = jobProgress;

        audioSource.Play ();

    }



    public void FinishJob ()
    {
        WorkerController.ShowWorker (assignedWorker);

        assignedWorker.AddExp (ProgressionFormulas.ExpByShipFixTime (JobTimeInMinutes));

        MasterManager.TimeAndScoreMan.Gold += shipToWorkOn.stats.payment;
        DockManager.totalNumberOfFixedShips++;
        //print ("Total number of fixed ships: " + DockManager.totalNumberOfFixedShips);

        Destroy (smokeCloud);
        Destroy (progressBar.gameObject);
        Destroy (GetComponentInChildren<Ship> ().gameObject);

        StopAllCoroutines ();
        currentIcon = null;
        Destroy (spyGlassIcon);
        Destroy (sawIcon);
        Destroy (crossedSawIcon);
        audioSource.Stop ();

        assignedWorker = null;
        isBeingWorked = false;
        Destroy (this);
    }

    //private void RandomizeJobStats ()
    //{
    //    jobTimeInMinutes = UnityEngine.Random.Range (30, 120);
    //    jobPayout = UnityEngine.Random.Range (50, 300);
    //}


    private GameObject CreateIcon (GameObject iconPrefab)
    {
        GameObject go = Instantiate (iconPrefab, transform, false);
        go.transform.position += iconOffset;
        go.transform.localScale = Vector3.one * 2.4f;
        return go;
    }

    private IEnumerator UpdateIcon ()
    {
        while (true)
        {
            if (currentIcon != null && (isBeingInspected || isBeingWorked))
            {
                currentIcon.SetActive (false);
                currentIcon = null;
            }
            else if (shipToWorkOn.stats.isInspected == false && currentIcon != spyGlassIcon && !(isBeingInspected || isBeingWorked))
            {
                if (currentIcon != null)
                    currentIcon.SetActive (false);

                currentIcon = spyGlassIcon;
                currentIcon.SetActive (true);
            }
            else if (MasterManager.TimeAndScoreMan.IsEnoughResources (
              shipToWorkOn.stats.woodRequirement,
              shipToWorkOn.stats.clothRequirement,
              shipToWorkOn.stats.tarRequirement
              ) && currentIcon != sawIcon && shipToWorkOn.stats.isInspected == true && !(isBeingInspected || isBeingWorked))
            {
                if (currentIcon != null)
                    currentIcon.SetActive (false);

                currentIcon = sawIcon;
                currentIcon.SetActive (true);
            }
            else if (currentIcon != crossedSawIcon && !MasterManager.TimeAndScoreMan.IsEnoughResources (
              shipToWorkOn.stats.woodRequirement,
              shipToWorkOn.stats.clothRequirement,
              shipToWorkOn.stats.tarRequirement
              ) && shipToWorkOn.stats.isInspected == true && !(isBeingInspected || isBeingWorked))
            {
                if (currentIcon != null)
                    currentIcon.SetActive (false);

                currentIcon = crossedSawIcon;
                currentIcon.SetActive (true);
            }

            yield return new WaitForSeconds (0.1f);
        }
    }
}