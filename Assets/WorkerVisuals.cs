using System;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerType
{
    RedBandana,
    BlueBandana,
    RedShirtBeardy,
    GreenShirBeardy,
    BlackShirtBaldy,
    WhiteShirtBaldy,
    ShaggyHair,
    WhitePornstar,
    BlackPornstar

}

[System.Serializable]
public class WorkerVisualData
{

    public Mesh mesh;
    public Material normalMat;
    public Material hoveredMat;
}

public class WorkerVisuals : MonoBehaviour
{
    [SerializeField]
    private List<WorkerVisualData> workersVisualData;
    [SerializeField]
    private RenderTexture[] cameraTextures;

    private static Dictionary<WorkerType, WorkerVisualData> workerTypeToVisuals;
    public static RenderTexture[] workerPortraits { get; private set; }

    private void Awake ()
    {
        workerTypeToVisuals = new Dictionary<WorkerType, WorkerVisualData> ();

        if (System.Enum.GetNames (typeof (WorkerType)).Length != workersVisualData.Count)
            Debug.LogError ("WorkerVisuals - Worker visuals list length does not mathch worker type enum count ");

        for (int i = 0; i < workersVisualData.Count; i++)
        {
            workerTypeToVisuals.Add ((WorkerType)i, workersVisualData[i]);
        }

        workerPortraits = cameraTextures;

        workersVisualData = null;
        cameraTextures = null;
        GC.Collect ();
    }

    public static WorkerVisualData GetVisuals (WorkerType workerType)
    {
        WorkerVisualData visuals;
        workerTypeToVisuals.TryGetValue (workerType, out visuals);

        if (visuals == null)
            throw new Exception ("WorkerVisualData GetVisuals() returned null");


        return visuals;
    }
}
