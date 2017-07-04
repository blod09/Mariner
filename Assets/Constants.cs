using System;
using UnityEngine;

public class Constants : MonoBehaviour
{
    [Header ("New Game Settings")]
    [SerializeField]
    private int startingGold;

    [SerializeField]
    private int numberOfWorkersAtStart;

    [SerializeField]
    private int workerBaseSalary;

    [SerializeField]
    private int numberOfDocksAtStart;

    [SerializeField]
    private int numberOfPitsAtStart;


    [Space]
    [Header ("Ship Spawn Settings")]

    [SerializeField]
    private float shipSpawnRate;
    [SerializeField]
    private float shipSpawnPityTimer;

    public int StartingGold { get { return startingGold; } private set { } }
    public int NumberOfWorkersAtStart { get { return numberOfWorkersAtStart; } private set { } }
    public int WorkerBaseSalary { get { return workerBaseSalary; } private set { } }
    public int NumberOfDocksAtStart { get { return numberOfDocksAtStart; } private set { } }
    public int NumberOfPitsAtStart { get { return numberOfPitsAtStart; } private set { } }
    public float ShipSpawnRate { get { return shipSpawnRate; } private set { } }
    public float ShipSpawnPityTimer { get { return shipSpawnPityTimer; } private set { } }


    // Singleton Stuff

    private static Constants instance;

    public static Constants Instance
    {
        get
        {
            return instance;
        }

        private set { instance = value; }
    }

    private void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception ("Singleton violation at Constants.Awake()");
        }
    }
}
