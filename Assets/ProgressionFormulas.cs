using UnityEngine;

public class ProgressionFormulas : MonoBehaviour
{


    // The bonus a worker gets to his speed base on his level
    // for example a worker with 100% bonus will finish a two hour job in one hour.

    public static float WorkerSpeed (int workerLevel)
    {
        return (Mathf.Pow (1.337f, workerLevel - 1));
    }

    public static int WorkerSalary (int workerLevel)
    {
        return (int)((workerLevel * Constants.Instance.WorkerBaseSalary) * 1.2f);
    }

    public static float GetShipSpawnRate ()
    {
        float spawnRate = Constants.Instance.ShipSpawnRate * ((float)DockManager.currentNumberOfDocks / ((float)(Ship.currentNumberOfShips + 1) * 2.0f));

        return spawnRate;
    }

    public static int CurerntDockCost ()
    {
        return (int)(Mathf.Pow ((DockManager.currentNumberOfDocks * 7.7f), 3) / 1.5f);
    }

    public static int CurrentPitCost ()
    {
        return (int)Mathf.Pow ((WorkingAreaManager.pitCount * 45.856f), 2) / 3;
    }

    public static int CurrentWorkerCost (int currentNumberOfWorkers)
    {
        return (int)Mathf.Pow ((currentNumberOfWorkers + 3), 3);
    }



    public static float ExpByShipFixTime (int shipFixTime)
    {
        return (float)shipFixTime * 0.09f;
    }

    public static float ExpForInspection ()
    {
        return (int)Random.Range (1, 4);
    }

    public static float ExpForResourceGeneration (int numOfResource)
    {
        return numOfResource * 0.8f;
    }

    public static int GetShipPenalty (int payment)
    {
        return payment / 2;
    }

    public static ShipTier GetRandomShipTier ()
    {
        ShipTier highestTier = HighestUnlockedShipTier ();

        for (int i = (int)highestTier; i >= 0; i--)
        {
            float _rand = Random.Range (0.0f, 1.0f);

            if (_rand <= 1.0f / ((float)i + 1.0f))
                return (ShipTier)i;
        }

        return ShipTier.Tier0;
    }



    private static ShipTier HighestUnlockedShipTier ()
    {
        if ((DockManager.currentNumberOfDocks > 4 && DockManager.totalNumberOfFixedShips >= 60) || DockManager.totalNumberOfFixedShips >= 100)
            return ShipTier.Tier4;
        if ((DockManager.currentNumberOfDocks > 3 && DockManager.totalNumberOfFixedShips >= 40) || DockManager.totalNumberOfFixedShips >= 60)
            return ShipTier.Tier3;
        if ((DockManager.currentNumberOfDocks > 2 && DockManager.totalNumberOfFixedShips >= 15) || DockManager.totalNumberOfFixedShips >= 30)
            return ShipTier.Tier2;
        if ((DockManager.currentNumberOfDocks > 1 && DockManager.totalNumberOfFixedShips >= 3) || DockManager.totalNumberOfFixedShips >= 10)
            return ShipTier.Tier1;

        return ShipTier.Tier0;
    }

    public static int ResourcesGeneratedByLevel (int level)
    {
        return (int)Mathf.Pow (Constants.Instance.ResourcesGeneratedAtLevelOne, (float)(level + 1.0f) / 1.2f);
    }

    public static int ResourcesCapacityByLevel (int level)
    {
        float lerpFactor = (1.0f / ((float)Constants.Instance.BuildingMaxLevel)) * ((float)level);

        return (int)Mathf.Pow (Constants.Instance.ResourceCapacityAtLevelOne, Mathf.Lerp (0.70f, 3.0f, lerpFactor));
    }

    public static int BuildingBuildCost (ResourceType type)
    {
        int cost = 0;
        int buildingCount = 0;

        switch (type)
        {
            case ResourceType.Wood:
                buildingCount = ResourceBuilding.lumbermillCount;
                break;
            case ResourceType.Cloth:
                buildingCount = ResourceBuilding.tailorCount;
                break;
            case ResourceType.Tar:
                buildingCount = ResourceBuilding.refineryCount;
                break;
            default:
                break;
        }

        for (int i = buildingCount + 1; i > 0; i--)
        {
            cost += 500 * i;
        }

        return cost;
    }

    public static int BuildingUpgradeCost (int level)
    {
        return level * 800;
    }
}
