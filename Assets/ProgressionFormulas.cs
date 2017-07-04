using UnityEngine;

public class ProgressionFormulas : MonoBehaviour
{

    // The bonus a worker gets to his speed base on his level
    // for example a worker with 100% bonus will finish a two hour job in one hour.
    public static float WorkerSpeed (int workerLevel)
    {
        return (Mathf.Pow (1.337f, workerLevel - 1));
    }

    public static float GetShipSpawnRate ()
    {
        return Constants.Instance.ShipSpawnRate;
    }

    public static int CurerntDockCost ()
    {
        return (int)Mathf.Pow ((DockManager.currentNumberOfDocks * 11.7f), 3) / 2;
    }

    public static int CurrentWorkerCost (int currentNumberOfWorkers)
    {
        return (int)Mathf.Pow ((currentNumberOfWorkers + 3), 3);
    }


    public static int CurrentPitCost ()
    {
        return (int)Mathf.Pow ((WorkingAreaManager.pitCount * 31.78f), 2) / 2;
    }

    //* 31.78f), 2);
}
