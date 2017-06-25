using UnityEngine;

public class ProgressionFormulas : MonoBehaviour
{

    // The bonus a worker gets to his speed base on his level
    // for example a worker with 100% bonus will finish a two hour job in one hour.
    public static float WorkerSpeed (int workerLevel)
    {
        return (Mathf.Pow (1.337f, workerLevel - 1));
    }
}
