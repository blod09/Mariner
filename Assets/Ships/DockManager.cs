using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockManager : MonoBehaviour
{
    [SerializeField]
    private GameObject shipPrefab;
    [SerializeField]
    private Transform rootDockObject;
    [SerializeField]
    private Vector3 offsetFromDock;

    [SerializeField]
    private float spawnRate = 0.01f;

    private List<Dock> docks;

    private void Awake ()
    {
        docks = new List<Dock> ();
        foreach (Transform t in rootDockObject)
        {
            Dock d = new Dock ();
            d.isOccupied = false;
            d.location = t.transform;
            docks.Add (d);
        }


        SpawnShip (0);
        StartCoroutine (SpawnShips ());
    }

    private IEnumerator SpawnShips ()
    {
        while (true)
        {
            for (int i = 0; i < docks.Count; i++)
            {
                if (docks[i].isOccupied == false && UnityEngine.Random.Range (0.0f, 1.0f) < spawnRate)
                {
                    SpawnShip (i);
                    break;
                }
            }
            yield return null;
        }

    }


    private void SpawnShip (int dockIndex)
    {

        try
        {
            GameObject go = Instantiate (shipPrefab, docks[dockIndex].location.position + offsetFromDock, Quaternion.identity);
            go.transform.eulerAngles = Vector3.up * 90f;
            Ship ship = go.GetComponent<Ship> ();
            ship.parentDock = docks[dockIndex];
            ship.location = ShipLocation.Dock;
            ship.minutesToLeaveAtStart = UnityEngine.Random.Range (1, 4) * 60;
            ship.minutesToLeaveNow = ship.minutesToLeaveAtStart;
            docks[dockIndex].isOccupied = true;
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.LogError (e + " SpawnShip() trying to spawn on non-existent dock");
        }


    }
}

public class Dock
{
    public bool isOccupied;
    public Transform location;
}
