using UnityEngine;

public enum ResourceType
{
    Wood,
    Cloth,
    Tar
}


public class ResourceBuilding : BaseJob
{
    public static int lumbermillCount { get; private set; }
    public static int tailorCount { get; private set; }
    public static int refineryCount { get; private set; }


    [SerializeField]
    private ResourceType buildingType;

    private int resourceGenerationLevel = 1;
    private int resourceCapacityLevel = 1;

    TimeAndScoreManager scoreManager;

    private int numberOfResourcesGenerated { get { return ProgressionFormulas.ResourcesGeneratedByLevel (resourceGenerationLevel); } }

    public ResourceType BuildingType { get; protected set; }
    public int ResourceGenerationLevel { get { return resourceGenerationLevel; } }
    public int ResourceCapacityLevel { get { return resourceCapacityLevel; } }



    protected override void Awake ()
    {
        base.Awake ();
        scoreManager = MasterManager.TimeAndScoreMan;
    }

    protected override void OnEnable ()
    {
        base.OnEnable ();
        switch (buildingType)
        {
            case ResourceType.Wood:
                scoreManager.maxWood += ProgressionFormulas.ResourcesCapacityByLevel (resourceCapacityLevel);
                lumbermillCount++;
                break;
            case ResourceType.Cloth:
                scoreManager.maxCloth += ProgressionFormulas.ResourcesCapacityByLevel (resourceCapacityLevel);
                tailorCount++;
                break;
            case ResourceType.Tar:
                scoreManager.maxTar += ProgressionFormulas.ResourcesCapacityByLevel (resourceCapacityLevel);
                refineryCount++;
                break;
            default:
                break;
        }
    }

    protected override void OnDisable ()
    {
        base.OnDisable ();

        switch (buildingType)
        {
            case ResourceType.Wood:
                scoreManager.maxWood -= ProgressionFormulas.ResourcesCapacityByLevel (resourceCapacityLevel);
                lumbermillCount--;
                break;
            case ResourceType.Cloth:
                scoreManager.maxCloth -= ProgressionFormulas.ResourcesCapacityByLevel (resourceCapacityLevel);
                tailorCount--;
                break;
            case ResourceType.Tar:
                scoreManager.maxTar -= ProgressionFormulas.ResourcesCapacityByLevel (resourceCapacityLevel);
                refineryCount--;
                break;
            default:
                break;
        }
    }

#if UNITY_EDITOR

    private void Update ()
    {
        if (Input.GetButtonDown ("Jump"))
        {
            LevelUpBuildingCapacity ();
            LevelUpBuildingGeneration ();
        }
    }

#endif


    protected override void OnTimeTick ()
    {
        base.OnTimeTick ();

        if (isBeingWorked == true)
        {
            if (jobProgress >= 1.0f)
            {
                //Debug.Log ("Job Finished");
                FinishGeneratingResource (assignedWorker);
                return;
            }

            float jobProgressToAdd = (1.0f / Constants.Instance.ResourceGenerationTime) * ProgressionFormulas.WorkerSpeed (assignedWorker.Level);
            jobProgress += jobProgressToAdd;
            progressBar.Progress = jobProgress;
        }
    }

    public override void TryToStartJob (Worker worker)
    {
        base.TryToStartJob (worker);

        switch (buildingType)
        {
            case ResourceType.Wood:
                if (scoreManager.CurrentWood >= scoreManager.maxWood)
                    return;
                break;
            case ResourceType.Cloth:
                if (scoreManager.CurrentCloth >= scoreManager.maxCloth)
                    return;
                break;
            case ResourceType.Tar:
                if (scoreManager.CurrentTar >= scoreManager.maxTar)
                    return;
                break;
            default:
                break;
        }

        GenerateResource (worker);

    }

    private void GenerateResource (Worker worker)
    {
        assignedWorker = worker;
        WorkerController.HideWorker (worker);
        isBeingWorked = true;


        progressBar = Instantiate (progressBarPrefab, canvas, false).GetComponent<ProgressBar> ();
        smokeCloud = Instantiate (smokeCloudPrefab, this.gameObject.transform, false);

        jobProgress = 0.0f;
        progressBar.targetToFollow = gameObject.transform;
        progressBar.Progress = jobProgress;
    }

    private void FinishGeneratingResource (Worker worker)
    {
        WorkerController.ShowWorker (assignedWorker);
        assignedWorker.AddExp (ProgressionFormulas.ExpForResourceGeneration (numberOfResourcesGenerated));


        switch (buildingType)
        {
            case ResourceType.Wood:
                scoreManager.AddWood (numberOfResourcesGenerated);
                break;
            case ResourceType.Cloth:
                scoreManager.AddCloth (numberOfResourcesGenerated);
                break;
            case ResourceType.Tar:
                scoreManager.AddTar (numberOfResourcesGenerated);
                break;
            default:
                break;
        }

        Destroy (smokeCloud);
        Destroy (progressBar.gameObject);
        assignedWorker = null;
        isBeingWorked = false;
    }




    public void LevelUpBuildingGeneration ()
    {
        int cost = ProgressionFormulas.BuildingUpgradeCost (resourceGenerationLevel);
        if (cost > MasterManager.TimeAndScoreMan.Gold ||
            resourceGenerationLevel >= Constants.Instance.BuildingMaxLevel)
            return;

        MasterManager.TimeAndScoreMan.Gold -= cost;
        resourceGenerationLevel++;
    }

    public void LevelUpBuildingCapacity ()
    {
        int cost = ProgressionFormulas.BuildingUpgradeCost (resourceCapacityLevel);
        if (cost > MasterManager.TimeAndScoreMan.Gold ||
            resourceCapacityLevel >= Constants.Instance.BuildingMaxLevel)
            return;

        MasterManager.TimeAndScoreMan.Gold -= cost;

        resourceCapacityLevel++;
        int oldCapacity = ProgressionFormulas.ResourcesCapacityByLevel (resourceCapacityLevel - 1);
        int newCapacity = ProgressionFormulas.ResourcesCapacityByLevel (resourceCapacityLevel);


        switch (buildingType)
        {
            case ResourceType.Wood:
                scoreManager.maxWood += (newCapacity - oldCapacity);
                break;
            case ResourceType.Cloth:
                scoreManager.maxCloth += (newCapacity - oldCapacity);
                break;
            case ResourceType.Tar:
                scoreManager.maxTar += (newCapacity - oldCapacity);
                break;
            default:
                break;
        }

    }
}
