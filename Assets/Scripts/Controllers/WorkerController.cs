using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkerController : MonoBehaviour
{

    #region Editor Fields

    [SerializeField]
    private GameObject _workerPrefab;
    [SerializeField]
    private Material _normalMaterial;
    [SerializeField]
    private Material _hoveredMaterial;

    #endregion


    private GameObject _currentHoveredWorker;

    private GameObject _currentSelectedWorker;

    private Vector3 _workerLastLegalPosition;

    private Dictionary<GameObject, Worker> _gameobjectToDataMap;

    private WorkerTooltip _workerTooltip;

    public int TotalWorkerCost { get; private set; }

    private void Awake ()
    {
        _gameobjectToDataMap = new Dictionary<GameObject, Worker> ();
        _workerTooltip = FindObjectOfType (typeof (WorkerTooltip)) as WorkerTooltip;

        if (_workerTooltip == null)
            throw new Exception ("Error: Can't find the worker tooltip object.");
    }

    private void Start ()
    {

        for (int i = 0; i < 2; i++)
        {
            Worker worker = new Worker ("Ricardo" + (i + 1), new List<float> (), 5);
            TotalWorkerCost += worker.CostPerHour;
            GameObject go = Instantiate (_workerPrefab, new Vector3 (i * -20, 6, -30), Quaternion.identity);
            go.name = worker.Name;
            //go.transform.SetParent (this.transform);
            _gameobjectToDataMap.Add (go, worker);
        }

    }

    #region Action Subscription

    private void OnEnable ()
    {
        MouseManager.onHover += OnHover;
        MouseManager.onClick += OnClick;
        MouseManager.onDrag += OnDrag;
        MouseManager.onRelease += OnRelease;

    }

    private void OnDisable ()
    {
        MouseManager.onHover -= OnHover;
        MouseManager.onClick -= OnClick;
        MouseManager.onDrag -= OnDrag;
        MouseManager.onRelease -= OnRelease;

    }

    #endregion

    #region Mouse Event Handlers

    private void OnHover (GameObject go)
    {

        // If we already have a worker that is being hovered over and it's not this one, then reset the material.
        if (_currentHoveredWorker != null && go != _currentHoveredWorker)
        {
            _currentHoveredWorker.GetComponentInChildren<Renderer> ().material = _normalMaterial;
        }

        // If the object being hovered over is one of the workers this script controlls, change it's material to _hoveredMaterial.
        if (_gameobjectToDataMap.ContainsKey (go))
        {
            go.GetComponentInChildren<Renderer> ().material = _hoveredMaterial;
            _currentHoveredWorker = go;
        }
    }


    // Same as the hover function above, but instead of setting the material, we toggle the projector on and off.
    private void OnClick (GameObject go)
    {
        if (_currentSelectedWorker != null && go != _currentSelectedWorker)
        {
            _currentSelectedWorker.GetComponentInChildren<Projector> ().enabled = false;
            _currentSelectedWorker = null;
            _workerTooltip.Hide ();
        }

        if (_gameobjectToDataMap.ContainsKey (go))
        {
            go.GetComponentInChildren<Projector> ().enabled = true;
            _currentSelectedWorker = go;
            _workerTooltip.Show (_gameobjectToDataMap[go], go.transform);
            _workerLastLegalPosition = go.transform.position;
        }
    }

    private void OnDrag (GameObject go, Vector3 startPos, Vector3 endPos, GameObject ground)
    {
        if (_currentSelectedWorker != go)
            return;
        if (_gameobjectToDataMap[go].CurrentJob != null)
            return;

        endPos.y += 2.0f;

        // TODO: If user is trying to place worker in illegal position, give feedback!
        _currentSelectedWorker.transform.position = endPos;
        if (isWalkableGround (ground))
        {
            _workerLastLegalPosition = endPos;
        }
    }

    private void OnRelease (GameObject go, GameObject releaseArea)
    {
        if (go != _currentSelectedWorker)
            return;

        _currentSelectedWorker.transform.position = _workerLastLegalPosition;

        Job job = releaseArea.GetComponent<Job> ();
        if (job != null)
        {
            if (job.isBeingWorked)
                return;
            _gameobjectToDataMap[go].CurrentJob = job;
            job.isBeingWorked = true;
        }



    }

    #endregion

    #region Helper Functions

    private bool isWalkableGround (GameObject ground)
    {
        if (ground == null)
            return false;

        MapArea area = ground.GetComponent<MapArea> ();
        if (area == null)
            return false;
        if (area.Type != TerrainType.Walkable)
            return false;

        return true;
    }

    #endregion





}
