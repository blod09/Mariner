using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockManager : MonoBehaviour
{
    [SerializeField]
    private ShipTooltip tooltipRefference;


    [SerializeField]
    private GameObject shipPrefab;
    [SerializeField]
    private GameObject shadowDock;
    [SerializeField]
    private Vector3 offsetFromDock;

    private List<Dock> docks;
    private List<GameObject> dockGameObjects;

    private float spawnTimer;

    public static int currentNumberOfDocks
    {
        get; private set;
    }

    private void Start ()
    {
        InitializeDocks ();

        SpawnShip (0);
        StartCoroutine (SpawnShips ());
    }

    private void OnEnable ()
    {
        MasterManager.TimeAndScoreMan.oneMinuteTick += OnOneMinuteTick;
    }

    private void OnDisable ()
    {
        MasterManager.TimeAndScoreMan.oneMinuteTick -= OnOneMinuteTick;
    }

    private void OnOneMinuteTick ()
    {
        spawnTimer += 1.0f;
    }

    private IEnumerator SpawnShips ()
    {
        while (true)
        {
            if (UnityEngine.Random.Range (0.0f, 1.0f) < ProgressionFormulas.GetShipSpawnRate () * currentNumberOfDocks || spawnTimer > Constants.Instance.ShipSpawnPityTimer)
            {
                for (int i = 0; i < docks.Count; i++)
                {
                    if (docks[i].isOccupied == false)
                    {
                        SpawnShip (i);
                        spawnTimer = 0.0f;
                        break;
                    }
                }
            }
            yield return null;
        }

    }


    private void SpawnShip (int dockIndex)
    {

        try
        {
            GameObject go = Instantiate (shipPrefab, docks[dockIndex].gameObjectTransform.position + offsetFromDock, Quaternion.identity);

            go.transform.eulerAngles = Vector3.up * 90f;
            Ship ship = go.GetComponent<Ship> ();
            ship.stats = new ShipStats ("Red Bear", ShipTier.Tier2);
            ship.tooltip = tooltipRefference;
            ship.parentDock = docks[dockIndex];
            ship.location = ShipLocation.Dock;


            docks[dockIndex].isOccupied = true;

        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.LogError (e + " SpawnShip() trying to spawn on non-existent dock");
        }


    }

    public void AddDock ()
    {
        if (currentNumberOfDocks < dockGameObjects.Count)
        {
            GameObject go = dockGameObjects[currentNumberOfDocks];

            Dock d = new Dock ();
            d.isOccupied = false;
            d.gameObjectTransform = go.transform;
            go.SetActive (true);
            docks.Add (d);
            currentNumberOfDocks++;

        }
    }

    public void BuildDock ()
    {
        if (currentNumberOfDocks < dockGameObjects.Count && ProgressionFormulas.CurerntDockCost () < MasterManager.TimeAndScoreMan.Gold)
        {
            MasterManager.TimeAndScoreMan.Gold -= ProgressionFormulas.CurerntDockCost ();
            AddDock ();
            MoveShadowDock ();
        }
    }

    private void MoveShadowDock ()
    {
        if (currentNumberOfDocks < dockGameObjects.Count)
        {
            shadowDock.transform.position = dockGameObjects[currentNumberOfDocks].transform.position;
        }
        else
        {
            shadowDock.SetActive (false);
        }

    }

    private void InitializeDocks ()
    {
        docks = new List<Dock> ();
        dockGameObjects = new List<GameObject> ();

        foreach (Transform t in transform)
        {
            if (t.parent == this.gameObject.transform)
            {
                dockGameObjects.Add (t.gameObject);
                t.gameObject.SetActive (false);
            }
        }

        for (int i = 0; i < Constants.Instance.NumberOfDocksAtStart; i++)
        {
            AddDock ();
        }

        MoveShadowDock ();
    }
}

public class Dock
{
    public bool isOccupied;
    public Transform gameObjectTransform;
}
